using Matrox.MatroxImagingLibrary;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Extension
{
    public static class BitmapExtension
    {
        internal static unsafe void bufCopy(this Bitmap bmp, byte[] pBuffer)
        {
            BitmapData bmpdata = null;

            try
            {
                int w = bmp.Width, h = bmp.Height;
                int s = bmpdata.Stride;

                bmpdata = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bmp.PixelFormat);
                int numbytes = s * h;
                //Marshal.Copy(pBuffer,0, bmpdata.Scan0, pBuffer.Length);
                byte* pPtr = (byte*) bmpdata.Scan0;

                var srcspan = new Span<byte>(pBuffer);
                var destspan = new Span<byte>(pPtr, numbytes);
                int numVectors = srcspan.Length / Vector<byte>.Count;
                int ceiling = numVectors * Vector<byte>.Count;
                ReadOnlySpan<Vector<byte>> srcVecArray = MemoryMarshal.Cast<byte, Vector<byte>>(srcspan);
                Span<Vector<byte>> destVecArray = MemoryMarshal.Cast<byte, Vector<byte>>(destspan);

                for (int i = 0; i < numVectors; i++)
                {
                    destVecArray[i] = srcVecArray[i];
                }

            }
            finally
            {
                if (bmpdata != null)
                    bmp.UnlockBits(bmpdata);
            }
        }

        internal static unsafe void bufCopy(this Bitmap bmp, IntPtr pBuffer)
        {
            BitmapData bmpdata = null;

            try
            {
                int w = bmp.Width, h = bmp.Height;
                int s = bmpdata.Stride;

                bmpdata = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bmp.PixelFormat);
                int numbytes = s * h;
                Buffer.MemoryCopy(pBuffer.ToPointer(), bmpdata.Scan0.ToPointer(), numbytes, numbytes);
            }
            finally
            {
                if (bmpdata != null)
                    bmp.UnlockBits(bmpdata);
            }
        }
        internal static Bitmap CreateGrayscaleImage(int width, int height)
        {
            // create new image
            Bitmap image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // set palette to grayscale
            SetGrayscalePalette(image);

            // return new image
            return image;
        }
        internal static void SetGrayscalePalette(this Bitmap image)
        {
            // check pixel format
             if (image.PixelFormat != PixelFormat.Format8bppIndexed)
                 throw new Exception("Source image is not 8 bpp image.");

            // get palette
            ColorPalette cp = image.Palette;

            // init palette
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);

            // set palette back
            image.Palette = cp;
        }



        //======================================

        public static unsafe void ChangeBitmapColor24(Bitmap bitmap, Color oldcolor, Color newcolor)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            if (bitmapData == null)
            {
                throw new InvalidOperationException("Unable to lock bitmap bits.");
            }

            try
            {
                byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
                int stride = bitmapData.Stride;

                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < stride; w += 3)
                    {
                        byte blue = *(ptr + (h * stride) + w);
                        byte green = *(ptr + (h * stride) + w + 1);
                        byte red = *(ptr + (h * stride) + w + 2);

                        if (blue == oldcolor.B && green == oldcolor.G && red == oldcolor.R)
                        {
                            *(ptr + (h * stride) + w) = newcolor.B;
                            *(ptr + (h * stride) + w + 1) = newcolor.G;
                            *(ptr + (h * stride) + w + 2) = newcolor.R;
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

    }
}
