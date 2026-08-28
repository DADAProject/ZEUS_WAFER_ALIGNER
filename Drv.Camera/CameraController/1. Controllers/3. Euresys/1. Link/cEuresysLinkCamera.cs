using Euresys.MultiCam;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Drv.CameraController
{
    public class cEuresysLinkCamera : cBaseCamera, ICamera, IDisposable
    {
        #region << Fields >>
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
            Master       = pMaster;
            ID           = pData.ID;
            CameraName   = pData.CameraName;
            mCameraDatas = pData;

            try
            {
                OnGrabEvent = new Dictionary<Type, CameraGrabHandler>();
                EventOneGrabMultiCam = new EventWaitHandle(false, EventResetMode.AutoReset);
                MC.OpenDriver();

                MC.SetParam((uint)(MC.BOARD + Master.ID), "BoardTopology", Master.InitData); //그래버의 보드 타입 
                MC.Create("CHANNEL", out mCamInstance);
                MC.SetParam(mCamInstance, "DriverIndex", Master.ID); //보드 인덱스

                SetMultiCam(mCameraDatas);
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

        public bool SetParameter(string pName, string pValue)
        {
            return true;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (mCamInstance != uint.MaxValue)
                {
                    MC.Delete(mCamInstance);
                    MC.CloseDriver();
                }

                if(GrabBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(GrabBuffer);

                disposed = true;
            }
        }
        private void SetMultiCam(cCameraDatas data)
        {
            try
            {
                MC.SetParam(mCamInstance, "Connector", data.Param.CamConnector);

                if (!string.IsNullOrEmpty(data.Param.CamFilePath))
                    MC.SetParam(mCamInstance, "CamFile", data.Param.CamFilePath);

                if (!data.Param.UseOnlyCameFile)
                {
                    //=============================================================

                    //Set Trigger Param
                    MC.SetParam(mCamInstance, "TrigMode", data.Param.TriggerMode); //"SOFT"
                    MC.SetParam(mCamInstance, "AcquisitionMode", "SNAPSHOT");
                    MC.SetParam(mCamInstance, "Expose_us", data.Param.ExposureTime); // Expose_us (Sentch) ,Exposure_us

                    //=============================================================     
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Exception] SetMultiCam = {ex.Message}");
            }
            finally
            {
                // Register the callback function
                mMultiCamCallback = new MC.CALLBACK(CallbackEuresys);
                MC.RegisterCallback(mCamInstance, mMultiCamCallback, mCamInstance);

                // Enable the signals corresponding to the callback functions
                MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_START_ACQUISITION_SEQUENCE, "ON");
                MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
                MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_END_CHANNEL_ACTIVITY, "ON");
                MC.SetParam(mCamInstance, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");

                // Prepare the channel in order to minimize the acquisition sequence startup latency
                MC.GetParam(mCamInstance, "ChannelState", out string sChannelState);
                MC.SetParam(mCamInstance, "ChannelState", "READY");

                if (data.Param.TriggerMode != "IMMEDIATE")
                {
                    MC.SetParam(mCamInstance, "SeqLength_Fr", -1);
                    MC.SetParam(mCamInstance, "ChannelState", "ACTIVE");
                }
                else MC.SetParam(mCamInstance, "SeqLength_Fr", 1);
            }

        }

        public override bool Grab()
        {
            try
            {
                mCameraStatus.GrabResult = GrabResult.Ready;

                if (!mCameraStatus.SimEnable && mCamInstance == uint.MaxValue)
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
                if (mCameraDatas.Param.TriggerMode == "SOFT" || mCameraDatas.Param.TriggerMode == "COMBINED")
                {
                    MC.SetParam(mCamInstance, "ForceTrig", "TRIG");
                }
                else if(mCameraDatas.Param.TriggerMode == "IMMEDIATE")
                {
                    MC.SetParam(mCamInstance, "ChannelState", "ACTIVE");
                }
                else // HARD
                {

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

        private unsafe void CallbackEuresys(ref MC.SIGNALINFO signalInfo)
        {
            lock (_EventLock)
            {
                UInt32 currentChannel = (UInt32)signalInfo.Context;

                try
                {
                    switch (signalInfo.Signal)
                    {
                        case MC.SIG_SURFACE_PROCESSING:
                            EventOneGrabMultiCam.Set();

                            var _CurrentSurface = signalInfo.SignalInfo;
                            Int32 width, height, bufferPitch, bufferSize;
                            IntPtr bufferAddress;
                            MC.GetParam(currentChannel, "ImageSizeX", out width);
                            MC.GetParam(currentChannel, "ImageSizeY", out height);
                            MC.GetParam(currentChannel, "BufferPitch", out bufferPitch);
                            MC.GetParam(currentChannel, "BufferSize", out bufferSize);
                            MC.GetParam(_CurrentSurface, "SurfaceAddr", out bufferAddress);
                            //Span<byte> byteArray = new Span<byte>((byte*)bufferAddress.ToPointer(), bufferSize);

                            if (GrabBuffer == IntPtr.Zero)
                                GrabBuffer = Marshal.AllocHGlobal((int)bufferSize);
                            
                            Buffer.MemoryCopy(bufferAddress.ToPointer(), GrabBuffer.ToPointer(), bufferSize, bufferSize);

                            GrabEventArg arg = new GrabEventArg()
                            {
                                Width = Convert.ToInt32(width),
                                Height = Convert.ToInt32(height),
                                PixelFormat = "",
                                //Image = (byte[])byteArray.ToArray().Clone()
                                ImagePtr = GrabBuffer
                            };
                            //byteArray = null;
                            TransmissionGrabEvent(this, arg);
                            OccurredGrabCommand(this, arg);
                            CameraGrabEvent(this, arg);
                            break;
                        case MC.SIG_ACQUISITION_FAILURE:
                        case MC.SIG_END_CHANNEL_ACTIVITY:
                        default:
                            break;
                    }
                }
                catch
                {
                    mCameraStatus.GrabResult = GrabResult.Error;
                    EventOneGrabMultiCam.Set();
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
                            EventOneGrabMultiCam.Set();
                            return;
                        }
                        EventOneGrabMultiCam.Set();

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
                            //Marshal.Copy(pBitmapData.Scan0, bImage,0, iHeight * iWidth);

                            if (GrabBuffer == IntPtr.Zero)
                                GrabBuffer = Marshal.AllocHGlobal((int)iHeight * iWidth);

                            Buffer.MemoryCopy((byte*)pBitmapData.Scan0.ToPointer(), GrabBuffer.ToPointer(), (int)iHeight * iWidth, (int)iHeight * iWidth);

                            GrabEventArg arg = new GrabEventArg()
                            {
                                Width = iWidth,
                                Height = iHeight,
                                PixelFormat = bmp.PixelFormat.ToString(),
                               // Image = bImage,
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
                        EventOneGrabMultiCam.Set();
                    }
                }

            }
        }

        #endregion

    }
}
