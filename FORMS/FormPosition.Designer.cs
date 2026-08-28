
namespace eMachine
{
    partial class FormPosition
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
            this.button1 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.sgPos = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbImg = new System.Windows.Forms.PictureBox();
            this.pnMotr = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgPos)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 609);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(790, 62);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(5);
            this.button1.Size = new System.Drawing.Size(790, 62);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.sgPos);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 308);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(790, 301);
            this.panel4.TabIndex = 3;
            // 
            // sgPos
            // 
            this.sgPos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgPos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgPos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sgPos.Dock = System.Windows.Forms.DockStyle.Top;
            this.sgPos.Location = new System.Drawing.Point(0, 0);
            this.sgPos.Margin = new System.Windows.Forms.Padding(2);
            this.sgPos.Name = "sgPos";
            this.sgPos.RowTemplate.Height = 30;
            this.sgPos.Size = new System.Drawing.Size(790, 110);
            this.sgPos.TabIndex = 474;
            this.sgPos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgPos_CellClick);
            this.sgPos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgPos_CellDoubleClick);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.pbImg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 308);
            this.panel2.TabIndex = 4;
            // 
            // pbImg
            // 
            this.pbImg.Location = new System.Drawing.Point(12, 12);
            this.pbImg.Name = "pbImg";
            this.pbImg.Size = new System.Drawing.Size(184, 133);
            this.pbImg.TabIndex = 0;
            this.pbImg.TabStop = false;
            // 
            // pnMotr
            // 
            this.pnMotr.BackColor = System.Drawing.Color.Transparent;
            this.pnMotr.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnMotr.Location = new System.Drawing.Point(324, 0);
            this.pnMotr.Name = "pnMotr";
            this.pnMotr.Size = new System.Drawing.Size(466, 308);
            this.pnMotr.TabIndex = 7;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.ClientSize = new System.Drawing.Size(790, 671);
            this.Controls.Add(this.pnMotr);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Name = "FormPosition";
            this.Text = "FormPosition";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPosition_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPosition_FormClosed);
            this.Load += new System.EventHandler(this.FormPosition_Load);
            this.VisibleChanged += new System.EventHandler(this.FormPosition_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgPos)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView sgPos;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnMotr;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pbImg;
    }
}