using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace eMachine
{

    public partial class FrmChart : Form
    {
		private const int MaxAxis  = 10;

		private ContextMenuStrip MenuStrip = new ContextMenuStrip();
        private List<double>   m_ListValueX = null;
		private List<double>[] m_ListValueY = new List<double>[MaxAxis];

		private Pen m_Pen;
		private Graphics m_g;
		private Rectangle m_ZoomRect;
		private bool m_bZoomNow;
		private bool m_bDrngZoom;

        public FrmChart()
        {
            InitializeComponent();

			chart2.Dock = DockStyle.Fill;
			chart2.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Milliseconds;
            chart2.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount; //FixedCount;
            chart2.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Milliseconds;
            chart2.ChartAreas[0].AxisX.Interval = 0;

			m_g = this.chart2.CreateGraphics();
			m_Pen = new Pen(Color.Black, 2);
			m_Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			m_bZoomNow  = false;
			m_bDrngZoom = false;

			openFileDialog1.InitialDirectory  = Application.StartupPath;
			saveFileDialog1.InitialDirectory  = Application.StartupPath;

            //Form Control Event
            FRM.SetChartValue1 += new FRM.SetCF1Handler(SetData   );
            FRM.SetChartValue2 += new FRM.SetCF2Handler(SetData   );
			FRM.ChartClear     += new FRM.SetCF3Handler(ClearChart);

			timer1.Enabled = true;
        }
		~FrmChart()
		{
			if (m_ListValueX  != null) m_ListValueX  = null;
			for (int n = 0; n < m_ListValueY.Length; n++)
			{
				if (m_ListValueY[n] != null) m_ListValueY[n] = null;
			}
			timer1.Enabled = false;
		}
		public int  GetAxisYCnt()
		{
			return chart2.Series.Count;
		}
		public void AddChartSeries(SeriesChartType ChartType, ChartValueType ValType, Color color, string Name)
		{
			int iSerieCnt = chart2.Series.Count;
			if (iSerieCnt <  0      ) return;
			if (iSerieCnt >= MaxAxis) return;
			//
			if (iSerieCnt == 0)
			{
				if (m_ListValueX  == null) m_ListValueX  = new List<double>();
			}
			//
			chart2.Series.Add(Name);
			//
			chart2.Series[iSerieCnt].ChartType   = ChartType;
			chart2.Series[iSerieCnt].Name        = Name     ;
			chart2.Series[iSerieCnt].XValueType  = ValType  ;
			chart2.Series[iSerieCnt].BorderColor = Color.Red;
			chart2.Series[iSerieCnt].Color       = color    ;
			chart2.Series[iSerieCnt].BorderWidth = 2        ;
			chart2.Series[iSerieCnt].MarkerStyle = MarkerStyle.Square;
			//chart2.Series[iSerieCnt].IsValueShownAsLabel = true;

			//			
			if (m_ListValueY[iSerieCnt] == null) m_ListValueY[iSerieCnt] = new List<double>();
			//
		}

		public void SetData(int Axis, double Value)
		{
			int    iLastIdx = 0;

			if (Axis < 0                   ) return;
			if (Axis >= chart2.Series.Count) return;			

  			try {
				
				//m_ListValueX.Add(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + DateTime.Now.ToString(" HH:mm:ss")).ToOADate());
				for (int n = 0; n < chart2.Series.Count; n++)
				{ 
					 if (n == Axis) m_ListValueY[n].Add(Value);
					 else
					 {
						 iLastIdx = m_ListValueY[n].Count;
						 if (iLastIdx > 0) m_ListValueY[n].Add(m_ListValueY[n][iLastIdx - 1]);
						 else              m_ListValueY[n].Add(0.0);
                     }
					 m_ListValueX.Add(DateTime.Now.ToOADate());
				}
				//
				UpdateSecondChart();
				}
			catch (Exception err)
			{
				System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
				return ;
			}
			//
			return ;
		}

		public void SetData(double[] Value)
		{
			if (chart2.Series.Count != Value.Length) return;

  			try {
				m_ListValueX.Add(DateTime.Now.ToOADate());
				//m_ListValueX.Add(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + DateTime.Now.ToString(" HH:mm:ss")).ToOADate());
				for (int n = 0; n < chart2.Series.Count; n++)
					 m_ListValueY[n].Add(Value[n]);
				//
				UpdateSecondChart();
				}
			catch (Exception err)
			{
				System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
				return ;
			}
			//
			return ;
		}

		public int GetLastXIndex() 
		{ 
			if (m_ListValueX.Count <= 0) return 0;
			return m_ListValueX.Count - 1; 
		}
		public double GetXValue(int Index)
		{
			if (Index < 0                  ) return 0.0;
			if (Index >= m_ListValueX.Count) return 0.0;

			return m_ListValueX[Index];
		}

		public void ClearChart()
		{
			m_ListValueX .Clear();
			//
			for (int n = 0; n < chart2.Series.Count; n++)
			{
				chart2.Series[n].Points.Clear();
				m_ListValueY[n].Clear();
			}
		}
		public void SelectPot(ref System.Windows.Forms.MouseEventArgs e)
		{
			//int iIdx = 0;
			int iSeriesIdx = -1;
			HitTestResult rslt = chart2.HitTest(e.X, e.Y);

			if (rslt.ChartElementType != ChartElementType.DataPoint) 
			{
				 for (int j = 0; j < chart2.Series.Count; j++) chart2.Series[j].BorderWidth = 2; 
				 return;
			}
			for (int n = 0; n < chart2.ChartAreas.Count; n++)
			{
				 if (rslt.ChartArea != chart2.ChartAreas[n]) continue;
				 for (int j = 0; j < chart2.Series.Count; j++)
				 {
				 	if (rslt.Series != chart2.Series[j]) chart2.Series[j].BorderWidth = 0; 
				 	else {
							chart2.Series[j].BorderWidth = 4;
							chart2.Series[j].ToolTip = string.Format("Index : {0}\n{1} : {2}", rslt.PointIndex + 1, chart2.Series[j].Name, m_ListValueY[j][rslt.PointIndex]);	
						}
				 	iSeriesIdx = j;
				 }
			}
		}

		private void SetZoom(bool On)
		{
			//
			m_bDrngZoom = On;

			if (On)
			{
				this.Cursor = Cursors.Hand;
				this.chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true; 
				this.chart2.ChartAreas[0].CursorX.IsUserEnabled = true; 
				this.chart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
			}
			else 
			{
				this.Cursor = Cursors.Arrow;
				chart2.ChartAreas[0].AxisX.ScaleView.ZoomReset();
				chart2.ChartAreas[0].AxisY.ScaleView.ZoomReset();
				this.chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = false; 
				this.chart2.ChartAreas[0].CursorX.IsUserEnabled = false; 
				this.chart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
			}
		}
		private void SaveChartXML(string FilePathName)
		{ //Serialize
			try {
				//string filePath = Application.StartupPath + "\\ChartData_Stream.xml";
				if (File.Exists(FilePathName))
				{
				    File.Copy(FilePathName, Application.StartupPath + "\\ChartData_Stream.bak", true);
				    File.Delete(FilePathName);
				}

				FileStream stream = new FileStream(FilePathName, FileMode.Create);

				chart2.Serializer.Content = SerializationContents.Default;
				chart2.Serializer.Format = System.Windows.Forms.DataVisualization.Charting.SerializationFormat.Xml;
				chart2.Serializer.Save(stream);

				stream.Close();
				}
			catch (Exception err) {
				System.Diagnostics.Debug.WriteLine("Exception:" + err.Message);
				MessageBox.Show("An exception occurred.\nPlease try again.");
				}
		}
		private void LoadChartXML(string FilePathName)
		{
            //string filePath = Application.StartupPath + "\\ChartData_Stream.xml";
            FileStream stream = new FileStream(FilePathName, FileMode.Open);
            chart2.Serializer.IsResetWhenLoading = true;
            chart2.Serializer.Load(stream);

            stream.Close();
		}
        private void UpdateSecondChart()
        {
			if (!this.Visible) return;

			try {
				//DateTime addTiime = DateTime.Now.AddSeconds(5);
				//chart2.ChartAreas[0].AxisX.Maximum = DateTime.Parse(addTiime.ToString("yyyy-MM-dd") + addTiime.ToString(" HH:mm:ss")).ToOADate();
				chart2.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(5).ToOADate();
				//			
				for (int n = 0; n < chart2.Series.Count; n++)
				{
					if (chart2.Series[n]       == null) continue;
					if (m_ListValueX.Count - 1 <  0   ) continue;
					chart2.Series[n].Points.AddXY(m_ListValueX[m_ListValueX.Count - 1], m_ListValueY[n][m_ListValueY[n].Count - 1]);
					//chart2.Series[n].Points[m_ListValueY[n].Count-1].Label = (m_ListValueY[n].Count).ToString();
				}
				//
				chart2.Invalidate();
				}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
			}
        }
		private void DrawZoomRect()
		{
			Pen pen = new Pen(Color.Red, 1.0f);
			pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			
			Rectangle screenRect = chart2.RectangleToScreen(m_ZoomRect);
			ControlPaint.DrawReversibleFrame(screenRect,  chart2.BackColor, FrameStyle.Thick); // , Dashed

			//chart2.Invalidate();
		}

		private void ZoomInToZoomRect()
		{
		    if (m_ZoomRect.Width == 0 || m_ZoomRect.Height == 0)
		        return;

			Rectangle r = m_ZoomRect;

			double dXStrt = chart2.ChartAreas[0].AxisX.PixelPositionToValue(r.Left );
			double dXFnsh = chart2.ChartAreas[0].AxisX.PixelPositionToValue(r.Right);

			chart2.ChartAreas[0].AxisX.ScaleView.Zoom(dXStrt, dXFnsh);
		}

        private void chart2_MouseDown(object sender, MouseEventArgs e)
        {
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			    return;

			this.Focus();
			//Test for Ctrl + Left Single Click to start displaying selection box
			if ((e.Button == MouseButtons.Left) && (e.Clicks == 1) &&
			        ((ModifierKeys & Keys.Control) != 0) && sender is Chart)
			{
				if (!m_bDrngZoom) return; 

  			    m_bZoomNow = true;
			    m_ZoomRect.Location = e.Location;
			    m_ZoomRect.Width = m_ZoomRect.Height = 0;

			    DrawZoomRect(); //Draw the new selection rect
			}
			else if(e.Button == MouseButtons.Right) 
			{  
				MenuStrip.ItemClicked -= new ToolStripItemClickedEventHandler(menuItem_Click);
				MenuStrip.Items.Clear(); 
				MenuStrip.Items.Add(m_bDrngZoom ? "Zoom Out" : "Zoom In"); 
				MenuStrip.Items.Add("Load Cahrt XML"					);
				MenuStrip.Items.Add("Save Cahrt XML"					);
				MenuStrip.Items.Add("Chart Clear" 					    );

				MenuStrip.Items[0].Tag = 0;
				MenuStrip.Items[1].Tag = 1;
				MenuStrip.Items[2].Tag = 2;
				MenuStrip.Items[3].Tag = 3;
				MenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(menuItem_Click);

				if (!MenuStrip.Visible) MenuStrip.Show((sender as Chart), new Point(e.X, e.Y));
			}			
			else { if (MenuStrip.Visible) MenuStrip.Hide(); }

			this.Focus();
        }
        private void menuItem_Click (object sender, ToolStripItemClickedEventArgs e)
        {
			int iTag = (int)e.ClickedItem.Tag;

			if (MenuStrip.Visible) MenuStrip.Hide();

			switch (iTag)
			{
				default: return;
				case 0 : SetZoom(!m_bDrngZoom); break;
				case 1 : 
						 if (openFileDialog1.ShowDialog() == DialogResult.OK)
						 {
						   LoadChartXML(openFileDialog1.FileName);
						 }
						 break;
				case 2 : 
						 if (saveFileDialog1.ShowDialog() == DialogResult.OK)
						 {
						     SaveChartXML(saveFileDialog1.FileName);
						 }
						 break;
				case 3:  
						 ClearChart();
						 break;
			} 
		}

        private void chart2_MouseUp(object sender, MouseEventArgs e)
        {
			if (!m_bDrngZoom) return;
			if (m_bZoomNow && e.Button == MouseButtons.Left)
			{
			    DrawZoomRect(); //Redraw the selection
			                    //rect, which erases it
			    if ((m_ZoomRect.Width != 0) && (m_ZoomRect.Height != 0))
			    {
			        //Just in case the selection was dragged from lower right to upper left
			        m_ZoomRect = new Rectangle(Math.Min(m_ZoomRect.Left, m_ZoomRect.Right),
			                Math.Min(m_ZoomRect.Top, m_ZoomRect.Bottom),
			                Math.Abs(m_ZoomRect.Width),
			                Math.Abs(m_ZoomRect.Height));
			        ZoomInToZoomRect(); //no Shift so Zoom in.
			    }
			    m_bZoomNow = false;

				this.Cursor = Cursors.Arrow;
			}
        }

        private void chart2_MouseMove(object sender, MouseEventArgs e)
        {
			//
			SelectPot(ref e);
			//
			if (!m_bDrngZoom) return;
			int X = e.X;
			int Y = e.Y;

			if (m_bZoomNow)
			{
			    DrawZoomRect(); //Redraw the old selection
			                    //rect, which erases it

				if (X > chart2.Width ) X = chart2.Width;
			    if (Y > chart2.Height) Y = chart2.Height;

			    m_ZoomRect.Width  = X - m_ZoomRect.Left;
			    m_ZoomRect.Height = Y - m_ZoomRect.Top;

			    DrawZoomRect(); //Draw the new selection rect
			}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
			timer1.Enabled = false;

			timer1.Enabled = true;
        }
    }
}
