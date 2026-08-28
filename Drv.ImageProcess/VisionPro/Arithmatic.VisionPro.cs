using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

namespace Drv.ImageProcess.Core
{
    internal partial class Arithmatic
    {
		internal bool Arith_OneLogic(CogImage8Grey mSrcID, CogImage8Grey mDstID, LOGIC_1_OPERATION eOper)
		{
            switch (eOper)
            {
                case LOGIC_1_OPERATION.E_LOGIC_1_INVERT:
					break;
            }

            return true;
		}
		internal bool Arith_TwoLogic(CogImage8Grey mSrcID1, CogImage8Grey mSrcID2, CogImage8Grey mDstID, LOGIC_2_OPERATION eOper)
		{
			

			return true;
		}


		internal bool Arith_OneCalcurate(CogImage8Grey mSrcID, ARITH_1_OPERATION eOper, out float fValue)
		{
			fValue = 0;

			switch (eOper)
			{
				case ARITH_1_OPERATION.E_ARITH_1_MINIMUM:
					break;
				case ARITH_1_OPERATION.E_ARITH_1_MAXIMUM:
					break;
				case ARITH_1_OPERATION.E_ARITH_1_AVERAGE:
					break;
				case ARITH_1_OPERATION.E_ARITH_1_MEAN:
					break;
				case ARITH_1_OPERATION.E_ARITH_1_VARIANCE:
					break;
				case ARITH_1_OPERATION.E_ARITH_1_STDDEV:
					break;
			}

			return true;
		}


		//internal bool Arith_GravityCenter(CogImage8Grey mSrcID, int iCondLow, out Point2f ptCenter)
		//{


		//	return true;
		//}

		internal bool Arith_PixelCount(CogImage8Grey mSrcID, ARITH_PIXEL_OPERATION eOper, int iCondLow, int iCondHigh, out int iPixels)
		{
			iPixels   = 0;
			

			switch (eOper)
            {
                case ARITH_PIXEL_OPERATION.E_ARITH_PIXEL_BELOW:
					break;
                case ARITH_PIXEL_OPERATION.E_ARITH_PIXEL_BETWEEN:
					break;
                case ARITH_PIXEL_OPERATION.E_ARITH_PIXEL_ABOVE:
					break;
            }
          
            return true;
		}

		internal bool Arith_Projection(CogImage8Grey mSrcID, BUFF mDstID, ARITH_PROJECT_DIR eDir)
		{
			

			return true;
		}

	}
}
