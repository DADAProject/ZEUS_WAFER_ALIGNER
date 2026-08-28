namespace eMachine
{
    partial class FormMIOSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMIOSetting));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.roundPanel1 = new System.Windows.Forms.RoundPanel();
            this.sgInput = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.roundPanel2 = new System.Windows.Forms.RoundPanel();
            this.sgOutput = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.btSave = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.roundPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgInput)).BeginInit();
            this.roundPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.roundPanel1);
            this.flowLayoutPanel1.Controls.Add(this.roundPanel2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1099, 779);
            this.flowLayoutPanel1.TabIndex = 1386;
            // 
            // roundPanel1
            // 
            this.roundPanel1.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel1.Controls.Add(this.sgInput);
            this.roundPanel1.Controls.Add(this.flowLayoutPanel2);
            this.roundPanel1.Location = new System.Drawing.Point(0, 0);
            this.roundPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundPanel1.Radious = 15;
            this.roundPanel1.Size = new System.Drawing.Size(538, 776);
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
            this.sgInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sgInput.Location = new System.Drawing.Point(3, 27);
            this.sgInput.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.sgInput.Name = "sgInput";
            this.sgInput.RowTemplate.Height = 30;
            this.sgInput.Size = new System.Drawing.Size(532, 745);
            this.sgInput.TabIndex = 425;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 20);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(532, 7);
            this.flowLayoutPanel2.TabIndex = 426;
            // 
            // roundPanel2
            // 
            this.roundPanel2.BackColor = System.Drawing.Color.Transparent;
            this.roundPanel2.Controls.Add(this.sgOutput);
            this.roundPanel2.Controls.Add(this.flowLayoutPanel3);
            this.roundPanel2.Location = new System.Drawing.Point(543, 0);
            this.roundPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel2.Name = "roundPanel2";
            this.roundPanel2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundPanel2.Radious = 15;
            this.roundPanel2.Size = new System.Drawing.Size(553, 776);
            this.roundPanel2.TabIndex = 1384;
            this.roundPanel2.TabStop = false;
            this.roundPanel2.Text = "Output";
            this.roundPanel2.TitleBackColor = System.Drawing.Color.LightCoral;
            this.roundPanel2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.roundPanel2.TitleForeColor = System.Drawing.Color.Black;
            // 
            // sgOutput
            // 
            this.sgOutput.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgOutput.Cursor = System.Windows.Forms.Cursors.Default;
            this.sgOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sgOutput.Location = new System.Drawing.Point(3, 27);
            this.sgOutput.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.sgOutput.Name = "sgOutput";
            this.sgOutput.RowTemplate.Height = 30;
            this.sgOutput.Size = new System.Drawing.Size(547, 745);
            this.sgOutput.TabIndex = 425;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 20);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.flowLayoutPanel3.Size = new System.Drawing.Size(547, 7);
            this.flowLayoutPanel3.TabIndex = 426;
            // 
            // btSave
            // 
            this.btSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSave.ForeColor = System.Drawing.Color.Black;
            this.btSave.Location = new System.Drawing.Point(1099, 0);
            this.btSave.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(147, 63);
            this.btSave.TabIndex = 1387;
            this.btSave.Tag = "1";
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = false;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btClose
            // 
            this.btClose.BackColor = System.Drawing.Color.White;
            this.btClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btClose.ForeColor = System.Drawing.Color.Black;
            this.btClose.Location = new System.Drawing.Point(1099, 716);
            this.btClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(147, 63);
            this.btClose.TabIndex = 1388;
            this.btClose.Tag = "1";
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = false;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // FormMIOSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(1246, 779);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMIOSetting";
            this.Text = "IO Setting";
            this.Load += new System.EventHandler(this.FormMIOSetting_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.roundPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgInput)).EndInit();
            this.roundPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RoundPanel roundPanel1;
        private System.Windows.Forms.DataGridView sgInput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RoundPanel roundPanel2;
        private System.Windows.Forms.DataGridView sgOutput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btClose;
    }
}