using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Drv.ImageProcess.Extension;
using OpenCvSharp.Features2D;

namespace Drv.ImageProcess.Core
{
	internal partial class Convolution
    {
		internal bool Convolve(CogImage8Grey mSrcID, CogImage8Grey mDstID, EDGE_OPERATION eKernelType)
		{			
			switch (eKernelType)
			{
				case EDGE_OPERATION.E_EDGE_DETECT:
				case EDGE_OPERATION.E_EDGE_DETECT2:
                    EdgeDetect(mSrcID, mDstID, eKernelType);
                    break;
                case EDGE_OPERATION.E_HORIZ_EDGE:
				case EDGE_OPERATION.E_VERT_EDGE:
                    GradientEdgeDetect(mSrcID, mDstID, eKernelType);
                    break;
                case EDGE_OPERATION.E_LAPLACIAN_EDGE:
                case EDGE_OPERATION.E_LAPLACIAN_EDGE2:
                    LaplacianEdgeDetect(mSrcID, mDstID, eKernelType);
                    break;
				case EDGE_OPERATION.E_SHARPEN:
					break;
				case EDGE_OPERATION.E_SHARPEN2:
					break;
				case EDGE_OPERATION.E_SMOOTH:
                    break;
				case EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE  :
				case EDGE_OPERATION.E_HORIZONTAL_SOBEL_EDGE:
					SobelEdgeDetect(mSrcID, mDstID, eKernelType);
					break;
				case EDGE_OPERATION.E_MEDIAN:
                    Median(mSrcID, mDstID);
					break;
			}
		
			return true;
		}
		internal bool SobelEdgeDetect(CogImage8Grey mSrcID, CogImage8Grey mDstID, EDGE_OPERATION eKernleType)
		{
			using (CogIPOneImageTool Tool = new CogIPOneImageTool())
			{
				CogIPOneImageConvolveNxM Filter = new CogIPOneImageConvolveNxM();
                Filter.KernelWidth = 3;
				Filter.KernelHeight = 3;

				int[,] Kernel;
                if (eKernleType == EDGE_OPERATION.E_VERTICAL_SOBEL_EDGE)
                {
                    Kernel = new int[3, 3] { { -1, 0, +1 }, 
						                     { -2, 0, +2 }, 
						                     { -1, 0, +1 } }; 
                }
                else
                {
                    Kernel = new int[3, 3] { { +1 ,+2 ,+1 },
                                             {  0 , 0 , 0},
                                             { -1 ,-2 ,-1} };
                }

                for (int y = 0; y < Filter.KernelHeight; y++)
                    for (int x = 0; x < Filter.KernelWidth; x++)
                        Filter.SetKernelValue(x, y, Kernel[y, x]);

                Tool.Operators.Add(Filter);
				Tool.InputImage = mSrcID;
				Tool.Run();
                VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);
			}

			return true;
		}

        internal bool GradientEdgeDetect(CogImage8Grey mSrcID, CogImage8Grey mDstID, EDGE_OPERATION eKernleType)
        {
            //Shift And Difference
            //화소의 위치를 위, 아래, 왼쪽, 오른쪽으로 하나씩 이동시키면, 이동 한만큼 화소 값의 기울기가 발생합니다.
            //그리고 원래 화소에서 이동 위치의 화소를 단순히 빼서 엣지를 구하는 방법입니다
            using (CogIPOneImageTool Tool = new CogIPOneImageTool())
            {
                CogIPOneImageConvolveNxM Filter = new CogIPOneImageConvolveNxM();
                Filter.KernelWidth = 3;
                Filter.KernelHeight = 3;

                int[,] Kernel;
                if (eKernleType == EDGE_OPERATION.E_VERT_EDGE)
                {
                    Kernel = new int[3, 3] { {  0, 0, 0 },
                                             { -1, 1, 0 },
                                             {  0, 0, 0 } };
                }
                else
                {
                    Kernel = new int[3, 3] { {  0, -1, 0 },
                                             {  0,  1, 0 },
                                             {  0,  0, 0 } };
                }

                for (int y = 0; y < Filter.KernelHeight; y++)
                    for (int x = 0; x < Filter.KernelWidth; x++)
                        Filter.SetKernelValue(x, y, Kernel[y, x]);

                Tool.Operators.Add(Filter);
                Tool.InputImage = mSrcID;
                Tool.Run();
                VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);

                Filter.Dispose();
            }

