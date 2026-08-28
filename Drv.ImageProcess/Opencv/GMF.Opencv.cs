using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using OpenCvSharp.XFeatures2D;

namespace Drv.ImageProcess.Core
{
	internal partial class GMF
	{
		internal unsafe bool GeometrictModelFinder(Mat nSrcImg, Mat nDstImg, string cContextPath, out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper)
		{

			dCenter_X = 0.0; dCenter_Y = 0.0; dAngle = 0.0; dScore = 0.0;

			int iWidth = nSrcImg.Width;
			int iHeight = nSrcImg.Height;

			using (Mat model = new Mat(cContextPath))
			using (Mat canny_m = new Mat())
			using (Mat canny_s = new Mat())
			{
				//Model
				Cv2.Canny(model, canny_m, 100, 100, 3, false);
				Cv2.FindContours(canny_m, out var contours_m, out var hierarchy_m, RetrievalModes.External, ContourApproximationModes.ApproxNone);

				double dMaxArea = 0;
				int iMaxIdx = 0;
				int iModelLength = contours_m.Length;
				for (int i = 0; i < iModelLength; i++)
                {
					var area = Cv2.ContourArea(contours_m[i].ToArray());
					if (dMaxArea < area)
					{
						dMaxArea = area;
						iMaxIdx = i;
					}
				}

				canny_m.SaveImage(@"C:\Users\DAx2\Desktop\Test\modelcanny.bmp");
				//test
				Cv2.DrawContours(model, contours_m, iMaxIdx, Scalar.Red, 3);
				model.SaveImage(@"C:\Users\DAx2\Desktop\Test\modeltest.bmp");

				double dMinArea = Cv2.ContourArea(contours_m[iMaxIdx].ToArray()) / 2;

				//Src
				Cv2.Canny(nSrcImg, canny_s, 10, 100, 3, false);
				Cv2.FindContours(canny_s, out var contours_s, out var hierarchy_s, RetrievalModes.External, ContourApproximationModes.ApproxNone);

				double dMinScore = 9999;
				int iMinIdx      = 0;
				PointF ptCenter  = new PointF();

				int iLength = contours_s.Length;
				for (int i = 0; i < iLength; ++i)
				{
					var area = Cv2.ContourArea(contours_s[i].ToArray());
					if (area < dMinArea) continue;

					double dMatch = Cv2.MatchShapes(contours_m[0], contours_s[i], ShapeMatchModes.I3, 0.0);

					if (dMatch < dMinScore)
					{
						var M = Cv2.Moments(contours_s[i]);

						dMinScore = dMatch;
						iMinIdx = i;
						ptCenter.X = (int)(M.M10/ M.M00);
						ptCenter.Y = (int)(M.M01 / M.M00);

					}
				}


				//Rect rECT = Cv2.BoundingRect(contours_s[iMinIdx]);
				//Cv2.DrawContours(nSrcImg, contours_s, iMinIdx, Scalar.Red, 3);
				//Cv2.PutText(nSrcImg, Math.Round(dMinScore, 5).ToString(), rECT.Location, HersheyFonts.HersheyComplex, 0.25, Scalar.Blue);
				//nSrcImg.SaveImage(@"C:\Users\DAx2\Desktop\Test\test.bmp");

			}

			return true;
		}



