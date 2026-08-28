using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using OpenCvSharp;

namespace Drv.ImageProcess.Core
{
	internal partial class Binarize
    {
		internal bool OneLimit_Binarize(Mat mSrcID, Mat mDstID, BINARIZE_ONELIMIT_OPERATION eOperation, double dCondLow)
		{
            ThresholdTypes type = ThresholdTypes.Binary;

            switch (eOperation)
            {
                case BINARIZE_ONELIMIT_OPERATION.E_IN_RANGE:
                    type = ThresholdTypes.Binary;
                    break;
                case BINARIZE_ONELIMIT_OPERATION.E_OUT_RANGE:
                    break;
                case BINARIZE_ONELIMIT_OPERATION.E_EQUAL:
					type = ThresholdTypes.Trunc;
					break;
                case BINARIZE_ONELIMIT_OPERATION.E_NOT_EQUAL:
                    break;
                case BINARIZE_ONELIMIT_OPERATION.E_GREATER:
                    break;
                case BINARIZE_ONELIMIT_OPERATION.E_LESS:
					type = ThresholdTypes.BinaryInv;
					break;
                case BINARIZE_ONELIMIT_OPERATION.E_GREATER_OR_EQUAL:
                    break;
                case BINARIZE_ONELIMIT_OPERATION.E_LESS_OR_EQUAL:
                    break;
                case BINARIZE_ONELIMIT_OPERATION.E_MASK:
                    break;
                default:
                    break;
            }

            Cv2.Threshold(mSrcID, mDstID, dCondLow, 255, type);

            return true;
		}

		internal bool TwoLimit_Binarize(Mat mSrcID, Mat mDstID, BINARIZE_TWOLIMIT_OPERATION eOperation, double dCondLow, double dCondHigh)
		{
			//나중에 타입변경
			ThresholdTypes type = ThresholdTypes.Binary;

			Cv2.Threshold(mSrcID, mDstID, dCondLow, 255, type);

			return true;
		}

		internal bool HistoLimit_Binarize(Mat mSrcID, Mat mDstID,
	BINARIZE_HISTO_OPERATION eOperation_Histo,
	BINARIZE_ONELIMIT_OPERATION eOperation_OneLimit,
	double dCondLow, double dCondHigh)
		{
			ThresholdTypes type = ThresholdTypes.Binary;

			switch (eOperation_OneLimit)
			{
				case BINARIZE_ONELIMIT_OPERATION.E_IN_RANGE:
					type = ThresholdTypes.Binary;
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_OUT_RANGE:
					type = ThresholdTypes.BinaryInv;
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_EQUAL:
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_NOT_EQUAL:
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_GREATER:
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_LESS:
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_GREATER_OR_EQUAL:
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_LESS_OR_EQUAL:
					break;
				case BINARIZE_ONELIMIT_OPERATION.E_MASK:
					break;
				default:
					break;
			}

			Mat hist = new Mat();
			int[] hdims = { 256 }; // Histogram size for each dimension
			Rangef[] ranges = { new Rangef((int)dCondLow, (int)dCondHigh), }; // min/max 
			Cv2.CalcHist(
				new Mat[] { mSrcID }, //Mat image
				new int[] { 0 },    //channels : grayscale = 0 / R,G,B = 0,1,2
				null,               //mask : all area = null
				hist,               //output
				1,                  //dims
				hdims,              //hist size
				ranges);            //range

			double minVal, maxVal;
			Cv2.MinMaxLoc(hist, out minVal, out maxVal);
			Cv2.Threshold(mSrcID, mDstID, maxVal, 255, type);

			return true;
		}

		internal bool OtsuBinarize(Mat mSrc, Mat mDst)
		{
			bool nRet = true;

            Cv2.Threshold(mSrc, mDst, 100, 255, ThresholdTypes.Otsu);

            return nRet;
		}

		internal unsafe bool SigmaBinarize(Mat mSrcID, Mat mMskID, Mat mDstID, double dPosSigma, double dNegSigma, SIGMA_BINARIZE_OPERATION eOper)
		{
			bool bRet = true;
            Scalar mean, std;
            if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_BINALIZE)
                Cv2.MeanStdDev(mSrcID, out mean, out std, null);
            else
                Cv2.MeanStdDev(mSrcID, out mean, out std, mMskID);

            double dAvg     = mean.ToDouble();
            double dStdev   = std.ToDouble();

            double dDark   = dAvg - dStdev * dNegSigma;
            double dBright = dAvg + dStdev * dPosSigma;

            if (dDark < 0 || dDark >= 255) dDark = 0;
            if (dBright > 255 || dBright <= 0) dBright = 255;

			if (mDstID == null) mDstID = new Mat(mSrcID.Size(), mSrcID.Type());

			byte* src  = (byte*)mSrcID.DataPointer;
			byte* dest = (byte*)mDstID.DataPointer;
			byte* mask = (byte*)mMskID.DataPointer;

			for (int nLen = 0; nLen < mDstID.Height; nLen++)
			{
				for (int nWid = 0; nWid < mDstID.Width; nWid++)
				{
					if (dNegSigma != 0 && dPosSigma == 0)
					{
						if (src[nLen * mSrcID.Width + nWid] <= dDark)
						{
							dest[nLen * mDstID.Width + nWid] = 255;
						}
						else
						{
							dest[nLen * mDstID.Width + nWid] = 0;
						}
					}
					else if (dNegSigma == 0 && dPosSigma != 0)
					{
						if (src[nLen * mSrcID.Width + nWid] >= dBright)
						{
							dest[nLen * mDstID.Width + nWid] = 255;
						}
						else
						{
							dest[nLen * mDstID.Width + nWid] = 0;
						}
					}
					else
					{
						if (src[nLen * mSrcID.Width + nWid] >= dBright || src[nLen * mSrcID.Width + nWid] <= dDark)
						{
							dest[nLen * mDstID.Width + nWid] = 255;
						}
						else
						{
							dest[nLen * mDstID.Width + nWid] = 0;
						}
					}

					if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_MASK_BINALIZE && mask[nLen * mMskID.Width + nWid] == 0)
					{
						dest[nLen * mDstID.Width + nWid] = 0;
					}
				}
			}

			return bRet;
		}


	}
}
