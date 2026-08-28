namespace eMachine
{
	partial class FrmCtrlMotr
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
            this.sgMotor = new System.Windows.Forms.DataGridView();
            this.timerProc = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sgMotor)).BeginInit();
            this.SuspendLayout();
            // 
            // sgMotor
            // 
            this.sgMotor.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.sgMotor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgMotor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgMotor.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgMotor.Location = new System.Drawing.Point(0, 0);
            this.sgMotor.Margin = new System.Windows.Forms.Padding(2);
            this.sgMotor.Name = "sgMotor";
            this.sgMotor.RowTemplate.Height = 30;
            this.sgMotor.Size = new System.Drawing.Size(786, 110);
            this.sgMotor.TabIndex = 475;
            this.sgMotor.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgMotor_CellLeave);
            this.sgMotor.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseClick);
            this.sgMotor.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseDown);
            this.sgMotor.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseUp);
            this.sgMotor.SelectionChanged += new System.EventHandler(this.sgMotor_SelectionChanged);
            // 
            // timerProc
            // 
            this.timerProc.Interval = 10;
            this.timerProc.Tick += new System.EventHandler(this.timerProc_Tick);
            // 
            // FrmCtrlMotr
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.ClientSize = new System.Drawing.Size(786, 526);
            this.Controls.Add(this.sgMotor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmCtrlMotr";
            this.Text = "FrmCtrlMotr";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmCtrlMotr_FormClosed);
            this.Load += new System.EventHandler(this.FrmCtrlMotr_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmCtrlMotr_VisibleChanged);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmCtrlMotr_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.sgMotor)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView sgMotor;
		private System.Windows.Forms.Timer timerProc;
	}
}