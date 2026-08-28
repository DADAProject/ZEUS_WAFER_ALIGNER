using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    //Define Actuator Id
    //---------------------------------------------------------------------------
    //UserSet  - Actuator ID 정의
    public enum EN_ACTR_ID : int
    {
        None               =  -1   , //NO//MAN//xFwd_//xBWd_//yFwd_//yBwd_//Comment
        aLPM_MGZLock_L1_L  =   0   , //00//290//X0040//X0041//Y0269//Y026A//LPM1 Base Lock - Left
        aLPM_MGZLock_L1_R          , //01//291//X0040//X0041//Y0269//Y026A//LPM1 Base Lock - Right
        aLPM_MGZLoad_L1            , //02//292//X0047//X0048//Y026B//Y026C//LPM1 Base Load
        aLPM_DoorOpen_L1           , //03//293//X004F//X0050//Y026D//YFFFF//LPM1 Door open
        aLPM_DoorLock_L1_L         , //04//294//X0051//X0052//Y026E//Y026F//LPM1 Door Lock - Left
        aLPM_DoorLock_L1_R         , //05//295//X0051//X0052//Y026E//Y026F//LPM1 Door Lock - Right

        aLPM_MGZLock_L2_L          , //06//296//X0053//X0054//Y026E//Y026F//LPM2 Base Lock - Left
        aLPM_MGZLock_L2_R          , //07//297//X0053//X0054//Y026E//Y026F//LPM2 Base Lock - Right
        aLPM_MGZLoad_L2            , //08//298//X0058//X0059//Y0277//Y0278//LPM2 Base Load
        aLPM_DoorOpen_L2           , //09//299//X006E//X006F//Y0288//YFFFF//LPM2 Door open
        aLPM_DoorLock_L2_L         , //10//300//X0063//X0064//Y0289//YFFFF//LPM2 Door Lock - Left
        aLPM_DoorLock_L2_R         , //11//301//X0063//X0064//Y0289//YFFFF//LPM2 Door Lock - Right

        aWAT_GuideFront            , //12//302//X0065//X0066//Y0289//YFFFF//Wafer Align Guide - Front
        aWAT_GuideLeft             , //13//303//X0071//X0072//Y028F//YFFFF//Wafer Align Guide - Left
        aWAT_GuideRear             , //14//304//X0073//XFFFF//Y0290//YFFFF//Wafer Align Guide - Rear
        aWAT_GuideRight            , //15//305//X0075//XFFFF//Y0290//YFFFF//Wafer Align Guide - Right
                                       
        EndOfId                        
    };                                 
                                       
}
