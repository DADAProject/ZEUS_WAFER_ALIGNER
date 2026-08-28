using System.Collections.Generic;
using System.Linq;


namespace Drv.CameraController
{
    /// <summary>
    /// 카메라 관리 클레스
    /// </summary>
    public class cGrabber : IGrabber, System.Collections.IEnumerable
    {
        #region # PRIVATE READONLY MEMBERS #

        /// <summary>
        /// 카메라을 담고 있는 리스트 변수
        /// </summary>
        private readonly List<ICamera> mlstCameras = new List<ICamera>();

        /// <summary>
        /// 상위 그래버 컨트롤러
        /// </summary>
        private readonly IController mParentController;

        #endregion # PRIVATE READONLY MEMBERS #

        #region # PUBLIC PROPERTIES #

        /// <summary>
        /// 카메라의 ID로 축을 가져온다
        /// </summary>
        /// <param name="pCamera">카메라 ID</param>
        /// <returns>제어할 카메라 반환</returns>
        public ICamera this[int pCamera]
        {
            get
            { return mlstCameras.Cast<ICamera>().FirstOrDefault(p => p.ID == pCamera); }
        }

        /// <summary>
        /// 축의 이름으로 축을 가져온다
        /// </summary>
        /// <param name="pName">축 이름</param>
        /// <returns>제어할 축 반환</returns>
        public ICamera this[string pName]
        {
            get
            { return mlstCameras.Cast<ICamera>().FirstOrDefault(pAxis => pAxis.CameraName == pName); }
        }

        #endregion # PUBLIC PROPERTIES #

        /// <summary>cn
        /// 생성자
        /// </summary>
        /// <param name="pMasterContoller">상위 컨트롤러 인터페이스</param>
        public cGrabber(IController pMasterContoller)
        {
            mParentController = pMasterContoller;
        }
        /// <summary>
        /// 카메라 추가
        /// </summary>
        /// <param name="pAxis">카메라 정보</param>
        public void AddCamera(ICamera pCamera)
        {
            if (this[pCamera.ID] != null)
            {
                throw new cCameraControlerException(string.Format("{0}의 ID가 이미 추가되어 있습니다.", pCamera.ID));
            }
            if (this[pCamera.CameraName] != null)
            {
                throw new cCameraControlerException(string.Format("{0}가 이미 추가되어 있습니다.", pCamera.CameraName));
            }
            mlstCameras.Add(pCamera);
        }

        /// <summary>
        /// Foreach 문을 사용하기 위한 함수
        /// </summary>
        /// <returns>축 1개씩 반환</returns>
        public System.Collections.IEnumerator GetEnumerator() {
            return mlstCameras.GetEnumerator();
        }

        //멀티 그랩으로 변경 해야함
        //public bool MxMove(string[] pAxes, double[] pPos, int pSpeedPercent, bool pWaitMotionEnd = false)
        //{
        //    return false;
        //}


    }
}