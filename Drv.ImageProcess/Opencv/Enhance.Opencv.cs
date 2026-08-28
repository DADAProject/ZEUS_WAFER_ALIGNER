using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;

namespace Drv.ImageProcess.Opencv
{
    internal partial class Enhance
    {
		internal unsafe bool ImageGammaOffset(Mat mSrc, Mat mDst, double dGamma)
		{
			Mat lookuptable = new Mat(1, 256, MatType.CV_8UC1);
			byte* src = (byte*)lookuptable.DataPointer;
			for (int ni = 0; ni < 256; ++ni)
				src[ni] = (byte)(Math.Pow(ni / 256.0, dGamma) * 255.0);

			Mat res = new Mat(mSrc.Size(), mSrc.Type());
			Cv2.LUT(mSrc, lookuptable, res);
			Cv2.HConcat(mSrc, res, mDst);

			return true;
		}

	}
}
