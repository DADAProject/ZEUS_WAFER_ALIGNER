using System;
using System.Runtime.InteropServices;

namespace Euresys
{
    /// <summary>
    /// Class to expose the MultiCam C API in .NET
    /// </summary>
    public sealed class MC
    {
        /// <summary>
        /// Native functions imported from the MultiCam C API.
        /// </summary>
        #region Native Methods
        class NativeMethods
        {
            private NativeMethods() { }

            [DllImport("MultiCam.dll")]
            internal static extern Int32 McOpenDriver(IntPtr instanceName);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McCloseDriver();
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McCreate(UInt32 modelInstance, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McCreateNm(String modelName, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McDelete(UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamInt(UInt32 instance, UInt32 parameterId, Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmInt(UInt32 instance, String parameterName, Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamStr(UInt32 instance, UInt32 parameterId, String value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmStr(UInt32 instance, String parameterName, String value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamFloat(UInt32 instance, UInt32 parameterId, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmFloat(UInt32 instance, String parameterName, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamInst(UInt32 instance, UInt32 parameterId, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmInst(UInt32 instance, String parameterName, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamPtr(UInt32 instance, UInt32 parameterId, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmPtr(UInt32 instance, String parameterName, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamInt64(UInt32 instance, UInt32 parameterId, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmInt64(UInt32 instance, String parameterName, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamInt(UInt32 instance, UInt32 parameterId, out Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmInt(UInt32 instance, String parameterName, out Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamStr(UInt32 instance, UInt32 parameterId, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmStr(UInt32 instance, String parameterName, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamFloat(UInt32 instance, UInt32 parameterId, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmFloat(UInt32 instance, String parameterName, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamInst(UInt32 instance, UInt32 parameterId, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmInst(UInt32 instance, String parameterName, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamPtr(UInt32 instance, UInt32 parameterId, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmPtr(UInt32 instance, String parameterName, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamInt64(UInt32 instance, UInt32 parameterId, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmInt64(UInt32 instance, String parameterName, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McRegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McWaitSignal(UInt32 instance, Int32 signal, UInt32 timeout, out SIGNALINFO info);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetSignalInfo(UInt32 instance, Int32 signal, out SIGNALINFO info);
        }
        #endregion

        #region Private Constants
        private const Int32 MAX_VALUE_LENGTH = 1024;
        #endregion

        #region Default object instance Constants
        public const UInt32 CONFIGURATION = 0x20000000;
        public const UInt32 BOARD = 0xE0000000;
        public const UInt32 CHANNEL = 0x8000FFFF;
        public const UInt32 DEFAULT_SURFACE_HANDLE = 0x4FFFFFFF;
        #endregion

        #region Specific parameter values Constants
        public const Int32 INFINITE = -1;
        public const Int32 INDETERMINATE = -1;
        public const Int32 DISABLE = 0;
        #endregion

        #region Signal handling Constants
        public const UInt32 SignalEnable = (24 << 14);

        public const Int32 SIG_ANY = 0;
        public const Int32 SIG_SURFACE_PROCESSING = 1;
        public const Int32 SIG_SURFACE_FILLED = 2;
        public const Int32 SIG_UNRECOVERABLE_OVERRUN = 3;
        public const Int32 SIG_FRAMETRIGGER_VIOLATION = 4;
        public const Int32 SIG_START_EXPOSURE = 5;
        public const Int32 SIG_END_EXPOSURE = 6;
        public const Int32 SIG_ACQUISITION_FAILURE = 7;
        public const Int32 SIG_CLUSTER_UNAVAILABLE = 8;
        public const Int32 SIG_RELEASE = 9;
        public const Int32 SIG_END_ACQUISITION_SEQUENCE = 10;
        public const Int32 SIG_START_ACQUISITION_SEQUENCE = 11;
        public const Int32 SIG_END_CHANNEL_ACTIVITY = 12;

        public const Int32 SIG_GOLOW = (1 << 12);
        public const Int32 SIG_GOHIGH = (2 << 12);
        public const Int32 SIG_GOOPEN = (3 << 12);
        #endregion

        #region Signal handling Type Definitions
        public delegate void CALLBACK(ref SIGNALINFO signalInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct SIGNALINFO
        {
            public IntPtr Context;
            public UInt32 Instance;
            public Int32 Signal;
            public UInt32 SignalInfo;
            public UInt32 SignalContext;
        };
        #endregion

        #region Constructors
        private MC() { }
        #endregion

        #region Error handling Methods
        private static String GetErrorMessage(Int32 errorCode)
        {
            const UInt32 ErrorDesc = (98 << 14);
            String errorDescription;
            UInt32 status = (UInt32)Math.Abs(errorCode);
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            if (NativeMethods.McGetParamStr(CONFIGURATION, ErrorDesc + status, text, MAX_VALUE_LENGTH) != 0)
                errorDescription = "Unknown error";
            else
                errorDescription = Marshal.PtrToStringAnsi(text);
            Marshal.FreeHGlobal(text);
            return errorDescription;
        }

        private static void ThrowOnMultiCamError(Int32 status, String action)
        {
            if (status != 0)
            {
                String error = action + ": " + GetErrorMessage(status);
                throw new Exception(error);
            }
        }
        #endregion

        #region Driver connection Methods
        public static void OpenDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McOpenDriver((IntPtr)null),
                "Cannot open MultiCam driver");
        }

        public static void CloseDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McCloseDriver(),
                "Cannot close MultiCam driver");
        }
        #endregion

        #region Object creation/deletion Methods
        public static Int32 Create(UInt32 modelInstance, out UInt32 instance)
        {
            Int32 status = NativeMethods.McCreate(modelInstance, out instance);
            ThrowOnMultiCamError(status,
                String.Format("Cannot create '{0}' instance", modelInstance));
            return status;
        }

        public static Int32 Create(String modelName, out UInt32 instance)
        {
            Int32 status = NativeMethods.McCreateNm(modelName, out instance);

            ThrowOnMultiCamError(status,
                String.Format("Cannot create '{0}' instance", modelName));
            return status;
        }

        public static void Delete(UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McDelete(instance),
                String.Format("Cannot delete '{0}' instance", instance));
        }
        #endregion

        #region Parameter 'setter' Methods
        public static void SetParam(UInt32 instance, UInt32 parameterId, Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt(instance, parameterId, value), 
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamStr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmStr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamFloat(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmFloat(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInst(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInst(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamPtr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmPtr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt64(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt64(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }
        #endregion

        #region Parameter 'getter' Methods
        public static void GetParam(UInt32 instance, UInt32 parameterId, out Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamStr(instance, parameterId, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterId));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, String parameterName, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmStr(instance, parameterName, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterName));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamFloat(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmFloat(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInst(instance, parameterId, out  value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInst(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmPtr(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt64(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt64(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }
        #endregion

        #region Signal handling Methods
        public static void RegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context)
        {
            ThrowOnMultiCamError(NativeMethods.McRegisterCallback(instance, callbackFunction, context),
                "Cannot register callback");
        }

        public static void WaitSignal(UInt32 instance, Int32 signal, UInt32 timeout, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McWaitSignal(instance, signal, timeout, out info),
                "WaitSignal error");
        }

        public static void GetSignalInfo(UInt32 instance, Int32 signal, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McGetSignalInfo(instance, signal, out info),
                "Cannot get signal information");
        }
        #endregion


/*
        // The object that will contain the acquired image
        private Bitmap image = null;
        
        // The object that will contain the palette information for the bitmap
        private ColorPalette imgpal = null;

        // The Mutex object that will protect image objects during processing
        private static Mutex imageMutex = new Mutex();

        // The MultiCam object that contains the acquired buffer
        private UInt32 currentSurface;

        CALLBACK multiCamCallback;

        UInt32       m_uChannel    ;
        string       m_sCamFile    ;
        string       m_sConnector  ;
        int          m_iFmtPixel   ;
        bool         m_bTrigger    ;

        public MultiCam()
        {
        }

        bool Open (uint BoardId, String sCamType="MONO_DECA", String sConnector="X",  String sCamFile = "", int iExposeTime = 20000)
        {
            OpenDriver();
            SetParam(CONFIGURATION, "ErrorLog", "error.log");
            Create("CHANNEL", out m_uChannel);

            SetParam(m_uChannel, "DriverIndex", BoardId);
            SetParam(BOARD + BoardId, "BoardTopology", sCamType);  //"MONO_DECA", "MONO"
            SetParam(m_uChannel, "Connector", m_sConnector);

            if(m_sConnector == "M") 
                SetParam(m_uChannel, "ColorFormat", "Y8");

            // Choose the video standard CamFile Load
            SetParam(m_uChannel, "CamFile", sCamFile);

            // Choose the camera expose duration
            // Max 5000, 
            SetParam(m_uChannel, "Expose_us", iExposeTime);

            // Set the acquisition mode to snapShot
            SetParam(m_uChannel, "AcquisitionMode", "SNAPSHOT");

        //////// 여기 아래에 MC_TrigMode를 MC_TrigMode_COMBINED으로 설정하면 하드웨어 트리거 및 소프트웨어 트리거 둘 다 쓴다는 내용입니다.

            SetParam(m_uChannel, "TrigMode", "COMBINED");
            // Choose the way the first acquisition is triggered
            //    val=McSetParamInt(m_Channel[camType], MC_TrigMode, MC_TrigMode_IMMEDIATE);
            // Choose the triggering mode for subsequent acquisitions
            //    val=McSetParamInt(m_Channel[camType], MC_NextTrigMode, MC_NextTrigMode_REPEAT);
            SetParam(m_uChannel, "NextTrigMode", "COMBINED"     );
            SetParam(m_uChannel, "TrigLine"    , "IIN1"         );
            SetParam(m_uChannel, "SeqLength_Fr", INDETERMINATE  );
           // 	val=McSetParamInt(m_Channel, MC_TrigLine, MC_TrigLine_NOM);
           // 	val=McSetParamInt(m_Channel, MC_TrigEdge, MC_TrigEdge_GOHIGH);
           // 	val=McSetParamInt(m_Channel, MC_TrigFilter, MC_TrigFilter_ON);
           // 	val=McSetParamInt(m_Channel, MC_TrigCtl, MC_TrigCtl_TTL);   
           //   Choose the number of images to acquire
           //   val=McSetParamInt(m_Channel, MC_SeqLength_Fr, MC_INDETERMINATE);

            // Enable MultiCam signals
           SetParam(m_uChannel, SignalEnable + SIG_SURFACE_FILLED      , "ON");
           SetParam(m_uChannel, SignalEnable + SIG_ACQUISITION_FAILURE , "ON");
           SetParam(m_uChannel, SignalEnable + SIG_END_CHANNEL_ACTIVITY, "ON");

           SetParam(m_uChannel, SignalHandling + SIG_SURFACE_FILLED      , SIGNALHANDLING_WAITING_SIGNALING);
           SetParam(m_uChannel, SignalHandling + SIG_ACQUISITION_FAILURE , SIGNALHANDLING_WAITING_SIGNALING);
           SetParam(m_uChannel, SignalHandling + SIG_END_CHANNEL_ACTIVITY, SIGNALHANDLING_WAITING_SIGNALING);

            // Register the callback function
            multiCamCallback = new CALLBACK(HandleSignal);
            RegisterCallback(m_uChannel, multiCamCallback, m_uChannel);
            return true;

        }
        void Close            ()
        {
            try
            {
                if (m_uChannel != 0)
                    SetParam(m_uChannel, "ChannelState", "FREE");

                Thread.Sleep(5);

                CloseDriver();
            }
            catch (MultiCamException ex)
            {
                cDEF.LOG.ExceptionTrace(MethodBase.GetCurrentMethod().Name + " - " + ex.ToString());
            }

        }
        void OnTrigger        ()
        {
            m_bTrigger = true;

            String channelState;
            GetParam(m_uChannel, "ChannelState", out channelState);
            if (channelState != "ACTIVE")
                SetParam(m_uChannel, "ChannelState", "ACTIVE");

            // Generate a soft trigger event
            SetParam(m_uChannel, "ForceTrig", "TRIG");
        }
        void OnStop        ()
        {
            m_bTrigger = false;      
            if (m_uChannel != 0)
                SetParam(m_uChannel, "ChannelState", "IDLE");
        }


        private void ProcessingCallback(SIGNALINFO signalInfo)
        {
            UInt32 currentChannel = (UInt32)signalInfo.Instance;
            currentSurface = signalInfo.SignalInfo;
            try
            {

                // Update the image with the acquired image buffer data 
                Int32 width, height, bufferPitch;
                IntPtr bufferAddress;
                GetParam(currentChannel, "ImageSizeX" , out width        );
                GetParam(currentChannel, "ImageSizeY" , out height       );
                GetParam(currentChannel, "BufferPitch", out bufferPitch  );
                GetParam(currentSurface, "SurfaceAddr", out bufferAddress);

                try
                {
                    imageMutex.WaitOne();
                    image = new Bitmap(width, height, bufferPitch, PixelFormat.Format8bppIndexed, bufferAddress);
                    imgpal = image.Palette;
                    // Build bitmap palette Y8
                    for (uint i = 0; i < 256; i++)
                    {
                        imgpal.Entries[i] = Color.FromArgb((byte)0xFF, (byte)i, (byte)i, (byte)i);
                    }
                    image.Palette = imgpal;
                }
                finally
                {
                    imageMutex.ReleaseMutex();
                }

            }
            catch (MultiCamException ex)
            {
                cDEF.LOG.ExceptionTrace(MethodBase.GetCurrentMethod().Name + " - " + ex.ToString());

            }
            catch (System.Exception ex)
            {
                 cDEF.LOG.ExceptionTrace(MethodBase.GetCurrentMethod().Name + " - " + ex.ToString());
            }
        }
        private void AcqFailureCallback(SIGNALINFO signalInfo)
        {
            MessageBox.Show("Not Connected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void HandleSignal(ref SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal)
            {
                case SIG_SURFACE_FILLED:
                    ProcessingCallback(signalInfo);
                    break;
                case SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;
                default:
                    throw new MultiCamException("Unknown signal");
            }

            if (m_bTrigger) m_bTrigger = false;
        }
    }
*/
    }
}
