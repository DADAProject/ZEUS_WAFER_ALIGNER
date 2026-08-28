using System;
using System.Runtime.InteropServices;

namespace Drv.ImageProcess
{
    public static class WinAPI
    {
        #region << winmm >>
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
        public static extern uint TimeBeginPeriod(uint uMilliseconds);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
        public static extern uint TimeEndPeriod(uint uMilliseconds);
        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint timeGetDevCaps(ref TIMECAPS timeCaps, uint sizeTimeCaps);

        [StructLayout(LayoutKind.Sequential)]
        public struct TIMECAPS
        {
            internal int wPeriodMin;

            internal int wPeriodMax;
        }
        #endregion


    }
}
