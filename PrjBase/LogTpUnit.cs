using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace eMachine
{
    public enum EN_MAT_TYPE : int
    {
        PLATE ,
        WAFER ,
        CHIP  ,
        MGZ   ,
        REEL  ,
		VISN  
    };

    public enum EN_CFG_ID : int
    {
        SETTING , //TK-IN시점에서 받아오는 PARAMETER등을 남길때 사용
        CHANGE  , //UI에서 값을 변경시 사용
        SAVE      //Recipe 파일이 저장될 경우 사용
    };


    public struct _TXfrTime {
      public String sDeviceID;
      public String sEventID ;
      public double dCurTime ;
      public double dMinTime ;
      public double dMaxTime ;
      public double dCurSet  ;

    } ;

    /***************************************************************************/
    /* Class: TLogTpUnit                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TLogTpUnit
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //const:   
        public const int MAX_PROC_TIME      = 200;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        String m_sLogPath  ;
        int    m_iWriteStep;
        String m_sPopBuf   ;

        int[]      m_iPrvLogTpAxe   = new int[2];
        String[]   m_sPrvLogTpEvent = new String[2];
        double[]  m_sPrvLogTpPosn   = new double[2];

        int[]      m_iPrvLogTpAct   = new int[2];
        String[]   m_sPrvLogTpActEvt= new String[2];
        double[]   m_sPrvLogTpActCmd= new double[2];


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //TQueueCls<LAMP_INFO> TestQue = new TQueueCls<LAMP_INFO>(5);
        Queue<String>       m_LogQue = new Queue<String>();
        _TXfrTime[]         XfrTime  = new _TXfrTime[MAX_PROC_TIME];

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLogTpUnit()
        {
            m_sLogPath = "MARS";
            Init();
        }
        ~TLogTpUnit() { }

        public void Init()                 
        {
            m_LogQue.Clear();

            ClsXFRTime();
            KillPast  ();
        }
        
        //Del Log.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        void KillPast()             //날짜 지난 Log 정리
        {
             FNC.DelDirFrDate(m_sLogPath   , DateTime.Now.AddDays( - 90));
        }

        //Get Function
        String GetMatType (EN_MAT_TYPE     iMatType )
        {
            String sMatType = "_";

            if(iMatType == EN_MAT_TYPE.PLATE) sMatType = "PLATE"   ;
            if(iMatType == EN_MAT_TYPE.WAFER) sMatType = "WAFER"   ;
            if(iMatType == EN_MAT_TYPE.CHIP ) sMatType = "CHIP"    ;
            if(iMatType == EN_MAT_TYPE.MGZ  ) sMatType = "Cassette";
            if(iMatType == EN_MAT_TYPE.REEL ) sMatType = "REEL"    ;

            return sMatType;
        }

        //Make Log.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool MakeLog(String sBuff) //Log 생성
        {
            //Local Var.
            string sPath = Application.StartupPath + "\\" + m_sLogPath;
            string sTemp;
            string sFile =   "[" + string.Format("{0:yyMMdd}", DateTime.Now)+ "]" + "Event.txt"; 
            //Make Dir.


            FNC.CreateDir(sPath);
            sPath = sPath + string.Format("\\{0:yy/MM/dd}"   ,DateTime.Now); FNC.CreateDir(sPath);
            sPath = sPath + string.Format("\\{0:yy/MM/dd_HH}",DateTime.Now) + ".txt"; 

        	String sDate     = string.Format("{0:yyyy'/'MM'/'dd}",DateTime.Now);
	        String sTime     = string.Format("{0:HH:mm:ss}"  ,DateTime.Now);

            //File Open.
            FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fp, Encoding.Default);
            sw.BaseStream.Seek(0, SeekOrigin.End);
	        sTemp  = sDate              + "\t";  //DATE
	        sTemp += sTime              + "\t";  //TIME
	        sTemp += sBuff              + "\r\n";//Header + Data

            sw.Write(sTemp);
            sw.Flush();
            sw.Close();

            return true;
        }

        //Make Data Format
        bool Make_Format  (String sData, ref String sWriteBuf)
        {
	        String sTemp1 = ""; 
            String sTemp2 = "";
            if(sWriteBuf != "" && sData != ""  ) sWriteBuf += "\t";
	        if(sData == ""     || sData == null) sData = "$";

	        for (int n = 0 ; n < sData.Length ; n++) {
		        sTemp1 = sData.Substring(n,1);
			         if (sTemp1 == "_") sTemp2 += "_";
		        else                    sTemp2 += sTemp1;
		        }


	        sTemp1 = string.Format("'{0}'" ,sTemp2  );
	        sWriteBuf += sTemp1;
            return true;
        }
        bool Make_Format  (String sKey , ref String sWriteBuf, String sVal)
        {
	        String sTemp1 = ""; 
            String sTemp2 = "";

            if(sKey == ""      || sKey == null ) sKey = "$";
            if(sWriteBuf != "" && sKey != ""   ) sWriteBuf += "\t";

	        for (int n = 0 ; n < sKey.Length ; n++) {
		        sTemp1 = sKey.Substring(n,1);
			         if (sTemp1 == "_") sTemp2 += "_";
		        else                    sTemp2 += sTemp1;
		        }


	        sTemp1 = string.Format("('{0}','{1}')" ,sTemp2, sVal);
	        sWriteBuf += sTemp1;
            return true;
        }
        bool Make_Format  (String sKey , ref String sWriteBuf, String sVal1, String sVal2, String sVal3)
        {

	        String sTemp1 = ""; 
            String sTemp2 = "";
            String sValDat = "";
            int    iValCnt = 0;

            if(sWriteBuf != "" && sKey != "") sWriteBuf += "\t";

	        for (int n = 0 ; n < sKey.Length ; n++) {
		        sTemp2 = sKey.Substring(n,1);
			         if (sTemp2 == "_") sTemp1 += "_";
		        else                    sTemp1 += sTemp2;
		        }

            if(sVal1 != "") { iValCnt ++; sTemp2 = sTemp2 + sVal1 + ","; }
            if(sVal2 != "") { iValCnt ++; sTemp2 = sTemp2 + sVal2 + ","; }
            if(sVal3 != "") { iValCnt ++; sTemp2 = sTemp2 + sVal3 + ","; }

            sTemp2  = sTemp2  .Substring(1, sTemp2.Length-1);
	        sValDat = string.Format("('{0}','[{1}, {2}]')",sTemp1 ,iValCnt, sTemp2);
            sWriteBuf += sValDat;
            return true;

        }


        void ClsXFRTime()
        {
            for(int i=0;i<MAX_PROC_TIME;i++)
            {
                XfrTime[i].sDeviceID = ""  ;
                XfrTime[i].sEventID  = ""  ;
                XfrTime[i].dCurTime  = 0   ;
                XfrTime[i].dMinTime  = 9999;
                XfrTime[i].dMaxTime  = 0   ;
                XfrTime[i].dCurSet   = 0   ;
            }
        }
        void SetXFRTime(String sDevID, String sEventID, String sStatus)
        {
            int   iXfrQty = 0;
            bool  bFind   = false;
            //DATA  갯수 찾기
            for(int i=0;i<MAX_PROC_TIME-1;i++)
            {
                if(XfrTime[i].sDeviceID != "") iXfrQty ++;
            }

            for(int i=0;i<iXfrQty;i++)
            {

                if(XfrTime[i].sDeviceID != sDevID || XfrTime[i].sEventID != sEventID) continue;
                bFind = true;
                if(sStatus == "START")  XfrTime[i].dCurSet = DateTime.Now.Ticks;
                else
                {//END 일경우
                 XfrTime[i].dCurTime = DateTime.Now.Ticks - XfrTime[i].dCurSet;
                 if(XfrTime[i].dMinTime> XfrTime[i].dCurTime) XfrTime[i].dMinTime = XfrTime[i].dCurTime;
                 if(XfrTime[i].dMaxTime< XfrTime[i].dCurTime) XfrTime[i].dMaxTime = XfrTime[i].dCurTime;
                }

            }

            if(!bFind)
            {
                XfrTime[iXfrQty].sDeviceID = sDevID;
                XfrTime[iXfrQty].sEventID  = sEventID;
                XfrTime[iXfrQty].dCurSet   = DateTime.Now.Ticks;
            }
        }


        //Update
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Update()
        {
            //Message Process..
            switch (m_iWriteStep) {
                case  0: if (m_LogQue.Count<=0) {
                             m_iWriteStep = 0;
                             break;
                             }
                         else {
                            m_sPopBuf = m_LogQue.Dequeue();
                             }
                         m_iWriteStep++;
                         break;
                case  1: if (!MakeLog(m_sPopBuf)) break;
                         m_iWriteStep ++;
                         break;
                case  2: m_sPopBuf = "";
                         m_iWriteStep = 0    ;
                         break;
                }
        }

        //Function	    [FNC]최소단위의 설비동작을 기록, 가장 중요함.
        //Transfer	    [XFR]사용자 정의된 Module 단위의 시작과 끝을 기록
        //Process	    [PRC]설비 동작 중 공정 구간에 해당하는 로그
        //Lot Event	    [LEH]Lot에 관련된 작업을 기록하는 로그
        //Alarm	        [ALM]설비에서 발생된 Error, Alarm을 기록하는 로그
        //Configuration	[CFG]설비 configuration의 변경점을 기록하는 로그

		public void Function(int iDeviceID, String sEventID , String sStatus   , String sMatID   , EN_MAT_TYPE iMatType ,  String sKey1 = "", String sVal1 = "", String sKey2 = "", String sVal2 = "", String sKey3 = "", String sVal3 = "")
        {//Function	    [FNC]최소단위의 설비동작을 기록, 가장 중요함.
            try
            {
                String sWriteBuf = "";
                String sDevID   = cDEF.POSN.GetPartName(iDeviceID).Trim();
                String sMatType = GetMatType (iMatType );

                Make_Format(sDevID    , ref sWriteBuf);  //DEVICE ID
                Make_Format("FNC"     , ref sWriteBuf);  //LOG TYPE
                Make_Format(sEventID  , ref sWriteBuf);  //EVENT ID
                Make_Format(sStatus   , ref sWriteBuf);  //STATUS
                Make_Format(sMatID    , ref sWriteBuf);  //Meterial ID
                Make_Format(sMatType  , ref sWriteBuf);  //Meterial Type
                Make_Format(sKey1     , ref sWriteBuf, sVal1);  //DATA1
                Make_Format(sKey2     , ref sWriteBuf, sVal2);  //DATA2
                Make_Format(sKey3     , ref sWriteBuf, sVal3);  //DATA3

                m_LogQue.Enqueue(sWriteBuf);
            }
            catch (Exception e)
            {
                cDEF.LOG.ExceptionTrace("LogThUnit-Function()", e);
            }
        }
		public void FunctionMove(int iPart, String sEvent, int iAxe, double dPosn)
        {
            if(sEvent           == ""    ) return;
            for(int i=0;i<m_iPrvLogTpAxe.Length;i++) {
                if(m_iPrvLogTpAxe  [i] == iAxe    &&
                   m_sPrvLogTpEvent[i] == sEvent  &&
                   m_sPrvLogTpPosn [i] == dPosn   ) return;
                }

            m_iPrvLogTpAxe  [1] = m_iPrvLogTpAxe  [0];
            m_sPrvLogTpEvent[1] = m_sPrvLogTpEvent[0];
            m_sPrvLogTpPosn [1] = m_sPrvLogTpPosn [0];
            m_iPrvLogTpAxe  [0] = iAxe  ;
            m_sPrvLogTpEvent[0] = sEvent;
            m_sPrvLogTpPosn [0] = dPosn ;

            String sPosnName = cDEF.MOTR[iAxe].m_sName + " " + cDEF.MOTR.GetPosnName(iPart, iAxe, dPosn);
            Function(iPart, sEvent, "$", "MOVE", EN_MAT_TYPE.CHIP, "TYPE", "MOTOR", sPosnName ,Convert.ToString(dPosn)); //

        }

		public void Transfer(int iDeviceID, String sEventID , String sStatus   , String sMatID   , EN_MAT_TYPE iMatType , String sFROM     , String sTO ,String sKey1 = "", String sVal1 = "", String sKey2 = "", String sVal2 = "", String sKey3 = "", String sVal3 = "")
        {//Transfer	    [XFR]사용자 정의된 Module 단위의 시작과 끝을 기록
            String sWriteBuf = "";
            String sDevID   = cDEF.POSN.GetPartName(iDeviceID).Trim();
            String sMatType = GetMatType (iMatType );

            Make_Format(sDevID   , ref sWriteBuf       );  //DEVICE ID
            Make_Format("XFR"    , ref sWriteBuf       );  //LOG TYPE
            Make_Format(sEventID , ref sWriteBuf       );  //EVENT ID
            Make_Format(sStatus  , ref sWriteBuf       );  //STATUS
            Make_Format(sMatID   , ref sWriteBuf       );  //Meterial ID
            Make_Format(sMatType , ref sWriteBuf       );  //Meterial Type
            Make_Format(sFROM    , ref sWriteBuf       );  //FROM
            Make_Format(sTO      , ref sWriteBuf       );  //TO
            Make_Format(sKey1    , ref sWriteBuf, sVal1);  //DATA1
            Make_Format(sKey2    , ref sWriteBuf, sVal2);  //DATA2
            Make_Format(sKey3    , ref sWriteBuf, sVal3);  //DATA3

            m_LogQue.Enqueue(sWriteBuf);


            SetXFRTime(sDevID, sEventID, sStatus);
        }		
        public void Process (int iDeviceID, String sEventID , String sStatus   , String sMatID   , String      sLotId   , String sRecipeId ,             String sKey1 = "", String sVal1 = "", String sKey2 = "", String sVal2 = "", String sKey3 = "", String sVal3 = "")
        {//Process	    [PRC]설비 동작 중 공정 구간에 해당하는 로그
            String sDevID   = cDEF.POSN.GetPartName(iDeviceID).Trim();
            String sWriteBuf = "";


            Make_Format(sDevID    , ref sWriteBuf       );  //DEVICE ID
            Make_Format("PRC"     , ref sWriteBuf       );  //LOG TYPE
            Make_Format(sEventID  , ref sWriteBuf       );  //EVENT ID
            Make_Format(sStatus   , ref sWriteBuf       );  //STATUS
            Make_Format(sMatID    , ref sWriteBuf       );  //Meterial ID
            Make_Format(sLotId    , ref sWriteBuf       );  //Lot ID
            Make_Format(sRecipeId , ref sWriteBuf       );  //Recipe ID
            Make_Format(sKey1     , ref sWriteBuf, sVal1);  //DATA1
            Make_Format(sKey2     , ref sWriteBuf, sVal2);  //DATA2
            Make_Format(sKey3     , ref sWriteBuf, sVal3);  //DATA3

            m_LogQue.Enqueue(sWriteBuf);
        }		
        public void LotEvent(int iDeviceID, String sEventID , String sLotId    , String sRecipeId, String      sCarrID  ,                                String sKey1 = "", String sVal1 = "", String sKey2 = "", String sVal2 = "", String sKey3 = "", String sVal3 = "")
        {//Lot Event	    [LEH]Lot에 관련된 작업을 기록하는 로그
            String sDevID   = cDEF.POSN.GetPartName(iDeviceID).Trim();
            String sWriteBuf = "";

            Make_Format(sDevID   , ref sWriteBuf       );  //DEVICE ID
            Make_Format("LEH"    , ref sWriteBuf       );  //LOG TYPE
            Make_Format(sEventID , ref sWriteBuf       );  //EVENT ID
            Make_Format(sLotId   , ref sWriteBuf       );  //Lot ID
            Make_Format(sRecipeId, ref sWriteBuf       );  //Recipe ID
            Make_Format(sCarrID  , ref sWriteBuf       );  //Carrier ID
            Make_Format(sKey1    , ref sWriteBuf ,sVal1);  //DATA1
            Make_Format(sKey2    , ref sWriteBuf ,sVal2);  //DATA2
            Make_Format(sKey3    , ref sWriteBuf ,sVal3);  //DATA3

            m_LogQue.Enqueue(sWriteBuf);

        }		
        public void Alarm   (int iDeviceID, String sEventID , String sAlarmCode, String sStatus  ,                                                       String sKey1 = "", String sVal1 = "", String sKey2 = "", String sVal2 = "", String sKey3 = "", String sVal3 = "")
        {//Alarm	        [ALM]설비에서 발생된 Error, Alarm을 기록하는 로그
            String sDevID   = cDEF.POSN.GetPartName(iDeviceID).Trim();
            String sWriteBuf = "";


            Make_Format(sDevID     , ref sWriteBuf       );  //DEVICE ID
            Make_Format("ALM"      , ref sWriteBuf       );  //LOG TYPE
            Make_Format(sEventID   , ref sWriteBuf       );  //EVENT ID
            Make_Format(sAlarmCode , ref sWriteBuf       );  //Alarm CODE
            Make_Format(sStatus    , ref sWriteBuf       );  //Status
            Make_Format(sKey1      , ref sWriteBuf, sVal1);  //DATA1
            Make_Format(sKey2      , ref sWriteBuf, sVal2);  //DATA2
            Make_Format(sKey3      , ref sWriteBuf, sVal3);  //DATA3

            m_LogQue.Enqueue(sWriteBuf);
        }		
        public void Config  (int iDeviceID, EN_CFG_ID  iConfigID,  String sKey1 = "", String sVal1 = "", String sKey2 = "", String sVal2 = "", String sKey3 = "", String sVal3 = "")
        {//Configuration	[CFG]설비 configuration의 변경점을 기록하는 로그
            String sDevID   = cDEF.POSN.GetPartName(iDeviceID).Trim();
            String sCfgID    = "";
            String sWriteBuf = "";


                 if(iConfigID == EN_CFG_ID.SETTING) sCfgID = "SETTING";
            else if(iConfigID == EN_CFG_ID.CHANGE ) sCfgID = "CHANGE" ;
            else if(iConfigID == EN_CFG_ID.SAVE   ) sCfgID = "SAVE"   ;


            Make_Format(sDevID  , ref sWriteBuf       );  //DEVICE ID
            Make_Format("CFG"   , ref sWriteBuf       );  //LOG TYPE
            Make_Format(sCfgID  , ref sWriteBuf       );  //Config ID  - Configuration 관련 작업 분류
            Make_Format(sKey1   , ref sWriteBuf, sVal1);  //DATA1
            Make_Format(sKey2   , ref sWriteBuf, sVal2);  //DATA2
            Make_Format(sKey3   , ref sWriteBuf, sVal3);  //DATA3

            m_LogQue.Enqueue(sWriteBuf);
        }		
        public void Config  (int iPosnID  ,  String sKey  = "", String sVal1 = "", String sVal2 = "", String sVal3 = "")
        {
	        //Local Var.
            String sWriteBuf = "";
            String sDevID    = "";


	        if(iPosnID == 0)  sDevID = "Common_Position";
	        if(iPosnID == 1)  sDevID = "Position";
	        if(iPosnID == 2)  sDevID = "Velocity";
	        if(iPosnID == 3)  sDevID = "JOB";
	        if(iPosnID == 4)  sDevID = "Setting";
	        if(iPosnID == 5)  sDevID = "DSTB";


            Make_Format(sDevID   , ref sWriteBuf      );  //DEVICE ID
            Make_Format("CFG"    , ref sWriteBuf      );  //LOG TYPE
            Make_Format("CHANGE" , ref sWriteBuf      );  //Config ID  - Configuration 관련 작업 분류
            Make_Format(sKey     , ref sWriteBuf, sVal1, sVal2, sVal3);  //DATA1

            m_LogQue.Enqueue(sWriteBuf);
        }




    }
}
