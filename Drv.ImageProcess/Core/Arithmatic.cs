using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
	public enum ARITH_PROJECT_DIR
	{
		E_ARITH_PROJECT_ROW,
		E_ARITH_PROJECT_COLUMN,
	}


	public enum ARITH_PIXEL_OPERATION
	{
		E_ARITH_PIXEL_BELOW,
		E_ARITH_PIXEL_BETWEEN,
		E_ARITH_PIXEL_ABOVE,
	}

	public enum ARITH_1_OPERATION
	{
		E_ARITH_1_MINIMUM,
		E_ARITH_1_MAXIMUM,
		E_ARITH_1_AVERAGE,
		E_ARITH_1_MEAN,
		E_ARITH_1_VARIANCE,
		E_ARITH_1_STDDEV,
	}

	public enum LOGIC_1_OPERATION
	{
		E_LOGIC_1_INVERT,
	}
	public enum LOGIC_2_OPERATION
	{
		
	}

	public enum ARITH_1_SRC_AND_CONST
	{
		E_ARITH_1_SRC_AND_CONST__M_CONST_DIV,
		E_ARITH_1_SRC_AND_CONST__M_CONST_DIV_FIXED_POINT,
		E_ARITH_1_SRC_AND_CONST__M_CONST_SUB,
		E_ARITH_1_SRC_AND_CONST__M_CONST_SUB_SATURATION,
		E_ARITH_1_SRC_AND_CONST__M_CONST_PASS,
	}

	public enum ARITH_1_SRC
	{
		E_ARITH_1_SRC,
	}
	public enum ARITH_2_SRC
	{
		E_ARITH_2_SRC,
	}

	internal partial class Arithmatic
    {
		internal bool MilArith_OneSrc(MIL_ID mSrcID, MIL_ID mDstID, ARITH_1_SRC eOper)
		{
			MIL.MimArith(mSrcID, MIL.M_NULL, mDstID, (long)eOper);
			return true;
		}
		internal bool MilArith_TwoSrc(MIL_ID mSrcID1, MIL_ID mSrcID2, MIL_ID mDstID, ARITH_2_SRC eOper)
		{
			MIL.MimArith(mSrcID1, mSrcID2, mDstID, (long)eOper);
			return true;
		}
		internal bool MilArith_OneSrcAndConst(MIL_ID mSrcID, MIL_ID mConst, MIL_ID mDstID, ARITH_1_SRC_AND_CONST eOper)
		{
			if (eOper == ARITH_1_SRC_AND_CONST.E_ARITH_1_SRC_AND_CONST__M_CONST_DIV ||
				eOper == ARITH_1_SRC_AND_CONST.E_ARITH_1_SRC_AND_CONST__M_CONST_DIV_FIXED_POINT ||
				eOper == ARITH_1_SRC_AND_CONST.E_ARITH_1_SRC_AND_CONST__M_CONST_SUB ||
				eOper == ARITH_1_SRC_AND_CONST.E_ARITH_1_SRC_AND_CONST__M_CONST_SUB_SATURATION)
			{
				MIL.MimArith(mConst, mSrcID, mDstID, (long) eOper);
			}
			else if (eOper == ARITH_1_SRC_AND_CONST.E_ARITH_1_SRC_AND_CONST__M_CONST_PASS)
			{
				MIL.MimArith(mConst, MIL.M_NULL, mDstID, (long)eOper);
			}
			else
			{
				MIL.MimArith(mSrcID, mConst, mDstID, (long)eOper);
			}
			return true;
		}
	}
}
