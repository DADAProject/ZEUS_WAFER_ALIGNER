using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InoModule
{
    public partial class FrmSCam : Form
    {
        public EN_CAM_SEL m_iRqCam1Sel;
        public EN_CAM_SEL m_iRqCam2Sel;
        int               m_iSelCam1  ;
        int               m_iSelCam2  ;
        bool              actived     ;
        public FrmSCam()
        {
            InitializeComponent();
        }

        private void FrmSCam_Load(object sender, EventArgs e)
        {
            m_iRqCam1Sel = EN_CAM_SEL.None;
            m_iRqCam2Sel = EN_CAM_SEL.None;

            m_iSelCam1   = 0;
            m_iSelCam2   = 1;

        }

        private void FrmSCam_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmProc.Enabled = false;
        }

        private void FrmSCam_VisibleChanged(object sender, EventArgs e)
        {
             if(this.Visible) 
             {//Show
                if(actived) return; 
                FormShow();  
                tmProc.Enabled = true;
             }
             else
             {//Hide
                if(!actived) return; 
                HideCam(m_iSelCam1);
                HideCam(m_iSelCam2);
                tmProc.Enabled = false;
             }
             actived = Visible;
        }


        public void FormShow()
        {
            SelCam1(m_iSelCam1);
            SelCam2(m_iSelCam2);            
        }

        public void FormSave()
        {


        }



        public void FormUpdate()
        {
            if(m_iRqCam1Sel !=  EN_CAM_SEL.None)
            {
                SelCam1((int)m_iRqCam1Sel);
                m_iRqCam1Sel = EN_CAM_SEL.None;
            }
            if(m_iRqCam2Sel !=  EN_CAM_SEL.None)
            {
                SelCam2((int)m_iRqCam2Sel);
                m_iRqCam2Sel = EN_CAM_SEL.None;
            }
        }
        public void SelCam1(int iCam)
        {
            if(iCam == m_iSelCam2                   ) return; //2번에서 사용중

            pnCam1   .BringToFront();
            pnSelCam1.BringToFront();

            HideCam(m_iSelCam1);
            m_iSelCam1 = iCam;
            ShowCam(m_iSelCam1, ref this.pnCam1);
        }
        public void SelCam2(int iCam)
        {
            if(iCam == (int)EN_CAM_SEL.NoShow)
            {
                HideCam(m_iSelCam2);
            }
            if(iCam == m_iSelCam1                   ) return; //1번에서 사용중

            pnCam2   .BringToFront();
            pnSelCam2.BringToFront();

            HideCam(m_iSelCam2);
            m_iSelCam2 = iCam;
            ShowCam(m_iSelCam2, ref this.pnCam2);
        }

        public void ShowCam(int iCam, ref Panel pnCam)
        {
            if(iCam <0 || iCam>=(int)EN_CAM.EndofCam) return;

            bool isNoRun = !cDEF.SEQ._bRun && cDEF.MAN._iManNo == 0;
            switch((EN_CAM)iCam)
            {
                case EN_CAM.BTB : FRM.ShowFormParent    (FRM.Cam1 , pnCam); FRM.Cam1.SetBtnVisible(false);  break;
            }  
            if(isNoRun && VSN.Cam[iCam] != null) 
            {
                VSN.Cam[iCam].OnLive(true);
                VSN.Cam[iCam].SetRoiVisibleAll  (false);
            }
        }
        public void HideCam(int iCam)
        {
            if(iCam <0 || iCam>=(int)EN_CAM.EndofCam) return;
            bool isNoRun = !cDEF.SEQ._bRun && cDEF.MAN._iManNo == 0;

            if(isNoRun && VSN.Cam[iCam] != null)  VSN.Cam[iCam].OnLive(false);
            switch((EN_CAM)iCam)
            {
                case EN_CAM.BTB: FRM.HideFormParent    (FRM.Cam1); break;
            }
        }
        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }
            tmProc.Enabled = false;
            FormUpdate();
            tmProc.Enabled = true ;
        }

        private void btnSelCam1_1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            m_iRqCam1Sel = (EN_CAM_SEL)iTag;
        }

        private void btnSelCam2_1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            m_iRqCam1Sel = (EN_CAM_SEL)iTag;
        }
    }
}
