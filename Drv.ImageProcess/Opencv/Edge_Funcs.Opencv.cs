using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Drv.ImageProcess.Core
{
	internal partial class Edge_Funcs
    {
		internal bool EdgeDetect(Mat mSrcID, Mat mIntensityDstImg, EDGE_TYPE mType)
		{
            Mat dx = new Mat(mSrcID.Width, mSrcID.Height, mSrcID.Type());
            Mat dy = new Mat(mSrcID.Width, mSrcID.Height, mSrcID.Type());
            Cv2.Sobel(mSrcID, dx, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
            Cv2.Sobel(mSrcID, dy, MatType.CV_32F, 0, 1, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);

            if(mType == EDGE_TYPE.E_BLACK_TO_WHITE) //사용
            {
                Cv2.Threshold(dx, dx, 0, float.MaxValue - 1, ThresholdTypes.Tozero);
                Cv2.Threshold(dy, dy, 0, float.MaxValue - 1, ThresholdTypes.Tozero);

            }
            else if (mType == EDGE_TYPE.E_WHITE_TO_BLACK)
            {
    
            }


            Cv2.ConvertScaleAbs(dx, dx);
            Cv2.ConvertScaleAbs(dy, dy);

            Cv2.AddWeighted(dx, 1, dy, 1, 0, mIntensityDstImg);



            dx?.Dispose();
            dy?.Dispose();

            return true;
		}

        internal bool FindLine(Mat mSrc, Mat mDst, stFittingLineParam stFindLine, FIND_ORIENTATION eOriant, ref Line2D pLine)
        {
            bool bRetVal = false;
            //////////////////////////////////////////////////////////////////////////
            // paramter
            double dFitLengthStd = stFindLine.FitLengthStd;
            double dFitRadiusStd = stFindLine.FitAngleStd;

            double dFitRadiusToler = stFindLine.FitAngle_Tolerance;
            double dFitLengthToler = stFindLine.FitLength_Tolerance;
            int iFitLength_Up_OffSet_Percent = stFindLine.FitLength_Up_OffSet_Percent;
            int iFitLength_Down_OffSet_Percent = stFindLine.FitLength_Down_OffSet_Percent;
            int iLengthOffSet = stFindLine.LengthOffSet;
            int iCenterOffSet_X = stFindLine.CenterOffSet_X;
            int iCenterOffSet_Y = stFindLine.CenterOffSet_Y;

            //////////////////////////////////////////////////////////////////////////

            int nWidth = (int)(mSrc.Width / 2);
            int nHeight = (int)(mSrc.Height / 2);

            ////Setting    
            int iMinLineLen  = (int)(dFitLengthStd / 5);
            int iMinLineGap  = 300;  //(int)(dFitLengthStd / 2);//55;3
            int iThreshold   = 100;  //(int)(dFitLengthStd / 2);//55;34 450
                                     //remove duple
            double dLimitRad = 1.5; // 1.5; //rad
            double dLimitPix = 25;  //25 //pixel

            //////////////////////////////////////////////////////////////////////////
            int nNumEdge = 0;

            OpenCvSharp.LineSegmentPoint[] lines = Cv2.HoughLinesP(mSrc, 1, Math.PI / 180, iThreshold, iMinLineLen, iMinLineGap);

            float fStrX = 0; float fStrY = 0;
            float fEndX = 0; float fEndY = 0;
            PointF ptStr = new PointF(); 
            PointF ptEnd = new PointF();

            if (lines != null || lines.Count() != 0)
            {
                // 확률적 중복 제거 적용
                var filteredLines = RemoveDuplicatesBayesian(lines, 0.8);

                lines = filteredLines.ToArray();

                nNumEdge = lines.Count();

                PointF[] ptS, ptE;
                double[] pnX, pnY, pnVX, pnVY, pnRad;
                pnX = new double[nNumEdge];
                pnY = new double[nNumEdge];
                pnVX = new double[nNumEdge];
                pnVY = new double[nNumEdge];
                pnRad = new double[nNumEdge];
                ptS = new PointF[nNumEdge];
                ptE = new PointF[nNumEdge];

                int iCnt = 0;

                for (int ni = 0; ni < nNumEdge; ni++)
                {
                    pnRad[ni]  = Math.Abs(Math.Atan2(lines[ni].P1.Y - lines[ni].P2.Y, lines[ni].P1.X - lines[ni].P2.X) * 180.0f) / Math.PI;
                    pnX[ni]    = lines[ni].P1.X;
                    pnY[ni]    = lines[ni].P1.Y;
                    pnVX[ni]   = lines[ni].P1.X - lines[ni].P2.X;//x길이
                    pnVY[ni]   = lines[ni].P1.Y - lines[ni].P2.Y;//y길이
                    ptS[ni] = new PointF(lines[ni].P1.X, lines[ni].P1.Y);
                    ptE[ni] = new PointF(lines[ni].P2.X, lines[ni].P2.Y);

                    if (pnRad[ni] >= 90)
                        pnRad[ni] -= 90;
                }

                for (int ni = 0; ni < nNumEdge; ni++)
                {
                    double dcal = ((dFitRadiusStd / 100));
                    double dcal2 = ((dFitLengthStd / 100));

                    //pnCenY[ni] < nLength + iCenterOffSet &&
                    //    pnCenY[ni] > nLength - iCenterOffSet &&
                    //    pnCenX[ni] < nWidth + iCenterOffSet &&
                    //    pnCenX[ni] > nWidth - iCenterOffSet)

                    if (eOriant == FIND_ORIENTATION.E_HORIZONTAL)
                    {
                        if (pnRad[ni]           < (double)(dFitRadiusStd + (dcal  * dFitRadiusToler)) &&
                            pnRad[ni]           > (double)(dFitRadiusStd - (dcal  * dFitRadiusToler)) &&
                            Math.Abs(pnVX[ni])  < (double)(dFitLengthStd + (dcal2 * iFitLength_Up_OffSet_Percent)) &&
                            Math.Abs(pnVX[ni])  > (double)(dFitLengthStd - (dcal2 * iFitLength_Down_OffSet_Percent)))
                        {
                            fStrX = (float)pnVX[ni];
                            fStrY = (float)pnVY[ni];
                            fEndX = (float)pnX[ni];
                            fEndY = (float)pnY[ni];
                            ptStr = ptS[ni];
                            ptEnd = ptE[ni];

                            bRetVal = true;
                            iCnt++;
                        }
                    }
                    else
                    {
                        if (pnRad[ni]           <  (double)(dFitRadiusStd + (dcal  * dFitRadiusToler)) &&
                            pnRad[ni]           >  (double)(dFitRadiusStd - (dcal  * dFitRadiusToler)) &&
                            Math.Abs(pnVY[ni])  <  (double)(dFitLengthStd + (dcal2 * iFitLength_Up_OffSet_Percent)) &&
                            Math.Abs(pnVY[ni])  >  (double)(dFitLengthStd - (dcal2 * iFitLength_Down_OffSet_Percent)))
                        {
                            fStrX = (float)pnVX[ni];
                            fStrY = (float)pnVY[ni];
                            fEndX = (float)pnX[ni];
                            fEndY = (float)pnY[ni];
                            ptStr = ptS[ni];
                            ptEnd = ptE[ni];
                            //


                            //Mat DEST = new Mat(mSrc.Rows, mSrc.Cols,MatType.CV_8SC3);
                            //DEST = mSrc.CvtColor(ColorConversionCodes.BGR2BGRA);
                            //DEST.Line((int)ptStr.X, (int)ptStr.Y, (int)ptEnd.X, (int)ptEnd.Y, Scalar.Orange,4);
                            //DEST.ImWrite($"C:\\Works\\IMAGE\\test.bmp");

                            bRetVal = true;
                            iCnt++;
                        }
                    }
                }

                if(iCnt != 1) bRetVal = false;

                if (bRetVal)
                {
                    pLine = new Line2D(fStrX, fStrY, fEndX, fEndY, new PointF[] { ptStr, ptEnd });
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

            return bRetVal;
        }

        // 베이지안 확률을 이용한 중복 판정
        public List<OpenCvSharp.LineSegmentPoint> RemoveDuplicatesBayesian(OpenCvSharp.LineSegmentPoint[] lines, double priorDuplicateProbability = 0.1)
        {
            var lineInfos = lines.Select(line => new
            {
                Line = line,
                Angle = CalculateAngle(line),
                Length = CalculateLength(line),
                Features = ExtractFeatures(line)
            }).ToArray();

            var result = new List<OpenCvSharp.LineSegmentPoint>();
            var used = new bool[lines.Length];

            for (int i = 0; i < lineInfos.Length; i++)
            {
                if (used[i]) continue;

                var currentLine = lineInfos[i];
                var duplicates = new List<int> { i };

                for (int j = i + 1; j < lineInfos.Length; j++)
                {
                    if (used[j]) continue;

                    double posteriorProb = CalculatePosteriorProbability(
                        currentLine.Features,
                        lineInfos[j].Features,
                        priorDuplicateProbability);

                    if (posteriorProb > 0.5) // 50% 이상 확률로 중복
                    {
                        duplicates.Add(j);
                        used[j] = true;
                    }
                }

                // 중복 그룹에서 가장 긴 라인 선택
                var bestIndex = duplicates.OrderByDescending(idx => lineInfos[idx].Length).First();
                result.Add(lineInfos[bestIndex].Line);
                used[bestIndex] = true;
            }

            return result;
        }

        private double CalculatePosteriorProbability(double[] features1, double[] features2, double priorProb)
        {
            // 특성 간 유사도 계산
            double similarity = 0.0;
            for (int i = 0; i < features1.Length; i++)
            {
                double diff = Math.Abs(features1[i] - features2[i]);
                double maxVal = Math.Max(Math.Abs(features1[i]), Math.Abs(features2[i]));
                similarity += 1.0 - (maxVal > 0 ? diff / maxVal : 0);
            }
            similarity /= features1.Length;

            // 베이지안 업데이트
            double likelihood = Math.Exp(-Math.Pow(1.0 - similarity, 2) / 0.1);
            double posterior = (likelihood * priorProb) /
                              (likelihood * priorProb + (1.0 - likelihood) * (1.0 - priorProb));

            return posterior;
        }

        // 각도 계산 (0-180도 범위)
        private double CalculateAngle(OpenCvSharp.LineSegmentPoint line)
        {
            double angle = Math.Atan2(line.P2.Y - line.P1.Y, line.P2.X - line.P1.X) * 180.0 / Math.PI;
            return Math.Abs(angle);
        }

        // 라인 길이 계산
        private double CalculateLength(OpenCvSharp.LineSegmentPoint line)
        {
            double dx = line.P2.X - line.P1.X;
            double dy = line.P2.Y - line.P1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // 두 점 간 거리 계산
        private double CalculateDistance(PointF p1, PointF p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private double[] ExtractFeatures(OpenCvSharp.LineSegmentPoint line)
        {
            double angle = CalculateAngle(line);
            double length = CalculateLength(line);
            double midX = (line.P1.X + line.P2.X) / 2.0;
            double midY = (line.P1.Y + line.P2.Y) / 2.0;

            return new double[] { angle, length, midX, midY };
        }
    }
}
