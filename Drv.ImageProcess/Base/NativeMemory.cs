using System;
using System.Runtime.InteropServices;

namespace Drv.ImageProcess
{
    //메모리를 스택에 할당하여 가비지 호출(2세대)을 최소화함
    public unsafe ref struct NativeMemory<T> where T : unmanaged
    {
        int _size;
        IntPtr _ptr;

        public NativeMemory(int size)
        {
            _size = size;

            long lSize = _size;
            lSize *= sizeof(T);

            IntPtr bufSize = new IntPtr(lSize);
            _ptr = Marshal.AllocHGlobal(bufSize);
        }

        public Span<T> GetView()
        {
            return new Span<T>(_ptr.ToPointer(), _size);
        }

        public void Dispose()
        {
            if (_ptr == IntPtr.Zero)
            {
                return;
            }

            Marshal.FreeHGlobal(_ptr);
            _ptr = IntPtr.Zero;
        }
    }
}
