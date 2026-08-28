using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using static eMachine.TLogUnit;
using System.Security.Cryptography;

namespace eMachine
{

    /***************************************************************************/
    /* Class: TRfid                                                            */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public enum EN_RFID_TCP_COMD {
        tcNone    = -1 ,
        tcENQ     = 0  ,
        EndOfId
    };
    public enum EN_BYTE_COUNT : int
    {
        ByteCnt0  = 0  ,
        ByteCnt1  = 1  ,
        ByteCnt2  = 2  ,
        ByteCnt3  = 3  ,
        ByteCnt4  = 4  ,
        ByteCnt5  = 5  ,
        ByteCnt6  = 6  ,
        ByteCnt7  = 7  ,
        ByteCnt8  = 8  ,
        ByteCnt9  = 9  ,
        ByteCnt10 = 10 ,
        ByteCnt11 = 11 ,
        ByteCnt12 = 12 ,
        ByteCnt13 = 13 ,
        ByteCnt14 = 14 ,
        ByteCnt15 = 15 ,
        ByteCnt16 = 16 ,
        ByteCnt28 = 28 ,
        ByteAll   = 112,
        EndOfId
    };
    //Command ID.
    //===========================================================================
    public enum EN_RFID_ID
    {
        riCh1  = 0 , //
        //riCh2  , //
        //riCh3  ,
        EndOfId
    }

    //Command ID.
    //===========================================================================
    public enum EN_RFID_CMD
    {
        rfcRead1  = 0 , //Read.  Module #1
        rfcRead2  = 1 , //Read.  Module #2
        rfcWrite1 = 2 , //Write. Module #1
        rfcWrite2 = 3 , //Write. Module #2
        rfcNone   = 4 ,
        EndOfCmd
    };

    //RFID. Parameter.
    //===========================================================================
    public class TRFID_PARA_BUFF
    {
        public int         Id ;
        public EN_RFID_CMD Cmd;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TRFID_PARA_BUFF()
        {
        }
        ~TRFID_PARA_BUFF() { }
        public void Init()
        {
            Id = -1;
            Cmd = EN_RFID_CMD.rfcNone;
        }
    };

    public struct _RF_TAG_DATA
    {
        public String TrayId    ;
        public String EqpNo     ;
        public String Qty       ;
        public String TQty      ;
        public String LotId     ;
        public String Step      ;
        public String LotType   ;
        public String PartNo    ;
        public String PkgCode   ;
        public String SecLotId  ;
        public String Cnt       ;
        public String LotTrayCnt;
    };

    //Rfid. Data List.
    //===========================================================================
    public unsafe class TRFCMD_SET
    {
        public EN_RFID_CMD Cmd           ; //Command ID.
        public char[] cName = new char[9]; //Command Name.
        public int iRxLen                ; //Reponse Length.
        public char[] cMNum = new char[3]; //Module Number.
        public char[] cCmd  = new char[3]; //Cmmmand.
        public char[] cAdd  = new char[3]; //Address.
        public char[] cLen  = new char[3]; //Len  (Only Read)
        public string cData              ; //Data (Only Write)
        public char[] cCS   = new char[3];
        public uint iMNum                ; //S - Ver. Module Number.
        public uint iCmd                 ; //S - Ver. Cmmmand.
        public uint iAdd                 ; //S - Ver. Address.
        public uint iLen                 ; //S - Ver. Len (Only Read)
        public uint iCS                  ; //S - Ver. Check Sum.

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public unsafe TRFCMD_SET(EN_RFID_CMD icmd, string sName, int iReLen, string sMNum, string sCmd, string sAdd, string sLen, string sCS, string sData, uint uiMNum, uint uiCmd, uint uiAdd, uint uiLen, uint uiCS)
        {
            Cmd    = icmd                 ;
            cName  = sName.ToCharArray()  ;
            iRxLen = iReLen               ;
            cMNum  = sMNum.ToCharArray()  ;
            cCmd   = sCmd .ToCharArray()  ;
            cAdd   = sAdd .ToCharArray()  ;
            cLen   = sLen .ToCharArray()  ;
            cData  = sData                ;     
            cCS    = sCS  .ToCharArray()  ;
            iMNum  = uiMNum               ;
            iCmd   = uiCmd                ;
            iAdd   = uiAdd                ;
            iLen   = uiLen                ;
            iCS    = uiCS                 ;
        }

        ~TRFCMD_SET() { }
    };

    public class TRFID_Ceyon
    {
        //Max.
        //===========================================================================
        public const int MAX_BUFFER = 1000;

        //---------------------------------------------------------------------------             
        public const int BITSIZEOF_BYTE    = 8  ;                                                       
        public const int MELSECBUFFER_SIZE = 112;                                                       
        public const int NUMBEROF_ADDRNO   = 12 ;                                                       
        public const int ACK_LENGTH        = 5  ;

        //각 Part별 Start Address No.                                                             
        public const int ADDR_TRAYID       =   0;    // m_sTrayId      00~07       Tray의 고유번호로써, RFID Tag의 전면에 Marking됨.    8201234                                                   
        public const int ADDR_EQPID        =   8;    // m_sEqpNo       08~15       해당 Lot이 Dispatching받은 설비 번호
        public const int ADDR_QTY          =  16;    // m_sQty         16~21       현재 Lot의 Tray 묶음별 Qty                           10250
        public const int ADDR_TQTY         =  22;    // m_sTQty        22~27       Total Qty : Lot안의 총 Chip 수량                     23800
        public const int ADDR_LOTID        =  28;    // m_sLotId       28~39       LotID                                                WZD034F43
        public const int ADDR_STEP         =  40;    // m_sStep        40~45       해당 STEP 정보                                       T130
        public const int ADDR_TYPE         =  46;    // m_sLotType     46~47       자재 진행여부 판단을 위해 PP,PE,PQ,E%등의 정보       PQ
        public const int ADDR_PARTNO       =  48;    // m_sPartNo      48~75       Part No (최대 28자리까지 할당)                       KT41G084QR-HCW0000-WCX2LW
        public const int ADDR_PKG          =  76;    // m_sPkgCode     76~78       PKG Code                                             2LW
        public const int ADDR_SECLOTID     =  80;    // m_sSecLotId    80~91       Merge될 경우, Merge된 모 LotID를 기입                WZD034F43
        public const int ADDR_CNT          =  92;    // m_sCnt         92~99       한 Lot이 2개이상의 실물로 이동할 경우, 이들의 순번   1001(10개의 실물중1번째)
        public const int ADDR_NONESP1      = 100;
        public const int ADDR_TRAYCNT      = 108;    // m_sLotTrayCnt 108~111      Lot 안의 Tray 총 매수                                35


        //Objects.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TSerialUnit2[]       RS232  = new TSerialUnit2[(int)EN_RFID_ID.EndOfId];
        TRfidTCP[]           TCPIP  = new TRfidTCP    [(int)EN_RFID_ID.EndOfId];
        //Thread               TH     = null ; //new Thread      (Th_Proc);

        //private:   // Member Var.            
        string               m_sIPAddr;
        int                  m_iPort  ;

