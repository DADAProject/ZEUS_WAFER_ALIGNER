using FASTECH;
using System;
using System.Net;
using System.Threading;
using WMX3ApiCLR;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TAxisEziServo                                                    */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    class TAxisEziServo
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
        int    m_iHomeType               ;
        int    m_iHomeSignal             ;
        int    m_iHomeZPhase             ;
        int    m_iGroupAxeNo             ;
        int    m_lTotalAxis              ; 
        int    m_iHomeDly                ;
        uint   m_uModuleID               ;
        bool   m_bReqServoOn             ;
        bool   m_bReqResetAlarm          ;
        bool   m_bSetUseTorque           ;

        //
        int    m_iTimeCount              ;
        bool   m_bConnected              ;
        bool   m_bInp                    ;
        bool   m_bHoming                 ;
        bool[] m_bInStat   = new bool[32];
        bool[] m_bOutStat  = new bool[32];

        string m_sMotrIP                 ;
        double m_dMaxPos                 ;
        double m_dMinPos                 ;
        int    m_ID                      ;

        const int MAX_IN  = 32;
        const int MAX_OUT = 32;


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public string m_sParamPath     ;
        public int    m_iMotorType     ;
        public int    m_iMotorKind     ;
        public int    m_iSONLevel      ;
        public int    m_iMotrKind      ;
        public double m_dCoef          ;
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
        public bool   m_bApplyScurve   ;
        public double m_dTorque        ;
        public double m_dPreTrgPos     ;
        public double m_dCmdPos        ;
        public double m_dEncPos        ;
        public double m_dTrgPos        ;
        public double m_dAbsOffset     ;
        public double m_dReqRingMaxPos ;
        public double m_dOrgCmdPos     ;
        public double m_dOrgEncPos     ;
        public int    m_iStepHome      ;
        public int    m_iHomeDir       ;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TAxisEziServo()
        {
            m_bConnected = false;

            Init();
        }
        ~TAxisEziServo() { }

        //Base Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
            m_iStepHome     =     0;
            m_lTotalAxis    =     0;        
            m_dHomeVel      =   0.0;
            m_dHomeAcc      =   0.0;
            m_dHomeDec      =   0.0;
            m_dHomeOffset   =   0.0;
            m_dAbsOffset    =   0.0;
            m_bKeepHomeProc = false;
            m_bReqServoOn   = false;
        }
        //--------------------------------------------------------------------------
        public void SetIP(string ip1, string ip2, string ip3, string ip4, int id)
        {
            m_sMotrIP = string.Format($"{ip1}.{ip2}.{ip3}.{ip4}");
            m_ID      = id; 
        }
        
        //--------------------------------------------------------------------------
        public void SetMinMax(double min, double max)
        {
            m_dMaxPos = min;
            m_dMinPos = max;
        }
        //--------------------------------------------------------------------------
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        public bool Open()
        {
            //if (m_bConnected) return true; 
            
            return Connect(m_sMotrIP, m_ID);
        }
        //--------------------------------------------------------------------------
        public void Close()
        {
            DisConnect(m_ID);
        }
        //------------------------------------------------------------------------
        public bool Connect(string IP, int BddId)
        {
            IPAddress ipaddr = null;

            if (IPAddress.TryParse(IP, out ipaddr))
            {
                DisConnect(BddId);
                bool result = EziMOTIONPlusELib.FAS_Connect(ipaddr, BddId);
                if (result)
                {
                    m_bConnected = true;

                    //Parameter Setting.
                    //int iMaxpulse = Convert.ToInt32(CalPosToPulse(m_dMaxPos));
                    //int iMinpulse = Convert.ToInt32(CalPosToPulse(m_dMinPos));

                    //SetServoParam(0,  9, iMaxpulse); //S/W Limit Plus Value
                    //SetServoParam(0, 10, iMinpulse); //S/W Limit Minus Value

                    //SetServoParam(0, 11,         0); //S/W Limit Stop Method [0 : Emergency Stop 방식으로서 즉시 정지 합니다.]
                    //SetServoParam(0, 17,         1); //Org Method
                                                     //[0 : Origin.]
                                                     //[1 : ‘Org Speed’ 값에 의해 원점 센서 지점까지 이동 후, 저속의 ‘Org Search Speed’값으로 Z-pulse 원점 복귀를 실시합니다.]
                                                     //[4 : 현재의 위치를 원점으로 설정할 때 사용됩니다.]

                    //
                    m_dTrgPos = m_dCmdPos;
                }
                else
                {
                    //MsgBox.Error("Motor Driver 연결을 실패 하였습니다.");
                    m_bConnected = false;
                }
            }
            //
            return m_bConnected;
        }
        //------------------------------------------------------------------------
        public bool DisConnect(int BdId)
        {
            EziMOTIONPlusELib.FAS_Close(BdId);

            m_bConnected = false;

            return true;
        }
        //--------------------------------------------------------------------------
        public bool DevReset()
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void Reset()
        {
            //Reset homing flag.
            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome     = 0;
            }
        }
        //---------------------------------------------------------------------------
        public void Scan()
        {
            //
        }
        //--------------------------------------------------------------------------
        public void ClearHomeEnd()
        {
            m_bKeepHomeProc = false;
            m_iStepHome     = 0;
            m_bHomeEnd      = false;
        }
        //Conversion Functions.
        //--------------------------------------------------------------------------
        public double CalPosToPulse(double Pos)
        {
            //Local Var.
            double dPulse;

            //Check Coef.
            if (m_dCoef <= 0) return 0;

            //
            dPulse = (Pos * m_dCoef);

            //Return.
            return dPulse;
        }
        //---------------------------------------------------------------------------
        public double CalPulseToPos(double Pulse)
        {
            //Local Var.
            double dPos;

            //Check Coef.
            if (m_dCoef <= 0) return 0;

            //
            dPos = ((double)Pulse / m_dCoef);

            //Return.
            return dPos;
        }
        //---------------------------------------------------------------------------
        double ConvVel(double Vel)
        {
            double lVel = m_dCoef * Vel;

	        return lVel;
        }
        //--------------------------------------------------------------------------
        double ConvAcc(double Vel, double Acc)
        {
            double lAcc = m_dCoef * Vel * (1.0 / Acc);

            return lAcc;
        }
        //---------------------------------------------------------------------------
        //get/Set Functions.
        public bool   gServo     ()            { return m_bServo;      }
        public bool   gHome      ()            { return m_bHome;       }
        public bool   gStop      ()            { return m_bStop;       }
        public bool   gReady     ()            { return m_bReady;      }
        public bool   gBusy      ()            { return m_bBusy;       }
        public bool   gHomeEnd   ()            { return m_bHomeEnd;    }
        public bool   gPackInPosn()            { return m_bPackInPosn; }
        public bool   gAlarm     ()            { return m_bAlarm;      }
        public bool   gCW        ()            { return m_bCW;         }
        public bool   gCCW       ()            { return m_bCCW;        }
        public bool   gLtBusy    ()            { return m_bLtBusy;     }
        public bool   gRing      ()            {return m_bRing;        }
		public double gTorque    ()            {return m_dTorque;      }
		public double gPreTrgPos ()            {return m_dPreTrgPos;   }
		public double gTrgPos    ()            {return m_dTrgPos;      }
		public int	  gHomeStep  ()            {return m_iStepHome;    }
        public void   sPreTrgPos (double bSet) { m_dPreTrgPos = bSet;  }
        public void   sTrgPos    (double bSet) { m_dTrgPos    = bSet;  }
        public void   sHomeEnd   (bool   bSet) { m_bHomeEnd   = bSet;  }

		//-------------------------------------------------------------------
        //Move Functions.
        public void SetServo(int iAxis, int iOn)
        {
            //Local Var.
            //int nRtn;
            
            //
            if (!m_bConnected) return;
            //if ( m_bServo    ) return;

            //Servo Off.
            if (iOn != 1)
            {
                if (!m_bServo) return;
                EziMOTIONPlusELib.FAS_EmergencyStop(iAxis);
                EziMOTIONPlusELib.FAS_ServoEnable(iAxis, iOn);
                return;
            }
            m_bReqServoOn = true;
            m_tServoWait.Clear();
        }
        //--------------------------------------------------------------------------
        public void SetAlarm(int iAxis, int On)
        {
            //CAXM.AxmSignalServoAlarmReset(iAxis, (uint)On);

            m_bReqResetAlarm = true;
        }
        //--------------------------------------------------------------------------
        public bool EmrgStop(int iAxis)
        {
            //E-Stop.
            return Stop(iAxis, false, 0);
        }
        //--------------------------------------------------------------------------
        public bool Stop(int iAxis, bool DecStop = false, double DecTime = 0.1)
        {
            //Local Var.
            int nRtn;

            //
            if (!m_bConnected) return false;
            if (!m_bServo    ) return false;

            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome = 0;
            }

            if (DecStop) nRtn = EziMOTIONPlusELib.FAS_MoveStop(iAxis);
            else         nRtn = EziMOTIONPlusELib.FAS_EmergencyStop(iAxis);

            //
            if (nRtn != EziMOTIONPlusELib.FMM_OK)
            {
                m_dTrgPos = m_dCmdPos;
            }

            //
            return nRtn == EziMOTIONPlusELib.FMM_OK;
        }
        //--------------------------------------------------------------------------
        public bool MoveJogP(int iAxis, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            //Local Var.
            int nRtn;
            uint lVelocity;
            EziMOTIONPlusELib.VELOCITY_OPTION_EX VelOpt = new EziMOTIONPlusELib.VELOCITY_OPTION_EX();

            //Check Status.
            if ( m_bAlarm    ) return false;
            if (!m_bServo    ) return false;
            if (!m_bConnected) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;
            if (Vel < 0.1) Vel = 0.1;
            
            VelOpt.BIT_USE_CUSTOMACCDEC = true;
            lVelocity = Convert.ToUInt32(ConvVel(Vel));
            VelOpt.wCustomAccDecTime = Convert.ToUInt16(Acc);

            //
            nRtn = EziMOTIONPlusELib.FAS_MoveVelocityEx(iAxis, lVelocity, 1, VelOpt);
            return nRtn == EziMOTIONPlusELib.FMM_OK;
        }
        //--------------------------------------------------------------------------
        public bool MoveJogN(int iAxis, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            //Local Var.
            int nRtn;
            uint lVelocity;
            EziMOTIONPlusELib.VELOCITY_OPTION_EX VelOpt = new EziMOTIONPlusELib.VELOCITY_OPTION_EX();

            //Check Status.
            if ( m_bAlarm    ) return false;
            if (!m_bServo    ) return false;
            if (!m_bConnected) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;
            if (Vel < 0.1) Vel = 0.1;

            VelOpt.BIT_USE_CUSTOMACCDEC = true;
            lVelocity = Convert.ToUInt32(ConvVel(Vel));
            VelOpt.wCustomAccDecTime = Convert.ToUInt16(Acc);

            //
            nRtn = EziMOTIONPlusELib.FAS_MoveVelocityEx(iAxis, lVelocity, 0, VelOpt);
            return nRtn == EziMOTIONPlusELib.FMM_OK;
        }
        //--------------------------------------------------------------------------
        public bool Move(int iAxis, double Pos, double Vel = 20.0, double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0, int iSpdRatio = 0)
        {
            //Local Var.
            int    nRtn = 0;
            int    lPosition = Convert.ToInt32(m_dCmdPos);
            uint   lVelocity;
            bool   bOk = false;
            EziMOTIONPlusELib.MOTION_OPTION_EX MoveOpt = new EziMOTIONPlusELib.MOTION_OPTION_EX();

            //Check Status.
            if ( m_bAlarm    ) return false;
            if (!m_bServo    ) return false;
            if (!m_bConnected) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;
            if (Vel < 0.1) Vel = 0.1;

            //
            if (m_bStop)
            {
                //
                MoveOpt.BIT_USE_CUSTOMACCEL = true;
                MoveOpt.BIT_USE_CUSTOMDECEL = true;
                MoveOpt.wCustomAccelTime    = Convert.ToUInt16(Acc * 1000); //Convert.ToUInt16(ConvAcc(Vel, Acc));
                MoveOpt.wCustomDecelTime    = Convert.ToUInt16(Acc * 1000); //Convert.ToUInt16(ConvAcc(Vel, Dec));
                lPosition                   = Convert.ToInt32((Pos             ));
                lVelocity                   = Convert.ToUInt32(ConvVel(Vel     ));

                //Result
                nRtn = EziMOTIONPlusELib.FAS_MoveSingleAxisAbsPos(iAxis, lPosition, lVelocity);

                //
                bOk = (nRtn == EziMOTIONPlusELib.FMM_OK);
            }

            //
            if (bOk) m_dTrgPos = lPosition;

            //
            return GetStop(iAxis, true);
        }
        //--------------------------------------------------------------------------
        public bool MoveOverride(int iAxis, double Pos, double Vel = 20.0, double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool MoveOverrideVel(int iAxis, double Pos, double Vel, double Acc, double Dec, double dOverridePos, double dOverrideVelocity)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool MoveHome(int iAxis, double Vel, double Acc, double Dec = 0.0, double OffsetPulse = 0.0, double OffSetPos = 0.0)
        {
            //Check Status.
            if ( m_bAlarm       ) return false;
            if (!m_bServo       ) return false;
            if (!m_bConnected   ) return false;
            if ( m_bKeepHomeProc) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;
            if (Vel < 0.1) Vel = 0.1;

            //
            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS || m_iHomeType == (int)EN_HOME_TYPE.DataSet) m_bHomeEnd = true;
            else
            {
                m_dHomeVel       = Vel        ;
                m_dHomeAcc       = Acc        ;
                m_dHomeDec       = Dec        ;
                m_dHomeOffset    = OffsetPulse;
                m_dHomeOffsetPos = OffSetPos  ;
                m_iStepHome      = 10         ;
                m_bKeepHomeProc  = true       ;
            }
            return true;
        }
        //--------------------------------------------------------------------------
        public bool SetMoveHomeForce(int iAxis, double Vel, double Acc)
        {
            //Check Status.
            if ( m_bAlarm       ) return false;
            if (!m_bServo       ) return false;
            if (!m_bConnected   ) return false;
            if ( m_bKeepHomeProc) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Vel < 0.1) Vel = 0.1;

            m_dHomeVel       = Vel;
            m_dHomeAcc       = Acc;
            m_dHomeDec       = Acc;
            m_dHomeOffset    = 0.0;
            m_dHomeOffsetPos = 0.0;
            m_iStepHome      = 10;
            m_bKeepHomeProc  = true;
            m_bForceHome     = true;
            return true;
        }
        //--------------------------------------------------------------------------
        public bool HomeStart(int iAxis, double Vel, double Acc, double Dec)
        {
            //Local Var.
            double dVel     = ConvVel(Vel);
            double dVelLast = dVel * 0.05 ; //원점 검색시 최종 검출 속도 (원점 검색의 정밀도 결정)
            int uHmsig = 0;

            if ( m_bAlarm) return false;
            if (!m_bServo) return false;

            SetServoParam(iAxis, 14, (int)dVel       ); //Org Speed[pps]
            SetServoParam(iAxis, 15, (int)dVelLast   ); //Org Search Speed[pps]
            SetServoParam(iAxis, 16, (int)Acc  * 1000); //Org Acc Dec Time[msec]

            if (m_iHomeSignal == 0) uHmsig = 0; //PosEndLimit
            if (m_iHomeSignal == 1) uHmsig = 2; //NegEndLimit
            if (m_iHomeSignal == 2) uHmsig = 0; //HomeSensor
            if (m_iHomeSignal == 3) uHmsig = 1; //EnconderZPhase
            if (m_iHomeSignal == 4) uHmsig = 6; //TorqueLimit

            SetServoParam(iAxis, 17, (int)uHmsig    ); //Org Method
            SetServoParam(iAxis, 18, m_iHomeDir == 0? 1 : 0); //Org Direction
           

            SetServoParam(iAxis, 19, (int)CalPosToPulse(m_dHomeOffsetPos)); //Org Position Set
            //SetServoParam(iAxis, 19, (int)m_dHomeOffset); //Org Position Set

            return true;
        }
        //--------------------------------------------------------------------------
        public bool HomeProc(int iAxis)
        {
            //Local Var.
            int nRtn = 0;

            //Check Alarm.
            if ( m_bAlarm ) { m_iStepHome = 0; m_bKeepHomeProc = false; return false; }
            if (!m_bServo ) { m_iStepHome = 0; m_bKeepHomeProc = false; return false; }

            //Cycle.
            switch (m_iStepHome)
            {
                case 0:
                    m_iStepHome = 0;
                    return false;

                case 10:
                    m_bHomeEnd   = false;
                    m_bLtBusy    = false;
                    m_iHomeDly   = 0;
                    m_bLtHomeSen = false;

                    //Stop(0, true);
                    EziMOTIONPlusELib.FAS_MoveStop(iAxis);
                    m_iStepHome++;
                    return false;

                case 11:
                    if (!m_bStop) return false;
                    m_iStepHome++;
                    return false;

                case 12:
                    if (!HomeStart(iAxis, m_dHomeVel, m_dHomeAcc, m_dHomeDec))
                    {
                        m_bKeepHomeProc = false;
                        m_iStepHome = 0;
                        return false;
                    }
                    m_iStepHome++;
                    return false;

                case 13:
                    if (m_iHomeDly > 100)
                    {
                        m_iHomeDly = 0;
                        m_iStepHome++;
                    }
                    else m_iHomeDly++;
                    return false;

                case 14:
                    nRtn = EziMOTIONPlusELib.FAS_MoveOriginSingleAxis(iAxis);
                    if (nRtn != EziMOTIONPlusELib.FMM_OK)
                    {
                        m_bKeepHomeProc = false;
                        m_iStepHome = 0;
                        return true;
                    }
                    m_iHomeDly = 0;
                    m_iStepHome++;
                    return false;

                case 15:
                    if (m_iHomeDly++ < 50 ) return false;
                    if (m_bHoming         ) return false;

                    if (!m_bForceHome)
                    {
                        //SetPos(iAxis, m_dHomeOffset);
                        //m_dPreTrgPos = m_dHomeOffsetPos;
                        //m_dTrgPos    = m_dHomeOffsetPos;
                    }
                    
                    SetPos(iAxis, 0.0);
                    m_dPreTrgPos = 0.0;
                    m_dTrgPos    = 0.0;
                    
                    m_bForceHome = false;
                    m_iHomeDly = 0;
                    m_iStepHome++;
                    return false;

                case 16:
                    //Parameter Setting.
                    //int iMaxpulse = Convert.ToInt32(CalPosToPulse(FM.dMaxPos));
                    //int iMinpulse = Convert.ToInt32(CalPosToPulse(FM.dMinPos));
                    //
                    ////Pulse Init
                    //SetServoParam(0,  9, iMaxpulse); //S/W Limit Plus Value
                    //SetServoParam(0, 10, iMinpulse); //S/W Limit Minus Value

                    m_bHomeEnd = true;

                    m_dTrgPos = m_dEncPos;
                    m_iStepHome = 0;
                    return true;
            }
            return false;
        }
        //---------------------------------------------------------------------------
        //Position Functions.
        //---------------------------------------------------------------------------
        public double GetTrgPos()
        {//Get Target Position.
            //Local Var.
            //double dTrgPos;
            
            //
            if (!m_bConnected) return 0;

            //dTrgPos = CalPulseToPos(m_dTrgPos);
            //return dTrgPos;

            return m_dTrgPos;
        }
        //---------------------------------------------------------------------------
        public double GetCmdPos(int Axis = 0)
        {//Get Command Position.
        
            //
            if (!m_bConnected) return 0;

            //double dCmdPos = CalPulseToPos(m_dCmdPos);
            //return dCmdPos;
            return m_dCmdPos;
        }
        //---------------------------------------------------------------------------
        public double GetEncPos(int Axis = 0)
        {//Get Encoder Position.
            //Local Var.
            if (!m_bConnected) return 0.0;

            //double dEncPos = 0.0;
            //
            //dEncPos = CalPulseToPos(m_dEncPos);
            //return dEncPos;

            return m_dEncPos;

        }
        //---------------------------------------------------------------------------
        public int gErrCode1() { return 0; }
        //---------------------------------------------------------------------------
        public int gErrCode2() { return 0; }
        //--------------------------------------------------------------------------                         
        public bool SetPos(int iAxis, double Pos)
        {
            //Local Var.
            int nRtn1;
            int nRtn2;
            int lPos = Convert.ToInt32(CalPosToPulse(Pos));

            if (!m_bConnected) return false;
            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS) return false;

            nRtn1 = EziMOTIONPlusELib.FAS_SetActualPos (iAxis, lPos);
            nRtn2 = EziMOTIONPlusELib.FAS_SetCommandPos(iAxis, lPos);
            return (nRtn1 == EziMOTIONPlusELib.FMM_OK) && (nRtn2 == EziMOTIONPlusELib.FMM_OK);
        }
        //--------------------------------------------------------------------------
        public bool SetPosEncToCmd(int iAxis)
        {
            //Local Var.
            int nRtn1;
            int nRtn2;
            int encPos = 0;

            if (!m_bConnected) return false;

            nRtn1 = EziMOTIONPlusELib.FAS_GetActualPos (iAxis, ref encPos);
            nRtn2 = EziMOTIONPlusELib.FAS_SetCommandPos(iAxis,     encPos);
            return (nRtn1 == EziMOTIONPlusELib.FMM_OK) && (nRtn2 == EziMOTIONPlusELib.FMM_OK);
        }
        //---------------------------------------------------------------------------
        public double GetOrgCmdPos(int iAxis) { return m_dOrgCmdPos; } //Get Command Position.
        //---------------------------------------------------------------------------
        public double GetOrgEncPos(int iAxis) { return m_dOrgEncPos; } //Get Encoder Position.
        //---------------------------------------------------------------------------
        public void ClearPos(int iAxis, double Pos = 0.0)
        {
            if (!m_bConnected) return;

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
        public bool GetStop(int iAxis, bool ChkEnc = false, double InPos = 0.1)
        {//Motion Done.
            //Local Var.
            double dTrg;
            double dCmd;
            double dEnc;

            //Get Motion Done.
            if (!m_bConnected) return true;

            //Check Stop.
            if (!ChkEnc) return m_bStop;
            else
            {
                if (!m_bHomeEnd) return m_bStop;
                dTrg = GetTrgPos();
                dCmd = GetCmdPos();
                dEnc = GetEncPos();

                if (m_bStop)
                {
                    if (Math.Abs(dCmd - dEnc) > InPos) return false;
                    if (Math.Abs(dTrg - dEnc) > InPos) return false;

                    //return
                    return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------
        public bool MotionDone(int iAxis)
        {
            return m_bStop;
        }
        //---------------------------------------------------------------------------
        //Set Functions.
        //---------------------------------------------------------------------------
        public void SetParamPath(String Path, String CmePath = "")
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetType(int iAxis, int iType = 0, int iKind = 0, int iNotUse = 0)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetABS(int iAxis, int Data1 = 0, int Data2 = 0)
        {
            //
        }
        //---------------------------------------------------------------------------
        public void SetAbsOrgOffset(int iAxis, double Data)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetPulseOut(int iAxis, int Data = 1)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetEncInput(int iAxis, int Data = 2)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetSONLevel(int iAxis, int Data = 0)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetMaxSped(int iAxis, long Vel)
        {
            /*
            Axis Max Speed : 위치 이동 명령(absolute move, incremental move)시 운전 가능한 최대 속도를 지정하여 어떠한 경우에도 이 값보다 빠른 속도로 운전되지 않도록 하며, 그 값은[pps] 단위로 설정합니다.
                             Pulse per resolution값에 따라 상한 값 범위가 달라집니다.
                             *10000인 경우 : 500,000
                             20000 인 경우 : 1,000,000
            */
            //SetServoParam(0, 1, Vel); //Axis Max Speed
        }
        //--------------------------------------------------------------------------
        public void SetInpLevel(int iAxis, int Data = 0)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetAlarmLevel(int iAxis, int Data = 0)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetLimitLevel(int iAxis, int PosData = 0, int NegData = 0)
        {
            /*
            Limit Sensor Logic : Limit 센서의 입력 상태를[ON]으로 인식하고자하는 신호의 레벨을 설정하는데 사용됩니다.
                                 ♦ 0 : 0 V(Active low 레벨)
                                 ♦ 1 : 24V(Active high 레벨)
            */
            //SetServoParam(0, 13, PosData); //Limit Sensor Logic
        }
        //--------------------------------------------------------------------------
        public void SetHomeLevel(int iAxis, int Data = 0)
        {
            /*
            Org Sensor Logic : 원점 센서의 입력을[ON] 상태로 인식하고자 하는 신호의 레벨을 설정하는데 사용됩니다.
                               ♦0 : 0 V(low 레벨)
                               ♦1 : 24V(high 레벨)
            */
            //SetServoParam(0, 21, Data); //Org Sensor Logic
        }
        //--------------------------------------------------------------------------
        public void SetDirection(int iAxis, int Data)
        {
            /*
            Motion Dir : 위치 명령에 의한 운전 시 모터의 회전 방향을 설정합니다.
                         ♦ 0 : CW 방향으로 이동합니다.
                         ♦ 1 : CCW 방향으로 이동합니다.
            */
            SetServoParam(0, 25, Data); //Motion Dir
        }
        //--------------------------------------------------------------------------
        public void SetAutoResp(int iAxis, int Data)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetPackType(int iAxis, int Data)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetHomeType(int iAxis, int Data)
        {
            //
            m_iHomeType = Data;
        }
        //--------------------------------------------------------------------------
        public void SetHomeOptn(int iAxis, int Data, int Data2)
        {
            //
            m_iHomeDir = Data2;
        }
        //--------------------------------------------------------------------------
        public void SetServoType(int iAxis, int Data = 1)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetAppScurve(int iAxis, bool Data = true)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetIntpAxe(int iAxis, int Data = -1)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetPairAxe(int lMasterAxeNo, int lSlaveAxeNo = -1, int Data2 = 1)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetCoefficient(double Data = 819)
        {
            m_dCoef = Data;
        }
        //--------------------------------------------------------------------------
        public void SetEncPulse(int iAxis, int Data)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetRingCounter(int iAxis, bool bEnable, double dMaxCntr)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetListedMotr(int iAxis1, int iAxis2)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetMoveHomeSensor(int iAxis, int Data1 = 2, int Data2 = 0)
        {
            //
            m_iHomeSignal = Data1;
            m_iHomeZPhase = Data2;

        }
        //---------------------------------------------------------------------------
        public void SetEndLimitEnable(int iAxis, int Data = 0)
        {
            //
        }
        //---------------------------------------------------------------------------
        public void SetAbsOffset(double dOffset)
        {
            m_dAbsOffset = dOffset;
        }
        //--------------------------------------------------------------------------
        public void SetServoParam(int iAxis, int iParamNo, int Data)
        {
            EziMOTIONPlusELib.FAS_SetParameter(iAxis, Convert.ToByte(iParamNo), Data);
        }
        //--------------------------------------------------------------------------
        //Set Trigger
        public void SetTriggerReset(int iAxis)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetTriggerTimeLevel(int iAxis, double dPulseWidth)
        {
            //
        }
        //--------------------------------------------------------------------------
        public void SetTriggerPos(int iAxis, double dPos, double dTrigWidth = 100.0)
        {
            //
        }
        //--------------------------------------------------------------------------      
        public bool SetTriggerBlock(int iAxis, double dStartPos, double dEndPos, double dPeriod, double dTrigWidth = 100.0)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void SetTriggerOneShot(int iAxis, double dPulseWidth)
        {
            //
        }
        //---------------------------------------------------------------------------
        public bool MoveTorque(int iAxis, int On, int Dir, double TorqueRatio, double VelRatio)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool MoveTorque(int iAxis, double dTorque, double Pos, double Vel = 20.0, double Acc = 0.3, double Dec = 0.0)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool MoveTorqueP(int iAxis, double dTorque, double dVel)
        {
            return true;
        }
        //--------------------------------------------------------------------------      
        public bool MoveTorqueN(int iAxis, double dTorque, double dVel)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool MoveTorqueStop(int iAxis)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool SetParamTorque(int iAxis)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void SetUseTorque(int iAxis, bool bUse, string sWAddr, string sRAddr)
        {
            //
        }
        //--------------------------------------------------------------------------
        public double GetTorque(int iAxis)
        {
            return 0.0;
        }
        //--------------------------------------------------------------------------
        public bool SetTorqueLimit(int iAxis, double dTorque)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void Update(int iAxis)
        {
            try
            {
                //Local Val.
                int    iCmdPos     = 0;
                int    iEncPos     = 0;
                int    iPosErr     = 0;
                int    iActVal     = 0;
                uint   uiInStat    = 0;
                uint   uiOutStat   = 0;
                uint   uiStatFlag  = 0;
                ushort usPosItemNo = 0;
                double dScanTime;
                double dStrtTime;
                int    result      = EziMOTIONPlusELib.FAS_GetAllStatus(iAxis, ref uiInStat, ref uiOutStat, ref uiStatFlag, ref iCmdPos, ref iEncPos, ref iPosErr, ref iActVal, ref usPosItemNo);

                //Return
                if (!m_bConnected) return;

                dStrtTime = cDEF.TICK._GetTickTime();

                if (result == EziMOTIONPlusELib.FMM_OK)
                {
                    m_dCmdPos = iCmdPos;
                    m_dEncPos = iEncPos;
                    
                    //
                    GetStatFlag  (uiStatFlag);
                    GetInputStat (uiInStat  );
                    GetOutputStat(uiOutStat );

                    m_iTimeCount = 0;
                }
                else
                {
                    if (result == EziMOTIONPlusELib.FMC_TIMEOUT_ERROR)
                    {
                        m_iTimeCount++;
                    }
                    if (m_iTimeCount > 3)
                    {
                        DisConnect(0);
                    }
                }

                if (m_iMotrKind == (int)EN_MOTR_KIND.ABS)
                {
                    m_dCmdPos = m_dCmdPos - m_dAbsOffset;
                    m_dEncPos = m_dEncPos - m_dAbsOffset;
                }

                //Ready
	            m_bReady = !m_bCW && !m_bCCW && !m_bAlarm && m_bHomeEnd && m_bServo && m_bPackInPosn && m_bStop;

                //Home Process
                HomeProc(iAxis);

                //Home End
                if (m_iMotorKind == (int)EN_MOTR_KIND.ABS || m_iHomeType == (int)EN_HOME_TYPE.DataSet)
                {
                    m_tHomeEnd.OnDelay(m_bServo && !m_bHomeEnd, 500);
                    if (m_tHomeEnd.Out) m_bHomeEnd = true;
                }

                //Busy
                if (m_bBusy && m_iStepHome != 0) m_bLtBusy = true;

                //Servo On
                m_tServoWait.OnDelay(m_bReqServoOn, 2000);
                if (m_tServoWait.Out)
                {
                    m_bReqServoOn = false;
                    EziMOTIONPlusELib.FAS_ServoEnable(iAxis, 1);
                    Thread.Sleep(20);
                    Stop(iAxis);
                    m_tServoWait.Clear();
                }
                
                //Alarm Reset
                m_tAlarmReset.OnDelay(m_bReqResetAlarm, 1000);
                if (m_tAlarmReset.Out)
                {
                    m_bReqResetAlarm = false;
                    EziMOTIONPlusELib.FAS_ServoAlarmReset(iAxis);
                }

                dScanTime = cDEF.TICK._GetTickTime() - dStrtTime;
            }
            catch (Exception e)
            {
                cDEF.LOG.ExceptionTrace("Axis EziServo Update", e);
            }
        }
        //------------------------------------------------------------------------
        public bool GetInputStat(uint iBuff)
        {
            for (int i = 0; i < MAX_IN; i++)
            {
                m_bInStat[i] = ((iBuff >> i) & 0x01) == 0x01;
            }
            return true;
        }
        //---------------------------------------------------------------------------
        public bool GetOutputStat(uint iBuff)
        {
            for (int i = 0; i < MAX_OUT; i++)
            {
                m_bOutStat[i] = ((iBuff >> i) & 0x01) == 0x01;
            }
            return true;
        }
        //---------------------------------------------------------------------------
        public bool GetStatFlag(uint iBuff)
        {
            m_bAlarm   = (( iBuff >>  0) & 0x01) == 0x01;
            m_bCW      = (( iBuff >>  1) & 0x01) == 0x01;
            m_bCCW     = (( iBuff >>  2) & 0x01) == 0x01;
            m_bServo   = (( iBuff >> 20) & 0x01) == 0x01;
            m_bInp     = (( iBuff >> 19) & 0x01) == 0x01;
            m_bStop    = (((iBuff >> 28) & 0x01) != 0x01) && m_bInp;
            m_bBusy    = !m_bStop;
            m_bHome    = (( iBuff >> 23) & 0x01) == 0x01;
            //m_bHomeEnd = (( iBuff >> 25) & 0x01) == 0x01;
            m_bHoming  = (( iBuff >> 18) & 0x01) == 0x01;

            //
            return true;
        }
        //--------------------------------------------------------------------------
        public bool gX(int add)
        {
            if (add > MAX_IN) return false; 

            return m_bInStat[add];
        }
        //--------------------------------------------------------------------------
        public bool gY(int add)
        {
            if (add > MAX_OUT) return false;

            return m_bOutStat[add];
        }
    }
}
