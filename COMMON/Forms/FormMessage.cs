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
    public partial class FrmMessage : Form
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //public:    /* Direct Accessable Vars.  */

        public String m_sTitle, m_sMsg;
        public int m_iKind = 0;

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public FrmMessage()
        {
            InitializeComponent();
        }
        //--------------------------------------------------------------------------
        private void FrmMessage_Load(object sender, EventArgs e)
        {
            lbTitle.Text = m_sTitle;
            lbMsg.Text = m_sMsg;
            this.Left = 300;
            this.Top    = 300;

            btnYes.Text = "YES";
            btnNo.Text = "NO";
            btnYes.Visible = true;

            if(m_iKind == 100)
            {
                btnYes.Visible = false;
                btnNo.Text = "CLOSE";
   
            }
        }
        //--------------------------------------------------------------------------
        private void FrmMessage_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        //--------------------------------------------------------------------------
        private void lbTitle_MouseDown(object sender, MouseEventArgs e)
        {
             var s = sender as Label;
             if(this  == null) return;
             if(s     == null) return;
             s.Tag = new Point(e.X, e.Y);
        }
        //--------------------------------------------------------------------------
        private void lbTitle_MouseMove(object sender, MouseEventArgs e)
        {
            var s = sender as Label;

            if(this  == null) return;
            if(s     == null) return;
            if(s.Tag == null) return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
            this.Left = this.Left + (e.X - ((Point)s.Tag).X);
            this.Top  = this.Top  + (e.Y - ((Point)s.Tag).Y);
        }
        //--------------------------------------------------------------------------
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)EN_MSG_KIND.UserShow:
                    this.Visible = true;
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //--------------------------------------------------------------------------
        private void btnNo_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
