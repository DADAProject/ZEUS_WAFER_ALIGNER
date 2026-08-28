using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


namespace eMachine
{
    /***************************************************************************/
    /* Class: TSerialUnit2                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    //Serial Style
    //===========================================================================
    public enum EN_COMM_TYPE {
        RcvLenFix , //Fixed Length.
        RcvLenVar , //Variable Length.
        RcvEndChr , //Use end character of stream.
        EndOfId
    };

    class TSerialUnit2
    {
        //Error Val
        //===========================================================================
        public const int MAX_TX_BUFF   =  1024;
        public const int MAX_RX_BUFF   =  1024;
        public const int COMERR_NONE   =    1 ;  //No Error.
        public const int COMERR_OPENED =  -10 ;  //Already Opened.
        public const int COMERR_NOOPEN =  -100;  //Open Error.
        public const int COMERR_NOSET  =  -101;  //Can't set the por.
        public const int COMERR_TX_TO  =  -200;  //Tx TimeOut.
        public const int COMERR_RX_TO  =  -201;  //Rx TimeOut.
        public const int COMERR_NO_TX  =  -205;  //Tx TimeOut.
        public const int COMERR_NO_RX  =  -206;  //Rx TimeOut.

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        IntPtr       m_hWnd;
        int          m_iwmNo;
        EN_COMM_TYPE m_iCommType;
        char         m_cEndChar ;

        byte[]       m_bTxBuf  = new byte  [MAX_TX_BUFF];
        byte[]       m_bRxBuf  = new byte  [MAX_RX_BUFF];

        int          m_iTxCnt    ;
        int          m_iRxCnt    ;
        int          m_iCommErr  ;

        int          m_iTxStep   ;
        int          m_iRxStep   ;

        int          m_iSetTxCnt ;
        int          m_iSetRxCnt ;
        int          m_iSetTimeOut;

        double[]     m_dScanTime = new double[3];
        double[]     m_dStrtTime = new double[3]; 

        //protected: /* Inheritable Vars.        */


        //public:    /* Direct Accessable Vars.  */
        
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   _IsOpen   { get { return m_Rs232.IsOpen ;           } }
        public int    _iTxCnt   { get { return m_iTxCnt       ;           } }
        public int    _iRxCnt   { get { return m_iRxCnt       ;           } }
        public bool   _HasTx    { get { return m_Rs232.BytesToWrite >  0; } }
        public bool   _HasRx    { get { return m_Rs232.BytesToRead  >  0; } }
        public int    _iCommErr { get { return m_iCommErr     ;           } }
        public char   _cEndChar { get { return m_cEndChar     ;           } set { m_cEndChar = value; } }
        public IntPtr _hWnd     { get { return m_hWnd         ;           } set { m_hWnd     = value; } }
        public int    _iwmNo    { get { return m_iwmNo        ;           } set { m_iwmNo    = value; } }
        public double GetTxScanTime() { return m_dScanTime[0]; }
        public double GetRxScanTime() { return m_dScanTime[1]; }
        public double GetTxRxTime  () { return m_dScanTime[2]; }

        public bool IsSend()
        {
            bool bChk1 =  m_iTxStep == 0;
            bool bChk2 = (m_iSetRxCnt == 0) || ((m_iSetRxCnt != 0) && (m_iRxStep == 0));
            return (bChk1 && bChk2);
        }
        public bool IsRcv()
        {
            switch (m_iCommType)
            {
                default : return false;
                case EN_COMM_TYPE.RcvLenFix : return (m_iRxCnt > 0) && (m_iRxCnt == m_iSetRxCnt) && (m_iRxStep == 0);
                case EN_COMM_TYPE.RcvLenVar : return (m_iRxCnt > 0)                                                 ;
                case EN_COMM_TYPE.RcvEndChr : return (m_iRxCnt > 0) && (m_bRxBuf[m_iRxCnt - 1] == m_cEndChar)       ;
            }
        }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        Thread          m_Th      = null;
        SerialPort      m_Rs232   = null;
        TOnDelayTimer   m_TxTimer = new TOnDelayTimer();
        TOnDelayTimer   m_RxTimer = new TOnDelayTimer();

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSerialUnit2()
        {
            //
            m_Th = new Thread(new ThreadStart(ThProc));
            m_Th.Start();
            //
            m_Rs232 = new SerialPort();
        }
        ~TSerialUnit2() 
        {
            m_Th.Join();
        }
        private void ThProc()
        {
            while (m_Th.IsAlive && m_Th != null)
            {
                Thread.Sleep(1);
                UpdateRx();
                UpdateTx();
            }
        }

