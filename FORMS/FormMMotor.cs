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
    public partial class FrmMMotor : Form
    {
        //
        FrmCtrlMC FrmCtlBtn = new FrmCtrlMC(0);
        //	
        bool               actived       ; 
        int                m_iSelMotrPart;
        public EN_PART_SEL m_iRqPartSel  ;

        public FrmMMotor()
        {
            InitializeComponent();
            //
            Panel           pn;
            RadioButton     rb;
            Grouper         gb;
            Control[]       ctls;
            //
            this  .BackColor = FRM.GetBaseColor();            
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType().Name.ToLower() == "panel")
                {
                    pn = ctl as Panel;
                    if (pn.Name != "pnMotrBase")
                        pn.BackColor = FRM.GetGridBackColor();
                    //
                    ctls = FNC.GetAllControlsUsingRecursive(pn);
                    foreach (Control ctl1 in ctls)
                    {
                        if (ctl1.GetType().Name.ToLower() == "radiobutton")
                        {
                            rb = ctl1 as RadioButton;
                            rb.ForeColor = FRM.GetForeColor();
                        }
                        else if (ctl1.GetType().Name.ToLower() == "grouper")
                        {
                            gb = ctl1 as Grouper;
                            gb.BackgroundColor = FRM.GetBaseColor();
                            //gb.CustomGroupBoxColor = Color.LightGray;
                            //gb.PaintGroupBox = true;
                        }
                    }
                }
            }
        }
        //--------------------------------------------------------------------------
        private void FrmMMotor_Load(object sender, EventArgs e)
        {
            FRM.SetFormParent(FRM.SMotor  ,this.pnBaseMotr );
            FRM.SetFormParent(FrmCtlBtn   ,this.pnHandle   );
            m_iSelMotrPart = 0;
            m_iRqPartSel   = EN_PART_SEL.None;
            sgSelPart .Dock = System.Windows.Forms.DockStyle.Fill; 
        }
        //--------------------------------------------------------------------------
        private void FrmMMotor_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }
        //--------------------------------------------------------------------------
        private void FrmMMotor_VisibleChanged(object sender, EventArgs e)
        {


            if(this.Visible && !actived) 
            {//Show
                FRM.ShowFormParent(FRM.SMotor,this.pnBaseMotr);
                FRM.ShowFormParent(FrmCtlBtn ,this.pnHandle  );
                FormShow();

                tmProc.Enabled = true;
            }
            if(!this.Visible && actived) 
            {//Hide
                FRM.HideFormParent(FrmCtlBtn    );
                tmProc.Enabled = false;
            }
            actived = this.Visible;
          
        }
        //--------------------------------------------------------------------------
        public void FormShow()
        {
            //
            btnSave.Visible = true;
            cDEF.POSN.DisplayPart(m_iSelMotrPart, ref sgSelPart, FRM.GetGridBackColor(), true); 
            sgSelPart.CurrentCell = sgSelPart.Rows[m_iSelMotrPart].Cells[0];
            m_iRqPartSel = (EN_PART_SEL)m_iSelMotrPart;
            
            if(cDEF.FM.EngrOptn.iSpeedRatio == 9) rbSpdRato1 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 8) rbSpdRato2 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 7) rbSpdRato3 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 6) rbSpdRato4 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 5) rbSpdRato5 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 4) rbSpdRato6 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 3) rbSpdRato7 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 2) rbSpdRato8 .Checked = true;
            if(cDEF.FM.EngrOptn.iSpeedRatio == 1) rbSpdRato9 .Checked = true;
                                                  rbSpdRato10.Checked = true;
        }
        //------------------------------------------------------------------------
        public void FormHide()
        {
            FRM.HideFormParent(FRM.SMotor);
            FRM.HideFormParent(FrmCtlBtn );
        }
        //------------------------------------------------------------------------
        public void FormSave()
        {
            cDEF.POSN.UpdateSpdByGrid(false, m_iSelMotrPart-1, ref sgMotorSpd);    //All포함으로 -1시킴
            cDEF.MOTR.Load           (false , cDEF.FM._sCrntDevice , (EN_SEQ_ID)m_iSelMotrPart-1); //All포함으로 -1시킴
            cDEF.MOTR.SetAxis        ();

            cDEF.FM.EngrOptn.iSpeedRatio = 0;
            if(rbSpdRato1.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 9;
            if(rbSpdRato2.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 8;
            if(rbSpdRato3.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 7;
            if(rbSpdRato4.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 6;
            if(rbSpdRato5.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 5;
            if(rbSpdRato6.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 4;
            if(rbSpdRato7.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 3;
            if(rbSpdRato8.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 2;
            if(rbSpdRato9.Checked) cDEF.FM.EngrOptn.iSpeedRatio = 1;
        }

        public void FormUpdate()
        {
            if(m_iRqPartSel != EN_PART_SEL.None)
            {
                m_iSelMotrPart = (int)m_iRqPartSel ;
	            cDEF.POSN.UpdateSpdByGrid  (true, m_iSelMotrPart-1, ref sgMotorSpd); //All포함으로 -1시킴
                FRM.SMotor.m_iRqPartSel = (EN_PART_SEL)m_iSelMotrPart-1; //All포함으로 -1시킴
                m_iRqPartSel = EN_PART_SEL.None;
            }


        }

        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }
			//
            tmProc.Enabled = false;

            FormUpdate();
			//
            tmProc.Enabled = true ;
        }

        private void sgSelPart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;
            int iGridR = CurrGrid.CurrentCell.RowIndex   ; 
            int iGridC = CurrGrid.CurrentCell.ColumnIndex;

            m_iRqPartSel = (EN_PART_SEL) iGridR;
        }

        private void btnDefaultLoad_Click(object sender, EventArgs e)
        {
            cDEF.MOTR.LoadCommSpd (true  , (EN_SEQ_ID)m_iSelMotrPart-1); //All포함으로 -1시킴
        }

        private void btnDefaultSave_Click(object sender, EventArgs e)
        {
            cDEF.MOTR.LoadCommSpd (false , (EN_SEQ_ID)m_iSelMotrPart-1); //All포함으로 -1시킴
        }

        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            if (!FRM.ShowMsg(true, "Confirm", "Do you want to save the parameter?", EN_MSG_KIND.UserModal)) return;

            FormSave();
        }
    }
}
