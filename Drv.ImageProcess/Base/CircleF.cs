using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Drv.ImageProcess.Base
{
    public struct CircleF : IEquatable<CircleF>
    {
        private PointF _center;

        // The radius of the circle
        private float _radius;

        //Edge Points first Best Score
        private PointF[] _point;

        /// <summary> Create a circle with the specific center and radius </summary>
        /// <param name="center"> The center of this circle </param>
        /// <param name="radius"> The radius of this circle </param>
        public CircleF(PointF center, float radius, PointF[] points = null)
        {
            _center = center;
            _radius = radius;
            _point  = points;
        }

        /// <summary> Get or Set the center of the circle </summary>
        public PointF Center
        {
            get { return _center; }
            set { _center = value; }
        }

        public float Radius { get { return _radius; } set { _radius = value; } }

        public double Area
        {
            get
            {
                return _radius * _radius * Math.PI;
            }
        }

        /// <summary> Get or Set the center of the circle </summary>
        public PointF[] Points
        {
            get { return _point; }
            set { _point = value; }
        }

        /// <summary>
        /// Compare this circle with <paramref name="circle2"/>
        /// </summary>
        /// <param name="circle2">The other box to be compared</param>
        /// <returns>true if the two boxes equals</returns>
        public bool Equals(CircleF circle2)
        {
            return Center.Equals(circle2.Center) && Radius.Equals(circle2.Radius);
        }
    }
}
