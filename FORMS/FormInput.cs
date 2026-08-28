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
    public partial class FrmInput : Form
    {

        public int    m_iPageSel;
        public string m_sNewVal ;
        public string m_sOldVal ;


        public FrmInput()
        {
            InitializeComponent();

            //TabControl Tab 제거
            //this.Height -= 20;
            Rectangle Rect = new Rectangle(tabSub1.Left, tabSub1.Top, tabSub1.Width, tabSub1.Height);
            tabInput.Region = new Region(Rect);   
            
        }

        private void FrmInput_Load(object sender, EventArgs e)
        {
            Display();
        }


        private void FrmInput_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FrmInput_VisibleChanged(object sender, EventArgs e)
        {

        }

        public void Display  ()
        {
            switch (m_iPageSel) {
                default ://Normal
                         lbSubTitle1   .Text = "Change Input Value";
                         edCurrVal.Text = m_sOldVal;
                         edNewVal .Text = ""       ;
				         break;
                }

           tabInput.SelectedIndex = m_iPageSel;

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            cDEF.LOG.Trace($"INPUT {Btn.Text} Button Click {edCurrVal.Text}->{edNewVal.Text}");

            switch(tabInput.SelectedIndex) 
            {
                case 0: m_sNewVal = edNewVal .Text;
                        break;
            }  

	        Btn.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            cDEF.LOG.Trace($"INPUT {Btn.Text} Button Click");
	        Btn.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
