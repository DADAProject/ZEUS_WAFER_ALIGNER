using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class PatternMatching
    {
		internal bool Pattern_Matching_ALL(MIL_ID SrcImg, MIL_ID DstImg, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
									    int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
										out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
		{
			dCenterX = 0.0; dCenterY = 0.0; dAngle = 0.0; dScore = 0.0;
			double[] pdCenter_X, pdCenter_Y, pdAngle, pdScore;
			MIL_ID nGoldenImg = MIL.M_NULL;
			int nWidth_G, nHeight_G, nWidth_T, nHeight_T;

			pdScore		= new double[stParam.Golden];
			pdCenter_X  = new double[stParam.Golden];
			pdCenter_Y  = new double[stParam.Golden];
			pdAngle		= new double[stParam.Golden];

			MIL.MbufClear(DstImg, 0);
			MIL.MgraColor(MIL.M_DEFAULT, 255);
			MIL.MgraDot(MIL.M_DEFAULT, DstImg, (MIL.MbufInquire(DstImg, MIL.M_SIZE_X, MIL.M_NULL)) / 2, (MIL.MbufInquire(DstImg, MIL.M_SIZE_Y, MIL.M_NULL)) / 2);

			for (int i = 0; i < stParam.Golden; i++)
			{
				pdCenter_X[i] = 0;
				pdCenter_Y[i] = 0;
				pdAngle[i]    = 0;
				pdScore[i]    = 0;

				string GoldenPath = stParam.MainPath + "\\" + stParam.FilePath[i];

				FileInfo info = new FileInfo(GoldenPath);
				if (info.Exists)
				{
					// Golden Image
					MIL.MbufRestore(GoldenPath, Alloc.SystemAlloc, ref nGoldenImg);

					if (nGoldenImg != MIL.M_NULL)
					{
						nWidth_G  = (int)MIL.MbufInquire(nGoldenImg, MIL.M_SIZE_X, MIL.M_NULL);
						nHeight_G = (int)MIL.MbufInquire(nGoldenImg, MIL.M_SIZE_Y, MIL.M_NULL);

						// Roi Image 
						nWidth_T  = (int)MIL.MbufInquire(SrcImg, MIL.M_SIZE_X, MIL.M_NULL);
						nHeight_T = (int)MIL.MbufInquire(SrcImg, MIL.M_SIZE_Y, MIL.M_NULL);

						Pattern_Matching(SrcImg, DstImg, nGoldenImg, nWidth_G, nHeight_G, nWidth_T, nHeight_T, nFind,
							nAccuracyMode, nAcceptanceSet, nAngleSet1, nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, ref pdCenter_X[i], ref pdCenter_Y[i], ref pdScore[i], ref pdAngle[i]);

						if (nGoldenImg != MIL.M_NULL) MIL.MbufFree(nGoldenImg);nGoldenImg = MIL.M_NULL; 

						if (nAcceptanceSet <= pdScore[i]) break;
					}
				}
			}

			int Temp = 0;

			for (int i = 0; i < stParam.Golden; i++)
			{
				if (pdScore[i] > Temp)
				{
					Temp     = (int)pdScore[i];
					dCenterX = pdCenter_X[i];
					dCenterY = pdCenter_Y[i];
					dAngle   = pdAngle[i];
					dScore   = pdScore[i];
				}
			}

			return true;
		}
		
		internal void Pattern_Matching(MIL_ID TargetImg, MIL_ID DstImg, MIL_ID GoldenImg, int nWidth_G, int nHeight_G, int nWidth_T, int nHeight_T, int nFind,
			PATTERNMATCHING_ACCURANCY_OPERATION strAccuracyMode,
			int nAcceptancdSet, PATTERNMATCHING_SETANGLE_OPERATION bAngleSet1,
			int nAngleSet2, int nAngleSet3, ref double dCenterX, ref double dCenterY, ref double dScore, ref double dAngle)
		{
			MIL_ID nmilResult = MIL.M_NULL;
			MIL_ID nmilImage  = MIL.M_NULL;

			MIL_INT nTotalPat = MIL.M_NULL;

			MIL.MpatAllocModel(Alloc.SystemAlloc, GoldenImg, 0, 0, nWidth_G, nHeight_G, MIL.M_NORMALIZED, ref nmilImage);
			MIL.MpatAllocResult(Alloc.SystemAlloc, MIL.M_DEFAULT, ref nmilResult);

			MIL.MpatSetNumber(nmilImage, nFind);
			MIL.MpatSetAcceptance(nmilImage, nAcceptancdSet);

			if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_MEDIUM)
			{
				MIL.MpatSetAccuracy(nmilImage, MIL.M_MEDIUM);
			}
			else if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_LOW)
			{
				MIL.MpatSetAccuracy(nmilImage, MIL.M_LOW);
			}
			else
			{
				MIL.MpatSetAccuracy(nmilImage, MIL.M_HIGH);
			}
			if (bAngleSet1 == PATTERNMATCHING_SETANGLE_OPERATION.E_PATTERNMATCHING_SETANGLE_ENABLE)
			{
				MIL.MpatSetAngle(nmilImage, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);
				MIL.MpatSetAngle(nmilImage, MIL.M_SEARCH_ANGLE_DELTA_NEG, nAngleSet2);
				MIL.MpatSetAngle(nmilImage, MIL.M_SEARCH_ANGLE_DELTA_POS, nAngleSet3);
				MIL.MpatSetAngle(nmilImage, MIL.M_SEARCH_ANGLE_ACCURACY, 0.25);
				MIL.MpatSetAngle(nmilImage, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_BILINEAR);
			}
			else
			{
				MIL.MpatSetAngle(nmilImage, MIL.M_SEARCH_ANGLE_MODE, MIL.M_DISABLE);
			}

			MIL.MpatPreprocModel(TargetImg, nmilImage, MIL.M_DEFAULT);
			MIL.MpatFindModel(TargetImg, nmilImage, nmilResult);

			MIL.MpatGetNumber(nmilResult, ref nTotalPat);

			if (nTotalPat > 0)
			{
				double[] pdposX, pdposY;
				double[] pdscore, pdangle;

				pdposX  = new double[nTotalPat];
				pdposY  = new double[nTotalPat];
				pdangle = new double[nTotalPat];
				pdscore = new double[nTotalPat];
				MIL.MpatGetResult(nmilResult, MIL.M_POSITION_X, pdposX);
				MIL.MpatGetResult(nmilResult, MIL.M_POSITION_Y, pdposY);
				MIL.MpatGetResult(nmilResult, MIL.M_ANGLE, pdangle);
				MIL.MpatGetResult(nmilResult, MIL.M_SCORE, pdscore);

				int Temp = 0;
				for (int i = 0; i < nTotalPat; i++)
				{
					if (pdscore[i] > Temp)
					{
						Temp		= (int)pdscore[i];
						dScore		= pdscore[i];
						dCenterX	= pdposX[i];
						dCenterY	= pdposY[i];
						dAngle		= pdangle[i];
					}
				}


				MIL.MbufClear(DstImg, 0);
				MIL_INT nMstX = (MIL_INT) (dCenterX - (nWidth_G / 2));
				MIL_INT nMstY = (MIL_INT) (dCenterY - (nHeight_G / 2));
				MIL_ID mChildSrc = MIL.M_NULL, mChildDst = MIL.M_NULL;

				if (nMstX + nWidth_G > nWidth_T)
				{
					nWidth_G = nWidth_G - ((int)nMstX + nWidth_G - nWidth_T);
				}
				if (nMstY + nHeight_G > nHeight_T)
				{
					nHeight_G = nHeight_G - ((int)nMstY + nHeight_G - nHeight_T);
				}

				MIL.MbufChild2d(TargetImg, nMstX, nMstY, nWidth_G, nHeight_G, ref mChildSrc);
				MIL.MbufChild2d(DstImg, nMstX, nMstY, nWidth_G, nHeight_G, ref mChildDst);
				MIL.MbufCopy(mChildSrc, mChildDst);

				if (mChildSrc != MIL.M_NULL) MIL.MbufFree(mChildSrc); mChildSrc = MIL.M_NULL;
				if (mChildDst != MIL.M_NULL) MIL.MbufFree(mChildDst); mChildDst = MIL.M_NULL;
			}
			else
			{
				MIL.MbufClear(DstImg, 0);
				MIL.MgraColor(MIL.M_DEFAULT, 255);
				MIL.MgraDot(MIL.M_DEFAULT, DstImg, (MIL.MbufInquire(DstImg, MIL.M_SIZE_X, MIL.M_NULL)) / 2, (MIL.MbufInquire(DstImg, MIL.M_SIZE_Y, MIL.M_NULL)) / 2);
				dCenterX = 0;
				dCenterY = 0;
				dScore = 0;
				dAngle = 0;
			}

			if (nmilImage  != MIL.M_NULL) MIL.MpatFree(nmilImage);
			if (nmilResult != MIL.M_NULL) MIL.MpatFree(nmilResult);
			nmilImage = MIL.M_NULL;
			nmilResult = MIL.M_NULL;
		}
	}
}
