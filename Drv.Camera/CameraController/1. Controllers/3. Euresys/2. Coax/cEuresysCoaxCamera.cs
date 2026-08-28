using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Euresys.gc;
using Euresys.ge;

using Euresys;
using System.Runtime.InteropServices;

namespace Drv.CameraController
{
    public class cEuresysCoaxCamera : cBaseCamera, ICamera, IDisposable
    {
        #region << Fields >>
        enum EModule { EInterface = 0, EDevice, ERemoteDevice, EDataStream };

        public sCameraStatus mCameraStatus = new sCameraStatus();
        public cCameraDatas mCameraDatas = null;

        private EventWaitHandle EventOneGrabCoaxCam = null;
        EGenTL m_pEGrabber;
        EGrabberCallbackMultiThread mCoaxCamCallback = null;
        bool disposed = false;
        #endregion

        #region << Constructor & Deconstructor>>

        public cEuresysCoaxCamera(IController pMaster, cCameraDatas pData)
        {
            Master = pMaster;
            ID = pData.ID;
            CameraName = pData.CameraName;
            mCameraDatas = pData;

            try
            {
                OnGrabEvent = new Dictionary<Type, CameraGrabHandler>();
                EventOneGrabCoaxCam = new EventWaitHandle(false, EventResetMode.AutoReset);
                m_pEGrabber = new EGenTL(); //체크 필요 (컨트롤러에서 해야할지 )
                mCoaxCamCallback = new EGrabberCallbackMultiThread(m_pEGrabber, Master.ID, ID, 0);
                mCoaxCamCallback.enableAllEvent();
                mCoaxCamCallback.resetBufferQueue();
                SetCoaxCam(pData);
                mCameraStatus.Connection = true;
                SetEnable(true);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"[Exception] cEuresysCoaxCamera = {ex.Message}");
                mCameraStatus.Connection = false;
            }

        }

        ~cEuresysCoaxCamera()
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
                mCoaxCamCallback?.shutdown();
                mCoaxCamCallback?.disableAllEvent();
                mCoaxCamCallback?.Dispose();
                m_pEGrabber?.Dispose();

