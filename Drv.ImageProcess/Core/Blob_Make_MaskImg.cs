using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	public enum BLOB_MAKE_MASKIMG_OPERATION
	{
		E_BLOB_MAKE_MASKIMG_TOP,
		E_BLOB_MAKE_MASKIMG_BOTTOM,
		E_BLOB_MAKE_MASKIMG_LEFT,
		E_BLOB_MAKE_MASKIMG_RIGHT,

	}
	public enum BLOB_MAKE_MASKIMG_FOREGROUND_COLOR_OPERATION
	{
		E_BLOB_MAKE_MASKIMG_FOREGROUND_COLOR_BLACK,
	}
	internal class Blob_Make_MaskImg
    {
		internal bool Blob_Make_Mask(MIL_ID SrcImg, MIL_ID DstImg, BLOB_MAKE_MASKIMG_OPERATION eDirection, RectangleF rMask, BLOB_MAKE_MASKIMG_FOREGROUND_COLOR_OPERATION eForegroundColor, int nExcludeBlobSize)
		{
			MIL_ID nFeatureListId = MIL.M_NULL;
			MIL_ID nBlobResultId  = MIL.M_NULL;
			MIL_INT nTotalBlob	  = MIL_INT.MinValue;

			double[] dBlob_X_Min = null;
			double[] dBlob_X_Max = null;
			double[] dBlob_Y_Max = null;
			double[] dBlob_Y_Min = null;
			double[] dBlob_Sizes = null;

			int nWidth = 0;
			int nHeight = 0;

			nWidth = (int)MIL.MbufInquire(SrcImg, MIL.M_SIZE_X, MIL.M_NULL);
			nHeight = (int)MIL.MbufInquire(SrcImg, MIL.M_SIZE_Y, MIL.M_NULL);

			MIL.MblobAllocFeatureList(Alloc.SystemAlloc, ref nFeatureListId);

			MIL.MblobSelectFeature(nFeatureListId, MIL.M_AREA);
			MIL.MblobSelectFeature(nFeatureListId, MIL.M_BOX_X_MAX);
			MIL.MblobSelectFeature(nFeatureListId, MIL.M_BOX_Y_MAX);
			MIL.MblobSelectFeature(nFeatureListId, MIL.M_BOX_X_MIN);
			MIL.MblobSelectFeature(nFeatureListId, MIL.M_BOX_Y_MIN);

			MIL.MblobAllocResult(Alloc.SystemAlloc, ref nBlobResultId);
			MIL.MblobControl(nBlobResultId, MIL.M_LATTICE, MIL.M_4_CONNECTED);

			if (eForegroundColor != BLOB_MAKE_MASKIMG_FOREGROUND_COLOR_OPERATION.E_BLOB_MAKE_MASKIMG_FOREGROUND_COLOR_BLACK)
			{
				MIL.MblobControl(nBlobResultId, MIL.M_FOREGROUND_VALUE, MIL.M_NONZERO);
			}
			else
			{
				MIL.MblobControl(nBlobResultId, MIL.M_FOREGROUND_VALUE, MIL.M_ZERO);
			}

			MIL.MblobCalculate(SrcImg, MIL.M_NULL, nFeatureListId, nBlobResultId);
			MIL.MblobSelect(nBlobResultId, MIL.M_EXCLUDE, MIL.M_AREA, MIL.M_LESS_OR_EQUAL, nExcludeBlobSize, MIL.M_NULL);
			int nBlobCount = (int)MIL.MblobGetNumber(nBlobResultId, ref nTotalBlob);

			if (nBlobCount == 0)
			{
				// No Exist Blob
				MIL.MblobFree(nFeatureListId); nFeatureListId = MIL.M_NULL;
				MIL.MblobFree(nBlobResultId); nBlobResultId = MIL.M_NULL;
				return false;
			}
			else
			{
				dBlob_X_Min = new double[nBlobCount];
				dBlob_X_Max = new double[nBlobCount];
				dBlob_Y_Max = new double[nBlobCount];
				dBlob_Y_Min = new double[nBlobCount];
				dBlob_Sizes = new double[nBlobCount];
				MIL.MblobGetResult(nBlobResultId, MIL.M_BOX_X_MIN, dBlob_X_Min);
				MIL.MblobGetResult(nBlobResultId, MIL.M_BOX_X_MAX, dBlob_X_Max);
				MIL.MblobGetResult(nBlobResultId, MIL.M_BOX_Y_MIN, dBlob_Y_Min);
				MIL.MblobGetResult(nBlobResultId, MIL.M_BOX_Y_MAX, dBlob_Y_Max);
				MIL.MblobGetResult(nBlobResultId, MIL.M_AREA, dBlob_Sizes);

				int TemeMaxSize = 0;
				int Index = 0;

				for (int i = 0; i < nBlobCount; i++)
				{
					if (TemeMaxSize < dBlob_Sizes[i])
					{
						TemeMaxSize = (int)dBlob_Sizes[i];
						Index = i;
					}
				}
				if (eDirection == BLOB_MAKE_MASKIMG_OPERATION.E_BLOB_MAKE_MASKIMG_TOP) // Top
				{
					rMask.Y = (float)(dBlob_Y_Min[Index] - rMask.Top);

					if (rMask.Y < 0)
					{
						rMask.Y = 0;
					}
					rMask.Height = (float)(dBlob_Y_Min[Index] + rMask.Bottom);

					if (rMask.Height > nHeight)
					{
						rMask.Height = nHeight;
					}
					rMask.X = 0;
					rMask.Width = nWidth;
				}
				else if (eDirection == BLOB_MAKE_MASKIMG_OPERATION.E_BLOB_MAKE_MASKIMG_BOTTOM) // Bottom
				{
					rMask.Y = (float)(dBlob_Y_Max[Index] - rMask.Top);
					if (rMask.Y < 0)
					{
						rMask.Y = 0;
					}
					rMask.Height = (float)(dBlob_Y_Max[Index] + rMask.Bottom);
					if (rMask.Height > nHeight)
					{
						rMask.Height = nHeight;
					}
					rMask.X = 0;
					rMask.Width = nWidth;
				}
				else if (eDirection == BLOB_MAKE_MASKIMG_OPERATION.E_BLOB_MAKE_MASKIMG_LEFT) // Left
				{
					rMask.X = (float)(dBlob_X_Min[Index] - rMask.Left);
					if (rMask.X < 0)
					{
						rMask.X = 0;
					}
					rMask.Width = (float)(dBlob_X_Min[Index] + rMask.Right);
					if (rMask.Width > nWidth)
					{
						rMask.Width = nWidth;
					}
					rMask.X = 0;
					rMask.Height = nHeight;
				}
				else // Right
				{
					rMask.X = (float)(dBlob_X_Max[Index] - rMask.Left);
					if (rMask.X < 0)
					{
						rMask.X = 0;
					}
					rMask.Width = (float)(dBlob_X_Max[Index] + rMask.Right);
					if (rMask.Width > nWidth)
					{
						rMask.Width = nWidth;
					}
					rMask.X = 0;
					rMask.Height = nHeight;
				}
				MIL.MbufClear(DstImg, 255);
				MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_BLACK);

				MIL.MgraRectFill(MIL.M_DEFAULT, DstImg, rMask.Left, rMask.Top, rMask.Right, rMask.Bottom);
			}
			MIL.MblobFree(nFeatureListId); nFeatureListId = MIL.M_NULL;
			MIL.MblobFree(nBlobResultId); nBlobResultId = MIL.M_NULL;
			return true;
		}
	}
}
