namespace eMachine
{
    partial class FrmChangeOperID
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.kToggleButton2 = new System.Windows.Forms.KToggleButton(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.kToggleButton1 = new System.Windows.Forms.KToggleButton(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.kToggleButton2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.kToggleButton1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 143);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.PowderBlue;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 36);
            this.panel2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID Change";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            // 
            // kToggleButton2
            // 
            this.kToggleButton2.AutoCheck = false;
            this.kToggleButton2.BackColor = System.Drawing.Color.DarkGray;
            this.kToggleButton2.Checked = false;
            this.kToggleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kToggleButton2.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kToggleButton2.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.kToggleButton2.ForeColor = System.Drawing.Color.Black;
            this.kToggleButton2.LedFullEnable = false;
            this.kToggleButton2.LedVisible = false;
            this.kToggleButton2.LedWidth = 20;
            this.kToggleButton2.Location = new System.Drawing.Point(127, 86);
            this.kToggleButton2.Name = "kToggleButton2";
            this.kToggleButton2.OffColor = System.Drawing.Color.Red;
            this.kToggleButton2.OnColor = System.Drawing.Color.Lime;
            this.kToggleButton2.RoundEdge = 5;
            this.kToggleButton2.Size = new System.Drawing.Size(109, 51);
            this.kToggleButton2.TabIndex = 8;
            this.kToggleButton2.Text2 = "CLOSE";
            this.kToggleButton2.TextOff = "";
            this.kToggleButton2.TextOn = "";
            this.kToggleButton2.TextOnOffEnable = false;
            this.kToggleButton2.UseVisualStyleBackColor = false;
            this.kToggleButton2.Click += new System.EventHandler(this.kToggleButton2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(6, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(227, 38);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "123456789";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // kToggleButton1
            // 
            this.kToggleButton1.AutoCheck = false;
            this.kToggleButton1.BackColor = System.Drawing.Color.DarkGray;
            this.kToggleButton1.Checked = false;
            this.kToggleButton1.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kToggleButton1.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.kToggleButton1.ForeColor = System.Drawing.Color.Black;
            this.kToggleButton1.LedFullEnable = false;
            this.kToggleButton1.LedVisible = false;
            this.kToggleButton1.LedWidth = 20;
            this.kToggleButton1.Location = new System.Drawing.Point(3, 86);
            this.kToggleButton1.Name = "kToggleButton1";
            this.kToggleButton1.OffColor = System.Drawing.Color.Red;
            this.kToggleButton1.OnColor = System.Drawing.Color.Lime;
            this.kToggleButton1.RoundEdge = 5;
            this.kToggleButton1.Size = new System.Drawing.Size(109, 51);
            this.kToggleButton1.TabIndex = 6;
            this.kToggleButton1.Text2 = "OK";
            this.kToggleButton1.TextOff = "";
            this.kToggleButton1.TextOn = "";
            this.kToggleButton1.TextOnOffEnable = false;
            this.kToggleButton1.UseVisualStyleBackColor = false;
            this.kToggleButton1.Click += new System.EventHandler(this.kToggleButton1_Click);
            // 
            // FrmChangeOperID
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(239, 143);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmChangeOperID";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmChangeOperID_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.KToggleButton kToggleButton2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.KToggleButton kToggleButton1;
    }
}