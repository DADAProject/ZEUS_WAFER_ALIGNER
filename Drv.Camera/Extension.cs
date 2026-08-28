using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace System.Threading
{
    public static class ThreadExtension
    {
        public static void Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);
            while (dateTimeAdd >= dateTimeNow)
            {
                System.Windows.Forms.Application.DoEvents();
                dateTimeNow = DateTime.Now;
            }
            return;
        }
    }
    public static class BitmapExtension
    {
        #region << API32 >>

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        #endregion


        public unsafe static Bitmap ImageFromRawGrayPtr(IntPtr pBuffer, int width, int height)
        {
            var output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            var rect = new Rectangle(0, 0, width, height);
            var bmpData = output.LockBits(rect,
                ImageLockMode.ReadWrite, output.PixelFormat);
            var ptr = bmpData.Scan0;
            long numbytes = width * height;
            CopyMemory(ptr, pBuffer, (uint)(numbytes));
            output.UnlockBits(bmpData);
            ColorPalette cp = output.Palette;

            // init palette
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);

            // set palette back
            output.Palette = cp;

            return output;
        }

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
