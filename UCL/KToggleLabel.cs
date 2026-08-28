using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms
{
    public partial class KToggleLabel : Label
    {
        private Color _OnColor;
        private Color _OffColor;
        private bool _Checked;
        private int _LedWidth;
        private string _TextOn;
        private string _TextOff;
        private bool _TextOnOffEnable;
        private bool _LedFullEnable;
        private bool _AutoCheck;

        public KToggleLabel()
        {
            InitializeComponent();
            init();
        }

        public KToggleLabel(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            init();
        }

        private void init()
        {
            _OnColor = Color.DarkOrange;
            _OffColor = Color.Gray;
            _Checked = false;
            _LedWidth = 20;
            _AutoCheck = false;
            base.AutoSize = false;
            this.Width = 110;
        }

        protected override void OnClick(EventArgs e)
        {
            if (AutoCheck)
                Checked = !Checked;
            base.OnClick(e);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle backrect = new Rectangle(0, 0, this.Width, this.Height);
            SolidBrush BackBrush = new SolidBrush(this.BackColor);
            e.Graphics.FillRectangle(BackBrush, backrect);

            int rectOffset = 0;
            if (BorderStyle != System.Windows.Forms.BorderStyle.None)
                rectOffset = 1;

            Rectangle ledrect;
            if (_LedFullEnable)
                ledrect = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Left - Padding.Right - rectOffset * 2, Height - Padding.Bottom - Padding.Top - rectOffset * 2);
            else
                ledrect = new Rectangle(Padding.Left, Padding.Top, _LedWidth, Height - Padding.Bottom - Padding.Top - rectOffset * 2);

            SolidBrush LedBrush;
            if (Enabled)
                LedBrush = new SolidBrush(_Checked ? _OnColor : OffColor);
            else
                LedBrush = new SolidBrush(_Checked ? Color.FromArgb(80, _OnColor) : Color.FromArgb(80, _OffColor));

            e.Graphics.FillRectangle(LedBrush, ledrect);

            // draw text on label
            SolidBrush drawBrush;
            if (Enabled)
                drawBrush = new SolidBrush(ForeColor);
            else
                drawBrush = new SolidBrush(Color.FromArgb(80, ForeColor));

            StringFormat sf = new StringFormat();

            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }

            RectangleF rectF;
            if (_LedFullEnable)
                rectF = new RectangleF(Padding.Left, Padding.Top, Width - Padding.Left - Padding.Right, Height - Padding.Top - Padding.Bottom);
            else
                rectF = new RectangleF(_LedWidth + Padding.Left, Padding.Top, Width - _LedWidth - Padding.Left - Padding.Right, Height - Padding.Top - Padding.Bottom);

            if (_TextOnOffEnable)
                e.Graphics.DrawString(_Checked ? _TextOn : _TextOff, Font, drawBrush, rectF, sf);
            else
                e.Graphics.DrawString(Text, Font, drawBrush, rectF, sf);

        }

        // property of On Color
        [Category("User")]
        [Description("On Color")]
        public Color OnColor
        {
            get
            {
                return _OnColor;
            }
            set
            {
                if (_OnColor != value)
                {
                    _OnColor = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("Off Color")]
        public Color OffColor
        {
            get
            {
                return _OffColor;
            }
            set
            {
                if (_OffColor != value)
                {
                    _OffColor = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("On/Off Status")]
        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                if (_Checked != value)
                {
                    _Checked = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("LED Rectangle Width")]
        public int LedWidth
        {
            get
            {
                return _LedWidth;
            }
            set
            {
                if (_LedWidth != value)
                {
                    _LedWidth = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("On Text")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string TextOn
        {
            get { return _TextOn; }
            set
            {
                if (_TextOn != value)
                {
                    _TextOn = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("Off Text")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string TextOff
        {
            get { return _TextOff; }
            set
            {
                if (_TextOff != value)
                {
                    _TextOff = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("On/Off Text Enable")]
        public bool TextOnOffEnable
        {
            get { return _TextOnOffEnable; }
            set
            {
                if (_TextOnOffEnable != value)
                {
                    _TextOnOffEnable = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("Led Full Enable")]
        public bool LedFullEnable
        {
            get { return _LedFullEnable; }
            set
            {
                if (_LedFullEnable != value)
                {
                    _LedFullEnable = value;
                    Invalidate();
                }
            }
        }

        [Category("User")]
        [Description("Auto Check")]
        public bool AutoCheck
        {
            get { return _AutoCheck; }
            set
            {
                if (_AutoCheck != value)
                    _AutoCheck = value;
            }
        }

    }
}
