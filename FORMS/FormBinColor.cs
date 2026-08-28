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
    public partial class FrmBinColor : Form
    {
        bool               actived       ; 
        public FrmBinColor()
        {
            InitializeComponent();
        }

        private void FrmBinColor_Load(object sender, EventArgs e)
        {

        }

        private void FrmBinColor_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FrmBinColor_VisibleChanged(object sender, EventArgs e)
        {

            if(this.Visible && !actived) 
            {//Show
                FormShow();
            }
            if(!this.Visible && actived) 
            {//Hide
            }
            actived = this.Visible;
          
        }

        public void FormShow()
        {
            SetWorkColorGrid(ref sgSelColorHole);
        }
        public void FormHide()
        {
        }
        public void FormSave()
        {
            cDEF.FM.ProjOptn.LoadColor(false);
        }

        public void FormUpdate()
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide(); //Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FormSave();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            //int iNo = 0;
            Random random = new Random();

            for (int n = 0; n < cDEF.FM.ProjOptn.cStatColor.Length; n++) {
                cDEF.FM.ProjOptn.cStatColor[n] = Color.FromArgb(random.Next(0,255),random.Next(0,255),random.Next(0,255));
                 }
            //
            FormShow();
        }

        public void SetWorkColorGrid(ref System.Windows.Forms.DataGridView Grid)
        {
            //
            String sTemp;
            String[] sItem  = {"Item", "Color"};
            //
            if(Grid == null) return;
            //
            int iMaxCnt = 0;
            int iSelGrid = Convert.ToInt32(Grid.Tag);
            //
            Grid.Rows.Clear();
            FNC.SetGridStyle(ref Grid, 40, true, true, false, DataGridViewSelectionMode.CellSelect);
            //
            Grid.MultiSelect                = false;            
            Grid.Dock                       = System.Windows.Forms.DockStyle.Fill;
            Grid.CellBorderStyle            = DataGridViewCellBorderStyle.Single;
            Grid.BackgroundColor            = Color.FromArgb(66, 72, 88);
            Grid.DefaultCellStyle.ForeColor = Color.Black;
            Grid.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            Grid.ColumnHeadersHeight        = 30;
            //
            for (int n = 0; n < sItem.Length; n++) 
            {
                Grid.Columns.Add(sItem[n], sItem[n]);
                Grid.Columns[n].Width = (Grid.Width / sItem.Length)-10;
                Grid.Columns[n].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (n == 1) Grid.Columns[n].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            //
            iMaxCnt = (int)EN_WAFER_STAT.EndOfId;

            for (int n = 0; n < iMaxCnt; n++)
            {
                if (n >= cDEF.FM.ProjOptn.cStatColor.Length) continue;
                sTemp = string.Format("{0}", Enum.GetName(typeof(EN_WAFER_STAT),n));
                //
                Grid.Rows.Add(sTemp, "");
            }
            //
            for (int n = 0; n < Grid.Rows.Count; n++) Grid[1, n].Style.BackColor = cDEF.FM.ProjOptn.cStatColor[n];
            //
            Grid.Visible = true;
        }

        private void sgSelColor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //
            DataGridView CurrGrid = (sender as DataGridView);
            ColorDialog colorDlg = new ColorDialog();
            int iSelGrid;
            //
            if (e.ColumnIndex != 1                             ) return;
            if (e.RowIndex >= (int)EN_WAFER_STAT.EndOfId         ) return;

            iSelGrid = Convert.ToInt32(CurrGrid.Tag);
            if (e.RowIndex >= cDEF.FM.ProjOptn.cStatColor.Length) return;
            //

            colorDlg.AllowFullOpen = true;
            colorDlg.AnyColor = true;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            { 
                CurrGrid[e.ColumnIndex , e.RowIndex].Value = colorDlg.Color.Name;
                CurrGrid[e.ColumnIndex , e.RowIndex].Style.BackColor = Color.FromArgb(colorDlg.Color.R, colorDlg.Color.G, colorDlg.Color.B);
                //
                cDEF.FM.ProjOptn.cStatColor[e.RowIndex] = Color.FromArgb(colorDlg.Color.R, colorDlg.Color.G, colorDlg.Color.B);
            }     
            CurrGrid.Refresh();
            CurrGrid.CurrentCell = null;
        }
    }
}
