namespace eMachine
{
    partial class FormBarCode
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
            this.connect = new System.Windows.Forms.Button();
            this.disconnect = new System.Windows.Forms.Button();
            this.lon = new System.Windows.Forms.Button();
            this.loff = new System.Windows.Forms.Button();
            this.receive = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.CommandPortInput = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.DataPortInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.connect.ForeColor = System.Drawing.Color.Black;
            this.connect.Location = new System.Drawing.Point(14, 12);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(303, 42);
            this.connect.TabIndex = 0;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = false;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // disconnect
            // 
            this.disconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.disconnect.ForeColor = System.Drawing.Color.Black;
            this.disconnect.Location = new System.Drawing.Point(324, 12);
            this.disconnect.Name = "disconnect";
            this.disconnect.Size = new System.Drawing.Size(303, 42);
            this.disconnect.TabIndex = 1;
            this.disconnect.Text = "Disconnect";
            this.disconnect.UseVisualStyleBackColor = false;
            this.disconnect.Click += new System.EventHandler(this.disconnect_Click);
            // 
            // lon
            // 
            this.lon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lon.ForeColor = System.Drawing.Color.Black;
            this.lon.Location = new System.Drawing.Point(14, 60);
            this.lon.Name = "lon";
            this.lon.Size = new System.Drawing.Size(303, 42);
            this.lon.TabIndex = 2;
            this.lon.Text = "Timing ON";
            this.lon.UseVisualStyleBackColor = false;
            this.lon.Click += new System.EventHandler(this.lon_Click);
            // 
            // loff
            // 
            this.loff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.loff.ForeColor = System.Drawing.Color.Black;
            this.loff.Location = new System.Drawing.Point(324, 60);
            this.loff.Name = "loff";
            this.loff.Size = new System.Drawing.Size(303, 42);
            this.loff.TabIndex = 3;
            this.loff.Text = "Timing OFF";
            this.loff.UseVisualStyleBackColor = false;
            this.loff.Click += new System.EventHandler(this.loff_Click);
            // 
            // receive
            // 
            this.receive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.receive.ForeColor = System.Drawing.Color.Black;
            this.receive.Location = new System.Drawing.Point(14, 108);
            this.receive.Name = "receive";
            this.receive.Size = new System.Drawing.Size(303, 42);
            this.receive.TabIndex = 4;
            this.receive.Text = "Receive Data";
            this.receive.UseVisualStyleBackColor = false;
            this.receive.Click += new System.EventHandler(this.receive_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(323, 137);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(303, 21);
            this.textBox1.TabIndex = 5;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(329, 113);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(90, 12);
            this.Label1.TabIndex = 21;
            this.Label1.Text = "Command Port";
            // 
            // CommandPortInput
            // 
            this.CommandPortInput.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.CommandPortInput.Location = new System.Drawing.Point(427, 105);
            this.CommandPortInput.MaxLength = 5;
            this.CommandPortInput.Name = "CommandPortInput";
            this.CommandPortInput.Size = new System.Drawing.Size(59, 21);
            this.CommandPortInput.TabIndex = 22;
            this.CommandPortInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PortInput_KeyPress);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(495, 109);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(56, 12);
            this.Label2.TabIndex = 23;
            this.Label2.Text = "Data Port";
            // 
            // DataPortInput
            // 
            this.DataPortInput.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.DataPortInput.Location = new System.Drawing.Point(562, 105);
            this.DataPortInput.MaxLength = 5;
            this.DataPortInput.Name = "DataPortInput";
            this.DataPortInput.Size = new System.Drawing.Size(59, 21);
            this.DataPortInput.TabIndex = 24;
            this.DataPortInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PortInput_KeyPress);
            // 
            // FormBarCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 171);
            this.Controls.Add(this.DataPortInput);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.CommandPortInput);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.receive);
            this.Controls.Add(this.loff);
            this.Controls.Add(this.lon);
            this.Controls.Add(this.disconnect);
            this.Controls.Add(this.connect);
            this.Name = "FormBarCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button disconnect;
        private System.Windows.Forms.Button lon;
        private System.Windows.Forms.Button loff;
        private System.Windows.Forms.Button receive;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox CommandPortInput;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox DataPortInput;
    }
}

