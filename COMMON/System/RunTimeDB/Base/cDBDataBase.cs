using DBProc.SysConfig;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DBProc
{
    public class cDBDataBase
    {
        public virtual bool Compare(cDBDataBase pData)
        {
            Type           type       = GetType();
            PropertyInfo[] properties = type.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                object thisValue   = properties[i].GetValue(this);
                object targetValue = properties[i].GetValue(pData);
                if (thisValue?.Equals(targetValue) == false)  return false;
            }

            return true;
        }

        public object Clone()
        {
            Type            type        = GetType();
            object          result      = Activator.CreateInstance(type);
            PropertyInfo[]  properties  = type.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                object thisValue = properties[i].GetValue(this);
                properties[i].SetValue(result,thisValue);
            }
            
            return result;
        }

        public string GetCreateQuery()
        {
            Type type = GetType();
            cDBTableAttribute tableAttr = cDBTableAttribute.GetAttribute(type);
            return GetCreateQuery(tableAttr.Name);
        }

        public string GetCreateQuery(string pTableName)
        {
            StringBuilder query = new StringBuilder($"CREATE TABLE {pTableName}(" );
            Type type = GetType();
            PropertyInfo[] properties = type.GetProperties();

            List<string> primaryKeys = new List<string>();

            for(int i=0; i< properties.Length; i++)
            {
                PropertyInfo p = properties[i];

                object[] attrs = p.GetCustomAttributes(true);

                cDBFieldAttribute attr = null;

                foreach (object a in attrs)
                {
                    if (a is cDBFieldAttribute dbAttr) attr = dbAttr;
                }

                
                if (attr.IsPrimaryKey) primaryKeys.Add(p.Name);
                    
                string dataSize     = attr.DataSize == 0? "":$"({attr.DataSize})";
                string dataType     = $" {attr.DataType}{dataSize}";
                string isNotNull    = attr.IsNotNull? " NOT NULL":"";
                string defaultValue = attr.DefaultValue.Length > 0? $" DEFAULT {attr.DefaultValue}":"";
                
                query.Append($"{p.Name}{dataType}{isNotNull}{defaultValue}");

                if (i < properties.Length - 1)
                    query.Append(", ");
                else
                {
                    if(primaryKeys.Count > 0)
                    {
                         query.Append($", PRIMARY KEY ({string.Join(",",primaryKeys)})");
                    }
                }

            }

            query.Append(")");

            return query.ToString();
        }
    }
}
