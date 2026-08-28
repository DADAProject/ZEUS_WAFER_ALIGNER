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

    /***************************************************************************/
    /* Class: TesterItemLinkj                                                  */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TesterItemLinkj
    {
        public int    m_nStationNum;
        public string m_strName    ;
        public string m_strUnit    ;
        public string m_strBais    ;
        public string m_strLower   ;
        public string m_strUpper   ;
        public string m_strApply   ;
        public string m_strClamp   ;
        public string m_strGain    ;
        public string m_strOffset  ;
        public string m_ItemCode   ;

        public TesterItemLinkj()
        {
            ResetData();
        }
        public void ResetData()
        {
            m_nStationNum = -1;
            m_strName     = "";
            m_strUnit     = "";
            m_strBais     = "";
            m_strLower    = "";
            m_strUpper    = "";
            m_strApply    = "";
            m_strClamp    = "";
            m_strGain     = "";
            m_strOffset   = "";
            m_ItemCode    = "";
        }
    }


    /***************************************************************************/
    /* Class: SocketDefineLinkJ                                                */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class SocketDefineLinkJ
    {
        #region Tester Enum
        //Scenario
        //[Initial]
        //Prober                           Tester 
        //                          <-     0x32 설비 모드       설비의 Manual/Auto 모드 송신
        //                          <-     0x33 설비 상태       설비의 State 송신
        //0x33 시간동기화           ->                          Time Synq 송신
        //0x34 프로젝트이름송신     ->                          프로젝트 이름 송신(Wafer가 없을 때만 보낼 수 있다)

        //[RUN] 
        //Prober                            Tester 
        //[00]0x32 RECIPE ID         ->                          RECIPE ID 송신
        //[01]                       <-     0x30 상태 정보       0x59 RECIPE ID 수신OK 송신
        //[01-1]                     <-     0x35 Item Info Data  Item Info Data 송신
        //[02]0x30 상태 정보         ->                          0x46 Prober 측정준비 송신
        //[03]                       <-     0x30 상태 정보       0x46 Tester 측정준비 송신
        //[04]0x30 상태 정보         ->                          0x4C Lot Start 송신


        //[05]0x30 상태 정보         ->                          0x57 Wafer 측정시작 송신
        //[06]0x31 On Chip Info      ->                          On Chip 정보 송신
        //[07]                       <-     0x30 상태 정보       0x43 Chip 측정 완료 송신
        //[08]                       <-     0x31 On Chip Info    On Chip 측정 결과 송신
        //[09]0x30 상태 정보         ->                          0x43 결과를 받을 때 송신
        //   0x30 상태 정보          ->                          0x52 결과를 못 받을 때 재송신 요청
        //                           <-     0x31 On Chip Info    On Chip 측정 결과 송신
        //[10]           6~9번 작업 반복 : 208번(192 pixel), 111번(102 Pixel) 
        //[11]0x30 상태 정보         ->                          0x4B Wafer 측정완료 송신
        //[12]           0~11번 작업 반복 : 1Lot 이 완료 될 때 까지 반복
        //[13]0x30 상태 정보         ->                          0x48 Lot 완료 송신 

        public enum EN_TESTER_TYPE 
        {
            DC                    = 0x30  , 
            OPTIC                 = 0x31    
        }

        public enum EN_CMD 
        {
            // Command Define Prober -> Tester
            RESERVED                      , 
            STATUS_INFO                   , //0x30 Prober의 상태 정보 전달 또는 Tester의 상태를 지시함
            CHIP_INFO                     , //0x31 On Chip의 좌표 및 On/Skip Chip 정보를 Tester에 전달함
            RECIPE_ID                     , //0x32 Prober에서 각 테스트마다 Recipe ID를 Tester에 전달함
            TIME_SYNQ                     , //0x33 Prober의 시간을 전달
            PROJECT_NAME                  , //0x34 Wafer내의 Chip 개수 정보와 관련된 프로젝트 이름을 전달함
            WAFER_NAME                    , //0x35 WAFER NAME 전송 
            WAFER_INFO                      //0x36 WAFER SCAN 정보 전송
 
       }

            
        public enum EN_CMD_REP 
        {
           // Command Define Tester -> Prober
            RESERVED                      , 
            STATUS_INFO                   , //0x30 Tester의 상태 정보 전달.
            MEASURE_DATA                  , //0x31 측정 데이터를 Prober로 전달함
            TESTER_MODE                   , //0x32 Tester의 모드 상태를 전달함
            TESETER_STATUS                , //0x33 Tester의 상태를 전달함
            TESTER_ALARM                  , //0x34 Tester에서 발생한 알람 정보를 전달함
            ITEM_INFO                     , //0x35 Prober에서 측정 파일 작성을 위한 Data 송신
            PROJECT_NAME                  , //0x36 TESTER에서 Project Name 요청 
 
            
            //Status-Info Reply
            TESTER_READY                  , //STATUS_INFO - Tester 측정 준비 완료
            RECIPE_REPLY                  , //STATUS_INFO - Prober로부터 Recipe를 전달받으면 Tester에서 응답
            MEASURE_END                   , //STATUS_INFO - Tester Chip 측정 완료
            MEASURE_FAIL                  , //STATUS_INFO - Tester에서 측정 실패시 전송 Probe에서는 해당 메시지 수신 후 Error 해제 상태 메시지를 받으면 동일한 Chip 번호로 다시 측정 할 수 있도록  On_Die_Info를 내려준다.
            TESTER_VISION_READY           , //STATUS_INFO - Tester에서 Vision 준비완료 송신
            TESTER_VISION_REPLY           , //STATUS_INFO - Prober로부터 Vision Start 전달 받으면 Tester에서 응답  
            EndOfRepCmd                          
        }



        public enum EN_STATUS_MSG
        {
            // Command Define
            RESERVED                      = 0x00  , 
            LOT_START                     = 0x4c, //Lot Start	Lot 의 웨이퍼 측정 시작
            WAFER_START                   = 0x57, //웨이퍼 측정 시작
            READY                         = 0x46, //Prober, Tester 측정 준비 완료
            CHIP_DATA_COMPLET             = 0x43, //Tester의 Chip 측정결과 데이터를 받은 경우 확인
            RESEND_CHIP_DATA              = 0x52, //Tester의 Chip 측정결과 데이터를 못받은 경우 재전송 요청
            WAFER_END                     = 0x4B, //웨이퍼 측정 끝
            LOT_END                       = 0x48, //Lot 내의 모든웨이퍼 측정 끝
            ERROR                         = 0x4E, //에러, fail 발생
            TESTER_SHUT_DOWN              = 0x45, //명령에 의한 Tester 종료
            ITEM_MISS_MATCH               = 0x49, //Item, OnChip Value MisMatch
            VISION_END                    = 0x41  //Vision 검사 완료 후 Tester 전송 
                
        }


        public enum EN_REP_STATUS_MSG
        {
            // Command Define
            RESERVED                      = 0x00  , 
            TESTER_READY                  = 0x46, //Tester 측정 준비 완료
            RECIPE_REPLY                  = 0x59, //Prober로부터 Recipe를 전달받으면 Tester에서 응답
            MEASURE_END                   = 0x43, //Tester Chip 측정 완료
            MEASURE_FAIL                  = 0x44, //Tester에서 측정 실패시 전송
            TESTER_VISION_READY           = 0x41, //Tester에서 Vision 준비완료 송신
            TESTER_VISION_REPLY           = 0x42  //Prober로부터 Vision Start 전달 받으면 Tester에서 응답

        }

        public enum EN_ERROR_CODE
        {
            // ERROR CODE DEFINE
            SUCESS                                 , //Error 가 없을 경우
            ITEM_RECIPE_NOT_EXIST                  , //0x31 Tester에 Item_Recipe가 없을 때 발생
            ITEM_RECIPE_FILE_FORMAT_ERROR          , //0x32 Item_Recipe의 파일 내용이 잘못 됐을 때 발생
            BIN_RECIPE_NOT_EXIST                   , //0x33 Tester에 Bin_Recipe가 없을 때 발생
            BIN_RECIPE_FILE_FORMAT_ERROR           , //0x34 Bin_Recipe의 파일 내용이 잘못 됐을 때 발생  
            MEASURE_ALARM                            //0x35 Tester 측정 중 Alarm 발생
            
        }

        public enum EN_TEST_STATUS
        {
            UNKNOWN                                ,
            IDLE                                   ,
            RUN                                    ,
            ERROR                                 
        }

        public enum EN_TEST_MODE
        {
            UNKNOWN                                ,
            MANUAL                                 ,
            AUTO                                  
        }
        #endregion

    }


    /***************************************************************************/
    /* Class: TDutData                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TDutData
    {
        public int      m_iBin    ;
        public int      m_iDutNo  ;
        public int      m_iSBin    ; //Pass : 0x00, Open Fail : 0x01, Shot Fail : 0x02
        public double[] m_dMeasure = new double [vDEF.MAX_TEST_ITEM]; 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TDutData()
        {
            ResetData();
        }
        ~TDutData() 
        { 

        }  

        public object Copy()//181101
        {
            return this.MemberwiseClone();
        }
       
        public void ResetData()
        {
            m_iBin        = -1;
            m_iDutNo      = 0 ;
            m_iSBin       = -1;   
            for(int i=0;i<vDEF.MAX_TEST_ITEM;i++) 
            {
                m_dMeasure[i] = 0;
            }
        }

        public void Load(BinaryReader br)
        {             

            m_iBin          = br.ReadInt32();
            m_iDutNo        = br.ReadInt32();
            m_iSBin         = br.ReadInt32();
            for(int i=0;i<vDEF.MAX_TEST_ITEM;i++) 
            {
                m_dMeasure[i] = br.ReadDouble();
            }
        }
        public void Save(BinaryWriter wr)
        {
            wr.Write(m_iBin  );
            wr.Write(m_iDutNo);
            wr.Write(m_iSBin );
            for(int i=0;i<vDEF.MAX_TEST_ITEM;i++) 
            {
                wr.Write(m_dMeasure[i] );
            }
        }

    }

    /***************************************************************************/
    /* Class: TTesterLinkj                                                     */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TTesterLinkj                                           
    {
        private const  int m_iBufferSize = 100000;

        AsyncSocketServer server;
        private Socket    Client;
        private bool      m_bTesterError       = false;
        private string    m_strTesterErrorCode = "";

                          
		bool[]            m_bRpyPass   = new bool[(int)SocketDefineLinkJ.EN_CMD_REP.EndOfRepCmd];
		bool[]            m_bReply     = new bool[(int)SocketDefineLinkJ.EN_CMD_REP.EndOfRepCmd];
        byte[]            m_TxByte     = new byte[m_iBufferSize];
        byte[]            m_RxByte     = new byte[m_iBufferSize];

        int               m_iTxByteCnt  ;
        int               m_iRxByteCnt  ; 
        bool              m_bThreadAbort;

        SocketDefineLinkJ.EN_ERROR_CODE   m_iError ;
        SocketDefineLinkJ.EN_TEST_STATUS  m_iStatus;
        SocketDefineLinkJ.EN_TEST_MODE    m_iMode  ;


        private const int BufferSize = 100000;
        private byte[] buffer = new byte[BufferSize];
        public string received;
        private Thread ThreadRecive;


        //Tester Data.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int     m_iMaxTestR  ;
        public int     m_iMaxTestC  ;
        public int     m_iMeasureQty;
        public int     m_iWorkX     ;
        public int     m_iWorkY     ;

        public int     m_iRcvDataQty;

        public bool    m_bRcvStatus;
        public bool    m_bLockLog  ;



        public TDutData       [,] Dut       = new TDutData       [vDEF.MAX_PIXEL_R,vDEF.MAX_PIXEL_C];
        public TesterItemLinkj[ ] TestItem  = new TesterItemLinkj[vDEF.MAX_TEST_ITEM               ];




        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public SocketDefineLinkJ.EN_ERROR_CODE _iError
        {
            get { return m_iError;  }
        }
        public SocketDefineLinkJ.EN_TEST_STATUS _iStatus
        {
            get { return m_iStatus;  }
        }
        public SocketDefineLinkJ.EN_TEST_MODE _iMode
        {
            get { return m_iMode;  }
        }

        public bool  _bRcvStatus         { get { return m_bRcvStatus;          }  set { m_bRcvStatus = value;         } }



        public TTesterLinkj()
        {
            m_bLockLog     = false;
            m_bThreadAbort = false;
        }

        public void Close()
        {
            m_bThreadAbort = true;
            ThreadRecive.Join();
            Client.Disconnect(true);
            server.Stop();
            
        }

        public bool IsReply(SocketDefineLinkJ.EN_CMD_REP iRepCmd)
        {
            if(iRepCmd<0 || iRepCmd>=SocketDefineLinkJ.EN_CMD_REP.EndOfRepCmd)  return false;
            return m_bReply[(int)iRepCmd];
        }

        public bool isRpyPass(SocketDefineLinkJ.EN_CMD_REP iRepCmd)
        {
            if(iRepCmd<0 || iRepCmd>=SocketDefineLinkJ.EN_CMD_REP.EndOfRepCmd)  return false;
            return m_bRpyPass[(int)iRepCmd];
        }


        public void Init(int iPort, string sHostIP = "" )
        {
            server            = new AsyncSocketServer   (iPort);
            server.OnAccept  += new AsyncSocketAcceptEventHandler(OnAcceptClient);
            server.Listen();
            
            //server.OnRecieve += new AsyncSocketServer.OnRecieveMessage(OnReciveSocket);

           
            m_iStatus    = SocketDefineLinkJ.EN_TEST_STATUS.UNKNOWN;
            m_iMode      = SocketDefineLinkJ.EN_TEST_MODE  .UNKNOWN;
            m_iError     = SocketDefineLinkJ.EN_ERROR_CODE .SUCESS ;
            m_iRxByteCnt = 0;
            for(int i=0;i<vDEF.MAX_PIXEL_R;i++)  
            {
                for(int j=0;j<vDEF.MAX_PIXEL_C;j++)  //Pixel 수량
                {
                    Dut[i,j] = new TDutData();
                }
            }

            for(int i=0;i<vDEF.MAX_TEST_ITEM;i++) 
            {
                TestItem[i] = new TesterItemLinkj(); 
            }

            LoadItemName(true);
        }

        private void OnAcceptClient(object sender, AsyncSocketAcceptEventArgs e)
        {
            Client                     = e.Worker;
            Client.ReceiveBufferSize   = m_iBufferSize;
            ThreadRecive     = new Thread(new ThreadStart(ReciveProc));
            
            ThreadRecive.Start();
            m_bThreadAbort = false;

        }


        public void Reset()
        {
            ClearTxData();
            ClearRxData();
        }


        public void SendClient(byte[] data)
        {

            if (Client == null)
                return;
            if (!Client.Connected)  return;
            try
            {
                Client.Send(data, data.Length, 0);
            }
            catch (Exception exc)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ.SendClient() " + exc.ToString());
            }
        }

        public void ReciveProc()
        {
        
            int len = 0;
            try
            {
                while (!m_bThreadAbort)
                {
                    len =  0;
                    Thread.Sleep(10);
                    if( Client == null   )  continue;
                    if(!Client.Connected)  continue;
   
                    Array.Clear(buffer, 0, m_iBufferSize);
                    len = Client.Receive(buffer);
                    if (len == 0) 
                    {
                        ThreadRecive.Join();
                        Client.Disconnect(true);
                        continue;
                    }
                    //System.Text.Encoding.ASCII.GetString(buffer);

                    OnReciveSocket(len, buffer);
                }
            }
            catch (Exception exc)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ.UpdateRecive() " + exc.ToString());
            }
         
        }
        public bool IsConnected()
        {
            bool isConnect = false;
            if (Client == null) return false;
            try {
                isConnect =  Client.Connected;
            }
            catch (Exception exc)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ.IsConnected() " + exc.ToString());
            }
            return isConnect;
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
        public void ClearRep(SocketDefineLinkJ.EN_CMD_REP iRepCmd)
        {
            m_bRpyPass[(int)iRepCmd] = false; 
            m_bReply  [(int)iRepCmd] = false;
        }


        public void ClearRepAll()
        {
            for(int i=0;i<(int)SocketDefineLinkJ.EN_CMD_REP.EndOfRepCmd; i++) {
                m_bRpyPass[i] = false;
                m_bReply  [i] = false;
            }

        }
      



        private bool OnCheckLength(byte[] data, int len)
        {
             bool bErrFormat  = false;
            if(data[0]     != 0x01) bErrFormat = true;
            if(data[len-3] != 0x04) bErrFormat = true;
            if(data[len-2] != 0x0d) bErrFormat = true;
            if(data[len-1] != 0x0a) bErrFormat = true;
            return bErrFormat;
        }
   
        private void OnReciveSocket(int len, byte[] data)
        {
            int iFindEnd = -1;
            int iFindStart = -1;

            byte[] ByteBuff = new byte[m_iBufferSize];
            byte byteNull = 0x00;
            byte byteEnd = 0x0D;


            try {
                iFindStart = Array.IndexOf ( m_RxByte, byteNull );
                if (iFindStart + len >= m_iBufferSize) {
                    ClearRxData ( );
                    return;
                }

                if(len>10000)
                {
                    iFindEnd = -1;

                }

                Array.Copy ( data, 0, m_RxByte, iFindStart, len );
                m_iRxByteCnt = iFindStart + len;
                do {
                    iFindEnd = Array.IndexOf ( m_RxByte, byteEnd );
                    
                    if (iFindEnd > 0 && m_RxByte[iFindEnd + 1] == 0x0A) {
                        Array.Copy ( m_RxByte, 0, ByteBuff, 0, iFindEnd + 2 );
                        Array.Clear ( m_RxByte, 0, iFindEnd + 2 );
                        Array.Copy ( m_RxByte, iFindEnd + 2, m_RxByte, 0, m_iBufferSize-(iFindEnd + 2));
                        ProcReciveData ( iFindEnd + 2, ByteBuff );
                    }
                } while (iFindEnd > 0);
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ OnReciveSocket()" + ex.ToString());
            }
        }





        private void ProcReciveData(int len, byte[] data)
        //private void OnReciveSocket(object sender, string data)
        {
            bool bErrCmd = false;

            bErrCmd = OnCheckLength (data, len);
            try {
                switch(data[2])
                {
                    default     : bErrCmd = true;        break;
                    case    0x30: RcvStatusInfo  (data); break;
                    case    0x31: RcvMeasureData (data); break;
                    case    0x32: RcvTesterMode  (data); break;
                    case    0x33: RcvTesterStatus(data); break;
                    case    0x34: RcvTesterAlarm (data); break;
                    case    0x35: RcvItemInfo    (data); break;
                    case    0x36: RcvReqPrjName  (data); break;
         
                }

            }
            catch (Exception ex) {
                cDEF.LOG.ExceptionTrace ( "TesterLinkJ ProcReciveData()" + ex.ToString ( ) );
            }
        }




        public void SeprTestData(byte[] RcvByte)
        {

            /*
            int iByteNo ;
            int iWorkX    = FNC.GetByteArrayToInt(RcvByte, 8 , 4);
            int iWorkY    = FNC.GetByteArrayToInt(RcvByte, 12, 4);
            int iMeasure  ;
            int iSign     ;
            int iDigit    ;
            m_iItemQty = 1; //임시 
            iByteNo = 16;
            for(int i=0;i<m_iMaxTestR;i++)  
            {
                for(int j=0;j<m_iMaxTestC;j++)  
                {
                    //Dut[i,j].m_iDutNo = FNC.GetByteArrayToInt(RcvByte, iByteNo  , 1);
                    //Dut[i,j].m_iRslt  = FNC.GetByteArrayToInt(RcvByte, iByteNo+1, 1);
                    Dut[i,j].m_iDutNo = RcvByte[iByteNo  ];
                    Dut[i,j].m_iRslt  = RcvByte[iByteNo+1];

                    iByteNo += 2;
                    for(int k=0;k<m_iItemQty;k++)  //Measure Item Qty 
                    {
                        //iMeasure = FNC.GetByteArrayToInt(RcvByte, iByteNo, 3);

                        iMeasure = RcvByte[iByteNo];
                        iMeasure = iMeasure << 8 | RcvByte[iByteNo+1];
                        iMeasure = iMeasure << 8 | RcvByte[iByteNo+2];

                        iSign    = (RcvByte[iByteNo+3]>>4) & 0xf;
                        iDigit   =  RcvByte[iByteNo+3]     & 0xf;
                        Dut[i,j].m_dMeasure[k] = (double)iMeasure * ((iSign==0) ? 1 : -1) / Math.Pow(10.0, iDigit);  
                        iByteNo += 4;
                    }
                }
            }
            */
            int iEndPos  = 0;
            int iFindPos = 0;
            int itemCnt  = 0;

            for (int r = 0; r < vDEF.MAX_PIXEL_R; r++)
            {
                for (int c = 0; c < vDEF.MAX_PIXEL_C; c++)  //Pixel 수량
                {
                    Dut[r, c].ResetData();
                    cDEF.DM.WAF[m_iWorkX,m_iWorkY].Pin[r, c].ResetData();
                }
            }

            try {
                    for(int i=0;i<RcvByte.Length;i++)
                    {
                        if(RcvByte[i] == 0x0D) { iEndPos = i;  break; }             
                    }
                    int iTestDataQty = m_iMaxTestR * m_iMaxTestC;

                    //string RcvString = "1,-21,1,1,3,2,1,5.9999675,0.169546729759815/1,-21,1,1,4,2,2,5.9998701,0.166202782544987/1,-21,1,1,4,1,3,5.9996613,0.167300927157313/1,-21,1,1,5,1,4,6.000037,0.167711651202648/1,-21,1,1,5,2,5,5.9999396,0.165595022324035/1,-21,1,1,6,2,6,6.0000231,0.157825449648528/1,-21,1,1,6,1,7,5.9999814,0.160031449416247/1,-21,1,1,7,1,8,5.9999396,0.1679023866627/1,-21,1,1,7,2,9,5.9999814,0.161082793216387/1,-21,1,1,8,2,10,5.9998561,0.155427368639469/1,-21,1,1,8,1,11,6.0000092,0.155389619622875/1,-21,1,1,9,1,12,6.0001517,0.168551351351297/1,-21,1,2,9,2,13,6.0000958,0.165788480606683/1,-21,1,2,10,2,14,5.9999,0.167749509565571/1,-21,1,2,10,1,15,5.9999139,0.167834078901029/1,-21,1,2,11,1,16,6.0000538,0.164286668311514/1,-21,1,2,11,2,17,6.0001238,0.158406208453824/1,-21,1,2,12,2,18,6.0000119,0.159723026714617/1,-21,1,2,12,1,19,6.0000678,0.167574324290561/1,-21,1,2,13,1,20,5.999872,0.161147479581858/1,-21,1,3,13,2,21,6.0001517,0.155278913469937/1,-21,1,3,14,2,22,5.9999559,0.155331429004954/1,-21,1,3,14,1,23,6.0000538,0.158164299271274/1,-21,1,3,15,1,24,6.0000346,0.168023397448539/1,-21,1,3,15,2,25,5.9999094,0.165617208071241/1,-21,1,3,16,2,26,5.9998538,0.167528982065506/1,-21,1,3,16,1,27,6.0000346,0.167916508313526/1,-21,1,3,17,1,28,6.0002014,0.163947851569314/1,-21,1,4,17,2,29,6.0001875,0.157075762372723/1,-21,1,4,18,2,30,6.0001736,0.159237615098097/1,-21,1,4,18,1,31,6.0001041,0.166885289938995/1,-21,1,4,19,1,32,6.000118,0.161064759372348/1,-21,1,4,19,2,33,5.999965,0.154780446332116/1,-21,1,4,20,2,34,6.0000763,0.155374487474688/1,-21,1,4,20,1,35,5.9999511,0.162108392780949/1,-21,1,4,21,1,36,6.0002377,0.167914397878878/1,-21,1,4,21,2,37,6.0000695,0.167973097908981/1,-21,1,4,22,2,38,5.9999294,0.167509847227909/1,-21,1,4,22,1,39,6.0001256,0.167836896281486/1,-21,1,4,23,1,40,5.9999153,0.160099487394118/1,-21,1,5,23,2,41,6.0001115,0.157159824012946/1,-21,1,5,24,2,42,6.0000835,0.159653256938974/1,-21,1,5,24,1,43,5.9998173,0.165267305458056/1,-21,1,5,25,1,44,5.9998873,0.154622195442966/1,-21,1,5,25,2,45,5.9998173,0.155210242525559/1,-21,1,5,26,2,46,6.0000275,0.155353027935614/1,-21,1,5,28,4,47,6.0000818,0.166681038173558/1,-21,1,5,28,3,48,6.0001238,0.167714041078374/1,-21,1,5,27,3,49,5.9999838,0.167770780942016/1,-21,1,5,27,4,50,5.9999698,0.16804876234729/1,-21,1,5,26,4,51,6.0000118,0.157513269285882/1,-21,1,5,26,3,52,5.9999838,0.157677068639258/1,-21,1,6,25,3,53,5.9998998,0.159754419389349/1,-21,1,6,25,4,54,6.0000958,0.162876407245609/1,-21,1,6,24,4,55,5.9999698,0.156821148998484/1,-21,1,6,24,3,56,5.9999978,0.155286201534661/1,-21,1,6,23,3,57,6.0000678,0.155746968983962/1,-21,1,6,23,4,58,5.9998858,0.160314430471352/1,-21,1,6,22,4,59,6.0001755,0.166032678707802/1,-21,1,6,22,3,60,6.0002172,0.167404555716617/1,-21,1,7,21,3,61,6.0002311,0.167936686899549/1,-21,1,7,21,4,62,5.9999946,0.168159246321933/1,-21,1,7,20,4,63,5.9999389,0.157906318306594/1,-21,1,7,20,3,64,6.0000363,0.158865124850171/1,-21,1,7,19,3,65,5.9999807,0.159790724800858/1,-21,1,7,19,4,66,5.9999946,0.159480271245537/1,-21,1,7,18,4,67,5.9997442,0.154674103472287/1,-21,1,7,18,3,68,6.0002172,0.155168497931299/1,-21,1,8,17,3,69,5.9999807,0.15484277463949/1,-21,1,8,17,4,70,5.9998415,0.162624118671535/1,-21,1,8,16,4,71,5.9998604,0.166209818389243/1,-21,1,8,16,3,72,6.000111,0.167366074798146/1,-21,1,8,15,3,73,5.9999718,0.167977057597375/1,-21,1,8,15,4,74,6.0001807,0.167899715255386/1,-21,1,8,14,4,75,6.0000553,0.157952436005673/1,-21,1,8,14,3,76,5.9998325,0.161421725267727/1,-21,1,8,13,3,77,5.9997768,0.159986585945685/1,-21,1,8,13,4,78,6.0001946,0.161252170913819/1,-21,1,8,12,4,79,5.9999652,0.166599411577328/1,-21,1,8,12,3,80,5.9998256,0.167320858088469/1,-21,0,0,11,3,81,5.9999652,0.167803804670461/1,-21,0,0,11,4,82,5.9998116,0.167772142521937/1,-21,0,0,10,4,83,6.0003001,0.15796782821166/1,-21,0,0,10,3,84,5.9998675,0.16077691468021/1,-21,0,0,9,3,85,5.9999233,0.164441800963762/1,-21,0,0,9,4,86,6.0001187,0.161341546199066/1,-21,0,0,8,4,87,6.0001187,0.155081476863163/1,-21,0,0,8,3,88,5.9998116,0.154947022889012/1,-21,0,0,7,3,89,6.0001187,0.155201142425646/1,
//21,0,0,7,4,90,5.9998814,0.16098169437334/1,-21,0,0,6,4,91,5.9999371,0.166461841268302/1,-21,0,0,6,3,92,5.9999231,0.167577205395596/1,-21,0,0,5,3,93,6.0000205,0.167711445937628/1,-21,0,0,5,4,94,6.0001318,0.167750793465585/1,-21,0,0,4,4,95,6.0002014,0.157735116698981/1,-21,0,0,4,3,96,5.999951,0.160990753405203/1,-21,0,0,3,3,97,6.0001179,0.166984065771541/1,-21,0,0,3,4,98,5.9997701,0.161369155937217/1,-21,0,0,2,4,99,5.999951,0.155310200530033/1,-21,0,0,2,3,100,5.9999231,0.155117713279601/1,-21,0,0,1,3,101,5.9999649,0.155199869333419/1,-21,0,0,1,4,102,6.0001875,0.160706121567912";
                    string  RcvString = FNC.GetByteArrayToString(RcvByte, 10, iEndPos-11);
                    for (int i=0; i<iTestDataQty; i++) {
                        iFindPos = RcvString.IndexOf("/");
                        if(iFindPos<=0) {
                            SeprItemData(RcvString);
                            itemCnt ++;
                            break;
                            }
                        SeprItemData(RcvString.Substring(0,iFindPos));
                        RcvString = RcvString.Substring(iFindPos+1);
                        itemCnt ++;
                        }
                    m_iRcvDataQty = itemCnt;
                    cDEF.SEQ.TBL.ShiftProbe(m_iWorkX, m_iWorkY);
                    //cDEF.SEQ.TBL.TempWafLog(m_iWorkX, m_iWorkY, true); //임시 로그 기록
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ SeprTestData()" + ex.ToString());
            }


        }

