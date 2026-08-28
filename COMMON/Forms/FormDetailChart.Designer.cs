namespace eMachine
{
    partial class FrmDetailChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.cbSelChart = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.cbSelChart);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 390);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(478, 60);
            this.panel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(521, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbSelChart
            // 
            this.cbSelChart.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbSelChart.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSelChart.FormattingEnabled = true;
            this.cbSelChart.Location = new System.Drawing.Point(120, 0);
            this.cbSelChart.Name = "cbSelChart";
            this.cbSelChart.Size = new System.Drawing.Size(176, 24);
            this.cbSelChart.TabIndex = 3;
            this.cbSelChart.SelectedIndexChanged += new System.EventHandler(this.cbSelChart_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(299, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(179, 60);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(120, 60);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Cart";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.IsValueShownAsLabel = true;
            series1.LabelBackColor = System.Drawing.Color.WhiteSmoke;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.Yes;
            series1.SmartLabelStyle.CalloutStyle = System.Windows.Forms.DataVisualization.Charting.LabelCalloutStyle.None;
            series1.SmartLabelStyle.IsOverlappedHidden = false;
            series1.SmartLabelStyle.MovingDirection = ((System.Windows.Forms.DataVisualization.Charting.LabelAlignmentStyles)((((System.Windows.Forms.DataVisualization.Charting.LabelAlignmentStyles.Right | System.Windows.Forms.DataVisualization.Charting.LabelAlignmentStyles.TopRight) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAlignmentStyles.BottomLeft) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAlignmentStyles.BottomRight)));
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(478, 390);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmDetailChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(478, 450);
            this.ControlBox = false;
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "FrmDetailChart";
            this.Text = "Detail Info";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDetailChart_FormClosing);
            this.Load += new System.EventHandler(this.FrmDetailChart_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbSelChart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
    }
}