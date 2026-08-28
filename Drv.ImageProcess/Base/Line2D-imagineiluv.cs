using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess
{
    public struct LineSegmentPoint : IEquatable<LineSegmentPoint>
    {
        public Point2f P1;
        public Point2f P2;

        public LineSegmentPoint(Point2f p1, Point2f p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public bool Equals(LineSegmentPoint other)
        {
            return (P1 == other.P1 && P2 == other.P2);
        }

        public static bool operator ==(LineSegmentPoint lhs, LineSegmentPoint rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(LineSegmentPoint lhs, LineSegmentPoint rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return P1.GetHashCode() + P2.GetHashCode();
        }

        public override string ToString()
        {
            return $"LineSegmentPoint (P1:{P1} P2:{P2})";
        }

        public static Point? LineIntersection(LineSegmentPoint line1, LineSegmentPoint line2)
        {
            var x1 = line1.P1.X;
            var y1 = line1.P1.Y;
            var f1 = line1.P2.X - line1.P1.X;
            var g1 = line1.P2.Y - line1.P1.Y;
            var x2 = line2.P1.X;
            var y2 = line2.P1.Y;
            var f2 = line2.P2.X - line2.P1.X;
            var g2 = line2.P2.Y - line2.P1.Y;

            double det = f2 * g1 - f1 * g2;
            if (Math.Abs(det) < 1e-9)
            {
                return null;
            }

            var dx = x2 - x1;
            var dy = y2 - y1;
            var t1 = (f2 * dy - g2 * dx) / det;
            //var t2 = (f1*dy - g1*dx)/det;

            return new Point
            {
                X = (int)Math.Round(x1 + (f1 * t1)),
                Y = (int)Math.Round(y1 + (g1 * t1))
            };
        }

        public Point? LineIntersection(LineSegmentPoint line)
        {
            return LineIntersection(this, line);
        }

        public static Point? SegmentIntersection(LineSegmentPoint seg1, LineSegmentPoint seg2)
        {
            if (IntersectedSegments(seg1, seg2))
                return LineIntersection(seg1, seg2);
            else
                return null;
        }

        public Point? SegmentIntersection(LineSegmentPoint seg)
        {
            return SegmentIntersection(this, seg);
        }
        public static bool IntersectedSegments(LineSegmentPoint seg1, LineSegmentPoint seg2)
        {
            var p1 = seg1.P1;
            var p2 = seg1.P2;
            var p3 = seg2.P1;
            var p4 = seg2.P2;

            checked
            {
                if (p1.X >= p2.X)
                {
                    if ((p1.X < p3.X && p1.X < p4.X) || (p2.X > p3.X && p2.X > p4.X))
                        return false;
                }
                else
                {
                    if ((p2.X < p3.X && p2.X < p4.X) || (p1.X > p3.X && p1.X > p4.X))
                        return false;
                }
                if (p1.Y >= p2.Y)
                {
                    if ((p1.Y < p3.Y && p1.Y < p4.Y) || (p2.Y > p3.Y && p2.Y > p4.Y))
                        return false;
                }
                else
                {
                    if ((p2.Y < p3.Y && p2.Y < p4.Y) || (p1.Y > p3.Y && p1.Y > p4.Y))
                        return false;
                }

                if (((long)(p1.X - p2.X) * (p3.Y - p1.Y) + (long)(p1.Y - p2.Y) * (p1.X - p3.X)) *
                    ((long)(p1.X - p2.X) * (p4.Y - p1.Y) + (long)(p1.Y - p2.Y) * (p1.X - p4.X)) > 0)
                    return false;
                if (((long)(p3.X - p4.X) * (p1.Y - p3.Y) + (long)(p3.Y - p4.Y) * (p3.X - p1.X)) *
                    ((long)(p3.X - p4.X) * (p2.Y - p3.Y) + (long)(p3.Y - p4.Y) * (p3.X - p2.X)) > 0)
                    return false;
            }
            return true;
        }
        public bool IntersectedSegments(LineSegmentPoint seg)
        {
            return IntersectedSegments(this, seg);
        }

        public static bool IntersectedLineAndSegment(LineSegmentPoint line, LineSegmentPoint seg)
        {
            var p1 = line.P1;
            var p2 = line.P2;
            var p3 = seg.P1;
            var p4 = seg.P2;
            if (((long)(p1.X - p2.X) * (p3.Y - p1.Y) + (long)(p1.Y - p2.Y) * (p1.X - p3.X)) *
                ((long)(p1.X - p2.X) * (p4.Y - p1.Y) + (long)(p1.Y - p2.Y) * (p1.X - p4.X)) > 0)
            {
                return false;
            }
            return true;
        }

        public static Point? LineAndSegmentIntersection(LineSegmentPoint line, LineSegmentPoint seg)
        {
            if (IntersectedLineAndSegment(line, seg))
                return LineIntersection(line, seg);
            else
                return null;
        }

        public double Length()
        {
            return P1.DistanceTo(P2);
        }

        public void Offset(int x, int y)
        {
            P1.X += x;
            P1.Y += y;
            P2.X += x;
            P2.Y += y;
        }

        public void Offset(Point p)
        {
            Offset(p.X, p.Y);
        }
    }
}
