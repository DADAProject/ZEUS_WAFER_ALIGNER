namespace eMachine
{
    partial class FrmLot2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLot2));
            this.edLotNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.edOperId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnVisnJobChange = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sgJobList = new System.Windows.Forms.DataGridView();
            this.edPartNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edDevice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgJobList)).BeginInit();
            this.SuspendLayout();
            // 
            // edLotNo
            // 
            this.edLotNo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edLotNo.Location = new System.Drawing.Point(178, 308);
            this.edLotNo.Margin = new System.Windows.Forms.Padding(2);
            this.edLotNo.Name = "edLotNo";
            this.edLotNo.Size = new System.Drawing.Size(425, 27);
            this.edLotNo.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.label4.Location = new System.Drawing.Point(7, 308);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 27);
            this.label4.TabIndex = 492;
            this.label4.Text = "Lot No.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edOperId
            // 
            this.edOperId.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edOperId.Location = new System.Drawing.Point(178, 368);
            this.edOperId.Margin = new System.Windows.Forms.Padding(2);
            this.edOperId.Name = "edOperId";
            this.edOperId.Size = new System.Drawing.Size(317, 27);
            this.edOperId.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.label1.Location = new System.Drawing.Point(7, 368);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 27);
            this.label1.TabIndex = 494;
            this.label1.Text = "Oper No.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnVisnJobChange
            // 
            this.btnVisnJobChange.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnVisnJobChange.Location = new System.Drawing.Point(497, 368);
            this.btnVisnJobChange.Margin = new System.Windows.Forms.Padding(2);
            this.btnVisnJobChange.Name = "btnVisnJobChange";
            this.btnVisnJobChange.Size = new System.Drawing.Size(107, 27);
            this.btnVisnJobChange.TabIndex = 498;
            this.btnVisnJobChange.Text = "Change";
            this.btnVisnJobChange.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.sgJobList);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox3.Size = new System.Drawing.Size(603, 270);
            this.groupBox3.TabIndex = 503;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " PROJECT LIST ";
            // 
            // sgJobList
            // 
            this.sgJobList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgJobList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.sgJobList.Cursor = System.Windows.Forms.Cursors.Default;
            this.sgJobList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sgJobList.Location = new System.Drawing.Point(5, 22);
            this.sgJobList.Margin = new System.Windows.Forms.Padding(2);
            this.sgJobList.Name = "sgJobList";
            this.sgJobList.RowTemplate.Height = 30;
            this.sgJobList.Size = new System.Drawing.Size(593, 243);
            this.sgJobList.TabIndex = 425;
            this.sgJobList.SelectionChanged += new System.EventHandler(this.sgLotList_SelectionChanged);
            // 
            // edPartNo
            // 
            this.edPartNo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edPartNo.Location = new System.Drawing.Point(178, 338);
            this.edPartNo.Margin = new System.Windows.Forms.Padding(2);
            this.edPartNo.Name = "edPartNo";
            this.edPartNo.Size = new System.Drawing.Size(425, 27);
            this.edPartNo.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.label2.Location = new System.Drawing.Point(7, 338);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 27);
            this.label2.TabIndex = 504;
            this.label2.Text = "Part no.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edDevice
            // 
            this.edDevice.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.edDevice.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edDevice.Location = new System.Drawing.Point(178, 278);
            this.edDevice.Margin = new System.Windows.Forms.Padding(2);
            this.edDevice.Name = "edDevice";
            this.edDevice.ReadOnly = true;
            this.edDevice.Size = new System.Drawing.Size(425, 27);
            this.edDevice.TabIndex = 507;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.label3.Location = new System.Drawing.Point(7, 278);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 27);
            this.label3.TabIndex = 506;
            this.label3.Text = "Recipe Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(408, 463);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(196, 50);
            this.button3.TabIndex = 512;
            this.button3.Text = "Close";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(7, 463);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(196, 50);
            this.button2.TabIndex = 511;
            this.button2.Text = "Recipe End";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(7, 398);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(597, 62);
            this.button1.TabIndex = 4;
            this.button1.Text = "Recipe Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(207, 463);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(196, 50);
            this.button4.TabIndex = 513;
            this.button4.Text = "Lot Cancel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FrmLot2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(609, 516);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.edDevice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edPartNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnVisnJobChange);
            this.Controls.Add(this.edOperId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edLotNo);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLot2";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Recipe Open";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLot_FormClosing);
            this.Load += new System.EventHandler(this.FrmLot_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgJobList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox edLotNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edOperId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVisnJobChange;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView sgJobList;
        private System.Windows.Forms.TextBox edPartNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edDevice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
    }
}