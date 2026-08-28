using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    /***************************************************************************/
    /* Class: TQueueCls                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    class TQueueCls<T>
    {
            public int iFront    ;
            public int iRear     ;
            public int iSaveCount;

            //Buffer.
            T[] pBuff;

            int iMaxsize;
            //--------------------------------------------------------------------------
            //Constructor & Destroyer.
            public TQueueCls(int Size) {
                //Initialize.
                iFront = iRear = 1;
                iSaveCount = 0;

                //To remove the critical error.
                if (Size < 10    ) iMaxsize = 10  ;
                else               iMaxsize = Size;

                //Create queue as type.
                pBuff = new T[iMaxsize * 2];
            }
            ~TQueueCls() { }
            //--------------------------------------------------------------------------
            //Check queue state.
            public bool IsFull ()
            {
                if (iSaveCount == (iMaxsize - 1)) return true;
                return false;
            }    
            //--------------------------------------------------------------------------
            public bool IsEmpty()
            {
                if (iFront == iRear) return true;
                return false;
            }
            //--------------------------------------------------------------------------
            //Clear queue.
            public void Clear  (        )
            {
                //Reset position.
                iFront = iRear = 1;
                iSaveCount     = 0;

                //Memory clear.
                //pBuff = null;
            }
            //--------------------------------------------------------------------------
            //Push data.
            public bool Push(T  Data)
            {
                if (iMaxsize <= 0) return false;
    
                //Cal rear point.
                int k = (iRear + 1) % iMaxsize;

                //Check full & Insert data to queue.
                if (iFront == k) return false;
                else {
                    pBuff[iRear = k] = Data;
                    iSaveCount++;
                    }

                //Succesful.
                return true;
            }    
            //--------------------------------------------------------------------------
            public bool Pop (ref T  Data)
            {
                //Check empty & Delete data in queue.
                if (iFront == iRear) return false;

                //Output data
                iFront++;
                Data = pBuff[iFront %= iMaxsize];
                iSaveCount--;

                //Succesful
                return true;
            }
            //--------------------------------------------------------------------------
            public int GetCount() { return iSaveCount; }
            public int GetFront() { return iFront    ; }
            public int GetRear () { return iRear     ; }
    };

