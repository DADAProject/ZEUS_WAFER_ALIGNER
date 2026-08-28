using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Media;
using static eMachine.cDEF;

namespace eMachine
{
    public class TSeqWaferAlignTable:TSeqUnit
    {
        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_tHome      = new TOnDelayTimer();
        TOnDelayTimer m_tToStop    = new TOnDelayTimer();
        TOnDelayTimer m_tToStart   = new TOnDelayTimer();
        TOnDelayTimer m_tTemp      = new TOnDelayTimer(); //Temp. Timer.
        TOnDelayTimer m_tWait      = new TOnDelayTimer(); //
        TOnDelayTimer m_tCyleMain  = new TOnDelayTimer(); //Main Cycle Timer.
        TOnDelayTimer m_tCyleWait  = new TOnDelayTimer(); //       
        TOnDelayTimer m_tCyleAlgn  = new TOnDelayTimer(); //
        TOnDelayTimer m_tHomeDelay = new TOnDelayTimer();
        TOnDelayTimer m_tVacErr    = new TOnDelayTimer();

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        EN_SEQ_ID   m_iPartId   ;
        EN_MOTR_ID  m_iMotrX    ;
        EN_MOTR_ID  m_iMotrY    ;
        EN_MOTR_ID  m_iMotrT    ;

        cCmdData m_AutoCmdData;
        TVisnRslt m_VisnRslt      = new TVisnRslt();
        TVisnRslt m_OtherVisnRslt = new TVisnRslt();

        DateTime[] VisnTime  = new DateTime[2];

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */

        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool        m_bToStart      ; //To... Flag.
        bool        m_bToStop       ;
        bool        m_bWorkEnd      ;

        bool        m_bDrngAlgn     ;
        bool        m_bDrngWait     ;

        int         m_iStepSeq      ; //Step.
        int         m_iStepMan      ;
        int         m_iStepHome     ;
        int         m_iStepAlgn     ;
        int         m_nAlignCount   ; //Daily Align Count
        int         m_RetryCnt      ; //Fail 시 Retry Count
        int         m_AlignCnt      ; //Vision 검사 횟수
        int         m_nManTestCnt   ;

        bool        m_bReqAlign     ;
        bool        m_bReqWait      ;
        bool        m_bAlignChecked ;
        int         m_nVerifyCnt    ;

        double      m_dDirectPosn   ; //Direct Moving Position.
        string      m_sLogMoveEvt   ;
        string      m_sTemp         ;

        EN_VISN_STEP enVisnMode     ;

        //Internal Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        double      m_dMPosX        ;
        double      m_dMPosY        ;
        double      m_dMPosT        ;
                    
        //Spare Var.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        string  m_sSpare1  ;
        string  m_sSpare2  ;
        bool    m_bSpare1, m_bSpare2;
        int     m_iSpare1, m_iSpare2;
        double  m_dSpare1, m_dSpare2;

        //SCAN TIME
        public double[]   m_dScanTime = new double[20];
        public double[]   m_dStrtTime = new double[20];

        string m_sWorkMsg = string.Empty;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int  _iStepMan     { get { return m_iStepMan     ;} set { m_iStepMan    = value; }}
        public int  _iStepSeq     { get { return m_iStepSeq     ;} set { m_iStepSeq    = value; }}
        public int  _iStepAlgn    { get { return m_iStepAlgn    ;} /*set { m_iStepAlgn   = value;}*/ }
        public int _nAlignCount   => m_nAlignCount;
        public int  _RetryCnt     => m_RetryCnt   ;
        public int  _AlignCnt     => m_AlignCnt   ;
        
        public int  _nManTestCnt  => m_nManTestCnt;
        public bool _bWorkEnd     { get { return m_bWorkEnd     ;} set { m_bWorkEnd    = value; }}           
        public bool _bDrngAlgn    { get { return m_bDrngAlgn    ;} set { m_bDrngAlgn   = value; }}
        public bool _bDrngWait    { get { return m_bDrngWait    ;} set { m_bDrngWait   = value; }}

        //public bool _bReqAlign    { get { return m_bReqAlign    ;} set { m_bReqAlign   = value; }}
        //public bool _bReqWait     { get { return m_bReqWait     ;} set { m_bReqWait    = value; }}
        public string _sWorkMsg => m_sWorkMsg;

        public DateTime _StartTime { get { return VisnTime[0]; } }


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSeqWaferAlignTable()
        {                  
            m_sSpare1       = string.Empty;
            m_sSpare2       = string.Empty;
            m_sLogMoveEvt   = string.Empty;

            m_iPartId       = EN_SEQ_ID .WAT    ;
            m_iMotrX        = EN_MOTR_ID.WAT_X  ;
            m_iMotrY        = EN_MOTR_ID.WAT_Y  ;
            m_iMotrT        = EN_MOTR_ID.WAT_T  ;

            m_dMPosX        = MOTR.GetEncPos(m_iMotrX);
            m_dMPosY        = MOTR.GetEncPos(m_iMotrY);
            m_dMPosT        = MOTR.GetEncPos(m_iMotrT);

            m_AutoCmdData   = new cCmdData(eCommand.None, null);
            m_VisnRslt     .ResetData();
            m_OtherVisnRslt.ResetData();

            m_nAlignCount   = 0;
            m_RetryCnt      = 0;
            m_nManTestCnt   = 0;
            m_AlignCnt      = 0;
            enVisnMode      = EN_VISN_STEP.ALIGN;
            m_bAlignChecked = false;

            m_sTemp         = string.Empty;
            m_nVerifyCnt    = 0;


            Init();
        }
        ~TSeqWaferAlignTable() { }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public override void Init()
        {
            
            m_bToStart      = false;
            m_bToStop       = false;
            m_bWorkEnd      = false;

            m_bDrngAlgn     = false;
            m_bDrngWait     = false;

            //Step 
            m_iStepSeq      = 0;
            m_iStepMan      = 0;
            m_iStepHome     = 0;
            m_iStepAlgn     = 0;
                            
            m_dDirectPosn   = 0.0;

            m_bReqAlign     = false;
            m_bReqWait      = false;

            //Clear Timer.
            m_tCyleMain .Clear();
            m_tHome     .Clear();
	        m_tToStop   .Clear();
	        m_tToStart  .Clear();
	        m_tTemp     .Clear();
	        m_tWait     .Clear();
            m_tCyleAlgn .Clear();
            m_tHomeDelay.Clear();
            
            //MOTR.Stop(m_iMotrT);

            m_AutoCmdData.ClearCMD();

            enVisnMode = EN_VISN_STEP.ALIGN;
            m_bAlignChecked = false;

        }
        //------------------------------------------------------------------------

        public override void Reset()
        {
            Init();
            
            ClearCalPos(); //JUNG/240808

        }

        public override void ClearHomeStep()
        {
            m_iStepHome = 10;
            m_tHome.Clear();
        }
        public override void ClearWorkEnd()
        {
            m_bWorkEnd = false;
        }
        public override int GetSeqStep()
        {
            return m_iStepSeq;
        }
        public override bool IsWorkEnd()
        {
            return m_bWorkEnd;
        }

        public override void Update()
        {


        }

        //Check disturbing.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public override bool CheckDstb(EN_MOTR_ID Motr, EN_COMD_ID Cmd = EN_COMD_ID.NoneCmd, 
                                                         int Step = vDEF.NONE_STEP, EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.NONE, double DirPosn = 0.0)
        {
	        ////Local Var.
            //bool              IsMovePlus;
            //String            sPartName      =  cDEF.POSN.GetPartName((int)m_iPartId); 
	        //bool              isOpenDoor     =  cDEF.SEQ.IsOpenAnyDoor()             ;
	        //bool              isDoorLock     =  true                                 ;
	        //bool              isNoRun        = (cDEF.SEQ._iStep == 0) &&   (cDEF.SEQ._iSeqStat != EN_SEQ_STAT.Init ) && m_iStepMan==0 && m_iStepHome==0;          
	        //double            dNextPosn;
            //
            //double            dEncPosWTR_Y   =  cDEF.MOTR.GetEncPos(m_iMotrY);
            //double            dCmdPosWTR_Y   =  cDEF.MOTR.GetCmdPos(m_iMotrY);
            //double            dTrgPosWTR_Y   =  cDEF.MOTR.GetTrgPos(m_iMotrY);
            //
			////
            //dNextPosn = cDEF.MOTR.GetNextCmdTrg(Motr, Cmd, Step, FIndex, DirPosn);
            ////
            //     if (Cmd == EN_COMD_ID.JogP  ) { IsMovePlus = true ; dNextPosn += 5.0; }
            //else if (Cmd == EN_COMD_ID.JogN  ) { IsMovePlus = false; dNextPosn -= 5.0; }
            //else if (Cmd == EN_COMD_ID.Home  )   IsMovePlus = false; 
            //else
            //{        
            //    if (Cmd == EN_COMD_ID.Direct) IsMovePlus = DirPosn   > cDEF.MOTR[(int)Motr].GetEncPos();                   
            //    else                          IsMovePlus = dNextPosn > cDEF.MOTR[(int)Motr].GetEncPos();
            //}
            //
	        ///Check this part.
	        if (Motr != m_iMotrX && Motr != m_iMotrY && Motr != m_iMotrT) return false;
            //
	        //if (isOpenDoor) 
            //{
		    //    cDEF.MOTR.Stop(Motr);
		    //    if (isNoRun) FRM.ShowWarn(true, string.Format("[{0}] 도어 열림 센서가 감지되어 모터가 움직일 수 없습니다.", sPartName)); 
		    //    return false;
		    //}
            //
	        ////Check Door Lock.
	        //if (!isDoorLock) 
            //{
		    //    cDEF.MOTR.Stop(Motr);
		    //    if (isNoRun) FRM.ShowWarn(true, string.Format("[{0}] 도어록 센서가 감지되어 모터가 움직이지 않습니다.", sPartName));
		    //    return false;
		    //}
            //
	        ////Check Safety. 
	        //if (cDEF.SEQ._bNoSafety) 
            //{
		    //    cDEF.MOTR.Stop(Motr);
		    //    if (isNoRun) FRM.ShowWarn(true, string.Format("[{0}] 모든 안전 점검(에어리어 센서의 도어 확인, 모터가 움직일 수 없음)", sPartName)); 
		    //    return false;
		    //}
            //
	        ////Check HomeEnd & Alarm.
            //if (!cDEF.MOTR[(int)m_iMotrT].GetReady()) { if (isNoRun) FRM.ShowWarn(true, string.Format("[{0}] {1}Motor가 준비되지 않아 모터를 움직일 수 없습니다.", sPartName, cDEF.POSN.GetMotorName((int)m_iMotrT  ))); return false; }
            //if (!cDEF.MOTR[(int)m_iMotrY].GetReady()) { if (isNoRun) FRM.ShowWarn(true, string.Format("[{0}] {1}Motor가 준비되지 않아 모터를 움직일 수 없습니다.", sPartName, cDEF.POSN.GetMotorName((int)m_iMotrY))); return false; }
            //
            ////             //With Cylinder
            ////             if(IsWaferExist(false))
            ////             {
            ////                 //check Vacuum
            ////                 if (!IsVacStat(true))
            ////                 {
            ////                     if (isNoRun) FRM.ShowWarn(true, string.Format("Wafer가 있으니 Vacuum 상태를 확인하세요"));
            ////                     cDEF.MOTR.Stop(Motr);
            ////                     return false;
            ////                 }
            ////             }
            //
            //
            //
            ////
            return true;
        }

