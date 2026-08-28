
namespace eMachine
{
    partial class FrmCamDx
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
            bThreadAbort = true;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCamDx));
            this.Toolbar = new System.Windows.Forms.ToolStrip();
            this.btnName = new System.Windows.Forms.ToolStripButton();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.btnPan = new System.Windows.Forms.ToolStripButton();
            this.btnZommPlus = new System.Windows.Forms.ToolStripButton();
            this.btnZoomMinus = new System.Windows.Forms.ToolStripButton();
            this.btnZoomAll = new System.Windows.Forms.ToolStripButton();
            this.btnCrossLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLightOne = new System.Windows.Forms.ToolStripButton();
            this.btnGrabOne = new System.Windows.Forms.ToolStripButton();
            this.btnLive = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnJog = new System.Windows.Forms.ToolStripButton();
            this.pbxPattern = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // Toolbar
            // 
            this.Toolbar.AutoSize = false;
            this.Toolbar.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Toolbar.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Toolbar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnName,
            this.btnSelect,
            this.btnPan,
            this.btnZommPlus,
            this.btnZoomMinus,
            this.btnZoomAll,
            this.btnCrossLine,
            this.toolStripSeparator4,
            this.btnLightOne,
            this.btnGrabOne,
            this.btnLive,
            this.btnOpen,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnJog});
            this.Toolbar.Location = new System.Drawing.Point(1, 1);
            this.Toolbar.Name = "Toolbar";
            this.Toolbar.Padding = new System.Windows.Forms.Padding(0);
            this.Toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Toolbar.Size = new System.Drawing.Size(459, 31);
            this.Toolbar.Stretch = true;
            this.Toolbar.TabIndex = 1245;
            // 
            // btnName
            // 
            this.btnName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnName.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnName.Name = "btnName";
            this.btnName.Size = new System.Drawing.Size(23, 28);
            this.btnName.Click += new System.EventHandler(this.btnCam_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnSelect.Image")));
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.White;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(28, 28);
            this.btnSelect.Tag = "";
            this.btnSelect.Text = "Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnPan
            // 
            this.btnPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPan.Image = ((System.Drawing.Image)(resources.GetObject("btnPan.Image")));
            this.btnPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(28, 28);
            this.btnPan.Tag = "";
            this.btnPan.Text = "Pan";
            this.btnPan.Click += new System.EventHandler(this.btnPan_Click);
            // 
            // btnZommPlus
            // 
            this.btnZommPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZommPlus.Image = ((System.Drawing.Image)(resources.GetObject("btnZommPlus.Image")));
            this.btnZommPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZommPlus.Name = "btnZommPlus";
            this.btnZommPlus.Size = new System.Drawing.Size(28, 28);
            this.btnZommPlus.Tag = "";
            this.btnZommPlus.Text = "Zoom Plus";
            this.btnZommPlus.Click += new System.EventHandler(this.btnZommPlus_Click);
            // 
            // btnZoomMinus
            // 
            this.btnZoomMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomMinus.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomMinus.Image")));
            this.btnZoomMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomMinus.Name = "btnZoomMinus";
            this.btnZoomMinus.Size = new System.Drawing.Size(28, 28);
            this.btnZoomMinus.Tag = "";
            this.btnZoomMinus.Text = "Zoom Minus";
            this.btnZoomMinus.ToolTipText = "ZoomMinus";
            this.btnZoomMinus.Click += new System.EventHandler(this.btnZoomMinus_Click);
            // 
            // btnZoomAll
            // 
            this.btnZoomAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomAll.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomAll.Image")));
            this.btnZoomAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomAll.Name = "btnZoomAll";
            this.btnZoomAll.Size = new System.Drawing.Size(28, 28);
            this.btnZoomAll.Tag = "";
            this.btnZoomAll.Text = "Zoom Fit";
            this.btnZoomAll.ToolTipText = "Zoom Fit";
            this.btnZoomAll.Click += new System.EventHandler(this.btnZoomAll_Click);
            // 
            // btnCrossLine
            // 
            this.btnCrossLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCrossLine.Image = ((System.Drawing.Image)(resources.GetObject("btnCrossLine.Image")));
            this.btnCrossLine.ImageTransparentColor = System.Drawing.Color.White;
            this.btnCrossLine.Name = "btnCrossLine";
            this.btnCrossLine.Size = new System.Drawing.Size(28, 28);
            this.btnCrossLine.Tag = "";
            this.btnCrossLine.Text = "Cross Line";
            this.btnCrossLine.ToolTipText = "Cross Line";
            this.btnCrossLine.Click += new System.EventHandler(this.btnCrossLine_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // btnLightOne
            // 
            this.btnLightOne.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLightOne.Image = ((System.Drawing.Image)(resources.GetObject("btnLightOne.Image")));
            this.btnLightOne.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLightOne.Name = "btnLightOne";
            this.btnLightOne.Size = new System.Drawing.Size(28, 28);
            this.btnLightOne.Tag = "Light On & Off";
            this.btnLightOne.Text = "Snap Shot";
            this.btnLightOne.ToolTipText = "Light";
            this.btnLightOne.Click += new System.EventHandler(this.btnLightOne_Click);
            // 
            // btnGrabOne
            // 
            this.btnGrabOne.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGrabOne.Image = ((System.Drawing.Image)(resources.GetObject("btnGrabOne.Image")));
            this.btnGrabOne.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGrabOne.Name = "btnGrabOne";
            this.btnGrabOne.Size = new System.Drawing.Size(28, 28);
            this.btnGrabOne.Tag = "Snap Shot";
            this.btnGrabOne.Text = "Snap Shot";
            this.btnGrabOne.Click += new System.EventHandler(this.btnGrabOne_Click);
            // 
            // btnLive
            // 
            this.btnLive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLive.Image = ((System.Drawing.Image)(resources.GetObject("btnLive.Image")));
            this.btnLive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(28, 28);
            this.btnLive.Tag = "";
            this.btnLive.Text = "Cam Live";
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(28, 28);
            this.btnOpen.Tag = "";
            this.btnOpen.Text = "Open Image";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(28, 28);
            this.btnSave.Tag = "";
            this.btnSave.Text = "Save Image";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // btnJog
            // 
            this.btnJog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnJog.Image = ((System.Drawing.Image)(resources.GetObject("btnJog.Image")));
            this.btnJog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnJog.Name = "btnJog";
            this.btnJog.Size = new System.Drawing.Size(28, 28);
            this.btnJog.Text = "Motion Joystick";
            this.btnJog.Click += new System.EventHandler(this.btnJog_Click);
            // 
            // pbxPattern
            // 
            this.pbxPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxPattern.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxPattern.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxPattern.Location = new System.Drawing.Point(379, 321);
            this.pbxPattern.Name = "pbxPattern";
            this.pbxPattern.Size = new System.Drawing.Size(80, 80);
            this.pbxPattern.TabIndex = 1247;
            this.pbxPattern.TabStop = false;
            this.pbxPattern.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmCamDx
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.pbxPattern);
            this.Controls.Add(this.Toolbar);
            this.Font = new System.Drawing.Font("굴림", 8.25F);
            this.Name = "FrmCamDx";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(461, 403);
            this.Load += new System.EventHandler(this.FrmCam_Load);
            this.SizeChanged += new System.EventHandler(this.FrmCam_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.FrmCam_VisibleChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmCam_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmCam_DragEnter);
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPattern)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.ToolStrip Toolbar;
        private System.Windows.Forms.ToolStripButton btnPan;
        private System.Windows.Forms.ToolStripButton btnZommPlus;
        private System.Windows.Forms.ToolStripButton btnZoomMinus;
        private System.Windows.Forms.ToolStripButton btnZoomAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnGrabOne;
        private System.Windows.Forms.ToolStripButton btnLive;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.PictureBox pbxPattern;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripButton btnName;
        private System.Windows.Forms.ToolStripButton btnCrossLine;
        private System.Windows.Forms.ToolStripButton btnSelect;
        private System.Windows.Forms.ToolStripButton btnLightOne;
        private System.Windows.Forms.ToolStripButton btnJog;
    }

}