using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EZGemPlusCS;
using System.IO;
using System.Runtime.InteropServices;



namespace InoModule.Common.Interface
{

    public enum EN_CONTROL_STATE: int
    {
        UNKOWN         = 0,
        EQ_OFFLINE     = 1,
        ATTEMPT_ONLINE = 2,
        HOST_OFFLINE   = 3,
        ONLINE_LOCAL   = 4,
        ONLINE_REMOTE  = 5,
    }
    public enum EN_COMM_STATE: int
    {
        DISABLE         = 1,
        ENABLE_NOT_COMM = 2,
        ENABLE_CUMM     = 3,
    }
    public enum EN_CONNECT_STATE: int
    {
        NOT_CONNECT          = 0,
        CONNECT              = 1,
        COMMUNICATING        = 2,
    }
    public enum EN_PROC_STATE: int
    {
        INIT      = 1,
        SETUP     = 2,
        READY     = 3,
        EXECUTING = 4,
        PAUSE     = 5,
        ERROR     = 6,
        WAIT_LOT  = 7,
    }
    public class ECID
    {
        //ECID
        public const int PORT              = 1;
        public const int DEVICEID          = 2;
        public const int T3                = 3;
        public const int T5                = 5;
        public const int T6                = 6;
        public const int T7                = 7;
        public const int T8                = 8;
        public const int LINKTEST          = 9;
        public const int ESTABLISH_TIMEOUT = 10;
        public const int SIGNAL_TOWER_COLOR_CONTROL = 41010;
    }



    public class TGemNvia
    {
        public struct ECIDValue
        {
            ///////////////// HSMS PARAM (ECID VARIABLES) ////////////////////////////////
            public uint   m_nPort        ;
            public uint   m_nDeviceID    ;
            public uint   m_nT3          ;
            public uint   m_nT5          ;
            public uint   m_nT6          ;
            public uint   m_nT7          ;
            public uint   m_nT8          ;
            public uint   m_nLinkInterval;
            public uint   m_nCommReqeustTimeout;
            public bool   m_bPassive     ;
            public string m_sModelName   ;
            public string m_sSoftRev     ;
            public uint   m_nRetry       ;       
            public uint   m_nCTTime      ;       
            public uint   m_nDefaultCommState;   
            public uint   m_nDefaultControlState;
            public uint   m_nIdleTime    ;       
            public string m_strMODE      ;       
            public string m_strPASSIVE   ;       
            public string m_strIP        ;
            public uint   m_nTimeFormat  ;

            ////////////////////////////////////////////////////////////////////////////////////
        }

        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public const int    MAX_CEID                    = 200       ;
        public const int    MAX_SVID                    = 200       ;
        public const int    MAX_ECID                    = 100       ;
        public const int    MAX_ALID                    = 2000      ;
        public const int    MAX_RCMD                    = 50        ;


       
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   
        bool   m_bShowMsg;
        string m_sLastMsg;

        //Control State


        //public:   
        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        String       m_sRCMD               ;
        String[]     m_sRCMD_CPName  = new String[MAX_RCMD];
        String[]     m_sRCMD_CPVal   = new String[MAX_RCMD];
        int          m_iRCMD_CPCount       ;


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Control State
        long              m_nCommState       = (int)EN_COMM_STATE   .DISABLE      ;
        long              m_nControlState    = (int)EN_CONTROL_STATE.EQ_OFFLINE   ;
        long              m_nGemState        = (int)EN_CONNECT_STATE.NOT_CONNECT  ;

        long              m_nProcessState      ;
        bool              m_bSecsInit          ;
        bool              m_bSecsStart         ;
        long              m_nPrvCtrlState      ;
        long              m_nPrvProcessState   ;
        bool              m_bControlStatChange ;
        bool              m_bProcessStatChange ;

        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\

        public long      _nControlState         {get { return m_nControlState;    } }
        public long      _nCommState            {get { return m_nCommState;       } }
        public long      _nGemState             {get { return m_nGemState;        } }
        public long      _nProcessState         {get { return m_nProcessState;    } }
        public bool      _bControlStatChange    {get { return m_bControlStatChange;    } }
        public bool      _bProcessStatChange    {get { return m_bProcessStatChange;    } }


        public long      _nPrvCtrlState    {get { return m_nPrvCtrlState;    } set { m_nPrvCtrlState = value ;     }}
        public long      _nPrvProcessState {get { return m_nPrvProcessState; } set { m_nPrvProcessState = value ;     }}

