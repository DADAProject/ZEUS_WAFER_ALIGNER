
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Drawing2D;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Linq;
using System.Drawing.Imaging;
using eMachine;
using System.Security.Policy;


/***************************************************************************/
/* Class: MsgBox                                                           */
/* Create:                                                                 */
/* Developer:                                                              */
/* Note:                                                                   */
/***************************************************************************/

public static class MsgBox
{
    public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
    {
        return MessageBox.Show(new Form { TopMost = true }, text, Application.ProductName, buttons, icon, defaultButton);
    }
    //--------------------------------------------------------------------------
    public static void Show(string text)
    {
        Show(text, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    //--------------------------------------------------------------------------
    public static void Error(string text)
    {
        Show(text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    //--------------------------------------------------------------------------
    public static void Error(string fmt, params object[] args)
    {
        Error(string.Format(fmt, args));
    }
    //--------------------------------------------------------------------------
    public static void Warning(string text)
    {
        Show(text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    //--------------------------------------------------------------------------
    public static bool Confirm(string text)
    {
        return Show(text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
    }
    //--------------------------------------------------------------------------
    public static bool Confirm(string fmt, params object[] args)
    {
        return Confirm(string.Format(fmt, args));
    }
}

/***************************************************************************/
/* Class: FNC                                                              */
/* Create:                                                                 */
/* Developer:                                                              */
/* Note:                                                                   */
/***************************************************************************/
public static class FNC
{
	internal static DateTime Delay(int ms)		
	{		
		DateTime ThisMoment = DateTime.Now;		
		TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);		
		DateTime AfterWards = ThisMoment.Add(duration);		
		while (AfterWards >= ThisMoment)		
		{		
			System.Windows.Forms.Application.DoEvents();		
			ThisMoment = DateTime.Now;		
		}		
		return DateTime.Now;		
	}
	internal static bool CalcMoveTravelTimeMs(double Dist, double vel, double Acc, double Dec, out double ElapTime)
	{
		//
		double dAccDist   = (vel * Acc)/2;
		double dDecDist   = (vel * Dec)/2;
		double dConstDist = Dist - (dAccDist + dDecDist);
		//
		if ((Acc + Dec) * 1000 <= 0) ElapTime = 0;
		else                         ElapTime = (Acc + Dec + dConstDist / vel) * 1000;
		//
		return (ElapTime > 0);
	}
    internal static double DegToRad(double angle)
    {
       return Math.PI * angle / 180.0;
    }
    internal static double RadToDeg(double angle)
    {
       return angle * (180.0 / Math.PI);
    }
    //---------------------------------------------------------------------------
    internal static double GetLinear(double dXi, double dX1, double dX2, double dY1, double dY2)
    {
        double dA      = 0.0;
        double dB      = 0.0;
        double dLinear = 0.0;

        if(dX2 - dX1 == 0) return 0.0;

        dA      = (dY2 -  dY1) / (dX2 - dX1);
        dB      =  dY1 -         (dA  * dX1);
        dLinear = (dA  *  dXi) +  dB        ;

        return dLinear;
    }
    //---------------------------------------------------------------------------
    internal static double GetTheta (double dDist1, double dDist2)
    {//
        //
        double radT   = 0.0;
        double dTheta = 0.0;

        radT =  Math.Atan(dDist2 / dDist1);
        return dTheta = (radT * 180.0) / Math.PI;
    }
    //---------------------------------------------------------------------------
    internal static double GetTheta (double PosX1, double PosY1, double PosX2, double PosY2)
    {//
        //
		double dX     = PosX2 - PosX1;
		double dY     = PosY2 - PosY1;
        double radT   = 0.0;
        double dTheta = 0.0;

        radT =  Math.Atan2(dY, dX);
        return dTheta = (radT * 180.0) / Math.PI;
    }
    internal static double GetUserYield1(int TotQty , int UserQty)
    {
        if (TotQty  <= 0) return 0.0;
        if (UserQty <= 0) return 100.0;
    
        return (100.0 - ((double)UserQty / (double)TotQty * 100.0));
    }
    //---------------------------------------------------------------------------
    internal static double GetUserYield2(int TotQty , int UserQty)
    {
        if (TotQty  <= 0) return 0.0;
        if (UserQty <= 0) return 0.0;
        
        return ((double)UserQty / (double)TotQty * 100.0);
    }
    internal static bool ChkBetweenNum(float Num , float Min, float Max)
    {
        return Num >= Min && Num <= Max ;
    }
    //--------------------------------------------------------------------------
    internal static byte[] StructToByte(object obj)
    {
        // 구조체에 활당된 메모리의 크기저장
        int Datasize = Marshal.SizeOf(obj);
        // 비관리 메모리 영역에 구조체 크기만큼의 메모리 할당
        IntPtr Buffer = Marshal.AllocHGlobal(Datasize ); //+1

        // 할당된 구조체 객체의 주소 저장
        Marshal.StructureToPtr(obj, Buffer, false);

        // 구조체가 복사될 배열
        Byte[] Data = new Byte[Datasize];

        // 구조체 객체를 배열에 복사
        Marshal.Copy(Buffer, Data, 0, Datasize);

        // 비관리 메모리 영역에 할당한 메모리 해제
        Marshal.FreeHGlobal(Buffer);

        return Data;
    }
    //--------------------------------------------------------------------------
    internal static object ByteToStruct(byte[] data, Type type)
    {

        // 배열의 크기만큼 비관리 메모리 영역에 메모리를 할당
        IntPtr Buffer = Marshal.AllocHGlobal(data.Length);

        // 배열에 저장된 데이터를 위에서 할당한 메모리 영역에 복사
        Marshal.Copy(data, 0, Buffer, data.Length);

        // 복사된 데이터를 구조체 객체로 변환
        object Obj = Marshal.PtrToStructure(Buffer, type);
        //handle.Free();

        // 비관리 메모리 영역에 할당했던 메모리를 해제
        Marshal.FreeHGlobal(Buffer);

        return Obj;
    }
    //--------------------------------------------------------------------------
    internal static char[] GetByteArrayToCharArray(byte[] btArray, int size)
    {
        char[] charTemp = new char[8192];

        for (int i = 0; i < size; i++)
            charTemp[i] = Convert.ToChar(btArray[i]);
        return charTemp;
    }
    //--------------------------------------------------------------------------
    internal static byte[] GetCharArrayToByteArray(char[] charArray, int size)
    {
        byte[] byteTemp = new byte[size];
        for (int i = 0; i < size; i++)
            byteTemp[i] = Convert.ToByte(charArray[i]);

        return byteTemp;
    }
    //--------------------------------------------------------------------------
    internal static char[] GetStringToCharArray(string str, int iSize = -1)
    {
        if(iSize<0) iSize = str.Length;
        if(iSize<=0) iSize = 1;

        char[] charTemp = new char[iSize];

        for (int i = 0; i < str.Length; i++)
            charTemp[i] = str[i];
        return charTemp;
    }
    //--------------------------------------------------------------------------
    internal static byte[] GetStringToByteArray(string str, int iSize = -1)
    {
        if(iSize<0) iSize = str.Length;
        if(iSize<=0) iSize = 1;
        byte[] ByteTemp = new byte[iSize];

        for (int i = 0; i < iSize; i++)
        {
            if (i >= str.Length) { 
                ByteTemp[i] = 0x20; 
                continue;
                }
            ByteTemp[i] = Convert.ToByte(str[i]);
        }
        return ByteTemp;
    }
    //--------------------------------------------------------------------------
    internal static void GetStringToCharArray(string str, int size, ref char[] charTemp)
    {
        //char[] charTemp = new char[size];

        for (int i = 0; i < str.Length; i++)
            charTemp[i] = str[i];
        //return charTemp;
    }
    //--------------------------------------------------------------------------
    internal static string GetByteArrayToString(byte[] btArray, int Strt, int size)
    {
        string StrTemp  = "";
        for (int i = 0; i < size; i++) 
        {
            StrTemp += Convert.ToChar(btArray[Strt+i]);
        }

        return StrTemp.Trim();
    }
    //------------------------------------------------------------------------
    internal static string GetByteArrayToHexString(byte[] btArray, int Strt, int size)
    {
        string StrTemp = "";
        for (int i = 0; i < size; i++)
        {
            StrTemp += (btArray[Strt + i].ToString("X2") + " ");
        }
        return StrTemp.Trim();
    }

    //--------------------------------------------------------------------------
    internal static string GetCharArrayToString(char[] charArray, int Strt, int size)
    {
        String StrTemp = "";
        for (int i = 0; i < size; i++) {
            StrTemp += charArray[Strt+i];
            }
        return StrTemp.Trim();
    }

    //--------------------------------------------------------------------------
    internal static int GetByteArrayToInt(byte[] btArray, int Strt, int size, bool isHex = false)
    {
        String StrTemp = "";
        int    RetVal  = 0;
        for (int i = 0; i < size; i++) {
            StrTemp += Convert.ToChar(btArray[Strt+i]);
            }

        if(isHex) RetVal =  int.Parse(StrTemp, System.Globalization.NumberStyles.HexNumber); 
        else      RetVal =  Int32.Parse(StrTemp);   

        return RetVal;
    }
    //--------------------------------------------------------------------------
    internal static double GetByteArrayToDouble(byte[] btArray, int Strt, int size)
    {
        String StrTemp = "";
        for (int i = 0; i < size; i++) {
            StrTemp += Convert.ToChar(btArray[Strt+i]);
            }
        return double.Parse(StrTemp);
    }
    //------------------------------------------------------------------------
    public static string ConvertToChar(string str, int len)
    {
        //Local Var.
        int iCnt;
        int iRxLen;
        //byte btDat;
        int  iVal;    

        string sTemp3;
        string sConvMsg = "";
        string sRxMsg = str + "          "; //밑에서 데이터 나눌때 수량부족 방지를 위해 추가 Space.
        byte[] temp_wr = new byte[1024];

        //Get Len.
        iRxLen = sRxMsg.Length;
        if (iRxLen <= 0   ) return "";
        if (iRxLen >= 1024) return "";

        //Converting...
        try
        { 
            iCnt = 0;
            for (int n = 0; n < iRxLen; n += 2)
            {
                sTemp3 = StrToHex(sRxMsg.Substring(n, 2).Trim(), true);
                if (sTemp3 == "") continue;
                iVal = Convert.ToInt32(sTemp3, 16);
                temp_wr[iCnt++] = Convert.ToByte(iVal); //(btDat == 0) ? (byte)' ' : btDat;
            }
        }
        catch (Exception e) { System.Diagnostics.Debug.WriteLine("Exception:" + e.Message); }

        sConvMsg = FNC.GetByteArrayToString(temp_wr, 0, iRxLen);

        //
        return sConvMsg;

    }
    //------------------------------------------------------------------------
    public static T StrToStruct<T>(string str) where T : struct
    {
        byte[] buffer = Encoding.ASCII.GetBytes(str);
        IntPtr ptr    = Marshal.AllocHGlobal(buffer.Length);
        
        Marshal.Copy(buffer, 0, ptr, buffer.Length);
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        
        return obj;
    }
    //------------------------------------------------------------------------
    public static string StrToHex(string strData, bool OnlyOne = false)
    {
        string resultHex = string.Empty;
        if (OnlyOne)
        {
            resultHex += string.Format("{0:X2}", strData);
        }
        else
        { 
            
            byte[] arr_byteStr = Encoding.Default.GetBytes(strData);
     
            foreach (byte byteStr in arr_byteStr)
                resultHex += string.Format("{0:X2}", byteStr);
        }
     
        return resultHex;
    }
    internal static string GetStrBuffToHead(string sHead,  string sBuff)
    {//
        
        string  Body = "";
        string  Temp;
        int     iBuffLen; 
        sHead   = sHead.Trim().ToUpper()+"=";
        sBuff   = sBuff.Trim().ToUpper();
        iBuffLen= sBuff.Length;

        int  n = sBuff.IndexOf(sHead);
     
        if(n<=0) return " ";
        n = n + sHead.Length;
        for (int i=n; i<=iBuffLen; i++) {
	        Temp = sBuff.Substring(i, 1);
            if(Temp!=" " && Temp!= ")") {
               Body += Temp;
            }
            else {
                return Body.Trim();
            }
        }
        return Body.Trim();
    }
    public static int TextToNumber(string text)
    {
        int n = 0;
        int sum = 0;
        int iLength = text.Length;
        char ch;

        for (n = 0; n < iLength; n++)
        {
             ch = text[n];
             sum = sum * 26 + ch - 'A' + 1;
        }
        return sum;
    }
    public static string str2hex(string strData)
    {
        string resultHex = string.Empty;
        byte[] arr_byteStr = Encoding.Default.GetBytes(strData);
     
        foreach (byte byteStr in arr_byteStr)
            resultHex += string.Format("{0:X2}", byteStr);
     
        return resultHex;
    }
	public static int ConvertASCIIToInt(string sTemp)
	{   //4Byte까지만 변경 가능
		int    iRet;
		Byte[] bytes = Encoding.ASCII.GetBytes(sTemp);

		if (bytes.Length < sizeof(int))
		{
			Array.Resize(ref bytes, sizeof(int));
		}
		//if (BitConverter.IsLittleEndian)
		//	Array.Reverse(bytes);

		iRet = BitConverter.ToInt32(bytes, 0);

		return iRet;
	}
	public static string ConvertIntToAscIIStr(int val)
	{   //4Byte까지만 변경 가능

        Byte[] bytes = BitConverter.GetBytes(val);

		return Encoding.Default.GetString(bytes);
	}          

    //--------------------------------------------------------------------------
    public static double Hypot(double dx, double dy) { return Math.Sqrt(dx * dx + dy * dy); }
    public static double Distance(double x1, double y1, double x2, double y2)
    {
        return Hypot(x2 - x1, y2 - y1);
    }
    //--------------------------------------------------------------------------
    public static bool CreateDir(String Src)
    {
        if(Src == null ) return false;
        if(Src == ""   ) return false;

        DirectoryInfo di = new DirectoryInfo(Src);
        if (!di.Exists) di.Create();
        di = null;
        return true;
    }
    //--------------------------------------------------------------------------
    public static bool DeleteDir(String Src)
    {
        if(Src == null ) return false;
        if(Src == ""   ) return false;

        DirectoryInfo di = new DirectoryInfo(Src);
        if (!di.Exists) return false;
        di.Delete(true);
        di = null;
        return true;
    }
    //--------------------------------------------------------------------------
    public static bool CopyDir(String Src, String Dest, bool isCopySub = true)
    {
        if(Src == null ) return false;
        if(Src == ""   ) return false;
        if(Dest == null) return false;
        if(Dest == ""  ) return false;

        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(Src);

        if (!dir.Exists)  return false;
        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(Dest))
        {
            Directory.CreateDirectory(Dest);
        }
        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(Dest, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (isCopySub)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(Dest, subdir.Name);
                CopyDir(subdir.FullName, temppath, isCopySub);
            }
        }
        return true;
    }
    //--------------------------------------------------------------------------
    public static bool MoveDir(String Src, String Dest)
    {
        if(Src == null ) return false;
        if(Src == ""   ) return false;
        if(Dest == null) return false;
        if(Dest == ""  ) return false;

        if (!Directory.Exists(Src )) return false;
        if ( Directory.Exists(Dest)) return false;

        Directory.Move(Src, Dest);
        return true;
    }
    //--------------------------------------------------------------------------
    public static bool CreateDirOnWork(String sPath)
    {
        if(sPath == null) return false;
        if(sPath == ""  ) return false;

        String sWorkPath = Application.StartupPath + "\\" + sPath;
        DirectoryInfo di = new DirectoryInfo(sWorkPath);
		try
		{
			if (!di.Exists) di.Create();
			di = null;           
		}
		catch (Exception e) { System.Diagnostics.Debug.WriteLine("Exception:" + e.Message); }
        return true;
    }
    //--------------------------------------------------------------------------
    public static bool FileExists(String sFileName)
    {
        if(sFileName == null) return false;
        if(sFileName == ""  ) return false;

        FileInfo fi = new FileInfo(sFileName);
        bool bExist =  fi.Exists;
        fi = null;
        return bExist; 
    }
    //--------------------------------------------------------------------------
    public static bool DelDirFrDate(String sPath, DateTime tDelDate)
    {
        if(sPath == null) return false;
        if(sPath == ""  ) return false;

        DirectoryInfo di = new DirectoryInfo(sPath);
        if (!di.Exists) return false;

        foreach(FileInfo file in di.GetFiles()) 
        {
            if(file.CreationTime<tDelDate)
            {
                file.Delete();
            }
        }

        DirectoryInfo[] Subdis = di.GetDirectories();
        foreach(DirectoryInfo Subdi in Subdis) 
        {
            if(Subdi.CreationTime<tDelDate)
            {
                Subdi.Delete(true);
            }
        }

        Subdis = null;
        di     = null;
        return true;
    }
    //---------------------------------------------------------------------------
    public static bool FileDelete(String sFileName)
    {
        if(sFileName == null) return false;
        if(sFileName == ""  ) return false;

        FileInfo fi = new FileInfo(sFileName);
        bool bExist =  fi.Exists;
        if(!bExist) return false;
        fi.Delete();
        return true; 
    }
    //---------------------------------------------------------------------------
    public static void DeleteOldFilesWithinThreshold(string folderPath, int maxFolderSize, double thresholdPercentage)
    { //지정 사이즈는 GB단위로 ....
        DirectoryInfo directory = new DirectoryInfo(folderPath);
        long folderSize = GetDirectorySize(directory);
        //thresholdPercentage = 0.10; //10%
        //1000000000 == 1GB
        long maxSize = (long)maxFolderSize * 1000000000;
        long thresholdSize = (long)(maxFolderSize * (1 - thresholdPercentage)) * 1000000000;

        if (folderSize > maxSize)
        {
            var files = directory.GetFiles("*", SearchOption.AllDirectories)
                                 .OrderBy(f => f.LastWriteTime)
                                 .ToList();

            foreach (var file in files)
            {
                folderSize -= file.Length;
                file.Delete();
                Console.WriteLine($"Deleted: {file.FullName}");
            
                if (folderSize <= thresholdSize)
                {
                    break;
                }
            }
        }
    }
    //--------------------------------------------------------------------------
    public static long GetDirectorySize(DirectoryInfo directory)
    {
        return directory.GetFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
    }
    //--------------------------------------------------------------------------
    public static int UpdateFileByGrid(String sPath, ref System.Windows.Forms.DataGridView Grid, Color backColor, bool isSort = false, bool isDispDate = false, string sMask = "", bool isViewMask = true)
    {
        if (sPath == null) return 0;
        if (sPath == "") return 0;

        int iTotWidth = 0;
        int iFndCnt = 0;
        int[] iWidth = { 40, 0, 200 };
        String[] sItem = { "NO", "DESC", "DATE" };

        String[] sName = new String[200];
        DateTime[] tDate = new DateTime[200];

        String tmpName;
        DateTime tmpDate;

        Grid.Dock = System.Windows.Forms.DockStyle.Fill;
        SetGridStyle(ref Grid, 30, true);
        Grid.BackgroundColor = backColor; //Color.FromArgb(66, 72, 88);

        //
        for (int i = 0; i < 3; i++)
        {
            if (i == 2 && !isDispDate) continue;
            Grid.Columns.Add(sItem[i], sItem[i]);
            Grid.Columns[i].Width = iWidth[i];
            iTotWidth += iWidth[i];
        }

        Grid.Columns[1].Width = Grid.Width - iTotWidth - 20;

        DirectoryInfo di = new DirectoryInfo(sPath);
        if (!di.Exists) return 0;

        foreach (FileInfo file in di.GetFiles(sMask))
        {
            if (iFndCnt >= 200) break;
            //if(sMask != "")
            //{ 
            //    if(file.Name.IndexOf(sMask) <= 0) continue;
            //}
            string sname = file.Name.Replace(sMask, string.Empty);
            if (isViewMask) sName[iFndCnt] = file.Name;
            else            sName[iFndCnt] = Path.ChangeExtension(file.Name, null);
            
            tDate[iFndCnt] = file.CreationTime;
            //tDate[iFndCnt] = file.LastWriteTime;
            iFndCnt++;
        }
        di = null;

        if (iFndCnt <= 0) return iFndCnt;

        //Sorting.
        if (isSort)
        {
            for (int i = 0; i < iFndCnt; i++)
            {
                for (int j = i; j < iFndCnt; j++)
                {
                    if (tDate[i] < tDate[j])
                    {
                        tmpName = sName[i]; sName[i] = sName[j]; sName[j] = tmpName;
                        tmpDate = tDate[i]; tDate[i] = tDate[j]; tDate[j] = tmpDate;
                    }
                }
            }
        }

        //Display.
        for (int i = 0; i < iFndCnt; i++)
        {
            sItem[0] = Convert.ToString(i + 1);
            sItem[1] = sName[i];
            sItem[2] = String.Format("{0:yy/MM/dd HH:mm:ss}", tDate[i]);
            Grid.Rows.Add(sItem);
        }

        Grid.Visible = true;

        //cDEF.POSN.SetGridFont(ref Grid);

        //Return.
        return iFndCnt;
    }
    //--------------------------------------------------------------------------
    public static int UpdateDirByGrid(String sPath, ref System.Windows.Forms.DataGridView Grid, Color BackColor, bool isSort = false, bool isDispDate = false)
    {
        if(sPath == null) return 0;
        if(sPath == ""  ) return 0;

        int iTotWidth   = 0;
        int iFndCnt     = 0;
        int[]    iWidth = {40, 0, 200};
        String[] sItem  = {"NO", "DESC", "DATE"};

        String[]   sName = new String  [200];
        DateTime[] tDate = new DateTime[200];
        
        String     tmpName;
        DateTime   tmpDate;

        FNC.SetGridStyle(ref Grid, 50, true, true, false, DataGridViewSelectionMode.FullRowSelect);

        Grid.Dock                                      = System.Windows.Forms.DockStyle.Top;
        Grid.CurrentCell                               = null;
        Grid.MultiSelect                               = false;
        Grid.BackgroundColor                           = BackColor; //Color.FromArgb(66, 72, 88);
        Grid.ColumnHeadersBorderStyle                  = DataGridViewHeaderBorderStyle.Single;
        Grid.CellBorderStyle                           = DataGridViewCellBorderStyle.Single;
        Grid.RowsDefaultCellStyle.BackColor            = Color.WhiteSmoke;//Color.FromArgb(204,255,255);
        Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke; //Color.FromArgb(173,255,255);
        Grid.Font                                      = new Font("Century Gothic", 12, FontStyle.Bold   );
        Grid.DefaultCellStyle.Font                     = new Font("Century Gothic", 12, FontStyle.Bold   );

        for(int i=0;i<3;i++) 
        {
            if(i==2 && !isDispDate) continue;
            Grid.Columns.Add(sItem[i] , sItem[i]);
            Grid.Columns[i].Width = iWidth[i];
            iTotWidth += iWidth[i];
        }
        Grid.Columns[1].Width = Grid.Width - iTotWidth-20;
        
        DirectoryInfo di = new DirectoryInfo(sPath);
        foreach(DirectoryInfo dir in di.GetDirectories()) 
        {
            if(iFndCnt>=200) break;

            if(dir.Name.ToUpper() == "COMMON") continue;
            if(dir.Name.ToUpper() == "NONE"  ) continue;

            sName[iFndCnt] = dir.Name        ;
            tDate[iFndCnt] = dir.CreationTime;                        
            iFndCnt ++;
        } 
        di = null;

        if(iFndCnt <= 0 ) return iFndCnt;
         //Sorting.
         if (isSort) {
             for (int i = 0 ; i < iFndCnt ; i++) {
                 for (int j = i ; j < iFndCnt ; j++) {
                     if (tDate[i]<tDate[j]) {
                         tmpName = sName[i]; sName[i] = sName[j]; sName[j] = tmpName;
                         tmpDate = tDate[i]; tDate[i] = tDate[j]; tDate[j] = tmpDate;
                         }
                     }
                 }
             }

         //Display.
         for (int i=0; i<iFndCnt; i++) {
             sItem[0] = Convert.ToString(i + 1);
             sItem[1] = sName[i];
             sItem[2] = String.Format("{0:yy/MM/dd HH:mm:ss}", tDate[i]);
             Grid.Rows.Add(sItem);
             }

         Grid.Visible = true;
         //Return.
         return iFndCnt;
    }
    //--------------------------------------------------------------------------
    public static void ExportGrid(ref System.Windows.Forms.DataGridView pGrid, string sFileName)
    {
        String sPath;
        String sData = "";

        string sFile = "[" + string.Format("{0:yyMMdd}", DateTime.Now) + "]" + sFileName + ".csv";
        //Make Dir.
        FNC.CreateDirOnWork("Export");
        sPath = Application.StartupPath + "\\Export\\" + sFile;

        try {
            //File Open.
            FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fp, Encoding.Default);
            sw.BaseStream.Seek(0, SeekOrigin.End);

            //Set List.
            for (int c = 0; c < pGrid.ColumnCount; c++)
            {
                sData += pGrid.Columns[c].Name;
                if (c < pGrid.ColumnCount - 1) sData += ",";
            }
            sw.WriteLine(sData);
            sData = "";
            for (int r = 0; r < pGrid.RowCount; r++)
            {
                for (int c = 0; c < pGrid.ColumnCount; c++)
                {
                    sData += pGrid[c, r].Value.ToString();
                    if (c < pGrid.ColumnCount - 1) sData += ",";
                }
                sw.WriteLine(sData);
                sData = "";
            }

            sw.Flush();
            sw.Close();

        }
        catch (Exception ex)
        {
            //cDEF.LOG.ExceptionTrace("FNC. ExportGrid " + ex.ToString());
            System.Diagnostics.Debug.WriteLine("FNC. ExportGrid:" + ex.Message);
        }  
    }
    //------------------------------------------------------------------------
    public static void SetGridStyle(ref System.Windows.Forms.DataGridView pGrid,
                                    int iRowHeight = 30         , bool bReadOnly        = false, 
                                    bool bCHeadersVisible = true, bool bRHeadersVisible = false,
                                    DataGridViewSelectionMode iSelMode = DataGridViewSelectionMode.CellSelect)

    {
        pGrid.Visible                                  = false ;
        pGrid.ReadOnly                                 = bReadOnly;
        pGrid.Enabled                                  = true  ;
        pGrid.AllowUserToAddRows                       = false ;
        pGrid.AllowUserToDeleteRows                    = false ;
        pGrid.AllowUserToOrderColumns                  = false ;
        pGrid.AllowUserToResizeColumns                 = false ;
        pGrid.AllowUserToResizeRows                    = false ;
        pGrid.ColumnHeadersVisible                     = bCHeadersVisible;
        pGrid.RowHeadersVisible                        = bRHeadersVisible;  
        pGrid.MultiSelect                              = true   ;
        pGrid.SelectionMode                            = iSelMode;
        pGrid.EditMode                                 = DataGridViewEditMode           .EditOnEnter  ;
        pGrid.AutoSizeColumnsMode                      = DataGridViewAutoSizeColumnsMode.None         ;
        //pGrid.AutoSizeRowsMode                         = DataGridViewAutoSizeRowsMode   .None         ;

        pGrid.BackgroundColor = Color.Snow;
        pGrid.Columns.Clear();
        pGrid.Font                                     = new System.Drawing.Font("Century Gothic", 11, FontStyle.Regular);
        pGrid.DefaultCellStyle.Font                    = new System.Drawing.Font("Century Gothic", 11);
        pGrid.ColumnHeadersDefaultCellStyle.Font       = new System.Drawing.Font("Century Gothic", 11);
        pGrid.RowHeadersDefaultCellStyle.Font          = new System.Drawing.Font("Century Gothic", 11);
        pGrid.DefaultCellStyle.ForeColor               = System.Drawing.Color.Black; 
        pGrid.ColumnHeadersDefaultCellStyle.BackColor  = System.Drawing.SystemColors.InactiveCaption;
        pGrid.RowHeadersDefaultCellStyle.BackColor     = System.Drawing.SystemColors.InactiveCaption;
        pGrid.EnableHeadersVisualStyles                = false;
        pGrid.RowTemplate.Height                       = iRowHeight;

        pGrid.CurrentCell = null;

		if (bCHeadersVisible) pGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
		if (bRHeadersVisible) pGrid.RowHeadersDefaultCellStyle   .Alignment = DataGridViewContentAlignment.MiddleCenter;
        
    }
    //------------------------------------------------------------------------
    public static void SetGridAlignRowHeight(ref System.Windows.Forms.DataGridView pGrid)
    {
        for (int n = 0; n < pGrid.Rows.Count; n++) pGrid.Rows[n].Height = (pGrid.Height - pGrid.ColumnHeadersHeight) / pGrid.Rows.Count;
    }
    //------------------------------------------------------------------------
    public static void SetGridAlignColWidth(ref System.Windows.Forms.DataGridView pGrid)
    {
        for (int n = 0; n < pGrid.Columns.Count; n++) pGrid.Columns[n].Width = (pGrid.Width/ pGrid.Columns.Count) - 1;
    }
    //------------------------------------------------------------------------
    public static void SetGridRowColWidthHeight(ref System.Windows.Forms.DataGridView pGrid)
    {
        SetGridAlignRowHeight(ref pGrid);
        SetGridAlignColWidth (ref pGrid);
    }

    public static bool IsTheSameCellValue(ref System.Windows.Forms.DataGridView dgv, int column, int row, int TargetCol = 0) 
    {
      int iCellTop = row;
      int iCellBtm = row -1;

      if(column   != TargetCol   ) return false; //0번 Column만 적용 
      if(iCellTop >= dgv.RowCount) return false;
      if(iCellBtm >= dgv.RowCount) return false;
      if(iCellTop <  0           ) return false;
      if(iCellBtm <  0           ) return false;

      System.Windows.Forms.DataGridViewCell cell1 = dgv[column , iCellTop];
      System.Windows.Forms.DataGridViewCell cell2 = dgv[column , iCellBtm];

      if ((cell1.Value == null)  || (cell2.Value == null) ) return false;
      
	  return cell1.Value.ToString() == cell2.Value.ToString();
    }

    public static void SameCellFormatting(ref System.Windows.Forms.DataGridView pGrid, DataGridViewCellFormattingEventArgs e, int TargetCol = 0)
    {
        if (e.RowIndex < 0) return;

        if (IsTheSameCellValue(ref pGrid, e.ColumnIndex, e.RowIndex, TargetCol))
        {
            e.Value = "";
            e.FormattingApplied = true;
        }
    }

    public static void SameCellPainting(ref System.Windows.Forms.DataGridView pGrid, DataGridViewCellPaintingEventArgs e, int TargetCol = 0)
    {
        if (e.RowIndex < 1 && e.ColumnIndex < 0) return;
        else e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
        //
        if (IsTheSameCellValue(ref pGrid, e.ColumnIndex, e.RowIndex, TargetCol))
        {
            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
        }
        else
        {
            e.AdvancedBorderStyle.Top = pGrid.AdvancedCellBorderStyle.Top;
        }
        //
        if (e.RowIndex >= pGrid.RowCount - 1)
        {
            e.AdvancedBorderStyle.Bottom = pGrid.AdvancedCellBorderStyle.Top;
            return;
        }
    }
    public static void SameCellColor(ref System.Windows.Forms.DataGridView pGrid, int ChgEndCol, Color FrstColor, Color ScndColor)
    {
        bool colorSet = false;
        for (int index = 0; index < pGrid.RowCount; index++)
        {
            //if (index == 0) continue;
            //
            if (!IsTheSameCellValue(ref pGrid, 0, index)) colorSet = !colorSet;
            //
            for (int c = 0; c < pGrid.ColumnCount; c++)
            {
                if (c > ChgEndCol) break;
                pGrid[c, index].Style.BackColor = colorSet ? FrstColor : ScndColor;
            }
        }
    }
    public static void ShowSubMenu(ref System.Windows.Forms.DataGridView pGrid, Color BackColor, params object[] args)
    {
        String sPName      ;
        int    iRowCnt  = args.Length;  
        if(pGrid == null) return;
        int iRowHeight = 50;

        SetGridStyle(ref pGrid, iRowHeight, true, false, false, DataGridViewSelectionMode.FullRowSelect);
        pGrid.Dock = System.Windows.Forms.DockStyle.Top;
        pGrid.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
        pGrid.BackgroundColor = BackColor; //Color.FromArgb(66, 72, 88);

        
        pGrid.DefaultCellStyle.ForeColor = Color.Black;
        pGrid.DefaultCellStyle.BackColor = Color.FromArgb(153, 153, 153);

        DataGridViewButtonColumn btnPart = new DataGridViewButtonColumn();  //버튼 추가
        btnPart.HeaderText = "";
        btnPart.Name = "btnPart";  
        btnPart.FlatStyle =  FlatStyle.Flat;
        

        //
        pGrid.Columns.Add(btnPart );
        pGrid.Columns[0].Width = pGrid.Width-1;

        for(int i=0;i<iRowCnt; i++)
        {
            sPName   = Convert.ToString(args[i]).Trim();
            pGrid   .Rows.Add(sPName); 
        }

        pGrid.Height = pGrid.RowCount * iRowHeight + 5; 

        pGrid.Visible          = true;
        pGrid.Rows[0].Selected = true;

		for (int index = 0; index < pGrid.RowCount; index++)
		{
			//
			pGrid[0, index].Style.Font = new Font("Century Gothic", 13, FontStyle.Bold);
		}
    }
    public static int ConvInt(string sData, int iNor = 0)
    {
        int    iData  = iNor;
        if(!int.TryParse(sData, out iData)) return 0;
        return iData;
    }
    public static double ConvDbl(string sData, int iNor = 0)
    {
        double dData  = iNor;
        if(!double.TryParse(sData, out dData)) return 0.0;
        return dData;
    }
    public static string ConvStr(object sData, int iDigit = 0)
    {
        string sVal = Convert.ToString(sData);
        if(sData == null) return "";
        if(sData.GetType().Name == "Double")
        {
                 if(iDigit == 1) sVal = String.Format("{0:F1}" , sData);
            else if(iDigit == 2) sVal = String.Format("{0:F2}" , sData);
        } 
        return sVal;
    }
    public static string ConvBStr(Byte sData)
    {
        string sVal = "";
        sVal +=  Convert.ToChar(sData);
        return sVal;
    }
    public static void SetDoubleBuffered(Control control)
    {
        // if not remote desktop session then enable double-buffering optimization
        if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                                         System.Reflection.BindingFlags.SetProperty |
                                         System.Reflection.BindingFlags.Instance    |
                                         System.Reflection.BindingFlags.NonPublic,
                                         null,
                                         control,
                                         new object[] { true });
        }
    }

    //[DllImport("user32.dll")]
    //public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);    
    private const int WM_SETREDRAW = 11;
    /// Suspend drawing updates for the specified control. After the control has been updated
    /// call DrawingControl.ResumeDrawing(Control control).
    public static void SuspendDrawing(Control control)
    {
        WinAPI.SendMessage(control.Handle, WM_SETREDRAW, 0, 0);
    }
    /// Resume drawing updates for the specified control.
    public static void ResumeDrawing(Control control)
    {
    WinAPI.SendMessage(control.Handle, WM_SETREDRAW, 1, 0);
        control.Refresh();
    }
    public static void ClearPictureBox(ref PictureBox pb, Color BackColor)
    {
        //
        pb.Image = null;
        pb.BackColor = BackColor;
        //Bitmap bmp = new Bitmap(pb.Width, pb.Height);
        //Graphics g = Graphics.FromImage(bmp);
		//
		//g.Clear(BackColor);

		//if (g     != null) g    .Dispose();
        //SolidBrush brush = new SolidBrush(Color.Black);
        //g.FillRectangle(brush, 0, 0, pb.Width, pb.Height);
        //pb.Image = bmp;
        //if (brush != null) brush.Dispose();
    }
    public static void DrawRect(ref Graphics g, int x1, int y1, int x2, int y2, Color PenColor, Color? BrushColor = null)
    {
        
        Pen p = new Pen(PenColor);
        g.DrawRectangle(p, x1, y1, x2, y2);
        p = null;
        if(BrushColor != null) {
            Brush brush = new SolidBrush((Color)BrushColor);            
            g.FillRectangle(brush, x1+1, y1+1, x2-1, y2-1);
            brush = null;
            }            
    }

    public static void DrawEllipse(ref Graphics g, int x1, int y1, int x2, int y2, Color PenColor, Color? BrushColor = null)
    {
        
        Pen p = new Pen(PenColor);
        g.DrawEllipse(p, x1, y1, x2, y2);
        p = null;
        if(BrushColor != null) {
            Brush brush = new SolidBrush((Color)BrushColor);            
            g.FillEllipse(brush, x1+1, y1+1, x2-2, y2-2);
            brush = null;
            }
        
    }
    public static void DrawText(ref Graphics g, int x1, int y1, Color PenColor, string sText, int size = 6)
    {
        //Font myFont = new Font("Small Fonts", size);
        Font myFont = new Font("Arial", size);
        Brush brush = new SolidBrush(PenColor);
        g.DrawString(sText, myFont, brush, new Point(x1, y1));
        myFont = null;
    }

	// 1. Deep Clone 구현
    public static T DeepClone<T>(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException("Object cannot be null.");

        return (T)Process(obj, new Dictionary<object, object>() { });
    }

	private static object Process(object obj, Dictionary<object, object> circular)
    {
        if (obj == null)
            return null; 

        Type type = obj.GetType(); 

		if (type == null) return null;

        if (type.IsValueType || type == typeof(string))
        {
            return obj;
        } 

        if (type.IsArray)
        {
            if (circular.ContainsKey(obj))
                return circular[obj];

			int iPos = type.FullName.IndexOf("[");
			string typeNoArray = type.FullName.Substring(0, iPos);
            //string typeNoArray = type.FullName.Replace("[]", string.Empty);	
            Type elementType = Type.GetType(typeNoArray + ", " + type.Assembly.FullName);
            var array = obj as Array;

            Array arrCopied;
			if      (array.Rank == 1) arrCopied = Array.CreateInstance(elementType, array.Length);
			else if (array.Rank == 2) arrCopied = Array.CreateInstance(elementType, array.GetLength(0), array.GetLength(1));
			else if (array.Rank == 3) arrCopied = Array.CreateInstance(elementType, array.GetLength(0), array.GetLength(1), array.GetLength(2));
			else return null;				

            circular[obj] = arrCopied; 

			if (array.Rank == 1) //1차원 배열
			{
				for (int i = 0; i < array.Length; i++)
				{
					object element = array.GetValue(i);
					object objCopy = null; 
				
					if (element != null && circular.ContainsKey(element))
					    objCopy = circular[element];
					else
					    objCopy = Process(element, circular); 
				
					arrCopied.SetValue(objCopy, i);
				}
			}
			else if (array.Rank == 2) //2차원 배열
			{
				for (int i = 0; i < array.GetLength(0); i++)
				{
					for (int j = 0; j < array.GetLength(1); j++)
					{
						object element = array.GetValue(i, j);
						object objCopy = null; 
					
						if (element != null && circular.ContainsKey(element))
						    objCopy = circular[element];
						else
						    objCopy = Process(element, circular); 
					
						arrCopied.SetValue(objCopy, i, j);
					}
				}
			}
			else if (array.Rank == 3) //3차원 배열까지만.
			{
				for (int i = 0; i < array.GetLength(0); i++)
				{
					for (int j = 0; j < array.GetLength(1); j++)
					{
						 for (int k = 0; k < array.GetLength(2); k++)
						 {
						 	  object element = array.GetValue(i, j, k);
						 	  object objCopy = null; 
						 	  
						 	  if (element != null && circular.ContainsKey(element))
						 	      objCopy = circular[element];
						 	  else
						 	      objCopy = Process(element, circular); 
						 	  
						 	  arrCopied.SetValue(objCopy, i, j, k);
						 }
					}
				}
			}
			else return null;

            return Convert.ChangeType(arrCopied, obj.GetType());
        } 

        if (type.IsClass)
        {
            if (circular.ContainsKey(obj))
                return circular[obj]; 

            object objValue = Activator.CreateInstance(obj.GetType()); //Class 생성자에 매개변수가 있을때는 안됨. 매개변수 없는 생성자 만들어야 함.
            circular[obj] = objValue;
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); 

            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(obj);

                if (fieldValue == null)
                    continue; 

                object objCopy = circular.ContainsKey(fieldValue) ? circular[fieldValue] : Process(fieldValue, circular);
                field.SetValue(objValue, objCopy);
            }

            return objValue;
        }
        else
            throw new ArgumentException("Unknown type");
    } 

    // 2. Serializable 객체에 대한  Deep Clone
    public static T SerializableDeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var bformatter = new BinaryFormatter();
            bformatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T) bformatter.Deserialize(ms);
        }
    }
	
	//
	public static Form FormDynamicCreate(string FrmName)
	{
		Assembly asm   = Assembly.GetEntryAssembly();
		string asmName = asm.GetName().Name.Substring(0, asm.GetName().Name.Length - 2); //"eMachine"; //			
		Type type  = Type.GetType(string.Format("{0}.{1}",asmName , FrmName));
		Object obj = Activator.CreateInstance(type);
		
		return (Form)obj;
	}
	public static void DrawGradation(PaintEventArgs e, Color StrtColor, Color EndColor, LinearGradientMode GMode)
	{
		Graphics g = e.Graphics;
		Rectangle rect = e.ClipRectangle;
		LinearGradientBrush lgb = new LinearGradientBrush(rect, StrtColor, EndColor, GMode);
		ColorBlend cb = new ColorBlend();
		cb.Colors = new Color[] { StrtColor, EndColor };
		cb.Positions = new Single[] {0.0F, 1.0F};
		lgb.InterpolationColors = cb;
		
		g.FillRectangle(lgb, rect);
		lgb.Dispose();
		g.Dispose();
	}
	public static void DrawOpacity(PaintEventArgs e)
	{
		Color c = Color.FromArgb(150, Color.LightPink);
		using (Brush b = new SolidBrush(c))
		{
		     e.Graphics.FillRectangle(b, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
		}
	}

	//Control 모서리 둥글게
	[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
	private static extern IntPtr CreateRoundRectRgn(int nLeftRect,
      int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

	[DllImport("user32.dll")] 
	private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw); 


	public static void DrawBtnEdgeRound(Button Btn, int EllipseVal)
	{
		if (EllipseVal <= 0) EllipseVal = 1;
		Btn.FlatAppearance.BorderSize = 0;
		Btn.FlatStyle = FlatStyle.Flat;
		Btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Btn.Width, Btn.Height, EllipseVal, EllipseVal));
	}
	public static void DrawCtrlEdgeRound(Control Ctrl, int EllipseVal, Color backColor)
	{
		string sCtrlName = Ctrl.GetType().Name;
		if (sCtrlName == "Button")
		{
			Button Btn = new Button();
			Btn = Ctrl as Button;
			if (Btn == null) return;
			Btn.BackColor = backColor;
		}
		else if (sCtrlName == "TextBox")
		{ //Display용으로 사용.
			TextBox tb = new TextBox();
			tb = Ctrl as TextBox;
			if (tb == null) return;
			tb.Enabled = false;
			tb.TabStop = false;
			tb.TextAlign = HorizontalAlignment.Center;
			tb.BackColor = backColor;
		}
		else if (sCtrlName == "Panel")
		{ //Display용으로 사용.
			Panel Panel = new Panel();
			Panel = Ctrl as Panel;
			Panel.BackColor = backColor;
		}

		if (EllipseVal <= 0) EllipseVal = 1;			
		IntPtr ip = CreateRoundRectRgn(0, 0, Ctrl.Width, Ctrl.Height, EllipseVal, EllipseVal); 
		int i = SetWindowRgn(Ctrl.Handle, ip, true);

		Ctrl.Refresh(); 
	}

	public static bool IsParseInt(object obj)
	{
		int parse = -1;
		try
		{
			parse = Int32.Parse(obj.ToString());
			return true;
		}
		catch
		{
			Int32.TryParse(obj.ToString(), out parse);
			return (parse == -1 || parse == 0) ? false : true;
		}
	} 
	public static bool IsParsedouble(object obj)
	{
		double parse = -1;
		try
		{
			parse = Double.Parse(obj.ToString());
			return true;
		}
		catch
		{
			Double.TryParse(obj.ToString(), out parse);
			return (parse == -1 || parse == 0) ? false : true;
		}
	} 

	public static bool ParseInt(object obj, int refVal, out int Val)
	{
		int parse = -1;
		try
		{
			parse = Int32.Parse(obj.ToString());
			Val = parse;
			return true;
		}
		catch
		{
			Int32.TryParse(obj.ToString(), out parse);
			if (parse == -1 || parse == 0) { Val = refVal; return false; }
			else                           { Val = parse ; return true ; }
		}
	} 
	public static bool ParseDouble(object obj, double refVal, out double Val)
	{
		double parse = -1;
		try
		{
			parse = Double.Parse(obj.ToString());
			Val = parse;
			return true;
		}
		catch
		{
			Double.TryParse(obj.ToString(), out parse);
			if (parse == -1 || parse == 0) { Val = refVal; return false; }
			else                           { Val = parse ; return true ; }
		}
	}
    
    public static Bitmap ChangeImgOpacity(Image img, float opacityvalue)
    { //PictureBox Image 투명하게....
        Bitmap bmp = new Bitmap(img.Width, img.Height);
        Graphics graphics = Graphics.FromImage(bmp);
        ColorMatrix colormatrix = new ColorMatrix();
        colormatrix.Matrix33 = opacityvalue;
        ImageAttributes imgAttribute = new ImageAttributes();
        imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
        graphics.Dispose();

        return bmp;
    }
    public static void SetToggleSwRender(ToggleSwitch ts)
    {
        ToggleSwitchCarbonRenderer render = new ToggleSwitchCarbonRenderer();
        render.LeftSideBackColor1  = Color.DarkGray ;
        render.LeftSideBackColor2  = Color.LightGray;
        render.RightSideBackColor1 = Color.DarkGray ;
        render.RightSideBackColor2 = Color.LightGray;
        ts.SetRenderer(render);
    }
    
    public static Control[] GetAllControlsUsingRecursive(Control containerControl)
    {
        List<Control> allControls = new List<Control>();
        foreach (Control ctl in containerControl.Controls)
        {
            allControls.Add(ctl);
            if (ctl.Controls.Count > 0)
            {
                allControls.AddRange(GetAllControlsUsingRecursive(ctl));
            }
        }
        return allControls.ToArray();
    }
    //------------------------------------------------------------------------
    public static void FileBackup(string path)
    {
        //Backup
        if (File.Exists(path))
        {
            string backup = path + ".bak";
            if (File.Exists(backup))
            {
                File.Delete(backup);
            }

            File.Copy(path, backup);
        }
    }

}

