using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    public partial class TAlgorithm
    {
        public void DrawCrossLine(Bitmap Src ,Pen pen, PointF pt, float length)
        {
            if (Src == null) return;

            using (var graphics = Graphics.FromImage(Src))
            {
                graphics.DrawLine(pen, new PointF(pt.X - length, pt.Y), new PointF(pt.X + length, pt.Y));
                graphics.DrawLine(pen, new PointF(pt.X, pt.Y - length), new PointF(pt.X, pt.Y + +length));
            }
        }

        public void DrawRectangle(Bitmap Src, Brush brush, Rectangle rect)
        {
            if (Src == null) return;

            using (var graphics = Graphics.FromImage(Src))
            {
                using (Pen pen = new Pen(brush,5))
                {
                    graphics.DrawRectangle(pen, rect);
                }
            }
        }

        public void DrawLine(Bitmap Src, Pen pen, PointF pt1, PointF pt2)
        {
            if (Src == null) return;

            using (var graphics = Graphics.FromImage(Src))
            {
                graphics.DrawLine(pen, pt1, pt2);
            }
        }

        public void DrawPoints(Bitmap Src, Pen pen, PointF[] pts)
        {
            if (Src == null) return;

            using (var graphics = Graphics.FromImage(Src))
            {
                GraphicsPath path = new GraphicsPath();

                path.AddLines(pts);

                graphics.DrawPath(pen, path);

                path?.Dispose();
            }
        }

        public void DrawString(Bitmap Src, Font font, Brush brush, PointF pt, string text)
        {
            if (Src == null) return;

            using (var graphics = Graphics.FromImage(Src))
            {
                graphics.DrawString(text, font, brush, pt);
            }
        }
        public void DrawCircle(Bitmap Src, Pen pen, PointF pt, float pRadius)
        {
            if (Src == null) return;

            using (var graphics = Graphics.FromImage(Src))
            {
                graphics.DrawEllipse(pen, pt.X-((pRadius*2)/2), pt.Y - ((pRadius*2) / 2), pRadius*2, pRadius*2);
                //graphics.DrawLine(pen, new PointF(pt.X - length, pt.Y), new PointF(pt.X + length, pt.Y));
                //graphics.DrawLine(pen, new PointF(pt.X, pt.Y - length), new PointF(pt.X, pt.Y + +length));
            }
        }

    }
}
