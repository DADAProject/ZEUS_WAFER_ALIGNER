using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	internal partial class Enhance
	{
		internal unsafe bool ImageGainOffset(MIL_ID mSrc, MIL_ID mDst, double dGain, int nOffset, bool bUseParallel)
		{
			//////////////////////////////////////////////////////////////////////////
			// Funtion Initialize
			BUFF biSrc = new BUFF();
			BUFF biDst = new BUFF();
			
			biSrc.buffID = mSrc;
			biDst.buffID = mDst;

			biSrc.InitBuffInfo(false, 0);
			biDst.InitBuffInfo(true , 0);

			int iWidth  = biSrc.wid;
			int iHeight = biSrc.len;
			int iPitch  = biSrc.pitch;

			//////////////////////////////////////////////////////////////////////////
			// algorithm routine
			long lWBytes_Src = biSrc.pitch;
			int nW_Src = biSrc.wid, nH_Src = biSrc.len;

			fixed (byte* pbySrc = biSrc.pBuff)
			{
				if (pbySrc == null)
					return false;

				fixed (byte* pbyDst = biDst.pBuff)
				{
					long lWBytes_Dst = biDst.pitch;
					int nW_Dst = biDst.wid, nH_Dst = biDst.len;

					if (bUseParallel)
					{
						//# ifndef bUseParallel
						//				for (int ni = 0; ni < nH_Src; ++ni)
						//#else
						//					parallel_for(0, nH_Src, [&](int ni)
						//			#endif
						//			{
						//					for (int nj = 0; nj < nW_Src; ++nj)
						//					{
						//						BYTE* pbyCur_Src = &pbySrc[ni * lWBytes_Src + nj];
						//						BYTE* pbyCur_Dst = &pbyDst[ni * lWBytes_Src + nj];
						//
						//						int nGV = int(double(*pbyCur_Src) * dGain) + nOffset;
						//						if (nGV < 0) nGV = 0;
						//						if (nGV > 255) nGV = 255;
						//
						//						(*pbyCur_Dst) = nGV;
						//					}
						//				}
					}
					else
					{
						for (int ni = 0; ni < nH_Src; ++ni)
						{
							for (int nj = 0; nj < nW_Src; ++nj)
							{
								byte pbyCur_Src = pbySrc[ni * lWBytes_Src + nj];
								byte pbyCur_Dst = pbyDst[ni * lWBytes_Src + nj];

								int nGV = (int)((double)(pbyCur_Src) * dGain) + nOffset;
								if (nGV < 0) nGV = 0;
								if (nGV > 255) nGV = 255;

								pbyCur_Dst = (byte)nGV;
							}
						}
					}


				}
			}

			return true;
		}

		internal unsafe bool ControlColorRange(MIL_ID mSrc, MIL_ID mDst, int nRange, int nOffset, int nWidth, int nHieght, int nCH)
		{
			byte[] bImgSrc = null;
			byte[] bImgDst = null;
			double[] dGraValue = null;
			double dNewInten = 0;

			bImgSrc = new byte[nWidth * nHieght * 3];
			bImgDst = new byte[nWidth * nHieght * 3];
			Array.Clear(bImgSrc, 0, nWidth * nHieght * 3);
			Array.Clear(bImgDst, 0, nWidth * nHieght * 3);

			dGraValue = new double[nRange];
			Array.Clear(dGraValue, 0, nRange);

			for (int nValue = 1; nValue <= nRange; nValue++)
			{
				dGraValue[nValue] = (double)((double)(nValue) / (double)(nRange));
			}

			MIL.MbufGetColor(mSrc, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BANDS, bImgSrc);


			for (int nH = 0; nH < nHieght; nH++)
			{
				for (int nW = 0; nW < nWidth; nW++)
				{
					dNewInten = (double)(dGraValue[bImgSrc[(nH * nWidth * 3) + (nW * 3) + 0]]);
					bImgDst[(nH * nWidth * 3) + (nW * 3) + 0] = (byte)(dNewInten * bImgSrc[(nH * nWidth * 3) + (nW * 3) + 0]);

					dNewInten = (double)(dGraValue[bImgSrc[(nH * nWidth * 3) + (nW * 3) + 1]]);
					bImgDst[(nH * nWidth * 3) + (nW * 3) + 1] = (byte)(dNewInten * bImgSrc[(nH * nWidth * 3) + (nW * 3) + 1]);

					dNewInten = (double)(dGraValue[bImgSrc[(nH * nWidth * 3) + (nW * 3) + 2]]);
					bImgDst[(nH * nWidth * 3) + (nW * 3) + 2] = (byte)(dNewInten * bImgSrc[(nH * nWidth * 3) + (nW * 3) + 2]);

					if (nCH > 2 || nCH < 0)
					{
						return false;
					}
					if (bImgDst[(nH * nWidth * 3) + (nW * 3) + nCH] > nOffset)
					{
						bImgDst[(nH * nWidth * 3) + (nW * 3) + nCH] = 255;
					}
				}
			}
			MIL.MbufPutColor(mDst, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BANDS, bImgDst);

			bImgSrc = null;
			bImgDst = null;
			dGraValue = null;
			return true;
		}
	}
}
