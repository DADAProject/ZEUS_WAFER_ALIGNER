using eMachine.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static eMachine.cDEF;

namespace eMachine
{
    public partial class FrmMain : Form
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        int    m_iPageSel;
        bool   m_bFlick  ; 
        bool   actived   ;    

		//bool   m_bShowDispWarn;
		string m_bDispWarnMsg ;
        //bool   m_UserFlag     ;

        private Point point = new Point();

        //protected: /* Inheritable Vars.        */
        private cScreenEffects[] m_Effects = new cScreenEffects[vDEF.MAX_SCREEN_EFF];

        //public:    /* Direct Accessable Vars.  */

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public FrmMain()
        {
            InitializeComponent();
            SetBounds(0, 0, 1280, 1024);  

			//User Display용
			//m_bShowDispWarn = false;
			m_bDispWarnMsg    = "User Display";
			tmEffect.Interval = 2000;

			for (int i = 0; i < vDEF.MAX_SCREEN_EFF; i++)
			{
				m_Effects[i] = new cScreenEffects(components, 500);

				if (m_Effects[i] != null)
				{
                    m_Effects[i].SetTextEffect(m_bDispWarnMsg, new[] { Color.Red, Color.DarkRed },
                    new Font("Tahoma", 100F, FontStyle.Bold),
                    -5);
				}
			}

            //
            pnBase.BackColor = FRM.GetBaseColor();


            //
            AdjResMonitor();
        }
        //------------------------------------------------------------------------
        private void AdjResMonitor()
        {
            FontAwesome.Sharp.IconButton b;
            int ctrlWidth = this.Width / pnMenu.Controls.Count;
            
			for(int i=1; i<= pnMenu.Controls.Count; i++) 
            {
			   b = pnMenu.Controls[string.Format("btnMenu{0}",i)] as FontAwesome.Sharp.IconButton;
               if (b == null) continue;
			   b.Width = ctrlWidth;
               b.Font  = new Font(b.Font, FontStyle.Regular);
			}                            
        }
        //------------------------------------------------------------------------
        private void FrmMain_Load(object sender, EventArgs e)
        {
            pnBase.Dock = System.Windows.Forms.DockStyle.Fill;

            btnMenu1.Select();

            FontAwesome.Sharp.IconButton b;
            for(int i=1; i<= pnMenu.Controls.Count; i++)
            {
               b = pnMenu.Controls[string.Format("btnMenu{0}",i)] as FontAwesome.Sharp.IconButton;
               if (b == null) continue;
			   b.Visible = true;
			}

            FrmProgress Progress = new FrmProgress();
            Progress.Tag = 0;
            if (Progress.ShowDialog() != DialogResult.OK)
            {
                this.Tag = 2; 
            }

            lbTitle.Text = vDEF.sOsTitle + " " + cDEF.FM._sVersion;
          //lbMaker.Text = vDEF.sMaker  ;

 
            // 폼 초기화
            FRM.Init(this.pnBase, this);

            //
            tmProc.Interval = 100;
            tmStat.Interval = 100;

            PageShow(1);

            //
            LOG.DisplayComEvent += DisplayComEvent;

        }
        //------------------------------------------------------------------------
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            WinAPI.TimeEndPeriod(1);

            Visible              = false;
            tmProc.Enabled       = false;
            tmEffect.Enabled     = false;

            FrmProgress Progress = new FrmProgress();
		    Visible      = false;
            Progress.Tag = 1;
            Progress.ShowDialog();
            
