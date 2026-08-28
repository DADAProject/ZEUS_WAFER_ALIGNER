using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using FASTECH;
using WMX3ApiCLR;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TIoTastech                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class TIoFastech
    {

        const int MAX_MASTER_COUNT = 10;
        const int MAX_CH           = 32;
        const int IN_COUNT         = 6 ;
        const int OUT_COUNT        = 9 ;

        private readonly uint[] mInBitAddr  = { 0x04000000, 0x08000000, 0x10000000, 0x20000000, 0x40000000, 0x80000000 };
        private readonly uint[] mOutBitAddr = { 0x00008000, 0x00010000, 0x00020000, 0x00040000, 0x00080000, 0x00100000, 0x00200000, 0x00400000, 0x00800000 };

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        //int m_iBitAddr;
        //int m_InModuleQty;
        //int m_OutModuleQty;
        //int m_iSlaveTotal ; 

        //int m_iDiData;
        //int m_iDoData;
        //int m_iDoInit;

        int m_nModuleQty; //Total Module 수량
        int m_nStartAdd ; //Start IP Address

        IPAddress m_ipaddr = null;
        string    m_sIPMain;

        //int[,] m_iXDefine   = new int[MAX_MASTER_COUNT,  MAX_CH];
        //int[,] m_iYDefine   = new int[MAX_MASTER_COUNT,  MAX_CH];
        //int[]  m_iReadData  = new int[MAX_MASTER_COUNT * IN_COUNT ];
        //int[]  m_iWriteData = new int[MAX_MASTER_COUNT * OUT_COUNT];
        //int[]  m_iWInitData = new int[MAX_MASTER_COUNT * MAX_CH];
        //int[,] m_iXModule   = new int[MAX_MASTER_COUNT,  MAX_CH];
        //int[,] m_iYModule   = new int[MAX_MASTER_COUNT,  MAX_CH];
        //int[]  m_iReqChange = new int[MAX_MASTER_COUNT * MAX_CH];
        //int[,] m_iModuleID  = new int[MAX_MASTER_COUNT,  MAX_CH];
        //int[]  m_iReadYData = new int[MAX_MASTER_COUNT * MAX_CH];

        bool[]  m_bReadData  = new bool[MAX_MASTER_COUNT * IN_COUNT ];
        bool[]  m_bWriteData = new bool[MAX_MASTER_COUNT * OUT_COUNT];


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool m_bLoaded;   //Device Driver Loaded.


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TIoFastech()
        {
            //Clear Var.
            //m_iSlaveTotal = 0    ;
            m_bLoaded = false;

            m_sIPMain = string.Empty;

        }

        ~TIoFastech() { }
        //Init.
        //------------------------------------------------------------------------
        public bool Init()
        {
            //
            if (m_ipaddr == null || m_nModuleQty < 1 || m_nStartAdd < 0) return false; 

            m_bLoaded = true;

            try
            {
                //
                string    ip    = string.Empty;
                IPAddress ipadd = null; 
                for (int n = 0; n < m_nModuleQty; n++)
                {
                    ip = string.Format($"{m_sIPMain}.{m_nStartAdd+n}");
                    IPAddress.TryParse(ip, out ipadd);
                    if (!EziMOTIONPlusELib.FAS_IsBdIDExist(n, ref ipadd)) m_bLoaded = false; 
                }

                if(m_bLoaded)
                {
                    //

                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FASTECH IO] Init exeception - {ex.Message}");
                m_bLoaded = false;
            }


            return m_bLoaded;
        }
        //--------------------------------------------------------------------------
        public void SetIP(string ip1, string ip2, string ip3, string ip4, int maxno)
        {
            //
            string sIP = string.Format($"{ip1}.{ip2}.{ip3}.{ip4}");
            IPAddress.TryParse(sIP, out m_ipaddr);

            m_sIPMain = string.Format($"{ip1}.{ip2}.{ip3}.");

            //
            m_nModuleQty = maxno;

            //
            int.TryParse(ip4, out m_nStartAdd);

        }
        //--------------------------------------------------------------------------
        public void Reset     ()
        {
            //Init.
            //Init();
        }
        //--------------------------------------------------------------------------
        public void Reload    ()
        {
            //Unload CNET
            UnloadLib ();

            //Init.
            //Init();
        }
        //--------------------------------------------------------------------------
        public void UnloadLib ()
        {
            //UnLoad Device.
        }
        //--------------------------------------------------------------------------
        //Get Memory.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private int GetMemoryX(int n)
        {
            ////Local Var.
            //string szRX;
            //string szCX;
            //int R, C;
            //
            ////Check.
            //if (n < 0) return 0;
            //
            //szRX = string.Format("{0:X6}"      , (n & 0xff00));
            //szCX = string.Format("{0,6:000000}", (n & 0x00f0));
            //
            //R = Convert.ToInt32(szRX) / 100     ;
            //C = Convert.ToInt32(szCX) / 0x10    ;
            //
            //if (R < 0 || R >= MAX_MASTER_COUNT) return 0;
            //if (C < 0 || C >= MAX_CH          ) return 0;
            //
            ////Return.
            //return m_iXModule[R, C];
            return -1;
        }
        //--------------------------------------------------------------------------
        private int GetMemoryY(int n, ref int addno)
        {
            ////Check.
            //if (n < 0) return 0;
            //
            //addno = n % OUT_COUNT;
            //
            ////Return.
            //return n / OUT_COUNT;

            return -1;
        }
        //--------------------------------------------------------------------------
        //I/O Func.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int  Input         (int Addr         )
        {
            //Check Error.
            if (!m_bLoaded    ) return 0;
            if (Addr < 0x0000 ) return 0;
            if (Addr >= 0xffff) return 0;

            //Return.
            return m_bReadData[Addr] ? 1 : 0;
        }

        //--------------------------------------------------------------------------
        public int  Output        (int Addr , int on)
        {
            //Check Error.
            if (!m_bLoaded    ) return 0;
            if (Addr < 0x0000 ) return 0;
            if (Addr >= 0xffff) return 0;
            
            m_bWriteData[Addr] = (on == 1) ? true : false ;

            return m_bWriteData[Addr] == true ? 1 : 0 ;

        }
        //--------------------------------------------------------------------------
        //Update.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Update           ()
        {
            //
            if (!m_bLoaded) return;

            //Read Update
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            UpdateIn();

            //Write Update
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            UpdateOut();
        }
        //--------------------------------------------------------------------------
        private void UpdateIn()
        {
            uint ioInput = 0;
            bool isOn    = false;

            for (int n =0; n < m_nModuleQty; n++)
            {
                EziMOTIONPlusELib.FAS_GetIOInput(n, ref ioInput);
                
                for (int j = 0; j < mInBitAddr.Length; j++)
                {
                    isOn = Convert.ToBoolean(ioInput & mInBitAddr[j]);

                    m_bReadData[n * mInBitAddr.Length + j] = isOn; 
                }
            }
        }
        //--------------------------------------------------------------------------
        private void UpdateOut()
        {
            bool isOn    = false;
            uint mask    = 0;
            
            for (int n = 0; n < m_nModuleQty; n++)
            {
                for (int j = 0; j < mOutBitAddr.Length; j++)
                {
                    isOn   = m_bWriteData[n * mOutBitAddr.Length + j];
                    mask   = mOutBitAddr[j];

                    if (isOn) EziMOTIONPlusELib.FAS_SetIOOutput(n, mask, 0);
                    else      EziMOTIONPlusELib.FAS_SetIOOutput(n, 0, mask);
                }
            }

        }
    }
}
