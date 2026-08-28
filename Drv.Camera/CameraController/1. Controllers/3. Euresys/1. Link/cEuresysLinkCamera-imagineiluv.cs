using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Euresys.MultiCam;
using Euresys;
namespace CameraController
{
    public class cEuresysLinkCamera : cBaseCamera, ICamera, IDisposable
    {
        #region << Fields >>
        private readonly object _GrapLock = new object();
        private event EventHandler<GrabEventArg> _GrabEvent;

        public sCameraStatus mCameraStatus = new sCameraStatus();
        public cCameraDatas mCameraDatas = null;

        private uint mCamInstance;
        private EventWaitHandle EventOneGrabMultiCam = null;
        private MC.CALLBACK mMultiCamCallback = null;
        bool disposed = false;
        #endregion

        #region << Constructor & Deconstructor>>

        public cEuresysLinkCamera(IController pMaster, cCameraDatas pData)
        {
            Master = pMaster;
            ID = pData.ID;
            mCameraDatas = pData;

            try
            {
                EventOneGrabMultiCam = new EventWaitHandle(false, EventResetMode.AutoReset);
                MC.OpenDriver();
                MC.GetParam(MC.CONFIGURATION, MC.BOARD ,out int a);
                //Getting number of boards
                //Status = McGetParamInt(MC_CONFIGURATION, MC_BoardCount, &BoardCount);
                ////Scanning across MultiCam boards
                //for (i = 0; i < BoardCount; i++)
                //{
                //    //Fetching the board name (String MultiCam parameter)
                //    Status = McGetParamStr(
                //      MC_BOARD + i,
                //      MC_BoardName,
                //      BoardInfo[i].BoardName,
                //      17);
                //    //Fetching the board serial number (Integer MultiCam parameter)
                //    Status = McGetParamInt(
                //      MC_BOARD + i,
                //      MC_SerialNumber,
                //      &BoardInfo[i].SerialNumber);
                //    //Fetching the board type (Enumerated MultiCam parameter)
                //    Status = McGetParamInt(
                //      MC_BOARD + i,
                //      MC_BoardType,
                //      &BoardInfo[i].BoardType);
                //}


                MC.Create(pData.CameraName, out mCamInstance);
                SetMultiCam(pData);
                mCameraStatus.Connection = true;
                SetEnable(true);
            }
            catch
            {
                mCamInstance = uint.MaxValue;
                mCameraStatus.Connection = false;
            }

        }

        ~cEuresysLinkCamera()
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
        public void Dispose()
        {
            if (!disposed)
            {
                MC.Delete(mCamInstance);
                MC.CloseDriver();

                disposed = true;
            }
        }
        private void SetMultiCam(cCameraDatas data)
        {
            try
            {
                //사용 유무 체크 만들어야함
                //MC.SetParam((uint)(MC.BOARD + BoardId), "BoardTopology", sCamType);
                //MC.SetParam(mCamInstance, "DriverIndex", BoardId);
                //MC.SetParam(mCamInstance, "Connector", sConnector);
                //MC.SetParam(mCamInstance, "CamFile", sCamFile);

                //MC.SetParam(Instance, "TrigMode", "IMMEDIATE");
                MC.SetParam(mCamInstance, "TrigMode", data.Param.TriggerMode); //"SOFT"
                //MC.SetParam(Instance, "ColorFormat"    , "Y8"       );
                MC.SetParam(mCamInstance, "AcquisitionMode", "SNAPSHOT");
                MC.SetParam(mCamInstance, "ChannelState", "ACTIVE");

                // Register the callback function
                mMultiCamCallback = new MC.CALLBACK(CallbackEuresys);
                MC.RegisterCallback(mCamInstance, mMultiCamCallback, mCamInstance);

                // Enable the signals corresponding to the callback functions
                MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
                MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");
                MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_END_CHANNEL_ACTIVITY, "ON");

            }
            catch 
            {
            }

            //MC.SetParam()
        }

        public bool Grab()
        {
            try
            {
                mCameraStatus.GrabResult = GrabResult.Ready;

                if (mCamInstance == uint.MaxValue)
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    return false;
                }

                EventOneGrabMultiCam.Reset();
                ExecuteSoftwareTrigger();
                if (EventOneGrabMultiCam.WaitOne(2000))
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
            catch (Exception ex) //exception 사용하지 않음
            {
                mCameraStatus.GrabResult = GrabResult.Error;
                return false;
            }
        }
        protected void ExecuteSoftwareTrigger()
        {
            mCameraStatus.GrabResult = GrabResult.Process;

            if (mCameraDatas.Param.TriggerMode == "On")
            {
                MC.SetParam(mCamInstance, "ForceTrig", "TRIG");
            }
            else
            {
                //그랩 
            }
        }

        public GrabEventArg GrabAndGetReply(TimeSpan pTimeout)
        {
            lock (_GrapLock)
            {
                if (pTimeout == null)
                    pTimeout = TimeSpan.FromSeconds(10);

                GrabEventArg mReply = null;

                EventHandler<GrabEventArg> ev = (s, e) => { mReply = e; };
                this._GrabEvent += ev;
                if (Grab() == false) return mReply;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (mReply == null && sw.Elapsed < pTimeout)
                    ThreadExtension.Delay(10);

                //Thread.Sleep(10);
                this._GrabEvent -= ev;
                this._GrabEvent = null;
                return mReply;
            }
        }

        private void TransmissionGrabEvent(ICamera pSender, GrabEventArg e)
        {
            GrabEventArg arg = e;

            if (this._GrabEvent != null)
            {
                this._GrabEvent(this, e);
                return;
            }
        }

        /// <summary>
        /// Camera Alarm Clear
        /// </summary>
        /// <returns>True = Succeed, False = Failed</returns>
        public bool AlarmClear()
        {
            mCameraStatus.GrabResult = GrabResult.Ready;
            return true;
        }


        public void Update()
        {
            //if(SentechCam != null)
            //{ 
            //    if(SentechCam.IsDeviceLost)
            //    { 
            //        mCameraStatus.Connection = false;
            //    }
            //}
        }







        #region << Events >>

        private void CallbackEuresys(ref MC.SIGNALINFO signalInfo)
        {
            UInt32 currentChannel = (UInt32)signalInfo.Context;

            try
            {
                switch (signalInfo.Signal)
                {
                    case MC.SIG_SURFACE_PROCESSING:
                        var _CurrentSurface = signalInfo.SignalInfo;
                       // MC.GetParam(_CurrentSurface, "SurfaceAddr", out _ImagePtr);

                        mCameraStatus.GrabResult = GrabResult.Good;
                        EventOneGrabMultiCam.Set();
                        //ProcessingCallbackEuresys(signalInfo);
                        break;
                    case MC.SIG_ACQUISITION_FAILURE:
                    case MC.SIG_END_CHANNEL_ACTIVITY:
                    default:
                        mCameraStatus.GrabResult = GrabResult.Error;
                        EventOneGrabMultiCam.Set();
                        break;
                }
            }
            catch
            {

            }
        }


        #endregion

    }
}
