using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using OpenCvSharp;
using static System.Net.WebRequestMethods;

namespace Drv.ImageProcess.Core
{
    internal partial class PatternMatching
    {
        const double VISION_TOLERANCE = 0.0000001;
        const int MATCH_CANDIDATE_NUM = 5;

        internal bool Pattern_Matching_ALL(Mat SrcImg, Mat DstImg, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
                                int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
                                out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
        {
            dCenterX = 0.0; dCenterY = 0.0; dAngle = 0.0; dScore = 0.0;
            double[] pdCenter_X, pdCenter_Y, pdAngle, pdScore;
            Mat nGoldenImg = null;
            int nWidth_G, nHeight_G, nWidth_T, nHeight_T;

            pdScore = new double[stParam.Golden];
            pdCenter_X = new double[stParam.Golden];
            pdCenter_Y = new double[stParam.Golden];
            pdAngle = new double[stParam.Golden];

            if (DstImg == null) DstImg = new Mat(SrcImg.Height, SrcImg.Width, SrcImg.Type());

            for (int i = 0; i < stParam.Golden; i++)
            {
                pdCenter_X[i] = 0;
                pdCenter_Y[i] = 0;
                pdAngle[i] = 0;
                pdScore[i] = 0;

                string GoldenPath = stParam.MainPath + "\\" + stParam.FilePath[i];

                FileInfo info = new FileInfo(GoldenPath);
                if (info.Exists)
                {
                    // Golden Image
                    nGoldenImg = new Mat(GoldenPath);
                    if (nGoldenImg != null)
                    {
                        nWidth_G = nGoldenImg.Width;
                        nHeight_G = nGoldenImg.Height;

                        // Roi Image 
                        nWidth_T = SrcImg.Width;
                        nHeight_T = SrcImg.Height;

                        Pattern_Matching(SrcImg, DstImg, nGoldenImg, nWidth_G, nHeight_G, nWidth_T, nHeight_T, nFind,
                            nAccuracyMode, nAcceptanceSet, nAngleSet1, nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, ref pdCenter_X[i], ref pdCenter_Y[i], ref pdScore[i], ref pdAngle[i]);

                        if (nGoldenImg != null) nGoldenImg.Dispose(); nGoldenImg = null;

                        if (nAcceptanceSet <= pdScore[i]) break;
                    }
                }
            }

            int Temp = 0;

            for (int i = 0; i < stParam.Golden; i++)
            {
                if (pdScore[i] > Temp)
                {
                    Temp = (int)pdScore[i];
                    dCenterX = pdCenter_X[i];
                    dCenterY = pdCenter_Y[i];
                    dAngle = pdAngle[i];
                    dScore = pdScore[i];
                }
            }

            return true;
        }

        internal unsafe void Pattern_Matching(Mat TargetImg, Mat DstImg, Mat GoldenImg, int nWidth_G, int nHeight_G, int nWidth_T, int nHeight_T, int nFind,
            PATTERNMATCHING_ACCURANCY_OPERATION strAccuracyMode,
            int nAcceptancdSet, PATTERNMATCHING_SETANGLE_OPERATION bAngleSet1,
            int nAngleSet2, int nAngleSet3, ref double dCenterX, ref double dCenterY, ref double dScore, ref double dAngle)
        {
            int nTotalPat = 0;

            TemplateMatchModes modes;
            if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_MEDIUM)
            {
                modes = TemplateMatchModes.SqDiffNormed;
            }
            else if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_LOW)
            {
                modes = TemplateMatchModes.CCorrNormed;
            }
            else
            {
                modes = TemplateMatchModes.CCoeffNormed;
            }


            List<System.Drawing.Point> lpos = new List<System.Drawing.Point>();
            List<double> lscore = new List<double>(), langle = new List<double>();

            double dAngleStep = 0.025;

