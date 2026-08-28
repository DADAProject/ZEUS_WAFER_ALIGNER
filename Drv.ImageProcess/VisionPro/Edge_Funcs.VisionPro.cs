using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Caliper;
using Drv.ImageProcess.Base;
using Drv.ImageProcess.Extension;

namespace Drv.ImageProcess.Core
{
	internal partial class Edge_Funcs
    { 
        //검증 필요
        //out put 확인 필요

        internal bool EdgeDetect(CogImage8Grey mSrcID, CogImage8Grey mIntensityDstImg)
		{
            using (CogSobelEdgeTool Tool = new CogSobelEdgeTool())
            {
                Tool.InputImage = mSrcID;
                Tool.Run();
                
                VisionProExtension.bufCopy((CogImage8Grey)Tool.Result.FinalMagnitudeImage, mIntensityDstImg);
            }

            return true;
		}

		internal bool FindCircle(CogImage8Grey mSrc, CogImage8Grey mDst, stFittingCircleParam stFindCircle, ref CircleF stCircle)
		{
			bool bRetVal = false;
			//////////////////////////////////////////////////////////////////////////
			// paramter
			double dFitRadiusStd               = stFindCircle.FitRadiusStd;
			int iFitRadius_Up_OffSet_Percent   = stFindCircle.FitRadius_Up_OffSet_Percent;
			int iFitRadius_Down_OffSet_Percent = stFindCircle.FitRadius_Down_OffSet_Percent;
			int iRadiusOffSet                  = stFindCircle.RadiusOffSet;
            int iCenterOffSet_X                = stFindCircle.CenterOffSet_X;
            int iCenterOffSet_Y                = stFindCircle.CenterOffSet_Y;

            //////////////////////////////////////////////////////////////////////////

            int nWidth  = (int)(mSrc.Width / 2);
			int nLength = (int)(mSrc.Height / 2);

            //////////////////////////////////////////////////////////////////////////
            int nNumEdge = 0;
            CogFindCircleTool FindCircle = new CogFindCircleTool();
            CogCircularArc Arc = new CogCircularArc();

            ////Setting
            Arc.CenterX = nWidth        + iCenterOffSet_X;
            Arc.CenterY = nLength       + iCenterOffSet_Y;
            Arc.Radius  = dFitRadiusStd + iRadiusOffSet  ;

            Arc.AngleStart = 0;
            Arc.AngleSpan  = 360 * 180.0 / Math.PI;

            int iFitRadius_Up   = (int)((dFitRadiusStd + iRadiusOffSet) * (1 + ((double)iFitRadius_Up_OffSet_Percent / 100)));
            int iFitRadius_Down = (int)((dFitRadiusStd + iRadiusOffSet) * (1 - ((double)iFitRadius_Down_OffSet_Percent / 100)));


            int iNumCalipers = 28;
            // Set up current, last run, and diagnostic records
            FindCircle.CurrentRecordEnable                               = CogFindCircleCurrentRecordConstants.InteractiveCaliperSize;
           // FindCircle.LastRunRecordEnable                               = CogFindCircleLastRunRecordConstants.FoundEdges;
           // FindCircle.LastRunRecordDiagEnable                           = CogFindCircleLastRunRecordDiagConstants.TransformedRegionPixels;

            // Set up Run Parameters
            FindCircle.RunParams.ExpectedCircularArc                     = Arc;
            FindCircle.RunParams.CaliperRunParams.FilterHalfSizeInPixels = 3;
            FindCircle.RunParams.CaliperRunParams.ContrastThreshold      = 10;
            FindCircle.RunParams.NumCalipers                             = iNumCalipers;
            FindCircle.RunParams.CaliperSearchLength                     = Math.Abs(iFitRadius_Up - iFitRadius_Down);
            FindCircle.RunParams.CaliperProjectionLength                 = Math.Abs(Arc.Radius / 10);
            FindCircle.RunParams.CaliperSearchDirection                  = CogFindCircleSearchDirectionConstants.Outward;


            //Run
            FindCircle.InputImage = mSrc;
            FindCircle.Run();

            nNumEdge = FindCircle.Results.NumPointsFound;

            float fRadius = -1;
            float fCenX   = 0; float fCenY = 0;
            List<PointF> points = new List<PointF>();

            if (nNumEdge > iNumCalipers / 2)
            {
                fCenX   = (float)FindCircle.Results.GetCircle().CenterX;
                fCenY   = (float)FindCircle.Results.GetCircle().CenterY;
                fRadius = (float)FindCircle.Results.GetCircle().Radius;

                double[] pnCenX, pnCenY, pnScore;
                pnCenX  = new double[nNumEdge];
                pnCenY  = new double[nNumEdge];
                pnScore = new double[nNumEdge];
                int iCnt = 0;

                for (int i = 0; i < FindCircle.Results.Count; i++)
                {
                    if (FindCircle.Results[i].Used)
                    {        
                        pnCenX[iCnt]  = FindCircle.Results[i].CaliperResults[0].PositionX;
                        pnCenY[iCnt]  = FindCircle.Results[i].CaliperResults[0].PositionY;
                        pnScore[iCnt] = FindCircle.Results[i].CaliperResults[0].Score;
                        iCnt++;
                    }
                }

                var DescendingScore = pnScore.OrderByDescending(x => x).ToArray();

                for (int i = 0; i < DescendingScore.Length; i++)
                {
                    var (maxValue, maxIndex) = pnScore.ToList().Select((x, idx) => (x == DescendingScore[i], idx)).Max();

                    points.Add(new PointF((float)pnCenX[maxIndex], (float)pnCenY[maxIndex]));
                }

                stCircle = new CircleF(new PointF(fCenX, fCenY), fRadius, points.ToArray());

                VisionProExtension.CgraCircleResults(mDst, FindCircle.Results);

                bRetVal = true;
            }
            else
            {
                stCircle = new CircleF(new PointF(fCenX, fCenY), fRadius);
            }

            Arc.Dispose();
            FindCircle.Dispose();

            if (bRetVal) return true;
            else        return false;
        }

