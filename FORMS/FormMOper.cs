using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;
using System.Diagnostics;
using static eMachine.cDEF;
using System.Drawing.Drawing2D;

namespace eMachine
{
    public partial class FrmMOper : Form
    {
        //
        FrmCtrlMC FrmCtlBtn = new FrmCtrlMC(1);
        FrmCam    FrmCamCtl = new FrmCam();

        int m_iSelWhre;
        int m_iSelPart;
        int m_iSelRow ;

        bool m_bFlick;
        bool actived;
        string[] sWafStat = new string[(int)EN_WAFER_STAT.EndOfId];
        Label [] lbIon    = new Label[(int)EN_WTR_WORK_AREA.EndOfId];


        DateTime startTime = new DateTime();
        ContextMenuStrip MenuStrip = new ContextMenuStrip();
        ToolTip tooltip = new ToolTip();

        Random[] TempRan = new Random[3];
        TSORT_INFO tSort = new TSORT_INFO();

        TimerUnit tu = new TimerUnit();

        Label[] m_lbMapColor = new Label[(int)EN_WAFER_STAT.EndOfId];
        Panel[] m_pnMapColor = new Panel[(int)EN_WAFER_STAT.EndOfId];

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public FrmMOper()
        {
            InitializeComponent();

            //
            Panel pn;
            Label lb;
            RoundPanel rp;
            FlowLayoutPanel fp;
            //
            startTime = DateTime.Now;
            //
            this.BackColor = FRM.GetBaseColor();
            //
            Control[] ctls = FNC.GetAllControlsUsingRecursive(this);
            foreach (Control ctl in ctls)
            {
                if (ctl.GetType().Name.ToLower() == "panel")
                {
                    pn = ctl as Panel;
                    pn.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "flowlayoutpanel")
                {
                    fp = ctl as FlowLayoutPanel;
                    fp.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "label")
                {
                    lb = ctl as Label;
                    lb.ForeColor = FRM.GetForeColor();
                }
                else if (ctl.GetType().Name.ToLower() == "RoundPanel")
                {
                    rp = ctl as RoundPanel;
                    rp.ForeColor = FRM.GetForeColor();
                }
            }
            //
            //pnMain .BackColor = Color.Black;
            //lpPanel.BackColor = Color.Black; 

            //
            tooltip.AutoPopDelay = 1000;
            tooltip.InitialDelay = 100;
            tooltip.ReshowDelay = 100;
            tooltip.IsBalloon = true;
            tooltip.SetToolTip(this.btogMFunc1, btogMFunc1.Text);
            tooltip.SetToolTip(this.btogMFunc2, btogMFunc2.Text);
            tooltip.SetToolTip(this.btogMFunc3, btogMFunc3.Text);
            tooltip.SetToolTip(this.btogMFunc4, btogMFunc4.Text);
            tooltip.SetToolTip(this.btogMFunc5, btogMFunc5.Text);
            tooltip.SetToolTip(this.btogMFunc6, btogMFunc6.Text);
            tooltip.SetToolTip(this.btogMFunc7, btogMFunc7.Text);

            //
            cDEF.LOG.DisplayLogEvent += DisplayLogEvent;
            cDEF.LOG.DisplayComEvent += DisplayComEvent;
            cDEF.LOG.DisplayTraceEvent += DisplayLogTrace;
            cDEF.LOG.DisplayRFIDEvent += DisplayRFIDTrace;


            //Wafer display
            sWafStat[(int)EN_WAFER_STAT.Empty] = "없음";
            sWafStat[(int)EN_WAFER_STAT.Mask] = "스캔";
            sWafStat[(int)EN_WAFER_STAT.Mount] = "작업대기";
            sWafStat[(int)EN_WAFER_STAT.Aligned] = "보정완료";
            sWafStat[(int)EN_WAFER_STAT.Skip] = "스킵";
            sWafStat[(int)EN_WAFER_STAT.Fnsh] = "작업종료";
            sWafStat[(int)EN_WAFER_STAT.Work] = "작업중";
            sWafStat[(int)EN_WAFER_STAT.Wait] = "비젼검사중";
            sWafStat[(int)EN_WAFER_STAT.Fail] = "작업실패";
            sWafStat[(int)EN_WAFER_STAT.FnshAlign] = "배출";

            cbSelWhre.Items.Clear();
            cbSelWhreFrom.Items.Clear();
            cbSelWhreTo.Items.Clear();

            cbSelSlot.Items.Clear();
            cbSelSlotFrom.Items.Clear();
            cbSelSlotTo.Items.Clear();


            for (int n = 0; n < (int)EN_WTR_WORK_AREA.EndOfId; n++)
            {
                cbSelWhre.Items.Add(vDEF.STR_WTR_WORK_AREA[n]);
                cbSelWhreFrom.Items.Add(vDEF.STR_WTR_WORK_AREA[n]);
                cbSelWhreTo.Items.Add(vDEF.STR_WTR_WORK_AREA[n]);
            }

            for (int n = 0; n < FM.ProjBase.iMaxMgzSlot[0]; n++)
            {
                cbSelSlot.Items.Add(n + 1);
                cbSelSlotFrom.Items.Add(n + 1);
                cbSelSlotTo.Items.Add(n + 1);
            }

            //
            lbWarning.BackColor = Color.Yellow;
            lbWarning.ForeColor = Color.Black;

            //
            lbIon[0] = lbIon1; //MGZ1
            lbIon[1] = lbIon2; //MGZ2
            lbIon[2] = lbIon3; //WAT 
            lbIon[3] = lbIon4; //ASM1
            lbIon[4] = lbIon5; //ASM2

            lbIon[0].Tag = (int)EN_OUT_ID.yLPM_PORT1_ION_RUN;//EN_IN_ID.xLPM_PORT1_Ionizer_Run;
            lbIon[1].Tag = (int)EN_OUT_ID.yLPM_PORT2_ION_RUN;//EN_IN_ID.xLPM_PORT2_Ionizer_Run;
            lbIon[2].Tag = (int)EN_OUT_ID.yWAT_ION_RUN;//EN_IN_ID.xWAT_Ionizer_Run      ;
            lbIon[3].Tag = (int)EN_OUT_ID.yMMC_MC1_ION_RUN;//EN_IN_ID.xMMC_MC1_Ionizer_Run  ;
            lbIon[4].Tag = (int)EN_OUT_ID.yMMC_MC2_ION_RUN;//EN_IN_ID.xMMC_MC2_Ionizer_Run  ;

        }