        public bool      _bShowMsg         {get { return m_bShowMsg  ;      } set { m_bShowMsg = value ;     }}
        public String    _sLastMsg         {get { return m_sLastMsg  ;      } }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
        public CEZGemPlusLib m_gem    = new CEZGemPlusLib(); // dll 참조
        public ECIDValue     ECV      = new ECIDValue    ();
        public FileInfo      fileinfo = new FileInfo     (Application.ExecutablePath);
        public List<string> m_listPPID; // 테스트를 위하여 가지고 있는 레시피 리스트

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TGemNvia()
        {
            Init();
        }
        ~TGemNvia() { }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Basic Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(    // GetIniValue 를 위해
            String  section      ,
            String  key          ,
            String  def          ,
            StringBuilder retVal ,
            int    size          ,
            String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(  // SetIniValue를 위해
            String section,
            String key    ,
            String val    ,
            String filePath);

        public String GetIniValue(String Section, String Key, String iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return temp.ToString();
        }
        // INI 값 설정
        public void SetIniValue(String Section, String Key, String Value, String iniPath)
        {
            WritePrivateProfileString(Section, Key, Value, iniPath);
        }

        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public string StrCommState(long nState = -1)
        {
            if(nState == -1)  nState = m_nCommState;

             return Enum.GetName(typeof(EN_COMM_STATE),nState);   
        }

        public string StrControlState(long nState = -1)
        {

            if(nState == -1)  nState = m_nControlState;

            return Enum.GetName(typeof(EN_CONTROL_STATE),nState);   

        }
        public string StrGemState(long nState = -1)
        {

            if(nState == -1)  nState = m_nGemState;

            return Enum.GetName(typeof(EN_CONNECT_STATE),nState);
        }




        //---------------------------------------------------------
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
            m_listPPID           = new List<string>();
            m_gem.OnEZGemEvent  += new ON_EZGEM_EVENT(OnEventReceived);     // Gem내부 이밴트를 받음.
            m_gem.OnEZGemMsg    += new ON_EZGEM_MSG  (OnMsgReceived  );     // Host로 부터 받은 메세지를 전달.

            ECV.m_nCommReqeustTimeout = 5;
            ECV.m_nPort               = 5000;
            ECV.m_nDeviceID           = 0;
            ECV.m_nT3                 = 15;
            ECV.m_nT5                 = 5;
            ECV.m_nT6                 = 6;
            ECV.m_nT7                 = 7;
            ECV.m_nT8                 = 8;
            ECV.m_nLinkInterval       = 60;
            ECV.m_bPassive            = true;
            ECV.m_sModelName          = "SAMPLE";
            ECV.m_sSoftRev            = "190109";
            
            SetGem    ();
            SetECID   ();
        }

        public void SetECID()
        {

            m_gem.AddECID   (ECID.T3, "T3", "SECOND", "U1");
            m_gem.SetECRange(ECID.T3, "1", "255");
            m_gem.SetECValue(ECID.T3, ECV.m_nT3.ToString());

            m_gem.AddECID   (ECID.T5, "T5", "SECOND", "U1");
            m_gem.SetECRange(ECID.T5, "1", "255");
            m_gem.SetECValue(ECID.T5, ECV.m_nT5.ToString());

            m_gem.AddECID   (ECID.T6, "T6", "SECOND", "U1");
            m_gem.SetECRange(ECID.T6, "1", "255");
            m_gem.SetECValue(ECID.T6, ECV.m_nT6.ToString());

            m_gem.AddECID   (ECID.T7, "T7", "SECOND", "U1");
            m_gem.SetECRange(ECID.T7, "1", "255");
            m_gem.SetECValue(ECID.T7, ECV.m_nT7.ToString());

            m_gem.AddECID   (ECID.T8, "T8", "SECOND", "U1");
            m_gem.SetECRange(ECID.T8, "1", "255");
            m_gem.SetECValue(ECID.T8, ECV.m_nT8.ToString());

            m_gem.AddECID   (ECID.ESTABLISH_TIMEOUT, "ECID.ECID_ESTABLISH_TIMEOUT", "SECOND", "U2");
            m_gem.SetECRange(ECID.ESTABLISH_TIMEOUT, "0", "9999");
            m_gem.SetECValue(ECID.ESTABLISH_TIMEOUT, ECV.m_nCommReqeustTimeout.ToString());

        }

        
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        //Make Log.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~         
		public void  Log           (String format, params object[] args)
        {
            //Local Var.
            string sPath;
            string sTemp;           
            try {
                string sFile =   "[" + string.Format("{0:yyMMdd}", DateTime.Now)+ "]" + "GEM.txt"; 
                sTemp = "[" + string.Format("{0:HH:mm:ss}", DateTime.Now) + "]" + String.Format(format, args) + "\r\n";
                //Make Dir.
                FNC.CreateDirOnWork("LOG");
                FNC.CreateDirOnWork("LOG\\GEM");
                sPath = Application.StartupPath + "\\LOG\\GEM\\" + sFile;
                using (Stream stream = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write)) 
                {
                    StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
                    sw.BaseStream.Seek                (0, SeekOrigin.End);

                    sw.Write(sTemp);
                    sw.Flush();
                    sw.Close();
                }
                m_bShowMsg = true ;
                m_sLastMsg = sTemp;
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TGemLinkJ. Log " + ex.ToString());
            }
        }