            KillProgram();
        }
        //------------------------------------------------------------------------
        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible && !actived) 
            {//Show
                tmProc.Enabled = true;
                tmStat.Enabled = true;
                tmEffect.Enabled = true;
                cDEF.SEQ.m_bPgmLoadOk = true;
            }
            if(!this.Visible && actived) 
            {//Hide
                tmProc.Enabled = false;
                tmStat.Enabled = false;
                tmEffect.Enabled = false;
            }
            actived = this.Visible;
        }
      
        //Event Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void btnMenu1_MouseUp(object sender, MouseEventArgs e)
        {
            FontAwesome.Sharp.IconButton Btn = (sender as FontAwesome.Sharp.IconButton);
            FontAwesome.Sharp.IconButton b;

            for (int i = 1; i <= pnMenu.Controls.Count; i++)
            {
                b = pnMenu.Controls[string.Format("btnMenu{0}", i)] as FontAwesome.Sharp.IconButton;

                if (b == null) continue;
                if (Btn.Equals(b))
                {
                    // b.Font.Dispose();
                    b.Font = new Font(b.Font, FontStyle.Bold);
                    //b.ForeColor = Color.Black;
                    b.BackColor = Color.FromArgb(57, 62, 71);
                    //b.FlatAppearance.MouseOverBackColor = Color.White;
                    //b.FlatAppearance.BorderSize = 0;
                }
                else
                {
                    b.Font = new Font(b.Font, FontStyle.Regular);
                    //b.ForeColor = Color.DimGray;
                    b.BackColor = Color.FromArgb(36, 45, 60);
                    //b.FlatAppearance.MouseOverBackColor = Color.Silver;
                    //b.FlatAppearance.BorderSize = 1;
                }
            }

            int iTag = Convert.ToInt32(Btn.Tag);
            PageShow(iTag);
        }
        //--------------------------------------------------------------------------
        private void btnMenu9_MouseMove(object sender, MouseEventArgs e)
        {
            FontAwesome.Sharp.IconButton Btn = (sender as FontAwesome.Sharp.IconButton);
            FontAwesome.Sharp.IconButton b;

            for (int i = 1; i <= pnMenu.Controls.Count; i++)
            {
                b = pnMenu.Controls[string.Format("btnMenu{0}", i)] as FontAwesome.Sharp.IconButton;

                if (b == null) continue;
                if (Btn.Equals(b))
                {
                    b.Font = new Font(b.Font, FontStyle.Bold);
                    b.IconColor = Color.FromArgb(219,219,195);
                }
            }
        }
        //--------------------------------------------------------------------------
        private void btnMenu9_MouseLeave(object sender, EventArgs e)
        {
            FontAwesome.Sharp.IconButton Btn = (sender as FontAwesome.Sharp.IconButton);
            FontAwesome.Sharp.IconButton b;

            for (int i = 1; i <= pnMenu.Controls.Count; i++)
            {
                b = pnMenu.Controls[string.Format("btnMenu{0}", i)] as FontAwesome.Sharp.IconButton;

                if (b == null) continue;
                if (Btn.Equals(b))
                {
                    b.Font = new Font(b.Font, FontStyle.Regular);
                    b.IconColor = SystemColors.ButtonFace;
                }
            }
        }

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void PageShow(int iPage)
        {
            if (m_iPageSel == iPage) return;

            PageHide();

            tbMainCon.SelectedIndex = 0;

            switch (iPage)
            {
                default: FRM.ShowFormParent(FRM.MOper   ,this.pnBase); break;
                case  1: FRM.ShowFormParent(FRM.MOper   ,this.pnBase); break;
                case  2: FRM.ShowFormParent(FRM.MProj   ,this.pnBase); break;
                case  3: FRM.ShowFormParent(FRM.MMotion ,this.pnBase); break;
                case  4: FRM.ShowFormParent(FRM.MMotor  ,this.pnBase); break;
                case  5: FRM.ShowFormParent(FRM.MIO     ,this.pnBase); break;
                case  6: FRM.ShowFormParent(FRM.MDb     ,this.pnBase); break;
                case  7: FRM.ShowFormParent(FRM.MSetting,this.pnBase); break;
                case  8: FRM.ShowFormParent(FRM.MAdmin  ,this.pnBase); break;
                
                //
                case  9: tbMainCon.SelectedIndex = 1; break;
                //
                case 10: FRM.Login.ShowDialog(); break;  
            }
            m_iPageSel = iPage;

        }
        //--------------------------------------------------------------------------
        public void PageHide()
        {
            switch (m_iPageSel)
            {
                default: FRM.MOper   .FormHide(); FRM.HideFormParent(FRM.MOper   ); break;
                case  1: FRM.MOper   .FormHide(); FRM.HideFormParent(FRM.MOper   ); break;
                case  2: FRM.MProj   .FormHide(); FRM.HideFormParent(FRM.MProj   ); break;
                case  3: FRM.MMotion .FormHide(); FRM.HideFormParent(FRM.MMotion ); break;
                case  4: FRM.MMotor  .FormHide(); FRM.HideFormParent(FRM.MMotor  ); break;
                case  5: FRM.MIO     .FormHide(); FRM.HideFormParent(FRM.MIO     ); break;
                case  6: FRM.MDb     .FormHide(); FRM.HideFormParent(FRM.MDb     ); break;
                case  7: FRM.MSetting.FormHide(); FRM.HideFormParent(FRM.MSetting); break;
                case  8: FRM.MAdmin  .FormHide(); FRM.HideFormParent(FRM.MAdmin  ); break;

                case 10: break;
            }

        }
        //--------------------------------------------------------------------------
        static public void KillProgram()
        {
            //
            AssemblyName a = Assembly.GetExecutingAssembly().GetName();
            string strProg = a.Name;

            Process[] pLIST;
            //
            //pLIST = Process.GetProcessesByName(strProg + ".vshost");
            //foreach (Process Proc in pLIST) 
            //    Proc.Kill();
            //
            pLIST = Process.GetProcessesByName(strProg);
            foreach (Process proc in pLIST) 
                proc.Kill(); 
            //
            pLIST = Process.GetProcessesByName($"{strProg}64");
            foreach (Process proc in pLIST) 
                proc.Kill(); 
            //
            pLIST = Process.GetProcessesByName($"{strProg}32");
            foreach (Process proc in pLIST) 
                proc.Kill(); 

        }
        //--------------------------------------------------------------------------
        public void ChangeLevel() 
        {
            int iLevel = (int)cDEF.FM.m_iCrntLevel;
            FontAwesome.Sharp.IconButton b;

            if (iLevel >= (int)(EN_LOGIN.EndOfId-1)) {
				for(int i=1; i<= pnMenu.Controls.Count; i++) {
				   b = pnMenu.Controls[string.Format("btnMenu{0}",i)] as FontAwesome.Sharp.IconButton;
                   if (b == null) continue;
				   b.Visible = true;
                   b.Enabled = true;
				   }
                btClose.Enabled = true;
                }
            else {
                btnMenu1 .Enabled = true;
                btnMenu2 .Enabled = cDEF.FM.LoginSet[iLevel].bEnableMenu[0];
                btnMenu3 .Enabled = cDEF.FM.LoginSet[iLevel].bEnableMenu[1];         
		        btnMenu4 .Enabled = cDEF.FM.LoginSet[iLevel].bEnableMenu[2];        
		        btnMenu5 .Enabled = cDEF.FM.LoginSet[iLevel].bEnableMenu[3];          
		        btnMenu6 .Enabled = cDEF.FM.LoginSet[iLevel].bEnableMenu[4];         
		        btnMenu7 .Enabled = cDEF.FM.LoginSet[iLevel].bEnableMenu[5];            
                btnMenu8 .Enabled = (iLevel == (int)EN_LOGIN.Master) ? true : false;
                btClose  .Enabled = cDEF.FM.LoginSet[iLevel].bEnableMenu[7]; 

            }
        }
        //------------------------------------------------------------------------
        private void btnMenu8_Click(object sender, EventArgs e)
        {
            if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("The Program can not be Exit while the Machine is running.");
                return;
            }

            if (MsgBox.Confirm("System Shut Down?"))
            {
                Close();
            }
        }
        //------------------------------------------------------------------------
        private void tmProc_Tick(object sender, EventArgs e)
        {
            if (!this.Visible) { this.tmProc.Enabled = false; return; }

            tmProc.Enabled = false;
            m_bFlick = !m_bFlick;
            
            EPU.UpdateErr();
            FRM.UpdateMsg();
            
            UpdateMachStat    ();
            
            lbCurDate.Text = string.Format("{0:yy/MM/dd  HH:mm:ss}", DateTime.Now) ;
            //
            if(FRM.RqChangeLevel) 
            {
                FRM.RqChangeLevel = false;
                ChangeLevel();
            }
            //
            if (cDEF.EPU.m_bReqHoldClear && (!FRM.Login.Visible || (FRM.Login == null)))
            {
                FRM.Login.ShowDialog();
            }

            lbCrntLevel.Text = string.Format($"Login Level : {(EN_LOGIN)FM.m_iCrntLevel}");


            //for (int i = 0; i < vDEF.MAX_SCREEN_EFF; i++)
            //{
            //    if (m_Effects[i] != null)
            //    {
            //        if (cDEF.SEQ.WAT._sWorkMsg != "")
            //        {
            //            m_Effects[i].SetText(cDEF.SEQ.WAT._sWorkMsg);
            //            m_Effects[i].ShowEffect(new Point(50, 50), cScreenEffects.eEffectType.TextLocusEffect, true);
            //        }
            //    }
            //}

            //
            this.Enabled = (FRM.Alarm == null) || !FRM.Alarm.Visible;
            //
            tmProc.Enabled = true;
        }
        //------------------------------------------------------------------------
        private void tmEffect_Tick(object sender, EventArgs e)
        {
            if (!this.Visible) { this.tmEffect.Enabled = false; return; }
            //
            //string sMsg = "";
            tmEffect.Enabled = false;
			//User Display
			try
			{
                //Run Mode
                //switch (cDEF.FM.SysOptn.iRunMode)
                //{
                //    case vDEF.MAN_RUN : sMsg = "During\r\nManual Mode\r\nRunning...";   break;
                //    case vDEF.DRY_RUN : sMsg = "During \r\nDry Run Mode\r\nRunning..."; break;
                //}
                //if (cDEF.FM.SysOptn.iRunMode != vDEF.AUTO_RUN)
                //{
                //    m_Effects[1].ShowEffect(new Point(), cScreenEffects.eEffectType.FullTextLocusEffect, sMsg); 
                //    return;
                //}
                
                //Test Mode Check
                //for (int n = 0; n < (int)EN_CAM.EndOfId; n++)
                //{
                //    switch (cDEF.FM.SysOptn.iTestMode[n])
                //    {
                //        case vDEF.MASK_AG   : sMsg = "Test All Good\r\nMasking..."; break;
                //        case vDEF.MASK_AF   : sMsg = "Test All Fail\r\nMasking..."; break;
                //        case vDEF.MASK_RNDM : sMsg = "Test Randon\r\nMasking...";   break;
                //        case vDEF.SKIP_VISN : sMsg = "Test Skip...";                break;
                //    } 
                //    if (cDEF.FM.SysOptn.iTestMode[n] != vDEF.CHCK_AWYS)
                //    {
                //        m_Effects[n + 2].ShowEffect(new Point(), cScreenEffects.eEffectType.FullTextLocusEffect, sMsg);
                //        return;
                //    }
                //}                
			}
			catch (Exception err)
			{
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
                return;
			}
            finally
            {
                tmEffect.Enabled = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
        //------------------------------------------------------------------------
        private void UpdateMachStat()
        {
			string sStat     = "";
			Color cForeColor = Color.Black;
			Color cBackColor = Color.Black;

            lbBtmStat1 .BackColor =  cDEF.EPU.HasError() ? (cDEF.SEQ._bFlick2 ? Color.Red       : Color.Transparent) : Color.Transparent;
            lbBtmStat2 .ForeColor =  cDEF.EPU.HasError() ? (cDEF.SEQ._bFlick2 ? Color.Black     : Color.White      ) : Color.White      ;
            lbBtmStat2 .BackColor = (cDEF.MAN._iManNo>0) ? (cDEF.SEQ._bFlick2 ? Color.Turquoise : Color.Transparent) : Color.Transparent;
            lbBtmStat2 .ForeColor = (cDEF.MAN._iManNo>0) ? (cDEF.SEQ._bFlick2 ? Color.Black     : Color.White      ) : Color.White      ;
	        
			switch (cDEF.SEQ._iSeqStat) 
            {
		        default        : break;
                case EN_SEQ_STAT.Init    : sStat = "Initializing" ; cForeColor = Color.Black                                   ; cBackColor = cDEF.SEQ._bFlick2 ? Color.Lime   : Color.Gray; break;                                                    
                case EN_SEQ_STAT.Error   : sStat = "Error Stop"   ; cForeColor = cDEF.SEQ._bFlick2 ? Color.White : Color.Black ; cBackColor = cDEF.SEQ._bFlick2 ? Color.Red    : Color.Gray; break;
                case EN_SEQ_STAT.RunWarn : sStat = "Run Warning"  ; cForeColor = Color.Black                                   ; cBackColor = cDEF.SEQ._bFlick2 ? Color.Lime   : Color.Gray; break; 
                case EN_SEQ_STAT.Warning : sStat = "Warning"      ; cForeColor = cDEF.SEQ._bFlick2? Color.White  : Color.Black ; cBackColor = cDEF.SEQ._bFlick2 ? Color.Maroon : Color.Gray; break;
                case EN_SEQ_STAT.Running : sStat = "Running"      ; cForeColor = Color.Black                                   ; cBackColor = cDEF.SEQ._bFlick2 ? Color.Lime   : Color.Gray; break;
                case EN_SEQ_STAT.Stop    : sStat = "Stop"         ; cForeColor = Color.Black                                   ; cBackColor = Color.Silver                                 ; break;                                                    
                case EN_SEQ_STAT.DoorOpen: sStat = "Door Opened"  ; cForeColor = Color.Red                                     ; cBackColor = Color.Yellow                                 ; break; 
                case EN_SEQ_STAT.WorkEnd : sStat = "Work Ended"   ; cForeColor = Color.Brown                                   ; cBackColor = Color.Aqua                                   ; break; 
                case EN_SEQ_STAT.Idle    : sStat = "Idle"         ; cForeColor = Color.Black                                   ; cBackColor = Color.Silver                                 ; break; 
            }

			lbBtmStat12.Text      = sStat     ;
			lbBtmStat12.ForeColor = cForeColor;
			lbBtmStat12.BackColor = cBackColor;
            //
            lbBtmStat11.Text = ((EN_LOGIN)cDEF.FM.m_iCrntLevel).ToString().ToUpper(); //Enum.GetName(typeof(EN_LOGIN), cDEF.FM.m_iCrntLevel);
            lbBtmStat11.ForeColor = cDEF.FM.m_iCrntLevel == (int)EN_LOGIN.Master? Color.Red : Color.Aqua;
           

        }
        //------------------------------------------------------------------------
        private void tmMsg_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmStat.Enabled = false; return; }

            tmStat.Enabled = false;
            lbBtmStat1 .Text = cDEF.EPU._iLastErr < 0 ? "" : string.Format($"[E{cDEF.EPU._iLastErr:0000}]" );
            lbBtmStat2 .Text = cDEF.MAN._iManNo   < 1 ? "" : string.Format($"[M{cDEF.MAN._iManNo:0000}]");
            lbBtmStat3 .Text = string.Format("[T0] {0:F1} ms" , cDEF.TH.m_dScanTime[0]);
            lbBtmStat4 .Text = string.Format("[T1] {0:F1} ms" , cDEF.TH.m_dScanTime[1]);
            lbBtmStat5 .Text = string.Format("[T2] {0:F1} ms" , cDEF.TH.m_dScanTime[2]);
            lbBtmStat6 .Text = string.Format("[T3] {0:F1} ms" , cDEF.TH.m_dScanTime[3]);
            lbBtmStat7 .Text = string.Format("[T4] {0:F1} ms" , cDEF.TH.m_dScanTime[4]);
            lbBtmStat8 .Text = string.Format("[T5] {0:F1} ms" , cDEF.TH.m_dScanTime[5]);
            lbBtmStat10.Text = string.Format("[A.C] {0:F1} s" , cDEF.SEQ.WAT.m_dScanTime[1]/1000);
            //lbBtmStat9.BackColor = FM.GetRunMode() != EN_RUN_MODE.AUTO_RUN ? (cDEF.SEQ._bFlick2 ? Color.Maroon : Color.Transparent) : Color.Lime;
            //lbBtmStat9.ForeColor = FM.GetRunMode() != EN_RUN_MODE.AUTO_RUN ?  Color.White : Color.Black;

            //
            lbStateAuto .Image =  SEQ._bAutoMode                     ? Resources._Green15 : Resources._Gray15;
            lbStateStop .Image = !SEQ._bAutoMode                     ? Resources._Green15 : Resources._Gray15;
            lbStateBusy .Image = (SEQ.WAT._bDrngAlgn || MAN._bHoming)? Resources._Green15 : Resources._Gray15;
            lbStateAlarm.Image =  EPU._bHasErr                       ? Resources._Red15   : Resources._Gray15;
            lbStateInit .Image =  SEQ.IsAllHomeEnd()                 ? Resources._Green15 : Resources._Red15 ;


            lbComClient .Image = COMZEUS._GetConCnt >0 ? Resources._Green15 : Resources._Red15 ;
            lbComLight  .Image = VISN.Light[(int)EN_CAM.WTB].IsOpen ? Resources._Green15 : Resources._Red15 ;
            lbComCarmera.Image = VISN.Cam[(int)EN_CAM.WTB].Cameara != null && 
                                 VISN.Cam[(int)EN_CAM.WTB].Cameara.CameraStatus.Connection? Resources._Green15 : Resources._Red15;

            lbOnLine.Text      = COMZEUS._GetConCnt > 0 ? "[ON LINE]" : "[OFF LINE]";
            lbOnLine.BackColor = COMZEUS._GetConCnt > 0 ? Color.Green : (SEQ._bFlick2? Color.Yellow : Color.Transparent);
            lbOnLine.ForeColor = COMZEUS._GetConCnt > 0 ? Color.White : (SEQ._bFlick2? Color.Red    : Color.Transparent);


            //
            lbCrntRecp.Text = cDEF.FM._sCrntDevice;

            //
            btClose.Enabled = !SEQ._bRun; //&& !SEQ._bAutoMode;

            //
            tmStat.Enabled = true;
        }
        //------------------------------------------------------------------------
        private void lbMaker_Click ( object sender, EventArgs e ) 
        {
            FRM.UpdateInfo.ShowDialog();
            tmStat.Enabled = true;
        }
        //------------------------------------------------------------------------
        private void lbBtmStat1_Click(object sender, EventArgs e)
        {
            cDEF.EPU.Clear();
        }
        //------------------------------------------------------------------------
        private void lbBtmStat2_Click(object sender, EventArgs e)
        {
            cDEF.MAN.Reset();
        }
        //------------------------------------------------------------------------
        private void lbTitle_DoubleClick(object sender, EventArgs e)
        {
            //m_UserFlag = !m_UserFlag;
            FRM.UpdateInfo.ShowDialog();
            tmStat.Enabled = true;

        }
        //------------------------------------------------------------------------
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //
            point = new Point(e.X, e.Y);
        }
        //---------------------------------------------------------------------------
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Location = new Point(this.Left - (point.X - e.X), this.Top - (point.Y - e.Y));
            }
        }
        //--------------------------------------------------------------------------
        private void btogMFunc5_Click(object sender, EventArgs e)
        {
            try
            {
                FRM.Login.ShowDialog();
            }
            catch (Exception err) { System.Diagnostics.Debug.WriteLine("Exception:" + err.Message); }
        }
        //---------------------------------------------------------------------------
        private void DisplayComEvent(string sLog)
        {
            try
            {
                lstCom?.Invoke((Action)delegate ()
                {
                    if (lstCom.Items.Count >= 200)
                        lstCom.Items.RemoveAt(lstCom.Items.Count - 1);

                    lstCom.Items.Insert(0, sLog);
                });
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"[Exception] DisplayComEvent = {ex.Message}");
            }
        }

        private void lbComClient_Click(object sender, EventArgs e)
        {

        }

        private void lbComCarmera_Click(object sender, EventArgs e)
        {

        }

        private void lbComLight_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnBase_Paint(object sender, PaintEventArgs e)
        {

        }
        //---------------------------------------------------------------------------
        private void lbStateStop_Click(object sender, EventArgs e)
        {
            //
            if (!cDEF.FM.IsMasterLv()) return; 
            if (!cDEF.SEQ._bAutoMode ) return; 

            //
            if (!FRM.ShowMsg(true, "MODE", "Manual Mode로 변경하시겠습니까?")) return;

            cDEF.SEQ._bAutoMode   = false;
            cDEF.SEQ._bBtnManStop = true; 

            cDEF.LOG.Trace("[MAN] Manual Mode 변경");
        }
        //---------------------------------------------------------------------------
        private void lbStateAuto_Click(object sender, EventArgs e)
        {
            if (!cDEF.FM.IsMasterLv()) return;
            if ( cDEF.SEQ._bAutoMode ) return;

            //
            if (!FRM.ShowMsg(true, "MODE", "Auto Mode로 변경하시겠습니까?")) return;

            cDEF.SEQ._bAutoMode    = true;
            cDEF.SEQ._bBtnManStart = true;

            cDEF.LOG.Trace("[MAN] Auto Mode 변경");
        }
    }
}
