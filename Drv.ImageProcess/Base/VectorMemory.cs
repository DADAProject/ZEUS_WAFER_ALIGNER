using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Base
{
    public unsafe ref struct VectorMemory<T> where T : unmanaged
    {
        public static void Copy(T[] src, T[] Dest)
        {
            var offset = Vector<T>.Count;
            int i = 0; int cnt = 0;
            for (i = 0; cnt < src.Length / offset; i += offset)
            {
                var v1 = new Vector<T>(src, i);
                v1.CopyTo(Dest, i);
                cnt++;
            }

            //remaining items
            for (i=0; i < src.Length; ++i) Dest[i] = src[i];
        }

        public static void ParallelCopy(T[] src, T[] Dest)
        {
            var offset = Vector<T>.Count;
            int i = 0;
            Parallel.For(0, src.Length / offset, (cnt) =>
            {
                var v1 = new Vector<T>(src, i);
                v1.CopyTo(Dest, i);
                i += offset;
            });

            //remaining items
            for (i = 0; i < src.Length; ++i) Dest[i] = src[i];
        }
    }
}