        //Request Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        
        public void SetReqAlign(bool set = true)
        {
            m_bReqAlign = set;
        }
        //--------------------------------------------------------------------------
        public void SetReqWait(bool set = true)
        {
            m_bReqWait = set;
        }
        //------------------------------------------------------------------------
        public bool IsWaferExist(bool Val = false)
        {
            if (cDEF.FM.SysOptn.iRunSkipMat == 1) return Val;
            

            return cDEF.IO.gX(EN_IN_ID.xWAFER_EXIST); 
        }

        //------------------------------------------------------------------------
        #region Find Unknown & Loss PKG
        public bool FindUnknown(EN_WAF_ID Wher = EN_WAF_ID.WAT)
        {
            //
            if (FM.SysOptn .bSkipVac[(int)Wher]) return false;
            //
            bool isErr = IsWaferExist(false) ;
            //
            return isErr;
        }
        //---------------------------------------------------------------------------
        public bool FindLoss(EN_WAF_ID Wher = EN_WAF_ID.WAT)
        {
            //
            if (cDEF.FM.SysOptn .bSkipVac[(int)Wher]) return false;
            //
            bool isErr = !IsWaferExist(true);
            //
            return isErr;
        }
        //------------------------------------------------------------------------
        #endregion
        public bool IsLocateWait(bool isMove)
        {
            if (FM.IsDryMode()) return true; 

            bool r1, r2, r3;

            if (!isMove)
            {
                r1 = MOTR.CmprPos(m_iMotrX, MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.Wait1]);
                r2 = MOTR.CmprPos(m_iMotrY, MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.Wait1]);
                r3 = MOTR.CmprPos(m_iMotrT, MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.Wait1]);
                return r1 && r2 && r3;
            }
            else
            {
                r1 = MoveMotr(m_iMotrX, EN_COMD_ID.Wait1);
                r2 = MoveMotr(m_iMotrY, EN_COMD_ID.Wait1);
                r3 = MoveMotr(m_iMotrT, EN_COMD_ID.Wait1);

                return r1 && r2 && r3;
            }
        }
        //--------------------------------------------------------------------------
        public bool SetVacOn()
        {
            IO.sY(EN_OUT_ID.yVACUUM_ON   ,  true );
            IO.sY(EN_OUT_ID.yVACUUM_PURGE,  false);
            
            bool r1 =  IO.gY(EN_OUT_ID.yVACUUM_ON   ,  true );
            bool r2 = !IO.gY(EN_OUT_ID.yVACUUM_PURGE,  false);

            return (r1 && r2);
        }
        //--------------------------------------------------------------------------
        public bool SetVacOff()
        {
            IO.sY(EN_OUT_ID.yVACUUM_ON   , false);
            IO.sY(EN_OUT_ID.yVACUUM_PURGE, true );
            
            bool r1 = !IO.gY(EN_OUT_ID.yVACUUM_ON   , false);
            bool r2 =  IO.gY(EN_OUT_ID.yVACUUM_PURGE, true );

            return (r1 && r2);
        }
        //--------------------------------------------------------------------------
        public bool IsVacOn()
        {
            bool bUseSkipVac = FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WAT];

            return bUseSkipVac ? true : IO.gX(EN_IN_ID.xVACUUM_ON);
        }
        //--------------------------------------------------------------------------
        public bool SetLight(bool set)
        {
            //cDEF.VISN.SetLightOn(set, (int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn);
            IO.sY(EN_OUT_ID.yLightOn, set);

            return IO.gY(EN_OUT_ID.yLightOn) ;     
        }
        //------------------------------------------------------------------------

        //Motor.
        public override bool MoveMotr(EN_MOTR_ID Motr, EN_COMD_ID Cmd, EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal, 
                                              int Step = vDEF.NONE_STEP, EN_FPOSN_INDEX Index = EN_FPOSN_INDEX.NONE)
        {
            bool        bRet   ;
            //double      dPosn  ;
           
            //Stop Command. (Stop은 간섭을 확인 하지 않음)
            if (Cmd == EN_COMD_ID.Stop) return cDEF.MOTR.MoveAsComd(Motr , Cmd , iSPD, Step , Index);

            //Check Disturb.
            if (!CheckDstb(Motr , Cmd , Step, Index)) return false;

            //Jog Command
            if (Cmd == EN_COMD_ID.JogP) return cDEF.MOTR.MoveAsComd(Motr , Cmd , iSPD, Step , Index);
            if (Cmd == EN_COMD_ID.JogN) return cDEF.MOTR.MoveAsComd(Motr , Cmd , iSPD, Step , Index);

	        //Find Step.
            if (((Cmd == EN_COMD_ID.FindStep1) ||
                 (Cmd == EN_COMD_ID.FindStep2) || 
                 (Cmd == EN_COMD_ID.FindStep3) ||
                 (Cmd == EN_COMD_ID.FindStep4))) {
		        return cDEF.MOTR.MoveAsComd(Motr , Cmd , iSPD, Step  , Index);
		        }

            //Command.
	        bRet =  cDEF.MOTR.MoveAsComd(Motr , Cmd , iSPD , Step , (EN_FPOSN_INDEX)Index);

            if(bRet) {                
                //dPosn  = cDEF.MOTR[(int)Motr].GetPosToCmdId(Cmd);
                //if (m_sLogMoveEvt == "" || m_sLogMoveEvt == null) m_sLogMoveEvt = "MANUAL"; 
                //cDEF.LogTP.FunctionMove ((int)m_iPartId, m_sLogMoveEvt, (int)Motr, dPosn);
                }
            return bRet;
        }
        //------------------------------------------------------------------------
        public override bool MoveDirect(EN_MOTR_ID Motr, double Posn)
        {
            //Set Direct Position.
            m_dDirectPosn = Posn;

            //Move.
	        if (!CheckDstb           (Motr , EN_COMD_ID.Direct ,                       vDEF.NONE_STEP , EN_FPOSN_INDEX.NONE , Posn)) return false;
            if (!cDEF.MOTR.MoveAsComd(Motr , EN_COMD_ID.Direct , EN_MOTR_VEL.Normal  , vDEF.NONE_STEP , EN_FPOSN_INDEX.NONE , Posn)) return false;

            if (m_sLogMoveEvt == "" || m_sLogMoveEvt == null) m_sLogMoveEvt = "MANUAL"; 

            //Reset Direct Position.
            m_dDirectPosn = 0.0;

            //Ok.
            return true;
        }

        //---------------------------------------------------------------------------
        public override bool ReqMoveMotr(EN_MOTR_ID Motr, EN_COMD_ID Cmd, EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal, 
                                         int Step = vDEF.NONE_STEP, EN_FPOSN_INDEX Index = EN_FPOSN_INDEX.NONE)
        {
            //Check During.
            if (m_iStepSeq != 0) return false;
            //EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal;
            
            //Move.
            return MoveMotr(Motr , Cmd, iSPD, Step, Index);
        }
        //------------------------------------------------------------------------
        public override bool ReqMoveDirect(EN_MOTR_ID Motr, double Posn)
        {
            //Check During.
            if (m_iStepSeq != 0) return false;
            //Move.
            return MoveDirect(Motr , Posn);
        }
        //------------------------------------------------------------------------
        public override bool MoveToLastWorkPosn(EN_MOTR_ID Motr)
        {
            //Ok.
            return true;
        }
        //------------------------------------------------------------------------
        public override bool MoveToSafetyWaitPosn()
		{
            bool r1 = MoveMotr(m_iMotrX, EN_COMD_ID.Wait1);
            bool r2 = MoveMotr(m_iMotrY, EN_COMD_ID.Wait1);
            bool r3 = MoveMotr(m_iMotrT, EN_COMD_ID.Wait1);

            return (r1 && r2 && r3);
		}
        //------------------------------------------------------------------------
        public bool MoveToLastWorkPosn()
        {
            //Ok.
            return true;
        }
        //------------------------------------------------------------------------
        //Move Home.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public override bool MoveHome()
        {
            bool r1, r2, r3;
            int  iFHomeErr = MOTR._iFHomeErr;
	        if (m_tHome.OnDelay(m_iStepHome >= 10 , 90000)) 
            {
                cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0001 + (int)m_iPartId);
                m_iStepHome = 0;
                return true;  
            }

	        //Move Home.
	        switch (m_iStepHome) 
            {
		        case  0: m_iStepHome = 0;
				         break;          

		    //Move Cylinder
		        case 10:
                    
                    ClearCalPos(); //JUNG/240808

                    //Clear Alarm
                    MOTR.SetAlarm(m_iMotrX, true);
                    MOTR.SetAlarm(m_iMotrY, true);
                    MOTR.SetAlarm(m_iMotrT, true);
                    
                    m_tHomeDelay.Clear();
                    m_iStepHome++;
				    return false;

                case 11:
                    if (!m_tHomeDelay.OnDelay(true, 500)) return false;

                    //Servo On
                    if (!MOTR[(int)m_iMotrX].GetServo()) MOTR.SetServo(m_iMotrX, true);
                    if (!MOTR[(int)m_iMotrY].GetServo()) MOTR.SetServo(m_iMotrY, true);
                    if (!MOTR[(int)m_iMotrT].GetServo()) MOTR.SetServo(m_iMotrT, true);
                    
                    m_tHomeDelay.Clear();
                    m_iStepHome++;
                    return false;
                
                case 12:
                    if (!m_tHomeDelay.OnDelay(true, 300)) return false;

                    r1 = MOTR[(int)m_iMotrX].GetServo();
                    r2 = MOTR[(int)m_iMotrY].GetServo();
                    r3 = MOTR[(int)m_iMotrT].GetServo();

                    if (!r1 || !r2 || !r3) return false;

                    m_tHomeDelay.Clear();
                    m_iStepHome++;
				    return false;

                //Proc Home
		        case 13:
                    if (!m_tHomeDelay.OnDelay(true, 100)) return false;

                    MOTR.ClearHomeEnd(m_iMotrX );
                    MOTR.ClearHomeEnd(m_iMotrY );
                    MOTR.ClearHomeEnd(m_iMotrT );

                    EPU.SetErr(iFHomeErr + (int)m_iMotrX, true);
                    EPU.SetErr(iFHomeErr + (int)m_iMotrY, true);
                    EPU.SetErr(iFHomeErr + (int)m_iMotrT, true);

                    m_iStepHome++;
				    return false;
		    
		        case 14: 
                    
                    if(!CheckDstb (m_iMotrX, EN_COMD_ID.Home)) return false;
                    if(!CheckDstb (m_iMotrY, EN_COMD_ID.Home)) return false;
                    if(!CheckDstb (m_iMotrT, EN_COMD_ID.Home)) return false;
                    
                    r1 = MOTR.MoveHome(m_iMotrX);
                    r2 = MOTR.MoveHome(m_iMotrY);
                    r3 = MOTR.MoveHome(m_iMotrT);

                    EPU.SetErr(iFHomeErr + (int)m_iMotrX, !r1);
                    EPU.SetErr(iFHomeErr + (int)m_iMotrY, !r2);
                    EPU.SetErr(iFHomeErr + (int)m_iMotrT, !r3);

                    if (!r1 || !r2 || !r3) return false;

                    EPU.SetErr(iFHomeErr + (int)m_iMotrX, false);
                    EPU.SetErr(iFHomeErr + (int)m_iMotrY, false);
                    EPU.SetErr(iFHomeErr + (int)m_iMotrT, false);

                    m_iStepHome++;
				    return false;

				case 15: 
                    if (!MoveToSafetyWaitPosn()) return false;

				    m_iStepHome = 0;
				    break;
            }                      

            //
            return true;                                                                         
        }

        //Manual Cycle
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool ManOneCycle(bool conti = false)
        {//
            
            bool bDrngFlag = true;

            //
            if (m_iStepMan < 0) m_iStepMan = 0;
            switch (m_iStepMan)
            {
                default:
                         
                    m_iStepMan = 0;
                    break;

                case 10:
                   
                    m_nManTestCnt = 0; 

                    m_iStepAlgn = 10;
                    
                    m_iStepMan++;
                    return false;

                case 11: 
                    if (!AutoAlgnCycle(ref bDrngFlag, true)) return false;

                    m_iStepMan++;
                    return false;

                case 12: 

                    if (conti && (++m_nManTestCnt < FM.EngrOptn.nTestRunCnt)) //
                    {
                        LOG.Trace($"Align Continue Test = {m_nManTestCnt} /{FM.EngrOptn.nTestRunCnt}");
                       
                        m_iStepMan++;
                        m_tWait.Clear();
                        return false;
                    }

                    m_tWait.Clear();
                    m_iStepMan = 0;
                    return true;

                case 13:
                    if (!m_tWait.OnDelay(true, 3000)) return false;
                    
                    if(!SetVacOn()) return false;
                    
                    m_iStepAlgn = 10;


                    m_iStepMan = 11;
                    return false;

            }
            return false;
        }
        public bool ManGrabCycle( )
        {//

            bool bDrngFlag = true;
            string sTemp;

            //
            if (m_iStepMan < 0) m_iStepMan = 0;
            switch (m_iStepMan)
            {
                default:

                    m_iStepMan = 0;
                    break;

                case 10:
                    if (!SetVacOn()) return false;

                    m_iStepMan++;
                    return false;

                case 11:
                    //Data Clear
                    m_VisnRslt     .ResetData();
                    m_OtherVisnRslt.ResetData();
                    ClearCalPos();

                    m_iStepMan++;
                    return false;

                case 12:
                    //Light On
                    SetLight(true);

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 13:
                    if (!m_tWait.OnDelay(true, cDEF.VISN.GetLightDelay((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn))) return false;

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 14:
                    if (!cDEF.VISN.VisnGrabStrt((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn)) return false;
                    SetLight(false);

                    if (cDEF.FM.SysOptn.iTestMode[(int)EN_CAM.WTB] == (int)vDEF.CHCK_AWYS)
                    {
                        //검사 타입 분류
                        if (cDEF.FM.ProjBase.iWaferType == 0 &&
                            cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Type != "SAWING") //Wafer
                        {
                            m_VisnRslt     .SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn);
                            m_OtherVisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn);
                        }
                        else//Ring frame
                        {
                            m_VisnRslt     .SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn);
                            m_OtherVisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn);
                        }

                        //NG
                        if (!m_VisnRslt.Match && m_VisnRslt.InspRslt != (int)EN_ERR_LIST.ERR_NONE)
                        {
                            FRM.ShowMsg(true, $"{m_OtherVisnRslt.Type} - Grab Fail");
                        }
                        else
                        {
                            sTemp = string.Format("Type:{0}/X:{1:F3}/Y:{2:F3}/T:{3:F3}", m_VisnRslt.Type, m_VisnRslt.X, m_VisnRslt.Y, m_VisnRslt.T);
                            FRM.ShowMsg(true,"" , sTemp);
                        }
                        if (!m_OtherVisnRslt.Match && m_OtherVisnRslt.InspRslt != (int)EN_ERR_LIST.ERR_NONE)
                        {
                            FRM.ShowMsg(true, $"{m_OtherVisnRslt.Type} - Grab Fail");
                        }
                        else
                        {
                            sTemp = string.Format("Type:{0}/X:{1:F3}/Y:{2:F3}/T:{3:F3}", m_OtherVisnRslt.Type, m_OtherVisnRslt.X, m_OtherVisnRslt.Y, m_OtherVisnRslt.T);
                            FRM.ShowMsg(true, "", sTemp);
                        }
                    }
                    m_iStepMan++;
                    return false;

                case 15:
                    m_iStepMan = 0;
                    return true;

            }
            return false;
        }
        //------------------------------------------------------------------------
        public bool ManImgOneCycle(bool conti = false)
        {//

            bool bDrngFlag = true;

            //
            if (m_iStepMan < 0) m_iStepMan = 0;
            switch (m_iStepMan)
            {
                default:

                    m_iStepMan = 0;
                    break;

                case 10:

                    m_nManTestCnt = 0;

                    m_iStepAlgn = 10;

                    m_iStepMan++;
                    return false;

                case 11:
                    if (!AutoAlgnCycle(ref bDrngFlag, true, true)) return false;

                    m_iStepMan++;
                    return false;

                case 12:

                    if (conti && (++m_nManTestCnt < FM.EngrOptn.nTestRunCnt)) //
                    {
                        LOG.Trace($"Align Continue Test = {m_nManTestCnt} /{FM.EngrOptn.nTestRunCnt}");
                    
                        m_iStepMan++;
                        m_tWait.Clear();
                        return false;
                    }

                    m_tWait.Clear();
                    m_iStepMan = 0;
                    return true;

                case 13:
                    if (!m_tWait.OnDelay(true, 3000)) return false;

                    if (!SetVacOn()) return false;

                    m_iStepAlgn = 10;


                    m_iStepMan = 11;
                    return false;

            }
            return false;
        }
        //--------------------------------------------------------------------------
        // Align Spec = 3sec (Move Wait 포함)
        //--------------------------------------------------------------------------
        public bool AutoAlgnCycle(ref bool DrngFlag, bool ManCycle = false, bool ImgTest = false)
        {//
            bool r1, r2, r3; 
            bool r4, r5, r6;

            //Align Cycle.
            if (m_iStepAlgn < 0) m_iStepAlgn = 0;
            switch (m_iStepAlgn)
            {
                default:
                    m_iStepAlgn = 0;
                    return true;

                case 10: //

                    DrngFlag = true;
                    m_dScanTime[0] = cDEF.TICK._GetTickTime() - m_dStrtTime[0];
                    m_dStrtTime[0] = cDEF.TICK._GetTickTime();
                    m_dStrtTime[1] = cDEF.TICK._GetTickTime();


                    //
                    m_bReqAlign     = false;
                    m_bAlignChecked = false;
                    m_RetryCnt      = 0;
                    m_AlignCnt      = 0;
                    enVisnMode      = EN_VISN_STEP.ALIGN;
                    m_nVerifyCnt    = 0 ;

                    //Que Clear
                    cDEF.VISN.Light[(int)EN_CAM.WTB].Reset(); //JUNG/240719추가

                    //Check Command
                    if (!ManCycle && m_AutoCmdData.Command != eCommand.AGN)
                    {
                        DrngFlag    = false;
                        m_iStepAlgn = 0;
                        m_sWorkMsg  = string.Empty;
                        return true;
                    }

                    //Check Light
                    if (!cDEF.VISN.Light[(int)EN_CAM.WTB].IsOpen)
                    {
                        EPU.SetErr(EN_ERR_LIST.ERR_0084, true); //Light Error

                        //Check Wafer 
                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0084;
                        COMZEUS.SetResult(m_AutoCmdData);

                        DrngFlag    = false;
                        m_iStepAlgn = 0;
                        m_sWorkMsg  = string.Empty;
                        return true;
                    }

                    //Check Wafer
                    if (!IsWaferExist(true))
                    {
                        EPU.SetErr(EN_ERR_LIST.ERR_0003, true); //Wafer Error

                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0003;
                        COMZEUS.SetResult(m_AutoCmdData);

                        DrngFlag    = false;
                        m_iStepAlgn = 0;
                        m_sWorkMsg  = string.Empty;
                        return true;
                    }

                    //
                    WriteSeqLog("Align Start");

                    m_sWorkMsg = "Start Align...";

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 11:
                    //if (!m_tWait.OnDelay(true, 100)) return false;

                    if (!SetVacOn()) return false;

                    WriteSeqLog("Vacuum On");

                    m_tVacErr.Clear();
                    m_tWait  .Clear();
                    m_iStepAlgn++;
                    return false;

                case 12: //                        
                    //if (!m_tWait.OnDelay(true, 500)) return false; //JUNG/230329/100 -> 500
                    if (!m_tWait.OnDelay(true, FM.EngrOptn.nVacDelay))
                    {
                        m_tVacErr.Clear();
                        return false; //JUNG/230330/Option 처리
                    }

                    //Check Vacuum ON
                    m_tVacErr.OnDelay(!IsVacOn(), 3 * 1000); //3sec
                    if (m_tVacErr.Out)
                    {
                        SetVacOff(); //JUNG/230602/고객사 요청사항

                        EPU.SetErr(EN_ERR_LIST.ERR_0002, true); //Vacuum Error

                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0002;
                        COMZEUS.SetResult(m_AutoCmdData);

                        m_sWorkMsg  = string.Empty;
                        DrngFlag    = false;
                        m_iStepAlgn = 0;
                        return true;
                    }

                    if (!IsVacOn()) return false;

                    //
                    m_sWorkMsg = "Move Wait Position...";

                    r1 = MoveMotr(EN_MOTR_ID.WAT_X, EN_COMD_ID.Wait1);
                    r2 = MoveMotr(EN_MOTR_ID.WAT_Y, EN_COMD_ID.Wait1);
                    r3 = MoveMotr(EN_MOTR_ID.WAT_T, EN_COMD_ID.Wait1);
                    if (!r1 || !r2 || !r3) return false;

                    WriteSeqLog("Move Wait Position");

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                case 13:
                    //if (!m_tWait.OnDelay(true, 100)) return false;
                    
                    WriteSeqLog("Vision Start");

                    //Data Clear
                    m_VisnRslt     .ResetData();
                    m_OtherVisnRslt.ResetData();
                    ClearCalPos();

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 14:

                    if(m_RetryCnt > 0)
                    {
                        if (!m_tWait.OnDelay(true, 500)) return false;
                    }
                    
                    //Start
                    VisnTime[0] = DateTime.Now;

                    m_sWorkMsg = "Start Grab...";

                    //Light On
                    if (!ImgTest)
                        SetLight(true);

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 15:
                    if (!m_tWait.OnDelay(true, cDEF.VISN.GetLightDelay((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn))) return false;

                    //Debug.WriteLine($"[Befor]X:{MOTR.GetEncPos(EN_MOTR_ID.WAT_X)} / Y:{MOTR.GetEncPos(EN_MOTR_ID.WAT_Y)} / T:{MOTR.GetEncPos(EN_MOTR_ID.WAT_T)}");

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 16:
                    if (!cDEF.VISN.VisnGrabStrt((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn, (ManCycle && ImgTest) ? cDEF.MAN.FuncArg.sArg1 : "")) return false;

                    //End
                    VisnTime[1] = DateTime.Now;

                    WriteSeqLog("Vision End");
                    SetLight(false);

                    r5 = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Match;
                    r6 = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).Match;

                    if (cDEF.FM.SysOptn.iTestMode[(int)EN_CAM.WTB] == (int)vDEF.CHCK_AWYS)
                    {
                        //검사 타입 분류
                        if (cDEF.FM.ProjBase.iWaferType == 0)
                        {
                            m_VisnRslt     .SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn);
                            m_OtherVisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn);
                        }
                        else//Ring frame
                        {
                            m_VisnRslt     .SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn);
                            m_OtherVisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn);
                        }
                        
                        //D컷 조명 추가에서는 Angle은 FAlgn.T값으로 적용.
                        if (cDEF.FM.EngrOptn.bUseDcutAlgnT && cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).Match)
                        {
                            m_VisnRslt.T = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).T;
                            //
                            if (cDEF.FM.ProjBase.iWaferType == 0)
                            {
                                cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.Calculate(m_VisnRslt.OriX - m_VisnRslt.CenX, m_VisnRslt.OriY - m_VisnRslt.CenY, m_VisnRslt.T, true,
                                                                               out double newCx, out double newCy);
                                m_VisnRslt.X = (double)newCx * TVisnUnit.Resoultion / 1000 * 1;
                                m_VisnRslt.Y = (double)newCy * TVisnUnit.Resoultion / 1000 * 1;
                            }
                        }

                        //NG
                        if (!m_VisnRslt.Match && m_VisnRslt.InspRslt != (int)EN_ERR_LIST.ERR_NONE)
                        {
                            if(m_RetryCnt++ < cDEF.FM.EngrOptn.nRetryCnt)
                            {
                                LOG.Trace($"Vision Fail Retry = {m_RetryCnt} / {cDEF.FM.EngrOptn.nRetryCnt}");

                                m_tWait.Clear() ;
                                m_iStepAlgn = 14;
                                return false;
                            }
                            else
                            {
                                SetVacOff();
                                SetLight(false);

                                EPU.SetErr((EN_ERR_LIST)m_VisnRslt.InspRslt, true); 

                                m_AutoCmdData.ErrorNumber = m_VisnRslt.InspRslt;
                                COMZEUS.SetResult(m_AutoCmdData);
                                
                                //
                                //m_VisnRslt.Item.Result = false;
                                m_VisnRslt.InspRslt    = m_AutoCmdData.ErrorNumber;
                                LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);


                                //검사 결과 영상 
                                cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);
                                DrngFlag    = false;
                                m_sWorkMsg  = string.Empty;
                                m_iStepAlgn = 0;
                                return true;
                            }
                        }
                        else 
                        {
                            //Draw 
                            System.Drawing.Brush brushes;
                            if (r5)
                            {
                                //Wafer & Ring Frame 내경 Draw
                                if (m_VisnRslt.Type == "SAWING")
                                {
                                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                                    {
                                        brushes = System.Drawing.Brushes.BlueViolet;
                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawRectangle(m_VisnRslt.Item.Overlay, brushes,
                                                                                             new Rectangle((int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutStrtX,
                                                                                                           (int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutStrtY,
                                                                                                           (int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutEndX,
                                                                                                           (int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutEndY));
                                    }
                                }
                                else
                                {
                                    cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawCircle(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(System.Drawing.Brushes.BlueViolet, 5),
                                                                                    new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY),
                                                                                    (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriR);
                                }
                            }
                            if (r6)
                            {
                                cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawCircle(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(System.Drawing.Brushes.Blue, 5),
                                                                                new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY),
                                                                                (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriR);
                                //
                                if (cDEF.FM.EngrOptn.bUseDcutAlgnT)
                                {
                                    PointF ptStrt = new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutStrtX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutStrtY);
                                    PointF ptEnd = new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutEndX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutEndY);
                                    int iAngleRegX = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).AngleRegionX;
                                    int iAngleRegY = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).AngleRegionY;
                                    cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawLine(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(System.Drawing.Color.Red, 10),
                                                                                  PointF.Add(ptStrt, new Size(iAngleRegX, iAngleRegY)),
                                                                                  PointF.Add(ptEnd, new Size(iAngleRegX, iAngleRegY)));
                                }

                            }

                            //추가 검사 결과 
                            if (FM.ProjBase.bUseCenterGap)
                            {
                                if (r5 && r6)
                                {
                                    //
                                    double P1 = Math.Pow(cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX, 2); 
                                    double P2 = Math.Pow(cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY, 2);
                                    
                                    double L1 = Math.Sqrt(P1 + P2) * TVisnUnit.Resoultion / 1000 * 1;
                                    double L2 = FM.ProjBase.dLimitCenterGap;

                                    //결과
                                    m_VisnRslt.Score = L1;
                                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                                    {
                                        brushes = L2 < L1 ? System.Drawing.Brushes.Red : System.Drawing.Brushes.RosyBrown;

                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawLine(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(brushes, 20),
                                                                                                               new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY),
                                                                                                               new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY));
                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, brushes, new PointF(50, 750), $"GAP : {Math.Round(m_VisnRslt.Score, 4)} mm ");
                                    }

                                    //Gap Check Log
                                    double dX = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX;
                                    double dY = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY;
                                    cDEF.LOG.Trace($"X:{dX}mm, Y:{dY}mm, GAP:{L1}mm");

                                    if (L2 < L1)
                                    {
                                        SetVacOff();

                                        EPU.SetErr(EN_ERR_LIST.ERR_0063, true); //Gap Check Error.

                                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0063;
                                        COMZEUS.SetResult(m_AutoCmdData);

                                        //
                                        m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                                        LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);
                                        //

                                        //검사 결과 영상 
                                        if (!ImgTest)
                                        {
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);
                                        }

                                        DrngFlag = false;
                                        m_sWorkMsg = string.Empty;
                                        m_iStepAlgn = 0;
                                        return true;
                                    }
                                }
                                else
                                {
                                    //GapCheck Error
                                    SetVacOff();

                                    //WaferAlign Fail
                                    if(!r5)
                                    {
                                        //Error
                                        EPU.SetErr(EN_ERR_LIST.ERR_0077, true); //Wafer 윤곽 미검출 Error!!
                                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0077;
                                        COMZEUS.SetResult(m_AutoCmdData);
                                        cDEF.LOG.Trace($"GapCheckFail!! - Wafer 윤곽 미검출 Error!!");
                                    }
                                    //RingFramAlign Fail
                                    else if(!r6)
                                    {
                                        EPU.SetErr(EN_ERR_LIST.ERR_0078, true); //[RingFrame] Ring Frame Detect Fail
                                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0078;
                                        COMZEUS.SetResult(m_AutoCmdData);
                                        cDEF.LOG.Trace($"GapCheckFail!! - [RingFrame] D-Cut Line Detect Fail");
                                    }
                                    //EPU.SetErr(EN_ERR_LIST.ERR_0063, true); //Gap Check Error.
                                    //m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0063;
                                    //COMZEUS.SetResult(m_AutoCmdData);

                                    //
                                    if (!m_OtherVisnRslt.Match)
                                    {
                                        using (System.Drawing.Font font = new System.Drawing.Font("Tahoma", 50, FontStyle.Bold))
                                        {
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50,  800), $"FAIL");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50,  900), $"Type : {m_OtherVisnRslt.Type}");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 1000), $"X : {Math.Round(m_OtherVisnRslt.X, 4)} mm");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 1100), $"Y : {Math.Round(m_OtherVisnRslt.Y, 4)} mm");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 1200), $"T : {Math.Round(m_OtherVisnRslt.T, 4)} ° ");
                                        }
                                    }

                                    //
                                    m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                                    LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                                    //검사 결과 영상 
                                    if (!ImgTest)
                                    {
                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);
                                    }

                                    //
                                    DrngFlag = false;
                                    m_sWorkMsg = string.Empty;
                                    m_iStepAlgn = 0;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        m_VisnRslt.X = 0.0;
                        m_VisnRslt.Y = 0.0;
                        m_VisnRslt.T = 0.0;
                    }
                    //
                    //검사 결과 영상 
                    cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 17:
                    //CHECK_TOLERANCE
                    if (CheckTolerance(m_bAlignChecked))
                    {
                        MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrX) + m_VisnRslt.Y; //기구 X = Vision Y
                        MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrY) + m_VisnRslt.X; 
                        MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrT) - m_VisnRslt.T;
                    }
                    else
                    {
                        if(m_bAlignChecked) //Retry
                        {
                            MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrX) + m_VisnRslt.Y; //기구 X = Vision Y
                            MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrY) + m_VisnRslt.X;
                            MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrT) - m_VisnRslt.T;

                            if(!CheckCalPosData() || ++m_nVerifyCnt > 3)
                            {
                                SetVacOff();
                                
                                ClearCalPos();

                                EPU.SetErr(EN_ERR_LIST.ERR_0062, true); //Wafer XYPosition Is Over

                                m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0062;
                                COMZEUS.SetResult(m_AutoCmdData);

                                //
                                m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                                LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);
                                
                                //검사 결과 영상 
                                if (!ImgTest) cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);

                                DrngFlag    = false;
                                m_sWorkMsg  = string.Empty;
                                m_iStepAlgn = 0;
                                return true;

                            }
                            
                            LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                            m_tWait.Clear();
                            m_iStepAlgn = 30;
                            return false;
                        }
                        else
                        {
                            SetVacOff();

                            EPU.SetErr(EN_ERR_LIST.ERR_0062, true); //Wafer XYPosition Is Over

                            m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0062;
                            COMZEUS.SetResult(m_AutoCmdData);

                            //
                            m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                            LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                            //검사 결과 영상 
                            if (!ImgTest) cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);

                            DrngFlag    = false;
                            m_sWorkMsg  = string.Empty;
                            m_iStepAlgn = 0;
                            return true;
                        }
                    }
                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 18:
                    if (ImgTest)
                    {
                        DrngFlag = false;
                        m_iStepAlgn = 0;
                        return true;
                    }
                    //Check Verify
                    if (m_bAlignChecked)
                    {
                        //
                        LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                        m_iStepAlgn = 21;
                        return false;
                    }

                    m_iStepAlgn++;
                    return false;

                    
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //Move
                case 19:
                    m_sWorkMsg = "Move Align Position...";

                    r1 = MoveMotr(EN_MOTR_ID.WAT_X, EN_COMD_ID.CalPos);
                    r2 = MoveMotr(EN_MOTR_ID.WAT_Y, EN_COMD_ID.CalPos);
                    r3 = MoveMotr(EN_MOTR_ID.WAT_T, EN_COMD_ID.CalPos);
                    if (!r1 || !r2 || !r3) return false;

                    WriteSeqLog("Move Align Position");

                    //Debug.WriteLine($"X:{MOTR.GetEncPos(EN_MOTR_ID.WAT_X)} / Y:{MOTR.GetEncPos(EN_MOTR_ID.WAT_Y)} / T:{MOTR.GetEncPos(EN_MOTR_ID.WAT_T)}");

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //check Option
                case 20:
                    
                    //
                    LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                    //Check Repeat Align
                    FM.EngrOptn.bUseAlignCheck = false; //JUNG/230331/기능 삭제
                    //if (FM.EngrOptn.bUseAlignCheck && (m_AlignCnt++ < cDEF.FM.EngrOptn.nAlignCnt))
                    //{
                    //    //
                    //    LOG.Trace($"Repeat Align = {m_AlignCnt} / {cDEF.FM.EngrOptn.nAlignCnt}");
                    //
                    //    enVisnMode = EN_VISN_STEP.ALIGN_RETRY;
                    //    
                    //    m_iStepAlgn = 13;
                    //    return false;
                    //}

                    //Check Verify
                    if (FM.EngrOptn.bUseAlignVerify)
                    {
                        LOG.Trace("Verify Align");

                        m_bAlignChecked = true; 
                        enVisnMode = EN_VISN_STEP.VERIFY;

                        WriteSeqLog("Align Verification");

                        m_tWait.Clear();
                        m_iStepAlgn = 13;
                        return false;
                    }

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 21:

                    //Check BCR
                    if (FM.EngrOptn.bUseBCR)
                    {
                        m_iStepAlgn = 50;
                        return false;
                    }
                    else
                    {
                        m_VisnRslt.BarCode = "NOT_USE";
                        m_sTemp = string.Empty;
                    }

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 22:
                    
                    m_sWorkMsg = "Finish Align...";

                    //
                    if (FM.EngrOptn.bUseBCR) LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], EN_VISN_STEP.ALIGN);

                    //
                    if(!ManCycle) SetVacOff();
                    //SetVacOff();
                    SetLight(false);

                    //Send command
                    if (FM.EngrOptn.bUseBCR) m_sTemp = string.Format($"/{m_VisnRslt.BarCode}");
                    m_AutoCmdData.Result = $"@AGN [0,0,0/{Math.Round(m_VisnRslt.X, 4)}," +
                                                       $"{Math.Round(m_VisnRslt.Y, 4)}," +
                                                       $"{Math.Round(m_VisnRslt.T, 4)}{m_sTemp}]";
                    COMZEUS.SetResult(m_AutoCmdData);
                    
                    m_dScanTime[1] = cDEF.TICK._GetTickTime() - m_dStrtTime[1];

                    WriteSeqLog($"Align End(Day Count : {m_nAlignCount})");
                    LOG.Trace($"Align End(Day Count : {m_nAlignCount}) / Cycle Time : {Math.Round((m_dScanTime[1] / 1000), 3)}sec");

                    //count 
                    m_nAlignCount++;
                    SPC.DAILY_DATA.iWorkQty++;


                    //
                    DrngFlag    = false;
                    m_sWorkMsg  = string.Empty;
                    m_iStepAlgn = 0;
                    return true;

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                case 30:
                    //Delay Test
                    if (!m_tWait.OnDelay(true, 500)) return false;


                    m_sWorkMsg = "Move Align Position...";

                    r1 = MoveMotr(EN_MOTR_ID.WAT_X, EN_COMD_ID.CalPos);
                    r2 = MoveMotr(EN_MOTR_ID.WAT_Y, EN_COMD_ID.CalPos);
                    r3 = MoveMotr(EN_MOTR_ID.WAT_T, EN_COMD_ID.CalPos);
                    if (!r1 || !r2 || !r3) return false;

                    enVisnMode = EN_VISN_STEP.ALIGN_RETRY;

                    WriteSeqLog($"Move Align Position - Verification ({m_nVerifyCnt})");

                    m_tWait.Clear();
                    m_iStepAlgn = 13;
                    return false;
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //Barcode Reading...
                case 50:
                    m_sWorkMsg = "Barcode Reading...";
                    WriteSeqLog("Barcode Reading Start");

                    //
                    BCR.CmdSetRead();

                    m_tWait.Clear();
                    m_iStepAlgn++;
                    return false;

                case 51:
                    if (!m_tWait.OnDelay(true, 3000)) return false;

                    if(BCR?._sReadBcr == string.Empty || BCR?._sReadBcr == "")
                    {
                        //
                        LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                        SetVacOff();

                        EPU.SetErr(EN_ERR_LIST.ERR_0020, true); //Barcode Not Found

                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0020;
                        COMZEUS.SetResult(m_AutoCmdData);

                        DrngFlag    = false;
                        m_iStepAlgn = 0;
                        m_sWorkMsg  = string.Empty;
                        return true;
                    }

                    //
                    m_VisnRslt.BarCode = BCR?._sReadBcr;

                    LOG.Trace($"Barcode : {BCR?._sReadBcr}");
                    
                    WriteSeqLog("Barcode Reading End");

                    m_tWait.Clear();
                    m_iStepAlgn = 22;
                    return false;

            }

            //return false; 
        }
        //------------------------------------------------------------------------
        public bool ManAlgnCycle()
        {//Vision 검사만...

            //bool r1, r2, r3; //, r4, r5, r6;

            //Align Cycle.
            if (m_iStepMan < 0) m_iStepMan = 0;
            switch (m_iStepMan)
            {
                default:
                    m_iStepMan = 0;
                    return true;

                case 10: //
                    //
                    m_bReqAlign     = false;
                    m_bAlignChecked = false;
                    m_RetryCnt      = 0;
                    m_nManTestCnt   = 0;

                    //Check Light
                    if (!cDEF.VISN.Light[(int)EN_CAM.WTB].IsOpen)
                    {
                        FRM.ShowWarn(true,"Light를 확인 하세요.");
                        m_iStepMan = 0;
                        return true; 
                    }

                    //
                    if (!IsWaferExist(true))
                    {
                        FRM.ShowWarn(true, "Wafer가 없습니다.");
                        m_iStepMan = 0;
                        return true;
                    }

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 11:
                    if (!m_tWait.OnDelay(true, 100)) return false;

                    if (!SetVacOn()) return false;

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 12: //                        
                    if (!m_tWait.OnDelay(true, 100)) return false;

                    //Check Vacuum ON
                    if (!IsVacOn())
                    {
                        FRM.ShowWarn(true, "Vacuum On Fail!!!");
                        m_iStepMan = 0;
                        return true;
                    }

                    //r1 = MoveMotr(EN_MOTR_ID.WAT_X, EN_COMD_ID.Wait1);
                    //r2 = MoveMotr(EN_MOTR_ID.WAT_Y, EN_COMD_ID.Wait1);
                    //r3 = MoveMotr(EN_MOTR_ID.WAT_T, EN_COMD_ID.Wait1);
                    //if (!r1 || !r2 || !r3) return false;


                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                case 13:
                    if (!m_tWait.OnDelay(true, 1000)) return false;

                    //Data Clear
                    m_VisnRslt.ResetData();
                    ClearCalPos();

                    //start
                    VisnTime[0] = DateTime.Now;

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 14:

                    //LightOn
                    SetLight(true);

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 15:
                    if (!m_tWait.OnDelay(true, cDEF.VISN.GetLightDelay((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn))) return false;
                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 16:
                    if (!cDEF.VISN.VisnGrabStrt((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn)) return false;

                    SetLight(false);

                    if (cDEF.FM.SysOptn.iTestMode[(int)EN_CAM.WTB] == (int)vDEF.CHCK_AWYS)
                    {
                        //검사 타입 분류
                        if (cDEF.FM.ProjBase.iWaferType == 0 &&
                            cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Type != "SAWING") //Wafer
                        {
                            m_VisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn);
                        }
                        else//Ring frame
                        {
                            m_VisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn);
                        }

                        //NG
                        if (!m_VisnRslt.Match && m_VisnRslt.InspRslt != (int)EN_ERR_LIST.ERR_NONE)
                        {
                            SetVacOff();

                            if(m_RetryCnt++ < cDEF.FM.EngrOptn.nRetryCnt)
                            {
                                LOG.Trace($"[Man] Visoin Retry = {m_RetryCnt} / {cDEF.FM.EngrOptn.nRetryCnt}");

                                m_tWait.Clear() ;
                                m_iStepMan = 14;
                                return false;
                            }
                            else
                            {
                                FRM.ShowWarn(true, "Align Fail!!!");
                                m_iStepMan = 0 ;
                                return true;
                            }
                        }
                        else
                        {
                            //추가 검사 결과 
                            if (FM.ProjBase.bUseCenterGap)
                            {
                                if (cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Match && cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).Match)
                                {
                                    //
                                    double P1 = Math.Pow(cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX, 2);
                                    double P2 = Math.Pow(cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY, 2);

                                    double L1 = Math.Sqrt(P1 + P2) * TVisnUnit.Resoultion / 1000 * 1;
                                    double L2 = FM.ProjBase.dLimitCenterGap;

                                    //결과
                                    m_VisnRslt.Score = L1;
                                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                                    {
                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawLine(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(System.Drawing.Brushes.RoyalBlue, 5),
                                                                                                               new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX),
                                                                                                               new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY));
                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.LimeGreen, new PointF(50, 650), $"T : {Math.Round(m_VisnRslt.Score, 4)} mm ");
                                    }

                                    if (L2 < L1)
                                    {
                                        SetVacOff();

                                        EPU.SetErr(EN_ERR_LIST.ERR_0063, true); //Gap Check Error.

                                        m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                                        LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);
                                        //

                                        m_sWorkMsg = string.Empty;
                                        m_iStepAlgn = 0;
                                        return true;
                                    }
                                }
                                else
                                {
                                    //Gap Check Error
                                    SetVacOff();

                                    //WaferAlign Fail
                                    if(!cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Match)
                                    {
                                        //Error
                                        EPU.SetErr(EN_ERR_LIST.ERR_0077, true); //Wafer 윤곽 미검출 Error!!
                                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0077;
                                        //COMZEUS.SetResult(m_AutoCmdData);
                                        cDEF.LOG.Trace($"GapCheckFail!! - Wafer 윤곽 미검출 Error!!");
                                    }
                                    //RingFramAlign Fail
                                    else if(!cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).Match)
                                    {
                                        EPU.SetErr(EN_ERR_LIST.ERR_0078, true); //[RingFrame] Ring Frame Detect Fail
                                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0078;
                                        //COMZEUS.SetResult(m_AutoCmdData);
                                        cDEF.LOG.Trace($"GapCheckFail!! - [RingFrame] D-Cut Line Detect Fail");
                                    }
                                    //EPU.SetErr(EN_ERR_LIST.ERR_0063, true); //Gap Check Error.
                                    m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                                    LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                                    //
                                    m_sWorkMsg = string.Empty;
                                    m_iStepAlgn = 0;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        m_VisnRslt.X = 0.0;
                        m_VisnRslt.Y = 0.0;
                        m_VisnRslt.T = 0.0;
                    }

                    //end
                    VisnTime[1] = DateTime.Now;

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 17:
                    //CHECK_TOLERANCE
                    if (CheckTolerance())
                    {
                        //MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrX) - m_VisnRslt.Y;
                        //MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrY) - m_VisnRslt.X;
                        //MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrT) - m_VisnRslt.T;
                    }
                    else
                    {
                        //
                        LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], EN_VISN_STEP.MANUAL);

                        FRM.ShowWarn(true, "Tolerance Fail!!!");
                        m_iStepMan = 0;
                        return true;
                    }

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 18:
                    
                    //
                    LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], EN_VISN_STEP.MANUAL);

                    m_iStepMan++;
                    return false;
                    
                case 19:

                    SetVacOff();

                    //Align Count Check
                    if (++m_nManTestCnt < FM.EngrOptn.nTestRunCnt)
                    {
                        LOG.Trace($"[Man] Test Count = {m_nManTestCnt} / {FM.EngrOptn.nTestRunCnt}");
                        m_iStepMan = 13;
                        return false;
                    }

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 20:
                    FRM.ShowWarn(true, $"Manual Test OK- {m_nManTestCnt}회");
                    m_iStepMan = 0;
                    return true;
            }

            //return false; 
        }
        //------------------------------------------------------------------------
        public bool FileAlgnCycle()
        {//
            bool DrngFlag;
            bool r1, r2, r3; 
            bool r4, r5, r6;

            //Align Cycle.
            if (m_iStepMan < 0) m_iStepMan = 0;
            switch (m_iStepMan)
            {
                default:
                    DrngFlag    = false;
                    m_iStepMan = 0;
                    return true;

                case 10: //

                    DrngFlag = true;

                    //
                    m_bAlignChecked = false;
                    enVisnMode      = EN_VISN_STEP.MANUAL;

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 11:

                    m_tVacErr.Clear();
                    m_tWait  .Clear();
                    m_iStepMan++;
                    return false;

                case 12: //                        
                    

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                case 13:

                    //Data Clear
                    m_VisnRslt.ResetData();
                    m_OtherVisnRslt.ResetData();
                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 14:


                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 15:
                    
                    //Start
                    VisnTime[0] = DateTime.Now;

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 16:
                    if (!cDEF.VISN.VisnGrabStrt((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn, cDEF.MAN.FuncArg.sArg1)) return false;
                    //
                    r5 = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Match;
                    r6 = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).Match;

                    if (cDEF.FM.SysOptn.iTestMode[(int)EN_CAM.WTB] == (int)vDEF.CHCK_AWYS)
                    {
                        //검사 타입 분류
                        //if (cDEF.FM.ProjBase.iWaferType == 0  &&
                        //    cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Type != "SAWING") //Wafer
                        if (cDEF.FM.ProjBase.iWaferType == 0)
                        {
                            m_VisnRslt.SetData      = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn);
                            m_OtherVisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn);
                        }
                        else//Ring frame
                        {
                            m_VisnRslt.SetData      = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn);
                            m_OtherVisnRslt.SetData = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn);
                        }

                        //D컷 조명 추가에서는 Angle은 FAlgn.T값으로 적용.
                        if (cDEF.FM.EngrOptn.bUseDcutAlgnT && cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).Match)
                        {
                            m_VisnRslt.T = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).T;
                            //
                            if (cDEF.FM.ProjBase.iWaferType == 0)
                            {
                                cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.Calculate(m_VisnRslt.OriX - m_VisnRslt.CenX, m_VisnRslt.OriY - m_VisnRslt.CenY, m_VisnRslt.T, true,
                                                                               out double newCx, out double newCy);
                                m_VisnRslt.X = (double)newCx * TVisnUnit.Resoultion / 1000 * 1;
                                m_VisnRslt.Y = (double)newCy * TVisnUnit.Resoultion / 1000 * 1;
                            }
                        }

                        //NG
                        if (!m_VisnRslt.Match && m_VisnRslt.InspRslt != (int)EN_ERR_LIST.ERR_NONE)
                        {
                            //SetVacOff();
                            SetLight(false);

                            EPU.SetErr((EN_ERR_LIST)m_VisnRslt.InspRslt, true);

                            m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                            LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                            //검사 결과 영상 
                            //cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);
                            DrngFlag = false;
                            m_sWorkMsg = string.Empty;
                            m_iStepAlgn = 0;
                            return true;
                        }
                        else
                        {
                            //r4 = FM.ProjBase.bUseCenterGap;
                            //r5 = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).Match;
                            //r6 = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).Match;
                            //Draw 
                            System.Drawing.Brush brushes;
                            if (r5)
                            {
                                //Wafer & Ring Frame 내경 Draw
                                if (m_VisnRslt.Type == "SAWING")
                                {
                                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                                    {
                                        brushes = System.Drawing.Brushes.BlueViolet;
                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawRectangle(m_VisnRslt.Item.Overlay, brushes,
                                                                                             new Rectangle((int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutStrtX,
                                                                                                           (int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutStrtY,
                                                                                                           (int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutEndX,
                                                                                                           (int)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).DcutEndY));
                                    }
                                }
                                else
                                {
                                    cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawCircle(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(System.Drawing.Brushes.BlueViolet, 5),
                                                                                    new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY),
                                                                                    (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriR);
                                }
                            }
                            if (r6)
                            {
                                cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawCircle(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(System.Drawing.Brushes.Blue, 5),
                                                                                new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY),
                                                                                (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriR);
                                //
                                if (cDEF.FM.EngrOptn.bUseDcutAlgnT)
                                {
                                    PointF ptStrt = new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutStrtX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutStrtY);
                                    PointF ptEnd = new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutEndX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).DcutEndY);
                                    int iAngleRegX = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).AngleRegionX;
                                    int iAngleRegY = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).AngleRegionY;
                                    cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawLine(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(System.Drawing.Color.Red, 10),
                                                                                  PointF.Add(ptStrt, new Size(iAngleRegX, iAngleRegY)),
                                                                                  PointF.Add(ptEnd, new Size(iAngleRegX, iAngleRegY)));
                                }

                            }

                            //추가 검사 결과 
                            if (FM.ProjBase.bUseCenterGap)
                            {
                                if (r5 && r6)
                                {
                                    //
                                    double P1 = Math.Pow(cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX, 2);
                                    double P2 = Math.Pow(cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY, 2);

                                    double L1 = Math.Sqrt(P1 + P2) * TVisnUnit.Resoultion / 1000 * 1;
                                    double L2 = FM.ProjBase.dLimitCenterGap;

                                    //결과
                                    m_VisnRslt.Score = L1;
                                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                                    {
                                        brushes = L2 < L1 ? System.Drawing.Brushes.Red : System.Drawing.Brushes.RosyBrown;

                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawLine(m_VisnRslt.Item.Overlay, new System.Drawing.Pen(brushes, 20),
                                                                                                               new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY),
                                                                                                               new PointF((float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX, (float)cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY));
                                        cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, brushes, new PointF(50, 750), $"GAP : {Math.Round(m_VisnRslt.Score, 4)} mm ");

                                    }

                                    //Gap Check Log
                                    double dX = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriX - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriX;
                                    double dY = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.WAlgn).OriY - cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt(EN_VISN_TYPE.FAlgn).OriY;
                                    cDEF.LOG.Trace($"X:{dX}mm, Y:{dY}mm, GAP:{L1}mm");

                                    if (L2 < L1)
                                    {
                                        SetVacOff();

                                        EPU.SetErr(EN_ERR_LIST.ERR_0063, true); //Gap Check Error.

                                        //
                                        m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                                        LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);
                                        //

                                        //검사 결과 영상 
                                        //cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);

                                        DrngFlag = false;
                                        m_sWorkMsg = string.Empty;
                                        m_iStepAlgn = 0;
                                        return true;
                                    }
                                }
                                else
                                {
                                    //GapCheck Error
                                    SetVacOff();

                                    //WaferAlign Fail
                                    if(!r5)
                                    {
                                        //Error
                                        EPU.SetErr(EN_ERR_LIST.ERR_0077, true); //Wafer 윤곽 미검출 Error!!
                                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0077;
                                        //COMZEUS.SetResult(m_AutoCmdData);
                                        cDEF.LOG.Trace($"GapCheckFail!! - Wafer 윤곽 미검출 Error!!");
                                    }
                                    //RingFramAlign Fail
                                    else if(!r6)
                                    {
                                        EPU.SetErr(EN_ERR_LIST.ERR_0078, true); //[RingFrame] Ring Frame Detect Fail
                                        m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0078;
                                        //COMZEUS.SetResult(m_AutoCmdData);
                                        cDEF.LOG.Trace($"GapCheckFail!! - [RingFrame] D-Cut Line Detect Fail");
                                    }
                                    //
                                    if (!m_OtherVisnRslt.Match)
                                    {
                                        using (System.Drawing.Font font = new System.Drawing.Font("Tahoma", 50, FontStyle.Bold))
                                        {
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 800), $"FAIL");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 900), $"Type : {m_OtherVisnRslt.Type}");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 1000), $"X : {Math.Round(m_OtherVisnRslt.X, 4)} mm");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 1100), $"Y : {Math.Round(m_OtherVisnRslt.Y, 4)} mm");
                                            cDEF.VISN.Cam[(int)EN_CAM.WTB].mAlgo.DrawString(m_VisnRslt.Item.Overlay, font, System.Drawing.Brushes.Blue, new PointF(50, 1200), $"T : {Math.Round(m_OtherVisnRslt.T, 4)} ° ");
                                        }
                                    }
                                    //
                                    m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                                    LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                                    //검사 결과 영상 
                                    cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);

                                    //
                                    DrngFlag = false;
                                    m_sWorkMsg = string.Empty;
                                    m_iStepAlgn = 0;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        m_VisnRslt.X = 0.0;
                        m_VisnRslt.Y = 0.0;
                        m_VisnRslt.T = 0.0;
                    }
                    //end
                    VisnTime[1] = DateTime.Now;

                    //검사 결과 영상 
                    cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked);
                    LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);

                    m_tWait.Clear();
                    m_iStepMan++;
                    return false;

                case 17:
                    //CHECK_TOLERANCE
                    if (CheckTolerance(m_bAlignChecked)) //if (CheckTolerance(m_bAlignChecked))
                    {
                        //MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrX) + m_VisnRslt.Y; //기구 X = Vision Y
                        //MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrY) + m_VisnRslt.X; 
                        //MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.CalPos] = MOTR.GetEncPos(m_iMotrT) - m_VisnRslt.T;
                    }
                    else
                    {
                        EPU.SetErr(EN_ERR_LIST.ERR_0062, true); //Wafer XYPosition Is Over

                        //m_AutoCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0062;
                        //COMZEUS.SetResult(m_AutoCmdData);

                        //
                        m_VisnRslt.InspRslt = m_AutoCmdData.ErrorNumber;
                        LOG.VisionResult(m_VisnRslt, VisnTime[0], VisnTime[1], enVisnMode);
                        
                        cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(m_VisnRslt.Item, m_bAlignChecked, true);

                        DrngFlag    = false;
                        m_iStepMan  = 0;
                        return true;
                    }
                    DrngFlag   = false;
                    m_iStepMan = 0;
                    return true;
            }
        }
        //------------------------------------------------------------------------
        private bool CheckCalPosData()
        {
            bool bOk = true;
            //cDEF.MOTR[m_iSelMotr].m_dMinPosn
            //cDEF.MOTR[m_iSelMotr].m_dMaxPosn

            if (MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.CalPos] < cDEF.MOTR[(int)m_iMotrX].m_dMinPosn ||
                MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.CalPos] > cDEF.MOTR[(int)m_iMotrX].m_dMaxPosn) bOk = false;

            if (MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.CalPos] < cDEF.MOTR[(int)m_iMotrY].m_dMinPosn ||
                MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.CalPos] > cDEF.MOTR[(int)m_iMotrY].m_dMaxPosn) bOk = false;

            if (MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.CalPos] < cDEF.MOTR[(int)m_iMotrT].m_dMinPosn ||
                MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.CalPos] > cDEF.MOTR[(int)m_iMotrT].m_dMaxPosn) bOk = false;

            return bOk; 
        }
        //--------------------------------------------------------------------------
        private void ClearCalPos()
        {
            MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.CalPos] = 0.0;
            MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.CalPos] = 0.0;
            MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.CalPos] = 0.0;
        }
        //--------------------------------------------------------------------------
        private bool CheckTolerance(bool verify = false)
        {
            //bool    bUseOnlyXY  = cDEF.FM.EngrOptn.bUseOnlyXY;
            //bool    bUseOnlyXY  = cDEF.FM.EngrOptn.bUseRingFrame3;
            bool    bUseOnlyXY  = false;
            double  mToleranceX = verify? FM.EngrOptn.dToleranceX_Verify : FM.EngrOptn.dToleranceX ;//5.01;
            double  mToleranceY = verify? FM.EngrOptn.dToleranceY_Verify : FM.EngrOptn.dToleranceY ;//5.01;
            double  mToleranceT = verify? FM.EngrOptn.dToleranceT_Verify : FM.EngrOptn.dToleranceT ;//5.01;

            //
            if (bUseOnlyXY)
            {
                cDEF.LOG.Trace(" > [CheckTolerance][RingFrame3] Align Only X,Y Axis");
                m_VisnRslt.T = 0;
            }

            bool xOK = Math.Abs(m_VisnRslt.X) <= mToleranceX ;
            bool yOK = Math.Abs(m_VisnRslt.Y) <= mToleranceY ;
            bool tOK = Math.Abs(m_VisnRslt.T) <= mToleranceT ;
            //                             
            string msg = string.Format($"[Tolerance] X = {Math.Round(m_VisnRslt.X,5)}/{mToleranceX} " +
                                                 $", Y = {Math.Round(m_VisnRslt.Y,5)}/{mToleranceY} " +
                                                 $", T = {Math.Round(m_VisnRslt.T,5)}/{mToleranceT} => Result : {xOK && yOK && tOK}");
            LOG.Trace(msg);


            //
            return (yOK && xOK && tOK);
        }
        //--------------------------------------------------------------------------
        public bool CheckTolerance(double x, double y, double t)
        {
          //bool   bUseOnlyXY  = cDEF.FM.EngrOptn.bUseOnlyXY    ;
            bool  bUseOnlyXY   = cDEF.FM.EngrOptn.bUseRingFrame3; //JUNG/230331
            double mToleranceX = FM.EngrOptn.dToleranceX;//5.01;
            double mToleranceY = FM.EngrOptn.dToleranceY;//5.01;
            double mToleranceT = FM.EngrOptn.dToleranceT;//5.01;

            bool xOK = Math.Abs(x) <= mToleranceX;
            bool yOK = Math.Abs(y) <= mToleranceY;
            bool tOK = bUseOnlyXY ? true : Math.Abs(t) <= mToleranceT;

            return (yOK && xOK && tOK);
        }

        //--------------------------------------------------------------------------

        //Running actions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public override bool ToStopCon()
        {
            //Clear Timer.
            m_bToStop = false;
            m_tToStop.Clear();

            //During the auto run, do not stop.
            if ( m_iStepSeq   != 0       ) return false;
            if ( m_iStepAlgn  != 0       ) return false;

            //Ok.
            return true;
        }
        //------------------------------------------------------------------------
        public override bool ToStartCon()
        {
            //Clear Timer.
            m_bToStart = false;
            m_tToStart.Clear();

            //Ok.
            return true;
        }
        //------------------------------------------------------------------------
        public override bool ToStart()
        {
            //
            //bool r1, r2;

            //Check Time Out.
            if (m_bToStart                                                                         ) return true;
            if (m_tToStart.OnDelay(!m_bToStart && (cDEF.SEQ._iSeqStat != EN_SEQ_STAT.Init) , 10000)) {cDEF. EPU.SetErr(EN_ERR_LIST.ERR_0044); return false; }

            ////
            //r1 = true; 
            //r2 = true; //
            //if (!r1 || !r2) return false;

            //Init.
            m_iStepSeq   = 0;     //Clear Step.
            m_iStepAlgn  = 0;

            //Clear Timer.
			m_tTemp     .Clear();
			m_tWait     .Clear();
			m_tCyleMain .Clear();
			m_tCyleWait .Clear();
            m_tCyleAlgn .Clear();

            //Set To Flags.
            m_bToStart   = true ;
            m_bToStop    = false;
            //Ok.
            return true;
        }
        //------------------------------------------------------------------------
        public override bool ToStop()
        {

            //Check Time Out.
            if (m_bToStop                                )   return true;
	        if (m_tToStop.OnDelay(!m_bToStop , 20 * 1000)) { cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0045); return false; }

            //
            bool r1, r2, r3;

            //
            r1 = cDEF.MOTR.Stop(m_iMotrX);
            r2 = cDEF.MOTR.Stop(m_iMotrY);
            r3 = cDEF.MOTR.Stop(m_iMotrT);
            if (!r1 || !r2 || !r3) return false;

            //Init.
            //SetLight(false);

            //Clear Step.
            m_iStepSeq   = 0;
            m_iStepAlgn  = 0;

            //Set To Flags.
	        m_bToStop  = true;
            
            ClearCalPos(); //JUNG/240808

            //Ok.
            return true;
        }
        //------------------------------------------------------------------------
        public override bool  StatusRun   ()
        {
            return false;
        }

        //------------------------------------------------------------------------
        public override bool AutoRun()
        {
            bool   r1, r2 , r3;

			//Get Motor Last Enc
            m_dMPosX  = cDEF.MOTR.GetEncPos(m_iMotrX );
            m_dMPosY  = cDEF.MOTR.GetEncPos(m_iMotrY );
            m_dMPosT  = cDEF.MOTR.GetEncPos(m_iMotrT );


	        //Check Cycle Time Out.                                                                       
	        m_tCyleMain .OnDelay((m_iStepSeq    != 0) &&                 !cDEF.EPU._bHasErr && (cDEF.FM.SysOptn.iRunMode != vDEF.MAN_RUN) , 5*60*1000);
            m_tCyleAlgn .OnDelay((m_iStepAlgn   != 0) &&  m_bDrngAlgn && !cDEF.EPU._bHasErr && (cDEF.FM.SysOptn.iRunMode != vDEF.MAN_RUN) , 5*60*1000);

            /* Error : Cycle TimeOut           */
	        if(m_tCyleMain.Out || m_tCyleAlgn.Out) 
	        {
 	            string sTemp = "";
                string sPartName = cDEF.POSN.GetPartName((int)m_iPartId); 

                if (cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0046, m_tCyleMain .Out)) sTemp = string.Format("{0} MAIN CYCLE TIMEOUT STATUS : m_iStepSeq={1}"                   , sPartName, m_iStepSeq                 );
                if (cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0047, m_tCyleAlgn .Out)) sTemp = string.Format("{0} ALIGN CYCLE TIMEOUT STATUS : m_iStepSeq={1}, m_iStepAlgn ={2}", sPartName, m_iStepSeq , m_iStepAlgn   );
                //
                cDEF.LOG.Trace(sTemp);
                cDEF.SEQ.CrntStatTrace(m_iPartId, sTemp);
                //
			    Reset();
			    return false; 
	        }

	        //Check Error & Decide Step.
	        if (m_iStepSeq == 0) 
            {
				//
                bool isLocateWaitX =  MOTR.CmprPos(m_iMotrX, cDEF.MOTR[(int)m_iMotrX].MP.dPosn[(int)EN_POSN_ID.Wait1]);
                bool isLocateWaitY =  MOTR.CmprPos(m_iMotrY, cDEF.MOTR[(int)m_iMotrY].MP.dPosn[(int)EN_POSN_ID.Wait1]);
                bool isLocateWaitT =  MOTR.CmprPos(m_iMotrT, cDEF.MOTR[(int)m_iMotrT].MP.dPosn[(int)EN_POSN_ID.Wait1]);
                bool isReqAlign    =  m_bReqAlign;

                //
                //bool isConAlgn     =  isLocateWaitX && isLocateWaitY && isLocateWaitT && isReqAlign; 
                bool isConAlgn     =  isReqAlign ; 
                bool isConWait     =  m_bReqWait ;//(isLocateWaitX ||  isLocateWaitY || isLocateWaitT);
                                      
		        //Return Con.

		        //Clear Var.
                m_bDrngAlgn    = false;
                m_bDrngWait    = false;

		        //Sequence Stop Flag.
		        if ( SEQ._bLtStop                                         ) return false;
                if (!SEQ._bRun    && (FM.SysOptn.iRunMode != vDEF.MAN_RUN)) return false;
                if ( EPU._bHasErr                                         ) return false;

                //Error.


                //Decide Step.
                if (isConAlgn   ) { m_bDrngAlgn  = true; m_iStepSeq =  10; m_iStepAlgn  = 10; goto __GOTO_CYCLE_START__; }
                if (isConWait   ) { m_bDrngWait  = true; m_iStepSeq = 300;                    goto __GOTO_CYCLE_START__; }
            }

	        //Cycle Start Line.
	        __GOTO_CYCLE_START__:

 	        //Cycle.
	        switch (m_iStepSeq) 
            {
  		        default : m_iStepSeq  = 0;
				          break;
                                        
                case  10: if (!AutoAlgnCycle (ref m_bDrngAlgn )) return false; m_iStepSeq = 0; return false;  //             

                //Wait
                //---------------------------------------------------------------------
                case 300: 
                    m_bDrngWait = true;
                    m_iStepSeq ++; 
                    return false;

                case 301:
                    m_bReqWait  = false;
                    m_bReqAlign = false;

                    //
                    m_iStepSeq ++; 
                    return false;

                case 302: 
                    r1 = MoveMotr(m_iMotrX, EN_COMD_ID.Wait1);
                    r2 = MoveMotr(m_iMotrY, EN_COMD_ID.Wait1);
                    r3 = MoveMotr(m_iMotrT, EN_COMD_ID.Wait1);
                    if (!r1 || !r2 || !r3) return false;

                    //Send command 
                    COMZEUS.SetResult(m_AutoCmdData);
                    
                    m_bDrngWait = false;
                    m_iStepSeq = 0; 
                    return true;
            }

            //
            return false;
        }
        //------------------------------------------------------------------------
        private void WriteSeqLog(string msg)
        {
            string sPartName = cDEF.POSN.GetPartName((int)m_iPartId);

            cDEF.LOG.SeqTrace($"[{sPartName}] {msg}");
        }
        //------------------------------------------------------------------------
        public override bool UpdateListVal(int no, out string sName, out string sValue)
        {//UserSet - FrmAdmin-OnCycle 화면 및 TimeOut Log에 저장할 변수 처리 
			int iCnt = 0;  
            sName  = "";
            sValue = "";
            object obj = new object(); 
            if (no ==  iCnt++) {sName = "m_bToStart     "; obj = m_bToStart           ; }
            if (no ==  iCnt++) {sName = "m_bToStop      "; obj = m_bToStop            ; }
            if (no ==  iCnt++) {sName = "m_bWorkEnd     "; obj = m_bWorkEnd           ; }
            if (no ==  iCnt++) {sName = "m_bDrngAlgn    "; obj = m_bDrngAlgn          ; }
            if (no ==  iCnt++) {sName = "m_bDrngWait    "; obj = m_bDrngWait          ; }
            if (no ==  iCnt++) {sName = "m_iStepSeq     "; obj = m_iStepSeq           ; }
            if (no ==  iCnt++) {sName = "m_iStepMan     "; obj = m_iStepMan           ; }
            if (no ==  iCnt++) {sName = "m_iStepHome    "; obj = m_iStepHome          ; }
            if (no ==  iCnt++) {sName = "m_iStepAlgn    "; obj = m_iStepAlgn          ; }
               
            if (no ==  iCnt++) {sName = "m_bReqAlign    "; obj = m_bReqAlign          ; }
            if (no ==  iCnt++) {sName = "m_bReqWait     "; obj = m_bReqWait           ; }
            if (no ==  iCnt++) {sName = "m_nAlignCount  "; obj = m_nAlignCount        ; }
            if (no ==  iCnt++) {sName = "m_RetryCnt     "; obj = m_RetryCnt           ; }
            if (no ==  iCnt++) {sName = "m_AlignCnt     "; obj = m_AlignCnt           ; }
            if (no ==  iCnt++) {sName = "m_nManTestCnt  "; obj = m_nManTestCnt        ; }

            if (no ==  iCnt++) {sName = "m_bAlignChecked"; obj = m_bAlignChecked      ; }
            if (no ==  iCnt++) {sName = "Command        "; obj = m_AutoCmdData.Command; }
            if (no ==  iCnt++) {sName = "enVisnMode     "; obj = enVisnMode.ToString(); }

            if(sName == "") return false;
            
            sName.Trim();
            sValue = obj.ToString(); 
            return true;
        }

        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public override void  Load (BinaryReader br)
        {
            m_sSpare1      = br.ReadString().Trim();
            m_sSpare2      = br.ReadString().Trim();
                           
            m_bSpare1      = br.ReadBoolean();
            m_bSpare2      = br.ReadBoolean();

            m_nAlignCount  = br.ReadInt32();
            m_iSpare1      = br.ReadInt32();
            m_iSpare2      = br.ReadInt32();
                         
            m_dMPosX       = br.ReadDouble();
            m_dMPosY       = br.ReadDouble();
            m_dMPosT       = br.ReadDouble();
                           
            m_dSpare1      = br.ReadDouble();
            m_dSpare2      = br.ReadDouble();
        }
        //------------------------------------------------------------------------
        public override void  Save(BinaryWriter wr)
        {
            wr.Write(m_sSpare1 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare2 .PadRight(vDEF.MAX_STR_LEN, ' '));
                               
            wr.Write(m_bSpare1     );
            wr.Write(m_bSpare2     );

            wr.Write(m_nAlignCount );
            wr.Write(m_iSpare1     );
            wr.Write(m_iSpare2     );
                                   
            wr.Write(m_dMPosX      );
            wr.Write(m_dMPosY      );
            wr.Write(m_dMPosT      );
			                       
            wr.Write(m_dSpare1     );
            wr.Write(m_dSpare2     );
        }
        //--------------------------------------------------------------------------
        public void SetCmdData(cCmdData data)
        {
            m_AutoCmdData = data;
        }
        //--------------------------------------------------------------------------
        public void ClearAlignCount()
        {
            m_nAlignCount = 0;
        }

    }
}
