using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Euresys.Open_eVision_22_04;
using System.Runtime.Remoting.Contexts;
using Cognex.VisionPro.ImageFile;
using Matrox.MatroxImagingLibrary;
using Drv.ImageProcess.Extension;

namespace Drv.ImageProcess.Core
{
	internal partial class GMF
	{
        internal bool GeometrictModelFinder(CogImage8Grey nSrcImg, CogImage8Grey nDstImg, string cContextPath,
            GMF_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
            out double dCenterX, out double dCenterY, out double dAngle, out double dScore, GMF_OPERATION nOper)
        {
            dCenterX = 0.0; dCenterY = 0.0; dAngle = 0.0; dScore = 0.0;

            int nNumResults = 0;

            CogPMAlignTool Match = new CogPMAlignTool();

            CogImage8Grey nContextImg;

            using (var bmp = new Bitmap(cContextPath))
            {
                nContextImg = new CogImage8Grey(bmp);
            }

            //이미지 포맷 버그
            // using (CogImageFileTool Tool = new CogImageFileTool())
            // {
            //     Tool.Operator.Open(cContextPath, CogImageFileModeConstants.Read);
            //     Tool.Run();
            //
            //     nContextImg =  CogImageConvert.GetIntensityImage(Tool.OutputImage, 0, 0, Tool.OutputImage.Width, Tool.OutputImage.Height);
            // }

            Match.Pattern.Origin.TranslationX = nContextImg.Width / 2.0;
            Match.Pattern.Origin.TranslationY = nContextImg.Height / 2.0;
            Match.Pattern.Origin.Rotation = 1;

            Match.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.BestTrained;
            Match.RunParams.RunMode = CogPMAlignRunModeConstants.SearchImage;
            Match.RunParams.ApproximateNumberToFind = 1;
            Match.RunParams.Timeout = 200.0;
            Match.RunParams.TimeoutEnabled = true;

            Match.Pattern.TrainMode = CogPMAlignTrainModeConstants.Image;
            Match.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;

            //Match.LastRunRecordEnable |= CogPMAlignLastRunRecordConstants.ResultsMatchShapeModels;
            //Match.RunParams.OwnedFlexParams.SaveDeformationInfo = CogPMAlignFlexDeformationInfoConstants.TransformAndUnwarpData;

            Match.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
            Match.RunParams.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;

            if (bAngleSet1 == GMF_SETANGLE_OPERATION.E_GMF_SETANGLE_ENABLE)
            {
                Match.RunParams.ZoneAngle.Low = Math.PI * nAngleSet2 / 180.0;
                Match.RunParams.ZoneAngle.High = Math.PI * nAngleSet3 / 180.0;
            }
            else
            {
                Match.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                Match.RunParams.ZoneAngle.Low = -0.052359858837820572;
                Match.RunParams.ZoneAngle.High = 0.052359858837820572;
            }

            Match.RunParams.ZoneScale.Low  = 0.95;
            Match.RunParams.ZoneScale.High = 1.05;

            Match.RunParams.ScoreUsingClutter = true;
            //if (Match.RunParams.ScoreUsingClutter)
            //{
            //    Match.RunParams.AcceptThreshold = (double)nAcceptancdSet / 100;
            //}

            CogRectangleAffine PatMaxTrainRegion = Match.Pattern.TrainRegion as CogRectangleAffine;
            if ((PatMaxTrainRegion != null))
            {
                PatMaxTrainRegion.SetOriginLengthsRotationSkew(0, 0, nContextImg.Width, nContextImg.Height, 0, 0);
                PatMaxTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position | CogRectangleAffineDOFConstants.Rotation | CogRectangleAffineDOFConstants.Size;
            }
            Match.Pattern.TrainImage = nContextImg;
            Match.Pattern.Train();

            Match.InputImage = nSrcImg;
            Match.Run();

            if (Match.Results == null || Match.Results.Count == 0)
            {
                Match.Results?.Dispose();
                nContextImg?.Dispose();
                Match?.Dispose();

                return false;
            }

            nNumResults = (int)Match.Results.Count;

            double[] pdPosX, pdPosY;
            double[] pdScore, pdAngle;
            pdPosX = new double[nNumResults];
            pdPosY = new double[nNumResults];
            pdAngle = new double[nNumResults];
            pdScore = new double[nNumResults];

            for (int i = 0; i < nNumResults; i++)
            {
                CogPMAlignResult MatchData = Match.Results[i];
                CogTransform2DLinear Pose = MatchData.GetPose();

                pdPosX[i] = Pose.TranslationX;
                pdPosY[i] = Pose.TranslationY;
                pdAngle[i] = Pose.Rotation * 180.0 / Math.PI;
                pdScore[i] = MatchData.Score;
            }

            double dMaxScore = 0;
            int nMax = 0;

            for (int nIndex = 0; nIndex < nNumResults; nIndex++)
            {
                if (pdScore[nIndex] > dMaxScore)
                {
                    dMaxScore = pdScore[nIndex];
                    nMax = nIndex;
                }
            }

            int nSizeX = nContextImg.Width;
            int nSizeY = nContextImg.Height;

            int nSrcImgSizeX = nSrcImg.Width;
            int nSrcImgSizeY = nSrcImg.Height;

            int nMstX = (int)(pdPosX[nMax] - (nSizeX / 2));
            int nMstY = (int)(pdPosY[nMax] - (nSizeY / 2));

            if (nMstX + nSizeX < nSrcImgSizeX &&
                nMstY + nSizeY < nSrcImgSizeY &&
                nMstX >= 0 &&
                nMstY >= 0)
            {
                if (nOper == GMF_OPERATION.E_GMF_DST_IMG)
                {
                    //테스트 필요
                    //등록된 마크 크기만큼 이미지 카피
                    //인트 포인터 로 바꺼서 
                    ICogImage8PixelMemory[] mChildSrc = null, mChildDst = null;
                    VisionProExtension.bufChild2d(nSrcImg, nMstX, nMstY, nSizeX, nSizeY, out mChildSrc);
                    VisionProExtension.bufChild2d(nDstImg, nMstX, nMstY, nSizeX, nSizeY, out mChildDst);
                    VisionProExtension.bufCopy(mChildSrc, mChildDst);
                }
                else if (nOper == GMF_OPERATION.E_GMF_RESULT_IMG)
                {
                    //결과 이미지 그려줌
                }
                else if (nOper == GMF_OPERATION.E_GMF_SRC_IMG)
                {
                    //결과 퓨처
                }
                else { }
            }

            Match.Results?.Dispose();
            nContextImg?.Dispose();
            Match?.Dispose();

            return true;
        }

