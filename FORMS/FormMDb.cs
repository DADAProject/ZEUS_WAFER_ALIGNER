using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SQLite;


namespace eMachine
{
    public partial class FrmMDb : Form
    {
        //
        FrmCtrlMC      FrmCtlBtn   = new FrmCtrlMC     (0);
        //
        int    m_iLogSel   ;
        String m_sFieldJam ;
        String m_sFieldProd;
        //int    m_iFirstPage;
        bool   actived     ;

        public FrmMDb()
        {
            InitializeComponent();
            //
            Panel           pn;
            TabPage         tp;
            Grouper     gb;
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
                else if (ctl.GetType().Name.ToLower() == "tabpage")
                {
                    tp = ctl as TabPage;
                    tp.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "grouper")
                {
                    gb = ctl as Grouper;
                    gb.BackgroundColor = FRM.GetBaseColor();
                    //gb.CustomGroupBoxColor = Color.LightGray;
                    //gb.PaintGroupBox = true;
                }
                else if (ctl.GetType().Name.ToLower() == "radiobutton")
                {
                    rb = ctl as RadioButton;
                    rb.ForeColor = FRM.GetForeColor();
                }
            }
        }
        //--------------------------------------------------------------------------
        private void FrmMDb_Load(object sender, EventArgs e)
        {
            //TabControl Tab 제거
            tabMenu.Height  = this.Height + tabMenu.ItemSize.Height;
            Rectangle Rect = new Rectangle(tpgMenu1.Left, tpgMenu1.Top, tpgMenu1.Width, tpgMenu1.Height);
            tabMenu.Region = new Region(Rect);

            rtLogView .Dock = System.Windows.Forms.DockStyle.Fill; 
            sgLog     .Dock = System.Windows.Forms.DockStyle.Fill;
            sgJam     .Dock = System.Windows.Forms.DockStyle.Fill;
            sgProdList.Dock = System.Windows.Forms.DockStyle.Fill;
            sgProd    .Dock = System.Windows.Forms.DockStyle.Fill;

            FRM.SetFormParent(FrmCtlBtn   ,this.pnHandle);

            m_iLogSel       = 0;

            //
        }
        //--------------------------------------------------------------------------
        private void FrmMDb_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc    .Enabled = false;
        }

        private void FrmMDb_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible && !actived) 
            {//Show
                FRM.ShowFormParent(FrmCtlBtn   ,this.pnHandle);
                //
                FNC.ShowSubMenu(ref sgSelPart, FRM.GetGridBackColor(),  "Log View     ",
                                                                        "Jam History  ");
                                                                      //"Align History");
                                                                      //"Camera Live");
                SelPage(0);    
                tmProc.Enabled = true;  
            }
            if(!this.Visible && actived) 
            {//Hide
                FRM.HideFormParent(FrmCtlBtn    );
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
                default: tpgPage1Update(); break;
                case  0: tpgPage1Update(); break;
                case  1: tpgPage2Update(); break;
                //case  2: tpgPage4Update(); break;
                //case  3: tpgPage4Update(); break;
            }
            tmProc.Enabled = true;
        }
        //------------------------------------------------------------------------
        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            SelPage(iGridR);
        }
        //--------------------------------------------------------------------------
        void SelPage(int iPage)
        {//화면의 메뉴 선택시 Tab PAGE 결정 
            tabMenu.SelectedIndex = iPage; 
            switch (iPage)
            {
                default: tpgPage1Show(); break;
                case  0: tpgPage1Show(); break;
                case  1: tpgPage2Show(); break; //Jam History
                //case  2: tpgPage4Show(); break; //Align History
                //case  3: tpgPage5Show(); break; //Camera Live
            }            
        }
        //------------------------------------------------------------------------
        public void FormHide()
        {
            FRM.HideFormParent(FrmCtlBtn    );
        }
        //--------------------------------------------------------------------------
        #region "PAGE1"
        public void tpgPage1Show()
        {//Tab Page #1 Show (화면 업데이트)
            btnSelLog1.Visible = true ; btnSelLog1.Text = EN_LOG_TYPE.EVENT .ToString(); //"Event";
            btnSelLog2.Visible = true ; btnSelLog2.Text = "Data Change";
            btnSelLog3.Visible = true ; btnSelLog3.Text = EN_LOG_TYPE.JAM   .ToString  (); //""JAM";
            btnSelLog4.Visible = true ; btnSelLog4.Text = EN_LOG_TYPE.TCPIP .ToString();
            btnSelLog5.Visible = true ; btnSelLog5.Text = EN_LOG_TYPE.RESULT.ToString  ();

            DisplayLogList(0);
        }
        //--------------------------------------------------------------------------
        public void tpgPage1Save()
        {//Tab Page #1 Hide (Save)


        }
        //--------------------------------------------------------------------------
        public void tpgPage1Update()
        {//Timer에서 Page1의 업데이트할 내용을 추가  


        }
        //--------------------------------------------------------------------------
        private void btnSelLog1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            DisplayLogList(iTag);
        }
        //--------------------------------------------------------------------------
        private void sgLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Button Btn = (sender as Button);
            //if(Btn == null) return;
            //int iTag = Convert.ToInt32(Btn.Tag);
            //DisplayLogList(iTag);
            DisplayLog();
        }
        //--------------------------------------------------------------------------
        public void SetSelLogBtnColor   (int iSel)
        {
            btnSelLog1.BackColor = (iSel == 0) ? Color.SkyBlue : Color.Gainsboro;
            btnSelLog2.BackColor = (iSel == 1) ? Color.SkyBlue : Color.Gainsboro;
            btnSelLog3.BackColor = (iSel == 2) ? Color.SkyBlue : Color.Gainsboro;
            btnSelLog4.BackColor = (iSel == 3) ? Color.SkyBlue : Color.Gainsboro;
            btnSelLog5.BackColor = (iSel == 4) ? Color.SkyBlue : Color.Gainsboro;
        }
        //--------------------------------------------------------------------------
        public void DisplayLogList   (int iSel)
        {
	        m_iLogSel = iSel;

	        String sPath = String.Empty;
            String sMask = String.Empty;

	             if (m_iLogSel == 0) { sPath = Application.StartupPath + "\\LOG\\EVENT"                                ; sMask = "*.txt"; }
	        else if (m_iLogSel == 1) { sPath = Application.StartupPath + "\\LOG\\DataChangeLog"                        ; sMask = "*.log"; }
            else if (m_iLogSel == 2) { sPath = Application.StartupPath + "\\LOG\\Jam"                                  ; sMask = "*.txt"; }
            else if (m_iLogSel == 3) { sPath = Application.StartupPath + "\\LOG\\TCPIP"                                ; sMask = "*.txt"; }
            else if (m_iLogSel == 4) { sPath = Application.StartupPath + "\\LOG\\" + EN_LOG_TYPE.RESULT.ToString()+"//"; sMask = "*.txt"; }

            if (sPath == String.Empty) return;

            SetSelLogBtnColor(iSel);
            FNC.UpdateFileByGrid(sPath, ref sgLog, FRM.GetGridBackColor(), true, false, sMask);
	        DisplayLog();
        }
        //------------------------------------------------------------------------
        public void DisplayLog   ()
        {
	        String sPath   = "";
	        String sLogFile;

            if(sgLog.CurrentCell == null) 
            {
                rtLogView.Clear();
                return;
            }

            int iGridR = sgLog.CurrentCell.RowIndex   ; 
            int iGridC = sgLog.CurrentCell.ColumnIndex;

	        rtLogView.Clear();

	             if (m_iLogSel == 0) sPath = Application.StartupPath + "\\LOG\\EVENT\\"        ;
	        else if (m_iLogSel == 1) sPath = Application.StartupPath + "\\LOG\\DataChangeLog\\";
            else if (m_iLogSel == 2) sPath = Application.StartupPath + "\\LOG\\Jam\\"          ;
            else if (m_iLogSel == 3) sPath = Application.StartupPath + "\\LOG\\TCPIP\\"        ;
            else if (m_iLogSel == 4) sPath = Application.StartupPath + "\\LOG\\" + EN_LOG_TYPE.RESULT.ToString() + "//";

            if (sgLog[1,iGridR].Value.ToString() == "") return;
	        sLogFile = sPath + sgLog[1,iGridR].Value.ToString();

            if(!File.Exists(sLogFile)) return;
            rtLogView.LoadFile(sLogFile, RichTextBoxStreamType.UnicodePlainText);

            rtLogView.Select(rtLogView.Text.Length, 0); //하단 이동
            rtLogView.ScrollToCaret();
        }

        #endregion "PAGE1"

        #region "PAGE2"
        public void tpgPage2Show()
        {//Tab Page #2 Show (화면 업데이트)
            dtpErrStart.Value = DateTime.Now;
            dtpErrEnd  .Value = DateTime.Now;
            
            m_sFieldJam = cDEF.SPC.dbJAM.UpdateGroupFieldByGrid(ref sgErrDisp);
            if(rbErrDisp1.Checked) DisplayJam(); 
            rbErrDisp1.Checked  = true;

        }
        //--------------------------------------------------------------------------
        public void tpgPage2Save()
        {//Tab Page #2 Hide (Save)
            cDEF.SPC.dbJAM.ExportExcel(ref sgJam);

        }
        //--------------------------------------------------------------------------
        public void tpgPage2Update()
        {//Timer에서 Page2의 업데이트할 내용을 추가  


        }
        //--------------------------------------------------------------------------
        void DisplayJam       ()
        {
            int iDispDate = 0;

                 if(rbErrDisp1.Checked) iDispDate = 0; //ToDay
            else if(rbErrDisp2.Checked) iDispDate = 1; //All
            else if(rbErrDisp3.Checked) iDispDate = 2; //Period

            if(m_sFieldJam == "") 
            {
                DisplayItemJam();
                return;
            }        
            
         	cDEF.SPC.dbJAM.ClearSqlString ();
            if(iDispDate == 0){
	            cDEF.SPC.dbJAM.SetDateTime    ("Day"         , DateTime.Now , DateTime.Now          );
                }
            if(iDispDate == 2){
	            cDEF.SPC.dbJAM.SetDateTime    ("Day"         , dtpErrStart.Value , dtpErrEnd  .Value);
                }

            cDEF.SPC.dbJAM.GroupCountSql  (m_sFieldJam    , "QTY");
            cDEF.SPC.dbJAM.SetSorting     ("", 1);

            cDEF.SPC.dbJAM.UpdateSQLByGrid(ref sgErrList);            
            cDEF.SPC.dbJAM.UpdateSQLByChart(ref ctJam, true); //Chart.

            //
            DisplayItemJam();
        }
        //--------------------------------------------------------------------------
        void DisplayItemJam   (String sField = "" , String sData = "")
        {
            int iDispDate = 0;

                 if(rbErrDisp1.Checked) iDispDate = 0; //ToDay
            else if(rbErrDisp2.Checked) iDispDate = 1; //All
            else if(rbErrDisp3.Checked) iDispDate = 2; //Period

        	cDEF.SPC.dbJAM.ClearSqlString ();
            if(iDispDate == 0){
	            cDEF.SPC.dbJAM.SetDateTime    ("Day"         , DateTime.Now , DateTime.Now          );
                }
            if(iDispDate == 2){
	            cDEF.SPC.dbJAM.SetDateTime    ("Day"         , dtpErrStart.Value , dtpErrEnd  .Value);
                }

            if(sField != "" &&  sData != "") 
            {
                if(sField == "Day")   cDEF.SPC.dbJAM.SetSearchData(sField, Convert.ToDateTime(sData));
                else                  cDEF.SPC.dbJAM.SetSearchData(sField, sData);
            }
            cDEF.SPC.dbJAM.SetSorting     ("", 1);
            cDEF.SPC.dbJAM.SearchSql      ();
            cDEF.SPC.dbJAM.UpdateSQLByGrid(ref sgJam);
        }
        //--------------------------------------------------------------------------
        private void rbErrDisp1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rButton = (sender as RadioButton);
            if (rButton == null) return;
            if (rButton.Checked) DisplayJam();
        }
        //--------------------------------------------------------------------------
        private void dtpErrStart_ValueChanged(object sender, EventArgs e)
        {
            if(rbErrDisp3.Checked) DisplayJam();
        }
        //--------------------------------------------------------------------------
        private void sgErrDisp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            String sCellDat = CurrGrid[1,iGridR].Value.ToString().Trim().ToUpper();
            m_sFieldJam = sCellDat;
            
            DisplayJam();
        }
        //--------------------------------------------------------------------------
        private void sgErrList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            String sCellDat = CurrGrid[0,iGridR].Value.ToString().Trim().ToUpper();
            DisplayItemJam(m_sFieldJam, sCellDat);
        }
        //--------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            int iTag = Convert.ToInt32(btn.Tag);

            if (!FRM.ShowMsg(true, " Warning ", "Do you want to All Clear JAM DB ?", EN_MSG_KIND.UserModal)) return;
            //
            if      (iTag == 0) cDEF.SPC.dbJAM .DelAllData();
            else if (iTag == 1) cDEF.SPC.dbPROD.DelAllData();
        }
        //--------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            int iTag = Convert.ToInt32(btn.Tag);

            if      (iTag == 0) cDEF.SPC.dbJAM .ExportExcel(ref sgJam );
            else if (iTag == 1) cDEF.SPC.dbPROD.ExportExcel(ref sgProd);
        }
        #endregion "PAGE2"
        //--------------------------------------------------------------------------
        #region "PAGE3"
        public void tpgPage3Show() //Align History
        {//Tab Page #3 Show (화면 업데이트)
            dtpProdStart.Value = DateTime.Now;
            dtpProdEnd  .Value = DateTime.Now;
            m_sFieldProd = cDEF.SPC.dbPROD.UpdateGroupFieldByGrid(ref sgProdDisp);
            if(rbProdDisp1.Checked) DisplayProd(); 
            rbProdDisp1.Checked  = true; 
        }
        //--------------------------------------------------------------------------
        public void tpgPage3Save()
        {//Tab Page #3 Hide (Save)
            cDEF.SPC.dbPROD.ExportExcel(ref sgProd);

        }
        //--------------------------------------------------------------------------
        public void tpgPage3Update()
        {//Timer에서 Page3의 업데이트할 내용을 추가  


        }
        //--------------------------------------------------------------------------
        void DisplayProd ()
        {
            int iDispDate = 0;

                 if(rbProdDisp1.Checked) iDispDate = 0; //ToDay
            else if(rbProdDisp2.Checked) iDispDate = 1; //All
            else if(rbProdDisp3.Checked) iDispDate = 2; //Period

            if(m_sFieldProd == "") 
            {
                DisplayItemProd();
                return;
            }        
            
         	cDEF.SPC.dbPROD.ClearSqlString ();
            if(iDispDate == 0)
            {
	            cDEF.SPC.dbPROD.SetDateTime    ("Day"         , DateTime.Now , DateTime.Now          );
            }
            if(iDispDate == 2)
            {
	            cDEF.SPC.dbPROD.SetDateTime    ("Day"         , dtpProdStart.Value , dtpProdEnd  .Value);
            }

	        cDEF.SPC.dbPROD.SetGroupCount   (m_sFieldProd                         );
	        cDEF.SPC.dbPROD.SetSumAvg       ("GoodQty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("RejectQty"       ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin2Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin3Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin4Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin5Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin6Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin7Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin8Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("Bin9Qty"         ,EN_GROUP_TYPE.SUM );
	        cDEF.SPC.dbPROD.SetSumAvg       ("JamQty"          ,EN_GROUP_TYPE.SUM );
            cDEF.SPC.dbPROD.SetSorting      ("", 1);
            cDEF.SPC.dbPROD.GroupSumAvgSql  (m_sFieldProd   );

            cDEF.SPC.dbPROD.UpdateSQLByGrid (ref sgProdList);
            cDEF.SPC.dbPROD.UpdateSQLByChart(ref ctProd, false); //Chart.

            //
            DisplayItemProd();
        }
        //--------------------------------------------------------------------------
        void DisplayItemProd   (String sField = "" , String sData = "")
        {
            int iDispDate = 0;

                 if(rbProdDisp1.Checked) iDispDate = 0; //ToDay
            else if(rbProdDisp2.Checked) iDispDate = 1; //All
            else if(rbProdDisp3.Checked) iDispDate = 2; //Period

         	cDEF.SPC.dbPROD.ClearSqlString ();
            if(iDispDate == 0){
	            cDEF.SPC.dbPROD.SetDateTime    ("Day"         , DateTime.Now , DateTime.Now          );
                }
            if(iDispDate == 2){
	            cDEF.SPC.dbPROD.SetDateTime    ("Day"         , dtpProdStart.Value , dtpProdEnd  .Value);
                }

            if(sField != "" &&  sData != "") 
            {
                if(sField == "Day")   cDEF.SPC.dbPROD.SetSearchData(sField, Convert.ToDateTime(sData));
                else                  cDEF.SPC.dbPROD.SetSearchData(sField, sData);
            }
            cDEF.SPC.dbPROD.SetSorting     ("", 1);
            cDEF.SPC.dbPROD.SearchSql      ();
            cDEF.SPC.dbPROD.UpdateSQLByGrid(ref sgProd);
        }
        //--------------------------------------------------------------------------
        private void rbProdDisp1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rButton = (sender as RadioButton);
            if(rButton.Checked) DisplayProd();
        }
        //--------------------------------------------------------------------------
        private void dtpProdStart_ValueChanged(object sender, EventArgs e)
        {
            if(rbProdDisp3.Checked) DisplayProd();
        }
        //--------------------------------------------------------------------------
        private void sgProdDisp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            String sCellDat = CurrGrid[1,iGridR].Value.ToString().Trim().ToUpper();
            m_sFieldProd = sCellDat;
            DisplayProd();
        }
        //--------------------------------------------------------------------------
        private void sgProdList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            String sCellDat = CurrGrid[0,iGridR].Value.ToString().Trim().ToUpper();
            DisplayItemProd(m_sFieldProd, sCellDat);
        }
        #endregion "PAGE3"

        #region "PAGE4"
        public void tpgPage4Show() //Align History
        {//Tab Page #4 Show (화면 업데이트)

        }
        //--------------------------------------------------------------------------
        public void tpgPage4Save()
        {//Tab Page #4 Hide (Save)


        }
        //--------------------------------------------------------------------------
        public void tpgPage4Update()
        {//Timer에서 Page4의 업데이트할 내용을 추가  


        }
        //--------------------------------------------------------------------------
        #endregion "PAGE4"

        #region "PAGE5"
        public void tpgPage5Show()
        {//Tab Page #5 Show (화면 업데이트)

        }

        public void tpgPage5Save()
        {//Tab Page #5 Hide (Save)


        }

        public void tpgPage5Update()
        {//Timer에서 Page5의 업데이트할 내용을 추가  


        }
        //--------------------------------------------------------------------------


        #endregion "PAGE5"


    }
}