        //--------------------------------------------------------------------------       
        public bool Open(EN_COMM_TYPE CommType, string sPortName, int iBaudRate = 9600, int iDataBit = 8, Parity iParity = Parity.None, StopBits iStopBits = StopBits.One, int SetTimeOut = 1000)
        {
            try
            {
                if (m_Rs232.IsOpen) return true;

                if (m_Rs232 == null) m_Rs232 = new SerialPort();
                m_Rs232.PortName     = sPortName;
                m_Rs232.BaudRate     = iBaudRate;
                m_Rs232.DataBits     = iDataBit ;
                m_Rs232.Parity       = iParity  ;
                m_Rs232.StopBits     = iStopBits;
                m_Rs232.ReadTimeout  = 500;
                m_Rs232.WriteTimeout = 500;
                m_Rs232.Open();
                //
                m_iCommType   = CommType;
                m_iSetTimeOut = SetTimeOut; 
                m_cEndChar    = (char)0x03;
                m_hWnd        = IntPtr.Zero;
                m_iwmNo       = 0x00;
                //
                m_iTxCnt = 0;
                m_iRxCnt = 0;
                m_iCommErr = COMERR_NONE;
                return true;
            }
            catch
            {
                MessageBox.Show(sPortName + " Rs232 Port Open Error");
                return false;
            }
        }
               
        public bool Port_Close()
        {
            if (m_Rs232.IsOpen)
            {
                m_Rs232.Close();
            }
            return true;
        }
        public bool ReOpen()
        {
            m_Rs232.DiscardInBuffer();
            m_Rs232.DiscardOutBuffer();
            Port_Close();
            Thread.Sleep(1000);
            m_Rs232.Open();
            return true;
        }
        public void DiscardInBuffer()
        {
            if ( m_Rs232 == null) return;
            if (!m_Rs232.IsOpen ) return;
            m_Rs232.DiscardInBuffer();
        }
        public void DiscardOutBuffer()
        {
            if ( m_Rs232 == null) return;
            if (!m_Rs232.IsOpen ) return;
            m_Rs232.DiscardOutBuffer();
        }

        public void ClearErr   () { m_iCommErr = COMERR_NONE; }
        public void ClearTxBuff() { m_iTxCnt = 0; Array.Clear(m_bTxBuf, 0 , m_bTxBuf.Length); }
        public void ClearRxBuff() { m_iRxCnt = 0; Array.Clear(m_bRxBuf, 0 , m_bRxBuf.Length); }

