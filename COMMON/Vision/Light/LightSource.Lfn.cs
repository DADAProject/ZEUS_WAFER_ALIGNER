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
        public enum enumLfnMode
        {
            READ = 0,
            WRITE
        };

        public enum enumLfnCmd : byte
        {
            CmdNone             ,
            SetFrameChannelValue,
            SetLiveStart        ,
            SetStop             ,
        };
        #endregion

        #region << Structures >>
        public struct ST_LFN_PARA_BUFF
        {
            public int iFrame       ;
            public int iCh          ;
            public enumLfnCmd iCmd  ; 
            public int iMode        ;
            public int iPara        ;
        };
        #endregion

        #region << Const >>
        private static readonly int Max_Channel_Lfn = 1;
        #endregion

        #region << Fields >>
        private ST_LFN_PARA_BUFF m_TxLfnParas;
        private ST_LFN_PARA_BUFF m_TxLfnParasBuf;
        private Queue<ST_LFN_PARA_BUFF> m_LfnParaQue = new Queue<ST_LFN_PARA_BUFF>();
        

        #endregion

        #region << Methods >>

        private void InitLfn()
        {
            m_iLightValue      = new int[Max_Channel_Lfn];
            m_iperiodtime      = new int[Max_Channel_Lfn];
            m_ihightime        = new int[Max_Channel_Lfn];
            m_iontime          = new int[Max_Channel_Lfn];
            m_idelaytime       = new int[Max_Channel_Lfn];
            m_itriggermode     = new int[Max_Channel_Lfn];
            m_imeasureframenum = new int[Max_Channel_Lfn];
            m_iStatError       = new int[Max_Channel_Lfn];
            m_iLightOnOff      = new int[Max_Channel_Lfn];
        }

        public bool SendMsgLfn(ST_LFN_PARA_BUFF Msg)
        {
            //Local Var.
            int iTxLen = 0;

            //Set Request Code.
            switch (Msg.iCmd)
            {
                default: return false;
                case enumLfnCmd.SetFrameChannelValue : iTxLen = m_MakeMsgSetFrameChannelValue(Msg.iCh + 1, Msg.iPara); break;
                case enumLfnCmd.SetLiveStart         : iTxLen = m_MakeMsgSetLiveStart        (Msg.iCh + 1           ); break;
                case enumLfnCmd.SetStop              : iTxLen = m_MakeMsgSetStop             (Msg.iCh + 1           ); break;
            }

            //Check Port.
            if ( RS232 == null) { return false; }
            if (!RS232._IsOpen) { return false; }

            m_bDrngComm = true;

            //Write Data./
            bool bRet = RS232.SendByte(m_szTxBuff, iTxLen);

            //Return.
            return bRet;
        }

        int AttatchData(byte Data)
        {
            //Local Var.
            int iLast = 0;
            byte byteNull = 0xFF;
            iLast = Array.IndexOf(m_szTxBuff, byteNull);

            //Check Max.
            if ((iLast + 1) >= 128)
            {
                Array.Clear(m_szTxBuff, 0, TX_BUFF);
                return 0;
            }

            //Attach.
            m_szTxBuff[iLast] = Data;

            //Ok.
            return (iLast + 1);
        }


        int m_MakeMsgSetLiveStart(int Ch)
        {
            //Local Var.
            int iLen = 0;

            AttatchData(0x02);
            AttatchData((byte)Convert.ToChar(Ch.ToString()));
            AttatchData(Convert.ToByte('o'));
            AttatchData(0x00);
            AttatchData(0x00);
            AttatchData(0x00);
            AttatchData(0x00);
            iLen = AttatchData(0x03);

            return iLen;
        }
        int m_MakeMsgSetStop(int Ch)
        {
            //Local Var.
            int iLen = 0;

            AttatchData(0x02);
            AttatchData((byte)Convert.ToChar(Ch.ToString()));
            AttatchData(Convert.ToByte('f'));
            AttatchData(0x00);
            AttatchData(0x00);
            AttatchData(0x00);
            AttatchData(0x00);
            iLen = AttatchData(0x03);

            return iLen;
        }
        int m_MakeMsgSetFrameChannelValue(int Ch, int iVal)
        {
            //Local Var.
            int iLen = 0;

            AttatchData(0x02);
            AttatchData((byte)Convert.ToChar(Ch.ToString()));
            AttatchData(Convert.ToByte('d'));
            AttatchData(Convert.ToByte(iVal.ToString("0000")[0]));
            AttatchData(Convert.ToByte(iVal.ToString("0000")[1]));
            AttatchData(Convert.ToByte(iVal.ToString("0000")[2]));
            AttatchData(Convert.ToByte(iVal.ToString("0000")[3]));
            iLen = AttatchData(0x03);

            return iLen;
        }


        private void SendCommandLfn(enumLfnMode Mod, enumLfnCmd Cmd, int nFrame, int nCh, int nPara)
        {
            ST_LFN_PARA_BUFF m_TmpPara = new ST_LFN_PARA_BUFF();

            m_TmpPara.iCmd = Cmd;
            m_TmpPara.iFrame = nFrame;
            m_TmpPara.iCh = nCh;
            m_TmpPara.iPara = nPara;
            m_TmpPara.iMode = (int)Mod;
            try
            {
                m_LfnParaQue.Enqueue(m_TmpPara); //끝부분에 데이타 추가
            }
            catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine("SendCommandLfn Exception:" + e.Message);
            }
        }

        private void UpdateLfn()
        {
            //Local Var.


            //Update.
            if (m_tSendTimer.OnDelay((m_iSendStep != 0 || m_bDrngComm), 50000))
            {
                Reset();
                m_bDrngComm = false;
            }

            //Message Process..
            switch (m_iSendStep)
            {
                case 0:
                    if (m_LfnParaQue.Count <= 0) 
                    {
                        //m_LfnParaQue.Clear();
                        m_iSendStep = 0; 
                        break; 
                    }

                    //m_LpdParaQue.Clear();
                    m_TxLfnParas = m_LfnParaQue.Dequeue();
                    m_iSendStep++;
                    return;

                case 1:
                    if (!SendMsgLfn(m_TxLfnParas)) break;
                    m_tSendDelay.Clear();
                    m_szTxBuff.MemSet(0xFF);
                    m_iSendStep++;
                    return;

                case 2:
                    if (!m_tSendDelay.OnDelay(true, 90)) return;
                    //if (m_bDrngComm) break;

                    //return no
                    m_bDrngComm = false;
                    m_tSendTimer.Clear(); //Clear Timer.
                    m_iSendStep = 0;
                    break;
            }
            //
            //m_tDelay.OnDelay((m_LfnParaQue.Count <= 0) && !cDEF.SEQ._bRun, 3000);
            //if (m_tDelay.Out)
            //{
            //
            //    m_tDelay.Clear();
            //}
        }
        private void SetLightOnLfn(int iCh, int iOnOff)
        {
            if (iOnOff > 0)
                SendCommandLfn(enumLfnMode.WRITE, enumLfnCmd.SetLiveStart, 0, iCh, iOnOff);
            else
                SendCommandLfn(enumLfnMode.WRITE, enumLfnCmd.SetStop, 0, iCh, iOnOff);
        }

        private void SetLightValueLfn(int iCh, int iValue)
        {
            SendCommandLfn(enumLfnMode.WRITE, enumLfnCmd.SetFrameChannelValue, 0, iCh, iValue);
        }


        private void SetAllLightValueLfn(int iValue)
        {
           // SendCommandLfn(enumLfnMode.WRITE, enumLfnCmd.SetAllChannelValue, 0, 0, iValue);
        }
        private void GetLightValueLfn(int iCh)
        {
            //SendCommandLfn(enumLfnMode.READ, enumLfnCmd.GetStat, 0, iCh, 0);
        }
        private void GetLightOnLfn(int iCh)
        {
            //SendCommandLfn(enumLfnMode.READ, enumLfnCmd.GetStat, 0, iCh, 1);
        }

        #endregion

        #region << Events >>
        public void OnReciveLfn(object sender, int len, byte[] data)
        {
        
            string sRcv; //Received Message.
            string sDat = ""; //Received Data.
            int iPos1;//, iPos2;

            //Chack
            if ( RS232 == null) return;
            if (!RS232._IsOpen) return;

            try
            {
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
                        if (m_TxLpdParasBuf.iPara == 0) m_iLightValue[m_TxLpdParasBuf.iCh] = int.Parse(sDat, System.Globalization.NumberStyles.HexNumber);
                        else if (m_TxLpdParasBuf.iPara == 1) m_iLightOnOff[m_TxLpdParasBuf.iCh] = sDat == "ON" ? 1 : 0;
                        else if (m_TxLpdParasBuf.iPara == 2) m_iStatError[m_TxLpdParasBuf.iCh] = sDat == "OK" ? 0 : 1;
                    }

                    //if (m_TxLpdParasBuf.iCmd == enumLpdCmd.GetFrameChannelValue) m_iLightValue[m_TxLpdParas.iCh] = int.Parse(sDat, System.Globalization.NumberStyles.HexNumber);
                    //if (m_TxLpdParasBuf.iCmd == enumLpdCmd.GetLiveState        ) m_iLightOnOff[m_TxLpdParas.iCh] = sDat == "ON" ? 1 : 0;

                    return;
                }
            }
            catch (Exception Err)
            {
                System.Diagnostics.Debug.WriteLine("Lfn Update Exception:" + Err.Message);
            }
        }

        #endregion
    }
}
