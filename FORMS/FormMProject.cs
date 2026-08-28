using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static eMachine.cDEF;

namespace eMachine
{
    public partial class FrmMProject : Form
    {
        
        FrmCtrlMC FrmCtlBtn = new FrmCtrlMC(0);
        FrmCamDx  FrmCamCtl = new FrmCamDx(EN_DISPLAY.ViewAndTeach);

        Size[] pnSize = new System.Drawing.Size[2];

        //
        int m_iSelPage   ;
        int  m_iSelF      ;
        bool actived      ;

        public FrmCamDx _FrmCamCtl { get { return FrmCamCtl; } }

        public FrmMProject()
        {
            InitializeComponent();
            //
            Panel           pn;
            TabPage         tp;
            RoundPanel      rp;
            Label           lb; 
            
            //
            this  .BackColor = FRM.GetBaseColor();
            //
            Control[] ctls = FNC.GetAllControlsUsingRecursive(this);
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
                    tp.BackColor = FRM.GetBaseColor();
                }
                else if (ctl.GetType().Name.ToLower() == "roundpanel")
                {
                    rp = ctl as RoundPanel;
                    rp.TitleBackColor = FRM.GetGridBackColor();
                    rp.TitleForeColor = FRM.GetForeColor    ();
                }
            }
            FRM.SetUCParent(FrmCamCtl, this.pnVisn);
            FrmCamCtl.Tracker.SetImageBound(new Rectangle(10, 10, 100, 100));
            FrmCamCtl.Toolbar.DoubleClick += Toolbar_DoubleClick;

