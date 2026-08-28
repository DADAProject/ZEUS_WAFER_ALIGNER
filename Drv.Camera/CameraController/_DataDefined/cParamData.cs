namespace Drv.CameraController
{
    public class cParamData
    {
        /// <summary>
        /// GigE Heartbeat
        /// </summary>
        public int Heartbeat = 5000000;


        /// <summary>
        /// StreamBufferCount
        /// </summary>
        public int BufferCount = 1;

        /// <summary>
        /// ExposureMode
        /// </summary>
        public string ExposureMode = "Timed";

        
        /// <summary>
        /// ExposureTime
        /// </summary>
        public double ExposureTime = 47458.0;

        /// <summary>
        /// Gain
        /// </summary>
        public double Gain = 0;
        
        /// <summary>
        /// TriggerMode
        /// </summary>
        public string TriggerMode = "On";

        /// <summary>
        /// TriggerSource
        /// </summary>
        public string TriggerSource = "Software";


        /// <summary>
        /// TriggerSelector
        /// </summary>
        public string TriggerSelector = "FrameStart";


        /// <summary>
        /// Camera File
        /// </summary>
        public string CamFilePath = "";


        /// <summary>
        /// Camera CamConnector
        /// </summary>
        public string CamConnector = "";


        public bool UseOnlyCameFile = false;

    }
}
