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
    public partial class FrmSManual : Form
    {
        EN_PART_SEL m_iRqPartSel        ;
        int m_iSelMotnPart              ;
        bool actived                    ;

        public EN_PART_SEL _iRqPartSel { get { return m_iRqPartSel; } set { m_iRqPartSel = value; } }

        public FrmSManual()
        {
            InitializeComponent();
            //
            TabPage tp;
            //
            this.BackColor = FRM.GetBaseColor();
            Control[] ctls = FNC.GetAllControlsUsingRecursive(this);
            foreach (Control ctl1 in ctls)
            {
                if (ctl1.GetType().Name.ToLower() == "tabpage")
                {
                    tp = ctl1 as TabPage;
                    tp.BackColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(210, 210, 208) : System.Drawing.Color.FromArgb(66, 72, 88);
                    tp.ForeColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(37, 51, 64) : System.Drawing.Color.FromArgb(230, 230, 200);
                }
            }

        }
        //------------------------------------------------------------------------
        private void FrmSManual_Load(object sender, EventArgs e)
        {
            tabMenu.Dock   = DockStyle.Top;
            tabMenu.Width  = 1100;
            tabMenu.Height = this.Height + tabMenu.ItemSize.Height;
            Rectangle Rect = new Rectangle(tpgMenu1.Left, tpgMenu1.Top, tpgMenu1.Width - 2, tpgMenu1.Height - 1); //
            tabMenu.Region = new Region(Rect);
            m_iRqPartSel   = EN_PART_SEL.None;
            m_iSelMotnPart = 0;

        }
        //------------------------------------------------------------------------
        private void FrmSManual_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        //------------------------------------------------------------------------
        private void FrmSManual_VisibleChanged(object sender, EventArgs e)
        {

            if (this.Visible && !actived)
            {//Show
                FormShow();
                tmProc.Enabled = true;
            }
            if (!this.Visible && actived)
            {//Hide
                tmProc.Enabled = false;
            }
            actived = this.Visible;
        }
        //------------------------------------------------------------------------
        public void FormShow()
        {
            SelPage(m_iSelMotnPart);
        }
        //--------------------------------------------------------------------------
        public void FormSave()
        {

        }
        //--------------------------------------------------------------------------
        public void FormUpdate()
        {
            if (m_iRqPartSel != EN_PART_SEL.None)
            {
                m_iSelMotnPart = (int)m_iRqPartSel;
                SelPage(m_iSelMotnPart);
                m_iRqPartSel = EN_PART_SEL.None;
            }

            switch (tabMenu.SelectedIndex)
            {
                default: tpgPage1Show(false); break;
                case 0: tpgPage1Show(false); break;
                case 1: tpgPage2Show(false); break;
                case 2: tpgPage3Show(false); break;
                case 3: tpgPage4Show(false); break;
            }
        }
        //--------------------------------------------------------------------------
        void SelPage(int iPage)
        {//화면의 메뉴 선택시 Tab PAGE 결정 
            tabMenu.SelectedIndex = iPage;
            switch (iPage)
            {
                default: tpgPage1Show(true); break;
                case 0: tpgPage1Show(true); break;
                case 1: tpgPage2Show(true); break;
                case 2: tpgPage3Show(true); break;
                case 3: tpgPage4Show(true); break;
            }
        }
        //--------------------------------------------------------------------------
        private void tmProc_Tick(object sender, EventArgs e)
        {
            if (!this.Visible) { this.tmProc.Enabled = false; return; }
            tmProc.Enabled = false;
            FormUpdate();

            btMan06.Visible = cDEF.FM.IsMasterLv(); //JUNG/230331//

            tmProc.Enabled = true;
        }
        //---------------------------------------------------------------------------
        void GetStatVac(Button Btn, bool bOn)
        {//UserSet - 각 PART별 Vacuum 상태 표시 
         //Get Object.
            int iTag = Convert.ToInt32(Btn.Tag);

            bool isStat = false;

            if (iTag == (int)EN_OUT_ID.yNone) return;

            bool isSignalOn = cDEF.IO.gY((EN_OUT_ID)iTag);

            //if(iTag == (int)EN_OUT_ID.yALG_WaferVac) isStat = cDEF.SEQ.ALG.IsVacStat(true);

            if (bOn) {
                Btn.BackColor = isStat ? Color.Red : (isSignalOn) ? Color.Lime : Color.Gray;
            }
            else {
                Btn.BackColor = !isStat ? Color.Red : (!isSignalOn) ? Color.Lime : Color.Gray;
            }
        }
        //---------------------------------------------------------------------------
        void GetStatOut(Button Btn, bool bOn)
        {//UserSet   - 각 PART별 Break 상태 표시 
         //Get Object.
            int iTag = Convert.ToInt32(Btn.Tag);

            if (iTag == (int)EN_OUT_ID.yNone) return;

            bool isSignalOn = cDEF.IO.gY((EN_OUT_ID)iTag);

            if (bOn) {
                Btn.BackColor = isSignalOn ? Color.Lime : Color.Gray;
            }
            else {
                Btn.BackColor = !isSignalOn ? Color.Lime : Color.Gray;
            }

        }
        //---------------------------------------------------------------------------
        void GetStatActr(Button Btn, EN_ACTR_CMD iCMD)
        {
            //Get Object.
            int iTag = Convert.ToInt32(Btn.Tag);

            if (iCMD == EN_ACTR_CMD.Fwd) {
                Btn.BackColor = cDEF.ACTR.Complete(iTag, (int)iCMD) ? Color.Lime : Color.Gray;
            }
            else {
                Btn.BackColor = cDEF.ACTR.Complete(iTag, (int)iCMD) ? Color.Lime : Color.Gray;
            }
        }

        void GetStatActrTog(Button Btn, EN_ACTR_CMD iCMD)
        {
            //Get Object.
            int iTag = Convert.ToInt32(Btn.Tag);
            if (iCMD == EN_ACTR_CMD.Fwd) {
                Btn.ImageIndex = cDEF.ACTR.Complete(iTag, (int)iCMD) ? 1 : 0;
            }
            else {
                Btn.ImageIndex = cDEF.ACTR.Complete(iTag, (int)iCMD) ? 1 : 0;
            }
        }
        //---------------------------------------------------------------------------
        void GetStatSen(Label Lbl)
        {
            //Get Object.
            int iTag = Convert.ToInt32(Lbl.Tag);
            if (iTag == (int)EN_IN_ID.xNone) return;

            Lbl.BackColor = cDEF.IO.gX((EN_IN_ID)iTag) ? Color.Lime : Color.Gray;
        }

        //---------------------------------------------------------------------------
        void MoveActr(int iActrNo, EN_ACTR_CMD iCMD)
        {
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
            }
            if (iActrNo == (int)EN_ACTR_ID.None) return;
            cDEF.ACTR.MoveCyl((int)iActrNo, (int)iCMD);
        }

        void MoveActrToggle(int iActrNo)
        {
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
            }
            if (iActrNo == (int)EN_ACTR_ID.None) return;
            bool isBwd = cDEF.ACTR.Complete((int)iActrNo, (int)EN_ACTR_CMD.Bwd);
            cDEF.ACTR.MoveCyl((int)iActrNo, isBwd ? (int)EN_ACTR_CMD.Fwd : (int)EN_ACTR_CMD.Bwd);
        }

        void SetOutput(int iOutNo, bool bOn)
        {
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
            }
            if (iOutNo == (int)EN_OUT_ID.yNone) return;
            cDEF.IO.sY((EN_OUT_ID)iOutNo, bOn);
        }

        void SetOutputToggle(int iOutNo)
        {
            if (cDEF.SEQ._bRun) {
                MsgBox.Warning("Can not be Move while the Machine is running.");
                return;
            }
            if (iOutNo == (int)EN_OUT_ID.yNone) return;

            bool bOn = cDEF.IO.gY((EN_OUT_ID)iOutNo);
            cDEF.IO.sY((EN_OUT_ID)iOutNo, !bOn);
        }

        //---------------------------------------------------------------------------
        void ManProc(int iManNo, bool bOn)
        {
            if (cDEF.SEQ._bRun) 
            {
                MsgBox.Warning("Can not be use while the Machine is running.");
                return;
            }

            if (bOn) cDEF.MAN.ManProcOn (iManNo, true, false);
            else     cDEF.MAN.ManProcOff(iManNo, false, true);
        }
        //------------------------------------------------------------------------
        public void tpgPage1Show(bool toCtrl) //WTR
        {//Tab Page #1 Show (화면 업데이트)
         //bool isOn = false;

            if (toCtrl)
            {
                //
                gbActr1_1.Visible = false; //kbtActrF1_1.Tag = 392 ; kbtActrB1_1.Tag = 392 ; gbActr1_1.GroupTitle = "Mapping Sensor Fwd/Bwd"   ; 
                gbActr1_1.Visible = false; //kbtActrF1_1.Tag = 3014; kbtActrB1_1.Tag = 3014; gbActr1_1.GroupTitle = "Mapping Sensor Fwd/Bwd"   ; 
                                           //
                gbActr1_2.Visible = false; //kbtActrF1_2.Tag = 3000; kbtActrB1_2.Tag = 3000; gbActr1_2.GroupTitle = "Mapper Conversion Fwd/Bwd"; 
                gbActr1_3.Visible = false; //kbtActrF1_3.Tag = 390 ; kbtActrB1_3.Tag = 390 ; gbActr1_3.GroupTitle = "Load CST Rotator"         ; 
                gbActr1_4.Visible = false; //kbtActrF1_4.Tag = 391 ; kbtActrB1_4.Tag = 391 ; gbActr1_4.GroupTitle = "Unload CST Rotator"       ; 
                                           //                                                                       
                gbActr1_5.Visible = true ;   kbtActrF1_5.Tag = 2500; kbtActrB1_5.Tag = 2500; gbActr1_5.GroupTitle = "Vacuum"  ; 
                gbActr1_6.Visible = true ;   kbtActrF1_6.Tag = 2501; kbtActrB1_6.Tag = 2501; gbActr1_6.GroupTitle = "Blow"    ; 
                                           //
                gbActr1_7.Visible = false; //kbtActrF1_7.Tag = 3003; kbtActrB1_7.Tag = 3003; gbActr1_7.GroupTitle = "B Fork - Vacuum"          ; 
                gbActr1_8.Visible = false; //kbtActrF1_8.Tag = 3004; kbtActrB1_8.Tag = 3004; gbActr1_8.GroupTitle = "B Fork - Blow"            ; 

                btMan02.Tag = 2002;
                btMan03.Tag = 2003;
                btMan04.Tag = 2004;
                btMan05.Tag = 2010;
                btMan06.Tag = 2011;
                btMan07.Tag = 2013; //2012;
                button1.Tag = 2020;


            }
            else
            {
                //kbtActrF1_1.Checked = cDEF.ACTR.Complete((int)EN_ACTR_ID.aWTR_MapFwBw, vDEF.FWD)    ; kbtActrB1_1.Checked = cDEF.ACTR.Complete((int)EN_ACTR_ID.aWTR_MapFwBw, vDEF.BWD);
                //kbtActrF1_2.Checked = cDEF.SEQ.WTR.IsMoveCylMappingCV(vDEF.FWD)                     ; kbtActrB1_2.Checked = cDEF.SEQ.WTR.IsMoveCylMappingCV(vDEF.BWD);
                //kbtActrF1_3.Checked = cDEF.ACTR.Complete((int)EN_ACTR_ID.aLMZ_Rot, vDEF.FWD)        ; kbtActrB1_3.Checked = cDEF.ACTR.Complete((int)EN_ACTR_ID.aLMZ_Rot, vDEF.BWD);
                //kbtActrF1_4.Checked = cDEF.ACTR.Complete((int)EN_ACTR_ID.aUMZ_Rot, vDEF.FWD)        ; kbtActrB1_4.Checked = cDEF.ACTR.Complete((int)EN_ACTR_ID.aUMZ_Rot, vDEF.BWD);

                kbtActrF1_5.Checked   = cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON   )                          ; kbtActrB1_5.Checked = !cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON   );
                kbtActrF1_6.Checked   = cDEF.IO.gY(EN_OUT_ID.yVACUUM_PURGE)                          ; kbtActrB1_6.Checked = !cDEF.IO.gY(EN_OUT_ID.yVACUUM_PURGE);
                
                //kbtActrF1_7.Checked = cDEF.IO.gY(EN_OUT_ID.yWTR_B_VacOn)                            ; kbtActrB1_7.Checked = !cDEF.IO.gY(EN_OUT_ID.yWTR_B_VacOn);
                //kbtActrF1_8.Checked = cDEF.IO.gY(EN_OUT_ID.yWTR_B_BlowOn)                           ; kbtActrB1_8.Checked = !cDEF.IO.gY(EN_OUT_ID.yWTR_B_BlowOn);

                //lb1_1.BackColor       = cDEF.IO.gX((EN_IN_ID)lb1_1.Tag) ? Color.Lime : Color.Gray    ;
                //lb1_2.BackColor     = cDEF.IO.gX(EN_IN_ID.xLMZ_Exist6) ? Color.Lime : Color.Gray    ;
                //lb1_3.BackColor     = cDEF.IO.gX(EN_IN_ID.xUMZ_Exist5) ? Color.Lime : Color.Gray    ;
                //lb1_4.BackColor     = cDEF.IO.gX(EN_IN_ID.xUMZ_Exist6) ? Color.Lime : Color.Gray    ;
                //lb1_5.BackColor     = cDEF.IO.gX(EN_IN_ID.xWTR_MapExist) ? Color.Lime : Color.Gray  ;

            }

        }
        //------------------------------------------------------------------------
        public void tpgPage2Show(bool toCtrl) //WAT
        {//Tab Page #2 Show (화면 업데이트)

            if (toCtrl)
            {
                gbActr2_1.Visible = true ; kbtActrF2_1.Tag = 302; kbtActrB2_1.Tag = 302; gbActr2_1.GroupTitle = "Align - Front" ;
                gbActr2_2.Visible = true ; kbtActrF2_2.Tag = 303; kbtActrB2_2.Tag = 303; gbActr2_2.GroupTitle = "Align - Left"  ;
                gbActr2_3.Visible = true ; kbtActrF2_3.Tag = 304; kbtActrB2_3.Tag = 304; gbActr2_3.GroupTitle = "Align - Rear"  ;
                gbActr2_4.Visible = true ; kbtActrF2_4.Tag = 305; kbtActrB2_4.Tag = 305; gbActr2_4.GroupTitle = "Align - Right" ;
                                           
                gbActr2_5.Visible = false; //kbtActrF2_5.Tag = 4000; kbtActrB2_5.Tag = 4000; gbActr2_5.GroupTitle = "Vacuum"          ; 
                gbActr2_6.Visible = false; //kbtActrF2_6.Tag = 4001; kbtActrB2_6.Tag = 4001; gbActr2_6.GroupTitle = "Blow"            ; 
                gbActr2_7.Visible = false; //kbtActrF2_7.Tag = 4002; kbtActrB2_7.Tag = 4002; gbActr2_7.GroupTitle = "Cleaner-Air Blow"; 
                gbActr2_8.Visible = false; //kbtActrF2_8.Tag = 4003; kbtActrB2_8.Tag = 4003; gbActr2_8.GroupTitle = "Cleaner-Suction" ; 

                //lb2_1.Visible     = true ; lb2_1.Text = string.Format($"WAFER CHECK[{cDEF.IO.sXA[(int)EN_IN_ID.xWAT_ExistWafer]}]" ); lb2_1.Tag = EN_IN_ID.xWAT_ExistWafer;
                //lb2_2.Visible     = true ; lb2_2.Text = string.Format($"ION RUN[{cDEF.IO.sYA[(int)EN_OUT_ID.yWAT_ION_RUN]}]"       ); lb2_2.Tag = EN_OUT_ID.yWAT_ION_RUN  ;
                //lb2_3.Visible     = true ; lb2_3.Text = string.Format($"ION ALARM[{cDEF.IO.sXA[(int)EN_IN_ID.xWAT_Ionizer_Alarm]}]"); lb2_3.Tag = EN_IN_ID.xWAT_Ionizer_Alarm  ;

                btMan10.Tag = 3000;
                btMan11.Tag = 3001;
                btMan12.Tag = 3002;
                btMan13.Tag = 3003;
                btMan14.Tag = 3004;
            }
            else
            {
                kbtActrF2_1.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aWAT_GuideFront ); kbtActrB2_1.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aWAT_GuideFront);
                kbtActrF2_2.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aWAT_GuideLeft  ); kbtActrB2_2.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aWAT_GuideLeft );
                kbtActrF2_3.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aWAT_GuideRear  ); kbtActrB2_3.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aWAT_GuideRear );
                kbtActrF2_4.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aWAT_GuideRight ); kbtActrB2_4.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aWAT_GuideRight);

                lb2_1.BackColor = cDEF.IO.gX((EN_IN_ID )lb2_1.Tag) ? Color.Lime : Color.Gray;
                lb2_2.BackColor = cDEF.IO.gY((EN_OUT_ID)lb2_2.Tag) ? Color.Lime : Color.Gray;
                lb2_3.BackColor = cDEF.IO.gX((EN_IN_ID )lb2_3.Tag) ? Color.Red  : Color.Gray;

            }
        }
        //------------------------------------------------------------------------
        public void tpgPage3Show(bool toCtrl) //LPM#1
        {//Tab Page #3 Show (화면 업데이트)

            if (toCtrl)
            {
                //
                gbActr3_1.Visible  = true ; kbtActrF3_1.Tag  = 290;  kbtActrB3_1.Tag  = 290;  gbActr3_1.GroupTitle  = "[LPM1] Base Lock - Left" ;
                gbActr3_2.Visible  = true ; kbtActrF3_2.Tag  = 291;  kbtActrB3_2.Tag  = 291;  gbActr3_2.GroupTitle  = "[LPM1] Base Lock - Right";
                gbActr3_3.Visible  = true ; kbtActrF3_3.Tag  = 292;  kbtActrB3_3.Tag  = 292;  gbActr3_3.GroupTitle  = "[LPM1] Base Load"        ;
                gbActr3_4.Visible  = true ; kbtActrF3_4.Tag  = 293;  kbtActrB3_4.Tag  = 293;  gbActr3_4.GroupTitle  = "[LPM1] Door open"        ;
                gbActr3_5.Visible  = true ; kbtActrF3_5.Tag  = 294;  kbtActrB3_5.Tag  = 294;  gbActr3_5.GroupTitle  = "[LPM1] Foup Lock - Left" ;
                gbActr3_6.Visible  = true ; kbtActrF3_6.Tag  = 295;  kbtActrB3_6.Tag  = 295;  gbActr3_6.GroupTitle  = "[LPM1] Foup Lock - Right";
                
                gbActr3_7.Visible  = true ; kbtActrF3_7.Tag  = 4500; kbtActrB3_7.Tag  = 4500; gbActr3_7.GroupTitle  = "[LPM1] Door Vacuum";
                gbActr3_8.Visible  = true ; kbtActrF3_8.Tag  = 4501; kbtActrB3_8.Tag  = 4501; gbActr3_8.GroupTitle  = "[LPM1] Door Blow" ;
                
                
                //lb3_1.Visible = true; lb3_1.Text = string.Format($"PORT AREA [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT_Area]}]"                );  lb3_1.Tag = EN_IN_ID.xLPM_PORT_Area        ;
                //lb3_2.Visible = true; lb3_2.Text = string.Format($"PORT1_PAD_01 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT1_PAD01]}]"           );  lb3_2.Tag = EN_IN_ID.xLPM_PORT1_PAD01      ;
                //lb3_3.Visible = true; lb3_3.Text = string.Format($"PORT1_PAD_02 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT1_PAD02]}]"           );  lb3_3.Tag = EN_IN_ID.xLPM_PORT1_PAD02      ;
                //lb3_4.Visible = true; lb3_4.Text = string.Format($"PORT1_PAD_03 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT1_PAD03]}]"           );  lb3_4.Tag = EN_IN_ID.xLPM_PORT1_PAD03      ;
                //lb3_5.Visible = true; lb3_5.Text = string.Format($"PORT1_INFOPAD1 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT1_INFOPAD_1]}]"     );  lb3_5.Tag = EN_IN_ID.xLPM_PORT1_INFOPAD_1  ;
                //lb3_6.Visible = true; lb3_6.Text = string.Format($"PORT1_INFOPAD2 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT1_INFOPAD_2]}]"     );  lb3_6.Tag = EN_IN_ID.xLPM_PORT1_INFOPAD_2  ;
                //lb3_7.Visible = true; lb3_7.Text = string.Format($"PORT1_MAP_SENSOR[{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT1_MAP_SENSOR]}]"   );  lb3_7.Tag = EN_IN_ID.xLPM_PORT1_MAP_SENSOR ;
                //lb3_8.Visible = true; lb3_8.Text = string.Format($"PORT1_PANEL_EXIST[{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT1_PANEL_EXIST]}]" );  lb3_8.Tag = EN_IN_ID.xLPM_PORT1_PANEL_EXIST;
                //                                                 
                //lb3_H.Visible = true; lb3_H.Text = string.Format($"PORT1_IONIZER_RUN[{cDEF.IO.sYA[(int)EN_OUT_ID.yLPM_PORT1_ION_RUN]}]"    );  lb3_H.Tag = EN_OUT_ID.yLPM_PORT1_ION_RUN;

                //
                btTab1.Tag   = 4000; //Load
                btTab2.Tag   = 4001; //Unpack
                btTab3.Tag   = 4002; //Mapping
                btTab4.Tag   = 4003; //Pack
                btTab5.Tag   = 4004; //RFID
                btTab3_6.Tag = 4504; //Ion On
                btTab3_7.Tag = 4505; //Ion Off

            }
            else
            {
                kbtActrF3_1.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_MGZLock_L1_L ); kbtActrB3_1.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_MGZLock_L1_L );
                kbtActrF3_2.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_MGZLock_L1_R ); kbtActrB3_2.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_MGZLock_L1_R );
                kbtActrF3_3.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_MGZLoad_L1   ); kbtActrB3_3.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_MGZLoad_L1   );
                kbtActrF3_4.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_DoorOpen_L1  ); kbtActrB3_4.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_DoorOpen_L1  );
                kbtActrF3_5.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_DoorLock_L1_L); kbtActrB3_5.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_DoorLock_L1_L);
                kbtActrF3_6.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_DoorLock_L1_R); kbtActrB3_6.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_DoorLock_L1_R);

              //kbtActrF3_7.Checked   = cDEF.IO.gY(EN_OUT_ID.yLPM_P1_DoorVacOn)              ; kbtActrB3_7.Checked = !cDEF.IO.gY(EN_OUT_ID.yLPM_P1_DoorVacOn);
                //kbtActrF3_7.Checked   = cDEF.IO.gX(EN_IN_ID.xLPM_PORT1_DOOR_VAC)             ; kbtActrB3_7.Checked = !cDEF.IO.gX(EN_IN_ID.xLPM_PORT1_DOOR_VAC);
                //kbtActrF3_8.Checked   = cDEF.IO.gY(EN_OUT_ID.yLPM_P1_DoorBlow )              ; kbtActrB3_8.Checked = !cDEF.IO.gY(EN_OUT_ID.yLPM_P1_DoorBlow );
                

                lb3_1.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_1.Tag) ? Color.Lime : Color.Gray;
                lb3_2.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_2.Tag) ? Color.Lime : Color.Gray;
                lb3_3.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_3.Tag) ? Color.Lime : Color.Gray;
                lb3_4.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_4.Tag) ? Color.Lime : Color.Gray;
                lb3_5.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_5.Tag) ? Color.Lime : Color.Gray;
                lb3_6.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_6.Tag) ? Color.Lime : Color.Gray;
                lb3_7.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_7.Tag) ? Color.Lime : Color.Gray;
                lb3_8.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_8.Tag) ? Color.Lime : Color.Gray;
                lb3_H.BackColor = cDEF.IO.gY((EN_OUT_ID)lb3_H.Tag) ? Color.Lime : Color.Gray;
                

            }
        }
        //------------------------------------------------------------------------
        public void tpgPage4Show(bool toCtrl)
        {//Tab Page #4 Show (화면 업데이트)
        
            if (toCtrl)
            {
                //
                gbActr4_1.Visible  = true ; kbtActrF4_1.Tag  = 296;  kbtActrB4_1.Tag  = 296;  gbActr4_1.GroupTitle  = "[LPM2] Base Lock - Left" ;
                gbActr4_2.Visible  = true ; kbtActrF4_2.Tag  = 297;  kbtActrB4_2.Tag  = 297;  gbActr4_2.GroupTitle  = "[LPM2] Base Lock - Right";
                gbActr4_3.Visible  = true ; kbtActrF4_3.Tag  = 298;  kbtActrB4_3.Tag  = 298;  gbActr4_3.GroupTitle  = "[LPM2] Base Load"        ;
                gbActr4_4.Visible  = true ; kbtActrF4_4.Tag  = 299;  kbtActrB4_4.Tag  = 299;  gbActr4_4.GroupTitle  = "[LPM2] Door open"        ;
                gbActr4_5.Visible  = true ; kbtActrF4_5.Tag  = 300;  kbtActrB4_5.Tag  = 300;  gbActr4_5.GroupTitle  = "[LPM2] Foup Lock - Left" ;
                gbActr4_6.Visible  = true ; kbtActrF4_6.Tag  = 301;  kbtActrB4_6.Tag  = 301;  gbActr4_6.GroupTitle  = "[LPM2] Foup Lock - Right";
                
                gbActr4_7.Visible  = true ; kbtActrF4_7.Tag  = 4502; kbtActrB4_7.Tag  = 4502; gbActr4_7.GroupTitle  = "[LPM2] Door Vacuum";
                gbActr4_8.Visible  = true ; kbtActrF4_8.Tag  = 4503; kbtActrB4_8.Tag  = 4503; gbActr4_8.GroupTitle  = "[LPM2] Door Blow"  ;
               
                //lb3_9.Visible = true; lb3_9.Text = string.Format($"PORT AREA [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT_Area]}]"                );  lb3_9.Tag = EN_IN_ID.xLPM_PORT_Area        ;
                //lb3_A.Visible = true; lb3_A.Text = string.Format($"PORT2_PAD_01 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT2_PAD01]}]"           );  lb3_A.Tag = EN_IN_ID.xLPM_PORT2_PAD01      ;
                //lb3_B.Visible = true; lb3_B.Text = string.Format($"PORT2_PAD_02 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT2_PAD02]}]"           );  lb3_B.Tag = EN_IN_ID.xLPM_PORT2_PAD02      ;
                //lb3_C.Visible = true; lb3_C.Text = string.Format($"PORT2_PAD_03 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT2_PAD03      ]}]"     );  lb3_C.Tag = EN_IN_ID.xLPM_PORT2_PAD03      ;
                //lb3_D.Visible = true; lb3_D.Text = string.Format($"PORT2_INFOPAD1 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT2_INFOPAD_1]}]"     );  lb3_D.Tag = EN_IN_ID.xLPM_PORT2_INFOPAD_1  ;
                //lb3_E.Visible = true; lb3_E.Text = string.Format($"PORT2_INFOPAD2 [{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT2_INFOPAD_2  ]}]"   );  lb3_E.Tag = EN_IN_ID.xLPM_PORT2_INFOPAD_2  ;
                //lb3_F.Visible = true; lb3_F.Text = string.Format($"PORT2_MAP_SENSOR[{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT2_MAP_SENSOR]}]"   );  lb3_F.Tag = EN_IN_ID.xLPM_PORT2_MAP_SENSOR ;
                //lb3_G.Visible = true; lb3_G.Text = string.Format($"PORT2_PANEL_EXIST[{cDEF.IO.sXA[(int)EN_IN_ID.xLPM_PORT2_PANEL_EXIST]}]" );  lb3_G.Tag = EN_IN_ID.xLPM_PORT2_PANEL_EXIST;
                //                                                 
                //lb3_I.Visible = false; lb3_I.Text = string.Format($"PORT2_IONIZER_RUN[{cDEF.IO.sYA[(int)EN_OUT_ID.yLPM_PORT1_ION_RUN]}]"   );  lb3_I.Tag = EN_OUT_ID.yLPM_PORT2_ION_RUN;

                //
                btTab4_1.Tag = 4100; //Load
                btTab4_2.Tag = 4101; //Unpack
                btTab4_3.Tag = 4102; //Mapping
                btTab4_4.Tag = 4103; //Pack
                btTab4_5.Tag = 4104; //RFID
                btTab4_6.Tag = 4506; //Ion On
                btTab4_7.Tag = 4507; //Ion Off


            }
            else
            {
                kbtActrF4_1.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_MGZLock_L2_L ); kbtActrB4_1.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_MGZLock_L2_L );
                kbtActrF4_2.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_MGZLock_L2_R ); kbtActrB4_2.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_MGZLock_L2_R );
                kbtActrF4_3.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_MGZLoad_L2   ); kbtActrB4_3.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_MGZLoad_L2   );
                kbtActrF4_4.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_DoorOpen_L2  ); kbtActrB4_4.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_DoorOpen_L2  );
                kbtActrF4_5.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_DoorLock_L2_L); kbtActrB4_5.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_DoorLock_L2_L);
                kbtActrF4_6.Checked = cDEF.ACTR.GetCompleteFwd(EN_ACTR_ID.aLPM_DoorLock_L2_R); kbtActrB4_6.Checked = cDEF.ACTR.GetCompleteBwd(EN_ACTR_ID.aLPM_DoorLock_L2_R);
              //kbtActrF4_7.Checked   = cDEF.IO.gY(EN_OUT_ID.yLPM_P2_DoorVacOn)              ; kbtActrB4_7.Checked = !cDEF.IO.gY(EN_OUT_ID.yLPM_P2_DoorVacOn);
                //kbtActrF4_7.Checked   = cDEF.IO.gX(EN_IN_ID.xLPM_PORT2_DOOR_VAC)             ; kbtActrB4_7.Checked = !cDEF.IO.gX(EN_IN_ID.xLPM_PORT2_DOOR_VAC);
                //kbtActrF4_8.Checked   = cDEF.IO.gY(EN_OUT_ID.yLPM_P2_DoorBlow )              ; kbtActrB4_8.Checked = !cDEF.IO.gY(EN_OUT_ID.yLPM_P2_DoorBlow );
                
                lb3_9.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_9.Tag) ? Color.Lime : Color.Gray;
                lb3_A.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_A.Tag) ? Color.Lime : Color.Gray;
                lb3_B.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_B.Tag) ? Color.Lime : Color.Gray;
                lb3_C.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_C.Tag) ? Color.Lime : Color.Gray;
                lb3_D.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_D.Tag) ? Color.Lime : Color.Gray;
                lb3_E.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_E.Tag) ? Color.Lime : Color.Gray;
                lb3_F.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_F.Tag) ? Color.Lime : Color.Gray;
                lb3_G.BackColor = cDEF.IO.gX((EN_IN_ID )lb3_G.Tag) ? Color.Lime : Color.Gray;
                lb3_I.BackColor = cDEF.IO.gY((EN_OUT_ID)lb3_I.Tag) ? Color.Lime : Color.Gray;

            }
        }
        //------------------------------------------------------------------------
        #region Page #1
        private void kbtActrB1_Click(object sender, EventArgs e)
        {
            KToggleButton Btn = (sender as KToggleButton);
            int iManNo = Convert.ToInt32(Btn.Tag);
            //
            ManProc(iManNo, false);
        }
        //------------------------------------------------------------------------
        private void kbtActrF1_Click(object sender, EventArgs e)
        {
            KToggleButton Btn = (sender as KToggleButton);
            int iManNo = Convert.ToInt32(Btn.Tag);
            //
            ManProc(iManNo, true);
        }
        #endregion
        //------------------------------------------------------------------------
        private void btMan10_Click(object sender, EventArgs e)
        {
            Button Btn     = (sender as Button);
            int iManNo     = Convert.ToInt32(Btn.Tag);
            
            //
            ManProc(iManNo, true);
        }
        //------------------------------------------------------------------------
        private void btMan11_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iManNo = Convert.ToInt32(Btn.Tag);
            
            ManProc(iManNo, true);
           
        }
        //------------------------------------------------------------------------
        private void btTab1_Click(object sender, EventArgs e)
        {
            //
            Button Btn = (sender as Button);
            int iManNo = Convert.ToInt32(Btn.Tag);

            ManProc(iManNo, true);
        }
        //------------------------------------------------------------------------
        private void cbSelWhreMGZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iManNo = Convert.ToInt32(Btn.Tag);


            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Filter = "[BMP FILE]|*.bmp|[JPEG FILE]|*.jpg|[ALL FILE]|*.*";
                FileDialog.DefaultExt = "*.bmp";
                FileDialog.Title = "이미지 불러오기";

                if (FileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap Temp = new Bitmap(FileDialog.FileName))
                    {
                        _TFuncArg arg = new _TFuncArg();

                        if (Temp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed ||
                            Temp.Width       != cDEF.VISN.Cam[(int)EN_CAM.WTB].Buffer.wid            ||
                            Temp.Height      != cDEF.VISN.Cam[(int)EN_CAM.WTB].Buffer.len             )
                        {
                              
                            MsgBox.Warning("Can not be Tested this image format");
                        }
                        else
                        {
                            arg.sArg1 = FileDialog.FileName;
                            //
                            cDEF.MAN.ManProcOn(iManNo, true, false, arg);
                        }
                    }
                }
            }
        
        }
    }
}
