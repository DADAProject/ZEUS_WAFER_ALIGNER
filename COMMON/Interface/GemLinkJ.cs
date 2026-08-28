using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using GEM_XGemPro;

namespace eMachine
{
    public enum _eCommState
    {       
        None           = 0,
        CommDisabled   = 1,  
        WaitCRFromHost = 2,  
        WaitDelay      = 3,  
        WaitCRA        = 4,  
        Communicating  = 5  

    }

    
    public enum _eGemState
    {
        Unknown  = -1, 
        Init     = 0 ,    
        Idle     = 1 ,    
        Setup    = 2 , 
        Ready    = 3 , 
        Execute  = 4 ,
    }



    public class TGemLinkJ
    {
                 

        //const short  VALUE_I1     = 0x7f;
        //const short  VALUE_I2     = 0x7fff;
        //const long   VALUE_I4     = 0x7fffffff;
        //const long   VALUE_I8     = 0x7fffffff;
        //const short  VALUE_U1     = 0xff;
        //const long   VALUE_U2     = 0xffff;
        //const double VALUE_U4     = 0xffffffff;
        //const double VALUE_U8     = 0xffffffff;
        //const float  VALUE_F4     = 1234.567f;
        //const double VALUE_F8     = 123456789.87654321;
        //const short  VALUE_BOOL   = 1;
        //const short  VALUE_BINARY = 10;
        //const string VALUE_STRING = "STRING; XGemPro Sample";
        //const string VALUE_JIS8   = "JIS8; XGemPro Sample";

        //const short VALUE_I1 = 10;
        //const short VALUE_I2 = 32555;
        //const long VALUE_I4 = 655360;
        //const long VALUE_I8 = 240000001;
        //const short VALUE_U1 = 250;
        //const long VALUE_U2 = 65000;
        //const double VALUE_U4 = 4294967290;
        //const double VALUE_U8 = 1844674400;
        //const float VALUE_F4 = 1234.567f;
        //const double VALUE_F8 = 123456789.87654321;
        //const short VALUE_BOOL = 1;
        //const short VALUE_BINARY = 10;
        //const string VALUE_STRING = "STRING; XGemPro Sample";
        //const string VALUE_JIS8 = "JIS8; XGemPro Sample";



        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int      MAX_ARRAY	= 7;
        const int      MAX_RCMD_VAL	= 50;
                                    
        const sbyte    VALUE_I1     = -128;
        const short    VALUE_I2     = -32768;
        const int      VALUE_I4     = -2147483648;
        const int      VALUE_I8     = -2147483648;
        const byte     VALUE_U1     = 255;
        const ushort   VALUE_U2     = 65535;
        const uint     VALUE_U4     = 4294967295;
        const uint     VALUE_U8     = 4294967295;
        const float    VALUE_F4     = 1234.567f;
        const double   VALUE_F8     = 123456789.87654321;
        const bool     VALUE_BOOL   = true;
        const byte     VALUE_BINARY = 10;
        const string   VALUE_STRING = "STRING; XGemPro Sample";
        const string   VALUE_JIS8   = "JIS8; XGemPro Sample";



       
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        sbyte[]        VALUE_I1_ARR     = new sbyte [MAX_ARRAY];
        short[]        VALUE_I2_ARR     = new short [MAX_ARRAY];
        int[]          VALUE_I4_ARR     = new int   [MAX_ARRAY];
        int[]          VALUE_I8_ARR     = new int   [MAX_ARRAY];
        byte[]         VALUE_U1_ARR     = new byte  [MAX_ARRAY];
        ushort[]       VALUE_U2_ARR     = new ushort[MAX_ARRAY];
        uint[]         VALUE_U4_ARR     = new uint  [MAX_ARRAY];
        uint[]         VALUE_U8_ARR     = new uint  [MAX_ARRAY];
        float[]        VALUE_F4_ARR     = new float [MAX_ARRAY];
        double[]       VALUE_F8_ARR     = new double[MAX_ARRAY];
        bool[]         VALUE_BOOL_ARR   = new bool  [MAX_ARRAY];
        byte[]         VALUE_BINARY_ARR = new byte  [MAX_ARRAY];   


        //Control State
        long              m_nCommState        ;
        long              m_nControlState     ;
        long              m_nGemState         ;
        long              m_nProcessState     ;
        bool              m_bSecsInit         ;
        bool              m_bSecsStart        ;
        bool              m_bControlStatChange;
        bool              m_bProcessStatChange;

        long              m_nPrvCtrlState     ;
        long              m_nPrvProcessState  ;



        //public:    /* Direct Accessable Vars.  */
        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool   m_bShowMsg;
        string m_sLastMsg;

        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public long               _nControlState    {get { return m_nControlState;    } }
        public long               _nCommState       {get { return m_nCommState;       } }
        public long               _nGemState        {get { return m_nGemState;       } }
        public long               _nProcessState    {get { return m_nProcessState;    } }

        public bool               _bControlStatChange    {get { return m_bControlStatChange;    } }
        public bool               _bProcessStatChange    {get { return m_bProcessStatChange;    } }

        public long               _nPrvCtrlState    {get { return m_nPrvCtrlState;    } set { m_nPrvCtrlState = value ;     }}
        public long               _nPrvProcessState {get { return m_nPrvProcessState; } set { m_nPrvProcessState = value ;     }}

        public bool        _bShowMsg       {get { return m_bShowMsg  ;     } set { m_bShowMsg = value ;     }}
        public String      _sLastMsg       {get { return m_sLastMsg  ;     } }

        public bool        _bSecsInit      {get { return m_bSecsInit ;     } }
        public bool        _bSecsStart     {get { return m_bSecsStart;     } }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
        public XGemProNet m_XGem = null;


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TGemLinkJ()
        {
            m_bSecsInit  = false;
            m_bSecsStart = false;

            m_nPrvCtrlState    = -1;
            m_nPrvProcessState = -1;

            init();
        }
        ~TGemLinkJ() { }

        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public string StrCommState(long nState = -1)
        {
            if(nState == -1)  nState = m_nCommState;

             return Enum.GetName(typeof(_eCommState),nState);   
        }

