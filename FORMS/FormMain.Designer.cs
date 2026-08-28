namespace eMachine
{
    partial class FrmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.tmProc = new System.Windows.Forms.Timer(this.components);
            this.tmStat = new System.Windows.Forms.Timer(this.components);
            this.pnMainTop = new System.Windows.Forms.Panel();
            this.lbOnLine = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbCrntRecp = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btogMFunc5 = new System.Windows.Forms.KToggleButton(this.components);
            this.lbCrntLevel = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.pnMenuBtm = new System.Windows.Forms.Panel();
            this.lbBtmStat12 = new System.Windows.Forms.Label();
            this.lbBtmStat11 = new System.Windows.Forms.Label();
            this.lbBtmStat10 = new System.Windows.Forms.Label();
            this.lbBtmStat9 = new System.Windows.Forms.Label();
            this.lbBtmStat8 = new System.Windows.Forms.Label();
            this.lbBtmStat7 = new System.Windows.Forms.Label();
            this.lbBtmStat6 = new System.Windows.Forms.Label();
            this.lbBtmStat5 = new System.Windows.Forms.Label();
            this.lbBtmStat4 = new System.Windows.Forms.Label();
            this.lbBtmStat3 = new System.Windows.Forms.Label();
            this.lbBtmStat2 = new System.Windows.Forms.Label();
            this.lbBtmStat1 = new System.Windows.Forms.Label();
            this.lbCurDate = new System.Windows.Forms.Label();
            this.tmEffect = new System.Windows.Forms.Timer(this.components);
            this.pnMenu = new System.Windows.Forms.Panel();
            this.tbMainCon = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnBase = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstCom = new System.Windows.Forms.ListBox();
            this.btnMenu8 = new FontAwesome.Sharp.IconButton();
            this.btnMenu9 = new FontAwesome.Sharp.IconButton();
            this.btnMenu7 = new FontAwesome.Sharp.IconButton();
            this.btnMenu6 = new FontAwesome.Sharp.IconButton();
            this.btnMenu5 = new FontAwesome.Sharp.IconButton();
            this.btnMenu4 = new FontAwesome.Sharp.IconButton();
            this.btnMenu3 = new FontAwesome.Sharp.IconButton();
            this.btnMenu2 = new FontAwesome.Sharp.IconButton();
            this.btnMenu1 = new FontAwesome.Sharp.IconButton();
            this.lbComClient = new System.Windows.Forms.Label();
            this.lbStateBusy = new System.Windows.Forms.Label();
            this.lbComLight = new System.Windows.Forms.Label();
            this.lbComCarmera = new System.Windows.Forms.Label();
            this.lbStateAlarm = new System.Windows.Forms.Label();
            this.lbStateStop = new System.Windows.Forms.Label();
            this.lbStateAuto = new System.Windows.Forms.Label();
            this.lbStateInit = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btClose = new FontAwesome.Sharp.IconButton();
            this.pnMainTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnMenuBtm.SuspendLayout();
            this.pnMenu.SuspendLayout();
            this.tbMainCon.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tmProc
            // 
            this.tmProc.Tick += new System.EventHandler(this.tmProc_Tick);
            // 
            // tmStat
            // 
            this.tmStat.Tick += new System.EventHandler(this.tmMsg_Tick);
            // 
            // pnMainTop
            // 
            this.pnMainTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(98)))));
            this.pnMainTop.Controls.Add(this.lbOnLine);
            this.pnMainTop.Controls.Add(this.panel1);
            this.pnMainTop.Controls.Add(this.tableLayoutPanel1);
            this.pnMainTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnMainTop.Location = new System.Drawing.Point(0, 0);
            this.pnMainTop.Margin = new System.Windows.Forms.Padding(2);
            this.pnMainTop.Name = "pnMainTop";
            this.pnMainTop.Padding = new System.Windows.Forms.Padding(1);
            this.pnMainTop.Size = new System.Drawing.Size(1280, 80);
            this.pnMainTop.TabIndex = 7;
            // 
            // lbOnLine
            // 
            this.lbOnLine.BackColor = System.Drawing.Color.Green;
            this.lbOnLine.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbOnLine.ForeColor = System.Drawing.Color.White;
            this.lbOnLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbOnLine.Location = new System.Drawing.Point(8, 50);
            this.lbOnLine.Name = "lbOnLine";
            this.lbOnLine.Size = new System.Drawing.Size(215, 26);
            this.lbOnLine.TabIndex = 76;
            this.lbOnLine.Text = "OFFLINE";
            this.lbOnLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lbComClient);
            this.panel1.Controls.Add(this.lbStateBusy);
            this.panel1.Controls.Add(this.lbComLight);
            this.panel1.Controls.Add(this.lbComCarmera);
            this.panel1.Controls.Add(this.lbStateAlarm);
            this.panel1.Controls.Add(this.lbStateStop);
            this.panel1.Controls.Add(this.lbStateAuto);
            this.panel1.Controls.Add(this.lbStateInit);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbCrntRecp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(229, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1050, 31);
            this.panel1.TabIndex = 75;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.DimGray;
            this.label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Location = new System.Drawing.Point(2, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(1, 20);
            this.label12.TabIndex = 73;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(285, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 18);
            this.label4.TabIndex = 53;
            this.label4.Text = "[Recipe]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // lbCrntRecp
            // 
            this.lbCrntRecp.BackColor = System.Drawing.Color.Transparent;
            this.lbCrntRecp.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCrntRecp.ForeColor = System.Drawing.Color.White;
            this.lbCrntRecp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbCrntRecp.Location = new System.Drawing.Point(353, 5);
            this.lbCrntRecp.Name = "lbCrntRecp";
            this.lbCrntRecp.Size = new System.Drawing.Size(220, 18);
            this.lbCrntRecp.TabIndex = 54;
            this.lbCrntRecp.Text = "None";
            this.lbCrntRecp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.30573F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.69427F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 365F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.Controls.Add(this.btogMFunc5, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbCrntLevel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btClose, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1278, 47);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // btogMFunc5
            // 
            this.btogMFunc5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btogMFunc5.AutoCheck = false;
            this.btogMFunc5.BackColor = System.Drawing.Color.Transparent;
            this.btogMFunc5.Checked = false;
            this.btogMFunc5.FlatAppearance.BorderSize = 0;
            this.btogMFunc5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btogMFunc5.Font = new System.Drawing.Font("Tahoma", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btogMFunc5.ForeColor = System.Drawing.Color.White;
            this.btogMFunc5.LedFullEnable = false;
            this.btogMFunc5.LedVisible = false;
            this.btogMFunc5.LedWidth = 10;
            this.btogMFunc5.Location = new System.Drawing.Point(1133, 3);
            this.btogMFunc5.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btogMFunc5.Name = "btogMFunc5";
            this.btogMFunc5.OffColor = System.Drawing.Color.DarkGray;
            this.btogMFunc5.OnColor = System.Drawing.Color.DarkOrange;
            this.btogMFunc5.Padding = new System.Windows.Forms.Padding(4);
            this.btogMFunc5.RoundEdge = 5;
            this.btogMFunc5.Size = new System.Drawing.Size(108, 43);
            this.btogMFunc5.TabIndex = 1392;
            this.btogMFunc5.Tag = "4";
            this.btogMFunc5.Text2 = "Login";
            this.btogMFunc5.TextOff = "";
            this.btogMFunc5.TextOn = "";
            this.btogMFunc5.TextOnOffEnable = false;
            this.btogMFunc5.UseVisualStyleBackColor = false;
            this.btogMFunc5.Click += new System.EventHandler(this.btogMFunc5_Click);
            // 
            // lbCrntLevel
            // 
            this.lbCrntLevel.BackColor = System.Drawing.Color.Transparent;
            this.lbCrntLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCrntLevel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCrntLevel.ForeColor = System.Drawing.Color.White;
            this.lbCrntLevel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbCrntLevel.Location = new System.Drawing.Point(769, 0);
            this.lbCrntLevel.Margin = new System.Windows.Forms.Padding(4, 0, 2, 0);
            this.lbCrntLevel.Name = "lbCrntLevel";
            this.lbCrntLevel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbCrntLevel.Size = new System.Drawing.Size(359, 47);
            this.lbCrntLevel.TabIndex = 23;
            this.lbCrntLevel.Text = "Login Level : Operator";
            this.lbCrntLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.White;
            this.lbTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbTitle.Location = new System.Drawing.Point(128, 0);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(4, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lbTitle.Size = new System.Drawing.Size(635, 47);
            this.lbTitle.TabIndex = 21;
            this.lbTitle.Text = "PROJECT Ver 1.0";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbTitle.DoubleClick += new System.EventHandler(this.lbTitle_DoubleClick);
            // 
            // pnMenuBtm
            // 
            this.pnMenuBtm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(32)))), ((int)(((byte)(41)))));
            this.pnMenuBtm.Controls.Add(this.lbBtmStat12);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat11);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat10);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat9);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat8);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat7);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat6);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat5);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat4);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat3);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat2);
            this.pnMenuBtm.Controls.Add(this.lbBtmStat1);
            this.pnMenuBtm.Controls.Add(this.lbCurDate);
            this.pnMenuBtm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnMenuBtm.Location = new System.Drawing.Point(0, 944);
            this.pnMenuBtm.Margin = new System.Windows.Forms.Padding(2);
            this.pnMenuBtm.Name = "pnMenuBtm";
            this.pnMenuBtm.Size = new System.Drawing.Size(1280, 24);
            this.pnMenuBtm.TabIndex = 3;
            // 
            // lbBtmStat12
            // 
            this.lbBtmStat12.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat12.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat12.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat12.Location = new System.Drawing.Point(891, 0);
            this.lbBtmStat12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat12.Name = "lbBtmStat12";
            this.lbBtmStat12.Size = new System.Drawing.Size(239, 24);
            this.lbBtmStat12.TabIndex = 534;
            this.lbBtmStat12.Text = " ";
            this.lbBtmStat12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat11
            // 
            this.lbBtmStat11.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat11.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat11.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat11.Location = new System.Drawing.Point(810, 0);
            this.lbBtmStat11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat11.Name = "lbBtmStat11";
            this.lbBtmStat11.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat11.TabIndex = 533;
            this.lbBtmStat11.Text = "Vacuum";
            this.lbBtmStat11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat10
            // 
            this.lbBtmStat10.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat10.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat10.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat10.Location = new System.Drawing.Point(729, 0);
            this.lbBtmStat10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat10.Name = "lbBtmStat10";
            this.lbBtmStat10.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat10.TabIndex = 531;
            this.lbBtmStat10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat9
            // 
            this.lbBtmStat9.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat9.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat9.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat9.Location = new System.Drawing.Point(648, 0);
            this.lbBtmStat9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat9.Name = "lbBtmStat9";
            this.lbBtmStat9.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat9.TabIndex = 530;
            this.lbBtmStat9.Text = " ";
            this.lbBtmStat9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat8
            // 
            this.lbBtmStat8.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat8.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat8.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat8.Location = new System.Drawing.Point(567, 0);
            this.lbBtmStat8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat8.Name = "lbBtmStat8";
            this.lbBtmStat8.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat8.TabIndex = 529;
            this.lbBtmStat8.Text = " ";
            this.lbBtmStat8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat7
            // 
            this.lbBtmStat7.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat7.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat7.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat7.Location = new System.Drawing.Point(486, 0);
            this.lbBtmStat7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat7.Name = "lbBtmStat7";
            this.lbBtmStat7.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat7.TabIndex = 528;
            this.lbBtmStat7.Text = " ";
            this.lbBtmStat7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat6
            // 
            this.lbBtmStat6.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat6.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat6.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat6.Location = new System.Drawing.Point(405, 0);
            this.lbBtmStat6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat6.Name = "lbBtmStat6";
            this.lbBtmStat6.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat6.TabIndex = 525;
            this.lbBtmStat6.Text = " ";
            this.lbBtmStat6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat5
            // 
            this.lbBtmStat5.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat5.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat5.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat5.Location = new System.Drawing.Point(324, 0);
            this.lbBtmStat5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat5.Name = "lbBtmStat5";
            this.lbBtmStat5.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat5.TabIndex = 524;
            this.lbBtmStat5.Text = " ";
            this.lbBtmStat5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat4
            // 
            this.lbBtmStat4.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat4.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat4.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat4.Location = new System.Drawing.Point(243, 0);
            this.lbBtmStat4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat4.Name = "lbBtmStat4";
            this.lbBtmStat4.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat4.TabIndex = 523;
            this.lbBtmStat4.Text = " ";
            this.lbBtmStat4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat3
            // 
            this.lbBtmStat3.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat3.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat3.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat3.Location = new System.Drawing.Point(162, 0);
            this.lbBtmStat3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat3.Name = "lbBtmStat3";
            this.lbBtmStat3.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat3.TabIndex = 522;
            this.lbBtmStat3.Text = " ";
            this.lbBtmStat3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBtmStat2
            // 
            this.lbBtmStat2.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat2.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat2.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat2.Location = new System.Drawing.Point(81, 0);
            this.lbBtmStat2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat2.Name = "lbBtmStat2";
            this.lbBtmStat2.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat2.TabIndex = 521;
            this.lbBtmStat2.Text = " M0000";
            this.lbBtmStat2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBtmStat2.Click += new System.EventHandler(this.lbBtmStat2_Click);
            // 
            // lbBtmStat1
            // 
            this.lbBtmStat1.BackColor = System.Drawing.Color.Transparent;
            this.lbBtmStat1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBtmStat1.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBtmStat1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBtmStat1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBtmStat1.ForeColor = System.Drawing.Color.White;
            this.lbBtmStat1.Location = new System.Drawing.Point(0, 0);
            this.lbBtmStat1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBtmStat1.Name = "lbBtmStat1";
            this.lbBtmStat1.Size = new System.Drawing.Size(81, 24);
            this.lbBtmStat1.TabIndex = 520;
            this.lbBtmStat1.Text = "E0000";
            this.lbBtmStat1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBtmStat1.Click += new System.EventHandler(this.lbBtmStat1_Click);
            // 
            // lbCurDate
            // 
            this.lbCurDate.BackColor = System.Drawing.Color.Transparent;
            this.lbCurDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCurDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbCurDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbCurDate.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbCurDate.ForeColor = System.Drawing.Color.White;
            this.lbCurDate.Location = new System.Drawing.Point(1076, 0);
            this.lbCurDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbCurDate.Name = "lbCurDate";
            this.lbCurDate.Size = new System.Drawing.Size(204, 24);
            this.lbCurDate.TabIndex = 453;
            this.lbCurDate.Text = "00-00-00 00:00:00";
            this.lbCurDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tmEffect
            // 
            this.tmEffect.Tick += new System.EventHandler(this.tmEffect_Tick);
            // 
            // pnMenu
            // 
            this.pnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            this.pnMenu.Controls.Add(this.btnMenu8);
            this.pnMenu.Controls.Add(this.btnMenu9);
            this.pnMenu.Controls.Add(this.btnMenu7);
            this.pnMenu.Controls.Add(this.btnMenu6);
            this.pnMenu.Controls.Add(this.btnMenu5);
            this.pnMenu.Controls.Add(this.btnMenu4);
            this.pnMenu.Controls.Add(this.btnMenu3);
            this.pnMenu.Controls.Add(this.btnMenu2);
            this.pnMenu.Controls.Add(this.btnMenu1);
            this.pnMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnMenu.Location = new System.Drawing.Point(0, 892);
            this.pnMenu.Margin = new System.Windows.Forms.Padding(0);
            this.pnMenu.Name = "pnMenu";
            this.pnMenu.Size = new System.Drawing.Size(1280, 52);
            this.pnMenu.TabIndex = 12;
            // 
            // tbMainCon
            // 
            this.tbMainCon.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tbMainCon.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tbMainCon.Controls.Add(this.tabPage1);
            this.tbMainCon.Controls.Add(this.tabPage2);
            this.tbMainCon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMainCon.Font = new System.Drawing.Font("Arial", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMainCon.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tbMainCon.ItemSize = new System.Drawing.Size(0, 1);
            this.tbMainCon.Location = new System.Drawing.Point(0, 80);
            this.tbMainCon.Margin = new System.Windows.Forms.Padding(0);
            this.tbMainCon.Multiline = true;
            this.tbMainCon.Name = "tbMainCon";
            this.tbMainCon.SelectedIndex = 0;
            this.tbMainCon.Size = new System.Drawing.Size(1280, 812);
            this.tbMainCon.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnBase);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1271, 804);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pnBase
            // 
            this.pnBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.pnBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBase.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnBase.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.pnBase.Location = new System.Drawing.Point(3, 3);
            this.pnBase.Margin = new System.Windows.Forms.Padding(2);
            this.pnBase.Name = "pnBase";
            this.pnBase.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pnBase.Size = new System.Drawing.Size(1265, 798);
            this.pnBase.TabIndex = 14;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstCom);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1271, 804);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstCom
            // 
            this.lstCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCom.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstCom.ForeColor = System.Drawing.Color.Black;
            this.lstCom.FormattingEnabled = true;
            this.lstCom.HorizontalScrollbar = true;
            this.lstCom.ItemHeight = 16;
            this.lstCom.Location = new System.Drawing.Point(3, 3);
            this.lstCom.Name = "lstCom";
            this.lstCom.Size = new System.Drawing.Size(1265, 798);
            this.lstCom.TabIndex = 1526;
            // 
            // btnMenu8
            // 
            this.btnMenu8.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu8.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu8.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu8.FlatAppearance.BorderSize = 0;
            this.btnMenu8.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(86)))), ((int)(((byte)(130)))));
            this.btnMenu8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu8.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu8.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu8.IconChar = FontAwesome.Sharp.IconChar.Asterisk;
            this.btnMenu8.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu8.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu8.IconSize = 32;
            this.btnMenu8.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu8.Location = new System.Drawing.Point(1008, 0);
            this.btnMenu8.Name = "btnMenu8";
            this.btnMenu8.Size = new System.Drawing.Size(126, 52);
            this.btnMenu8.TabIndex = 26;
            this.btnMenu8.Tag = "8";
            this.btnMenu8.Text = "Master";
            this.btnMenu8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu8.UseVisualStyleBackColor = false;
            this.btnMenu8.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu8.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu8.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu9
            // 
            this.btnMenu9.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu9.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu9.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu9.FlatAppearance.BorderSize = 0;
            this.btnMenu9.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(86)))), ((int)(((byte)(130)))));
            this.btnMenu9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu9.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu9.IconChar = FontAwesome.Sharp.IconChar.Readme;
            this.btnMenu9.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu9.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu9.IconSize = 32;
            this.btnMenu9.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu9.Location = new System.Drawing.Point(882, 0);
            this.btnMenu9.Name = "btnMenu9";
            this.btnMenu9.Size = new System.Drawing.Size(126, 52);
            this.btnMenu9.TabIndex = 25;
            this.btnMenu9.Tag = "9";
            this.btnMenu9.Text = "TCP/IP Log";
            this.btnMenu9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu9.UseVisualStyleBackColor = false;
            this.btnMenu9.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu9.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu9.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu7
            // 
            this.btnMenu7.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu7.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu7.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu7.FlatAppearance.BorderSize = 0;
            this.btnMenu7.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu7.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu7.IconChar = FontAwesome.Sharp.IconChar.SlidersH;
            this.btnMenu7.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu7.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu7.IconSize = 32;
            this.btnMenu7.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu7.Location = new System.Drawing.Point(756, 0);
            this.btnMenu7.Name = "btnMenu7";
            this.btnMenu7.Size = new System.Drawing.Size(126, 52);
            this.btnMenu7.TabIndex = 23;
            this.btnMenu7.Tag = "7";
            this.btnMenu7.Text = "Setting";
            this.btnMenu7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu7.UseVisualStyleBackColor = false;
            this.btnMenu7.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu6
            // 
            this.btnMenu6.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu6.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu6.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu6.FlatAppearance.BorderSize = 0;
            this.btnMenu6.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu6.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu6.IconChar = FontAwesome.Sharp.IconChar.ChartArea;
            this.btnMenu6.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu6.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu6.IconSize = 32;
            this.btnMenu6.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu6.Location = new System.Drawing.Point(630, 0);
            this.btnMenu6.Name = "btnMenu6";
            this.btnMenu6.Size = new System.Drawing.Size(126, 52);
            this.btnMenu6.TabIndex = 22;
            this.btnMenu6.Tag = "6";
            this.btnMenu6.Text = "History";
            this.btnMenu6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu6.UseVisualStyleBackColor = false;
            this.btnMenu6.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu5
            // 
            this.btnMenu5.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu5.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu5.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu5.FlatAppearance.BorderSize = 0;
            this.btnMenu5.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu5.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu5.IconChar = FontAwesome.Sharp.IconChar.ListUl;
            this.btnMenu5.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu5.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu5.IconSize = 32;
            this.btnMenu5.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu5.Location = new System.Drawing.Point(504, 0);
            this.btnMenu5.Name = "btnMenu5";
            this.btnMenu5.Size = new System.Drawing.Size(126, 52);
            this.btnMenu5.TabIndex = 21;
            this.btnMenu5.Tag = "5";
            this.btnMenu5.Text = "IO";
            this.btnMenu5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu5.UseVisualStyleBackColor = false;
            this.btnMenu5.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu4
            // 
            this.btnMenu4.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu4.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu4.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu4.FlatAppearance.BorderSize = 0;
            this.btnMenu4.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu4.IconChar = FontAwesome.Sharp.IconChar.Gears;
            this.btnMenu4.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu4.IconSize = 32;
            this.btnMenu4.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu4.Location = new System.Drawing.Point(378, 0);
            this.btnMenu4.Name = "btnMenu4";
            this.btnMenu4.Size = new System.Drawing.Size(126, 52);
            this.btnMenu4.TabIndex = 20;
            this.btnMenu4.Tag = "4";
            this.btnMenu4.Text = "Motor";
            this.btnMenu4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu4.UseVisualStyleBackColor = false;
            this.btnMenu4.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu3
            // 
            this.btnMenu3.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu3.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu3.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu3.FlatAppearance.BorderSize = 0;
            this.btnMenu3.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu3.IconChar = FontAwesome.Sharp.IconChar.MapMarked;
            this.btnMenu3.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu3.IconSize = 32;
            this.btnMenu3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu3.Location = new System.Drawing.Point(252, 0);
            this.btnMenu3.Name = "btnMenu3";
            this.btnMenu3.Size = new System.Drawing.Size(126, 52);
            this.btnMenu3.TabIndex = 19;
            this.btnMenu3.Tag = "3";
            this.btnMenu3.Text = "Motion";
            this.btnMenu3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu3.UseVisualStyleBackColor = false;
            this.btnMenu3.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu2
            // 
            this.btnMenu2.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu2.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu2.FlatAppearance.BorderSize = 0;
            this.btnMenu2.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu2.IconChar = FontAwesome.Sharp.IconChar.ProjectDiagram;
            this.btnMenu2.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu2.IconSize = 32;
            this.btnMenu2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu2.Location = new System.Drawing.Point(126, 0);
            this.btnMenu2.Name = "btnMenu2";
            this.btnMenu2.Size = new System.Drawing.Size(126, 52);
            this.btnMenu2.TabIndex = 14;
            this.btnMenu2.Tag = "2";
            this.btnMenu2.Text = "Recipe";
            this.btnMenu2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu2.UseVisualStyleBackColor = false;
            this.btnMenu2.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // btnMenu1
            // 
            this.btnMenu1.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu1.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMenu1.FlatAppearance.BorderSize = 0;
            this.btnMenu1.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(71)))));
            this.btnMenu1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu1.IconChar = FontAwesome.Sharp.IconChar.Television;
            this.btnMenu1.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMenu1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMenu1.IconSize = 32;
            this.btnMenu1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMenu1.Location = new System.Drawing.Point(0, 0);
            this.btnMenu1.Name = "btnMenu1";
            this.btnMenu1.Size = new System.Drawing.Size(126, 52);
            this.btnMenu1.TabIndex = 9;
            this.btnMenu1.Tag = "1";
            this.btnMenu1.Text = "Home";
            this.btnMenu1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMenu1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnMenu1.UseVisualStyleBackColor = false;
            this.btnMenu1.MouseLeave += new System.EventHandler(this.btnMenu9_MouseLeave);
            this.btnMenu1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMenu9_MouseMove);
            this.btnMenu1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMenu1_MouseUp);
            // 
            // lbComClient
            // 
            this.lbComClient.BackColor = System.Drawing.Color.Transparent;
            this.lbComClient.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbComClient.ForeColor = System.Drawing.Color.White;
            this.lbComClient.Image = ((System.Drawing.Image)(resources.GetObject("lbComClient.Image")));
            this.lbComClient.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbComClient.Location = new System.Drawing.Point(9, 7);
            this.lbComClient.Name = "lbComClient";
            this.lbComClient.Size = new System.Drawing.Size(66, 18);
            this.lbComClient.TabIndex = 79;
            this.lbComClient.Text = "     Client";
            this.lbComClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbComClient.Click += new System.EventHandler(this.lbComClient_Click);
            // 
            // lbStateBusy
            // 
            this.lbStateBusy.BackColor = System.Drawing.Color.Transparent;
            this.lbStateBusy.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbStateBusy.ForeColor = System.Drawing.Color.White;
            this.lbStateBusy.Image = ((System.Drawing.Image)(resources.GetObject("lbStateBusy.Image")));
            this.lbStateBusy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbStateBusy.Location = new System.Drawing.Point(867, 5);
            this.lbStateBusy.Name = "lbStateBusy";
            this.lbStateBusy.Size = new System.Drawing.Size(75, 18);
            this.lbStateBusy.TabIndex = 79;
            this.lbStateBusy.Text = "    Busy";
            this.lbStateBusy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbComLight
            // 
            this.lbComLight.BackColor = System.Drawing.Color.Transparent;
            this.lbComLight.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbComLight.ForeColor = System.Drawing.Color.White;
            this.lbComLight.Image = ((System.Drawing.Image)(resources.GetObject("lbComLight.Image")));
            this.lbComLight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbComLight.Location = new System.Drawing.Point(174, 7);
            this.lbComLight.Name = "lbComLight";
            this.lbComLight.Size = new System.Drawing.Size(105, 18);
            this.lbComLight.TabIndex = 78;
            this.lbComLight.Text = "     Ilumination";
            this.lbComLight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbComLight.Click += new System.EventHandler(this.lbComLight_Click);
            // 
            // lbComCarmera
            // 
            this.lbComCarmera.BackColor = System.Drawing.Color.Transparent;
            this.lbComCarmera.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbComCarmera.ForeColor = System.Drawing.Color.White;
            this.lbComCarmera.Image = ((System.Drawing.Image)(resources.GetObject("lbComCarmera.Image")));
            this.lbComCarmera.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbComCarmera.Location = new System.Drawing.Point(81, 7);
            this.lbComCarmera.Name = "lbComCarmera";
            this.lbComCarmera.Size = new System.Drawing.Size(86, 18);
            this.lbComCarmera.TabIndex = 77;
            this.lbComCarmera.Text = "     Carmera";
            this.lbComCarmera.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbComCarmera.Click += new System.EventHandler(this.lbComCarmera_Click);
            // 
            // lbStateAlarm
            // 
            this.lbStateAlarm.BackColor = System.Drawing.Color.Transparent;
            this.lbStateAlarm.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbStateAlarm.ForeColor = System.Drawing.Color.White;
            this.lbStateAlarm.Image = ((System.Drawing.Image)(resources.GetObject("lbStateAlarm.Image")));
            this.lbStateAlarm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbStateAlarm.Location = new System.Drawing.Point(948, 5);
            this.lbStateAlarm.Name = "lbStateAlarm";
            this.lbStateAlarm.Size = new System.Drawing.Size(92, 18);
            this.lbStateAlarm.TabIndex = 76;
            this.lbStateAlarm.Text = "    ALARM";
            this.lbStateAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbStateStop
            // 
            this.lbStateStop.BackColor = System.Drawing.Color.Transparent;
            this.lbStateStop.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbStateStop.ForeColor = System.Drawing.Color.White;
            this.lbStateStop.Image = global::eMachine.Properties.Resources._Green15;
            this.lbStateStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbStateStop.Location = new System.Drawing.Point(772, 5);
            this.lbStateStop.Name = "lbStateStop";
            this.lbStateStop.Size = new System.Drawing.Size(89, 18);
            this.lbStateStop.TabIndex = 75;
            this.lbStateStop.Text = "    Manual";
            this.lbStateStop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbStateStop.Click += new System.EventHandler(this.lbStateStop_Click);
            // 
            // lbStateAuto
            // 
            this.lbStateAuto.BackColor = System.Drawing.Color.Transparent;
            this.lbStateAuto.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbStateAuto.ForeColor = System.Drawing.Color.White;
            this.lbStateAuto.Image = ((System.Drawing.Image)(resources.GetObject("lbStateAuto.Image")));
            this.lbStateAuto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbStateAuto.Location = new System.Drawing.Point(689, 5);
            this.lbStateAuto.Name = "lbStateAuto";
            this.lbStateAuto.Size = new System.Drawing.Size(77, 18);
            this.lbStateAuto.TabIndex = 74;
            this.lbStateAuto.Text = "    Auto";
            this.lbStateAuto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbStateAuto.Click += new System.EventHandler(this.lbStateAuto_Click);
            // 
            // lbStateInit
            // 
            this.lbStateInit.BackColor = System.Drawing.Color.Transparent;
            this.lbStateInit.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbStateInit.ForeColor = System.Drawing.Color.White;
            this.lbStateInit.Image = ((System.Drawing.Image)(resources.GetObject("lbStateInit.Image")));
            this.lbStateInit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbStateInit.Location = new System.Drawing.Point(577, 5);
            this.lbStateInit.Name = "lbStateInit";
            this.lbStateInit.Size = new System.Drawing.Size(106, 18);
            this.lbStateInit.TabIndex = 48;
            this.lbStateInit.Text = "    Initialized";
            this.lbStateInit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::eMachine.Properties.Resources.DADA_S_removebg_preview;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(118, 41);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.BackColor = System.Drawing.Color.Transparent;
            this.btClose.FlatAppearance.BorderSize = 0;
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClose.IconChar = FontAwesome.Sharp.IconChar.PowerOff;
            this.btClose.IconColor = System.Drawing.Color.Red;
            this.btClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btClose.IconSize = 32;
            this.btClose.Location = new System.Drawing.Point(1244, 0);
            this.btClose.Margin = new System.Windows.Forms.Padding(0);
            this.btClose.Name = "btClose";
            this.btClose.Padding = new System.Windows.Forms.Padding(1, 7, 1, 1);
            this.btClose.Size = new System.Drawing.Size(34, 47);
            this.btClose.TabIndex = 15;
            this.btClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btClose.UseVisualStyleBackColor = false;
            this.btClose.Click += new System.EventHandler(this.btnMenu8_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(67)))), ((int)(((byte)(95)))));
            this.ClientSize = new System.Drawing.Size(1280, 968);
            this.ControlBox = false;
            this.Controls.Add(this.tbMainCon);
            this.Controls.Add(this.pnMenu);
            this.Controls.Add(this.pnMainTop);
            this.Controls.Add(this.pnMenuBtm);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(1280, 1022);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DADA";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmMain_VisibleChanged);
            this.pnMainTop.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnMenuBtm.ResumeLayout(false);
            this.pnMenu.ResumeLayout(false);
            this.tbMainCon.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMenuBtm;
        private System.Windows.Forms.Panel pnMainTop;
        private System.Windows.Forms.Timer tmProc;
        private System.Windows.Forms.Timer tmStat;
        private System.Windows.Forms.Label lbBtmStat6;
        private System.Windows.Forms.Label lbBtmStat5;
        private System.Windows.Forms.Label lbBtmStat4;
        private System.Windows.Forms.Label lbBtmStat3;
        private System.Windows.Forms.Label lbBtmStat2;
        private System.Windows.Forms.Label lbBtmStat1;
        private System.Windows.Forms.Label lbCurDate;
        private System.Windows.Forms.Label lbBtmStat8;
        private System.Windows.Forms.Label lbBtmStat7;
        private System.Windows.Forms.Label lbBtmStat9;
        private System.Windows.Forms.Label lbBtmStat10;
        private System.Windows.Forms.Timer tmEffect;
        private FontAwesome.Sharp.IconButton btClose;
        private System.Windows.Forms.Panel pnMenu;
        private FontAwesome.Sharp.IconButton btnMenu7;
        private FontAwesome.Sharp.IconButton btnMenu6;
        private FontAwesome.Sharp.IconButton btnMenu5;
        private FontAwesome.Sharp.IconButton btnMenu4;
        private FontAwesome.Sharp.IconButton btnMenu3;
        private FontAwesome.Sharp.IconButton btnMenu2;
        private FontAwesome.Sharp.IconButton btnMenu1;
        private System.Windows.Forms.Label lbBtmStat12;
        private System.Windows.Forms.Label lbBtmStat11;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbCrntLevel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbComClient;
        private System.Windows.Forms.Label lbStateBusy;
        private System.Windows.Forms.Label lbComLight;
        private System.Windows.Forms.Label lbComCarmera;
        private System.Windows.Forms.Label lbStateAlarm;
        private System.Windows.Forms.Label lbStateStop;
        private System.Windows.Forms.Label lbStateAuto;
        private System.Windows.Forms.Label lbStateInit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbCrntRecp;
        private System.Windows.Forms.Label lbOnLine;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.KToggleButton btogMFunc5;
        private FontAwesome.Sharp.IconButton btnMenu9;
        private FontAwesome.Sharp.IconButton btnMenu8;
        private System.Windows.Forms.TabControl tbMainCon;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pnBase;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lstCom;
    }
}

