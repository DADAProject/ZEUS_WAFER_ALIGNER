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
    public partial class FrmChangeOperID : Form
    {
        Timer      m_TimerFI   = new Timer();

        public FrmChangeOperID()
        {
            InitializeComponent();

        }
        private void FormChangeOperID_Load(object sender, EventArgs e)
        {
            textBox1.Text = cDEF.FM.m_sCrntOperID;
            textBox1.Focus();

            //this.pnOperID.MouseDown += new MouseEventHandler(pnMouse_MouseDown);
            //this.pnOperID.MouseMove += new MouseEventHandler(pnMouse_MouseMove);

            //Fade In.            
            m_TimerFI.Interval = 10; //we'll increase the opacity every 10ms
            m_TimerFI.Tick += new EventHandler(FadeInForm); //this calls the function that changes opacity
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();
        }
        private void pnMouse_MouseDown(object sender, MouseEventArgs e)
        {
             var s = sender as RoundPanel;
             if(this == null) return;
             if(s    == null) return;
             s.Tag = new Point(e.X, e.Y);
        }
        private void pnMouse_MouseMove(object sender, MouseEventArgs e)
        {
            var s = sender as RoundPanel;
            if(this  == null) return;
            if(s     == null) return;
            if(s.Tag == null) return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
            this.Left = this.Left + (e.X - ((Point)s.Tag).X);
            this.Top  = this.Top  + (e.Y - ((Point)s.Tag).Y);
        }


        private void kToggleButton1_Click(object sender, EventArgs e)
        {
            cDEF.FM.m_sCrntOperID = textBox1.Text.Trim();
            cDEF.LOG.SeqTrace($"ID Change : {cDEF.FM.m_sCrntOperID}");
            Close();
        }

        private void kToggleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FadeInForm(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)  
            {
                m_TimerFI.Stop();   //this stops the timer if the form is completely displayed
                m_TimerFI.Tick -= new EventHandler(FadeInForm); 
            }
            else
                this.Opacity += 0.1;
        }
        //--------------------------------------------------------------------------
        private void fadeOutForm(object sender, EventArgs e)
        {
            if (this.Opacity <= 0)     //check if opacity is 0
            {
                m_TimerFI.Stop();    //if it is, we stop the timer
                m_TimerFI.Tick -= new EventHandler(fadeOutForm);
                Close();   //and we try to close the form
            }
            else
                this.Opacity -= 0.2;
        }

        private void FrmChangeOperID_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    //cancel the event so the form won't be closed

            m_TimerFI.Tick += new EventHandler(fadeOutForm);  //this calls the fade out function
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();

            if (this.Opacity == 0)  //if the form is completly transparent
                e.Cancel = false;   //resume the event - the program can be closed
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
             var s = sender as Label;
             if(this == null) return;
             if(s    == null) return;
             s.Tag = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            var s = sender as Label;
            if(this  == null) return;
            if(s     == null) return;
            if(s.Tag == null) return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
            this.Left = this.Left + (e.X - ((Point)s.Tag).X);
            this.Top  = this.Top  + (e.Y - ((Point)s.Tag).Y);
        }
    }
}