        public string StrControlState(long nState = -1)
        {

            if(nState == -1)  nState = m_nControlState;

            return Enum.GetName(typeof(EN_CONTROL_STATE),nState);   

        }
        public string StrGemState(long nState = -1)
        {

            if(nState == -1)  nState = m_nGemState;

            return Enum.GetName(typeof(_eGemState),nState);
        }

        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void init()
        {
            m_XGem = new XGemProNet();

            // Link XGemEventHandler...
            m_XGem.OnSECSMessageReceived     += new OnSECSMessageReceived    (OnSECSMessageReceived    );
            m_XGem.OnGEMCommStateChanged     += new OnGEMCommStateChanged    (OnGEMCommStateChanged    );
            m_XGem.OnGEMControlStateChanged  += new OnGEMControlStateChanged (OnGEMControlStateChanged );
            m_XGem.OnGEMReqChangeECV         += new OnGEMReqChangeECV        (OnGEMReqChangeECV        );
            m_XGem.OnGEMECVChanged           += new OnGEMECVChanged          (OnGEMECVChanged          );
            m_XGem.OnGEMReqGetDateTime       += new OnGEMReqGetDateTime      (OnGEMReqGetDateTime      );
            m_XGem.OnGEMRspGetDateTime       += new OnGEMRspGetDateTime      (OnGEMRspGetDateTime      );
            m_XGem.OnGEMReqDateTime          += new OnGEMReqDateTime         (OnGEMReqDateTime         );
            m_XGem.OnGEMErrorEvent           += new OnGEMErrorEvent          (OnGEMErrorEvent          );
            m_XGem.OnGEMReqRemoteCommand     += new OnGEMReqRemoteCommand    (OnGEMReqRemoteCommand    );
            m_XGem.OnGEMReqPPLoadInquire     += new OnGEMReqPPLoadInquire    (OnGEMReqPPLoadInquire    );
            m_XGem.OnGEMRspPPLoadInquire     += new OnGEMRspPPLoadInquire    (OnGEMRspPPLoadInquire    );
            m_XGem.OnGEMReqPPSend            += new OnGEMReqPPSend           (OnGEMReqPPSend           );
            m_XGem.OnGEMRspPPSend            += new OnGEMRspPPSend           (OnGEMRspPPSend           );
            m_XGem.OnGEMReqPP                += new OnGEMReqPP               (OnGEMReqPP               );
            m_XGem.OnGEMRspPP                += new OnGEMRspPP               (OnGEMRspPP               );
            m_XGem.OnGEMReqPPDelete          += new OnGEMReqPPDelete         (OnGEMReqPPDelete         );
            m_XGem.OnGEMReqPPList            += new OnGEMReqPPList           (OnGEMReqPPList           );
            m_XGem.OnGEMReqPPFmtSend         += new OnGEMReqPPFmtSend        (OnGEMReqPPFmtSend        );
            m_XGem.OnGEMRspPPFmtSend         += new OnGEMRspPPFmtSend        (OnGEMRspPPFmtSend        );
            m_XGem.OnGEMReqPPFmt             += new OnGEMReqPPFmt            (OnGEMReqPPFmt            );
            m_XGem.OnGEMRspPPFmt             += new OnGEMRspPPFmt            (OnGEMRspPPFmt            );
            m_XGem.OnGEMRspPPFmtVerification += new OnGEMRspPPFmtVerification(OnGEMRspPPFmtVerification);
            m_XGem.OnGEMTerminalMessage      += new OnGEMTerminalMessage     (OnGEMTerminalMessage     );
            m_XGem.OnGEMTerminalMultiMessage += new OnGEMTerminalMultiMessage(OnGEMTerminalMultiMessage);
            m_XGem.OnGEMSpoolStateChanged    += new OnGEMSpoolStateChanged   (OnGEMSpoolStateChanged   );
            m_XGem.OnXGEMStateEvent          += new OnXGEMStateEvent         (OnXGEMStateEvent         );
            m_XGem.OnGEMRspAllECInfo         += new OnGEMRspAllECInfo        (OnGEMRspAllECInfo        );

            m_XGem.OnGEMReqOffline           += new OnGEMReqOffline          (m_XGem_OnGEMReqOffline   );
            m_XGem.OnGEMReqOnline            += new OnGEMReqOnline           (m_XGem_OnGEMReqOnline    );
            m_XGem.OnGEMSecondaryMsgReceived += new OnGEMSecondaryMsgReceived(m_XGem_OnGEMSecondaryMsgReceived);
            m_XGem.OnGEMReqPPSendEx          += new OnGEMReqPPSendEx         (m_XGem_OnGEMReqPPSendEx  );

            // Initializing array value
            for (int i = 0; i < MAX_ARRAY; i++)
            {
                VALUE_I1_ARR    [i] = (sbyte) (i + 1);
                VALUE_I2_ARR    [i] = (short) (i + 1);
                VALUE_I4_ARR    [i] = i + 1;  
                VALUE_I8_ARR    [i] = i + 1;
                VALUE_U1_ARR    [i] = (byte)  (i + 1);
                VALUE_U2_ARR    [i] = (ushort)(i + 1);
                VALUE_U4_ARR    [i] = (uint)  (i + 1);
                VALUE_U8_ARR    [i] = (uint)  (i + 1);
                VALUE_F4_ARR    [i] = (float) (i + 1);
                VALUE_F8_ARR    [i] = i + 1;
                VALUE_BOOL_ARR  [i] = true;
                VALUE_BINARY_ARR[i] = 1;
            }
        }

