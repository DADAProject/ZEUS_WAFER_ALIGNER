namespace eMachine
{
    partial class FrmProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProgress));
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.pnPrggress = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbLoadingModule = new System.Windows.Forms.Label();
            this.pnTitle = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbPrjName = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.pnPrggress.SuspendLayout();
            this.pnTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // pnPrggress
            // 
            this.pnPrggress.BackColor = System.Drawing.Color.Transparent;
            this.pnPrggress.Controls.Add(this.progressBar1);
            this.pnPrggress.Controls.Add(this.lbLoadingModule);
            this.pnPrggress.Controls.Add(this.pnTitle);
            this.pnPrggress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnPrggress.Location = new System.Drawing.Point(3, 3);
            this.pnPrggress.Margin = new System.Windows.Forms.Padding(0);
            this.pnPrggress.Name = "pnPrggress";
            this.pnPrggress.Size = new System.Drawing.Size(642, 156);
            this.pnPrggress.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(51)))), ((int)(((byte)(64)))));
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 144);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(642, 12);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 2;
            // 
            // lbLoadingModule
            // 
            this.lbLoadingModule.BackColor = System.Drawing.Color.Transparent;
            this.lbLoadingModule.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbLoadingModule.Font = new System.Drawing.Font("Arial Narrow", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lbLoadingModule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(189)))));
            this.lbLoadingModule.Location = new System.Drawing.Point(0, 100);
            this.lbLoadingModule.Margin = new System.Windows.Forms.Padding(0);
            this.lbLoadingModule.Name = "lbLoadingModule";
            this.lbLoadingModule.Padding = new System.Windows.Forms.Padding(3);
            this.lbLoadingModule.Size = new System.Drawing.Size(642, 41);
            this.lbLoadingModule.TabIndex = 1;
            this.lbLoadingModule.Text = ".....";
            this.lbLoadingModule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnTitle
            // 
            this.pnTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.pnTitle.Controls.Add(this.pictureBox1);
            this.pnTitle.Controls.Add(this.lbPrjName);
            this.pnTitle.Controls.Add(this.lbVersion);
            this.pnTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTitle.Location = new System.Drawing.Point(0, 0);
            this.pnTitle.Name = "pnTitle";
            this.pnTitle.Size = new System.Drawing.Size(642, 100);
            this.pnTitle.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::eMachine.Properties.Resources.DADA;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(4, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(324, 95);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lbPrjName
            // 
            this.lbPrjName.BackColor = System.Drawing.Color.Transparent;
            this.lbPrjName.Font = new System.Drawing.Font("Arial", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPrjName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(200)))));
            this.lbPrjName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbPrjName.Location = new System.Drawing.Point(218, 5);
            this.lbPrjName.Name = "lbPrjName";
            this.lbPrjName.Size = new System.Drawing.Size(412, 70);
            this.lbPrjName.TabIndex = 0;
            this.lbPrjName.Text = "MACHINE NAME";
            this.lbPrjName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbVersion
            // 
            this.lbVersion.BackColor = System.Drawing.Color.Transparent;
            this.lbVersion.Font = new System.Drawing.Font("Arial Narrow", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(200)))));
            this.lbVersion.Location = new System.Drawing.Point(315, 70);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(311, 25);
            this.lbVersion.TabIndex = 1;
            this.lbVersion.Text = "VER : 1.0 ";
            this.lbVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmProgress
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(51)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(648, 162);
            this.ControlBox = false;
            this.Controls.Add(this.pnPrggress);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmProgress";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmProgress_Load);
            this.pnPrggress.ResumeLayout(false);
            this.pnTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.Panel pnPrggress;
        private System.Windows.Forms.Panel pnTitle;
        private System.Windows.Forms.Label lbLoadingModule;
        private System.Windows.Forms.Label lbPrjName;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}