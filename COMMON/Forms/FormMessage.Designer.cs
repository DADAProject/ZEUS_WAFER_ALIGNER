namespace eMachine
{
    partial class FrmMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMessage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.lbMsg = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.btnYes);
            this.panel1.Controls.Add(this.btnNo);
            this.panel1.Controls.Add(this.lbMsg);
            this.panel1.Controls.Add(this.lbTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(605, 206);
            this.panel1.TabIndex = 431;
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.Image = ((System.Drawing.Image)(resources.GetObject("btnYes.Image")));
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnYes.Location = new System.Drawing.Point(339, 149);
            this.btnYes.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnYes.Name = "btnYes";
            this.btnYes.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnYes.Size = new System.Drawing.Size(127, 53);
            this.btnYes.TabIndex = 432;
            this.btnYes.Text = "YES";
            this.btnYes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnYes.UseVisualStyleBackColor = false;
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo.Location = new System.Drawing.Point(469, 149);
            this.btnNo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo.Name = "btnNo";
            this.btnNo.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo.Size = new System.Drawing.Size(127, 53);
            this.btnNo.TabIndex = 433;
            this.btnNo.Text = "Close";
            this.btnNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.BackColor = System.Drawing.Color.White;
            this.lbMsg.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMsg.Location = new System.Drawing.Point(8, 36);
            this.lbMsg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(589, 108);
            this.lbMsg.TabIndex = 431;
            this.lbMsg.Text = ".....";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.DimGray;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Gold;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(605, 36);
            this.lbTitle.TabIndex = 421;
            this.lbTitle.Text = "MESSAGE";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseDown);
            this.lbTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseMove);
            // 
            // FrmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(605, 205);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmMessage";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMessage_FormClosed);
            this.Load += new System.EventHandler(this.FrmMessage_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
    }
}