/*
"10,20,0,9,1,1,1,0.0099905,544.534893527369/10,20,0,9,1,2,2,0.0099877,544.537059737094/10,20,0,9,1,3,3,0.0099905,544.541952682495/10,20,0,9,2,3,4,0.0099988,544.541634217378/10,20,0,9,2,2,5,0.0100211,544.54144351433/10,20,0,9,2,1,6,0.0100239,544.538873458255/10,20,0,9,3,1,7,0.0100002,544.535238635345/10,20,0,9,3,2,8,0.0100308,544.537445549293/10,20,0,9,3,3,9,0.0098388,544.53650786067/10,20,0,9,4,3,10,0.0100044,544.537733781232/10,20,0,9,4,2,11,0.009996,544.535973207701/10,20,0,9,4,1,12,0.0099515,544.538334635255/10,20,0,9,5,1,13,0.0100333,544.537459293587/10,20,0,9,5,2,14,0.0099047,544.537007100519/10,20,0,9,5,3,15,0.0100445,544.538774618431/10,20,0,9,6,3,16,0.0100012,544.535092303663/10,20,0,9,6,2,17,0.0099928,544.535793036164/10,20,0,9,6,1,18,0.010018,544.532471132255/10,20,0,9,7,1,19,0.009913,544.539422847904/10,20,0,9,7,2,20,0.0100445,544.537101057959/10,20,0,9,7,3,21,0.0100026,544.535410312116/10,20,0,9,8,3,22,0.0100347,544.537747265091/10,20,0,9,8,2,23,0.0099214,544.536851326744/10,20,0,9,8,1,24,0.0100403,544.53658591408/10,20,0,9,9,1,25,0.0100293,544.539255556752/10,20,0,9,9,2,26,0.0100001,544.540778257524/10,20,0,9,9,3,27,0.009982,544.539129536067/10,20,0,9,10,3,28,0.0099751,544.5351504936/10,20,0,9,10,2,29,0.0099612,544.535902993992/10,20,0,9,10,1,30,0.0100043,544.536929596947/10,20,0,9,11,1,31,0.0099778,544.537515786235/10,20,0,9,11,2,32,0.0100307,544.539449079105/10,20,0,9,11,3,33,0.0099987,544.539552044027/10,20,0,9,12,3,34,0.009932,544.536302202556/10,20,0,9,12,2,35,0.009989,544.540330910786/10,20,0,9,12,1,36,0.0100224,544.534967825867/10,20,0,9,13,1,37,0.0099947,544.53664296832/10,20,0,9,13,2,38,0.0099933,544.543664825896/10,20,0,9,13,3,39,0.0099877,544.538599883341/10,20,0,9,14,3,40,0.0100059,544.538056158499/10,20,0,9,14,2,41,0.0099891,544.535519473342/10,20,0,9,14,1,42,0.0100087,544.538705091019/10,20,0,9,15,1,43,0.0100017,544.533766811289/10,20,0,9,15,2,44,0.0099947,544.541375533982/10,20,0,9,15,3,45,0.0100157,544.537739958751/10,20,0,9,16,3,46,0.0099947,544.53422468049/10,20,0,9,16,2,47,0.0100115,544.533603852582/10,20,0,9,16,1,48,0.0100325,544.543205748582/10,20,0,9,17,1,49,0.0099344,544.537013785661/10,20,0,9,17,2,50,0.0099918,544.537513141658/10,20,0,9,17,3,51,0.0100016,544.537767318244/10,20,0,9,18,3,52,0.0099484,544.541379858071/10,20,0,9,18,2,53,0.0099358,544.538153335693/10,20,0,9,18,1,54,0.0099918,544.53625271892/10,20,0,9,19,1,55,0.0100058,544.539276792567/10,20,0,9,19,2,56,0.0100002,544.536858121186/10,20,0,9,19,3,57,0.0100044,544.540293523869/10,20,0,9,20,3,58,0.0100423,544.53729086876/10,20,0,9,20,2,59,0.0099974,544.539295418268/10,20,0,9,20,1,60,0.0099834,544.538590400528/10,20,0,9,21,1,61,0.0100464,544.538577270426/10,20,0,9,21,2,62,0.0099365,544.535395981692/10,20,0,9,21,3,63,0.0099866,544.537064002054/10,20,0,9,22,3,64,0.0100144,544.541244757335/10,20,0,9,22,2,65,0.0100255,544.537313825914/10,20,0,9,22,1,66,0.0099615,544.534088521908/10,20,0,9,23,1,67,0.0099685,544.533836563149/10,20,0,9,23,2,68,0.0100033,544.534472189859/10,20,0,9,23,3,69,0.0099685,544.540190264352/10,20,0,9,24,3,70,0.0100019,544.542925480059/10,20,0,9,24,2,71,0.0100005,544.53868642171/10,20,0,9,24,1,72,0.0099991,544.536125234149/10,20,0,9,25,1,73,0.0100017,544.538802296203/10,20,0,9,25,2,74,0.0099891,544.545969169153/10,20,0,9,25,3,75,0.0099181,544.536475762824/10,20,0,9,26,3,76,0.0099905,544.538864532335/10,20,0,9,26,2,77,0.0100044,544.539085223655/10,20,0,9,26,1,78,0.0100184,544.537452360997/10,20,0,9,27,1,79,0.0099905,544.537314085229/10,20,0,9,27,2,80,0.0100072,544.535257828919/10,20,0,0,27,3,81,0.0099961,544.538569513105/10,20,0,0,28,3,82,0.0099947,544.539901356163/10,20,0,0,28,2,83,0.0100058,544.537570949553/10,20,0,0,28,1,84,0.0100058,544.537955456404/10,20,0,0,29,1,85,0.0099865,544.536897093043/10,20,0,0,29,2,86,0.0099879,544.537064428716/10,20,0,0,29,3,87,0.0099879,544.538650216737/10,20,0,0,30,3,88,0.0099851,544.539306256377/10,20,0,0,30,2,89,0.0099991,544.536622366057/10,20,0,0,30,1,90,0.0099991,544.532070997361/10,20,0,0,31,1,91,0.0100033,544.535002655451/10,20,0,0,31,2,92,0.0099963,544.538145965411/10,20,,0,31,3,93,0.010006,544.536213072764/10,20,0,0,32,3,94,0.010006,544.536933188225/10,20,0,0,32,2,95,0.0099991,544.540137245373/10,20,0,0,32,1,96,0.0099321,544.534687038948/10,20,0,0,32,6,97,0.0099778,544.535359157731/10,20,0,0,32,5,98,0.0100126,544.544118375651/10,20,0,0,32,4,99,0.0100529,544.535047659225/10,20,0,0,31,4,100,0.010007,544.536176983013/10,20,0,0,31,5,101,0.0100042,544.539662178914/10,20,0,0,31,6,102,0.0099945,544.538024053065/10,20,0,0,30,6,103,0.0100028,544.538085934907/10,20,0,0,30,5,104,0.0100306,544.540262893369/10,20,0,0,30,4,105,0.0100028,544.540735419845/10,20,0,0,29,4,106,0.0100098,544.534595432018/10,20,0,0,29,5,107,0.0100487,544.537748912257/10,20,0,0,29,6,108,0.0100084,544.53759195022/10,20,0,0,28,6,109,0.0099939,544.541180782452/10,20,0,0,28,5,110,0.0099995,544.536974929971/10,20,0,0,28,4,111,0.0100051,544.534788113163/10,20,0,0,27,4,112,0.0100078,544.534121453875/10,20,0,0,27,5,113,0.0099245,544.536151691897/10,20,0,0,27,6,114,0.0100301,544.536871295389/10,20,0,0,26,6,115,0.0100078,544.533468733284/10,20,0,0,26,5,116,0.0100078,544.535646887607/10,20,0,0,26,4,117,0.010019,544.541193993006/10,20,0,0,25,4,118,0.0099648,544.534257463423/10,20,0,0,25,5,119,0.009987,544.539639075988/10,20,0,0,25,6,120,0.0099787,544.537565143465/10,20,0,0,24,6,121,0.0100098,544.540056618682/10,20,0,0,24,5,122,0.0099478,544.53949551703/10,20,0,0,24,4,123,0.009945,544.542026394924/10,20,0,0,23,4,124,0.0099323,544.537718647722/10,20,0,0,23,5,125,0.0099309,544.541365401091/10,20,0,0,23,6,126,0.0099717,544.53690817951/10,20,0,0,22,6,127,0.0099238,544.535692230489/10,20,0,0,22,5,128,0.0100168,544.538028202128/10,20,0,0,22,4,129,0.0100154,544.539391366229/10,20,0,0,21,4,130,0.0099436,544.538375043747/10,20,0,0,21,5,131,0.0099182,544.53845446868/10,20,0,0,21,6,132,0.0101154,544.53727347449/10,20,0,0,20,6,133,0.010012,544.536458196922/10,20,0,0,20,5,134,0.0099897,544.53781571976/10,20,0,0,20,4,135,0.0099492,544.535042448052/10,20,0,0,19,4,136,0.010019,544.536737831298/10,20,0,0,19,5,137,0.0099953,544.534973525715/10,20,0,0,19,6,138,0.0099813,544.538791300031/10,20,0,0,18,6,139,0.0100176,544.542891696926/10,20,0,0,18,5,140,0.0100009,544.532396866491/10,20,0,0,18,4,141,0.0100134,544.538167744224/10,20,0,0,17,4,142,0.0099939,544.538977784645/10,20,0,0,17,5,143,0.0100009,544.536996538746/10,20,0,0,17,6,144,0.0099939,544.536279616182/10,20,0,0,16,6,145,0.0099934,544.539513673/10,20,0,0,16,5,146,0.009992,544.530617147483/10,20,0,0,16,4,147,0.0099823,544.540062008062/10,20,0,0,15,4,148,0.0099934,544.538952138349/10,20,0,0,15,5,149,0.0100059,544.540536123288/10,20,0,0,15,6,150,0.0099989,544.537786757173/10,20,0,0,14,6,151,0.0099892,544.540110020169/10,20,0,0,14,5,152,0.0099934,544.535359166388/10,20,0,0,14,4,153,0.0100142,544.539200366963/10,20,0,0,13,4,154,0.0100198,544.540854636428/10,20,0,0,13,5,155,0.0100656,544.539618235698/10,20,0,0,13,6,156,0.0100448,544.539647993032/10,20,0,0,12,6,157,0.0099914,544.53994933552/10,20,0,0,12,5,158,0.0100011,544.537915233577/10,20,0,0,12,4,159,0.0098927,544.539015606695/10,20,0,0,11,4,160,0.009908,544.539115143748/10,20,0,0,11,5,161,0.0099706,544.535622583157/10,20,0,0,11,6,162,0.0097009,544.538458131967/10,20,0,0,10,6,163,0.0098496,544.537936504947/10,20,0,0,10,5,164,0.0098816,544.537182331159/10,20,0,0,10,4,165,0.0098343,544.538199583364/10,20,0,0,9,4,166,0.010015,544.532396602893/10,20,0,0,9,5,167,0.0098733,544.5323958205/10,20,0,0,9,6,168,0.0098343,544.536956804956/10,20,0,0,8,6,169,0.0098089,544.540799052582/10,20,0,0,8,5,170,0.0099413,544.536173217474/10,20,0,0,8,4,171,0.0099901,544.540844354203/10,20,0,0,7,4,172,0.0099079,544.537704842419/10,20,0,0,7,5,173,0.0099246,544.538488915335/10,20,0,0,7,6,174,0.0099832,544.536307422105/10,20,0,0,6,6,175,0.0100431,544.536305735417/10,20,0,0,6,5,176,0.0099776,544.537721785174/10,20,0,0,6,4,177,0.0099706,544.536051690587/10,20,0,0,5,4,178,0.0099943,544.53573148019/10,20,0,0,5,5,179,0.0099622,544.537956790665/10,20,0,0,5,6,180,0.009834,544.543395773642/10,20,0,0,4,6,181,0.0099946,544.539013786832/10,20,0,0,4,5,182,0.0100099,544.534809558457/10,20,0,0,44,183,0.0099891,544.537883242479/10,20,0,0,3,4,184,0.0100362,544.539766258559/10,20,0,0,3,5,185,0.0100029,544.539468344067/10,20,0,0,3,6,186,0.0099849,544.539565031323/10,20,0,0,2,6,187,0.009996,544.536804393203/10,20,0,0,2,5,188,0.0099988,544.53731101655/10,20,0,0,2,4,189,0.0100529,544.538478026053/10,20,0,0,1,4,190,0.0100071,544.541494852679/10,20,0,0,1,5,191,0.0099904,544.534993158527/10,20,0,0,1,6,192,0.0100043,544.536088765713"
    */


        public void SeprItemData(string Data)
        {

            int iFindPos = 0;
            int iFndQty  = 0;
            int iMasure  = 0;   
            String[]   sFindItem = new String  [vDEF.MAX_TEST_ITEM];


            try{ 

                for (int i=0; i<m_iMeasureQty+7; i++) {
                    iFindPos = Data.IndexOf(",");
                    if(iFindPos<=0) {
                        sFindItem[i] = Data.Trim();
                        iFndQty ++;
                        break;
                        }
                    sFindItem[i] = Data.Substring(0,iFindPos).Trim();
                    Data = Data.Substring(iFindPos+1);
                    iFndQty ++;
                    }

                int iR  = 0; 
                int iC  = 0;
                if(!Int32.TryParse(sFindItem[4], out iC)) return;
                if(!Int32.TryParse(sFindItem[5], out iR)) return;
                iC -= 1;
                iR -= 1;

                if(iC<0 || iC>=m_iMaxTestC              ) return;
                if(iR<0 || iR>=m_iMaxTestR              ) return;
 
                Dut[iR,iC].m_iBin    = Convert.ToInt32(sFindItem[2]);
                Dut[iR,iC].m_iSBin   = Convert.ToInt32(sFindItem[3]);
                Dut[iR,iC].m_iDutNo  = Convert.ToInt32(sFindItem[6]);
                for (int i=0; i<iFndQty - 7; i++) {
                    Dut[iR,iC].m_dMeasure[i] = Convert.ToDouble(sFindItem[7+i]);
                    iMasure ++;
                    }
                //cDEF.DM.WAF[m_iWorkX,m_iWorkY].Pin[iR, iC] = (TDutData)Dut[iR, iC].Copy();

                cDEF.DM.WAF[m_iWorkX,m_iWorkY].Pin[iR, iC].m_iBin   = Dut[iR,iC].m_iBin  ;
                cDEF.DM.WAF[m_iWorkX,m_iWorkY].Pin[iR, iC].m_iSBin  = Dut[iR,iC].m_iSBin ; 
                cDEF.DM.WAF[m_iWorkX,m_iWorkY].Pin[iR, iC].m_iDutNo = Dut[iR,iC].m_iDutNo;
                for(int i=0;i<vDEF.MAX_TEST_ITEM;i++)
                    cDEF.DM.WAF[m_iWorkX,m_iWorkY].Pin[iR, iC].m_dMeasure[i] = Dut[iR,iC].m_dMeasure[i]; 

                
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ SeprItemData()" + ex.ToString());
            }
        }


        public void NoneSeprItemInfo(byte[] RcvByte)
        {
            //Item에 해당하는 값들은 ‘,’로 구분한다.
            //Ex)X,Y,BIN,S_BIN,Px,Py,INDEX
            //   위 항목은 Fix이며 이후의 아이템은 Item_Recipe에 따라 달라진다.
            //   INDEX는 PixelNo이다.

            int iEndPos = 0;
            int iFindPos = 0;
            String[] sFindItem = new String[vDEF.MAX_TEST_ITEM];

            try
            {
                int itemCnt = 0;
                for (int i = 0; i < RcvByte.Length; i++)
                {
                    if (RcvByte[i] == 0x0D) { iEndPos = i; break; }
                }
                string colum = "WF_ID,XADR,YADR,RANK,S_RANK,PXADR,PYADR,INDEX,VF1,IF,VF2-100,Wd1,Wp1,WX1,WY1,CCT1,CRI1,LM1,VF2-300,Wd2,Wp2,WX2,WY2,CCT2,CRI2,LM2,VF3,VR,IR,VF4,EOS,VF6,DVF,dlX,dlY,Intensity,";
                //string RcvString = ""X,Y,BIN_S_BIN,Px,Py,INDEX,VF01_DC,IV,"";
                //"XADR,YADR,RANK,S_BIN,PXADR,PYADR,INDEX,VF1,IF,VF2-100,Wd1,Wp1,WX1,WY1,CCT1,CRI1,LM1,VF2-300,Wd2,Wp2,WX2,WY2,CCT2,CRI2,LM2,VF3,VR,IR,VF4,EOS,VF6,DVF"
                string RcvString = FNC.GetByteArrayToString(RcvByte, 8, iEndPos - 9);


                for (int i = 0; i < vDEF.MAX_TEST_ITEM; i++)
                {
                    iFindPos = RcvString.IndexOf(",");
                    if (iFindPos <= 0)
                    {
                        sFindItem[i] = RcvString.Trim();
                        itemCnt++;
                        break;
                    }
                    sFindItem[i] = RcvString.Substring(0, iFindPos).Trim();
                    RcvString = RcvString.Substring(iFindPos + 1);
                    itemCnt++;
                }

                int iMeasureQty = 0;
                int iMeasurePos = 0; //Measure 시작위치
                for (int i = 0; i < vDEF.MAX_TEST_ITEM; i++)
                {
                    TestItem[i].ResetData();
                }

                for (int i = 0; i < itemCnt; i++)
                {
                    if (sFindItem[i].IndexOf("INDEX") >= 0) { iMeasurePos = i + 1; break; }
                }

                for (int i = iMeasurePos; i < itemCnt; i++)
                {
                    if (sFindItem[i] == "") continue;
                    TestItem[iMeasureQty].m_strName = sFindItem[i];
                    iMeasureQty++;
                }
                m_iMeasureQty = iMeasureQty;
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ SeprItemInfo()" + ex.ToString());
            }

        }


        public void SeprItemInfo(byte[] RcvByte)
        {
            //Item에 해당하는 값들은 ‘,’로 구분한다.
            //Ex)X,Y,BIN,S_BIN,Px,Py,INDEX
            //   위 항목은 Fix이며 이후의 아이템은 Item_Recipe에 따라 달라진다.
            //   INDEX는 PixelNo이다.

            int iEndPos  = 0;
            int iFindPos = 0;
            String[]   sFindItem = new String  [vDEF.MAX_TEST_ITEM];

            try {
                int itemCnt  = 0;
                for(int i=0;i<RcvByte.Length;i++)
                {
                    if(RcvByte[i] == 0x0D) { iEndPos = i;  break; }             
                }

                //string RcvString = ""X,Y,BIN_S_BIN,Px,Py,INDEX,VF01_DC,IV,"";
                //"XADR,YADR,RANK,S_BIN,PXADR,PYADR,INDEX,VF1,IF,VF2-100,Wd1,Wp1,WX1,WY1,CCT1,CRI1,LM1,VF2-300,Wd2,Wp2,WX2,WY2,CCT2,CRI2,LM2,VF3,VR,IR,VF4,EOS,VF6,DVF"
                string  RcvString = FNC.GetByteArrayToString(RcvByte, 8, iEndPos-9);
                

                for (int i=0; i<vDEF.MAX_TEST_ITEM; i++) {
                    iFindPos = RcvString.IndexOf(",");
                    if(iFindPos<=0) {
                        sFindItem[i] = RcvString.Trim();
                        itemCnt ++;
                        break;
                        }
                    sFindItem[i] = RcvString.Substring(0,iFindPos).Trim();
                    RcvString = RcvString.Substring(iFindPos+1);
                    itemCnt ++;
                    }

                int iMeasureQty  = 0;
                int iMeasurePos  = 0; //Measure 시작위치
                for(int i=0; i<vDEF.MAX_TEST_ITEM; i++) {
                    TestItem[i].ResetData();
                  }

                for (int i=0; i<itemCnt; i++) {
                    if(sFindItem[i].IndexOf("INDEX") >= 0) { iMeasurePos = i+1; break; }
                }

                for (int i=iMeasurePos; i<itemCnt; i++) {
                    if(sFindItem[i] == "") continue;
                    TestItem[iMeasureQty].m_strName = sFindItem[i];
                    iMeasureQty ++; 
                    }
                m_iMeasureQty = iMeasureQty;
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TesterLinkJ SeprItemInfo()" + ex.ToString());
            }

        }

        public bool RcvStatusInfo(byte[] RcvByte)
        {
            //0x30 : 상태 정보 송신
            //SOH	     Tester Type	Message Type	Message	  EOT	    CR/LF
            //1 Byte	1 Byte	        1 Byte (0x30)	2 Bytes	  1 Byte	2 Bytes
            bool   isPass = false;
            int iCmd = (int)SocketDefineLinkJ.EN_CMD_REP.STATUS_INFO   ; 
                 if(RcvByte[4] == (byte)SocketDefineLinkJ.EN_REP_STATUS_MSG.TESTER_READY       ) {iCmd = (int)SocketDefineLinkJ.EN_CMD_REP.TESTER_READY       ; }//Tester 측정 준비 완료
            else if(RcvByte[4] == (byte)SocketDefineLinkJ.EN_REP_STATUS_MSG.RECIPE_REPLY       ) {iCmd = (int)SocketDefineLinkJ.EN_CMD_REP.RECIPE_REPLY       ; }//Prober로부터 Recipe를 전달받으면 Tester에서 응답
            else if(RcvByte[4] == (byte)SocketDefineLinkJ.EN_REP_STATUS_MSG.MEASURE_END        ) {iCmd = (int)SocketDefineLinkJ.EN_CMD_REP.MEASURE_END        ; }//Tester Chip 측정 완료
            else if(RcvByte[4] == (byte)SocketDefineLinkJ.EN_REP_STATUS_MSG.MEASURE_FAIL       ) {iCmd = (int)SocketDefineLinkJ.EN_CMD_REP.MEASURE_FAIL       ; }//Tester에서 측정 실패시 전송
            else if(RcvByte[4] == (byte)SocketDefineLinkJ.EN_REP_STATUS_MSG.TESTER_VISION_READY) {iCmd = (int)SocketDefineLinkJ.EN_CMD_REP.TESTER_VISION_READY; }//Tester에서 Vision 준비완료 송신
            else if(RcvByte[4] == (byte)SocketDefineLinkJ.EN_REP_STATUS_MSG.TESTER_VISION_REPLY) {iCmd = (int)SocketDefineLinkJ.EN_CMD_REP.TESTER_VISION_REPLY; }//Prober로부터 Vision Start 전달 받으면 Tester에서 응답

            Log(false, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD_REP),iCmd)); 

            //if(iCmd == (int)SocketDefineLinkJ.EN_CMD_REP.STATUS_INFO ) m_bRcvStatus = true ;
            //if(iCmd == (int)SocketDefineLinkJ.EN_CMD_REP.TESTER_READY) m_bRcvStatus = true; 

            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;
            return isPass;
        }
        public bool RcvMeasureData(byte[] RcvByte)
        {
            //0x31 : On Chip 측정 Data 송신
            //SOH	Tester Type	  Message Type	  STX	    DATA SIZE	   측정 데이터	EOT	    Length	CR/LF
            //1Byte	1Byte	      1 Byte (0x31)	  1 Byte	4 Bytes	      N Bytes	    1 Byte	N Byte	2Bytes

            int iCmd      = (int)SocketDefineLinkJ.EN_CMD_REP.MEASURE_DATA; 
            bool isPass = false;

            SeprTestData(RcvByte);
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            Log(false, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD_REP),iCmd)); 
            return isPass;

        }
        public bool RcvTesterMode(byte[] RcvByte)
        {
            //0x32 : 설비 모드 송신
            //SOH	     Tester Type	Message Type	Message	  EOT	    CR/LF
            //1 Byte	1 Byte	       1 Byte (0x32)	1 Bytes	  1 Byte	2 Bytes

            int    iCmd     = (int)SocketDefineLinkJ.EN_CMD_REP.TESTER_MODE ; 
            bool   isPass   = false;
            string StrLog;

            m_iMode = SocketDefineLinkJ.EN_TEST_MODE.UNKNOWN   ;

                 if(RcvByte[3] == 0x30) m_iMode = SocketDefineLinkJ.EN_TEST_MODE.MANUAL ; //
            else if(RcvByte[3] == 0x31) m_iMode = SocketDefineLinkJ.EN_TEST_MODE.AUTO   ; //

            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD_REP    ),iCmd     ) + " - " +  
                     Enum.GetName(typeof(SocketDefineLinkJ.EN_TEST_MODE  ),m_iMode  );

            Log(false, StrLog); 

            return isPass;

        }

        public bool RcvTesterStatus(byte[] RcvByte)
        {
            //0x33 : 설비 상태 송신
            //SOH	    Tester Type	  Message Type	   Message	EOT	    CR/LF
            //1 Byte	1 Byte	      1 Byte (0x33)	   1 Bytes	1 Byte	2 Bytes
            int    iCmd     = (int)SocketDefineLinkJ.EN_CMD_REP.TESETER_STATUS   ; 
            bool   isPass   = false;
            string StrLog;
            m_iStatus = SocketDefineLinkJ.EN_TEST_STATUS.UNKNOWN;

                 if(RcvByte[3] == 0x31) m_iStatus = SocketDefineLinkJ.EN_TEST_STATUS.IDLE ; //
            else if(RcvByte[3] == 0x32) m_iStatus = SocketDefineLinkJ.EN_TEST_STATUS.RUN  ; //
            else if(RcvByte[3] == 0x33) m_iStatus = SocketDefineLinkJ.EN_TEST_STATUS.ERROR; //

            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD_REP    ),iCmd     ) + " - " +  
                     Enum.GetName(typeof(SocketDefineLinkJ.EN_TEST_STATUS),m_iStatus);

            Log(false, StrLog); 
            return isPass;

        }


        public bool RcvTesterAlarm(byte[] RcvByte)
        {
            //0x34 : 설비 알람 송신
            //SOH	     Tester Type	Message Type	Message	   EOT	    CR/LF
            //1 Byte	1 Byte	        1 Byte (0x34)	3 Bytes	   1 Byte	2 Bytes
            int    iCmd   = (int)SocketDefineLinkJ.EN_CMD_REP.TESTER_ALARM   ; 
            bool   isPass = false;
            string StrLog;
            m_iError     = SocketDefineLinkJ.EN_ERROR_CODE.SUCESS;

                 if(RcvByte[5] == 0x31) m_iError = SocketDefineLinkJ.EN_ERROR_CODE.ITEM_RECIPE_NOT_EXIST        ; //Tester에 Item_Recipe가 없을 때 발생
            else if(RcvByte[5] == 0x32) m_iError = SocketDefineLinkJ.EN_ERROR_CODE.ITEM_RECIPE_FILE_FORMAT_ERROR; //Item_Recipe의 파일 내용이 잘못 됐을 때 발생
            else if(RcvByte[5] == 0x33) m_iError = SocketDefineLinkJ.EN_ERROR_CODE.BIN_RECIPE_NOT_EXIST         ; //Tester에 Bin_Recipe가 없을 때 발생
            else if(RcvByte[5] == 0x34) m_iError = SocketDefineLinkJ.EN_ERROR_CODE.BIN_RECIPE_FILE_FORMAT_ERROR ; //Bin_Recipe의 파일 내용이 잘못 됐을 때 발생  
            else if(RcvByte[5] == 0x35) m_iError = SocketDefineLinkJ.EN_ERROR_CODE.MEASURE_ALARM                ; //Tester 측정 중 Alarm 발생

            if(m_iError == SocketDefineLinkJ.EN_ERROR_CODE.ITEM_RECIPE_NOT_EXIST        ) cDEF.EPU.SetErr(0610, true);
            if(m_iError == SocketDefineLinkJ.EN_ERROR_CODE.ITEM_RECIPE_FILE_FORMAT_ERROR) cDEF.EPU.SetErr(0610, true);
            if(m_iError == SocketDefineLinkJ.EN_ERROR_CODE.BIN_RECIPE_NOT_EXIST         ) cDEF.EPU.SetErr(0611, true);
            if(m_iError == SocketDefineLinkJ.EN_ERROR_CODE.BIN_RECIPE_FILE_FORMAT_ERROR ) cDEF.EPU.SetErr(0612, true);
            if(m_iError == SocketDefineLinkJ.EN_ERROR_CODE.MEASURE_ALARM                ) 
            {
                cDEF.DM.WAF.SetTo(m_iWorkX,m_iWorkY, EN_CHIP_STAT.Rslt, EN_CHIP_RSLT.Fail);
                cDEF.EPU.SetErr(0613, true);
            }
   
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            StrLog = Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD_REP    ),iCmd     ) + " - " + 
                     Enum.GetName(typeof(SocketDefineLinkJ.EN_ERROR_CODE ),m_iError );

            Log(false, StrLog); 
            return isPass;
        }

        public bool RcvItemInfo(byte[] RcvByte)
        {
            //SOH	TesterType	Message Type	STX	Length	Data Info	EOT	CR/LF
            //1Byte	1Byte   	1 Byte (0x35)	1 Byte	    4 Bytes	    N Byte	  1 Byte	2Bytes

            int iCmd    = (int)SocketDefineLinkJ.EN_CMD_REP.ITEM_INFO; 
            bool isPass = false;

            SeprItemInfo(RcvByte);
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            Log(false, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD_REP),iCmd)); 
            LoadItemName(false);
            return isPass;

        }

        public bool RcvReqPrjName(byte[] RcvByte)
        {
            //SOH	    Tester Type	  Message Type	   EOT	    CR/LF
            //1 Byte	1 Byte	      1 Byte (0x33)	   1 Byte	2 Bytes


            int iCmd    = (int)SocketDefineLinkJ.EN_CMD_REP.PROJECT_NAME; 
            bool isPass = false;

            m_bRcvStatus = true ; //SendProjName(cDEF.FM._sCrntDevice);
            m_bRpyPass[iCmd] = isPass;
            m_bReply  [iCmd] = true  ;

            Log(false, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD_REP),iCmd)); 
            return isPass;

        }

        public void LoadItemName(bool IsLoad)
        {
            String sPath;
            String sFile = "TestItem";
            String sSection = sFile;
            String sName    ;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("Project");
            sPath = Application.StartupPath + "\\Project\\" + sFile + ".INI";

            if (IsLoad)
            {
                sName = sSection + "_MeasureQty   "; ini.Load(sPath, sSection, sName, out m_iMeasureQty);
                for(int i=0;i<vDEF.MAX_TEST_ITEM; i++)
                {
                    sName = sSection + string.Format("_{0}NAME" ,i); ini.Load(sPath, sSection, sName, out TestItem[i].m_strName);
                }
            }
            else 
            {
                sName = sSection + "_MeasureQty   "; ini.Save(sPath, sSection, sName, m_iMeasureQty);
                for(int i=0;i<vDEF.MAX_TEST_ITEM; i++)
                {
                    sName = sSection + string.Format("_{0}NAME" ,i); ini.Save(sPath, sSection, sName, TestItem[i].m_strName);
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


        public void AddTestData()
        {   
            int    bin      = 0x00;
            byte[] StrByteX = new byte[4];
            byte[] StrByteY = new byte[4];
            string strX     = string.Format("{0}{1:000}",(m_iWorkX>0) ? "+":"-", Math.Abs(m_iWorkX));
            string strY     = string.Format("{0}{1:000}",(m_iWorkY>0) ? "+":"-", Math.Abs(m_iWorkY));

            StrByteX  = FNC.GetStringToByteArray(strX,strX.Length);
            StrByteY  = FNC.GetStringToByteArray(strY,strY.Length);

            AddTxData  (StrByteX);
            AddTxData  (StrByteY);

            for(int i=0;i<m_iMaxTestR;i++)   
            {
                for(int j=0;j<m_iMaxTestC;j++)
                { 
                    if(cDEF.FM.ProjBase.iPinNo[i,j]<=0) continue;
                    bin = (cDEF.FM.ProjBase.iPinNo[i,j] > 0)? 0x01 : 0x00; 
                    AddTxByte((byte)bin);
                }
                     
            }
        }

        public void AddScanData()
        {

            int iFindLX, iFindLY;
            int iFindRX, iFindRY;
            int iFindTX, iFindTY;
            int iFindBX, iFindBY;
            int iCX;
            int iCY;
 

            bool bFindL = cDEF.DM.WAF.FindChipX(true , out iFindLX, out iFindLY);
            bool bFindR = cDEF.DM.WAF.FindChipX(false, out iFindRX, out iFindRY);
            bool bFindT = cDEF.DM.WAF.FindChipY(true , out iFindTX, out iFindTY);
            bool bFindB = cDEF.DM.WAF.FindChipY(false, out iFindBX, out iFindBY);

            if (!bFindL || !bFindR || !bFindT || !bFindB) return;
            iCX = Math.Abs(iFindLX);
            iCY = Math.Abs(iFindTY);

            int bin = 0x00;
            byte[] StrByteSizX = new byte[3];
            byte[] StrByteSizY = new byte[3];
            byte[] StrByteCenX = new byte[4];
            byte[] StrByteCenY = new byte[4];

            string strSizX = string.Format("{0:000}"   , Math.Abs(iFindRX - iFindLX)+1);
            string strSizY = string.Format("{0:000}"   , Math.Abs(iFindBY - iFindTY)+1);
            string strCenX = string.Format("{0}{1:000}", (iCX > 0) ? "+" : "-", Math.Abs(iCX+1));
            string strCenY = string.Format("{0}{1:000}", (iCY > 0) ? "+" : "-", Math.Abs(iCY+1));

            StrByteSizX = FNC.GetStringToByteArray(strSizX, strSizX.Length);
            StrByteSizY = FNC.GetStringToByteArray(strSizY, strSizY.Length);
            StrByteCenX = FNC.GetStringToByteArray(strCenX, strCenX.Length);
            StrByteCenY = FNC.GetStringToByteArray(strCenY, strCenY.Length);

            AddTxData(StrByteSizX);
            AddTxData(StrByteSizY);
            AddTxData(StrByteCenX);
            AddTxData(StrByteCenY);

            for (int y = iFindTY; y <= iFindBY; y++) {
                for (int x = iFindLX; x <= iFindRX; x++) {
                    bin = (cDEF.DM.WAF[x,y].IsExist()) ? 0x01 : 0x00;
                    AddTxByte((byte)bin);
                }
            }
        }

        public void SendStatusInfo(SocketDefineLinkJ.EN_STATUS_MSG MSG)
        {//0x30 Prober의 상태 정보 전달 또는 Tester의 상태를 지시함
            string LogMsg;
            //0x30 : 상태 정보 송신 
            //Message Type	    Message	
            //1 Byte (0x30)	    2 Bytes	

            ClearTxData();
            AddTxByte(0x30     );
            AddTxByte(0x30     );
            AddTxByte((byte)MSG);

            LogMsg = Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD       ),(int)SocketDefineLinkJ.EN_CMD.STATUS_INFO) + " - " + 
                     Enum.GetName(typeof(SocketDefineLinkJ.EN_STATUS_MSG),(int)MSG);
            Log(true, LogMsg); 

            Send();
   
        }

        public void SendChipInfo(int ipR, int ipC, int WorkX, int WorkY)
        {//0x30 Prober의 상태 정보 전달 또는 Tester의 상태를 지시함
            //0x31 : On Chip 정보 송신
            //Message Type	   STX	         X 좌표	    Y 좌표	  Die Info
            //1 Byte (0x31)	   1 Byte(0x02)	 4 Bytes	4 Bytes	  N Bytes

            m_iMaxTestR  = ipR    ;
            m_iMaxTestC  = ipC    ;
            m_iWorkX     = WorkX  ;
            m_iWorkY     = WorkY  ;


            ClearTxData();
            AddTxByte  (0x31);
            AddTxByte  (0x02);
            AddTestData(    );

            Log(true, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD),(int)SocketDefineLinkJ.EN_CMD.CHIP_INFO) ); 
            Send();
        }



        public void SendRecipeId(string sItemRecipe, string sBinRecipe)
        {//0x31 On Chip의 좌표 및 On/Skip Chip 정보를 Tester에 전달함
            //0x32 : RECIPE ID 송신
            //Message Type	 STX	        Recipe File Name
            //1 Byte (0x32)	 1 Byte(0x02)	N Bytes
            sItemRecipe += ",";

            byte[] ItemRecipe = new byte[sItemRecipe.Length];
            byte[] BinRecipe  = new byte[sBinRecipe .Length];

            ItemRecipe = FNC.GetStringToByteArray(sItemRecipe,sItemRecipe.Length); 
            BinRecipe  = FNC.GetStringToByteArray(sBinRecipe ,sBinRecipe.Length ); 

            ClearTxData();
            AddTxByte(0x32      );
            AddTxByte(0x02      );
            AddTxData(ItemRecipe);
            AddTxData(BinRecipe );

            Log(true, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD),(int)SocketDefineLinkJ.EN_CMD.RECIPE_ID) ); 
            Send();
        }
        public void SendTimeSynq()
        {//0x33 Prober의 시간을 전달
            //Time Synq 송신(시간 동기화)
            //Message Type	   Message
            //1 Byte (0x33)	   14 Bytes
            String strTime  = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            int    iStrLen  = strTime.Length;

            byte[] StrByte = new byte[iStrLen];
            StrByte        = FNC.GetStringToByteArray(strTime,iStrLen);

            ClearTxData();
            AddTxByte(0x33   );
            AddTxData(StrByte);
            Send();
        }

        public void SendProjName(string PrjName)
        {//0x34 Wafer내의 Chip 개수 정보와 관련된 프로젝트 이름을 전달함
            //0x34 : 프로젝트 이름 송신
            //Message Type	   Message
            //1 Byte (0x34)	   N Bytes
            int    iStrLen  = PrjName.Length;
            byte[] StrByte = new byte[iStrLen];
            StrByte        = FNC.GetStringToByteArray(PrjName,iStrLen); 

            ClearTxData();
            AddTxByte(0x34   );
            AddTxData(StrByte);

            Log(true, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD),(int)SocketDefineLinkJ.EN_CMD.PROJECT_NAME) );
            Send();
        }

        public void SendWaferName(string WafName)
        {//0x35 Wafer Name 전송
            //0x35 : 프로젝트 이름 송신
            //Message Type	   Message
            //1 Byte (0x34)	   N Bytes
            int    iStrLen  = WafName.Length;
            byte[] StrByte = new byte[iStrLen];
            StrByte        = FNC.GetStringToByteArray(WafName,iStrLen); 

            ClearTxData();
            AddTxByte(0x35   );
            AddTxData(StrByte);

            Log(true, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD),(int)SocketDefineLinkJ.EN_CMD.WAFER_NAME) );
            Send();
        }


        public void SendScanInfo()
        {//0x36 Wafer Scan Data 전송
            //0x36 : Wafer Scan  Data  전송 
            //SOH	Tester Type	Message Type	X Size	Y Size	기준점 위치	유효 Chip Info	EOT	    CR/LF	   
            //1Byte	1Byte	    1 Byte (0x31)	3 Bytes	3 Bytes	8 Bytes	    N Bytes	        1 Byte	2Bytes	 


            ClearTxData();
            AddTxByte(0x36);
            AddScanData();


            Log(true, Enum.GetName(typeof(SocketDefineLinkJ.EN_CMD), (int)SocketDefineLinkJ.EN_CMD.WAFER_INFO));
            Send();
        }

        public void Send()
        {
            if(!IsConnected()) return;
            byte[] SendByte = new byte[m_iTxByteCnt + 5];
            Array.Copy(m_TxByte, 0, SendByte, 2, m_iTxByteCnt);

            SendByte[0               ] = 0x01; //SOT
            SendByte[1               ] = (byte)SocketDefineLinkJ.EN_TESTER_TYPE.OPTIC; //Tester Type (0x30: DC, 0x31: OPTIC)
            SendByte[m_iTxByteCnt + 2] = 0x04; //EOT
            SendByte[m_iTxByteCnt + 3] = 0x0D; //CR
            SendByte[m_iTxByteCnt + 4] = 0x0A; //LF

            SendClient(SendByte);
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
                cDEF.LOG.ExceptionTrace("TTesterLinkj. Log " + ex.ToString());
            }
            m_bLockLog = false;
        }

    }


}
