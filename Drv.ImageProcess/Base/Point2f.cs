using System;
using System.Runtime.InteropServices;

namespace Drv.ImageProcess
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Point2f : IEquatable<Point2f>
    {
        public float X { get; }
        public float Y { get; }

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
    }
}
