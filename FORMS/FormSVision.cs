using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace eMachine
{
    public partial class FrmSVision : Form
    {
        EN_PART_SEL m_iRqPartSel;
        int m_iSelVisnPart;
        int m_iSelF1;
        int m_iSelF2;
        int m_iSelF3;
        bool actived;

 

        public string _sillSel { get { return lbCrntDvc1_1.Text; } }
        public string _sRoiSel { get { return lbCrntDvc2_1.Text; } }
        public string _sRefSel { get { return lbCrntDvc3_1.Text; } }


        public FrmSVision()
        {
            InitializeComponent();
            //
            TabPage tp;
            //
            this.BackColor = FRM.GetBaseColor();
            Control[] ctls = FNC.GetAllControlsUsingRecursive(this);
            foreach (Control ctl1 in ctls)
            {
                if (ctl1.GetType().Name.ToLower() == "tabpage")
                {
                    tp = ctl1 as TabPage;
                    tp.BackColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(210, 210, 208) : System.Drawing.Color.FromArgb(66, 72, 88);
                    tp.ForeColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(37, 51, 64) : System.Drawing.Color.FromArgb(230, 230, 200);
                }
            }
        }
        //------------------------------------------------------------------------
        private void FrmSManual_Load(object sender, EventArgs e)
        {
            tabMenu.Dock   = DockStyle.Fill;
            tabMenu.Width  = 1100;
            tabMenu.Height = this.Height + tabMenu.ItemSize.Height;
            
            Rectangle Rect = new Rectangle(tpgMenu1.Left, tpgMenu1.Top, tpgMenu1.Width - 2, tpgMenu1.Height - 1); //
            tabMenu.Region = new Region(Rect);
            m_iRqPartSel   = EN_PART_SEL.None;
            m_iSelVisnPart = 0;
            m_iSelF1       = 0;
            m_iSelF2       = 0;
    
            //
            SelPage_Click(button2, null);
        }
        //------------------------------------------------------------------------
        private void FrmSManual_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        //------------------------------------------------------------------------
        private void FrmSManual_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && actived)
            {//Show
                FormShow();
                tmProc.Enabled = true;
            }
            if (!this.Visible && actived)
            {//Hide
                tmProc.Enabled = false;
            }
            actived = this.Visible;
        }
        //------------------------------------------------------------------------
        public void FormShow()
        {
            SelPage(m_iSelVisnPart);
        }

        public void FormSave()
        {
            switch (tabMenu.SelectedIndex)
            {
                default: tpgPage1Save(); break;
                case 0 : tpgPage1Save(); break;
                case 1 : tpgPage2Save(); break;
                case 2 : tpgPage3Save(); break;
                case 3 : tpgPage4Save(); break;
                case 4 : tpgPage5Save(); break;
            }
        }

        public void FormUpdate()
        {
            if (m_iRqPartSel != EN_PART_SEL.None)
            {
                m_iSelVisnPart = (int)m_iRqPartSel;
                SelPage(m_iSelVisnPart);
                m_iRqPartSel = EN_PART_SEL.None;
            }

            switch (tabMenu.SelectedIndex)
            {
                default: tpgPage1Show(false); break;
                case  0: tpgPage1Show(false); break;
                case  1: tpgPage2Show(false); break;
                case  2: tpgPage3Show(false); break;
                case  3: tpgPage4Show(false); break;
                case  4: tpgPage5Show(false); break;
            }
        }
        public void FormUpdateUI()
        {
            //
            Control[] ctls = FNC.GetAllControlsUsingRecursive(pnSelButton);
            for (int i = 0; i < ctls.Length; i++)
            {
                int iTag = Convert.ToInt32(ctls[i].Tag);
                if (m_iSelVisnPart == iTag) ctls[i].BackColor = FRM.GetGridBackColor();
                else                        ctls[i].BackColor = FRM.GetBaseColor();

                ctls[i].Refresh();
            }
        }

        void SelPage(int iPage)
        {//화면의 메뉴 선택시 Tab PAGE 결정 
            tabMenu.SelectedIndex = iPage;
            switch (iPage)
            {
                default: tpgPage1Show(true); break;
                case 0 : tpgPage1Show(true); break;
                case 1 : tpgPage2Show(true); break;
                case 2 : tpgPage3Show(true); break;
                case 3 : tpgPage4Show(true); break;
                case 4 : tpgPage5Show(true); break;
            }
        }


        private void tmProc_Tick(object sender, EventArgs e)
        {
            if (!this.Visible) { this.tmProc.Enabled = false; return; }
            tmProc.Enabled = false;
            FormUpdate();
            FormUpdateUI();
            tmProc.Enabled = true;
        }

        private void SelPage_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
            int iTag = Convert.ToInt32(btn.Tag);
            m_iSelVisnPart = iTag;
            SelPage(m_iSelVisnPart);
        }
        private void Display(DataGridView dgv, int iSel)
        {
            if (dgv.CurrentCell == null) return;

            int iGridR = dgv.CurrentCell.RowIndex;
            int iGridC = dgv.CurrentCell.ColumnIndex;

            if (Convert.ToInt32(dgv.Tag) == 0)
            {
                lbCrntDvc1_1.Text = dgv[1, iGridR].Value.ToString();
                btProjInfo1_1.BackColor = (iSel == 0) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;
                btProjInfo1_2.BackColor = (iSel == 1) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;

                Tillumination ill = new Tillumination();
                ill.Load(true, lbCrntDvc1_1.Text);
                tbProjInfo1_1.Text    = ill.dGain.ToString();
                tbProjInfo1_2.Text    = ill.dExposureTime.ToString();
                tbProjInfo1_3.Text    = ill.dLightValue[0].ToString();
                tbProjInfo1_4.Text    = ill.dLightDelay.ToString();
                cbProjInfo1_1.Checked = ill.dUseLight[0] ? true : false;
            }
            else if (Convert.ToInt32(dgv.Tag) == 1)
            {
                lbCrntDvc2_1.Text = dgv[1, iGridR].Value.ToString();
                btProjInfo2_1.BackColor = (iSel == 0) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;
                btProjInfo2_2.BackColor = (iSel == 1) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;

                TROI roi = new TROI();
                roi.Load(true, lbCrntDvc2_1.Text);
                Drv.Control.IShape Tracker = FRM.MProj._FrmCamCtl.Tracker;

                tbProjInfo2_5.Text = roi.dX.ToString();
                tbProjInfo2_6.Text = roi.dY.ToString();
                tbProjInfo2_7.Text = roi.dWidth.ToString();
                tbProjInfo2_8.Text = roi.dHeight.ToString();
            }
            else
            {
                lbCrntDvc3_1.Text = dgv[1, iGridR].Value.ToString();
                btProjInfo3_1.BackColor = (iSel == 0) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;
                btProjInfo3_2.BackColor = (iSel == 1) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;

                TReference Ref = new TReference();
                Ref.Load(true, lbCrntDvc3_1.Text);

                tbProjInfo3_1.Text = Ref.dX.ToString();
                tbProjInfo3_2.Text = Ref.dY.ToString();
                tbProjInfo3_3.Text = Ref.dWidth.ToString();
                tbProjInfo3_4.Text = Ref.dHeight.ToString();

                pbProjInfo3_1.BackgroundImage?.Dispose();
                pbProjInfo3_1.BackgroundImageLayout = ImageLayout.Stretch;

                if (string.IsNullOrWhiteSpace(Ref.sPath)) return;
                if (!File.Exists(Ref.sPath)) return;

                using (System.IO.FileStream sw = new System.IO.FileStream(Ref.sPath, System.IO.FileMode.Open))
                {
                    pbProjInfo3_1.BackgroundImage = (Bitmap)Image.FromStream(sw);
                }
            }
        
        }

        //------------------------------------------------------------------------
        public void tpgPage1Show(bool toCtrl)
        {
            //Tab Page #1 Show (화면 업데이트)            
            if (toCtrl)
            {
                String sJobPath = Application.StartupPath + "\\Vision";
                FNC.UpdateFileByGrid(sJobPath, ref sgJobList1_1, FRM.GetGridBackColor(), false, true, Tillumination.Extension, false);
                Display(sgJobList1_1, m_iSelF1);
            }
            else
            {
             

            }
        }
        public void tpgPage1Save()
        {
            if (lbCrntDvc1_1.Text == "" && m_iSelF1 > 0)
            {
                FRM.ShowMsg(true, "Confirm", "Please select Job File");
                return;
            }

            Tillumination till = new Tillumination()
            {
                dGain           = Convert.ToDouble(tbProjInfo1_1.Text == string.Empty ? "0" : tbProjInfo1_1.Text),
                dExposureTime   = Convert.ToDouble(tbProjInfo1_2.Text == string.Empty ? "0" : tbProjInfo1_2.Text),
                dLightDelay     = Convert.ToDouble(tbProjInfo1_4.Text == string.Empty ? "0" : tbProjInfo1_4.Text),
            };
            till.dLightValue[0] = Convert.ToDouble(tbProjInfo1_3.Text == string.Empty ? "0" : tbProjInfo1_3.Text);
            till.dUseLight[0]   = cbProjInfo1_1.Checked;

            till.Load(false, lbCrntDvc1_1.Text);
            cDEF.LOG.Trace( "Save (" + lbCrntDvc1_1.Text + ")");
            tpgPage1Show(true);
        }
        //------------------------------------------------------------------------
        public void tpgPage2Show(bool toCtrl) 
        {
            //Tab Page #2 Show (화면 업데이트)
            if (toCtrl)
            {
                String sJobPath = Application.StartupPath + "\\Vision";
                FNC.UpdateFileByGrid(sJobPath, ref sgJobList2_1, FRM.GetGridBackColor(), false, true, TROI.Extension, false);
                Display(sgJobList2_1, m_iSelF2);
            }
            else
            {
                Drv.Control.IShape Tracker = FRM.MProj._FrmCamCtl.Tracker;
                RectangleF ImageBound = Tracker.GetImageBound();

                tbProjInfo2_1.Text = ImageBound.X.ToString();
                tbProjInfo2_2.Text = ImageBound.Y.ToString();
                tbProjInfo2_3.Text = ImageBound.Width.ToString();
                tbProjInfo2_4.Text = ImageBound.Height.ToString();
            }
        }
        public void tpgPage2Save()
        {
            if (lbCrntDvc2_1.Text == "" && m_iSelF2 > 0)
            {
                FRM.ShowMsg(true, "Confirm", "Please select Job File");
                return;
            }

            TROI tRoi = new TROI()
            {
                dX      = Convert.ToDouble(tbProjInfo2_1.Text == string.Empty ? "0" : tbProjInfo2_1.Text),
                dY      = Convert.ToDouble(tbProjInfo2_2.Text == string.Empty ? "0" : tbProjInfo2_2.Text),
                dWidth  = Convert.ToDouble(tbProjInfo2_3.Text == string.Empty ? "0" : tbProjInfo2_3.Text),
                dHeight = Convert.ToDouble(tbProjInfo1_4.Text == string.Empty ? "0" : tbProjInfo2_4.Text),
            };
            tRoi.Load(false, lbCrntDvc2_1.Text);
            cDEF.LOG.Trace("Save (" + lbNewDvc2_1.Text + ")");

            tpgPage2Show(true);
        }
        //------------------------------------------------------------------------
        public void tpgPage3Show(bool toCtrl) 
        {//Tab Page #3 Show (화면 업데이트)
         //bool isOn = false;

            if (toCtrl)
            {
                String sJobPath = Application.StartupPath + "\\Vision";
                FNC.UpdateFileByGrid(sJobPath, ref sgJobList3_1, FRM.GetGridBackColor(), false, true, TReference.Extension, false);
                Display(sgJobList3_1, m_iSelF3);
            }
            else
            {
                Drv.Control.IShape Tracker = FRM.MProj._FrmCamCtl.Tracker;
                RectangleF ImageBound = Tracker.GetImageBound();

                tbProjInfo3_1.Text = ImageBound.X.ToString();
                tbProjInfo3_2.Text = ImageBound.Y.ToString();
                tbProjInfo3_3.Text = ImageBound.Width.ToString();
                tbProjInfo3_4.Text = ImageBound.Height.ToString();
            }
        }
        public void tpgPage3Save()
        {
            if (lbCrntDvc3_1.Text == "" && m_iSelF3 > 0)
            {
                FRM.ShowMsg(true, "Confirm", "Please select Job File");
                return;
            }

            Drv.Control.IShape Tracker = FRM.MProj._FrmCamCtl.Tracker;
            RectangleF ImageBound = Tracker.GetImageBound();
            Bitmap BaseImage = FRM.MProj._FrmCamCtl.GetCurrentImage();

            if (BaseImage == null) { MsgBox.Warning("You can not Crete the use Bmp File."); return; }
            if (ImageBound.Width <= 0 || ImageBound.Height <= 0) { MsgBox.Warning("You can not Crete the use Bmp File."); return; }
            if (ImageBound.X <= 0 || ImageBound.Y <= 0) { MsgBox.Warning("You can not Crete the use Bmp File."); return; }

            TReference tRef = new TReference()
            {
                dX = Convert.ToDouble(tbProjInfo2_1.Text == string.Empty ? "0" : tbProjInfo2_1.Text),
                dY = Convert.ToDouble(tbProjInfo2_2.Text == string.Empty ? "0" : tbProjInfo2_2.Text),
                dWidth = Convert.ToDouble(tbProjInfo2_3.Text == string.Empty ? "0" : tbProjInfo2_3.Text),
                dHeight = Convert.ToDouble(tbProjInfo1_4.Text == string.Empty ? "0" : tbProjInfo2_4.Text),
                sPath = TVisnUnit.Path + lbCrntDvc3_1.Text + ".bmp"
            };

            using (var Bmp = new Bitmap((int)ImageBound.Width, (int)ImageBound.Height))
            {
                using (Graphics g = Graphics.FromImage(Bmp))
                {
                    g.DrawImage(BaseImage, new Rectangle(0, 0, Bmp.Width, Bmp.Height), ImageBound, GraphicsUnit.Pixel);
                }

                //convert 8pp
                //using (var RefBmp = Bmp.MakeGrayscale())
                //{
                //    RefBmp.Save(tRef.sPath);
                //}
            }

            tRef.Load(false, lbCrntDvc3_1.Text);
            cDEF.LOG.Trace("Save (" + lbNewDvc3_1.Text + ")");
            tpgPage3Show(true);
        }
        #region Page #1

        private void sgJobList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Display(sgJobList1_1, m_iSelF1);
        }

        private void btProjInfo1_1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            m_iSelF1 = iTag;
            Display(sgJobList1_1, m_iSelF1);
        }

        private void btProjInfo1_3_Click(object sender, EventArgs e)
        {
            //Local Var.
            String sPath1 = Application.StartupPath + "\\Vision\\" + lbCrntDvc1_1.Text;
            String sPath2 = Application.StartupPath + "\\Vision\\" + lbNewDvc1_1.Text;

            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            if (cDEF.SEQ._bRun)
            {
                MsgBox.Warning("The JobFile can not be download while the Machine is running.");
                return;
            }

            //Check None Name.
            if (lbCrntDvc1_1.Text == "" && m_iSelF1 > 0)
            {
                FRM.ShowMsg(true, "Confirm", "Please select Job File");
                return;
            }

            switch (m_iSelF1)
            {
                default: break;
                case 0://New를 선택 했을 경우        
                    if (lbNewDvc1_1.Text == lbCrntDvc1_1.Text) { MsgBox.Warning("You can not Crete the use Job File."); return; }
                    if (!FRM.ShowMsg(true, "Confirm", "Do you want to " + btProjInfo1_1.Text + "(" + lbNewDvc2_1.Text + ")" + " ?")) return;
                    //FNC.CreateFile(sPath2 + ".ill");
                    Tillumination illumination = new Tillumination() 
                    { 
                        dGain         = Convert.ToDouble(tbProjInfo1_1.Text == string.Empty ? "0" : tbProjInfo1_1.Text),
                        dExposureTime = Convert.ToDouble(tbProjInfo1_2.Text == string.Empty ? "0" : tbProjInfo1_2.Text),
                        dLightDelay   = Convert.ToDouble(tbProjInfo1_4.Text == string.Empty ? "0" : tbProjInfo1_4.Text),
                    };

                    illumination.dLightValue[0] = Convert.ToDouble(tbProjInfo1_3.Text == string.Empty ? "0" : tbProjInfo1_3.Text);
                    illumination.dUseLight[0]   = cbProjInfo1_1.Checked;

                    illumination.Load(false, lbNewDvc1_1.Text);
                    cDEF.LOG.Trace(btProjInfo1_1.Text + "(" + lbNewDvc1_1.Text + ")");
                    break;
                case 1://Delete. 선택했을 경우
                    if (lbCrntDvc1_1.Text == "" || lbCrntDvc1_1.Text == "Default") { MsgBox.Warning("You can not Delete the use Job File."); return; }
                    if (!FRM.ShowMsg(true, "Confirm", "Do you want to " + btProjInfo1_2.Text + "(" + lbNewDvc2_1.Text + ")" + " ?")) return;
                    FNC.FileDelete(sPath1 +".ill");
                    cDEF.LOG.Trace(btProjInfo1_2.Text + "(" + lbCrntDvc1_1.Text + ")");
                    break;
            }
            tpgPage1Show(true);
        }

        #endregion

        #region Page #2
        private void sgJobList2_1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Display(sgJobList2_1, m_iSelF2);
        }
        private void btProjInfo2_1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            m_iSelF2 = iTag;
            Display(sgJobList2_1, m_iSelF2);
        }
        private void btProjInfo2_3_Click(object sender, EventArgs e)
        {
            //Local Var.
            String sPath1 = Application.StartupPath + "\\Vision\\" + lbCrntDvc2_1.Text;
            String sPath2 = Application.StartupPath + "\\Vision\\" + lbNewDvc2_1.Text;

            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            if (cDEF.SEQ._bRun)
            {
                MsgBox.Warning("The JobFile can not be download while the Machine is running.");
                return;
            }

            //Check None Name.
            if (lbCrntDvc2_1.Text == "" && m_iSelF2 > 0)
            {
                FRM.ShowMsg(true, "Confirm", "Please select Job File");
                return;
            }

            switch (m_iSelF2)
            {
                default: break;
                case 0://New를 선택 했을 경우        
                    if (lbNewDvc2_1.Text == lbCrntDvc2_1.Text) { MsgBox.Warning("You can not Crete the use Job File."); return; }
                    if (!FRM.ShowMsg(true, "Confirm", "Do you want to " + btProjInfo2_1.Text + "(" + lbNewDvc2_1.Text + ")" + " ?")) return;
                    //FNC.CreateFile(sPath2 + ".ill");
                    TROI tRoi = new TROI()
                    {
                        dX      = Convert.ToDouble(tbProjInfo2_1.Text == string.Empty ? "0" : tbProjInfo2_1.Text),
                        dY      = Convert.ToDouble(tbProjInfo2_2.Text == string.Empty ? "0" : tbProjInfo2_2.Text),
                        dWidth  = Convert.ToDouble(tbProjInfo2_3.Text == string.Empty ? "0" : tbProjInfo2_3.Text),
                        dHeight = Convert.ToDouble(tbProjInfo1_4.Text == string.Empty ? "0" : tbProjInfo2_4.Text),
                    };
                    tRoi.Load(false, lbNewDvc2_1.Text);
                    cDEF.LOG.Trace(btProjInfo2_1.Text + "(" + lbNewDvc2_1.Text + ")");
                    break;
                case 1://Delete. 선택했을 경우
                    if (lbCrntDvc2_1.Text == "" || lbCrntDvc2_1.Text == "Default") { MsgBox.Warning("You can not Delete the use Job File."); return; }
                    if (!FRM.ShowMsg(true, "Confirm", "Do you want to " + btProjInfo2_2.Text + "(" + lbNewDvc2_1.Text + ")" + " ?")) return;
                    FNC.FileDelete(sPath1 + ".roi");
                    cDEF.LOG.Trace(btProjInfo2_2.Text + "(" + lbCrntDvc2_1.Text + ")");
                    break;
            }
            tpgPage2Show(true);
        }

        #endregion

        #region Page #3
        private void btProjInfo3_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            m_iSelF3 = iTag;
            Display(sgJobList3_1, m_iSelF3);
        }
        private void btProjInfo3_3_Click(object sender, EventArgs e)
        {
            //Local Var.
            String sPath1 = Application.StartupPath + "\\Vision\\" + lbCrntDvc3_1.Text;
            String sPath2 = Application.StartupPath + "\\Vision\\" + lbNewDvc3_1.Text;

            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            if (cDEF.SEQ._bRun)
            {
                MsgBox.Warning("The JobFile can not be download while the Machine is running.");
                return;
            }

            //Check None Name.
            if (lbCrntDvc3_1.Text == "" && m_iSelF3 > 0)
            {
                FRM.ShowMsg(true, "Confirm", "Please select Job File");
                return;
            }

            switch (m_iSelF3)
            {
                default: break;
                case 0://New를 선택 했을 경우        
                    if (lbNewDvc3_1.Text == lbCrntDvc3_1.Text) { MsgBox.Warning("You can not Crete the use Job File."); return; }
                    if (!FRM.ShowMsg(true, "Confirm", "Do you want to " + btProjInfo3_1.Text + " ?")) return;
                    //FNC.CreateFile(sPath2 + ".ill");
                    TReference tRef = new TReference()
                    {
                        dX = Convert.ToDouble(tbProjInfo3_1.Text == string.Empty ? "0" : tbProjInfo3_1.Text),
                        dY = Convert.ToDouble(tbProjInfo3_2.Text == string.Empty ? "0" : tbProjInfo3_2.Text),
                        dWidth = Convert.ToDouble(tbProjInfo3_3.Text == string.Empty ? "0" : tbProjInfo3_3.Text),
                        dHeight = Convert.ToDouble(tbProjInfo3_4.Text == string.Empty ? "0" : tbProjInfo3_4.Text),
                        sPath = TVisnUnit.Path + lbNewDvc3_1.Text + ".bmp",
                     };

                    Drv.Control.IShape Tracker = FRM.MProj._FrmCamCtl.Tracker;
                    RectangleF ImageBound = Tracker.GetImageBound();
                    Bitmap BaseImage = FRM.MProj._FrmCamCtl.GetCurrentImage();

                    if (BaseImage == null) { MsgBox.Warning("You can not Crete the use Bmp File."); return; }
                    if (ImageBound.Width <= 0 || ImageBound.Height <= 0) { MsgBox.Warning("You can not Crete the use Bmp File."); return; }
                    if (ImageBound.X     <= 0 || ImageBound.Y      <= 0) { MsgBox.Warning("You can not Crete the use Bmp File."); return; }

                    using (var Bmp = new Bitmap((int)ImageBound.Width, (int)ImageBound.Height))
                    {
                        using (Graphics g = Graphics.FromImage(Bmp))
                        {
                            g.DrawImage(BaseImage, new Rectangle(0, 0, Bmp.Width, Bmp.Height), ImageBound, GraphicsUnit.Pixel);
                        }

                        //convert 8pp
                        //using (var RefBmp = Bmp.MakeGrayscale())
                        //{
                        //    RefBmp.Save(tRef.sPath);
                        //}
                    }
           
                    tRef.Load(false, lbNewDvc3_1.Text);
                    cDEF.LOG.Trace(btProjInfo3_1.Text + "(" + lbNewDvc3_1.Text + ")");
                    break;
                case 1://Delete. 선택했을 경우
                    if (lbCrntDvc3_1.Text == "" || lbCrntDvc3_1.Text == "Default") { MsgBox.Warning("You can not Delete the use Job File."); return; }
                    if (!FRM.ShowMsg(true, "Confirm", "Do you want to " + btProjInfo3_2.Text + " ?")) return;
                    FNC.FileDelete(sPath1 + ".ref");
                    cDEF.LOG.Trace(btProjInfo3_2.Text + "(" + lbCrntDvc3_1.Text + ")");
                    break;
            }
            tpgPage3Show(true);
        }
        public unsafe Bitmap MakeGrayscale(Bitmap original)
        {
            //make an empty bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format8bppIndexed);

            //lock the new bitmap in memory
            BitmapData newData = newBitmap.LockBits(
               new Rectangle(0, 0, original.Width, original.Height),
              ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            for (int y = 0; y < original.Height; y++)
            {
                //get the data from the new image
                byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);

                for (int x = 0; x < original.Width; x++)
                {
                    Color originalColor = original.GetPixel(x, y);

                    //create the grayscale version of the pixel
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                        + (originalColor.B * .11));

                    //set the new image's pixel to the grayscale version
                    nRow[x] = (byte)grayScale;
                }
            }

            ColorPalette cp = newBitmap.Palette;

            // init palette
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);

            // set palette back
            newBitmap.Palette = cp;
            
            newBitmap.UnlockBits(newData);

            return newBitmap;
        }

        private void sgJobList3_1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Display(sgJobList3_1, m_iSelF3);
        }
        #endregion

        #region Page #4
        public void tpgPage4Show(bool toCtrl)
        {//Tab Page #4 Show (화면 업데이트)
         //bool isOn = false;
            if (toCtrl)
            {
              

            }
            else
            {

            }
        }

        public void tpgPage4Save()
        {

        }
    

        #endregion

        #region Page #5
        public void tpgPage5Show(bool toCtrl)
        {//Tab Page #4 Show (화면 업데이트)
         //bool isOn = false;

            if (toCtrl)
            {

            }
            else
            {

            }
        }

        public void tpgPage5Save()
        {

        }

        #endregion



    }
}
