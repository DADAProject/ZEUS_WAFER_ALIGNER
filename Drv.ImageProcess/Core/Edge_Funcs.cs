using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
    public enum EDGE_TYPE
    {
        E_BLACK_TO_WHITE = 0,
        E_WHITE_TO_BLACK = 1,
        E_ALL   = 2,
    }

    //나중엔 나눠 쓰자.
    public struct stFittingParam
	{
		#region Circle Fit
		public int RadiusOffSet { get; set; }
		public int CenterOffSet { get; set; }
		public int FitRadius_Up_OffSet_Percent { get; set; }
		public int FitRadius_Down_OffSet_Percent { get; set; }
		public double FitRadiusStd { get; set; }
		#endregion

		#region Rectangle Fit
	    public int FitSize_Up_OffSet_Percent { get; set; }
	    public int FitSize_Down_OffSet_Percent { get; set; }
		public double FitAngleStd { get; set; }
		#endregion
	}

	public struct stFittingRectParam
	{
		public int CenterOffSet_X { get; set; }
		public int CenterOffSet_Y { get; set; }

		public int FitSize_Tolerance { get; set; } // Pixel
		public double FitWidthStd { get; set; }     //Pixel
		public double FitHeightStd { get; set; }     //Pixel
		public double FitAngleStd { get; set; }     
	}

    public struct stFittingCircleParam
    {
        public int CenterOffSet_X                { get; set; }
        public int CenterOffSet_Y                { get; set; }
        public int FitRadius_Tolerance           { get; set; }
        public int FitRadius_Up_OffSet_Percent   { get; set; }
        public int FitRadius_Down_OffSet_Percent { get; set; }
        public int RadiusOffSet                  { get; set; }
        public double FitRadiusStd               { get; set; }
    }

    public enum FIND_POLARITY 
    {
        E_POSITIVE = 1,
        E_NEGATIVE = 2,
    }

    public enum FIND_ORIENTATION
    {
        E_VERTICAL = 1,
        E_HORIZONTAL = 2,
    }

    public struct stFittingLineParam
    {
        public int CenterOffSet_X { get; set; }
        public int CenterOffSet_Y { get; set; }
        public int FitLength_Tolerance { get; set; }
        public int FitLength_Up_OffSet_Percent { get; set; }
        public int FitLength_Down_OffSet_Percent { get; set; }
        public int LengthOffSet { get; set; }
        public double FitLengthStd { get; set; }
        public double FitAngleStd { get; set; }
        public double FitAngle_Tolerance { get; set; }
    }

}
