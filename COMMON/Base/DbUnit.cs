using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.IO;


/***************************************************************************/
/* Class: TSqLiteDB                                                       */
/* Create:                                                                 */
/* Developer:                                                              */
/* Note:                                                                   */
/***************************************************************************/
public enum EN_FIELD_TYPE : int
{
	    dftDate     ,
	    dftTime     ,
	    dftString   ,
	    dftInteger  ,
	    dftFloat    ,
	    dftDFloat   ,
	    dftTimeStamp
};

public enum EN_GROUP_TYPE : int
{
    SUM = 0,
    AVR

}

enum EN_TABLE_DB : int
{
	dbJAM    ,
	dbPRD    ,
    dbEndOfDB

};

public struct _TSearchData {
    public int          iMode  ; 
    public String       sField ;
    public String       sData  ;
    public DateTime     dtDate ;
};

public struct _TDateData {
    public String       sField ;
    public DateTime     dtStart;
    public DateTime     dtEnd  ;
};

/***************************************************************************/
/* Class: TSqLiteDB                                                        */
/* Create:                                                                 */
/* Developer:                                                              */
/* Note:                                                                   */
/***************************************************************************/
public class TSqLiteDB
{
    //Vars.
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //private:   /* Member Var.             */
    int             m_iFieldCnt               ;
    String          m_sSortSql                ;
    String          m_sSql                    ;
    String          m_sSumSql                 ;
    String          m_sDbName                 ;
    String          m_sTblName                ;
    _TSearchData    SrhData                   ; //Search 용 변수
    _TDateData      DateData                  ; //Date 처리용 변수


    String[]       m_sFieldName = new string [50 ];
    String[]       m_sFieldType = new string [50 ];
    bool[]         m_bGroup     = new bool   [50 ];

    //protected: /* Inheritable Vars.        */

    //public:    /* Direct Accessable Vars.  */

    //Property.
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


    //Member Class 
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private SQLiteConnection  sql_con;
    private SQLiteCommand     sql_cmd;
    //private SQLiteDataAdapter DB     ;
    public  SQLiteDataReader  rdr    ;

    private DataSet   DS = new DataSet  ();
    private DataTable DT = new DataTable();
    

    //Method
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //생성자 & 소멸자. (Constructor & Destructor)

    public TSqLiteDB()
    {
        m_iFieldCnt = 0;
    }
    ~TSqLiteDB() { }
    //--------------------------------------------------------------------------
    public void   Init           (String sDbPath, String sDbName)
    {
        try
        {
            FNC.CreateDir(sDbPath);
            m_sDbName     = sDbPath + "\\" + sDbName + ".sqlite";
            bool isCreate = !FNC.FileExists(m_sDbName);

            //if (Environment.Is64BitProcess) File.Copy(Application.StartupPath + "\\x64\\SQLite.Interop.dll", Application.StartupPath + "\\SQLite.Interop.dll", true);
            //else                            File.Copy(Application.StartupPath + "\\x86\\SQLite.Interop.dll", Application.StartupPath + "\\SQLite.Interop.dll", true);

            if (isCreate) SQLiteConnection.CreateFile(m_sDbName);
            sql_con = new SQLiteConnection("Data Source=" + m_sDbName + ";Version=3;");
            if (isCreate) CreateTbl();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("DB Init:" + ex.Message);
        }
    }	
    //--------------------------------------------------------------------------
    public void   ClearSqlString ()
    {
        DateTime? dt = null;

        m_sSql           = "" ;
        m_sSumSql        = "" ;
        DateData.dtStart = Convert.ToDateTime(dt);
        DateData.dtEnd   = Convert.ToDateTime(dt);
        m_sSortSql       = "";
        SrhData.sField   = "" ;
        SrhData.sData    = "" ;
        SrhData.dtDate   = Convert.ToDateTime(dt);

    }	   
    //--------------------------------------------------------------------------
    public String GetFieldTypeStr(EN_FIELD_TYPE fType, int Size=0)
    {
        String sTemp = "";  
        if(fType == EN_FIELD_TYPE.dftDate      )  sTemp = "DATE";
        if(fType == EN_FIELD_TYPE.dftTime      )  sTemp = "TIME";
        if(fType == EN_FIELD_TYPE.dftString    )  sTemp = string.Format("VARCHAR({0})"  , Size);
        if(fType == EN_FIELD_TYPE.dftInteger   )  sTemp = "INT";
        if(fType == EN_FIELD_TYPE.dftFloat     )  sTemp = "FLOAT";
        if(fType == EN_FIELD_TYPE.dftDFloat    )  sTemp = string.Format("DECIMAL(10,{0})", Size);
        if(fType == EN_FIELD_TYPE.dftTimeStamp )  sTemp = "TIMESTAMP";
        return sTemp;
    }	
    //--------------------------------------------------------------------------

