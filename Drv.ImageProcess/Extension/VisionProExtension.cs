using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.Implementation.Internal;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ToolBlock;
using Drv.ImageProcess.Base;
using Drv.ImageProcess.Core;
using Drv.ImageProcess.Util;
using Euresys.Open_eVision_22_04;
using HalconDotNet;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;
using OpenCvSharp.Features2D;
using static System.Net.Mime.MediaTypeNames;

namespace Drv.ImageProcess.Extension
{
    internal static class VisionProExtension
    {
        //internal static unsafe CogImage8Grey SetROI(BUFF info)
        //{
        //    using (CogCopyRegion region = new CogCopyRegion())
        //    {
        //        region.
        //    }
        //    return ROI;
        //}

        internal static unsafe void SetClear(CogImage8Grey mSrcID, byte byClearGV = 0)
        {
            //Need Checked
            //Set?
            var Pointer = mSrcID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mSrcID.Width, mSrcID.Height);
        
            MemoryExtension.MemSet(Pointer.Scan0, (int) byClearGV, mSrcID.Width * mSrcID.Height);
        }
        internal static unsafe void bufCopy(CogImage8Grey mSrcID, CogImage8Grey mDstID)
        {
            //Need Checked
            //Set?
            var SrcPointer = mSrcID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mSrcID.Width, mSrcID.Height);
            var DstPointer = mDstID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mDstID.Width, mDstID.Height);

            MemoryExtension.CopyMemory(DstPointer.Scan0, SrcPointer.Scan0, (uint)(mDstID.Width * mDstID.Height));
        }

        internal static unsafe void bufCopy(Mat mSrcID, CogImage8Grey mDstID)
        {
            var Pointer = mDstID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mSrcID.Width, mSrcID.Height);

            int iWidth = Pointer.Width;
            int iHeight = Pointer.Height;
            int iStride = Pointer.Stride;

            for (int y = 0; y < iHeight; y++)
            {
                //get the data from the new image
                byte* nSrcRow = (byte*)mSrcID.Ptr(0, 0).ToPointer() + (y * iWidth);
                byte* nDstRow = (byte*)Pointer.Scan0.ToPointer() + (y * iStride);

                Buffer.MemoryCopy(nSrcRow, nDstRow, iWidth, iWidth);
            }
        }

        internal static unsafe void bufCopy(ICogImage8PixelMemory[] mSrcID,ICogImage8PixelMemory[] mDstID)
        {
            for (int y = 0; y < mSrcID.Length; y++)
            {
                //get the data from the new image
                byte* nSrcRow = (byte*)mSrcID[y].Scan0.ToPointer();
                byte* nDstRow = (byte*)mDstID[y].Scan0.ToPointer();

                Buffer.MemoryCopy(nSrcRow, nDstRow, mSrcID[y].Width, mDstID[y].Width);
            }
        }
        internal static unsafe void bufChild2d(CogImage8Grey mSrcID, int mOffsetX, int mOffsetY, int pSizeX, int pSizeY, out ICogImage8PixelMemory[] pBufIdPtr)
        {
            pBufIdPtr = new ICogImage8PixelMemory[pSizeY];

            for (int y = 0; y < pSizeY; y++)
            {
                int iOffsetY = mOffsetY + y;
                pBufIdPtr[y] = mSrcID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, mOffsetX, iOffsetY, pSizeX, 1);
            }
        }
    
        //internal static unsafe void bufCopy(CogImage8Grey mSrcID, IntPtr mBuffer)
        //{
        //    CogImage8Root Root = new CogImage8Root();
        //    Root.Initialize(mSrcID.Width, mSrcID.Height, mBuffer, mSrcID.Width, null);
        //    mSrcID.SetRoot(Root);
        //}

        //internal static unsafe CogImage8Grey[] ToArray(BUFF[] mSrcID)
        //{
        //    return mSrcID.Select(x => x.CbuffID).ToArray();
        //}

        internal static unsafe CogImage8Grey ToRecordImage(CogImage8Grey mSrcID, CogRecord mRecord)
        {
            CogImage8Grey mDst6;

            Mat mat = new Mat(mSrcID.Width, mSrcID.Height, MatType.CV_8UC1);

            var Pointer = mSrcID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mSrcID.Width, mSrcID.Height);

            Buffer.MemoryCopy(Pointer.Scan0.ToPointer(), mat.Ptr(0, 0).ToPointer(), mSrcID.Width * mSrcID.Height, mSrcID.Width * mSrcID.Height);

            Cv2.Rectangle(mat, new OpenCvSharp.Point(0, 0), new OpenCvSharp.Point(300, 300), Scalar.Red, 1);

            CogImage8Root Root = new CogImage8Root();
            Root.Initialize(mat.Width, mat.Height, mat.Ptr(0, 0), mat.Width, null);

            mDst6 = new CogImage8Grey(mat.Width, mat.Height);
            mDst6.SetRoot(Root);
            mat.Dispose();

            //MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE);

            //MIL.MgraArcFill(MIL.M_DEFAULT, mDst, stCenterpt.X, stCenterpt.Y, (double)(nFindRadius - iRadiusOffSet), (double)(nFindRadius - iRadiusOffSet), 0, 360);
            return mDst6;
        }

        internal static unsafe Mat ToMat(CogImage8Grey mSrcID)
        {
            Mat mDstID = new Mat(mSrcID.Height, mSrcID.Width, MatType.CV_8UC1);

            var Pointer = mSrcID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mSrcID.Width, mSrcID.Height);

            int iWidth  = Pointer.Width;
            int iHeight = Pointer.Height;
            int iStride = Pointer.Stride;

            for (int y = 0; y < iHeight; y++)
            {
                //get the data from the new image
                byte* nSrcRow = (byte*)Pointer.Scan0.ToPointer()    + (y * iStride);
                byte* nDstRow = (byte*)mDstID.Ptr(0, 0).ToPointer() + (y * iWidth );

                Buffer.MemoryCopy(nSrcRow, nDstRow, iWidth, iWidth);
            }

            return mDstID;
        }


        internal static unsafe void CgraCircleResults(CogImage8Grey mSrcID, CogFindCircleResults pCircle)
        {
            using (Mat mDstID = ToMat(mSrcID))
            {
                Color color = Color.FromName(pCircle.GetCircle().Color.ToString());
                Scalar scalar = Scalar.FromRgb((int)color.R, (int)color.G, (int)color.B);

                int iLineWidthInScreenPixels = pCircle.GetCircle().LineWidthInScreenPixels;

                Cv2.Circle(mDstID, (int)pCircle.GetCircle().CenterX, 
                                   (int)pCircle.GetCircle().CenterY, 
                                   (int)pCircle.GetCircle().Radius, scalar, iLineWidthInScreenPixels);

                for (int i = 0; i < pCircle.Count; i++)
                {
                    CogCaliperResults Caliper = pCircle[i].CaliperResults;

                    if (Caliper.Count <= 0) continue;

                    if (Caliper.Edges != null)
                    {
                        Cv2.Line(mDstID, (int)Caliper.Edges[0].PositionX - iLineWidthInScreenPixels, (int)Caliper.Edges[0].PositionY,
                                         (int)Caliper.Edges[0].PositionX + iLineWidthInScreenPixels, (int)Caliper.Edges[0].PositionY,
                                         pCircle[i].Used ? scalar : Scalar.FromRgb((int)color.R, 0, 0),
                                         iLineWidthInScreenPixels);

                        Cv2.Line(mDstID, (int)Caliper.Edges[0].PositionX, (int)Caliper.Edges[0].PositionY - iLineWidthInScreenPixels,
                                         (int)Caliper.Edges[0].PositionX, (int)Caliper.Edges[0].PositionY + iLineWidthInScreenPixels,
                                         pCircle[i].Used ? scalar : Scalar.FromRgb((int)color.R, 0, 0),
                                         iLineWidthInScreenPixels);
                    }

                }
    
                bufCopy(mDstID, mSrcID);
            }
        }

        internal static unsafe void CgraLineResults(CogImage8Grey mSrcID, CogFindLineResults pLine)
        {
            using (Mat mDstID = ToMat(mSrcID))
            {
                Color color = Color.FromName(pLine.GetLine().Color.ToString());
                Scalar scalar = Scalar.FromRgb((int)color.R, (int)color.G, (int)color.B);

                int iLineWidthInScreenPixels = pLine.GetLine().LineWidthInScreenPixels;

                var LineSegment = pLine.GetLineSegment();


                Cv2.Line(mDstID,(int)LineSegment.StartX, (int)LineSegment.StartY,
                                (int)LineSegment.EndX  , (int)LineSegment.EndY,
                                scalar, iLineWidthInScreenPixels);

                for (int i = 0; i < pLine.Count; i++)
                {
                    CogCaliperResults Caliper = pLine[i].CaliperResults;

                    if (Caliper.Count <= 0) continue;

                    if (Caliper.Edges != null)
                    {
                        Cv2.Line(mDstID, (int)Caliper.Edges[0].PositionX - iLineWidthInScreenPixels, (int)Caliper.Edges[0].PositionY,
                                         (int)Caliper.Edges[0].PositionX + iLineWidthInScreenPixels, (int)Caliper.Edges[0].PositionY,
                                         pLine[i].Used ? scalar : Scalar.FromRgb((int)color.R, 0, 0),
                                         iLineWidthInScreenPixels);

                        Cv2.Line(mDstID, (int)Caliper.Edges[0].PositionX, (int)Caliper.Edges[0].PositionY - iLineWidthInScreenPixels,
                                         (int)Caliper.Edges[0].PositionX, (int)Caliper.Edges[0].PositionY + iLineWidthInScreenPixels,
                                         pLine[i].Used ? scalar : Scalar.FromRgb((int)color.R, 0, 0),
                                         iLineWidthInScreenPixels);
                    }

                }



                bufCopy(mDstID, mSrcID);
            }
        }

        internal static unsafe void CmodDrawResult(CogImage8Grey mSrcID, CogImage8Grey mContextID, CogPMAlignResultGraphicConstants pGraphicOptions, CogPMAlignResult pAlign)
        {
            var SrcPointer = mContextID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mContextID.Width, mContextID.Height);
            var DstPointer = mSrcID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mSrcID.Width, mSrcID.Height);

            var Graphics = pAlign.CreateResultGraphics(pGraphicOptions);

            int iLineWidthInScreenPixels = Graphics.LineWidthInScreenPixels;

            var Transform2DLinear = pAlign.GetPose();

            int nMstX = (int)(Transform2DLinear.TranslationX - (int)(mContextID.Width / 2));
            int nMstY = (int)(Transform2DLinear.TranslationY - (int)(mContextID.Height / 2));

            int iWidth = SrcPointer.Width;
            int iHeight = SrcPointer.Height;
            int iStride = SrcPointer.Stride;

            int iPitch = DstPointer.Stride;

            //get the data from the new image
            for (int y = 0; y < iHeight; y++)
            {
                byte* nDstRow = (byte*)DstPointer.Scan0.ToPointer() + ((nMstY + y) * iPitch) + nMstX; //
                byte* nSrcRow = (byte*)SrcPointer.Scan0.ToPointer() + (y * iStride); //patten

                Buffer.MemoryCopy(nSrcRow, nDstRow, iWidth, iWidth);
            }
        }

        internal static unsafe void CmodDrawResults(CogImage8Grey mSrcID, CogImage8Grey mContextID, CogPMAlignResultGraphicConstants pGraphicOptions, CogPMAlignResults pAligns)
        {
            var SrcPointer = mContextID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mContextID.Width, mContextID.Height);
            var DstPointer = mSrcID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, mSrcID.Width, mSrcID.Height);

            for (int i = 0; i < pAligns.Count; i++)
            {
                var Graphics = pAligns[i].CreateResultGraphics(CogPMAlignResultGraphicConstants.Origin);
               Graphics.FitToImage(mContextID, 1, 1);
                CogRectangle rect = new CogRectangle();
                Graphics.FitToBoundingBox(rect);
                mContextID.ToBitmap().Save("test.bmp");
                //var RLE = regionPixel.CreateRLE(0, 255);
                //var image = RLE.CreateImage8Grey();
                //image.ToBitmap().Save("test.bmp"); 
                ////var image = RLE.DrawIntoImage8Grey();


                int iLineWidthInScreenPixels = Graphics.LineWidthInScreenPixels;

                var Transform2DLinear = pAligns[i].GetPose();

                int nMstX = (int)(Transform2DLinear.TranslationX - (int)(mContextID.Width / 2));
                int nMstY = (int)(Transform2DLinear.TranslationY - (int)(mContextID.Height / 2));

                int iWidth  = SrcPointer.Width;
                int iHeight = SrcPointer.Height;
                int iStride = SrcPointer.Stride;

                int iPitch = DstPointer.Stride;

                //get the data from the new image
                for (int y = 0; y < iHeight; y++)
                {
                    byte* nDstRow = (byte*)DstPointer.Scan0.ToPointer() + ((nMstY + y) * iPitch) + nMstX; //
                    byte* nSrcRow = (byte*)SrcPointer.Scan0.ToPointer() + (y * iStride); //patten

                    Buffer.MemoryCopy(nSrcRow, nDstRow, iWidth, iWidth);
                }
            }

        }


        //==============================================================
    }
}
