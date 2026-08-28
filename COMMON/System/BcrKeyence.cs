using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    public enum EN_KEYENCE_CMD : int
    {
        None = -1,
        SetTrigger, //Set Trigger
    };

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public class TBUFF_KEYENCE
    {
        public int    iCmd ;
        public int    iPara;
        public double dPara;
        public bool   bPara;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TBUFF_KEYENCE()
        {
            ResetData();
        }
        ~TBUFF_KEYENCE() { }
        public object Copy()
        {
            return this.MemberwiseClone();
        }
        //------------------------------------------------------------------------
        public void ResetData()
        {
            iCmd  = 0;
            iPara = 0;
            dPara = 0.0;
            bPara = false;
        }
    };


    /************************************************************************/
    /*                                                                      */
    /************************************************************************/

    public class TBcrKeyence
    {
        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int TX_BUFF = 1024;
        const int RX_BUFF = 1024;

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   // Member Var.            
        byte[] m_szTxBuff = new byte[TX_BUFF];

        string m_sRcvMsg  ;
        bool   m_bDrngComm; //Process Value.
        bool   m_bErrComm ; //Communication - 통신` 에러
        int    m_iSendStep; //Update Step - Read Cycle.
        bool   m_bWatchOn ; //- Controller의 상태를 모니터링 할 것인지를 결정.

        string m_sReadBcr ; //Read 된 BCR 

        bool   m_bConnect ; //Communication Connect Flag
        bool   m_bRetry   ; //Communication Retry Connect

        //protected: //Inheritable Vars.        
        string m_sHostAddress = "192.168.100.100"; //
        int    m_iPort        = 9004; //

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_tSendTimer = new TOnDelayTimer();
        TOnDelayTimer m_tSendDelay = new TOnDelayTimer();

        //public:    //Direct Accessible Vars.  
        //Buffer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public List<TBUFF_KEYENCE> m_CmdList = new List<TBUFF_KEYENCE>();
        public TBUFF_KEYENCE m_TxBuff = new TBUFF_KEYENCE();

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   _bErr      { get { return m_bErrComm; } }
        public bool   _bWatchOn1 { get { return m_bWatchOn; } set { m_bWatchOn = value; } }
        public string _sReadBcr  { get { return m_sReadBcr; } }

        public bool _IsConnect
        {
            get { return m_bConnect; }
        }

        public bool _IsRetry
        {
            get { return m_bRetry; }
        }

        //Objects.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        AsyncSocketClient Socket;

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TBcrKeyence()
        {
            Socket = new AsyncSocketClient(0);
            Socket.OnReceive += DataReceivedHandler;
            Socket.OnConnet  += ConnectHandler     ;
            Socket.OnClose   += DisconnectHandler  ;

        }
        //--------------------------------------------------------------------------
        ~TBcrKeyence()
        {


        }
        //--------------------------------------------------------------------------       
        public bool Connect(string ip, int port)
        {
            //
            m_sHostAddress = ip  ;
            m_iPort        = port;

            try
            {
                if (Socket.Connection != null && Socket.Connection.Connected == true) return true;

                Socket.Connect(m_sHostAddress, m_iPort);

                //Clear Queue.
                m_CmdList.Clear();

                //Var.
                m_bWatchOn = true;

                return true;
            }
            catch
            {
                MsgBox.Error(m_sHostAddress + " Client Connect Error");
                return false;
            }
        }
        //------------------------------------------------------------------------
        public bool DisConnect()
        {
            try
            {
                Socket.Close();
                Socket = null;

                return true;
            }
            catch
            {
                MsgBox.Error(m_sHostAddress + " Client DisConnect Error");
                return false;
            }
        }
        //--------------------------------------------------------------------------
        public void Reset()
        {
            m_CmdList   .Clear();
            m_tSendTimer.Clear();

            m_bDrngComm = false;
            m_iSendStep = 0;
            m_sReadBcr  = string.Empty;
        }
        //------------------------------------------------------------------------
        public void DataClear()
        {
            m_sReadBcr = string.Empty;
        }
        //Make send message.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int m_MakeMsgSetTrigger(bool On)
        {
            int iLen = 0;

            iLen = AttatchData(On ? "LON\r" : "LOFF\r");        //

            return iLen;
        }
        //---------------------------------------------------------------------------

        int AttatchData(string sData)
        {
            //Local Var.
            int  iLast    = 0;
            //byte byteNull = 0x00;
           //iLast       = Array.IndexOf(m_szTxBuff, byteNull);

            byte[] Data = ASCIIEncoding.ASCII.GetBytes(sData);
            int Cnt = Data.Length;

            //Check Max.
            if ((iLast + Cnt) >= 128)
            {
                Array.Clear(m_szTxBuff, 0, TX_BUFF);
                return 0;
            }

            //Attatch.
            Array.Copy(Data, 0, m_szTxBuff, iLast, Cnt);

            //Ok.
            return (iLast + Cnt);
        }
        //------------------------------------------------------------------------
        int AttatchData(byte[] Data, int Cnt)
        {
            //Local Var.
            int iLast = 0;
            byte byteNull = 0xFF;
            iLast = Array.IndexOf(m_szTxBuff, byteNull);

            //Check Max.
            if ((iLast + Cnt) >= 128)
            {
                Array.Clear(m_szTxBuff, 0, TX_BUFF);
                return 0;
            }

            //Attatch.
            Array.Copy(Data, 0, m_szTxBuff, iLast, Cnt);

            //Ok.
            return (iLast + Cnt);
        }

        //Cmd.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void CmdSetRead()
        {
            //
            Reset();

            //
            TBUFF_KEYENCE m_TmpBuff = new TBUFF_KEYENCE();
            
            //           
            m_TmpBuff.bPara = true;
            m_CmdList.Add(m_TmpBuff);

        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Interface.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool SndMsg(TBUFF_KEYENCE TxBuff)
        {
            //Local Var.
            int iTxLen = 0;

            //Set Request Code.
            switch (TxBuff.iCmd)
            {
                case (int)EN_KEYENCE_CMD.SetTrigger: iTxLen = m_MakeMsgSetTrigger(TxBuff.bPara); break;
            }

            //Check Port.
            if (!_IsConnect) return false;
            if (iTxLen <= 0) return false;

            m_bDrngComm = true;

            //Write Data./
            bool bRet = Socket.Send(m_szTxBuff, iTxLen);

            string sTx = FNC.GetByteArrayToHexString(m_szTxBuff, 0, iTxLen);
            cDEF.LOG.BarcodeTrace("[SND]" + sTx);

            //Return.
            return bRet;
        }
        //------------------------------------------------------------------------
        public void DataReceivedHandler(object sender, AsyncSocketReceiveEventArgs e)
        {
            //
            m_TxBuff.ResetData();

            int    iLength = e.ReceiveBytes;
            byte[] bRecive = new byte[iLength];
            Array.Copy(e.ReceiveData, 0, bRecive, 0, iLength);

            m_sReadBcr = Encoding.GetEncoding("Shift_JIS").GetString(bRecive);

            m_sRcvMsg = m_sReadBcr.Trim();

            cDEF.LOG.BarcodeTrace(m_sRcvMsg);
            
            //Check.
            m_bDrngComm = false;
            m_bErrComm  = false;
        }
        //------------------------------------------------------------------------
        public void ConnectHandler(object sender, AsyncSocketConnectionEventArgs e)
        {
            m_bConnect = true;
        }
        //--------------------------------------------------------------------------
        public void DisconnectHandler(object sender, AsyncSocketConnectionEventArgs e)
        {
            m_bConnect = false;

            if (m_bRetry) this.Connect(m_sHostAddress, m_iPort);
        }

        //--------------------------------------------------------------------------       

        //Check Comm. Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
        void UpdateMsg()
        {
            //Local Var.

            //Update.
            if (m_tSendTimer.OnDelay((m_iSendStep != 0 || m_bDrngComm), 10000))
            {
                Reset();
                m_bErrComm = true;
            }

            try
            {
                //Message Process..
                switch (m_iSendStep)
                {
                    case 0:
                        if (m_CmdList.Count == 0) { m_iSendStep = 0; break; }
                        m_iSendStep++;
                        break;

                    case 1:
                        if (m_CmdList.Count < 1)
                        {
                            m_iSendStep = 0;
                            break;
                        }

                        m_TxBuff = (TBUFF_KEYENCE)m_CmdList[0].Copy();
                        m_CmdList.RemoveAt(0);
                        
                        if (!SndMsg((m_TxBuff))) break;
                        m_szTxBuff.MemSet(0xFF);

                        m_tSendDelay.Clear();
                        m_iSendStep++;
                        break;


                    case 2:
                        if (!m_tSendDelay.OnDelay(true, 1000)) break;
                        if (m_bDrngComm) break;

                        m_tSendTimer.Clear(); //Clear Timer.
                        m_bErrComm  = false;
                        m_iSendStep = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("UpdateMsg. Update " + ex.ToString());

            }
        }
        //------------------------------------------------------------------------
        public void Update()
        {
            if (m_bWatchOn && CanSendMsg())
            {

            }
            UpdateMsg();
        }
    }
}