        private void OnEventReceived(IntPtr lpParam, short nEventId, int lParam)
        {
            ///////////////////////////////////////////////////
            switch (nEventId)
            {
                case 1   : OnConnected    ()      ; break; // tcp connect 
                case 2   : OnDisconnected ()      ; break; // tcp disconnect
                case 401 : OnMsgIn        (lParam); break; // Msg In시 호출되는 함수.
                case 402 : OnMsgOut       (lParam); break; // Msg Out시 호출되는 함수.
                case 1001: OnOffline      ()      ; break; // S1F15
                case 1002: OnOnlineLocal  ()      ; break; // S1F17
                case 1003: OnOnlineRemote ()      ; break; // S1F17
                case 1010: OnCommunicating()      ; break;
                case 1030: OnRemoteCommand(lParam); break; // S2F41w
                case 1015: OnNewHOST_ECID (lParam); break; // S2F15w
                case 1050: OnTerminalMsg  (lParam); break; // S10F3, S10F5
                default  :                          break;
            }

        }
        public void OnMsgIn(int lParam)
        {
            int nStream = 0, nFunction = 0;
            nStream   = (int)(lParam / 1000);
            nFunction =       lParam % 1000;
            Log(string.Format("(H->E) S{0},F{1}", nStream, nFunction));

        }

        public void OnMsgOut(int lParam)
        {
            int nStream = 0, nFunction = 0;
            nStream     = (int)(lParam / 1000);
            nFunction   = lParam % 1000;
            Log(string.Format("(E->H) S{0},F{1}", nStream, nFunction));

        }

        public void OnConnected()
        {
            m_nGemState       = (short)EN_CONNECT_STATE.CONNECT;
        }


        public void OnDisconnected()
        {
            m_nGemState      = (short)EN_CONNECT_STATE.NOT_CONNECT;
        }

        public void OnOffline()
        {
            m_nControlState = (int)EN_CONTROL_STATE.HOST_OFFLINE;
            //m_gem.SetSVIDValue  (SVID.CONTROL_STATE, m_nControlState.ToString());
            //m_gem.SendEventReport(CEID.CONTROL_OFFLINE);
            m_bControlStatChange = true;

        }

        public void OnOnlineLocal()
        {
            m_nControlState    = (int)EN_CONTROL_STATE.ONLINE_LOCAL;
            //m_gem.SetSVIDValue  (SVID.CONTROL_STATE, m_nControlState.ToString());
            //m_gem.SendEventReport(CEID.CONTROL_LOCAL);

            m_bControlStatChange = true;
        }


        public void OnOnlineRemote()
        {
            m_nControlState    = (int)EN_CONTROL_STATE.ONLINE_REMOTE;
            //m_gem.SetSVIDValue   (SVID.CONTROL_STATE, m_nControlState.ToString());
            //m_gem.SendEventReport(CEID.CONTROL_REMOTE);

            m_bControlStatChange = true;
        }

        public void OnCommunicating()
        {
            m_nGemState      = (short)EN_CONNECT_STATE.COMMUNICATING;
        }

        public void OnRemoteCommand( int lMsgId )
        {

	        long    nReturn         ;
            short   nFormat = 0     ; 
	        string  sText           ;
	        string  sValue          = "";
	        string  sParmName       = "";
	        string  sParmValue      = "";
	        short   nHCACK          = 0 ;
        

	        //RCMD Data
            m_iRCMD_CPCount = m_gem.GetRemoteCommand(lMsgId, ref m_sRCMD);

            sText = string.Format("RCMD = {0}, Count = {1} , MsgId = {2}", m_sRCMD, m_iRCMD_CPCount, lMsgId);
	        Log(sText);

	        for(short i = 0; i < m_iRCMD_CPCount; i++) {
		        if(i>=MAX_RCMD) break;
                m_gem.GetRemoteCommandParam(lMsgId, i, ref sParmName, ref sParmValue, ref nFormat);
		        m_sRCMD_CPName[i] = sParmName ;
		        m_sRCMD_CPVal [i] = sParmValue;
		        }

            nHCACK = (short)cDEF.GEM.RCMDReceived(m_sRCMD, m_iRCMD_CPCount, m_sRCMD_CPName, m_sRCMD_CPVal);  //RCMD Process
	        nReturn = m_gem.ReplyRemoteCommand(lMsgId, nHCACK); // 0x00 = OK;
	        if( nReturn == 0 )
            {
			    Log("Send ReplyRemoteCommand successfully");
		    }
		    else {
			    Log("Fail to ReplyRemoteCommand ({0})", nReturn);
		    }
        }

        public void OnNewHOST_ECID( int lMsgId )
        {
            short nECCount = 0;
            int nECID = 0;
            string strNewValue = "";


            nECCount = 0;
            while( nECCount > -1 )
            {
                nECCount = m_gem.GetHostSetECID(lMsgId, ref nECID, ref strNewValue); // HOST에서 전송한 ECID, ECVALUE를 가져옴.
                
                if( nECCount < 0 )
                {
                    break;
                }
                Log(string.Format("ECID={0},VALUE={1}", nECID, strNewValue));
                m_gem.SetECValue(nECID, strNewValue); //해당 값을 ECID에 저장.
            }
            

            m_gem.ReplyHostSetECID(lMsgId, 0); // ECID에 대한 응답을 보냄.

        }


