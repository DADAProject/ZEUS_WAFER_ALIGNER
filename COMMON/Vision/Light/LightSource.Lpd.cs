using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    //LEESOS Light Control
    public partial class TLightSource
    {
        #region << Enums >>
        public enum enumLpdMode
        {
            READ = 0,
            WRITE
        };

        public enum enumLpdCmd : byte
        {
            CmdNone             ,
            SetFrameChannelValue,
            SetAllChannelValue  ,
            SetLiveStart        ,
            SetStop             ,
            //GetFrameChannelValue,
            //GetLiveState        ,
            //GetErr              ,
            GetStat             ,
        };
        #endregion

        #region << Structures >>
        public struct ST_LPD_PARA_BUFF
        {
            public int iFrame       ;
            public int iCh          ;
            public enumLpdCmd iCmd  ; 
            public int iMode        ;
            public int iPara        ;
        };
        #endregion

        #region << Const >>
        private static readonly int Max_Channel_Lpd = 8;
        #endregion

        #region << Fields >>
        private ST_LPD_PARA_BUFF m_TxLpdParas;
        private ST_LPD_PARA_BUFF m_TxLpdParasBuf;
        private Queue<ST_LPD_PARA_BUFF> m_LpdParaQue = new Queue<ST_LPD_PARA_BUFF>();
        

        #endregion

        #region << Methods >>

        private void InitLpd()
        {
            m_iLightValue      = new int[Max_Channel_Lpd];
            m_iperiodtime      = new int[Max_Channel_Lpd];
            m_ihightime        = new int[Max_Channel_Lpd];
            m_iontime          = new int[Max_Channel_Lpd];
            m_idelaytime       = new int[Max_Channel_Lpd];
            m_itriggermode     = new int[Max_Channel_Lpd];
            m_imeasureframenum = new int[Max_Channel_Lpd];
            m_iStatError       = new int[Max_Channel_Lpd];
            m_iLightOnOff      = new int[Max_Channel_Lpd];
        }

        public bool SendMsgLpd()
        {
            string sWrData = "";
            byte[] szTxBuff;
            //Check Port.
            if ( RS232 == null) { return false; }
            if (!RS232._IsOpen) { return false; }

            switch (m_TxLpdParas.iCmd)
            {
                default: return false;
                case enumLpdCmd.SetFrameChannelValue: sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'C', m_TxLpdParas.iCh + 1, m_TxLpdParas.iPara.ToString("X3")); break;
                case enumLpdCmd.SetAllChannelValue  : sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'C', 'T'                 , m_TxLpdParas.iPara.ToString("X3")); break;
                case enumLpdCmd.SetLiveStart        : sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'H', m_TxLpdParas.iCh + 1, "ON"                             ); break;
                case enumLpdCmd.SetStop             : sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'H', m_TxLpdParas.iCh + 1, "OF"                             ); break;
                case enumLpdCmd.GetStat             : sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'S', m_TxLpdParas.iCh + 1, m_TxLpdParas.iPara.ToString("D2")); break;
                //case enumLpdCmd.GetFrameChannelValue: sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'S', m_TxLpdParas.iCh + 1, "00"                           ); break;
                //case enumLpdCmd.GetLiveState        : sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'S', m_TxLpdParas.iCh + 1, "01"                           ); break;
                //case enumLpdCmd.GetErr              : sWrData = string.Format("{0}{1}{2}{3}\r\n", 'L', 'S', m_TxLpdParas.iCh + 1, "02"                           ); break;
            }

            szTxBuff = FNC.GetStringToByteArray(sWrData);

            m_bDrngComm = true;
            //Write Data.
            bool bRet = RS232.SendByte(szTxBuff, sWrData.Length);
            //Return.
            return bRet;
        }


        private void SendCommandLpd(enumLpdMode Mod, enumLpdCmd Cmd, int nFrame, int nCh, int nPara)
        {
            ST_LPD_PARA_BUFF m_TmpPara = new ST_LPD_PARA_BUFF();
            m_TmpPara.iCmd = Cmd;
            m_TmpPara.iFrame = nFrame;
            m_TmpPara.iCh = nCh;
            m_TmpPara.iPara = nPara;
            m_TmpPara.iMode = (int)Mod;
            m_LpdParaQue.Enqueue(m_TmpPara); //끝부분에 데이타 추가
        }

        private void UpdateLpd()
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
                    if (m_LpdParaQue.Count <= 0) { m_iSendStep = 0; break; }
                    //m_LpdParaQue.Clear();
                    m_TxLpdParas = m_LpdParaQue.Dequeue();
                    m_iSendStep++;
                    break;

                case 1:
                    if (!SendMsgLpd()) break;
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
            //
            m_tDelay.OnDelay((m_LpdParaQue.Count <= 0) && !cDEF.SEQ._bRun, 3000);
            if (m_tDelay.Out)
            {
                //GetLightValueLpd(0);
                //for (int n = 0; n < Max_Channel_Lpd; n++)
                //{
                //    //GetLightValueLpd(n);
                //    //GetLightOnLpd(n);
                //    //GetLightErr(n);
                //}
                m_tDelay.Clear();
            }
        }
        private void SetLightOnLpd(int iCh, int iOnOff)
        {
            if (iOnOff > 0)
                SendCommandLpd(enumLpdMode.WRITE, enumLpdCmd.SetLiveStart, 0, iCh, iOnOff);
            else
                SendCommandLpd(enumLpdMode.WRITE, enumLpdCmd.SetStop, 0, iCh, iOnOff);
            //
            //SendCommandLpd(enumLpdMode.READ, enumLpdCmd.GetStat, 0, iCh, 0);
            //SendCommandLpd(enumLpdMode.READ, enumLpdCmd.GetStat, 0, iCh, 1);
            //SendCommandLpd(enumLpdMode.READ, enumLpdCmd.GetStat, 0, iCh, 2);
        }

        private void SetLightValueLpd(int iCh, int iValue)
        {
            SendCommandLpd(enumLpdMode.WRITE, enumLpdCmd.SetFrameChannelValue, 0, iCh, iValue);
            //
            //SendCommandLpd(enumLpdMode.READ, enumLpdCmd.GetStat, 0, iCh, 0);
        }


        private void SetAllLightValueLpd(int iValue)
        {
            SendCommandLpd(enumLpdMode.WRITE, enumLpdCmd.SetAllChannelValue, 0, 0, iValue);
        }
        private void GetLightValueLpd(int iCh)
        {
            SendCommandLpd(enumLpdMode.READ, enumLpdCmd.GetStat, 0, iCh, 0);
        }
        private void GetLightOnLpd(int iCh)
        {
            SendCommandLpd(enumLpdMode.READ, enumLpdCmd.GetStat, 0, iCh, 1);
        }

        private void GetLightErr(int iCh)
        {
            SendCommandLpd(enumLpdMode.READ, enumLpdCmd.GetStat, 0, iCh, 2);
        }
        #endregion

        #region << Events >>
        public void OnReciveLpd(object sender, int len, byte[] data)
        {
            string sRcv; //Received Message.
            string sDat = ""; //Received Data.
            int iPos1;

            //Check
            if ( RS232 == null) return;
            if (!RS232._IsOpen) return;

            if (len <= 0) return;
            if (len >= Max_BuffSize) { Clear(); return; }
            Array.Copy(data, 0, m_szRxBuff, 0, len);

            //Set Light Data.
            sRcv = FNC.GetByteArrayToString(m_szRxBuff, 0, len);
            Array.Clear(m_szRxBuff, 0, m_szRxBuff.Length);
            iPos1 = sRcv.IndexOf("\r\n");
            //
            m_TxLpdParasBuf = m_TxLpdParas;
            m_bDrngComm = false;            

            //Write Mode
            if (m_TxLpdParasBuf.iMode == (int)enumLpdMode.WRITE)
            {
                // R1FFF[CR][LF],
                // RTFFF[CR][LF]
                // R1OK[CR][LF],
                // R1ER[CR][LF]

                if (sRcv.Contains("ER")) return;
           
                if (m_TxLpdParasBuf.iCmd == enumLpdCmd.SetFrameChannelValue) m_iLightValue[m_TxLpdParasBuf.iCh] = m_TxLpdParasBuf.iPara;
                if (m_TxLpdParasBuf.iCmd == enumLpdCmd.SetAllChannelValue  ) m_iLightValue[m_TxLpdParasBuf.iCh] = m_TxLpdParasBuf.iPara;
                if (m_TxLpdParasBuf.iCmd == enumLpdCmd.SetLiveStart        ) m_iLightOnOff[m_TxLpdParasBuf.iCh] = 1;
                if (m_TxLpdParasBuf.iCmd == enumLpdCmd.SetStop             ) m_iLightOnOff[m_TxLpdParasBuf.iCh] = 0;

                return;
            }

            //Read Mode
            if (m_TxLpdParas.iMode == (int)enumLpdMode.READ)
            {
                // LS101[CR][LF]
                // R1ON[CR][LF], R
                sDat = sRcv.Substring(2, sRcv.Length - 2);
                if (m_TxLpdParasBuf.iCmd == enumLpdCmd.GetStat)
                {
                    if      (m_TxLpdParasBuf.iPara == 0) m_iLightValue[m_TxLpdParasBuf.iCh] = int.Parse(sDat, System.Globalization.NumberStyles.HexNumber);
                    else if (m_TxLpdParasBuf.iPara == 1) m_iLightOnOff[m_TxLpdParasBuf.iCh] = sDat == "ON" ? 1 : 0;
                    else if (m_TxLpdParasBuf.iPara == 2) m_iStatError [m_TxLpdParasBuf.iCh] = sDat == "OK" ? 0 : 1;
                }

                //if (m_TxLpdParasBuf.iCmd == enumLpdCmd.GetFrameChannelValue) m_iLightValue[m_TxLpdParas.iCh] = int.Parse(sDat, System.Globalization.NumberStyles.HexNumber);
                //if (m_TxLpdParasBuf.iCmd == enumLpdCmd.GetLiveState        ) m_iLightOnOff[m_TxLpdParas.iCh] = sDat == "ON" ? 1 : 0;

                return;
            }
        }

        #endregion
    }
}
