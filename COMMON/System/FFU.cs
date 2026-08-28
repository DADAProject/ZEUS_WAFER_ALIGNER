using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static Emgu.CV.DepthAI.Camera;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace eMachine
{
    enum EN_FFU_CH : int
    {
        Ch1 = 0,
        Ch2,
        Ch3,
        Ch4,

        EndOfId
    };

    //Temp. Command ID.
    //===========================================================================
    public enum EN_FFU_CMD : int
    {
        None = -1  ,
        SetSV      , //지정속도 변경
        GetPV      , //Get PV, ALARM, SV

    };

    /************************************************************************/
    /* 
     * <Tx(PC에서 LV32에 상태요청을 위한 데이터 구조): HOST[Ask: PV & ALARM & SV] -> LV32>
     *  STX: 0x02 (고정 값) 
        MODE1: 0x8A (고정 값: BLOCK READ) 
        MODE2: 0x87 (고정 값: Process Value(PV)/Alarm Data/Setting Value(SV)) 
        LV32_ID: 1 ~ 32 LV32 ID 선택, 데이터 전송 시 0x80으로 |(or) 연산. 
        DPU ID: 0x9F(고정 값) 
        Start ICU_ID: 1 ~ 32 ICU ID 선택, 데이터 전송 시 0x80으로 |(or) 연산(Start ID는 End ID보다 작거나 같아야 함). 
        End ICU_ID: 1 ~ 32 ICU ID 선택, 데이터 전송 시 0x80으로 |(or) 연산(End ID는 Start ID보다 크거나 같아야 함). 
        Check Sum: STX와 ETX를 제외한 나머지 페킷의 총합의 하위 1Byte 사용함. 
        (ex: MODE1 + MODE2 + LV32_ID + DPU ID + Start ICU_ID + End ICU_ID => 하위 1Byte)                                                                     


       <Rx(PC에서 상태요청에 의한 LV32 응답 데이터 구조): HOST <- LV32[Send: PV & ALARM & SV]>
        STX: 0x02 (고정 값) 
        MODE1: 0x8A (고정 값: BLOCK READ) 
        MODE2: 0x87 (고정 값: Process Value(PV)/Alarm Data/Setting Value(SV)) 
        LV32_ID: 1 ~ 32 LV32 ID 선택, 데이터 전송 시 0x80으로 |(or) 연산. 
        DPU ID: 0x9F(고정 값) 
        Start ICU Data[ID(1Byte), PV(1Byte), ALARM(1Byte), SV(1Byte)] ~ End ICU Data[ID(1Byte), PV(1Byte), ALARM(1Byte), SV(1Byte)] 
        Check Sum: STX와 ETX를 제외한 나머지 페킷의 총합의 하위 1Byte 사용함. 
        (ex: MODE1 + MODE2 + LV32_ID + DPU ID + Start ICU Data ~ + End ICU Data => 하위 1Byte)

    */
    /************************************************************************/


    /***************************************************************************/
    /* Structures & Variables                                                  */
    /***************************************************************************/

    public struct TBUFF_FFU
    {
        public int       iMode1;
        public int       iMode2;
        public int       iId   ; //LV32 ID
        public int       iDPUId;
        public int       iStart;
        public int       iEnd  ;
        public int       iCmd  ;
        public int       iPara ;
       

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TBUFF_FFU(int id)
        {
            iId = id | 0x80;

            //고정 Value
            iMode1 = 0x8A; 
            iMode2 = 0x87;
            iDPUId = 0x9F;

            iCmd   = 0;
            iPara  = 0;
            iStart = 0;
            iEnd   = 0;
        }
      
        public void ResetData()
        {
            iCmd   = 0;
            iPara  = 0;
            iStart = 0;
            iEnd   = 0;
            //iId    = 0; 
        }
    };



    /***************************************************************************/
    /* Class: TFanFilterUnint                                                  */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TFanFilterUnint
    {
        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int TX_BUFF       = 1024 ; 
        const int RX_BUFF       = 1024 ;    
        
        const int STX           = 0x02 ;    
        const int ETX           = 0x03 ;
        const int xOR           = 0x80 ;
        const int DATAOK        = 0xB9 ; //OK Data

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:    //Member Var.   
        int            m_iModel     ;
        int            m_iMaxCh     ;
        int            m_iLV32ID    ;
        int[]          m_iSV      = new int[(int)EN_FFU_CH.EndOfId]; //Set     Value.
        int[]          m_iPV      = new int[(int)EN_FFU_CH.EndOfId]; //Process Value.
        int[]          m_iSetSV   = new int[(int)EN_FFU_CH.EndOfId]; //Process Value.
        int[]          m_iAlarm   = new int[(int)EN_FFU_CH.EndOfId]; //Alarm Info.


        bool           m_bDrngComm  ; //Process Value.
        bool           m_bErrComm   ; //Communication - 통신` 에러
        int            m_iSendStep  ; //Update Step - Read Cycle.
        bool           m_bWatchOn   ; //            - Controller의 상태(PV,SV,ST)를 모니터링 할 것인지를 결정.
      //bool           m_bErrTemp   ; //

        //Buffer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        byte[]                      m_szTxBuff  = new byte[TX_BUFF];
        //public List<TBUFF_FFU>      m_CmdList   = new List<TBUFF_FFU>();
        public Queue<TBUFF_FFU>     m_CmdList   = new Queue<TBUFF_FFU>();

        public TBUFF_FFU            m_TxBuff    = new TBUFF_FFU();   

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer  m_tSendTimer  = new TOnDelayTimer();
        TOnDelayTimer  m_tSendDelay  = new TOnDelayTimer();
 

         //Object.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TSerialUnit    m_RS232;


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    _bCon        {get { return m_RS232._IsOpen; } }
        public bool    _bErr        {get { return m_bErrComm;} }
        public bool    _bWatchOn1   {get { return m_bWatchOn;  } set { m_bWatchOn = value; } }
        


        ///Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //m_pDrawImg

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TFanFilterUnint()
        {
            m_szTxBuff.MemSet(0xFF);

            //
            m_RS232            = new TSerialUnit();
            m_RS232.OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);
        }
        //------------------------------------------------------------------------
        ~TFanFilterUnint() 
        { 
            if (m_RS232 != null) { m_RS232.Port_Close(); m_RS232 = null;}
        }
        //------------------------------------------------------------------------
        private bool CheckCh(int Ch)
        {
            if (Ch <  1       ) return false;
            if (Ch >  m_iMaxCh) return false;
            //
            return true;
        }
        //------------------------------------------------------------------------
        public void SetSV(int Ch , int Val)
        {
            m_iSetSV[Ch] = Val;
        }
        //------------------------------------------------------------------------
        public int GetSV(int Ch)
        {
            //
            if (!CheckCh(Ch)) return 0;
            return m_iSV[Ch];
        }
        //------------------------------------------------------------------------
        public int GetPV(int Ch)
        {
            //
            if (!CheckCh(Ch)) return 0;
            return m_iPV[Ch];
        }
        //------------------------------------------------------------------------
        public int GetAlarm(int Ch)
        {
            //
            if (!CheckCh(Ch)) return 0;

            return m_iAlarm[Ch];
        }
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init (string sPortNo="COM1")
        {
            //Data Bits    : 8Bit 
            //Parity       : None
            //Stop Bit     : 1 Stop Bit
            //Baud rate    : 9,600 bps
            //Flow Control : None
            m_RS232.Open(sPortNo, 9600, 8, Parity.None, StopBits.One );
            
            if (!m_RS232._IsOpen)
            {
                MsgBox.Error($"[FFU] COM Port[{sPortNo}] Open Fail");
                return; 
            }

            //
            m_iLV32ID = 1; //
            m_iMaxCh  = 2; 

            //Clear Queue.
            m_CmdList.Clear  ();

            //Var.
            m_bWatchOn    = true;

        }       
        //------------------------------------------------------------------------
        public void Reset()
        {
            m_CmdList   .Clear();
            m_tSendTimer.Clear();  
            m_bDrngComm = false;  
            m_iSendStep = 0; 

        }       
        //------------------------------------------------------------------------
        public void Close()
        { 
            if (m_RS232 != null) { m_RS232.Port_Close(); m_RS232 = null; }
        }
        //------------------------------------------------------------------------
        bool GetErrComm() { return m_bErrComm; }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Interface.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool SndMsg(TBUFF_FFU TxBuff)
        {
            //Local Var.
            int iTxLen = 0;
            
            //Set Request Code.
            switch (TxBuff.iCmd) 
            {
                case (int)EN_FFU_CMD.GetPV     : iTxLen = m_MakeMsgGetPV  (TxBuff); break;

                case (int)EN_FFU_CMD.SetSV     : iTxLen = m_MakeMsgSetSV  (TxBuff); break;
            }

            //Check Port.
            if (!m_RS232._IsOpen) return false;
            if (iTxLen <= 0     ) return false;
            
            m_bDrngComm = true;
            
            //Write Data./
            bool bRet = m_RS232.SendByte(m_szTxBuff,iTxLen);

            string sTx = FNC.GetByteArrayToHexString(m_szTxBuff, 0, iTxLen);
            cDEF.LOG.RS232Trace("[SND]" + sTx);

            //Return.
            return bRet;
        }

        //------------------------------------------------------------------------
        void OnRecive(object sender, int len, byte[] data)
        {
            /************************************************************************/
            /*   STX: 0x02 (고정 값) 
                 MODE1: 0x8A (고정 값: BLOCK READ) 
                 MODE2: 0x87 (고정 값: Process Value(PV)/Alarm Data/Setting Value(SV)) 
                 LV32_ID: 1 ~ 32 LV32 ID 선택, 데이터 전송 시 0x80으로 |(or) 연산. 
                 DPU ID: 0x9F(고정 값) 
                 Start ICU Data[ID(1Byte), PV(1Byte), ALARM(1Byte), SV(1Byte)] ~ End ICU Data[ID(1Byte), PV(1Byte), ALARM(1Byte), SV(1Byte)] 
                 Check Sum: STX와 ETX를 제외한 나머지 페킷의 총합의 하위 1Byte 사용함. 
                  (ex: MODE1 + MODE2 + LV32_ID + DPU ID + Start ICU Data ~ + End ICU Data => 하위 1Byte)                                                                      */
            /************************************************************************/

            //Local Var.
            //uint getCRC = 0;
            bool bSetPV  = (Convert.ToInt32(data[2]) == 0x89) && (Convert.ToInt32(data[3]) == 0x84);
            int iLVID    = (Convert.ToInt32(data[4]) & 0xF) ;
            int iCh      = (Convert.ToInt32(data[5]) & 0xF) ;

            //Check.
            m_bDrngComm = false;
            m_bErrComm  = false;
            
            //string sData = Encoding.ASCII.GetString(data);
            string sData = FNC.GetByteArrayToHexString(m_szTxBuff, 0, len);
            cDEF.LOG.RS232Trace("[RCV]" + sData);

            //
            if (iLVID < 1 || iLVID > 32       ) { m_TxBuff.ResetData();return;}
            if (iCh   < 1 || iCh   > m_iMaxCh ) { m_TxBuff.ResetData();return;}

            if(m_TxBuff.iCmd == (int)EN_FFU_CMD.GetPV)
            {
                //
                int iPV    = Convert.ToInt32(data[6]) * 10 ; 
                int iAlarm = Convert.ToInt32(data[7]) & 0xF; 
                int iSV    = Convert.ToInt32(data[8]) * 10 ;

                m_iPV   [iCh-1] = iPV   ; 
                m_iAlarm[iCh-1] = iAlarm; 
                m_iSV   [iCh-1] = iSV   ;

            }
            else if(m_TxBuff.iCmd == (int)EN_FFU_CMD.SetSV)
            {
                int iOK = Convert.ToInt32(data[7]);
                if(iOK != DATAOK)
                {
                    //Error??
                }
            }

            //
            m_TxBuff.ResetData();

        }
        //------------------------------------------------------------------------
        private int ChekSum(TBUFF_FFU data)
        {
            int  nChksum = 0;
            byte byteOr  = 0xFF;
            bool bSetPV = data.iMode1 == 0x89;

            int nTotal = data.iMode1 + data.iMode2 + data.iId + data.iDPUId + 
                         data.iStart + 
                         data.iEnd   + 
                         data.iPara  ;

            //if (bSetPV) nTotal += data.iPara;

            nChksum = nTotal & byteOr;

            return nChksum; 
        }
        //Make send message.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int m_MakeMsgGetPV(TBUFF_FFU buff)
        {
            //STX MODE1 MODE2 LV32_ID DPU ID Start ICU_ID End ICU_ID Check_Sum ETX
            //예) HOST에서 LV32_ID가 1이고 ICU 1번부터 3번까지 Process Value & ALARM & Setting Value를 요청.
            //(1) Tx: HOST -> LV32(Ask PV & ALARM & SV)
            //0x02 0x8A 0x87 0x81 0x9F 0x81 0x83 0x35 0x03

            //Local Var.
            int  iLen     = 0;
            int  iLVID    = buff.iId    | xOR; 
          //int  iLVID    = 1           | xOR; 
            int  iStartId = buff.iStart | xOR; 
            int  iEndId   = buff.iEnd   | xOR; 

                   AttatchData(Convert.ToByte(STX           ));   //STX
                   AttatchData(Convert.ToByte(buff.iMode1   ));   //MODE1
                   AttatchData(Convert.ToByte(buff.iMode2   ));   //MODE2 - (고정 값: Process Value(PV)/Alarm Data/Setting Value(SV))
                   AttatchData(Convert.ToByte(iLVID         ));   //LV32_ID
                   AttatchData(Convert.ToByte(buff.iDPUId   ));   //DUP_ID(고정)
                   
                   AttatchData(Convert.ToByte(iStartId      ));   //Start ICU_ID
                   AttatchData(Convert.ToByte(iEndId        ));   //End ICU_ID
                   
                   AttatchData(Convert.ToByte(ChekSum(buff) ));   //CheckSum
            iLen = AttatchData(Convert.ToByte(ETX           ));   //STX

            return iLen;
        }
        //------------------------------------------------------------------------
        int m_MakeMsgSetSV(TBUFF_FFU buff)
        {
            //Tx: HOST->LV32(Unit Command RPM)
            /************************************************************************/
            /*  LV32_ID      : 1 ~ 32 LV32 ID 선택, 데이터 전송 시 0x80으로 |(or) 연산. 
                Start ICU_ID : 1 ~ 32 ICU ID 선택, 데이터 전송 시 0x80으로 |(or) 연산(Start ID는 End ID보다 작거나 같아야 함). 
                End ICU_ID   : 1 ~ 32 ICU ID 선택, 데이터 전송 시 0x80으로 |(or) 연산(End ID는 Start ID보다 크거나 같아야 함). 
                SV           : 0 ~ 140 (ex: 1 -> 10 rpm, 100 -> 1000 rpm)                    
            /************************************************************************/

            //Local Var.
            int  iLen     = 0;
            int  iStartId = buff.iStart | xOR; 
            int  iEndId   = buff.iEnd   | xOR;
            int  iSV      = buff.iPara  ;

                   AttatchData(Convert.ToByte(STX           ));   //STX
                   AttatchData(Convert.ToByte(buff.iMode1   ));   //MODE1
                   AttatchData(Convert.ToByte(buff.iMode2   ));   //MODE2 - (고정 값: Process Value(PV)/Alarm Data/Setting Value(SV))
                   AttatchData(Convert.ToByte(buff.iId      ));   //LV32_ID
                   AttatchData(Convert.ToByte(buff.iDPUId   ));   //DUP_ID(고정)
                   
                   AttatchData(Convert.ToByte(iStartId      ));   //Start ICU_ID
                   AttatchData(Convert.ToByte(iEndId        ));   //End ICU_ID
                   AttatchData(Convert.ToByte(iSV           ));   //SV
                   
                   AttatchData(Convert.ToByte(ChekSum(buff) ));   //CheckSum
            iLen = AttatchData(Convert.ToByte(ETX           ));   //STX

            return iLen ;
        }
        //---------------------------------------------------------------------------
        private int AttatchData(byte[] Data , int Cnt)
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
        private int AttatchData(byte Data)
        {
            //Local Var.
            int iLast     = 0;
            byte byteNull = 0xFF;
            iLast = Array.IndexOf(m_szTxBuff, byteNull);

            //Check Max.
            if ((iLast + 1) >= 128) 
            {
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
        public void CmdSetSV(int Ch, int Val)
        {
            if (!CheckCh(Ch)) return;
            //           
            m_iSetSV[Ch-1]     = Val;
            
            //
            TBUFF_FFU  m_TmpBuff  = new TBUFF_FFU(m_iLV32ID);
            
            m_TmpBuff.iCmd   = (int)EN_FFU_CMD.SetSV;
            m_TmpBuff.iMode1 = 0x89; //Send
            m_TmpBuff.iMode2 = 0x84;
            m_TmpBuff.iStart = Ch; 
            m_TmpBuff.iEnd   = Ch; 
            m_TmpBuff.iPara  = Val / 10;

            m_CmdList.Enqueue(m_TmpBuff); 
        }     
        //------------------------------------------------------------------------
        public void CmdSetSV_Multi(int start, int end, int Val)
        {
            if (!CheckCh(start)) return;
            if ( start > end   ) return; 
            
            //           
            for (int n = start; n < end; n++)
            {
                m_iSetSV[n-1] = Val;
            }
            
            //
            TBUFF_FFU  m_TmpBuff  = new TBUFF_FFU(m_iLV32ID);
            
            m_TmpBuff.iCmd   = (int)EN_FFU_CMD.SetSV;
            m_TmpBuff.iMode1 = 0x89; //Send
            m_TmpBuff.iMode2 = 0x84;
            m_TmpBuff.iStart = start; 
            m_TmpBuff.iEnd   = end  ; 
            m_TmpBuff.iPara  = Val / 10;

            m_CmdList.Enqueue(m_TmpBuff); 
        }     

        //------------------------------------------------------------------------
        public void CmdGetPV(int Ch)
        {
            if (!CheckCh(Ch)) return;

            //
            TBUFF_FFU m_TmpBuff  = new TBUFF_FFU(m_iLV32ID);

            m_TmpBuff.iCmd   = (int)EN_FFU_CMD.GetPV;
            m_TmpBuff.iStart = Ch; 
            m_TmpBuff.iEnd   = Ch;

            m_CmdList.Enqueue(m_TmpBuff); 
        }
        //------------------------------------------------------------------------
        public void CmdGetPV_Multi(int start, int end)
        {
            //
            TBUFF_FFU m_TmpBuff = new TBUFF_FFU(m_iLV32ID);
            m_TmpBuff.iCmd   = (int)EN_FFU_CMD.GetPV;
            m_TmpBuff.iStart = start; 
            m_TmpBuff.iEnd   = end  ;

            m_CmdList.Enqueue(m_TmpBuff);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
            if (m_tSendTimer.OnDelay((m_iSendStep != 0 || m_bDrngComm), 5000)) 
            {
                Reset();  
                m_bErrComm = true;
            }

            try 
            { 
                //Message Process..
                switch (m_iSendStep) 
                {
                    case  0: if ( m_CmdList.Count == 0)  { m_iSendStep = 0 ; return; }
                         m_iSendStep ++;
                         break;

                    case 1:
                        if (m_CmdList.Count < 1) { m_iSendStep = 0; return; }

                        m_TxBuff = m_CmdList.Dequeue();

                        m_iSendStep++;
                        return;

                    case 2: 
                        if (!SndMsg((m_TxBuff))) return;
                        m_szTxBuff.MemSet(0xFF);

                        m_tSendDelay.Clear();   
                        m_iSendStep ++;
                        return;


                    case 3: 
                        if(!m_tSendDelay.OnDelay(true, 50)) return;
                        if(m_bDrngComm                    ) return;   
                        
                        m_tSendTimer.Clear(); //Clear Timer.
                        m_bErrComm = false ;
                        m_iSendStep = 0;
                        return;
                }
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("[FFU]UpdateMsg. Update " + ex.ToString());
                
            } 
        }        
        //------------------------------------------------------------------------
        public void Update      ()
        {
            //Update.
            if(m_bWatchOn && CanSendMsg()) 
            {
                //for (int i = 0; i < (int)EN_FFU_CH.EndOfId; i++)
                //{
                //    for (int j = 0; j < m_iMaxCh; j++)
                //    {
                //        //Set SV
                //        if (m_iSetSV[j] != m_iSV[j]) CmdSetSV(j+1, m_iSetSV[j]);
                //        else                         CmdGetPV(j+1);
                //    }
                //}
            }
            //
            UpdateMsg    ();
        }     
    }
}
