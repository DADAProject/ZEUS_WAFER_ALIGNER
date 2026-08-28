using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using Drv.Control;
using Drv.CameraController;
using System.Windows.Navigation;

namespace eMachine
{
    public enum EN_DISPLAY : int
    {
        None = -1,
        View   ,
        Teach  ,
        ViewAndTeach,
        EndofDisplay
    }


    public partial class FrmCamDx : UserControl
    {
        #region << API32 >>

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        #endregion

        #region << Fields >>
        private cCtrDisplayTracker _VisView;
        private EN_CAM _eCamera;
        private EN_DISPLAY _eType;
        private Stopwatch _FpsTimer;
        private Stopwatch _FrameTimer;
        private bool actived;
        private int FrameInterval;
        private bool IsLive;
        private System.Timers.Timer bwLive;
        private System.Threading.Thread thFrame;
        bool bThreadAbort = new bool();
        private readonly ConcurrentQueue<Bitmap> mFrameDataQueue = new ConcurrentQueue<Bitmap>();
        private readonly ConcurrentQueue<Bitmap> mOverrideDataQueue = new ConcurrentQueue<Bitmap>();
        #endregion

        #region << Properties >>
        public bool Actived
        {
            get { return actived; }
            set
            {
                if (actived != value)
                {
                    actived = value;
                }
            }
        }

        public float ZoomFactor
        {
            get { return _VisView.ZoomFactor ; }
            set
            {
                if (_VisView.ZoomFactor != value)
                {
                    _VisView.ZoomFactor = value;
                }
            }
        }
        public EN_CAM Camera
        {
            set { _eCamera = value; btnName.Text = _eCamera.ToString();}
            get { return _eCamera; }
        }
        public IShape Tracker
        {
            //1개만 생성하여 사용
            get { return _VisView.GetTracker(0); }      
        }


        #endregion

        #region << Constructor >>
        public FrmCamDx(EN_DISPLAY pType)
        {
            InitializeComponent();

            _VisView                     = new cCtrDisplayTracker ();
            _VisView.Name                = "VisView";
            _VisView.Location            = new Point(1, 33);
            _VisView.Size                = new Size(this.Width - _VisView.Location.X, this.Height - _VisView.Location.Y);
            _VisView.Anchor              = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            _VisView.BackColor           = System.Drawing.Color.LightGray;
            _VisView.MiniMapScale        = 0.2F;
            _VisView.ShowMenu            = false;
            _VisView.ShowMiniMap         = true;
            _VisView.ShowMousePosition   = false;
            _VisView.CanContextMenuStrip = false;
            _VisView.UseCrossBar         = false;
            _VisView.ZoomFactor          = 0.1F;
            _VisView.Visible             = true;
            //_VisView.Dock              = DockStyle.Fill;

            _FpsTimer = new Stopwatch();
            _FrameTimer = Stopwatch.StartNew();
            bwLive = new System.Timers.Timer();
            bwLive.Elapsed += new System.Timers.ElapsedEventHandler(bwLive_DoWork);
            thFrame =  new Thread(new ThreadStart(ThrdExcute0));
            thFrame.IsBackground = true;
            thFrame.Priority = ThreadPriority.Lowest;
            thFrame.Start();

            FrameInterval   = 200; //이벤트 부하
            bwLive.Interval = FrameInterval; //Live Grab Interval

            _eType   = pType;
            _eCamera = EN_CAM.WTB;
            if (_eType == EN_DISPLAY.Teach) btnName.Text = "Teach Display";
            else                            btnName.Text = _eCamera.ToString();

            btnName.ForeColor = Color.Black;
            this.AllowDrop = true;
            this.Visible   = false;
            Controls.Add(_VisView);

            if (_eType != EN_DISPLAY.View)
            {
                cShapeRectangle Rect = new cShapeRectangle(0, 0, 0, 0, 0);
                Rect.AvailableAngle = false;
                _VisView.AddTracker(Rect);
            }
        }

        ~FrmCamDx()
        { 
        
        }

        #endregion