        public void OnTerminalMsg( int lMsgId )
        {

            // S10F3, S10F5를 받은 경우. 장비화면이나 팝업화면에 표시 
            short nTID = 0;
            int nCount = 0;
            string strMessage = "";

            nCount = 0;
            while( nCount > - 1 )
            {
                nCount = m_gem.GetTerminalMsg(lMsgId, ref nTID, ref strMessage);
                if (nCount < 0)
                {
                    break;
                }
                Log(string.Format("TID={0},MESSAGE={1}", nTID, strMessage));
            }

            //터미널 메세지의 응답은 불필요.

        }



        private void OnMsgReceived(IntPtr lpParam, int lMsgId)
        {
            //////////////////////////////////////////////////

            short nStream = 0, nFunction = 0, nWbit = 0;
            int nLength = 0;

            m_gem.GetMsgInfo(lMsgId, ref nStream, ref nFunction, ref nWbit, ref nLength);

            if (nStream == 7 && nFunction == 3)
            {
                OnS7F3(lMsgId);
            }
            else if( nStream == 7 && nFunction == 5 )
            {
                OnS7F5(lMsgId);
            }
            else if (nStream == 7 && nFunction == 17)
            {
                OnS7F17(lMsgId);
            }
            else if (nStream == 7 && nFunction == 19)
            {
                OnS7F19(lMsgId);
            }
			else if (nStream == 2 && nFunction == 41) // S2F41-20190515
			{
				//OnS2F41(lMsgId);
			}
          
        }

        public void OnS7F3( int lMsgId )
        {
            ///////////  Process Program Send(PPS)

            string strPPID = "";
            byte nACKC7 = 0x03;
            string strFilename = "";

            m_gem.GetListItemOpen(lMsgId);
            //GetAsciiItem((long)lMsgId, strPPID);	//Device Name을 strPPID에 담아 줌

            m_gem.GetAsciiItem(lMsgId, ref strPPID);

            strFilename = strPPID + ".rcp";
            m_gem.GetFileBinaryItem(lMsgId, strFilename);

            m_gem.GetListItemClose(lMsgId);

            Log(strPPID);

            int rMsgId = m_gem.CreateReplyMsg(lMsgId);

            m_gem.AddBinaryItem(lMsgId, nACKC7);

            ///////// ACKC7의 응답값 /////////////////
            // 0 : Accepted
            // 1 : Permission not granted
            // 2 : Length error
            // 3 : Matrix overflow
            // 4 : PPID not found
            // 5 : Mode unsupported
            // 6 : Command will be performed

            m_gem.SendMsg(rMsgId);
        }

        public void OnS7F5( int lMsgId )
        {
            string strPPID = "";
            m_gem.GetAsciiItem(lMsgId, ref strPPID);

            //strPPID에 들어 있는 Device Name으로 해당 Device에 대한 파일 생성하여 그 Path Name을 반한하는 함수를 만들어서 사용
            string strPathName = strPPID + ".rcp";

            //S7F6을 생성하는 부분
            int rMsgId = m_gem.CreateReplyMsg(lMsgId);

            m_gem.OpenListItem(rMsgId);
            if (strPathName != "")		//해당 DeviceFile 이 존재 하면 추가 없다면 추가 하지 않음
            {
                m_gem.AddAsciiItem(rMsgId, strPPID, strPPID.Length);
                m_gem.AddFileBinaryItem(rMsgId, strPathName);	//File PathName을 기반으로 파일을 추가 함
            }
            m_gem.CloseListItem(rMsgId);
            m_gem.SendMsg(rMsgId);			//Host로 S7F6 전송
        }

        public void OnS7F17( int lMsgId )
        {
            byte nACK7 = 0x00;
            short nCount = 0;
            string strPPID = "";


            List<string> listPPID = new List<string>();

            ///////// ACKC7의 응답값 /////////////////
            // 0 : Accepted
            // 1 : Permission not granted
            // 2 : Length error
            // 3 : Matrix overflow
            // 4 : PPID not found
            // 5 : Mode unsupported
            // 6 : Command will be performed

            nACK7 = 0x00;

            nCount = m_gem.GetListItemOpen(lMsgId);
            ///////////////// 삭제할 레시피의 개수와 삭제가 가능한지 여부 파악 //////////////
            if (nCount == 0)
            {
                ////////////// 저장된 모든 레시피 삭제 
                nACK7 = 0x00;
            }
            else
            {
                ////////////// 삭제할 레시피 목록이 내려옴
                ////들어온 strPPID가 삭제가 가능한지, 존재하는 레시피인지 검색
                for (int i = 0; i < nCount; i++)
                {
                    strPPID = "";
                    m_gem.GetAsciiItem(lMsgId, ref strPPID);

                    listPPID.Add(strPPID);
                }
                ////////// listPPID에 저장된 모든 레시피가 삭제가 가능한 경우 nACK7 == 0x00;
                ////////// 삭제가 불가능하거나 없는 경우 위의 ACKC7의 응답값 을 참조하여 설정 ///////////////////
            }
            m_gem.GetListItemClose(lMsgId);


            ////////////// 실제로 레시피를 삭제하는 부분 ///////////////////////////
            if (nACK7 == 0x00 && nCount > 0)
            {
                /// strPPID에 저장된 레시피 삭제
                for (int j = 0; j < nCount; j++)
                {
                    Log(listPPID[j]);
                }
            }
            else if (nACK7 == 0x00 && nCount == 0)
            {
                //전체 레시피 삭제 
            }


            nACK7 = 0x04;

            /////// S7,F17의 응답인 S7,F18 Message를 만듬(rMsgId)
            int rMsgId = m_gem.CreateReplyMsg(lMsgId);
            m_gem.AddBinaryItem(lMsgId, nACK7);
            m_gem.SendMsg(rMsgId);
        }