            if (bAngleSet1 == PATTERNMATCHING_SETANGLE_OPERATION.E_PATTERNMATCHING_SETANGLE_ENABLE)
            {
                for (double Angle = nAngleSet2; Angle <= nAngleSet3; Angle += dAngleStep)
                {
                    Mat matrix = Cv2.GetRotationMatrix2D(new OpenCvSharp.Point2f(GoldenImg.Width / 2, GoldenImg.Height / 2), Angle, 1.0);
                    Mat angle = new Mat();
                    Cv2.WarpAffine(GoldenImg, angle, matrix, new OpenCvSharp.Size(GoldenImg.Width, GoldenImg.Height), InterpolationFlags.Linear, BorderTypes.Constant, Scalar.White); ;
                    Mat Result = new Mat();
                    Cv2.MatchTemplate(TargetImg, angle, Result, modes);
                    Cv2.Normalize(Result, Result, 0, 1, NormTypes.MinMax, -1);

                    for (int y = 0; y < Result.Height; y++)
                    {
                        for (int x = 0; x < Result.Width; x++)
                        {
                            float value = Result.At<float>(y, x);

                            if (nAcceptancdSet < value * 100)
                            {
                                if (nFind <= nTotalPat) break;

                                nTotalPat++;
                                lpos.Add(new System.Drawing.Point(x + GoldenImg.Width / 2, y + GoldenImg.Height / 2));
                                lscore.Add(value * 100);
                                langle.Add(Angle);
                            }
                        }
                    }
                    Result.Dispose();
                    angle.Dispose();
                    matrix.Dispose();
                }
            }
            else
            {
                Mat Result = new Mat();
                Mat Th = new Mat();
                Cv2.MatchTemplate(TargetImg, GoldenImg, Result, modes);
                Cv2.Normalize(Result, Result, 0, 1, NormTypes.MinMax, -1);

                for (int y = 0; y < Result.Height; y++)
                {
                    for (int x = 0; x < Result.Width; x++)
                    {
                        float value = Result.At<float>(y, x);
                        if (nAcceptancdSet < value * 100)
                        {
                            if (nFind <= nTotalPat) break;

                            nTotalPat++;
                            lpos.Add(new System.Drawing.Point(x + GoldenImg.Width / 2, y + GoldenImg.Height / 2));
                            lscore.Add(value * 100);
                            langle.Add(0.0);
                        }
                    }
                }
                Result.Dispose();
            }

            if (nTotalPat > 0)
            {
                double[] pdposX, pdposY;
                double[] pdscore, pdangle;

                pdposX = new double[nTotalPat];
                pdposY = new double[nTotalPat];
                pdangle = new double[nTotalPat];
                pdscore = new double[nTotalPat];

                for (int i = 0; i < nTotalPat; i++)
                {
                    pdposX[i] = lpos[i].X;
                    pdposY[i] = lpos[i].Y;
                    pdangle[i] = langle[i];
                    pdscore[i] = lscore[i];
                }

                int Temp = 0;
                for (int i = 0; i < nTotalPat; i++)
                {
                    if (pdscore[i] > Temp)
                    {
                        Temp = (int)pdscore[i];
                        dScore = pdscore[i];
                        dCenterX = pdposX[i];
                        dCenterY = pdposY[i];
                        dAngle = pdangle[i];
                    }
                }

                int nMstX = (int)(dCenterX - (nWidth_G / 2));
                int nMstY = (int)(dCenterY - (nHeight_G / 2));

                if (nMstX + nWidth_G > nWidth_T)
                {
                    nWidth_G = nWidth_G - ((int)nMstX + nWidth_G - nWidth_T);
                }
                if (nMstY + nHeight_G > nHeight_T)
                {
                    nHeight_G = nHeight_G - ((int)nMstY + nHeight_G - nHeight_T);
                }

                OpencvExtension.MatbufChildCopy(TargetImg, nMstX, nMstY, nWidth_G, nHeight_G, DstImg);
            }
            else
            {
                dCenterX = 0;
                dCenterY = 0;
                dScore = 0;
                dAngle = 0;
            }
        }



        //==속도 각도 비율 개선

