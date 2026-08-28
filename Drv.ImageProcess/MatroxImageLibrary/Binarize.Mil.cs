using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class Binarize
    {
		internal bool OneLimit_Binarize(MIL_ID mSrcID, MIL_ID mDstID, BINARIZE_ONELIMIT_OPERATION eOperation, double dCondLow)
		{
			MIL.MimBinarize(mSrcID, mDstID, (long) eOperation, dCondLow, MIL.M_NULL);
			return true;
		}

		internal bool TwoLimit_Binarize(MIL_ID mSrcID, MIL_ID mDstID, BINARIZE_TWOLIMIT_OPERATION eOperation, double dCondLow, double dCondHigh)
		{
			MIL.MimBinarize(mSrcID, mDstID, (long) eOperation, dCondLow, dCondHigh);

			return true;
		}

		internal bool HistoLimit_Binarize(MIL_ID mSrcID, MIL_ID mDstID,
			BINARIZE_HISTO_OPERATION eOperation_Histo,
			BINARIZE_ONELIMIT_OPERATION eOperation_OneLimit,
			double dCondLow, double dCondHigh)
		{
			MIL_INT Operation = (long) eOperation_Histo + (long) eOperation_OneLimit;

			MIL.MimBinarize(mSrcID, mDstID, (long) Operation, dCondLow, dCondHigh);

			return true;
		}

		internal bool OtsuBinarize(MIL_ID mSrc, MIL_ID mDst)
		{
			//////////////////////////////////////////////////////////////////////////
			// Funtion Initialize
			bool nRet = true;
			BUFF biSrc = new BUFF();
			BUFF biDst = new BUFF();
			biSrc.buffID = mSrc;
			biDst.buffID = mDst;

			biSrc.InitBuffInfo(false, 0);
			biDst.InitBuffInfo(true , 0);

			int wid = biSrc.wid, len = biSrc.len;
			long pitch = biSrc.pitch;

			//////////////////////////////////////////////////////////////////////////
			// algorithm routine
			double[] dHistoData = new double[256];

			for (int i = 0; i < 256; i++)
			{
				dHistoData[i] = 0.0;
			}

			double dArea = wid * len;

			for (int x = 0; x < wid; x++)
			{
				for (int y = 0; y < len; y++)
				{
					dHistoData[biSrc.pBuff[y * wid + x]] += 1;
				}
			}

			for (int i = 0; i < 256; i++)
			{
				dHistoData[i] /= dArea;
			}

			double[] e = new double[256], f = new double[256];
			e[0] = dHistoData[0];
			for (int k = 1; k < 256; k++)
			{
				e[k] = e[k - 1] + dHistoData[k];
			}

			f[0] = (double)(0 + 1) * dHistoData[0];
			for (int k = 1; k < 256; k++)
			{
				f[k] = f[k - 1] + (double)(k + 1) * dHistoData[k];
			}

			double F = f[255];
			double[] sigma = new double[256];
			for (int k = 0; k < 256; k++)
			{
				double tmp = e[k] - e[k] * e[k];
				if (tmp == 0)
					tmp = 0.000001;

				sigma[k] = (double)(((F * e[k] - f[k]) * (F * e[k] - f[k])) / tmp);
			}

			double max_sigma = -10000.0;
			int nThres = -1;

			for (int i = 0; i < 256; i++)
			{
				if (sigma[i] > max_sigma)
				{
					nThres = i;
					max_sigma = sigma[i];
				}
			}

			byte[] LUT = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				if (i < nThres || i > 250)
					LUT[i] = 255;
				else
					LUT[i] = 0;
			}

			for (int x = 0; x < wid; x++)
			{
				for (int y = 0; y < len; y++)
				{
					biDst.pBuff[y * wid + x] = LUT[biSrc.pBuff[y * wid + x]];
				}
			}

			MIL.MbufPut2d(biDst.buffID, 0, 0, biDst.wid, biDst.len, biDst.pBuff);

			return nRet;
		}
		internal bool SigmaBinarize(MIL_ID mSrcID, MIL_ID mMskID, MIL_ID mDstID, double dPosSigma, double dNegSigma, SIGMA_BINARIZE_OPERATION eOper)
		{
			bool bRet = true;

			MIL_ID mAllArea = MIL.M_NULL;

			BUFF biSrc = new BUFF();
			BUFF biMsk = new BUFF();
			BUFF biDst = new BUFF();
			biSrc.buffID = mSrcID;
			biDst.buffID = mDstID;

			biSrc.InitBuffInfo(false, 0);
			biDst.InitBuffInfo(true, 0);

			MIL_ID m_blobResult = MIL.M_NULL;
			MIL_ID m_blobFeature = MIL.M_NULL;

			MIL.MblobAllocFeatureList(Alloc.SystemAlloc, ref m_blobFeature);
			MIL.MblobAllocResult(Alloc.SystemAlloc, ref m_blobResult);		

			MIL.MblobSelectFeature(m_blobFeature, MIL.M_MEAN_PIXEL);
			MIL.MblobSelectFeature(m_blobFeature, MIL.M_SIGMA_PIXEL);

			if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_BINALIZE)
			{
				if (mAllArea != MIL.M_NULL)
				{
					MIL.MbufFree(mAllArea);
					mAllArea = MIL.M_NULL;
				}
				MIL.MbufAlloc2d(Alloc.SystemAlloc, biSrc.wid, biSrc.len, 8 + MIL.M_UNSIGNED, MIL.M_PROC + MIL.M_IMAGE, ref mAllArea);

				biMsk.buffID = mAllArea;
				biSrc.InitBuffInfo(true, 0);
			}
			else
			{
				biMsk.buffID = mMskID;
				biSrc.InitBuffInfo(false, 0);
			}

			MIL.MblobCalculate(biMsk.buffID, biSrc.buffID, m_blobFeature, m_blobResult);
			MIL.MblobSelect(m_blobResult, MIL.M_EXCLUDE, MIL.M_AREA, MIL.M_LESS, 0, MIL.M_NULL);

			MIL_INT blobNum = 0;
			MIL.MblobGetNumber(m_blobResult, blobNum);

			if (blobNum >= 1)
			{
				double[] dAvg = new double[blobNum];
				double[] dStdev = new double[blobNum];

				MIL.MblobGetResult(m_blobResult, MIL.M_MEAN_PIXEL, dAvg);
				MIL.MblobGetResult(m_blobResult, MIL.M_SIGMA_PIXEL, dStdev);

				double dDark   = dAvg[0] - dStdev[0] * dNegSigma;
				double dBright = dAvg[0] + dStdev[0] * dPosSigma;

				if (dDark   < 0   || dDark   >= 255) dDark   = 0;
				if (dBright > 255 || dBright <= 0  ) dBright = 255;


				for (int nLen = 0; nLen < biMsk.len; nLen++)
				{
					for (int nWid = 0; nWid < biMsk.wid; nWid++)
					{
						if (dNegSigma != 0 && dPosSigma == 0)
						{
							if (biSrc.pBuff[nLen * biSrc.pitch + nWid] <= dDark)
							{
								biDst.pBuff[nLen * biDst.pitch + nWid] = 255;
							}
							else
							{
								biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
							}
						}
						else if (dNegSigma == 0 && dPosSigma != 0)
						{
							if (biSrc.pBuff[nLen * biSrc.pitch + nWid] >= dBright)
							{
								biDst.pBuff[nLen * biDst.pitch + nWid] = 255;
							}
							else
							{
								biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
							}
						}
						else
						{
							if (biSrc.pBuff[nLen * biSrc.pitch + nWid] >= dBright || biSrc.pBuff[nLen * biSrc.pitch + nWid] <= dDark)
							{
								biDst.pBuff[nLen * biDst.pitch + nWid] = 255;
							}
							else
							{
								biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
							}
						}

						if (eOper == SIGMA_BINARIZE_OPERATION.E_SIGMA_MASK_BINALIZE && biMsk.pBuff[nLen * biMsk.pitch + nWid] == 0)
						{
							biDst.pBuff[nLen * biDst.pitch + nWid] = 0;
						}
					}
				}
			}
			else
				bRet = false;

			MIL.MbufPut2d(biDst.buffID, 0, 0, biDst.wid, biDst.len, biDst.pBuff);

			if (mAllArea != MIL.M_NULL) MIL.MbufFree(mAllArea); mAllArea = MIL.M_NULL;
			if (m_blobResult  != MIL.M_NULL) MIL.MblobFree(m_blobResult); m_blobResult = MIL.M_NULL;
			if (m_blobFeature != MIL.M_NULL) MIL.MblobFree(m_blobFeature); m_blobFeature = MIL.M_NULL;
			return bRet;
		}
	}
}
