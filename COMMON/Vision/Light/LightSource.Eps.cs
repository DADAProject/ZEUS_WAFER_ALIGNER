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
        public enum enumEpsMode
        {
            READ = 0,
            WRITE
        };

        public enum enumEpsCmd : byte
        {
            CmdNone,
            SetBaudrate,
            SetFrameChannelValue,
            SetLiveStart,
            SetStop,
            GetFrameChannelValue,
            GetVersion,
        };

        #endregion

        #region << Structures >>
        public struct ST_EPS_PARA_BUFF
        {
            public int iFrame;
            public int iCh;
            public enumEpsCmd iCmd;
            public int iMode;
            public int iPara;
        };
        #endregion

        #region << Const >>

        private static readonly int Max_Channel_Eps = 4;

        #endregion

        #region << Fields >>
        private ST_EPS_PARA_BUFF m_TxEpsParas;
        private Queue<ST_EPS_PARA_BUFF> m_EpsParaQue = new Queue<ST_EPS_PARA_BUFF>();

        #endregion

        #region << Methods >>

        private void InitEps()
        {
            m_iLightValue       = new int[Max_Channel_Eps];
            m_iperiodtime       = new int[Max_Channel_Eps];
            m_ihightime         = new int[Max_Channel_Eps];
            m_iontime           = new int[Max_Channel_Eps];
            m_idelaytime        = new int[Max_Channel_Eps];
            m_itriggermode      = new int[Max_Channel_Eps];
            m_imeasureframenum  = new int[Max_Channel_Eps];

            m_iLightOnOff       = new int[Max_Channel_Eps];
        }

        public bool SendMsgEps()
        {
            string sWrData = "";
            byte[] szTxBuff;
            //Check Port.
            if (RS232 == null) { return false; }
            if (!RS232._IsOpen) { return false; }

            switch (m_TxEpsParas.iCmd)
            {
                default: return false;
                case enumEpsCmd.SetFrameChannelValue:   sWrData = string.Format("{0}{1}{2}{3}\r\n", ':', 'L', m_TxEpsParas.iCh + 1, m_TxEpsParas.iPara.ToString("D3")); break;
                case enumEpsCmd.GetFrameChannelValue:   sWrData = string.Format("{0}{1}{2}{3}\r\n", ':', 'R', m_TxEpsParas.iCh + 1, m_TxEpsParas.iPara);                break;
                case enumEpsCmd.SetLiveStart:           sWrData = string.Format("{0}{1}{2}\r\n"   , ':', 'O', m_TxEpsParas.iCh + 1); break;
                case enumEpsCmd.SetStop:                sWrData = string.Format("{0}{1}{2}\r\n"   , ':', 'F', m_TxEpsParas.iCh + 1); break;
            }

            szTxBuff = FNC.GetStringToByteArray(sWrData);

            m_bDrngComm = true;
            //Write Data.
            bool bRet = RS232.SendByte(szTxBuff, sWrData.Length);
            //Return.
            return bRet;

        }


        private void SendCommandEps(enumEpsMode Mod,enumEpsCmd Cmd, int nFrame, int nCh, int nPara)
        {
            ST_EPS_PARA_BUFF m_TmpPara = new ST_EPS_PARA_BUFF();
            m_TmpPara.iCmd      = Cmd;
            m_TmpPara.iFrame    = nFrame;
            m_TmpPara.iCh       = nCh;
            m_TmpPara.iPara     = nPara;
            m_TmpPara.iMode     = (int)Mod;
            m_EpsParaQue.Enqueue(m_TmpPara); //끝부분에 데이타 추가
        }

        private void UpdateEps()
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
                    if (m_EpsParaQue.Count <= 0) { m_iSendStep = 0; break; }
                    m_TxEpsParas = m_EpsParaQue.Dequeue();
                    m_iSendStep++;
                    break;

                case 1:
                    if (!SendMsgEps()) break;
                    m_tSendDelay.Clear();
                    m_iSendStep++;
                    break;

                case 2:
                    if (!m_tSendDelay.OnDelay(true, 50)) break;
                    if (m_bDrngComm) break;
                    m_tSendTimer.Clear(); //Clear Timer.
                    m_iSendStep = 0;
                    break;
            }
        }
        private void SetLightOnEps(int iCh, int iOnOff)
        {
            if(iOnOff > 0)
                SendCommandEps(enumEpsMode.WRITE, enumEpsCmd.SetLiveStart, 0, iCh, iOnOff);
            else
                SendCommandEps(enumEpsMode.WRITE, enumEpsCmd.SetStop, 0, iCh, iOnOff);      
        }

        private void SetLightValueEps(int iCh, int iValue)
         {
            SendCommandEps(enumEpsMode.WRITE, enumEpsCmd.SetFrameChannelValue, 0, iCh, iValue);
           
            SendCommandEps(enumEpsMode.READ, enumEpsCmd.GetFrameChannelValue, 0, iCh, 1); //채널 1개만 요청

             //m_iLightValue[iCh] = iValue;
        }

        private void SetAllLightValueEps(int iUseCh = -1)
        {
            if (iUseCh < 0) iUseCh = Max_Channel;

            for (int i = 0; i < iUseCh; i++)
            {
                SendCommandEps(enumEpsMode.WRITE, enumEpsCmd.SetFrameChannelValue, 0, i, m_iLightValue[i]);
            }
        }

        #endregion

        #region << Events >>
        public void OnReciveEps(object sender, int len, byte[] data)
        {
            string sRcv; //Received Message.
            string sDat = ""; //Received Data.
            int iPos1;//, iPos2;


            char[] str = new char[10];
            for (int iz = 0; iz < 10; iz++)
            {
                str[iz] += (char)data[iz];
            }

            //Chack
            if (RS232 == null) return;
            if (!RS232._IsOpen) return;

            if (len <= 0) return;
            if (len >= Max_BuffSize) { Clear(); return; }
            Array.Copy(data, 0, m_szRxBuff, 0, len);

            //Set Barcode Data.
            sRcv = FNC.GetByteArrayToString(m_szRxBuff, 0, len);
            Array.Clear(m_szRxBuff, 0, m_szRxBuff.Length);
            m_bDrngComm = false;

            iPos1 = sRcv.IndexOf("\r\n");

            //Write Mode
            if (m_TxEpsParas.iMode == (int)enumEpsMode.WRITE)
            {
                if (sRcv.IndexOf("\u0006") < 0) return;
                if (m_TxEpsParas.iCmd == enumEpsCmd.SetFrameChannelValue)   m_iLightValue[m_TxEpsParas.iCh] = m_TxEpsParas.iPara;
                //if (m_TxEpsParas.iCmd == enumEpsCmd.SetLiveStart)           m_iLightOnOff[m_TxEpsParas.iCh] = 1;
                //if (m_TxEpsParas.iCmd == enumEpsCmd.SetStop)                m_iLightOnOff[m_TxEpsParas.iCh] = 0;
                
                return;
            }
            //Read Mode

            if (m_TxEpsParas.iMode == (int)enumEpsMode.READ)
            {
                if (sRcv.IndexOf("\u0021") > 0) return;
                sDat = sRcv.Substring(sRcv.IndexOf('R') + 2, 3);
                //iPos2 = sDat.IndexOf("\r\n");

                //if (iPos2 > 0)
                //{//2°³ ÀÌ»ó ÀÐÈù°æ¿ì
                //    sDat = sRcv.Substring(iPos1 + 3, iPos2 - 1).Trim();
                //}
                //else
                //{
                //    sDat = sRcv.Substring(1, iPos1 - 1).Trim();
                //  }
                if (m_TxEpsParas.iCmd == enumEpsCmd.GetFrameChannelValue) m_iLightValue[m_TxParas.iCh] = Convert.ToInt32(sDat);
                return;
            }
        }

        #endregion
    }
}
