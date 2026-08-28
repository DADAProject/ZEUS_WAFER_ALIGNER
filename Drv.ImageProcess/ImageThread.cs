using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Drv.ImageProcess
{
    public enum EN_THREAD_TYPE
    {
        Auto,
        Manual,
        Update1,
        Update2,
    }

    public class ImageThread
    {
        public enum CycleProcStat
        {
            Idle,  //준비상태 
            Start,  //Cycle Start 상태 
            Delay,  //대기 상태 
            Run,  //Cycle ing 상태 
            Error,
            Fnsh     // Fnsh 상태
        }

        const int MAX_THRD_PROC = 4;
        bool[] bThreadAbort = new bool[MAX_THRD_PROC];
        bool[] bThreadUse = new bool[MAX_THRD_PROC];
        int[] iThreadInterval = new int[MAX_THRD_PROC];
        SpinWait[] sw = new SpinWait[MAX_THRD_PROC];
        Thread[] Thrd = new Thread[MAX_THRD_PROC];

        CycleProcStat CycleProc;
        bool m_bValid;

        int m_iStep;                                         //Step 처리 
        int m_iCycleDly;                                     //Cycle Delay
        bool m_bRqStart;                                     //Cycle Flag


        TOnDelayTimer m_tCycle = new TOnDelayTimer();  //Cycle TimeOut 처리 

        public delegate bool AutoCycleHandler();


        public AutoCycleHandler _fAutoCycle;

        public bool UseAutoGCCollector { get; set; }

        public ImageThread()
        {
            Process currentProcess = Process.GetCurrentProcess();

            foreach (ProcessThread processThread in currentProcess.Threads)
            {
                processThread.PriorityBoostEnabled = true;
                processThread.ProcessorAffinity = currentProcess.ProcessorAffinity;
            }

            for (int i = 0; i < MAX_THRD_PROC; i++)
            {
                Thrd[i] = new Thread(new ParameterizedThreadStart(OneVisionCycle));
                bThreadUse[i] = true; iThreadInterval[i] = 1;
            }

            //Auto Run
            CycleProc = CycleProcStat.Idle;
            m_iStep = 0;
            m_bRqStart = false;
            UseAutoGCCollector = false;
        }

        #region  << Methods >>

        public void StartThread()
        {
            for (int i = 0; i < MAX_THRD_PROC; i++)
            {
                if (bThreadUse[i])
                {
                    //Thrd[i].ApartmentState = ApartmentState.MTA;
                    Thrd[i].IsBackground = true;
                    if (i == 0) Thrd[i].Priority = ThreadPriority.Highest;
                    else Thrd[i].Priority = ThreadPriority.Normal;
                    Thrd[i].Start(i);
                }
            }
        }

        public void EndThread()
        {
            for (int i = 0; i < MAX_THRD_PROC; i++)
            {
                bThreadAbort[i] = true;
                Thrd[i].Join();
            }
        }

        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
        private void OneVisionCycle(object id)
        {
            try
            {
                while (true)
                {
                    OneCycleMethode(id);
                    sw[(int)id].SpinOnce();
                    //Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                //이벤트 전달
                Debug.WriteLine($"[OneVisionCycle] Exception : {e.Message}");
            }
        }

        private bool OneCycleMethode(object id)
        {
            if ((EN_THREAD_TYPE)Convert.ToInt32(id) == EN_THREAD_TYPE.Manual)
            {
                ManualCycleMethode();
            }

            if ((EN_THREAD_TYPE)Convert.ToInt32(id) == EN_THREAD_TYPE.Auto)
            {
                AutoCycleMethode();
            }

            //오토런 매뉴얼 
            return true;
        }
        private bool AutoCycleMethode()
        {
            bool isStrtErr = (CycleProc == CycleProcStat.Start) && m_iStep != 10;
            bool isDrng = m_iStep > 10;

            m_tCycle.OnDelay(isStrtErr || isDrng, 5000);
            if (m_tCycle.Out)
            {
                m_tCycle.Clear();
                CycleProc = CycleProcStat.Error;
                m_iStep = 0;
                return false;
            }

            try
            {
                if (m_iStep == 0)
                {
                    if (m_bRqStart)
                    {
                        m_bRqStart = false;
                        CycleProc = CycleProcStat.Delay;
                        m_iStep = 10;
                    }
                }

                if (m_iStep == 10)
                {
                    CycleProc = CycleProcStat.Run;
                    if (AutoCycle()) m_iStep++;
                    //if (_fAutoCycle is null) return false; 
                    //if (_fAutoCycle.Invoke()) m_iStep++;
                }

                if (m_iStep == 11)
                {
                    CycleProc = CycleProcStat.Fnsh;
                    m_iStep = 0;

                    if (UseAutoGCCollector)
                        Task.Run(() => GC.Collect()); //MemoryFree

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AutoCycleMethode] Exception : {ex.Message}");

                CycleProc = CycleProcStat.Error;
                m_iStep = 0;
                return false;
            }

            return true;
        }

        private bool ManualCycleMethode()
        {
            //if (ThreadPool.SetMinThreads(0, 0) && ThreadPool.SetMaxThreads(2, 0))
            //{
            //    // ThreadPool에 등록, 델리게이트 함수로 ThreadProc를 등록
            //    // 파라미터로 Node 인스턴스를 생성해서 넘긴다.
            //    ThreadPool.QueueUserWorkItem(ThreadProc, new Node { Text = "A", Count = 3, Tick = 1000 });
            //    ThreadPool.QueueUserWorkItem(ThreadProc, new Node { Text = "B", Count = 5, Tick = 10 });
            //    ThreadPool.QueueUserWorkItem(ThreadProc, new Node { Text = "C", Count = 2, Tick = 500 });
            //    ThreadPool.QueueUserWorkItem(ThreadProc, new Node { Text = "D", Count = 7, Tick = 300 });
            //    ThreadPool.QueueUserWorkItem(ThreadProc, new Node { Text = "E", Count = 4, Tick = 200 });
            //}

            return true;
        }
        #endregion

        #region  << Vurtural Methods >>
        public virtual bool ManualCycle() { return false; }
        public virtual bool AutoCycle() { return false; }

        public virtual void OneCycleReset()
        {
            if (m_iStep != 0) return;
            CycleProc = CycleProcStat.Idle;
        }
        public virtual bool OneCycleStart()
        {
            CycleProc = CycleProcStat.Start;
            if (m_iStep != 0) return false;
            if (m_bRqStart) return false;

            m_bRqStart = true;
            System.Threading.Thread.Sleep(0);
            return true;
        }
        public virtual bool OneCycleFnsh()
        {
            if (CycleProc == CycleProcStat.Error) return true;
            if (CycleProc != CycleProcStat.Fnsh) return false;

            return true;
        }
        #endregion

    }
}
