using System;
using System.Runtime.InteropServices;

using Matrox.MatroxImagingLibrary;
using Euresys.Open_eVision_22_04;
using OpenCvSharp;

using Drv.ImageProcess.Core;
using System.Drawing;
using Drv.ImageProcess.Base;
using Drv.ImageProcess.Extension;

namespace Drv.ImageProcess
{
    public class ImageProcessing : IDisposable
    {
        #region << Singleton >>
        private static readonly Lazy<ImageProcessing> _instance = new Lazy<ImageProcessing>(() => new ImageProcessing());
        public static ImageProcessing Instance { get { return _instance.Value; } }
        #endregion

        #region << Fields >>
        Rotate mRotate = new Rotate();
        Resize mResize = new Resize();
        Flip mFlip = new Flip();
        AdaptiveBin mAdaptive = new AdaptiveBin();
        Convolution mConvolution = new Convolution();
        Edge_Funcs mEdge_Funcs = new Edge_Funcs();
        Contour mContour = new Contour();
        Enhance mEnhance = new Enhance();
        MultiRegionThr mMultiRegionThr = new MultiRegionThr();
        Frequency mFrequency = new Frequency();
        Binarize mBinarize = new Binarize();
        Morphology mMorphology = new Morphology();
        Arithmatic mArithmatic = new Arithmatic();
        PatternMatching mPatternMatching = new PatternMatching();
        GMF mGMF = new GMF();
        Blob mBolb = new Blob();
        Blob_Make_MaskImg mBlob_Make_MaskImg = new Blob_Make_MaskImg();
        CalibrationFunc mCalibrationFunc = new CalibrationFunc();
        CodeReader mCodeReader = new CodeReader();
        Focus mFocus = new Focus();
        private bool disposedValue;
        #endregion

        #region << Constructor >>
        #endregion

        #region << Deconstructor >>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mBolb.Dispose();
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region << SYSYEM >>
        public static void SetPeriodTimer()
        {
            WinAPI.TIMECAPS TimeCaps = new WinAPI.TIMECAPS();
            uint time = WinAPI.timeGetDevCaps(ref TimeCaps, (uint)Marshal.SizeOf(typeof(WinAPI.TIMECAPS)));
            WinAPI.TimeBeginPeriod((uint)TimeCaps.wPeriodMin);
        }

        public static void ReleasePeriodTimer()
        {
            WinAPI.TimeEndPeriod(1);
        }

        public bool EVisionInitialize()
        {
            if (mBolb.mSelection == null) mBolb.mSelection  = new EObjectSelection();
            if (mBolb.mCodedImage == null) mBolb.mCodedImage = new ECodedImage2();

            if (mCodeReader.mCodeReader1 == null) mCodeReader.mCodeReader1 = new EMatrixCodeReader();
            if (mCodeReader.mCodeReader2 == null) mCodeReader.mCodeReader2 = new Euresys.Open_eVision_22_04.EasyMatrixCode2.EMatrixCodeReader();

            return Alloc.EVisionInitialize();
        }

        public bool EVisionDispose()
        {
            mBolb.mSelection?.Dispose();
            mBolb.mCodedImage?.Dispose();

            mCodeReader.mCodeReader1?.Dispose();
            mCodeReader.mCodeReader2?.Dispose();

            return Alloc.EVisionDispose();
        }

        public bool VisionProInitialize()
        {
            if(mBolb.mCogBlob == null) mBolb.mCogBlob = new Cognex.VisionPro.Blob.CogBlobTool();

            return Alloc.OpenCVInitialize();
        }

        public bool VisionProDispose()
        {
            mBolb.mCogBlob?.Dispose();

            return true;
        }

        public bool OpenCVInitialize()
        {
            return Alloc.OpenCVInitialize();
        }
        public bool OpenCVDispose()
        {
            return true;
        }

