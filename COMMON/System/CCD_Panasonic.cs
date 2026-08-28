using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace eMachine
{
    /***************************************************************************/
    /* Class:                                                                  */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TCCD_Panasonic
    {
        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int OFFSET        = 23   ;
        const int TX_BUFF       = 1024 ; 
        const int RX_BUFF       = 1024 ;    


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:    //Member Var.   
        int            m_iMaxCh     ;
        double[]       m_dMV      = new double[(int)EN_ADDR.EndOfId];    //Get Metered Value.

        bool           m_bDrngComm  ; //Process Value.
        bool           m_bErrComm   ; //Communication - 통신` 에러
        int            m_iSendStep  ; //Update Step - Read Cycle.
        bool           m_bWatchOn   ; //            - Controller의 상태(PV,SV,ST)를 모니터링 할 것인지를 결정.
        //bool           m_bErrTemp   ; //
        //Buffer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        byte[]                  m_szTxBuff  = new byte[TX_BUFF];
        public List<TBUFF_INFO> m_CmdList   = new List<TBUFF_INFO>();
        public TBUFF_INFO       m_TxBuff    = new TBUFF_INFO();   

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer  m_tSendTimer  = new TOnDelayTimer();
        TOnDelayTimer  m_tSendDelay  = new TOnDelayTimer();
 

         //Object.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TSerialUnit    m_RS232;
        public TAlignUnit AlignUnit = new TAlignUnit();
      
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    _bErr        {get { return m_bErrComm;} }
        public bool    _bWatchOn1   {get { return m_bWatchOn;        } set { m_bWatchOn = value;        } }
        


        ///Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //m_pDrawImg

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TCCD_Panasonic()
        {
            m_szTxBuff.MemSet(0xFF);

            //
            m_iMaxCh = 1;
            //
            m_RS232            = new TSerialUnit();
            m_RS232.OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);
        }
        ~TCCD_Panasonic() 
        { 
            if (m_RS232 != null) { m_RS232.Port_Close(); m_RS232 = null;}
        }

        private bool CheckCh(int Ch)
        {
            if (Ch <  0       ) return false;
            if (Ch >= m_iMaxCh) return false;
            //
            return true;
        }
         
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init ( string sPortNo="COM1")
        {
            //All OFF
            //MODBUS
            //RTU
            //8bit
            //Even
            //1bit
            m_RS232.Open(sPortNo, 115200, 8, Parity.Even, StopBits.One );      
            
            if (!m_RS232._IsOpen)
            {
                MsgBox.Error("[CCD Sensor] COM Port 1 Open Fail");
            }                       
            
            //Clear Queue.
            m_CmdList.Clear  (   );

            //Var.
            m_bWatchOn    = true;
        }       
        public void Reset()
        {
            m_CmdList   .Clear();
            m_tSendTimer.Clear();  
            m_bDrngComm = false;  
            m_iSendStep = 0; 

        }       
        public void Close()
        { 
            m_RS232.Port_Close();
        }   

        bool GetErrComm() { return m_bErrComm; }


        //Interface.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool SndMsg(TBUFF_INFO TxBuff)
        {
            //Local Var.
            int iTxLen = 0;
            //Set Request Code.
            switch (TxBuff.iCmd)
            {
                case (int)EN_CMD.GetMV   : iTxLen = m_MakeMsgGetMV(TxBuff.iAddr); break;
            }

            //Check Port.
            if (!m_RS232._IsOpen) return false;
            if (iTxLen <= 0     ) return false;
            m_bDrngComm = true;
            //Write Data./
            bool bRet = m_RS232.SendByte(m_szTxBuff,iTxLen);
            //Return.
            return bRet;
        }


        void OnRecive(object sender, int len, byte[] data)
        {
            //Local Var.
            //uint getCRC = 0;
            int  iRcvAddr = Convert.ToInt32(data[0]) - 1;

            //Check.
            m_bDrngComm = false;
            m_bErrComm  = false;
            //
            m_TxBuff.ResetData();            
            //
            if (iRcvAddr < 0 || iRcvAddr > (int)EN_ADDR.EndOfId) return;
            //

            if (data[1] == 0x83)
            {
                //예외 코드
                //if      (data[2] == 0x01) //ILLEGAL FUNCTION
                //else if (data[2] == 0x02) //ILLEGAL DATA ADDRESS
                //else if (data[2] == 0x03) //ILLEGAL DATA VALUE
                //else                      //SERVER DEVICE FAILURE
            }
            else if(data[1] == 0x03)
            {
                int iLengh = Convert.ToInt32(data[2]);
                int iValue = GetValue(data);
                m_dMV[iRcvAddr] = Convert.ToDouble(iValue / 10000.0);
            }

        }
        //Make send message.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        int m_MakeMsgGetMV(int Addr)
        {
            //Local Var.
            uint getCRC   = 0;
            int  iLen     = 0;
            int  iStrtHi  = 0x64;//0x64
            int  iRgstLen = 0x02; 
            int  iAddr    = Addr + 1;

            AttatchData       (Convert.ToByte(iAddr         ));
            AttatchData       (Convert.ToByte(0x03          )); //Function
            AttatchData       (Convert.ToByte(iStrtHi  / 256)); //Start Address HI
            AttatchData       (Convert.ToByte(iStrtHi  % 256)); //Start Address LO
            AttatchData       (Convert.ToByte(iRgstLen / 256)); //Start Regist Lengh HI
            iLen = AttatchData(Convert.ToByte(iRgstLen % 256)); //Start Regist Lengh LO

            getCRC = CRC16(m_szTxBuff, iLen);
            
            AttatchData(Convert.ToByte(getCRC%256));
            AttatchData(Convert.ToByte(getCRC/256));
            return iLen + 2;
        }


        //---------------------------------------------------------------------------
        int AttatchData(byte[] Data , int Cnt)
        {
            //Local Var.
            int iLast = 0;
            byte byteNull = 0xFF;
            iLast = Array.IndexOf(m_szTxBuff, byteNull);

            //Check Max.
            if ((iLast + Cnt) >= 128) {
                Array.Clear(m_szTxBuff,0,TX_BUFF);
                return 0;
                }

            //Attatch.
            Array.Copy(Data, 0, m_szTxBuff, iLast, Cnt);

            //Ok.
            return (iLast + Cnt);
        }


        //---------------------------------------------------------------------------
        int AttatchData(byte Data)
        {
            //Local Var.
            int iLast = 0;
            byte byteNull = 0xFF;
            iLast = Array.IndexOf(m_szTxBuff, byteNull);

            //Check Max.
            if ((iLast + 1) >= 128) {
                Array.Clear(m_szTxBuff,0,TX_BUFF);
                return 0;
                }
            //Attatch.
            m_szTxBuff[iLast] = Data;

            //Ok.
            return (iLast + 1);
        }


        //Cmd.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        public void CmdGetMeter(int Addr)
        {
            if(Addr < 0 || Addr >= (int)EN_ADDR.EndOfId) return;
            //
            TBUFF_INFO  m_TmpBuff  = new TBUFF_INFO();
            //
            m_TmpBuff.iAddr = Addr;
            m_TmpBuff.iCmd  = (int)EN_CMD.GetMV;
            m_CmdList.Add(m_TmpBuff); 
        }     

        uint CRC16(byte[] puchMsg, int usDataLen)
        {
	        int i           ;
            int iDataCnt = 0;
	        uint crc, flag;

	        crc = 0xFFFF;

	        while(usDataLen>0)
	        {
		        crc ^= puchMsg[iDataCnt++];

		        for (i=0; i<8; i++)
		        {
			        flag = crc & 0x0001;
			        crc >>= 1;
			        if(flag==1) crc ^= 0xA001;
		        }
                usDataLen--;
	        }

	        return crc;
        }
        private int GetValue(byte[] bytes)
        {
            int iDecLo = 0x0000, iDecHi = 0x0000;

            iDecLo = (bytes[3] << 8) | bytes[4];
            iDecHi = ((bytes[5] << 8) | bytes[6]) * UInt16.MaxValue;

            return iDecLo + iDecHi;
        }

        private string ByteToHex(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }

        //Check Comm. Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool CanSendMsg()
        {
            if (!m_RS232  ._IsOpen      ) return false;
            if ( m_CmdList.Count != 0   ) return false;
            if (m_bDrngComm             ) return false;
            if (m_iSendStep != 0        ) return false;
            return true;
        }     

        //Update Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        void UpdateMsg   ()
        {
            //Local Var.

            //Update.
            if (m_tSendTimer.OnDelay((m_iSendStep != 0 || m_bDrngComm), 1000)) {
                Reset();  
                m_bErrComm = true;
            }

            try { 
            //Message Process..
            switch (m_iSendStep) {
                case  0: if ( m_CmdList.Count == 0)  { m_iSendStep = 0     ; break; }
                         m_iSendStep ++;
                         break;

                case 1: 
                         m_TxBuff = (TBUFF_INFO)m_CmdList[0].Copy();
                         m_CmdList.RemoveAt(0); 
                         if (!SndMsg((m_TxBuff))) break;
                         m_szTxBuff.MemSet(0xFF);

                         m_tSendDelay.Clear();   
                         m_iSendStep ++;
                         break;


                case 2: if(!m_tSendDelay.OnDelay(true, 10)) break;
                         if(m_bDrngComm                   ) break;   
                         m_tSendTimer.Clear(); //Clear Timer.
                         m_bErrComm = false ;
                         m_iSendStep = 0;
                         break;
                }
            }
            catch (Exception ex)
            {
                //cDEF.LOG.ExceptionTrace("UpdateMsg. Update " + ex.ToString());
                System.Diagnostics.Debug.WriteLine("Exception:" + ex.Message);
            } 
        }        
        public void Update      ()
        {
            //Update.
            if(m_bWatchOn && CanSendMsg()) 
            {
                for (int i = 0; i < (int)EN_ADDR.EndOfId; i++)
                {
                    CmdGetMeter(i);
                }
            }
            //
            UpdateMsg    ();
        }


        //User Properties
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public double GetMeteredValue(int Address)
        {
            return m_dMV[Address];
        }
    }
}
