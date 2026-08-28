using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace eMachine
{
    public class cResult : IDisposable
    {
        public bool Using              { get; set; } 

        public Image Src               { get; set; } 
        public Image Overlay           { get; set; } 
        public Image Last              { get; set; } 

        public DateTime StartTime      { get; set; }
        public double GrapTime_ms      { get; set; }
        public double DetectTime_ms    { get; set; }
        public DateTime EndTime        { get; set; }

        public bool IsCompleted        {get; set;}

        public int ErrorNumber         {get; set;}

        public double X_Pixel          { get; set; }
        public double Y_Pixel          { get; set; }
        public double T_Pixel          { get; set; }

        public double X_mm             { get; set; }
        public double Y_mm             { get; set; }
        public double T_mm             { get; set; }

        public double StartPtX         { get; set; }
        public double StartPtY         { get; set; }
        public double StartPtT         { get; set; }

        
        public double AlignCompletePtX { get; set; }
        public double AlignCompletePtY { get; set; }
        public double AlignCompletePtT { get; set; }

        public string Barcode          { get; set; }

        public int Type = 0;
        
        public string sLogTime          { get; set; }
        //--------------------------------------------------------------------------
        public void Clear()
        {
            Using            = false;
            ErrorNumber      = -1;
            
            X_Pixel          = 0.0;
            Y_Pixel          = 0.0;
            T_Pixel          = 0.0;

            X_mm             = 0.0;
            Y_mm             = 0.0;
            T_mm             = 0.0;

            StartPtX         = 0.0;
            StartPtY         = 0.0;
            StartPtT         = 0.0;
            
            AlignCompletePtX = 0.0; 
            AlignCompletePtY = 0.0; 
            AlignCompletePtT = 0.0;

            Barcode          = string.Empty;
            sLogTime         = string.Empty;

            StartTime        = DateTime.Now;
            EndTime          = DateTime.Now;

            IsCompleted      = false;

        }
        //--------------------------------------------------------------------------
        public void Dispose()
        {
            Src    ?.Dispose();
            Last   ?.Dispose();
            Overlay?.Dispose();
        }
        //--------------------------------------------------------------------------
        public void SaveLog()
        {
            Using = true;
            
            try
            {
                StringBuilder sb = new StringBuilder();
                //header = "START TIME\tGrap Time (ms)\tDetect Time (ms)\tEnd Time\tTotal Time (ms)\tBefor Position(X,Y,T)\tAfter Position(X,Y,T)\tResult Pixel(X,Y,T)\tResult(X,Y,T)\tDiscription";
                sb.Append(StartTime    .ToString("yy-MM-dd HH:mm:ss.f")                                                           ); sb.Append("\t");  //START TIME
                sb.Append(GrapTime_ms  .ToString()                                                                                ); sb.Append("\t");  //Grap Time (ms)
                sb.Append(DetectTime_ms.ToString()                                                                                ); sb.Append("\t");  //Detect Time (ms)
                sb.Append(EndTime      .ToString("yy-MM-dd HH:mm:ss.f")                                                           ); sb.Append("\t");  //End Time
                sb.Append((EndTime - StartTime).TotalMilliseconds.ToString()                                                      ); sb.Append("\t");  //Total Time (ms)
                sb.Append($"{Math.Round(StartPtX, 3)},{Math.Round(StartPtY, 3)},{Math.Round(StartPtT, 3)}"                        ); sb.Append("\t");  //Before Position(X,Y,T)
                sb.Append($"{Math.Round(AlignCompletePtX, 3)},{Math.Round(AlignCompletePtY, 3)},{Math.Round(AlignCompletePtT, 3)}"); sb.Append("\t");  //After Position(X,Y,T)
                sb.Append($"{Math.Round(X_Pixel, 3)},{Math.Round(Y_Pixel, 3)},{Math.Round(T_Pixel, 3)}"                           ); sb.Append("\t");  //Result Pixel(X,Y,T)
                sb.Append($"{Math.Round(X_mm   , 3)},{Math.Round(Y_mm   , 3)},{Math.Round(T_mm   , 3)}"                           ); sb.Append("\t");  //Result(X,Y,T)

                if(IsCompleted) sb.Append("Completed"); //Discription
                else
                {
                    int alarm = ErrorNumber;
                    sb.Append(cDEF.EPU.GetName(alarm));
                }

                //
                cDEF.LOG.VisionTrace(sb.ToString(), sLogTime);

                //
                bool useImageSave = cDEF.FM.EngrOptn.bUseImageSave;
                if(useImageSave)
                {
                    string imageSaveDir = cDEF.FM.EngrOptn.sImageSavePath;
                    string dateDir 　　  = $"{DateTime.Now:yy-MM-dd}";
                    string destSaveDir 　= $"{imageSaveDir}\\{dateDir}";

                    if(Directory.Exists(destSaveDir) == false) Directory.CreateDirectory(destSaveDir);

                    ImageFormat saveImageFormat = ImageFormat.Bmp;
                    string fileNameS = $"{StartTime:yyMMddHHmmssf}S";
                    string fileNameO = $"{StartTime:yyMMddHHmmssf}O";
                    string fileNameL = $"{StartTime:yyMMddHHmmssf}L";

                    Src    ?.Save($"{destSaveDir}\\{fileNameS}.{saveImageFormat}",saveImageFormat);
                    Overlay?.Save($"{destSaveDir}\\{fileNameO}.{saveImageFormat}",saveImageFormat);
                    Last   ?.Save($"{destSaveDir}\\{fileNameL}.{saveImageFormat}",saveImageFormat);

                    //
                    int maxSize  = cDEF.FM.EngrOptn.nMaxImageStorage;
                    DeleteOldFile    (imageSaveDir, imageSaveDir,maxSize);
                    DeleteEmptyFolder(imageSaveDir);
                    
                    Debug.WriteLine("LOG COMPLETE");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Using = false;
            }
        }
        //--------------------------------------------------------------------------
        public static void SaveLog(cResult pR)
        {
            pR.Using = true;
            
            try
            {
                StringBuilder sb = new StringBuilder();
                //header = "START TIME\tGrap Time (ms)\tDetect Time (ms)\tEnd Time\tTotal Time (ms)\tBefor Position(X,Y,T)\tAfter Position(X,Y,T)\tResult Pixel(X,Y,T)\tResult(X,Y,T)\tDiscription";
                sb.Append(pR.StartTime.ToString("yy-MM-dd HH:mm:ss.f")                                                                     ); sb.Append("\t");  //START TIME
                sb.Append(pR.GrapTime_ms.ToString()                                                                                        ); sb.Append("\t");  //Grap Time (ms)
                sb.Append(pR.DetectTime_ms.ToString()                                                                                      ); sb.Append("\t");  //Detect Time (ms)
                sb.Append(pR.EndTime.ToString("yy-MM-dd HH:mm:ss.f")                                                                       ); sb.Append("\t");  //End Time
                sb.Append((pR.EndTime - pR.StartTime).TotalMilliseconds.ToString()                                                         ); sb.Append("\t");  //Total Time (ms)
                sb.Append($"{Math.Round(pR.StartPtX, 3)},{Math.Round(pR.StartPtY, 3)},{Math.Round(pR.StartPtT, 3)}"                        ); sb.Append("\t");  //Before Position(X,Y,T)
                sb.Append($"{Math.Round(pR.AlignCompletePtX, 3)},{Math.Round(pR.AlignCompletePtY, 3)},{Math.Round(pR.AlignCompletePtT, 3)}"); sb.Append("\t");  //After Position(X,Y,T)
                sb.Append($"{Math.Round(pR.X_Pixel, 3)},{Math.Round(pR.Y_Pixel, 3)},{Math.Round(pR.T_Pixel, 3)}"                           ); sb.Append("\t");  //Result Pixel(X,Y,T)
                sb.Append($"{Math.Round(pR.X_mm   , 3)},{Math.Round(pR.Y_mm   , 3)},{Math.Round(pR.T_mm   , 3)}"                           ); sb.Append("\t");  //Result(X,Y,T)

                if(pR.IsCompleted) sb.Append("Completed"); //Discription
                else
                {
                    int alarm = pR.ErrorNumber;
                    sb.Append(cDEF.EPU.GetName(alarm));
                }
                
                //
                cDEF.LOG.VisionTrace(sb.ToString(), pR.sLogTime); 

                //
                bool useImageSave = cDEF.FM.EngrOptn.bUseImageSave; ;
                if(useImageSave)
                {
                    string imageSaveDir = cDEF.FM.EngrOptn.sImageSavePath;
                    string dateDir 　　  = $"{DateTime.Now:yy-MM-dd}";
                    string destSaveDir 　= $"{imageSaveDir}\\{dateDir}";

                    if(Directory.Exists(destSaveDir) == false) Directory.CreateDirectory(destSaveDir);

                    ImageFormat saveImageFormat = ImageFormat.Bmp;
                    string fileNameS = $"{pR.StartTime:yyMMddHHmmssf}S";
                    string fileNameO = $"{pR.StartTime:yyMMddHHmmssf}O";
                    string fileNameL = $"{pR.StartTime:yyMMddHHmmssf}L";

                    pR.Src    ?.Save($"{destSaveDir}\\{fileNameS}.{saveImageFormat}",saveImageFormat);
                    pR.Overlay?.Save($"{destSaveDir}\\{fileNameO}.{saveImageFormat}",saveImageFormat);
                    pR.Last   ?.Save($"{destSaveDir}\\{fileNameL}.{saveImageFormat}",saveImageFormat);

                    //
                    int maxSize  = cDEF.FM.EngrOptn.nMaxImageStorage;
                    DeleteOldFile(imageSaveDir, imageSaveDir,maxSize);
                    DeleteEmptyFolder(imageSaveDir);
                    Debug.WriteLine("LOG COMPLETE");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                pR.Using = false;
            }
        }

        //--------------------------------------------------------------------------
        private static void DeleteOldFile(string pRoot,string pDirectory, double pMaxSize)
        {
            long   size     =  GetDirectorySize(new DirectoryInfo(pRoot));
            if (size < pMaxSize * 1000000000) return;

            if (Directory.Exists(pDirectory))                                                                        
            {
                string[] directorys = Directory.GetDirectories(pDirectory).OrderBy(p => p).ToArray();

                if (directorys.Length > 0)
                {
                    foreach (string dir in directorys) DeleteOldFile(pRoot, dir, pMaxSize);
                }

                string[] fileNames = Directory.GetFiles(pDirectory);

                List<FileInfo> files = new List<FileInfo>();

                foreach (string path in fileNames)                                                                   
                {
                    files.Add(new FileInfo(path));
                }

                FileInfo[] sortedFiles = files.OrderBy(p => p.LastWriteTime).ToArray();

                foreach (FileInfo fileInfo in sortedFiles)
                {
                    fileInfo.Delete();
                    size  =  GetDirectorySize(new DirectoryInfo(pRoot));
                    if(size < pMaxSize * 1000000000) break;
                }
            }

            //DeleteOldFile(pDirectory, pStorageDasy);
        }
        //--------------------------------------------------------------------------
        private static bool DeleteEmptyFolder(string pDirectory)
        {
            if (Directory.Exists(pDirectory))                                                                           //저장 경로 유효 성 판단
            {
                string[] fileNames = Directory.GetFiles(pDirectory);
                string[] directorys;
                while (true)
                {
                    directorys = Directory.GetDirectories(pDirectory);

                    if (directorys.Length > 0)
                    {
                        bool existFile = false;
                        foreach (string dir in directorys)
                        {
                            if (DeleteEmptyFolder(dir) == false) existFile = true;
                        }
                        if (existFile == true) break;
                    }
                    else
                    {
                        break;
                    }
                }
                if (fileNames.Length == 0 && directorys.Length == 0)
                {
                    Directory.Delete(pDirectory);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------
        private static long  GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long sum = 0;
            var files = directoryInfo.EnumerateFiles();
            var directories = directoryInfo.EnumerateDirectories();
            foreach (var file in files)
                sum += file.Length;
            foreach (var directory in directories)
                sum += GetDirectorySize(directory);
            return sum;
        }
        //--------------------------------------------------------------------------
        public ListViewItem GetListViewItem()
        {
            ListViewItem item = new ListViewItem(this.StartTime.ToString("yy-MM-dd HH:mm:ss.f"));
            item.SubItems.Add($"{this.GrapTime_ms} ms" );
            item.SubItems.Add($"{this.DetectTime_ms} ms" );
            item.SubItems.Add(this.EndTime.ToString("yy-MM-dd HH:mm:ss.f"));
            item.SubItems.Add($"{(this.EndTime - this.StartTime).TotalMilliseconds} ms" );

            item.SubItems.Add($"{Math.Round(StartPtX, 3)},{Math.Round(StartPtY, 3)},{Math.Round(StartPtT, 3)}");
            item.SubItems.Add($"{Math.Round(AlignCompletePtX, 3)},{Math.Round(AlignCompletePtY, 3)},{Math.Round(AlignCompletePtT, 3)}");

            item.SubItems.Add($"{Math.Round(this.X_Pixel, 3)},{Math.Round(this.Y_Pixel, 3)},{Math.Round(this.T_Pixel, 3)}");
            item.SubItems.Add($"{Math.Round(this.X_mm, 3)},{Math.Round(this.Y_mm, 3)},{Math.Round(this.T_mm, 3)}");
            item.SubItems.Add($"{Barcode}");

            if (IsCompleted) item.SubItems.Add("Completed");
            else
            {
                int alarm = ErrorNumber;
                item.SubItems.Add(cDEF.EPU.GetName(alarm)); 
            }

            return item;
        }
        //--------------------------------------------------------------------------
        public cResult DeepCopy()
        {
            cResult other = MemberwiseClone() as cResult;
            
            other.Src     = Src     ?.Clone() as Image;
            other.Last    = Last    ?.Clone() as Image;
            other.Overlay = Overlay ?.Clone() as Image;

            return other;
        }
    }

}