        public void OnS7F19( int lMsgId )
        {
            //20100701
            //listPPID는 전역 변수 입니다. Host로 부터 Recipe ID List Request 가 왔으므로 해당 Recipe List를 전송해야 합니다.
            //이 함수를 타는 시점에 listPPID에 Recipe List를 Update 하는 부분을 추가 해 두면 됩니다.

            GetCurrentRecipeList();
            int nCount = 0;
            nCount = m_listPPID.Count;
            /// 현재 장비에 저장된 Recipe목록을 Host로 보내는 부분


            int rMsgId = m_gem.CreateReplyMsg(lMsgId);
            m_gem.OpenListItem(rMsgId);
            for (int i = 0; i < nCount; i++)
            {
                string strPPID = "";
                strPPID = m_listPPID[i];
                m_gem.AddAsciiItem(rMsgId, strPPID, strPPID.Length);
            }
            m_gem.CloseListItem(rMsgId);
            m_gem.SendMsg(rMsgId);
        }
        public void GetCurrentRecipeList()
        {
            m_listPPID.Clear();


            //== 실제 장비의 레시피 이름을 찾는다.
            m_listPPID.Add("PPID_01");
            m_listPPID.Add("PPID_02");
            m_listPPID.Add("PPID_03");
            m_listPPID.Add("PPID_04");
        }


        public bool OnStart(string sCfgFile = "")
        {
          
            SetGem ();



            m_gem.SetFormatFile (Application.StartupPath + "\\SECS\\FORMAT.SML"); // S9계열 error메세지를 거르기 위하여 미리 포멧을 정의
            m_gem.SetFormatCheck(true);

            m_gem.CommRequest = 5;
            m_gem.WriteUserLog(string.Format("{0} C#전용 EZGEM DLL버젼을 구동시작", GetStringNow()));
            


            //각 ID는 미리 등록을 해두어야 사용이 가능하다.
            AddSVID(); // SVID등록하기
            AddCEID(); // CEID등록하기
            AddALID(); // ALAMR등록하기
            AddECID(); // ECID등록하기
            AddRCMD(); // REMOTE COMMAND 등록하기

            //각 ID별로 포멧을 정의함. I1, I2 , I4 , U1 , U2 , U4 가능.
            m_gem.SetFormatCode("SVID"   , "U4");
            m_gem.SetFormatCode("CEID"   , "U4");
            m_gem.SetFormatCode("ECID"   , "U4");
            m_gem.SetFormatCode("ALID"   , "U4");
            m_gem.SetFormatCode("TRACEID", "U4");
            m_gem.SetFormatCode("RPTID"  , "U4");
            m_gem.SetFormatCode("DATAID" , "U4");


            //////////////////////////////////////////////////////////////////////////
	        //		GEM드라이버에서 자동으로 응답하는 것을 막아줌
	        //		S7F17 과 S7F19의 경우는 장비 회사에서 처리 해 주어야 함
	        //		S7F17 = Recipe Delete Request
	        //		S7F19 = Recipe List Request
            m_gem.DisableAutoReply(1, 15);
	        m_gem.DisableAutoReply(2, 25);
            m_gem.DisableAutoReply(7, 3 );
            m_gem.DisableAutoReply(7, 5 );
            m_gem.DisableAutoReply(7, 17);
	        m_gem.DisableAutoReply(7, 19);

	        m_gem.DisableAutoReply(65, 4 );
	        m_gem.DisableAutoReply(65, 6 );
	        m_gem.DisableAutoReply(65, 8 );
	        m_gem.DisableAutoReply(65, 10);
            m_gem.DisableAutoReply(2,  41); //S2F41-20190515

            bool isStart = m_gem.Start() == 0;
            if (isStart)
            {

                m_gem.GoOnlineRemote(); // S1F13을 통해 EstablishCommuncation이 되었을 때 진행할 Online함수
                                        // OnlineLocal을 원할 경우 GoOnlineLocal(); 함수를 호출.
                //----Gem 시작시 중요 설정값을 화면에 표시 ---------------
                if (m_gem.PassiveMode == 1) Log("PASSIVE MODE");
                else                        Log("ACTIVE MODE");
                Log(string.Format("PORT={0}"  , ECV.m_nPort));
                Log(string.Format("DEVICE={0}", ECV.m_nDeviceID));
                Log("EZGEM DLL (+) STARTED");
                //---------------------------------------------------------
            }

            return isStart;
        }
        public void OnStop()
        {
	        m_gem.Stop();
	        Log("GEM PROCESS STOPPED");
        }
        public bool IsReady     ()//Xgem Status 확인
        {
            if ( m_nGemState     != (int)EN_CONNECT_STATE.COMMUNICATING   )  { return false; }
            if ( m_nControlState != (int)EN_CONTROL_STATE.ONLINE_LOCAL &&
                 m_nControlState != (int)EN_CONTROL_STATE.ONLINE_REMOTE   )  { return false; }
            return true;
        }

