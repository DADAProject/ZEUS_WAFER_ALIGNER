using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drv.ImageProcess.Extension;
using OpenCvSharp;

namespace Drv.ImageProcess
{
    internal partial class Resize
	{
		internal unsafe bool ImageResize(Mat mSrc, Mat mDst)
		{

			return true;
		}

		internal unsafe bool ImageCrop(Mat mSrcID, Mat mDstID, Rect mRect)
		{
            if (mRect.X                < 0                    ) return false;
            if (mRect.X                > mSrcID.Width           ) return false;
            if (mRect.Y                < 0                    ) return false;
            if (mRect.Y                > mSrcID.Height          ) return false;
                                                     
            if (mRect.Width            > mSrcID.Width           ) return false;
            if (mRect.Height           > mSrcID.Height          ) return false;
            if (mRect.X + mRect.Width  > mSrcID.Width           ) return false;
            if (mRect.Y + mRect.Height > mSrcID.Height          ) return false;

            Mat CropMat = mSrcID.SubMat(new OpenCvSharp.Rect(mRect.X, mRect.Y, mRect.Width, mRect.Height));
            CropMat.CopyTo(mDstID);
            CropMat.Dispose();

            return true;
		}
	}
}
