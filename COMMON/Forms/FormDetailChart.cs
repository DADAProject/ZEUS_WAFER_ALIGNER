using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace eMachine
{
    public partial class FrmDetailChart : Form
    {
        int             m_iWndType  ; //0 : Panel에 붙히기, 1: 새로운 창에 띄우기.
        int             m_iValueType;
        SeriesChartType m_ChartType ;
        Timer           m_TimerFI   = new Timer();
        
        public FrmDetailChart()
        {
            InitializeComponent();
            //
            this.Opacity = 0;  //first the opacity is 0
        }
        public FrmDetailChart(int WndType, int ValueType, SeriesChartType ChartType)
        {
            InitializeComponent();
            //
            this.Opacity = 0;  //first the opacity is 0
            //
            m_iWndType   = WndType  ;
            m_iValueType = ValueType;
            m_ChartType  = ChartType;
        }

        private void FrmDetailChart_Load(object sender, EventArgs e)
        {
            if (m_iWndType == 1)
            {
                //Fade In.            
                m_TimerFI.Interval = 10; //we'll increase the opacity every 10ms
                m_TimerFI.Tick += new EventHandler(FadeInForm); //this calls the function that changes opacity
                m_TimerFI.Enabled = true;
                m_TimerFI.Start();
            }
            //
            //SetChartValue(m_iValueType, m_ChartType);
            //
            timer1.Enabled = true;
        }
        private void FrmDetailChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_iWndType == 1)
            {
                e.Cancel = true;    //cancel the event so the form won't be closed

                m_TimerFI.Tick += new EventHandler(fadeOutForm);  //this calls the fade out function
                m_TimerFI.Enabled = true;
                m_TimerFI.Start();

                if (this.Opacity == 0)  //if the form is completly transparent
                    e.Cancel = false;   //resume the event - the program can be closed
            }
        }
        protected override void WndProc(ref Message m)
        {
            //if (!this.Visible) return;
            int wParam = (int)m.WParam;
            switch (wParam) 
            { 
                case 0:
                    break;
                default :
                    base.WndProc(ref m);
                    break;
            }            
        }
        private void FadeInForm(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)  
            {
                m_TimerFI.Stop();   //this stops the timer if the form is completely displayed
                m_TimerFI.Tick -= new EventHandler(FadeInForm); 
            }
            else
                this.Opacity += 0.05;
        }
        private void fadeOutForm(object sender, EventArgs e)
        {
            if (this.Opacity <= 0)     //check if opacity is 0
            {
                m_TimerFI.Stop();    //if it is, we stop the timer
                m_TimerFI.Tick -= new EventHandler(fadeOutForm);
                Close();   //and we try to close the form
            }
            else
                this.Opacity -= 0.05;
        }
        private void cbSelChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)cbSelChart.SelectedIndex;
            chart1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.Close();
        }
        private void SetChartValue(int ValueType, SeriesChartType ChartType)
        {
            //
            chart1.Series[0].Points.Clear();

            switch (m_iValueType)
            {
                case 0 : //Detail PVI Qty.
                         //chart1.Series[0].Points.Add(cDEF.LOT.WafQty.iLoadPVICnt[0]); chart1.Series[0].Points[0].AxisLabel = "PVI Good";
                         //chart1.Series[0].Points.Add(cDEF.LOT.WafQty.iLoadPVICnt[1]); chart1.Series[0].Points[1].AxisLabel = "PVI Fail";
                         break;

                case 10: //Detail Yield.
                         chart1.Series[0].LabelFormat = "{0.00%}"; //"{0.#%}"
                         break;

                case 20: //Time.
                         break;

                case 30: //JAM.
                         break;
            }
            //
            chart1.Series[0].ChartType = ChartType;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (!this.Visible) return;
            //
            switch (m_iValueType)
            {
                case 0 : //Detail PVI Qty.
                         //chart1.Series[0].Points[0].SetValueY(cDEF.LOT.WafQty.iLoadPVICnt[0]); 
                         //chart1.Series[0].Points[1].SetValueY(cDEF.LOT.WafQty.iLoadPVICnt[1]); 
                         break;

                case 10: break;
                case 20: 
                         //chart1.Series[0].Points[0].SetValueY(cDEF.SPC.DAILY_DATA.dRunTime  * 10000); 
                         //chart1.Series[0].Points[1].SetValueY(cDEF.SPC.DAILY_DATA.dErrorTime* 10000); 
                         //chart1.Series[0].Points[2].SetValueY(cDEF.SPC.DAILY_DATA.dDownTime * 10000); 
                         //chart1.Series[0].Points[3].SetValueY(cDEF.SPC.DAILY_DATA.dIdleTime * 10000);
                         break;
            }
            chart1.Invalidate();

            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //cDEF.LOT.WafQty.iLoadPVICnt[1]++;
        }
    }
}
