using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Extension
{
    public static class ContourExtension
    {
        public static void GetBoundingRect(ContourPoints mPoints, out Rect mBoundingRect)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });
            OpenCvSharp.Rect temp = Cv2.BoundingRect(Contour.ToArray());

            mBoundingRect = new Rect(temp.X, temp.Y, temp.Width, temp.Height);
            Contour.Clear();
        }
        public static void GetMinAreaRect(ContourPoints mPoints, out RotatedRect mMinAreaRect)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });
            OpenCvSharp.RotatedRect temp = Cv2.MinAreaRect(Contour.ToArray());

            mMinAreaRect = new RotatedRect(new Point2f(temp.Center.X, temp.Center.Y),
                                            new SizeF(temp.Size.Width, temp.Size.Height),
                                            temp.Angle);
            Contour.Clear();;
        }
        public static Point2f[] GetBoxPoints(RotatedRect rect)
        {
            OpenCvSharp.RotatedRect temp = new OpenCvSharp.RotatedRect(
                new OpenCvSharp.Point2f(rect.Center.X, rect.Center.Y),
                new OpenCvSharp.Size2f(rect.Size.Width, rect.Size.Height),
                rect.Angle);

            OpenCvSharp.Point2f[] vertex = Cv2.BoxPoints(temp);
            List<Point2f> points = vertex.ToList().ConvertAll(delegate (OpenCvSharp.Point2f point) { return new Point2f(point.X, point.Y); });

            return points.ToArray();
        }

        public static void GetMinEnclosingCircle(ContourPoints mPoints, out Point2f pCenter, out float pRadius)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });
            Cv2.MinEnclosingCircle(Contour, out OpenCvSharp.Point2f center, out float radius);
            pCenter = new Point2f(center.X, center.Y);
            pRadius = radius;
            Contour.Clear();
        }

        public static double GetMinEnclosingTriangle(ContourPoints mPoints, out Point2f[] mTriangle)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            double dArea = Cv2.MinEnclosingTriangle(Contour, out OpenCvSharp.Point2f[] triangle);
            List<Point2f> temp = triangle.ToList().ConvertAll(delegate (OpenCvSharp.Point2f point) { return new Point2f(point.X, point.Y); });
            mTriangle = temp.ToArray();
            Contour.Clear();

            return dArea;
        }

        public static RotatedRect FitEllipse(ContourPoints mPoints)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            OpenCvSharp.RotatedRect temp = Cv2.FitEllipse(Contour);
            return new RotatedRect(new Point2f(temp.Center.X, temp.Center.Y),
                               new SizeF(temp.Size.Width, temp.Size.Height),
                               temp.Angle);
        }
        /// <summary>
        /// 중심점을 통과하는 직선
        /// </summary>
        /// <param name="mPoints"></param>
        /// <param name="mType">거리 계산 방식</param>
        /// <param name="mParam">distType에 전달할 인자(최적값 0)</param>
        /// <param name="mReps">반지름 정확도</param>
        /// <param name="mAeps">각도 정확도</param>
        /// <returns></returns>
        public static Line2D Fitline(ContourPoints mPoints, DistanceTypes mType = DistanceTypes.L2, double mParam = 0, double mReps = 0.01, double mAeps = 0.01)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });
            OpenCvSharp.Line2D temp = Cv2.FitLine(Contour, mType, mParam, mReps, mAeps);

            return new Line2D(temp.Vx, temp.Vy, temp.X1, temp.Y1);
        }


        public static double GetArcLength(ContourPoints mPoints, bool closed)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            return Cv2.ArcLength(Contour, closed);
        }

        /// <summary>
        /// It is the ratio of width to height of bounding rect of the object.
        /// </summary>
        /// <param name="mPoints"></param>
        /// <returns></returns>
        public static double GetAspectRatio(ContourPoints mPoints)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            var rect = Cv2.BoundingRect(Contour);

            return rect.Width / rect.Height;
        }

        /// <summary>
        /// Extent is the ratio of contour area to bounding rectangle area.
        /// </summary>
        /// <param name="mPoints"></param>
        /// <returns></returns>
        public static double GetExtent(ContourPoints mPoints)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            var area = Cv2.ContourArea(Contour, false);
            var rect = Cv2.BoundingRect(Contour);

            return area / rect.Width * rect.Height;
        }

        // <summary>
        // Solidity is the ratio of contour area to its convex hull area.
        // </summary>
        // <param name="mPoints"></param>
        // <returns></returns>
        public static double GetSolidity(ContourPoints mPoints)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            var hull = Cv2.ConvexHull(Contour);

            return Cv2.ContourArea(Contour, false) / Cv2.ContourArea(hull, false);
        }

        // <summary>
        // Equivalent Diameter is the diameter of the circle whose area is same as the contour area.
        // </summary>
        // <param name="mPoints"></param>
        // <returns></returns>
        public static double GetEquivalentDiameter(ContourPoints mPoints)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            return Math.Sqrt(4 * Cv2.ContourArea(Contour, false) / Math.PI);
        }

        // <summary>
        // Orientation is the angle at which object is directed. Following method also gives the Major Axis and Minor Axis lengths.
        // </summary>
        // <param name="mPoints"></param>
        // <returns></returns>
        public static double GetOrientation(ContourPoints mPoints)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            return Cv2.FitEllipse(Contour).Angle;
        }


        public static double GetContourArea(ContourPoints mPoints, bool oriented = false)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            return Cv2.ContourArea(Contour, oriented);
        }
        public static bool isContourConvex(ContourPoints mPoints)
        {
            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            return Cv2.IsContourConvex(Contour);
        }

        public static List<Point2f> GetApproxPolyDP(ContourPoints mPoints, double epsilon, bool closed)
        {
            List<Point2f> ApproxPolyDP = new List<Point2f>();

            List<OpenCvSharp.Point> Contour = mPoints.Points.ConvertAll(delegate (Point2f point) { return new OpenCvSharp.Point(point.X, point.Y); });

            Cv2.ApproxPolyDP(Contour, epsilon, closed);

            return ApproxPolyDP;
        }

        



    }

}
