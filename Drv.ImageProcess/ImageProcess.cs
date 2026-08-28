using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;
using Drv.ImageProcess.Core;
using Euresys.Open_eVision_22_04;
using OpenCvSharp;
using System.Runtime.InteropServices;
using Drv.ImageProcess.Extension;

namespace Drv.ImageProcess
{
    public static class ImageProcess
    {
        #region << Fields >>
        static Rotate mRotate = new Rotate();
        static Resize mResize = new Resize();
        static Flip mFlip = new Flip();
        static AdaptiveBin mAdaptive = new AdaptiveBin();
        static Convolution mConvolution = new Convolution();
        static Edge_Funcs mEdge_Funcs = new Edge_Funcs();
        static Contour mContour = new Contour();
        static Enhance mEnhance = new Enhance();
        static MultiRegionThr mMultiRegionThr = new MultiRegionThr();
        static Frequency mFrequency = new Frequency();
        static Binarize mBinarize = new Binarize();
        static Morphology mMorphology = new Morphology();
        static Arithmatic mArithmatic = new Arithmatic();
        static PatternMatching mPatternMatching = new PatternMatching();
        static GMF mGMF = new GMF();
        static Blob mBolb = new Blob();
        static Blob_Make_MaskImg mBlob_Make_MaskImg = new Blob_Make_MaskImg();
        static CalibrationFunc mCalibrationFunc = new CalibrationFunc();
        static CodeReader mCodeReader = new CodeReader();
        static Focus mFocus = new Focus();
        private static bool disposedValue;

        #endregion

        #region << Constructor >>
        static ImageProcess()
        {
            WinAPI.TIMECAPS TimeCaps = new WinAPI.TIMECAPS();
            uint time = WinAPI.timeGetDevCaps(ref TimeCaps, (uint)Marshal.SizeOf(typeof(WinAPI.TIMECAPS)));
            WinAPI.TimeBeginPeriod((uint)TimeCaps.wPeriodMin);
        }
        #endregion

        #region << Deconstructor >>
        public static void Dispose()
        {
            WinAPI.TimeEndPeriod(1);
        }
        #endregion

        #region << SYSYEM >>

        public static bool EVisionInitialize()
        {
            return Alloc.EVisionInitialize();
        }

        public static bool EVisionDispose()
        {
            return Alloc.EVisionDispose();
        }
        public static bool MilAppAlloc()
        {
            return Alloc.MilAppAlloc();
        }
        public static bool MilAppDispose()
        {
            return Alloc.MilAppDispose();
        }

        public static bool MilSystemAlloc()
        {
            return Alloc.MilSystemAlloc();
        }
        public static bool MilSystemDispose()
        {
            return Alloc.MilSystemDispose();
        }
        #endregion

        #region << Convolution >>
        public static bool SobelEdgeDetect(MIL_ID nsrcImg, MIL_ID ndstImg, EDGE_OPERATION eKernelType)
        {
            return mConvolution.SobelEdgeDetect(nsrcImg, ndstImg, eKernelType);
        }
        public static bool Convolve(MIL_ID mSrcID, MIL_ID mDstID, EDGE_OPERATION eKernelType)
        {
            return mConvolution.Convolve(mSrcID, mDstID, eKernelType);
        }
        #endregion

        #region << Morphology >>
        public static bool Morphology(BUFF mSrcID, BUFF mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mMorphology.Simple(mSrcID.buffID, mDstID.buffID, eOperation, eCondition, nNbIteration);
                case BufferType.Evision: return mMorphology.Simple(mSrcID.EbuffID, mDstID.EbuffID, eOperation, eCondition, nNbIteration);
                case BufferType.Opencv: break;
                default: break;
            }
            return false;
        }
        #endregion

        #region << Pattern Matching >>

        public static bool Pattern_Matching_ALL(BUFF mSrcID, BUFF mDstID, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
            int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
            out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
        {
            dCenterX = 0; dCenterY = 0; dScore = 0; dAngle = 0;

            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil:
                    return mPatternMatching.Pattern_Matching_ALL(mSrcID.buffID, mDstID.buffID, stParam, nFind, nAccuracyMode, nAcceptanceSet, nAngleSet1,
                                                             nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, out dCenterX, out dCenterY, out dScore, out dAngle, nOper);
                case BufferType.Evision:
                    return mPatternMatching.Pattern_Matching_ALL(mSrcID.EbuffID, mDstID.EbuffID, stParam, nFind, nAccuracyMode, nAcceptanceSet, nAngleSet1,
                                                             nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, out dCenterX, out dCenterY, out dScore, out dAngle, nOper);
                case BufferType.Opencv:
                    return mPatternMatching.Pattern_Matching_ALL(mSrcID.ObuffID, mDstID.ObuffID, stParam, nFind, nAccuracyMode, nAcceptanceSet, nAngleSet1,
                                                             nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, out dCenterX, out dCenterY, out dScore, out dAngle, nOper);
                default: break;
            }

            return false;
        }

