using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TThreadUnit                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TThreadUnit
    {
        const int TH0 = 0;
        const int TH1 = 1;
        const int TH2 = 2;
        const int TH3 = 3;
        const int TH4 = 4;
        const int TH5 = 5;
        const int TH6 = 6;
        const int TH7 = 7;
        const int TH8 = 8;
        const int TH9 = 9;

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        static Mutex mMutex;
        const int MAX_THRD_PROC = 10;

        bool[]      bThreadAbort    = new bool    [MAX_THRD_PROC];
        bool[]      bThreadUse      = new bool    [MAX_THRD_PROC];
        int []      iThreadInterval = new int     [MAX_THRD_PROC];
        SpinWait[]  sw              = new SpinWait[MAX_THRD_PROC];

        //protected: /* Inheritable Vars.        */
        //SpinWait w = new SpinWait();

        //public:    /* Direct Accessable Vars.  */

        //SCAN TIME
        public double[] m_dScanTime = new double[30];
        public double[] m_dStrtTime = new double[30];

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private Thread[] Thrd = new Thread[MAX_THRD_PROC];

        //Thread Thrd1;
        //Thread Thrd2;
        //Thread Thrd3;
        //Thread Thrd4;

        Object Obj = new Object();

		static readonly object _locker = new object();

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TThreadUnit() 
        {
			mMutex = new Mutex();
            
            //
            Thrd[TH0] = new Thread(new ThreadStart(ThrdExcute0));
            Thrd[TH1] = new Thread(new ThreadStart(ThrdExcute1));
            Thrd[TH2] = new Thread(new ThreadStart(ThrdExcute2));
            Thrd[TH3] = new Thread(new ThreadStart(ThrdExcute3));
            Thrd[TH4] = new Thread(new ThreadStart(ThrdExcute4));
            Thrd[TH5] = new Thread(new ThreadStart(ThrdExcute5));
            Thrd[TH6] = new Thread(new ThreadStart(ThrdExcute6));
            //Thrd[TH7] = new Thread(new ThreadStart(ThrdExcute7));
            //Thrd[TH8] = new Thread(new ThreadStart(ThrdExcute8));
            
            //
            for (int n = 0; n < MAX_THRD_PROC; n++)
            {
                bThreadUse     [n] = false; 
                iThreadInterval[n] = 1;
            }

        }
        ~TThreadUnit() { }
        //--------------------------------------------------------------------------
        public void StartThread()
        {

            //Set Use Thread
            bThreadUse[TH0] = true ;
            bThreadUse[TH1] = true ;
            bThreadUse[TH2] = true ;
            bThreadUse[TH3] = true ;
            bThreadUse[TH4] = true ; //

            bThreadUse[TH5] = true; //BCR
            bThreadUse[TH6] = true; //BCR

            iThreadInterval[TH5] = 10;

            //
            for (int n = 0; n < MAX_THRD_PROC; n++)
			{
                if (bThreadUse[n]) 
                { 
                    Thrd[n].IsBackground = true; 
                    if      (n == TH4) Thrd[n].Priority = ThreadPriority.Normal; 
                    else if (n == TH5) Thrd[n].Priority = ThreadPriority.Normal;
                    else               Thrd[n].Priority = ThreadPriority.Highest; 
                    Thrd[n].Start(); 
                }
            }
        }
        //--------------------------------------------------------------------------
        public void EndThread()
        {
            //
            for (int n = 0; n < MAX_THRD_PROC; n++)
            {
                if (bThreadUse[n])
                {
                    bThreadAbort[n] = true;
                    Thrd[n].Join();
                }
            }

        }
        //------------------------------------------------------------------------
		static async void TaskSleep(int Delayms)
		{
			await Task.Delay(Delayms);
		}
        //--------------------------------------------------------------------------
        public void ThrdExcute0()
        {
			//
			
            //int iThrdNo = Thread.CurrentThread.GetHashCode();
            try
            {                
                while (!bThreadAbort[0])
                {
					lock (_locker)
					{
						m_dScanTime[0] = cDEF.TICK._GetTickTime() - m_dStrtTime[0]; 
						m_dStrtTime[0] = cDEF.TICK._GetTickTime();

						Monitor.Enter(Obj);
						cDEF.SEQ.Update1();
				
						//mMutex.WaitOne();
						//Delay((int)iThreadInterval[0]);
						Thread.Sleep((int)iThreadInterval[0]);
                        //if (!w.NextSpinWillYield)
                        //sw[0].SpinOnce();
                        //Thread.SpinWait(1);

						Monitor.Exit(Obj);
					}
                }
                //Thread.CurrentThread.Abort();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Thread0] Exception - {ex.Message}");
                cDEF.LOG.ExceptionTrace("Thread0", ex);
            }
        }
        //--------------------------------------------------------------------------
        public void ThrdExcute1()
        {
            try
            {
                while (!bThreadAbort[1])
                {

                    m_dScanTime[1] = cDEF.TICK._GetTickTime() - m_dStrtTime[1]; 
                    m_dStrtTime[1] = cDEF.TICK._GetTickTime();

                    cDEF.SEQ.Update2();
                    //Thread.Sleep((int)iThreadInterval[1]);
                    sw[1].SpinOnce();
                }
                //Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Thread1] Exception - {ex.Message}");
                cDEF.LOG.ExceptionTrace("Thread1", ex);
            }
        }
        //--------------------------------------------------------------------------
        public void ThrdExcute2()
        {
            try
            {
                while (!bThreadAbort[2])
                {
                    m_dScanTime[2] = cDEF.TICK._GetTickTime() - m_dStrtTime[2]; 
                    m_dStrtTime[2] = cDEF.TICK._GetTickTime();

                    cDEF.SEQ.Update3();
                    //Thread.Sleep((int)iThreadInterval[2]);
                    sw[2].SpinOnce();
                }
                //Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Thread2] Exception - {ex.Message}");
                cDEF.LOG.ExceptionTrace("Thread2", ex);
            }

        }
        //--------------------------------------------------------------------------
        public void ThrdExcute3()
        {
            try
            {
                while (!bThreadAbort[3])
                {
                    m_dScanTime[3] = cDEF.TICK._GetTickTime() - m_dStrtTime[3]; 
                    m_dStrtTime[3] = cDEF.TICK._GetTickTime();

                    try
                    {
                        cDEF.SEQ.AutoRun();
                        //Thread.Sleep(iThreadInterval[3]);
                        sw[3].SpinOnce();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"ThrdExcute3 --> Exception : {e.Message}");
                        cDEF.LOG.ExceptionTrace("Th3" , e);
                    }
                }
                //Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("Thread3", ex);
                Debug.WriteLine($"ThrdExcute3 --> Exception : {ex.Message}");
            }
        }
        //------------------------------------------------------------------------
        public void ThrdExcute4()
        {
            try
            {
                while (!bThreadAbort[TH4])
                {
                    m_dScanTime[TH4] = cDEF.TICK._GetTickTime() - m_dStrtTime[TH4];
                    m_dStrtTime[TH4] = cDEF.TICK._GetTickTime();

                    try
                    {
                        cDEF.VISN.Update();

                        sw[4].SpinOnce();
                    }
                    catch (Exception e)
                    {
                        cDEF.LOG.ExceptionTrace("Th4", e);
                        Debug.WriteLine($"ThrdExcute4 --> Exception : {e.Message}");
                    }
                }
                //Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("Thread4", ex);
                Debug.WriteLine($"ThrdExcute4 --> Exception : {ex.Message}");
            }
        }
        //------------------------------------------------------------------------
        public void ThrdExcute5()
        {
            try
            {
                while (!bThreadAbort[TH5])
                {
                    m_dScanTime[TH5] = cDEF.TICK._GetTickTime() - m_dStrtTime[TH5];
                    m_dStrtTime[TH5] = cDEF.TICK._GetTickTime();

                    try
                    {
                        //
                        cDEF.BCR?.Update();
                        //cDEF.TempAutonics.Update();

                        Thread.Sleep(1);
                        //sw[5].SpinOnce();

                    }
                    catch (Exception e)
                    {
                        cDEF.LOG.ExceptionTrace("Th5", e);
                        Debug.WriteLine($"ThrdExcute5 --> Exception : {e.Message}");
                    }
                }
                //Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("Thread5", ex);
                Debug.WriteLine($"ThrdExcute5 --> Exception : {ex.Message}");
            }
        }
        public void ThrdExcute6()
        {
            try
            {
                while (!bThreadAbort[TH6])
                {
                    m_dScanTime[TH6] = cDEF.TICK._GetTickTime() - m_dStrtTime[TH6];
                    m_dStrtTime[TH6] = cDEF.TICK._GetTickTime();

                    try
                    {
                        Thread.Sleep(300);
                        //
                        cDEF.VISN.UpdateLog();

                        //sw[5].SpinOnce();

                    }
                    catch (Exception e)
                    {
                        cDEF.LOG.ExceptionTrace("TH6", e);
                        Debug.WriteLine($"ThrdExcute5 --> Exception : {e.Message}");
                    }
                }
                //Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("Thread6", ex);
                Debug.WriteLine($"ThrdExcute6 --> Exception : {ex.Message}");
            }
        }

    }
}
