using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
    internal partial class Measurement
    {
        internal bool LineMeasuring(EImageBW8 mSrc, EImageBW8 mDst, stMeasParam param, MAKER_BOX_OPERATION nBoxSet,
            Point2f nBoxSet_Center, int nBoxSet_Width, int nBoxSet_Height, int nBoxSet_Angle,
            ref Line2D pLine, ref double pWidth)
        {
            bool bRetVal = false;

            double dMeasWitdhStd               = param.MeasWidthStd;
            int iMeasWidth_Up_OffSet_Percent   = param.MeasWidth_Up_OffSet_Percent;
            int iMeasWidth_Down_OffSet_Percent = param.MeasWidth_Down_OffSet_Percent;
            long lFstPolarity                  = (long)param.FstPolarity;
            long lSecPolarity                  = (long)param.SecPolarity;
            long lType                         = (long)param.Type;
            long lOrienatation                 = (long)param.Orienatation;

            ELineGauge LineGauge = new ELineGauge();

            //LineGauge.Rotatable = true;
            //LineGauge.Resizable = true;
            //LineGauge.Dragable = true;
            //LineGauge.SetCenterXY(0, 0);
            //LineGauge.Length = 200;
            //LineGauge.Tolerance = 50;


            if (nBoxSet == MAKER_BOX_OPERATION.E_MAKER_BOX_ENABLE)
            {
                LineGauge.SetCenterXY(nBoxSet_Center.X, nBoxSet_Center.Y);
                LineGauge.Length = nBoxSet_Width;
                LineGauge.Tolerance = nBoxSet_Height;
                LineGauge.Angle = nBoxSet_Angle;
            }
            else
            {

            }
            //LineGauge.Attach(Shape);

            double dFstX = 0; double dFstY = 0;
            double dSecX = 0; double dSecY = 0;
            double dWidth= 0; double dAngle = 0;
            double dCX = 0; double dCY = 0;

            LineGauge.Measure(mSrc);
            int nNumResults = (int)LineGauge.NumSamples;

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
                    pLine = new Line2D(dFstX, dFstY, dSecX, dSecY);
                    pWidth = dWidth;

                }
                else
                {
                    pLine = new Line2D();
                }
            }
            else
            {
                bRetVal = false;
            }

            LineGauge.Dispose();
            //Shape.Dispose();

            return bRetVal;
        }
    }
}
