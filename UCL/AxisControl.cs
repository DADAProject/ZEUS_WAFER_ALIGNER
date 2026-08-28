using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public enum EN_AXIS_TYPE : int
    {
        X = 0,
        Y = 1,
        Z = 2,
    }

    public partial class AxisControl : UserControl
    {
        private EN_AXIS_TYPE Type;         //나중에 X, Y, Z 모양 바디 생성 기본모양 변경
                                           //축사이즈 변경
        private bool m_Inverse = false;

        //Draw Object
        private double m_dDrawPos = new double();
        private Image image;
        //                

        /// <summary>
        /// Type of Axis
        /// </summary>
        [System.ComponentModel.Browsable(true),
        System.ComponentModel.Category("Axis Invers"),
        System.ComponentModel.Description("Axis Invers.")]
        [DefaultValue(false)]
        public bool AxisInvers
        {
            get { return m_Inverse; }
            set
            {
                m_Inverse = value;
                Invalidate();
            }
        }

        //이미지 브라우져
        [Category("Image")]
        [Description("Motor Image")]
        [Localizable(true)]
        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                Invalidate();
            }
        }


        [System.ComponentModel.Browsable(false)]
        public bool IsMoving { get; set; }

        [System.ComponentModel.Browsable(false)]
        private double DrawPosition
        {
            set
            {
                if (m_dDrawPos == value)
                {
                    IsMoving = false;
                }
                else
                {
                    IsMoving = true;
                    m_dDrawPos = value;
                }
            }
        }


        //=============================================
        private double _MinPos = -1;
        private double _MaxPos = 100;
        private double _EncPos = 0;

        [Category("MinPos")]
        [Description("Axis MinPos")]
        public double MinPos { get { return _MinPos; } set { _MinPos = value; CalcAxis(); } }
        [Category("MaxPos")]
        [Description("Axis MaxPos")]
        public double MaxPos { get { return _MaxPos; } set { _MaxPos = value; CalcAxis(); } }

        [Category("MaxPos")]
        [Description("Axis MaxPos")]
        public double EncPos { get { return _EncPos; } set { _EncPos = value; CalcAxis(); } }

        //==============================================
        private bool _IsCW = false;
        private bool _IsCCW = false;
        private bool _IsHome = false;
        private bool IsCW { get { return _IsCW; } set { _IsCW = value; CalcAxis(); } }
        private bool IsCCW { get { return _IsCCW; } set { _IsCCW = value; CalcAxis(); } }
        private bool IsHome { get { return _IsHome; } set { _IsHome = value; CalcAxis(); } }

        private bool _VisibleCW = true;
        private bool _VisibleCCW = true;
        private bool _VisibleHome = true;

        public bool VisibleCW { get { return _VisibleCW; } set { _VisibleCW = value; CalcAxis(); } }
        public bool VisibleCCW { get { return _VisibleCCW; } set { _VisibleCCW = value; CalcAxis(); } }
        public bool VisibleHome { get { return _VisibleHome; } set { _VisibleHome = value; CalcAxis(); } }


        private ContentAlignment _AlignmentCW = ContentAlignment.TopLeft;
        private ContentAlignment _AlignmentCCW = ContentAlignment.TopRight;
        private ContentAlignment _AlignmentHome = ContentAlignment.TopCenter;

        public ContentAlignment AlignmentCW { get { return _AlignmentCW; } set { _AlignmentCW = value; CalcAxis(); } }
        public ContentAlignment AlignmentCCW { get { return _AlignmentCCW; } set { _AlignmentCCW = value; CalcAxis(); } }
        public ContentAlignment AlignmentHome { get { return _AlignmentHome; } set { _AlignmentHome = value; CalcAxis(); } }


        [Category("OnColor")]
        [Description("Sensor On Color")]
        public Color OnColor { get; set; } = Color.LimeGreen;
        [Category("OffColor")]
        [Description("Sensor Off Color")]
        public Color OffColor { get; set; } = Color.Red;

        //========================================================

        public AxisControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
            ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor | ControlStyles.StandardDoubleClick, true);
            Type = EN_AXIS_TYPE.X;
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.AutoSize = false;
            this.AutoScaleMode = AutoScaleMode.None;

            Motor.Visible = false;
            this.ForeColor = SystemColors.ActiveCaption;
            AxisControl_Resize(null, null);
        }


        private void AxisControl_Resize(object sender, EventArgs e)
        {
            this.Margin = new Padding(0, 0, 0, 0);
            double ParentWidth = this.Width;
            double ParentHeight = this.Height;

            int Width = 0;
            int Height = 0;

            int cx = 0;
            int cy = 0;

            if (Type.Equals(EN_AXIS_TYPE.X))
            {
                Width = Convert.ToInt32(ParentHeight * 1.0);
                Height = Convert.ToInt32(ParentHeight * 1.0);

                if (Width <= 0) Width = this.Width / 2;
                if (Height <= 0) Height = this.Height / 2;

                Motor.Size = new Size(Height, Height);
                cx = Convert.ToInt32(this.Width / 2);
                cy = Convert.ToInt32(this.Height / 2);

                Motor.Location = new Point(cx - (Motor.Size.Width / 2), 0);
            }
            else if (Type.Equals(EN_AXIS_TYPE.Y))
            {
                Width = Convert.ToInt32(ParentWidth * 1.0);
                Height = Convert.ToInt32(ParentWidth * 1.0);

                if (Width <= 0) Width = this.Width / 2;
                if (Height <= 0) Height = this.Height / 2;

                Motor.Size = new Size(Width, Width);
                cx = Convert.ToInt32(this.Width / 2);
                cy = Convert.ToInt32(this.Height / 2);

                if (!AxisInvers)
                    Motor.Location = new Point(cx - (Motor.Size.Width / 2), 0);
                else
                    Motor.Location = new Point(0, this.Height - cy - (Motor.Size.Height / 2));
            }

            Invalidate();
        }

        private void CalcAxis()
        {
            double MinPos = _MinPos;
            double MaxPos = _MaxPos;
            double Distance = MaxPos + Math.Abs(MinPos);

            double Width = 0.0;
            double Height = 0.0;

            double Scale = 0.0;
            double EncPos = 0.0;
            double DrawPos = 0.0;

            if (Type.Equals(EN_AXIS_TYPE.X))
            {
                Width = this.Width - Motor.Size.Height;
                Height = this.Height;

                Scale = Distance / Width;
                EncPos = _EncPos;
                if (MinPos < 0) EncPos += Math.Abs(MinPos);

                DrawPos = EncPos / Scale;
                Motor.Location = new Point((this.Height / 2) + (int)DrawPos, 0);
            }
            else if (Type.Equals(EN_AXIS_TYPE.Y))
            {
                Width = this.Width;
                Height = this.Height - Motor.Size.Width;

                if (Width <= 0) Width = this.Width / 2;
                if (Height <= 0) Height = this.Height / 2;

                Scale = Distance / Height;
                EncPos = _EncPos;
                if (MinPos < 0) EncPos += Math.Abs(MinPos);
                DrawPos = EncPos / Scale;

                if (!AxisInvers)
                    Motor.Location = new Point(0, (int)DrawPos);
                else
                    Motor.Location = new Point(0, (int)this.Height - this.Width - (int)DrawPos);

            }

            DrawPosition = DrawPos;
            Invalidate();
        }

        private void DrawAxis(PaintEventArgs e)
        {
            Graphics DC = e.Graphics;
            Motor.Visible = false;

            if (image is null)
            {
                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    DC.FillRectangle(brush, new Rectangle(Motor.Location, Motor.Size));
                }
            }
            else
            {
                DC.DrawImage(image, new Rectangle(Motor.Location, Motor.Size));
            }

        }

        private void DrawSensor(PaintEventArgs e)
        {
            Graphics DC = e.Graphics;

            if (VisibleCW)
            {
                Color color = IsCW ? OnColor : OffColor;

                using (SolidBrush brush = new SolidBrush(color))
                {
                    DC.FillEllipse(brush, Pos2Alignment(AlignmentCW, new Size(10, 10)));
                }
            }

            if (VisibleCCW)
            {
                Color color = IsCCW ? OnColor : OffColor;

                using (SolidBrush brush = new SolidBrush(color))
                {
                    DC.FillEllipse(brush, Pos2Alignment(AlignmentCCW, new Size(10, 10)));
                }
            }

            if (VisibleHome)
            {
                Color color = IsHome ? OnColor : OffColor;

                using (SolidBrush brush = new SolidBrush(color))
                {
                    DC.FillEllipse(brush, Pos2Alignment(AlignmentHome, new Size(10, 10)));
                }
            }
        }

        private void AxisControl_Paint(object sender, PaintEventArgs e)
        {
            DrawAxis(e);
            DrawSensor(e);
        }

        private Rectangle Pos2Alignment(ContentAlignment align, Size size)
        {

            Point location = new Point();
            int w = this.Size.Width;
            int h = this.Size.Height;

            int cx = this.Size.Width / 2;
            int cy = this.Size.Height / 2;

            switch (align)
            {
                default: break;
                case ContentAlignment.TopLeft: location.X = 0; location.Y = 0; break;
                case ContentAlignment.TopCenter: location.X = cx - (size.Width / 2); location.Y = 0; break;
                case ContentAlignment.TopRight: location.X = w - (size.Width); location.Y = 0; break;

                case ContentAlignment.MiddleLeft: location.X = 0; location.Y = cy - (size.Height / 2); break;
                case ContentAlignment.MiddleCenter: location.X = cx - (size.Width / 2); location.Y = cy - (size.Height / 2); break;
                case ContentAlignment.MiddleRight: location.X = w - (size.Width); location.Y = cy - (size.Height / 2); break;

                case ContentAlignment.BottomLeft: location.X = 0; location.Y = h - (size.Height); break;
                case ContentAlignment.BottomCenter: location.X = cx - (size.Width / 2); location.Y = h - (size.Height); break;
                case ContentAlignment.BottomRight: location.X = w - (size.Width); location.Y = h - (size.Height); break;
            }

            return new Rectangle(location, size);
        }

        //=====================================================================
        public void SetMinMaxPos(double minPos, double maxPos)
        {
            this.MinPos = minPos;
            this.MaxPos = maxPos;
        }

        public void UpdateSensor(bool home, bool cw, bool ccw)
        {
            this.IsHome = home;
            this.IsCCW = ccw;
            this.IsCW = cw;
        }

        public void UpdateEncoder(double encPos)
        {
            this.EncPos = encPos;
        }
    }
}
