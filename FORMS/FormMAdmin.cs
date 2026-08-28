using System;
using System.Drawing;
using System.Windows.Forms;
using static eMachine.cDEF;

namespace eMachine
{
    public partial class FrmMAdmin : Form
    {
        //
        FrmCtrlMC FrmCtlBtn = new FrmCtrlMC(0);
        //
		int   m_iSelPage      ;
        int   m_iOneShotTag   ;
        int   m_iSelWhre      ;
        int   m_iSelPart      ;
        int   m_iSelRow       ;
        int   m_iOneShotAuto  ;
        bool  actived         ;

        //
        ContextMenuStrip MenuStrip = new ContextMenuStrip(); 


        public FrmMAdmin()
        {
            InitializeComponent();
            //
            Panel           pn;
            Label           lb;
            TabPage         tp;
            GroupBox        gb;
            RadioButton     rb;
            Control[] ctls = FNC.GetAllControlsUsingRecursive(this);
            //
            this.BackColor = FRM.GetBaseColor();
            foreach (Control ctl in ctls)
            {
                if (ctl.GetType().Name.ToLower() == "panel")
                {
                    pn = ctl as Panel;
                    pn.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "label")
                {
                    lb = ctl as Label;
                    lb.ForeColor = FRM.GetForeColor();
                }
                else if (ctl.GetType().Name.ToLower() == "tabpage")
                {
                    tp = ctl as TabPage;
                    tp.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "groupbox")
                {
                    gb = ctl as GroupBox;
                    gb.ForeColor = FRM.GetForeColor();
                }
                else if (ctl.GetType().Name.ToLower() == "radiobutton")
                {
                    rb = ctl as RadioButton;
                    rb.ForeColor = FRM.GetForeColor();
                }
            }

            //
            cbMotor.Items.Clear();
            for (EN_MOTR_ID n = 0; n < EN_MOTR_ID.EndOfId; n++) cbMotor.Items.Add(string.Format($"{n}"));

        }

        private void FrmMAdmin_Load(object sender, EventArgs e)
        {

            //TabControl Tab 제거
            Rectangle Rect = new Rectangle(tpgMenu1.Left, tpgMenu1.Top, tpgMenu1.Width, tpgMenu1.Height);
            tpgMenu4.Region = new Region(Rect);
            btnSave.Visible = false;

            FRM.SetFormParent(FRM.SMotor  ,this.pnBaseMotr );
            FRM.SetFormParent(FrmCtlBtn   ,this.pnHandle   );

	        m_iOneShotTag  = -1;   
			m_iSelPage     =  0;
            m_iOneShotAuto = -1; 
            
            tmProc.Interval    = 100;
            tmOneShot.Interval = 50 ;
        }

        private void FrmMAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc    .Enabled = false;
            tmOneShot .Enabled = false;
            cDEF.MOTR .m_bSkipChkCrash = false;   
        }

