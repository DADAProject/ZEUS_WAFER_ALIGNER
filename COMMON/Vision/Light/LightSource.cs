using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    public partial class TLightSource
    {

        #region << Enums >>
        public enum LightSourceType { Lvs, Eps, Lpd , Lfn}
        private TSerialUnit RS232;

        #endregion

        #region << Fields >>
        private LightSourceType LightType;
        #endregion

        private static readonly int Max_Channel = 16;
        private static readonly int Max_Frame = 10;
        private static readonly int Max_BuffSize = 512;


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   // Member Var.            
        int version;

        bool m_bDrngComm; //Process Value.
        int m_iSendStep; //Update Step - Read Cycle.

        int[,] frame = new int[Max_Channel, Max_Frame];

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_tSendTimer = new TOnDelayTimer();
        TOnDelayTimer m_tSendDelay = new TOnDelayTimer();
        TOnDelayTimer m_tDelay     = new TOnDelayTimer();


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //const Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        const int OFFSET = 23;
        const int TX_BUFF = 1024;
        const int RX_BUFF = 1024;

        byte[] m_szTxBuff = new byte[Max_BuffSize];
        byte[] m_szRxBuff = new byte[Max_BuffSize];

        int m_iRxCnt;


        //protected: //Inheritable Vars.        

        //public:    //Direct Accessable Vars.  
        public int[] m_iLightOnOff; // 
        public int[] m_iLightValue; // 
        public int[] m_iperiodtime; // ㎲ 단위
        public int[] m_ihightime; // ㎲ 단위
        public int[] m_iontime; // ㎲ 단위
        public int[] m_idelaytime; // ㎲ 단위
        public int[] m_itriggermode; // 0 : internal, 1 : external
        public int[] m_imeasureframenum;
        public int[] m_iStatError;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsOpen { get; private set; }

        //Objects.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLightSource()
        {
            RS232 = new TSerialUnit();
            RS232.OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);
            m_ParaQue.Clear();
            m_szTxBuff.MemSet(0xFF);
        }
        ~TLightSource()
        {


        }

        #region << Methods >>
        public void Init(LightSourceType lightType, string sPortNo) //"COM1"
        {
            try
            {
                LightType = lightType;

                RS232.Open(sPortNo, 19200, 8, Parity.None, StopBits.One);

                if (LightType == LightSourceType.Eps) InitEps();
                if (LightType == LightSourceType.Lpd) InitLpd();
                if (LightType == LightSourceType.Lfn) InitLfn();

                if (!RS232._IsOpen)
                {
                    IsOpen = false;
                    MsgBox.Error($"[LightSource : {lightType}] COM Port[{sPortNo}] Open Fail");
                    return;
                }
                else
                {
                    IsOpen = true;
                }
                //
                Reset();

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Init : {e.Message}");
                throw;
            }
        }

        public void Reset()
        {
            try 
            { 
                m_LfnParaQue.Clear();

                m_iSendStep = 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Reset : {e.Message}");
            }
        }

        public void Clear()
        {
            m_iRxCnt = 0;
        }
        public void Close()
        {
            RS232.Port_Close();
            IsOpen = false;
        }
        #endregion

        #region << Events >>
        public void OnRecive(object sender, int len, byte[] data)
        {
            if (LightType == LightSourceType.Lvs) OnReciveLvs(sender, len, data);
            if (LightType == LightSourceType.Eps) OnReciveEps(sender, len, data);
            if (LightType == LightSourceType.Lpd) OnReciveLpd(sender, len, data);
            if (LightType == LightSourceType.Lfn) OnReciveLfn(sender, len, data);
        }

        #endregion

        #region << Methods >>
        public void SetLightOn(int iCh, bool bOnOff)
        {
            //if (LightType == LightSourceType.Lvs) SetLightValueLvs(iCh, bOnOff);
            if (LightType == LightSourceType.Eps) SetLightOnEps(iCh, bOnOff == true ? 1 : 0);
            if (LightType == LightSourceType.Lpd) SetLightOnLpd(iCh, bOnOff == true ? 1 : 0);
            if (LightType == LightSourceType.Lfn) SetLightOnLfn(iCh, bOnOff == true ? 1 : 0);
        }

        public void SetLightValue(int iCh, int iValue)
        {
            if (LightType == LightSourceType.Lvs) SetLightValueLvs(iCh, iValue);
            if (LightType == LightSourceType.Eps) SetLightValueEps(iCh, iValue);
            if (LightType == LightSourceType.Lpd) SetLightValueLpd(iCh, iValue);
            if (LightType == LightSourceType.Lfn) SetLightValueLfn(iCh, iValue);
        }

        public void SetAllLightValue(int iUseCh = -1)
        {
            if (LightType == LightSourceType.Lvs) SetAllLightValueLvs(iUseCh);
            if (LightType == LightSourceType.Eps) SetAllLightValueEps(iUseCh);
            if (LightType == LightSourceType.Lpd) SetAllLightValueLpd(iUseCh);
        }

        public void GetStatLightOn(int iCh)
        {
            if (iCh < 0 || iCh >= Max_Channel) return;
            if (LightType == LightSourceType.Lpd) GetLightOnLpd(iCh);
        }
        public void GetStatLightValue(int iCh)
        {
            if (iCh < 0 || iCh >= Max_Channel) return;
            if (LightType == LightSourceType.Lpd) GetLightValueLpd(iCh);
        }

        public int GetLightOn(int iCh)
        {
            if (iCh < 0 || iCh >= Max_Channel) return 0;
            return m_iLightOnOff[iCh];
        }
        public int GetLightValue(int iCh)
        {
            if (iCh < 0 || iCh >= Max_Channel) return 0;
            return m_iLightValue[iCh];
        }

        public void Load(string DevName, int iUseCh = -1)
        {
            //UserSet - 저장할 CHIP 변수 (절대 추가/삭제 하지 마시오 - 이진파일)
            String sPath;
            //String sFile = string.Format("{0}Param", LightType.ToString());
            String sFile = Enum.GetName(typeof(LightSourceType), LightType);


            //Make Dir.
            FNC.CreateDirOnWork("Project");
            FNC.CreateDirOnWork("Project\\" + DevName);

            sPath = Application.StartupPath + "\\Project\\" + DevName + "\\" + sFile + ".LightSource";

            if (!File.Exists(sPath)) return;

            using (var fs = new FileStream(sPath, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                version = br.ReadInt32();
                for (int i = 0; i < Max_Channel; i++)
                {
                    m_iLightValue     [i] = br.ReadInt32();
                    m_iperiodtime     [i] = br.ReadInt32();
                    m_ihightime       [i] = br.ReadInt32();
                    m_iontime         [i] = br.ReadInt32();
                    m_idelaytime      [i] = br.ReadInt32();
                    m_itriggermode    [i] = br.ReadInt32();
                    m_imeasureframenum[i] = br.ReadInt32();
                }
                br.Close();
            }

            SetAllLightValue(iUseCh);
        }
        public void Save(string DevName)
        {
            //UserSet - 저장할 CHIP 변수 (절대 추가/삭제 하지 마시오 - 이진파일)
            String sPath;
            //String sFile = string.Format("{0}Param", LightType.ToString());
            String sFile = Enum.GetName(typeof(LightSourceType), LightType);

            //Make Dir.
            FNC.CreateDirOnWork("Project");
            FNC.CreateDirOnWork("Project\\" + DevName);

            sPath = Application.StartupPath + "\\Project\\" + DevName + "\\" + sFile + ".LightSource";
            using (var fs = new FileStream(sPath, FileMode.OpenOrCreate))
            {
                BinaryWriter wr = new BinaryWriter(fs);
                wr.Write(version);
                for (int i = 0; i < Max_Channel; i++)
                {
                    wr.Write(m_iLightValue[i]);
                    wr.Write(m_iperiodtime[i]);
                    wr.Write(m_ihightime[i]);
                    wr.Write(m_iontime[i]);
                    wr.Write(m_idelaytime[i]);
                    wr.Write(m_itriggermode[i]);
                    wr.Write(m_imeasureframenum[i]);
                }
                wr.Close();

            }
        }

        public void Update()
        {
            if (LightType == LightSourceType.Lvs) UpdateLvs();
            if (LightType == LightSourceType.Eps) UpdateEps();
            if (LightType == LightSourceType.Lpd) UpdateLpd();
            if (LightType == LightSourceType.Lfn) UpdateLfn();
        }
        #endregion

    }

}