        //RFID. Send Command String.
        //===========================================================================
        List<TRFCMD_SET>             RfidCmdLists = new List<TRFCMD_SET>();
        Queue[]                      m_ParaQue    = new Queue[(int)EN_RFID_ID.EndOfId];    
        //TQueueCls<TRFID_PARA_BUFF>[] m_ParaQue    = new TQueueCls<TRFID_PARA_BUFF>(512)[1];
        TRFID_PARA_BUFF[]            m_TxParas    = new TRFID_PARA_BUFF[(int)EN_RFID_ID.EndOfId];
        TRFID_PARA_BUFF              m_TmpPara    = new TRFID_PARA_BUFF();   
        char[][]                     m_szTxBuff   = new char[(int)EN_RFID_ID.EndOfId][];
        char[][]                     m_szRxBuff   = new char[(int)EN_RFID_ID.EndOfId][];
        string                       m_sBuff;

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer[] TOutRFIDstep   = new TOnDelayTimer[(int)EN_RFID_ID.EndOfId];
        TOnDelayTimer[] m_ResetTimer   = new TOnDelayTimer[(int)EN_RFID_ID.EndOfId];
        TOnDelayTimer[] m_WatchTimer   = new TOnDelayTimer[(int)EN_RFID_ID.EndOfId];
        TOnDelayTimer[] m_MsgTimer     = new TOnDelayTimer[(int)EN_RFID_ID.EndOfId];
        TOnDelayTimer   m_tWait        = new TOnDelayTimer();
        TOnDelayTimer   m_tRecieveWait = new TOnDelayTimer();

        //Buffers.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int[]                    m_iRFIDstep = new int         [(int)EN_RFID_ID.EndOfId];
        bool[]                   m_bDuringTag= new bool        [(int)EN_RFID_ID.EndOfId];
        bool[]                   m_bNeedRcv  = new bool        [(int)EN_RFID_ID.EndOfId];
        string[]                 m_sRcvStr   = new string      [(int)EN_RFID_ID.EndOfId]; //Readed String.
        EN_INTERFACE_TYPE        m_iIntType; //Interface Type        
                                 
        int[]                    m_iWatchStep   = new int[(int)EN_RFID_ID.EndOfId]; //Update Step   - Read Cycle.
        int[]                    m_iMsgStep     = new int[(int)EN_RFID_ID.EndOfId]; //              - Tx Cycle.
        double                   m_wStrtLogTime           ; //              - Start Logging Time.
        //double                   m_wStrtTime              ;
        //double                   m_wScanTime              ;
        //
        //IntPtr                   m_pHandle                ;
        bool                     m_bDispLog               ;

        //protected: //Inheritable Vars.        

        //public:    //Direct Accessable Vars.  
        public EN_RFID_CMD[]     m_iSetCmd   = new EN_RFID_CMD [(int)EN_RFID_ID.EndOfId];
        public bool[]            m_bErrRead  = new bool        [(int)EN_RFID_ID.EndOfId];
        public bool[]            m_bErrWrite = new bool        [(int)EN_RFID_ID.EndOfId];

        public bool              m_bWatchOn; //              - Controller의 상태(PV,SV,ST)를 모니터링 할 것인지를 결정.
        public bool              m_bTraceOn; //              - 통신 정보 Trace.
        public bool              m_bLogging; //Temp Logging  - Flag.
        public _RF_TAG_DATA[]    WTAG        = new _RF_TAG_DATA[(int)EN_RFID_ID.EndOfId];
        public _RF_TAG_DATA[]    RTAG        = new _RF_TAG_DATA[(int)EN_RFID_ID.EndOfId];

        bool m_IsConnect; //Conection

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_INTERFACE_TYPE _iIntType { get { return m_iIntType; } }
        public bool              _bDispLog { get { return m_bDispLog; } set { m_bDispLog = value; } }
        public bool              _IsConnect { get { return m_IsConnect; } }

      
        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TRFID_Ceyon(EN_INTERFACE_TYPE interType = EN_INTERFACE_TYPE.TCPIP) //0 :Serial, 1 :TCP
        {
            TRFCMD_SET[] RfidCmd = new TRFCMD_SET[(int)EN_RFID_CMD.EndOfCmd];
                                                                            //icmd, sName, iReLen, sMNum, sCmd, sAdd, sLen, sCS, sData                   , uiMNum, uiCmd, uiAdd, uiLen, uiCS
            RfidCmd[(int)EN_RFID_CMD.rfcRead1 ] = new TRFCMD_SET(EN_RFID_CMD.rfcRead1 , "[READ ]", 0, "01", "80", "00", "70", "00", vDEF.chNON.ToString(), 0x01, 0x80, 0x00, 0x70, 0x00); 
            RfidCmd[(int)EN_RFID_CMD.rfcRead2 ] = new TRFCMD_SET(EN_RFID_CMD.rfcRead2 , "[READ ]", 0, "01", "81", "00", "70", "00", vDEF.chNON.ToString(), 0x01, 0x81, 0x00, 0x70, 0x00);
            RfidCmd[(int)EN_RFID_CMD.rfcWrite1] = new TRFCMD_SET(EN_RFID_CMD.rfcWrite1, "[WRITE]", 0, "01", "90", "08", "68", "00", vDEF.chNON.ToString(), 0x01, 0x90, 0x08, 0x68, 0x00);
            RfidCmd[(int)EN_RFID_CMD.rfcWrite2] = new TRFCMD_SET(EN_RFID_CMD.rfcWrite1, "[WRITE]", 0, "01", "91", "08", "68", "00", vDEF.chNON.ToString(), 0x01, 0x91, 0x08, 0x68, 0x00);
            RfidCmd[(int)EN_RFID_CMD.rfcNone  ] = new TRFCMD_SET(EN_RFID_CMD.rfcNone  , ""       , 0, ""  , ""  , ""  , ""  , "00", vDEF.chNON.ToString(), 0x00, 0x80, 0x00, 0x00, 0x00);
            //
            for (int n = 0; n < (int)EN_RFID_CMD.EndOfCmd; n++) RfidCmdLists.Add(RfidCmd[n]);
            //
            for (int n = 0; n < (int)EN_RFID_ID.EndOfId; n++) 
            { 
                m_szTxBuff  [n] = new char[512];
                m_szRxBuff  [n] = new char[512];
                TOutRFIDstep[n] = new TOnDelayTimer();
                m_ResetTimer[n] = new TOnDelayTimer();
                m_WatchTimer[n] = new TOnDelayTimer();
                m_MsgTimer  [n] = new TOnDelayTimer();
                //
                m_iSetCmd   [n] = new EN_RFID_CMD ();
                WTAG        [n] = new _RF_TAG_DATA();
                RTAG        [n] = new _RF_TAG_DATA();
                //
                m_iIntType = interType;
                RS232       [n] = null;
                TCPIP       [n] = null;
                m_ParaQue   [n] = new Queue();
                m_TxParas   [n] = new TRFID_PARA_BUFF();

                //
                m_sRcvStr    [n] = ""   ;
                m_bNeedRcv   [n] = false;
                m_bErrRead   [n] = false;
                m_bErrWrite  [n] = false;
                m_iRFIDstep  [n] = 0    ;
                m_iSetCmd    [n] = EN_RFID_CMD.rfcNone;
            }

            m_sIPAddr       = ""  ;
            m_iPort         = 1024;
            m_bDispLog      = false;
            
            //
            //Init(interType, EN_COMM_TYPE.RcvEndChr, EN_RS232_PORT_NO.COM3);
            
            //
            //TH = new Thread(Th_Proc);
            //TH.Start();
        }
        //--------------------------------------------------------------------------
        ~TRFID_Ceyon() 
        {
            //for (int n = 0; n < (int)EN_RFID_ID.EndOfId; n++)
            //{
            //    if (TCPIP[n] != null) {                       TCPIP[n].Close     (); }
            //    if (RS232[n] != null) { if (RS232[n]._IsOpen) RS232[n].Port_Close(); }
            //    RS232[n] = null; 
            //}
            ////
            //TH.Join();
        }
        //------------------------------------------------------------------------
        private void Th_Proc()
        {
            //while (TH.IsAlive && TH != null)
            //{                
            //    Update();
            //    Thread.Sleep(100);
            //}
        }
        //-------------------------------------------------------------------
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void   Init (EN_INTERFACE_TYPE interType)
        {
            //
            m_sIPAddr       = "192.168.100.50";
            m_iPort         = 1470;

            //RS232
            EN_COMM_TYPE     iCommType = EN_COMM_TYPE.RcvEndChr;
            EN_RS232_PORT_NO iPortNo   = EN_RS232_PORT_NO.COM3;


            //
            for (int n = 0; n < (int)EN_RFID_ID.EndOfId; n++)
            { 
                if (interType == EN_INTERFACE_TYPE.SEARIAL)
                {
                    RS232[n] = new TSerialUnit2();
                    RS232[n].Open(iCommType, Enum.GetName(typeof(EN_RS232_PORT_NO),iPortNo + n), 9600, 8, Parity.None, StopBits.One, 5000);
                    RS232[n]._cEndChar = vDEF.chETX;
                    RS232[n]._hWnd     = IntPtr.Zero;
                }
                else if (interType == EN_INTERFACE_TYPE.TCPIP)
                {
                    TCPIP[n]    = new TRfidTCP(n, (int)EN_RFID_TCP_COMD.EndOfId);
                    m_IsConnect = TCPIP[n].Connect(m_sIPAddr, m_iPort + n);
                }
            }

            //
            m_bWatchOn     = true ; //            - Controller의 상태(PV,SV,ST)를 모니터링 할 것인지를 결정.
            m_bTraceOn     = false; //            - 통신 정보 Trace.
            m_bLogging     = false; //Temp Logging Flag.

            //
            Reset();
        }

