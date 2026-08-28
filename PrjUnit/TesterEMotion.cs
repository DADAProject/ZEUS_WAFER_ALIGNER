using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace InoModule
{
    

    //Scenario
    //[Initial]
    //0.	Info	->		            설비의 상태 전송
    //1    .		<-	info	        Tester 의 Version 전송
    //2.	state	->		
    //3.	    	<-	state           Tester의 상태 전송
    //[RUN]
    //0.	model	->		            Model Id 전송
    //1.	    	<-	model           Tester에서 해당 모델 Download 후 결과 전송
    //2.	state	->		
    //3.	    	<-	state [Ready]	Tester의 상태 전송
    //4                                 (Ready 전송 받았을 경우 Start 시작)
    //6.	start	->		
    //7.		    <-	state	        Tester의 진행 상태 전송
    //8.		    <-	result	
    //9.		    <-	State[Error]	Test 문제 발생시 Error 전송
    //10	reset	->	            	state [Error] 받았을 경우 reset 전송 후 retry 진행


    public enum EN_CMD_EMOTION
    {
        // Command Define Handler -> Tester                    
        info	  ,//Tester 정보 Tester 정보 요청
        model	  ,//Model ID Handler에서 각 테스트마다 Model ID를 Tester에 전달함.
        state     ,//상태 정보 Tester 상태 정보 요청
        start	  ,//Test 시작 Tester 에 Test 시작 요청
        result	  ,//Test 결과 Tester 결과를 전송
        reset	  ,//Tester Reset	Tester Alarm Reset 요청
        EndOfCmd

    }

    public enum EN_STAT_EMOTION
    {   
        reserved   ,
        ready      ,
        measure    ,
        contact    ,
        discharge  ,
        finish     ,
        error      ,
        EndOfStat
    }


    /***************************************************************************/
    /* Class: TTesterEMotion                                                   */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TTesterEMotion                                           
    {
        private const  int m_iBufferSize = 10000;
        AsyncSocketClient Client;

                          
		bool[]            m_bRpyPass   = new bool[(int)EN_CMD_EMOTION.EndOfCmd];
		bool[]            m_bReply     = new bool[(int)EN_CMD_EMOTION.EndOfCmd];
        byte[]            m_TxByte     = new byte[m_iBufferSize];
        byte[]            m_RxByte     = new byte[m_iBufferSize];

        int               m_iTxByteCnt  ;
        int               m_iRxByteCnt  ; 

        EN_STAT_EMOTION   m_iStatus;


        private byte[]    buffer = new byte[m_iBufferSize];

        string            m_sErrString;

        //Tester Data.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool       m_bLockLog   ;
        public bool       m_bConnected ;
        public string     m_sRcvModel  ; //Tester에서 받은 Job Name
        public string     m_sRcvVersion; //Tester에서 받은 Tester Version
        public EN_JIG_ID  m_iSndJigId  ;
    

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_STAT_EMOTION _iStatus     {get { return m_iStatus;           } }
        public bool            _bConnected  {get { return m_bConnected;        } }
        public string          _sErrString  {get { return m_sErrString;        } }

        

        public TTesterEMotion()
        {
            Client             = new AsyncSocketClient(1);
            Client.OnReceive  += new AsyncSocketReceiveEventHandler(OnReceive);
            Client.OnConnet   += new AsyncSocketConnectEventHandler(OnConnet );
            Client.OnClose    += new AsyncSocketCloseEventHandler  (OnClose  );
            Client.OnError    += new AsyncSocketErrorEventHandler  (OnError  );


            m_bLockLog     = false;
            m_bConnected   = false;  

        }

        public void Close()
        {

        }

        //--------------------------------------------------------------------------
        public bool GoConnect(string sIP, int iPort)
        {  
            m_iStatus    = EN_STAT_EMOTION.reserved;
            m_iRxByteCnt = 0;

            Client.Connect(sIP, iPort);
            return true;
        }
        //--------------------------------------------------------------------------
        public bool GoClose()
        {  
            Client.Close();
            return true;
        }



        public bool IsReply(EN_CMD_EMOTION iRepCmd)
        {
            if(iRepCmd<0 || iRepCmd>=EN_CMD_EMOTION.EndOfCmd)  return false;
            return m_bReply[(int)iRepCmd];
        }

        public bool isRpyPass(EN_CMD_EMOTION iRepCmd)
        {
            if(iRepCmd<0 || iRepCmd>=EN_CMD_EMOTION.EndOfCmd)  return false;
            return m_bRpyPass[(int)iRepCmd];
        }


        public void Reset()
        {
            ClearTxData();
            ClearRxData();
        }
        //--------------------------------------------------------------------------
        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {      
            m_bConnected = false;  
            m_sErrString = e.AsyncSocketException.Message;
            MsgBox.Error(m_sErrString);
        }
        //--------------------------------------------------------------------------
        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        { 
            m_bConnected = true ;    
        }
        //--------------------------------------------------------------------------
        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {  
            m_bConnected = false;
        }
        //--------------------------------------------------------------------------
        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {

            OnReciveSocket(e.ReceiveBytes, e.ReceiveData);
        }

        public void ClearTxData()
        {
            m_iTxByteCnt = 0;
            Array.Clear(m_TxByte, 0, m_TxByte.Length);
        }
        public void ClearRxData()
        {
            m_iRxByteCnt = 0;
            Array.Clear(m_RxByte, 0, m_RxByte.Length);
        }
 
        //Recive Process
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #region [Recv Command]
        public void ClearRep(EN_CMD_EMOTION iRepCmd)
        {
            m_bRpyPass[(int)iRepCmd] = false; 
            m_bReply  [(int)iRepCmd] = false;
        }


        public void ClearRepAll()
        {
            for(int i=0;i<(int)EN_CMD_EMOTION.EndOfCmd; i++) {
                m_bRpyPass[i] = false;
                m_bReply  [i] = false;
            }

        }
      
        private bool OnCheckLength(byte[] data, int len)
        {
             bool bErrFormat  = false;
            if(data[0]     != 0x01) bErrFormat = true;
            if(data[len-1] != 0x04) bErrFormat = true;
            return bErrFormat;
        }
 
        public void OnReciveSocket(int len, byte[] data)
        {
            int iFindEnd   = -1;
            int iFindStart = -1;
            int iFindNull  = -1;
            int iDataLen   = 0 ;

            byte[] ByteBuff = new byte[m_iBufferSize];
            byte byteNull  = 0x00;
            byte byteStart = 0x23;
            byte byteEnd   = 0x0D;


            try {

                iFindNull = Array.IndexOf ( m_RxByte, byteNull );
                if (iFindNull<0 || (iFindNull + len) >= m_iBufferSize) {
                    ClearRxData ( );
                    return;
                }
                if(len>10000) 
                {
                    iFindNull = -1;
                    ClearRxData ( );
                    return;
                }

                Array.Copy ( data, 0, m_RxByte, iFindNull, len );
                m_iRxByteCnt = iFindNull + len;
                do {

                    iFindStart = Array.IndexOf ( m_RxByte, byteStart ); 
                    for(int i=iFindStart;i<(iFindNull+len);i++)
                    {
                        if (m_RxByte[i] == byteEnd) { iFindEnd = iFindStart + i; break;}   
                    }

                    if (iFindStart>=0 && iFindEnd > 0) {
                        Array.Copy ( m_RxByte, iFindStart, ByteBuff, 0, iFindEnd+1);
                        iDataLen = iFindEnd-iFindStart;
                        Array.Clear( m_RxByte, iFindStart, iDataLen+1);
                        Array.Copy ( m_RxByte, iFindEnd+1, m_RxByte, iFindStart, m_iBufferSize-(iDataLen+1));

                        ProcReciveData ( iDataLen, ByteBuff );
                    }
                } while (iFindEnd > 0);
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TTesterEMotion OnReciveSocket()" + ex.ToString());
            }
        }


        private void ProcReciveData(int len, byte[] data)
        //private void OnReciveSocket(object sender, string data)
        {
            bool bErrCmd = false;

            bErrCmd = OnCheckLength (data, len);

            string  Data   = FNC.GetByteArrayToString(data, 1, len-1).Trim(); 
            string  RcvCmd = FNC.GetByteArrayToString(data, 1, 8    ).ToLower().Trim(); 


            try {
                     if(RcvCmd.IndexOf("info"  ) >= 0) { RcvInfo	(data); }
                else if(RcvCmd.IndexOf("model" ) >= 0) { RcvModel 	(data); }
                else if(RcvCmd.IndexOf("state" ) >= 0) { RcvState 	(data); }
                else if(RcvCmd.IndexOf("start" ) >= 0) { RcvStart	(data); }
                else if(RcvCmd.IndexOf("result") >= 0) { RcvResult	(data); }
                else                                     bErrCmd = true;
            }
            catch (Exception ex) {
                cDEF.LOG.ExceptionTrace ( "TTesterEMotion ProcReciveData()" + ex.ToString ( ) );
            }
        }


        public bool RcvInfo(byte[] RcvByte)
        {
            int    iCmd     = (int)EN_CMD_EMOTION.info; 
            bool   isPass   = false;
            string StrLog;  
            int    iDatalen = FNC.GetByteArrayToInt   (RcvByte, 11, 4);
            string sRcvData = FNC.GetByteArrayToString(RcvByte, 15, iDatalen).Trim(); 
       
            m_sRcvModel     = FNC.GetStrBuffToHead("model"  ,sRcvData);
            m_sRcvVersion   = FNC.GetStrBuffToHead("ver"    ,sRcvData);
    
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(EN_CMD_EMOTION   ),iCmd       ) + " - " + sRcvData; 
            Log(false, StrLog); 

            return isPass;

        }
        public bool RcvModel(byte[] RcvByte)
        {
            int    iCmd     = (int)EN_CMD_EMOTION.model; 
            bool   isPass   = false;
            string StrLog;  
            int    iDatalen = FNC.GetByteArrayToInt   (RcvByte, 11, 4);
            string sRcvData = FNC.GetByteArrayToString(RcvByte, 15, iDatalen).Trim(); 
       
            m_sRcvModel     = sRcvData;
    
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(EN_CMD_EMOTION   ),iCmd       ) + " - " + sRcvData; 

            Log(false, StrLog); 

            return isPass;

        }
        public bool RcvState(byte[] RcvByte)
        {
            int    iCmd     = (int)EN_CMD_EMOTION.state; 
            bool   isPass   = false;
            string StrLog;  
            int    iDatalen = FNC.GetByteArrayToInt   (RcvByte, 11, 4);
            string sRcvData = FNC.GetByteArrayToString(RcvByte, 15, iDatalen).Trim(); 

            m_sRcvModel     = FNC.GetStrBuffToHead("model"  ,sRcvData);
            
            m_iStatus = EN_STAT_EMOTION.reserved;

                 if(sRcvData.IndexOf("ready"    ) > 0) m_iStatus = EN_STAT_EMOTION.ready    ;
            else if(sRcvData.IndexOf("measure"  ) > 0) m_iStatus = EN_STAT_EMOTION.measure  ;
            else if(sRcvData.IndexOf("contact"  ) > 0) m_iStatus = EN_STAT_EMOTION.contact  ;
            else if(sRcvData.IndexOf("discharge") > 0) m_iStatus = EN_STAT_EMOTION.discharge;
            else if(sRcvData.IndexOf("error"    ) > 0) m_iStatus = EN_STAT_EMOTION.error    ;
                                      
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(EN_CMD_EMOTION   ),iCmd       ) + " - " + sRcvData; 

            Log(false, StrLog); 

            return isPass;

        }


        public bool RcvStart(byte[] RcvByte)
        {
            int    iCmd     = (int)EN_CMD_EMOTION.start; 
            bool   isPass   = false;
            string StrLog;  
            int    iDatalen = FNC.GetByteArrayToInt   (RcvByte, 11, 4);
            string sRcvData = FNC.GetByteArrayToString(RcvByte, 15, iDatalen).Trim(); 
       
            m_sRcvModel     = sRcvData;
    
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(EN_CMD_EMOTION   ),iCmd       ) + " - " + sRcvData; 

            Log(false, StrLog); 

            return isPass;

        }


        public bool RcvResult(byte[] RcvByte)
        {
            int    iCmd     = (int)EN_CMD_EMOTION.model; 
            bool   isPass   = false;
            string StrLog;  
            int    iDatalen = FNC.GetByteArrayToInt   (RcvByte, 11, 4);
            string sRcvData = FNC.GetByteArrayToString(RcvByte, 15, iDatalen).Trim(); 

            string sRslttS1 = FNC.GetStrBuffToHead("1",sRcvData);
            string sRslttS2 = FNC.GetStrBuffToHead("2",sRcvData);
            string sRslttS3 = FNC.GetStrBuffToHead("3",sRcvData);
            string sRslttS4 = FNC.GetStrBuffToHead("4",sRcvData);

            SeprTestData(0,sRslttS1);
            SeprTestData(1,sRslttS2);
            SeprTestData(2,sRslttS3);
            SeprTestData(3,sRslttS4);


            //결과 받자 마자 혹시 몰라서 상태 변경 
            m_iStatus = EN_STAT_EMOTION.ready;

            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(EN_CMD_EMOTION   ),iCmd       ) + " - " + sRcvData; 

            Log(false, StrLog); 

            return isPass;
        }

        public void SeprTestData(int iSlot, string sTestData)
        {
           //‘M’ Mask
           //‘G’ Good
           //‘N’ Ng
           //‘O’ Open
           //‘S’ Short

            string Temp;
            int    N;
            int    iJig  = (int)m_iSndJigId; //Start시 Tester에 요청한 Jig ID
           
            if(m_iSndJigId<0 || m_iSndJigId>=EN_JIG_ID.EndOfId) return;

            for (int r = 0 ; r < vDEF.MAX_JIG_R ; r++) {
                for (int c = 0 ; c < vDEF.MAX_JIG_C ; c++) {
                    N = r * vDEF.MAX_JIG_C + c;
                    Temp = sTestData.Substring(N, 1);
                    if(N >= sTestData.Length) continue;
                    
                    cDEF.DM.JIG[iJig][iSlot, r,c].SetTo(EN_CHIP_STAT.Rslt, EN_CHIP_RSLT.Unknown);
                    if(Temp == "M" ) cDEF.DM.JIG[iJig][iSlot, r,c].SetTo(EN_CHIP_STAT.Rslt, EN_CHIP_RSLT.Mask );
                    if(Temp == "G" ) cDEF.DM.JIG[iJig][iSlot, r,c].SetTo(EN_CHIP_STAT.Rslt, EN_CHIP_RSLT.Good );
                    if(Temp == "N" ) cDEF.DM.JIG[iJig][iSlot, r,c].SetTo(EN_CHIP_STAT.Rslt, EN_CHIP_RSLT.Fail );
                    if(Temp == "O" ) cDEF.DM.JIG[iJig][iSlot, r,c].SetTo(EN_CHIP_STAT.Rslt, EN_CHIP_RSLT.Open );
                    if(Temp == "S" ) cDEF.DM.JIG[iJig][iSlot, r,c].SetTo(EN_CHIP_STAT.Rslt, EN_CHIP_RSLT.Short);

                }
		    }

        }

        #endregion

        //Send Process
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #region [Send Command]
        public void AddTxByte(byte  Data)
        {
            m_TxByte[m_iTxByteCnt] = Data;
            m_iTxByteCnt ++;
        }

        public void AddTxData(byte[] Data)
        {
            Array.Copy(Data, 0, m_TxByte, m_iTxByteCnt, Data.Length);
            m_iTxByteCnt += Data.Length;
        }

        public void AddStrTxData(string Data, int iLen = -1)
        {

            if(iLen <0) iLen = Data.Length;
            if(iLen<=0) iLen = 1;
            
            byte[] ByteData  = new byte[iLen];
            ByteData         = FNC.GetStringToByteArray(Data,iLen);


            Array.Copy(ByteData, 0, m_TxByte, m_iTxByteCnt, ByteData.Length);
            m_iTxByteCnt += ByteData.Length;
        }


        public string GetHandlerStat()
        {
            string StrStat = "stop";

                 if (cDEF.SEQ._iSeqStat == EN_SEQ_STAT.Error  ) StrStat = "error";
            else if (cDEF.SEQ._iSeqStat == EN_SEQ_STAT.Idle   ) StrStat = "idle" ;
            else if (cDEF.SEQ._iSeqStat == EN_SEQ_STAT.Running) StrStat = "run"  ;
            else if (cDEF.SEQ._iSeqStat == EN_SEQ_STAT.RunWarn) StrStat = "run"  ;

            return StrStat;
        }

        public void SendInfo(int iMcNo)
        {
            string strData         = GetHandlerStat();
            string strcmd          = Enum.GetName(typeof(EN_CMD_EMOTION), EN_CMD_EMOTION.info);
            string strMcId         = string.Format("{0:00}"  , iMcNo);
            string strDataLen      = string.Format("{0:0000}", strData.Length);

            ClearTxData  ();
            AddStrTxData (strcmd    , 8);
            AddStrTxData (strMcId   , 2);
            AddStrTxData (strDataLen, 4);
            AddStrTxData (strData   , strData.Length);

            Log(true, strcmd); 
            Send();
        }

        public void SendModel(int iMcNo, string ModelName)
        {
            string strData         = ModelName;
            string strcmd          = Enum.GetName(typeof(EN_CMD_EMOTION), EN_CMD_EMOTION.model);
            string strMcId         = string.Format("{0:00}"  , iMcNo);
            string strDataLen      = string.Format("{0:0000}", strData.Length);

            ClearTxData  ();
            AddStrTxData (strcmd    , 8);
            AddStrTxData (strMcId   , 2);
            AddStrTxData (strDataLen, 4);
            AddStrTxData (strData   , strData.Length);

            Log(true, strcmd); 
            Send();
        }

        public void SendState(int iMcNo)
        {
            string strData         = GetHandlerStat();
            string strcmd          = Enum.GetName(typeof(EN_CMD_EMOTION), EN_CMD_EMOTION.model);
            string strMcId         = string.Format("{0:00}"  , iMcNo);
            string strDataLen      = string.Format("{0:0000}", strData.Length);

            ClearTxData  ();
            AddStrTxData (strcmd    , 8);
            AddStrTxData (strMcId   , 2);
            AddStrTxData (strDataLen, 4);
            AddStrTxData (strData   , strData.Length);

            Log(true, strcmd); 
            Send();
        }
        public string GetSlotData(EN_JIG_ID iJig, int iSlot)
        {
            string sData = string.Format("{0}="  , iSlot+1);
            string sStat = "";

            if(iJig<0 || iJig>=EN_JIG_ID.EndOfId) return " ";

            for (int r = 0 ; r < vDEF.MAX_JIG_R ; r++) {
                for (int c = 0 ; c < vDEF.MAX_JIG_C ; c++) {
                    sStat = "N"; 
                    if(cDEF.DM.JIG[(int)iJig][iSlot, r,c]._iStat == EN_CHIP_STAT.Mask ) sStat = "M";
                    if(cDEF.DM.JIG[(int)iJig][iSlot, r,c]._iStat == EN_CHIP_STAT.Mount) sStat = "G";

                    sData += sStat;
                }
		    }
            
            return sData;
        }


        public void SendStart(int iMcNo, EN_JIG_ID iJig)
        {
            string strData         = "";

            strData += GetSlotData(iJig, 0) + " ";
            strData += GetSlotData(iJig, 1) + " ";
            strData += GetSlotData(iJig, 2) + " ";
            strData += GetSlotData(iJig, 3) ;

            string strcmd          = Enum.GetName(typeof(EN_CMD_EMOTION), EN_CMD_EMOTION.reset);
            string strMcId         = string.Format("{0:00}"  , iMcNo);
            string strDataLen      = string.Format("{0:0000}", strData.Length);


            ClearTxData  ();
            AddStrTxData (strcmd    , 8);
            AddStrTxData (strMcId   , 2);
            AddStrTxData (strDataLen, 4);
            AddStrTxData (strData   , strData.Length);

            Log(true, strcmd); 
            Send();

            m_iSndJigId = iJig; //Tester 에 요청한 JigID
        }


        public void SendResult(int iMcNo)
        {
            string strData         = GetHandlerStat();
            string strcmd          = Enum.GetName(typeof(EN_CMD_EMOTION), EN_CMD_EMOTION.result);
            string strMcId         = string.Format("{0:00}"  , iMcNo);
            string strDataLen      = string.Format("{0:0000}", strData.Length);

            ClearTxData  ();
            AddStrTxData (strcmd    , 8);
            AddStrTxData (strMcId   , 2);
            AddStrTxData (strDataLen, 4);
            AddStrTxData (strData   , strData.Length);

            Log(true, strcmd); 
            Send();
        }

        public void SendReset(int iMcNo)
        {
            string strData         = GetHandlerStat();
            string strcmd          = Enum.GetName(typeof(EN_CMD_EMOTION), EN_CMD_EMOTION.reset);
            string strMcId         = string.Format("{0:00}"  , iMcNo);
            string strDataLen      = string.Format("{0:0000}", strData.Length);

            ClearTxData  ();
            AddStrTxData (strcmd    , 8);
            AddStrTxData (strMcId   , 2);
            AddStrTxData (strDataLen, 4);
            AddStrTxData (strData   , strData.Length);

            Log(true, strcmd); 
            Send();
        }


        public void Send()
        {
            if(!m_bConnected) return;
            byte[] SendByte = new byte[m_iTxByteCnt + 2];
            Array.Copy(m_TxByte, 0, SendByte, 1, m_iTxByteCnt);

            SendByte[0               ] = 0x23; //SOT
            SendByte[m_iTxByteCnt + 1] = 0x0D; //EOT

            try {
                Client.Send(SendByte);  
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TTesterEMotion. Send " + ex.ToString());
            }
        }
        #endregion


		
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        //Make Log.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void  Log           (bool bSend, String Msg)
        {


            //Local Var.
            string sPath;
            string sTemp;

            if(m_bLockLog) return;

            m_bLockLog = true;
            try {
                string sFile =   "[" + string.Format("{0:yyMMdd}", DateTime.Now)+ "]" + "Tester.txt"; 

                if(bSend) {
                    sTemp = "[" + string.Format("{0:HH:mm:ss}", DateTime.Now) + "]" + "[PROBE --> TESTER]" + Msg + "\r\n";
                }
                else {
                    sTemp = "[" + string.Format("{0:HH:mm:ss}", DateTime.Now) + "]" + "[TESTER --> PROBE]" + Msg + "\r\n";
                }

                //Make Dir.
                FNC.CreateDirOnWork("LOG");
                FNC.CreateDirOnWork("LOG\\TESTER");
                sPath = Application.StartupPath + "\\LOG\\TESTER\\" + sFile;

                using (Stream stream = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write)) 
                {
                    StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
                    sw.BaseStream.Seek(0, SeekOrigin.End);

                    sw.Write(sTemp);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TTesterEMotion. Log " + ex.ToString());
            }
            m_bLockLog = false;
        }

    }


}
