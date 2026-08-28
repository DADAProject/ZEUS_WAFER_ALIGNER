namespace Drv.CameraController
{
    partial class frmControllerSetting
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("1: Axis1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Ajin", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.trList = new System.Windows.Forms.TreeView();
            this.pnControllerData = new System.Windows.Forms.Panel();
            this.txtControllerName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbControllerType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pnAxisData = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.numAxisIndex = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAxisName = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnControllerInitData = new System.Windows.Forms.Panel();
            this.btnAddCtr = new System.Windows.Forms.Button();
            this.btnAddAxis = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnControllerData.SuspendLayout();
            this.pnAxisData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxisIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // trList
            // 
            this.trList.HideSelection = false;
            this.trList.Location = new System.Drawing.Point(12, 12);
            this.trList.Name = "trList";
            treeNode1.Name = "노드1";
            treeNode1.Text = "1: Axis1";
            treeNode2.Name = "노드0";
            treeNode2.Text = "Ajin";
            this.trList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.trList.Size = new System.Drawing.Size(273, 530);
            this.trList.TabIndex = 0;
            this.trList.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trList_BeforeSelect);
            this.trList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trList_AfterSelect);
            // 
            // pnControllerData
            // 
            this.pnControllerData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnControllerData.Controls.Add(this.txtControllerName);
            this.pnControllerData.Controls.Add(this.label11);
            this.pnControllerData.Controls.Add(this.cmbControllerType);
            this.pnControllerData.Controls.Add(this.label10);
            this.pnControllerData.Location = new System.Drawing.Point(291, 12);
            this.pnControllerData.Name = "pnControllerData";
            this.pnControllerData.Size = new System.Drawing.Size(604, 75);
            this.pnControllerData.TabIndex = 1;
            // 
            // txtControllerName
            // 
            this.txtControllerName.Location = new System.Drawing.Point(366, 40);
            this.txtControllerName.Name = "txtControllerName";
            this.txtControllerName.Size = new System.Drawing.Size(223, 21);
            this.txtControllerName.TabIndex = 16;
            this.txtControllerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Silver;
            this.label11.Location = new System.Drawing.Point(14, 40);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label11.Size = new System.Drawing.Size(347, 21);
            this.label11.TabIndex = 15;
            this.label11.Text = "Controller Name";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbControllerType
            // 
            this.cmbControllerType.FormattingEnabled = true;
            this.cmbControllerType.Location = new System.Drawing.Point(366, 14);
            this.cmbControllerType.Name = "cmbControllerType";
            this.cmbControllerType.Size = new System.Drawing.Size(223, 20);
            this.cmbControllerType.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Silver;
            this.label10.Location = new System.Drawing.Point(13, 13);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label10.Size = new System.Drawing.Size(347, 21);
            this.label10.TabIndex = 13;
            this.label10.Text = "Controller Type";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnAxisData
            // 
            this.pnAxisData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnAxisData.Controls.Add(this.label7);
            this.pnAxisData.Controls.Add(this.numAxisIndex);
            this.pnAxisData.Controls.Add(this.label6);
            this.pnAxisData.Controls.Add(this.txtAxisName);
            this.pnAxisData.Location = new System.Drawing.Point(291, 226);
            this.pnAxisData.Name = "pnAxisData";
            this.pnAxisData.Size = new System.Drawing.Size(604, 316);
            this.pnAxisData.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Silver;
            this.label7.Location = new System.Drawing.Point(13, 46);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(347, 21);
            this.label7.TabIndex = 13;
            this.label7.Text = "CameraIndex";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numAxisIndex
            // 
            this.numAxisIndex.Location = new System.Drawing.Point(366, 46);
            this.numAxisIndex.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numAxisIndex.Name = "numAxisIndex";
            this.numAxisIndex.Size = new System.Drawing.Size(223, 21);
            this.numAxisIndex.TabIndex = 12;
            this.numAxisIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Silver;
            this.label6.Location = new System.Drawing.Point(13, 17);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(347, 21);
            this.label6.TabIndex = 11;
            this.label6.Text = "Camera Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAxisName
            // 
            this.txtAxisName.Location = new System.Drawing.Point(366, 17);
            this.txtAxisName.Name = "txtAxisName";
            this.txtAxisName.Size = new System.Drawing.Size(223, 21);
            this.txtAxisName.TabIndex = 10;
            this.txtAxisName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(658, 548);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(739, 548);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(820, 548);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnControllerInitData
            // 
            this.pnControllerInitData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnControllerInitData.Location = new System.Drawing.Point(291, 94);
            this.pnControllerInitData.Name = "pnControllerInitData";
            this.pnControllerInitData.Size = new System.Drawing.Size(604, 124);
            this.pnControllerInitData.TabIndex = 6;
            // 
            // btnAddCtr
            // 
            this.btnAddCtr.Location = new System.Drawing.Point(12, 548);
            this.btnAddCtr.Name = "btnAddCtr";
            this.btnAddCtr.Size = new System.Drawing.Size(75, 23);
            this.btnAddCtr.TabIndex = 7;
            this.btnAddCtr.Text = "Add Ctr";
            this.btnAddCtr.UseVisualStyleBackColor = true;
            this.btnAddCtr.Click += new System.EventHandler(this.btnAddCtr_Click);
            // 
            // btnAddAxis
            // 
            this.btnAddAxis.Location = new System.Drawing.Point(93, 548);
            this.btnAddAxis.Name = "btnAddAxis";
            this.btnAddAxis.Size = new System.Drawing.Size(75, 23);
            this.btnAddAxis.TabIndex = 8;
            this.btnAddAxis.Text = "Add Camera";
            this.btnAddAxis.UseVisualStyleBackColor = true;
            this.btnAddAxis.Click += new System.EventHandler(this.btnAddAxis_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(210, 548);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmControllerSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 586);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAddAxis);
            this.Controls.Add(this.btnAddCtr);
            this.Controls.Add(this.pnControllerInitData);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pnAxisData);
            this.Controls.Add(this.pnControllerData);
            this.Controls.Add(this.trList);
            this.Name = "frmControllerSetting";
            this.Load += new System.EventHandler(this.frmControllerSetting_Load);
            this.pnControllerData.ResumeLayout(false);
            this.pnControllerData.PerformLayout();
            this.pnAxisData.ResumeLayout(false);
            this.pnAxisData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxisIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trList;
        private System.Windows.Forms.Panel pnControllerData;
        private System.Windows.Forms.Panel pnAxisData;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numAxisIndex;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAxisName;
        private System.Windows.Forms.Panel pnControllerInitData;
        private System.Windows.Forms.TextBox txtControllerName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbControllerType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnAddCtr;
        private System.Windows.Forms.Button btnAddAxis;
        private System.Windows.Forms.Button btnDelete;
    }
}