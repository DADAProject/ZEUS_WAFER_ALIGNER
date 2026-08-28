using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess
{
    internal partial class Enhance
	{
		internal unsafe bool ImageGammaOffset(EImageBW8 mSrc, EImageBW8 mDst, double dGain, int nOffset, bool bUseParallel = false)
		{
			EasyImage.GainOffset(mSrc, mDst, (float)dGain, (float)nOffset);

			return true;
		}


		internal bool ImageScaleMax(EImageBW8 mSrcID, EImageBW8 mDstID)
		{
			EasyImage.PixelStat(mSrcID, out EBW8 pixelMin, out EBW8 pixelMax, out float average);

			double dGrain  = (double)((average / (float)pixelMax.Value) + 1.0f);
			double dOffset = (double)(pixelMin.Value);

			if (dGrain > 3.0) return false;
			if (dGrain <   0) return false;

			if (dGrain < 0  ) return false;
			if (dGrain < 0  ) return false;

			EasyImage.GainOffset(mSrcID, mDstID, (float)dGrain, (float)dOffset);

			return true;
		}

		
		internal unsafe bool ImageIlluminate(EImageBW8 mSrcID, EImageBW8 mDstID, int nKernelHorizontial = 7, int nKernelVertical = 7, double dGain = 1.0)
		{
			int iWidth = mSrcID.Width;
			int iHeight = mSrcID.Height;

			EImageBW8 MeanImage = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));
			EasyImage.ConvolUniform(mSrcID, MeanImage, (uint)nKernelHorizontial, (uint)nKernelVertical);

			byte* org = (byte*)mSrcID.GetImagePtr().ToPointer();
			byte* src = (byte*)MeanImage.GetImagePtr().ToPointer();
			byte* dst = (byte*)mDstID.GetImagePtr().ToPointer();

			int offset = 0;

			for (int y = 0; y < iHeight; y++)
			{
				for (int x = 0; x < iWidth; x++, org++, src++, dst++)
				{
					*dst = (byte)Math.Min(Math.Round((127 - *src) * dGain) + *org, 255);
				}
				dst += offset;
				src += offset;
			}

			MeanImage.Dispose();

			return false;
		}

		internal unsafe bool ImageEmphasize(EImageBW8 mSrcID, EImageBW8 mDstID, int nKernelHorizontial = 7, int nKernelVertical = 7, double dGain = 1.0)
		{
			//Emphasize 연산자는 이미지의 고주파 영역(가장자리 및 모서리)을 강조합니다. 결과 이미지가 더 선명하게 나타납니다.
			//먼저 절차는 저역 통과(MeanImage)로 필터링을 수행합니다
			//결과 회색 값(res)은 얻은 회색 값(mean)과 원래 회색 값(orig)에서 다음과 같이 계산됩니다.
			// res := round((orig - mean) * Factor) + orig

			int iWidth  = mSrcID.Width;
			int iHeight = mSrcID.Height;

			EImageBW8 MeanImage = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));
			EasyImage.ConvolUniform(mSrcID, MeanImage, (uint)nKernelHorizontial, (uint)nKernelVertical);

			byte* org = (byte*)mSrcID.GetImagePtr().ToPointer();
			byte* src = (byte*) MeanImage.GetImagePtr().ToPointer();
			byte* dst = (byte*) mDstID.GetImagePtr().ToPointer();

			int offset = 0;

			for (int y = 0; y < iHeight; y++)
			{
				for (int x = 0; x < iWidth; x++, org++, src++, dst++)
				{
					*dst = (byte)Math.Min(Math.Round((*org - *src) * dGain) + *org, 255);
				}
				dst += offset;
				src += offset;
			}

			MeanImage.Dispose();

			return false;
		}

		internal unsafe bool ImageFlatFieldCorrection(EImageBW8 mSrcID, EImageBW8 mDstID)
		{
			int iWidth  = mSrcID.Width;
			int iHeight = mSrcID.Height;

			EImageBW8 Resize = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));
			Easy.Resize(mSrcID, Resize);

			EImageBW8 Gaussian  = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));
			EImageBW8 Gaussian1 = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));
			EImageBW8 Gaussian2 = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));
			EImageBW8 Gaussian3 = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));
			EImageBW8 Gaussian4 = new EImageBW8((int)(iWidth / 3), (int)(iHeight / 3));

			// create background image from the input image blurring it with Gaussian 5 times
			EasyImage.ConvolGaussian(Resize   , Gaussian , 5, 21); Resize   .Dispose();
			EasyImage.ConvolGaussian(Gaussian , Gaussian1, 5, 21); Gaussian .Dispose();
			EasyImage.ConvolGaussian(Gaussian1, Gaussian2, 5, 21); Gaussian1.Dispose();
			EasyImage.ConvolGaussian(Gaussian2, Gaussian3, 5, 21); Gaussian2.Dispose();
			EasyImage.ConvolGaussian(Gaussian3, Gaussian4, 5, 21); Gaussian3.Dispose();

			// resize the blurred image back to original size
			EImageBW8 BackGround = new EImageBW8(iWidth, iHeight);
			Easy.Resize(Gaussian4, BackGround); Gaussian4.Dispose();

			// get background image's statistics (mean value is used as correction factor)
			EasyImage.PixelStat(BackGround, out EBW8 pixelMin, out EBW8 pixelMax, out float average);

			byte* src = (byte*)mSrcID.GetImagePtr().ToPointer();
			byte* dst = (byte*)mDstID.GetImagePtr().ToPointer();
			byte* bg = (byte*)BackGround.GetImagePtr().ToPointer();
			//
			//// grayscale image
			double mean = (double)average;
			int offset = 0;

			for (int y = 0; y < iHeight; y++)
			{
				for (int x = 0; x < iWidth; x++, src++, dst++, bg++)
				{
					if (*bg != 0)
					{
						*dst = (byte)Math.Min(mean * *src / *bg, 255);
					}
				}
				dst += offset;
				src += offset;
				bg += offset;
			}

			BackGround.Dispose();

			return true;
		}


	}
}
