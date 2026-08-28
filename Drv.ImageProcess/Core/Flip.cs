using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	public enum IMAGE_FLIP_OPERATION
	{
		E_IMAGE_M_FLIP_HORIZONTAL,
		E_IMAGE_M_FLIP_VERTICAL,
	}

	internal class Flip
    {
		internal bool ImageFlip(MIL_ID mSrcID, MIL_ID mDstID, IMAGE_FLIP_OPERATION eFlipType)
		{
			if (eFlipType == IMAGE_FLIP_OPERATION.E_IMAGE_M_FLIP_HORIZONTAL)
			{
				MIL.MimFlip(mSrcID, mDstID, MIL.M_FLIP_HORIZONTAL, MIL.M_DEFAULT);
			}
			else
			{
				MIL.MimFlip(mSrcID, mDstID, MIL.M_FLIP_VERTICAL, MIL.M_DEFAULT);
			}
			return true;
		}
	}
}
