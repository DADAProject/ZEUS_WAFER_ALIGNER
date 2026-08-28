using System;

namespace DBProc.SysConfig
{
    [AttributeUsage(AttributeTargets.Property)]
    public class cDBFieldAttribute: Attribute
    {
        public enum eDataType {TEXT,INTEGER, STRING, BOOLEAN }

        public bool      IsPrimaryKey { get; private set; }
        public bool      IsNotNull    { get; private set; }
        public string    DefaultValue { get; private set; }
        public eDataType DataType     { get; private set; }
        public int       DataSize     { get; private set; }

        public cDBFieldAttribute(eDataType pDataType, int pDataSize, bool pIsPrimaryKey, bool pIsNotNull, string pDefaultValue)
        {
            DataType     = pDataType;
            DataSize     = pDataSize;
            IsPrimaryKey = pIsPrimaryKey;
            IsNotNull    = pIsNotNull;
            DefaultValue = pDefaultValue;
        }


        public static cDBFieldAttribute GetAttribute(Type p)
        {
            return GetAttribute(p);
        }
    }

}
