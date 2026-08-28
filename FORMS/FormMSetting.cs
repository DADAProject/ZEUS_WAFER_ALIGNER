using System;
using System.Drawing;
using System.Windows.Forms;

namespace eMachine
{
    public partial class FrmMSetting : Form
    {
        //
        FrmCtrlMC   FrmCtlBtn  = new FrmCtrlMC  (0);
        FrmChart    FrmChart   = new FrmChart   ( );
        
        //	
        int  m_iSelPage   ;
        int  m_iSelActrNo ;
        int  m_iSelErrNo  ;
        int  m_nLastPage  ;
        bool actived      ;

        TextBox[]  TcpItem = new TextBox[30]; //TCP/IP Item 
        //int    [,] arItems = new int[(int)EN_SEND_LIST.EndOfList,10] { 
        //    {0 ,1,                     -1,-1,-1,-1,-1,-1,-1,-1    },
        //    {0 ,1,                     -1,-1,-1,-1,-1,-1,-1,-1    },
        //    {0 ,1,2,3,                 -1,-1,-1,-1,-1,-1          },
        //    {0 ,1,4,                   -1,-1,-1,-1,-1,-1,-1       },
        //    {5 ,                       -1,-1,-1,-1,-1,-1,-1,-1,-1 },
        //    {0 ,6,7,8,                 -1,-1,-1,-1,-1,-1          },
        //    {0 ,6,7,9,                 -1,-1,-1,-1,-1,-1          },
        //    {0 ,6,7,9,                 -1,-1,-1,-1,-1,-1          },
        //    {10,                       -1,-1,-1,-1,-1,-1,-1,-1,-1 },
        //    {11,12,13,14,15,7,16,17,18,-1                         }, 
        //    {19,20,21,22,23,           -1,-1,-1,-1,-1             }};

        public FrmMSetting()
        {
            InitializeComponent();

            //
            Panel           pn;
            TabPage         tp;
            //Label           lb;
            RadioButton     rb;
            Grouper         gb;
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
                else if (ctl.GetType().Name.ToLower() == "tabpage")
                {
                    tp = ctl as TabPage;
                    tp.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "radiobutton")
                {
                    rb = ctl as RadioButton;
                    rb.ForeColor = FRM.GetForeColor();
                }
                else if (ctl.GetType().Name.ToLower() == "grouper")
                {
                    gb = ctl as Grouper;
                    gb.BackgroundColor = FRM.GetGridBackColor();
                }
            }

            //
            TextBox tbitem; 
            for (int n = 0; n < 30; n++)
            {
                tbitem = grouperTCP.Controls[string.Format($"tbTcpItem{n+1:D2}")] as TextBox;
                if (tbitem == null) break ;
                TcpItem[n] = tbitem;
            }
            
