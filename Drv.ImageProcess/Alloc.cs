using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;
using Euresys.Open_eVision_22_04;
using OpenCvSharp;

namespace Drv.ImageProcess
{
    internal static class Alloc
    {
        public static MIL_ID AppAlloc { get; set; }
        public static MIL_ID SystemAlloc { get; set; }

        #region << SYSYEM >>

        public static bool EVisionInitialize()
        {
            try
            {
                Easy.Initialize();
                Console.WriteLine($"Version : {Easy.Version}");
                //Console.WriteLine($"IsGPUAvailable : {Easy.IsGPUAvailable()}");
                Console.WriteLine($"NumberOfAvailableProcessorCores : {Easy.NumberOfAvailableProcessorCores}");
                Console.WriteLine($"MaxNumberOfProcessingThreads : {Easy.MaxNumberOfProcessingThreads}");
                //Console.WriteLine($"NumGPUs : {Easy.NumGPUs}");
                Easy.MaxNumberOfProcessingThreads = Easy.NumberOfAvailableProcessorCores;
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public static bool EVisionDispose()
        {
            Easy.Terminate();

            return true;
        }
        public static bool OpenCVInitialize()
        {
            try
            {
                new Mat();
                Console.WriteLine($"Version : {Cv2.GetVersionString()}");
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }


        public static bool MilAppAlloc()
        {
            if (AppAlloc != MIL.M_NULL) return true;

            AppAlloc = MIL.MappAlloc(MIL.M_DEFAULT, MIL.M_NULL);

            if (AppAlloc == MIL.M_NULL) return false;
            else return true;
        }
        public static bool MilAppDispose()
        {
            if (MIL.M_NULL != AppAlloc)
            {
                MIL.MappFree(AppAlloc);
            }

            return true;
        }

        public static bool MilSystemAlloc()
        {
            if (SystemAlloc != MIL.M_NULL) return true;

            SystemAlloc = MIL.MsysAlloc(MIL.M_SYSTEM_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_NULL);

            if (SystemAlloc == MIL.M_NULL) return false;
            else return true;
        }
        public static bool MilSystemDispose()
        {
            if (MIL.M_NULL != SystemAlloc)
            {
                MIL.MsysFree(SystemAlloc);
            }

            return true;
        }

        public static bool MilSystemTransmission(MIL_ID systemAlloc)
        {
            SystemAlloc = systemAlloc;
            return true;
        }
        #endregion
    }
}
