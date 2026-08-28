using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    public partial class TLightSource
    {
        #region << Enums >>
        public enum enumLvsMode
        {
            READ = 0,
            WRITE
        };

        public enum enumLvsCmd : int
        {
            CmdNone,
            SetPeriodTime,
            SetHighTime,
            SetOnTime,
            SetDelayTime,
            SetTriggerMode,
            SetBaudrate,
            SetMeasureFrameCnt,
            SetFrameChannelValue,
            SetLiveStart,
            SetMeasureStart,
            SetStop,
            GetPeriodTime,
            GetHighTime,
            GetOnTime,
            GetDelayTime,
            GetTriggerMode,
            GetBaudrate,
            GetMeasureFrameCnt,
            GetFrameChannelValue,
            GetVersion,
        };

        #endregion

        #region << Structures >>
        public struct ST_LVS_PARA_BUFF
        {
            public int iFrame;
            public int iCh;
            public enumLvsCmd iCmd;
            public int iMode;
            public int iPara;
        };
        #endregion

        #region << Fields >>
        private ST_LVS_PARA_BUFF m_TxParas;
        private Queue<ST_LVS_PARA_BUFF> m_ParaQue = new Queue<ST_LVS_PARA_BUFF>();

        #endregion

        #region << Methods >>
        public bool SendMsgLvs()
        {
            string sWrData = "";
            byte[] szTxBuff;
            //Check Port.
            if (RS232 == null) { return false; }
            if (!RS232._IsOpen) { return false; }

            switch (m_TxParas.iCmd)
            {
                default: return false;
                case enumLvsCmd.SetPeriodTime:          sWrData = string.Format("setept {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetHighTime:            sWrData = string.Format("seteht {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetOnTime:              sWrData = string.Format("setlot {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetDelayTime:           sWrData = string.Format("setldt {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetTriggerMode:         sWrData = string.Format("settrg {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetBaudrate:            sWrData = string.Format("setbdr {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetMeasureFrameCnt:     sWrData = string.Format("setcnt {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetFrameChannelValue:   sWrData = string.Format("setled {0} {1} {2}\r\n", m_TxParas.iFrame, m_TxParas.iCh, m_TxParas.iPara); break;
                case enumLvsCmd.SetLiveStart:           sWrData = string.Format("lstart {0}\r\n", m_TxParas.iPara); break;
                case enumLvsCmd.SetMeasureStart:        sWrData = string.Format("mstart\r\n"); break;
                case enumLvsCmd.SetStop:                sWrData = string.Format("stop\r\n"); break;
                case enumLvsCmd.GetPeriodTime:          sWrData = string.Format("getept\r\n"); break;
                case enumLvsCmd.GetHighTime:            sWrData = string.Format("geteht\r\n"); break;
                case enumLvsCmd.GetOnTime:              sWrData = string.Format("getlot\r\n"); break;
                case enumLvsCmd.GetDelayTime:           sWrData = string.Format("getldt\r\n"); break;
                case enumLvsCmd.GetTriggerMode:         sWrData = string.Format("gettrg\r\n"); break;
                case enumLvsCmd.GetBaudrate:            sWrData = string.Format("getbdr\r\n"); break;
                case enumLvsCmd.GetMeasureFrameCnt:     sWrData = string.Format("getcnt\r\n"); break;
                case enumLvsCmd.GetFrameChannelValue:   sWrData = string.Format("getled {0} {1}\r\n", m_TxParas.iFrame, m_TxParas.iCh); break;
                case enumLvsCmd.GetVersion:             sWrData = string.Format("getver\r\n"); break;
            }


            szTxBuff = FNC.GetStringToByteArray(sWrData);

            m_bDrngComm = true;
            //Write Data.
            bool bRet = RS232.SendByte(szTxBuff, sWrData.Length);
            //Return.
            return bRet;

        }


        private void SendCommand(enumLvsMode iMode, enumLvsCmd Cmd, int nFrame, int nCh, int nValue)
        {
            ST_LVS_PARA_BUFF m_TmpPara = new ST_LVS_PARA_BUFF();
            m_TmpPara.iMode = (int)iMode;
            m_TmpPara.iCmd = Cmd;
            m_TmpPara.iFrame = nFrame;
            m_TmpPara.iCh = nCh;
            m_TmpPara.iPara = nValue;
            m_ParaQue.Enqueue(m_TmpPara); //끝부분에 데이타 추가
        }

        private void UpdateLvs()
        {
            //Local Var.
            //Update.
            if (m_tSendTimer.OnDelay((m_iSendStep != 0 || m_bDrngComm), 2000))
            {
                Reset();
                m_bDrngComm = false;
            }

            //Message Process..
            switch (m_iSendStep)
            {
                case 0:
                    if (m_ParaQue.Count <= 0) { m_iSendStep = 0; break; }
                    m_TxParas = m_ParaQue.Dequeue();
                    m_iSendStep++;
                    break;

                case 1:
                    if (!SendMsgLvs()) break;
                    m_tSendDelay.Clear();
                    m_iSendStep++;
                    break;

                case 2:
                    if (!m_tSendDelay.OnDelay(true, 100)) break;
                    if (m_bDrngComm) break;
                    m_tSendTimer.Clear(); //Clear Timer.
                    m_iSendStep = 0;
                    break;
            }
        }

        private void SetLightValueLvs(int iCh, int iValue)
        {
            SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetStop, 0, iCh, iCh);
            //if (iCh == 3) 
            //{
            //    SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetFrameChannelValue, 0, 0, 0);
            //    SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetFrameChannelValue, 0, 1, 0);
            //    m_iLightValue[0] = 0;
            //    m_iLightValue[1] = 0;
            //}
            //else
            SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetFrameChannelValue, 0, iCh, iValue);
            SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetLiveStart, 0, iCh, iCh);

            //SendCommand(enumLvsMode.WRITE, enumLvsCmd.GetFrameChannelValue  , 0, iCh, iCh   );

            m_iLightValue[iCh] = iValue;
        }

        private void SetAllLightValueLvs(int iUseCh = -1)
        {
            if (iUseCh < 0) iUseCh = Max_Channel;
            SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetStop, 0, 0, 0);
            for (int i = 0; i < iUseCh; i++)
            {
                SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetFrameChannelValue, 0, i, m_iLightValue[i]);
            }
            SendCommand(enumLvsMode.WRITE, enumLvsCmd.SetLiveStart, 0, 0, 0);
        }

        #endregion

        #region << Events >>
        public void OnReciveLvs(object sender, int len, byte[] data)
        {
            string sRcv; //Received Message.
            string sDat; //Received Data.
            int iPos1, iPos2;


            char[] str = new char[10];
            for (int iz = 0; iz < 10; iz++)
            {
                str[iz] += (char)data[iz];
            }


            m_bDrngComm = false;


            //Chack
            if (RS232 == null) return;
            if (!RS232._IsOpen) return;

            if (m_iRxCnt + len >= Max_BuffSize) { Clear(); return; }
            Array.Copy(data, 0, m_szRxBuff, m_iRxCnt, len);
            m_iRxCnt += len;

            //Set Barcode Data.
            sRcv = FNC.GetByteArrayToString(m_szRxBuff, 0, m_iRxCnt);
            Array.Clear(m_szRxBuff, 0, m_szRxBuff.Length);
            iPos1 = sRcv.IndexOf("\r\n");
            if (iPos1 <= 0) return;

            if (m_TxParas.iMode == (int)enumLvsMode.WRITE)
            {
                //Write Mode
                if (sRcv.IndexOf("@ok") < 0) return;
                if (m_TxParas.iCmd == enumLvsCmd.SetPeriodTime) m_iperiodtime[m_TxParas.iCh] = m_TxParas.iPara;
                if (m_TxParas.iCmd == enumLvsCmd.SetHighTime) m_ihightime[m_TxParas.iCh] = m_TxParas.iPara;
                if (m_TxParas.iCmd == enumLvsCmd.SetOnTime) m_iontime[m_TxParas.iCh] = m_TxParas.iPara;
                if (m_TxParas.iCmd == enumLvsCmd.SetDelayTime) m_idelaytime[m_TxParas.iCh] = m_TxParas.iPara;
                if (m_TxParas.iCmd == enumLvsCmd.SetTriggerMode) m_itriggermode[m_TxParas.iCh] = m_TxParas.iPara;
                if (m_TxParas.iCmd == enumLvsCmd.SetMeasureFrameCnt) m_imeasureframenum[m_TxParas.iCh] = m_TxParas.iPara;
                if (m_TxParas.iCmd == enumLvsCmd.SetFrameChannelValue) m_iLightValue[m_TxParas.iCh] = m_TxParas.iPara;

                return;
            }

            //Read Mode
            if (sRcv.IndexOf("@err") > 0) return;

            sDat = sRcv.Substring(iPos1 + 2, sRcv.Length);
            iPos2 = sDat.IndexOf("\r\n");

            if (iPos2 > 0)
            {//2°³ ÀÌ»ó ÀÐÈù°æ¿ì
                sDat = sRcv.Substring(iPos1 + 3, iPos2 - 1).Trim();
            }
            else
            {
                sDat = sRcv.Substring(1, iPos1 - 1).Trim();
            }

            if (m_TxParas.iCmd == enumLvsCmd.GetPeriodTime) m_iperiodtime[m_TxParas.iCh] = Convert.ToInt32(sDat);
            if (m_TxParas.iCmd == enumLvsCmd.GetHighTime) m_ihightime[m_TxParas.iCh] = Convert.ToInt32(sDat);
            if (m_TxParas.iCmd == enumLvsCmd.GetOnTime) m_iontime[m_TxParas.iCh] = Convert.ToInt32(sDat);
            if (m_TxParas.iCmd == enumLvsCmd.GetDelayTime) m_idelaytime[m_TxParas.iCh] = Convert.ToInt32(sDat);
            if (m_TxParas.iCmd == enumLvsCmd.GetTriggerMode) m_itriggermode[m_TxParas.iCh] = Convert.ToInt32(sDat);
            if (m_TxParas.iCmd == enumLvsCmd.GetMeasureFrameCnt) m_imeasureframenum[m_TxParas.iCh] = Convert.ToInt32(sDat);
            if (m_TxParas.iCmd == enumLvsCmd.GetFrameChannelValue) m_iLightValue[m_TxParas.iCh] = Convert.ToInt32(sDat);

        }

        #endregion
    }
}
