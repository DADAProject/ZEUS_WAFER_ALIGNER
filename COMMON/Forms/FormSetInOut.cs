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
    public partial class FrmSetInOut : Form
    {
        public FrmSetInOut()
        {
            InitializeComponent();
        }
        //--------------------------------------------------------------------------
        private void FrmSetInOut_Load(object sender, EventArgs e)
        {
            cDEF.IO.UpdateAddress(true, ref SGInputTable, ref SGOutputTable);
            UpdateStat(ref SGInputTable );
            UpdateStat(ref SGOutputTable);
        }
        //--------------------------------------------------------------------------
        private void FrmSetInOut_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        //--------------------------------------------------------------------------
        private void lbTitle_MouseDown(object sender, MouseEventArgs e)
        {
             var s = sender as Label;
             if(this == null) return;
             if(s    == null) return;
             s.Tag = new Point(e.X, e.Y);
        }
        //--------------------------------------------------------------------------
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
        //--------------------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)
        {
	        cDEF.IO.UpdateAddress(false, ref SGInputTable, ref SGOutputTable);
	        this.Close();
        }
        //--------------------------------------------------------------------------
        private void btnNo_Click(object sender, EventArgs e)
        {
            Close();
        }
        //--------------------------------------------------------------------------
        public void UpdateStat(ref System.Windows.Forms.DataGridView Grid)
        {
            int iDat;
            for(int i=0; i<Grid.RowCount ;i++) 
            {
                for(int j=1; j<Grid.ColumnCount ;j++) 
                {
                    if (Grid[j, i].Value.ToString() == "") continue;
                    iDat = FNC.ConvInt(Grid[j,i].Value.ToString(), -1);
                    Grid[j,i].Style.BackColor = (iDat>=0) ? Color.LightYellow : Color.Silver;
                }
            }
        }
        //--------------------------------------------------------------------------
        private void SGInputTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateStat(ref SGInputTable );
        }
        //--------------------------------------------------------------------------
        private void SGOutputTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateStat(ref SGOutputTable );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            int Ch = 0;
            if (iTag == 0)
                textBox1.Text = cDEF.IO.GetMemoryX(Convert.ToInt32(textBox1.Text), out Ch).ToString();
            else 
                textBox1.Text = cDEF.IO.GetMemoryY(Convert.ToInt32(textBox1.Text), out Ch).ToString();
        }
    }
}
