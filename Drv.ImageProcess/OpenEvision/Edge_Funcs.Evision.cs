using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using Euresys.Open_eVision_22_04;
using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class Edge_Funcs
    {
		internal bool EdgeDetect(EImageBW8 mSrcID, EImageBW8 mIntensityDstImg)
		{
			ECannyEdgeDetector Edge = new ECannyEdgeDetector();
			Edge.Apply(mSrcID, mIntensityDstImg);
			Edge.Dispose();

			return true;
		}

		internal bool FindCircle(EImageBW8 mSrc, EImageBW8 mDst, stFittingParam stFindCircle, ref PointF stCenterpt, int nFindRadius)
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

			int nWidth  = (int)(mSrc.Width / 2);
			int nLength = (int)(mSrc.Height / 2);

			//////////////////////////////////////////////////////////////////////////
			int nNumEdge = 0;
			EImageBW8 mIntensityDstImg = new EImageBW8();
			ECannyEdgeDetector Edge = new ECannyEdgeDetector();
			Edge.Apply(mSrc, mIntensityDstImg);
			Edge.Dispose();

			ECircleGauge gauge = new ECircleGauge();

			//Setting
			gauge.Angle = (float)dFitRadiusStd;

			gauge.Measure(mIntensityDstImg);
			
			nNumEdge = (int) gauge.NumSamples;
			if (mDst == null) mDst = new EImageBW8(mSrc.Width, mSrc.Height);

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

				float centerX = gauge.MeasuredCircle.CenterX;
				float centerY = gauge.MeasuredCircle.CenterY;
				float diameter = gauge.MeasuredCircle.Diameter;
				float length = gauge.MeasuredCircle.ArcLength;

				double dMinDiff = 100000f;

				for (int ni = 0; ni < nNumEdge; ni++)
				{
					double dcal = ((dFitRadiusStd / 100));

					if (pnRad[ni] < dFitRadiusStd + (dcal * iFitRadius_Up_OffSet_Percent) &&
						pnRad[ni] > dFitRadiusStd - (dcal * iFitRadius_Down_OffSet_Percent) &&
						pnCenY[ni] < nLength + iCenterOffSet &&
						pnCenY[ni] > nLength - iCenterOffSet &&
						pnCenX[ni] < nWidth + iCenterOffSet &&
						pnCenX[ni] > nWidth - iCenterOffSet)

					{
						double dDiff = Math.Abs(dFitRadiusStd - pnRad[ni]);
						if (dDiff < dMinDiff)
						{
							dDiff = dMinDiff;

							nRadius = (int)(pnRad[ni] + .5f);
							nCenX = (int)(pnCenX[ni] + .5f);
							nCenY = (int)(pnCenY[ni] + .5f);

							bRetVal = true;
						}
					}

				}

				if (bRetVal)
				{
					stCenterpt.X = nCenX;
					stCenterpt.Y = nCenY;
					nFindRadius = (int)nRadius;

					//MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE);
					//MIL.MgraArcFill(MIL.M_DEFAULT, mDst, stCenterpt.X, stCenterpt.Y, (double)(nFindRadius - iRadiusOffSet), (double)(nFindRadius - iRadiusOffSet), 0, 360);
				}
				else
				{
					stCenterpt.X = 0;
					stCenterpt.Y = 0;
					nRadius = -1;
				}

				//if (nmefRes != MIL.M_NULL) MIL.MedgeFree(nmefRes); nmefRes = MIL.M_NULL;
				//if (nmefContext != MIL.M_NULL) MIL.MedgeFree(nmefContext); nmefContext = MIL.M_NULL;

				return bRetVal;
			}
			else return false;
		}

		internal bool FindRectangle(EImageBW8 mSrc, EImageBW8 mDst, stFittingRectParam stFindCircle, ref RotatedRect stRect)
		{
			bool bRetVal = false;
			//////////////////////////////////////////////////////////////////////////
			// paramter
			double dFitWidthStd                = stFindCircle.FitWidthStd;
			double dFitHeightStd               = stFindCircle.FitHeightStd;
			double dFitAngleStd				   = stFindCircle.FitAngleStd;
			int iFitSize_Tolerance             = stFindCircle.FitSize_Tolerance;
			int iCenterOffSet_X				   = stFindCircle.CenterOffSet_X;
			int iCenterOffSet_Y				   = stFindCircle.CenterOffSet_Y;
			//////////////////////////////////////////////////////////////////////////

			int nWidth  = (int)(mSrc.Width / 2);
			int nLength = (int)(mSrc.Height / 2);

			//////////////////////////////////////////////////////////////////////////
			EImageBW8 mIntensityDstImg = new EImageBW8();
			mIntensityDstImg.SetSize(mSrc.Width, mSrc.Height);
			ECannyEdgeDetector Edge = new ECannyEdgeDetector();
			Edge.Apply(mSrc, mIntensityDstImg);
		
			ERectangleGauge gauge = new ERectangleGauge();
			gauge.TransitionIndex  = 0;
			gauge.TransitionType   = ETransitionType.BwOrWb; 
			gauge.TransitionChoice = ETransitionChoice.LargestAmplitude;

			double dcal = dFitWidthStd > dFitHeightStd ? ((dFitWidthStd / 100)) : ((dFitHeightStd / 100));
			//float fTolerance = (float)(dcal * iFitSize_Tolerance);
			// 그냥 퍼센트 넣어줌
			float fTolerance = iFitSize_Tolerance;
			//////////////////////////////////////////////////////////////////////////

			EPoint center = new EPoint(nWidth + iCenterOffSet_X, nLength + iCenterOffSet_Y);
			//자제 사이즈 에 오셋은 torr로 계산
			ERectangle rectangle	 = new ERectangle(center, (float)dFitWidthStd, (float)dFitHeightStd, 0);
			gauge.Rectangle          = rectangle;
			gauge.Angle              = (float)dFitAngleStd;
			gauge.Tolerance          = fTolerance;

			gauge.Thickness          = 1;
			gauge.Threshold          = 20;
			gauge.SamplingStep       = 1;
			gauge.FilteringThreshold = 3;
			gauge.NumFilteringPasses = 3;
			gauge.Measure(mSrc);

            EvisionExtension.SetClear(mDst, 0);

            if (gauge.MeasuredRectangle.SizeX == rectangle.SizeX &&
				gauge.MeasuredRectangle.SizeY == rectangle.SizeY)
			{
				stRect = new RotatedRect(0, 0, 0, 0, 0);
				bRetVal = false;
			}
            else
			{
				stRect = new RotatedRect(new Point2f(gauge.MeasuredRectangle.CenterX, gauge.MeasuredRectangle.CenterY),
										 new SizeF(gauge.MeasuredRectangle.SizeX, gauge.MeasuredRectangle.SizeY),
										 gauge.MeasuredRectangle.Angle  );
				bRetVal = true;
			}


			//Draw Test, (나중에 드로우 모드 만들기)
			if (bRetVal)
			{
				IntPtr hDC = Easy.OpenImageGraphicContext(mDst);
				//////Draw the rectangle gauge
				gauge.Draw(hDC);
				//// Draw the fitted rectangle in green
				gauge.Draw(hDC, EDrawingMode.Actual);
			
				Easy.CloseImageGraphicContext(mDst, hDC);
			}
			else
			{
				mSrc.CopyTo(mDst);
			}

			if(Edge  != null) Edge.Dispose();
			if(gauge != null) gauge.Dispose();

			return bRetVal;
		}




	}
}
