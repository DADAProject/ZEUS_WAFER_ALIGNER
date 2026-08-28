using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class RoundPanel : GroupBox
    {
        private Color titleBackColor;
        //private HatchStyle  titleHatchStyle;
        private Font titleFont;
        private Color titleForeColor;
        private int radious;

        [Category("Ex"), Description("Title Back Color")]
        public Color TitleBackColor { get { return titleBackColor; } set { titleBackColor = value; Invalidate(); } }
        //[Category("Ex"), Description("Title Hatch Style")]
        //public HatchStyle TitleHatchStyle { get { return titleHatchStyle; }  set { titleHatchStyle = value; Invalidate(); } }
        [Category("Ex"), Description("Title Font")]
        public Font TitleFont { get { return titleFont; } set { titleFont = value; Invalidate(); } }
        [Category("Ex"), Description("Title ForeColor")]
        public Color TitleForeColor { get { return titleForeColor; } set { titleForeColor = value; Invalidate(); } }
        [Category("Ex"), Description("Radius")]
        public int Radious { get { return radious; } set { radious = value; Invalidate(); } }

        public RoundPanel()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.DoubleBuffered = true;
            this.titleBackColor = Color.SteelBlue;
            this.titleForeColor = Color.White;
            this.titleFont = new Font(this.Font.FontFamily, Font.Size, FontStyle.Bold);
            this.BackColor = Color.Transparent;
            this.radious = 15;
            //this.titleHatchStyle = HatchStyle.Percent60;
        }
        private GraphicsPath GetRoundRectagle(Rectangle b, int r)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(b.X, b.Y, r, r, 180, 90);
            path.AddArc(b.X + b.Width - r - 1, b.Y, r, r, 270, 90);
            path.AddArc(b.X + b.Width - r - 1, b.Y + b.Height - r - 1, r, r, 0, 90);
            path.AddArc(b.X, b.Y + b.Height - r - 1, r, r, 90, 90);
            path.CloseAllFigures();
            return path;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GroupBoxRenderer.DrawParentBackground(e.Graphics, this.ClientRectangle, this);
            var rect = this.ClientRectangle;
            using (var path = GetRoundRectagle(this.ClientRectangle, radious))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (!string.IsNullOrEmpty(Text))
                {
                    rect = new Rectangle(0, 0, rect.Width, titleFont.Height + Padding.Bottom + Padding.Top);
                }
                else rect = new Rectangle(0, 0, rect.Width, 1);
                //          
                if (this.BackColor != Color.Transparent)
                    using (var brush = new SolidBrush(BackColor))
                        e.Graphics.FillPath(brush, path);
                var clip = e.Graphics.ClipBounds;
                e.Graphics.SetClip(rect);
                //
                using (var brush = new SolidBrush(titleBackColor)) //new HatchBrush(titleHatchStyle, titleBackColor, ControlPaint.Light(titleBackColor)))
                    e.Graphics.FillPath(brush, path);
                using (var pen = new Pen(titleBackColor, 1))
                    e.Graphics.DrawPath(pen, path);
                //
                TextRenderer.DrawText(e.Graphics, Text, titleFont, rect, titleForeColor);
                e.Graphics.SetClip(clip);
                using (var pen = new Pen(titleBackColor, 1))
                    e.Graphics.DrawPath(pen, path);
            }
        }
    }
}
