using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;

namespace Drv.ImageProcess.Core
{
    internal partial class Arithmatic
    {
        internal bool Arith_OneLogic(EImageBW8 mSrcID, EImageBW8 mDstID, LOGIC_1_OPERATION eOper)
        {
            switch (eOper)
            {
                case LOGIC_1_OPERATION.E_LOGIC_1_INVERT:
                    EasyImage.Oper(EArithmeticLogicOperation.Invert, mSrcID, mDstID);
                    break;
            }

            return true;
        }
        internal bool Arith_TwoLogic(EImageBW8 mSrcID1, EImageBW8 mSrcID2, EImageBW8 mDstID, LOGIC_2_OPERATION eOper)
        {


            return true;
        }


        internal bool Arith_OneCalcurate(EImageBW8 mSrcID, ARITH_1_OPERATION eOper, out float fValue)
        {
            fValue = 0;

            switch (eOper)
            {
                case ARITH_1_OPERATION.E_ARITH_1_MINIMUM:
                    EasyImage.PixelMinBW8(mSrcID, out EBW8 Min); fValue = Min.Value;
                    break;
                case ARITH_1_OPERATION.E_ARITH_1_MAXIMUM:
                    EasyImage.PixelMaxBW8(mSrcID, out EBW8 Max); fValue = Max.Value;
                    break;
                case ARITH_1_OPERATION.E_ARITH_1_AVERAGE:
                    EasyImage.PixelAverage(mSrcID, out float average); fValue = average;
                    break;
                case ARITH_1_OPERATION.E_ARITH_1_MEAN:
                    EasyImage.PixelVariance(mSrcID, out float Dump1, out float Mean); fValue = Mean;
                    break;
                case ARITH_1_OPERATION.E_ARITH_1_VARIANCE:
                    EasyImage.PixelVariance(mSrcID, out float Variance, out float Dump2); fValue = Variance;
                    break;
                case ARITH_1_OPERATION.E_ARITH_1_STDDEV:
                    EasyImage.PixelStdDev(mSrcID, out float StdDev, out float Dump3); fValue = StdDev;
                    break;
            }

            return true;
        }


        internal bool Arith_GravityCenter(EImageBW8 mSrcID, int iCondLow, out Point2f ptCenter)
        {
            EasyImage.GravityCenter(mSrcID, (uint)iCondLow, out float gravityCenterX, out float gravityCenterY);

            ptCenter = new Point2f(gravityCenterX, gravityCenterY);

            return true;
        }

        internal bool Arith_PixelCount(EImageBW8 mSrcID, ARITH_PIXEL_OPERATION eOper, int iCondLow, int iCondHigh, out int iPixels)
        {
            iPixels = 0;
            EBW8 Low = new EBW8((byte)iCondLow);
            EBW8 High = new EBW8((byte)iCondHigh);

            switch (eOper)
            {
                case ARITH_PIXEL_OPERATION.E_ARITH_PIXEL_BELOW:
                    EasyImage.PixelCount(mSrcID, Low, High, out iPixels, out int Dump1, out int Dump2);
                    break;
                case ARITH_PIXEL_OPERATION.E_ARITH_PIXEL_BETWEEN:
                    EasyImage.PixelCount(mSrcID, Low, High, out int Dump3, out iPixels, out int Dump4);
                    break;
                case ARITH_PIXEL_OPERATION.E_ARITH_PIXEL_ABOVE:
                    EasyImage.PixelCount(mSrcID, Low, High, out int Dump5, out int Dump6, out iPixels);
                    break;
            }

            return true;
        }

        internal bool Arith_Projection(EImageBW8 mSrcID, BUFF mDstID, ARITH_PROJECT_DIR eDir)
        {
            EBW8Vector mVector = new EBW8Vector();

            if (eDir == ARITH_PROJECT_DIR.E_ARITH_PROJECT_COLUMN)
            {
                EasyImage.ProjectOnAColumn(mSrcID, mVector);
                mDstID.pBuff = new byte[mSrcID.Height];

                for (int i = 0; i < mSrcID.Height; i++)
                    mDstID.pBuff[i] = (byte)mVector.GetElement(i).Value;
            }
            else
            {
                EasyImage.ProjectOnARow(mSrcID, mVector);
                mDstID.pBuff = new byte[mSrcID.Width];

                for (int i = 0; i < mSrcID.Width; i++)
                    mDstID.pBuff[i] = (byte)mVector.GetElement(i).Value;
            }

            return true;
        }

    }
}
