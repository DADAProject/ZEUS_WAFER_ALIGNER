using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
    internal partial class PatternMatching
    {
		internal bool Pattern_Matching_ALL(EImageBW8 SrcImg, EImageBW8 DstImg, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
								int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
								out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
		{
			dCenterX = 0.0; dCenterY = 0.0; dAngle = 0.0; dScore = 0.0;
			double[] pdCenter_X, pdCenter_Y, pdAngle, pdScore;
			EImageBW8 nGoldenImg = null;
			int nWidth_G, nHeight_G, nWidth_T, nHeight_T;

			pdScore		= new double[stParam.Golden];
			pdCenter_X  = new double[stParam.Golden];
			pdCenter_Y  = new double[stParam.Golden];
			pdAngle		= new double[stParam.Golden];

			if(DstImg == null) DstImg = new EImageBW8(SrcImg.Width, SrcImg.Height);

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
					nGoldenImg = new EImageBW8();
					nGoldenImg.Load(GoldenPath);
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

		internal void Pattern_Matching(EImageBW8 TargetImg, EImageBW8 DstImg, EImageBW8 GoldenImg, int nWidth_G, int nHeight_G, int nWidth_T, int nHeight_T, int nFind,
			PATTERNMATCHING_ACCURANCY_OPERATION strAccuracyMode,
			int nAcceptancdSet, PATTERNMATCHING_SETANGLE_OPERATION bAngleSet1,
			int nAngleSet2, int nAngleSet3, ref double dCenterX, ref double dCenterY, ref double dScore, ref double dAngle, PATTERNMATCHING_OPERATION nOper)
		{
			int nTotalPat = 0;

			EMatcher Match = new EMatcher();

			//====검증 필요
			Match.AdvancedLearning	= false;
			Match.MinReducedArea    = 64;
			Match.ContrastMode		= EMatchContrastMode.Normal;
			Match.CorrelationMode	= ECorrelationMode.Normalized;
			Match.Interpolate       = true;

			//====
			if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_MEDIUM)
			{
				Match.FinalReduction = 3;
			}
			else if (strAccuracyMode == PATTERNMATCHING_ACCURANCY_OPERATION.E_PATTERNMATCHING_ACCURANCY_LOW)
			{
				Match.FinalReduction = 1;
			}
			else
			{
				Match.FinalReduction = 5;
			}

			if (bAngleSet1 == PATTERNMATCHING_SETANGLE_OPERATION.E_PATTERNMATCHING_SETANGLE_ENABLE)
			{
				Match.MinAngle    = nAngleSet2;
				Match.MaxAngle    = nAngleSet3;
			}
			else
			{
				Match.MinAngle = 0.0f;
				Match.MaxAngle = 0.0f;
			}

			if (0 < nAcceptancdSet) Match.MinScore = nAcceptancdSet / 100;

			Match.LearnPattern(GoldenImg);
			Match.Match(TargetImg);
			nTotalPat = (int) Match.NumPositions;
			int iFindIdx = -1;

			if (nTotalPat > 0)
			{
				double[] pdposX, pdposY;
				double[] pdscore, pdangle;
				pdposX  = new double[nTotalPat];
				pdposY  = new double[nTotalPat];
				pdangle = new double[nTotalPat];
				pdscore = new double[nTotalPat];

				for (int i = 0; i < nTotalPat; i++)
				{
					EMatchPosition MatchData = Match.GetPosition((uint)i);
			
					pdposX[i]  = MatchData.CenterX;
					pdposY[i]  = MatchData.CenterY;
					pdangle[i] = MatchData.Angle;
					pdscore[i] = MatchData.Score;
				}

				int Temp = 0;
				for (int i = 0; i < nTotalPat; i++)
				{
					if (pdscore[i] > Temp)
					{
						Temp     = (int)pdscore[i];
						dScore   = pdscore[i];
						dCenterX = pdposX[i];
						dCenterY = pdposY[i];
						dAngle   = pdangle[i];
						iFindIdx = 0;
					}
				}

				int nMstX = (int)(dCenterX - (nWidth_G  / 2));
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
				dScore   = 0;
				dAngle   = 0;
			}

			if (nOper == PATTERNMATCHING_OPERATION.E_PATTERNMATCHING_DST_IMG)
			{
				//등록된 마크 크기만큼 이미지 카피
			}
			else if (nOper == PATTERNMATCHING_OPERATION.E_PATTERNMATCHING_RESULT_IMG)
			{
				//결과 이미지 그려줌
				IntPtr hDC = Easy.OpenImageGraphicContext(DstImg);
				if (iFindIdx > -1) Match.DrawPositionWithCurrentPen(hDC, (uint)iFindIdx);
				Easy.CloseImageGraphicContext(DstImg, hDC);
			}
			else if (nOper == PATTERNMATCHING_OPERATION.E_PATTERNMATCHING_SRC_IMG)
			{
				TargetImg.CopyTo(DstImg);
				IntPtr hDC = Easy.OpenImageGraphicContext(DstImg);
				if(iFindIdx > -1) Match.DrawPositionWithCurrentPen(hDC, (uint)iFindIdx);
				Easy.CloseImageGraphicContext(DstImg, hDC);
			}
			else { }

			Match.Dispose();
		}


		internal bool Pattern_Matching_One(EImageBW8 TargetImg, EImageBW8 DstImg, EImageBW8 GoldenImg, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION strAccuracyMode,
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
