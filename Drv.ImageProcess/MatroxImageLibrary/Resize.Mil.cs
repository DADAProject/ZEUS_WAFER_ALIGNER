using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess
{
    internal partial class Resize
	{
		internal unsafe bool ImageResize(MIL_ID mSrc, MIL_ID mDst)
		{
			double dwid_Src = (double)MIL.MbufInquire(mSrc, MIL.M_SIZE_X, MIL.M_NULL);
			double dlen_Src = (double)MIL.MbufInquire(mSrc, MIL.M_SIZE_Y, MIL.M_NULL);
				   
			double dwid_Dest = (double)MIL.MbufInquire(mDst, MIL.M_SIZE_X, MIL.M_NULL);
			double dlen_Dest = (double)MIL.MbufInquire(mDst, MIL.M_SIZE_Y, MIL.M_NULL);

			double dScaleX = (double)(dwid_Dest / dwid_Src);
			double dScaleY = (double)(dlen_Dest / dlen_Src);

			//M_NEAREST_NEIGHBOR Nearest neighbor(no interpolation).
			//M_BILINEAR Bilinear interpolation
			//M_BICUBIC   Bicubic interpolation
			//M_AVERAGE Averaging. For dezooming only.
			//M_INTERPOLATE Interpolated resizing:
			//for zooming = bilinear,
			//for dezooming = averaging
			//M_DEFAULT   Same as M_NEAREST_NEIGHBOR

			MIL.MimResize(mSrc, mDst, dScaleX, dScaleY, MIL.M_NEAREST_NEIGHBOR);
			return true;
		}

	
	}
}
