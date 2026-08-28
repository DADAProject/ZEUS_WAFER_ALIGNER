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
    public partial class FrmCtrlMC : Form
    {
        bool actived         ;
        public FrmCtrlMC(int Type)
        {
            InitializeComponent();
            //
            this.BackColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(210,210,208) : System.Drawing.Color.FromArgb(66 , 72 , 88 );
            //
            this.Width  = (Type == 0) ? 154 : 416;
            this.Height = (Type == 0) ? 256 : 88 ;
            this.panel1.Dock  = (Type == 0) ? DockStyle.Top : DockStyle.Left;
            this.panel2.Dock  = (Type == 0) ? DockStyle.Top : DockStyle.Left;
            this.panel3.Dock  = (Type == 0) ? DockStyle.Top : DockStyle.Left;

            this.panel1.Height = (Type == 0) ? 83  : 82;
            this.panel2.Height = (Type == 0) ? 83  : 82;
            this.panel3.Height = (Type == 0) ? 83  : 82;
            this.panel1.Width  = (Type == 0) ? 148 : 137;
            this.panel2.Width  = (Type == 0) ? 148 : 137;
            this.panel3.Width  = (Type == 0) ? 148 : 137;

            tmProc.Enabled = true;
        }

        private void kToggleButton1_Click(object sender, EventArgs e)
        {
            KToggleButton Btn = (sender as KToggleButton);
            int iTag = Convert.ToInt32(Btn.Tag); 

            switch  (iTag)
            {
                case 1: cDEF.SEQ._bBtnManStart = true;                    break;
                case 2: cDEF.SEQ._bBtnManStop  = true;                    break;
                case 3: cDEF.SEQ._bBtnManReset = !cDEF.SEQ._bBtnManReset; break;  
            }
        }

        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }

            tmProc.Enabled = false;
            bool bAutoMode = cDEF.SEQ._bAutoMode || cDEF.FM.IsDryMode();
            //
            btnStart.Enabled = !cDEF.SEQ._bRun && cDEF.LOT._bLotOpen && bAutoMode ? true : false;
		  //btnStop	.Enabled =  cDEF.SEQ._bRun                       ? true : false;
			btnReset.Enabled = !cDEF.SEQ._bRun                       ? true : false;

            tmProc.Enabled = true;
        }

        private void FormCtrlMC_Load(object sender, EventArgs e)
        {
            btnStart.Height = this.Height / 3;
            btnStop .Height = this.Height / 3;
            btnReset.Height = this.Height / 3;
            //
            tmProc.Enabled = true;
        }

        private void FormCtrlMC_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }

        private void FormCtrlMC_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible && !actived) 
            {//Show
                //
                tmProc.Enabled = true;
			
            }
            if(!this.Visible && actived) 
            {//Hide
                tmProc.Enabled = false;  
            }
            actived = this.Visible;
        }
    }
}
