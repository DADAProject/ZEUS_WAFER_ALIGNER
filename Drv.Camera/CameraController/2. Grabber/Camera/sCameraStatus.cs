namespace Drv.CameraController
{

    public enum GrabResult { Ready, Process, Good, Error }

    /// <summary>
    /// 카메라 상태
    /// </summary>
    public struct sCameraStatus
    {
        /// <summary>
        /// 카메라 Enable 상태
        /// </summary>
        public bool Enable;

        /// <summary>
        /// 카메라 Simulration 상태
        /// </summary>
        public bool SimEnable;

        /// <summary>
        /// 카메라 Simulration 경로
        /// </summary>
        public string SimPath;

        /// <summary>
        /// 카메라 연결 상태
        /// </summary>
        public bool Connection;

        /// <summary>
        /// 카메라 그랩 상태
        /// </summary>
        public GrabResult GrabResult;

        public object Tag;
    }

}