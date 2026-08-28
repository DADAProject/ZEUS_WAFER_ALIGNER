using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    ///Define Motor ID                            
    //---------------------------------------------------------------------------
    //UserSet  - Motor ID 정의

    public enum  EN_MOTR_ID  : int 
    {
        None   =  -1 ,                               //MOTOR TYPE	SERVO MODEL	용량(W)	MAKER	DRIVER MODEL	DRIVER MAKER	BRAKE

        WAT_X   =   0 , // Wafer Align Table X-Axis 
        WAT_Y   =   1 , // Wafer Align Table Y-Axis 
        WAT_T   =   2 , // Wafer Align Table T-Axis 

        EndOfId
    }
    
}
