using System;
using System.Diagnostics;

namespace eMachine
{

    public class cPerformanceTimer : IDisposable
    {
        private readonly double mStartStemp;

        private readonly string mTitle;

        //private readonly Stopwatch sw = new Stopwatch();
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

        public cPerformanceTimer(string pTitle)
        {
            mTitle = pTitle;
            //sw.Start();
            mStartStemp = Stopwatch.GetTimestamp();
        }


        public void Dispose()
        {
            var elapsed = Math.Round(Elapsed_sec,4);

            //Debug.WriteLine("{0} : Elapsed Time {1} {2}", this.Title, elapsed, sw.Elapsed);
            Debug.WriteLine("{0} : Elapsed Time {1}", mTitle, elapsed);
        }
    }

}
