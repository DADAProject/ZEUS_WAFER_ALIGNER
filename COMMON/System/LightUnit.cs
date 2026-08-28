using GEM_XGemPro;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace eMachine
{
    public class TLightUnit : cComPortBase
    {
        private readonly Thread  mProcessThread = null;
        private Stopwatch KeepAlive { get; set; }
        public ushort Timeout_ms { get; set; } = 5000;

        public TLightUnit(): base()
        {
            KeepAlive      = Stopwatch.StartNew();
            mProcessThread = new Thread(Process)
            {
                IsBackground = true,
                Name = string.Format("TLightUnit.mProcessThread")
            };
            mProcessThread.Start();
        }
        //--------------------------------------------------------------------------
        public bool Open(string sPort)
        {
            try
            {
                if (Array.FindIndex(SerialPort.GetPortNames(), element => element == sPort) < 0)
                {
                    MessageBox.Show($"Light Com Port Error : {sPort}은 존재 하지 않습니다.");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Exception] - LightUnit-Open : " + ex.Message);
                return false;
            }

            //
            base.SetPortName(sPort);
            
            return base.Open(19200,8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
        }
        //--------------------------------------------------------------------------
        private void Process()
        {
            while(IsDisposed == false)
            {
                if (IsConnected && KeepAlive.ElapsedMilliseconds > Timeout_ms) //10 sec
                {
                    OFF();
                }

                Thread.Sleep(10);
            }
        }
        //--------------------------------------------------------------------------
        #region << Methods >>

        private void KeepLightAlive()
        {
            KeepAlive = Stopwatch.StartNew();
        }
        //--------------------------------------------------------------------------
        public void ON()
        {
            if(Port.IsOpen == false) return;
            
            Port.Write(Encoding.ASCII.GetString(OnFPack(true)));
            Port.Write(Encoding.ASCII.GetString(ValuePack(100)));

            KeepLightAlive();
        }
        //--------------------------------------------------------------------------
        public void OFF()
        {
            if(Port.IsOpen == false) return;
            Port.Write(Encoding.ASCII.GetString(OnFPack(false)));
            //Port.Write(Encoding.ASCII.GetString(ValuePack(0)));
        }
        //--------------------------------------------------------------------------
        public void SetValue(int iVal)
        {
            Port.Write(Encoding.ASCII.GetString(ValuePack(iVal)));
        }
        //--------------------------------------------------------------------------

        private byte[] ValuePack(int iVal)
        { 
            byte[] packet = new byte[8];
            packet[0] = 0x02;
            packet[1] = 0x31;
            packet[2] = Convert.ToByte('d');
            packet[3] = Convert.ToByte(iVal.ToString("0000")[0]);
            packet[4] = Convert.ToByte(iVal.ToString("0000")[1]);
            packet[5] = Convert.ToByte(iVal.ToString("0000")[2]);
            packet[6] = Convert.ToByte(iVal.ToString("0000")[3]);
            packet[7] = 0x03;

            return packet;
        }
        //--------------------------------------------------------------------------
        private byte[] OnFPack(bool bOnF)
        { 
            char cOnF = bOnF ? 'o' : 'f';
            byte[] packet = new byte[8];
            packet[0] = 0x02;
            packet[1] = 0x31;
            packet[2] = Convert.ToByte(cOnF);
            packet[3] = 0x00;
            packet[4] = 0x00;
            packet[5] = 0x00;
            packet[6] = 0x00;
            packet[7] = 0x03;

            return packet;
        }
        #endregion
        //--------------------------------------------------------------------------
        public new void Close()
        {
            base.Close();

            if (mProcessThread.IsAlive)
            {
                IsDisposed = true;
                if (mProcessThread.Join(1000)) mProcessThread.Abort();
            }

        }
    }
}
