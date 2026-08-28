
using System;
using System.Security.Cryptography;

using static DBProc.SysConfig.cDBRunTimeProc;

namespace DBProc.SysConfig
{
    public class cDBRunTimeProc : cDBProcBase<cData>
    {
        public const string TimeFormat   = "yyyy-MM-dd HH:mm:ss.ffff";
        public const string DBTimeFormat = "%Y-%m-%d %H:%M:%f";

        //public int ShiftTimeHour = 22;
        [cDBTable("TB_RUN_TIME")]
        public class cData : cDBDataBase
        {
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, true , true , "")] public string UpdateTime { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 10, false, false, "")] public string State      { get; set; }

            public cData()
            {
            }
            public cData(string pState)
            {
                UpdateTime = DateTime.Now.ToString(TimeFormat);
                State = pState;
            }
        }

        #region < Singleton >
        private static readonly cDBRunTimeProc mInstance = new cDBRunTimeProc();
        public static cDBRunTimeProc Instance { get { return mInstance; } }
        #endregion

        #region < Constructors >
        public cDBRunTimeProc(){ }
        #endregion

        #region < Public Methods >
        public void Initialize(string pFilePath,string pFileName, bool pIsKeepCon)
        {
            string FilePath = pFilePath + "\\" + pFileName + ".db";
            if (System.IO.File.Exists(FilePath) == false)
            {
                cSQLiteQueryProc.CreateDefault(FilePath);
            }
            Query = new cSQLiteQueryProc(FilePath, pIsKeepCon);
            base.Initialize(Query);
        }

        public cData[] GetDatas(DateTime pStart, DateTime pEnd)
        {
            string startTime = $"strftime('{DBTimeFormat}','{pStart.ToString(TimeFormat)}')";
            string endTime   = $"strftime('{DBTimeFormat}','{pEnd.ToString(TimeFormat)}')";
            string where     = $"strftime('{DBTimeFormat}',UpdateTime) >= {startTime} AND strftime('{DBTimeFormat}',UpdateTime) <= {endTime}";

            cData[] datas = GetRows(where);
            return datas;
        }

        public cData GetLastData(DateTime pStart, DateTime pEnd)
        {
            string startTime = $"strftime('{DBTimeFormat}','{pStart.ToString(TimeFormat)}')";
            string endTime   = $"strftime('{DBTimeFormat}','{pEnd.ToString(TimeFormat)}')";
            string where     = $"strftime('{DBTimeFormat}',UpdateTime) >= {startTime} AND strftime('{DBTimeFormat}',UpdateTime) <= {endTime}";
            string oderby    = "ORDER BY strftime('%Y-%m-%d %H:%M:%f',UpdateTime) DESC LIMIT 1";

            cData[] datas = GetRows($"{where} {oderby}");
            if(datas.Length > 0) return datas[0];
            else return null;
        }

        //public TimeSpan[] GetTimeALL(DateTime pStartDay, DateTime pEndDay)
        //{
        //    DateTime shiftStartTime = new DateTime(pStartDay.Year,pStartDay.Month, pStartDay.Day, ShiftTimeHour,0,0);
        //    DateTime shiftEndTime   = new DateTime(pEndDay.Year,pEndDay.Month, pEndDay.Day, ShiftTimeHour,0,0);

        //    cData[] datas = GetDatas(shiftStartTime, shiftEndTime);

        //    if (datas.Length == 0) return new TimeSpan[]{new TimeSpan(), new TimeSpan(), new TimeSpan() };

        //    TimeSpan runTime     = new TimeSpan();
        //    TimeSpan stopTime    = new TimeSpan();
        //    TimeSpan alarmTime   = new TimeSpan();

        //    DateTime.TryParse(datas[0].UpdateTime, out DateTime changedTimeStart);
        //    if(datas[0].State == "RUN")
        //    {
        //        runTime += changedTimeStart - shiftStartTime;
        //    }
        //    else if(datas[0].State == "STOP")
        //    {
        //        stopTime += changedTimeStart - shiftStartTime;
        //    }
        //    else if(datas[0].State == "ALARM")
        //    {
        //        alarmTime += changedTimeStart - shiftStartTime;
        //    }

        //    for (int i = 0; i < datas.Length - 1; i++)
        //    {
        //        bool r1 = DateTime.TryParse(datas[i].UpdateTime, out DateTime currentTime);
        //        bool r2 = DateTime.TryParse(datas[i + 1].UpdateTime, out DateTime nextTime);

        //        if (datas[i].State == "RUN")
        //        {
        //            if (r1 && r2) runTime += nextTime - currentTime;
        //        }
        //        else if (datas[i].State == "STOP")
        //        {
        //            if (r1 && r2) stopTime += nextTime - currentTime;
        //        }
        //        else if (datas[i].State == "ALRM")
        //        {
        //            if (r1 && r2) alarmTime += nextTime - currentTime;
        //        }
        //    }

        //    DateTime.TryParse(datas[datas.Length - 1].UpdateTime, out DateTime changedTimeEnd);

        //    if (datas[datas.Length - 1].State == "RUN")
        //    {
        //        if(shiftEndTime > DateTime.Now) runTime += DateTime.Now - changedTimeEnd;
        //        else
        //        {
        //            runTime += shiftEndTime - changedTimeEnd;
        //        }
        //    }
        //    else if (datas[datas.Length - 1].State == "STOP")
        //    {
        //        if(shiftEndTime > DateTime.Now) runTime += DateTime.Now - changedTimeEnd;
        //        else
        //        {
        //            stopTime += shiftEndTime - changedTimeEnd;
        //        }
        //    }
        //    else if (datas[datas.Length - 1].State == "ALARM")
        //    {
        //        if(shiftEndTime > DateTime.Now) runTime += DateTime.Now - changedTimeEnd;
        //        else
        //        {
        //            alarmTime += shiftEndTime - changedTimeEnd;
        //        }
        //    }
        //    return new TimeSpan[]{runTime, stopTime, alarmTime };
        //}

        private TimeSpan GetTime(DateTime pStartDay, DateTime pEndDay, string pState)
        {
            DateTime shiftStartTime = new DateTime(pStartDay.Year,pStartDay.Month, pStartDay.Day, pStartDay.Hour,0,0);
            DateTime shiftEndTime   = new DateTime(pEndDay.Year,pEndDay.Month, pEndDay.Day, pEndDay.Hour,0,0);

            cData[] datas = GetDatas(shiftStartTime, shiftEndTime);

            if (datas.Length == 0) return new TimeSpan();

            TimeSpan runTime   = new TimeSpan();

            if(datas[0].State == pState)
            {
                DateTime.TryParse(datas[0].UpdateTime, out DateTime changedTime);
                TimeSpan addtime = changedTime - shiftStartTime;
                runTime += addtime;
            }

            for (int i = 0; i < datas.Length - 1; i++)
            {
                if (datas[i].State == pState)
                {
                    bool r1 = DateTime.TryParse(datas[i    ].UpdateTime, out DateTime currentTime);
                    bool r2 = DateTime.TryParse(datas[i + 1].UpdateTime, out DateTime nextTime);
                    if(r1 && r2) runTime += nextTime - currentTime;
                }
            }

            if(datas[datas.Length - 1].State == pState)
            {
                DateTime.TryParse(datas[datas.Length - 1].UpdateTime, out DateTime changedTime);
                if(shiftEndTime > DateTime.Now) runTime += DateTime.Now - changedTime;
                else
                {
                    runTime += shiftEndTime - changedTime;
                }
            }

            return runTime;
        }
        public TimeSpan GetRunTime(DateTime pStartDay, DateTime pEndDay)
        {
            return GetTime(pStartDay,pEndDay,"RUN");
        }

        public TimeSpan GetRunTimeforDay()
        {
            DateTime StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 0, 0, 0).AddDays(-1);
            DateTime EndDate   = StartDate.AddDays(1);

            return GetTime(StartDate, EndDate, "RUN");
        }
        public TimeSpan GetStopTime(DateTime pStartDay, DateTime pEndDay)
        {
            return GetTime(pStartDay,pEndDay,"STOP");
        }
        public TimeSpan GetAlarmTime(DateTime pStartDay, DateTime pEndDay)
        {
            return GetTime(pStartDay,pEndDay,"ALARM");
        }
        public int GetErrorCount(DateTime pStartDay, DateTime pEndDay)
        {
            string where = $"strftime('{DBTimeFormat}',InnerID) >= {pStartDay} AND strftime('{DBTimeFormat}',InnerID) <= {pEndDay} AND State = 'ALARM'";

            return GetRowCount(where);
        }

        public cData GetLastState(DateTime pStartDay , DateTime pEndDay)
        {
            DateTime shiftStartTime = new DateTime(pStartDay.Year, pStartDay.Month, pStartDay.Day, pStartDay.Hour, 0, 0);
            DateTime shiftEndTime = new DateTime(pEndDay.Year, pEndDay.Month, pEndDay.Day, pEndDay.Hour, 0, 0);

            cData datas = GetLastData(shiftStartTime, shiftEndTime);

            return datas;
        }

        public cData ChangeState(string pState)
        {

            DateTime dtStartDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 0, 0).AddDays(-1);
            DateTime dtEndDay   = dtStartDay.AddDays(1);

            cData data = GetLastState(dtStartDay, dtEndDay);

            if (data != null)
            {
                if (data.State == pState)
                {
                    return null;
                }
            }

            data = new cData(pState);
            if(InsertRow(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        #endregion


    }
}