        //-------------------------------------------------------------------
        //Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public bool IsConnected(EN_RFID_ID Id)
        {
            int iId = (int)Id;
            if      (m_iIntType == EN_INTERFACE_TYPE.SEARIAL)
            {
                return RS232[iId]._IsOpen;
            }
            else if (m_iIntType == EN_INTERFACE_TYPE.TCPIP  )
            {
                return TCPIP[iId]._bConnected;
            }
            return false;
        }
        //------------------------------------------------------------------------
        public void Reset()
        {
            //
            Clear();            
            //Init. Var.
            for (int n = 0; n < (int)EN_RFID_ID.EndOfId; n++) Reset((EN_RFID_ID)n);
        }
        //------------------------------------------------------------------------
        public void   Reset(EN_RFID_ID Id)
        {
            int iIdx = (int)Id;
            //Init. Var.
            Clear(Id);
            m_sRcvStr    [iIdx] = ""   ;
            m_bNeedRcv   [iIdx] = false;
            m_bErrRead   [iIdx] = false;
            m_bErrWrite  [iIdx] = false;
            m_iRFIDstep  [iIdx] = 0    ;
            m_iSetCmd    [iIdx] = EN_RFID_CMD.rfcNone;
        }
        //------------------------------------------------------------------------
        public void   Clear()
        {            
            for (int n = 0 ; n < (int)EN_RFID_ID.EndOfId ; n++)
            { 
                //Clear Queue.
                m_ParaQue[n].Clear();
                //
                Clear((EN_RFID_ID)n);
            }
            //Init. Var.
            m_bLogging = false;
        }
        //------------------------------------------------------------------------
        public void   Clear(EN_RFID_ID Id)
        {
            int iIdx = (int)Id;
            if (Id <  EN_RFID_ID.riCh1  ) return;
            if (Id >= EN_RFID_ID.EndOfId) return;

            //Serial.    
            if (RS232[iIdx] != null) RS232[iIdx].DiscardInBuffer();
            if (TCPIP[iIdx] != null) TCPIP[iIdx].Init();

            //Clear Queue.
            m_ParaQue[(int)Id].Clear(); 

            //Init. Buffer.
            m_TxParas [iIdx].Init();
            Array.Clear(m_szTxBuff[iIdx], 0 , m_szTxBuff[iIdx].Length);
            Array.Clear(m_szRxBuff[iIdx], 0 , m_szRxBuff[iIdx].Length);

            //Array.Clear(m_TxParas , iIdx * m_TxParas .GetLength(0) , m_TxParas .GetLength(1));
            //Array.Clear(m_szTxBuff, iIdx * m_szTxBuff.GetLength(0) , m_szTxBuff.GetLength(1));
            //Array.Clear(m_szRxBuff, iIdx * m_szRxBuff.GetLength(0) , m_szRxBuff.GetLength(1));

            //Init. Var.
            m_bLogging = false;
            m_iMsgStep[iIdx] = 0;
        }
        //------------------------------------------------------------------------
        public void ClearWriteBuf(EN_RFID_ID Id)
        {
            int iIdx = (int)Id;
            //
            WTAG[iIdx].Step        = "";
            WTAG[iIdx].LotType     = "";
            WTAG[iIdx].PartNo      = "";
            WTAG[iIdx].EqpNo       = "";
            WTAG[iIdx].Qty         = "";
            WTAG[iIdx].TQty        = "";
            WTAG[iIdx].LotId       = "";
            WTAG[iIdx].PkgCode     = "";
            WTAG[iIdx].SecLotId    = "";
            WTAG[iIdx].Cnt         = "";
            WTAG[iIdx].LotTrayCnt  = "";
        }
        //------------------------------------------------------------------------
        public void CmdSetRFID(EN_RFID_ID ch, EN_RFID_CMD cmd)
        {
            if (ch >= EN_RFID_ID.EndOfId) return; 

            m_iSetCmd[(int)ch] = cmd;
        }
        //------------------------------------------------------------------------
        public void ClearReadBuf(EN_RFID_ID Id)
        {
            int iIdx = (int)Id;
            //
            RTAG[iIdx].Step        = "";
            RTAG[iIdx].LotType     = "";
            RTAG[iIdx].PartNo      = "";
            RTAG[iIdx].EqpNo       = "";
            RTAG[iIdx].Qty         = "";
            RTAG[iIdx].TQty        = "";
            RTAG[iIdx].LotId       = "";
            RTAG[iIdx].PkgCode     = "";
            RTAG[iIdx].SecLotId    = "";
            RTAG[iIdx].Cnt         = "";
            RTAG[iIdx].LotTrayCnt  = "";
        }
        //------------------------------------------------------------------------
        public void Close()
        {
            m_bTraceOn = false;

            for (int n = 0 ; n < (int)EN_RFID_ID.EndOfId ; n++) 
            { 
                if (RS232[n] != null) { RS232[n].Port_Close(); RS232[n] = null; }
                if (TCPIP[n] != null) { TCPIP[n].Close     (); TCPIP[n] = null; }
            }
        }
        //------------------------------------------------------------------------
        public void Close(EN_RFID_ID Id)
        {
            m_bTraceOn = false;

            for (int n = 0 ; n < (int)EN_RFID_ID.EndOfId ; n++) 
            { 
                if (RS232[n] != null) { RS232[n].Port_Close(); }
                if (TCPIP[n] != null) { TCPIP[n].Close     (); }
            }
        }
        //------------------------------------------------------------------------
        public void ReConnect(EN_RFID_ID Id)
        {
            if      (m_iIntType == EN_INTERFACE_TYPE.SEARIAL) RS232[(int)Id].ReOpen();
            else if (m_iIntType == EN_INTERFACE_TYPE.TCPIP  )
            {
                TCPIP[(int)Id].Close();
                Thread.Sleep(1000);
                TCPIP[(int)Id].Connect(m_sIPAddr, m_iPort);
            }
        }
        //------------------------------------------------------------------------
        public void Open (int iCh, int iCommType = (int)EN_COMM_TYPE.RcvEndChr, EN_RS232_PORT_NO iPortNo = EN_RS232_PORT_NO.COM2)
        {
            if (m_iIntType == EN_INTERFACE_TYPE.SEARIAL) 
            { 
                if (RS232[iCh] != null)
                {
                    if (!RS232[iCh]._IsOpen)
                    {
                        RS232[iCh].Open((EN_COMM_TYPE)iCommType,  Enum.GetName(typeof(EN_RS232_PORT_NO),iPortNo + iCh), 9600, 8, Parity.None, StopBits.One, 1000);
                        RS232[iCh]._cEndChar = vDEF.chETX;
                    }
                }
                else if (RS232[iCh] == null)
                {
                    RS232[iCh]            = new TSerialUnit2 ();
                    //RS232[iCh].OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);
                    RS232[iCh].Open((EN_COMM_TYPE)iCommType, Enum.GetName(typeof(EN_RS232_PORT_NO),iPortNo + iCh), 9600, 8, Parity.None, StopBits.One, 1000);                
                }
            }
            else if (m_iIntType == EN_INTERFACE_TYPE.TCPIP)
            {
                TCPIP[iCh].Connect(m_sIPAddr, m_iPort);
            }
        }
        //--------------------------------------------------------------------------
        public bool SndMsg(EN_RFID_ID Id)
        {
            //Local Var.
            bool bRet       = false;
            int iIdx        = (int)Id;
            int iTxLen      = -1;
            byte[] btTxData = new byte[512];
            string sMsg     = "";

            //Set Request Code.
            switch (m_iSetCmd[(int)Id]) 
            {
                case EN_RFID_CMD.rfcRead1  : iTxLen = m_MakeMsgRead (Id , m_iSetCmd[iIdx] , m_szTxBuff[iIdx]); break;
                case EN_RFID_CMD.rfcRead2  : iTxLen = m_MakeMsgRead (Id , m_iSetCmd[iIdx] , m_szTxBuff[iIdx]); break;
                case EN_RFID_CMD.rfcWrite1 : iTxLen = m_MakeMsgWrite(Id , m_iSetCmd[iIdx] , m_szTxBuff[iIdx]); break;
                case EN_RFID_CMD.rfcWrite2 : iTxLen = m_MakeMsgWrite(Id , m_iSetCmd[iIdx] , m_szTxBuff[iIdx]); break;
            }
        
            //Check Port.
            try
            {
                btTxData = FNC.GetCharArrayToByteArray(m_szTxBuff[iIdx], iTxLen); //Encoding.ASCII.GetBytes(m_szTxBuff[iIdx].ToString());
                sMsg     = FNC.GetByteArrayToString(btTxData, 0, iTxLen);
                //
                if (m_iIntType == EN_INTERFACE_TYPE.SEARIAL) 
                { 
                    if (RS232[iIdx] == null || !RS232[iIdx]._IsOpen) return false;
                    bRet = RS232[iIdx].SetStream(btTxData, iTxLen, RfidCmdLists[(int)m_TxParas[iIdx].Cmd].iRxLen, 10000);
                }
                else if (m_iIntType == EN_INTERFACE_TYPE.TCPIP)
                {
                    string sTemp = FNC.GetCharArrayToString(m_szTxBuff[iIdx], 0, iTxLen);
                    bRet = TCPIP[iIdx].MakeSendMsg((int)EN_RFID_TCP_COMD.tcENQ, sTemp) == EN_TCP_STAT.Sended;
                }

                //Trace.
                WriteLog(FNC.str2hex(sMsg), Id, true);
                
                //Return.
                return bRet;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[RFID][SndMsg]Exception:" + e.Message);
                return false;
            }
        }        
        //------------------------------------------------------------------------
        public bool RcvMsg(EN_RFID_ID Id)
        {
            //Local Var.
            int     iIdx = (int)Id;
            string  sMsg = ""; //Received Message.
            int     iRet;
            bool    bVerS = GetVer(Id) == "CAP1.3S"; //CAP1.3(ASCII), CAP1.3S(Binary)         
            byte[]  bRxBuff = new byte[1024];
            char[]  cRsp = new char[1024];

            //
            if (m_iIntType == EN_INTERFACE_TYPE.SEARIAL) 
            { 
                if ((RS232[iIdx] == null) || !RS232[iIdx].IsRcv()) return false;
                //
                iRet = RS232[iIdx].GetStream(ref bRxBuff); //get recv byte
                sMsg = FNC.GetByteArrayToString(bRxBuff, 0, iRet);  //byte->string
            }
            else if (m_iIntType == EN_INTERFACE_TYPE.TCPIP)
            {
                if ((TCPIP[iIdx] == null)                               ) return false;
                if (!TCPIP[iIdx].IsReceived((int)EN_RFID_TCP_COMD.tcENQ)) return false;

                sMsg = TCPIP[iIdx]._sRxData;
                iRet = sMsg.Length;
            }
            else return false;

            //
            cRsp = sMsg.ToCharArray();            
            if (cRsp[0       ] != vDEF.chSTX && cRsp[0] != vDEF.chACK && cRsp[0] != vDEF.chNAK) return false;
            if (cRsp[iRet - 1] != vDEF.chETX                                                  ) return false;
            //
            if (cRsp[0] == vDEF.chNAK)
            {
                
                Thread.Sleep(200);
            }
            //
            m_sRcvStr[iIdx] = sMsg;

            //
            WriteLog(FNC.str2hex(sMsg).ToString(), Id, false);

            //
            return (iRet > 0);            
        }

