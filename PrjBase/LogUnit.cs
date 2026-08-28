using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Collections;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TLogUnit                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    
    public enum EN_LOG_TYPE
    {
        EVENT    = 0,
        EXCEPTION   ,
        JAM         ,
        RESULT      ,
        SEQ         ,
        RS232       ,
        LOT         ,
        VISION      ,
        TCPIP       ,
        RFID        ,
        Barcode     ,
        TEST        ,

        EndofList
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    /************************************************************************/
    /* LOG Structure                                                        */
    /************************************************************************/
    public struct ST_LOG_INFO
    {
        public EN_LOG_TYPE eType    ;
        public string      sMsg     ;
        public string      sdt      ; //Time
        public EN_SEQ_ID   iPart    ;
        public string      sFilePath;
        public bool        bDisplay ;

        public ST_LOG_INFO(string log)
        {
            eType     = EN_LOG_TYPE.EVENT;
            sMsg      = log;
            sdt       = string.Format($"{DateTime.Now:HH:mm:ss}");
            iPart     = EN_SEQ_ID.ALL;
            sFilePath = string.Empty;
            bDisplay  = false;
        }

        public void Reset()
        {
            eType     = EN_LOG_TYPE.EVENT;
            sMsg      = string.Empty ;
            sdt       = string.Empty ;
            iPart     = EN_SEQ_ID.ALL;
            sFilePath = string.Empty ;
            bDisplay  = false;
        }
    }
    //---------------------------------------------------------------------------
    public class TLogUnit
    {
        //
        public delegate void DisplayLogEventDelegate(string       pOut);
        public delegate void DisplayVisnDataDelegate(ListViewItem pOut);
        
        //
        public event DisplayLogEventDelegate DisplayLogEvent  ;
        public event DisplayLogEventDelegate DisplayComEvent  ;
        public event DisplayLogEventDelegate DisplayTraceEvent;
        public event DisplayLogEventDelegate DisplayRFIDEvent ;
        
        public event DisplayVisnDataDelegate DisplayResult    ;

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        bool m_bUpdateLog;
        
        Queue<ST_LOG_INFO > m_Que       = new Queue<ST_LOG_INFO>();
        Queue<ListViewItem> m_QueResult = new Queue<ListViewItem>();
        private readonly object m_lock = new object();
        ST_LOG_INFO         stLog       = new ST_LOG_INFO("");
        
        private Thread     m_threadLog;   // Log Thread

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLogUnit()
        {
            Init();
            
            //
            m_bUpdateLog = true;
            m_threadLog  = new Thread(new ThreadStart(UpdateLog)); // 로그 쓰레드 실행.
            m_threadLog.Start();

        }
        ~TLogUnit() {}

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init()
        {//UserSet - 자동 생성 Log Folder 처리 

            FNC.CreateDirOnWork("LOG");
            EN_LOG_TYPE logtype = EN_LOG_TYPE.EVENT; 
            for (int n = 0; n < (int)EN_LOG_TYPE.EndofList; n++)
            {
                FNC.CreateDirOnWork($"LOG\\{logtype++.ToString()}\\");
            }

            //Add
            FNC.CreateDirOnWork("IMAGE\\");

        }
        //Del Log.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  KillPast()
        {//UserSet - Log File Delete 처리  
            
            EN_LOG_TYPE logtype = EN_LOG_TYPE.EVENT;
            for (int n = 0; n < (int)EN_LOG_TYPE.EndofList; n++)
            {
                FNC.DelDirFrDate($"LOG\\{logtype++.ToString()}\\", DateTime.Now.AddDays(-30));
            }

            //이미지 파일 삭제
            FNC.DelDirFrDate(cDEF.FM.EngrOptn.sImageSavePath, DateTime.Now.AddDays(-cDEF.FM.EngrOptn.iMaxImageDay));
        }
        //---------------------------------------------------------------------------
        public void KillThread()
        {
            if (m_threadLog.IsAlive)
            {
                m_bUpdateLog = false ;
                if (m_threadLog.Join(1000)) m_threadLog.Abort();
            }
        }
        //---------------------------------------------------------------------------
        public string GetNowTimeFormat()
        {
            return DateTime.Now.ToString("HH:mm:ss.fff");
        }

        //Make Log.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Trace(string msg)
        {
            if (msg == "" || msg == string.Empty) return;

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.EVENT;
            log.sMsg        = msg;
            log.sdt         = GetNowTimeFormat();

            if (string.IsNullOrEmpty(log.sdt)) return;
            if (string.IsNullOrEmpty(msg    )) return;
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_Trace ExceptionTrace " + ex.ToString());
            }
            //if(display) DisplayTraceEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
        }
        //------------------------------------------------------------------------
        //[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
		//public void  Trace           (String format, params object[] args)
        //{
        //    //
        //    ST_LOG_INFO log = new ST_LOG_INFO("");
        //    log.eType       = EN_LOG_TYPE.EVENT; 
        //    log.sMsg        = String.Format(format, args);
        //    log.sdt         = GetNowTimeFormat();
        //    m_Que.Enqueue(log);
        //}
        //---------------------------------------------------------------------------
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
		public void  JamTrace (int No , DateTime SetTime , String Name , int Part , int Kind)
        {
            //
            string sTemp;
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType = EN_LOG_TYPE.JAM;

            sTemp = string.Format("ERR_{0:0000},", No);
            sTemp += SetTime + ",";
            sTemp += string.Format("{0:00}", Part) + ",";
            sTemp += string.Format("{0:00}", Kind) + ",";
            sTemp += Name ;

            log.sMsg = sTemp; 
            log.sdt = GetNowTimeFormat();
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_JamTrace ExceptionTrace " + ex.ToString());
            }
        }
        //------------------------------------------------------------------------
        public void SeqTrace(string msg)
        {
            if (msg == "" || msg == string.Empty) return; 

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.SEQ;
            log.sMsg        = msg;
            log.sdt         = GetNowTimeFormat();

            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_SeqTrace ExceptionTrace " + ex.ToString());
            }
            //DisplayLogEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
        }
        //------------------------------------------------------------------------
        public void VisionTrace(string msg, string time = "")
        {
            if (msg == "" || msg == string.Empty) return;

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.VISION;
            log.sMsg        = msg;
            log.sdt         = time == "" ? GetNowTimeFormat() : time;
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_VisionTrace ExceptionTrace " + ex.ToString());
            }
        }
        //------------------------------------------------------------------------
        public void VisionResult(TVisnRslt result, DateTime start, DateTime end, EN_VISN_STEP mode = EN_VISN_STEP.ALIGN) 
        {
            //ListViewItem 
            ListViewItem item = new ListViewItem($"{result.No}");
            item.SubItems.Add(start.ToString("MM-dd HH:mm:ss.f"));
            item.SubItems.Add(end.ToString("MM-dd HH:mm:ss.f"));
            item.SubItems.Add($"{(end - start).TotalMilliseconds} ms");

            item.SubItems.Add($"{Math.Round(result.X, 4)}");
            item.SubItems.Add($"{Math.Round(result.Y, 4)}");
            item.SubItems.Add($"{Math.Round(result.T, 4)}");
            
            item.SubItems.Add($"{result.Mode}");
            item.SubItems.Add($"{result.Type}");

            item.SubItems.Add($"{result.BarCode }");
            item.SubItems.Add($"{mode}");

            if (result.Match && result.InspRslt == (int)EN_ERR_LIST.ERR_NONE) item.SubItems.Add("Completed");
            else
            {
                item.SubItems.Add(cDEF.EPU.GetName(result.InspRslt));
            }
            try
            {
                lock (m_lock)
                {
                    m_QueResult.Enqueue(item);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_VisionResult ExceptionTrace " + ex.ToString());
            }
            

            //Text logging
            string sTemp = string.Format($"[{mode}] {result.ToString()},{start.ToString("MM-dd HH:mm:ss.f")},{end.ToString("MM-dd HH:mm:ss.f")}");
            VisionResult(sTemp);
}
        //------------------------------------------------------------------------
        public void VisionResult(string msg)
        {
            if (msg == "" || msg == string.Empty) return;

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.RESULT;
            log.sMsg        = msg;
            log.sdt         = GetNowTimeFormat() ;
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_VisionResult ExceptionTrace " + ex.ToString());
            }
        }
        //------------------------------------------------------------------------
        public void TCPIPTrace(string msg)
        {
            if (msg == "" || msg == string.Empty) return;

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.TCPIP;
            log.sMsg        = msg;
            log.sdt         = GetNowTimeFormat();
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_TCPIPTrace ExceptionTrace " + ex.ToString());
            }

            //DisplayComEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void RFIDTrace(string msg, bool display = true)
        {
            if (msg == "" || msg == string.Empty) return;

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.RFID;
            log.sMsg        = msg;
            log.sdt         = GetNowTimeFormat();
            log.bDisplay    = display;
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_RFIDTrace ExceptionTrace " + ex.ToString());
            }
            //if(display) DisplayRFIDEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void BarcodeTrace(string msg, bool display = true)
        {
            if (msg == "" || msg == string.Empty) return;

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.Barcode;
            log.sMsg        = msg;
            log.sdt         = GetNowTimeFormat();
            log.bDisplay    = display;
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_BarcodeTrace ExceptionTrace " + ex.ToString());
            }
        }
        //------------------------------------------------------------------------
        public void RS232Trace(string msg)
        {
            if (msg == "" || msg == string.Empty) return;

            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.RS232;
            log.sMsg        = msg;
            log.sdt         = GetNowTimeFormat();
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_RS232Trace ExceptionTrace " + ex.ToString());
            }
        }
        //---------------------------------------------------------------------------
        public void  CycleTrace (String Name , String Lot , double Min , double Max , double Avg , int Cnt)
        {

        }
        //---------------------------------------------------------------------------
        public void  ExceptionTrace  (String Msg ,Exception Ex)
        {
            //
            string sTemp;
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.EXCEPTION;

            sTemp    = string.Format($"{Msg} {Ex.Message}\r\n{Ex.StackTrace}");
            log.sMsg = sTemp;
            log.sdt  = GetNowTimeFormat();
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_ExceptionTrace ExceptionTrace " + ex.ToString());
            }


            //ExceptionTrace(string.Format("{0} {1}\r\n{2}",Msg,Ex.Message,Ex.StackTrace));
        }
        //---------------------------------------------------------------------------
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]    
		public void  ExceptionTrace  (String Msg)
        {
            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.EXCEPTION;
            log.sMsg        = Msg;
            log.sdt         = GetNowTimeFormat();
            try
            {
                lock (m_lock)
                {
                    m_Que.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                ExceptionTrace("TLogUnit_ExceptionTrace ExceptionTrace " + ex.ToString());
            }

        }
        //------------------------------------------------------------------------

        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        public void ExceptionTraceLog(string Msg)
        {
            //Local Var.           
            string sPath;
            string sTemp;
            string sFile = "[" + string.Format("{0:yyMMdd}", DateTime.Now)+ "]" + "Exception.txt"; 

            //Make Dir.
            FNC.CreateDirOnWork("LOG");
            FNC.CreateDirOnWork("LOG\\Exception");
            sPath = Application.StartupPath + "\\LOG\\Exception\\" + sFile;
            
            try 
            {
                //File Open.
                using (Stream stream = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sTemp = "[" + string.Format("{0:HH:mm:ss}", DateTime.Now) + "]" + Msg + "\r\n";
                    sw.Write(sTemp);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                //ExceptionTrace("TLogUnit. ExceptionTrace " + ex.ToString());
                System.Diagnostics.Debug.WriteLine("ExceptionTraceLog : " + ex.Message);
            } 
            
        }
        //---------------------------------------------------------------------------
        public void UpdateLog()
        {
            try
            {
                while(m_bUpdateLog)
                {
                    Thread.Sleep(10);  // Sleep 500ms

                    ST_LOG_INFO? nextLog = null;
                    lock (m_lock)
                    {
                        if (m_Que.Count > 0)
                        {
                            nextLog = m_Que.Dequeue();
                        }
                    }

                    if (nextLog.HasValue)
                    {
                        //Logging
                        WriteLog(nextLog.Value);
                        //Release
                    }
                    
                    //Vision Data
                    ListViewItem nextResult = null;
                    lock (m_lock)
                    {
                        if (m_QueResult.Count > 0)
                        {
                            nextResult = m_QueResult.Dequeue();
                        }
                    }

                    if (nextResult != null)
                    {
                        //Logging
                        WriteLogResult(nextResult);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionTraceLog("[UpdateLog]"+ex.Message);
                Debug.WriteLine($"UpdateLog Exception : {ex.Message}");

                throw;
            }
        }
        //---------------------------------------------------------------------------
        private void WriteLog(ST_LOG_INFO log)
        {
            switch (log.eType)
            {  
                case EN_LOG_TYPE.EVENT:
                    DisplayTraceEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
                    break;
                case EN_LOG_TYPE.EXCEPTION:
                    break;
                case EN_LOG_TYPE.JAM:
                    break;
                case EN_LOG_TYPE.RESULT:
                    break;
                case EN_LOG_TYPE.RS232:
                    break;
                case EN_LOG_TYPE.LOT:
                    break;
                case EN_LOG_TYPE.VISION:
                    break;
                case EN_LOG_TYPE.TEST:
                    break;
                case EN_LOG_TYPE.SEQ:
                    DisplayLogEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
                    if (cDEF.FM.SysOptn.iSkipSeqLog == 1) return; //Log 미사용
                    break;
                case EN_LOG_TYPE.TCPIP:
                    DisplayComEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
                    break;
                case EN_LOG_TYPE.RFID:
                    DisplayRFIDEvent?.Invoke(string.Format($"[{log.sdt}] {log.sMsg}"));
                    break; 

                default:
                    break;
            }

            //
            WriteLogToFile(log);

        }
        //--------------------------------------------------------------------------
        private void WriteLogResult(ListViewItem result)
        {
            DisplayResult?.Invoke(result);
        }
        //---------------------------------------------------------------------------
        private void WriteLogToFile(ST_LOG_INFO log)
        {
            if (string.IsNullOrEmpty(log.sdt )) return;
            if (string.IsNullOrEmpty(log.sMsg)) return;

            //Local Var.
            string sPath, sTemp;
            string sFile = string.Format($"[{DateTime.Now:yyMMdd}] {log.eType.ToString()}.txt");

            //Make Dir.
            FNC.CreateDirOnWork("LOG");
            FNC.CreateDirOnWork($"LOG\\{log.eType.ToString()}");
            
            sPath = string.Format($"{Application.StartupPath}\\LOG\\{log.eType.ToString()}\\{sFile}");

            //File Open.
            try
            {
                using (Stream stream = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(stream, Encoding.Unicode);
                    sw.BaseStream.Seek(0, SeekOrigin.End);

                    sTemp = string.Format($"[{log.sdt}] {log.sMsg} \r\n");
                    
                    sw.Write(sTemp);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"TLogUnit. {log.eType.ToString()} {ex.ToString()}");
                ExceptionTrace($"TLogUnit. {log.eType.ToString()} {ex.ToString()}");
            }
        }
        //------------------------------------------------------------------------
        public void TraceTest(String format)
        {
            //
            ST_LOG_INFO log = new ST_LOG_INFO("");
            log.eType       = EN_LOG_TYPE.EVENT;
            log.sMsg        = String.Format(format);
            log.sdt         = GetNowTimeFormat();
            lock (m_lock)
            {
                m_Que.Enqueue(log);
            }
        }
    }
}