        //---------------------------------------------------------------------------
        public void ReqControlStatusChange(int iControlMode, bool bSndFlag = true)
        {
	        m_nControlState = iControlMode;
	        if(m_nPrvCtrlState != iControlMode){
		        m_nPrvCtrlState = iControlMode;
                m_bControlStatChange = true;
	        }

            if(!bSndFlag) return;
            if(iControlMode == (int)EN_CONTROL_STATE.EQ_OFFLINE) m_gem.GoOffline();

        }

        public void SetProcessingState(long nState = -1)
        {

            if(nState == -1)  nState = m_nProcessState;

            if(nState != m_nProcessState)
            {
                m_bProcessStatChange = true;
                m_nPrvProcessState  = m_nProcessState;
            }
            m_nProcessState     = nState;
        }



        public void SetGem()
        {
            ReadConfig();
            //------------ 속성(Attribute) Parameter
            m_gem.DeviceID         = (short)ECV.m_nDeviceID;    // Default = 0
            m_gem.Port             = (short)ECV.m_nPort    ;    // Default = 5000
            m_gem.PassiveMode      = (ECV.m_bPassive) ? (short)1: (short)0;
            m_gem.T3               = (short)ECV.m_nT3;
            m_gem.T5               = (short)ECV.m_nT5;
            m_gem.T6               = (short)ECV.m_nT6;
            m_gem.T7               = (short)ECV.m_nT7;
            m_gem.T8               = (short)ECV.m_nT8;
            m_gem.RetryCount       = (short)ECV.m_nRetry;        // Default = 0 
            m_gem.LinkTestInterval = (short)ECV.m_nLinkInterval; // Default = 30 sec
            m_gem.SetIP(ECV.m_strIP); // Default = "127.0.0.1" (localhost)
            
            //------------- Method
            m_gem.SetLogFile    (string.Format("SECS\\GEM.LOG")); // Default Log = "GEM.LOG"
            m_gem.SetLogRetention(30);

            m_gem.SetTimeFormat((ECV.m_nTimeFormat==0) ? (short)12 : (short)16); //Timeformat 설정 12자리와 16자리
	        m_gem.SetModelName  (ECV.m_sModelName);
	        m_gem.SetSoftRev    (ECV.m_sSoftRev  );
        }
       public uint TranShort(string strValue)
        {
			int m = 0;
			uint s = 0;
			try
			{
				m = Int32.Parse(strValue);
			}
			catch
			{
				m = 0;
			}
			s = (uint)m;
			return s;
        }


        public string GetStringNow()
        {
            string strTime = "";
            DateTime now = DateTime.Now;
            strTime = now.ToString("MM-dd HH:mm:ss");

            return strTime;
        }


        public void ReadConfig()
        {

            String sPath    ;

            String sFile = "GEM";
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("SECS");
            sPath = Application.StartupPath + "\\SECS\\" + sFile + ".INI";

            ini.Load(sPath, "GEM", "PORT"                  , out ECV.m_nPort               );
            ini.Load(sPath, "GEM", "DEVICEID"              , out ECV.m_nDeviceID           );
            ini.Load(sPath, "GEM", "LINKTEST"              , out ECV.m_nLinkInterval       );
            ini.Load(sPath, "GEM", "RETRY"                 , out ECV.m_nRetry              );
            ini.Load(sPath, "GEM", "CTTIME"                , out ECV.m_nCTTime             );
            ini.Load(sPath, "GEM", "T3"                    , out ECV.m_nT3                 );
            ini.Load(sPath, "GEM", "T5"                    , out ECV.m_nT5                 );
            ini.Load(sPath, "GEM", "T6"                    , out ECV.m_nT6                 );
            ini.Load(sPath, "GEM", "T7"                    , out ECV.m_nT7                 );
            ini.Load(sPath, "GEM", "T8"                    , out ECV.m_nT8                 );
            ini.Load(sPath, "GEM", "DEFAULTCOMMSTATE"      , out ECV.m_nDefaultCommState   );
            ini.Load(sPath, "GEM", "DEFAULTCONTROLSTATE"   , out ECV.m_nDefaultControlState);
            ini.Load(sPath, "GEM", "IdleTime"              , out ECV.m_nIdleTime           );
            ini.Load(sPath, "GEM", "PASSIVE"               , out ECV.m_strPASSIVE          );
            ini.Load(sPath, "GEM", "IP"                    , out ECV.m_strIP               );

            ini.Load(sPath, "GEM", "CommRequestTimeout"    , out ECV.m_nCommReqeustTimeout );
            ini.Load(sPath, "GEM", "MDLN"                  , out ECV.m_sModelName          );
            ini.Load(sPath, "GEM", "SOFTREV"               , out ECV.m_sSoftRev            );
            ini.Load(sPath, "GEM", "TimeFormat"            , out ECV.m_nTimeFormat         );

            if(ECV.m_strPASSIVE == "1") ECV.m_bPassive = true ;
            else                        ECV.m_bPassive = false;
            ini = null;
       
        }


