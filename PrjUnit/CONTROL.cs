using System;
using System.IO;

namespace BarcodeReaderLibrary
{
    public class CHisto
    {
        double sum, sum2;
        int n;

        public CHisto()
        {
            n = 0;
            sum = 0.0;
            sum2 = 0.0;
        }
        public void add(double x)
        {
            n++;
            sum += x;
            sum2 += x * x;
        }
        public double mean()
        {
            if (n < 1) return 0;
            return sum / n;
        }
        public double stdevp()
        {
            if(n < 1) return 0;
            return Math.Sqrt(((double)n * sum2 - sum*sum) / ((double)n * (double)n));
        }
        public double stdev()
        {
            if (n < 2) return 0;
            return Math.Sqrt(((double)n * sum2 - sum * sum) / ((double)n * (double)(n - 1)));
        }

    }

    public class IMAGE_BUFF
    {
        public byte[] data;
        public IntPtr ptr;
        public int width, height;
    }

    public class LIBRARY_RETURN
    {
        public bool err;
        public string msg;
        public void clear()
        {
            err = false;
            msg = null;
        }
    }
    public struct SQC
    {
        public const int END = 0;
        public const int STOP = 10000;
        public const int ERROR = 11000;
        public const int EMERGENCY = 12000;
        public const int LIBRARY_ERROR = 13000;
        public const int TIME_OUT = 14000;
    }
    public struct RET
    {
        public int i, i1, i2, i3, i4;
        public bool b, b1, b2, b3, b4;
        public string s, s1, s2, s3, s4;
        public double d, d1, d2, d3, d4;
    }
    
    public struct FILE
    {
        public static string PARA;
        public static string PARA_BAK;

        public static string MC_CONFIG;
        public static string PKG;
        public static string LOGIN;
        public static string LOG;
        public static string YIELD;
    }
    public struct PATH
    {
        public static string DEFAULT;
        public static string SYSTEM;
        public static string VISION;
        public static string MOTION;
        public static string PARAMETER;
        public static string MC_CONFIG;
        public static string PKG;
        public static string LOG;
        public static string YIELD;
        public static string DEBUG;
        public static string STATISTIC;
        public static string OHTEC;
        public static string PROJECT;
        public static string ERROR_IMAGE;

        public static void set(string defaulPath)
        {
            DEFAULT = defaulPath;
            SYSTEM = DEFAULT + "System\\";
            VISION = DEFAULT + "Vision\\";
            MOTION = DEFAULT + "Motion\\";
            PARAMETER = DEFAULT + "Parameter\\";
            MC_CONFIG = SYSTEM;
            PKG = DEFAULT + "Package\\";
            LOG = DEFAULT + "Log\\";
            YIELD = DEFAULT + "Yield\\";
            DEBUG = DEFAULT + "Debug\\";
            STATISTIC = DEFAULT + "Statistic\\";
            OHTEC = "d:\\innobiz\\OhTec\\";
            PROJECT = DEFAULT + "Project\\";
            ERROR_IMAGE = SYSTEM + "ErrorImage\\";
            #region FILE set
            FILE.PARA = PARAMETER + "para.xlsx";
            FILE.PARA_BAK = PARAMETER + "para.bak";
            FILE.MC_CONFIG = SYSTEM + "mcConfig.ini";
            FILE.PKG = PKG + "pkg.ini";
            FILE.LOGIN = SYSTEM + "login.xlsx";
            FILE.LOG = LOG + "log.txt";
            FILE.YIELD = YIELD + "yield.txt";
            #endregion;
        }
    }
  
    public enum JOG_RET
    {
        ESC,
        OK
    }

    public enum HOMING
    {
        INVALID = -1,
        NEEDED,
        SKIP,
        SEARCHING,
        FAIL,
        SUCCESS,
    }