            return true;
        }

        internal bool LaplacianEdgeDetect(CogImage8Grey mSrcID, CogImage8Grey mDstID, EDGE_OPERATION eKernleType)
        {
            //2차 미분
            //회색으로 나타나는 부분은 라플라시안 필터 수행 결괏값이 0에 가까운 부분이고,
            //엣지 근방의 흰색 또는 검정색으로 표현된 픽셀은 라플라시안 결괏값이 큰 양수 또는 음수 값을 갖는 픽셀임을 의미한다.

            using (CogIPOneImageTool Tool = new CogIPOneImageTool())
            {
                CogIPOneImageConvolveNxM Filter = new CogIPOneImageConvolveNxM();
                Filter.KernelWidth = 3;
                Filter.KernelHeight = 3;

                int[,] Kernel;
                if (eKernleType == EDGE_OPERATION.E_LAPLACIAN_EDGE)
                {
                    Kernel = new int[3, 3] { {  0,  1,  0 },
                                             {  1, -4,  1 },
                                             {  0,  1,  0 } };
                }
                else
                {
                    Kernel = new int[3, 3] { {  1,  1,  1 },
                                             {  1, -8,  1 },
                                             {  1,  1,  1 } };
                }

                for (int y = 0; y < Filter.KernelHeight; y++)
                    for (int x = 0; x < Filter.KernelWidth; x++)
                        Filter.SetKernelValue(x, y, Kernel[y, x]);

                Tool.Operators.Add(Filter);
                Tool.InputImage = mSrcID;
                Tool.Run();
                VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);

                Filter.Dispose();
            }

            return true;
        }

        internal bool EdgeDetect(CogImage8Grey mSrcID, CogImage8Grey mDstID, EDGE_OPERATION eKernleType)
        {
            using (CogIPOneImageTool Tool = new CogIPOneImageTool())
            {
                for (int i = 0; i < 2; i++)
                {
                    CogIPOneImageConvolveNxM Filter = new CogIPOneImageConvolveNxM();
  
                    int[,] Kernel;
                    if (eKernleType == EDGE_OPERATION.E_EDGE_DETECT)
                    {   
                        //Prewitt
                        Filter.KernelWidth = 3;
                        Filter.KernelHeight = 3;

                        if (i==0) Kernel = new int[3, 3] { { -1,  0, +1 },
                                                          { -1,  0, +1 },
                                                          { -1,  0, +1 } };

                        else     Kernel = new int[3, 3] { { -1, -1, -1 },
                                                          {  0,  0,  0 },
                                                          { +1, +1, +1 } };

                    }
                    else
                    {
                        //roberts
                        Filter.KernelWidth  = 2;
                        Filter.KernelHeight = 2;

                        if (i == 0) Kernel = new int[2, 2] { { +1,  0 },
                                                             {  0, -1 } };

                        else       Kernel = new int[2, 2] { {  0, +1 },
                                                            { -1, 0 } };
                    }
                    ////Scharr
                    //Filter.KernelWidth = 3;
                    //Filter.KernelHeight = 3;
                    //
                    //if (i == 0) Kernel = new int[3, 3] { {  -3,  0, +3  },
                    //                                     { -10,  0, +10 },
                    //                                     {  -3,  0, +3  } };
                    //
                    //else        Kernel = new int[3, 3] { { -3, -10, -3 },
                    //                                     {  0,  0,  0  },
                    //                                     { +3, +10, +3 } };

                    for (int y = 0; y < Filter.KernelHeight; y++)
                        for (int x = 0; x < Filter.KernelWidth; x++)
                            Filter.SetKernelValue(x, y, Kernel[y, x]);

                    Tool.Operators.Add(Filter);

                }
             

                Tool.InputImage = mSrcID;
                Tool.Run();
                VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);
            }

            return true;
        }

        internal bool Median(CogImage8Grey mSrcID, CogImage8Grey mDstID)
        {
            using (CogIPOneImageTool Tool = new CogIPOneImageTool())
            {
                CogIPOneImageMedian3x3 Filter = new CogIPOneImageMedian3x3();
                Tool.Operators.Add(Filter);
                Tool.InputImage = mSrcID;
                Tool.Run();
                VisionProExtension.bufCopy((CogImage8Grey)Tool.OutputImage, mDstID);

                Filter.Dispose();
            }

            return true;
        }

    }
}
