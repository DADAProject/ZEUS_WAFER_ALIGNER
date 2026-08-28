using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drv.CameraController
{
    /// <summary>
    /// 최상위 컨트롤러 클레스 다음과 같은 구로를 같는다
    /// MasterControl -> Controller -> Grabber -> Camera1
    ///                             -> Grabber -> Camera2
    ///                             -> ...
    ///               -> Controller -> Grabber -> Camera1
    ///                             -> Grabber -> Camera2
    ///                             -> ...          
    ///               -> ...
    /// </summary>
    public class cVision : System.Collections.IEnumerable
    {
        #region < Singleton >
        private static readonly cVision _instance = new cVision();
        public static cVision Instance { get { return _instance; } }
        #endregion

        #region # PUBLIC EVENTS #

        public event CameraGrabHandler GrabEvent;
        //
        ///// <summary>
        ///// 비상정지 상태 변경 발생시 발생
        ///// </summary>
        //public event EmergencyEventHandler EventChangedEmergency;
        //
        ///// <summary>
        ///// 알람 이벤트
        ///// </summary>
        //public event MessageEventHandler EventAlarm;

        #endregion # PUBLIC EVENTS #

        #region # PRIVATE MEMBERS #
        /// <summary>
        /// 카메라 모니터링 쓰레드
        /// </summary>
        private System.Threading.Thread mMonitoringThread;

        /// <summary>
        /// 컨트롤러 리스트
        /// </summary>
        private readonly List<IController> mlstController = new List<IController>();

        #endregion # PRIVATE MEMBERS #

        #region # PUBLIC PROPERTIES #

        public bool UseAutoGCCollector { get; set; } = false;

        public bool WasInitiated
        {
            get
            {
                foreach (ICamera camera in this)
                {
                    if (camera.WasInitiated == false) return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 모든 카메라 Enable 상태
        /// </summary>
        public bool EnableAll
        {
            get
            {
                if (mlstController.Count == 0) return false;
                return mlstController[0].GetStatusEnableAll();
            }
        }
        /// <summary>
        /// 모든 카메라 Connection 상태
        /// </summary>
        public bool ConnectionAll
        {
            get
            {
                if (mlstController.Count == 0) return false;
                return mlstController[0].GetStatusConnectionAll();
            }
        }
        /// <summary>
        /// 컨트롤러에 연결된 카메라
        /// </summary>
        /// <param name="pCameraName">카메라의 이름</param>
        /// <returns>카메라 반환</returns>
        public ICamera this[string pCameraName]
        {
            get
            {
                ICamera camera = mlstController.Select(pCtr => pCtr.Grabber[pCameraName]).FirstOrDefault(pCamera => pCamera != null);
                return camera;
            }
        }

        public ICamera this[Enum pCameraName]
        {
            get
            {
                return this[pCameraName.ToString()];
            }
        }


        public bool UseAlarmMonitoring { get; set; }
        public bool IsDisposed { get; private set; }
        #endregion # PUBLIC PROPERTIES #

        #region # CONSTRUCTOR & DESTRUCTOR #

        public cVision()
        {

        }
        #endregion

        #region # PUBLIC METHODS #

        public bool Initialize(params cControllerData[] pDatas)
        {
            try
            {
                bool initComplete = false;
                for (int i = 0; i < pDatas.Length; i++)
                {
                    IController ctrTemp;

                    if (pDatas[i].ControllerType == eControllerType.EVISION)
                    {
                        ctrTemp = new cEvisionController();
                    }
                    else if(pDatas[i].ControllerType == eControllerType.EURESYS)
                    {
                        ctrTemp = new cEuresysController();
                    }
                    else
                    {
                        ctrTemp = new cTestController();
                    }

                    initComplete = ctrTemp.Initialize(pDatas[i]);
                    this.mlstController.Add(ctrTemp);
                    
                    ctrTemp.GrabEvent += ctrTemp_GrabEvent;
                }

                mMonitoringThread = new System.Threading.Thread(Monitoring)
                {
                    IsBackground = true,
                    Name = "Camera Alarm Monitoring"
                };
                mMonitoringThread.Start();

                return initComplete;
            }
            catch { throw; }

        }

        private void ctrTemp_GrabEvent(ICamera pSender, GrabEventArg e)
        {
            //GrabEvent?.Invoke(pSender, e);
            //GrabEvent?.BeginInvoke(pSender, e, null, null);
            Task.Run(() => GrabEvent?.Invoke(pSender, e));
        }

        /// <summary>
        /// Foreach 문을 사용하기 위한 함수
        /// </summary>
        /// <returns>축 1개씩 반환</returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            try
            {
                return mlstController.SelectMany(pCtr => pCtr.Grabber.Cast<ICamera>()).GetEnumerator();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 지정된 카메라의 키값이 있는지 검사 하는 함수
        /// </summary>
        /// <param name="pKey">검사할 키 값</param>
        /// <returns>true = 이미 존재함, false = 존재하지 않음</returns>
        public bool ContainsCameraKey(string pKey)
        {
            try
            {
                return mlstController.Select(pCtr => pCtr.Grabber[pKey]).Any(pCamera => pCamera != null);
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 컨트롤러 제어 인터페이스를 가져온다
        /// </summary>
        /// <param name="pName">컨트롤러 이름</param>
        /// <returns>제어할 컨트롤러</returns>
        public IController GetController(string pName)
        {
            try
            {
                return mlstController.FirstOrDefault(pCtr => pCtr.Name == pName);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 컨트롤러 제어 인터페이스를 가져온다
        /// </summary>
        /// <param name="pAxisName">축 이름</param>
        /// <returns>제어할 컨트롤러</returns>
        public IController GetControllerByCamera(string pCameraName)
        {
            try
            {
                return mlstController.FirstOrDefault(pCtr => pCtr.Grabber[pCameraName] != null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 컨트롤러 제어 인터페이스를 가져온다
        /// </summary>
        /// <returns>제어할 컨트롤러</returns>
        public IController[] GetControllerAll()
        {
            try
            {
                return mlstController.ToArray();
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 컨트롤러 제거 함수
        /// </summary>
        public void Dispose()
        {
            foreach (IController ctr in mlstController) ctr.Dispose();
            IsDisposed = true;
        }

        #endregion # PUBLIC METHODS #

        private void Monitoring()
        {
            UseAlarmMonitoring = true;

            while (IsDisposed == false)
            {
                System.Threading.Thread.Sleep(500);


            }
        }

    }
}