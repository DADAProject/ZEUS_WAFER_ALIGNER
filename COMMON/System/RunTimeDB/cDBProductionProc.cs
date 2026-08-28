
using System;

using static DBProc.SysConfig.cDBProductionProc;

namespace DBProc.SysConfig
{
    public class cDBProductionProc : cDBProcBase<cData>
    {
        public const string TimeFormat   = "yyyy-MM-dd HH:mm:ss.ffff";
        public const string DBTimeFormat = "%Y-%m-%d %H:%M:%f";
        public enum eTimeKind
        {
            LoadLiftInTime            ,
            LoadLiftRFIDReadTime      ,
            LoadRotaterInTime         ,
            LoadPickerPickTime        ,
            LoadPickerPlaceTime       ,
            TH1PickTime               ,
            TH1PlaceTime              ,
            TH2PickTime               ,
            TH2PlaceTime              ,
            TH3PickTime               ,
            TH3PlaceTime              ,
            TH4PickTime               ,
            TH4PlaceTime              ,
            SonicBathStartTime        ,
            SonicBathCompleteTime     ,
            WashingChamberStartTime   ,
            WashingChamberCompleteTime,
            DryChamberStartTime       ,
            DryChamberCompleteTime    ,
            UnloadPickerPickTime      ,
            UnloadPickerPlaceTime     ,
            UnloadRotaterInTime       ,
            UnloadLiftInTime          ,
            UnloadLiftRFIDReadTime    ,
            UnloadLiftOutTime         
        }

        [cDBTable("TB_PRODUCTION")]
        public class cData : cDBDataBase
        {
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, true , true , "")] public string InnerID                   { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 30, false, false, "")] public string LoadMgzID                 { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 30, false, false, "")] public string UnloadMgzID               { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string LoadLiftInTime            { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string LoadLiftRFIDReadTime      { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string LoadRotaterInTime         { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string LoadPickerPickTime        { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string LoadPickerPlaceTime       { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH1PickTime               { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH1PlaceTime              { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH2PickTime               { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH2PlaceTime              { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH3PickTime               { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH3PlaceTime              { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH4PickTime               { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string TH4PlaceTime              { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string SonicBathStartTime        { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string SonicBathCompleteTime     { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string WashingChamberStartTime   { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string WashingChamberCompleteTime{ get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string DryChamberStartTime       { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string DryChamberCompleteTime    { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string UnloadPickerPickTime      { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string UnloadPickerPlaceTime     { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string UnloadRotaterInTime       { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string UnloadLiftInTime          { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string UnloadLiftRFIDReadTime    { get; set; }
            [cDBField(cDBFieldAttribute.eDataType.STRING, 24, false, false, "")] public string UnloadLiftOutTime         { get; set; }

            public cData()
            {
                InnerID = DateTime.Now.ToString(TimeFormat);
            }
        }

        #region < Singleton >
        private static readonly cDBProductionProc mInstance = new cDBProductionProc();
        public static cDBProductionProc Instance { get { return mInstance; } }
        #endregion

        #region < Constructors >
        public cDBProductionProc(){ }
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

        public cData[] GetDatas(DateTime pStart, DateTime pEnd, string pInnerID = null)
        {
            string startTime = $"strftime('{DBTimeFormat}','{pStart.ToString(TimeFormat)}')";
            string endTime = $"strftime('{DBTimeFormat}','{pEnd.ToString(TimeFormat)}')";
            string where = $"strftime('{DBTimeFormat}',InnerID) >= {startTime} AND strftime('{DBTimeFormat}',InnerID) <= {endTime}";
            if (pInnerID != null)
                where = $"{where} AND InnerID = '{pInnerID}'";

            cData[] datas = GetRows(where);
            return datas;
        }

        public int GetInCount(DateTime pStart, DateTime pEnd)
        {
            string startTime = $"strftime('{DBTimeFormat}','{pStart.ToString(TimeFormat)}')";
            string endTime   = $"strftime('{DBTimeFormat}','{pEnd  .ToString(TimeFormat)}')";
            string queryText = $"SELECT COUNT(*) FROM TB_PRODUCTION";
            string where     = $"WHERE strftime('{DBTimeFormat}',InnerID) >= {startTime}";
                   where     = $"{where} AND strftime('{DBTimeFormat}',InnerID) <= {endTime}";

            object result    = Query.ExecuteScalar( $"{queryText} {where}");

            int.TryParse(result.ToString() , out int inCount);
            return inCount;
        }
        public int GetOutCount(DateTime pStart, DateTime pEnd)
        {
            string startTime = $"strftime('{DBTimeFormat}','{pStart.ToString(TimeFormat)}')";
            string endTime   = $"strftime('{DBTimeFormat}','{pEnd  .ToString(TimeFormat)}')";
            string queryText = $"SELECT COUNT(*) FROM TB_PRODUCTION";
            string where     = $"WHERE strftime('{DBTimeFormat}',InnerID) >= {startTime} AND strftime('{DBTimeFormat}',InnerID) <= {endTime}";
                   where     = $"{where} AND LENGTH(UnloadLiftOutTime) >= 23";
            
            object result    = Query.ExecuteScalar( $"{queryText} {where}");

            int.TryParse(result.ToString() , out int OutCount);
            return OutCount;
        }

        public bool UpdateTime(string pInnerID,eTimeKind pTimeKind)
        {
            string queryText = $"UPDATE TB_PRODUCTION SET {pTimeKind} = '{DateTime.Now.ToString(TimeFormat)}'";
            string where     = $"WHERE InnerID = '{pInnerID}'";
            return Query.ExecuteNonQuery($"{queryText} {where}") > 0;
        }

        public bool UpdateLoadMgzID(string pInnerID, string pID)
{
            string queryText = $"UPDATE TB_PRODUCTION SET LoadMgzID = '{pID}'";
            string where     = $"WHERE InnerID = '{pInnerID}'";
            return Query.ExecuteNonQuery($"{queryText} {where}") > 0;
        }
        public bool UpdateUnloadMgzID(string pInnerID, string pID)
        {
            string queryText = $"UPDATE TB_PRODUCTION SET UnloadMgzID = '{pID}'";
            string where = $"WHERE InnerID = '{pInnerID}'";
            return Query.ExecuteNonQuery($"{queryText} {where}") > 0;
        }

        public cData Create()
        {
            cData data = new cData();
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