        //--------------------------------------------------------------------------
        public bool SendString(string Data)
        {
            if (!m_Rs232.IsOpen          ) return false;
            if (m_Rs232.BytesToWrite != 0) return false;

            try
            { 
                m_Rs232.WriteLine(Data.ToString());
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
                m_iCommErr = COMERR_NO_TX;
                return false;
            }
            //
            return true;
        }
        public bool SendByte(byte[] Data, int iDataLen = -1)
        {
            if (!m_Rs232.IsOpen          ) return false;
            if (m_Rs232.BytesToWrite != 0) return false;
            //
            if (iDataLen < 0 ) iDataLen = Data.Length;
            //
            try
            {
                m_Rs232.Write(Data, 0, iDataLen);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
                m_iCommErr = COMERR_NO_TX;
                return false;
            }
            //
            m_iTxCnt = Data.Length;
            //
            return true;
        }
        private int SendData(byte[] SndData, int iDataLen = -1)
        {
            if (!m_Rs232.IsOpen          ) return 0;
            if (m_Rs232.BytesToWrite != 0) return 0;
            //
            if (iDataLen < 0 ) iDataLen = SndData.Length;
            //
            try
            {
                m_Rs232.Write(SndData, 0, iDataLen);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
                m_iCommErr = COMERR_NO_TX;
                return 0;
            }
            //
            m_iTxCnt = SndData.Length;
            //
            return iDataLen;
        }
        private int RecvData(ref byte[] RcvData)
        {
            int      iRcvCnt = 0;
            int      nByteRead ;
            byte[]   bRcvData = new byte[MAX_RX_BUFF];
            //
            if (!m_Rs232.IsOpen) return COMERR_NOOPEN;
            //
            nByteRead  = m_Rs232.BytesToRead;
            //
            if (nByteRead >= MAX_RX_BUFF)
            {
                m_Rs232.DiscardInBuffer();
                ClearRxBuff();
                return 0;
            }
            if (nByteRead > 0)
            {
                try
                {
                    iRcvCnt = m_Rs232.Read(bRcvData, 0, MAX_RX_BUFF);
                }
                catch (Exception)
                {
                    m_Rs232.DiscardInBuffer();
                    ClearRxBuff();
                    nByteRead = 0;
                    m_iCommErr = COMERR_NO_RX;
                }
            }
            else return 0;
            //
            for (int n = 0; n < nByteRead; n++) RcvData[n] = bRcvData[n];
            return nByteRead;
        }
        public bool SetStream(byte[] Data, int TxLen, int RxLen, int TimeOut)
        {
            if (Data      == null       ) return false;
            if (m_iTxStep != 0          ) return false;
            if (TxLen     <= 0          ) return false;
            if (RxLen     >= MAX_RX_BUFF) return false;
            //
            ClearTxBuff();
            //
            m_iSetTxCnt   = TxLen  ;
            m_iSetRxCnt   = RxLen  ;
            m_iSetTimeOut = TimeOut;
            //
            try
            { 
                Array.Copy(Data, m_bTxBuf, TxLen);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
                ClearTxBuff();
                return false;
            }
            //
            m_iTxStep = 10;
            //
            return true;            
        }
        public int GetStream(ref byte[] Data)
        {
            int iCnt = 0;
            //
            if (Data        == null       ) return 0;
            if (Data.Length >  MAX_RX_BUFF) return 0;
            if (m_iRxCnt    <= 0          ) return 0;
            if (m_iCommType == (int)EN_COMM_TYPE.RcvLenFix)
            {
                if (m_iRxCnt != m_iSetRxCnt) return 0;
            }
            //
            iCnt = m_iRxCnt;
            try
            { 
                Array.Copy(m_bRxBuf, Data, iCnt);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
                ClearRxBuff();
                return 0;
            }
            //
            ClearRxBuff();
            return iCnt;
        }
        public int TxBuff()
        {
            if (m_iSetTxCnt <= 0) return 0;
            //
            int iRet = SendData(m_bTxBuf, m_iSetTxCnt);
            //
            return iRet;
        }
        public int RxBuff()
        {
            byte[] bBuff = new byte[MAX_RX_BUFF];
            //
            int iRet = RecvData(ref bBuff);
            //
            try
            { 
                for (int n = 0; n < iRet; n++)
                {
                    if ((m_iRxCnt + n) > MAX_RX_BUFF) return 0;
                    m_bRxBuf[m_iRxCnt + n] = bBuff[n];
                }
                m_iRxCnt += iRet;

                //if (iRet > 0) 
                //{ 
                //    Array.Copy(bBuff, m_bRxBuf, bBuff.Length);
                //    m_iRxCnt += iRet;
                //}
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
                ClearRxBuff();
                return 0;
            }
            //
            return m_iRxCnt;
        }

