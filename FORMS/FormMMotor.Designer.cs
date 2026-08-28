namespace eMachine
{
    partial class FrmMMotor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMMotor));
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.pnMotrBase = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.grouper5 = new System.Windows.Forms.Grouper();
            this.rbSpdRato10 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato9 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato8 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato7 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato6 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato5 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato4 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato3 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato2 = new System.Windows.Forms.RadioButton();
            this.rbSpdRato1 = new System.Windows.Forms.RadioButton();
            this.btnDefaultSave = new System.Windows.Forms.Button();
            this.btnDefaultLoad = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.sgMotorSpd = new System.Windows.Forms.DataGridView();
            this.pnBaseMotr = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnHandle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.sgSelPart = new System.Windows.Forms.DataGridView();
            this.pnMotrBase.SuspendLayout();
            this.panel10.SuspendLayout();
            this.grouper5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgMotorSpd)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).BeginInit();
            this.SuspendLayout();
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // pnMotrBase
            // 
            this.pnMotrBase.BackColor = System.Drawing.Color.Transparent;
            this.pnMotrBase.Controls.Add(this.panel10);
            this.pnMotrBase.Controls.Add(this.panel2);
            this.pnMotrBase.Controls.Add(this.pnBaseMotr);
            this.pnMotrBase.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnMotrBase.Location = new System.Drawing.Point(0, 0);
            this.pnMotrBase.Margin = new System.Windows.Forms.Padding(2);
            this.pnMotrBase.Name = "pnMotrBase";
            this.pnMotrBase.Size = new System.Drawing.Size(1080, 879);
            this.pnMotrBase.TabIndex = 1384;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Transparent;
            this.panel10.Controls.Add(this.grouper5);
            this.panel10.Controls.Add(this.btnDefaultSave);
            this.panel10.Controls.Add(this.btnDefaultLoad);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 796);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(1080, 63);
            this.panel10.TabIndex = 1383;
            // 
            // grouper5
            // 
            this.grouper5.BackgroundColor = System.Drawing.Color.White;
            this.grouper5.BackgroundGradientColor = System.Drawing.Color.White;
            this.grouper5.BackgroundGradientMode = System.Windows.Forms.Grouper.GroupBoxGradientMode.None;
            this.grouper5.BorderColor = System.Drawing.Color.Black;
            this.grouper5.BorderThickness = 1F;
            this.grouper5.Controls.Add(this.rbSpdRato10);
            this.grouper5.Controls.Add(this.rbSpdRato9);
            this.grouper5.Controls.Add(this.rbSpdRato8);
            this.grouper5.Controls.Add(this.rbSpdRato7);
            this.grouper5.Controls.Add(this.rbSpdRato6);
            this.grouper5.Controls.Add(this.rbSpdRato5);
            this.grouper5.Controls.Add(this.rbSpdRato4);
            this.grouper5.Controls.Add(this.rbSpdRato3);
            this.grouper5.Controls.Add(this.rbSpdRato2);
            this.grouper5.Controls.Add(this.rbSpdRato1);
            this.grouper5.CustomGroupBoxColor = System.Drawing.Color.LightGray;
            this.grouper5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grouper5.ForeColor = System.Drawing.Color.Black;
            this.grouper5.GroupImage = null;
            this.grouper5.GroupTitle = "Velocity Ratio";
            this.grouper5.Location = new System.Drawing.Point(3, 3);
            this.grouper5.Name = "grouper5";
            this.grouper5.Padding = new System.Windows.Forms.Padding(30, 10, 3, 3);
            this.grouper5.PaintGroupBox = false;
            this.grouper5.RoundCorners = 5;
            this.grouper5.ShadowColor = System.Drawing.Color.Silver;
            this.grouper5.ShadowControl = false;
            this.grouper5.ShadowThickness = 3;
            this.grouper5.Size = new System.Drawing.Size(786, 54);
            this.grouper5.TabIndex = 498;
            this.grouper5.TabStop = false;
            this.grouper5.Text = "grouper5";
            this.grouper5.Visible = false;
            // 
            // rbSpdRato10
            // 
            this.rbSpdRato10.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato10.Location = new System.Drawing.Point(678, 24);
            this.rbSpdRato10.Name = "rbSpdRato10";
            this.rbSpdRato10.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato10.TabIndex = 19;
            this.rbSpdRato10.TabStop = true;
            this.rbSpdRato10.Text = "100%";
            this.rbSpdRato10.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato9
            // 
            this.rbSpdRato9.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato9.Location = new System.Drawing.Point(606, 24);
            this.rbSpdRato9.Name = "rbSpdRato9";
            this.rbSpdRato9.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato9.TabIndex = 18;
            this.rbSpdRato9.TabStop = true;
            this.rbSpdRato9.Text = "90%";
            this.rbSpdRato9.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato8
            // 
            this.rbSpdRato8.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato8.Location = new System.Drawing.Point(534, 24);
            this.rbSpdRato8.Name = "rbSpdRato8";
            this.rbSpdRato8.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato8.TabIndex = 17;
            this.rbSpdRato8.TabStop = true;
            this.rbSpdRato8.Text = "80%";
            this.rbSpdRato8.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato7
            // 
            this.rbSpdRato7.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato7.Location = new System.Drawing.Point(462, 24);
            this.rbSpdRato7.Name = "rbSpdRato7";
            this.rbSpdRato7.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato7.TabIndex = 16;
            this.rbSpdRato7.TabStop = true;
            this.rbSpdRato7.Text = "70%";
            this.rbSpdRato7.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato6
            // 
            this.rbSpdRato6.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato6.Location = new System.Drawing.Point(390, 24);
            this.rbSpdRato6.Name = "rbSpdRato6";
            this.rbSpdRato6.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato6.TabIndex = 15;
            this.rbSpdRato6.TabStop = true;
            this.rbSpdRato6.Text = "60%";
            this.rbSpdRato6.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato5
            // 
            this.rbSpdRato5.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato5.Location = new System.Drawing.Point(318, 24);
            this.rbSpdRato5.Name = "rbSpdRato5";
            this.rbSpdRato5.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato5.TabIndex = 14;
            this.rbSpdRato5.TabStop = true;
            this.rbSpdRato5.Text = "50%";
            this.rbSpdRato5.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato4
            // 
            this.rbSpdRato4.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato4.Location = new System.Drawing.Point(246, 24);
            this.rbSpdRato4.Name = "rbSpdRato4";
            this.rbSpdRato4.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato4.TabIndex = 13;
            this.rbSpdRato4.TabStop = true;
            this.rbSpdRato4.Text = "40%";
            this.rbSpdRato4.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato3
            // 
            this.rbSpdRato3.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato3.Location = new System.Drawing.Point(174, 24);
            this.rbSpdRato3.Name = "rbSpdRato3";
            this.rbSpdRato3.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato3.TabIndex = 12;
            this.rbSpdRato3.TabStop = true;
            this.rbSpdRato3.Text = "30%";
            this.rbSpdRato3.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato2
            // 
            this.rbSpdRato2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato2.Location = new System.Drawing.Point(102, 24);
            this.rbSpdRato2.Name = "rbSpdRato2";
            this.rbSpdRato2.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato2.TabIndex = 11;
            this.rbSpdRato2.TabStop = true;
            this.rbSpdRato2.Text = "20%";
            this.rbSpdRato2.UseVisualStyleBackColor = true;
            // 
            // rbSpdRato1
            // 
            this.rbSpdRato1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSpdRato1.Location = new System.Drawing.Point(30, 24);
            this.rbSpdRato1.Name = "rbSpdRato1";
            this.rbSpdRato1.Size = new System.Drawing.Size(72, 27);
            this.rbSpdRato1.TabIndex = 10;
            this.rbSpdRato1.TabStop = true;
            this.rbSpdRato1.Text = "10%";
            this.rbSpdRato1.UseVisualStyleBackColor = true;
            // 
            // btnDefaultSave
            // 
            this.btnDefaultSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnDefaultSave.FlatAppearance.BorderSize = 2;
            this.btnDefaultSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnDefaultSave.ForeColor = System.Drawing.Color.Black;
            this.btnDefaultSave.Location = new System.Drawing.Point(950, 8);
            this.btnDefaultSave.Name = "btnDefaultSave";
            this.btnDefaultSave.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnDefaultSave.Size = new System.Drawing.Size(124, 49);
            this.btnDefaultSave.TabIndex = 1371;
            this.btnDefaultSave.Text = "Defalut Save";
            this.btnDefaultSave.UseVisualStyleBackColor = true;
            this.btnDefaultSave.Visible = false;
            // 
            // btnDefaultLoad
            // 
            this.btnDefaultLoad.BackColor = System.Drawing.SystemColors.Control;
            this.btnDefaultLoad.FlatAppearance.BorderSize = 2;
            this.btnDefaultLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnDefaultLoad.ForeColor = System.Drawing.Color.Black;
            this.btnDefaultLoad.Location = new System.Drawing.Point(795, 8);
            this.btnDefaultLoad.Name = "btnDefaultLoad";
            this.btnDefaultLoad.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnDefaultLoad.Size = new System.Drawing.Size(124, 49);
            this.btnDefaultLoad.TabIndex = 1372;
            this.btnDefaultLoad.Text = "Defalut Load";
            this.btnDefaultLoad.UseVisualStyleBackColor = true;
            this.btnDefaultLoad.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sgMotorSpd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 460);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(1080, 336);
            this.panel2.TabIndex = 1370;
            // 
            // sgMotorSpd
            // 
            this.sgMotorSpd.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.sgMotorSpd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sgMotorSpd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sgMotorSpd.Location = new System.Drawing.Point(3, 3);
            this.sgMotorSpd.Margin = new System.Windows.Forms.Padding(2);
            this.sgMotorSpd.Name = "sgMotorSpd";
            this.sgMotorSpd.RowTemplate.Height = 30;
            this.sgMotorSpd.Size = new System.Drawing.Size(1074, 330);
            this.sgMotorSpd.TabIndex = 426;
            // 
            // pnBaseMotr
            // 
            this.pnBaseMotr.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnBaseMotr.Location = new System.Drawing.Point(0, 0);
            this.pnBaseMotr.Margin = new System.Windows.Forms.Padding(0);
            this.pnBaseMotr.Name = "pnBaseMotr";
            this.pnBaseMotr.Padding = new System.Windows.Forms.Padding(3);
            this.pnBaseMotr.Size = new System.Drawing.Size(1080, 460);
            this.pnBaseMotr.TabIndex = 1369;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.panel1.Controls.Add(this.pnHandle);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.sgSelPart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1091, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(164, 879);
            this.panel1.TabIndex = 1386;
            // 
            // pnHandle
            // 
            this.pnHandle.BackColor = System.Drawing.Color.Transparent;
            this.pnHandle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnHandle.Location = new System.Drawing.Point(5, 618);
            this.pnHandle.Name = "pnHandle";
            this.pnHandle.Size = new System.Drawing.Size(154, 256);
            this.pnHandle.TabIndex = 1382;
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
            this.btnSave.TabIndex = 1369;
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
            this.sgSelPart.GridColor = System.Drawing.Color.Gainsboro;
            this.sgSelPart.Location = new System.Drawing.Point(5, 5);
            this.sgSelPart.Margin = new System.Windows.Forms.Padding(2);
            this.sgSelPart.Name = "sgSelPart";
            this.sgSelPart.RowTemplate.Height = 30;
            this.sgSelPart.Size = new System.Drawing.Size(154, 182);
            this.sgSelPart.TabIndex = 473;
            this.sgSelPart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sgSelPart_CellClick);
            // 
            // FrmMMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1255, 879);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnMotrBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMMotor";
            this.Text = "FrmMMotor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMMotor_FormClosed);
            this.Load += new System.EventHandler(this.FrmMMotor_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmMMotor_VisibleChanged);
            this.pnMotrBase.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.grouper5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgMotorSpd)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sgSelPart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.Panel pnMotrBase;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView sgMotorSpd;
        private System.Windows.Forms.Panel pnBaseMotr;
        private System.Windows.Forms.Panel panel10;
        internal System.Windows.Forms.Button btnDefaultSave;
        internal System.Windows.Forms.Button btnDefaultLoad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnHandle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView sgSelPart;
        private System.Windows.Forms.Grouper grouper5;
        private System.Windows.Forms.RadioButton rbSpdRato10;
        private System.Windows.Forms.RadioButton rbSpdRato9;
        private System.Windows.Forms.RadioButton rbSpdRato8;
        private System.Windows.Forms.RadioButton rbSpdRato7;
        private System.Windows.Forms.RadioButton rbSpdRato6;
        private System.Windows.Forms.RadioButton rbSpdRato5;
        private System.Windows.Forms.RadioButton rbSpdRato4;
        private System.Windows.Forms.RadioButton rbSpdRato3;
        private System.Windows.Forms.RadioButton rbSpdRato2;
        private System.Windows.Forms.RadioButton rbSpdRato1;
    }
}