		void       WriteConfigFile        ()
        {
            String sPath      ;
            String sFile = "GEM";
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("SECS");
            sPath = Application.StartupPath + "\\SECS\\" + sFile + ".INI";

            ini.Save(sPath, "GEM", "PORT"                  , ECV.m_nPort               );
            ini.Save(sPath, "GEM", "DEVICEID"              , ECV.m_nDeviceID           );
            ini.Save(sPath, "GEM", "LINKTEST"              , ECV.m_nLinkInterval       );
            ini.Save(sPath, "GEM", "RETRY"                 , ECV.m_nRetry              );
            ini.Save(sPath, "GEM", "CTTIME"                , ECV.m_nCTTime             );
            ini.Save(sPath, "GEM", "T3"                    , ECV.m_nT3                 );
            ini.Save(sPath, "GEM", "T5"                    , ECV.m_nT5                 );
            ini.Save(sPath, "GEM", "T6"                    , ECV.m_nT6                 );
            ini.Save(sPath, "GEM", "T7"                    , ECV.m_nT7                 );
            ini.Save(sPath, "GEM", "T8"                    , ECV.m_nT8                 );
            ini.Save(sPath, "GEM", "DEFAULTCOMMSTATE"      , ECV.m_nDefaultCommState   );
            ini.Save(sPath, "GEM", "DEFAULTCONTROLSTATE"   , ECV.m_nDefaultControlState);
            ini.Save(sPath, "GEM", "IdleTime"              , ECV.m_nIdleTime           );
            ini.Save(sPath, "GEM", "MODE"                  , ECV.m_strMODE             );
            ini.Save(sPath, "GEM", "PASSIVE"               , ECV.m_strPASSIVE          );
            ini.Save(sPath, "GEM", "IP"                    , ECV.m_strIP               );
            ini.Save(sPath, "GEM", "CommRequestTimeout"    , ECV.m_nCommReqeustTimeout );
            ini.Save(sPath, "GEM", "MDLN"                  , ECV.m_sModelName          );
            ini.Save(sPath, "GEM", "SOFTREV"               , ECV.m_sSoftRev            );
            ini.Save(sPath, "GEM", "TimeFormat"            , ECV.m_nTimeFormat         );    

        }

       public bool AddSVID()
       {//SVID를 gem dll에 등록한다.
            String sPath    ;

            String sFile = "NVIA_SVID";
            //Make Dir.
            FNC.CreateDirOnWork("SECS");
            sPath = Application.StartupPath + "\\SECS\\" + sFile + ".TXT";


            if (!File.Exists(sPath))
            {
                cDEF.LOG.Trace("[SVID] File Name is invalid, File Name= " + sPath);
                return false;
            }

            string   s    ; 
            string[] a_res;

            StreamReader sr = new StreamReader(sPath, Encoding.ASCII);
            try
            {
                while (sr.Peek() >= 0)
                {
                    s     = sr.ReadLine();
                    a_res = s.Trim().Split(',');
                    if(a_res[0] == "") continue;
                    
                    m_gem.AddSVID(FNC.ConvInt(a_res[0]), a_res[1] , a_res[2], "" ); //iID,	sNAME , sFORMAT, sUNIT

                }
                sr.Close();
            }
            catch (Exception ex)
            {
                cDEF.LOG.Trace("SVID File Read Error, msg=" + ex.Message);
                sr.Close();
                return false;
            }
           return true;  
        }

        public bool AddCEID()
        {

            String sPath    ;
            String sFile = "NVIA_CEID";
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("SECS");
            sPath = Application.StartupPath + "\\SECS\\" + sFile + ".TXT";


            if (!File.Exists(sPath))
            {
                cDEF.LOG.Trace("[CEID] File Name is invalid, File Name= " + sPath);
                return false;
            }

            string   s    ; 
            string[] a_res;
            StreamReader sr = new StreamReader(sPath, Encoding.ASCII);
            try
            {
                while (sr.Peek() >= 0)
                {
                    s     = sr.ReadLine();
                    a_res = s.Trim().Split(',');
                    if(a_res[0] == "") continue;
                    m_gem.AddCEID(FNC.ConvInt(a_res[0]), a_res[1] , ""); //iID sNAME, sDESC

                }
                sr.Close();
            }
            catch (Exception ex)
            {
                cDEF.LOG.Trace("CEID File Read Error, msg=" + ex.Message);
                sr.Close();
                return false;
            }
           return true;  
        }

