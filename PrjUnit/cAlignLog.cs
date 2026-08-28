using Drv.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaferAligner
{
    public class cAlignLog
    {
        private static readonly ConcurrentQueue<cResult> ResultQueue = new ConcurrentQueue<cResult>();

        private static Thread AlignLogSaveThread;

        private static bool IsRun = false;
        public static void Start()
        {
            void AlignLogSaveThreadMethod()
            {
                while (IsRun)
                {
                    Thread.Sleep(100);
                    if (ResultQueue.Count > 0)
                    {
                        if (ResultQueue.TryDequeue(out cResult result))
                        {
                            SaveLog(result);
                        }

                    }
                }
            }

            if (IsRun == false)
            {
                IsRun = true;
                AlignLogSaveThread = new Thread(AlignLogSaveThreadMethod);
                AlignLogSaveThread.Name = "AlignLogSaveThread";
                AlignLogSaveThread.IsBackground = true;
                AlignLogSaveThread.Start();
            }
        }
        public static void Stop()
        {
            IsRun = false;
            AlignLogSaveThread.Join();
        }

        public static void AddResult(cResult pResult)
        {
            ResultQueue.Enqueue(pResult);

        }

        public static Task RunSaveLogTask(cResult pR)
        {
            return Task.Run(() =>
            {
                SaveLog(pR);
            });
        }

        private static void SaveLog(cResult pR)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //header = "START TIME\tGrap Time (ms)\tDetect Time (ms)\tEnd Time\tTotal Time (ms)\tBefor Position(X,Y,T)\tAfter Position(X,Y,T)\tResult Pixel(X,Y,T)\tResult(X,Y,T)\tDiscription";
                sb.Append(pR.StartTime.ToString("yy-MM-dd HH:mm:ss.f")                                                               ); sb.Append("\t");  //START TIME
                sb.Append(pR.GrapTime_ms.ToString()                                                                                  ); sb.Append("\t");  //Grap Time (ms)
                sb.Append(pR.DetectTime_ms.ToString()                                                                                ); sb.Append("\t");  //Detect Time (ms)
                sb.Append(pR.EndTime.ToString("yy-MM-dd HH:mm:ss.f")                                                                 ); sb.Append("\t");  //End Time
                sb.Append((pR.EndTime - pR.StartTime).TotalMilliseconds.ToString()                                                      ); sb.Append("\t");  //Total Time (ms)
                sb.Append($"{Math.Round(pR.StartPtX, 3)},{Math.Round(pR.StartPtY, 3)},{Math.Round(pR.StartPtT, 3)}"                        ); sb.Append("\t");  //Befor Position(X,Y,T)
                sb.Append($"{Math.Round(pR.AlignCompletePtX, 3)},{Math.Round(pR.AlignCompletePtY, 3)},{Math.Round(pR.AlignCompletePtT, 3)}"); sb.Append("\t");  //After Position(X,Y,T)
                sb.Append($"{Math.Round(pR.X_Pixel, 3)},{Math.Round(pR.Y_Pixel, 3)},{Math.Round(pR.T_Pixel, 3)}"            ); sb.Append("\t");  //Result Pixel(X,Y,T)
                sb.Append($"{Math.Round(pR.X_mm   , 3)},{Math.Round(pR.Y_mm   , 3)},{Math.Round(pR.T_mm   , 3)}"            ); sb.Append("\t");  //Result(X,Y,T)

                if(pR.IsCompleted) sb.Append("Completed"); //Discription
                else
                {
                    eAlarm alarm = (eAlarm)pR.ErrorNumber;
                    sb.Append(alarm.ToString());
                }
                cLog.Instance.Write(eLogKind.ALIGN, sb.ToString());

                bool useImageSave = cParameter.Instance.Get(cParams.eEquipment.USE_IMAGE_SAVE);

                if(useImageSave)
                {
                    string imageSaveDir = cParameter.Instance.Get(cParams.eEquipment.IMAGE_SAVE_PATH);
                    string dateDir = $"{DateTime.Now:yy-MM-dd}";
                    string destSaveDir = $"{imageSaveDir}\\{dateDir}";

                    if(Directory.Exists(destSaveDir) == false) Directory.CreateDirectory(destSaveDir);

                    ImageFormat saveImageFormat = ImageFormat.Bmp;
                    string fileNameS = $"{pR.StartTime:yyMMddHHmmssf}S";
                    string fileNameO = $"{pR.StartTime:yyMMddHHmmssf}O";
                    string fileNameL = $"{pR.StartTime:yyMMddHHmmssf}L";

                    pR.Src    ?.Save($"{destSaveDir}\\{fileNameS}.{saveImageFormat}",saveImageFormat);
                    pR.Overlay?.Save($"{destSaveDir}\\{fileNameO}.{saveImageFormat}",saveImageFormat);
                    pR.Last   ?.Save($"{destSaveDir}\\{fileNameL}.{saveImageFormat}",saveImageFormat);

                    double maxSize  = cParameter.Instance.Get(cParams.eEquipment.MAX_IMAGE_STORAGE);
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
            }

        }


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
    }
}
