using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.CameraController
{
    public class cMatroxLinkCamera : cBaseCamera, ICamera, IDisposable
    {
        #region << Fields >>
        private readonly object _GrapLock = new object();
        private event EventHandler<GrabEventArg> _GrabEvent;

        public sCameraStatus mCameraStatus = new sCameraStatus();
        public cCameraDatas mCameraDatas = null;

        private MIL_ID mCamInstance = MIL.M_NULL; // Digitizer identifier.
        private MIL_ID mBufferId    = MIL.M_NULL;
        private EventWaitHandle EventOneGrabMatrox = null;
        
        bool disposed = false;
        #endregion

        #region << Constructor & Deconstructor>>

        public cMatroxLinkCamera(IController pMaster, cCameraDatas pData)
        {
            Master       = pMaster;
            ID           = pData.ID; 
            mCameraDatas = pData;

            cMatroxController Matrox = Master as cMatroxController;
            try
            {
                EventOneGrabMatrox = new EventWaitHandle(false, EventResetMode.AutoReset);
                //Matrox.ID는 보드 순서
                //mCameraDatas.CameraName 는 카메라 DCF파일
                MIL.MdigAlloc(Matrox.mMilSystem, Matrox.ID, mCameraDatas.CameraName, MIL.M_DEFAULT, ref mCamInstance);

                SetMatroxCam(pData);
                mCameraStatus.Connection = true;
                SetEnable(true);
            }
            catch
            {
                mCamInstance = uint.MaxValue;
                mCameraStatus.Connection = false;
            }

        }

        ~cMatroxLinkCamera()
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
            if (!disposed)
            {
                //훅킹을 종료해야하는지 확인해야함
                //MdigHookFunction(DigId, M_GRAB_START + M_UNHOOK, HookHandlerPtr,UserDataPtr);

                if (mCamInstance != null) MIL.MdigFree(mCamInstance);
                disposed = true;
            }
        }
        private void SetMatroxCam(cCameraDatas data)
        {
            try
            {
                //사용 유무 체크 만들어야함
                //MC.SetParam((uint)(MC.BOARD + BoardId), "BoardTopology", sCamType);
                //MC.SetParam(mCamInstance, "DriverIndex", BoardId);
                //MC.SetParam(mCamInstance, "Connector", sConnector);
                //MC.SetParam(mCamInstance, "CamFile", sCamFile);

                //MC.SetParam(Instance, "TrigMode", "IMMEDIATE");
                //MC.SetParam(mCamInstance, "TrigMode", data.Param.TriggerMode); //"SOFT"
                ////MC.SetParam(Instance, "ColorFormat"    , "Y8"       );
                //MC.SetParam(mCamInstance, "AcquisitionMode", "SNAPSHOT");
                //MC.SetParam(mCamInstance, "ChannelState", "ACTIVE");
                //
                //// Register the callback function
                //mMultiCamCallback = new MC.CALLBACK(CallbackEuresys);
                //MC.RegisterCallback(mCamInstance, mMultiCamCallback, mCamInstance);
                //
                //// Enable the signals corresponding to the callback functions
                //MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
                //MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");
                //MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_END_CHANNEL_ACTIVITY, "ON");

                MIL.MdigHookFunction(mCamInstance, MIL.M_GRAB_FRAME_END, FrameEndHookHandler, mBufferId);
            }
            catch 
            {
            }
        }
        public override bool Grab()
        {
            try
            {
                mCameraStatus.GrabResult = GrabResult.Ready;

                if (mCamInstance == uint.MaxValue)
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    return false;
                }

                EventOneGrabMatrox.Reset();
                ExecuteSoftwareTrigger();
                if (EventOneGrabMatrox.WaitOne(2000))
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
                Debug.WriteLine($"[Exception] Grab = {ex.Message}");
                mCameraStatus.GrabResult = GrabResult.Error;
                return false;
            }
        }
        protected void ExecuteSoftwareTrigger()
        {
            mCameraStatus.GrabResult = GrabResult.Process;

            if (mCameraDatas.Param.TriggerMode == "On")
            {
            }
            else
            {
                //MIL.MdigGrab(mCamInstance, ); ;

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

        private new void TransmissionGrabEvent(ICamera pSender, GrabEventArg e)
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

        MIL_INT FrameEndHookHandler(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            MIL_ID ModifiedBufferId = MIL.M_NULL;

            if (!IntPtr.Zero.Equals(HookDataPtr))
            {
                GCHandle hUserData = GCHandle.FromIntPtr(HookDataPtr);
                //MbufGet(Dlg->m_MilImgBuffer, Dlg->m_UserImgBuffer);
                //
                //HookDataStruct UserData = hUserData.Target as HookDataStruct;
                //
                //MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref ModifiedBufferId);
            }
            return 0;
        }
        #endregion

    }
}
