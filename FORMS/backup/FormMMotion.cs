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
    public partial class FrmMMotion : Form
    {
		
        bool                actived       ;
        int                 m_iSelMotnPart;    
        public EN_PART_SEL  m_iRqPartSel  ;

		//Cell Down 관련
		bool                m_bDrngJog    ;
		Point               m_pDownCellRC = new Point(-1, -1); 

        public FrmMMotion()
        {
            InitializeComponent();
        }

        private void FrmMMotion_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmMMotion_Load(object sender, EventArgs e)
        {
            sgMotor   .Dock = System.Windows.Forms.DockStyle.Fill; 
            sgPos     .Dock = System.Windows.Forms.DockStyle.Fill; 
            sgSelPart .Dock = System.Windows.Forms.DockStyle.Fill; 

            m_iSelMotnPart  =  0;
            m_iRqPartSel    =  EN_PART_SEL.None;
            cDEF.POSN._iSelMotor = -1; //iSelMotr     ; //-1

            FRM.SetFormParent(FRM.SManual  ,this.pnBaseMan);
			FRM.SetFormParent(FRM.CtrlMotr ,this.pnMotr   );

			m_bDrngJog      = false;
			m_pDownCellRC.X = -1;
			m_pDownCellRC.Y = -1;
        }

        private void FrmMMotion_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }

        private void FrmMMotion_VisibleChanged(object sender, EventArgs e)
        {
			//Cell Down 관련 초기화
			m_bDrngJog      = false;
			m_pDownCellRC.X = -1;
			m_pDownCellRC.Y = -1;
			//
            if(this.Visible && !actived) 
            {//Show
                FRM.ShowFormParent(FRM.SManual ,this.pnBaseMan);
				FRM.ShowFormParent(FRM.CtrlMotr,this.pnMotr   );

                cDEF.POSN.DisplayPart(m_iSelMotnPart, ref sgSelPart);
                cDEF.POSN._iSelPart = m_iSelMotnPart;
	            cDEF.POSN.DisplayMotor(ref sgMotor, m_iSelMotnPart);
                this.sgSelPart.CurrentCell = this.sgSelPart[0, m_iSelMotnPart];

                FormShow();  
                tmProc.Enabled = true;
            }
            if(!this.Visible && actived) 
            {//Hide
               tmProc.Enabled = false;
            }
            actived = this.Visible;
        }
        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            m_iRqPartSel = (EN_PART_SEL)iGridR ;
        }

        public void FormShow()
        {
	        String sPartName = cDEF.POSN.GetPartName(m_iSelMotnPart) + "\r\n" + cDEF.POSN.GetMotorName(cDEF.POSN._iSelMotor);
	        lbSelPart.Text = sPartName;
            //안전을 위해서
	        cDEF.POSN.DisplayItem(ref sgPos,true);
            FRM.SManual ._iRqPartSel = (EN_PART_SEL)m_iSelMotnPart;
			FRM.CtrlMotr._iRqPartSel = (EN_PART_SEL)m_iSelMotnPart;
            sgSelPart.CurrentCell = sgSelPart.Rows[m_iSelMotnPart].Cells[0];
        }
        public void FormHide()
        {
            FRM.HideFormParent(FRM.SManual );
			FRM.HideFormParent(FRM.CtrlMotr);
        }
        public void FormSave()
        {
	        cDEF.POSN.DisplayItem(ref sgPos, false);
            cDEF.MOTR.Load(false , cDEF.FM._sCrntDevice, (EN_SEQ_ID)m_iSelMotnPart);
        }

        public void FormUpdate()
        {
            if(m_iRqPartSel != EN_PART_SEL.None)
            {
                m_iSelMotnPart = (int)m_iRqPartSel ;
				
                cDEF.POSN._iSelPart = m_iSelMotnPart;
                cDEF.POSN.DisplayMotor(ref sgMotor, m_iSelMotnPart);
	            cDEF.POSN.DisplayItem (ref sgPos  ,true);
                m_iRqPartSel = EN_PART_SEL.None;
				//Cell Down 관련 초기화
				m_bDrngJog      = false;
				m_pDownCellRC.X = -1;
				m_pDownCellRC.Y = -1;
            }
			//
            cDEF.POSN.DisplayPos(ref sgMotor, cDEF.POSN._iSelPart);

			if (((m_pDownCellRC.X <  0) || (m_pDownCellRC.Y <  0)) &&  m_bDrngJog)   m_bDrngJog = false;
			if (((m_pDownCellRC.X > -1) || (m_pDownCellRC.Y > -1)) && !m_bDrngJog) { m_pDownCellRC.X = -1; m_pDownCellRC.Y = -1; }
			//Mouse Down 계속 이벤트 발생. 이걸로 대체.
			if (m_bDrngJog && ((m_pDownCellRC.X > -1) || (m_pDownCellRC.Y > -1))) {
				MouseEventArgs mArgs = new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 2, MousePosition.X, MousePosition.Y, 0);				
				DataGridViewCellMouseEventArgs dgvmArg = new DataGridViewCellMouseEventArgs(m_pDownCellRC.X, m_pDownCellRC.Y, MousePosition.X, MousePosition.Y, mArgs);				
				sgMotor_CellMouseClick(sgMotor, dgvmArg);
				}	
        }

        private void tmProc_Tick(object sender, EventArgs e)
        {
            //if(!this.Visible) {this.tmProc.Enabled = false; return; }

            tmProc.Enabled = false;
            FormUpdate();
            tmProc.Enabled = true ;
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
            FormShow(     );
        }

        private void sgPos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;
            if(iGridC != 1) return;
            cDEF.POSN.ShowKeyPad();
        }

        private void sgPos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = e.RowIndex   ; 
            int iGridC = e.ColumnIndex;
               
            if(iGridC != 3) return;
            String sCellDat = CurrGrid[iGridC,iGridR].Value.ToString().Trim().ToUpper();
            String sManNo   = sCellDat.Substring(4,4);
            int    iManNo   = Convert.ToInt32(sManNo);
        	cDEF.MAN.ManProcOn (iManNo, true , false);
        }
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

        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            FormSave();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag); 

            switch  (iTag)
            {
                case 1: cDEF.SEQ._bBtnManStart = true;                    break;
                case 2: cDEF.SEQ._bBtnManStop  = true;                    break;
                case 3: cDEF.SEQ._bBtnManReset = !cDEF.SEQ._bBtnManReset; break;  
            }
        }


	}
}
