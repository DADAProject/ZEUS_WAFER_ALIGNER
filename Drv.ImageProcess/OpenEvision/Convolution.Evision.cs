using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
	internal partial class Convolution
    {
		internal bool Convolve(EImageBW8 mSrcID, EImageBW8 mDstID, EDGE_OPERATION eKernelType)
		{			
			switch (eKernelType)
			{
				case EDGE_OPERATION.E_EDGE_DETECT:
					EasyImage.ConvolPrewitt(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_EDGE_DETECT2:
					EasyImage.ConvolRoberts(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_HORIZ_EDGE:
					EasyImage.ConvolGradientX(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_VERT_EDGE:
					EasyImage.ConvolGradientY(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE:
					EasyImage.ConvolLaplacian4(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE2:
					EasyImage.ConvolLaplacian8(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_SHARPEN:
					EasyImage.ConvolGaussian(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_SHARPEN2:
					EasyImage.ConvolGaussian(mSrcID, mDstID, 5, 5);
					break;
				case EDGE_OPERATION.E_SMOOTH:
					EasyImage.ConvolUniform(mSrcID, mDstID);
					break;
				case EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE  :
				case EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE:
					SobelEdgeDetect(mSrcID, mDstID, eKernelType);
					break;
				case EDGE_OPERATION.E_MEDIAN:
					EasyImage.Median(mSrcID, mDstID); //3x3
					break;
			}
		
			return true;
		}
		internal bool SobelEdgeDetect(EImageBW8 nsrcImg, EImageBW8 ndstImg, EDGE_OPERATION eKernleType)
		{
			if    (eKernleType == EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE)
				EasyImage.ConvolSobelX(nsrcImg, ndstImg);
			else//(eKernleType == EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE)
				EasyImage.ConvolSobelY(nsrcImg, ndstImg);

			return true;
		}
	}
}
