using System.Linq;
using System.Threading;

using Matrox.MatroxImagingLibrary;

namespace Drv.CameraController
{
    public class cMatroxController : cControllerBase, IController
    {
        public MIL_ID mMilSystem = MIL.M_NULL; // System identifier.
        private Thread mMotionThread;
        public cMatroxController()
        {
            Grabber = new cGrabber(this);
        }

        /// <summary>
        /// 모든 카메라의 Connection상태 리턴
        /// </summary>
        /// <returns></returns>
        public bool GetStatusConnectionAll()
        {
            if (Grabber.Cast<ICamera>().Any(pCamera => !pCamera.CameraStatus.Connection)) return false;
            return true;
        }

        /// <summary>
        /// 모든 카메라의 Enable상태 리턴
        /// </summary>
        /// <returns></returns>
        public bool GetStatusEnableAll()
        {
            if (Grabber.Cast<ICamera>().Any(pCamera => !pCamera.CameraStatus.Enable)) return false;
            return true;
        }

        /// <summary>
        /// 각 축별 상태 에러 메시지 반환
        /// </summary>
        /// <returns></returns>
        public string[] GetStatusErrMsg()
        {
            return new string[0];
        }

        /// <summary>
        /// 제어기 초기화 및 연결
        /// </summary>
        /// <param name="pInitializeData">각 제어기에 맞는 초기화 데이터</param>
        /// <returns>초기화 성공 여부</returns>
        public bool Initialize(cControllerData pData)
        {
            //컨트롤러 연결
            Name = pData.ControllerName;
            ID   = pData.ControllerID;
            //시스템 할당
            //pData.InitData는 보드 이름
            //pData.ControllerID 보드 순서
            mMilSystem = MIL.MsysAlloc(pData.InitData, ID, MIL.M_DEFAULT, MIL.M_NULL);

            for (int i = 0; i < pData.CameraDatas.Length; i++)
            {
                ICamera camera;

                if (pData.CameraDatas[i].CameraType == eCameraType.LINK)
                    camera = new cMatroxLinkCamera(this, pData.CameraDatas[i]);
                //else if(pData.CameraDatas[i].CameraType == eCameraType.COAX)
                else
                    camera = new cTestCamera(this, pData.CameraDatas[i]);

                Grabber.AddCamera(camera);
                camera.GrabEvent += Controller_GrabEvent;
            }

            mMotionThread = new Thread(MotionThread)
            {
                IsBackground = true
            };

            mMotionThread.Start();

            return true;
        }

        /// <summary>
        /// 객체 제거
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
            foreach (ICamera camera in Grabber) camera.Dispose();

            if(mMilSystem != MIL.M_NULL) MIL.MsysFree(mMilSystem);
        }

        public void MotionThread()
        {
            while (IsDisposed == false)
            {
                try
                {
                    foreach (ICamera camera in Grabber) camera.Update();

                    Thread.Sleep(1);
                }
                catch { }
            }
        }
    }
}
