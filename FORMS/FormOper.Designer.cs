namespace eMachine
{
    partial class FormOper
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewData = new System.Windows.Forms.ListView();
            this.No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StartTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EndTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultXmm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultYmm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultTmm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Mode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultBarcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btOper09 = new System.Windows.Forms.KToggleButton(this.components);
            this.btOper07 = new System.Windows.Forms.KToggleButton(this.components);
            this.btOper08 = new System.Windows.Forms.KToggleButton(this.components);
            this.pnVision = new System.Windows.Forms.Panel();
            this.pnLightCon = new System.Windows.Forms.RoundPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnImageSave = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.pnSkipWafer = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pnSkipVac = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btOper06 = new System.Windows.Forms.KToggleButton(this.components);
            this.btOper05 = new System.Windows.Forms.KToggleButton(this.components);
            this.btOper04 = new System.Windows.Forms.KToggleButton(this.components);
            this.btOper03 = new System.Windows.Forms.KToggleButton(this.components);
            this.btOper02 = new System.Windows.Forms.KToggleButton(this.components);
            this.btOper01 = new System.Windows.Forms.KToggleButton(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbAxisPositionT = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbAxisPositionY = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAxisPositionX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnMsg = new System.Windows.Forms.Panel();
            this.lbMsg = new System.Windows.Forms.Label();
            this.pnDisplayWarn = new System.Windows.Forms.Panel();
            this.lbDispWarn = new System.Windows.Forms.ListBox();
            this.lbWarning = new System.Windows.Forms.Label();
            this.pnImageTest = new System.Windows.Forms.RoundPanel();
            this.button4 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnVision.SuspendLayout();
            this.pnLightCon.SuspendLayout();
            this.pnImageSave.SuspendLayout();
            this.pnSkipWafer.SuspendLayout();
            this.pnSkipVac.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnMsg.SuspendLayout();
            this.pnDisplayWarn.SuspendLayout();
            this.pnImageTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.listViewData, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 500);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1255, 379);
            this.tableLayoutPanel1.TabIndex = 152;
            // 
            // listViewData
            // 
            this.listViewData.BackColor = System.Drawing.Color.LightGray;
            this.listViewData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.No,
            this.StartTime,
            this.EndTime,
            this.TotalTime,
            this.ResultXmm,
            this.ResultYmm,
            this.ResultTmm,
            this.Mode,
            this.Type,
            this.ResultBarcode,
            this.ResultType,
            this.Description});
            this.listViewData.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewData.ForeColor = System.Drawing.Color.Black;
            this.listViewData.FullRowSelect = true;
            this.listViewData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewData.HideSelection = false;
            this.listViewData.Location = new System.Drawing.Point(3, 3);
            this.listViewData.MultiSelect = false;
            this.listViewData.Name = "listViewData";
            this.listViewData.Size = new System.Drawing.Size(1249, 363);
            this.listViewData.TabIndex = 152;
            this.listViewData.UseCompatibleStateImageBehavior = false;
            this.listViewData.View = System.Windows.Forms.View.Details;
            // 
            // No
            // 
            this.No.Text = "No";
            // 
            // StartTime
            // 
            this.StartTime.Text = "Start Time";
            this.StartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StartTime.Width = 120;
            // 
            // EndTime
            // 
            this.EndTime.Text = "End Time";
            this.EndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.EndTime.Width = 120;
            // 
            // TotalTime
            // 
            this.TotalTime.Text = "Total Time";
            this.TotalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TotalTime.Width = 100;
            // 
            // ResultXmm
            // 
            this.ResultXmm.Text = "X-Result (mm)";
            this.ResultXmm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ResultXmm.Width = 110;
            // 
            // ResultYmm
            // 
            this.ResultYmm.Text = "Y-Result (mm)";
            this.ResultYmm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ResultYmm.Width = 110;
            // 
            // ResultTmm
            // 
            this.ResultTmm.Text = "T-Result (°)";
            this.ResultTmm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ResultTmm.Width = 110;
            // 
            // Mode
            // 
            this.Mode.Text = "Mode";
            this.Mode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Mode.Width = 70;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Type.Width = 70;
            // 
            // ResultBarcode
            // 
            this.ResultBarcode.Text = "BarCode";
            this.ResultBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ResultBarcode.Width = 180;
            // 
            // ResultType
            // 
            this.ResultType.Text = "Step";
            this.ResultType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ResultType.Width = 100;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Description.Width = 120;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 10;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Controls.Add(this.btOper09, 9, 0);
            this.tableLayoutPanel2.Controls.Add(this.btOper07, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btOper08, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 372);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1249, 4);
            this.tableLayoutPanel2.TabIndex = 153;
            // 
            // btOper09
            // 
            this.btOper09.AutoCheck = false;
            this.btOper09.BackColor = System.Drawing.Color.Silver;
            this.btOper09.Checked = false;
            this.btOper09.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOper09.FlatAppearance.BorderSize = 0;
            this.btOper09.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper09.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOper09.ForeColor = System.Drawing.Color.Black;
            this.btOper09.LedFullEnable = false;
            this.btOper09.LedVisible = false;
            this.btOper09.LedWidth = 10;
            this.btOper09.Location = new System.Drawing.Point(1119, 3);
            this.btOper09.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper09.Name = "btOper09";
            this.btOper09.OffColor = System.Drawing.Color.DarkGray;
            this.btOper09.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper09.Padding = new System.Windows.Forms.Padding(4);
            this.btOper09.RoundEdge = 5;
            this.btOper09.Size = new System.Drawing.Size(127, 1);
            this.btOper09.TabIndex = 1395;
            this.btOper09.Tag = "6";
            this.btOper09.Text2 = "Save CSV";
            this.btOper09.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btOper09.TextOff = "";
            this.btOper09.TextOn = "";
            this.btOper09.TextOnOffEnable = false;
            this.btOper09.UseVisualStyleBackColor = false;
            this.btOper09.Visible = false;
            this.btOper09.Click += new System.EventHandler(this.btOper03_Click);
            // 
            // btOper07
            // 
            this.btOper07.AutoCheck = false;
            this.btOper07.BackColor = System.Drawing.Color.Silver;
            this.btOper07.Checked = false;
            this.btOper07.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOper07.FlatAppearance.BorderSize = 0;
            this.btOper07.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper07.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOper07.ForeColor = System.Drawing.Color.Black;
            this.btOper07.LedFullEnable = false;
            this.btOper07.LedVisible = false;
            this.btOper07.LedWidth = 10;
            this.btOper07.Location = new System.Drawing.Point(3, 3);
            this.btOper07.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper07.Name = "btOper07";
            this.btOper07.OffColor = System.Drawing.Color.DarkGray;
            this.btOper07.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper07.Padding = new System.Windows.Forms.Padding(4);
            this.btOper07.RoundEdge = 5;
            this.btOper07.Size = new System.Drawing.Size(118, 1);
            this.btOper07.TabIndex = 1393;
            this.btOper07.Tag = "6";
            this.btOper07.Text2 = "Full";
            this.btOper07.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btOper07.TextOff = "";
            this.btOper07.TextOn = "";
            this.btOper07.TextOnOffEnable = false;
            this.btOper07.UseVisualStyleBackColor = false;
            this.btOper07.Visible = false;
            this.btOper07.Click += new System.EventHandler(this.btOper03_Click);
            // 
            // btOper08
            // 
            this.btOper08.AutoCheck = false;
            this.btOper08.BackColor = System.Drawing.Color.Silver;
            this.btOper08.Checked = false;
            this.btOper08.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOper08.FlatAppearance.BorderSize = 0;
            this.btOper08.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper08.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOper08.ForeColor = System.Drawing.Color.Black;
            this.btOper08.LedFullEnable = false;
            this.btOper08.LedVisible = false;
            this.btOper08.LedWidth = 10;
            this.btOper08.Location = new System.Drawing.Point(127, 3);
            this.btOper08.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper08.Name = "btOper08";
            this.btOper08.OffColor = System.Drawing.Color.DarkGray;
            this.btOper08.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper08.Padding = new System.Windows.Forms.Padding(4);
            this.btOper08.RoundEdge = 5;
            this.btOper08.Size = new System.Drawing.Size(118, 1);
            this.btOper08.TabIndex = 1393;
            this.btOper08.Tag = "6";
            this.btOper08.Text2 = "Hide";
            this.btOper08.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btOper08.TextOff = "";
            this.btOper08.TextOn = "";
            this.btOper08.TextOnOffEnable = false;
            this.btOper08.UseVisualStyleBackColor = false;
            this.btOper08.Visible = false;
            this.btOper08.Click += new System.EventHandler(this.btOper03_Click);
            // 
            // pnVision
            // 
            this.pnVision.BackColor = System.Drawing.Color.LightGray;
            this.pnVision.Controls.Add(this.pnImageTest);
            this.pnVision.Controls.Add(this.pnLightCon);
            this.pnVision.Controls.Add(this.pnImageSave);
            this.pnVision.Controls.Add(this.pnSkipWafer);
            this.pnVision.Controls.Add(this.pnSkipVac);
            this.pnVision.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnVision.Location = new System.Drawing.Point(0, 0);
            this.pnVision.Name = "pnVision";
            this.pnVision.Size = new System.Drawing.Size(1255, 448);
            this.pnVision.TabIndex = 153;
            // 
            // pnLightCon
            // 
            this.pnLightCon.BackColor = System.Drawing.Color.Transparent;
            this.pnLightCon.Controls.Add(this.button2);
            this.pnLightCon.Controls.Add(this.button1);
            this.pnLightCon.Location = new System.Drawing.Point(15, 192);
            this.pnLightCon.Name = "pnLightCon";
            this.pnLightCon.Radious = 15;
            this.pnLightCon.Size = new System.Drawing.Size(157, 126);
            this.pnLightCon.TabIndex = 607;
            this.pnLightCon.TabStop = false;
            this.pnLightCon.Text = "Light Control";
            this.pnLightCon.TitleBackColor = System.Drawing.Color.SteelBlue;
            this.pnLightCon.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnLightCon.TitleForeColor = System.Drawing.Color.White;
            this.pnLightCon.Visible = false;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(20, 80);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 39);
            this.button2.TabIndex = 172;
            this.button2.Text = "Off";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(20, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 39);
            this.button1.TabIndex = 172;
            this.button1.Text = "On";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnImageSave
            // 
            this.pnImageSave.Controls.Add(this.label6);
            this.pnImageSave.Location = new System.Drawing.Point(12, 145);
            this.pnImageSave.Name = "pnImageSave";
            this.pnImageSave.Size = new System.Drawing.Size(170, 30);
            this.pnImageSave.TabIndex = 171;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Yellow;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 30);
            this.label6.TabIndex = 0;
            this.label6.Text = "All Image Save...";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnSkipWafer
            // 
            this.pnSkipWafer.Controls.Add(this.label5);
            this.pnSkipWafer.Location = new System.Drawing.Point(12, 94);
            this.pnSkipWafer.Name = "pnSkipWafer";
            this.pnSkipWafer.Size = new System.Drawing.Size(170, 30);
            this.pnSkipWafer.TabIndex = 171;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Red;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Yellow;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "Skip Wafer";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnSkipVac
            // 
            this.pnSkipVac.Controls.Add(this.label3);
            this.pnSkipVac.Location = new System.Drawing.Point(12, 58);
            this.pnSkipVac.Name = "pnSkipVac";
            this.pnSkipVac.Size = new System.Drawing.Size(170, 30);
            this.pnSkipVac.TabIndex = 171;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Red;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Yellow;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "Skip Vacuum";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(98)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btOper06);
            this.panel2.Controls.Add(this.btOper05);
            this.panel2.Controls.Add(this.btOper04);
            this.panel2.Controls.Add(this.btOper03);
            this.panel2.Controls.Add(this.btOper02);
            this.panel2.Controls.Add(this.btOper01);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbAxisPositionT);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.lbAxisPositionY);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lbAxisPositionX);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 458);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1255, 42);
            this.panel2.TabIndex = 170;
            // 
            // btOper06
            // 
            this.btOper06.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOper06.AutoCheck = false;
            this.btOper06.BackColor = System.Drawing.Color.Transparent;
            this.btOper06.Checked = false;
            this.btOper06.FlatAppearance.BorderSize = 0;
            this.btOper06.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkOrange;
            this.btOper06.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper06.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOper06.ForeColor = System.Drawing.Color.White;
            this.btOper06.Image = global::eMachine.Properties.Resources.lbReset_Image;
            this.btOper06.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOper06.LedFullEnable = false;
            this.btOper06.LedVisible = false;
            this.btOper06.LedWidth = 10;
            this.btOper06.Location = new System.Drawing.Point(1121, 4);
            this.btOper06.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper06.Name = "btOper06";
            this.btOper06.OffColor = System.Drawing.Color.Gray;
            this.btOper06.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper06.Padding = new System.Windows.Forms.Padding(1);
            this.btOper06.RoundEdge = 5;
            this.btOper06.Size = new System.Drawing.Size(130, 33);
            this.btOper06.TabIndex = 1391;
            this.btOper06.Tag = "2";
            this.btOper06.Text2 = "Reset";
            this.btOper06.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btOper06.TextOff = "";
            this.btOper06.TextOn = "";
            this.btOper06.TextOnOffEnable = false;
            this.btOper06.UseVisualStyleBackColor = false;
            this.btOper06.Click += new System.EventHandler(this.btOper03_Click);
            this.btOper06.MouseLeave += new System.EventHandler(this.btOper03_MouseLeave);
            this.btOper06.MouseHover += new System.EventHandler(this.btOper03_MouseHover);
            // 
            // btOper05
            // 
            this.btOper05.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOper05.AutoCheck = false;
            this.btOper05.BackColor = System.Drawing.Color.Transparent;
            this.btOper05.Checked = false;
            this.btOper05.FlatAppearance.BorderSize = 0;
            this.btOper05.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkOrange;
            this.btOper05.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper05.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOper05.ForeColor = System.Drawing.Color.White;
            this.btOper05.Image = global::eMachine.Properties.Resources.lbAlign_Image;
            this.btOper05.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOper05.LedFullEnable = false;
            this.btOper05.LedVisible = false;
            this.btOper05.LedWidth = 10;
            this.btOper05.Location = new System.Drawing.Point(985, 4);
            this.btOper05.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper05.Name = "btOper05";
            this.btOper05.OffColor = System.Drawing.Color.Gray;
            this.btOper05.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper05.Padding = new System.Windows.Forms.Padding(1);
            this.btOper05.RoundEdge = 5;
            this.btOper05.Size = new System.Drawing.Size(130, 33);
            this.btOper05.TabIndex = 1391;
            this.btOper05.Tag = "2";
            this.btOper05.Text2 = "     Align";
            this.btOper05.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btOper05.TextOff = "";
            this.btOper05.TextOn = "";
            this.btOper05.TextOnOffEnable = false;
            this.btOper05.UseVisualStyleBackColor = false;
            this.btOper05.Click += new System.EventHandler(this.btOper03_Click);
            this.btOper05.MouseLeave += new System.EventHandler(this.btOper03_MouseLeave);
            this.btOper05.MouseHover += new System.EventHandler(this.btOper03_MouseHover);
            // 
            // btOper04
            // 
            this.btOper04.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOper04.AutoCheck = false;
            this.btOper04.BackColor = System.Drawing.Color.Transparent;
            this.btOper04.Checked = false;
            this.btOper04.FlatAppearance.BorderSize = 0;
            this.btOper04.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkOrange;
            this.btOper04.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper04.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOper04.ForeColor = System.Drawing.Color.White;
            this.btOper04.Image = global::eMachine.Properties.Resources.lbHome_Image;
            this.btOper04.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOper04.LedFullEnable = false;
            this.btOper04.LedVisible = false;
            this.btOper04.LedWidth = 10;
            this.btOper04.Location = new System.Drawing.Point(849, 4);
            this.btOper04.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper04.Name = "btOper04";
            this.btOper04.OffColor = System.Drawing.Color.Gray;
            this.btOper04.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper04.Padding = new System.Windows.Forms.Padding(1);
            this.btOper04.RoundEdge = 5;
            this.btOper04.Size = new System.Drawing.Size(130, 33);
            this.btOper04.TabIndex = 1391;
            this.btOper04.Tag = "2";
            this.btOper04.Text2 = "     Home";
            this.btOper04.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btOper04.TextOff = "";
            this.btOper04.TextOn = "";
            this.btOper04.TextOnOffEnable = false;
            this.btOper04.UseVisualStyleBackColor = false;
            this.btOper04.Click += new System.EventHandler(this.btOper03_Click);
            this.btOper04.MouseLeave += new System.EventHandler(this.btOper03_MouseLeave);
            this.btOper04.MouseHover += new System.EventHandler(this.btOper03_MouseHover);
            // 
            // btOper03
            // 
            this.btOper03.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOper03.AutoCheck = false;
            this.btOper03.BackColor = System.Drawing.Color.Transparent;
            this.btOper03.Checked = false;
            this.btOper03.FlatAppearance.BorderSize = 0;
            this.btOper03.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkOrange;
            this.btOper03.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper03.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOper03.ForeColor = System.Drawing.Color.White;
            this.btOper03.Image = global::eMachine.Properties.Resources.lbVacuum_Image;
            this.btOper03.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOper03.LedFullEnable = false;
            this.btOper03.LedVisible = false;
            this.btOper03.LedWidth = 20;
            this.btOper03.Location = new System.Drawing.Point(713, 4);
            this.btOper03.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper03.Name = "btOper03";
            this.btOper03.OffColor = System.Drawing.Color.Gray;
            this.btOper03.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btOper03.Padding = new System.Windows.Forms.Padding(1);
            this.btOper03.RoundEdge = 5;
            this.btOper03.Size = new System.Drawing.Size(130, 33);
            this.btOper03.TabIndex = 1391;
            this.btOper03.Tag = "2";
            this.btOper03.Text2 = "     Vacuum";
            this.btOper03.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btOper03.TextOff = "";
            this.btOper03.TextOn = "";
            this.btOper03.TextOnOffEnable = false;
            this.btOper03.UseVisualStyleBackColor = false;
            this.btOper03.Click += new System.EventHandler(this.btOper03_Click);
            this.btOper03.MouseLeave += new System.EventHandler(this.btOper03_MouseLeave);
            this.btOper03.MouseHover += new System.EventHandler(this.btOper03_MouseHover);
            // 
            // btOper02
            // 
            this.btOper02.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOper02.AutoCheck = false;
            this.btOper02.BackColor = System.Drawing.Color.Transparent;
            this.btOper02.Checked = false;
            this.btOper02.FlatAppearance.BorderSize = 0;
            this.btOper02.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkOrange;
            this.btOper02.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper02.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btOper02.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOper02.ForeColor = System.Drawing.Color.White;
            this.btOper02.Image = global::eMachine.Properties.Resources._Gray_20;
            this.btOper02.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOper02.LedFullEnable = false;
            this.btOper02.LedVisible = false;
            this.btOper02.LedWidth = 10;
            this.btOper02.Location = new System.Drawing.Point(544, 5);
            this.btOper02.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper02.Name = "btOper02";
            this.btOper02.OffColor = System.Drawing.Color.Gray;
            this.btOper02.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper02.Padding = new System.Windows.Forms.Padding(1);
            this.btOper02.RoundEdge = 5;
            this.btOper02.Size = new System.Drawing.Size(153, 29);
            this.btOper02.TabIndex = 1391;
            this.btOper02.Tag = "2";
            this.btOper02.Text2 = "    Vacuum Sensor";
            this.btOper02.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btOper02.TextOff = "";
            this.btOper02.TextOn = "";
            this.btOper02.TextOnOffEnable = false;
            this.btOper02.UseVisualStyleBackColor = false;
            // 
            // btOper01
            // 
            this.btOper01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOper01.AutoCheck = false;
            this.btOper01.BackColor = System.Drawing.Color.Transparent;
            this.btOper01.Checked = false;
            this.btOper01.FlatAppearance.BorderSize = 0;
            this.btOper01.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkOrange;
            this.btOper01.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.btOper01.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btOper01.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOper01.ForeColor = System.Drawing.Color.White;
            this.btOper01.Image = global::eMachine.Properties.Resources._Gray_20;
            this.btOper01.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOper01.LedFullEnable = false;
            this.btOper01.LedVisible = false;
            this.btOper01.LedWidth = 10;
            this.btOper01.Location = new System.Drawing.Point(418, 5);
            this.btOper01.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.btOper01.Name = "btOper01";
            this.btOper01.OffColor = System.Drawing.Color.Gray;
            this.btOper01.OnColor = System.Drawing.Color.DarkOrange;
            this.btOper01.Padding = new System.Windows.Forms.Padding(4);
            this.btOper01.RoundEdge = 5;
            this.btOper01.Size = new System.Drawing.Size(123, 29);
            this.btOper01.TabIndex = 1391;
            this.btOper01.Tag = "2";
            this.btOper01.Text2 = "    Wafer Exist";
            this.btOper01.TextOff = "";
            this.btOper01.TextOn = "";
            this.btOper01.TextOnOffEnable = false;
            this.btOper01.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(705, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(2, 31);
            this.label8.TabIndex = 180;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(411, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(2, 31);
            this.label1.TabIndex = 173;
            // 
            // lbAxisPositionT
            // 
            this.lbAxisPositionT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAxisPositionT.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAxisPositionT.ForeColor = System.Drawing.Color.White;
            this.lbAxisPositionT.Location = new System.Drawing.Point(298, 9);
            this.lbAxisPositionT.Name = "lbAxisPositionT";
            this.lbAxisPositionT.Size = new System.Drawing.Size(92, 23);
            this.lbAxisPositionT.TabIndex = 169;
            this.lbAxisPositionT.Text = "0";
            this.lbAxisPositionT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(272, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 29);
            this.label7.TabIndex = 168;
            this.label7.Text = "T :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbAxisPositionY
            // 
            this.lbAxisPositionY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAxisPositionY.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAxisPositionY.ForeColor = System.Drawing.Color.White;
            this.lbAxisPositionY.Location = new System.Drawing.Point(166, 9);
            this.lbAxisPositionY.Name = "lbAxisPositionY";
            this.lbAxisPositionY.Size = new System.Drawing.Size(92, 23);
            this.lbAxisPositionY.TabIndex = 167;
            this.lbAxisPositionY.Text = "0";
            this.lbAxisPositionY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(139, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 29);
            this.label4.TabIndex = 166;
            this.label4.Text = "Y :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbAxisPositionX
            // 
            this.lbAxisPositionX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAxisPositionX.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAxisPositionX.ForeColor = System.Drawing.Color.White;
            this.lbAxisPositionX.Location = new System.Drawing.Point(31, 9);
            this.lbAxisPositionX.Name = "lbAxisPositionX";
            this.lbAxisPositionX.Size = new System.Drawing.Size(92, 23);
            this.lbAxisPositionX.TabIndex = 165;
            this.lbAxisPositionX.Text = "0";
            this.lbAxisPositionX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(1, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 29);
            this.label2.TabIndex = 164;
            this.label2.Text = "X :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnMsg
            // 
            this.pnMsg.Controls.Add(this.lbMsg);
            this.pnMsg.Location = new System.Drawing.Point(427, 252);
            this.pnMsg.Name = "pnMsg";
            this.pnMsg.Size = new System.Drawing.Size(400, 54);
            this.pnMsg.TabIndex = 171;
            // 
            // lbMsg
            // 
            this.lbMsg.BackColor = System.Drawing.Color.Aqua;
            this.lbMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMsg.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbMsg.Location = new System.Drawing.Point(0, 0);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(400, 54);
            this.lbMsg.TabIndex = 0;
            this.lbMsg.Text = "Message";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnDisplayWarn
            // 
            this.pnDisplayWarn.BackColor = System.Drawing.Color.Yellow;
            this.pnDisplayWarn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDisplayWarn.Controls.Add(this.lbDispWarn);
            this.pnDisplayWarn.Controls.Add(this.lbWarning);
            this.pnDisplayWarn.Location = new System.Drawing.Point(781, 676);
            this.pnDisplayWarn.Name = "pnDisplayWarn";
            this.pnDisplayWarn.Size = new System.Drawing.Size(466, 170);
            this.pnDisplayWarn.TabIndex = 1507;
            // 
            // lbDispWarn
            // 
            this.lbDispWarn.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDispWarn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDispWarn.FormattingEnabled = true;
            this.lbDispWarn.ItemHeight = 16;
            this.lbDispWarn.Location = new System.Drawing.Point(0, 26);
            this.lbDispWarn.Name = "lbDispWarn";
            this.lbDispWarn.Size = new System.Drawing.Size(464, 148);
            this.lbDispWarn.TabIndex = 1527;
            // 
            // lbWarning
            // 
            this.lbWarning.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbWarning.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWarning.ForeColor = System.Drawing.Color.Black;
            this.lbWarning.Location = new System.Drawing.Point(0, 0);
            this.lbWarning.Name = "lbWarning";
            this.lbWarning.Size = new System.Drawing.Size(464, 26);
            this.lbWarning.TabIndex = 1526;
            this.lbWarning.Text = "Warning";
            this.lbWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnImageTest
            // 
            this.pnImageTest.BackColor = System.Drawing.Color.Transparent;
            this.pnImageTest.Controls.Add(this.button4);
            this.pnImageTest.Location = new System.Drawing.Point(15, 325);
            this.pnImageTest.Name = "pnImageTest";
            this.pnImageTest.Radious = 15;
            this.pnImageTest.Size = new System.Drawing.Size(157, 92);
            this.pnImageTest.TabIndex = 608;
            this.pnImageTest.TabStop = false;
            this.pnImageTest.Text = "Image Test";
            this.pnImageTest.TitleBackColor = System.Drawing.Color.SteelBlue;
            this.pnImageTest.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnImageTest.TitleForeColor = System.Drawing.Color.White;
            this.pnImageTest.Visible = false;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(20, 41);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(114, 39);
            this.button4.TabIndex = 172;
            this.button4.Text = "TEST";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FormOper
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(1255, 879);
            this.Controls.Add(this.pnDisplayWarn);
            this.Controls.Add(this.pnMsg);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnVision);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormOper";
            this.Load += new System.EventHandler(this.FormOper_Load);
            this.VisibleChanged += new System.EventHandler(this.FormOper_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnVision.ResumeLayout(false);
            this.pnLightCon.ResumeLayout(false);
            this.pnImageSave.ResumeLayout(false);
            this.pnSkipWafer.ResumeLayout(false);
            this.pnSkipVac.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnMsg.ResumeLayout(false);
            this.pnDisplayWarn.ResumeLayout(false);
            this.pnImageTest.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView listViewData;
        private System.Windows.Forms.ColumnHeader StartTime;
        private System.Windows.Forms.ColumnHeader EndTime;
        private System.Windows.Forms.ColumnHeader Resultmm;
        private System.Windows.Forms.ColumnHeader ResultBarcode;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.Panel pnVision;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbAxisPositionT;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbAxisPositionY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbAxisPositionX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.KToggleButton btOper07;
        private System.Windows.Forms.KToggleButton btOper08;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.KToggleButton btOper09;
        private System.Windows.Forms.KToggleButton btOper01;
        private System.Windows.Forms.KToggleButton btOper02;
        private System.Windows.Forms.KToggleButton btOper06;
        private System.Windows.Forms.KToggleButton btOper05;
        private System.Windows.Forms.KToggleButton btOper04;
        private System.Windows.Forms.KToggleButton btOper03;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader ResultXmm;
        private System.Windows.Forms.ColumnHeader ResultYmm;
        private System.Windows.Forms.ColumnHeader ResultTmm;
        private System.Windows.Forms.ColumnHeader TotalTime;
        private System.Windows.Forms.Panel pnMsg;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Panel pnSkipVac;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnSkipWafer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader ResultType;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RoundPanel pnLightCon;
        private System.Windows.Forms.ColumnHeader Mode;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader No;
        private System.Windows.Forms.Panel pnImageSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnDisplayWarn;
        private System.Windows.Forms.ListBox lbDispWarn;
        private System.Windows.Forms.Label lbWarning;
        private System.Windows.Forms.RoundPanel pnImageTest;
        private System.Windows.Forms.Button button4;
    }
}