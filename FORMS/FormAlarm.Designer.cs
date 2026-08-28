namespace eMachine
{
    partial class FrmAlarm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAlarm));
            this.lbTitle = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rtErrSolution = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rtErrCause = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lBoxError = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabControl();
            this.tabErrSub1 = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.btnSub1Close = new System.Windows.Forms.Button();
            this.lbSub1Msg = new System.Windows.Forms.Label();
            this.tabErrSub2 = new System.Windows.Forms.TabPage();
            this.btnErrSub2_1 = new System.Windows.Forms.Button();
            this.btnErrSub2_2 = new System.Windows.Forms.Button();
            this.lbSub2Msg = new System.Windows.Forms.Label();
            this.lbSub2Title = new System.Windows.Forms.Label();
            this.tabErrSub3 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnErrSub3_1 = new System.Windows.Forms.Button();
            this.btnErrSub3_2 = new System.Windows.Forms.Button();
            this.lbSub3Title = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbSubMsg3 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.imgError = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btUnknownAct1 = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabErrSub1.SuspendLayout();
            this.tabErrSub2.SuspendLayout();
            this.tabErrSub3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(920, 43);
            this.lbTitle.TabIndex = 425;
            this.lbTitle.Text = "          ALARM";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseDown);
            this.lbTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseMove);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.rtErrSolution);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.rtErrCause);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(400, 46);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(515, 187);
            this.panel4.TabIndex = 428;
            // 
            // rtErrSolution
            // 
            this.rtErrSolution.BackColor = System.Drawing.SystemColors.Info;
            this.rtErrSolution.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtErrSolution.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtErrSolution.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtErrSolution.Location = new System.Drawing.Point(0, 121);
            this.rtErrSolution.Margin = new System.Windows.Forms.Padding(1);
            this.rtErrSolution.Name = "rtErrSolution";
            this.rtErrSolution.ReadOnly = true;
            this.rtErrSolution.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtErrSolution.Size = new System.Drawing.Size(513, 68);
            this.rtErrSolution.TabIndex = 430;
            this.rtErrSolution.Text = "";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(0, 94);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(513, 27);
            this.label5.TabIndex = 429;
            this.label5.Text = "Solution";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtErrCause
            // 
            this.rtErrCause.BackColor = System.Drawing.SystemColors.Info;
            this.rtErrCause.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtErrCause.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtErrCause.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtErrCause.Location = new System.Drawing.Point(0, 26);
            this.rtErrCause.Margin = new System.Windows.Forms.Padding(1);
            this.rtErrCause.Name = "rtErrCause";
            this.rtErrCause.ReadOnly = true;
            this.rtErrCause.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtErrCause.Size = new System.Drawing.Size(513, 68);
            this.rtErrCause.TabIndex = 428;
            this.rtErrCause.Text = "";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(513, 26);
            this.label2.TabIndex = 421;
            this.label2.Text = "Cause";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lBoxError
            // 
            this.lBoxError.BackColor = System.Drawing.SystemColors.Info;
            this.lBoxError.FormattingEnabled = true;
            this.lBoxError.ItemHeight = 12;
            this.lBoxError.Location = new System.Drawing.Point(6, 46);
            this.lBoxError.Margin = new System.Windows.Forms.Padding(2);
            this.lBoxError.Name = "lBoxError";
            this.lBoxError.Size = new System.Drawing.Size(392, 88);
            this.lBoxError.TabIndex = 432;
            // 
            // tabPage3
            // 
            this.tabPage3.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabPage3.Controls.Add(this.tabErrSub1);
            this.tabPage3.Controls.Add(this.tabErrSub2);
            this.tabPage3.Controls.Add(this.tabErrSub3);
            this.tabPage3.Controls.Add(this.tabPage1);
            this.tabPage3.ItemSize = new System.Drawing.Size(100, 20);
            this.tabPage3.Location = new System.Drawing.Point(396, 236);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.SelectedIndex = 0;
            this.tabPage3.ShowToolTips = true;
            this.tabPage3.Size = new System.Drawing.Size(522, 175);
            this.tabPage3.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabPage3.TabIndex = 434;
            // 
            // tabErrSub1
            // 
            this.tabErrSub1.BackColor = System.Drawing.SystemColors.Info;
            this.tabErrSub1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabErrSub1.Controls.Add(this.button4);
            this.tabErrSub1.Controls.Add(this.btnSub1Close);
            this.tabErrSub1.Controls.Add(this.lbSub1Msg);
            this.tabErrSub1.Cursor = System.Windows.Forms.Cursors.No;
            this.tabErrSub1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabErrSub1.Location = new System.Drawing.Point(4, 4);
            this.tabErrSub1.Margin = new System.Windows.Forms.Padding(2);
            this.tabErrSub1.Name = "tabErrSub1";
            this.tabErrSub1.Padding = new System.Windows.Forms.Padding(2);
            this.tabErrSub1.Size = new System.Drawing.Size(514, 147);
            this.tabErrSub1.TabIndex = 0;
            this.tabErrSub1.Text = "tbsub1";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button4.Cursor = System.Windows.Forms.Cursors.Default;
            this.button4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(198, 91);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button4.Name = "button4";
            this.button4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.button4.Size = new System.Drawing.Size(154, 49);
            this.button4.TabIndex = 436;
            this.button4.Tag = "0";
            this.button4.Text = "Reset";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.btnSub1Reset_Click);
            // 
            // btnSub1Close
            // 
            this.btnSub1Close.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSub1Close.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSub1Close.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSub1Close.Image = ((System.Drawing.Image)(resources.GetObject("btnSub1Close.Image")));
            this.btnSub1Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSub1Close.Location = new System.Drawing.Point(354, 91);
            this.btnSub1Close.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSub1Close.Name = "btnSub1Close";
            this.btnSub1Close.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnSub1Close.Size = new System.Drawing.Size(154, 49);
            this.btnSub1Close.TabIndex = 437;
            this.btnSub1Close.Tag = "0";
            this.btnSub1Close.Text = "Close";
            this.btnSub1Close.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSub1Close.UseVisualStyleBackColor = false;
            this.btnSub1Close.Click += new System.EventHandler(this.btnSub1Reset_Click);
            // 
            // lbSub1Msg
            // 
            this.lbSub1Msg.BackColor = System.Drawing.Color.Transparent;
            this.lbSub1Msg.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSub1Msg.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSub1Msg.Location = new System.Drawing.Point(2, 2);
            this.lbSub1Msg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSub1Msg.Name = "lbSub1Msg";
            this.lbSub1Msg.Size = new System.Drawing.Size(508, 89);
            this.lbSub1Msg.TabIndex = 431;
            this.lbSub1Msg.Text = ".....";
            this.lbSub1Msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabErrSub2
            // 
            this.tabErrSub2.BackColor = System.Drawing.SystemColors.Info;
            this.tabErrSub2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabErrSub2.Controls.Add(this.btnErrSub2_1);
            this.tabErrSub2.Controls.Add(this.btnErrSub2_2);
            this.tabErrSub2.Controls.Add(this.lbSub2Msg);
            this.tabErrSub2.Controls.Add(this.lbSub2Title);
            this.tabErrSub2.Location = new System.Drawing.Point(4, 4);
            this.tabErrSub2.Margin = new System.Windows.Forms.Padding(2);
            this.tabErrSub2.Name = "tabErrSub2";
            this.tabErrSub2.Padding = new System.Windows.Forms.Padding(2);
            this.tabErrSub2.Size = new System.Drawing.Size(514, 147);
            this.tabErrSub2.TabIndex = 1;
            this.tabErrSub2.Text = "tbsub2";
            // 
            // btnErrSub2_1
            // 
            this.btnErrSub2_1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnErrSub2_1.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnErrSub2_1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnErrSub2_1.Image = ((System.Drawing.Image)(resources.GetObject("btnErrSub2_1.Image")));
            this.btnErrSub2_1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnErrSub2_1.Location = new System.Drawing.Point(200, 94);
            this.btnErrSub2_1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnErrSub2_1.Name = "btnErrSub2_1";
            this.btnErrSub2_1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnErrSub2_1.Size = new System.Drawing.Size(154, 49);
            this.btnErrSub2_1.TabIndex = 436;
            this.btnErrSub2_1.Tag = "0";
            this.btnErrSub2_1.Text = "Confirm";
            this.btnErrSub2_1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnErrSub2_1.UseVisualStyleBackColor = false;
            this.btnErrSub2_1.Click += new System.EventHandler(this.btnErrSub2_1_Click);
            // 
            // btnErrSub2_2
            // 
            this.btnErrSub2_2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnErrSub2_2.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnErrSub2_2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnErrSub2_2.Image = ((System.Drawing.Image)(resources.GetObject("btnErrSub2_2.Image")));
            this.btnErrSub2_2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnErrSub2_2.Location = new System.Drawing.Point(356, 94);
            this.btnErrSub2_2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnErrSub2_2.Name = "btnErrSub2_2";
            this.btnErrSub2_2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnErrSub2_2.Size = new System.Drawing.Size(154, 49);
            this.btnErrSub2_2.TabIndex = 437;
            this.btnErrSub2_2.Tag = "0";
            this.btnErrSub2_2.Text = "Cancel";
            this.btnErrSub2_2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnErrSub2_2.UseVisualStyleBackColor = false;
            this.btnErrSub2_2.Click += new System.EventHandler(this.btnSub1Close_Click);
            // 
            // lbSub2Msg
            // 
            this.lbSub2Msg.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSub2Msg.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSub2Msg.Location = new System.Drawing.Point(2, 31);
            this.lbSub2Msg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSub2Msg.Name = "lbSub2Msg";
            this.lbSub2Msg.Size = new System.Drawing.Size(508, 65);
            this.lbSub2Msg.TabIndex = 432;
            this.lbSub2Msg.Text = ".....";
            this.lbSub2Msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSub2Title
            // 
            this.lbSub2Title.BackColor = System.Drawing.Color.DimGray;
            this.lbSub2Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSub2Title.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSub2Title.ForeColor = System.Drawing.Color.Gold;
            this.lbSub2Title.Location = new System.Drawing.Point(2, 2);
            this.lbSub2Title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSub2Title.Name = "lbSub2Title";
            this.lbSub2Title.Size = new System.Drawing.Size(508, 29);
            this.lbSub2Title.TabIndex = 422;
            this.lbSub2Title.Text = "MESSAGE";
            this.lbSub2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabErrSub3
            // 
            this.tabErrSub3.BackColor = System.Drawing.SystemColors.Info;
            this.tabErrSub3.Controls.Add(this.button1);
            this.tabErrSub3.Controls.Add(this.btnErrSub3_1);
            this.tabErrSub3.Controls.Add(this.btnErrSub3_2);
            this.tabErrSub3.Controls.Add(this.lbSub3Title);
            this.tabErrSub3.Location = new System.Drawing.Point(4, 4);
            this.tabErrSub3.Name = "tabErrSub3";
            this.tabErrSub3.Padding = new System.Windows.Forms.Padding(3);
            this.tabErrSub3.Size = new System.Drawing.Size(514, 147);
            this.tabErrSub3.TabIndex = 2;
            this.tabErrSub3.Text = "tbsub3";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button1.Cursor = System.Windows.Forms.Cursors.Default;
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(44, 95);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.button1.Size = new System.Drawing.Size(154, 49);
            this.button1.TabIndex = 441;
            this.button1.Tag = "0";
            this.button1.Text = "Work End";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnErrSub3_1
            // 
            this.btnErrSub3_1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnErrSub3_1.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnErrSub3_1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnErrSub3_1.Image = ((System.Drawing.Image)(resources.GetObject("btnErrSub3_1.Image")));
            this.btnErrSub3_1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnErrSub3_1.Location = new System.Drawing.Point(202, 95);
            this.btnErrSub3_1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnErrSub3_1.Name = "btnErrSub3_1";
            this.btnErrSub3_1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnErrSub3_1.Size = new System.Drawing.Size(154, 49);
            this.btnErrSub3_1.TabIndex = 439;
            this.btnErrSub3_1.Tag = "1";
            this.btnErrSub3_1.Text = "Skip";
            this.btnErrSub3_1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnErrSub3_1.UseVisualStyleBackColor = false;
            this.btnErrSub3_1.Click += new System.EventHandler(this.btnErrSub3_1_Click);
            // 
            // btnErrSub3_2
            // 
            this.btnErrSub3_2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnErrSub3_2.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnErrSub3_2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnErrSub3_2.Image = ((System.Drawing.Image)(resources.GetObject("btnErrSub3_2.Image")));
            this.btnErrSub3_2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnErrSub3_2.Location = new System.Drawing.Point(358, 95);
            this.btnErrSub3_2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnErrSub3_2.Name = "btnErrSub3_2";
            this.btnErrSub3_2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnErrSub3_2.Size = new System.Drawing.Size(154, 49);
            this.btnErrSub3_2.TabIndex = 440;
            this.btnErrSub3_2.Tag = "2";
            this.btnErrSub3_2.Text = "Reset";
            this.btnErrSub3_2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnErrSub3_2.UseVisualStyleBackColor = false;
            this.btnErrSub3_2.Click += new System.EventHandler(this.btnSub1Close_Click);
            // 
            // lbSub3Title
            // 
            this.lbSub3Title.BackColor = System.Drawing.Color.DimGray;
            this.lbSub3Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSub3Title.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSub3Title.ForeColor = System.Drawing.Color.Gold;
            this.lbSub3Title.Location = new System.Drawing.Point(3, 3);
            this.lbSub3Title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSub3Title.Name = "lbSub3Title";
            this.lbSub3Title.Size = new System.Drawing.Size(508, 29);
            this.lbSub3Title.TabIndex = 423;
            this.lbSub3Title.Text = "MESSAGE";
            this.lbSub3Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Info;
            this.tabPage1.Controls.Add(this.btUnknownAct1);
            this.tabPage1.Controls.Add(this.lbSubMsg3);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(514, 147);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "tbsub4";
            // 
            // lbSubMsg3
            // 
            this.lbSubMsg3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSubMsg3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSubMsg3.Location = new System.Drawing.Point(3, 32);
            this.lbSubMsg3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbSubMsg3.Name = "lbSubMsg3";
            this.lbSubMsg3.Size = new System.Drawing.Size(508, 65);
            this.lbSubMsg3.TabIndex = 444;
            this.lbSubMsg3.Text = ".....";
            this.lbSubMsg3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DimGray;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Gold;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(508, 29);
            this.label3.TabIndex = 443;
            this.label3.Text = "MESSAGE";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button2.Cursor = System.Windows.Forms.Cursors.Default;
            this.button2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(219, 95);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.button2.Size = new System.Drawing.Size(140, 49);
            this.button2.TabIndex = 441;
            this.button2.Tag = "1";
            this.button2.Text = "Skip";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button3.Cursor = System.Windows.Forms.Cursors.Default;
            this.button3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(365, 95);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button3.Name = "button3";
            this.button3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.button3.Size = new System.Drawing.Size(140, 49);
            this.button3.TabIndex = 442;
            this.button3.Tag = "2";
            this.button3.Text = "Reset";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button2_Click);
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // imgError
            // 
            this.imgError.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.imgError.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgError.Location = new System.Drawing.Point(6, 137);
            this.imgError.Margin = new System.Windows.Forms.Padding(2);
            this.imgError.Name = "imgError";
            this.imgError.Size = new System.Drawing.Size(390, 250);
            this.imgError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgError.TabIndex = 433;
            this.imgError.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(3, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 43);
            this.pictureBox1.TabIndex = 430;
            this.pictureBox1.TabStop = false;
            // 
            // btUnknownAct1
            // 
            this.btUnknownAct1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btUnknownAct1.Cursor = System.Windows.Forms.Cursors.Default;
            this.btUnknownAct1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btUnknownAct1.Image = ((System.Drawing.Image)(resources.GetObject("btUnknownAct1.Image")));
            this.btUnknownAct1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btUnknownAct1.Location = new System.Drawing.Point(73, 95);
            this.btUnknownAct1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btUnknownAct1.Name = "btUnknownAct1";
            this.btUnknownAct1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btUnknownAct1.Size = new System.Drawing.Size(140, 49);
            this.btUnknownAct1.TabIndex = 445;
            this.btUnknownAct1.Tag = "0";
            this.btUnknownAct1.Text = "자재 제거";
            this.btUnknownAct1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btUnknownAct1.UseVisualStyleBackColor = false;
            this.btUnknownAct1.Click += new System.EventHandler(this.btUnknownAct1_Click);
            // 
            // FrmAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(920, 415);
            this.Controls.Add(this.tabPage3);
            this.Controls.Add(this.imgError);
            this.Controls.Add(this.lBoxError);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.lbTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmAlarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmAlarm";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAlarm_FormClosed);
            this.Load += new System.EventHandler(this.FormAlarm_Load);
            this.panel4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabErrSub1.ResumeLayout(false);
            this.tabErrSub2.ResumeLayout(false);
            this.tabErrSub3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.RichTextBox rtErrSolution;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.RichTextBox rtErrCause;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox lBoxError;
        private System.Windows.Forms.PictureBox imgError;
        private System.Windows.Forms.TabControl tabPage3;
        private System.Windows.Forms.TabPage tabErrSub1;
        private System.Windows.Forms.Label lbSub1Msg;
        private System.Windows.Forms.TabPage tabErrSub2;
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.Label lbSub2Title;
        private System.Windows.Forms.Button btnErrSub2_1;
        private System.Windows.Forms.Button btnErrSub2_2;
        private System.Windows.Forms.Label lbSub2Msg;
        private System.Windows.Forms.TabPage tabErrSub3;
        private System.Windows.Forms.Button btnErrSub3_1;
        private System.Windows.Forms.Button btnErrSub3_2;
        private System.Windows.Forms.Label lbSub3Title;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lbSubMsg3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnSub1Close;
        private System.Windows.Forms.Button btUnknownAct1;
    }
}