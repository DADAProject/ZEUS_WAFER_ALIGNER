namespace eMachine
{
    partial class FrmBinColor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBinColor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.sgSelColorHole = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRandom = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgSelColorHole)).BeginInit();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sgSelColorHole);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(529, 558);
            this.panel1.TabIndex = 1433;
            // 
            // sgSelColorHole
            // 
            this.sgSelColorHole.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgSelColorHole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgSelColorHole.ColumnHeadersHeight = 30;
            this.sgSelColorHole.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.sgSelColorHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sgSelColorHole.GridColor = System.Drawing.Color.Gainsboro;
            this.sgSelColorHole.Location = new System.Drawing.Point(1, 75);
            this.sgSelColorHole.Margin = new System.Windows.Forms.Padding(2);
            this.sgSelColorHole.Name = "sgSelColorHole";
            this.sgSelColorHole.RowTemplate.Height = 30;
            this.sgSelColorHole.Size = new System.Drawing.Size(527, 482);
            this.sgSelColorHole.TabIndex = 1440;
            this.sgSelColorHole.Tag = "0";
            this.sgSelColorHole.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgSelColor_CellClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(527, 74);
            this.panel2.TabIndex = 1439;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(527, 74);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hole\r\nStatus Color";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRandom
            // 
            this.btnRandom.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnRandom.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRandom.FlatAppearance.BorderSize = 0;
            this.btnRandom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRandom.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRandom.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRandom.Image = ((System.Drawing.Image)(resources.GetObject("btnRandom.Image")));
            this.btnRandom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRandom.Location = new System.Drawing.Point(1, 3);
            this.btnRandom.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Padding = new System.Windows.Forms.Padding(1);
            this.btnRandom.Size = new System.Drawing.Size(262, 52);
            this.btnRandom.TabIndex = 1434;
            this.btnRandom.Tag = "5";
            this.btnRandom.Text = "Random";
            this.btnRandom.UseVisualStyleBackColor = false;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(265, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(1, 0, 1, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(1);
            this.btnSave.Size = new System.Drawing.Size(262, 58);
            this.btnSave.TabIndex = 1435;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(1, 61);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(1);
            this.btnCancel.Size = new System.Drawing.Size(528, 56);
            this.btnCancel.TabIndex = 1436;
            this.btnCancel.Tag = "0";
            this.btnCancel.Text = "CLOSE";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnRandom);
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 562);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(529, 113);
            this.flowLayoutPanel1.TabIndex = 1437;
            // 
            // FrmBinColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.ClientSize = new System.Drawing.Size(533, 677);
            this.ControlBox = false;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmBinColor";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowIcon = false;
            this.Text = "COLOR";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmBinColor_FormClosed);
            this.Load += new System.EventHandler(this.FrmBinColor_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmBinColor_VisibleChanged);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgSelColorHole)).EndInit();
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DataGridView sgSelColorHole;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
    }
}