        public class MatchParameter
        {
            OpenCvSharp.Point2d pt;
            double dMatchScore;
            double dMatchAngle;
            //Mat matRotatedSrc;
            Rect rectRoi;
            double dAngleStart;
            double dAngleEnd;
            RotatedRect rectR;
            Rect rectBounding;
            bool bDelete;

            double[,] vecResult = new double[3,3];//for subpixel
	        int iMaxScoreIndex;//for subpixel
            bool bPosOnBorder;
            Point2d ptSubPixel;
            double dNewAngle;

            public MatchParameter(OpenCvSharp.Point2f ptMinMax, double dScore, double dAngle)//, Mat matRotatedSrc = Mat ())
            {
                pt = new OpenCvSharp.Point2d(ptMinMax.X, ptMinMax.Y);
                dMatchScore = dScore;
                dMatchAngle = dAngle;

                bDelete = false;
                dNewAngle = 0.0;

                bPosOnBorder = false;
            }
            MatchParameter()
            {
                double dMatchScore = 0;
                double dMatchAngle = 0;
            }
            ~MatchParameter()
            {

            }
        };

        internal unsafe void Pattern_Matching2(Mat TargetImg, Mat DstImg, Mat GoldenImg, int nWidth_G, int nHeight_G, int nWidth_T, int nHeight_T, int nFind,
         PATTERNMATCHING_ACCURANCY_OPERATION strAccuracyMode,
         int nAcceptancdSet, PATTERNMATCHING_SETANGLE_OPERATION bAngleSet1,
         int nAngleSet2, int nAngleSet3, ref double dCenterX, ref double dCenterY, ref double dScore, ref double dAngle)
        {
            int nTotalPat = 0;

            OpencvMatcher Match = new OpencvMatcher();

            

            TemplateMatchModes modes;
            if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_MEDIUM)
            {
                modes = TemplateMatchModes.SqDiffNormed;
            }
            else if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_LOW)
            {
                modes = TemplateMatchModes.CCorrNormed;
            }
            else
            {
                modes = TemplateMatchModes.CCoeffNormed;
            }

            if (bAngleSet1 == PATTERNMATCHING_SETANGLE_OPERATION.E_PATTERNMATCHING_SETANGLE_ENABLE)
            {
                Match.MinAngle = nAngleSet2;
                Match.MaxAngle = nAngleSet3;
            }
            else
            {
                Match.MinAngle = 0.0f;
                Match.MaxAngle = 0.0f;
            }


            Match.LearnPattern(GoldenImg);

            var PyrPatterns = LearnPattern(GoldenImg);

            int m_iMinReduceArea = 255;
            int iTopLayer = GetTopLayer(TargetImg, (int)Math.Sqrt((double)m_iMinReduceArea));
            Cv2.BuildOpticalFlowPyramid(TargetImg, out Mat[] PyrTargets, new OpenCvSharp.Size(m_iMinReduceArea, m_iMinReduceArea), iTopLayer, false);

            double dAngleStep = Math.Atan(2.0 / Math.Max(PyrPatterns.Last().Cols, PyrPatterns.Last().Rows)) * (180.0 / Math.PI);

            int iTopSrcW = PyrTargets.Last().Cols, iTopSrcH = PyrTargets.Last().Rows;
            OpenCvSharp.Point2f ptCenter = new OpenCvSharp.Point2f((iTopSrcW -1) / 2.0f, (iTopSrcH - 1) / 2.0f);

            //Caculate lowest score at every layer
            List<double> LayerScores = new List<double>(new double[iTopLayer + 1]);
            LayerScores[0] = (double)(nAcceptancdSet / 100.0);

            for (int iLayer = 1; iLayer <= iTopLayer; iLayer++)
                LayerScores[iLayer] = LayerScores[iLayer - 1] * 0.9;

            OpenCvSharp.Size sizePat        = PyrPatterns.Last().Size();
            bool bCalMaxByBlock = (PyrTargets.Last().Width * PyrTargets.Last().Height / sizePat.Width * sizePat.Height > 500) && nFind > 10;

            List<MatchParameter> MatchParams = new List<MatchParameter>();

            if (bAngleSet1 == PATTERNMATCHING_SETANGLE_OPERATION.E_PATTERNMATCHING_SETANGLE_ENABLE)
            {
                for (double Angle = nAngleSet2; Angle <= nAngleSet3; Angle += dAngleStep)
                {
                    Mat matrix = Cv2.GetRotationMatrix2D(ptCenter, Angle, 1.0);
                    Mat matRotated = new Mat();

                    OpenCvSharp.Size sizeBest = GetBestRotationSize(PyrTargets.Last().Size(), PyrPatterns.Last().Size(), Angle);

                    float fTranslationX = (sizeBest.Width  - 1) / 2.0f - ptCenter.X;
                    float fTranslationY = (sizeBest.Height - 1) / 2.0f - ptCenter.Y;
                    matrix.At<double>(0, 2) += fTranslationX;
                    matrix.At<double>(1, 2) += fTranslationY;

                    Cv2.WarpAffine(PyrTargets.Last(), matRotated, matrix, sizeBest, InterpolationFlags.Linear, BorderTypes.Constant, Scalar.Black);

                    Mat matResult = new Mat();
                    try
                    {
                        Cv2.MatchTemplate(matRotated, PyrPatterns.Last(), matResult, modes);
                    }
                    catch (Exception)
                    {
                        break;
                    }

                    //수집
                    if (bCalMaxByBlock)
                    {
                       // s_BlockMax blockMax(matResult, pTemplData->vecPyramid[iTopLayer].size ());
                       // blockMax.GetMaxValueLoc(dMaxVal, ptMaxLoc);
                       // if (dMaxVal < vecLayerScore[iTopLayer])
                       //     continue;
                       // vecMatchParameter.push_back(s_MatchParameter(Point2f(ptMaxLoc.x - fTranslationX, ptMaxLoc.y - fTranslationY), dMaxVal, vecAngles[i]));
                       // for (int j = 0; j < m_iMaxPos + MATCH_CANDIDATE_NUM - 1; j++)
                       // {
                       //     ptMaxLoc = GetNextMaxLoc(matResult, ptMaxLoc, pTemplData->vecPyramid[iTopLayer].size(), dValue, m_dMaxOverlap, blockMax);
                       //     if (dMaxVal < LayerScores[iTopLayer])
                       //         continue;
                       //     vecMatchParameter.push_back(s_MatchParameter(Point2f(ptMaxLoc.x - fTranslationX, ptMaxLoc.y - fTranslationY), dValue, vecAngles[i]));
                       // }
                    }
                    else
                    {
                        Cv2.MinMaxLoc(matResult, out double dMaxVal, out double dMaxLoc, out OpenCvSharp.Point ptMaxVal, out OpenCvSharp.Point ptMaxLoc);
                        if (dMaxVal < LayerScores[iTopLayer])
                            continue;

                        MatchParams.Add(new MatchParameter(new OpenCvSharp.Point2f(ptMaxLoc.X - fTranslationX, ptMaxLoc.Y - fTranslationY), dMaxVal, Angle));

                        for (int j = 0; j < nFind + MATCH_CANDIDATE_NUM - 1; j++)
                        {
                            ptMaxLoc = GetNextMaxLoc(matResult, ptMaxLoc, PyrPatterns.Last().Size(), out double dValue, 0);
                            if (dMaxVal < LayerScores[iTopLayer])
                                continue;
                            MatchParams.Add(new MatchParameter(new OpenCvSharp.Point2f(ptMaxLoc.X - fTranslationX, ptMaxLoc.Y - fTranslationY), dValue, Angle));
                        }
                    }


                    matResult.Dispose();
                    matRotated.Dispose();
                    matrix.Dispose();
                }


            }
            else
            {
              
            }

            if (nTotalPat > 0)
            {
                double[] pdposX, pdposY;
                double[] pdscore, pdangle;

                pdposX = new double[nTotalPat];
                pdposY = new double[nTotalPat];
                pdangle = new double[nTotalPat];
                pdscore = new double[nTotalPat];

                //for (int i = 0; i < nTotalPat; i++)
                //{
                //    pdposX[i] = lpos[i].X;
                //    pdposY[i] = lpos[i].Y;
                //    pdangle[i] = langle[i];
                //    pdscore[i] = lscore[i];
                //}

                int Temp = 0;
                for (int i = 0; i < nTotalPat; i++)
                {
                    if (pdscore[i] > Temp)
                    {
                        Temp = (int)pdscore[i];
                        dScore = pdscore[i];
                        dCenterX = pdposX[i];
                        dCenterY = pdposY[i];
                        dAngle = pdangle[i];
                    }
                }

                int nMstX = (int)(dCenterX - (nWidth_G / 2));
                int nMstY = (int)(dCenterY - (nHeight_G / 2));

                if (nMstX + nWidth_G > nWidth_T)
                {
                    nWidth_G = nWidth_G - ((int)nMstX + nWidth_G - nWidth_T);
                }
                if (nMstY + nHeight_G > nHeight_T)
                {
                    nHeight_G = nHeight_G - ((int)nMstY + nHeight_G - nHeight_T);
                }

                OpencvExtension.MatbufChildCopy(TargetImg, nMstX, nMstY, nWidth_G, nHeight_G, DstImg);
            }
            else
            {
                dCenterX = 0;
                dCenterY = 0;
                dScore = 0;
                dAngle = 0;
            }


            Match?.Dispose();
        }


        Mat[] LearnPattern(Mat GoldenImg)
        {
            int m_iMinReduceArea = 255;
            int iTopLayer = GetTopLayer(GoldenImg, (int)Math.Sqrt((double)m_iMinReduceArea));
            Cv2.BuildOpticalFlowPyramid(GoldenImg, out Mat[] PyramidImg, new OpenCvSharp.Size(m_iMinReduceArea, m_iMinReduceArea), iTopLayer, false);

            //for (int i = 0; i < iSize; i++)
            //{
            //    double invArea = 1. / ((double)templData->vecPyramid[i].rows * templData->vecPyramid[i].cols);
            //    Scalar templMean, templSdv;
            //    double templNorm = 0, templSum2 = 0;

            //    meanStdDev(templData->vecPyramid[i], templMean, templSdv);
            //    templNorm = templSdv[0] * templSdv[0] + templSdv[1] * templSdv[1] + templSdv[2] * templSdv[2] + templSdv[3] * templSdv[3];

            //    if (templNorm < DBL_EPSILON)
            //    {
            //        templData->vecResultEqual1[i] = TRUE;
            //    }
            //    templSum2 = templNorm + templMean[0] * templMean[0] + templMean[1] * templMean[1] + templMean[2] * templMean[2] + templMean[3] * templMean[3];


            //    templSum2 /= invArea;
            //    templNorm = std::sqrt(templNorm);
            //    templNorm /= std::sqrt(invArea); // care of accuracy here


            //    templData->vecInvArea[i] = invArea;
            //    templData->vecTemplMean[i] = templMean;
            //    templData->vecTemplNorm[i] = templNorm;
            //}
            //templData->bIsPatternLearned = TRUE;

            return PyramidImg;
        }

        OpenCvSharp.Size GetBestRotationSize(OpenCvSharp.Size sizeSrc, OpenCvSharp.Size sizeDst, double dRAngle)
        {
            double dRAngle_radian = dRAngle * (Math.PI / 180.0);
            OpenCvSharp.Point ptLT = new OpenCvSharp.Point(0,0), ptLB = new OpenCvSharp.Point(0, sizeSrc.Height - 1), ptRB = new OpenCvSharp.Point(sizeSrc.Width - 1, sizeSrc.Height - 1), ptRT = new OpenCvSharp.Point(sizeSrc.Width - 1, 0);
            Point2f ptCenter = new Point2f((sizeSrc.Width -1) / 2.0f, (sizeSrc.Height - 1) / 2.0f);
            Point2f ptLT_R = ptRotatePt2f(new Point2f(ptLT.X, ptLT.Y), ptCenter, dRAngle_radian);
            Point2f ptLB_R = ptRotatePt2f(new Point2f(ptLB.X, ptLB.Y), ptCenter, dRAngle_radian);
            Point2f ptRB_R = ptRotatePt2f(new Point2f(ptRB.X, ptRB.Y), ptCenter, dRAngle_radian);
            Point2f ptRT_R = ptRotatePt2f(new Point2f(ptRT.X, ptRT.Y), ptCenter, dRAngle_radian);

            float fTopY    = Math.Max(Math.Max(ptLT_R.Y, ptLB_R.Y), Math.Max(ptRB_R.Y, ptRT_R.Y));
            float fBottomY = Math.Min(Math.Min(ptLT_R.Y, ptLB_R.Y), Math.Min(ptRB_R.Y, ptRT_R.Y));
            float fRightX  = Math.Max(Math.Max(ptLT_R.X, ptLB_R.X), Math.Max(ptRB_R.X, ptRT_R.X));
            float fLeftX   = Math.Min(Math.Min(ptLT_R.X, ptLB_R.X), Math.Min(ptRB_R.X, ptRT_R.X));

            if (dRAngle > 360)
                dRAngle -= 360;
            else if (dRAngle < 0)
                dRAngle += 360;

            if (Math.Abs(Math.Abs(dRAngle) - 90) < VISION_TOLERANCE || Math.Abs(Math.Abs(dRAngle) - 270) < VISION_TOLERANCE)
            {
                return new OpenCvSharp.Size(sizeSrc.Height, sizeSrc.Width);
            }
            else if (Math.Abs(dRAngle) < VISION_TOLERANCE || Math.Abs(Math.Abs(dRAngle) - 180) < VISION_TOLERANCE)
            {
                return sizeSrc;
            }

            double dAngle = dRAngle;

            if (dAngle > 0 && dAngle < 90)
            {
            }
            else if (dAngle > 90 && dAngle < 180)
            {
                dAngle -= 90;
            }
            else if (dAngle > 180 && dAngle < 270)
            {
                dAngle -= 180;
            }
            else if (dAngle > 270 && dAngle < 360)
            {
                dAngle -= 270;
            }
            else//Debug
            {
               // throw;
            }

            float fH1 = (float)(sizeDst.Width  * Math.Sin(dAngle * (Math.PI / 180.0)) * Math.Cos(dAngle * (Math.PI / 180.0)));
            float fH2 = (float)(sizeDst.Height * Math.Sin(dAngle * (Math.PI / 180.0)) * Math.Cos(dAngle * (Math.PI / 180.0)));

            int iHalfHeight = (int)Math.Ceiling(fTopY   - ptCenter.Y - fH1);
            int iHalfWidth  = (int)Math.Ceiling(fRightX - ptCenter.X - fH2);

            OpenCvSharp.Size sizeRet = new OpenCvSharp.Size(iHalfWidth * 2, iHalfHeight * 2);

            bool bWrongSize = (sizeDst.Width < sizeRet.Width && sizeDst.Height > sizeRet.Height)
                || (sizeDst.Width > sizeRet.Width && sizeDst.Height < sizeRet.Height
                    || sizeDst.Width * sizeDst.Height > sizeRet.Width * sizeRet.Height);
            if (bWrongSize)
                sizeRet = new OpenCvSharp.Size((int)(fRightX - fLeftX + 0.5), (int)(fTopY - fBottomY + 0.5));

            return sizeRet;
        }

        Point2f ptRotatePt2f(Point2f ptInput, Point2f ptOrg, double dAngle)
        {
            double dWidth = ptOrg.X * 2;
            double dHeight = ptOrg.Y * 2;
            double dY1 = dHeight - ptInput.Y, dY2 = dHeight - ptOrg.Y;

            double dX = (ptInput.X - ptOrg.X) * Math.Cos(dAngle) - (dY1 - ptOrg.Y) * Math.Sin(dAngle) + ptOrg.X;
            double dY = (ptInput.X - ptOrg.X) * Math.Sin(dAngle) + (dY1 - ptOrg.Y) * Math.Cos(dAngle) + dY2;

            dY = -dY + dHeight;
            return new Point2f((float)dX, (float)dY);
        }
        int GetTopLayer(Mat matTempl, int iMinDstLength)
        {
            int iTopLayer = 0;
            int iMinReduceArea = iMinDstLength * iMinDstLength;
            int iArea = matTempl.Width * matTempl.Height;
            while (iArea > iMinReduceArea)
            {
                iArea /= 4;
                iTopLayer++;
            }
            return iTopLayer;
        }

        OpenCvSharp.Point GetNextMaxLoc(Mat matResult, OpenCvSharp.Point ptMaxLoc, OpenCvSharp.Size sizeTemplate,out double dMaxValue, double dMaxOverlap)
        {
        
            int iStartX = (int)(ptMaxLoc.X - sizeTemplate.Width  * (1 - dMaxOverlap));
            int iStartY = (int)(ptMaxLoc.Y - sizeTemplate.Height * (1 - dMaxOverlap));

           // rectangle(matResult, new Rect(iStartX, iStartY, 2 * sizeTemplate.Width * (1 - dMaxOverlap), 2 * sizeTemplate.Height * (1 - dMaxOverlap)), Scalar(-1), CV_FILLED);

            Cv2.MinMaxLoc(matResult, out double dMaxVal, out dMaxValue, out OpenCvSharp.Point ptMaxVal, out OpenCvSharp.Point ptNewMaxLoc);

            return ptNewMaxLoc;
        }

        //Point GetNextMaxLoc(Mat matResult, Point ptMaxLoc, Size sizeTemplate, out double dMaxValue, double dMaxOverlap, s_BlockMax & blockMax)
        //{
        //    int iStartX = (int)(ptMaxLoc.X - sizeTemplate.Width * (1 - dMaxOverlap));
        //    int iStartY = (int)(ptMaxLoc.Y - sizeTemplate.Height * (1 - dMaxOverlap));
        //    Rect rectIgnore = new Rect(iStartX, iStartY, (int)(2 * sizeTemplate.Width * (1 - dMaxOverlap)), (int)(2 * sizeTemplate.Height * (1 - dMaxOverlap)));
        //    //塗黑
        //    rectangle(matResult, rectIgnore, Scalar(-1), CV_FILLED);
        //    blockMax.UpdateMax(rectIgnore);
        //    Point ptReturn;
        //    blockMax.GetMaxValueLoc(dMaxValue, ptReturn);
        //    return ptReturn;
        //}

    }

    internal class OpencvMatcher : IDisposable
    {
        public uint MinReducedArea { get; set; } = 255;
        public float MinAngle { get; set; } = -3;
        public float MaxAngle { get; set; } =  3;
      


        private bool disposedValue = false;
        private Mat[] mPyramidImg { get; set; }

        ~OpencvMatcher()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    DisposePattern();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }

        public void LearnPattern(Mat GoldenImg)
        {
            int m_iMinReduceArea = 255;
            int iTopLayer = GetTopLayer(GoldenImg, (int)Math.Sqrt((double)m_iMinReduceArea));
            Cv2.BuildOpticalFlowPyramid(GoldenImg, out Mat[] PyramidImg, new OpenCvSharp.Size(m_iMinReduceArea, m_iMinReduceArea), iTopLayer, false);

            mPyramidImg = PyramidImg;
        }

        private void DisposePattern()
        {
            if (mPyramidImg != null && mPyramidImg.Length > 0)
            {
                for (int i = 0; i < mPyramidImg.Length; i++)
                {
                    mPyramidImg[i].Dispose();
                }
            }
        }

        int GetTopLayer(Mat matTempl, int iMinDstLength)
        {
            int iTopLayer = 0;
            int iMinReduceArea = iMinDstLength * iMinDstLength;
            int iArea = matTempl.Width * matTempl.Height;
            while (iArea > iMinReduceArea)
            {
                iArea /= 4;
                iTopLayer++;
            }
            return iTopLayer;
        }


    }

}