                if (GrabBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(GrabBuffer);

                disposed = true;
            }
        }
        private void SetCoaxCam(cCameraDatas data)
        {
            mCoaxCamCallback.reallocBuffers((ulong)data.Param.BufferCount);

            if (!string.IsNullOrEmpty(data.Param.CamFilePath))
                mCoaxCamCallback.runScript(data.Param.CamFilePath);

            if (!data.Param.UseOnlyCameFile)
            {
                //SetValueString(EModule.EInterface, "LineSelector", "IOUT11");
                //SetValueString(EModule.EInterface, "LineInputToolSelector", "LIN2");
                //SetValueString(EModule.EInterface, "LineInputToolSource", "IOUT11");
                //SetValueString(EModule.EInterface, "LineInputToolActivation", "FallingEdge");

                //SetValueString(EDevice, "CameraControlMethod", _T("RC"));
                //SetValueString(EDevice, "ExposureReadoutOverlap", _T("True"));
                //SetValueString(EDevice, "CycleTriggerSource", _T("LIN1")); //트리거 모드 
                //SetValueInt(EDevice, "ExposureRecoveryTime", 100);
                //SetValueInt(EDevice, "CycleMinimumPeriod", (1000000 + 80 - 1) / 80);

                SetValueString(EModule.ERemoteDevice, "TriggerMode", data.Param.TriggerMode);
                SetValueString(EModule.ERemoteDevice, "TriggerSource", data.Param.TriggerSource);


                SetValueString(EModule.EDevice, "CameraControlMethod", "RG");//
                SetValueDouble(EModule.EDevice, "ExposureTime", data.Param.ExposureTime);

                SetValueString(EModule.EDevice, "CycleTriggerSource", "StartCycle");
            }

            mCoaxCamCallback.onNewBufferEvent = onNewBufferEvent;

            if (data.Param.TriggerMode == "On")
            {
                mCoaxCamCallback.start(); //
            }
        }

        private void SetValueInt(EModule module, string node, int value)
        {
            switch (module)
            {
                case EModule.EInterface:mCoaxCamCallback.setIntegerInterfaceModule(node, value);break;
                case EModule.EDevice: mCoaxCamCallback.setIntegerDeviceModule(node, value);break;
                case EModule.ERemoteDevice: mCoaxCamCallback.setIntegerRemoteModule(node, value);break;
                case EModule.EDataStream: mCoaxCamCallback.setIntegerStreamModule(node, value);break;
                default:break;
            }
        }

        private long GetValueInteger(EModule module, string node)
        {
            long value = 0;
            switch (module)
            {
                case EModule.EInterface:value = mCoaxCamCallback.getIntegerInterfaceModule(node);return value;
                case EModule.EDevice:value = mCoaxCamCallback.getIntegerDeviceModule(node);return value;
                case EModule.ERemoteDevice:value = mCoaxCamCallback.getIntegerRemoteModule(node);return value;
                case EModule.EDataStream:value = mCoaxCamCallback.getIntegerStreamModule(node);return value;
                default:return value;
            }
        }

        private void SetValueDouble(EModule module, string node, double value)
        {
            switch (module)
            {
                case EModule.EInterface: mCoaxCamCallback.setFloatInterfaceModule(node, value);break;
                case EModule.EDevice: mCoaxCamCallback.setFloatDeviceModule(node, value);break;
                case EModule.ERemoteDevice: mCoaxCamCallback.setFloatRemoteModule(node, value);break;
                case EModule.EDataStream: mCoaxCamCallback.setFloatStreamModule(node, value);break;
                default:break;
            }
        }

        private double GetValueDouble(EModule module, string node)
        {
            double value = 0.0;
            switch (module)
            {
                case EModule.EInterface:value = mCoaxCamCallback.getFloatInterfaceModule(node);return value;
                case EModule.EDevice:value = mCoaxCamCallback.getFloatDeviceModule(node);return value;
                case EModule.ERemoteDevice:value = mCoaxCamCallback.getFloatRemoteModule(node);return value;
                case EModule.EDataStream:value = mCoaxCamCallback.getFloatStreamModule(node);return value;
                default:return value;
            }
        }

        private void SetValueString(EModule module, string node, string value)
        {
            switch (module)
            {
                case EModule.EInterface: mCoaxCamCallback.setStringInterfaceModule(node, value);break;
                case EModule.EDevice: mCoaxCamCallback.setStringDeviceModule(node, value);break;
                case EModule.ERemoteDevice: mCoaxCamCallback.setStringRemoteModule(node, value);break;
                case EModule.EDataStream: mCoaxCamCallback.setStringStreamModule(node, value);break;
                default:break;
            }
        }

        private string GetValueString(EModule module, string node)
        {
            string value = "";
            switch (module)
            {
                case EModule.EInterface: value = mCoaxCamCallback.getStringInterfaceModule(node);return value;
                case EModule.EDevice: value = mCoaxCamCallback.getStringDeviceModule(node);return value;
                case EModule.ERemoteDevice: value = mCoaxCamCallback.getStringRemoteModule(node);return value;
                case EModule.EDataStream: value = mCoaxCamCallback.getStringStreamModule(node);return value;
                default:
                    return value;
            }
        }

        public override bool Grab()
        {
            try
            {
                mCameraStatus.GrabResult = GrabResult.Ready;

                if (!mCameraStatus.SimEnable && mCoaxCamCallback == null)
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    return false;
                }

                EventOneGrabCoaxCam.Reset();
                ExecuteSoftwareTrigger();
                if (EventOneGrabCoaxCam.WaitOne(2000))
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
        protected void ExecuteSoftwareTrigger()
        {
            mCameraStatus.GrabResult = GrabResult.Process;

            if (mCameraStatus.SimEnable)
            {
                Task.Run(() => SimCallbackEuresys());
            }
            else
            {
                if (mCameraDatas.Param.TriggerMode == "On")
                {
                    //mCoaxCamCallback.executeRemoteModule("TriggerSoftware");

                    mCoaxCamCallback.executeDeviceModule("StartCycle");
                }
                else
                {
                    mCoaxCamCallback.start(1);
                }
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
        public unsafe void onNewBufferEvent(EGrabberCallbackMultiThread g, NewBufferData data)
        {
            lock (_EventLock)
            {
                try
                {
                    using (Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer(g, data))
                    {
                        EventOneGrabCoaxCam.Set();

                        IntPtr bufferAddress;
                        Int64 width, height, bufferSize;
                        string format = string.Empty;

                        buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out bufferAddress);
                        buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_SIZE, out bufferSize);
                        buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_WIDTH, out width);
                        buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_HEIGHT, out height);
                        //buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_PIXELFORMAT, out format);

                        //Span<byte> byteArray = new Span<byte>((byte*)imgPtr.ToPointer(), bufferSize);
                        if(GrabBuffer == IntPtr.Zero)
                            GrabBuffer = Marshal.AllocHGlobal((int)bufferSize);

                        System.Buffer.MemoryCopy(bufferAddress.ToPointer(), GrabBuffer.ToPointer(), bufferSize, bufferSize);
                       
                        GrabEventArg arg = new GrabEventArg()
                        {
                            Width = Convert.ToInt32(width),
                            Height = Convert.ToInt32(height),
                            PixelFormat = format,
                            //Image = (byte[])byteArray.ToArray().Clone()
                            ImagePtr = GrabBuffer
                        };

                        TransmissionGrabEvent(this, arg);
                        OccurredGrabCommand(this, arg);
                        CameraGrabEvent(this, arg);
                    };
                }
                catch (Exception ex)
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    EventOneGrabCoaxCam.Set();

                    Debug.WriteLine($"[onNewBufferEvent] Exception : {ex.Message}");
                }
            }

        }

        private unsafe void SimCallbackEuresys()
        {
            lock (_EventLock)
            {
                if (!string.IsNullOrEmpty(mCameraStatus.SimPath))
                {
                    try
                    {
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(mCameraStatus.SimPath);
                        var bmps = di.GetFiles().ToList().FindAll(File => File.Extension.ToLower().CompareTo(".bmp") == 0);

                        if (bmps.Count <= 0)
                        {
                            mCameraStatus.GrabResult = GrabResult.Error;
                            EventOneGrabCoaxCam.Set();
                            return;
                        }
                        EventOneGrabCoaxCam.Set();

                        Random r = new Random();
                        byte[] buff = System.IO.File.ReadAllBytes(bmps[r.Next(0, bmps.Count)].FullName);
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(buff))
                        using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms))
                        {
                            System.Drawing.Imaging.BitmapData pBitmapData = bmp.LockBits(
                                   new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                                   System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                            int iStride = pBitmapData.Stride;
                            int iHeight = bmp.Height;
                            int iWidth = bmp.Width;
                            //byte[] bImage = new byte[iHeight * iWidth];
                            //Marshal.Copy(pBitmapData.Scan0, bImage, 0, iHeight * iWidth);
                            if (GrabBuffer == IntPtr.Zero)
                                GrabBuffer = Marshal.AllocHGlobal((int)iHeight * iWidth);

                            System.Buffer.MemoryCopy((byte*)pBitmapData.Scan0.ToPointer(), GrabBuffer.ToPointer(), (int)iHeight * iWidth, (int)iHeight * iWidth);

                            GrabEventArg arg = new GrabEventArg()
                            {
                                Width = iWidth,
                                Height = iHeight,
                                PixelFormat = bmp.PixelFormat.ToString(),
                                //Image = bImage,
                                ImagePtr = GrabBuffer
                            };

                            TransmissionGrabEvent(this, arg);
                            OccurredGrabCommand(this, arg);
                            CameraGrabEvent(this, arg);
                            bmp.UnlockBits(pBitmapData);
                        }

                        di = null; r = null; buff = null;
                        bmps.Clear(); bmps = null;
                    }
                    catch
                    {
                        mCameraStatus.GrabResult = GrabResult.Error;
                        EventOneGrabCoaxCam.Set();
                    }
                }
            }
        }

        #endregion

    }
}
