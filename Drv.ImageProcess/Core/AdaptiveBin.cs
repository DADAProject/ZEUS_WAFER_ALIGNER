using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
    internal class AdaptiveBin
    {
		//internal bool AdaptiveBinarize(MIL_ID mSrc, MIL_ID mDst, int nMaskSize, double dFactor, bool bUseParallel)
		//{
		//	BUFF_INFO biSrc = new BUFF_INFO();
		//	BUFF_INFO biDst = new BUFF_INFO();
		//	biSrc.buffID = mSrc;
		//	biDst.buffID = mDst;

		//	biSrc.InitBuffInfo(false, 0);
		//	biDst.InitBuffInfo(true, 0);
		
		//	int wid = biSrc.wid, len = biSrc.len;
		//	long pitch = biSrc.pitch;
		
		//	//////////////////////////////////////////////////////////////////////////
		//	// Etc val;
		//	RectangleF rcRoi = new RectangleF(0, 0, wid, len);
		//	int nOuterMaskSize = 0;
		//	byte byOuterMaskGV = 255;
		//	//	BYTE				byInnerMaskGV		(0);
		//	List<RectangleF> pvrcInnerMaskArea = null;
		//	//:::::::::::::::::::::::::::::://
		
		//	long lWBytes_Src = pitch;
		//	int nW_Src = wid, nH_Src = len;
		//		BYTE* pSrc = biSrc.pBuff;
		//	if (!pSrc)
		//	{
		//		return false;
		//	}
		
		//	int nWBytes_Src = (int)(lWBytes_Src);
		
		//	long lWBytes_Dst(pitch);
		//	int nW_Dst(wid), nH_Dst(len);
		//	BYTE* pDst = biDst.pBuff;
		
		//	// algorithm routine
		//	int nWBytes_Dst = (int)(lWBytes_Dst);
		
		//	int nLocal_L = Math.Max(0, rcRoi.Left - nOuterMaskSize);
		//	int nLocal_T = Math.Max(0, rcRoi.Top - nOuterMaskSize);
		//	int nLocal_R = Math.Min(nW_Src, rcRoi.Right + nOuterMaskSize);
		//	int nLocal_B = Math.Min(nH_Src, rcRoi.Bottom + nOuterMaskSize);
		//	CRect rcRoi_Local(nLocal_L, nLocal_T, nLocal_R, nLocal_B);
		
		//	int nW_Local = rcRoi_Local.Width();
		//	int nH_Local = rcRoi_Local.Height();
		
		//	int nWBytes_Local = biSrc.pitch;
		//	BYTE* pLocal = biSrc.pBuff;
		
		//	int nMarginX = rcRoi.left - rcRoi_Local.left;
		//	int nMarginY = rcRoi.top - rcRoi_Local.top;
		
		//	if (nMarginX< 0) nMarginX = 0;
		//	if (nMarginY< 0) nMarginY = 0;
		
		//	// Outer Mask
		//#ifndef bUseParallel
		//	for (int nCoordi_Y = 0; nCoordi_Y<nH_Local; ++nCoordi_Y)
		//#else
		//	parallel_for(0, nH_Local, [&] (int nCoordi_Y)
		//#endif
		//	{
		//		// case 1. Outer Mask
		
		//		if (nCoordi_Y < nMarginY || nCoordi_Y >= nMarginY + nH_Dst)
		//		{
		//			memset(&pLocal[nCoordi_Y * nWBytes_Local], byOuterMaskGV, nW_Local);
		//		}
		//		else
		//		{
		//			memset(&pLocal[nCoordi_Y * nWBytes_Local], byOuterMaskGV, nMarginX);
		//			memcpy(&pLocal[nCoordi_Y * nWBytes_Local + nMarginX], &pSrc[(nCoordi_Y + rcRoi.top) * nWBytes_Src + rcRoi.left], nW_Dst);
		//			memset(&pLocal[nCoordi_Y * nWBytes_Local + nMarginX + nW_Dst], byOuterMaskGV, nMarginX);
		//		}
		
		//	}
		//#ifdef bUseParallel
		//	);
		//#endif
		//	if (pvrcInnerMaskArea)
		//	{
		//		std::vector<CRect> ::const_iterator iterBegin = pvrcInnerMaskArea->begin();
		//	std::vector<CRect> ::const_iterator iterEnd = pvrcInnerMaskArea->end();
		//		for (std::vector<CRect> ::const_iterator iter = iterBegin; iter != iterEnd; std::advance(iter, 1))
		//		{
		//			// Margin ??
		//			CRect rcCur = (*iter);
		//	rcCur -= CPoint(rcRoi_Local.left + nMarginX, rcRoi_Local.top + nMarginY);
		//}
		//	}
		
		//	std::vector<int> vnIntegral(nW_Local* nH_Local);
		//std::vector<int> vnSqrInteg(nW_Local* nH_Local);
		
		//int nMaskHalf = nMaskSize >> 1;
		
		//// Pre Accumulation
		//vnIntegral[0] = pLocal[0];
		//vnSqrInteg[0] = pLocal[0] * pLocal[0];
		
		//# ifndef bUseParallel
		//for (int nCoordi_X = 1; nCoordi_X < nW_Local; ++nCoordi_X)
		//#else
		//	parallel_for(1, nW_Local, [&](int nCoordi_X)
		//#endif
		//	{
		//	int nCur = pLocal[nCoordi_X];
		//	vnIntegral[nCoordi_X] = vnIntegral[nCoordi_X - 1] + nCur;
		//	vnSqrInteg[nCoordi_X] = vnSqrInteg[nCoordi_X - 1] + nCur * nCur;
		//}
		//#ifdef bUseParallel
		//	);
		//#endif
		
		//for (int nCoordi_Y = 1; nCoordi_Y < nH_Local; ++nCoordi_Y)
		//{
		//	// kk : 이전 (y-1) 결과를 현재(y) 에서 사용하기 때문에, y Loop 에서는 parallel_for() 를 사용할 수 없다.
		//	int nLineSum = 0, nLineSqrSum = 0;
		//	for (int nCoordi_X = 0; nCoordi_X < nW_Local; ++nCoordi_X)
		//	{
		//		// kk : 2중 For 문에서 내부 Loop 에 대해서 parallel_for() 를 사용하면 일반적으로 성능 저하가 있으며, nLineSum, nLineSqrSum 때문에, 여기서 사용하면 결과가 이상해 진다.
		//		int nCur = pLocal[nCoordi_Y * nWBytes_Local + nCoordi_X];
		//		nLineSum += nCur;
		//		nLineSqrSum += nCur * nCur;
		//		vnIntegral[(nCoordi_Y * nW_Local) + nCoordi_X] = vnIntegral[((nCoordi_Y - 1) * nW_Local) + nCoordi_X] + nLineSum;
		//		vnSqrInteg[(nCoordi_Y * nW_Local) + nCoordi_X] = vnSqrInteg[((nCoordi_Y - 1) * nW_Local) + nCoordi_X] + nLineSqrSum;
		//	}
		//}
		
		////algorithm main;
		
		//# ifndef bUseParallel
		//for (int nCoordi_Y = nMarginY; nCoordi_Y < nMarginY + nH_Dst; nCoordi_Y++)
		//#else
		//	parallel_for(nMarginY, nMarginY + nH_Dst, [&](int nCoordi_Y)
		//#endif
		//	{
		//	for (int nCoordi_X = nMarginX; nCoordi_X < nMarginX + nW_Dst; nCoordi_X++)
		//	{
		//		// 이중 Loop 에서 내부 Loop 까지 parallel_for() 를 사용하면 오히려 성능이 저하된다
		
		//		//clip windows 
		//		int xmin = Math.Max(0, nCoordi_X - nMaskHalf);
		//		int ymin = Math.Max(0, nCoordi_Y - nMaskHalf);
		//		int xmax = Math.Min(nW_Local - 1, nCoordi_X + nMaskHalf);
		//		int ymax = Math.Min(nH_Local - 1, nCoordi_Y + nMaskHalf);
		//		int nArea = (xmax - xmin + 1) * (ymax - ymin + 1);
		//		// calculate window mean and std deviation;
		//		int nDiff, nSqrDiff;
		//		if (!xmin && !ymin)
		//		{ // origin
		//			nDiff = vnIntegral[(ymax * nW_Local) + xmax];
		//			nSqrDiff = vnSqrInteg[(ymax * nW_Local) + xmax];
		//		}
		//		else if (!xmin && ymin)
		//		{ // first column
		//			nDiff = vnIntegral[(ymax * nW_Local) + xmax] - vnIntegral[((ymin - 1) * nW_Local) + xmax];
		//			nSqrDiff = vnSqrInteg[(ymax * nW_Local) + xmax] - vnSqrInteg[((ymin - 1) * nW_Local) + xmax];
		//		}
		//		else if (xmin && !ymin)
		//		{ // first row
		//			nDiff = vnIntegral[(ymax * nW_Local) + xmax] - vnIntegral[(ymax * nW_Local) + (xmin - 1)];
		//			nSqrDiff = vnSqrInteg[(ymax * nW_Local) + xmax] - vnSqrInteg[(ymax * nW_Local) + (xmin - 1)];
		//		}
		//		else
		//		{ // rest of the image
		//			int diagsum = vnIntegral[(ymax * nW_Local) + xmax] + vnIntegral[((ymin - 1) * nW_Local) + (xmin - 1)];
		//			int idiagsum = vnIntegral[((ymin - 1) * nW_Local) + xmax] + vnIntegral[(ymax * nW_Local) + (xmin - 1)];
		//			nDiff = diagsum - idiagsum;
		//			int sqdiagsum = vnSqrInteg[(ymax * nW_Local) + xmax] + vnSqrInteg[((ymin - 1) * nW_Local) + (xmin - 1)];
		//			int sqidiagsum = vnSqrInteg[((ymin - 1) * nW_Local) + xmax] + vnSqrInteg[(ymax * nW_Local) + (xmin - 1)];
		//			nSqrDiff = sqdiagsum - sqidiagsum;
		//		}
		//		// threshold = window_mean *(1 + factor * (std_dev/128.-1));
		//		// 128 = max_allowed_std_deviation the gray image;
		
		//		double dMean = 0.f;
		//		if (nArea != 0.f) dMean = double(nDiff) / double(nArea);
		
		//		double dTmp = 0.f;
		//		if (nArea != 0.f)
		//			dTmp = (double)nSqrDiff - double(nDiff) * nDiff / double(nArea);
		
		//		double dStd = 0.f;
		//		if (nArea - 1 != 0.f)
		//			dStd = sqrt(fabs(dTmp / double(nArea - 1)));
		
		//		double dThreshold = dMean * (1.f + dFactor * ((dStd / 128.0) - 1.f));
		
		//		if (pLocal[nCoordi_Y * nWBytes_Local + nCoordi_X] < dThreshold)
		//			pDst[(nCoordi_Y - nMarginY) * nWBytes_Dst + (nCoordi_X - nMarginX)] = 0;
		//		else
		//			pDst[(nCoordi_Y - nMarginY) * nWBytes_Dst + (nCoordi_X - nMarginX)] = 255;
		//	}
		//}
		//#ifdef bUseParallel
		//	);
		//#endif
		//return true;
		//}
    }
}