        void m_XGem_OnGEMReqPPSendEx(long nMsgId, string sPpid, string sRecipePath)
        {//S7F3(H->E) Process Program Send (PPS)
         //SECS Message인 S7F3(Process Program Send (PPS))을 Host에서 받았을 경우 발생하는 event입니다.
         //XGemPro에서 Application으로 PPBODY 정보를 file로 전송하고자 할 때사용합니다.

            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPSendEx : Ppid({0}), RecipePath({1})", sPpid, sRecipePath);
            Log(sLog);

            m_XGem.GEMRspPPSendEx(nMsgId, sPpid, sRecipePath, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPSendEx : Ppid({0}), RecipePath({1})", sPpid, sRecipePath);
            Log(sLog);
        }

        void m_XGem_OnGEMSecondaryMsgReceived(long nS, long nF, long nSysbyte, string sParam1, string sParam2, string sParam3)
        {//Host로부터 Secondary Message를 받으면 EQ Application에서 이 event가 발생합니다.
            string sLog = String.Format("[XGEM ==> EQ] OnGEMSecondaryMsgReceived");
            Log(sLog);

            sLog = String.Format("               stream:{0}, function:{1}, sysbyte:{2}, Param1:{3}, Param2:{4}, Param3:{5}", nS, nF, nSysbyte, sParam1, sParam2, sParam3);
            Log(sLog);
        }

        void m_XGem_OnGEMReqOnline(long nMsgId, long nFromState, long nToState)
        {//S1F17(H->E) Request ON-LINE(RONL)
         //SECS Message인 S1F17(Request ON-LINE (RONL))을 Host에서 받았을 경우 발생하는 event입니다. S1F17에 대한 응답 메시지인 S1F18(GEMRspOnline)을 보낼 때 
         //Ack로 응답을 주면 XGemPro에서는 Control State를 FromState에서 ToState로 변경 합니다.

            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqOnline");
            Log(sLog);

            sLog = String.Format("               nMsgId:{0}, nFromState:{1}, nToState:{2}", nMsgId, nFromState, nToState);
            Log(sLog);

            m_XGem.GEMRspOnline(nMsgId, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspOnline => nMsgId:{0}, nAck:{1}", nMsgId, 0);
            Log(sLog);
        }

        void m_XGem_OnGEMReqOffline(long nMsgId, long nFromState, long nToState)
        {//S1F15(H->E) Request OFF-LINE (ROFL)
         //SECS Message인 S1F15(Request OFF-LINE (ROFL))을 Host에서 받았을 경우 발생하는 event입니다. S1F15에 대한 응답 메시지인 S1F16(GEMRspOffline)을 보낼 때 
         //Ack로 응답을 주면 XGemPro에서는 Control State를 FromState에서 ToState로 변경 합니다

            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqOffline");
            Log(sLog);

            sLog = String.Format("               nMsgId:{0}, nFromState:{1}, nToState:{2}",nMsgId, nFromState, nToState);
            Log(sLog);

            m_XGem.GEMRsqOffline(nMsgId, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRsqOffline => nMsgId:{0}, nAck:{1}", nMsgId, 0);
            Log(sLog);
        }


        #region XGemEventHandler 멤버

        public void OnSECSMessageReceived(long nObjectID, long nStream, long nFunction, long nSysbyte)
        {//Host로부터 User Defined Message를 받았을 경우 발생하는 event입니다.
            //throw new Exception("The method or operation is not implemented.");
            string sValue = "";
            sValue = String.Format(
                        "[XGEM ==> EQ] OnSECSMessageReceived : ObjectID({0}), S{1},F{2}, Sysbyte({3})",
                            nObjectID, nStream, nFunction, nSysbyte);
            Log(sValue);


            if(nStream == 2 && nFunction == 99)
	        {//MAP DOWNLOAD
		        OnS2F99W(nObjectID);
	        }
	        else if (nStream == 2 && nFunction == 95)
	        {//PROBE SPEC DOWNLOAD
		        OnS2F95W(nObjectID);
	        }
	        else
	        {
                m_XGem.CloseObject( nObjectID );    
	        }
        }

        public void OnS2F99W(long nObjectID)
        {//MAP DOWNLOAD


        }
        public void OnS2F95W(long nObjectID)
        {//PROBE SPEC DOWNLOAD


        }

        public void OnGEMCommStateChanged(long nState)
        {//XGemPro Process에서 Communication State 의 상태이 변경되었을 때 발생하는 event입니다.
            //throw new Exception("The method or operation is not implemented.");
            m_nCommState = nState;

            string szState = StrCommState(nState);

            if(nState == (long)_eCommState.Communicating) {  
                ReqControlStatusChange((long)EN_CONTROL_STATE.ONLINE_REMOTE);
                }
            string sLog = String.Format("[XGEM ==> EQ] OnGEMCommStateChanged:{0}", szState);
            Log(sLog);
        }

        public void OnGEMControlStateChanged(long nState)
        {//XGemPro Process에서 Control State 의 상태가 변경되었을 때 발생하는 event입니다

            //throw new Exception("The method or operation is not implemented.");
            string szState  = StrControlState(nState);
            m_nControlState = nState;

            string sLog = String.Format("[XGEM ==> EQ] OnGEMControlStateChanged:{0}", szState);
            Log(sLog);
        }

        public void OnGEMReqChangeECV(long nMsgId, long nCount, long[] pnEcids, string[] psVals)
        {//S2F15(HE) New Equipment Constant Send (ECS)
         //SECS Message인 S2F15(New Equipment Constant Send (ECS))을 Host에서 받았을 경우 발생하는 event입니다.
   
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqChangeECV");
            Log(sLog);

            for (int i = 0; i < nCount; i++)
            {
                sLog = String.Format("     Ecid:{0}, Value:{1}", pnEcids[i], psVals[i]);
                Log(sLog);
            }

            m_XGem.GEMRspChangeECV(nMsgId, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspChangeECV");
            Log(sLog);
        }

        public void OnGEMECVChanged(long nCount, long[] pnEcids, string[] psVals)
        {//XGemPro Process에서 ECID에 대한 ECV정보가 변경이 되었을 경우 발생하는 event입니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMECVChanged");
            Log(sLog);

            for (int i = 0; i < nCount; i++)
            {
                sLog = String.Format("               Ecid:{0}, Value:{1}", pnEcids[i], psVals[i]);
                Log(sLog);
            }
        }

        public void OnGEMReqGetDateTime(long nMsgId)
        {//S2F17(H->E) Date and Time Request (DTR)
         //SECS Message인 S2F17(Date and Time Request (DTR)))을 Host에서 받았을 경우 발생하는 event입니다.
   
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqGetDateTime");
            Log(sLog);

            string sSystemTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            m_XGem.GEMRspGetDateTime(nMsgId, sSystemTime);
            sLog = String.Format("[EQ ==> XGEM] GEMRspGetDateTime:{0}", sSystemTime);
            Log(sLog);      

        }

        public void OnGEMRspGetDateTime(string sSystemTime)
        {//S2F18(H->E) Date and Time Data (DTD)
         //SECS Message인 S2F18(Date and Time Data (DTD))을 Host에서 받았을 경우 발생하는 event입니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspGetDateTime : systemtime({0})", sSystemTime);
            Log(sLog);

            //Debug.WriteLine(String.Format("OnGEMRspGetDateTimeExgemctrl1 th={0}\n", Thread.CurrentThread.ManagedThreadId));
        }

        public void OnGEMReqDateTime(long nMsgId, string sSystemTime)
        {//S2F31(H->E) Date and Time Set Request (DTS)
         //SECS Message인 S2F31(Date and Time Set Request (DTS))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S2F17에 응답 message인 S2F18 (GEMRspGetDateTime() method)를 호출해야 합니다.
    
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqDateTime : systemtime({0})", sSystemTime);
            Log(sLog);

            m_XGem.GEMRspDateTime(nMsgId, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspDateTime");
            Log(sLog);
        }

        public void OnGEMErrorEvent(string sErrorName, long nErrorCode)
        {//XGemPro 의 내부 Error 발생 시 Error Code를 반환합니다.
         //XGemPro control의 method는 대부분 XGemPro Process와 비동기 방식으로 통신이 이루어지기 때문에 Processing중 발생하는 Error에 대해서 Application으로 알려주는 역할을 합니다.
 
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMErrorEvent : ErrorName({0}) ErrorCode({1})", sErrorName, nErrorCode);
            Log(sLog);
        }

        public void OnGEMReqRemoteCommand(long nMsgId, string sRcmd, long nCount, string[] psNames, string[] psVals)
        {//S2F41(HE) Host Command Send (HCS)
         //SECS Message인 S2F41(Host Command Send (HCS))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S2F41에 응답 message인 S2F42 (GEMRspRemoteCommand() method)를 호출해야 합니다.

            //throw new Exception("The method or operation is not implemented.");
            long nACK = 0;
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqRemoteCommand : Remote Command({0}), ", sRcmd);
            Log(sLog);

            for (int i = 0; i < nCount; i++)
            {
                sLog = String.Format("               Name: {0}, Value: {1}", psNames[i], psVals[i]);
                Log(sLog);
            }

            //nACK = cDEF.GEM.RCMDReceived(sRcmd, nCount, psNames, psVals);  //RCMD Process

            long[] nResult = new long[1];
            m_XGem.GEMRspRemoteCommand(nMsgId, sRcmd, nACK, 1, psNames, nResult);
            sLog = String.Format("[EQ ==> XGEM] GEMRspRemoteCommand");
            Log(sLog);


        }

        public void OnGEMReqPPLoadInquire(long nMsgId, string sPpid, long nLength)
        {//S7F1(H->E) Process Program Load Inquire (PPI)
         //SECS Message인 S7F1(Process Program Load Inquire (PPI))을 Host에서 받았을 경우 발생하는 event입니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPLoadInquire : Ppid({0}), ", sPpid);
            Log(sLog);

            m_XGem.GEMRspPPLoadInquire(nMsgId, sPpid, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPLoadInquire : Ppid({0}), ", sPpid);
            Log(sLog);
        }

        public void OnGEMRspPPLoadInquire(string sPpid, long nResult)
        {//S7F2(HE) Process Program Load Grant (PPG)
         //SECS Message인 S7F2 (Process Program Load Grant (PPG))을 Host에서 받았을 경우 발생하는 event입니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPLoadInquire : Ppid({0}), Result({1})", sPpid, nResult);
            Log(sLog);
        }

        public void OnGEMReqPPSend(long nMsgId, string sPpid, byte[] psBody)
        {//S7F3(H->E) Process Program Send (PPS)
         //SECS Message인 S7F3(Process Program Send (PPS))을 Host에서 받았을 경우 발생하는 event입니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPSend : Ppid({0}), Body({1})", sPpid, psBody);
            Log(sLog);

            m_XGem.GEMRspPPSend(nMsgId, sPpid, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPSend : Ppid({0}), ", sPpid);
            Log(sLog);
        }

        public void OnGEMRspPPSend(string sPpid, long nResult)
        {//S7F4(H->E) Process Program Send (PPS)
         //SECS Message인 S7F4(Process Program Send (PPS))을 Host에서 받았을 경우 발생하는 event입니다.
         //이 event는 GEMReqPPSend() method(S7F3)에 대한 응답으로 사용합니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPSend : Ppid({0}), Result({1})", sPpid, nResult);
            Log(sLog);
        }

        public void OnGEMReqPP(long nMsgId, string sPpid)
        {//S7F5(H->E) Process Program Request (PPR)
         //SECS Message인 S7F5(Process Program Request (PPR))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S7F5에 응답 message인 S7F6 (GEMRspPPLoad () method)를 호출해야 합니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPP : Ppid({0})", sPpid);
            Log(sLog);

            byte[] psBody = new byte[100];
            psBody[0] = (byte)'P';
            psBody[1] = (byte)'P';
            psBody[2] = (byte)'b';
            psBody[3] = (byte)'o';
            psBody[4] = (byte)'d';
            psBody[5] = (byte)'y';
            m_XGem.GEMRspPP(nMsgId, sPpid, psBody);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPP : Ppid({0}), Body({1})", sPpid, psBody);
            Log(sLog);
        }

        public void OnGEMRspPP(string sPpid, byte[] psBody)
        {//S7F6(H->E) Process Program Data (PPD)
         //SECS Message인 S7F6(Process Program Data (PPD))을 Host에서 받았을 경우 발생하는 event입니다.
 
            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPP : Ppid({0}), Body({1})", sPpid, psBody);
            Log(sLog);
        }

        public void OnGEMReqPPDelete(long nMsgId, long nCount, string[] psPpid)
        {//S7F17(HE) Delete Process Program Send (DPS)
         //SECS Message인 S7F17(Delete Process Program Send (DPS))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S7F17에 응답 message인 S7F18(GEMRspPPDelete() method)를 호출해야 합니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPDelete");
            Log(sLog);

            string[] psTemp = new string[nCount];
            for (int i = 0; i < nCount; i++)
            {
                psTemp[i] = psPpid[i];
                sLog = String.Format("               Ppid: {0}", psPpid[i]);
                Log(sLog);
            }

            m_XGem.GEMRspPPDelete(nMsgId, nCount, psTemp, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPDelete");
            Log(sLog);
        }

        public void OnGEMReqPPList(long nMsgId)
        {//S7F19(HE) Current EPPD Request (RER)
         //SECS Message인 S7F19(Current EPPD Request (RER))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S7F19에 응답 message인 S7F20(GEMRspPPList() method)를 호출해야 합니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPList");
            Log(sLog);

            long nCount = 0;
            string[] saPpids = new string[2];

            nCount = 2;
            saPpids[0] = "PPID001";
            saPpids[1] = "PPID002";
            m_XGem.GEMRspPPList(nMsgId, nCount, saPpids);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPList");
            Log(sLog);
        }

        public void OnGEMReqPPFmtSend(long nMsgId, string sPpid, string sMdln, string sSoftRev, long nCount, string[] psCCode, long[] pnParamCount, string[] psParamNames)
        {//S7F23(HE) Formatted Process Program Send (FPS)
         //SECS Message인 S7F23(Formatted Process Program Send (FPS))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S7F23에 응답 message인 S7F24(GEMRspPPFmtSend() method)를 호출해야 합니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPFmtSend => Ppid:{0}, Mdln:{1}, SoftRev:{2}", sPpid, sMdln, sSoftRev);
            Log(sLog);

            m_XGem.GEMRspPPFmtSend(nMsgId, sPpid, 0);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPFmtSend");
            Log(sLog);
        }

        public void OnGEMRspPPFmtSend(string sPpid, long nResult)
        {//S7F24(HE) Formatted Process Program Acknowledge (FPA)
         //SECS Message인 S7F24(Formatted Process Program Acknowledge (FPA))을 Host에서 받았을 경우 발생하는 event입니다.
         //이 event는 GEMReqPPFmtSend) method(S7F23)에 대한 응답으로 사용합니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPFmtSend : Ppid({0}), Result({1})", sPpid, nResult);
            Log(sLog);
        }

        public void OnGEMReqPPFmt(long nMsgId, string sPpid)
        {//S7F25(HE) Formatted Process Program Request (FPR
         //SECS Message인 S7F25(Formatted Process Program Request (FPR))을 Host에서 받았을 경우 발생하는 event입니다.


            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMReqPPFmt : Ppid({0})", sPpid);
            Log(sLog);
            
            string   sMdln    = "";
            string   sSoftRev = "";
            long     nCount   = 0;
            string[] saCCodes = new string [2];
            long[]   naPCount = new long   [2];
            string[] saPNames = new string [10];

            sMdln       = "Mdln";
            sSoftRev    = "001";
            nCount      = 2;
            saCCodes[0] = "1";
            naPCount[0] = 5;
            saPNames[0] = "Param001";
            saPNames[1] = "Param002";
            saPNames[2] = "Param003";
            saPNames[3] = "Param004";
            saPNames[4] = "Param005";

            saCCodes[1] = "2";
            naPCount[1] = 5;
            saPNames[5] = "Param006";
            saPNames[6] = "Param007";
            saPNames[7] = "Param008";
            saPNames[8] = "Param009";
            saPNames[9] = "Param010";

            m_XGem.GEMRspPPFmt(nMsgId, sPpid, sMdln, sSoftRev, nCount, saCCodes, naPCount, saPNames);
            sLog = String.Format("[EQ ==> XGEM] GEMRspPPFmt");
            Log(sLog);
        }

        public void OnGEMRspPPFmt(string sPpid, string sMdln, string sSoftRev, long nCount, string[] psCCode, long[] pnParamCount, string[] psParamNames)
        {//S7F26(HE) Formatted Process Program Data (FPD)
         //SECS Message인 S7F26(Formatted Process Program Data (FPD))을 Host에서 받았을 경우 발생하는 event입니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPFmt : Ppid({0})", sPpid);
            Log(sLog);
        }

        public void OnGEMRspPPFmtVerification(string sPpid, long nResult)
        {//S7F28(H->E) Process Program Verification Acknowledge (PVA)
         //SECS Message인 S7F28(Process Program Verification Acknowledge (PVA)을 Host에서 받았을 경우 발생하는 event입니다.
         //이 event는 GEMReqPPFmtVerification() method(S7F27)에 대한 응답으로 사용합니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspPPFmtVerification : Result({0})", nResult);
            Log(sLog);
        }

        public void OnGEMTerminalMessage(long nTid, string sMsg)
        {//S10F3(H->E) Terminal Display, Single (VTN)
         //SECS Message인 S10F3(Terminal Display, Single (VTN))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S10F3에 응답 message인 S10F4는 XGemPro Process에서 Host로 보고하며 application에서 사용해야 하는 응답 method는 없습니다

            string[]   psMsg   = new string  [1];

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMTerminalMessage : Tid({0}), Msg({1})", nTid, sMsg);
            Log(sLog);

            psMsg[0] = sMsg;

            //cDEF.GEM.TerminalReceived(1, psMsg);
        }

        public void OnGEMTerminalMultiMessage(long nTid, long nCount, string[] psMsg)
        {//S10F5(H->E) Terminal Display, Multi-Block (VTN)
         //SECS Message인 S10F5(Terminal Display, Multi-Block (VTN))을 Host에서 받았을 경우 발생하는 event입니다.
         //handler 내에서는 수신한 S10F5에 응답 message인 S10F6는 XGemPro Process에서 Host로 보고하며 application에서 사용해야 하는 응답 method는 없습니다.

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMTerminalMultiMessage : Tid({0}), ", nTid);
            Log(sLog);

            for (int i = 0; i < nCount; i++)
            {
                sLog = String.Format("               B: {0}", psMsg[i]);
                Log(sLog);
            }

            //cDEF.GEM.TerminalReceived(nCount, psMsg);
        }

        public void OnGEMSpoolStateChanged(long nState, long nLoadState, long nUnloadState, string sFullTime, long nMaxTransmit, long nMsgNum, long nTotalNum, long nTransmitFail)
        {//XGemPro Process 내에서 Spool State 상태 변경 시 발생하는 event입니다


            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMSpoolStateChanged => State:{0}, LoadState:{1}, UnloadState:{2}, FullTime:{3}, MaxTransmit:{4}, MsgNum:{5}, TotalNum:{6}, TransmitFail:{7}", 
                            nState, nLoadState, nUnloadState, sFullTime, nMaxTransmit, nMsgNum, nTotalNum, nTransmitFail);
            Log(sLog);

        }

        public void OnXGEMStateEvent(long nState)
        {
            //throw new Exception("The method or operation is not implemented.");
            string szState = StrGemState(nState); 
            m_nGemState = nState;

            string sLog = String.Format("[XGEM ==> EQ] OnXGEMStateEvent:{0}", szState);
            Log(sLog);

            if (nState == (int)_eGemState.Ready || nState == (int)_eGemState.Execute)
            {
                long  [] naVid   = new long  [2];
                string[] saValue = new string[2];
                naVid[0] = 14; saValue[0] = vDEF.sPrjName;
                naVid[1] = 15; saValue[1] = cDEF.FM._sVersion;
                SetSVID(2, naVid, saValue);
                //SetECVChanged(2, naVid, saValue);
            }

            if (nState == (int)_eGemState.Execute)
            {
                SetCommunicationEnable(1); //XGemPro과 Host 간의 Communication State를 안정화 하도록 요구합니다
                sLog = String.Format("[EQ ==> XGEM] GEMSetEstablish");
                Log(sLog);
                                                                                                                                        
            }
        }

        public void OnGEMRspAllECInfo(long lCount, long[] plVid, string[] psName, string[] psValue, string[] psDefault, string[] psMin, string[] psMax, string[] psUnit)
        {//XGemPro Process에서 모든 ECID 한 정보를 요청할 경우 발생하는 event 입니다

            //throw new Exception("The method or operation is not implemented.");
            string sLog = String.Format("[XGEM ==> EQ] OnGEMRspAllECInfo");
            Log(sLog);

            for (int i = 0; i < lCount; i++)
            {
                sLog = String.Format(
                            "               Vid:{0}, Name:{1}, Value:{2}, Default:{3}, Min:{4}, Max:{5}, Unit:{6},", 
                                plVid[i], psName[i], psValue[i], psDefault[i], psMin[i], psMax[i], psUnit[i]);
                Log(sLog);
            }
        }

        #endregion


        #region 1. Initialize and Start...
        public bool Initialize(string sCfgFile)
        {
            long nReturn = m_XGem.Initialize( sCfgFile );    
	        if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] XGem initialized successfully ({0})", nReturn);
            }
            else {
                Log("[EQ ==> XGEM] Fail to initialize XGem ({0})", nReturn);
            }

            m_bSecsInit = (nReturn == 0);    
            return m_bSecsInit;

        }
    
        public bool OnStart(string sCfgFile = "")
        {
            if (!m_bSecsInit && sCfgFile != "") {
                if(!Initialize(sCfgFile)) return false;
                }

            long nReturn = 0; //m_XGem.Start(); 
            if (nReturn == 0)
            {
                m_bSecsStart = true;
                Log("[EQ ==> XGEM] XGem started successfully ({0})", nReturn);
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to start XGem ({0})", nReturn);
            }
            m_bSecsStart =  (nReturn == 0);
            return m_bSecsStart;
        }

        public bool OnStopGem()
        {
  
            long nReturn = m_XGem.Stop(); 
            if (nReturn == 0)
            {
                m_bSecsStart     = false;
                m_nCommState     = (long)_eCommState.CommDisabled;
                m_nControlState  = (long)EN_CONTROL_STATE.UNKOWN ;

                Log("[EQ ==> XGEM] XGem stopped successfully ({0})", nReturn);
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to stop XGem ({0})", nReturn);
            }
            return (nReturn == 0);

        }

        public bool OnStop()
        {
            
            OnStopGem();

            long nReturn = m_XGem.Close(); 
            if (nReturn == 0)
            {
                m_bSecsInit = false;
                //Log("[EQ ==> XGEM] XGem closed successfully ({0})", nReturn);
            }
            else
            {
                //Log("[EQ ==> XGEM] Fail to close XGem ({0})", nReturn);
            }
            return (nReturn == 0);

        }
        public bool IsReady     ()//Xgem Status 확인
        {
            if (!m_bSecsInit                                             ) return false; 
            if (!m_bSecsStart                                            ) return false; 
            if ( m_nCommState   != (int)_eCommState   .Communicating     ) return false; 
            if ( m_nGemState    != (int)_eGemState    .Ready  && 
                 m_nGemState    != (int)_eGemState    .Execute           ) return false; 

            return true;
        }
        #endregion
        

        #region 2. Communication State...
        public bool SetCommunicationEnable(long nStat)
        {//XGemPro과 Host 간의 Communication State를 안정화 하도록 요구합니다.

            //Argument : bState value(0: disable, 1: enable)
            long nReturn = m_XGem.GEMSetEstablish(nStat); 
	        if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] GEMSetEstablish successfully ({0})", nReturn);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetEstablish ({0})", nReturn);
            }
            return (nReturn == 0);

        }
        #endregion


        #region 3. Control State...
        public bool ReqControlStatusChange(long nStat, bool bSndFlag = true)
        {
            //nState = 1 OffLine
            //nState = 2 Attempt OnLine
            //nState = 3 Host OffLine
            //nState = 4 Online-Local
            //nState = 5 Online-Remote

            m_nControlState = nStat;
	        if(m_nPrvCtrlState != nStat){
		        m_nPrvCtrlState = nStat;
                m_bControlStatChange = true;
	        }
            if(!bSndFlag) return true;
                            
            long nReturn = 0;
            string szState = StrControlState(nStat);

                 if(nStat == 1) nReturn = m_XGem.GEMReqOffline    (); 
            else if(nStat == 3) nReturn = m_XGem.GEMReqHostOffline(); 
            else if(nStat == 4) nReturn = m_XGem.GEMReqLocal      (); 
            else if(nStat == 5) nReturn = m_XGem.GEMReqRemote     (); 

	        if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] ControlStatusChange[{0}] successfully ({1})", szState, nReturn);
            }
            else {
                Log("[EQ ==> XGEM] Fail to ControlStatusChange [{0}] ({1})", szState, nReturn);
            } 
            return (nReturn == 0);

        }

