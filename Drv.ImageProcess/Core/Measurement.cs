using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Core
{
	public enum MAKER_TYPE
	{
		E_EDGE		= 2,
		E_STRIPEL	= 3,
	}

	public enum MAKER_ORIENTATION
	{
		E_VERTICAL   = 1,
		E_HORIZONTAL = 2,
	}

	public enum MAKER_POLARITY
	{
		E_NEGATIVE = -2,
		E_POSITIVE =  2,
		E_OPPOSITE =  3,
		E_SAME     =  4
	}
	public enum MAKER_BOX_OPERATION
	{
		E_MAKER_BOX_ENABLE,
		E_MAKER_BOX_DISABLE,
	}

	public struct stMeasParam
	{
		public MAKER_TYPE Type { get; set; }

		public double MeasWidthStd { get; set; }
		public int MeasWidth_Up_OffSet_Percent { get; set; }
		public int MeasWidth_Down_OffSet_Percent { get; set; }

		public MAKER_POLARITY FstPolarity { get; set; }
		public MAKER_POLARITY SecPolarity { get; set; }
		public MAKER_ORIENTATION Orienatation { get; set; }
	}
}
