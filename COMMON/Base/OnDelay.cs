using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TTIckTime                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TTIckTime
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
        //--------------------------------------------------------------------------
        public double _GetTickTime ()
        {

            long liEndCounter,liFrequency ;
            QueryPerformanceCounter    (out liEndCounter);
            QueryPerformanceFrequency  (out liFrequency );

            return ( ((double)liEndCounter / (double)liFrequency) * 1000.0);
 
        }
        //--------------------------------------------------------------------------
        public double _GetTickTime_us ()
        {
    
            long liEndCounter,liFrequency ;
            QueryPerformanceCounter  (out liEndCounter);
            QueryPerformanceFrequency(out liFrequency );

            return ( ((double)liEndCounter / (double)liFrequency) *  1000000);
        }
 
    }

    /***************************************************************************/
    /* Class: TOnDelayTimer                                                    */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TOnDelayTimer : TTIckTime
    {
        bool   bScan          ; //0 : TickCount사용,  1 : Scan 단위
        double PreTickTime    ;

        public bool   Out     ;
		public double StrtTime;
        public double SetTime ; //[ms]설정 시간
        public double CurTime ; //[ms]현재 시간 (0 -> 설정시간 )

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TOnDelayTimer()
        {
        }
        ~TOnDelayTimer() { }

        //---------------------------------------------------------------------------
        public TOnDelayTimer(bool _bScan  ) { bScan = _bScan; Clear(); }
        //---------------------------------------------------------------------------
        public void Start        (        ) { StrtTime = _GetTickTime(); }
        //---------------------------------------------------------------------------
        public void Clear        (        ) { OnDelay(false , 0); }
        //---------------------------------------------------------------------------
        public bool OnDelay      (bool SeqInput, double _SetTime) { SetTime = _SetTime; return OnDelay(SeqInput); }
        //---------------------------------------------------------------------------
        public bool OnDelay      (bool SeqInput)
        {
            double CurTick = _GetTickTime();
            if (SeqInput) 
            {
                if (bScan)	CurTime++;
                else		CurTime += CurTick - PreTickTime;
                if (CurTime >= SetTime) { CurTime = SetTime; Out = true ; }
                else                                         Out = false;
            }
            else 
            {
                CurTime = 0;
                Out = false;
            }
            
            PreTickTime = CurTick;
            return Out;
        }
		public double GetSpanTime_ms()
		{
			double CurTick  = _GetTickTime();
			double SpanTime = CurTick - StrtTime;
			//TimeSpan ElapTime = new TimeSpan(SpanTime);
			return SpanTime; // * 1000.0F;
		}
    }


    /***************************************************************************/
    /* Class: TOnDelayTimer_us                                                 */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class TOnDelayTimer_us : TTIckTime
    {
    	bool   bScan      ; //0 : TickCount사용,  1 : Scan 단위
        double PreTickTime;

        public bool   Out    ;
        public double SetTime; //[ms]설정 시간
        public double CurTime; //[ms]현재 시간 (0 -> 설정시간 )

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TOnDelayTimer_us()
        {
        }
        ~TOnDelayTimer_us() { }

        public TOnDelayTimer_us(bool _bScan  ) { bScan = _bScan; Clear(); }
        //---------------------------------------------------------------------------
        public void Clear      (             ) { OnDelay(false , 0); }
        //---------------------------------------------------------------------------
        public bool OnDelay    (bool SeqInput, double _SetTime) { SetTime = _SetTime; return OnDelay(SeqInput); }
        //---------------------------------------------------------------------------
        public bool OnDelay    (bool SeqInput)
        {
            double CurTick = _GetTickTime_us();
            if (SeqInput) {
                if (bScan)	CurTime++;
                else		CurTime += CurTick - PreTickTime;
                if (CurTime >= SetTime) { CurTime = SetTime; Out = true ; }
                else                                         Out = false;
                }
            else {
                CurTime = 0;
                Out = false;
                }
            PreTickTime = CurTick;
            return Out;
        }
    }
}
