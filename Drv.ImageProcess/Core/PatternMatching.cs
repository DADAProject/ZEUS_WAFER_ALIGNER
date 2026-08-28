using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Core
{
	public enum PATTERNMATCHING_SETANGLE_OPERATION
	{
		E_PATTERNMATCHING_SETANGLE_ENABLE,
		E_PATTERNMATCHING_SETANGLE_DISABLE,
	}

	public enum PATTERNMATCHING_ACCURANCY_OPERATION
	{
		E_PATTERNMATCHING_ACCURANCY_LOW,
		E_PATTERNMATCHING_ACCURANCY_MEDIUM,
		E_PATTERNMATCHING_ACCURANCY_HIGH,
	}

	public enum PATTERNMATCHING_OPERATION
	{
		E_PATTERNMATCHING_NONE,
		E_PATTERNMATCHING_SRC_IMG,
		E_PATTERNMATCHING_DST_IMG,
		E_PATTERNMATCHING_RESULT_IMG
	}

	public struct stMatchParam
	{
		public int Golden { get; set; }
		public string MainPath { get; set; }
		public string[] FilePath { get; set; }
	}
}