    public String GetFieldName   (int iNo)
    {
        
         String sTemp = m_sFieldName [iNo];            

        return sTemp;
    }	
    //--------------------------------------------------------------------------
    public bool isGroupField (int iNo)
    {
        return m_bGroup [iNo];
    }	
    //--------------------------------------------------------------------------

    public int    GetFieldCnt    ()
    {
        return m_iFieldCnt;
    }    
    //--------------------------------------------------------------------------
    //Table 처리
    public void   SetTable       (String Name)
    {
        m_sTblName =  Name;

    }	
    //--------------------------------------------------------------------------
    public void   AddField       (String Name, EN_FIELD_TYPE fType, int Size=0, bool bGroup = false)
    {
        int iFieldIndex = m_iFieldCnt  ;
        m_sFieldName [iFieldIndex] = Name  ;
        m_sFieldType [iFieldIndex] = GetFieldTypeStr(fType, Size);
        m_bGroup     [iFieldIndex] = bGroup;
        m_iFieldCnt   ++;
    }
    //--------------------------------------------------------------------------
    public void   CreateTbl      ()
    {
        String sFieldSql = "";
        String SqlStr        ;

        for(int i=0; i<m_iFieldCnt;i++)
        {
	        sFieldSql += m_sFieldName [i] + " " + m_sFieldType [i] + ",";
        }
        sFieldSql = sFieldSql.Substring(0, sFieldSql.Length-1);
        SqlStr =  "CREATE TABLE " + m_sTblName + "(" + sFieldSql + ")";
        ClearSqlString();
        AddSQL(SqlStr);
        ExecSQL();
    }	
    //--------------------------------------------------------------------------
    public void   DeleteTbl      ()
    {
        String SqlStr =  "DROP TABLE " + m_sTblName;
        ClearSqlString();
        AddSQL(SqlStr);
        ExecSQL();
    }	
    //--------------------------------------------------------------------------
    public void   ExecSQL        (bool isClose = true)
    {
        try
        {
            if(sql_con == null) return;
            
            sql_con.Open();
            sql_cmd            = sql_con.CreateCommand(); 
            sql_cmd.CommandText= m_sSql; 
            sql_cmd.ExecuteNonQuery(); 

            if(isClose) sql_con.Close(); 
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("[Exception] ExecSQL : " + ex.Message);
        }
    }
    //--------------------------------------------------------------------------	
    public void   UpdateSQLByGrid        (ref System.Windows.Forms.DataGridView Grid)
    {
        int[]    iWidth = {40, 0, 200};
  
        Grid.Visible                  = false;
        Grid.ReadOnly                 = true ;
        Grid.Enabled                  = true ;
        Grid.AllowUserToAddRows       = false;
        Grid.AllowUserToDeleteRows    = false;
        Grid.AllowUserToOrderColumns  = true ;
        Grid.AllowUserToResizeColumns = true ;
        Grid.AllowUserToResizeRows    = false;
        Grid.ColumnHeadersVisible     = true ;
        Grid.RowHeadersVisible        = false;  
        Grid.MultiSelect              = false;
        Grid.SelectionMode            = DataGridViewSelectionMode.FullRowSelect;
        Grid.EditMode                 = DataGridViewEditMode.EditOnEnter;
        Grid.AutoSizeColumnsMode      = DataGridViewAutoSizeColumnsMode.AllCells;
        Grid.AutoSizeRowsMode         = DataGridViewAutoSizeRowsMode.None;
        Grid.ForeColor                = System.Drawing.Color.Black;

        Grid.DefaultCellStyle.Font              = new System.Drawing.Font("Centry Gothic", 10);
        Grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Centry Gothic", 10);

        try
        {
            if (sql_con == null) return; 
            
            int iDbCnt = 0; 
            sql_con.Open(); 
            sql_cmd = sql_con.CreateCommand(); 
            sql_cmd.CommandText= m_sSql;    
            sql_cmd.ExecuteNonQuery(); 
            rdr = sql_cmd.ExecuteReader();

            Grid.Columns.Clear();
            for (int iCol = 0; iCol < rdr.FieldCount; iCol++)
            {                    
                Grid.Columns.Add(rdr.GetName(iCol).ToString() , rdr.GetName(iCol).ToString());
                //Grid.Columns[iCol].Width = 150;
            }

            string[] sItem = new string[rdr.FieldCount+1];  
            string   sTypeName;
            while(rdr.Read())
            {
                for (int iCol = 0; iCol < rdr.FieldCount; iCol++)  {
                    sTypeName = rdr.GetDataTypeName(iCol);
                    if(sTypeName == "DATE")  sItem[iCol] = string.Format("{0:yy/MM/dd}", rdr.GetValue(iCol));
                    else                     sItem[iCol] = Convert.ToString(rdr.GetValue(iCol)); 
                }
                Grid.Rows.Add(sItem);
                iDbCnt ++;
            }
            sql_con.Close(); 
        }
        catch (Exception e)
        {
            //cDEF.LOG.ExceptionTrace("DbUnit - UpdateSQLByGrid() " + e.ToString());
            if(sql_con != null) sql_con.Close();
            System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
        }
        //
        Grid.Visible = true;
    }
    //--------------------------------------------------------------------------
    public void   UpdateSQLByChart        (ref System.Windows.Forms.DataVisualization.Charting.Chart chart, bool isJamDB)
    {
        try
        {
            if (sql_con == null) return;

            int iDbCnt = 0;                 
            string   sTypeName , str;
            //
            chart.Series.Clear();
            //
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = m_sSql;
            sql_cmd.ExecuteNonQuery();
            rdr = sql_cmd.ExecuteReader();
            //
            string[] sItem = new string[rdr.FieldCount];  
            //
            for (int n = 1; n < rdr.FieldCount; n++)
            { 
                str = rdr.GetName(n).ToString();
                chart.Series.Add(isJamDB ? string.Format("Jam {0}", str) : string.Format("Prod {0}", str));
                chart.Series[n-1].ChartType = isJamDB ? System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column : System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar;
                chart.Series[n-1].IsValueShownAsLabel = true;
                chart.Series[n-1].IsVisibleInLegend = true;
            }
            //
            while(rdr.Read())
            {
                for (int iCol = 0; iCol < rdr.FieldCount; iCol++)  {
                
                    sTypeName = rdr.GetDataTypeName(iCol);
                    if(sTypeName == "DATE")  sItem[iCol] = string.Format("{0:yy/MM/dd}", rdr.GetValue(iCol));
                    else                     sItem[iCol] = Convert.ToString(rdr.GetValue(iCol)); 
                }
                //
                for (int n = 0; n <chart.Series.Count; n++)
                {                            
                    chart.Series[n].Points.Add(Convert.ToDouble(sItem[n+1]));                         
                    chart.Series[n].Points[iDbCnt].AxisLabel = sItem[0];
                }
                iDbCnt ++;
            }
            sql_con.Close();
        }
        catch (Exception e) { System.Diagnostics.Debug.WriteLine("Exception:" + e.Message); sql_con.Close(); }
    }
    //--------------------------------------------------------------------------
    public String  UpdateGroupFieldByGrid        (ref System.Windows.Forms.DataGridView Grid)
    {
        int iTotWidth = 0;
        int[]    iWidth = {40, 0};
        String[] sItem  = {"NO", "FIELD"};
        String   sFindField = "";

        //FNC.SetGridStyle(ref Grid); //, System.Windows.Forms.DockStyle.None);
        Grid.Visible                  = false;
        Grid.ReadOnly                 = true ;
        Grid.Enabled                  = true ;
        Grid.AllowUserToAddRows       = false;
        Grid.AllowUserToDeleteRows    = false;
        Grid.AllowUserToOrderColumns  = false;
        Grid.AllowUserToResizeColumns = true ;
        Grid.AllowUserToResizeRows    = false;
        Grid.ColumnHeadersVisible     = true ;
        Grid.RowHeadersVisible        = true ;  
        Grid.RowHeadersVisible        = false;
        Grid.MultiSelect              = false;
        Grid.SelectionMode            = DataGridViewSelectionMode.FullRowSelect;
        Grid.EditMode                 = DataGridViewEditMode.EditOnEnter;
        Grid.ForeColor                = System.Drawing.Color.Black;

        Grid.DefaultCellStyle.Font = new System.Drawing.Font("Centry Gothic", 10);
        Grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Centry Gothic", 10);

        Grid.Columns.Clear();
        for(int i=0;i<2;i++) 
        {
            Grid.Columns.Add(sItem[i] , sItem[i]);
            Grid.Columns[i].Width = iWidth[i];
            iTotWidth += iWidth[i];
        }
        Grid.Columns[1].Width = Grid.Width - iTotWidth-20;
        
         //Display.
         for (int i=0; i<GetFieldCnt(); i++) {
             sItem[0] = Convert.ToString(i + 1);
             sItem[1] = GetFieldName(i);

             if(!isGroupField(i)) continue;
             if(sFindField == "") sFindField = sItem[1];
             Grid.Rows.Add(sItem);
             }

        Grid.Visible = true;
        return sFindField;
    }
    //--------------------------------------------------------------------------
    public void UpdateReader        ()
    {
        sql_con.Open(); 
        sql_cmd = sql_con.CreateCommand(); 
        sql_cmd.CommandText= m_sSql;    
        sql_cmd.ExecuteNonQuery(); 
        rdr = sql_cmd.ExecuteReader();
    }  
    //--------------------------------------------------------------------------
    public bool NextRead        ()
    {
        return rdr.Read();

    }   
    //--------------------------------------------------------------------------
    public void   CloseSQL        ()
    {
        sql_con.Close(); 
    }
    //--------------------------------------------------------------------------
    //Record 처리
    public int    GetDataCount   ()
    {
        int iRecordCnt = 0;


        return iRecordCnt;

    }	
    //--------------------------------------------------------------------------
    public void   SetData        (String sData)
    {
        if(sData == "") sData = "NONE";
        m_sSql += "'" + sData + "',";

    }	
    //--------------------------------------------------------------------------
    public void   SetData        (int iData    )
    {
        m_sSql += Convert.ToString(iData)+ ",";
    }	
    //--------------------------------------------------------------------------
    public void   InsData        ()
    {
        String SqlStr = "insert into " + m_sTblName + " (";

         for (int i = 0; i < GetFieldCnt(); i++) {

	         SqlStr += GetFieldName(i) + ",";
         }

         SqlStr = SqlStr.Substring(0, SqlStr.Length-1);
         m_sSql = m_sSql.Substring(0, m_sSql.Length-1);

         SqlStr = SqlStr + ") values (" +
		          m_sSql + ")";

        ClearSqlString();
        AddSQL(SqlStr);
        ExecSQL();
    }	
    //--------------------------------------------------------------------------
    public void   DelAllData     ()
    {
        String SqlStr = "delete from " + m_sTblName;
        ClearSqlString();
        AddSQL(SqlStr);
        ExecSQL();

    }	
    //--------------------------------------------------------------------------
    public void   DelDataByDay   (String sField, int iDay)
    {
        DateTime  tDate = DateTime.Now.AddDays( - iDay);
        String    sDate = string.Format("{0:yy/MM/dd}", tDate);
        ClearSqlString();
        AddSQL  ("delete from " + m_sTblName            );
        AddSQL  ("Where " + sField+ " < '" + sDate + "'");
        ExecSQL();
    }
    //--------------------------------------------------------------------------
    public void   GetData        (int iFieldNo, ref String    pBuf)
    {       
        String sField = GetFieldName(iFieldNo); 
        pBuf = Convert.ToString(rdr[sField]);

    }	
    //--------------------------------------------------------------------------
    public void   GetData        (int iFieldNo, ref DateTime  pBuf)
    {
        String sField = GetFieldName(iFieldNo); 
        pBuf = Convert.ToDateTime(rdr[sField]);
    }	
    //--------------------------------------------------------------------------
    public void   GetData        (int iFieldNo, ref int       pBuf)
    {
        String sField = GetFieldName(iFieldNo); 
        pBuf = Convert.ToInt32(rdr[sField]);
    }	
    //--------------------------------------------------------------------------
    public void   GetData        (int iFieldNo, ref double    pBuf)
    {
        String sField = GetFieldName(iFieldNo); 
        pBuf = Convert.ToDouble(rdr[sField]);
    }	
    //--------------------------------------------------------------------------
    //View 처리
    public void   AddSQL         (String SqlStr)
    {
        if(m_sSql != "") m_sSql += " ";
        m_sSql += SqlStr;
    }	
    //--------------------------------------------------------------------------
    public void   AddSQLDate_Sort()
    {
        String tStart = string.Format("{0:yy/MM/dd}", DateData.dtStart);
        String tEnd   = string.Format("{0:yy/MM/dd}", DateData.dtEnd  );

        if(DateData.dtStart.Ticks > 0 && DateData.dtEnd.Ticks > 0 && DateData.sField != "") {
	        AddSQL("Where " + DateData.sField + " Between '" + tStart + "' AND '" + tEnd + "'");
	        }
        if(m_sSortSql  != "") AddSQL(m_sSortSql );
    }	
    //--------------------------------------------------------------------------
    public void   SetDateTime    (String sField, DateTime sDate1, DateTime sDate2)
    {
        DateData.dtStart = sDate1;
        DateData.dtEnd   = sDate2;
        DateData.sField = sField;
    }	
    //--------------------------------------------------------------------------
    public void   SetSearchData  (String sField, String    Data)
    {
        SrhData.iMode   = 0;
        SrhData.sField  = sField;
        SrhData.sData   = Data  ;
    }	
    //--------------------------------------------------------------------------
    public void   SetSearchData  (String sField, DateTime  Date)
    {
        SrhData.iMode   = 2;
        SrhData.sField  = sField;
        SrhData.dtDate  = Date  ;
    }	
    //--------------------------------------------------------------------------
    public void   SetSorting     (String sField, int Kind)
    {
        m_sSortSql  = "";
        if (sField == "") return;
        if (Kind == 0) m_sSortSql = "Order by " + sField + " ASC" ;
        else           m_sSortSql = "Order by " + sField + " DESC";
    }	
    //--------------------------------------------------------------------------
    public void   SetGroupCount  (String sField, String Data = "")
    {
        String sDecode;

        if(Data == "")
        {
	        m_sSumSql += " ,COUNT(" + sField+") As " + sField + "_QTY";
        }
        else
        {
	        sDecode = "decode(" + sField + ",'" + Data + "',1,NULL)";
	        m_sSumSql += " ,COUNT(" + sDecode+") As " + Data + "_QTY";;
        }
    }	
    //--------------------------------------------------------------------------
    public void   SetSumAvg      (String sField, EN_GROUP_TYPE Kind)
    {
        if (Kind == EN_GROUP_TYPE.SUM) m_sSumSql += " ,SUM("+sField+") As " + sField;
        else                           m_sSumSql += " ,AVG("+sField+") As " + sField;
    }	
    //--------------------------------------------------------------------------
    public void   AllDataSql  ()
    {
    	AddSQL("select * from " + m_sTblName);
        AddSQLDate_Sort();
    }	
    //--------------------------------------------------------------------------
    public void   GroupCountSql (String gField, String cField)
    {
        AddSQL("Select " + gField + ", COUNT(*) AS " + cField + " from " + m_sTblName);
        AddSQLDate_Sort();
        AddSQL("Group by " + gField);

    }
    //--------------------------------------------------------------------------
    public void GroupSumAvgSql(String gField)
    {
        if(m_sSumSql == "") return;
        AddSQL("Select " + gField + m_sSumSql + " from " + m_sTblName);
        String tStart, tEnd;
        if(DateData.dtStart.Ticks != 0 && DateData.dtEnd  .Ticks != 0)
        {
	        tStart = string.Format("{0:yy/MM/dd}", DateData.dtStart);
            tEnd   = string.Format("{0:yy/MM/dd}", DateData.dtEnd  );

	        if(DateData.dtStart.Ticks > 0 && DateData.dtEnd.Ticks > 0 && DateData.sField != "") {
		        AddSQL("Where " + DateData.sField + " Between '" + tStart + "' AND '" + tEnd + "'");
		        }
         }
        AddSQL("Group by " + gField);
        if(m_sSortSql  != "") AddSQL(m_sSortSql );
    }	
    //--------------------------------------------------------------------------
    public void SearchSql      ()
    {
        String tSearch = string.Format("{0:yy/MM/dd}", SrhData .dtDate );
        String tStart  = string.Format("{0:yy/MM/dd}", DateData.dtStart);
        String tEnd    = string.Format("{0:yy/MM/dd}", DateData.dtEnd  );

        AddSQL("select * from " + m_sTblName);
        if(SrhData.sField != "" && (SrhData.sData != "" || SrhData.dtDate.Ticks>0)) {
	        if (SrhData.iMode == 0) {
		        AddSQL("Where "+SrhData.sField+" Like '%" + SrhData.sData + "%'");
		        }
	        else if (SrhData.iMode == 1) {
		        //
		        }
	        else {
		        AddSQL("Where "+SrhData.sField+" = '" + tSearch + "'");
		        }
	        if(DateData.dtStart.Ticks > 0 && DateData.dtEnd.Ticks > 0 && DateData.sField != "") AddSQL("AND ");
	        }

        if(DateData.dtStart.Ticks > 0 && DateData.dtEnd.Ticks > 0 && DateData.sField != "") {
	        AddSQL("Where " + DateData.sField + " Between '" + tStart + "' AND '" + tEnd + "'");
	        }
        if(m_sSortSql  != "") AddSQL(m_sSortSql );
    }	
    //--------------------------------------------------------------------------
    public void   ExportExcel    (ref System.Windows.Forms.DataGridView Grid)
    {
        String sPath  ;
        String sData  = "";

        string sFile =   "[" + string.Format("{0:yyMMdd}", DateTime.Now)+ "]" + m_sTblName + "csv"; 
        //Make Dir.
        FNC.CreateDirOnWork("Export");
        sPath = Application.StartupPath + "\\Export\\" + sFile;

        //File Open.
        FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fp, Encoding.Default);
        sw.BaseStream.Seek(0, SeekOrigin.End);

