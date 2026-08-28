using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static eMachine.cDEF;

namespace eMachine
{
    /***************************************************************************/
    /* TClass                                                                  */
    /***************************************************************************/
    public abstract class TSeqUnit 
    {
        //Constructor & Destroyer.
        public TSeqUnit() 
        {
        }
        ~TSeqUnit() { }
        
        public abstract void  ClearHomeStep();
        public abstract int   GetSeqStep   ();

        //Check disturbing
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public abstract bool  CheckDstb    (EN_MOTR_ID Motr, EN_COMD_ID Cmd = EN_COMD_ID.NoneCmd, 
                                               int Step = vDEF.NONE_STEP, EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.NONE, double DirPosn = 0.0);

        // Processing.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public abstract bool  MoveHome          ();
        public abstract bool  MoveToLastWorkPosn(EN_MOTR_ID Motr);
		public abstract bool  MoveToSafetyWaitPosn();
        public abstract bool  MoveDirect        (EN_MOTR_ID Motr, double Posn);
		public abstract bool  ReqMoveDirect     (EN_MOTR_ID Motr, double Posn);

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public abstract void  Init ();
        public abstract void  Reset();
        public abstract void  ClearWorkEnd();

        //Move Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public abstract bool  MoveMotr   (EN_MOTR_ID Motr, EN_COMD_ID Cmd, EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal, 
                                              int Step = vDEF.NONE_STEP, EN_FPOSN_INDEX Index = EN_FPOSN_INDEX.NONE);
		public abstract bool  ReqMoveMotr(EN_MOTR_ID Motr, EN_COMD_ID Cmd, EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal, 
                                              int Step = vDEF.NONE_STEP, EN_FPOSN_INDEX Index = EN_FPOSN_INDEX.NONE);
        //Status
        public abstract bool  ToStopCon   ();
        public abstract bool  ToStartCon  ();
        public abstract bool  ToStart     ();
        public abstract bool  ToStop      ();
        public abstract bool  StatusRun   ();
        public abstract bool  AutoRun     ();
        public abstract bool  IsWorkEnd   ();
        public abstract void  Update      ();

        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public abstract void   Load        (BinaryReader br);
        public abstract void   Save        (BinaryWriter wr);

