
using System;

namespace DBProc.SysConfig
{
    [AttributeUsage(AttributeTargets.Class)]
    public class cDBTableAttribute: Attribute
    {
        public string    Name { get; private set; }

        public cDBTableAttribute(string pName)
        {
            Name     = pName;
        }

        public static cDBTableAttribute GetAttribute(Type p)
        {
            return GetCustomAttribute(p,typeof(cDBTableAttribute)) as cDBTableAttribute;
        }
    }

}
