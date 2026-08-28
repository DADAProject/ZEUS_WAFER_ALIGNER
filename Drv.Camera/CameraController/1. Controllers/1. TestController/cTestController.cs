using System.Linq;
using System.Threading;

namespace Drv.CameraController
{
    public class cTestController : cControllerBase, IController
    {
        private Thread mMotionThread;
        public cTestController()
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

            for (int i = 0; i < pData.CameraDatas.Length; i++)
            {
                ICamera camera = new cTestCamera(this, pData.CameraDatas[i]);
                Grabber.AddCamera(camera);
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
            //foreach (cTestAxis axis in Axes) axis.();
        }

        public void MotionThread()
        {
            while (IsDisposed == false)
            {
                try
                {
                    foreach (cTestCamera camera in Grabber) camera.Update();

                    Thread.Sleep(1);
                }
                catch { }
            }
        }
    }
}
