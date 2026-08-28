using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Util
{
    public class PerformanceTimer : IDisposable
    {
        private readonly double mStartStemp;

        private readonly string mTitle;
        public double Elapsed_sec
        {
            get
            {
                return (Stopwatch.GetTimestamp() - mStartStemp) / Stopwatch.Frequency;
            }
        }
        public double Elapsed_ms
        {
            get
            {
                return (Stopwatch.GetTimestamp() - mStartStemp) / Stopwatch.Frequency * 1000;
            }
        }
        public double Elapsed_us
        {
            get
            {
                return (Stopwatch.GetTimestamp() - mStartStemp) / Stopwatch.Frequency * 1000000;
            }
        }

        public PerformanceTimer(string pTitle)
        {
            mTitle = pTitle;
            mStartStemp = Stopwatch.GetTimestamp();
        }


        public void Dispose()
        {
        }
    }
}
