namespace Calendar
{
    partial class cFrmCalendar
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
            this.lbYear = new System.Windows.Forms.Label();
            this.lbUp = new System.Windows.Forms.Label();
            this.lbDown = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbYear
            // 
            this.lbYear.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbYear.ForeColor = System.Drawing.Color.White;
            this.lbYear.Location = new System.Drawing.Point(2, 7);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(138, 30);
            this.lbYear.TabIndex = 0;
            this.lbYear.Text = "0000년 00월";
            this.lbYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbYear.Click += new System.EventHandler(this.lbYearClickEvent);
            this.lbYear.MouseEnter += new System.EventHandler(this.MouseEnterEvent);
            this.lbYear.MouseLeave += new System.EventHandler(this.MouseLeaveEvent);
            // 
            // lbUp
            // 
            this.lbUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUp.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbUp.ForeColor = System.Drawing.Color.White;
            this.lbUp.Location = new System.Drawing.Point(560, 7);
            this.lbUp.Name = "lbUp";
            this.lbUp.Size = new System.Drawing.Size(46, 30);
            this.lbUp.TabIndex = 1;
            this.lbUp.Text = "△";
            this.lbUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbUp.Click += new System.EventHandler(this.lbUpClickEvent);
            this.lbUp.MouseEnter += new System.EventHandler(this.MouseEnterEvent);
            this.lbUp.MouseLeave += new System.EventHandler(this.MouseLeaveEvent);
            // 
            // lbDown
            // 
            this.lbDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDown.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbDown.ForeColor = System.Drawing.Color.White;
            this.lbDown.Location = new System.Drawing.Point(606, 7);
            this.lbDown.Name = "lbDown";
            this.lbDown.Size = new System.Drawing.Size(46, 30);
            this.lbDown.TabIndex = 1;
            this.lbDown.Text = "▽";
            this.lbDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lbDown.Click += new System.EventHandler(this.lbDownClickEvent);
            this.lbDown.MouseEnter += new System.EventHandler(this.MouseEnterEvent);
            this.lbDown.MouseLeave += new System.EventHandler(this.MouseLeaveEvent);
            // 
            // cFrmCalendar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(668, 304);
            this.ControlBox = false;
            this.Controls.Add(this.lbDown);
            this.Controls.Add(this.lbUp);
            this.Controls.Add(this.lbYear);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "cFrmCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintEvent);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownEvent);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveEvent);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbYear;
        private System.Windows.Forms.Label lbUp;
        private System.Windows.Forms.Label lbDown;
    }
}