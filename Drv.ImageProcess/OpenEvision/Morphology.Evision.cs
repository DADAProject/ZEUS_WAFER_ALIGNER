using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
	internal partial class Morphology
    {
		internal bool Simple(EImageBW8 mSrcID, EImageBW8 mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
		{
			bool ret = false;
            //////////////////////////////////////////////////////////////////////////
            // Todo : MorphologyLine3 함수는 제거 예정이지만
            // 현제 작성된 구조는 마지막 파라메터를 필요로 하지않음
            // HORZ에 대한 구조체가 정의 되어있기 때문
            try
            {
				switch (eOperation)
				{
					case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
						MorphologyEx(mSrcID, mDstID, eOperation, eCondition, nNbIteration);
						ret = true;
						break;
				}
			}
            catch (Exception)
            {
				//2.7버전에서 에러 발생 (메모리 충동)
				ret = false;
            }
		
	
			return ret;
		}
		internal bool SimpleShape(EImageBW8 mSrcID, EImageBW8 mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape)
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
		internal void MorphologyEx(EImageBW8 mSrcID, EImageBW8 mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
		{
			switch (nOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					EasyImage.Dilate(mSrcID, mDstID, nNbIteration);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					EasyImage.Erode(mSrcID, mDstID, nNbIteration);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					EasyImage.Open(mSrcID, mDstID, nNbIteration);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					EasyImage.Close(mSrcID, mDstID, nNbIteration);
					break;
			}
		}
		internal void MorphologyEx(EImageBW8 mSrcID, EImageBW8 mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape)
		{

			switch (nOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					if      (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE   )	EasyImage.DilateBox (mSrcID, mDstID, 3    );
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.DilateBox (mSrcID, mDstID, 3, 3 );
					else                                                        EasyImage.DilateDisk(mSrcID, mDstID, 3    );
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					if      (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE   )	EasyImage.ErodeBox (mSrcID, mDstID, 3    );
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.ErodeBox (mSrcID, mDstID, 3, 3 );
					else                                                        EasyImage.ErodeDisk(mSrcID, mDstID, 3    );
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					if      (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE	  ) EasyImage.OpenBox (mSrcID, mDstID, 3    );
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.OpenBox (mSrcID, mDstID, 3, 3 );
					else                                                        EasyImage.OpenDisk(mSrcID, mDstID, 3    );
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					if      (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE   ) EasyImage.CloseBox (mSrcID, mDstID, 3    );
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.CloseBox (mSrcID, mDstID, 3, 3 );
					else                                                        EasyImage.CloseDisk(mSrcID, mDstID, 3    );
					break;
			}
		}

		internal bool Kernel(EImageBW8 mSrcID, EImageBW8 mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration, 
			int nKernelHorizontial, int nKernelVertical, float[] plKernelData)
		{
			long nOperation = (long)eOperation;
			long nMode		= (long)eCondition;

			

			return true;
		}

		internal bool KernelShape(EImageBW8 mSrcID, EImageBW8 mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape,
			int nKernelHorizontial, int nKernelVertical)
		{
			long nMode = (long)eCondition;

			switch (eOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE)         EasyImage.DilateBox(mSrcID , mDstID,(uint)nKernelHorizontial);
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.DilateBox(mSrcID , mDstID,(uint)nKernelHorizontial, (uint)nKernelVertical);
					else													    EasyImage.DilateDisk(mSrcID, mDstID,(uint)nKernelHorizontial);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE)         EasyImage.ErodeBox(mSrcID , mDstID, (uint)nKernelHorizontial);
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.ErodeBox(mSrcID , mDstID, (uint)nKernelHorizontial, (uint)nKernelVertical);
					else                                                        EasyImage.ErodeDisk(mSrcID, mDstID, (uint)nKernelHorizontial);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE)         EasyImage.OpenBox(mSrcID , mDstID, (uint)nKernelHorizontial);
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.OpenBox(mSrcID , mDstID, (uint)nKernelHorizontial, (uint)nKernelVertical);
					else												        EasyImage.OpenDisk(mSrcID, mDstID, (uint)nKernelHorizontial);
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE)			EasyImage.CloseBox(mSrcID , mDstID, (uint)nKernelHorizontial);
					else if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE) EasyImage.CloseBox(mSrcID , mDstID, (uint)nKernelHorizontial, (uint)nKernelVertical);
					else													    EasyImage.CloseDisk(mSrcID, mDstID, (uint)nKernelHorizontial);
					break;
			}


			return true;
		}
	}
}