        public bool MilAppAlloc()
        {
            return Alloc.MilAppAlloc();
        }
        public bool MilAppDispose()
        {
            return Alloc.MilAppDispose();
        }
        public bool MilSystemAlloc()
        {
            return Alloc.MilSystemAlloc();
        }
        public bool MilSystemDispose()
        {
            return Alloc.MilSystemDispose();
        }

        #endregion

        #region << Convolution >>
        public bool Convolve(BUFF mSrcID, BUFF mDstID, EDGE_OPERATION eKernelType)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: return mConvolution.Convolve(mSrcID.buffID, mDstID.buffID, eKernelType);
                case BufferType.Evision: return mConvolution.Convolve(mSrcID.EbuffID, mDstID.EbuffID, eKernelType);
                case BufferType.Opencv: return mConvolution.Convolve(mSrcID.ObuffID, mDstID.ObuffID, eKernelType);
                default: break;
            }
            return false;
        }

        public bool SobelEdgeDetect(BUFF mSrcID, BUFF mDstID, SOBEL_OPERATION eKernelType)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil    : return mConvolution.SobelEdgeDetect(mSrcID.buffID , mDstID.buffID , (EDGE_OPERATION)eKernelType);
                case BufferType.Evision: return mConvolution.SobelEdgeDetect(mSrcID.EbuffID, mDstID.EbuffID, (EDGE_OPERATION)eKernelType);
                case BufferType.Opencv : break;
                default: break;
            }
            return false;
        }


        #endregion

        #region << Morphology >>
        public bool Morphology(BUFF mSrcID, BUFF mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil    : return mMorphology.Simple(mSrcID.buffID, mDstID.buffID, eOperation, eCondition, nNbIteration);
                case BufferType.Evision: return mMorphology.Simple(mSrcID.EbuffID, mDstID.EbuffID, eOperation, eCondition, nNbIteration);
                case BufferType.Opencv : break;
                default                : break;
            }
            return false;
        }
        public bool Morphology(BUFF mSrcID, BUFF mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil    : break;
                case BufferType.Evision: return mMorphology.SimpleShape(mSrcID.EbuffID, mDstID.EbuffID, eOperation, eCondition, eShape);
                case BufferType.Opencv : return mMorphology.SimpleShape(mSrcID.ObuffID, mDstID.ObuffID, eOperation, eCondition, eShape);
                default                : break;
            }
            return false;
        }

        public bool Kernel(BUFF mSrcID, BUFF mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition,
            MORPHOLOGY_SHAPE eShape, int nKernelHorizontial, int nKernelVertical, int niterations = 1)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mMorphology.KernelShape(mSrcID.EbuffID, mDstID.EbuffID, eOperation, eCondition, eShape, nKernelHorizontial, nKernelVertical);
                case BufferType.Opencv: return mMorphology.KernelShape(mSrcID.ObuffID, mDstID.ObuffID, eOperation, eCondition, eShape, nKernelHorizontial, nKernelVertical, niterations);
                default: break;
            }
            return false;
        }

        #endregion

        #region << Pattern Matching >>

        public bool Pattern_Matching_ALL(BUFF mSrcID, BUFF mDstID, stMatchParam stParam, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
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

        public bool Pattern_Matching_One(BUFF mSrcID, BUFF mDstID, BUFF mRefID, int nFind, PATTERNMATCHING_ACCURANCY_OPERATION nAccuracyMode,
       int nAcceptanceSet, PATTERNMATCHING_SETANGLE_OPERATION nAngleSet1, int nAngleSet_DELTA_NEG, int nAngleSet_DELTA_POS,
       out double dCenterX, out double dCenterY, out double dScore, out double dAngle, PATTERNMATCHING_OPERATION nOper)
        {
            dCenterX = 0; dCenterY = 0; dScore = 0; dAngle = 0;


            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision:
                    return mPatternMatching.Pattern_Matching_One(mSrcID.EbuffID, mDstID.EbuffID, mRefID.EbuffID, nFind, nAccuracyMode, nAcceptanceSet, nAngleSet1,
                                                             nAngleSet_DELTA_NEG, nAngleSet_DELTA_POS, ref dCenterX, ref dCenterY, ref dScore, ref dAngle, nOper);
                case BufferType.Opencv: break;
                default: break;
            }

            return false;

        }
        #endregion

        #region << GMF >>
        public bool GeometrictModelFinder(BUFF mSrcID, BUFF mDstID, string sContextPath,
            GMF_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
            out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
        {
            dCenter_X = 0; dCenter_Y = 0; dAngle = 0; dScore = 0;

            if (mSrcID.buffType != mDstID.buffType)
                return false;


            if (!mSrcID.Allocated) return false;
            if (!mDstID.Allocated) return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil       : return mGMF.GeometrictModelFinder(mSrcID.buffID, mDstID.buffID, sContextPath, bAngleSet1, nAngleSet2, nAngleSet3, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
                case BufferType.Evision   : return mGMF.GeometrictModelFinder(mSrcID.EbuffID, mDstID.EbuffID, sContextPath, bAngleSet1, nAngleSet2, nAngleSet3, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
                case BufferType.Opencv    :
               // case BufferType.VisionPro : return mGMF.GeometrictModelFinder(mSrcID.CbuffID, mDstID.CbuffID, sContextPath, bAngleSet1, nAngleSet2, nAngleSet3, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
                default: break;
            }

            return false;
        }

        public bool GeometrictModelFinder(BUFF mSrcID, BUFF mDstID, string cContextPath,
           GMF_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
           out double[] dCenter_X, out double[] dCenter_Y, out double[] dAngle, out double[] dScore, GMF_OPERATION nOper)
        {
            dCenter_X = null; dCenter_Y = null; dAngle = null; dScore = null;

            if (mSrcID.buffType != mDstID.buffType)
                return false;

            if (!mSrcID.Allocated) return false;
            if (!mDstID.Allocated) return false;

            switch (mSrcID.buffType)
            {
              //  case BufferType.VisionPro: return mGMF.GeometrictModelFinder(mSrcID.CbuffID, mDstID.CbuffID, cContextPath, bAngleSet1, nAngleSet2, nAngleSet3, out dCenter_X, out dCenter_Y, out dAngle, out dScore, nOper);
                default: break;
            }

            return false;
        }

  
        #endregion

        #region << MultiRegionThr >>
        public bool MultiRegionThresholdUsingMil(MIL_ID Mil_SrcImg, MIL_ID Mil_DstImg, int nStart, int nEnd, int nClass)
        {
            return mMultiRegionThr.MultiRegionThresholdUsingMil(Mil_SrcImg, Mil_DstImg, nStart, nEnd, nClass);
        }
        #endregion

        #region << Rotate >>
        public bool Image_Rotate(MIL_ID mSrcID, MIL_ID mDstID, double dAngle, double dSrcCenterPosX, double dSrcCenterPosY,
            double dDstCenterPosX, double dDstCenterPosY)
        {
            return mRotate.MilRotate(mSrcID, mDstID, dAngle, dSrcCenterPosX, dSrcCenterPosY, dDstCenterPosX, dDstCenterPosY);
        }
        #endregion

        #region << Flip >>
        public bool Flip(MIL_ID mSrcID, MIL_ID mDstID, IMAGE_FLIP_OPERATION eFlipType)
        {
            return mFlip.ImageFlip(mSrcID, mDstID, eFlipType);
        }
        #endregion

        #region << Blob_Make_Mask >>
        public bool Blob_Make_MaskImg(MIL_ID SrcImg, MIL_ID DstImg, BLOB_MAKE_MASKIMG_OPERATION eDirection, RectangleF rMask, BLOB_MAKE_MASKIMG_FOREGROUND_COLOR_OPERATION eForegroundColor, int nExcludeBlobSize)
        {
            return mBlob_Make_MaskImg.Blob_Make_Mask(SrcImg, DstImg, eDirection, rMask, eForegroundColor, nExcludeBlobSize);
        }
        #endregion

        #region << Binarize >>

        public bool OneLimit_Binarize(BUFF mSrcID, BUFF mDstID, BINARIZE_ONELIMIT_OPERATION eOperation, double dCondLow)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil    : return mBinarize.OneLimit_Binarize(mSrcID.buffID, mDstID.buffID, eOperation, dCondLow);
                case BufferType.Evision: return mBinarize.OneLimit_Binarize(mSrcID.EbuffID, mDstID.EbuffID, eOperation, dCondLow);
                case BufferType.Opencv : return mBinarize.OneLimit_Binarize(mSrcID.ObuffID, mDstID.ObuffID, eOperation, dCondLow);
                default: break;
            }
            return false;
        }

        public bool TwoLimit_Binarize(BUFF mSrcID, BUFF mDstID, BINARIZE_TWOLIMIT_OPERATION eOperation, double dCondLow, double dCondHigh)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil    : return mBinarize.TwoLimit_Binarize(mSrcID.buffID , mDstID.buffID , eOperation, dCondLow, dCondHigh);
                case BufferType.Evision: return mBinarize.TwoLimit_Binarize(mSrcID.EbuffID, mDstID.EbuffID, eOperation, dCondLow, dCondHigh);
                case BufferType.Opencv : return mBinarize.TwoLimit_Binarize(mSrcID.ObuffID, mDstID.ObuffID, eOperation, dCondLow, dCondHigh);
                default: break;
            }
            return false;
        }

        public bool HistoLimit_Binarize(BUFF mSrcID, BUFF mDstID, BINARIZE_HISTO_OPERATION eOperation_Histo,
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

        public bool OtsuBinarize(BUFF mSrcID, BUFF mDstID)
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

        public bool SigmaBinarize(BUFF mSrcID, BUFF mMskID, BUFF mDstID, double dPosSigma, double dNegSigma, SIGMA_BINARIZE_OPERATION eOper)
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
        public bool EdgeDetect(BUFF mSrcID, BUFF mDstID, EDGE_TYPE mType = EDGE_TYPE.E_ALL)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            if (!mSrcID.Allocated) return false;
            if (!mDstID.Allocated) return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil      : return mEdge_Funcs.EdgeDetect(mSrcID.buffID , mDstID.buffID);
                case BufferType.Evision  : return mEdge_Funcs.EdgeDetect(mSrcID.EbuffID, mDstID.EbuffID);
                case BufferType.Opencv   : return mEdge_Funcs.EdgeDetect(mSrcID.ObuffID, mDstID.ObuffID, mType);
                case BufferType.Halcon   : break;
                case BufferType.VisionPro: break;
                default: break;
            }
            return false;
        }
 
        public bool FindCircle(BUFF mSrcID, BUFF mDstID, stFittingCircleParam stFindCircle, ref CircleF stCircle)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            if (!mSrcID.Allocated) return false;
            if (!mDstID.Allocated) return false;


            switch (mSrcID.buffType)
            {
                case BufferType.Mil       : break;
                case BufferType.Evision   : break;
                case BufferType.Opencv    : break;
                case BufferType.Halcon    : break;
                //case BufferType.VisionPro : return mEdge_Funcs.FindCircle(mSrcID.CbuffID, mDstID.CbuffID, stFindCircle, ref stCircle);
                default                   : break;
            } 
            return false;
        }

        public bool FindLine(BUFF mSrcID, BUFF mDstID, stFittingLineParam stFindLine, FIND_ORIENTATION eOriant, ref Line2D pLine)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            if (!mSrcID.Allocated) return false;
            if (!mDstID.Allocated) return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: break;
                case BufferType.Opencv: return mEdge_Funcs.FindLine(mSrcID.ObuffID, mDstID.ObuffID, stFindLine, eOriant, ref pLine);
                case BufferType.Halcon: break;
                //case BufferType.VisionPro: return mEdge_Funcs.FindLine(mSrcID.CbuffID, mDstID.CbuffID, stFindLine, eOriant, ref pLine);
                default: break;
            }
            return false;
        }

        public bool FindRectangle(BUFF mSrcID, BUFF mDstID, stFittingRectParam stFindRect, ref RotatedRect stRect)
        {
            if (mSrcID.buffType != mDstID.buffType)
                return false;

            if (!mSrcID.Allocated) return false;
            if (!mDstID.Allocated) return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mEdge_Funcs.FindRectangle(mSrcID.EbuffID, mDstID.EbuffID, stFindRect, ref stRect);
                case BufferType.Opencv: break;
                default: break;
            }
            return false;
        }

        public bool ContourTrace(BUFF mSrcID, out ContourPoints[] cp,
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

        #endregion

        #region << Blob >>
        public bool Blob_ReconstructFromSeed(MIL_ID mSrcID, MIL_ID mSeedID, MIL_ID mDstID, MORPHOLOGY_CONDITION eCondition)
        {
            return mBolb.Blob_ReconstructFromSeed(mSrcID, mSeedID, mDstID, eCondition);
        }
        public bool Blob_FillHole(MIL_ID mSrcID, MIL_ID mDstID)
        {
            return mBolb.Blob_FillHole(mSrcID, mDstID);
        }
        public bool Blob_EraseBorder(MIL_ID mSrcID, MIL_ID mDstID)
        {
            return mBolb.Blob_EraseBorder(mSrcID, mDstID);
        }
        public bool Blob_ConvexHull(MIL_ID mSrcID, MIL_ID mDstID, CONVEXHULL_TYPE eConvexHullType, bool bUseParallel = false)
        {
            return mBolb.Blob_ConvexHull(mSrcID, mDstID, eConvexHullType, bUseParallel);
        }
        public bool Blob_Select_Geometry(MIL_ID mSrcID, MIL_ID mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
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

        public bool Blob_Get_Count(BUFF mSrcID, ref uint mCount)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil:
                    break;
                case BufferType.Evision  : return mBolb.Blob_Get_Count_Evision(ref mCount);
                case BufferType.VisionPro: return mBolb.Blob_Get_Count_VisionPro(ref mCount);
                case BufferType.Opencv   : break;
                default: break;
            }

            return false;
        }

        public bool Blob_Get_BOX_Point(BufferType bufferType, uint mIndex, ref double dLX, ref double dTY, ref double dRX, ref double dBY)
        {
            switch (bufferType)
            {
                case BufferType.Mil:
                    break;
                case BufferType.Evision : return mBolb.Blob_Get_BOX_Point(mIndex, ref dLX, ref dTY, ref dRX, ref dBY);
                case BufferType.Opencv  : break;
                default: break;
            }

            return false;
        }

        public bool Blob_Mark_GetPoint(BufferType bufferType, uint mIndex, ref double dX, ref double dY, BLOB_MARK_DIRECTION nDirection)
        {
            switch (bufferType)
            {
                case BufferType.Mil:
                    break;
                case BufferType.Evision : return mBolb.Blob_Mark_GetPoint(mIndex, ref dX, ref dY, (int)nDirection);
                case BufferType.Opencv  : break;
                default: break;
            }

            return false;
        }

        public bool Blob_Select_Position(BUFF mSrcID, BUFF mDstID, BLOB_COMMON_FEATURE eFeature, double lMin, double lMax, bool bMaxLimit,
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

        public bool CodeRead(BUFF mSrcID, BUFF mDstID, string cContextPath, CODE_MODE nMode, uint iTimeOut, out string sDecoded)
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

        public bool CodeRead(BUFF mSrcID, BUFF mDstID, CODE_MODE nMode, uint iTimeOut, out string sDecoded)
        {
            sDecoded = string.Empty;

            if (mSrcID.buffType != mDstID.buffType)
                return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision:
                    {
                        if (!mSrcID.UseROI) return mCodeReader.CodeRead(mSrcID.EbuffID, mDstID.EbuffID, nMode, iTimeOut, out sDecoded);
                        else return mCodeReader.CodeRead(EvisionExtension.SetROI(mSrcID).TopParent, mDstID.EbuffID, nMode, iTimeOut, out sDecoded);
                    }
                case BufferType.Opencv: break;
                default: break;
            }
            return false;
        }




        public bool CodeLoad(BufferType mbuffType, string sPath)
        {
            switch (mbuffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mCodeReader.CodeLoad(sPath);
                case BufferType.Opencv: break;
                default: break;
            }

            return false;
        }

        public bool CodeSave(BufferType mbuffType, string sPath)
        {
            switch (mbuffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mCodeReader.CodeSave(sPath);
                case BufferType.Opencv: break;
                default: break;
            }

            return false;
        }
        #endregion

        #region << Focus >>
        public bool Focus(BUFF mSrcID, Rect mRegion, ref double dRobbery)
        {
            if (!mSrcID.Allocated) return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: break;
                case BufferType.Opencv: break;
                case BufferType.Halcon: break;
                //case BufferType.VisionPro: return mFocus.Focusing(mSrcID.CbuffID, mRegion, ref dRobbery);
                default: break;
            }

            return true;
        }

        public bool Focus(BUFF mSrcID, ref double dRobbery)
        {
            if (!mSrcID.Allocated) return false;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: break;
                case BufferType.Opencv: break;
                case BufferType.Halcon: break;
             //   case BufferType.VisionPro: return mFocus.Focusing(mSrcID.CbuffID, mSrcID.UseROI ? mSrcID.ROI : new Rect(0,0, mSrcID.wid, mSrcID.len), ref dRobbery);
                default: break;
            }

            return true;
        }

        public bool AutoFocus(BUFF[] mSrcID, Rect mRegion, ref int pIndex)
        {
            if (mSrcID.Length < 1) return false;
            if (!mSrcID[0].Allocated) return false;

            switch (mSrcID[0].buffType)
            {
                case BufferType.Mil      : break;
                case BufferType.Evision  : break;
                case BufferType.Opencv   : break;
                case BufferType.Halcon   : break;
               // case BufferType.VisionPro: return mFocus.AutoFocusing(VisionProExtension.ToArray(mSrcID), mRegion, ref pIndex);
                default: break;
            }

            return true;
        }

        #endregion

        #region << Resize >>
        public bool ImageResize(BUFF mSrcID, BUFF mDstID)
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

        public bool ImageCrop(BUFF mSrcID, BUFF mDstID, Rect mRect)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil    : break;
                case BufferType.Evision: return mResize.ImageCrop(mSrcID.EbuffID, mDstID.EbuffID, mRect);
                case BufferType.Opencv : return mResize.ImageCrop(mSrcID.ObuffID, mDstID.ObuffID, mRect);
                default: break;
            }

            return true;
        }
        #endregion

        #region << Arithmetic >>
        public bool Arith_OneCalcurate(BUFF mSrcID, ARITH_1_OPERATION eOper, out float dValue)
        {
            dValue = 0.0f;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mArithmatic.Arith_OneCalcurate(mSrcID.EbuffID, eOper, out dValue);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }

        public bool Arith_OneLogic(BUFF mSrcID, BUFF mDstID, LOGIC_1_OPERATION eOper)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mArithmatic.Arith_OneLogic(mSrcID.EbuffID, mDstID.EbuffID, eOper);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }

        public bool Arith_Projection(BUFF mSrcID, BUFF mDstID, ARITH_PROJECT_DIR eDir)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mArithmatic.Arith_Projection(mSrcID.EbuffID, mDstID, eDir);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }

        public bool Arith_GravityCenter(BUFF mSrcID, int iCondLow, out Point2f ptCenter)
        {
            ptCenter = new Point2f();

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mArithmatic.Arith_GravityCenter(mSrcID.EbuffID, iCondLow, out ptCenter);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }

        public bool Arith_PixelCount(BUFF mSrcID, ARITH_PIXEL_OPERATION eOper, int iCondLow, int iCondHigh, out int iPixels)
        {
            iPixels = 0;

            switch (mSrcID.buffType)
            {
                case BufferType.Mil: break;
                case BufferType.Evision: return mArithmatic.Arith_PixelCount(mSrcID.EbuffID, eOper, iCondLow, iCondHigh, out iPixels);
                case BufferType.Opencv: break;
                default: break;
            }

            return true;
        }

        #endregion

        #region << Enhance >>
        public bool ImageGammaOffset(BUFF mSrcID, BUFF mDstID, double dGain, int nOffset, bool bUseParallel = false)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil      : break;
                case BufferType.Evision  : return mEnhance.ImageGammaOffset(mSrcID.EbuffID, mDstID.EbuffID, dGain, nOffset, bUseParallel);
                case BufferType.Opencv   : break;
                case BufferType.Halcon   : return mEnhance.ImageGammaOffset(mSrcID.HbuffID, mDstID.HbuffID, dGain, nOffset, bUseParallel);
                case BufferType.VisionPro: break;
                default: break;
            }

            return true;
        }

        public  bool ImageScaleMax(BUFF mSrcID, BUFF mDstID)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil      : break;
                case BufferType.Evision  : return mEnhance.ImageScaleMax(mSrcID.EbuffID, mDstID.EbuffID);
                case BufferType.Opencv   : break;
                case BufferType.Halcon   : return mEnhance.ImageScaleMax(mSrcID.HbuffID, mDstID.HbuffID);
                case BufferType.VisionPro: break;
                default: break;
            }

            return true;
        }

        public bool ImageIlluminate(BUFF mSrcID, BUFF mDstID, int nKernelHorizontial = 7, int nKernelVertical = 7, double dGain = 1.0)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil      : break;
                case BufferType.Evision  : return mEnhance.ImageIlluminate(mSrcID.EbuffID, mDstID.EbuffID, nKernelHorizontial, nKernelVertical, dGain);
                case BufferType.Opencv   : break;
                case BufferType.Halcon   : return mEnhance.ImageIlluminate(mSrcID.HbuffID, mDstID.HbuffID, nKernelHorizontial, nKernelVertical, dGain);
                case BufferType.VisionPro: break;
                default: break;
            }

            return true;
        }

        public bool ImageEmphasize(BUFF mSrcID, BUFF mDstID, int nKernelHorizontial = 7, int nKernelVertical = 7, double dGain = 1.0)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil      : break;
                case BufferType.Evision  : return mEnhance.ImageEmphasize(mSrcID.EbuffID, mDstID.EbuffID, nKernelHorizontial, nKernelVertical, dGain);
                case BufferType.Opencv   : break;
                case BufferType.Halcon   : return mEnhance.ImageEmphasize(mSrcID.HbuffID, mDstID.HbuffID, nKernelHorizontial, nKernelVertical, dGain);
                case BufferType.VisionPro: break;
                default: break;
            }

            return true;
        }
        public bool ImageFlatFieldCorrection(BUFF mSrcID, BUFF mDstID)
        {
            switch (mSrcID.buffType)
            {
                case BufferType.Mil      : break;
                case BufferType.Evision  : return mEnhance.ImageFlatFieldCorrection(mSrcID.EbuffID, mDstID.EbuffID);
                case BufferType.Opencv   : break;
                case BufferType.Halcon   : break;
                case BufferType.VisionPro: break;
                default: break;
            }

            return true;
        }
        #endregion
    }
}
