using System;
using System.Globalization;
using System.Reflection;

namespace Drv.CameraController
{
    public class cVersion
    {
        private static string mVer;

        public static string Ver
        {
            get
            {
                SetBuildTimeVersion();
                return mVer;
            }
        }

        public static string VersionNote
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("This Version: ");
                sb.AppendLine(Ver);

                sb.AppendLine("//---------------------------------------------------------------------------------------------------//");
                sb.AppendLine("Version: ");
                sb.AppendLine("Date: ");
                sb.AppendLine("Discription: ");
                sb.AppendLine("     1. ");
                sb.AppendLine("     2. ");
                sb.AppendLine("//---------------------------------------------------------------------------------------------------//");

                return sb.ToString();
            }
        }


        private static void SetBuildTimeVersion()
        {
            //1. Assembly.GetExecutingAssembly().FullName의 값은 'ApplicationName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' 와 같다.
            string strVersionText = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Trim().Split('=')[1];

            //2. Version Text의 세번째 값(Build Number)은 2000년 1월 1일부터 Build된 날짜까지의 총 일(Days) 수 이다.
            int      days        = Convert.ToInt32(strVersionText.Split('.')[2]);
            DateTime refDate     = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refDate.AddDays(days);

            //3. Verion Text의 네번째 값(Revision NUmber)은 자정으로부터 Build된 시간까지의 지나간 초(Second) 값 이다.
            int seconds = Convert.ToInt32(strVersionText.Split('.')[3]);
            seconds *= 2;
            dtBuildDate = dtBuildDate.AddSeconds(seconds);

            //4. 시차조정
            DaylightTime daylingTime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);
            if (TimeZone.IsDaylightSavingTime(dtBuildDate, daylingTime))
                dtBuildDate = dtBuildDate.Add(daylingTime.Delta);

            //5. 버전 빌드날자 + 빌드한 날의 초(Second) 값
            mVer = string.Format("Ver. {0}.{1}",dtBuildDate.ToString("yyMMdd"),seconds);
        }
    }
}
