using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TRfidTCP                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TRfidTCP
    {
        AsyncSocketClient  Client;
        Thread Th = null; //new Thread(new ThreadStart(ThProc));
        bool   m_bUpdate; 

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        bool   m_bConnected  ;
        bool   m_bError      ;
        bool   m_bTimeOut    ;
        int    m_iID         ;
        string m_sErrContext ;
        int    m_iMaxCmd     ;

        bool   m_bSended     ;
        int    m_iTxCmd      ;
        int    m_iRxCmd      ;
        string m_sTxData     ;
        string m_sRxData     ;

        double m_dSendedTime ;

        char[] m_szTxBuff = new char[1024];
        char[] m_szRxBuff = new char[1024];

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        List<bool> m_bAck = new List<bool>();

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool   _bConnected  { get { return m_bConnected ; } }  
		public bool   _bError      { get { return m_bError     ; } }
		public int    _iID         { get { return m_iID        ; } }
        public string _sErrContext { get { return m_sErrContext; } }
        public string _sRxData     { get { return m_sRxData    ; } }
		public int    _iMaxCmd     { get { return m_iMaxCmd    ; } }

        public TRfidTCP(int iID, int MaxCmdNo)
        {    
            Client             = new AsyncSocketClient(iID);
            Client.OnReceive  += new AsyncSocketReceiveEventHandler(OnReceive);
            Client.OnConnet   += new AsyncSocketConnectEventHandler(OnConnet );
            Client.OnClose    += new AsyncSocketCloseEventHandler  (OnClose  );
            Client.OnError    += new AsyncSocketErrorEventHandler  (OnError  );

            m_iID        = iID  ;
            m_iMaxCmd    = MaxCmdNo;
            m_bConnected = false; 
            m_bError     = false;
            m_bTimeOut   = false;

            for (int n = 0; n < m_iMaxCmd; n++) m_bAck.Add(false);

            m_bUpdate = true; 
            Th = new Thread(new ThreadStart(ThProc));
            Th.Start();
        }
        ~TRfidTCP() 
        {
            Th.Join();
        }
        //------------------------------------------------------------------------
        private void ThProc()
        {
            while(m_bUpdate) // && Th.IsAlive)
            {            
                Thread.Sleep(10);
                Update();                
            }          
        }
        /***************************************************************************/
        /* Client Socket Status                                                    */
        /***************************************************************************/
        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        { 
            m_bConnected = true ;    
            m_bError     = false;
        }
        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {  
            m_bConnected = false;
        }
        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {      
            m_bConnected  = false;  
            m_bError      = true ; 
            m_sErrContext = e.AsyncSocketException.Message;
        }
        private bool OnSend(int iCmd, string Data)
        {
            int uBufSize = 0;
            try
            {
                //
                byte[] classbyte = FNC.GetStringToByteArray(Data);
                if(classbyte != null)
                {
                    uBufSize   = classbyte.Length;
                    Array.Clear(m_szTxBuff, 0 , m_szTxBuff.Length);
                    m_szTxBuff = FNC.GetByteArrayToCharArray(classbyte, uBufSize);
                    //
                    Client.Send(classbyte);
 	                m_bAck[iCmd] = false;
                    //Log(true, iCmd, FNC.GetCharArrayToString(cmData.szBuf , 0, (int)uBufSize));
                    return true;
                }
                else
                {
                     //Log(true, iCmd, "SEND FAIL");
                    return false;
                }
            }
            catch (Exception e)
            {
                //Log(true, iCmd, "SEND FAIL");
                Debug.WriteLine($"[Exception] OnSend {e.Message}");
                return false;
            }
        }
        //------------------------------------------------------------------------
        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            //
            bool isRxFnsh = false;
            int  iRcvCmd  = (int)EN_RFID_TCP_COMD.tcENQ;           

            //
            if (e.ReceiveBytes <= 0   ) return;
            if (e.ReceiveBytes >= 1024) return;

            //
            byte[] btRxBuff = new byte[e.ReceiveBytes];
            Array.Copy(e.ReceiveData, 0, btRxBuff, 0, e.ReceiveBytes);
            //btRxBuff = e.ReceiveData;
            char chFnsh ;
            char chFrst = Convert.ToChar(btRxBuff[0]);
            //
            if ((chFrst == vDEF.chSTX) ||
                (chFrst == vDEF.chACK) ||
                (chFrst == vDEF.chNAK)) m_sRxData = "";
            for (int n = 0; n < e.ReceiveBytes; n++)
            {
                chFnsh = Convert.ToChar(btRxBuff[n]);
                if (chFnsh == vDEF.chETX) isRxFnsh = true;
            }
            m_sRxData += FNC.GetByteArrayToString(btRxBuff, 0, btRxBuff.Length);
            if (!isRxFnsh) return;
            
            //
            m_bSended = false;
            m_iRxCmd  = iRcvCmd;
            m_bAck[m_iRxCmd] = true;
            
            //
            if ((m_iRxCmd > -1) && (m_iRxCmd < m_iMaxCmd)) { }
            else { }
        }
        //------------------------------------------------------------------------
        public bool Connect(string sIP, int iPort)
        {  
            bool bRet = false;
            if (!m_bConnected)
            {
                ClearErr();
                bRet = Client.Connect(sIP, iPort);
            }
            else return true;

            return bRet;
        }
        //------------------------------------------------------------------------
        public bool Close()
        {  
            Client.Close();
            //Th.Join();

            KillThread();
            //
            return true;
        }
        //---------------------------------------------------------------------------
        public void KillThread()
        {
            if (Th.IsAlive)
            {
                m_bUpdate = false;
                if (Th.Join(1000)) Th.Abort();
            }
        }

        //------------------------------------------------------------------------
        public void Init()
        {
            m_bSended  = false;
            m_bTimeOut = false;
            m_iTxCmd   = -1   ;
            m_iRxCmd   = -1   ;
            m_sTxData  = ""   ;
            m_sRxData  = ""   ;

            Array.Clear(m_szTxBuff, 0 , m_szTxBuff.Length);
            Array.Clear(m_szRxBuff, 0 , m_szRxBuff.Length);            
        }
        //------------------------------------------------------------------------
        public void InitAck(int Cmd)
        {
            if ((Cmd < 0) || (Cmd >= m_iMaxCmd)) return;
            m_bAck[Cmd] = false;
        }
        public void ClearErr()
        {
            Init();
            m_bError = false ; 
        }
        public bool CanCommunicate()
        {
            if (!m_bConnected) return false;
            if ( m_bError    ) return false;
            if ( m_bTimeOut  ) return false;
            //
            return true;
        }
        public bool IsReceived(int Cmd)
        {
            if (m_iRxCmd != Cmd) return false;
            if (m_iRxCmd == -1 ) return false;
            //
            if (m_iRxCmd == Cmd)
            {
                m_iRxCmd = -1;
                return true;
            }
            //
            return false;
        }
        //public EN_TCP_STAT MakeSendMsg(int Cmd, char[] Data)
        //{
        //    if (CanCommunicate()) return EN_TCP_STAT.NoCon;
        //    //
        //    if (m_bSended) return EN_TCP_STAT.NoRecv;
        //    if (m_iRxCmd != -1) return (IsReceived(Cmd) ? EN_TCP_STAT.Ok : EN_TCP_STAT.NoRecv);
        //    if (m_iTxCmd != -1) return EN_TCP_STAT.NoRecv;
        //    //
        //    Init();
        //    InitAck(Cmd);
        //    //
        //    m_sTxData = Data;
        //    m_iTxCmd = Cmd;
        //    m_bSended = true;

        //    return EN_TCP_STAT.Sended;
        //}
        public EN_TCP_STAT MakeSendMsg(int Cmd, string Data)
        {
            if (!CanCommunicate()) return EN_TCP_STAT.NoCon;
            //
            if (m_bSended     ) return                                     EN_TCP_STAT.NoRecv ;
            if (m_iRxCmd != -1) return (IsReceived(Cmd) ? EN_TCP_STAT.Ok : EN_TCP_STAT.NoRecv);
            if (m_iTxCmd != -1) return                                     EN_TCP_STAT.NoRecv ;
            //
            Init   (   );
            InitAck(Cmd);
            //
            m_sTxData = Data;
            m_iTxCmd  = Cmd ;
            m_bSended = true;

            return EN_TCP_STAT.Sended;
        }
        //------------------------------------------------------------------------
        public void Update()
        {
            double dCrntTickTime = Environment.TickCount;
            
            //Send Timeout.
            if (m_bSended && ((dCrntTickTime - m_dSendedTime) > 5000)) 
            {
                m_bTimeOut = true;
                Init();
            }
            else 
            {
                if (!m_bSended) m_dSendedTime = dCrntTickTime;
                m_bTimeOut = false;
            }
            
            //
            if (!m_bTimeOut && m_bSended && (m_iTxCmd > -1))
            {
                m_dSendedTime = dCrntTickTime;
                OnSend(m_iTxCmd, m_sTxData);
                m_iTxCmd = -1;
            }
        }
    }
}
