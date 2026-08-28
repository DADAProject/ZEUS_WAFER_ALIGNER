using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenCvSharp;
using Drv.ImageProcess.Core;
using System.Runtime.InteropServices.ComTypes;

namespace Drv.ImageProcess
{
	internal partial class Contour
    {
		internal bool ContourTrace(Mat mSrc, out ContourPoints[] cp, 
			RetrievalModes mode1 = RetrievalModes.CComp, ContourApproximationModes mode2 = ContourApproximationModes.ApproxSimple)
		{
            Cv2.FindContours(mSrc, out var contours, out var hierarchys, mode1, mode2);

			int iLength = contours.Length;
			int iPtCnt = 0;

			HierarchyIndex Index;
			cp = new ContourPoints[iLength];
			for (int idx = 0; idx < iLength; idx++)
			{
				cp[idx] = new ContourPoints();
			    cp[idx].Points = new List<Point2f>();

				Index = hierarchys[idx];
				for (int idxpt = 0; idxpt < contours[idx].Length; idxpt++)
				{
					cp[idx].Update(contours[idx][idxpt].X, contours[idx][idxpt].Y);
					cp[idx].num = iPtCnt++;
					if (MAX_CONTOUR < cp[idx].num) break;
				}

				cp[idx].Hierarchy = new Hierarchy(Index.Next, Index.Parent, Index.Child, Index.Parent);
            }

			return true;
		}


		//미리 메모리 할당해서 사용
		//internal bool ContourTrace(Mat mSrc, ref ContourPoints[] cp,
		//RetrievalModes mode1 = RetrievalModes.CComp, ContourApproximationModes mode2 = ContourApproximationModes.ApproxSimple)
		//{
		//	Cv2.FindContours(mSrc, out var contours, out var hierarchys, mode1, mode2);

		//	int iLength = contours.Length;
		//	int iPtLength = 0;
		//	int iPtCnt = 0;

		//	HierarchyIndex Index;
		//	for (int idx = 0; idx < iLength; idx++)
		//	{
		//		iPtCnt = 0;
		//		Index = hierarchys[idx];
		//		for (int idxpt = 0; idxpt < iPtLength; idxpt++)
		//		{
		//			//cp[idx].Update(contours[idx][idxpt].X, contours[idx][idxpt].Y, new Hierarchy(Index.Next, Index.Parent, Index.Child, Index.Parent));
		//			cp[idx].points[iPtCnt].X			= contours[idx][idxpt].X; 
		//			cp[idx].points[iPtCnt].Y			= contours[idx][idxpt].Y;
		//			cp[idx].hierarchys[iPtCnt].Child	= Index.Child;
		//			cp[idx].hierarchys[iPtCnt].Next	    = Index.Next;
		//			cp[idx].hierarchys[iPtCnt].Parent	= Index.Parent;
		//			cp[idx].hierarchys[iPtCnt].Previous	= Index.Previous;

		//			cp[idx].num				= iPtCnt++;
		//			if (MAX_CONTOUR < cp[idx].num) break;
		//		}
		//	}

		//	return true;
		//}
		//

	}

}
