using System.Runtime.InteropServices;

namespace BarcodeReaderLibrary
{
    public class QueryTimer
    {
        [DllImport("kernel32.dll")]
        extern static short QueryPerformanceCounter(ref long x);
        [DllImport("kernel32.dll")]
        extern static short QueryPerformanceFrequency(ref long x);

        private long _lCtr1 = 0;
        private long _lCtr2 = 0;
        private long _lFreq = 0;

        public void Reset()
        {
            QueryPerformanceCounter(ref _lCtr1);
        }

        public double Elapsed
        {
            get
            {
                QueryPerformanceFrequency(ref _lFreq);
                QueryPerformanceCounter(ref _lCtr2);
                return (_lCtr2 - _lCtr1) * 1000.0 / _lFreq;
            }
        }
    }
}
