namespace eMachine{
    partial class FrmUpdateInfo {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
        components.Dispose();
        }
        base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpdateInfo));
            this.btnNo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lBoxUpdate = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo.Location = new System.Drawing.Point(542, 248);
            this.btnNo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo.Name = "btnNo";
            this.btnNo.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo.Size = new System.Drawing.Size(147, 40);
            this.btnNo.TabIndex = 436;
            this.btnNo.Text = "Close";
            this.btnNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DimGray;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(-4, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 26);
            this.label2.TabIndex = 438;
            this.label2.Text = "VERSION";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbVersion
            // 
            this.lbVersion.BackColor = System.Drawing.Color.Gainsboro;
            this.lbVersion.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVersion.ForeColor = System.Drawing.Color.Black;
            this.lbVersion.Location = new System.Drawing.Point(178, 2);
            this.lbVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(511, 26);
            this.lbVersion.TabIndex = 439;
            this.lbVersion.Text = "1.1.1";
            this.lbVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lBoxUpdate
            // 
            this.lBoxUpdate.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lBoxUpdate.FormattingEnabled = true;
            this.lBoxUpdate.Location = new System.Drawing.Point(0, 30);
            this.lBoxUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.lBoxUpdate.Name = "lBoxUpdate";
            this.lBoxUpdate.Size = new System.Drawing.Size(689, 212);
            this.lBoxUpdate.TabIndex = 440;
            // 
            // FrmUpdateInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 292);
            this.ControlBox = false;
            this.Controls.Add(this.lBoxUpdate);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnNo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmUpdateInfo";
            this.Text = "PROGRAM UPDATE INFORMATION";
            this.Load += new System.EventHandler(this.FormUpdateInfo_Load);
            this.VisibleChanged += new System.EventHandler(this.FormUpdateInfo_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.ListBox lBoxUpdate;
    }
}