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
    public partial class FrmAlarm : Form
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //public:    /* Direct Accessable Vars.  */
        public int m_iToolId     ;
        public int m_iBtmKind    ;
        public int m_iSubFormSel ;
        public int m_iSelFormKind;

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public FrmAlarm()
        {
            InitializeComponent();
        }

        private void FormAlarm_Load(object sender, EventArgs e)
        {
            //TabControl Tab 제거
            this.Height -= 20;
            Rectangle Rect = new Rectangle(tabErrSub1.Left, tabErrSub1.Top, tabErrSub1.Width, tabErrSub1.Height);
            tabPage3.Region = new Region(Rect);   

	        ErrorDisplay();
            SubDisplay  ();
            tmProc.Enabled = true;   
        }

        private void FrmAlarm_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }

        private void lbTitle_MouseDown(object sender, MouseEventArgs e)
        {
             var s = sender as Label;
             if(this  == null) return;
             if(s     == null) return;
             s.Tag = new Point(e.X, e.Y);
        }

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

        private void tmProc_Tick(object sender, EventArgs e)
        {
             if(!this.Visible) {this.tmProc.Enabled = false; return; }
        }

	    public void ErrorDisplay()
        {
	        String     Str  ;
            int        iErrNo = -1;
	        lBoxError.Items.Clear();
	        for (int i = 0 ; i < vDEF.MAX_ERR ; i++) {
		        if (cDEF.EPU[i].m_bOn && (cDEF.EPU[i].IsGradeErr())) {
    			    Str = String.Format("[ERR{0,4:0000}] {1}", i, cDEF.EPU.GetName(i));      
                    if(iErrNo < 0) iErrNo = i;
			        lBoxError.Items.Add(Str);
			        }
		        }
            if(iErrNo<0)
            {
                return;
            }
                 
	        lbTitle.Text = "           " + String.Format("[ERR{0,4:0000}] {1}", iErrNo, cDEF.EPU.GetName(iErrNo));  
            rtErrCause.Text    =  cDEF.EPU.GetCause   (iErrNo);
            rtErrSolution.Text =  cDEF.EPU.GetSolution(iErrNo);

            //Error Picture.
            cDEF.EPU.GetPicture(iErrNo , ref imgError);

        }
        //------------------------------------------------------------------------
        public void SubDisplay  ()
        {
            switch (m_iSubFormSel) 
            {
                default: //Normal
                         lbSub1Msg   .Text = string.Format($"If click [{button4.Text.Trim()}] Button, Jam becomes Clear.");
				         break;
                
                case  1: //Knife
                    
                    if (m_iSelFormKind == 1)
                    {
                        lbSub2Msg.Text = "Knife 사용 후 작업을 진행하시겠습니까?";
                    }
                    break;

                case  2: //Lot End
                    if (m_iSelFormKind == 0)
                    {
                       lbSub2Msg.Text = "Request Lot End - 작업을 종료하시겠습니까?";
                    }

                    btnErrSub2_1.Text = "Confirm";
                    btnErrSub2_2.Text = "Cancel";
                    
                    break;

                case 3: //Wafer 사라짐
                    if      (m_iSelFormKind == 0 ) lbSubMsg3.Text = "Transfer A Wafer가 사라졌습니다.";
                    else if (m_iSelFormKind == 1 ) lbSubMsg3.Text = "Transfer B Wafer가 사라졌습니다.";
                    else if (m_iSelFormKind == 2 ) lbSubMsg3.Text = "Align Table Wafer가 사라졌습니다.";
                    else if (m_iSelFormKind == 3 ) lbSubMsg3.Text = "Wafer Table Wafer가 사라졌습니다.";
                    
                    else if (m_iSelFormKind == 10) lbSubMsg3.Text = "Supply Tape가 없습니다.";
                    else if (m_iSelFormKind == 11) lbSubMsg3.Text = "Used Tape가 가득 찼습니다.";
                    else if (m_iSelFormKind == 12) lbSubMsg3.Text = "Protection Tape가 가득 찼습니다.";
                    
                    else lbSubMsg3.Text = "";

                    //
                    if (m_iSelFormKind < 10) btUnknownAct1.Visible = true ; 
                    else                     btUnknownAct1.Visible = false; 

                    break;
                
                case 4: 

                    break;
            }
            
            //Page Index
            tabPage3.SelectedIndex = m_iSubFormSel;
        }
        //------------------------------------------------------------------------
        private void btnSub1Reset_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            cDEF.LOG.Trace($"ALARM {Btn.Text} Button Click");
            string sBtn = Btn.Text; 

            switch (tabPage3.SelectedIndex) 
            {
                case 0: break;
                case 1:
                        if (m_iSelFormKind == 0) //알 수 없는 Bolt 발견
                        {
                            //cDEF.DM.TOOL[(int)m_iToolId][0].SetTo(EN_UNIT_STAT.Mount); 
                           
                        }
                        else if (m_iSelFormKind == 1) //Bolt 사라짐
                        {
                            //cDEF.DM.TOOL[(int)m_iToolId][0].SetTo(EN_UNIT_STAT.Empty);
                        }
                        break;

            }      
            //
	        cDEF.LampBuzz.BuzzOff();
            if (sBtn == "Reset")
            {
                cDEF.SEQ.Reset();
                if (cDEF.SEQ._bAutoMode) cDEF.SEQ._bBtnManStart = true; 
            }
            this.Close();
        }
        //------------------------------------------------------------------------
        private void ResetClose()
        {
            //
            cDEF.SEQ.Reset();
            cDEF.LampBuzz.BuzzOff();
            //
            this.Close();
        }
        //------------------------------------------------------------------------
        private void btnSub1Close_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            cDEF.LOG.Trace($"ALARM {Btn.Text} Button Click");

            switch(tabPage3.SelectedIndex) 
            {
                case 0: cDEF.SEQ.Reset();
                        break;

                case 1:
                        cDEF.SEQ.Reset();
                        break;
                case 2:
                        cDEF.SEQ.Reset();
                        break;

            }  

	        Close   ();
        }
        //------------------------------------------------------------------------
        private void btnErrSub2_1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            cDEF.LOG.Trace($"ALARM {Btn.Text} Button Click");   
            
            cDEF.SEQ.Reset();
            cDEF.SEQ.Reset();

            if (m_iSelFormKind == 0)
            {
                cDEF.LOT._bReqLotEnd = true;
                cDEF.SEQ._bBtnManStart = true;
            }
            else if (m_iSelFormKind == 1)
            {
                //
                //cDEF.SEQ.WLM._bChkSkipKnife = true; 
            }

            Close();

        }
        //------------------------------------------------------------------------
        private void btnErrSub3_1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            //

            //
            cDEF.SEQ.Reset();
            Close();
        }
        //------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            cDEF.LOG.Trace($"ALARM {Btn.Text} Button Click");   
            int iSelBtn = Convert.ToInt32(Btn.Tag);
            
            switch (iSelBtn)
            {
                case 0: //Work End
                        LOT._bReqLotEnd = true; 
                        break;
                
                case 1: //SKIP
                        break;
                
                case 2: cDEF.SEQ.Reset();
                        break;
            }
            //
            Close();
        }
        //------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            LOG.Trace($"ALARM {Btn.Text} Button Click");   
            
            int iSelBtn = Convert.ToInt32(Btn.Tag);
            //
            switch (iSelBtn)
            {
                case 1: //Skip

                    //if      (m_iSelFormKind == 10) SEQ.WLM._bSkipChkSplyEmpty = true;
                    //else if (m_iSelFormKind == 11) SEQ.WLM._bSkipChkUsed1Full = true;
                    //else if (m_iSelFormKind == 12) SEQ.WLM._bSkipChkUsed2Full = true;

                    break;

                case 2: cDEF.SEQ.Reset();
                        break;
            }
            //
            Close();
        }
        //------------------------------------------------------------------------
        private void btUnknownAct1_Click(object sender, EventArgs e)
        {
            //
            //switch (m_iSelFormKind)
            //{
            //    case 0: cDEF.DM.WAF[(int)EN_WAF_ID.WTR_A].SetTo(EN_WAFER_STAT.Empty); break;
            //    case 1: cDEF.DM.WAF[(int)EN_WAF_ID.WTR_B].SetTo(EN_WAFER_STAT.Empty); break;
            //    case 2: cDEF.DM.WAF[(int)EN_WAF_ID.WAT  ].SetTo(EN_WAFER_STAT.Empty); break;
            //    case 3: cDEF.DM.WAF[(int)EN_WAF_ID.WTB  ].SetTo(EN_WAFER_STAT.Empty); break;
            //}
            //
            ResetClose();

        }
    }
}
