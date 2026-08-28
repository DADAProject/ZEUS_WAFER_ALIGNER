using System;
using System.Runtime.InteropServices;

namespace Drv.ImageProcess
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2f : IEquatable<Point2f>
    {
        public float X;
        public float Y;

        public Point2f(float x, float y)
        {
            X = x;
            Y = y;

        }

        public bool Equals(Point2f other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }
        public static bool operator ==(Point2f lhs, Point2f rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(Point2f lhs, Point2f rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static double Distance(Point2f p1, Point2f p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        public double DistanceTo(Point2f p)
        {
            return Distance(this, p);
        }
        public static double DotProduct(Point2f p1, Point2f p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public double DotProduct(Point2f p)
        {
            return DotProduct(this, p);
        }

        public static double CrossProduct(Point2f p1, Point2f p2)
        {
            return p1.X * p2.Y - p2.X * p1.Y;
        }
        public double CrossProduct(Point2f p)
        {
            return CrossProduct(this, p);
        }
    }
}
