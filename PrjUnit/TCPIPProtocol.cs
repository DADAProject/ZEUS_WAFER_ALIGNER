using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    //ID_Message name

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Equipment → EFEM
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public enum EN_RECV_LIST
    {
        R001_Online_Request               = 0, //= 001,
        R002_Port_Status_Request             , //= 002,
        R003_FOUP_Status_Request             , //= 003,
        R004_Robot_Operation_Request         , //= 004,
        R005_Port_Mode_Request               , //= 005,
        R006_Port_Operation_Request          , //= 006,
        R007_Track_InfoChange_Request        , //= 007,
        R008_Port_E84_Status_Report          , //= 008,
        R031_EFEM_Status_Request             , //= 031,
        R032_EFEM_Transportation_Request     , //= 032,
        R033_FOUP_ID_Verify_result           , //= 033,
        R034_Port_Slot_Map_Verify_result     , //= 034,
        R035_Panel_ID_Verify_result          , //= 035,
        R036_Panel_Process_status_Reply      , //= 036,
        R037_Reset_Alarm_Request_EQ_EFEM     , //= 037,
        
        R132_EFEM_Transportation_Request     , //= 132,   //(Slave use only)

        EndOfList 
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //EFEM → Equipment
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public enum EN_SEND_LIST
    {
        C600_FOUP_ID_Read_Result_Report         = 0, //600,
        C601_FOUP_ID_write_Result_Report           , //601,
        C602_Port_Status_Report                    , //602,
        C604_Port_Slot_Map_Report                  , //604,
        C605_Robot_Status_Report                   , //605,
        C611_Panel_ID_Reading_Status_Report        , //611,
        C612_Panel_CCD_Alignment_Status_Report     , //612,
        C613_Subpanel_ID_Reading_Status_Report     , //613,
        C631_EFEM_Status_Report                    , //631,
        C632_Panel_Transportation_Report           , //632,
        C690_Alarm_Event_Report                    , //690,

        EndOfList
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public enum EN_ROBOT_OPER
    {
        none    = -1,
        Start   =  0, 
        Stop        , 
        Pause       ,
        Resume      ,
        Abort       ,

        EndOfList
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public enum EN_PORT_OPER
    {
        none      =-1,
        Load      = 0, 
        Unload       , 
        Mapping      ,
        RFIDRead     ,
        RFIDWrite    ,

        EndOfList
    }
    //------------------------------------------------------------------------
    public enum EN_PORT_MODE
    {
        none   = -1,
        Manual = 0 ,
        Auto       ,
        Disable    , 
        
        EndOfList
    }
    //------------------------------------------------------------------------
    public enum EN_ARM_ACTION
    {
        P = 0 ,//P: Panel storage (Put) 
        G     ,//G: Panel acquisition (Get)  
        F     ,//F: Operation completed (Finish) 
        A      //A: Suspended (Abort)

    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public enum EN_TR_MODE
    {
        //If <A [1] Get from EQ / FOUP> = 0
        //01 = Load Port 1
        //02 = Load Port 2
        //03 = Load Port 3
        //04 = Load Port 4
        
        //If <A [1] Get from EQ / FOUP> = 1
        //01 = EQ 1
        //02 = EQ 2
        //03 = EQ 3 (Reserved)
        //04 = EQ 4 (Reserved)
        
        none       =-1     ,
                   
        Get        = 0     , 
        Put        = 1     ,
        Exchange           ,
        CCD_Align_Pos      ,
        Read_Pos           ,
        Remove_Pos         ,

        EndOfList
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public enum EN_PORT_ID
    {
        P1 = 0, 
        P2    ,
        P3    ,
        P4    ,

        EndOfList
    }         
    //------------------------------------------------------------------------
    public enum EN_PORT_STATUS_MODE
    {
        //Load port FOUP operating mode
        //  00: Buffer
        //  01: only Source (Load)
        //  10: only Target (Unload)
        //  11: both Source and Target
        Buffer     =  0,
        OnlyLoad   =  1, 
        OnlyUnload = 10, 
        BothMode   = 11,

        EndOfList 

    }
    //------------------------------------------------------------------------
    public enum EN_PORT_STATUS
    {
        //Port status
        //  0: Port disabled
        //  1: Load Request (LDRQ)
        //  2: Load Complete (LDCM)
        //  3: Unload Request (UDRQ)
        //  4: Unload Complete (UDCM)
        Disable = 0, 
        LDRQ    = 1, //
        LDCM       ,
        UDRQ       ,
        UDCM       ,

        EndOfList

    }
    //------------------------------------------------------------------------
    public enum EN_TARGET_SOURCE
    {
        //<A[1] Get from / Put into EQ / FOUP >
        //0 = FOUP
        //1 = EQ

        FOUP = 0, //
        EQ      ,

        EndOfList

    }

}
