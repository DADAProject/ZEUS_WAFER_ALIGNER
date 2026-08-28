using Drv.CameraController;
using Drv.ImageProcess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace eMachine
{
    public class TCamResultItem
    {
        public EN_VISN_TYPE Type { get; set; }
        public DateTime Time { get; set; }
        public Bitmap Src     { get; set; }
        public Bitmap Overlay { get; set; }
        public int No         { get; set; }
        public bool Result    { get; set; }

        public void Dispose()
        {
            Src?.Dispose();
            Overlay?.Dispose();
            Result = false;
        }
    }

    public class TCamItem : CustomCam
    {
        #region << Fields >>

        public EN_CAM mCamType;
        public bool bUseCam           = true;
        public TSET[] m_SET           = new TSET[(int)EN_VISN_TYPE.EndOfId] ; //vDEF.MAX_VISN_RSLT];
        public TVisnRslt[] m_VisnRslt = new TVisnRslt[(int)EN_VISN_TYPE.EndOfId];
        public TAlgorithm mAlgo       = new TAlgorithm();

        Queue<TCamResultItem> m_QueResult = new Queue<TCamResultItem>();

        #endregion

        private ICamera mCamera;
        public ICamera Cameara { get { return mCamera; } }


        private BUFF mBuffer;
        public BUFF Buffer { get { return mBuffer; } }
        bool   m_bDrng = false;

        private TOnDelayTimer m_tHoldtime = new TOnDelayTimer();

        #region << Constructor && Deconstructor >>
        public TCamItem(EN_CAM pType) 
        {
            mCamType = pType;
            InitializeField();
            //InitializeParam(Defalt)
            InitializeBuffer();
            InitializeCamera();
        }
        //------------------------------------------------------------------------
        protected bool InitializeField()
        {
            for (int i = 0; i < (int)EN_VISN_TYPE.EndOfId; i++)
            {
                m_VisnRslt[i] = new TVisnRslt();
            }

            for (int j = 0; j < (int)EN_VISN_TYPE.EndOfId; j++) // vDEF.MAX_VISN_RSLT; j++)
            {
                m_SET[j] = new TSET();
                m_SET[j].ResetData();
            }

            return true;
        }
        //------------------------------------------------------------------------
        protected bool InitializeBuffer()
        {
            try
            {
               mBuffer = new BUFF(BufferType.Opencv);
               mBuffer.AllocBuffInfo(4096, 3000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Visn Lib Error : {ex.Message}");
                return false;
            }

            return true;
        }
        //------------------------------------------------------------------------
        protected bool InitializeCamera()
        {
            bool bSimMode = false;  //Simulation Mode
            m_bDrng = false;

            try
            {
                string CamName                    = mCamType.ToString();
                cControllerData ctrData           = new cControllerData();
                ctrData.ControllerType            = eControllerType.EVISION;
                ctrData.ControllerName            = "SENTECH";
                ctrData.InitData                  = "MONO";
                ctrData.ControllerID              = 0;

                ctrData.CameraDatas               = new cCameraDatas[1];
                ctrData.CameraDatas[0]            = new cCameraDatas();
                ctrData.CameraDatas[0].ID         = 0;
                ctrData.CameraDatas[0].CameraName = CamName;
                ctrData.CameraDatas[0].CameraType = bSimMode ? eCameraType.TEST_CAM : eCameraType.OMRON;

                //Defalt 셋팅으로 넣어주어야함 강제로  DEfalt 만들어야함.
                ctrData.CameraDatas[0].Param = new cParamData()
                {
                    UseOnlyCameFile = false,
                    TriggerMode     = "On",
                    TriggerSource   = "Software",
                    TriggerSelector = "FrameStart",

                    BufferCount     = 1,
                    ExposureMode    = "Timed",
                    ExposureTime    = 10000,
                    Heartbeat       = 300000000,
                };

                bool bRet = cVision.Instance.Initialize(ctrData);

                mCamera = cVision.Instance[CamName];
                //if (bSimMode) mCamera.SetSimEnable(true, Application.StartupPath + "\\IMAGE\\Top\\");

                return bRet;
            }
            catch (Exception ex) 
            { 
                Debug.WriteLine($"[Exception] InitializeCamera = {ex.Message}"); 
                return false; 
            }
        }

        //------------------------------------------------------------------------
        public void Final()
        {
            mCamera?.Dispose();
            //mBuffer.Dispose();
        }
        #endregion

        #region << Methods >>
        //------------------------------------------------------------------------
        public bool Grab()
        {
            return mCamera.Grab();
        }
        //------------------------------------------------------------------------
        //카메라 & 조명 변경 파라미터
        public bool SetParameter(string sName)
        {
            if (sName == string.Empty)
            {
                //기본 셋팅 사용
                //cVision.Instance[""]?.SetParameter("", ill.dGain.ToString());
                //cVision.Instance[""]?.SetParameter("", ill.dGain.ToString());
                //cVision.Instance[""]?.SetParameter("", ill.dGain.ToString());
                //cVision.Instance[""]?.SetParameter("", ill.dGain.ToString());
            }
            else
            {
                Tillumination.Getillumination(sName);
               //cVision.Instance[""]?.SetParameter("", ill.dGain.ToString());

            }
            return false;
        }
        //------------------------------------------------------------------------
        public void OneAlgoStrt(EN_VISN_TYPE Type, string path = "")
        {
            string sLog = string.Empty;
            m_VisnRslt[(int)Type].ResetData();
            //m_VisnRslt[(int)Type].Item = null;
            m_VisnRslt[(int)Type].CamId = this.mCamType;

            sLog = $"[GRAB START] CAM : {this.mCamType}";
            cDEF.LOG.VisionTrace(sLog);

            //SetParameter(""); //현재 카메라 
            if (path == "")
            {
                var GrabBuffer = mCamera.GrabAndGetReply(TimeSpan.FromMinutes(1));
                if (GrabBuffer == null)
                {
                    m_VisnRslt[(int)Type].Match = false;
                    m_VisnRslt[(int)Type].InspRslt = (int)EN_ERR_LIST.ERR_0073;
                    sLog = $"[GRAB ERROR] CAM :{this.mCamType}";
                    cDEF.LOG.VisionTrace(sLog);
                    return;
                }
                sLog = $"[GRAB END] CAM : {this.mCamType}";
                cDEF.LOG.VisionTrace(sLog);

                mBuffer.ImportBuffInfo(GrabBuffer.ImagePtr, GrabBuffer.Width, GrabBuffer.Height);
            }
            else
            {
                try
                {
                    mBuffer.ImportBuffInfo(path);

                    //화면 갱신 
                    FRM.MOper.FrmCamCtrl.SetSourceImage(mBuffer.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed));
                }
                catch (System.Exception ex)
                {
                    m_VisnRslt[(int)Type].Match = false;
                    m_VisnRslt[(int)Type].InspRslt = (int)EN_ERR_LIST.ERR_0073;
                    sLog = $"[IMAGE ERROR] CAM :{this.mCamType}";
                    cDEF.LOG.VisionTrace(sLog);
                    Debug.WriteLine($"[Exception] OneAlgoStrt - {ex.Message}");
                    return;
                }
                    
            }

            sLog = $"[VISION START] CAM :{this.mCamType} , ALGO : {Type}";
            cDEF.LOG.VisionTrace(sLog);

            try
            {
                switch (Type)
                {
                    case EN_VISN_TYPE.WAlgn:
                        m_VisnRslt[(int)Type] = mAlgo.WAlign(Buffer, m_SET[(int)Type], mCamType);
                        break;
                }
            }
            catch 
            {
                //Detect TimeOut Error
                m_VisnRslt[(int)Type].Match    = false;
                m_VisnRslt[(int)Type].InspRslt = (int)EN_ERR_LIST.ERR_0075;
                sLog = $"[EXCEPTION ERROR] CAM :{this.mCamType}";
                cDEF.LOG.VisionTrace(sLog);
                return;
            }
            
            sLog = $"[VISION END] {m_VisnRslt[(int)Type].ToString()}";
            cDEF.LOG.VisionTrace(sLog);
        }
        //------------------------------------------------------------------------
        public void OneAlgoStrt(string path = "")
        {
            bool isGetImgOk = false;
            string sLog = string.Empty;
            for (int type = 0; type < (int)EN_VISN_TYPE.EndOfId; type++)
            {
                m_VisnRslt[type].ResetData();
                //m_VisnRslt[type].Item = null;
                m_VisnRslt[type].CamId = this.mCamType;
            }
       
            sLog = $"[GRAB START] CAM : {this.mCamType}";
            cDEF.LOG.VisionTrace(sLog);

            //SetParameter(""); //현재 카메라 
            if (path == "")
            {
                var GrabBuffer = mCamera.GrabAndGetReply(TimeSpan.FromMinutes(1));
                isGetImgOk = (GrabBuffer != null);
                if (GrabBuffer == null)
                {
                    SetAllVisnFail(EN_ERR_LIST.ERR_0073);
                    sLog = $"[GRAB ERROR] CAM :{this.mCamType}";
                    cDEF.LOG.VisionTrace(sLog);
                    return;
                }
                sLog = $"[GRAB END] CAM : {this.mCamType}";
                cDEF.LOG.VisionTrace(sLog);
                //
                mBuffer.ImportBuffInfo(GrabBuffer.ImagePtr, GrabBuffer.Width, GrabBuffer.Height);
            }
            else
            {
                try
                {
                    mBuffer.ImportBuffInfo(path);

                    //화면 갱신 
                    FRM.MOper.FrmCamCtrl.SetSourceImage(mBuffer.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed));
                }
                catch (System.Exception ex)
                {
                    SetAllVisnFail(EN_ERR_LIST.ERR_0073);
                    sLog = $"[IMAGE ERROR] CAM :{this.mCamType}";
                    cDEF.LOG.VisionTrace(sLog);
                    Debug.WriteLine($"[Exception] OneAlgoStrt - {ex.Message}");
                    return;
                }
            }
            //
            for (int type = 0; type < (int)EN_VISN_TYPE.EndOfId; type++)
            {
                sLog = $"[VISION START] CAM :{this.mCamType} , ALGO : {(EN_VISN_TYPE)type}";
                cDEF.LOG.VisionTrace(sLog);

                try
                {
                    switch ((EN_VISN_TYPE)type)
                    {
                        case EN_VISN_TYPE.WAlgn:
                            m_VisnRslt[(int)type].SetData = mAlgo.WAlign(Buffer, m_SET[0], isGetImgOk);
                            //m_VisnRslt[(int)type] = mAlgo.WAlign(Buffer, m_SET[0]);
                            break;

                        case EN_VISN_TYPE.FAlgn:
                            m_VisnRslt[(int)type].SetData = mAlgo.FAlign(Buffer, m_SET[0], isGetImgOk);
                            //m_VisnRslt[(int)type] = mAlgo.FAlign(Buffer, m_SET[0]);
                            break;
                    }
                }
                catch
                {
                    //Detect TimeOut Error
                    m_VisnRslt[type].Match = false;
                    m_VisnRslt[type].InspRslt = (int)EN_ERR_LIST.ERR_0075;
                    sLog = $"[EXCEPTION ERROR] CAM :{this.mCamType} Detect Timeout Error";
                    cDEF.LOG.VisionTrace(sLog);
                }  

                sLog = $"[VISION END] {m_VisnRslt[(int)(EN_VISN_TYPE)type].ToString()}";
                cDEF.LOG.VisionTrace(sLog);
            }
        }
        //------------------------------------------------------------------------
        public Bitmap OneThresholdStrt(Bitmap Src, int iLow)
        {
            //티칭시 사용
            //32to 8bit ptr
            using (var Gray = Src.MakeGrayscale())
            {
                var ImagePtr = Gray.GetPtr(System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                BUFF SrcImg = new BUFF(BufferType.Opencv);
                BUFF DstImg = new BUFF(BufferType.Opencv);

                SrcImg.ImportBuffInfo(ImagePtr, Src.Width, Src.Height);
                DstImg.AllocBuffInfo(Src.Width, Src.Height);

                //mAlgo.Threshold(SrcImg, DstImg, iLow);

                var RstImg = DstImg.ToBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                BitmapExtension.ChangeBitmapColor24(RstImg, Color.White, Color.LimeGreen);

                SrcImg.Dispose();
                DstImg.Dispose();

                return RstImg;
            }
        }
        //-------------------------------------------------------------------
        public TVisnRslt GetVisnRslt(int i)
        {
            return m_VisnRslt[i];
        }
        //-------------------------------------------------------------------
        public TVisnRslt GetVisnRslt(EN_VISN_TYPE type)
        {
            return m_VisnRslt[(int)type];
        }
        //-------------------------------------------------------------------
        public void SetAllVisnFail(EN_ERR_LIST error)
        {
            for (int type = 0; type < (int)EN_VISN_TYPE.EndOfId; type++)
            {
                m_VisnRslt[type].Match    = false;
                m_VisnRslt[type].InspRslt = (int)error;

                if (m_VisnRslt[type].Item != null)
                {
                    m_VisnRslt[type].Item?.Dispose();
                    m_VisnRslt[type].Item = null;
                }
            }
        }
        //-------------------------------------------------------------------
        public void Update()
        {
            if (m_tHoldtime.OnDelay(m_bDrng, 2000)) m_bDrng = false; 

            //Vision Result
            if (m_QueResult.Count > 0)
            {
                TCamResultItem m_cResult = m_QueResult.Dequeue();

                //Logging
                //WriteVisnResult(m_cResult, false);
            }
        }
        //-------------------------------------------------------------------
        private void WriteVisnResult(TCamResultItem result, bool verify, bool test = false)
        {
            try
            {
                if (result         == null) return;
                if (result.Src     == null) return;
                if (result.Overlay == null) return;

                if (result.Result)
                {
                    FRM.MOper.FrmCamCtrl.SetOverlayImage(result.Overlay);
                }   if (!cDEF.FM.EngrOptn.bUseImageSave) return; //Save Error Image 

                string sDay  = string.Format("{0:yyMMdd}", DateTime.Now);
                string sPath = string.Format($"{cDEF.FM.EngrOptn.sImageSavePath}\\{sDay}");

                //Make Dir.
                FNC.CreateDir(sPath);

                System.Drawing.Imaging.ImageFormat saveImageFormat1 = System.Drawing.Imaging.ImageFormat.Bmp;
                System.Drawing.Imaging.ImageFormat saveImageFormat2 = System.Drawing.Imaging.ImageFormat.Jpeg;
                string fileNameS = verify ? $"{result.No:D4}_{result.Time:yy-MM-dd_HHmmss.f}_S_V" : $"{result.No:D4}_{result.Time:yy-MM-dd_HHmmss.f}_S_O";
                //string fileNameO = $"{result.No:D4}_{result.Time:yy-MM-dd_HHmmss.f}_O";
                string fileNameL = verify ? $"{result.No:D4}_{result.Time:yy-MM-dd_HHmmss.f}_L_V" : $"{result.No:D4}_{result.Time:yy-MM-dd_HHmmss.f}_L_O";
                if(test)
                {
                    fileNameL = $"TEST_{result.Time:yy-MM-dd_HHmmss.f}_L_O";
                }

                //Debug.WriteLine($"Src, Overay Save - 420");

                if (!test)
                {
                    result.Src?.Save($"{sPath}\\{fileNameS}.{saveImageFormat1}", saveImageFormat1);
                    //result.Overlay?.Save($"{sPath}\\{fileNameO}.{saveImageFormat}", saveImageFormat);
                }

                //
                if (result.Result)
                {                    
                    try
                    {
                        if (result.Src != null)
                        {
                            var last = new Bitmap(result.Src.Width, result.Src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                            using (Graphics g = Graphics.FromImage(last))
                            {
                                g.DrawImage(result.Src, 0, 0);
                                g.DrawImage(result.Overlay, 0, 0);

                                using (Pen pen = new Pen(Brushes.Red, 5))
                                {
                                    g.DrawLine(pen, 0, result.Src.Height / 2, result.Src.Width, result.Src.Height / 2);
                                    g.DrawLine(pen, result.Src.Width / 2, 0, result.Src.Width / 2, result.Src.Height);
                                }
                                g?.Dispose();
                            }
                            last?.Save($"{sPath}\\{fileNameL}.{saveImageFormat2}", saveImageFormat2);
                        }
                    }
                    catch (Exception err)
                    {
                        Debug.WriteLine($"[Exception] Overlay Draw = {err.Message}");
                        cDEF.LOG.ExceptionTrace("[Exception] Overlay Draw", err);
                    }
                }
                //result.Dispose();
                
                //
                int maxSize = cDEF.FM.EngrOptn.nMaxImageStorage;
                //DeleteOldFile(cDEF.FM.EngrOptn.sImageSavePath, sPath, maxSize);
                FNC.DeleteOldFilesWithinThreshold(string.Format($"{cDEF.FM.EngrOptn.sImageSavePath}"), maxSize, 0.10);
                DeleteEmptyFolder(cDEF.FM.EngrOptn.sImageSavePath);
            }
            catch (Exception err) 
            {
                Debug.WriteLine($"[Exception] WriteVisnResult = {err.Message}");
                cDEF.LOG.ExceptionTrace("[Exception] WriteVisnResult", err);
            }
        }
        //--------------------------------------------------------------------------
        private static void DeleteOldFile(string pRoot, string pDirectory, double pMaxSize)
        {
            long size = GetDirectorySize(new DirectoryInfo(pRoot));
            if (size < pMaxSize * 1000000000) return;

            if (Directory.Exists(pRoot))
            {
                string[] directorys = Directory.GetDirectories(pRoot).OrderBy(p => p).ToArray();

                if (directorys.Length > 0)
                {
                    foreach (string dir in directorys) DeleteOldFile(pDirectory, dir, pMaxSize);
                }

                string[] fileNames = Directory.GetFiles(pDirectory);

                List<FileInfo> files = new List<FileInfo>();

                foreach (string path in fileNames)
                {
                    files.Add(new FileInfo(path));
                }

                FileInfo[] sortedFiles = files.OrderBy(p => p.LastWriteTime).ToArray();

                foreach (FileInfo fileInfo in sortedFiles)
                {
                    fileInfo.Delete();
                    size = GetDirectorySize(new DirectoryInfo(pRoot));
                    if (size < pMaxSize * 1000000000) break;
                }
            }

            //DeleteOldFile(pDirectory, pStorageDasy);
        }
        //--------------------------------------------------------------------------
        private static bool DeleteEmptyFolder(string pDirectory)
        {
            if (Directory.Exists(pDirectory)) //저장 경로 유효 성 판단
            {
                string[] fileNames = Directory.GetFiles(pDirectory);
                string[] directorys;
                while (true)
                {
                    directorys = Directory.GetDirectories(pDirectory);

                    if (directorys.Length > 0)
                    {
                        bool existFile = false;
                        foreach (string dir in directorys)
                        {
                            if (DeleteEmptyFolder(dir) == false) existFile = true;
                        }
                        if (existFile == true) break;
                    }
                    else
                    {
                        break;
                    }
                }
                if (fileNames.Length == 0 && directorys.Length == 0)
                {
                    Directory.Delete(pDirectory);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------
        private static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long sum = 0;
            var files = directoryInfo.EnumerateFiles();
            var directories = directoryInfo.EnumerateDirectories();
            foreach (var file in files)
                sum += file.Length;
            foreach (var directory in directories)
                sum += GetDirectorySize(directory);
            return sum;
        }

        //======================================
        public void VisionResult(TCamResultItem result, bool verify, bool test = false)
        {
            //m_QueResult.Enqueue(result);
            WriteVisnResult(result, verify, test);
        }
        #endregion
    }
}
