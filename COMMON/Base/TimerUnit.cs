using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/***************************************************************************/
/* Class: Timer Class                                                      */
/* Create:                                                                 */
/* Developer: JUNG                                                         */
/* Note:                                                                   */
/***************************************************************************/
namespace eMachine
{
    public class TimerUnit
    {
        //Var
        private DateTime settime    = new DateTime();
        private bool bReqStart;
        private bool bOut     ;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TimerUnit()
        {
            settime   = DateTime.Now;
            bReqStart = false;
            bOut      = false;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool Out => bOut;

        //------------------------------------------------------------------------
        public void Clear()
        {
            bOut    = false;
            settime = DateTime.Now;
            bReqStart = true; 
        }
        //------------------------------------------------------------------------
        public void Reset()
        {
            settime   = DateTime.Now;
            bReqStart = true        ;
            bOut      = false       ;
        }
        //------------------------------------------------------------------------
        public bool OnDelay(bool on, double time)
        {
            if (on)
            {
                if(bReqStart)
                {
                    bOut      = false;
                    bReqStart = false;
                    settime   = DateTime.Now;
                }
                return OnDelay(time);
            }
            else
            {
                settime   = DateTime.Now;
                bReqStart = true ;
                bOut      = false;
            }

            return bOut; 
        }
        //------------------------------------------------------------------------
        public bool OnDelay(double time)
        {
            if (DateTime.Now >= settime.AddMilliseconds(time))
            {
                bOut = true;
                return bOut; 
            }
            
            bOut = false;
            return bOut;

        }
        //------------------------------------------------------------------------
        public double GetDelayTime()
        {
            TimeSpan sp = new TimeSpan();
            sp = DateTime.Now - settime;
            return sp.TotalMilliseconds;
        }
    }

}
