namespace eMachine
{
    partial class FrmSetInOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetInOut));
            this.panel1 = new System.Windows.Forms.Panel();
            this.SGInputTable = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SGOutputTable = new System.Windows.Forms.DataGridView();
            this.lbMotrName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SGInputTable)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SGOutputTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SGInputTable);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1, 31);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(539, 175);
            this.panel1.TabIndex = 431;
            // 
            // SGInputTable
            // 
            this.SGInputTable.AllowUserToOrderColumns = true;
            this.SGInputTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SGInputTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.SGInputTable.Location = new System.Drawing.Point(0, 19);
            this.SGInputTable.Margin = new System.Windows.Forms.Padding(2);
            this.SGInputTable.Name = "SGInputTable";
            this.SGInputTable.RowTemplate.Height = 30;
            this.SGInputTable.Size = new System.Drawing.Size(537, 137);
            this.SGInputTable.TabIndex = 425;
            this.SGInputTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.SGInputTable_CellEndEdit);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(537, 19);
            this.label1.TabIndex = 424;
            this.label1.Text = "INPUT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.SGOutputTable);
            this.panel2.Controls.Add(this.lbMotrName);
            this.panel2.Location = new System.Drawing.Point(1, 209);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(539, 175);
            this.panel2.TabIndex = 432;
            // 
            // SGOutputTable
            // 
            this.SGOutputTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SGOutputTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.SGOutputTable.Location = new System.Drawing.Point(0, 19);
            this.SGOutputTable.Margin = new System.Windows.Forms.Padding(2);
            this.SGOutputTable.Name = "SGOutputTable";
            this.SGOutputTable.RowTemplate.Height = 30;
            this.SGOutputTable.Size = new System.Drawing.Size(537, 137);
            this.SGOutputTable.TabIndex = 426;
            this.SGOutputTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.SGOutputTable_CellEndEdit);
            // 
            // lbMotrName
            // 
            this.lbMotrName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lbMotrName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMotrName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMotrName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbMotrName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMotrName.ForeColor = System.Drawing.Color.Black;
            this.lbMotrName.Location = new System.Drawing.Point(0, 0);
            this.lbMotrName.Margin = new System.Windows.Forms.Padding(1);
            this.lbMotrName.Name = "lbMotrName";
            this.lbMotrName.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.lbMotrName.Size = new System.Drawing.Size(537, 19);
            this.lbMotrName.TabIndex = 425;
            this.lbMotrName.Text = "OUTPUT";
            this.lbMotrName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(308, 385);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(111, 40);
            this.btnSave.TabIndex = 434;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo.Location = new System.Drawing.Point(423, 385);
            this.btnNo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo.Name = "btnNo";
            this.btnNo.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo.Size = new System.Drawing.Size(118, 40);
            this.btnNo.TabIndex = 435;
            this.btnNo.Text = "Close";
            this.btnNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.DimGray;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Gold;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(541, 29);
            this.lbTitle.TabIndex = 436;
            this.lbTitle.Text = "SET IO ADDRESS";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseDown);
            this.lbTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 12);
            this.label2.TabIndex = 437;
            this.label2.Text = "I/O NUM";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(69, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(71, 21);
            this.textBox1.TabIndex = 438;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(4, 389);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 38);
            this.groupBox1.TabIndex = 439;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Check Addr ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(144, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 439;
            this.button1.Tag = "0";
            this.button1.Text = "Input";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(221, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 440;
            this.button2.Tag = "1";
            this.button2.Text = "Output";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmSetInOut
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(541, 428);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmSetInOut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSetInOut";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSetInOut_FormClosed);
            this.Load += new System.EventHandler(this.FrmSetInOut_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SGInputTable)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SGOutputTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView SGInputTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView SGOutputTable;
        private System.Windows.Forms.Label lbMotrName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}