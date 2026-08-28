using System.Data;
using System.Windows.Forms;


namespace eMachine
{
    public partial class FormMIOSetting : Form
    {
        //Var
        DataTable dtInput  = new DataTable();
        DataTable dtOutput = new DataTable();


        public FormMIOSetting()
        {
            InitializeComponent();

        }
        //--------------------------------------------------------------------------
        private void SetGrid()
        {
            string sTemp, sTemp1;

            FNC.SetGridStyle(ref sgInput );
            FNC.SetGridStyle(ref sgOutput);

            //
            dtInput.TableName = "INPUT";
            dtInput.Columns.Add("NO");
            dtInput.Columns.Add("ADDR");
            dtInput.Columns.Add("NAME");
            dtInput.Columns.Add("INV", typeof(bool));

            //
            dtInput .Clear();
            for (int i = 0; i < cDEF.IO._iNumOfX; i++)
            {
                sTemp  = string.Format("X{0:X4}", cDEF.IO.XA[i]);
                sTemp1 = string.Format($"({cDEF.IO.sXA[i]}) {cDEF.IO.XComt[i]}");
                dtInput.Rows.Add(i+1, sTemp, sTemp1, cDEF.IO.XInv[i] == 1 ? true: false);
            }

            dtOutput.TableName = "OUTPUT";
            dtOutput.Columns.Add("NO");
            dtOutput.Columns.Add("ADDR");
            dtOutput.Columns.Add("NAME");
            dtOutput.Columns.Add("INV", typeof(bool));
            
            dtOutput.Clear();
            for (int i = 0; i < cDEF.IO._iNumOfY; i++)
            {
                sTemp  = string.Format("Y{0:X4}", cDEF.IO.YA[i]);
                sTemp1 = string.Format($"({cDEF.IO.sYA[i]}) {cDEF.IO.YComt[i]}");
                dtOutput.Rows.Add(i+1, sTemp, sTemp1, cDEF.IO.YInv[i] == 1 ? true: false);
            }

            sgInput.DataSource = dtInput;
            sgInput.Visible = true;

            sgOutput.DataSource = dtOutput;
            sgOutput.Visible = true;

            //
            int[] iWidth  = { 40, 60, 0, 40};
            int iTotWidth = 0;
            for (int i = 0; i < iWidth.Length; i++)
            {
                sgInput .Columns[i].Width = iWidth[i];
                sgOutput.Columns[i].Width = iWidth[i];
                iTotWidth += iWidth[i];
            }
            sgInput .Columns[2].Width = sgInput .Width - iTotWidth - 20;
            sgOutput.Columns[2].Width = sgOutput.Width - iTotWidth - 20;


        }
        //--------------------------------------------------------------------------
        private void FormMIOSetting_Load(object sender, System.EventArgs e)
        {
            //Set Grid
            SetGrid();

        }
        //--------------------------------------------------------------------------
        private void btClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        //--------------------------------------------------------------------------
        private void btSave_Click(object sender, System.EventArgs e)
        {

            for (int i = 0; i < cDEF.IO._iNumOfX; i++)
            {
                cDEF.IO.XInv[i] = dtInput.Rows[i]["INV"].ToString().ToUpper() == "TRUE" ? 1 : 0;
            }
            for (int i = 0; i < cDEF.IO._iNumOfY; i++)
            {
                cDEF.IO.YInv[i] = dtOutput.Rows[i]["INV"].ToString().ToUpper() == "TRUE" ? 1 : 0;
            }

            //
            cDEF.IO.Load(false);

        }
    }
}
