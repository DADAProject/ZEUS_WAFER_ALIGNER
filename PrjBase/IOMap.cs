using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
//  [12/23/2021 JUNG]

    //INPUT ID.
    public enum EN_IN_ID : int
    {
        xNone = -1                     ,

        xWAFER_EXIST                   , //X0000//4//WAFER_EXIST
        xVACUUM_ON                     , //X0001//4//VACUUM_IS_ON
        X0002                          , //X0002//4//
        X0003                          , //X0003//4//               
        X0004                          , //X0004//4// 
        X0005                          , //X0005//4//
                                         
        X0006                          , //X0006//4// 
        X0007                          , //X0007//4//
        X0008                          , //X0008//
        X0009                          , //X0009//
        X000A                          , //X000A//
        X000B                          , //X000B//

        xSYS_FanAlarm01                , //X000C//Exhaust FAN STOP_1
        xSYS_FanAlarm02                , //X000D//Exhaust FAN STOP_2
        xSYS_FanAlarm03                , //X000E//Intake FAN STOP_1
        xSYS_FanAlarm04                , //X000F//Intake FAN STOP_2
        X0010                          , //X0010//
        X0011                          , //X0011//
        X0012                          , //X0012//
        X0013                          , //X0013//
        X0014                          , //X0014//
        X0015                          , //X0015//
        X0016                          , //X0016//
        X0017                          , //X0017//

        EndOfId //Input 총 갯수
    };

    //OUTPUT ID.
    public enum EN_OUT_ID : int
    {//UserSet - Output ID 정의
        yNone =       -1               ,
        
        Y1000                          ,//Y1000//4//
        Y1001                          ,//Y1001//4//
        Y1002                          ,//Y1002//4//
        Y1003                          ,//Y1003//4//
        Y1004                          ,//Y1004//4//
        Y1005                          ,//Y1005//4//
        Y1006                          ,//Y1006//4//
        Y1007                          ,//Y1007//4//
        Y1008                          ,//Y1008//4//

        yVACUUM_ON                     ,//Y1009//4//VACUUM_ON
        yVACUUM_PURGE                  ,//Y100A//4//VACUUM_PURGE
        yLightOn                       ,//Y100B//4//LIGHT_ON
        Y100C                          ,//Y100C//4//
        Y100D                          ,//Y100D//4//
        Y100E                          ,//Y100E//4//
        Y100F                          ,//Y100F//4//
        Y1010                          ,//Y1010//4//
        Y1011                          ,//Y1011//4//

        Y1012                          ,//Y1012//4//
        Y1013                          ,//Y1013//4//
        Y1014                          ,//Y1014//4//
        Y1015                          ,//Y1015//4//
        Y1016                          ,//Y1016//4//
        Y1017                          ,//Y1017//4//
        Y1018                          ,//Y1018//4//
        Y1019                          ,//Y1019//4//
        Y101A                          ,//Y101A//4//

        EndOfId //Output 총 갯수

    };

    //Analog Channel ID.
    //===========================================================================
    //UserSet - Analog Input ID 정의
    public enum EN_AI_CH : int
    {
        None = -1,

        EndOfAI
    };

    //UserSet - Analog Out ID 정의 
    public enum EN_AO_CH : int
    {
        Press = 0 ,

        EndOfAO
    };

}
