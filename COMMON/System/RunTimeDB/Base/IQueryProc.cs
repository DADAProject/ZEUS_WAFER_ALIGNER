using System.Data;
using System.Data.Common;

namespace DBProc
{
    public interface IQueryProc
    {
        bool        Open           ();
        bool        Close          ();
        int         ExecuteNonQuery(string pQuery, DbConnection pCon);
        int         ExecuteNonQuery(string pQuery);
        int         ExecuteNonQuery(string pQuery, params object[] pArg);
        object      ExecuteScalar  (string pQuery, DbConnection pCon);
        object      ExecuteScalar  (string pQuery);
        object      ExecuteScalar  (string pQuery, params object[] pArg);
        DataTable   ExecuteReader  (string pQuery, DbConnection pCon);
        DataTable   ExecuteReader  (string pQuery);
        DataTable   ExecuteReader  (string pQuery, params object[] pArg);
        string[]    GetColumnNames (string pTableName);
        bool        IsTableExist   (string pTableName);

    }
}