    public abstract class CONTROL
    {
        public double TIME_OUT = 30000;
        public bool isActivate;
        public RET ret;
        public bool req;
        public int sqc, Esqc;
        public QueryTimer dwell = new QueryTimer();
        public QueryTimer timer = new QueryTimer();
        public void clear()
        {
            sqc = 0; Esqc = 0;
            req = false;
        }
        public bool RUNING
        {
            get
            {
                if (req || sqc != 0) return true;
                else return false;
            }
        }
        public bool ERROR
        {
            get
            {
                if (Esqc != 0) return true;
                return false;
            }
        }
    }
    public abstract class CONTROL_NIP100
    {
        public double TIME_OUT = 10000;
        public CONTROL_NAME_NIP100 name = CONTROL_NAME_NIP100.INVALID;
        public bool isActivate;
        public RET ret;
        public ON_OFF[] ON = new ON_OFF[10];
        public bool req;
        public int sqc, Esqc;
        public QueryTimer dwell = new QueryTimer();
        public QueryTimer timer = new QueryTimer();
        public void clear()
        {
            sqc = 0; Esqc = 0;
            req = false;
        }
        public bool RUNING
        {
            get
            {
                if (req || sqc != 0) return true;
                else return false;
            }
        }
        public bool ERROR
        {
            get
            {
                if (Esqc != 0) return true;
                return false;
            }
        }
    }
    public enum CONTROL_NAME_NIP100
    {
        INVALID = -1,
        FULL_AUTO,
        AREA_INTERLOCK,

        PICKER_R,
        LOADING_Z,
        ALIGN_Z,
        CONTACT_Z,
        UNLOADING_Z,
        ALIGN,
        PROBE,
        PURGE,

        STAGE,
        NEEDLE,
        GRIPPER,
        CASSETTE,
        BOWL_FEEDER,
       
        PICKUP_VISION,
        INSPECTION_VISION,
        PROBE_VISION,
        SCAN_VISION,
        UNDER1_VISION,
        UNDER2_VISION,
    }
    public enum AXIS_NAME_NIP100
    {
        INVALID = -1,

        PICKER_R,
        PICKER_LOADING_Z,
        PICKER_ALIGN_Z,
        PICKER_CONTACT_Z,

        PICKER_UNLOADING_Z,
        ALIGN_X,
        PROBE_Z,
        UNLOADING_STAGE_X,

        UNLOADING_STAGE_Y,
        UNLOADING_STAGE_T,
        NEEDLE_UNIT_Z,
        NEEDLE_PIN_Z,

        UNLOADING_GRIPPER_X,
        UNLOADING_CASSETTE_Z,
        UNDEFINE1,
        UNDEFINE2,

        LENGTH,
    }

    public enum MC_NAMESPACE
    {
        INVALID = -1,
        NIP100,
        LENGTH,
    }
    public enum EQUIP_MAKER
    {
        INVALID = -1,
        INNOBIZ,
        LENGTH,
    }
    public enum EQUIP_USER
    {
        INVALID = -1,
        SSC,
        LENGTH,
    }
    public enum CAMERA
    {
        INVALID = -1,

        UNDER1,
        SCAN,
        PICKUP,
        UNDER2,
        PROBE,
        //INSPECTION,

        LENGTH,
    }

    public struct ThreadTime
    {
        public double average, min, max;
    }
    public class MovingTime
    {
        public MovingTime()
        {
            clear();
        }
        public void clear()
        {
            target = -1;
            settle = -1;
            total = -1;
        }
        public double target, settle, total;
    }
    public class VisionTime
    {
        public VisionTime()
        {
            clear();
        }
        public void clear()
        {
            grab = -1;
            find = -1;
            total = -1;
        }
        public double grab, find, total;
    }

    public struct IntergerXY
    {
        public int x, y;
    }
    public struct DoubleXY
    {
        public double x, y;
    }
    public struct VisionModelResult
    {
        public bool FIND;
        public double x, y, t, score;
    }
    public struct VisionBlobRectResult
    {
        public bool FIND;
        public double rectangularity;
        public double x, y, t, w, h;
    }
    public struct DoubleXYT
    {
        public double x, y, t;
    }
    public struct DoubleXYZ
    {
        public double x, y, z;
    }
    public struct DoubleXYZT
    {
        public double x, y, z, t;
    }
    public enum ON_OFF
    {
        OFF,
        ON
    }
}
