using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using YJ_CLib;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TAxisSimul                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class TAxisSimul
    {
        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_tWaitTimer   = new TOnDelayTimer();
        TOnDelayTimer m_tServoWait   = new TOnDelayTimer();
        TOnDelayTimer m_tRingCounter = new TOnDelayTimer();
        TOnDelayTimer m_tAlarmReset  = new TOnDelayTimer();
        TOnDelayTimer m_tHomeEnd     = new TOnDelayTimer();
        
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
		//double m_dCrntCnt     ;
		//double m_dPreCnt      ;
		//bool   m_bMoveStop    ;

        int  m_iHomeType      ;
        int  m_iHomeSignal    ;
        int  m_iHomeZPhase    ;
        int  m_iGroupAxeNo    ;
        uint m_uModuleID      ;
        bool m_bReqServoOn    ;
        bool m_bReqResetAlarm ;
        bool m_bSetUseTorque  ;
        int  m_lTotalAxis     ; 
        int  m_iHomeDly       ;


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public String m_sParamPath     ;
        public int    m_iMotorType     ;
        public int    m_iMotorKind     ;
        public double m_dCoef          ;
        public int    m_iSONLevel      ;
        public int    m_iMotrKind      ;
        public string m_sToqWAddr      ;
        public string m_sToqRAddr      ;

        //Vars. - Home
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int    m_iSetHomeLevel ;
        public bool   m_bKeepHomeProc ;
        public bool   m_bForceHome    ;
        public double m_dHomeVel      ;
        public double m_dHomeAcc      ;
        public double m_dHomeDec      ;
        public double m_dHomeOffset   ;
        public double m_dHomeOffsetPos;


        //Var. - Update
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   m_bServo         ;
        public bool   m_bHome          ;
        public bool   m_bStop          ;
        public bool   m_bReady         ;
        public bool   m_bBusy          ;
        public bool   m_bHomeEnd       ;
        public bool   m_bPackInPosn    ;
        public bool   m_bAlarm         ;
        public bool   m_bCW            ;
        public bool   m_bCCW           ;
        public bool   m_bLtBusy        ;
        public bool   m_bRing          ;
        public bool   m_bLtHomeSen     ;
        public bool   m_bRingCounter   ;
        public double m_dTorque        ;
        public double m_dPreTrgPos     ;
        public double m_dTrgPos        ;
        public double m_dAbsOffset     ;
        public double m_dRingMaxCnt    ;
        public int    m_iStepHome      ;
        public bool   m_bApplyScurve   ;
        public int    m_iHomeDir       ;

        public double m_dCmdPos        ;
        public double m_dEncPos        ;

        public double m_dOrgCmdPos	   ;
        public double m_dOrgEncPos	   ;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		TAxisSimulatior m_SimulAxis    = new TAxisSimulatior();
		SSimulAxisInfo  m_GetMotrInfo  = new SSimulAxisInfo ();

		//JSimulMoveInfo m_SimulAxis   = new JSimulMoveInfo();
		//SSimulAxisInfo m_GetMotrInfo = new SSimulAxisInfo();

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TAxisSimul()
        {
            Init();
        }
        ~TAxisSimul() { }

        //Base Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void       Init()
        {
            m_iStepHome     = 0;
            m_bKeepHomeProc = false;
            m_dHomeVel      = 0.0;
            m_dHomeAcc      = 0.0;
            m_dHomeDec      = 0.0;
            m_dHomeOffset   = 0.0;
            m_dAbsOffset    = 0.0;
            m_bReqServoOn   = false;
            m_lTotalAxis    = 0;       
        }

        public void  SetComPort    (string sPort)
        {

        }

        //--------------------------------------------------------------------------
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        public bool       Open                ()
        {
            //Local Var.

            //Check Already Init.
            //if (m_lTotalAxis > 0) return true;

            try {
			  
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxisSimul. Open " + ex.ToString());
            }
            //Return.
            return true; //(m_lTotalAxis > 0);
        }
        //--------------------------------------------------------------------------
        public void       Close               ()
        {

        }
        //--------------------------------------------------------------------------
        public bool       DevReset            ()
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void       Reset               ()
        {
            //Reset homing flag.
            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome     = 0;
            }
        }
        //--------------------------------------------------------------------------
		public void       ClearHomeEnd        ()
        {
            m_bKeepHomeProc = false;
            m_iStepHome     = 0;
            m_bHomeEnd      = false;

			
        }
        //Conv' Functions.
        //--------------------------------------------------------------------------
        double ConvVel      (double Vel)
        {
	        double lVel = m_dCoef * Vel;

	        return lVel;
        }
        //--------------------------------------------------------------------------
        double ConvAcc      (double Vel, double Acc)
        {
	        double lAcc = m_dCoef * Vel * (1.0 / Acc);

            return lAcc;
        }
		//-------------------------------------------------------------------
        //get/Set Functions.
		public bool  	  gServo              () {return m_bServo;      }
		public bool  	  gHome               () {return m_bHome;       }
		public bool  	  gStop               () {return m_bStop;       }
		public bool  	  gReady              () {return m_bReady;      }
		public bool  	  gBusy               () {return m_bBusy;       }
		public bool  	  gHomeEnd            () {return m_bHomeEnd;    }
		public bool  	  gPackInPosn         () {return m_bPackInPosn; }
		public bool  	  gAlarm              () {return m_bAlarm;      }
		public bool  	  gCW                 () {return m_bCW;         }
		public bool  	  gCCW                () {return m_bCCW;        }
		public bool  	  gLtBusy             () {return m_bLtBusy;     }
		public bool  	  gRing               () {return m_bRing;       }
		public double	  gTorque             () {return m_dTorque;     }
		public double	  gPreTrgPos          () {return m_dPreTrgPos;  }
		public double	  gTrgPos             () {return m_dTrgPos;     }
		public double	  gOrgEncPos          () {return m_dOrgEncPos;  }
		public int	      gHomeStep           () {return m_iStepHome;   }

        public void       sPreTrgPos          (double bSet) { m_dPreTrgPos = bSet; }
        public void       sTrgPos             (double bSet) { m_dTrgPos    = bSet; }
        public void       sHomeEnd            (bool   bSet) { m_bHomeEnd   = bSet; }

		//-------------------------------------------------------------------
        //Move Functions.
        public void SetServo(int iAxis, int iOn)
        {
            //Servo On/Off.
            if (iOn != 1)
            {
                if (!m_bServo) return;
				m_bServo = false;
                return;
            }

            // Encoder type을 설정한다.
            // 기본값은 0(TYPE_INCREMENTAL)로 설정되어 있습니다.
            // 설정값은 0 ~ 1까지 설정 할 수 있습니다.
            // 설정값 : 0(TYPE_INCREMENTAL), 1(TYPE_ABSOLUTE).
            if (m_bServo) return;

            m_bReqServoOn = true;
            m_tServoWait.Clear();
        }
        //--------------------------------------------------------------------------
        public void SetAlarm(int iAxis, int On)
        {
            m_bReqResetAlarm = true;
        }
        //--------------------------------------------------------------------------
        public bool       EmrgStop            (int iAxis        )
        {
            //E-Stop.
            return Stop(iAxis, false, 0);
        }
        //--------------------------------------------------------------------------
		public bool       Stop                (int iAxis, bool   DecStop = false , double DecTime = 0.1)
        {
            //Local Var.
            //uint  uReturn = 0;
            if (!m_bServo) return false;

            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome = 0;
            }

            //Stop.
            m_SimulAxis.SetStop();

            return true;
        }
        //--------------------------------------------------------------------------
        public bool       MoveJogP            (int iAxis, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            bool bRet = false;

            //Check Status.
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;
            if (Vel < 0.1) Vel = 0.1;

            double lVel = ConvVel(Vel);
            double lAcc = Acc; //ConvAcc(Vel, Acc);
            double lDec = Dec; //ConvAcc(Vel, Dec);            

            //uReturn = CAXM.AxmMoveVel(iAxis, lVel, lAcc, lDec);
	
			bRet = m_SimulAxis.SetMoveInfo(YJ_CLib.JSimulMoveType.SIM_MT_VEL, YJ_CLib.JSimulVelProfile.SIM_VP_TRAPZOID,  m_SimulAxis._dTrgCnt+2, lVel, lAcc, lDec);

            return bRet;
        }
        //--------------------------------------------------------------------------
		public bool       MoveJogN            (int iAxis, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            bool bRet;

            //Move Jog.
            //Check Status.
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Vel <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;

            double lVel = ConvVel(Vel);
            double lAcc = Acc; //ConvAcc(Vel, Acc);
            double lDec = Dec; //ConvAcc(Vel, Dec); 
            //
			bRet = m_SimulAxis.SetMoveInfo(YJ_CLib.JSimulMoveType.SIM_MT_VEL, YJ_CLib.JSimulVelProfile.SIM_VP_TRAPZOID,  m_SimulAxis._dTrgCnt-2, -lVel, lAcc, lDec);

            return bRet;
        }
        //--------------------------------------------------------------------------
        public bool       Move                (int iAxis, double Pos , double Vel = 20.0, 
                                                          double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0, int iSpdRatio = 0)
        {
            //Local Var.
			bool   bRet = false;
            //long lFuncRet     ;
            //uint dwProfile = 0;
            //double dPos       ;

            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Dec <= 0 ) Acc = Dec;


            double lVel = ConvVel(Vel);
            double lAcc = Acc; //ConvAcc(Vel, Acc);
            double lDec = Dec; //ConvAcc(Vel, Dec);

			if (m_bStop)
				bRet = m_SimulAxis.SetMoveInfo(YJ_CLib.JSimulMoveType.SIM_MT_ABS, YJ_CLib.JSimulVelProfile.SIM_VP_TRAPZOID, Pos, lVel, lAcc, lDec);

			return bRet;

            //if (m_bApplyScurve)
            //{
            //    CAXM.AxmMotGetProfileMode(iAxis, ref dwProfile);
            //    if ((dwProfile == 0) || (dwProfile == 1)) dwProfile += 3;
            //    else dwProfile = 3;
            //    CAXM.AxmMotSetProfileMode(iAxis, dwProfile);
            //}

            //lFuncRet = CAXM.AxmMoveStartPos(iAxis, Pos, lVel, lAcc, lDec);
            //CAXM.AxmMotSetParaLoad(iAxis, Pos, lVel, lAcc, lDec);

            //return (lFuncRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS);
        }
        //--------------------------------------------------------------------------
        public bool       MoveOverride        (int iAxis, double Pos , double Vel = 20.0, 
                                                         double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0)  //abs move with mm.
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool       MoveHome            (int iAxis, double Vel , double Acc, double Dec = 0.0, double OffsetPulse = 0.0 , double OffSetPos = 0.0)
        {

            if (m_bAlarm       ) return false;
            if (!m_bServo      ) return false;
            if (Vel <= 0       ) return false;
            if (m_bKeepHomeProc) return false;
            if (Dec <= 0       ) Acc = Dec;

            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS || m_iHomeType == (int)EN_HOME_TYPE.DataSet)
            {
                //
                m_bHomeEnd = true;
            }
            else
            {
                m_dHomeVel       = Vel;
                m_dHomeAcc       = Acc;
                m_dHomeDec       = Dec;
                m_dHomeOffset    = OffsetPulse;
                m_dHomeOffsetPos = OffSetPos;
                m_iStepHome      = 10;
                m_bKeepHomeProc  = true ;
				m_bForceHome     = false;
            }
            return true;
        }
        //--------------------------------------------------------------------------
        public bool SetMoveHomeForce       (int iAxis, double Vel , double Acc)
        {
            if (m_bAlarm       ) return false;
            if (!m_bServo      ) return false;
            if (Vel <= 0       ) return false;
            if (m_bKeepHomeProc) return false;

            m_dHomeVel      = Vel;
            m_dHomeAcc      = Acc;
            m_dHomeDec      = Acc;
            m_dHomeOffset   = 0.0;
            m_dHomeOffsetPos= 0.0;
            m_iStepHome     = 10;
            m_bKeepHomeProc = true;
            m_bForceHome    = true;
            return true;
        }
        //--------------------------------------------------------------------------
        public bool       MoveOverrideVel     (int iAxis, double Pos , double Vel , 
                                                          double Acc , double Dec, double dOverridePos, double dOverrideVelocity)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool HomeStart           (int iAxis, double Vel , double Acc, double Dec)
        {
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;

			bool   bRet = false;
			double dPos = 0.0;
            double lVel = ConvVel(Vel);
            double lAcc = Acc;
            double lDec = Dec;

			bRet = m_SimulAxis.SetMoveInfo(YJ_CLib.JSimulMoveType.SIM_MT_ABS, YJ_CLib.JSimulVelProfile.SIM_VP_TRAPZOID, dPos, lVel, lAcc, lDec);

            return bRet;            
        }
        //--------------------------------------------------------------------------
        public bool       HomeProc            (int iAxis)
        {
            //Local Var.
            bool bHomeResult = false;

            //Check Alarm.
            if ( m_bAlarm ) { m_iStepHome = 0; m_bKeepHomeProc = false; return false; }
            if (!m_bServo ) { m_iStepHome = 0; m_bKeepHomeProc = false; return false; }

	        if(m_iStepHome > 12 && m_bHome && !m_bLtHomeSen)   m_bLtHomeSen = true;


	         //Cycle.
	         switch (m_iStepHome) {
                  case  0: m_iStepHome = 0;
                           return false;
		          case 10:
				           m_bHomeEnd      = false;
				           m_bLtBusy       = false;
                           m_iHomeDly      = 0;
				           m_bLtHomeSen    = false;
				           //Stop(iAxis);
                           m_iStepHome++;
                           return false;

                  case 11: if(!m_bStop) return false;
                           m_iStepHome++;
                           return false;

                  case 12: //Move.
                           if (!HomeStart(iAxis, m_dHomeVel, m_dHomeAcc, m_dHomeDec))
                           {
                              m_bKeepHomeProc = false;
                              m_iStepHome=0;
                              return false;
                           }
                           m_iStepHome++;
                           return false;
                  case 13: //if (StopCnt++ < 100) return false;
                           bHomeResult = m_SimulAxis.GetMoveHomeEnd();
						   if (!bHomeResult) return false;
				           m_iStepHome++;
				           return false;
				  case 14: if (m_iHomeDly++ < 100) return false;
						   m_SimulAxis.SimulClear();
						   m_iHomeDly = 0;
				           m_iStepHome++;
				           return false;
                  case 15: if (m_iHomeDly++ < 100) return false;
                           
                           if(!m_bForceHome) 
                           {
                              SetPos(iAxis, m_dHomeOffset);
                              m_dPreTrgPos = m_dHomeOffsetPos;
                              m_dTrgPos    = m_dHomeOffsetPos;
                           }

                           m_bForceHome = false;
                           m_iHomeDly = 0;
                           m_iStepHome++;
                           return false;

                  case 16: if (m_iHomeDly++ < 100) return false;
                           m_bHomeEnd = true;
				           m_iStepHome++;
                           return false;

                  case 17: m_bKeepHomeProc = false;
                           m_iStepHome     = 0    ;
                           return true;

                  }

             //NG.
	         return false;
        }
        //--------------------------------------------------------------------------
        public bool       MoveSpline          (int iAxis1, int iAxe2 , double Axe1Vel , double Axe1Acc , double Axe1Dec)
        {
            if (m_bAlarm    ) return false;
            if (!m_bServo   ) return false;
            if (Axe1Vel <= 0) return false;
            if (Axe1Dec <= 0) return false;

            return true;
        }
        //--------------------------------------------------------------------------        
        //Position Functions.
        public double     GetCmdPos           (int iAxis            )    
        {//Get Command Position.

            return m_dCmdPos;         
        }
        //--------------------------------------------------------------------------
        public double     GetEncPos           (int iAxis            ) 
        {//Get Encoder Position.

			return m_dEncPos;
        } 

        public int        gErrCode1           (           ) { return 0; }
        public int        gErrCode2           (           ) { return 0; }

        //--------------------------------------------------------------------------                         
        public bool       SetPos              (int iAxis, double Pos)
        {
			m_SimulAxis.SetSimulSetPos(Pos);

            return true;
        }
        //--------------------------------------------------------------------------
        public bool       SetPosEncToCmd      (int iAxis)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public double     GetOrgCmdPos        (int iAxis            ) { return m_dOrgCmdPos; }               //Get Command Position.
        public double     GetOrgEncPos        (int iAxis            ) { return m_dOrgEncPos; }               //Get Encoder Position.
        //--------------------------------------------------------------------------
        public void       ClearPos            (int iAxis, double Pos = 0.0)
        {
            m_bHomeEnd = false;
            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome = 0;
                Stop(iAxis);
            }
            SetPos(iAxis, Pos);
        }
        //--------------------------------------------------------------------------
        //Get Function
        public bool       GetStop             (int iAxis, bool ChkEnc = false , double InPos = 0.1)  
        {//Motion Done.
            return false;
        }
        //--------------------------------------------------------------------------
        public bool       MotionDone          (int iAxis)
        {
            return m_bStop;
        }
        public bool GetContiMotnDone(int CoodiNo)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        //Set Functions.
        public void       SetParamPath        (String Path, String CmePath = "")
        {
            m_sParamPath = Path;
        }
        //--------------------------------------------------------------------------
        public void       SetType             (int iAxis, int    iType = 0 ,int iKind = 0 , int iNotUse = 0)
        {
            //int  iBoardNo   = 0;
            //int  iMidulePos = 0;
            uint uModuleID  = 0; 

            m_iMotorType    = iType;
            m_iMotorKind    = iKind;

            //[06H] PCI-N804,404
            //[0AH] Mechatrolink II
            //[24H] Mechatrolink III
            m_uModuleID = uModuleID;
            //통신 연결 안되어 있으면 값 안넘어와서 0x00 추가
            bool isECAT = m_uModuleID == 0x00 || m_uModuleID == 0xE1;

            if(!isECAT) { }

        }
        //--------------------------------------------------------------------------
		public void       SetABS              (int iAxis, int    Data1    = 0,int Data2 = 0     )
        {
            m_iMotrKind = Data1;
        }
        public void       SetAbsOrgOffset     (int iAxis, double Data)
        {
        }
        //--------------------------------------------------------------------------
		public void       SetPulseOut         (int iAxis, int    Data     = 1                        )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetEncInput         (int iAxis, int    Data     = 2                        ) 
        {//2:Sqr4Mode
        }
        //--------------------------------------------------------------------------
        public void       SetSONLevel         (int iAxis, int    Data     = 0                        ) 
        {
        }
        //--------------------------------------------------------------------------
        public void       SetMaxSped          (int iAxis, long   Vel                                 )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetInpLevel         (int iAxis, int    Data     = 0                        )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetAlarmLevel       (int iAxis, int    Data     = 0                        )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetLimitLevel       (int iAxis, int    PosData  = 0 , int NegData     = 0  )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetHomeLevel        (int iAxis, int    Data     = 0                        )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetDirection        (int iAxis, int    Data                                )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetAutoResp         (int iAxis, int    Data                                )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetPackType         (int iAxis, int    Data                )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetHomeType         (int iAxis, int    Data                )
        {
            m_iHomeType = Data;
        }
        //--------------------------------------------------------------------------
        public void       SetHomeOptn         (int iAxis, int    Data, int Data2     )
        {
            m_iHomeDir = Data2;
        }
        //--------------------------------------------------------------------------
        public void       SetServoType        (int iAxis, int    Data     = 1                        )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetAppScurve        (int iAxis, bool   Data     = true                     )
        {
            m_bApplyScurve = Data;
        }
        //--------------------------------------------------------------------------
        public void       SetIntpAxe          (int iAxis, int    Data     = -1                       )
        {
        }
        //--------------------------------------------------------------------------
        public void SetPairAxe(int lMasterAxeNo, int lSlaveAxeNo = -1, int Data2 = 1)
        {
        }
        //--------------------------------------------------------------------------
        public void       SetCoefficient      (double  Data     = 819                               )
        {
            m_dCoef = Data;
        }
        //--------------------------------------------------------------------------
        public void       SetEncPulse         (int iAxis, int    Data                    )
        {
        }
        //--------------------------------------------------------------------------
        public void       SetRingCounter      (int iAxis, bool bEnable, double dMaxCntr  )
        {
            m_bRing        = bEnable ;
            m_bRingCounter = bEnable ;
            m_dRingMaxCnt  = dMaxCntr;

			m_SimulAxis.SetRingInfo(m_bRingCounter, m_dRingMaxCnt);
        }

        //--------------------------------------------------------------------------
        public void       SetListedMotr       (int iAxis1 , int iAxis2)
        {
        }
        //--------------------------------------------------------------------------
        public void  	  SetMoveHomeSensor   (int iAxis, int Data1 = 2, int Data2 = 0 ) 
        {
            m_iHomeSignal = Data1;
            m_iHomeZPhase = Data2;
        }
        public void       SetEndLimitEnable   (int iAxis, int Data = 0) {  }
        public void       SetAbsOffset        (double     dOffset     ) {m_dAbsOffset       = dOffset;}
        //--------------------------------------------------------------------------
        public void       SetServoParam       (int iAxis, int iParamNo, int Data)
        {
        }
        //--------------------------------------------------------------------------
        //Set Trigger
        public void       SetTriggerReset     (int iAxis)
        {
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerTimeLevel (int iAxis, double dPulseWidth)
        {
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerPos       (int iAxis, double dPos, double dTrigWidth = 100.0)
        {
        } 
        //--------------------------------------------------------------------------      
        public bool       SetTriggerBlock     (int iAxis, double dStartPos, double dEndPos, double dPeriod, double dTrigWidth = 100.0)
        {
			return true;
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerOneShot   (int iAxis, double dPulseWidth) 
        {
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorque          (int iAxis, double dTorque, double Pos , double Vel = 20.0, double Acc = 0.3, double Dec = 0.0)
        {
			return true;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueP         (int iAxis, double dTorque, double dVel)
        {
			return true;
        } 
        //--------------------------------------------------------------------------      
        public bool       MoveTorqueN         (int iAxis, double dTorque, double dVel)
        {
			return true;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueStop      (int iAxis)
        {
			return true;
        }
        //--------------------------------------------------------------------------
        public bool       SetParamTorque      (int iAxis)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void       SetUseTorque        (int iAxis, bool bUse, string sWAddr, string sRAddr)
        {
            m_bSetUseTorque = bUse   ;
            m_sToqWAddr     = sWAddr ;
            m_sToqRAddr     = sRAddr ;
        }

        //--------------------------------------------------------------------------
        public double       GetTorque      (int iAxis)
        {
            double dTroque  = 0.0;
            return dTroque;
        }
        //--------------------------------------------------------------------------
        public bool       SetTorqueLimit      (int iAxis, double dTorque)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void Update(int iAxis)
        {
            try
            {
                //Local Val
                double      dEncPos     ;
                double      dScanTime   ;
                double      dStrtTime   ; 


                if ((iAxis < 0) || (iAxis >= cDEF.MOTR._iNumOfMotr)) return;

                dStrtTime = cDEF.TICK._GetTickTime(); 
				//
				m_SimulAxis.UpdateAxisStat();
				m_SimulAxis.GetSimulAxisInfo(ref m_GetMotrInfo);
				//
                m_dTorque     = 0.0;
                dEncPos       = m_GetMotrInfo.dEncCnt;
                m_dCmdPos     = m_GetMotrInfo.dEncCnt;
                m_dEncPos     = dEncPos              ;

                //m_bServo      = m_bServo;

                //Motion Status.
	            m_bHome       =  m_GetMotrInfo.bHome; //Home Sensor
                m_bBusy       =  m_GetMotrInfo.bBusy; //Busy.
	            m_bPackInPosn =  m_GetMotrInfo.bPackInPosn; //In Position.
	            m_bAlarm      =  m_GetMotrInfo.bAlarm; //Alarm.
	            m_bCW         =  m_GetMotrInfo.bCW; //CW.
                m_bCCW        =  m_GetMotrInfo.bCCW; //CCW.
	            m_bStop       =  !m_bBusy && m_bPackInPosn; //Stop.
	            m_bReady      =  !m_bCW && !m_bCCW && !m_bAlarm && m_bHomeEnd && m_bServo && m_bPackInPosn && m_bStop; //Ready.

				//
                HomeProc(iAxis);

	            if (m_bBusy && m_iStepHome != 0) m_bLtBusy = true;

                //Servo On
                m_tServoWait.OnDelay(m_bReqServoOn, 2000);

                if(m_tServoWait.Out)
                {
                    m_bReqServoOn = false;
                    m_bServo = true;
                    Stop(iAxis);
                    m_tServoWait.Clear();
                }
                //m_tRingCounter.OnDelay(m_bReqRingCounter, 1000);
                //if(m_tRingCounter.Out)
                //{
                //    m_bReqRingCounter = false;
                //}

                //if(m_iMotorKind == (int)EN_MOTR_KIND.ABS || m_iHomeType == (int)EN_HOME_TYPE.DataSet) {
                //    m_tHomeEnd.OnDelay(m_bServo && !m_bHomeEnd , 500);
                //    if(m_tHomeEnd.Out) m_bHomeEnd = true;
                //    }

                //Alarm Reset
                m_tAlarmReset.OnDelay(m_bReqResetAlarm, 1000);
                if(m_tAlarmReset.Out)
                {
                    m_bReqResetAlarm = false;
					m_SimulAxis.SimulReset();
                    m_bAlarm = false;
                }
                dScanTime = cDEF.TICK._GetTickTime() - dStrtTime;
            }
            catch (Exception e)
            {
                cDEF.LOG.ExceptionTrace("Axis Simulation Update", e);
            }
        }
	}
}