        internal bool GeometrictModelFinder(CogImage8Grey nSrcImg, CogImage8Grey nDstImg, string cContextPath,
            GMF_SETANGLE_OPERATION bAngleSet1, int nAngleSet2, int nAngleSet3,
            out double[] dCenterX, out double[] dCenterY, out double[] dAngle, out double[] dScore, GMF_OPERATION nOper)
        {
            bool bRet = false;

            int nTotalPat = 0;
            dCenterX = null; dCenterY = null; dAngle = null; dScore = null;

            CogPMAlignTool Match = new CogPMAlignTool();

            CogImage8Grey nContextImg;

            using (CogImageFileTool Tool = new CogImageFileTool())
            {
                Tool.Operator.Open(cContextPath, CogImageFileModeConstants.Read);
                Tool.Run();
                nContextImg = Tool.OutputImage as CogImage8Grey;
            }

            Match.Pattern.Origin.TranslationX = nContextImg.Width  / 2.0;
            Match.Pattern.Origin.TranslationY = nContextImg.Height / 2.0;


            Match.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.BestTrained;
            Match.RunParams.RunMode      = CogPMAlignRunModeConstants.SearchImage;
            Match.RunParams.ApproximateNumberToFind = 1;
            Match.RunParams.Timeout = 200.0;
            Match.RunParams.TimeoutEnabled = true;

            Match.Pattern.TrainMode      = CogPMAlignTrainModeConstants.Image;
            Match.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;

            //Match.LastRunRecordEnable |= CogPMAlignLastRunRecordConstants.ResultsMatchShapeModels;
            //Match.RunParams.OwnedFlexParams.SaveDeformationInfo = CogPMAlignFlexDeformationInfoConstants.TransformAndUnwarpData;


            if (bAngleSet1 == GMF_SETANGLE_OPERATION.E_GMF_SETANGLE_ENABLE)
            {
                Match.RunParams.ZoneAngle.Low  = Math.PI * nAngleSet2 / 180.0;
                Match.RunParams.ZoneAngle.High = Math.PI * nAngleSet3 / 180.0;
            }
            else
            {
                Match.RunParams.ZoneAngle.Low = -0.052359858837820572;
                Match.RunParams.ZoneAngle.High = 0.052359858837820572;
            }

            Match.RunParams.ZoneScale.Low = 0.95;
            Match.RunParams.ZoneScale.High = 1.05;


            Match.RunParams.ScoreUsingClutter = true;

            //if (Match.RunParams.ScoreUsingClutter)
            //{
            //    Match.RunParams.AcceptThreshold = (double)nAcceptancdSet / 100;
            //}

            CogRectangleAffine PatMaxTrainRegion = Match.Pattern.TrainRegion as CogRectangleAffine;
            if ((PatMaxTrainRegion != null))
            {
                PatMaxTrainRegion.SetOriginLengthsRotationSkew(0, 0, nContextImg.Width, nContextImg.Height, 0, 0);
                PatMaxTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position | CogRectangleAffineDOFConstants.Rotation | CogRectangleAffineDOFConstants.Size;
            }
            Match.Pattern.TrainImage = nContextImg;
            Match.Pattern.Train();

            Match.InputImage = nSrcImg;
            Match.Run();

            if (Match.Results != null)
            {
                nTotalPat = (int)Match.Results.Count;

                if (nTotalPat > 0)
                {
                    bRet = true;

                    double[] pdposX, pdposY;
                    double[] pdscore, pdangle;
                    pdposX = new double[nTotalPat];
                    pdposY = new double[nTotalPat];
                    pdangle = new double[nTotalPat];
                    pdscore = new double[nTotalPat];

                    for (int i = 0; i < nTotalPat; i++)
                    {
                        CogPMAlignResult MatchData = Match.Results[i];
                        CogTransform2DLinear Pose = MatchData.GetPose();

                        pdposX[i] = Pose.TranslationX;
                        pdposY[i] = Pose.TranslationY;
                        pdangle[i] = Pose.Rotation * 180.0 / Math.PI;
                        pdscore[i] = MatchData.Score;     
                    }

                    dCenterX = pdposX;
                    dCenterY = pdposY;
                    dAngle = pdangle;
                    dScore = pdscore;
                }

                if (nOper == GMF_OPERATION.E_GMF_DST_IMG)
                {
                    //VisionProExtension.CmodDrawResults(nDstImg, nContextImg, CogPMAlignResultGraphicConstants.MatchRegion, Match.Results);
                    //등록된 마크 크기만큼 이미지 카피
                }
                else if (nOper == GMF_OPERATION.E_GMF_RESULT_IMG)
                {
                    //결과 이미지 그려줌
                }
                else if (nOper == GMF_OPERATION.E_GMF_SRC_IMG)
                {
                    //결과 퓨처 넣기
                }
                else { }
            }

            Match.Results?.Dispose();
            nContextImg?.Dispose();
            Match?.Dispose();

            return bRet;
        }
    }
}
