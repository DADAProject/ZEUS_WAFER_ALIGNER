using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.SearchMax;

namespace Drv.ImageProcess.Core
{
    internal partial class PatternMatching
    {
		internal bool Pattern_Matching_ALL(CogImage8Grey SrcImg, CogImage8Grey DstImg, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
								int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
								out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
		{
			dCenterX = 0.0; dCenterY = 0.0; dAngle = 0.0; dScore = 0.0;
			double[] pdCenter_X, pdCenter_Y, pdAngle, pdScore;
			CogImage8Grey nGoldenImg = null;
			int nWidth_G, nHeight_G, nWidth_T, nHeight_T;

			pdScore		= new double[stParam.Golden];
			pdCenter_X  = new double[stParam.Golden];
			pdCenter_Y  = new double[stParam.Golden];
			pdAngle		= new double[stParam.Golden];

			if(DstImg == null) DstImg = new CogImage8Grey(SrcImg.Width, SrcImg.Height);

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
					nGoldenImg = new CogImage8Grey();

                    using (CogImageFileTool Tool = new CogImageFileTool())
                    {
                        Tool.Operator.Open(GoldenPath, CogImageFileModeConstants.Read);
                        Tool.Run();
                        nGoldenImg = Tool.OutputImage as CogImage8Grey;
                    }

					if (nGoldenImg != null)
					{
						nWidth_G  = nGoldenImg.Width;
						nHeight_G = nGoldenImg.Height;

						// Roi Image 
						nWidth_T  = SrcImg.Width;
						nHeight_T = SrcImg.Height;

						Pattern_Matching(SrcImg, DstImg, nGoldenImg, nWidth_G, nHeight_G, nWidth_T, nHeight_T, nFind,
							nAccuracyMode, nAcceptanceSet, nAngleSet1, nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS,
							ref pdCenter_X[i], ref pdCenter_Y[i], ref pdScore[i], ref pdAngle[i], nOper);

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
					Temp     = (int)pdScore[i];
					dCenterX = pdCenter_X[i];
					dCenterY = pdCenter_Y[i];
					dAngle   = pdAngle[i];
					dScore   = pdScore[i];
				}
			}

			return true;
		}

        internal void Pattern_Matching(CogImage8Grey TargetImg, CogImage8Grey DstImg, CogImage8Grey GoldenImg,
            int nWidth_G, int nHeight_G, int nWidth_T, int nHeight_T, int nFind,
            PATTERNMATCHING_ACCURANCY_OPERATION strAccuracyMode, int nAcceptancdSet,
            PATTERNMATCHING_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
            ref double dCenterX, ref double dCenterY, ref double dScore, ref double dAngle, PATTERNMATCHING_OPERATION nOper)
        {
            int nTotalPat = 0;

            CogSearchMaxTool Match = new CogSearchMaxTool();

			Match.Pattern.TrainMode = CogSearchMaxTrainModeConstants.EvaluateDOFsAtRuntime;
            Match.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBox;
            Match.Pattern.Origin.TranslationX = GoldenImg.Width / 2.0;
            Match.Pattern.Origin.TranslationY = GoldenImg.Height / 2.0;

            Match.RunParams.Timeout = 200.0;
            Match.RunParams.TimeoutEnabled = true;
            Match.RunParams.MaximumNumberToFind = nFind;

            //====
            if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_MEDIUM)
            {
                Match.RunParams.RunAlgorithm = CogSearchMaxRunAlgorithmConstants.Standard;
            }
            else if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_LOW)
            {
                Match.RunParams.RunAlgorithm = CogSearchMaxRunAlgorithmConstants.HighAccuracy;
            }
            else
            {
                Match.RunParams.RunAlgorithm = CogSearchMaxRunAlgorithmConstants.Standard;
            }

            if (bAngleSet1 == PATTERNMATCHING_SETANGLE_OPERATION.E_PATTERNMATCHING_SETANGLE_ENABLE)
            {
                Match.RunParams.ZoneAngle.Low = nAngleSet2 * 180.0 / Math.PI;
                Match.RunParams.ZoneAngle.High = nAngleSet3 * 180.0 / Math.PI;
            }
            else
            {
                Match.RunParams.ZoneAngle.Low = -0.052359858837820572;
                Match.RunParams.ZoneAngle.High = 0.052359858837820572;
            }

            Match.RunParams.ZoneScale.Low = 0.95;
            Match.RunParams.ZoneScale.High = 1.05;

            if (0 < nAcceptancdSet)
            {
                Match.RunParams.CompressionEnabledForScore = true;
                Match.RunParams.AcceptThreshold = (double)nAcceptancdSet / 100;
            }

            Match.Pattern.TrainImage = GoldenImg;
            Match.Pattern.Train();

            Match.InputImage = TargetImg;
            Match.Run();

            nTotalPat = (int)Match.Results.Count;
            int iFindIdx = -1;

            CogSearchMaxResults MatchData = Match.Results;

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
                    CogTransform2DLinear Pose = MatchData[i].GetPose();

                    pdposX[i] = Pose.TranslationX;
                    pdposY[i] = Pose.TranslationY;
                    pdangle[i] = Pose.Rotation * 180.0 / Math.PI;
                    pdscore[i] = MatchData[i].Score;
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
                        iFindIdx = i;
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
            }
            else
            {
                dCenterX = 0;
                dCenterY = 0;
                dScore = 0;
                dAngle = 0;
            }

            if (nOper == PATTERNMATCHING_OPERATION.E_PATTERNMATCHING_DST_IMG)
            {
                //등록된 마크 크기만큼 이미지 카피
            }
            else if (nOper == PATTERNMATCHING_OPERATION.E_PATTERNMATCHING_RESULT_IMG)
            {
                //결과 이미지 그려줌

            }
            else if (nOper == PATTERNMATCHING_OPERATION.E_PATTERNMATCHING_SRC_IMG)
            {
                //ICogRecord lastRunRecord = Match.CreateLastRunRecord();
            }
            else { }

            Match.Dispose();
        }

        internal bool Pattern_Matching_One(CogImage8Grey TargetImg, CogImage8Grey DstImg, CogImage8Grey GoldenImg, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION strAccuracyMode,
		int nAcceptancdSet, PATTERNMATCHING_SETANGLE_OPERATION bAngleSet1,
		int nAngleSet2, int nAngleSet3, ref double dCenterX, ref double dCenterY, ref double dScore, ref double dAngle, PATTERNMATCHING_OPERATION nOper)
		{

			Pattern_Matching(TargetImg, DstImg, GoldenImg, GoldenImg.Width, GoldenImg.Height, TargetImg.Width, TargetImg.Height, nFind, strAccuracyMode, nAcceptancdSet,
				bAngleSet1, nAngleSet2, nAngleSet3, ref dCenterX, ref dCenterY, ref dScore, ref dAngle, nOper);


			if (dScore == 0) return false;
			else             return true;
		}

	}
}