        public bool AddALID()
        {
            String sPath    ;

            String sFile = "NVIA_ALARM";
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("SECS");
            sPath = Application.StartupPath + "\\SECS\\" + sFile + ".TXT";


            if (!File.Exists(sPath))
            {
                cDEF.LOG.Trace("[ALID] File Name is invalid, File Name= " + sPath);
                return false;
            }

            string   s    ; 
            string[] a_res;
            StreamReader sr = new StreamReader(sPath, Encoding.ASCII);
            try
            {
                while (sr.Peek() >= 0)
                {
                    s     = sr.ReadLine();
                    a_res = s.Trim().Split(',');
                    if(a_res[0] == "") continue;
                    m_gem.AddALID(FNC.ConvInt(a_res[0]), a_res[1] , a_res[2]); //iID ALTX, ALCD

                }
                sr.Close();
            }
            catch (Exception ex)
            {
                cDEF.LOG.Trace("ALID File Read Error, msg=" + ex.Message);
                sr.Close();
                return false;
            }
           return true;  
        }

        public bool AddECID()
        {
            String sPath    ;

            String sFile = "NVIA_ECID";
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("SECS");
            sPath = Application.StartupPath + "\\SECS\\" + sFile + ".TXT";

            if (!File.Exists(sPath))
            {
                cDEF.LOG.Trace("[ECID] File Name is invalid, File Name= " + sPath);
                return false;
            }

            string   s    ; 
            string[] a_res;
            StreamReader sr = new StreamReader(sPath, Encoding.ASCII);
            try
            {
                while (sr.Peek() >= 0)
                {
                    s     = sr.ReadLine();
                    a_res = s.Trim().Split(',');
                    if(a_res[0] == "") continue;
		            m_gem.AddECID           (FNC.ConvInt(a_res[0]), a_res[1]  ,a_res[5], a_res[6]); //iID,    sNAME  ,UNIT, A
		            m_gem.SetECRange        (FNC.ConvInt(a_res[0]), a_res[2]  ,a_res[3]          ); //sMIN   ,sMAX
		            m_gem.SetECValue        (FNC.ConvInt(a_res[0]), a_res[4]);                      //sVALUE
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                cDEF.LOG.Trace("ECID File Read Error, msg=" + ex.Message);
                sr.Close();
                return false;
            }
           return true;  
        }

		bool  AddRCMD()
        {
	        String sNAME    ;
            String sPath    ;
            String sTemp    ;

            String sFile = "NVIA_RCMD";
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("SECS");
            sPath = Application.StartupPath + "\\SECS\\" + sFile + ".TXT";


            if (!File.Exists(sPath))
            {
                cDEF.LOG.Trace("[RCMD] File Name is invalid, File Name= " + sPath);
                return false;
            }

            string   s    ; 
            string[] a_res;
            StreamReader sr = new StreamReader(sPath, Encoding.ASCII);
            try
            {
                while (sr.Peek() >= 0)
                {
                    s     = sr.ReadLine();
                    a_res = s.Trim().Split(',');
                    if(a_res[0] == "") continue;
                    m_gem.AddRemoteCommand(a_res[0]);
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                cDEF.LOG.Trace("RCMD File Read Error, msg=" + ex.Message);
                sr.Close();
                return false;
            }
           return true;  
        }


		public bool       SetEvent              (long nCEID)
        {
 	        m_gem.SendEventReport((int)nCEID);
	        return true; ;
       
        }
		public bool SetSVID  (long iSVID, String strValue)
        {
 	        m_gem.SetSVIDValue((int)iSVID, strValue);
	        return true;       
        }
        public bool SetSVID(long nCount, long[] naVid, string[] saValue)
        {//Variable에 대한 Value값을 Vid 로 설정하고자 할때 사용합니다.
            //Update variable    
            for(int i=0;i<nCount;i++)
            {
                m_gem.SetSVIDValue((int)naVid[i], saValue[i]);
            }    
	        return true;                  
        }


		public bool SetECV (long iECID, String strValue)
        {
	        m_gem.SetECValue((int)iECID, strValue);
	        return true;        
        }


        public bool SetAlarm(long nAlarmID, long nSet)
        {//S5F1(H<-E) Alarm Report Send(ARS)
         //Equipment 에서 Alarm 발생시 Alarm 정보를 XGemPro 으로 전송한다
         //XGemPro에서 S5F1을 Host로 보고하며 Alarm Detect Event 및 Alarm Clear Event가발생됩니다.

           short ALCD = (nSet == 1) ? (short)(128 + 1) : (short)1;
 	       long nReturn = m_gem.SendAlarmReport((int)nAlarmID, ALCD);
 	       if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] SetAlarm => ID:{0}, State:{1} ({2})", nAlarmID, 1, nReturn);
            }
            else {
                Log("[EQ ==> XGEM] Fail SetAlarm ({0})", nReturn);
            }
            return (nReturn == 0);
        }
    }
}
