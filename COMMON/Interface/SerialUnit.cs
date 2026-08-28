using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TSerialUnit                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class TSerialUnit
    {

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        double[] m_dConverterToRising   = new double[100];
        byte[]   RecvBuf                = new byte[1024];
        int      m_iReadCnt                   ;
        int      m_iReadBufferCnt             ;
        byte     m_iRecvByte                  ;
        byte     m_iEndByte             = 0xFF;

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool _IsOpen
        {
            get { return Rs232.IsOpen; }
        }
        public int _iReadCnt
        {
            get { return m_iReadCnt; }
        }
        public byte _iRecvByte
        {
            get { return m_iRecvByte; }
        }

        public byte _iEndByte
        {
            get { return m_iEndByte;  }
            set { m_iEndByte = value; }
        }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public SerialPort Rs232   ;
        public delegate void OnRecieveMessage(object sender, int Len, byte[] str);
        public event         OnRecieveMessage OnRecieve;
        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSerialUnit()
        {
            Rs232               = new SerialPort();
            Rs232.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }
        ~TSerialUnit() { }
        //--------------------------------------------------------------------------       
        public bool Open(string sPortName, int iBaudRate = 9600, int iDataBit = 8, Parity iParity = Parity.None, StopBits iStopBits = StopBits.One)
        {
            try
            {
                if (Rs232.IsOpen) return true;
                
                Rs232.PortName     = sPortName;
                Rs232.BaudRate     = iBaudRate;
                Rs232.DataBits     = iDataBit ;
                Rs232.Parity       = iParity  ;
                Rs232.StopBits     = iStopBits;
                Rs232.ReadTimeout  = -1;
                Rs232.WriteTimeout = -1;


                Rs232.Open();
                return true;
            }
            catch
            {
                //MsgBox.Error(sPortName + " Rs232 Port Open Error");
                return false;
            }
        }
        //--------------------------------------------------------------------------       
        public bool Port_Close()
        {
            if (Rs232.IsOpen)
            {
                Rs232.Close();
            }
            return true;
        }
        //--------------------------------------------------------------------------       
        public bool SendString(string Data)
        {
            if (!Rs232.IsOpen) return false;
            Rs232.WriteLine(Data.ToString());
            return true;
        }
        //--------------------------------------------------------------------------       
        public bool SendByte(byte[] Data, int iDataLen = -1)
        {
            if (!Rs232.IsOpen) return false;
            if (iDataLen<0   ) iDataLen = Data.Length;

            Rs232.Write(Data, 0, iDataLen);
            
            char[] str = new char[Data.Length];
            for(int iz = 0 ; iz < Data.Length ; iz++)
            {
               str[iz] += (char)Data[iz];

            }
            return true;
        }
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                
                SerialPort sp = (SerialPort)sender;
                if (sp.BytesToRead >= 0)
                {
                    if (m_iEndByte == 0xFF)
                    {
                        m_iReadCnt = sp.Read(RecvBuf, 0, 1024);
                        m_iRecvByte = RecvBuf[m_iReadCnt - 1];

                        OnRecieve(this, m_iReadCnt, RecvBuf);
                    }   
                    else //통신 끊어짐 연결
                    {
                        m_iReadCnt = sp.Read(RecvBuf, m_iReadBufferCnt, 1024 - m_iReadBufferCnt);
                        m_iRecvByte = RecvBuf[m_iReadCnt + m_iReadBufferCnt - 1];

                        if (m_iRecvByte == m_iEndByte)
                        {
                            m_iReadCnt = m_iReadBufferCnt + m_iReadCnt;
                            m_iReadBufferCnt = 0;
                            OnRecieve(this, m_iReadCnt, RecvBuf);
                        }
                        else
                        {
                            m_iReadBufferCnt += m_iReadCnt;
                        }

                    }

                    //m_sRecString = sp.ReadExisting();
                    //m_iRcvLen  = sp.Read(RecvBuf, 0, (int)sp.BytesToRead);
                    //sp.Read(RecvBuf,1,(int)sp.BytesToRead);
                    /*
                    int i = 0;
                    while (sp.BytesToRead > 0)
                    {
                      RecvBuf[i]=sp.ReadByte();
                      i++;
                    }


                    OnRecieve(this, RecvBuf);
                    */
                }
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace(ex.ToString());
            }
        }
        public void DicardInBuffer()
        {
            if (!Rs232.IsOpen) return;
            Rs232.DiscardInBuffer();
        }
    }
}
    //사용예시
    //---------------------------------------------
    /*    
    public class TestClass
    {
        TSerialUnit RS232;
        public TestClass()
        {    
            RS232             = new TSerialUnit();
            RS232.Open("COM1");
            RS232.OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);
        }
        private void OnRecive(object sender, byte[] data)
        {
            string sString = Encoding.ASCII.GetString(data);     
        }
    }
    */