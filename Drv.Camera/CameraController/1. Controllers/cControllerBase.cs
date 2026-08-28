using System;
using System.Threading.Tasks;

namespace Drv.CameraController
{
    public class cControllerBase
    {
        public event CameraGrabHandler GrabEvent;

        public event TEventHandler<string> RequestLogWriteEvent;


        public bool IsDisposed { get; protected set; }


        /// <summary>
        /// 보드  데이터
        /// </summary>
        public string InitData { get; protected set; }

        /// <summary>
        /// 보드 별칭
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 보드 순서
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// 각각의 카메라을 묶고 있는 카메라관리 클레스
        /// </summary>
        public cGrabber Grabber { get; protected set; }

        /// <summary>
        /// 알람 이벤트
        /// </summary>
        public void RequestLogWrite(string pStr)
        {
            RequestLogWriteEvent?.Invoke(this,pStr);
        }

        /// <summary>
        /// 카메라에서 발생한 그랩이벤트 컨트롤러 이벤트 연동
        /// UI에서 쓸건지, Cycle에서 쓸건지 정해서 함수 바꿔줘야함
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="e"></param>
        protected void Controller_GrabEvent(ICamera pSender, GrabEventArg e)
        {
            //GrabEvent?.Invoke(pSender, e);
            //GrabEvent?.BeginInvoke(pSender, e, null, null);
            Task.Run(() => GrabEvent?.Invoke(pSender, e));
        }
    }
}
