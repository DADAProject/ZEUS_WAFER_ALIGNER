namespace eMachine
{
    partial class FrmSTool
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
            this.timerProc = new System.Windows.Forms.Timer(this.components);
            this.sgTool = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.sgTool)).BeginInit();
            this.SuspendLayout();
            // 
            // timerProc
            // 
            this.timerProc.Tick += new System.EventHandler(this.timerProc_Tick);
            // 
            // sgTool
            // 
            this.sgTool.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.sgTool.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgTool.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgTool.Location = new System.Drawing.Point(0, 0);
            this.sgTool.Margin = new System.Windows.Forms.Padding(2);
            this.sgTool.Name = "sgTool";
            this.sgTool.RowTemplate.Height = 30;
            this.sgTool.Size = new System.Drawing.Size(1064, 182);
            this.sgTool.TabIndex = 426;
            // 
            // FrmSTool
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(1064, 464);
            this.Controls.Add(this.sgTool);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmSTool";
            this.Text = "FrmSTool";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTool_FormClosed);
            this.Load += new System.EventHandler(this.FormTool_Load);
            this.VisibleChanged += new System.EventHandler(this.FormTool_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.sgTool)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerProc;
        private System.Windows.Forms.DataGridView sgTool;
    }
}