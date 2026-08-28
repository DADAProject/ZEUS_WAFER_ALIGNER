using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class Blob
	{
		internal bool Blob_ReconstructFromSeed(MIL_ID mSrcID, MIL_ID mSeedID, MIL_ID mDstID, MORPHOLOGY_CONDITION eCondition)
		{
			MIL.MblobReconstruct(mSrcID, mSeedID, mDstID, MIL.M_RECONSTRUCT_FROM_SEED, (long)eCondition);

			return true;
		}

		internal bool Blob_FillHole(MIL_ID mSrcID, MIL_ID mDstID)
		{
			MIL.MblobReconstruct(mSrcID, MIL.M_NULL, mDstID, MIL.M_FILL_HOLES, MIL.M_8_CONNECTED);

			return true;
		}

		internal bool Blob_EraseBorder(MIL_ID mSrcID, MIL_ID mDstID)
		{
			MIL.MblobReconstruct(mSrcID, MIL.M_NULL, mDstID, MIL.M_ERASE_BORDER_BLOBS, MIL.M_8_CONNECTED);

			return true;
		}

		internal bool Blob_ConvexHull(MIL_ID mSrcID, MIL_ID mDstID, CONVEXHULL_TYPE eConvexHullType, bool bUseParallel)
		{
			//////////////////////////////////////////////////////////////////////////
			// Funtion Initialize
			BUFF biSrc = new BUFF();
			BUFF biDst = new BUFF();
			biSrc.buffID = mSrcID;
			biDst.buffID = mDstID;

			biSrc.InitBuffInfo(false, 0);
			biDst.InitBuffInfo(true, 0);

			int nInflateSize = 1;
			int nW = biSrc.wid + (nInflateSize * 2);
			int nH = biSrc.len + (nInflateSize * 2);

			MIL_ID mSrcInflateID = MIL.M_NULL;
			MIL.MbufAlloc2d(Alloc.SystemAlloc, nW, nH, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC, ref mSrcInflateID);

			MIL_ID mDstInflateID = MIL.M_NULL;
			MIL.MbufAlloc2d(Alloc.SystemAlloc, nW, nH, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC, ref mDstInflateID);


			BUFF bistDstInflate = new BUFF();
			bistDstInflate.buffID = mDstInflateID;
			bistDstInflate.InitBuffInfo(true, 0);

			BUFF bistSrcInflate = biSrc.CopyBuffer(mSrcInflateID, 0, 0, biSrc.wid, biSrc.len, nInflateSize, nInflateSize);

			MIL.MimArith(bistSrcInflate.buffID, MIL.M_NULL, bistSrcInflate.buffID, MIL.M_NOT);

			int wid = bistSrcInflate.wid, len = bistSrcInflate.len;
			//	long	pitch(stSrcInflate.pitch);

			//////////////////////////////////////////////////////////////////////////
			ContourPoints ContPnt = new ContourPoints();
			ContourPoints ConvexPt = new ContourPoints();
			Contour contour = new Contour();
			contour.ContourTrace(bistSrcInflate.buffID, ref ContPnt);

			/*int num = m_pMil.CalcBlob(Dst.buffID, Dst.buffID, TRUE, FALSE, 20);
			m_pMil.SelectBlob_MaxArea(num, Dst.buffID);*/

			int nPointNum = ContPnt.num / 2;

			int[] pnrc = new int[nPointNum + 5];
			int[] pncc = new int[nPointNum + 5];

			for (int ni = 0; ni < nPointNum; ni++)
			{
				pncc[ni] = (int)ContPnt.Points[ni * 2].X;
				pnrc[ni] = (int)ContPnt.Points[ni * 2].Y;
			}

			int nConvexPtNum = MilExtension.GetConvexHullPt(pnrc, pncc, nPointNum);
			ConvexPt.num = nConvexPtNum;

			for (int ni = 0; ni < nConvexPtNum; ni++)
			{
				ConvexPt.Points.Add(new Point2f(pncc[ni], pnrc[ni]));
			}

			PointF[] poly = new PointF[nConvexPtNum + 5];
			//	FPoint pt;
			int nmax_x, nmax_y, nmin_x, nmin_y;
			int nxx, nyy;
			//	int cnt = 0;

			nmax_x = nmax_y = 0;
			nmin_x = nmin_y = 1000;

			for (int ni = 0; ni < nConvexPtNum; ni++)
			{
				nxx = pncc[ni];
				nyy = pnrc[ni];

				if (nxx > nmax_x) nmax_x = nxx;
				if (nyy > nmax_y) nmax_y = nyy;
				if (nxx < nmin_x) nmin_x = nxx;
				if (nyy < nmin_y) nmin_y = nyy;

				poly[ni].X = (float)pncc[ni];
				poly[ni].Y = (float)pnrc[ni];
			}

			//#ifndef bUseParallel
			for (int ni = nmin_y; ni <= nmax_y; ni++)
			// #else
			// 	parallel_for(min_y, max_y, [&] (int i)
			// #endif
			{
				for (int nj = nmin_x; nj <= nmax_x; nj++)
				{
					if (bistSrcInflate.pBuff[ni * bistSrcInflate.pitch + nj] > /*240*/0)
					{
						// 				pt.x = (float)j;
						// 				pt.y = (float)i;
						int InOut = MilExtension.pnpoly(nConvexPtNum, poly, (float)nj, (float)ni);
						if (InOut > 0)
						{
							bistDstInflate.pBuff[ni * bistDstInflate.pitch + nj] = 255;
							//					cnt++;
						}
					}
				}
			}
			//#ifdef bUseParallel
			//	);
			//#endif

			if (eConvexHullType == CONVEXHULL_TYPE.E_CONVEXHULL_EXTRACT_ONLY)
			{
				biDst = bistDstInflate.CopyBuffer(mDstID, nInflateSize, nInflateSize, biDst.wid, biDst.len, 0, 0);
			}
			else if (eConvexHullType == CONVEXHULL_TYPE.E_CONVEXHULL_FILL)
			{
				biDst = bistDstInflate.CopyBuffer(mDstID, nInflateSize, nInflateSize, biDst.wid, biDst.len, 0, 0);

				MIL.MimArith(mSrcID, mDstID, mDstID, MIL.M_OR);
			}

			if (bistSrcInflate.buffID != MIL.M_NULL) MIL.MbufFree(bistSrcInflate.buffID); bistSrcInflate.buffID = MIL.M_NULL;
			if (bistDstInflate.buffID != MIL.M_NULL) MIL.MbufFree(bistDstInflate.buffID); bistDstInflate.buffID = MIL.M_NULL;

			return true;
		}

		internal bool Blob_Select_Geometry(MIL_ID mSrcID, MIL_ID mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
			BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod,
			DECISION_LIST_OPER eDecisionOperType, int nExceptionBlobNum,
			BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
		{
			bool bRetVal = true;

			if (mSrcID != mDstID)
				MIL.MbufCopy(mSrcID, mDstID);

			// [1] Init
			MIL_ID mBlobResult = MIL.M_NULL;
			MIL.MblobAllocResult(Alloc.SystemAlloc, ref mBlobResult);

			// [2] Valid Range
			CalcBlob(mDstID, MIL.M_NULL, eFeature, ref mBlobResult);
			SelectBlob(mDstID, eFeature, lMin, lMax, bMaxLimit, ref mBlobResult);
			ExtractBlob_Geometry(mDstID, eFeature, eExtractMethod, mBlobResult);

			MIL_INT nBlobNum = 0;
			MIL.MblobGetNumber(mBlobResult, ref nBlobNum);

			switch (eDecisionOperType)
			{
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_GREATER:
					if (nBlobNum > nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_GREATER_OR_EQUAL:
					if (nBlobNum >= nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_LESS:
					if (nBlobNum < nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_LESS_OR_EQUAL:
					if (nBlobNum <= nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_EQUAL:
					if (nBlobNum == nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_NOT_EQUAL:
					if (nBlobNum != nExceptionBlobNum)
						bRetVal = false;
					break;
			}

			if (bRetVal)
			{
				PostProcessing_Mask(mSrcID, mDstID, mBlobResult, eMaskType, eDstGV, nInflateSize);
			}

			if (mBlobResult != MIL.M_NULL) MIL.MblobFree(mBlobResult);
			mBlobResult = MIL.M_NULL;

			return bRetVal;
		}

		internal void CalcBlob(MIL_ID mDstID, MIL_ID mGrayID, BLOB_COMMON_FEATURE eFeature, ref MIL_ID mBlobResult)
		{
			MIL_ID mBlobFeature = MIL.M_NULL;
			MIL.MblobAllocFeatureList(Alloc.SystemAlloc, ref mBlobFeature);

			if (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X)
			{
				MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_X_MIN);
				MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_X_MAX);
			}
			else if (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_Y)
			{
				MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_Y_MIN);
				MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_Y_MAX);
			}
			else
			{
				// [2] Select Feature
				MIL_INT nOper = MIL.M_NULL;
				switch (eFeature)
				{
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA: nOper = MIL.M_AREA; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA_BOX: nOper = MIL.M_BOX_AREA; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_WIDTH: nOper = MIL.M_BREADTH; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_LENGTH: nOper = MIL.M_LENGTH; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_ELONGATION: nOper = MIL.M_ELONGATION; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MIN: nOper = MIL.M_FERET_MIN_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MAX: nOper = MIL.M_FERET_MAX_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MEAN: nOper = MIL.M_FERET_MEAN_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_ELONGATION: nOper = MIL.M_FERET_ELONGATION; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_HULL_FILL_RATIO: nOper = MIL.M_CONVEX_HULL_FILL_RATIO; break;
					case BLOB_COMMON_FEATURE.E_ROUGHNESS: nOper = MIL.M_ROUGHNESS; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_PERIMETER: nOper = MIL.M_CONVEX_PERIMETER; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_AREA: nOper = MIL.M_CONVEX_HULL_AREA; break;
					case BLOB_COMMON_FEATURE.E_COMPACTNESS: nOper = MIL.M_COMPACTNESS; break;

					default:
						return;
				}

				MIL.MblobSelectFeature(mBlobFeature, nOper);
			}

			// 기본적으로 Select 할 Feature List 
			//	-> GetBlobLabel(), GetBlobRect(), GetBlobCenterPt() 는 언제든 가능해야 하므로!!!
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_LABEL_VALUE);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_CENTER_OF_GRAVITY);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_FERET_X);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_FERET_Y);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_X_MAX);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_X_MIN);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_Y_MAX);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_BOX_Y_MIN);
			MIL.MblobSelectFeature(mBlobFeature, MIL.M_LENGTH);

			// Blob Calculate
			MIL.MblobCalculate(mDstID, mGrayID, mBlobFeature, mBlobResult);

			if (mBlobFeature != MIL.M_NULL) MIL.MblobFree(mBlobFeature); mBlobFeature = MIL.M_NULL;
		}
		internal void SelectBlob(MIL_ID mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit, ref MIL_ID mBlobResult)
		{
			if (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X ||
				eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_Y)
			{
				SelectBlob_Simple(mDstID, MIL.M_AREA, lMin, lMax, true, ref mBlobResult);

				if (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X)
					SelectBlob_Complex_BoxSize(mDstID, true, lMin, lMax, bMaxLimit, ref mBlobResult);
				else    // E_BLOB_COMMON_FEATURE_BOXSIZE_Y
					SelectBlob_Complex_BoxSize(mDstID, false, lMin, lMax, bMaxLimit, ref mBlobResult);
			}
			else
			{
				// [2] Select Feature
				MIL_INT nOper = MIL.M_NULL;
				switch (eFeature)
				{
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA: nOper = MIL.M_AREA; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA_BOX: nOper = MIL.M_BOX_AREA; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_WIDTH: nOper = MIL.M_BREADTH; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_LENGTH: nOper = MIL.M_LENGTH; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_ELONGATION: nOper = MIL.M_ELONGATION; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MIN: nOper = MIL.M_FERET_MIN_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MAX: nOper = MIL.M_FERET_MAX_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MEAN: nOper = MIL.M_FERET_MEAN_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_ELONGATION: nOper = MIL.M_FERET_ELONGATION; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_HULL_FILL_RATIO: nOper = MIL.M_CONVEX_HULL_FILL_RATIO; break;
					case BLOB_COMMON_FEATURE.E_ROUGHNESS: nOper = MIL.M_ROUGHNESS; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_PERIMETER: nOper = MIL.M_CONVEX_PERIMETER; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_AREA: nOper = MIL.M_CONVEX_HULL_AREA; break;
					case BLOB_COMMON_FEATURE.E_COMPACTNESS: nOper = MIL.M_COMPACTNESS; break;
					default:
						return;
				}

				SelectBlob_Simple(mDstID, nOper, lMin, lMax, bMaxLimit, ref mBlobResult);
			}
		}
		internal void SelectBlob_Simple(MIL_ID mDstID, MIL_INT nOper, double lMin, double lMax, bool bMaxLimit, ref MIL_ID mBlobResult)
		{
			if (bMaxLimit)
				MIL.MblobSelect(mBlobResult, MIL.M_EXCLUDE, nOper, MIL.M_LESS, lMin, MIL.M_NULL);
			else
				MIL.MblobSelect(mBlobResult, MIL.M_EXCLUDE, nOper, MIL.M_OUT_RANGE, lMin, lMax);

			MIL.MblobFill(mBlobResult, mDstID, MIL.M_EXCLUDED_BLOBS, 0);
			MIL_INT TotalBlob = 0;
			MIL.MblobGetNumber(mBlobResult, ref TotalBlob);
			double[] Temp_Length = new double[TotalBlob];
			Array.Clear(Temp_Length, 0, Temp_Length.Length);
			MIL.MblobGetResult(mBlobResult, MIL.M_LENGTH, Temp_Length);
		}
		internal void SelectBlob_Complex_BoxSize(MIL_ID mDstID, bool bSelectBoxSizeX, double lMin, double lMax, bool bMaxLimit, ref MIL_ID mBlobResult)
		{

			List<double> vDouble = new List<double>();
			List<long> vLabel = new List<long>();

			if (bSelectBoxSizeX)
				GetBlobResult_BoxSize_X(mBlobResult, ref vDouble);
			else
				GetBlobResult_BoxSize_Y(mBlobResult, ref vDouble);

			GetBlobResult_Label(mBlobResult, ref vLabel);

			if (vDouble.Count != vLabel.Count)
			{
				// Fail
				return;
			}

			MIL_INT nBblobNum = vDouble.Count;     // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				bool bUnSelectBlob = false;
				if (vDouble[nCnt] < lMin)
				{
					bUnSelectBlob = true;
				}
				else if (!bMaxLimit)
				{
					if (vDouble[nCnt] > lMax)
					{
						bUnSelectBlob = true;
					}
				}

				if (bUnSelectBlob)
				{
					UnSelectBlob_Label(mDstID, mBlobResult, vLabel[nCnt]);
				}
			}
		}
		internal void ExtractBlob_Position(MIL_ID mDstID, BLOB_SELECT_POSITION_EXTRACT_FEATURE eExtractFeature,
			BLOB_SELECT_POSITION_EXTRACT_METHOD eExtractMethod, MIL_ID mBlobResult)
		{
			// [1] Get Result Point - ExtractFeature
			List<PointF> vPoint = new List<PointF>();

			switch (eExtractFeature)
			{
				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_GRAVITY_CENTER:
					GetBlobResult_GravityCenter(mBlobResult, ref vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_FERET_CENTER:
					GetBlobResult_FeretCenter(mBlobResult, ref vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_LT:
					GetBlobResult_BOX_LT(mBlobResult, ref vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_RT:
					GetBlobResult_BOX_RT(mBlobResult, ref vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_RB:
					GetBlobResult_BOX_RB(mBlobResult, ref vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_LB:
				default:
					GetBlobResult_BOX_LB(mBlobResult, ref vPoint);
					break;

			}

			// Extract - Method

			switch (eExtractMethod)
			{
				case BLOB_SELECT_POSITION_EXTRACT_METHOD.E_BLOB_SELECT_POSITION_EXTRACT_METHOD_X_MIN:
					ExtractBlob_PointType_MinOrMax(mDstID, mBlobResult, true, true, vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_METHOD.E_BLOB_SELECT_POSITION_EXTRACT_METHOD_X_MAX:
					ExtractBlob_PointType_MinOrMax(mDstID, mBlobResult, true, false, vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_METHOD.E_BLOB_SELECT_POSITION_EXTRACT_METHOD_Y_MIN:
					ExtractBlob_PointType_MinOrMax(mDstID, mBlobResult, false, true, vPoint);
					break;

				case BLOB_SELECT_POSITION_EXTRACT_METHOD.E_BLOB_SELECT_POSITION_EXTRACT_METHOD_Y_MAX:
				default:
					ExtractBlob_PointType_MinOrMax(mDstID, mBlobResult, false, false, vPoint);
					break;

			}
		}
		internal void ExtractBlob_Geometry(MIL_ID mDstID, BLOB_COMMON_FEATURE eFeature,
			BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod, MIL_ID mBlobResult)
		{
			if (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X ||
				eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_Y)
			{
				if (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X)
					ExtractBlob_Geometry_Complex_BoxSize(mDstID, true, eExtractMethod, mBlobResult);
				else    // E_BLOB_COMMON_FEATURE_BOXSIZE_Y
					ExtractBlob_Geometry_Complex_BoxSize(mDstID, false, eExtractMethod, mBlobResult);
			}
			else
			{
				// [2] Select Feature
				MIL_INT nOper = MIL.M_NULL;
				switch (eFeature)
				{
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA: nOper = MIL.M_AREA; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA_BOX: nOper = MIL.M_BOX_AREA; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_WIDTH: nOper = MIL.M_BREADTH; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_LENGTH: nOper = MIL.M_LENGTH; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_ELONGATION: nOper = MIL.M_ELONGATION; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MIN: nOper = MIL.M_FERET_MIN_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MAX: nOper = MIL.M_FERET_MAX_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MEAN: nOper = MIL.M_FERET_MEAN_DIAMETER; break;
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_ELONGATION: nOper = MIL.M_FERET_ELONGATION; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_HULL_FILL_RATIO: nOper = MIL.M_CONVEX_HULL_FILL_RATIO; break;
					case BLOB_COMMON_FEATURE.E_ROUGHNESS: nOper = MIL.M_ROUGHNESS; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_PERIMETER: nOper = MIL.M_CONVEX_PERIMETER; break;
					case BLOB_COMMON_FEATURE.E_CONVEX_AREA: nOper = MIL.M_CONVEX_HULL_AREA; break;
					case BLOB_COMMON_FEATURE.E_COMPACTNESS: nOper = MIL.M_COMPACTNESS; break;
					default:
						return;
				}

				ExtractBlob_Geometry_Simple(mDstID, nOper, eExtractMethod, mBlobResult);
			}
		}
		internal void ExtractBlob_Geometry_Complex_BoxSize(MIL_ID mDstID, bool bSelectBoxSizeX, BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod, MIL_ID mBlobResult)
		{
			if (eExtractMethod == BLOB_SELECT_GEOMETRY_EXTRACT_METHOD.E_BLOB_SELECT_GEOMETRY_EXTRACT_METHOD_ALL)
				return; // ALL 일 경우, Filtering 하지 않는다.

			List<double> vDouble = new List<double>();

			if (bSelectBoxSizeX)
				GetBlobResult_BoxSize_X(mBlobResult, ref vDouble);
			else
				GetBlobResult_BoxSize_Y(mBlobResult, ref vDouble);


			if (eExtractMethod == BLOB_SELECT_GEOMETRY_EXTRACT_METHOD.E_BLOB_SELECT_GEOMETRY_EXTRACT_METHOD_MIN)
			{
				ExtractBlob_DoubleType_MinOrMax(mDstID, mBlobResult, true, vDouble);
			}
			else // eMethod == E_BLOB_SELECT_GEOMETRY_METHOD_MAX
			{
				ExtractBlob_DoubleType_MinOrMax(mDstID, mBlobResult, false, vDouble);
			}
		}

		internal void ExtractBlob_DoubleType_MinOrMax(MIL_ID mDstID, MIL_ID mBlobResult, bool bMin, List<double> vDoubleType)
		{
			// Blob Result가 0이면 오류발생하여 예외처리.
			if (vDoubleType.Count <= 0) return;

			double dMin = vDoubleType[0];
			double dMax = vDoubleType[0];
			int nMinIndex = 0;
			int nMaxIndex = 0;

			MIL_INT nBblobNum = vDoubleType.Count;

			for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				if (dMin > vDoubleType[nCnt])
				{
					nMinIndex = nCnt;
					dMin = vDoubleType[nCnt];
				}
				if (dMax < vDoubleType[nCnt])
				{
					nMaxIndex = nCnt;
					dMax = vDoubleType[nCnt];
				}
			}

			List<long> vLabel = new List<long>();
			GetBlobResult_Label(mBlobResult, ref vLabel);

			if (vDoubleType.Count != vLabel.Count)
			{
				// Fail
				return;
			}

			if (bMin)
			{
				MIL.MblobSelect(mBlobResult, MIL.M_EXCLUDE, MIL.M_LABEL_VALUE, MIL.M_NOT_EQUAL, vLabel[nMinIndex], MIL.M_NULL);
			}
			else
			{
				MIL.MblobSelect(mBlobResult, MIL.M_EXCLUDE, MIL.M_LABEL_VALUE, MIL.M_NOT_EQUAL, vLabel[nMaxIndex], MIL.M_NULL);
			}
			MIL.MblobFill(mBlobResult, mDstID, MIL.M_EXCLUDED_BLOBS, 0);
		}

		internal void ExtractBlob_PointType_MinOrMax(MIL_ID mDstID, MIL_ID mBlobResult, bool bX, bool bMin, List<PointF> vPointType)
		{
			// Blob Result가 0이면 오류발생하여 예외처리.
			if (vPointType.Count <= 0) return;

			double dMin = vPointType[0].Y;
			double dMax = vPointType[0].Y;
			if (bX)
			{
				dMin = vPointType[0].X;
				dMax = vPointType[0].X;
			}

			int nMinIndex = 0;
			int nMaxIndex = 0;

			MIL_INT nBblobNum = vPointType.Count();

			if (nBblobNum < 2) return; // 1개 이하는 무시

			if (bX)
			{
				for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
				{
					if (dMin > vPointType[nCnt].X)
					{
						nMinIndex = nCnt;
						dMin = vPointType[nCnt].X;
					}
					if (dMax < vPointType[nCnt].X)
					{
						nMaxIndex = nCnt;
						dMax = vPointType[nCnt].X;
					}
				}
			}
			else
			{
				for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
				{
					if (dMin > vPointType[nCnt].Y)
					{
						nMinIndex = nCnt;
						dMin = vPointType[nCnt].Y;
					}
					if (dMax < vPointType[nCnt].Y)
					{
						nMaxIndex = nCnt;
						dMax = vPointType[nCnt].Y;
					}
				}
			}

			List<long> vLabel = new List<long>();
			GetBlobResult_Label(mBlobResult, ref vLabel);

			if (vPointType.Count() != vLabel.Count())
			{
				// Fail
				return;
			}

			if (bMin)
			{
				MIL.MblobSelect(mBlobResult, MIL.M_EXCLUDE, MIL.M_LABEL_VALUE, MIL.M_NOT_EQUAL, vLabel[nMinIndex], MIL.M_NULL);
			}
			else
			{
				MIL.MblobSelect(mBlobResult, MIL.M_EXCLUDE, MIL.M_LABEL_VALUE, MIL.M_NOT_EQUAL, vLabel[nMaxIndex], MIL.M_NULL);
			}
			MIL.MblobFill(mBlobResult, mDstID, MIL.M_EXCLUDED_BLOBS, 0);
		}
		internal void ExtractBlob_Geometry_Simple(MIL_ID mDstID, MIL_INT nOper, BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod, MIL_ID mBlobResult)
		{
			if (eExtractMethod == BLOB_SELECT_GEOMETRY_EXTRACT_METHOD.E_BLOB_SELECT_GEOMETRY_EXTRACT_METHOD_ALL)
				return; // ALL 일 경우, Filtering 하지 않는다.

			List<double> vDouble = new List<double>();
			GetBlobResult_DoubleType(mBlobResult, nOper, ref vDouble);

			if (vDouble.Count <= 0)
				return; // Blob 이 존재하지 않을 경우, Return 한다.

			switch (eExtractMethod)
			{
				case BLOB_SELECT_GEOMETRY_EXTRACT_METHOD.E_BLOB_SELECT_GEOMETRY_EXTRACT_METHOD_MAX:
					ExtractBlob_DoubleType_MinOrMax(mDstID, mBlobResult, false, vDouble);
					break;
				case BLOB_SELECT_GEOMETRY_EXTRACT_METHOD.E_BLOB_SELECT_GEOMETRY_EXTRACT_METHOD_MIN:
					ExtractBlob_DoubleType_MinOrMax(mDstID, mBlobResult, true, vDouble);
					break;
				default:
					return;
			}
		}
		internal void GetBlobResult_GravityCenter(MIL_ID mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();

			MIL_INT nBblobNum = MIL_INT.MinValue;
			MIL.MblobGetNumber(mBlobResult, ref nBblobNum);

			if (nBblobNum < 1) return; // 예외처리

			double[] pdCenterX = new double[nBblobNum];
			double[] pdCenterY = new double[nBblobNum];
			MIL.MblobGetResult(mBlobResult, MIL.M_CENTER_OF_GRAVITY_X, pdCenterX);
			MIL.MblobGetResult(mBlobResult, MIL.M_CENTER_OF_GRAVITY_Y, pdCenterY);

			for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)pdCenterX[nCnt] + .5f;
				ptCenter.Y = (float)pdCenterY[nCnt] + .5f;

				vPoint.Add(ptCenter);
			}
			pdCenterX = null;
			pdCenterY = null;
		}
		internal void GetBlobResult_FeretCenter(MIL_ID mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();

			MIL_INT nBblobNum = MIL_INT.MinValue;
			MIL.MblobGetNumber(mBlobResult, ref nBblobNum);

			if (nBblobNum < 1) return; // 예외처리

			double[] pdCenterX = new double[nBblobNum];
			double[] pdCenterY = new double[nBblobNum];
			MIL.MblobGetResult(mBlobResult, MIL.M_FERET_X, pdCenterX);
			MIL.MblobGetResult(mBlobResult, MIL.M_FERET_Y, pdCenterY);

			for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)pdCenterX[nCnt] + .5f;
				ptCenter.Y = (float)pdCenterY[nCnt] + .5f;

				vPoint.Add(ptCenter);
			}
			pdCenterX = null;
			pdCenterY = null;
		}
		internal void GetBlobResult_BOX_LT(MIL_ID mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();

			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MIN, ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MIN, ref vBoxY);

			if (vBoxX.Count != vBoxY.Count)
			{
				// Fail...
				return;
			}

			int nBlobNum = vBoxX.Count;   // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)vBoxX[nCnt] + .5f;
				ptCenter.Y = (float)vBoxY[nCnt] + .5f;

				vPoint.Add(ptCenter);
			}
		}

		internal void GetBlobResult_BOX_RT(MIL_ID mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();

			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MAX, ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MIN, ref vBoxY);

			if (vBoxX.Count != vBoxY.Count)
			{
				// Fail...
				return;
			}

			int nBlobNum = vBoxX.Count;   // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)vBoxX[nCnt] + .5f;
				ptCenter.Y = (float)vBoxY[nCnt] + .5f;

				vPoint.Add(ptCenter);
			}
		}

		internal void GetBlobResult_BOX_RB(MIL_ID mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();

			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MAX, ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MAX, ref vBoxY);

			if (vBoxX.Count != vBoxY.Count)
			{
				// Fail...
				return;
			}
			int nBlobNum = vBoxX.Count;   // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)vBoxX[nCnt] + .5f;
				ptCenter.Y = (float)vBoxY[nCnt] + .5f;

				vPoint.Add(ptCenter);
			}
		}

		internal void GetBlobResult_BOX_LB(MIL_ID mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();

			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MIN, ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MAX, ref vBoxY);

			if (vBoxX.Count != vBoxY.Count)
			{
				// Fail...
				return;
			}

			int nBlobNum = vBoxX.Count;   // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)vBoxX[nCnt];
				ptCenter.Y = (float)vBoxY[nCnt];

				vPoint.Add(ptCenter);
			}
		}
		internal void GetBlobResult_BoxSize_X(MIL_ID mBlobResult, ref List<double> vBoxSize_X)
		{
			vBoxSize_X.Clear();

			List<double> vBoxX_Min = new List<double>();
			List<double> vBoxX_Max = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MIN, ref vBoxX_Min);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MAX, ref vBoxX_Max);

			if (vBoxX_Min.Count != vBoxX_Max.Count)
			{
				// Fail...
				return;
			}

			int nBlobNum = vBoxX_Min.Count;       // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				double dBoxSize_X = vBoxX_Max[nCnt] - vBoxX_Min[nCnt];
				vBoxSize_X.Add(dBoxSize_X);
			}
		}

		internal void GetBlobResult_BoxSize_Y(MIL_ID mBlobResult, ref List<double> vBoxSize_Y)
		{
			vBoxSize_Y.Clear();

			List<double> vBoxY_Min = new List<double>();
			List<double> vBoxY_Max = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MIN, ref vBoxY_Min);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MAX, ref vBoxY_Max);

			if (vBoxY_Min.Count != vBoxY_Max.Count)
			{
				// Fail...
				return;
			}

			int nBlobNum = vBoxY_Min.Count;     // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int ni = 0; ni < nBlobNum; ++ni)
			{
				double dBoxSize_Y = vBoxY_Max[ni] - vBoxY_Min[ni];

				vBoxSize_Y.Add(dBoxSize_Y);
			}
		}

		internal void GetBlobResult_DoubleType(MIL_ID mBlobResult, MIL_INT nOper, ref List<double> vDoubleType)
		{
			MIL_INT nBblobNum = MIL_INT.MinValue;
			MIL.MblobGetNumber(mBlobResult, ref nBblobNum);

			if (nBblobNum < 1) return;

			double[] pdDoubleType = new double[nBblobNum];
			MIL.MblobGetResult(mBlobResult, nOper, pdDoubleType);

			vDoubleType.Clear();
			for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				vDoubleType.Add(pdDoubleType[nCnt]);
			}
			pdDoubleType = null;
		}
		void GetBlobResult_Label(MIL_ID mBlobResult, ref List<long> vLabel)
		{
			MIL_INT nBblobNum = MIL_INT.MinValue;
			MIL.MblobGetNumber(mBlobResult, ref nBblobNum);

			if (nBblobNum < 1) return;

			long[] plLabel = new long[nBblobNum];
			MIL.MblobGetResult(mBlobResult, MIL.M_LABEL_VALUE + MIL.M_TYPE_LONG, plLabel);

			vLabel.Clear();
			for (int nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				vLabel.Add(plLabel[nCnt]);
			}

			plLabel = null;
		}
		internal bool PostProcessing_Mask(MIL_ID mSrcID, MIL_ID mDstID, MIL_ID mBlobResult, BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
		{
			bool bRet = false;
			switch (eMaskType)
			{
				case BLOB_MASK.E_BLOB_MASKTYPE_NONE:
					bRet = true;
					return bRet;
				case BLOB_MASK.E_BLOB_MASKTYPE_FILLRECT:
					PostProcessing_Mask_Rect(mSrcID, mDstID, mBlobResult, nInflateSize, true, eDstGV);
					break;
				case BLOB_MASK.E_BLOB_MASKTYPE_RECT:
					PostProcessing_Mask_Rect(mSrcID, mDstID, mBlobResult, nInflateSize, false, eDstGV);
					break;
				case BLOB_MASK.E_BLOB_MASKTYPE_HORIZONE:
					PostProcessing_Horizone_Mask_Line(mSrcID, mDstID, mBlobResult, nInflateSize, eDstGV);
					break;
				case BLOB_MASK.E_BLOB_MASKTYPE_VERTICAL:
					PostProcessing_Vertical_Mask_Line(mSrcID, mDstID, mBlobResult, nInflateSize, eDstGV);
					break;
			}
			return bRet;
		}
		internal bool PostProcessing_Mask_Rect(MIL_ID mSrcID, MIL_ID mDstID, MIL_ID mBlobResult, int nInflateSize, bool bFillRect, GV_OPERATION eDstGV)
		{
			//////////////////////////////////////////////////////////////////////////
			// Init Funtion
			List<RectangleF> vrcBlobBox = new List<RectangleF>();

			GetBlobResult_BoxRect(mBlobResult, ref vrcBlobBox);

			foreach (RectangleF BlobBox in vrcBlobBox)
			{
				RectangleF rcTmpRect = BlobBox;

				rcTmpRect.Inflate(nInflateSize, nInflateSize);

				if (bFillRect) RectFill(mDstID, rcTmpRect, eDstGV);
				else RectBox(mDstID, rcTmpRect, eDstGV);
			}

			return true;
		}
		internal bool PostProcessing_Vertical_Mask_Line(MIL_ID mSrcID, MIL_ID mDstID, MIL_ID mBlobResult, int nInflateSize, GV_OPERATION eDstGV)
		{
			//////////////////////////////////////////////////////////////////////////
			// Init Funtion
			List<RectangleF> vrcBlobBox = new List<RectangleF>();

			GetBlobResult_BoxRect(mBlobResult, ref vrcBlobBox);
			//////////////////////////////////////////////////////////////////////////

			foreach (RectangleF BlobBox in vrcBlobBox)
			{
				RectangleF rcLeft = new RectangleF(), rcRight = new RectangleF(), rcTmpRect = BlobBox;

				byte byColor = (byte)eDstGV;

				rcTmpRect.Inflate(nInflateSize, nInflateSize);
				rcLeft.X = rcTmpRect.Left; rcLeft.Width = rcTmpRect.Width; rcLeft.Y = rcTmpRect.Y; rcLeft.Height = rcTmpRect.Height;
				rcRight.X = rcTmpRect.Right; rcRight.Width = rcTmpRect.Width; rcRight.Y = rcTmpRect.Y; rcRight.Height = rcTmpRect.Height;

				MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byColor, byColor, byColor));
				MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcLeft.Left,
					rcLeft.Top,
					rcLeft.Right,
					rcLeft.Bottom);
				MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcRight.Left,
					rcRight.Top,
					rcRight.Right,
					rcRight.Bottom);
			}


			return true;
		}

		internal bool PostProcessing_Horizone_Mask_Line(MIL_ID mSrcID, MIL_ID mDstID, MIL_ID mBlobResult, int nInflateSize, GV_OPERATION eDstGV)
		{
			//////////////////////////////////////////////////////////////////////////
			// Init Funtion
			List<RectangleF> vrcBlobBox = new List<RectangleF>();

			GetBlobResult_BoxRect(mBlobResult, ref vrcBlobBox);
			//////////////////////////////////////////////////////////////////////////


			foreach (RectangleF BlobBox in vrcBlobBox)
			{
				RectangleF rcTop = new RectangleF(), rcBtm = new RectangleF(), rcTmpRect = BlobBox;

				byte byColor = (byte)eDstGV;

				rcTmpRect.Inflate(nInflateSize, nInflateSize);
				rcTop.X = rcTmpRect.Left; rcTop.Width = rcTmpRect.Width; rcTop.Y = rcTmpRect.Y; rcTop.Height = rcTmpRect.Height;
				rcBtm.X = rcTmpRect.Right; rcBtm.Width = rcTmpRect.Width; rcBtm.Y = rcTmpRect.Y; rcBtm.Height = rcTmpRect.Height;

				MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byColor, byColor, byColor));
				MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcTop.Left,
					rcTop.Top,
					rcTop.Right,
					rcTop.Bottom);
				MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcBtm.Left,
					rcBtm.Top,
					rcBtm.Right,
					rcBtm.Bottom);
			}

			return true;
		}
		internal void GetBlobResult_BoxRect(MIL_ID mBlobResult, ref List<RectangleF> vBoxRect)
		{
			vBoxRect.Clear();

			List<double> vBoxX_Min = new List<double>();
			List<double> vBoxX_Max = new List<double>();
			List<double> vBoxY_Min = new List<double>();
			List<double> vBoxY_Max = new List<double>();


			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MIN, ref vBoxX_Min);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_X_MAX, ref vBoxX_Max);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MIN, ref vBoxY_Min);
			GetBlobResult_DoubleType(mBlobResult, MIL.M_BOX_Y_MAX, ref vBoxY_Max);

			if ((vBoxX_Min.Count != vBoxX_Max.Count) || (vBoxY_Min.Count != vBoxY_Max.Count) || (vBoxX_Min.Count != vBoxY_Min.Count))
			{
				// Fail...
				return;
			}

			int nBlobNum = vBoxX_Min.Count;   // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{

				RectangleF rcRect = new RectangleF();
				rcRect.X = (float)Math.Round(vBoxX_Min[nCnt]);
				rcRect.Y = (float)Math.Round(vBoxY_Min[nCnt]);
				rcRect.Width = (float)Math.Round(vBoxX_Max[nCnt] - vBoxX_Min[nCnt]);
				rcRect.Height = (float)Math.Round(vBoxY_Max[nCnt] - vBoxY_Min[nCnt]);

				vBoxRect.Add(rcRect);
			}
		}

		internal void RectFill(MIL_ID mDstID, RectangleF rtRect, GV_OPERATION eGVOperation)
		{
			byte byFill = (byte)eGVOperation;
			MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byFill, byFill, byFill));
			MIL.MgraRectFill(MIL.M_DEFAULT, mDstID, rtRect.Left,
													rtRect.Top,
													rtRect.Right,
													rtRect.Bottom);
		}
		internal void RectBox(MIL_ID mDstID, RectangleF rtRect, GV_OPERATION eGVOperation)
		{
			byte byFill = (byte)eGVOperation;
			MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byFill, byFill, byFill));
			MIL.MgraRectFill(MIL.M_DEFAULT, mDstID, rtRect.Left,
													rtRect.Top,
													rtRect.Right,
													rtRect.Bottom);
		}
		internal void UnSelectBlob_Label(MIL_ID mDstID, MIL_ID mBlobResult, long lLabel)
		{
			MIL.MblobSelect(mBlobResult, MIL.M_EXCLUDE, MIL.M_LABEL_VALUE, MIL.M_EQUAL, lLabel, MIL.M_NULL);
			MIL.MblobFill(mBlobResult, mDstID, MIL.M_EXCLUDED_BLOBS, 0);
		}

		internal bool Blob_Select_Position(MIL_ID mSrcID, MIL_ID mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
				BLOB_SELECT_POSITION_EXTRACT_FEATURE eExtractFeature, BLOB_SELECT_POSITION_EXTRACT_METHOD eExtractMethod,
				DECISION_LIST_OPER eDecisionOperType, int nExceptionBlobNum,
				BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
		{
			bool bRetVal = true;

			if (mSrcID != mDstID)
				MIL.MbufCopy(mSrcID, mDstID);

			// [1] Init
			MIL_ID mBlobResult = MIL.M_NULL;
			MIL.MblobAllocResult(Alloc.SystemAlloc, ref mBlobResult);

			// [2] Valid Range
			CalcBlob(mDstID, MIL.M_NULL, eFeature, ref mBlobResult);
			SelectBlob(mDstID, eFeature, lMin, lMax, bMaxLimit, ref mBlobResult);
			ExtractBlob_Position(mDstID, eExtractFeature, eExtractMethod, mBlobResult);

			MIL_INT nBlobNum = 0;
			MIL.MblobGetNumber(mBlobResult, ref nBlobNum);

			switch (eDecisionOperType)
			{
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_GREATER:
					if (nBlobNum > nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_GREATER_OR_EQUAL:
					if (nBlobNum >= nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_LESS:
					if (nBlobNum < nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_LESS_OR_EQUAL:
					if (nBlobNum <= nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_EQUAL:
					if (nBlobNum == nExceptionBlobNum)
						bRetVal = false;
					break;
				case DECISION_LIST_OPER.E_DECISION_LIST_OPER_NOT_EQUAL:
					if (nBlobNum != nExceptionBlobNum)
						bRetVal = false;
					break;
			}

			if (bRetVal)
			{
				PostProcessing_Mask(mSrcID, mDstID, mBlobResult, eMaskType, eDstGV, nInflateSize);
			}

			if (mBlobResult != MIL.M_NULL) MIL.MblobFree(mBlobResult); mBlobResult = MIL.M_NULL;

			return bRetVal;
		}

		internal bool Blob_Mark_GetPoint(MIL_ID mSrcID, MIL_ID mDstID, ref double dX, ref double dY, int nDirection)
		{
			MIL_ID MilBlobResult = MIL.M_NULL, MilBlobFeatureList = MIL.M_NULL;
			MIL_INT TotalBlobs = MIL_INT.MinValue;
			double[] CogMinX, CogMaxX,
					 CogX, CogY,
					 CogMinY, CogMaxY;

			MIL.MblobAllocFeatureList(Alloc.SystemAlloc, ref MilBlobFeatureList);

			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_CENTER_OF_GRAVITY);
			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_X_MIN);
			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_X_MAX);
			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_Y_MIN);
			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_Y_MAX);

			MIL.MblobAllocResult(Alloc.SystemAlloc, ref MilBlobResult);
			MIL.MblobCalculate(mSrcID, MIL.M_NULL, MilBlobFeatureList, MilBlobResult);
			MIL.MblobGetNumber(MilBlobResult, ref TotalBlobs);

			if (TotalBlobs != 0)
			{
				CogX	= new double[TotalBlobs];
				CogY	= new double[TotalBlobs];
				CogMinX = new double[TotalBlobs];
				CogMaxX = new double[TotalBlobs];
				CogMinY = new double[TotalBlobs];
				CogMaxY = new double[TotalBlobs];
			}
			else
			{
				return true;
			}

			MIL.MblobGetResult(MilBlobResult, MIL.M_CENTER_OF_GRAVITY_X, CogX);
			MIL.MblobGetResult(MilBlobResult, MIL.M_CENTER_OF_GRAVITY_Y, CogY);
			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_X_MIN, CogMinX);
			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_X_MAX, CogMaxX);
			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_Y_MIN, CogMinY);
			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_Y_MAX, CogMaxY);

			if (nDirection == 0)
			{// Left
				dX += CogMinX[0];
				dY += CogY[0];
			}
			else if (nDirection == 1)
			{// Center
				dX += CogX[0];
				dY += CogY[0];
			}
			else if (nDirection == 2)
			{// Right
				dX += CogMaxX[0];
				dY += CogY[0];
			}
			else if (nDirection == 3)
			{// Top
				dX += CogX[0];
				dY += CogMinY[0];
			}
			else
			{// Bottom
				dX += CogX[0];
				dY += CogMaxY[0];
			}
			/* Free all allocations. */
			if (MilBlobResult != MIL.M_NULL) MIL.MblobFree(MilBlobResult); MilBlobResult = MIL.M_NULL;
			if (MilBlobFeatureList != MIL.M_NULL) MIL.MblobFree(MilBlobFeatureList); MilBlobFeatureList = MIL.M_NULL;

			return true;
		}

		internal bool Blob_Get_BOX_Point(MIL_ID mSrcID, MIL_ID mDstID, ref double dLX, ref double dTY, ref double dRX, ref double dBY)
		{
			MIL_ID MilBlobResult = MIL.M_NULL, MilBlobFeatureList = MIL.M_NULL;
			MIL_INT TotalBlobs = MIL.M_NULL;
			double[] dBoxLx, dBoxRx, dBoxTy, dBoxBy;


			MIL.MblobAllocFeatureList(Alloc.SystemAlloc, ref MilBlobFeatureList);

			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_X_MIN);
			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_X_MAX);
			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_Y_MIN);
			MIL.MblobSelectFeature(MilBlobFeatureList, MIL.M_BOX_Y_MAX);

			MIL.MblobAllocResult(Alloc.SystemAlloc, ref MilBlobResult);
			MIL.MblobCalculate(mSrcID, MIL.M_NULL, MilBlobFeatureList, MilBlobResult);
			MIL.MblobGetNumber(MilBlobResult, ref TotalBlobs);

			if (TotalBlobs != 0)
			{
				dBoxLx = new double[TotalBlobs];
				dBoxRx = new double[TotalBlobs];
				dBoxTy = new double[TotalBlobs];
				dBoxBy = new double[TotalBlobs];
			}
			else
			{
				return true;
			}

			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_X_MIN, dBoxLx);
			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_X_MAX, dBoxRx);
			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_Y_MIN, dBoxTy);
			MIL.MblobGetResult(MilBlobResult, MIL.M_BOX_Y_MAX, dBoxBy);

			dLX = dBoxLx[0];
			dRX = dBoxRx[0];
			dTY = dBoxTy[0];
			dBY = dBoxBy[0];
			return true;
		}
	}
}
