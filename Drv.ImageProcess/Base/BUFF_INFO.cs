using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Matrox.MatroxImagingLibrary;
using Internal_Open_eVision_22_04_22_04;
using Euresys.Open_eVision_22_04;
using OpenCvSharp;
using HalconDotNet;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Drv.ImageProcess.Extension;
using OpenCvSharp.Extensions;

namespace Drv.ImageProcess
{
	public enum BufferType
	{
		Mil,
		Evision,
		Opencv,
		Halcon,
		VisionPro,
	}

	public struct BUFF : IDisposable
	{
		#region << Library Fields >>
		internal MIL_ID buffID         { get; set; }
		internal EImageBW8 EbuffID     { get; set; }
		internal Mat ObuffID           { get; set; }
		internal HImage HbuffID        { get; set; }
		internal Object CbuffID { get; set; }
		#endregion

		public BufferType buffType { get; set; }
		public int pitch { get; set; }
		public int wid { get; set; }
		public int len { get; set; }
		public byte[] pBuff { get; set; }

		//===========================
		public bool Allocated { get; set; }

		//일단은 하나만 사용
		public Rect ROI { get; set; }
		internal bool UseROI { get; set; }

		public BUFF(BufferType type = BufferType.Mil)
		{
			this.buffType = type;

			this.buffID   = MIL.M_NULL;
			this.EbuffID  = null;
			this.ObuffID  = null;
			this.HbuffID  = null;
			this.CbuffID  = null;

			this.pBuff    = null;
			this.pitch    = int.MinValue;
			this.wid      = int.MinValue;
			this.len      = int.MinValue;
			this.Allocated = false;

			ROI    = new Rect();
			UseROI = false;
		}

		public bool AllocBuffInfo(BUFF info)
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID == MIL.M_NULL)
				{
					MIL_ID MilImage = MIL.M_NULL;
					MIL.MbufAlloc2d(Alloc.SystemAlloc, info.wid, info.len, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);
					this.buffID = MilImage;
					InitBuffInfo(false, 0, false);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID == null)
				{
					this.pitch = info.pitch;
					this.wid = info.wid;
					this.len = info.len;
					this.EbuffID = new EImageBW8(info.wid, info.len);
                    InitBuffInfo(false, 0, false);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.Opencv)
			{
				if (this.ObuffID == null)
				{
					this.ObuffID = new Mat(info.len, info.wid, info.ObuffID.Type());
					InitBuffInfo(false, 0);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.Halcon)
			{
				if (this.HbuffID == null)
				{
					this.HbuffID = new HImage(info.HbuffID.GetImageType(), info.wid, info.len);
					InitBuffInfo(false, 0);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.VisionPro)
			{
				if (this.CbuffID == null)
				{
					//this.CbuffID = new CogImage8Grey(info.wid, info.len);
					InitBuffInfo(false, 0);
					this.Allocated = true;
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
					MIL.MbufAlloc2d(Alloc.SystemAlloc, width, height, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);
					buffID = MilImage;
					InitBuffInfo(false, 0, false);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID == null)
				{
					this.pitch = (int)width;
					this.wid = (int)width;
					this.len = (int)height;
					this.EbuffID = new EImageBW8(this.wid, this.len);
					InitBuffInfo(false, 0, false);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.Opencv)
			{
				if (this.ObuffID == null)
				{
					this.pitch = (int)width;
					this.wid = (int)width;
					this.len = (int)height;
					this.ObuffID = new Mat(this.len, this.wid, MatType.CV_8UC1);
					InitBuffInfo(false, 0);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.Halcon)
			{
				if (this.HbuffID == null)
				{
					this.pitch = (int)width;
					this.wid = (int)width;
					this.len = (int)height;
					this.HbuffID = new HImage("byte", this.wid, this.len);
					InitBuffInfo(false, 0);
					this.Allocated = true;
					return true;
				}
			}
			else if (this.buffType == BufferType.VisionPro)
			{
				if (this.CbuffID == null)
				{
					this.pitch = (int)width;
					this.wid = (int)width;
					this.len = (int)height;
					//this.CbuffID = new CogImage8Grey(this.wid, this.len);
					InitBuffInfo(false, 0);
					this.Allocated = true;
					return true;
				}
			}
			return false;
		}

		public bool InitBuffInfo(bool bClearBuffer/*=FALSE*/, byte byInitGV/*=0*/, bool bCopyBuffer = true)
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID != MIL.M_NULL)
				{
					this.pitch = (int)MIL.MbufInquire(this.buffID, MIL.M_PITCH, MIL.M_NULL);
					this.wid = (int)MIL.MbufInquire(this.buffID, MIL.M_SIZE_X, MIL.M_NULL);
					this.len = (int)MIL.MbufInquire(this.buffID, MIL.M_SIZE_Y, MIL.M_NULL);

					if (bCopyBuffer)
					{
						this.pBuff = new byte[this.wid * this.len];
						MIL.MbufGet(this.buffID, pBuff);

						if (bClearBuffer)
							MIL.MbufClear(this.buffID, byInitGV);
					}
					return true;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID != null)
				{
					this.pitch = (int)this.EbuffID.ColPitch;
					this.wid = (int)this.EbuffID.Width;
					this.len = (int)this.EbuffID.Height;

					if (bCopyBuffer)
					{
						this.pBuff = new byte[this.wid * this.len];
						IntPtr Pointer = this.EbuffID.GetImagePtr(0, 0);
						Marshal.Copy(Pointer, this.pBuff, 0, this.pBuff.Length);

						if (bClearBuffer)
							EvisionExtension.SetClear(this.EbuffID, byInitGV);
					}

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

					if (bCopyBuffer)
					{
						this.pBuff = new byte[this.wid * this.len];
						Marshal.Copy(this.ObuffID.Data, this.pBuff, 0, this.wid * this.len);

						if (bClearBuffer)
							this.ObuffID.SetTo(new Scalar(byInitGV));
					}

					return true;
				}
			}
			else if (this.buffType == BufferType.Halcon)
			{
				if (this.HbuffID != null)
				{
					this.HbuffID.GetImageSize(out HTuple Width, out HTuple Height);
					this.pitch = (int)Width.I;
					this.wid   = (int)Width.I;
					this.len   = (int)Height.I;

					if (bCopyBuffer)
					{
						IntPtr Pointer = this.HbuffID.GetImagePointer1(out HTuple TempType, out HTuple TempWidth, out HTuple TempHeight);
						this.pBuff = new byte[this.wid * this.len];
						Marshal.Copy(Pointer, this.pBuff, 0, this.wid * this.len);

						if (bClearBuffer)
							HalconExtension.SetClear(this.HbuffID);
					}

					return true;
				}
			}
			else if (this.buffType == BufferType.VisionPro)
			{
				if (this.CbuffID != null)
				{
					//this.pitch = (int)this.CbuffID.Width;
					//this.wid   = (int)this.CbuffID.Width;
					//this.len   = (int)this.CbuffID.Height;
				
					//if (bCopyBuffer)
					//{
					//	var Pointer = this.CbuffID.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite,0, 0, this.CbuffID.Width, this.CbuffID.Height);
					//	this.pBuff = new byte[this.wid * this.len];
					//	Marshal.Copy(Pointer.Scan0, this.pBuff, 0, this.wid * this.len);
						
					//	if (bClearBuffer)
					//		VisionProExtension.SetClear(this.CbuffID);
					//}
				
					return true;
				}
			}
			return false;
		}