        internal bool FindLine(CogImage8Grey mSrc, CogImage8Grey mDst, stFittingLineParam stFindLine, FIND_ORIENTATION eOriant, ref Line2D pLine)
        {
            bool bRetVal = false;
            //////////////////////////////////////////////////////////////////////////
            // paramter
            double dFitLengthStd               = stFindLine.FitLengthStd;
            double dFitAngleStd                = stFindLine.FitAngleStd;

            int iFitLength_Up_OffSet_Percent   = stFindLine.FitLength_Up_OffSet_Percent;
            int iFitLength_Down_OffSet_Percent = stFindLine.FitLength_Down_OffSet_Percent;
            int iLengthOffSet                  = stFindLine.LengthOffSet;
            int iCenterOffSet_X                = stFindLine.CenterOffSet_X;
            int iCenterOffSet_Y                = stFindLine.CenterOffSet_Y;

            //////////////////////////////////////////////////////////////////////////

            int nWidth  = (int)(mSrc.Width  / 2);
            int nHeight = (int)(mSrc.Height / 2);

            int iNumCalipers = 28;

            //////////////////////////////////////////////////////////////////////////
            int nNumEdge = 0;

            CogFindLineTool LineTool   = new CogFindLineTool();
            CogLineSegment Segment     = new CogLineSegment();

            ////Setting    
            ////Setting    
            double dLength = dFitLengthStd / 2;
            double dStartX = dLength * Math.Cos(Math.PI * dFitAngleStd / 180.0) * -1;
            double dStartY = dLength * Math.Sin(Math.PI * dFitAngleStd / 180.0) * -1;



            Segment.SetStartLengthRotation(nWidth + iCenterOffSet_X + dStartX,
                                           nHeight + iCenterOffSet_Y + dStartY,
                                           dFitLengthStd,
                                           Math.PI * dFitAngleStd / 180.0);


            int iFitLength_Up   = (int)((dFitLengthStd + iLengthOffSet) * (1 + ((double)iFitLength_Up_OffSet_Percent   / 100)));
            int iFitLength_Down = (int)((dFitLengthStd + iLengthOffSet) * (1 - ((double)iFitLength_Down_OffSet_Percent / 100)));

            // Set up Run Parameters
            LineTool.RunParams.ExpectedLineSegment      = Segment;
            LineTool.RunParams.NumCalipers              = iNumCalipers;
            LineTool.RunParams.DecrementNumToIgnore     = true;
            LineTool.RunParams.NumToIgnore              = 3;
            LineTool.RunParams.CaliperSearchDirection   = eOriant == FIND_ORIENTATION.E_HORIZONTAL ? 90 * 180.0 / Math.PI : -90 * 180.0 / Math.PI;
            LineTool.RunParams.CaliperSearchLength      = Math.Abs(iFitLength_Up - iFitLength_Down);
            LineTool.RunParams.CaliperProjectionLength  = Math.Abs(Segment.Length / 10);

            LineTool.InputImage = mSrc;
            LineTool.Run();


            float fStrX = 0; float fStrY = 0;
            float fEndX = 0; float fEndY = 0;

            if (LineTool.Results != null)
            {
                nNumEdge = LineTool.Results.NumPointsFound;

                List<PointF> points = new List<PointF>();

                if (nNumEdge > iNumCalipers / 2)
                {
                    fStrX = (float)LineTool.Results.GetLineSegment().StartX;
                    fStrY = (float)LineTool.Results.GetLineSegment().StartY;
                    fEndX = (float)LineTool.Results.GetLineSegment().EndX;
                    fEndY = (float)LineTool.Results.GetLineSegment().EndY;

                    double[] pnCenX, pnCenY, pnScore;
                    pnCenX = new double[nNumEdge];
                    pnCenY = new double[nNumEdge];
                    pnScore = new double[nNumEdge];

                    int iCnt = 0;
                    for (int i = 0; i < LineTool.Results.Count; i++)
                    {
                        if (LineTool.Results[i].Used)
                        {
                            pnCenX[iCnt] = LineTool.Results[i].CaliperResults[0].PositionX;
                            pnCenY[iCnt] = LineTool.Results[i].CaliperResults[0].PositionY;
                            pnScore[iCnt] = LineTool.Results[i].CaliperResults[0].Score;
                            iCnt++;
                        }
                    }

                    var DescendingScore = pnScore.OrderByDescending(x => x).ToArray();

                    for (int i = 0; i < DescendingScore.Length; i++)
                    {
                        var (maxValue, maxIndex) = pnScore.ToList().Select((x, idx) => (x == DescendingScore[i], idx)).Max();

                        points.Add(new PointF((float)pnCenX[maxIndex], (float)pnCenY[maxIndex]));
                    }

                    pLine = new Line2D(fStrX, fStrY, fEndX, fEndY, points.ToArray());

                    VisionProExtension.CgraLineResults(mDst, LineTool.Results);

                    bRetVal = true;
                }
                else
                {
                    pLine = new Line2D(fStrX, fStrY, fEndX, fEndY);
                }
            }
            else
            {
                pLine = new Line2D(fStrX, fStrY, fEndX, fEndY);
            }


            Segment.Dispose();
            LineTool.Dispose();

            return bRetVal;
        }
        internal bool FindRectangle(CogImage8Grey mSrc, CogImage8Grey mDst, stFittingRectParam stFindCircle, ref RotatedRect stRect)
        {
            bool bRetVal = false;
            //////////////////////////////////////////////////////////////////////////
            // paramter
            double dFitWidthStd    = stFindCircle.FitWidthStd;
            double dFitHeightStd   = stFindCircle.FitHeightStd;
            double dFitAngleStd    = stFindCircle.FitAngleStd;
            int iFitSize_Tolerance = stFindCircle.FitSize_Tolerance;
            int iCenterOffSet_X    = stFindCircle.CenterOffSet_X;
            int iCenterOffSet_Y    = stFindCircle.CenterOffSet_Y;
            //////////////////////////////////////////////////////////////////////////

            int nWidth  = (int)(mSrc.Width  / 2);
            int nLength = (int)(mSrc.Height / 2);

            //////////////////////////////////////////////////////////////////////////
            

            return bRetVal;
        }

    }
}
