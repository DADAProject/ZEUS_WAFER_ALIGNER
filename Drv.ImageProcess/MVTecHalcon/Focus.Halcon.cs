using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;

namespace Drv.ImageProcess.Core
{
	internal partial class Focus
    {
		internal bool Focusing(HImage mSrcID, Rect mRegion, out double dRobbery)
		{
			dRobbery = 0.0;


            return true;
		}

		internal bool AutoFocusing(HImage[] mSrcID, Rect mRegion, out int mIndex)
		{
			mIndex = -1;
			int iLength = mSrcID.Length;
			List<double> listRobbery = new List<double>();
			for (int i = 0; i < iLength; i++)
            {
				if (Focusing(mSrcID[i], mRegion, out double dRobbery)) listRobbery.Add(dRobbery);
				else return false;
            }

			mIndex = listRobbery.IndexOf(listRobbery.Max());

			return true;
		}
	}
}
