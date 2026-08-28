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
    public partial class FrmLot2 : Form
    {
        Timer      m_TimerFI   = new Timer();

        //
        public FrmLot2()
        {
            InitializeComponent();
            //
            this.Opacity = 0;  //first the opacity is 0
            //
            String sJobPath = Application.StartupPath + "\\Project";

            FNC.UpdateDirByGrid(sJobPath, ref sgJobList, FRM.GetGridBackColor(), false, true);

            string sTemp = string.Empty;
            //Current Cell Display
            for (int r = 0; r < sgJobList.RowCount; r++)
            {
                sTemp = sgJobList[1, r].Value.ToString(); 
                if(cDEF.FM._sCrntDevice == sTemp)
                {
                    sgJobList[1, r].Selected = true;  //JUNG/220405/요청사항
                    
                    edDevice.Text = sgJobList[1, r].Value.ToString();
                    break;
                }
            }

            //Default Data input
            edLotNo.Text  = DateTime.Now.ToString(); //cDEF.LOT.Info.sLotNo1; //
            edPartNo.Text = "_";                     //cDEF.LOT.Info.sPartNo; //
            edOperId.Text = "Lami";
        }
        //------------------------------------------------------------------------
        private void FrmLot_Load(object sender, EventArgs e)
        {
            //Fade In.            
            m_TimerFI.Interval = 10; //we'll increase the opacity every 10ms
            m_TimerFI.Tick += new EventHandler(FadeInForm); //this calls the function that changes opacity
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();
        }
        //------------------------------------------------------------------------
        private void FrmLot_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    //cancel the event so the form won't be closed

            m_TimerFI.Tick += new EventHandler(fadeOutForm);  //this calls the fade out function
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();

            if (this.Opacity == 0)  //if the form is completly transparent
                e.Cancel = false;   //resume the event - the program can be closed
        }
        //------------------------------------------------------------------------
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
            else this.Opacity -= 0.2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TLOT_INFO LotInfo = new TLOT_INFO();

          //if (cDEF.LOT._bLotOpen          ) { FRM.ShowWarn(true,"Lot has already Opened."          ); return; }
            if (cDEF.LOT._bLotOpen          ) //JUNG/220317/KEC 요청으로 자동 삭제 후 Lot Open
            {
                UserLotEnd();
            }

            if (edLotNo  .Text == ""        ) { FRM.ShowWarn(true,"Please enter Lot No."             ); return; }
            //if (edLotNo  .Text.Length  > 14 ) { FRM.ShowWarn(true,"Lot No Digits Over! (14 digits)"  ); return; }
            //if (edPartNo .Text.Length  > 29 ) { FRM.ShowWarn(true,"Part No Digits Over! (29 digits)" ); return; }
            if (edDevice .Text == ""        ) { FRM.ShowWarn(true,"Please select a recipe."          ); return; }
            if (edOperId .Text == ""        ) { FRM.ShowWarn(true,"Please enter your Operator ID. "  ); return; }
            //                
            LotInfo.sLotNo1   = edLotNo .Text;
            LotInfo.sPartNo   = edPartNo.Text;
            LotInfo.sOperator = edOperId.Text;
            LotInfo.sJobFile = edDevice .Text;             
            //
            if (!cDEF.LOT.LotOpen(LotInfo))
            {
                FRM.ShowWarn(true, "Lot Open Failed");
                return;
            }

            cDEF.FM.m_sCrntOperID = LotInfo.sOperator ;
            //
            Close();
        }

        private void sgLotList_SelectionChanged(object sender, EventArgs e)
        {
            if (sgJobList.CurrentCell == null) return;

            int iGridR = sgJobList.CurrentCell.RowIndex;
            //int iGridC = sgJobList.CurrentCell.ColumnIndex;
            edDevice.Text = sgJobList[1, iGridR].Value.ToString();
        }
        //------------------------------------------------------------------------
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
        //------------------------------------------------------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);            
            
            if(cDEF.SEQ.IsWorkEnd()) { FRM.ShowWarn(true,"The operation has not been completed." ); return; } 
            cDEF.LOG.Trace($"LOT FORM {Btn.Text} Button Click");

            if (!FRM.ShowMsg(true, " Warning ", "Do you want to proceed with Lot Cancel?", EN_MSG_KIND.UserModal)) return;
            //
            cDEF.LOT.LotCancel();
        }
        //------------------------------------------------------------------------
        private void UserLotEnd()
        {
            if (!cDEF.LOT.LotEnd())
            {
                FRM.ShowWarn(true, "Lot Force Work End Failed.");
                return;
            }
        }
    }
}