		public bool DisposeBuffInfo()
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID != MIL.M_NULL)
				{
					MIL.MbufFree(this.buffID);
					this.buffID = MIL.M_NULL;
					this.pitch = int.MinValue;
					this.wid = int.MinValue;
					this.len = int.MinValue;
					this.pBuff = null;
					this.Allocated = false;
				}
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID != null)
				{
					this.EbuffID.Dispose();
					this.pitch = int.MinValue;
					this.wid = int.MinValue;
					this.len = int.MinValue;
					this.pBuff = null;
					this.Allocated = false;
				}
			}
			else if (this.buffType == BufferType.Opencv)
			{
				if (this.ObuffID != null)
				{
					this.ObuffID.Dispose();
					this.pitch = int.MinValue;
					this.wid = int.MinValue;
					this.len = int.MinValue;
					this.pBuff = null;
					this.Allocated = false;
				}
			}
			else if (this.buffType == BufferType.Halcon)
			{
				if (this.HbuffID != null)
				{
					this.HbuffID.Dispose();
					this.pitch = int.MinValue;
					this.wid = int.MinValue;
					this.len = int.MinValue;
					this.pBuff = null;
					this.Allocated = false;
				}
			}
			else if (this.buffType == BufferType.VisionPro)
			{
				if (this.CbuffID != null)
				{
					//this.CbuffID.Dispose();
					this.CbuffID = null; 
					this.pitch = int.MinValue;
					this.wid = int.MinValue;
					this.len = int.MinValue;
					this.pBuff = null;
					this.Allocated = false;
				}
			}

			GC.SuppressFinalize(this);
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
				InitBuffInfo(false, 0, false);
				this.Allocated = true;
				return true;
			}
			else if (this.buffType == BufferType.Evision)
			{
				EImageBW8 EImage = new EImageBW8();
				EImage.Load(sPath);
				DisposeBuffInfo();
				this.EbuffID = EImage;
				InitBuffInfo(false, 0, false);
				this.Allocated = true;
				return true;
			}
			else if (this.buffType == BufferType.Opencv)
			{
				DisposeBuffInfo();
				this.ObuffID = new Mat(sPath, ImreadModes.Grayscale);
				InitBuffInfo(false, 0, false);
				this.Allocated = true;
				return true;
			}
			else if (this.buffType == BufferType.Halcon)
			{         
				HImage HImage = new HImage();
 				HImage.ReadImage(sPath);

				DisposeBuffInfo();
				if (1 < HImage.CountChannels())
				{		
					this.HbuffID = HImage.Rgb1ToGray();
					HImage.Dispose();
				}
				else
					this.HbuffID = HImage;

				InitBuffInfo(false, 0, false);
				this.Allocated = true;
				return true;
			}
			else if (this.buffType == BufferType.VisionPro)
			{
				//using (CogImageFileTool Tool = new CogImageFileTool())
				//{
                //    DisposeBuffInfo();
                //    Tool.Operator.Open(sPath, CogImageFileModeConstants.Read);
				//	Tool.Run();
				//	this.CbuffID = Tool.OutputImage as CogImage8Grey;
                //    InitBuffInfo(false, 0, false);
                //    this.Allocated = true;
				//	return true;
				//}
			}
			return false;
		}
		public unsafe bool ImportBuffInfo(byte[] pBuffer, int pWidth, int pHeight)
		{
			if (this.Allocated != true) return false;

			if (this.buffType == BufferType.Mil)
			{
				MIL.MbufPut(this.buffID, pBuff);
				return true;
			}
			else if (this.buffType == BufferType.Evision)
			{
				IntPtr CopyPtr = Marshal.AllocHGlobal(pWidth * pHeight);
				Marshal.Copy(pBuffer, 0, CopyPtr, pWidth * pHeight);
				this.EbuffID.SetImagePtr(pWidth, pHeight, CopyPtr);
				Marshal.FreeHGlobal(CopyPtr);
				return true;
			}
			else if (this.buffType == BufferType.Opencv)
			{
				//Need Checked
				this.ObuffID = Cv2.ImDecode(pBuffer, ImreadModes.Grayscale);
				//Cv2.ImDecode(pBuffer, ImreadModes.Grayscale);
				//Buffer.BlockCopy(pBuffer, 0, this.pBuff, 0, pWidth * pHeight);

				return true;
			}
			else if (this.buffType == BufferType.Halcon)
			{
				//Need Checked
				IntPtr CopyPtr = Marshal.AllocHGlobal(pWidth * pHeight);
				Marshal.Copy(pBuffer, 0, CopyPtr, pWidth * pHeight);
				IntPtr Pointer = this.HbuffID.GetImagePointer1(out HTuple Type, out HTuple Width, out HTuple Height);
				Buffer.MemoryCopy(Pointer.ToPointer(), CopyPtr.ToPointer(), pWidth * pHeight, pWidth * pHeight);


				return true;
			}
			else if (this.buffType == BufferType.VisionPro)
			{
				//Need Checked
				IntPtr CopyPtr = Marshal.AllocHGlobal(pWidth * pHeight);
				Marshal.Copy(pBuffer, 0, CopyPtr, pWidth * pHeight);
				CogImage8Root Root = new CogImage8Root();
				Root.Initialize(pWidth, pHeight, CopyPtr, pWidth, null);
				//this.CbuffID.SetRoot(Root);
				Marshal.FreeHGlobal(CopyPtr);

				return true;
			}

			return false;
		}
		public unsafe bool ImportBuffInfo(IntPtr pBuffer, int pWidth, int pHeight)
		{
			if (this.Allocated != true) return false;

			if (this.buffType == BufferType.Mil)
			{
				byte[] pTempBuff = new byte[pWidth * pHeight];
				Marshal.Copy(pTempBuff, 0, pBuffer, pWidth * pHeight);
				MIL.MbufPut(this.buffID, pTempBuff);

				return true;
			}
			else if (this.buffType == BufferType.Evision)
			{
				for (int y = 0; y < pHeight; y++)
				{
					//get the data from the new image
					byte* nSrcRow = (byte*)pBuffer.ToPointer() + (y * pWidth);
					byte* nDstRow = (byte*)this.EbuffID.GetImagePtr().ToPointer() + (y * this.EbuffID.RowPitch);

					Buffer.MemoryCopy(nSrcRow, nDstRow, pWidth, pWidth);
				}

				return true;
			}
			else if (this.buffType == BufferType.Opencv)
			{
				IntPtr ptr = this.ObuffID.Ptr(0, 0);
				Buffer.MemoryCopy(pBuffer.ToPointer(), ptr.ToPointer(), pWidth * pHeight, pWidth * pHeight);
				return true;
			}
			else if (this.buffType == BufferType.Halcon)
			{
				//Need Checked
				IntPtr Pointer = this.HbuffID.GetImagePointer1(out HTuple Type, out HTuple Width, out HTuple Height);
				Buffer.MemoryCopy(pBuffer.ToPointer(), Pointer.ToPointer(), pWidth * pHeight, pWidth * pHeight);

				return true;
			}
			else if (this.buffType == BufferType.VisionPro)
			{
		
				return true;
			}

			return false;
		}
		public bool ExportBuffInfo(string sPath)
		{
			if (this.buffType == BufferType.Mil)
			{
				if (this.buffID == MIL.M_NULL) return false;

				MIL.MbufExport(sPath, MIL.M_TIFF, this.buffID);
			}
			else if (this.buffType == BufferType.Evision)
			{
				if (this.EbuffID == null) return false;

				this.EbuffID.Save(sPath);
			}
			else if (this.buffType == BufferType.Opencv)
			{
				if (this.ObuffID == null) return false;

				this.ObuffID.SaveImage(sPath);
			}
			else if (this.buffType == BufferType.VisionPro)
			{
				
			}
			return false;
		}

		public System.Drawing.Bitmap ToBitmap(System.Drawing.Imaging.PixelFormat pPixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb)
		{
			if (this.Allocated != true) return null;

			if (this.buffType == BufferType.Mil)
			{

			}
			else if (this.buffType == BufferType.Evision)
			{
				return EvisionExtension.ToBitmap(this.EbuffID, pPixelFormat);
			}
			else if (this.buffType == BufferType.Opencv)
			{
                return BitmapConverter.ToBitmap(this.ObuffID, pPixelFormat);
            }

            return null;
		}

		//
		internal BUFF CopyBuffer(MIL_ID mDstID, int nX_Src, int nY_Src, int nCx_Src, int nCy_Src, int nX_Dst, int nY_Dst)
		{
			BUFF biDst = new BUFF();
			biDst.buffID = mDstID;
			biDst.InitBuffInfo(true, 0);

			int nNew_SrcX = nX_Src;
			int nNew_SrcCx = nCx_Src;
			if (nNew_SrcX + nNew_SrcCx >= this.wid)
				nNew_SrcCx = this.wid - nNew_SrcX;

			int nNew_SrcY = nY_Src;
			int nNew_SrcCy = nCy_Src;
			if (nNew_SrcY + nNew_SrcCy >= this.len)
				nNew_SrcCy = this.len - 1 - nNew_SrcY;

			int nNew_DstX = nX_Dst;
			if (nNew_DstX + nNew_SrcCx >= biDst.wid)
				nNew_SrcCx = biDst.wid - 1 - nNew_DstX;

			int nNew_DstY = nY_Dst;
			if (nNew_DstY + nNew_SrcCy >= biDst.len)
				nNew_SrcCy = biDst.len - nNew_DstY;

			for (int i = 0; i < nNew_SrcCy; ++i)
			{
				byte[] pSrc = MilExtension.GetBuffer_ByPoint(this.pBuff, biDst.pitch, nNew_SrcCx, nNew_SrcCy, nNew_DstX, nNew_DstY + i);

				Array.Copy(pSrc, 0, biDst.pBuff, biDst.wid * i, biDst.wid);
			}

			return biDst;
		}


		//
		#region << COPY >>
		public BUFF Copy()
		{
			BUFF copy = new BUFF(this.buffType);
			copy.AllocBuffInfo(this);

			if (this.buffType == BufferType.Mil)
			{

			}
			else if (this.buffType == BufferType.Evision)
			{
				this.EbuffID.CopyTo(copy.EbuffID);
			}
			else if (this.buffType == BufferType.Opencv)
			{

			}
			else if (this.buffType == BufferType.Halcon)
			{

			}
			else if (this.buffType == BufferType.VisionPro)
			{
                //copy.CbuffID = this.CbuffID.Copy();
            }

			return copy;
		}
		#endregion

		#region << ROI >>
		public bool SetROI(Rect rect)
		{
			//생각좀 해보자
			if (this.buffType == BufferType.Evision)
			{
				UseROI = true;

				this.ROI = rect;
			}

			return false;
		}

		#endregion
		public void Dispose()
		{
			DisposeBuffInfo();
		}
	}
}
