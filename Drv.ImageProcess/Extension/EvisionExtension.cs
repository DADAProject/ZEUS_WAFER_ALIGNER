using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Extension
{
    internal static class EvisionExtension
    {
        internal static unsafe EROIBW8 SetROI(BUFF info)
        {
            EROIBW8 ROI = new EROIBW8();
            ROI.Attach(info.EbuffID);
            ROI.SetPlacement(info.ROI.X, info.ROI.Y, info.ROI.Width, info.ROI.Height);
            return ROI;
        }

        internal static unsafe void SetClear(EImageBW8 image, byte byClearGV = 0)
        {
            IntPtr ptr = image.GetImagePtr();
            MemoryExtension.MemSet(ptr, (int)byClearGV, image.Width * image.Height);
        }

        internal static unsafe void SetPointer(EImageBW8 image, int pWidth, int pHeight, byte[] pPixels)
        {
            IntPtr CopyPtr = Marshal.AllocHGlobal(pWidth * pHeight);
            Marshal.Copy(pPixels, 0, CopyPtr, pWidth * pHeight);
            image.SetImagePtr(pWidth, pHeight, CopyPtr);
            Marshal.FreeHGlobal(CopyPtr);
        }

        internal static unsafe Bitmap ToBitmap(EImageBW8 image, PixelFormat pPixelFormat = PixelFormat.Format8bppIndexed)
        {
            if (pPixelFormat == PixelFormat.Undefined) return null;
            if (pPixelFormat == PixelFormat.DontCare ) return null;
            if (pPixelFormat == PixelFormat.Max      ) return null;
            if (pPixelFormat == PixelFormat.Gdi      ) return null;
            if (pPixelFormat == PixelFormat.Alpha    ) return null;
            if (pPixelFormat == PixelFormat.PAlpha   ) return null;
            if (pPixelFormat == PixelFormat.Canonical) return null;
            if (pPixelFormat == PixelFormat.Extended ) return null;


            if (pPixelFormat == PixelFormat.Indexed              ||
                pPixelFormat == PixelFormat.Format1bppIndexed    ||
                pPixelFormat == PixelFormat.Format4bppIndexed    ||
                pPixelFormat == PixelFormat.Format8bppIndexed    ||
                pPixelFormat == PixelFormat.Format16bppGrayScale  )
            {
                //Draw 안되느거

                //Graphics graphics = Graphics.FromImage(bmp);
                //image.Draw(graphics);
                //graphics?.Dispose();
            }
            else
            {
                Bitmap bmp = new Bitmap(image.Width, image.Height, pPixelFormat);
                Graphics graphics = Graphics.FromImage(bmp);
                image.Draw(graphics);
                graphics?.Dispose();

                return bmp;
            }

            return null;
        }
    }
}