        //--------------------------------------------------------------------------
        public void UpdateTx()
        {
            if (!m_Th.IsAlive || m_Th == null) return;
            if (!m_Rs232.IsOpen              ) return;
            //
            if (m_TxTimer.OnDelay((m_iTxStep != 0), m_iSetTimeOut))
            {
                m_Rs232.DiscardOutBuffer();
                ClearTxBuff();
                m_TxTimer.Clear();
                m_iTxStep = 0;
                m_iCommErr = COMERR_NONE;
            }
            //
            switch(m_iTxStep)   
            {
                default : m_iTxStep = 0; return;
                case 10 : m_dStrtTime[0] = Environment.TickCount;
                          m_TxTimer.Clear();
                          m_iTxStep++;
                          return;
                case 11 : if (TxBuff() != m_iSetTxCnt) return;
                          m_iTxStep++;
                          return;
                case 12 : if (m_iCommType == EN_COMM_TYPE.RcvLenFix)
                          { 
                              if (m_iSetRxCnt != 0)
                              {
                                if (m_iRxCnt != 0) return;
                                m_iRxStep = 10;
                              }
                          }
                          m_dScanTime[0] = Environment.TickCount - m_dStrtTime[0];
                          m_dStrtTime[2] = Environment.TickCount;
                          m_iTxStep = 0; 
                          return;
            }
        }
        public void UpdateRx()
        {
            if (!m_Th.IsAlive || m_Th == null) return;
            if (!m_Rs232.IsOpen              ) return;
            //
            if (m_RxTimer.OnDelay(((m_iRxStep == 11) || (m_iRxStep == 1)), m_iSetTimeOut))
            {
                m_Rs232.DiscardInBuffer();
                ClearRxBuff();
                m_RxTimer.Clear();
                m_iRxStep = 0;
                m_iCommErr = COMERR_NONE;
            }

            if (m_iCommType == EN_COMM_TYPE.RcvLenFix)
            {
            //
                switch (m_iRxStep)
                {
                    default : m_iRxStep = 0; return;
                    case 0  : return;
                    case 10 : m_dStrtTime[1] = Environment.TickCount;
                              m_RxTimer.Clear();
                              m_iRxStep++;
                              return;
                    case 11 : if (RxBuff() != m_iSetRxCnt) return;
                              m_iRxStep++;
                              return;
                    case 12: if (m_hWnd != IntPtr.Zero)
                              {
                                  WinAPI.SendMessage(m_hWnd, m_iwmNo, 0, 0);
                              }
                              m_dScanTime[1] = Environment.TickCount - m_dStrtTime[1];
                              m_dScanTime[2] = Environment.TickCount - m_dStrtTime[2];
                              m_iRxStep = 0; 
                              return;
                }
            }
            else if (m_iCommType == EN_COMM_TYPE.RcvLenVar)
            {
                switch (m_iRxStep)
                {
                    default : m_iRxStep = 0; return;
                    case  0 : 
                              m_dStrtTime[1] = Environment.TickCount;
                              if (_HasRx) { m_RxTimer.Clear(); m_iRxStep++; }
                              return;
                    case  1 : if (RxBuff() <= 0) return;
                              m_iRxStep++;
                              return;
                    case  2: if (m_hWnd != IntPtr.Zero)
                              {
                                   WinAPI.SendMessage(m_hWnd, m_iwmNo, 0, 0);
                              }
                              m_dScanTime[1] = Environment.TickCount - m_dStrtTime[1];    
                              m_dScanTime[2] = Environment.TickCount - m_dStrtTime[2];
                              m_iRxStep = 0; 
                              return;
                }
            }
            else if (m_iCommType == EN_COMM_TYPE.RcvEndChr)
            {
                switch (m_iRxStep)
                {
                    case  0 : 
                              m_dStrtTime[1] = Environment.TickCount;
                              if (_HasRx) { m_RxTimer.Clear(); m_iRxStep++; }                         
                              return;
                    case  1 : 
                              if (RxBuff() <= 0                       ) return;
                              if (m_bRxBuf[m_iRxCnt - 1] != m_cEndChar) return;                              
                              m_iRxStep++;
                              return;
                    case  2 : if (FNC.GetByteArrayToString(m_bRxBuf, 0, m_iRxCnt) != "") return;
                              m_iRxStep++;
                              return;
                    case  3: if (m_hWnd != IntPtr.Zero)
                              {
                                   WinAPI.SendMessage(m_hWnd, m_iwmNo, 0, 0);
                              }
                              m_dScanTime[1] = Environment.TickCount - m_dStrtTime[1];
                              m_dScanTime[2] = Environment.TickCount - m_dStrtTime[2];
                              m_iRxStep = 0; 
                              return;
                }
            }
        }
    }
}
