namespace eMachine
{
    partial class FrmMMotion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMMotion));
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnMotr = new System.Windows.Forms.Panel();
            this.pnBaseMan = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cblPart = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnHandle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.sgSelPart = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.sgPos = new System.Windows.Forms.DataGridView();
            this.lbSelPart = new System.Windows.Forms.Label();
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgPos)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.panel3.Controls.Add(this.pnMotr);
            this.panel3.Location = new System.Drawing.Point(8, 7);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(435, 423);
            this.panel3.TabIndex = 435;
            // 
            // pnMotr
            // 
            this.pnMotr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMotr.Location = new System.Drawing.Point(0, 0);
            this.pnMotr.Name = "pnMotr";
            this.pnMotr.Padding = new System.Windows.Forms.Padding(3);
            this.pnMotr.Size = new System.Drawing.Size(435, 423);
            this.pnMotr.TabIndex = 475;
            // 
            // pnBaseMan
            // 
            this.pnBaseMan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.pnBaseMan.Location = new System.Drawing.Point(7, 433);
            this.pnBaseMan.Margin = new System.Windows.Forms.Padding(0);
            this.pnBaseMan.Name = "pnBaseMan";
            this.pnBaseMan.Size = new System.Drawing.Size(1076, 444);
            this.pnBaseMan.TabIndex = 1365;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.pnHandle);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.sgSelPart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1091, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(164, 879);
            this.panel1.TabIndex = 1366;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cblPart);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(5, 239);
            this.panel5.Margin = new System.Windows.Forms.Padding(5);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(1, 5, 1, 0);
            this.panel5.Size = new System.Drawing.Size(154, 259);
            this.panel5.TabIndex = 1382;
            // 
            // cblPart
            // 
            this.cblPart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.cblPart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cblPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.cblPart.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cblPart.ForeColor = System.Drawing.SystemColors.Window;
            this.cblPart.FormattingEnabled = true;
            this.cblPart.Items.AddRange(new object[] {
            "cbAligh"});
            this.cblPart.Location = new System.Drawing.Point(1, 38);
            this.cblPart.Name = "cblPart";
            this.cblPart.Size = new System.Drawing.Size(152, 198);
            this.cblPart.TabIndex = 0;
            this.cblPart.SelectedIndexChanged += new System.EventHandler(this.cblPart_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Snow;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(1, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add Part";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 467);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(154, 151);
            this.panel4.TabIndex = 1381;
            // 
            // pnHandle
            // 
            this.pnHandle.BackColor = System.Drawing.Color.Transparent;
            this.pnHandle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnHandle.Location = new System.Drawing.Point(5, 618);
            this.pnHandle.Name = "pnHandle";
            this.pnHandle.Size = new System.Drawing.Size(154, 256);
            this.pnHandle.TabIndex = 1380;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(5, 187);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(154, 52);
            this.btnSave.TabIndex = 1368;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseUp);
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
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.panel2.Controls.Add(this.sgPos);
            this.panel2.Controls.Add(this.lbSelPart);
            this.panel2.Location = new System.Drawing.Point(452, 7);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(631, 422);
            this.panel2.TabIndex = 1368;
            // 
            // sgPos
            // 
            this.sgPos.AllowUserToAddRows = false;
            this.sgPos.AllowUserToDeleteRows = false;
            this.sgPos.AllowUserToResizeColumns = false;
            this.sgPos.AllowUserToResizeRows = false;
            this.sgPos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgPos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgPos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgPos.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgPos.Location = new System.Drawing.Point(3, 34);
            this.sgPos.Margin = new System.Windows.Forms.Padding(2);
            this.sgPos.Name = "sgPos";
            this.sgPos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.sgPos.RowTemplate.Height = 30;
            this.sgPos.Size = new System.Drawing.Size(625, 110);
            this.sgPos.TabIndex = 473;
            this.sgPos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgPos_CellClick);
            this.sgPos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgPos_CellDoubleClick);
            // 
            // lbSelPart
            // 
            this.lbSelPart.BackColor = System.Drawing.Color.SlateGray;
            this.lbSelPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSelPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbSelPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSelPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.lbSelPart.Location = new System.Drawing.Point(3, 3);
            this.lbSelPart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSelPart.Name = "lbSelPart";
            this.lbSelPart.Size = new System.Drawing.Size(625, 31);
            this.lbSelPart.TabIndex = 472;
            this.lbSelPart.Text = "FWD";
            this.lbSelPart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // FrmMMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(1255, 879);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnBaseMan);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMMotion";
            this.Text = "FrmMMotion";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMMotion_FormClosed);
            this.Load += new System.EventHandler(this.FrmMMotion_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmMMotion_VisibleChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmMMotion_Paint);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgPos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnBaseMan;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView sgSelPart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView sgPos;
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.Label lbSelPart;
        private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Panel pnMotr;
        private System.Windows.Forms.Panel pnHandle;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckedListBox cblPart;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
    }
}