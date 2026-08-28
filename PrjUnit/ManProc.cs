using System;

namespace eMachine
{
    /***************************************************************************/
    /* Structures & Variables                                                  */
    /***************************************************************************/
    public struct _TFuncArg
    {
        public bool   bArg1;
        public bool   bArg2;
        public bool   bArg3;
        public bool   bArg4;
        public bool   bArg5;
        public bool   bArg6;
        public int    iArg1;
        public int    iArg2;
        public int    iArg3;
        public int    iArg4;
        public int    iArg5;
        public int    iArg6;
        public double dArg1;
        public double dArg2;
        public double dArg3;
        public double dArg4;
        public double dArg5;
        public double dArg6;

        public string sArg1;
        public string sArg2;
        public string sArg3;
        public string sArg4;
        public string sArg5;
        public string sArg6;
    };

    /***************************************************************************/
    /* Class: TManProc                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TManProc
    {
        /* Base Constants                                                          */
        /***************************************************************************/

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_ChngMotrOnTimer  = new TOnDelayTimer();
        TOnDelayTimer m_ChngMotrOffTimer = new TOnDelayTimer();
        TOnDelayTimer m_HomeTimer        = new TOnDelayTimer();
        TOnDelayTimer m_RNRTimer         = new TOnDelayTimer();
        TOnDelayTimer m_CycleTimer       = new TOnDelayTimer();
        TOnDelayTimer m_TestTimer        = new TOnDelayTimer();
        TOnDelayTimer m_VibOnTimer       = new TOnDelayTimer();
        TOnDelayTimer m_VibOffTimer      = new TOnDelayTimer();
        TOnDelayTimer m_TempTimer        = new TOnDelayTimer();

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        int        m_iManNo            ; //Selected Manual No.
        int        m_iPrevManNo        ;
        bool       m_bHoming           ; //For Homing.
        bool       m_bOneShot          ;
        bool       m_bLtProcOn         ;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        bool       m_bRptMotr               ; //For Repeat Functions.
        bool       m_bRptNozMotr            ;
        bool       m_bDirMotr               ;
        bool[]     m_bRptActr = new bool[(int)EN_ACTR_ID.EndOfId];
        bool       m_bRptActrIng            ;
        int        m_iRMotrID               ;
        int        m_iChngMotrDlay          ;
        int        m_iChngActrDlay          ;
        int        m_iStepMan               ;
        int        m_iWher                  ;
        double     m_dP1                    ;
        double     m_dV1                    ;
        double     m_dA1                    ;
        double     m_dD1                    ;
        double     m_dP2                    ;
        double     m_dV2                    ;
        double     m_dA2                    ;
        double     m_dD2                    ;

        bool       m_bReqWCK                ;
        int        m_iFuncStep              ;
        int        m_nVacCheckCount         ;
        int        m_nVacTestCount          ;

        //SCAN TIME
        public double[]   m_dScanTime = new double[10];
        public double[]   m_dStrtTime = new double[10];

        private cCmdData m_ManCmdData;


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int _iManNo
        {
            get { return m_iManNo; }
            set { m_iManNo = value; }
        }
        public bool _bOneShot
        {
            get { return m_bOneShot; }
            set { m_bOneShot = value; }
        }
        public bool _bHoming
        {
            get { return m_bHoming; }
        }
        public int _iWher
        {
            get { return m_iWher; }
            set { m_iWher = value; }
        }
        public bool _bRptMotr
        {
            get { return m_bRptMotr; }
            set { m_bRptMotr = value; }
        }
        public bool _bRun 
        { 
            get { return _iManNo != 0 ? true : false; } 
        }

        public bool _bReqWCK   { get { return m_bReqWCK  ; } set { m_bReqWCK   = value; } }
      //public int  _iFuncStep { get { return m_iFuncStep; } set { m_iFuncStep = value; } }
        public int  _iFuncStep => m_iFuncStep ;
        

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public _TFuncArg   FuncArg; //Manual Processing Function Arguments.
        public TSORT_INFO  SortInfo = new TSORT_INFO();
        public TVisnRslt   VisnRslt = new TVisnRslt ();


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TManProc()
        {
            m_ManCmdData = new cCmdData(eCommand.None, null);

            Init();
        }
        ~TManProc() { }

        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

		//Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {

            m_iManNo     = 0;
            m_iPrevManNo = 0;
            m_iStepMan   = 0;
            m_bOneShot   = false;
            m_bHoming    = false;

            m_ManCmdData.ClearCMD();

        }
        //--------------------------------------------------------------------------
        public void Reset()
        {
            Init();
        }
        
        //Clear Step.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ClearHomeStep(EN_SEQ_ID Whre)
        {
            //Clear Home Timer.
            m_HomeTimer.Clear();
            cDEF.SEQ.ClearAllHomeStep(Whre);
        }
        //Master Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetRptMotr(int Motr, bool Flag) { 
            m_iRMotrID = Motr; 
            m_bRptMotr = Flag; 
            m_bDirMotr = false; 
        }
        //------------------------------------------------------------------------
        public void SetRptNozMotr(bool Flag, int iDelay) { 
            m_iChngMotrDlay = iDelay; 
            m_bRptNozMotr   = Flag; 
            m_bDirMotr      = false; 
        }
        //------------------------------------------------------------------------
        public void SetRMPara(int Dlay, double P1, double V1, double A1, double D1, double P2, double V2, double A2, double D2)
        {
            m_iChngMotrDlay = Dlay;
            m_dP1           = P1  ;
            m_dV1           = V1  ;
            m_dA1           = A1  ;
            m_dD1           = D1  ;
            m_dP2           = P2  ;
            m_dV2           = V2  ;
            m_dA2           = A2  ;
            m_dD2           = D2  ;
        }
        //------------------------------------------------------------------------
        public void SetRAPara(int Dlay) 
        { 
            m_iChngActrDlay = Dlay; 
        }

		//User Defined Functions.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool RptMotr()
        {
            if (m_iRMotrID <0 || m_iRMotrID>=cDEF.MOTR._iNumOfMotr) m_bRptMotr = false;
            if ( cDEF.MOTR[m_iRMotrID].GetAlarm  ()) m_bRptMotr = false;
            if ( cDEF.MOTR[m_iRMotrID].GetCW     ()) m_bRptMotr = false;
            if ( cDEF.MOTR[m_iRMotrID].GetCCW    ()) m_bRptMotr = false;
            if (!cDEF.MOTR[m_iRMotrID].GetHomeEnd()) m_bRptMotr = false;
            if (!cDEF.MOTR[m_iRMotrID].GetServo  ()) m_bRptMotr = false;



            //Changing Timer.
            m_ChngMotrOnTimer .OnDelay(cDEF.MOTR.MotnDone((EN_MOTR_ID)m_iRMotrID) && !m_bDirMotr , m_iChngMotrDlay);
            m_ChngMotrOffTimer.OnDelay(cDEF.MOTR.MotnDone((EN_MOTR_ID)m_iRMotrID) && m_bDirMotr, m_iChngMotrDlay  );
            if (m_ChngMotrOnTimer.Out ) { cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iRMotrID, m_dP1, m_dV1, m_dA1, m_dD1); m_bDirMotr = true; }
            if (m_ChngMotrOffTimer.Out) { cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iRMotrID, m_dP2, m_dV2, m_dA2, m_dD2); m_bDirMotr = false; }

            //Ok.
            return true;
        }

		//Homing.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void MoveAllHome()
        {
            bool bErr = false;
            m_HomeTimer.OnDelay(m_bHoming , 100000);
            if (cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0001, m_HomeTimer.Out)) //if (cDEF.EPU.SetErr(vDEF.ERR_ALLHOME, m_HomeTimer.Out))
            {
                m_iManNo  = 0      ;
                m_bHoming = false  ;
                //Trace Log.
                cDEF.LOG.Trace   ("AllHome TimeOut");
                cDEF.LOG.SeqTrace("AllHome TimeOut");
            }

            cDEF.SEQ.GoDoorLock(true, true);
            //if (cDEF.SEQ._bRun          ) { FRM.ShowWarn(true, "Machine is Run, the motor can not move.."              ); m_iManNo = 0; m_bHoming = false; return; }
            //if (cDEF.SEQ.IsOpenAnyDoor()) { FRM.ShowWarn(true, "Door open sensor is detected, the motor can not move.."); m_iManNo = 0; m_bHoming = false; return; }

            //Inspection.
            if (!cDEF.SEQ.InspectSafety   ()) bErr = true;
            if (!cDEF.SEQ.InspectEmergency()) bErr = true;
            if ( cDEF.EPU._bHasErr          ) bErr = true;
            //if (!cDEF.SEQ.InspectActuator ()) bErr = true;
            //if (!cDEF.SEQ.InspectMainAir  ()) bErr = true;
            
            if(bErr) 
            {
                m_iManNo = 0; 
                m_bHoming = false; 
                return;
            }

            m_bHoming = !(cDEF.SEQ.MoveHome(EN_SEQ_ID.ALL));
            if (!m_bHoming) 
            { 
                m_iManNo = 0;
                //cDEF.SEQ.GoDoorLock(true); 
                if(m_ManCmdData.Command != eCommand.None) cDEF.COMZEUS.SetResult(m_ManCmdData);
                //if (cDEF.SEQ._bAutoMode) cDEF.SEQ._bBtnManStart = true;  //Home은 메뉴얼일때만...
            } 
            
            //cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0001, m_bHoming);
        }
        //------------------------------------------------------------------------
        public void MoveSelHome(EN_SEQ_ID Whre)
        {
            //Local Var.
            bool bErr = false;
            //Time Out.
            m_HomeTimer.OnDelay(m_bHoming, 60000);

            //Inspection.
            cDEF.SEQ.GoDoorLock(true);
            if (cDEF.SEQ._bRun              ) { FRM.ShowWarn(true, "Machine is Run, the motor can not move.."              ); m_iManNo = 0; m_bHoming = false; return; }
            if (cDEF.SEQ.IsOpenAnyDoor    ()) { FRM.ShowWarn(true, "Door open sensor is detected, the motor can not move.."); m_iManNo = 0; m_bHoming = false; return; }

            if (!cDEF.SEQ.InspectMainAir  ()) bErr = true;
            if (!cDEF.SEQ.InspectSafety   ()) bErr = true;
            if (!cDEF.SEQ.InspectEmergency()) bErr = true;
            if (!cDEF.SEQ.InspectActuator ()) bErr = true;
            if ( cDEF.EPU._bHasErr          ) bErr = true;
            if (bErr)
            {
                m_iManNo = 0;
                m_bHoming = false;
                return;
            }
            
            if (cDEF.EPU.SetErr(vDEF.ERR_PARTHOME+(int)Whre , m_HomeTimer.Out)) 
            { 
                m_iManNo = 0; 
                return; 
            } 
            
            //Return.
            m_bHoming = !cDEF.SEQ.MoveHome(Whre);
            if (!m_bHoming) { m_iManNo = 0; m_bHoming = false; }
        }

        //------------------------------------------------------------------------
		//MoveMotr
        public void ManMoveMotr(EN_MOTR_ID iMotr, EN_COMD_ID Cmd) 
        {
            //JUNG/220128
            for (int i = 0; i < (int)EN_MOTR_ID.EndOfId; i++)
            {
                if (iMotr == (EN_MOTR_ID)i) continue;
                cDEF.MOTR.Stop((EN_MOTR_ID)i);
            }

            int iPart = 0; 
            int iItem = 0;
            if (!cDEF.POSN.GetMotorPart(ref iPart, ref iItem, (int)iMotr)) return;
            cDEF.SEQ.MoveMotr((EN_SEQ_ID)iPart, iMotr, Cmd);
        }
        //------------------------------------------------------------------------
        public void ManMoveDirect(EN_MOTR_ID iMotr, double dPosn) 
        {
            int iPart = 0;
            int iItem = 0;
            if (!cDEF.POSN.GetMotorPart(ref iPart, ref iItem, (int)iMotr)) return;
            cDEF.SEQ.MoveDirect((EN_SEQ_ID)iPart, iMotr, dPosn);
        }

		//Manual Cycle
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



		//Manual Processing.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ManProcOn(int No, bool OnKey, bool OffKey, _TFuncArg Arg) 
        { 
            FuncArg = Arg; 
            ManProcOn(No, OnKey, OffKey); 
        }
        //--------------------------------------------------------------------------
        public void ManProcOff(int No, bool OnKey, bool OffKey, _TFuncArg Arg) 
        { 
            FuncArg = Arg; 
            ManProcOff(No, OnKey, OffKey); 
        }
        //------------------------------------------------------------------------
        public void ManProcOn(int No, bool OnKey, bool OffKey)
        {

            //Local Var.
            bool   isBtnAutoSW   = cDEF.SEQ._bAutoMode;
            double dPosn         = 0.0;
            //bool   bWithOutDoor  = (No>=400 && No <= 405) || (No>=4500 && No <= 4507) || (No>=2500 && No<=2503);
            bool   bSimMode      = cDEF.FM.SysOptn.bSimulRun ;

            //Check No. Error.
            if (No <  0) return;

            //Clear HomeEnd Flag.
            for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++)
            {
                if(!OnKey && (No == cDEF.MOTR.ManNoJog((EN_MOTR_ID)i))) cDEF.MOTR.Stop((EN_MOTR_ID)i);
            }

            //Check Running.
            //if (cDEF.SEQ._bRun                           ) { FRM.ShowWarn(true, "Machine Is Run!!"                                      ); return; }
            if (cDEF.SEQ._bRun && No == 1 ) 
            {
                cDEF.SEQ._bBtnManStop = true; 
            }

            if (m_iManNo != 0                            )                                                                                 return;
            //if (isBtnAutoSW && !bSimMode                 ) { FRM.ShowWarn(true, "Please Change Manual Mode, the motor can not move.."   ); return; }
            //if (cDEF.SEQ.IsOpenAnyDoor() && !bWithOutDoor) { FRM.ShowWarn(true, "Door open sensor is detected, the motor can not move.."); return; }

            //Trace Log.
            if (m_iPrevManNo != m_iManNo) cDEF.LOG.Trace($"MANUAL ON [{No:0000}]");
            m_iPrevManNo = m_iManNo;

            //Clear HomeEnd Flag.
            for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++)
            {
                if(No == cDEF.MOTR.ManNoHome((EN_MOTR_ID)i)) cDEF.MOTR.ClearHomeEnd((EN_MOTR_ID)i);
                //Jog Stop.
                if (!OnKey && (No == cDEF.MOTR.ManNoJog ((EN_MOTR_ID)i))) cDEF.MOTR.Stop((EN_MOTR_ID)i);
                if (           No == cDEF.MOTR.ManNoStop((EN_MOTR_ID)i) ) cDEF.MOTR.Stop((EN_MOTR_ID)i);
            }

            //Set Manual No & Flag.
            m_iManNo    = No  ;
            m_bLtProcOn = true;


            for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++)
            {
                if (!OnKey || OffKey) continue;
                     if (No == cDEF.MOTR.ManNoStop  ((EN_MOTR_ID)i)    ) ManMoveMotr         ((EN_MOTR_ID)i, EN_COMD_ID.Stop);
                else if (No == cDEF.MOTR.ManNoJog   ((EN_MOTR_ID)i)    ) ManMoveMotr         ((EN_MOTR_ID)i, EN_COMD_ID.JogP);
                else if (No == cDEF.MOTR.ManNoServo ((EN_MOTR_ID)i)    ) cDEF.MOTR.SetServo  ((EN_MOTR_ID)i, true     );
                else if (No == cDEF.MOTR.ManNoAlarm ((EN_MOTR_ID)i)    ) cDEF.MOTR.SetAlarm  ((EN_MOTR_ID)i, true     );
                else if (No == cDEF.MOTR.ManNoHome  ((EN_MOTR_ID)i)    ) ManMoveMotr         ((EN_MOTR_ID)i, EN_COMD_ID.Home);
                else if (cDEF.POSN.GetPosnByManNo   (i, No, out dPosn )) ManMoveDirect       ((EN_MOTR_ID)i, dPosn    );
            }

            for(int i=0; i<cDEF.ACTR._iNumOfACT;i++) 
            {
                if (!OnKey || OffKey) continue;
                if (No == cDEF.ACTR.ManNoActr(i)) cDEF.ACTR.MoveCyl(i, (int)EN_ACTR_CMD.Fwd);
            }

            if(!OnKey || OffKey) No = 0;
            
            //Selection.
            switch (No) 
            {
                case 0001: ClearHomeStep (EN_SEQ_ID.ALL); m_bHoming = true;                            return;  //ALL HOME
                case 0002:                                                                             return; 
                case 0003:                                                                             return; 
                case 0004:                                                                             return;  //
                case 0005:                                                                             return;  //
                                                                                                       
                //UserSet - PART HOME의 처리                                                            
                //ex)                                                                                  
                //case 0006: ClearSelHomeStep (piXXX); m_bHoming = true;                                 return;  //
                                                                                                       
                                                                                                       
                //                                                                                     
                case 2002: cDEF.SEQ.WAT._iStepMan = 10;                                                return;  //Align Cycle
                                                                                                       
                case 2003: cDEF.SEQ.WAT.SetLight(true );                                               break;
                case 2004: cDEF.SEQ.WAT.SetLight(false);                                               break;
                                                                                                       
                case 2010: cDEF.SEQ.WAT._iStepMan = 10;                                                return;
                case 2011: cDEF.SEQ.WAT._iStepMan = 10;                                                return;
                case 2012: cDEF.VISN.VisnimageStrt((int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn, FuncArg.sArg1); break;
                case 2013: cDEF.SEQ.WAT._iStepMan = 10;                                                return;
                case 2014: cDEF.SEQ.WAT._iStepMan = 10;                                                return;

                case 2020: cDEF.SEQ.WAT._iStepMan = 10;                                                return;

                case 2500: cDEF.IO.sY(EN_OUT_ID.yVACUUM_ON   , true);                                  break;
                case 2501: cDEF.IO.sY(EN_OUT_ID.yVACUUM_PURGE, true);                                  break;


                case 3002: cDEF.BCR.Reset(); cDEF.BCR.CmdSetRead();                                   break ; //Wafer Align - Read Tag


            }

            //Reset Manual No & Flag.
            m_iManNo    = 0;

        }
        //------------------------------------------------------------------------
        public void ManProcOff(int No, bool OnKey, bool OffKey)
        {
            bool isBtnAutoSW   = cDEF.SEQ._bAutoMode;
            //bool bWithOutDoor  = (No >= 400 && No <= 405) || (No >= 4500 && No <= 4507) || (No >= 2500 && No <= 2503);

            //Check No. Error.
            if (No <  0           ) return;

            //Clear HomeEnd Flag.
            //for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++)
            //{
            //    if(!OnKey && (No == cDEF.MOTR.ManNoJog((EN_MOTR_ID)i))) cDEF.MOTR.Stop((EN_MOTR_ID)i);
            //}

            //Check Running.
            if (cDEF.SEQ._bRun                           ) { FRM.ShowWarn(true, "Machine Is Run!!"                                      ); return; }
            //if (isBtnAutoSW                              ) { FRM.ShowWarn(true, "Please Change Manual Mode, the motor can not move.."   ); return; }
                                                         
            if (m_iManNo != 0                            ) return;
            //if (cDEF.SEQ.IsOpenAnyDoor() && !bWithOutDoor) { FRM.ShowWarn(true, "Door open sensor is detected, the motor can not move.."); return; }

            //Trace Log.
            if (m_iPrevManNo != m_iManNo) cDEF.LOG.Trace($"MANUAL OFF [{No:0000}]") ;
            m_iPrevManNo = m_iManNo;

            //Jog Stop.
            for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++)
            {
                //Jog Stop.
                if(!OffKey && (No == cDEF.MOTR.ManNoJog((EN_MOTR_ID)i))) cDEF.MOTR.Stop((EN_MOTR_ID)i);
            }
            
            //Set Manual No & Flag.
            m_iManNo    = No  ;
            m_bLtProcOn = true;

            for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++)
            {
                 if(!OffKey || OnKey) continue;
                      if (No == cDEF.MOTR.ManNoStop  ((EN_MOTR_ID)i)) ManMoveMotr         ((EN_MOTR_ID)i, EN_COMD_ID.Stop);
                 else if (No == cDEF.MOTR.ManNoJog   ((EN_MOTR_ID)i)) ManMoveMotr         ((EN_MOTR_ID)i, EN_COMD_ID.JogN);
                 else if (No == cDEF.MOTR.ManNoServo ((EN_MOTR_ID)i)) cDEF.MOTR.SetServo  ((EN_MOTR_ID)i, true           );
                 else if (No == cDEF.MOTR.ManNoAlarm ((EN_MOTR_ID)i)) cDEF.MOTR.SetAlarm  ((EN_MOTR_ID)i, true           );
            }

            //
            for(int i=0; i<cDEF.ACTR._iNumOfACT;i++) 
            {
                 if(!OffKey || OnKey)  continue;
                 if (No == cDEF.ACTR.ManNoActr(i)) cDEF.ACTR.MoveCyl(i, (int)EN_ACTR_CMD.Bwd);
            }

            if(!OffKey || OnKey) No = 0;

            //Selection.
            switch (No) 
            {
                default : break;
                

                //UserSet - PART MANUAL 처리 
                //ex)
                //case 2000: cDEF.SEQ.MGZ.MoveMotr    (EN_MOTR_ID.mGRP_Z , EN_COMD_ID.OneStepB1 ); break ;
                //

               
                case 2500: cDEF.IO.sY(EN_OUT_ID.yVACUUM_ON   , false);                           break;
                case 2501: cDEF.IO.sY(EN_OUT_ID.yVACUUM_PURGE, false);                           break;

            }
            //Reset Manual No & Flag.
            m_iManNo    = 0;
            m_bLtProcOn = false  ;
        }
        //------------------------------------------------------------------------
        public bool isSkipTimeOut(int No)
        {//UserSet - Time Out 제외할 Manual No 추가(MAMNUAL 동작 시간을 길 경우 사용)
            if (No == 2010) return true;
            if (No == 2011) return true;
           

            return false;
        }
        //------------------------------------------------------------------------
        public bool ManCycleRun()
        {
            //bool bDrngFlag = true;

            try
            {
                if (cDEF.SEQ._bRun) return true;

                //Check Repeating.
                m_bRptActrIng = false;
                for (int n = 0 ; n < cDEF.ACTR._iNumOfACT ; n++)
                {
                    if (m_bRptActr[n]) { m_bRptActrIng = true; break; }
                }

                //강제 Manual 작업시 다른 Manual은 무시.
                if (m_bRptMotr || m_bRptActrIng) 
                {
                    m_iManNo = 0;
                }

                //Master Manual Control. (간섭 확인 없음)
                if (cDEF.FM.m_iCrntLevel > (int)EN_LOGIN.Engineer) 
                {
                    //Repeat Functions.
                    if (m_bRptMotr      ) RptMotr();
                }

                //Check Alarm.
                if (cDEF.EPU ._bHasErr) { Init(); return true; }

                //
                bool isCheckTO = !m_bOneShot && !m_bHoming && (m_iManNo != 0 ) && !isSkipTimeOut(m_iManNo);
                if (m_CycleTimer.OnDelay(isCheckTO , 60 * 1000 * 3)) 
                {
                    string Msg = string.Format($"[{m_iManNo:0000}] - Manual Cycle TimeOut");
                    FRM.ShowWarn(true, Msg);
                    m_iManNo   = 0;
                    cDEF.LOG.Trace(Msg);
                }

                //Check No. Cycle.
                if (m_iManNo == 0) { m_bHoming = false; return true; }
                if (!m_bLtProcOn ) return true;

                //Cycle Step.
                if     ((m_iManNo == 1) && m_bHoming && (cDEF.SEQ._iSeqStat == EN_SEQ_STAT.Init      )) { MoveAllHome(            ); m_iStepMan = 0; } //00//ALL PART HOME

                //
                else if (m_iManNo == 2002) { if (cDEF.SEQ.WAT.ManOneCycle   (     )) { m_iStepMan = 0; m_iManNo = 0; } } //Align Cycle
                                                                                  
                else if (m_iManNo == 2010) { if (cDEF.SEQ.WAT.ManAlgnCycle  (     )) { m_iStepMan = 0; m_iManNo = 0; } } //Manual Align Cycle
                else if (m_iManNo == 2011) { if (cDEF.SEQ.WAT.ManOneCycle   (true )) { m_iStepMan = 0; m_iManNo = 0; } } //Manual Align Cycle - Continue mode
                else if (m_iManNo == 2013) { if (cDEF.SEQ.WAT.ManImgOneCycle(false)) { m_iStepMan = 0; m_iManNo = 0; } } //Manual Align Cycle - Continue mode
                else if (m_iManNo == 2014) { if (cDEF.SEQ.WAT.FileAlgnCycle (     )) { m_iStepMan = 0; m_iManNo = 0; } } //Manual Align Cycle - Continue mode

                else if (m_iManNo == 2020) { if (cDEF.SEQ.WAT.ManGrabCycle(    )) { m_iStepMan = 0; m_iManNo = 0; } } //Manual Align Cycle - Continue mode


                //
                return false;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"ManProc ManCycleRun Exception - {err.Message}");
                cDEF.LOG.ExceptionTrace("ManProc ManCycleRun Exception - ", err);
                return false;
            }
        }

        //------------------------------------------------------------------------
            public bool ManFunction()
            {
                if (m_iFuncStep == 0)
                {
                    if (m_bReqWCK)
                    {
                        m_bReqWCK   = false;    
                        m_iFuncStep = 10;
                        return false;
                    }
                }

                //
                if (m_iFuncStep < 10) m_iFuncStep = 0;
                switch (m_iFuncStep)
                {
                    default:
                        m_iFuncStep = 0;
                        return true;

                    case 10:
                        //Check Vacuum
                        if (cDEF.FM.EngrOptn.iVacOption == 1) //Only Vacuum Sensor
                        {
                            m_iFuncStep = 20;
                            return false;
                        }
                    
                        m_iFuncStep ++;
                        return false;

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    case 11: //Only Sensor
                        if (cDEF.FM.EngrOptn.iVacOption == 0 || cDEF.SEQ.WAT.IsWaferExist())
                        {
                            if (m_ManCmdData.Command != eCommand.None) cDEF.COMZEUS.SetResultWCK(m_ManCmdData, cDEF.SEQ.WAT.IsWaferExist());
                        
                            m_iFuncStep = 0;
                            return true;

                        }

                        m_iFuncStep = 20;
                        return false;

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    case 20: //Only Vacuum
                        //if (!cDEF.SEQ.WAT.IsWaferExist())
                        //{
                        //    if (m_ManCmdData.Command != eCommand.None) cDEF.COMZEUS.SetResultWCK(m_ManCmdData, cDEF.SEQ.WAT.IsWaferExist());

                        //    m_iFuncStep = 0;
                        //    return true;

                        //}

                        m_TestTimer.Clear();
                        m_nVacCheckCount = 0;
                    
                        m_iFuncStep++;
                        return false;

                    case 21:
                        if (!cDEF.SEQ.WAT.SetVacOn()) return false;

                        m_TempTimer.Clear();
                        m_iFuncStep++;
                        return false;
                
                    case 22:
                        m_TestTimer.OnDelay(true, cDEF.FM.EngrOptn.iVacTimeOut);
                        if (m_TestTimer.Out)
                        {
                            cDEF.SEQ.WAT.SetVacOff();
                        
                            m_iFuncStep = 25;
                            return false;
                        }

                        if (!m_TempTimer.OnDelay(true, 100)) return false; //100ms delay

                        if (cDEF.SEQ.WAT.IsVacOn()) m_nVacCheckCount++; 
                        else                        m_nVacCheckCount = 0;

                        //if (cDEF.FM.EngrOptn.iVacCount == Math.Abs(m_nVacCheckCount))
                        if (m_nVacCheckCount >= cDEF.FM.EngrOptn.iVacCount)
                        {
                            m_iFuncStep = 23;
                            return false;
                        }

                        m_iFuncStep = 21;
                        return false;

                    case 23:
                        if (!cDEF.SEQ.WAT.SetVacOff()) return false; 

                        if (m_ManCmdData.Command != eCommand.None) cDEF.COMZEUS.SetResultWCK(m_ManCmdData, true) ;
                    
                        m_iFuncStep = 0;
                        return true;
                
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    case 25:
                        cDEF.SEQ.WAT.SetVacOff();

                        m_ManCmdData.ErrorNumber = (int)EN_ERR_LIST.ERR_0015; //Vacuum Check Error
                        if (m_ManCmdData.Command != eCommand.None) cDEF.COMZEUS.SetResult(m_ManCmdData);

                        m_iFuncStep = 0;
                        return true;

                }

            }

        //--------------------------------------------------------------------------
        public void SetCmdData(cCmdData data)
        {
            m_ManCmdData = data; 
        }
        //--------------------------------------------------------------------------
        public void ClearCmdData()
        {
            m_ManCmdData.ClearCMD();
        }

    }
}