        //Set List.
        for (int c=0; c<Grid.ColumnCount;c++) {
            sData += Grid.Columns[c].Name;
            if(c < Grid.ColumnCount-1) sData += ",";
            }
        sw.WriteLine(sData);
        sData = "";
        for (int r=0; r<Grid.RowCount;r++) {
            for (int c=0; c<Grid.ColumnCount;c++) {
                sData += Grid[c,r].Value.ToString();
                if(c < Grid.ColumnCount-1) sData += ",";
                } 
            sw.WriteLine(sData);
            sData = "";
            }

        sw.Flush();
        sw.Close();
    }	
	//--------------------------------------------------------------------------
	public void AddSQLDate_SortbyShiftDay()
    {
       String tStart = string.Format("{0:yy/MM/dd HH:mm:ss.ffff}", DateData.dtStart);
       String tEnd = string.Format("{0:yy/MM/dd HH:mm:ss.ffff}", DateData.dtEnd);

       if (DateData.dtStart.Ticks > 0 && DateData.dtEnd.Ticks > 0 && DateData.sField != "")
       {
           AddSQL("Where " + "t" + " Between '" + tStart + "' AND '" + tEnd + "'");
       }
       if (m_sSortSql != "") AddSQL(m_sSortSql);
    }
    //--------------------------------------------------------------------------
	public int    GetDataCountByShiftDay   ()
    {
        try
        {
            int iRecordCnt = 0;
            //AddSQL("Select " + " COUNT(*) AS " + "Cnt" + " from " + m_sTblName);
            //AddSQLDate_Sort();

            //UpdateReader();

            //rdr.Read();
            //iRecordCnt = Convert.ToInt32(rdr.GetValue(0));
            //sql_con.Close();

            AddSQL("Select " + " (Day || ' ' || StartTime) as " + "t" + " from " + m_sTblName);
            AddSQLDate_SortbyShiftDay();

            UpdateReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(rdr);
            iRecordCnt = dataTable.Rows.Count;

            sql_con.Close();

            return iRecordCnt;
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"[Exception Error] {e.Message}");
            sql_con.Close();
            return 0;
        }
    }	
	 //--------------------------------------------------------------------------
    public void GetErrorListForDay(ref System.Windows.Forms.DataGridView Grid)
    {
        //Grid.Dock = System.Windows.Forms.DockStyle.Fill;
        //FNC.SetGridStyle(ref Grid, 30, false, true, false, DataGridViewSelectionMode.RowHeaderSelect);
        ////
        //Grid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None; //DataGridViewAutoSizeColumnMode.Fill; //DataGridViewAutoSizeColumnMode.NotSet;
        //Grid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        //Grid.ScrollBars = ScrollBars.None;
        //Grid.BackgroundColor = System.Drawing.Color.FromArgb(66, 72, 88);
        //Grid.ClearSelection();
        //Grid.MultiSelect = false;
        //Grid.ReadOnly = true;
        
        Grid.Rows.Clear();
        Grid.Columns.Clear();
        Grid.Visible                  = false;
        Grid.ReadOnly                 = true ;
        Grid.Enabled                  = true ;
        Grid.AllowUserToAddRows       = false;
        Grid.AllowUserToDeleteRows    = false;
        Grid.AllowUserToOrderColumns  = true ;
        Grid.AllowUserToResizeColumns = true ;
        Grid.AllowUserToResizeRows    = false;
        Grid.ColumnHeadersVisible     = true ;
        Grid.RowHeadersVisible        = false;  
        Grid.MultiSelect              = false;
        Grid.SelectionMode            = DataGridViewSelectionMode.FullRowSelect;
        Grid.EditMode                 = DataGridViewEditMode.EditOnEnter;
        //Grid.AutoSizeColumnsMode      = DataGridViewAutoSizeColumnsMode.AllCells;
        Grid.AutoSizeRowsMode         = DataGridViewAutoSizeRowsMode.None;
        Grid.ForeColor                = System.Drawing.Color.Black;

        Grid.DefaultCellStyle.Font              = new System.Drawing.Font("Centry Gothic", 10);
        Grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Centry Gothic", 10);
    

        Grid.Columns.Add("No", "No");
        Grid.Columns.Add("CODE", "CODE");
        Grid.Columns.Add("NAME", "NAME");
        Grid.Columns.Add("COUNT", "COUNT");

        Grid.Columns[0].Width = 40;
        Grid.Columns[1].Width = 70;
        Grid.Columns[2].Width = Grid.Width - (40 + 70 + 70);
        Grid.Columns[3].Width = 70;

    try
    {
        AddSQL("Select " + "ErrNo , Contents ," + " COUNT(*) AS " + "Cnt" + "," + "(Day || ' ' || StartTime) as t  " + " from " + m_sTblName);
        AddSQLDate_SortbyShiftDay();
        AddSQL("Group by " + "ErrNo");
        AddSQL("Order by " + "Cnt" + " DESC");
        UpdateReader();

        string sName = "";
        int idbCnt = 0;
        while (rdr.Read())
        {
            string[] sItem = new string[rdr.FieldCount + 1];
            sItem[0] = (idbCnt + 1).ToString();

            for (int iCol = 0; iCol < rdr.FieldCount; iCol++)
            {
                sName = rdr.GetName(iCol);
                     if (sName == "ErrNo")      sItem[iCol + 1] = Convert.ToString(rdr.GetValue(iCol));
                else if (sName == "Contents")   sItem[iCol + 1] = Convert.ToString(rdr.GetValue(iCol));
                else if (sName == "Cnt")        sItem[iCol + 1] = Convert.ToString(rdr.GetValue(iCol));
            }
            Grid.Rows.Add(sItem);
            idbCnt++;
        }
        sql_con.Close();
        }
    catch (Exception e)
    {
        //cDEF.LOG.ExceptionTrace("DbUnit - UpdateSQLByGrid() " + e.ToString());
        System.Diagnostics.Debug.WriteLine($"[Exception Error] {e.Message}");
        sql_con.Close();
    }
        Grid.Visible = true;
     
    }
}
