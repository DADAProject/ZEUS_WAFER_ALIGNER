using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class GMF
    {
		internal bool GeometrictModelFinder(MIL_ID nSrcImg, MIL_ID nDstImg, string cContextPath,
			GMF_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
			out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
		{
			dCenter_X = 0.0; dCenter_Y = 0.0; dAngle = 0.0; dScore = 0.0;

			MIL_ID nModContext = MIL.M_NULL; // 컨텍스트
			MIL_ID nModResult  = MIL.M_NULL; // 결과버퍼

			MIL_INT nMSrcImgSizeX = MIL_INT.MinValue;
			MIL_INT nMSrcImgSizeY = MIL_INT.MinValue;
			MIL.MbufInquire(nSrcImg, MIL.M_SIZE_X, ref nMSrcImgSizeX);
			MIL.MbufInquire(nSrcImg, MIL.M_SIZE_Y, ref nMSrcImgSizeY);

			MIL.MmodRestore(cContextPath, Alloc.SystemAlloc, MIL.M_DEFAULT, ref nModContext); //컨텍스트 할당
			MIL.MmodAllocResult(Alloc.SystemAlloc, MIL.M_DEFAULT, ref nModResult);            //결과버퍼 할당

			MIL_INT nModelCnt = 0;
			MIL.MmodInquire(nModContext, MIL.M_CONTEXT, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref nModelCnt);
			MIL.MmodPreprocess(nModContext, MIL.M_DEFAULT);
			MIL.MmodFind(nModContext, nSrcImg, nModResult); // 탐색

			MIL_INT nNumResults = 0;
			MIL.MmodGetResult(nModResult, MIL.M_DEFAULT, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref nNumResults);  // 탐색된 갯수 얻기

			if (nNumResults == 0)
			{
				if (nModContext != MIL.M_NULL) MIL.MmodPreprocess(nModContext, MIL.M_RESET);
				if (nModResult  != MIL.M_NULL) MIL.MmodFree(nModResult ); nModResult  = MIL.M_NULL;
				if (nModContext != MIL.M_NULL) MIL.MmodFree(nModContext); nModContext = MIL.M_NULL;

				return false;
			}

			MIL_INT[] pnModel    = new MIL_INT[nNumResults];
			double[] pdXPosition = new double[nNumResults];
			double[] pdYPosition = new double[nNumResults];
			double[] pdAngle     = new double[nNumResults];
			double[] pdScore     = new double[nNumResults];

			MIL.MmodGetResult(nModResult, MIL.M_DEFAULT, MIL.M_INDEX + MIL.M_TYPE_MIL_INT, pnModel	  );
			MIL.MmodGetResult(nModResult, MIL.M_DEFAULT, MIL.M_POSITION_X				 , pdXPosition);
			MIL.MmodGetResult(nModResult, MIL.M_DEFAULT, MIL.M_POSITION_Y				 , pdYPosition);
			MIL.MmodGetResult(nModResult, MIL.M_DEFAULT, MIL.M_ANGLE					 , pdAngle    );
			MIL.MmodGetResult(nModResult, MIL.M_DEFAULT, MIL.M_SCORE					 , pdScore    );

			double dMaxScore = 0;
			int nMax = 0;

			for (int nIndex = 0; nIndex < nNumResults; nIndex++)
			{
				if (pdScore[nIndex] > dMaxScore)
				{
					dMaxScore = pdScore[nIndex];
					nMax = nIndex;
				}
			}

			MIL.MbufClear(nDstImg, 0);

			MIL_INT MModAllocSizeX = MIL_INT.MinValue;
			MIL_INT MModAllocSizeY = MIL_INT.MinValue;
			MIL.MmodInquire(nModContext, pnModel[nMax], MIL.M_ALLOC_SIZE_X, ref MModAllocSizeX);   // 등록된 모델의 Alloc Size X
			MIL.MmodInquire(nModContext, pnModel[nMax], MIL.M_ALLOC_SIZE_Y, ref MModAllocSizeY);   // 등록된 모델의 Alloc Size Y	

			MIL_INT nMstX;
			MIL_INT nMstY;

			nMstX = (MIL_INT)(pdXPosition[nMax] - (int)(MModAllocSizeX / 2));
			nMstY = (MIL_INT)(pdYPosition[nMax] - (int)(MModAllocSizeY / 2));

			if ( nMstX + (MIL_INT)MModAllocSizeX < nMSrcImgSizeX &&
				 nMstY + (MIL_INT)MModAllocSizeY < nMSrcImgSizeY &&
				 nMstX >= 0 &&
				 nMstY >= 0)
			{
				if (nOper == GMF_OPERATION.E_GMF_DST_IMG)
				{
					MIL_ID mChildSrc = MIL.M_NULL, mChildDst = MIL.M_NULL;
					MIL.MbufChild2d(nSrcImg, nMstX, nMstY, (MIL_INT)MModAllocSizeX, (MIL_INT)MModAllocSizeY, ref mChildSrc);
					MIL.MbufChild2d(nDstImg, nMstX, nMstY, (MIL_INT)MModAllocSizeX, (MIL_INT)MModAllocSizeY, ref mChildDst);
					MIL.MbufCopy(mChildSrc, mChildDst);

					if (mChildSrc != MIL.M_NULL) MIL.MbufFree(mChildSrc); mChildSrc = MIL.M_NULL;
					if (mChildDst != MIL.M_NULL) MIL.MbufFree(mChildDst); mChildDst = MIL.M_NULL;
				}
				else if (nOper == GMF_OPERATION.E_GMF_RESULT_IMG)
				{
					MIL_ID nmilResultEdge = MIL.M_NULL;
					MIL.MbufAlloc2d(Alloc.SystemAlloc, nMSrcImgSizeX, nMSrcImgSizeY, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC, ref nmilResultEdge);
					MIL.MbufClear(nmilResultEdge, 0);
					MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE); 
					MIL.MmodDraw(MIL.M_DEFAULT, nModResult, nmilResultEdge, MIL.M_DRAW_EDGES, MIL.M_ALL, MIL.M_DEFAULT);
					MIL.MbufCopy(nmilResultEdge, nDstImg);

					if (nmilResultEdge != MIL.M_NULL) MIL.MbufFree(nmilResultEdge);
					nmilResultEdge = MIL.M_NULL;
				}
				else
				{
					MIL_ID nmilChild = MIL.M_NULL;
					MIL.MbufChild2d(nDstImg, nMstX + 1, nMstY + 1, (MIL_INT)MModAllocSizeX, (MIL_INT)MModAllocSizeY, ref nmilChild);
					MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE); 
					MIL.MmodDraw(MIL.M_DEFAULT, nModContext, nmilChild, MIL.M_DRAW_IMAGE, pnModel[nMax], MIL.M_DEFAULT);

					if (nmilChild != MIL.M_NULL) MIL.MbufFree(nmilChild); nmilChild = MIL.M_NULL;
				}
			}

			// 가장 높은 Score의 Center값을 리턴
			dCenter_X = pdXPosition[nMax];
			dCenter_Y = pdYPosition[nMax];
			dAngle    = pdAngle[nMax];
			dScore    = dMaxScore;

			if (nModContext != MIL.M_NULL) MIL.MmodPreprocess(nModContext, MIL.M_RESET);
			if (nModResult  != MIL.M_NULL) MIL.MmodFree(nModResult ); nModResult  = MIL.M_NULL;
			if (nModContext != MIL.M_NULL) MIL.MmodFree(nModContext); nModContext = MIL.M_NULL;

			return true;
		}

		internal BUFF GeometrictModelDefine(MIL_ID nSrcImg, RectangleF ROI)
		{
			BUFF biContext = new BUFF();
			MIL_ID ContextID = MIL.M_NULL;
			MIL.MmodAlloc(Alloc.SystemAlloc, MIL.M_GEOMETRIC, MIL.M_DEFAULT, ref ContextID);
			MIL.MmodDefine(ContextID, MIL.M_IMAGE, nSrcImg, Math.Truncate(ROI.Left), Math.Truncate(ROI.Top), Math.Round(ROI.Width), Math.Round(ROI.Height));
			biContext.buffID = ContextID;
			return biContext;
		}

		internal bool GeometrictModelSave(MIL_ID nSrcImg, string sPath)
		{
			MIL.MmodSave(sPath, nSrcImg, MIL.M_DEFAULT);
			return true;
		}
	}
}