        //---------------------------------------------------------------------------
        public string GetVer(EN_RFID_ID Id)
        {
            switch (Id) 
            {
                case EN_RFID_ID.riCh1 : return "CAP1.3S";
                //case EN_RFID_ID.riCh2 : return "CAP1.3";
                //case EN_RFID_ID.riCh3 : return "CAP1.3";
            }
        
            return "";
        }
//
        //---------------------------------------------------------------------------
        public int m_MakeMsgRead(EN_RFID_ID Id , EN_RFID_CMD Cmd , char[] Buff)
        {
            //Local Bar.
            int  iIdx = (int)Id;
            int  iCmd = (int)Cmd;
            int  iLen = 0;
            //int  iSum = 0;
            bool bVerS = GetVer(Id) == "CAP1.3S";
        
            //
            if (bVerS) 
            {
                m_szTxBuff[iIdx][iLen++] = vDEF.chENQ                    ; RfidCmdLists[iCmd].iCS  = vDEF.chENQ              ;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iMNum; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iMNum;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iCmd ; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iCmd ;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iAdd ; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iAdd ;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iLen ; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iLen ;
        
                //Check Sum
                RfidCmdLists[iCmd].iCS = RfidCmdLists[iCmd].iCS%0x100;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iCS;
            }
            else 
            {
                //Set Basis Data #1.
                AttatchData(Id , vDEF.chENQ                  ); //STX.
                AttatchData(Id , RfidCmdLists[iCmd].cMNum , 2); //Unit No
                AttatchData(Id , RfidCmdLists[iCmd].cCmd  , 2); //Command
                AttatchData(Id , RfidCmdLists[iCmd].cAdd  , 2); //Address
                AttatchData(Id , RfidCmdLists[iCmd].cLen  , 2); //Len : 10진수 -> 112
        
                //Check Sum
                MakeCheckSum(m_szTxBuff[iIdx] , Cmd);
                iLen = AttatchData(Id , RfidCmdLists[iCmd].cCS , 2); //CheckSum
            }
        
            //Ok.
            return iLen;
        }
        //------------------------------------------------------------------------
        public unsafe int m_MakeMsgWrite(EN_RFID_ID Id , EN_RFID_CMD Cmd , char[] Buff)
        {
            //Local Bar.
            int    iIdx  = (int)Id;
            int    iCmd  = (int)Cmd;
            int    iLen  =   0                 ;
            int[]  iTemp = new int[(int)EN_BYTE_COUNT.ByteAll];
            string sStr  = ""                  ;
            string sTmp  = ""                  ;
            bool   bVerS = GetVer(Id) == "CAP1.3S";
        
            if (bVerS) {
                m_szTxBuff[iIdx][iLen++] = vDEF.chENQ                    ; RfidCmdLists[iCmd].iCS  = vDEF.chENQ              ;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iMNum; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iMNum;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iCmd ; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iCmd ;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iAdd ; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iAdd ;
                m_szTxBuff[iIdx][iLen++] = (char)RfidCmdLists[iCmd].iLen ; RfidCmdLists[iCmd].iCS += RfidCmdLists[iCmd].iLen ;

                RfidCmdLists[iCmd].cData = GoWrtieTag(Id);
                for (int n = 0 ; n < (int)EN_BYTE_COUNT.ByteAll; n++) {
                    if (RfidCmdLists[iCmd].cData[n] == 0x00) {
                        iLen += n;
                        break;
                        }
                    m_szTxBuff  [iIdx][iLen+n]  =       RfidCmdLists[iCmd].cData[n]; //Data Shift
                    RfidCmdLists[iCmd].iCS     += (uint)RfidCmdLists[iCmd].cData[n]; //Check Sum
                    }
                RfidCmdLists[iCmd].iCS     =       RfidCmdLists[iCmd].iCS % 0x100;
                m_szTxBuff  [iIdx][iLen++] = (char)RfidCmdLists[iCmd].iCS;
                }
            else {
                //Set Basis Data #1.
                AttatchData(Id , vDEF.chENQ                  ); //STX.
                AttatchData(Id , RfidCmdLists[iCmd].cMNum , 2); //Unit No
                AttatchData(Id , RfidCmdLists[iCmd].cCmd  , 2); //Command
                AttatchData(Id , RfidCmdLists[iCmd].cAdd  , 2); //Address
                AttatchData(Id , RfidCmdLists[iCmd].cLen  , 2); //Address
                sTmp = GoWrtieTag(Id);
                sStr = FNC.str2hex(sTmp);

                RfidCmdLists[iCmd].cData = sStr;
                AttatchData(Id , RfidCmdLists[iCmd].cData , (MELSECBUFFER_SIZE - ADDR_EQPID) * 2); //Len :
                //Check Sum
                MakeCheckSum(m_szTxBuff[iIdx] , Cmd);
                iLen = AttatchData(Id , RfidCmdLists[iCmd].cCS , 2); //ETX
                }
        
            //Ok.
            return iLen;
        }
        //------------------------------------------------------------------------
        public int AttatchData(EN_RFID_ID Id , int Data)
        {
            //Local Var.
            int iLast = 0;
        
            //Find Last.
            for (int n = 0 ; n < 512 ; n++) {
                if (m_szTxBuff[(int)Id][n] == 0x00) { iLast = n; break; }
                }
        
            //Check Max.
            if ((iLast + 1) >= 512) {
                Array.Clear(m_szTxBuff[(int)Id], 0, m_szTxBuff[(int)Id].Length);
                return 0;
                }
        
            char cTemp = (Char)Data;
        
            //Attatch.
            m_szTxBuff[(int)Id][iLast] = cTemp;
        
            //Ok.
            return (iLast + 1);
        }
        //------------------------------------------------------------------------
        public int AttatchData(EN_RFID_ID Id , char[] Data , int Cnt)
        {
            //Local Var.
            int iLast = 0;
        
            //Find Last.
            for (int n = 0 ; n < 512 ; n++) {
                if (m_szTxBuff[(int)Id][n] == 0x00) { iLast = n; break; }
                }
        
            //Check Max.
            if ((iLast + Cnt) >= 512) {
                Array.Clear(m_szTxBuff[(int)Id], 0, m_szTxBuff[(int)Id].Length);
                return 0;
                }
        
            //Attatch.
            //public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length);
            Array.Copy(Data, 0, m_szTxBuff[(int)Id], iLast,  Cnt);
        
            //Ok.
            return (iLast + Cnt);
        }
        //------------------------------------------------------------------------
        public int AttatchData(EN_RFID_ID Id , string Data , int Cnt)
        {
            //Local Var.
            int iLast = 0;
        
            //Find Last.
            for (int n = 0 ; n < 512 ; n++) 
            {
                if (m_szTxBuff[(int)Id][n] == 0x00) { iLast = n; break; }
            }
        
            //Check Max.
            if ((iLast + Cnt) >= 512) 
            {
                Array.Clear(m_szTxBuff[(int)Id], 0, m_szTxBuff[(int)Id].Length);
                return 0;
            }
        
            //Attatch.
            Array.Copy(Data.ToCharArray(), 0, m_szTxBuff[(int)Id], iLast, Cnt);
        
            //Ok.
            return (iLast + Cnt);
        }
        //------------------------------------------------------------------------

