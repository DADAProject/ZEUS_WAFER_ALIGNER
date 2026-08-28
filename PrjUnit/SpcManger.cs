using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace eMachine
{

    /***************************************************************************/
    /* Class: TSpcManger                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    //SPC Shift
    //===========================================================================
    public enum EN_DAY_SHIFT
    {
        All,
        ShiftGY,
        ShiftDY,
        ShiftSW
    };


    /***************************************************************************/
    /* Structures                                                              */
    /***************************************************************************/
    //Machine Efficiency.
    //===========================================================================
    public class TSPC_EFF {
        public double dtInit      ;
        public double dtWarning   ;
        public double dtMCError   ;
        public double dtHMError   ;
        public double dtMLError   ;
        public double dtMDError   ;
        public double dtError     ;
        public double dtRunning   ;
        public double dtRunWarn   ;
        public double dtStop      ;
        public double dtMaint     ;
        public double dtIdle      ;
        public int    iJamCnt     ;

        public TSPC_EFF()
        {
            ResetData();
        }
        public void ResetData()
        {
            dtInit         = 0; 
            dtWarning      = 0;
            dtMCError      = 0;
            dtHMError      = 0;
            dtMLError      = 0;
            dtMDError      = 0;
            dtError        = 0;
            dtRunning      = 0;
            dtRunWarn      = 0;
            dtStop         = 0;
            dtMaint        = 0;
            dtIdle         = 0;
            iJamCnt        = 0;
        }
    };


    //Daily Data.
    //UserSet - ADD DAILY DATA
    //===========================================================================
    public class TDAILY_DATA {
        public int       iWorkQty            ;
        public int       iJamQty             ;
        public int       iAlignCnt           ;

        public double    dRunTime            ;
        public double    dErrorTime          ;
        public double    dDownTime           ;
        public double    dIdleTime           ;

        public TDAILY_DATA()
        {
            ResetData();
        }
        public void ResetData()
        {
            iWorkQty    = 0;
            iJamQty     = 0;
            dRunTime    = 0;
            dErrorTime  = 0;
            dDownTime   = 0;
            dIdleTime   = 0;

            iAlignCnt   = 0;
        }
    };

    public class TSpcManger
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        public  double      m_dSeqSrtTime     ;
        public  double      m_dDrngJamTime    ;
        public  double      m_dSeqEndTime     ;
        public  double      m_dDrngSeqTime    ;
        public  DateTime    m_tDayChangeTime  ; //하루는 전날 pm10:00 ~ 금일 pm10:00
        public  DateTime    m_tClearJamTime   ; //Jam All Clear Time.
        public  int         m_iLastJamNo      ;
        public  bool        m_bDayChanged     ;
        //private EN_SEQ_STAT m_LastStat        ;


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */

        //int    m_iDBSavePeriod    ;
        //double m_dJamCountInterval;
        //String m_sDBForder        ;
        //int    m_iDBClearPeriod   ;
        //String m_sDBClsDate       ;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TSqLiteDB   dbJAM       = new TSqLiteDB  ();
        public TSqLiteDB   dbPROD      = new TSqLiteDB  ();
        public TSPC_EFF    SPC_EFF     = new TSPC_EFF   ();
        public TDAILY_DATA DAILY_DATA  = new TDAILY_DATA();


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSpcManger()
        {
            m_tDayChangeTime = DateTime.Parse("23:00:00");
            m_bDayChanged    = false;
            
            //
            m_dSeqSrtTime  = cDEF.TICK._GetTickTime();
            m_dSeqEndTime  = cDEF.TICK._GetTickTime();
            m_dDrngJamTime = m_dDrngSeqTime = 0.0;
        }
        
        ~TSpcManger() { }

        //--------------------------------------------------------------------------
        public void InitDB()
        {//UserSet - Database Field 정의  
        
            string sPathDB = Application.StartupPath + "\\Database";

 	        dbJAM.AddField      ("Day"             , EN_FIELD_TYPE.dftDate        ,  0 , true);
	        dbJAM.AddField      ("Month"           , EN_FIELD_TYPE.dftString      ,  7 , true);
	        dbJAM.AddField      ("ErrNo"           , EN_FIELD_TYPE.dftString      ,  8 , true);
          //dbJAM.AddField      ("LotNo"           , EN_FIELD_TYPE.dftString      , 20 , true);
          //dbJAM.AddField      ("Result"          , EN_FIELD_TYPE.dftInteger     , 0  , true);
	        dbJAM.AddField      ("StartTime"       , EN_FIELD_TYPE.dftTimeStamp   , 0        );
	        dbJAM.AddField      ("EndTime"         , EN_FIELD_TYPE.dftTimeStamp   , 0        );
	        dbJAM.AddField      ("Contents"        , EN_FIELD_TYPE.dftString      ,100       );
	        dbJAM.SetTable      ("JAMLIST"                                                   );
            dbJAM.Init          (sPathDB, "JamDB"                                            );

            //
 	        dbPROD.AddField     ("Day"             , EN_FIELD_TYPE.dftDate         ,0  , true);
	        dbPROD.AddField     ("Month"           , EN_FIELD_TYPE.dftString      , 7  , true);
	        dbPROD.AddField     ("WorkQty"         , EN_FIELD_TYPE.dftInteger                );
	        dbPROD.SetTable     ("PRODLIST"                                                  );
            dbPROD.Init         (sPathDB, "ProdDB"                                           );
        }
        
        /***************************************************************************/
        /* Time Cal.                                                               */
        /***************************************************************************/
        public double ConvTimeDateTimeToSec(DateTime Time)
        {//Time.Ticks usec 
            return (Time.Ticks / 1000.0); //날짜를 초단위로 변경한다.
        }

        //---------------------------------------------------------------------------
        public string ConvTimeSecToStr(double second)
        {
            int Csec1, Csec2;
            string temp;

            Csec1   =  (int)second / 3600 ; //H
            second -= (Csec1  * 3600)     ; 
            Csec2   =  (int)second / 60   ; //M
            second -= (Csec2  * 60  )     ; //S

            temp = string.Format("{0:00}:{1:00}:{2:00}" , Csec1 , Csec2 , (int)second);
            return temp;
        }

        //---------------------------------------------------------------------------
        public string ConvTimeTickToStr(double tickCnt)
        {
            int Hour, Minute , Sec;
            string temp;

  
            Hour     = (int)tickCnt / 3600000 ;
            tickCnt -= (Hour   * 3600000)     ;
            Minute   = (int)tickCnt / 60000   ;
            tickCnt -= (Minute   * 60000  )   ;
            Sec      = (int)tickCnt / 1000    ;
            tickCnt -= (Sec   * 1000   )      ;

            temp = string.Format("{0:00}:{1:00}:{2:00}" , Hour , Minute , Sec);
            return temp;
        }
        //---------------------------------------------------------------------------
        public string  GetAvrScanTime(int iCnt, double dTotTime)
        {
            double tAvrScanTime = 0.0;
            if(iCnt>0)   tAvrScanTime = dTotTime/iCnt;
            else         tAvrScanTime = dTotTime;

            return ConvTimeTickToStr(tAvrScanTime);
        }
        //---------------------------------------------------------------------------
        public  DateTime ChangeDateTime (DateTime SetDate, bool First , bool Section)
        {
            //Local Var.
            DateTime  tDate     = DateTime.Now;
            
            tDate = SetDate.Date;
            

  	        if (First) { //시작 날짜 시간
                if (Section) tDate = tDate.AddDays(1);  
                else 
                {
                    if (DateTime.Now < m_tDayChangeTime) tDate = tDate.AddDays(-1); //오늘 22:00 이전은 어제 22:00 부터
                }
            }
            else 
            { //종료 날짜 시간
                if(!Section && DateTime.Now > m_tDayChangeTime)  tDate = tDate.AddDays(1);  
            }

            tDate = tDate.AddHours(m_tDayChangeTime.Hour);
            return tDate;

        }

        //---------------------------------------------------------------------------
        public DateTime ChangeDate(DateTime SetDate)
        {
            DateTime tDate = DateTime.Now;
            
            if (DateTime.Now < m_tDayChangeTime) 
            {
               return tDate = DateTime.Now;    //설정 시간 이전은 오늘
            }
            else 
            {
               return tDate.AddDays(1);
            }
            
            //return tDate;
        }
        //----------------------------------------------------------------------------
        public EN_DAY_SHIFT GetShift(DateTime tDateTime)
        {
            //Local Var.
 
            DateTime CTime ; CTime  = DateTime.Now;
            DateTime Time22; Time22 = DateTime.  Parse("22:00:00");
            DateTime Time06; Time06 = DateTime.  Parse("06:00:00");
            DateTime Time14; Time14 = DateTime.  Parse("14:00:00");

            //Cal. Shift.
            if      ((CTime >= Time22) || (CTime < Time06)) return EN_DAY_SHIFT.ShiftGY; //GY
            else if ((CTime >= Time06) && (CTime < Time14)) return EN_DAY_SHIFT.ShiftDY; //DY
            else if ((CTime >= Time14) && (CTime < Time22)) return EN_DAY_SHIFT.ShiftSW; //SW
            
            return EN_DAY_SHIFT.All;
        }
        //---------------------------------------------------------------------------
        public int CalUPH(String LotRunTime , int WorkCnt)
        {
            //Local Var.R
            double WorkTime =  DateTime.Parse(LotRunTime).Ticks;
            double Hour     =  WorkTime / 60.0 / 60.0 / 1000.0; //시 / 분 / 초 / usec

            //Check Error.
            if (WorkCnt <= 0 || Hour <= 0) return 0;

            //Cal UPH.
   	        return (int)((int)WorkCnt / Hour);
        }
        //---------------------------------------------------------------------------
        public int CalUPH(TimeSpan sp, int WorkCnt)
        {
            //Local Var.R
            double Hour = sp.TotalMilliseconds / 60.0 / 60.0 / 1000.0; //시 / 분 / 초 / usec

            //Check Error.
            if (WorkCnt <= 0 || Hour <= 0) return 0;

            //Cal UPH.
            return (int)((int)WorkCnt / Hour);
        }
        //---------------------------------------------------------------------------
        public int CalUPEH(double LotStrtTime , double LotEndTime , int WorkCnt)
        {
            //Local Var.
            //String Temp;            

            double WorkTime = LotEndTime - LotStrtTime;
            double Hour     = WorkTime / 60.0 / 60.0 / 1000.0; //시 / 분 / 초 / usec

            //Check Error.
            if (WorkCnt <= 0 || Hour <= 0) return 0;
            

            //Cal UPH.
   	        return (int)(WorkCnt / Hour);
        }
        //---------------------------------------------------------------------------
        public int CalUPEH(TimeSpan sp, int WorkCnt)
        {
            //Local Var.
            //String Temp;            

            double WorkTime = sp.TotalMilliseconds;
            double Hour = WorkTime / 60.0 / 60.0 / 1000.0; //시 / 분 / 초 / usec

            //Check Error.
            if (WorkCnt <= 0 || Hour <= 0) return 0;

            //Cal UPH.
            return (int)(WorkCnt / Hour);
        }

        public double CalUPH(double dtSecScan, int iCnt)
        {
            //Local Var.
            if (dtSecScan <= 0) return 0.0;
            if (iCnt      <= 0) return 0.0;

            return (3600.0 * (double)iCnt) / ((double)dtSecScan);
        }

        //---------------------------------------------------------------------------
        public string CalMTBI(double RunTime , int JamCnt)
        {
            //Local Var.
            double dMTBI = 0.0 ;
            //string strMTBI = "";
            
            //Check Error.
            if (RunTime <= 0) return "00:00:00";
            if (JamCnt  <= 0) JamCnt = 1  ;

            //Cal UPH.
   	        dMTBI = RunTime / JamCnt;

            //return ConvTimeSecToStr(dMTBI);            
            return ConvTimeTickToStr(dMTBI);
        }
        //---------------------------------------------------------------------------
        public bool IsChangedDay(bool IsChkOnly = false)
        {
            //Local Var.
            int Hour1;
            int Hour2;

            //Set Time.
            Hour1 = m_tDayChangeTime.Hour;
            Hour2 = DateTime.Now.Hour;

            //Check Changing Day.
	        if (Hour1 <= Hour2) 
            {
                if (!IsChkOnly) 
                {
    	            if (!m_bDayChanged) 
                    {
             	        m_bDayChanged = true;
                        return true;
                    }
                }
                else return true;
            }
            else 
            {
    	        m_bDayChanged = false;
    	        return false;
            }
        
            return false;
        }
        //---------------------------------------------------------------------------
        public DateTime ChangeDay(DateTime CurrDate)
        {
            
            if(CurrDate.TimeOfDay > m_tDayChangeTime.TimeOfDay) {//현재 시간이 설정 시간보다 큰 경우
               if(!m_bDayChanged) return  CurrDate.AddDays(1); //설정시간 이후는 내일
               }
            else {//현재 시간이 설정 시간보다 작은 경우
               if(m_bDayChanged)  return CurrDate.AddDays(-1);  //설정시간 이전은 어제
               }
           
           return CurrDate;
        }
        //--------------------------------------------------------------------------
        public void InsDbJam(int iErrNo) 
        {
            if(iErrNo<0 || iErrNo>=vDEF.MAX_ERR) return;
            
            //Check Skip Error
            if (!cDEF.EPU.CheckWriteErr(iErrNo)) return;
            
            //Check Error No.
            //if ( IsChkSameErr     (ErrNo)) return;
            //if ( cDEF.FM.SysOptn .iSkipErrLog == 1) return; //iSkipErrLog 추가c

            //Server 전송하지 않는 Error는 DB 저장되지 않음.
            //if (!cDEF.EPU[iErrNo].m_bOnAtRun                          ) return;
            //if ( cDEF.EPU[iErrNo].m_iGrade != (int)EN_ERR_GRADE.Error ) return;
            
            DateTime  STime   = cDEF.EPU.GetSetTimeDB(iErrNo);

            string    sDate    = string.Format("{0:yy/MM/dd}", STime       );
            string    sMonth   = string.Format("{0:yy/MM}"   , STime       );
            string    sStart   = string.Format("{0:HH:mm:ss}", STime       );
            string    sEnd     = string.Format("{0:HH:mm:ss}", DateTime.Now);
            string    sErrNo   = string.Format("E{0:0000}"   , iErrNo      );
            string    sErrName = cDEF.EPU.GetName(iErrNo);
          //string    sResult  = cDEF.SEQ.WAT._VisnResult.IsCompleted? "OK" : "FAIL";

            dbJAM.ClearSqlString();

	        dbJAM.SetData(sDate   );          
	        dbJAM.SetData(sMonth  );   
	        dbJAM.SetData(sErrNo  );   
           //dbJAM.SetData(sLotNo  );
          //dbJAM.SetData(sResult );
	        dbJAM.SetData(sStart  );   
	        dbJAM.SetData(sEnd    );   
	        dbJAM.SetData(sErrName);  
            dbJAM.InsData(); 

            SPC_EFF   .iJamCnt ++;
            DAILY_DATA.iJamQty ++;
        }
        //--------------------------------------------------------------------------
        public void InsDbProd() 
        {
            /*
                //Local Var.
                //Local Var.
                AnsiString sWorkMode , sShift;
                AnsiString sTemp             ;
                TDateTime  tTrayIn , tReqTrayInTime   , tTrayInTime ;
                TDateTime  tTrayOut, tReqTrayOutTime  , tTrayOutTime;
                TDateTime  tReelIn , tReqReelInTime   , tReelInTime ;
                TDateTime  tReelOut, tReqReelOutTime  , tReelOutTime;
                TDateTime  tRunTime;

                int        iUPEH;
                unsigned short iHour, iMin, iSec, iMSec;


                double     dTrayInTime ;
                double     dTrayOutTime;
                double     dReelInTime ;
                double     dReelOutTime;
                double     dRunTime    ;
                double     dErrorTime  ;
                int        iRunDatNo   ;
                double     iStopTime   ;
                int        iFailQty[MAX_VISN_ID];

                if(iReel < 0 || iReel>=MAX_USE_TNR) iReel = 0;

                //KBH 160905
                int iWorkMode = (LOT.IsLotOpen()) ? LOT.GetWorkMode() : FM.ProjBase.iWorkMode;

                switch (iMode) {
                   case prdNormal:        if(LOT.GetWorkMode() == wmWaferToReel) sWorkMode = "Wafer To Reel";
                                     else if(LOT.GetWorkMode() == wmWaferToTray) sWorkMode = "Wafer To Tray";
                                     else if(LOT.GetWorkMode() == wmTrayToReel ) sWorkMode = "Tray To Reel" ;
                                     else if(LOT.GetWorkMode() == wmTrayToTray ) sWorkMode = "Tray To Tray" ;
                                     else if(LOT.GetWorkMode() == wmReelToTray ) sWorkMode = "Reel To Tray" ;
                                     else if(LOT.GetWorkMode() == wmReelToReel ) sWorkMode = "Reel To Reel" ;
                                     break;
                   case prdCancel  : sWorkMode = "CANCEL"    ; break;
                   case prdNextReel: sWorkMode = "NEXT REEL" ; break;
                   default          : sWorkMode = ""         ; break;
                   }

                if(LOT.LStrc.m_LotInfo.bICNConversion) sWorkMode = "컨버젼"  ;
                if(LOT.LStrc.m_LotInfo.bSkipWrongQty ) sWorkMode = "수량오차";
                if(LOT.LStrc.m_LotInfo.bForceICNCode ) sWorkMode = "강제배출";

                switch (GetShift(LOT.LStrc.m_LotTime.dLotEndTime)) {
                    case   1 : sShift = "G.Y"; break;
                    case   2 : sShift = "DAY"; break;
                    case   3 : sShift = "S.W"; break;
                    default  : sShift = ""   ; break;
                    }

                int iMOQ_Qty         = LOT.LStrc      .m_WorkQty.iReelMOQ           ;
                if(iMOQ_Qty<=0) iMOQ_Qty = 1; //
                int iPocketPlaceQty  = LOT.GetAReelPlceQty()                        ;
                int iReelQty         = iPocketPlaceQty/iMOQ_Qty                     ;
                int iLoadQty         = LOT.LStrc      .m_LotInfo.iInQty    + (iReelQty*iMOQ_Qty);
                int iGoodQty         = LOT.GetAReelPlceQty()                        ;
                int iJamQty          = 0;


                for(int i=0;i<MAX_VISN_ID;i++) {
                    iFailQty[i] = LOT.LStrc      .m_WorkQty.iStkIRej   [i ];
                    }

                //Run Time.
                if(tTrayInTime   > tTrayOutTime)   { tTrayInTime     -= 1; tRunTime = tTrayInTime     - tTrayOutTime;    }
                else                               {                       tRunTime = tTrayOutTime    - tTrayInTime;     }

                //Tray In Time
                if(tReqTrayInTime > tTrayInTime)   { tReqTrayInTime  -= 1; tTrayIn  = tReqTrayInTime  - tTrayInTime;     }
                else                               {                       tTrayIn  = tTrayInTime     - tReqTrayInTime;  }

                //Tray Out Time
                if(tReqTrayOutTime > tTrayOutTime) { tReqTrayOutTime -= 1; tTrayOut = tReqTrayOutTime - tTrayOutTime;    }
                else                               {                       tTrayOut = tTrayOutTime    - tReqTrayOutTime; }

                //Reel In Time
                if(tReqReelInTime > tReelInTime  ) { tReqReelInTime -= 1;  tReelIn  = tReqReelInTime  - tReelInTime;     }
                else                               {                       tReelIn  = tReelInTime     - tReqReelInTime;  }

                //Reel Out Time
                if(tReqReelOutTime > tReelOutTime) { tReqReelOutTime -= 1; tReelOut = tReqReelOutTime - tReelOutTime;    }
                else                               {                       tReelOut = tReelOutTime    - tReqReelOutTime; }



                tRunTime .DecodeTime(&iHour, &iMin, &iSec, &iMSec); dRunTime     = iHour * 3600000 + iMin * 60000 + iSec*1000 + iMSec;
                tTrayIn  .DecodeTime(&iHour, &iMin, &iSec, &iMSec); dTrayInTime  = iHour * 3600000 + iMin * 60000 + iSec*1000 + iMSec;
                tTrayOut .DecodeTime(&iHour, &iMin, &iSec, &iMSec); dTrayOutTime = iHour * 3600000 + iMin * 60000 + iSec*1000 + iMSec;
                tReelIn  .DecodeTime(&iHour, &iMin, &iSec, &iMSec); dReelInTime  = iHour * 3600000 + iMin * 60000 + iSec*1000 + iMSec;
                tReelOut .DecodeTime(&iHour, &iMin, &iSec, &iMSec); dReelOutTime = iHour * 3600000 + iMin * 60000 + iSec*1000 + iMSec;

                //Cal DayData
                DayData.iLoadQty        += iLoadQty       ;
                DayData.iGoodQty        += iPocketPlaceQty;
                for(int i=0;i<MAX_VISN_ID;i++) {
                    DayData.iRejQty[i ] += LOT.LStrc      .m_WorkQty.iStkIRej   [i ];
                    }

                DayData.iReelQty        += iReelQty       ;
                //DayData.iJamQty         += iJamQty        ;
                DayData.dRunTime        += dRunTime       ;

                if(iFailQty[viOTI ]>0) DayData.iReelICNQty     ++;
                if(iFailQty[viOTI2]>0) DayData.iReelICNQty     ++;
                if(iFailQty[viOTI3]>0) DayData.iReelICNQty     ++;
                if(iFailQty[viOTI4]>0) DayData.iReelICNQty     ++;


                //TT(160819)
                TDateTime  tStartTime = LOT.LStrc.m_LotTime.dLotStrtTime;
                TDateTime  tEndTime   = (bEnd) ? LOT.LStrc.m_LotTime.dLotEndTime :  LOT.LStrc.m_LotTime.dSplitEndTime;

                double     dCalWorkTime  = ConvTime(tEndTime - tStartTime);
                double     dCalRunTime   = LOT.LStrc.m_LotTime.dLotRunTime/1000;
                double     dCalJamTime   = (LOT.LStrc.m_LotTime.dLotMCJamTime + LOT.LStrc.m_LotTime.dLotHMJamTime + LOT.LStrc.m_LotTime.dLotMLJamTime + LOT.LStrc.m_LotTime.dLotMDJamTime )/1000;
                double     dCalStopTime  = dCalWorkTime - dCalRunTime - dCalJamTime; //LOT.LStrc.m_LotTime.dLotStopTime/1000;


                //Get UPEH
                iUPEH = CalUPH(dCalWorkTime , LOT.GetAReelPlceQty()    );

                //Write DB.
                try {
                    PrdTable.Open   ();
                    PrdTable.Append ();

                    PrdTable.AddData("E_Day"          , ChangeDate(Now())                          );
                    PrdTable.AddData("E_Month"        , Now()                                      );
                    PrdTable.AddData("WorkQty"        , iLoadQty                                   );
                    PrdTable.AddData("GoodQty"        , iGoodQty                                   );
                    PrdTable.AddData("PocketPlaceOty" , iPocketPlaceQty                            );
                    PrdTable.AddData("Reel1PlaceQty"  , LOT.GetReelPlceQty(0));
                    PrdTable.AddData("Reel2PlaceQty"  , LOT.GetReelPlceQty(1));
                    PrdTable.AddData("Reel3PlaceQty"  , LOT.GetReelPlceQty(2));
                    PrdTable.AddData("Reel4PlaceQty"  , LOT.GetReelPlceQty(3));
                    PrdTable.AddData("Reel1SealQty"   , LOT.GetReelCompQty(0));
                    PrdTable.AddData("Reel2SealQty"   , LOT.GetReelCompQty(1));
                    PrdTable.AddData("Reel3SealQty"   , LOT.GetReelCompQty(2));
                    PrdTable.AddData("Reel4SealQty"   , LOT.GetReelCompQty(3));
                    PrdTable.AddData("Reel1PostQty"   , LOT.LStrc.m_WorkQty.iPostVisionCnt[0]);
                    PrdTable.AddData("Reel2PostQty"   , LOT.LStrc.m_WorkQty.iPostVisionCnt[1]);
                    PrdTable.AddData("Reel3PostQty"   , LOT.LStrc.m_WorkQty.iPostVisionCnt[2]);
                    PrdTable.AddData("Reel4PostQty"   , LOT.LStrc.m_WorkQty.iPostVisionCnt[3]);
                    PrdTable.AddData("Reel1OtiQty"    , LOT.LStrc.m_WorkQty.iOtiVisionCnt2[0]);
                    PrdTable.AddData("Reel2OtiQty"    , LOT.LStrc.m_WorkQty.iOtiVisionCnt2[1]);
                    PrdTable.AddData("Reel3OtiQty"    , LOT.LStrc.m_WorkQty.iOtiVisionCnt2[2]);
                    PrdTable.AddData("Reel4OtiQty"    , LOT.LStrc.m_WorkQty.iOtiVisionCnt2[3]);
                    PrdTable.AddData("FailQtyHEI"     , iFailQty[viHEI ]                     );
                    PrdTable.AddData("FailQtyTOP"     , iFailQty[viTOP ]                     );
                    PrdTable.AddData("FailQtySD"      , iFailQty[viSD  ]                     );
                    PrdTable.AddData("FailQtyBTM"     , iFailQty[viBTM ]                     );
                    PrdTable.AddData("ReelQty"        , iReelQty                             );
                    PrdTable.AddData("JamQty"         , iJamQty                              );
                    PrdTable.AddData("RunTime"        , (!bEnd)? 0.0 : dCalRunTime           );
                    PrdTable.AddData("ErrorTime"      , (!bEnd)? 0.0 : dCalJamTime           );
                    PrdTable.AddData("DownTime"       , (!bEnd)? 0.0 : dCalStopTime          );
                    PrdTable.Post ();
                    PrdTable.Close();
                }
                catch (Exception &err)
                {
                    //Var.
                    AnsiString Path    ;
                    AnsiString FileName;
                    FILE       *fout   ;

                    //Set Path.
                    Path = ExtractFilePath(Application -> ExeName) + "Exception\\";
                    if (!DirectoryExists(Path)) ForceDirectories(Path);
                    FileName = Path + "DBException1.Log";

                    //Write Exception Log.
                    AnsiString eMsg = err.Message + Now().FormatString("[yyyymmdd_hhmmss]") + AnsiString("\n");

                    if ( (fout = fopen(FileName.c_str(), "a+")) != NULL) {
                        fputs (eMsg.c_str() , fout);
                        fclose(                      fout);
                        }
                }  
            */                   
        }                                           
        //--------------------------------------------------------------------------
        public void Update(EN_SEQ_STAT iSeqStat) 
        {

            //if(SEQ._bRun) return;

            //Set Start.
            m_dSeqSrtTime = cDEF.TICK._GetTickTime();

            //Get Drng Seq Time.
            m_dDrngSeqTime = m_dSeqSrtTime - m_dSeqEndTime;


            EN_ERR_KIND iLastErrKind = (EN_ERR_KIND)cDEF.EPU.GetKind(cDEF.EPU._iLastErr);
            //Lot별 시간
            switch (iSeqStat)
            {
                case EN_SEQ_STAT.Init   : cDEF.LOT.AddLotStopTime(m_dDrngSeqTime); break;
                case EN_SEQ_STAT.Warning: cDEF.LOT.AddLotStopTime(m_dDrngSeqTime); break;
                case EN_SEQ_STAT.Error  : 
                             if (iLastErrKind == EN_ERR_KIND.Machine ) cDEF.LOT.AddLotMCJamTime(m_dDrngSeqTime);
                        else if (iLastErrKind == EN_ERR_KIND.Human   ) cDEF.LOT.AddLotHMJamTime(m_dDrngSeqTime);
                        else if (iLastErrKind == EN_ERR_KIND.Material) cDEF.LOT.AddLotMLJamTime(m_dDrngSeqTime);
                        else                                           cDEF.LOT.AddLotMDJamTime(m_dDrngSeqTime);
                    break;
                case EN_SEQ_STAT.Running: cDEF.LOT.AddLotRunTime (m_dDrngSeqTime ); break;
                case EN_SEQ_STAT.RunWarn: cDEF.LOT.AddLotRunTime (m_dDrngSeqTime ); break;
                case EN_SEQ_STAT.Stop   : 
                    if (!cDEF.LOT._bLotOpen && cDEF.LOT._bLotEnded)
                        cDEF.LOT.AddLotIdleTime(m_dDrngSeqTime);
                    else
                        cDEF.LOT.AddLotStopTime(m_dDrngSeqTime); 
                    break;
                    
                default                 : cDEF.LOT.AddLotIdleTime(m_dDrngSeqTime); break;
            }

            //Clear. //86400000 == 24:00:00

            //Day Seq별 시간 증가.
            switch (iSeqStat)
            {
                case EN_SEQ_STAT.Running: DAILY_DATA.dRunTime  += m_dDrngSeqTime; break;
                case EN_SEQ_STAT.Init   : DAILY_DATA.dDownTime += m_dDrngSeqTime; break;
                case EN_SEQ_STAT.Warning: DAILY_DATA.dDownTime += m_dDrngSeqTime; break;
                case EN_SEQ_STAT.Error  : if (cDEF.EPU._bHasErr) DAILY_DATA.dErrorTime += m_dDrngSeqTime;
                                          else                   DAILY_DATA.dDownTime  += m_dDrngSeqTime;
                                          break;
                case EN_SEQ_STAT.Stop   : if(cDEF.EPU._bHasErr ) DAILY_DATA.dErrorTime += m_dDrngSeqTime;                                   
                                           else                  DAILY_DATA.dDownTime  += m_dDrngSeqTime;
                                           break;
                case EN_SEQ_STAT.Idle: if (cDEF.EPU._bHasErr   ) DAILY_DATA.dErrorTime += m_dDrngSeqTime;
                                           else                  DAILY_DATA.dIdleTime  += m_dDrngSeqTime;
                                           break;
 
            }

            //Lot Seq별 시간 증가.

            //Set End Time.
            m_dSeqEndTime = cDEF.TICK._GetTickTime();

            //Check Changing Time.
            if (!IsChangedDay()) return;

            //WriteDBDEf(     ); 
            string stemp = string.Format($"[Day Change] MTBI({CalMTBI(DAILY_DATA.dRunTime, DAILY_DATA.iJamQty)}), WORK QTY({DAILY_DATA.iWorkQty}), ERROR QTY({DAILY_DATA.iJamQty})");
            cDEF.LOG.Trace(stemp);
            stemp = string.Format($"                > Run Time : {GetDayRunTime()}, Down Time : {GetDayDownTime()}, Error Time : {GetDayErrTime()}");
            cDEF.LOG.Trace(stemp);

            //Kill Past Log
            cDEF.LOG.KillPast();

            SPC_EFF   .ResetData();
            DAILY_DATA.ResetData();

            //
            cDEF.SEQ.WAT.ClearAlignCount();

        }
        //------------------------------------------------------------------------
        public void DisplayTime(ref System.Windows.Forms.DataGridView Grid)
        {
            int iCnt = 0;
			//
			if (Grid == null) return;
			//
			if ((Grid.RowCount <= 0) || (Grid.ColumnCount <= 0))
			{
                Grid.Rows.Clear();
                //Set Default Grid 
				FNC.SetGridStyle(ref Grid, 50, true, false, false, DataGridViewSelectionMode.RowHeaderSelect);
                //Set User Grid.
                Grid.MultiSelect = false;
                Grid.BackgroundColor = FRM.UIType == EN_UI_TYPE.Light ? Color.FromArgb(210,210,208) : Color.FromArgb(66, 72, 88);
                Grid.RowsDefaultCellStyle.BackColor = Color.FromArgb(153, 153, 153);
                Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(143, 143, 143);
                Grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                Grid.GridColor = Color.FromArgb(66, 72, 88);
                Grid.ScrollBars = ScrollBars.None;
                Grid.Enabled = false;

                //Set Column
                Grid.Columns.Add("Item"  , "" );
                Grid.Columns.Add("Time"  , "" ); 

				//Set Rows		
				Grid.Rows.Add(" Total" , 0);
				Grid.Rows.Add(" Run"   , 0);
				Grid.Rows.Add(" Jam"   , 0);
				Grid.Rows.Add(" Down"  , 0);
				Grid.Rows.Add(" Idle"  , 0);

                //정렬.
                for (int n = 0; n < Grid.Columns.Count; n++) Grid.Columns[n].SortMode = DataGridViewColumnSortMode.NotSortable; 
                //Text Align
                Grid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft  ;
                Grid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //Text Style
                Grid.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 13, FontStyle.Bold); Grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(254, 254, 254);
                Grid.Columns[0].DefaultCellStyle.Font   = new Font("Century Gothic", 13, FontStyle.Bold); Grid.Columns[0].DefaultCellStyle.ForeColor   = Color.FromArgb(254, 254, 254);
                Grid.Columns[1].DefaultCellStyle.Font   = new Font("Century Gothic", 12, FontStyle.Bold); Grid.Columns[1].DefaultCellStyle.ForeColor   = Color.DarkSlateBlue;
                //Col Width.                                         
				Grid.Columns[0] .Width  = (Grid.Width / Grid.Columns.Count); 
				Grid.Columns[1] .Width  = (Grid.Width / Grid.Columns.Count);
                //Row Height.                
                for (int n = 0; n < Grid.Rows.Count; n++) Grid.Rows[n].Height = Grid.Height / Grid.Rows.Count;
                //
                Grid.CurrentCell = null;
            }
            else
            {
                TimeSpan tsDiff = TimeSpan.FromHours(24) + (DateTime.Now - cDEF.SPC.m_tDayChangeTime);
                Grid[1 , iCnt++].Value = string.Format("{0:D2}:{1:D2}:{2:D2}" , tsDiff.Hours, tsDiff.Minutes, tsDiff.Seconds);	
                tsDiff = TimeSpan.FromTicks((long)cDEF.SPC.DAILY_DATA.dRunTime*10000);
                Grid[1 , iCnt++].Value = string.Format("{0:D2}:{1:D2}:{2:D2}" , tsDiff.Hours, tsDiff.Minutes, tsDiff.Seconds);
                tsDiff = TimeSpan.FromTicks((long)cDEF.SPC.DAILY_DATA.dErrorTime*10000);
                Grid[1 , iCnt++].Value = string.Format("{0:D2}:{1:D2}:{2:D2}" , tsDiff.Hours, tsDiff.Minutes, tsDiff.Seconds);
                tsDiff = TimeSpan.FromTicks((long)cDEF.SPC.DAILY_DATA.dDownTime*10000);
                Grid[1 , iCnt++].Value = string.Format("{0:D2}:{1:D2}:{2:D2}" , tsDiff.Hours, tsDiff.Minutes, tsDiff.Seconds);
                tsDiff = TimeSpan.FromTicks((long)cDEF.SPC.DAILY_DATA.dIdleTime*10000);
                Grid[1 , iCnt++].Value = string.Format("{0:D2}:{1:D2}:{2:D2}" , tsDiff.Hours, tsDiff.Minutes, tsDiff.Seconds);
            }

            //Visible
            if (!Grid.Visible && (Grid.RowCount > 0) && (Grid.ColumnCount > 0)) Grid.Visible = true; 

        }
        //------------------------------------------------------------------------
        public void Load(bool isLoad) 
        {
            LoadDayData(isLoad);
            LoadTime   (isLoad);
        }
        //--------------------------------------------------------------------------
        public void LoadDayData(bool isLoad) 
        {
            String sPath;
            String sFile = "DailyData";
            String sSection = sFile;
            //String sName;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\SPC");

            sPath = Application.StartupPath + "\\System\\SPC\\" + sFile + ".INI";

            //Load Work Quantity.
            if (isLoad) {
                ini.Load(sPath , sSection , "DayChanged "  , out m_bDayChanged          );
                ini.Load(sPath , sSection , "WorkQty    "  , out DAILY_DATA.iWorkQty    );
                ini.Load(sPath , sSection , "JamQty     "  , out DAILY_DATA.iJamQty     );
                ini.Load(sPath , sSection , "RunTime    "  , out DAILY_DATA.dRunTime    );
                ini.Load(sPath , sSection , "ErrorTime  "  , out DAILY_DATA.dErrorTime  );
                ini.Load(sPath , sSection , "DownTime   "  , out DAILY_DATA.dDownTime   );
                ini.Load(sPath , sSection , "IdleTime   "  , out DAILY_DATA.dIdleTime   );
                ini.Load(sPath , sSection , "AlignCnt   "  , out DAILY_DATA.iAlignCnt   );
               
            }
            else 
            {
                ini.Save(sPath , sSection , "DayChanged "  , m_bDayChanged              );
                ini.Save(sPath , sSection , "WorkQty    "  , DAILY_DATA.iWorkQty        );
                ini.Save(sPath , sSection , "JamQty     "  , DAILY_DATA.iJamQty         );
                ini.Save(sPath , sSection , "RunTime    "  , DAILY_DATA.dRunTime        );
                ini.Save(sPath , sSection , "ErrorTime  "  , DAILY_DATA.dErrorTime      );
                ini.Save(sPath , sSection , "DownTime   "  , DAILY_DATA.dDownTime       );
                ini.Save(sPath , sSection , "IdleTime   "  , DAILY_DATA.dIdleTime       );
                ini.Save(sPath , sSection , "AlignCnt   "  , DAILY_DATA.iAlignCnt       );
            }

            ini = null;
        }
        //--------------------------------------------------------------------------
        public void LoadTime(bool isLoad) 
        {
            String sPath;
            String sFile = "EffTime";
            String sSection = sFile;
            //String sName;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\SPC");

            sPath = Application.StartupPath + "\\System\\SPC\\" + sFile + ".INI";

            //Load Work Quantity.
            if (isLoad) 
            {

                ini.Load(sPath , sSection , "Init      " , out SPC_EFF.dtInit      );
                ini.Load(sPath , sSection , "Warning   " , out SPC_EFF.dtWarning   );
                ini.Load(sPath , sSection , "MCError   " , out SPC_EFF.dtMCError   );
                ini.Load(sPath , sSection , "HMError   " , out SPC_EFF.dtHMError   );
                ini.Load(sPath , sSection , "MLError   " , out SPC_EFF.dtMLError   );
                ini.Load(sPath , sSection , "MDError   " , out SPC_EFF.dtMDError   );
                ini.Load(sPath , sSection , "Error     " , out SPC_EFF.dtError     );
                ini.Load(sPath , sSection , "Running   " , out SPC_EFF.dtRunning   );
                ini.Load(sPath , sSection , "RunWarn   " , out SPC_EFF.dtRunWarn   );
                ini.Load(sPath , sSection , "Stop      " , out SPC_EFF.dtStop      );
                ini.Load(sPath , sSection , "Maint     " , out SPC_EFF.dtMaint     );
                ini.Load(sPath , sSection , "Idle      " , out SPC_EFF.dtIdle      );
                ini.Load(sPath , sSection , "JamCnt    " , out SPC_EFF.iJamCnt     );
            }
            else 
            {
                ini.Save(sPath , sSection, "Init      " ,  SPC_EFF.dtInit      );
                ini.Save(sPath , sSection, "Warning   " ,  SPC_EFF.dtWarning   );
                ini.Save(sPath , sSection, "MCError   " ,  SPC_EFF.dtMCError   );
                ini.Save(sPath , sSection, "HMError   " ,  SPC_EFF.dtHMError   );
                ini.Save(sPath , sSection, "MLError   " ,  SPC_EFF.dtMLError   );
                ini.Save(sPath , sSection, "MDError   " ,  SPC_EFF.dtMDError   );
                ini.Save(sPath , sSection, "Error     " ,  SPC_EFF.dtError     );
                ini.Save(sPath , sSection, "Running   " ,  SPC_EFF.dtRunning   );
                ini.Save(sPath , sSection, "RunWarn   " ,  SPC_EFF.dtRunWarn   );
                ini.Save(sPath , sSection, "Stop      " ,  SPC_EFF.dtStop      );
                ini.Save(sPath , sSection, "Maint     " ,  SPC_EFF.dtMaint     );
                ini.Save(sPath , sSection, "Idle      " ,  SPC_EFF.dtIdle      );
                ini.Save(sPath , sSection, "JamCnt    " ,  SPC_EFF.iJamCnt     );
            }
            ini = null;
        }
        //------------------------------------------------------------------------
        public string GetDayRunTime()
        {
            return ConvTimeTickToStr(DAILY_DATA.dRunTime);

        }
        //------------------------------------------------------------------------
        public string GetDayErrTime()
        {
            return ConvTimeTickToStr(DAILY_DATA.dErrorTime);
        }
        //------------------------------------------------------------------------
        public string GetDayDownTime()
        {
            return ConvTimeTickToStr(DAILY_DATA.dDownTime);
        }


    }
}