            pnSize[0] = new Size(529, 358);
            pnSize[1] = new Size(825, 800);
        }


        //--------------------------------------------------------------------------
        private void FrmMProject_Load(object sender, EventArgs e)
        {
            Rectangle Rect;
            //
            tabMenu.Height    = this.Height + tabMenu.ItemSize.Height;
                    Rect      = new Rectangle(tpgMenu1.Left, tpgMenu1.Top, tpgMenu1.Width, tpgMenu1.Height);
            tabMenu.Region    = new Region(Rect); 
            //
            FRM.SetFormParent(FrmCtlBtn ,this.pnHandle  );
            FRM.SetFormParent(FRM.SVision, this.pnBaseVisn);
            //

            cbCamera.Items.Clear();
            cbInspection.Items.Clear();
            for (int n = 0; n < (int)EN_CAM.EndofCam; n++) cbCamera.Items.Add(Enum.GetName(typeof(EN_CAM), n));
            for (int n = 0; n < (int)EN_VISN_TYPE.FAlgn; n++) cbInspection.Items.Add(Enum.GetName(typeof(EN_VISN_TYPE), n));
            cbProjInfo1_1.Items.AddRange(vDEF.STR_WAF_SIZE);
            cbProjInfo1_2.Items.AddRange(vDEF.STR_WAF_TYPE);

            cbCamera.SelectedIndex = 0;
            cbInspection.SelectedIndex = 0;
            cbProjInfo1_1.SelectedIndex = 0;
            cbProjInfo1_2.SelectedIndex = 0;

            btnSave.Visible = false;
        }
        //--------------------------------------------------------------------------
        private void FrmMProject_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }
        private void FrmMProject_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible && !actived) 
            {//Show
                FRM.ShowFormParent(FrmCtlBtn ,this.pnHandle  );
                FRM.ShowUCParent(FrmCamCtl, this.pnVisn);
                FRM.ShowFormParent(FRM.SVision, this.pnBaseVisn);

                //
                FNC.ShowSubMenu(ref sgSelPart, FRM.GetGridBackColor(),  "Conversion" ,
                                                                        "Wafer Info",
                                                                        "Vision Info");
                //
                SelPage(m_iSelPage);    
			    sgSelPart.ClearSelection();
			    sgSelPart.Rows[m_iSelPage].Cells[0].Selected = true;
                tmProc.Enabled = true;  
            }
            if(!this.Visible && actived) 
            {//Hide
                FRM.HideFormParent(FrmCtlBtn  );
                FRM.HideUCParent  (FrmCamCtl  );
                FRM.HideFormParent(FRM.SVision);
                //
                tmProc.Enabled = false;
            }
            actived = this.Visible;
        }

        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }

            tmProc.Enabled = false;

            switch (tabMenu.SelectedIndex)
            {
                default: tpgPage1Update(); break;
                case  0: tpgPage1Update(); break;
                case  1: tpgPage2Update(); break;
                case  2: tpgPage3Update(); break;
            }
            tmProc.Enabled = true;

        }
        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("The parameter can not be changed while the Machine is running.");
                return;
            }      
            if(!FRM.ShowMsg(true, "Confirm", "Do you want to save the parameter?", EN_MSG_KIND.UserModal)) return;
            switch (tabMenu.SelectedIndex)
            {
                default: tpgPage1Save(); break;
                case  0: tpgPage1Save(); break;
                case  1: tpgPage2Save(); break;
                case  2: tpgPage3Save(); break;
            }
        }

        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

			m_iSelPage = iGridR;

            SelPage(m_iSelPage);
        }

        void SelPage(int iPage)
        {//화면의 메뉴 선택시 Tab PAGE 결정 
            btnSave.Visible = false;
            tabMenu.SelectedIndex = iPage; 
            switch (iPage)
            {
                default: tpgPage1Show(); break;
                case  0: tpgPage1Show(); break;
                case  1: tpgPage2Show(); break;
                case  2: tpgPage3Show(); break;
            }
            
        }
        public void FormHide()
        {
            FRM.HideFormParent(FRM.STool  );
			//FRM.HideFormParent(FRM.Cam1   );
            FRM.HideFormParent(FrmCtlBtn  );
        }

        #region "PAGE1"
        public void tpgPage1Show()
        {//Tab Page #1 Show (화면 업데이트)
            String sJobPath  = Application.StartupPath + "\\Project";
            FNC.UpdateDirByGrid(sJobPath , ref sgJobList, FRM.GetGridBackColor(), false, true);
            //new, Copy, Rename, Delete Button 상태에 따라 화면 Update
            FDisplay(m_iSelF);
            lbCrntDvc       .Text= cDEF.FM._sCrntDevice        ;
        }

        public void tpgPage1Save()
        {//Tab Page #1 Hide (Save)


        }

        public void tpgPage1Update()
        {//Timer에서 Page1의 업데이트할 내용을 추가  


        }
        public void FDisplay(int iSel)
        {
            if(sgJobList.CurrentCell == null) return;

            int iGridR = sgJobList.CurrentCell.RowIndex   ; 
            int iGridC = sgJobList.CurrentCell.ColumnIndex;
            edFName1.Text = sgJobList[1,iGridR].Value.ToString();
            lbSelDvc.Text = sgJobList[1,iGridR].Value.ToString();

            btnJobCtrl1.BackColor = (iSel == 0) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;
            btnJobCtrl2.BackColor = (iSel == 1) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;
            btnJobCtrl3.BackColor = (iSel == 2) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;
            btnJobCtrl4.BackColor = (iSel == 3) ? Color.LightGoldenrodYellow : Color.WhiteSmoke;

            switch (iSel) {
               default : break;
               case 0  : lb1_3.Hide(); edFName2.Hide(); lb1_2.Text ="New Name" ; lb1_3.Text =""         ; break; //New
               case 1  : lb1_3.Show(); edFName2.Show(); lb1_2.Text ="Form Name"; lb1_3.Text ="To Name"  ; break; //Modify
               case 2  : lb1_3.Show(); edFName2.Show(); lb1_2.Text ="Form Name"; lb1_3.Text ="To Name"  ; break; //Copy
               case 3  : lb1_3.Hide(); edFName2.Hide(); lb1_2.Text ="Del Name" ; lb1_3.Text =""         ; break; //Del
               }
        }
        private void sgJobList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FDisplay(m_iSelF);
        }

        private void btnJobCtrl1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            m_iSelF = iTag;
            FDisplay(m_iSelF);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            //Local Var.
            String sSelDvc  = lbSelDvc.Text;

            //Check Running Status.
            if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("The JobFile can not be download while the Machine is running.");
                return;
            }
            //if (cDEF.LOT._bLotOpen)
            //{
            //    MsgBox.Warning("Lot Open중에는 Recipe 변경이 불가합니다.");
            //    return;
            //}

            //JOB File 적용.
            if(sSelDvc  == "") { if(FRM.ShowMsg(true, "Confirm", "Plase Selected job file")) return; }

            if(!FRM.ShowMsg(true, "Confirm", "Do you want to DOWNLOAD selected job file?")) return;


            //File Loading.
            cDEF.FM.LoadProj      (true , sSelDvc);
            cDEF.FM.ApplyProject  (sSelDvc       );
            tpgPage1Show          (              );

            //Trace Log.
            cDEF.LOG.Trace($"DOWNLOAD JOB FILE H:{sSelDvc}"); 
        }
        //------------------------------------------------------------------------
        private void btnExec_Click(object sender, EventArgs e)
        {
            //Local Var.
            String sPath1 = Application.StartupPath + "\\Project\\" + edFName1.Text; 
            String sPath2 = Application.StartupPath + "\\Project\\" + edFName2.Text; 

            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("The JobFile can not be download while the Machine is running.");
                return;
            }

            //Check None Name.
            if (edFName1.Text == "") 
            { 
                MsgBox.Warning("Please select Job File");
                return;
            }

   	        switch (m_iSelF) {
             default : break;
    	        case 0://New를 선택 했을 경우        
                       if(edFName1.Text == lbCrntDvc.Text) { MsgBox.Warning("You can not change the use Job File.");  return; }
                       if(!FRM.ShowMsg(true, "Confirm", "Do you want to " + btnJobCtrl1.Text + " ?")) return;
                       FNC.CreateDir(sPath1);       
                       cDEF.LOG.Trace(btnJobCtrl1.Text + "(" + edFName1.Text + ")");
                       break;
                case 1://Rename을 선택했을 경우
                       if(edFName1.Text == lbCrntDvc.Text) { MsgBox.Warning("You can not change the use Job File.");  return; }
                       if(!FRM.ShowMsg(true, "Confirm", "Do you want to " + btnJobCtrl2.Text + " ?")) return;

                       FNC.MoveDir(sPath1, sPath2);       
                       cDEF.LOG.Trace(btnJobCtrl2.Text + "(" + edFName1.Text + "->" + edFName2.Text + ")");                      
                       break;
                case 2://Copy를 선택했을 경우
                       if (edFName2.Text == lbCrntDvc.Text) { MsgBox.Warning("You can not change the use Job File.");  return; }
                       if(!FRM.ShowMsg(true, "Confirm", "Do you want to " + btnJobCtrl3.Text + " ?")) return;
                       FNC.CopyDir(sPath1, sPath2);      
                       cDEF.LOG.Trace(btnJobCtrl3.Text + "(" + edFName1.Text + "->" + edFName2.Text + ")");   
                       break;
                case 3://Delete. 선택했을 경우
                       if(edFName1.Text == lbCrntDvc.Text) { MsgBox.Warning("You can not change the use Job File.");  return; }
                       if(!FRM.ShowMsg(true, "Confirm", "Do you want to " + btnJobCtrl4.Text + " ?")) return;
                       FNC.DeleteDir(sPath1);
                       cDEF.LOG.Trace(btnJobCtrl4.Text + "(" + edFName1.Text + ")"); 
                       break;
            }
            tpgPage1Show();
        }

        #endregion "PAGE1"

        #region "PAGE2"
        public void tpgPage2Show()
        {//Tab Page #2 Show (화면 업데이트)
            btnSave.Visible = true;
            //Default
            cbProjInfo1_1.SelectedIndex = FM.ProjBase.iWaferSize;
            cbProjInfo1_2.SelectedIndex = FM.ProjBase.iWaferType;

            tbProjInfo1_2.Text          = Convert.ToString(FM.ProjBase.dNotchSize);
            tbProjInfo1_3.Text          = Convert.ToString(FM.ProjBase.dEdgeLength);
            tbProjInfo1_4.Text          = Convert.ToString(FM.ProjBase.dEdgeAngle);

            tbProjInfo1_1.Checked   = FM.ProjBase.bUseCenterGap;
            tbProjInfo1_5.Text      = Convert.ToString(FM.ProjBase.dLimitCenterGap);
        }
        //------------------------------------------------------------------------
        public void tpgPage2Save()
        {//Tab Page #2 Hide (Save)				
            //Default
            POSN.WriteDatChLog(3, ref FM.ProjBase.iWaferSize          , cbProjInfo1_1.SelectedIndex  , "cbProjInfo1_1");
            POSN.WriteDatChLog(3, ref FM.ProjBase.iWaferType          , cbProjInfo1_2.SelectedIndex  , "cbProjInfo1_2");

            POSN.WriteDatChLog(3, ref FM.ProjBase.dNotchSize          , tbProjInfo1_2.Text           , "tbProjInfo1_2");
            POSN.WriteDatChLog(3, ref FM.ProjBase.dEdgeLength         , tbProjInfo1_3.Text           , "tbProjInfo1_3");
            POSN.WriteDatChLog(3, ref FM.ProjBase.dEdgeAngle          , tbProjInfo1_4.Text           , "tbProjInfo1_4");


            POSN.WriteDatChLog(3, ref FM.ProjBase.bUseCenterGap, tbProjInfo1_1.Checked, "tbProjInfo1_1");
            POSN.WriteDatChLog(3, ref FM.ProjBase.dLimitCenterGap, tbProjInfo1_5.Text, "tbProjInfo1_5");

            //
            FM.ProjBase.Load(false, FM._sCrntDevice);
            FM.ApplyProject (FM._sCrntDevice);

            
            //SetFileSave(EN_SAVE_TYPE.OptEng); //TEST
        }

        public void tpgPage2Update()
        {//Timer에서 Page2의 업데이트할 내용을 추가  

            
        }
        #endregion "PAGE2"

        #region "PAGE3"
        public void tpgPage3Show()
        {//Tab Page #3 Show (화면 업데이트)
            btnSave.Visible = true;

            this.pnVisn.Size = pnSize[0];
            pnBaseVisn.Visible  = true;
            pnVisnParam.Visible = true;
            pnTechParam.Visible = false;


            if (cbCamera.SelectedIndex     < 0) return;
            if (cbInspection.SelectedIndex < 0) return;

            if (cDEF.VISN.Cam[cbCamera.SelectedIndex] == null) return;

            tpgPage3ShowParam(cbCamera.SelectedIndex, cbInspection.SelectedIndex);
        }
        public void tpgPage3Save()
        {//Tab Page #3 Hide (Save)	

            cDEF.FM.ProjBase.Load(false, cDEF.FM._sCrntDevice);
            cDEF.FM.ApplyProject (cDEF.FM._sCrntDevice);
            cDEF.FM.EngrOptn.Load(false);
            FRM.SVision.FormSave();

            if (cbCamera.SelectedIndex                 <    0) return;
            if (cbInspection.SelectedIndex             <    0) return;
            if (cDEF.VISN.Cam[cbCamera.SelectedIndex] == null) return;

            TSET SET = cDEF.VISN.Cam[cbCamera.SelectedIndex].m_SET[cbInspection.SelectedIndex];


            POSN.WriteDatChLog(3, ref SET.sIlluminationName  , cbProjInfo4_1 .Text, "cbProjInfo4_1 ");
            POSN.WriteDatChLog(3, ref SET.sROIName[0]        , cbProjInfo4_2 .Text, "cbProjInfo4_2 ");
            POSN.WriteDatChLog(3, ref SET.sROIName[1]        , cbProjInfo4_3 .Text, "cbProjInfo4_3 ");
            POSN.WriteDatChLog(3, ref SET.sROIName[2]        , cbProjInfo4_4 .Text, "cbProjInfo4_4 ");
            POSN.WriteDatChLog(3, ref SET.sROIName[3]        , cbProjInfo4_5 .Text, "cbProjInfo4_5 ");
            POSN.WriteDatChLog(3, ref SET.sROIName[4]        , cbProjInfo4_6 .Text, "cbProjInfo4_6 ");
                                                                                                   
                                                                                                   
            POSN.WriteDatChLog(3, ref SET.dMinSizeTorr[0]    , tbProjInfo4_1 .Text, "tbProjInfo4_1 ");
            POSN.WriteDatChLog(3, ref SET.dMaxSizeTorr[0]    , tbProjInfo4_2 .Text, "tbProjInfo4_2 ");
            POSN.WriteDatChLog(3, ref SET.dMinSizeTorr[1]    , tbProjInfo4_3 .Text, "tbProjInfo4_3 ");
            POSN.WriteDatChLog(3, ref SET.dMaxSizeTorr[1]    , tbProjInfo4_4 .Text, "tbProjInfo4_4 ");                                                                                                 
            POSN.WriteDatChLog(3, ref SET.dMinSizeTorr[2]    , tbProjInfo4_7 .Text, "tbProjInfo4_7 ");
            POSN.WriteDatChLog(3, ref SET.dMaxSizeTorr[2]    , tbProjInfo4_8 .Text, "tbProjInfo4_8 ");
            POSN.WriteDatChLog(3, ref SET.dMinSizeTorr[3]    , tbProjInfo4_11.Text, "tbProjInfo4_11");
            POSN.WriteDatChLog(3, ref SET.dMaxSizeTorr[3]    , tbProjInfo4_12.Text, "tbProjInfo4_12");
            POSN.WriteDatChLog(3, ref SET.dMaxSizeTorr[4]    , tbProjInfo4_14.Text, "tbProjInfo4_14");
            POSN.WriteDatChLog(3, ref SET.iInscribedPoint    , tbProjInfo4_15.Text, "tbProjInfo4_15");

            POSN.WriteDatChLog(3, ref SET.iThreshold[0]      , tbProjInfo4_5.Text, "tbProjInfo4_5  ");
            POSN.WriteDatChLog(3, ref SET.iThreshold[1]      , tbProjInfo4_6.Text, "tbProjInfo4_6  ");
            POSN.WriteDatChLog(3, ref SET.iThreshold[2]      , tbProjInfo4_13.Text, "tbProjInfo4_13");


            POSN.WriteDatChLog(3, ref SET.iKernel[0]         , tbProjInfo4_9 .Text, "tbProjInfo4_9 ");
            POSN.WriteDatChLog(3, ref SET.iIterable[0]       , tbProjInfo4_10.Text, "tbProjInfo4_10");


            cDEF.VISN.Load(false, cDEF.FM._sCrntDevice);
			//260521 //AOCV
            FrmCamCtl.ClearOverlayImage();
            FrmCamCtl.DrawAllROIs(SET);
        }

        public void tpgPage3Update()
        {//Timer에서 Page3의 업데이트할 내용을 추가  

        }

        private void cbParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCamera    .SelectedIndex < 0) return;
            if (cbInspection.SelectedIndex < 0) return;

            if (cDEF.VISN.Cam[cbCamera.SelectedIndex] == null) return;

            tpgPage3ShowParam(cbCamera.SelectedIndex, cbInspection.SelectedIndex);
        }
        private void tpgPage3ShowParam(int camid, int inspid)
        {
            TSET SET = cDEF.VISN.Cam[camid].m_SET[inspid];
            cbProjInfo4_1 .Text = SET.sIlluminationName.ToString();
            cbProjInfo4_2 .Text = SET.sROIName[0].ToString();
            cbProjInfo4_3 .Text = SET.sROIName[1].ToString();
            cbProjInfo4_4 .Text = SET.sROIName[2].ToString();
            cbProjInfo4_5 .Text = SET.sROIName[3].ToString();
            cbProjInfo4_6 .Text = SET.sROIName[4].ToString();
                          
            //Vision      
            tbProjInfo4_1 .Text = SET.dMinSizeTorr[0].ToString();
            tbProjInfo4_2 .Text = SET.dMaxSizeTorr[0].ToString();
            tbProjInfo4_3 .Text = SET.dMinSizeTorr[1].ToString();
            tbProjInfo4_4 .Text = SET.dMaxSizeTorr[1].ToString();                 
            tbProjInfo4_7 .Text = SET.dMinSizeTorr[2].ToString();
            tbProjInfo4_8 .Text = SET.dMaxSizeTorr[2].ToString();
            tbProjInfo4_11.Text = SET.dMinSizeTorr[3].ToString();
            tbProjInfo4_12.Text = SET.dMaxSizeTorr[3].ToString();
            tbProjInfo4_14.Text = SET.dMaxSizeTorr[4].ToString();
            tbProjInfo4_15.Text = SET.iInscribedPoint.ToString();

            tbProjInfo4_5.Text  = SET.iThreshold[0].ToString();
            tbProjInfo4_6.Text  = SET.iThreshold[1].ToString();
            tbProjInfo4_13.Text = SET.iThreshold[2].ToString();

            tbProjInfo4_9  .Text = SET.iKernel[0].ToString();
            tbProjInfo4_10 .Text = SET.iIterable[0].ToString();
			//260521 //AOCV            
            FrmCamCtl.ClearOverlayImage();
            FrmCamCtl.DrawAllROIs(SET);
        }
        private void cbProjInfo4_DropDown(object sender, EventArgs e)
        {
            ComboBox bt = (sender as ComboBox);

            string sMask = string.Empty;
            switch (bt.Name)
            {
                case "cbProjInfo4_1": sMask = Tillumination.Extension; break;
                default:  sMask = TROI.Extension; break;
            }


            DirectoryInfo di = new DirectoryInfo(TVisnUnit.Path);
            if (!di.Exists) return;

            bt.Items.Clear();
            foreach (FileInfo file in di.GetFiles(sMask))
            {
                string name = file.Name.Replace(sMask.Replace("*", ""), string.Empty);
                bt.Items.Add(name);
            }
        }

        private void Toolbar_DoubleClick(object sender, EventArgs e)
        {
            if (this.pnVisn.Size == pnSize[0])
            {
                this.pnVisn.Size = pnSize[1];
                this.pnVisn.BringToFront();
                pnBaseVisn.Visible = false;
                pnVisnParam.Visible = false;
                pnTechParam.Visible = true;
            }
            else
            {
                this.pnVisn.Size = pnSize[0];
                this.pnVisn.BringToFront();
                pnBaseVisn.Visible = true;
                pnVisnParam.Visible = true;
                pnTechParam.Visible = false;
            }
        }


        #endregion "PAGE3"

        private void btAdmin02_Click(object sender, EventArgs e)
        {
            cDEF.VISN.SetLightOn(true, (int)EN_CAM.WTB, EN_VISN_TYPE.WAlgn);
        }
    }

}
