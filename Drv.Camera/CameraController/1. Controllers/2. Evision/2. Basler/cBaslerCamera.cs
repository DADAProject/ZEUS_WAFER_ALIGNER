using PylonC.NET;
using Basler.Pylon;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Drv.CameraController
{
    public class cBaslerCamera : cBaseCamera, ICamera , IDisposable
    {
        #region << Fields >>
        public sCameraStatus mCameraStatus          = new sCameraStatus();
        public cCameraDatas mCameraDatas            = null;

        private Camera PylonCam;
        private EventWaitHandle EventOneGrabBasler = null;
        #endregion

        #region << Constructor & Deconstructor>>

        public cBaslerCamera(IController pMaster, cCameraDatas pData)
        {
            
            Master          = pMaster;
            ID              = pData.ID;
            mCameraDatas    = pData;

            try
            {
                OnGrabEvent = new Dictionary<Type, CameraGrabHandler>();
                EventOneGrabBasler = new EventWaitHandle(false, EventResetMode.AutoReset);

                List<ICameraInfo> allCameras = CameraFinder.Enumerate();
                foreach (ICameraInfo cameraInfo in allCameras)
                {
                    //유저 네임을 쓴껀지 카메라 메인 이름을 쓸껀지 정해야함
                    if (cameraInfo["UserDefinedName"] == pData.CameraName)
                    {
                        SetBasler(cameraInfo, pData);
                        mCameraStatus.Connection = true;
                        SetEnable(true);
                        break;
                    }
                }
            }
            catch
            {
            }
        }
        ~cBaslerCamera()
        {
            Dispose();
        }

        #endregion

        #region << Properties >>
        /// <summary>
        /// Camera Status (Connection, Enable, GrabResult)
        /// </summary>
        public sCameraStatus CameraStatus
        {
            get { return mCameraStatus; }
            protected set
            {
                mCameraStatus = value;
            }
        }
        #endregion

        public eDefaultCameraAlarm GetDefaultAlarm()
        {
            return eDefaultCameraAlarm.None;
        }



        /// <summary>
        /// Read Camera Alarm Text
        /// </summary>
        /// <returns>Alarm Text Array</returns>
        public string[] GetCameraAlarmString()
        {
             //List<string> alarm = new List<string>();

            return new string[0];
        }

        /// <summary>
        /// Camera ON/OFF
        /// </summary>
        /// <param name="pOn">True = On, False = Off</param>
        /// <returns>True = Succeed, False = Failed</returns>
        public bool SetEnable(bool pOn, int TimeOut = 0)
        {
            mCameraStatus.Enable = pOn;
            return true;
        }
        public bool SetSimEnable(bool pOn, string sPath)
        {
            mCameraStatus.SimEnable = pOn;
            mCameraStatus.SimPath = sPath;

            return true;
        }

        public bool SetParameter(string pName, string pValue)
        {
            return true;
        }
        public void Dispose()
        {
            if (PylonCam != null)
            {
                PylonCam.StreamGrabber.Stop();
                PylonCam.Close();
                PylonCam.Dispose();
                PylonCam = null;
            }
        }

        public override bool Grab()
        {
            try
            {
                mCameraStatus.GrabResult = GrabResult.Ready;

                if (PylonCam == null)
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    return false;
                }

                if (PylonCam.WaitForFrameTriggerReady(100, TimeoutHandling.Return))
                {
                    EventOneGrabBasler.Reset();
                    ExecuteSoftwareTrigger(PylonCam);
                    if (EventOneGrabBasler.WaitOne(1000))
                    {
                        mCameraStatus.GrabResult = GrabResult.Good;
                        return true;
                    }
                    else
                    {
                        mCameraStatus.GrabResult = GrabResult.Error;
                        return false;
                    }
                }
                else
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    return false;
                }

            }
            catch //exception 사용하지 않음
            {
                mCameraStatus.GrabResult = GrabResult.Error;
                return false;
            }
        }

  
        /// <summary>
        /// Camera Alarm Clear
        /// </summary>
        /// <returns>True = Succeed, False = Failed</returns>
        public bool AlarmClear()
        {
            return true;
        }


        public void Update()
        {
         
        }



        protected void SetBasler(ICameraInfo device, cCameraDatas data)
        {
            PylonCam = null;
            PylonCam = new Camera(device);
        
            try
            {
                PylonCam.Open();
                if (PylonCam.IsOpen)
                {
                    //해야하나?
                    //PylonCam.Parameters[PLCamera.Width].SetValue(Width);
                    //PylonCam.Parameters[PLCamera.Height].SetValue(Height);
                    //PylonCam.Parameters[PLCamera.CenterX].SetValue(true);
                    //PylonCam.Parameters[PLCamera.CenterY].SetValue(true);


                    PylonCam.Parameters[PLCamera.TriggerSelector].SetValue(Convert.ToString(data.Param.TriggerSelector));
                    PylonCam.Parameters[PLCamera.TriggerMode].SetValue(Convert.ToString(data.Param.TriggerMode));
                    PylonCam.Parameters[PLCamera.TriggerSource].SetValue(Convert.ToString(data.Param.TriggerSource));
                    PylonCam.Parameters[PLCamera.ExposureMode].SetValue(Convert.ToString(Convert.ToString(data.Param.ExposureMode)));
                    PylonCam.Parameters[PLCamera.AcquisitionFrameCount].SetValue(Convert.ToInt32(data.Param.BufferCount));
                    PylonCam.Parameters[PLCamera.ExposureTimeAbs].SetValue(Convert.ToInt32(data.Param.ExposureTime));

                    //ExposeTime 추가 해야함 나중에 버츄럴로 등록 => 개별 함수을 통해 설정되도록 한다.
                    //다른것들도 그렇게 진행 될수도 있음.
                    // PylonCam.Parameters[PLCamera.PixelFormat].SetValue(PLCamera.PixelFormat.Mono8); //카메라에서 자동 셋팅
       

                    if (mCameraDatas.Param.TriggerMode == "On") PylonCam.CameraOpened += Configuration.SoftwareTrigger;
                    else                                        PylonCam.CameraOpened += Configuration.AcquireSingleFrame;

                    PylonCam.StreamGrabber.ImageGrabbed += CallbackBasler;
                    PylonCam.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                }
            }
            catch 
            {
            }
        }


        protected void ExecuteSoftwareTrigger(Camera device)
        {
            mCameraStatus.GrabResult = GrabResult.Process;

            if (mCameraDatas.Param.TriggerMode == "On")
            {
                device.ExecuteSoftwareTrigger();
            }
            else
            {
                device.StreamGrabber.GrabOne(1000);
                //그랩 
            }
        }
        #region << Events >>

        private void CallbackBasler(Object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                IGrabResult grabResult = e.GrabResult;

                if (grabResult.IsValid)
                {
                    byte[] Buffer = grabResult.PixelData as byte[];

                    GrabEventArg arg = new GrabEventArg()
                    {
                        Width = Convert.ToInt32(grabResult.Width),
                        Height = Convert.ToInt32(grabResult.Height),
                        PixelFormat = grabResult.PixelTypeValue.ToString(),
                        Image = (byte[])Buffer.Clone()
                    };

                    CameraGrabEvent(this, arg);

                    EventOneGrabBasler.Set();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Exception] CallbackBasler = {ex.Message}");

                e.DisposeGrabResultIfClone();
                mCameraStatus.GrabResult = GrabResult.Error;
            }
        }


        #endregion

    }
}
