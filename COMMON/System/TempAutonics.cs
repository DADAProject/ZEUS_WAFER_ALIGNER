using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace eMachine
{

    /***************************************************************************/
    /* Structures & Variables                                                  */
    /***************************************************************************/
    enum EN_MODEL : int
    {
        TK = 0,
        TM2   ,
        TM4   ,

        EndOfId 
    };

    enum EN_MODEL_CH : int
    {
        //TK : 1ch
        //TM2: 2ch
        //TM4: 4ch
        Ch1 = 0,
        Ch2    ,
        Ch3    ,
        Ch4    ,

        EndOfId 
    };

        //Temp. Controller ID.
    //===========================================================================
    public enum EN_TEMP_CH : int
    {
        None     = -1 ,
        tpcChuck =  0 ,
        tpcKnife =  1 , 
        EndOfCh  
    };

    public class TBUFF_AUTONICS
    {
       public int    iAddr;
       public int    iCh  ;
       public int    iCmd ;
       public int    iPara;
       public double dPara;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TBUFF_AUTONICS()
        {
            ResetData();
        }
        ~TBUFF_AUTONICS() { }
        public object Copy()
        {
            return this.MemberwiseClone();
        }
        public void ResetData()
        {
            iAddr  = 0;
            iCh    = 0;
            iCmd   = 0;
            iPara  = 0;
            dPara  = 0.0;
        }
    };



    /***************************************************************************/
    /* Class: TTempAutonics                                                    */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TTempAutonics
    {
        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int OFFSET        = 23   ;
        const int TX_BUFF       = 1024 ; 
        const int RX_BUFF       = 1024 ;    


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:    //Member Var.   
        int            m_iModel     ;
        int            m_iMaxCh     ;
        int[,]         m_iSV      = new int[(int)EN_TEMP_CH.EndOfCh, (int)EN_MODEL_CH.EndOfId]; //Set     Value.
        int[,]         m_iPV      = new int[(int)EN_TEMP_CH.EndOfCh, (int)EN_MODEL_CH.EndOfId]; //Process Value.
        int[,]         m_iBias    = new int[(int)EN_TEMP_CH.EndOfCh, (int)EN_MODEL_CH.EndOfId]; //Process Input Bias Value.
        int[,]         m_iSetSV   = new int[(int)EN_TEMP_CH.EndOfCh, (int)EN_MODEL_CH.EndOfId]; //Process Value.
        int[,]         m_iSetBias = new int[(int)EN_TEMP_CH.EndOfCh, (int)EN_MODEL_CH.EndOfId]; //Process Input Bias Value.

        bool           m_bDrngComm  ; //Process Value.
        bool           m_bErrComm   ; //Communication - 통신` 에러
        int            m_iSendStep  ; //Update Step - Read Cycle.
        bool           m_bWatchOn   ; //            - Controller의 상태(PV,SV,ST)를 모니터링 할 것인지를 결정.
        //bool           m_bErrTemp   ; //
        //Buffer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        byte[]                      m_szTxBuff  = new byte[TX_BUFF];
        public List<TBUFF_AUTONICS> m_CmdList   = new List<TBUFF_AUTONICS>();
        public TBUFF_AUTONICS       m_TxBuff    = new TBUFF_AUTONICS();   

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer  m_tSendTimer  = new TOnDelayTimer();
        TOnDelayTimer  m_tSendDelay  = new TOnDelayTimer();
 

         //Object.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TSerialUnit    m_RS232;


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
        public TTempAutonics()
        {
            m_szTxBuff.MemSet(0xFF);

            m_iModel = (int)EN_MODEL.TK;

            //
            if      (m_iModel == (int)EN_MODEL.TM2) m_iMaxCh = 2;
            else if (m_iModel == (int)EN_MODEL.TM4) m_iMaxCh = 4;
            else                                    m_iMaxCh = 1;
            
            //
            m_RS232            = new TSerialUnit();
            m_RS232.OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);
        }
        ~TTempAutonics() 
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
         
        public void SetSV(int Addr, int Ch , int Val)
        {
            m_iSetSV[Addr, Ch] = Val;
        }
        //------------------------------------------------------------------------
        public int GetSV(int Addr, int Ch = 1)
        {
            if (m_iModel == (int)EN_MODEL.TK) Ch = 0;
            //
            if (Addr < 0 || Addr >= (int)EN_TEMP_CH.EndOfCh) return 0;
            if (!CheckCh(Ch)) return 0;
            return m_iSV[Addr, Ch];
        }
        //------------------------------------------------------------------------
        public int GetPV(int Addr, int Ch = 1)
        {
            if (m_iModel == (int)EN_MODEL.TK) Ch = 0;
            //
            if (Addr < 0 || Addr >= (int)EN_TEMP_CH.EndOfCh) return 0;
            if (!CheckCh(Ch)) return 0;
            return m_iPV[Addr, Ch];
        }
        //------------------------------------------------------------------------

        public void SetBias(int Addr, int Ch , int Val)
        {
            m_iSetBias[Addr, Ch] = Val;
        }

        public int GetBias(int Addr, int Ch)
        {
            if (m_iModel == (int)EN_MODEL.TK) Ch = 0;
            //
            if (Addr < 0 || Addr >= (int)EN_TEMP_CH.EndOfCh) return 0;
            if (!CheckCh(Ch)) return 0;
            return m_iSV[Addr, Ch];
        }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init ( string sPortNo="COM1")
        {
            m_RS232.Open(sPortNo, 9600, 8, Parity.None, StopBits.Two );      
            
            if (!m_RS232._IsOpen)
            {
                MsgBox.Error($"[Temp Controller] COM Port[{sPortNo}] Open Fail");
                return; 
            }                       
            
            //Clear Queue.
            m_CmdList.Clear  (   );

            //Var.
            m_bWatchOn    = true;

        }       
        //------------------------------------------------------------------------
        public void Reset()
        {
            //m_CmdList   .Clear();
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
        bool SndMsg(TBUFF_AUTONICS TxBuff)
        {
            //Local Var.
            int iTxLen = 0;
            
            //Set Request Code.
            switch (TxBuff.iCmd) 
            {
                case (int)EN_TEMP_CMD.SetSV   : iTxLen = m_MakeMsgSetSV  (TxBuff.iAddr, TxBuff.iCh, TxBuff.iPara  ); break;
                case (int)EN_TEMP_CMD.GetPV   : iTxLen = m_MakeMsgGetPV  (TxBuff.iAddr, TxBuff.iCh                ); break;
                case (int)EN_TEMP_CMD.SetBias : iTxLen = m_MakeMsgSetBias(TxBuff.iAddr, TxBuff.iCh, TxBuff.iPara  ); break;
                case (int)EN_TEMP_CMD.GetBias : iTxLen = m_MakeMsgGetBias(TxBuff.iAddr, TxBuff.iCh                ); break;
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

        //------------------------------------------------------------------------
        void OnRecive(object sender, int len, byte[] data)
        {
            //Local Var.
            //uint getCRC = 0;
            int  iRcvAddr = Convert.ToInt32(data[0]) - 1;
            int  iRcvCh   = m_TxBuff.iCh;

            //Check.
            m_bDrngComm = false;
            m_bErrComm  = false;
            //
            m_TxBuff.ResetData();            
            //
            if (iRcvAddr < 0 || iRcvAddr > (int)EN_TEMP_CH.EndOfCh) return;
            //
            if (m_iModel == (int)EN_MODEL.TK ) iRcvCh = 0;
            else
            {
                if (!CheckCh(iRcvCh)) return;
            }
            //
            if (data[1] == 0x03)
            {
                //Get Bias
                m_iBias[iRcvAddr, iRcvCh] = data[3];
                m_iBias[iRcvAddr, iRcvCh] = (m_iBias[iRcvAddr, iRcvCh] << 8) | data[4];

                if (m_iBias[iRcvAddr, iRcvCh] < -999) m_iBias[iRcvAddr, iRcvCh] = -999;
                if (m_iBias[iRcvAddr, iRcvCh] >  999) m_iBias[iRcvAddr, iRcvCh] = 999;
            }
            if (data[1] == 0x04)
            {
                //getCRC = CRC16(data, 11);
                //if (data[11] != getCRC % 256) return;
                //if (data[12] != getCRC / 256) return;
                m_iPV[iRcvAddr, iRcvCh] = data[3];
                m_iPV[iRcvAddr, iRcvCh] = (m_iPV[iRcvAddr, iRcvCh] << 8) | data[4];
                if (m_iPV[iRcvAddr, iRcvCh] < 0  ) m_iPV[iRcvAddr, iRcvCh] = 0;
                if (m_iPV[iRcvAddr, iRcvCh] > 350) m_iPV[iRcvAddr, iRcvCh] = 350;

                //m_iSV = data[9];
                //m_iSV = (m_iSV << 8) | data[10];
                //if (m_iSV < 0) m_iSV = 0;
                //if (m_iSV > 250) m_iSV = 250;
            }
            if (data[1] == 0x06)
            {
                //getCRC = CRC16(data, 6);
                //if (data[6] != getCRC % 256) return;
                //if (data[7] != getCRC / 256) return;
                //300
                m_iSV[iRcvAddr, iRcvCh] = data[4];
                m_iSV[iRcvAddr, iRcvCh] = (m_iSV[iRcvAddr, iRcvCh] << 8) | data[5];

                if (m_iSV[iRcvAddr, iRcvCh] < 0  ) m_iSV[iRcvAddr, iRcvCh] = 0;
                if (m_iSV[iRcvAddr, iRcvCh] > 350) m_iSV[iRcvAddr, iRcvCh] = 350;
            }
        }
        //Make send message.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int m_MakeMsgGetPV(int Addr, int Ch)
        {
            //Local Var.
            uint getCRC  = 0;
            int  iLen    = 0;
            int  iStrtLo = 0xE8 + (Ch * 6); 
            int  iAddr   = Addr + 1;

            AttatchData(Convert.ToByte(iAddr  ));        //
            AttatchData(Convert.ToByte(0x04   ));        //Function
            AttatchData(Convert.ToByte(0x03   ));        //Start Address HI
            AttatchData(Convert.ToByte(iStrtLo));        //Start Address LO
            AttatchData(Convert.ToByte(0x00   ));        //데이타 Data 개수 HI
            iLen   = AttatchData(Convert.ToByte(0x04));  //데이타 Data 개수 LO
            getCRC = CRC16(m_szTxBuff, iLen);
            
            AttatchData(Convert.ToByte(getCRC%256));
            AttatchData(Convert.ToByte(getCRC/256));
            return iLen + 2;
        }
        int m_MakeMsgSetSV(int Addr, int Ch, int Value)
        {
            //Local Var.
            uint getCRC  = 0;
            int  iLen    = 0;
            int  iStrtHi = 0x00 + (Ch * 4);
            int  iStrtLo = 0x00 + (Ch * 5); 
            int  iAddr   = Addr + 1;

            AttatchData(Convert.ToByte(iAddr  ));
            AttatchData(Convert.ToByte(0x06   ));         //Function
            AttatchData(Convert.ToByte(iStrtHi));         //Start Address HI
            AttatchData(Convert.ToByte(iStrtLo));         //Start Address LO
            AttatchData(Convert.ToByte(Value >> 8));      //데이타 Data 개수 HI

            iLen   = AttatchData(Convert.ToByte(Value > 256 ? Value - 256 :Value ));  //데이타 Data 개수 LO
            getCRC = CRC16(m_szTxBuff, iLen);
            
            AttatchData(Convert.ToByte(getCRC%256));
            AttatchData(Convert.ToByte(getCRC/256));
            return iLen + 2;
        }

        // 검증 X
        int m_MakeMsgGetBias(int Addr, int Ch)
        {
            //Local Var.
            uint getCRC = 0;
            int iLen = 0;
            int iStrtLo = 0x9E
                ;
            int iAddr = Addr + 1;

            AttatchData(Convert.ToByte(iAddr));
            AttatchData(Convert.ToByte(0x03));         //Function
            AttatchData(Convert.ToByte(0x00));         //Start Address HI
            AttatchData(Convert.ToByte(iStrtLo));      //Start Address LO
            AttatchData(Convert.ToByte(0x00));         //데이타 Data 개수 HI
            iLen = AttatchData(Convert.ToByte(0x01));  //데이타 Data 개수 LO
            getCRC = CRC16(m_szTxBuff, iLen);

            AttatchData(Convert.ToByte(getCRC % 256));
            AttatchData(Convert.ToByte(getCRC / 256));
            return iLen + 2;
        }
        int m_MakeMsgSetBias(int Addr, int Ch, int Value)
        {
            //Local Var.
            uint getCRC = 0;
            int iLen = 0;
            int iStrtHi = 0x00;
            int iStrtLo = 0x9E;
            int iAddr = Addr + 1;

            AttatchData(Convert.ToByte(iAddr));
            AttatchData(Convert.ToByte(0x06));            //Function
            AttatchData(Convert.ToByte(iStrtHi));         //Start Address HI
            AttatchData(Convert.ToByte(iStrtLo));         //Start Address LO
            AttatchData(Convert.ToByte(Value >> 8));      //데이타 Data 개수 HI
            iLen = AttatchData(Convert.ToByte(Value));    //데이타 Data 개수 LO
            getCRC = CRC16(m_szTxBuff, iLen);

            AttatchData(Convert.ToByte(getCRC % 256));
            AttatchData(Convert.ToByte(getCRC / 256));
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
            
            //Attach.
            m_szTxBuff[iLast] = Data;

            //Ok.
            return (iLast + 1);
        }


        //Cmd.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void CmdSetSV(int Addr, int Ch, int  Val  )
        {
            if (Addr < 0 || Addr >= (int)EN_TEMP_CH.EndOfCh) return;
            if (!CheckCh(Ch)) return;
            //
            TBUFF_AUTONICS  m_TmpBuff  = new TBUFF_AUTONICS();
            //           
            m_iSetSV[Addr, Ch]   = Val;

            m_TmpBuff.iAddr = Addr;
            m_TmpBuff.iCh   = Ch  ;
            m_TmpBuff.iCmd  = (int)EN_TEMP_CMD.SetSV;
            m_TmpBuff.iPara = Val;
            m_CmdList.Add(m_TmpBuff); 
        }     
        //------------------------------------------------------------------------
        public void CmdGetPV(int Addr, int Ch)
        {
            if(Addr < 0 || Addr >= (int)EN_TEMP_CH.EndOfCh) return;
            if (!CheckCh(Ch)) return;
            //
            TBUFF_AUTONICS  m_TmpBuff  = new TBUFF_AUTONICS();
            //
            m_TmpBuff.iAddr = Addr;
            m_TmpBuff.iCh   = Ch  ;
            m_TmpBuff.iCmd  = (int)EN_TEMP_CMD.GetPV;
            m_CmdList.Add(m_TmpBuff); 
        }     
        //------------------------------------------------------------------------

        public void CmdSetBias(int Addr, int Ch, int  Val  )
        {
            if(Addr < 0 || Addr >= (int)EN_TEMP_CH.EndOfCh) return;
            if (!CheckCh(Ch)) return;
            //
            TBUFF_AUTONICS  m_TmpBuff  = new TBUFF_AUTONICS();
            //
            m_iSetBias[Addr, Ch]   = Val;

            m_TmpBuff.iAddr = Addr;
            m_TmpBuff.iCh   = Ch  ;
            m_TmpBuff.iCmd  = (int)EN_TEMP_CMD.SetBias;
            m_TmpBuff.iPara = Val ;
            m_CmdList.Add(m_TmpBuff); 
        }
        //------------------------------------------------------------------------
        public void CmdGetBias(int Addr, int Ch)
        {
            if(Addr < 0 || Addr >= (int)EN_TEMP_CH.EndOfCh) return;
            if (!CheckCh(Ch)) return;
            //
            TBUFF_AUTONICS  m_TmpBuff  = new TBUFF_AUTONICS();
            //
            m_TmpBuff.iAddr = Addr;
            m_TmpBuff.iCh   = Ch  ;
            m_TmpBuff.iCmd  = (int)EN_TEMP_CMD.GetBias;
            m_CmdList.Add(m_TmpBuff); 
        }
        //------------------------------------------------------------------------
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

            try 
            { 
                //Message Process..
                switch (m_iSendStep) 
                {
                    case  0: if ( m_CmdList.Count == 0)  { m_iSendStep = 0     ; break; }
                         m_iSendStep ++;
                         break;

                    case 1:
                             if (m_CmdList.Count < 1) break;
                             m_TxBuff = (TBUFF_AUTONICS)m_CmdList[0].Copy();
                             m_CmdList.RemoveAt(0); 
                             if (!SndMsg((m_TxBuff))) break;
                             m_szTxBuff.MemSet(0xFF);

                             m_tSendDelay.Clear();   
                             m_iSendStep ++;
                             break;


                    case 2: if(!m_tSendDelay.OnDelay(true, 50)) break;
                             if(m_bDrngComm                   ) break;   
                             m_tSendTimer.Clear(); //Clear Timer.
                             m_bErrComm = false ;
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
        public void Update      ()
        {
            //Update.
            if(m_bWatchOn && CanSendMsg()) 
            {
                for (int i = 0; i < (int)EN_TEMP_CH.EndOfCh; i++)
                {
                    for (int j = 0; j < m_iMaxCh; j++)
                    {
                        //Set SV
                        if (m_iSetSV[i, j] != m_iSV[i, j]) CmdSetSV(i, j, m_iSetSV[i, j]);
                        else                               CmdGetPV(i, j);

                        //Set Bias
                        //if (m_iSetBias[i, j] != m_iBias[i, j]) CmdSetBias(i, j, m_iSetBias[i, j]);
                        //else                                   CmdGetBias(i, j);
                      
                    }
                }
            }
            //
            UpdateMsg    ();
        }     
    }
}
