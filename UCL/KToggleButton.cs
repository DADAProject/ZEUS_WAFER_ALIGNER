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
    public partial class KToggleButton : Button
    {
        private Color _OnColor;
        private Color _OffColor;
        private bool _Checked;
        private bool _AutoCheck;
        private bool _LedVisible;
        private int _LedWidth;
        private string _TextOn;
        private string _TextOff;
        private string _TextOrigin;
        private bool _TextOnOffEnable;
        private bool _LedFullEnable;
        private int _RoundEdge;

        private Color _cCrntBackColor;

        public KToggleButton()
        {
            InitializeComponent();
            //
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
            //
            init();
        }

        public KToggleButton(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            init();
        }

        private void init()
        {
            _OnColor = Color.Lime;
            _OffColor = Color.Red;
            _Checked = false;
            _AutoCheck = false;
            _LedVisible = true;
            _LedWidth = 20;
            _TextOn = "";
            _TextOff = "";
            _TextOrigin = "";
            _TextOnOffEnable = false;
            _RoundEdge = 5;
            _cCrntBackColor = this.BackColor;
            this.Width = 143;
            this.MouseClick += new MouseEventHandler(KToggleButton_MouseClick);
        }

        void KToggleButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (_AutoCheck)
                Checked = !Checked;
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
        [Description("Round Edge")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public int RoundEdge
        {
            get { return _RoundEdge; }
            set
            {
                if (_RoundEdge != value)
                {
                    _RoundEdge = value;
                    Invalidate();
                }
            }
        }


        //[SettingsBindable(true)]
        //[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Category("Text")]
        [Description("Text Original")]
        new public string Text
        {
            get { return Text2; }
            set { Text2 = value; }
        }

        [Browsable(false)]
        public string Text2
        {
            get { return _TextOrigin; }
            set
            {
                if (_TextOrigin != value)
                {
                    _TextOrigin = value;
                    Invalidate();
                }
            }
        }
        private GraphicsPath GetRoundRectagle(Rectangle b, int r)
        {
            GraphicsPath path = new GraphicsPath();
            //
            try
            {
                path.AddArc(b.X, b.Y, r, r, 180, 90);
                path.AddArc(b.X + b.Width - r - 1, b.Y, r, r, 270, 90);
                path.AddArc(b.X + b.Width - r - 1, b.Y + b.Height - r - 1, r, r, 0, 90);
                path.AddArc(b.X, b.Y + b.Height - r - 1, r, r, 90, 90);
                path.CloseAllFigures();
            }
            catch (Exception err) { }
            finally { }
            //
            return path;
        }
        private void UserPaint(Graphics g, Color backColor)
        {
            GroupBoxRenderer.DrawParentBackground(g, this.ClientRectangle, this);
            Rectangle ledrect;
            SolidBrush LedBrush;

            //
            using (var path = GetRoundRectagle(this.ClientRectangle, _RoundEdge))
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //
                if (backColor != Color.Transparent)
                {
                    using (var brush = new SolidBrush(backColor))
                        g.FillPath(brush, path);

                    _cCrntBackColor = backColor;
                }
                //
                if (this.FlatAppearance.BorderSize > 0)
                {
                    using (var pen = new Pen(this.FlatAppearance.BorderColor, this.FlatAppearance.BorderSize))
                        g.DrawPath(pen, path);
                }

                //
                if (Enabled)
                    LedBrush = new SolidBrush(_Checked ? _OnColor : OffColor);
                else
                    LedBrush = new SolidBrush(_Checked ? Color.FromArgb(80, _OnColor) : Color.FromArgb(80, _OffColor));

                if (_LedVisible)
                {
                    if (_LedFullEnable)
                        ledrect = new Rectangle(this.ClientRectangle.X + 2 + Padding.Left, this.ClientRectangle.Y + 2 + Padding.Top, this.ClientRectangle.Width - 4 - Padding.Left - Padding.Right, this.ClientRectangle.Height - 4 - Padding.Top - Padding.Bottom);
                    else
                        ledrect = new Rectangle(this.ClientRectangle.X + 2 + Padding.Left, this.ClientRectangle.Y + 2 + Padding.Top, _LedWidth, this.ClientRectangle.Height - 4 - Padding.Top - Padding.Bottom);
                    g.FillRectangle(LedBrush, ledrect);
                }
                else
                {
                    ledrect = new Rectangle();
                }

                SolidBrush drawBrush;
                if (Enabled)
                    drawBrush = new SolidBrush(ForeColor);
                else
                    drawBrush = new SolidBrush(Color.FromArgb(80, ForeColor));

                //
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

                Rectangle rectF;
                if (_LedFullEnable || !_LedVisible)
                    rectF = new Rectangle(this.ClientRectangle.X + Padding.Left, this.ClientRectangle.Y + Padding.Top + 1, this.ClientRectangle.Width - 2 - Padding.Left - Padding.Right, this.ClientRectangle.Height - Padding.Top - Padding.Bottom);
                else
                    rectF = new Rectangle(this.ClientRectangle.X + 2 + Padding.Left + _LedWidth, this.ClientRectangle.Y + Padding.Top + 1, this.ClientRectangle.Width - (this.ClientRectangle.X + 2 + Padding.Left + Padding.Right + _LedWidth), this.ClientRectangle.Height - Padding.Top - Padding.Bottom);

                if (_TextOnOffEnable)
                    g.DrawString(_Checked ? _TextOn : _TextOff, Font, drawBrush, rectF, sf);
                else
                    g.DrawString(_TextOrigin, Font, drawBrush, rectF, sf);
            }
            //
            DrawImage(g, ledrect);
        }
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);

            if (_cCrntBackColor != this.FlatAppearance.MouseOverBackColor)
            {
                Graphics g = CreateGraphics();
                UserPaint(g, this.FlatAppearance.MouseOverBackColor);
            }

        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            UserPaint(e.Graphics, this.BackColor);
        }
        private void DrawImage(Graphics graphics, Rectangle LedRect)
        {
            if (this.Image == null) { return; }
            Rectangle rectangle = new Rectangle(8, 8, this.Image.Width, this.Image.Height);
            switch (this.ImageAlign)
            {
                case ContentAlignment.TopCenter: rectangle = new Rectangle((Width / 2 - this.Image.Width / 2) + LedRect.X, 8, this.Image.Width, this.Image.Height); break;
                case ContentAlignment.TopRight: rectangle = new Rectangle((Width - 8 - this.Image.Width) + LedRect.X, 8, this.Image.Width, this.Image.Height); break;
                case ContentAlignment.MiddleLeft: rectangle = new Rectangle(8 + LedRect.X, Height / 2 - this.Image.Height / 2, this.Image.Width, this.Image.Height); break;
                case ContentAlignment.MiddleCenter: rectangle = new Rectangle((Width / 2 - this.Image.Width / 2), Height / 2 - this.Image.Height / 2, this.Image.Width, this.Image.Height); break; //+ LedRect.X
                case ContentAlignment.MiddleRight: rectangle = new Rectangle((Width - 8 - this.Image.Width) + LedRect.X, Height / 2 - this.Image.Height / 2, this.Image.Width, this.Image.Height); break;
                case ContentAlignment.BottomLeft: rectangle = new Rectangle(8 + LedRect.X, Height - 8 - this.Image.Height, this.Image.Width, this.Image.Height); break;
                case ContentAlignment.BottomCenter: rectangle = new Rectangle((Width / 2 - this.Image.Width / 2) + LedRect.X, Height - 8 - this.Image.Height, this.Image.Width, this.Image.Height); break;
                case ContentAlignment.BottomRight: rectangle = new Rectangle((Width - 8 - this.Image.Width) + LedRect.X, Height - 8 - this.Image.Height, this.Image.Width, this.Image.Height); break;
            }

            graphics.DrawImage(this.Image, rectangle);
        }

        [Category("LED Status")]
        [Description("LED Visible")]
        public bool LedVisible
        {
            get
            {
                return _LedVisible;
            }
            set
            {
                if (_LedVisible != value)
                {
                    _LedVisible = value;
                    Invalidate();
                }
            }
        }

        [Category("LED Status")]
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

        [Category("LED Status")]
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

        [Category("LED Status")]
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

        [Category("LED Status")]
        [Description("Auto Check Status")]
        public bool AutoCheck
        {
            get
            {
                return _AutoCheck;
            }
            set
            {
                if (_AutoCheck != value)
                {
                    _AutoCheck = value;
                }
            }
        }


        [Category("LED Status")]
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

        [Category("LED Status")]
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
    }
}

