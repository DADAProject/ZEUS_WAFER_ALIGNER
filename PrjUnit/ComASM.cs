using ElmoMotionControl.GMAS.EASComponents.MMCLibDotNET;
using Emgu.CV.Aruco;
using Emgu.CV.Dnn;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using static eMachine.cDEF;


namespace eMachine
{
    
    /***************************************************************************/
    /* Class: TCOMASM                                                          */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TCOMASM
    {
        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int    TX_BUFF    = 1024;
        const int    RX_BUFF    = 1024;
        const string START_SIGN = "@";
        const string END_SYMBOL = ";";
        const string ACKOK      = "0";
        const string ACKNG      = "1";
        const int    CMD_LENGTH  = 3 ; //@제외
        const string STR_FOUP_EMPTY  = "**************************"; //16자리
        const int    MAX_FOUPID_LEN  = 16;
        const int    MAX_PANELID_LEN = 16;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        bool[] SEND_MSG = new bool[(int)EN_SEND_LIST.EndOfList];
        bool[] RECV_MSG = new bool[(int)EN_RECV_LIST.EndOfList];

        List<string> lstCmdSend = new List<string>() ;
        List<string> lstCmdRcv  = new List<string>() ;

        //
        //Queue<ST_TCPIP_CMD> m_CmdList = new Queue<ST_TCPIP_CMD>();
        //ST_TCPIP_CMD        m_TxBuff  = new ST_TCPIP_CMD() ;
        Queue<string>  m_CmdList = new Queue<string>();
        string         m_TxBuff  ;

        byte[] m_szTxBuff = new byte[TX_BUFF];

        string m_sRcvMsg  ;
        bool   m_bDrngComm; //Process Value.
        bool   m_bErrComm ; //Communication - 통신 에러
        bool   m_bWatchOn ; //Controller의 상태를 모니터링 할 것인지를 결정.
        bool   m_bConnect ; //Communication Connect Flag
        bool   m_bRetry   ; //Communication Retry Connect
        int    m_iSendStep; //Step - Read Cycle.

        //protected: //Inheritable Vars.        
        static string m_sHostAddress = "192.168.100.100"; //
        static int    m_iPort        = 9004; //

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_tSendTimer = new TOnDelayTimer();
        TOnDelayTimer m_tSendDelay = new TOnDelayTimer();

        //
        ST_R001_ONLINE_REQUEST                      stR001 = new ST_R001_ONLINE_REQUEST                     (false);
        ST_R002_Port_Status_Request                 stR002 = new ST_R002_Port_Status_Request                (false);
        ST_R003_FOUP_Status_Request                 stR003 = new ST_R003_FOUP_Status_Request                (false);
        ST_R004_Robot_Operation_Request             stR004 = new ST_R004_Robot_Operation_Request            (false);
        ST_R005_Port_Mode_Request                   stR005 = new ST_R005_Port_Mode_Request                  (false);
        ST_R006_Port_Operation_Request              stR006 = new ST_R006_Port_Operation_Request             (false);
        ST_R008_Port_E84_Status_Report              stR008 = new ST_R008_Port_E84_Status_Report             (false);
        ST_R031_EFEM_Status_Request                 stR031 = new ST_R031_EFEM_Status_Request                (false);
        ST_R032_EFEM_Transportation_Request         stR032 = new ST_R032_EFEM_Transportation_Request        (false);
        ST_R132_EFEM_Transportation_Request         stR132 = new ST_R132_EFEM_Transportation_Request        (false);
        ST_R033_FOUP_ID_Verify_Result               stR033 = new ST_R033_FOUP_ID_Verify_Result              (false);
        ST_R034_Port_Slot_Map_Verify_Result         stR034 = new ST_R034_Port_Slot_Map_Verify_Result        (false);
        ST_R035_Panel_ID_Verify_Result              stR035 = new ST_R035_Panel_ID_Verify_Result             (false);
        ST_R036_Panel_Process_status_Reply          stR036 = new ST_R036_Panel_Process_status_Reply         (false);
        ST_R037_Reset_Alarm                         stR037 = new ST_R037_Reset_Alarm                        (false);
                                                                                                            
        //For Response                                                                                     
        ST_R600_FOUP_ID_Read_Result_Response        stR600 = new ST_R600_FOUP_ID_Read_Result_Response       (false);
        ST_R601_FOUP_ID_write_Result_Response       stR601 = new ST_R601_FOUP_ID_write_Result_Response      (false);
        ST_R602_Port_Status_Response                stR602 = new ST_R602_Port_Status_Response               (false);
        ST_R604_Port_Slot_Map_Response              stR604 = new ST_R604_Port_Slot_Map_Response             (false);
        ST_R605_Robot_Status_Response               stR605 = new ST_R605_Robot_Status_Response              (false);
        ST_R611_Panel_ID_Reading_Status_Response    stR611 = new ST_R611_Panel_ID_Reading_Status_Response   (false);
        ST_R612_Panel_CCD_Alignment_Status_Response stR612 = new ST_R612_Panel_CCD_Alignment_Status_Response(false);
        ST_R631_EFEM_Status_Response                stR631 = new ST_R631_EFEM_Status_Response               (false);
        ST_R632_Panel_Transportation_Response       stR632 = new ST_R632_Panel_Transportation_Response      (false);
        ST_R690_Alarm_Event_Response                stR690 = new ST_R690_Alarm_Event_Response               (false);


        //
        private Dictionary<string, ST_SENDED_DADT> m_dcSendList = new Dictionary<string, ST_SENDED_DADT>();

        //
        private bool[]  m_bRcvCmdList   = new bool[(int)EN_RECV_LIST.EndOfList];
        private int     m_nReqRobotOper = new int ();
        private int []  m_nReqPortOper  = new int [(int)EN_PORT_ID.EndOfList]; //R006-Port Operation Request
        private int     m_nReqTransport = new int ();
        private int []  m_nReqPortMode  = new int [(int)EN_PORT_ID.EndOfList];

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */


        //Objects.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        AsyncSocketClient Socket;



        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool _bErr      { get { return m_bErrComm  ; } }
        public bool _bDrngComm { get { return m_bDrngComm ; } }
        public bool _bWatchOn  { get { return m_bWatchOn; } set { m_bWatchOn = value; } }

        public bool _IsConnect 
        {
            get { return m_bConnect; }
        }

        public bool _IsRetry
        {
            get { return m_bRetry; }
        }

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TCOMASM()
        {
            //
            Socket = new AsyncSocketClient(0);
            Socket.OnReceive += DataReceivedHandler;
            Socket.OnConnet  += ConnectHandler     ;
            Socket.OnClose   += DisconnectHandler  ;

            m_iSendStep = 0;

            Init();

            //
            m_dcSendList.Clear();

        }
        ~TCOMASM() { }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
            //Init.
            for (EN_SEND_LIST n = 0; n < EN_SEND_LIST.EndOfList; n++ )
            {
                lstCmdSend.Add(n.ToString().Substring(0,4));
            }
            
            for (EN_RECV_LIST n = 0; n < EN_RECV_LIST.EndOfList; n++)
            {
                lstCmdRcv.Add(n.ToString().Substring(0, 4));
            }


            for (int n = 0; n < (int)EN_PORT_ID.EndOfList; n++)
            {
                ClearReqPortOper (n);
                ClearReqPortMode (n);
            }

            ClearReqTransport();
            ClearReqRobotOper();

        }
        //------------------------------------------------------------------------
        public bool IsRcvMsg(EN_RECV_LIST rcv)
        {
            return RECV_MSG[(int)rcv];
        }
        //------------------------------------------------------------------------
        public bool SetSndMsg(EN_SEND_LIST snd)
        {
            SEND_MSG[(int)snd] = true ;
            
            return SEND_MSG[(int)snd];
        }
        //--------------------------------------------------------------------------       
        public bool Connect()
        {
            try
            {
                if (Socket.Connection != null && Socket.Connection.Connected == true) return true;

                m_sHostAddress = "127.0.0.1"; //For TEST
                Socket.Connect(m_sHostAddress, m_iPort);

                //Clear Queue.
                m_CmdList.Clear();

                //Var.
                m_bWatchOn = true;

                return true;
            }
            catch
            {
                MsgBox.Error(m_sHostAddress + " >> Client Connect Error");
                return false;
            }
        }
        //------------------------------------------------------------------------
        public bool DisConnect()
        {
            try
            {
                Socket.Close();

                return true;
            }
            catch
            {
                MsgBox.Error(m_sHostAddress + " >> Client DisConnect Error");
                return false;
            }
        }
        //--------------------------------------------------------------------------
        public void Reset()
        {
            m_CmdList   .Clear();
            m_tSendTimer.Clear();
            m_dcSendList.Clear();
            m_iSendStep = 0;

            m_bDrngComm = false;

            if (!m_bConnect) Connect();
        }
        //---------------------------------------------------------------------------

