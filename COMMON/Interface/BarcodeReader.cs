using System;
using System.Text;
using System.IO.Ports;
using System.Threading;
using BarcodeReaderLibrary;

namespace InoModule
{
    public class BarcodeReader : SerialPort
    {        

        public QueryTimer          dwell       = new QueryTimer();
        public LIBRARY_RETURN      ret         = new LIBRARY_RETURN();
        private TSerialUnit        rRS232 = new TSerialUnit();
        public bool isActivate
        {
            get
            {
                return IsOpen;
            }
        }

        internal TSerialUnit RRS232
        {
            get
            {
                return rRS232;
            }

            set
            {
                rRS232 = value;
            }
        }
         public void   Init (string sPortNo="COM1")
        {
            rRS232.Open(sPortNo);
        }

        public void activate(string portName)
        {
            ret.clear();
            try
            {
                deactivate(); if (ret.err) return;
                PortName = portName;
                BaudRate = 9600;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                ReadBufferSize = 256;
                WriteBufferSize = 256;
                ReadTimeout = 500;
                WriteTimeout = 500;
                DtrEnable = true;
                RtsEnable = true;
                Open();
                if (!isActivate)
                {
                    ret.err = true;
                    //ret.msg = string.Format("{0} COM PORT OPEN ERROR", b1.methodName(portName));
                    return;
                }
                DiscardOutBuffer();
                DiscardInBuffer();
                ReadExisting();

            }
            catch (Exception ex)
            {
                ret.err = true;
                //ret.msg = string.Format("{0} [{1} : {2}]", b1.methodName(portName), PortName, ex.Message);
                //msg.Exception(b1.methodName(portName), ex);
            }
        }
        public void deactivate()
        {
            ret.clear();
            try
            {
                if (!isActivate) return;
                DiscardOutBuffer();
                DiscardInBuffer();
                ReadExisting();
                Close();
            }
            catch (Exception ex)
            {
                ret.err = true;
                //ret.msg = string.Format("{0} [{1} : {2}]", b1.methodName(), PortName, ex.Message);
                //msg.Exception(b1.methodName(), ex);
            }

        }

        public double time = -1;

        byte[] BYTE = new byte[8];
        public bool read(out string id)
        {
            dwell.Reset(); time = -1;
            string buff = string.Empty;
            id = string.Empty;
            buff= ReadExisting();
            //b1.idle(10);
            try
            {
                BYTE[0] = 0x02;
                BYTE[1] = Convert.ToByte('Z');
                BYTE[2] = 0x03;
                //for (int i = 0; i < 100; i++)
                //{
                //    Write(BYTE, 0, 3);
                //    b1.idle(200);
                //    buff = ReadExisting();
                //    if (buff.Length != 0) break;
                //}
                //buff = ReadExisting();
                Write(BYTE, 0, 3);
                //b1.idle(200);
                buff = ReadExisting();

                BYTE[0] = 0x02;
                BYTE[1] = Convert.ToByte('Y');
                BYTE[2] = 0x03;
                Write(BYTE, 0, 3);
                time = dwell.Elapsed;
                if (buff.Length < 5) return false;
                if(buff.Length > 14)
                {
                    return false;
                }
                id = buff.Replace("\r","");
                return true;
            }
            catch (Exception ex)
            {
                //debug.add(DEBUG.EXCEPTION, string.Format("Barcod Reader Error [{0}]", ex.Message));
                return false;
            }
        }
    }
}
