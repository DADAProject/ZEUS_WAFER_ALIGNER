using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class Convolution
    {
		internal bool Convolve(MIL_ID mSrcID, MIL_ID mDstID, EDGE_OPERATION eKernelType)
		{
			MIL_ID nKernelType = 0;
			
			switch (eKernelType)
			{
				case EDGE_OPERATION.E_EDGE_DETECT:
					nKernelType = MIL.M_EDGE_DETECT;
					break;
				case EDGE_OPERATION.E_EDGE_DETECT2:
					nKernelType = MIL.M_EDGE_DETECT2;
					break;
				case EDGE_OPERATION.E_HORIZ_EDGE:
					nKernelType = MIL.M_HORIZ_EDGE;
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE:
					nKernelType = MIL.M_LAPLACIAN_EDGE;
					break;
				case EDGE_OPERATION.E_LAPLACIAN_EDGE2:
					nKernelType = MIL.M_LAPLACIAN_EDGE2;
					break;
				case EDGE_OPERATION.E_SHARPEN:
					nKernelType = MIL.M_SHARPEN;
					break;
				case EDGE_OPERATION.E_SHARPEN2:
					nKernelType = MIL.M_SHARPEN2;
					break;
				case EDGE_OPERATION.E_SMOOTH:
					nKernelType = MIL.M_SMOOTH;
					break;
				case EDGE_OPERATION.E_VERT_EDGE:
					nKernelType = MIL.M_VERT_EDGE;
					break;
				case EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE:
				case EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE:
					nKernelType = MIL.M_NULL;
					break;
			}

			if (nKernelType != MIL.M_NULL)
				MIL.MimConvolve(mSrcID, mDstID, nKernelType);
			else
				SobelEdgeDetect(mSrcID, mDstID, eKernelType);

			return true;
		}
		internal bool SobelEdgeDetect(MIL_ID nsrcImg, MIL_ID ndstImg, EDGE_OPERATION eKernelType)
		{
			int nROI_Height, nROI_Width;
			byte[] pbyArry, pbyArry1;
			int nThr = 0;

			nROI_Width  = Convert.ToInt32(MIL.MbufInquire(nsrcImg, MIL.M_SIZE_X, MIL.M_NULL));
			nROI_Height = Convert.ToInt32(MIL.MbufInquire(nsrcImg, MIL.M_SIZE_Y, MIL.M_NULL));

			pbyArry  = new byte[nROI_Width * nROI_Height];
			pbyArry1 = new byte[nROI_Width * nROI_Height];

			Array.Clear(pbyArry , 0, pbyArry.Length);
			Array.Clear(pbyArry1, 0, pbyArry.Length);

			MIL.MbufGet2d(nsrcImg, 0, 0, nROI_Width, nROI_Height, pbyArry);

			if (eKernelType == EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE)
			{
				for (int na = 1; na < nROI_Height - 1; na++)
				{
					for (int nb = 1; nb < nROI_Width - 1; nb++)
					{
						nThr = ((pbyArry[(na - 1) * nROI_Width + (nb + 1)] * 1) - (pbyArry[(na - 1) * nROI_Width + (nb - 1)] * 1))
							 + ((pbyArry[(na    ) * nROI_Width + (nb + 1)] * 2) - (pbyArry[(na    ) * nROI_Width + (nb - 1)] * 2))
							 + ((pbyArry[(na + 1) * nROI_Width + (nb + 1)] * 1) - (pbyArry[(na + 1) * nROI_Width + (nb - 1)] * 1));

						if (nThr >= 220) { nThr = 255; }
						if (nThr < 0) { nThr = 0; }

						pbyArry1[(na) * nROI_Width + (nb)] = (byte) nThr;
					}
				}
			}
			else
			{
				for (int na = 1; na < nROI_Height - 1; na++)
				{
					for (int nb = 1; nb < nROI_Width - 1; nb++)
					{
						nThr = (pbyArry[(na - 1) * nROI_Width + (nb - 1)] * 1) + (pbyArry[(na - 1) * nROI_Width + (nb)] * 2) + (pbyArry[(na - 1) * nROI_Width + (nb + 1)] * 1)
							- ((pbyArry[(na + 1) * nROI_Width + (nb - 1)] * 1) + (pbyArry[(na + 1) * nROI_Width + (nb)] * 2) + (pbyArry[(na + 1) * nROI_Width + (nb + 1)] * 1));

						if (nThr >= 220) { nThr = 255; }
						if (nThr < 0) { nThr = 0; }

						pbyArry1[(na) * nROI_Width + (nb)] = (byte) nThr;
					}
				}
			}
			MIL.MbufPut2d(ndstImg, 0, 0, nROI_Width, nROI_Height, pbyArry1);

			return true;
		}
	}
}
