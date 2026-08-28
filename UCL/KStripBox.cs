using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    // ENUM
    public enum eStripDirection { None, LeftToRight, RIghtToLeft };
    public enum eStripNumType { Alphabet, Number };
    public enum eStripStatus : byte
    {
        Rank000 = 0,
        Rank001 = 1, Rank002 = 2, Rank003 = 3, Rank004 = 4, Rank005 = 5, Rank006 = 6, Rank007 = 7, Rank008 = 8, Rank009 = 9, Rank010 = 10,
        Rank011 = 11, Rank012 = 12, Rank013 = 13, Rank014 = 14, Rank015 = 15, Rank016 = 16, Rank017 = 17, Rank018 = 18, Rank019 = 19, Rank020 = 20,
        Rank021 = 21, Rank022 = 22, Rank023 = 23, Rank024 = 24, Rank025 = 25, Rank026 = 26, Rank027 = 27, Rank028 = 28, Rank029 = 29, Rank030 = 30,
        Rank031 = 31, Rank032 = 32, Rank033 = 33, Rank034 = 34, Rank035 = 35, Rank036 = 36, Rank037 = 37, Rank038 = 38, Rank039 = 39, Rank040 = 40,
        Rank041 = 41, Rank042 = 42, Rank043 = 43, Rank044 = 44, Rank045 = 45, Rank046 = 46, Rank047 = 47, Rank048 = 48, Rank049 = 49, Rank050 = 50,
        Rank051 = 51, Rank052 = 52, Rank053 = 53, Rank054 = 54, Rank055 = 55, Rank056 = 56, Rank057 = 57, Rank058 = 58, Rank059 = 59, Rank060 = 60,
        Rank061 = 61, Rank062 = 62, Rank063 = 63, Rank064 = 64, Rank065 = 65, Rank066 = 66, Rank067 = 67, Rank068 = 68, Rank069 = 69, Rank070 = 70,
        Rank071 = 71, Rank072 = 72, Rank073 = 73, Rank074 = 74, Rank075 = 75, Rank076 = 76, Rank077 = 77, Rank078 = 78, Rank079 = 79, Rank080 = 80,
        Rank081 = 81, Rank082 = 82, Rank083 = 83, Rank084 = 84, Rank085 = 85, Rank086 = 86, Rank087 = 87, Rank088 = 88, Rank089 = 89, Rank090 = 90,
        Rank091 = 91, Rank092 = 92, Rank093 = 93, Rank094 = 94, Rank095 = 95, Rank096 = 96, Rank097 = 97, Rank098 = 98, Rank099 = 99, Rank100 = 100,
        Rank101 = 101, Rank102 = 102, Rank103 = 103, Rank104 = 104, Rank105 = 105, Rank106 = 106, Rank107 = 107, Rank108 = 108, Rank109 = 109, Rank110 = 110,
        Rank111 = 111, Rank112 = 112, Rank113 = 113, Rank114 = 114, Rank115 = 115, Rank116 = 116, Rank117 = 117, Rank118 = 118, Rank119 = 119, Rank120 = 120,
        Rank121 = 121, Rank122 = 122, Rank123 = 123, Rank124 = 124, Rank125 = 125, Rank126 = 126, Rank127 = 127, Rank128 = 128, Rank129 = 129, Rank130 = 130,
        Rank131 = 131, Rank132 = 132, Rank133 = 133, Rank134 = 134, Rank135 = 135, Rank136 = 136, Rank137 = 137, Rank138 = 138, Rank139 = 139, Rank140 = 140,
        Rank141 = 141, Rank142 = 142, Rank143 = 143, Rank144 = 144, Rank145 = 145, Rank146 = 146, Rank147 = 147, Rank148 = 148, Rank149 = 149, Rank150 = 150,
        NON_151 = 151, NON_152 = 152, NON_153 = 153, NON_154 = 154, NON_155 = 155, NON_156 = 156, NON_157 = 157, NON_158 = 158, NON_159 = 159, NON_160 = 160,
        NON_161 = 161, NON_162 = 162, NON_163 = 163, NON_164 = 164, NON_165 = 165, NON_166 = 166, NON_167 = 167, NON_168 = 168, NON_169 = 169, NON_170 = 170,
        NON_171 = 171, NON_172 = 172, NON_173 = 173, NON_174 = 174, NON_175 = 175, NON_176 = 176, NON_177 = 177, NON_178 = 178, NON_179 = 179, NON_180 = 180,
        NON_181 = 181, NON_182 = 182, NON_183 = 183, NON_184 = 184, NON_185 = 185, NON_186 = 186, NON_187 = 187, NON_188 = 188, NON_189 = 189, NON_190 = 190,
        NON_191 = 191, NON_192 = 192, NON_193 = 193, NON_194 = 194, NON_195 = 195, NON_196 = 196, NON_197 = 197, NON_198 = 198, NON_199 = 199, NON_200 = 200,
        NON_201 = 201, NON_202 = 202, NON_203 = 203, NON_204 = 204, NON_205 = 205, NON_206 = 206, NON_207 = 207, NON_208 = 208, NON_209 = 209, NON_210 = 210,
        NON_211 = 211, NON_212 = 212, NON_213 = 213, NON_214 = 214, NON_215 = 215, NON_216 = 216, NON_217 = 217, NON_218 = 218, NON_219 = 219, NON_220 = 220,
        NON_221 = 221, NON_222 = 222, NON_223 = 223, NON_224 = 224, NON_225 = 225, NON_226 = 226, NON_227 = 227, NON_228 = 228, NON_229 = 229, NON_230 = 230,
        NON_231 = 231, NON_232 = 232, NON_233 = 233, NON_234 = 234, NON_235 = 235, NON_236 = 236, NON_237 = 237, NON_238 = 238, NON_239 = 239, NON_240 = 240,
        NON_241 = 241, NON_242 = 242, NON_243 = 243, NON_244 = 244, NON_245 = 245, NON_246 = 246,
        AlignKey = 247,
        AlignCen = 248,
        DataOnly = 249,
        ScanOnly = 250,
        Empty = 251,
        Exist = 252,
        Work = 253,
        Finish = 254,
        Error = 255
    };

    // DELEGATE
    public delegate void dEventStripDrawCell1(int col, int row);
    public delegate void dEventStripDrawCell2(KStripBox obj, PaintEventArgs e);
    public delegate void dEventStripSelected(KStripBox obj, int idx, int col, int row, float trinum, int areanum, eStripStatus sts_value);

    public partial class KStripBox : UserControl
    {
        #region 멤버 변수들..
        const string AlphabetString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


        // 내부용
        private string[] _LabelString;
        private RectangleF[] _LabelRect;

        private Bitmap _SelectImage;
        private RectangleF[,] _DrawRect;
        private string[,] _DrawString;
        private string[,] _DrawTriStr; // 트리거 및 Area 문자
        private eStripStatus[,] _StripStatus;// eStripStatus 형
        public float[,] TriNum;      // 트리거 번호
        public int[,] AreaNum;     // 1개 트리거 영역안의 분할 영역 번호


        // Property
        private bool _DrawEnable;

        private int _ColCount;
        private int _RowCount;
        private int _SegCol;
        private int _SegRow;

        private float _RoundRectFact;  // 0.0 ~ 1.0
        private eStripNumType _ColNumType;
        private eStripNumType _RowNumType;

        private Color _ColorActive;
        private Color _ColorSelect;
        private Color _ColorBoarder;
        private Color _ColorLabel;
        private Color _ColorItemBoarder;
        private Color _ColorTriAreaNum;

        private Color[] _ColorRanks;    // 256 가지


        private bool _SelectEnable;
        private bool _ActiveEnable;
        private int _SelectIndex;
        private int _ActiveIndex;

        private bool _VisibleLabel;
        private bool _VisibleNumber;
        private bool _VisibleTriNum;
        private bool _VisibleAreaNum;

        private int _DistanceItem;    // cell간     픽셀 간격
        private int _DistanceSegment; // 세그먼트간 픽셀 간격

        private Padding _DrawOffset;
        private int _BoarderSize;     // 최외각선 두께
        private int _LineWeight;      // Cell  선 두께

        private eStripDirection _Direction;
        private bool _ColNumLeftToRight;
        private bool _RowNumTopToBottom;
        private bool _MouseMoveChangeEnable;
        private bool _IsMouseDown;
        private string _TriNumFormat;

        private Graphics gra;
        private Pen ActivePen;
        private Pen SelectPen;
        private Pen boarderPen;
        private SolidBrush itemBrushLabel;
        private Pen itemBoarderPen;
        private SolidBrush triBrush;
        private SolidBrush[] itemDrawBrush;
        private SolidBrush fontBrush;
        private StringFormat strfm;
        private PointF txtpt;
        private RectangleF tmpRect;
        private GraphicsPath path;




        private event dEventStripSelected _OnSelected; // 이벤트 선언
        private event dEventStripSelected _OnSelectedDouble;
        private event dEventStripSelected _OnActive;
        #endregion

        #region 생성자 및 초기화
        public KStripBox()
        {
            _ColCount = 10;
            _RowCount = 10;
            _SegCol = 1;
            _SegRow = 1;
            _RoundRectFact = 0.2f;

            _ColNumType = eStripNumType.Number;
            _RowNumType = eStripNumType.Number;

            _ColorActive = Color.Blue;
            _ColorSelect = Color.Red;
            _ColorBoarder = Color.Black;
            _ColorLabel = Color.White;
            _ColorItemBoarder = Color.Black;
            _ColorTriAreaNum = Color.Black;

            _ColorRanks = new Color[256];
            for (int i = 0; i < 256; i++)
                _ColorRanks[i] = BackColor;

            ColorRankZero = Color.Lime;  // 0
            ColorAlignKey = Color.Gray;  // 247
            ColorAlignCen = Color.White;
            ColorDataOnly = Color.DarkBlue;
            ColorScanOnly = Color.DarkRed;
            ColorEmpty = Color.DimGray;
            ColorExist = Color.AliceBlue;
            ColorWork = Color.SkyBlue;
            ColorFinish = Color.Yellow;
            ColorError = Color.Red;   // 255

            _SelectEnable = true;
            _ActiveEnable = true;
            _SelectIndex = -1;
            _ActiveIndex = -1;

            _VisibleLabel = false;
            _VisibleNumber = false;
            _VisibleTriNum = true;
            _VisibleAreaNum = false;

            _DistanceItem = 0;
            _DistanceSegment = 0;

            _DrawOffset = new Padding(3);
            _BoarderSize = 1;               // 최외각선 두께
            _LineWeight = 1;               // Cell 선 두께

            _Direction = eStripDirection.None;
            _ColNumLeftToRight = true;
            _RowNumTopToBottom = true;
            _MouseMoveChangeEnable = false;
            _IsMouseDown = false;
            _TriNumFormat = string.Empty;

            _DrawEnable = false;
            ChangedColor();
            ReStruct();
            ReSizeRect();
            for (int i = 0; i < _ColCount; i++)
                for (int j = 0; j < _RowCount; j++)
                    _StripStatus[i, j] = eStripStatus.Exist;
            _DrawEnable = true;
            DoubleBuffered = true;


            this.Resize += new EventHandler(DrawReSize);
            this.Paint += new PaintEventHandler(DrawCtrl);
            this.MouseClick += new MouseEventHandler(MouseClickE);
            this.MouseMove += new MouseEventHandler(MouseMoveE);
            this.MouseLeave += new EventHandler(MouseLeaveE);
            this.MouseDown += new MouseEventHandler(MouseDownE);
            this.MouseUp += new MouseEventHandler(MouseUpE);
            this.MouseDoubleClick += new MouseEventHandler(MouseDoubleClickE);

            InitializeComponent();
        }



        void MouseDownE(object sender, MouseEventArgs e)
        {
            _IsMouseDown = true;
        }

        void MouseUpE(object sender, MouseEventArgs e)
        {
            _IsMouseDown = false;
        }

        #endregion

        #region 인터페이스 구현부[IReadWrite]
        public bool AssignTo(KStripBox Dest)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                BinaryReader br = new BinaryReader(ms);
                BinaryWriter bw = new BinaryWriter(ms);
                this.SaveStream(ms, bw);
                ms.Seek(0, SeekOrigin.Begin);
                Dest.LoadStream(ms, br);
                bw.Close();
                br.Close();
                ms.Close();
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0} " + GetType().ToString() + " AssignTo Error, msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }
        public bool AssignFrom(KStripBox Source)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                BinaryReader br = new BinaryReader(ms);
                BinaryWriter bw = new BinaryWriter(ms);
                Source.SaveStream(ms, bw);
                ms.Seek(0, SeekOrigin.Begin);
                LoadStream(ms, br);
                bw.Close();
                br.Close();
                ms.Close();
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0} " + GetType().ToString() + " AssignFrom Error, msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }
        public bool SaveBinaryFile(string filename = "")
        {
            FileStream fs = null;
            BinaryWriter bw = null;

            try
            {
                fs = new FileStream(filename, FileMode.Create);
                if (fs == null)
                    Trace.TraceError("{0}, " + GetType().ToString() + " SaveToFile, FileStream Error, filestream=null", "[ERROR]::");
                bw = new BinaryWriter(fs);
                if (bw == null)
                {
                    fs.Close();
                    Trace.TraceError("{0} " + GetType().ToString() + " SaveToFile, BinaryWriter Error, BinaryWriter=null", "[ERROR]::");
                }
                SaveStream(fs, bw);
                bw.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                bw.Close();
                fs.Close();
                Trace.TraceError("{0} " + GetType().ToString() + " SaveToFile Error, msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }
        public bool LoadBinaryFile(string filename = "")
        {
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                if (!File.Exists(filename))
                    Trace.TraceError("{0} " + GetType().ToString() + ", LoadFromFile, File Not Find", "[ERROR]::");
                fs = new FileStream(filename, FileMode.Open);
                if (fs == null)
                    Trace.TraceError("{0} " + GetType().ToString() + ", LoadFromFile, FileStream Error,  filestream= null", "[ERROR]::");

                br = new BinaryReader(fs);
                if (br == null)
                {
                    fs.Close();
                    Trace.TraceError("{0} " + GetType().ToString() + ", LoadFromFile, BinaryReader Error,  BinaryReader= null", "[ERROR]::");
                }
                LoadStream(fs, br);
                br.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                br.Close();
                fs.Close();
                Trace.TraceError("{0} " + GetType().ToString() + " LoadFromFile Error, msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }
        public bool SaveTextFile(string filename = "")
        {
            try
            {
                FileStream fileStream = new FileStream(filename, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.Default);
                SaveText(streamWriter);
                streamWriter.Close();
                fileStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0} " + GetType().ToString() + ",SaveTextFile Error, msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }
        public bool LoadTextFile(string filename = "")
        {
            try
            {
                if (!File.Exists(filename))
                    Trace.TraceError("{0} " + GetType().ToString() + ",LoadTxt Error, File Not Exist", "[ERROR]::");

                FileStream fileStream = new FileStream(filename, FileMode.Open);
                StreamReader streamReader = new StreamReader(fileStream, System.Text.Encoding.Default);
                LoadText(streamReader);
                streamReader.Close();
                fileStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0} " + GetType().ToString() + ",LoadTextFile Error, msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }
        //public bool SaveIniFile    (string filename ="")
        //{
        //    try
        //    {
        //        IniFile inifile = new IniFile(filename);
        //        if(!SaveIni(inifile))
        //            return false;
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        Trace.TraceError ("{0} "+GetType().ToString()+",SaveIniFile Error, msg="+ex.Message, "[ERROR]::");
        //        return false;
        //    }
        //}        
        //public bool LoadIniFile    (string filename ="")
        //{
        //    try
        //    {
        //        if(!File.Exists(filename))
        //            Trace.TraceError ("{0} "+GetType().ToString()+",LoadIniFile Error, File Not Exist", "[ERROR]::");
        //        IniFile inifile = new IniFile(filename);
        //        if(!LoadIni(inifile))
        //            return false;
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        Trace.TraceError ("{0} "+GetType().ToString()+",LoadIniFile Error, msg="+ex.Message, "[ERROR]::");
        //        return false;
        //    }
        //}        

        public bool SaveStream(Stream stream, BinaryWriter bw)
        {
            try
            {
                bw.Write((int)_ColCount);
                bw.Write((int)_RowCount);
                bw.Write((int)_SegCol);
                bw.Write((int)_SegRow);

                bw.Write((float)_RoundRectFact);
                bw.Write((int)_ColNumType);
                bw.Write((int)_RowNumType);

                bw.Write((int)_ColorActive.ToArgb());
                bw.Write((int)_ColorSelect.ToArgb());
                bw.Write((int)_ColorBoarder.ToArgb());
                bw.Write((int)_ColorLabel.ToArgb());
                bw.Write((int)_ColorItemBoarder.ToArgb());
                bw.Write((int)_ColorTriAreaNum.ToArgb());

                int i, j;
                for (i = 0; i < ColorRanks.Length; i++)
                    bw.Write((int)ColorRanks[i].ToArgb());


                //bw.Write(selectEnable);
                //bw.Write(activeEnable);


                bw.Write((bool)_VisibleLabel);
                bw.Write((bool)_VisibleNumber);
                bw.Write((bool)_VisibleTriNum);
                bw.Write((bool)_VisibleAreaNum);
                bw.Write((int)_DistanceItem);
                bw.Write((int)_DistanceSegment);
                bw.Write((int)_DrawOffset.Left);
                bw.Write((int)_DrawOffset.Right);
                bw.Write((int)_DrawOffset.Top);
                bw.Write((int)_DrawOffset.Bottom);
                bw.Write((int)_BoarderSize);
                bw.Write((int)_LineWeight);
                bw.Write((int)_Direction);
                bw.Write((bool)_ColNumLeftToRight);
                bw.Write((bool)_RowNumTopToBottom);
                bw.Write((bool)_MouseMoveChangeEnable);


                for (i = 0; i < _ColCount; i++)
                {
                    for (j = 0; j < _RowCount; j++)
                    {
                        bw.Write((float)TriNum[i, j]);
                        bw.Write((int)AreaNum[i, j]);
                    }
                }

                for (i = 0; i < _ColCount; i++)
                    for (j = 0; j < _RowCount; j++)
                        bw.Write((ushort)_StripStatus[i, j]);

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}, " + GetType().ToString() + ", SaveStream Error. msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }
        public bool LoadStream(Stream stream, BinaryReader br)
        {
            try
            {
                _ColCount = br.ReadInt32();
                _RowCount = br.ReadInt32();
                _SegCol = br.ReadInt32();
                _SegRow = br.ReadInt32();

                _RoundRectFact = br.ReadSingle();
                _ColNumType = (eStripNumType)br.ReadInt32();
                _RowNumType = (eStripNumType)br.ReadInt32();

                _ColorActive = Color.FromArgb(br.ReadInt32());
                _ColorSelect = Color.FromArgb(br.ReadInt32());
                _ColorBoarder = Color.FromArgb(br.ReadInt32());
                _ColorLabel = Color.FromArgb(br.ReadInt32());
                _ColorItemBoarder = Color.FromArgb(br.ReadInt32());
                _ColorTriAreaNum = Color.FromArgb(br.ReadInt32());

                int i, j;
                for (i = 0; i < ColorRanks.Length; i++)
                    ColorRanks[i] = Color.FromArgb(br.ReadInt32());

                // selectEnable
                // activeEnable
                _VisibleLabel = br.ReadBoolean();
                _VisibleNumber = br.ReadBoolean();
                _VisibleTriNum = br.ReadBoolean();
                _VisibleAreaNum = br.ReadBoolean();
                _DistanceItem = br.ReadInt32();
                _DistanceSegment = br.ReadInt32();
                _DrawOffset.Left = br.ReadInt32();
                _DrawOffset.Right = br.ReadInt32();
                _DrawOffset.Top = br.ReadInt32();
                _DrawOffset.Bottom = br.ReadInt32();
                _BoarderSize = br.ReadInt32();
                _LineWeight = br.ReadInt32();
                _Direction = (eStripDirection)br.ReadInt32();
                _ColNumLeftToRight = br.ReadBoolean();
                _RowNumTopToBottom = br.ReadBoolean();
                _MouseMoveChangeEnable = br.ReadBoolean();

                ReStruct();

                for (i = 0; i < _ColCount; i++)
                {
                    for (j = 0; j < _RowCount; j++)
                    {
                        TriNum[i, j] = br.ReadSingle();
                        AreaNum[i, j] = br.ReadInt32();
                    }
                }

                for (i = 0; i < _ColCount; i++)
                    for (j = 0; j < _RowCount; j++)
                        _StripStatus[i, j] = (eStripStatus)br.ReadByte();

                ReSizeRect();
                SetStatusAllExist();
                return true;

            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}, " + GetType().ToString() + ", LoadStream Error. msg=" + ex.Message, "[ERROR]::");
                return false;
            }
        }

        // 사용안함.
        public bool SaveText(StreamWriter writer) { return false; }
        public bool LoadText(StreamReader reader) { return false; }
        #endregion

        #region UtilFunction

        public void GetRoundedRectPath(ref GraphicsPath path, RectangleF rect)
        {
            path.Reset();

            float rLen = Math.Min(rect.Width, rect.Height);
            float diameter = (float)(rLen * _RoundRectFact);

            if (diameter == 0.0) diameter = 1.0f;
            RectangleF arcRect = new RectangleF(rect.Location, new SizeF(diameter, diameter));

            path.AddArc(arcRect, 180.0f, 90.0f);

            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270.0f, 90.0f);

            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0.0f, 90.0f);

            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90.0f, 90.0f);

            path.CloseFigure();
        }

        #endregion

        #region 이벤트들..

        void MouseLeaveE(object sender, EventArgs e)
        {
            if (_ActiveEnable)
            {
                _ActiveIndex = -1;
                Invalidate();
            }
            _IsMouseDown = false;
        }

        void MouseMoveE(object sender, MouseEventArgs e)
        {
            //int i,j;

            //double X1 = _DrawRect[0,0].X;
            //double Y1 = _DrawRect[0,0].Y;
            //double X2 = _DrawRect[_ColCount-1,_RowCount-1].X+_DrawRect[_ColCount-1,_RowCount-1].Width;
            //double Y2 = _DrawRect[_ColCount-1,_RowCount-1].Y+_DrawRect[_ColCount-1,_RowCount-1].Height;

            //int sx = (int)((e.X-X1)/((X2-X1)/_ColCount))-2;
            //if(sx <0)
            //    sx =0;
            //int sy = (int)((e.Y-Y1)/((Y2-Y1)/_RowCount))-2;
            //if(sy <0)
            //    sy =0;

            if (_SelectImage == null)
                return;
            if (e.X < 0 || e.X >= _SelectImage.Width)
                return;
            if (e.Y < 0 || e.Y >= _SelectImage.Height)
                return;

            int ix = 0;
            int iy = 0;

            if (_ActiveEnable || _SelectEnable)
            {
                ix = (_SelectImage.GetPixel(e.X, e.Y).ToArgb() & 0x00FFF000) >> 12;
                iy = (_SelectImage.GetPixel(e.X, e.Y).ToArgb() & 0x00000FFF);
            }

            if (_ActiveEnable)
            {

                if (ix > 0 && iy > 0)
                {
                    _ActiveIndex = (int)((ix - 1) + (iy - 1) * _ColCount);
                }
                else
                    _ActiveIndex = -1;


                //for (i = sx; i < _ColCount; i++)
                //{
                //    for (j= sy; j < _RowCount; j++)
                //    {
                //        if ( _DrawRect[i,j].Contains (e.X, e.Y) )
                //        {
                //            _ActiveIndex = i+j*_ColCount;
                //            goto Activejump;
                //        }
                //    }
                //}

                ////for (i = 0; i < _ColCount; i++)
                ////{
                ////    for (j= 0; j < _RowCount; j++)
                ////    {
                ////        if ( _DrawRect[i,j].Contains (e.X, e.Y) )
                ////        {
                ////            _ActiveIndex = i+j*_ColCount;
                ////            goto Activejump;
                ////        }
                ////    }
                ////}
                // Activejump:

                if (_ActiveIndex > -1)
                {
                    int col = _ActiveIndex % _ColCount;
                    int row = (int)(_ActiveIndex / _ColCount);

                    if (_DrawEnable)
                        Invalidate();
                    if (_OnActive != null)
                        _OnActive(this, _ActiveIndex, col, row, TriNum[col, row], AreaNum[col, row], _StripStatus[col, row]);
                }
            }

            if (_MouseMoveChangeEnable && _SelectEnable && _IsMouseDown)
            {

                if (ix > 0 && iy > 0)
                {
                    _SelectIndex = (int)((ix - 1) + (iy - 1) * _ColCount);
                }
                else
                    _SelectIndex = -1;

                //                _SelectIndex = -1;

                //                for (i = sx; i < _ColCount; i++)
                //                {
                //                    for (j= sy; j < _RowCount; j++)
                //                    {
                //                        if ( _DrawRect[i,j].Contains (e.X, e.Y) )
                //                        {
                //                            _SelectIndex = i+j*_ColCount;
                //                            goto SelectJump;
                //                        }
                //                    }
                //                }

                //                //for (i = 0; i < _ColCount; i++)
                //                //{
                //                //    for (j= 0; j < _RowCount; j++)
                //                //    {
                //                //        if ( _DrawRect[i,j].Contains (e.X, e.Y) )
                //                //        {
                //                //            _SelectIndex = i+j*_ColCount;
                //                //            goto SelectJump;
                //                //        }
                //                //    }
                //                //}

                //SelectJump:
                if (_SelectIndex > -1)
                {
                    int col = _SelectIndex % _ColCount;
                    int row = (int)(_SelectIndex / _ColCount);

                    if (_DrawEnable)
                        Invalidate();
                    if (_OnSelected != null)
                        _OnSelected(this, _SelectIndex, col, row, TriNum[col, row], AreaNum[col, row], _StripStatus[col, row]);
                }
            }

        }

        void MouseClickE(object sender, MouseEventArgs e)
        {
            //int i,j;
            //RectangleF ptf;
            if (_SelectEnable)
            {
                int ix = (_SelectImage.GetPixel(e.X, e.Y).ToArgb() & 0x00FFF000) >> 12;
                int iy = (_SelectImage.GetPixel(e.X, e.Y).ToArgb() & 0x00000FFF);
                if (ix > 0 && iy > 0)
                {
                    _SelectIndex = (int)((ix - 1) + (iy - 1) * _ColCount);
                }
                else
                    _SelectIndex = -1;

                //                double X1 = _DrawRect[0,0].X;
                //                double Y1 = _DrawRect[0,0].Y;
                //                double X2 = _DrawRect[_ColCount-1,_RowCount-1].X+_DrawRect[_ColCount-1,_RowCount-1].Width;
                //                double Y2 = _DrawRect[_ColCount-1,_RowCount-1].Y+_DrawRect[_ColCount-1,_RowCount-1].Height;

                //                int sx = (int)((e.X-X1)/((X2-X1)/_ColCount)) -2;
                //                if(sx <0)
                //                    sx =0;
                //                int sy = (int)((e.Y-Y1)/((Y2-Y1)/_RowCount)) -2;
                //                if(sy <0)
                //                    sy =0;

                //                _SelectIndex = -1;

                //                for (i = sx; i < _ColCount; i++)
                //                {
                //                    for (j= sy; j < _RowCount; j++)
                //                    {
                //                        ptf = new RectangleF(_DrawRect[i,j].X,_DrawRect[i,j].Y,_DrawRect[i,j].Width,_DrawRect[i,j].Height);
                //                        ptf.Inflate(_DistanceItem,_DistanceItem);
                //                        if ( ptf.Contains (e.X, e.Y) )
                //                        {
                //                            _SelectIndex = i+j*_ColCount;
                //                            goto SelectJump_Click;
                //                        }
                //                    }
                //                }

                //                //for (i = 0; i < _ColCount; i++)
                //                //{
                //                //    for (j= 0; j < _RowCount; j++)
                //                //    {
                //                //        ptf = new RectangleF(_DrawRect[i,j].X,_DrawRect[i,j].Y,_DrawRect[i,j].Width,_DrawRect[i,j].Height);
                //                //        ptf.Inflate(_DistanceItem,_DistanceItem);
                //                //        if ( ptf.Contains (e.X, e.Y) )
                //                //        {
                //                //            _SelectIndex = i+j*_ColCount;
                //                //            goto SelectJump_Click;
                //                //        }
                //                //    }
                //                //}
                //SelectJump_Click:               

                if (_SelectIndex > -1)
                {
                    int col = _SelectIndex % _ColCount;
                    int row = (int)(_SelectIndex / _ColCount);
                    if (_DrawEnable)
                        Invalidate();
                    if (_OnSelected != null)
                        _OnSelected(this, _SelectIndex, col, row, TriNum[col, row], AreaNum[col, row], _StripStatus[col, row]);
                }
            }
        }

        void MouseDoubleClickE(object sender, MouseEventArgs e)
        {

            //int i,j;
            if (_SelectEnable)
            {
                int ix = (_SelectImage.GetPixel(e.X, e.Y).ToArgb() & 0x00FFF000) >> 12;
                int iy = (_SelectImage.GetPixel(e.X, e.Y).ToArgb() & 0x00000FFF);
                if (ix > 0 && iy > 0)
                {
                    _SelectIndex = (int)((ix - 1) + (iy - 1) * _ColCount);
                }
                else
                    _SelectIndex = -1;

                //                double X1 = _DrawRect[0,0].X;
                //                double Y1 = _DrawRect[0,0].Y;
                //                double X2 = _DrawRect[_ColCount-1,_RowCount-1].X+_DrawRect[_ColCount-1,_RowCount-1].Width;
                //                double Y2 = _DrawRect[_ColCount-1,_RowCount-1].Y+_DrawRect[_ColCount-1,_RowCount-1].Height;

                //                int sx = (int)((e.X-X1)/((X2-X1)/_ColCount)) -2;
                //                if(sx <0)
                //                    sx =0;
                //                int sy = (int)((e.Y-Y1)/((Y2-Y1)/_RowCount)) -2;
                //                if(sy <0)
                //                    sy =0;

                //                _SelectIndex = -1;

                //                for (i = 0; i < _ColCount; i++)
                //                {
                //                    for (j = 0; j < _RowCount; j++)
                //                    {
                //                        if (_DrawRect[i, j].Contains(e.X, e.Y))
                //                        {
                //                            _SelectIndex = i+j*_ColCount;
                //                            goto SelectJump_DClick;
                //                        }
                //                    }
                //                }
                //SelectJump_DClick:

                if (_SelectIndex > -1)
                {
                    int col = _SelectIndex % _ColCount;
                    int row = (int)(_SelectIndex / _ColCount);
                    if (_DrawEnable)
                        Invalidate();
                    if (_OnSelectedDouble != null)
                        _OnSelectedDouble(this, _SelectIndex, col, row, TriNum[col, row], AreaNum[col, row], _StripStatus[col, row]);
                }
            }
        }

        void DrawReSize(object sender, EventArgs e)
        {
            if (_DrawEnable)
            {
                ReSizeRect();
                Invalidate();
            }
        }

        #endregion

        #region 일반함수
        public bool GetIdxToXY(int idx, out int col, out int row)
        {
            col = 0; row = 0;
            if (idx < 0 && idx >= (_RowCount * _ColCount))
                return false;
            col = idx % _ColCount;
            row = (int)(idx / _ColCount);
            return true;
        }

        public bool GetIdxToXY(int idx, out int scol, out int srow, out int ccol, out int crow)
        {
            scol = 0; srow = 0; ccol = 0; crow = 0;
            if (idx < 0 && idx >= (_RowCount * _ColCount))
                return false;
            scol = (int)((idx % _ColCount) / _SegCol);
            srow = (int)((int)(idx / _ColCount) / _SegRow);
            ccol = (idx % _ColCount) % _SegCol;
            crow = (int)(idx / _ColCount) % _SegRow;


            return true;
        }

        public bool GetXYToIdx(int col, int row, out int idx)
        {
            idx = 0;
            if ((col < 0 && col >= _ColCount) || (row < 0 && row >= _RowCount))
                return false;
            idx = col + row * _ColCount;
            return true;
        }

        public bool GetXYToIdx(int scol, int srow, int ccol, int crow, out int idx)
        {
            idx = (scol * _SegCol + ccol) + (srow * _SegRow + crow) * _ColCount;
            if (idx < 0 && idx >= (_RowCount * _ColCount))
            {
                idx = 0;
                return false;
            }
            return true;
        }

        public void SetStatusAll(eStripStatus status, bool setRank000)
        {
            int i, j;
            for (i = 0; i < _ColCount; i++)
            {
                for (j = 0; j < _RowCount; j++)
                {
                    if (setRank000)
                        _StripStatus[i, j] = status;
                    else
                        if (_StripStatus[i, j] != eStripStatus.Rank000)
                        _StripStatus[i, j] = status;
                }
            }

            if (_DrawEnable)
                Invalidate();
        }

        public void SetStatusAllExist()
        {
            int i, j;
            for (i = 0; i < _ColCount; i++)
            {
                for (j = 0; j < _RowCount; j++)
                {
                    if (_StripStatus[i, j] != eStripStatus.Rank000)
                        _StripStatus[i, j] = eStripStatus.Exist;
                }
            }
            if (_DrawEnable)
                Invalidate();
        }

        public void SetStatus(int index, eStripStatus sts_value, bool paint)
        {
            Debug.Assert((index >= 0) && (index < _ColCount * _RowCount), string.Format("SetStatus(index) ==> index < 0 or index >= {0}", (_ColCount * _RowCount).ToString()), "[ERROR]::");
            _StripStatus[index % _ColCount, (int)(index / _ColCount)] = sts_value;
            if (paint)
                DrawCell(index % _ColCount, (int)(index / _ColCount));
        }

        public void SetStatus(int colidx, int rowidx, eStripStatus sts_value, bool paint)
        {
            Debug.Assert((colidx >= 0) && (colidx < _ColCount) && (rowidx >= 0) && (rowidx < _RowCount),
                        string.Format("SetStatus(col idx, row idx) ==> col/row idx < 0 or col idx >= {0} or row idx >= {1}", _ColCount.ToString(), _RowCount.ToString()), "[ERROR]::");
            _StripStatus[colidx, rowidx] = sts_value;
            if (paint)
                DrawCell(colidx, rowidx);
        }

        public void SetStatus(int segcolidx, int segrowidx, int cellcolidx, int cellrowidx, eStripStatus sts_value, bool paint)
        {
            _StripStatus[segcolidx * _SegCol + cellcolidx, segrowidx * _SegRow + cellrowidx] = sts_value;
            if (paint)
                DrawCell(segcolidx * _SegCol + cellcolidx, segrowidx * _SegRow + cellrowidx);
        }


        public eStripStatus GetStatus(int index)
        {
            Debug.Assert((index >= 0) && (index < _ColCount * _RowCount), string.Format("GetStatus(index) ==> index < 0 or index >= {0}", (_ColCount * _RowCount).ToString()), "[ERROR]::");
            return _StripStatus[index % _ColCount, (int)(index / _ColCount)];
        }

        public eStripStatus GetStatus(int colidx, int rowidx)
        {
            Debug.Assert((colidx >= 0) && (colidx < _ColCount) && (rowidx >= 0) && (rowidx < _RowCount),
                        string.Format("GetStatus(col idx, row idx) ==> col/row idx < 0 or col idx >= {0} or row idx >= {1}", _ColCount.ToString(), _RowCount.ToString()), "[ERROR]::");
            return _StripStatus[colidx, rowidx];
        }


        public void SetTriNum(int index, float value)
        {
            Debug.Assert((index >= 0) && (index < _ColCount * _RowCount), string.Format("SetTriNum(index) ==> index < 0 or index >= {0}", (_ColCount * _RowCount).ToString()), "[ERROR]::");
            TriNum[index % _ColCount, (int)(index / _ColCount)] = value;

            if (_DrawEnable)
            {
                DrawTriString(index % _ColCount, (int)(index / _ColCount));
                DrawCell(index % _ColCount, (int)(index / _ColCount));
            }
        }

        public void SetTriNum(int colidx, int rowidx, float value)
        {
            Debug.Assert((colidx >= 0) && (colidx < _ColCount) && (rowidx >= 0) && (rowidx < _RowCount),
                        string.Format("SetTriNum(col idx, row idx) ==> col/row idx < 0 or col idx >= {0} or row idx >= {1}", _ColCount.ToString(), _RowCount.ToString()), "[ERROR]::");
            TriNum[colidx, rowidx] = value;

            if (_DrawEnable)
            {
                DrawTriString(colidx, rowidx);
                DrawCell(colidx, rowidx);
            }
        }

        public float GetTriNum(int index)
        {
            Debug.Assert((index >= 0) && (index < _ColCount * _RowCount), string.Format("GetTriNum(index) ==> index < 0 or index >= {0}", (_ColCount * _RowCount).ToString()), "[ERROR]::");
            return TriNum[index % _ColCount, (int)(index / _ColCount)];
        }

        public float GetTriNum(int colidx, int rowidx)
        {
            Debug.Assert((colidx >= 0) && (colidx < _ColCount) && (rowidx >= 0) && (rowidx < _RowCount),
                        string.Format("GetTriNum(col idx, row idx) ==> col/row idx < 0 or col idx >= {0} or row idx >= {1}", _ColCount.ToString(), _RowCount.ToString()), "[ERROR]::");
            return TriNum[colidx, rowidx];
        }

        public void SetAreaNum(int index, int value)
        {
            Debug.Assert((index >= 0) && (index < _ColCount * _RowCount), string.Format("SetAreaNum(index) ==> index < 0 or index >= {0}", (_ColCount * _RowCount).ToString()), "[ERROR]::");
            AreaNum[index % _ColCount, (int)(index / _ColCount)] = value;

            if (_DrawEnable)
            {
                DrawString(index % _ColCount, (int)(index / _ColCount));
                DrawCell(index % _ColCount, (int)(index / _ColCount));
            }
        }

        public void SetAreaNum(int colidx, int rowidx, int value)
        {
            Debug.Assert((colidx >= 0) && (colidx < _ColCount) && (rowidx >= 0) && (rowidx < _RowCount),
                        string.Format("SetAreaNum(col idx, row idx) ==> col/row idx < 0 or col idx >= {0} or row idx >= {1}", _ColCount.ToString(), _RowCount.ToString()), "[ERROR]::");
            AreaNum[colidx, rowidx] = value;

            if (_DrawEnable)
            {
                DrawString(colidx, rowidx);
                DrawCell(colidx, rowidx);
            }
        }

        public int GetAreaNum(int index)
        {
            Debug.Assert((index >= 0) && (index < _ColCount * _RowCount), string.Format("GetAreaNum(index) ==> index < 0 or index >= {0}", (_ColCount * _RowCount).ToString()), "[ERROR]::");
            return AreaNum[index % _ColCount, (int)(index / _ColCount)];
        }

        public int GetAreaNum(int colidx, int rowidx)
        {
            Debug.Assert((colidx >= 0) && (colidx < _ColCount) && (rowidx >= 0) && (rowidx < _RowCount), string.Format("GetAreaNum(col idx, row idx) ==> col/row idx < 0 or col idx >= {0} or row idx >= {1}", _ColCount.ToString(), _RowCount.ToString()), "[ERROR]::");
            return AreaNum[colidx, rowidx];
        }

        public void ReStruct()
        {
            if ((_StripStatus == null) || (_StripStatus.Length != _RowCount * _ColCount))
            {
                _StripStatus = new eStripStatus[_ColCount, _RowCount];
                for (int i = 0; i < _ColCount; i++)
                    for (int j = 0; j < _RowCount; j++)
                        _StripStatus[i, j] = eStripStatus.Exist;
            }
            if ((_LabelRect == null) || (_LabelRect.Length != (_ColCount + _RowCount + 1)))
                _LabelRect = new RectangleF[_ColCount + _RowCount + 1];
            if ((_DrawRect == null) || (_DrawRect.Length != (_RowCount * _ColCount)))
                _DrawRect = new RectangleF[_ColCount, _RowCount];
            if ((_LabelString == null) || (_LabelString.Length != (_ColCount + _RowCount + 1)))
                _LabelString = new string[_ColCount + _RowCount + 1];
            if ((_DrawString == null) || (_DrawString.Length != (_RowCount * _ColCount)))
                _DrawString = new string[_ColCount, _RowCount];
            if ((TriNum == null) || (TriNum.Length != (_RowCount * _ColCount)))
                TriNum = new float[_ColCount, _RowCount];
            if ((AreaNum == null) || (AreaNum.Length != (_RowCount * _ColCount)))
                AreaNum = new int[_ColCount, _RowCount];
            if ((_DrawTriStr == null) || (_DrawTriStr.Length != (_RowCount * _ColCount)))
                _DrawTriStr = new string[_ColCount, _RowCount];


        }

        public void ReSizeRect()
        {
            float orgx, orgy;
            float tw, th, sizex, sizey;

            int segColCount, segRowCount;
            int tmpColCnt, tmpRowCnt;  //
            int i, j;

            if (_VisibleLabel)
            {
                tmpColCnt = _ColCount + 1;
                tmpRowCnt = _RowCount + 1;
            }
            else
            {
                tmpColCnt = _ColCount;
                tmpRowCnt = _RowCount;
            }

            if ((_ColCount % _SegCol) != 0)
                segColCount = (int)(_ColCount / _SegCol) + 1;
            else
                segColCount = (int)(_ColCount / _SegCol);

            if ((_RowCount % _SegRow) != 0)
                segRowCount = (int)(_RowCount / _SegRow) + 1;
            else
                segRowCount = (int)(_RowCount / _SegRow);


            orgx = _BoarderSize + _DrawOffset.Left;  // 그리기 기준 위치
            orgy = _BoarderSize + _DrawOffset.Top;   // 

            tw = Width - _BoarderSize * 2 - _DrawOffset.Left - _DrawOffset.Right - 1;  // 그릴 공간
            th = Height - _BoarderSize * 2 - _DrawOffset.Top - _DrawOffset.Bottom - 1;

            sizex = (tw - ((segColCount - 1) * _DistanceSegment) - ((tmpColCnt - 1 - (segColCount - 1)) * _DistanceItem)) / tmpColCnt;  // Rectangle 크기
            sizey = (th - ((segRowCount - 1) * _DistanceSegment) - ((tmpRowCnt - 1 - (segRowCount - 1)) * _DistanceItem)) / tmpRowCnt;  // Rectangle 크기

            if (_VisibleLabel)
            {
                for (i = 0; i < _ColCount; i++)
                {
                    _LabelRect[i].X = (orgx + (i + 1) * (sizex + _DistanceItem) + ((int)(i / _SegCol)) * (_DistanceSegment - _DistanceItem));
                    _LabelRect[i].Y = orgy;
                    _LabelRect[i].Width = sizex;
                    _LabelRect[i].Height = sizey;
                }

                for (i = 0; i < _RowCount; i++)
                {
                    _LabelRect[_ColCount + i].X = orgx;
                    _LabelRect[_ColCount + i].Y = (orgy + (i + 1) * (sizey + _DistanceItem) + ((int)(i / _SegRow)) * (_DistanceSegment - _DistanceItem));
                    _LabelRect[_ColCount + i].Width = sizex;
                    _LabelRect[_ColCount + i].Height = sizey;
                }

                _LabelRect[_ColCount + _RowCount].X = orgx;
                _LabelRect[_ColCount + _RowCount].Y = orgy;
                _LabelRect[_ColCount + _RowCount].Width = sizex;
                _LabelRect[_ColCount + _RowCount].Height = sizey;


                for (i = 0; i < _ColCount; i++)
                {
                    for (j = 0; j < _RowCount; j++)
                    {
                        _DrawRect[i, j].X = (orgx + (i + 1) * (sizex + _DistanceItem) + ((int)(i / _SegCol)) * (_DistanceSegment - _DistanceItem));
                        _DrawRect[i, j].Y = (orgy + (j + 1) * (sizey + _DistanceItem) + ((int)(j / _SegRow)) * (_DistanceSegment - _DistanceItem));
                        _DrawRect[i, j].Width = sizex;
                        _DrawRect[i, j].Height = sizey;
                    }
                }
            }
            else
            {
                for (i = 0; i < _ColCount; i++)
                {
                    for (j = 0; j < _RowCount; j++)
                    {
                        _DrawRect[i, j].X = (orgx + i * (sizex + _DistanceItem) + ((int)(i / _SegCol)) * (_DistanceSegment - _DistanceItem));
                        _DrawRect[i, j].Y = (orgy + j * (sizey + _DistanceItem) + ((int)(j / _SegRow)) * (_DistanceSegment - _DistanceItem));
                        _DrawRect[i, j].Width = sizex;
                        _DrawRect[i, j].Height = sizey;
                    }
                }
            }


            DrawLabelString();
            DrawString();
            DrawTriString();

            if (_SelectEnable || _ActiveEnable)
            {
                _SelectImage = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics selgra = Graphics.FromImage(_SelectImage);
                SolidBrush sb;
                selgra.Clear(Color.FromArgb(255, 0, 0, 0));

                for (i = 0; i < _ColCount; i++)
                {
                    for (j = 0; j < _RowCount; j++)
                    {
                        sb = new SolidBrush(Color.FromArgb((int)(0xFF000000 | (((i + 1) << 12) | (j + 1))))); // (int)(((i+1)<<12) | (j+1))));
                        //GetRoundedRectPath(ref path, _DrawRect[i,j]);                    
                        selgra.FillRectangle(sb, _DrawRect[i, j]);
                    }
                }
            }
            else
                _SelectImage = null;

        }

        public void DrawLabelString()
        {
            int i;

            for (i = 0; i < _ColCount; i++)
            {
                if ((_ColNumType == eStripNumType.Alphabet) && (_ColCount < 27))
                {
                    if (_ColNumLeftToRight)
                        _LabelString[i] = AlphabetString.Substring(i, 1);
                    else
                        _LabelString[i] = AlphabetString.Substring(_ColCount - i - 1, 1);
                }
                else
                {
                    if (_ColNumLeftToRight)
                        _LabelString[i] = i.ToString().PadLeft(2, '0');
                    else
                        _LabelString[i] = (_ColCount - 1 - i).ToString().PadLeft(2, '0');
                }
            }

            for (i = _ColCount; i < _ColCount + _RowCount; i++)
            {

                if ((_RowNumType == eStripNumType.Alphabet) && (_RowCount < 27))
                {
                    if (_RowNumTopToBottom)
                        _LabelString[i] = AlphabetString.Substring(i - _ColCount, 1);
                    else
                        _LabelString[i] = AlphabetString.Substring(_ColCount + _RowCount - i - 1, 1);
                }
                else
                {
                    if (_RowNumTopToBottom)
                        _LabelString[i] = (i - _ColCount).ToString().PadLeft(2, '0');
                    else
                        _LabelString[i] = (_ColCount + _RowCount - 1 - i).ToString().PadLeft(2, '0');

                }
            }

            switch (_Direction)
            {
                case eStripDirection.None:
                    _LabelString[_RowCount + _ColCount] = "";
                    break;
                case eStripDirection.LeftToRight:
                    _LabelString[_RowCount + _ColCount] = "▶▶";
                    break;
                case eStripDirection.RIghtToLeft:
                    _LabelString[_RowCount + _ColCount] = "◀◀";
                    break;
                default:
                    _LabelString[_RowCount + _ColCount] = "";
                    break;
            }
        }


        public void DrawString()
        {
            int i, j;
            for (i = 0; i < _ColCount; i++)
            {
                for (j = 0; j < _RowCount; j++)
                {
                    if ((_RowNumType == eStripNumType.Alphabet) && (_RowCount < 27))
                    {
                        if (_RowNumTopToBottom)
                            _DrawString[i, j] = AlphabetString.Substring(j, 1);
                        else
                            _DrawString[i, j] = AlphabetString.Substring(_RowCount - j - 1, 1);

                    }
                    else
                    {
                        if (_RowNumTopToBottom)
                            _DrawString[i, j] = (j + 1).ToString().PadLeft(2, '0');
                        else
                            _DrawString[i, j] = (_RowCount - j).ToString().PadLeft(2, '0');
                    }

                    if ((_ColNumType == eStripNumType.Alphabet) && (_ColCount < 27))
                    {
                        if (_ColNumLeftToRight)
                            _DrawString[i, j] = _DrawString[i, j] + "-" + AlphabetString.Substring(i, 1);
                        else
                            _DrawString[i, j] = _DrawString[i, j] + "-" + AlphabetString.Substring(_ColCount - i - 1, 1);
                    }
                    else
                    {
                        if (_ColNumLeftToRight)
                            _DrawString[i, j] = _DrawString[i, j] + "-" + (i + 1).ToString().PadLeft(2, '0');
                        else
                            _DrawString[i, j] = _DrawString[i, j] + "-" + (_ColCount - i).ToString().PadLeft(2, '0');
                    }
                }
            }
        }

        public void DrawString(int col, int row)
        {
            if ((_RowNumType == eStripNumType.Alphabet) && (_RowCount < 27))
            {
                if (_RowNumTopToBottom)
                    _DrawString[col, row] = AlphabetString.Substring(row, 1);
                else
                    _DrawString[col, row] = AlphabetString.Substring(_RowCount - row - 1, 1);

            }
            else
            {
                if (_RowNumTopToBottom)
                    _DrawString[col, row] = (row + 1).ToString().PadLeft(2, '0');
                else
                    _DrawString[col, row] = (_RowCount - row).ToString().PadLeft(2, '0');
            }

            if ((_ColNumType == eStripNumType.Alphabet) && (_ColCount < 27))
            {
                if (_ColNumLeftToRight)
                    _DrawString[col, row] = _DrawString[col, row] + "-" + AlphabetString.Substring(col, 1);
                else
                    _DrawString[col, row] = _DrawString[col, row] + "-" + AlphabetString.Substring(_ColCount - col - 1, 1);
            }
            else
            {
                if (_ColNumLeftToRight)
                    _DrawString[col, row] = _DrawString[col, row] + "-" + (col + 1).ToString().PadLeft(2, '0');
                else
                    _DrawString[col, row] = _DrawString[col, row] + "-" + (_ColCount - col).ToString().PadLeft(2, '0');
            }
        }

        public void DrawTriString()
        {
            int i, j;
            for (i = 0; i < _ColCount; i++)
            {
                for (j = 0; j < _RowCount; j++)
                {
                    _DrawTriStr[i, j] = string.Empty;
                    if (_VisibleTriNum)
                    {
                        if (_TriNumFormat.Trim().Length > 0 && (_TriNumFormat.Trim()[0] == 'D' || _TriNumFormat.Trim()[0] == 'd'))
                            _DrawTriStr[i, j] = ((int)TriNum[i, j]).ToString(_TriNumFormat);
                        else
                            _DrawTriStr[i, j] = TriNum[i, j].ToString(_TriNumFormat);
                        if (_VisibleAreaNum)
                            _DrawTriStr[i, j] += "-" + AreaNum[i, j].ToString(_TriNumFormat);
                    }
                    else
                    {
                        if (_VisibleAreaNum)
                            _DrawTriStr[i, j] += AreaNum[i, j].ToString(_TriNumFormat);
                    }
                }
            }
        }

        public void DrawTriString(int col, int row)
        {
            _DrawTriStr[col, row] = string.Empty;
            if (_VisibleTriNum)
            {
                if (_TriNumFormat.Trim().Length > 0 && (_TriNumFormat.Trim()[0] == 'D' || _TriNumFormat.Trim()[0] == 'd'))
                    _DrawTriStr[col, row] = ((int)TriNum[col, row]).ToString(_TriNumFormat);
                else
                    _DrawTriStr[col, row] = TriNum[col, row].ToString(_TriNumFormat);
                if (_VisibleAreaNum)
                    _DrawTriStr[col, row] += "-" + AreaNum[col, row].ToString(_TriNumFormat);
            }
            else
            {
                if (_VisibleAreaNum)
                    _DrawTriStr[col, row] += AreaNum[col, row].ToString(_TriNumFormat);
            }
        }


        public void ChangedColor()
        {
            ActivePen = new Pen(_ColorActive, 1);
            SelectPen = new Pen(_ColorSelect, 1);
            boarderPen = new Pen(_ColorBoarder, 1);
            itemBrushLabel = new SolidBrush(_ColorLabel);
            itemBoarderPen = new Pen(_ColorItemBoarder, _LineWeight);
            triBrush = new SolidBrush(_ColorTriAreaNum);

            itemDrawBrush = new SolidBrush[ColorRanks.Length];
            for (int i = 0; i < ColorRanks.Length; i++)
                itemDrawBrush[i] = new SolidBrush(_ColorRanks[i]);

            fontBrush = new SolidBrush(this.ForeColor);
            strfm = new StringFormat();
            strfm.Alignment = StringAlignment.Center;
            strfm.LineAlignment = StringAlignment.Center;
            txtpt = new PointF();
            tmpRect = new RectangleF();
            path = new GraphicsPath();
        }

        //public void ChangedColorRank(int idx)
        //{
        //    itemDrawBrush[idx+10] = new SolidBrush(_ColorRanks[idx]);
        //}

        public void SetRankColor(byte idx, Color color)
        {
            _ColorRanks[idx] = color;
            itemDrawBrush[idx] = new SolidBrush(_ColorRanks[idx]);
        }


        public void DrawCell(int col, int row)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    dEventStripDrawCell1 drawcell = new dEventStripDrawCell1(DrawCell);
                    this.Invoke(drawcell, new object[] { col, row });
                }
                else
                {
                    int index = col + row * _ColCount;
                    Debug.Assert((index >= 0) && (index < _ColCount * _RowCount), string.Format("DrawCell(index) ==> index < 0 or index >= {0}", (_ColCount * _RowCount)), "[ERROR]::");

                    //gra = Graphics.FromHwnd(this.Handle);
                    gra.SmoothingMode = SmoothingMode.HighSpeed;

                    //Pen ActivePen              = new Pen(_ColorActive, 1);
                    //Pen SelectPen              = new Pen(_ColorSelect, 1);
                    //Pen boarderPen             = new Pen(_ColorBoarder, 1);
                    //SolidBrush itemBrushLabel  = new SolidBrush(_ColorLabel);
                    //Pen itemBoarderPen         = new Pen(_ColorItemBoarder, _LineWeight);
                    //SolidBrush triBrush        = new SolidBrush(_ColorTriAreaNum);
                    //SolidBrush[] itemDrawBrush = new SolidBrush[10];
                    //itemDrawBrush[0]           = new SolidBrush(SystemColors.ButtonFace);
                    //itemDrawBrush[1]           = new SolidBrush(_ColorEmpty);
                    //itemDrawBrush[2]           = new SolidBrush(_ColorExists);
                    //itemDrawBrush[3]           = new SolidBrush(_ColorSkip);
                    //itemDrawBrush[4]           = new SolidBrush(_ColorPart);
                    //itemDrawBrush[5]           = new SolidBrush(_ColorRunning);
                    //itemDrawBrush[6]           = new SolidBrush(_ColorFinished);
                    //itemDrawBrush[7]           = new SolidBrush(_ColorError);
                    //itemDrawBrush[8]           = new SolidBrush(_ColorUnknown);
                    //itemDrawBrush[9]           = new SolidBrush(_ColorSelected);
                    //SolidBrush fontBrush = new SolidBrush(this.ForeColor);


                    //StringFormat strfm   = new StringFormat();
                    //strfm.Alignment      = StringAlignment.Center;
                    //strfm.LineAlignment  = StringAlignment.Center;
                    //PointF       txtpt   = new PointF();
                    //RectangleF   tmpRect = new RectangleF();
                    //GraphicsPath path    = new GraphicsPath();


                    GetRoundedRectPath(ref path, _DrawRect[col, row]);
                    gra.FillPath(itemDrawBrush[(ushort)_StripStatus[col, row]], path);

                    if (_VisibleNumber)
                    {
                        if (_VisibleTriNum || _VisibleAreaNum)
                        {
                            txtpt.X = (float)(_DrawRect[col, row].X + _DrawRect[col, row].Width / 2.0);
                            txtpt.Y = (float)(_DrawRect[col, row].Y + _DrawRect[col, row].Height / 3.0);

                            if (_StripStatus[col, row] != eStripStatus.Rank000)  // _StripStatus[col,row] != eStripStatus.Empty && 
                                gra.DrawString(_DrawString[col, row], this.Font, fontBrush, txtpt.X, txtpt.Y, strfm);

                            txtpt.X = (float)(_DrawRect[col, row].X + _DrawRect[col, row].Width / 2.0);
                            txtpt.Y = (float)(_DrawRect[col, row].Y + _DrawRect[col, row].Height * 3.0 / 4.0);
                            if (_StripStatus[col, row] != eStripStatus.Rank000)  //_StripStatus[col,row] != eStripStatus.Empty && 
                                gra.DrawString(_DrawTriStr[col, row], this.Font, triBrush, txtpt.X, txtpt.Y, strfm);
                        }
                        else
                        {
                            txtpt.X = (float)(_DrawRect[col, row].X + _DrawRect[col, row].Width / 2.0);
                            txtpt.Y = (float)(_DrawRect[col, row].Y + _DrawRect[col, row].Height / 2.0);
                            if (_StripStatus[col, row] != eStripStatus.Rank000)  //_StripStatus[col,row] != eStripStatus.Empty && 
                                gra.DrawString(_DrawString[col, row], this.Font, fontBrush, txtpt.X, txtpt.Y, strfm);
                        }
                    }
                    else
                    {
                        if (_VisibleTriNum || _VisibleAreaNum)
                        {
                            txtpt.X = (float)(_DrawRect[col, row].X + _DrawRect[col, row].Width / 2.0);
                            txtpt.Y = (float)(_DrawRect[col, row].Y + _DrawRect[col, row].Height / 2.0);
                            if (_StripStatus[col, row] != eStripStatus.Rank000) //_StripStatus[col,row] != eStripStatus.Empty && 
                                gra.DrawString(_DrawTriStr[col, row], this.Font, triBrush, txtpt.X, txtpt.Y, strfm);
                        }
                    }



                    if (_StripStatus[col, row] != eStripStatus.Rank000)
                        gra.DrawPath(itemBoarderPen, path);

                    if (_SelectEnable && (_SelectIndex == index))
                    {
                        tmpRect.X = _DrawRect[col, row].X + 1;
                        tmpRect.Y = _DrawRect[col, row].Y + 1;
                        tmpRect.Width = _DrawRect[col, row].Width - 2;
                        tmpRect.Height = _DrawRect[col, row].Height - 2;
                        GetRoundedRectPath(ref path, tmpRect);
                        gra.DrawPath(SelectPen, path);
                    }

                    if (_ActiveEnable && (_ActiveIndex == index))
                    {
                        tmpRect.X = _DrawRect[col, row].X + 2;
                        tmpRect.Y = _DrawRect[col, row].Y + 2;
                        tmpRect.Width = _DrawRect[col, row].Width - 4;
                        tmpRect.Height = _DrawRect[col, row].Height - 4;
                        GetRoundedRectPath(ref path, tmpRect);
                        gra.DrawPath(ActivePen, path);
                    }

                    //this.Invalidate(new Rectangle((int)_DrawRect[index].X,(int)_DrawRect[index].Y,(int)_DrawRect[index].Width,(int)_DrawRect[index].Height),false);
                }
            }
            catch (Exception e)
            {
                Trace.Write("STRIP DrawCell(index),  err=" + e.Message, "[ERROR]::");
            }
        }

        private void DrawCtrl(object sender, PaintEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    dEventStripDrawCell2 drawctrl = new dEventStripDrawCell2(DrawCtrl);
                    this.Invoke(drawctrl, new object[] { sender, e });
                }
                else
                {
                    gra = e.Graphics; //Graphics.FromHwnd(this.Handle);
                    gra.SmoothingMode = SmoothingMode.HighSpeed;
                    //Pen        ActivePen      = new Pen        (_ColorActive , 1);
                    //Pen        SelectPen      = new Pen        (_ColorSelect , 1);
                    //Pen        boarderPen     = new Pen        (_ColorBoarder, 1);
                    //SolidBrush itemBrushLabel = new SolidBrush (_ColorLabel);
                    //Pen        itemBoarderPen = new Pen        (_ColorItemBoarder, _LineWeight);
                    //SolidBrush triBrush       = new SolidBrush (this._ColorTriAreaNum);
                    //SolidBrush[] itemDrawBrush = new SolidBrush[10];
                    //itemDrawBrush[0] = new SolidBrush(SystemColors.ButtonFace);
                    //itemDrawBrush[1] = new SolidBrush(_ColorEmpty);
                    //itemDrawBrush[2] = new SolidBrush(_ColorExists);
                    //itemDrawBrush[3] = new SolidBrush(_ColorSkip);
                    //itemDrawBrush[4] = new SolidBrush(_ColorPart);
                    //itemDrawBrush[5] = new SolidBrush(_ColorRunning);
                    //itemDrawBrush[6] = new SolidBrush(_ColorFinished);
                    //itemDrawBrush[7] = new SolidBrush(_ColorError);
                    //itemDrawBrush[8] = new SolidBrush(_ColorUnknown);
                    //itemDrawBrush[9] = new SolidBrush(_ColorSelected);
                    //SolidBrush fontBrush  = new SolidBrush (this.ForeColor);
                    //StringFormat strfm = new StringFormat();
                    //strfm.Alignment = StringAlignment.Center;
                    //strfm.LineAlignment = StringAlignment.Center;
                    //PointF txtpt = new PointF();
                    //GraphicsPath path = new GraphicsPath ();

                    int i, j;

                    for (int m = 0; m < _BoarderSize; m++)
                        gra.DrawRectangle(boarderPen, m, m, Width - 1 - m * 2, Height - 1 - m * 2);

                    if (_VisibleLabel)
                    {
                        for (i = 0; i < _ColCount; i++)
                        {
                            GetRoundedRectPath(ref path, _LabelRect[i]);
                            gra.FillPath(itemBrushLabel, path);

                            txtpt.X = (float)(_LabelRect[i].X + _LabelRect[i].Width / 2.0);
                            txtpt.Y = (float)(_LabelRect[i].Y + _LabelRect[i].Height / 2.0);

                            gra.DrawString(_LabelString[i], this.Font, fontBrush, txtpt.X, txtpt.Y, strfm);

                            gra.DrawPath(itemBoarderPen, path);
                        }

                        for (i = _ColCount; i < _ColCount + _RowCount; i++)
                        {
                            GetRoundedRectPath(ref path, _LabelRect[i]);
                            gra.FillPath(itemBrushLabel, path);

                            txtpt.X = (float)(_LabelRect[i].X + _LabelRect[i].Width / 2.0);
                            txtpt.Y = (float)(_LabelRect[i].Y + _LabelRect[i].Height / 2.0);

                            gra.DrawString(_LabelString[i], this.Font, fontBrush, txtpt.X, txtpt.Y, strfm);
                            gra.DrawPath(itemBoarderPen, path);
                        }

                        GetRoundedRectPath(ref path, _LabelRect[_ColCount + _RowCount]);
                        txtpt.X = (float)(_LabelRect[_ColCount + _RowCount].X + _LabelRect[_ColCount + _RowCount].Width / 2.0);
                        txtpt.Y = (float)(_LabelRect[_ColCount + _RowCount].Y + _LabelRect[_ColCount + _RowCount].Height / 2.0);

                        gra.DrawString(_LabelString[_ColCount + _RowCount], this.Font, fontBrush, txtpt.X, txtpt.Y, strfm);
                    }


                    RectangleF tmpRect = new RectangleF();
                    for (i = 0; i < _ColCount; i++)
                    {
                        for (j = 0; j < _RowCount; j++)
                        {
                            GetRoundedRectPath(ref path, _DrawRect[i, j]);
                            gra.FillPath(itemDrawBrush[(ushort)_StripStatus[i, j]], path);


                            if (_VisibleNumber)
                            {
                                if (_VisibleTriNum || _VisibleAreaNum)
                                {
                                    txtpt.X = (float)(_DrawRect[i, j].X + _DrawRect[i, j].Width / 2.0);
                                    txtpt.Y = (float)(_DrawRect[i, j].Y + _DrawRect[i, j].Height / 3.0);
                                    if (_StripStatus[i, j] != eStripStatus.Rank000)  //_StripStatus[i,j] != eStripStatus.Empty && 
                                        gra.DrawString(_DrawString[i, j], this.Font, fontBrush, txtpt.X, txtpt.Y, strfm);

                                    txtpt.X = (float)(_DrawRect[i, j].X + _DrawRect[i, j].Width / 2.0);
                                    txtpt.Y = (float)(_DrawRect[i, j].Y + _DrawRect[i, j].Height * 3.0 / 4.0);
                                    if (_StripStatus[i, j] != eStripStatus.Rank000)  //_StripStatus[i,j] != eStripStatus.Empty && 
                                        gra.DrawString(_DrawTriStr[i, j], this.Font, triBrush, txtpt.X, txtpt.Y, strfm);
                                }
                                else
                                {
                                    txtpt.X = (float)(_DrawRect[i, j].X + _DrawRect[i, j].Width / 2.0);
                                    txtpt.Y = (float)(_DrawRect[i, j].Y + _DrawRect[i, j].Height / 2.0);
                                    if (_StripStatus[i, j] != eStripStatus.Rank000) //_StripStatus[i,j] != eStripStatus.Empty && 
                                        gra.DrawString(_DrawString[i, j], this.Font, fontBrush, txtpt.X, txtpt.Y, strfm);
                                }
                            }
                            else
                            {
                                if (_VisibleTriNum || _VisibleAreaNum)
                                {
                                    txtpt.X = (float)(_DrawRect[i, j].X + _DrawRect[i, j].Width / 2.0);
                                    txtpt.Y = (float)(_DrawRect[i, j].Y + _DrawRect[i, j].Height / 2.0);
                                    if (_StripStatus[i, j] != eStripStatus.Rank000) //_StripStatus[i,j] != eStripStatus.Empty && 
                                        gra.DrawString(_DrawTriStr[i, j], this.Font, triBrush, txtpt.X, txtpt.Y, strfm);
                                }
                            }

                            if (_StripStatus[i, j] != eStripStatus.Rank000)
                                gra.DrawPath(itemBoarderPen, path);

                            if (_SelectEnable && (_SelectIndex == i + j * _ColCount))
                            {

                                tmpRect.X = _DrawRect[i, j].X + 1;
                                tmpRect.Y = _DrawRect[i, j].Y + 1;
                                tmpRect.Width = _DrawRect[i, j].Width - 2;
                                tmpRect.Height = _DrawRect[i, j].Height - 2;

                                GetRoundedRectPath(ref path, tmpRect);
                                gra.DrawPath(SelectPen, path);
                            }

                            if (_ActiveEnable && (_ActiveIndex == i + j * _ColCount))
                            {
                                tmpRect.X = _DrawRect[i, j].X + 2;
                                tmpRect.Y = _DrawRect[i, j].Y + 2;
                                tmpRect.Width = _DrawRect[i, j].Width - 4;
                                tmpRect.Height = _DrawRect[i, j].Height - 4;
                                GetRoundedRectPath(ref path, tmpRect);
                                gra.DrawPath(ActivePen, path);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Write("STRIP DrawCell(sender,PaintEventArgs),  err=" + ex.Message, "[ERROR]::");
            }
        }

        #endregion

        #region Property 들..

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Active 색상")]
        public Color ColorActive
        {
            get { return _ColorActive; }
            set
            {
                if (_ColorActive != value)
                {
                    _ColorActive = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Select 색상")]
        public Color ColorSelect
        {
            get { return _ColorSelect; }
            set
            {
                if (_ColorSelect != value)
                {
                    _ColorSelect = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }


        [Category("Control Color")]
        [Browsable(true)]
        [Description("Boarder 색상")]
        public Color ColorBoarder
        {
            get { return _ColorBoarder; }
            set
            {
                if (_ColorBoarder != value)
                {
                    _ColorBoarder = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }


        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell Label 표시 색상")]
        public Color ColorLabel
        {
            get { return _ColorLabel; }
            set
            {
                if (_ColorLabel != value)
                {
                    _ColorLabel = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Item Boarder 색상")]
        public Color ColorItemBoarder
        {
            get { return _ColorItemBoarder; }
            set
            {
                if (_ColorItemBoarder != value)
                {
                    _ColorItemBoarder = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Trigger & Area Number 색상")]
        public Color ColorTriAreaNum
        {
            get { return _ColorTriAreaNum; }
            set
            {
                if (_ColorTriAreaNum != value)
                {
                    _ColorTriAreaNum = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell RankZero 상태 색상")]
        public Color ColorRankZero
        {
            get { return _ColorRanks[0]; }
            set
            {
                if (_ColorRanks[0] != value)
                {
                    _ColorRanks[0] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell AlignKey 상태 색상")]
        public Color ColorAlignKey
        {
            get { return _ColorRanks[247]; }
            set
            {
                if (_ColorRanks[247] != value)
                {
                    _ColorRanks[247] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell Align Center 상태 색상")]
        public Color ColorAlignCen
        {
            get { return _ColorRanks[248]; }
            set
            {
                if (_ColorRanks[248] != value)
                {
                    _ColorRanks[248] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell Data Only 상태 색상")]
        public Color ColorDataOnly
        {
            get { return _ColorRanks[249]; }
            set
            {
                if (_ColorRanks[249] != value)
                {
                    _ColorRanks[249] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell ScanOnly 상태 색상")]
        public Color ColorScanOnly
        {
            get { return _ColorRanks[250]; }
            set
            {
                if (_ColorRanks[250] != value)
                {
                    _ColorRanks[250] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell Empty 상태 색상")]
        public Color ColorEmpty
        {
            get { return _ColorRanks[251]; }
            set
            {
                if (_ColorRanks[251] != value)
                {
                    _ColorRanks[251] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell 있음 상태 색상")]
        public Color ColorExist
        {
            get { return _ColorRanks[252]; }
            set
            {
                if (_ColorRanks[252] != value)
                {
                    _ColorRanks[252] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }

        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell Working 상태 색상")]
        public Color ColorWork
        {
            get { return _ColorRanks[253]; }
            set
            {
                if (_ColorRanks[253] != value)
                {
                    _ColorRanks[253] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell Finish 상태 색상")]
        public Color ColorFinish
        {
            get { return _ColorRanks[254]; }
            set
            {
                if (_ColorRanks[254] != value)
                {
                    _ColorRanks[254] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Cell Error 상태 색상")]
        public Color ColorError
        {
            get { return _ColorRanks[255]; }
            set
            {
                if (_ColorRanks[255] != value)
                {
                    _ColorRanks[255] = value;
                    ChangedColor();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Control Color")]
        [Browsable(true)]
        [Description("Ranking System 색상들")]
        public Color[] ColorRanks
        {
            get { return _ColorRanks; }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Select 가능")]
        public bool SelectEnable
        {
            get { return _SelectEnable; }
            set
            {
                if (_SelectEnable != value)
                {
                    _SelectEnable = value;
                    if (!_SelectEnable)
                        _SelectIndex = -1;
                    ReSizeRect();

                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Active 가능")]
        public bool ActiveEnable
        {
            get { return _ActiveEnable; }
            set
            {
                if (_ActiveEnable != value)
                {
                    _ActiveEnable = value;
                    if (!_ActiveEnable)
                        _ActiveIndex = -1;
                    ReSizeRect();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Visible Title Label")]
        public bool VisibleLabel
        {
            get { return _VisibleLabel; }
            set
            {
                if (_VisibleLabel != value)
                {
                    _VisibleLabel = value;
                    ReSizeRect();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Visible Cell Number")]
        public bool VisibleNumber
        {
            get { return _VisibleNumber; }
            set
            {
                if (_VisibleNumber != value)
                {
                    _VisibleNumber = value;
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Visible Trigger Number")]
        public bool VisibleTriNum
        {
            get { return _VisibleTriNum; }
            set
            {
                if (_VisibleTriNum != value)
                {
                    _VisibleTriNum = value;
                    ReStruct();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("String Property")]
        [Browsable(true)]
        [Description("Trigger Number Format")]
        public string TriNumFormat
        {
            get { return _TriNumFormat; }
            set
            {
                if (_TriNumFormat != value)
                {
                    _TriNumFormat = value;
                    DrawTriString();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Visible Area Number")]
        public bool VisibleAreaNum
        {
            get { return _VisibleAreaNum; }
            set
            {
                if (_VisibleAreaNum != value)
                {
                    _VisibleAreaNum = value;
                    ReStruct();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Strip Column Text Order")]
        public bool ColNumLeftToRight
        {
            get { return _ColNumLeftToRight; }
            set
            {
                if (_ColNumLeftToRight != value)
                {
                    _ColNumLeftToRight = value;
                    ReStruct();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Strip Row Text Order")]
        public bool RowNumTopToBottom
        {
            get { return _RowNumTopToBottom; }
            set
            {
                if (_RowNumTopToBottom != value)
                {
                    _RowNumTopToBottom = value;
                    ReStruct();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Mouse Down && Move ==> Cell Change")]
        public bool MouseMoveChangeEnable
        {
            get { return _MouseMoveChangeEnable; }
            set
            {
                if (_MouseMoveChangeEnable != value)
                {
                    _MouseMoveChangeEnable = value;
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Bool Property")]
        [Browsable(true)]
        [Description("Draw Enable : Restruct && ResizeRect && Deselect All")]
        public bool DrawEnable
        {
            get { return _DrawEnable; }
            set
            {
                _DrawEnable = value;
                if (_DrawEnable)
                {
                    ReStruct();
                    ReSizeRect();
                    Invalidate();
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Item간 그려지는 간격(픽셀)")]
        public int DistanceItem
        {
            get { return _DistanceItem; }
            set
            {
                if (_DistanceItem != value)
                {
                    _DistanceItem = value;
                    if (_DrawEnable)
                    {
                        ReSizeRect();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Segment간 그려지는 간격(픽셀)")]
        public int DistanceSegment
        {
            get { return _DistanceSegment; }
            set
            {
                if (_DistanceSegment != value)
                {
                    _DistanceSegment = value;
                    if (_DrawEnable)
                    {
                        ReSizeRect();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Board Line Size(픽셀)")]
        public int BoarderSize
        {
            get { return _BoarderSize; }
            set
            {
                if (_BoarderSize != value)
                {
                    _BoarderSize = value;
                    if (_DrawEnable)
                    {
                        ReSizeRect();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Item Line Size(픽셀)")]
        public int LineWeight
        {
            get { return _LineWeight; }
            set
            {
                if (_LineWeight != value)
                {
                    _LineWeight = value;
                    ChangedColor();
                    if (_DrawEnable)
                    {
                        ReSizeRect();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Board 와 Item 간 Offset(픽셀)")]
        public Padding DrawOffset
        {
            get { return _DrawOffset; }
            set
            {
                if (_DrawOffset != value)
                {
                    _DrawOffset = value;
                    if (_DrawEnable)
                    {
                        ReSizeRect();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("그리드 열 갯수")]
        public int RowCount
        {
            get { return _RowCount; }
            set
            {
                if (_RowCount != value)
                {
                    _RowCount = value;
                    if (_DrawEnable)
                    {
                        ReStruct();
                        ReSizeRect();
                        SetStatusAllExist();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("그리드 행 갯수")]
        public int ColCount
        {
            get { return _ColCount; }
            set
            {
                if (_ColCount != value)
                {
                    _ColCount = value;
                    if (_DrawEnable)
                    {
                        ReStruct();
                        ReSizeRect();
                        SetStatusAllExist();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("세그먼트당 열 갯수")]
        public int SegRow
        {
            get { return _SegRow; }
            set
            {
                if (_SegRow != value)
                {
                    _SegRow = value;
                    if (_DrawEnable)
                    {
                        ReStruct();
                        ReSizeRect();
                        SetStatusAllExist();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("세그먼트당 행 갯수")]
        public int SegCol
        {
            get { return _SegCol; }
            set
            {
                if (_SegCol != value)
                {
                    _SegCol = value;
                    if (_DrawEnable)
                    {
                        ReStruct();
                        ReSizeRect();
                        SetStatusAllExist();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Select Index")]
        public int SelectIndex
        {
            get { return _SelectIndex; }
            set
            {
                if (_SelectIndex != value)
                {
                    _SelectIndex = value;
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Active Index")]
        public int ActiveIndex
        {
            get { return _ActiveIndex; }
            set
            {
                if (_ActiveIndex != value)
                {
                    _ActiveIndex = value;
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Rounded Rectangle Factor (0.0~1.0)")]
        public float RoundRectFact
        {
            get { return _RoundRectFact; }
            set
            {
                if (_RoundRectFact != value)
                {
                    if (value > 1.0f) _RoundRectFact = 1.0f;
                    else if (value < 0.0f) _RoundRectFact = 0.0f;
                    else _RoundRectFact = value;
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Column Number Type(Alphabet or Number)")]
        public eStripNumType ColNumType
        {
            get { return _ColNumType; }
            set
            {
                if (_ColNumType != value)
                {
                    _ColNumType = value;
                    if (_DrawEnable)
                    {
                        ReSizeRect();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Row Number Type(Alphabet or Number)")]
        public eStripNumType RowNumType
        {
            get { return _RowNumType; }
            set
            {
                if (_RowNumType != value)
                {
                    _RowNumType = value;
                    if (_DrawEnable)
                    {
                        ReSizeRect();
                        Invalidate();
                    }
                }
            }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Strip Cell Status")]
        public eStripStatus[,] StripStatus
        {
            get { return _StripStatus; }
        }

        [Category("Geometric")]
        [Browsable(true)]
        [Description("Strip Flow Direction")]
        public eStripDirection Direction
        {
            get { return _Direction; }
            set
            {
                if (_Direction != value)
                {
                    _Direction = value;
                    ReStruct();
                    if (_DrawEnable)
                        Invalidate();
                }
            }
        }

        [Category("User Event")]
        [Browsable(true)]
        [Description("마우스 선택시 발생")]
        public event dEventStripSelected OnSelected
        {
            add
            {
                if (_OnSelected == null)
                    _OnSelected += value;
            }
            remove
            {
                if (_OnSelected == null) return;

                lock (_OnSelected)
                {
                    _OnSelected -= value;
                }
            }
        }

        [Category("User Event")]
        [Browsable(true)]
        [Description("마우스 더블 클릭시 발생")]
        public event dEventStripSelected OnSelectedDouble
        {
            add
            {
                if (_OnSelectedDouble == null)
                    _OnSelectedDouble += value;
            }
            remove
            {
                if (_OnSelectedDouble == null) return;

                lock (_OnSelectedDouble)
                {
                    _OnSelectedDouble -= value;
                }
            }
        }

        [Category("User Event")]
        [Browsable(true)]
        [Description("마우스 셀간 이동시 발생")]
        public event dEventStripSelected OnActive
        {
            add
            {
                if (_OnActive == null)
                    _OnActive += value;
            }
            remove
            {
                if (_OnActive == null) return;

                lock (_OnActive)
                {
                    _OnActive -= value;
                }
            }
        }
        #endregion

        // 마우스가 집입시 포커스 얻기
        private void Focus_MouseEnter(object sender, EventArgs e)
        {
            //((Form)(this.TopLevelControl)).Activate();
        }

        private void KStripBox_BackColorChanged(object sender, EventArgs e)
        {
            ChangedColor();
            if (_DrawEnable)
                Invalidate();
        }
    }
}