/*
            GroupBoxRenderer.DrawParentBackground(e.Graphics, this.ClientRectangle, this);
            Rectangle  ledrect;
            SolidBrush LedBrush;
            
            //
            using (var path = GetRoundRectagle(this.ClientRectangle, _RoundEdge))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //
                if (this.BackColor != Color.Transparent)
                    using (var brush = new SolidBrush(BackColor))
                        e.Graphics.FillPath(brush, path);
                //
                using (var pen = new Pen(this.FlatAppearance.BorderColor, 1))
                    e.Graphics.DrawPath(pen, path);

                //
                if (Enabled)
                    LedBrush = new SolidBrush(_Checked?_OnColor:OffColor);
                else
                    LedBrush = new SolidBrush(_Checked?Color.FromArgb(80,_OnColor):Color.FromArgb(80,_OffColor));
                
                if (_LedVisible)
                {
                    if(_LedFullEnable)
                        ledrect  = new Rectangle (this.ClientRectangle.X+2+Padding.Left,this.ClientRectangle.Y+2+Padding.Top,this.ClientRectangle.Width-4-Padding.Left-Padding.Right,this.ClientRectangle.Height-4-Padding.Top-Padding.Bottom);
                    else
                        ledrect  = new Rectangle (this.ClientRectangle.X+2+Padding.Left,this.ClientRectangle.Y+2+Padding.Top,_LedWidth,this.ClientRectangle.Height-4-Padding.Top-Padding.Bottom);
                    e.Graphics.FillRectangle(LedBrush, ledrect);
                }
                else
                {
			        ledrect = new Rectangle();
                }

                SolidBrush drawBrush;
                if (Enabled)
                    drawBrush = new SolidBrush(ForeColor);
                else
                    drawBrush = new SolidBrush(Color.FromArgb(80,ForeColor));

                //
                StringFormat sf = new StringFormat();
                switch(TextAlign)
                {
                    case ContentAlignment.TopLeft   :
                        sf.Alignment     = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Near;
                        break;
                
                    case ContentAlignment.MiddleLeft:
                        sf.Alignment     = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.BottomLeft:
                        sf.Alignment     = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Far;
                        break;
                        
                    case ContentAlignment.TopCenter :
                        sf.Alignment     = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.MiddleCenter:
                        sf.Alignment     = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.BottomCenter:
                        sf.Alignment     = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Far;
                        break;
                
                    case ContentAlignment.TopRight  :
                        sf.Alignment     = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.MiddleRight:
                        sf.Alignment     = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.BottomRight:
                        sf.Alignment     = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Far;
                        break;
                }

                Rectangle  rectF;
                if(_LedFullEnable || !_LedVisible)
                    rectF = new Rectangle(this.ClientRectangle.X +Padding.Left,this.ClientRectangle.Y+Padding.Top+1, this.ClientRectangle.Width -2-Padding.Left - Padding.Right,this.ClientRectangle.Height- Padding.Top - Padding.Bottom);
                else
                    rectF = new Rectangle(this.ClientRectangle.X +2+Padding.Left + _LedWidth,this.ClientRectangle.Y+Padding.Top+1,this.ClientRectangle.Width - (this.ClientRectangle.X +2+Padding.Left+Padding.Right + _LedWidth),this.ClientRectangle.Height- Padding.Top - Padding.Bottom);
                
                if(_TextOnOffEnable)
                    e.Graphics.DrawString(_Checked?_TextOn:_TextOff, Font, drawBrush, rectF, sf);
                else
                    e.Graphics.DrawString(_TextOrigin, Font, drawBrush, rectF, sf);
            }
            //
            DrawImage(e.Graphics, ledrect);
*/