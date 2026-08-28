namespace eMachine
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.tabLogin = new System.Windows.Forms.TabControl();
            this.tpgLogin2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.edCheckNewPassWord = new System.Windows.Forms.TextBox();
            this.edNewPassWord = new System.Windows.Forms.TextBox();
            this.edOldPassWord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tpgLogin1 = new System.Windows.Forms.TabPage();
            this.btnCancel = new System.Windows.Forms.Button();
            this.BTN_PasswordChenge = new System.Windows.Forms.Button();
            this.BTN_LOGIN = new System.Windows.Forms.Button();
            this.edInPass = new System.Windows.Forms.TextBox();
            this.lblPassWord = new System.Windows.Forms.Label();
            this.btn_Master = new System.Windows.Forms.Button();
            this.btn_Maint = new System.Windows.Forms.Button();
            this.btn_Opp = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.tabLogin.SuspendLayout();
            this.tpgLogin2.SuspendLayout();
            this.tpgLogin1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabLogin
            // 
            this.tabLogin.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabLogin.Controls.Add(this.tpgLogin2);
            this.tabLogin.Controls.Add(this.tpgLogin1);
            this.tabLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabLogin.ItemSize = new System.Drawing.Size(100, 20);
            this.tabLogin.Location = new System.Drawing.Point(0, 0);
            this.tabLogin.Margin = new System.Windows.Forms.Padding(2);
            this.tabLogin.Name = "tabLogin";
            this.tabLogin.SelectedIndex = 0;
            this.tabLogin.ShowToolTips = true;
            this.tabLogin.Size = new System.Drawing.Size(457, 303);
            this.tabLogin.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabLogin.TabIndex = 419;
            // 
            // tpgLogin2
            // 
            this.tpgLogin2.Controls.Add(this.label5);
            this.tpgLogin2.Controls.Add(this.btn_Save);
            this.tpgLogin2.Controls.Add(this.btn_Exit);
            this.tpgLogin2.Controls.Add(this.edCheckNewPassWord);
            this.tpgLogin2.Controls.Add(this.edNewPassWord);
            this.tpgLogin2.Controls.Add(this.edOldPassWord);
            this.tpgLogin2.Controls.Add(this.label1);
            this.tpgLogin2.Controls.Add(this.label2);
            this.tpgLogin2.Controls.Add(this.label4);
            this.tpgLogin2.Location = new System.Drawing.Point(4, 4);
            this.tpgLogin2.Margin = new System.Windows.Forms.Padding(2);
            this.tpgLogin2.Name = "tpgLogin2";
            this.tpgLogin2.Padding = new System.Windows.Forms.Padding(2);
            this.tpgLogin2.Size = new System.Drawing.Size(449, 275);
            this.tpgLogin2.TabIndex = 1;
            this.tpgLogin2.Text = "tpgLogin1";
            this.tpgLogin2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.DimGray;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Gold;
            this.label5.Location = new System.Drawing.Point(2, 2);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(445, 29);
            this.label5.TabIndex = 436;
            this.label5.Text = "Change Password";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Save.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Save.Location = new System.Drawing.Point(180, 212);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btn_Save.Size = new System.Drawing.Size(127, 53);
            this.btn_Save.TabIndex = 434;
            this.btn_Save.Text = "Save";
            this.btn_Save.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.BTN_PasswordChenge_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Exit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Exit.Image = ((System.Drawing.Image)(resources.GetObject("btn_Exit.Image")));
            this.btn_Exit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Exit.Location = new System.Drawing.Point(310, 212);
            this.btn_Exit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btn_Exit.Size = new System.Drawing.Size(127, 53);
            this.btn_Exit.TabIndex = 435;
            this.btn_Exit.Text = "Back";
            this.btn_Exit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Exit.UseVisualStyleBackColor = false;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // edCheckNewPassWord
            // 
            this.edCheckNewPassWord.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edCheckNewPassWord.Location = new System.Drawing.Point(258, 128);
            this.edCheckNewPassWord.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.edCheckNewPassWord.Name = "edCheckNewPassWord";
            this.edCheckNewPassWord.PasswordChar = '*';
            this.edCheckNewPassWord.Size = new System.Drawing.Size(175, 36);
            this.edCheckNewPassWord.TabIndex = 433;
            this.edCheckNewPassWord.Text = "0000";
            this.edCheckNewPassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // edNewPassWord
            // 
            this.edNewPassWord.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edNewPassWord.Location = new System.Drawing.Point(258, 93);
            this.edNewPassWord.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.edNewPassWord.Name = "edNewPassWord";
            this.edNewPassWord.PasswordChar = '*';
            this.edNewPassWord.Size = new System.Drawing.Size(175, 36);
            this.edNewPassWord.TabIndex = 432;
            this.edNewPassWord.Text = "0000";
            this.edNewPassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // edOldPassWord
            // 
            this.edOldPassWord.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edOldPassWord.Location = new System.Drawing.Point(258, 58);
            this.edOldPassWord.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.edOldPassWord.Name = "edOldPassWord";
            this.edOldPassWord.PasswordChar = '*';
            this.edOldPassWord.Size = new System.Drawing.Size(175, 36);
            this.edOldPassWord.TabIndex = 431;
            this.edOldPassWord.Text = "0000";
            this.edOldPassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(20, 133);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 23);
            this.label1.TabIndex = 430;
            this.label1.Text = "NEW PASSWORD CHECK";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(20, 99);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 23);
            this.label2.TabIndex = 429;
            this.label2.Text = "NEW PASSWORD";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(20, 64);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 23);
            this.label4.TabIndex = 428;
            this.label4.Text = "OLD PASSWORD";
            // 
            // tpgLogin1
            // 
            this.tpgLogin1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tpgLogin1.Controls.Add(this.btnCancel);
            this.tpgLogin1.Controls.Add(this.BTN_PasswordChenge);
            this.tpgLogin1.Controls.Add(this.BTN_LOGIN);
            this.tpgLogin1.Controls.Add(this.edInPass);
            this.tpgLogin1.Controls.Add(this.lblPassWord);
            this.tpgLogin1.Controls.Add(this.btn_Master);
            this.tpgLogin1.Controls.Add(this.btn_Maint);
            this.tpgLogin1.Controls.Add(this.btn_Opp);
            this.tpgLogin1.Controls.Add(this.lbTitle);
            this.tpgLogin1.Cursor = System.Windows.Forms.Cursors.No;
            this.tpgLogin1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tpgLogin1.Location = new System.Drawing.Point(4, 4);
            this.tpgLogin1.Margin = new System.Windows.Forms.Padding(2);
            this.tpgLogin1.Name = "tpgLogin1";
            this.tpgLogin1.Padding = new System.Windows.Forms.Padding(2);
            this.tpgLogin1.Size = new System.Drawing.Size(449, 275);
            this.tpgLogin1.TabIndex = 0;
            this.tpgLogin1.Text = "tpgLogin1";
            this.tpgLogin1.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(297, 198);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnCancel.Size = new System.Drawing.Size(134, 67);
            this.btnCancel.TabIndex = 429;
            this.btnCancel.Text = "CALCEL";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // BTN_PasswordChenge
            // 
            this.BTN_PasswordChenge.BackColor = System.Drawing.Color.White;
            this.BTN_PasswordChenge.Cursor = System.Windows.Forms.Cursors.Default;
            this.BTN_PasswordChenge.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_PasswordChenge.ForeColor = System.Drawing.Color.Black;
            this.BTN_PasswordChenge.Image = ((System.Drawing.Image)(resources.GetObject("BTN_PasswordChenge.Image")));
            this.BTN_PasswordChenge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_PasswordChenge.Location = new System.Drawing.Point(20, 198);
            this.BTN_PasswordChenge.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BTN_PasswordChenge.Name = "BTN_PasswordChenge";
            this.BTN_PasswordChenge.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BTN_PasswordChenge.Size = new System.Drawing.Size(134, 67);
            this.BTN_PasswordChenge.TabIndex = 428;
            this.BTN_PasswordChenge.Text = "PASSWORD\r\nCHANGE";
            this.BTN_PasswordChenge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BTN_PasswordChenge.UseVisualStyleBackColor = false;
            this.BTN_PasswordChenge.Click += new System.EventHandler(this.BTN_PasswordChenge_Click_1);
            // 
            // BTN_LOGIN
            // 
            this.BTN_LOGIN.BackColor = System.Drawing.Color.White;
            this.BTN_LOGIN.Cursor = System.Windows.Forms.Cursors.Default;
            this.BTN_LOGIN.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_LOGIN.ForeColor = System.Drawing.Color.Black;
            this.BTN_LOGIN.Image = ((System.Drawing.Image)(resources.GetObject("BTN_LOGIN.Image")));
            this.BTN_LOGIN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_LOGIN.Location = new System.Drawing.Point(157, 198);
            this.BTN_LOGIN.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BTN_LOGIN.Name = "BTN_LOGIN";
            this.BTN_LOGIN.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BTN_LOGIN.Size = new System.Drawing.Size(134, 67);
            this.BTN_LOGIN.TabIndex = 427;
            this.BTN_LOGIN.Text = "LOGIN";
            this.BTN_LOGIN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BTN_LOGIN.UseVisualStyleBackColor = false;
            this.BTN_LOGIN.Click += new System.EventHandler(this.BTN_LOGIN_Click_1);
            // 
            // edInPass
            // 
            this.edInPass.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edInPass.Location = new System.Drawing.Point(157, 152);
            this.edInPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.edInPass.Name = "edInPass";
            this.edInPass.PasswordChar = '*';
            this.edInPass.Size = new System.Drawing.Size(275, 33);
            this.edInPass.TabIndex = 426;
            this.edInPass.Text = "0000";
            this.edInPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.edInPass.TextChanged += new System.EventHandler(this.edInPass_TextChanged);
            this.edInPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edInPass_KeyDown);
            // 
            // lblPassWord
            // 
            this.lblPassWord.AutoSize = true;
            this.lblPassWord.Font = new System.Drawing.Font("맑은 고딕", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblPassWord.ForeColor = System.Drawing.Color.Black;
            this.lblPassWord.Location = new System.Drawing.Point(28, 157);
            this.lblPassWord.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassWord.Name = "lblPassWord";
            this.lblPassWord.Size = new System.Drawing.Size(119, 25);
            this.lblPassWord.TabIndex = 425;
            this.lblPassWord.Text = "PASSWORD";
            // 
            // btn_Master
            // 
            this.btn_Master.BackColor = System.Drawing.Color.White;
            this.btn_Master.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Master.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Master.Image = ((System.Drawing.Image)(resources.GetObject("btn_Master.Image")));
            this.btn_Master.Location = new System.Drawing.Point(344, 41);
            this.btn_Master.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Master.Name = "btn_Master";
            this.btn_Master.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.btn_Master.Size = new System.Drawing.Size(87, 93);
            this.btn_Master.TabIndex = 424;
            this.btn_Master.Tag = "2";
            this.btn_Master.Text = "MASTER";
            this.btn_Master.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Master.UseVisualStyleBackColor = false;
            this.btn_Master.Click += new System.EventHandler(this.btn_Opp_Click);
            // 
            // btn_Maint
            // 
            this.btn_Maint.BackColor = System.Drawing.Color.White;
            this.btn_Maint.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Maint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Maint.Image = ((System.Drawing.Image)(resources.GetObject("btn_Maint.Image")));
            this.btn_Maint.Location = new System.Drawing.Point(251, 41);
            this.btn_Maint.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Maint.Name = "btn_Maint";
            this.btn_Maint.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.btn_Maint.Size = new System.Drawing.Size(87, 93);
            this.btn_Maint.TabIndex = 423;
            this.btn_Maint.Tag = "1";
            this.btn_Maint.Text = "ENGINEER";
            this.btn_Maint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Maint.UseVisualStyleBackColor = false;
            this.btn_Maint.Click += new System.EventHandler(this.btn_Opp_Click);
            // 
            // btn_Opp
            // 
            this.btn_Opp.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btn_Opp.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Opp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Opp.Image = ((System.Drawing.Image)(resources.GetObject("btn_Opp.Image")));
            this.btn_Opp.Location = new System.Drawing.Point(157, 41);
            this.btn_Opp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Opp.Name = "btn_Opp";
            this.btn_Opp.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.btn_Opp.Size = new System.Drawing.Size(87, 93);
            this.btn_Opp.TabIndex = 422;
            this.btn_Opp.Tag = "0";
            this.btn_Opp.Text = "OPERATOR";
            this.btn_Opp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Opp.UseVisualStyleBackColor = false;
            this.btn_Opp.Click += new System.EventHandler(this.btn_Opp_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.DimGray;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Gold;
            this.lbTitle.Location = new System.Drawing.Point(2, 2);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(443, 29);
            this.lbTitle.TabIndex = 421;
            this.lbTitle.Text = "LOGIN LEVEL";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseDown);
            this.lbTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbTitle_MouseMove);
            // 
            // FrmLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(457, 313);
            this.Controls.Add(this.tabLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormLogin";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLogin_FormClosed);
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.tabLogin.ResumeLayout(false);
            this.tpgLogin2.ResumeLayout(false);
            this.tpgLogin2.PerformLayout();
            this.tpgLogin1.ResumeLayout(false);
            this.tpgLogin1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabLogin;
        private System.Windows.Forms.TabPage tpgLogin1;
        internal System.Windows.Forms.Button BTN_PasswordChenge;
        internal System.Windows.Forms.Button BTN_LOGIN;
        internal System.Windows.Forms.TextBox edInPass;
        private System.Windows.Forms.Label lblPassWord;
        private System.Windows.Forms.Button btn_Master;
        private System.Windows.Forms.Button btn_Maint;
        private System.Windows.Forms.Button btn_Opp;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.TabPage tpgLogin2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.TextBox edCheckNewPassWord;
        private System.Windows.Forms.TextBox edNewPassWord;
        private System.Windows.Forms.TextBox edOldPassWord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Button btnCancel;

    }
}