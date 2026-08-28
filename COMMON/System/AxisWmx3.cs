using System;
using System.IO;
using WMX3ApiCLR;

namespace eMachine
{

    /***************************************************************************/
    /* Class: TAxisComi                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class TAxisWmx3
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
        int m_iMoveHomeSensor;
        int m_iDeviceCnt     ;
        int m_iTotalAxis     ;
        int m_iStopCnt       ;


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public String m_sParamPath     ;
        public int    m_iMotorType     ;
        public int    m_iMotorKind     ;
        public double m_dCoef          ;
        public int    m_iSONLevel      ;
        public int    m_iEndLimitEnable;

        //Vars. - Home
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   m_bKeepHomeProc ;
        public double m_dHomeVel      ;
        public double m_dHomeAcc      ;
        public double m_dHomeDec      ;
        public double m_dHomeOffset   ;
        public double m_dHomeOffsetPos;

        //Var. - Update
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   m_bApplyScurve;
        public double m_dCommandPos ;
        public double m_dEncoderPos ;

        public double m_dOrgCmdPos  ;
        public double m_dOrgEncPos  ;


        public bool   m_bServo      ;
        public bool   m_bHome       ;
        public bool   m_bHomeDone   ;
        public bool   m_bStop       ;
        public bool   m_bReady      ;
        public bool   m_bBusy       ;
        public bool   m_bHomeEnd    ;
        public bool   m_bPackInPosn ;
        public bool   m_bAlarm      ;
        public int    m_iAlarmCode  ;
        public bool   m_bCW         ;
        public bool   m_bCCW        ;
        public bool   m_bZP         ;
        public bool   m_bLtBusy     ;
        public bool   m_bRing       ;
        public double m_dTorque     ;
        public double m_dPreTrgPos  ;
        public double m_dTrgPos     ;
        public double m_dAbsOffset  ;
        public int    m_iStepHome   ;
        public string m_sWmx3Path   ;
        public int    m_iHomeType   ;
        public int    m_iOPStat     ;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //-------------------------------------------------------------------
        //get/Set Functions.
        public bool   gServo      () { return m_bServo;      }
        public bool   gHome       () { return m_bHome;       }
        public bool   gHomeDone   () { return m_bHomeDone;   }
        public bool   gStop       () { return m_bStop;       }
        public bool   gReady      () { return m_bReady;      }
        public bool   gBusy       () { return m_bBusy;       }
        public bool   gHomeEnd    () { return m_bHomeEnd;    }
        public bool   gPackInPosn () { return m_bPackInPosn; }
        public bool   gAlarm      () { return m_bAlarm;      }
        public int    gAlarmCode  () { return m_iAlarmCode;  }
        public bool   gCW         () { return m_bCW;         }
        public bool   gCCW        () { return m_bCCW;        }
        public bool   gZP         () { return m_bZP;         }
        public bool   gLtBusy     () { return m_bLtBusy;     }
        public bool   gRing       () { return m_bRing;       }
        public double gTorque     () { return m_dTorque;     }
        public double gPreTrgPos  () { return m_dPreTrgPos;  }
        public double gTrgPos     () { return m_dTrgPos;     }
        public int    gHomeStep   () { return m_iStepHome;   }
        public int    gOPStat     () { return m_iOPStat;     }
        public void   sPreTrgPos  (double bSet) { m_dPreTrgPos = bSet; }
        public void   sTrgPos     (double bSet) { m_dTrgPos    = bSet; }
        public void   sHomeEnd    (bool bSet  ) { m_bHomeEnd   = bSet; }

        
        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~        
        WMX3Api           Wmx3         = new WMX3Api          (); // When all the devices are done, the WMX3 engine will also terminate.	        
        DevicesInfo       Wmx3DevInfo  = new DevicesInfo      (); // Get DevicesInfo to determine the type of device currently created
        CoreMotionStatus  CmStatus     = new CoreMotionStatus ();
        CoreMotion        Wmx3Lib_cm                            ; 
        
        //
        Config.SystemParam systemParam = new Config.SystemParam();


        Motion.JogCommand JogCmd      = new Motion.JogCommand();
        Motion.PosCommand PosCmd      = new Motion.PosCommand();
        Motion.PosCommand PrePosCmd   = new Motion.PosCommand();

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TAxisWmx3()
        {
        }
        ~TAxisWmx3() { }

        //Base Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void Init()
        {
            m_bServo      = false;
            m_bHome       = false;
            m_bStop       = false;
            m_bReady      = false;
            m_bBusy       = false;
            m_bHomeEnd    = false;
            m_bPackInPosn = false;
            m_bAlarm      = false;
            m_bCW         = false;
            m_bCCW        = false;
            m_bZP         = false;
            m_bLtBusy     = false;
            m_dTorque     = 0.0  ;
            m_bRing       = false;
            m_iAlarmCode  = 0    ;
            m_iOPStat     = 0    ;
            m_bHomeDone   = false;

            m_iStepHome     = 0;
            m_bKeepHomeProc = false;
            m_dHomeVel      = 0.0;
            m_dHomeAcc      = 0.0;
            m_dHomeDec      = 0.0;
            m_dHomeOffset   = 0.0;
        }

        public void  SetComPort (string sPort)
        {

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool Open()
        {
            //Local Var.
                        
            //Check Already Init.
            if (m_iDeviceCnt > 0) return true; //이미 Initial 했으므로 

            //Initialize Class
            Init();

            try 
            {
                m_sWmx3Path = @"C:\Program Files\SoftServo\WMX3";
                if (!Directory.Exists(m_sWmx3Path))
                {
                    MsgBox.Error("[WMX3 Motion] Directory Fail!!!");
                    return false;
                }

                // Create device.
                int ret = Wmx3.CreateDevice(m_sWmx3Path, DeviceType.DeviceTypeNormal, 0xFFFFFFFF);
                if(ret != ErrorCode.None)
                {
                    MsgBox.Error("[WMX3 Motion] Create Device Fail!!!");
                    return false;
                }

                // Set Device Name.
                Wmx3.SetDeviceName("device");
                
                // Get created device state.
                Wmx3.GetAllDevices(ref Wmx3DevInfo);                
                m_iDeviceCnt = Convert.ToInt32(Wmx3DevInfo.Count);
                if(m_iDeviceCnt<=0) 
                {
                    MsgBox.Error("[Soft Motion] Cann't Load Device (Load Axis is Zero!)");
                    return false;
                }
                
                //
                Wmx3Lib_cm = new CoreMotion(Wmx3);

                //4. Parameter Read & Write
                //SystemParam에는 모든 축에 대한 Parameter들이 있습니다.
                Wmx3Lib_cm.Config.GetParam(ref systemParam);

                WriteParam("");

                //Start Communication.
                Wmx3.StartCommunication(0xFFFFFFFF);
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxisWmx3. Open " + ex.ToString());
            }
            //Return.
            return (m_iTotalAxis > 0);
        }
        //-------------------------------------------------------------------------
        public void Close()
        {
            try
            { 
                if (m_iDeviceCnt <= 0) return;
                //
                
                // Stop Communication.
                Wmx3.StopCommunication(0xFFFFFFFF);

                //Quit device.
                Wmx3.CloseDevice();
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxisWmx3. Close " + ex.ToString());
            }
        }
        //-------------------------------------------------------------------------
        public bool DevReset()
        {
            return true;
        }
        //-------------------------------------------------------------------------
        public void Reset ()
        {
            //Reset homing flag.
            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome = 0;
            }
        }
        //-------------------------------------------------------------------------
		public void ClearHomeEnd()
        {
            m_bKeepHomeProc = false;
            m_iStepHome     = 0;
            m_bHomeEnd      = false;
        }
        //-------------------------------------------------------------------------
        //Conv' Functions.
        //--------------------------------------------------------------------------
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
		//-------------------------------------------------------------------
        //Move Functions.
        public void SetServo(int iAxe, int on)
        {
            //Servo On/Off.
            Wmx3Lib_cm.AxisControl.SetServoOn(iAxe, on);
        }
        //--------------------------------------------------------------------------       
        public void SetAlarm(int iAxe, int on)
        {
            Wmx3Lib_cm.AxisControl.ClearAmpAlarm (iAxe);
            Wmx3Lib_cm.AxisControl.ClearAxisAlarm(iAxe);
        }
        //--------------------------------------------------------------------------       
        public void SetCount(int iAxe, int target , int Pulse)
        {

        }
        //--------------------------------------------------------------------------       
        public bool SetERCLogic(int iAxe, int SelLogic)
        {
            return true;
        }
        public bool SetCurrentLimit(int Axis, double PosRatio, double NegRatio)
        {
            return true;
        }
        //--------------------------------------------------------------------------       
        public bool EmrgStop(int iAxe)
        {
            //E-Stop.
            return Stop(iAxe, false, 0);
        }
        //--------------------------------------------------------------------------       
		public bool Stop(int iAxe, bool   DecStop = false , double DecTime = 0.1)
        {
            //Local Var.
            double dVel  = 0.0;
            double dDec  = 0.0;
            bool   isOk  = false;

            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome = 0;
            }

            //Stop.
            if (!DecStop || (DecTime <= 0)) isOk = Wmx3Lib_cm.Motion.ExecQuickStop(iAxe) == ErrorCode.None;
            else
            {
                dDec = (DecTime <= 0) ? (dVel * 10) : (dVel / DecTime);
                isOk = Wmx3Lib_cm.Motion.Stop(iAxe, dDec) == ErrorCode.None;
            }

            /*
                if (isOk) {
                    m_dTrgPos = GetCmdPos(iAxe);
                    }
            */
            //
            return isOk;
        }
        //--------------------------------------------------------------------------       
        public bool MoveJogP(int iAxe, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            //Check Status.
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;

            if (Vel <= 0 ) return false;
            if (Acc <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;

            double dVel = ConvVel(Vel);
            double dAcc = ConvAcc(Vel, Acc);
            double dDec = ConvAcc(Vel, Dec);
            //
            JogCmd.Profile.Type             = WMX3ApiCLR.ProfileType.Trapezoidal;
            JogCmd.Axis                     = iAxe;
            JogCmd.Profile.Velocity         = dVel;
            JogCmd.Profile.Acc              = dAcc;
            JogCmd.Profile.Dec              = dDec;

            // Rotate the motor at the specific speed.
            return Wmx3Lib_cm.Motion.StartJog(JogCmd) == ErrorCode.None;
        }
        //--------------------------------------------------------------------------       
		public bool MoveJogN(int iAxe, double Vel = 40.0, double Acc = 0.01, double Dec = 0.0)
        {
            //Move Jog.
            //Check Status.
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Vel <= 0 ) return false;
            if (Dec <= 0 ) Acc = Dec;

            double dVel = ConvVel(Vel);
            double dAcc = ConvAcc(Vel, Acc);
            double dDec = ConvAcc(Vel, Dec);
            //
            JogCmd.Profile.Type             = WMX3ApiCLR.ProfileType.Trapezoidal;
            JogCmd.Axis                     = iAxe;
            JogCmd.Profile.Velocity         = dVel;
            JogCmd.Profile.Acc              = dAcc;
            JogCmd.Profile.Dec              = dDec;

            // Rotate the motor at the specific speed.
            return Wmx3Lib_cm.Motion.StartJog(JogCmd) == ErrorCode.None;
        }
        //--------------------------------------------------------------------------       
        public bool Move(int iAxe, double Pos , double Vel = 20.0, 
                         double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0, int iSpdRatio = 0)
        {
            //Local Var.
            long   lFuncRet;
            bool   Receive ;
            double dPos    = 0.0;
            //
            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            if (Dec <= 0 ) Acc = Dec;
            //
            if(m_bRing) 
            {
                dPos = Pos - m_dEncoderPos;
                Pos = dPos;
            }
            //
            double dVel = ConvVel(Vel);
            double dAcc = ConvAcc(Vel, Acc);
            double dDec = ConvAcc(Vel, Dec);
            //
            PosCmd.Profile.Type             = m_bApplyScurve ? WMX3ApiCLR.ProfileType.SCurve : WMX3ApiCLR.ProfileType.Trapezoidal;
            PosCmd.Axis                     = iAxe;
            PosCmd.Target                   = Pos;
            PosCmd.Profile.Velocity         = dVel;
            PosCmd.Profile.Acc              = dAcc;
            PosCmd.Profile.Dec              = dDec;
            //
            if(m_bRing) lFuncRet = Wmx3Lib_cm.Motion.StartMov(PosCmd); //상대위치 이동.
            else        lFuncRet = Wmx3Lib_cm.Motion.StartPos(PosCmd); //절대위치 이동.
            //
            Receive = (lFuncRet == ErrorCode.None);
            if(Receive)
            {
            }
            //
            return Receive;
        }
        //--------------------------------------------------------------------------       
        public bool MoveOverride(int iAxe, double Pos , double Vel = 20.0, 
                                 double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0)  //abs move with mm.
        {
            return true;
        }
        public bool MoveOverrideVel(int iAxe, double Pos , double Vel , 
                                    double Acc , double Dec, double dOverridePos, double dOverrideVelocity)
        {
            return true;
        }
        public bool MoveSpline(int iAxe1, int iAxe2 , double Axe1Vel , double Axe1Acc , double Axe1Dec)
        {
            return true;
        }

        //--------------------------------------------------------------------------       
        public bool MoveHome (int iAxe, double Vel , double Acc, double Dec = 0.0, double OffsetPulse = 0.0 , double OffSetPos = 0.0)
        {
            if (m_bAlarm       ) return false;
            if (!m_bServo      ) return false;
            if (Vel <= 0       ) return false;
            if (m_bKeepHomeProc) return false;
            if (Dec <= 0       ) Acc = Dec;

            if (m_iMotorKind == (int)EN_MOTR_KIND.ABS)
            {
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
        //------------------------------------------------------------------------
        public bool SetMoveHomeForce(int iAxis, double Vel, double Acc)
        {
            return true; 
        }
        //--------------------------------------------------------------------------       
        public bool MoveHomeForce(int iAxe, double Vel , double Acc)
        {
            return true;
        }
        //--------------------------------------------------------------------------       
        public bool HomeStart(int iAxe, double Vel , double Acc, double Dec)
        {
            bool IsServo = ((m_iMotorType == (int)EN_MOTR_TYPE.Rotary) || (m_iMotorType == (int)EN_MOTR_TYPE.Linear));

            if (m_bAlarm ) return false;
            if (!m_bServo) return false;
            //
            long lFuncRet;
            double dVel = ConvVel(Vel);
            double dAcc = ConvAcc(Vel, Acc);
            double dDec = ConvAcc(Vel, Dec);

            //double lCripVel = dVel * ((IsServo) ? 0.03 : 0.03);
            //double lEscDist = (IsServo) ? m_dCoef : (m_dCoef / 5.0);

            //
            Config.HomeParam homeParam = new Config.HomeParam();
            Wmx3Lib_cm.Config.GetHomeParam(iAxe, ref homeParam);

            //Home Type
            //0 : Negative Direction HS Falling
            //1 : Current Position Home
            //2 : Negative Direction LS Falling
            //3 : Negative Direction LS Falling + Z Pulse
            switch (m_iHomeType)
            {
                default : homeParam.HomeType = Config.HomeType.CurrentPos     ; break;
                case  0 : homeParam.HomeType = Config.HomeType.HS             ; break;
                case  1 : homeParam.HomeType = Config.HomeType.CurrentPos     ; break;
                case  2 : homeParam.HomeType = Config.HomeType.LS             ; break;
                case  3 : homeParam.HomeType = Config.HomeType.LSReverseZPulse; break;
            }
            //
            homeParam.HomeDirection = Config.HomeDirection.Negative;
            homeParam.HomingVelocityFast    = dVel;
            homeParam.HomingVelocityFastAcc = dAcc;
            homeParam.HomingVelocityFastDec = dDec;
            //
            Wmx3Lib_cm.Config.SetHomeParam(iAxe, homeParam);            

            lFuncRet = Wmx3Lib_cm.Home.StartHome(iAxe);

            return (lFuncRet == ErrorCode.None);
        }
        //--------------------------------------------------------------------------       
        public bool HomeProc(int iAxe)
        {
            //Local Var.

            //Check Alarm.
            if ( m_bAlarm  ) { m_iStepHome = 0; m_bKeepHomeProc = false; return false; }
            if (!m_bServo  ) { m_iStepHome = 0; m_bKeepHomeProc = false; return false; }

             //Cycle.
             switch (m_iStepHome) {
                  case  0: m_iStepHome = 0;
                           return false;
                  case 10:
                           m_bHomeEnd      = false;
                           m_bLtBusy       = false;
                           m_iStopCnt      = 0;

                           //Motor Stop
                           Wmx3Lib_cm.Motion.ExecQuickStop(iAxe);
                           m_iStepHome++;
                           return false;

                  case 11: if(!m_bStop) return false;
                           m_iStepHome++;
                           return false;


                  case 12: //Move.
                           if(!HomeStart(iAxe, m_dHomeVel, m_dHomeAcc, m_dHomeDec))
                           {
                              m_bKeepHomeProc = false;
                              m_iStepHome=0;
                              return false;
                           }
                           m_iStepHome++;
                           return false;

                  case 13: if (m_iStopCnt++ < 500) return false;
                           m_iStepHome++;
                           return false;
                  case 14: //
                           if (m_iOPStat == (int)OperationState.Home) return false;
                           m_iStopCnt = 0;
                           m_iStepHome++;
                           return false;
                  
                case 15: //if (m_iStopCnt++ < 500) return false;
                           m_iStepHome++; 
						   return false;
                
                case 16: //
                           if (!m_bHomeDone) return false;
                           m_iStopCnt = 0;
                           m_iStepHome++;
                           return false;
                  
                  case 17: m_iStepHome++; return false;
                  case 18: m_iStepHome++; return false;
                  case 19: if (m_iStopCnt++ < 500) return false;
                           m_iStepHome++;
                           return false;
                  case 20: //
                           if (!m_bStop) return false;
                           Wmx3Lib_cm.Home.SetCommandPos (iAxe, m_dHomeOffset);
                           Wmx3Lib_cm.Home.SetFeedbackPos(iAxe, m_dHomeOffset);
                           m_dPreTrgPos = m_dHomeOffsetPos;
                           m_dTrgPos    = m_dHomeOffsetPos;
                           m_iStepHome++;
                           return false;
                  case 21: m_bHomeEnd = true;
                           m_iStepHome++;
                           return false;
                  case 22: m_iStepHome++; return false;
                  case 23: m_iStepHome++; return false;
                  case 24: m_iStepHome++; return false;
                  case 25: m_iStepHome     = 0    ;
                           return true;
                  }

             //NG.
             return false;
        }
        //--------------------------------------------------------------------------               
        //Position Functions.
        public double     GetCmdPos           (int iAxe            )    
        {//Get Command Position.
            return m_dCommandPos;           
        }
        //--------------------------------------------------------------------------       
        public double     GetEncPos           (int iAxe            ) 
        {//Get Encoder Position.
            return m_dEncoderPos;
        }     
        //--------------------------------------------------------------------------                            
        public bool       SetPos              (int iAxe, double Pos)
        {
            int Rslt1 = Wmx3Lib_cm.Home.SetFeedbackPos(iAxe, Pos);
            int Rslt2 = Wmx3Lib_cm.Home.SetCommandPos (iAxe, Pos);

            return (Rslt1 == ErrorCode.None) && (Rslt2 == ErrorCode.None);
        }
        //--------------------------------------------------------------------------       
        public bool       SetPosEncToCmd      (int iAxe)
        {
            int Rslt = Wmx3Lib_cm.Home.SetCommandPos (iAxe, m_dEncoderPos);

            return (Rslt == ErrorCode.None);
        }

        //--------------------------------------------------------------------------
        public double     GetOrgCmdPos        (int iAxis            ) { return m_dOrgCmdPos; }               //Get Command Position.
        public double     GetOrgEncPos        (int iAxis            ) { return m_dOrgEncPos; }               //Get Encoder Position.

        public int        gErrCode1           (           ) { return 0; }
        public int        gErrCode2           (           ) { return 0; }

        //--------------------------------------------------------------------------       
        public void       ClearPos            (int iAxe, double Pos = 0.0)
        {
            m_bHomeEnd = false;
            if (m_bKeepHomeProc)
            {
                m_bKeepHomeProc = false;
                m_iStepHome = 0;
                Stop(iAxe);
            }
            SetPos(iAxe, Pos);
        }
        //--------------------------------------------------------------------------       
        //Get Function
        public bool       GetStop             (int iAxe, bool ChkEnc = false , double InPos = 0.1)  
        {//Motion Done.
            return false;
        }
        //--------------------------------------------------------------------------       
        public bool       MotionDone          (int iAxe)
        {
            return true;
        }
        //--------------------------------------------------------------------------       
        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void       SetParamPath        (String Path, String CmePath = "")
        {
            m_sParamPath = Path;
            //m_sCmePath = CmePath;
        }
        //--------------------------------------------------------------------------       
        public void       SetType             (int iAxe, int    iType = 0 ,int iKind = 0 , int iNotUse = 0)
        {
            m_iMotorType = iType;
            m_iMotorKind = iKind;
        }
        //--------------------------------------------------------------------------
        public void       SetUseTorque        (int iAxis, bool bUse, string sWAddr, string sRAddr)
        {
            //m_bSetUseTorque = bUse   ;
            //m_sToqWAddr     = sWAddr ;
            //m_sToqRAddr     = sRAddr ;
        }

        //--------------------------------------------------------------------------       
		public void       SetABS              (int iAxe, int    Data1    = 0,int Data2 = 0     )
        {

        }
        //--------------------------------------------------------------------------       
		public void       SetPulseOut         (int iAxe, int    Data     = 1                        )
        {
            
        }
        //--------------------------------------------------------------------------       
        public void       SetEncInput         (int iAxe, int    Data     = 2                        ) 
        {//2:Sqr4Mode
            ;
        }
        //--------------------------------------------------------------------------       
        public void       SetSONLevel         (int iAxe, int    Data     = 0                        ) 
        {//1:Positive Level. 0:Negative Level.
            m_iSONLevel = Data;            
        }
        //--------------------------------------------------------------------------       
        public void       SetMaxSped          (int iAxe, long   Vel                                 )
        {

        }
        //--------------------------------------------------------------------------       
        public void       SetInpLevel         (int iAxe, int    Data     = 0                        )
        {
        }
        //--------------------------------------------------------------------------       
        public void       SetAlarmLevel       (int iAxe, int    Data     = 0                        )
        {
        }
        //--------------------------------------------------------------------------       
        public void       SetLimitLevel       (int iAxe, int    PosData  = 0 , int NegData     = 0  )
        {
        }
        //--------------------------------------------------------------------------       
        public void       SetHomeLevel        (int iAxe, int    Data     = 0                        )
        {
        }
        //--------------------------------------------------------------------------       
        public void       SetDirection        (int iAxe, int    Data                                )
        {

        }
        //--------------------------------------------------------------------------       
        public void       SetAutoResp         (int iAxe, int    Data                                )
        {

        }
        //--------------------------------------------------------------------------       
        public void       SetPackType         (int iAxe, int    Data                )
        {

        }
        //--------------------------------------------------------------------------       
        public void       SetHomeType         (int iAxe, int    Data                )
        {
            m_iHomeType = Data;
        }
        //--------------------------------------------------------------------------       
        public void       SetHomeOptn         (int iAxe, int    Data , int Data2    )
        {

        }
        //--------------------------------------------------------------------------
        public void       SetServoType        (int iAxe, int    Data     = 1                        )
        {

        }
        //--------------------------------------------------------------------------
        public void       SetAppScurve        (int iAxe, bool   Data     = true                     )
        {
            m_bApplyScurve = Data;
        }
        public void       SetIntpAxe          (int iAxe, int    Data     = -1                       )
        {

        }
        //--------------------------------------------------------------------------
        public void       SetPairAxe          (int iAxe, int    Data1    = -1   , int  Data2 = 1    )
        {

        }
        //--------------------------------------------------------------------------
        public void       SetCoefficient      (double  Data     = 819 )
        {
            m_dCoef = Data;
        }
        //--------------------------------------------------------------------------
        public void       SetEncPulse         (int iAxe, int    Data)
        {

        }
        //--------------------------------------------------------------------------
        public void       SetRingCounter      (int iAxe, bool bEnable, double dMaxCntr)
        {//상대 위치 이동
            //Local Var.
            
            //Set Var.
            m_bRing = bEnable;

        }
        //--------------------------------------------------------------------------
        public void       SetListedMotr       (int iAxis1 , int iAxis2)
        {

        }
        //--------------------------------------------------------------------------
        public void  	  SetMoveHomeSensor   (int iAxe, int Data = 0, int Data2 = 0) { m_iMoveHomeSensor = Data;    }
        public void       SetEndLimitEnable   (int iAxe, int Data = 0               ) { m_iEndLimitEnable = Data;    }
        public void       SetAbsOffset        (double dOffset                       ) { m_dAbsOffset      = dOffset; }
        //--------------------------------------------------------------------------
        public void       SetServoParam       (int iAxe, int iParamNo, int Data)
        {
            //생성한 SystemParam에 현재 적용된 Parameter들을 불러옵니다.
            systemParam.HomeParam[iAxe].HomeType = Config.HomeType.HSZPulse;




            //해당 축의 HomeType을 변경합니다.
            Wmx3Lib_cm.Config.SetParam(systemParam);

        }
        //------------------------------------------------------------------------
        public void WriteParam(string paramFilePath)
        {
            if (paramFilePath == "") return; 

            //사용하시는 Parameter파일(.xml)이 있다면 해당 함수를 통하여 읽어와서 적용시킬 수 있습니다.

            //변경한 Parameter를 적용합니다.
            //string paramFilePath = "";
            Wmx3Lib_cm.Config.ImportAndSetAll(paramFilePath);
        }
        //--------------------------------------------------------------------------
        //Set Trigger
        public void       SetTriggerReset     (int iAxe)
        {

        }
        //--------------------------------------------------------------------------
        public void       SetTriggerTimeLevel (int iAxe, double dPulseWidth)
        {

        }
        //--------------------------------------------------------------------------
        public void       SetTriggerPos       (int iAxe, double dPos, double dTrigWidth = 100.0)
        {

        }  
        //--------------------------------------------------------------------------     
        public bool       SetTriggerBlock     (int iAxe, double dStartPos, double dEndPos, double dPeriod, double dTrigWidth = 100.0)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerOneShot   (int iAxe, double dPulseWidth) 
        {

        }
        //--------------------------------------------------------------------------
        public bool       MoveTorque          (int iAxe, double dTorque, double Pos , double Vel = 20.0, double Acc = 0.3, double Dec = 0.0)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueP         (int iAxe, double dTorque, double dVel)
        {
            return true;
        }   
        //--------------------------------------------------------------------------    
        public bool       MoveTorqueN         (int iAxe, double dTorque, double dVel)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueStop      (int iAxe)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public bool       SetParamTorque      (int iAxe)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void       SetUseTorque        (bool bUse, int iWAddr, int iRAddr)
        {

        }
        //--------------------------------------------------------------------------
        public bool       SetTorqueLimit      (int iAxe, double dTorque)
        {
            return true;
        }
        //--------------------------------------------------------------------------
        public void Update(int iAxis)
        {
            //Local Val
            bool   bServoStat  = false;
            double dScanTime;
            double dStrtTime; 

                        //Set Buffer.
            if ((iAxis < 0) || (iAxis >= cDEF.MOTR._iNumOfMotr)) return;
                                                               
            try {
                dStrtTime = cDEF.TICK._GetTickTime(); 
                //Get Status.
                //if (iAxis == 0) Wmx3Lib_cm.GetStatus(ref CmStatus);
                Wmx3Lib_cm.GetStatus(ref CmStatus); //TEST필요...
                dScanTime = cDEF.TICK._GetTickTime() - dStrtTime; 

                m_dOrgCmdPos  =  CmStatus.AxesStatus[iAxis].PosCmd      ;
                m_dOrgEncPos  =  CmStatus.AxesStatus[iAxis].ActualPos   ;
                m_dTorque     =  CmStatus.AxesStatus[iAxis].ActualTorque;

                //Motion Status.
                m_iOPStat = (int)CmStatus.AxesStatus[iAxis].OpState;
                bServoStat = CmStatus.AxesStatus[iAxis].ServoOn;
                if (m_iMotorType == (int)EN_MOTR_TYPE.Step) m_bServo = (m_iSONLevel == 1) ? !bServoStat :  bServoStat;
                else                                        m_bServo = (m_iSONLevel == 1) ?  bServoStat : !bServoStat;
                //
                m_bHome       = CmStatus.AxesStatus[iAxis].HomeSwitch  ; //Home Sensor
                m_bStop       = CmStatus.AxesStatus[iAxis].OpState == OperationState.Stop; //Stop.
                m_bBusy       = !m_bStop                               ; //Busy.
                m_bPackInPosn = CmStatus.AxesStatus[iAxis].InPos       ; //In Position.
                m_bAlarm      = CmStatus.AxesStatus[iAxis].AmpAlarm    ; //Amp Alarm.
                m_iAlarmCode  = CmStatus.AxesStatus[iAxis].AmpAlarmCode;
                m_bCW         = CmStatus.AxesStatus[iAxis].PositiveLS  ; //CW Positive Limit
                m_bCCW        = CmStatus.AxesStatus[iAxis].NegativeLS  ; //CCW Negative Limit.
                m_bHomeDone   = CmStatus.AxesStatus[iAxis].HomeDone    ; //Home Done.

                m_bReady = !m_bCW && !m_bCCW && !m_bAlarm && m_bHomeEnd && m_bServo && m_bPackInPosn && m_bStop; //Ready.

                HomeProc(iAxis);
                if (m_bBusy && m_iStepHome!=0) m_bLtBusy = true;

                if (m_iHomeType == (int)EN_HOME_TYPE.DataSet)
                {
                    m_tHomeEnd.OnDelay(m_bServo && !m_bHomeEnd, 500);
                    if (m_tHomeEnd.Out) m_bHomeEnd = true;
                }
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxisComi. Update " + ex.ToString());
            }
        }
    }
}
