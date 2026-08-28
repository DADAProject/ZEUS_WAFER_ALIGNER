using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess
{
    internal partial class Resize
	{
		internal unsafe bool ImageResize(EImageBW8 mSrc, EImageBW8 mDst)
		{
			Easy.Resize(mSrc, mDst);

			return true;
		}

		internal unsafe bool ImageCrop(EImageBW8 mSrc, EImageBW8 mDst, Rect mRect)
		{
			bool bRet = false;
			EROIBW8 ROI = new EROIBW8();

			if (mRect.X				   < 0          ) return false;
			if (mRect.X                > mSrc.Width ) return false;
			if (mRect.Y				   < 0          ) return false;
			if (mRect.Y				   > mSrc.Height) return false;

			if (mRect.Width			   > mSrc.Width ) return false;
			if (mRect.Height		   > mSrc.Height) return false;
			if (mRect.X + mRect.Width  > mSrc.Width ) return false;
			if (mRect.Y + mRect.Height > mSrc.Height) return false;

			try
			{
				ROI.Attach(mSrc, mRect.X, mRect.Y, mRect.Width, mRect.Height);
				ROI.CropToImage();
				EasyImage.Copy(ROI, mDst);

				bRet = true;
			}
            catch 
            {
				bRet = false;
			}

			ROI.Dispose();

			return bRet;
		}
	}
}
