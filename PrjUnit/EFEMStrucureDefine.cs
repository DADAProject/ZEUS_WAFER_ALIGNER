using ElmoMotionControl.GMAS.EASComponents.MMCLibDotNET;
using Emgu.CV.Dnn;
using Sentech.GenApiDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace eMachine
{
    /************************************************************************/
    /* VAR                                                                  */
    /************************************************************************/

    /************************************************************************/
    /* Structure                                                            */
    /************************************************************************/

    /************************************************************************/
    /* Equipment → EFEM                                                     */
    /************************************************************************/

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R001_ONLINE_REQUEST
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] command;
        //
        public string ID      => Encoding.ASCII.GetString(id     );
        public string EQNo    => Encoding.ASCII.GetString(eqno   );
        public string COMMAND => Encoding.ASCII.GetString(command);

        public override string ToString() => $"{ID}{EQNo}{COMMAND}";

        //
        public ST_R001_ONLINE_REQUEST(bool rtn = false)
        {
            id      = new byte[4];
            eqno    = new byte[2];
            command = new byte[1];
        }

    }

    public struct ST_C001_ONLINE_Reply
    {
        public string ID     ; 
        public string EQNo   ; 
        public string GRANT  ; //Success or failure of acceptance (0: OK / 1: NG)

        public ST_C001_ONLINE_Reply(string data = "")
        {
            this.ID      = "C001";
            this.EQNo    = data  ;
            this.GRANT   = data  ;
        }
        public override string ToString() => $"{ID}{EQNo}{GRANT}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R002_Port_Status_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;

        //
        public string ID     => Encoding.ASCII.GetString(id  );
        public string EQNo   => Encoding.ASCII.GetString(eqno);
        public string PortNo => Encoding.ASCII.GetString(portno);

        public override string ToString() => $"{ID}{EQNo}{PortNo}";

        //
        public ST_R002_Port_Status_Request(bool rtn = false)
        {
            id     = new byte[4];
            eqno   = new byte[2];
            portno = new byte[2];
        }
    }

    public struct ST_C002_Port_Status_Reply
    {
        /*
        <A [2] EQNo>	Same value as requested
        <A [2] PortNo>	Same value as requested
        <A [1] PortStatus>	Port status 0: Port disabled 1: Load Request (LDRQ) 2: Load Complete (LDCM) 3: Unload Request (UDRQ) 4: Unload Complete (UDCM)
        */
        public string ID     ;
        public string EQNo   ;
        public string PORTNO ;
        public string STATUS ; //Port status 0: Port disabled 1: Load Request(LDRQ) 2: Load Complete(LDCM) 3: Unload Request(UDRQ) 4: Unload Complete(UDCM)

        public ST_C002_Port_Status_Reply(string data = "")
        {
            this.ID      = "C002";
            this.EQNo    = data  ;
            this.PORTNO  = data  ;
            this.STATUS  = data  ;
        }
        public override string ToString() => $"{ID}{EQNo}{PORTNO}{STATUS}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R003_FOUP_Status_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string EQNo   => Encoding.ASCII.GetString(eqno  );
        public string PortNo => Encoding.ASCII.GetString(portno);
        
        public override string ToString() => $"{ID}{EQNo}";
        
        //
        public ST_R003_FOUP_Status_Request(bool rtn = false)
        {
            id     = new byte[4];
            eqno   = new byte[2];
            portno = new byte[2];
        }
    }

    public struct ST_C003_FOUP_Status_Reply
    {
        /*
        <A [2] EQNo>	       Same value as requested
        <A [2] PortNo>	       Same value as requested
        <A [112 112] FOUPID>   Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. If there is no FOUP, fill it with "*".
        <A [1] FOUPStatus>	   FOUP status 0: No FOUP 1: Waiting for slot mapping 2: Ready for access 3: Accessing 4: Suspending access 5: Ending access
         */

        public string ID        ;
        public string EQNo      ;
        public string PortNo    ;
        public string FOUPID    ;//Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. If there is no FOUP, fill it with "*".
        public string FOUPStatus;//FOUP status 0: No FOUP 1: Waiting for slot mapping 2: Ready for access 3: Accessing 4: Suspending access 5: Ending access

        public ST_C003_FOUP_Status_Reply(string data = "")
        {
            this.ID         = "C003";
            this.EQNo       = data  ;
            this.PortNo     = data  ;
            this.FOUPID     = data  ;
            this.FOUPStatus = data  ;
        }
        public override string ToString() => $"{ID}{EQNo}{PortNo}{FOUPID}{FOUPStatus}";
    }                                                             

    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R004_Robot_Operation_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] status;
        
        //
        public string ID       => Encoding.ASCII.GetString(id    );
        public string RBStatus => Encoding.ASCII.GetString(status);


        public override string ToString() => $"{ID}{RBStatus}";

        //
        public ST_R004_Robot_Operation_Request(bool rtn = false)
        {
            id     = new byte[4];
            status = new byte[1];
        }
    }

    public struct ST_C004_Robot_Operation_Reply
    {
        public string ID        ;
        public string ACK       ; //Response Judge 0 = OK 1 = NG If NG, command result 3: Request failed (robot is being prepared)


        public ST_C004_Robot_Operation_Reply(string data = "")
        {
            this.ID         = "C004";
            this.ACK        = data  ;
        }
        public override string ToString() => $"{ID}{ACK}";
    }

    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R005_Port_Mode_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] portstatus;
        //
        public string ID => Encoding.ASCII.GetString(id);
        public string PortNo => Encoding.ASCII.GetString(portno);
        public string PortStatus => Encoding.ASCII.GetString(portstatus); //0 = Manual 1 = Auto D = Disable

        //
        public override string ToString() => $"{ID}{PortNo}{PortStatus}";

        //
        public ST_R005_Port_Mode_Request(bool rtn = false)
        {
            id         = new byte[4];
            portno     = new byte[2];
            portstatus = new byte[1];
        }

    }

    public struct ST_C005_Port_Mode_Reply
    {
        public string ID;
        public string ACK; //Response Judge 0 = OK 1 = NG


        public ST_C005_Port_Mode_Reply(string data = "")
        {
            this.ID  = "C005";
            this.ACK = data;
        }
        public override string ToString() => $"{ID}{ACK}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R006_Port_Operation_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] portoper;
        //
        public string ID       => Encoding.ASCII.GetString(id);
        public string PortNo   => Encoding.ASCII.GetString(portno);
        public string PortOper => Encoding.ASCII.GetString(portoper); //0 = Manual 1 = Auto D = Disable

        //
        public override string ToString() => $"{ID}{PortNo}{PortOper}";

        //
        public ST_R006_Port_Operation_Request(bool rtn = false)
        {
            id         = new byte[4];
            portno     = new byte[2];
            portoper   = new byte[1];
        }

    }

    public struct ST_C006_Port_Operation_Reply
    {
        public string ID    ;
        public string PORTNO;
        public string ACK   ; //Response Judge 0 = OK 1 = NG


        public ST_C006_Port_Operation_Reply(string data = "")
        {
            this.ID     = "C006";
            this.PORTNO = data;
            this.ACK    = data;
        }
        public override string ToString() => $"{ID}{PORTNO}{ACK}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R008_Port_E84_Status_Report
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        
        //
        public string ID         => Encoding.ASCII.GetString(id    );
        public string PortNo     => Encoding.ASCII.GetString(portno); //01 = Load Port 1 02 = Load Port 2 03 = Load Port 3 04 = Load Port 4

        //
        public override string ToString() => $"{ID}{PortNo}";

        //
        public ST_R008_Port_E84_Status_Report(bool rtn = false)
        {
            id         = new byte[4];
            portno     = new byte[2];
        }

    }

    public struct ST_C008_Port_E84_Status_Reply
    {
        public string ID     ;
        public string PortNo ;
        public string GRANT  ; //0 = OK 1 = NG
        public string E84Code; //A<1>~A< 8>(INPUT)  RB_E84_VALID, RB_E84_CS0  , RB_E84_CS1     , RB_E84_SPARE_03, RB_E84_TR_REQ  , RB_E84_BUSY    , RB_E84_COMPT  , RB_E84_CONT
                               //A<9>~A<16>(OUTPUT) WB_E84_L_REQ, WB_E84_U_REQ, WB_E84_SPARE_02, WB_E84_READY   , WB_E84_SPARE_04, WB_E84_SPARE_05, WB_E84_HO_AVBL, WB_E84_ES

        public ST_C008_Port_E84_Status_Reply(string data = "")
        {
            this.ID      = "C008";
            this.PortNo  = data  ;
            this.GRANT   = data  ;
            this.E84Code = data  ;
        
        }
        public override string ToString() => $"{ID}{PortNo}{GRANT}{E84Code}";
    }

    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R031_EFEM_Status_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;

        //
        public string ID   => Encoding.ASCII.GetString(id  );
        public string EQNo => Encoding.ASCII.GetString(eqno); 

        //
        public override string ToString() => $"{ID}{EQNo}";

        //
        public ST_R031_EFEM_Status_Request(bool rtn = false)
        {
            id   = new byte[4];
            eqno = new byte[2];
        }

    }

    public struct ST_C031_EFEM_Status_Reply
    {
        public string ID         ;
        public string EQNo       ; 
        public string ACK        ; 
        public string EFMEFStatus; // Shows the status of the following 16 items in order by 1 to 3 characters
                                  // ・ Operation Mode 0: Standalone / 1: Inline
                                  // ・ EFEM Status 1: Ready / 0: Preparing or error
                                  // ・ Robot Upper Arm Panel
                                  // ・ Robot Lower Arm Panel 0: No panel /n(1-4): PortnThere is a panel in / D: Disabled
                                  // ・ EQ1 Online Status
                                  // ・ EQ2 Online Status 0: Offline / 1: Online
                                  // ・ Load Port 1 Transport Mode
                                  // ・ Load Port 2 Transport Mode
                                  // ・ Load Port 3 Transport Mode
                                  // ・ Load Port 4 Transport Mode 0: Auto / 1: Manual / D: Invalid or not installed
                                  // ・ T1 Time out value (2 characters: 01 to 30)
                                  // ・ T2 Time out value (3 characters: 060 to 240)
                                  // ・ D1 Time out value (2 characters: 20 to 60)
                                  // ・ D2 Time out value (2 characters: 20-60)
                                  // ・ D3 Time out value (2 characters: 03-10)
                                  // ・ Signal Tower Status (4 characters each for R, Y, G, B) 0: Off / 1: Lit / 2: Flashing

        public ST_C031_EFEM_Status_Reply(string data = "")
        {
            this.ID          = "C008";
            this.EQNo        = data;
            this.ACK         = data;
            this.EFMEFStatus = data;
        }

        public override string ToString() => $"{ID}{EQNo}{ACK}{EFMEFStatus}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R032_EFEM_Transportation_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] stageid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] trmode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] panelid;

        //
        public string ID      => Encoding.ASCII.GetString(id     );
        public string EQNo    => Encoding.ASCII.GetString(eqno   );
        public string StageID => Encoding.ASCII.GetString(stageid);
        public string TRMode  => Encoding.ASCII.GetString(trmode ); //Transfer mode 0: Clear 1: Load 2: Unload 3: Exchange 4: Abort
        public string PanelID => Encoding.ASCII.GetString(panelid);

        //
        public override string ToString() => $"{ID}{EQNo}{StageID}{TRMode}{PanelID}";

        //
        public ST_R032_EFEM_Transportation_Request(bool rtn = false)
        {
            id      = new byte[4];
            eqno    = new byte[2];
            stageid = new byte[2];
            trmode  = new byte[1];
            panelid = new byte[16];
        }
    }

    public struct ST_C032_EFEM_Transportation_Reply
    {
        /*
        <A [2] EQNo>	    Same value as requested
        <A [1] ACKC>	    Acceptance success / failure S: Success / E: Error
        <A [4] Error Code>	Error code (error details) 0000: No error 0001: Robot is being prepared 0002: The panel with the specified panel ID does not exist 0003: EFEM cannot operate 2 panels because it is 1 arm 0004: There is no loadable panel (only when a load is requested) )
        */

        public string ID     ;
        public string EQNo   ;
        public string ACK    ; //Acceptance success,failure | S: Success / E: Error
        public string ErrCode; //Error code (error details)
                               //0000: No error
                               //0001: Robot is being prepared
                               //0002: The panel with the specified panel ID does not exist
                               //0003: EFEM cannot operate 2 panels because it is 1 arm
                               //0004: There is no loadable panel (only when a load is requested) )

        public ST_C032_EFEM_Transportation_Reply(string data = "")
        {
            this.ID      = "C032";
            this.EQNo    = data;
            this.ACK     = data;
            this.ErrCode = data;

        }

        public override string ToString() => $"{ID}{EQNo}{ACK}{ErrCode}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R132_EFEM_Transportation_Request
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] where;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] stageid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] slotno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] armno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] trmode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] option;


        //
        public string ID       => Encoding.ASCII.GetString(id      );
        public string EQNo     => Encoding.ASCII.GetString(eqno    );
        public string Where    => Encoding.ASCII.GetString(where   );
        public string StageID  => Encoding.ASCII.GetString(stageid );
        public string PortNo   => Encoding.ASCII.GetString(portno  );
        public string SlotNo   => Encoding.ASCII.GetString(slotno  );
        public string ArmNo    => Encoding.ASCII.GetString(armno   );
        public string TRMode   => Encoding.ASCII.GetString(trmode  ); 
        public string Option   => Encoding.ASCII.GetString(option  );

        //
        public override string ToString() => $"{ID}{EQNo}{Where}{StageID}{PortNo}{SlotNo}{ArmNo}{TRMode}{Option}";

        //
        public ST_R132_EFEM_Transportation_Request(bool rtn)
        {
            id      = new byte[4];
            eqno    = new byte[2];
            where   = new byte[1];
            stageid = new byte[2];
            portno  = new byte[2];
            slotno  = new byte[2];
            armno   = new byte[1];
            trmode  = new byte[1];
            option  = new byte[1];
        }
    }

    public struct ST_C132_EFEM_Transportation_Reply
    {
        /*
        <A [2] EQNo>	    Same value as requested
        <A [1] ACKC>	    Acceptance success / failure S: Success / E: Error
        <A [4] Error Code>	Error code (error details) 0000: No error 0001: Robot is being prepared 0002: The panel with the specified panel ID does not exist 0003: EFEM cannot operate 2 panels because it is 1 arm 0004: There is no loadable panel (only when a load is requested) )
        */

        public string ID     ;
        public string EQNo   ;
        public string ACK    ; //Acceptance success,failure | S: Success / E: Error
        public string ErrCode; //Error code (error details)
                               //0000: No error
                               //0001: Robot is being prepared
                               //0002: The panel with the specified panel ID does not exist
                               //0003: EFEM cannot operate 2 panels because it is 1 arm
                               //0004: There is no loadable panel (only when a load is requested)

                               //0005: Robot is moving...???

        public ST_C132_EFEM_Transportation_Reply(string data)
        {
            this.ID      = "C132";
            this.EQNo    = data;
            this.ACK     = data;
            this.ErrCode = data;

        }

        public override string ToString() => $"{ID}{EQNo}{ACK}{ErrCode}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R033_FOUP_ID_Verify_Result
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] foupid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] result;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string EQNo   => Encoding.ASCII.GetString(eqno  );
        public string PortNo => Encoding.ASCII.GetString(portno);
        public string FUOPID => Encoding.ASCII.GetString(foupid); //Transfer mode 0: Clear 1: Load 2: Unload 3: Exchange 4: Abort
        public string Result => Encoding.ASCII.GetString(result);

        //
        public override string ToString() => $"{ID}{EQNo}{PortNo}{FUOPID}{Result}";

        //
        public ST_R033_FOUP_ID_Verify_Result(bool rtn)
        {
            id     = new byte[4];
            eqno   = new byte[2];
            portno = new byte[2];
            foupid = new byte[16];
            result = new byte[1];
        }
    }

    public struct ST_C033_FOUP_ID_Verify_Reply
    {
        /*
        <A [2] EQNo>	Same value as requested
        <A [2] PortNo>	Same value as requested
        <A [16] FOUPID>	Same value as requested
        <A [1] ACK>	Acceptance success / failure (0: OK / 1: NG (EFEM uninitialized) / 2: NG (not waiting for Slot Map judgment))
        */

        public string ID    ;
        public string EQNo  ;
        public string PortNo;
        public string FOUPID; 
        public string ACK   ; //Acceptance success / failure (0: OK / 1: NG (EFEM uninitialized) / 2: NG (not waiting for FOUP ID judgment))


        public ST_C033_FOUP_ID_Verify_Reply(string data)
        {
            this.ID     = "C033";
            this.EQNo   = data;
            this.PortNo = data;
            this.FOUPID = data;
            this.ACK    = data;
            
        }
        public override string ToString() => $"{ID}{EQNo}{PortNo}{FOUPID}{ACK}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R034_Port_Slot_Map_Verify_Result
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] foupid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] result;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string EQNo   => Encoding.ASCII.GetString(eqno  );
        public string PortNo => Encoding.ASCII.GetString(portno);
        public string FUOPID => Encoding.ASCII.GetString(foupid); //Transfer mode 0: Clear 1: Load 2: Unload 3: Exchange 4: Abort
        public string Result => Encoding.ASCII.GetString(result);

        //
        public override string ToString() => $"{ID}{EQNo}{PortNo}{FUOPID}{Result}";

        //
        public ST_R034_Port_Slot_Map_Verify_Result(bool rtn)
        {
            id     = new byte[4];
            eqno   = new byte[2];
            portno = new byte[2];
            foupid = new byte[16];
            result = new byte[1];
        }
    }

    public struct ST_C034_Port_Slot_Map_Verify_Reply
    {
        /*
        <A [2] EQNo>	Same value as requested
        <A [2] PortNo>	Same value as requested
        <A [16] FOUPID>	Same value as requested
        <A [1] ACK>	Acceptance success / failure (0: OK / 1: NG (EFEM uninitialized) / 2: NG (not waiting for Slot Map judgment))
        */

        public string ID    ;
        public string EQNo  ;
        public string PORTNO;
        public string FOUPID; 
        public string ACK   ; //Acceptance success / failure (0: OK / 1: NG (EFEM uninitialized) / 2: NG (not waiting for Slot Map judgment))


        public ST_C034_Port_Slot_Map_Verify_Reply(string data)
        {
            this.ID     = "C034";
            this.EQNo   = data;
            this.PORTNO = data;
            this.FOUPID = data;
            this.ACK    = data;
            
        }
        public override string ToString() => $"{ID}{EQNo}{PORTNO}{FOUPID}{ACK}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R035_Panel_ID_Verify_Result
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] stageid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] slotno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] panelid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] result;

        //
        public string ID      => Encoding.ASCII.GetString(id     );
        public string EQNo    => Encoding.ASCII.GetString(eqno   );
        public string StageId => Encoding.ASCII.GetString(stageid);
        public string PortNo  => Encoding.ASCII.GetString(portno );
        public string SlotNo  => Encoding.ASCII.GetString(slotno );
        public string PanelId => Encoding.ASCII.GetString(panelid);
        public string Result  => Encoding.ASCII.GetString(result );

        //
        public override string ToString() => $"{ID}{EQNo}{StageId}{PortNo}{SlotNo}{PanelId}{Result}";

        //
        public ST_R035_Panel_ID_Verify_Result(bool rtn)
        {
            id      = new byte[4];
            eqno    = new byte[2];
            stageid = new byte[2];
            portno  = new byte[2];
            slotno  = new byte[2];
            panelid = new byte[16];
            result  = new byte[1];
        }
    }

    public struct ST_C035_Panel_ID_Verify_Reply
    {
        public string ID     ;
        public string EQNo   ;
        public string STAGEID;
        public string ACK    ; //


        public ST_C035_Panel_ID_Verify_Reply(string data)
        {
            this.ID      = "C035";
            this.EQNo    = data;
            this.STAGEID = data;
            this.ACK     = data;
            
        }
        public override string ToString() => $"{ID}{EQNo}{STAGEID}{ACK}";
    }
    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R036_Panel_Process_status_Reply
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] eqno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] stageid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] status;

        //
        public string ID      => Encoding.ASCII.GetString(id     );
        public string EQNo    => Encoding.ASCII.GetString(eqno   );
        public string StageId => Encoding.ASCII.GetString(stageid);
        public string Status  => Encoding.ASCII.GetString(status ); //0: Panel is not on the device
                                                                    //1: Start 2: Pause 3: Resume 4: Completed (Process End, OK)
                                                                    //5: Canceled (Process Cancel, NG) 6: Panel is removed
        public override string ToString() => $"{ID}{EQNo}{StageId}{Status}";

        //
        public ST_R036_Panel_Process_status_Reply(bool rtn)
        {
            id      = new byte[4];
            eqno    = new byte[2];
            stageid = new byte[2];
            status  = new byte[1];
        }
    }

    public struct ST_C036_Panel_Process_status_Reply
    {
        public string ID     ;
        public string EQNo   ;
        public string STAGEID;
        public string ACK    ; //

        public ST_C036_Panel_Process_status_Reply(string data)
        {
            this.ID      = "C036";
            this.EQNo    = data;
            this.STAGEID = data;
            this.ACK     = data;
        }
        public override string ToString() => $"{ID}{EQNo}{STAGEID}{ACK}";
    }

    //------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R037_Reset_Alarm
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] reset;

        //
        public string ID    => Encoding.ASCII.GetString(id   );
        public string Reset => Encoding.ASCII.GetString(reset); //1 = Reset

        public override string ToString() => $"{ID}{Reset}";

        //
        public ST_R037_Reset_Alarm(bool rtn)
        {
            id    = new byte[4];
            reset = new byte[1];
        }

    }

    public struct ST_C037_Reset_Alarm_Reply
    {
        public string ID;
        public string ACK; //


        public ST_C037_Reset_Alarm_Reply(string data )
        {
            this.ID = "C037";
            this.ACK = data;
        }
        public override string ToString() => $"{ID}{ACK}";
    }

    /************************************************************************/
    /* EFEM → Equipment                                                     */
    /************************************************************************/
    public struct ST_C600_FOUP_ID_Read_Result_Report
    {
        /*
        <A [2] PortNo>	          Port No (01, 02…)
        <A [112 112] FOUPID>	 [ASCII112 112 bytes] FOUP ID
         
        */
        public string ID    ;
        public string PortNo; //
        public string FOUPID; //


        public ST_C600_FOUP_ID_Read_Result_Report(string data = "")
        {
            this.ID     = "C600";
            this.PortNo = data;
            this.FOUPID = data; //16
        }
        public override string ToString() => $"{ID}{PortNo}{FOUPID}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R600_FOUP_ID_Read_Result_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string PORTNo => Encoding.ASCII.GetString(portno); 
        public string ACK    => Encoding.ASCII.GetString(ack   ); 
        public bool   bRtn ;

        public override string ToString() => $"{ID}{PORTNo}{ACK}";
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(portno, 0, portno.Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }

        //
        public ST_R600_FOUP_ID_Read_Result_Response(bool rtn = false)
        {
            id     = new byte[4];
            portno = new byte[2];
            ack    = new byte[1];

            bRtn = rtn;
        }
    }
    //------------------------------------------------------------------------
    public struct ST_C601_FOUP_ID_write_Result_Report
    {
        public string ID    ;
        public string PortNo; //
        public string FOUPID; //


        public ST_C601_FOUP_ID_write_Result_Report(string data)
        {
            this.ID     = "C601";
            this.PortNo = data;
            this.FOUPID = data; //16
        }
        public override string ToString() => $"{ID}{PortNo}{FOUPID}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R601_FOUP_ID_write_Result_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string PORTNo => Encoding.ASCII.GetString(portno); 
        public string ACK    => Encoding.ASCII.GetString(ack   );
        
        public override string ToString() => $"{ID}{PORTNo}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(portno, 0, portno.Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }

        //
        public ST_R601_FOUP_ID_write_Result_Response(bool rtn = false)
        {
            id     = new byte[4];
            portno = new byte[2];
            ack    = new byte[1];

            bRtn   = rtn;
        }
    }
    //------------------------------------------------------------------------
    public struct ST_C602_Port_Status_Report
    {
        /*
        <A[2] PortNo>
        <A[1] PortStatus>   Port status 0: Port disabled 1: Load Request (LDRQ) 2: Load Complete (LDCM) 3: Unload Request (UDRQ) 4: Unload Complete (UDCM)
        <A[112 112] FOUPID>
        <A[2] Mode>
                            //Load port FOUP operating mode
                            //00: Buffer
                            //01: only Source (Load)
                            //10: only Target (Unload)
                            //11: both Source and Target
        */

        public string ID        ;
        public string PortNo    ; 
        public string PortStatus; 
        public string FOUPID    ; 
        public string Mode      ; 

        public ST_C602_Port_Status_Report(string data)
        {
            this.ID         = "C602";
            this.PortNo     = data;
            this.PortStatus = data;
            this.FOUPID     = data; //16
            this.Mode       = data; //16
        }
        public override string ToString() => $"{ID}{PortNo}{PortStatus}{FOUPID}{Mode}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R602_Port_Status_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id ;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string PORTNo => Encoding.ASCII.GetString(portno); 
        public string ACK    => Encoding.ASCII.GetString(ack   ); 

        public override string ToString() => $"{ID}{PORTNo}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(portno, 0, portno.Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }

        //
        public ST_R602_Port_Status_Response(bool rtn = false)
        {
            id     = new byte[4];
            portno = new byte[2];
            ack    = new byte[1];

            bRtn   = rtn;
        }
    }
    //------------------------------------------------------------------------
    public struct ST_C604_Port_Slot_Map_Report
    {
        /*
        <A [2] PortNo>	     Target port (01-04)
        <A [112 112] FOUPID> Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. If there is no FOUP, fill it with "*".
        <A [25] MAPRD>	     Each slot indicates the status of each slot by one of the following: 
                             0: No 
                             wafer: Normal wafer placement (Wafer) 
                             2: Crossed wafer placement (Crossed)
                             ?: Undefined 
                             W: Wafer Overlapping wafers
        */

        public string ID        ;
        public string PortNo    ; 
        public string FOUPID    ; 
        public string MAPRD     ; 

        public ST_C604_Port_Slot_Map_Report(string data)
        {
            this.ID         = "C604";
            this.PortNo     = data;
            this.FOUPID     = data; //16
            this.MAPRD      = data;
        }

        public override string ToString() => $"{ID}{PortNo}{FOUPID}{MAPRD}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R604_Port_Slot_Map_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] portno;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string PORTNo => Encoding.ASCII.GetString(portno); 
        public string ACK    => Encoding.ASCII.GetString(ack   ); 

        public override string ToString() => $"{ID}{PORTNo}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(portno, 0, portno.Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }
        //
        public ST_R604_Port_Slot_Map_Response(bool rtn = false)
        {
            id     = new byte[4];
            portno = new byte[2];
            ack    = new byte[1];

            bRtn   = rtn;
        }
    }

    //------------------------------------------------------------------------
    public struct ST_C605_Robot_Status_Report
    {
        /*
        <A [1] RobotStatus>	0 = idle 1 = Ready 2 = Run 3 = Alarm 4 = manual 5 = Pause
        */

        public string ID        ;
        public string RBStatus  ; 

        public ST_C605_Robot_Status_Report(string data)
        {
            this.ID         = "C605";
            this.RBStatus   = data;
        }

        public override string ToString() => $"{ID}{RBStatus}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R605_Robot_Status_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string ACK    => Encoding.ASCII.GetString(ack   ); 

        public override string ToString() => $"{ID}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }

        //
        public ST_R605_Robot_Status_Response(bool rtn = false)
        {
            id   = new byte[4];
            ack  = new byte[1];

            bRtn = rtn;
        }
    }

    //------------------------------------------------------------------------
    public struct ST_C611_Panel_ID_Reading_Status_Report
    {
        /*
        <A [2] PortNo>	 Target port (01-04)
        <A [2] SlotNo>	 Target slot (01-25)
        <A [16] PanelID> The part of the panel ID that is not enough for the maximum number of characters is filled with spaces. If the panel ID reading fails, fill it with "*".
        <A [1] Readout>	 Type of read operation 
                            0: Invalid 
                            1: Read 
                            2: Key input 
                            3: Automatic input by EFEM
        */

        public string ID        ;
        public string PortNo    ; 
        public string SlotNo    ; 
        public string PanelID   ; 
        public string ReadOut   ; 

        public ST_C611_Panel_ID_Reading_Status_Report(string data)
        {
            this.ID        = "C611";
            this.PortNo    = data;
            this.SlotNo    = data;
            this.PanelID   = data;
            this.ReadOut   = data;
        }

        public override string ToString() => $"{ID}{PortNo}{PanelID}{ReadOut}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R611_Panel_ID_Reading_Status_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string ACK    => Encoding.ASCII.GetString(ack   ); 

        public override string ToString() => $"{ID}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }

        //
        public ST_R611_Panel_ID_Reading_Status_Response(bool rtn = false)
        {
            id   = new byte[4];
            ack  = new byte[1];

            bRtn = rtn;
        }

    }

    //------------------------------------------------------------------------
    public struct ST_C612_Panel_CCD_Alignment_Status_Report
    {
        /*
        <A [2] PortNo>	        Target port (01-04)
        <A [2] SlotNo>	        Target slot (01-25)
        <A [16] PanelID>	    Target panel ID Fill in the space that is not enough for the maximum number of characters. If there is no panel ID, fill it with "*".
        <A [1] Align. Status>	Panel CCD Alignment state (1: OK / ≠ 1: Fail)
        */

        public string ID        ;
        public string PortNo    ; 
        public string SlotNo    ; 
        public string PanelID   ; 
        public string AStatus   ; 

        public ST_C612_Panel_CCD_Alignment_Status_Report(string data = "")
        {
            this.ID        = "C612";
            this.PortNo    = data;
            this.SlotNo    = data;
            this.PanelID   = data;
            this.AStatus   = data;
        }

        public override string ToString() => $"{ID}{PortNo}{SlotNo}{PanelID}{AStatus}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R612_Panel_CCD_Alignment_Status_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string ACK    => Encoding.ASCII.GetString(ack   );  //Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

        public override string ToString() => $"{ID}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }

        //
        public ST_R612_Panel_CCD_Alignment_Status_Response(bool rtn = false)
        {
            id   = new byte[4];
            ack  = new byte[1];

            bRtn = rtn;
        }
    }
    //------------------------------------------------------------------------
    public struct ST_C631_EFEM_Status_Report
    {
        /*
        <A [25] EFEM Status>	Structure similar to <EFEM Status> of C031
                                Shows the status of the following 16 items in order by 1 to 3 characters 
                                ・ Operation Mode 0: Standalone / 1: Inline 
                                ・ EFEM Status 1: Ready / 0: Preparing or error 
                                ・ Robot Upper Arm Panel 
                                ・ Robot Lower Arm Panel 0: No panel /n(1-4): PortnThere is a panel in / D: Disabled 
                                ・ EQ1 Online Status 
                                ・ EQ2 Online Status 0: Offline / 1: Online 
                                ・ Load Port 1 Transport Mode 
                                ・ Load Port 2 Transport Mode 
                                ・ Load Port 3 Transport Mode 
                                ・ Load Port 4 Transport Mode 0: Auto / 1: Manual / D: Invalid or not installed 
                                ・ T1 Time out value (2 characters: 01 to 30) 
                                ・ T2 Time out value (3 characters: 060 to 240) 
                                ・ D1 Time out value (2 characters: 20 to 60) 
                                ・ D2 Time out value (2 characters: 20-60) 
                                ・ D3 Time out value (2 characters: 03-10) 
                                ・ Signal Tower Status (4 characters each for R, Y, G, B) 0: Off / 1: Lit / 2: Flashing
        */

        private string ID        ;
        public  string EFEMStatus;

        public ST_C631_EFEM_Status_Report(string data = "")
        {
            this.ID         = "C631";
            this.EFEMStatus = data;
        }

        public override string ToString() => $"{ID}{EFEMStatus}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R631_EFEM_Status_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string ACK    => Encoding.ASCII.GetString(ack   );  //Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

        public override string ToString() => $"{ID}{ACK}";
        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }
        //
        public ST_R631_EFEM_Status_Response(bool rtn = false)
        {
            id   = new byte[4];
            ack  = new byte[1];

            bRtn = rtn;
        }

    }

    //------------------------------------------------------------------------
    public struct ST_C632_Panel_Transportation_Report
    {
        /*
        <A [1] ARM>	            Arm used 1: Upper Arm 2: Lower Arm
        <A [1] ARM Action>	    Type of arm movement P: Panel storage (Put) G: Panel acquisition (Get) F: Operation completed (Finish) A: Suspended (Abort)
        <A [1] Position>	    Transfer target (1: load port side / 2: device side)
        <A [2] UNIT NO>	        Target unit <Position> = 1, target port (01 to 04) <Position> = 2, target device (01 to)
        <A [2] UNIT EXT>	    Target unit details If <Position> = 1, target slot (01 to 25), if <Position> = 2, target stage (01 to 02)
        <A [16] PanelID>	    Target panel ID Fill in the space that is not enough for the maximum number of characters.
        <A [16] SourceFOUPID>	Panel acquisition source FOUP ID Valid only in any of the following cases ・ <ARM Action> = G and <Position> = 1 ・ <ARM Action> = P and <Position> = 2
        <A [2] SourcePortNo>	Panel acquisition source port (01 to 04) Valid only in any of the following cases ・ <ARM Action> = G and <Position> = 1 ・ <ARM Action> = P and <Position> = 2
        <A [2] SourceSlotNo>	Panel acquisition source slot (01 to 25) Valid only in any of the following cases ・ <ARM Action> = G and <Position> = 1 ・ <ARM Action> = P and <Position> = 2

        */

        private string ID          ;
        public string ARMNo        ;
        public string ARMAction    ;
        public string PositionNo   ;
        public string UNITNo       ;
        public string UNITExt      ;
        public string PanelID      ;
        public string SrcFOUPID    ;
        public string SrcPortNo    ;
        public string SrcSlotNo    ;

        public ST_C632_Panel_Transportation_Report(string data = "")
        {
            this.ID         = "C632";
            this.ARMNo      = "1" ;
            this.ARMAction  = data;
            this.PositionNo = data;
            this.UNITNo     = data;
            this.UNITExt    = data;
            this.PanelID    = data;
            this.SrcFOUPID  = data;
            this.SrcPortNo  = data;
            this.SrcSlotNo  = data;
        }

        public override string ToString() => $"{ID}{ARMNo}{ARMAction}{PositionNo}{UNITNo}{UNITExt}{PanelID}{SrcFOUPID}{SrcPortNo}{SrcSlotNo}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R632_Panel_Transportation_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string ACK    => Encoding.ASCII.GetString(ack   );  //Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

        public override string ToString() => $"{ID}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }
        
        //
        public ST_R632_Panel_Transportation_Response(bool rtn = false)
        {
            id   = new byte[4];
            ack  = new byte[1];

            bRtn = rtn;
        }

    }
    //------------------------------------------------------------------------
    public struct ST_C690_Alarm_Event_Report
    {
        /*
        <A [1] UNIT>	    Unit (0: EFEM / 1: Robot / 2: Load port)
        <A [2] UNIT EXT>	Unit details When <UNIT> = 2, the target port (01 to 04) When <UNIT> ≠ 2, "**"
        <A [1] Error Label>	1: Reset possible 2: Reset not possible
        <A [4] Error Code>	Error code (Refer to Error Code List in communication specifications)
        <A [1] Set>	        Alarm set / reset (0: reset / 1: set)

        */

        private string ID        ;
        public string UNIT      ;
        public string UNITExt   ;
        public string ErrLabel  ;
        public string ErrCode   ;
        public string SET       ;

        public ST_C690_Alarm_Event_Report(string data = "")
        {
            this.ID         = "C690";
            this.UNIT       = data;
            this.UNITExt    = data;
            this.ErrLabel   = data;
            this.ErrCode    = data;
            this.SET        = data;
        }

        public override string ToString() => $"{ID}{UNIT}{UNITExt}{ErrLabel}{ErrCode}{UNITExt}{SET}";
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ST_R690_Alarm_Event_Response
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] ack;

        //
        public string ID     => Encoding.ASCII.GetString(id    );
        public string ACK    => Encoding.ASCII.GetString(ack   );  //Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

        public override string ToString() => $"{ID}{ACK}";

        public bool bRtn;
        public void Clear()
        {
            Array.Clear(id    , 0, id.    Length);
            Array.Clear(ack   , 0, ack.   Length);

            bRtn = false;
        }
        //
        public ST_R690_Alarm_Event_Response(bool rtn = false)
        {
            id   = new byte[4];
            ack  = new byte[1];

            bRtn = rtn;
        }
    }

    /************************************************************************/
    /* Structure                                                            */
    /************************************************************************/
    public struct ST_TCPIP_CMD
    {
        public string Id;
        public string Msg;

        public ST_TCPIP_CMD(string id)
        {
            Id  = id;
            Msg = string.Empty;

        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public struct ST_SENDED_DADT
    {
        public int      nRetryCnt ;
        public DateTime dtSendTime;
        public string   sMsg      ;
        public string   Id        ;
        public bool     bSended   ;
    }
    
    
    /************************************************************************/
    /* Pick Info                                                            */
    /************************************************************************/
    public struct ST_TRANSFER_INFO
    {
        /*
        <<Rcv
        <A[2] EQNo                          > Communication parameter EQNo
        <A[1] Get from / Put into EQ / FOUP > 0 = FOUP 1 = EQ
        <A[2] STAGEID> Target STAGE ID      > If only one stage, < STAGEID > always ”00” If multi stage, then<STAGEID> is 01,02,… ..
        <A[2] Port / EQ No.                 > If<A[1] Get from EQ / FOUP> = 0(FOUP)-- > 01 = Load Port 1 02 = Load Port 2 03 = Load Port 3    04 = Load Port 4 
                                              If<A[1] Get from EQ / FOUP> = 1(EQ)  -- > 01 = EQ 1        02 = EQ 2        03 = EQ 3(Reserved) 04 = EQ 4(Reserved)
        <A[2] Slot No.                      > Slot No. (01, 02, ……)
        <A[1] Upper / Lower ARM             > 0 = Upper Arm 1 = Lower Arm
        <A[1] Transport Mode                > 0 = Get
                                              1 = Put
                                              2 = Exchange(Upper arm get panel from EQ and Lower arm put panel into EQ, In<A[1] Upper / Lower ARM> only can choose ”0” (UpperArm))
                                              3 = CCD_Align_position(only) panel CCD Align.) 
                                              4 = Read_position(only read panel 2D Code)
                                              5 = Remove_position(Special case use only)
        <A[1] Option Parameter              > (Only for < A[1] Transport Mode > is Get) 
                                              0 = Do nothing, or not Get mode 
                                              1 = CCD Alignment 
                                              2 = Read 2D code 
                                              3 = CCD Alignment + Read 2D code
        */
        public bool bFind    ;
        public int  nEQNo    ;
        public int  nSource  ; //0 = FOUP 1 = EQ
        public int  nStageId ;
        public int  nTargetNo; //FOUP> = 0(FOUP)-- > 01 = Load Port 1 02 = Load Port 2 03 = Load Port 3    04 = Load Port 4 
                               //FOUP> = 1(EQ)  -- > 01 = EQ 1        02 = EQ 2        03 = EQ 3(Reserved) 04 = EQ 4(Reserved)
        public int  nSlotNo  ; //Slot No. (01, 02, ……)
        public int  nArmNo   ; //0 = Upper Arm 1 = Lower Arm
        public int  nTRMode  ; //Transport Mode  > 0 = Get 1 = Put
        public int  nOption  ;

        public ST_TRANSFER_INFO(bool find)
        {

            bFind     = find ; 
            nEQNo     = -1   ;
            nSource   = -1   ;
            nStageId  = -1   ;
            nTargetNo = -1   ;
            nSlotNo   = -1   ;
            nArmNo    = -1   ;
            nTRMode   = -1   ;
            nOption   = -1   ;
        }
        //------------------------------------------------------------------------
        public void Clear()
        {
            bFind     = false ; 
            nEQNo     = -1    ;
            nSource   = -1    ;
            nStageId  = -1    ;
            nTargetNo = -1    ;
            nSlotNo   = -1    ;
            nArmNo    = -1    ;
            nTRMode   = -1    ;
            nOption   = -1    ;
        }
        //------------------------------------------------------------------------
        public string GetLogData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[R132] ");
            sb.Append($"EQNo={nEQNo:D2}/");
            sb.Append(string.Format("Where={0}_{1}/", nSource , nSource == 0 ? "FOUP" : "EQ"));
            sb.Append($"Stage={nStageId:D2}/");
            sb.Append($"Taget={nTargetNo:D2}/");
            sb.Append($"SlotNo={nSlotNo:D2}/");
            sb.Append(string.Format("ArmNo={0}_{1}/" , nArmNo, nArmNo == 0 ? "Upper" : "Lower"));
            sb.Append(string.Format("TRMode={0}_{1}/", nTRMode, (EN_TR_MODE)nTRMode));
            sb.Append($"Option={nOption}");

            return sb.ToString(); 
        }

        //------------------------------------------------------------------------
        public bool CheckData()
        {
            bFind = true;

            if (nEQNo < 0 || nSource < 0 || nStageId < 0 || nTargetNo < 0 || nSlotNo < 0 || nTRMode < 0) bFind = false;

            return bFind;
        }
        //------------------------------------------------------------------------
        public int GetPortNo()
        {
            //FOUP = 0 & Get Mode = 0 || nTRMode == 1
            //if (nSource == 0 && (nTRMode == 0 || nTRMode == 1)) 
            if (GetSource() == EN_TARGET_SOURCE.FOUP && (GetTRMode() == EN_TR_MODE.Get || GetTRMode() == EN_TR_MODE.Put)) 
            {
                if (nTargetNo > 0 && nTargetNo < 5) //01~04
                {
                    return nTargetNo - 1;
                }
            }

            return -1;

        }
        //------------------------------------------------------------------------
        public int GetPickSlotNo(EN_MGZ_ID id)
        {
            bFind = false; 
            
            if (nSource == 0 && nTRMode == 0) //FOUP = 0 & Get Mode = 0
            {
                if(nTargetNo > 0 && nTargetNo < 5) //01~04
                {
                    if(nSlotNo > 0 && nSlotNo <= cDEF.FM.ProjBase.iMaxMgzSlot[(int)id])
                    {
                        if ((int)id == nTargetNo - 1)
                        {
                            bFind = true;
                            return nSlotNo - 1;
                        }
                    }
                }
            }

            return -1; 
        }
        //------------------------------------------------------------------------
        public int GetPlcekSlotNo(EN_MGZ_ID id)
        {
            bFind = false;
            
            if (IsPlaceFOUP()) //FOUP = 0 & Get Mode = 1(PUT) 
            {
                if (nTargetNo > 0 && nTargetNo < 5) //01~04
                {
                    if (nSlotNo > 0 && nSlotNo <= cDEF.FM.ProjBase.iMaxMgzSlot[(int)id])
                    {
                        if ((int)id == nTargetNo - 1)
                        {
                            bFind = true; 
                            return nSlotNo - 1;
                        }
                    }
                }
            }

            return -1;
        }
        //------------------------------------------------------------------------
        public bool GetPlcekSlotNo(ref int slot)
        {
            slot = -1; 

            if (IsPlaceFOUP()) //FOUP = 0 & Get Mode = 1(PUT) 
            {
                if (nTargetNo > 0 && nTargetNo < 5) //01~04
                {
                    if (nSlotNo > 0 && nSlotNo <= cDEF.FM.ProjBase.iMaxMgzSlot[0])
                    {   
                        slot = nSlotNo - 1;
                        return true; 
                    }
                }
            }

            return false;
        }

        //------------------------------------------------------------------------
        public bool GetPickMCNo(ref EN_WTR_WORK_AREA targetno)
        {
            targetno = EN_WTR_WORK_AREA.None;

            if (IsPickEQ()) 
            {
                if (nTargetNo > 0 && nTargetNo < 5) //01~04
                {
                    targetno = (EN_WTR_WORK_AREA)(nTargetNo + 2);
                    return true; 
                }
            }

            return false; 
        }

        //------------------------------------------------------------------------
        public bool GetPlaceMCNo(ref EN_WTR_WORK_AREA targetno)
        {
            targetno = EN_WTR_WORK_AREA.None;

            if (IsPlaceEQ()) //if (nSource == 1 && nTRMode == 1) //EQ & Get Mode = 1(PUT) 
            {
                if (nTargetNo > 0 && nTargetNo < 5) //01~04
                {
                    targetno = (EN_WTR_WORK_AREA)(nTargetNo + 2);
                    return true; 
                }
            }
            return false;
        }
        //------------------------------------------------------------------------
        public EN_TR_MODE GetTRMode()
        {
            if(nSource == 1 || nSource == 0)
            {
                return (EN_TR_MODE)nTRMode; 
            }

            return EN_TR_MODE.none;
        }
        //------------------------------------------------------------------------
        public EN_TARGET_SOURCE GetSource()
        {
            return (EN_TARGET_SOURCE)nSource;
        }
        //------------------------------------------------------------------------
        public bool IsPickFOUP()
        {
            bool r1 = nSource == (int)EN_TARGET_SOURCE.FOUP; 
            bool r2 = GetTRMode() == EN_TR_MODE.Get;

            return r1 && r2;
        }
        //------------------------------------------------------------------------
        public bool IsPlaceFOUP()
        {
            bool r1 = nSource == (int)EN_TARGET_SOURCE.FOUP;
            bool r2 = GetTRMode() == EN_TR_MODE.Put;

            return r1 && r2;
        }
        //------------------------------------------------------------------------
        public bool IsPickEQ()
        {
            bool r1 = nSource == (int)EN_TARGET_SOURCE.EQ;
            bool r2 = GetTRMode() == EN_TR_MODE.Get;

            return r1 && r2;
        }
        //------------------------------------------------------------------------
        public bool IsPlaceEQ()
        {
            bool r1 = nSource == (int)EN_TARGET_SOURCE.EQ;
            bool r2 = GetTRMode() == EN_TR_MODE.Put;

            return r1 && r2;
        }


    }


}
