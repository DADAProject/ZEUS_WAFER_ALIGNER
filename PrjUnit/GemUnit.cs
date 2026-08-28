using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoModule
{

    public enum EN_GEM_MAX : int {
        VID_ITEM        = 50 ,
        CMD_ITEM        = 50 ,
        EVENT_MSG       = 100,
    }
    /***************************************************************************/
    /* EVENT REPORT ID MAPPING(CEID) - EVENT ID                                */
    /***************************************************************************/
    public enum EN_CEID {
        ControlStatus               = 1001,
        MachineStatus               = 1002,
        Port_Load_Compleate         = 1012,
        Port_Unload_Request         = 1013,
        BCR_Read_Complete           = 1021,
        WaferLoadingCompleate       = 1022,
        WaferUnloadCompleated       = 1023,
        ProcessStart                = 1041,
        ProcessEnd                  = 1042,
        ECID_Change                 = 1075,
        Probe_Spec_Request          = 1100,
        Probe_Result_Report         = 1200,
        EndofId                 

    }

    
    /***************************************************************************/
    /* Remote Command ID                                                       */
    /***************************************************************************/
    public enum EN_RCMD_ID {
         NONE            = 0,   
         CANCEL          = 1,
         LOTINFO         = 2,
         PAUSE           = 3,
         PPSELECT        = 4,
         RESUME          = 5,
         START           = 6,
         STOP            = 7,
         WAFERUNLOAD     = 8,
         EndofId
    }

    /***************************************************************************/
    /* VID MAPPING 
    /***************************************************************************/
    public enum EN_SVID {
        ControlState              = 201,
        EQState                   = 203,
        AlarmState                = 204,
        PPID                      = 206,
        Port_Id                   = 207,
        PortState                 = 208,
        LotID                     = 223,
        WaferID                   = 224,
        SlotNo                    = 225,
        AlarmCode                 = 230,
        AlarmText                 = 232,
        CurrentTime               = 233,
        TestReceipeName           = 300,
        EsdReceipeMapId           = 301,
        CassetteID                = 302,
        TotalCount                = 304,	
        GoodCount                 = 305,	
        FailCount                 = 306,	
        Yield                     = 307,	
        ReportBin                 = 601,	
        ReportQty                 = 602,	
        MedainName                = 603,	
        MedainValue               = 604,	
        GoodNgName                = 605,	
        GoodNgValue               = 606,	
        SpecItemList              = 607,	
        EndofId
    }

    //---------------------------------------------------------------------------
    public enum EN_GEM_CMD_TYPE {
        Default                     = 0 ,
        LOAD_PORT_STATUS_CHANGE     = 1 ,
        UNLOAD_PORT_STATUS_CHANGE   = 2 ,

    }

    /***************************************************************************/
    /* Struct Define                                                           */
    /***************************************************************************/
    public struct ST_SVID_BUFF {
       public int        iType   ;
       public long       lSVID   ;
       public string     strValue;
    };

    public struct ST_ECID_SET  {
        public long       lECID   ;
        public string     sECNAME ;
        public string     sECMIN  ;
        public string     sECMAX  ;
        public string     sECDEF  ;
        public string     sUNITS  ;
    };

    public struct ST_RCMD_START  {

        public string PPID           ;
        public string LOTID          ;
        public string PARTID         ;
        public string SLOTMAP        ;
        public string PRODUCTIONTYPE ;
        public string IN_MAP_IP      ;
        public string TESTMODE       ;
        public string FILEPATH       ;
        public string WAFER          ;

        public string ITEM_RECIPE    ;
        public string BIN_RECIPE     ;

    };


    public struct ST_RCMD_PPSELECT  {
        public string PPID  ;
        public string LOTID ;
        public string PORTID;
        public string WAFER ;
    };



    /***************************************************************************/
    /* Data Class                                                              */
    /***************************************************************************/
    public class TEVT_SET {
        public long     nCEID  ;
        public long     nCount ; 
        public long  [] nRPID   = new long  [(int)EN_GEM_MAX.VID_ITEM];
        public long  [] nVID    = new long  [(int)EN_GEM_MAX.VID_ITEM];
        public string[] sValue  = new string[(int)EN_GEM_MAX.VID_ITEM];


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TEVT_SET()
        {
            ResetData();
        }
        ~TEVT_SET() { }

        public void ResetData()
        {
            nCEID  = 0;
            nCount = 0;
            for(int i=0;i<(int)EN_GEM_MAX.VID_ITEM;i++)
            {
                nRPID  [i] = 0;
                nVID   [i] = 0;
                sValue [i] = "";
            }

        }
    };



    public class TGemUnit
    {

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */



        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */

        //Timer
        TOnDelayTimer    m_TimerEVTSend  = new TOnDelayTimer();
        TOnDelayTimer    m_EvtDelayTimer = new TOnDelayTimer();
        TOnDelayTimer    m_tFDCUpdate    = new TOnDelayTimer();


        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool[]           m_bRcvRCMD = new bool [(int)EN_RCMD_ID.EndofId];


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int               m_iEVTStep         ;
        long              m_nPreNorCEID      ;
        bool              m_bSending         ;
        EN_RCMD_ID        m_iLastRcvRCMD     ;




        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class & struct
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
        TEVT_SET                 _SndEVT         = new TEVT_SET        ();
        TEVT_SET                 _TempEVT        = new TEVT_SET        ();
        public ST_RCMD_START     _RCMD_START     = new ST_RCMD_START   ();
        public ST_RCMD_PPSELECT  _RCMD_PPSELECT  = new ST_RCMD_PPSELECT();

        //TQueueCls<TEVT_SET  >    m_EvtQue        = new TQueueCls<TEVT_SET>   (512);

        public List<TEVT_SET>    m_EvtList       = new List<TEVT_SET>();

        

                             
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TGemUnit()
        {
            Init();    
        }
        ~TGemUnit() { }


        //Base Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init          ()
        {
            m_iEVTStep = 0;
            m_bSending = false; 


            //m_EvtQue.Clear();
            m_EvtList.Clear(); //THP(190118)

        }
        public void Close         ()
        {
            cDEF.GemDll.OnStop();
        }
 
        public bool  StartXGem         ()     //XGem을 Start 한다.
        {
            //return cDEF.GemDll.OnStart("EQ.cfg");

            return true;
        }
        public void       StopXGem          ()     //XGem을 종료한다. (강제종료시 사용)
        {
            cDEF.GemDll.OnStop();
        }

        //Status
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsGemRdy     ()//Xgem Status 확인
        {
            return cDEF.GemDll.IsReady();
        }
        public void ClsRcvRcmd(EN_RCMD_ID iRcmd = EN_RCMD_ID.NONE)
        {
            if(iRcmd ==  EN_RCMD_ID.NONE)
            {
                for(int i=0;i<(int)EN_RCMD_ID.EndofId;i++)
                    m_bRcvRCMD[i] = false;
                return;
            }
            if(iRcmd<0 || iRcmd>=EN_RCMD_ID.EndofId) return;
            m_bRcvRCMD[(int)iRcmd] = false;
        }
        public void SetRcvRcmd(EN_RCMD_ID iRcmd)
        {
            if(iRcmd<0 || iRcmd>=EN_RCMD_ID.EndofId) return;
            m_bRcvRCMD[(int)iRcmd] = true;
        }
        public bool IsRcvRcmd(EN_RCMD_ID iRcmd)
        {
            if(iRcmd<0 || iRcmd>=EN_RCMD_ID.EndofId) return false;
            return m_bRcvRCMD[(int)iRcmd];

        }

        //SVID Process Functions
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  SETVAL (EN_SVID nVidNo, object  Data, bool bFirst = false)
        {
            string sValue;
            if (bFirst                                      ) _TempEVT.ResetData();
            if (_TempEVT.nCount >= (int)EN_GEM_MAX.VID_ITEM ) return;

            sValue = Convert.ToString(Data);
            if(sValue == "") sValue = " ";

            _TempEVT.nVID     [_TempEVT.nCount] = (long)nVidNo;
            _TempEVT.sValue   [_TempEVT.nCount] = sValue      ;

            _TempEVT.nCount ++;
        }

        //ALARM Process Functions. (S5Fx 관련)
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   GoSendALARM       (int iAlarmNo, bool bSet = true)
        {
            return cDEF.GemDll.SetAlarm(iAlarmNo, (bSet) ? 1 : 0);
        }

        //EVENT Process Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool PushEVT (EN_CEID iNo)
        {
            if (iNo  == 0  ) return false;
            _TempEVT.nCEID = (long)iNo;
            
            
            //m_EvtQue.Push(_TempEVT);
            m_EvtList.Add(_TempEVT); //THP(190118)

            return true;
        }


        public bool GoSendEvt         ()
        {

            TEVT_SET  EVTData = m_EvtList[0]; 
            m_EvtList.RemoveAt(0);  

            //if (!IsXGemRdy()) return false;
            if(!cDEF.GemDll.SetSVID(EVTData.nCount, EVTData.nVID, EVTData.sValue)) return false;
            //Send Event
            return cDEF.GemDll.SetEvent(EVTData.nCEID);

        }

        void       UpdateEvt         ()
        {
            //Check Sending Flag.
            if (m_TimerEVTSend.OnDelay(m_bSending , 1000)) {
                m_bSending = false;
                m_TimerEVTSend.Clear();
                m_iEVTStep = 0;
                return;
                }

            //Send Event.
            switch (m_iEVTStep) {
                default: m_iEVTStep = 0;
                         m_EvtDelayTimer.Clear();
                         return;

                case  0: //Check Sending.
                         if (m_bSending  ) { m_iEVTStep = 0; return; }
                         //Check Empty.
                         if (m_EvtList.Count <=0) { m_iEVTStep = 0; return; } //THP(190118))
                         m_bSending = true;
                         m_EvtDelayTimer.Clear();
                         m_iEVTStep++;
                         return;

                case  1: if(!m_EvtDelayTimer.OnDelay(true, 100)) return ;
                         m_EvtDelayTimer.Clear();
                         GoSendEvt();
                         m_iEVTStep++;
                         return;

                case  3: if(!m_EvtDelayTimer.OnDelay(true, 200)) return ;
                         m_EvtDelayTimer.Clear();
                         m_iEVTStep = 0;
                         return;
                }

        }

        //Update Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        void UpdateFDC         ()
        {

        }


        public void   Update()
        {
            if(cDEF.GemDll._bControlStatChange) SendCEID(EN_CEID.ControlStatus);
            if(cDEF.GemDll._bProcessStatChange) SendCEID(EN_CEID.MachineStatus);
            
            UpdateEvt      (); //Event Process
            UpdateFDC      (); 

        }

        //EStablish Communications(Power ON)
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //[HOST]  [EXGEM]     <-[EQ]initialize()
        //[HOST]  [EXGEM]     <-[EQ]Start()
        //[HOST]  [EXGEM]     <-[EQ]GEMSetParam()
        //[HOST]  [EXGEM]     <-[EQ]GEMSetVariable()
        //[HOST]  [EXGEM]     <-[EQ]GEMEQInitialized()
        //[HOST]  [EXGEM]     <-[EQ]GEMSetEstablish()
        //[HOST]->[EXGEM]S1F13  [EQ]
        //[HOST]<-[EXGEM]S1F14  [EQ]
        //[HOST]<-[EXGEM]S1F13  [EQ]
        //[HOST]->[EXGEM]S1F14  [EQ]
        //[HOST]  [EXGEM]     ->[EQ]GEMCommStateChanged()

        //Status Changed
        //[HOST]  [EXGEM]    <-[EQ]GEMReqOffLind()
        //[HOST]  [EXGEM]    ->[EQ]GETControlStateChanged

        //[HOST]->[EXGEM]S1F1   [EQ] AreYouThere Request
        //[HOST]<-[EXGEM]S1F2   [EQ] OnLineData(설비 명및 Version 전송)

        //[HOST]->[EXGEM]S1F3   [EQ] EQUIPMENT STATUS REQUEST     - 설비 상태 요청 및 SVID에 저장된 DATA 요청
        //[HOST]  [EXGEM]S1F4 <-[EQ] EQUIPMENT STATUS DATA        - 설비 상태 및 SVID 저장된 DATA 전송

        //[HOST]->[EXGEM]S2F37  [EQ] Enable Disable EventReport   - 사용할 EVENT REPORT(CEID) 서버에서 설정
        //[HOST]  [EXGEM]S2F38<-[EQ] EnableDisableEventReportAck  - ACK 전송

        //[HOST]->[EXGEM]S2F33  [EQ] DefineReport                 - 사용할 VID를 묶에서  Roport ID로 설정
        //[HOST]  [EXGEM]S2F34<-[EQ] DefineReportAck              - ACK 전송

        //[HOST]->[EXGEM]S2F35  [EQ] LinkEventReport              - EVENT ID와 Report ID 연결
        //[HOST]  [EXGEM]S2F36<-[EQ] LinkEventReportAck           - ACK 전송

        public void TerminalReceived(long nCount, string[] psMsg)
        {//Terminal 메시지 처리 


        }
        public long RCMDReceived(string sRcmd, long nCount, string[] psNames, string[] psVals)
        {//Remote Command 처리 
            int i;
            int iPosSlsh;
            m_iLastRcvRCMD = EN_RCMD_ID.NONE;
            for(i=0;i<(int)EN_RCMD_ID.EndofId;i++)
            {
                if(sRcmd.Trim() == Enum.GetName(typeof(EN_RCMD_ID),i).Trim())  { m_iLastRcvRCMD = (EN_RCMD_ID)i; break; }
            }

            long nACK = 0;
            switch(m_iLastRcvRCMD)
            {
                case EN_RCMD_ID.LOTINFO:
                     break;
           
                case EN_RCMD_ID.START:
                    for(i=0 ; i<nCount ; i++){ 
                        if(psNames[i].IndexOf("PPID"          ) >= 0) _RCMD_START.PPID           = psVals[i]; 
                        if(psNames[i].IndexOf("LOTID"         ) >= 0) _RCMD_START.LOTID          = psVals[i]; 
                        if(psNames[i].IndexOf("PARTID"        ) >= 0) _RCMD_START.PARTID         = psVals[i];
                        if(psNames[i].IndexOf("PRODUCTIONTYPE") >= 0) _RCMD_START.PRODUCTIONTYPE = psVals[i];
                        if(psNames[i].IndexOf("TESTMODE"      ) >= 0) _RCMD_START.TESTMODE       = psVals[i];
                        if(psNames[i].IndexOf("FILEPATH"      ) >= 0) _RCMD_START.FILEPATH       = psVals[i];
                        }

                     
                     if(_RCMD_START.PPID != "")
                     {
                        iPosSlsh = _RCMD_START.PPID.IndexOf("/") ;
                        if(iPosSlsh < 0) break;
                        _RCMD_START.ITEM_RECIPE = _RCMD_START.PPID.Substring(0, iPosSlsh);
                        _RCMD_START.BIN_RECIPE  = _RCMD_START.PPID.Substring(iPosSlsh+1);
                     }

                     break;

                case EN_RCMD_ID.PPSELECT:
                    for(i=0 ; i<nCount ; i++){ 
                        if(psNames[i].IndexOf("PPID"          ) >= 0) _RCMD_PPSELECT.PPID   = psVals[i]; 
                        if(psNames[i].IndexOf("LOTID"         ) >= 0) _RCMD_PPSELECT.LOTID  = psVals[i]; 
                        if(psNames[i].IndexOf("PORTID"        ) >= 0) _RCMD_PPSELECT.PORTID = psVals[i];
                        if(psNames[i].IndexOf("WAFER"         ) >= 0) _RCMD_PPSELECT.WAFER  = psVals[i];
                        }
                     break;

                case EN_RCMD_ID.WAFERUNLOAD:
                     break;

                case EN_RCMD_ID.PAUSE:
                     break;

                case EN_RCMD_ID.RESUME:
                     break;

                case EN_RCMD_ID.STOP:
                     break;

                case EN_RCMD_ID.CANCEL:
                     break;
            }

            SetRcvRcmd(m_iLastRcvRCMD);
            return nACK;
        }

        public long GetPortStat(int iMgzPort, bool bExtMgz)
        {//0=idle, 1=Busy, 2=Run
            long nPortStat   = 0;
            bool isMGZ_Wait  =  cDEF.DM.MGZ  [iMgzPort].IsOneStat  (EN_UNIT_STAT.Wait  );
            if(bExtMgz) {
                if(isMGZ_Wait) nPortStat = 2;
                else           nPortStat = 1;
            }
            return nPortStat;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Send Event Function
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SendCEID		     (EN_CEID nCEID, params object[] sData)	
        {//Send Event 처리
            
/*
2019-01-11 10:34:57.140] SEND SSR:S1F3 W SystemBytes=3:
                               <L,3 [SVIDCOUNT]
                                   <U2,1 '201' [SVID]>
                                   <U2,1 '203' [SVID]>
                                   <U2,1 '208' [SVID]>
                               >.

[2019-01-11 10:34:57.156] RECV SSD:S1F4 SystemBytes=3:
                               <L,3 [SVCOUNT]
                                   <L,0>
                                   <L,0>
                                   <L,0>
                               >.
*/
            long nStat = 0;
                
            switch(nCEID)
            {
                case EN_CEID.ControlStatus:

                    //0=offline, 1=local, 2=remote
                         if (cDEF.GemDll._nControlState == (long)EN_CONTROL_STATE.ONLINE_LOCAL ) nStat = 1;
                    else if (cDEF.GemDll._nControlState == (long)EN_CONTROL_STATE.ONLINE_REMOTE) nStat = 2;
                    else                                                                         nStat = 0;

                     SETVAL(EN_SVID.ControlState, nStat , true);
                     //
                     SETVAL(EN_SVID.EQState      , cDEF.GemDll._nProcessState);
                     SETVAL(EN_SVID.Port_Id      , GetPortStat((int)EN_MGZ_ID.WMG, cDEF.SEQ.WMG.isStatExtMgz()));

                     break;
                case EN_CEID.MachineStatus:

                         if (cDEF.GemDll._nProcessState == (int)EN_SEQ_STAT.Error  ) nStat = 4;
                    else if (cDEF.GemDll._nProcessState == (int)EN_SEQ_STAT.Idle   ) nStat = 2;
                    else if (cDEF.GemDll._nProcessState == (int)EN_SEQ_STAT.Running) nStat = 1;
                    else if (cDEF.GemDll._nProcessState == (int)EN_SEQ_STAT.RunWarn) nStat = 1;
                    else                                                             nStat = 2;

                     SETVAL(EN_SVID.EQState     , nStat, true);
                     //
                     SETVAL(EN_SVID.ControlState, cDEF.GemDll._nControlState);
                     SETVAL(EN_SVID.Port_Id     , GetPortStat((int)EN_MGZ_ID.WMG, cDEF.SEQ.WMG.isStatExtMgz()));
                     
                     break;
                case EN_CEID.Port_Load_Compleate:
                     
                     SETVAL(EN_SVID.Port_Id      , sData[0] , true);
                     break;
                case EN_CEID.Port_Unload_Request:
                     SETVAL(EN_SVID.Port_Id      , sData[0] , true);
                     break;
                case EN_CEID.BCR_Read_Complete:
                     SETVAL(EN_SVID.LotID        , sData[0] , true);
                     SETVAL(EN_SVID.WaferID      , sData[1]       );
                     SETVAL(EN_SVID.Port_Id      , sData[2]       );
                     SETVAL(EN_SVID.SlotNo       , sData[3]       );
                     break;
                case EN_CEID.WaferLoadingCompleate:
                     SETVAL(EN_SVID.LotID        , sData[0] , true);
                     SETVAL(EN_SVID.WaferID      , sData[1]       );
                     SETVAL(EN_SVID.Port_Id      , sData[2]       );
                     SETVAL(EN_SVID.SlotNo       , sData[3]       );
                     break;
                case EN_CEID.WaferUnloadCompleated:
                     SETVAL(EN_SVID.LotID        , sData[0] , true);
                     SETVAL(EN_SVID.WaferID      , sData[1]       );
                     SETVAL(EN_SVID.Port_Id      , sData[2]       );
                     SETVAL(EN_SVID.SlotNo       , sData[3]       );
                     SETVAL(EN_SVID.TotalCount   , sData[4]       );
                     SETVAL(EN_SVID.GoodCount    , sData[5]       );
                     SETVAL(EN_SVID.FailCount    , sData[6]       );
                     SETVAL(EN_SVID.Yield        , sData[7]       );
                     break;
                case EN_CEID.ProcessStart:
                     SETVAL(EN_SVID.LotID        , sData[0] , true);
                     SETVAL(EN_SVID.PPID         , sData[1]       );
                     SETVAL(EN_SVID.Port_Id      , sData[2]       );
                     break;
                case EN_CEID.ProcessEnd:
                     SETVAL(EN_SVID.LotID        , sData[0] , true);
                     SETVAL(EN_SVID.PPID         , sData[1]       );
                     SETVAL(EN_SVID.Port_Id      , sData[2]       );
                     break;
                case EN_CEID.Probe_Spec_Request:
                     SETVAL(EN_SVID.Port_Id     , sData[0] , true);
                     SETVAL(EN_SVID.PPID        , sData[1]       );
                     SETVAL(EN_SVID.LotID       , sData[2]       );
                     break;                     

                 case EN_CEID.Probe_Result_Report:
                     SETVAL(EN_SVID.LotID           , sData[0] , true);
                     SETVAL(EN_SVID.Port_Id         , sData[1]       );
                     SETVAL(EN_SVID.SlotNo          , sData[2]       );
                     SETVAL(EN_SVID.ReportBin       , sData[3]       );
                     SETVAL(EN_SVID.ReportQty       , sData[4]       );
                     SETVAL(EN_SVID.MedainName      , sData[5]       );
                     SETVAL(EN_SVID.MedainValue     , sData[6]       );
                     SETVAL(EN_SVID.GoodNgName      , sData[7]       );
                     SETVAL(EN_SVID.GoodNgValue     , sData[8]       );
                     break;
            }
            PushEVT  (nCEID);
        }
    
    }
}
