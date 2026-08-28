namespace eMachine
{
    partial class FrmMIO
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
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.sgOutput = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnHandle = new System.Windows.Forms.Panel();
            this.btnAddressIO = new System.Windows.Forms.Button();
            this.sgSelPart = new System.Windows.Forms.DataGridView();
            this.roundPanel1 = new System.Windows.Forms.RoundPanel();
            this.sgInput = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.roundPanel2 = new System.Windows.Forms.RoundPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btIOSet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sgOutput)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).BeginInit();
            this.roundPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgInput)).BeginInit();
            this.roundPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // sgOutput
            // 
            this.sgOutput.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgOutput.Cursor = System.Windows.Forms.Cursors.Default;
            this.sgOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgOutput.Location = new System.Drawing.Point(3, 28);
            this.sgOutput.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.sgOutput.Name = "sgOutput";
            this.sgOutput.RowTemplate.Height = 30;
            this.sgOutput.Size = new System.Drawing.Size(609, 228);
            this.sgOutput.TabIndex = 425;
            this.sgOutput.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgOutput_CellClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btIOSet);
            this.panel1.Controls.Add(this.pnHandle);
            this.panel1.Controls.Add(this.btnAddressIO);
            this.panel1.Controls.Add(this.sgSelPart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1244, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(187, 1091);
            this.panel1.TabIndex = 1382;
            // 
            // pnHandle
            // 
            this.pnHandle.BackColor = System.Drawing.Color.Transparent;
            this.pnHandle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnHandle.Location = new System.Drawing.Point(1, 770);
            this.pnHandle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnHandle.Name = "pnHandle";
            this.pnHandle.Size = new System.Drawing.Size(185, 320);
            this.pnHandle.TabIndex = 1378;
            // 
            // btnAddressIO
            // 
            this.btnAddressIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAddressIO.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddressIO.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddressIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddressIO.ForeColor = System.Drawing.Color.Black;
            this.btnAddressIO.Location = new System.Drawing.Point(1, 229);
            this.btnAddressIO.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddressIO.Name = "btnAddressIO";
            this.btnAddressIO.Size = new System.Drawing.Size(185, 59);
            this.btnAddressIO.TabIndex = 1376;
            this.btnAddressIO.Tag = "1";
            this.btnAddressIO.Text = "Address Setting";
            this.btnAddressIO.UseVisualStyleBackColor = false;
            this.btnAddressIO.Visible = false;
            this.btnAddressIO.Click += new System.EventHandler(this.btnAddressIO_Click);
            // 
            // sgSelPart
            // 
            this.sgSelPart.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgSelPart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgSelPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgSelPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgSelPart.Location = new System.Drawing.Point(1, 1);
            this.sgSelPart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.sgSelPart.Name = "sgSelPart";
            this.sgSelPart.RowTemplate.Height = 30;
            this.sgSelPart.Size = new System.Drawing.Size(185, 228);
            this.sgSelPart.TabIndex = 473;
            this.sgSelPart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgSelPart_CellClick);
            // 
            // roundPanel1
            // 
            this.roundPanel1.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel1.Controls.Add(this.sgInput);
            this.roundPanel1.Controls.Add(this.flowLayoutPanel2);
            this.roundPanel1.Location = new System.Drawing.Point(0, 0);
            this.roundPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundPanel1.Radious = 15;
            this.roundPanel1.Size = new System.Drawing.Size(615, 1056);
            this.roundPanel1.TabIndex = 1383;
            this.roundPanel1.TabStop = false;
            this.roundPanel1.Text = "Input";
            this.roundPanel1.TitleBackColor = System.Drawing.Color.ForestGreen;
            this.roundPanel1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.roundPanel1.TitleForeColor = System.Drawing.Color.Black;
            // 
            // sgInput
            // 
            this.sgInput.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgInput.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.sgInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgInput.Location = new System.Drawing.Point(3, 28);
            this.sgInput.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.sgInput.Name = "sgInput";
            this.sgInput.RowTemplate.Height = 30;
            this.sgInput.Size = new System.Drawing.Size(609, 228);
            this.sgInput.TabIndex = 425;
            this.sgInput.SelectionChanged += new System.EventHandler(this.sgOutput_SelectionChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 22);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(609, 6);
            this.flowLayoutPanel2.TabIndex = 426;
            // 
            // roundPanel2
            // 
            this.roundPanel2.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel2.Controls.Add(this.sgOutput);
            this.roundPanel2.Controls.Add(this.flowLayoutPanel3);
            this.roundPanel2.Location = new System.Drawing.Point(621, 0);
            this.roundPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel2.Name = "roundPanel2";
            this.roundPanel2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundPanel2.Radious = 15;
            this.roundPanel2.Size = new System.Drawing.Size(615, 1056);
            this.roundPanel2.TabIndex = 1384;
            this.roundPanel2.TabStop = false;
            this.roundPanel2.Text = "Output";
            this.roundPanel2.TitleBackColor = System.Drawing.Color.LightCoral;
            this.roundPanel2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.roundPanel2.TitleForeColor = System.Drawing.Color.Black;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 22);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.flowLayoutPanel3.Size = new System.Drawing.Size(609, 6);
            this.flowLayoutPanel3.TabIndex = 426;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.roundPanel1);
            this.flowLayoutPanel1.Controls.Add(this.roundPanel2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 4);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1240, 1091);
            this.flowLayoutPanel1.TabIndex = 1385;
            // 
            // btIOSet
            // 
            this.btIOSet.BackColor = System.Drawing.Color.Khaki;
            this.btIOSet.Dock = System.Windows.Forms.DockStyle.Top;
            this.btIOSet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btIOSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btIOSet.ForeColor = System.Drawing.Color.Black;
            this.btIOSet.Location = new System.Drawing.Point(1, 288);
            this.btIOSet.Margin = new System.Windows.Forms.Padding(2);
            this.btIOSet.Name = "btIOSet";
            this.btIOSet.Size = new System.Drawing.Size(185, 59);
            this.btIOSet.TabIndex = 1380;
            this.btIOSet.Tag = "1";
            this.btIOSet.Text = "IO SETTING";
            this.btIOSet.UseVisualStyleBackColor = false;
            this.btIOSet.Click += new System.EventHandler(this.btIOSet_Click);
            // 
            // FrmMIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(1434, 1099);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMIO";
            this.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Text = "FrmMIO";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMIO_FormClosed);
            this.Load += new System.EventHandler(this.FrmMIO_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmMIO_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.sgOutput)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).EndInit();
            this.roundPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgInput)).EndInit();
            this.roundPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.DataGridView sgOutput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnHandle;
        private System.Windows.Forms.Button btnAddressIO;
        private System.Windows.Forms.DataGridView sgSelPart;
        private System.Windows.Forms.RoundPanel roundPanel1;
        private System.Windows.Forms.RoundPanel roundPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.DataGridView sgInput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btIOSet;
    }
}