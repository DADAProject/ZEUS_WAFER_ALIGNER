using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Core
{
	public enum BINARIZE_HISTO_OPERATION : long
	{
	}
	public enum BINARIZE_TWOLIMIT_OPERATION : long
	{
		E_TRIANGLE_BISECTION_BRIGHT      = 48, //근이 반드시 존재하는 폐구간을 이분한 후, 이 중 근이 존재하는 하위 폐구간을 선택하는 것을 반복하여서 근을 찾는 알고리즘
		E_TRIANGLE_BISECTION_DARK        = 32, //근이 반드시 존재하는 폐구간을 이분한 후, 이 중 근이 존재하는 하위 폐구간을 선택하는 것을 반복하여서 근을 찾는 알고리즘
		E_BIMODAL					     = 64,  //Bimodal distribution은 통계학에서 서로 다른 두 개의 최빈값을 갖는 연속확률분포이다. 아래 그림에서처럼 두 개의 극대값이 있는 확률분포함수 그래프를 나타낸다.
		E_FIXED							 = 80, //
		E_DISCRETE_RANGE			     = 4096,//Discrete이란 분리되거나 다른 모양이나 형태를 갖는 것을 의미합니다.
		E_RETURN_VALUE_AS_FLOAT_IN_INT   = 8192
	}
	public enum BINARIZE_ONELIMIT_OPERATION : long
	{
		E_IN_RANGE         = 1,
		E_OUT_RANGE        = 2,
		E_EQUAL            = 3,
		E_NOT_EQUAL        = 4,
		E_GREATER          = 5,
		E_LESS             = 6,
		E_GREATER_OR_EQUAL = 7,
		E_LESS_OR_EQUAL    = 8,
		E_MASK             = 4095,
	}

	public enum SIGMA_BINARIZE_OPERATION : long
	{
		E_SIGMA_BINALIZE,
		E_SIGMA_MASK_BINALIZE,
	}
}
