using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    public class SwitchIO
    {
        EN_OUT_ID yON, yOff; 
        EN_IN_ID  xOn, xOff; 

        public SwitchIO(EN_OUT_ID on, EN_OUT_ID off)
        {
            yON  = on ;
            yOff = off;

            xOn  = EN_IN_ID.xNone;
            xOff = EN_IN_ID.xNone;
        }
        //------------------------------------------------------------------------
        public void SetInIO(EN_IN_ID on, EN_IN_ID off)
        {
            xOn  = on;
            xOff = off; 
        }
        //------------------------------------------------------------------------
        public bool ON()
        {
            bool r1 = cDEF.IO.sY(yON , true );
            bool r2 = cDEF.IO.sY(yOff, false);
            return r1 && r2; 
        }
        //------------------------------------------------------------------------
        public bool OFF()
        {
            bool r1 = cDEF.IO.sY(yON , false);
            bool r2 = cDEF.IO.sY(yOff, true);
            return r1 && r2;
        }
        //------------------------------------------------------------------------
        public void SetOnIO(EN_OUT_ID id)
        {
            yON = id;
        }
        //------------------------------------------------------------------------
        public void SetOffIO(EN_OUT_ID id)
        {
            yOff = id;
        }
        //------------------------------------------------------------------------
        public bool IsOn()
        {
            if (xOn == EN_IN_ID.xNone) return false; 
            return cDEF.IO.gX(xOn);
        }
        //------------------------------------------------------------------------
        public bool IsOff()
        {
            if (xOff == EN_IN_ID.xNone) return false;
            return cDEF.IO.gX(xOff);
        }

    }
}