        //------------------------------------------------------------------------
        private void FrmOper_Load(object sender, EventArgs e)
        {
            SetBounds(0, 0, 1280, 895);
            //
            FRM.SetFormParent(FrmCtlBtn, this.pn8);

        }
        //------------------------------------------------------------------------
        private void FrmOper_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerProc.Enabled = false;
        }
        //------------------------------------------------------------------------
        private void FrmOper_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && !actived)
            {//Show
                FRM.ShowFormParent(FrmCtlBtn, this.pn8);

                //
                timerProc.Enabled = true;

            }

            if (!this.Visible && actived)
            {//Hide
                FRM.HideFormParent(FrmCtlBtn);
                //FRM.HideFormParent(FRM.WafMapViewer);

                timerProc.Enabled = false;
            }
            actived = this.Visible;

            //button10.Visible = true;
            //panel15.Visible = true; 

        }
        //------------------------------------------------------------------------
        public void FormHide()
        {
            FRM.HideFormParent(FrmCtlBtn);
        }
        //------------------------------------------------------------------------
        private void DisplayLogEvent(string sLog)
        {
            lstMain?.Invoke((Action)delegate ()
            {
                if (lstMain.Items.Count >= 500)
                    lstMain.Items.RemoveAt(lstMain.Items.Count - 1);

                lstMain.Items.Insert(0, sLog);
            });
        }
        //------------------------------------------------------------------------
        private void DisplayComEvent(string sLog)
        {
            lstCom?.Invoke((Action)delegate ()
            {
                if (lstCom.Items.Count >= 500)
                    lstCom.Items.RemoveAt(lstCom.Items.Count - 1);

                lstCom.Items.Insert(0, sLog);
            });
        }
        //------------------------------------------------------------------------
        private void DisplayLogTrace(string sLog)
        {
            lstTrace?.Invoke((Action)delegate ()
            {
                if (lstTrace.Items.Count >= 500)
                    lstTrace.Items.RemoveAt(lstTrace.Items.Count - 1);

                lstTrace.Items.Insert(0, sLog);
            });
        }
        //------------------------------------------------------------------------
        private void DisplayRFIDTrace(string sLog)
        {
            lstRFID?.Invoke((Action)delegate ()
            {
                if (lstRFID.Items.Count >= 500)
                    lstRFID.Items.RemoveAt(lstRFID.Items.Count - 1);

                lstRFID.Items.Insert(0, sLog);
            });
        }

        //------------------------------------------------------------------------
        private void timerProc_Tick(object sender, EventArgs e)
        {
            //int    iVal;
            if (!this.Visible) { this.timerProc.Enabled = false; return; }

            timerProc.Enabled = false;

            m_bFlick = !m_bFlick;


            //
            DM.WAF[(int)EN_WAF_ID.WTR ].UpdateUnit(ref pbWaf1, true, true);
            DM.WAF[(int)EN_WAF_ID.WAT ].UpdateUnit(ref pbWaf2, true, true);
            DM.WAF[(int)EN_WAF_ID.ASM1].UpdateUnit(ref pbWaf3, true);
            DM.WAF[(int)EN_WAF_ID.ASM2].UpdateUnit(ref pbWaf4, true);

            DM.MGZ[(int)EN_MGZ_ID.MGZ1].Update(ref pbMGZ01);
            DM.MGZ[(int)EN_MGZ_ID.MGZ2].Update(ref pbMGZ02);

            //
            btogMFunc1.Checked = (SEQ.IsAllHomeEnd()) ? true : false;
            btogMFunc2.Checked = (LOT._bLotOpen) ? true : false;
            btogMFunc3.Checked = (SEQ.IsDoorLock()) ? true : false;
            //btogMFunc6   .Checked = false;

            //  
            lbAuto.Text = SEQ._bAutoMode ? "AUTO" : "TEACH";
            lbAuto.BackColor = SEQ._bAutoMode ? Color.Lime : Color.Gray;   //cDEF.IO.gX(EN_IN_ID.xSW_AutoMode)

            //lbFoupDoor1.BackColor = DM.MGZ[(int)EN_MGZ_ID.MGZ1]._bDoorOpen ? Color.Lime : Color.LightGray;
            //lbFoupDoor2.BackColor = DM.MGZ[(int)EN_MGZ_ID.MGZ2]._bDoorOpen ? Color.Lime : Color.LightGray;
            lbFoupDoor1.BackColor = (SEQ.LPM1.IsLocateWait()) ? Color.Lime : Color.LightGray;
            lbFoupDoor2.BackColor = (SEQ.LPM2.IsLocateWait()) ? Color.Lime : Color.LightGray;

            lbTarget01.Text = string.Format($"Targ: {DM.MGZ[(int)EN_MGZ_ID.MGZ1]._iTargerMC + 1}");
            lbTarget02.Text = string.Format($"Targ: {DM.MGZ[(int)EN_MGZ_ID.MGZ2]._iTargerMC + 1}");

            lbReqSlotNoL1.Text = SEQ.WTR.GetPickSlotNo(EN_MGZ_ID.MGZ1);
            lbReqSlotNoL2.Text = SEQ.WTR.GetPickSlotNo(EN_MGZ_ID.MGZ2);
            lbReqSlotNoL3.Text = SEQ.WTR.GetPlceSlotNo(EN_MGZ_ID.MGZ1);
            lbReqSlotNoL4.Text = SEQ.WTR.GetPlceSlotNo(EN_MGZ_ID.MGZ2);

            lbRFID_MGZ1.Text = string.Format($"RFID:{DM.MGZ[(int)EN_MGZ_ID.MGZ1]._sRFID}");
            lbRFID_MGZ2.Text = string.Format($"RFID:{DM.MGZ[(int)EN_MGZ_ID.MGZ2]._sRFID}");

            lbTRMode.Text    = string.Format($"[TR MODE] {SEQ.WTR.GetTRMode()}"); //

            lbDrngAlign.Visible  =  SEQ.WAT._bDrngAlgn && SEQ._bFlick1;
            lbDrngTR.Visible     = (SEQ.WTR._bDrngPick || SEQ.WTR._bDrngPlce) && SEQ._bFlick1;
            if (lbDrngTR.Visible) lbDrngTR.Text = SEQ.WTR._sWorkMsg;

            lbReqLoadSW01.Visible = SEQ._bReqLoadSW01;
            lbReqLoadSW02.Visible = SEQ._bReqLoadSW02;

            lbMgzLock1.BackColor = SEQ.LPM1.IsCylMGZLockUnlock(EN_MGZ_ID.MGZ1, true) ? Color.Lime : Color.Gray;
            lbMgzLock2.BackColor = SEQ.LPM2.IsCylMGZLockUnlock(EN_MGZ_ID.MGZ2, true) ? Color.Lime : Color.Gray;

            btLoad1.BackColor    = SEQ.LPM1.IsCylLoadUnload   (EN_MGZ_ID.MGZ1, true) ? Color.Lime : Color.Gray;
            btLoad2.BackColor    = SEQ.LPM2.IsCylLoadUnload   (EN_MGZ_ID.MGZ2, true) ? Color.Lime : Color.Gray;

            lbBarcode01.Text = DM.WAF[(int)EN_WAF_ID.ASM1]._sBarCodeNo;
            lbBarcode02.Text = DM.WAF[(int)EN_WAF_ID.ASM2]._sBarCodeNo;

            lbBarcodeCon.BackColor = BCR   ._IsConnect ? Color.Lime : Color.Gray;
            lbRFIDCon.BackColor    = RFID  ._IsConnect ? Color.Lime : Color.Gray;
          //lbFFUCon.BackColor     = FFU   ._bCon      ? Color.Lime : Color.Gray;
            lbASMCon.BackColor     = COMASM._IsConnect ? Color.Lime : Color.Red ;

            //
            UpdatewarnDisp();

            //
            lbCSTState01.Text = DM.MGZ[(int)EN_MGZ_ID.MGZ1].GetPortStatus().ToString();
            lbCSTState02.Text = DM.MGZ[(int)EN_MGZ_ID.MGZ2].GetPortStatus().ToString();

            //
            pnExstFoup1.BackColor  = SEQ.LPM1.IsExistMGZ(EN_MGZ_ID.MGZ1) ? Color.Lime : Color.LightGray;
            pnExstFoup2.BackColor  = SEQ.LPM2.IsExistMGZ(EN_MGZ_ID.MGZ2) ? Color.Lime : Color.LightGray;
            pnWFExistWAT.BackColor = IO.gX(EN_IN_ID.xWAT_ExistWafer)     ? Color.Lime : Color.LightGray;
            pnWFExistTR.BackColor  = IO.gX(EN_IN_ID.xWTR_ExistWafer)     ? Color.Lime : Color.LightGray;
            lbWafPressTR.BackColor = SEQ.WTR.IsVacStat(true)             ? Color.Lime : Color.LightGray;


            if (tabControl1.SelectedIndex == 4)
            {
                //btPortOperP1.Text = string.Format($"[P1]{COMASM.GetReqPortOper(EN_PORT_ID.P1)}");
                //btPortOperP2.Text = string.Format($"[P2]{COMASM.GetReqPortOper(EN_PORT_ID.P2)}"); 
                
                btPortOperP1.Text = string.Format($"[P1] {DM.MGZ[(int)EN_MGZ_ID.MGZ1].GetPortOper()}");
                btPortOperP2.Text = string.Format($"[P2] {DM.MGZ[(int)EN_MGZ_ID.MGZ2].GetPortOper()}");

                //btTransportP1.Text = string.Format($"[TR MODE]{COMASM.GetReqTransport()}"); //
                btTransportP1.Text = string.Format($"[TR MODE] {SEQ.WTR.GetTRMode()}"); //

                //btPortModeP1.Text = string.Format($"[P1]{COMASM.GetReqPortMode(EN_PORT_ID.P1)}"); //
                //btPortModeP2.Text = string.Format($"[P2]{COMASM.GetReqPortMode(EN_PORT_ID.P2)}"); //
                btPortModeP1.Text = string.Format($"[P1] {DM.MGZ[(int)EN_MGZ_ID.MGZ1].GetPortMode()}"); //
                btPortModeP2.Text = string.Format($"[P2] {DM.MGZ[(int)EN_MGZ_ID.MGZ2].GetPortMode()}"); //
            }

            for (int n = 0; n < (int)EN_WTR_WORK_AREA.EndOfId; n++)
            {
                lbIon[n].BackColor = GetIonColor(n); // IO.gX((EN_IN_ID)lbIon[n].Tag)? Color.Lime : Color.LightGray; 
            }

            lbArea1.BackColor = IO.gX((int)EN_IN_ID.xLPM_PORT_Area) ? (SEQ._bFlick1 ? Color.Yellow : Color.Red) : FRM.GetGridBackColor();
            lbArea2.BackColor = IO.gX((int)EN_IN_ID.xLPM_PORT_Area) ? (SEQ._bFlick1 ? Color.Yellow : Color.Red) : FRM.GetGridBackColor();


            grManFunc.Enabled = !SEQ._bRun && !SEQ._bAutoMode;

            btogMFunc1.Enabled = !SEQ._bRun;
            //btogMFunc2.Enabled = !SEQ._bRun;
            btogMFunc3.Enabled = !SEQ._bRun;
            btogMFunc4.Enabled = !SEQ._bRun;

            btogMFunc6.Enabled = !SEQ._bRun;
            btogMFunc7.Enabled = !SEQ._bRun;

            //Port Oper Status
            lbReqOperLPM1.Visible = SEQ._bFlick1 && DM.MGZ[(int)EN_MGZ_ID.MGZ1].GetPortOper() > EN_PORT_OPER.none;
            lbReqOperLPM2.Visible = SEQ._bFlick1 && DM.MGZ[(int)EN_MGZ_ID.MGZ2].GetPortOper() > EN_PORT_OPER.none;
            lbReqOperLPM1.Text = DM.MGZ[(int)EN_MGZ_ID.MGZ1].GetPortOper().ToString();
            lbReqOperLPM2.Text = DM.MGZ[(int)EN_MGZ_ID.MGZ2].GetPortOper().ToString();



            /*
             
            //
            lbLotNo .Text = cDEF.LOT.Info.sLotNo1; lbLotNo .BackColor = cDEF.LOT._bLotOpen ? (!cDEF.SEQ.m_bFlick2 ? Color.Lime : Color.Transparent) : Color.Transparent;
            lbPartNo.Text = cDEF.LOT.Info.sPartNo; lbPartNo.BackColor = cDEF.LOT._bLotOpen ? (!cDEF.SEQ.m_bFlick2 ? Color.Lime : Color.Transparent) : Color.Transparent;
            lbRecipe.Text = cDEF.FM._sCrntDevice ; lbRecipe.BackColor = cDEF.LOT._bLotOpen ? (!cDEF.SEQ.m_bFlick2 ? Color.Lime : Color.Transparent) : Color.Transparent;
            lbStat01.Text = cDEF.FM.ProjBase.iWafType == (int)EN_WAF_TYPE.Inch5 ? "5 INCH" : "6 INCH";
            lbStat02.Text = ((EN_UMZ_WORK_MODE)FM.ProjBase.iUMZWorkMode).ToString().ToUpper();
            lbStat03.Text = cDEF.FM.m_sCrntOperID;


            lbStat10.Text = LOT.LotQty.iWorkQty.ToString();
            lbStat11.Text = string.Format($"TOP:{LOT.LotQty.iLoadQty}/BTM:{LOT.LotQty.iUnloadQty}");
            //lbStat12.Text = string.Format($"{cDEF.SEQ.WLM.m_dScanTime[0]/1000:##0.##} s / {3600/(cDEF.SEQ.WLM.m_dScanTime[0]/1000):#0.#}");
            //lbStat13.Text = string.Format("{0}", (int)cDEF.LOT.GetUPEH());
            lbStat12.Text = string.Format($"{cDEF.SEQ.WLM.m_dScanTime[0] / 1000:#0.##} s");
            lbStat13.Text = string.Format($"{3600 / (cDEF.SEQ.WLM.m_dScanTime[0] / 1000):#0}"); //JUNG/220209/UPEH 삭제

            //TimeSpan dateDiff = cDEF.SPC.m_tDayChangeTime - DateTime.Now;
            //lbStat20.Text = cDEF.SPC.ConvetSapnTimeToStr(DateTime.Now, cDEF.SPC.m_tDayChangeTime); //

            //lbStat21.Text = cDEF.SPC.dbRunTimeProc.GetRunTimeforDay().ToString(); 
            lbStat21.Text = cDEF.SPC.ConvTimeTickToStr(cDEF.SPC.DAILY_DATA.dRunTime);
            lbStat22.Text = cDEF.SPC.ConvTimeTickToStr(cDEF.SPC.DAILY_DATA.dErrorTime);
            lbStat23.Text = cDEF.SPC.SPC_EFF.iJamCnt.ToString();

            //FM.ProjBase.iUMZWorkMode == (int)EN_UMZ_WORK_MODE.UnloadOnly? 
            lbMGZMode.Text = "[CST MODE : " + ((EN_UMZ_WORK_MODE)FM.ProjBase.iUMZWorkMode).ToString().ToUpper() + "]";

            //pgKnifeLifeTotal.Value = FM.SysCnt.iKnifeTotUseCnt; 
            lbKnifeLife.Text = string.Format($"{FM.SysCnt.iKnifeTotUseCnt} [{FM.SysCnt.iKnifeUseStep+1} / {FM.EngrOptn.nKnifeUseStepCnt}회]");
            pgKnifeStepCnt.Maximum = FM.EngrOptn.iKnifeChangeCnt;
            pgKnifeStepCnt.Value   = (FM.SysCnt.iKnifeUseCnt > pgKnifeStepCnt.Maximum) ? pgKnifeStepCnt.Maximum : FM.SysCnt.iKnifeUseCnt;
            lbStepCnt     .Text    = FM.SysCnt.iKnifeUseCnt.ToString();

            lbKnifeCnt01.Text = FM.SysCnt.iKnifeCntofThic[0].ToString();
            lbKnifeCnt02.Text = FM.SysCnt.iKnifeCntofThic[1].ToString();

            lbUserSetDepth.Text = string.Format($"{FM.EngrOptn.dKnifeChangeDepth}");
            lbSetStepValue.Text = "/ " + FM.EngrOptn.iKnifeChangeCnt.ToString();

            if(FM.EngrOptn.bUseKnifeMan)
            {
                lbCountSkip.ForeColor = Color.Red;
                lbCountSkip.Text      = "Count Skip!!!";
                lbCountSkip.Visible   = (SEQ.WLM._bChkSkipKnife && SEQ.m_bFlick1) ? true : false;
            }
            else
            {
                lbCountSkip.ForeColor = Color.Blue; 
                lbCountSkip.Text      = "[Limit Skip]"; 
                lbCountSkip.Visible   = true; 
            }

            //
            pbPress.Value = (int)cDEF.FM.ProjBase.dTapeAttachForce + (int)cDEF.FM.ProjBase.dForceOffset;
            lbPress.Text = string.Format("{0:F1}kgf", cDEF.FM.ProjBase.dTapeAttachForce + cDEF.FM.ProjBase.dForceOffset);

            //
            aGauge1.Value = cDEF.TempAutonics.GetPV((int)EN_TEMP_CH.tpcKnife);
            aGauge2.Value = cDEF.TempAutonics.GetPV((int)EN_TEMP_CH.tpcChuck);

            aGauge1.GaugeLabels[0].Text = aGauge1.Value.ToString() + "˚c";
            aGauge2.GaugeLabels[0].Text = aGauge2.Value.ToString() + "˚c";
            aGauge1.GaugeLabels[1].Text = string.Format("{0}˚c", (int)cDEF.FM.ProjBase.dTopTemp);
            aGauge2.GaugeLabels[1].Text = string.Format("{0}˚c", (int)cDEF.FM.ProjBase.dBtmTemp);


            pnIonOn01.BackColor = cDEF.IO.gY(EN_OUT_ID.yION_WAT_PowOn     ) ? Color.Lime : Color.Gray;
            pnIonOn02.BackColor = cDEF.IO.gY(EN_OUT_ID.yION_WLT_PowOnLeft ) ? Color.Lime : Color.Gray;
            pnIonOn03.BackColor = cDEF.IO.gY(EN_OUT_ID.yION_WLT_PowOnRight) ? Color.Lime : Color.Gray;
            pnIonOn04.BackColor = cDEF.IO.gY(EN_OUT_ID.yION_ST_PowOn      ) ? Color.Lime : Color.Gray;
            pnIonOn05.BackColor = cDEF.IO.gY(EN_OUT_ID.yION_UT_PowOn      ) ? Color.Lime : Color.Gray;

            //
            lbWorkMsg.Visible = cDEF.SEQ.WLM._bDrngLami && cDEF.SEQ.m_bFlick2;
            if (cDEF.SEQ.WLM._bDrngLami)
            {
                lbWorkMsg.ForeColor = Color.Lime;
                lbWorkMsg.BackColor = Color.FromArgb(66, 72, 88);
                lbWorkMsg.Text      = cDEF.SEQ.WLM._sWorkMsg;
            }

            lbWorkAlign.Visible = cDEF.SEQ.WAT._bDrngAlgn && cDEF.SEQ.m_bFlick2;
            if (cDEF.SEQ.WAT._bDrngAlgn)
            {
                lbWorkAlign.ForeColor = Color.Orange;
                lbWorkAlign.BackColor = Color.FromArgb(66, 72, 88);
                lbWorkAlign.Text      = cDEF.SEQ.WAT._sWorkMsg;
            }

            //
            pnErrChkLD01.BackColor = cDEF.IO.gX(EN_IN_ID.xLMZ_Protrusion5) ? Color.Red : Color.Gray;
            pnErrChkLD02.BackColor = cDEF.IO.gX(EN_IN_ID.xLMZ_Protrusion6) ? Color.Red : Color.Gray;
            pnErrChkUD01.BackColor = cDEF.IO.gX(EN_IN_ID.xUMZ_Protrusion5) ? Color.Red : Color.Gray;
            pnErrChkUD02.BackColor = cDEF.IO.gX(EN_IN_ID.xUMZ_Protrusion6) ? Color.Red : Color.Gray;

            btogMFunc6.Checked = (IO.gY(EN_OUT_ID.ySYS_MC_Heater01) || IO.gY(EN_OUT_ID.ySYS_MC_Heater02)) ? true : false;

            //Remain Time
            double dScanTime = cDEF.SEQ.WLM.m_dScanTime[0];
            if (dScanTime < 40000 || dScanTime > 60000) dScanTime = 48.0 * 1000; //Spec. 75ea/1h
            double dTotalTime = DM.GetNeedWorkMap() * dScanTime;
            lbRemainTime.Text = string.Format($"{cDEF.SPC.ConvTimeTickToStr(dTotalTime)} / {DM.GetNeedWorkMap()}ea");



            //
            bool bLMZWork = cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ].IsOneStat(EN_WAFER_STAT.Mount) && SEQ.WTR._nLastPickArea == (int)EN_MGZ_ID.LMZ;
            bool bUMZWork = cDEF.DM.MGZ[(int)EN_MGZ_ID.UMZ].IsOneStat(EN_WAFER_STAT.Mount) && SEQ.WTR._nLastPickArea == (int)EN_MGZ_ID.UMZ;
            bool bFnshLMZ = SEQ.WTR.IsFinishMgz(EN_MGZ_ID.LMZ) && SEQ.WTR.ChkExistMgz(EN_MGZ_ID.LMZ);
            bool bFnshUMZ = SEQ.WTR.IsFinishMgz(EN_MGZ_ID.UMZ) && SEQ.WTR.ChkExistMgz(EN_MGZ_ID.UMZ);

            lbWorkLMZ.Text      = bLMZWork ? "작업중" : (bFnshLMZ ? "완료" : "대기");
            lbWorkLMZ.BackColor = bLMZWork ? Color.Lime : (bFnshLMZ ? Color.Brown : Color.Gray);
            lbWorkUMZ.Text      = bUMZWork ? "작업중" : (bFnshUMZ ? "완료" : "대기");
            lbWorkUMZ.BackColor = bUMZWork ? Color.Lime : (bFnshUMZ ? Color.Brown : Color.Gray);

            //
            tgMan02.Checked = cDEF.MOTR[(int)EN_MOTR_ID.TCK_R].GetServo() && cDEF.MOTR[(int)EN_MOTR_ID.TCK_R].GetHomeEnd() ? true : false ;

            tgMan03.Checked = cDEF.IO.gY(EN_OUT_ID.yWTR_A_VacOn) || SEQ.WTR.IsVacStat(EN_WAF_ID.WTR_A, true);
            tgMan04.Checked = cDEF.IO.gY(EN_OUT_ID.yWTR_B_VacOn) || SEQ.WTR.IsVacStat(EN_WAF_ID.WTR_B, true);
            tgMan05.Checked = cDEF.IO.gY(EN_OUT_ID.yWAT_VacOn  ) || SEQ.WAT.IsVacStat(true);
            tgMan06.Checked = cDEF.IO.gY(EN_OUT_ID.yWLT_InVacOn) || SEQ.WLM.IsVacStat(vDEF.INNER, true);

            //
            lbScanSkip.Visible = cDEF.FM.EngrOptn.bUseScanFuc?  false : true;
            lbScanSkip.ForeColor = Color.Blue;

            //
            btWFStop.Checked = SEQ.WTR._bInputHold;

            //
            btLight.Checked = SEQ._TempLampOn;

            //
            grKnifeFunc.Enabled = !SEQ._bAutoMode;

            //
            //lpnHotKey.Enabled   = !SEQ._bRun;
            btogMFunc1.Enabled = !SEQ._bRun;
            btogMFunc2.Enabled = !SEQ._bRun;
            btogMFunc3.Enabled = !SEQ._bRun;
            btogMFunc4.Enabled = !SEQ._bRun;
            btogMFunc6.Enabled = !SEQ._bRun;
            btogMFunc7.Enabled = !SEQ._bRun;
            */


            //
            timerProc.Enabled = true;
        }
        //------------------------------------------------------------------------
        private Color GetIonColor(int no)
        {
            Color rtnColor = Color.Black;

            EN_IN_ID xRun = EN_IN_ID.xNone;
            EN_IN_ID xAlrm = EN_IN_ID.xNone;
            EN_OUT_ID yRun = EN_OUT_ID.yNone;

            switch ((EN_WTR_WORK_AREA)no)
            {
                case EN_WTR_WORK_AREA.MGZ1:
                    //xRun  = EN_IN_ID.xLPM_PORT1_Ionizer_Run  ;
                    xAlrm = EN_IN_ID.xLPM_PORT1_Ionizer_Alarm;
                    yRun = EN_OUT_ID.yLPM_PORT1_ION_RUN;
                    break;
                case EN_WTR_WORK_AREA.MGZ2:
                    //xRun  = EN_IN_ID.xLPM_PORT2_Ionizer_Run  ;
                    xAlrm = EN_IN_ID.xLPM_PORT2_Ionizer_Alarm;
                    yRun = EN_OUT_ID.yLPM_PORT2_ION_RUN;
                    break;
                case EN_WTR_WORK_AREA.WAT:
                    //xRun  = EN_IN_ID.xWAT_Ionizer_Run  ;
                    xAlrm = EN_IN_ID.xWAT_Ionizer_Alarm;
                    yRun = EN_OUT_ID.yWAT_ION_RUN;
                    break;
                case EN_WTR_WORK_AREA.ASM1:
                    //xRun  = EN_IN_ID.xMMC_MC1_Ionizer_Run  ;
                    xAlrm = EN_IN_ID.xMMC_MC1_Ionizer_Alarm;
                    yRun = EN_OUT_ID.yMMC_MC1_ION_RUN;

                    break;
                case EN_WTR_WORK_AREA.ASM2:
                    //xRun  = EN_IN_ID.xMMC_MC2_Ionizer_Run  ;
                    xAlrm = EN_IN_ID.xMMC_MC2_Ionizer_Alarm;
                    yRun = EN_OUT_ID.yMMC_MC1_ION_RUN;
                    break;
                default:
                    rtnColor = Color.Black;
                    break;
            }

            if (IO.gX(xAlrm)) rtnColor = Color.Red;
            else if (IO.gX(xRun) || IO.gY(yRun)) rtnColor = Color.Lime;
            else rtnColor = Color.Silver;

            return rtnColor;
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
        private void btogMFunc1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            bool isAllCloseDoor = !cDEF.SEQ.IsOpenAnyDoor();
            string sTemp = string.Empty;

            if ((SEQ._bRun) && ((iTag != 4) && (iTag != 6)))
            {
                MsgBox.Warning("The parameter can not be changed while the Machine is running.");
                return;
            }

            switch (iTag)
            {
                default: break;
                case 0:
                    //Initial
                    if (!FRM.ShowMsg(true, " Confirm ", "Do you want to perform a All initialization?", EN_MSG_KIND.UserModal)) return;
                    if ((cDEF.FM.m_iCrntLevel == (int)EN_LOGIN.Operator) && !isAllCloseDoor)
                    {
                        MsgBox.Warning("The door is open and you can not perform initialization");
                        return;
                    }
                    //
                    cDEF.MAN.ManProcOn(1, true, false);
                    cDEF.LOG.Trace("All Home Start");
                    break;

                case 1:
                    //Lot Open
                    //if (!cDEF.SEQ.WTR.ChkExistMgz(EN_MGZ_ID.LMZ) && !cDEF.SEQ.WTR.ChkExistMgz(EN_MGZ_ID.UMZ))
                    //{
                    //    if (!FRM.ShowMsg(true, " Confirm ", "작업할 Cassette를 Loading 후 Lot Open을 실행하십시오.", EN_MSG_KIND.UserModal)) return;
                    //    return; 
                    //}

                    FrmLot2 frmLot = new FrmLot2();
                    frmLot.ShowDialog();
                    break;

                case 2:
                    //Door Open
                    cDEF.IO.sY(EN_OUT_ID.yDR_Lock_Left , !cDEF.IO.gY(EN_OUT_ID.yDR_Lock_Left ));
                    cDEF.IO.sY(EN_OUT_ID.yDR_Lock_Right, !cDEF.IO.gY(EN_OUT_ID.yDR_Lock_Right));

                    sTemp = IO.gY(EN_OUT_ID.yDR_Lock_Left) ? "Close" : "Open";
                    LOG.Trace($"Door {sTemp}");

                    break;

                case 3:
                    //Set Operator ID
                    //FrmChangeOperID FrmOperID = new FrmChangeOperID();
                    //FrmOperID.ShowDialog();

                    break;

                case 4:
                    //Login.
                    try
                    {
                        FRM.Login.ShowDialog();
                    }
                    catch (Exception err) { System.Diagnostics.Debug.WriteLine("Exception:" + err.Message); }
                    break; //

                case 5: //LPM#1 Unload
                    if (SEQ.LPM1.IsCylLoaded()) MAN.ManProcOn(4005, true, false); //Unload
                    break;

                case 6://LPM#2 Unload
                    if (SEQ.LPM2.IsCylLoaded()) MAN.ManProcOn(4105, true, false); //Unload
                    break;

                case 7:
                    MAN.ManProcOn(2500, false, true); //Transfer vacuum Off
                    DM.WAF[(int)EN_WAF_ID.WTR].ClearData();
                    break;

            }
        }
        //------------------------------------------------------------------------
        private bool SetPopMenuUpdate(int iKind)
        {//Kind - 0 : One, 1 : All
            ToolStripMenuItem ts = new ToolStripMenuItem();
            //
            MenuStrip.ItemClicked -= new ToolStripItemClickedEventHandler(menuItemOne_Click);
            MenuStrip.ItemClicked -= new ToolStripItemClickedEventHandler(menuItemAll_Click);

            //
            MenuStrip.Items.Clear();
            MenuStrip.Items.Add(ts);

            //
            ts.Text = "Wafer Map";
            ts.Font = new Font("Tahoma", 12.0f);
            ts.BackColor = Color.FromArgb(218, 241, 252);

            //
            for (int i = 0; i < vDEF.STR_WAF_STAT.Length; i++) MenuStrip.Items.Add(string.Format("{0} - {1}", (iKind == 0) ? "One" : "All", vDEF.STR_WAF_STAT[i]));

            if (iKind == 0) MenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(menuItemOne_Click);
            else MenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(menuItemAll_Click);
            //
            return true;
        }
        //------------------------------------------------------------------------
        private void menuItemOne_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            int iTag = -1;
            string sStr = "";
            //EN_WAFER_STAT  iStat = EN_WAFER_STAT.None;
            //
            if (cDEF.SEQ._bRun)
            {
                MsgBox.Warning("Can not be Change while the Machine is running.");
                return;
            }
            //
            for (int n = 0; n < vDEF.STR_WAF_STAT.Length; n++)
            {
                sStr = e.ClickedItem.Text.Substring(6, e.ClickedItem.Text.Length - 6);
                if (vDEF.STR_WAF_STAT[n].ToLower() == sStr.ToLower()) { iTag = n; break; }
            }

            ////
            //switch(iTag) 
            //{
            //    default : return;
            //    case  0  : iStat = EN_WAFER_STAT.None   ; break;
            //    case  1  : iStat = EN_WAFER_STAT.Empty  ; break;
            //    case  2  : iStat = EN_WAFER_STAT.Mask   ; break;
            //    case  3  : iStat = EN_WAFER_STAT.Mount  ; break;
            //    case  4  : iStat = EN_WAFER_STAT.Aligned; break;
            //    case  5  : iStat = EN_WAFER_STAT.Skip   ; break;
            //    case  6  : iStat = EN_WAFER_STAT.Fnsh   ; break;
            //    case  7  : iStat = EN_WAFER_STAT.Work   ; break;
            //    case  8  : iStat = EN_WAFER_STAT.Wait   ; break;
            //    case  9  : iStat = EN_WAFER_STAT.Fail   ; break;
            //}

            //
            if (m_iSelWhre == 0) cDEF.DM.MGZ[m_iSelPart].SetTo(m_iSelRow, (EN_WAFER_STAT)iTag);
            else cDEF.DM.WAF[m_iSelPart].SetTo((EN_WAFER_STAT)iTag);

            sStr = Enum.GetName(typeof(EN_WAF_ID), m_iSelPart) + " Wafer Unit";

            //
            LOG.Trace($"[USER] ItemOne_Click {sStr} Click - {(EN_WAFER_STAT)iTag}");
        }
        //------------------------------------------------------------------------
        private void menuItemAll_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            int iTag = -1;
            string sStr = "";
            EN_WAFER_STAT iStat = EN_WAFER_STAT.None;

            //
            if (m_iSelWhre != 0) return;

            //
            if (cDEF.SEQ._bRun)
            {
                MsgBox.Warning("Can not be Change while the Machine is running.");
                return;
            }

            //
            for (int n = 0; n < vDEF.STR_WAF_STAT.Length; n++)
            {
                sStr = e.ClickedItem.Text.Substring(6, e.ClickedItem.Text.Length - 6);
                if (vDEF.STR_WAF_STAT[n].ToLower() == sStr.ToLower()) { iTag = n; break; }
            }

            if (iTag < 0) return;

            ////
            //switch(iTag) 
            //{
            //    default  : return;
            //    case  0  : iStat = EN_WAFER_STAT.Empty  ; break;
            //    case  1  : iStat = EN_WAFER_STAT.Mask   ; break;
            //    case  2  : iStat = EN_WAFER_STAT.Mount  ; break;
            //    case  3  : iStat = EN_WAFER_STAT.Aligned; break;
            //    case  4  : iStat = EN_WAFER_STAT.Skip   ; break;
            //    case  5  : iStat = EN_WAFER_STAT.Fnsh   ; break;
            //    case  6  : iStat = EN_WAFER_STAT.Work   ; break;
            //    case  7  : iStat = EN_WAFER_STAT.Wait   ; break;
            //    case  8  : iStat = EN_WAFER_STAT.Fail   ; break;
            //}

            //
            cDEF.DM.MGZ[m_iSelPart].SetTo((EN_WAFER_STAT)iTag);
            if ((EN_WAFER_STAT)iTag == EN_WAFER_STAT.None && (EN_MGZ_ID)m_iSelPart == EN_MGZ_ID.MGZ1) cDEF.DM.MGZ[m_iSelPart].ClearMap();
            if ((EN_WAFER_STAT)iTag == EN_WAFER_STAT.Empty && (EN_MGZ_ID)m_iSelPart == EN_MGZ_ID.MGZ1) cDEF.DM.MGZ[m_iSelPart].ClearMap();
            if ((EN_WAFER_STAT)iTag == EN_WAFER_STAT.None && (EN_MGZ_ID)m_iSelPart == EN_MGZ_ID.MGZ2) cDEF.DM.MGZ[m_iSelPart].ClearMap();
            if ((EN_WAFER_STAT)iTag == EN_WAFER_STAT.Empty && (EN_MGZ_ID)m_iSelPart == EN_MGZ_ID.MGZ2) cDEF.DM.MGZ[m_iSelPart].ClearMap();

            //
            sStr = Enum.GetName(typeof(EN_MGZ_ID), m_iSelPart) + " Load Unit";
            LOG.Trace($"[USER] ItemAll_Click {sStr} Click - {((EN_WAFER_STAT)iTag)}");
        }

        //------------------------------------------------------------------------
        private void pbCST_MouseDown(object sender, MouseEventArgs e)
        {
            //
            if (FM.IsOperLv()) return;

            //
            bool isRising = false;
            PictureBox pBox = (sender as PictureBox);
            int iTag = Convert.ToInt32(pBox.Tag);
            m_iSelPart = iTag;

            //Magazine
            if (iTag == 0)
            {
                m_iSelWhre = 0;
                m_iSelPart = iTag;
                cDEF.DM.MGZ[iTag].GetImageRC(ref pBox, e.X, e.Y, out m_iSelRow);
            }
            else if (iTag >= 9 && iTag <= 16)
            {
                //WTR_T = 0   ,
                //WTR_B       ,
                //WAT         ,
                //WTB         ,
                //BUFF        ,
                //BUFF1       ,
                //BUFF2       ,
                //BUFF3       ,
                m_iSelWhre = 1;
                m_iSelPart = iTag - 9;
            }
            else return;

            //
            if (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left) { isRising = SetPopMenuUpdate(0); } //One
            else if (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Right) { isRising = SetPopMenuUpdate(1); } //All
            else return;

            if (isRising) MenuStrip.Show((sender as PictureBox), new Point(e.X, e.Y));
            else MenuStrip.Hide();
        }
        //------------------------------------------------------------------------
        private void pnFoupDoor_DoubleClick(object sender, EventArgs e)
        {
            Panel spn = sender as Panel;
            int iTag = Convert.ToInt32(spn.Tag);

            DM.MGZ[iTag]._bDoorOpen = !DM.MGZ[iTag]._bDoorOpen;

            switch (iTag)
            {
                case 0:
                    break;
                case 1:
                    break;

                default:
                    break;
            }

            //Door 

        }
        //------------------------------------------------------------------------
        private void btAllClear_Click(object sender, EventArgs e)
        {
            //
            DM.ClearMap();
        }
        //------------------------------------------------------------------------
        private void pbWaf1_MouseDown(object sender, MouseEventArgs e)
        {
            if (FM.m_iCrntLevel == (int)EN_LOGIN.Operator) return;

            //
            bool isRising = false;

            PictureBox pBox = (sender as PictureBox);
            int iTag = Convert.ToInt32(pBox.Tag);
            m_iSelPart = iTag;

            //Magazine
            if ((iTag == 0) || (iTag == 1))
            {
                m_iSelWhre = 0;
                m_iSelPart = iTag;
                DM.MGZ[iTag].GetImageRC(ref pBox, e.X, e.Y, out m_iSelRow);
            }
            else if ((iTag == 10) || (iTag == 11) || (iTag == 12) || (iTag == 13)) //Wafer
            {
                m_iSelWhre = 1;
                m_iSelPart = iTag - 10;
            }
            else return;

            //
            if      (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left) { isRising = SetPopMenuUpdate(0); } //One
            else if (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Right) { isRising = (SetPopMenuUpdate(1) && m_iSelWhre == 0); } //All
            else return;

            if (isRising) MenuStrip.Show((sender as PictureBox), new Point(e.X, e.Y));
            else MenuStrip.Hide();
        }
        //------------------------------------------------------------------------
        private void btLoad1_Click(object sender, EventArgs e)
        {
            if (!FM.IsMasterLv()) return;
            if (SEQ._bRun) return;
            //
            Button sbt = sender as Button;
            int nTag = 0;
            int.TryParse(sbt.Tag.ToString(), out nTag);

            if (SEQ.LPM1.IsCylLoadUnload((EN_MGZ_ID)nTag, false)) SEQ.LPM1.MoveCylMGZLoad((EN_MGZ_ID)nTag);
            else SEQ.LPM1.MoveCylMGZUnload((EN_MGZ_ID)nTag);
        }
        //------------------------------------------------------------------------
        private void lbMgzLock1_Click(object sender, EventArgs e)
        {
            if (!FM.IsMasterLv()) return;
            if (SEQ._bRun) return;

            Label sbt = sender as Label;
            int nTag = 0;
            int.TryParse(sbt.Tag.ToString(), out nTag);
            EN_MGZ_ID mid = (EN_MGZ_ID)(nTag - 10);

            if (SEQ.LPM1.IsCylMGZLockUnlock(mid, false)) SEQ.LPM1.MoveCylMGZLock(mid);
            else SEQ.LPM1.MoveCylMGZUnlock(mid);
        }
        //------------------------------------------------------------------------
        private void tgMan01_Click(object sender, EventArgs e)
        {
            if (SEQ._bRun || SEQ._bAutoMode) return;

            int m_nSelTarget = cbSelWhre.SelectedIndex;
            int m_nSelSlot   = cbSelSlot.SelectedIndex;

            bool bMGZPart = (m_nSelTarget == (int)EN_WTR_WORK_AREA.MGZ1) || (m_nSelTarget == (int)EN_WTR_WORK_AREA.MGZ2);
            if (bMGZPart)
            {
                if (m_nSelSlot < 0 || m_nSelSlot > cDEF.DM.MGZ[(int)EN_MGZ_ID.MGZ1]._iMaxSlot)
                {
                    FRM.ShowWarn(true, string.Format("Slot을 지정해 주세요."));
                    return;
                }
            }

            KToggleButton sbt = sender as KToggleButton;
            int nTag = Convert.ToInt32(sbt.Tag);
            string sTxt = sbt.Text2;

            if (!FRM.ShowMsg(true, "Confirm", string.Format($"[{sTxt}] Manual 동작을 진행 하시겠습니까?"))) return;

            MAN.FuncArg.iArg1 = m_nSelTarget;
            MAN.FuncArg.iArg2 = m_nSelSlot; //

            MAN.ManProcOn(nTag, true, false);

        }
        //------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            //
            string sSTtoString = "C001011";
            ST_R001_ONLINE_REQUEST stC001 = new ST_R001_ONLINE_REQUEST();
            stC001 = FNC.StrToStruct<ST_R001_ONLINE_REQUEST>(sSTtoString);

            Debug.WriteLine(stC001.ID);
            Debug.WriteLine(stC001.ToString());

        }
        //------------------------------------------------------------------------
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //
            int nSelfrom  = cbSelWhreFrom.SelectedIndex;
            int nslotfrom = cbSelSlotFrom.SelectedIndex;

            if (nSelfrom < 0) return;

            if ((nSelfrom == 0 || nSelfrom == 1) && nslotfrom < 0) return;
            
            cbSelWhreFrom.SelectedIndex = cbSelWhreTo.SelectedIndex;
            cbSelSlotFrom.SelectedIndex = cbSelSlotTo.SelectedIndex;
            
            cbSelWhreTo.SelectedIndex = nSelfrom ;
            cbSelSlotTo.SelectedIndex = nslotfrom;
        }
        //------------------------------------------------------------------------
        private void kToggleButton2_Click(object sender, EventArgs e)
        {
            //
            if (SEQ._bRun || SEQ._bAutoMode) return;

            int m_nSelFrom     = cbSelWhreFrom.SelectedIndex;
            int m_nSelSlotFrom = cbSelSlotFrom.SelectedIndex;

            int m_nSelTo       = cbSelWhreTo  .SelectedIndex;
            int m_nSelSlotTo   = cbSelSlotTo  .SelectedIndex;

            bool bMGZPartFr = (m_nSelFrom == (int)EN_WTR_WORK_AREA.MGZ1) || (m_nSelFrom == (int)EN_WTR_WORK_AREA.MGZ2);
            bool bMGZPartTo = (m_nSelTo   == (int)EN_WTR_WORK_AREA.MGZ1) || (m_nSelTo   == (int)EN_WTR_WORK_AREA.MGZ2);
            if (bMGZPartFr)
            {
                if (m_nSelSlotFrom < 0 || m_nSelSlotFrom > cDEF.DM.MGZ[(int)EN_MGZ_ID.MGZ1]._iMaxSlot)
                {
                    FRM.ShowWarn(true, string.Format("Form Slot을 지정해 주세요."));
                    return;
                }
            }
            else if (bMGZPartTo)
            {
                if (m_nSelSlotTo < 0 || m_nSelSlotTo > cDEF.DM.MGZ[(int)EN_MGZ_ID.MGZ1]._iMaxSlot)
                {
                    FRM.ShowWarn(true, string.Format("To Slot을 지정해 주세요."));
                    return;
                }
            }
            
            //
            KToggleButton sbt = sender as KToggleButton;
            string sTxt       = sbt.Text2;

            if (!FRM.ShowMsg(true, "Confirm", string.Format($"[{sTxt}] Manual 동작을 진행 하시겠습니까?"))) return;

            MAN.FuncArg.iArg1 = m_nSelFrom    ;
            MAN.FuncArg.iArg2 = m_nSelSlotFrom; //
            MAN.FuncArg.iArg3 = m_nSelTo      ;
            MAN.FuncArg.iArg4 = m_nSelSlotTo  ; //

            MAN.ManProcOn(2002, true, false);

        }
    }
}