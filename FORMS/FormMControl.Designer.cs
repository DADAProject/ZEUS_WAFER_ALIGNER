namespace eMachine
{
	partial class FrmMControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMControl));
            this.tabMenu = new System.Windows.Forms.TabControl();
            this.tpgMenu1 = new System.Windows.Forms.TabPage();
            this.pnStatus = new System.Windows.Forms.Panel();
            this.tpgMenu2 = new System.Windows.Forms.TabPage();
            this.tpgMenu3 = new System.Windows.Forms.TabPage();
            this.tpgMenu4 = new System.Windows.Forms.TabPage();
            this.tpgMenu5 = new System.Windows.Forms.TabPage();
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnHandle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.sgSelPart = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tabMenu.SuspendLayout();
            this.tpgMenu1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMenu
            // 
            this.tabMenu.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabMenu.Controls.Add(this.tpgMenu1);
            this.tabMenu.Controls.Add(this.tpgMenu2);
            this.tabMenu.Controls.Add(this.tpgMenu3);
            this.tabMenu.Controls.Add(this.tpgMenu4);
            this.tabMenu.Controls.Add(this.tpgMenu5);
            this.tabMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabMenu.ItemSize = new System.Drawing.Size(100, 20);
            this.tabMenu.Location = new System.Drawing.Point(0, 0);
            this.tabMenu.Margin = new System.Windows.Forms.Padding(2);
            this.tabMenu.Name = "tabMenu";
            this.tabMenu.SelectedIndex = 0;
            this.tabMenu.ShowToolTips = true;
            this.tabMenu.Size = new System.Drawing.Size(1083, 879);
            this.tabMenu.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabMenu.TabIndex = 1360;
            // 
            // tpgMenu1
            // 
            this.tpgMenu1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.tpgMenu1.Controls.Add(this.pnStatus);
            this.tpgMenu1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tpgMenu1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpgMenu1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tpgMenu1.Location = new System.Drawing.Point(4, 4);
            this.tpgMenu1.Margin = new System.Windows.Forms.Padding(2);
            this.tpgMenu1.Name = "tpgMenu1";
            this.tpgMenu1.Padding = new System.Windows.Forms.Padding(2);
            this.tpgMenu1.Size = new System.Drawing.Size(1075, 851);
            this.tpgMenu1.TabIndex = 0;
            this.tpgMenu1.Text = "tpgMenu1";
            // 
            // pnStatus
            // 
            this.pnStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.pnStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnStatus.Location = new System.Drawing.Point(2, 2);
            this.pnStatus.Name = "pnStatus";
            this.pnStatus.Padding = new System.Windows.Forms.Padding(5);
            this.pnStatus.Size = new System.Drawing.Size(1071, 847);
            this.pnStatus.TabIndex = 0;
            // 
            // tpgMenu2
            // 
            this.tpgMenu2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.tpgMenu2.Location = new System.Drawing.Point(4, 4);
            this.tpgMenu2.Margin = new System.Windows.Forms.Padding(2);
            this.tpgMenu2.Name = "tpgMenu2";
            this.tpgMenu2.Padding = new System.Windows.Forms.Padding(5);
            this.tpgMenu2.Size = new System.Drawing.Size(1104, 851);
            this.tpgMenu2.TabIndex = 1;
            this.tpgMenu2.Text = "tpgMenu2";
            // 
            // tpgMenu3
            // 
            this.tpgMenu3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.tpgMenu3.Location = new System.Drawing.Point(4, 4);
            this.tpgMenu3.Margin = new System.Windows.Forms.Padding(2);
            this.tpgMenu3.Name = "tpgMenu3";
            this.tpgMenu3.Padding = new System.Windows.Forms.Padding(5);
            this.tpgMenu3.Size = new System.Drawing.Size(1104, 851);
            this.tpgMenu3.TabIndex = 2;
            this.tpgMenu3.Text = "tpgMenu3";
            // 
            // tpgMenu4
            // 
            this.tpgMenu4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.tpgMenu4.Location = new System.Drawing.Point(4, 4);
            this.tpgMenu4.Margin = new System.Windows.Forms.Padding(2);
            this.tpgMenu4.Name = "tpgMenu4";
            this.tpgMenu4.Padding = new System.Windows.Forms.Padding(5);
            this.tpgMenu4.Size = new System.Drawing.Size(1104, 851);
            this.tpgMenu4.TabIndex = 3;
            this.tpgMenu4.Text = "tpgMenu4";
            // 
            // tpgMenu5
            // 
            this.tpgMenu5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.tpgMenu5.Location = new System.Drawing.Point(4, 4);
            this.tpgMenu5.Name = "tpgMenu5";
            this.tpgMenu5.Padding = new System.Windows.Forms.Padding(5);
            this.tpgMenu5.Size = new System.Drawing.Size(1104, 851);
            this.tpgMenu5.TabIndex = 5;
            this.tpgMenu5.Text = "tpgMenu5";
            // 
            // tmProc
            // 
            this.tmProc.Interval = 10;
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(1083, 816);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(172, 63);
            this.panel4.TabIndex = 1372;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.panel1.Controls.Add(this.pnHandle);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.sgSelPart);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1091, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(164, 816);
            this.panel1.TabIndex = 1373;
            // 
            // pnHandle
            // 
            this.pnHandle.BackColor = System.Drawing.Color.Transparent;
            this.pnHandle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnHandle.Location = new System.Drawing.Point(5, 555);
            this.pnHandle.Name = "pnHandle";
            this.pnHandle.Size = new System.Drawing.Size(154, 256);
            this.pnHandle.TabIndex = 1371;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(5, 189);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(154, 52);
            this.btnSave.TabIndex = 1359;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseUp);
            // 
            // sgSelPart
            // 
            this.sgSelPart.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.sgSelPart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgSelPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgSelPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgSelPart.Location = new System.Drawing.Point(5, 7);
            this.sgSelPart.Margin = new System.Windows.Forms.Padding(2);
            this.sgSelPart.Name = "sgSelPart";
            this.sgSelPart.RowTemplate.Height = 30;
            this.sgSelPart.Size = new System.Drawing.Size(154, 182);
            this.sgSelPart.TabIndex = 473;
            this.sgSelPart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgSelPart_CellClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 2);
            this.label1.TabIndex = 472;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmMControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(1255, 879);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.tabMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMControl";
            this.Text = "FrmMControl";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMControl_FormClosed);
            this.Load += new System.EventHandler(this.FrmMControl_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmMControl_VisibleChanged);
            this.tabMenu.ResumeLayout(false);
            this.tpgMenu1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabMenu;
		private System.Windows.Forms.TabPage tpgMenu1;
		private System.Windows.Forms.TabPage tpgMenu2;
		private System.Windows.Forms.TabPage tpgMenu3;
		private System.Windows.Forms.TabPage tpgMenu4;
		private System.Windows.Forms.TabPage tpgMenu5;
		private System.Windows.Forms.Timer tmProc;
		private System.Windows.Forms.Panel pnStatus;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnHandle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView sgSelPart;
        private System.Windows.Forms.Label label1;
    }
}