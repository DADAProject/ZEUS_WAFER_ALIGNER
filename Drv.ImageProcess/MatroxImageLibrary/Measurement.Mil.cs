using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;


namespace Drv.ImageProcess.Core
{
    internal partial class Measurement
    {
        internal bool LineMeasuring(MIL_ID mSrc, MIL_ID mDst, stMeasParam param, MAKER_BOX_OPERATION nBoxSet,
            Point2f nBoxSet_Center, int nBoxSet_Width, int nBoxSet_Height, int nBoxSet_Angle,
            ref LineSegmentPoint pLine, ref double pWidth)
        {
            bool bRetVal = false;

            double dMeasWitdhStd               = param.MeasWidthStd;
            int iMeasWidth_Up_OffSet_Percent   = param.MeasWidth_Up_OffSet_Percent;
            int iMeasWidth_Down_OffSet_Percent = param.MeasWidth_Down_OffSet_Percent;
            long lFstPolarity                  = (long)param.FstPolarity;
            long lSecPolarity                  = (long)param.SecPolarity;
            long lType                         = (long)param.Type;
            long lOrienatation                 = (long)param.Orienatation;

            MIL_ID MakerID = MIL.M_NULL; // 컨텍스트
            //Allocate sa stirip maker
            MIL.MmeasAllocMarker(Alloc.SystemAlloc, lType, MIL.M_DEFAULT, ref MakerID);
            //
            ////Specify the strip characteristics
            MIL.MmeasSetMarker(MakerID, MIL.M_NUMBER, MIL.M_ALL, MIL.M_NULL);
            MIL.MmeasSetMarker(MakerID, MIL.M_POLARITY, lFstPolarity, lSecPolarity); //별로 안중요
            MIL.MmeasSetMarker(MakerID, MIL.M_ORIENTATION, lOrienatation, MIL.M_NULL); //중요

            if (nBoxSet == MAKER_BOX_OPERATION.E_MAKER_BOX_ENABLE)
            {
                //검토해야함
               MIL.MmeasSetMarker(MakerID, MIL.M_BOX_CENTER, nBoxSet_Center.X, nBoxSet_Center.Y);
               MIL.MmeasSetMarker(MakerID, MIL.M_BOX_SIZE, nBoxSet_Width, nBoxSet_Height);
                //MIL.MmeasSetMarker(MakerID, MIL.M_BOX_ANGLE_MODE, MIL.M_ENABLE, MIL.M_NULL);
            }

            //Find the strip and measure its width and angle
            MIL.MmeasFindMarker(MIL.M_DEFAULT, mSrc, MakerID, MIL.M_DEFAULT);

            //Find the strip Nubber
            MIL_INT nNumResults = 0;
            MIL.MmeasGetResult(MakerID, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref nNumResults);

            MIL.MbufClear(mDst, 0);

            double dFstX = 0; double dFstY = 0;
            double dSecX = 0; double dSecY = 0;
            double dWidth= 0; double dAngle = 0;
            double dCX = 0; double dCY = 0;

            if (nNumResults > 0)
            {
                //Get tje stripe position, width and angle
                double[] pdPositionX  = new double[nNumResults];
                double[] pdPositionY  = new double[nNumResults];
                double[] pdFirstEdgeX  = new double[nNumResults];
                double[] pdFirstEdgeY  = new double[nNumResults];
                double[] pdSecondEdgeX = new double[nNumResults];
                double[] pdSecondEdgeY = new double[nNumResults];
                double[] pdWidth       = new double[nNumResults];
                double[] pdAngle       = new double[nNumResults];

                MIL.MmeasGetResult(MakerID, MIL.M_POSITION_X, pdPositionX);
                MIL.MmeasGetResult(MakerID, MIL.M_POSITION_Y, pdPositionY);
                MIL.MmeasGetResult(MakerID, MIL.M_POSITION + MIL.M_EDGE_FIRST, pdFirstEdgeX, pdFirstEdgeY);
                MIL.MmeasGetResult(MakerID, MIL.M_POSITION + MIL.M_EDGE_SECOND, pdSecondEdgeX, pdSecondEdgeY);
                MIL.MmeasGetResult(MakerID, MIL.M_WIDTH , pdWidth);
                MIL.MmeasGetResult(MakerID, MIL.M_ANGLE, pdAngle);

                double dMinDiff = 100000f;

                for (int ni = 0; ni < nNumResults; ni++)
                {
                    double dcal = ((dMeasWitdhStd / 100));

                    if (pdWidth[ni] < dMeasWitdhStd + (dcal * iMeasWidth_Up_OffSet_Percent) &&
                        pdWidth[ni] > dMeasWitdhStd - (dcal * iMeasWidth_Down_OffSet_Percent))

                    {
                        double dDiff = Math.Abs(dMeasWitdhStd - pdWidth[ni]);
                        if (dDiff < dMinDiff)
                        {
                            dDiff = dMinDiff;

                            dFstX  = pdFirstEdgeX[ni] ; dFstY = pdFirstEdgeY[ni];

                            dSecX  = pdSecondEdgeX[ni]; dSecY = pdSecondEdgeY[ni];

                            dWidth = pdWidth[ni]      ; dAngle = pdAngle[ni];

                            dCX    = pdPositionX[ni]  ; dCY = pdPositionY[ni];
                            bRetVal = true;
                        }
                    }

                }

                if (bRetVal)
                {
                    pLine = new LineSegmentPoint(new Point2f((float)dFstX, (float)dFstY), new Point2f((float)dSecX, (float)dSecY));
                    pWidth = dWidth;

                    //MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                    //특정 지정된건만 그리기
                    //MIL.MmeasDraw(MIL.M_DEFAULT, MakerID, mDst, MIL.M_DRAW_POSITION
                    //+ MIL.M_EDGE_FIRST
                    //+ MIL.M_EDGE_SECOND
                    //+ MIL.M_DRAW_WIDTH
                    //, MIL.M_DEFAULT, MIL.M_RESULT);
                    // MIL.MgraArcFill(MIL.M_DEFAULT, mDst, stCenterpt.X, stCenterpt.Y, (double)(nFindRadius - iRadiusOffSet), (double)(nFindRadius - iRadiusOffSet), 0, 360);
                }
                else
                {
                    pLine = new LineSegmentPoint();
                }
            }
            else
            {
                bRetVal = false;
            }

            if (MakerID != null) MIL.MmeasFree(MakerID); MakerID = MIL.M_NULL;


            return bRetVal;
        }
    }
}
