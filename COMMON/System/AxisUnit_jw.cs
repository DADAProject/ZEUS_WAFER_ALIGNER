using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    /***************************************************************************/
    /* Constants                                                               */
    /***************************************************************************/
    public enum EN_MOTR_MAKER  : int
    {
        NONE    ,
        COMI    ,
        AJIN    ,
        AJECAT  ,
        EZSTEP  ,
        HABER   ,
        ELMO    ,
        WMX3    ,
		SIMUL   ,
        EZISERVO,
		EndOfId
    }

    //Motor Type.
    //===========================================================================
    public enum EN_MOTR_TYPE  : int
    {
        Rotary      ,
        Linear      ,
        Step        ,
        StepOriental,
        Induct      ,
        ACS         ,
        VCM         ,
        Etc
    }

    //Motor Kind.
    //===========================================================================
    enum EN_MOTR_KIND  : int
    {
        INC,
        ABS
    }

    //Motion Command ID.
    //===========================================================================
        //Common.
    public enum EN_COMD_ID  : int 
    {
        NoneCmd    = -1   ,
        User1      = 0    , //User defined position as Motor Characteristics.
        User2      = 1    ,
        User3      = 2    ,
        User4      = 3    ,
        User5      = 4    ,
        User6      = 5    ,
        User7      = 6    ,
        User8      = 7    ,
        User9      = 8    ,
        User10     = 9    ,
        User11     = 10   ,
        User12     = 11   ,
        User13     = 12   ,
        User14     = 13   ,
        User15     = 14   ,
        User16     = 15   ,
        User17     = 16   ,
        User18     = 17   ,
        User19     = 18   ,
        User20     = 19   ,
        User21     = 20   ,
        User22     = 21   ,
        User23     = 22   ,
        User24     = 23   ,
        User25     = 24   ,
        User26     = 25   ,
        User27     = 26   ,
        User28     = 27   ,
        User29     = 28   ,
        User30     = 29   ,
        Stop       = 30   , //Basis command.
        EStop      = 31   ,
        Home       = 32   ,
        JogP       = 33   ,
        JogN       = 34   ,
        RPMMove    = 35   ,
        Wait1      = 36   ,
        Wait2      = 37   ,
        Wait3      = 38   ,
        FindStep1  = 39   , //Step Command.
        FindStepF1 = 40   ,
        FindStepB1 = 41   ,
        OneStepF1  = 42   ,
        OneStepB1  = 43   ,
        FindStep2  = 44   ,
        FindStepF2 = 45   ,
        FindStepB2 = 46   ,
        OneStepF2  = 47   ,
        OneStepB2  = 48   ,
        FindStep3  = 49   ,
        FindStepF3 = 50   ,
        FindStepB3 = 51   ,
        OneStepF3  = 52   ,
        OneStepB3  = 53   ,
        FindStep4  = 54   ,
        FindStepF4 = 55   ,
        FindStepB4 = 56   ,
        OneStepF4  = 57   ,
        OneStepB4  = 58   ,
        UserPitchP = 59   ,
        UserPitchN = 60   ,
        FSP1_1     = 61   , //Step1 - Index1
        FSP1_2     = 62   , //        - fpIndex2
        FSP1_3     = 63   , //        - fpIndex3
        FSP1_4     = 64   , //        - fpIndex4
        FSP1_5     = 65   , //        - fpIndex5
        FSP1_6     = 66   , //        - fpIndex6
        FSP1_7     = 67   , //        - fpIndex7
        FSP1_8     = 68   , //        - fpIndex8
        FSP1_9     = 69   , //        - fpIndex9
        FSP2_1     = 70   , //Step2 - Index1
        FSP2_2     = 71   , //        - fpIndex2
        FSP2_3     = 72   , //        - fpIndex3
        FSP2_4     = 73   , //        - fpIndex4
        FSP2_5     = 74   , //        - fpIndex5
        FSP2_6     = 75   , //        - fpIndex6
        FSP2_7     = 76   , //        - fpIndex7
        FSP2_8     = 77   , //        - fpIndex8
        FSP2_9     = 78   , //        - fpIndex9


        FSP3_1     = 79   , //Step3 - Index1
        FSP3_2     = 80   , //        - fpIndex2
        FSP3_3     = 81   , //        - fpIndex3
        FSP3_4     = 82   , //        - fpIndex4
        FSP3_5     = 83   , //        - fpIndex5
        FSP3_6     = 84   , //        - fpIndex6
        FSP3_7     = 85   , //        - fpIndex7
        FSP3_8     = 86   , //        - fpIndex8
        FSP3_9     = 87   , //        - fpIndex9

        FSP4_1     = 88   , //Step4 - Index1
        FSP4_2     = 89   , //        - fpIndex2
        FSP4_3     = 90   , //        - fpIndex3
        FSP4_4     = 91   , //        - fpIndex4
        FSP4_5     = 92   , //        - fpIndex5
        FSP4_6     = 93   , //        - fpIndex6
        FSP4_7     = 94   , //        - fpIndex7
        FSP4_8     = 95   , //        - fpIndex8
        FSP4_9     = 96   , //        - fpIndex9

        LSP1_1     = 97   ,
        LSP1_2     = 98   ,
        LSP1_3     = 99   ,
        LSP1_4     = 100  ,
        LSP1_5     = 101  ,
        LSP1_6     = 102  ,
        LSP1_7     = 103  ,
        LSP1_8     = 104  ,
        LSP1_9     = 105  ,

        LSP2_1     = 106  ,
        LSP2_2     = 107  ,
        LSP2_3     = 108  ,
        LSP2_4     = 109  ,
        LSP2_5     = 110  ,
        LSP2_6     = 111  ,
        LSP2_7     = 112  ,
        LSP2_8     = 113  ,
        LSP2_9     = 114  ,

        LSP3_1     = 115  ,
        LSP3_2     = 116  ,
        LSP3_3     = 117  ,
        LSP3_4     = 118  ,
        LSP3_5     = 119  ,
        LSP3_6     = 120  ,
        LSP3_7     = 121  ,
        LSP3_8     = 122  ,
        LSP3_9     = 123  ,

        LSP4_1     = 124  ,
        LSP4_2     = 125  ,
        LSP4_3     = 126  ,
        LSP4_4     = 127  ,
        LSP4_5     = 128  ,
        LSP4_6     = 129  ,
        LSP4_7     = 130  ,
        LSP4_8     = 131  ,
        LSP4_9     = 132  ,

        Zero       = 133  , //To Zero.
        Direct     = 134  , //To Direct.
        Pick1      = 135  , //Tool Z-Axis.
        Pick2      = 136  ,
        Plce1      = 137  ,
        Plce2      = 138  ,
        PickInc    = 139  ,
        PlceInc    = 140  ,
        Sply       = 141  ,
        Stck       = 142  ,
        CalPos     = 143

    };

    //Motor Speed ID.
    //===========================================================================
        //Motor Velocity ID.
    public enum  EN_MOTR_VEL  : int 
    {
        Normal = -1,
        Home       ,  //0
        Work       ,  //1
        Dry        ,  //2
        HJog       ,  //3  (5 ~ 9 UserDefined)
        LJog       ,  //4  (5 ~ 9 UserDefined)
        User1      ,
        User2      ,
        User3      ,
        User4      ,
        User5

    };

    //Motor Position ID.
    //===========================================================================
        //Common.
    public enum  EN_POSN_ID  : int 
    {
        None      = -1   ,
        User1     = 0    , //User defined position as Motor Characteristics.
        User2     = 1    ,
        User3     = 2    ,
        User4     = 3    ,
        User5     = 4    ,
        User6     = 5    ,
        User7     = 6    ,
        User8     = 7    ,
        User9     = 8    ,
        User10    = 9    ,
        User11    = 10   ,
        User12    = 11   ,
        User13    = 12   ,
        User14    = 13   ,
        User15    = 14   ,
        User16    = 15   ,
        User17    = 16   ,
        User18    = 17   ,
        User19    = 18   ,
        User20    = 19   ,
        User21    = 20   ,
        User22    = 21   ,
        User23    = 22   ,
        User24    = 23   ,
        User25    = 24   ,
        User26    = 25   ,
        User27    = 26   ,
        User28    = 27   ,
        User29    = 28   ,
        User30    = 29   ,
        Wait1     = 30   , //Basis Position.
        Wait2     = 31   ,
        Wait3     = 32   ,
        IntpPos   = 33   , //
        FSP1_1    = 34   , //Step1 - Index1
        FSP1_2    = 35   , //        - fpIndex2
        FSP1_3    = 36   , //        - fpIndex3
        FSP1_4    = 37   , //        - fpIndex4
        FSP1_5    = 38   , //        - fpIndex5
        FSP1_6    = 39   , //        - fpIndex6
        FSP1_7    = 40   , //        - fpIndex7
        FSP1_8    = 41   , //        - fpIndex8
        FSP1_9    = 42   , //        - fpIndex9
        FSP2_1    = 43   , //Step2 - Index1
        FSP2_2    = 44   , //        - fpIndex2
        FSP2_3    = 45   , //        - fpIndex3
        FSP2_4    = 46   , //        - fpIndex4
        FSP2_5    = 47   , //        - fpIndex5
        FSP2_6    = 48   , //        - fpIndex6
        FSP2_7    = 49   , //        - fpIndex7
        FSP2_8    = 50   , //        - fpIndex8
        FSP2_9    = 51   , //        - fpIndex9
        FSP3_1    = 52   , //Step3 - Index1
        FSP3_2    = 53   , //        - fpIndex2
        FSP3_3    = 54   , //        - fpIndex3
        FSP3_4    = 55   , //        - fpIndex4
        FSP3_5    = 56   , //        - fpIndex5
        FSP3_6    = 57   , //        - fpIndex6
        FSP3_7    = 58   , //        - fpIndex7
        FSP3_8    = 59   , //        - fpIndex8
        FSP3_9    = 60   , //        - fpIndex9
        FSP4_1    = 61   , //Step4 - Index1
        FSP4_2    = 62   , //        - fpIndex2
        FSP4_3    = 63   , //        - fpIndex3
        FSP4_4    = 64   , //        - fpIndex4
        FSP4_5    = 65   , //        - fpIndex5
        FSP4_6    = 66   , //        - fpIndex6
        FSP4_7    = 67   , //        - fpIndex7
        FSP4_8    = 68   , //        - fpIndex8
        FSP4_9    = 69   , //        - fpIndex9
        Sply      = 70   ,
        Stck      = 71   ,
        CalPos    = 72   , //Calculated Position.
        UserPitch = 73   , //User PITCH
        InPos     = 74   , //InPosition.
        Pick1     = 75   , //Tool Z-Axis.
        Pick2     = 76   ,
        Plce1     = 77   ,
        Plce2     = 78   ,
        PickInc   = 79   ,  //Pick-Up Auto Increasing Command.
        PlceInc   = 80   ,
        FindOff1  = 81   ,
        FindOff2  = 82   ,
        FindInc1  = 83   ,
        FindInc2  = 84
    }

    //Direction.
    //===========================================================================
    public enum  EN_SERVO_DIR  : int 
    {
        Cw  ,
        Ccw
    }

    //ABS Motor
    //===========================================================================
    public enum  EN_ABS_USE  : int 
    {
        NotUse,
        Use
    }

    //Auto Tunning Response.
    //===========================================================================
    public enum  EN_AUT_RESP  : int 
    {
        Normal ,
        High   ,
        Low
    }

    //Limit Usage.
    //===========================================================================
    public enum  EN_LIMIT_USAGE  : int 
    {
        Use  ,
        NoUse,
        UseP ,
        UseN
    }

    //Linear Pack Type.
    //===========================================================================
    public enum  EN_PACK_TYPE  : int 
    {
        Normal   ,
        RJ004_12M,
        RJ004_24M,
        RJ004_48M
    }

    //Home Type.
    //===========================================================================
    public enum  EN_HOME_TYPE:int 
    {
        Normal        = 0  ,
        DataSet       = 1  ,

        elRLS_F_INDEX = 11 , //RLS Falling + Index             1 + 10
        elFLS_F_INDEX = 12 , //FLS Falling + Index             2
        elRLS_F       = 27 , //RLS Falling                    17
        elFLS_F       = 28 , //FLS Falling                    18
        elNEG_HOME_F  = 31 , //Home Neg Side, Falling Edgy    21
        elNEG_HOME_R  = 32 , //Home Neg Side, Rising Edgy     22
        elDATA_SET    = 45   //Current Position Home(DataSet) 35
    }

    //Home Option.
    //===========================================================================
    public enum  EN_HOME_OPT  : int 
    {
        optUse    ,
        optNoUse
    }

    //Home Option.
    //===========================================================================
    public enum  EN_ENCPULSE  : int 
    {
        pulse4000 ,
        pulse65535
    }

    //Motor Speed Mode
    //===========================================================================
    public enum  EN_SPED_MODE  : int 
    {
        Work,
        Dry
    }

    //Motor FindStep Type
    //===========================================================================
    public enum  EN_FSTEP_INDEX  : int 
    {
        NONE  = -100 ,
        StepA =    0 ,
        Step1 =    1 , //FSP1_x
        Step2 =    2 , //FSP2_x
        Step3 =    3 , //FSP3_x
        Step4 =    4   //FSP4_x
    }
    //---------------------------------------------------------------------------
    public enum  EN_FPOSN_INDEX  : int 
    {
        NONE   = -100 ,
        Index1 =    0 , //FSPx_1
        Index2        , //FSPx_2
        Index3        , //FSPx_3
        Index4        , //FSPx_4
        Index5        , //FSPx_5
        Index6        , //FSPx_6
        Index7        , //FSPx_7
        Index8        , //FSPx_8
        Index9          //FSPx_9
    }

    //MOTOR SPEED RATIO ID.
    //===========================================================================
    public enum  EN_SPED_RATIO  : int 
    {
        sr100 ,
        sr90  ,
        sr80  ,
        sr70  ,
        sr60  ,
        sr50  ,
        sr40  ,
        sr30  ,
        sr20  ,
        sr10
    }
    //---------------------------------------------------------------------------
    enum EN_MOTR_DELAY  : int
    {
        TimeOut, //0                                                                        
        Stop     //1  (6 ~ 9 UserDefined)
    }

    /***************************************************************************/
    /* Structures & Variables                                                  */
    /***************************************************************************/

    public struct _TDstbPosn
    {
        public double dPos ;
        public string sName;
        public string sDesc; 
        public string sUnit; 
    };

    /***************************************************************************/
    /* Class: TMOTN_PARA                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TMOTN_PARA {
        public int    iType;
        public bool   bRing;
        public double dPulse;
        public double dPitch;

        public double[] dPosn     = new double[vDEF.MAX_POSN];
        public double[] dVel      = new double[vDEF.MAX_SPED];
        public double[] dAcc      = new double[vDEF.MAX_SPED];
        public double[] dDec      = new double[vDEF.MAX_SPED];
        public double[] dTime     = new double[vDEF.MAX_DLAY];
        public string[] sPosn_Desc= new string[vDEF.MAX_POSN];
        public int   [] iPosnKind = new int   [vDEF.MAX_POSN]; 

        public double MaxPos;
        public double MaxVel;
        public double MaxAcc;
        public double MinPos;
        public double MinVel;
        public double MinAcc;

        public double[] dArea1 = new double[20];
        public double[] dArea2 = new double[20];

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TMOTN_PARA()
        {
        }
        ~TMOTN_PARA() { }
    };

    /***************************************************************************/
    /* delegate                                                                */
    /***************************************************************************/

	delegate 	void   dgfInit          ();
    delegate    bool   dgfDevReset      ();
    delegate    void   dgfClose         ();
    delegate    void   dgfReset         ();
    delegate    void   dgfUpdateAxe     (int iAxis);
    delegate    bool   dgfOpen          ();
    delegate    void   dgfClearHomeEnd  ();
    delegate    void   dgfSetServo      (int iAxis, int iOn                        );
    delegate    void   dgfSetAlarm      (int iAxis, int iOn                        );
    delegate    bool   dgfEmrgStop      (int iAxis                                 );
    delegate    bool   dgfStop          (int iAxis, bool   DecStop , double DecTime);
    delegate    bool   dgfMoveJogP      (int iAxis, double Vel, double Acc, double Dec = 0.0);
    delegate    bool   dgfMoveJogN      (int iAxis, double Vel, double Acc, double Dec = 0.0);
    delegate    bool   dgfMove          (int iAxis, double Pos , double Vel = 20.0, 
                                                    double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0, int iSpdRatio = 0);
    delegate    bool   dgfMoveOverride  (int iAxis, double Pos , double Vel = 20.0, 
                                                    double Acc = 0.3, double Dec = 0.0, double SndPos = 0.0); //abs move with mm.
    delegate    bool   dgfMoveHome      (int iAxis, double Vel , double Acc, double Dec = 0.0, double OffsetPulse = 0.0 , double OffSetPos = 0.0);
    delegate    double dgfGetCmdPos     (int iAxis                                 );
    delegate    double dgfGetEncPos     (int iAxis                                 );
    delegate    bool   dgfSetPos        (int iAxis, double Pos                     );
    delegate    bool   dgfSetPosEncToCmd(int iAxis                                 );
                       
    delegate    void   dgfClearPos      (int iAxis, double Pos = 0.0);
    delegate    bool   dgfGetStop       (int iAxis, bool ChkEnc, double InPos);  
    delegate    bool   dgfMotionDone    (int iAxis                                 );
    delegate    void   dgfSetParamPath  (String Path, String CmePath = "");
    delegate    void   dgfSetType       (int iAxis, int    iType = 0 ,int iKind = 0 , int iNotUse = 0);
    delegate    void   dgfSetPulseOut   (int iAxis, int    Data   = 1);
    delegate    void   dgfSetEncInput   (int iAxis, int    Data   );
    delegate    void   dgfSetSONLevel   (int iAxis, int    Data   );
	delegate	void   dgfSetInpLevel   (int iAxis, int    Data   );
	delegate	void   dgfSetAlarmLevel (int iAxis, int    Data   );
	delegate	void   dgfSetLimitLevel (int iAxis, int    PosData, int NegData);
    delegate    void   dgfSetHomeLevel  (int iAxis, int    Data   );
    delegate    void   dgfSetDirection  (int iAxis, int    Data   );
    delegate    void   dgfSetAutoResp   (int iAxis, int    Data   );
    delegate    void   dgfSetPackType   (int iAxis, int    Data   );
    delegate    void   dgfSetHomeType   (int iAxis, int    Data   );
    delegate    void   dgfSetHomeOptn   (int iAxis, int    Data   , int Data2);
	delegate    void   dgfSetMoveHomeSen(int iAxis, int    Data1  , int Data2);
    delegate    void   dgfSetEncPulse   (int iAxis, int    Data   );
    delegate    void   dgfSetServoType  (int iAxis, int    Data   );
    delegate    void   dgfSetAppScurve  (int iAxis, bool   Data   );
    delegate    void   dgfSetIntpAxe    (int iAxis, int    Data   );
    delegate    void   dgfSetPairAxe    (int lMasterAxeNo, int lSlaveAxeNo=0, int Data2 = 0);
    delegate    void   dgfSetCoefficient(double Data              );
    delegate    void   dgfSetRingCounter(int iAxis, bool bEnable, double dMaxCntr);

	delegate    void   dgfSetEndLimitEnable(int iAxis, int  Data);
	delegate    void   dgfSetServoParam    (int iAxis, int  iParamNo, int Data);
	delegate    void   dgfSetUseTorque     (int iAxis, bool Data, string sWAddr, string sRAddr);
	delegate	void   dgfSetABS           (int iAxis, int  Data1,int Data2);

	delegate	void   dgfSetPreTrgPos    (double dPos);
	delegate	void   dgfSetTrgPos       (double dPos);
    delegate    void   dgfSetHomeEnd      (bool bSet);
	delegate    bool   dgfSetMoveHomeForce(int iAxis, double Vel , double Acc);
	

	delegate	bool   dgfGServo        ();
	delegate	bool   dgfGHome         ();
	delegate	bool   dgfGStop         ();
	delegate	bool   dgfGReady        ();
	delegate	bool   dgfGBusy         ();
	delegate	bool   dgfGHomeEnd      ();
	delegate	bool   dgfGPackInPosn   ();
	delegate	bool   dgfGAlarm        ();
	delegate	bool   dgfGCW           ();
	delegate	bool   dgfGCCW          ();
	delegate	bool   dgfGZP           ();
	delegate	bool   dgfGLtBusy       ();
	delegate	bool   dgfGRing         ();
	delegate	double dgfGTorque       ();
	delegate	double dgfGPreTrgPos    ();
	delegate	double dgfGTrgPos       ();
	delegate	double dgfGOrgEncPos    ();
	delegate	int	   dgfGHomeStep     ();

    delegate	void   dgfSetComPort    (string sPort);
    
    delegate    int    dgfGetErrCode1   ();
    delegate    int    dgfGetErrCode2   ();



    /***************************************************************************/
    /* Class: TAxisUnit                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TAxisUnit
    {
        const int MAX_PITCH = 5;


        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_StopOnTimer  = new TOnDelayTimer();
        TOnDelayTimer m_MoveTimer    = new TOnDelayTimer();
        TOnDelayTimer m_MoveInpTimer = new TOnDelayTimer();
        TOnDelayTimer m_tAbsOrigin   = new TOnDelayTimer();
        TOnDelayTimer m_tHomeEnd     = new TOnDelayTimer();
        TOnDelayTimer m_tJogStopDely = new TOnDelayTimer();

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */

        int      m_iAxis  ; //My AJIN.
        double   dScanTime_UP;
        double   dStrtTime_UP;
        double   dScanTime_MV;
        double   dStrtTime_MV;
                
        double   m_dCoef       ; //Position.
        double   m_dCmdPos     ;
        double   m_dEncPos     ;
        bool     m_bJogSpd     ;
        bool     m_bStepMove   ;
        bool     m_bJogN       ; //Jog Status.
        bool     m_bJogP       ;
        bool     m_bLtJogN     ;
        bool     m_bLtJogP     ;
        bool     m_bTorqueN    ; //Torque Status.
        bool     m_bTorqueP    ;
        bool     m_bLtTorqueN  ;
        bool     m_bLtTorqueP  ;
        bool     m_bPrivServoOn;
        bool     m_bPrivAlarm  ;
        double[] m_dPitch    = new double[MAX_PITCH];
        double[] m_dCenPitch = new double[MAX_PITCH];
        int[]    m_NoSlot    = new int   [MAX_PITCH];
        double   m_dSpedGain   ; //One Time Motor Gain & Speed Mode.
        double   m_dAccGain    ;
        int      m_iSpedRatio  ;
        int      m_iObjNo      ;
        int      m_iAbsStep    ;
        int      m_iMaker      ;    
        bool     m_bInitAxis   ;

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        //Var Motor Option
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int    m_iMotrType      ;
        public int    m_iMotrKind      ;
        public double m_dGR_Pulse      ;
        public double m_dGR_Disp       ;
        public double m_dMinPosn       ;
        public double m_dMaxPosn       ;
        public double m_dMaxVel        ;
        public double m_dMinAcc        ;
        public double m_dMinDec        ;
        public double m_dHomeOff       ;
        public double m_dMaxPulse      ;
        public double m_dInPos         ;
        public double m_dPartialPos    ;
        public double m_dAbsOriginPos  ;
        public int    m_iSONLevel      ;
        public int    m_iInpLevel      ;
        public int    m_iAlarmLevel    ;
        public int    m_iEncInputLevel ;
        public int    m_iEndLimitEnable;
        public int    m_iHomeLevel     ;
        public int    m_iProcessDir    ; //MotorÀÇ Running ÁøÇà ¹æÇâ°ú HomeÀÇ ¹æÇâ ¼³Á¤.
        public int    m_iPulseOut      ;
        public int    m_iUseCam        ;
        public int    m_iNoUseEnc      ;
        public int    m_iNoUseMotr     ;
        public int    m_iNoUseHomeSen  ;
        public int    m_iMotionPart    ;
        public double m_dRingMax       ;
        public int    m_iPackType      ;
        public int    m_iIntpAxisNo    ;
        public int    m_iPairAxisNo    ;
        public int    m_iMoveDirection ;
        public int    m_iAutoResp      ;
        public int    m_iHomeType      ;
        public int    m_iHomeOptUse    ;
        public int    m_iEncPulse      ;
        public int    m_iServoType     ;
        public bool   m_bSetScurve     ;
        public int    m_iComPort       ; //EN_PORT_NAME
        public int    m_iCOMIStartIO   ;
        public int    m_iHomeSignal    ;
        public int    m_iHomeZPhase    ;

        public int    m_iPosLimitLevel ;
        public int    m_iNegLimitLevel ;
        public bool   m_bUseRingCounter;
        public bool   m_bUseOverride   ;
        public bool   m_bUseTorque     ;
        public bool   m_bRising        ;
        public int    m_iHomeDir       ;
        public string m_sComPort       ;


        public string m_sToqWAddr      ;
        public string m_sToqRAddr      ;


        //Define Motor                   
        public string m_sName          ;
        public string m_sNameAxis      ;
        public string m_sImgP          ;
        public string m_sImgN          ;

        //Define Manual No               
        public int    m_iManStop       ;
        public int    m_iManJog        ;
        public int    m_iManPitch      ;
        public int    m_iManServo      ;
        public int    m_iManAlarm      ;
        public int    m_iManDirect     ;
        public int    m_iManHome       ;
        public int    m_iManPartHome   ;

        //Define Error  No               
        public int    m_iErrAlarm      ;
        public int    m_iErrCW         ;
        public int    m_iErrCCW        ;
        public int    m_iErrHome       ;
        public int    m_iErrControl    ;
        public int    m_iErrHold       ;
        public int    m_iErrPos        ;
        public int    m_iErrVel        ;
        public int    m_iErrAcc        ;

        //Buffer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TMOTN_PARA MP     = new TMOTN_PARA(); //Motion parameters. (INI  )
        public TMOTN_PARA CMP    = new TMOTN_PARA();
        public TMOTN_PARA TempMP = new TMOTN_PARA();


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TAxisAjin     AJIN ;
        //TAxisWmx3   WMX3 ;
		TAxisSimul    SIMUL;
        TAxisEziServo EziServo;

        //delegate 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        dgfInit             _fInit              ;
        dgfDevReset         _fDevReset          ;
        dgfClose            _fClose             ;
        dgfReset            _fReset             ;
        dgfUpdateAxe        _fUpdateAxe         ;
        dgfOpen             _fOpen              ;
        dgfClearHomeEnd     _fClearHomeEnd      ;
        dgfSetServo         _fSetServo          ;
        dgfSetAlarm         _fSetAlarm          ;
        dgfEmrgStop         _fEmrgStop          ;
        dgfStop             _fStop              ;
        dgfMoveJogP         _fMoveJogP          ;
        dgfMoveJogN         _fMoveJogN          ;
        dgfMove             _fMove              ;
        dgfMoveOverride     _fMoveOverride      ;
                            
        dgfMoveHome         _fMoveHome          ;
        dgfGetCmdPos        _fGetCmdPos         ;
        dgfGetEncPos        _fGetEncPos         ;
        dgfSetPos           _fSetPos            ;
        dgfSetPosEncToCmd   _fSetPosEncToCmd    ;
                                    
        dgfClearPos         _fClearPos          ;
        dgfGetStop          _fGetStop           ;
        dgfMotionDone       _fMotionDone        ;
        dgfSetParamPath     _fSetParamPath      ;
        dgfSetType          _fSetType           ;
        dgfSetPulseOut      _fSetPulseOut       ;
        dgfSetEncInput      _fSetEncInput       ;
        dgfSetSONLevel      _fSetSONLevel       ;
        dgfSetInpLevel      _fSetInpLevel       ;
        dgfSetAlarmLevel    _fSetAlarmLevel     ;
        dgfSetLimitLevel    _fSetLimitLevel     ;
        dgfSetHomeLevel     _fSetHomeLevel      ;
        dgfSetDirection     _fSetDirection      ;
        dgfSetAutoResp      _fSetAutoResp       ;
        dgfSetPackType      _fSetPackType       ;
        dgfSetHomeType      _fSetHomeType       ;
        dgfSetHomeOptn      _fSetHomeOptn       ;
		dgfSetMoveHomeSen   _fSetMoveHomeSen    ;
        dgfSetEncPulse      _fSetEncPulse       ;
        dgfSetServoType     _fSetServoType      ;
        dgfSetAppScurve     _fSetAppScurve      ;
        dgfSetIntpAxe       _fSetIntpAxe        ;
        dgfSetPairAxe       _fSetPairAxe        ;
        dgfSetCoefficient   _fSetCoefficient    ;
        dgfSetRingCounter   _fSetRingCounter    ;                                  
        dgfSetABS           _fSetABS            ;                               
        dgfSetPreTrgPos     _fSetPreTrgPos      ;
        dgfSetTrgPos        _fSetTrgPos         ;
        dgfSetHomeEnd       _fSetHomeEnd        ;	
		dgfSetMoveHomeForce _fSetMoveHomeForce  ;

		dgfSetEndLimitEnable _fSetEndLimitEnable;
		dgfSetServoParam     _fSetServoParam    ;
		dgfSetUseTorque      _fSetUseTorque     ;
		
								
        dgfGServo          _fGServo            ;
        dgfGHome           _fGHome             ;
        dgfGStop           _fGStop             ;
        dgfGReady          _fGReady            ;
        dgfGBusy           _fGBusy             ;
        dgfGHomeEnd        _fGHomeEnd          ;
        dgfGPackInPosn     _fGPackInPosn       ;
        dgfGAlarm          _fGAlarm            ;
        dgfGCW             _fGCW               ;
        dgfGCCW            _fGCCW              ;
        dgfGLtBusy         _fGLtBusy           ;
        dgfGRing           _fGRing             ;
        dgfGTorque         _fGTorque           ;
        dgfGPreTrgPos      _fGPreTrgPos        ;
        dgfGTrgPos         _fGTrgPos           ;
		dgfGOrgEncPos      _fGOrgEncPos        ;
        dgfGHomeStep       _fGHomeStep         ;
        dgfSetComPort      _fSetComport        ;
        dgfGetErrCode1     _fGErrCode1         ;
        dgfGetErrCode2     _fGErrCode2         ;
                                               

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int    _iNoUseMotr   { get { return m_iNoUseMotr;  } }
        public int    _iMaker       { get { return m_iMaker;      } }
        public int    _iPairAxisNo  { get { return m_iPairAxisNo; } }
        public bool   _bInitAxis    { get { return m_bInitAxis;   } set { m_bInitAxis = value; } } 
		public double _dScanTime_UP { get { return dScanTime_UP;  }	}
		public double _dScanTime_MV { get { return dScanTime_MV;  }	}

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TAxisUnit()
        {
            //Init. Var.
            m_iAxis           = -1;
            m_iObjNo          = -1;
            m_iMaker          = (int)EN_MOTR_MAKER.SIMUL;
            m_iMotrType       = (int)EN_MOTR_TYPE.Rotary;
            m_iMotrKind       = (int)EN_MOTR_KIND.INC;
            m_dGR_Pulse       = 8192.0;
            m_dGR_Disp        = 10.0;
            m_dMinPosn        = 0.0;
            m_dMaxPosn        = 100.0;
            m_dMaxVel         = 100.0;
            m_dMinAcc         = 0.1;
            m_dMinDec         = 0.1;
            m_dHomeOff        = 0.0;
            m_dMaxPulse       = 8192;
            m_dInPos          = 1.0;
            m_dPartialPos     = 1.0;
            m_iSONLevel       = 1;
            m_iInpLevel       = 1;
            m_iAlarmLevel     = 1;
            m_iEncInputLevel  = 3;
            m_iEndLimitEnable = 1;
            m_iHomeLevel      = 1;
            m_iProcessDir     = 1;
            m_iPulseOut       = 8;
            m_iUseCam         = 0;
            m_iNoUseEnc       = 0;
            m_iNoUseHomeSen   = 0;
            m_iMotionPart     = 0;
            m_iHomeDir        = 0;
            m_dRingMax        = 360.0;
            m_iPackType       = (int)EN_PACK_TYPE.RJ004_12M;
            m_iIntpAxisNo     = -1;
            m_iPairAxisNo     = -1;
            m_iSpedRatio      = (int)EN_SPED_RATIO.sr100;
            m_bUseOverride    = false;
            m_sComPort        = "COM1";

            //Define Manual No
            m_iManStop        = -1;
            m_iManJog         = -1;
            m_iManPitch       = -1;
            m_iManServo       = -1;
            m_iManAlarm       = -1;
            m_iManDirect      = -1;

            //Define Error  No
            m_iErrAlarm       = -1;
            m_iErrCW          = -1;
            m_iErrCCW         = -1;
            m_iErrHome        = -1;
            m_iErrControl     = -1;
            m_iErrHold        = -1;
            m_iErrPos         = -1;
            m_iErrVel         = -1;
            m_iErrAcc         = -1;
            m_bRising         = false;
            m_bSetScurve      = true;
            m_iHomeSignal     = 2;
            m_iHomeZPhase     = 0;
            m_iPosLimitLevel  = 1;
            m_iNegLimitLevel  = 1;
            m_iAbsStep = 0;

            for (int i = 0; i < MAX_PITCH; i++)
            {
                m_dPitch   [i] = 0.0;
                m_dCenPitch[i] = 0.0;
                m_NoSlot   [i] = 0;
            }
        }
        ~TAxisUnit() { }

        //Base Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init(int iAxe, int iObjNo, int iMaker)
        {
            //Init. Var.
            m_iAxis  = iAxe  ;
            m_iObjNo = iObjNo;
            m_iMaker = iMaker;

			if(isMakerSIMUL())
			{
                SIMUL = new TAxisSimul();
                //
                _fInit              = SIMUL.Init             ;
                _fDevReset          = SIMUL.DevReset         ;
                _fClose             = SIMUL.Close            ;
                _fReset             = SIMUL.Reset            ;
                _fUpdateAxe         = SIMUL.Update           ;
                _fOpen              = SIMUL.Open             ;
                _fClearHomeEnd      = SIMUL.ClearHomeEnd     ;
                _fSetServo          = SIMUL.SetServo         ;
                _fSetAlarm          = SIMUL.SetAlarm         ;
                _fEmrgStop          = SIMUL.EmrgStop         ;
                _fStop              = SIMUL.Stop             ;            
                _fMoveJogP          = SIMUL.MoveJogP         ;
                _fMoveJogN          = SIMUL.MoveJogN         ;
                _fMove              = SIMUL.Move             ;
                _fMoveOverride      = SIMUL.MoveOverride     ;
                _fMoveHome          = SIMUL.MoveHome         ;
				_fSetMoveHomeForce  = SIMUL.SetMoveHomeForce ;
                _fGetCmdPos         = SIMUL.GetCmdPos        ;
                _fGetEncPos         = SIMUL.GetEncPos        ;
                _fSetPos            = SIMUL.SetPos           ;
                _fSetPosEncToCmd    = SIMUL.SetPosEncToCmd   ;
                _fClearPos          = SIMUL.ClearPos         ;
                _fGetStop           = SIMUL.GetStop          ;
                _fMotionDone        = SIMUL.MotionDone       ;
                _fSetParamPath      = SIMUL.SetParamPath     ;
                _fSetType           = SIMUL.SetType          ;
                _fSetPulseOut       = SIMUL.SetPulseOut      ;
                _fSetEncInput       = SIMUL.SetEncInput      ;
                _fSetSONLevel       = SIMUL.SetSONLevel      ;
                _fSetInpLevel       = SIMUL.SetInpLevel      ;
                _fSetAlarmLevel     = SIMUL.SetAlarmLevel    ;
                _fSetLimitLevel     = SIMUL.SetLimitLevel    ;
                _fSetHomeLevel      = SIMUL.SetHomeLevel     ;
                _fSetDirection      = SIMUL.SetDirection     ;
                _fSetAutoResp       = SIMUL.SetAutoResp      ;
                _fSetPackType       = SIMUL.SetPackType      ;
                _fSetHomeType       = SIMUL.SetHomeType      ;
                _fSetHomeOptn       = SIMUL.SetHomeOptn      ;
				_fSetMoveHomeSen    = SIMUL.SetMoveHomeSensor;
                _fSetEncPulse       = SIMUL.SetEncPulse      ;
                _fSetServoType      = SIMUL.SetServoType     ;
                _fSetAppScurve      = SIMUL.SetAppScurve     ;
                _fSetIntpAxe        = SIMUL.SetIntpAxe       ;
                _fSetPairAxe        = SIMUL.SetPairAxe       ;
                _fSetCoefficient    = SIMUL.SetCoefficient   ;
                _fSetRingCounter    = SIMUL.SetRingCounter   ;
				_fSetEndLimitEnable	= SIMUL.SetEndLimitEnable;
				_fSetServoParam    	= SIMUL.SetServoParam    ;
				_fSetUseTorque      = SIMUL.SetUseTorque     ;                                  
                _fSetABS            = SIMUL.SetABS           ;		    
                _fSetPreTrgPos      = SIMUL.sPreTrgPos       ;
                _fSetTrgPos         = SIMUL.sTrgPos          ;
                _fSetHomeEnd        = SIMUL.sHomeEnd         ;
                _fGServo            = SIMUL.gServo           ;
                _fGHome             = SIMUL.gHome            ;
                _fGStop             = SIMUL.gStop            ;
                _fGReady            = SIMUL.gReady           ;
                _fGBusy             = SIMUL.gBusy            ;
                _fGHomeEnd          = SIMUL.gHomeEnd         ;
                _fGPackInPosn       = SIMUL.gPackInPosn      ;
                _fGAlarm            = SIMUL.gAlarm           ;
                _fGCW               = SIMUL.gCW              ;
                _fGCCW              = SIMUL.gCCW             ;
                _fGLtBusy           = SIMUL.gLtBusy          ;
                _fGRing             = SIMUL.gRing            ;
                _fGTorque           = SIMUL.gTorque          ;
                _fGPreTrgPos        = SIMUL.gPreTrgPos       ;
                _fGTrgPos           = SIMUL.gTrgPos          ;
				_fGOrgEncPos        = SIMUL.gOrgEncPos       ;
                _fGHomeStep         = SIMUL.gHomeStep        ;
                _fSetComport        = SIMUL.SetComPort       ;
                _fGErrCode1         = SIMUL.gErrCode1        ;
                _fGErrCode2         = SIMUL.gErrCode2        ;
			}
            else if(isMakerAjin())
            {
                AJIN = new TAxisAjin();
                _fInit              = AJIN.Init             ;
                _fDevReset          = AJIN.DevReset         ;
                _fClose             = AJIN.Close            ;
                _fReset             = AJIN.Reset            ;
                _fUpdateAxe         = AJIN.Update           ;
                _fOpen              = AJIN.Open             ;
                _fClearHomeEnd      = AJIN.ClearHomeEnd     ;
                _fSetServo          = AJIN.SetServo         ;
                _fSetAlarm          = AJIN.SetAlarm         ;
                _fEmrgStop          = AJIN.EmrgStop         ;
                _fStop              = AJIN.Stop             ;
                _fMoveJogP          = AJIN.MoveJogP         ;
                _fMoveJogN          = AJIN.MoveJogN         ;
                _fMove              = AJIN.Move             ;
                _fMoveOverride      = AJIN.MoveOverride     ;
                _fMoveHome          = AJIN.MoveHome         ;
                _fGetCmdPos         = AJIN.GetCmdPos        ;
                _fGetEncPos         = AJIN.GetEncPos        ;
                _fSetPos            = AJIN.SetPos           ;
                _fSetPosEncToCmd    = AJIN.SetPosEncToCmd   ;
                _fClearPos          = AJIN.ClearPos         ;
                _fGetStop           = AJIN.GetStop          ;
                _fMotionDone        = AJIN.MotionDone       ;
                _fSetParamPath      = AJIN.SetParamPath     ;
                _fSetType           = AJIN.SetType          ;
                _fSetPulseOut       = AJIN.SetPulseOut      ;
                _fSetEncInput       = AJIN.SetEncInput      ;
                _fSetSONLevel       = AJIN.SetSONLevel      ;
                _fSetInpLevel       = AJIN.SetInpLevel      ;
                _fSetAlarmLevel     = AJIN.SetAlarmLevel    ;
                _fSetLimitLevel     = AJIN.SetLimitLevel    ;
                _fSetHomeLevel      = AJIN.SetHomeLevel     ;
                _fSetDirection      = AJIN.SetDirection     ;
                _fSetAutoResp       = AJIN.SetAutoResp      ;
                _fSetPackType       = AJIN.SetPackType      ;
                _fSetHomeType       = AJIN.SetHomeType      ;
                _fSetHomeOptn       = AJIN.SetHomeOptn      ;
                _fSetMoveHomeSen    = AJIN.SetMoveHomeSensor;
                _fSetEncPulse       = AJIN.SetEncPulse      ;
                _fSetServoType      = AJIN.SetServoType     ;
                _fSetAppScurve      = AJIN.SetAppScurve     ;
                _fSetIntpAxe        = AJIN.SetIntpAxe       ;
                _fSetPairAxe        = AJIN.SetPairAxe       ;
                _fSetCoefficient    = AJIN.SetCoefficient   ;
                _fSetRingCounter    = AJIN.SetRingCounter   ;
                _fSetEndLimitEnable = AJIN.SetEndLimitEnable;
                _fSetServoParam     = AJIN.SetServoParam    ;
                _fSetUseTorque      = AJIN.SetUseTorque     ;
                _fSetABS            = AJIN.SetABS           ;
                _fSetPreTrgPos      = AJIN.sPreTrgPos       ;
                _fSetTrgPos         = AJIN.sTrgPos          ;
                _fSetHomeEnd        = AJIN.sHomeEnd         ;
                _fGServo            = AJIN.gServo           ;
                _fGHome             = AJIN.gHome            ;
                _fGStop             = AJIN.gStop            ;
                _fGReady            = AJIN.gReady           ;
                _fGBusy             = AJIN.gBusy            ;
                _fGHomeEnd          = AJIN.gHomeEnd         ;
                _fGPackInPosn       = AJIN.gPackInPosn      ;
                _fGAlarm            = AJIN.gAlarm           ;
                _fGCW               = AJIN.gCW              ;
                _fGCCW              = AJIN.gCCW             ;
                _fGLtBusy           = AJIN.gLtBusy          ;
                _fGRing             = AJIN.gRing            ;
                _fGTorque           = AJIN.gTorque          ;
                _fGPreTrgPos        = AJIN.gPreTrgPos       ;
                _fGTrgPos           = AJIN.gTrgPos          ;
                _fGHomeStep         = AJIN.gHomeStep        ;
                _fSetComport        = AJIN.SetComPort       ;
                _fGErrCode1         = AJIN.gErrCode1        ;
                _fGErrCode2         = AJIN.gErrCode2        ;
            }
            else if(isMakerEziServo())
            {
                EziServo = new TAxisEziServo();
                _fInit              = EziServo.Init             ;
                _fDevReset          = EziServo.DevReset         ;
                _fClose             = EziServo.Close            ;
                _fReset             = EziServo.Reset            ;
                _fUpdateAxe         = EziServo.Update           ;
                _fOpen              = EziServo.Open             ;
                _fClearHomeEnd      = EziServo.ClearHomeEnd     ;
                _fSetServo          = EziServo.SetServo         ;
                _fSetAlarm          = EziServo.SetAlarm         ;
                _fEmrgStop          = EziServo.EmrgStop         ;
                _fStop              = EziServo.Stop             ;
                _fMoveJogP          = EziServo.MoveJogP         ;
                _fMoveJogN          = EziServo.MoveJogN         ;
                _fMove              = EziServo.Move             ;
                _fMoveOverride      = EziServo.MoveOverride     ;
                _fMoveHome          = EziServo.MoveHome         ;
                _fGetCmdPos         = EziServo.GetCmdPos        ;
                _fGetEncPos         = EziServo.GetEncPos        ;
                _fSetPos            = EziServo.SetPos           ;
                _fSetPosEncToCmd    = EziServo.SetPosEncToCmd   ;
                _fClearPos          = EziServo.ClearPos         ;
                _fGetStop           = EziServo.GetStop          ;
                _fMotionDone        = EziServo.MotionDone       ;
                _fSetParamPath      = EziServo.SetParamPath     ;
                _fSetType           = EziServo.SetType          ;
                _fSetPulseOut       = EziServo.SetPulseOut      ;
                _fSetEncInput       = EziServo.SetEncInput      ;
                _fSetSONLevel       = EziServo.SetSONLevel      ;
                _fSetInpLevel       = EziServo.SetInpLevel      ;
                _fSetAlarmLevel     = EziServo.SetAlarmLevel    ;
                _fSetLimitLevel     = EziServo.SetLimitLevel    ;
                _fSetHomeLevel      = EziServo.SetHomeLevel     ;
                _fSetDirection      = EziServo.SetDirection     ;
                _fSetAutoResp       = EziServo.SetAutoResp      ;
                _fSetPackType       = EziServo.SetPackType      ;
                _fSetHomeType       = EziServo.SetHomeType      ;
                _fSetHomeOptn       = EziServo.SetHomeOptn      ;
                _fSetMoveHomeSen    = EziServo.SetMoveHomeSensor;
                _fSetEncPulse       = EziServo.SetEncPulse      ;
                _fSetServoType      = EziServo.SetServoType     ;
                _fSetAppScurve      = EziServo.SetAppScurve     ;
                _fSetIntpAxe        = EziServo.SetIntpAxe       ;
                _fSetPairAxe        = EziServo.SetPairAxe       ;
                _fSetCoefficient    = EziServo.SetCoefficient   ;
                _fSetRingCounter    = EziServo.SetRingCounter   ;
                _fSetEndLimitEnable = EziServo.SetEndLimitEnable;
                _fSetServoParam     = EziServo.SetServoParam    ;
                _fSetUseTorque      = EziServo.SetUseTorque     ;
                _fSetABS            = EziServo.SetABS           ;
                _fSetPreTrgPos      = EziServo.sPreTrgPos       ;
                _fSetTrgPos         = EziServo.sTrgPos          ;
                _fSetHomeEnd        = EziServo.sHomeEnd         ;
                _fGServo            = EziServo.gServo           ;
                _fGHome             = EziServo.gHome            ;
                _fGStop             = EziServo.gStop            ;
                _fGReady            = EziServo.gReady           ;
                _fGBusy             = EziServo.gBusy            ;
                _fGHomeEnd          = EziServo.gHomeEnd         ;
                _fGPackInPosn       = EziServo.gPackInPosn      ;
                _fGAlarm            = EziServo.gAlarm           ;
                _fGCW               = EziServo.gCW              ;
                _fGCCW              = EziServo.gCCW             ;
                _fGLtBusy           = EziServo.gLtBusy          ;
                _fGRing             = EziServo.gRing            ;
                _fGTorque           = EziServo.gTorque          ;
                _fGPreTrgPos        = EziServo.gPreTrgPos       ;
                _fGTrgPos           = EziServo.gTrgPos          ;
                _fGHomeStep         = EziServo.gHomeStep        ;
                _fGErrCode1         = EziServo.gErrCode1        ;
                _fGErrCode2         = EziServo.gErrCode2        ;
            }

            else return;
            _fInit        ();
        }
        //--------------------------------------------------------------------------
        public bool        Open             (    )
        {
            bool bOpen = false;

            //Open Motor
            bOpen       = _fOpen();
            return bOpen;
        }
        //--------------------------------------------------------------------------
        public void        Close            (    )
        {
            _fClose();
        }
        //--------------------------------------------------------------------------
        public void        Reset            (    )
        {
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            if(!m_bInitAxis) return;

            //Reset homing flag.
            if (GetStop ()) { _fReset(); }
            if (GetAlarm()) { _fClearPos(m_iObjNo); }
            _fSetTrgPos(m_dCmdPos);

            m_dSpedGain = 1.0;
            m_dAccGain  = 1.0;


            //Reset Timer.
            m_StopOnTimer .Clear();
            m_MoveTimer   .Clear();
            m_MoveInpTimer.Clear();
        }
        //--------------------------------------------------------------------------
        public void  DevReset         (    )
        {
            _fDevReset();
        }
        //Home Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void        ClearHomeEnd     (             )
        {
            //Check Motor Status.
            if (m_iNoUseMotr==1) return;
            if(!m_bInitAxis    ) return;
            _fClearHomeEnd();

            int iPairAxe = GetPairAxe();
            if (iPairAxe >= 0)
            {
                _fClearHomeEnd();
            }
        }
        //--------------------------------------------------------------------------
        public bool HomeProc(double Vel = 50.0)
        {
            return false;
        }
        //--------------------------------------------------------------------------
        //Motion Done
        public bool MotnDone(bool ChkEnc, double Inp)
        {
            return GetStop(ChkEnc, Inp);
        }
        //--------------------------------------------------------------------------
        public bool MotnDone()
        {
            return GetStop(true, MP.dPosn[(int)EN_POSN_ID.InPos]);
        }
        //--------------------------------------------------------------------------
        public bool isMakerAjin()
        {
            return (m_iMaker == (int)EN_MOTR_MAKER.AJIN || m_iMaker == (int)EN_MOTR_MAKER.AJECAT);
        }
        //---------------------------------------------------------------------------
        public bool isMakerWMX3()
        {
            return (m_iMaker == (int)EN_MOTR_MAKER.WMX3);
        }
        //---------------------------------------------------------------------------
        public bool isMakerSIMUL()
        {
            return (m_iMaker == (int)EN_MOTR_MAKER.SIMUL);
        }
        //---------------------------------------------------------------------------
        public bool isMakerEziServo()
        {
            return (m_iMaker == (int)EN_MOTR_MAKER.EZISERVO);
        }
        //---------------------------------------------------------------------------
        public bool CheckMaker(EN_MOTR_MAKER maker)
        {
            return (m_iMaker == (int)maker);
        }

        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetParameter(int Axis = -1)
        {
            SetAxis          (Axis);
            SetType          (m_iMotrType, m_iMotrKind, m_iNoUseMotr);
            SetSONLevel      (m_iSONLevel);
            SetInpLevel      (m_iInpLevel);
            SetAlarmLevel    (m_iAlarmLevel);
            SetHomeLevel     (m_iHomeLevel);
            SetPulseOut      (m_iPulseOut);
            SetEncInput      (m_iEncInputLevel);
            SetAppScurve     (m_bSetScurve);
            SetPackType      (m_iPackType);
            SetDirection     (m_iMoveDirection);
            SetIntpAxe       (m_iIntpAxisNo); //보간
            SetPairAxe       (m_iPairAxisNo); //동기화
            SetAutoResp      (m_iAutoResp);
            SetHomeType      (m_iHomeType);
            SetHomeOptn      (m_iHomeOptUse, m_iHomeDir);
            SetEncPulse      (m_iEncPulse);
            SetServoType     (m_iServoType);
            SetMoveHomeSen   (m_iHomeSignal, m_iHomeZPhase);
            SetEndLimitEnable(m_iEndLimitEnable);
            SetLimitLevel    (m_iPosLimitLevel, m_iNegLimitLevel);
            SetABS           (m_iComPort, m_iCOMIStartIO);
            SetMaxMin        (m_dMaxPosn, m_dMaxVel, 5.0, m_dMinPosn, 0.05, m_dMinAcc);
            SetMaxSped       ();
            SetInPos         (m_dInPos);

            //Set Coeff.
            if (m_dGR_Disp <= 0) { SetCoefficient(8192.0 / 10.0); SetMaxPulse(8192); }
            else                 { SetCoefficient(m_dGR_Pulse / m_dGR_Disp); SetMaxPulse(m_dGR_Pulse); }

            SetAbsOrigin  (m_dAbsOriginPos);
            SetUseTorque  (m_bUseTorque     , m_sToqWAddr, m_sToqRAddr);

            if ((Axis == (int)EN_MOTR_ID.FDC_T) ||
                (Axis == (int)EN_MOTR_ID.FWC_T) ||
                (Axis == (int)EN_MOTR_ID.RDC_T) ||
                (Axis == (int)EN_MOTR_ID.RWC_T))
            {
                SetRingCounter(m_bUseRingCounter, m_dGR_Disp, 5.0); //감속비 5 : 1
            }
            else SetRingCounter(m_bUseRingCounter, m_dGR_Disp);
        }
        //--------------------------------------------------------------------------
        public void SetAxis(int Axis = -1)
        {
            m_iAxis = Axis;
        }
        //--------------------------------------------------------------------------
        public void SetType(int iType, int iKind, int iNotUse = 0)
        {
            if (!m_bInitAxis    ) return;
            m_iMotrType = iType;
            m_iMotrKind = iKind;
            _fSetType(m_iObjNo, iType, iKind, iNotUse);            
        }
        //--------------------------------------------------------------------------
        public void SetABS(int ComPort, int SrartIO)
        {
            if (!m_bInitAxis                                     ) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            if (m_iMotrKind  != (int)EN_MOTR_KIND.ABS            ) return;
            if (m_iMaker == (int)EN_MOTR_MAKER.COMI && m_iMotrKind == (int)EN_MOTR_KIND.ABS) _fSetABS(m_iObjNo, ComPort, SrartIO);
            else
            {
                _fSetABS(m_iObjNo, m_iMotrKind , 0);
            }
        }
        //--------------------------------------------------------------------------
        public void SetPart(int iPart = 0)
        {
            m_iMotionPart = iPart;
        }
        //--------------------------------------------------------------------------
        public void SetProcessDir(int iDir)
        {
            m_iProcessDir = iDir; 
        }
        //--------------------------------------------------------------------------

        public void SetParamPath(string Path, string CemPath)
        {
            //if(!m_bInitAxis    ) return;
            _fSetParamPath(Path, CemPath);  
        }
        //--------------------------------------------------------------------------
        public void SetCoefficient(double Data = 819)
        {
            m_dCoef = Data; _fSetCoefficient(Data);
        }
        //--------------------------------------------------------------------------
        public void SetPulseOut(int Data = 1)
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; _fSetPulseOut(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void SetEncInput(int Data = 2) //2:Sqr4Mode
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; _fSetEncInput(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void SetSONLevel(int Data = 0) //1:Positive Level. 0:Negative Level.
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            m_iSONLevel = Data;
            _fSetSONLevel(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void SetInpLevel(int Data = 0)
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            _fSetInpLevel(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void SetAlarmLevel(int Data = 0)
        {
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            _fSetAlarmLevel(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void SetHomeLevel(int Data = 0)
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            _fSetHomeLevel(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void SetLimitLevel(int PosData = 0, int NegData = 0)
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            _fSetLimitLevel(m_iObjNo, PosData, NegData);
        }

        //--------------------------------------------------------------------------
        public void SetComPort(string sComPort)
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            _fSetComport(sComPort);
        }
        //--------------------------------------------------------------------------
        public void SetInPos(double Data)
        {
            m_dInPos = Data;

            //Check Min/Max.
            if (m_dInPos < m_dGR_Disp / m_dGR_Pulse) m_dInPos = m_dGR_Disp / m_dGR_Pulse;
            if (m_dInPos > m_dGR_Disp / 10) m_dInPos = m_dGR_Disp / 10;
        }
        //--------------------------------------------------------------------------
        public void SetHomeOff(double Off = 0)
        {
            m_dHomeOff = Off;

        }
        //--------------------------------------------------------------------------
        public void SetAbsOrigin(double dPos = 0)
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;

            double dAbsOff = dPos + m_dHomeOff;
            if (m_iMotrKind != (int)EN_MOTR_KIND.ABS) return;
            m_dAbsOriginPos = dPos;

            if (isMakerSIMUL()) SIMUL.SetAbsOffset(CalPosToPulse(dAbsOff));
            //else if(isMakerAjin ()) AJIN .SetAbsOffset(CalPosToPulse(dAbsOff));
        }
        //--------------------------------------------------------------------------
        public void SetAppScurve(bool bApply = true)
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
            m_bSetScurve = bApply;
            _fSetAppScurve(m_iObjNo, bApply);
        }
        //--------------------------------------------------------------------------
        public void SetNoUseEnc(bool NoUse)
        {
            m_iNoUseEnc = NoUse ? 1 : 0;
        }
        //--------------------------------------------------------------------------
        public void SetNoUseHomeSen(bool NoUse)
        {
            m_iNoUseHomeSen = NoUse ? 1 : 0;
        }
        //--------------------------------------------------------------------------
        public void SetCamUse(int iUseCam, double dPartialPos)
        {
            m_iUseCam = iUseCam;
            m_dPartialPos = dPartialPos;
        }
        //--------------------------------------------------------------------------
        public void SetMaxSped()
        {
            if (!m_bInitAxis) return;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return;
        }
        //--------------------------------------------------------------------------
        public void        SetMaxPulse      (double        Data)
        {
            m_dMaxPulse = Data; 
        }
        //--------------------------------------------------------------------------
        public void        SetDirection     (int        Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetDirection(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void        SetAutoResp      (int        Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetAutoResp(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void        SetPackType      (int        Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetPackType(m_iObjNo, Data); 
        }
        //--------------------------------------------------------------------------
        public void        SetHomeType      (int        Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetHomeType(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void        SetHomeOptn      (int        Data, int Data2)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetHomeOptn(m_iObjNo, Data, Data2);
        }
        //--------------------------------------------------------------------------
        public void        SetEncPulse      (int        Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetEncPulse(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void        SetServoType     (int        Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetServoType(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void        SetIntpAxe       (int        Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetIntpAxe(m_iObjNo, Data); 
        }
        //--------------------------------------------------------------------------
        public void        SetPairAxe       (int iSlaveNo=0, int Data2=1) 
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            if(iSlaveNo < 0 || iSlaveNo >= cDEF.MOTR._iNumOfMotr) return;

            int iSlaveAxe = cDEF.MOTR[iSlaveNo].m_iObjNo;

            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; _fSetPairAxe(m_iObjNo, iSlaveAxe, Data2);
        }
        //--------------------------------------------------------------------------
        public void        SetVGain         (double     V                                      )
        {
            m_dSpedGain = V;
            int iPairAxe = GetPairAxe();
            if (iPairAxe >= 0)
            {
                cDEF.MOTR[iPairAxe].SetVGain(V);
            }
        }
        //--------------------------------------------------------------------------
        public void        SetAGain         (double     A                                      )
        {
            m_dAccGain = A;
            int iPairAxe = GetPairAxe();
            if (iPairAxe >= 0)
            {
                cDEF.MOTR[iPairAxe].SetAGain(A);
            }
        }
        //--------------------------------------------------------------------------
        public void        SetGain          (double     V         , double A                   )
        {
            m_dSpedGain = V;
            m_dAccGain  = A;
        }
        //--------------------------------------------------------------------------
        public void        SetJogSpd        (bool       Data)
        {
            m_bJogSpd = Data;
        }
        //--------------------------------------------------------------------------
        public void        SetHomeEnd       (bool       Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            if (Data)
            {
                if (m_iHomeType == (int)EN_HOME_TYPE.DataSet) _fSetPos(m_iObjNo, 0.0);
                if (GetAbsAxe() && _fGHomeEnd() == false) _fMove(m_iObjNo, _fGetEncPos(m_iObjNo), MP.MinVel, MP.MaxAcc);
            }
            _fSetHomeEnd(Data);
            int iPairAxe = GetPairAxe();
            if (iPairAxe >= 0)
            {
                cDEF.MOTR[iPairAxe].SetHomeEnd(Data);
            }
        }
        //--------------------------------------------------------------------------
        public void        SetStepMove      (bool       Data)
        {
            m_bStepMove = Data;
        }
        //--------------------------------------------------------------------------
        public void        SetMoveHomeSen   (int Data1, int Data2 )
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

			_fSetMoveHomeSen(m_iObjNo, Data1, Data2); 
        }
        //--------------------------------------------------------------------------
        public void        SetEndLimitEnable(int       Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            
			_fSetEndLimitEnable(m_iObjNo, Data);
        }
        //--------------------------------------------------------------------------
        public void        SetServoParam    (int        iParamNo, int Data)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            
			_fSetServoParam(m_iObjNo, iParamNo, Data);
        }
        //--------------------------------------------------------------------------
        public void        SetUseTorque     (bool       Data, string sWAddr, string sRAddr)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            _fSetUseTorque(m_iObjNo, Data, sWAddr, sRAddr);
        }
        //--------------------------------------------------------------------------
        //
        //Device Informations.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetPitch(double dPitch, int iSlot = 1, double dCenPitch = 0.0, EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.StepA)
        {
            if (FStep == EN_FSTEP_INDEX.StepA)
            {
                m_dPitch[0] = dPitch; m_dCenPitch[0] = dCenPitch; m_NoSlot[0] = iSlot;
                m_dPitch[1] = dPitch; m_dCenPitch[1] = dCenPitch; m_NoSlot[1] = iSlot;
                m_dPitch[2] = dPitch; m_dCenPitch[2] = dCenPitch; m_NoSlot[2] = iSlot;
                m_dPitch[3] = dPitch; m_dCenPitch[3] = dCenPitch; m_NoSlot[3] = iSlot;
            }

            if (FStep < 0 || (int)FStep >= MAX_PITCH) return;
            m_dPitch[(int)FStep] = dPitch;
            m_NoSlot[(int)FStep] = iSlot;
            m_dCenPitch[(int)FStep] = dCenPitch;
        }
        //--------------------------------------------------------------------------
        public double GetPitch(EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            if (FStep < 0 || (int)FStep >= MAX_PITCH) return 1.0;

            double dRet = m_dPitch[(int)FStep];
            if (dRet < 0.1) dRet = 0.1;
            return dRet;
        }
        //--------------------------------------------------------------------------
        public double GetCenPitch(EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            if (FStep < 0 || (int)FStep >= MAX_PITCH) return 0.0;

            double dRet = m_dCenPitch[(int)FStep];
            if (dRet <= 0) dRet = 0.0;
            return dRet;
        }
        //--------------------------------------------------------------------------
        public double GetCenPitch(int iSlotNo, EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            int  iMaxSlot  = GetMaxSlot(FStep);
            int  iMidSlot  = iMaxSlot / 2;
            bool IsLowSlot = iSlotNo < iMidSlot;
            if (FStep < 0 || (int)FStep >= MAX_PITCH) return 0.0;
            if (IsLowSlot) return 0.0;

            double dRet = m_dCenPitch[(int)FStep];
            if (dRet <= 0) dRet = 0.0;
            return dRet;
        }
        //--------------------------------------------------------------------------
        public int GetMaxSlot(EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            if (FStep < 0 || (int)FStep >= MAX_PITCH) return 1;

            int dRet = m_NoSlot[(int)FStep];
            if (dRet < 0) dRet = 1;
            return dRet;
        }
        //--------------------------------------------------------------------------
        //In/Out Sensors.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool        GetServo         (                                               ) //Servo Motor Enable Signal.
        {
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 
            return _fGServo();
        }
        //--------------------------------------------------------------------------
        public bool        GetHome          (                                               ) //Home Sensor.
        {
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 
            return _fGHome();
        }
        //--------------------------------------------------------------------------
        public bool        GetStop          (bool ChkEnc = false , double InPos = 0.2       ) //Motion Done.
        {
            //Local Var.
            double dTrg;
            double dCmd;
            double dEnc;

            //Get Motion Done.
            if(!m_bInitAxis                                     ) return true;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return true; 
            if(m_iNoUseMotr==1                                  ) return true; 

            //Check Stop.
            if (!ChkEnc) return _fGStop();
            else
            {
                if (!GetHomeEnd()) return _fGStop();
                dTrg = GetTrgPos();
                dCmd = GetCmdPos();
                dEnc = GetEncPos();
                if (_fGStop())
                {
                    if (_fGRing())
                    {
                        bool r1 = Math.Abs((m_dGR_Disp - dEnc) - dTrg) < InPos;
                        bool r2 = Math.Abs(dEnc - dTrg) < InPos;
                        if (r1 || r2) dEnc = dTrg;
                    }
                    if (Math.Abs(dCmd - dEnc) > InPos) return false;
                    if (Math.Abs(dTrg - dCmd) > InPos) return false;
                    if (Math.Abs(dTrg - dEnc) > InPos) return false;
                    //Set Pre Trg Pos.
                    _fSetPreTrgPos(dTrg);

                    if (m_bRising)
                    {
						dScanTime_MV = cDEF.TICK._GetTickTime() - dStrtTime_MV;
                        m_bRising = false;
                    }

                    //return
                    return true;

                    //
                    //if (m_MoveInpTimer.Ondelay(true , 10)) return true ;
                    //else                                   return false;
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------
        public bool GetBusy() //Check in motion.
        {
            if (!m_bInitAxis) return false;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false;
            return _fGBusy();
        }
        //--------------------------------------------------------------------------
        public bool GetCntrErr() //Check Control Error. (Command Position >< Actual Position)
        {
            m_StopOnTimer.OnDelay(GetStop(), 100);
            if (m_StopOnTimer.Out) { if (GetErrPos() > m_dInPos) return true; }
            else { if (CalPosToPulse(GetErrPos()) > m_dGR_Pulse) return true; }
            return false;
        }
        //--------------------------------------------------------------------------
        public bool GetHomeEnd() //Check Home Done. (Logical)
        {
            return _fGHomeEnd(); 
        }
        //--------------------------------------------------------------------------
        public bool GetTimeOut() //Check Moving TimeOut.
        {
            return m_MoveTimer.OnDelay(!GetStop(true, m_dInPos), MP.dTime[(int)EN_MOTR_DELAY.TimeOut]);
        }
        //--------------------------------------------------------------------------
        public bool GetPackInPosn() //Check InPosition Signal. (Servo Pack Signal)
        {
            if (!m_bInitAxis                                     ) return false;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            return _fGPackInPosn();
        }
        //--------------------------------------------------------------------------
        public bool GetAlarm() //Check Motor Alarm Signal.
        {
            if (!m_bInitAxis                                     ) return false;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false;
            if (m_iNoUseMotr == 1                                ) return false;
            return _fGAlarm();
        }
        //--------------------------------------------------------------------------
        public bool GetCW() //Check CW Limit.
        {
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false;

            if      (m_iPosLimitLevel == 0) { return !_fGCW(); }//LOW
            else if (m_iPosLimitLevel == 1) { return  _fGCW(); }//high
            else return false;
            
            //return _fGCW();
        }
        //--------------------------------------------------------------------------
        public bool GetCCW() //Check CCW Limit.
        {
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false;

            if      (m_iNegLimitLevel == 0) { return !_fGCCW(); }//LOW
            else if (m_iNegLimitLevel == 1) { return  _fGCCW(); }//high
            else return false;

            //return _fGCCW();
        }
        //--------------------------------------------------------------------------
        public bool GetOk() //Check No Limit & Servo Enable.
        {
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 
            if (m_iNoUseMotr==1                                 ) return true ; 
            //if (GetCCW       ()) return false;
            //if (GetCW        ()) return false;
            if (GetAlarm     ()) return false;
            if (!GetHomeEnd  ()) return false;
            if (!GetServo    ()) return false;
            return true;
        }
        //--------------------------------------------------------------------------
        public bool GetReady() //Check ready to move.
        {
            if (m_iNoUseMotr==1 ) return true; 

            //if (GetCCW        ()) return false;
            //if (GetCW         ()) return false;
            if (GetAlarm      ()) return false;
            //if (!GetServo     ()) return false;
            //if (!GetPackInPosn()) return false;
            //if (!GetStop      ()) return false;
            return true;
        }
        //--------------------------------------------------------------------------
        public int GetHomeStep()
        {
            return _fGHomeStep();
        }
        //--------------------------------------------------------------------------
        public bool GetJogSpd()
        {
            return m_bJogSpd;
        }
        //--------------------------------------------------------------------------
        public bool GetStepMove()
        {
            return m_bStepMove;
        }
        //--------------------------------------------------------------------------
        public double GetTorque()
        {
            if (!m_bInitAxis                                     ) return 0;
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return 0; 
            return _fGTorque();
        }
        //--------------------------------------------------------------------------
        public int GetRunCode()
        {
            return 0;
        }
        //--------------------------------------------------------------------------
        public int GetAlarmCode(int Type = 0)
        {
            switch (Type)
            {
                default: return 0;
                case 0 : return  _fGErrCode1();
                case 1 : return  _fGErrCode2();
            }

            //return 0;
        }
        //--------------------------------------------------------------------------
        public int GetDetailCode()
        {
            return 0;
        }
        //--------------------------------------------------------------------------
        public string GetName(bool IsNor)
        {
            return "";
        }
        //--------------------------------------------------------------------------      
        public double GetFindStep(int Step, EN_FSTEP_INDEX FStep, EN_FPOSN_INDEX FIndex)
        {
            //Local Var.
            double dPitch;
            double dCenPitch;
            double dMovePosn;

            //Check None Step, Index.
            if (Step   == vDEF.NONE_STEP     ) Step   = 0;
            if (FStep  == EN_FSTEP_INDEX.NONE) FStep  = EN_FSTEP_INDEX.Step1;
            if (FIndex == EN_FPOSN_INDEX.NONE) FIndex = EN_FPOSN_INDEX.Index1;

            if(!m_bInitAxis                                     ) return 0;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return 0; 
            if(m_iNoUseMotr==1                                  ) return 0;

            //Get Pitch.
            dPitch = GetPitch(FStep);
            dCenPitch = GetCenPitch(Step, FStep);
            int iPosnNo = 0;
            //Set position to move.
            switch (FStep)
            {
                case EN_FSTEP_INDEX.Step1: iPosnNo = (int)EN_POSN_ID.FSP1_1; break;
                case EN_FSTEP_INDEX.Step2: iPosnNo = (int)EN_POSN_ID.FSP2_1; break;
                case EN_FSTEP_INDEX.Step3: iPosnNo = (int)EN_POSN_ID.FSP3_1; break;
                case EN_FSTEP_INDEX.Step4: iPosnNo = (int)EN_POSN_ID.FSP4_1; break;
            }
            if ((iPosnNo + FIndex) < 0 || (iPosnNo + (int)FIndex) >= vDEF.MAX_POSN) FIndex = EN_FPOSN_INDEX.Index1;
            if (m_iProcessDir==1)
            {
                
                dMovePosn = MP.dPosn[iPosnNo + (int)FIndex] + (double)Step * dPitch + dCenPitch;

            }
            else
            {
                dMovePosn = MP.dPosn[iPosnNo + (int)FIndex] - (double)Step * dPitch - dCenPitch;
            }

            return dMovePosn;
        }
        //--------------------------------------------------------------------------
        public double GetNextCmdTrg(EN_COMD_ID Comd, int Step, EN_FPOSN_INDEX FIndex, double DirPosn)
        {
            //Local Var.
            int iMaxSlot;
            int iPosnId ;
            double dCenPitch, dCenPitchP, dCenPitchN;
            double dPitch, dPitchP, dPitchN;
            EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1;

            //Check None Step, Index.
            if (Step == vDEF.NONE_STEP) Step = 0;
            if (FIndex == EN_FPOSN_INDEX.NONE) FIndex = EN_FPOSN_INDEX.Index1;
            if (m_iNoUseMotr==1              ) return GetTrgPos();

				 if (Comd == EN_COMD_ID.FindStep1  ||
				     Comd == EN_COMD_ID.FindStepF1 ||
				     Comd == EN_COMD_ID.FindStepB1 ||
				     Comd == EN_COMD_ID.OneStepF1  ||
				     Comd == EN_COMD_ID.OneStepB1) FStep = EN_FSTEP_INDEX.Step1;
            else if (Comd == EN_COMD_ID.FindStep2 ||
					 Comd == EN_COMD_ID.FindStepF2 ||
					 Comd == EN_COMD_ID.FindStepB2 ||
					 Comd == EN_COMD_ID.OneStepF2 ||
					 Comd == EN_COMD_ID.OneStepB2) FStep = EN_FSTEP_INDEX.Step2;
            else if (Comd == EN_COMD_ID.FindStep3 ||
					 Comd == EN_COMD_ID.FindStepF3 ||
					 Comd == EN_COMD_ID.FindStepB3 ||
					 Comd == EN_COMD_ID.OneStepF3 ||
					 Comd == EN_COMD_ID.OneStepB3) FStep = EN_FSTEP_INDEX.Step3;
            else if (Comd == EN_COMD_ID.FindStep4 ||
					 Comd == EN_COMD_ID.FindStepF4 ||
					 Comd == EN_COMD_ID.FindStepB4 ||
					 Comd == EN_COMD_ID.OneStepF4 ||
					 Comd == EN_COMD_ID.OneStepB4) FStep = EN_FSTEP_INDEX.Step4;

            //
            iMaxSlot = GetMaxSlot(FStep);    
            //Get Pitch.
            dPitch = GetPitch(FStep);
            dCenPitch = GetCenPitch(Step, FStep);

            dPitchP = dPitch * ((m_iProcessDir==1) ? 1 : -1);
            dPitchN = dPitch * ((m_iProcessDir==1) ? 1 : -1);
            dCenPitchP = dCenPitch * ((m_iProcessDir==1) ? 1 : -1);
            dCenPitchN = dCenPitch * ((m_iProcessDir==1) ? 1 : -1);

            //Command
            switch (Comd)
            {
                case EN_COMD_ID.JogP: return GetCmdPos();
                case EN_COMD_ID.JogN: return GetCmdPos();
                case EN_COMD_ID.FindStep1: 
                    iPosnId = (int)EN_POSN_ID.FSP1_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP1_1;
                    return MP.dPosn[iPosnId] + (Step * dPitchP) + dCenPitchP;

                case EN_COMD_ID.FindStepF1: 
                    iPosnId = (int)EN_POSN_ID.FSP1_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP1_1;
                    return MP.dPosn[iPosnId] + (Step * dPitch) + dCenPitch;
                case EN_COMD_ID.FindStepB1: 
                    iPosnId = (int)EN_POSN_ID.FSP1_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP1_1;
                    return MP.dPosn[iPosnId]- (Step * dPitchP) - dCenPitch;

                case EN_COMD_ID.OneStepF1: return GetTrgPos() + dPitchP + dCenPitchP;
                case EN_COMD_ID.OneStepB1: return GetTrgPos() - dPitchN - dCenPitchN;
                case EN_COMD_ID.FindStep2: 
                    iPosnId = (int)EN_POSN_ID.FSP2_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP2_1;
                    return MP.dPosn[iPosnId] + (Step * dPitchP) + dCenPitchP;
                case EN_COMD_ID.FindStepF2: 
                    iPosnId = (int)EN_POSN_ID.FSP2_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP2_1;
                    return MP.dPosn[iPosnId] + (Step * dPitch) + dCenPitch;
                case EN_COMD_ID.FindStepB2:
                    iPosnId = (int)EN_POSN_ID.FSP2_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP2_1;
                    return MP.dPosn[iPosnId] - (Step * dPitch) - dCenPitch;
                case EN_COMD_ID.OneStepF2: return GetTrgPos() + dPitchP + dCenPitchP;
                case EN_COMD_ID.OneStepB2: return GetTrgPos() - dPitchN - dCenPitchN;
                case EN_COMD_ID.FindStep3:
                    iPosnId = (int)EN_POSN_ID.FSP3_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP3_1;
                    return MP.dPosn[iPosnId] + (Step * dPitchP) + dCenPitchP;
                case EN_COMD_ID.FindStepF3: 
                    iPosnId = (int)EN_POSN_ID.FSP3_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP3_1;
                    return MP.dPosn[iPosnId] + (Step * dPitchP) + dCenPitchP;
                case EN_COMD_ID.FindStepB3: 
                    iPosnId = (int)EN_POSN_ID.FSP3_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP3_1;
                    return MP.dPosn[iPosnId] - (Step * dPitchP) + dCenPitchP;
                case EN_COMD_ID.OneStepF3: return GetTrgPos() + dPitchP + dCenPitchP;
                case EN_COMD_ID.OneStepB3: return GetTrgPos() - dPitchN - dCenPitchN;
                case EN_COMD_ID.FindStep4:
                    iPosnId = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP4_1;
                    return MP.dPosn[iPosnId] + (Step * dPitchP) + dCenPitchP;
                case EN_COMD_ID.FindStepF4: 
                    iPosnId = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP4_1;
                    return MP.dPosn[iPosnId] + (Step * dPitchP) + dCenPitchP;

                case EN_COMD_ID.FindStepB4: 
                    iPosnId = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) iPosnId = (int)EN_POSN_ID.FSP4_1;
                    return MP.dPosn[iPosnId] - (Step * dPitchP) - dCenPitchP;
                case EN_COMD_ID.OneStepF4: return GetTrgPos() + dPitchP + dCenPitchP;
                case EN_COMD_ID.OneStepB4: return GetTrgPos() - dPitchN - dCenPitchN;

                case EN_COMD_ID.UserPitchP: return GetCmdPos() + (MP.dPosn[(int)EN_POSN_ID.UserPitch] * 1.0);
                case EN_COMD_ID.UserPitchN: return GetCmdPos() + (MP.dPosn[(int)EN_POSN_ID.UserPitch] * -1.0);
                case EN_COMD_ID.Wait1 : return MP.dPosn[(int)EN_POSN_ID.Wait1];
                case EN_COMD_ID.Wait2 : return MP.dPosn[(int)EN_POSN_ID.Wait2];
                case EN_COMD_ID.Wait3 : return MP.dPosn[(int)EN_POSN_ID.Wait3];
                case EN_COMD_ID.FSP1_1: return MP.dPosn[(int)EN_POSN_ID.FSP1_1];
                case EN_COMD_ID.FSP1_2: return MP.dPosn[(int)EN_POSN_ID.FSP1_2];
                case EN_COMD_ID.FSP1_3: return MP.dPosn[(int)EN_POSN_ID.FSP1_3];
                case EN_COMD_ID.FSP1_4: return MP.dPosn[(int)EN_POSN_ID.FSP1_4];
                case EN_COMD_ID.FSP1_5: return MP.dPosn[(int)EN_POSN_ID.FSP1_5];
                case EN_COMD_ID.FSP1_6: return MP.dPosn[(int)EN_POSN_ID.FSP1_6];
                case EN_COMD_ID.FSP1_7: return MP.dPosn[(int)EN_POSN_ID.FSP1_7];
                case EN_COMD_ID.FSP1_8: return MP.dPosn[(int)EN_POSN_ID.FSP1_8];
                case EN_COMD_ID.FSP1_9: return MP.dPosn[(int)EN_POSN_ID.FSP1_9];

                case EN_COMD_ID.FSP2_1: return MP.dPosn[(int)EN_POSN_ID.FSP2_1];
                case EN_COMD_ID.FSP2_2: return MP.dPosn[(int)EN_POSN_ID.FSP2_2];
                case EN_COMD_ID.FSP2_3: return MP.dPosn[(int)EN_POSN_ID.FSP2_3];
                case EN_COMD_ID.FSP2_4: return MP.dPosn[(int)EN_POSN_ID.FSP2_4];
                case EN_COMD_ID.FSP2_5: return MP.dPosn[(int)EN_POSN_ID.FSP2_5];
                case EN_COMD_ID.FSP2_6: return MP.dPosn[(int)EN_POSN_ID.FSP2_6];
                case EN_COMD_ID.FSP2_7: return MP.dPosn[(int)EN_POSN_ID.FSP2_7];
                case EN_COMD_ID.FSP2_8: return MP.dPosn[(int)EN_POSN_ID.FSP2_8];
                case EN_COMD_ID.FSP2_9: return MP.dPosn[(int)EN_POSN_ID.FSP2_9];

                case EN_COMD_ID.FSP3_1: return MP.dPosn[(int)EN_POSN_ID.FSP3_1];
                case EN_COMD_ID.FSP3_2: return MP.dPosn[(int)EN_POSN_ID.FSP3_2];
                case EN_COMD_ID.FSP3_3: return MP.dPosn[(int)EN_POSN_ID.FSP3_3];
                case EN_COMD_ID.FSP3_4: return MP.dPosn[(int)EN_POSN_ID.FSP3_4];
                case EN_COMD_ID.FSP3_5: return MP.dPosn[(int)EN_POSN_ID.FSP3_5];
                case EN_COMD_ID.FSP3_6: return MP.dPosn[(int)EN_POSN_ID.FSP3_6];
                case EN_COMD_ID.FSP3_7: return MP.dPosn[(int)EN_POSN_ID.FSP3_7];
                case EN_COMD_ID.FSP3_8: return MP.dPosn[(int)EN_POSN_ID.FSP3_8];
                case EN_COMD_ID.FSP3_9: return MP.dPosn[(int)EN_POSN_ID.FSP3_9];

                case EN_COMD_ID.FSP4_1: return MP.dPosn[(int)EN_POSN_ID.FSP4_1];
                case EN_COMD_ID.FSP4_2: return MP.dPosn[(int)EN_POSN_ID.FSP4_2];
                case EN_COMD_ID.FSP4_3: return MP.dPosn[(int)EN_POSN_ID.FSP4_3];
                case EN_COMD_ID.FSP4_4: return MP.dPosn[(int)EN_POSN_ID.FSP4_4];
                case EN_COMD_ID.FSP4_5: return MP.dPosn[(int)EN_POSN_ID.FSP4_5];
                case EN_COMD_ID.FSP4_6: return MP.dPosn[(int)EN_POSN_ID.FSP4_6];
                case EN_COMD_ID.FSP4_7: return MP.dPosn[(int)EN_POSN_ID.FSP4_7];
                case EN_COMD_ID.FSP4_8: return MP.dPosn[(int)EN_POSN_ID.FSP4_8];
                case EN_COMD_ID.FSP4_9: return MP.dPosn[(int)EN_POSN_ID.FSP4_9];

                case EN_COMD_ID.LSP1_1: return MP.dPosn[(int)EN_POSN_ID.FSP1_1] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_2: return MP.dPosn[(int)EN_POSN_ID.FSP1_2] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_3: return MP.dPosn[(int)EN_POSN_ID.FSP1_3] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_4: return MP.dPosn[(int)EN_POSN_ID.FSP1_4] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_5: return MP.dPosn[(int)EN_POSN_ID.FSP1_5] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_6: return MP.dPosn[(int)EN_POSN_ID.FSP1_6] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_7: return MP.dPosn[(int)EN_POSN_ID.FSP1_7] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_8: return MP.dPosn[(int)EN_POSN_ID.FSP1_8] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP1_9: return MP.dPosn[(int)EN_POSN_ID.FSP1_9] + (iMaxSlot - 1) * dPitchP + dCenPitchP;

                case EN_COMD_ID.LSP2_1: return MP.dPosn[(int)EN_POSN_ID.FSP2_1] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_2: return MP.dPosn[(int)EN_POSN_ID.FSP2_2] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_3: return MP.dPosn[(int)EN_POSN_ID.FSP2_3] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_4: return MP.dPosn[(int)EN_POSN_ID.FSP2_4] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_5: return MP.dPosn[(int)EN_POSN_ID.FSP2_5] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_6: return MP.dPosn[(int)EN_POSN_ID.FSP2_6] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_7: return MP.dPosn[(int)EN_POSN_ID.FSP2_7] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_8: return MP.dPosn[(int)EN_POSN_ID.FSP2_8] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP2_9: return MP.dPosn[(int)EN_POSN_ID.FSP2_9] + (iMaxSlot - 1) * dPitchP + dCenPitchP;

                case EN_COMD_ID.LSP3_1: return MP.dPosn[(int)EN_POSN_ID.FSP3_1] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_2: return MP.dPosn[(int)EN_POSN_ID.FSP3_2] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_3: return MP.dPosn[(int)EN_POSN_ID.FSP3_3] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_4: return MP.dPosn[(int)EN_POSN_ID.FSP3_4] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_5: return MP.dPosn[(int)EN_POSN_ID.FSP3_5] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_6: return MP.dPosn[(int)EN_POSN_ID.FSP3_6] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_7: return MP.dPosn[(int)EN_POSN_ID.FSP3_7] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_8: return MP.dPosn[(int)EN_POSN_ID.FSP3_8] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP3_9: return MP.dPosn[(int)EN_POSN_ID.FSP3_9] + (iMaxSlot - 1) * dPitchP + dCenPitchP;

                case EN_COMD_ID.LSP4_1: return MP.dPosn[(int)EN_POSN_ID.FSP4_1] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_2: return MP.dPosn[(int)EN_POSN_ID.FSP4_2] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_3: return MP.dPosn[(int)EN_POSN_ID.FSP4_3] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_4: return MP.dPosn[(int)EN_POSN_ID.FSP4_4] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_5: return MP.dPosn[(int)EN_POSN_ID.FSP4_5] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_6: return MP.dPosn[(int)EN_POSN_ID.FSP4_6] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_7: return MP.dPosn[(int)EN_POSN_ID.FSP4_7] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_8: return MP.dPosn[(int)EN_POSN_ID.FSP4_8] + (iMaxSlot - 1) * dPitchP + dCenPitchP;
                case EN_COMD_ID.LSP4_9: return MP.dPosn[(int)EN_POSN_ID.FSP4_9] + (iMaxSlot - 1) * dPitchP + dCenPitchP;

                case EN_COMD_ID.Zero:    return 0.0;
                case EN_COMD_ID.Direct:  return DirPosn;
                case EN_COMD_ID.Pick1:   return MP.dPosn[(int)EN_POSN_ID.Pick1];
                case EN_COMD_ID.Pick2:   return MP.dPosn[(int)EN_POSN_ID.Pick2];
                case EN_COMD_ID.Plce1:   return MP.dPosn[(int)EN_POSN_ID.Plce1];
                case EN_COMD_ID.Plce2:   return MP.dPosn[(int)EN_POSN_ID.Plce2];
                case EN_COMD_ID.PickInc: return MP.dPosn[(int)EN_POSN_ID.PickInc];
                case EN_COMD_ID.PlceInc: return MP.dPosn[(int)EN_POSN_ID.PlceInc];
                case EN_COMD_ID.Sply:    return MP.dPosn[(int)EN_POSN_ID.Sply];
                case EN_COMD_ID.Stck:    return MP.dPosn[(int)EN_POSN_ID.Stck];
                case EN_COMD_ID.CalPos:  return MP.dPosn[(int)EN_POSN_ID.CalPos];

                case EN_COMD_ID.User1:   return MP.dPosn[(int)EN_POSN_ID.User1];
                case EN_COMD_ID.User2:   return MP.dPosn[(int)EN_POSN_ID.User2];
                case EN_COMD_ID.User3:   return MP.dPosn[(int)EN_POSN_ID.User3];
                case EN_COMD_ID.User4:   return MP.dPosn[(int)EN_POSN_ID.User4];
                case EN_COMD_ID.User5:   return MP.dPosn[(int)EN_POSN_ID.User5];
                case EN_COMD_ID.User6:   return MP.dPosn[(int)EN_POSN_ID.User6];
                case EN_COMD_ID.User7:   return MP.dPosn[(int)EN_POSN_ID.User7];
                case EN_COMD_ID.User8:   return MP.dPosn[(int)EN_POSN_ID.User8];
                case EN_COMD_ID.User9:   return MP.dPosn[(int)EN_POSN_ID.User9];
                case EN_COMD_ID.User10:  return MP.dPosn[(int)EN_POSN_ID.User10];
                case EN_COMD_ID.User11:  return MP.dPosn[(int)EN_POSN_ID.User11];
                case EN_COMD_ID.User12:  return MP.dPosn[(int)EN_POSN_ID.User12];
                case EN_COMD_ID.User13:  return MP.dPosn[(int)EN_POSN_ID.User13];
                case EN_COMD_ID.User14:  return MP.dPosn[(int)EN_POSN_ID.User14];
                case EN_COMD_ID.User15:  return MP.dPosn[(int)EN_POSN_ID.User15];
                case EN_COMD_ID.User16:  return MP.dPosn[(int)EN_POSN_ID.User16];
                case EN_COMD_ID.User17:  return MP.dPosn[(int)EN_POSN_ID.User17];
                case EN_COMD_ID.User18:  return MP.dPosn[(int)EN_POSN_ID.User18];
                case EN_COMD_ID.User19:  return MP.dPosn[(int)EN_POSN_ID.User19];
                case EN_COMD_ID.User20:  return MP.dPosn[(int)EN_POSN_ID.User20];
                case EN_COMD_ID.User21:  return MP.dPosn[(int)EN_POSN_ID.User21];
                case EN_COMD_ID.User22:  return MP.dPosn[(int)EN_POSN_ID.User22];
                case EN_COMD_ID.User23:  return MP.dPosn[(int)EN_POSN_ID.User23];
                case EN_COMD_ID.User24:  return MP.dPosn[(int)EN_POSN_ID.User24];
                case EN_COMD_ID.User25:  return MP.dPosn[(int)EN_POSN_ID.User25];
                case EN_COMD_ID.User26:  return MP.dPosn[(int)EN_POSN_ID.User26];
                case EN_COMD_ID.User27:  return MP.dPosn[(int)EN_POSN_ID.User27];
                case EN_COMD_ID.User28:  return MP.dPosn[(int)EN_POSN_ID.User28];
                case EN_COMD_ID.User29:  return MP.dPosn[(int)EN_POSN_ID.User29];
                case EN_COMD_ID.User30:  return MP.dPosn[(int)EN_POSN_ID.User30];

                default: return GetTrgPos();
            }
        }
        //--------------------------------------------------------------------------
        public int         GetPairMstrAxe   ()
        {
            return -1;
        }
        //--------------------------------------------------------------------------
        public int         GetPairAxe       ()
        {
            int iSubAxis;
            iSubAxis = cDEF.MOTR[m_iObjNo].m_iPairAxisNo;
            return iSubAxis;
        }
        //--------------------------------------------------------------------------
        public bool        GetAbsAxe        ()
        {
            return false;
        }
        //Position Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double      GetPreTrgPos     (             )                           //Get Target  Position.
        {
            if(!m_bInitAxis                                     ) return 0;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return 0; 
            return _fGPreTrgPos();
        }
        //--------------------------------------------------------------------------
        public double      GetTrgPos        (             )                           //Get Target  Position.
        {
            if(!m_bInitAxis                                     ) return 0;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return 0; 
            return _fGTrgPos();
        }
        //--------------------------------------------------------------------------
        public double      GetCmdPos        (             )                           //Get Command Position.
        {                      
            if(!m_bInitAxis                                     ) return 0;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return 0; 
            //Local Var.
            double cmdPos;

            //Get.
            cmdPos = _fGetCmdPos(m_iObjNo);
            cmdPos = CalPulseToPos(cmdPos);

            //Return.
            return cmdPos;
        }
        //--------------------------------------------------------------------------
        public double      GetEncPos        (             )                           //Get Encoder Position.
        {
            //Local Var.
            double encPos;

            if(!m_bInitAxis                                     ) return 0;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return 0; 

            bool IsServo = m_iMotrType == (int)EN_MOTR_TYPE.Rotary || m_iMotrType == (int)EN_MOTR_TYPE.Linear;
            if (IsServo && m_iNoUseEnc == 0) encPos = _fGetEncPos(m_iObjNo);
            else 
            {
                encPos = _fGetCmdPos(m_iObjNo);
            }

            encPos = CalPulseToPos(encPos);
            //Return.
            return encPos;
        }
        //--------------------------------------------------------------------------
        public double      GetAbsEncPos     (             )                           //Get Encoder Position with Offset.
        {
            return (GetEncPos() + m_dHomeOff); 
        }
        //--------------------------------------------------------------------------
        public double      GetErrPos        (             )                           //Get Control Error Position.
        {
            return Math.Abs(GetCmdPos() - GetEncPos());
        }
        //--------------------------------------------------------------------------
        public void        ClearPos         (double Pos  = 0.0)                           //Clear Position.
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            double dPulse = (Pos * m_dCoef);
            _fClearPos(m_iObjNo, dPulse);
			SetPos(Pos);
        }
        //--------------------------------------------------------------------------
        public bool        SetPos           (double Pos  = 0.0)                           //Set Any Position.
        {
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            _fSetTrgPos   (Pos);
            _fSetPreTrgPos(Pos);

            double dPulse = (Pos * m_dCoef);
            return _fSetPos(m_iObjNo, dPulse);
        }
        //--------------------------------------------------------------------------
        public void        SetPosEncToCmd   (             )
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetPosEncToCmd(m_iObjNo);
        }
        //--------------------------------------------------------------------------
        public void        SetPosEncToTrg   (             )
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            double encPos = _fGetEncPos(m_iObjNo);
            _fSetPreTrgPos(GetTrgPos());
            _fSetTrgPos(CalPulseToPos(encPos));
        }
        //--------------------------------------------------------------------------
        public void        SetPosCmdToTrg   (             )
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            double cmdPos = _fGetCmdPos(m_iObjNo);
            _fSetPreTrgPos(GetTrgPos());
            _fSetTrgPos(CalPulseToPos(cmdPos));
        }
        //--------------------------------------------------------------------------
        public void        SetPosEncToAll   (             )
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            double encPos = _fGetEncPos(m_iObjNo);

            //Set Command.
            _fSetPos(m_iObjNo, encPos);
        }
        //--------------------------------------------------------------------------
        public double      GetOrgEncPos     ()
        {
            double dOrgPos ;
            if (m_iMaker != (int)EN_MOTR_MAKER.AJIN && m_iMaker != (int)EN_MOTR_MAKER.AJECAT) return 0.0;
            dOrgPos = _fGOrgEncPos();
            return CalPulseToPos(dOrgPos);
        }     
        //--------------------------------------------------------------------------   
        //Limit Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //Get.
        public double GetMaxPos()            { return MP.MaxPos; }
        public double GetMaxVel()            { return MP.MaxVel; }
        public double GetMaxAcc()            { return MP.MaxAcc; }
        public double GetMinPos()            { return MP.MinPos; }
        public double GetMinVel()            { return MP.MinVel; }
        public double GetMinAcc()            { return MP.MinAcc; }
            //Set.                             
        public void   SetMaxPos(double Data) { MP.MaxPos = Data; }
        public void   SetMaxVel(double Data) { MP.MaxVel = Data; }
        public void   SetMaxAcc(double Data) { MP.MaxAcc = Data; }
        public void   SetMinPos(double Data) { MP.MinPos = Data; }
        public void   SetMinVel(double Data) { MP.MinVel = Data; }
        public void   SetMinAcc(double Data) { MP.MinAcc = Data; }
        public void   SetMaxMin(double MaxPos, double MaxVel, double MaxAcc, double MinPos, double MinVel, double MinAcc)
        {
            MP.MaxPos = MaxPos;
            MP.MaxVel = MaxVel  ;
            MP.MaxAcc = MaxAcc;
            MP.MinPos = MinPos;
            MP.MinVel = MinVel;
            MP.MinAcc = MinAcc;
        }
        //--------------------------------------------------------------------------
        //Move Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetServo(bool On)
        {
            
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            if (m_iNoUseMotr==1) SetHomeEnd(true);

            if (cDEF.MOTR[m_iObjNo].IsPairSlave()) return;

            Reset();
            //Servo On/Off.
            if (m_iMotrType == (int)EN_MOTR_TYPE.StepOriental)
            {
                if (m_iSONLevel==1) { _fSetServo(m_iObjNo, (!On) ? 1:0); }
                else                { _fSetServo(m_iObjNo, ( On) ? 1:0); }
            }
            else
            {
                if (m_iSONLevel==1) { _fSetServo(m_iObjNo,  On ? 1 : 0); }
                else                { _fSetServo(m_iObjNo,  On ? 0 : 1); }
            }

            if (m_iMaker != (int)EN_MOTR_MAKER.AJIN && m_iMaker != (int)EN_MOTR_MAKER.AJECAT) Stop();
        }
        //--------------------------------------------------------------------------
        public void SetAlarm(bool on)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            _fSetAlarm(m_iObjNo, (on)? 1: 0);
        }   
        //--------------------------------------------------------------------------   
        public bool        EmrgStop         (  )
        {
            bool isOk = false;

            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            //Reset Jog Status.
            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //E-Stop.
            isOk = _fEmrgStop(m_iObjNo);

            //if (isOk) {
            //    SetPosEncToAll();
            //    }
            return isOk;
        } 
        //--------------------------------------------------------------------------       
        public bool        Stop             (bool   DecStop = false , double DecTime = 0.1)
        {
            bool isOk = false;

            if(!m_bInitAxis                                     ) return true;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return true; 

            //Reset Jog Status.
            m_bJogN = false;
            m_bJogP = false;


            if (m_bUseTorque)
            {
                if (m_bLtTorqueN || m_bLtTorqueP)
                {
                    MoveTorqueStop();
                    return true;
                }
            }
            //Stop.
            isOk = _fStop(m_iObjNo, DecStop, DecTime);

            //if (isOk) {
            //    SetPosEncToAll();
            //    }

            return isOk;
        }
        //--------------------------------------------------------------------------
        public bool MoveJog(bool Dir, EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal)
        {
            //Move JOG.

            if (iSPD == EN_MOTR_VEL.Normal)
            {
                if (m_bJogSpd)
                {
                    if (Dir) { return MoveJogP(MP.dVel[(int)EN_MOTR_VEL.HJog]); }
                    else     { return MoveJogN(MP.dVel[(int)EN_MOTR_VEL.HJog]); }
                }
                else
                {
                    if (Dir) { return MoveJogP(MP.dVel[(int)EN_MOTR_VEL.LJog]); }
                    else     { return MoveJogN(MP.dVel[(int)EN_MOTR_VEL.LJog]); }
                }
            }
            else
            {
                if (iSPD < 0 || (int)iSPD >= vDEF.MAX_SPED) return false;
                if (Dir) { return MoveJogP(MP.dVel[(int)iSPD]); }
                else     { return MoveJogN(MP.dVel[(int)iSPD]); }
            }
        }

        public bool MoveRPM(double dVel)
        {
            return _fMoveJogP(m_iObjNo, dVel, MP.dAcc[(int)EN_MOTR_VEL.Work], MP.dDec[(int)EN_MOTR_VEL.Work]);

            //return MoveJogP(dVel); 
        }

        public bool MoveJog(bool Dir, double dVel)
        {
            //Move JOG.
            if (Dir) { return MoveJogP(dVel); }
            else     { return MoveJogN(dVel); }
        }
        //--------------------------------------------------------------------------
        public bool        MoveHome         (                                         )
        {
            //Local Val
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            //Check use.

            if (m_iHomeType == (int)EN_HOME_TYPE.DataSet) _fSetPos(m_iObjNo, 0.0);
            if (m_iNoUseMotr==1 || m_iNoUseHomeSen == 1)
            {
                _fSetPos(m_iObjNo, CalPosToPulse(m_dHomeOff));
                _fSetHomeEnd(true);
                return true;
            }
            //Reset Jog Status.
            m_bJogN = false;
            m_bJogP = false;

            if (GetHomeEnd()) return true;

            //Move Home Start
            _fMoveHome(m_iObjNo, MP.dVel[(int)EN_MOTR_VEL.Home], MP.dAcc[(int)EN_MOTR_VEL.Work], MP.dDec[(int)EN_MOTR_VEL.Work], CalPosToPulse(m_dHomeOff), m_dHomeOff);

            if (m_iMotrKind == (int)EN_MOTR_KIND.ABS || m_iHomeType == (int)EN_HOME_TYPE.DataSet)
            {
                _fSetPreTrgPos(GetEncPos());
                _fSetTrgPos(GetEncPos());
                
            }
            //Chekc Home End
            return (GetHomeStep() == 0);
        }
        //--------------------------------------------------------------------------
        public bool        MoveHomeForce    ()
        {
            //Local Val
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 
            if(m_iMaker != (int)EN_MOTR_MAKER.AJIN && m_iMaker != (int)EN_MOTR_MAKER.AJECAT) return false;
            //Check use.
            if (m_iNoUseMotr==1 || m_iNoUseHomeSen == 1)
            {
                _fSetHomeEnd(true);
                return true;
                }

            //Reset Jog Status.
            m_bJogN = false;
            m_bJogP = false;

            if(GetHomeEnd()) return true;

            //Move Home Start
            _fSetMoveHomeForce(m_iObjNo, MP.dVel[(int)EN_MOTR_VEL.Home], MP.dAcc[(int)EN_MOTR_VEL.Work]);

            if (m_iMotrKind == (int)EN_MOTR_KIND.ABS) {
                _fSetPreTrgPos(GetEncPos());
                _fSetTrgPos(GetEncPos());

            }
            //Chekc Home End
            return (GetHomeStep() == 0);
        }
        //--------------------------------------------------------------------------
        public bool        MoveOverrideVel  (double Pos , double Vel , double Acc , double Dec, double dOverridePos, double dOverrideVelocity)
        {
            //Local Var.
            bool   Receive   ;
            double tPos ,tOverridePos;


            //Apply Coef.
            tPos         = CalPosToPulse(Pos         );
            tOverridePos = CalPosToPulse(dOverridePos);

            //Set Gain.
                //Speed.
            if (m_dSpedGain < 0.01) m_dSpedGain = 0.01;
            if (m_dSpedGain > 2.0 ) m_dSpedGain = 2.0 ;
            Vel *= m_dSpedGain;
            m_dSpedGain = 1.0;
                //Acc.
            if (m_dAccGain  < 0.01 ) m_dAccGain  = 0.01;
            if (m_dAccGain  > 5.0  ) m_dAccGain  = 5.0 ;
            Acc *= m_dAccGain;
            Dec *= m_dAccGain;
            m_dAccGain = 1.0;


            //Set Speed Ratio.
            switch ((EN_SPED_RATIO)m_iSpedRatio) {
                case EN_SPED_RATIO.sr100: Vel *= 1.0; break;
                case EN_SPED_RATIO.sr90 : Vel *= 0.9; break;
                case EN_SPED_RATIO.sr80 : Vel *= 0.8; break;
                case EN_SPED_RATIO.sr70 : Vel *= 0.7; break;
                case EN_SPED_RATIO.sr60 : Vel *= 0.6; break;
                case EN_SPED_RATIO.sr50 : Vel *= 0.5; break;
                case EN_SPED_RATIO.sr40 : Vel *= 0.4; break;
                case EN_SPED_RATIO.sr30 : Vel *= 0.3; break;
                case EN_SPED_RATIO.sr20 : Vel *= 0.2; break;
                case EN_SPED_RATIO.sr10 : Vel *= 0.1; break;
                default                 : Vel *= 1.0; break;
                }

            //Check Motor Limit.
            if (Dec <= 0       ) { Dec = Acc ;                 }
            if (!GetHomeEnd()  ) { Stop(true); return false;   }
            if (Pos > MP.MaxPos) { Stop(true); return false;   }
            if (Pos < MP.MinPos) { Stop(true); return false;   }
            if (Vel > MP.MaxVel) { Vel = MP.MaxVel;            }
            if (Vel < MP.MinVel) { Vel = MP.MinVel;            }
            if (Acc > MP.MaxAcc) { Acc = MP.MaxAcc;            }
            if (Acc < MP.MinAcc) { Acc = MP.MinAcc;            }
            if (Dec > MP.MaxAcc) { Dec = MP.MaxAcc;            }
            if (Dec < MP.MinAcc) { Dec = MP.MinAcc;            }
            if (dOverrideVelocity > MP.MaxVel) { dOverrideVelocity = MP.MaxVel; }
            if (dOverrideVelocity < MP.MinVel) { dOverrideVelocity = MP.MinVel; }

            if(!m_bInitAxis                                     ) return true;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return true; 
            if(m_iNoUseMotr==1                                  ) return true;
            if(m_iMaker != (int)EN_MOTR_MAKER.AJIN && m_iMaker != (int)EN_MOTR_MAKER.AJECAT) return true;

            //Reset Jog Status.
            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //Move Motor.
            if (!GetOk()) return false;
            else {
                Receive = false;
                if (_fGStop()) {
                    //dwStrtTime = GetTickTime_us();
                    //if(m_iMaker == (int)EN_MOTR_MAKER.AJIN) {
                    //    Receive = AJIN.MoveOverrideVel(m_iObjNo, tPos, Vel, Acc, Dec, tOverridePos, dOverrideVelocity);
                    //    }
                    //dwScanTime = GetTickTime_us() - dwStrtTime;
                    if (!m_bRising) {
                        //dwMoveStrtTime = GetTickTime();
                        m_bRising = true;
                        }
                    }
                else {
                    if(m_bUseOverride) {
                        //Receive = MoveOverride(m_iObjNo, tPos, Vel, Acc, Dec);
                        }
                    }

                if (Receive) { 
                    _fSetPreTrgPos(GetTrgPos());
                    _fSetTrgPos(Pos);
                    }
                }

            //Timer Clear.
            m_MoveInpTimer.Clear();

            //Check InPos.
            return GetStop(true , m_dInPos);
        }
        //--------------------------------------------------------------------------
        public bool        MoveJogP         (double Vel)
        {
            //Check Status.
            if (Vel     <= 0) return false;
            if (GetAlarm  ()) return false;
            if (!GetServo ()) return false;

            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            //Set Jog Status.
            m_bJogP = true;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //
            return _fMoveJogP(m_iObjNo, Vel, MP.dAcc[(int)EN_MOTR_VEL.Work], MP.dDec[(int)EN_MOTR_VEL.Work]);
        }
        //--------------------------------------------------------------------------
        public bool        MoveJogN         (double Vel                                   )
        {
            //Check Status.
            if (m_iNoUseMotr==1) return false;
            if (Vel <= 0       ) return false;
            if (GetAlarm ()    ) return false;
            if (!GetServo()    ) return false;


            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            //Set Jog Status.
            m_bJogN    = true ;
            m_bTorqueN = false;
            m_bTorqueP = false;

            return _fMoveJogN(m_iObjNo, Vel, MP.dAcc[(int)EN_MOTR_VEL.Work], MP.dDec[(int)EN_MOTR_VEL.Work]);
        }
        //--------------------------------------------------------------------------
        public bool        MoveA            (double Pos , double Vel = 20.0, double Acc = 0.3 , double Dec = 0.0)  //abs move with mm.
        {
            //Local Var.
            bool   Receive   ;
            double tPos      ;
            //Apply Coef.
            tPos = CalPosToPulse(Pos);

            //Set Gain.
                //Speed.
            if (m_dSpedGain < 0.01) m_dSpedGain = 0.01;
            if (m_dSpedGain > 2.0 ) m_dSpedGain = 2.0 ;
            Vel *= m_dSpedGain;
            m_dSpedGain = 1.0;
                //Acc.
            if (m_dAccGain  < 0.01 ) m_dAccGain  = 0.01;
            if (m_dAccGain  > 5.0  ) m_dAccGain  = 5.0 ;
            Acc *= m_dAccGain;
            Dec *= m_dAccGain;
            m_dAccGain = 1.0;


            //Set Speed Ratio.
            switch ((EN_SPED_RATIO)m_iSpedRatio)
            {
                case EN_SPED_RATIO.sr100: Vel *= 1.0; break;
                case EN_SPED_RATIO.sr90: Vel *= 0.9; break;
                case EN_SPED_RATIO.sr80: Vel *= 0.8; break;
                case EN_SPED_RATIO.sr70: Vel *= 0.7; break;
                case EN_SPED_RATIO.sr60: Vel *= 0.6; break;
                case EN_SPED_RATIO.sr50: Vel *= 0.5; break;
                case EN_SPED_RATIO.sr40: Vel *= 0.4; break;
                case EN_SPED_RATIO.sr30: Vel *= 0.3; break;
                case EN_SPED_RATIO.sr20: Vel *= 0.2; break;
                case EN_SPED_RATIO.sr10: Vel *= 0.1; break;
                default: Vel *= 1.0; break;
            }


            //Check Motor Limit.
            if (Dec <= 0       ) { Dec = Acc; }
            if (!GetHomeEnd()  ) { Stop(true); return false; }
            if (Pos > MP.MaxPos) { Stop(true); return false; }
            if (Pos < MP.MinPos) { Stop(true); return false; }
            if (Vel > MP.MaxVel) { Vel = MP.MaxVel; }
            if (Vel < MP.MinVel) { Vel = MP.MinVel; }
            if (Acc > MP.MaxAcc) { Acc = MP.MaxAcc; }
            if (Acc < MP.MinAcc) { Acc = MP.MinAcc; }
            if (Dec > MP.MaxAcc) { Dec = MP.MaxAcc; }
            if (Dec < MP.MinAcc) { Dec = MP.MinAcc; }

            if(!m_bInitAxis                                     ) return true;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return true; 
            if(m_iNoUseMotr==1                                  ) return true;

            //Reset Jog Status.
            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //bool bStop = *m_pbStop || fabs(GetEncPos() - Pos) < m_dInPos;

            //Move Motor.
            if (!GetOk()) return false;
            Receive = false;
            if (_fGStop()) 
            {
                //dwStrtTime = GetTickTime_us();
                Receive = _fMove(m_iObjNo, tPos, Vel, Acc, Dec, m_iSpedRatio);
                //dwScanTime = GetTickTime_us() - dwStrtTime;
                if (!m_bRising && Receive) 
				{ 
					m_bRising = true; 
					dStrtTime_MV = cDEF.TICK._GetTickTime(); 
				}
            }
            else if(m_bUseOverride) 
            {
                //if(isMakerAjin()) Receive = AJIN.MoveOverride(m_iObjNo, tPos, Vel, Acc, Dec);                
            }

            if (Receive) 
            {  
                _fSetPreTrgPos(GetTrgPos());
                _fSetTrgPos(Pos);
            }
            //Timer Clear.
            m_MoveInpTimer.Clear();
            //Check InPos.
            return GetStop(true , m_dInPos);
        }    
        //--------------------------------------------------------------------------   
        public bool        MoveP            (double Pos , double Vel = 20.0, double Acc = 0.3                   )  //Position over ride motion.
        {
            //Local Var.
            double cPos = GetCmdPos();

            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 
            if(m_iNoUseMotr==1                                  ) return true;

            //Reset Jog Status.
            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //Move Relative.
            return MoveP(Pos + cPos, Vel, Acc);
        }
        //--------------------------------------------------------------------------
        public bool        MoveR            (double Pos , double Vel = 20.0, double Acc = 0.3                   )  
        {
            //Local Var.
            double cPos = GetCmdPos();

            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 
            if(m_iNoUseMotr==1                                  ) return true ;

            //Reset Jog Status.
            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //Move Relative.
            return MoveA(Pos + cPos, Vel, Acc);
        }
        //--------------------------------------------------------------------------
        //Ring Conter Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void         SetRingCounter  (bool Enable , double Max, double Redu = 1.0)
        {
            //Local Var.
            double MaxCntr;

            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            //Set Var.
            m_dRingMax = Max * Redu;
            MaxCntr = (Max * Redu) * m_dCoef;
            //MaxCntr = Max * m_dCoef;

            _fSetRingCounter(m_iObjNo, Enable, MaxCntr);
        }
        //--------------------------------------------------------------------------
        //Min-Max
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool        CheckMinMax      (double P , double V , double A) //Value Checking.
        {
            //Check.
            if (m_iNoUseMotr==1 ) return true;
            if (!CheckMinMaxP(P)) return false;
            if (!CheckMinMaxV(V)) return false;
            if (!CheckMinMaxA(A)) return false;

            //Ok.
            return true;
        }
        //--------------------------------------------------------------------------
        public bool        CheckMinMaxP     (double P                      )
        {
            //Check.
            if (m_iNoUseMotr==1                       ) return true;
            if ((GetMaxPos() < P) || (GetMinPos() > P)) return false;

            //Ok.
            return true;
        }
        //--------------------------------------------------------------------------
        public bool        CheckMinMaxV     (double V                      )
        {
            //Check.
            if (m_iNoUseMotr==1                       ) return true;
            if ((GetMaxVel() < V) || (GetMinVel() > V)) return false;
            //Ok.
            return true;
        }
        //--------------------------------------------------------------------------
        public bool        CheckMinMaxA     (double A                      )
        {
            //Check.
            if (m_iNoUseMotr==1                       ) return true;
            if ((GetMaxAcc() < A) || (GetMinAcc() > A)) return false;

            //Ok.
            return true;
        }
        //--------------------------------------------------------------------------
        public bool        CheckMinMax      (int    P , int    V           ) //mpPara Cheking.
        {
            //Cheeck Spare ID.
            if (m_iNoUseMotr==1 ) return true;

            //Check.
            if (!CheckMinMaxP(P)) return false;
            if (!CheckMinMaxV(V)) return false;
            if (!CheckMinMaxA(V)) return false;

            //Ok.
            return true;
        }
        //--------------------------------------------------------------------------
        public bool        CheckMinMaxP     (int    P                      )
        {
            if (P < 0 || P >= vDEF.MAX_POSN) return false;
            if (m_iNoUseMotr==1 ) return true;
            return CheckMinMaxP(MP.dPosn[P]);
        }
        //--------------------------------------------------------------------------
        public bool        CheckMinMaxV     (int    V                      )
        {
            //Cheeck Spare ID.
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return true;
            if (m_iNoUseMotr==1) return true;
            if (V >= vDEF.MAX_SPED) return true;

            //Check.
            return CheckMinMaxV(MP.dVel[V]);
        }
        //--------------------------------------------------------------------------
        public bool        CheckMinMaxA     (int    V                      )
        {
            //Check Motor Axe.
            if (m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return true;

            if (V >= vDEF.MAX_SPED) return true;
            if (m_iNoUseMotr==1   ) return true;
            //Check.       
            return CheckMinMaxA(MP.dAcc[V]);
        }
        //--------------------------------------------------------------------------
        public bool        CheckPartMotr    (int Part                      )
        {
            return m_iMotionPart == Part;
        }
        //Comapare position.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool        CmprPos          (double Pos)
        {
            bool isOk;
            double dCortPos = 0.0;


            if (m_iNoUseMotr==1) return true;

            Pos -= dCortPos;

            isOk = (Math.Abs(GetEncPos() - Pos) < MP.dPosn[(int)EN_POSN_ID.InPos]) &&
                   (Math.Abs(GetCmdPos() - Pos) < MP.dPosn[(int)EN_POSN_ID.InPos]);

            if (isOk)
            {
                //SetGain(1.0, 1.0);
            }
            return isOk;
        }
        //--------------------------------------------------------------------------
        public bool CmprPos(double Pos, double InPos)
        {
            bool isOk;

            double dCortPos = 0.0;
            Pos -= dCortPos;

            if (m_iNoUseMotr==1) return true;

            isOk = (Math.Abs(GetEncPos() - Pos) < InPos) &&
                   (Math.Abs(GetCmdPos() - Pos) < InPos);

            if (isOk)
            {
                //SetGain(1.0, 1.0);
            }

            return isOk;
        }
        //--------------------------------------------------------------------------
        public int CmprPosStat(double Pos, bool ChkTrg = false)
        {
            double dGetPos = ChkTrg ? GetTrgPos() : GetEncPos();

            if (m_iNoUseMotr==1) return vDEF.CMPR_SAME;

            if (dGetPos < Pos  ) return vDEF.CMPR_SMAL;
            if (dGetPos == Pos ) return vDEF.CMPR_SAME;
            if (dGetPos > Pos  ) return vDEF.CMPR_LARG;

            return vDEF.CMPR_SAME;
        }
        //--------------------------------------------------------------------------
        public int         CmprPosStat      (double Pos    , double Offset                   , bool ChkTrg          = false              )
        {
            double dGetPos = ChkTrg ? GetTrgPos() : GetEncPos();

            if (m_iNoUseMotr==1           ) return vDEF.CMPR_SAME;

            if (dGetPos < (Pos - Offset)  ) return vDEF.CMPR_SMAL;
            if (dGetPos > (Pos + Offset)  ) return vDEF.CMPR_LARG;
            if ((dGetPos >= (Pos - Offset)) && (dGetPos <= (Pos + Offset))) return vDEF.CMPR_SAME;

            return vDEF.CMPR_SAME;
        }
        //--------------------------------------------------------------------------
        public int         CmprPosStat      (int    PosID                                    , bool ChkTrg          = false              )
        {
            double dGetPos = ChkTrg ? GetTrgPos() : GetEncPos();

            if (PosID < 0 || PosID >= vDEF.MAX_POSN) return 0;
            if (m_iNoUseMotr==1                    ) return 0;

            if ( dGetPos <  (MP.dPosn[PosID] - MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.CMPR_SMAL;
            if ( dGetPos >  (MP.dPosn[PosID] + MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.CMPR_LARG;
            if ((dGetPos >= (MP.dPosn[PosID] - MP.dPosn[(int)EN_POSN_ID.InPos])) && (dGetPos <= (MP.dPosn[PosID] + MP.dPosn[(int)EN_POSN_ID.InPos]))) return vDEF.CMPR_SAME;

            return vDEF.CMPR_SAME;
        }
        //--------------------------------------------------------------------------
        public bool CmprStep(int Step, EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1, EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.Index1)
        {
            int iPosnID;

            if (m_iNoUseMotr==1) return true;

                 if (FStep == EN_FSTEP_INDEX.Step1) iPosnID = (int)EN_POSN_ID.FSP1_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step2) iPosnID = (int)EN_POSN_ID.FSP2_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step3) iPosnID = (int)EN_POSN_ID.FSP3_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step4) iPosnID = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;
            else                                    iPosnID = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;

            if ((iPosnID + FIndex) < 0 || (iPosnID + (int)FIndex) >= vDEF.MAX_POSN) return false;

            if (m_iProcessDir==1) return CmprPos((MP.dPosn[iPosnID] + (double)Step * GetPitch(FStep) + GetCenPitch(Step, FStep)));
            else return CmprPos((MP.dPosn[iPosnID] - (double)Step * GetPitch(FStep) - GetCenPitch(Step, FStep)));
        }
        //--------------------------------------------------------------------------
        public bool CmprStepF(int Step, EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1, EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.Index1)
        {
            int iPosnID;

            if (m_iNoUseMotr==1) return true;

                 if (FStep == EN_FSTEP_INDEX.Step1) iPosnID = (int)EN_POSN_ID.FSP1_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step2) iPosnID = (int)EN_POSN_ID.FSP2_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step3) iPosnID = (int)EN_POSN_ID.FSP3_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step4) iPosnID = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;
            else                                    iPosnID = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;

            if ((iPosnID + FIndex) < 0 || (iPosnID + (int)FIndex) >= vDEF.MAX_POSN) return false;


            return CmprPos((MP.dPosn[iPosnID] + (double)Step * GetPitch(FStep) + GetCenPitch(Step, FStep)));
        }
        //--------------------------------------------------------------------------
        public bool CmprStepB(int Step, EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1, EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.Index1)
        {
            int iPosnID;

            if (m_iNoUseMotr==1) return true;

                 if (FStep == EN_FSTEP_INDEX.Step1) iPosnID = (int)EN_POSN_ID.FSP1_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step2) iPosnID = (int)EN_POSN_ID.FSP2_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step3) iPosnID = (int)EN_POSN_ID.FSP3_1 + (int)FIndex;
            else if (FStep == EN_FSTEP_INDEX.Step4) iPosnID = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;
            else                                    iPosnID = (int)EN_POSN_ID.FSP4_1 + (int)FIndex;

            if ((iPosnID + FIndex) < 0 || (iPosnID + (int)FIndex) >= vDEF.MAX_POSN) return false;


            return CmprPos((MP.dPosn[iPosnID] - (double)Step * GetPitch(FStep) - GetCenPitch(Step, FStep)));
        }
        //--------------------------------------------------------------------------
        public bool CmprArea(double Pos, double Area, bool NoChkEnc = false)
        {
            bool isOk;

            double dCortPos = 0.0;
            Pos -= dCortPos;


            if (m_iNoUseMotr==1) return true;

            if (NoChkEnc) isOk = (Math.Abs(GetTrgPos() - Pos) < Area) && Math.Abs(GetCmdPos() - Pos) < Area;
            else          isOk = (Math.Abs(GetEncPos() - Pos) < Area) && Math.Abs(GetCmdPos() - Pos) < Area;

            return isOk;
        }
        //--------------------------------------------------------------------------
        public int WhreTarget()
        {
            if (m_iNoUseMotr==1) return vDEF.MOTR_TARG_STP;

                 if (GetTrgPos() > (GetEncPos() + MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.MOTR_TARG_POS;
            else if (GetTrgPos() < (GetEncPos() - MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.MOTR_TARG_NEG;
            else if (GetTrgPos() == GetCmdPos())                                    return vDEF.MOTR_TARG_STP;

            return vDEF.MOTR_TARG_STP;
        }
        //--------------------------------------------------------------------------
        public EN_POSN_ID GetCmdToPosnId(EN_COMD_ID Comd)
        {
            EN_POSN_ID iPosnId = EN_POSN_ID.None;

            //
            //if (m_iNoUseMotr==1) return iPosnId;

            switch (Comd)
            {
                default: iPosnId = EN_POSN_ID.None; break;
                case EN_COMD_ID.User1:   iPosnId = EN_POSN_ID.User1;   break;
                case EN_COMD_ID.User2:   iPosnId = EN_POSN_ID.User2;   break;
                case EN_COMD_ID.User3:   iPosnId = EN_POSN_ID.User3;   break;
                case EN_COMD_ID.User4:   iPosnId = EN_POSN_ID.User4;   break;
                case EN_COMD_ID.User5:   iPosnId = EN_POSN_ID.User5;   break;
                case EN_COMD_ID.User6:   iPosnId = EN_POSN_ID.User6;   break;
                case EN_COMD_ID.User7:   iPosnId = EN_POSN_ID.User7;   break;
                case EN_COMD_ID.User8:   iPosnId = EN_POSN_ID.User8;   break;
                case EN_COMD_ID.User9:   iPosnId = EN_POSN_ID.User9;   break;
                case EN_COMD_ID.User10:  iPosnId = EN_POSN_ID.User10;  break;
                case EN_COMD_ID.User11:  iPosnId = EN_POSN_ID.User11;  break;
                case EN_COMD_ID.User12:  iPosnId = EN_POSN_ID.User12;  break;
                case EN_COMD_ID.User13:  iPosnId = EN_POSN_ID.User13;  break;
                case EN_COMD_ID.User14:  iPosnId = EN_POSN_ID.User14;  break;
                case EN_COMD_ID.User15:  iPosnId = EN_POSN_ID.User15;  break;
                case EN_COMD_ID.User16:  iPosnId = EN_POSN_ID.User16;  break;
                case EN_COMD_ID.User17:  iPosnId = EN_POSN_ID.User17;  break;
                case EN_COMD_ID.User18:  iPosnId = EN_POSN_ID.User18;  break;
                case EN_COMD_ID.User19:  iPosnId = EN_POSN_ID.User19;  break;
                case EN_COMD_ID.User20:  iPosnId = EN_POSN_ID.User20;  break;
                case EN_COMD_ID.User21:  iPosnId = EN_POSN_ID.User21;  break;
                case EN_COMD_ID.User22:  iPosnId = EN_POSN_ID.User22;  break;
                case EN_COMD_ID.User23:  iPosnId = EN_POSN_ID.User23;  break;
                case EN_COMD_ID.User24:  iPosnId = EN_POSN_ID.User24;  break;
                case EN_COMD_ID.User25:  iPosnId = EN_POSN_ID.User25;  break;
                case EN_COMD_ID.User26:  iPosnId = EN_POSN_ID.User26;  break;
                case EN_COMD_ID.User27:  iPosnId = EN_POSN_ID.User27;  break;
                case EN_COMD_ID.User28:  iPosnId = EN_POSN_ID.User28;  break;
                case EN_COMD_ID.User29:  iPosnId = EN_POSN_ID.User29;  break;
                case EN_COMD_ID.User30:  iPosnId = EN_POSN_ID.User30;  break;
                case EN_COMD_ID.Wait1 :  iPosnId = EN_POSN_ID.Wait1;   break;
                case EN_COMD_ID.Wait2 :  iPosnId = EN_POSN_ID.Wait2;   break;
                case EN_COMD_ID.Wait3 :  iPosnId = EN_POSN_ID.Wait3;   break;
                case EN_COMD_ID.FSP1_1:  iPosnId = EN_POSN_ID.FSP1_1;  break;
                case EN_COMD_ID.FSP1_2:  iPosnId = EN_POSN_ID.FSP1_2;  break;
                case EN_COMD_ID.FSP1_3:  iPosnId = EN_POSN_ID.FSP1_3;  break;
                case EN_COMD_ID.FSP1_4:  iPosnId = EN_POSN_ID.FSP1_4;  break;
                case EN_COMD_ID.FSP1_5:  iPosnId = EN_POSN_ID.FSP1_5;  break;
                case EN_COMD_ID.FSP1_6:  iPosnId = EN_POSN_ID.FSP1_6;  break;
                case EN_COMD_ID.FSP1_7:  iPosnId = EN_POSN_ID.FSP1_7;  break;
                case EN_COMD_ID.FSP1_8:  iPosnId = EN_POSN_ID.FSP1_8;  break;
                case EN_COMD_ID.FSP1_9:  iPosnId = EN_POSN_ID.FSP1_9;  break;
                                                                       
                case EN_COMD_ID.FSP2_1:  iPosnId = EN_POSN_ID.FSP2_1;  break;
                case EN_COMD_ID.FSP2_2:  iPosnId = EN_POSN_ID.FSP2_2;  break;
                case EN_COMD_ID.FSP2_3:  iPosnId = EN_POSN_ID.FSP2_3;  break;
                case EN_COMD_ID.FSP2_4:  iPosnId = EN_POSN_ID.FSP2_4;  break;
                case EN_COMD_ID.FSP2_5:  iPosnId = EN_POSN_ID.FSP2_5;  break;
                case EN_COMD_ID.FSP2_6:  iPosnId = EN_POSN_ID.FSP2_6;  break;
                case EN_COMD_ID.FSP2_7:  iPosnId = EN_POSN_ID.FSP2_7;  break;
                case EN_COMD_ID.FSP2_8:  iPosnId = EN_POSN_ID.FSP2_8;  break;
                case EN_COMD_ID.FSP2_9:  iPosnId = EN_POSN_ID.FSP2_9;  break;
                                                                       
                case EN_COMD_ID.FSP3_1:  iPosnId = EN_POSN_ID.FSP3_1;  break;
                case EN_COMD_ID.FSP3_2:  iPosnId = EN_POSN_ID.FSP3_2;  break;
                case EN_COMD_ID.FSP3_3:  iPosnId = EN_POSN_ID.FSP3_3;  break;
                case EN_COMD_ID.FSP3_4:  iPosnId = EN_POSN_ID.FSP3_4;  break;
                case EN_COMD_ID.FSP3_5:  iPosnId = EN_POSN_ID.FSP3_5;  break;
                case EN_COMD_ID.FSP3_6:  iPosnId = EN_POSN_ID.FSP3_6;  break;
                case EN_COMD_ID.FSP3_7:  iPosnId = EN_POSN_ID.FSP3_7;  break;
                case EN_COMD_ID.FSP3_8:  iPosnId = EN_POSN_ID.FSP3_8;  break;
                case EN_COMD_ID.FSP3_9:  iPosnId = EN_POSN_ID.FSP3_9;  break;
                                                                       
                case EN_COMD_ID.FSP4_1:  iPosnId = EN_POSN_ID.FSP4_1;  break;
                case EN_COMD_ID.FSP4_2:  iPosnId = EN_POSN_ID.FSP4_2;  break;
                case EN_COMD_ID.FSP4_3:  iPosnId = EN_POSN_ID.FSP4_3;  break;
                case EN_COMD_ID.FSP4_4:  iPosnId = EN_POSN_ID.FSP4_4;  break;
                case EN_COMD_ID.FSP4_5:  iPosnId = EN_POSN_ID.FSP4_5;  break;
                case EN_COMD_ID.FSP4_6:  iPosnId = EN_POSN_ID.FSP4_6;  break;
                case EN_COMD_ID.FSP4_7:  iPosnId = EN_POSN_ID.FSP4_7;  break;
                case EN_COMD_ID.FSP4_8:  iPosnId = EN_POSN_ID.FSP4_8;  break;
                case EN_COMD_ID.FSP4_9:  iPosnId = EN_POSN_ID.FSP4_9;  break;

                case EN_COMD_ID.Pick1:   iPosnId = EN_POSN_ID.Pick1;   break;
                case EN_COMD_ID.Pick2:   iPosnId = EN_POSN_ID.Pick2;   break;
                case EN_COMD_ID.Plce1:   iPosnId = EN_POSN_ID.Plce1;   break;
                case EN_COMD_ID.Plce2:   iPosnId = EN_POSN_ID.Plce2;   break;
                case EN_COMD_ID.PickInc: iPosnId = EN_POSN_ID.PickInc; break;
                case EN_COMD_ID.PlceInc: iPosnId = EN_POSN_ID.PlceInc; break;
                case EN_COMD_ID.Sply:    iPosnId = EN_POSN_ID.Sply;    break;
                case EN_COMD_ID.Stck:    iPosnId = EN_POSN_ID.Stck;    break;
                case EN_COMD_ID.CalPos:  iPosnId = EN_POSN_ID.CalPos;  break;
            }
            return iPosnId;
        }
        //--------------------------------------------------------------------------

        public double GetPosToCmdId(EN_COMD_ID Comd)
        {
            EN_POSN_ID iPosnID = GetCmdToPosnId(Comd);
            double dPosn   = MP.dPosn[(int)iPosnID];
            return dPosn;
        }
        //--------------------------------------------------------------------------

        public bool CmprPosByCmd(EN_COMD_ID Comd, double InPos = 0, int iStep = 0, int iIndex = 0)
        {
            //Local Val.
            bool isOk;
            double dPos;
            EN_POSN_ID iPosnId;


            if (m_iNoUseMotr==1) return true;

            //Comp Step
            if (Comd == EN_COMD_ID.FindStep1 ) return CmprStep (iStep, EN_FSTEP_INDEX.Step1, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStep2 ) return CmprStep (iStep, EN_FSTEP_INDEX.Step2, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStep3 ) return CmprStep (iStep, EN_FSTEP_INDEX.Step3, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStep4 ) return CmprStep (iStep, EN_FSTEP_INDEX.Step4, (EN_FPOSN_INDEX)iIndex);

            if (Comd == EN_COMD_ID.FindStepF1) return CmprStepF(iStep, EN_FSTEP_INDEX.Step1, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStepF2) return CmprStepF(iStep, EN_FSTEP_INDEX.Step2, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStepF3) return CmprStepF(iStep, EN_FSTEP_INDEX.Step3, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStepF4) return CmprStepF(iStep, EN_FSTEP_INDEX.Step4, (EN_FPOSN_INDEX)iIndex);

            if (Comd == EN_COMD_ID.FindStepB1) return CmprStepB(iStep, EN_FSTEP_INDEX.Step1, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStepB2) return CmprStepB(iStep, EN_FSTEP_INDEX.Step2, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStepB3) return CmprStepB(iStep, EN_FSTEP_INDEX.Step3, (EN_FPOSN_INDEX)iIndex);
            if (Comd == EN_COMD_ID.FindStepB4) return CmprStepB(iStep, EN_FSTEP_INDEX.Step4, (EN_FPOSN_INDEX)iIndex);

            //Comd Move
            iPosnId = GetCmdToPosnId(Comd);
            if (iPosnId == EN_POSN_ID.None) return false;
            if (InPos <= 0) InPos = MP.dPosn[(int)EN_POSN_ID.InPos];
            dPos = MP.dPosn[(int)iPosnId];

            double dCortPos = 0.0;
            dPos -= dCortPos;

            isOk = (Math.Abs(GetEncPos() - dPos) < InPos) &&
                   (Math.Abs(GetCmdPos() - dPos) < InPos);


            if (isOk)
            {
                //SetGain(1.0, 1.0);
            }
            return isOk;
        }
        //--------------------------------------------------------------------------
        //Status Check
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool        IsPairSlave      ()
        {
            for (int i = 0; i < cDEF.MOTR._iNumOfMotr; i++)
            {
                if (m_iPairAxisNo == m_iAxis) return true;
            }

            return false;
        }
        //--------------------------------------------------------------------------
        //Set Trigger
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void       SetTriggerPos     (double dPos                                     , double dTrigWidth = 100)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            //Apply Coef.
            double tPos = CalPosToPulse(dPos);
            //if(isMakerAjin()) AJIN.SetTriggerPos(m_iObjNo, tPos, dTrigWidth);
        }
        //--------------------------------------------------------------------------
        public bool       SetTriggerBlock   (double dStartPos, double dEndPos, double dPeriod, double dTrigWidth = 100)
        {
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 
            
            bool bRet = false;    
            //Apply Coef.
            double tStrtPos = CalPosToPulse(dStartPos);
            double tEndPos  = CalPosToPulse(dEndPos  );
            double tPeriod  = CalPosToPulse(dPeriod  );

            //if(isMakerAjin()) bRet = AJIN.SetTriggerBlock(m_iObjNo, tStrtPos, tEndPos, tPeriod, dTrigWidth);
            return bRet;    
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerOneShot (double dTrigWidth)
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            //if(isMakerAjin()) AJIN.SetTriggerOneShot(m_iObjNo, dTrigWidth);  
        }
        //--------------------------------------------------------------------------
        public void       SetTriggerReset   ()
        {
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 
            //if(isMakerAjin()) AJIN.SetTriggerReset(m_iObjNo); 
        }
        //--------------------------------------------------------------------------
        //Torque
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool       MoveTorqueP       (double dTorque, double dVel)
        {
            bool bRet = false; 

            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueP = true ;

            //if(isMakerAjin()) bRet = AJIN.MoveTorqueP(m_iObjNo, dTorque, dVel);
            return bRet;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueN       (double dTorque, double dVel)
        {
            bool bRet = false; 
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = true ;

            //if(isMakerAjin()) bRet = AJIN.MoveTorqueN(m_iObjNo, dTorque, dVel);
            return bRet;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorqueStop    ()
        {
            bool bRet = false; 
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //if(isMakerAjin()) bRet = AJIN.MoveTorqueStop(m_iObjNo);
            return bRet;
        }
		public bool  MoveTorque(int On, int Dir, double TorqueRatio, double VelRatio)
		{
            bool bRet = false; 
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

			//if (isMakerAjin()) bRet = AJIN.MoveTorque(m_iObjNo, On, Dir, TorqueRatio, VelRatio);
			if (isMakerSIMUL()) {
				if (On == 1) return _fMoveJogP(m_iObjNo, VelRatio, MP.dAcc[(int)EN_MOTR_VEL.Work], MP.dDec[(int)EN_MOTR_VEL.Work]);
				else         return _fStop(m_iObjNo, false, 0.1);
				}

			return bRet;
		}
        //--------------------------------------------------------------------------
        public bool       SetParamTorque    ()
        {
            bool bRet = false; 
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            //if(isMakerAjin()) bRet = AJIN.SetParamTorque(m_iObjNo);
            return bRet;
        }
        //--------------------------------------------------------------------------
        public bool       MoveTorque        (double dTorque, double Pos , double Vel = 20.0, double Acc = 0.3 , double Dec = 0.0)
        {
            bool bRet = false; 
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            m_bJogN    = false;
            m_bJogP    = false;
            m_bTorqueN = false;
            m_bTorqueP = false;

            //if(isMakerAjin()) bRet = AJIN.MoveTorque(m_iObjNo, dTorque, Pos, Vel, Acc, Dec);
            return bRet;
        }
        //--------------------------------------------------------------------------
        public bool       SetTorqueLimit    (double dTorque)
        {
            bool bRet = false; 
            if(!m_bInitAxis                                     ) return false;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return false; 

            //if(isMakerAjin()) bRet = AJIN.SetTorqueLimit(m_iObjNo, dTorque);
            return bRet;
        }
        //--------------------------------------------------------------------------
        //Update Motor Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void        Update           (               )
        {
            int iPairMstrAxe = -1;
            int iPairSubAxe  = -1;
            //
            if(!m_bInitAxis                                     ) return;
            if(m_iObjNo < 0 || m_iObjNo >= cDEF.MOTR._iNumOfMotr) return; 

            try { 
                dStrtTime_UP = cDEF.TICK._GetTickTime(); 
                 //Update
                _fUpdateAxe(m_iObjNo);
                

                iPairMstrAxe = GetPairMstrAxe();
                iPairSubAxe  = GetPairAxe();
                m_dCmdPos    = GetCmdPos();
                m_dEncPos    = GetEncPos();
                

                //If Jog On , set target to current command.
                if (m_bJogN || m_bJogP) { SetPosCmdToTrg(); }
                if (m_bJogN && !m_bLtJogN) m_bLtJogN = true;
                if (!m_bJogN && m_bLtJogN && _fGStop()) { m_bLtJogN = false; SetPosCmdToTrg(); }
                if (m_bJogP && !m_bLtJogP) m_bLtJogP = true;
                if (!m_bJogP && m_bLtJogP && _fGStop()) { m_bLtJogP = false; SetPosCmdToTrg(); }


                if (m_bTorqueN || m_bTorqueP) { SetPosCmdToTrg(); }
                if (m_bTorqueN && !m_bLtTorqueN) m_bLtTorqueN = true;
                if (!m_bTorqueN && m_bLtTorqueN && _fGStop()) { m_bLtTorqueN = false; SetPosCmdToTrg(); }
                if (m_bTorqueP && !m_bLtTorqueP) m_bLtTorqueP = true;
                if (!m_bTorqueP && m_bLtTorqueP && _fGStop()) { m_bLtTorqueP = false; SetPosCmdToTrg(); }
                

                //If Servo is off, Set target position to current positon.
                if (!GetOk() && (GetHomeStep() == 0))
                {
                    SetPosEncToTrg();
                    //If Servo is off, Set target position to current positon.
                    if (GetStop() && GetHomeEnd() && iPairMstrAxe >= 0)
                    {
                        if (Math.Abs(m_dCmdPos - m_dEncPos) < m_dInPos) { SetPosCmdToTrg(); }
                    }
                }

                //If Servo is off, Set target position to current positon.
                //if (GetStop() && GetHomeEnd())
                //{
                //    if (Math.Abs(m_dCmdPos - m_dEncPos) < m_dInPos) { 
                //        SetPosCmdToTrg();
                //        }
                //}                

                //Check Ring Counter.
                if (_fGRing() && _fGStop())
                {
                    if (!GetHomeEnd()) SetPosCmdToTrg();
                    else
                    {
                        if (_fMotionDone(m_iObjNo)) SetPosCmdToTrg();
                    }
                }
                //Jog Force Stop
                if (m_bJogP || m_bJogN)
                {
                    if (m_tJogStopDely.OnDelay(true, 2000))
                    {
                        EmrgStop();
                    }
                }
                else m_tJogStopDely.Clear();
                
                //Check Error.
                if (m_iNoUseMotr==0 && !m_bPrivAlarm   && GetAlarm ()) { Stop();           _fClearHomeEnd(); }
                if (m_iNoUseMotr==0 &&  m_bPrivServoOn && !GetServo()) { SetPosEncToCmd(); _fClearHomeEnd(); }
                m_bPrivAlarm    = GetAlarm();
                m_bPrivServoOn  = GetServo();
                //Check Limit.
                if (m_iNoUseMotr==0 && GetServo() && GetHomeEnd() && !GetStop() && (m_dEncPos > MP.MaxPos)) Stop(true);
                if (m_iNoUseMotr==0 && GetServo() && GetHomeEnd() && !GetStop() && (m_dEncPos < MP.MinPos)) Stop(true);
                

                if(isMakerAjin()) SetAbsOriginCycle();
                if (iPairSubAxe >= 0)
                {//m_iObjNo
                   cDEF.MOTR[iPairSubAxe].SetHomeEnd(GetHomeEnd());
                }
                dScanTime_UP = cDEF.TICK._GetTickTime() - dStrtTime_UP; 

            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxis. Update " + ex.ToString());
                return;
            }
        }
        //--------------------------------------------------------------------------
        //Read/Write Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(bool IsLoad, string Path, TMOTN_PARA TMP)
        {
            //Local Var.
            String AxisPath   ;
            string AxisPathCMP;
            String sFile1  = string.Format("[{0:00}]AXIS"  , m_iAxis);
            String sSection ;
            String sName    , sDesc;

            String[] sSpdDesc           = { "HomeSpeed"    , "AutoSpeed"    , "ManualSpeed"  ,
                                            "JogHighSpeed" , "JogLowSpeed"  , "UserSpeed1"   ,
                                            "UserSpeed2"   , "UserSpeed3"   , "UserSpeed4"   , "UserSpeed5" };

            String[] sTimeDesc          = {"TimeOut"      , "Stop_Delay"   , "" , "" , "" , "" , "" , "" , "" , "" };

            if (m_iAxis < 0 || m_iAxis >= cDEF.MOTR._iNumOfMotr) return;

            TIniUnit ini    = new TIniUnit();
            TIniUnit iniCMP = new TIniUnit();

            String PathCMP = Application.StartupPath +"\\Project";
            FNC.CreateDir(PathCMP);

            //Make Dir Common
            PathCMP += "\\Common";
            FNC.CreateDir(PathCMP);
            AxisPathCMP = PathCMP + "\\"+ sFile1 + ".Ini";

            //Make Dir.
            FNC.CreateDir(Path);
            AxisPath = Path + "\\"+ sFile1 + ".Ini";
            
            //Position.
            for (int i = 0 ; i < vDEF.MAX_POSN ; i++ ) 
            {
                sSection = string.Format("{0}_{1}_Position" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Posn #" + (i + 1);
                sDesc    = sSection + "_Desc #" + (i + 1);
                if (IsLoad) 
                {
                    ini   .Load(AxisPath   , sSection, sName, out TMP .dPosn[i]);
                    iniCMP.Load(AxisPathCMP, sSection, sName, out CMP .dPosn[i]);
                    if(TMP .iPosnKind[i] == (int)EN_POS_ID.COMM) TMP .dPosn[i] = CMP.dPosn[i];
                }
                else
                {
                    if(TMP .iPosnKind[i] == (int)EN_POS_ID.COMM) CMP.dPosn[i] = TMP .dPosn[i];
                    ini    .Save(AxisPath   , sSection, sName, TMP .dPosn [i]);
                    iniCMP .Save(AxisPathCMP, sSection, sName, CMP .dPosn [i]);   
                }
            }

            //Velocity.
            for (int i = 0 ; i < vDEF.MAX_SPED ; i++ ) 
            {
                sSection = string.Format("{0}_{1}_Velocity" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Vel #" + (i + 1);
                sDesc    = sSection + "_Desc #" + (i + 1);
                if (IsLoad) 
                {
                   ini.Load(AxisPath, sSection, sName, out TMP .dVel[i]); 
                }
                else        
                {
                    ini.Save(AxisPath, sSection, sName, TMP.dVel      [i]);
                }
            }

            //Acceleration.
            for (int i = 0 ; i < vDEF.MAX_SPED ; i++ ) 
            {   
                sSection = string.Format("{0}_{1}_Acceleration" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Acc #" + (i + 1);
                sDesc    = sSection + "_Desc #" + (i + 1);
                if (IsLoad) {
                    ini.Load(AxisPath, sSection, sName, out TMP .dAcc[i]); 

                    }
                else        {
                    ini.Save(AxisPath, sSection, sName, TMP.dAcc      [i]);
                }
            }


            //Deceleration.
            for (int i = 0 ; i < vDEF.MAX_SPED ; i++ ) {
                sSection = string.Format("{0}_{1}_Deceleration" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Dec #"  + (i + 1);
                sDesc    = sSection + "_Desc #" + (i + 1);
                if (IsLoad) {
                    ini.Load(AxisPath, sSection, sName, out TMP .dDec[i]); 
                }
                else        {
                    ini.Save(AxisPath, sSection, sName, TMP.dDec      [i]);
                }
            }

            //TimeSet.
            for (int i = 0 ; i < vDEF.MAX_DLAY ; i++ ) {                
                sSection = string.Format("{0}_{1}_TimeSet" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Time #" + (i + 1);
                sDesc    = sSection + "_Desc #" + (i + 1);
                if (IsLoad) {
                     ini.Load(AxisPath, sSection, sName, out TMP .dTime[i]); 
                }
                else        {
                    ini.Save(AxisPath, sSection, sName, TMP.dTime     [i]);
                }

            }
            if(IsLoad)
            {
                for (int i = 0 ; i < vDEF.MAX_SPED ; i++ ) {
                    if(TMP.dAcc[i]  <= 0) TMP .dAcc[i] = 0.5;
                    if(TMP.dDec[i]  <= 0) TMP .dDec[i] = 0.5;
                    if(TMP.dVel[i]  <= 0) TMP .dVel[i] = 30 ;
                }
                if(TMP.dPosn[(int)EN_POSN_ID.InPos]  <= 0) TMP .dPosn[(int)EN_POSN_ID.InPos] = 0.1 ;
            }


            ini     = null;
            iniCMP  = null;
        }
        //--------------------------------------------------------------------------
        public void LoadCommSpd(bool IsLoad)
        {//현재 속도 정보를 DEFAULT로 저장할때 사용한다. 
            //Local Var.
            String AxisPath;

            //JSM 160316
            String sFile1  = string.Format("[{0:00}]AXIS"  , m_iAxis);
            String sSection ;
            String sName    ;

            if (m_iAxis < 0 || m_iAxis >= cDEF.MOTR._iNumOfMotr) return;

            TIniUnit ini = new TIniUnit();

            //Make Dir.
            String Path = Application.StartupPath +"\\Project";

            //Make Dir.
            FNC.CreateDir(Path);
            Path += "\\Common";
            FNC.CreateDir(Path);
            AxisPath = Path + "\\"+ sFile1 + ".Ini";

            //Velocity.
            for (int i = 0 ; i < vDEF.MAX_SPED ; i++ ) 
            {
                sSection = string.Format("{0}_{1}_Velocity" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Vel #" + (i + 1);
                if (IsLoad) 
                {
                   ini.Load(AxisPath, sSection, sName, out CMP .dVel[i]);
                   MP.dVel[i] = CMP.dVel[i]; 
                }
                else        
                {
                    CMP.dVel[i] = MP.dVel[i]; 
                    ini.Save(AxisPath, sSection, sName, CMP.dVel      [i]);
                }
            }

            //Acceleration.
            for (int i = 0 ; i < vDEF.MAX_SPED ; i++ ) 
            {   
                sSection = string.Format("{0}_{1}_Acceleration" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Acc #" + (i + 1);
                if (IsLoad) {
                    ini.Load(AxisPath, sSection, sName, out CMP .dAcc[i]);
                    MP.dAcc[i] = CMP.dAcc[i]; 

                    }
                else  {
                    CMP.dAcc[i] = MP.dAcc[i]; 
                    ini.Save(AxisPath, sSection, sName, CMP.dAcc      [i]);
                }
            }


            //Deceleration.
            for (int i = 0 ; i < vDEF.MAX_SPED ; i++ ) {
                sSection = string.Format("{0}_{1}_Deceleration" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Dec #" + (i + 1);
                if (IsLoad) {
                    ini.Load(AxisPath, sSection, sName, out CMP .dDec[i]);
                    MP.dDec[i] = CMP.dDec[i];  
                }
                else        {
                    CMP.dDec[i] = MP.dDec[i]; 
                    ini.Save(AxisPath, sSection, sName, CMP.dDec      [i]);
                }
            }

            //TimeSet.
            for (int i = 0 ; i < vDEF.MAX_DLAY ; i++ ) {                
                sSection = string.Format("{0}_{1}_TimeSet" , m_sNameAxis, m_sName);    // sAxisName[m_iAxis] -> m_sName 변경
                sName    = sSection + "_Time #" + (i + 1);
                if (IsLoad) {
                     ini.Load(AxisPath, sSection, sName, out CMP .dTime[i]);
                     MP.dTime[i] = CMP.dTime[i];  
                }
                else        {
                    CMP.dTime[i] = MP.dTime[i]; 
                    ini.Save(AxisPath, sSection, sName, CMP.dTime     [i]);
                }

            }
            ini = null;
        }
        //--------------------------------------------------------------------------
        public void LoadOptn(bool IsLoad)
        {
            //Local Var.
            String sName   ;


            TIniUnit ini = new TIniUnit();

            //Make Dir.
            String Path = Application.StartupPath +"\\System";

            //Make Dir.
            FNC.CreateDir(Path);
            Path += "\\MasterMotor.INI";

            //Check File Exist.
            //MasterMotor 파일이 없을 경우 Defaul로 Master Option 설정.
            if (!FNC.FileExists(Path))
            {
                IsLoad = false;
                MsgBox.Error("Can not find the Master Motor configuration file.\\nPlease contact XXXXXXX Co., Ltd..");
            }

            sName = string.Format("{0:00}" , m_iAxis);


            //Load Master Options.
            if (IsLoad) {
                ini.Load(Path,"MotrType"        , sName, out m_iMotrType       );
                ini.Load(Path,"MotrKind"        , sName, out m_iMotrKind       );
                ini.Load(Path,"GR_Pulse"        , sName, out m_dGR_Pulse       );
                ini.Load(Path,"GR_Disp"         , sName, out m_dGR_Disp        );
                ini.Load(Path,"MinPosn"         , sName, out m_dMinPosn        );
                ini.Load(Path,"MaxPosn"         , sName, out m_dMaxPosn        );
                ini.Load(Path,"MaxVel"          , sName, out m_dMaxVel         );
                ini.Load(Path,"MinAcc"          , sName, out m_dMinAcc         );
                ini.Load(Path,"MinDec"          , sName, out m_dMinDec         );
                ini.Load(Path,"HomeOff"         , sName, out m_dHomeOff        );
                ini.Load(Path,"MaxPulse"        , sName, out m_dMaxPulse       );
                ini.Load(Path,"InPos"           , sName, out m_dInPos          );
                ini.Load(Path,"PartialPos"      , sName, out m_dPartialPos     );
                ini.Load(Path,"AbsOriginPos"    , sName, out m_dAbsOriginPos   );
                ini.Load(Path,"SONLevel"        , sName, out m_iSONLevel       );
                ini.Load(Path,"InpLevel"        , sName, out m_iInpLevel       );
                ini.Load(Path,"AlarmLevel"      , sName, out m_iAlarmLevel     );
                ini.Load(Path,"EncInputLevel"   , sName, out m_iEncInputLevel  );
                ini.Load(Path,"EndLimitEnable"  , sName, out m_iEndLimitEnable );
                ini.Load(Path,"HomeLevel"       , sName, out m_iHomeLevel      );
                ini.Load(Path,"ProcessDir"      , sName, out m_iProcessDir     );
                ini.Load(Path,"PulseOut"        , sName, out m_iPulseOut       );
                ini.Load(Path,"UseCam"          , sName, out m_iUseCam         );
                ini.Load(Path,"NoUseEnc"        , sName, out m_iNoUseEnc       );
                ini.Load(Path,"NoUseMotr"       , sName, out m_iNoUseMotr      );
                ini.Load(Path,"NoUseHomeSen"    , sName, out m_iNoUseHomeSen   );
                ini.Load(Path,"dRingMax"        , sName, out m_dRingMax        );
                ini.Load(Path,"MoveDirection"   , sName, out m_iMoveDirection  );
                ini.Load(Path,"PackType"        , sName, out m_iPackType       );
                ini.Load(Path,"IntpAxisNo"      , sName, out m_iIntpAxisNo     );
                ini.Load(Path,"PairAxisNo"      , sName, out m_iPairAxisNo     );
                ini.Load(Path,"AutoResp"        , sName, out m_iAutoResp       );
                ini.Load(Path,"HomeType"        , sName, out m_iHomeType       );
                ini.Load(Path,"HomeOptUse"      , sName, out m_iHomeOptUse     );
                ini.Load(Path,"EncPulse"        , sName, out m_iEncPulse       );
                ini.Load(Path,"ServoType"       , sName, out m_iServoType      );
                ini.Load(Path,"SetScurve"       , sName, out m_bSetScurve      );
                //ini.Load(Path,"ComPort"         , sName, out m_iComPort        );
                ini.Load(Path,"COMIStartIO"     , sName, out m_iCOMIStartIO    );
                ini.Load(Path,"RingCounter"     , sName, out m_bUseRingCounter );
                ini.Load(Path,"UseOverride"     , sName, out m_bUseOverride    );
                

                ini.Load(Path,"PosLimitLevel"   , sName, out m_iPosLimitLevel  );
                ini.Load(Path,"NegLimitLevel"   , sName, out m_iNegLimitLevel  );
                ini.Load(Path,"HomeSignal"      , sName, out m_iHomeSignal     );
                ini.Load(Path,"HomeZPhase"      , sName, out m_iHomeZPhase     );
                ini.Load(Path,"UseTorque"       , sName, out m_bUseTorque      );
                ini.Load(Path,"HomeDir"         , sName, out m_iHomeDir        );

                ini.Load(Path,"ToqWAddr"        , sName, out m_sToqWAddr       );
                ini.Load(Path,"ToqRAddr"        , sName, out m_sToqRAddr       );
                ini.Load(Path,"ComPort"         , sName, out m_sComPort        );

                }
            else {
                ini.Save(Path,"MotrType"        , sName, m_iMotrType       );
                ini.Save(Path,"MotrKind"        , sName, m_iMotrKind       );
                ini.Save(Path,"GR_Pulse"        , sName, m_dGR_Pulse       );
                ini.Save(Path,"GR_Disp"         , sName, m_dGR_Disp        );
                ini.Save(Path,"MinPosn"         , sName, m_dMinPosn        );
                ini.Save(Path,"MaxPosn"         , sName, m_dMaxPosn        );
                ini.Save(Path,"MaxVel"          , sName, m_dMaxVel         );
                ini.Save(Path,"MinAcc"          , sName, m_dMinAcc         );
                ini.Save(Path,"MinDec"          , sName, m_dMinDec         );
                ini.Save(Path,"HomeOff"         , sName, m_dHomeOff        );
                ini.Save(Path,"MaxPulse"        , sName, m_dMaxPulse       );
                ini.Save(Path,"InPos"           , sName, m_dInPos          );
                ini.Save(Path,"PartialPos"      , sName, m_dPartialPos     );
                ini.Save(Path,"AbsOriginPos"    , sName, m_dAbsOriginPos   );
                ini.Save(Path,"SONLevel"        , sName, m_iSONLevel       );
                ini.Save(Path,"InpLevel"        , sName, m_iInpLevel       );
                ini.Save(Path,"AlarmLevel"      , sName, m_iAlarmLevel     );
                ini.Save(Path,"EncInputLevel"   , sName, m_iEncInputLevel  );
                ini.Save(Path,"EndLimitEnable"  , sName, m_iEndLimitEnable );
                ini.Save(Path,"HomeLevel"       , sName, m_iHomeLevel      );
                ini.Save(Path,"ProcessDir"      , sName, m_iProcessDir     );
                ini.Save(Path,"PulseOut"        , sName, m_iPulseOut       );
                ini.Save(Path,"UseCam"          , sName, m_iUseCam         );
                ini.Save(Path,"NoUseEnc"        , sName, m_iNoUseEnc       );
                ini.Save(Path,"NoUseMotr"       , sName, m_iNoUseMotr      );
                ini.Save(Path,"NoUseHomeSen"    , sName, m_iNoUseHomeSen   );
                ini.Save(Path,"dRingMax"        , sName, m_dRingMax        );
                ini.Save(Path,"MoveDirection"   , sName, m_iMoveDirection  );
                ini.Save(Path,"PackType"        , sName, m_iPackType       );
                ini.Save(Path,"IntpAxisNo"      , sName, m_iIntpAxisNo     );
                ini.Save(Path,"PairAxisNo"      , sName, m_iPairAxisNo     );
                ini.Save(Path,"AutoResp"        , sName, m_iAutoResp       );
                ini.Save(Path,"HomeType"        , sName, m_iHomeType       );
                ini.Save(Path,"HomeOptUse"      , sName, m_iHomeOptUse     );
                ini.Save(Path,"EncPulse"        , sName, m_iEncPulse       );
                ini.Save(Path,"ServoType"       , sName, m_iServoType      );
                ini.Save(Path,"SetScurve"       , sName, m_bSetScurve      );
                //ini.Save(Path,"ComPort"         , sName, m_iComPort        );
                ini.Save(Path,"COMIStartIO"     , sName, m_iCOMIStartIO    );
                ini.Save(Path,"RingCounter"     , sName, m_bUseRingCounter );
                ini.Save(Path,"UseOverride"     , sName, m_bUseOverride    );
                ini.Save(Path,"PosLimitLevel"   , sName, m_iPosLimitLevel  );
                ini.Save(Path,"NegLimitLevel"   , sName, m_iNegLimitLevel  );
                ini.Save(Path,"HomeSignal"      , sName, m_iHomeSignal     );
                ini.Save(Path,"HomeZPhase"      , sName, m_iHomeZPhase     );

                ini.Save(Path,"UseTorque"       , sName, m_bUseTorque      );
                ini.Save(Path,"HomeDir"         , sName, m_iHomeDir        );

                ini.Save(Path,"ToqWAddr"        , sName, m_sToqWAddr       );
                ini.Save(Path,"ToqRAddr"        , sName, m_sToqRAddr       );
                ini.Save(Path,"ComPort"         , sName, m_sComPort        );
                }
            ini = null;
        }
        //Conversion.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double      CalPulseToPos    (double    Pulse)
        {
            //Local Var.
            double dPos;

            //Check Coef.
            if (m_dCoef <= 0) return 0;

            //
            dPos = ((double)Pulse / m_dCoef);

            //Check Cam.

            if (m_iUseCam == 0) return dPos;

            //Cal.
            dPos = CalCamDegToPos(dPos);

            //Return.
            return dPos;
        }
        //--------------------------------------------------------------------------
        public double         CalPosToPulse    (double Pos  )
        {
            //Local Var.
            double dPulse;

            //Check Coef.
            if (m_dCoef <= 0) return 0;

            //
            dPulse = (Pos * m_dCoef);

            //Check Cam.
            if ((m_dGR_Pulse == 1) && (m_dGR_Disp == 1)) return dPulse;
            else
            {
                if (m_iUseCam == 0) return (int)dPulse; //
            }

            //Cal.
            dPulse = CalCamPosToDeg(Pos) * m_dCoef;

            //Return.
            return dPulse;
        }
        //--------------------------------------------------------------------------
        public double CalCamDegToPos(double Deg)
        {
            //Local Var.
            double h = m_dPartialPos;
            double b = 360.0;
            double T = Deg;
            double s;

            //Check Max.
            if (Math.Abs(T) > b) T = b;

            //Cal.
            s = (h) * (Math.Sin(Math.PI * (T / b)));
            //Ok.
            return s;
        }
        //--------------------------------------------------------------------------
        public double      CalCamPosToDeg   (double Pos  )
        {
            //Local Var.
            double h = m_dPartialPos;
            double b = 360.0;
            double s = Pos;
            double T;

            //Check Max.
            //s = fabs(s);
            if (Math.Abs(s) > h) s = h;

            //Cal.
            T = (Math.Asin((s) / h) * b) / Math.PI;

            //Ok.
            return T;
        }
        //-------------------------------------------------------------------
        //Display.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void DispParamFrm()
        {
            FrmSetMotr frmSetMotr = new FrmSetMotr();
            frmSetMotr.m_iSelMotr = m_iAxis;
            frmSetMotr.ShowDialog();
            frmSetMotr = null;
        }
        //--------------------------------------------------------------------------
        //SET ABS POSITION
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ReqAbsOrigin()
        {
            if (m_iMaker != (int)EN_MOTR_MAKER.AJIN && m_iMaker != (int)EN_MOTR_MAKER.AJECAT)
            {
                m_iAbsStep = 0;
                return;
            }
            m_iAbsStep = 10;
        }
        //--------------------------------------------------------------------------
        public bool SetAbsOriginCycle()
        {
            switch (m_iAbsStep)
            {
                default: m_iAbsStep = 0; break;

                case 10:
                    SetServo(false);
                    m_iAbsStep++;
                    break;

                case 11:
                    if (GetServo())
                    {
                        SetServo(false);
                        break;
                    }
                    SetHomeLevel(m_iHomeLevel);
                    m_tAbsOrigin.Clear();
                    m_iAbsStep++;
                    break;

                case 12:
                    if (!m_tAbsOrigin.OnDelay(true, 5000)) return false;
                    SetAbsOrigin(GetOrgEncPos());
                    LoadOptn(false);
                    m_iAbsStep++;
                    break;

                case 13:
                    SetServo(true);
                    m_iAbsStep = 0;
                    return true;
            }
            return false;
        }
    }
}
