using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using HalconDotNet;

namespace Drv.ImageProcess
{
    internal partial class Enhance
	{
		internal unsafe bool ImageGammaOffset(HImage mSrcID, HImage mDstID, double dGain, int nOffset, bool bUseParallel = false)
		{
			HObject ObjectID = null;
			HOperatorSet.ScaleImage(mSrcID, out ObjectID, dGain, nOffset);

			if (ObjectID != null)
			{
				HalconExtension.HobjectToHimage(ObjectID, ref mDstID);
				ObjectID.Dispose();
				return true;
			}
			else
			{
				return false;
			}
		}

		//오퍼레이터 Illuminate는 대비를 향상시킵니다.이미지의 매우 어두운 부분은 더 강하게 "조명"되고, 매우 밝은 부분은 "어두워집니다".
		//orig가 원래 회색 값이고 mean이 연산자 MeanImage 및 필터 크기 maskHeight x maskWidth를 통해 감지된 저역 통과 필터링된 이미지의 해당 회색 값인 경우.
		//byte-images의 경우 val은 127이고 int2-images 및 uint2-images의 경우 val은 중앙값과 같습니다.결과 회색 값이 새롭습니다.
		//new = round ( (val - mean) * Factor + orig ).
		//즉, 저역 통과 마스크가 선택될수록 더 큰 요소도 함께 선택되어야 합니다.

		internal unsafe bool ImageIlluminate(HImage mSrcID, HImage mDstID, int nKernelHorizontial = 101, int nKernelVertical = 101, double dGain = 0.7)
		{
			HObject ObjectID = null;
			HOperatorSet.Illuminate(mSrcID, out ObjectID, nKernelHorizontial, nKernelVertical, dGain);

			if (ObjectID != null)
			{
				HalconExtension.HobjectToHimage(ObjectID, ref mDstID);
				ObjectID.Dispose();
				return true;
			}
			else
			{
				return false;
			}
		}

		internal bool ImageScaleMax(HImage mSrcID, HImage mDstID)
		{
			HObject ObjectID = null; 
			HOperatorSet.ScaleImageMax(mSrcID, out ObjectID);

			if (ObjectID != null)
			{
				HalconExtension.HobjectToHimage(ObjectID, ref mDstID);
				ObjectID.Dispose();
				return true;
			}
			else
			{
				return false;
			}
		}


		//연산자 강조는 이미지의 고주파 영역(가장자리 및 모서리)을 강조합니다.결과 이미지가 더 선명하게 나타납니다.
		//먼저 절차는 저역 통과(MeanImage) 로 필터링을 수행합니다.
		//결과 회색 값(res) 은 얻은 회색 값(mean) 과 원래 회색 값(orig) 에서 다음과 같이 계산됩니다.
		//res := round((orig - mean) * Factor) + orig
		//Factor는 대비 증가를 측정하는 역할을 합니다.분할 주파수는 필터 행렬의 크기에 따라 결정됩니다. 행렬이 클수록 분할 주파수는 낮아집니다.
		//가장자리 처리로 회색 값이 이미지 가장자리에서 미러링됩니다.회색 값의 오버플로 및/또는 언더플로가 잘립니다.
		internal bool ImageEmphasize(HImage mSrcID, HImage mDstID, int nKernelHorizontial = 7, int nKernelVertical = 7, double dGain = 1.0)
		{
			HObject ObjectID = null;
			HOperatorSet.Emphasize(mSrcID, out ObjectID, nKernelHorizontial, nKernelVertical, dGain);

			if (ObjectID != null)
			{
				HalconExtension.HobjectToHimage(ObjectID, ref mDstID);
				ObjectID.Dispose();
				return true;
			}
			else
			{
				return false;
			}
		}

		

		//Enhancement
		//List of Operators
		//CoherenceEnhancingDiff
		//Perform a coherence enhancing diffusion of an image.
		//Emphasize
		//Enhance contrast of the image.
		//EquHistoImage
		//Histogram linearization of images
		//MeanCurvatureFlow
		//Apply the mean curvature flow to an image.
		//ScaleImageMax
		//Maximum gray value spreading in the value range 0 to 255.
		//ShockFilter
		//Apply a shock filter to an image.
	}
}
