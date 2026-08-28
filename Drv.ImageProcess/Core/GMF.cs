using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Core
{
	public enum GMF_SETANGLE_OPERATION
	{
		E_GMF_SETANGLE_ENABLE,
		E_GMF_SETANGLE_DISABLE,
	}

	public enum GMF_TYPE
	{
		SURF,
	}

	public enum GMF_OPERATION
	{
		E_GMF_NONE,
		E_GMF_SRC_IMG,
		E_GMF_DST_IMG,
		E_GMF_RESULT_IMG
	}
}
