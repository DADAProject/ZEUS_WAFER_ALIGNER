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
			this.sgMotor = new System.Windows.Forms.DataGridView();
			this.pnBaseMan = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.sgSelPart = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.sgPos = new System.Windows.Forms.DataGridView();
			this.lbSelPart = new System.Windows.Forms.Label();
			this.tmProc = new System.Windows.Forms.Timer(this.components);
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sgMotor)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sgPos)).BeginInit();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.pnMotr);
			this.panel3.Controls.Add(this.sgMotor);
			this.panel3.Location = new System.Drawing.Point(7, 7);
			this.panel3.Margin = new System.Windows.Forms.Padding(2);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(449, 467);
			this.panel3.TabIndex = 435;
			// 
			// pnMotr
			// 
			this.pnMotr.Location = new System.Drawing.Point(0, 201);
			this.pnMotr.Name = "pnMotr";
			this.pnMotr.Size = new System.Drawing.Size(447, 264);
			this.pnMotr.TabIndex = 475;
			// 
			// sgMotor
			// 
			this.sgMotor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.sgMotor.Location = new System.Drawing.Point(0, 0);
			this.sgMotor.Margin = new System.Windows.Forms.Padding(2);
			this.sgMotor.Name = "sgMotor";
			this.sgMotor.RowTemplate.Height = 30;
			this.sgMotor.Size = new System.Drawing.Size(447, 110);
			this.sgMotor.TabIndex = 474;
			this.sgMotor.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseClick);
			this.sgMotor.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseDown);
			this.sgMotor.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sgMotor_CellMouseUp);
			this.sgMotor.SelectionChanged += new System.EventHandler(this.sgMotor_SelectionChanged);
			// 
			// pnBaseMan
			// 
			this.pnBaseMan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnBaseMan.Location = new System.Drawing.Point(5, 476);
			this.pnBaseMan.Margin = new System.Windows.Forms.Padding(0);
			this.pnBaseMan.Name = "pnBaseMan";
			this.pnBaseMan.Size = new System.Drawing.Size(1108, 429);
			this.pnBaseMan.TabIndex = 1365;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.SlateGray;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.btnReset);
			this.panel1.Controls.Add(this.btnStop);
			this.panel1.Controls.Add(this.btnStart);
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Controls.Add(this.sgSelPart);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(1115, 6);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(160, 900);
			this.panel1.TabIndex = 1366;
			// 
			// btnReset
			// 
			this.btnReset.BackColor = System.Drawing.Color.Silver;
			this.btnReset.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnReset.Font = new System.Drawing.Font("Impact", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnReset.ForeColor = System.Drawing.Color.DimGray;
			this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
			this.btnReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnReset.Location = new System.Drawing.Point(1, 831);
			this.btnReset.Margin = new System.Windows.Forms.Padding(2);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(155, 65);
			this.btnReset.TabIndex = 1371;
			this.btnReset.Tag = "3";
			this.btnReset.Text = "RESET";
			this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnReset.UseVisualStyleBackColor = false;
			this.btnReset.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.BackColor = System.Drawing.Color.Silver;
			this.btnStop.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStop.Font = new System.Drawing.Font("Impact", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStop.ForeColor = System.Drawing.Color.DimGray;
			this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
			this.btnStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnStop.Location = new System.Drawing.Point(1, 764);
			this.btnStop.Margin = new System.Windows.Forms.Padding(2);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(155, 65);
			this.btnStop.TabIndex = 1370;
			this.btnStop.Tag = "2";
			this.btnStop.Text = "STOP";
			this.btnStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStart
			// 
			this.btnStart.BackColor = System.Drawing.Color.Silver;
			this.btnStart.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStart.Font = new System.Drawing.Font("Impact", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStart.ForeColor = System.Drawing.Color.DimGray;
			this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
			this.btnStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnStart.Location = new System.Drawing.Point(1, 697);
			this.btnStart.Margin = new System.Windows.Forms.Padding(2);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(155, 65);
			this.btnStart.TabIndex = 1369;
			this.btnStart.Tag = "1";
			this.btnStart.Text = "START";
			this.btnStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnStart.UseVisualStyleBackColor = false;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSave.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
			this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSave.Location = new System.Drawing.Point(0, 202);
			this.btnSave.Margin = new System.Windows.Forms.Padding(2);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(158, 52);
			this.btnSave.TabIndex = 1368;
			this.btnSave.Text = "SAVE";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseUp);
			// 
			// sgSelPart
			// 
			this.sgSelPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.sgSelPart.Dock = System.Windows.Forms.DockStyle.Top;
			this.sgSelPart.Location = new System.Drawing.Point(0, 20);
			this.sgSelPart.Margin = new System.Windows.Forms.Padding(2);
			this.sgSelPart.Name = "sgSelPart";
			this.sgSelPart.RowTemplate.Height = 30;
			this.sgSelPart.Size = new System.Drawing.Size(158, 182);
			this.sgSelPart.TabIndex = 473;
			this.sgSelPart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgSelPart_CellClick);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.SlateGray;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label1.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(158, 20);
			this.label1.TabIndex = 472;
			this.label1.Text = "SUB MENU";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.sgPos);
			this.panel2.Controls.Add(this.lbSelPart);
			this.panel2.Location = new System.Drawing.Point(460, 7);
			this.panel2.Margin = new System.Windows.Forms.Padding(2);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(651, 466);
			this.panel2.TabIndex = 1368;
			// 
			// sgPos
			// 
			this.sgPos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.sgPos.Dock = System.Windows.Forms.DockStyle.Top;
			this.sgPos.Location = new System.Drawing.Point(0, 31);
			this.sgPos.Margin = new System.Windows.Forms.Padding(2);
			this.sgPos.Name = "sgPos";
			this.sgPos.RowTemplate.Height = 30;
			this.sgPos.Size = new System.Drawing.Size(649, 110);
			this.sgPos.TabIndex = 473;
			this.sgPos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgPos_CellClick);
			this.sgPos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgPos_CellDoubleClick);
			// 
			// lbSelPart
			// 
			this.lbSelPart.BackColor = System.Drawing.Color.SlateGray;
			this.lbSelPart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbSelPart.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbSelPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lbSelPart.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbSelPart.ForeColor = System.Drawing.Color.Black;
			this.lbSelPart.Location = new System.Drawing.Point(0, 0);
			this.lbSelPart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lbSelPart.Name = "lbSelPart";
			this.lbSelPart.Size = new System.Drawing.Size(649, 31);
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
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1280, 914);
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
			((System.ComponentModel.ISupportInitialize)(this.sgMotor)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).EndInit();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.sgPos)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView sgMotor;
        private System.Windows.Forms.Panel pnBaseMan;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView sgSelPart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView sgPos;
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.Label lbSelPart;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Panel pnMotr;
	}
}