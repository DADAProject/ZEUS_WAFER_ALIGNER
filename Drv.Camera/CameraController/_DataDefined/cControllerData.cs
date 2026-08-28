namespace Drv.CameraController
{
    public class cControllerData
    {
        /// <summary>
        /// 컨트롤러 초기화 데이터
        /// </summary>
        public string InitData;

        /// <summary>
        /// 컨트롤러 타입
        /// </summary>
        public eControllerType ControllerType;

        /// <summary>
        /// 컨트롤러 아이디
        /// </summary>
        public int ControllerID;

        /// <summary>
        /// 컨트롤러 이름
        /// </summary>
        public string ControllerName;

        /// <summary>
        /// 컨트롤러에 연결된 카메라정보
        /// </summary>
        public cCameraDatas[] CameraDatas = new cCameraDatas[0];
    }
}
