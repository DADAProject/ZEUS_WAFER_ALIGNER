using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Euresys.Open_eVision_22_04;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Drv.ImageProcess.Core
{
    internal partial class Measurement
    {
        internal bool LineMeasuring(CogImage8Grey mSrc, CogImage8Grey mDst, stMeasParam param, MAKER_BOX_OPERATION nBoxSet,
            Point2f nBoxSet_Center, int nBoxSet_Width, int nBoxSet_Height, int nBoxSet_Angle,
            ref Line2D pLine, ref double pWidth)
        {
            bool bRetVal = false;

            double dMeasWitdhStd               = param.MeasWidthStd;
            int iMeasWidth_Up_OffSet_Percent   = param.MeasWidth_Up_OffSet_Percent;
            int iMeasWidth_Down_OffSet_Percent = param.MeasWidth_Down_OffSet_Percent;
            CogCaliperPolarityConstants lFstPolarity = CogCaliperPolarityConstants.DontCare;
            CogCaliperPolarityConstants lSecPolarity = CogCaliperPolarityConstants.DontCare;
            long lType                         = (long)param.Type;
            long lOrienatation                 = (long)param.Orienatation;

            switch (param.FstPolarity)
            {
                case MAKER_POLARITY.E_NEGATIVE: lFstPolarity = CogCaliperPolarityConstants.DarkToLight; break;
                case MAKER_POLARITY.E_POSITIVE: lFstPolarity = CogCaliperPolarityConstants.LightToDark; break;
                case MAKER_POLARITY.E_OPPOSITE: 
                case MAKER_POLARITY.E_SAME:
                    lFstPolarity = CogCaliperPolarityConstants.DontCare; break;
            }
            switch (param.SecPolarity)
            {
                case MAKER_POLARITY.E_NEGATIVE: lSecPolarity = CogCaliperPolarityConstants.DarkToLight; break;
                case MAKER_POLARITY.E_POSITIVE: lSecPolarity = CogCaliperPolarityConstants.LightToDark; break;
                case MAKER_POLARITY.E_OPPOSITE:
                case MAKER_POLARITY.E_SAME:
                    lSecPolarity = CogCaliperPolarityConstants.DontCare; break;
            }

            CogFindLineTool LineTool = new CogFindLineTool();

            LineTool.RunParams.CaliperRunParams.Edge0Polarity = lFstPolarity;
            LineTool.RunParams.CaliperRunParams.Edge1Polarity = lSecPolarity;


            LineTool.InputImage = mSrc;
            LineTool.RunParams.CaliperSearchDirection = nBoxSet_Angle * 180.0 / Math.PI;
          
            if (nBoxSet == MAKER_BOX_OPERATION.E_MAKER_BOX_ENABLE)
            {
                //LineTool.RunParams.ExpectedLineSegment.select
            }
            else
            {

            }

            double dFstX = 0; double dFstY = 0;
            double dSecX = 0; double dSecY = 0;
            double dWidth= 0; double dAngle = 0;
            double dCX = 0; double dCY = 0;

            LineTool.Run();

            int nNumResults = (int)0;

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

            LineTool.Dispose();

            return bRetVal;
        }
    }
}
