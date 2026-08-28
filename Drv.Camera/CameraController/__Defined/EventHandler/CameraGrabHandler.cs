using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Drv.CameraController
{
    public class GrabEventArg : IDisposable
    {
        /// <summary>
        /// Camera Width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Camera Height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Camera PixelFormat
        /// </summary>
        public string PixelFormat { get; set; }

        /// <summary>
        /// Camera Image Array
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Camera Image Pointer
        /// </summary>
        public IntPtr ImagePtr { get; set; }

        ~GrabEventArg()
        {
            Dispose();
        }
        public void Dispose()
        {
            Image = null;
            GC.SuppressFinalize(this);
            if(cVision.Instance.UseAutoGCCollector) Task.Run(() => GC.Collect()); //MemoryFree
        }

        public System.Drawing.Bitmap ToBitmap(PixelFormat format)
        {
            Bitmap temp = null;

            if (format == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                temp = BitmapExtension.ImageFromRawGrayPtr(this.ImagePtr, this.Width, this.Height);
            }

            return temp;
        }
    }

    public delegate void CameraGrabHandler(ICamera pSender, GrabEventArg e);
}