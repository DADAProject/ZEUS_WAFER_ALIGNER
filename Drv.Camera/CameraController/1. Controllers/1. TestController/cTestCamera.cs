using System;
using System.Diagnostics;

namespace Drv.CameraController
{
    public class cTestCamera : cBaseCamera, ICamera
    {
        public cTestCamera(IController pMaster, cCameraDatas pData)
        {
            Master = pMaster;
            ID = pData.ID;
        }

        public eDefaultCameraAlarm GetDefaultAlarm()
        {
            return eDefaultCameraAlarm.None;
        }

      
        public sCameraStatus       mCameraStatus            = new sCameraStatus();

        /// <summary>
        /// Camera Status (Enable, InPosition, Moving, Accelerating)
        /// </summary>
        public sCameraStatus CameraStatus
        { 
            get { return mCameraStatus; }
            protected set
            {
                mCameraStatus = value;
            }
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Read Camera Alarm Text
        /// </summary>
        /// <returns>Alarm Text Array</returns>
        public string[] GetCameraAlarmString()
        {
             //List<string> alarm = new List<string>();

             //if (mSignalStatus.PositiveSoftLimit) alarm.Add("Positive Software Limit +");
             //  if (mSignalStatus.PositiveSoftLimit) alarm.Add("Negative Software Limit -");

             //  if (Convert.ToBoolean(mSignalStatus & eDefaultAixsAlarm.Positive_Limit)) alarm.Add("Positive Limit +");
             //  if (Convert.ToBoolean(mSignalStatus & eDefaultAixsAlarm.Negative_Limit)) alarm.Add("Negative Limit -");

             //  if (Convert.ToBoolean(defaultAlarm & eDefaultAixsAlarm.Servo_Alarm))
             //  {
             //      WasInitiated = false;
             //      alarm.Add("Servo Alarm");
             //  }


            return new string[0];
        }

        /// <summary>
        /// Servo ON/OFF
        /// </summary>
        /// <param name="pOn">True = On, False = Off</param>
        /// <param name="TimeOut">타임 아웃 시간</param>
        /// <returns>True = Succeed, False = Failed</returns>
        public bool SetServoOn(bool pOn, int TimeOut = 0)
        {
            //mAxisStatus.InPosition = true;
            //mAxisStatus.Enable = pOn;
            //mSignalStatus.ServoOn = true;
            return true;
        }

        public bool SetParameter(string pName, string pValue)
        {
            return true;
        }
        public override bool Grab()
        {
            return false;
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
           // if(this.Position != TargetPosition)
           //{
           //    if(mAxisStatus.Moving == true)
           //    {
           //        double      t      = (Stopwatch.GetTimestamp() - mMovingStartTime) / Stopwatch.Frequency;
           //        sTestMotion m      = Traipzoidal(t);
           //        bool        isStop = m.Acc == 0 && Position == TargetPosition;
           //
           //        if(IsJogging == true)
           //        {
           //            isStop = false;
           //            TargetPosition = Position;
           //        }
           //
           //        if (isStop || MustStop)
           //        {
           //            TargetPosition = Position;
           //            mAxisStatus.Accelerating = false;
           //            mAxisStatus.Moving       = false;
           //            mAxisStatus.InPosition   = true;
           //            MustStop = false;
           //            IsJogging = false;
           //            //System.Diagnostics.Debug.WriteLine("0");
           //        }
           //        else
           //        {
           //            //System.Diagnostics.Debug.WriteLine(m.Position);
           //            Position = m.Position;
           //            mAxisStatus.Accelerating = m.Acc != 0;
           //            mAxisStatus.InPosition   = false;
           //            //System.Diagnostics.Debug.WriteLine("x");
           //        }
           //    }
           //}
        }

        public bool SetEnable(bool pOn, int TimeOut = 0)
        {
            throw new NotImplementedException();
        }

        public bool SetSimEnable(bool pOn, string sPath)
        {
            throw new NotImplementedException();
        }
   
        public new GrabEventArg GrabAndGetReply(TimeSpan pTimeout)
        {
            throw new NotImplementedException();
        }


    }
}
