using Sentech.GenApiDotNET;
using Sentech.StApiDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drv.CameraController
{
    public class cOmronCamera : cBaseCamera, ICamera, IDisposable
    {
        #region << Fields >>
        public sCameraStatus mCameraStatus            = new sCameraStatus();
        public cCameraDatas mCameraDatas              = null;

        private static CStApiAutoInit SentechApi      = null;
        private static CStSystem SentechSystem        = null;

        private CStDevice SentechCam                  = null;
        private EventWaitHandle EventOneGrabSentech   = null;
        private CStDataStream SentechStream           = null;
        //private CStDataStreamArray SentechStreamArray = null;

        bool disposed = false;
        #endregion

        #region << Constructor & Deconstructor>>

        public cOmronCamera(IController pMaster, cCameraDatas pData)
        {

            Master = pMaster;
            ID = pData.ID;
            mCameraDatas = pData;
            CameraName = pData.CameraName;

            try
            {
                OnGrabEvent = new Dictionary<Type, CameraGrabHandler>();
                EventOneGrabSentech = new EventWaitHandle(false, EventResetMode.AutoReset);
                if (cOmronCamera.SentechApi == null) cOmronCamera.SentechApi = new CStApiAutoInit();
                if (cOmronCamera.SentechSystem == null) cOmronCamera.SentechSystem = new CStSystem(eStSystemVendor.Default, eStInterfaceType.GigEVision);

                for (uint i = 0; i < cOmronCamera.SentechSystem.InterfaceCount; i++)
                {
                    IStInterface interfaceInfo = cOmronCamera.SentechSystem.GetIStInterface(i);
                    interfaceInfo.UpdateDeviceList();
                    uint CamCnt = interfaceInfo.DeviceCount;

                    for (uint j = 0; j < CamCnt; j++)
                    {
                        IStDeviceInfo caminfo = interfaceInfo.GetIStDeviceInfo(j);

                        if (caminfo.UserDefinedName == pData.CameraName)
                        {
                            eDeviceAccessFlags deviceFlags = eDeviceAccessFlags.EXCLUSIVE;
                            if (caminfo.AccessStatus == eDeviceAccessStatus.READONLY)
                                deviceFlags = eDeviceAccessFlags.READONLY;
                            SentechCam = interfaceInfo.CreateStDevice(caminfo.ID, deviceFlags);
                            SetSentech(SentechCam, pData);
                            mCameraStatus.Connection = true;
                            SetEnable(true);
                            break;
                        }
                    }

                    if (SentechCam != null) break;
                }
            }
            catch
            {
                mCameraStatus.Connection = false;
            }
            finally
            {
                //SentechSystem.Dispose();
                //SentechApi.Dispose();
            }

            GrabSimEvent += CameraGrabSimCallback;
        }
        //2026 07
        ~cOmronCamera()
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

        /// <summary>
        /// Camera Sim ON/OFF
        /// </summary>
        /// <param name="pOn">True = On, False = Off</param>
        /// <returns>True = Succeed, False = Failed</returns>
        public bool SetSimEnable(bool pOn, string sPath)
        {
            mCameraStatus.SimEnable = pOn;
            mCameraStatus.SimPath = sPath;

            return true;
        }

        public bool SetParameter(string pName, string pValue)
        {
            INodeMap nodeMapRemote = SentechCam.GetRemoteIStPort().GetINodeMap();

            if (new string[] { "ExposureTime" }.Any(x => nodeMapRemote.DeviceName.Contains(x)))
                SetFloat(nodeMapRemote, pName, Convert.ToSingle(pValue));

            if (new string[] { "Gain" }.Any(x => nodeMapRemote.DeviceName.Contains(x)))
                SetFloat(nodeMapRemote, pName, Convert.ToSingle(pValue));

            return true;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (SentechStream != null && SentechStream.Handle != IntPtr.Zero)
                {
                    if (SentechStream.IsGrabbing)
                        SentechStream.StopAcquisition(eAcqStopFlags.KILL);
                }
                //SentechCam?.AcquisitionStop();

                SentechStream?.Dispose();
                SentechCam?.Dispose();
                disposed = true;
            }
        }

        public override bool Grab()
        {
            try
            {
                mCameraStatus.GrabResult = GrabResult.Ready;

                if (SentechCam == null)
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    return false;
                }

                EventOneGrabSentech.Reset();
                ExecuteSoftwareTrigger(SentechCam);
                if (EventOneGrabSentech.WaitOne(2000))
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
            catch
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



        protected void SetSentech(CStDevice device, cCameraDatas data)
        {
            SentechStream = device.CreateStDataStream(0);
            SentechStream.StreamBufferCount = Convert.ToUInt32(data.Param.BufferCount);

            try
            {
                SentechStream.RegisterCallbackMethod(CallbackSentech);

                INodeMap nodeMapRemote = SentechCam.GetRemoteIStPort().GetINodeMap();
                //IInteger IHeight = nodeMapRemote.GetNode<IInteger>("Height");
                //IInteger IWidth  = nodeMapRemote.GetNode<IInteger>("Width");
                //IEnum    IFormat = nodeMapRemote.GetNode<IEnum>("PixelFormat");

                //if (!string.IsNullOrEmpty(data.Param.CamFilePath))
                //
                if (!data.Param.UseOnlyCameFile)
                {
                    SetEnumeration(nodeMapRemote, "TriggerSelector", Convert.ToString(data.Param.TriggerSelector));
                    SetEnumeration(nodeMapRemote, "TriggerMode", Convert.ToString(data.Param.TriggerMode));
                    SetEnumeration(nodeMapRemote, "TriggerSource", Convert.ToString(data.Param.TriggerSource));
                    SetEnumeration(nodeMapRemote, "ExposureMode", Convert.ToString(data.Param.ExposureMode));
                    SetInteger(nodeMapRemote, "AcquisitionFrameCount", Convert.ToInt32(data.Param.BufferCount));
                    SetFloat(nodeMapRemote, "ExposureTime", Convert.ToSingle(data.Param.ExposureTime));
                    //
                    ////HeartBeat
                    //모델에 따라 파라미터명 다름
                    IString IModel = nodeMapRemote.GetNode<IString>("DeviceModelName");

                    // if (new string[] { "STC-MBS", "STC-MCS", "STC-MCS", "FS-MBS", "FS-MC" }.Any(x => IModel.Value.Contains(x)))//STC-MBS/MCS,FS-MBS/MC series
                    //     SetFloat(nodeMapRemote, "DeviceLinkHeartbeatTimeout", Convert.ToSingle(data.Param.Heartbeat));
                    // else  //STC-SB,SC,CMB,.CMC
                    //     SetFloat(nodeMapRemote, "GevHeartbeatTimeout", Convert.ToSingle(data.Param.Heartbeat));

                    ////SetInteger(nodeMapRemote, "GevHeartbeatTimeout", Convert.ToInt32(data.Param.Heartbeat));
                    //설정하면 트리거 안됨 //OS에서 환경변수 설정해야함
                }

                if (mCameraDatas.Param.TriggerMode == "On")
                {
                    SentechStream.StartAcquisition();
                    SentechCam.AcquisitionStart();
                }
            }
            catch (Exception ex)
            {
                // cDEF.LOG.ExceptionTrace("CustomCam.SetBasler" + ex.ToString());
                Debug.WriteLine($"[Exception] SetSentech = {ex.Message}");
            }
        }

        protected void SetEnumeration(INodeMap nodeMap, string enumerationName, string valueName)
        {
            // Get the IEnum interface.
            IEnum enumNode = nodeMap.GetNode<IEnum>(enumerationName);
            // Update the settings using the IEnum interface.
            if (enumNode.StringValue.Contains(valueName) == false)
                enumNode.StringValue = valueName;
        }

        protected void SetInteger(INodeMap nodeMap, string enumerationName, int value)
        {
            // Get the IEnum interface.
            IInteger IntNode = nodeMap.GetNode<IInteger>(enumerationName);
            // Update the settings using the IEnum interface.

            IntNode.Value = value;
        }
        protected void SetFloat(INodeMap nodeMap, string enumerationName, float value)
        {
            // Get the IEnum interface.
            IFloat IntNode = nodeMap.GetNode<IFloat>(enumerationName);
            // Update the settings using the IEnum interface.

            IntNode.Value = value;
        }


        protected void ExecuteSoftwareTrigger(CStDevice device)
        {
            mCameraStatus.GrabResult = GrabResult.Process;

            if (!mCameraStatus.SimEnable)
            {
                if (mCameraDatas.Param.TriggerMode == "On")
                {
                    INodeMap nodeMapRemote = device.GetRemoteIStPort().GetINodeMap();
                    ICommand commandNode = nodeMapRemote.GetNode<ICommand>("TriggerSoftware");

                    commandNode.Execute();
                }
                else
                {
                    SentechStream.StartAcquisition(1);
                }
            }
            else
            {
                CameraGrabSimEvent();
            }
        }

        #region << Events >>

        private unsafe void CallbackSentech(IStCallbackParamBase paramBase, object[] param)
        {
            if (paramBase.CallbackType == eStCallbackType.TL_DataStreamNewBuffer)
            {
                // In case of receiving a NewBuffer events:
                // Convert received callback parameter into IStCallbackParamGenTLEventNewBuffer for acquiring additional information.
                IStCallbackParamGenTLEventNewBuffer callbackParam = paramBase as IStCallbackParamGenTLEventNewBuffer;

                try
                {
                    // Get the IStDataStream interface object from the received callback parameter.
                    IStDataStream dataStream = callbackParam.GetIStDataStream();

                    // Retrieve the buffer of image data for that callback indicated there is a buffer received.
                    using (CStStreamBuffer streamBuffer = dataStream.RetrieveBuffer(0))
                    {
                        // Check if the acquired data contains image data.
                        if (streamBuffer.GetIStStreamBufferInfo().IsImagePresent)
                        {
                            //칼라랑 모노랑 구분해야함
                            // If yes, we create a IStImage object for further image handling.
                            IStImage Buffer = streamBuffer.GetIStImage();
                            Int32 bufferSize = (Int32)streamBuffer.GetIStStreamBufferInfo().BufferSize;

                            if (GrabBuffer == IntPtr.Zero)
                                GrabBuffer = Marshal.AllocHGlobal((int)bufferSize);

                            System.Buffer.MemoryCopy(streamBuffer.GetIStStreamBufferInfo().BaseAddress.ToPointer(), GrabBuffer.ToPointer(), bufferSize, bufferSize);

                            GrabEventArg arg = new GrabEventArg()
                            {
                                Width = Convert.ToInt32(Buffer.ImageWidth),
                                Height = Convert.ToInt32(Buffer.ImageHeight),
                                PixelFormat = Buffer.ImagePixelFormat.ToString(),
                                Image = (byte[])Buffer.GetByteArray().Clone(),
                                ImagePtr = GrabBuffer
                            };

                            TransmissionGrabEvent(this, arg);
                            CameraGrabEvent(this, arg);
                            EventOneGrabSentech.Set();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //  cDEF.LOG.ExceptionTrace("CustomCam.CallbackSentech" + ex.ToString());
                    Debug.WriteLine($"[Exception] CallbackSentech = {ex.Message}");
                }
            }
            else
            {
                mCameraStatus.GrabResult = GrabResult.Error;
            }
        }

        private void CameraGrabSimCallback()
        {
            if (!string.IsNullOrEmpty(mCameraStatus.SimPath))
            {
                //랜덤하게 이미지 로드
                string[] files = { "", };
                try
                {
                    files = System.IO.Directory.GetFiles(mCameraStatus.SimPath, "*.*", System.IO.SearchOption.AllDirectories);

                    GrabEventArg arg = new GrabEventArg()
                    {
                        //Width = Convert.ToInt32(Buffer.ImageWidth),
                        //Height = Convert.ToInt32(Buffer.ImageHeight),
                        //PixelFormat = Buffer.ImagePixelFormat.ToString(),
                        //Image = (byte[])Buffer.GetByteArray().Clone()
                    };

                    TransmissionGrabEvent(this, arg);
                    CameraGrabEvent(this, arg);

                    EventOneGrabSentech.Set();
                }
                catch
                {

                }
            }
        }
        #endregion

    }
}
