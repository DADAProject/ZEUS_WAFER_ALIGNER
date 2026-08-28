using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class Edge_Funcs
    {
		internal bool EdgeDetect(MIL_ID mSrcID, MIL_ID mIntensityDstImg)
		{
			MIL.MimEdgeDetect(mSrcID, mIntensityDstImg, MIL.M_NULL, MIL.M_SOBEL, MIL.M_REGULAR_EDGE_DETECT, MIL.M_NULL);

			return true;
		}

		internal bool FindCircle(MIL_ID mSrc, MIL_ID mDst, stFittingParam stFindCircle, ref PointF stCenterpt, int nFindRadius)
		{
			bool bRetVal = false;
			//////////////////////////////////////////////////////////////////////////
			// paramter
			double dFitRadiusStd               = stFindCircle.FitRadiusStd;
			int iFitRadius_Up_OffSet_Percent   = stFindCircle.FitRadius_Up_OffSet_Percent;
			int iFitRadius_Down_OffSet_Percent = stFindCircle.FitRadius_Down_OffSet_Percent;
			int iRadiusOffSet                  = stFindCircle.RadiusOffSet;
			int iCenterOffSet                  = stFindCircle.CenterOffSet;
			//////////////////////////////////////////////////////////////////////////

			int nWidth  = (int)(MIL.MbufInquire(mSrc, MIL.M_SIZE_X, MIL.M_NULL) / 2);
			int nLength = (int)(MIL.MbufInquire(mSrc, MIL.M_SIZE_Y, MIL.M_NULL) / 2);

			//////////////////////////////////////////////////////////////////////////

			MIL_ID nmefContext = MIL.M_NULL;
			MIL_ID nmefRes = MIL.M_NULL;
			MIL_INT nNumEdge = MIL.M_NULL;

			MIL.MedgeAlloc(MIL.M_DEFAULT_HOST, MIL.M_CONTOUR, MIL.M_DEFAULT, ref nmefContext);
			MIL.MedgeAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref nmefRes);

			MIL.MedgeControl(nmefContext, MIL.M_MOMENT_ELONGATION, MIL.M_ENABLE);
			MIL.MedgeControl(nmefContext, MIL.M_CIRCLE_FIT, MIL.M_ENABLE);

			MIL.MedgeCalculate(nmefContext, mSrc, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, nmefRes, MIL.M_DEFAULT);
			MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref nNumEdge, MIL.M_NULL);

			MIL.MbufClear(mDst, 0);

			long nRadius = 0;
			int nCenX = 0; int nCenY = 0;
			if (nNumEdge > 0)
			{
				double[] pnCenX, pnCenY, pnRad, pnErr, pnCover;
				pnCenX  = new double[nNumEdge];
				pnCenY  = new double[nNumEdge];
				pnRad   = new double[nNumEdge];
				pnErr   = new double[nNumEdge];
				pnCover = new double[nNumEdge];

				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_CENTER_X, pnCenX , MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_CENTER_Y, pnCenY , MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_RADIUS  , pnRad  , MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_ERROR   , pnErr  , MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_CIRCLE_FIT_COVERAGE, pnCover, MIL.M_NULL);

				double dMinDiff = 100000f;

				for (int ni = 0; ni < nNumEdge; ni++)
				{	
					double dcal = ((dFitRadiusStd / 100));

					if (pnRad[ni]  < dFitRadiusStd + (dcal * iFitRadius_Up_OffSet_Percent  ) &&
						pnRad[ni]  > dFitRadiusStd - (dcal * iFitRadius_Down_OffSet_Percent) &&
						pnCenY[ni] < nLength + iCenterOffSet && 
						pnCenY[ni] > nLength - iCenterOffSet &&
						pnCenX[ni] < nWidth  + iCenterOffSet && 
						pnCenX[ni] > nWidth  - iCenterOffSet)

					{
						double dDiff = Math.Abs(dFitRadiusStd - pnRad[ni]);
						if (dDiff < dMinDiff)
						{
							dDiff = dMinDiff;

							nRadius = (int)(pnRad[ni] + .5f);
							nCenX   = (int)(pnCenX[ni] + .5f);
							nCenY   = (int)(pnCenY[ni] + .5f);

							bRetVal = true;
						}
					}

				}

				if (bRetVal)
				{
					stCenterpt.X = nCenX;
					stCenterpt.Y = nCenY;
					nFindRadius  = (int)nRadius;

					MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE);
					MIL.MgraArcFill(MIL.M_DEFAULT, mDst, stCenterpt.X, stCenterpt.Y, (double)(nFindRadius - iRadiusOffSet), (double)(nFindRadius - iRadiusOffSet), 0, 360);
				}
				else
				{
					stCenterpt.X = 0;
					stCenterpt.Y = 0;
					nRadius		 = -1;
				}

				if (nmefRes     != MIL.M_NULL) MIL.MedgeFree(nmefRes    ); nmefRes     = MIL.M_NULL;
				if (nmefContext != MIL.M_NULL) MIL.MedgeFree(nmefContext); nmefContext = MIL.M_NULL;

				return bRetVal;
			}
			else return false;
		}
		internal bool FindLine(MIL_ID mSrc, MIL_ID mDst, stFittingParam stFindCircle, ref PointF stStartpt, ref PointF stEndpt)
		{
			bool bRetVal = false;
			//////////////////////////////////////////////////////////////////////////
			// paramter
			double dFitRadiusStd = stFindCircle.FitRadiusStd;
			int iFitRadius_Up_OffSet_Percent = stFindCircle.FitRadius_Up_OffSet_Percent;
			int iFitRadius_Down_OffSet_Percent = stFindCircle.FitRadius_Down_OffSet_Percent;
			int iRadiusOffSet = stFindCircle.RadiusOffSet;
			int iCenterOffSet = stFindCircle.CenterOffSet;
			//////////////////////////////////////////////////////////////////////////

			int nWidth = (int)(MIL.MbufInquire(mSrc, MIL.M_SIZE_X, MIL.M_NULL) / 2);
			int nLength = (int)(MIL.MbufInquire(mSrc, MIL.M_SIZE_Y, MIL.M_NULL) / 2);

			//////////////////////////////////////////////////////////////////////////

			MIL_ID nmefContext = MIL.M_NULL;
			MIL_ID nmefRes = MIL.M_NULL;
			MIL_INT nNumEdge = MIL.M_NULL;

			MIL.MedgeAlloc(MIL.M_DEFAULT_HOST, MIL.M_CONTOUR, MIL.M_DEFAULT, ref nmefContext);
			MIL.MedgeAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, ref nmefRes);

			MIL.MedgeControl(nmefContext, MIL.M_MOMENT_ELONGATION, MIL.M_ENABLE);
			MIL.MedgeControl(nmefContext, MIL.M_LINE_FIT, MIL.M_ENABLE);

			MIL.MedgeCalculate(nmefContext, mSrc, MIL.M_NULL, MIL.M_NULL, MIL.M_NULL, nmefRes, MIL.M_DEFAULT);
			MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_NUMBER_OF_CHAINS + MIL.M_TYPE_MIL_INT, ref nNumEdge, MIL.M_NULL);

			MIL.MbufClear(mDst, 0);

			int nCenX = 0; int nCenY = 0;
			if (nNumEdge > 0)
			{
				double[] pnA, pnB, pnC, pnErr, pnCover;
				pnA = new double[nNumEdge];
				pnB = new double[nNumEdge];
				pnC = new double[nNumEdge];
				pnErr = new double[nNumEdge];
				pnCover = new double[nNumEdge];

				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_LINE_FIT_A, pnA, MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_LINE_FIT_B, pnB, MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_LINE_FIT_C, pnC, MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_LINE_FIT_ERROR, pnErr, MIL.M_NULL);
				MIL.MedgeGetResult(nmefRes, MIL.M_DEFAULT, MIL.M_LINE_FIT_COVERAGE, pnCover, MIL.M_NULL);

				double dMinDiff = 100000f;

				for (int ni = 0; ni < nNumEdge; ni++)
				{
					double dcal = ((dFitRadiusStd / 100));

					//if (pnRad[ni] < dFitRadiusStd + (dcal * iFitRadius_Up_OffSet_Percent) &&
					//	pnRad[ni] > dFitRadiusStd - (dcal * iFitRadius_Down_OffSet_Percent) &&
					//	pnCenY[ni] < nLength + iCenterOffSet &&
					//	pnCenY[ni] > nLength - iCenterOffSet &&
					//	pnCenX[ni] < nWidth + iCenterOffSet &&
					//	pnCenX[ni] > nWidth - iCenterOffSet)

					//{
					//	double dDiff = Math.Abs(dFitRadiusStd - pnRad[ni]);
					//	if (dDiff < dMinDiff)
					//	{
					//		dDiff = dMinDiff;

					//		nRadius = (int)(pnRad[ni] + .5f);
					//		nCenX = (int)(pnCenX[ni] + .5f);
					//		nCenY = (int)(pnCenY[ni] + .5f);

					//		bRetVal = true;
					//	}
					//}

				}

				if (bRetVal)
				{
					MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE);
					//MIL.MgraArcFill(MIL.M_DEFAULT, mDst, stCenterpt.X, stCenterpt.Y, (double)(nFindRadius - iRadiusOffSet), (double)(nFindRadius - iRadiusOffSet), 0, 360);
				}
				else
				{
					
					
					
				}

				if (nmefRes != MIL.M_NULL) MIL.MedgeFree(nmefRes); nmefRes = MIL.M_NULL;
				if (nmefContext != MIL.M_NULL) MIL.MedgeFree(nmefContext); nmefContext = MIL.M_NULL;

				return bRetVal;
			}
			else return false;
		}


	}
}
