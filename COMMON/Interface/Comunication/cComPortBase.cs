using System;
using System.IO.Ports;
using System.Linq;

namespace eMachine
{
    public class cComPortBase 
    {
        #region < Fields > #
        private SerialPort mPort       = new SerialPort();
        private string     mPortName   = "COM1";
        private bool       mEnable     = true;
        public byte        mErrorCount = 0;
        #endregion

        #region < Properties >
        public string Name { get; set; }
        public string Key { get; set; }
        public string[] ConnectionParams { get; set; }

        public bool IsConnected { get; private set; }

        public bool IsEnable
        {
            get { return mEnable; }
            set
            {
                if (mEnable != value)
                {
                    mEnable = value;
                    if (mEnable == false) Close();
                }
            }
        }
        public bool IsDisposed { get; protected set; }

        public SerialPort Port
        {
            get { return mPort; }
        }
        #endregion

        #region < CONSTRUCTOR >

        public cComPortBase()
        {

        }
        public cComPortBase(string pComPortName)
        {
            mPortName = pComPortName;
        } 

        #endregion

        #region < Methods >

        /// <summary>
        /// 지정된 수량 만큼 패킷 받을때까지 대기
        /// </summary>
        /// <param name="pCnt">받을 수량</param>
        /// <param name="pTimeout_ms">타임 아웃</param>
        /// <returns></returns>
        protected bool WaitReceivePacketCount(int pCnt, int pTimeout_ms)
        {
            DateTime timeStamp = DateTime.Now;
            while (mPort.BytesToRead < pCnt)
            {
                System.Threading.Thread.Sleep(1);
                if ((DateTime.Now - timeStamp).TotalMilliseconds > pTimeout_ms) return false;
            }
            return true;
        }
        //--------------------------------------------------------------------------

        public bool Open(int pBaudRate, int pDataBits, Parity pParity, StopBits pStopBits)
        {
            Close();
            string[] ports = SerialPort.GetPortNames();
            if (ports != null && ports.Length > 0 && ports.Any(p => p == mPortName) == false) return false;
            try
            {
                mErrorCount = 0;
                IsConnected = false;
                mPort = new SerialPort
                {
                    PortName     = mPortName,
                    BaudRate     = pBaudRate,
                    DataBits     = pDataBits,
                    Parity       = pParity,
                    StopBits     = pStopBits,
                    WriteTimeout = -1,
                    ReadTimeout  = -1
                };
                mPort.Open();
                IsConnected = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------
        public void Close()
        {
            if (mPort != null)
            {
                mPort.Dispose();
                mPort = null;
            }
            IsConnected = false;
        }
        //--------------------------------------------------------------------------
        public virtual void Dispose()
        {
            IsDisposed = true;
            Close();
        }
        //--------------------------------------------------------------------------
        public void SetPortName(string pPortName)
        {
            Close();
            mPort = new SerialPort();
            mPortName = pPortName;
        }


        #endregion

    }
}
