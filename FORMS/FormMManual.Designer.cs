namespace eMachine
{
    partial class FrmMManual
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMManual));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnHandle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.sgSelPart = new System.Windows.Forms.DataGridView();
            this.pnBaseMan = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.sgMotor = new System.Windows.Forms.DataGridView();
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgMotor)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.panel1.Controls.Add(this.pnHandle);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.sgSelPart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1091, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(164, 879);
            this.panel1.TabIndex = 1371;
            // 
            // pnHandle
            // 
            this.pnHandle.BackColor = System.Drawing.Color.Transparent;
            this.pnHandle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnHandle.Location = new System.Drawing.Point(5, 618);
            this.pnHandle.Name = "pnHandle";
            this.pnHandle.Size = new System.Drawing.Size(154, 256);
            this.pnHandle.TabIndex = 1379;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(5, 187);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(154, 52);
            this.btnSave.TabIndex = 1377;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // sgSelPart
            // 
            this.sgSelPart.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgSelPart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgSelPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgSelPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgSelPart.Location = new System.Drawing.Point(5, 5);
            this.sgSelPart.Margin = new System.Windows.Forms.Padding(2);
            this.sgSelPart.Name = "sgSelPart";
            this.sgSelPart.RowTemplate.Height = 30;
            this.sgSelPart.Size = new System.Drawing.Size(154, 182);
            this.sgSelPart.TabIndex = 473;
            this.sgSelPart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgSelPart_CellClick);
            // 
            // pnBaseMan
            // 
            this.pnBaseMan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBaseMan.Location = new System.Drawing.Point(4, 459);
            this.pnBaseMan.Margin = new System.Windows.Forms.Padding(0);
            this.pnBaseMan.Name = "pnBaseMan";
            this.pnBaseMan.Size = new System.Drawing.Size(1070, 418);
            this.pnBaseMan.TabIndex = 1373;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.sgMotor);
            this.panel3.Location = new System.Drawing.Point(4, 5);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1070, 445);
            this.panel3.TabIndex = 1369;
            this.panel3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseUp);
            // 
            // sgMotor
            // 
            this.sgMotor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgMotor.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgMotor.Location = new System.Drawing.Point(0, 0);
            this.sgMotor.Margin = new System.Windows.Forms.Padding(2);
            this.sgMotor.Name = "sgMotor";
            this.sgMotor.RowTemplate.Height = 30;
            this.sgMotor.Size = new System.Drawing.Size(1068, 110);
            this.sgMotor.TabIndex = 474;
            this.sgMotor.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseDown);
            this.sgMotor.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseUp);
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // FrmMManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(1255, 879);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnBaseMan);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMManual";
            this.Text = "FrmMManual";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMManual_FormClosed);
            this.Load += new System.EventHandler(this.FrmMManual_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmMManual_VisibleChanged);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgMotor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView sgSelPart;
        private System.Windows.Forms.Panel pnBaseMan;
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView sgMotor;
		private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnHandle;
    }
}