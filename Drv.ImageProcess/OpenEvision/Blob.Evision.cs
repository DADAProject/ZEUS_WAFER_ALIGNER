using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{	
	internal partial class Blob 
	{
		//internal EObjectSelection mSelection  = new EObjectSelection(); //Blob instance
		//internal ECodedImage2     mCodedImage = new ECodedImage2();

		public EObjectSelection mSelection;
        public ECodedImage2 mCodedImage;

        internal bool Blob_ReconstructFromSeed(EImageBW8 mSrcID, EImageBW8 mSeedID, EImageBW8 mDstID, MORPHOLOGY_CONDITION eCondition)
		{
			//MIL.MblobReconstruct(mSrcID, mSeedID, mDstID, MIL.M_RECONSTRUCT_FROM_SEED, (long) eCondition);

			return true;
		}

		internal bool Blob_FillHole(EImageBW8 mSrcID, EImageBW8 mDstID)
		{
			//MIL.MblobReconstruct(mSrcID, MIL.M_NULL, mDstID, MIL.M_FILL_HOLES, MIL.M_8_CONNECTED);

			return true;
		}

		internal bool Blob_EraseBorder(EImageBW8 mSrcID, EImageBW8 mDstID)
		{
			//MIL.MblobReconstruct(mSrcID, MIL.M_NULL, mDstID, MIL.M_ERASE_BORDER_BLOBS, MIL.M_8_CONNECTED);

			return true;
		}

		internal bool Blob_ConvexHull(EImageBW8 mSrcID, EImageBW8 mDstID, CONVEXHULL_TYPE eConvexHullType, bool bUseParallel)
		{
			//////////////////////////////////////////////////////////////////////////
			// Funtion Initialize
			//BUFF_INFO biSrc = new BUFF_INFO();
			//BUFF_INFO biDst = new BUFF_INFO();
			//biSrc.buffID = mSrcID;
			//biDst.buffID = mDstID;
			//
			//biSrc.InitBuffInfo(false, 0);
			//biDst.InitBuffInfo(true , 0);
			//
			//int nInflateSize = 1;
			//int nW = biSrc.wid + (nInflateSize * 2);
			//int nH = biSrc.len + (nInflateSize * 2);
			//
			//MIL_ID mSrcInflateID = MIL.M_NULL;
			//MIL.MbufAlloc2d(ImageProcess.SystemAlloc, nW, nH, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC, ref mSrcInflateID);
			//
			//MIL_ID mDstInflateID = MIL.M_NULL;
			//MIL.MbufAlloc2d(ImageProcess.SystemAlloc, nW, nH, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC, ref mDstInflateID);
			//
			//
			//BUFF_INFO bistDstInflate = new BUFF_INFO();
			//bistDstInflate.buffID = mDstInflateID;
			//bistDstInflate.InitBuffInfo(true, 0);
			//
			//BUFF_INFO bistSrcInflate = biSrc.CopyBuffer(mSrcInflateID, 0, 0, biSrc.wid, biSrc.len, nInflateSize, nInflateSize);
			//
			//MIL.MimArith(bistSrcInflate.buffID, MIL.M_NULL, bistSrcInflate.buffID, MIL.M_NOT);
			//
			//int wid = bistSrcInflate.wid, len = bistSrcInflate.len;
			////	long	pitch(stSrcInflate.pitch);
			//
			////////////////////////////////////////////////////////////////////////////
			//ContourPoints ContPnt  = new ContourPoints();
			//ContourPoints ConvexPt = new ContourPoints();
			//Contour contour = new Contour();
			//contour.ContourTrace(bistSrcInflate.buffID, ref ContPnt);
			//
			///*int num = m_pMil.CalcBlob(Dst.buffID, Dst.buffID, TRUE, FALSE, 20);
			//m_pMil.SelectBlob_MaxArea(num, Dst.buffID);*/
			//
			//int nPointNum = ContPnt.num / 2;
			//
			//int[] pnrc = new int[nPointNum + 5];
			//int[] pncc = new int[nPointNum + 5];
			//
			//for (int ni = 0; ni < nPointNum; ni++)
			//{
			//	pncc[ni] = (int) ContPnt.points[ni * 2].X;
			//	pnrc[ni] = (int) ContPnt.points[ni * 2].Y;
			//}
			//
			//int nConvexPtNum = MilExtension.GetConvexHullPt(pnrc, pncc, nPointNum);
			//ConvexPt.num = nConvexPtNum;
			//
			//for (int ni = 0; ni < nConvexPtNum; ni++)
			//{
			//	ConvexPt.points[ni].X = pncc[ni];
			//	ConvexPt.points[ni].Y = pnrc[ni];
			//}
			//
			//PointF[] poly = new PointF[nConvexPtNum + 5];
			////	FPoint pt;
			//int nmax_x, nmax_y, nmin_x, nmin_y;
			//int nxx, nyy;
			////	int cnt = 0;
			//
			//nmax_x = nmax_y = 0;
			//nmin_x = nmin_y = 1000;
			//
			//for (int ni = 0; ni < nConvexPtNum; ni++)
			//{
			//	nxx = pncc[ni];
			//	nyy = pnrc[ni];
			//
			//	if (nxx > nmax_x) nmax_x = nxx;
			//	if (nyy > nmax_y) nmax_y = nyy;
			//	if (nxx < nmin_x) nmin_x = nxx;
			//	if (nyy < nmin_y) nmin_y = nyy;
			//
			//	poly[ni].X = (float)pncc[ni];
			//	poly[ni].Y = (float)pnrc[ni];
			//}
			//
			////#ifndef bUseParallel
			//for (int ni = nmin_y; ni <= nmax_y; ni++)
			//// #else
			//// 	parallel_for(min_y, max_y, [&] (int i)
			//// #endif
			//{
			//	for (int nj = nmin_x; nj <= nmax_x; nj++)
			//	{
			//		if (bistSrcInflate.pBuff[ni * bistSrcInflate.pitch + nj] > /*240*/0)
			//		{
			//			// 				pt.x = (float)j;
			//			// 				pt.y = (float)i;
			//			int InOut = MilExtension.pnpoly(nConvexPtNum, poly, (float)nj, (float)ni);
			//			if (InOut > 0)
			//			{
			//				bistDstInflate.pBuff[ni * bistDstInflate.pitch + nj] = 255;
			//				//					cnt++;
			//			}
			//		}
			//	}
			//}
			////#ifdef bUseParallel
			////	);
			////#endif
			//
			//if (eConvexHullType == CONVEXHULL_TYPE.E_CONVEXHULL_EXTRACT_ONLY)
			//{
			//	biDst = bistDstInflate.CopyBuffer(mDstID, nInflateSize, nInflateSize, biDst.wid, biDst.len, 0, 0);
			//}
			//else if (eConvexHullType == CONVEXHULL_TYPE.E_CONVEXHULL_FILL)
			//{
			//	biDst = bistDstInflate.CopyBuffer(mDstID, nInflateSize, nInflateSize, biDst.wid, biDst.len, 0, 0);
			//
			//	MIL.MimArith(mSrcID, mDstID, mDstID, MIL.M_OR);
			//}
			//
			//if (bistSrcInflate.buffID != MIL.M_NULL) MIL.MbufFree(bistSrcInflate.buffID); bistSrcInflate.buffID = MIL.M_NULL;
			//if (bistDstInflate.buffID != MIL.M_NULL) MIL.MbufFree(bistDstInflate.buffID); bistDstInflate.buffID = MIL.M_NULL;

			return true;
		}

		internal bool Blob_Select_Geometry(EImageBW8 mSrcID, EImageBW8 mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
			BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod,
			DECISION_LIST_OPER eDecisionOperType, int nExceptionBlobNum,
			BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
		{
			bool bRetVal = true;

			//if (mSrcID != mDstID)
			//	MIL.MbufCopy(mSrcID, mDstID);
			//
			//// [1] Init
			//MIL_ID mBlobResult = MIL.M_NULL;
			//MIL.MblobAllocResult(ImageProcess.SystemAlloc, ref mBlobResult);
			//
			//// [2] Valid Range
			//CalcBlob(mDstID, MIL.M_NULL, eFeature, ref mBlobResult);
			//SelectBlob(mDstID, eFeature, lMin, lMax, bMaxLimit, ref mBlobResult);
			//ExtractBlob_Geometry(mDstID, eFeature, eExtractMethod, mBlobResult);
			//
			//MIL_INT nBlobNum = 0;
			//MIL.MblobGetNumber(mBlobResult, ref nBlobNum);
			//
			//switch (eDecisionOperType)
			//{
			//	case DECISION_LIST_OPER.E_DECISION_LIST_OPER_GREATER:
			//		if (nBlobNum > nExceptionBlobNum)
			//			bRetVal = false;
			//		break;
			//	case DECISION_LIST_OPER.E_DECISION_LIST_OPER_GREATER_OR_EQUAL:
			//		if (nBlobNum >= nExceptionBlobNum)
			//			bRetVal = false;
			//		break;
			//	case DECISION_LIST_OPER.E_DECISION_LIST_OPER_LESS:
			//		if (nBlobNum < nExceptionBlobNum)
			//			bRetVal = false;
			//		break;
			//	case DECISION_LIST_OPER.E_DECISION_LIST_OPER_LESS_OR_EQUAL:
			//		if (nBlobNum <= nExceptionBlobNum)
			//			bRetVal = false;
			//		break;
			//	case DECISION_LIST_OPER.E_DECISION_LIST_OPER_EQUAL:
			//		if (nBlobNum == nExceptionBlobNum)
			//			bRetVal = false;
			//		break;
			//	case DECISION_LIST_OPER.E_DECISION_LIST_OPER_NOT_EQUAL:
			//		if (nBlobNum != nExceptionBlobNum)
			//			bRetVal = false;
			//		break;
			//}
			//
			//if (bRetVal)
			//{
			//	PostProcessing_Mask(mSrcID, mDstID, mBlobResult, eMaskType, eDstGV, nInflateSize);
			//}
			//
			//if (mBlobResult != MIL.M_NULL) MIL.MblobFree(mBlobResult);
			//mBlobResult = MIL.M_NULL;

			return bRetVal;
		}

		internal void SelectBlob(EImageBW8 mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit, ref EObjectSelection mBlobResult, ref ECodedImage2 mCodeResult)
		{
			if (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X ||
				eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_Y)
			{
				SelectBlob_Simple(mDstID, (int)eFeature, lMin, lMax, true, ref mBlobResult, ref mCodeResult);

				if    (eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X)
					SelectBlob_Complex_BoxSize(mDstID, true, lMin, lMax, bMaxLimit, ref mBlobResult);
				else// eFeature == BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_Y
					SelectBlob_Complex_BoxSize(mDstID, false, lMin, lMax, bMaxLimit, ref mBlobResult);
			}
			else
			{
				// [2] Select Feature
				int nOper = 0;
				switch (eFeature)
				{
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA               : nOper = (int)EFeature.Area;            break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA_BOX           : nOper = (int)MIL.M_BOX_AREA;               break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_WIDTH          : nOper = (int)MIL.M_BREADTH;                break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_LENGTH         : nOper = (int)MIL.M_LENGTH;                 break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_ELONGATION     : nOper = (int)MIL.M_ELONGATION;             break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MIN       : nOper = (int)MIL.M_FERET_MIN_DIAMETER;     break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MAX       : nOper = (int)MIL.M_FERET_MAX_DIAMETER;     break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MEAN      : nOper = (int)MIL.M_FERET_MEAN_DIAMETER;    break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_ELONGATION: nOper = (int)MIL.M_FERET_ELONGATION;       break;
					//case BLOB_COMMON_FEATURE.E_CONVEX_HULL_FILL_RATIO                 : nOper = (int)MIL.M_CONVEX_HULL_FILL_RATIO; break;
					case BLOB_COMMON_FEATURE.E_ROUGHNESS                              : nOper = (int)EFeature.Eccentricity;		break;
					//case BLOB_COMMON_FEATURE.E_CONVEX_PERIMETER                       : nOper = (int)MIL.M_CONVEX_PERIMETER;       break;
					//case BLOB_COMMON_FEATURE.E_CONVEX_AREA                            : nOper = (int)MIL.M_CONVEX_HULL_AREA;       break;
					//case BLOB_COMMON_FEATURE.E_COMPACTNESS                            : nOper = (int)MIL.M_COMPACTNESS;            break;
					default:
						return;
				}

				SelectBlob_Simple(mDstID, nOper, lMin, lMax, bMaxLimit, ref mBlobResult, ref mCodeResult);
			}
		}
		internal void SelectBlob_Simple(EImageBW8 mDstID, int nOper, double lMin, double lMax, bool bMaxLimit, ref EObjectSelection mBlobResult, ref ECodedImage2 mCodeResult)
		{
			//AREA는 RemoveUsingUnsignedIntegerFeature
			//X는
			EFeature eFeature = (EFeature)nOper;
			if   (nOper == (int)BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_X)
				eFeature = EFeature.BoundingBoxWidth;
			else if (nOper == (int)BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_BOXSIZE_Y)
				eFeature = EFeature.BoundingBoxHeight;

			if (eFeature == EFeature.BoundingBoxWidth || eFeature == EFeature.BoundingBoxHeight)
			{
				if (bMaxLimit)
					mBlobResult.RemoveUsingFloatFeature(eFeature, (uint)lMin, ESingleThresholdMode.Less);
				else
					mBlobResult.RemoveUsingUnsignedIntegerFeature(eFeature, (uint)lMin, (uint)lMax, EDoubleThresholdMode.Outside);
			}
            else if(eFeature == EFeature.Area)
            {
				if (bMaxLimit)
					mBlobResult.RemoveUsingUnsignedIntegerFeature(eFeature, (uint)lMin, ESingleThresholdMode.Less);
				else
					mBlobResult.RemoveUsingUnsignedIntegerFeature(eFeature, (uint)lMin, (uint)lMax, EDoubleThresholdMode.Outside);
			}
            else
            {
				if (bMaxLimit)
					mBlobResult.RemoveUsingFloatFeature(eFeature, (uint)lMin, ESingleThresholdMode.Less);
				else
					mBlobResult.RemoveUsingUnsignedIntegerFeature(eFeature, (uint)lMin, (uint)lMax, EDoubleThresholdMode.Outside);
			}
			

			uint TotalBlob = mBlobResult.ElementCount;
			//IntPtr GraphicContext = Easy.OpenImageGraphicContext(mDstID);
			//2.7 버전 속도 개선 22.7안됨
			//mCodeResult.Draw(GraphicContext, mBlobResult);
			//Easy.CloseImageGraphicContext(mDstID, GraphicContext);
		}
		internal void SelectBlob_Complex_BoxSize(EImageBW8 mDstID, bool bSelectBoxSizeX, double lMin, double lMax, bool bMaxLimit, ref EObjectSelection mBlobResult)
		{
			List<double> vDouble = new List<double>();
			List<long> vLabel	 = new List<long>();
			
			if (bSelectBoxSizeX)
				GetBlobResult_BoxSize_X(mBlobResult, ref vDouble);
			else
				GetBlobResult_BoxSize_Y(mBlobResult, ref vDouble);
			
			GetBlobResult_Label(mBlobResult, ref vLabel);
			
			if (vDouble.Count != vLabel.Count) return;  // Fail
			
			uint nBblobNum = mBlobResult.ElementCount;  
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
		internal void ExtractBlob_Position(EImageBW8 mDstID, BLOB_SELECT_POSITION_EXTRACT_FEATURE eExtractFeature,
			BLOB_SELECT_POSITION_EXTRACT_METHOD eExtractMethod, EObjectSelection mBlobResult)
		{
			// [1] Get Result Point - ExtractFeature
			List<PointF> vPoint = new List<PointF>(); 
			
			switch (eExtractFeature)
			{
				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_GRAVITY_CENTER:
					GetBlobResult_GravityCenter(mSelection, ref vPoint);
					break;
			
				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_FERET_CENTER:
					GetBlobResult_FeretCenter(mSelection, ref vPoint);
					break;
			
				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_LT:
					GetBlobResult_BOX_LT(mSelection, ref vPoint);
					break;
			
				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_RT:
					GetBlobResult_BOX_RT(mSelection, ref vPoint);
					break;
			
				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_RB:
					GetBlobResult_BOX_RB(mSelection, ref vPoint);
					break;
			
				case BLOB_SELECT_POSITION_EXTRACT_FEATURE.E_BLOB_SELECT_POSITION_EXTRACT_FEATURE_BOX_LB:
					GetBlobResult_BOX_LB(mSelection, ref vPoint);
					break;
				default:
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
					ExtractBlob_PointType_MinOrMax(mDstID, mBlobResult, false, false, vPoint);
					break;
				default:
					break;
			
			}
		}
		internal void ExtractBlob_Geometry(EImageBW8 mDstID, BLOB_COMMON_FEATURE eFeature,
			BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod, EObjectSelection mBlobResult)
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
				int nOper = 0;
				switch (eFeature)
				{
					case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA               : nOper = (int)EFeature.Area;             break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_AREA_BOX           : nOper = MIL.M_BOX_AREA;               break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_WIDTH          : nOper = MIL.M_BREADTH;                break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_LENGTH         : nOper = MIL.M_LENGTH;                 break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_DIA_ELONGATION     : nOper = MIL.M_ELONGATION;             break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MIN       : nOper = MIL.M_FERET_MIN_DIAMETER;     break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MAX       : nOper = MIL.M_FERET_MAX_DIAMETER;     break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_MEAN      : nOper = MIL.M_FERET_MEAN_DIAMETER;    break;
					//case BLOB_COMMON_FEATURE.E_BLOB_COMMON_FEATURE_FERETDIA_ELONGATION: nOper = MIL.M_FERET_ELONGATION;       break;
					//case BLOB_COMMON_FEATURE.E_CONVEX_HULL_FILL_RATIO                 : nOper = MIL.M_CONVEX_HULL_FILL_RATIO; break;
					//case BLOB_COMMON_FEATURE.E_ROUGHNESS                              : nOper = MIL.M_ROUGHNESS;              break;
					//case BLOB_COMMON_FEATURE.E_CONVEX_PERIMETER                       : nOper = MIL.M_CONVEX_PERIMETER;       break;
					//case BLOB_COMMON_FEATURE.E_CONVEX_AREA                            : nOper = MIL.M_CONVEX_HULL_AREA;       break;
					//case BLOB_COMMON_FEATURE.E_COMPACTNESS                            : nOper = MIL.M_COMPACTNESS;            break;
					default:
						return;
				}
			
				ExtractBlob_Geometry_Simple(mDstID, nOper, eExtractMethod, mBlobResult);
			}
		}
		internal void ExtractBlob_Geometry_Complex_BoxSize(EImageBW8 mDstID, bool bSelectBoxSizeX, BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod, EObjectSelection mBlobResult)
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

		internal void ExtractBlob_DoubleType_MinOrMax(EImageBW8 mDstID, EObjectSelection mBlobResult, bool bMin, List<double> vDoubleType)
		{
			// Blob Result가 0이면 오류발생하여 예외처리.
			if (vDoubleType.Count <= 0) return;
			
			double dMin = vDoubleType[0];
			double dMax = vDoubleType[0];
			int nMinIndex = 0;
			int nMaxIndex = 0;
			
			int nBblobNum = vDoubleType.Count;
			
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
				mBlobResult.RemoveUsingIntegerFeature(EFeature.ElementIndex, (int)vLabel[nMinIndex], ESingleThresholdMode.Different);
			}
			else
			{
				mBlobResult.RemoveUsingIntegerFeature(EFeature.ElementIndex, (int)vLabel[nMaxIndex], ESingleThresholdMode.Different);
			}
			//MIL.MblobFill(mBlobResult, mDstID, MIL.M_EXCLUDED_BLOBS, 0);
		}

		internal void ExtractBlob_PointType_MinOrMax(EImageBW8 mDstID, EObjectSelection mBlobResult, bool bX, bool bMin, List<PointF> vPointType)
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
			
			int nBblobNum = vPointType.Count();
			
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
			
			if (vPointType.Count() != vLabel.Count()) return; // Fail

			if (bMin)
			{
				mBlobResult.RemoveUsingUnsignedIntegerFeature(EFeature.ElementIndex, (uint)vLabel[nMinIndex], ESingleThresholdMode.Different);
			}
			else
			{
				mBlobResult.RemoveUsingUnsignedIntegerFeature(EFeature.ElementIndex, (uint)vLabel[nMaxIndex], ESingleThresholdMode.Different);
			}
		}

		internal void ExtractBlob_Geometry_Simple(EImageBW8 mDstID, int nOper, BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod, EObjectSelection mBlobResult)
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
		internal void GetBlobResult_GravityCenter(EObjectSelection mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();
			uint nBblobNum = mBlobResult.ElementCount;

			if (nBblobNum < 1) return; // 예외처리
			
			for (uint nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)mBlobResult.GetFloatFeature(nCnt, EFeature.GravityCenterX) + .5f;
				ptCenter.Y = (float)mBlobResult.GetFloatFeature(nCnt, EFeature.GravityCenterY) + .5f;
			
				vPoint.Add(ptCenter);
			}
		}
		internal void GetBlobResult_FeretCenter(EObjectSelection mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();
			uint nBblobNum = mBlobResult.ElementCount;
			
			if (nBblobNum < 1) return; // 예외처리
			
			for (uint nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				PointF ptCenter = new PointF();
				ptCenter.X = (float)mBlobResult.GetFloatFeature(nCnt, EFeature.FeretBoxCenterX) + .5f;
				ptCenter.Y = (float)mBlobResult.GetFloatFeature(nCnt, EFeature.FeretBoxCenterY) + .5f;

				vPoint.Add(ptCenter);
			}
		}
		internal void GetBlobResult_BOX_LT(EObjectSelection mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();
			
			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.LeftLimit, ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.TopLimit , ref vBoxY);
			
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
		internal void GetBlobResult_BOX_RT(EObjectSelection mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();
			
			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.RightLimit, ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.TopLimit  , ref vBoxY);
			
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
		internal void GetBlobResult_BOX_RB(EObjectSelection mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();
			
			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.RightLimit , ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.BottomLimit, ref vBoxY);
			
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
		internal void GetBlobResult_BOX_LB(EObjectSelection mBlobResult, ref List<PointF> vPoint)
		{
			vPoint.Clear();
			
			List<double> vBoxX = new List<double>();
			List<double> vBoxY = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.LeftLimit  , ref vBoxX);
			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.BottomLimit, ref vBoxY);
			
			if (vBoxX.Count != vBoxY.Count)
			{
				// Fail...
				return;
			}
			
			int nBlobNum = vBoxX.Count;   // MblobGetNumber(mBlobResult, &nBblobNum);
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				PointF ptCenter = new PointF() ;
				ptCenter.X = (float)vBoxX[nCnt];
				ptCenter.Y = (float)vBoxY[nCnt];
			
				vPoint.Add(ptCenter);
			}
		}
		internal void GetBlobResult_BoxSize_X(EObjectSelection mBlobResult, ref List<double> vBoxSize_X)
		{
			vBoxSize_X.Clear();
			
			List<double> vBoxX_Min = new List<double>();
			List<double> vBoxX_Max = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.LeftLimit, ref vBoxX_Min);
			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.RightLimit, ref vBoxX_Max);
			
			if (vBoxX_Min.Count != vBoxX_Max.Count)
			{
				// Fail...
				return;
			}
			
			int nBlobNum = vBoxX_Min.Count;      
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				double dBoxSize_X = vBoxX_Max[nCnt] - vBoxX_Min[nCnt];
				vBoxSize_X.Add(dBoxSize_X);
			}
		}
		internal void GetBlobResult_BoxSize_Y(EObjectSelection mBlobResult, ref List<double> vBoxSize_Y)
		{
			vBoxSize_Y.Clear();
			
			List<double> vBoxY_Min = new List<double>(); 
			List<double> vBoxY_Max = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.TopLimit , ref vBoxY_Min);
			GetBlobResult_DoubleType(mBlobResult, (int)EFeature.BottomLimit, ref vBoxY_Max);
	    	
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

		internal void GetBlobResult_DoubleType(EObjectSelection mBlobResult, int nOper, ref List<double> vDoubleType)
		{
			uint nBblobNum = mBlobResult.ElementCount;
			
			if (nBblobNum < 1) return;
			
			vDoubleType.Clear();

			//LeftLimit = 7,RightLimit = 8,TopLimit = 9,BottomLimit = 10 = GetIntegerFeature
			for (uint nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				vDoubleType.Add((double)mBlobResult.GetIntegerFeature(nCnt, (EFeature)nOper));
			}
		}

		void GetBlobResult_Label(EObjectSelection mBlobResult, ref List<long> vLabel)
		{
			uint nBblobNum = mBlobResult.ElementCount;

			if (nBblobNum < 1) return;
			
			vLabel.Clear();
			for (uint nCnt = 0; nCnt < nBblobNum; ++nCnt)
			{
				vLabel.Add((long)mBlobResult.GetUnsignedIntegerFeature(nCnt, EFeature.ElementIndex));
			}
		}
		internal bool PostProcessing_Mask(EImageBW8 mSrcID, EImageBW8 mDstID, EObjectSelection mBlobResult, BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
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
		internal bool PostProcessing_Mask_Rect(EImageBW8 mSrcID, EImageBW8 mDstID, EObjectSelection mBlobResult, int nInflateSize, bool bFillRect, GV_OPERATION eDstGV)
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
		internal bool PostProcessing_Vertical_Mask_Line(EImageBW8 mSrcID, EImageBW8 mDstID, EObjectSelection mBlobResult, int nInflateSize, GV_OPERATION eDstGV)
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
				rcLeft.X  = rcTmpRect.Left ; rcLeft.Width  = rcTmpRect.Width; rcLeft.Y  = rcTmpRect.Y; rcLeft.Height  = rcTmpRect.Height;
				rcRight.X = rcTmpRect.Right; rcRight.Width = rcTmpRect.Width; rcRight.Y = rcTmpRect.Y; rcRight.Height = rcTmpRect.Height;

				//MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byColor, byColor, byColor));
				//MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcLeft.Left,
				//	rcLeft.Top,
				//	rcLeft.Right,
				//	rcLeft.Bottom);
				//MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcRight.Left,
				//	rcRight.Top,
				//	rcRight.Right,
				//	rcRight.Bottom);
			}

			return true;
		}
		internal bool PostProcessing_Horizone_Mask_Line(EImageBW8 mSrcID, EImageBW8 mDstID, EObjectSelection mBlobResult, int nInflateSize, GV_OPERATION eDstGV)
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
			
				//MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byColor, byColor, byColor));
				//MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcTop.Left,
				//	rcTop.Top,
				//	rcTop.Right,
				//	rcTop.Bottom);
				//MIL.MgraLine(MIL.M_DEFAULT, mDstID, rcBtm.Left,
				//	rcBtm.Top,
				//	rcBtm.Right,
				//	rcBtm.Bottom);
			}
		
			return true;
		}
		internal void GetBlobResult_BoxRect(EObjectSelection mBlobResult, ref List<RectangleF> vBoxRect)
		{
			vBoxRect.Clear();
			
			List<double> vBoxX_Min = new List<double>();
			List<double> vBoxX_Max = new List<double>();
			List<double> vBoxY_Min = new List<double>();
			List<double> vBoxY_Max = new List<double>();

			GetBlobResult_DoubleType(mBlobResult, (int) EFeature.LeftLimit    , ref vBoxX_Min);
			GetBlobResult_DoubleType(mBlobResult, (int) EFeature.RightLimit   , ref vBoxX_Max);
			GetBlobResult_DoubleType(mBlobResult, (int) EFeature.TopLimit     , ref vBoxY_Min);
			GetBlobResult_DoubleType(mBlobResult, (int) EFeature.BottomLimit  , ref vBoxY_Max);
			
			if ((vBoxX_Min.Count != vBoxX_Max.Count) || (vBoxY_Min.Count != vBoxY_Max.Count) || (vBoxX_Min.Count != vBoxY_Min.Count)) return; // Fail...
						
			int nBlobNum = vBoxX_Min.Count;  
			for (int nCnt = 0; nCnt < nBlobNum; ++nCnt)
			{
				RectangleF rcRect = new RectangleF();
				rcRect.X		= (float)Math.Round(vBoxX_Min[nCnt]					 );
				rcRect.Y		= (float)Math.Round(vBoxY_Min[nCnt]					 );
				rcRect.Width	= (float)Math.Round(vBoxX_Max[nCnt] - vBoxX_Min[nCnt]);
				rcRect.Height	= (float)Math.Round(vBoxY_Max[nCnt] - vBoxY_Min[nCnt]);
			
				vBoxRect.Add(rcRect);
			}
		}
		internal void RectFill(EImageBW8 mDstID, RectangleF rtRect, GV_OPERATION eGVOperation)
		{
 			byte byFill = (byte)eGVOperation;
			//MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byFill, byFill, byFill));
			//MIL.MgraRectFill(MIL.M_DEFAULT, mDstID, rtRect.Left,
			//									    rtRect.Top,
			//									    rtRect.Right,
			//									    rtRect.Bottom);
		}
		internal void RectBox(EImageBW8 mDstID, RectangleF rtRect, GV_OPERATION eGVOperation)
		{
			byte byFill = (byte)eGVOperation;

			//MIL.MgraColor(MIL.M_DEFAULT, MIL.M_RGB888(byFill, byFill, byFill));
			//MIL.MgraRectFill(MIL.M_DEFAULT, mDstID, rtRect.Left,
			//										rtRect.Top,
			//										rtRect.Right,
			//										rtRect.Bottom);
		}
		internal void UnSelectBlob_Label(EImageBW8 mDstID, EObjectSelection mBlobResult, long lLabel)
		{
			mBlobResult.RemoveUsingUnsignedIntegerFeature(EFeature.ElementIndex, (uint)lLabel, ESingleThresholdMode.Equal);
		}

		internal bool Blob_Select_Position(EImageBW8 mSrcID, EImageBW8 mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
				BLOB_SELECT_POSITION_EXTRACT_FEATURE eExtractFeature, BLOB_SELECT_POSITION_EXTRACT_METHOD eExtractMethod,
				DECISION_LIST_OPER eDecisionOperType, int nExceptionBlobNum,
				BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
		{
            if (mSelection  == null) mSelection  = new EObjectSelection();
            if (mCodedImage == null) mCodedImage = new ECodedImage2();

            bool bRetVal = true;
			EvisionExtension.SetClear(mDstID);

			// [1] Init
			EImageEncoder Encoder = new EImageEncoder(); // EImageEncoder instance
			mCodedImage.ClearFeatureCache();
			Encoder.Encode(mSrcID, mCodedImage);

            mSelection.ClearFeatureCache();
			mSelection.Clear();
			mSelection.AddObjects(mCodedImage);
			//
			// [2] Valid Range
			SelectBlob(mDstID, eFeature, lMin, lMax, bMaxLimit, ref mSelection, ref mCodedImage);
			ExtractBlob_Position(mDstID, eExtractFeature, eExtractMethod, mSelection);
			//
			uint nBlobNum = mSelection.ElementCount;	
			//
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
				PostProcessing_Mask(mSrcID, mDstID, mSelection, eMaskType, eDstGV, nInflateSize);
			}

			//if (mCodedImage != null) mCodedImage.Dispose(); mCodedImage = null;
			if (Encoder     != null) Encoder.Dispose(); Encoder = null;

			return bRetVal;
		}

		internal bool Blob_Mark_GetPoint(uint mIndex, ref double dX, ref double dY, int nDirection)
		{
			uint TotalBlobs = mSelection.ElementCount;

			if (TotalBlobs <= 0 || TotalBlobs < mIndex) return false;

			double CogMinX, CogMaxX, CogMinY, CogMaxY, CogX, CogY, CogCx, CogCy;

			CogX    = (double)mSelection.GetFloatFeature(mIndex, EFeature.GravityCenterX);
			CogY    = (double)mSelection.GetFloatFeature(mIndex, EFeature.GravityCenterY);
			CogMinX = (double)mSelection.GetIntegerFeature(mIndex, EFeature.LeftLimit);
			CogMaxX = (double)mSelection.GetIntegerFeature(mIndex, EFeature.RightLimit);
			CogMinY = (double)mSelection.GetIntegerFeature(mIndex, EFeature.TopLimit);
			CogMaxY = (double)mSelection.GetIntegerFeature(mIndex, EFeature.BottomLimit);


			CogCx   = (double)mSelection.GetFloatFeature(mIndex, EFeature.BoundingBoxCenterX);
			CogCy   = (double)mSelection.GetFloatFeature(mIndex, EFeature.BoundingBoxCenterY);

			if (nDirection == 0)
			{// Left
				dX += CogMinX;
				dY += CogY;
			}
			else if (nDirection == 1)
			{// Gravity of Center
				dX += CogX;
				dY += CogY;
			}
			else if (nDirection == 2)
			{// Right
				dX += CogMaxX;
				dY += CogY;
			}
			else if (nDirection == 3)
			{// Top
				dX += CogX;
				dY += CogMinY;
			}
			else if (nDirection == 4)
			{// Bottom
				dX += CogX;
				dY += CogMaxY;
			}
			else
			{// Boundary of Center
				dX += CogCx;
				dY += CogCy;
			}

			return true;
		}

		internal bool Blob_Get_BOX_Point(uint mIndex, ref double dLX, ref double dTY, ref double dRX, ref double dBY)
		{
			uint TotalBlobs = mSelection.ElementCount;
		
			if (TotalBlobs <= 0 || TotalBlobs < mIndex) return false;

			dLX = mSelection.GetIntegerFeature(mIndex, EFeature.LeftLimit);
			dRX = mSelection.GetIntegerFeature(mIndex, EFeature.RightLimit);
			dTY = mSelection.GetIntegerFeature(mIndex, EFeature.TopLimit);
			dBY = mSelection.GetIntegerFeature(mIndex, EFeature.BottomLimit);

			return true;
		}

		internal bool Blob_Get_Count_Evision(ref uint mCount)
		{
			mCount = mSelection.ElementCount;

			return true;
		}
    }
}
