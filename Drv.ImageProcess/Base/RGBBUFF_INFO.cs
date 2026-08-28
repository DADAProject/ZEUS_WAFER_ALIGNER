using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;
using Internal_Open_eVision_22_04_22_04;
using Euresys.Open_eVision_22_04;
using OpenCvSharp;
using System.Runtime.InteropServices;
using System.Numerics;

namespace Drv.ImageProcess
{
	public struct RGBBUFF : IDisposable
	{
		public BufferType buffType { get; set; }
		public MIL_ID buffID       { get; set; }

		public EImageC24 EbuffID   { get; set; }
		public Mat ObuffID		   { get; set; }

		public int pitch           { get; set; }
		public int wid             { get; set; }
		public int len             { get; set; }
		public byte[] pBuff        { get; set; }

		public RGBBUFF(BufferType type = BufferType.Mil)
		{
			this.buffType    = type;
			this.buffID      = MIL.M_NULL;
			this.EbuffID	 = null;
			this.ObuffID     = null;
			this.pitch       = int.MinValue;
			this.wid         = int.MinValue;
			this.len         = int.MinValue;
			this.pBuff       = null;
		}

		//나중에 함수명 바꿔야함
		public bool AllocBuffInfo(RGBBUFF info)
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID == MIL.M_NULL)
				{
					MIL_ID MilImage = MIL.M_NULL;
					MIL.MbufAllocColor(Alloc.SystemAlloc, 3, info.wid, info.len, + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);
					this.buffID = MilImage;
					InitBuffInfo(false, 0);
					return true;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID == null)
				{
					//EImageBW8 EImage = new EImageBW8(info.wid, info.len);
					//this.EbuffID = EImage;
					InitBuffInfo(false, 0);
					return true;
				}
			}
			else if (this.buffType == BufferType.Opencv)
			{
				if (this.ObuffID == null)
				{
					this.ObuffID = new Mat(info.len, info.wid, info.ObuffID.Type());
					InitBuffInfo(false, 0);
					return true;
				}
			}
			return false;
		}

		public bool AllocBuffInfo(int width, int height)
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID == MIL.M_NULL)
				{
					MIL_ID MilImage = MIL.M_NULL;
					MIL.MbufAllocColor(Alloc.SystemAlloc, 3, width, height, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);
					buffID = MilImage;
					InitBuffInfo(false, 0);
					return true;
				}
			}

			return false;
		}

		public bool InitBuffInfo(bool bClearBuffer/*=FALSE*/, byte byInitGV/*=0*/)
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID != MIL.M_NULL)
				{
					this.pitch = (int)MIL.MbufInquire(this.buffID, MIL.M_PITCH, MIL.M_NULL);
					this.wid   = (int)MIL.MbufInquire(this.buffID, MIL.M_SIZE_X, MIL.M_NULL);
					this.len   = (int)MIL.MbufInquire(this.buffID, MIL.M_SIZE_Y, MIL.M_NULL);
					
					this.pBuff = new byte[this.wid * this.len * 3];
					MIL.MbufGet(this.buffID, pBuff);

					if (bClearBuffer)
						MIL.MbufClear(this.buffID, byInitGV);

					return true;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID != null)
				{
					this.pitch = (int)this.EbuffID.ColPitch;
					this.wid   = (int)this.EbuffID.Width;
					this.len   = (int)this.EbuffID.Height;
					this.pBuff = new byte[this.wid * this.len];

					return true;
				}
			}
			else if (this.buffType == BufferType.Opencv)
			{
				if (this.ObuffID != null)
				{
					this.pitch = (int)this.ObuffID.Width;
					this.wid = (int)this.ObuffID.Width;
					this.len = (int)this.ObuffID.Height;
					this.pBuff = new byte[this.wid * this.len];

					Marshal.Copy(this.ObuffID.Data, this.pBuff, 0, this.wid * this.len * 3);

					if (bClearBuffer)
						this.ObuffID.SetTo(new Scalar(byInitGV));
					
					return true;
				}
			}

			return false;
		}

		public bool DisposeBuffInfo()
		{
			if (this.buffType      == BufferType.Mil)
			{
				if (this.buffID != MIL.M_NULL)
				{
					MIL.MbufFree(this.buffID);
					this.buffID    = MIL.M_NULL;
					this.pitch     = int.MinValue;
					this.wid       = int.MinValue;
					this.len       = int.MinValue;
					this.pBuff     = null;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID != null)
				{
					this.EbuffID.Dispose();
					this.pitch     = int.MinValue;
					this.wid       = int.MinValue;
					this.len       = int.MinValue;
					this.pBuff     = null;
				}
			}
			else if (this.buffType == BufferType.Opencv)
			{
				if (this.ObuffID != null)
				{
					this.ObuffID.Dispose();
					this.pitch     = int.MinValue;
					this.wid       = int.MinValue;
					this.len       = int.MinValue;
					this.pBuff     = null;
				}
			}

			return true;
		}

		public bool ImportBuffInfo(string sPath)
		{
			if (this.buffType == BufferType.Mil)
			{
				MIL_ID MilImage = MIL.M_NULL;
				MIL.MbufRestore(sPath, Alloc.SystemAlloc, ref MilImage);
				DisposeBuffInfo();
				this.buffID = MilImage;
				InitBuffInfo(false, 0);
			}
			else if (this.buffType == BufferType.Evision)
			{
				//칼라일때 검증해야함
				EImageBW8 EImage = new EImageBW8();
				EImage.Load(sPath);
				DisposeBuffInfo();
				//this.EbuffID = EImage;
				InitBuffInfo(false, 0);
			}
			else if (this.buffType == BufferType.Opencv)
			{
				this.ObuffID = new Mat(sPath, ImreadModes.Color);
				InitBuffInfo(false, 0);
			}
			return true;
		}
		public bool ImportBuffInfo(byte[] pBuffer, int pWidth, int pHeight)
		{
			if (this.buffType == BufferType.Mil)
			{
			
			}
			else if (this.buffType == BufferType.Evision)
			{
				
			}
			else if (this.buffType == BufferType.Opencv)
			{
				this.ObuffID = new Mat(new Size(pWidth, pHeight), MatType.CV_8UC3);
				this.wid = pWidth;
				this.len = pHeight;
				this.pitch = pWidth;
				Cv2.ImDecode(pBuffer, ImreadModes.Color);
				Buffer.BlockCopy(pBuffer, 0, this.pBuff, 0, pWidth * pHeight * 3);
			}
			return true;
		}

		public bool ExportBuffInfo(string sPath)
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID != MIL.M_NULL)
				{
					MIL.MbufExport(sPath, MIL.M_TIFF, this.buffID);

					return true;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				EbuffID.Save(sPath);
			}
			else if (this.buffType == BufferType.Opencv)
			{
				ObuffID.SaveImage(sPath);
			}
			return false;
		}

		public void Dispose()
        {
			DisposeBuffInfo();
		}
    }
}
