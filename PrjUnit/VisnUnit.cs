/*
 * NUL's Class Libraries
 * by NUL
 * copyright JC Soft Lab. 2018, all rights reserved
 */
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace eMachine
{
    public enum EN_VISN_TYPE : int
    {
        None = -1,
        WAlgn, //Wafer Align
        FAlgn, //Frame Align

        EndOfId
    }
    public enum EN_VISN_STEP : int
    {
        None = -1  ,
        ALIGN = 0  , //Wafer Align
        ALIGN_RETRY,
        FAIL_RETRY ,
        VERIFY     , //
        MANUAL     , //Manual 

        EndOfId
    }


    public enum EN_USER_DIR : int
    {
        None = -1,
        Left_Btm = 0,
        Left_Top = 1,
        Rght_Btm = 2,
        Rght_Top = 3,

        EndofId

    }
    /***************************************************************************/
    /* Class: VSN                                                              */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/


    public class TVisnUnit
    {
        public static string Path = Application.StartupPath + "\\Vision\\";

        public static double Resoultion = 111.3172;
        public static double Distortion = -1.37;

        // 카메라 배열
        //int          m_iCamCount  ;
        //int          m_iStepLast  ;

        //
        public TCamItem[] Cam       = new TCamItem    [(int)EN_CAM.EndofCam];
        public TLightSource[] Light = new TLightSource[(int)EN_CAM.EndofCam];
        //
        TOnDelayTimer[] m_tStartOn = new TOnDelayTimer[(int)EN_CAM.EndofCam];
        TOnDelayTimer[] m_tResetOn = new TOnDelayTimer[(int)EN_CAM.EndofCam];

        //                                  
        TOnDelayTimer m_tCyleMain = new TOnDelayTimer(); //Main Cycle Timer.
        TOnDelayTimer m_tCyleWait = new TOnDelayTimer(); //
        TOnDelayTimer m_tTemp = new TOnDelayTimer(); //Temp. Timer.
        TOnDelayTimer m_tWait = new TOnDelayTimer(); //
        TOnDelayTimer m_tLightDly = new TOnDelayTimer();

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TVisnUnit()
        {

        }
        ~TVisnUnit() { }

        public bool Init()
        {
            //
            for (int i = 0; i < (int)EN_CAM.EndofCam; i++)
            {
                try
                {
                    Cam[i] = new TCamItem((EN_CAM)i);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Visn Init Error : {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Visn Init Error : {ex.Message}");
                }
            }

            Light[(int)EN_CAM.WTB] = new TLightSource();
            Light[(int)EN_CAM.WTB].Init(TLightSource.LightSourceType.Lfn, cDEF.FM.EngrOptn.sCom_Light);

            return true;
        }


        //-----------------------------------------------------------------------
        public void ApplyProject(bool isLoad, string DevName)
        {
            Load(isLoad, DevName);

            if (!isLoad) cDEF.FM.ApplyProject(cDEF.FM._sCrntDevice);
        }

        //-----------------------------------------------------------------------
        public void Final()
        {
            try
            {
                for (int i = 0; i < (int)EN_CAM.EndofCam; i++)
                {
                    if (Cam[i] != null) Cam[i].Final();
                }

                Light[(int)EN_CAM.WTB].Close();
            }
            catch (Exception e)
            {
                cDEF.LOG.ExceptionTrace("VisnUnit. Final()" + e.ToString());
            }
        }
        //-----------------------------------------------------------------------
        public bool Reset()
        {
            //
            SetMCStatus(2, true);
            //
            for (int i = 0; i < (int)EN_CAM.EndofCam; i++)
            {
                StartVisn(i, false);
            }
            //
            return true;
        }
        //-----------------------------------------------------------------------
        public bool StartVisn(int Visn, bool On)
        {
            EN_OUT_ID iAddr = EN_OUT_ID.yNone + Visn;
            //
            cDEF.IO.sY(iAddr, On);
            return cDEF.IO.gY(iAddr);
        }
        //-----------------------------------------------------------------------
        public bool GetMCReset()
        {
            EN_OUT_ID iAddr = EN_OUT_ID.yNone;
            //
            return cDEF.IO.gY(iAddr);
        }
        //-----------------------------------------------------------------------
        public bool GetStart(int Visn)
        {
            EN_OUT_ID iAddr = (EN_OUT_ID)((int)EN_OUT_ID.yNone + Visn);
            //
            return cDEF.IO.gY(iAddr);
        }
        //-----------------------------------------------------------------------
        public bool GetReady(int Visn, bool Dry, bool On)
        {
            EN_IN_ID iAddr = EN_IN_ID.xNone + Visn;
            //
            if (Dry == true) return On;
            //
            return cDEF.IO.gX(iAddr);
        }
        //-----------------------------------------------------------------------
        public bool GetBusy(int Visn, bool Dry, bool On)
        {
            EN_IN_ID iAddr = EN_IN_ID.xNone + Visn;
            //
            if (Dry == true) return On;
            //
            bool rslt = cDEF.IO.gX(iAddr);
            return rslt;
        }
        //-----------------------------------------------------------------------
        public bool SetMCStatus(int Type, bool On)
        {
            //0 : Start, 1 : Stop, 2 : Reset
            switch (Type)
            {
                case 0: cDEF.IO.sY(EN_OUT_ID.yNone, On); break;
                case 1: cDEF.IO.sY(EN_OUT_ID.yNone, On); break;
                case 2: cDEF.IO.sY(EN_OUT_ID.yNone, On); break;
            }

            return true;
        }
        //-----------------------------------------------------------------------
        public bool UseVisn(int CamId)
        {
            if (!Cam[CamId].bUseCam) return false;

            return true;
        }
        //-----------------------------------------------------------------------
        public void UpdateVisnIO(bool Run)
        {
            bool isStrtOn = false;

            //Kill the vision start signal.
            if ((cDEF.MAN._iManNo < 0) && !Run)
            {
                for (int v = 0; v < (int)EN_CAM.EndofCam; v++)
                {
                    isStrtOn = GetStart(v);

                    if (isStrtOn) StartVisn(v, false);
                }
            }

            //Switch Off Vision Reset Signal.
            for (int v = 0; v < (int)EN_CAM.EndofCam; v++)
            {
                if (m_tResetOn[v].OnDelay(GetMCReset(), 1000))
                {
                    SetMCStatus(2, false);
                }
            }
            //
            if (cDEF.SEQ._bRun)
            {
                SetMCStatus(1, false); //Stop
                SetMCStatus(0, true); //Run
            }
            else
            {
                SetMCStatus(1, true);
                SetMCStatus(0, false);
            }
        }


        public void Update()
        {
            if (Light[(int)EN_CAM.WTB] != null) Light[(int)EN_CAM.WTB].Update();
        }

        public void UpdateLog()
        {
            for (int i = 0; i < (int)EN_CAM.EndofCam; i++)
            {
                if (Cam[i] != null) Cam[i].Update();
            }
        }

        //-----------------------------------------------------------------------
        public bool IsInspctVisn(int CamId)
        {
            //if ((CamId == (int)EN_CAM.BTV) || (CamId == (int)EN_CAM.TPV)) return true;

            return false;
        }
        //-----------------------------------------------------------------------

        public TVisnRslt[] GetVisnRslt(int CamId)
        {
            return Cam[CamId].m_VisnRslt;
        }
        //-----------------------------------------------------------------------
        public bool VisnGrabStrt(EN_CAM CamId, EN_VISN_TYPE Type, string manImgPath = "")
        {
           //
            if (cDEF.FM.SysOptn.iTestMode[(int)CamId] == vDEF.CHCK_AWYS)
                Cam[(int)CamId].OneAlgoStrt(manImgPath);

            return true;
        }
        //-----------------------------------------------------------------------
        public bool VisnimageStrt(EN_CAM CamId, EN_VISN_TYPE Type, string path)
        {
            //
            Cam[(int)CamId].OneAlgoStrt(path);

            for (int type = 0; type < (int)EN_VISN_TYPE.EndOfId; type++)
            {
                TVisnRslt Relt = cDEF.VISN.Cam[(int)EN_CAM.WTB].GetVisnRslt((EN_VISN_TYPE)type);
                string Msg = string.Format($"[{(EN_VISN_TYPE)type} Manual Vision Cycle Result] {Relt.ToString()}");
                FRM.ShowWarn(true, Msg);
            }
          
            return true;
        }
        //-----------------------------------------------------------------------
        public bool SetLightOn(bool On, int CamId, EN_VISN_TYPE Type)
        {
            //int iCh     = (int)EN_LIGHT_CH.BACKLIGHT;
            int iCamId  = (int)CamId;

            string Name          = Cam[(int)CamId].m_SET[(int)Type].sIlluminationName;
            double dLightDelay   = Tillumination.Getillumination(Name).dLightDelay;
            double[] dLightValue = Tillumination.Getillumination(Name).dLightValue;
            bool[] dUseLight     = Tillumination.Getillumination(Name).dUseLight;

            if (dUseLight[(int)EN_LIGHT_CH.BACKLIGHT])
            {
                if (On)
                {
                    if(cDEF.FM.EngrOptn.bUseVisnIO)
                    {
                        for (int iCh = 0; iCh < (int)EN_LIGHT_CH.EndOfCh; iCh++)
                        {
                            cDEF.VISN.Light[(int)EN_CAM.WTB].SetLightValue(iCh, (int)dLightValue[(int)EN_LIGHT_CH.BACKLIGHT]);
                        }
                    }
                    else
                    {
                        for (int iCh = 0; iCh < (int)EN_LIGHT_CH.EndOfCh; iCh++)
                        {
                            cDEF.VISN.Light[(int)EN_CAM.WTB].SetLightValue(iCh, (int)dLightValue[(int)EN_LIGHT_CH.BACKLIGHT]);
                        }
                        for (int iCh = 0; iCh < (int)EN_LIGHT_CH.EndOfCh; iCh++)
                        {
                            cDEF.VISN.Light[(int)EN_CAM.WTB].SetLightOn(iCh, true);
                        }
                    }
                }
                else
                {
                    if(cDEF.FM.EngrOptn.bUseVisnIO)
                    {
                        for (int iCh = 0; iCh < (int)EN_LIGHT_CH.EndOfCh; iCh++)
                        {
                            cDEF.VISN.Light[(int)EN_CAM.WTB].SetLightOn(iCh, false);
                            //cDEF.VISN.Light[(int)EN_CAM.WTB].SetLightValue(iCh, 0);
                        }
                    }
                }
            }
            else
            {
                for (int iCh = 0; iCh < (int)EN_LIGHT_CH.EndOfCh; iCh++)
                {
                    cDEF.VISN.Light[(int)EN_CAM.WTB].SetLightOn(iCh, false);
                }
            }
            return true;
        }
        //-----------------------------------------------------------------------
        public double GetLightDelay(int CamId, EN_VISN_TYPE Type)
        {
            int iCamId = (int)CamId;
            //
            string Name = Cam[(int)CamId].m_SET[(int)Type].sIlluminationName;
            double dRetVal = Tillumination.Getillumination(Name).dLightDelay;
            return dRetVal;
        }
        //-----------------------------------------------------------------------
        public void SetTcpVisnRslt(int CamId, int PatId = 0)
        {
            //if (Cam[CamId].m_CamType != CameraType.TcpIp) return;

            // int count = Cam[CamId].CmmCam.m_RcvRslt.iRsltCount;
            // 
            // Cam[CamId].m_VisnRslt[PatId].Initialize();
            //
            //
            // if (count == 0) return;             
            // //
            // Cam[CamId].m_VisnRslt[PatId] = new TVisnRslt[count];     
            // for (int i = 0; i < count; i++) 
            // {
            //     Cam[CamId].m_VisnRslt[PatId][i] = new TVisnRslt();
            //     Cam[CamId].m_VisnRslt[PatId][i] = Cam[CamId].CmmCam.m_RcvAlgnRslt[i].Copy();
            // }
        }
        //-----------------------------------------------------------------------
        public int GetResultCnt(int CamId, int PatId = 0)
        {
            //if (Cam[CamId].m_CamType == CameraType.TcpIp)
            //{
            //    return Cam[CamId].CmmCam.m_RcvRslt.iRsltCount;
            //}
            //else
            //{
            //    return (int)Cam[CamId].m_Matchs[PatId].NumPositions;
            //}

            //int iCnt = 0;
            //for (int n = 0; n < vDEF.MAX_WAF_INSP_XCNT * vDEF.MAX_WAF_INSP_YCNT; n++)
            //{
            //    if   (Cam[CamId].m_CamType == CameraType.TcpIp)
            //    {
            //        return Cam[CamId].CmmCam.m_RcvRslt.iRsltCount;
            //    }
            //    else
            //    {
            //        if (Cam[CamId].m_VisnRslt[PatId]?[n] == null) continue;
            //        if (Cam[CamId].m_VisnRslt[PatId][n].bMatch) iCnt++;
            //    }
            //}
            //return iCnt;
            return 0;
        }

        #region Functions..
        public Color StatusToColor(VisionStatus vs)
        {
            switch (vs)
            {
                case VisionStatus.Running: break;
                case VisionStatus.Fiducial: break;
                case VisionStatus.Ocr: break;
                case VisionStatus.Align: break;
                case VisionStatus.Mapping: break;
                case VisionStatus.Finished: break;
                case VisionStatus.Error: break;
                case VisionStatus.Timeout: break;
            }
            return Color.Gray;
        }
        #endregion
        //-----------------------------------------------------------------------
        public void Load(bool isLoad, string DevName)
        {
            for (int i = 0; i < (int)EN_CAM.EndofCam; i++)
            {
                LoadParam(isLoad, i, DevName);
            }
        }

        public void LoadParam(bool IsLoad, int iCamNo, string DevName)
        {
            String sPath;
            String sFile = "ProjectVisn";
            String sSection;
            String sName;
            TIniUnit ini = new TIniUnit();
            TSET SET;

            //Make Dir.
            FNC.CreateDirOnWork("Project");
            FNC.CreateDirOnWork("Project\\" + DevName);
            sPath = Application.StartupPath + "\\Project\\" + DevName + "\\" + sFile + ".INI";

            for (int j = 0; j < (int)EN_VISN_TYPE.EndOfId; j++) // vDEF.MAX_VISN_RSLT; j++)
            {
                sSection = string.Format($"{Enum.GetName(typeof(EN_CAM), iCamNo)}_VISN_SET_" +
                                         $"{Enum.GetName(typeof(EN_VISN_TYPE), j)}");

                SET = Cam[iCamNo].m_SET[j];

                if (IsLoad)
                {
                    sName = string.Format("UseVisn          "); ini.Load(sPath, sSection, sName, out SET.bUseVisn);
                    sName = string.Format("XCenterDist      "); ini.Load(sPath, sSection, sName, out SET.dXCenterDist);
                    sName = string.Format("YCenterDist      "); ini.Load(sPath, sSection, sName, out SET.dYCenterDist);

                    sName = string.Format("IlluminationName "); ini.Load(sPath, sSection, sName, out SET.sIlluminationName);
                    for (int idx = 0; idx < SET.sROIName.Length; idx++)
                    { 
                        sName = string.Format($"ROIName{idx}"); ini.Load(sPath, sSection, sName, out SET.sROIName[idx]); 
                    }

                    sName = string.Format("RefName          "); ini.Load(sPath, sSection, sName, out SET.sRefName);


                    for (int idx = 0; idx < SET.dMinSizeTorr.Length; idx++)
                    {
                        sName = string.Format($"MinSizeTorr{idx}"); ini.Load(sPath, sSection, sName, out SET.dMinSizeTorr[idx]);
                    }

                    for (int idx = 0; idx < SET.dMaxSizeTorr.Length; idx++)
                    {
                        sName = string.Format($"MaxSizeTorr{idx}"); ini.Load(sPath, sSection, sName, out SET.dMaxSizeTorr[idx]);
                    }

                    for (int idx = 0; idx < SET.iThreshold.Length; idx++)
                    {
                        sName = string.Format($"Threshold{idx}"); ini.Load(sPath, sSection, sName, out SET.iThreshold[idx]);
                    }

                    for (int idx = 0; idx < SET.iKernel.Length; idx++)
                    {
                        sName = string.Format($"Kernel{idx}"); ini.Load(sPath, sSection, sName, out SET.iKernel[idx]);
                    }

                    for (int idx = 0; idx < SET.iIterable.Length; idx++)
                    {
                        sName = string.Format($"iterable{idx}"); ini.Load(sPath, sSection, sName, out SET.iIterable[idx]);
                    }

                    sName = string.Format("InscribedPoint      "); ini.Load(sPath, sSection, sName, out SET.iInscribedPoint);
                }
                else
                {
                    sName = string.Format("UseVisn          "); ini.Save(sPath, sSection, sName, SET.bUseVisn);
                    sName = string.Format("XCenterDist      "); ini.Save(sPath, sSection, sName, SET.dXCenterDist);
                    sName = string.Format("YCenterDist      "); ini.Save(sPath, sSection, sName, SET.dYCenterDist);

                    sName = string.Format("IlluminationName "); ini.Save(sPath, sSection, sName, SET.sIlluminationName);
                    for (int idx = 0; idx < SET.sROIName.Length; idx++)
                    { 
                        sName = string.Format($"ROIName{idx}"); ini.Save(sPath, sSection, sName, SET.sROIName[idx]);
                    }

                    sName = string.Format("RefName          "); ini.Save(sPath, sSection, sName, SET.sRefName);


                    for (int idx = 0; idx < SET.dMinSizeTorr.Length; idx++)
                    {
                        sName = string.Format($"MinSizeTorr{idx}"); ini.Save(sPath, sSection, sName, SET.dMinSizeTorr[idx]);
                    }

                    for (int idx = 0; idx < SET.dMaxSizeTorr.Length; idx++)
                    {
                        sName = string.Format($"MaxSizeTorr{idx}"); ini.Save(sPath, sSection, sName, SET.dMaxSizeTorr[idx]);
                    }

                    for (int idx = 0; idx < SET.iThreshold.Length; idx++)
                    {
                        sName = string.Format($"Threshold{idx}"); ini.Save(sPath, sSection, sName, SET.iThreshold[idx]);
                    }

                    for (int idx = 0; idx < SET.iKernel.Length; idx++)
                    {
                        sName = string.Format($"Kernel{idx}"); ini.Save(sPath, sSection, sName, SET.iKernel[idx]);
                    }

                    for (int idx = 0; idx < SET.iIterable.Length; idx++)
                    {
                        sName = string.Format($"iterable{idx}"); ini.Save(sPath, sSection, sName, SET.iIterable[idx]);
                    }

                    sName = string.Format("InscribedPoint      "); ini.Save(sPath, sSection, sName, SET.iInscribedPoint);

                }
            }


            ini = null;
        }
        //-----------------------------------------------------------------------


    }

}






