using DBProc.SysConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace DBProc
{
    public class cSQLiteQueryProc : IQueryProc
    {
        public delegate void ExceptionEventHandler(string pQuery, Exception pEx);

        public static event ExceptionEventHandler ExceptionEvent;

        #region < Fields >
        private static readonly string mPASSWORD       = null;
        private bool                   mKeepConnection = false;
        private string                 mConStr;
        private SQLiteConnection       mConnection;
        #endregion

        #region < Properties >
        public string FilePath     { get; private set; }
        
        #endregion

        #region < Constructors >

        public cSQLiteQueryProc(string pFilePath,bool pIsKeepConnection)
        {
            Initialize(pFilePath,pIsKeepConnection);
        }

        #endregion

        #region < Methods >

        public void Initialize(string pFilePath,bool pIsKeepConnection)
        {
            if(mPASSWORD != null)
            {
                mConStr = string.Format(@"Data Source= {0};Password={1} ", pFilePath, mPASSWORD);
            }
            else
            {
                mConStr = string.Format($@"Data Source= {pFilePath}");
            }
            mKeepConnection = pIsKeepConnection;
            mConnection = new SQLiteConnection(mConStr);
        }

        #region # Open Close #
        public bool Open()
        {
            if (mKeepConnection == false) return false;
            else
            {
                if (Close())
                {
                    mConnection = new SQLiteConnection(mConStr);
                    mConnection.Open();
                    return true;
                }
                else return false;
            }
        }
        public bool Close()
        {
            if (mKeepConnection == false) return false;
            else
            {
                if (mConnection != null)
                {
                    mConnection.Dispose();
                    mConnection = null;
                }
                return true;
            }

        } 
        #endregion

        #region # ExecuteNonQuery #
        public int ExecuteNonQuery(string pQuery, DbConnection pCon)
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    if(pCon.State == ConnectionState.Closed) pCon.Open();
                    cmd.Connection = pCon as SQLiteConnection;
                    cmd.CommandText = pQuery;
                    cmd.CommandTimeout = 500;
                    int nResult = cmd.ExecuteNonQuery();

                    return nResult;
                }
            }
            catch (Exception ex) 
            {
                ExceptionEvent?.Invoke(pQuery, ex);
                return -1; 
            }
        }

        public int ExecuteNonQuery(string pQuery)
        {

            if (mKeepConnection) return ExecuteNonQuery(pQuery, mConnection);
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(mConStr))
                {
                    return ExecuteNonQuery(pQuery, conn);
                }
            }
        }

        public int ExecuteNonQuery(string pQuery, params object[] pArg)
        {
            try
            {
                return ExecuteNonQuery(string.Format(pQuery, pArg));
            }
            catch (Exception ex) { throw (ex); }
        } 
        #endregion

        #region # ExecuteScalar #
        public object ExecuteScalar(string pQuery, DbConnection pCon)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(mConStr))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        if (pCon.State == ConnectionState.Closed) pCon.Open();
                        cmd.Connection = pCon as SQLiteConnection;
                        cmd.CommandText = pQuery;
                        object objRet = cmd.ExecuteScalar();

                        return objRet;
                    }
                }
            }
            catch (Exception ex) 
            {
                ExceptionEvent?.Invoke(pQuery, ex);
                return null; 
            }
        }

        public object ExecuteScalar(string pQuery)
        {
            if (mKeepConnection) return ExecuteScalar(pQuery, mConnection);
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(mConStr))
                {
                    return ExecuteScalar(pQuery, conn);
                }
            }
        }

        public object ExecuteScalar(string pQuery, params object[] pArg)
        {
            try
            {
                return ExecuteScalar(string.Format(pQuery, pArg));
            }
            catch (Exception ex) { throw (ex); }
        } 
        #endregion

        #region # ExecuteReader #
        public DataTable ExecuteReader(string pQuery, DbConnection pCon)
        {
            try
            {

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(pQuery, pCon as SQLiteConnection))
                {
                    using (DataSet ds = new DataSet())
                    {
                        if (pCon.State == ConnectionState.Closed) pCon.Open();

                        DataTable dt = new DataTable();
                        adapter.Fill(ds);
                        dt = ds.Tables[0];

                        return dt;
                    }
                }
            }
            catch (Exception ex) 
            {
                ExceptionEvent?.Invoke(pQuery, ex);
                return null; 
            }

        }

        public DataTable ExecuteReader(string pQuery)
        {

            if (mKeepConnection) return ExecuteReader(pQuery, mConnection);
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(mConStr))
                {
                    return ExecuteReader(pQuery, conn);
                }
            }
        }

        public DataTable ExecuteReader(string pQuery, params object[] pArg)
        {
            try
            {
                return ExecuteReader(string.Format(pQuery, pArg));
            }
            catch (Exception ex) { throw (ex); }
        } 
        #endregion

        public string[] GetColumnNames(string pTableName)
        {
           DataTable dt = ExecuteReader(string.Format("PRAGMA table_info('{0}')",pTableName));

           List<string> lst = new List<string>();

           foreach(DataRow row in dt.Rows)
           {
               lst.Add(row["name"].ToString());
           }
            return lst.ToArray();
        }

        public bool IsTableExist(string pTableName)
        {
                    try
            {
                string strQuery   = string.Format("SELECT count(*) FROM sqlite_master WHERE tbl_name = '{0}' AND type = 'table'", pTableName);
                object result     = null;
                int    existCount = 0;
                result = ExecuteScalar(strQuery);

                if (result != null) existCount = Convert.ToInt32(result);

                return (existCount > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public static bool CreateDefault(string pFilePath)
        {
            SQLiteConnection.CreateFile(pFilePath);

            if (File.Exists(pFilePath))
            {
                string conStr = string.Format(@"Data Source= {0}", pFilePath);

                using (SQLiteConnection connection = new SQLiteConnection(conStr))
                {
                    if(mPASSWORD != null)
                    {
                        connection.SetPassword(mPASSWORD);
                        connection.ChangePassword(mPASSWORD); 
                    }
                    connection.Open();

                    List<string> sqlList = new List<string>
                    {
                        new cDBProductionProc.cData().GetCreateQuery(),
                        new cDBRunTimeProc   .cData().GetCreateQuery()
                        
                    };

                    foreach (string sql in sqlList)
                    {
                        using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                        {
                            int ret = command.ExecuteNonQuery();
                            if (ret < 0)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

    }
}
