using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace eMachine
{

    /***************************************************************************/
    /* Structures & Variables                                                  */
    /***************************************************************************/
    public enum EN_CON_TYPE : int
    {
        Speed = 0,
        Torque   ,

        EndOfId 
    };

 
    public enum EN_TOQ_MOTN : int
    {
        STOP  = 0,
        RUN_CW   ,
        RUN_CCW  , 
        BREAK    , 
        RESET    ,
    
        EndOfId 
    };
    public enum EN_CMD_TORQUE : int
    {
        None    = 0,
        wTorque = 1, //Write- 속도/토크지령 (구동속도rmp or 최대 100 토크설정)
        rTorque    , //Read - 현재 RPM(회전속도 or 모터 토크)
        rState     , //Read - controller 상태 읽기
        rCurrent   , //Read - 연속실효 전류값을 0.1A 단위로 표시
        wAction    , //모터의 구동 관련 파라메타

        EndOfId 
    };
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public struct BUFF_TOURQUE
    {
        public EN_CMD_TORQUE iCmd       ;
        public int           nSlave     ;
        public int           nFuction   ;
        public int           nStartAdd_H;
        public int           nStartAdd_L;
        public int           nNOP_H     ;
        public int           nNOP_L     ;
        public int           nDATA1     ;
        public int           nDATA2     ;
        

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public BUFF_TOURQUE(int n)
        {
            iCmd        = EN_CMD_TORQUE.None;
            nSlave      = 0;
            nFuction    = 0;
            nStartAdd_H = 0;
            nStartAdd_L = 0;
            nNOP_H      = 0;
            nNOP_L      = 0;
            nDATA1      = 0;
            nDATA2      = 0;
        }
        public void ResetData()
        {
            iCmd        = EN_CMD_TORQUE.None;
            nSlave      = 0;
            nFuction    = 0;
            nStartAdd_H = 0;
            nStartAdd_L = 0;
            nNOP_H      = 0;
            nNOP_L      = 0;
            nDATA1      = 0;
            nDATA2      = 0;
        }
    };


    /***************************************************************************/
    /* Class: Torque Controller(DX3000)                                        */
    /* Create:                                                                 */
    /* Developer: JUNG                                                         */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TTorqueDKM
    {
        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int OFFSET        = 23   ;
        const int TX_BUFF       = 1024 ; 
        const int RX_BUFF       = 1024 ;    
        
        const int FC_READ       = 0x04 ;    
        const int FC_WRITE      = 0x06 ;

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:    //Member Var.   
        int         m_nSlaveNo   ; //Slave No
        int         m_nState     ;  //
        int         m_nRPM       ; 
        int         m_nTorque    ; 
        int         m_nCurrent   ;  //
        int         m_nSetAction ; 
        int         m_nSetTouque ; 
        EN_CON_TYPE m_nConType   ;  //Control Type
        
        //
        bool          m_bDrngComm  ; //Process Value.
        bool          m_bErrComm   ; //Communication - 통신 에러
        int           m_iSendStep  ; //Update Step - Read Cycle.
        bool          m_bWatchOn   ; //            - Controller의 상태(PV,SV,ST)를 모니터링 할 것인지를 결정.
        //bool           m_bErrTemp   ; //

        EN_CMD_TORQUE m_iLastCmd ;
        int           m_nLastVal ;

        //Buffer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        byte[]                       m_szTxBuff  = new byte[TX_BUFF];
        private Queue<BUFF_TOURQUE>  m_CmdList   = new Queue<BUFF_TOURQUE>();
        private BUFF_TOURQUE         m_TxBuff    = new BUFF_TOURQUE();   

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer  m_tSendTimer  = new TOnDelayTimer();
        TOnDelayTimer  m_tSendDelay  = new TOnDelayTimer();
        TOnDelayTimer  m_tRplyDelay  = new TOnDelayTimer();
        TOnDelayTimer  m_tWaitDelay  = new TOnDelayTimer();


        //Object.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TSerialUnit    m_RS485;


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    _bErr       {get { return m_bErrComm;} }
        public bool    _bWatchOn   {get { return m_bWatchOn;        } set { m_bWatchOn = value;        } }
                                  
        public int     GetTorque   () => m_nTorque   ;
        public int     GetSetTorque() => m_nSetTouque;
        
        public int     GetRPM      () => m_nRPM      ;
        public int     GetCurrent  () => m_nCurrent  ;

        public int     GetState    () => m_nState    ;

        


        ///Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //m_pDrawImg

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TTorqueDKM(/*int sno, EN_CON_TYPE type = EN_CON_TYPE.Torque*/)
        {
            m_szTxBuff.MemSet(0xFF);

            //
            m_RS485 = new TSerialUnit();
            m_RS485.OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);

            m_nSlaveNo    = 1   ;
            m_nConType    = EN_CON_TYPE.Torque ; 
            m_iLastCmd    = EN_CMD_TORQUE.None;
            m_nLastVal    = -1    ;
            
            m_nState      = -1    ;
            m_nRPM        = -1    ;
            m_nTorque     = -1    ;
            m_nCurrent    = -1    ;
            m_nSetAction  = -1    ;
            m_nSetTouque  = -1    ;
        }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init ( string sPortNo)
        {
            //
            //Clear Queue.
            m_CmdList.Clear  (   );
            
            //
            m_RS485.Open(sPortNo, 115200, 8, Parity.None, StopBits.One);
            if (!m_RS485._IsOpen)
            {
                MsgBox.Error($"[Torque Controller] COM Port[{sPortNo}] Open Fail");
                return; 
            }                       

            //Var.
            m_bWatchOn    = true;

        }       
        //------------------------------------------------------------------------
        public void Reset()
        {
            //m_CmdList   .Clear();
            m_tSendTimer.Clear();  
            m_bDrngComm  = false;  
            m_iSendStep  = 0;
            
            m_iLastCmd    = EN_CMD_TORQUE.None;
            m_nLastVal    = -1;
            m_nState      = -1    ;
            m_nRPM        = -1    ;
            m_nTorque     = -1    ;
            m_nCurrent    = -1    ;
            m_nSetAction  = -1    ;
            m_nSetTouque  = -1    ;
           
        }
        //------------------------------------------------------------------------
        public void Close()
        {
            if (m_RS485 != null) { m_RS485.Port_Close(); m_RS485 = null; }
        }   
        //------------------------------------------------------------------------
        bool GetErrComm() { return m_bErrComm; }

        //Interface.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool SndMsg(BUFF_TOURQUE TxBuff)
        {
            //Local Var.
            int iTxLen = 0;

            //
            if (TxBuff.nSlave < 1) return false; 

            //Set Request Code.
            switch (TxBuff.iCmd)
            {
                case EN_CMD_TORQUE.wTorque : break;
                case EN_CMD_TORQUE.rTorque : break;
                case EN_CMD_TORQUE.rState  : break;
                case EN_CMD_TORQUE.rCurrent: break;
                case EN_CMD_TORQUE.wAction : break;
            }

            //
            iTxLen = m_MakeSendMsg(TxBuff);

            //Check Port.
            if (!m_RS485._IsOpen) return false;
            if (iTxLen <= 0     ) return false;
            
            m_bDrngComm = true;
            
            //Write Data./
            bool bRet = m_RS485.SendByte(m_szTxBuff,iTxLen);

            if (bRet) m_iLastCmd = TxBuff.iCmd;
            else      m_iLastCmd = EN_CMD_TORQUE.None;

            //Return.
            return bRet;
        }

        //------------------------------------------------------------------------
        void OnRecive(object sender, int len, byte[] data)
        {
            //Local Var.
            EN_CMD_TORQUE CmdType  = m_TxBuff.iCmd;
            int           nRcvAddr = Convert.ToInt32(data[0]);
            int           nByteCnt = Convert.ToInt32(data[2]);
            int           nDataHi, nDataLo ;

            //Check.
            m_bDrngComm = false;
            m_bErrComm  = false;
            
            //
            m_TxBuff.ResetData();

            //
            if (data[1] == FC_READ) //0x04 --> 7byte
            {
                if (len != 7) return; 

                nDataHi = data[3];
                nDataLo = (nDataHi << 8) | data[4];

                switch (CmdType)
                {
                    case EN_CMD_TORQUE.rTorque    :
                        m_nTorque = nDataLo;
                        break;

                    case EN_CMD_TORQUE.rState  :
                        m_nState = nDataLo;
                        break;

                    case EN_CMD_TORQUE.rCurrent:
                        m_nCurrent = nDataLo;
                        break;

                    default                  :
                        break;
                }
            }
            else if (data[1] == FC_WRITE) //8byte
            {
                if (len < 8) return;

                nDataHi = data[4];
                nDataLo = (nDataHi << 8) | data[5];

                switch (CmdType)
                {
                    case EN_CMD_TORQUE.wTorque :
                        m_nSetTouque = nDataLo;
                        break;
                    case EN_CMD_TORQUE.wAction :
                        m_nSetAction = nDataLo;
                        break;
                    default:
                        break;
                }
            }
        }

        //Make send message.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private int m_MakeSendMsg(BUFF_TOURQUE TxBuff)
        {
            //
            if (TxBuff.iCmd == EN_CMD_TORQUE.None) return 0;

            //Local Var.
            uint getCRC  = 0;
            int  iLen    = 0;

                   AttatchData(Convert.ToByte(TxBuff.nSlave     ));  //
                   AttatchData(Convert.ToByte(TxBuff.nFuction   ));  //Function
                   AttatchData(Convert.ToByte(TxBuff.nStartAdd_H));  //Start Address HI
                   AttatchData(Convert.ToByte(TxBuff.nStartAdd_L));  //Start Address LO
                   AttatchData(Convert.ToByte(TxBuff.nNOP_H     ));  //데이타 Data 개수 HI
            iLen = AttatchData(Convert.ToByte(TxBuff.nNOP_L     ));  //데이타 Data 개수 LO
            getCRC = CRC16(m_szTxBuff, iLen);
            
            AttatchData(Convert.ToByte(getCRC%256));
            AttatchData(Convert.ToByte(getCRC/256));
            return iLen + 2;
        }
        
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Command
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void CmdReadState()
        {//현재 상태 Read
            //Var.
            BUFF_TOURQUE TmpBuff = new BUFF_TOURQUE(0);

            TmpBuff.iCmd        = EN_CMD_TORQUE.rState   ;
            TmpBuff.nSlave      = m_nSlaveNo             ;
            TmpBuff.nFuction    = FC_READ                ;
            TmpBuff.nStartAdd_H = 0                      ;
            TmpBuff.nStartAdd_L = 0x03                   ;
            TmpBuff.nNOP_H      = 0                      ;
            TmpBuff.nNOP_L      = 0x01                   ;

            m_CmdList.Enqueue(TmpBuff);

        }
        //------------------------------------------------------------------------
        public void CmdReadTorque()
        {//현재 상태 Read
            //Var.
            BUFF_TOURQUE TmpBuff = new BUFF_TOURQUE(0);

            TmpBuff.iCmd        = EN_CMD_TORQUE.rTorque  ;
            TmpBuff.nSlave      = m_nSlaveNo             ;
            TmpBuff.nFuction    = FC_READ                ;
            TmpBuff.nStartAdd_H = 0                      ;
            TmpBuff.nStartAdd_L = 0x02                   ;
            TmpBuff.nNOP_H      = 0                      ;
            TmpBuff.nNOP_L      = 0x01                   ;

            m_CmdList.Enqueue(TmpBuff);

        }
        //------------------------------------------------------------------------
        public void CmdReadCurrent()
        {//현재 상태 Read
            //Var.
            BUFF_TOURQUE TmpBuff = new BUFF_TOURQUE(0);

            TmpBuff.iCmd        = EN_CMD_TORQUE.rCurrent ;
            TmpBuff.nSlave      = m_nSlaveNo             ;
            TmpBuff.nFuction    = FC_READ                ;
            TmpBuff.nStartAdd_H = 0                      ;
            TmpBuff.nStartAdd_L = 0x04                   ;
            TmpBuff.nNOP_H      = 0                      ;
            TmpBuff.nNOP_L      = 0x01                   ;

            m_CmdList.Enqueue(TmpBuff);

        }

        //------------------------------------------------------------------------
        public bool CmdWriteTorque(int val)
        {//Speed or Torque 

            if (cDEF.FM.SysOptn.bSimulRun) return true;

            if (m_iLastCmd == EN_CMD_TORQUE.wTorque)
            {
                if (!m_tRplyDelay.OnDelay(true, 300)) return false;
                if ( m_nTorque     == val           ) return true ;
            }

            //
            m_nLastVal = val;

            //Var.
            BUFF_TOURQUE TmpBuff = new BUFF_TOURQUE(0);

            TmpBuff.iCmd        = EN_CMD_TORQUE.wTorque;
            TmpBuff.nSlave      = m_nSlaveNo           ;
            TmpBuff.nFuction    = FC_WRITE             ;
            TmpBuff.nStartAdd_H = 0                    ;
            TmpBuff.nStartAdd_L = 0x01                 ;
            TmpBuff.nNOP_H      = val >> 8             ;
            TmpBuff.nNOP_L      = val & 255            ;

            m_CmdList.Enqueue(TmpBuff);
            m_tRplyDelay.Clear();

            return false;
        }
        //------------------------------------------------------------------------
        public void CmdWriteRun(EN_TOQ_MOTN go = EN_TOQ_MOTN.STOP)
        {//
            //Var.
            BUFF_TOURQUE TmpBuff = new BUFF_TOURQUE(0);
            int cmdtype = 1;

            switch (go)
            {
                case EN_TOQ_MOTN.STOP   : cmdtype = 1; break;
                case EN_TOQ_MOTN.RUN_CW : cmdtype = 2; break;
                case EN_TOQ_MOTN.RUN_CCW: cmdtype = 4; break;
                case EN_TOQ_MOTN.BREAK  : cmdtype = 6; break;
                case EN_TOQ_MOTN.RESET  : cmdtype = 8; break;
                default                 : cmdtype = 1; break;
            }

            TmpBuff.iCmd        = EN_CMD_TORQUE.wAction;
            TmpBuff.nSlave      = m_nSlaveNo           ;
            TmpBuff.nFuction    = FC_WRITE             ;
            TmpBuff.nStartAdd_H = 0                    ;
            TmpBuff.nStartAdd_L = 0x05                 ;
            TmpBuff.nNOP_H      = 0                    ;
            TmpBuff.nNOP_L      = cmdtype              ;

            m_CmdList.Enqueue(TmpBuff);
        }
        //------------------------------------------------------------------------
        private bool IsExistQue(EN_CMD_TORQUE cmd)
        {
            foreach (var list in m_CmdList)
            {
                if (list.iCmd == cmd) return true;
            }
            return false;
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

            //Attach.
            Array.Copy(Data, 0, m_szTxBuff, iLast, Cnt);

            //Ok.
            return (iLast + Cnt);
        }


        //---------------------------------------------------------------------------
        int AttatchData(byte Data)
        {
            //Local Var.
            int  iLast = 0;
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
            if (!m_RS485  ._IsOpen      ) return false;
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
            if (m_tSendTimer.OnDelay((m_iSendStep != 0 || m_bDrngComm), 1000)) 
            {
                Reset();  
                m_bErrComm = true;
            }

            try 
            { 
                //Message Process..
                switch (m_iSendStep) 
                {
                    case  0: 
                        if ( m_CmdList.Count == 0)  
                        {
                            //300ms 
                            if (!m_tWaitDelay.OnDelay(true, 300)) break;

                             CmdReadTorque();
                             CmdReadState ();

                            m_tWaitDelay.Clear();

                            m_iSendStep = 0; //
                            break; 
                        }

                        m_iSendStep ++;
                        break;

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    case 1: 
                        m_TxBuff = m_CmdList.Dequeue();
                        
                        if (!SndMsg((m_TxBuff))) break;
                        m_szTxBuff.MemSet(0xFF);

                        m_tSendDelay.Clear();   
                        m_iSendStep ++;
                        break;


                    case 2: 
                        if(!m_tSendDelay.OnDelay(true, 50)) break;
                        if( m_bDrngComm                   ) break;   
                        m_tSendTimer.Clear(); //Clear Timer.
                        m_bErrComm = false ;
                        m_iSendStep = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                //cDEF.LOG.ExceptionTrace("UpdateMsg. Update " + ex.ToString());
                Debug.WriteLine($"[TorqueDKM] Exception : {ex.Message}");
            } 
        }        
        //------------------------------------------------------------------------
        public void Update      ()
        {
            //Update.
            if (!m_bWatchOn) return;
            
            if (CanSendMsg()) 
            {
                //
            }
            
            //
            UpdateMsg  ();
        }     
    }
}
