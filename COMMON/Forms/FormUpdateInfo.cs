using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine {
    public partial class FrmUpdateInfo : Form {
        
        public FrmUpdateInfo() 
        {
            InitializeComponent();
        }
        //--------------------------------------------------------------------------
        private void FormUpdateInfo_Load(object sender, EventArgs e) 
        {
            lbVersion.Text = cDEF.FM._sVersion;
            lBoxUpdate.Items.Clear();


            //for (int i = 0; i < vDEF.MAX_UPDATE_INFO; i++) 
            //{
            //    if (cDEF.FM.m_sUpInform[i] == "" || cDEF.FM.m_sUpInform[i] == null) break;
            //    lBoxUpdate.Items.Add ( cDEF.FM.m_sUpInform[i] );
            //}

            //
            lBoxUpdate.Items.Add(cDEF.FM.m_sUpInform[0]);
            for (int i = vDEF.MAX_UPDATE_INFO-1; i > 0 ; i--) 
            {
                if (cDEF.FM.m_sUpInform[i] == "" || cDEF.FM.m_sUpInform[i] == null) continue;
                lBoxUpdate.Items.Add (cDEF.FM.m_sUpInform[i]);
            }
        }
        //--------------------------------------------------------------------------
        private void FormUpdateInfo_VisibleChanged(object sender, EventArgs e) 
        {

        }
        //--------------------------------------------------------------------------       
        private void btnNo_Click(object sender, EventArgs e) 
        {
            Close();
        }
    }
}