        #region << Events >>
        private void FrmCam_Load(object sender, EventArgs e)
        {
            //
            try
            {
                if (_VisView == null) return;
                _VisView.ScaleSet(cCtrImageDisplayDx.eSizeMenu.ZoomSize);
            }
            catch (Exception ex)
            {
                 cDEF.LOG.ExceptionTrace("FrmCam2_Load" + ex.ToString());
            }
        }

        public void FrmCam_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Visible)
                {//Show
                    if (_VisView == null) return;
                    _VisView.ScaleSet(cCtrImageDisplayDx.eSizeMenu.ZoomSize);
                    timer1.Enabled = true;

                     foreach (var item in cVision.Instance.GetControllerAll())
                     {
                         item.GrabEvent += Camera_GrabEvent;
                     }
                }
                else
                {//Hide
                    timer1.Enabled = false;

                    foreach (var item in cVision.Instance.GetControllerAll())
                    {
                        item.GrabEvent -= Camera_GrabEvent;
                    }
                    //
                }

                actived = this.Visible;
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("FrmCam2_VisibleChanged" + ex.ToString());
            }
        }

        private void FrmCam_SizeChanged(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_VisView == null) return;
            btnLive.BackColor = IsLive ? Color.Yellow : SystemColors.ButtonFace;

            btnGrabOne.Enabled = !cDEF.SEQ._bRun;
            btnLive   .Enabled = !cDEF.SEQ._bRun;
            btnOpen   .Enabled = !cDEF.SEQ._bRun;
            btnSave   .Enabled = !cDEF.SEQ._bRun;


            if (_eType == EN_DISPLAY.ViewAndTeach)
            {
                btnLive.Enabled = true;
                btnLightOne.Enabled = false;
            }


            if (_eCamera == EN_CAM.None)
            {
                btnGrabOne.Enabled = false;
                btnLightOne.Enabled = false;
                btnLive.Enabled    = false;
            }
        }


        private void bwLive_DoWork(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!this.Visible                   ) { this.bwLive.Enabled = false; IsLive = false; return; }
            if (cDEF.SEQ._bRun || cDEF.MAN._bRun) { this.bwLive.Enabled = false; IsLive = false; return; }

            bwLive.Enabled = false;

            if ((int)_eCamera < 0) return;
            if (cDEF.VISN.Cam[(int)_eCamera] != null)
            {
                if (cDEF.VISN.Cam[(int)_eCamera].Grab())
                    _FpsTimer.Restart();
            
                bwLive.Enabled = IsLive;
            }
            else
            {
                Invalidate();
            }
        }

        public void ThrdExcute0()
        {
            try
            {
                while (!bThreadAbort)
                {
                    if (mFrameDataQueue.Count > 0)
                    {
                        if (mFrameDataQueue.TryDequeue(out Bitmap FrameData))
                        {
                            _VisView.Invoke(new MethodInvoker(delegate ()
                            {
                                _VisView.BaseImage = FrameData;
								//260521 //AOCV
                                if (m_pendingDrawROIs)
                                {
                                    DrawAllROIs_Internal();
                                    m_pendingDrawROIs = false;
                                }
                            }));
                        }
                    }

                    if (mOverrideDataQueue.Count > 0)
                    {
                        if (mOverrideDataQueue.TryDequeue(out Bitmap FrameData))
                        {
                            _VisView.Invoke(new MethodInvoker(delegate ()
                            {
                                _VisView.OverlayImage = FrameData;
                            }));
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Thread0] Exception - {ex.Message}");
                cDEF.LOG.ExceptionTrace("Thread0", ex);
            }
        }

        private void FrmCam_DragEnter(object sender, DragEventArgs e)
        {
            if (_VisView == null) return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void FrmCam_DragDrop(object sender, DragEventArgs e)
        {
            if (_VisView == null) return;

            //파일 확인 
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                string GetExtension = Path.GetExtension(file);

                if (GetExtension.Equals(".bmp") || GetExtension.Equals(".jpg"))
                {
                    _VisView.BaseImage = LoadImageFromStream(file);
                    DrawAllROIs_Internal(); //260521 //AOCV
                    Refresh();
                    break;
                }
            }
        }

        #endregion

        #region << User Click Events >>
        private void btnZommPlus_Click(object sender, EventArgs e)
        {
            if (_VisView == null) return;
            _VisView.CanImageContol = true;
            _VisView.Zoom(new Point(_VisView.Size.Width / 2, _VisView.Size.Height / 2), true);
            _VisView.CanImageContol = false;
            _VisView.Refresh();
        }

        // Zoom Out
        private void btnZoomMinus_Click(object sender, EventArgs e)
        {
            if (_VisView == null) return;
            _VisView.CanImageContol = true;
            _VisView.Zoom(new Point(_VisView.Size.Width / 2, _VisView.Size.Height / 2), false);
            _VisView.CanImageContol = false;
            _VisView.Refresh();
        }

        // Zoom All
        private void btnZoomAll_Click(object sender, EventArgs e)
        {
            if (_VisView == null) return;
            _VisView.ScaleSet(cCtrImageDisplayDx.eSizeMenu.ZoomSize);
            _VisView.Refresh();
        }

        // 1개 이미지 취득
        private void btnGrabOne_Click(object sender, EventArgs e)
        {
           if ((int)_eCamera < 0)                    return;
           if (_VisView == null)                     return;
           if (cDEF.VISN.Cam[(int)_eCamera] == null) return;
           
           if (IsLive)
           {
               LiveStop();
           }
            ClearOverlayImage();

            cDEF.VISN.Cam[(int)_eCamera].SetParameter("");
            cDEF.VISN.Cam[(int)_eCamera].Grab();
        }

        // 라이브
        private void btnLive_Click(object sender, EventArgs e)
        {
            if (_VisView == null) return;

            ClearOverlayImage();

            if (IsLive)
            {
                LiveStop();
            }
            else
            {
                LiveStart();
            }
        }

        // 등록 패턴 보기 클릭
        private void btnPatVisible_Click(object sender, EventArgs e)
        {
            //Test
           // SetOverlayImage(new Point(0, 0), new Point(100, 100), new Pen(Color.FromArgb(255, 0, 0, 255), 190));
        }

        // 이미지 열기
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (_VisView == null) return;

            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Filter = "[BMP FILE]|*.bmp|[JPEG FILE]|*.jpg|[ALL FILE]|*.*";
                FileDialog.DefaultExt = "*.bmp";
                FileDialog.Title = "이미지 불러오기";

                if (FileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadImage(FileDialog.FileName);
                    DrawAllROIs_Internal(); //260521 //AOCV
                }
            }
        }

        // 이미지 저장
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_VisView == null) return;

            using (SaveFileDialog FileDialog = new SaveFileDialog())
            {
                FileDialog.Filter = "Image File (*.png)|*.png";
                FileDialog.DefaultExt = ".bmp";
                FileDialog.Title = "이미지 저장";
                if (DialogResult.OK != FileDialog.ShowDialog()) return;

                using (System.IO.FileStream sw = new System.IO.FileStream(FileDialog.FileName,
                    System.IO.FileMode.Create, FileAccess.Write))
                {
                    Bitmap temp = ConvertTo32bpp(_VisView.BaseImage);
                    temp.Save(sw, System.Drawing.Imaging.ImageFormat.Png);
                    temp.Dispose();
                }
            }
        }

        //Cross Line Visible Flag
        private void btnCrossLine_Click(object sender, EventArgs e)
        {
            if (_VisView == null) return;

            _VisView.UseCrossBar = !_VisView.UseCrossBar;

            Refresh();
        }

        private void btnPan_Click(object sender, EventArgs e)
        {
            btnSelect.Checked = false;
            btnPan.Checked = true;
            _VisView.CanImageContol = true;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            btnSelect.Checked = true;
            btnPan.Checked = false;
            _VisView.CanImageContol = false;
        }


        private void btnLightOne_Click(object sender, EventArgs e)
        {
            
        }
        private void btnCam_Click(object sender, EventArgs e)
        {
            btnName.Text = _eCamera.ToString();
        }

        #endregion

        #region << Methods >>
        public void SetBtnVisible(bool visible)
        {
            this.Toolbar.Visible = visible;
        }
        //--------------------------------------------------------------------------
        public Bitmap GetCurrentImage()
        {
           return  _VisView.BaseImage;
        }
        //--------------------------------------------------------------------------
        public void LoadImage(string sPath)
        {
            Bitmap temp = ConvertTo32bpp(LoadImageFromStream(sPath));
            _VisView.BaseImage = temp;
            _VisView.ScaleSet(cCtrImageDisplayDx.eSizeMenu.ZoomSize);
            Refresh();
        }
        //--------------------------------------------------------------------------
        public void LoadImage(Bitmap pBmp)
        {
            Bitmap temp = ConvertTo32bpp(pBmp);
            _VisView.BaseImage = temp;
            _VisView.ScaleSet(cCtrImageDisplayDx.eSizeMenu.ZoomSize);
            Refresh();
        }
        //--------------------------------------------------------------------------
        public void LiveStart()
        {
            if (!IsLive)
            {
                IsLive = true;
                bwLive.Enabled = true;
            }
        }
        //--------------------------------------------------------------------------
        public void LiveStop()
        {
            if (IsLive)
            {
                IsLive = false;
                bwLive.Enabled = false;
            }
        }
        //--------------------------------------------------------------------------
        //Drawing Line
        private bool SetOverlayImage(bool overlay = false)
        {
            if (_VisView.BaseImage == null) return false;

            //Transparent Image
            _VisView.Invoke(new MethodInvoker(delegate ()
            {
				//260521 //AOCV
                if (_VisView.OverlayImage == null || 
                    _VisView.OverlayImage.PixelFormat == PixelFormat.DontCare ||
                    _VisView.OverlayImage.Width != _VisView.BaseImage.Width || 
                    _VisView.OverlayImage.Height != _VisView.BaseImage.Height)
                {
                    if (_VisView.OverlayImage != null) _VisView.OverlayImage.Dispose();
                    _VisView.OverlayImage = new Bitmap(_VisView.BaseImage.Width, _VisView.BaseImage.Height, PixelFormat.Format32bppArgb);
                }

                if (!overlay) //Add Drawing
                {
                    using (var graphics = Graphics.FromImage(_VisView.OverlayImage))
                    {
                        graphics.Clear(Color.FromArgb(255,0,0,0));
                    }
                }
            }));
            return true;
        }
        //--------------------------------------------------------------------------
        public void ClearOverlayImage()
        {
            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                if (_VisView.BaseImage    == null || 
                    _VisView.OverlayImage == null || 
                    _VisView.BaseImage.PixelFormat == PixelFormat.DontCare) return;

                _VisView.OverlayImage?.Dispose();
                _VisView.OverlayImage = new Bitmap(_VisView.BaseImage.Width, _VisView.BaseImage.Height, PixelFormat.Format32bppArgb);
            }));
        }
        //--------------------------------------------------------------------------
        public void SetOverlayImage(Bitmap Bmp)
        {
            if (Bmp == null) return;
            if (_VisView.BaseImage == null) return;
            try
            {
                _VisView.Invoke(new MethodInvoker(delegate ()
                {
                    _VisView.OverlayImage?.Dispose();
                    _VisView.OverlayImage = Bmp.Clone() as Bitmap;
                }));
            }
            catch(Exception e)
            {
                Debug.WriteLine($"[Exception ] SetOverlayImage :{e.Message}");
            }

        }
        //--------------------------------------------------------------------------
        public void SetSourceImage(Bitmap Bmp)
        {
            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                _VisView.BaseImage?.Dispose();
                _VisView.BaseImage = Bmp.Clone() as Bitmap;
            }));
        }
        //--------------------------------------------------------------------------
        public void DrawLine(Pen pen, PointF pt1, PointF pt2, bool overlay = false)
        {
            if(!_VisView.Visible) return;

            if (!SetOverlayImage(overlay)) return;

            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                using (var graphics = Graphics.FromImage(_VisView.OverlayImage))
                {
                    graphics.DrawLine(pen, pt1, pt2);
                }

                //Refresh ovverade 
                _VisView.OverlayImage = _VisView.OverlayImage.Clone() as Bitmap;

            }));   
        }
        //--------------------------------------------------------------------------
        public void DrawCrossLine(Pen pen, PointF pt, float length, bool overlay = false)
        {
            if (!_VisView.Visible) return;
            if (!SetOverlayImage(overlay)) return;

            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                try
                {
                    if (_VisView.OverlayImage == null) return;
                    using (var graphics = Graphics.FromImage(_VisView.OverlayImage))
                    {
                        graphics.DrawLine(pen, new PointF(pt.X - length, pt.Y), new PointF(pt.X + length, pt.Y));
                        graphics.DrawLine(pen, new PointF(pt.X, pt.Y - length), new PointF(pt.X, pt.Y + +length));
                    }

                    //Refresh ovverade 
                    _VisView.OverlayImage = _VisView.OverlayImage.Clone() as Bitmap;
                }
                catch (Exception)
                {

                }
              

            }));

        }
        //--------------------------------------------------------------------------
        public void DrawCircle(Pen pen, PointF ptCenter, float radius, bool overlay = false)
        {
            if (!_VisView.Visible) return;
            if (!SetOverlayImage(overlay)) return;

            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                using (var graphics = Graphics.FromImage(_VisView.OverlayImage))
                {
                    graphics.DrawEllipse(pen, new RectangleF(ptCenter.X - radius, ptCenter.Y - radius, radius * 2, radius * 2));
                }

                //Refresh ovverade 
                _VisView.OverlayImage = _VisView.OverlayImage.Clone() as Bitmap;
            }));
        }
        //--------------------------------------------------------------------------
        public void DrawRectangle(Pen pen, Rectangle rect, bool overlay = false)
        {
            if (!_VisView.Visible) return;
            if (!SetOverlayImage(overlay)) return;

            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                using (var graphics = Graphics.FromImage(_VisView.OverlayImage))
                {
                    graphics.DrawRectangle(pen, rect);
                }

                //Refresh ovverade 
                _VisView.OverlayImage = _VisView.OverlayImage.Clone() as Bitmap;
            }));
        }
        //--------------------------------------------------------------------------

        public void DrawString(Font font, Brush brush, PointF pt, string text, bool overlay = false)
        {
            if (!_VisView.Visible) return;
            if (!SetOverlayImage(overlay)) return;

            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                using (var graphics = Graphics.FromImage(_VisView.OverlayImage))
                {
                    graphics.DrawString(text,font, brush, pt);
                }

                //Refresh ovverade 
                _VisView.OverlayImage = _VisView.OverlayImage.Clone() as Bitmap;
            }));
        }
        //--------------------------------------------------------------------------
        private TSET m_OverlayROIs = null;
        private bool m_pendingDrawROIs = false;

		//260521 //AOCV
        public void DrawAllROIs(TSET set = null)
        {
            m_OverlayROIs = set;
            if (set == null || set.sROIName == null) return;
            
            if (_VisView.BaseImage == null)
            {
                m_pendingDrawROIs = true;
                return;
            }

            DrawAllROIs_Internal();
        }

		//260521 //AOCV
        private void DrawAllROIs_Internal()
        {
            if (!SetOverlayImage(true)) return;
            if (!cDEF.FM.SysOptn.bViewROI) return;

            Color[] rawColors = new Color[] { Color.Blue, Color.Orange, Color.Green, Color.Cyan, Color.Magenta, Color.Yellow, Color.Red, Color.Pink };
            Color[] bgrColors = new Color[rawColors.Length];
            for (int c = 0; c < rawColors.Length; c++)
            {
                bgrColors[c] = Color.FromArgb(rawColors[c].A, rawColors[c].B, rawColors[c].G, rawColors[c].R);
            }
            
            _VisView.Invoke(new MethodInvoker(delegate ()
            {
                if (_VisView.OverlayImage == null) return;

                using (var graphics = Graphics.FromImage(_VisView.OverlayImage))
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(TVisnUnit.Path);
                        if (di.Exists)
                        {
                            FileInfo[] files = di.GetFiles("*.roi").OrderBy(f => f.Name).ToArray();
                            for (int i = 0; i < files.Length; i++)
                            {
                                string name = Path.GetFileNameWithoutExtension(files[i].Name);
                                if (string.IsNullOrEmpty(name)) continue;
                                
                                TROI roi = TROI.GeRegion(name);
                                if (roi != null && roi.dWidth > 0 && roi.dHeight > 0)
                                {
                                    Pen penToUse = new Pen(bgrColors[i % bgrColors.Length], 10);
                                    graphics.DrawRectangle(penToUse, new Rectangle((int)roi.dX, (int)roi.dY, (int)roi.dWidth, (int)roi.dHeight));
                                    
                                    string textToDraw = $"{(i + 1)}";
                                    using (Font font = new Font("Tahoma", 70, FontStyle.Bold))
                                    using (Brush brush = new SolidBrush(penToUse.Color))
                                    {
                                        graphics.DrawString(textToDraw, font, brush, new PointF((float)roi.dX, (float)roi.dY - 150));
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cDEF.LOG.ExceptionTrace($"DrawAllROIs Error: {ex.Message}");
                    }
                }
                
                //Refresh overlay once
                _VisView.OverlayImage = _VisView.OverlayImage.Clone() as Bitmap;
            }));
        }
        #endregion

        #region << Private Methodes >>

        private Bitmap ConvertTo32bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        private Bitmap LoadImageFromStream(string path)
        {
            Bitmap dest = null;

            using (System.IO.FileStream sw = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                dest = (Bitmap)Image.FromStream(sw);
            }

            return dest;
        }
        public Image ImageFromRawGrayArray(byte[] arr, int width, int height)
        {
            var output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            var rect = new Rectangle(0, 0, width, height);
            var bmpData = output.LockBits(rect,
                ImageLockMode.ReadWrite, output.PixelFormat);
            var ptr = bmpData.Scan0;
            Marshal.Copy(arr, 0, ptr, arr.Length);
            output.UnlockBits(bmpData);

            ColorPalette cp = output.Palette;

            // init palette
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);

            // set palette back
            output.Palette = cp;

            return output;
        }

        public unsafe Bitmap ImageFromRawGrayPtr(IntPtr pBuffer, int width, int height)
        {
            var output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            var rect = new Rectangle(0, 0, width, height);
            var bmpData = output.LockBits(rect,
                ImageLockMode.ReadWrite, output.PixelFormat);
            var ptr = bmpData.Scan0;
            long numbytes = width * height;
            CopyMemory(ptr, pBuffer, (uint)(numbytes));
            output.UnlockBits(bmpData);
            ColorPalette cp = output.Palette;

            // init palette
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);

            // set palette back
            output.Palette = cp;

            return output;
        }

        #endregion

        #region << Private Events >>
        private void KeepFrameAlive()
        {
            _FrameTimer = Stopwatch.StartNew();
        }

        public async void Camera_GrabEvent(ICamera pSender, GrabEventArg e)
        {
            if (!this.Visible || pSender.CameraName != btnName.Text) return;

            if (_FrameTimer.ElapsedMilliseconds > FrameInterval)
            {
                IntPtr buffer = Marshal.AllocHGlobal(e.Width * e.Height);
                CopyMemory(buffer, e.ImagePtr, (uint)(e.Width * e.Height));
                Bitmap temp = ImageFromRawGrayPtr(buffer, e.Width, e.Height);
                mFrameDataQueue.Enqueue(temp);
                Marshal.FreeHGlobal(buffer);

                KeepFrameAlive();
            }

           await Task.Delay(0);
        }

        #endregion

        private void btnJog_Click(object sender, EventArgs e)
        {
 
        }
    }
}
