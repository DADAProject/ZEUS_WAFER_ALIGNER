using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Core
{
	public enum EDGE_OPERATION
	{
		E_EDGE_DETECT = 0,
		E_EDGE_DETECT2         ,
		E_HORIZ_EDGE           ,
		E_LAPLACIAN_EDGE       ,
		E_LAPLACIAN_EDGE2      ,
		E_SHARPEN              ,  // Gaussian
		E_SHARPEN2             ,  // Gaussian 5X5
		E_SMOOTH               ,  //Uniform
		E_VERT_EDGE            ,
		E_VERTICAL_SOBEL_EDGE  ,
		E_HORIZONTAL_SOBEL_EDGE,

		E_MEDIAN = 20,
	}

	public enum SOBEL_OPERATION
	{
		E_VERTICAL_SOBEL_EDGE   = EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE,
		E_HORIZONTAL_SOBEL_EDGE = EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE,
	}
}
