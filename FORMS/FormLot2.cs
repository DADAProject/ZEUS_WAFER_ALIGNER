using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static eMachine.cDEF; 

namespace eMachine
{
    public partial class FrmLot : Form
    {
        Timer      m_TimerFI   = new Timer();

        //
        public FrmLot()
        {
            InitializeComponent();
            //
            this.Opacity = 0;  //first the opacity is 0 
            //
            edDevice.Text = cDEF.FM._sCrntDevice;
            edLotNo1.Text = "";
            edLotNo2.Text = "";
        }
        private void FrmLot_Load(object sender, EventArgs e)
        {
            //Fade In.            
            m_TimerFI.Interval = 10; //we'll increase the opacity every 10ms
            m_TimerFI.Tick += new EventHandler(FadeInForm); //this calls the function that changes opacity
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();
        }

        private void FrmLot_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    //cancel the event so the form won't be closed

            m_TimerFI.Tick += new EventHandler(fadeOutForm);  //this calls the fade out function
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();

            if (this.Opacity == 0)  //if the form is completely transparent
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
                this.Opacity += 0.2;
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

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TLOT_INFO LotInfo = new TLOT_INFO();

            if (cDEF.LOT ._bLotOpen         ) { FRM.ShowWarn(true,"Lot has already Opened."          ); return; }                                                                                                           
            if (edLotNo1 .Text == ""        ) { FRM.ShowWarn(true,"Please enter Lot No."             ); return; }
          //if (edLotNo1 .Text.Length  > 14 ) { FRM.ShowWarn(true,"Lot No Digits Over! (14 digits)"  ); return; }
          //if (edPartNo .Text.Length  > 29 ) { FRM.ShowWarn(true,"Part No Digits Over! (29 digits)" ); return; }
            if (edDevice .Text == ""        ) { FRM.ShowWarn(true,"Please select a recipe."          ); return; }
            if (edOperId .Text == ""        ) { FRM.ShowWarn(true,"Please enter your Operator ID. "  ); return; }
            //                
            LotInfo.sLotNo1      = edLotNo1.Text.ToUpper();
            LotInfo.sLotNo2      = edLotNo2.Text.ToUpper();
            LotInfo.sPartNo      = edPartNo.Text.ToUpper();
            LotInfo.sOperator    = edOperId.Text.ToUpper();
            LotInfo.sJobFile     = edDevice.Text.ToUpper();     
            //
            if (!cDEF.LOT.LotOpen(LotInfo))
            {
                FRM.ShowWarn(true, "Lot Open Failed");
                return;
            }
            //
            cDEF.FM.m_sCrntOperID = LotInfo.sOperator ;
            //
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);            
            
            if(cDEF.SEQ.IsWorkEnd()) { FRM.ShowWarn(true,"The operation has not been completed." ); return; } 
            cDEF.LOG.Trace($"LOT FORM {Btn.Text} Button Click");

            if (!FRM.ShowMsg(true, " Warning ", "Do you want to force quit a job in progress?", EN_MSG_KIND.UserModal)) return;

            if (!cDEF.LOT.LotEnd())
            {
                FRM.ShowWarn(true, "Lot Force Work End Failed.");
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);            
            
            if(cDEF.SEQ.IsWorkEnd()) { FRM.ShowWarn(true,"The operation has not been completed." ); return; } 
            cDEF.LOG.Trace($"LOT FORM {Btn.Text} Button Click");

            if (!FRM.ShowMsg(true, " Warning ", "Do you want to proceed with Lot Cancel?", EN_MSG_KIND.UserModal)) return;
            //
            cDEF.LOT.LotCancel();
        }
    }
}
