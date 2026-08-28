using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Implementation;
using Drv.ImageProcess.Extension;
using Euresys.Open_eVision_22_04;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;

namespace Drv.ImageProcess.Core
{
    internal partial class Binarize
    {
		internal bool OneLimit_Binarize(CogImage8Grey mSrcID, CogImage8Grey mDstID, BINARIZE_ONELIMIT_OPERATION eOperation, double dCondLow)
		{
			using (var PixelMap = new CogIPOneImagePixelMap())
			{
                byte[] LookUp = new byte[256];

                switch (eOperation)
                {
                    case BINARIZE_ONELIMIT_OPERATION.E_IN_RANGE:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow >  (double)idx)  LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_OUT_RANGE:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow <  (double)idx) LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_EQUAL:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow == (double)idx) LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_NOT_EQUAL:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow != (double)idx) LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_GREATER:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow <  (double)idx) LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_LESS:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow >  (double)idx) LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_GREATER_OR_EQUAL:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow <= (double)idx) LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_LESS_OR_EQUAL:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow >= (double)idx) LookUp[idx] = 255;
                        break;
                    case BINARIZE_ONELIMIT_OPERATION.E_MASK:
                        break;
                }
                PixelMap.SetMap(LookUp);
                VisionProExtension.bufCopy((CogImage8Grey)PixelMap.Execute(mSrcID, CogRegionModeConstants.PixelAlignedBoundingBox, null), mDstID);
            }


            return true;
		}


        internal bool TwoLimit_Binarize(CogImage8Grey mSrcID, CogImage8Grey mDstID, BINARIZE_TWOLIMIT_OPERATION eOperation, double dCondLow, double dCondHigh)
        {
            using (var PixelMap = new CogIPOneImagePixelMap())
            {
                byte[] LookUp = new byte[256];

                switch (eOperation)
                {
                    case BINARIZE_TWOLIMIT_OPERATION.E_TRIANGLE_BISECTION_BRIGHT:
                        break;
                    case BINARIZE_TWOLIMIT_OPERATION.E_TRIANGLE_BISECTION_DARK:
                        break;
                    case BINARIZE_TWOLIMIT_OPERATION.E_BIMODAL:
                        break;
                    case BINARIZE_TWOLIMIT_OPERATION.E_FIXED:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow < (double)idx && (double)idx < dCondHigh) LookUp[idx] = 255;
                        break;
                    case BINARIZE_TWOLIMIT_OPERATION.E_DISCRETE_RANGE:
                        for (int idx = 0; idx < LookUp.Length; idx++) if (dCondLow > (double)idx || (double)idx > dCondHigh) LookUp[idx] = 255;
                        break;
                    case BINARIZE_TWOLIMIT_OPERATION.E_RETURN_VALUE_AS_FLOAT_IN_INT:
                        break;
                    default:
                        break;
                }
                PixelMap.SetMap(LookUp);
                VisionProExtension.bufCopy((CogImage8Grey)PixelMap.Execute(mSrcID, CogRegionModeConstants.PixelAlignedBoundingBox, null), mDstID);
            }


            return true;
        }

    }
}
