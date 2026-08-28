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
    public partial class FrmMIO : Form
    {
        //
        FrmCtrlMC FrmCtlBtn = new FrmCtrlMC(0);
        //
        int                m_iIOPartSel ;
        bool               actived      ; 
        
        public FrmMIO()
        {
            InitializeComponent();
            //
            Panel           pn;
            //CheckedListBox  cb;
            //Control[]       ctls;
            //
            this  .BackColor = FRM.GetBaseColor();            
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "panel")
                {
                    pn = ctl as Panel;
                    pn.BackColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(210,210,208) : System.Drawing.Color.FromArgb(66 , 72 , 88 );
                }
            }

            //
            //btnAddressIO.Visible = cDEF.IO.IsIOEtherCatType() ? false : true; 
        }
        //------------------------------------------------------------------------
        private void FrmMIO_Load(object sender, EventArgs e)
        {
            //
            m_iIOPartSel = -1;

		    //cDEF.IO.UpdateByGrid(0 , m_iIOPartSel, ref sgInput );
		    //cDEF.IO.UpdateByGrid(1 , m_iIOPartSel, ref sgOutput);
			cDEF.POSN.DisplayPart(m_iIOPartSel, ref sgSelPart, FRM.GetGridBackColor(), true , true); 
            //
            FRM.SetFormParent(FrmCtlBtn ,this.pnHandle);
        }
        //------------------------------------------------------------------------
        private void FrmMIO_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }
        //------------------------------------------------------------------------
        private void FrmMIO_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible && !actived) 
            {//Show
                FRM.ShowFormParent(FrmCtlBtn ,this.pnHandle);
                tmProc.Enabled = true;
                FormShow();     
            }
            if(!this.Visible && actived) 
            {//Hide
                FRM.HideFormParent(FrmCtlBtn    );
                tmProc.Enabled = false;  
            }
            actived = this.Visible;
        }
        //------------------------------------------------------------------------
        public void FormShow()
        {
			sgSelPart.ClearSelection();
			if (m_iIOPartSel < 0) sgSelPart.Rows[0             ].Cells[0].Selected = true;
			else                  sgSelPart.Rows[m_iIOPartSel+1].Cells[0].Selected = true;
            //
			cDEF.IO.UpdateByGrid (0, m_iIOPartSel, ref sgInput , FRM.GetGridBackColor());
			cDEF.IO.UpdateByGrid (1, m_iIOPartSel, ref sgOutput, FRM.GetGridBackColor());
			//FormUpdate(m_iIOPartSel);
        }
        //------------------------------------------------------------------------
        public void FormHide()
        {
            FRM.HideFormParent(FrmCtlBtn    );
        }
        //------------------------------------------------------------------------
        public void FormSave()
        {
        }
        //------------------------------------------------------------------------
        public void FormUpdate(int SelPage)
        {
            //
			cDEF.IO.DisplayStatus(0, SelPage, ref sgInput );
			cDEF.IO.DisplayStatus(1, SelPage, ref sgOutput);
		}
        //------------------------------------------------------------------------
        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }
            tmProc.Enabled = false;

            FormUpdate(m_iIOPartSel);
            tmProc.Enabled = true ;
        }
        //------------------------------------------------------------------------
        private void sgOutput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             DataGridView CurrGrid = (sender as DataGridView);
            if(CurrGrid.CurrentCell == null) return;

            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            string sValue = (sender as DataGridView)[0, iGridR].Value.ToString();
            int iOutNo = Convert.ToInt32(sValue);

	        if(iGridC == 3) cDEF.IO.FrceOutput(iOutNo-1, true );
	        if(iGridC == 4) cDEF.IO.FrceOutput(iOutNo-1, false);  
        }
        //------------------------------------------------------------------------
        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

			m_iIOPartSel = iGridR - 1;

            //FormUpdate(m_iIOPartSel);
            cDEF.IO.UpdateByGrid(0, m_iIOPartSel, ref sgInput , FRM.GetGridBackColor());
            cDEF.IO.UpdateByGrid(1, m_iIOPartSel, ref sgOutput, FRM.GetGridBackColor());
        }
        //------------------------------------------------------------------------
        private void btnAddressIO_Click(object sender, EventArgs e)
        {
            //Check Running Status.
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("The parameter can not be changed while the Machine is running.");
                return;
                }
	        //Create Form.
	        cDEF.IO.ShowFrmAddress();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
        }

        private void sgOutput_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dg = sender as DataGridView;
            dg.ClearSelection();
        }
        //--------------------------------------------------------------------------
        private void btIOSet_Click(object sender, EventArgs e)
        {
            //
            FormMIOSetting IOSET = new FormMIOSetting();
            IOSET.ShowDialog();

        }
    }
}
