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
    public partial class FrmSTool : Form
    {
        bool actived;
        public FrmSTool()
        {
            InitializeComponent();
        }

        private void FormTool_Load(object sender, EventArgs e)
        {

        }

        private void FormTool_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FormTool_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible) 
            {//Show
                //if(actived) return; 
                FormShow();  
                timerProc.Enabled = true;
            }
            else
            {//Hide
                //if(!actived) return; 
                timerProc.Enabled = false;
            }
            actived = Visible;

        }     

        public void FormShow()
        {
            //cDEF.FM.ProjTool.UpdateByGrid(true, ref sgTool);
        }

        public void FormSave()
        {
            //cDEF.FM.ProjTool.UpdateByGrid(false, ref sgTool);
        }

        public void FormUpdate()
        {


        }

        private void sgTool_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex       < 1) return;
            if (e.ColumnIndex    < 0) return;
            /* Cell Merge시 값 지우기 
            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex)) {
              e.Value = "";
              e.FormattingApplied = true; 
            }
            */
        }

        private void sgTool_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView CurrGrid = (sender as DataGridView);
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;  
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
              return;

            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex)) {
              e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;  
            } else {
              e.AdvancedBorderStyle.Top = CurrGrid.AdvancedCellBorderStyle.Top;  
            }
        }

       bool IsTheSameCellValue(int column, int row) {

         DataGridViewCell cell1 = sgTool[column, row];
         DataGridViewCell cell2 = sgTool[column, row - 1];

         if (cell1.Value == null || cell2.Value == null) {
           return false;
         }

         if(cell1.Value.ToString().Length < 10) return false;
         if(cell2.Value.ToString().Length < 10) return false;

         if (cell1.Value.ToString().Substring(1,10) == cell2.Value.ToString().Substring(1,10)) {
           return true;
         } else {
           return false;
         }
       }

       private void timerProc_Tick(object sender, EventArgs e)
       {
            if(!this.Visible) {this.timerProc.Enabled = false; return; }

           //cDEF.FM.ProjTool.UpdateVacStatByGrid(ref sgTool);
       }

       private void sgTool_CellClick(object sender, DataGridViewCellEventArgs e)
       {
            DataGridView CurrGrid = (sender as DataGridView);

            if(CurrGrid.CurrentCell == null) return;

            int iGridR = e.RowIndex   ; 
            int iGridC = e.ColumnIndex;
               
            String sCellDat = CurrGrid[iGridC,iGridR].Value.ToString().Trim().ToUpper();
            if(sCellDat.ToUpper() == "TRUE" ) { CurrGrid[iGridC,iGridR].Value = "False";  CurrGrid.Rows[iGridR].ReadOnly = false; }   
            if(sCellDat.ToUpper() == "FALSE") { CurrGrid[iGridC,iGridR].Value = "True" ;  CurrGrid.Rows[iGridR].ReadOnly = false; }
       }

    }
}
