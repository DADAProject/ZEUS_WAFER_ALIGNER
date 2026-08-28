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
    public partial class FrmMap : Form
    {
        //
        int [] LDMGZ  = new int [50];
        int [] UDMGZ  = new int [50];
        int [] WAFER  = new int [(int)EN_WAF_ID.EndOfId];

        Label[] lbMgzLD = new Label[50];
        Label[] lbMgzUD = new Label[50];

        bool m_bShow; 


        public FrmMap()
        {
            InitializeComponent();

            for (int i = 0; i < LDMGZ.Length; i++)
            {
                LDMGZ[i] = (int)EN_WAFER_STAT.Empty;
                UDMGZ[i] = (int)EN_WAFER_STAT.Empty;
            }

        }
        //------------------------------------------------------------------------
        private void FormMap_Load(object sender, EventArgs e)
        {
            //
            int nLDSize = cDEF.FM.ProjBase.iMaxMgzSlot[(int)EN_MGZ_ID.LMZ ];

            //Load Magazine
            if (nLDSize != LDMGZ.Length) 
            { 
                Array.Resize<int> (ref LDMGZ, nLDSize); 
                Array.Resize<Label>(ref lbMgzLD, nLDSize); 
                tlpLDMGZ.RowCount = nLDSize;
                tlpLDMGZ.Controls.Clear();
                //
                for (int i = 0; i < LDMGZ.Length; i++)
                {
                    //if (cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ].IsStat(i, EN_WAFER_STAT.Mount)) LDMGZ[i] = (int)EN_WAFER_STAT.Mount; 
                    //else                                                              LDMGZ[i] = (int)EN_WAFER_STAT.Empty;

                    LDMGZ[i] = (int)cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ].GetStat(i);
                    if (LDMGZ[i] == (int)EN_WAFER_STAT.Mask) LDMGZ[i] = (int)EN_WAFER_STAT.Empty;

                    //lbMgzLD[i] = CreateLabel($"lbMgzLD{i}", $"[{i+1:0#}]", i);
                    lbMgzLD[i] = CreateLabel($"lbMgzLD{i}", $"[{nLDSize - i :0#}]", i); //JUNG/220329
                    
                    tlpLDMGZ.Controls.Add(lbMgzLD[i], 0, i);
                }
            }
            else
            {
                for (int i = 0; i < LDMGZ.Length; i++)
                {
                    //if (cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ].IsStat(i, EN_WAFER_STAT.Mount)) LDMGZ[i] = (int)EN_WAFER_STAT.Mount;
                    //else LDMGZ[i] = (int)EN_WAFER_STAT.Empty;

                    LDMGZ[i] = (int)cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ].GetStat(i);
                    if (LDMGZ[i] == (int)EN_WAFER_STAT.Mask) LDMGZ[i] = (int)EN_WAFER_STAT.Empty;
                }

            }

            //Wafer
            //cDEF.DM.WAF[(int)EN_WAF_ID.WTR_A].Update(ref pbWaf1, true);
            //cDEF.DM.WAF[(int)EN_WAF_ID.WTR_B].Update(ref pbWaf2, true);
            //cDEF.DM.WAF[(int)EN_WAF_ID.WAT  ].Update(ref pbWaf3, true);
            //cDEF.DM.WAF[(int)EN_WAF_ID.WTB  ].Update(ref pbWaf4, true);

            for (int i = 0; i < (int)EN_WAF_ID.EndOfId; i++)
            {
                WAFER[i] = (int)cDEF.DM.WAF[i].GetWaferStat();
            }

            m_bShow = true;

            //
            tmUpdate.Interval = 100;
            tmUpdate.Enabled = true; 

        }
        //------------------------------------------------------------------------
        private void btExit_Click(object sender, EventArgs e)
        {
            //
            m_bShow = false; 
            tmUpdate.Enabled = false;
            this.Close();
        }
        //------------------------------------------------------------------------
        private void btExist_Click(object sender, EventArgs e)
        {
            //
            if (cDEF.SEQ._bRun) return; 

            //
            for (int i = 0; i < LDMGZ.Length; i++)
            {
                cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ].SetTo(i, (EN_WAFER_STAT)LDMGZ[i]);
            }

            //
            for (int i = 0; i < (int)EN_WAF_ID.EndOfId; i++)
            {
                cDEF.DM.WAF[i].SetWaferTo((EN_WAFER_STAT)WAFER[i]);
            }

            //
            cDEF.LOG.Trace("FormMap - Apply Click", true);
        }
        //------------------------------------------------------------------------
        private void pbMgz1_MouseDown(object sender, MouseEventArgs e)
        {
            //
            PictureBox pBox = (sender as PictureBox);
            int iTag        = Convert.ToInt32(pBox.Tag);
            int m_iSelRow   = -1;

            if ((iTag == 0) || (iTag == 1))
            {
                cDEF.DM.MGZ[iTag].GetImageRC(ref pBox, e.X, e.Y, out m_iSelRow);
            }

            if(LDMGZ[m_iSelRow] > 0) LDMGZ[m_iSelRow] = 0;


        }
        //------------------------------------------------------------------------
        public Label CreateLabel(string name, string text, int tag)
        {
            Label lbl       = new Label();
            lbl.Name        = name;
            lbl.Text        = text;
            lbl.Dock        = DockStyle.Top;
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Height      = 20;
            lbl.Tag         = tag; 
            lbl.Click      += Label_Click;

            return lbl;
        }
        //------------------------------------------------------------------------
        private void Label_Click(object sender, EventArgs e)
        {
            Label slb = sender as Label;
            int nTag = -1;
            int.TryParse(slb.Tag.ToString(), out nTag);

            if (nTag < 100) //Load
            {
                if (LDMGZ[nTag] == (int)EN_WAFER_STAT.Empty) LDMGZ[nTag] = (int)EN_WAFER_STAT.Mount;
                else                                       LDMGZ[nTag] = (int)EN_WAFER_STAT.Empty;
            } 
            else //Unload
            {
                if (UDMGZ[nTag-100] == (int)EN_WAFER_STAT.Empty) UDMGZ[nTag - 100] = (int)EN_WAFER_STAT.Mount;
                else                                           UDMGZ[nTag - 100] = (int)EN_WAFER_STAT.Empty;
            }


        }
        //------------------------------------------------------------------------
        private void btAllExist_Click(object sender, EventArgs e)
        {
            //
            Button sb = sender as Button;
            int nTag = -1; 
            int.TryParse(sb.Tag.ToString(), out nTag);

            switch (nTag)
            {
                case 0:
                    for (int i = 0; i < LDMGZ.Length; i++) LDMGZ[i] = (int)EN_WAFER_STAT.Mount;
                    break;
                case 1:
                    for (int i = 0; i < LDMGZ.Length; i++) LDMGZ[i] = (int)EN_WAFER_STAT.Empty;
                    break;
                case 2:
                    for (int i = 0; i < UDMGZ.Length; i++) UDMGZ[i] = (int)EN_WAFER_STAT.Mount;
                    break;
                case 3:
                    for (int i = 0; i < UDMGZ.Length; i++) UDMGZ[i] = (int)EN_WAFER_STAT.Empty;
                    break;
                case 4:
                    for (int i = 0; i < UDMGZ.Length; i++) LDMGZ[i] = (int)EN_WAFER_STAT.Mask;
                    break;
                case 5:
                    for (int i = 0; i < UDMGZ.Length; i++) UDMGZ[i] = (int)EN_WAFER_STAT.Mask;
                    break;


                default:
                    break;
            }


        }
        //------------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            //
            tmUpdate.Enabled = false;

            for (int i = 0; i < LDMGZ.Length; i++)
            {
                //if(LDMGZ[i]) lbMgzLD[i].BackColor = cDEF.FM.ProjOptn.cStatColor[(int)EN_WAFER_STAT.Mount];
                //else         lbMgzLD[i].BackColor = cDEF.FM.ProjOptn.cStatColor[(int)EN_WAFER_STAT.Empty];
                lbMgzLD[i].BackColor = cDEF.FM.ProjOptn.cStatColor[(int)LDMGZ[i]];
            }


            for (int i = 0; i < LDMGZ.Length; i++)
            {
                //if(UDMGZ[i]) lbMgzUD[i].BackColor = cDEF.FM.ProjOptn.cStatColor[(int)EN_WAFER_STAT.Mount];
                //else         lbMgzUD[i].BackColor = cDEF.FM.ProjOptn.cStatColor[(int)EN_WAFER_STAT.Empty];
                lbMgzUD[i].BackColor = cDEF.FM.ProjOptn.cStatColor[(int)UDMGZ[i]];
            }

            for (int i = 0; i < (int)EN_WAF_ID.EndOfId; i++)
            {
                //if(WAFER[i] == 0) 
            }

            //Wafer
            //cDEF.DM.WAF[(int)EN_WAF_ID.WTR_A].UpdateMap(ref pbWaf1, true, WAFER[(int)EN_WAF_ID.WTR_A]);
            //cDEF.DM.WAF[(int)EN_WAF_ID.WTR_B].UpdateMap(ref pbWaf2, true, WAFER[(int)EN_WAF_ID.WTR_B]);
            //cDEF.DM.WAF[(int)EN_WAF_ID.WAT  ].UpdateMap(ref pbWaf3, true, WAFER[(int)EN_WAF_ID.WAT  ]);
            //cDEF.DM.WAF[(int)EN_WAF_ID.WTB  ].UpdateMap(ref pbWaf4, true, WAFER[(int)EN_WAF_ID.WTB  ]);
           

            //
            tmUpdate.Enabled = true;
        }
        //------------------------------------------------------------------------
        private void pbWaf4_Click(object sender, EventArgs e)
        {
            //
            PictureBox pBox = (sender as PictureBox);
            int nTag = Convert.ToInt32(pBox.Tag);

            switch (nTag)
            {
                case (int)EN_WAF_ID.WTR_T: //
                case (int)EN_WAF_ID.WTR_B: //
                case (int)EN_WAF_ID.WAT  : //
                case (int)EN_WAF_ID.WTB  : //
                    //if(WAFER[nTag] != (int)EN_WAFER_STAT.Empty) WAFER[nTag] = (int)EN_WAFER_STAT.Empty;
                    WAFER[nTag] = (int)EN_WAFER_STAT.Empty;
                    break;

                default:
                    break;
            }

        }
        //------------------------------------------------------------------------
        private void btAllClear_Click(object sender, EventArgs e)
        {
            //All Map Clear
            if (!FRM.ShowMsg(true, " Warning ", "전체 Map을 삭제 하시겠습니까?", EN_MSG_KIND.UserModal)) return;

            for (int i = 0; i < LDMGZ.Length; i++) LDMGZ[i] = (int)EN_WAFER_STAT.Empty;
            for (int i = 0; i < UDMGZ.Length; i++) UDMGZ[i] = (int)EN_WAFER_STAT.Empty;

            for (int i = 0; i < (int)EN_WAF_ID.EndOfId; i++) WAFER[i] = (int)EN_WAFER_STAT.Empty;

            cDEF.DM.ClearMap();

            cDEF.LOG.Trace("FormMap - Data Init", true);

        }
        //------------------------------------------------------------------------
        private void btOK_Click(object sender, EventArgs e)
        {
            //
            Button sb = sender as Button;
            string sBtn = sb.Text;
            int nTag = Convert.ToInt32(sb.Tag);

            switch (nTag)
            {
                case 0: //가동 
                    tmUpdate.Enabled = false;
                    
                    break;
                
                case 1: //Retry
                    //Magazine 있으면...
                    if(cDEF.SEQ.WTR.ChkExistMgz(EN_MGZ_ID.LMZ))
                    {
                        cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ].SetTo(EN_WAFER_STAT.Mask);
                    }

                    break;

                default:
                    break;
            }

            cDEF.LOG.Trace($"User Map Check - {sBtn}", true);

            
            //
            cDEF.SEQ.Reset();
            cDEF.SEQ._bBtnManStart = true;

            m_bShow = false;

            //
            tmUpdate.Enabled = false;
            this.Hide();
        }
        //------------------------------------------------------------------------
        private void SetSacnCheckMode(bool On)
        {
            pnConfirm.Visible    =  On;
            
          //btExist   .Visible   = !On;
            btAllClear.Visible   = !On;
          //btExit    .Visible   = !On; 

            //
            //tlpLDMGZ.Enabled     = !On;
            //tlpUDMGZ.Enabled     = !On;

            if(On)
            {
                lbLDCnt.Text = cDEF.DM.MGZ[(int)EN_MGZ_ID.LMZ ].GetCntStat(EN_WAFER_STAT.Mount).ToString();
                //lbUDCnt.Text = cDEF.DM.MGZ[(int)EN_MGZ_ID.BUFF].GetCntStat(EN_WAFER_STAT.Mount).ToString();
            }
        }
        //------------------------------------------------------------------------
        public void ShowMapWindow(bool scan = false)
        {
            //
            if (m_bShow) return;
            
            m_bShow = true; 
            SetSacnCheckMode(scan);
            
            this.ShowDialog();

        }
    }
}
