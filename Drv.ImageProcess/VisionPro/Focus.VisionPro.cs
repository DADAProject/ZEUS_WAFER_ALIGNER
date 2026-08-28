using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;

namespace Drv.ImageProcess.Core
{
	internal partial class Focus
    {
		internal bool Focusing(CogImage8Grey mSrcID, Rect mRegion, ref double dRobbery)
		{
			CogRectangle RectRegion = new CogRectangle();
			RectRegion.Interactive = true;
			RectRegion.GraphicDOFEnable = CogRectangleDOFConstants.All;
			RectRegion.SetXYWidthHeight(mRegion.X, mRegion.Y, mRegion.Width, mRegion.Height);

            //자기상관법은 영상의 흐림 정도를 계산하여 영상의 선명도를 반영하며,
            //영상의 흐림 정도는 영상의 오프셋과 영상 일부의 표준 자기상관계수에 의해 결정되며
            //자기상관계수가 높을수록 흐려짐 이미지.


            //그라디언트 에너지 모드는 차이의 제곱을 합산하여 선명도를 측정합니다.
		    //전체 이미지에서 인접한 픽셀 값 사이. 일반적으로 더 선명한 이미지
		    //더 큰 픽셀 대 픽셀 대비 값을 가지며 더 높은 결과 점수를 생성합니다.

            CogImageSharpness SharpnessParams         = new CogImageSharpness();
			SharpnessParams.Mode			          = CogImageSharpnessModeConstants.GradientEnergy;
			SharpnessParams.AutoCorrelationNoiseLevel = 0;
			SharpnessParams.GradientEnergyLowPassSmoothing = 0;

            using (CogImageSharpnessTool Tool = new CogImageSharpnessTool())
			{
				Tool.InputImage = mSrcID;
				Tool.Region		= RectRegion;
				Tool.RunParams  = SharpnessParams;
				Tool.Run();
				dRobbery = (double)(Tool.Score * 1000000.0);
			}

			SharpnessParams.Dispose();
			RectRegion.Dispose();

			return true;
		}

		internal bool AutoFocusing(CogImage8Grey[] mSrcID, Rect mRegion, ref int mIndex)
		{
			int iLength = mSrcID.Length;
			List<double> listRobbery = new List<double>();
			for (int i = 0; i < iLength; i++)
            {
				double dRobbery = 0;

                if (Focusing(mSrcID[i], mRegion, ref dRobbery))
					listRobbery.Add(dRobbery);

				else return false;
            }

			mIndex = listRobbery.IndexOf(listRobbery.Max());

			return true;
		}
	}
}
