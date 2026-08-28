using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Drv.ImageProcess.Core
{
	internal partial class Morphology
    {
		internal bool Simple(Mat mSrcID, Mat mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
		{
			bool ret = false;
			//////////////////////////////////////////////////////////////////////////
			// Todo : MorphologyLine3 함수는 제거 예정이지만
			// 현제 작성된 구조는 마지막 파라메터를 필요로 하지않음
			// HORZ에 대한 구조체가 정의 되어있기 때문
			switch (eOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE        :
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE         :
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN          :
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					MorphologyEx(mSrcID, mDstID, eOperation, eCondition, nNbIteration);
					ret = true;
					break;
			}

			
			return ret;
		}
        internal bool SimpleShape(Mat mSrcID, Mat mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape)
        {
            bool ret = false;
            //////////////////////////////////////////////////////////////////////////
            // Todo : MorphologyLine3 함수는 제거 예정이지만
            // 현제 작성된 구조는 마지막 파라메터를 필요로 하지않음
            // HORZ에 대한 구조체가 정의 되어있기 때문
            switch (eOperation)
            {
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
                    MorphologyEx(mSrcID, mDstID, eOperation, eCondition, eShape);
                    ret = true;
                    break;
            }

            return ret;
        }

		internal bool KernelShape(Mat mSrcID, Mat mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape,
			int nKernelHorizontial, int nKernelVertical, int niterations)
		{

            MorphTypes types = MorphTypes.HitMiss;
            switch (eOperation)
            {
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
                    types = MorphTypes.Dilate;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
                    types = MorphTypes.Erode;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
                    types = MorphTypes.Open;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
                    types = MorphTypes.Close;
                    break;
            }

            MorphShapes shapes = MorphShapes.Rect;
            Mat Kernel = null;

            switch (eShape)
            {
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_CIRCLE:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(nKernelHorizontial, nKernelVertical));
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(nKernelHorizontial, nKernelVertical));
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(nKernelHorizontial, 1));
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_CROSS:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Cross, new Size(nKernelHorizontial, nKernelVertical));
                    break;
            }

            Cv2.MorphologyEx(mSrcID, mDstID, types, element: Kernel, anchor: null, iterations: niterations);

            Kernel?.Dispose();

            return true;
		}
		internal void MorphologyEx(Mat mSrcID, Mat mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
		{
			MorphTypes types = MorphTypes.HitMiss; ;
			switch (nOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					types = MorphTypes.Dilate;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					types = MorphTypes.Erode;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					types = MorphTypes.Open;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					types = MorphTypes.Close;
					break;
			}

			Cv2.MorphologyEx(mSrcID, mDstID, types, element: null, anchor: null, iterations: nNbIteration);
		}

        internal void MorphologyEx(Mat mSrcID, Mat mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape)
        {
            MorphTypes types = MorphTypes.HitMiss; 
            switch (nOperation)
            {
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
                    types = MorphTypes.Dilate;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
                    types = MorphTypes.Erode;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
                    types = MorphTypes.Open;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
                    types = MorphTypes.Close;
                    break;
            }

			MorphShapes shapes = MorphShapes.Rect;
			Mat Kernel = null;

            switch (eShape)
            {
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_CIRCLE:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3));
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Rect   , new Size(3, 3));
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Rect   , new Size(3, 1));
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_CROSS:
                    Kernel = Cv2.GetStructuringElement(MorphShapes.Cross  , new Size(3, 3));
                    break;
            }

            Cv2.MorphologyEx(mSrcID, mDstID, types, element: Kernel, anchor: null, iterations: 1);

			Kernel?.Dispose();
        }

        internal bool Kernel(Mat mSrcID, Mat mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration, int nKernelHorizontial, int nKernelVertical, out float[] plKernelData)
		{
			long nOperation = (long)eOperation;
			long nMode		= (long)eCondition;

			//////////////////////////////////////////////////////////////////////////
			// Set Don't Care
			plKernelData = new float[nKernelVertical * nKernelHorizontial];
			for (int nYmask = 0; nYmask < nKernelVertical; ++nYmask)
			{
				for (int nXmask = 0; nXmask < nKernelHorizontial; ++nXmask)
				{
					if (plKernelData[(nYmask * nKernelHorizontial) + nXmask] == 0)
						plKernelData[(nYmask * nKernelHorizontial) + nXmask] = -1;
				}
			}

			//////////////////////////////////////////////////////////////////////////
			// Set OverScanSize
			int nOverscanSize = Math.Max(nKernelHorizontial, nKernelVertical);
			nOverscanSize = (int)(nOverscanSize * nNbIteration);
			nOverscanSize += 1;

			Mat MatStructElement = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(nKernelHorizontial, nKernelVertical));

			//MIL.MbufAlloc2d(ImageProcess.SystemAlloc, nKernelHorizontial, nKernelVertical, 32, MIL.M_STRUCT_ELEMENT, MIL.M_NULL);
			////MbufPut2d(MilStructElement, OffsetX, OffsetY, nKernelHorizontial, nKernelVertical, plKernelData);
			//MIL.MbufPut2d(MilStructElement, 0L, 0L, nKernelHorizontial, nKernelVertical, plKernelData);

			//MIL.MbufControlNeighborhood(MilStructElement, MIL.M_OVERSCAN, MIL.M_MIRROR);
			//MIL.MimMorphic(mSrcID, mDstID, MilStructElement, nOperation, nNbIteration, nMode);
			//MIL.MbufControlNeighborhood(MilStructElement, MIL.M_OVERSCAN, MIL.M_DEFAULT);

			MatStructElement.Dispose();

			return true;
		}
	}
}
