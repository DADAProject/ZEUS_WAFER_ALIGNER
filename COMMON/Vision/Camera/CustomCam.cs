using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    public enum VisionStatus
    {
        Finished,
        Running,
        Fiducial, // 웨이퍼, 피커 얼라인
        Ocr,
        Align, // 스탬프 얼라인
        Mapping,
        Error,
        NotExists,
        Timeout
    }

    public abstract class CustomCam : IDisposable
    {
        #region << Import >>
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        #endregion

        #region << Feilds >>

        private bool disposedValue;
        #endregion

        #region << Deconstructor >>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class TSET
    {
        //====================================================
        //Camera Parameters
        public bool bUseVisn;
        public double dXCenterDist;
        public double dYCenterDist;

        //====================================================
        //Algorithm Parameters (To Load Parameter Class by Name)
        public string sIlluminationName;
        public string[] sROIName = new string[5];
        public string sRefName;

        //====================================================
        //Vision Parameters 
        public int[] iThreshold         = new int[10];
        public int[] iKernel            = new int[10];
        public int[] iIterable          = new int[10];

        public double[] dMinSizeTorr    = new double[10];
        public double[] dMaxSizeTorr    = new double[10];
        public int iInscribedPoint;

        //Align Parameter
        public double dMinScore;
        public double dMinTheta;
        public double dMaxTheta;
        public int iAlignRetryCnt;

        //Scan Parameter
        public int iScanXCnt;
        public int iScanYCnt;
        public double dScanMinScore;
        public int iScanEdgeCnt;

        //Delay Parameter
        public int iMotrStopDelay;
        public int iDelayGrab;
        public int iOffDelayRslt;

        //Other Parameter
        public int iContFailCnt;
        public int iStckFailCnt;
        public int iRsltMode;

        //Save Parameter
        public string sImagePath;
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSET()
        {
            ResetData();
        }
        ~TSET()
        {

        }
        public object Copy() { return this.MemberwiseClone(); }
        public void ResetData()
        {
            bUseVisn       = false;
            dXCenterDist   = 0;
            dYCenterDist   = 0;
            dMinScore      = 0;
            dMinTheta      = 0;
            dMaxTheta      = 0;
            iAlignRetryCnt = 0;
            iScanXCnt      = 0;
            iScanYCnt      = 0;
            dScanMinScore  = 0;
            iScanEdgeCnt   = 0;
            iMotrStopDelay = 0;
            iDelayGrab     = 0;
            iOffDelayRslt  = 0;
            iContFailCnt   = 0;
            iStckFailCnt   = 0;
            sImagePath     = Application.StartupPath + "\\Image\\";
            iInscribedPoint = 3000;

            for (int i = 0; i < iThreshold.Length   ; i++) iThreshold[i]    = 100;
            for (int i = 0; i < iKernel.Length      ; i++) iKernel[i]       = 3;
            for (int i = 0; i < iIterable.Length    ; i++) iIterable[i]     = 1;

            for (int i = 0; i < dMinSizeTorr.Length; i++) dMinSizeTorr[i] = 0.1;
            for (int i = 0; i < dMaxSizeTorr.Length; i++) dMaxSizeTorr[i] = 0.1;


            sIlluminationName   = "Default";
            for (int i = 0; i < sROIName.Length; i++) sROIName[i] = "Default";
            sRefName            = "Default";
        }

    }

    public class TVisnRslt
    {
        public EN_CAM CamId         { get; set; }
        public double T             { get; set; }  
        public double X             { get; set; }
        public double Y             { get; set; }
        public double CenX          { get; set; } = 4096 / 2;
        public double CenY          { get; set; } = 3000 / 2;
        public double OriX          { get; set; } 
        public double OriY          { get; set; } 
        public double OriR          { get; set; } 
        public int XPix             { get; set; }
        public int YPix             { get; set; }
        public float DcutStrtX      { get; set; }
        public float DcutStrtY      { get; set; }
        public float DcutEndX       { get; set; }
        public float DcutEndY       { get; set; }
        public int   AngleRegionX   { get; set; }
        public int   AngleRegionY   { get; set; }
        public double Score         { get; set; }
        public bool Match           { get; set; }
        public int InspRslt         { get; set; }
        public string BarCode       { get; set; }
        public string Type          { get; set; }
        public string Mode          { get; set; }
        public int No               { get; set; }
        public TCamResultItem  Item { get; set; }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TVisnRslt()
        {
            ResetData();
        }
        ~TVisnRslt() { }

        public TVisnRslt Copy()
        {
            return FNC.DeepClone(this) as TVisnRslt;
            //return this.MemberwiseClone();
        }

        public void ResetData()
        {
            X         = 0;
            Y         = 0;
            T         = 0;
          //CenX      = 0;
          //CenY         = 0;
            OriX         = 0;
            OriY         = 0;
            OriR         = 0;
            XPix         = 0;
            YPix         = 0;
            DcutStrtX    = 0;
            DcutStrtY    = 0;
            DcutEndX     = 0;
            DcutEndY     = 0;
            AngleRegionX = 0;
            AngleRegionY = 0;
            Match        = false;
            Score        = 0;
            InspRslt     = 0;
            No           = 0;
                      
            BarCode      = string.Empty; 
            Type         = string.Empty;
            Mode         = string.Empty;

            Item?.Dispose();
            Item = null;
        }
        
        public override string ToString()
        {
            return $"[{nameof(No      )}:" + $"{No     }]" +
                   $"{nameof(CamId    )}:" + $"{CamId   }," +
                   $"{nameof(X        )}:" + $"{Math.Round(X,5)}," +
                   $"{nameof(Y        )}:" + $"{Math.Round(Y,5)}," +
                   $"{nameof(T        )}:" + $"{Math.Round(T,5)}," +
                   $"{nameof(CenX     )}:" + $"{CenX    }," +
                   $"{nameof(CenY     )}:" + $"{CenY    }," +
                   $"{nameof(OriX     )}:" + $"{OriX    }," +
                   $"{nameof(OriY     )}:" + $"{OriY    }," +
                   $"{nameof(OriR     )}:" + $"{OriR    }," +
                   $"{nameof(XPix     )}:" + $"{XPix    }," +
                   $"{nameof(YPix     )}:" + $"{YPix    }," +
                   $"{nameof(DcutStrtX)}:" + $"{DcutStrtX}," +
                   $"{nameof(DcutStrtY)}:" + $"{DcutStrtY}," +
                   $"{nameof(DcutEndX )}:" + $"{DcutEndX }," +
                   $"{nameof(DcutEndY )}:" + $"{DcutEndY }," +
                   $"{nameof(AngleRegionX)}:" + $"{AngleRegionX}," +
                   $"{nameof(AngleRegionY)}:" + $"{AngleRegionY}," +
                   $"{nameof(Score    )}:" + $"{Score   }," +
                   $"{nameof(Match    )}:" + $"{Match   }," +
                   $"{nameof(InspRslt )}:" + $"{InspRslt}," +
                   $"{nameof(Mode     )}:" + $"{Mode    }," +
                   $"{nameof(Type     )}:" + $"{Type    }," +
                   $"{nameof(BarCode  )}:" + $"{BarCode } ";
        }

        public TVisnRslt SetData
        {
            set
            {
                this.CamId    = value.CamId   ;
                this.X        = value.X       ;
                this.Y        = value.Y       ;
                this.T        = value.T       ;
                this.CenX     = value.CenX    ;
                this.CenY     = value.CenY    ;
                this.OriX     = value.OriX    ;
                this.OriY     = value.OriY    ;
                this.OriR     = value.OriR    ;
                this.DcutStrtX= value.DcutStrtX;
                this.DcutStrtY= value.DcutStrtY;
                this.DcutEndX = value.DcutEndX ;
                this.DcutEndY = value.DcutEndY ;
                this.AngleRegionX = value.AngleRegionX;
                this.AngleRegionY = value.AngleRegionY;
                this.Score    = value.Score   ;
                this.Match    = value.Match   ;
                this.InspRslt = value.InspRslt;
                this.BarCode  = value.BarCode ;
                this.Mode     = value.Mode    ;
                this.Type     = value.Type    ;
                this.No       = value.No      ;

                this.Item     = value.Item;
            }
        }
    }

    //============================================
    //카메라 & 조명 관련
    public class Tillumination
    {
        private static readonly int Max_Channel = 1;
        public static string Extension = "*.ill";

        public string   sName;
        public double   dGain;
        public double   dExposureTime;
        public double[] dLightValue = new double[Tillumination.Max_Channel];
        public bool[]   dUseLight   = new bool  [Tillumination.Max_Channel];
        public double   dLightDelay;

        public void Load(bool IsLoad, String DevName)
        {
            String sFilePath;
            String sFile = DevName;
            String sSection = sFile;
            String sName;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("Vision");
            sFilePath = TVisnUnit.Path + sFile + ".ill";

            //
            if (IsLoad)
            {
                ini.Loadini(sFilePath);
                sName = sSection + string.Format("_Gain         "); ini.Load(sFilePath, sSection, sName, out dGain);
                sName = sSection + string.Format("_ExposureTime "); ini.Load(sFilePath, sSection, sName, out dExposureTime);

                for (int i = 0; i < Tillumination.Max_Channel; i++)
                {
                    sName = sSection + string.Format($"_LightValue{i}   "); ini.Load(sFilePath, sSection, sName, out dLightValue[i]);
                    sName = sSection + string.Format($"_UseLight{i}   "); ini.Load(sFilePath, sSection, sName, out dUseLight[i]);
                }

                sName = sSection + string.Format("_LightDelay   "); ini.Load(sFilePath, sSection, sName, out dLightDelay);
            }
            else
            {
                sName = sSection + string.Format("_Gain         "); ini.Save(sFilePath, sSection, sName, dGain);
                sName = sSection + string.Format("_ExposureTime "); ini.Save(sFilePath, sSection, sName, dExposureTime);

                for (int i = 0; i < Tillumination.Max_Channel; i++)
                {
                    sName = sSection + string.Format($"_LightValue{i}   "); ini.Save(sFilePath, sSection, sName, dLightValue[i]);
                    sName = sSection + string.Format($"_UseLight{i}    "); ini.Save(sFilePath, sSection, sName, dUseLight[i]);
                }
                sName = sSection + string.Format("_LightDelay   "); ini.Save(sFilePath, sSection, sName, dLightDelay);
                ini.Saveini(sFilePath);
            }
            ini = null;
        }

        public static Tillumination Getillumination(string name)
        {
            Tillumination cill = new Tillumination();
            cill.Load(true, name);

            return cill;
        }
    }
    //============================================
    //영상 ROI 관련
    public class TROI
    {
        public static string Extension = "*.roi";

        public double dX;
        public double dY;
        public double dWidth;
        public double dHeight;

        public void Load(bool IsLoad, String DevName)
        {
            String sFilePath;
            String sFile = DevName;
            String sSection = sFile;
            String sName;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("Vision");
            sFilePath = TVisnUnit.Path + sFile + ".roi";


            if (IsLoad)
            {
                ini.Loadini(sFilePath);
                sName = sSection + string.Format("_X            "); ini.Load(sFilePath, sSection, sName, out dX);
                sName = sSection + string.Format("_Y            "); ini.Load(sFilePath, sSection, sName, out dY);
                sName = sSection + string.Format("_Width        "); ini.Load(sFilePath, sSection, sName, out dWidth);
                sName = sSection + string.Format("_Height       "); ini.Load(sFilePath, sSection, sName, out dHeight);
            }
            else
            {
                sName = sSection + string.Format("_X            "); ini.Save(sFilePath, sSection, sName, dX);
                sName = sSection + string.Format("_Y            "); ini.Save(sFilePath, sSection, sName, dY);
                sName = sSection + string.Format("_Width        "); ini.Save(sFilePath, sSection, sName, dWidth);
                sName = sSection + string.Format("_Height       "); ini.Save(sFilePath, sSection, sName, dHeight);
                ini.Saveini(sFilePath);
            }
            ini = null;
        }

        public static TROI GeRegion(string name)
        {
            TROI cRoi = new TROI();
            cRoi.Load(true, name);
            return cRoi;
        }
    }

    //영상 ROI 관련
    public class TReference
    {
        public static string Extension = "*.ref";

        public string sPath;
        public double dX;
        public double dY;
        public double dWidth;
        public double dHeight;
        public void Load(bool IsLoad, String DevName)
        {
            String sFilePath;
            String sFile = DevName;
            String sSection = sFile;
            String sName;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("Vision");
            sFilePath = TVisnUnit.Path + sFile + ".ref";

            if (IsLoad)
            {
                ini.Loadini(sFilePath);
                sName = sSection + string.Format("_Path         "); ini.Load(sFilePath, sSection, sName, out sPath);
                sName = sSection + string.Format("_X            "); ini.Load(sFilePath, sSection, sName, out dX);
                sName = sSection + string.Format("_Y            "); ini.Load(sFilePath, sSection, sName, out dY);
                sName = sSection + string.Format("_Width        "); ini.Load(sFilePath, sSection, sName, out dWidth);
                sName = sSection + string.Format("_Height       "); ini.Load(sFilePath, sSection, sName, out dHeight);
            }
            else
            {
                sName = sSection + string.Format("_Path         "); ini.Save(sFilePath, sSection, sName, sPath);
                sName = sSection + string.Format("_X            "); ini.Save(sFilePath, sSection, sName, dX);
                sName = sSection + string.Format("_Y            "); ini.Save(sFilePath, sSection, sName, dY);
                sName = sSection + string.Format("_Width        "); ini.Save(sFilePath, sSection, sName, dWidth);
                sName = sSection + string.Format("_Height       "); ini.Save(sFilePath, sSection, sName, dHeight);
                ini.Saveini(sFilePath);
            }
            ini = null;
        }

        public static TReference GetReference(string name)
        {
            TReference cRef= new TReference();
            cRef.Load(true, name);

            return cRef;
        }
    }

    //영상 Match 관련
    //============================================
    public class TMatchPos
    {
        // public EN_CAM iCamId;
        public double dT;
        public double dX;
        public double dY;
        public int iXPix;
        public int iYPix;
        public int iBin;
        public int iMBin;

        public double dAngle;
        public double dCenterX;
        public double dCenterY;
        public double dAreaRatio;
        public bool bInterpolated;
        public double dScale;
        public double dScaleX;
        public double dScaleY;
        public double dScore;
        public bool bMatch;
        public int iOrgX;
        public int iOrgY;
        public int iWidth;
        public int iHeight;

        public double dIntensit;
        public double dCx;
        public double dCy;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TMatchPos()
        {
            ResetData();
        }
        ~TMatchPos() { }

        public object Copy()
        {
            return this.MemberwiseClone();
        }

        public void ResetData()
        {
            dT            = 0;
            dX            = 0;
            dY            = 0;
            iXPix         = 0;
            iYPix         = 0;
            bMatch        = false;
            dAngle        = 0;
            dCenterX      = 0;
            dCenterY      = 0;
            dAreaRatio    = 0;
            bInterpolated = false;
            dScale        = 0;
            dScaleX       = 0;
            dScaleY       = 0;
            dScore        = 0;

            iOrgX         = 0;
            iOrgY         = 0;
            iWidth        = 0;
            iHeight       = 0;
        }


    }

}
