using Cognex.VisionPro;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Extension
{
    internal static class OpencvExtension
    {
        internal static unsafe void MatbufChildCopy(Mat parent, int nOffsetX, int nOffsetY, int nSizeW, int nSizeH, Mat child)
        {
            int iSizeX = nOffsetX + nSizeW;
            int iSizeY = nOffsetY + nSizeH;
            
            long iStep      = parent.Step();
            long iElemSize  = parent.ElemSize();
            int  iChannel   = parent.Channels();
            byte* src       = (byte*)parent.DataPointer;
            byte* dest      = (byte*)child.DataPointer;

            for (int y = nOffsetY; y < iSizeY; y++)
            {
                for (int x = nOffsetX; x < iSizeX; x++)
                {
                    if (iChannel == 1)
                    {
                        dest[y * iStep + x * iElemSize + 0] = (byte)src[y * iStep + x * iElemSize + 0];
                    }
                    else
                    {
                        dest[y * iStep + x * iElemSize + 0] = (byte)src[y * iStep + x * iElemSize + 0];
                        dest[y * iStep + x * iElemSize + 1] = (byte)src[y * iStep + x * iElemSize + 1];
                        dest[y * iStep + x * iElemSize + 2] = (byte)src[y * iStep + x * iElemSize + 2];
                    }

                }
            }

            //속도 체크 필요
            //var srcspan     = new Span<byte>(parent.DataPointer, parent.Width * parent.Height * parent.Channels());
            //var destspan    = new Span<byte>(child.DataPointer, child.Width * child.Height * child.Channels());
            //int numVectors  = srcspan.Length / Vector<byte>.Count; 
            //int ceiling     = numVectors * Vector<byte>.Count;

            //ReadOnlySpan<Vector<byte>> srcVecArray = MemoryMarshal.Cast<byte, Vector<byte>>(srcspan);
            //Span<Vector<byte>> destVecArray        = MemoryMarshal.Cast<byte, Vector<byte>>(destspan);

            //for (int i = 0; i < numVectors; i++)
            //{
            //    destVecArray[i] = srcVecArray[i];
            //}
        }

        internal static unsafe void MatbufCopy(Mat mSrcID, Mat mDstID)
        {
            byte* src  = (byte*)mSrcID.DataPointer;
            byte* dest = (byte*)mDstID.DataPointer;

            Buffer.MemoryCopy(src, dest, mSrcID.Width * mSrcID.Height, mDstID.Width * mDstID.Height);
        }


        internal static void ToMatPoint(this List<System.Drawing.PointF> parent, out OpenCvSharp.Point[] child)
        {
            List<OpenCvSharp.Point> temp = parent.ConvertAll(delegate (System.Drawing.PointF point) { return new OpenCvSharp.Point(point.X, point.Y); });
            child = temp.ToArray();
            //속도 체크 필요
            //int iLength = parent.Count;
            //child = new OpenCvSharp.Point[iLength];
            //
            //for (int i = 0; i < iLength; i++)
            //{
            //    child[i].X = (int) parent[i].X;
            //    child[i].Y = (int) parent[i].X;
            //}
        }

        internal static unsafe CogImage8Grey ToCogImage8Grey(Mat mSrcID)
        {
            CogImage8Grey mDstID = new CogImage8Grey(mSrcID.Width, mSrcID.Height);

            CogImage8Root Root = new CogImage8Root();
            Root.Initialize(mSrcID.Width, mSrcID.Height, mSrcID.Ptr(0, 0), mSrcID.Width, null);
            mDstID.SetRoot(Root);

            return mDstID;
        }



    }
}
