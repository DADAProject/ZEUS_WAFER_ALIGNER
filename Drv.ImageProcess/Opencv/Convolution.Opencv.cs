using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;

namespace Drv.ImageProcess.Core
{
	internal partial class Convolution
    {
		internal bool Convolve(Mat mSrcID, Mat mDstID, EDGE_OPERATION eKernelType)
		{
			//나중에 파라미터 하나 만들어야함
			int nKernelType = 0;
			
			switch (eKernelType)
			{
				case EDGE_OPERATION.E_EDGE_DETECT:
					break;
				case EDGE_OPERATION.E_EDGE_DETECT2:
					break;
				case EDGE_OPERATION.E_HORIZ_EDGE:
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE:
					Cv2.Laplacian(mSrcID, mDstID, MatType.CV_8UC1,3);
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE2:
					break;
				case EDGE_OPERATION.E_SHARPEN:
					Cv2.BilateralFilter(mSrcID, mDstID, -1, 10, 5);
                    break;
				case EDGE_OPERATION.E_SHARPEN2:
					break;
				case EDGE_OPERATION.E_SMOOTH:
					Cv2.Blur(mSrcID, mDstID, new Size(3,3));
					break;
				case EDGE_OPERATION.E_VERT_EDGE:
					break;
				case EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE:
				case EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE:
					break;
			}

			return true;
		}
	}
}