        int AttatchData(string sData)
        {
            //Local Var.
            int iLast     = 0;
            byte byteNull = 0xff;
            iLast         = Array.IndexOf(m_szTxBuff, byteNull);

            byte[] Data = ASCIIEncoding.ASCII.GetBytes(sData);
            int Cnt = Data.Length;

            //Check Max.
            if ((iLast + Cnt) >= 128)
            {
                Array.Clear(m_szTxBuff, 0, TX_BUFF);
                return 0;
            }

            //Attach.
            Array.Copy(Data, 0, m_szTxBuff, iLast, Cnt);

            //Ok.
            return (iLast + Cnt);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Interface.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool SndMsg(ST_TCPIP_CMD TxBuff)
        {
            //Local Var.
            int iTxLen = 0;

                     AttatchData(START_SIGN);

                     AttatchData(TxBuff.Id );
                     AttatchData(TxBuff.Msg);

            iTxLen = AttatchData(END_SYMBOL);

            //Check Port.
            if (!_IsConnect) return false;
            if (iTxLen <= 0) return false;

            m_bDrngComm = true;

            //Write Data./
            bool bRet = Socket.Send(m_szTxBuff, iTxLen);

            //Return.
            return bRet;
        }
        //------------------------------------------------------------------------
        private bool SndMsg(string TxBuff)
        {
            //Local Var.
            int iTxLen = 0;

                     AttatchData(START_SIGN);
                     AttatchData(TxBuff    );
            iTxLen = AttatchData(END_SYMBOL);

            //Check Port.
            if (!_IsConnect) return false;
            if (iTxLen <= 0) return false;

            m_bDrngComm = true;

            //Write Data.
            bool bRet = Socket.Send(m_szTxBuff, iTxLen);

            //
            AddSendQue(TxBuff);

            LOG.TCPIPTrace("[SND] " + START_SIGN + TxBuff + END_SYMBOL);

            //Return.
            return bRet;
        }
        //------------------------------------------------------------------------
        public void DataReceivedHandler(object sender, AsyncSocketReceiveEventArgs e)
        {
            //Check.
            m_bDrngComm = false;
            m_bErrComm  = false;

            int iLength    = e.ReceiveBytes;
            byte[] bRecive = new byte[iLength];
            Array.Copy(e.ReceiveData, 0, bRecive, 0, iLength);

            m_sRcvMsg = Encoding.GetEncoding("Shift_JIS").GetString(bRecive).Trim();
            
            //
            LOG.TCPIPTrace("[RCV] " + m_sRcvMsg);
            
            //
            FuncReceive(m_sRcvMsg);
        }
        //------------------------------------------------------------------------
        public void ConnectHandler(object sender, AsyncSocketConnectionEventArgs e)
        {
            m_bConnect = true;
            
            LOG.TCPIPTrace(">>> Connected...");
        }
        //------------------------------------------------------------------------
        public void DisconnectHandler(object sender, AsyncSocketConnectionEventArgs e)
        {
            m_bConnect = false;
            LOG.TCPIPTrace("<<< Disconnected...");
            if (m_bRetry) this.Connect();
        }
        //------------------------------------------------------------------------


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Check Comm. Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool CanSendMsg()
        {
            if (Socket == null      ) return false;
            if (!_IsConnect         ) return false;
            if (m_CmdList.Count != 0) return false;
            if (m_bDrngComm         ) return false;
            if (m_iSendStep != 0    ) return false;

            return true;
        }
        //Update Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        private void UpdateMsg()
        {
            //Local Var.
            if (FM.IsDryMode()) return; 
            
            //Update.
            if (m_tSendTimer.OnDelay((m_iSendStep != 0 || m_bDrngComm), 15 * 1000))
            {
                Reset();
                m_bErrComm = true;
                cDEF.LOG.TCPIPTrace("Comm Time Out");
            }

            try
            {
                //
                CheckRcvData();

                //??? 에러일때 어떻게 ???
                if(EPU._bHasErr)
                {

                }

                //Message Process..
                switch (m_iSendStep)
                {
                    case 0:
                        if (m_CmdList.Count == 0) { m_iSendStep = 0; break; }
                        
                        m_szTxBuff.MemSet(0xFF);
                        m_iSendStep++;
                        return;

                    case 1:
                        if (m_CmdList.Count < 1) break;
                        
                        //
                        m_TxBuff = m_CmdList.Dequeue();
                        
                        m_tSendDelay.Clear();
                        m_iSendStep++;
                        return;

                    case 2:
                        if (!SndMsg((m_TxBuff))) return;

                        m_szTxBuff.MemSet(0xFF);

                        m_tSendDelay.Clear();
                        m_iSendStep++;
                        return;

                    case 3:
                        if (!m_tSendDelay.OnDelay(true, 100)) return;
                        if (m_bDrngComm                     ) return;
                        
                        m_tSendTimer.Clear(); //Clear Timer.
                        m_bErrComm  = false;
                        m_iSendStep = 0;
                        return;
                }
            }
            catch (Exception ex)
            {
                m_bErrComm = true;
                cDEF.LOG.ExceptionTrace("[ComASM] Update " + ex.ToString());
            }
        }
        //------------------------------------------------------------------------
        public void Update()
        {
            if (m_bWatchOn && CanSendMsg())
            {

            }
            //
            UpdateMsg();
        }
        //------------------------------------------------------------------------
        private void MsgEnqueue(string msg)
        {
            //JUNG/220905
            if (FM.IsDryMode()) return;

            //string sID = msg.Substring(1, 3);

            if (m_CmdList   .Contains   (msg)) return; //중복 방지
          //if (m_dcSendList.ContainsKey(sID)) return; 

            m_CmdList.Enqueue(msg);

            //
            //LOG.TCPIPTrace("[SND] " + msg);
        }
        //------------------------------------------------------------------------
        private void FuncReceive(string rcv)
        {//Receive data 처리
            
            //check Message
            if (rcv.Substring(1,1)             != "R") return;
            if (rcv.Substring(rcv.Length-1, 1) != ";") return;

            //var
            string sRcvID  = rcv.Substring(2, 3); //ID
            string sRcvMsg = rcv.Substring(5   ); //Msg body

            rcv = rcv.Substring(1, rcv.Length-2); //@, ;삭제

            //Check Length
            if (!CheckRcvData(sRcvID, rcv))
            {
                cDEF.LOG.TCPIPTrace($"Receive Data Error - {sRcvID}");
                //EPU.SetErr(EN_ERR_LIST.ERR_0720, true); //??? 사용
                return;
            }

            //
            RemoveSendQue(sRcvID);

            switch (sRcvID)
            {
                //Request Cmd (Equipment → EFEM)
                case "001"://R001_Online_Request
                    stR001 = FNC.StrToStruct<ST_R001_ONLINE_REQUEST              >(rcv);
                    CmdC001OnlineRequest();
                    break;
                                                                             
                case "002"://R002_Port_Status_Request                        
                    stR002 = FNC.StrToStruct<ST_R002_Port_Status_Request         >(rcv);
                    CmdC002ReplyPortStatus();
                    break;
                                                                             
                case "003"://R003_FOUP_Status_Request                        
                    stR003 = FNC.StrToStruct<ST_R003_FOUP_Status_Request         >(rcv);
                    CmdC003ReplyFOUPStatus();
                    break;
                                                                             
                case "004"://R004_Robot_Operation_Request                    
                    stR004 = FNC.StrToStruct<ST_R004_Robot_Operation_Request     >(rcv);
                    CmdC004RobotOperation();
                    break;
                                                                             
                case "005"://R005_Port_Mode_Request                          
                    stR005 = FNC.StrToStruct<ST_R005_Port_Mode_Request           >(rcv);
                    CmdC005PortMode();
                    break;

                case "006"://R006_Port_Operation_Request
                    stR006 = FNC.StrToStruct<ST_R006_Port_Operation_Request      >(rcv);
                    CmdC006PortOperation();
                    break;
                //case "007"://R007_Track_InfoChange_Request
                //    break;

                case "008"://R008_Port_E84_Status_Report
                    stR008 = FNC.StrToStruct<ST_R008_Port_E84_Status_Report      >(rcv);
                    CmdC008PortE84Status();
                    break;
                
                case "031"://R031_EFEM_Status_Request                            
                    stR031 = FNC.StrToStruct<ST_R031_EFEM_Status_Request         >(rcv);
                    CmdC031EFEMStatus();
                    break;
                
                case "032"://R032_EFEM_Transportation_Request                    
                    stR032 = FNC.StrToStruct<ST_R032_EFEM_Transportation_Request >(rcv);
                    CmdC032EFEMTransportation();
                    break;

                case "132":
                    stR132 = FNC.StrToStruct<ST_R132_EFEM_Transportation_Request >(rcv);
                    CmdC132EFEMTransportation();
                    break;
                
                case "033"://R033_FOUP_ID_Verify_result                          
                    stR033 = FNC.StrToStruct<ST_R033_FOUP_ID_Verify_Result       >(rcv);
                    CmdC033FOUPIDVerify();                                       
                    break;

                case "034"://R034_Port_Slot_Map_Verify_result                    
                    stR034 = FNC.StrToStruct<ST_R034_Port_Slot_Map_Verify_Result >(rcv);
                    CmdC034PortSlotMapVerify();                                  
                    break;
                
                case "035"://R035_Panel_ID_Verify_result                         
                    stR035 = FNC.StrToStruct<ST_R035_Panel_ID_Verify_Result      >(rcv);
                    CmdC035PanelIDVerify();                                      
                    break;

                case "036"://R036_Panel_Process_status_Reply                     
                    stR036 = FNC.StrToStruct<ST_R036_Panel_Process_status_Reply  >(rcv);
                    CmdC036PanelProcessStatus();                                 
                    break;

                case "037"://R037_Reset_Alarm_Request_EQ_EFEM                    
                    stR037 = FNC.StrToStruct<ST_R037_Reset_Alarm                 >(rcv);
                    CmdC037RestAlarmRequest();
                    break;

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //Reply Cmd(EFEM → Equipment)
                case "600":
                    stR600 = FNC.StrToStruct<ST_R600_FOUP_ID_Read_Result_Response       >(rcv);
                    if(stR600.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0302, true);
                    return;
                case "601":
                    stR601 = FNC.StrToStruct<ST_R601_FOUP_ID_write_Result_Response      >(rcv);
                    if(stR601.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0303, true);
                    return;
                case "602":
                    stR602 = FNC.StrToStruct<ST_R602_Port_Status_Response               >(rcv);
                    if (stR602.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0304, true);
                    return;
                case "604":
                    stR604 = FNC.StrToStruct<ST_R604_Port_Slot_Map_Response             >(rcv);
                    if (stR604.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0305, true);
                    return;
                case "605":
                    stR605 = FNC.StrToStruct<ST_R605_Robot_Status_Response              >(rcv);
                    if (stR605.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0306, true);
                    return;
                case "611":
                    stR611 = FNC.StrToStruct<ST_R611_Panel_ID_Reading_Status_Response   >(rcv);
                    if (stR611.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0307, true);
                    return;
                case "612":
                    stR612 = FNC.StrToStruct<ST_R612_Panel_CCD_Alignment_Status_Response>(rcv);
                    if (stR612.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0308, true);
                    return;
                case "631":
                    stR631 = FNC.StrToStruct<ST_R631_EFEM_Status_Response               >(rcv);
                    if (stR631.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0309, true);
                    return;
                case "632":
                    stR632 = FNC.StrToStruct<ST_R632_Panel_Transportation_Response      >(rcv);
                    if (stR632.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0310, true);
                    return;
                case "690":
                    stR690 = FNC.StrToStruct<ST_R690_Alarm_Event_Response               >(rcv);
                    if (stR690.ACK != ACKOK) EPU.SetErr(EN_ERR_LIST.ERR_0311, true);
                    return;

                default:
                    return;
            }
        }
        //------------------------------------------------------------------------
        private bool CheckRcvData(string id, string msg)
        {
            bool ret       = true;
            int nMsgLength = msg.Length - CMD_LENGTH;

            //bool bCheck    = true;
            //if (!bCheck) return true; 

            switch (id)
            {
                //Request Cmd (Equipment → EFEM)
                case "001"://R001_Online_Request
                    if (nMsgLength != 3) ret = false;
                    break;

                case "002"://R002_Port_Status_Request                        
                    if (nMsgLength != 4) ret = false;
                    break;

                case "003"://R003_FOUP_Status_Request
                    if (nMsgLength != 4) ret = false;
                    break;

                case "004"://R004_Robot_Operation_Request
                    if (nMsgLength != 1 ) ret = false;
                    break;
                
                case "005"://R005_Port_Mode_Request                          
                    if (nMsgLength != 3) ret = false;
                    break;

                case "006"://R006_Port_Operation_Request
                    if (nMsgLength != 5) ret = false;
                    break;
                //case "007"://R007_Track_InfoChange_Request
                //    break;

                case "008"://R008_Port_E84_Status_Report
                    if (nMsgLength != 2) ret = false;
                    break;

                case "031"://R031_EFEM_Status_Request                            
                    if (nMsgLength != 2) ret = false;
                    break;

                case "032"://R032_EFEM_Transportation_Request                    
                    if (nMsgLength != 5 + MAX_PANELID_LEN) ret = false;
                    break;

                case "132":
                    if (nMsgLength != 12) ret = false;
                    break;

                case "033"://R033_FOUP_ID_Verify_result                          
                    if (nMsgLength != 5 + MAX_FOUPID_LEN) ret = false;
                    break;

                case "034"://R034_Port_Slot_Map_Verify_result                    
                    if (nMsgLength != 5 + MAX_FOUPID_LEN) ret = false;
                    break;

                case "035"://R035_Panel_ID_Verify_result                         
                    if (nMsgLength != 5) ret = false;
                    break;

                case "036"://R036_Panel_Process_status_Reply                     
                    if (nMsgLength != 5) ret = false;
                    break;

                case "037"://R037_Reset_Alarm_Request_EQ_EFEM                    
                    if (nMsgLength != 1) ret = false;
                    break;

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //Reply Cmd(EFEM → Equipment)
                case "600":
                    if (nMsgLength != 3) ret = false; 
                    break;
                case "601":
                    if (nMsgLength != 3) ret = false;
                    break;
                case "602":
                    if (nMsgLength != 3) ret = false;
                    break;
                case "604":
                    if (nMsgLength != 3) ret = false;
                    break;
                case "605":
                    if (nMsgLength != 1) ret = false;
                    break;
                case "611":
                    if (nMsgLength != 1) ret = false;
                    break;
                case "612":
                    if (nMsgLength != 1) ret = false;
                    break;
                case "631":
                    if (nMsgLength != 1) ret = false;
                    break;
                case "632":
                    if (nMsgLength != 1) ret = false;
                    break;
                case "690":
                    if (nMsgLength != 1) ret = false;
                    break;

                default:
                    return false;
            }

            return ret; 
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Make send message.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int m_MakeMsgSetTrigger(bool On)
        {
            int iLen = 0;

            iLen = AttatchData(On ? "LON\r" : "LOFF\r");        //

            return iLen;
        }
        
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Cmd.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void CmdC001OnlineRequest()
        {
            /*Condition that <GRANT> is NG: EFEM uninitialized */
            /*
             <A [2] EQNo>
             <A [1] GRANT>
            */

            //var
            ST_C001_ONLINE_Reply stC001 = new ST_C001_ONLINE_Reply("");

            //EQNo           
            stC001.EQNo = stR001.EQNo;

            //GRANT
            if(stR001.COMMAND == "1")//Online
            {
                stC001.GRANT = string.Format("{0}", SEQ.IsAllHomeEnd() ? "0" : "1"); //GRANT
            }
            else //off-line
            {
                stC001.GRANT = "0";
            }

            //
            MsgEnqueue(stC001.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC001OnlineRequest(string eqno, string grant)
        {
            /*Condition that <GRANT> is NG: EFEM uninitialized */
            /*
             <A [2] EQNo>
             <A [1] GRANT>
            */

            ST_C001_ONLINE_Reply stC001 = new ST_C001_ONLINE_Reply("");

            //EQNo           
            stC001.EQNo  = eqno ;
            stC001.GRANT = grant;

            MsgEnqueue("[MAN]" + stC001.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC002ReplyPortStatus()
        {
            /*
             <A [2] EQNo>
             <A [2] PortNo>     = Target port (01-04)
             <A [1] PortStatus> = Port status 
                                  0: Port disabled 
                                  1: Load Request (LDRQ) 
                                  2: Load Complete (LDCM) 
                                  3: Unload Request (UDRQ) 
                                  4: Unload Complete (UDCM)
             */

            //
            ST_C002_Port_Status_Reply stC002 = new ST_C002_Port_Status_Reply("");
            stC002.EQNo   = stR002.EQNo  ;
            stC002.PORTNO = stR002.PortNo;
            int.TryParse(stR002.PortNo, out int nPort);
            switch (nPort)
            {
                case 1:
                    stC002.STATUS = SEQ.LPM1.GetPortStatus();
                    break; 
                case 2:
                    stC002.STATUS = SEQ.LPM2.GetPortStatus();
                    break; 
                default:
                    stC002.STATUS = ((int)EN_PORT_STATUS.Disable).ToString();
                    break;
            }

            //
            MsgEnqueue(stC002.ToString()); //
        }
        //------------------------------------------------------------------------
        public void CmdC002ReplyPortStatus(string eqno, string portno, string status)
        {
            /*
             <A [2] EQNo>
             <A [2] PortNo>     = Target port (01-04)
             <A [1] PortStatus> = Port status 0: Port disabled 1: Load Request (LDRQ) 2: Load Complete (LDCM) 3: Unload Request (UDRQ) 4: Unload Complete (UDCM)
             */

            //
            ST_C002_Port_Status_Reply stC002 = new ST_C002_Port_Status_Reply("");
            stC002.EQNo   = eqno  ;
            stC002.PORTNO = portno;
            stC002.STATUS = status;

            MsgEnqueue(stC002.ToString()); // 
        }
        //------------------------------------------------------------------------
        public void CmdC003ReplyFOUPStatus()
        {
            /*
            <A [2] EQNo>	        Same value as requested
            <A [2] PortNo>	        Same value as requested
            <A [112 112] FOUPID>	Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. 
                                    If there is no FOUP, fill it with "*".
            <A [1] FOUPStatus>	    FOUP status 
                                    0: No FOUP 
                                    1: Waiting for slot mapping 
                                    2: Ready for access 
                                    3: Accessing 
                                    4: Suspending access 
                                    5: Ending access
            */

            ST_C003_FOUP_Status_Reply stC003 = new ST_C003_FOUP_Status_Reply("");
            stC003.EQNo       = stR003.EQNo   ; 
            stC003.PortNo     = stR003.PortNo ;
            stC003.FOUPID     = STR_FOUP_EMPTY;
            stC003.FOUPStatus = "0"           ;

            switch (stR003.PortNo)
            {
                case "01":
                    stC003.FOUPID     = SEQ.LPM1.GetFOUPID(); //
                    stC003.FOUPStatus = SEQ.LPM1.GetFOUPStatus();
                    break;

                case "02":
                    stC003.FOUPID     = SEQ.LPM2.GetFOUPID(); //
                    stC003.FOUPStatus = SEQ.LPM2.GetFOUPStatus();
                    break;

                //case "03":
                //    sReplyMsg += "******"; //
                //    sReplyMsg += "0"; //
                //    break;
                //case "04":
                //    sReplyMsg += "******"; //
                //    sReplyMsg += "0"; //
                //    break;
                //
                default:
                    break;
            }


            //
            MsgEnqueue(stC003.ToString()); //
        }

        //------------------------------------------------------------------------
        public void CmdC003FOUPStatus(string eqno, string portno, string id, string status)
        {
            /*
            <A [2] EQNo>	        Same value as requested
            <A [2] PortNo>	        Same value as requested
            <A [112 112] FOUPID>	Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. If there is no FOUP, fill it with "*".
            <A [1] FOUPStatus>	    FOUP status 0: No FOUP 1: Waiting for slot mapping 2: Ready for access 3: Accessing 4: Suspending access 5: Ending access
            */
            
            ST_C003_FOUP_Status_Reply stC003 = new ST_C003_FOUP_Status_Reply("");
            stC003.EQNo       = eqno  ; 
            stC003.PortNo     = portno;
            stC003.FOUPID     = id    ;
            stC003.FOUPStatus = status; 

            //
            MsgEnqueue(stC003.ToString());


        }
        //------------------------------------------------------------------------
        public void CmdC004RobotOperation()
        {
            /*
            <<
            <A [1] RobotStatus>	0 = Start 
                                1 = Stop (cycle stop) 
                                2 = Pause 
                                3 = Resume 
                                4 = Abort
            
            >>
            <A [1] ACK>	Response Judge 0 = OK 1 = NG
            If NG, command result 3: Request failed (robot is being prepared)
            */

            //
            ST_C004_Robot_Operation_Reply stC004 = new ST_C004_Robot_Operation_Reply("");
            stC004.ACK = ACKOK;
            //

            
            switch (stR004.RBStatus)
            {
                case "0":
                    if (!SEQ.CheckStrtBtn())
                    {
                        stC004.ACK = ACKNG;
                        break;
                    }
                    
                    SEQ._bBtnManStart = true;
                    m_nReqRobotOper = (int)EN_ROBOT_OPER.Start;

                    break;

                case "1":
                    SEQ._bBtnManStop  = true;
                    m_nReqRobotOper   = (int)EN_ROBOT_OPER.Stop; 
                    break;

                case "2":
                    SEQ._bBtnManStop  = true;
                    m_nReqRobotOper   = (int)EN_ROBOT_OPER.Pause; 
                    break;

                case "3":
                    SEQ._bBtnManStart = true;
                    m_nReqRobotOper   = (int)EN_ROBOT_OPER.Resume; 
                    break;

                case "4":
                    SEQ._bBtnManStop  = true;
                    m_nReqRobotOper   = (int)EN_ROBOT_OPER.Abort; 
                    break;

                default:
                    stC004.ACK = ACKNG;
                    break;
            }

            //
            MsgEnqueue(stC004.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC004RobotOperation(string ack)
        {
            /*
            <A [1] RobotStatus>	0 = Start 1 = Stop (cycle stop) 2 = Pause 3 = Resume 4 = Abort

            
            <A [1] ACK>	Response Judge 0 = OK 1 = NG
            If NG, command result 3: Request failed (robot is being prepared)
            */

            //
            ST_C004_Robot_Operation_Reply stC004 = new ST_C004_Robot_Operation_Reply("");
            stC004.ACK = ack;

            //
            MsgEnqueue(stC004.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC005PortMode()
        {
            /*
            <A [2] PortNo>	Port No (01, 02…)
            <A [1] PortStatus>	0 = Manual 1 = Auto D = Disable

            <A [1] ACK>	Response Judge 0 = OK 1 = NG
            */

            //
            ST_C005_Port_Mode_Reply stC005 = new ST_C005_Port_Mode_Reply("");
            stC005.ACK = ACKOK;


            EN_PORT_MODE nStatus = EN_PORT_MODE.none ; 
            //
            switch (stR005.PortStatus)
            {
                case "0":
                    nStatus = EN_PORT_MODE.Manual;
                    break;

                case "1":
                    nStatus = EN_PORT_MODE.Auto;
                    break;

                case "D":
                    nStatus = EN_PORT_MODE.Disable;
                    break;
                default:
                    stC005.ACK = ACKNG;
                    break;
            }

            //
            int.TryParse(stR005.PortNo, out int nPort);
            if(nPort> 0)
            {
                m_nReqPortMode[nPort-1] = (int)nStatus;
                DM.MGZ[nPort-1].SetPortMode(nStatus);
                LOG.Trace($"[R005] Port Mode Request - Port:{nPort} / Status : {nStatus}");
            }
            else
            {
                stC005.ACK = ACKNG;
            }

            //
            MsgEnqueue(stC005.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC005PortMode(string ack)
        {
            /*
            <A [2] PortNo>	Port No (01, 02…)
            <A [1] PortStatus>	0 = Manual 1 = Auto D = Disable

            <A [1] ACK>	Response Judge 0 = OK 1 = NG
            
            */

            //
            //
            ST_C005_Port_Mode_Reply stC005 = new ST_C005_Port_Mode_Reply("");
            stC005.ACK = ack;

            //
            MsgEnqueue(stC005.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC006PortOperation()
        {
            /*
            >> Receive
            <A [2] EQNo>	        Communication parameter EQNo
            <A [2] PortNo>	        Target port (01-04)
            <A [1] Port Operation>	Port operation type 
                                    0 = Load 
                                    1 = Unload 
                                    2 = Mapping 
                                    3 = RFID Read 
                                    4 = RFID Write
             
            << Send
            <A [2] EQNo>	Same value as requested
            <A [2] PortNo>	Same value as requested
            <A [1] ACK>	    Acceptance success or failure 
                            0: OK 
                            1: NG (illegal command) 
                            2: NG (no FOUP) 
                            3: NG (port not reserved for device)

            */

            //
            ST_C006_Port_Operation_Reply stC006 = new ST_C006_Port_Operation_Reply("");
            stC006.ACK = ACKOK;


            //
            int.TryParse(stR006.PortNo  , out int nPort    );
            int.TryParse(stR006.PortOper, out int nPortOper);
            
            if (nPort> 0 && nPort < 5)
            {
                if(nPortOper >= 0 && nPortOper < 5)
                {
                    m_nReqPortOper[nPort - 1] = nPortOper;
                }
                else
                {
                    stC006.ACK = ACKNG;
                }

                DM.MGZ[nPort - 1].SetPortOper((EN_PORT_OPER)nPortOper);
                LOG.Trace($"[R006] Port Oper Request - {((EN_PORT_OPER)m_nReqPortOper[nPort - 1])}");
            }
            else
            {
                stC006.ACK = "3" ;
            }

            //
            MsgEnqueue(stC006.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC006PortOper(string port, string ack)
        {
            /*
            <A [2] PortNo>	Port No (01, 02…)
            <A [1] PortStatus>	0 = Manual 1 = Auto D = Disable

            <A [1] ACK>	Response Judge 0 = OK 1 = NG
            
            */

            //
            //
            ST_C006_Port_Operation_Reply stC006 = new ST_C006_Port_Operation_Reply("");
            stC006.PORTNO = port; 
            stC006.ACK    = ack;

            //
            MsgEnqueue(stC006.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC008PortE84Status() //??? IO???
        {
            /*
            <A [2] Port No.>	01 = Load Port 1 02 = Load Port 2 03 = Load Port 3 04 = Load Port 4

            
            <A [2] Port No.>	same with R008
            <A [1] GRANT>	    0 = OK 1 = NG
            <A [16] E84 Code>	A<1>~A<8> (INPUT)  RB_E84_VALID、 RB_E84_CS0  、 RB_E84_CS1     、 RB_E84_SPARE_03              、 RB_E84_TR_REQ    、 RB_E84_BUSY  、 RB_E84_COMPT、 RB_E84_CONT 
                                A<9>~A<16>(OUTPUT) WB_E84_L_REQ、 WB_E84_U_REQ、 WB_E84_SPARE_02、 WB_E84_READY、 WB_E84_SPARE_04、 WB_E84_SPARE_05 , WB_E84_HO_AVBL,  WB_E84_ES
            */

            //
            ST_C008_Port_E84_Status_Reply stC008 = new ST_C008_Port_E84_Status_Reply("");
            stC008.PortNo  = stR008.PortNo;
            stC008.GRANT   = ACKOK;
            stC008.E84Code = "1234567890123456"; //???

            //
            MsgEnqueue(stC008.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC008PortE84Status(string portno, string ack, string code) //
        {
            /*
            <A [2] Port No.>	01 = Load Port 1 02 = Load Port 2 03 = Load Port 3 04 = Load Port 4

            
            <A [2] Port No.>	same with R008
            <A [1] GRANT>	    0 = OK 1 = NG
            <A [16] E84 Code>	A<1>~A<8> (INPUT)   RB_E84_VALID、 RB_E84_CS0、 RB_E84_CS1、 RB_E84_SPARE_03、 RB_E84_TR_REQ、 RB_E84_BUSY、 RB_E84_COMPT、 RB_E84_CONT 
                                A<9>~A<16>(OUTPUT) WB_E84_L_REQ、 WB_E84_U_REQ、 WB_E84_SPARE_02、 WB_E84_READY、 WB_E84_SPARE_04、 WB_E84_SPARE_05 , WB_E84_HO_AVBL, WB_E84_ES
            */

            //
            ST_C008_Port_E84_Status_Reply stC008 = new ST_C008_Port_E84_Status_Reply("");
            stC008.PortNo  = portno;
            stC008.GRANT   = ack   ;
            stC008.E84Code = code  ; //???

            //
            MsgEnqueue(stC008.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC031EFEMStatus() //
        {//Panel Clear / Load / Unload / Exchange operation start request

            /*
            >>Rcv
            <A [2] EQNo>	Communication parameter EQNo

            <<Snd
            <A [2] EQNo>	      Same value as requested
            <A [1] ACK>	          Success or failure of acceptance (0: OK / 1: NG)
            <A [25] EFEM Status>  Shows the status of the following 16 items in order by 1 to 3 characters 
                            ・ [0 ]/[1] Operation Mode 0: Standalone / 1: Inline 
                            ・ [1 ]/[1] EFEM Status 1: Ready / 0: Preparing or error 
                            ・ [2 ]/[1] Robot Upper Arm Panel 
                            ・ [3 ]/[1] Robot Lower Arm Panel 0: No panel /n(1-4): Port n There is a panel in / D: Disabled 
                            ・ [4 ]/[1] EQ1 Online Status 
                            ・ [5 ]/[1] EQ2 Online Status 0: Offline / 1: Online 
                            ・ [6 ]/[1] Load Port 1 Transport Mode 
                            ・ [7 ]/[1] Load Port 2 Transport Mode 
                            ・ [8 ]/[1] Load Port 3 Transport Mode 
                            ・ [9 ]/[1] Load Port 4 Transport Mode 0: Auto / 1: Manual / D: Invalid or not installed 
                            ・ [10]/[2] T1 Time out value (2 characters: 01 to 30) 
                            ・ [11]/[3] T2 Time out value (3 characters: 060 to 240) 
                            ・ [12]/[2] D1 Time out value (2 characters: 20 to 60) 
                            ・ [13]/[2] D2 Time out value (2 characters: 20-60) 
                            ・ [14]/[2] D3 Time out value (2 characters: 03-10) 
                            ・ [15]/[4] Signal Tower Status (4 characters each for R, Y, G, B) 0: Off / 1: Lit / 2: Flashing

            
            <Transfer Mode>
            0 = Get 
            1 = Put 
            2 = Exchange (Upper arm get panel from EQ and Lower arm put panel into EQ, In <A [1] Upper / Lower ARM> only can choose ”0” (UpperArm)) 
            3 = CCD_Align_position (only) panel CCD Align.) 
            4 = Read_position (only read panel 2D Code) 
            5 = Remove_position (Special case use only)


             */
            //
            ST_C031_EFEM_Status_Reply stC031 = new ST_C031_EFEM_Status_Reply("");
            stC031.EQNo        = stR031.EQNo;
            stC031.ACK         = ACKOK;
            stC031.EFMEFStatus = GetEFEMStatus();

            //
            MsgEnqueue(stC031.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC032EFEMTransportation() //
        {//Panel Clear / Load / Unload / Exchange operation start request
            /*
            <<Rcv
            <A [2] EQNo>	        Communication parameter EQNo
            <A [2] STAGEID>	        Target STAGE ID
            <A [1] Transport Mode>	Transfer mode 
                                    0: Clear 
                                    1: Load 
                                    2: Unload 
                                    3: Exchange 
                                    4: Abort

            <A [16] PanelID>	    Valid only when the target panel ID 
                                    <Transport Mode> is 2 or 3. 
                                    Fill in the space that is not enough for the maximum number of characters. 
                                    If the device does not have a panel or the panel ID is unknown, fill it with "*".
                                    
            >> Send                 
            <A [2] EQNo>	        Same value as requested	
            <A [1] ACKC>	        Acceptance success / failure S: Success / E: Error	
            <A [4] Error Code>	    Error code (error details) 
                                    0000: No error 
                                    0001: Robot is being prepared 
                                    0002: The panel with the specified panel ID does not exist 
                                    0003: EFEM cannot operate 2 panels because it is 1 arm 
                                    0004: There is no loadable panel (only when a load is requested)
            */
            ST_C032_EFEM_Transportation_Reply stC032 = new ST_C032_EFEM_Transportation_Reply("");
            stC032.EQNo    = stR032.EQNo;
            stC032.ErrCode = "0000";


            //
            switch (stR032.TRMode)
            {
                case "0": //Clear

                    break;

                case "1": //Load
                    //Check Panel ID??

                    //Check StageID
                    //stR032.StageID; //To SeqTransfer

                    break;
                case "2": //Unload
                    //Check Panel ID??

                    //stR032.StageID; //To SeqTransfer

                    break;
                case "3": //Exchange
                    stC032.ErrCode = "0003";
                    break;
                case "4": //Abort
                    break;

                default:
                    break;
            }

            //
            int.TryParse(stC032.ErrCode, out int errcode);
            stC032.ACK  = errcode < 1? "S" : "E";

            //
            MsgEnqueue(stC032.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC032EFEMTransportation(string eqno, string ack, string errcode) //
        {//Panel Clear / Load / Unload / Exchange operation start request
            /*
            <<Rcv
            <A [2] EQNo>	        Communication parameter EQNo
            <A [2] STAGEID>	        Target STAGE ID
            <A [1] Transport Mode>	Transfer mode 0: Clear 1: Load 2: Unload 3: Exchange 4: Abort
            <A [16] PanelID>	    Valid only when the target panel ID 
                                    <Transport Mode> is 2 or 3. Fill in the space that is not enough for the maximum number of characters. 
                                    If the device does not have a panel or the panel ID is unknown, fill it with "*".

            >> Send
            <A [2] EQNo>	        Same value as requested	
            <A [1] ACKC>	        Acceptance success / failure S: Success / E: Error	
            <A [4] Error Code>	    Error code (error details) 
                                    0000: No error 
                                    0001: Robot is being prepared 
                                    0002: The panel with the specified panel ID does not exist 
                                    0003: EFEM cannot operate 2 panels because it is 1 arm 
                                    0004: There is no loadable panel (only when a load is requested)

            */


            ST_C032_EFEM_Transportation_Reply stC032 = new ST_C032_EFEM_Transportation_Reply("");
            stC032.EQNo    = eqno   ;
            stC032.ACK     = ack    ;
            stC032.ErrCode = errcode;

            //
            MsgEnqueue(stC032.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC132EFEMTransportation() //(Slave use only)
        {
            /*
             <<Rcv
             <A[2] EQNo                          > Communication parameter EQNo
             <A[1] Get from / Put into EQ / FOUP > 0 = FOUP 1 = EQ
             <A[2] STAGEID                       > Target STAGE ID 
                                                    If only one stage, < STAGEID > always ”00” If multi stage, then<STAGEID> is 01,02,… ..
             <A[2] Port / EQ No.                 > If < A[1] Get from EQ / FOUP > = 0(FOUP) --> 01 = Load Port 1 02 = Load Port 2 03 = Load Port 3    04 = Load Port 4 
                                                   If < A[1] Get from EQ / FOUP > = 1(EQ)   --> 01 = EQ 1        02 = EQ 2        03 = EQ 3(Reserved) 04 = EQ 4(Reserved)
             <A[2] Slot No.                      > Slot No. (01, 02, ……)
             <A[1] Upper / Lower ARM             > 0 = Upper Arm 1 = Lower Arm
             <A[1] Transport Mode                > 0 = Get 
                                                   1 = Put 
                                                   2 = Exchange(Upper arm get panel from EQ and Lower arm put panel into EQ, In < A[1] Upper / Lower ARM> only can choose ”0” (UpperArm)) 
                                                   3 = CCD_Align_position(only) panel CCD Align.
                                                   4 = Read_position(only read panel 2D Code) 
                                                   5 = Remove_position(Special case use only)
             
            <A[1] Option Parameter              > (Only for < A[1] Transport Mode > is Get) 
                                                   0 = Do nothing, or not Get mode 
                                                   1 = CCD Alignment 
                                                   2 = Read 2D code 
                                                   3 = CCD Alignment + Read 2D code

            >>Snd
            <A [2] EQNo>        Same value as requested
            <A [1] ACKC>        Acceptance success / failure S: Success / E: Error
            <A [4] Error Code>  Error code (error details) 
                                0000: No error 
                                0001: Robot is being prepared 
                                0002: The panel with the specified panel ID does not exist 
                                0003: EFEM cannot operate 2 panels because it is 1 arm 
                                0004: There is no loadable panel (only when a load is requested) )

            */
            

            bool bRtn = false; 

            //
            ST_TRANSFER_INFO info = new ST_TRANSFER_INFO(false);
            int.TryParse(stR132.EQNo   , out info.nEQNo    );
            int.TryParse(stR132.Where  , out info.nSource  ); //0 = FOUP 1 = EQ
            int.TryParse(stR132.StageID, out info.nStageId ); //
            int.TryParse(stR132.PortNo , out info.nTargetNo); //nSource == 0? LPM No / nSource == 1? EQ No
            int.TryParse(stR132.SlotNo , out info.nSlotNo  ); //info.nSlotNo -= 1;
            int.TryParse(stR132.ArmNo  , out info.nArmNo   );
            int.TryParse(stR132.TRMode , out info.nTRMode  );
            int.TryParse(stR132.Option , out info.nOption  );

            m_nReqTransport = info.nTRMode;

            //
            LOG.TCPIPTrace(info.GetLogData());

            ST_C132_EFEM_Transportation_Reply stC132 = new ST_C132_EFEM_Transportation_Reply("");
            stC132.EQNo = stR132.EQNo;

            if (info.CheckData())
            {
                switch (info.nTRMode) //EN_TRASPORT_MODE
                {
                    case 0: //Get
                        bRtn = SEQ.WTR.SetTransportGetInfo(info);
                        break;
                    case 1: //Put
                        bRtn = SEQ.WTR.SetTransportPutInfo(info);
                        break;
                    case 2: //Exchange
                        break;
                    case 3: //CCD_Align_position
                        bRtn = SEQ.WTR.SetTransportGetInfo(info);
                        break;
                    case 4: //Read_position(only read panel 2D Code) 
                        bRtn = SEQ.WTR.SetTransportGetInfo(info);
                        break;

                    case 5: //Remove_position(Special case use only) //???
                        break;

                    default:
                        break;
                }
                
                stC132.ACK     = bRtn ? "S"    : "E";
                stC132.ErrCode = bRtn ? "0000" : "0001"; //??? Error Code 확인 필요...
            }
            else
            {
                bRtn           = false;
                stC132.ACK     = "E";
                stC132.ErrCode = "0002"; //??? Error Code 확인 필요...
            }

            //
            if (!SEQ.IsAllHomeEnd())
            {
                stC132.ErrCode = "0001";
                stC132.ACK     = "E";
            }
            
            //
            MsgEnqueue(stC132.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC033FOUPIDVerify() //
        {
            /*
            <<
            <A [2] EQNo>	Communication parameter EQNo
            <A [2] PortNo>	Target port (01-04)
            <A [16] FOUPID>	Target FOUP ID
            <A [1] Result>	Judgment result (0: OK / 1: NG)

            >>
            <A [2] EQNo>	Same value as requested
            <A [2] PortNo>	Same value as requested
            <A [16] FOUPID>	Same value as requested
            <A [1] ACK>	    Acceptance success / failure 
                            0: OK
                            1: NG (EFEM uninitialized) 
                            2: NG (not waiting for FOUP ID judgment)
            */

            int.TryParse(stR033.PortNo, out int nPortNo);
            bool isAllHomeEnd  = MOTR.IsAllHomeEnd();
            bool isEmptyFoupId = true;
            if (nPortNo > 0)
            {
                isEmptyFoupId = DM.MGZ[nPortNo - 1]._sRFID == string.Empty || DM.MGZ[nPortNo - 1]._sRFID == "";
            }

            ST_C033_FOUP_ID_Verify_Reply stC033 = new ST_C033_FOUP_ID_Verify_Reply("");
            stC033.EQNo   = stR033.EQNo  ;
            stC033.PortNo = stR033.PortNo;
            stC033.FOUPID = stR033.FUOPID;
            stC033.ACK    = !isAllHomeEnd? "1" : (isEmptyFoupId ? "2" : "0"); //???

            //
            MsgEnqueue(stC033.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC033FOUPIDVerify(string eqno, string portno, string fid, string ack) //
        {
            /*
            <<
            <A [2] EQNo>	Communication parameter EQNo
            <A [2] PortNo>	Target port (01-04)
            <A [16] FOUPID>	Target FOUP ID
            <A [1] Result>	Judgment result (0: OK / 1: NG)

            >>
            <A [2] EQNo>	Same value as requested
            <A [2] PortNo>	Same value as requested
            <A [16] FOUPID>	Same value as requested
            <A [1] ACK>	Acceptance success / failure (0: OK / 1: NG (EFEM uninitialized) / 2: NG (not waiting for FOUP ID judgment))
            */


            ST_C033_FOUP_ID_Verify_Reply stC033 = new ST_C033_FOUP_ID_Verify_Reply("");
            stC033.EQNo   = eqno  ;
            stC033.PortNo = portno;
            stC033.FOUPID = fid   ;
            stC033.ACK    = ack   ;

            //
            MsgEnqueue(stC033.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC034PortSlotMapVerify() 
        {
            /*
            <<
            <A [2] EQNo>	Communication parameter EQNo
            <A [2] PortNo>	Target port (01-04)
            <A [16] FOUPID>	Target FOUP ID
            <A [1] Result>	Judgment result (0: OK / 1: NG)

            >> 
            <A [2] EQNo>	Same value as requested
            <A [2] PortNo>	Same value as requested
            <A [16] FOUPID>	Same value as requested
            <A [1] ACK>	    Acceptance success / failure 
                            0: OK 
                            1: NG (EFEM uninitialized)
                            2: NG (not waiting for Slot Map judgment)

            */
            int.TryParse(stR034.PortNo, out int nPortNo);
            bool isAllHomeEnd  = MOTR.IsAllHomeEnd();
            bool isEmptyMap    = true;
            if (nPortNo > 0)
            {
                isEmptyMap = !DM.MGZ[nPortNo-1].IsOneExist();
            }


            ST_C034_Port_Slot_Map_Verify_Reply stC034 = new ST_C034_Port_Slot_Map_Verify_Reply("");
            stC034.EQNo   = stR034.EQNo   ;
            stC034.PORTNO = stR034.PortNo ;
            stC034.FOUPID = stR034.FUOPID ;
            stC034.ACK    = !isAllHomeEnd ? "1" : (isEmptyMap ? "2" : "0");

            //
            MsgEnqueue(stC034.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC034PortSlotMapVerify(string eqno, string portno, string fid, string ack)
        {
            ST_C034_Port_Slot_Map_Verify_Reply stC034 = new ST_C034_Port_Slot_Map_Verify_Reply("");
            stC034.EQNo   = eqno  ;
            stC034.PORTNO = portno;
            stC034.FOUPID = fid   ;
            stC034.ACK    = ack   ;

            //
            MsgEnqueue(stC034.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC035PanelIDVerify()
        {
            /*
            <<
            <A [2] EQNo>	  Communication parameter EQNo
            <A [2] STAGEID>	  Target STAGE ID
            <A [2] PortNo>	  Target port (01-04)
            <A [2] SlotNo>	  Target slot (01-25)
            <A [16] PanelID>  Target panel ID
            <A [1] Result>	  Judgment result (0: OK / 1: NG)

            >> 
            <A [2] EQNo>	  Same value as requested
            <A [2] STAGEID>	  Target STAGE ID
            <A [1] ACK>	      Success or failure of acceptance (0: OK / 1: NG)
            */
            bool bOk = true; 

            //
            int.TryParse(stR035.PortNo , out int nPort   );
            int.TryParse(stR035.SlotNo , out int nslot   );
            int.TryParse(stR035.StageId, out int nStageId);
            if (nPort > 0 && nslot > 0 && nslot <= FM.ProjBase.iMaxMgzSlot[nPort - 1])
            {
                //DM.MGZ[nPort - 1].SetToPanelID(nslot-1, stR035.PanelId);
                DM.MGZ[nPort - 1].SetToPanelID(FM.ProjBase.iMaxMgzSlot[nPort - 1] - nslot, stR035.PanelId);
            }
            else bOk = false; 

            ST_C035_Panel_ID_Verify_Reply stC035 = new ST_C035_Panel_ID_Verify_Reply("");
            stC035.EQNo    = stR035.EQNo   ;
            stC035.STAGEID = stR035.StageId;
            stC035.ACK     = bOk ? "0":"1" ;
            
            //
            MsgEnqueue(stC035.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC035PanelIDVerify(string eqno, string stageid, string ack)
        {
            ST_C035_Panel_ID_Verify_Reply stC035 = new ST_C035_Panel_ID_Verify_Reply("");
            stC035.EQNo    = eqno;
            stC035.STAGEID = stageid;
            stC035.ACK     = ack;

            //
            MsgEnqueue(stC035.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC036PanelProcessStatus()
        {
            /*
            <<
            <A [2] EQNo>	Communication parameter EQNo
            <A [2] STAGEID>	Target STAGE ID
            <A [1] Status>	0: Panel is not on the device 
                            1: Start 
                            2: Pause 
                            3: Resume 
                            4: Completed (Process End, OK) 
                            5: Canceled (Process Cancel, NG) 
                            6: Panel is removed

            >> 
            <A [2] EQNo>	Same value as requested
            <A [2] STAGEID>	Same value as requested
            <A [1] ACK>	Success or failure of acceptance (0: OK / 1: NG)
            */


            ST_C036_Panel_Process_status_Reply  stC036 = new ST_C036_Panel_Process_status_Reply("");
            stC036.EQNo    = stR036.EQNo   ;
            stC036.STAGEID = stR036.StageId;
            stC036.ACK     = SEQ.IsAllHomeEnd()? "0":"1";
            
            //
            MsgEnqueue(stC036.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC036PanelProcessStatus(string eqno, string stageid, string ack = "0")
        {
            ST_C036_Panel_Process_status_Reply  stC036 = new ST_C036_Panel_Process_status_Reply("");
            stC036.EQNo    = eqno   ;
            stC036.STAGEID = stageid;
            stC036.ACK     = ack    ;
            
            //
            MsgEnqueue(stC036.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC037RestAlarmRequest()
        {
            /*
            <<
            <A [1] Alarm Reset>	1 = Reset

            >> 
            <A [1] ACK>	[ASCII 1 byte] Acknowledge Code
            */


            ST_C037_Reset_Alarm_Reply stC037 = new ST_C037_Reset_Alarm_Reply("");
            stC037.ACK     = "0";

            //
            if(stR037.Reset == "1") SEQ.Reset();
            
            //
            MsgEnqueue(stC037.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC037RestAlarmRequest(string ack)
        {

            ST_C037_Reset_Alarm_Reply stC037 = new ST_C037_Reset_Alarm_Reply("");
            stC037.ACK = ack;

            //
            MsgEnqueue(stC037.ToString());

        }
        
        /************************************************************************/
        /* EFEM → Equipment                                                     */
        /************************************************************************/
        public void CmdC600_FOUPIDReadResult(int port, string foupid = "")
        {
            /*
            <<
            <A [2] PortNo>	Port No (01, 02…)
            <A [112 112] FOUPID>	 [ASCII112 112 bytes] FOUP ID

            >> 
            <A [2] PortNo>	Same value as requested
            <A [1] ACK>	    Confirmation Code 0 = OK 1 = Not Accept

            */

            //
            stR600.Clear();

            if (port < 0) return;

            ST_C600_FOUP_ID_Read_Result_Report stC600 = new ST_C600_FOUP_ID_Read_Result_Report("");
            stC600.PortNo = string.Format($"{port+1:D2}");
            stC600.FOUPID = (foupid == "")? DM.MGZ[(int)port]._sRFID : foupid;

            //
            MsgEnqueue(stC600.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC600_FOUPIDReadResult(string port, string foupid)
        {
            int.TryParse(port, out int nPort);
            if(nPort>= 0) CmdC600_FOUPIDReadResult(nPort, foupid);
        }
        //------------------------------------------------------------------------
        public void CmdC601_FOUPIDWriteResult(int port, string foupid ="")
        {
            /*
            <<
            <A [2] PortNo>	Port No (01, 02…)
            <A [112 112] FOUPID>	 [ASCII112 112 bytes] FOUP ID

            >> 
            <A [2] PortNo>	Same value as requested
            <A [1] ACK>	    Confirmation Code 0 = OK 1 = Not Accept

            */

            //
            stR601.Clear();

            if (port < 0) return;

            ST_C601_FOUP_ID_write_Result_Report stC601 = new ST_C601_FOUP_ID_write_Result_Report("");
            stC601.PortNo = string.Format($"{port + 1:D2}");
            stC601.FOUPID = (foupid == "") ? DM.MGZ[(int)port]._sRFID : foupid;

            //
            MsgEnqueue(stC601.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC602_PortStatusReport(int port)
        {
            /*
            <<
            <A [2] PortNo>	      Target port (01-04)
            <A [1] PortStatus>	  Port status 
                                    0: Port disabled 
                                    1: Load Request (LDRQ) 
                                    2: Load Complete (LDCM) 
                                    3: Unload Request (UDRQ) 
                                    4: Unload Complete (UDCM)
            <A [112 112] FOUPID>  Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. 
                                  If there is no FOUP, fill it with "*".
            <A [2] Mode>	      Load port FOUP operating mode 
                                    00: Buffer 
                                    01: only Source (Load) 
                                    10: only Target (Unload) 
                                    11: both Source and Target

            >> 
            <A [2] PortNo>	      Same value as requested
            <A [1] ACK>	          Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */

            //
            stR602.Clear();

            if (port < 0) return;

            ST_C602_Port_Status_Report stC602 = new ST_C602_Port_Status_Report("");
            stC602.PortNo     = string.Format($"{port+1:D2}")  ;
            stC602.PortStatus = string.Format($"{(int)DM.MGZ[port].GetPortStatus()}");
            stC602.FOUPID     = DM.MGZ[port]._sRFID ;
          //stC602.Mode       = "11"; //both Source and Target
            stC602.Mode       = string.Format($"{(int)EN_PORT_STATUS_MODE.BothMode}");

            if (stC602.FOUPID == "") stC602.FOUPID = STR_FOUP_EMPTY;

            //
            MsgEnqueue(stC602.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC602_PortStatusReport(int port, EN_PORT_STATUS status, string foupid ="", string mode="")
        {
            /*
            <<
            <A [2] PortNo>	      Target port (01-04)
            <A [1] PortStatus>	  Port status 
                                    0: Port disabled 
                                    1: Load Request (LDRQ) 
                                    2: Load Complete (LDCM) 
                                    3: Unload Request (UDRQ) 
                                    4: Unload Complete (UDCM)
            <A [112 112] FOUPID>  Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. 
                                  If there is no FOUP, fill it with "*".
            <A [2] Mode>	      Load port FOUP operating mode 
                                    00: Buffer 
                                    01: only Source (Load) 
                                    10: only Target (Unload) 
                                    11: both Source and Target

            >> 
            <A [2] PortNo>	      Same value as requested
            <A [1] ACK>	          Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */

            //
            stR602.Clear();

            if (port < 0) return;

            ST_C602_Port_Status_Report stC602 = new ST_C602_Port_Status_Report("");
            stC602.PortNo     = string.Format($"{port+1:D2}")  ;
            stC602.PortStatus = string.Format($"{(int)status}");
            stC602.FOUPID     = foupid == ""? DM.MGZ[port]._sRFID : foupid;
          //stC602.Mode       = "11"; //both Source and Target
            stC602.Mode       = string.Format($"{(int)EN_PORT_STATUS_MODE.BothMode}");

            if (stC602.FOUPID == "") stC602.FOUPID = STR_FOUP_EMPTY;

            //
            MsgEnqueue(stC602.ToString());

        }
        //------------------------------------------------------------------------
        public void CmdC602_PortStatusReport(int port, string status, string foupid = "", string mode = "")
        {
            int.TryParse(status, out int portstatus);
            if(portstatus>=0) CmdC602_PortStatusReport(port, (EN_PORT_STATUS)portstatus, foupid, mode);
        }
        //------------------------------------------------------------------------
        public void CmdC604_PortSlotMapReport(int portno, string foupid ="", string maprd="")
        {
            /*
            <<
            <A [2] PortNo>	     Target port (01-04)
            <A [112 112] FOUPID> Fill in the space that is not enough for the maximum number of FOUP ID characters installed in the port. If there is no FOUP, fill it with "*".
            <A [25] MAPRD>	     Each slot indicates the status of each slot by one of the following: 
                0: No wafer
                1: Normal wafer placement (Wafer) 
                2: Crossed wafer placement (Crossed)
                ?: Undefined 
                W: Wafer Overlapping wafers

            >> 
            <A [2] PortNo>	     Same value as requested
            <A [1] ACK>	         Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */

            //
            stR604.Clear();

            if (portno < 0) return; 
            
            ST_C604_Port_Slot_Map_Report stC604 = new ST_C604_Port_Slot_Map_Report("");
            stC604.PortNo   = string.Format($"{portno + 1:D2}") ;
            stC604.FOUPID   = DM.MGZ[portno]._sRFID; //foupid; 
            stC604.MAPRD    = maprd == "" ?  SEQ.LPM1.GetMapRD(portno): maprd  ; 

            //
            MsgEnqueue(stC604.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC605_RobotStatusReport(string status = "")
        {
            /*
            <<
            <A [1] RobotStatus>	0 = idle 1 = Ready 2 = Run 3 = Alarm 4 = manual 5 = Pause

            >> 
            <A [1] ACK>	         Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */
            if (status == "")
            {
                switch (SEQ._iSeqStat)
                {
                    case EN_SEQ_STAT.Idle    : status = "0"; break; 
                    case EN_SEQ_STAT.Stop    : status = "0"; break; 
                    case EN_SEQ_STAT.Running : status = "2"; break; 
                    case EN_SEQ_STAT.Init    : status = "3"; break; 
                    case EN_SEQ_STAT.Error   : status = "3"; break; 
                    case EN_SEQ_STAT.DoorOpen: status = "4"; break; 
                    case EN_SEQ_STAT.RunWarn : status = "2"; break; 

                    default                  : status = "0"; break;
                }
            }

            //
            stR605.Clear();

            ST_C605_Robot_Status_Report stC605 = new ST_C605_Robot_Status_Report("");
            stC605.RBStatus   = status  ;

            //
            MsgEnqueue(stC605.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC611_PanelIDReadingStatusReport()
        {
            /*
            <<
            <A [2] PortNo>	  Target port (01-04)
            <A [2] SlotNo>	  Target slot (01-25)
            <A [16] PanelID>  The part of the panel ID that is not enough for the maximum number of characters is filled with spaces. If the panel ID reading fails, fill it with "*".
            <A [1] Readout>	  Type of read operation 
                            0: Invalid 
                            1: Read 
                            2: Key input 
                            3: Automatic input by EFEM


            >> 
            <A [1] ACK>	Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */
            string portno  = ((int)DM.WAF[(int)EN_WAF_ID.WTR]._iFromMgz).ToString("D2");
            string slotno  = DM.WAF[(int)EN_WAF_ID.WTR]._iSlot          .ToString("D2"); ;
            string panelid = DM.WAF[(int)EN_WAF_ID.WTR]._sBarCodeNo;
            string readout = "1";

            //
            stR611.Clear();

            ST_C611_Panel_ID_Reading_Status_Report stC611 = new ST_C611_Panel_ID_Reading_Status_Report("");
            stC611.PortNo  = portno  ;
            stC611.SlotNo  = slotno  ;
            stC611.PanelID = panelid ;
            stC611.ReadOut = readout ;

            //
            MsgEnqueue(stC611.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC611_PanelIDReadingStatusReport(string portno, string slotno, string panelid, string readout)
        {
            /*
            <<
            <A [2] PortNo>	  Target port (01-04)
            <A [2] SlotNo>	  Target slot (01-25)
            <A [16] PanelID>  The part of the panel ID that is not enough for the maximum number of characters is filled with spaces. If the panel ID reading fails, fill it with "*".
            <A [1] Readout>	  Type of read operation 
                            0: Invalid 
                            1: Read 
                            2: Key input 
                            3: Automatic input by EFEM


            >> 
            <A [1] ACK>	Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */
            
            //
            stR611.Clear();

            ST_C611_Panel_ID_Reading_Status_Report stC611 = new ST_C611_Panel_ID_Reading_Status_Report("");
            stC611.PortNo  = portno  ;
            stC611.SlotNo  = slotno  ;
            stC611.PanelID = panelid ;
            stC611.ReadOut = readout ;

            //
            MsgEnqueue(stC611.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC612_PanelCCDAlignStatusReport()
        {
            /*
            <<
            <A [2] PortNo>	        Target port (01-04)
            <A [2] SlotNo>	        Target slot (01-25)
            <A [16] PanelID>	    Target panel ID Fill in the space that is not enough for the maximum number of characters. If there is no panel ID, fill it with "*".
            <A [1] Align. Status>	Panel CCD Alignment state (1: OK / ≠ 1: Fail)


            >> 
            <A [1] ACK>	Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */
            bool   bWTR_Mount = DM.WAF[(int)EN_WAF_ID.WTR].IsWaferStat(EN_WAFER_STAT.Mount);
            string portno     = DM.WAF[(int)EN_WAF_ID.WTR]._iTargerMC.ToString("D2");
            string slotno     = bWTR_Mount ? "01" : DM.WAF[(int)EN_WAF_ID.WTR]._iSlot.ToString("D2");
            string panelid    = STR_FOUP_EMPTY;
            string align      = "1";

            //
            stR612.Clear();

            ST_C612_Panel_CCD_Alignment_Status_Report stC612 = new ST_C612_Panel_CCD_Alignment_Status_Report("")
            {
                PortNo  = portno,
                SlotNo  = slotno,
                PanelID = panelid,
                AStatus = align
            };

            //
            MsgEnqueue(stC612.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC612_PanelCCDAlignStatusReport(string portno, string slotno, string panelid, string align)
        {
            /*
            <<
            <A [2] PortNo>	        Target port (01-04)
            <A [2] SlotNo>	        Target slot (01-25)
            <A [16] PanelID>	    Target panel ID Fill in the space that is not enough for the maximum number of characters. If there is no panel ID, fill it with "*".
            <A [1] Align. Status>	Panel CCD Alignment state (1: OK / ≠ 1: Fail)


            >> 
            <A [1] ACK>	Success or failure of acceptance (0: acceptance / 1: refusal / 2 or more: other)

            */


            //
            stR612.Clear();

            ST_C612_Panel_CCD_Alignment_Status_Report stC612 = new ST_C612_Panel_CCD_Alignment_Status_Report("");
            stC612.PortNo  = portno  ;
            stC612.SlotNo  = slotno  ;
            stC612.PanelID = panelid ;
            stC612.AStatus = align   ;

            //
            MsgEnqueue(stC612.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC631_EFEM_StatusReport(string status = "")
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
            //
            stR631.Clear();

            ST_C631_EFEM_Status_Report stC631 = new ST_C631_EFEM_Status_Report("");
            stC631.EFEMStatus = status == ""? GetEFEMStatus() : status; 

            //
            MsgEnqueue(stC631.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC632_Panel_Transportation_Report(EN_ARM_ACTION armact, int whre)
        {
            /*
            <A [1] ARM>	Arm used 
                            1: Upper Arm 2: Lower Arm
            <A [1] ARM Action>	    Type of arm movement 
                            P: Panel storage (Put) 
                            G: Panel acquisition (Get) 
                            F: Operation completed (Finish) 
                            A: Suspended (Abort)
            <A [1] Position>	Transfer target 
                            1: load port side
                            2: device side - Main MC
            <A [2] UNIT NO>	        Target unit 
                        <Position> = 1, target port   (01 to 04) 
                        <Position> = 2, target device (01 to)
            <A [2] UNIT EXT>	    Target unit details 
                        If <Position> = 1, target slot (01 to 25), 
                        if <Position> = 2, target stage (01 to 02)
            <A [16] PanelID>	    Target panel ID Fill in the space that is not enough for the maximum number of characters.
            <A [16] SourceFOUPID>	Panel acquisition source FOUP ID Valid only in any of the following cases 
                        <ARM Action> = G and <Position> = 1 ・ 
                        <ARM Action> = P and <Position> = 2
            <A [2] SourcePortNo>	Panel acquisition source port (01 to 04) Valid only in any of the following cases ・ 
                        <ARM Action> = G and <Position> = 1 ・ 
                        <ARM Action> = P and <Position> = 2
            <A [2] SourceSlotNo>	Panel acquisition source slot (01 to 25) Valid only in any of the following cases ・ 
                        <ARM Action> = G and <Position> = 1 ・ 
                        <ARM Action> = P and <Position> = 2

            */
            //
            bool isMGZ = whre <= (int)(EN_WTR_WORK_AREA.MGZ2);

            stR632.Clear();

            string arm       = "1";
            string armaction = armact.ToString(); //pick ? "G" : "P";
            string pos       = (armact == EN_ARM_ACTION.P || armact == EN_ARM_ACTION.F) ? "2" : "1";
            string unitno    = pos == "2" ? "01" : (isMGZ? DM.MGZ[whre]._iTargerMC.ToString("D2") : DM.WAF[whre]._iTargerMC.ToString("D2"));
            string unitext   = pos == "2" ? DM.WAF[(int)EN_WAF_ID.WTR]._iSlot.ToString("D2") : (isMGZ? DM.MGZ[whre]._iTargerMC.ToString("D2") : DM.WAF[whre]._iTargerMC.ToString("D2"));
            string panelid   = DM.WAF[(int)EN_WAF_ID.WTR]._sBarCodeNo;
            string sfoupid   = DM.WAF[(int)EN_WAF_ID.WTR]._sRFID     ;
            string sportid   = whre.ToString("D2");
            string sslotno   = DM.WAF[(int)EN_WAF_ID.WTR]._iSlot.ToString("D2") ;

            ST_C632_Panel_Transportation_Report stC632 = new ST_C632_Panel_Transportation_Report("")
            {
                ARMNo      = arm      ,
                ARMAction  = armaction,
                PositionNo = pos      ,
                UNITNo     = unitno   ,
                UNITExt    = unitext  ,
                PanelID    = panelid  ,
                SrcFOUPID  = sfoupid  ,
                SrcPortNo  = sportid  ,
                SrcSlotNo  = sslotno
            };

            //
            MsgEnqueue(stC632.ToString());
        }

        //------------------------------------------------------------------------
        public void CmdC632_Panel_Transportation_Report(string arm, string armaction, string pos, string unitno, string unitext, string panelid, string sfoupid, string sportid, string sslotno)
        {
            /*
            <A [1] ARM>	Arm used 
                            1: Upper Arm 2: Lower Arm
            <A [1] ARM Action>	    Type of arm movement 
                            P: Panel storage (Put) 
                            G: Panel acquisition (Get) 
                            F: Operation completed (Finish) 
                            A: Suspended (Abort)
            <A [1] Position>	    Transfer target 
                            1: load port side
                            2: device side)
            <A [2] UNIT NO>	        Target unit 
                        <Position> = 1, target port   (01 to 04) 
                        <Position> = 2, target device (01 to)
            <A [2] UNIT EXT>	    Target unit details 
                        If <Position> = 1, target slot (01 to 25), 
                        if <Position> = 2, target stage (01 to 02)
            <A [16] PanelID>	    Target panel ID Fill in the space that is not enough for the maximum number of characters.
            <A [16] SourceFOUPID>	Panel acquisition source FOUP ID Valid only in any of the following cases 
                        <ARM Action> = G and <Position> = 1 ・ 
                        <ARM Action> = P and <Position> = 2
            <A [2] SourcePortNo>	Panel acquisition source port (01 to 04) Valid only in any of the following cases ・ 
                        <ARM Action> = G and <Position> = 1 ・ 
                        <ARM Action> = P and <Position> = 2
            <A [2] SourceSlotNo>	Panel acquisition source slot (01 to 25) Valid only in any of the following cases ・ 
                        <ARM Action> = G and <Position> = 1 ・ 
                        <ARM Action> = P and <Position> = 2

            */
            //
            stR632.Clear();

            ST_C632_Panel_Transportation_Report stC632 = new ST_C632_Panel_Transportation_Report("");
            stC632.ARMNo      = arm      ;
            stC632.ARMAction  = armaction;
            stC632.PositionNo = pos      ;
            stC632.UNITNo     = unitno   ;
            stC632.UNITExt    = unitext  ;
            stC632.PanelID    = panelid  ;
            stC632.SrcFOUPID  = sfoupid  ;
            stC632.SrcPortNo  = sportid  ;
            stC632.SrcSlotNo  = sslotno  ;

            //
            MsgEnqueue(stC632.ToString());
        }
        //------------------------------------------------------------------------
        public void CmdC690_AlarmEventReport(string unit, string unitext, string errorlabel, string code, string eset)
        {
            /*
            <A [1] UNIT       >	Unit (0: EFEM / 1: Robot / 2: Load port)
            <A [2] UNIT EXT   >	Unit details When <UNIT> = 2, the target port (01 to 04) When <UNIT> ≠ 2, "**"
            <A [1] Error Label>	1: Reset possible 2: Reset not possible
            <A [4] Error Code >	Error code (Refer to Error Code List in communication specifications)
            <A [1] Set        >	Alarm set / reset (0: reset / 1: set)
             
             */
            //
            stR690.Clear();

            ST_C690_Alarm_Event_Report stC690 = new ST_C690_Alarm_Event_Report("");
            stC690.UNIT     = unit      ;
            stC690.UNITExt  = unitext   ;
            stC690.ErrLabel = errorlabel;
            stC690.ErrCode  = code      ;
            stC690.SET      = eset      ; 

            //
            MsgEnqueue(stC690.ToString());
        }
        //------------------------------------------------------------------------
        private string GetEFEMStatus()
        {
            string OpMode   = "1";
            string status   = (!SEQ.IsAllHomeEnd() || EPU._bHasErr)? "0" : "1"; //1: Ready / 0: Preparing or error 
            string UpPanel  = DM.WAF[(int)EN_WAF_ID.WTR].IsWaferEmpty() ? "0" : DM.WAF[(int)EN_WAF_ID.WTR]._iTargerMC.ToString();
            string LowPanel = "D";
            string EQ1      = "1"; //??? EQ의 정의가...
            string EQ2      = "0";

            string LPort1   = "0"; //Auto, Manual??
            string LPort2   = "0";
            string LPort3   = "D";
            string LPort4   = "D";
            
            string T1       = "30";
            string T2       = "240";
            string D1       = "60";
            string D2       = "60";
            string D3       = "10";

            string ST_R     = ((int)LampBuzz._loRed   ).ToString();
            string ST_Y     = ((int)LampBuzz._loYellow).ToString();
            string ST_G     = ((int)LampBuzz._loGreen ).ToString();
            string ST_B     = "0";

            string EFMEFStatus = string.Format($"{OpMode}{status}{UpPanel}{LowPanel}{EQ1}{EQ2}" +
                                               $"{LPort1}{LPort2}{LPort3}{LPort4}" +
                                               $"{T1}{T2}{D1}{D2}{D3}{ST_R}{ST_Y}{ST_G}{ST_B}");
            return EFMEFStatus; 
        }

        //------------------------------------------------------------------------
        public void AddSendQue(string msg)
        {
            //
            string sType = msg.Substring(0, 1); //Type
            string sID   = msg.Substring(1, 3); //ID

            int.TryParse(sID, out int nIDNo);

            //Check Type
            if (sType != "C") return;
            if (nIDNo < 600 ) return; 

            ST_SENDED_DADT stSend = new ST_SENDED_DADT();
            stSend.dtSendTime = DateTime.Now;
            stSend.Id         = sID;
            

            //
            if (m_dcSendList.ContainsKey(sID))
            {
                stSend = m_dcSendList[sID];
                stSend.nRetryCnt++;
                stSend.dtSendTime = DateTime.Now;
                //
                m_dcSendList.Remove(sID); //Key 삭제
            }
            else
            {
                stSend.nRetryCnt  = 1;
                stSend.sMsg       = msg; 
            }
            stSend.bSended = false;

            //Add Key
            m_dcSendList.Add(sID, stSend);

        }
        //------------------------------------------------------------------------
        public void RemoveSendQue(string id)
        {
            if (m_dcSendList.ContainsKey(id))
            {
                m_dcSendList.Remove(id); //Key 삭제
            }
        }
        //------------------------------------------------------------------------
        public void CheckRcvData()
        {//check response 

            //
            //if (m_dcSendList.Count < 1) return; 

            //var
            TimeSpan timeDiff;
            DateTime dtNow  = DateTime.Now;
            
            //
            foreach (KeyValuePair<string, ST_SENDED_DADT> data in m_dcSendList)
            {
                if(data.Value.nRetryCnt >= 3)
                {
                    if (FM.IsAutoMode())
                    {
                        //Set Error
                        EPU.SetErr(EN_ERR_LIST.ERR_0300, true); //TCP/IP 통신 TimeOut
                    }

                    LOG.TCPIPTrace(string.Format($"Rcv timeout : CMD - {data.Key}"));
                    Reset();

                    //
                    m_dcSendList.Clear();
                }
                else
                {
                    timeDiff = dtNow - data.Value.dtSendTime;
                    if (timeDiff.Seconds > 3)
                    {
                        //if (m_CmdList.Contains(data.Value.sMsg)) return;
                        if (data.Value.bSended                 ) return; 
                        
                        m_bDrngComm = false;

                        //
                        ST_SENDED_DADT stSendData = new ST_SENDED_DADT();
                        stSendData = m_dcSendList[data.Value.Id];
                        stSendData.bSended = true;
                        m_dcSendList[data.Value.Id] = stSendData;

                        MsgEnqueue(data.Value.sMsg);
                    }
                }
            }
        }
        //------------------------------------------------------------------------
        public void ClearRcvCmdList  (int index) { m_bRcvCmdList  [index] = false                     ; }
        public void ClearReqRobotOper(         ) { m_nReqRobotOper        = (int)EN_ROBOT_OPER.none   ; }
        public void ClearReqPortOper (int port ) { m_nReqPortOper [port]  = (int)EN_PORT_OPER.none    ; }
        public void ClearReqTransport(         ) { m_nReqTransport        = (int)EN_TR_MODE.none; }
        public void ClearReqPortMode (int port ) { m_nReqPortMode [port]  = (int)EN_PORT_MODE.none    ; }
        
        public bool GetRcvCmdList    (int index) 
        {
            //if (FM.IsDryMode()) return true; 
            return m_bRcvCmdList  [index]; 
        }
        //------------------------------------------------------------------------
        public int GetReqRobotOper  (int man) 
        {
            //if (FM.IsDryMode()) return man;
            return m_nReqRobotOper; 
        }
        //------------------------------------------------------------------------
        public EN_PORT_OPER GetReqPortOper   (EN_PORT_ID port)
        {
            //if (FM.IsDryMode()) return man;
            return (EN_PORT_OPER)m_nReqPortOper [(int)port]; 
        }
        //------------------------------------------------------------------------
        public EN_TR_MODE GetReqTransport  () 
        {
            //if (FM.IsDryMode()) return man;
            return (EN_TR_MODE)m_nReqTransport; 
        }
        //------------------------------------------------------------------------
        public EN_PORT_MODE GetReqPortMode(EN_PORT_ID port)
        {
            //if (FM.IsDryMode()) return 1;
            return (EN_PORT_MODE)m_nReqPortMode[(int)port];
        }
        //------------------------------------------------------------------------

    }
}
