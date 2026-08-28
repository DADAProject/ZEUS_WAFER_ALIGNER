using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class Morphology
    {
		internal bool Simple(MIL_ID mSrcID, MIL_ID mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MIL_INT nNbIteration)
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

		internal void MorphologyEx(MIL_ID mSrcID, MIL_ID mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, MIL_INT nNbIteration)
		{
			
			switch (nOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					MIL.MimDilate(mSrcID, mDstID, nNbIteration, (long)eCondition);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					MIL.MimErode(mSrcID, mDstID, nNbIteration, (long)eCondition);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					MIL.MimOpen(mSrcID, mDstID, nNbIteration, (long)eCondition);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					MIL.MimClose(mSrcID, mDstID, nNbIteration, (long)eCondition);
					break;
			}
		}


		internal bool Kernel(MIL_ID mSrcID, MIL_ID mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MIL_INT nNbIteration, int nKernelHorizontial, int nKernelVertical, float[] plKernelData)
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
						plKernelData[(nYmask * nKernelHorizontial) + nXmask] = MIL.M_DONT_CARE;
				}
			}

			//////////////////////////////////////////////////////////////////////////
			// Set OverScanSize
			int nOverscanSize = Math.Max(nKernelHorizontial, nKernelVertical);
			nOverscanSize = (int)(nOverscanSize * nNbIteration);
			nOverscanSize += 1;

			MIL_ID MilStructElement = MIL.MbufAlloc2d(Alloc.SystemAlloc, nKernelHorizontial, nKernelVertical, 32, MIL.M_STRUCT_ELEMENT, MIL.M_NULL);
			// 	MbufPut2d(MilStructElement, OffsetX, OffsetY, nKernelHorizontial, nKernelVertical, plKernelData);
			MIL.MbufPut2d(MilStructElement, 0L, 0L, nKernelHorizontial, nKernelVertical, plKernelData);

			MIL.MbufControlNeighborhood(MilStructElement, MIL.M_OVERSCAN, MIL.M_MIRROR);
			MIL.MimMorphic(mSrcID, mDstID, MilStructElement, nOperation, nNbIteration, nMode);
			MIL.MbufControlNeighborhood(MilStructElement, MIL.M_OVERSCAN, MIL.M_DEFAULT);

			MIL.MbufFree(MilStructElement);
			MilStructElement = MIL.M_NULL;

			return true;
		}
	}
}
