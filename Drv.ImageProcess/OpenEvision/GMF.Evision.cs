using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
	internal partial class GMF
	{
		internal unsafe bool GeometrictModelFinder(EImageBW8 nSrcImg, EImageBW8 nDstImg, string cContextPath,
			GMF_SETANGLE_OPERATION bAngleSet1,int nAngleSet2, int nAngleSet3,
			out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
		{
			bool bRet = false;
			dCenter_X = 0.0; dCenter_Y = 0.0; dAngle = 0.0; dScore = 0.0;
			EImageBW8 Context = new EImageBW8();
			EPatternFinder Finder = new EPatternFinder();

			//Param
			Finder.Interpolate	  = true;
			Finder.PatternType = EPatternType.ConsistentEdges;

			if (bAngleSet1 == GMF_SETANGLE_OPERATION.E_GMF_SETANGLE_DISABLE)
			{
				Finder.AngleTolerance = 0;
				Finder.AngleBias      = 0;
			}
			else
            {
				Finder.AngleTolerance = nAngleSet2;
				Finder.AngleBias      = nAngleSet3;
			}

			Finder.ScaleTolerance   = 0;
			Finder.ScaleBias        = 100;

			Finder.ContrastMode     = EFindContrastMode.Normal;
			Finder.LocalSearchMode  = ELocalSearchMode.ExtendedMore;
			Finder.LightBalance		= 0.0f;

			//==버그 있어 테스트 필요
			Finder.ReductionMode     = EReductionMode.Manual;
			Finder.ReductionStrength = 1.0f; //100이 더 안정 적인거 같음
			//==

			Finder.MaxInstances = 1;

			Context.Load(cContextPath);

			Finder.Learn(Context); //이미지 

			if (Finder.LearningDone)
			{
				EFoundPattern[] FinderPatterns = Finder.Find(nSrcImg); // 탐색
				int nNumResults = FinderPatterns.Length;

				if (nNumResults == 0) bRet = false;
                else
                {
					double[] pdXPosition = new double[nNumResults];
					double[] pdYPosition = new double[nNumResults];
					double[] pdAngle = new double[nNumResults];
					double[] pdScore = new double[nNumResults];

					for (int i = 0; i < nNumResults; i++)
					{
						pdXPosition[i] = FinderPatterns[i].Center.X;
						pdYPosition[i] = FinderPatterns[i].Center.Y;
						pdAngle[i] = FinderPatterns[i].Angle;
						pdScore[i] = FinderPatterns[i].Score;
					}
					double dMaxScore = 0; int nMax = 0;

					for (int nIndex = 0; nIndex < nNumResults; nIndex++)
					{
						if (pdScore[nIndex] > dMaxScore)
						{
							dMaxScore = pdScore[nIndex];
							nMax = nIndex;
						}
					}

					// 가장 높은 Score의 Center값을 리턴
					dCenter_X = pdXPosition[nMax];
					dCenter_Y = pdYPosition[nMax];
					dAngle = pdAngle[nMax];
					dScore = dMaxScore;

					bRet = true;

					if (nOper == GMF_OPERATION.E_GMF_DST_IMG)
					{
						//등록된 마크 크기만큼 이미지 카피
					}
					else if (nOper == GMF_OPERATION.E_GMF_RESULT_IMG)
					{
						//결과 이미지 그려줌
						IntPtr hDC = Easy.OpenImageGraphicContext(nDstImg);
						FinderPatterns[nMax].Draw(hDC);
						Easy.CloseImageGraphicContext(nDstImg, hDC);
					}
					else if (nOper == GMF_OPERATION.E_GMF_SRC_IMG)
					{
						nSrcImg.CopyTo(nDstImg);
						IntPtr hDC = Easy.OpenImageGraphicContext(nDstImg);
						FinderPatterns[nMax].Draw(hDC);
						Easy.CloseImageGraphicContext(nDstImg, hDC);
					}
					else { }
				}	
			}
            else
            {
				dCenter_X = 0;
				dCenter_Y = 0;

				dAngle    = 0;
				dScore    = 0;
				bRet = false;
			}

			if (Finder		 != null) { Finder.Dispose();       Finder = null; }
			if (Context		 != null) { Context.Dispose();      Context = null; }

			return bRet;
		}

		internal unsafe bool GeometrictModelFinder(EImageBW8 nSrcImg, EImageBW8 nDstImg, EImageBW8 nContextImg,
		GMF_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
		out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
		{
			bool bRet = false;
			dCenter_X = 0.0; dCenter_Y = 0.0; dAngle = 0.0; dScore = 0.0;
			EPatternFinder Finder = new EPatternFinder();

			//Param
			Finder.Interpolate = true;
			Finder.PatternType = EPatternType.ThinStructure;
			if (bAngleSet1 == GMF_SETANGLE_OPERATION.E_GMF_SETANGLE_DISABLE)
			{
				Finder.AngleTolerance = 0;
				Finder.AngleBias = 0;
			}
			else
			{
				Finder.AngleTolerance = nAngleSet2;
				Finder.AngleBias	  = nAngleSet3;
			}
			Finder.ScaleTolerance = 0;
			Finder.ContrastMode = EFindContrastMode.Normal;
			Finder.LocalSearchMode = ELocalSearchMode.ExtendedMore;
			Finder.Learn(nContextImg); //이미지 

			if (Finder.LearningDone)
			{
				EFoundPattern[] FinderPatterns = Finder.Find(nSrcImg); // 탐색
				int nNumResults = FinderPatterns.Length;

				if (nNumResults == 0) bRet = false;
				else
				{
					double[] pdXPosition = new double[nNumResults];
					double[] pdYPosition = new double[nNumResults];
					double[] pdAngle = new double[nNumResults];
					double[] pdScore = new double[nNumResults];

					for (int i = 0; i < nNumResults; i++)
					{
						pdXPosition[i] = FinderPatterns[i].Center.X;
						pdYPosition[i] = FinderPatterns[i].Center.Y;
						pdAngle[i] = FinderPatterns[i].Angle;
						pdScore[i] = FinderPatterns[i].Score;
					}
					double dMaxScore = 0; int nMax = 0;

					for (int nIndex = 0; nIndex < nNumResults; nIndex++)
					{
						if (pdScore[nIndex] > dMaxScore)
						{
							dMaxScore = pdScore[nIndex];
							nMax = nIndex;
						}
					}

					// 가장 높은 Score의 Center값을 리턴
					dCenter_X = pdXPosition[nMax];
					dCenter_Y = pdYPosition[nMax];
					dAngle = pdAngle[nMax];
					dScore = dMaxScore;

					bRet = true;

					if (nOper == GMF_OPERATION.E_GMF_DST_IMG)
					{
						//등록된 마크 크기만큼 이미지 카피
					}
					else if (nOper == GMF_OPERATION.E_GMF_RESULT_IMG)
					{
						//결과 이미지 그려줌
					}
					else if (nOper == GMF_OPERATION.E_GMF_SRC_IMG)
					{
						nSrcImg.CopyTo(nDstImg);
						IntPtr hDC = Easy.OpenImageGraphicContext(nDstImg);
						FinderPatterns[nMax].Draw(hDC);
						Easy.CloseImageGraphicContext(nDstImg, hDC);
					}
					else { }
				}
			}
			else
			{
				dCenter_X = 0;
				dCenter_Y = 0;

				dAngle = 0;
				dScore = 0;
				bRet = false;
			}

			if (Finder != null) { Finder.Dispose(); Finder = null; }

			return bRet;
		}

	}
}
