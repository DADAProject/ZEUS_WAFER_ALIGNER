namespace Drv.CameraController
{
    /// <summary>
    /// 각각의 카메라을 묶고 있는 인터페이스
    /// </summary>
    public interface IGrabber
    {
        /// <summary>
        /// 카메라의 이름으로 카메라을 가져온다
        /// </summary>
        /// <param name="pName">카메라 이름</param>
        /// <returns>제어할 카메라 반환</returns>
        ICamera this[string pName] { get; }

        /// <summary>
        /// 카메라의 ID로 카메라을 가져온다
        /// </summary>
        /// <param name="pNCamera">카메라 ID</param>
        /// <returns>제어할 카메라 반환</returns>
        ICamera this[int pNCamera] { get; }


        ///// <summary>
        ///// 다축 이동 함수
        ///// </summary>
        ///// <param name="pAxes">이동할 축 배열</param>
        ///// <param name="pPos">이동할 위치 배열</param>
        ///// <param name="pSpeedPercent">이동할 속도 1 ~ 100% (설정된 최대 속도에 비례한 최대 속도)</param>
        ///// <param name="pWaitMotionEnd">True일 경우 위치 값에 도착할 때 까지 기다린다</param>
        ///// <returns>True = Succeed, False = Failed</returns>
        //bool MxMove(string[] pAxes, double[] pPos, int pSpeedPercent, bool pWaitMotionEnd = false);
    }
}