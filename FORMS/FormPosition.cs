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
    public partial class FormPosition : Form
    {
        FrmCtrlMotr         CtrlMotrFrm;
        Timer               m_TimerFI   = new Timer();
        //
        int                 m_iSelMotnPart;    
        public EN_PART_SEL  m_iRqPartSel  ;

        public FormPosition(int selPart)
        {
            InitializeComponent();
            //
            this.Opacity = 0;  //first the opacity is 0
            //
            CtrlMotrFrm = new FrmCtrlMotr();
            m_iSelMotnPart = selPart;
        }
        protected override void WndProc(ref Message m)
        {
            //if (!this.Visible) return;
            switch (m.Msg)
            {
                case vDEF.WM_CTRL_MOTR:
                    RefreshDispPos();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        private void FormPosition_Load(object sender, EventArgs e)
        {
            //Fade In.            
            m_TimerFI.Interval = 10; //we'll increase the opacity every 10ms
            m_TimerFI.Tick += new EventHandler(FadeInForm); //this calls the function that changes opacity
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();
            //
            pbImg.Dock = System.Windows.Forms.DockStyle.Fill;
            sgPos.Dock = System.Windows.Forms.DockStyle.Fill;
            FRM.SetFormParent(CtrlMotrFrm ,this.pnMotr);

            FormShow();
        }

        private void FormPosition_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) 
            {//Show
                FRM.ShowFormParent(CtrlMotrFrm, this.pnMotr);
                m_iRqPartSel = (EN_PART_SEL)m_iSelMotnPart;
                cDEF.POSN._iSelPart  = m_iSelMotnPart;
                CtrlMotrFrm ._iRqPartSel = (EN_PART_SEL)m_iSelMotnPart;
                this.Text = cDEF.POSN.GetPartName(cDEF.POSN._iSelPart);
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
            }
        }
        private void FormShow()
        {
            //pbImg.Load(@"");
            //pbImg.SizeMode = PictureBoxSizeMode.StretchImage;
            //
            cDEF.POSN.DisplayItem(ref sgPos, FRM.GetGridBackColor(), true);
			CtrlMotrFrm._iRqPartSel = (EN_PART_SEL)m_iSelMotnPart;
        }
		public void RefreshDispPos()
		{
			//
            cDEF.POSN._iSelPart  = m_iSelMotnPart;
			cDEF.POSN.DisplayItem(ref sgPos,FRM.GetGridBackColor(), true);
		}
        private void FormPosition_FormClosed(object sender, FormClosedEventArgs e)
        {
	        //cDEF.POSN.DisplayItem(ref sgPos, false);
            //cDEF.MOTR.Load(false , cDEF.FM._sCrntDevice, (EN_SEQ_ID)m_iSelMotnPart);
            FRM.HideFormParent(CtrlMotrFrm);
            timer1.Enabled = false;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            timer1.Enabled = true ;
        }
        private void button1_Click(object sender, EventArgs e)
        {
	        cDEF.POSN.DisplayItem(ref sgPos, FRM.GetGridBackColor(), false);
            cDEF.MOTR.Load(false , cDEF.FM._sCrntDevice, (EN_SEQ_ID)m_iSelMotnPart);
        }

        private void FormPosition_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    //cancel the event so the form won't be closed

            m_TimerFI.Tick += new EventHandler(fadeOutForm);  //this calls the fade out function
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();

            if (this.Opacity == 0)  //if the form is completly transparent
                e.Cancel = false;   //resume the event - the program can be closed
        }
        private void FadeInForm(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)  
            {
                m_TimerFI.Stop();   //this stops the timer if the form is completely displayed
                m_TimerFI.Tick -= new EventHandler(FadeInForm); 
            }
            else
                this.Opacity += 0.05;
        }
        private void fadeOutForm(object sender, EventArgs e)
        {
            if (this.Opacity <= 0)     //check if opacity is 0
            {
                m_TimerFI.Stop();    //if it is, we stop the timer
                m_TimerFI.Tick -= new EventHandler(fadeOutForm);
                Close();   //and we try to close the form
            }
            else
                this.Opacity -= 0.05;
        }
    }
}