        //Status Update Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public abstract bool UpdateListVal (int no, out string sName, out string sValue);

    }


    /***************************************************************************/
    /*  Sequence Class                                                         */
    /***************************************************************************/
    public class TSequence
    {
        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		TOnDelayTimer   m_ToStopTimer      = new TOnDelayTimer();                 
		TOnDelayTimer   m_ToStrtTimer      = new TOnDelayTimer();                 
		TOnDelayTimer   m_FlickOnTimer1    = new TOnDelayTimer();               
		TOnDelayTimer   m_FlickOffTimer1   = new TOnDelayTimer();              
		TOnDelayTimer   m_FlickOnTimer2    = new TOnDelayTimer();               
		TOnDelayTimer   m_FlickOffTimer2   = new TOnDelayTimer();              
		TOnDelayTimer   m_FlickOnTimer3    = new TOnDelayTimer();               
		TOnDelayTimer   m_FlickOffTimer3   = new TOnDelayTimer();              
		TOnDelayTimer   m_MachineHoldTimer = new TOnDelayTimer();            
		TOnDelayTimer   m_WaitStopTimer    = new TOnDelayTimer();                
		TOnDelayTimer   m_WaitStartTimer   = new TOnDelayTimer();               
		TOnDelayTimer   m_tLevelChkTime    = new TOnDelayTimer();               
        TOnDelayTimer   m_tInspectDly      = new TOnDelayTimer();   

		TOnDelayTimer[] m_tMgzExs          = new TOnDelayTimer[(int)EN_MGZ_ID.EndOfId];               
        TOnDelayTimer[] m_tMgzNon          = new TOnDelayTimer[(int)EN_MGZ_ID.EndOfId];

        TOnDelayTimer[] m_AutoDOCtrl       = new TOnDelayTimer[10];
        TOnDelayTimer[] m_tFanAlarm        = new TOnDelayTimer[10];
        TOnDelayTimer[] m_tHeaterMC        = new TOnDelayTimer[10];
        TOnDelayTimer[] m_DoorError        = new TOnDelayTimer[20];

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        EN_SEQ_STAT m_iSeqStat ; //Current Sequence Status.
        

        int    m_iStep                                       ;
        bool   m_bBtnReset                                   ; //Button Input.
        bool   m_bBtnStart                                   ;
        bool   m_bBtnStop                                    ; //m_bBtn... = 모든 버튼들을 ||로 묶은것
        bool   m_bBtnWinReset                                ; //m_bWin... = 화면상의 버튼을 마우스로 클릭한것.
        bool   m_bBtnWinStart                                ; //m_bMan... = 장비 시퀜스 상에서 신호를 줄때 씀.
        bool   m_bBtnWinStop                                 ;
        bool   m_bBtnManReset                                ;
        bool   m_bBtnManStart                                ;
        bool   m_bBtnManStop                                 ;
               
        bool   m_bResetCon                                   ;
        bool   m_bRunCon                                     ;
        bool   m_bStopCon                                    ;
        bool   m_bNoSafety                                   ; //Door safety flag.
        bool   m_bRun                                        ; //Run Flag. (Latched)
        bool   m_bLtStop                                     ;
        bool   m_bLoadStop                                   ;
        bool[] TS_Rslt          = new bool[vDEF.MAX_SEQ_PART];
        bool   m_bTapeLockState                              ; //
        bool   m_bWorkEndState                               ;
        bool   m_TempLampOn                                  ; //임시로 Lamp On/Off
        bool   m_bReqReset                                   ; //

		//Define I/O
		EN_IN_ID  m_xStartSW                                 ;
		EN_IN_ID  m_xStopSW                                  ;
		EN_IN_ID  m_xResetSW                                 ;

		EN_OUT_ID  m_yLampStart                              ;
		EN_OUT_ID  m_yLampStop                               ;
		EN_OUT_ID  m_yLampReset                              ;


        private cCmdData m_SeqCmdData;
        //Spare Var.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        string  m_sSpare1  ;
        string  m_sSpare2  ;
        string  m_sSpare3  ;
        string  m_sSpare4  ;
        string  m_sSpare5  ;
        bool    m_bSpare1, m_bSpare2, m_bSpare3, m_bSpare4, m_bSpare5; //, m_bSpare6, m_bSpare7, m_bSpare8, m_bSpare9, m_bSpare10;
        int     m_iSpare1, m_iSpare2, m_iSpare3, m_iSpare4, m_iSpare5; //, m_iSpare6, m_iSpare7, m_iSpare8, m_iSpare9, m_iSpare10;
        double  m_dSpare1, m_dSpare2, m_dSpare3, m_dSpare4, m_dSpare5; //, m_dSpare6, m_dSpare7, m_dSpare8, m_dSpare9, m_dSpare10;



        //protected: /* Inheritable Vars.        */
        //SCAN TIME
        double[]           m_dScanTimeUP = new double[30];
        double[]           m_dStrtTimeUP = new double[30]; 
        double[]           m_dScanTimeAR = new double[vDEF.MAX_SEQ_PART];
        double[]           m_dStrtTimeAR = new double[vDEF.MAX_SEQ_PART];

        //public:    /* Direct Accessable Vars.  */
		public  bool       m_bAutoRunning      ;
		private bool       m_bFlick1           ; //Flicking Flags.
		private bool       m_bFlick2           ;
		private bool       m_bFlick3           ;
		private bool       m_bEdgeFlick1       ;
		private bool       m_bEdgeFlick2       ;
		private bool       m_bEdgeFlick3       ;
		public  bool       m_bFrceCntrIO       ; //IO 강제 설정이 필요 할 때  UpdateSeqStat() 역활을 하지 않음.
		public  bool       m_bFrceRoomLamp     ; //Room Lamp Flags.
		public  bool       m_bRqChLevel        ;
        public  bool       m_bRqExitPgm        ;
        public  bool       m_bPgmLoadOk        ;
        private bool       m_bReqLoadSW01      ;
        private bool       m_bReqLoadSW02      ;
        private bool       m_bAutoMode         ;

        public bool[]      m_bChkMgzExs = new bool[(int)EN_MGZ_ID.EndOfId];
        public bool[]      m_bChkMgzNon = new bool[(int)EN_MGZ_ID.EndOfId];

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool        _bRun         {get { return m_bRun         ;  }} 
        public bool        _bLtStop      {get { return m_bLtStop      ;  } set { m_bLtStop       = value; }}
        public bool        _bLoadStop    {get { return m_bLoadStop    ;  } set { m_bLoadStop     = value; }}
        public EN_SEQ_STAT _iSeqStat     {get { return m_iSeqStat     ;  }}
        public int         _iStep        {get { return m_iStep        ;  }}
        public bool        _bNoSafety    {get { return m_bNoSafety    ;  }}
        public bool        _bBtnManReset {get { return m_bBtnManReset ;  } set { m_bBtnManReset  = value; }}
        public bool        _bBtnManStart {get { return m_bBtnManStart ;  } set { m_bBtnManStart  = value; }}
        public bool        _bBtnManStop  {get { return m_bBtnManStop  ;  } set { m_bBtnManStop   = value; }}
        public bool        _bWorkEndState{get { return m_bWorkEndState;  } set { m_bWorkEndState = value; }}
        
        public bool        _bReqLoadSW01 {get { return m_bReqLoadSW01 ;  } set { m_bReqLoadSW01 = value; }}
        public bool        _bReqLoadSW02 {get { return m_bReqLoadSW02 ;  } set { m_bReqLoadSW02 = value; }}
        public bool        _bAutoMode    {get { return m_bAutoMode    ;  } set { m_bAutoMode    = value; }}
        
        public bool        _bTapeLockState => m_bTapeLockState; //
        public bool        _TempLampOn     => m_TempLampOn;

        public bool        _bFlick1 { get { return m_bFlick1; } }
        public bool        _bFlick2 { get { return m_bFlick2; } }
        public bool        _bFlick3 { get { return m_bFlick3; } }
        public bool       _bReqReset { get { return m_bReqReset; } set { m_bReqReset = value; }}

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TEdgeUnit   m_EdgeBtnReset  = new TEdgeUnit();
        TEdgeUnit   m_EdgeFlick1    = new TEdgeUnit();
        TEdgeUnit   m_EdgeFlick2    = new TEdgeUnit();
        TEdgeUnit   m_EdgeFlick3    = new TEdgeUnit();
        TEdgeUnit[] m_EdgeBtnPanel  = new TEdgeUnit [20];
        TEdgeUnit[] m_EdgeMgzExs    = new TEdgeUnit [(int)EN_MGZ_ID.EndOfId];
        TEdgeUnit[] m_EdgeMgzNon    = new TEdgeUnit [(int)EN_MGZ_ID.EndOfId];
        TEdgeUnit   m_EdgeTech      = new TEdgeUnit();


        //Sequence List
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        List<TSeqUnit> SeqList = new List<TSeqUnit>();

        //UserSet - Part Sequence 설정 
        public TSeqWaferAlignTable WAT  = new TSeqWaferAlignTable();

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSequence()
        {
            m_sSpare1      = "";
            m_sSpare2      = "";
            m_sSpare3      = "";
            m_sSpare4      = "";
            m_sSpare5      = "";

            m_bPgmLoadOk   = false;

            for (int i=  0; i < m_EdgeBtnPanel.Length; i++) m_EdgeBtnPanel[i] = new TEdgeUnit    ();
            for (int i = 0; i < m_AutoDOCtrl  .Length; i++) m_AutoDOCtrl  [i] = new TOnDelayTimer();
            for (int i = 0; i < m_tFanAlarm   .Length; i++) m_tFanAlarm   [i] = new TOnDelayTimer();
            for (int i = 0; i < m_tHeaterMC   .Length; i++) m_tHeaterMC   [i] = new TOnDelayTimer();
            for (int i = 0; i < m_DoorError   .Length; i++) m_DoorError   [i] = new TOnDelayTimer();

            Init();

            //UserSet - Sequence에 Part Sequence Class 추가  
            //EN_SEQ_ID 순서로...
			SetList(WAT );

            //UserSet - Panel S/W IO 설정 
            m_xStartSW    = EN_IN_ID .xNone;
		    m_xStopSW     = EN_IN_ID .xNone;
		    m_xResetSW    = EN_IN_ID .xNone;

		    m_yLampStart  = EN_OUT_ID.yNone;
		    m_yLampStop   = EN_OUT_ID.yNone;
		    m_yLampReset  = EN_OUT_ID.yNone;

            m_SeqCmdData = new cCmdData(eCommand.None, null);
        }
        ~TSequence() { }
        //------------------------------------------------------------------------
        public void SetList(TSeqUnit SeqUnit)
        {
            SeqList.Add(SeqUnit);
        }
        //------------------------------------------------------------------------
		//Check Button.
        public void  CheckToStartTO    ()
        {
            //Local Var.
            //String Msg = "";
            String Temp = string.Empty ;

            //Check Timer.
            if (!m_ToStrtTimer.OnDelay(m_iStep == 14, 35000)) return;
            //Trace Log.
            if (m_iStep == 14)
            {
                Temp = string.Format("ToStartTimeOut : m_iStep={0}", m_iStep);
                for(int i=0; i<SeqList.Count;i++) {
                    Temp += string.Format(" {0}={1}", cDEF.POSN.GetPartName(i), TS_Rslt[i]); 
                }
            }
            CrntStatTrace(EN_SEQ_ID.ALL, Temp);
            cDEF.LOG.Trace(Temp);
            cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0042); //ToStartTimeOut

            //Clear Flag.
            m_iStep        = 0;
            m_bRun         = false;
            m_bAutoRunning = false;
        }		
        //------------------------------------------------------------------------
		public void  CheckToStopTO()
        {
            //Local Var.
            string Msg  = string.Empty;
            string Temp = string.Empty;

            //Check Timer.
            if (!m_ToStopTimer.OnDelay((m_iStep == 17) || m_bLtStop, 35000)) return;

            //Trace Log.
            Temp = string.Format("ToStopTimeOut : m_iStep={0}", m_iStep);
            for(int i=0; i<SeqList.Count;i++) 
            {
                Temp += string.Format(" {0}={1}", cDEF.POSN.GetPartName(i), TS_Rslt[i]); 
            }

            ToStop();
            CrntStatTrace(EN_SEQ_ID.ALL, Temp);
            cDEF.LOG.Trace(Temp);
            cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0043); //ToStopTimeOut
      
            SaveWorkInfo();

            //Clear Flag.
            m_iStep        = 0;
            m_bLtStop      = false;

            m_bRun         = false;
            m_bAutoRunning = false;
        }
        //------------------------------------------------------------------------
		public bool  CheckStrtBtn() 
        {
            //Check Door
            //if (IO.IsDoorOpen() && FM.SysOptn.iChkTopDoor != 0)
            //{
            //    FRM.ShowWarn(true, "DOOR를 닫아 주세요.");
            //    return false;
            //}

            //Check Home
            if (!IsAllHomeEnd())
            {
                //FRM.ShowWarn(true, "All motor initialization is not complete.");
                FRM.ShowWarn(true, "Home 동작이 필요한 MOTOR가 있습니다.");
                return false;
            }
            
            //Check Recipe
            //if(!cDEF.LOT._bLotOpen)
            //{
            //    FRM.ShowWarn(true, "Recipe 선택 후, Recipe Open을 진행해 주세요");
            //    return false;
            //}

            //Check Mode
            if(!_bAutoMode && FM.IsAutoMode())
            {
                FRM.ShowWarn(true, "AUTO MODE에서 가동 가능합니다.");
                return false;
            }

            //Check Load Position
            //if(!LPM1.IsCylLoadUnload(EN_MGZ_ID.MGZ1, true) && !LPM2.IsCylLoadUnload(EN_MGZ_ID.MGZ2, true))
            //{
            //    FRM.ShowWarn(true, "FOUP을 LOAD하세요.");
            //    return false;
            //}

            return true;
        }
        //------------------------------------------------------------------------
        public void  CheckButton       () //Run.
        {//UserSet - Start 버튼 처리 눌릴 경우 처리 
            //Local Var.
            bool isErr          = cDEF.EPU._bHasErr;
            bool isErrDisp      = cDEF.EPU._bHasDsp;
            bool isAllCloseDoor = !IsOpenAnyDoor();  //Door Sensor.
            bool isTechMode     = false;

            //Read Button State. (Combination input switch and buttons)
            bool isBtnStart     =  cDEF.IO.gX(m_xStartSW) || m_bBtnWinStart || m_bBtnManStart;
            bool isBtnStop      =  cDEF.IO.gX(m_xStopSW ) || m_bBtnWinStop  || m_bBtnManStop || isTechMode;
            bool isBtnReset     =  cDEF.IO.gX(m_xResetSW) || m_bBtnWinReset || m_bBtnManReset;
            
            m_bBtnWinStart = false;
            m_bBtnWinStop  = false;
            m_bBtnWinReset = false;

            m_tInspectDly.OnDelay(m_bPgmLoadOk, 5000);

            //Reset Con.
            isBtnReset = m_EdgeBtnReset.IsRising(isBtnReset);

            //Set Button State.
            if (!isBtnStart               ) m_bBtnStart = false;
            if (!isBtnStart               ) m_bBtnStart = false;
            if (!isBtnStop                ) m_bBtnStop  = false;
            if (!isBtnReset               ) m_bBtnReset = false;
            if (!m_bBtnStart && isBtnStart) m_bBtnStart = true ;
            if (!m_bBtnStop && isBtnStop  )
            {
                m_bBtnStop = true;
            }
            if (!m_bBtnReset && isBtnReset)
            {
                m_bBtnReset = true; cDEF.LOG.Trace(">>> RESET <<<");
            }
            
            //Init. Button Flags.
            if (m_bBtnStop ) { m_bBtnWinStop  = false; m_bBtnManStop  = false; }
            if (m_bBtnReset) { m_bBtnWinReset = false; m_bBtnManReset = false; }
            if (m_bBtnStart)
            {
                m_bBtnWinStart = false;
                m_bBtnManStart = false;
                //설비 START시 자동으로 LEVEL을 OPERATOR로 변경.
                if ((cDEF.FM.m_iCrntLevel != (int)EN_LOGIN.Operator) && isAllCloseDoor)
                {
                    FM.SetUserLevel(); //FRM.ChangeLevel(EN_LOGIN.Operator);
                }

                GoDoorLock(true);
            }

            //m_tLevelChkTime.OnDelay(_bRun && (cDEF.FM.m_iCrntLevel >= (int)EN_LOGIN.Engineer), cDEF.FM.EngrOptn.iChangeOperTime * 1000);
            //if (m_tLevelChkTime.Out)
            //{
            //    cDEF.FM.DefaultSysChkOptn();
            //    FM.SetUserLevel(); //cDEF.FM.m_iCrntLevel = (int)EN_LOGIN.Operator;
            //    m_bRqChLevel = true;
            //    GoDoorLock(true);
            //    m_tLevelChkTime.Clear();
            //}

            //Set Condition Flags.
            m_bStopCon  = m_bBtnStop  || isErr;
            m_bRunCon   = m_bBtnStart && !isErr && !m_bRun && !m_bBtnStop;
            m_bResetCon = m_bBtnReset && !m_bRun;

            //Buzzer Off.
            if (isErr && m_bBtnStop) cDEF.LampBuzz.BuzzOff();

            //Decide Step.
            //Stop
            if      (m_bStopCon && (m_iStep == 0)) m_bRun    = false;
            else if (m_bStopCon && (m_iStep != 0)) m_bLtStop = true ;

            //Run.
            if (m_bRunCon && (m_iStep == 0))
            {
                m_bLtStop  = false;
                m_iStep    = 10;
                m_ToStrtTimer.Clear();
            }
            //Reset.
            if (m_bResetCon) Reset();
            if (m_bBtnReset && isErrDisp) cDEF.EPU.Clear();

            //Time Out.
            CheckToStartTO();
            CheckToStopTO ();

            //Running Step.
            switch (m_iStep)
            {
                default: m_bAutoRunning = false;
                         m_bLtStop      = false;
                         m_bRun         = false;
                         m_iStep        = 0;
                         return;

                case 10:
                         //Check Start Button Condition. 
                         if (!CheckStrtBtn    ()) { m_iStep = 0; return; }
                         
                         //Inspect.
                       //if (!InspectMainAir  ()) { m_iStep = 0; return; }
                         if (!InspectEmergency()) { m_iStep = 0; return; }
                         if (!InspectSafety   ()) { m_iStep = 0; return; }
                         if (!InspectMotor    ()) { m_iStep = 0; return; }
                       //if (!InspectActuator ()) { m_iStep = 0; return; }
                         m_iStep++;
                         return;

                case 11: if (!cDEF.MOTR.InspectMinMax()) { m_iStep = 0; FRM.ShowWarn(true,"Check position / speed Parameter."); return; }
                         if (cDEF.MAN._iManNo > 0      ) { m_iStep = 0; FRM.ShowWarn(true,"Manual function is operating"     ); return; }
                         m_iStep++;
                         return;

                case 12:
                         if (isErr) { m_iStep = 0; return; }
                         m_iStep++;
                         return;

                case 13: 
                         if(!ToStartCon()) return;
                         m_iStep++;
                         return;

                case 14: //To Start.
                         if (isErr) { m_iStep = 0; }
                         if (!ToStart()) return;
                         //
                         SEQ.ClearWorkEnd();
                         //
                         m_TempLampOn = false;
                         m_WaitStartTimer.Clear();
                         m_iStep++;
                         return;

                case 15: 
                         m_bRun = true;
                         if (!m_WaitStartTimer.OnDelay(true, 500)) return;
                         LOG.Trace(">>> START <<<");
                         
                         if(FM.IsAutoMode()) FM.ApplySystem(); //JUNG/220203

                         //
                         if(m_SeqCmdData.Command == eCommand.AUT) COMZEUS.SetResult(m_SeqCmdData);

                         m_bAutoRunning = true;

                         m_iStep++;
                         return;

                case 16: m_WaitStopTimer.OnDelay(m_bLtStop, 200); 
                         if (m_bLtStop && m_WaitStopTimer.Out)
                         {
                             if (!ToStopCon()) return;
                             m_bLtStop      = false;
                             m_bAutoRunning = false;
                             m_iStep++;
                             return;
                         }
                         else
                         {
                             //LotEnd.
                             if (cDEF.SEQ.IsWorkEnd())
                             {                                
                                m_bLtStop = true;
                                return;
                             }
                             //Run Check.
                             if (!m_bRun) return;
                         }
                         return;

                case 17: m_bAutoRunning = false;
                         if (!ToStop()) return;
                         SaveWorkInfo();
                         //
                         m_bRun = false;
                         LOG.Trace(">>> STOP <<<");
                         m_iStep = 0;

                         //
                         if (m_SeqCmdData.Command == eCommand.MAN) COMZEUS.SetResult(m_SeqCmdData);
                         
                         if(m_bReqReset)
                         {
                            Reset();
                         }

                         //
                         if (SEQ.IsWorkEnd()) { LOT.LotEnd(); } // cDEF.EPU.SetErr(999, true); }
                         
                         //

                         return;
            }
        }
        //------------------------------------------------------------------------
        public void  CheckButtonPanel  ()
        {//UserSet - 설비 Panel에 SW 눌릴경우 처리 
            //Local Var.
            bool isErr           =  EPU._bHasErr;
            bool bDoorLock       =  IsDoorLock();
            bool isRstSW         = (IO.gX(m_xResetSW) || m_bBtnWinReset);
            bool isDR_Lock_SW    = false; //(IO.gX(EN_IN_ID.xDR_DoorLock_Left) || IO.gX(EN_IN_ID.xDR_DoorLock_Rigt));
            bool xAutoMode       = false; // IO.gX(EN_IN_ID.xSW_AutoMode      );
            bool isManMode       = !m_bRun && !xAutoMode;

            //
            if (m_EdgeBtnPanel[10].IsRising(isDR_Lock_SW) && bDoorLock && !m_bRun)
            {
                GoDoorLock(false);
            }

            //Update Switch's Lamp
            IO.sY(m_yLampStart ,(m_iStep != 0)             ); //Start Lamp. // m_bRun;
            IO.sY(m_yLampStop  ,!m_bRun                    ); //Stop  Lamp.
            IO.sY(m_yLampReset ,isErr ? m_bFlick1 : isRstSW); //Reset Lamp.

            //cDEF.IO.sY(EN_OUT_ID.yLampDoorOpen, bDoorLock); //상태 표시로 변경-> 열수 있을 때 Lamp On

        }
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init             ()
        {
            //Init. Var.
            m_bBtnReset      = false; //Button Input.
            m_bBtnStart      = false;
            m_bBtnStop       = false;
            m_bBtnWinReset   = false;
            m_bBtnWinStart   = false;
            m_bBtnWinStop    = false;
            m_bBtnManReset   = false;
            m_bBtnManStart   = false;
            m_bBtnManStop    = false;
            m_bResetCon      = false;
            m_bRunCon        = false;
            m_bStopCon       = false;
            m_bNoSafety      = false;
            m_bRun           = false;
            m_bLtStop        = false;
            m_bAutoRunning   = false;
            m_bFlick1        = false; //Flicking Flag
            m_bFlick2        = false;
            m_bFlick3        = false;
            m_bEdgeFlick1    = false;
            m_bEdgeFlick2    = false;
            m_bEdgeFlick3    = false;
            m_bFrceCntrIO    = false; //
            m_bLoadStop      = false;
            m_bTapeLockState = false;
            m_iSeqStat       = EN_SEQ_STAT.Stop; //Current Sequence Status.
            m_iStep          = 0; //Sequence Step.
            m_bRqExitPgm     = false;
            m_bWorkEndState  = false;

            for (int n = 0; n < (int)EN_MGZ_ID.EndOfId; n++)
            {
                m_bChkMgzExs[n] = false;
                m_bChkMgzNon[n] = false;
                m_EdgeMgzExs[n] = new TEdgeUnit();
                m_EdgeMgzNon[n] = new TEdgeUnit();
                m_tMgzExs   [n] = new TOnDelayTimer();
                m_tMgzNon   [n] = new TOnDelayTimer();
            }

            //Init. Timer
            //Initial Sequence Part
            for(int i=0; i<SeqList.Count;i++) 
            {
                SeqList[i].Init();
            }

            //Clear Timer
            m_tInspectDly.Clear();


        }
        //------------------------------------------------------------------------
        public void  Reset            ()
        {
            //Check running flag.
            if (m_bRun                        ) return;
            if (m_iSeqStat == EN_SEQ_STAT.Init) return;

            //Moving Parts.
            //Reset Sequence Part
            for(int i=0; i<SeqList.Count;i++) 
            {
                SeqList[i].Reset();
            }

            //System.
            MOTR.Reset();
            ACTR.Reset();

            //Error.
            EPU.Clear();

            //Manual.
            MAN.Reset();

			//Vision Reset
			//cDEF.VISN.Reset();

            //Close User Message.
            FRM.ShowWarn(false);

            //Init. Flags.
            m_bNoSafety = false;

            //Switch On Buzzer Flag.
            LampBuzz._bBuzzOff = false;

            //
            //cDEF.Aligner.Reset();

            //
            m_iStep = 0;

            m_bWorkEndState = false;

            if(m_bReqReset)
            {
                m_bReqReset    = false;
                if (cDEF.SEQ._bAutoMode && cDEF.SEQ.IsAllHomeEnd())
                {
                    m_bBtnManStart = true;
                    if (m_SeqCmdData.Command == eCommand.RST) COMZEUS.SetResult(m_SeqCmdData);
                }
            }
        }
        //------------------------------------------------------------------------
		public bool  IsNoRun          () 
        { 
            return !m_bRun && !m_bLtStop && (m_iSeqStat != EN_SEQ_STAT.Init); 
        }

        //Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//Go Function
		public bool  GoDoorLock       (bool bOn, bool NoChkRun = false)
        {
            bool bSetOn;
            //B접점 - ON되면 해제
            if (bOn) bSetOn = false;
            else     bSetOn = true;
            // 
            //if (cDEF.FM.SysOptn.iChkDrLock == 0) bSetOn = true;
            //
            if (!NoChkRun)
            {
                if (!bSetOn && m_bRun                          ) return false;
                if (!bSetOn && (m_iSeqStat == EN_SEQ_STAT.Init)) return false;
            }
            //UserSet - Door Lock Output 처리     
            //IO.sY(EN_OUT_ID.yDR_Lock_Left   , bSetOn); //
            //IO.sY(EN_OUT_ID.yDR_Lock_Right  , bSetOn); //

            return true;

        }
        //------------------------------------------------------------------------
			//Inspection Machine Status.
		public bool  InspectMainAir   (                )
        {
            bool isOk = true;

            if (!cDEF.IO._bInitOk              ) return true;
            if (!m_bPgmLoadOk                  ) return true; 
            if (!m_tInspectDly.Out             ) return true;
            if (cDEF.FM.SysOptn.iChkSafety == 0) return true;
            if (FM.SysOptn.bSimulRun           ) return true;
           

            //UserSet - Main Air Input 처리
            //bool bMainAir = !IO.gX(EN_IN_ID.xSYS_MAIN_Air  ) ; 
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0760, bMainAir)) isOk = false;

            return isOk;

        }
        //------------------------------------------------------------------------
        public bool  InspectEmergency (                )
        {
            bool isOk = true;

            if (!cDEF.IO._bInitOk              ) return true;
            if (!m_bPgmLoadOk                  ) return true;
            if (!m_tInspectDly.Out             ) return true;
            if (FM.SysOptn.bSimulRun           ) return true;

            //UserSet - Emergency Input 처리 
            //bool bEMO = !cDEF.IO.gX(EN_IN_ID.xEMO_01) || !cDEF.IO.gX(EN_IN_ID.xEMO_02) || 
            //            !cDEF.IO.gX(EN_IN_ID.xEMO_03) || !cDEF.IO.gX(EN_IN_ID.xEMO_04);

            //if (EPU.SetErr(EN_ERR_LIST.ERR_0765, IO.gX(EN_IN_ID.xEMO_FRONT_LEFT))) isOk = false;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0766, IO.gX(EN_IN_ID.xEMO_FRONT_CENT))) isOk = false;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0767, IO.gX(EN_IN_ID.xEMO_FRONT_RIGT))) isOk = false;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0768, IO.gX(EN_IN_ID.xEMO_LEFT_SIDE ))) isOk = false;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0769, IO.gX(EN_IN_ID.xEMO_RIGT_SIDE ))) isOk = false;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0770, IO.gX(EN_IN_ID.xEMO_REAR_CENT ))) isOk = false;
                                                                 
            //Ok.                                                
            return isOk;
        }
        //------------------------------------------------------------------------
        public bool  InspectSafety    (                )
        {
            if (!cDEF.IO._bInitOk ) return true;
            if (!m_bPgmLoadOk     ) return true;
            if (!m_tInspectDly.Out) return true;

            //Local Var.
            //Check.
            bool isOk = !IsChkAnySafety();

            //Reject Start Button. (Motor Holding Error - Safety 검출)
            if (m_bBtnStart && !isOk) {
                //Emergency Process
                for (int i = 0; i < cDEF.MOTR._iNumOfMotr; i++)
                {
                    cDEF.EPU.SetErr(cDEF.MOTR.ErrNoHold((EN_MOTR_ID)i));
                    cDEF.MOTR.EmrgStop((EN_MOTR_ID)i);
                }
            }
            //Set Safety Flag.
            if (!isOk) 
            {
                m_bNoSafety = true ;
            }
            if(isOk && m_bNoSafety && !IsChkAnySafety()) m_bNoSafety = false;

            
            //Ok.
            return isOk; //Error 시 Fail Return
        }
        //------------------------------------------------------------------------
		public bool  InspectActuator  (                )
        {
            //Local Var.
            bool isOk = true;

            if (!cDEF.IO._bInitOk ) return true;
            if (!m_bPgmLoadOk     ) return true;
            if (!m_tInspectDly.Out) return true; 

            int iErrFNo = cDEF.ACTR.m_iErrFNo;
            //Inspect Actuator
            if (!cDEF.IO._bInitOk) return true;
            for (int i = 0; i < cDEF.ACTR._iNumOfACT; i++)
            {
                if (cDEF.EPU.SetErr(iErrFNo + i, cDEF.ACTR.Err(i) != 0)) isOk = false;
            }
            //Ok.
            return isOk; //Error 시 Fail Return        
        }
        //------------------------------------------------------------------------
        public bool  InspectMotor     (                )
        {
            int  iErrNo;
            bool isOk     = true;
            bool isNoRun  = (cDEF.SEQ._iStep == 0) &&  cDEF.MAN._iManNo == 0;

            if (!cDEF.MOTR[0]._bInitAxis) return true;
            if (!m_bPgmLoadOk           ) return true;
            if (!m_tInspectDly.Out      ) return true; 

            for (int i = 0; i < cDEF.MOTR._iNumOfMotr; i++)
            {
                //Motor Alarm.
                iErrNo = cDEF.MOTR.ErrNoAlarm((EN_MOTR_ID)i);
                if (cDEF.MOTR[i]._iNoUseMotr==1) continue;
                if (cDEF.EPU.SetErr(iErrNo, cDEF.MOTR[i].GetAlarm())) { cDEF.MOTR.m_bNeedReboot = true; isOk = false; }


                //CW Limit.
                iErrNo = cDEF.MOTR.ErrNoCW((EN_MOTR_ID)i);
                if (cDEF.EPU.SetErr(iErrNo, !isNoRun && cDEF.MOTR[i].GetCW() && cDEF.MOTR[i].GetHomeEnd())) isOk = false;
                
                //CCW Limit.
                iErrNo = cDEF.MOTR.ErrNoCCW((EN_MOTR_ID)i);
                if (cDEF.EPU.SetErr(iErrNo, !isNoRun && cDEF.MOTR[i].GetCCW() && cDEF.MOTR[i].GetHomeEnd())) isOk = false;

                //Control Error.
                //if(!m_bBtnStart) continue;
                //iErrNo = cDEF.MOTR.ErrNoControl((EN_MOTR_ID)i);
                //EPU.SetErr(iErrNo , cDEF.MOTR.pAXIS[i]->GetCntrErr());
            }
            //Ok.
            return isOk;  //Error 시 Fail Return
        }
        //------------------------------------------------------------------------
        public bool  InspectHomeEnd   (bool FrceChk = false)
        {
            //Local Var.
            bool isOk      = true;
            int  iErrNo    = 0;

            //Running 중에만 Check.
            if (!m_bRun && !FrceChk) return isOk;

            //Inspect.
            for (int i = 0; i < cDEF.MOTR._iNumOfMotr; i++)
            {
                if (cDEF.MOTR[i]._iNoUseMotr==1) continue;
                iErrNo = cDEF.MOTR.ErrNoHome((EN_MOTR_ID)i);

                if (cDEF.EPU.SetErr(iErrNo, !cDEF.MOTR[i].GetHomeEnd())) isOk = false;
            }
            //Ok.
            return isOk; //Error 시 Fail Return
        }
        //------------------------------------------------------------------------
        public void  InspectHold      (                ) //설비 HOLD 검사.
        {
            
            //Local Var.
            String sTemp = "";
            bool isHold = m_bRun && !m_bLtStop;
            for(int i=0; i<SeqList.Count;i++) 
            {
                if(SeqList[i].GetSeqStep() != 0) { isHold = false; break;}
            }

            //
            if (m_MachineHoldTimer.OnDelay(isHold, 300 * 1000))
            {
                m_MachineHoldTimer.Clear();
                sTemp = "Machine Downtime exceeded 5 minutes";
                CrntStatTrace(EN_SEQ_ID.ALL, "Title : HOLD");
                FRM.ShowWarn(true, sTemp);
                cDEF.LOG.Trace(sTemp);
                m_bLtStop = true;
            }
        }
        //------------------------------------------------------------------------
		public void InspectSensor(                )
        {
            if (!cDEF.IO._bInitOk            ) return ;
            if (!m_bPgmLoadOk                ) return ;
            if (!m_tInspectDly.Out           ) return ; 
           
        }
        //------------------------------------------------------------------------
		public bool  InspectTempCtrl (                )
        {//UserSet - Temp Ctrl Alarm 처리 
            bool isErr = false;

            //
            return isErr;
        }
        //------------------------------------------------------------------------
        public bool InspectFan()
        {
            if ( cDEF.FM.SysOptn.iChkFan == 0) return false;
            if (!cDEF.IO._bInitOk            ) return true;
            if (!m_bPgmLoadOk                ) return true;
            if (!m_tInspectDly.Out           ) return true;
            if (FM.SysOptn.bFanSkipAlarm) return false; //2026 08 25 2LC8 
            
            //JUNG/Run이 아니면 Alarm X
            //if (!_bRun                       ) return false;

            bool isErr = false;
            int  nStart = 0;

            //Timer
            //ERR_0016 //Exhaust FAN STOP_1
            //ERR_0017 //Exhaust FAN STOP_2
            //ERR_0018 //Intake FAN STOP_1
            //ERR_0019 //Intake FAN STOP_2
            if (EPU.SetErr(EN_ERR_LIST.ERR_0016, IO.gX(EN_IN_ID.xSYS_FanAlarm01))) isErr = true;
            if (EPU.SetErr(EN_ERR_LIST.ERR_0017, IO.gX(EN_IN_ID.xSYS_FanAlarm02))) isErr = true;
            if (EPU.SetErr(EN_ERR_LIST.ERR_0018, IO.gX(EN_IN_ID.xSYS_FanAlarm03))) isErr = true;
            if (EPU.SetErr(EN_ERR_LIST.ERR_0019, IO.gX(EN_IN_ID.xSYS_FanAlarm04))) isErr = true;
            //for (int n = (int)EN_IN_ID.xSYS_FanAlarm01; n <= (int)EN_IN_ID.xSYS_FanAlarm04; n++)
            //{
            //    nStart = n - (int)EN_IN_ID.xSYS_FanAlarm01;
            //    m_tFanAlarm[nStart].OnDelay(IO.gX((EN_IN_ID)n), 3000);
            //    if(m_tFanAlarm[nStart].Out)
            //    {
            //        if (EPU.SetErr(EN_ERR_LIST.ERR_0016 + nStart, IO.gX((EN_IN_ID)n))) isErr = true;
            //    }
            //}

            return isErr;
        }

        //------------------------------------------------------------------------
		//Status Check
        public bool  IsDoorLock       (                )
        {//UserSet - Door Lock Output 처리  
            //if (cDEF.FM.SysOptn.iChkDrLock == 0) return true;            
            bool r0 = true; //IO.gY(EN_OUT_ID.yDR_Lock_Left );
            bool r1 = true; //IO.gY(EN_OUT_ID.yDR_Lock_Right);
            bool r2 = true; 
            bool r3 = true; 
            bool r4 = true; 
            bool r5 = true;
            bool r6 = true;
            bool r7 = true;
            bool r8 = true;
            bool r9 = true;

            return (r0 && r1 && r2 && r3 && r4 && r5 && r6 && r7 && r8 && r9);
        }
        //------------------------------------------------------------------------
        public bool  IsChkAnySafety   (                )
        {
            if (!cDEF.IO._bInitOk) return true;

            //Check Sensor.
            if (m_bRun)
            {
                if (IsOpenAnyDoor()) return true;
            }
            if (IsSafetyAlarm()) return true;
            //No Check.
            return false;
        }
        //------------------------------------------------------------------------
        public bool  IsOpenAnyDoor    (                )
        {
            //Check Sensor.
            bool isErr = false;

			if (cDEF.FM.SysOptn.iChkTopDoor == 0) return false;

            if (!cDEF.IO._bInitOk ) return true;
            if (!m_bPgmLoadOk     ) return true;
            if (!m_tInspectDly.Out) return true; 
            
            bool bChkStat  = m_bRun || m_iSeqStat == EN_SEQ_STAT.Init || m_bBtnStart;

            //UserSet - Door Open Input Sensor 처리 
            //Check Sensor.
            //B접점
            //if (bChkStat)
            //{
            //    for (int n = 0; n < 2; n++) //JUNG/220328
            //    {
            //        m_DoorError[n].OnDelay(IO.gX((int)EN_IN_ID.xDR_DoorLock_Left + n), 500); // xDR_DoorLock_Rigt
            //        if (EPU.SetErr(EN_ERR_LIST.ERR_0660 + n, m_DoorError[n].Out)) isErr = true;
            //    }
            //}


            //No Check.
            return isErr;
        }
        //------------------------------------------------------------------------
		public bool  IsSafetyAlarm    (                )
        {//UserSet - Safety Input 처리
            //Check Option.
            bool isErr = false;
            //if (FM.SysOptn.iChkSafety == 0) return false;
            //
            ////Relay, MC
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0700, !IO.gX(EN_IN_ID.xDR_SafetyRelay1        ))) isErr = true;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0701, !IO.gX(EN_IN_ID.xDR_SafetyRelay2        ))) isErr = true;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0702, !IO.gX(EN_IN_ID.xSYS_MAIN_MC            ))) isErr = true;
            //
            ////Ionizer
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0705,  IO.gX(EN_IN_ID.xWAT_Ionizer_Alarm      ))) isErr = true;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0706,  IO.gX(EN_IN_ID.xMMC_MC1_Ionizer_Alarm  ))) isErr = true;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0707,  IO.gX(EN_IN_ID.xMMC_MC2_Ionizer_Alarm  ))) isErr = true;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0708,  IO.gX(EN_IN_ID.xLPM_PORT1_Ionizer_Alarm))) isErr = true;
            //if (EPU.SetErr(EN_ERR_LIST.ERR_0709,  IO.gX(EN_IN_ID.xLPM_PORT2_Ionizer_Alarm))) isErr = true;

            return isErr;
        }
        //------------------------------------------------------------------------
		public bool  IsAllHomeEnd     (                )
        {
            for (int Axe = 0; Axe < cDEF.MOTR._iNumOfMotr; Axe++)
            {
                if ( cDEF.MOTR[Axe]._iNoUseMotr==1) continue;
                if (!cDEF.MOTR[Axe].GetHomeEnd()  ) return false;
            }

            return true;
        }
        //------------------------------------------------------------------------
        public void ClearAllHomeStep(EN_SEQ_ID Whre)
        {
            if(Whre == EN_SEQ_ID.ALL)
            {
                for(int i=0; i<SeqList.Count;i++) 
                    SeqList[i].ClearHomeStep();
                return;
            }
            SeqList[(int)Whre].ClearHomeStep();
            
        }
        //------------------------------------------------------------------------
        public bool MoveHome(EN_SEQ_ID Whre)
        {
            bool isDone = true;
            if(Whre == EN_SEQ_ID.ALL)
            {
                for(int i=0; i<SeqList.Count;i++) 
                    if(!SeqList[i].MoveHome()) isDone = false; 
                return isDone;
            }
            isDone = SeqList[(int)Whre].MoveHome();
            return isDone;
        }
        //------------------------------------------------------------------------
        public bool MoveMotr(EN_SEQ_ID Whre, EN_MOTR_ID iMotr, EN_COMD_ID Cmd)
        {
            if(Whre<0 || (int)Whre>=SeqList.Count) return false;
            return SeqList[(int)Whre].MoveMotr(iMotr, Cmd);
        }
        //------------------------------------------------------------------------
        public bool ReqMoveMotr(EN_SEQ_ID Whre, EN_MOTR_ID iMotr, EN_COMD_ID Cmd)
        {
            if(Whre<0 || (int)Whre>=SeqList.Count) return false;
            return SeqList[(int)Whre].ReqMoveMotr(iMotr, Cmd);
        }
        //------------------------------------------------------------------------
        public bool MoveDirect(EN_SEQ_ID Whre, EN_MOTR_ID iMotr, double dPosn)
        {
            if(Whre<0 || (int)Whre>=SeqList.Count) return false;
             return SeqList[(int)Whre].MoveDirect(iMotr, dPosn);
        }
        //------------------------------------------------------------------------
        public bool ReqMoveDirect(EN_SEQ_ID Whre, EN_MOTR_ID iMotr, double dPosn)
        {
            if(Whre<0 || (int)Whre>=SeqList.Count) return false;
             return SeqList[(int)Whre].ReqMoveDirect(iMotr, dPosn);
        }
        public bool  MoveToLastWorkPosn(EN_SEQ_ID Whre, EN_MOTR_ID iMotr)
        {
            if(Whre<0 || (int)Whre>=SeqList.Count) return false;
             return SeqList[(int)Whre].MoveToLastWorkPosn(iMotr);
        }
		public bool  MoveToSafetyWaitPosn(EN_SEQ_ID Whre)
        {
            if(Whre<0 || (int)Whre>=SeqList.Count) return false;
             return SeqList[(int)Whre].MoveToSafetyWaitPosn();
        }
        //------------------------------------------------------------------------
        public bool CheckDstb(EN_SEQ_ID Whre, EN_MOTR_ID  Axe, EN_COMD_ID Cmd, int Step, EN_FPOSN_INDEX FIndex, double DirPosn)
        {
            if(Whre<0 || (int)Whre>=SeqList.Count) return false;
            return SeqList[(int)Whre].CheckDstb(Axe, Cmd, Step, FIndex, DirPosn);
        }
        //------------------------------------------------------------------------
        public bool CheckDstbActr(int iActrNo, int iAct)
        {//UserSet - Cylinder 의 간섭 조건을 설정한다. 

            return true;
        }
        //------------------------------------------------------------------------
        public bool IsWorkEnd()
        {                         
            for(int i=0; i<SeqList.Count;i++) 
                if(!SeqList[i].IsWorkEnd()) return false;
            return true;
        }
        //------------------------------------------------------------------------
        public void ClearWorkEnd()
        {
            for(int i=0; i<SeqList.Count;i++) 
                SeqList[i].ClearWorkEnd();
        }

        //------------------------------------------------------------------------
			//Running actions.
		public bool  ToStartCon       (                ) //Clear data to start.
        {
            //Call ToStart.
            bool isCon = true;
            for(int i=0; i<SeqList.Count;i++) 
            {
                TS_Rslt[i] = (cDEF.FM.SysOptn.bOffAR[i] || SeqList[i].ToStartCon());
                isCon = isCon && TS_Rslt[i];
            }
            return isCon;
        }
        //------------------------------------------------------------------------
        public bool  ToStopCon        (                ) //Check condition to stop the cycle. (Check iStepIndex number)
        {
            //Call ToStop.
            bool isCon = true;
            for(int i=0; i<SeqList.Count;i++) 
            {
                TS_Rslt[i] = (cDEF.FM.SysOptn.bOffAR[i] || SeqList[i].ToStopCon());
                isCon = isCon && TS_Rslt[i];
            }
            return isCon;
        }
        //------------------------------------------------------------------------
		public bool  ToStart          (                ) //Functions when start cycle running.
        {
            //Reset Flag.
            cDEF.EPU._bUpdatedErrForm = false ;

            //Stop The Actuator.
            cDEF.ACTR.SetRpt((int)EN_ACTR_ID.None, false);

            //Call ToStop.
            bool isCon = true;
            for(int i=0; i<SeqList.Count;i++) 
            {
                TS_Rslt[i] = (cDEF.FM.SysOptn.bOffAR[i] || SeqList[i].ToStart());
                isCon = isCon && TS_Rslt[i];
            }
            if(!isCon) return false;

            //Return.
            return true;
        }
        //------------------------------------------------------------------------
        public bool  ToStop           (                ) //Functions when stop the cycle.
        {
            //Call ToStop
            bool isCon = true;
            for(int i=0; i<SeqList.Count;i++) 
            {
                TS_Rslt[i] = (cDEF.FM.SysOptn.bOffAR[i] || SeqList[i].ToStop());
                isCon = isCon && TS_Rslt[i];
            }
            if(isCon) GoDoorLock(false);
            return isCon;

        }
        //------------------------------------------------------------------------
        public bool  AutoRun          (                ) //Auto Run.
        {
            if (!m_bAutoRunning) return false;
            for(int i=0; i<SeqList.Count;i++) 
            {
                m_dStrtTimeAR[i] = cDEF.TICK._GetTickTime();
                if (!cDEF.FM.SysOptn.bOffAR[i]) SeqList[i].AutoRun();
                m_dScanTimeAR[i] = cDEF.TICK._GetTickTime() - m_dStrtTimeAR[i];
            }
            //Ok.
            return true;
        }
        //------------------------------------------------------------------------
        public void  AutoRunPart      (int iPart)
        {
            if(iPart<0 || iPart>=SeqList.Count) return;
            if(cDEF.FM.SysOptn.bOffAR[iPart]  ) return;
            if(cDEF.EPU._bHasErr              ) return;
            SeqList[iPart].AutoRun();

        }
        //------------------------------------------------------------------------
        public void  ResetPart        (int iPart)
        {
            if(iPart<0 || iPart>=SeqList.Count) return;
            SeqList[iPart].Reset();
        }
        //------------------------------------------------------------------------
        //Check Cycle Time.
        public double    GetUPH       (double ScanTime , int Cnt)
        {
            if (ScanTime <= 0) return 0.0;
            if (Cnt <= 0) return 0.0;

            return (3600.0 * (double)Cnt) / ((double)ScanTime / 1000.0);
        }
        //------------------------------------------------------------------------
        //Real Time Update.
        public void  Update1           (                )
        {
            int iCnt = 0;
            if (m_bRqExitPgm) return;

            try
            {
                //Update Motor State (Input).
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                cDEF.MOTR.Update(m_bRun);
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;

                //Check Motor Crash
                cDEF.MOTR.InspectCrash(m_iSeqStat);

                //Update I/O.(input)
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                cDEF.IO .UpdateInput      ();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;

                //Update Analog input
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                //cDEF.ALG .UpdateAnalogInput();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;

                //Check Button.
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                CheckButton     ();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;

                //Check Panel Button. 
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                CheckButtonPanel();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;
            
                //Update Actuator (Input).
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                cDEF.ACTR.Update();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;

                //Update I/O (output)
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                cDEF.IO .UpdateOutput();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;

                //Update Analog Output
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                //cDEF.ALG.UpdateAnalogOutput();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;

                //Update IO(Vac & KVM & DVC)
                UpdateAutoDO         (m_bAutoMode); //m_bRun

                //Update IO (DEVICE - CEIP. DEVICE-NET)
                m_dStrtTimeUP[iCnt] = cDEF.TICK._GetTickTime_us();
                cDEF.IO.Update();
                m_dScanTimeUP[iCnt] = cDEF.TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt]; iCnt++;


            }
            catch (Exception e)
            {
                Debug.WriteLine("Seq Update 1" + e.Message);
                cDEF.LOG.ExceptionTrace("Seq Update 1", e);
                return;
            }
        }
        //------------------------------------------------------------------------
        public void  Update2           (                )
        {
            if (m_bRqExitPgm) return;

            try
            {
                //Update ErrorProc.
                cDEF.EPU.HasError();

                //Manual Cycle Running.`
                if (!cDEF.MAN._bOneShot) cDEF.MAN.ManCycleRun();

                cDEF.MAN.ManFunction();

                //Update Sequence Status (Lamp && Buzzer && Efficiency).
                UpdateSeqState();

                //
                for (int i = 0; i < SeqList.Count; i++)
                {
                    SeqList[i].StatusRun();
                }



            }
            catch (Exception e)
            {
                Debug.WriteLine("Seq Update 2" + e.Message);
                cDEF.LOG.ExceptionTrace("Seq Update 2", e);
                return;
            }
        }
        //------------------------------------------------------------------------
        public void Update3()
        {
            int iCnt = 10;
            if (m_bRqExitPgm) return;

            try
            {
                //cDEF.LogTP.Update();
                
                //Update Sequence Part
                for(int i=0; i<SeqList.Count;i++) SeqList[i].Update();
                            
                //Inspection.
                m_dStrtTimeUP[iCnt] = TICK._GetTickTime_us();
                InspectMainAir   ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime_us();
                InspectEmergency ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime_us();
                InspectSafety    ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime_us();
                InspectActuator  ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime_us() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime_us();
                InspectMotor     ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime();
                InspectHomeEnd   ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime();
                //InspectSensor ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime();
                //InspectHold      ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime();
                //InspectTempCtrl  ();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime();
                InspectFan();
                m_dScanTimeUP[iCnt] = TICK._GetTickTime() - m_dStrtTimeUP[iCnt++];

                m_dStrtTimeUP[iCnt] = TICK._GetTickTime();
                //
                m_dScanTimeUP[iCnt] = TICK._GetTickTime() - m_dStrtTimeUP[iCnt++];

            }
            catch (Exception e)
            {
                Debug.WriteLine("Seq Update 3" + e.Message);
                cDEF.LOG.ExceptionTrace("Seq Update 3", e);
                return;
            }

        }
        //------------------------------------------------------------------------
		public void  UpdateSeqState    (                )
        {
            //Local Var.
            bool isErr     = cDEF.EPU._bHasErr;
            bool isWrn     = cDEF.EPU._bHasWrn;
            bool isDsp     = cDEF.EPU._bHasDsp;
            bool isWorkEnd = m_bWorkEndState; 

            //
            try
            {
                //Flicking Timer.
                if (m_bFlick1) { m_FlickOnTimer1 .Clear(); if (m_FlickOffTimer1.OnDelay( m_bFlick1 , 500 )) m_bFlick1 = false; }
                else           { m_FlickOffTimer1.Clear(); if (m_FlickOnTimer1 .OnDelay(!m_bFlick1 , 500 )) m_bFlick1 = true ; }
                if (m_bFlick2) { m_FlickOnTimer2.Clear (); if (m_FlickOffTimer2.OnDelay( m_bFlick2 , 1000)) m_bFlick2 = false; }
                else           { m_FlickOffTimer2.Clear(); if (m_FlickOnTimer2 .OnDelay(!m_bFlick2 , 1000)) m_bFlick2 = true ; }
                if (m_bFlick3) { m_FlickOnTimer3.Clear (); if (m_FlickOffTimer3.OnDelay( m_bFlick3 , 2000)) m_bFlick3 = false; }
                else           { m_FlickOffTimer3.Clear(); if (m_FlickOnTimer3 .OnDelay(!m_bFlick3 , 2000)) m_bFlick3 = true ; }
                m_bEdgeFlick1 = m_EdgeFlick1.IsRising(m_bFlick1);
                m_bEdgeFlick2 = m_EdgeFlick2.IsRising(m_bFlick2);
                m_bEdgeFlick3 = m_EdgeFlick3.IsRising(m_bFlick3);

                //Set Sequence State.
                int iLampBuzzStat = (int)m_iSeqStat;

                     if ( cDEF.MAN._bHoming        ) { m_iSeqStat = EN_SEQ_STAT.Init    ; }
                else if ( isErr                    ) { m_iSeqStat = EN_SEQ_STAT.Error   ; }
                else if ( isDsp                    ) { m_iSeqStat = EN_SEQ_STAT.RunWarn ; }
                else if ( isWrn                    ) { m_iSeqStat = EN_SEQ_STAT.Warning ; }
                else if ( m_bRun                   ) { m_iSeqStat = EN_SEQ_STAT.Running ; }
                else if ( isWorkEnd                ) { m_iSeqStat = EN_SEQ_STAT.WorkEnd ; }
              //else if ( IO.IsDoorOpen()          ) { m_iSeqStat = EN_SEQ_STAT.DoorOpen; }
                else                                 { m_iSeqStat = EN_SEQ_STAT.Stop    ; }

                //Update Error Lamp & Buzz.
                cDEF.LampBuzz.Update(iLampBuzzStat);
                
                //SPC.
                cDEF.SPC.Update(m_iSeqStat);
            }
            catch (Exception e)
            {
                cDEF.LOG.ExceptionTrace("UpdateSeqState", e);
                Debug.WriteLine($"[UpdateSeqState] Exception : {e.Message}");
                return;
            }
        }

        //------------------------------------------------------------------------
        public void UpdateAutoDO(bool Run)
        {
            //
            int iCnt = m_AutoDOCtrl.Length; 
            //if (m_AutoDOCtrl[0].OnDelay(cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON    ) && Run, 3000)) { cDEF.IO.sY(EN_OUT_ID.yVACUUM_ON   , false); }
            if (m_AutoDOCtrl[1].OnDelay(cDEF.IO.gY(EN_OUT_ID.yVACUUM_PURGE ) , 300)) { cDEF.IO.sY(EN_OUT_ID.yVACUUM_PURGE, false); } //

        }
        //------------------------------------------------------------------------

        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  LoadSys          (bool IsLoad) //전체적인 Para 로딩.
        {
            cDEF.IO  .Load(IsLoad);
            cDEF.ACTR.Load(IsLoad);
            cDEF.MOTR.Load(IsLoad, cDEF.FM._sCrntDevice);

        }
        //------------------------------------------------------------------------
        public void  Load            (BinaryReader br             )
        {
            m_sSpare1  = br.ReadString().Trim();
            m_sSpare2  = br.ReadString().Trim();
            m_sSpare3  = br.ReadString().Trim();
            m_sSpare4  = br.ReadString().Trim();
            m_sSpare5  = br.ReadString().Trim();
                         
            m_bLoadStop= br.ReadBoolean();
            
            m_bSpare1  = br.ReadBoolean();
            m_bSpare2  = br.ReadBoolean();
            m_bSpare3  = br.ReadBoolean();
            m_bSpare4  = br.ReadBoolean();
            m_bSpare5  = br.ReadBoolean();
            //m_bSpare6  = br.ReadBoolean();
            //m_bSpare7  = br.ReadBoolean();
            //m_bSpare8  = br.ReadBoolean();
            //m_bSpare9  = br.ReadBoolean();
            //m_bSpare10 = br.ReadBoolean();

            m_iSpare1  = br.ReadInt32();
            m_iSpare2  = br.ReadInt32();
            m_iSpare3  = br.ReadInt32();
            m_iSpare4  = br.ReadInt32();
            m_iSpare5  = br.ReadInt32();
            //m_iSpare6  = br.ReadInt32();
            //m_iSpare7  = br.ReadInt32();
            //m_iSpare8  = br.ReadInt32();
            //m_iSpare9  = br.ReadInt32();
            //m_iSpare10 = br.ReadInt32();

            m_dSpare1  = br.ReadDouble();
            m_dSpare2  = br.ReadDouble();
            m_dSpare3  = br.ReadDouble();
            m_dSpare4  = br.ReadDouble();
            m_dSpare5  = br.ReadDouble();
            //m_dSpare6  = br.ReadDouble();
            //m_dSpare7  = br.ReadDouble();
            //m_dSpare8  = br.ReadDouble();
            //m_dSpare9  = br.ReadDouble();
            //m_dSpare10 = br.ReadDouble();

        }
        //------------------------------------------------------------------------
        public void  Save            (BinaryWriter wr)
        {

            wr.Write(m_sSpare1 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare2 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare3 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare4 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare5 .PadRight(vDEF.MAX_STR_LEN, ' '));

            wr.Write(m_bLoadStop);

            wr.Write(m_bSpare1  );
            wr.Write(m_bSpare2  );
            wr.Write(m_bSpare3  );
            wr.Write(m_bSpare4  );
            wr.Write(m_bSpare5  );
            //wr.Write(m_bSpare6  );
            //wr.Write(m_bSpare7  );
            //wr.Write(m_bSpare8  );
            //wr.Write(m_bSpare9  );
            //wr.Write(m_bSpare10 );
                                
            wr.Write(m_iSpare1  );
            wr.Write(m_iSpare2  );
            wr.Write(m_iSpare3  );
            wr.Write(m_iSpare4  );
            wr.Write(m_iSpare5  );
            //wr.Write(m_iSpare6  );
            //wr.Write(m_iSpare7  );
            //wr.Write(m_iSpare8  );
            //wr.Write(m_iSpare9  );
            //wr.Write(m_iSpare10 );
                                
            wr.Write(m_dSpare1  );
            wr.Write(m_dSpare2  );
            wr.Write(m_dSpare3  );
            wr.Write(m_dSpare4  );
            wr.Write(m_dSpare5  );
            //wr.Write(m_dSpare6  );
            //wr.Write(m_dSpare7  );
            //wr.Write(m_dSpare8  );
            //wr.Write(m_dSpare9  );
            //wr.Write(m_dSpare10 );
        }
        //------------------------------------------------------------------------
        public void  LoadWorkInfo    (bool IsLoad                             ) //작업에 관련된 Binary 로딩.
        {
            //Local Var.
            string Path = Application.StartupPath + "\\SeqData";

            //Make Dir.
            FNC.CreateDir (Path);
            
            //File Open.
            Path += "\\Sequence.DAT";

            //File Open.
            int iFAccess  = IsLoad ? (int)FileAccess.Read : (int)FileAccess.Write;
            FileStream fp = new FileStream(Path, FileMode.OpenOrCreate, (FileAccess)iFAccess);

            if(IsLoad) 
            {
                BinaryReader br = new BinaryReader(fp);
                if(br.PeekChar()<0) return;
                for(int i=0; i<SeqList.Count;i++) SeqList[i].Load(br);
                br.Close();
                br = null;
            }   
            else 
            {
                BinaryWriter wr = new BinaryWriter(fp);
                for(int i=0; i<SeqList.Count;i++) SeqList[i].Save(wr);
                wr.Close();
                wr = null;
            }
            fp = null;
        }
        //------------------------------------------------------------------------
        public void  CrntStatTrace   (EN_SEQ_ID Part , string Title = "") //Hold시 현재 작업정보 저장.
        {
            //Local Var.
            string Msg         = "";
            DateTime CurrDate  = DateTime.Now;
            string OccDateTime = string.Format("{0:yyMMdd_}", DateTime.Now) + string.Format("{0:HHmm}", DateTime.Now);
            string Path        = Application.StartupPath + "\\LOG\\CRNT_STAT";
            string FileName    = Path + "\\[" +  string.Format("{0:yyMMdd_HH}", DateTime.Now) + "]CRNT_STAT.LOG";

            //Make Dir.
            FNC.CreateDirOnWork("LOG");
            FNC.CreateDirOnWork("LOG\\CRNT_STAT");


            //Open Trace File.
            FileStream fp = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);

            //Save Working Info. 
 
            //Save Step Informations.
            Msg += ">>> " + CurrDate.ToString() + " <<<\r\n";
            Msg += (Title != "") ? Title + "\r\n" : Title;
            Msg += "-----------------------------------------------------------------------------------------------------------\r\n";
            for(int i=0; i<SeqList.Count;i++) 
            {
                if((Part != EN_SEQ_ID.ALL) && ((int)Part != i)) continue;
                Msg += SaveLog(i);
            }
            Msg += SaveLog(SeqList.Count);
            Msg += "-----------------------------------------------------------------------------------------------------------\r\n\r\n";

            //Save Log.
            StreamWriter sw = new StreamWriter(fp, Encoding.Default);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.Write(Msg);
            sw.Flush();
            sw.Close();

        }
        //------------------------------------------------------------------------
        public bool  SaveWorkInfo    ()
        {
            LoadWorkInfo(false); //Save All Sequence Parameters.
            cDEF.FM .LoadLastInfo(false);
            
            //cDEF.DM .LoadMap     (false); //Map Save.
            cDEF.SPC.Load        (false);
            cDEF.LOT.LoadLot     (false);

            cDEF.FM .SysCnt .Load(false, cDEF.FM._sCrntDevice);
            return true;
        }

        //Status Update Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void UpdateGrid(int iPart, ref System.Windows.Forms.DataGridView pGrid)
        {

            int i;
            int iTotWidth    = 0;
	        int[]     iWidth = {0, 100};
	        string[]  sItem  = {"NAME", "VALUE"};
            string    sName, sValue;

            pGrid.Visible  = false;
            if(pGrid.RowCount == 0) 
            {

                FNC.SetGridStyle(ref pGrid);
                pGrid.Dock                     = System.Windows.Forms.DockStyle.Top;

                for(i=0;i<2;i++) 
                {
                    pGrid.Columns.Add(sItem[i] , sItem[i]);
                    pGrid.Columns[i].Width = iWidth[i];
                    iTotWidth += iWidth[i];
                }
                pGrid.Columns[0].Width = pGrid.Width - iTotWidth-20;
                pGrid.Columns[0].DefaultCellStyle.WrapMode  = DataGridViewTriState.True;
                pGrid.Columns[0].DefaultCellStyle.BackColor = Color.Silver;
                for(i=0; i<50;i++) 
                {
                    if(!UpdateListVal(iPart, i, out sName, out sValue)) break; 
                    pGrid.Rows.Add(sName,  sValue);
                }
            }
            else 
            {
                for(i=0; i<pGrid.RowCount;i++) 
                {
                    if(!UpdateListVal(iPart, i, out sName, out sValue)) break; 
                    pGrid[1,i].Value = sValue;
                }
            }
            pGrid.Visible   = true;
        }
        //------------------------------------------------------------------------
        public void UpdateScanTimeGrid(int iPart, ref System.Windows.Forms.DataGridView pGrid)
        {
            int    i;
            int    iTotWidth    = 0;
	        int[]  iWidth = {0, 60};
            int    iMaxRow;

            pGrid.Visible = false;
            if(pGrid.RowCount == 0) 
            {
                FNC.SetGridStyle(ref pGrid, 30, true, false, false);
                //
                for (i=0;i<iWidth.Length;i++) 
                {
                    pGrid.Columns.Add("", "");
                    pGrid.Columns[i].Width = iWidth[i];
                    iTotWidth += iWidth[i];
                }
                pGrid.Columns[0].Width = pGrid.Width - iTotWidth-20;
                pGrid.Columns[0].DefaultCellStyle.WrapMode  = DataGridViewTriState.True;
                pGrid.Columns[0].DefaultCellStyle.BackColor = Color.Silver;
                //
                iMaxRow = (iPart == 0) ? m_dScanTimeUP.Length : m_dScanTimeAR.Length;
                for(i=0; i<iMaxRow;i++) 
                {
                    if (iPart == 0)
                        pGrid.Rows.Add(i+1,  (int)m_dScanTimeUP[i]);
                    else
                        pGrid.Rows.Add(Enum.GetName(typeof(EN_SEQ_ID), i),  (int)m_dScanTimeAR[i]);
                }
            }
            else {
                for(i=0; i<pGrid.RowCount;i++) 
                {
                    pGrid[1,i].Value = (iPart == 0) ? (int)m_dScanTimeUP[i] : (int)m_dScanTimeAR[i];
                }
            }
            pGrid.Visible   = true;
        }
        //------------------------------------------------------------------------
        public String SaveLog(int iPart)
        {
            string sMsg = "";    
            string sName, sValue;

	        sMsg += cDEF.POSN.GetPartName((int)iPart) + Convert.ToString("\r\n");
            for(int i=0; i<50;i++) 
            {
                if(!UpdateListVal(iPart, i, out sName, out sValue)) break; 
                sMsg += String.Format("{0} = {1}\r\n" , sName, sValue);
            }
            return sMsg;
        }
        //------------------------------------------------------------------------
        public bool UpdateListVal(int iPart, int no, out string sName, out string sValue)
        {//UserSet - FrmAdmin-OnCycle 화면 및 TimeOut Log에 저장할 변수 처리  
            sName  = "";
            sValue = "";
            object obj = new object(); 

            if(iPart<0 || iPart>=SeqList.Count)
            {

                if(no ==  0) {sName = "m_bRun    "; obj = m_bRun     ; }
                if(no ==  1) {sName = "m_bLtStop "; obj = m_bLtStop  ; }
                if(no ==  2) {sName = "m_iSeqStat"; obj = m_iSeqStat ; }
                if(no ==  3) {sName = "m_iStep   "; obj = m_iStep    ; }
                if(sName == "") return false;
                sName.Trim();
                sValue = obj.ToString(); 
                return true;
            }
            obj = null; 
            
            return SeqList[iPart].UpdateListVal(no, out sName, out sValue);  
        }
        //--------------------------------------------------------------------------
        public void SetCmdData(cCmdData data)
        {
            m_SeqCmdData = data;
        }
        //--------------------------------------------------------------------------
        public void ClearCmdData()
        {
            m_SeqCmdData.ClearCMD() ;
        }

    }
}
