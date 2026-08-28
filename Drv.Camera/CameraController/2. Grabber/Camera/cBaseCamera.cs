using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Drv.CameraController
{
    /// <summary>
    /// 컨트롤러 카메라의 베이스 클레스
    /// </summary>
    public abstract class cBaseCamera
    {
        /// <summary>
        /// Camera Grab Buffer
        /// </summary>
        internal IntPtr GrabBuffer;

        /// <summary>
        /// Occurs Camera Grab Event
        /// </summary>
        public event CameraGrabHandler GrabEvent;

        public Dictionary<Type, CameraGrabHandler> OnGrabEvent { get; set; }

        /// <summary>
        /// Occurs Camera Simultation Grab Event
        /// </summary>
        public event SimulationEventHandler GrabSimEvent;
        
        /// <summary>
        /// Parent Camera Control
        /// </summary>
        public IController Master { get; protected set; }

        /// <summary>
        /// Camera Name String
        /// </summary>
        public string CameraName { get; protected set; }

        /// <summary>
        /// Camera Control Key
        /// </summary>
        public int ID { get; protected set; }


        public bool WasInitiated { get; set; }

        //===================
        protected readonly object _GrapLock = new object();
        protected readonly object _EventLock = new object();
        protected event EventHandler<GrabEventArg> _GrabEvent;
        public  abstract bool Grab();

        //===================

        protected void CameraGrabEvent(ICamera pSender, GrabEventArg e)
        {
            GrabEvent?.Invoke(pSender, e);
            //GrabEvent?.BeginInvoke(pSender, e, null, null);
            //Task.Run(() => GrabEvent?.Invoke(pSender, e));  
        }

        protected void CameraGrabSimEvent()
        {
            GrabSimEvent?.Invoke();
            //Task.Run(() => GrabSimEvent?.Invoke());
        }
        //========================
        public GrabEventArg GrabAndGetReply(TimeSpan pTimeout)
        {
            lock (_GrapLock)
            {
                if (pTimeout == null)
                    pTimeout = TimeSpan.FromSeconds(10);

                GrabEventArg mReply = null;

                EventHandler<GrabEventArg> ev = (s, e) => { mReply = e; };
                this._GrabEvent += ev;
                if (Grab() == false) return mReply;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (mReply == null && sw.Elapsed < pTimeout)
                    ThreadExtension.Delay(10);

                //Thread.Sleep(10);

                this._GrabEvent -= ev;
                this._GrabEvent = null;
                return mReply;
            }
        }

        protected void TransmissionGrabEvent(ICamera pSender, GrabEventArg e)
        {
            GrabEventArg arg = e;

            if (this._GrabEvent != null)
            {
                this._GrabEvent(this, arg);
                return;
            }
        }

        //===============
        public void AddUpdateOnGrabCommand(Type midType, CameraGrabHandler deleg)
        {
            if (OnGrabEvent.ContainsKey(midType))
                OnGrabEvent[midType] = deleg;
            else
                OnGrabEvent.Add(midType, deleg);
        }

        protected void OccurredGrabCommand(ICamera pSender, GrabEventArg e)
        {
            GrabEventArg arg = e;

            if(this.OnGrabEvent == null) return;

            foreach (KeyValuePair<Type, CameraGrabHandler> action in this.OnGrabEvent)
            {
                action.Value(pSender, new GrabEventArg()
                {
                    Height = arg.Height,
                    Width = arg.Width,
                    PixelFormat = arg.PixelFormat,
                    Image = arg.Image,
                    ImagePtr = arg.ImagePtr,
                });

            }
          
        }

    }
}