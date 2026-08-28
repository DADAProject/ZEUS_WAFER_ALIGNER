using System;


namespace Drv.CameraController
{
    /// <summary>
    /// 예외 처리 이벤트 핸들러
    /// </summary>
    /// <param name="pLocation">발생위치</param>
    /// <param name="pEx">발생한 예외</param>
    public delegate void ExceptionEventHandler(string pLocation, Exception pEx);
}