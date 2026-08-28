using System;
using System.Runtime.InteropServices;

namespace Drv.ImageProcess
{
    static unsafe class Unmanaged
    {
        public static void* malloc<T>(int elementCount)
            where T : struct
        {
            return Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)) *
                        elementCount).ToPointer();
        }

        public static void* calloc<T>(int elementCount)
            where T : struct
        {
            int newSizeInBytes = Marshal.SizeOf(typeof(T)) * elementCount;
            byte* newArrayPointer =
            (byte*)Marshal.AllocHGlobal(newSizeInBytes).ToPointer();

            for (int i = 0; i < newSizeInBytes; i++)
                *(newArrayPointer + i) = 0;

            return (void*)newArrayPointer;
        }

        public static void free(void* pointerToUnmanagedMemory)
        {
            Marshal.FreeHGlobal(new IntPtr(pointerToUnmanagedMemory));
        }

        public static void* realloc<T>(void* oldPointer, int newElementCount)
            where T : struct
        {
            return (Marshal.ReAllocHGlobal(new IntPtr(oldPointer),
                new IntPtr(Marshal.SizeOf(typeof(T)) * newElementCount))).ToPointer();
        }
    }
}