		internal unsafe bool FeatureModelFinder(Mat nSrcImg, Mat nDstImg, string cContextPath, out double dCenter_X, out double dCenter_Y, out double dAngle, out double dScore, GMF_OPERATION nOper, GMF_TYPE nType = GMF_TYPE.SURF)
		{

			dCenter_X = 0.0; dCenter_Y = 0.0; dAngle = 0.0; dScore = 0.0;

			int k = 2;
			double hessianThresh = 300;
			double uniquenessThreshold = 0.8;

			int iWidth = nSrcImg.Width;
			int iHeight = nSrcImg.Height;


			var detector = SURF.Create(hessianThreshold: hessianThresh); //A good default value could be from 300 to 500, depending from the image contrast.
			//using (Mat src	 = new Mat(nSrcImg))
			using (Mat model = new Mat(cContextPath))
			using (Mat descriptors1 = new Mat())
			using (Mat descriptors2 = new Mat())
			{
				KeyPoint[] keypoints1, keypoints2;
				detector.DetectAndCompute(nSrcImg, null, out keypoints1, descriptors1);
				detector.DetectAndCompute(model  , null, out keypoints2, descriptors2);

				// Match descriptor vectors 
				var bfMatcher = new BFMatcher(NormTypes.L2, false);
				var flannMatcher = new FlannBasedMatcher();
				//DMatch[][] bfMatches = bfMatcher.KnnMatch(descriptors1, descriptors2, k);
				DMatch[][] flannMatches = flannMatcher.KnnMatch(descriptors1, descriptors2, k);

				Mat mask = new Mat(flannMatches.Length, 1, MatType.CV_8U);
				int nonZero = Cv2.CountNonZero(mask);
				List<OpenCvSharp.Point2f> obj = new List<OpenCvSharp.Point2f>();
				List<OpenCvSharp.Point2f> scene = new List<OpenCvSharp.Point2f>();
				List<OpenCvSharp.DMatch> goodMatchesList = new List<OpenCvSharp.DMatch>();
				//iterate through the mask only pulling out nonzero items because they're matches

		
				//foreach (DMatch[] Matches in flannMatches.Where(x => x.Length > 1)) 
				//{
    //                foreach (DMatch Match in Matches)
    //                {
				//		if (Matches[0].Distance < 0.7 * Matches[1].Distance)
				//		{
				//			obj.Add(keypoints1[Matches[0].QueryIdx].Pt);
				//			scene.Add(keypoints2[Matches[0].TrainIdx].Pt);
				//			goodMatchesList.Add(Matches[0]);
				//		}
				//	}
				
				//}

				List<OpenCvSharp.Point2d> objPts = obj.ConvertAll(delegate (OpenCvSharp.Point2f point) { return new OpenCvSharp.Point2d(point.X, point.Y); });
				List<OpenCvSharp.Point2d> scenePts = scene.ConvertAll(delegate (OpenCvSharp.Point2f point) { return new OpenCvSharp.Point2d(point.X, point.Y); });

				if (nonZero >= 4)
				{
					//Point2f pt1_Ref = keypointsR[good_matches[index1].trainIdx].pt;
					//Point2f pt2_Ref = keypointsR[good_matches[index2].trainIdx].pt;
					//
					//Point2f pt1_Test = keypointsT[good_matches[index1].queryIdx].pt;
					//Point2f pt2_Test = keypointsT[good_matches[index2].queryIdx].pt;

					Mat homography = Cv2.FindHomography(objPts, scenePts, HomographyMethods.Ransac, 5);
					nonZero = Cv2.CountNonZero(mask);

					if (homography != null)
					{
						OpenCvSharp.Point2f[] obj_corners = { new OpenCvSharp.Point2f(0, 0),
									             new OpenCvSharp.Point2f(model.Cols - 1, 0),
									             new OpenCvSharp.Point2f(model.Cols - 1, model.Rows - 1),
									             new OpenCvSharp.Point2f(0, model.Rows - 1) };
						OpenCvSharp.Point2f[] scene_corners = Cv2.PerspectiveTransform(obj_corners, homography);

						
					}
				}
			
				//뷰어
				//var bfView = new Mat();
				//Cv2.DrawMatches(nSrcImg, keypoints1, model, keypoints2, bfMatches, bfView);
				//var flannView = new Mat();
				//Cv2.DrawMatches(nSrcImg, keypoints1, model, keypoints2, flannMatches, flannView);
				//
				//using (var win1 = new Window("SURF matching (by BFMather)", WindowFlags.AutoSize))
				//using (var win2 = new Window("SURF matching (by FlannBasedMatcher)", WindowFlags.AutoSize))
				//{
				//	win1.Image = bfView;
				//	win2.Image = flannView;
				//
				//	Cv2.WaitKey();
				//}
			}

			return true;
		}


	}
}
