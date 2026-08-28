using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
	internal partial class Focus
    {

		//배열로 할껀지 정해야함
		internal bool Focusing(EImageBW8 mSrcID, out double dRobbery)
		{
			dRobbery = EasyImage.Focusing(mSrcID);

			return true;
		}
	}
}
