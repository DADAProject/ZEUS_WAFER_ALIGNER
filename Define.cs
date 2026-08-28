using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace eMachine
{
    #region "DEFINE_ENUM"    //Enum Define
    public enum EN_UI_TYPE : int
    {
        Light = 0,
        Dark = 1,
        EndOfId
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Operation Level.(사용자 Level )
    //===========================================================================
    public enum EN_LOGIN : int
    {
        Operator = 0,
        Engineer    ,
        Master      ,
        EndOfId
    };


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //MAP TYPE
    //===========================================================================
    public enum EN_MAP_TYPE : int
    {
        DATA    ,
        SCAN    ,
        WORK    ,
        EndOfId
    };

    //Sequence Status.
    //===========================================================================
    public enum EN_SEQ_STAT : uint
    {
        Init     = 0,
        Warning  = 1,
        Error    = 2,
        RunWarn  = 3,
        Running  = 4,
        Stop     = 5,
        DoorOpen = 6,
        WorkEnd  = 7,
        Idle     = 8, 

        EndofId 

    };
    public enum EN_PART_SEL : int
    {
        None = -2,
        All  = -1,
        P1       ,
        P2       ,
        P3       ,
        P4       ,
        P5       ,
        P6       ,
        P7       ,
        P8       ,
        P9       ,
        P10      ,
        P11      ,
        P12      ,
        P13
    };

    public enum EN_CAM_SEL : int
    {
        None   = -2,
        NoShow = -1,
        C1         ,
        C2         ,
        C3         ,
        C4         ,
        C5         ,
        C6         ,

    };

    //MESSAGE ID
    //===========================================================================
    public enum EN_MSG_KIND : int
    {
        UserShow = 100,
        UserModal,
    }

    //TCPIP ID 
    //===========================================================================
    //UserSet  - TCP ID 정의 
    public enum EN_INTERFACE_TYPE : int
    {
        SEARIAL,
        TCPIP,
        EndOfId
    }

    public enum EN_TCPIP : int
    {
        None = -1,
        TCP_IP_1,
        SECS_GEM,

        EndOfId
    };

    public enum EN_FTP : int
    {
        DOWNLOAD_1 = EN_TCPIP.EndOfId,
        DOWNLOAD_2,
        UPLOAD,
        ENGINEER,
        EndOfId
    };


    //TCP Status.
    public enum EN_TCP_STAT : int
    {
        NoSend  = -3,
        NoRecv  = -2,
        OffLine = -1,
        NoCon   = 0 ,
        Ok      = 1 ,
        Sended  = 2
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Serial Port No
    //===========================================================================
    public enum EN_RS232_PORT_NO : int
    {
        COM1,
        COM2,
        COM3,
        COM4,
        COM5,
        COM6,
        COM7,
        COM8,
        COM9,
        COM10,
        COM11,
        COM12,
        COM13,
        COM14,
        COM15,
        EndOfId
    };
    //
    //===========================================================================
    public enum EN_OBJ_KIND : int
    {
        None = -1, //  None
	    TOOL     , //  Tool Type
	    TRAY     , //  Tray Type
	    POCK     , //  Pock Type
	    STCK     , //  Stacker Type
	    TBLE     , //  Bib Type
	    WAFER    , //  Wafer Type
	    PLATE    , //  Plate Type
	    JIG		 ,
	    FOUP
    };

	//===========================================================================
	//Display Direction.
	public enum EN_MAP_DIR  : int
    { 
        None = -1  ,
	    Deg0       ,  //0
	    Deg90      ,  //90
	    Deg180     ,  //180
	    Deg270     ,  //270
	    Deg270_VMir,  //270 + Vert. Mirror.
	    Deg0_HMir  ,  //Horz. Mirrror.
	    Deg180_VMir   //180 + Vert. Mirror.
	};

    //===========================================================================
    //Move Status.		
    public enum EN_MOVE_STAT : int
    {
        NegMoved = -1,
        PosMoved = 0,
        NoMoved  = 1

    };
    //===========================================================================
    //Seperator Type
    public enum EN_SPER_TYPE : int
    {
        None = -1,
        SPLY,
        STCK
    };
    //Pick & Place
    public enum EN_PP_KIND : int
    {
        None = -1,
        PICK = 0 ,
        PLCE = 1 ,
    };
    //===========================================================================
    //Move Type
    public enum EN_MOVE_TYPE : int
    {
        None = -1,
        MOTR,
        CYLD
    };

    public enum EN_WAF_TYPE : int
    {
        None  = -1 ,
      //Inch4      ,
        Inch5      ,
        Inch6      ,
        Inch8      ,
        Inch12     ,

        EndOfId
    };
    public enum EN_WAF_THICK : int
    {
        None = -1,
        T130 = 0 , 
        T260 = 1 , 

        EndOfId
    };

            //Chip Stats.
    //===========================================================================
    //UserSet  - CHIP STATUS 정보 정의 
    public enum EN_CHIP_STAT  : int
    {
        None     , //Table도 없고 칩도 없는 경우.   
        Mask     ,
		Skip     ,
        Empty    , //Table는 있고 칩은 없는 경우.
        Mount    , //작업 대기 Chip만 있는 경우.
        Rslt     ,
        Fnsh     , //완료 상태 
        GScan    , //Scan Good
		FScan    , //Scan Fail
		PFail    , //PVI Fail.
        Start    ,
        EndOfId
    };
    public enum EN_CHIP_STAT2  : int
    {
        None     , //Table도 없고 칩도 없는 경우.   
        Mask     ,
		Skip     ,
        Empty    , //Table는 있고 칩은 없는 경우.
        Mount    , //작업 대기 Chip만 있는 경우.
		RsltGood ,
		RsltFail ,
        Fnsh     , //완료 상태 
        GScan    ,
        Start    ,
        EndOfId
    };
    //Chip Result.
    //===========================================================================
    //UserSet  - CHIP RESULT 정보 정의
    public enum EN_CHIP_RSLT  : int
    {
        None     ,//
        Good     ,//Test Result Good
        Fail     ,//Test Result Fail
        Wait     ,
        Skip     ,
        EndOfId
    };

    
    //Magazine status.
    //===========================================================================
    //UserSet  - Magazine Status 정의
    public enum EN_MGZ_STAT  : int
    {
        None = -1 ,//
        IDLE      , //
        WAIT      , //
        READY     , //
        EXEC      , //
        RESEV     , //
        COMP      , //

        EndOfId

    };
    //UserSet  - Wafer Status 정의
    public enum EN_WAFER_STAT  : int
    {
        None = 0 ,//
        Empty    , //Magazine는 있고 Plate(Wafer)은 없는 경우.
        Mask     , //Magazine는 있고 Plate(Wafer)은 있고 Scan 동작을 위한 Map
        Mount    , //Magazine는 있고 아무것도 안한 Plate(Wafer)만 있는 경우.
        Aligned  , //Align 완료
        Skip     , //Magazine도 있고 Plate(Wafer)도 있으나 작업하지 않는 Wafer.
        Fnsh     , //작업 완료 되었을때.
        Work     , //Magazine에 작업 중인 Wafer가 존재할때 
        Wait     , //Vision 검사 중...
        Fail     , //Align Fail 
        FnshAlign, //Foup에 넣을 수 있는 경우

        ReqLoad  , //Load 요청...
        
        EndOfId

        /*
         * Empty -> Mask -> [Scan] -> Mount -> [TR-Top] -> [Pre Align] -> Aligned 
         * -> [TR-Top] -> [Chuck] -> Mount ->  Work?? -> Fnsh -> [TR-Btm] -> [CST]
         * 
         * <Work 인 경우 Restart 시 Align 필요???>
         */

    };
    
    //Chip Find Mode.
    //===========================================================================
    public enum EN_FIND  : int
    {//UserSet - Find 함수 사용시 Mode 정의 
        Exist = -1, //존재 여부 만 확인.
        Mask      ,
	 	Skip      ,
	 	SkipMask  ,
        Empty     , //Table는 있고 칩은 없는 경우.
        Start     ,
        Mount     , //작업 대기 Chip만 있는 경우.			
        GScan     ,
        Rslt      , //검사 완료 상태 
        Fnsh      , //완료 상태 
        RsltGood  , 
        RsltFail  , 
    
	 	EndOfId
    };

    //Tool ID 
    //===========================================================================
    //UserSet  - TOOL ID 정의 
    public enum EN_TOOL_ID  : int
    {
        None = -1  ,

        EndOfId
    };     
 
    //WAF ID 
    //===========================================================================
    //UserSet  -  
    public enum EN_WAF_ID  : int
    {
        None   = -1 ,
        WAT     = 0, //Wafer Align Table
        //WTR    =  0 , //Wafer Transfer
        //ASM1        , //Attach 01
        //ASM2        , //Attach 02

        EndOfId
    };
    //===========================================================================
    //UserSet  -  
    public enum EN_ASM_ID  : int
    {
        None   = -1 ,
        ASM1   =  0 ,
        ASM2        ,

        EndOfId
    };     

    //===========================================================================
    //UserSet  -  
    //Magazine ID 
    public enum EN_MGZ_ID  : int
    {
        None   = -1 ,
        MGZ1   =  0 ,
        MGZ2        ,

        EndOfId
    };     

    public enum EN_WTR_WORK_AREA  : int
    {
        None     = -1 ,

        MGZ1     =  0 , //0//LPM01
        MGZ2          , //1//LPM02
        WAT           , //2//Align Table
        ASM1          , //3//ASM01
        ASM2          , //4//ASM02

        EndOfId
    }; 
    public enum EN_UMZ_WORK_MODE  : int
    {
        None        = -1 ,
        UnloadOnly  =  0 , //Unload Only
        LoadUnload       , //Load, Unload

        EndOfId
    }; 
    public enum EN_WTR_FORK  : int
    {
        None     = -1 ,
        A        =  0 , //  
        B             , //

        EndOfId
    }; 
    public enum EN_MGZ_SCAN_TYPE : int
    {
        Step     ,
        StepScan ,
        Line     ,

        EndOfId
    }
    public enum EN_TAPE_FEED_TYPE : int
    {
        SENSOR   , //Sensor.
        VOVERRIDE, //Velocity Override

        EndOfId
    }

        ///Define Vision ID
    //---------------------------------------------------------------------------
    public enum EN_CAM  : int 
    {
		None      = -1 ,
		
        WTB            ,

        EndofCam
    }

    public enum EN_WAFER_TYPE
    {
        NONE     ,
        BASE     ,
        SAWING   ,
        NOTCH    ,

        RINGFRAME,
    }
    public enum EN_LIGHT_CH : int
    {
        None      = -1,
        BACKLIGHT = 0,
        BACKLIGHT2 = 1,

        EndOfCh
    };
    public enum EN_RSLT_KIND  : int
	{
		None      = -2 ,
		All       = -1 ,
		WTB            ,
		UDV            ,
		MRT_A          , //Main Raotor Align Vision
		MRT_I1         , //Main Raotor Inspect #1 Vision
		MRT_I2         , //Main Raotor Inspect #2 Vision
		SRT_I1         ,
		BTV            , //Bottom Vision

		EndOfId
	}

    //Temp. Command ID.
    //===========================================================================
    public enum EN_TEMP_CMD : int {
        None = -1 ,
        SetSV     , //Set Setting Value.
        GetSV     , //Get Setting Value.
        GetPV     , //Get Process Value.
        
        SetBias   , //Set Setting Bias Value.
        GetBias   , //Get Setting Bias Value.

    };

    //Temp. Status
    //===========================================================================
    public enum EN_TEMP_STAT : int{
        rsStop ,
        rsRun
    };

    //Work Mode ID.
    //===========================================================================
    //UserSet  - SEQUENCE PART를 정의 
    public enum EN_WORK_MODE : int
    {
        UnKnown = -1,
        EndOfId
    };
    public enum EN_LH_AREA : int
    {
        UnKnown = -1,
        WAF,
        UDV,
        MRT,
        EndOfId
    };
    public enum EN_TH_AREA : int
    {
        UnKnown = -1,
        SRT,
        BTV,
        UT,
        NT, //Tray
        EndOfId
    };
    public enum EN_SEAL_KIND : int
    {
        STRT_SEAL,
        WORK_SEAL,
        FNSH_SEAL,
        STRT_ATTH,
        FNSH_ATTH
    };

    //Machine Part ID.
    //===========================================================================
    //UserSet  - SEQUENCE PART를 정의 
    //ex) XXX
    public enum  EN_SEQ_ID  : int 
    {
        ALL = -1, //    All.
        WAT     , //0// Wafer Align Table
        WTR     , //1// Wafer Transfer
        LPM1    , //2// Load Port Module 1
        LPM2    , //3// Load Port Module 2
       
        SYS     , //4// SYSTEM
    };
    //------------------------------------------------------------------------
    public enum EN_ION_ID  : int
    {
        ALIGN   = 0 , //
        MAIN_MC1    , //
        MAIN_MC2    , //
        LPM_PORT1   , //
        LPM_PORT2   , //
        
        EndOfId
    };     

    ///Define DSTB ID
    //---------------------------------------------------------------------------
    //UserSet - 간섭 조건 ID 정의 
    public enum  EN_DSTB_ID  : int 
    {
        DP_MOVE_WTR_X_DSTB_Y        , //WTR X축이 이동하기 위한 최소 Y축 위치.
        DP_WTR_Y_UNKNOWN_T_DSTB     , //WTR T축이 알 수 없는 위치시 WTR Y축 최대 이송 위치.
        DP_WTR_Z_MGZ_MAX_MOVE_POS   , //WTR Z축이 Magazine내에서 최대 이동할 수 있는 이송량.
        DP_MOVE_WTR_Z_L1_DSTB       , //WTR-Z가 LPM1에서 최대하강 위치
        DP_MOVE_WTR_Z_L2_DSTB       , //WTR-Z가 LPM2에서 최대하강 위치
        DP_MOVE_WTR_Z_AL_DSTB       , //WTR-Z가 Align에서 최대하강 위치
        DP_MOVE_WTR_Z_M1_DSTB       , //WTR-Z가 Machine1에서 최대하강 위치
        DP_MOVE_WTR_Z_M2_DSTB       , //WTR-Z가 Machine1에서 최대하강 위치


        EndOfId
    };

    public enum EN_VAC_ERR_TYPE : int
    {
        UnKnownPkg,
        ValveErr
    }

    public enum EN_G85_BIN_TYPE : int
    {
        DEC,   //decimal
        ASC    //ASCII	
    }

    public enum EN_LAMI_STEP : int
    {
        TapeFeeding , //Tape Feeding
        TapeRolling , //Tape Attach
        TapeCutPos  ,
        TapeCutting ,
        TapeEdgeCut ,
        TapeDeTape  ,

        EndOfId

    }
    public enum EN_WHRE_TFM : int
    {
        LEFT ,
        CUT  ,
        RGHT ,

        EndOfId
    }
    public enum EN_WHRE_WTR : int
    {
        UnKnown = -1 ,
        LPM1    ,
        LPM2    ,
        WAT     ,
        ASM1    ,
        ASM2    ,

        EndOfId
    }
    
    
    public enum EN_SAVE_TYPE : int
    {
        None = -1  , 
        Motor      , 
        OptEng     , 
        OptMaster  , 
        LoginSet   , 
        Actuator   , 
        OptNet     , 
        LampBuzz   , 
    };
    public enum EN_RUN_MODE : int
    {
        AUTO_RUN = 0, 
        MAN_RUN     , 
        DRY_RUN     
    }

    public enum EN_TEST_MODE : int
    {
        CHK_AWAY = 0,
        ALL_GOOD,
        ALL_FAIL,
        RANDOM,
    }
    public enum EN_ALIGN_TYPE : int
    {
        //Right -> Rear -> Left -> Front
        RightRearLeftFront = 0,
        LeftFrontRightRear = 1,

    }

    #endregion "DEFINE_ENUM"

    public struct ST_SAVE_INFO
    {
        public EN_SAVE_TYPE eType;
        public bool         bLoad;

        public ST_SAVE_INFO(bool load = false)
        {
            eType = EN_SAVE_TYPE.None;
            bLoad = load; 
        }
    }


    #region "public struct"
    public struct _TBLE_POSN {
        public double dX     ; //X in um.
        public double dY     ; //Y in um.
        public double dT     ; //Angle in degree.
        public double dX2    ; //X in um.
        public double dY2    ; //Y in um.
        public double dScore ; //Score
        public int    ipX    ; //
        public int    ipY    ; //Y in mm.
        public bool   bExist ;  
        public bool   bDouble;  

        public _TBLE_POSN(double val = 0.0)
        {
            dX      = 0.0  ;
            dY      = 0.0  ;
            dT      = 0.0  ;
            dX2     = 0.0  ;
            dY2     = 0.0  ;
            dScore  = 0.0  ;
            ipX     = 0    ;
            ipY     = 0    ;
            bExist  = false;
            bDouble = false;
        }

        public void ResetData()
        {
            dX  = 0.0;
            dY  = 0.0;
            dT  = 0.0;
            dX2 = 0.0;
            dY2 = 0.0;
            ipX = 0;
            ipY = 0;
        }
        public void Set(double x, double y, double degree, double score)
        {
            dX     = x     ;
            dY     = y     ;
            dT     = degree;
            dScore = score ;

            if(bExist) // 이미 있으면 더블
                bDouble = true;
            else
                bExist = true; // 없는 상태면 칩 exist 로 변경
        }
        public string GetData()
        {
            string sdata = string.Format($"Align Data : DX={dX}, DY={dY}, DT={dT}");
            return sdata; 
        }
    };
    public struct _CircleCodi
    {
        public double dX    ;
        public double dY    ;
        public double dR    ;
        public double dAngle;

        public _CircleCodi(double val = 0.0)
        {
            dX     = val;
            dY     = val;
            dAngle = val;
            dR     = val;
        }
    };

    public struct _G85_BIN_INFO {
        public string BinCode; //
        public string BinCount; //
        public string BinQuality; //
        public string BinDescription; //
    };		


    /***************************************************************************/
    /* Class: TAnalogPara                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TAnalogPara 
    {
        public double dMaxPres                   ; //Set Value.
        public double dCoefBreakPres             ; //Coefficient. Break(Blow).
        public double dCoefVacPres               ;
        public double[] dBreakOff      = new double[(int)EN_AI_CH.EndOfAI];  //Feedback.
        public double[] dVacPres       = new double[(int)EN_AI_CH.EndOfAI];
        public double[] dCurBreakPres  = new double[(int)EN_AO_CH.EndOfAO];
        public double[] dFlow          = new double[(int)EN_AI_CH.EndOfAI];

        //
        public double[] dCoeff         = new double[(int)EN_AI_CH.EndOfAI];
        public double[] dGetVal        = new double[(int)EN_AI_CH.EndOfAI];

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TAnalogPara()
        {
        }
        ~TAnalogPara() { }

        public TAnalogPara Copy()
        {
	   	return FNC.DeepClone(this) as TAnalogPara;
	   	//return this.MemberwiseClone() as TSORT_INFO;
        }
    };
    #endregion "public struct"
    /***************************************************************************/
    /* Class: vDEF                                                             */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    #region "public const "
    public static partial class vDEF 
    {        
        /***************************************************************************/
        /* Base Constants                                                          */
        /***************************************************************************/
        public const int    _FileVersion         = 100;

        //Max Screen Effects
        //===========================================================================
		public const int    MAX_SCREEN_EFF       = 10;

        //Running Mode.
        //===========================================================================
        public const int    AUTO_RUN             = 0; //Auto Run Mode
		public const int    MAN_RUN              = 1; //Manual & Debug Mode (Vision Use)
        public const int    DRY_RUN              = 2; //Dry Run (Vision No Use - Random Value)

        //Vacuum Mode
        //===========================================================================
        public const int    VAC_CHCK             = 0; //Vanuum Check.
        public const int    VAC_NONE             = 1; //NO Vanuum Check.

        //Result public const .
        //===========================================================================
        public const int    GOOD                 = 0;
        public const int    FAIL                 = 1;

        //Dual Part public const .
        //===========================================================================
        public const int    DualF1               = 0;
        public const int    DualF2               = 1;


        //Direction
        //===========================================================================
		public const int    drCW                 = 0;
		public const int    drCCW                = 1;


        //On/Off
        //===========================================================================
		public const int    OFF                = 0;
		public const int    ON                 = 1;

        //FWD / BWD
        //===========================================================================
		public const int    BWD                = 0;
		public const int    FWD                = 1;

        //
        //===========================================================================
		public const int    INNER                = 0;
		public const int    OUTER                = 1;


        //Part public const .
        //===========================================================================
        public const int    P_LEFT               = 0;
        public const int    P_RGHT               = 1;

        //Vision Mode.
        //===========================================================================
        public const int    CHCK_AWYS            = 0;
        public const int    MASK_AG              = 1;
        public const int    MASK_AF              = 2;
        public const int    MASK_RNDM            = 3;
        public const int    SKIP_VISN            = 4;

        //Vision.
        //===========================================================================

        
        //Vision Max Count
        //===========================================================================
        public const int    MAX_INSP_ITEM        = 30;
        public const int    MAX_INSP_KIND        = 10;
        public const int    MAX_INSP_RSLT        = 2 ;

        //Option Save Mode.
        //===========================================================================
        public const int    ENGR_OPT_SYS         = 0; //Option for System
        public const int    ENGR_OPT_WRK         = 1; //Option for Work
        public const int    ENGR_OPT_NET         = 2; //Option for NetWork
        public const int    ENGR_OPT_BUF         = 3; //Option for Buffer Offset

        //Motor.
        //===========================================================================
        public const int    MAX_POSN             = 100;
        public const int    MAX_SPED             = 10 ;
        public const int    MAX_DLAY             = 10 ;
        public const int    MAX_COMMON_POS       = 80 ;
                                                 
        //Current target position of motor.      
        public const int    MOTR_TARG_NEG        = -1;
        public const int    MOTR_TARG_STP        =  0;
        public const int    MOTR_TARG_POS        =  1;
                                                 
        //Result of position comparison.       
        public const int    CMPR_SMAL            = -1;
        public const int    CMPR_SAME            =  0;
        public const int    CMPR_LARG            =  1;
                                                 
        //NONE STEP ID.                          
        public const int    NONE_STEP            = -100;
        public const int    NONE_INDX            = -100;
        public const int    UNKNOWN_AREA         =  -1 ;
                            
        //Speed             
        public const int    SPD_LOW              =  0;
        public const int    SPD_HIGH             =  1;

        /***************************************************************************/
        /* MAX Constants                                                           */
        /***************************************************************************/           
        //===========================================================================
        public const int    MAX_STR_LEN          = 30;
        public const int    MAX_STR_EQPID_LEN    = 10;
        public const int    MAX_STR_LOTID_LEN    = 20;
        public const int    MAX_STR_WAFID_LEN    = 10;
        public const int    MAX_STR_OPERID_LEN   = 10;
        public const int    MAX_STR_RECPID_LEN   = 30;
        public const int    MAX_STR_TIME_LEN     = 16;  
		public const int    MAX_BIN_LENGTH       =  1;
        public const int    MAX_UPDATE_INFO      = 200;
        public const int    MAX_RS232_BUFFER     = 1024;
        
        //MAX System
        //===========================================================================
        public const int    MAX_ERR              = 150 ;
        public const int    MAX_WARN             = 101 ;
        public const int    MAX_LOT              = 3   ;
        public const int    MAX_LAMP_KIND        = 8   ;
            

        //Max Seq public const 
        //===========================================================================
        public const int    MAX_SEQ_PART         = 20 ;
        public const int    MAXITEM              = 110;
        public const int    NUMBEROF_RCVBIN      = 16 ;
        public const int    MAX_INSP_TYPE        = 8  ;
        public const int    MAX_BIN_NO           = 128 + 26; //알파벳 포함.
		public const int    MAX_WORK_BIN_NO      = 2  ;
        public const int    MAX_RSLT             = 10 ;
        public const int    MAX_PVI              = 3  ;
        public const int    MAX_SORT_RANK        = 3  ;
        
        //---------------------------------------------------------------------------
        //UserSet - 처음 Class 생성용으로 사용됨 (최대 사용값으로 세팅하시오)
        public const int    MAX_RFID             = 5  ; 
        public const int    MAX_TR_MODULE        = 5  ;

		public const int    MAX_POCK_NO          = 200;

        public const int    MAX_CASE_R           =  2 ;  //Default Row Size of Case Y.
        public const int    MAX_CASE_C           =  4 ;  //Default Col Size of Case X.

        public const int    MAX_TRAY_R           =  5 ;  //Default Row Size of Tray Y.
        public const int    MAX_TRAY_C           =  5 ;  //Default Col Size of Tray X.

        public const int    MAX_PLATE_R          = 200;  //Default Row Size of Plate Y.
        public const int    MAX_PLATE_C          = 200;  //Default Col Size of Plate X.
                                                 
        public const int    MAX_JIG_R            = 1  ;  //Default Row Size of Plate Y.
        public const int    MAX_JIG_C            = 8  ;  //Default Col Size of Plate X.
        public const int    MAX_JIG_ZONE         = 2  ;  
                                                 
		public const double WAF_12_SIZE          = 304.80;
		public const double WAF_8_SIZE           = 203.20;
        public const double WAF_6_SIZE           = 152.40;
        public const double WAF_5_SIZE           = 127.00;
        public const double WAF_4_SIZE           = 101.60;
          
        public const int    MAX_WAF_Y            = 50; //200; //Default Row Size of Wafer. Y. (Half Size)
        public const int    MAX_WAF_X            = 50; //200; //Default Col Size of Wafer  X. (Half Size)

        public const int    MAX_NOZL_C           = 8 ; //define 단열 , 복열
        public const int    MAX_NOZL_R           = 1 ;
		public const int    MAX_NOZL             = 1 ; //define 각 Tool Max Nozzle

        public const int    MAX_SPH_NOZL         = 1 ;
        public const int    MAX_ROT_NOZL         = 4 ;

        public const int    MAX_MGZ_SLOT         = 6 ;
        public const int    MAX_MGZ              = 4 ;

        public const int    MAX_WAF_INSP_XCNT    = 3 ;
        public const int    MAX_WAF_INSP_YCNT    = 3 ;

        public const int    MAX_LAYER            = 8 ;

		public const double JD_TRAY_SIZE_X       = 135.90; //Jedec Tray Size
		public const double JD_TRAY_SIZE_Y       = 315.00;
		public const double JD_TRAY_SIZE_Z       =   6.35;

		public const double JIG_TRAY_SIZE_X       = 384.90; //
		public const double JIG_TRAY_SIZE_Y       = 214.00;
		public const double JIG_TRAY_SIZE_Z       =   6.35;

        public const int    MAX_WP_Z_MOTR        = MAX_NOZL / 2; //Z 모터 한개당 Nozzle 2개 컨트롤.
        public const int    MAX_TP_Z_MOTR        = MAX_NOZL / 2; //Z 모터 한개당 Nozzle 2개 컨트롤.

        //---------------------------------------------------------------------------
        //UserSet - Home Error 정의 
        public const int    ERR_ALLHOME          = 490; //All Home 발생시 Error No
        public const int    ERR_PARTHOME         = 491; //Part Home 발생시 Error No

         //---------------------------------------------------------------------------
        //UserSet - TESTER DUT MAX QTY
        public const int    MAX_TEST_ITEM        = 40;
        public const int    MAX_BIN_VAL          =  5; //INSTENC 검사 ITEM
        public const int    MAX_DEVZ_R           =  3; 
        public const int    MAX_DEVZ_C           =  3; 

        //---------------------------------------------------------------------------
        //UserSet - 
		public const int    MAX_FORCE_CAL_CNT    = 1000;

        //---------------------------------------------------------------------------
        //
        public const double VISN_FOV_X           = 16.90;
        public const double VISN_FOV_Y           = 14.13;


        //---------------------------------------------------------------------------
        //UserSet - 화면에 표시될 설비명 설정  
        public const string sPrjName     = "WAFER ALIGNER";        
        public const string sOsTitle     = "WAFER ALIGNER";
        public const string sMaker       = "DADA System";

        //---------------------------------------------------------------------------
        //UserSet - Part Dimension Info.


        //---------------------------------------------------------------------------
        //UserSet - 


        //---------------------------------------------------------------------------
        //UserSet - User PostMessage
        public const int WM_CTRL_MOTR = 0x1A;


        public const int MAX_FOUP_ID = 16;

        //Communication Stream.
        //===========================================================================
        public const char chNON = (char)0x00;
        public const char chENQ = (char)0x05;
        public const char chACK = (char)0x06;
        public const char chSTX = (char)0x02;
        public const char chETX = (char)0x03;
        public const char chNAK = (char)0x15;
        public const char chSP  = (char)0x20;

        //Max State
        //===========================================================================
        //UserSet - 상태 변경 메뉴에 사용할 항목 설정
        public static string[] STR_WAF_STAT = 
        {
            "None"    ,
            "Empty"   ,
            "Mask"    ,
            "Mount"   ,
            "Aligned" ,
            "Skip"    ,
            "Fnsh"    ,
            "Work"    ,
            "Wait"    ,
            "Fail"    ,
            "Fnsh_A"  ,
            "ReqLoad" ,
        };

        public static string[] STR_MGZ_STAT =
        {
            "NONE"          ,
            "SKIP"          ,
            "EMPTY"         ,
            "MOUNT"         ,
            "WORK"          ,
            "FINISH"        ,
        };
        public static string[] STR_WAF_SIZE =
        {
          //"4 INCH" ,
          //  "5 INCH" ,
          //  "6 INCH" ,
          //"8 INCH" ,
            "12 INCH" ,
        };

        public static string[] STR_WAF_TYPE =
        {
            "WAFER" ,
            "RING FRAME" ,
        };

        public static string[] STR_WAF_ID =
        {
            "WTR " ,
            "WAT " ,
            "MC#1" ,
            "MC#2" ,
        };
        public static string[] STR_MGZ_ID =
        {
            "Load Cassette"   ,
            "Unload Cassette" ,
        };
        public static string[] STR_WTR_WORK_AREA =
        {
            "LPM01" ,
            "LPM02" ,
            "Align Table",
            "ASM01"      ,
            "ASM02"      
        };

        //Max Chip State
        //===========================================================================
        //UserSet - CHIP 상태 변경 메뉴에 사용할 항목 설정
        public static string[] STR_CHP_STAT =
        {
            "None "    , //Table도 없고 칩도 없는 경우.   
			"Mask "    ,
            "Skip "    ,
            "Empty"    , //Table는 있고 칩은 없는 경우.
			"Mount"    , //작업 대기 Chip만 있는 경우.
			"Rslt "    ,
            "Fnsh "    ,
            "GScan"    ,
            "Start"    ,  //완료 상태 
        };
        public static string[] STR_CHP_STAT2 =
        {
            "None"     ,
            "Mask"     ,
            "Skip"     ,
            "Empty"    ,
            "Mount"    ,
            "RsltGood" ,
            "RsltFail" ,
            "Fnsh"     ,
            "GScan"    ,
            "Start"    ,
        };
        public static string[] STR_CHP_RSLT =
        {
            "None"     ,//Test Result Unknown
			"Good"     ,//Test Result Good
			"Fail"     ,//Test Result Fail
			"Wait"     ,
            "Skip"
        };

        //Max UNIT State
        //===========================================================================
        //UserSet - UNIT 상태 변경 메뉴에 사용할 항목 설정
        public static string[] STR_UNIT_STAT =
        {
            "None"   ,
            "Empty"  ,
            "Mask"   ,
            "Mount"  ,
            "Aligned",
            "Skip"   ,
            "Fnsh"   ,
            "Work"   ,
            "Wait"   ,
            "Fail"   ,
            "Fnsh_A" ,
            "ReqLoad", 

        };

        public static string[] STR_PLT_STAT =
        {
            "NONE"          ,
            "MASK"          ,
            "SKIP"          ,
            "EMPTY"         ,
            "MOUNT"         ,
            "WORK"          ,
            "WAIT"          ,
            "FINISH"        ,
            "ERROR"         ,
            "SUPPLY"        ,
            "EJECT"         ,
        };

        public static string[] STR_VISN =
        {
            "Wafer Table Vision"             ,
            "Under Vision"                   ,
            "Main Rotator Align Vision"      , //Main Raotor Align Vision
			"Main Rotator Inspect #1 Vision" , //Main Raotor Inspect #1 Vision
			"Main Rotator Inspect #2 Vision" , //Main Raotor Inspect #2 Vision
			"Sub Rotator Inspect Vision"     , //Main Raotor Inspect #1 Vision
			"Bottom Inspect Vision"          , //Bottom Vision
		};
        public static string[] STR_VISN2 =
        {
            "WTB"      ,
            "UDV"      ,
            "MRA"      , //Main Raotor Align Vision
			"MRI1"     , //Main Raotor Inspect #1 Vision
			"MRI2"     , //Main Raotor Inspect #2 Vision
			"SRI"      , //Main Raotor Inspect #1 Vision
			"BTV"      , //Bottom Vision
		};

        //------------------------------------------------------------------------
        public static string[] STR_SEND_LIST =
        {
            "C600_FOUP_ID_Read_Result_Report"       ,
            "C601_FOUP_ID_write_Result_Report"      ,
            "C602_Port_Status_Report"               ,
            "C604_Port_Slot_Map_Report"             ,
            "C605_Robot_Status_Report"              ,
            "C611_Panel_ID_Reading_Status_Report"   ,
            "C612_Panel_CCD_Alignment_Status_Report",
            "C613_Subpanel_ID_Reading_Status_Report",
            "C631_EFEM_Status_Report"               ,
            "C632_Panel_Transportation_Report"      ,
            "C690_Alarm_Event_Report"               ,
        };
        //------------------------------------------------------------------------
        public static string[] STR_RCV_LIST =
        {
            "R001_Online_Request"             ,
            "R002_Port_Status_Request"        ,
            "R003_FOUP_Status_Request"        ,
            "R004_Robot_Operation_Request"    ,
            "R005_Port_Mode_Request"          ,
            "R006_Port_Operation_Request"     ,
            "R007_Track_InfoChange_Request"   ,
            "R008_Port_E84_Status_Report"     ,
            "R031_EFEM_Status_Request"        ,
            "R032_EFEM_Transportation_Request",
            "R033_FOUP_ID_Verify_result"      ,
            "R034_Port_Slot_Map_Verify_result",
            "R035_Panel_ID_Verify_result"     ,
            "R036_Panel_Process_status_Reply" ,
            "R037_Reset_Alarm_Request_EQ_EFEM",
            "R132_EFEM_Transportation_Request",
        };


        //String - Tool Name 
        //===========================================================================
        //UserSet - FormTool 설정화면에 표시할 이름 설정

    }
    #endregion "public const "  

    /***************************************************************************/
    /* Class: FRM                                                              */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    #region "FORM"
    public static class FRM
    {
        static bool m_bRqShowMsg;
        static bool m_bRqHideMsg;
        static string m_sWarnMsg;
        public static bool           RqChangeLevel;

        public static EN_UI_TYPE     UIType       ;
        public static FrmProgress    Progress     ;
        public static FrmMessage     Message      ;
        public static FrmLogin       Login        ;
        public static FrmAlarm       Alarm        ;
        public static FrmInputPos    InputPos     ;
        public static FrmUpdateInfo  UpdateInfo   ;
        public static FrmInput       Input        ;
        public static FrmDetailChart DetailChart  = null;
        //public static FrmMap         UserMap      ;


        //Main Form
        public static FrmMain        MAIN        ;
      //public static FrmMOper       MOper       ;
        public static FormOper       MOper       ;
        public static FrmMProject    MProj       ;
      //public static FrmMManual     MManual     ;
        public static FrmMMotion     MMotion     ;
        public static FrmMMotor      MMotor      ;
        public static FrmMIO         MIO         ;
        public static FrmMDb         MDb         ;
        public static FrmMSetting    MSetting    ;
        public static FrmMAdmin      MAdmin      ;
		public static FrmMControl    MControl    ;

        //Sub Form
        public static FrmSManual    SManual      ;
        public static FrmSMotor     SMotor       ;
        public static FrmSTool      STool        ;
        public static FrmSVision    SVision      ;
        public static FrmBinColor   BinColor     ;


        public static void Init(Control Ctrl, Form form)
        {
            //
            UIType        = EN_UI_TYPE.Light;
            
            //
            RqChangeLevel = false;
            //
            Login       = new FrmLogin      ();
            Alarm       = new FrmAlarm      ();
            InputPos    = new FrmInputPos   ();
            UpdateInfo  = new FrmUpdateInfo ();
            Input       = new FrmInput      ();

            //Main Form
            //MAIN         = new FrmMain        ();
            MOper        = new FormOper       ();
            MProj        = new FrmMProject    ();

            MMotion      = new FrmMMotion     ();
            MMotor       = new FrmMMotor      ();
            MIO          = new FrmMIO         ();
            MDb          = new FrmMDb         ();
            MSetting     = new FrmMSetting    ();
            MAdmin       = new FrmMAdmin      ();
			MControl     = new FrmMControl    ();
                         
            //Sub Form                 
            SManual      = new FrmSManual     ();
            SMotor       = new FrmSMotor      ();
            STool        = new FrmSTool       ();
            SVision      = new FrmSVision();
            BinColor     = new FrmBinColor    ();

            MAIN         = (FrmMain)form; 

            SetFormParent(MOper    ,Ctrl);
            SetFormParent(MProj    ,Ctrl);
            SetFormParent(MControl ,Ctrl);
            SetFormParent(MMotion  ,Ctrl);
            SetFormParent(MMotor   ,Ctrl);
            SetFormParent(MIO      ,Ctrl);
            SetFormParent(MDb      ,Ctrl);
            SetFormParent(MSetting ,Ctrl);
            SetFormParent(MAdmin   ,Ctrl);

            //#if DEBUG
            //
            //            ChangeLevel(EN_LOGIN.Master);
            //#else 
            //            ChangeLevel(EN_LOGIN.Operator);
            //#endif

            //
            ChangeLevel(EN_LOGIN.Operator); //
            //ChangeLevel(EN_LOGIN.Master);  

        }
        //------------------------------------------------------------------------
        public   static void SetFormParent(Form F, Control Ctrl)
        {
            if (F    == null) return;
            if (Ctrl == null) return;
            //
            F.DoubleBuffered(true);
            F.AutoScaleMode   = AutoScaleMode.None;
            F.FormBorderStyle = FormBorderStyle.None;
            F.Size            = Ctrl.Size;
            F.StartPosition   = FormStartPosition.Manual;
            F.ShowInTaskbar   = false;
            F.Location        = new Point(0,0);
            F.DoubleBufferingAllDataGrid();
            
        }
        public static void SetUCParent(UserControl F, Control Ctrl)
        {
            if (F == null) return;
            if (Ctrl == null) return;
            //
            F.DoubleBuffered(true);
            F.AutoScaleMode = AutoScaleMode.None;
            F.Size = Ctrl.Size;
            F.Location = new Point(0, 0);
            F.DoubleBufferingAllDataGrid();
        }
        //------------------------------------------------------------------------
        // 페이지폼 보이기
        public static void ShowFormParent(Form F, Control Ctrl)
        {
            if (F == null) return;
            if (F.Visible) return;
            Ctrl.HideChildForms();
            F.ShowInside(Ctrl);
        }
        public static void ShowUCParent(UserControl F, Control Ctrl)
        {
            if (F == null) return;
            if (F.Visible) return;
            Ctrl.HideChildForms();
            F.ShowInside(Ctrl);
        }

        //------------------------------------------------------------------------
        // 페이지폼 보이기
        public static void HideFormParent(Form F)
        {
            if ( F == null) return;
            if (!F.Visible) return;
            F.Visible = false;
            F.Hide();
        }
        public static void HideUCParent(UserControl F)
        {
            if (F == null) return;
            if (!F.Visible) return;
            F.Visible = false;
            F.Hide();
        }
        //------------------------------------------------------------------------
        public static void panelSetForm(Panel panel, Form form, bool AllClear = false)        
        {
            form.TopLevel = false;
            form.TopMost = true;
            if (AllClear) panel.Controls.Clear();
            panel.Controls.Add(form);
            form.Show();
        }
        //------------------------------------------------------------------------
        public static void ChangeLevel(EN_LOGIN iLevel = EN_LOGIN.Operator)
        {
            RqChangeLevel = true;
            if(iLevel == (int)EN_LOGIN.Operator)
            {
                //Forced Door Locking & Check Door Closing
                cDEF.FM.DefaultSysChkOptn();
            }
            cDEF.FM.m_iCrntLevel = (int)iLevel;
        }
        //------------------------------------------------------------------------
        public static bool ShowMsg(bool bShow, String sTitle = "", String sMsg = "", EN_MSG_KIND iKind = EN_MSG_KIND.UserModal)
        {                
            //
            try
            {
                if (Message != null)
                {
                    Message.Close();
                    Message = null;
                }
            }
            catch (Exception err) { System.Diagnostics.Debug.WriteLine("Exception:" + err.Message); }

            //
            if (!bShow) return true;
            Message               = new FrmMessage();
            Message.m_iKind       = (int)iKind;
            Message.m_sTitle      = sTitle;
            Message.m_sMsg        = sMsg;
            Message.BringToFront();
            Message.TopMost       = true;
            Message.StartPosition = FormStartPosition.CenterParent;
            //
            if      (iKind == EN_MSG_KIND.UserShow ) { Message.Show(); return false; }
            else if (iKind == EN_MSG_KIND.UserModal)
            {
                DialogResult dr = new DialogResult();
                dr = Message.ShowDialog();
                return dr == DialogResult.Yes;
            }
            return true;
        }
        //--------------------------------------------------------------------------
        public static void ShowWarn(bool bShow, string sMsg = "")
        {
            m_sWarnMsg = sMsg;
            if (bShow) m_bRqShowMsg = true;
            else
            {
                m_bRqHideMsg = true;
                m_bRqShowMsg = false;
                m_sWarnMsg = "";
            }
        }
        //--------------------------------------------------------------------------
        public static void UpdateMsg()
        {
            if (m_bRqHideMsg)
            {
                m_bRqHideMsg = false;
                ShowMsg(false);
            }

            if (m_bRqShowMsg)
            {
                m_bRqShowMsg = false;
                if (m_sWarnMsg != "") ShowMsg(true, "Warning", m_sWarnMsg, EN_MSG_KIND.UserShow);
            }
        }
        //------------------------------------------------------------------------
        public static void ViewDetailQtyChart(int ValueType, System.Windows.Forms.DataVisualization.Charting.SeriesChartType ChartType)
            {
           if (DetailChart != null) return;
           DetailChart = new FrmDetailChart(1, ValueType, ChartType);
           if (DetailChart.ShowDialog() != DialogResult.None) 
               DetailChart = null;
        }
        //다른 Form Control Handling Event.
        //----------------------------------------------
            //Display Wafer Map Form.
        public delegate void SetDWFHandler(EN_MAP_TYPE Type);
        public static event SetDWFHandler SetWafMapType = null;
		public static void DispWafMap(EN_MAP_TYPE Type)
		{
            if (SetWafMapType != null) SetWafMapType(Type);
		}

        //Display Chart Form.
        public delegate void SetCF1Handler(int Axis, double   Val);
        public delegate void SetCF2Handler(          double[] Val);
        public delegate void SetCF3Handler(                      );
        public static event SetCF1Handler SetChartValue1 = null;
        public static event SetCF2Handler SetChartValue2 = null;
        public static event SetCF3Handler ChartClear     = null;
		public static void SetChartValue(int Axis, double Val)
		{
            if (SetChartValue1 != null) SetChartValue1(Axis, Val);
		}
		public static void SetChartValue(double[] Val)
		{
            if (SetChartValue2 != null) SetChartValue2(Val);
		}
		public static void ClearChart()
		{
            if (ChartClear != null) ChartClear();
		}

        public static Color GetBaseColor()
        {
            return FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(236,236,237) : System.Drawing.Color.FromArgb(25 , 41 , 55 );
        }
        public static Color GetGridBackColor()
        {
            return FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(210,210,208) : System.Drawing.Color.FromArgb(66 , 72 , 88 );
        }
        public static Color GetForeColor()
        {
            return FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(37 ,51 ,64 ) : System.Drawing.Color.FromArgb(230, 230, 200);
        }
    }
#endregion "FORM"

    /***************************************************************************/
    /* Class: cDEF                                                             */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static partial class cDEF 
    {
        //
        private static Queue<ST_SAVE_INFO> m_QueSave = new Queue<ST_SAVE_INFO>();
        private static ST_SAVE_INFO       SaveInfo = new ST_SAVE_INFO();

#region "Standard Unit"
        public static TTIckTime       TICK        ;
        public static TThreadUnit     TH          ;
        public static TSysIO          IO          ;
        public static TSysActuator    ACTR        ;
        public static TSysMotor       MOTR        ;
        //public static TDataManger     DM          ;
        public static TFileManger     FM          ;
        public static TLogUnit        LOG         ;

        public static TSpcManger      SPC         ;
        public static TLotUnit        LOT         ;
        public static TLampBuzz       LampBuzz    ;
        public static TPosnUnit       POSN        ;
        public static TErrProc        EPU         ;
        public static TManProc        MAN         ;
        public static TSequence       SEQ         ;
        public static TBinColors      BCOLOR      ;

        public static TVisnUnit       VISN;

        public static TBcrKeyence     BCR         ;  // Barcode Reader
        public static TCOMZEUS        COMZEUS     ;

        //public static TAnalogAjin     ALG         ;
        //public static TCOMASM         COMASM      ;
        //public static TCCD_Keyence  Aligner     ;
        //public static TTempAutonics TempAutonics;  
        //public static TTorqueDKM    TOQMOTR     ;
        //public static TVisnUnit     VISN        ;
        //public static TCCD_Panasonic Aligner    ;
        //public static TBcrOpticon   BcrOpticon  ; 
        //public static TLightSource  LVS         ;   
        //public static TSysServerComm  CommL     ;   
        //public static TSysServerComm  CommR     ;   
        //public static THirataLoadPort[] Load = new THirataLoadPort[2];   
        //public static TKoroRobot        Robot   ;
        //public static TAceAligner       Aligner ;


        //UserSet - 추가된 UNIT 생성
        //public static TTempTOHO        TempTOHO    ;  
        //public static TLoadCellAL50    LCAL50      ;  
        //public static TLoadCellCSD819c CSD891c     ;
        //public static TTesterLinkj     TesterLinkj ;   
        //public static TBcrOpticon      BcrOpticon  ;   
        //public static TLightSource     LVS         ;   
        //public static TGemLinkJ        GemLinkJ    ;                              
        //public static TGemUnit         GEM         ;                              
        //public static TFtpUnit         FTP         ;   //
        //public static TFastenerUnit    Fastener    ;

        #endregion "Standard Unit"
        public static bool DllInit()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\Library");

                if (di.Exists == false) return false;

                foreach (var file in di.GetFiles())
                {
                    string sourceFileName = file.FullName;
                    string destFileName = Application.StartupPath + "\\" + file.Name;

                    if (File.Exists(destFileName) == false)
                        File.Copy(sourceFileName, destFileName, false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Exception] Dll Init-" + ex.Message);
                cDEF.LOG.ExceptionTrace(ex.ToString());
            }

            return true;
        }
        //--------------------------------------------------------------------------
        public static bool ResetCamHotlink()
        {
            bool bOk  = true;

            //Camera Hot Link ReSet.
            if (!NetworkAdapterHelper.DisableNetworkAdapter("Gigabit", "Top Camera"))
            {
                bOk = false; 
                MessageBox.Show("[Disable FAIL] Top Camera - Disable Network Adapter");
            }
            
            if(bOk)
            {
                Thread.Sleep(3000);
                if (!NetworkAdapterHelper.EnableNetworkAdapter("Gigabit", "Top Camera"))
                {
                    MessageBox.Show("[Enable FAIL] Top Camera - Enable Network Adapter");
                }
            
                Thread.Sleep(5000);
            }
                
            //
            return true;
        }
        //--------------------------------------------------------------------------
        public static bool Init()
        {
            try 
            { 
                TICK         = new TTIckTime       ();
                TH           = new TThreadUnit     ();
                IO           = new TSysIO          ();
                ACTR         = new TSysActuator    ();
                MOTR         = new TSysMotor       ();
                //DM           = new TDataManger     ();
                FM           = new TFileManger     ();
                LOG          = new TLogUnit        ();
                SPC          = new TSpcManger      ();
                LOT          = new TLotUnit        ();
                LampBuzz     = new TLampBuzz       ();
                POSN         = new TPosnUnit       ();
                EPU          = new TErrProc        ();
                MAN          = new TManProc        ();
                SEQ          = new TSequence       ();
                BCR          = new TBcrKeyence     ();
                COMZEUS      = new TCOMZEUS        ();
                //ALG          = new TAnalogAjin     ();

                VISN         = new TVisnUnit();

                //UserSet - 추가된 UNIT 생성
                //Aligner      = new TCCD_Keyence    ();
                //VISN         = new TVisnUnit       ();
                //TempAutonics = new TTempAutonics   ();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Exception]Init-" + ex.Message);
                LOG.ExceptionTrace(ex.ToString());
            }

            return true;
        }
        //------------------------------------------------------------------------
        public static bool IOInit(bool sim)
        {
            //UserSet - 사용할 IO 종류 및 IO INIT  항목 설정
            //CNET - CNET 사용, MdDEVN - DEVICE NET 사용
            String sFNameAddress = Application.StartupPath + "\\System\\IOMap.MAP";
            //
            IO._sFNameX = Application.StartupPath + "\\System\\Input.INI" ;
            IO._sFNameY = Application.StartupPath + "\\System\\Output.INI";

            //
            IO.Load(true);
            if(sim) IO.Init(enIO_MAKER.SIMUL  , sFNameAddress); 
            else    IO.Init(enIO_MAKER.FASTECH, sFNameAddress); //Model, Number Of Input,  Number Of Out

            //
            //ALG.Init();

            //
            return IO._bInitOk;
        }
        //------------------------------------------------------------------------
        public static bool ACTRInit(bool sim)
        {
            //UserSet - Actuator Init
            string sFNameACT = Application.StartupPath + "\\System\\Actuator.INI";

            //Actuator 개수, First Actuator Err No, Actuator Set File Name, Simulation Mode
            ACTR.Init(POSN._iLErrNo, POSN._iLManNo, sFNameACT, sim);  

            return true;
        }
        //------------------------------------------------------------------------
        public static bool LampBuzzInit()
        {
            //UserSet - Buzzer Lamp 화면에 설정할 항목 및 Buzzer/Lamp IO 설정
            string[] sKIND  = {"Init"       , //0
                               "Warning"    , //1
                               "Error"      , //2
                               "Run Warning", //3
                               "Run"        , //4
                               "Stop"       , //5
                               "Door Open"  , //6
                               "Work End"   , //7
                               ""           , //
                               "" 
                               };

            //UserSet - Tower Lamp Buzzer IO Define
            LampBuzz._iYLempRed = EN_OUT_ID.yNone;
            LampBuzz._iYLempYel = EN_OUT_ID.yNone;
            LampBuzz._iYLempGrn = EN_OUT_ID.yNone;
            LampBuzz._iYBuzz1   = EN_OUT_ID.yNone;
            LampBuzz._iYBuzz2   = EN_OUT_ID.yNone;
            LampBuzz._iYBuzz3   = EN_OUT_ID.yNone;

            LampBuzz.SetKindStr(sKIND);
            LampBuzz.Load(true);
            return true;

        }
        //------------------------------------------------------------------------
        public static  void  InitMotrName()
        {
            //UserSet - 화면에 표시될 Motor 이름 및 Motor Error 설정 
            int iFHomeMan   = 5  ; //First Part Home Manual No
            int iFManNo     = 30 ; //First Motor Manual Start No
            int iFErrNo     = 101; //First Motor Error  Start No (처음에 + 10 하므로 실제는 110임)
            
            MOTR._iFHomeErr = 140 ; //First Motor Homing Error No

            //대소문자 구분
            //↔ : "1",    ↕:"2",
            //← :"f" ,    →:"g",  ↑:"h" , ↓:"i",
            //▲ : "p",    ▼:"q",  ◀:"t" , ▶:"u",
            //◀▶: "tu" , ▶◀: "ut"
            //←→:"D""F", ↑↓:"E""G", D
            //바있는  화살표  ↓"," ↑:"+"
            //동그라미 화살표 : "P","Q"

            //UserSet - Set Motor Desc.                                      //Error  ,Manual   , Home Manual No
            POSN.SetMotor(EN_MOTR_ID.WAT_X, "Wafer Align Table", "X", "t", "u", iFErrNo, iFManNo, iFHomeMan); //
            POSN.SetMotor(EN_MOTR_ID.WAT_Y, "Wafer Align Table", "Y", "p", "q", iFErrNo, iFManNo, iFHomeMan); //
            POSN.SetMotor(EN_MOTR_ID.WAT_T, "Wafer Align Table", "T", "Q", "P", iFErrNo, iFManNo, iFHomeMan); //Wafer Align Table T Axis
        }                                  
        //------------------------------------------------------------------------
        public static  void InitPosName()
        {
            //UserSet - Pos 화면에 설정할 이름 및 모터 설정 
            string sPart = string.Empty ;

            //Part no, Part Name, Pos Item Name        ,단위 ,Pos Index, 소수점 자리, 공용위치, 모터 No, Move Manual No
            sPart = "Wafer Align";
            
            //WTR_X
            POSN.Set(EN_SEQ_ID.WAT, sPart, "Wait Pos." , "mm", EN_POSN_ID.Wait1 , 3, EN_POS_ID.COMM, EN_MOTR_ID.WAT_X);
            POSN.Set(EN_SEQ_ID.WAT, sPart, "Align Pos.", "mm", EN_POSN_ID.CalPos, 3, EN_POS_ID.VIEW, EN_MOTR_ID.WAT_X);

            //WTR_Y
            POSN.Set(EN_SEQ_ID.WAT, sPart, "Wait Pos." , "mm", EN_POSN_ID.Wait1 , 3, EN_POS_ID.COMM, EN_MOTR_ID.WAT_Y);
            POSN.Set(EN_SEQ_ID.WAT, sPart, "Align Pos.", "mm", EN_POSN_ID.CalPos, 3, EN_POS_ID.VIEW, EN_MOTR_ID.WAT_Y);

            //WTR_T
            POSN.Set(EN_SEQ_ID.WAT, sPart, "Wait Pos." , "º" , EN_POSN_ID.Wait1 , 3, EN_POS_ID.COMM, EN_MOTR_ID.WAT_T);
            POSN.Set(EN_SEQ_ID.WAT, sPart, "Align Pos.", "º" , EN_POSN_ID.CalPos, 3, EN_POS_ID.VIEW, EN_MOTR_ID.WAT_T);
        }
        //------------------------------------------------------------------------
        public static void  InitDstb()                                                                               
        {
	    	//int iCnt = 0;

            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR X축이 이동하기 위한 최소 Y축 위치."             );
            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR T축이 알 수 없는 위치시 WTR Y축 최대 이송 위치." );
            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR Z축이 Magazine내에서 최대 이동할 수 있는 이송량.");
            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR-Y가 LPM1에서 최대하강 위치"                    );
            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR-Y가 LPM2에서 최대하강 위치"                    );
            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR-Y가 Align에서 최대하강 위치"                   );
            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR-Y가 Machine1에서 최대하강 위치"                );
            //MOTR.SetDstb(Enum.GetName(typeof(EN_DSTB_ID), iCnt++), "WTR-Y가 Machine1에서 최대하강 위치"                );
        }
        //------------------------------------------------------------------------
        public static void    InitMotor(bool sim)
        {
            //UserSet - 모터 메이커의 모터 시작 번지 입력 
            //ex) MOTR.Init   (EN_MOTR_MAKER.COMI,0, EN_MOTR_MAKER.AJECAT, 4);
            //MOTR.Init            (EN_MOTR_MAKER.WMX3, 0);

            if (sim) MOTR.Init(EN_MOTR_MAKER.SIMUL    , 0);
            else     MOTR.Init(EN_MOTR_MAKER.FASTECH  , 0);

            InitMotrName         ();
            InitPosName          (); 
            InitDstb             ();

            MOTR.Load            (true , FM._sCrntDevice);
            MOTR.LoadMotrDisturb (true                  );
            MOTR.SetAxis         (                      );
        }
        //------------------------------------------------------------------------
        public static void SetFileSave(EN_SAVE_TYPE type, bool load = false)
        {
            ST_SAVE_INFO si = new ST_SAVE_INFO();
            si.eType = type;
            si.bLoad = load;

            m_QueSave.Enqueue(si);
        }
        //------------------------------------------------------------------------
        public static void UpdateFileSave ()
        {//Function of File Save 

            //Check Que 
            if (m_QueSave.Count > 0)
            {
                SaveInfo = m_QueSave.Dequeue();

                switch (SaveInfo.eType)
                {
                    case EN_SAVE_TYPE.Motor:
                        break;
                    case EN_SAVE_TYPE.OptEng:
                        //cDEF.FM.EngrOptn.Load_XML(false);
                        break;
                    case EN_SAVE_TYPE.OptMaster:
                        break;
                    case EN_SAVE_TYPE.Actuator:
                        break;
                    case EN_SAVE_TYPE.LampBuzz:
                        LampBuzz.Load(false);
                        break; 
                    default:
                        break;
                }
            }
        }
        //------------------------------------------------------------------------
        public static void SetUpdateInfo()
        {
            int Cnt = 0; 

            //UserSet - Update 내용 및 Version Name
            FM.m_sUpInform[Cnt++] = "[Version][일자   ][요청자][처리자] [내용]";
            FM.m_sUpInform[Cnt++] = "[V.2.0.1][230201][JUNG ][JUNG] 1차검증완료";
            FM.m_sUpInform[Cnt++] = "[V.2.0.1][230206][JUNG ][JUNG] LOG 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.0.1][230207][JUNG ][JUNG] DB 확인 / BCR 동작은 Align 동작 후로 변경";
            FM.m_sUpInform[Cnt++] = "[V.2.0.1][230207][JUNG ][JUNG] MTBI 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230210][JUNG ][JUNG] BCR IP/PORT Option 처리";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230215][JUNG ][YOU ] SAWING Wafer 검증 완료";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230215][JUNG ][YOU ] SAWING/Base Wafer 검증 완료";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230224][JUNG ][JUNG] Wafer Skip Mode 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230313][JUNG ][JUNG] IO Inverse 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230329][JUNG ][JUNG] Vacuum Error Delay 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230330][JUNG ][JUNG] Ring Frame Option 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230330][JUNG ][JUNG] Vacuum Delay 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230331][JUNG ][JUNG] Wafer Detect Skip Option 삭제(미감지시 무조건 Ring Frame으로)";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230406][JUNG ][JUNG] Sequence Cycle Time Log 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230602][심두선][JUNG] Vacuum Error 발생 시 Vacuum Off 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][230801][심두선][JUNG] Ring Frame Option 수정2- Wafer or Ring Frame이냐 두개 Option";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][231004][곽호륜][JUNG] Vacuum On/Off Auto일때도 동작하도록 변경";
			FM.m_sUpInform[Cnt++] = "[V.2.1.1][231018][JUNG ][JUNG] Image 삭제 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][231024][JUNG ][JUNG] MAN일때 MAN message 받는 경우 @MAN 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][231106][JUNG ][JUNG] Wafer vacuum Check Sequence 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][231220][곽호륜][CJW ] RCP 커맨드 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][240205][     ][CJW ] GrabTest 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][240220][천강훈][CJW ] Image 파일 주기삭제 기능추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.1][240220][     ][YOU ] 링프레임 웨이퍼 오차 검사 기능추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240227][     ] Wafer 위치 편차 검출 기능 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240718][     ] NG IMAGE SAVE 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240725][     ] Light Control 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240805][     ] Image 용량 삭제 기능 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240805][     ] Vision Result Image Save 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240807][     ] Notch Type Align 시 D Cut 보정 옵션 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240808][     ] Cal Position Clear 추가(HOME/RESET/TOSTOP)";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240808][     ] File Image Test 추가(Master Mode)";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240808][     ] Max Position Check 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240808][     ] Ring Frame 외각 표시 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240809][     ] Vision Parameter InscribedPoint 설정 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][240809][     ] SAW Wafer 외각 표시 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][241016][     ] RingFrame Edge Image Threshold 가변 Detect 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250123][     ] RST Command 시 Stop 후 Reset완료 명령 전송";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250123][     ] INIT Command 시 Server Off시에도 Server On 후 Initialize 실행";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250219][     ] Main 화면 Vacuum ON/OFF 변경";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250317][     ] ERR_0074-DetectFailed Error 세분화";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250317][     ] ERR_0063-WaferCenterVsRingCenterOver Error 세분화";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250317][     ] GapCheck Error 시 비교 Data Log";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250414][     ] Log Enqueue 부분 Try Catch 구문 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250721][     ] 중국향 Sawing Wafer Detect 알고리즘 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][250730][     ] Vision Align 부분 Try Catch 구문 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][251226][     ] WCK 알고리즘 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][260102][     ] Vacuum Delay Time 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][260109][     ] WCK 기능 동작 시 Return Code 변경 요청";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][260421][     ] Log 누락 관련 수정";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][260507][     ] FanAlarm 추가";
            FM.m_sUpInform[Cnt++] = "[V.2.1.2][260729][     ] WCS 관련 수정";

            //Version
            FM._sVersion = "V2.1.2(260804_18H)";
        }
    }
}