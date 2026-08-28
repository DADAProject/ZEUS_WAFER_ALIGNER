using System;


namespace Drv.CameraController
{
    public class cCameraControlerException : Exception
    {
        public cCameraControlerException(string pMessage)
            : base(pMessage)
        {
        }
    }
}