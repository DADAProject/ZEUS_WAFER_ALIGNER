using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess.Core
{
    internal class Rotate
    {
		internal bool MilRotate(MIL_ID mSrcID, MIL_ID mDstID, double dAngle, double dSrcCenterPosX, double dSrcCenterPosY, double dDstCenterPosX, double dDstCenterPosY)
		{
			MIL.MbufCopy(mSrcID, mDstID);
			//////////////////////////////////////////////////////////////////////////
			// Funtion Initialize
			BUFF biSrc = new BUFF();
			BUFF biDst = new BUFF();
			biSrc.buffID = mSrcID;
			biDst.buffID = mDstID;

			biSrc.InitBuffInfo(false, 0);
			biDst.InitBuffInfo(true , 0);

			if (dAngle != 0) 
			{
				MimRotate(biSrc.buffID, biDst.buffID, dAngle, dSrcCenterPosX, dSrcCenterPosY, dDstCenterPosX, dDstCenterPosY);
			}
			else
			{
				MIL.MbufCopy(biSrc.buffID, biDst.buffID);
			}
			return true;
		}

		internal void MimRotate(MIL_ID mSrcID, MIL_ID mDstID, double dAngle, double dSrcCenterPosX, double dSrcCenterPosY, double dDstCenterPosX, double dDstCenterPosY)
		{
			MIL.MimRotate(mSrcID, mDstID, dAngle, dSrcCenterPosX, dSrcCenterPosY, dDstCenterPosX, dDstCenterPosY, MIL.M_DEFAULT);
		}
	}
}
