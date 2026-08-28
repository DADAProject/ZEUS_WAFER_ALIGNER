using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
	public partial class FrmMControl : Form
	{
        //
        FrmCtrlMC FrmCtlBtn = new FrmCtrlMC(0);
        //
		bool actived      ;

		public FrmMControl()
		{
			InitializeComponent();
		}
		private void FrmMControl_Load(object sender, EventArgs e)
		{
            SetBounds(0, 0, 1280, 895);
            Rectangle Rect = new Rectangle(tpgMenu1.Left, tpgMenu1.Top, tpgMenu1.Width, tpgMenu1.Height);
            tabMenu.Region = new Region(Rect);
            //Double Buffer.            
            FNC.SetDoubleBuffered(this.tabMenu  );
			//
			//FRM.SetFormParent(FRM.StatusMap ,pnStatus ); 
            FRM.SetFormParent(FrmCtlBtn ,this.pnHandle);
			//
			btnSave.Visible = false;
		}
		private void FrmMControl_FormClosed(object sender, FormClosedEventArgs e)
		{
			tmProc    .Enabled = false;
		}

		private void FrmMControl_VisibleChanged(object sender, EventArgs e)
		{
             if(this.Visible) 
             {//Show
                if(actived) return; 
				//FRM.ShowFormParent(FRM.StatusMap ,pnStatus);
                FRM.ShowFormParent(FrmCtlBtn ,this.pnHandle);
                //
                FNC.ShowSubMenu(ref sgSelPart, FRM.GetGridBackColor(),  "Status"      ,
                                                                        "Reaserved #2",    
                                                                        "Reaserved #3",    
                                                                        "Reaserved #4",   
                                                                        "Reaserved #5");
                SelPage(0);    
                tmProc.Enabled = true;  
             }
             else
             {//Hide
                if(!actived) return; 
				//FRM.HideFormParent(FRM.StatusMap);
                FRM.HideFormParent(FrmCtlBtn    );
                tmProc    .Enabled = false;
             }
             actived = Visible;
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
                case  3: tpgPage4Update(); break;
                case  4: tpgPage5Update(); break;
            }
            tmProc.Enabled = true;
		}

		private void btnSave_MouseUp(object sender, MouseEventArgs e)
		{
            if (cDEF.SEQ._bRun) {
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
                case  3: tpgPage4Save(); break;
                case  4: tpgPage5Save(); break;
            }
		}

		private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
		{
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            if(CurrGrid[iGridC, iGridR].Value.ToString().Trim().ToUpper() == "ALL") iGridR -= 1;
            SelPage(iGridR);
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
                case  3: tpgPage4Show(); break;
                case  4: tpgPage5Show(); break;
            }            
        }
        public void FormHide()
        {
			//FRM.HideFormParent(FRM.StatusMap);
            FRM.HideFormParent(FrmCtlBtn    );
            //FRM.StatusMap.FormHide();
        }

        #region "PAGE1"
        public void tpgPage1Show()
        {//Tab Page #1 Show (화면 업데이트)
            btnSave.Visible = false;
        }

        public void tpgPage1Save()
        {//Tab Page #1 Hide (Save)

        }

        public void tpgPage1Update()
        {//Timer에서 Page1의 업데이트할 내용을 추가  

        }
		#endregion "PAGE1"

        #region "PAGE2"
        public void tpgPage2Show()
        {//Tab Page #2 Show (화면 업데이트)
            btnSave.Visible = false;
        }

        public void tpgPage2Save()
        {//Tab Page #2 Hide (Save)

        }

        public void tpgPage2Update()
        {//Timer에서 Page2의 업데이트할 내용을 추가  

        }
		#endregion "PAGE2"

        #region "PAGE3"
        public void tpgPage3Show()
        {//Tab Page #1 Show (화면 업데이트)
            btnSave.Visible = false;
        }

        public void tpgPage3Save()
        {//Tab Page #3 Hide (Save)

        }

        public void tpgPage3Update()
        {//Timer에서 Page3의 업데이트할 내용을 추가  

        }
		#endregion "PAGE3"

        #region "PAGE4"
        public void tpgPage4Show()
        {//Tab Page #4 Show (화면 업데이트)
            btnSave.Visible = false;
        }

        public void tpgPage4Save()
        {//Tab Page #4 Hide (Save)

        }

        public void tpgPage4Update()
        {//Timer에서 Page4의 업데이트할 내용을 추가  

        }
		#endregion "PAGE4"

        #region "PAGE5"
        public void tpgPage5Show()
        {//Tab Page #5 Show (화면 업데이트)
            btnSave.Visible = false;
        }

        public void tpgPage5Save()
        {//Tab Page #5 Hide (Save)

        }

        public void tpgPage5Update()
        {//Timer에서 Page5의 업데이트할 내용을 추가  

        }
		#endregion "PAGE5"
	}
}
