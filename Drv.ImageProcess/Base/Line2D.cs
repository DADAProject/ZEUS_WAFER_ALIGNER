using Drv.ImageProcess.Base;
using Euresys.Open_eVision_22_04;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess
{
    public struct Line2D : IEquatable<Line2D>
    {
        public double Vx { get; }
        public double Vy { get; }
        public double X1 { get; }
        public double Y1 { get; }

        private PointF[] _point;

        public Line2D(float[] line, PointF[] points = null)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            Vx = line[0];
            Vy = line[1];
            X1 = line[2];
            Y1 = line[3];

            _point = points;
        }

        public Line2D(double vx, double vy, double x1, double y1, PointF[] points = null)
        {
            Vx = vx;
            Vy = vy;
            X1 = x1;
            Y1 = y1;

            _point = points;
        }

        public PointF[] GetLinePoint()
        {
            return _point;
        }
        
        public double GetVectorRadian()
        {
            return Math.Atan2(Vy, Vx);
        }
        public double GetVectorAngle()
        {
            return GetVectorRadian() * 180 / Math.PI;
        }
        public double Distance(Point2f point)
        {
            return Distance(point.X, point.Y);
        }

        public double Distance(double x, double y)
        {
            // 公式で
            var m = Vy / Vx;
            var n = Y1 - m * X1;
            return Math.Abs(y - m * x - n) / Math.Sqrt(1 + m * m);
        }

        /// <summary>
        /// Compare this line with <paramref name="line2"/>
        /// </summary>
        /// <param name="line2">The other box to be compared</param>
        /// <returns>true if the two boxes equals</returns>
        public bool Equals(Line2D line2)
        {
            return X1.Equals(line2.X1) && Y1.Equals(line2.Y1) &&
                   Vx.Equals(line2.Vx) && Vy.Equals(line2.Vy);
        }

    }
}
