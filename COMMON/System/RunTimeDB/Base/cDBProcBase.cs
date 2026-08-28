using DBProc.SysConfig;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DBProc
{
    public class cDBProcBase<T> where T : new()
    {
        #region < Readonly Fields >
        protected readonly string TableName;
        protected string[] ColumnNames {get; private set;}
        protected IQueryProc   Query{get; set;}
        #endregion

        #region < Constructor >
        public cDBProcBase()
        {
            Type type = typeof(T);
            cDBTableAttribute tableAttr = cDBTableAttribute.GetAttribute(type);
            TableName = tableAttr.Name;
        }
        public cDBProcBase(string pTableName)
        {
            TableName = pTableName;
        } 
        #endregion

        #region < Private Mehtods >
        public virtual void Initialize(IQueryProc pQuery)
        {
            Query = pQuery;
            
            ColumnNames = pQuery.GetColumnNames(TableName);
            Type   t        = typeof(T);

            for (int i = 0; i < ColumnNames.Length; i++)
            {
                if (t.GetProperty(ColumnNames[i]) == null)
                {
                    if(MessageBox.Show(string.Format("{0} Type does not have a {1} variable name.\r\nYou wan`t drop the Column {1}", t.ToString(), ColumnNames[i]),
                        "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string query = $"ALTER TABLE {TableName} DROP COLUMN {ColumnNames[i]}";
                        ExecuteNonQuery(query);
                        //StringBuilder query = new StringBuilder($"CREATE TABLE {pTableName}(" );
                    }

                    System.Diagnostics.Debug.WriteLine(string.Format("{0} Type does not have a {1} variable name.", t.ToString(), ColumnNames[i]));
                }
            }

            PropertyInfo[] properties = t.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                if(ColumnNames.Any( p=>p == properties[i].Name) == false)
                {
                    if (MessageBox.Show($"There is no {properties[i].Name} column in the {TableName} table.\r\nYou wan`t add the Column {1}",
                        "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        PropertyInfo p = properties[i];

                        object[] attrs = p.GetCustomAttributes(true);

                        cDBFieldAttribute attr = null;

                        foreach (object a in attrs)
                        {
                            if (a is cDBFieldAttribute dbAttr)
                                attr = dbAttr;
                        }


                        string dataSize     = attr.DataSize == 0 ? "" : $"({attr.DataSize})";
                        string dataType     = $" {attr.DataType}{dataSize}";
                        string isNotNull    = attr.IsNotNull ? " NOT NULL" : "";
                        string defaultValue = attr.DefaultValue.Length > 0 ? $" DEFAULT {attr.DefaultValue}" : "";

                        string query = $"ALTER TABLE {TableName} add {properties[i].Name}{dataType}{isNotNull}{defaultValue}";
                        ExecuteNonQuery(query);
                    }

                }
            }
        }

        private T RowToData(DataRow pRow)
        {
            try
            {
                if (IsNullObject(pRow)) return default;

                T data = new T();
                Type type = typeof(T);

                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    PropertyInfo property = type.GetProperty(ColumnNames[i]);

                    if (property != null)
                    {
                        if(pRow.Table.Columns.Contains(ColumnNames[i]) == false) continue;
                        string strValue = pRow[ColumnNames[i]].ToString();

                        if (pRow[ColumnNames[i]].GetType() == typeof(DateTime))
                        {
                            DateTime time = Convert.ToDateTime(pRow[ColumnNames[i]]);
                           
                            if (time.Millisecond > 0) strValue = time.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                            else strValue = time.ToString("yyyy-MM-dd HH:mm:ss");

                            if (property.GetType() == typeof(DateTime))
                            {
                                property.SetValue(data, time);
                            }
                            else
                            {
                                object value = StringConvert(strValue, property.PropertyType);
                                property.SetValue(data, value);

                            }
                        }
                        else
                        {
                            object value = StringConvert(pRow[ColumnNames[i]].ToString(), property.PropertyType);
                            property.SetValue(data, value);
                        }
                        
                    }
                }

                return data;
            }
            catch (Exception) {return default; }
        } 

        #endregion

        #region < public Methods >
        public bool IsNullObject(object pObject)
        {
            if (pObject != null) return false;
            return true;
        } 

        public bool IsTableExist()
        {
            return Query.IsTableExist(TableName);
        }

        public bool ExecuteNonQuery(string pQuery)
        {
            int nResult = Query.ExecuteNonQuery(pQuery);
            return nResult > 0;
        }
        public T[] GetRows(string pWhere = null)
        {
            return GetRows(null, pWhere);
        }

        public T[] GetRows(string pSelectColNames, string pWhere = null , string pSorting = null)
        {
            try
            {
                
                string    strQuery  = string.Format("SELECT * FROM {0}", TableName);
                List<T>   lst       = new List<T>();


                if (pSelectColNames != null && pSelectColNames.Length > 0)
                {
                    strQuery = string.Format("SELECT {0} FROM {1}", pSelectColNames, TableName);
                }

                if (string.IsNullOrWhiteSpace(pWhere) == false)
                {
                    strQuery = string.Format("{0} WHERE {1}", strQuery, pWhere);
                }

                if (string.IsNullOrWhiteSpace(pSorting) == false)
                {
                    strQuery = string.Format("{0} {1}", strQuery, pSorting);
                }

                using (DataTable dTable = Query.ExecuteReader(strQuery))
                {
                    if (dTable != null)
                    {
                        foreach (DataRow row in dTable.Rows)
                        {
                            T data = RowToData(row);

                            lst.Add(data);
                        }
                    }
                   return lst.ToArray();
                }
            }
            catch (Exception)
            {
                return new T[0];
            }
        }

        public string GetInsertQuery(T pData)
        {
            string  strFields = "";
            string  strValues = "";
            Type    type      = typeof(T);

            for (int i = 0; i < ColumnNames.Length; i++)
            {
                strFields = string.Format("{0}{1},", strFields, ColumnNames[i]);
                PropertyInfo property = type.GetProperty(ColumnNames[i]);
                if (property != null)
                {
                    object value = property.GetValue(pData);
                    if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime time = Convert.ToDateTime(value);
                        if (time.Millisecond > 0) value = time.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                        else value = time.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    strValues = string.Format("{0}'{1}',", strValues, value);
                }
                else
                {
                }
            }

            strFields = strFields.Substring(0, strFields.Length - 1);
            strValues = strValues.Substring(0, strValues.Length - 1);
            string strQuery = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", TableName, strFields, strValues);
            return strQuery;
        }

        public bool InsertRow(T pData)
        {
            string strQuery      = GetInsertQuery(pData);

            int nResult = Query.ExecuteNonQuery(strQuery);

            return nResult > 0;
        }

        public bool UpdateRows(T pData,string pFormat, params object[] pArgs)
        {
            return UpdateRows(pData, string.Format(pFormat,pArgs));
        }

        public bool UpdateRows(T pData,string pWhere = null)
        {
            return UpdateRows(pData, ColumnNames, pWhere);
        }

        public bool UpdateRows(T pData,string[] pUpdateCol, string pWhere = null)
        {
            int     nResult = 0;
            string  strSets = "";
            Type    type    = typeof(T);

            for (int i = 0; i < pUpdateCol.Length; i++)
            {
                PropertyInfo property = type.GetProperty(pUpdateCol[i]);
                if (property != null)
                {
                    object value = property.GetValue(pData);
                    if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime time = Convert.ToDateTime(value);
                        if (time.Millisecond > 0) value = time.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                        else value = time.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    strSets = string.Format("{0}{1} = '{2}',", strSets, pUpdateCol[i], value);
                }

            }

            strSets  = strSets.Substring(0, strSets.Length - 1);
            string strQuery = string.Format("UPDATE {0} SET {1}", TableName, strSets);

            if (string.IsNullOrWhiteSpace(pWhere) == false)
            {
                strQuery = string.Format("{0} WHERE {1}", strQuery, pWhere);
            }

            for (int i = 0; i < 5; i++)
            {
                nResult = Query.ExecuteNonQuery(strQuery);
                if (nResult > 0) break;
                else
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
            return nResult > 0;
        }
        #endregion

        #region < Methods >

        public bool DeleteRows(string pWhere = null)
        {
            if (GetRowCount(pWhere) <= 0) return true;
            
            string strQuery = string.Format("DELETE FROM {0}", TableName);

            if (string.IsNullOrWhiteSpace(pWhere) == false)
            {
                strQuery = string.Format("{0} WHERE {1}", strQuery, pWhere);
            }

            int nResult = Query.ExecuteNonQuery(strQuery);
            return nResult >= 0;
        }

        public bool WirteLog(string pLogFilePath, string pWhere = null)
        {
            try
            {
                string dirLocation = pLogFilePath.Substring(0, pLogFilePath.LastIndexOf('\\'));
                if (Directory.Exists(dirLocation) == false) Directory.CreateDirectory(dirLocation);
                T[] rows = GetRows(pWhere);
                object value = null;
                string buffer = "";
                Type type = typeof(T);
                PropertyInfo property = null;
                StreamWriter writer = new StreamWriter(pLogFilePath, false);

                try
                {
                    for (int i = 0; i < ColumnNames.Length; i++)
                    {
                        buffer = string.Format("{0}{1}\t", buffer, ColumnNames[i]);
                    }
                    writer.WriteLine(buffer);
                    writer.Flush();

                    if (rows != null)
                    {
                        for (int idxRow = 0; idxRow < rows.Length; idxRow++)
                        {
                            T rowData = rows[idxRow];
                            buffer = "";

                            for (int idxCol = 0; idxCol < ColumnNames.Length; idxCol++)
                            {
                                property = type.GetProperty(ColumnNames[idxCol]);
                                if (property != null)
                                {
                                    value = property.GetValue(rowData);
                                    buffer = string.Format("{0}{1}\t", buffer, value);
                                }
                            }

                            writer.WriteLine(buffer);
                            writer.Flush();
                        }
                        return true;
                    }
                }
                finally
                {
                    writer.Dispose();
                }
            }
            catch { return false; }
            return true;
        }

        public bool DeleteRowAfterWriteLog(string pLogFilePath, string pWhere = null)
        {
            try
            {
                if (File.Exists(pLogFilePath) && GetRowCount() == 0) return true;
                if (WirteLog(pLogFilePath, pWhere))
                {
                    if (DeleteRows(pWhere)) return true;
                }
            }
            catch { return false; }
            return true;
        }

        public int GetRowCount(string pFormat, params object[] pArgs)
        {
            return GetRowCount(string.Format(pFormat,pArgs));
        }

        public int GetRowCount(string pWhere = null)
        {
            try
            {
                object result = 0;
                string strQuery = string.Format("SELECT COUNT(*) FROM {0}", TableName);

                if (string.IsNullOrWhiteSpace(pWhere) == false)
                {
                    strQuery = string.Format("{0} WHERE {1}", strQuery, pWhere);
                }

                result = Query.ExecuteScalar(strQuery);
                if (result != null)
                    return Convert.ToInt32(result);

                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public object GetInstanceOnlyOne(string ColunmName, string pWhere = null)
        {
            try
            {
                if (GetRowCount(pWhere) != 0) return null;

                object result = 0;
                string strQuery = string.Format("SELECT {0} FROM {1}", ColunmName, TableName);

                if (string.IsNullOrWhiteSpace(pWhere) == false)
                {
                    strQuery = string.Format("{0} WHERE {1}", strQuery, pWhere);
                }

                result = Query.ExecuteScalar(strQuery);

                if (result != null)
                    return result;

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public object StringConvert(string pValue, Type pType)
        {
            try
            {
                object typeValue = null;

                if (pType == typeof(Int16))
                {
                    if (Int16.TryParse(pValue, out short bufferInt16))
                        typeValue = bufferInt16;
                }
                else if (pType == typeof(UInt16))
                {
                    if (UInt16.TryParse(pValue, out ushort bufferUInt16))
                        typeValue = bufferUInt16;
                }
                else if (pType == typeof(int))
                {
                    if (Int32.TryParse(pValue, out int bufferInt32))
                        typeValue = bufferInt32;
                }
                else if (pType == typeof(uint))
                {
                    if (UInt32.TryParse(pValue, out uint bufferUInt32))
                        typeValue = bufferUInt32;
                }
                else if (pType == typeof(long))
                {
                    if (Int64.TryParse(pValue, out long bufferInt64))
                        typeValue = bufferInt64;
                }
                else if (pType == typeof(ulong))
                {
                    if (ulong.TryParse(pValue, out ulong bufferUInt64))
                        typeValue = bufferUInt64;
                }
                else if (pType == typeof(float))
                {
                    if (Single.TryParse(pValue, out float bufferSingle))
                        typeValue = bufferSingle;
                }
                else if (pType == typeof(byte))
                {
                    if (Byte.TryParse(pValue, out byte bufferByte))
                        typeValue = bufferByte;
                }
                else if (pType == typeof(DateTime))
                {
                    if (DateTime.TryParse(pValue, out DateTime dt))
                        typeValue = dt;
                }
                else if (pType == typeof(bool))
                {
                    if (bool.TryParse(pValue, out bool bufferBool))
                        typeValue = bufferBool;
                }
                else if (pType == typeof(string))
                {
                    typeValue = pValue;
                }
                else
                {
                }
                return typeValue;
            }
            catch (Exception) { throw; }
        }
    }
}
