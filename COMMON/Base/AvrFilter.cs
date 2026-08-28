using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine.Common.Base
{
    public class TAvrFilter
    {
        int      nDegree;
        double[] nValue;

        public TAvrFilter()
        {
            nDegree = 10;
            nValue  = Enumerable.Repeat<double>(0, nDegree).ToArray<double>();
        }
        public TAvrFilter(int iDegree)
        {
            if (iDegree <= 0) iDegree = 1;
            //
            nDegree = iDegree;
            nValue  = Enumerable.Repeat<double>(0, nDegree).ToArray<double>();
        }
        ~TAvrFilter() { }

        public void Clear()
        {
            Array.Clear(nValue, 0, nValue.Length);
        }
        public void SetDegree(int num)
        {
            //To remove the critical error
            if (num <= 0) num = 1;
            //set degree
            nDegree = num;
            Array.Resize(ref nValue, nDegree);
            //nValue = null;
            //nValue  = Enumerable.Repeat<double>(0, nDegree).ToArray<double>();

        }

    }
}
