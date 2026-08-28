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
    public partial class FrmSMotor : Form
    {
        bool               actived         ;
        int                m_iSelMotr      ;
        public int         m_iSelMotrPart  ;
        public EN_PART_SEL m_iRqPartSel    ;
        bool               m_IsSelecting   ;
		bool               m_bDrngJog      ;
		Button             m_pDownBtn      ;

        public FrmSMotor()
        {
            InitializeComponent();
            //
            Panel           pn;
            Label           lb;
            TabPage         tp;
            Grouper         gb;
            Control[]       ctls;
            //
            this  .BackColor = FRM.GetBaseColor();     
            ctls = FNC.GetAllControlsUsingRecursive(this);       
            foreach (Control ctl in ctls)
            {
                if (ctl.GetType().Name.ToLower() == "panel")
                {
                    pn = ctl as Panel;
                    pn.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "label")
                {
                    lb = ctl as Label;
                    if (lb.Name != "lbWarning")
                        lb.ForeColor = FRM.GetForeColor();
                }
                else if (ctl.GetType().Name.ToLower() == "tabpage")
                {
                    tp = ctl as TabPage;
                    tp.BackColor = FRM.GetGridBackColor();
                }
                else if (ctl.GetType().Name.ToLower() == "grouper")
                {
                    gb = ctl as Grouper;
                    gb.BackgroundColor = FRM.GetGridBackColor();
                    //gb.CustomGroupBoxColor = Color.LightGray;
                    //gb.PaintGroupBox = true;
                }
            }

        }
        //------------------------------------------------------------------------
        private void FrmSMotor_Load(object sender, EventArgs e)
        {
            m_iSelMotr       = 0;
            m_iSelMotrPart   = -1;
            m_iRqPartSel     = EN_PART_SEL.None;

			m_bDrngJog       = false;
			m_pDownBtn       = null ;

            tmProc.Interval = 50;
        }

        private void FrmSMotor_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }

        private void FrmSMotor_VisibleChanged(object sender, EventArgs e)
        {
             if(this.Visible) 
             {//Show
                if(actived) 
                {
                    //if (!tmProc.Enabled) tmProc.Enabled = true;
                    return; 
                }
                FormShow();  
                tmProc.Enabled = true;
             }
             else
             {//Hide
                if(!actived) 
                {
                    //if (tmProc.Enabled) tmProc.Enabled = false;
                    return; 
                }
                tmProc.Enabled = false;
             }
             actived = Visible;
			 //
			 m_bDrngJog = false;
			 m_pDownBtn = null ;
        }
        //------------------------------------------------------------------------
        public void FormShow()
        {
            //cDEF.POSN.DisplayMotorStat(ref sgMotor,-1);
            if(m_iSelMotrPart == (int)EN_PART_SEL.All || m_iSelMotrPart == (int)EN_PART_SEL.None)   cDEF.POSN.DisplayMotorStat(ref sgMotor, (int)EN_PART_SEL.All);
            else                                                                                    cDEF.POSN.DisplayMotorStat(ref sgMotor, m_iSelMotrPart);
        }
        //------------------------------------------------------------------------
        public void FormSave()
        {


        }
        //------------------------------------------------------------------------
        public void FormUpdate()
        {
            try
            {
                if (m_iRqPartSel != EN_PART_SEL.None)
                {
                    m_iSelMotrPart = (int)m_iRqPartSel;
                    cDEF.POSN.DisplayMotorStat(ref sgMotor, (int)m_iRqPartSel);
                    m_iRqPartSel = EN_PART_SEL.None;
                }
                cDEF.POSN.UpdateMotorStat(ref sgMotor);
                lbMotrName.Text = cDEF.POSN.GetMotorName(m_iSelMotr);
                lbTrgPosn.Text = string.Format("{0:F4}", cDEF.MOTR[m_iSelMotr].GetTrgPos());
                lbCmdPosn.Text = string.Format("{0:F4}", cDEF.MOTR[m_iSelMotr].GetCmdPos());
                lbEncPosn.Text = string.Format("{0:F4}", cDEF.MOTR[m_iSelMotr].GetEncPos());
                btnMotrCtrl3.BackColor = (cDEF.MOTR[m_iSelMotr].GetServo()) ? Color.Lime : Color.Silver;

				textBox1.Text  = string.Format("{0}", cDEF.MOTR[m_iSelMotr]._dScanTime_MV);
				panel2.BackColor = cDEF.MOTR[m_iSelMotr].GetStop() ? Color.Gray : Color.Maroon;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("FormUpdate Exception:" + err.Message);
            }
        }
        //------------------------------------------------------------------------
        private void sgMotor_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            String sCellDat, sTmp;
            DataGridView CurrGrid = (sender as DataGridView);
            if (CurrGrid.CurrentCell == null) return;

            try
            {
                int iGridR = e.RowIndex;
                int iGridC = e.ColumnIndex;

                sCellDat = CurrGrid[0, iGridR].Value.ToString().Trim().ToUpper();
                m_iSelMotr = Convert.ToInt32(sCellDat);

                if (m_iSelMotr < 0) return;

                if (Control.ModifierKeys == Keys.Alt && e.Button == MouseButtons.Left)
                {
                    sTmp = string.Format("Do you want change {0} Motor Initial?", Enum.GetName(typeof(EN_MOTR_ID), m_iSelMotr));
                    if (!MsgBox.Confirm(sTmp)) return;
                    //Home 반전.
                    cDEF.MOTR[m_iSelMotr].SetHomeEnd(!cDEF.MOTR[m_iSelMotr].GetHomeEnd());
                    cDEF.LOG.Trace($"ADMIN MOTOR - Manual Home Flag CLICK - AXIS : {m_iSelMotr}");
                }
                if (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left)
                {
                    cDEF.MOTR.DispParamFrm((EN_MOTR_ID)m_iSelMotr);
                    cDEF.LOG.Trace($"ADMIN MOTOR - Motor Set CLICK - AXIS : {m_iSelMotr}");
                    //FormShow();
                }
            }
            catch (Exception err)
            {
                //
                System.Diagnostics.Debug.WriteLine("sgMotor_CellMouseDown Exception:" + err.Message);
            }
        }
		private void btnMotrCtrl5_Click(object sender, EventArgs e)
		{
            try
            {
                int        iTag    = Convert.ToInt32(m_pDownBtn.Tag);
                double     dTorque = Convert.ToDouble(edTorque.Text);
                double     dToqVel = Convert.ToDouble(edTorqueVel.Text);
                EN_COMD_ID iCmd    = EN_COMD_ID.NoneCmd;
                //
                if      (iTag == 5) iCmd = EN_COMD_ID.JogP;
                else if (iTag == 6) iCmd = EN_COMD_ID.JogN;
                //
                if (!cDEF.MOTR.CheckCrash((EN_MOTR_ID)m_iSelMotr, iCmd))
                {
					m_bDrngJog = false;
					m_pDownBtn = null;
                    FRM.ShowWarn(true, "The motor can not be moved because Motor is crash");
                    return;
                }
				//
				if (((iCmd == EN_COMD_ID.JogP) || (iCmd == EN_COMD_ID.JogP)) && (m_pDownBtn == null)) return;
				if (((iCmd == EN_COMD_ID.JogP) || (iCmd == EN_COMD_ID.JogP)) && (!m_bDrngJog       )) return; 
                //
                switch (iTag)
                {
                    default: break;
                    case 5:
                        cDEF.MOTR.MoveJog((EN_MOTR_ID)m_iSelMotr, true);
                        break;
                    case 6:
                        cDEF.MOTR.MoveJog((EN_MOTR_ID)m_iSelMotr, false);
                        break;
                    case 8:
                        if (!cDEF.MOTR[m_iSelMotr].m_bUseTorque) return;
                        cDEF.MOTR[m_iSelMotr].MoveTorqueP(dTorque, dToqVel); break;
                    case 9:
                        if (!cDEF.MOTR[m_iSelMotr].m_bUseTorque) return;
                        cDEF.MOTR[m_iSelMotr].MoveTorqueN(dTorque, dToqVel); break;
                }
                string temp = string.Format($"ADMIN MOTOR - {(sender as Button).Text} BUTTON CLICK - AXIS : {m_iSelMotr}");
                cDEF.LOG.Trace(temp);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
		}
        private void btnMotrCtrl5_MouseDown(object sender, MouseEventArgs e)
        {
			Button Btn = (sender as Button);
			int iTag = Convert.ToInt32(Btn.Tag);

			if ((iTag == 5) || (iTag == 6))
			{
				m_bDrngJog = true;
				m_pDownBtn = Btn;
			}
        }

        private void btnMotrCtrl5_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Button Btn = (sender as Button);
                int iTag = Convert.ToInt32(Btn.Tag);
                if (iTag == 8 || iTag == 9)
                {
                    if (!cDEF.MOTR[m_iSelMotr].m_bUseTorque) return;
                    cDEF.MOTR[m_iSelMotr].MoveTorqueStop();
                }
                else
                {
					m_bDrngJog = false;
					m_pDownBtn = null;
                    cDEF.MOTR.Stop((EN_MOTR_ID)m_iSelMotr);
                }
                string stemp = string.Format($"ADMIN MOTOR - {(sender as Button).Text} BUTTON CLICK - AXIS : {m_iSelMotr}");
                cDEF.LOG.Trace(stemp);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }
        private void sgMotor_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView CurrGrid = (sender as DataGridView);

                if (CurrGrid.CurrentCell == null) return;

                int iGridR = CurrGrid.CurrentCell.RowIndex;
                int iGridC = CurrGrid.CurrentCell.ColumnIndex;

                String sCellDat = CurrGrid[0, iGridR].Value.ToString().Trim().ToUpper();
                m_iSelMotr = Convert.ToInt32(sCellDat);

                if (m_iSelMotr < 0) m_iSelMotr = 0;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }

        private void btnMotrCtrl1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cDEF.SEQ._bRun)
                {
                    MsgBox.Warning("The Machine is running.");
                    return;
                }
                Button Btn = (sender as Button);
                int iTag = Convert.ToInt32(Btn.Tag);
                switch (iTag)
                {
                    default: break;
                    case 1:
                        cDEF.MOTR.SetAlarm((EN_MOTR_ID)m_iSelMotr, true); // Reset Alarm
                        break;
                    case 2:
                        cDEF.MOTR.ClearHomeEnd((EN_MOTR_ID)m_iSelMotr);
                        cDEF.MOTR.MoveHome((EN_MOTR_ID)m_iSelMotr);
                        break;
                    case 3:
                        cDEF.MOTR.SetServo((EN_MOTR_ID)m_iSelMotr, !cDEF.MOTR[m_iSelMotr].GetServo()); //Servo Off
                        break;
                    case 4:
                        cDEF.MOTR.Stop((EN_MOTR_ID)m_iSelMotr); //Emergency
                        break;
                    case 7:
                        cDEF.MOTR.ClearPos((EN_MOTR_ID)m_iSelMotr); //Clear Position.
                        break;
                    case 10:
                        if (!cDEF.MOTR.IsAllServoOn()) { MsgBox.Warning("Please All Motor Servo On"); return; }
                        cDEF.MAN.ManProcOn(1, true, false);
                        break;
                    case 11:
                        cDEF.MOTR.SetServo(true);
                        break;
                    case 12:
                        cDEF.MOTR.SetServo(false);
                        break;
                    case 13:
                        cDEF.MOTR.SetAlarm(true);
                        cDEF.SEQ.Reset();
                        if (cDEF.MOTR.m_bSkipChkCrash) cDEF.MOTR.m_bNeedReboot = false;
                        break;
                    case 14:
                        if (cDEF.MOTR[m_iSelMotr]._iMaker != (int)EN_MOTR_MAKER.AJIN) return;
                        cDEF.SEQ.Reset();
                        cDEF.MOTR.ReqDrvReset(EN_MOTR_MAKER.AJIN);
                        break;
                    case 15:
                        if (cDEF.MOTR[m_iSelMotr].m_iMotrKind != (int)EN_MOTR_KIND.ABS) return;

                        cDEF.MOTR.ClearHomeEnd((EN_MOTR_ID)m_iSelMotr);
                        cDEF.MOTR[m_iSelMotr].MoveHomeForce();
                        break;
                    case 16:
                        if (cDEF.MOTR[m_iSelMotr].m_iMotrKind != (int)EN_MOTR_KIND.ABS) return;
                        cDEF.MOTR[m_iSelMotr].ReqAbsOrigin();
                        break;
                }
                string temp = string.Format($"ADMIN MOTOR - {(sender as Button).Text} BUTTON CLICK - AXIS : {m_iSelMotr}");
                cDEF.LOG.Trace(temp);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }
        private void btnAbsHome_Click(object sender, EventArgs e)
        {
            try
            {
                if ((cDEF.MOTR[m_iSelMotr].m_iMotrKind != 1)) return;

                double dAbsOriginPos;
                dAbsOriginPos = cDEF.MOTR[m_iSelMotr].m_dAbsOriginPos;
                edAbsOrigin.Text = dAbsOriginPos.ToString();

                cDEF.MOTR[m_iSelMotr].ClearHomeEnd();

                cDEF.MOTR[m_iSelMotr].MoveHomeForce();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }
        private void btnAbsSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((cDEF.MOTR[m_iSelMotr].m_iMotrKind != 1)) return;


                cDEF.MOTR[m_iSelMotr].ReqAbsOrigin();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }

        private void btnRept1_Click(object sender, EventArgs e)
        {
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
                }    
			if (cDEF.MAN._bRptMotr)
			{
                FRM.ShowWarn(true, "During Repeat Current Motor");
                return;  //
			}
            try
            {
                Button Btn = (sender as Button);
                int iTag = Convert.ToInt32(Btn.Tag);

                double dPos1    = Convert.ToDouble(edRepPos1.Text);
                double dVel1    = Convert.ToDouble(edRepVel1.Text);
                double dAcc1    = Convert.ToDouble(edRepAcc1.Text);
                double dDec1    = Convert.ToDouble(edRepAcc1.Text);
                double dPos2    = Convert.ToDouble(edRepPos2.Text);
                double dVel2    = Convert.ToDouble(edRepVel2.Text);
                double dAcc2    = Convert.ToDouble(edRepAcc2.Text);
                double dDec2    = Convert.ToDouble(edRepAcc2.Text);
                double dPosJog  = Convert.ToDouble(edJogPos.Text );
                double dVelJog  = Convert.ToDouble(edJogVel.Text );
                double dTorque  = Convert.ToDouble(edTorque.Text );
                double dToqVel  = Convert.ToDouble(edTorqueVel.Text);
                double dToqPos  = Convert.ToDouble(edToqPos.Text );
                bool   bCheckOk = false;

                double dPosJogMinus = cDEF.MOTR.GetEncPos((EN_MOTR_ID)m_iSelMotr) - dPosJog;
                double dPosJogPlus  = cDEF.MOTR.GetEncPos((EN_MOTR_ID)m_iSelMotr) + dPosJog;

                switch (iTag)
                {
                    default: break;
                    case 1: bCheckOk = cDEF.MOTR.CheckCrash((EN_MOTR_ID)m_iSelMotr, EN_COMD_ID.Direct, -100, EN_FPOSN_INDEX.NONE, dPos1       ); break;
                    case 2: bCheckOk = cDEF.MOTR.CheckCrash((EN_MOTR_ID)m_iSelMotr, EN_COMD_ID.Direct, -100, EN_FPOSN_INDEX.NONE, dPos2       ); break;
                    case 3: bCheckOk = cDEF.MOTR.CheckCrash((EN_MOTR_ID)m_iSelMotr, EN_COMD_ID.Direct, -100, EN_FPOSN_INDEX.NONE, dPosJogMinus); break;
                    case 4: bCheckOk = cDEF.MOTR.CheckCrash((EN_MOTR_ID)m_iSelMotr, EN_COMD_ID.Direct, -100, EN_FPOSN_INDEX.NONE, dPosJogPlus ); break;
                }

                if (!bCheckOk)
                {
                    FRM.ShowWarn(true, "The motor can not be moved because Motor is crash");
                    return;  //
                }

                double dJogAcc = cDEF.MOTR[m_iSelMotr].MP.dAcc[(int)EN_MOTR_VEL.LJog];

                //
                switch (iTag)
                {
                    default: break;
                    case 1: cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iSelMotr, dPos1, dVel1, dAcc1  , dDec1             ); break;
                    case 2: cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iSelMotr, dPos2, dVel2, dAcc2  , dDec2             ); break;
                  //case 3: cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iSelMotr, dPosJogMinus, dVelJog, 0.1, 0.1); break;
                  //case 4: cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iSelMotr, dPosJogPlus , dVelJog, 0.1, 0.1); break;
                    case 3: cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iSelMotr, dPosJogMinus, dVelJog, dJogAcc, dJogAcc); break;
                    case 4: cDEF.MOTR.MoveMotr((EN_MOTR_ID)m_iSelMotr, dPosJogPlus , dVelJog, dJogAcc, dJogAcc); break;
                    case 5:
                        if (!cDEF.MOTR[m_iSelMotr].m_bUseTorque) return;
                        cDEF.MOTR[m_iSelMotr].MoveTorque(dTorque, dToqPos, dToqVel); break;
                }

                cDEF.LOG.Trace($"ADMIN MOTOR - DIRECT MOVE BUTTON CLICK - AXIS : {m_iSelMotr}, POS1 : {dPos1:F4}, POS2 : {dPos2:F4}");
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }
        //------------------------------------------------------------------------
        private void sbRepeatMove_Click(object sender, EventArgs e)
        {
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
                }    

            try
            {
                Button Btn = (sender as Button);
                int iTag = Convert.ToInt32(Btn.Tag);

                double dP1 = Convert.ToDouble(edRepPos1.Text);
                double dV1 = Convert.ToDouble(edRepVel1.Text);
                double dA1 = Convert.ToDouble(edRepAcc1.Text);
                double dD1 = Convert.ToDouble(edRepAcc1.Text);
                double dP2 = Convert.ToDouble(edRepPos2.Text);
                double dV2 = Convert.ToDouble(edRepVel2.Text);
                double dA2 = Convert.ToDouble(edRepAcc2.Text);
                double dD2 = Convert.ToDouble(edRepAcc2.Text);
                int iDlay = Convert.ToInt32(edRepDelay.Text);

                //
                switch (iTag)
                {
                    default: break;
                    case 1:
                        if (!cDEF.MOTR.CheckCrash((EN_MOTR_ID)m_iSelMotr))
                        {
                            FRM.ShowWarn(true, "The motor can not be moved because Motor is crash");
                            return;  //
                        }
						if (cDEF.MAN._bRptMotr)
						{
                            FRM.ShowWarn(true, "During Repeat Current Motor");
                            return;  //
						}
                        cDEF.MAN.SetRMPara(iDlay, dP1, dV1, dA1, dD1, dP2, dV2, dA2, dD2);
                        cDEF.MAN.SetRptMotr(m_iSelMotr, true);
                        break;
                    case 2:
                        cDEF.MAN.SetRptMotr(m_iSelMotr, false);
                        break;
                }
                string temp = string.Format($"ADMIN MOTOR - {(sender as Button).Text} BUTTON CLICK - AXIS : {m_iSelMotr}");
                cDEF.LOG.Trace(temp);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }

        private void tmProc_Tick(object sender, EventArgs e)
        {
			//
			if ((m_pDownBtn == null) &&  m_bDrngJog) m_bDrngJog = false;
			if ((m_pDownBtn != null) && !m_bDrngJog) m_pDownBtn = null ;
			//Mouse Down 계속 이벤트 발생. 이걸로 대체.
			if (m_bDrngJog && (m_pDownBtn != null)) {
				m_pDownBtn.PerformClick();
				}
            try
            {
                if (!this.Visible) { this.tmProc.Enabled = false; return; }
                tmProc.Enabled = false;
                FormUpdate();
                
                tmProc.Enabled = true;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
            }
        }
        private void sgMotor_SelectionChanged(object sender, EventArgs e)
        {
            if(sgMotor.SelectedCells.Count == 0) return;
            if(m_IsSelecting) return;

            m_IsSelecting = true;

            int iGridR = -1;
            int iGridC = -1;
            

            try
            {
                foreach (DataGridViewCell cell in sgMotor.SelectedCells)
                {
                    sgMotor.MultiSelect = true;
                    sgMotor.ClearSelection();
                    iGridR = cell.RowIndex;
                    iGridC = cell.ColumnIndex;


                    String sCellDat = sgMotor[0, iGridR].Value.ToString().Trim().ToUpper();
                    m_iSelMotr = Convert.ToInt32(sCellDat);

                    if (m_iSelMotr < 0) m_iSelMotr = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        sgMotor[i, iGridR].ReadOnly = true;
                        if (sgMotor[i, iGridR].Selected == false) sgMotor[i, iGridR].Selected = true;

                    }
                    break;
                }
            }
            finally
            {
                m_IsSelecting = false;
            }
        }


	}
}
