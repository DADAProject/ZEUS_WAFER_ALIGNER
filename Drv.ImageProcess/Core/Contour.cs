using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess
{
	public enum RETRIVAL_MODE : int
	{
		E_EXTERNAL       = 0,
		E_LIST           = 1,
		E_CCOMP          = 2,
		E_TREE		     = 3,
		E_FLOODFILL      = 4,
	}

	public enum APPROXIMATION_MODE : int
	{
		E_APPROXNONE     = 1,
		E_APPROXSIMPLE   = 2,
		E_APPROXTC89L1   = 3,
		E_APPROXTC89KCOS = 4
	}

	public class Hierarchy
	{
		public int Next { get; set; }
		public int Previous { get; set; }
		public int Child { get; set; }
		public int Parent { get; set; }

		public Hierarchy()
		{ 
		
		}

		public Hierarchy(int next, int previous, int child, int parent)
		{
			Next = next;
			Previous = previous;
			Child = child;
			Parent = parent;
		}

	}

    public class ContourPoints 
	{
		public int num { get; set; }

		public List<Point2f> Points { get; set; } //속도 체크 => 배열 할당시 GC 호출로 인한 딜레이 발생
		public Hierarchy Hierarchy { get; set; }

		public ContourPoints()
		{
			Points	   = new List<Point2f>();
            Hierarchy  = new Hierarchy();
			num		   = 0;
        }

		public void Update(double dx, double dy, Hierarchy hierarchy = null)
		{
			Points.Add(new Point2f((float)dx, (float)dy));
		}
        public void Update(Hierarchy hierarchy )
        {
			Hierarchy = hierarchy;
        }

        public void Clear()
		{
			Points.Clear();
		}
	}

	internal partial class Contour
	{
		internal int MAX_CONTOUR = 99999;
	}
}
