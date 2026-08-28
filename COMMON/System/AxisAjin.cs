using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace eMachine
{
    /***************************************************************************/
    /* Class: TAxisAjin                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/


    class TAxisAjin
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
        public bool   m_bReqRingCounter;
        public double m_dTorque        ;
        public double m_dPreTrgPos     ;
        public double m_dTrgPos        ;
        public double m_dAbsOffset     ;
        public double m_dReqRingMaxPos ;
        public int    m_iStepHome      ;
        public bool   m_bApplyScurve   ;
        public int    m_iHomeDir       ;

        public double m_dCmdPos;
        public double m_dEncPos;

        public double m_dOrgCmdPos;
        public double m_dOrgEncPos;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TAxisAjin()
        {
            Init();
        }
        ~TAxisAjin() { }

        //Base Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void       Init                ()
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
            if (m_lTotalAxis > 0) return true;

            try {
                // Initialize AXT board
                if (CAXL.AxlIsOpened() == 0)
                {
                    if ((CAXL.AxlOpenNoReset(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS))
                    {
                        MsgBox.Error("[AJIN Motion] AXL Open Fail");
                        return false;
                    }
                }
                //Setup File 로딩 시 필요.
                //if (CAXM.AxmMotLoadParaAll(m_sParamPath) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                //{
                //    MsgBox.Error("[AJIN Motion] Mot Load Fail");
                //    return false;
                //}

                CAXM.AxmInfoGetAxisCount(ref m_lTotalAxis);
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxisAjin. Open " + ex.ToString());
            }
            //Return.
            return (m_lTotalAxis > 0);
        }
        //--------------------------------------------------------------------------
        public void       Close               ()
        {
            if (CAXL.AxlIsOpened() == 1) CAXL.AxlClose();
        }
        //--------------------------------------------------------------------------
        public bool       DevReset            ()
        {
            uint uRslt = CAXL.AxlOpenNoReset(7);
            return uRslt == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
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
        public double ConvVel      (double Vel)
        {
	        double lVel = m_dCoef * Vel;

	        return lVel;
        }
        //--------------------------------------------------------------------------
        public double ConvAcc      (double Vel, double Acc)
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
                CAXM.AxmMoveEStop(iAxis);
                CAXM.AxmSignalServoOn(iAxis, (uint)iOn);
                return;
            }

            // Encoder type을 설정한다.
            // 기본값은 0(TYPE_INCREMENTAL)로 설정되어 있습니다.
            // 설정값은 0 ~ 1까지 설정 할 수 있습니다.
            // 설정값 : 0(TYPE_INCREMENTAL), 1(TYPE_ABSOLUTE).
            if (m_bServo) return;

            CAXDev.AxmSignalSetEncoderType(iAxis, (uint)m_iMotorKind);
            m_bReqServoOn = true;
            m_tServoWait.Clear();
        }
        //--------------------------------------------------------------------------
        public void SetAlarm(int iAxis, int On)
        {
            CAXM.AxmSignalServoAlarmReset(iAxis, (uint)On);
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
            uint  uReturn = 0;
            if (!m_bServo) return false;

            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome = 0;
            }

            //Stop.
            if (!DecStop || (DecTime <= 0)) uReturn = CAXM.AxmMoveEStop(iAxis);
            else
            {
                uReturn = CAXM.AxmMoveSStop(iAxis);
            }
            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }
        //--------------------------------------------------------------------------
        public bool       MoveJogP            (int iAxis, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            uint uReturn = 0;

            //Check Status.
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;
            if (Vel < 0.1) Vel = 0.1;

            double lVel = ConvVel(Vel);
            double lAcc = ConvAcc(Vel, Acc);
            double lDec = ConvAcc(Vel, Dec);
            

            uReturn = CAXM.AxmMoveVel(iAxis, lVel, lAcc, lDec);

            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }
        //--------------------------------------------------------------------------
		public bool       MoveJogN            (int iAxis, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            uint uReturn = 0;

            //Move Jog.
            //Check Status.
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Vel <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;

            double lVel = ConvVel(Vel);
            double lAcc = ConvAcc(Vel, Acc);
            double lDec = ConvAcc(Vel, Dec);
            //
            uReturn = CAXM.AxmMoveVel(iAxis, -lVel, lAcc, lDec);

            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }
        //--------------------------------------------------------------------------
        public bool       Move                (int iAxis, double Pos , double Vel = 20.0, 
                                                         double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0, int iSpdRatio = 0)
        {
            //Local Var.
            long lFuncRet     ;
            uint dwProfile = 0;
            //double dPos       ;

            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Dec <= 0 ) Acc = Dec;

            if (m_iMotrKind == (int)EN_MOTR_KIND.ABS) Pos = Pos + m_dAbsOffset;

            double lVel = ConvVel(Vel);
            double lAcc = ConvAcc(Vel, Acc);
            double lDec = ConvAcc(Vel, Dec);

            /*
            if(m_bRing) {
                dPos = Pos - m_dEncPos;
                Pos = dPos;
                }
            */

            CAXM.AxmMotSetAbsRelMode(iAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE);

            if (m_bApplyScurve)
            {
                CAXM.AxmMotGetProfileMode(iAxis, ref dwProfile);
                if ((dwProfile == 0) || (dwProfile == 1)) dwProfile += 3;
                else dwProfile = 3;
                CAXM.AxmMotSetProfileMode(iAxis, dwProfile);
            }

            lFuncRet = CAXM.AxmMoveStartPos(iAxis, Pos, lVel, lAcc, lDec);
            CAXM.AxmMotSetParaLoad(iAxis, Pos, lVel, lAcc, lDec);

            return (lFuncRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS);
        }
        //--------------------------------------------------------------------------
        public bool       MoveOverride        (int iAxis, double Pos , double Vel = 20.0, 
                                                         double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0)  //abs move with mm.
        {
            //Local Var.
            long lFuncRet;

            double dReadInitpos   = 0.0;
            double dReadInitvel   = 0.0;
            double dReadInitaccel = 0.0;
            double dReadInitdecel = 0.0;
            

            if (m_iMotrKind == (int)EN_MOTR_KIND.ABS) Pos = Pos + m_dAbsOffset;

            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Dec <= 0) Acc = Dec;

            //
            double lVel = ConvVel(Vel);
            double lAcc = ConvAcc(Vel, Acc);
            double lDec = ConvAcc(Vel, Dec);

            CAXM.AxmMotGetParaLoad(iAxis, ref dReadInitpos, ref dReadInitvel, ref dReadInitaccel, ref dReadInitdecel);

            if      (dReadInitpos != Pos ) lFuncRet = CAXM.AxmOverridePos(iAxis, Pos );
            else if (dReadInitvel != lVel) lFuncRet = CAXM.AxmOverrideVel(iAxis, lVel);
            else return true;

            if (lFuncRet != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;
            return true;
        }

        //--------------------------------------------------------------------------
        public bool       MoveOverrideVel     (int iAxis, double Pos , double Vel , 
                                                          double Acc , double Dec, double dOverridePos, double dOverrideVelocity)
        {
            //lTarget - 속도를 변경할 위치 소스
            //COMMAND - 00H
            //ACTUAL  - 01H

            //Local Var.
            long lFuncRet;

            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Dec <= 0 ) Acc = Dec;

            if (m_iMotrKind == (int)EN_MOTR_KIND.ABS)
            {
                Pos = Pos + m_dAbsOffset;
                dOverridePos = dOverridePos + m_dAbsOffset;
            }

            double lVel = ConvVel(Vel);
            double lAcc = ConvAcc(Vel, Acc);
            double lDec = ConvAcc(Vel, Dec);
            double lOverrideVelocity = ConvVel(dOverrideVelocity);

            CAXM.AxmOverrideSetMaxVel(iAxis, lOverrideVelocity);
            lFuncRet = CAXM.AxmOverrideVelAtPos(iAxis, Pos, lVel, lAcc, lDec, dOverridePos, lOverrideVelocity, (int)AXT_MOTION_SELECTION.COMMAND);
            return lFuncRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }

        //--------------------------------------------------------------------------
        public bool       MoveHome            (int iAxis, double Vel , double Acc, double Dec = 0.0, double OffsetPulse = 0.0 , double OffSetPos = 0.0)
        {
            bool isECAT = m_uModuleID == 0x00 || m_uModuleID == 0xE1;

            if (m_bAlarm       ) return false;
            if (!m_bServo      ) return false;
            if (Vel <= 0       ) return false;
            if (m_bKeepHomeProc) return false;
            if (Dec <= 0       ) Acc = Dec;

            if (isECAT) 
            {
                m_dHomeVel         = Vel        ;
                m_dHomeAcc         = Acc        ;
                m_dHomeDec         = Acc        ;
                m_dHomeOffset      = OffsetPulse;
                m_dHomeOffsetPos   = OffSetPos  ;
                m_iStepHome        = 10         ;
                m_bKeepHomeProc    = true       ;
                m_bForceHome       = false      ;
            }
            else if (m_iMotorKind == (int)EN_MOTR_KIND.ABS || m_iHomeType == (int)EN_HOME_TYPE.DataSet)
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
                m_bKeepHomeProc  = true;
            }
            return true;
        }
        //--------------------------------------------------------------------------
        public bool      SetMoveHomeForce       (int iAxis, double Vel , double Acc)
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
        public bool HomeStart           (int iAxis, double Vel , double Acc, double Dec)
        {
            uint AxmResult;
            bool IsServo = ((m_iMotorType == (int)EN_MOTR_TYPE.Rotary) || (m_iMotorType == (int)EN_MOTR_TYPE.Linear));
            bool isECAT  =   m_uModuleID == 0x00 || m_uModuleID == 0xE1;
            

            if (m_bAlarm ) return false;
            if (!m_bServo) return false;

            double dVel = ConvVel(Vel);
            double lAcc = ConvAcc(Vel, Acc);
            double lDec = ConvAcc(Vel, Dec);


            double dVelFirst  = ConvVel(Vel)           ;    //원점 검색시 초기 검출 속도 (원점 센서가 감지 안되어 있을 경우)
            double dVelSecond = dVelFirst * 0.5        ;    //원점 검색 후 반대 방향으로 빠져  나오는 속도
            double dVelThird  = dVelFirst * 0.05       ;    //1차 센서 검출 후 재 센서 검색 위한 속도
            double dVelLast   = dVelFirst * 0.01       ;    //원점 검색시 최종 검출 속도 (원점 검색의 정밀도 결정)
            double dAccFirst  = ConvAcc(Vel, Acc) * 2.0;    //원점 검색시 초기 고속 검출 가속도
            double dAccSecond = dAccFirst * 2.0        ;    //1차 센서 검색 후 반대 방향으로 빠져 나오는 가속도

            uint   uHmsig   = 4            ; //원점 신호에 사용할 센서 : PosEndLimit(0), NegEndLimit(1) , HomeSensor(4)
            int    iHmDir   = 0            ; //원점 검출시 초기 진행 방향 ( 1: +방향, 0: - 방향)
            uint   uZphas   = 0            ; //원점 센서의 Z상 검출 여부 (1: 사용함 , 0: 사용안함)
            double dHClrTim = 1000.0       ; //원점 검색 후 Enc Set하기 위한 대기 시간
            double dHOffset = m_dHomeOffset; //원점 검색 후 Offset Set 값

            //
            if (m_iHomeSignal == 0) uHmsig = 0 ; //PosEndLimit
            if (m_iHomeSignal == 1) uHmsig = 1 ; //NegEndLimit
            if (m_iHomeSignal == 2) uHmsig = 4 ; //HomeSensor
            if (m_iHomeSignal == 3) uHmsig = 5 ; //EnconderZPhase
            if (m_iHomeSignal == 4) uHmsig = 16; //TorqueLimit

            uZphas  = (uint)m_iHomeZPhase;
            iHmDir  = m_iHomeDir         ;   

            CAXM.AxmHomeSetVel   (iAxis, dVelFirst, dVelSecond, dVelThird, dVelLast, dAccFirst, dAccSecond);
            if(!isECAT)
            {
                CAXM.AxmHomeSetMethod(iAxis, iHmDir, uHmsig, uZphas, dHClrTim, dHOffset);
            }
            else {
                //EtherCat
                //Ref Mode
                //0  Positive
                //1  Negative
                //2  Gantry,Pos,Same Linear
                //3  Gantry,Neg,Same Linear
                //4  Gantry,Pos,Contrary Linear
                //5  Gantry,Neg,Contrary Linear
                //6  CW->ContinueCCW
                //7  CW->ContinueCW
                //8  CCW->ContinueCCW
                //10 CCW->ContinueCW
                //11 CCW->ShortestCCW
                //12 CCW->ShortestCW

                // Change Homing method(For XENAX REF mode)
                // lHmDir       : Don't care
                // uHomeSig     : 100 + REF Mode
                // uZphas       : Don't care
                // dHomeClrTime : Don't care
                // dHomeOffset  : Don't care
                //uHmsig = 1;
                CAXM.AxmHomeSetMethod(iAxis, iHmDir, uHmsig, 0, 0.0, dHOffset);

            }
            AxmResult = CAXM.AxmHomeSetStart(iAxis);
            return (AxmResult == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS);
        }
        //--------------------------------------------------------------------------
        public bool       HomeProc            (int iAxis)
        {
            //Local Var.
            uint uHomeResult = 0;
            bool isECAT      = m_uModuleID == 0x00 || m_uModuleID == 0xE1;
            double dEnc1, dEnc2;

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
				           CAXM.AxmMoveSStop(iAxis);
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
                           m_iHomeDly = 0;
                           m_iStepHome++;
                           return false;
                  case 13: //if (StopCnt++ < 100) return false;
                           //if (m_iHomeDly++ < 500) return false;
                           CAXM.AxmHomeGetResult(iAxis, ref uHomeResult);
                           if (uHomeResult != (int)AXT_MOTION_HOME_RESULT.HOME_SEARCHING) return false;
                           m_iHomeDly = 0;
                           m_iStepHome++;
				           return false;

                  case 14: //if (StopCnt++ < 100) return false;
                           if (m_iHomeDly++ < 500) return false;
                           CAXM.AxmHomeGetResult(iAxis, ref uHomeResult);
                           if (uHomeResult == (int)AXT_MOTION_HOME_RESULT.HOME_SEARCHING) return false;
                           m_iHomeDly = 0;
                           m_iStepHome++;
				           return false;

                  case 15: //CAXM.AxmHomeGetRate(iAxe, &uHomeMainStepNo, &uHomeStepNo_0);
                           //if (uHomeStepNo_0 < 100) return false;
                           if (m_iHomeDly++ < 500) return false;
                           CAXM.AxmHomeGetResult(iAxis, ref uHomeResult);
                           if (uHomeResult != (int)AXT_MOTION_HOME_RESULT.HOME_SUCCESS) return false;
                           m_iHomeDly = 0;
                           m_iStepHome++;
                           return false;
                  case 16: m_iStepHome++; return false;
                  case 17: m_iStepHome++; return false;
                  case 18: if (m_bBusy) return false;
                           if (m_iHomeDly++ < 1500) return false;
                           if (!m_bForceHome) 
                           {
                              //SetPos(iAxis, m_dHomeOffset);
                              //m_dPreTrgPos = m_dHomeOffsetPos;
                              //m_dTrgPos    = m_dHomeOffsetPos;
                           }

                           dEnc1        = m_dEncPos;
                           SetPos(iAxis, 0.0);
                           m_dPreTrgPos = 0.0;
                           m_dTrgPos    = 0.0;
                           dEnc2        = m_dEncPos;
                           m_bForceHome = false;
                           m_iHomeDly   = 0;
                           m_iStepHome++;
                           return false;

                  case 19: if (m_iHomeDly++ < 100) return false;
                           m_bHomeEnd = true;
				           m_iStepHome++;
                           return false;
                  case 20:
                           m_iStepHome  = 0 ;
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
            double cmdPos = 0.0;

            if (m_bRing)
            {
                CAXM.AxmStatusGetCmdPos(iAxis, ref cmdPos);
                m_dCmdPos = cmdPos;
            }
            return m_dCmdPos;         
        }
        //--------------------------------------------------------------------------
        public double     GetEncPos           (int iAxis            ) 
        {//Get Encoder Position.

            bool isStepMotr = (m_iMotorType == (int)EN_MOTR_TYPE.StepOriental) ||
                              (m_iMotorType == (int)EN_MOTR_TYPE.Step);
            return (isStepMotr) ? m_dCmdPos : m_dEncPos;
        } 

        public int        gErrCode1           (           ) { return 0; }
        public int        gErrCode2           (           ) { return 0; }

        //--------------------------------------------------------------------------                         
        public bool       SetPos              (int iAxis, double Pos)
        {
            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS) return false;

            CAXM.AxmStatusSetActPos(iAxis, Pos);
            CAXM.AxmStatusSetCmdPos(iAxis, Pos);

            return true;
        }
        //--------------------------------------------------------------------------
        public bool       SetPosEncToCmd      (int iAxis)
        {
            //Local Var.
            double encPos = 0.0;
            CAXM.AxmStatusGetActPos(iAxis, ref encPos);
            CAXM.AxmStatusSetCmdPos(iAxis,     encPos);
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
            uint iBusy = 1;
            CAXM.AxmContiIsMotion(CoodiNo, ref iBusy);
            return (iBusy == 0);
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
            int  iBoardNo   = 0;
            int  iMidulePos = 0;
            uint uModuleID  = 0; 

            m_iMotorType    = iType;
            m_iMotorKind    = iKind;
            CAXM.AxmInfoGetAxis(iAxis, ref iBoardNo, ref iMidulePos, ref uModuleID);

            //[06h] PCI-N804/404, 4 Axis
            //[07h] RTEX A4N slave, 1 Axis
            //[09h] RTEX PM slave, 1 Axis
            //[0Ah] Mechatrolink-II SGDV slave, 1 Axis
            //[0Bh] Mechatrolink-II JEPMC-PL2910, 2 Axis
            m_uModuleID = uModuleID;
            //통신 연결 안되어 있으면 값 안넘어와서 0x00 추가
            bool isECAT = m_uModuleID == 0x00 || m_uModuleID == 0xE1; 

            if(!isECAT)
                CAXM.AxmStatusSetReadServoLoadRatio(iAxis, 2); //부하율 읽어오는 함수
        }
        //--------------------------------------------------------------------------
		public void       SetABS              (int iAxis, int    Data1    = 0,int Data2 = 0     )
        {
            m_iMotrKind = Data1;
        }
        public void       SetAbsOrgOffset     (int iAxis, double Data)
        {
            //지정 축의 절대치 엔코더 원점 Offset 위치를 설정한다 절대치 엔코더 사용 시에만 함수 사용이 가능
            //하며 , Firmware 버전이 v1.3 이상 되어야 사용이 가능하다 현재 사용 중인 Firmware 버전이 v1.3 이하
            //라면 SoftwarePackage 에 포함되어 있는 최신 버전의 Firmware 로 Update 하여 사용해야 한다

            uint   uFuncRet;
            double dOrgOffsetPos = Data;

            uFuncRet = CAXM.AxmStatusSetAbsOrgOffset(iAxis , dOrgOffsetPos);
        }
        //--------------------------------------------------------------------------
		public void       SetPulseOut         (int iAxis, int    Data     = 1                        )
        {
            //[00h]1펄스 방식, PULSE(Active High), 정방향(DIR=Low)/ 역방향(DIR=High)
            //[01h]1펄스 방식, PULSE(Active High), 정방향(DIR=High) / 역방향(DIR=Low)
            //[02h]1펄스 방식, PULSE(Active Low), 정방향(DIR=Low) / 역방향(DIR=High)
            //[03h]1펄스 방식, PULSE(Active Low), 정방향(DIR=High) / 역방향(DIR=Low)
            //[04h]2펄스 방식, PULSE(CCW:역방향), DIR(CW:정방향), Active High
            //[05h]2펄스 방식, PULSE(CCW:역방향), DIR(CW:정방향), Active Low
            //[06h]2펄스 방식, PULSE(CW:정방향), DIR(CCW:역방향), Active High
            //[07h]2펄스 방식, PULSE(CW:정방향), DIR(CCW:역방향), Active Low
            //[08h]2상(90' 위상차), PULSE lead DIR(CW:정방향), PULSE lag DIR(CCW:역방향)
            //[09h]2상(90' 위상차), PULSE lead DIR(CCW:정방향), PULSE lag DIR(CW:역방향)
            CAXM.AxmMotSetPulseOutMethod(iAxis, (uint)Data);
        }
        //--------------------------------------------------------------------------
        public void       SetEncInput         (int iAxis, int    Data     = 2                        ) 
        {//2:Sqr4Mode
            //[00h]정방향 Up/Down
            //[01h]정방향 1체배
            //[02h]정방향 2체배
            //[03h]정방향 4체배
            //[04h]역방향 Up/Down
            //[05h]역방향 1체배
            //[06h]역방향 2체배
            //[07h]역방향 4체배
            CAXM.AxmMotSetEncInputMethod(iAxis, (uint)Data);
        }
        //--------------------------------------------------------------------------
        public void       SetSONLevel         (int iAxis, int    Data     = 0                        ) 
        {//1:Positive Level. 0:Negative Level.
            m_iSONLevel = Data;
            //PCI-N804, N404, RTEX-PM만 유효
            //00H : B접점
            //01H : A접점
            uint uReturn = 0;

            bool isECAT = m_uModuleID == 0x00 || m_uModuleID == 0xE1;
            if(isECAT) return;
            //if(m_uModuleID == 0x0C) return;

            uReturn = CAXM.AxmSignalSetServoOnLevel(iAxis, (uint)Data);
        }
        //--------------------------------------------------------------------------
        public void       SetMaxSped          (int iAxis, long   Vel                                 )
        {
            CAXM.AxmMotSetMaxVel(iAxis, Vel);
        }
        //--------------------------------------------------------------------------
        public void       SetInpLevel         (int iAxis, int    Data     = 0                        )
        {
            //[00h]B 접점(NORMAL CLOSE)
            //[01h]A 접점(NORMAL OPEN)
            //[02h]사용안함
            //[03h] Active level을 유지하고 사용하도록 설정(현상태 유지)
            if (Data == 0) Data = 2; //B접점일 경우 사용안함으로 변경
            CAXM.AxmSignalSetInpos(iAxis, (uint)Data);
        }
        //--------------------------------------------------------------------------
        public void       SetAlarmLevel       (int iAxis, int    Data     = 0                        )
        {
            //[00h]B 접점(NORMAL CLOSE)
            //[01h]A 접점(NORMAL OPEN)

            //bool isECAT = m_uModuleID == 0x00 || m_uModuleID == 0xE1;
            //if(isECAT) return;

            uint rst = CAXM.AxmSignalSetServoAlarm(iAxis, (uint)Data);
        }
        //--------------------------------------------------------------------------
        public void       SetLimitLevel       (int iAxis, int    PosData  = 0 , int NegData = 0  )
        {
            uint uPositiveLevel = (uint)PosData;
            uint uNegativeLevel = (uint)NegData;
            uint uStopMode      = 0;
            uint uRslt = 0;
            //upStopMode       - 리미트 센서 감지 후 정지모드 설정값
            //dwPositiveLevel  - (+) 리미트 센서 Level
            //  [00h]B 접점(NORMAL CLOSE)
            //  [01h]A 접점(NORMAL OPEN)
            //  [02h]사용안함
            //  [03h] Active level을 유지하고 사용하도록 설정(현상태 유지)

            //dwNegativeLevel  - (-) 리미트 센서 Level
            //  [00h]B 접점(NORMAL CLOSE)
            //  [01h]A 접점(NORMAL OPEN)
            //  [02h]사용안함
            //  [03h] Active level을 유지하고 사용하도록 설정(현상태 유지)

            //P-Limit Level
            CAXM.AxmSignalGetLimit(iAxis, ref uStopMode, ref uPositiveLevel, ref uNegativeLevel);
            //
            uRslt = CAXM.AxmSignalSetLimit(iAxis, uStopMode, (uint)PosData,  (uint)NegData);
        }
        //--------------------------------------------------------------------------
        public void       SetHomeLevel        (int iAxis, int    Data     = 0)
        {
            //[00h]B 접점(NORMAL CLOSE)
            //[01h]A 접점(NORMAL OPEN)

            //CAXM.AxmSignalSetZphaseLevel(m_iAxis, Data); //PCI-N804/404 Board에서 사용, Z상은 엔코더의 제로 펄스
            m_iSetHomeLevel = Data;
            CAXM.AxmHomeSetSignalLevel(iAxis, (uint)Data);
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
            uint   uGantryUse        = 0;
            uint   uSlaveHomeUse     = 0;  //0: Master 축만 원점 검색, 1:Master, Slave 둘다 원점 검색
            double dSlaveOffset      = 0;  //Master, Slave 축의 기구적 Offset 값
            double dSlaveOffsetRange = 10; //Master, Slave 축의 오차 한계
            uint   uFuncRet;
   
            if (lMasterAxeNo < 0 || lMasterAxeNo >= cDEF.MOTR._iNumOfMotr) return;
            if (lSlaveAxeNo  < 0 || lSlaveAxeNo  >= cDEF.MOTR._iNumOfMotr) return;

            m_iGroupAxeNo = lSlaveAxeNo;

            CAXM.AxmGantryGetEnable(lMasterAxeNo, ref uSlaveHomeUse, ref dSlaveOffset, ref dSlaveOffsetRange, ref uGantryUse);

            if (Data2 == 1)
            {
                if (uGantryUse == 0)
                {
                    uFuncRet = CAXM.AxmGantrySetEnable(lMasterAxeNo, lSlaveAxeNo, uSlaveHomeUse, dSlaveOffset, dSlaveOffsetRange);
                }
            }
            else
            {
                if (uGantryUse == 1)
                {
                    uFuncRet = CAXM.AxmGantrySetDisable(lMasterAxeNo, lSlaveAxeNo);
                }
            }
        }
        //--------------------------------------------------------------------------
        public void       SetCoefficient      (double  Data     = 819                               )
        {
            m_dCoef = Data;
        }
        //--------------------------------------------------------------------------
        public void       SetEncPulse         (int iAxis, int    Data                                )
        {

        }
        //--------------------------------------------------------------------------
        public void       SetRingCounter      (int iAxis, bool bEnable, double dMaxCntr  )
        {
            //Local Var.
            
            //Set Var.
            m_bRing = bEnable;

            if (!bEnable)
            {
                CAXM.AxmStatusSetPosType(iAxis, 0, dMaxCntr, 0.0);
                return;
            }

            //현재 축의 정보를 0으로 설정
            CAXM.AxmStatusSetActPos(iAxis, 0);
            CAXM.AxmStatusSetCmdPos(iAxis, 0);

            m_bReqRingCounter = true;
            m_dReqRingMaxPos = dMaxCntr;
        }

        //--------------------------------------------------------------------------
        public void       SetListedMotr       (int iAxis1 , int iAxis2                              )
        {

        }
        //--------------------------------------------------------------------------
        public void  	  SetMoveHomeSensor   (int iAxis, int Data1 = 2, int Data2 = 0                ) 
        {
            m_iHomeSignal = Data1;
            m_iHomeZPhase = Data2;
        }

        public void       SetEndLimitEnable   (int iAxis, int Data = 0) {  }
        public void       SetAbsOffset        (double     dOffset     ) {m_dAbsOffset       = dOffset;}
        //--------------------------------------------------------------------------
        public void       SetServoParam       (int iAxis, int iParamNo, int Data)
        {
            uint uParam = 0x5881;
            uint uSize  = 4;
            uint uMode  = 0x11;
            if(m_uModuleID == 0x06) return;

            //축번호
            //파라메터 번호
            //파라메터 Byte사이즈
            //쓰기모드             - 00h : 공통 파라메터 RAM영역, 01h: 공통파라메터 비휘발성, 10h:서보 파라메터 RAM영역, 11h:서보 파라메터 비휘발성
            //데이타 배열

            CAXM.AxmM3ServoSetParameter(iAxis, (uint)iParamNo, uSize, uMode, ref uParam);
        }
        //--------------------------------------------------------------------------
        //Set Trigger
        public void       SetTriggerReset     (int iAxis)
        {
            CAXM.AxmTriggerSetReset(iAxis);
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerTimeLevel (int iAxis, double dPulseWidth)
        {
            //dPulseWidth : Trigger Time (1 = 1usec ) ~  최대 (40000 = 4msec) 까지
            uint uTriggerLevel = (uint)AXT_MOTION_LEVEL_MODE.HIGH  ; //LOW(0), HIGH(1), UNUSED(2), USED(3)
            uint uSelect       = (uint)AXT_MOTION_SELECTION.COMMAND; //Command Pos(0), Actual Pos(1)
            uint uInterrupt    = (uint)AXT_USE.ENABLE              ; //Trigger 출력시 Interrupt 출력 여부 DISABLE(0), ENABLE(1)

            if (dPulseWidth < 0) dPulseWidth = 100; //0.1 ms

            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS) return;

            CAXM.AxmTriggerSetTimeLevel(iAxis, dPulseWidth, uTriggerLevel, uSelect, uInterrupt);
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerPos       (int iAxis, double dPos, double dTrigWidth = 100.0)
        {
            double[] dTrigPos = new double[1];
            int      iTrigNum = 1;

            dTrigPos[0] = dPos;

            if(m_iMotorKind == (int)EN_MOTR_KIND.ABS) return;
            //SetTriggerReset     (iAxis);
            SetTriggerTimeLevel      (iAxis, dTrigWidth        );
            CAXM.AxmTriggerOnlyAbs   (iAxis, iTrigNum, dTrigPos);
        } 
        //--------------------------------------------------------------------------      
        public bool       SetTriggerBlock     (int iAxis, double dStartPos, double dEndPos, double dPeriod, double dTrigWidth = 100.0)
        {
            uint uReturn = 0;
            
            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS) return false;
            SetTriggerReset    (iAxis);
            SetTriggerTimeLevel(iAxis, dTrigWidth);

            uReturn = CAXM.AxmTriggerSetBlock(iAxis, dStartPos, dEndPos, dPeriod);

            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerOneShot   (int iAxis, double dPulseWidth) 
        {
            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS) return;
            SetTriggerReset       (iAxis             );
            SetTriggerTimeLevel   (iAxis, dPulseWidth);
            CAXM.AxmTriggerOneShot(iAxis             );
        }
		public bool		  MoveTorque          (int iAxis, int On, int Dir, double TorqueRatio, double VelRatio)
		{
			//Local Var.
			//long   bIsIgnored         ;
			long   lFuncRet           ;
			bool   Receive            ;

			if ( m_bAlarm) return false;
			if (!m_bServo) return false;

			//
			if (Dir == vDEF.drCW) TorqueRatio *=  1;
			else                  TorqueRatio *= -1;

			//
			if (On == 1) {
			    lFuncRet = CAXM.AxmMoveStartTorque(iAxis, TorqueRatio, VelRatio, 0, 0, 0);
			    }
			else {
			    lFuncRet = CAXM.AxmMoveTorqueStop(iAxis, 0);
			    }

			//
			Receive  = (lFuncRet == 0);

			return Receive;
		}
        //--------------------------------------------------------------------------
        public bool       MoveTorque          (int iAxis, double dTorque, double Pos , double Vel = 20.0, double Acc = 0.3, double Dec = 0.0)
        {
            uint uReturn = 0;

            //Check Status.
            if (m_bAlarm           ) return false;
            if (!m_bServo          ) return false;
            if (m_uModuleID == 0x06) return false;  //PCI-N804,404

            if (Vel <= 0           ) return false;
            if (Acc <= 0           ) return false;
            if (Dec <= 0           ) Acc = Dec;
            if (Vel < 0.1          ) Vel = 0.1;

            uint uVel   = (uint)ConvVel(Vel);
            uint uAcc   = (uint)ConvAcc(Vel, Acc);
            uint uDec   = (uint)ConvAcc(Vel, Dec);
            uint uTQREF = (uint)(dTorque * 100);//Parameter 에서 0.01로 설정

            uReturn = CAXM.AxmM3ServoPosing(iAxis, (uint)Pos, uVel, uAcc, uDec, uTQREF);
            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueP         (int iAxis, double dTorque, double dVel)
        {
            uint uReturn = 0;
            uint uVLIM =  (uint)ConvVel(dVel);
            int  iTQREF = (int)(dTorque * 100); //Parameter 에서 0.01로 설정

            //Check Status.
            if (m_bAlarm           ) return false;
            if (!m_bServo          ) return false;
            if (m_uModuleID == 0x06) return false;  //PCI-N804,404

            if (dTorque <= 0       ) return false;
            if (dVel <= 0          ) return false;

            //dTorque : 최대 출력 토크에 대한 %값 (구동방향은 dTorque 값이 양수이면 CW, 음수이면 CCW 로 구동 한다.)
            //dVel 최대 모터 구동 속도에 대한 %값
            //가속 모드 설정 : [00h] LINEAR_ACCDCEL, [01h] EXPO_ACCELDCEL , [02h] SCURVE_ACCELDECEL
            //Gain 설정 :  [00h] GAIN_1ST, [01h] GAIN_2ND
            //제어 Loop 설정 :  [00h] PI_LOOP , [01h] P_LOOP
            //return CAXM.AxmMoveStartTorque(iAxe, dTorque, dVel, 0, 0, 0);

            //초기화 할때 한번만... 
            //CAXM.AxmStatusSetReadServoLoadRatio(iAxe,2); //부하율 읽어오는 함수
            uReturn = CAXM.AxmM3ServoTrqctrl(iAxis, uVLIM, iTQREF);
            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        } 
        //--------------------------------------------------------------------------      
        public bool       MoveTorqueN         (int iAxis, double dTorque, double dVel)
        {
            uint uReturn = 0;
            uint uVLIM = (uint)ConvVel(dVel);
            int iTQREF = (int)(dTorque * 100); //Parameter 에서 0.01로 설정


            //Check Status.
            if (m_bAlarm           ) return false;
            if (!m_bServo          ) return false;
            if (m_uModuleID == 0x06) return false;  //PCI-N804,404

            if (dTorque <= 0       ) return false;
            if (dVel <= 0          ) return false;

            //dTorque : 최대 출력 토크에 대한 %값 (구동방향은 dTorque 값이 양수이면 CW, 음수이면 CCW 로 구동 한다.)
            //dVel 최대 모터 구동 속도에 대한 %값
            //가속 모드 설정 : [00h] LINEAR_ACCDCEL, [01h] EXPO_ACCELDCEL , [02h] SCURVE_ACCELDECEL
            //Gain 설정 :  [00h] GAIN_1ST, [01h] GAIN_2ND
            //제어 Loop 설정 :  [00h] PI_LOOP , [01h] P_LOOP
            //return CAXM.AxmMoveStartTorque(iAxe, dTorque, dVel, 0, 0, 0);
            //CAXM.AxmStatusSetReadServoLoadRatio(iAxe,2);

            uReturn = CAXM.AxmM3ServoTrqctrl(iAxis, uVLIM, -iTQREF);
            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS; ;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueStop      (int iAxis)
        {
            uint uReturn = 0;            

            if (m_uModuleID == 0x06) return false;  //PCI-N804,404
            //정지 모드 설정
            //[00h] 1ST 또는 2ND의 일정한 리니어 감속률에 따른 정지
            //[01h] 즉시 정지
            //[02h] 정지를 위한 일정한 리니어 감속률에 따른 정지

            // 모터의 토크 출력을 낮추어 정지시킴.
            CAXM.AxmM3ServoTrqctrl(iAxis, 10, 5);
            uReturn = CAXM.AxmMoveEStop(iAxis);
            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }
        //--------------------------------------------------------------------------
        public bool       SetParamTorque      (int iAxis)
        {
	        //COMMON_PARAMETER_RAM				= 0x00,
	        //COMMON_PARAMETER_RETENTIVE_MEMORY	= 0x01,
	        //DEVICE_PARAMETER_RAM				= 0x10,
	        //DEVICE_PARAMETER_RETENTIVE_MEMORY	= 0x11

        /*
	        주의 사항:
	        - 비휘발성 메모리 쓰기 횟수 제한이 있어, 여러 번 쓰기 동작은 하지 말아야 합니다.
	        - 해당 파라미터는 변경 되었을 경우만 실행하며, 반드시 서보 앰프 전원을 껐다가 켜야 적용됩니다.
        */
	        uint   uNo            = 0;
	        uint   uSize          = 0;
	        uint   uMode          = 0x01;
	        uint   uToruqUnit     = 0;
            uint   uToruqBaseUnit = 0;
	        uint   uReturn        = 0;
            string sTemp          ;
                       
            uNo                   = 0x47;	// Torque Unit, 1H Percentage(%) of rated torque
            uSize                 = 4;
	        uToruqUnit            = 1;
	        // common para
            uReturn = CAXM.AxmM3ServoSetParameter(iAxis, uNo, uSize, uMode, ref uToruqUnit);
	        Thread.Sleep(50);
	        if (uReturn != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
	        {
                sTemp = string.Format("Set Torque Parameter Error (AxmM3ServoSetParameterError code:{0})", uReturn);
                MsgBox.Error(sTemp);
                return false;
	        }

        ///////////////////////////////////////////////////////////////////////////////////////
            uNo            = 0x48;	// Torque Base Unit
            uSize          = 4;
            uToruqBaseUnit = 2;

            uReturn = CAXM.AxmM3ServoSetParameter(iAxis, uNo, uSize, uMode, ref uToruqBaseUnit);
	        if (uReturn != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
	        {
                sTemp = string.Format("Set Torque Parameter Error (AxmM3ServoSetParameterError code:{0})", uReturn);
                MsgBox.Error(sTemp);
                return false;
	        }
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
            int    iData    = 0;
            byte[] btTQREF  = new byte[4];
            bool   isECAT   = m_uModuleID == 0x00 || m_uModuleID == 0xE1;
            uint   iDataLen = 0;
            ushort usToqAddr= 0;

            if (!m_bSetUseTorque    )  return dTroque;
            if (m_sToqRAddr == null )  return dTroque;
            

            if(isECAT)
            { 
                if(m_sToqRAddr == "") return dTroque;
                //if(!Int32.TryParse(m_sToqRAddr, out iData)) return dTroque;
                //CAXM.AxmStatusReadServoLoadRatio(iAxis, ref m_dTorque);
                //세팅은 설정 토크 % * 10 , 맥스 6000
                //POS 6077 - 24695
                //NET 6078 
                usToqAddr = (ushort)Convert.ToInt32(m_sToqRAddr, 16);
                CAXDev.AxlECatReadSdo(1, usToqAddr, 0, ref btTQREF[0], 4, ref iDataLen);     
           
                iData =BitConverter.ToInt16(btTQREF,0);
                dTroque = iData / 10.0;

            }
            else
            {
                CAXM.AxmStatusReadServoLoadRatio(iAxis, ref dTroque);
            }

            return dTroque;
        }
        //--------------------------------------------------------------------------
        public bool       SetTorqueLimit      (int iAxis, double dTorque)
        {
            uint   uReturn  = 0;
            uint   uTQREF   = 0;
            int    iData    = 0;
            ushort usToqAddr= 0;
            byte[] btTQREF = new byte[4];

            //Check Status.
            if (m_bAlarm           ) return false;
            if (m_uModuleID == 0x06) return false;  //PCI-N804,404
            if (dTorque < 0        ) return false;

            bool isECAT = m_uModuleID == 0x00 || m_uModuleID == 0xE1;
            if(isECAT) {
                //byteValue = dTorque * 0.32;   //세팅값을 g으로 설정
                //dwReturn = AxlECatWriteSdoFromAxisByte(iAxe, 0x6073, 0, &byValue);

                usToqAddr = (ushort)Convert.ToInt32(m_sToqWAddr, 16);

                //세팅은 설정 토크 % * 10 , 맥스 6000 
                //POS 60E0 - 24800
                //NET 60E1 

                iData      = (int)(dTorque * 10);
                btTQREF[0] = Convert.ToByte(iData       & 0xFF); //Press Set Data LO
                btTQREF[1] = Convert.ToByte(iData >> 8  & 0xFF); //Press Set Data HI
                btTQREF[2] = 0x00;
                btTQREF[3] = 0x00;
                usToqAddr = (ushort)Convert.ToInt32(m_sToqWAddr, 16);
                uReturn   = CAXDev.AxlECatWriteSdo(1, usToqAddr, 0, ref btTQREF[0], 2);
            }
            else 
            {
                uTQREF = (uint)(dTorque * 100); //Parameter 에서 0.01로 설정
                uReturn = CAXM.AxmM3ServoSetTorqLimit(iAxis, uTQREF);
            }
            return uReturn == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        }

        //--------------------------------------------------------------------------
        public void Update(int iAxis)
        {
            try
            {
                //Local Val
                MOTION_INFO MotionInfo = new MOTION_INFO();
                bool        isECAT     = m_uModuleID == 0x00 || m_uModuleID == 0xE1;

                uint        uDriveStatus;
                uint        uMechaSig   ;
                uint        uUIN, uUOUT ;
                double      dEncPos     ;
                double      dScanTime   ;
                double      dStrtTime   ; 
                bool        isServoOn      = false;
                uint        uStopMode      = 0;
                uint        uPositiveLevel = 0;
                uint        uNegativeLevel = 0;

                MotionInfo.uMask = 0x1F;
                if ((iAxis < 0) || (iAxis >= cDEF.MOTR._iNumOfMotr)) return;

                dStrtTime = cDEF.TICK._GetTickTime(); 

                CAXM.AxmStatusReadMotionInfo(iAxis, ref MotionInfo);
                CAXM.AxmSignalGetLimit(iAxis, ref uStopMode, ref uPositiveLevel, ref uNegativeLevel);
                m_dTorque = GetTorque(iAxis);

                dEncPos       = MotionInfo.dActPos ;
                m_dCmdPos     = MotionInfo.dCmdPos ;
                m_dEncPos     = dEncPos            ;
                uMechaSig     = MotionInfo.uMechSig;
                uDriveStatus  = MotionInfo.uDrvStat;
                uUIN          = MotionInfo.uInput  ;
                uUOUT         = MotionInfo.uOutput ;

                //m_bServo      =  ((uUOUT     >> 0  ) & 0x01 ) == 0x01; //Servo
                isServoOn     = ((uUOUT     >> 0  ) & 0x01 ) == 0x01;
                if (m_iMotorType == (int)EN_MOTR_TYPE.StepOriental) m_bServo = (m_iSONLevel == 1) ? isServoOn : !isServoOn ;
                else                                                m_bServo = (m_iSONLevel == 1) ? isServoOn : !isServoOn ;
                //m_bServo  = isServoOn;

                //Motion Status.
	            m_bHome       =  ((uMechaSig >> 7  ) & 0x01 ) == 0x01; //Home Sensor
                m_bBusy       =  ((uDriveStatus      & 0x01)) == 0x01; //Busy.
	            m_bPackInPosn =  ((uMechaSig >> 5  ) & 0x01 ) == 0x01; //In Position.
	            m_bAlarm      =  ((uMechaSig >> 4  ) & 0x01 ) == 0x01; //Alarm.
	            m_bCW         =  (uPositiveLevel == 2) ? false : ((uMechaSig >> 0  ) & 0x01 ) == 0x01; //CW.  //221019 수정
                m_bCCW        =  (uNegativeLevel == 2) ? false : ((uMechaSig >> 1  ) & 0x01 ) == 0x01; //CCW. //221019 수정
                if(m_iMotorType == 2) m_bPackInPosn = true;
	            m_bStop       =  !m_bBusy && m_bPackInPosn; //Stop.

                m_dOrgCmdPos  =  MotionInfo.dCmdPos;
                m_dOrgEncPos  =  dEncPos           ;

                if (m_iMotrKind == (int)EN_MOTR_KIND.ABS)
                {
                    m_dCmdPos = m_dCmdPos - m_dAbsOffset;
                    m_dEncPos = m_dEncPos - m_dAbsOffset;
                }

	            m_bReady      =  !m_bCW && !m_bCCW && !m_bAlarm && m_bHomeEnd && m_bServo && m_bPackInPosn && m_bStop; //Ready.
                HomeProc(iAxis);

	            if (m_bBusy && m_iStepHome != 0) m_bLtBusy = true;

                //Servo On
                m_tServoWait.OnDelay(m_bReqServoOn, 2000);

                if(m_tServoWait.Out)
                {
                    m_bReqServoOn = false;
                    CAXM.AxmSignalServoOn(iAxis, 1);
                    Thread.Sleep(20);
                    Stop(iAxis);
                    m_tServoWait.Clear();
                }
                m_tRingCounter.OnDelay(m_bReqRingCounter, 1000);
                if(m_tRingCounter.Out)
                {
                    CAXM.AxmStatusSetPosType(iAxis, 1, m_dReqRingMaxPos, 0.0); // 0 : Default, 1: Ring Counter
                    m_bReqRingCounter = false;
                }

                if(m_iMotorKind == (int)EN_MOTR_KIND.ABS || m_iHomeType == (int)EN_HOME_TYPE.DataSet) {
                    m_tHomeEnd.OnDelay(m_bServo && !m_bHomeEnd , 500);
                    if(m_tHomeEnd.Out) m_bHomeEnd = true;
                    }

                //Alarm Reset
                m_tAlarmReset.OnDelay(m_bReqResetAlarm, 1000);
                if(m_tAlarmReset.Out)
                {
                    m_bReqResetAlarm = false;
                    CAXM.AxmSignalServoAlarmReset(iAxis, 0);
                }
                dScanTime = cDEF.TICK._GetTickTime() - dStrtTime;
            }
            catch (Exception e)
            {
                cDEF.LOG.ExceptionTrace("Axis AJIN Update", e);
            }
        }
    }
}
