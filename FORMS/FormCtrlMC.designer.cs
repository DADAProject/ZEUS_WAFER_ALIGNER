namespace eMachine
{
    partial class FrmCtrlMC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCtrlMC));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.KToggleButton(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.KToggleButton(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.KToggleButton(this.components);
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(137, 82);
            this.panel1.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.AutoCheck = false;
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnStart.Checked = false;
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btnStart.Font = new System.Drawing.Font("Impact", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.LedFullEnable = false;
            this.btnStart.LedVisible = false;
            this.btnStart.LedWidth = 20;
            this.btnStart.Location = new System.Drawing.Point(1, 1);
            this.btnStart.Name = "btnStart";
            this.btnStart.OffColor = System.Drawing.Color.Red;
            this.btnStart.OnColor = System.Drawing.Color.Lime;
            this.btnStart.RoundEdge = 10;
            this.btnStart.Size = new System.Drawing.Size(135, 80);
            this.btnStart.TabIndex = 1;
            this.btnStart.Tag = "1";
            this.btnStart.Text2 = "";
            this.btnStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStart.TextOff = "";
            this.btnStart.TextOn = "";
            this.btnStart.TextOnOffEnable = false;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.kToggleButton1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnStop);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(140, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(137, 82);
            this.panel2.TabIndex = 2;
            // 
            // btnStop
            // 
            this.btnStop.AutoCheck = false;
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnStop.Checked = false;
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btnStop.Font = new System.Drawing.Font("Impact", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.LedFullEnable = false;
            this.btnStop.LedVisible = false;
            this.btnStop.LedWidth = 10;
            this.btnStop.Location = new System.Drawing.Point(1, 1);
            this.btnStop.Name = "btnStop";
            this.btnStop.OffColor = System.Drawing.Color.Red;
            this.btnStop.OnColor = System.Drawing.Color.Lime;
            this.btnStop.RoundEdge = 10;
            this.btnStop.Size = new System.Drawing.Size(135, 80);
            this.btnStop.TabIndex = 1;
            this.btnStop.Tag = "2";
            this.btnStop.Text2 = "";
            this.btnStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStop.TextOff = "";
            this.btnStop.TextOn = "";
            this.btnStop.TextOnOffEnable = false;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.kToggleButton1_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnReset);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(277, 3);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(137, 82);
            this.panel3.TabIndex = 3;
            // 
            // btnReset
            // 
            this.btnReset.AutoCheck = false;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnReset.Checked = false;
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btnReset.Font = new System.Drawing.Font("Impact", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.LedFullEnable = false;
            this.btnReset.LedVisible = false;
            this.btnReset.LedWidth = 20;
            this.btnReset.Location = new System.Drawing.Point(1, 1);
            this.btnReset.Name = "btnReset";
            this.btnReset.OffColor = System.Drawing.Color.Red;
            this.btnReset.OnColor = System.Drawing.Color.Lime;
            this.btnReset.RoundEdge = 10;
            this.btnReset.Size = new System.Drawing.Size(135, 80);
            this.btnReset.TabIndex = 1;
            this.btnReset.Tag = "3";
            this.btnReset.Text2 = "";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReset.TextOff = "";
            this.btnReset.TextOn = "";
            this.btnReset.TextOnOffEnable = false;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.kToggleButton1_Click);
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // FrmCtrlMC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(416, 88);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCtrlMC";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCtrlMC";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCtrlMC_FormClosed);
            this.Load += new System.EventHandler(this.FormCtrlMC_Load);
            this.VisibleChanged += new System.EventHandler(this.FormCtrlMC_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.KToggleButton btnStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.KToggleButton btnStop;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.KToggleButton btnReset;
        private System.Windows.Forms.Timer tmProc;
    }
}