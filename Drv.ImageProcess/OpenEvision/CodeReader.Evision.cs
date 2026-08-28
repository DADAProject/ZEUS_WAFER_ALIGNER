using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Euresys.Open_eVision_22_04;
using Euresys.Open_eVision_22_04.EasyMatrixCode2;

namespace Drv.ImageProcess.Core
{
	internal partial class CodeReader
    {
        int miType = 0;
        bool m_bAutoLearn = true;
        internal bool CodeRead(EImageBW8 mSrcID, EImageBW8 mDstID, string cContextPath, CODE_MODE nMode,  uint iTimeOut, out string sDecoded)
		{
            //Open_eVision_2_17?? 2가 빠른지 1이 빠른지 상황에 따라 다름
            bool ret = false;
            sDecoded = string.Empty;

            if (miType == 0) //EasyMatrixCode2
            {
                Euresys.Open_eVision_22_04.EasyMatrixCode2.EMatrixCodeReader CodeReader = new Euresys.Open_eVision_22_04.EasyMatrixCode2.EMatrixCodeReader(); // EMatrixCodeReader instance
                Euresys.Open_eVision_22_04.EasyMatrixCode2.EMatrixCode[] Result = null;
                CodeReader.TimeOut = iTimeOut;
                CodeReader.ReadMode = nMode == CODE_MODE.E_CODE_SPEED ? EReadMode.Speed : EReadMode.Quality;
                CodeReader.MaxNumCodes = 1;
              
                try
                {
                    if (cContextPath != string.Empty) 
                        CodeReader.Load(cContextPath);

                    if (mSrcID.FirstSubROI == null)
                        CodeReader.Read(mSrcID);
                    else
                        CodeReader.Read(mSrcID.FirstSubROI);

                    Result = CodeReader.ReadResults;
                    sDecoded = Result[0].DecodedString;

                    if (string.IsNullOrEmpty(sDecoded)) ret = false;
                    else ret = true;
                }
                catch (Exception ex) 
                { 
                    ret = false;
                    sDecoded = $"CodeRead Error : {ex.Message}";
                }

                if (CodeReader != null) { CodeReader.Dispose(); CodeReader = null; }
                if (Result != null)
                {
                    int iLength = Result.Length;
                    for (int i = 0; i < iLength; i++){Result[i].Dispose(); Result = null;}
                }
            }
            else
            {
                Euresys.Open_eVision_22_04.EMatrixCodeReader CodeReader = new Euresys.Open_eVision_22_04.EMatrixCodeReader(); // EMatrixCodeReader instance
                Euresys.Open_eVision_22_04.EMatrixCode Result = null;
                CodeReader.TimeOut = iTimeOut;

                try
                {
                    if (cContextPath != string.Empty)
                        CodeReader.Load(cContextPath);

                    if (mSrcID.FirstSubROI == null)
                        Result = CodeReader.Read(mSrcID);
                    else
                        Result = CodeReader.Read(mSrcID.FirstSubROI);

                    sDecoded = Result.DecodedString;

                    if (string.IsNullOrEmpty(sDecoded)) ret = false;
                    else ret = true;
                }
                catch (Exception ex)
                {
                    if (m_bAutoLearn) mCodeReader1.LearnMore(mSrcID);

                    ret = false;
                    sDecoded = $"CodeRead Error : {ex.Message}";
                }

                if (CodeReader != null) { CodeReader.Dispose(); CodeReader = null; }
                if (Result     != null) { Result.Dispose();     Result     = null; }
            }

            return ret;
		}

        public Euresys.Open_eVision_22_04.EMatrixCodeReader mCodeReader1;
        public Euresys.Open_eVision_22_04.EasyMatrixCode2.EMatrixCodeReader mCodeReader2;

        internal bool CodeRead(EImageBW8 mSrcID, EImageBW8 mDstID, CODE_MODE nMode, uint iTimeOut, out string sDecoded)
        {
            //Open_eVision_2_17?? 2가 빠른지 1이 빠른지 상황에 따라 다름
            bool ret = false;
            sDecoded = string.Empty;

            if (miType == 0) //EasyMatrixCode2
            {
                if (mCodeReader2 == null) mCodeReader2 = new Euresys.Open_eVision_22_04.EasyMatrixCode2.EMatrixCodeReader();
                
                Euresys.Open_eVision_22_04.EasyMatrixCode2.EMatrixCode[] Result = null;
                mCodeReader2.TimeOut = iTimeOut;
                mCodeReader2.ReadMode = nMode == CODE_MODE.E_CODE_SPEED ? EReadMode.Speed : EReadMode.Quality;
                mCodeReader2.MaxNumCodes = 1;

                try
                {
                    if (mSrcID.FirstSubROI == null)
                        mCodeReader2.Read(mSrcID);
                    else
                        mCodeReader2.Read(mSrcID.FirstSubROI);

                    Result = mCodeReader2.ReadResults;
                    sDecoded = Result[0].DecodedString;

                    if (string.IsNullOrEmpty(sDecoded)) ret = false;
                    else ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                    sDecoded = $"CodeRead Error : {ex.Message}";
                }

                if (Result != null)
                {
                    int iLength = Result.Length;
                    for (int i = 0; i < iLength; i++) { Result[i].Dispose(); Result = null; }
                }
            }
            else
            {
                if (mCodeReader1 == null) mCodeReader1 = new Euresys.Open_eVision_22_04.EMatrixCodeReader();

                Euresys.Open_eVision_22_04.EMatrixCode Result = null;
                mCodeReader1.TimeOut = iTimeOut;

                try
                {
                 
                    if (mSrcID.FirstSubROI == null)
                        Result = mCodeReader1.Read(mSrcID);
                    else
                        Result = mCodeReader1.Read(mSrcID.FirstSubROI);

                    sDecoded = Result.DecodedString;

                    if (string.IsNullOrEmpty(sDecoded)) ret = false;
                    else ret = true;
                }
                catch (Exception ex)
                {
                    if (m_bAutoLearn) mCodeReader1.LearnMore(mSrcID);

                    ret = false;
                    sDecoded = $"CodeRead Error : {ex.Message}";
                }

                if (Result != null) { Result.Dispose(); Result = null; }
            }

            return ret;
        }

        internal bool CodeLoad(string sPath)
        {
            //Open_eVision_2_17?? 2가 빠른지 1이 빠른지 상황에 따라 다름
            bool ret = false;

            try
            {
                if (miType == 0) //EasyMatrixCode2
                    mCodeReader2.Load(sPath);
                else
                    mCodeReader1.Load(sPath);
            }
            catch 
            {
                ret = false;
            }
           

            return ret;
        }

        internal bool CodeSave(string sPath)
        {
            //Open_eVision_2_17?? 2가 빠른지 1이 빠른지 상황에 따라 다름
            bool ret = false;

            try
            {
                if (miType == 0) //EasyMatrixCode2
                    mCodeReader2.Save(sPath);
                else
                    mCodeReader1.Save(sPath);
            }
            catch
            {
                ret = false;
            }


            return ret;
        }

        internal bool Dispose()
        {
            if (mCodeReader1 != null) mCodeReader1.Dispose();
            if (mCodeReader2 != null) mCodeReader1.Dispose();

            return true;
        }

    }
}
