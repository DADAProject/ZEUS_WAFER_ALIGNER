using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess
{
    public readonly struct LineSegmentPoint
    {
        public double Vx { get; }
        public double Vy { get; }
        public double X1 { get; }
        public double Y1 { get; }

        public LineSegmentPoint(float[] line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            Vx = line[0];
            Vy = line[1];
            X1 = line[2];
            Y1 = line[3];
        }

        public LineSegmentPoint(Point2f pt1, Point2f pt2)
        {
            Vx = pt1.X;
            Vy = pt1.Y;
            X1 = pt2.X;
            Y1 = pt2.Y;
        }

        public LineSegmentPoint(double vx, double vy, double x1, double y1)
        {
            Vx = vx;
            Vy = vy;
            X1 = x1;
            Y1 = y1;

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
            var m = Vy / Vx;
            var n = Y1 - m * X1;
            return Math.Abs(y - m * x - n) / Math.Sqrt(1 + m * m);
        }


        public void FitSize(int width, int height, out Point pt1, out Point pt2)
        {
            double t = (width + height);
            pt1 = new Point
            {
                X = (int)Math.Round(X1 - Vx * t),
                Y = (int)Math.Round(Y1 - Vy * t)
            };
            pt2 = new Point
            {
                X = (int)Math.Round(X1 + Vx * t),
                Y = (int)Math.Round(Y1 + Vy * t)
            };
        }
    }
}