        public static bool Pattern_Matching_ALL(MIL_ID SrcImg, MIL_ID DstImg, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
           int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
           out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
        {
            return mPatternMatching.Pattern_Matching_ALL(SrcImg, DstImg, stParam, nFind, nAccuracyMode, nAcceptanceSet, nAngleSet1,
                nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, out dCenterX, out dCenterY, out dScore, out dAngle, nOper);
        }

        public static bool Pattern_Matching_ALL(EImageBW8 SrcImg, EImageBW8 DstImg, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
            int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
            out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
        {
            return mPatternMatching.Pattern_Matching_ALL(SrcImg, DstImg, stParam, nFind, nAccuracyMode, nAcceptanceSet, nAngleSet1,
                nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, out dCenterX, out dCenterY, out dScore, out dAngle, nOper);
        }

        public static bool Pattern_Matching_ALL(Mat SrcImg, Mat DstImg, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
            int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
            out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
        {
            return mPatternMatching.Pattern_Matching_ALL(SrcImg, DstImg, stParam, nFind, nAccuracyMode, nAcceptanceSet, nAngleSet1,
                nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, out dCenterX, out dCenterY, out dScore, out dAngle, nOper);
        }
        #endregion

        #region << GMF >>
        public static bool GeometrictModelFinder(BUFF mSrcID, BUFF mDstID, string cContextPath,
            GMF_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
            out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
        {
            dCenter_X = 0; dCenter_Y = 0; dAngle = 0; dScore = 0;

            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mGMF.GeometrictModelFinder(mSrcID.buffID, mDstID.buffID, cContextPath, bAngleSet1, nAngleSet2, nAngleSet3, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
                case BufferType.Evision: return mGMF.GeometrictModelFinder(mSrcID.EbuffID, mDstID.EbuffID, cContextPath, bAngleSet1, nAngleSet2, nAngleSet3, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
                case BufferType.Opencv:
                default: break;
            }

            return false;
        }

        //public static bool GeometrictModelFinder(MIL_ID nSrcImg, MIL_ID nDstImg, string cContextPath, out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
        //{
        //    return mGMF.GeometrictModelFinder(nSrcImg, nDstImg, cContextPath, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
        //}
        //public static BUFF_INFO GeometrictModelDefine(MIL_ID nSrcImg, RectangleF ROI)
        //{
        //    return mGMF.GeometrictModelDefine(nSrcImg, ROI);
        //}
        //public static bool GeometrictModelSave(MIL_ID nSrcImg, string sPath)
        //{
        //    return mGMF.GeometrictModelSave(nSrcImg, sPath);
        //}

        //public static bool GeometrictModelFinder(Mat nSrcImg, Mat nDstImg, string cContextPath, out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
        //{
        //    return mGMF.GeometrictModelFinder(nSrcImg, nDstImg, cContextPath, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
        //}

        //public static bool FeatureModelFinder(Mat nSrcImg, Mat nDstImg, string cContextPath, out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
        //{
        //    return mGMF.FeatureModelFinder(nSrcImg, nDstImg, cContextPath, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
        //}

        #endregion

        #region << MultiRegionThr >>
        public static bool MultiRegionThresholdUsingMil(MIL_ID Mil_SrcImg, MIL_ID Mil_DstImg, int nStart, int nEnd, int nClass)
        {
            return mMultiRegionThr.MultiRegionThresholdUsingMil(Mil_SrcImg, Mil_DstImg, nStart, nEnd, nClass);
        }
        #endregion

        #region << Rotate >>
        public static bool Image_Rotate(MIL_ID mSrcID, MIL_ID mDstID, double dAngle, double dSrcCenterPosX, double dSrcCenterPosY,
            double dDstCenterPosX, double dDstCenterPosY)
        {
            return mRotate.MilRotate(mSrcID, mDstID, dAngle, dSrcCenterPosX, dSrcCenterPosY, dDstCenterPosX, dDstCenterPosY);
        }
        #endregion

        #region << Flip >>
        public static bool Flip(MIL_ID mSrcID, MIL_ID mDstID, IMAGE_FLIP_OPERATION eFlipType)
        {
            return mFlip.ImageFlip(mSrcID, mDstID, eFlipType);
        }
        #endregion

        #region << Blob_Make_Mask >>
        public static bool Blob_Make_MaskImg(MIL_ID SrcImg, MIL_ID DstImg, BLOB_MAKE_MASKIMG_OPERATION eDirection, RectangleF rMask, BLOB_MAKE_MASKIMG_FOREGROUND_COLOR_OPERATION eForegroundColor, int nExcludeBlobSize)
        {
            return mBlob_Make_MaskImg.Blob_Make_Mask(SrcImg, DstImg, eDirection, rMask, eForegroundColor, nExcludeBlobSize);
        }
        #endregion

        #region << Binarize >>

        public static bool OneLimit_Binarize(BUFF mSrcID, BUFF mDstID, BINARIZE_ONELIMIT_OPERATION eOperation, double dCondLow)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mBinarize.OneLimit_Binarize(mSrcID.buffID, mDstID.buffID, eOperation, dCondLow);
                case BufferType.Evision: return mBinarize.OneLimit_Binarize(mSrcID.EbuffID, mDstID.EbuffID, eOperation, dCondLow);
                case BufferType.Opencv: return mBinarize.OneLimit_Binarize(mSrcID.ObuffID, mDstID.ObuffID, eOperation, dCondLow);
                default: break;
            }
            return false;
        }

        public static bool TwoLimit_Binarize(BUFF mSrcID, BUFF mDstID, BINARIZE_TWOLIMIT_OPERATION eOperation, double dCondLow, double dCondHigh)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mBinarize.TwoLimit_Binarize(mSrcID.buffID, mDstID.buffID, eOperation, dCondLow, dCondHigh);
                case BufferType.Evision: break;
                case BufferType.Opencv: return mBinarize.TwoLimit_Binarize(mSrcID.ObuffID, mDstID.ObuffID, eOperation, dCondLow, dCondHigh);
                default: break;
            }
            return false;
        }

        public static bool HistoLimit_Binarize(BUFF mSrcID, BUFF mDstID, BINARIZE_HISTO_OPERATION eOperation_Histo,
          BINARIZE_ONELIMIT_OPERATION eOperation_OneLimit,
          double dCondLow, double dCondHigh)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mBinarize.HistoLimit_Binarize(mSrcID.buffID, mDstID.buffID, eOperation_Histo, eOperation_OneLimit, dCondLow, dCondHigh);
                case BufferType.Evision: break;
                case BufferType.Opencv: return mBinarize.HistoLimit_Binarize(mSrcID.ObuffID, mDstID.ObuffID, eOperation_Histo, eOperation_OneLimit, dCondLow, dCondHigh);
                default: break;
            }
            return false;
        }

        public static bool OtsuBinarize(BUFF mSrcID, BUFF mDstID)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mBinarize.OtsuBinarize(mSrcID.buffID, mDstID.buffID);
                case BufferType.Evision: break;
                case BufferType.Opencv: return mBinarize.OtsuBinarize(mSrcID.ObuffID, mDstID.ObuffID);
                default: break;
            }
            return false;
        }

        public static bool SigmaBinarize(BUFF mSrcID, BUFF mMskID, BUFF mDstID, double dPosSigma, double dNegSigma, SIGMA_BINARIZE_OPERATION eOper)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mBinarize.SigmaBinarize(mSrcID.buffID, mMskID.buffID, mDstID.buffID, dPosSigma, dNegSigma, eOper);
                case BufferType.Evision: break;
                case BufferType.Opencv: return mBinarize.SigmaBinarize(mSrcID.ObuffID, mMskID.ObuffID, mDstID.ObuffID, dPosSigma, dNegSigma, eOper);
                default: break;
            }
            return false;
        }

        #endregion

        #region << Edge >>
        public static bool EdgeDetect(MIL_ID mSrcID, MIL_ID mIntensityDstImg)
        {
            return mEdge_Funcs.EdgeDetect(mSrcID, mIntensityDstImg);
        }
        public static bool FindCircle(MIL_ID mSrc, MIL_ID mDst, stFittingParam stFindCircle, ref PointF stCenterpt, int nFindRadius)
        {
            return mEdge_Funcs.FindCircle(mSrc, mDst, stFindCircle, ref stCenterpt, nFindRadius);
        }

        public static bool FindRectangle(BUFF mSrcID, BUFF mDstID, stFittingRectParam stFindRect, ref RotatedRect stRect)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mEdge_Funcs.FindRectangle(mSrcID.EbuffID, mDstID.EbuffID, stFindRect, ref stRect);
                case BufferType.Opencv: break;
                default: break;
            }
            return false;
        }

        //public static bool EdgeDetect(Mat mSrcID, Mat mIntensityDstImg)
        //{
        //    return mEdge_Funcs.EdgeDetect(mSrcID, mIntensityDstImg);
        //}
        public static bool ContourTrace(BUFF mSrcID, out ContourPoints[] cp,
         RETRIVAL_MODE mode1 = RETRIVAL_MODE.E_CCOMP, APPROXIMATION_MODE mode2 = APPROXIMATION_MODE.E_APPROXSIMPLE)
        {
            cp = null;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: break;
                case BufferType.Opencv: return mContour.ContourTrace(mSrcID.ObuffID, out cp, (RetrievalModes)mode1, (ContourApproximationModes)mode2);
                default: break;
            }
            return false;
        }

        public static bool ContourTrace(Mat mSrcID, out ContourPoints[] cp,
            RETRIVAL_MODE mode1 = RETRIVAL_MODE.E_CCOMP, APPROXIMATION_MODE mode2 = APPROXIMATION_MODE.E_APPROXSIMPLE)
        {
            return mContour.ContourTrace(mSrcID, out cp, (RetrievalModes)mode1, (ContourApproximationModes)mode2);
        }

        #endregion

        #region << Blob >>
        public static bool Blob_ReconstructFromSeed(MIL_ID mSrcID, MIL_ID mSeedID, MIL_ID mDstID, MORPHOLOGY_CONDITION eCondition)
        {
            return mBolb.Blob_ReconstructFromSeed(mSrcID, mSeedID, mDstID, eCondition);
        }
        public static bool Blob_FillHole(MIL_ID mSrcID, MIL_ID mDstID)
        {
            return mBolb.Blob_FillHole(mSrcID, mDstID);
        }
        public static bool Blob_EraseBorder(MIL_ID mSrcID, MIL_ID mDstID)
        {
            return mBolb.Blob_EraseBorder(mSrcID, mDstID);
        }
        public static bool Blob_ConvexHull(MIL_ID mSrcID, MIL_ID mDstID, CONVEXHULL_TYPE eConvexHullType, bool bUseParallel = false)
        {
            return mBolb.Blob_ConvexHull(mSrcID, mDstID, eConvexHullType, bUseParallel);
        }
        public static bool Blob_Select_Geometry(MIL_ID mSrcID, MIL_ID mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
            BLOB_SELECT_GEOMETRY_EXTRACT_METHOD eExtractMethod,
            DECISION_LIST_OPER eDecisionOperType, int nExceptionBlobNum,
            BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
        {
            return mBolb.Blob_Select_Geometry(mSrcID, mDstID, eFeature, lMin, lMax, bMaxLimit, eExtractMethod,
                eDecisionOperType, nExceptionBlobNum, eMaskType, eDstGV, nInflateSize);
        }
        //public static bool Blob_Select_Position(MIL_ID mSrcID, MIL_ID mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
        //    BLOB_SELECT_POSITION_EXTRACT_FEATURE eExtractFeature,
        //    BLOB_SELECT_POSITION_EXTRACT_METHOD eExtractMethod,
        //    DECISION_LIST_OPER eDecisionOperType, int nExceptionBlobNum,
        //    BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
        //{
        //    return mBolb.Blob_Select_Position(mSrcID, mDstID, eFeature, lMin, lMax, bMaxLimit, eExtractFeature,
        //        eExtractMethod, eDecisionOperType, nExceptionBlobNum, eMaskType, eDstGV, nInflateSize);
        //}

        public static bool Blob_Get_Count(BUFF mSrcID, ref uint mCount)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil:
                    break;
               // case BufferType.Evision:
               //     return mBolb.Blob_Get_Count(ref mCount);
                case BufferType.Opencv: break;
                default: break;
            }

            return false;
        }

        public static bool Blob_Get_BOX_Point(BUFF mSrcID, uint mIndex, ref double dLX, ref double dTY, ref double dRX, ref double dBY)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil:
                    break;
               // case BufferType.Evision:
               //     return mBolb.Blob_Get_BOX_Point(mIndex, ref dLX, ref dTY, ref dRX, ref dBY);
                case BufferType.Opencv: break;
                default: break;
            }

            return false;
        }

        public static bool Blob_Mark_GetPoint(BUFF mSrcID, uint mIndex, ref double dX, ref double dY, BLOB_MARK_DIRECTION nDirection)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil:
                    break;
               // case BufferType.Evision:
               //     return mBolb.Blob_Mark_GetPoint(mIndex, ref dX, ref dY, (int)nDirection);
                case BufferType.Opencv: break;
                default: break;
            }

            return false;
        }

        public static bool Blob_Select_Position(BUFF mSrcID, BUFF mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
         BLOB_SELECT_POSITION_EXTRACT_FEATURE eExtractFeature, BLOB_SELECT_POSITION_EXTRACT_METHOD eExtractMethod,
         DECISION_LIST_OPER eDecisionOperType, int nExceptionBlobNum, BLOB_MASK eMaskType, GV_OPERATION eDstGV, int nInflateSize)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil:
                    return mBolb.Blob_Select_Position(mSrcID.buffID, mDstID.buffID, eFeature, lMin, lMax, bMaxLimit, eExtractFeature,
                 eExtractMethod, eDecisionOperType, nExceptionBlobNum, eMaskType, eDstGV, nInflateSize);
                case BufferType.Evision:
                    {
                        if (!mSrcID.UseROI) return mBolb.Blob_Select_Position(mSrcID.EbuffID, mDstID.EbuffID, eFeature, lMin, lMax, bMaxLimit, eExtractFeature,
                                                    eExtractMethod, eDecisionOperType, nExceptionBlobNum, eMaskType, eDstGV, nInflateSize);
                        else return mBolb.Blob_Select_Position(EvisionExtension.SetROI(mSrcID) as EImageBW8, mDstID.EbuffID, eFeature, lMin, lMax, bMaxLimit, eExtractFeature,
                                     eExtractMethod, eDecisionOperType, nExceptionBlobNum, eMaskType, eDstGV, nInflateSize);
                    }
                case BufferType.Opencv: break;
                default: break;
            }

            return false;
        }
        #endregion

        #region << Barcode >>

        public static bool CodeRead(BUFF mSrcID, BUFF mDstID, string cContextPath, CODE_MODE nMode, uint iTimeOut, out string sDecoded)
        {
            sDecoded = string.Empty;

            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision:
                    {
                        if (!mSrcID.UseROI) return mCodeReader.CodeRead(mSrcID.EbuffID, mDstID.EbuffID, cContextPath, nMode, iTimeOut, out sDecoded);
                        else return mCodeReader.CodeRead(EvisionExtension.SetROI(mSrcID).TopParent, mDstID.EbuffID, cContextPath, nMode, iTimeOut, out sDecoded);
                    }
                case BufferType.Opencv: break;
                default: break;
            }
            return false;
        }

        #endregion

        #region << Focus >>
        public static bool Focus(BUFF mSrcID, out double dRobbery)
        {
            dRobbery = 0;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mFocus.Focusing(mSrcID.EbuffID, out dRobbery);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }

        #endregion

        #region << Resize >>
        public static bool ImageResize(BUFF mSrcID, BUFF mDstID)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mResize.ImageResize(mSrcID.buffID, mDstID.buffID);
                case BufferType.Evision: return mResize.ImageResize(mSrcID.EbuffID, mDstID.EbuffID);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }

        public static bool ImageCrop(BUFF mSrcID, BUFF mDstID, Rect mRect)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mResize.ImageCrop(mSrcID.EbuffID, mDstID.EbuffID, mRect);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }
        #endregion
        public static void TEST(BUFF src, BUFF dest)
        {

        }

        public static void TEST()
        {
            EImageBW8 mImage = new EImageBW8(100, 100);
            mImage.Save($"C:\\Users\\DAx2\\Desktop\\TnR\\EImageBW8.bmp");
            mImage.Dispose();


            EImageBW8 mDstID = new EImageBW8(1138, 258);
            for (int i = 0; i < 100000000; i++)                                  //반복 테스트
            {
                byte[] text = new byte[1138 * 258];
                IntPtr CopyPtr = Marshal.AllocHGlobal(1138 * 258);              //이미지 크기와 동일한 크기의 포인터 생성
                MemoryExtension.MemSet(CopyPtr, 100, 1138 * 258);               //포인터 특정픽셀로 Clear
                mDstID.SetImagePtr(1138, 258, CopyPtr);                         //클리어 한 포인터을 이미지 포인터에 셋팅
                //mDstID.Save($"C:\\Users\\DAx2\\Desktop\\TnR\\SetImagePtr.bmp");//이미지 저장
                Marshal.FreeHGlobal(CopyPtr);      //포인터 릴리즈
                text = null;
                //GC.Collect();
            }
            mDstID.Dispose();                                                 //이미지 릴리즈

            // EasyImage.Contour
        }
    }
}
