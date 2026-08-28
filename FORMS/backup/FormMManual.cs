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
    public partial class FrmMManual : Form
    {
        bool                actived       ;
        int                 m_iSelMotnPart;    

        public FrmMManual()
        {
            InitializeComponent();
        }

        private void FrmMManual_Load(object sender, EventArgs e)
        {
            FRM.SetFormParent(FRM.SManual  ,this.pnBaseMan );
			//
			m_iSelMotnPart = 0;
			
        }

        private void FrmMManual_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }

        private void FrmMManual_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible && !actived) 
            {//Show
                FRM.ShowFormParent    (FRM.SManual,this.pnBaseMan );
                //this.sgSelPart.CurrentCell = this.sgSelPart[0, m_iSelMotnPart];
                FormShow();
                tmProc.Enabled = true;  
            }
            if(!this.Visible && actived) 
            {//Hide
              tmProc.Enabled = false;  
            }
            actived = this.Visible;
        }

        public void FormShow()
        {
			cDEF.POSN.DisplayPart (m_iSelMotnPart, ref sgSelPart);
			sgSelPart.ClearSelection();
			sgSelPart.Rows[m_iSelMotnPart].Cells[0].Selected = true;
			

			FormUpdate(m_iSelMotnPart);
        }
        public void FormHide()
        {
            FRM.HideFormParent(FRM.SManual);
        }
        public void FormSave()
        {


        }

        public void FormUpdate(int SelPage)
        {
            cDEF.POSN.DisplayMotor(ref sgMotor, SelPage);
            FRM.SManual._iRqPartSel = (EN_PART_SEL)SelPage;
            cDEF.EPU.GetPicturePart(SelPage+1, ref imgPart);
            cDEF.POSN.DisplayPos(ref sgMotor, cDEF.POSN._iSelPart);

			m_iSelMotnPart = SelPage;
        }

        private void tmProc_Tick(object sender, EventArgs e)
        {
            //if(!this.Visible) {this.tmProc.Enabled = false; return; }
            tmProc.Enabled = false;
            //FormUpdate();
            tmProc.Enabled = true ;
        }

        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

			FormUpdate(iGridR);
        }

        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            FormSave();
        }
        private void sgMotor_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            if(CurrGrid.CurrentCell == null) return;

            int iGridR = e.RowIndex   ; 
            int iGridC = e.ColumnIndex;

            if(iGridR<0) return;
            String sCellDat = CurrGrid[0,iGridR].Value.ToString().Trim().ToUpper();
            String sMotor   = sCellDat.Substring(1,2);
            int    iMotr    = Convert.ToInt32(sMotor);
            int    iManNo;

            if(iGridC != 2 &&  iGridC != 3) return;
            if(iMotr<0 || iMotr>=cDEF.MOTR._iNumOfMotr) return;
            iManNo = cDEF.MOTR.ManNoJog((EN_MOTR_ID)iMotr); 

            if(iGridC == 2) cDEF.MAN.ManProcOn (iManNo     , true , false );
            if(iGridC == 3) cDEF.MAN.ManProcOff(iManNo     , false , true );
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

            cDEF.MOTR.Stop((EN_MOTR_ID)iMotr);
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
