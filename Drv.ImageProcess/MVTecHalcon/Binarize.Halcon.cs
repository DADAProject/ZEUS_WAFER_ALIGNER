using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using HalconDotNet;

namespace Drv.ImageProcess.Core
{
	internal partial class Binarize
    {
		internal bool OneLimit_Binarize(HImage mSrcID, HImage mDstID, BINARIZE_ONELIMIT_OPERATION eOperation, double dCondLow)
		{
			HObject ObjectID = null;

            switch (eOperation)
            {
				case BINARIZE_ONELIMIT_OPERATION.E_IN_RANGE        : HOperatorSet.Threshold(mSrcID, out ObjectID, dCondLow - 1, 255			);
					break;											 
                case BINARIZE_ONELIMIT_OPERATION.E_OUT_RANGE       : HOperatorSet.Threshold(mSrcID, out ObjectID, 0           , dCondLow - 1);
					break;											 													      
                case BINARIZE_ONELIMIT_OPERATION.E_EQUAL           : HOperatorSet.Threshold(mSrcID, out ObjectID, dCondLow    , dCondLow	);
					break;											 													      
                case BINARIZE_ONELIMIT_OPERATION.E_NOT_EQUAL       :
					HObject ObjectID1 = null , ObjectID2 = null;

					HOperatorSet.Threshold(mSrcID, out ObjectID1, 0           , dCondLow - 1);
					HOperatorSet.Threshold(mSrcID, out ObjectID2, dCondLow - 1, 255         );
					HOperatorSet.ConcatObj(ObjectID1, ObjectID2, out ObjectID);

					ObjectID1.Dispose();
					ObjectID2.Dispose();

					break;											 													      
                case BINARIZE_ONELIMIT_OPERATION.E_GREATER         : HOperatorSet.Threshold(mSrcID, out ObjectID, dCondLow    , 255			);
					break;											 													      
                case BINARIZE_ONELIMIT_OPERATION.E_LESS            : HOperatorSet.Threshold(mSrcID, out ObjectID, 0           , dCondLow - 1);
					break;																								      
                case BINARIZE_ONELIMIT_OPERATION.E_GREATER_OR_EQUAL: HOperatorSet.Threshold(mSrcID, out ObjectID, dCondLow    , 255			);
					break;																								      
                case BINARIZE_ONELIMIT_OPERATION.E_LESS_OR_EQUAL   : HOperatorSet.Threshold(mSrcID, out ObjectID, 0           , dCondLow    );
					break;
                case BINARIZE_ONELIMIT_OPERATION.E_MASK            : 
                    break;
            }

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

		internal bool TwoLimit_Binarize(HImage mSrcID, HImage mDstID, BINARIZE_TWOLIMIT_OPERATION eOperation, double dCondLow, double dCondHigh)
		{
			HObject ObjectID = null;

			switch (eOperation)
			{
				case BINARIZE_TWOLIMIT_OPERATION.E_TRIANGLE_BISECTION_BRIGHT   : break;
				case BINARIZE_TWOLIMIT_OPERATION.E_TRIANGLE_BISECTION_DARK     : break;
				case BINARIZE_TWOLIMIT_OPERATION.E_BIMODAL                     : break; 
				case BINARIZE_TWOLIMIT_OPERATION.E_FIXED                       : break;
				case BINARIZE_TWOLIMIT_OPERATION.E_DISCRETE_RANGE              : break;
				case BINARIZE_TWOLIMIT_OPERATION.E_RETURN_VALUE_AS_FLOAT_IN_INT: break;
			}

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
			//return true;
		}

		internal bool HistoLimit_Binarize(HImage mSrcID, HImage mDstID,
			BINARIZE_HISTO_OPERATION eOperation_Histo,
			BINARIZE_ONELIMIT_OPERATION eOperation_OneLimit,
			double dCondLow, double dCondHigh)
		{


			return true;
		}

		//binary_threshold는 자동으로 결정된 전역 임계값을 사용하여 단일 채널 이미지를 분할하고 영역에 분할된 영역을 반환합니다. 이것은 예를 들어 균일하게 조명된 배경에서 문자를 분할하는 데 유용합니다. 
		//binary_threshold는 또한 UsedThreshold에서 사용된 임계값을 반환합니다.

		//사용된 임계값은 방법에 제공된 방법에 의해 결정됩니다.
		//현재 연산자는 'max_separability'와 'smooth_histo'의 두 가지 방법을 제공합니다.
		//두 방법 모두 바이모달 히스토그램이 있는 이미지에만 사용해야 합니다.
		//'smooth_histo' 메서드는 bin_threshold 연산자가 제공한 것과 동일한 기능을 제공합니다.
		//방법 'max_separability'는 UsedThreshold에 대해 더 작은 값을 결정하는 경향이 있습니다.
		//또한 나머지 스펙트럼에서 멀리 떨어져 있는 히스토그램의 얇은 고립된 피크에 덜 민감하며 종종 'smooth_histo'보다 빠릅니다.
		//분리성 극대화
		//Method = 'max_separability'를 선택하면 Otsu에 따른 그레이 레벨 히스토그램을 기반으로 하는 자동 임계값이 호출됩니다(참고 문헌의 논문 참조).
		//알고리즘은 먼저 이미지의 히스토그램을 계산한 다음 통계적 모멘트를 사용하여 픽셀을 전경과 배경으로 나누고 이 두 클래스 간의 분리성을 최대화하는 최적의 임계값을 찾습니다.
		//이 방법은 byte 및 uint2 이미지에만 사용할 수 있습니다.
		//LightDark = 'light'인 경우 회색 값이 크거나 같은 모든 픽셀이 선택됩니다.
		//LightDark = 'dark'인 경우 회색 값이 보다 작은 모든 픽셀이 선택됩니다.
		//히스토그램 평활화
		//Method = 'smooth_histo'를 선택하여 binary_threshold는 다음과 같은 방식으로 임계값을 결정합니다.
		//먼저 회색 값의 상대 히스토그램이 결정됩니다.
		//그런 다음 임계값 연산을 위한 매개변수로 사용되는 히스토그램에서 관련 최소값을 추출합니다.최소값의 수를 줄이기 위해 히스토그램은 auto_threshold에서와 같이 가우스로 평활화됩니다.
		//평활 히스토그램에 최소값이 하나만 있을 때까지 마스크 크기가 확대됩니다.그런 다음 임계값이 이 최소값의 위치로 설정됩니다.
		//LightDark = 'light'인 경우 회색 값이 크거나 같은 모든 픽셀이 선택됩니다.
		//LightDark = 'dark'인 경우 회색 값이 보다 작은 모든 픽셀이 선택됩니다.
		internal bool OtsuBinarize(HImage mSrcID, HImage mDstID)
		{
			HObject ObjectID = null;

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

		//auto_threshold는 다중 임계값을 사용하여 단일 채널 이미지를 분할합니다. 먼저, 회색 값의 절대 히스토그램이 결정됩니다.
		//그런 다음 히스토그램에서 관련 최소값을 추출하여 임계값 연산을 위한 매개변수로 연속적으로 사용합니다. 
		//바이트 이미지에 사용되는 임계값은 0, 255이며 모든 최소값은 히스토그램에서 추출됩니다
		//(히스토그램이 표준 편차 시그마를 갖는 가우스 필터로 평활화된 후).각 회색 값 간격에 대해 하나의 영역이 생성됩니다.
		//따라서 영역의 수는 최소값 +1의 수입니다. uint2 이미지의 경우 위의 절차가 유사하게 사용됩니다.
		//그러나 여기에서 가장 높은 임계값은 65535입니다.또한 uint2 이미지의 경우 Sigma 값(가상)은 256개 값이 있는 히스토그램을 참조하지만 내부적으로는 더 높은 해상도의 히스토그램이 사용됩니다.
		//이는 매개변수 Sigma를 변경하지 않고도 이미지 유형 간 전환을 용이하게 하기 위해 수행됩니다.플로트 이미지의 경우 임계값은 이미지의 최소 및 최대 회색 값과 히스토그램에서 추출된 모든 최소값입니다. 
		//여기에서 매개변수 Sigma의 스케일링은 이미지의 원래 회색 값을 나타냅니다.시그마 값이 클수록 더 적은 수의 영역이 추출됩니다. 이 연산자는 추출할 영역이 유사한 회색 값(균일 영역)을 나타내는 경우에 유용합니다.

		//Sigma for the Gaussian smoothing of the histogram.
		//Default value: 2.0
		//Suggested values: 0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0
		//Typical range of values: 0.0 ≤ Sigma ≤ 100.0(lin)
		//Minimum increment: 0.01
		//Recommended increment: 0.3
		//Restriction: Sigma >= 0.0

		internal bool SigmaBinarize(HImage mSrcID, HImage mMskID, HImage mDstID, double dPosSigma, double dNegSigma, SIGMA_BINARIZE_OPERATION eOper)
		{
			bool bRet = false;
			HObject ObjectID = null;
			HObject ObjectID1 = null;
			HObject ObjectID2 = null;

			BUFF biMsk = new BUFF();
			BUFF biDst = new BUFF();

			biMsk.HbuffID = mMskID;
			biMsk.InitBuffInfo(false, 0);

			HOperatorSet.AutoThreshold(mSrcID, out ObjectID1, dNegSigma);
			HOperatorSet.AutoThreshold(mSrcID, out ObjectID2, dPosSigma);
			HOperatorSet.ConcatObj(ObjectID1, ObjectID2, out ObjectID);

			if (ObjectID != null)
			{
				HalconExtension.HobjectToHimage(ObjectID, ref mDstID);
				bRet = true;
			}
			else bRet = false;


			ObjectID ?.Dispose();
			ObjectID1?.Dispose();
			ObjectID2?.Dispose();


			if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_MASK_BINALIZE)
			{
				biDst.HbuffID = mDstID;
				biDst.InitBuffInfo(false, 0);

				for (int nLen = 0; nLen < biMsk.len; nLen++)
				{
					for (int nWid = 0; nWid < biMsk.wid; nWid++)
					{
						if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_MASK_BINALIZE && biMsk.pBuff[nLen * biMsk.pitch + nWid] == 0)
						{
							biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
						}
					}
				}
			}

			return bRet;
		}

		internal bool AdaptiveBinarize(HImage mSrcID, HImage mDstID, int nKernel)
		{
			HObject ObjectID = null;

			HOperatorSet.LocalThreshold(mSrcID, out ObjectID, "adapted_std_deviation", "light", "mask_size", nKernel);

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

	}
}
