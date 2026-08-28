using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using HalconDotNet;

namespace Drv.ImageProcess.Core
{
	internal partial class Convolution
    {
		internal bool Convolve(HImage mSrcID, HImage mDstID, EDGE_OPERATION eKernelType)
		{
			HObject ObjectID = null;

			int nKernelType = 0;
			
			switch (eKernelType)
			{
				case EDGE_OPERATION.E_EDGE_DETECT:
					break;
				case EDGE_OPERATION.E_EDGE_DETECT2:
					break;
				case EDGE_OPERATION.E_HORIZ_EDGE:
					break;
				case EDGE_OPERATION.E_VERT_EDGE:
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE:
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE2:
					break;
				case EDGE_OPERATION.E_SHARPEN:
					break;
				case EDGE_OPERATION.E_SHARPEN2:
					break;
				case EDGE_OPERATION.E_SMOOTH:
					break;
				case EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE   :
				case EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE :
					SobelEdgeDetect(mSrcID, mDstID, eKernelType);
					break;
				case EDGE_OPERATION.E_MEDIAN:
					HOperatorSet.MedianRect(mSrcID, out ObjectID, 3, 3);
					break;
			}

			if (ObjectID != null)
			{
				HalconExtension.HobjectToHimage(ObjectID, ref mDstID);
				ObjectID.Dispose();
				return true;
			}
			else
			{
				return false;
			}
		}
		internal bool SobelEdgeDetect(HImage nsrcImg, HImage ndstImg, EDGE_OPERATION eKernleType)
		{
			HObject ObjectID = null;

			if (eKernleType == EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE)
				HOperatorSet.SobelAmp(nsrcImg, out ObjectID, "x", 3);
			else//(eKernleType == EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE)
				HOperatorSet.SobelAmp(nsrcImg, out ObjectID, "y", 3);

			if (ObjectID != null)
			{
				HalconExtension.HobjectToHimage(ObjectID, ref ndstImg);
				ObjectID.Dispose();
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
