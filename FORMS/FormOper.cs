using eMachine.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eMachine
{
    public partial class FormOper : Form
    {
        public FrmCamDx FrmCamCtrl = new FrmCamDx(EN_DISPLAY.View);


        bool actived;
        public FormOper()
        {
            InitializeComponent();

            FRM.SetUCParent(FrmCamCtrl, this.pnVision);

            cDEF.LOG.DisplayResult += DisplayVisnResult;
        }

        private void FormOper_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && !actived)
            {//Show
                FRM.ShowUCParent(FrmCamCtrl, this.pnVision);
                //m_iSelMan   =  0;
                //
                timer1.Enabled = true;
            }
            if (!this.Visible && actived)
            {//Hide
                FRM.HideUCParent(FrmCamCtrl);
                timer1.Enabled = false;
            }
            actived = this.Visible;
        }
        //--------------------------------------------------------------------------
        public void FormShow()
        {
            timer1.Enabled = true;

            //

        }
        //--------------------------------------------------------------------------
        public void FormHide()
        {
            timer1.Enabled = false;
        }
        //--------------------------------------------------------------------------
        private void btOper03_Click(object sender, EventArgs e)
        {

            //
            KToggleButton sbt = sender as KToggleButton;
            string sMsg       = sbt.Text.Trim();
            string sVac       = cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON) ? "OFF" : "On";

            if (cDEF.SEQ._bAutoMode && sMsg != "Reset")
            {
                MsgBox.Warning("Can not be use while Auto Mode.");
                return;
            }

            //
            switch (sMsg)
            {
                case "Vacuum":
                    if (!FRM.ShowMsg(true, " Warning ", $"Do you want to {sMsg} {sVac}?", EN_MSG_KIND.UserModal)) return;
                    if (cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON))
                    {
                        //Vacuum Off
                        cDEF.SEQ.WAT.SetVacOff();
                    }
                    else
                    {
                        //Vacuum On
                        cDEF.SEQ.WAT.SetVacOn();
                    }
                    //cDEF.IO.sY(EN_OUT_ID.yVACUUM_ON, !cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON));
                    cDEF.LOG.Trace($"[Man] Vacuum Click - SET : {cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON)}");
                    break;
                
                case "Home":
                    if (!FRM.ShowMsg(true, " Warning ", $"Do you want to {sMsg}?", EN_MSG_KIND.UserModal)) return;
                    cDEF.LOG.Trace("[Man] Home Click");
                    cDEF.MAN.ManProcOn(1, true, false);
                    break;
                
                case "Align":
                    if(!cDEF.VISN.Cam[(int)EN_CAM.WTB].Cameara.CameraStatus.Connection)
                    {
                        FRM.ShowWarn(true, "Camera 연결이 안되었습니다.");
                        return;
                    }

                    if(!cDEF.SEQ.IsAllHomeEnd())
                    {
                        FRM.ShowWarn(true, "Home 동작이 필요합니다.");
                        return; 
                    }
                    if (!FRM.ShowMsg(true, " Warning ", $"Do you want to {sMsg}?", EN_MSG_KIND.UserModal)) return;

                    cDEF.MAN.ManProcOn(2002, true, false);

                    break;
                
                case "Reset":
                    cDEF.SEQ.Reset();
                    cDEF.LOG.Trace("[Man] Reset Click");
                    break;
                
                case "Full":
                    break;
                case "Hide":
                    break;
                case "Save CSV":
                    break;

                default:
                    break;
            }
        }
        //--------------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            //
            timer1.Enabled = false;

            //btOper01.Image          = cDEF.IO.gX(EN_IN_ID.xWAFER_EXIST) ? Resources._Green_20 : Resources._Gray_20;
            //btOper02.Image          = cDEF.IO.gX(EN_IN_ID.xVACUUM_ON  ) ? Resources._Green_20 : Resources._Gray_20;
            btOper01.Image          = cDEF.SEQ.WAT.IsWaferExist() ? Resources._Green_20 : Resources._Gray_20;
            btOper02.Image          = cDEF.SEQ.WAT.IsVacOn     () ? Resources._Green_20 : Resources._Gray_20;


            lbAxisPositionX.Text    = Math.Round(cDEF.MOTR.GetEncPos(EN_MOTR_ID.WAT_X),4).ToString();
            lbAxisPositionY.Text    = Math.Round(cDEF.MOTR.GetEncPos(EN_MOTR_ID.WAT_Y),4).ToString();
            lbAxisPositionT.Text    = Math.Round(cDEF.MOTR.GetEncPos(EN_MOTR_ID.WAT_T),4).ToString();


            pnMsg.Visible = cDEF.SEQ.WAT._iStepAlgn >= 10 && cDEF.SEQ._bFlick1;
            if (pnMsg.Visible) lbMsg.Text = cDEF.SEQ.WAT._sWorkMsg;


            pnSkipVac  .Visible = cDEF.FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WAT] && cDEF.SEQ._bFlick2;
            pnSkipWafer.Visible = cDEF.FM.SysOptn.iRunSkipMat == 1             && cDEF.SEQ._bFlick2;
            pnImageSave.Visible = cDEF.FM.EngrOptn.bUseImageSave               && cDEF.SEQ._bFlick2;

            pnLightCon .Visible = cDEF.FM.IsMasterLv();
            pnImageTest.Visible = cDEF.FM.IsMasterLv();

            UpdatewarnDisp();

            //
            timer1.Enabled = true;
        }
        //--------------------------------------------------------------------------
        private void FormOper_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true; 
        }
        //--------------------------------------------------------------------------
        private void DisplayVisnResult(ListViewItem result)
        {
            listViewData?.Invoke((Action)delegate ()
            {
                if (listViewData.Items.Count > 200) listViewData.Items.RemoveAt(listViewData.Items.Count - 1);

                listViewData.Items.Insert(0, result);
                listViewData.Items[0].Selected = true;

            });
        }
        //--------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            //cDEF.VISN.SetLightOn(true, (int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn);
            cDEF.SEQ.WAT.SetLight(true);
        }
        //--------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            //cDEF.VISN.SetLightOn(false, (int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn);
            cDEF.SEQ.WAT.SetLight(false);
        }

        private void btOper03_MouseHover(object sender, EventArgs e)
        {
            //Transparent
            KToggleButton sBtn = sender as KToggleButton;

            sBtn.BackColor = sBtn.OffColor;//Color.Aqua;
        }
        //------------------------------------------------------------------------
        private void btOper03_MouseLeave(object sender, EventArgs e)
        {
            //Transparent
            KToggleButton sBtn = sender as KToggleButton;

            sBtn.BackColor = Color.Transparent;

        }
        //------------------------------------------------------------------------
        private void UpdatewarnDisp()
        {
            lbDispWarn.Items.Clear();
            if (cDEF.EPU._bHasWrn || cDEF.EPU._bHasDsp)
            {
                string stemp;
                for (int i = 1; i < vDEF.MAX_ERR; i++)
                {
                    if (cDEF.EPU[i].m_bOn && (cDEF.EPU[i].IsGradeDisp() || cDEF.EPU[i].IsGradeWarn()))
                    {
                        stemp = string.Format("[ERR{0,4:0000}] {1}", i, cDEF.EPU.GetName(i));
                        lbDispWarn.Items.Add(stemp);
                    }
                }
            }
            //
            pnDisplayWarn.Visible = (cDEF.EPU._bHasWrn || cDEF.EPU._bHasDsp) && cDEF.SEQ._bFlick2;
        }
        //------------------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            //
            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Filter     = "[BMP FILE]|*.bmp|[JPEG FILE]|*.jpg|[ALL FILE]|*.*";
                FileDialog.DefaultExt = "*.bmp";
                FileDialog.Title      = "이미지 불러오기";

                if (FileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap Temp = new Bitmap(FileDialog.FileName))
                    {
                        _TFuncArg arg = new _TFuncArg();

                        if (Temp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed ||
                            Temp.Width       != cDEF.VISN.Cam[(int)EN_CAM.WTB].Buffer.wid            ||
                            Temp.Height      != cDEF.VISN.Cam[(int)EN_CAM.WTB].Buffer.len             )
                        {
                              
                            MsgBox.Warning("Can not be Tested this image format");
                        }
                        else
                        {
                            arg.sArg1 = FileDialog.FileName;
                            //
                            cDEF.MAN.ManProcOn(2014, true, false, arg);
                        }
                    }
                }
            }
        }
    }
}