        #endregion

        #region 4. Process State...
        public void SetProcessingState(long nState)
        {
            if(nState == -1)  nState = m_nProcessState;
            if(nState != m_nProcessState)
            {
                m_bProcessStatChange = true;
                m_nPrvProcessState  = m_nProcessState;
            }
            m_nProcessState     = nState;


        }
        #endregion

        #region 5. Event Notification...
        public bool SetEvent(long nEventId)
        {//Host로 Event를 발생시킬 때 사용합니다.
            //Update variable            
            long nReturn = m_XGem.GEMSetEvent(nEventId);
            if (nReturn  == 0)
            {
                Log("[EQ ==> XGEM] XGem GEMSetEvent[{0}] successfully ({1})", nEventId, nReturn);
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to GEMSetEvent[{0}] ({0})", nEventId, nReturn);
            }
            return (nReturn == 0);
        }

        public bool SetEventEnable(long nCount, long[] naCEID, long Enable)
        {//설비에서 각각의 CEID에 대한 Event Enabled 속성을 변경을 하고자 할 때 사용한다. n개의 CEID를 동시에 Enable or Disable로 변경할 수 있다.

            string sLog;
            long nReturn = m_XGem.GEMSetEventEnable(nCount, naCEID, Enable);
            if (nReturn  == 0)
            {
                for(int i=0; i<nCount; i++)
                {
                    sLog = String.Format("[EQ ==> XGEM] GEMSetEventEnable{0} Enable:{1} = {2}", naCEID[i], Enable, nReturn);
                    Log(sLog);
                }
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to GEMSetEventEnable ({0})", nReturn);
            }
            return (nReturn == 0);

        }
        #endregion


