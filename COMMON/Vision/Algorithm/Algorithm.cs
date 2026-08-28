using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using Drv.ImageProcess;
using Drv.ImageProcess.Core;
using System.Windows.Documents;
using Drv.ImageProcess.Base;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    public partial class TAlgorithm
    {
        ImageProcessing Algo = new ImageProcessing();

        public TAlgorithm()
        {
            //사용 라이브러리 init
            Algo.OpenCVInitialize();
        }
        ~TAlgorithm()
        {
            Algo.OpenCVDispose();
        }
        //------------------------------------------------------------------------
        public TVisnRslt WAlign(BUFF Source, TSET Param, EN_CAM Camid)
        {
            //JUNG/230330
            //cDEF.FM.EngrOptn.bUseRingFrame1 = false; 
            //cDEF.FM.EngrOptn.bUseRingFrame2 = false;

            //cDEF.FM.EngrOptn.bUseRingFrame1 = cDEF.FM.EngrOptn.bUseRingFrame3; //JUNG/230801/Option 하나로 사용
            //cDEF.FM.EngrOptn.bUseRingFrame2 = cDEF.FM.EngrOptn.bUseRingFrame3;

            //
            TVisnRslt Rslt = new TVisnRslt();
            Rslt.ResetData();
            
            //Image Save
            int nNo = cDEF.SEQ.WAT._nAlignCount;
            Rslt.No = nNo;

            //Param
            int iInscribedPoint;
            double dWaferSize  = vDEF.WAF_12_SIZE             * 1000 / TVisnUnit.Resoultion;
            double dNotchSize  = cDEF.FM.ProjBase.dNotchSize  * 1000 / TVisnUnit.Resoultion;
            double dEdgeLength = cDEF.FM.ProjBase.dEdgeLength * 1000 / TVisnUnit.Resoultion;
            double dEdgeAngle  = cDEF.FM.ProjBase.dEdgeAngle                               ;

            //1. Find RingFrame Angle
            float fFrameAngle = 0.0f;
            Line2D line = new Line2D();
            bool FindAngle = false;

            //Source.ExportBuffInfo(@"C:\Works\Source.bmp");
            BUFF Destination = new BUFF(BufferType.Opencv);
            Destination.AllocBuffInfo(Source);
            Bitmap DestBitmap = new Bitmap(Destination.wid, Destination.len, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //To Binary Image
            BUFF Binary = new BUFF(BufferType.Opencv);
            Binary.AllocBuffInfo(Source);
            Algo.OneLimit_Binarize(Source, Binary, Drv.ImageProcess.Core.BINARIZE_ONELIMIT_OPERATION.E_GREATER, Param.iThreshold[0]);
            //Binary.ExportBuffInfo(@"C:\Works\Binary.bmp");

            //To Edge Image
            BUFF Edge = new BUFF(BufferType.Opencv);
            Edge.AllocBuffInfo(Source);
            Algo.EdgeDetect(Binary, Edge);

            //To Moph Image
            BUFF Moph = new BUFF(BufferType.Opencv);
            Moph.AllocBuffInfo(Source);
            //Algo.Morphology(Binary, Moph, MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN, MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY, MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE);
            Algo.Kernel(Binary, Moph, MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN, MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY, MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE, Param.iKernel[0], Param.iKernel[0], Param.iIterable[0]);
            //Moph.ExportBuffInfo(@"C:\Works\Moph.bmp"); //JUNG/영상처리/매꾼상태표시

            BUFF MophEdge = new BUFF(BufferType.Opencv);
            MophEdge.AllocBuffInfo(Moph);
            Algo    .EdgeDetect   (Moph, MophEdge);
            //MophEdge.ExportBuffInfo(@"C:\Works\MophEdge.bmp");

            //Check Wafer Type
            //1. Find Contour
            //2. Expected Target Extraction
            //3. Find Wafer Type
            EN_WAFER_TYPE Type = GetWaferType(MophEdge, Param, dWaferSize, out ContourPoints Contour, out iInscribedPoint);
            Rslt.Type = Type.ToString();
            
            if (Contour == null)
            {
                // 조명 안켜짐. && Ring Frame 검사 
                cDEF.LOG.VisionTrace("Contour == null");

                //bool bUseSkipWafer = cDEF.FM.EngrOptn.bUseWaferSkip; //Wafer 미검출 시 Ring Frame으로 Align
                bool bUseSkipWafer = cDEF.FM.EngrOptn.bUseRingFrame3; //

                if (!bUseSkipWafer)
                {
                    //Error
                    Rslt.Match    = false;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0076; //Wafer 미검출 Erorr!! [Contour == null]

                    cDEF.LOG.Trace("Wafer 미검출 Erorr!! [Contour == null]");
                }
                else
                {
                    Type = EN_WAFER_TYPE.RINGFRAME;

                    Rect AngleRegion = GetRegion(Param.sROIName[4]);
                    if (!CheckRegion(AngleRegion))
                    {
                        Rslt.Match = false;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0028;
                        goto __GOTO_CHECK__;
                    }
                    DrawRectangle(DestBitmap, Brushes.LightSeaGreen, new Rectangle(AngleRegion.X, AngleRegion.Y, AngleRegion.Width, AngleRegion.Height));

                    bool FindFrame = GetFrameCenter(Source, DestBitmap, Param, out float CenterX, out float CenterY, out float Radius);
                    bool FindFrameAngle = GetFrameAngle(Source, Param, dEdgeAngle, dEdgeLength, out line);

                    if (FindFrame && FindFrameAngle)
                    {
                        Rslt.Match    = true;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;

                        fFrameAngle = (float)line.GetVectorAngle();

                        if (fFrameAngle >= 90)
                            fFrameAngle -= 90;

                        if (fFrameAngle <= -90)
                            fFrameAngle += 90;

                        Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), new Point2f(CenterX, CenterY), TVisnUnit.Distortion);
                        Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, fFrameAngle, true, out double cx, out double cy);

                        Rslt.Type = Type.ToString();
                        Rslt.XPix = (int)(pCorrectCenter.X - Rslt.CenX);
                        Rslt.YPix = (int)(pCorrectCenter.Y - Rslt.CenY);
                        Rslt.X    = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                        Rslt.Y    = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                        Rslt.T    = fFrameAngle;

                        DrawCrossLine(DestBitmap, new Pen(Brushes.LimeGreen, 10), new PointF(CenterX, CenterY), 30);
                        DrawLine(DestBitmap, new Pen(Brushes.RoyalBlue, 5), PointF.Add(line.GetLinePoint()[0], new Size(AngleRegion.X, AngleRegion.Y)), PointF.Add(line.GetLinePoint()[1], new Size(AngleRegion.X, AngleRegion.Y)));

                        cDEF.LOG.Trace("[UseSkipWafer] RING FRAME 변경");

                        goto __GOTO_CHECK__;
                    }
                    else
                    {
                        if(!FindFrame)
                        {
                            //Error
                            Rslt.Match    = false;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0078; //[RingFrame] Ring Frame Detect Fail
                            cDEF.LOG.Trace($"[RingFrame] Ring Frame Detect Fail");
                        }
                        else if(!FindFrameAngle)
                        {
                            //Error
                            Rslt.Match    = false;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0079; //[RingFrame] D-Cut Line Detect Fail
                            cDEF.LOG.Trace($"[RingFrame] D-Cut Line Detect Fail");
                        }
                    }
                }

                //
                MophEdge   .Dispose();
                Moph       .Dispose();
                Edge       .Dispose();
                Binary     .Dispose();
                Destination.Dispose();

                return Rslt;
            }
 
            if (Type == EN_WAFER_TYPE.BASE)
            {
                if (GetWaferType(Edge, Param, dWaferSize, out ContourPoints NotchContour, out iInscribedPoint) == EN_WAFER_TYPE.BASE)
                {
                    //노치 웨이퍼가 없을땐 이미지 전체로 함.
                    Rect NotchRegion = GetRegion(Param.sROIName[0]);
                    Rect AngleRegion = GetRegion(Param.sROIName[4]);

                    if (!CheckRegion(NotchRegion)) { NotchRegion = new Rect(0, 0, Source.wid, Source.len); }
                    DrawRectangle(DestBitmap, Brushes.LightSeaGreen, new Rectangle(NotchRegion.X, NotchRegion.Y, NotchRegion.Width, NotchRegion.Height));
                    if (!CheckRegion(AngleRegion)) 
                    {
                        Rslt.Match = false;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0028;
                        goto __GOTO_CHECK__;
                    }
                    DrawRectangle(DestBitmap, Brushes.LightSeaGreen, new Rectangle(AngleRegion.X, AngleRegion.Y, AngleRegion.Width, AngleRegion.Height));

                    if (FindNotch(NotchContour, dNotchSize, Param.dMinSizeTorr[1], Param.dMaxSizeTorr[1], out Point2f Notch, NotchRegion))
                    {
                        //Notch Type
                        Type = EN_WAFER_TYPE.NOTCH;
                        Drv.ImageProcess.Extension.ContourExtension.GetMinEnclosingCircle(Contour, out Point2f pCenter, out float pRadius);

                        if (cDEF.FM.EngrOptn.bUseRingFrame1)
                        {
                            bool FindFrame      = GetFrameCenter(Source, DestBitmap, Param, out float CenterX , out float CenterY, out float Radius);
                            bool FindFrameAngle = GetFrameAngle(Source, Param, dEdgeAngle, dEdgeLength, out line);

                            if (FindFrame && FindFrameAngle)
                            {
                                pCenter = new Point2f(CenterX, CenterY);

                                fFrameAngle = (float)line.GetVectorAngle();
                                FindAngle = true;

                                if (fFrameAngle >= 90)
                                    fFrameAngle -= 90;

                                if (fFrameAngle <= -90)
                                    fFrameAngle += 90;

                                Rslt.Match    = true;
                                Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;

                                DrawLine(DestBitmap, new Pen(Brushes.RoyalBlue, 5), PointF.Add(line.GetLinePoint()[0], new Size(AngleRegion.X, AngleRegion.Y)), PointF.Add(line.GetLinePoint()[1], new Size(AngleRegion.X, AngleRegion.Y)));
                            }
                            else
                            {
                                if(!FindFrame)
                                {
                                    //Error
                                    Rslt.Match    = false;
                                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0078; //[RingFrame] Ring Frame Detect Fail
                                    cDEF.LOG.Trace($"[RingFrame] Ring Frame Detect Fail");
                                }
                                else if(!FindFrameAngle)
                                {
                                    //Error
                                    Rslt.Match    = false;
                                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0079; //[RingFrame] D-Cut Line Detect Fail
                                    cDEF.LOG.Trace($"[RingFrame] D-Cut Line Detect Fail");
                                }
                                //cDEF.LOG.Trace($"[RingFrame1] FindNotch Fail");

                                //Error
                                //Rslt.Match    = false;
                                //Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0074;     
                                goto __GOTO_CHECK__;
                            }
                        }
                        else
                        {
                            Rslt.Match    = true;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                        }
                        
                        Rslt.Match    = true;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;

                        Point2f pCorrectNotch  = CorrectDistotion (new Point2f(Source.wid / 2, Source.len / 2), Notch  , TVisnUnit.Distortion);
                        Point2f pCorrectCenter = CorrectDistotion (new Point2f(Source.wid / 2, Source.len / 2), pCenter, TVisnUnit.Distortion);
                        double Angle           = CalcAngleTwoPoint(pCorrectCenter, pCorrectNotch);

                        if (FindAngle) Angle = (double)fFrameAngle;

                        Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, Angle, true, out double cx, out double cy);

                        Rslt.Type = Type.ToString();
                        Rslt.XPix = (int   )(pCorrectCenter.X - Rslt.CenX);
                        Rslt.YPix = (int   )(pCorrectCenter.Y - Rslt.CenY);
                        Rslt.X    = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                        Rslt.Y    = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                        Rslt.T    = Angle;

                        if (Rslt.Match)
                        {
                            DrawCrossLine(DestBitmap, new Pen(Brushes.LimeGreen, 10), new PointF(Notch.X  , Notch.Y  ), 30);
                            DrawCrossLine(DestBitmap, new Pen(Brushes.LimeGreen, 10), new PointF(pCenter.X, pCenter.Y), 30);
                        }
                    }
                    else
                    {
                        //No Notch(Base Type)
                        Type = EN_WAFER_TYPE.BASE;

                        Drv.ImageProcess.Extension.ContourExtension.GetMinAreaRect(Contour, out RotatedRect MinAreaRect);

                        if (cDEF.FM.EngrOptn.bUseRingFrame2) //
                        {
                            bool FindFrame = GetFrameCenter(Source, DestBitmap, Param, out float CenterX, out float CenterY, out float Radius);
                            bool FindFrameAngle = GetFrameAngle(Source, Param, dEdgeAngle, dEdgeLength, out line);

                            if (FindFrame && FindFrameAngle)
                            {
                                MinAreaRect.Center = new Point2f(CenterX, CenterY);
                                fFrameAngle = (float)line.GetVectorAngle();
                                FindAngle = true;

                                if (fFrameAngle >= 90)
                                    fFrameAngle -= 90;

                                if (fFrameAngle <= -90)
                                    fFrameAngle += 90;

                                Rslt.Match    = true;
                                Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                            }
                            else
                            {
                                if(!FindFrame)
                                {
                                    //Error
                                    Rslt.Match    = false;
                                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0078; //[RingFrame] Ring Frame Detect Fail
                                    cDEF.LOG.Trace($"[RingFrame] Ring Frame Detect Fail");
                                }
                                else if(!FindFrameAngle)
                                {
                                    //Error
                                    Rslt.Match    = false;
                                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0079; //[RingFrame] D-Cut Line Detect Fail
                                    cDEF.LOG.Trace($"[RingFrame] D-Cut Line Detect Fail");
                                }
                                //cDEF.LOG.Trace($"[RingFrame2] FindNotch Fail");

                                //Error
                                //Rslt.Match    = false;
                                //Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0074;     
                                goto __GOTO_CHECK__;
                            }
                        }
                        else
                        {
                            Rslt.Match    = true;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                        }
                        
                        Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), MinAreaRect.Center, TVisnUnit.Distortion);
                        double Angle = 0.0;
                        if (FindAngle) Angle = (double)fFrameAngle;

                        Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, Angle, true, out double cx, out double cy);

                        Rslt.Type = Type.ToString();
                        Rslt.XPix = (int)(pCorrectCenter.X - Rslt.CenX);
                        Rslt.YPix = (int)(pCorrectCenter.Y - Rslt.CenY);
                        Rslt.X    = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                        Rslt.Y    = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                        Rslt.T    = Angle;

                        //if(Rslt.Match)
                        DrawCrossLine(DestBitmap, new Pen(Brushes.LimeGreen, 10), new PointF(MinAreaRect.Center.X, MinAreaRect.Center.Y), 30);
                    }
                }
                else
                {
                    cDEF.LOG.Trace($"Wafer 윤곽 미검출 Error!![ERR_0077]");

                    //Error
                    Rslt.Match    = false;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0077; //Wafer 윤곽 미검출 Error!!
                    goto __GOTO_CHECK__;
                }
            }
            else
            {
                //Sawing Type
                Type = EN_WAFER_TYPE.SAWING;
                Drv.ImageProcess.Extension.ContourExtension.GetMinAreaRect(Contour, out RotatedRect MinAreaRect);

                if (cDEF.FM.EngrOptn.bUseRingFrame3)
                {
                    bool FindFrame = GetFrameCenter(Source, DestBitmap, Param, out float CenterX, out float CenterY, out float Radius);
                    bool FindFrameAngle = GetFrameAngle(Source, Param, dEdgeAngle, dEdgeLength, out line);

                    if (FindFrame && FindFrameAngle)
                    {
                        MinAreaRect.Center = new Point2f(CenterX, CenterY);
                        fFrameAngle = (float)line.GetVectorAngle();
                        FindAngle = true;

                        if (fFrameAngle >= 90)
                            fFrameAngle -= 90;

                        if (fFrameAngle <= -90)
                            fFrameAngle += 90;

                        Rslt.Match    = true;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                    }
                    else
                    {
                        if(!FindFrame)
                        {
                            //Error
                            Rslt.Match    = false;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0078; //[RingFrame] Ring Frame Detect Fail
                            cDEF.LOG.Trace($"[RingFrame] Ring Frame Detect Fail");
                        }
                        else if(!FindFrameAngle)
                        {
                            //Error
                            Rslt.Match    = false;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0079; //[RingFrame] D-Cut Line Detect Fail
                            cDEF.LOG.Trace($"[RingFrame] D-Cut Line Detect Fail");
                        }
                        //Error
                        //Rslt.Match    = false;
                        //Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0074;

                        //cDEF.LOG.Trace("[RingFrame3] SAWING Fail[ERR_0074]");
                    }
                }
                else
                {
                    Rslt.Match    = true;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                }

                double Angle = MinAreaRect.Angle > 45 ? Math.Abs(90 - MinAreaRect.Angle) : MinAreaRect.Angle;
                
                if (cDEF.FM.EngrOptn.bUseRingFrame3        ) Angle = 0;  //JUNG/230330
                if (FindAngle) Angle = (double)fFrameAngle;

                Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), new Point2f(MinAreaRect.Center.X, MinAreaRect.Center.Y), TVisnUnit.Distortion);
                
                Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, Angle, true, out double cx, out double cy);

                Rslt.Type = Type.ToString();
                Rslt.XPix = (int)(pCorrectCenter.X - Rslt.CenX);
                Rslt.YPix = (int)(pCorrectCenter.Y - Rslt.CenY);
                Rslt.X    = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                Rslt.Y    = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                Rslt.T    = Angle;
                
                //if (Rslt.Match)
                DrawCrossLine(DestBitmap, new Pen(Brushes.LimeGreen, 10), new PointF(MinAreaRect.Center.X, MinAreaRect.Center.Y), 30);

            }
            
        __GOTO_CHECK__:

            //Option
            bool bUseRingFrame = false;
            if      (Type == EN_WAFER_TYPE.NOTCH    ) bUseRingFrame = cDEF.FM.EngrOptn.bUseRingFrame1;
            else if (Type == EN_WAFER_TYPE.BASE     ) bUseRingFrame = cDEF.FM.EngrOptn.bUseRingFrame2;
            else if (Type == EN_WAFER_TYPE.SAWING   ) bUseRingFrame = cDEF.FM.EngrOptn.bUseRingFrame3;
            else if (Type == EN_WAFER_TYPE.RINGFRAME) bUseRingFrame = true;

            string sMode = bUseRingFrame ? "Ring Frame" : "Wafer"; 
            Rslt.Mode = sMode;

            //Check Tolerance
            if(!cDEF.SEQ.WAT.CheckTolerance(Rslt.X, Rslt.Y, Rslt.T))
            {
                Rslt.Match    = false;
                Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0062;
            }

            //
            if (Rslt.Match && Rslt.InspRslt == (int)EN_ERR_LIST.ERR_NONE)
            {
                using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                {
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50,  50), $"No : {nNo}");
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 150), $"Type : {Type}");
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 250), $"Mode : {sMode}");
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 350), $"X : {Math.Round(Rslt.X,4)} mm");
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 450), $"Y : {Math.Round(Rslt.Y,4)} mm");
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 550), $"T : {Math.Round(Rslt.T,4)} ° ");
                }
            }
            else
            {
                using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                {
                    DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50,  50), $"No : {nNo}");
                    DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 150), $"Detect Fail");
                    DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 250), $"Result:{cDEF.EPU.GetName(Rslt.InspRslt)}");
                    
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 450), $"Type : {Type}"); //JUNG/230330/추가
                    DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 550), $"Mode : {sMode}");
                }
            }

            //Result(Image Logging)
            Rslt.Item = new TCamResultItem()
            {
                Type = EN_VISN_TYPE.WAlgn,
                Time = cDEF.SEQ.WAT._StartTime,
                Src = Source.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed),
                Overlay = DestBitmap,
                No = Rslt.No,
                Result = Rslt.Match
            };
            //Result(Image Logging)
            //cDEF.VISN.Cam[(int)EN_CAM.WTB].VisionResult(new TCamResultItem()
            //{
            //    Time        = cDEF.SEQ.WAT._StartTime,
            //    Src         = Source.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed),
            //    Overlay     = DestBitmap,
            //    No          = Rslt.No,
            //    Result      = Rslt.Match
            //});

            //Release
            MophEdge   .Dispose();
            Moph       .Dispose();
            Edge       .Dispose();
            Binary     .Dispose();
            Destination.Dispose();

            return Rslt;
        }
        //---------------------------------------------------------------------------
        public TVisnRslt WAlign(BUFF Source, TSET Param, bool GetImgOk)
        {
            TVisnRslt Rslt = new TVisnRslt();
            Rslt.ResetData();

            try
            {
                if (cDEF.FM.ProjBase.iWaferType == 1 && !cDEF.FM.ProjBase.bUseCenterGap) return Rslt;
                
                //Image Save
                int nNo = cDEF.SEQ.WAT._nAlignCount;
                Rslt.No = nNo;
                
                //Param
                int    iInscribedPoint;
                double dWaferSize   = vDEF.WAF_12_SIZE * 1000 / TVisnUnit.Resoultion;
                double dNotchSize   = cDEF.FM.ProjBase.dNotchSize * 1000 / TVisnUnit.Resoultion;
                double dEdgeLength  = cDEF.FM.ProjBase.dEdgeLength * 1000 / TVisnUnit.Resoultion;
                double dEdgeAngle   = cDEF.FM.ProjBase.dEdgeAngle;
                
                
                //To Color Image
                BUFF Destination = new BUFF(BufferType.Opencv);
                Destination.AllocBuffInfo(Source);
                Bitmap DestBitmap = new Bitmap(Destination.wid, Destination.len, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                
                //To Binary Image
                BUFF Binary = new BUFF(BufferType.Opencv);
                Binary.AllocBuffInfo(Source);
                Algo.OneLimit_Binarize(Source, Binary, Drv.ImageProcess.Core.BINARIZE_ONELIMIT_OPERATION.E_GREATER, Param.iThreshold[0]);
                //Binary.ExportBuffInfo(@"C:\Works\Binary.bmp");
                
                //To Edge Image
                BUFF Edge = new BUFF(BufferType.Opencv);
                Edge.AllocBuffInfo(Source);
                Algo.EdgeDetect(Binary, Edge);
                //Edge.ExportBuffInfo(@"C:\Works\Edge.bmp");
                
                //To Moph Image
                BUFF Moph = new BUFF(BufferType.Opencv);
                Moph.AllocBuffInfo(Source);
                //Algo.Morphology(Binary, Moph, MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN, MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY, MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE);
                Algo.Kernel(Binary, Moph, MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN, MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY, MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE, Param.iKernel[0], Param.iKernel[0], Param.iIterable[0]);
                //Moph.ExportBuffInfo(@"C:\Works\Moph.bmp"); //JUNG/영상처리/매꾼상태표시
                
                BUFF MophEdge = new BUFF(BufferType.Opencv);
                MophEdge.AllocBuffInfo(Moph);
                Algo.EdgeDetect(Moph, MophEdge);
                //MophEdge.ExportBuffInfo(@"C:\Works\MophEdge.bmp");
                
                //Check Wafer Type
                //1. Find Contour
                //2. Expected Target Extraction
                //3. Find Wafer Type
                EN_WAFER_TYPE Type = GetWaferType(MophEdge, Param, dWaferSize, out ContourPoints Contour, out iInscribedPoint);
                Rslt.Type = Type.ToString();
                //
                if (Contour == null)
                {
                    // 조명 안켜짐
                    cDEF.LOG.VisionTrace("Contour == null");
                
                    Rslt.Item = new TCamResultItem()
                    {
                        Type = EN_VISN_TYPE.WAlgn,
                        Time = cDEF.SEQ.WAT._StartTime,
                        Src = Source.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed),
                        Overlay = Source.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed),
                        No = Rslt.No,
                        Result = Rslt.Match
                    };
                
                    //Error
                    Rslt.Match    = false;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0076; //Wafer 미검출 Erorr!! [Contour == null]
                
                    cDEF.LOG.Trace("Wafer 미검출 Erorr!! [Contour == null]");
                
                    //
                    MophEdge.Dispose();
                    Moph    .Dispose();
                    Edge    .Dispose();
                    Binary  .Dispose();
                    return Rslt;
                }
                
                if (Type == EN_WAFER_TYPE.BASE)
                {
                    EN_WAFER_TYPE WaferType = GetWaferType(Edge, Param, dWaferSize, out ContourPoints NotchContour, out iInscribedPoint);
                
                    bool isUnkownWafer = false;
                    //웨어퍼로 인식가능하게 해야되는지 체크 필요
                    if (cDEF.FM.ProjBase.iWaferType == 1 && cDEF.FM.ProjBase.bUseCenterGap && Type == EN_WAFER_TYPE.BASE && WaferType == EN_WAFER_TYPE.NONE)
                    {
                        isUnkownWafer = true;
                    }
                
                    if (WaferType == EN_WAFER_TYPE.BASE || isUnkownWafer)
                    {
                        //노치 웨이퍼가 없을땐 이미지 전체로 함.
                        Rect NotchRegion = GetRegion(Param.sROIName[0]);
                        Rect AngleRegion = GetRegion(Param.sROIName[4]);
                
                        if (!CheckRegion(NotchRegion)) { NotchRegion = new Rect(0, 0, Source.wid, Source.len); }
                        DrawRectangle(DestBitmap, Brushes.LightSeaGreen, new Rectangle(NotchRegion.X, NotchRegion.Y, NotchRegion.Width, NotchRegion.Height));
                        if (!CheckRegion(AngleRegion))
                        {
                            Rslt.Match = false;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0028;
                            goto __GOTO_CHECK__;
                        }
                        DrawRectangle(DestBitmap, Brushes.LightSeaGreen, new Rectangle(AngleRegion.X, AngleRegion.Y, AngleRegion.Width, AngleRegion.Height));
                
                        if (FindNotch(isUnkownWafer ? Contour : NotchContour, dNotchSize, Param.dMinSizeTorr[1], Param.dMaxSizeTorr[1], out Point2f Notch, NotchRegion))
                        {
                            //Notch Type
                            Type = EN_WAFER_TYPE.NOTCH;
                            Drv.ImageProcess.Extension.ContourExtension.GetMinEnclosingCircle(Contour, out Point2f pCenter, out float pRadius);
                
                            Rslt.Match = true;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                
                            Point2f pCorrectNotch = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), Notch, TVisnUnit.Distortion);
                            Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), pCenter, TVisnUnit.Distortion);
                            double Angle = CalcAngleTwoPoint(pCorrectCenter, pCorrectNotch);
                            Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, Angle, true, out double cx, out double cy);
                
                            Rslt.Type = Type.ToString();
                            Rslt.XPix = (int)(pCorrectCenter.X - Rslt.CenX);
                            Rslt.YPix = (int)(pCorrectCenter.Y - Rslt.CenY);
                            Rslt.OriX = pCorrectCenter.X;
                            Rslt.OriY = pCorrectCenter.Y;
                            Rslt.OriR = (double)pRadius;
                            Rslt.DcutStrtX = 0;
                            Rslt.DcutStrtY = 0;
                            Rslt.DcutEndX  = 0;
                            Rslt.DcutEndY  = 0;
                            Rslt.AngleRegionX = 0;
                            Rslt.AngleRegionY = 0;
                            Rslt.X = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                            Rslt.Y = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                            Rslt.T = Angle;
                
                            if (Rslt.Match)
                            {
                                DrawCrossLine(DestBitmap, new Pen(Brushes.LimeGreen , 10), new PointF(Notch.X  , Notch.Y  ), 30);
                                DrawCrossLine(DestBitmap, new Pen(Brushes.BlueViolet, 10), new PointF(pCenter.X, pCenter.Y), 30);
                            }
                        }
                        else
                        {
                            //No Notch(Base Type)
                            Type = EN_WAFER_TYPE.BASE;
                
                            //Drv.ImageProcess.Extension.ContourExtension.GetMinEnclosingCircle(Contour, out Point2f pCenter, out float pRadius);
                            Drv.ImageProcess.Extension.ContourExtension.GetMinAreaRect(Contour, out RotatedRect MinAreaRect);
                
                            Rslt.Match = true;
                            Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                
                            //Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), pCenter, TVisnUnit.Distortion);
                            Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), MinAreaRect.Center, TVisnUnit.Distortion);
                            
                            double Angle = 0.0;
                            Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, Angle, true, out double cx, out double cy);
                
                            Rslt.Type = Type.ToString();
                            Rslt.XPix = (int)(pCorrectCenter.X - Rslt.CenX);
                            Rslt.YPix = (int)(pCorrectCenter.Y - Rslt.CenY);
                            Rslt.OriX = pCorrectCenter.X;
                            Rslt.OriY = pCorrectCenter.Y;
                            Rslt.OriR = MinAreaRect.Size.Width/2; // pRadius;
                            Rslt.DcutStrtX = 0;
                            Rslt.DcutStrtY = 0;
                            Rslt.DcutEndX  = 0;
                            Rslt.DcutEndY  = 0;
                            Rslt.AngleRegionX = 0;
                            Rslt.AngleRegionY = 0;
                            Rslt.X = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                            Rslt.Y = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                            Rslt.T = Angle;
                
                            if (Rslt.Match)
                            {
                                //DrawCrossLine(DestBitmap, new Pen(Brushes.BlueViolet, 10), new PointF(pCenter.X, pCenter.Y), 30);
                                DrawCrossLine(DestBitmap, new Pen(Brushes.LimeGreen, 10), new PointF(MinAreaRect.Center.X, MinAreaRect.Center.Y), 30);
                            }
                        }
                    }
                    else
                    {
                        cDEF.LOG.Trace($"Wafer 윤곽 미검출 Error!![ERR_0077]");
                
                        //Error
                        Rslt.Match    = false;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0077; //Wafer 윤곽 미검출 Error!!
                
                        goto __GOTO_CHECK__;
                    }
                }
                else
                {
                    //Sawing Type
                    Type = EN_WAFER_TYPE.SAWING;
                    Drv.ImageProcess.Extension.ContourExtension.GetMinAreaRect(Contour, out RotatedRect MinAreaRect);
                    Rslt.Match = true;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;
                
                    double Angle = MinAreaRect.Angle > 45 ? Math.Abs(90 - MinAreaRect.Angle) : MinAreaRect.Angle;
                    Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), new Point2f(MinAreaRect.Center.X, MinAreaRect.Center.Y), TVisnUnit.Distortion);
                    Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, Angle, true, out double cx, out double cy);
                
                    Rslt.Type = Type.ToString();
                    Rslt.XPix = (int)(pCorrectCenter.X - Rslt.CenX);
                    Rslt.YPix = (int)(pCorrectCenter.Y - Rslt.CenY);
                    Rslt.OriX = pCorrectCenter.X;
                    Rslt.OriY = pCorrectCenter.Y;
                    Rslt.OriR = 0;
                    Rslt.DcutStrtX = pCorrectCenter.X - (MinAreaRect.Size.Width  / 2);
                    Rslt.DcutStrtY = pCorrectCenter.Y - (MinAreaRect.Size.Height / 2);
                    Rslt.DcutEndX  = MinAreaRect.Size.Width;
                    Rslt.DcutEndY  = MinAreaRect.Size.Height;
                    Rslt.AngleRegionX = 0;
                    Rslt.AngleRegionY = 0;
                    Rslt.X    = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                    Rslt.Y    = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                    Rslt.T    = Angle;
                
                
                    DrawCrossLine(DestBitmap, new Pen(Brushes.BlueViolet, 10), new PointF(MinAreaRect.Center.X, MinAreaRect.Center.Y), 30);
                }

                __GOTO_CHECK__:

                //Option
                Rslt.Mode = "Wafer";
                
                //
                if (Rslt.Match && Rslt.InspRslt == (int)EN_ERR_LIST.ERR_NONE)
                {
                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                    {
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 50), $"No : {nNo}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 150), $"Type : {Type}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 250), $"Mode : {Rslt.Mode}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 350), $"D-Cut Align : {cDEF.FM.EngrOptn.bUseDcutAlgnT}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 450), $"X : {Math.Round(Rslt.X, 4)} mm");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 550), $"Y : {Math.Round(Rslt.Y, 4)} mm");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 650), $"T : {Math.Round(Rslt.T, 4)} ° ");
                    }
                }
                else
                {
                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                    {
                        DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 50), $"No : {nNo}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 150), $"D-Cut Align : {cDEF.FM.EngrOptn.bUseDcutAlgnT}");
                        DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 250), $"Detect Fail");
                        DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 350), $"Result:{cDEF.EPU.GetName(Rslt.InspRslt)}");
                
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 550), $"Type : {Type}"); //JUNG/230330/추가
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 650), $"Mode : {Rslt.Mode}");
                    }
                }
                
                //Result(Image Logging)
                Rslt.Item = new TCamResultItem()
                {
                    Type = EN_VISN_TYPE.WAlgn,
                    Time = cDEF.SEQ.WAT._StartTime,
                    Src = Source.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed),
                    Overlay = DestBitmap,
                    No = Rslt.No,
                    Result = Rslt.Match
                };
                
                //Check Tolerance
                if (!cDEF.SEQ.WAT.CheckTolerance(Rslt.X, Rslt.Y, Rslt.T))
                {
                    Rslt.Match = false;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0062;
                }
                
                //Release
                MophEdge.Dispose();
                Moph    .Dispose();
                Edge    .Dispose();
                Binary  .Dispose();
                
                return Rslt;
            }
            catch (Exception err)
            {
                string sLog = $"[EXCEPTION ERROR] WAlign :{Rslt.Type}";
                cDEF.LOG.VisionTrace(sLog);
                cDEF.LOG.ExceptionTrace("[Exception] WAlign", err);
                Rslt.ResetData();
                return Rslt;
            }
        }
        //---------------------------------------------------------------------------
        public TVisnRslt FAlign(BUFF Source, TSET Param, bool GetImgOk)
        {
            //

            TVisnRslt Rslt = new TVisnRslt();
            Rslt.ResetData();
            try
            {
                //Image Save
                int nNo = cDEF.SEQ.WAT._nAlignCount;
                Rslt.No = nNo;

                //Param
                double dWaferSize  = vDEF.WAF_12_SIZE * 1000 / TVisnUnit.Resoultion;
                double dNotchSize  = cDEF.FM.ProjBase.dNotchSize * 1000 / TVisnUnit.Resoultion;
                double dEdgeLength = cDEF.FM.ProjBase.dEdgeLength * 1000 / TVisnUnit.Resoultion;
                double dEdgeAngle  = cDEF.FM.ProjBase.dEdgeAngle;

                //1. Find RingFrame Angle
                float fFrameAngle = 0.0f;
                Line2D line = new Line2D();

                //Source.ExportBuffInfo(@"C:\Works\Source.bmp");
                BUFF Destination = new BUFF(BufferType.Opencv);
                Destination.AllocBuffInfo(Source);
                Bitmap DestBitmap = new Bitmap(Destination.wid, Destination.len, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                //To Binary Image
                BUFF Binary = new BUFF(BufferType.Opencv);
                Binary.AllocBuffInfo(Source);
                Algo.OneLimit_Binarize(Source, Binary, Drv.ImageProcess.Core.BINARIZE_ONELIMIT_OPERATION.E_GREATER, Param.iThreshold[0]);
                //Binary.ExportBuffInfo(@"C:\Works\Binary.bmp");

                //To Edge Image
                BUFF Edge = new BUFF(BufferType.Opencv);
                Edge.AllocBuffInfo(Source);
                Algo.EdgeDetect(Binary, Edge);

                //To Moph Image
                BUFF Moph = new BUFF(BufferType.Opencv);
                Moph.AllocBuffInfo(Source);
                //Algo.Morphology(Binary, Moph, MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN, MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY, MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE);
                Algo.Kernel(Binary, Moph, MORPHOLOGY_OPERATION.E_MORPHOLOGY_OPERATION_OPEN, MORPHOLOGY_CONDITION.E_MORPHOLOGY_BINARY, MORPHOLOGY_SHAPE.E_MORPHOLOGY_RECTANGLE, Param.iKernel[0], Param.iKernel[0], Param.iIterable[0]);
                //Moph.ExportBuffInfo(@"C:\Works\Moph.bmp"); //JUNG/영상처리/매꾼상태표시

                BUFF MophEdge = new BUFF(BufferType.Opencv);
                MophEdge.AllocBuffInfo(Moph);
                Algo.EdgeDetect(Moph, MophEdge);
                //MophEdge.ExportBuffInfo(@"C:\Works\MophEdge.bmp");

                //Check Wafer Type
                //1. Find Contour
                //2. Expected Target Extraction
                //3. Find Wafer Type
                EN_WAFER_TYPE Type = EN_WAFER_TYPE.RINGFRAME;
                Rslt.Type = Type.ToString();
                //
                Rect AngleRegion = GetRegion(Param.sROIName[4]);
                if (!CheckRegion(AngleRegion))
                {
                    Rslt.Match = false;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0028;
                    goto __GOTO_CHECK__;
                }
                DrawRectangle(DestBitmap, Brushes.LightSeaGreen, new Rectangle(AngleRegion.X, AngleRegion.Y, AngleRegion.Width, AngleRegion.Height));

                bool FindFrame      = GetFrameCenter(Source, DestBitmap, Param, out float CenterX, out float CenterY, out float Radius);
                bool FindFrameAngle = GetFrameAngle (Source, Param, dEdgeAngle, dEdgeLength, out line);

                if (FindFrame && FindFrameAngle)
                {
                    Rslt.Match    = true;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;

                    fFrameAngle   = (float)line.GetVectorAngle();

                    if (fFrameAngle >=  90) fFrameAngle -= 90;
                    if (fFrameAngle <= -90) fFrameAngle += 90;

                    Point2f pCorrectCenter = CorrectDistotion(new Point2f(Source.wid / 2, Source.len / 2), new Point2f(CenterX, CenterY), TVisnUnit.Distortion);
                    Calculate(pCorrectCenter.X - Rslt.CenX, pCorrectCenter.Y - Rslt.CenY, fFrameAngle, true, out double cx, out double cy);

                    //Rslt.Match    = true;
                    //Rslt.InspRslt = (int)EN_ERR_LIST.ERR_NONE;

                    Rslt.Type         = Type.ToString();
                    Rslt.XPix         = (int)(pCorrectCenter.X - Rslt.CenX);
                    Rslt.YPix         = (int)(pCorrectCenter.Y - Rslt.CenY);
                    Rslt.OriX         = pCorrectCenter.X;
                    Rslt.OriY         = pCorrectCenter.Y;
                    Rslt.OriR         = Radius;
                    Rslt.DcutStrtX    = line.GetLinePoint()[0].X;
                    Rslt.DcutStrtY    = line.GetLinePoint()[0].Y;
                    Rslt.DcutEndX     = line.GetLinePoint()[1].X;
                    Rslt.DcutEndY     = line.GetLinePoint()[1].Y;
                    Rslt.AngleRegionX = AngleRegion.X;
                    Rslt.AngleRegionY = AngleRegion.Y;
                    Rslt.X            = (double)cx * TVisnUnit.Resoultion / 1000 * 1;
                    Rslt.Y            = (double)cy * TVisnUnit.Resoultion / 1000 * 1;
                    Rslt.T            = fFrameAngle;
                    //
                    DrawCrossLine(DestBitmap, new Pen(Brushes.Blue  , 10), new PointF(CenterX, CenterY), 30);
                    DrawLine     (DestBitmap, new Pen(Brushes.Maroon, 5), PointF.Add(line.GetLinePoint()[0], new Size(AngleRegion.X, AngleRegion.Y)), PointF.Add(line.GetLinePoint()[1], new Size(AngleRegion.X, AngleRegion.Y)));

                }
                else
                {
                    if(!FindFrame)
                    {
                        //Error
                        Rslt.Match    = false;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0078; //[RingFrame] Ring Frame Detect Fail
                        cDEF.LOG.Trace($"[RingFrame] Ring Frame Detect Fail");
                    }
                    else if(!FindFrameAngle)
                    {
                        //Error
                        Rslt.Match    = false;
                        Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0079; //[RingFrame] D-Cut Line Detect Fail
                        cDEF.LOG.Trace($"[RingFrame] D-Cut Line Detect Fail");
                    }
                }
                __GOTO_CHECK__:

                //Option
                Rslt.Mode = "Ring Frame";

                //
                if (Rslt.Match && Rslt.InspRslt == (int)EN_ERR_LIST.ERR_NONE)
                {
                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                    {
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 50), $"No : {nNo}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 150), $"Type : {Type}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 250), $"Mode : {Rslt.Mode}");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 350), $"X : {Math.Round(Rslt.X, 4)} mm");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 450), $"Y : {Math.Round(Rslt.Y, 4)} mm");
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 550), $"T : {Math.Round(Rslt.T, 4)} ° ");
                    }
                }
                else
                {
                    using (Font font = new Font("Tahoma", 50, FontStyle.Bold))
                    {
                        DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 50), $"No : {nNo}");
                        DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 150), $"Detect Fail");
                        DrawString(DestBitmap, font, Brushes.IndianRed, new PointF(50, 250), $"Result:{cDEF.EPU.GetName(Rslt.InspRslt)}");

                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 450), $"Type : {Type}"); //JUNG/230330/추가
                        DrawString(DestBitmap, font, Brushes.LimeGreen, new PointF(50, 550), $"Mode : {Rslt.Mode}");
                    }
                }

                //Result(Image Logging)
                Rslt.Item = new TCamResultItem()
                {
                    Type = EN_VISN_TYPE.WAlgn,
                    Time = cDEF.SEQ.WAT._StartTime,
                    Src = Source.ToBitmap(System.Drawing.Imaging.PixelFormat.Format8bppIndexed),
                    Overlay = BitmapHelper.Clone(DestBitmap, System.Drawing.Imaging.PixelFormat.Format32bppArgb), //DestBitmap, //
                    No = Rslt.No,
                    Result = Rslt.Match
                };
                
                //Check Tolerance
                if (!cDEF.SEQ.WAT.CheckTolerance(Rslt.X, Rslt.Y, Rslt.T))
                {
                    Rslt.Match = false;
                    Rslt.InspRslt = (int)EN_ERR_LIST.ERR_0062;
                }

                //Release
                MophEdge.Dispose();
                Moph.Dispose();
                Edge.Dispose();
                Binary.Dispose();
                Destination.Dispose();

                return Rslt;
            }
            catch (Exception err) 
            {
                string sLog = $"[EXCEPTION ERROR] FAlign :{Rslt.Type}";
                cDEF.LOG.VisionTrace(sLog);
                cDEF.LOG.ExceptionTrace("[Exception] FAlign", err);
                Rslt.ResetData();
                return Rslt;
            }
        }
        //---------------------------------------------------------------------------
        private EN_WAFER_TYPE GetWaferType(BUFF Source, TSET Param, double dWaferSize, out ContourPoints pContour, out int InscribedPoint)
        {
            EN_WAFER_TYPE Type = EN_WAFER_TYPE.NONE;

            int iInscribedPoint = 0;
            int iMinSize = (int)(dWaferSize * (1 - Param.dMinSizeTorr[0] / 100));
            int iMaxSize = (int)(dWaferSize * (1 + Param.dMaxSizeTorr[0] / 100));
            //int iInscribedCount = 2000; // 3000;
            int iInscribedCount = Param.iInscribedPoint; // 3000;
            if (Param.iInscribedPoint <= 0)
            {
                iInscribedCount = 3000;
                Param.iInscribedPoint = 3000;
            }


            Algo.ContourTrace(Source, out ContourPoints[] Contour, RETRIVAL_MODE.E_CCOMP, APPROXIMATION_MODE.E_APPROXNONE);

            List<ContourPoints> WaferEdges = new List<ContourPoints>();

            for (int i = 0; i < Contour.Length; i++)
            {
                if (Contour[i].Points.Count < 300) continue;

                double dArcLength = Drv.ImageProcess.Extension.ContourExtension.GetArcLength(Contour[i], true);
                var PointsDP = Drv.ImageProcess.Extension.ContourExtension.GetApproxPolyDP(Contour[i], dArcLength * 0.02, true);

                Drv.ImageProcess.Extension.ContourExtension.GetBoundingRect(Contour[i], out Rect BoundingRect);
                
                //using (Bitmap BoundingRectBmp = new Bitmap(Source.wid, Source.len, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                //{
                //    DrawRectangle(BoundingRectBmp, Brushes.LightSeaGreen, new Rectangle(BoundingRect.X, BoundingRect.Y, BoundingRect.Width, BoundingRect.Height));
                //    BoundingRectBmp.Save(@"C:\Works\BoundingRectBmp.bmp");
                //}
                //remove by size
                if (BoundingRect.Width  < iMinSize) continue;
                if (BoundingRect.Height < iMinSize) continue;
                if (BoundingRect.Width  > iMaxSize) continue;
                if (BoundingRect.Height > iMaxSize) continue;


                //Remove duplicates and children
                if (Contour[i].Hierarchy.Child != -1) continue;
                if (Contour[i].Hierarchy.Next  != -1) continue;

                WaferEdges.Add(Contour[i]);
            }

            if (WaferEdges.Count == 1)
            {
                pContour = WaferEdges[0];

                Drv.ImageProcess.Extension.ContourExtension.GetMinEnclosingCircle(pContour, out Point2f pCenter, out float pRadius);

                //Drawind Countor
                //using (Bitmap ContourBmp = new Bitmap(Source.wid, Source.len, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                //{
                //    DrawPoints(ContourBmp, new Pen(Brushes.LimeGreen, 5), pContour.Points.Select(n=>new PointF(n.X,n.Y)).ToArray());
                //    ContourBmp.Save(@"C:\Works\ContourBmp.bmp");
                //}
                iInscribedPoint = GetInscribedPoint(pContour, new Drv.ImageProcess.Base.CircleF(new PointF(pCenter.X, pCenter.Y), pRadius), Param.dMinSizeTorr[2], Param.dMaxSizeTorr[2]);
                InscribedPoint = iInscribedPoint;
                if (iInscribedPoint > iInscribedCount)
                {
                    Type = EN_WAFER_TYPE.BASE;
                }
                else
                {
                    Type = EN_WAFER_TYPE.SAWING;
                }
            }
            else
            {
                InscribedPoint = 0;
                pContour = null;

                Type = EN_WAFER_TYPE.NONE;
            }

            return Type;
        }
        //---------------------------------------------------------------------------
        public async Task<(EN_WAFER_TYPE, ContourPoints)> AsyncGetWaferType(BUFF Source, TSET Param, double dWaferSize)
        {
            EN_WAFER_TYPE Type = EN_WAFER_TYPE.NONE;
            ContourPoints Contour = null;
            int iMinSize = (int)(dWaferSize * (1 - Param.dMinSizeTorr[0] / 100));
            int iMaxSize = (int)(dWaferSize * (1 + Param.dMaxSizeTorr[0] / 100));
            int iInscribedCount = Param.iInscribedPoint; // 3000;
            if (Param.iInscribedPoint <= 0)
            {
                iInscribedCount = 3000;
                Param.iInscribedPoint = 3000;
            }

            await Task.Run(() =>
            {
                Algo.ContourTrace(Source, out ContourPoints[] Contours, RETRIVAL_MODE.E_CCOMP, APPROXIMATION_MODE.E_APPROXNONE);

                List<ContourPoints> WaferEdges = new List<ContourPoints>();

                for (int i = 0; i < Contours.Length; i++)
                {
                    if (Contours[i].Points.Count < 300) continue;

                    double dArcLength = Drv.ImageProcess.Extension.ContourExtension.GetArcLength(Contours[i], true);
                    var PointsDP = Drv.ImageProcess.Extension.ContourExtension.GetApproxPolyDP(Contours[i], dArcLength * 0.02, true);

                    Drv.ImageProcess.Extension.ContourExtension.GetBoundingRect(Contours[i], out Rect BoundingRect);

                    //remove by size
                    if (BoundingRect.Width < iMinSize) continue;
                    if (BoundingRect.Height < iMinSize) continue;
                    if (BoundingRect.Width > iMaxSize) continue;
                    if (BoundingRect.Height > iMaxSize) continue;


                    //Remove duplicates and children
                    if (Contours[i].Hierarchy.Child != -1) continue;
                    if (Contours[i].Hierarchy.Next != -1) continue;

                    WaferEdges.Add(Contours[i]);
                }

                if (WaferEdges.Count == 1)
                {
                    Contour = WaferEdges[0];

                    Drv.ImageProcess.Extension.ContourExtension.GetMinEnclosingCircle(Contour, out Point2f pCenter, out float pRadius);

                    //Drawind Countor
                    //using (Bitmap ContourBmp = new Bitmap(Source.wid, Source.len, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    //{
                    //    DrawPoints(ContourBmp, new Pen(Brushes.LimeGreen, 5), pContour.Points.Select(n=>new PointF(n.X,n.Y)).ToArray());
                    //    ContourBmp.Save(@"C:\Works\ContourBmp.bmp");
                    //}

                    if (GetInscribedPoint(Contour, new Drv.ImageProcess.Base.CircleF(new PointF(pCenter.X, pCenter.Y), pRadius), Param.dMinSizeTorr[2], Param.dMaxSizeTorr[2]) > iInscribedCount)
                    {
                        Type = EN_WAFER_TYPE.BASE;
                    }
                    else
                    {
                        Type = EN_WAFER_TYPE.SAWING;
                    }
                }
                else
                {
                    Contour = null;

                    Type = EN_WAFER_TYPE.NONE;
                }

            });

            return (Type, Contour);
        }
        //---------------------------------------------------------------------------
        private bool GetFrameCenter(BUFF Source, Bitmap DestBitmap, TSET Param, out float CenterX, out float CenterY, out float Radius)
        {
            Rect EdgeRegion;
            Point2f center = new Point2f(Source.wid / 2, Source.len / 2);

            ContourPoints FrameContour = new ContourPoints();

            for (int i = 1; i < 4; i++)
            {
                EdgeRegion = GetRegion(Param.sROIName[i]);
                if (!CheckRegion(EdgeRegion)) break;
                //
                DrawRectangle(DestBitmap, Brushes.LightSeaGreen, new Rectangle(EdgeRegion.X, EdgeRegion.Y, EdgeRegion.Width, EdgeRegion.Height));


                BUFF RingPoint = new BUFF(BufferType.Opencv);
                RingPoint.AllocBuffInfo(EdgeRegion.Width, EdgeRegion.Height);
                Algo.ImageCrop(Source, RingPoint, EdgeRegion);
                //RingPoint.ExportBuffInfo(@"C:\Works\RingPoint.bmp");


                BUFF RingBinary = new BUFF(BufferType.Opencv);
                RingBinary.AllocBuffInfo(RingPoint);
                Algo.OneLimit_Binarize(RingPoint, RingBinary, Drv.ImageProcess.Core.BINARIZE_ONELIMIT_OPERATION.E_GREATER, Param.iThreshold[1]);

                BUFF RingEdge = new BUFF(BufferType.Opencv);
                RingEdge.AllocBuffInfo(RingBinary);
                Algo.EdgeDetect(RingBinary, RingEdge);
                //RingEdge.ExportBuffInfo(@"C:\Works\RingEdge.bmp");

                FindFrameEdge(RingEdge, center, EdgeRegion, out ContourPoints pContour);

                if (pContour != null)
                {
                    FrameContour.Points.AddRange(pContour.Points);
                    FrameContour.num += pContour.Points.Count;
                }

                RingEdge  .Dispose();
                RingBinary.Dispose();
                RingPoint .Dispose();
            }

            if (FrameContour.num > 0)
            {
                FitFrameCenter(FrameContour, out float cX, out float cY, out float R);

                CenterX = cX;
                CenterY = cY;
                Radius = R;
                return true;
            }
            else
            {
                CenterX = 0;
                CenterY = 0;
                Radius = 0;

                return false;
            }

        }
        //---------------------------------------------------------------------------
        private int GetInscribedPoint(ContourPoints pContour, Drv.ImageProcess.Base.CircleF pCircle, double dMinSizeTorr, double dMaxSizeTorr)
        {
            int iInscribedPoint = 0;

            double dDistance = 0.0;

            double dMinSize = pCircle.Radius * (1 - dMinSizeTorr / 100);
            double dMaxSize = pCircle.Radius * (1 + dMaxSizeTorr / 100);

            foreach (Point2f point in pContour.Points)
            {
                dDistance = CalcDistanceTwoPoint(new Point2f(pCircle.Center.X, pCircle.Center.Y), point);

                if (dMinSize <= dDistance && dDistance <= dMaxSize)
                {
                    iInscribedPoint++;
                }
            }

            return iInscribedPoint;
        }
        //---------------------------------------------------------------------------
        private double CalcDistanceTwoPoint(Point2f src1, Point2f src2)
        {
            return Math.Sqrt((Math.Pow(src1.X - src2.X, 2) + Math.Pow(src1.Y - src2.Y, 2)));
        }
        //---------------------------------------------------------------------------
        private double CalcAngleTwoPoint(Point2f src1, Point2f src2)
        {
            if (src2.X < 0 && src2.X < 0) return 0.0;

            return Math.Atan2(src2.Y - src1.Y, src2.X - src1.X) * 180.0f / Math.PI;
        }
        //---------------------------------------------------------------------------
        private double DegToRad(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        //---------------------------------------------------------------------------
        public void Calculate(double X, double Y, double T, bool Rev, out double cX, out double cY)
        {
            //Local Var.
            double dRad;
            double dCos;
            double dSin;
            double[] CalcPosn = new double[2];

            //Set Position Data.
            dRad = DegToRad(T);
            dCos = Math.Cos(dRad);
            dSin = Math.Sin(dRad);

            //Cal.
            if (Rev)
            {
                cX = (X *  dCos) + (Y * dSin);
                cY = (X * -dSin) + (Y * dCos);
            }
            else
            {
                cX = (X * dCos) + (Y * -dSin);
                cY = (X * dSin) + (Y *  dCos);
            }
        }
        //---------------------------------------------------------------------------
        private bool FindNotch(ContourPoints pContour, double dNotchSize, double dMinSizeTorr, double dMaxSizeTorr, out Point2f pNotch, Rect pNotchArea)
        {
            pNotch = new Point2f(0, 0);

            Drv.ImageProcess.Extension.ContourExtension.GetMinEnclosingCircle(pContour, out Point2f pCenter, out float pRadius);

            double dMinSize     = (double)(dNotchSize * (1 - dMinSizeTorr / 100));
            double dMaxSize     = (double)(dNotchSize * (1 + dMaxSizeTorr / 100));

            double dDistance    = 0.0;

            double dMaxDistance = Math.Abs(pRadius - dMinSize);
            double dMinDistance = Math.Abs(pRadius - dMaxSize);

            double dDistanceMin = 99999;

            foreach (Point2f point in pContour.Points)
            {
                dDistance = CalcDistanceTwoPoint(pCenter, point);

                //원의 반지름과 원과 점사이의 거리의 차이

                if (pNotchArea.Contains((int)point.X, (int)point.Y))
                {
                    if (pRadius < dDistance) continue;

                    if (dMinDistance <= dDistance && dDistance <= dMaxDistance)
                    {
                        if (dDistance < dDistanceMin)
                        {
                            dDistanceMin = dDistance;
                            pNotch = point;
                        }
                    }
                }

            }
            if (pNotch.X == 0 && pNotch.Y == 0) return false;
            else return true;
        }
        //---------------------------------------------------------------------------
        private void FindFrameEdge(BUFF Source, Point2f pCenter, Rect pArea, out ContourPoints pContour)
        {
            Algo.ContourTrace(Source, out ContourPoints[] EdgeContour, RETRIVAL_MODE.E_CCOMP, APPROXIMATION_MODE.E_APPROXNONE);

            int iMax = -1;

            double dMaxDistance = -999999;
            int iArc = Source.wid > Source.len ? Source.wid : Source.len;

            for (int i = 0; i < EdgeContour.Length; i++)
            {
                double dArcLength = Drv.ImageProcess.Extension.ContourExtension.GetArcLength(EdgeContour[i], true);
                if (dArcLength < iArc) continue;

                Point2f ptAver = new Point2f(EdgeContour[i].Points.Average(x => x.X) + pArea.X, EdgeContour[i].Points.Average(x => x.Y) + pArea.Y);

                double dDistance = CalcDistanceTwoPoint(ptAver, pCenter);
                if (dMaxDistance < dDistance)
                {
                    dMaxDistance = dDistance;
                    iMax = i;
                }
            }

            if (0 <= iMax)
            {
                pContour = new ContourPoints();
                pContour.Points = EdgeContour[iMax].Points.Select(edge => new Point2f(edge.X + pArea.X, edge.Y + pArea.Y)).ToList();
            }
            else
                pContour = null;
        }
        //----------------------------------------------------------------------------
        private bool GetFrameAngle(BUFF Source, TSET Param, double stdAngle, double Length,  out Line2D line)
        {
            bool bRet = false;

            Drv.ImageProcess.Rect EdgeRegion = GetRegion(Param.sROIName[4]);

            BUFF RingAnglePoint = new BUFF(BufferType.Opencv);
            RingAnglePoint.AllocBuffInfo(EdgeRegion.Width, EdgeRegion.Height);
            Algo.ImageCrop(Source, RingAnglePoint, EdgeRegion);
            //RingAnglePoint.ExportBuffInfo(@"C:\Works\RingAnglePoint.bmp");


            BUFF RingAngleblur = new BUFF(BufferType.Opencv);
            RingAngleblur.AllocBuffInfo(RingAnglePoint);
            Algo.Convolve(RingAnglePoint, RingAngleblur, EDGE_OPERATION.E_SHARPEN);


            BUFF RingAngleBinary = new BUFF(BufferType.Opencv);
            RingAngleBinary.AllocBuffInfo(RingAngleblur);
            Algo.OneLimit_Binarize(RingAngleblur, RingAngleBinary, Drv.ImageProcess.Core.BINARIZE_ONELIMIT_OPERATION.E_GREATER, Param.iThreshold[2]);
            //RingAngleBinary.ExportBuffInfo(@"C:\Works\RingAngleBinary.bmp");

            BUFF RingAngleEdge = new BUFF(BufferType.Opencv);
            RingAngleEdge.AllocBuffInfo(RingAngleBinary);
            Algo.EdgeDetect(RingAngleBinary, RingAngleEdge, EDGE_TYPE.E_BLACK_TO_WHITE);
            //RingAngleEdge.ExportBuffInfo(@"C:\Works\RingAngleEdge.bmp");

            //Line Fitting
            BUFF RingLineEdge = new BUFF(BufferType.Opencv);
            RingLineEdge.AllocBuffInfo(RingAngleEdge);
            //RingLineEdge.ExportBuffInfo(@"C:\Works\RingLineEdge.bmp");

            stFittingLineParam stFindLine            = new stFittingLineParam();
            stFindLine.FitLengthStd                  = Length;
            stFindLine.FitLength_Up_OffSet_Percent   = (int)Param.dMinSizeTorr[3];
            stFindLine.FitLength_Down_OffSet_Percent = (int)Param.dMaxSizeTorr[3];

            stFindLine.FitAngleStd                   = stdAngle; //Max Angle
            stFindLine.FitAngle_Tolerance            = (int)Param.dMaxSizeTorr[4]; 

            Line2D RingLine = new Line2D();
            bRet = Algo.FindLine(RingAngleEdge, RingLineEdge, stFindLine, FIND_ORIENTATION.E_VERTICAL, ref RingLine);

            RingLineEdge   .Dispose();
            RingAngleEdge  .Dispose();
            RingAngleBinary.Dispose();
            RingAngleblur  .Dispose();
            RingAnglePoint .Dispose();

            line = RingLine;
            return bRet;
        }
        //---------------------------------------------------------------------------
        public bool FitFrameCenter(ContourPoints pointFs, out float CenterX, out float CenterY, out float CenterR)
        {
            try
            {
                //int iRet = 0;
                Matrix<float> YMat;
                Matrix<float> RMat;
                Matrix<float> AMat;
                List<float> YLit = new List<float>();
                List<float[]> RLit = new List<float[]>();
                //------Build Y matrix
                foreach (var pointF in pointFs.Points)
                    YLit.Add((float)(pointF.X * pointF.X + pointF.Y * pointF.Y));

                float[,] Yarray = new float[YLit.Count, 1];
                for (int i = 0; i < YLit.Count; i++)
                    Yarray[i, 0] = YLit[i];
                YMat = CreateMatrix.DenseOfArray<float>(Yarray);

                //Build R matrix
                foreach (var pointF in pointFs.Points)
                    RLit.Add(new float[] { (float)(-pointF.X), (float)(-pointF.Y), -1 });
                float[,] Rarray = new float[RLit.Count, 3];
                for (int i = 0; i < RLit.Count; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Rarray[i, j] = RLit[i][j];
                    }
                }
                RMat = CreateMatrix.DenseOfArray<float>(Rarray);
                Matrix<float> RTMat = RMat.Transpose();
                Matrix<float> RRTInvMat = (RTMat.Multiply(RMat)).Inverse();
                AMat = RRTInvMat.Multiply(RTMat.Multiply(YMat));

                float[,] Aarray = AMat.ToArray();
                float A = Aarray[0, 0];
                float B = Aarray[1, 0];
                float C = Aarray[2, 0];
                CenterX = A / -2.0f;
                CenterY = B / -2.0f;
                CenterR = (float)(Math.Sqrt((A * A + B * B - 4 * C)) / 2.0f);

                if (float.IsNaN(CenterX)) { CenterX = 0.0f; return false; }
                if (float.IsNaN(CenterY)) { CenterX = 0.0f; return false; }
                if (float.IsNaN(CenterR)) { CenterX = 0.0f; return false; }

                return true;
            }
            catch (Exception err) 
            {
                Debug.WriteLine($"[Exception] FitFrameCenter = {err.Message}");
                CenterX = 0; CenterY = 0; CenterR = 0; 
                return false; 
            }
        }
        //----------------------------------------------------------------------------
        public static Point2f CorrectDistotion(Point2f pCenter, Point2f pPoint, double dDistotion)
        {
            double DistanceX = Math.Abs(pCenter.X - pPoint.X);
            double DistanceY = Math.Abs(pCenter.Y - pPoint.Y);

            float x, y;

            //Cal X
            if (pPoint.X < pCenter.X)
                x = (float)(pPoint.X + (DistanceX * dDistotion / 100));
            else
                x = (float)(pPoint.X - (DistanceX * dDistotion / 100));

            //Cal Y
            if (pPoint.Y < pCenter.Y)
                y = (float)(pPoint.Y + (DistanceY * dDistotion / 100));
            else
                y = (float)(pPoint.Y - (DistanceY * dDistotion / 100));

            return new Point2f(x, y);

        }
        //----------------------------------------------------------------------------
        public void Threshold(BUFF Source, BUFF Destnation, int iLow)
        {
            TVisnRslt Rslt = new TVisnRslt();
            Rslt.ResetData();

            //Rect region = GetRegion(Param.sROIName);
            //
            //if (CheckRegion(region)) { Rslt.InspRslt = (int)EN_VISN_ERROR.ROI; return Rslt; }
        }
    }
}
