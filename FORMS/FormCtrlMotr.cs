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
	public partial class FrmCtrlMotr : Form
	{
		bool          actived       ;
        EN_PART_SEL   m_iRqPartSel  ;
        int           m_iSelMotnPart;
		//Cell Down 관련
		bool          m_bDrngJog    ;
		Point         m_pDownCellRC = new Point(-1, -1);

        TOnDelayTimer m_tCheck      = new TOnDelayTimer();

		public EN_PART_SEL  _iRqPartSel{ get { return m_iRqPartSel  ;} set { m_iRqPartSel  = value; }}

		public FrmCtrlMotr()
		{
			InitializeComponent();
            //
            this  .BackColor = FRM.GetBaseColor();
		}

		private void FrmCtrlMotr_Load(object sender, EventArgs e)
		{
			sgMotor   .Dock = System.Windows.Forms.DockStyle.Fill;

            m_iRqPartSel    = EN_PART_SEL.None;
            m_iSelMotnPart  =  0;

			m_bDrngJog      = false;
			m_pDownCellRC.X = -1;
			m_pDownCellRC.Y = -1;
		}

		private void FrmCtrlMotr_FormClosed(object sender, FormClosedEventArgs e)
		{
			timerProc.Enabled = false;
		}

		private void FrmCtrlMotr_VisibleChanged(object sender, EventArgs e)
		{
			m_bDrngJog      = false;
			m_pDownCellRC.X = -1;
			m_pDownCellRC.Y = -1;

            if(this.Visible) 
            {//Show
                //if(actived) return; 
                FormShow();  
                timerProc.Enabled = true;
            }
            else
            {//Hide
                //if(!actived) return; 
                timerProc.Enabled = false;
            }
            actived = Visible;
		}
        public void FormShow()
        {
            if (this.Parent.Visible) 
            {
				//if (this.ParentForm.Name == "FrmMMotion") //FRM.MMotion.RefreshDispPos();
                //{ 
                    IntPtr hwnd = this.ParentForm.Handle; //cDEF.FindWindow(null, "FrmMMotion");
                    try
                    {
                    WinAPI.SendMessageA(hwnd, vDEF.WM_CTRL_MOTR, IntPtr.Zero, IntPtr.Zero); // (hwnd, vDEF.WM_USER_01,  0,  0);
                    }
                    catch (EntryPointNotFoundException e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
                        return;
                    }
            }
        }
        public void FormUpdate()
        {
            //
            if(m_iRqPartSel !=  EN_PART_SEL.None)
            {
                m_iSelMotnPart = (int)m_iRqPartSel;
				cDEF.POSN.DisplayMotor(ref sgMotor, m_iSelMotnPart, FRM.GetGridBackColor());
                m_iRqPartSel = EN_PART_SEL.None;
            }
			cDEF.POSN.DisplayPos(ref sgMotor, m_iSelMotnPart);
			//
			if (((m_pDownCellRC.X <  0) || (m_pDownCellRC.Y <  0)) &&  m_bDrngJog)   m_bDrngJog = false;
			if (((m_pDownCellRC.X > -1) || (m_pDownCellRC.Y > -1)) && !m_bDrngJog) { m_pDownCellRC.X = -1; m_pDownCellRC.Y = -1; }
			//Mouse Down 계속 이벤트 발생. 이걸로 대체.
			if (m_bDrngJog && ((m_pDownCellRC.X > -1) || (m_pDownCellRC.Y > -1))) {
				MouseEventArgs m_mArgs   = new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 2, MousePosition.X, MousePosition.Y, 0);				
				DataGridViewCellMouseEventArgs m_dgvmArg = new DataGridViewCellMouseEventArgs(m_pDownCellRC.X, m_pDownCellRC.Y, MousePosition.X, MousePosition.Y, m_mArgs);				
				sgMotor_CellMouseClick(sgMotor, m_dgvmArg);
                m_mArgs   = null; 
                m_dgvmArg = null;
				}
            
            m_tCheck.OnDelay(m_bDrngJog && (FRM.Message != null), 100);
            if (m_tCheck.Out && FRM.Message.Visible)
            {
                //m_bDrngJog = false;
				MouseEventArgs m_mArgs = new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 2, MousePosition.X, MousePosition.Y, 0);				
				DataGridViewCellMouseEventArgs m_dgvmArg = new DataGridViewCellMouseEventArgs(m_pDownCellRC.X, m_pDownCellRC.Y, MousePosition.X, MousePosition.Y, m_mArgs);		
                sgMotor_CellMouseUp(sgMotor, m_dgvmArg);
                m_mArgs   = null;
                m_dgvmArg = null;
            }
        }
		private void timerProc_Tick(object sender, EventArgs e)
		{
            if(!this.Visible) {this.timerProc.Enabled = false; return; }
            timerProc.Enabled = false;
            FormUpdate();
            timerProc.Enabled = true ;
		}
        //------------------------------------------------------------------------
        private void sgMotor_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
            DataGridView CurrGrid = (sender as DataGridView);
            if (CurrGrid.CurrentCell == null) return;

            int iGridR = e.RowIndex   ; 
            int iGridC = e.ColumnIndex;

            if(iGridR<0) return;
            String sCellDat = CurrGrid[0, iGridR].Value.ToString().Trim().ToUpper();
            String sMotor   = sCellDat.Substring(1,2);
            int    iMotr    = Convert.ToInt32(sMotor);
            int    iManNo;

            if(iGridC != 2  && iGridC != 3 ) return;
            if(iMotr<0 || iMotr>=cDEF.MOTR._iNumOfMotr ) return;
            iManNo = cDEF.MOTR.ManNoJog((EN_MOTR_ID)iMotr); 

            if(iGridC == 2) cDEF.MAN.ManProcOn (iManNo     , true , false );
            if(iGridC == 3) cDEF.MAN.ManProcOff(iManNo     , false , true );
		}
        //------------------------------------------------------------------------
		private void sgMotor_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
            DataGridView CurrGrid = (sender as DataGridView);
            if(CurrGrid.CurrentCell == null) return;			

            int iGridR = e.RowIndex   ; 
            int iGridC = e.ColumnIndex;

            if(iGridR<0) return;

            //String sCellDat = CurrGrid[0,iGridR].Value.ToString().Trim().ToUpper();
            //String sMotor   = sCellDat.Substring(1,2);
            //int    iMotr    = Convert.ToInt32(sMotor);
            //int    iManNo;

            if(iGridC != 2 &&  iGridC != 3) return;
            //if(iMotr<0 || iMotr>=cDEF.MOTR._iNumOfMotr) return;
            //iManNo = cDEF.MOTR.ManNoJog((EN_MOTR_ID)iMotr); 

            //if(iGridC == 2) cDEF.MAN.ManProcOn (iManNo , true , false );
            //if(iGridC == 3) cDEF.MAN.ManProcOff(iManNo , false , true );

			//
			m_bDrngJog      = true;			
			m_pDownCellRC.X = iGridC;
			m_pDownCellRC.Y = iGridR;
		}

		private void sgMotor_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
            DataGridView CurrGrid = (sender as DataGridView);
            if(CurrGrid.CurrentCell == null) return;

            int iGridR = e.RowIndex   ; 
            int iGridC = e.ColumnIndex;

            if(iGridR<0) return;

            String sCellDat = CurrGrid[0,iGridR].Value.ToString().Trim().ToUpper();
            String sMotor   = sCellDat.Substring(1,2);
            int    iMotr    = Convert.ToInt32(sMotor);

			m_bDrngJog      = false;
			m_pDownCellRC.X = -1;
			m_pDownCellRC.Y = -1;
			//
            cDEF.MOTR.Stop((EN_MOTR_ID)iMotr);
		}

		private void sgMotor_SelectionChanged(object sender, EventArgs e)
		{
            DataGridView CurrGrid = (sender as DataGridView);
            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            String sCellDat = CurrGrid[0,iGridR].Value.ToString().Trim().ToUpper();
            String sMotor   = sCellDat.Substring(1,2);
            int    iMotr    = Convert.ToInt32(sMotor);

            cDEF.POSN._iSelMotor = iMotr;
            FormShow();
		}
        //------------------------------------------------------------------------
        private void FrmCtrlMotr_MouseUp(object sender, MouseEventArgs e)
        {
            //
            //JUNG/220128
            //for (int i = 0; i < (int)EN_MOTR_ID.EndOfId; i++)
            //{
            //    cDEF.MOTR.Stop((EN_MOTR_ID)i);
            //}

        }
        //------------------------------------------------------------------------
        private void sgMotor_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //
            //JUNG/220128
            //for (int i = 0; i < (int)EN_MOTR_ID.EndOfId; i++)
            //{
            //    cDEF.MOTR.Stop((EN_MOTR_ID)i);
            //}


        }
    }
}