        private void FrmMAdmin_VisibleChanged(object sender, EventArgs e)
        {
             if(this.Visible && !actived) 
             {//Show
                if(actived) return;
                FRM.ShowFormParent(FrmCtlBtn  ,this.pnHandle);
                //
                FNC.ShowSubMenu(ref sgSelPart, FRM.GetGridBackColor(),  "Interference",
                                                                        "One Cycle"   ,    
                                                                        "Option"      ,
                                                                        "Debug"       );
                SelPage(m_iSelPage);   
				sgSelPart.ClearSelection();
				sgSelPart.Rows[m_iSelPage].Cells[0].Selected = true;
 
                tmProc.Enabled = true;  
             }
             if(!this.Visible && actived) 
             {//Hide
                if(!actived) return;
                FRM.HideFormParent(FrmCtlBtn );
                tmProc    .Enabled = false;
                tmOneShot .Enabled = false;
                cDEF.MOTR .m_bSkipChkCrash = false;   
             }
             actived = this.Visible;
        }
        //------------------------------------------------------------------------
        public void FormHide()
        {
            FRM.HideFormParent(FRM.SMotor);
            FRM.HideFormParent(FrmCtlBtn );
        }
        //------------------------------------------------------------------------
        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }

            tmProc.Enabled = false;
            switch (tpgMenu4.SelectedIndex)
            {
                default: tpgPage1Update(); break;
                case  0: tpgPage1Update(); break;
                case  1: tpgPage2Update(); break;
              //case  2: tpgPage3Update(); break;
                case  3: tpgPage3Update(); break;

            }       
            tmProc.Enabled = true;
        }
        //------------------------------------------------------------------------
        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            try
            { 
                if (cDEF.SEQ._bRun) {
                    MsgBox.Warning("The parameter can not be changed while the Machine is running.");
                    return;
                    }      
                if(!FRM.ShowMsg(true, "Confirm", "Do you want to save the parameter?", EN_MSG_KIND.UserModal)) return;
                switch (tpgMenu4.SelectedIndex)
                {
                    default: tpgPage1Save(); break;
                    case  0: tpgPage1Save(); break;
                    case  1: tpgPage2Save(); break;
                    case  2: tpgPage3Save(); break;
                }
            }
            catch (Exception err)
            {
                MsgBox.Error(err.Message);
                return;
            }
        }
        //------------------------------------------------------------------------
        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            SelPage(iGridR);
        }
        //------------------------------------------------------------------------
        void SelPage(int iPage)
        {//화면의 메뉴 선택시 Tab PAGE 결정 
            btnSave.Visible = false;
            //
            if (tpgMenu4.SelectedIndex == 0) FRM.HideFormParent(FRM.SMotor);  

            tpgMenu4.SelectedIndex = iPage; 

            switch (iPage)
            {
                default: tpgPage1Show(); break;
                case  0: tpgPage1Show(); break;
                case  1: tpgPage2Show(); break;
                case  2: tpgPage3Show(); break;
                case  3: tpgPage4Show(); break;
            }
            m_iSelPage = iPage;
        }
        //------------------------------------------------------------------------
        #region "PAGE1"
        public void tpgPage1Show()
        {//Tab Page #1 Show (화면 업데이트)
            btnSave.Visible = true;
            FRM.SMotor.m_iSelMotrPart = (int)EN_PART_SEL.All;
            FRM.ShowFormParent(FRM.SMotor,this.pnBaseMotr);
            cDEF.MOTR.UpdateDstbByGrid(true, ref sgDSTB);
        }

        public void tpgPage1Save()
        {//Tab Page #1 Hide (Save)
            cDEF.MOTR.UpdateDstbByGrid(false, ref sgDSTB);
            
        }

        public void tpgPage1Update()
        {//Timer에서 Page1의 업데이트할 내용을 추가  


        }
        #endregion "PAGE1"
        //------------------------------------------------------------------------
        #region "PAGE2"
        public void tpgPage2Show()
        {//Tab Page #2 Show (화면 업데이트)
            tmOneShot.Enabled = false;
	        for(int i=0; i<cDEF.POSN.GetPartCnt(); i++)
	        {//ALL = -1, piSys = PartCnt + 1;
                if(i==0 ) {gbOneShot1 .Visible = true; gbOneShot1 .Text =  cDEF.POSN.GetPartName(i); }
                if(i==1 ) {gbOneShot2 .Visible = true; gbOneShot2 .Text =  cDEF.POSN.GetPartName(i); }
                if(i==2 ) {gbOneShot3 .Visible = true; gbOneShot3 .Text =  cDEF.POSN.GetPartName(i); }
                if(i==3 ) {gbOneShot4 .Visible = true; gbOneShot4 .Text =  cDEF.POSN.GetPartName(i); }
                //if(i==6 ) {gbOneShot7 .Visible = true; gbOneShot7 .Text =  cDEF.POSN.GetPartName(i); }
                //if(i==7 ) {gbOneShot8 .Visible = true; gbOneShot8 .Text =  cDEF.POSN.GetPartName(i); }
                //if(i==8 ) {gbOneShot9 .Visible = true; gbOneShot9 .Text =  cDEF.POSN.GetPartName(i); }
                //if(i==9 ) {gbOneShot10.Visible = true; gbOneShot10.Text =  cDEF.POSN.GetPartName(i); }
                //if(i==10) {gbOneShot11.Visible = true; gbOneShot11.Text =  cDEF.POSN.GetPartName(i); }
	        }
        }

        public void tpgPage2Save()
        {//Tab Page #2 Hide (Save)


        }

        public void tpgPage2Update()
        {//Timer에서 Page2의 업데이트할 내용을 추가  
	        for(int i=0; i<cDEF.POSN.GetPartCnt(); i++)
	        {//ALL = -1, piSys = PartCnt + 1;
                if(i==0  && sgOnShot1 .Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot1 );
                if(i==1  && sgOnShot2 .Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot2 );
                if(i==2  && sgOnShot3 .Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot3 );
                if(i==3  && sgOnShot4 .Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot4 );
                //if(i==6  && sgOnShot7 .Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot7 );
                //if(i==7  && sgOnShot8 .Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot8 );
                //if(i==8  && sgOnShot9 .Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot9 );
                //if(i==9  && sgOnShot10.Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot10); 
                //if(i==10 && sgOnShot11.Visible) cDEF.SEQ.UpdateGrid(i, ref sgOnShot11); 
	        }
            //
            cDEF.SEQ.UpdateGrid(cDEF.POSN.GetPartCnt(), ref sgOnShot12);

            //cDEF.SEQ.UpdateScanTimeGrid(0, ref sgScanUP);
            //cDEF.SEQ.UpdateScanTimeGrid(1, ref sgScanAR);

            lbTime1.Text = string.Format($"RUN TIME   : {cDEF.SPC.GetDayRunTime ()}");
            lbTime2.Text = string.Format($"DOWN TIME  : {cDEF.SPC.GetDayDownTime()}");
            lbTime3.Text = string.Format($"ERROR TIME : {cDEF.SPC.GetDayErrTime ()}");

        }
        //------------------------------------------------------------------------
        private void tmOneShot_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) { this.tmOneShot.Enabled = false; return; }

			this.tmOneShot.Enabled = false;

	        //Run OneShot AutoRun.
	        if(m_iOneShotTag == 30)
	        {
		        for(int i=0;i<cDEF.POSN.GetPartCnt();i++)  cDEF.SEQ.AutoRunPart(i);
	        }
	        else
	        {
		        cDEF.SEQ.AutoRunPart(m_iOneShotTag);
	        }

            if(!cDEF.EPU._bHasErr) this.tmOneShot.Enabled = true;

        }
        //------------------------------------------------------------------------
        private void btOneShot1_MouseDown(object sender, MouseEventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag); 
            if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
            } 

	        //Set Var.
	        m_iOneShotTag    = iTag;

	        //Enable Timer.
	        tmOneShot.Enabled = true;
        }
        //------------------------------------------------------------------------
        private void btOneShot1_MouseUp(object sender, MouseEventArgs e)
        {
            
			if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
            } 

	        //Set Var.
	        //m_iOneShotTag    = -1   ;

	        //Enable Timer.
	        //tmOneShot.Enabled = false;
        }
        //------------------------------------------------------------------------
        private void btOneShotRst1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag); 
            
			if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
            } 
		
			cDEF.SEQ.ResetPart(iTag);
	        
            //Set Var.
	        m_iOneShotTag    = -1   ;

	        //Enable Timer.
	        tmOneShot.Enabled = false;
        }
        //------------------------------------------------------------------------
        private void pbWaf4_MouseDown(object sender, MouseEventArgs e)
        {
            //bool isRising = false;
            ////
            //if (cDEF.FM.m_iCrntLevel == (int)EN_LOGIN.Operator) return;
            //
            //PictureBox pBox = (sender as PictureBox);
            //int iTag  = Convert.ToInt32(pBox.Tag);
            //m_iSelPart = iTag;
            ////Magazine
            //if ((iTag == 0) || (iTag == 1))
            //{
            //    m_iSelWhre = 0;
            //    m_iSelPart = iTag;
            //    cDEF.DM.MGZ[iTag].GetImageRC(ref pBox, e.X, e.Y, out m_iSelRow);
            //}
            //else if ((iTag == 10) || (iTag == 11) || (iTag == 12) || (iTag == 13))
            //{
            //    m_iSelWhre = 1;
            //    m_iSelPart = iTag - 10;
            //}
            //else return;
            ////
            //if          (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left ) { isRising = SetPopMenuUpdate(0); } //One
            //else if     (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Right) { isRising = SetPopMenuUpdate(1); } //All
            //else return;
            //
			//if (isRising) MenuStrip.Show((sender as PictureBox), new Point(e.X, e.Y));
			//else		  MenuStrip.Hide();
        }
        //------------------------------------------------------------------------
        private bool SetPopMenuUpdate (int iKind)
        {//Kind - 0 : One, 1 : All
            ToolStripMenuItem ts = new ToolStripMenuItem();
            //
            MenuStrip.ItemClicked -= new ToolStripItemClickedEventHandler(menuItemOne_Click);    
            MenuStrip.ItemClicked -= new ToolStripItemClickedEventHandler(menuItemAll_Click);    
            //
            MenuStrip.Items.Clear(); 
            MenuStrip.Items.Add(ts);
            //
            ts.Text = "Wafer Mask";            
            ts.Font = new Font("Tahoma", 12.0f);
            ts.BackColor = Color.FromArgb(218,241,252);
            //
            for (int i = 0; i < vDEF.STR_WAF_STAT .Length; i++) MenuStrip.Items.Add(string.Format("{0} - {1}", (iKind == 0) ? "One" : "All", vDEF.STR_WAF_STAT [i])); 
            if (iKind == 0) MenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(menuItemOne_Click);
            else            MenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(menuItemAll_Click);
			//
			return true;
        }
        //------------------------------------------------------------------------
        private void menuItemOne_Click (object sender, ToolStripItemClickedEventArgs e)
        {
            //int          iTag  = -1;
            //string       sStr  = "";
            //EN_WAFER_STAT  iStat = EN_WAFER_STAT.None;
            ////
            //if (cDEF.SEQ._bRun)
            //{
            //    MsgBox.Warning("Can not be Change while the Machine is running.");
            //    return;
            //} 
            ////
            //for (int n = 0; n < vDEF.STR_WAF_STAT.Length; n++)
            //{
            //    sStr = e.ClickedItem.Text.Substring(6, e.ClickedItem.Text.Length - 6);
            //    if (vDEF.STR_WAF_STAT[n].ToLower() == sStr.ToLower()) {  iTag = n; break; }
            //}
            ////
            //switch(iTag) 
            //{
            //    default : return;
            //    case  0  : iStat = EN_WAFER_STAT.Empty   ; break;
            //    case  1  : iStat = EN_WAFER_STAT.Mask    ; break;             
            //    case  2  : iStat = EN_WAFER_STAT.Mount   ; break;
            //    case  3  : iStat = EN_WAFER_STAT.Aligned ; break;
            //    case  4  : iStat = EN_WAFER_STAT.Skip    ; break;
            //    case  5  : iStat = EN_WAFER_STAT.Fnsh    ; break;
            //    case  6  : iStat = EN_WAFER_STAT.Work    ; break;
            //    case  7  : iStat = EN_WAFER_STAT.Wait    ; break;
            //    case  8  : iStat = EN_WAFER_STAT.Fail    ; break;
            //}                                    
            ////
            //if (m_iSelWhre == 0) DM.MGZ[m_iSelPart].SetTo(m_iSelRow, iStat);
            //else                 DM.WAF[m_iSelPart].SetTo(           iStat);
            //sStr = Enum.GetName(typeof(EN_WAF_ID  ), m_iSelPart) + "Wafer Unit";
            ////
            //cDEF.LOG.Trace($"ItemOne {sStr} Click");
        }
        //------------------------------------------------------------------------
        private void menuItemAll_Click (object sender, ToolStripItemClickedEventArgs e)
        {
            //int          iTag  = -1;
            //string       sStr  = "";
            //EN_WAFER_STAT  iStat = EN_WAFER_STAT.None;
            ////
            //if (m_iSelWhre != 0)  return;
            ////
            //if (cDEF.SEQ._bRun)
            //{
            //    MsgBox.Warning("Can not be Change while the Machine is running.");
            //    return;
            //} 
            ////
            //for (int n = 0; n < vDEF.STR_WAF_STAT.Length; n++)
            //{
            //    sStr = e.ClickedItem.Text.Substring(6, e.ClickedItem.Text.Length - 6);
            //    if (vDEF.STR_WAF_STAT[n].ToLower() == sStr.ToLower()) {  iTag = n; break; }
            //}
            ////
            //switch(iTag) 
            //{
            //    default : return;
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
            ////
            //cDEF.DM.MGZ[m_iSelPart].SetTo(iStat);
            //sStr = Enum.GetName(typeof(EN_WAF_ID  ), m_iSelPart) + "Cassette Unit";
            ////
            //cDEF.LOG.Trace($"ItemOne {sStr} Click");
        }
        #endregion "PAGE2"

        //------------------------------------------------------------------------
        #region "PAGE3"
        public void tpgPage3Show()
        {//Tab Page #3 Show (화면 업데이트)
            //
            Label    label   ;
            ComboBox comboBox;   
            ToggleSwitch toggle;
        //
            btnSave.Visible = true;
            
            int iPartCnt = cDEF.POSN.GetPartCnt();
            for (int n = 0; n < iPartCnt; n++)
            {
                toggle = gbAROpt.Controls[string.Format("tsAROpt{0}", n+1)] as ToggleSwitch;
                label  = gbAROpt.Controls[string.Format("lbAROpt{0}", n+1)] as Label;
                if (toggle != null)
                { 
                    toggle.Visible = true; toggle.Checked = cDEF.FM.SysOptn .bOffAR[n];
                }
                if (label != null)
                { 
                    label .Visible = true;
                    label .Text =  cDEF.POSN.GetPartName(n);
                }
            }

            if(cDEF.FM.SysOptn .iRunMode == 0) rbRunOpt1.Checked = true;
            if(cDEF.FM.SysOptn .iRunMode == 1) rbRunOpt2.Checked = true;
            if(cDEF.FM.SysOptn .iRunMode == 2) rbRunOpt3.Checked = true;

            if(cDEF.FM.SysOptn .iLangOpt == 0) rbLangOpt1.Checked = true;
            if(cDEF.FM.SysOptn .iLangOpt == 1) rbLangOpt2.Checked = true;
            if(cDEF.FM.SysOptn .iLangOpt == 2) rbLangOpt3.Checked = true;


		    tsMAOption1        .Checked   =  FM.SysOptn.iChkTopDoor == 1 ? true : false;
		    tsMAOption2        .Checked   =  FM.SysOptn.iChkDrLock  == 1 ? true : false;
		    tsMAOption3        .Checked   =  FM.SysOptn.iChkSafety  == 1 ? true : false;
            tsMAOption4        .Checked   =  FM.SysOptn.iChkFan     == 1 ? true : false;
            tsMAOption5        .Checked   =  FM.SysOptn.iSkipSeqLog == 1 ? true : false;

            tsMatSkip1         .Checked   =  FM.SysOptn.iRunSkipMat == 1 ? true : false;

            groupBox4          .Visible   = cDEF.FM.IsMasterLv();

            tsSpecOpt1         .Checked   =  MOTR.m_bSkipChkCrash;
            tsSpecOpt2         .Checked   =  FM.SysOptn.bSimulRun;
            tsSpecOpt3         .Checked   =  FM.SysOptn.bViewROI ; //260804 //AOCV
            tsSpecOpt4         .Checked   =  FM.SysOptn.bFanSkipAlarm; //2026 08 25 2LC8

            tsVacSkip1.Checked = FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WAT ] ;
            //tsVacSkip2.Checked = FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WTR ] ;
            //tsVacSkip3.Checked = FM.SysOptn.bSkipVac[(int)EN_WAF_ID.ASM1] ;
            //tsVacSkip4.Checked = FM.SysOptn.bSkipVac[(int)EN_WAF_ID.ASM2] ;

            for (int n = 0; n < (int)EN_WAF_ID.EndOfId; n++)
            {
                toggle = gbVacSkip.Controls[string.Format("tsVacSkip{0}", n+1)] as ToggleSwitch;
                label  = gbVacSkip.Controls[string.Format("lbVacSkip{0}", n+1)] as Label;
                if (toggle != null)
                { 
                    toggle.Visible = true; 
                    toggle.Checked = cDEF.FM.SysOptn.bSkipVac[n] ? true : false;
                }
                if (label != null)
                {
                    label.Visible = true; label.Text = vDEF.STR_WAF_ID[n];
                }
            }

            for (int n = 0; n < (int)EN_CAM.EndofCam; n++)
            {
                comboBox = gbTestOptn.Controls[string.Format("cbTest{0}", n + 1)] as ComboBox;
                label = gbTestOptn.Controls[string.Format("lbTest{0}", n + 1)] as Label;
                if (comboBox != null)
                {
                    comboBox.Visible = true;
                    comboBox.SelectedIndex = cDEF.FM.SysOptn.iTestMode[n];
                }
                if (label != null)
                {
                    label.Visible = true; label.Text = "CAMERA_" + Enum.GetName(typeof(EN_CAM), n);
                }
            }
        }
        //------------------------------------------------------------------------
        public void tpgPage3Save()
        {//Tab Page #3 Hide (Save)
            //CheckBox checkBox;
            ComboBox comboBox;    
            ToggleSwitch toggle;
            //
            int iPartCnt = cDEF.POSN.GetPartCnt();
            for (int n = 0; n < iPartCnt; n++)
            {
                toggle = gbAROpt.Controls[string.Format("tsAROpt{0}", n+1)] as ToggleSwitch;
                if (toggle == null) continue;
                cDEF.FM.SysOptn .bOffAR[n] = toggle.Checked;
            }

            if(rbRunOpt1.Checked ) cDEF.FM.SysOptn .iRunMode = 0;
            if(rbRunOpt2.Checked ) cDEF.FM.SysOptn .iRunMode = 1;
            if(rbRunOpt3.Checked ) cDEF.FM.SysOptn .iRunMode = 2;
                                                                
            if(rbLangOpt1.Checked) cDEF.FM.SysOptn .iLangOpt = 0;
            if(rbLangOpt2.Checked) cDEF.FM.SysOptn .iLangOpt = 1;
            if(rbLangOpt3.Checked) cDEF.FM.SysOptn .iLangOpt = 2;

            cDEF.FM.SysOptn.iChkTopDoor  = (tsMAOption1   .Checked)? 1 : 0;
            cDEF.FM.SysOptn.iChkDrLock   = (tsMAOption2   .Checked)? 1 : 0;
            cDEF.FM.SysOptn.iChkSafety   = (tsMAOption3   .Checked)? 1 : 0;
            cDEF.FM.SysOptn.iChkFan      = (tsMAOption4   .Checked)? 1 : 0;
            cDEF.FM.SysOptn.iSkipSeqLog  = (tsMAOption5   .Checked)? 1 : 0;

            cDEF.FM.SysOptn.iRunSkipMat  = (tsMatSkip1    .Checked)? 1 : 0;



            cDEF.MOTR .m_bSkipChkCrash   =  tsSpecOpt1    .Checked;     
            cDEF.FM.SysOptn.bSimulRun    =  tsSpecOpt2    .Checked;
            cDEF.FM.SysOptn.bViewROI     =  tsSpecOpt3    .Checked; //260804 //AOCV
            cDEF.FM.SysOptn.bFanSkipAlarm =  tsSpecOpt4    .Checked; //2026 08 25 2LC8

            //cDEF.FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WTR_A] = tsVacSkip1.Checked? true: false;
            //cDEF.FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WTR_B] = tsVacSkip2.Checked? true: false;
            //cDEF.FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WAT  ] = tsVacSkip3.Checked? true: false;
            //cDEF.FM.SysOptn.bSkipVac[(int)EN_WAF_ID.WTB  ] = tsVacSkip4.Checked? true: false;

            for (int n = 0; n < (int)EN_WAF_ID.EndOfId; n++)
            {
                toggle = gbVacSkip.Controls[string.Format("tsVacSkip{0}", n+1)] as ToggleSwitch;
                if (toggle == null) continue;
                cDEF.FM.SysOptn.bSkipVac[n] = toggle.Checked;
            }
            for (int n = 0; n < (int)EN_CAM.EndofCam; n++)
            {
                comboBox = gbTestOptn.Controls[string.Format("cbTest{0}", n + 1)] as ComboBox;
                if (comboBox == null) continue;
                cDEF.FM.SysOptn.iTestMode[n] = comboBox.SelectedIndex;
            }
            //2026 08 25  2LC8
            cDEF.FM.SysOptn.Load(false);
            
           
        }
        //------------------------------------------------------------------------
        public void tpgPage3Update()
        {//Timer에서 Page4의 업데이트할 내용을 추가  
            

        }
        //------------------------------------------------------------------------
        public void tpgPage4Show()
        {//Tab Page #4 Show (화면 업데이트)
            //
            //Label        label   ;
            //ToggleSwitch toggle;
            
            //
            btnSave.Visible = true;
            
 
        }
        //------------------------------------------------------------------------
        private void btnDefList_Click(object sender, EventArgs e)
        {
            cDEF.FM .DefineMotrList(     );
            cDEF.FM .DefineActrList(     );
            cDEF.EPU.LoadErrDataIni(false);
        }
        //------------------------------------------------------------------------
        private void button35_Click(object sender, EventArgs e)
        {
            ToggleSwitch toggle;

            tsMAOption1.Checked = false;
            tsMAOption2.Checked = false;
            tsMAOption3.Checked = false;
            tsMAOption4.Checked = false;
            
            rbRunOpt1  .Checked = false;
            rbRunOpt2  .Checked = true ;
            rbRunOpt3  .Checked = false;

            tsMatSkip1 .Checked = true ;

            cbTest1 .SelectedIndex = 1;
            cbTest2 .SelectedIndex = 1;
            cbTest3 .SelectedIndex = 1;
            cbTest4 .SelectedIndex = 1;
            cbTest5 .SelectedIndex = 1;
            cbTest6 .SelectedIndex = 1;
            cbTest7 .SelectedIndex = 1;
            cbTest8 .SelectedIndex = 1;
            cbTest9 .SelectedIndex = 1;
            cbTest10.SelectedIndex = 1;

            for (int n = 0; n < (int)EN_WAF_ID.EndOfId; n++)
            {
                toggle = gbVacSkip.Controls[string.Format("tsVacSkip{0}", n+1)] as ToggleSwitch;
                if (toggle == null) continue;
                toggle.Checked = true;
            }

			//tsAROpt1 .Checked   = false;
			//tsAROpt2 .Checked   = false;
			//tsAROpt3 .Checked   = false;
			//tsAROpt4 .Checked   = false;
			//tsAROpt5 .Checked   = false;
			//tsAROpt6 .Checked   = false;
			//tsAROpt7 .Checked   = true;
			//tsAROpt8 .Checked   = false;
			//tsAROpt9 .Checked   = false;
			//tsAROpt10.Checked   = false;
        }



        #endregion "PAGE3"
        //------------------------------------------------------------------------
        private void btAutoRun1_Click(object sender, EventArgs e)
        {

            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            m_iOneShotTag = iTag;

            //One Shot - Auto Run 
            cDEF.SEQ.AutoRunPart(m_iOneShotTag);

        }
        //------------------------------------------------------------------------
        private void btDataInit_Click(object sender, EventArgs e)
        {
            //
            //DM.ClearMap();
            //
            //LOG.Trace("Master - Data Init");
        }
        //------------------------------------------------------------------------
        private void button6_Click(object sender, EventArgs e)
        {
            cDEF.SetFileSave(EN_SAVE_TYPE.OptEng);
        }
        //------------------------------------------------------------------------
        private void bt4_1_Click(object sender, EventArgs e)
        {
            //isConPlceWTB_A
            //isConPlceWAT_A
            //isConPlceLMZ_B
            //isConPlceUMZ_B
            //
            //isConPickLMZ_A
            //isConPickUMZ_A
            //isConPickWAT_A
            //isConPickWAT_B
            //isConPickWTB_B

            Button sb = sender as Button;
            int nTag = (int)sb.Tag;




        }
        //------------------------------------------------------------------------
        private void btAdd_Click(object sender, EventArgs e)
        {
            
        }
        //------------------------------------------------------------------------
        private void btAdmin01_Click(object sender, EventArgs e)
        {

        }
        //--------------------------------------------------------------------------
        private void btAdmin02_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tsMAOption1.Checked = false;
            tsMAOption2.Checked = false;
            tsMAOption3.Checked = false;
            tsMAOption4.Checked = false;

            rbRunOpt1.Checked = false;
            rbRunOpt2.Checked = true;
            rbRunOpt3.Checked = false;

            tsMatSkip1.Checked = true;

            cbTest1.SelectedIndex = 1;
            cbTest2.SelectedIndex = 1;
            cbTest3.SelectedIndex = 1;
            cbTest4.SelectedIndex = 1;
            cbTest5.SelectedIndex = 1;
            cbTest6.SelectedIndex = 1;
            cbTest7.SelectedIndex = 1;
            cbTest8.SelectedIndex = 1;
            cbTest9.SelectedIndex = 1;
            cbTest10.SelectedIndex = 1;

            tsSpecOpt2.Checked = true;
            tsVacSkip1.Checked = true;
            tsVacSkip2.Checked = true;
            tsVacSkip3.Checked = true;
            tsVacSkip4.Checked = true;
        }
        //--------------------------------------------------------------------------
        private void btSetErr_Click(object sender, EventArgs e)
        {
            int.TryParse(tbErrNo.Text, out int nNo);

            if (nNo < 0) return;

            EPU.SetErr(nNo);
           
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        //------------------------------------------------------------------------
        private void btDayClear_Click(object sender, EventArgs e)
        {
            //
            SPC.SPC_EFF   .ResetData();
            SPC.DAILY_DATA.ResetData();

            //
            SEQ.WAT.ClearAlignCount();

            LOG.Trace(">> MASTER << Day Clear");
        }
        //------------------------------------------------------------------------
        private void btDayLog_Click(object sender, EventArgs e)
        {
            string stemp = string.Format($"[Day Change] MTBI({SPC.CalMTBI(SPC.DAILY_DATA.dRunTime, SPC.DAILY_DATA.iJamQty)}), WORK QTY({SPC.DAILY_DATA.iWorkQty}), ERROR QTY({SPC.DAILY_DATA.iJamQty})");
            LOG.Trace(stemp);
            stemp = string.Format($"                > Run Time : {SPC.GetDayRunTime()}, Down Time : {SPC.GetDayDownTime()}, Error Time : {SPC.GetDayErrTime()}");
            LOG.Trace(stemp);

        }
        //------------------------------------------------------------------------
        private void btCal_Click(object sender, EventArgs e)
        {
            //
            int.TryParse(tbPluse.Text.Trim(), out int nPluse);
            int nMotr = cbMotor.SelectedIndex;

            if (nMotr < 0) return; 

            tbPos.Text = (Math.Round(MOTR[nMotr].CalPulseToPos((double)nPluse),5)).ToString() ;
        }
    }
}
