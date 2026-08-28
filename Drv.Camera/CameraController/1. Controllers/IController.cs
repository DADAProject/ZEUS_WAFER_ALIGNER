using System;

namespace Drv.CameraController
{
    /// <summary>
    /// 컨트롤러 카메라 인터 페이스
    /// </summary>
    public interface IController
    {
        event CameraGrabHandler GrabEvent;

        event TEventHandler<string> RequestLogWriteEvent;

        /// <summary>
        /// 모든 카메라의 Enable상태 리턴
        /// </summary>
        /// <returns></returns>
        bool GetStatusEnableAll();

        /// <summary>
        /// 모든 카메라의 Connection상태 리턴
        /// </summary>
        /// <returns></returns>
        bool GetStatusConnectionAll();

        /// <summary>
        /// 각 축별 상태 에러 메시지 반환
        /// </summary>
        /// <returns></returns>
        string[] GetStatusErrMsg();

        /// <summary>
        /// 알람 이벤트
        /// </summary>
        void RequestLogWrite(string pStr);

        /// <summary>
        /// 보드  데이터
        /// </summary>
        string InitData { get; }


        /// <summary>
        /// 보드 별칭
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 보드 순서
        /// </summary>
        int ID { get; }


        /// <summary>
        /// 각각의 카메라을 묶고 있는 그래버관리 클레스
        /// </summary>
        cGrabber Grabber { get; }

        /// <summary>
        /// 그래버 초기화 및 연결
        /// </summary>
        /// <param name="pInitializeData">각 제어기에 맞는 초기화 데이터</param>
        /// <returns>초기화 성공 여부</returns>
        bool Initialize(cControllerData pData);

        /// <summary>
        /// 객체 제거
        /// </summary>
        void Dispose();
    }
}