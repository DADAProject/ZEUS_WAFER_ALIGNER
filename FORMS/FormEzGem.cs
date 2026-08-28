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
    public partial class FrmEzGem : Form
    {
        public FrmEzGem()
        {
            InitializeComponent();
        }


        private void FormEzGem_Load(object sender, EventArgs e)
        {
            timerProc.Enabled = true;

        }

        private void FormEzGem_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerProc.Enabled = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timerProc_Tick(object sender, EventArgs e)
        {
            if(cDEF.GemDll._bShowMsg)
            {
                cDEF.GemDll._bShowMsg = false;
                m_lstMsg.Items.Add(cDEF.GemDll._sLastMsg);
                if(m_lstMsg.Items.Count> 100)
                {
                    m_lstMsg.Items.RemoveAt(0);
                }
            }
            lbConnectState   .Text = cDEF.GemDll.StrGemState    ();
            lbCommState      .Text = cDEF.GemDll.StrCommState   ();
            lbControlState   .Text = cDEF.GemDll.StrControlState();

            lDeviceID        .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nDeviceID    );
            lRetry           .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nRetry       );
            lLinkTest        .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nLinkInterval);
            lPort            .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nPort        );
            lT3              .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nT3          );
            lT5              .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nT5          );
            lT6              .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nT6          );
            lT7              .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nT7          );
            lT8              .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nT8          );
            lCTTime          .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nCTTime      );
            lbIP             .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_strIP        );
            lIdleTime        .Text = FNC.ConvStr(cDEF.GemDll.ECV.m_nIdleTime    );


            if(cDEF.GemDll.ECV.m_bPassive     ) rbPassive.Checked = true;
            else                                rbActive .Checked = true;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            m_lstMsg.Items.Clear();
        }

        private void btnXGemStart_Click(object sender, EventArgs e)
        {
            cDEF.GemDll.OnStart();


        }

        private void btnXGemStop_Click(object sender, EventArgs e)
        {
            cDEF.GemDll.OnStop();
        }

        private void btnOffline_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            cDEF.LOG.Trace("{0} Button Click", Btn.Text.ToString());
            switch(iTag)
            {
                default : break;
                case 1  : cDEF.GemDll.ReqControlStatusChange((int)EN_CONTROL_STATE.EQ_OFFLINE    );  break;
                case 2  : cDEF.GemDll.ReqControlStatusChange((int)EN_CONTROL_STATE.ONLINE_LOCAL  );  break;
                case 3  : cDEF.GemDll.ReqControlStatusChange((int)EN_CONTROL_STATE.ONLINE_REMOTE );  break;
            }
        }

        private void btnGEMSetECVChanged_Click(object sender, EventArgs e)
        {

        }

        private void btnTerminalMessage_Click(object sender, EventArgs e)
        {

        }

        private void btnAlarmDetect_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            long lAlarmNo = FNC.ConvInt(edAlarmNo.Text);
            cDEF.LOG.Trace("{0} Button Click", Btn.Text.ToString());
            switch(iTag)
            {
                default : break;
                case 1  : cDEF.GemDll.SetAlarm(lAlarmNo, (long)1);  break;
                case 2  : cDEF.GemDll.SetAlarm(lAlarmNo, (long)0);  break;
            }
        }

        private void btnEvent_Click(object sender, EventArgs e)
        {
            cDEF.GEM.SETVAL((EN_SVID)FNC.ConvInt(EVTVidNo1.Text),edEVTData1.Text, true);
            cDEF.GEM.SETVAL((EN_SVID)FNC.ConvInt(EVTVidNo2.Text),edEVTData2.Text);
            cDEF.GEM.SETVAL((EN_SVID)FNC.ConvInt(EVTVidNo3.Text),edEVTData3.Text);
            cDEF.GEM.SETVAL((EN_SVID)FNC.ConvInt(EVTVidNo4.Text),edEVTData4.Text);
            cDEF.GEM.SETVAL((EN_SVID)FNC.ConvInt(EVTVidNo5.Text),edEVTData5.Text);
            cDEF.GEM.SendCEID((EN_CEID)FNC.ConvInt(EVTVidNo5.Text));

        }
    }
}