        public void MakeCheckSum(char[] Data , EN_RFID_CMD Cmd)
        {
            //Local Var.
            //int    iLen     ;
            int    iSum  = 0;
            int    iTemp = 0;
            char[] cTemp = new char[2];
            string sHex     ;
        
            //Sum
            for (int n = 0; n < 512 ; n++) {
                if (Data[n] == 0x00) break;
                iTemp = (int)Data[n];
                iSum += iTemp;
                }
        
            //Make Check Sum.
            iSum  = iSum % 0x100      ;
            sHex  = iSum.ToString("X");
            for (int n = 0; n<sHex.Length; n++)
            {
                cTemp[n] = (char)Int16.Parse(sHex.Substring(n, 1), NumberStyles.AllowHexSpecifier);
            }
        
            //Copy
            Array.Copy(cTemp, RfidCmdLists[(int)Cmd].cCS, 2);
        }
        //------------------------------------------------------------------------
        public bool UpdateReadData(EN_RFID_ID Id , int iAddr, int iByteSize, bool bAll)
        {
            int iIdx = (int)Id;
            string sTemp;
            if (m_sRcvStr[iIdx] == "") return false;
        
            if (bAll)
            {
                sTemp = GetData(Id , iAddr, iByteSize , GetVer(Id).Trim() == "CAP1.3S");
                RTAG[iIdx].TrayId     = GetBuff(sTemp , ADDR_TRAYID  , (int)EN_BYTE_COUNT.ByteCnt8 );
                RTAG[iIdx].EqpNo      = GetBuff(sTemp , ADDR_EQPID   , (int)EN_BYTE_COUNT.ByteCnt8 );
                RTAG[iIdx].Qty        = GetBuff(sTemp , ADDR_QTY     , (int)EN_BYTE_COUNT.ByteCnt6 );
                RTAG[iIdx].TQty       = GetBuff(sTemp , ADDR_TQTY    , (int)EN_BYTE_COUNT.ByteCnt6 );
                RTAG[iIdx].LotId      = GetBuff(sTemp , ADDR_LOTID   , (int)EN_BYTE_COUNT.ByteCnt12);
                RTAG[iIdx].Step       = GetBuff(sTemp , ADDR_STEP    , (int)EN_BYTE_COUNT.ByteCnt6 );
                RTAG[iIdx].LotType    = GetBuff(sTemp , ADDR_TYPE    , (int)EN_BYTE_COUNT.ByteCnt2 );
                RTAG[iIdx].PartNo     = GetBuff(sTemp , ADDR_PARTNO  , (int)EN_BYTE_COUNT.ByteCnt28);
                RTAG[iIdx].PkgCode    = GetBuff(sTemp , ADDR_PKG     , (int)EN_BYTE_COUNT.ByteCnt3 );
                RTAG[iIdx].SecLotId   = GetBuff(sTemp , ADDR_SECLOTID, (int)EN_BYTE_COUNT.ByteCnt12);
                RTAG[iIdx].Cnt        = GetBuff(sTemp , ADDR_CNT     , (int)EN_BYTE_COUNT.ByteCnt8 );
                RTAG[iIdx].LotTrayCnt = GetBuff(sTemp , ADDR_TRAYCNT , (int)EN_BYTE_COUNT.ByteCnt4 );
                //WriteLog(sTemp , Id , true);
            }
            else 
            {
                if      (iAddr==ADDR_TRAYID  ) { RTAG[iIdx].TrayId     = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_EQPID   ) { RTAG[iIdx].EqpNo      = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_QTY     ) { RTAG[iIdx].Qty        = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_TQTY    ) { RTAG[iIdx].TQty       = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_LOTID   ) { RTAG[iIdx].LotId      = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_STEP    ) { RTAG[iIdx].Step       = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_TYPE    ) { RTAG[iIdx].LotType    = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_PARTNO  ) { RTAG[iIdx].PartNo     = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_PKG     ) { RTAG[iIdx].PkgCode    = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_SECLOTID) { RTAG[iIdx].SecLotId   = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_CNT     ) { RTAG[iIdx].Cnt        = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
                else if (iAddr==ADDR_TRAYCNT ) { RTAG[iIdx].LotTrayCnt = GetData(Id , iAddr , iByteSize , GetVer(Id).Trim() == "CAP1.3S");}
            }

            return true;
        }
        //------------------------------------------------------------------------
        public string GoWrtieTag(EN_RFID_ID Id)
        {
            int Idx = (int)Id;

            m_sBuff = "";
            SetBuff(WTAG[Idx].EqpNo      , ADDR_EQPID    , (int)EN_BYTE_COUNT.ByteCnt8 );   // 8
            SetBuff(WTAG[Idx].Qty        , ADDR_QTY      , (int)EN_BYTE_COUNT.ByteCnt6 );   //14
            SetBuff(WTAG[Idx].TQty       , ADDR_TQTY     , (int)EN_BYTE_COUNT.ByteCnt6 );   //20
            SetBuff(WTAG[Idx].LotId      , ADDR_LOTID    , (int)EN_BYTE_COUNT.ByteCnt12);   //32
            SetBuff(WTAG[Idx].Step       , ADDR_STEP     , (int)EN_BYTE_COUNT.ByteCnt6 );   //38
            SetBuff(WTAG[Idx].LotType    , ADDR_TYPE     , (int)EN_BYTE_COUNT.ByteCnt2 );   //40
            SetBuff(WTAG[Idx].PartNo     , ADDR_PARTNO   , (int)EN_BYTE_COUNT.ByteCnt28);   //68
            SetBuff(WTAG[Idx].PkgCode    , ADDR_PKG      , (int)EN_BYTE_COUNT.ByteCnt4 );   //72 //실제 사용하는 ByteSize는 3이지만 4로 설정.(4번째 영역은 비어있음.)
            SetBuff(WTAG[Idx].SecLotId   , ADDR_SECLOTID , (int)EN_BYTE_COUNT.ByteCnt12);   //84
            SetBuff(WTAG[Idx].Cnt        , ADDR_CNT      , (int)EN_BYTE_COUNT.ByteCnt8 );   //92
            SetBuff("        "           , ADDR_NONESP1  , (int)EN_BYTE_COUNT.ByteCnt8 );   //100
            SetBuff(WTAG[Idx].LotTrayCnt , ADDR_TRAYCNT  , (int)EN_BYTE_COUNT.ByteCnt4 );   //104
        
          //RfidCmdLists[Cmd].cData = m_sBuff.c_str();
        
            //WriteLog(m_sBuff , Id);
        
            return m_sBuff;
        }
        //------------------------------------------------------------------------

        public void SetBuff(string Data, int AddrNo, int ByteSize)
        {
            //TrayId 부분을 제외하고 writing한다.
            string sTemp;
            AddrNo -= 8;
        
            int length = Data.Length;
        
            if (length > ByteSize) length = ByteSize;        
            if (ByteSize == 0) 
            {
                ByteSize = length * sizeof(ushort);
            }
            //
            sTemp = Data.Substring(0, Data.Length);
            m_sBuff += sTemp;
            //
            for (int i=AddrNo+length; i<AddrNo+ByteSize; i++) 
            {
                //나머지 뒤 난은 Space charater(0x20) 로 설정
                sTemp = vDEF.chSP.ToString();
                m_sBuff += sTemp;
            }
        }
        //------------------------------------------------------------------------
        public string GetBuff(string Buff, int Addr, int b_Size)
        {
            //Local Var.
            string rValue = ""         ;
            int    iLen = Addr + b_Size;
        
            //Check.
            if (iLen > Buff.Length) return "";
        
            //
            for(int i = Addr ; i < iLen ; i++) {
                rValue += Buff[i + 1];
                }
            rValue = rValue.Trim();
        
            //
            return rValue;
        
        }
        //------------------------------------------------------------------------
        public string GetData(EN_RFID_ID Id , int iAddr , int rByteCnt , bool VerS)
        {
            int    iIdx  = (int)Id;
            string sTemp = "";
            if ( m_sRcvStr[iIdx] == "") return sTemp;
        
            int iLen = m_sRcvStr[iIdx].Length;
            int icut = m_sRcvStr[iIdx].IndexOf(RfidCmdLists[(int)m_iSetCmd[iIdx]].iCmd.ToString());
            if (VerS) 
            {
                if (icut != 0) 
                {
                    m_sRcvStr[iIdx] = m_sRcvStr[iIdx].Substring(icut+1, iLen-(icut+1));
                }
                else 
                {
                    if (iLen < 4) return sTemp;
                    m_sRcvStr[iIdx] = m_sRcvStr[iIdx].Substring(1, iLen-3);
                }
            }
            else 
            {
                if (icut != 0) 
                {
                    m_sRcvStr[iIdx] = m_sRcvStr[iIdx].Substring(icut+2, iLen-(icut+3));
                }
                else 
                {
                    if (iLen < 4) return sTemp;
                    m_sRcvStr[iIdx] = m_sRcvStr[iIdx].Substring(0, iLen-2);
                }
            }
        
            sTemp = m_sRcvStr[iIdx];
        
            //Clear.
            m_sRcvStr[iIdx] = "";
        
            return sTemp;
        }
        //------------------------------------------------------------------------
        public void SetParaCmd(EN_RFID_ID Id, EN_RFID_CMD Cmd) 
        { 
            if ((Id < 0) || (Id >= EN_RFID_ID.EndOfId)) return; 
            
            m_TmpPara.Id  = (int)Id; 
            m_TmpPara.Cmd = Cmd; 

            m_ParaQue[m_TmpPara.Id].Enqueue(m_TmpPara); 
        }
        //------------------------------------------------------------------------
        public void ChkCmdSet(EN_RFID_ID Id, EN_RFID_CMD Cmd)
        {
            m_TmpPara.Id = (int)Id;
            if      (Cmd == EN_RFID_CMD.rfcWrite1) { m_TmpPara.Cmd = EN_RFID_CMD.rfcRead1; m_iSetCmd[m_TmpPara.Id] = EN_RFID_CMD.rfcRead1; }
            else if (Cmd == EN_RFID_CMD.rfcWrite2) { m_TmpPara.Cmd = EN_RFID_CMD.rfcRead2; m_iSetCmd[m_TmpPara.Id] = EN_RFID_CMD.rfcRead2; }
            else                                   { m_TmpPara.Cmd = EN_RFID_CMD.rfcNone ; m_iSetCmd[m_TmpPara.Id] = EN_RFID_CMD.rfcNone ; }
            m_ParaQue[m_TmpPara.Id].Enqueue(m_TmpPara);
         }
        //---------------------------------------------------------------------------
        public bool ChkVerifyRead(EN_RFID_ID Id)
        {       
            int iId = (int)Id;
            int iTrayIdLen = RTAG[iId].TrayId.Length;
            //int iTempId   ;
            
            if(RTAG[iId].TrayId.Trim() == "") return false;
            
            //if(iTrayIdLen != 8              ) return false;
       
            //for(int i=0;i<iTrayIdLen;i++)
            //{
            //    iTempId = Convert.ToInt32(RTAG[iId].TrayId.Substring(i + 1, 1));
            //    //int.TryParse(RTAG[iId].TrayId.Substring(i + 1, 1), out iTempId);
            //    if(iTempId<0) return false;
            //}

            //Ok
            return true;
        }
        
        //---------------------------------------------------------------------------
        public bool ChkVerifyWrite(EN_RFID_ID Id)
        {
            int iId = (int)Id;
            //
            if (WTAG[iId].LotId   .Trim() != RTAG[iId].LotId   .Trim()) return false;
            if (WTAG[iId].SecLotId.Trim() != RTAG[iId].SecLotId.Trim()) return false;
            if (WTAG[iId].EqpNo   .Trim() != RTAG[iId].EqpNo   .Trim()) return false;
        
            //Ok
            return true;
        }
        //---------------------------------------------------------------------------
        public bool IsCommDuring(EN_RFID_ID Id = EN_RFID_ID.riCh1) {  return m_bDuringTag[(int)Id] || m_iSetCmd[(int)Id] != EN_RFID_CMD.rfcNone; }
        //---------------------------------------------------------------------------
        public bool WriteACK(EN_RFID_ID Id , EN_RFID_CMD Cmd)
        {
            //Local Var.
            int     iId  = (int)Id;
            int     iCmd = (int)Cmd;
            char[]  cAck = new char[512];
            bool    bVerS = GetVer(Id) == "CAP1.3S";
            //string  sTemp;
        
            //Null Check.(Rcv Data Check.)
            if (m_sRcvStr[iId] == "") return false;
        
            //Shift
            cAck = m_sRcvStr[iId].ToCharArray();
            //PGM Down Check.(뻑나는거 체크)
            for (int n = 0; n < ACK_LENGTH; n++) {
                if (cAck[n] == 0x00) {
                    if (n != (ACK_LENGTH - 1)) return false;
                    }
                }
        
            //check
            if (bVerS) {
                if (cAck[0] == vDEF.chNAK                      ) { m_bErrWrite[iId] = true; return false; }
                if (cAck[0] != vDEF.chACK                      ) return false;
                if (cAck[1] != (char)RfidCmdLists[iCmd].iMNum  ) return false;
                if (cAck[2] != (char)RfidCmdLists[iCmd].iCmd   ) return false;
                if (cAck[3] != vDEF.chETX                      ) return false;
                }
            else {
                if (cAck[0] == vDEF.chNAK                      ) { m_bErrWrite[iId] = true; return false; }
                if (cAck[0] != vDEF.chACK                      ) return false;
                if (cAck[1] !=      RfidCmdLists[iCmd].cMNum[0]) return false;
                if (cAck[2] !=      RfidCmdLists[iCmd].cMNum[1]) return false;
                if (cAck[3] !=      RfidCmdLists[iCmd].cCmd [0]) return false;
                if (cAck[4] !=      RfidCmdLists[iCmd].cCmd [1]) return false;
                if (cAck[5] != vDEF.chETX                      ) return false;
                }
        
            //Clear.
            m_sRcvStr[iId] = "";
        
            return true;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Update.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool CanSendMsg(EN_RFID_ID Id)
        {
            int iIdx = (int)Id;
            //
            if ( RS232   [iIdx] == null               ) return false;
            if (!RS232   [iIdx]._IsOpen               ) return false;
            if (m_iSetCmd[iIdx] != EN_RFID_CMD.rfcNone) return false;
            if (m_iSetCmd[iIdx] != EN_RFID_CMD.rfcNone) return false;
            if (m_ParaQue[iIdx].Count <= 0            ) return false;
            //
            return true;
        }
        //------------------------------------------------------------------------
        public void   UpdateMsg        (   )
        {
            for (int n = 0; n < (int)EN_RFID_ID.EndOfId; n++)
            {
                if (m_MsgTimer[n].OnDelay(m_iMsgStep[n] != 0, 10000))
                {
                    if (RS232[n] != null) { RS232[n].DiscardInBuffer(); RS232[n].DiscardOutBuffer(); }
                    if (TCPIP[n] != null) TCPIP[n].ClearErr       ();
                    m_ParaQue[n].Clear();
                    m_iMsgStep[n] = 0;                    
                }
                //
                switch (m_iMsgStep[n])
                {
                    case 0 : if ( m_ParaQue[n].Count <= 0) { m_iMsgStep[n] = 0; break; }
                             //
                             m_TxParas[n] = (TRFID_PARA_BUFF)m_ParaQue[n].Dequeue();
                             //
                             Array.Clear(m_szTxBuff[n], 0, m_szTxBuff[n].Length);
                             Array.Clear(m_szRxBuff[n], 0, m_szRxBuff[n].Length);
                             m_iMsgStep[n]++;
                             return;
                    
                    case 1 : 
                             if (!SndMsg((EN_RFID_ID)n)) return;
                             m_iMsgStep[n]++;
                             return;

                    case 2 : if (!RcvMsg((EN_RFID_ID)n)) return;
                             m_TxParas [n].Init();
                             m_MsgTimer[n].Clear();
                             m_iMsgStep[n] = 0; 
                             return;
                }
            }
        }
        //------------------------------------------------------------------------
        public void GoMain(EN_RFID_ID Id)
        {
            int iId = (int)Id;
            //
            TOutRFIDstep[iId].OnDelay(m_iRFIDstep[iId] != 0, 15000);
            if (TOutRFIDstep[iId].Out)
            {
                m_iRFIDstep [iId] = 0;
                m_bDuringTag[iId] = false;
                TOutRFIDstep[iId].Clear();
            }
            //
            if (m_iRFIDstep[iId] == 0)
            {
                if (m_ParaQue[iId].Count >= 512) return;
                m_bDuringTag[iId] = false;
                
                if     ((m_iSetCmd[iId] == EN_RFID_CMD.rfcRead1 ) || (m_iSetCmd[iId] == EN_RFID_CMD.rfcRead2 )) m_iRFIDstep [iId] = 10; 
                else if((m_iSetCmd[iId] == EN_RFID_CMD.rfcWrite1) || (m_iSetCmd[iId] == EN_RFID_CMD.rfcWrite1)) m_iRFIDstep [iId] = 30;
                return;
            }
            //
            switch (m_iRFIDstep[iId])
            {
                default : m_iRFIDstep[iId] = 0; return;
                
                    //Read
                case 10 : 
                    m_bDuringTag[iId] = true;
                    m_bErrRead  [iId] = false;
                    ClearReadBuf(Id);
                    //
                    SetParaCmd(Id, m_iSetCmd[iId]);
                    m_iRFIDstep[iId]++;
                    return;
                
                case 11 : 
                    if (!UpdateReadData(Id, ADDR_TRAYID, (int)EN_BYTE_COUNT.ByteAll, true)) return;
                    m_iRFIDstep[iId]++;
                    return;
                
                case 12 : 
                    if (m_bNeedRcv[iId])
                    {
                        if (!ChkVerifyWrite(Id)) { m_bErrWrite[iId] = true; m_bErrRead[iId] = true; }
                    }
                    else
                    {
                        if (!ChkVerifyRead(Id)) m_bErrRead[iId] = true;
                    }
                    m_bDuringTag[iId] = false;
                    m_bNeedRcv  [iId] = false;
                    m_iSetCmd   [iId] = EN_RFID_CMD.rfcNone;
                    m_iRFIDstep [iId] = 0;
                    return;
                
                //Write
                case 30 : 
                    m_bDuringTag[iId] = true;
                    m_bErrWrite[iId] = false;
                    ClearWriteBuf(Id);
                    //
                    SetParaCmd(Id, m_iSetCmd[iId]);
                    m_iRFIDstep[iId]++;
                    return;
                
                case 31 : 
                    if ( m_bErrWrite[iId]) { Reset(Id); return; }
                    if (!WriteACK(Id, m_iSetCmd[iId])) return;
                    m_tWait.Clear();
                    m_iRFIDstep[iId]++;
                    return;
                
                case 32 : 
                    if (!m_tWait.OnDelay(true, 500)) return;
                    ChkCmdSet(Id, m_iSetCmd[iId]);
                    m_iRFIDstep[iId]++;
                    return;

                case 33 : 
                    if (!UpdateReadData(Id, ADDR_TRAYID, (int)EN_BYTE_COUNT.ByteAll, true)) return;
                    m_iRFIDstep[iId]++;
                    return;    
                
                case 34 : 
                    if (!ChkVerifyWrite(Id)) {  m_bErrWrite[iId] = true; m_bErrRead[iId] = true; }
                    m_bDuringTag[iId] = false;
                    m_iSetCmd   [iId] = EN_RFID_CMD.rfcNone;
                    m_iRFIDstep [iId] = 0;
                    return;                         
            }
        }
        //------------------------------------------------------------------------
        public void Update()
        {
            UpdateMsg();
            for (int n = 0; n < (int)EN_RFID_ID.EndOfId; n++) GoMain((EN_RFID_ID)n);
        }
        //------------------------------------------------------------------------
		public void  WriteLog (string str, EN_RFID_ID Id, bool send)
        {
            //Local Var.
            string sSend = send ? "[SEND]" : "[RECV]"; 
            string sTemp = string.Format($"{sSend} CH[{Id}]-{str}");

            cDEF.LOG.RFIDTrace(sTemp);
        }
        //------------------------------------------------------------------------
        public string GetRFTagData(EN_RFID_ID id)
        {
            return m_sRcvStr[(int)id];
        }
        //------------------------------------------------------------------------
        public string GetRFTagData(int id)
        {
            return GetRFTagData((EN_RFID_ID)id);
        }
    }
}
