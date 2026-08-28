using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
	internal partial class Binarize
    {
		internal bool OneLimit_Binarize(EImageBW8 mSrcID, EImageBW8 mDstID, BINARIZE_ONELIMIT_OPERATION eOperation, double dCondLow)
		{
            switch (eOperation)
            {
                case BINARIZE_ONELIMIT_OPERATION.E_IN_RANGE         : EasyImage.Threshold(mSrcID, mDstID, (uint)dCondLow, (byte)255, (byte)0);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_OUT_RANGE        : EasyImage.Threshold(mSrcID, mDstID, (uint)dCondLow, (byte)0  , (byte)255);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_EQUAL            : EasyImage.DoubleThreshold(mSrcID, mDstID, (uint)dCondLow - 1, (uint)dCondLow + 1, (byte)0  , 255, (byte)  0);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_NOT_EQUAL        : EasyImage.DoubleThreshold(mSrcID, mDstID, (uint)dCondLow - 1, (uint)dCondLow + 1, (byte)255,   0, (byte)255);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_GREATER			: EasyImage.Threshold(mSrcID, mDstID, (uint)dCondLow);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_LESS				: EasyImage.Threshold(mSrcID, mDstID, (uint)dCondLow,(byte)255, (byte)0);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_GREATER_OR_EQUAL : EasyImage.Threshold(mSrcID, mDstID, (uint)dCondLow - 1);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_LESS_OR_EQUAL    : EasyImage.Threshold(mSrcID, mDstID, (uint)dCondLow - 1, (byte)255, (byte)0);
					return true;
				case BINARIZE_ONELIMIT_OPERATION.E_MASK:
					return true;
            }

			return false;
		}

		internal bool TwoLimit_Binarize(EImageBW8 mSrcID, EImageBW8 mDstID, BINARIZE_TWOLIMIT_OPERATION eOperation, double dCondLow, double dCondHigh)
		{
            switch (eOperation)
            {
                case BINARIZE_TWOLIMIT_OPERATION.E_TRIANGLE_BISECTION_BRIGHT:
                    break;
                case BINARIZE_TWOLIMIT_OPERATION.E_TRIANGLE_BISECTION_DARK:
                    break;
                case BINARIZE_TWOLIMIT_OPERATION.E_BIMODAL:
                    break;
                case BINARIZE_TWOLIMIT_OPERATION.E_FIXED  :
					EasyImage.DoubleThreshold(mSrcID, mDstID, (uint)dCondLow, (uint)dCondHigh, 0, 255, 0);
					return true;
                case BINARIZE_TWOLIMIT_OPERATION.E_DISCRETE_RANGE:
                    break;
                case BINARIZE_TWOLIMIT_OPERATION.E_RETURN_VALUE_AS_FLOAT_IN_INT:
                    break;
                default:
                    break;
            }

            //EasyImage.DoubleThreshold(mSrcID, mDstID, (uint)dCondLow, (uint)dCondHigh, 1, 1, 1);

			return false;
		}

		internal bool HistoLimit_Binarize(EImageBW8 mSrcID, EImageBW8 mDstID,
			BINARIZE_HISTO_OPERATION eOperation_Histo,
			BINARIZE_ONELIMIT_OPERATION eOperation_OneLimit,
			double dCondLow, double dCondHigh)
		{


			return true;
		}

		internal bool OtsuBinarize(EImageBW8 mSrcID, EImageBW8 mDstID)
		{
			//이 플러그인 임계값은 Otsu 임계값 기술과 유사한 최대 엔트로피 알고리즘을 사용하는 이미지입니다.
			//여기서, 클래스 간 분산을 최대화(동일하게, 클래스 내 분산을 최소화)하는 대신 클래스 간 엔트로피가 최대화됩니다.
			EasyImage.AutoThreshold(mSrcID, mDstID, EThresholdMode.MaxEntropy);

			return true;
		}

		//internal bool SigmaBinarize(EImageBW8 mSrcID, EImageBW8 mMskID, EImageBW8 mDstID, double dPosSigma, double dNegSigma, SIGMA_BINARIZE_OPERATION eOper)
		//{
		//	bool bRet = true;

		//	BUFF biSrc = new BUFF_INFO();
		//	BUFF biMsk = new BUFF_INFO();
		//	BUFF biDst = new BUFF_INFO();
		//	biSrc.buffID = mSrcID;
		//	biDst.buffID = mDstID;

		//	biSrc.InitBuffInfo(false, 0);
		//	biDst.InitBuffInfo(true, 0);

		//	MIL_ID m_blobResult = MIL.M_NULL;
		//	MIL_ID m_blobFeature = MIL.M_NULL;

		//	MIL.MblobAllocFeatureList(ImageProcess.SystemAlloc, ref m_blobFeature);
		//	MIL.MblobAllocResult(ImageProcess.SystemAlloc, ref m_blobResult);

		//	MIL.MblobSelectFeature(m_blobFeature, MIL.M_MEAN_PIXEL);
		//	MIL.MblobSelectFeature(m_blobFeature, MIL.M_SIGMA_PIXEL);

		//	if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_BINALIZE)
		//	{
		//		if (mAllArea != MIL.M_NULL)
		//		{
		//			MIL.MbufFree(mAllArea);
		//			mAllArea = MIL.M_NULL;
		//		}
		//		MIL.MbufAlloc2d(ImageProcess.SystemAlloc, biSrc.wid, biSrc.len, 8 + MIL.M_UNSIGNED, MIL.M_PROC + MIL.M_IMAGE, ref mAllArea);

		//		biMsk.buffID = mAllArea;
		//		biSrc.InitBuffInfo(true, 0);
		//	}
		//	else
		//	{
		//		biMsk.buffID = mMskID;
		//		biSrc.InitBuffInfo(false, 0);
		//	}

		//	MIL.MblobCalculate(biMsk.buffID, biSrc.buffID, m_blobFeature, m_blobResult);
		//	MIL.MblobSelect(m_blobResult, MIL.M_EXCLUDE, MIL.M_AREA, MIL.M_LESS, 0, MIL.M_NULL);

		//	MIL_INT blobNum = 0;
		//	MIL.MblobGetNumber(m_blobResult, blobNum);

		//	if (blobNum >= 1)
		//	{
		//		double[] dAvg = new double[blobNum];
		//		double[] dStdev = new double[blobNum];

		//		MIL.MblobGetResult(m_blobResult, MIL.M_MEAN_PIXEL, dAvg);
		//		MIL.MblobGetResult(m_blobResult, MIL.M_SIGMA_PIXEL, dStdev);

		//		double dDark   = dAvg[0] - dStdev[0] * dNegSigma;
		//		double dBright = dAvg[0] + dStdev[0] * dPosSigma;

		//		if (dDark   < 0   || dDark   >= 255) dDark   = 0;
		//		if (dBright > 255 || dBright <= 0  ) dBright = 255;


		//		for (int nLen = 0; nLen < biMsk.len; nLen++)
		//		{
		//			for (int nWid = 0; nWid < biMsk.wid; nWid++)
		//			{
		//				if (dNegSigma != 0 && dPosSigma == 0)
		//				{
		//					if (biSrc.pBuff[nLen * biSrc.pitch + nWid] <= dDark)
		//					{
		//						biDst.pBuff[nLen * biDst.pitch + nWid] = 255;
		//					}
		//					else
		//					{
		//						biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
		//					}
		//				}
		//				else if (dNegSigma == 0 && dPosSigma != 0)
		//				{
		//					if (biSrc.pBuff[nLen * biSrc.pitch + nWid] >= dBright)
		//					{
		//						biDst.pBuff[nLen * biDst.pitch + nWid] = 255;
		//					}
		//					else
		//					{
		//						biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
		//					}
		//				}
		//				else
		//				{
		//					if (biSrc.pBuff[nLen * biSrc.pitch + nWid] >= dBright || biSrc.pBuff[nLen * biSrc.pitch + nWid] <= dDark)
		//					{
		//						biDst.pBuff[nLen * biDst.pitch + nWid] = 255;
		//					}
		//					else
		//					{
		//						biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
		//					}
		//				}

		//				if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_MASK_BINALIZE && biMsk.pBuff[nLen * biMsk.pitch + nWid] == 0)
		//				{
		//					biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
		//				}
		//			}
		//		}
		//	}
		//	else
		//		bRet = false;

		//	MIL.MbufPut2d(biDst.buffID, 0, 0, biDst.wid, biDst.len, biDst.pBuff);

		//	if (mAllArea != MIL.M_NULL) MIL.MbufFree(mAllArea); mAllArea = MIL.M_NULL;
		//	if (m_blobResult  != MIL.M_NULL) MIL.MblobFree(m_blobResult); m_blobResult = MIL.M_NULL;
		//	if (m_blobFeature != MIL.M_NULL) MIL.MblobFree(m_blobFeature); m_blobFeature = MIL.M_NULL;
		//	return bRet;
		//}

		internal bool AdaptiveBinarize(EImageBW8 mSrcID, EImageBW8 mDstID, int nKernel)
		{
			EasyImage.AdaptiveThreshold(mSrcID, mDstID, EAdaptiveThresholdMethod.Mean, nKernel, 0);

			return true;
		}
	}
}
