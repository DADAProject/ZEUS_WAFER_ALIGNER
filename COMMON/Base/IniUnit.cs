using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


    /***************************************************************************/
    /* Class: TIniUnit                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    class TIniUnit
    {
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int size, String filePath);  
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(String section, String key, String val, String filePath);


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TIniUnit()
        {
        }
        ~TIniUnit() { }
        //--------------------------------------------------------------------------
        public void Load(string FilePath, string Section, string Key, out int Value)
        {
            var temp = new StringBuilder(255);
            string retVal;
            GetPrivateProfileString(Section.Trim(), Key.Trim(), "0", temp, 255, FilePath);
            retVal = temp.ToString().Trim();
            if(retVal == "") retVal = "0";
            Value = Convert.ToInt32(retVal);  
        }
        //--------------------------------------------------------------------------
        public void Save(string FilePath, string Section, string Key, int Value)
        {
            string wVal = Convert.ToString(Value);
            WritePrivateProfileString(Section.Trim(), Key.Trim(), wVal, FilePath);
        }

        //--------------------------------------------------------------------------
        public void Load(string FilePath, string Section, string Key, out uint Value)
        {
            var temp = new StringBuilder(255);
            string retVal;
            GetPrivateProfileString(Section.Trim(), Key.Trim(), "0", temp, 255, FilePath);
            retVal = temp.ToString().Trim();
            if(retVal == "") retVal = "0";
            Value = Convert.ToUInt32(retVal);  
        }
        //--------------------------------------------------------------------------
        public void Save(string FilePath, string Section, string Key, uint Value)
        {
            string wVal = Convert.ToString(Value);
            WritePrivateProfileString(Section.Trim(), Key.Trim(), wVal, FilePath);
        }
        //--------------------------------------------------------------------------
        public void Load(string FilePath, string Section, string Key, out short Value)
        {
            var temp = new StringBuilder(255);
            string retVal;
            GetPrivateProfileString(Section.Trim(), Key.Trim(), "0", temp, 255, FilePath);
            retVal = temp.ToString().Trim();
            if(retVal == "") retVal = "0";
            Value = Convert.ToInt16(retVal);  
        }
        //--------------------------------------------------------------------------
        public void Save(string FilePath, string Section, string Key, short Value)
        {
            string wVal = Convert.ToString(Value);
            WritePrivateProfileString(Section.Trim(), Key.Trim(), wVal, FilePath);
        }
        //--------------------------------------------------------------------------
        public void Load(string FilePath, string Section, string Key, out double Value)
        {
            var temp = new StringBuilder(255);
            string retVal;
            GetPrivateProfileString(Section.Trim(), Key.Trim(), "0.0", temp, 255, FilePath);
            retVal = temp.ToString().Trim();
            if(retVal == "") retVal = "0.0";
            Value = Convert.ToDouble(retVal); 
        }
        //--------------------------------------------------------------------------
        public void Save(string FilePath, string Section, string Key, double Value)
        {
            string wVal = Convert.ToString(Value);
            WritePrivateProfileString(Section.Trim(), Key.Trim(), wVal, FilePath);
        }
        //--------------------------------------------------------------------------

        public void Load(string FilePath, string Section, string Key, out string Value)
        {
            var temp = new StringBuilder(255);
            string retVal;
            GetPrivateProfileString(Section.Trim(), Key.Trim(), string.Empty, temp, 255, FilePath);
            retVal = temp.ToString().Trim();
            Value = retVal;
            
        }
        //--------------------------------------------------------------------------
        public void Save(string FilePath, string Section, string Key, string Value)
        {
            string wVal = Convert.ToString(Value);
            WritePrivateProfileString(Section.Trim(), Key.Trim(), wVal, FilePath);
        }
        //--------------------------------------------------------------------------
        public void Load(string FilePath, string Section, string Key, out bool Value)
        {
            var temp = new StringBuilder(255);
            string retVal;
            GetPrivateProfileString(Section.Trim(), Key.Trim(), "false", temp, 255, FilePath);
            retVal = temp.ToString().Trim();
            if(retVal == "1") retVal = "True" ;
            if(retVal == "0") retVal = "false";
            if(retVal == "" ) retVal = "false";
            Value = Convert.ToBoolean(retVal); 
        }
        //--------------------------------------------------------------------------
        public void Save(string FilePath, string Section, string Key, bool Value)
        {
            string wVal = Convert.ToString((Value) ? "1" : "0");
            WritePrivateProfileString(Section.Trim(), Key.Trim(), wVal, FilePath);
        }
		public string   GetLineStringFrINI     (string sData)
        {//List Box에 개행 (\n) 사용시 처리 
            String sNewStr = "";
            sNewStr = sData.Replace("*","\n");
            return sNewStr;
        }
		public string   SetLineStringToINI       (string sData)
        {//List Box에 개행 (\n) 사용시 처리
            String sNewStr = "";
            sNewStr = sData.Replace("\n","*");
            return sNewStr;

        }
    }

