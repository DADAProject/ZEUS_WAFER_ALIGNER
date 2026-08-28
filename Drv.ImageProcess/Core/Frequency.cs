using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{

	internal class Frequency
    {
		//internal bool FrequencyBinarize(MIL_ID mSrcID, MIL_ID mDstID, byte byLowLimitGVGV, byte byHighLimitGVGV, float fLowPercent, float fHighPercent, MIL.Mil_BINARIZE_ONELIMIT_OPERATION_TYPE eCondition, int nThOffSet)
		//{
		//	double dThMax = m_cAssistAlgo3.GetAvgGV(mSrcID, byLowLimitGVGV, byHighLimitGVGV, fLowPercent, fHighPercent);

		//	double dFinalTh = dThMax + nThOffSet;

		//	if (dFinalTh < 0)
		//	{
		//		dFinalTh = 0;
		//	}

		//	if (dFinalTh > 255)
		//	{
		//		dFinalTh = 255;
		//	}

		//	MIL.MimBinarize(mSrcID, mDstID, g_nMil_Binarize_OneLimit_Operation[eCondition], dFinalTh, MIL.M_NULL);

		//	return true;
		//}
	}
}
