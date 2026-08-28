using System;
using System.Collections.Generic;

namespace Drv.CameraController
{
    /// <summary>
    /// 축을 제어할 인터페이스
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// Occurs Camera Grab Evnet
        /// </summary>
        event CameraGrabHandler GrabEvent;

        Dictionary<Type, CameraGrabHandler> OnGrabEvent { get; set; }

        /// <summary>
        /// Camera Name String
        /// </summary>
        string CameraName { get; }

        /// <summary>
        /// Camera Control Key
        /// </summary>
        int ID { get; }


        bool WasInitiated { get; set; }


        /// <summary>
        /// Camera Status (Connection, Enable, GrabResult)
        /// </summary>
        sCameraStatus CameraStatus { get;  }

        /// <summary>
        /// Read Camera Alarm Text
        /// </summary>
        /// <returns>Alarm Text Array</returns>
        string[] GetCameraAlarmString();

        eDefaultCameraAlarm GetDefaultAlarm();

        /// <summary>
        /// Camera Enable ON/OFF
        /// </summary>
        /// <param name="pOn">True = On, False = Off</param>
        /// <param name="TimeOut">타임 아웃 시간</param>
        /// <returns>True = Succeed, False = Failed</returns>
        bool SetEnable(bool pOn,int TimeOut = 0);

        /// <summary>
        /// Camera Simulation Enable ON/OFF
        /// </summary>
        /// <param name="pOn">True = On, False = Off</param>
        /// <param name="sPath">이미지 경로</param>
        /// <returns>True = Succeed, False = Failed</returns>
        bool SetSimEnable(bool pOn, string sPath);

        /// <summary>
        /// Set Camera Parameter 
        /// </summary>
        /// <param name="pName">True = On, False = Off</param>
        /// <param name="pValue">Value</param>
        /// <returns>True = Succeed, False = Failed</returns>
        bool SetParameter(string pName, string pValue);

        /// <summary>
        /// Camera Alarm Clear
        /// </summary>
        /// <returns>True = Succeed, False = Failed</returns>
        bool AlarmClear();

        bool Grab();

        GrabEventArg GrabAndGetReply(TimeSpan pTimeout);
        
        void Update();

        void Dispose();

        void AddUpdateOnGrabCommand(Type midType, CameraGrabHandler deleg);
      
    }
}