            ////
            //cDEF.LOG.DisplayComEvent += DisplayComEvent;

        }
        //------------------------------------------------------------------------
        private void FrmMSetting_Load(object sender, EventArgs e)
        {
            tabMenu.Height  = this.Height + tabMenu.ItemSize.Height;
            Rectangle Rect  = new Rectangle(tpgMenu1.Left, tpgMenu1.Top, tpgMenu1.Width, tpgMenu1.Height);
            tabMenu.Region  = new Region(Rect);
                 
			m_iSelPage   = 0 ; //Selected Page
            m_iSelActrNo = 0 ;
            m_iSelErrNo  = 0 ;
            m_nLastPage  = -1;

            //
            FRM.SetFormParent (FrmCtlBtn  ,this.pnHandle );

            btnSave.Visible = false;

            //RFID
            comboBox1.Items.Clear();
            comboBox1.Items.Clear();

            for (int n = 0; n < (int)EN_RFID_ID.EndOfId; n++)
                comboBox1.Items.Add(Enum.GetName(typeof(EN_RFID_ID), n));
            for (int n = 0; n < (int)EN_RFID_CMD.EndOfCmd; n++) 
                comboBox2.Items.Add(Enum.GetName(typeof(EN_RFID_CMD), n));
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;


            //cbSendList.Items.Clear();
            //for (int n = 0; n < (int)EN_SEND_LIST.EndOfList; n++)
            //{
            //    cbSendList.Items.Add((EN_SEND_LIST)n);
            //}

        }
        //------------------------------------------------------------------------
        private void FrmMSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc    .Enabled = false;
        }
        //------------------------------------------------------------------------
        private void FrmMSetting_VisibleChanged(object sender, EventArgs e)
        {
             if(this.Visible && !actived) 
             {//Show
                FRM.ShowFormParent(FrmCtlBtn , this.pnHandle );
                //
                FNC.ShowSubMenu(ref sgSelPart, FRM.GetGridBackColor(),  "Option"    ,
                                                                        //"Live"      , 
                                                                        "Error List",
                                                                        "Test");
                SelPage(m_iSelPage);  
				sgSelPart.ClearSelection();
				sgSelPart.Rows[m_iSelPage].Cells[0].Selected = true;

                tmProc.Enabled = true;  
             }

             if(!this.Visible && actived)
             {//Hide
                FRM.HideFormParent(FrmCtlBtn );

                tmProc    .Enabled = false;
             }
             actived = this.Visible;
        }
        //------------------------------------------------------------------------
        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }

            tmProc.Enabled = false;

            switch (tabMenu.SelectedIndex)
            {
                default: tpgPage5Update(); break;
                
                case  0: tpgPage5Update(); break;
                case  1: tpgPage6Update(); break;
                case  2: tpgPage3Update(); break;
                case  3: tpgPage6Update(); break; //Test
                //case  3: tpgPage3Update(); break;
                //case  5: tpgPage1Update(); break;
            }
            tmProc.Enabled = true;

        }
        //------------------------------------------------------------------------
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
                default: tpgPage5Save(); break;
                
                case 0: tpgPage5Save(); break;
                case 1: tpgPage3Save(); break;
                case 2: tpgPage6Save(); break;
                //case 3: tpgPage3Save(); break;
                //case 4: tpgPage6Save(); break;
                //case 5: tpgPage1Save(); break;
            }
        }
        //------------------------------------------------------------------------
        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR            = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC            = CurrGrid.CurrentCell.ColumnIndex;

            SelPage(iGridR);
        }
        //------------------------------------------------------------------------
        void SelPage(int iPage)
        {//화면의 메뉴 선택시 Tab PAGE 결정 

            if (m_nLastPage == iPage) return;
            m_nLastPage = iPage; 

            btnSave.Visible = false;
            tabMenu.SelectedIndex = iPage;
            
            switch (iPage)
            {
                default: tpgPage5Show(); break;
            
                case  0: tpgPage5Show(); break; //Option
                case  1: tpgPage3Show(); break; //Error List
                case  2: tpgPage6Show(); break; //TEST
                //case  3: tpgPage6Show(); break; //Monitor
                //case  4: tpgPage4Show(); break; //Lamp
                //case  5: tpgPage5Show(); break; //
                //case  6: /*tpgPage1Show();*/ break; //
            }


            //
            m_iSelPage = iPage;            
        }
        //--------------------------------------------------------------------------
        public void FormHide()
        {
            FRM.HideFormParent(FrmCtlBtn);
            FRM.HideFormParent(FrmChart );
        }
        //--------------------------------------------------------------------------
        #region "PAGE1"
        public void tpgPage1Show()
        {//Tab Page #1 Show (화면 업데이트)
            btnSave.Visible = true;
            cDEF.FM.NetOptn.UpdateByGrid(true, ref sgNetwork);
        }

        public void tpgPage1Save()
        {//Tab Page #1 Hide (Save)
            cDEF.FM.NetOptn.UpdateByGrid(false, ref sgNetwork);

        }

        public void tpgPage1Update()
        {//Timer에서 Page1의 업데이트할 내용을 추가  


        }

        private void sgNetwork_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = e.RowIndex   ; 
            int iGridC = e.ColumnIndex;
        }

        private void sgNetwork_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
			DataGridView CurrGrid = (sender as DataGridView);

            FNC.SameCellFormatting(ref CurrGrid, e);
        }

        private void sgNetwork_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            FNC.SameCellPainting(ref CurrGrid, e);
        }
        #endregion "PAGE1"

        #region "PAGE2"
        public void tpgPage2Show()
        {//Tab Page #2 Show (화면 업데이트)
            btnSave.Visible = true;
            cDEF.ACTR.UpdateByGrid(ref sgActuator);
        }

        public void tpgPage2Save()
        {//Tab Page #2 Hide (Save)
            cDEF.ACTR.SaveFrGrid(ref sgActuator);

        }

        public void tpgPage2Update()
        {//Timer에서 Page2의 업데이트할 내용을 추가  
            cDEF.ACTR.DisplayStat(ref sgActuator);

            if(m_iSelActrNo<0 || m_iSelActrNo>=cDEF.ACTR._iNumOfACT) return;
            
            lbFwdStat.BackColor = (cDEF.ACTR.Complete(m_iSelActrNo, 1)     ) ? Color.Lime : Color.Gray;
            lbAlarm  .BackColor = (cDEF.ACTR.Err     (m_iSelActrNo   ) != 0) ? Color.Red  : Color.Gray;
            lbBwdStat.BackColor = (cDEF.ACTR.Complete(m_iSelActrNo, 0)     ) ? Color.Lime : Color.Gray;

        }
        private void btnMotrCtrl5_Click(object sender, EventArgs e)
        {
            bool isOk = false;
            int  iDly = Convert.ToInt32(edContDelay.Text);;

            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);


            //Check Running Status.
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("The Manual function can not be Operating while the Machine is running.");
                return;
                }

            switch(iTag) 
            {
                default : break;
                case 1: cDEF.ACTR.MoveCyl(m_iSelActrNo, (int)EN_ACTR_CMD.Fwd);
                        break;
                case 2: cDEF.ACTR.MoveCyl(m_iSelActrNo, (int)EN_ACTR_CMD.Bwd);
                        break;
                case 3: 
	                    isOk   =  cDEF.ACTR.MoveCyl(m_iSelActrNo, (int)EN_ACTR_CMD.Fwd); 
	                    if (isOk) {
		                    cDEF.ACTR.Reset();
		                    cDEF.ACTR.SetRpt(-1           , false , iDly); 
		                    cDEF.ACTR.SetRpt(m_iSelActrNo , true  , iDly);
		                    }

	                    cDEF.ACTR.m_bRptActrIng = true;
                        break;
                case 4:  
	                    cDEF.ACTR.Reset();
	                    cDEF.ACTR.SetRpt(-1     , false , iDly); 
	                    for(int i=0;i<sgActuator.RowCount;i++)
	                    {
		                    if(sgActuator[13,i].Value.ToString().ToUpper() == "TRUE")
		                    {
			                    cDEF.ACTR.SetRpt(i, true, iDly);
			                    isOk = true;
		                    }
	                    }
	                    if(isOk) cDEF.ACTR.m_bRptActrIng = true;

                          break;
                case 5:   cDEF.ACTR.m_bRptActrIng = false; 
                          break;
                case 6:   cDEF.ACTR.Reset();
                          break;
            }
        }
        //--------------------------------------------------------------------------
        private void sgActuator_CurrentCellChanged(object sender, EventArgs e)
        {
             DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;   
            m_iSelActrNo = iGridR; 
        }

        #endregion "PAGE2"
        //--------------------------------------------------------------------------
        #region "PAGE3"
        public void tpgPage3Show()
        {//Tab Page #3 Show (화면 업데이트)
            btnSave.Visible = true;
	        cbErrPart.Items.Clear();
	        for(int i=0; i<=cDEF.POSN.GetPartCnt(); i++)
	        {//ALL = -1, piSys = PartCnt + 1;
		        cbErrPart.Items.Add(cDEF.POSN.GetPartName(i));
	        }

            cDEF.EPU.Display(ref sgErrList);
            UpdateErrItem(ref pbErrImg);
        }
        //--------------------------------------------------------------------------
        public void tpgPage3Save()
        {//Tab Page #3 Hide (Save)
            if(m_iSelErrNo<=0) return;

            if      (rbErrGrade1.Checked) cDEF.EPU[m_iSelErrNo].m_iGrade = 0 ;
            else if (rbErrGrade2.Checked) cDEF.EPU[m_iSelErrNo].m_iGrade = 1 ;
            else if (rbErrGrade3.Checked) cDEF.EPU[m_iSelErrNo].m_iGrade = 2 ;

            if      (rbErrKind1.Checked ) cDEF.EPU[m_iSelErrNo].m_iKind  = 0 ;
            else if (rbErrKind2.Checked ) cDEF.EPU[m_iSelErrNo].m_iKind  = 1 ;
            else if (rbErrKind3.Checked ) cDEF.EPU[m_iSelErrNo].m_iKind  = 2 ;
            else if (rbErrKind4.Checked ) cDEF.EPU[m_iSelErrNo].m_iKind  = 3 ;

            cDEF.EPU[m_iSelErrNo].m_bHoldErr   = rbErrHold2.Checked       ;
            cDEF.EPU[m_iSelErrNo].m_bSendErr   = rbErrSend2.Checked       ;
            cDEF.EPU[m_iSelErrNo].m_iPart      = cbErrPart.SelectedIndex  ;
            cDEF.EPU[m_iSelErrNo].m_sName      = edErrName.Text           ;
            cDEF.EPU[m_iSelErrNo].m_sSoluttion = mmErrSolution.Text.Trim();
            cDEF.EPU[m_iSelErrNo].m_sCause     = mmErrCause.Text.Trim()   ;

            cDEF.EPU.SaveErrDataOneini (m_iSelErrNo);

            //cDEF.EPU.SetCause    (m_iSelErrNo, mmErrCause.Text   );
            //cDEF.EPU.SetSolution (m_iSelErrNo, mmErrSolution.Text);

            sgErrList[1, m_iSelErrNo-1].Value = edErrName.Text;

        }
        //------------------------------------------------------------------------
        public void tpgPage3Update()
        {//Timer에서 Page3의 업데이트할 내용을 추가  


        }
        //------------------------------------------------------------------------
        public void UpdateErrItem(ref PictureBox pb)
        {
            edErrNo.Text = string.Format("ERR{0,4:0000}" , m_iSelErrNo);

            if      (cDEF.EPU[m_iSelErrNo].m_iGrade == 0) rbErrGrade1.Checked = true;
            else if (cDEF.EPU[m_iSelErrNo].m_iGrade == 1) rbErrGrade2.Checked = true;
            else if (cDEF.EPU[m_iSelErrNo].m_iGrade == 2) rbErrGrade3.Checked = true;

            if      (cDEF.EPU[m_iSelErrNo].m_iKind == 0) rbErrKind1.Checked = true  ;
            else if (cDEF.EPU[m_iSelErrNo].m_iKind == 1) rbErrKind2.Checked = true  ;
            else if (cDEF.EPU[m_iSelErrNo].m_iKind == 2) rbErrKind3.Checked = true  ;
            else if (cDEF.EPU[m_iSelErrNo].m_iKind == 3) rbErrKind4.Checked = true  ;

            rbErrHold1.Checked       = !cDEF.EPU[m_iSelErrNo].m_bHoldErr  ;
            rbErrHold2.Checked       =  cDEF.EPU[m_iSelErrNo].m_bHoldErr  ;
                                                                          
            rbErrSend1.Checked       = !cDEF.EPU[m_iSelErrNo].m_bSendErr  ;
            rbErrSend2.Checked       =  cDEF.EPU[m_iSelErrNo].m_bSendErr  ;

            cbErrPart.SelectedIndex  =  cDEF.EPU[m_iSelErrNo].m_iPart     ;
            edErrName.Text           =  cDEF.EPU[m_iSelErrNo].m_sName     ; 
            mmErrCause.Text          =  cDEF.EPU[m_iSelErrNo].m_sCause    ; //cDEF.EPU.GetCause   (m_iSelErrNo);
            mmErrSolution.Text       =  cDEF.EPU[m_iSelErrNo].m_sSoluttion; //cDEF.EPU.GetSolution(m_iSelErrNo);

            cDEF.EPU.GetPicture(m_iSelErrNo , ref pb);

        }
        private void sgErrList_CurrentCellChanged(object sender, EventArgs e)
        {
             DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;   
            m_iSelErrNo = iGridR+1; 
            UpdateErrItem(ref pbErrImg);
        }
        //--------------------------------------------------------------------------
        private void btnForceErr_Click(object sender, EventArgs e)
        {
            cDEF.EPU.SetErr(m_iSelErrNo, true);
            cDEF.EPU._bUpdatedErrForm = false;
        }
        //--------------------------------------------------------------------------
        private void btnExcel_Click(object sender, EventArgs e)
        {
            cDEF.EPU.ExportList();
        }

        #endregion "PAGE3"
        //--------------------------------------------------------------------------
        #region "PAGE4"
        public void tpgPage4Show()
        {//Tab Page #4 Show (화면 업데이트)
            btnSave.Visible = true;
            cDEF.LampBuzz._bTest     = false ;
	        cDEF.LampBuzz._iTestStat = (int)EN_SEQ_STAT.Stop;
	        cDEF.LampBuzz.UpdateGrid(true, ref sgLampBuzz);
        }

        public void tpgPage4Save()
        {//Tab Page #4 Hide (Save)
	        cDEF.LampBuzz._bTest     = false ;
	        cDEF.LampBuzz._iTestStat = (int)EN_SEQ_STAT.Stop;
            cDEF.LampBuzz.UpdateGrid(false, ref sgLampBuzz);

        }
        //--------------------------------------------------------------------------
        public void tpgPage4Update()
        {//Timer에서 Page4의 업데이트할 내용을 추가  


        }
        //--------------------------------------------------------------------------
        private void sgLampBuzz_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int iCol = e.ColumnIndex;
            int iRow = e.RowIndex;

            if(iCol != 5) return;
            cDEF.LampBuzz._bBuzzOff  = false;
            cDEF.LampBuzz._bTest     = !cDEF.LampBuzz._bTest;
            cDEF.LampBuzz._iTestStat = (cDEF.LampBuzz._bTest) ? iRow : (int)cDEF.SEQ._iSeqStat;
        }
        #endregion "PAGE4"
        //--------------------------------------------------------------------------
        #region "PAGE5"
        public void tpgPage5Show()
        {//Tab Page #6 Show (화면 업데이트)
            btnSave.Visible = true;

            cbCOM.Items.Clear();
            for (int n = 0; n < 10; n++) cbCOM.Items.Add(string.Format($"COM{n+1}"));

            tsMSOption1      .Checked = cDEF.FM.EngrOptn.bUseAutoHome                     ;
            tsMSOption2      .Checked = cDEF.FM.EngrOptn.bAutoLotEnd                      ;
            tsMSOption3      .Checked = cDEF.FM.EngrOptn.bLampatRun                       ;
           
            tsHoldErrProcess .Checked = cDEF.FM.EngrOptn.bHoldErrProcess                  ;
            edLastErrTime    .Text    = Convert.ToString(cDEF.FM.EngrOptn.iLastErrTime   );
            edLastErrCnt     .Text    = Convert.ToString(cDEF.FM.EngrOptn.iLastErrCnt    );
          //edChangeOperTime .Text    = Convert.ToString(cDEF.FM.EngrOptn.iChangeOperTime);
          //tbEQNo           .Text    = cDEF.FM.EngrOptn.sEQNo;
            
            if      (cDEF.FM.EngrOptn.iVacOption == 0) rbVacOpt1.Checked = true; //Only Sensor
            else if (cDEF.FM.EngrOptn.iVacOption == 1) rbVacOpt2.Checked = true; //Only Vacuum
            else if (cDEF.FM.EngrOptn.iVacOption == 2) rbVacOpt3.Checked = true; //All Check

            tbVacCnt    .Text = cDEF.FM.EngrOptn.iVacCount  .ToString();      
            tbVacTimeOut.Text = cDEF.FM.EngrOptn.iVacTimeOut.ToString();
            tbVacDelay  .Text = cDEF.FM.EngrOptn.nVacDelay  .ToString();

            cbSelMenu1_1     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[0]  ; 
            cbSelMenu1_3     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[1]  ; 
            cbSelMenu1_4     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[2]  ; 
            cbSelMenu1_5     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[3]  ; 
            cbSelMenu1_6     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[4]  ; 
            cbSelMenu1_7     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[5]  ; 
            cbSelMenu1_8     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[6]  ;

            cbSelMenu2_1     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[0]  ; 
            cbSelMenu2_3     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[1]  ; 
            cbSelMenu2_4     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[2]  ; 
            cbSelMenu2_5     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[3]  ; 
            cbSelMenu2_6     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[4]  ; 
            cbSelMenu2_7     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[5]  ; 
            cbSelMenu2_8     .Checked = cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[6]  ; 

            //
            btOpt01.Text      = Convert.ToString(cDEF.FM.EngrOptn.nServerPort     );
            btOpt02.Text      = Convert.ToString(cDEF.FM.EngrOptn.dToleranceX     );
            btOpt03.Text      = Convert.ToString(cDEF.FM.EngrOptn.dToleranceY     );
            btOpt04.Text      = Convert.ToString(cDEF.FM.EngrOptn.dToleranceT     );
            btOpt05.Text      = Convert.ToString(cDEF.FM.EngrOptn.sBCRIP          );
            btOpt06.Text      = Convert.ToString(cDEF.FM.EngrOptn.nBCRRetryCnt    );
            btOpt07.Text      = Convert.ToString(cDEF.FM.EngrOptn.nMaxImageStorage);
            tbMaxDay.Text     = Convert.ToString(cDEF.FM.EngrOptn.iMaxImageDay    );
            //btOpt08.Text      = string.Format($"{Application.StartupPath}\\IMAGE" );//Convert.ToString(cDEF.FM.EngrOptn.sImageSavePath  );
            btOpt08.Text      = cDEF.FM.EngrOptn.sImageSavePath;
            btOpt09.Text      = Convert.ToString(cDEF.FM.EngrOptn.nTestRunCnt     );
            btOpt10.Text      = Convert.ToString(cDEF.FM.EngrOptn.nRetryCnt       );
            btOpt11.Text      = Convert.ToString(cDEF.FM.EngrOptn.nAlignCnt       );
            btOpt12.Text      = Convert.ToString(cDEF.FM.EngrOptn.nBCRPort        );
            
            cbCOM  .Text      = Convert.ToString(cDEF.FM.EngrOptn.sCom_Light      );

            tbIP1.Text        = cDEF.FM.EngrOptn.sIP1.ToString();
            tbIP2.Text        = cDEF.FM.EngrOptn.sIP2.ToString();
            tbIP3.Text        = cDEF.FM.EngrOptn.sIP3.ToString();
            tbIP4.Text        = cDEF.FM.EngrOptn.sIP4.ToString();


            //
            tsOptn01.Checked  = cDEF.FM.EngrOptn.bUseBCR        ;
            tsOptn02.Checked  = cDEF.FM.EngrOptn.bUseAlignCheck ;
            tsOptn03.Checked  = cDEF.FM.EngrOptn.bUseImageSave  ;
            tsOptn04.Checked  = cDEF.FM.EngrOptn.bUseOnlyXY     ;
            tsOptn05.Checked  = cDEF.FM.EngrOptn.bUseRingFrame1 ;
            tsOptn06.Checked  = cDEF.FM.EngrOptn.bUseRingFrame2 ;
            tsOptn07.Checked  = cDEF.FM.EngrOptn.bUseRingFrame3 ;
            tsOptn08.Checked  = cDEF.FM.EngrOptn.bUseAlignVerify;
            tsOptn09.Checked  = cDEF.FM.EngrOptn.bUseWaferSkip  ;
			tsOptn10.Checked  = cDEF.FM.EngrOptn.bUseFindRingFrameAngle ;
			tsOptn11.Checked  = cDEF.FM.EngrOptn.bUseVisnIO     ;
            tsOptn12.Checked  = cDEF.FM.EngrOptn.bUseDcutAlgnT  ;

            btOpt22.Text      = Convert.ToString(cDEF.FM.EngrOptn.dToleranceX_Verify);
            btOpt23.Text      = Convert.ToString(cDEF.FM.EngrOptn.dToleranceY_Verify);
            btOpt24.Text      = Convert.ToString(cDEF.FM.EngrOptn.dToleranceT_Verify);


        }
        //--------------------------------------------------------------------------
        public void tpgPage5Save()
        {//Tab Page #5 Hide (Save)
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseAutoHome     , tsMSOption1      .Checked ? 1 : 0, "EnableOptn1   ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bAutoLotEnd      , tsMSOption2      .Checked ? 1 : 0, "EnableOptn2   ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bLampatRun       , tsMSOption3      .Checked ? 1 : 0, "EnableOptn3   ");

            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bHoldErrProcess  , tsHoldErrProcess .Checked ? 1 : 0, "HoldErrProcess");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.iLastErrTime     , edLastErrTime    .Text           , "LastErrTime   ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.iLastErrCnt      , edLastErrCnt     .Text           , "LastErrCnt    ");
          //cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.iChangeOperTime  , edChangeOperTime .Text           , "ChangeOperTime");
          //cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sEQNo            , tbEQNo           .Text           , "tbEQNo"        );
            if      (rbVacOpt1.Checked) cDEF.FM.EngrOptn.iVacOption = 0;
            else if (rbVacOpt2.Checked) cDEF.FM.EngrOptn.iVacOption = 1;
            else if (rbVacOpt3.Checked) cDEF.FM.EngrOptn.iVacOption = 2;
            
			cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.iVacCount        , tbVacCnt    .Text, "VacCount  ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.iVacTimeOut      , tbVacTimeOut.Text, "VacTimeOut");
            
			cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nVacDelay        , tbVacDelay       .Text           , "tbVacDelay"    );

            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[0], cbSelMenu1_1 .Checked ? 1 : 0, "1_EnableMenu1");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[1], cbSelMenu1_3 .Checked ? 1 : 0, "1_EnableMenu3");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[2], cbSelMenu1_4 .Checked ? 1 : 0, "1_EnableMenu4");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[3], cbSelMenu1_5 .Checked ? 1 : 0, "1_EnableMenu5");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[4], cbSelMenu1_6 .Checked ? 1 : 0, "1_EnableMenu6");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[5], cbSelMenu1_7 .Checked ? 1 : 0, "1_EnableMenu7");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Operator].bEnableMenu[6], cbSelMenu1_8 .Checked ? 1 : 0, "1_EnableMenu8");

            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[0], cbSelMenu2_1 .Checked ? 1 : 0, "2_EnableMenu1");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[1], cbSelMenu2_3 .Checked ? 1 : 0, "2_EnableMenu3");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[2], cbSelMenu2_4 .Checked ? 1 : 0, "2_EnableMenu4");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[3], cbSelMenu2_5 .Checked ? 1 : 0, "2_EnableMenu5");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[4], cbSelMenu2_6 .Checked ? 1 : 0, "2_EnableMenu6");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[5], cbSelMenu2_7 .Checked ? 1 : 0, "2_EnableMenu7");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.LoginSet[(int)EN_LOGIN.Engineer].bEnableMenu[6], cbSelMenu2_8 .Checked ? 1 : 0, "2_EnableMenu8");

            //
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nServerPort             , btOpt01.Text , "ServerPort      ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.dToleranceX             , btOpt02.Text , "ToleranceX      ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.dToleranceY             , btOpt03.Text , "ToleranceY      ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.dToleranceT             , btOpt04.Text , "ToleranceT      ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sBCRIP                  , btOpt05.Text , "BCRIP           ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nBCRRetryCnt            , btOpt06.Text , "BCRRetryCnt     ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nMaxImageStorage        , btOpt07.Text , "MaxImageStorage ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.iMaxImageDay            , tbMaxDay.Text, "MaxImageDay     ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sImageSavePath          , btOpt08.Text , "ImageSavePath   ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nTestRunCnt             , btOpt09.Text , "TestRunCnt      ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nRetryCnt               , btOpt10.Text , "nRetryCnt       ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nAlignCnt               , btOpt11.Text , "nAlignCnt       ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.nBCRPort                , btOpt12.Text , "nBCRPort        ");
            
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sCom_Light              , cbCOM.Text   , "Com_Light       ");
            
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseBCR                 , tsOptn01.Checked ? 1 : 0, "UseBCR             ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseAlignCheck          , tsOptn02.Checked ? 1 : 0, "UseAlignCheck      ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseImageSave           , tsOptn03.Checked ? 1 : 0, "UseImageSave       ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseOnlyXY              , tsOptn04.Checked ? 1 : 0, "UseOnlyXY          ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseRingFrame1          , tsOptn05.Checked ? 1 : 0, "UseRingFrame Notch ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseRingFrame2          , tsOptn06.Checked ? 1 : 0, "UseRingFrame Base  ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseRingFrame3          , tsOptn07.Checked ? 1 : 0, "UseRingFrame Sawing");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseAlignVerify         , tsOptn08.Checked ? 1 : 0, "UseAlignVerify     ");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseWaferSkip           , tsOptn09.Checked ? 1 : 0, "UseWaferSkip       ");
 			cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseFindRingFrameAngle  , tsOptn10.Checked ? 1 : 0, "UseRingFrame Angle ");
 			cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseVisnIO              , tsOptn11.Checked ? 1 : 0, "UseVisnIO"          );
 			cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.bUseDcutAlgnT           , tsOptn12.Checked ? 1 : 0, "UseDcutAlgnT"       );
            
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sIP1                    , tbIP1.Text    , "IP1");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sIP2                    , tbIP2.Text    , "IP2");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sIP3                    , tbIP3.Text    , "IP3");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.sIP4                    , tbIP4.Text    , "IP4");

            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.dToleranceX_Verify      , btOpt22.Text , "ToleranceX_Verify");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.dToleranceY_Verify      , btOpt23.Text , "ToleranceY_Verify");
            cDEF.POSN.WriteDatChLog(4, ref cDEF.FM.EngrOptn.dToleranceT_Verify      , btOpt24.Text , "ToleranceT_Verify");


            //
            cDEF.FM.EngrOptn .Load  (false);

            cDEF.FM.LoadLoginSet(false);
            
            //
            tpgPage5Show();
        }
        //------------------------------------------------------------------------
        public void tpgPage5Update()
        {//Timer에서 Page6의 업데이트할 내용을 추가  

            //pnAlignRepeat.Visible = cDEF.FM.IsMasterLv();
            pnManCnt.Visible = cDEF.FM.IsMasterLv();

        }
        #endregion "PAGE5"

        #region "PAGE6"
        //------------------------------------------------------------------------
        public void tpgPage6Show()
        {
            //TEST

        }
        //------------------------------------------------------------------------
        public void tpgPage6Save()
        {

        }
        //------------------------------------------------------------------------
        public void tpgPage6Update()
        {//Timer에서 Page6의 업데이트할 내용을 추가  

            //Manual Test 
            lbBcrData .Text    = cDEF.BCR?._sReadBcr;
            lbBcrCon.BackColor = cDEF.BCR ._IsConnect ? Color.Lime : Color.Gray;

        }

        #endregion "PAGE6"
        //------------------------------------------------------------------------
        private void pbErrImg_DoubleClick(object sender, EventArgs e)
        {
        }
        //------------------------------------------------------------------------
        private void lbION01_Click(object sender, EventArgs e)
        {
            //
            Label sl = sender as Label;
            int nTag = Convert.ToInt32(sl.Tag);

            EN_ION_ID id = EN_ION_ID.ALIGN + nTag;

            //cDEF.SEQ.WLM.SetIonizer(id, true);
        }
        //------------------------------------------------------------------------
        private void btIONOff_Click(object sender, EventArgs e)
        {
            for (EN_ION_ID n = 0; n < EN_ION_ID.EndOfId ; n++)
            {
                //cDEF.SEQ.WLM.SetIonizer(n, false);
            }
        }
        //------------------------------------------------------------------------
        private void button5_Click(object sender, EventArgs e)
        {
            ////웨어퍼 그림을 그려줌
            ////데이터 복사 
            //List<_CircleCodi> temp = cDEF.Aligner.AlignUnit.make_CircleInfo(cDEF.Aligner.AlignUnit.CircleInfo
            //    , 50
            //    , 200
            //    , 200
            //    , 1.0);
            //
            //cDEF.Aligner.AlignUnit.DrawCircle(ref pictureBox1, temp);
        }
        //------------------------------------------------------------------------
        private void button6_Click(object sender, EventArgs e)
        {
            ////파일오픈창 생성 및 설정
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.InitialDirectory = Environment.CurrentDirectory;
            //ofd.Title = "Load Align Raw Data";
            //ofd.FileName = "test";
            //ofd.Filter = "Raw File (*.txt) | *.txt; | 모든 파일 (*.*) | *.*";
            //
            ////파일 오픈창 로드
            //DialogResult dr = ofd.ShowDialog();
            //
            ////OK버튼 클릭시
            //if (dr == DialogResult.OK)
            //{
            //    cDEF.Aligner.AlignUnit.LoadData(ofd.FileName);
            //}
        }
        //------------------------------------------------------------------------
        private void btColor_Click(object sender, EventArgs e)
        {
            //Color Change
            FRM.BinColor.ShowDialog();
        }
        //------------------------------------------------------------------------
        private void btRFRead_Click(object sender, EventArgs e)
        {
            //
            //Button sbt  = sender as Button;
            //string stxt = sbt.Text;
            //
            //int nCh     = comboBox1.SelectedIndex; 
            //int nCmd    = comboBox2.SelectedIndex; 
            //
            //switch (stxt)
            //{
            //    case "Read":
            //        if (nCmd >= (int)EN_RFID_CMD.rfcWrite1) break;
            //        cDEF.RFID.ClearReadBuf((EN_RFID_ID)nCh);
            //        //cDEF.RFID.CmdSetRFID  ((EN_RFID_ID)nCh, EN_RFID_CMD.rfcRead1);
            //        cDEF.RFID.CmdSetRFID  ((EN_RFID_ID)nCh, (EN_RFID_CMD)nCmd);
            //        break;
            //    
            //    case "Write":
            //        if (nCmd < (int)EN_RFID_CMD.rfcWrite1) break; 
            //
            //        cDEF.RFID.ClearWriteBuf((EN_RFID_ID)nCh);
            //        //cDEF.RFID.CmdSetRFID   ((EN_RFID_ID)nCh, EN_RFID_CMD.rfcWrite1);
            //        cDEF.RFID.CmdSetRFID   ((EN_RFID_ID)nCh, (EN_RFID_CMD)nCmd);
            //        break;
            //
            //    case "Clear":
            //        cDEF.RFID.ClearReadBuf ((EN_RFID_ID)nCh);
            //        cDEF.RFID.ClearWriteBuf((EN_RFID_ID)nCh);
            //
            //        break;
            //
            //    default:
            //        break;
            //}

        }
        //------------------------------------------------------------------------
        private void btBarcodeRead_Click(object sender, EventArgs e)
        {
            //
            Button sbt = sender as Button;
            string stxt = sbt.Text;

            int nCh = comboBox1.SelectedIndex;
            switch (stxt)
            {
                case "Read":
                    cDEF.BCR?.CmdSetRead();
                    break;

                case "Write":

                    break;

                case "Clear":
                    cDEF.BCR?.Reset();
                    break;

                default:
                    break;
            }

        }
        //------------------------------------------------------------------------
        private void btBarcodeClear_Click(object sender, EventArgs e)
        {
            cDEF.BCR?.DataClear();
            //lbBcrData.Text = "";
        }
        //------------------------------------------------------------------------
        private void btTcpSend_Click(object sender, EventArgs e)
        {
            int    nSelIndex = cbSendList.SelectedIndex;
          //string sSelMsg = cbSendList.Text;

            //int.TryParse(TcpItem[0].Text, out int nPort);
            //nPort -= 1; 

            //switch ((EN_SEND_LIST)nSelIndex)
            //{
            //    case EN_SEND_LIST.C600_FOUP_ID_Read_Result_Report:
            //        cDEF.COMASM.CmdC600_FOUPIDReadResult(nPort, TcpItem[1].Text);
            //        break;
            //    case EN_SEND_LIST.C601_FOUP_ID_write_Result_Report:
            //        cDEF.COMASM.CmdC601_FOUPIDWriteResult(nPort, TcpItem[1].Text);
            //        break;
            //    case EN_SEND_LIST.C602_Port_Status_Report:
            //        cDEF.COMASM.CmdC602_PortStatusReport(nPort, TcpItem[1].Text, TcpItem[2].Text, TcpItem[3].Text);
            //        break;
            //    case EN_SEND_LIST.C604_Port_Slot_Map_Report:
            //        cDEF.COMASM.CmdC604_PortSlotMapReport(nPort, TcpItem[1].Text, TcpItem[4].Text);
            //        break;
            //    case EN_SEND_LIST.C605_Robot_Status_Report:
            //        cDEF.COMASM.CmdC605_RobotStatusReport(TcpItem[5].Text);
            //        break;
            //    case EN_SEND_LIST.C611_Panel_ID_Reading_Status_Report:
            //        cDEF.COMASM.CmdC611_PanelIDReadingStatusReport(TcpItem[0].Text, TcpItem[6].Text, TcpItem[7].Text, TcpItem[8].Text);
            //        break;
            //    case EN_SEND_LIST.C612_Panel_CCD_Alignment_Status_Report:
            //        cDEF.COMASM.CmdC612_PanelCCDAlignStatusReport(TcpItem[0].Text, TcpItem[6].Text, TcpItem[7].Text, TcpItem[9].Text);
            //        break;
            //    case EN_SEND_LIST.C613_Subpanel_ID_Reading_Status_Report:
            //        
            //        break;
            //    case EN_SEND_LIST.C631_EFEM_Status_Report:
            //        cDEF.COMASM.CmdC631_EFEM_StatusReport (TcpItem[10].Text);
            //        break;
            //    case EN_SEND_LIST.C632_Panel_Transportation_Report:
            //        cDEF.COMASM.CmdC632_Panel_Transportation_Report(TcpItem[11].Text, TcpItem[12].Text, TcpItem[13].Text, TcpItem[14].Text,
            //            TcpItem[15].Text, TcpItem[7].Text, TcpItem[16].Text, TcpItem[17].Text, TcpItem[18].Text);
            //        break;
            //    case EN_SEND_LIST.C690_Alarm_Event_Report:
            //        cDEF.COMASM.CmdC690_AlarmEventReport(TcpItem[19].Text, TcpItem[20].Text, TcpItem[21].Text, TcpItem[22].Text, TcpItem[23].Text);
            //        break;
            //    default:
            //        break;
            //}

        }
        //------------------------------------------------------------------------
        private void cbSendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            //int nSelIndex = cbSendList.SelectedIndex;
            //if (nSelIndex < 0) return;
            //
            //for (int m = 0; m < TcpItem.Length; m++)
            //{
            //    if (TcpItem[m] == null) continue;
            //    TcpItem[m].Enabled   = false; 
            //    TcpItem[m].BackColor = Color.WhiteSmoke; 
            //}
            //
            //for (int n = 0; n < 10; n++)
            //{
            //    if (arItems[nSelIndex, n] >= 0) 
            //    {
            //        TcpItem[arItems[nSelIndex, n]].BackColor = Color.Yellow; 
            //        TcpItem[arItems[nSelIndex, n]].Enabled   = true;
            //    }
            //}
        }
        //------------------------------------------------------------------------
        private void label39_Click(object sender, EventArgs e)
        {

        }
        //------------------------------------------------------------------------
        private void btOpt08_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (FolderBrowserDialog browser = new FolderBrowserDialog())
            {
                browser.RootFolder = Environment.SpecialFolder.Desktop;
                browser.Description = "이미지 파일 저장 위치";

                if (browser.ShowDialog() == DialogResult.OK)
                {
                    btOpt08.Text = browser.SelectedPath;
                }
            }
        }

        private void tpgMenu5_Click(object sender, EventArgs e)
        {

        }
    }
}
