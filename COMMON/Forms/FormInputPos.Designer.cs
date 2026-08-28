namespace eMachine
{
    partial class FrmInputPos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInputPos));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.edMaxPos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.edMinPos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edMotrPos = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.edOldPos = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPN = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.btnNo0 = new System.Windows.Forms.Button();
            this.btnNo3 = new System.Windows.Forms.Button();
            this.btnNo2 = new System.Windows.Forms.Button();
            this.btnNo1 = new System.Windows.Forms.Button();
            this.btnNo6 = new System.Windows.Forms.Button();
            this.btnNo5 = new System.Windows.Forms.Button();
            this.btnNo4 = new System.Windows.Forms.Button();
            this.btnNo9 = new System.Windows.Forms.Button();
            this.btnNo8 = new System.Windows.Forms.Button();
            this.btnNo7 = new System.Windows.Forms.Button();
            this.btnBS = new System.Windows.Forms.Button();
            this.btnCls = new System.Windows.Forms.Button();
            this.btnSetPos = new System.Windows.Forms.Button();
            this.btnGetMotr = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.edIncDec = new System.Windows.Forms.TextBox();
            this.edValue = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.Timer_Update = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.lbTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(528, 304);
            this.panel1.TabIndex = 432;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.edMaxPos);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.edMinPos);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.edMotrPos);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.edOldPos);
            this.panel4.Location = new System.Drawing.Point(384, 73);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(140, 169);
            this.panel4.TabIndex = 436;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 124);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 18);
            this.label4.TabIndex = 450;
            this.label4.Text = "Max Position";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edMaxPos
            // 
            this.edMaxPos.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edMaxPos.Location = new System.Drawing.Point(9, 144);
            this.edMaxPos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edMaxPos.Name = "edMaxPos";
            this.edMaxPos.Size = new System.Drawing.Size(123, 20);
            this.edMaxPos.TabIndex = 449;
            this.edMaxPos.Text = "0";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 18);
            this.label3.TabIndex = 448;
            this.label3.Text = "Min Position";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edMinPos
            // 
            this.edMinPos.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edMinPos.Location = new System.Drawing.Point(9, 104);
            this.edMinPos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edMinPos.Name = "edMinPos";
            this.edMinPos.Size = new System.Drawing.Size(123, 20);
            this.edMinPos.TabIndex = 447;
            this.edMinPos.Text = "0";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(7, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 18);
            this.label2.TabIndex = 446;
            this.label2.Text = "Motor Current Position";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edMotrPos
            // 
            this.edMotrPos.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edMotrPos.Location = new System.Drawing.Point(10, 64);
            this.edMotrPos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edMotrPos.Name = "edMotrPos";
            this.edMotrPos.Size = new System.Drawing.Size(123, 20);
            this.edMotrPos.TabIndex = 445;
            this.edMotrPos.Text = "0";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(7, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 18);
            this.label1.TabIndex = 444;
            this.label1.Text = "Now Setting Position";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edOldPos
            // 
            this.edOldPos.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edOldPos.Location = new System.Drawing.Point(10, 24);
            this.edOldPos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edOldPos.Name = "edOldPos";
            this.edOldPos.Size = new System.Drawing.Size(123, 20);
            this.edOldPos.TabIndex = 443;
            this.edOldPos.Text = "0";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnPN);
            this.panel3.Controls.Add(this.btnDot);
            this.panel3.Controls.Add(this.btnNo0);
            this.panel3.Controls.Add(this.btnNo3);
            this.panel3.Controls.Add(this.btnNo2);
            this.panel3.Controls.Add(this.btnNo1);
            this.panel3.Controls.Add(this.btnNo6);
            this.panel3.Controls.Add(this.btnNo5);
            this.panel3.Controls.Add(this.btnNo4);
            this.panel3.Controls.Add(this.btnNo9);
            this.panel3.Controls.Add(this.btnNo8);
            this.panel3.Controls.Add(this.btnNo7);
            this.panel3.Controls.Add(this.btnBS);
            this.panel3.Controls.Add(this.btnCls);
            this.panel3.Controls.Add(this.btnSetPos);
            this.panel3.Controls.Add(this.btnGetMotr);
            this.panel3.Location = new System.Drawing.Point(5, 72);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(376, 228);
            this.panel3.TabIndex = 435;
            // 
            // btnPN
            // 
            this.btnPN.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPN.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPN.Location = new System.Drawing.Point(181, 170);
            this.btnPN.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnPN.Name = "btnPN";
            this.btnPN.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnPN.Size = new System.Drawing.Size(85, 53);
            this.btnPN.TabIndex = 448;
            this.btnPN.Text = "+/-";
            this.btnPN.UseVisualStyleBackColor = false;
            this.btnPN.Click += new System.EventHandler(this.btnPN_Click);
            // 
            // btnDot
            // 
            this.btnDot.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDot.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDot.Location = new System.Drawing.Point(92, 171);
            this.btnDot.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnDot.Name = "btnDot";
            this.btnDot.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnDot.Size = new System.Drawing.Size(85, 53);
            this.btnDot.TabIndex = 447;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = false;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // btnNo0
            // 
            this.btnNo0.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo0.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo0.Location = new System.Drawing.Point(3, 171);
            this.btnNo0.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo0.Name = "btnNo0";
            this.btnNo0.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo0.Size = new System.Drawing.Size(85, 53);
            this.btnNo0.TabIndex = 446;
            this.btnNo0.Tag = "0";
            this.btnNo0.Text = "0";
            this.btnNo0.UseVisualStyleBackColor = false;
            this.btnNo0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo3
            // 
            this.btnNo3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo3.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo3.Location = new System.Drawing.Point(181, 115);
            this.btnNo3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo3.Name = "btnNo3";
            this.btnNo3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo3.Size = new System.Drawing.Size(85, 53);
            this.btnNo3.TabIndex = 445;
            this.btnNo3.Tag = "3";
            this.btnNo3.Text = "3";
            this.btnNo3.UseVisualStyleBackColor = false;
            this.btnNo3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo2
            // 
            this.btnNo2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo2.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo2.Location = new System.Drawing.Point(92, 115);
            this.btnNo2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo2.Name = "btnNo2";
            this.btnNo2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo2.Size = new System.Drawing.Size(85, 53);
            this.btnNo2.TabIndex = 444;
            this.btnNo2.Tag = "2";
            this.btnNo2.Text = "2";
            this.btnNo2.UseVisualStyleBackColor = false;
            this.btnNo2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo1
            // 
            this.btnNo1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo1.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo1.Location = new System.Drawing.Point(3, 115);
            this.btnNo1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo1.Name = "btnNo1";
            this.btnNo1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo1.Size = new System.Drawing.Size(85, 53);
            this.btnNo1.TabIndex = 443;
            this.btnNo1.Tag = "1";
            this.btnNo1.Text = "1";
            this.btnNo1.UseVisualStyleBackColor = false;
            this.btnNo1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo6
            // 
            this.btnNo6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo6.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo6.Location = new System.Drawing.Point(181, 58);
            this.btnNo6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo6.Name = "btnNo6";
            this.btnNo6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo6.Size = new System.Drawing.Size(85, 53);
            this.btnNo6.TabIndex = 442;
            this.btnNo6.Tag = "6";
            this.btnNo6.Text = "6";
            this.btnNo6.UseVisualStyleBackColor = false;
            this.btnNo6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo5
            // 
            this.btnNo5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo5.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo5.Location = new System.Drawing.Point(92, 59);
            this.btnNo5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo5.Name = "btnNo5";
            this.btnNo5.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo5.Size = new System.Drawing.Size(85, 53);
            this.btnNo5.TabIndex = 441;
            this.btnNo5.Tag = "5";
            this.btnNo5.Text = "5";
            this.btnNo5.UseVisualStyleBackColor = false;
            this.btnNo5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo4
            // 
            this.btnNo4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo4.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo4.Location = new System.Drawing.Point(3, 59);
            this.btnNo4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo4.Name = "btnNo4";
            this.btnNo4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo4.Size = new System.Drawing.Size(85, 53);
            this.btnNo4.TabIndex = 440;
            this.btnNo4.Tag = "4";
            this.btnNo4.Text = "4";
            this.btnNo4.UseVisualStyleBackColor = false;
            this.btnNo4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo9
            // 
            this.btnNo9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo9.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo9.Location = new System.Drawing.Point(180, 2);
            this.btnNo9.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo9.Name = "btnNo9";
            this.btnNo9.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo9.Size = new System.Drawing.Size(85, 53);
            this.btnNo9.TabIndex = 439;
            this.btnNo9.Tag = "9";
            this.btnNo9.Text = "9";
            this.btnNo9.UseVisualStyleBackColor = false;
            this.btnNo9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo8
            // 
            this.btnNo8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo8.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo8.Location = new System.Drawing.Point(91, 3);
            this.btnNo8.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo8.Name = "btnNo8";
            this.btnNo8.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo8.Size = new System.Drawing.Size(85, 53);
            this.btnNo8.TabIndex = 438;
            this.btnNo8.Tag = "8";
            this.btnNo8.Text = "8";
            this.btnNo8.UseVisualStyleBackColor = false;
            this.btnNo8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnNo7
            // 
            this.btnNo7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo7.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNo7.Location = new System.Drawing.Point(2, 3);
            this.btnNo7.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNo7.Name = "btnNo7";
            this.btnNo7.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnNo7.Size = new System.Drawing.Size(85, 53);
            this.btnNo7.TabIndex = 437;
            this.btnNo7.Tag = "7";
            this.btnNo7.Text = "7";
            this.btnNo7.UseVisualStyleBackColor = false;
            this.btnNo7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo0_MouseDown);
            // 
            // btnBS
            // 
            this.btnBS.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBS.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBS.Location = new System.Drawing.Point(266, 3);
            this.btnBS.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnBS.Name = "btnBS";
            this.btnBS.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnBS.Size = new System.Drawing.Size(106, 53);
            this.btnBS.TabIndex = 436;
            this.btnBS.Text = "BS";
            this.btnBS.UseVisualStyleBackColor = false;
            this.btnBS.Click += new System.EventHandler(this.btnBS_Click);
            // 
            // btnCls
            // 
            this.btnCls.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCls.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCls.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCls.Location = new System.Drawing.Point(266, 58);
            this.btnCls.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCls.Name = "btnCls";
            this.btnCls.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnCls.Size = new System.Drawing.Size(106, 53);
            this.btnCls.TabIndex = 435;
            this.btnCls.Text = "CLS";
            this.btnCls.UseVisualStyleBackColor = false;
            this.btnCls.Click += new System.EventHandler(this.btnCls_Click);
            // 
            // btnSetPos
            // 
            this.btnSetPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSetPos.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnSetPos.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetPos.Image = ((System.Drawing.Image)(resources.GetObject("btnSetPos.Image")));
            this.btnSetPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSetPos.Location = new System.Drawing.Point(266, 171);
            this.btnSetPos.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSetPos.Name = "btnSetPos";
            this.btnSetPos.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnSetPos.Size = new System.Drawing.Size(106, 53);
            this.btnSetPos.TabIndex = 434;
            this.btnSetPos.Text = "Set Position";
            this.btnSetPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSetPos.UseVisualStyleBackColor = false;
            this.btnSetPos.Click += new System.EventHandler(this.btnSetPos_Click);
            // 
            // btnGetMotr
            // 
            this.btnGetMotr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnGetMotr.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetMotr.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetMotr.Location = new System.Drawing.Point(266, 115);
            this.btnGetMotr.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnGetMotr.Name = "btnGetMotr";
            this.btnGetMotr.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnGetMotr.Size = new System.Drawing.Size(106, 53);
            this.btnGetMotr.TabIndex = 433;
            this.btnGetMotr.Text = "Motor Position";
            this.btnGetMotr.UseVisualStyleBackColor = false;
            this.btnGetMotr.Click += new System.EventHandler(this.btnGetMotr_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnMinus);
            this.panel2.Controls.Add(this.btnPlus);
            this.panel2.Controls.Add(this.edIncDec);
            this.panel2.Controls.Add(this.edValue);
            this.panel2.Location = new System.Drawing.Point(5, 28);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(520, 42);
            this.panel2.TabIndex = 434;
            // 
            // btnMinus
            // 
            this.btnMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnMinus.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMinus.Location = new System.Drawing.Point(416, 3);
            this.btnMinus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnMinus.Size = new System.Drawing.Size(95, 33);
            this.btnMinus.TabIndex = 446;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = false;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnPlus.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlus.Location = new System.Drawing.Point(316, 3);
            this.btnPlus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnPlus.Size = new System.Drawing.Size(95, 33);
            this.btnPlus.TabIndex = 445;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // edIncDec
            // 
            this.edIncDec.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edIncDec.Location = new System.Drawing.Point(242, 3);
            this.edIncDec.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edIncDec.Name = "edIncDec";
            this.edIncDec.Size = new System.Drawing.Size(73, 36);
            this.edIncDec.TabIndex = 444;
            this.edIncDec.Text = "0";
            this.edIncDec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // edValue
            // 
            this.edValue.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edValue.Location = new System.Drawing.Point(3, 2);
            this.edValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edValue.Name = "edValue";
            this.edValue.Size = new System.Drawing.Size(236, 36);
            this.edValue.TabIndex = 443;
            this.edValue.Text = "0";
            this.edValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(384, 247);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnClose.Size = new System.Drawing.Size(138, 53);
            this.btnClose.TabIndex = 433;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.lbTitle.Size = new System.Drawing.Size(528, 26);
            this.lbTitle.TabIndex = 421;
            this.lbTitle.Text = "INPUT";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseDown);
            this.lbTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseMove);
            // 
            // Timer_Update
            // 
            this.Timer_Update.Tick += new System.EventHandler(this.Timer_Update_Tick);
            // 
            // FrmInputPos
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(528, 305);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmInputPos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormInputPos";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmInputPos_FormClosed);
            this.Load += new System.EventHandler(this.FrmInputPos_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnGetMotr;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnNo9;
        private System.Windows.Forms.Button btnNo8;
        private System.Windows.Forms.Button btnNo7;
        private System.Windows.Forms.Button btnBS;
        private System.Windows.Forms.Button btnCls;
        private System.Windows.Forms.Button btnSetPos;
        private System.Windows.Forms.Button btnPN;
        private System.Windows.Forms.Button btnDot;
        private System.Windows.Forms.Button btnNo0;
        private System.Windows.Forms.Button btnNo3;
        private System.Windows.Forms.Button btnNo2;
        private System.Windows.Forms.Button btnNo1;
        private System.Windows.Forms.Button btnNo6;
        private System.Windows.Forms.Button btnNo5;
        private System.Windows.Forms.Button btnNo4;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.TextBox edIncDec;
        private System.Windows.Forms.TextBox edValue;
        private System.Windows.Forms.TextBox edOldPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edMaxPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edMinPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edMotrPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer Timer_Update;
    }
}