using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    /***************************************************************************/
    /* Class: TEdgeUnit                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TEdgeUnit
    {
        //private:   /* Member Var.             */
        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool m_bStat;
        bool m_bInit;

        //private:   /* Member Func.            */
        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

       //public:    /* Direct Accessable Var.  */
        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Var.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //public:    /* Direct Accessable Func. */
        //생성자 & 소멸자. (Constructor & Destructor)
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TEdgeUnit() { Init(); }
        ~TEdgeUnit() {         }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init(         ) { m_bStat = false; m_bInit = false;}
        public void Init(bool Crnt) { m_bStat = Crnt ; m_bInit = true ;}

        //Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsChange(bool Crnt)
        {
            if (!m_bInit        ) { m_bInit = true ; m_bStat = Crnt ; return false ;}
            if ( m_bStat != Crnt) {
                 m_bStat  = Crnt;
                 return true;
                 }
            return false;
        }
        public bool IsRising (bool  Crnt                                  ) { bool isChng = (IsChange(Crnt) &&  Crnt);                                      return isChng; }
        public bool IsFalling(bool  Crnt                                  ) { bool isChng = (IsChange(Crnt) && !Crnt);                                      return isChng; }
        public bool IsRising (ref bool Crnt ,                 bool isClear) { bool isChng = (IsChange(Crnt) &&  Crnt); if (isChng && isClear) Crnt = !Crnt; return isChng; }
        public bool IsFalling(ref bool Crnt ,                 bool isClear) { bool isChng = (IsChange(Crnt) && !Crnt); if (isChng && isClear) Crnt = !Crnt; return isChng; }
        public bool IsRising (ref bool Crnt , ref bool Item , bool isClear) { bool isChng = (IsChange(Crnt) &&  Crnt); if (isChng && isClear) Item = false; return isChng; }
        public bool IsFalling(ref bool Crnt , ref bool Item , bool isClear) { bool isChng = (IsChange(Crnt) && !Crnt); if (isChng && isClear) Item = true ; return isChng; }
    }
