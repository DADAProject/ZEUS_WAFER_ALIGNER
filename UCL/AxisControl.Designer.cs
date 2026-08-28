
namespace System.Windows.Forms
{
    partial class AxisControl
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Motor = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Motor)).BeginInit();
            this.SuspendLayout();
            // 
            // Motor
            // 
            this.Motor.BackColor = System.Drawing.Color.DodgerBlue;
            this.Motor.Location = new System.Drawing.Point(114, 4);
            this.Motor.Name = "Motor";
            this.Motor.Size = new System.Drawing.Size(59, 41);
            this.Motor.TabIndex = 0;
            this.Motor.TabStop = false;
            // 
            // AxisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.Controls.Add(this.Motor);
            this.Name = "AxisControl";
            this.Size = new System.Drawing.Size(296, 48);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AxisControl_Paint);
            this.Resize += new System.EventHandler(this.AxisControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Motor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Motor;
    }
}
