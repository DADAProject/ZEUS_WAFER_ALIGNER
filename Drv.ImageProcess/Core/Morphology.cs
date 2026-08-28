using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Core
{
	public enum MORPHOLOGY_OPERATION : long
	{
		E_MORPHOLOGY_OPERATION_DILATE   = 2  ,
		E_MORPHOLOGY_OPERATION_ERODE	= 1  ,
		E_MORPHOLOGY_OPERATION_OPEN		= 16 ,
		E_MORPHOLOGY_OPERATION_CLOSE	= 32 ,
	}
	public enum MORPHOLOGY_CONDITION : long
	{
		E_MORPHOLOGY_GRAYSCALE = 512,
		E_MORPHOLOGY_BINARY    = 4096,
	}

	public enum MORPHOLOGY_SHAPE : long
	{
		E_MORPHOLOGY_SQUARE    = 0,
		E_MORPHOLOGY_RECTANGLE = 1,
		E_MORPHOLOGY_CIRCLE    = 2,
        E_MORPHOLOGY_CROSS     = 3,
    }
}
