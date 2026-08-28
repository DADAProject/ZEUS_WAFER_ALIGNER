using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Drv.ImageProcess.Extension;

namespace Drv.ImageProcess.Core
{
	internal partial class Morphology
    {
		internal bool Simple(CogImage8Grey mSrcID, CogImage8Grey mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
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
				//
				ret = false;
            }
		
	
			return ret;
		}
		internal bool SimpleShape(CogImage8Grey mSrcID, CogImage8Grey mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape)
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
		internal void MorphologyEx(CogImage8Grey mSrcID, CogImage8Grey mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration)
		{
			CogIPOneImageMorphologyOperationConstants eOperation = CogIPOneImageMorphologyOperationConstants.Erode;

			//MinusErode 마이너스 확인해야함
			switch (nOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					eOperation = CogIPOneImageMorphologyOperationConstants.Dilate;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					eOperation = CogIPOneImageMorphologyOperationConstants.Erode;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					eOperation = CogIPOneImageMorphologyOperationConstants.Open;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					eOperation = CogIPOneImageMorphologyOperationConstants.Close;
					break;
			}

			using (CogIPOneImageTool Tool = new CogIPOneImageTool())
			{
				//CogIPOneImageGreyMorphology
				//CogIPOneImageGreyMorphology3x3Element
				//CogIPOneImageGreyMorphology3x3Elements
				//CogIPOneImageGreyMorphology3x3Element MxN
				CogIPOneImageGreyMorphology Filter = new CogIPOneImageGreyMorphology();
				Filter.Operation = eOperation;
				Tool.Operators.Add(Filter);
				Tool.InputImage = mSrcID;
				Tool.Run();

				CogIPOneImageGreyMorphologyNxM Filter2 = new CogIPOneImageGreyMorphologyNxM();
				Filter2.Operation = eOperation;
				Filter2.KernelWidth = 3;
				Filter2.KernelHeight = 3;
				//SetKernelValue 해줘야하나본데??
				Tool.Operators.Add(Filter);
				Tool.InputImage = mSrcID;
				Tool.Run();

                VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);
            }

        }
		internal void MorphologyEx(CogImage8Grey mSrcID, CogImage8Grey mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape)
		{
			int iOperation = 0,iShape = 0;

            //MinusErode 마이너스 확인해야함
            switch (nOperation)
            {
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
                    iOperation = (int)CogIPOneImageMorphologyOperationConstants.Dilate;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
                    iOperation = (int)CogIPOneImageMorphologyOperationConstants.Erode;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
                    iOperation = (int)CogIPOneImageMorphologyOperationConstants.Open;
                    break;
                case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
                    iOperation = (int)CogIPOneImageMorphologyOperationConstants.Close;
                    break;
            }

            switch (eShape)
            {
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_CIRCLE:
                    iShape = (int)CogIPOneImageMorphology3x3ElementTypeConstants.Type3x3Diamond;
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE:
                    iShape = (int)CogIPOneImageMorphology3x3ElementTypeConstants.Type3x3Square;
                    break;
                case MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE:
                    iShape = (int)CogIPOneImageMorphology3x3ElementTypeConstants.Type1x3Line0Deg;
                    break;
            }

            using (CogIPOneImageTool Tool = new CogIPOneImageTool())
            {
                CogIPOneImageGreyMorphology Filter = new CogIPOneImageGreyMorphology();
                CogIPOneImageGreyMorphology3x3Element Elements = new CogIPOneImageGreyMorphology3x3Element();
				Elements.MemberMask = CogIPOneImageMorphologyPositionConstants.Center;
				Elements.Type = (CogIPOneImageMorphology3x3ElementTypeConstants)iShape;

                Filter.OriginX = 1; Filter.OriginY = 1;
                Filter.Operation = (CogIPOneImageMorphologyOperationConstants)iOperation;
				Filter.Elements.Add(Elements);

                Tool.Operators.Add(Filter);
                Tool.InputImage = mSrcID;
                Tool.Run();

                VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);

                Elements.Dispose();
                Filter.Dispose();
            }
        }

		internal bool Kernel(CogImage8Grey mSrcID, CogImage8Grey mDstID, MORPHOLOGY_OPERATION eOperation, MORPHOLOGY_CONDITION eCondition, int nNbIteration, 
			int nKernelHorizontial, int nKernelVertical, float[] plKernelData)
		{
			long nOperation = (long)eOperation;
			long nMode		= (long)eCondition;

			

			return true;
		}

		internal bool KernelShape(CogImage8Grey mSrcID, CogImage8Grey mDstID, MORPHOLOGY_OPERATION nOperation, MORPHOLOGY_CONDITION eCondition, MORPHOLOGY_SHAPE eShape,
			int nKernelHorizontial, int nKernelVertical)
		{
			CogIPOneImageMorphologyOperationConstants eOperation = CogIPOneImageMorphologyOperationConstants.Erode;

			switch (nOperation)
			{
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_DILATE:
					eOperation = CogIPOneImageMorphologyOperationConstants.Dilate;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_ERODE:
					eOperation = CogIPOneImageMorphologyOperationConstants.Erode;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN:
					eOperation = CogIPOneImageMorphologyOperationConstants.Open;
					break;
				case MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_CLOSE:
					eOperation = CogIPOneImageMorphologyOperationConstants.Open;
					break;
			}

			using (CogIPOneImageTool Tool = new CogIPOneImageTool())
			{
				CogIPOneImageGreyMorphologyNxM Filter = new CogIPOneImageGreyMorphologyNxM();
				Filter.Operation    = eOperation;
				Filter.KernelWidth  = nKernelHorizontial;
				Filter.KernelHeight = eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE ? 1 : nKernelVertical;

                for (int y = 0; y < Filter.KernelHeight; y++)
                {
                    for (int x = 0; x < Filter.KernelWidth; x++)
                    {
						int value = 0;
						if (eShape == MORPHOLOGY_SHAPE.E_MORPHOLOGY_SQUARE)
						{
							if (eCondition == MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY)
							{
								value = 1;
							}
							else //(eCondition == MORPHOLOGY_CONDITION.E_MORPHOLOGY_GRAYSCALE)
							{
								//1, 0의 값을 사용하는 것이 아니라 Dilation의 경우 Max값을, Erosion의 경우 Min 값을 사용한다는 점이 다른 점 입니다.
							}
						}
                        else
                        {
							if (eCondition == MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY)
							{
								value = 1;
							}
							else //(eCondition == MORPHOLOGY_CONDITION.E_MORPHOLOGY_GRAYSCALE)
							{

							}
						}
						
						Filter.SetKernelValue(x, y, value);
					}
                }

				Tool.Operators.Add(Filter);
				Tool.InputImage = mSrcID;
				Tool.Run();

				//속도 비교
				//1.
				VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);

				//2.
				mDstID.Dispose();
				mDstID = (CogImage8Grey)Tool.OutputImage;
			}

			return true;
		}
	}
}
