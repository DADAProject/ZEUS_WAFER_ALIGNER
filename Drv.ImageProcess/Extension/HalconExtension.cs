using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;

namespace Drv.ImageProcess.Extension
{
    internal static class HalconExtension
    {
        internal static unsafe void SetClear(HImage image, byte byClearGV = 0)
        {
            IntPtr Pointer = image.GetImagePointer1(out HTuple Type, out HTuple Width, out HTuple Height);

            MemoryExtension.MemSet(Pointer, (int) byClearGV, Width * Height);
        }

        internal static void HobjectToHimage(HObject hobject, ref HImage image)
        {
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
        }

        internal static void HobjectToRGBHimage(HObject hobject, ref HImage image)
        {
            HTuple pointerRed, pointerGreen, pointerBlue, type, width, height;
            HOperatorSet.GetImagePointer3(hobject, out pointerRed, out pointerGreen, out pointerBlue, out type, out width, out height);
            image.GenImage3(type, width, height, pointerRed, pointerGreen, pointerBlue);
        }

    }
}
