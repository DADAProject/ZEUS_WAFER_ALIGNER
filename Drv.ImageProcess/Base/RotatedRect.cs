using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Drv.ImageProcess
{
    public struct RotatedRect
    {
        public Point2f Center;
        public SizeF Size;
        public float Angle;

        public RotatedRect(float x, float y, float w, float h, float angle)
        {
            Center = new Point2f(x + w / 2, y+ h / 2);
            Size = new SizeF(w, h);
            Angle = angle;
        }

        public RotatedRect(Point2f center, SizeF size, float angle)
        {
            Center = center;
            Size = size;
            Angle = angle;
        }


    }
}
