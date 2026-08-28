using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
    internal class MultiRegionThr
    {
	    internal bool MultiRegionThresholdUsingMil(MIL_ID Mil_SrcImg, MIL_ID Mil_DstImg, int nStart, int nEnd, int nClass)
		{
			int nstart, nend;
			nstart = nStart;
			nend = nEnd;
			int nWidth  = (int)MIL.MbufInquire(Mil_SrcImg, MIL.M_SIZE_X, MIL.M_NULL);
			int nHeight = (int)MIL.MbufInquire(Mil_SrcImg, MIL.M_SIZE_X, MIL.M_NULL);

			byte[] SrcImg = new byte[nWidth * nHeight];
			MIL.MbufGet2d(Mil_SrcImg, 0, 0, nWidth, nHeight, SrcImg);
			char[] LUT = new char[256];

			/* compute histogram */
			int[] ulhistogram = new int[256];
			Array.Clear(ulhistogram, 0, ulhistogram.Length);
			int ni, nj;

			//	number_of_pixels = Width * Height;
			for (ni = 0; ni < nHeight; ni++)
				for (nj = 0; nj < nWidth; nj++)
					ulhistogram[SrcImg[nWidth * ni + nj]]++;

			int NumClass = nClass;
			int[] Class  = new int[nClass];

			for (ni = 0; ni < NumClass; ni++)
				Class[ni] = -1;

			double Vd = partition_recursive(NumClass, ulhistogram, nstart, nend, Class);

			int RealCnt = 0;
			for (ni = 0; ni < NumClass; ni++)
			{
				if (Class[ni] >= 0 && Class[ni] < 256)
					RealCnt++;
			}

			int index = 0, limit;
			if (RealCnt == 0)
			{
				for (ni = 0; ni < 128; ni++) LUT[ni]   = (char) 0;
				for (ni = 128; ni < 256; ni++) LUT[ni] = (char)255;

			}
			else
			{
				for (ni = 0; ni < 256; ni++) LUT[ni] = (char)255;

				limit = 256 / (RealCnt);
				if (limit > 255) limit = 255;

				int Val = 0;
				for (index = 0; index < RealCnt; index++, Val += limit)
				{
					int startIndex;
					if (index - 1 < 0) startIndex = 0;
					else
						startIndex = Class[index - 1];
					for (ni = startIndex; ni < Class[index]; ni++) LUT[ni] = (char)Val;
				}
			}

			for (ni = 0; ni < nHeight; ni++)
				for (nj = 0; nj < nWidth; nj++)
					SrcImg[nWidth * ni + nj] = (byte)LUT[SrcImg[nWidth * ni + nj]];

			MIL.MbufPut2d(Mil_DstImg, 0, 0, nWidth, nHeight, SrcImg);

			return true;
		}
		internal double partition_recursive(int nclass, int[] ulH, int nStart, int nEnd, int[] nClass)
		{
			int nstart, nend;
			nstart = nStart;
			nend = nEnd;

			int ns = 0; double dsx = 0.0, dsxx = 0.0;

			if (nclass == 1)
			{
				for (int i = nstart; i < nend; i++)
				{
					if (ulH[i] == 0) continue;
					ns += ulH[i];
					dsx += (double)(i * ulH[i]);
					dsxx += (double)(i * i * ulH[i]);
				}
				double dqv = (dsxx - (dsx * dsx / ns));
				return dqv;  //variation
			}

			dsx = 0.0; dsxx = 0.0; ns = 0;
			double dvdmin = (double) ulong.MaxValue;
			int[] ntClass = new int[nclass - 1];

			for (int ni = nstart; ni < (nend - nclass + 1); ni++)
			{
				if (ulH[ni] == 0) continue;
				ns		+= (int)ulH[ni];
				dsx		+= (double)(ni * ulH[ni]);
				dsxx	+= (double)(ni * ni * ulH[ni]);
				double dqv = (dsxx - (dsx * dsx / ns));
				double nv1 = partition_recursive(nclass - 1, ulH, ni + 1, nend, ntClass);
				double vd = dqv + nv1;
				if (vd < dvdmin)
				{
					dvdmin = vd;
					nClass[0] = ni;
					Buffer.BlockCopy(nClass, 1, ntClass, 0, nclass - 1);
					//Array.Copy(nClass, 1, ntClass, 0, nclass - 1);
				}
			}
			return dvdmin;
		}
	}
}