        #region 7. Process Program...
        public bool SetPPChanged(long nMode, string sPpid, byte[] psBody)
        {//이 함수는 UnFormated Process Program이 생성, 삭제, 수정이 되었을 때 호출 합니다.
         //GEMSetPPChanged() method 호출하면 Host로 Process Program Changed Event를 발생 시킵니다.

	        //nMode : 1(Created), 2(Edited), 3(Deleted)
            long nSize   = psBody.Length; 
            long nReturn = m_XGem.GEMSetPPChanged(nMode, sPpid, nSize, psBody);
	        if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] GEMSetPPChanged successfully");
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetPPChanged ({0})", nReturn);
            }
            return (nReturn == 0);
        }

        public bool SetPPFmtChanged(long nMode, string sPpid, string sMdln, string sSoftRev, string[] saCCodes, long[]	naPCount, string[] saPNames)
        {//이 함수는 Formated Process Program이 생성, 삭제, 수정이 되었을 때 호출 합니다.

	        // Description: Formatted Process Program이 생성, 수정, 삭제되었을 경우 사용함.
	        // 주의 사항 : psParamNames는 2차원 배열 형태 정보를 1차원 배열로 나열한 것입니다.
	        // nCount만큼 할당되어야 하는 인자 : psCCode, pnParamCount
	        // pnParamCount 배열 값을 모두 더한 만큼 할당되어야 하는 인자 : psParamNames

 	        //nMode : 1(Created), 2(Edited), 3(Deleted)
            long nCount  = saCCodes.Length; 
	        long nReturn = m_XGem.GEMSetPPFmtChanged(nMode, sPpid, sMdln, sSoftRev, nCount, saCCodes, naPCount, saPNames);
	        if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] GEMSetPPFmtChanged successfully");
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetPPFmtChanged ({0})", nReturn);
            } 

            return (nReturn == 0);
        }


        public bool ReqPPLoadInquireS7F1(string sPpid, long nLength)
        {//SECS Message에서 S7F1(Process Program Load Inquire (PPI))로 Host로 보고할 때사용을 합니다.
         //GEMReqPPLoadInquire() method를 호출하면 Host로 S7F1을 보고하며 Host에서응답message인 S7F2를 받으면 XGemPro control에서 GEMRspPPLoadInquire() event가 발생됩니다.

	        long nReturn = m_XGem.GEMReqPPLoadInquire(sPpid, nLength);
	        if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] GEMReqPPLoadInquire successfully");
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMReqPPLoadInquire ({0})", nReturn);
            } 
            return (nReturn == 0);
        }
    
        public bool ReqPPSendS7F3(string sPpid, byte[] psBody)
        {//SECS Message에서 S7F3(Process Program Send (PPS))로 사용을 합니다.
         //GEMReqPPSend() method를 호출하면 Host로 S7F3을 보고하며 Host에서응답message인 S7F4를 받으면 XGemPro control에서 GEMRspPPSend() event가 발생됩니다.


            long nReturn = m_XGem.GEMReqPPSend(sPpid, psBody);
	        if( nReturn == 0 ) {
                Log("[EQ ==> XGEM] GEMReqPPSend successfully");
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMReqPPSend ({0})", nReturn);
            }
            return (nReturn == 0);
       }

       public bool ReqPPS7F5(string sPpid)
       {//S7F5(H<-E) Process Program Request (PPR)
        //SECS Message에서 S7F5(Process Program Request (PPR))로 사용을 합니다.
        //GEMReqPP() method를 호출하면 Host로 S7F5을 보고하며 Host에서응답message인 S7F6를 받으면 XGemPro control에서 GEMRspPP() event가 발생됩니다.


	       long nReturn = m_XGem.GEMReqPP(sPpid);
	       if( nReturn == 0 ) {
               Log("[EQ ==> XGEM] GEMReqPP successfully");
           }
           else {
               Log("[EQ ==> XGEM] Fail to GEMReqPP ({0})", nReturn);
           }
           return (nReturn == 0);
       }

       public bool ReqPPFmtSendS7F23(long nMode, string sPpid, string sMdln, string sSoftRev, string[] saCCodes, long[]	naPCount, string[] saPNames)
       {//S7F23(H<-E) Formatted Process Program Send (FPS)
        //GEMReqPPFmtSend() method를 호출하면 Host로 S7F23을 보고하며 Host에서응답message인 S7F24를 받으면 XGemPro control에서 GEMRspPPFmtSend() event가 발생됩니다.

	        // Description: Formatted Process Program이 생성, 수정, 삭제되었을 경우 사용함.
	        // 주의 사항 : psParamNames는 2차원 배열 형태 정보를 1차원 배열로 나열한 것입니다.
	        // nCount만큼 할당되어야 하는 인자 : psCCode, pnParamCount
	        // pnParamCount 배열 값을 모두 더한 만큼 할당되어야 하는 인자 : psParamNames

 	       //nMode : 1(Created), 2(Edited), 3(Deleted)
           long nCount  = saCCodes.Length; 
	       long nReturn = m_XGem.GEMReqPPFmtSend(sPpid, sMdln, sSoftRev, nCount, saCCodes, naPCount, saPNames);
	       if( nReturn == 0 ) {
               Log("[EQ ==> XGEM] GEMReqPPFmtSend successfully");
           }
           else {
               Log("[EQ ==> XGEM] Fail to GEMReqPPFmtSend ({0})", nReturn);
           }

           return (nReturn == 0);
       }

       public bool GEMReqPPFmtS7F25(string sPpid)
       {//S7F25(H<-E) Formatted Process Program Request (FPR)
        //SECS Message에서 S7F25(Formatted Process Program Request (FPR))로 사용을 합니다.


	       long nReturn = m_XGem.GEMReqPPFmt(sPpid);
	       if( nReturn == 0 ) {
               Log("[EQ ==> XGEM] GEMReqPPFmt successfully");
           }
           else {
               Log("[EQ ==> XGEM] Fail to GEMReqPPFmt ({0})", nReturn);
           }
           return (nReturn == 0);
       }

       public bool ReqPPFmtVerificationS7F27(string sPpid, long[] naAck, string[] saSeqNo, string[] saError)
       {//S7F27(H<-E) Process Program Verification Send (PVS)
        //SECS Message에서 S7F27(Process Program Verification Send (PVS))로 사용을 합니다.

           long nCount  = naAck.Length; 
	       long nReturn = m_XGem.GEMReqPPFmtVerification(sPpid, nCount, naAck, saSeqNo, saError);
	       if( nReturn == 0 ) {
               Log("[EQ ==> XGEM] GEMReqPPFmtVerification successfully");
           }
           else {
               Log("[EQ ==> XGEM] Fail to GEMReqPPFmtVerification ({0})", nReturn);
           }
           return (nReturn == 0);
       }
       #endregion


        #region 14. Alarm Management...
        public bool SetAlarm(long nAlarmID, long nSet)
        {//S5F1(H<-E) Alarm Report Send(ARS)
         //Equipment 에서 Alarm 발생시 Alarm 정보를 XGemPro 으로 전송한다
         //XGemPro에서 S5F1을 Host로 보고하며 Alarm Detect Event 및 Alarm Clear Event가발생됩니다.

           string sLog; 
 	       long nReturn = m_XGem.GEMSetAlarm(nAlarmID, nSet);
 	       if( nReturn == 0 ) {
                sLog = String.Format("[EQ ==> XGEM] GEMSetAlarm => ID:{0}, State:{1} ({2})", nAlarmID, 1, nReturn);
                Log(sLog);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetAlarm ({0})", nReturn);
            }
            return (nReturn == 0);
        }

        public bool SetAlarmEnable(long nCount, long[] naALID, long nEnable)
        {//설비에서 각각의 Alarm에 대한 Event Enabled 속성을 변경을 하고자 할 때 사용한다. n개의 ALID를 동시에 Enable or Disable로 변경할 수 있다.
         //주의 사항) 이 method는 EQ Application과 XGemPro Process간 동기화 처리가 되어 동작하기 때문에 사용 시 EQ Application에서 blocking 상태가 발생 할 수 있습니다.

           string sLog; 
 	       long nReturn = m_XGem.GEMSetAlarmEnable(nCount, naALID, nEnable);
 	       if( nReturn == 0 ) {
                for(int i=0;i<nCount;i++) 
                {
                    sLog = String.Format("[EQ ==> XGEM] GEMSetAlarmEnable {0} Enable:{1} = {2}", naALID[i], nEnable, nReturn);
                    Log(sLog);
                }
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetAlarmEnable ({0})", nReturn);
            }
            return (nReturn == 0);
        }


        #endregion


        #region 16. Equipment Constants...
        public bool SetECVChanged(long nCount, long[] naEcid, string[] saVals)
        {//ECID의 ECV 정보가 변경되었을 때 이 함수를 호출합니다.
         //GEMSetECVChanged() method를 호출하면 XGemPro Process는 EcId들의 Value값을 Update하고 ECVChanged Event를 발생 시킵니다.

           string sLog; 
 	       long nReturn = m_XGem.GEMSetECVChanged(nCount, naEcid, saVals);
 	       if( nReturn == 0 ) {
                sLog = String.Format("[EQ ==> XGEM] GEMSetECVChanged => Ecid:{0}, Val:{1} ({2})", naEcid[0], saVals[0], nReturn);
                Log(sLog);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetECVChanged ({0})", nReturn);
            }
            return (nReturn == 0);
        }


        public bool ReqAllECInfo()
        {//모든 ECV 정보를 가져올 때 이 함수를 호출합니다.
         //GEMReqAllECInfo () method를 호출하면 XGemPro Process는 GEMRspAllECInfo Event를 발생 시킵니다.
 
           string sLog; 
 	       long nReturn = m_XGem.GEMReqAllECInfo();
 	       if( nReturn == 0 ) {
                sLog = String.Format("[EQ ==> XGEM] GEMReqAllECInfo ({0})", nReturn);
                Log(sLog);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMReqAllECInfo ({0})", nReturn);
            }
            return (nReturn == 0);
        }
        #endregion


        #region 18. Terminal Services...
        public bool SetTerminalMessage(short nTid,  string sMsg)
        {//S10F1(H<-E) Terminal Request (TRN)
         //SECS Message에서 S10F1(Process Program Acknowledge (PPA))로 사용을 합니다.
         //GEMSetTerminalMessage() method를 호출하면 Host로 S10F1을 보고합니다.

           string sLog; 
  	       long nReturn = m_XGem.GEMSetTerminalMessage(nTid, sMsg);
 	       if( nReturn == 0 ) {
                sLog = String.Format("[EQ ==> XGEM] GEMSetTerminalMessage => Tid:{0}, Msg:{1} ({2})", nTid, sMsg, nReturn);
                Log(sLog);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetTerminalMessage ({0})", nReturn);
            }
            return (nReturn == 0);
        }

        #endregion


        #region 20. Clock...
        public bool ReqGetDateTime()
        {//S2F17(H<-E) Date and Time Request (DTR)
         //설비에서 Host로 Data and Time 요청 시 사용합니다. XGemPro에서 S2F17 Message를 Host로 보냅니다.

           string sLog; 
  	       long nReturn = m_XGem.GEMReqGetDateTime();
 	       if( nReturn == 0 ) {
                sLog = String.Format("[EQ ==> XGEM] GEMReqGetDateTime ({0})", nReturn);
                Log(sLog);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMReqGetDateTime ({0})", nReturn);
            }
            return (nReturn == 0);
        }
        #endregion


        #region Param...
        public string GetParam(string sName)
        {//설정된 Parameter 값의 결과를 반환한다
         
           string sLog; 
           string sValue = null; 
  	       long nReturn = m_XGem.GEMGetParam(sName, ref sValue);
 	       if( nReturn == 0 ) {
                sLog = String.Format("[EQ ==> XGEM] GEMGetParam => Name:{0}, Value:{1} ({2})", sName, sValue, nReturn);
                Log(sLog);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMGetParam ({0})", nReturn);
            }
            return sValue;
        }

        public bool SetParam(string sName, string sValue)
        {//Runtime시에 설정 가능한 Parameters 값을 변경할 수 있습니다.
         //[Device] ID Device ID가 하나 이상인 경우는 쉼표(,) 으로 구분하여 입력한다.
         //[IP] HSMS 커넥션에 사용할 IP 주소를 변경
         //[Port] HSMS 커넥션에 사용할 Port 주소를 변경
         //[Active] true: TCP/IP 연결시 active 모드로 변경 false: TCP/IP 연결시 passive 모드로 변경
         //[T3] T3 timeout parameter 를 변경
         //[T5] T5 timeout parameter 를 변경
         //[T6] T6 timeout parameter 를 변경
         //[T7] T7 timeout parameter 를 변경
         //[T8] T8 timeout parameter 를 변경
         //[Link Test Interval] HSMS 사용시 linktest 간격 (초)를 변경
         //[Retry Limit] SECS-I 통신의 블록 재전송 회수 또는 HSMS 메시지 응답 없을 때 재전송 회수를 변경        

           string sLog; 
  	       long nReturn = m_XGem.GEMSetParam(sName, sValue);
 	       if( nReturn == 0 ) {
                sLog = sLog = String.Format("[EQ ==> XGEM] GEMSetParam => Name:{0}, Value:{1} ({2})", sName, sValue, nReturn);
                Log(sLog);
            }
            else {
                Log("[EQ ==> XGEM] Fail to GEMSetParam ({0})", nReturn);
            }
            return (nReturn == 0);
        }
        #endregion


        #region Variable...


        public bool SetSVID(long nVid, string nValue)
        {//Variable에 대한 Value값을 Vid 로 설정하고자 할때 사용합니다.
            //Update variable    
            
            long[]   naVid   = new long  [2];
            string[] saValue = new string[2];

            naVid  [0] = nVid ;
            saValue[0] = nValue;
                        
            long nReturn = m_XGem.GEMSetVariable(1, naVid, saValue);
            if (nReturn  == 0)
            {
                Log("[EQ ==> XGEM] XGem GEMSetVariable successfully ({0})", nReturn);
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to GEMSetVariable ({0})", nReturn);
            }
            return (nReturn == 0);
        }

        public bool SetSVID(long nCount, long[] naVid, string[] saValue)
        {//Variable에 대한 Value값을 Vid 로 설정하고자 할때 사용합니다.
            //Update variable            
            long nReturn = m_XGem.GEMSetVariable(nCount, naVid, saValue);
            if (nReturn  == 0)
            {
                Log("[EQ ==> XGEM] XGem GEMSetVariable successfully ({0})", nReturn);
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to GEMSetVariable ({0})", nReturn);
            }
            return (nReturn == 0);
        }

        public bool SetNameSVID(long nCount, string[] saVidName, string[] saValue)
        {//Variable에 대한 Value를 Variable Name을 사용하여 설정 할 때 사용합니다.

            long nReturn = m_XGem.GEMSetVarName(nCount, saVidName, saValue);
            if (nReturn  == 0)
            {
                Log("[EQ ==> XGEM] XGem GEMSetVarName successfully ({0})", nReturn);
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to GEMSetVarName ({0})", nReturn);
            }
            return (nReturn == 0);

        }

        public bool GetVarName(long nCount, ref string[] saVidName, ref string[] saValue)
        {//Variable에 대한 Value값을 Varialbe Name으로 얻고자 할때 사용합니다.

            long nReturn = m_XGem.GEMGetVarName(nCount, ref saVidName, ref saValue);
            if (nReturn  == 0)
            {
                Log("[EQ ==> XGEM] XGem GEMGetVarName successfully ({0})", nReturn);
            }
            else
            {
                Log("[EQ ==> XGEM] Fail to GEMGetVarName ({0})", nReturn);
            }
            return (nReturn == 0);

        }
        #endregion

        #region User defined message ...
        #endregion


        
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
    }


}
