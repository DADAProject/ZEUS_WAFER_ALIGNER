using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TIoCNET                                                          */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class TIoAJIN
    {
        public struct TModuleInfo
        {
            public readonly int    iModuleId   ;
            public readonly int    iCountInput ;
            public readonly int    iCountOutput;
            public readonly int    iBoardID    ;
            public readonly string sName       ;

            public TModuleInfo(int pModuleIdx, int pInCount, int pOutCount, int pBoardID, string pName = "")
            {
                iModuleId    = pModuleIdx;
                iCountInput  = pInCount  ;
                iCountOutput = pOutCount ;
                iBoardID     = pBoardID  ;
                sName        = pName     ;
            }
        }

        public TModuleInfo[] ModuleInfo;

        const int MAX_MASTER_COUNT = 1 ;
        const int MAX_CH           = 64;
        EN_ALG_KIND algKind;
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        int          m_iBitAddr    ;
        int          m_InModuleQty ;
        int          m_OutModuleQty;
        //int          m_iSlaveTotal ; 

        int          m_iDiData     ;
        int          m_iDoData     ;
        int          m_iDoInit     ;
        int          m_iModuleQty  ;
                  

        int[,] m_iXDefine   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[,] m_iYDefine   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[]  m_iReadData  = new int [MAX_MASTER_COUNT * MAX_CH];
        int[]  m_iWriteData = new int [MAX_MASTER_COUNT * MAX_CH];
        int[]  m_iWInitData = new int [MAX_MASTER_COUNT * MAX_CH];
        int[,] m_iXModule   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[,] m_iYModule   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[]  m_iReqChange = new int [MAX_MASTER_COUNT * MAX_CH];
        int[,] m_iModuleID  = new int [MAX_MASTER_COUNT , MAX_CH];
        int[]  m_iReadYData = new int [MAX_MASTER_COUNT * MAX_CH];


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool m_bLoaded;   //Device Driver Loaded.
        public String m_sFNAddress;
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TIoAJIN()
        {
            //초기화시에 한번만 사용.
            //for (int i = 0; i < MAX_MASTER_COUNT; i++)
            //{
            //    for (int j = 0; j < MAX_CH; j++)
            //    {
            //        m_iXDefine[i,j] = -1;
            //        m_iYDefine[i,j] = -1;
            //        }
            //    }

            //Cnet Addr 할당 방법
            //0x020(Hex)  ==  32 (Dec) / 16(Dec) = 2  (Channel No)
            //0x190(Hex)  ==  400(Dec) / 16(Dec) = 25 (Channel No)
            //Clear Var.


            //Clear Var.
            //m_iSlaveTotal = 0    ;
            m_bLoaded     = false;
        }
        ~TIoAJIN() { }
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool Init(EN_ALG_KIND kind)
        {
            //
            algKind = kind; 

            if (kind == EN_ALG_KIND.EtherCAT)
            {
                return Init_ECAT();
            }
            else
            {
                return Init();
            }
        }
        //------------------------------------------------------------------------
        public bool Init()
        {			
            //Clear Var.
            //m_iSlaveTotal = 0;
            uint uiStatus = 0    ;
            uint uiRet    = 0    ;
            m_bLoaded     = false;
            m_iModuleQty  = 0    ;

            //Load Device.
            //디바이스를 로드 합니다.
            Load(true);

            if (CAXL.AxlIsOpened() == 0)
            {
                if ((CAXL.AxlOpenNoReset(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS))
                {
                    MsgBox.Error("[AJIN] AXL Open Fail");
                    return false;
                }
            }
            //
            uiRet = CAXD.AxdInfoIsDIOModule(ref uiStatus);
            if (uiRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                if (uiStatus != (uint)AXT_EXISTENCE.STATUS_EXIST)
                {
                    MsgBox.Error("[AJIN IO]Module does not exist.");
                    return false;
                }
            }
            else
            {
                MsgBox.Error("[AJIN IO]AxdInfoIsDIOModule Error!!");
                return false;
            }

            int  lDBoardNo    = 0;
            int  lDModulePos  = 0;
            uint ModuleId     = 0;
            int  nModuleNo    = 0;

            for (int i = 0 ; i < MAX_MASTER_COUNT ; i++) 
            {
                for (int j = 0 ; j < MAX_CH ; j++) 
                {
                    if(m_iXDefine[i,j]<0 && m_iYDefine[i,j]<0) continue;
                    
                    if (m_iXDefine[i, j] >= 0) { m_iXModule[i, j] = nModuleNo;  }
                    else                       { m_iYModule[i, j] = nModuleNo;  }
                    m_iReqChange[nModuleNo] = 0;
                    nModuleNo ++;
                }
            }
            
            CAXD.AxdInfoGetModuleCount(ref m_iModuleQty);
            for (int i = 0; i < m_iModuleQty; i++)
            {
                CAXD.AxdInfoGetModule(i, ref lDBoardNo, ref lDModulePos, ref ModuleId);
                m_iModuleID[0, i] = (int)ModuleId;
                //if (ModuleId==0x97(151) /* 1 : Input  32          */) { AxtDIOModule[i].InByteCnt=4; AxtDIOModule[i].OutByteCnt=0;	} //AXT_SIO_DI32
                //if (ModuleId==0x9E(158) /* 2 : Output 32          */) { AxtDIOModule[i].InByteCnt=0; AxtDIOModule[i].OutByteCnt=4;	} //AXT_SIO_DO32T
                //if (ModuleId==0x9F(159) /* 0 : Input  16/Output 16*/) { AxtDIOModule[i].InByteCnt=2; AxtDIOModule[i].OutByteCnt=2;	} //AXT_SIO_DB32T
            }
            m_bLoaded = true;
            return true;
        }
        //------------------------------------------------------------------------
        public bool Init_ECAT()
        {
            //Clear Var.
            uint uiStatus = 0;
            uint uiRet    = 0;
            uint uiRet1   = 0;
            uint uiRet2   = 0;
            //m_iSlaveTotal = 0;
            m_bLoaded     = false;
            m_iModuleQty  = 0;

            //디바이스를 로드 합니다.
            //Load(true);  //Load Device.

            if (CAXL.AxlIsOpened() == 0)
            {
                if ((CAXL.AxlOpenNoReset(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS))
                {
                    MsgBox.Error("[AJIN] AXL Open Fail");
                    return false;
                }
            }
            //
            uiRet = CAXD.AxdInfoIsDIOModule(ref uiStatus);
            if (uiRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                if (uiStatus != (uint)AXT_EXISTENCE.STATUS_EXIST)
                {
                    MsgBox.Error("[AJIN IO]Module does not exist.");
                    return false;
                }
            }
            else
            {
                MsgBox.Error("[AJIN IO]AxdInfoIsDIOModule Error!!");
                return false;
            }

            int  lDBoardNo    = 0;
            int  lDModulePos  = 0;
            uint ModuleId     = 0;
            int  iInputCount  = 0;
            int  iOutputCount = 0;
            
            uiRet = CAXD.AxdInfoGetModuleCount(ref m_iModuleQty);
            if (uiRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ModuleInfo = new TModuleInfo[m_iModuleQty];
            }
            else 
            {
                MsgBox.Error("[AJIN IO]AxdInfoGetModuleCount Error!!");
                return false;
            }

            //Set Module Info
            for (int i = 0; i < m_iModuleQty; i++)
            {
                //
                uiRet = CAXD.AxdInfoGetModule(i, ref lDBoardNo, ref lDModulePos, ref ModuleId);
                
                if (uiRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //Input
                    uiRet1 = CAXD.AxdInfoGetInputCount( (int)i, ref iInputCount);

                    //Output
                    uiRet2 = CAXD.AxdInfoGetOutputCount((int)i, ref iOutputCount);

                    if ((uiRet1 != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) &&
                        (uiRet2 != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)) continue;
                    
                    //
                    ModuleInfo[i] = new TModuleInfo(i, iInputCount, iOutputCount, lDBoardNo, ModuleId.ToString());
                }
                else
                {
                    MsgBox.Error("[AJIN IO]AxdInfoGetModule Error!!");
                    return false; 
                }
            }
            
            //
            m_bLoaded = true;
            return true;
        }

        //--------------------------------------------------------------------------
        public void Reset     ()
        {
            //Init.
            Init();
        }
        //--------------------------------------------------------------------------
        public void Reload    ()
        {
            //Unload CNET
            UnloadLib ();

            //Init.
            Init();
        }
        //--------------------------------------------------------------------------
        public void UnloadLib ()
        {
            //UnLoad Device.
        }   
        //--------------------------------------------------------------------------
        //Get Memory.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int GetMemoryX(int n)
        {
            //Local Var.
            string szRX;
            string szCX;
            int R, C;

            //Check.
            if (n < 0) return 0;

            szRX = string.Format("{0:X6}"      , (n & 0xff00));
            szCX = string.Format("{0,6:000000}", (n & 0x00f0));

            R = Convert.ToInt32(szRX) / 100     ;
            C = Convert.ToInt32(szCX) / 0x10    ;

            if (R < 0 || R >= MAX_MASTER_COUNT) return 0;
            if (C < 0 || C >= MAX_CH          ) return 0;

            //Return.
            return m_iXModule[R, C];
        }
        //--------------------------------------------------------------------------
        public int GetMemoryY(int n)
        {
            //Local Var.
            string szRX;
            string szCX;
            int R, C;

            //Check.
            if (n < 0) return 0;

            szRX = string.Format("{0:X6}"      , (n & 0xff00));
            szCX = string.Format("{0,6:000000}", (n & 0x00f0));

            R = Convert.ToInt32(szRX) / 100  ;
            C = Convert.ToInt32(szCX) / 0x10 ;

            if (R < 0 || R >= MAX_MASTER_COUNT) return 0;
            if (C < 0 || C >= MAX_CH          ) return 0;

            //Return.
            return m_iYModule[R, C];
        }
        //--------------------------------------------------------------------------
        //I/O Func.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int  Input         (int Addr         )
        {
            int iModCh;
            //Check Error.
            if (!m_bLoaded    ) return 0;
            if (Addr < 0x0000 ) return 0;
            if (Addr >= 0xffff) return 0;

            //Get Addr.
            iModCh = GetMemoryX(Addr);
            if (iModCh < 0) return 0;

            m_iBitAddr = Addr & 0x0f;

            //Get.
            m_iDiData = (m_iReadData[iModCh] >> m_iBitAddr) & 0x01;

            //Return.
            return (int)(m_iDiData & 0x01);
        }
        //------------------------------------------------------------------------
        public int Input_ECAT    (int Addr         )
        {
            try
            {
                if (Addr < 0) return 0;

                int byteIndex = Addr / 16;
                int bitIndex  = Addr % 16;

                ushort mask = Convert.ToUInt16(1 << bitIndex);
                return (m_iReadData[byteIndex] & mask) > 0 ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        //--------------------------------------------------------------------------
        public int  Output        (int Addr , int on)
        {
            int iModCh;
            
            //Check Error.
            if (!m_bLoaded    ) return 0;
            if (Addr < 0x0000 ) return 0;
            if (Addr >= 0xffff) return 0;
            
            //Get Addr.
            iModCh = GetMemoryY(Addr);
            if (iModCh < 0) return 0;
            
            m_iBitAddr = Addr & 0x0f;
            
            //Put.
            m_iDoData = (m_iWriteData[iModCh] >> m_iBitAddr) & 0x01;
            
            if (m_iDoData != on)
            {
                if (on == 1) m_iWriteData[iModCh] = m_iWriteData[iModCh] |= 1   << m_iBitAddr;
                else         m_iWriteData[iModCh] = m_iWriteData[iModCh] &= ~(1 << m_iBitAddr);
            
                m_iReqChange[iModCh] = 1;
            }
            //Return.
            return (int)(m_iDoData & 0x01);

        }
        //--------------------------------------------------------------------------
        public int  Output_ECAT        (int Addr, int on)
        {
            try
            {
                if (Addr < 0) return 0;

                int byteIndex = Addr / 16;
                int bitIndex  = Addr % 16;

                var mask = (1 << bitIndex);
                m_iWriteData[byteIndex] = Convert.ToUInt16(on == 1 ? (m_iWriteData[byteIndex] | mask) : (m_iWriteData[byteIndex] & ~mask));

                return (m_iWriteData[byteIndex] & mask) > 0 ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }
        //------------------------------------------------------------------------
        public int OutputRead_ECAT(int Addr)
        {
            //Check Error.
            if (!m_bLoaded    ) return 0;
            if (Addr < 0x0000 ) return 0;
            if (Addr >= 0xffff) return 0;
            //
            try
            {
                if (Addr < 0) return 0;

                int byteIndex = Addr / 16;
                int bitIndex = Addr % 16;

                ushort mask = Convert.ToUInt16(1 << bitIndex);
                return (m_iReadYData[byteIndex] & mask) > 0 ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        //------------------------------------------------------------------------
        public int  OutInit         (int Addr         )
        {
            int iModCh;
            //Check Error.
            if (!m_bLoaded    ) return 0;
            if (Addr < 0x0000 ) return 0;
            if (Addr >= 0xffff) return 0;

            //Get Addr.
            iModCh = GetMemoryY(Addr);
            if (iModCh < 0) return 0;

            m_iBitAddr = Addr & 0x0f;

            //Get.
            m_iDoInit = (m_iWInitData[iModCh] >> m_iBitAddr) & 0x01;

            //Return.
            return (int)(m_iDoInit & 0x01);
        }
        //------------------------------------------------------------------------
        public int OutInitECAT(int Addr)
        {
            //int iModCh;
            
            //Check Error.
            if (!m_bLoaded    ) return 0;
            if (Addr < 0x0000 ) return 0;
            if (Addr >= 0xffff) return 0;

            //
            try
            {
                if (Addr < 0) return 0;

                int byteIndex = Addr / 16;
                int bitIndex = Addr % 16;

                ushort mask = Convert.ToUInt16(1 << bitIndex);
                return (m_iWInitData[byteIndex] & mask) > 0 ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }


        //--------------------------------------------------------------------------
        //Update.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Update           ()
        {
            int     iModuleNoX = 0;
            int     iModuleNoY = 0;
 
            uint[,] uiDataIO  = new uint[2, MAX_CH];

            if (!m_bLoaded) return;

             //Read Update
             //--------------------------------------------------------
             for (int i = 0; i < m_iModuleQty; i++)
             {
                //if(m_iModuleID[0, i] != 0x97) continue;
                if (algKind == EN_ALG_KIND.EtherCAT)
                {
                    if (m_iModuleID[0, i] != 0x63) continue; //ECAT
                }
                else if(algKind == EN_ALG_KIND.AXA)
                {
                    if (m_iModuleID[0, i] != 0x85) continue; //RTEX
                }
                CAXD.AxdiReadInportWord(i, 0, ref uiDataIO[0, iModuleNoX++]);
                CAXD.AxdiReadInportWord(i, 1, ref uiDataIO[0, iModuleNoX++]);

             }
             iModuleNoX = 0;
             for (int i = 0; i < MAX_CH; i++)
             {
                if(m_iXDefine[0, i] < 0) continue;
                m_iReadData[i] = (int)uiDataIO[0, iModuleNoX++]; 
             }

             //Write Update
             //--------------------------------------------------------
             for (int i = 0; i < MAX_CH; i++)
             {
                if(m_iYDefine[0, i] < 0) continue;
                 uiDataIO[1, iModuleNoY++] = (uint)m_iWriteData[i];
             }
             iModuleNoY = 0;
             for (int i = 0; i < m_iModuleQty; i++)
             {
                //if(m_iModuleID[0, i] != 0x98) continue;
                if (algKind == EN_ALG_KIND.EtherCAT)
                {
                    if (m_iModuleID[0, i] != 0x64) continue; //ECAT
                }
                else if(algKind == EN_ALG_KIND.AXA)
                {
                    if (m_iModuleID[0, i] != 0x84) continue; //RTEX
                }

                CAXD.AxdoWriteOutportWord(i, 0, uiDataIO[1, iModuleNoY++]);
                CAXD.AxdoWriteOutportWord(i, 1, uiDataIO[1, iModuleNoY++]);
             }
        }
        //------------------------------------------------------------------------
        public void Update_ECAT()
        {
            //
            if (!m_bLoaded) return;

            uint ret;
            int Idy       = 0;
            int Idx       = 0;
            int IdyRtn    = 0;
            int moduleIdx = 0;

//             for (int i = 0; i < ModuleInfo.Length; i++)
//             {
//                 moduleIdx = ModuleInfo[i].iModuleId;
//                 for (int offset = 0; offset < ModuleInfo[i].iCountInput / 16; offset++)
//                 {
//                     uint newState = 0;
//                     ret           = CAXD.AxdiReadInportWord(moduleIdx, offset, ref newState);
//                     if(m_iReadData[Idx].Equals(Convert.ToUInt16(newState)) == false)
//                     {
//             
//                     }
//                     m_iReadData[Idx++] = Convert.ToUInt16(newState);
//                 }
//             }
//             
//             Idx = 0;
//             for (int i = 0; i < ModuleInfo.Length; i++)
//             {
//                 moduleIdx = ModuleInfo[i].iModuleId;
//                 for (int offset = 0; offset < ModuleInfo[i].iCountOutput / 16; offset++)
//                 {
//                     uint newState = Convert.ToUInt16(m_iWriteData[Idx++]);
//                     ret           = CAXD.AxdoWriteOutportWord(moduleIdx, offset, newState);
//                 }
//             }


            //
            for (int i = 0; i < ModuleInfo.Length; i++)
            {
                //
                moduleIdx = ModuleInfo[i].iModuleId;

                //Read Input
                if (ModuleInfo[i].iCountInput > 0)
                {
                    for (int offset = 0; offset < ModuleInfo[i].iCountInput / 16; offset++)
                    {
                        uint newState = 0;
                        ret = CAXD.AxdiReadInportWord(moduleIdx, offset, ref newState);
                        m_iReadData[Idx++] = Convert.ToUInt16(newState);
                    }
                }

                //Write Output
                if (ModuleInfo[i].iCountOutput > 0)
                {
                    for (int offset = 0; offset < ModuleInfo[i].iCountOutput / 16; offset++)
                    {
                        uint newState = Convert.ToUInt16(m_iWriteData[Idy++]);
                        ret = CAXD.AxdoWriteOutportWord(moduleIdx, offset, newState);
                    }

                    //Read Write Value
                    for (int offset = 0; offset < ModuleInfo[i].iCountOutput / 16; offset++)
                    {
                        uint newState = 0;
                        ret = CAXD.AxdoReadOutportWord(moduleIdx, offset, ref newState);
                        m_iReadYData[IdyRtn++] = Convert.ToUInt16(newState);
                    }
                }
            }
        }

        //------------------------------------------------------------------------
        public void GetDoInitData()
        {
            //int iModuleNoY = 0;

            uint[] uiDataIO = new uint[MAX_CH];

            if (!m_bLoaded) return;

            uint ret;
            //int Idy = 0;
            //int Idx = 0;
            int IdyRtn = 0;
            int moduleIdx = 0;

            for (int i = 0; i < ModuleInfo.Length; i++)
            {
                //
                moduleIdx = ModuleInfo[i].iModuleId;

                if (ModuleInfo[i].iCountOutput > 0)
                {
                    //Read Write Value
                    for (int offset = 0; offset < ModuleInfo[i].iCountOutput / 16; offset++)
                    {
                        uint newState = 0;
                        ret = CAXD.AxdoReadOutportWord(moduleIdx, offset, ref newState);
                        m_iWInitData[IdyRtn++] = Convert.ToUInt16(newState);
                    }
                }
            }
        }
        //--------------------------------------------------------------------------
        public void UpdateByGrid(bool isInput, ref System.Windows.Forms.DataGridView Table)
        {
             int i=0;
            //Check Pointer
            if (Table == null) return;

            Table.Dock = System.Windows.Forms.DockStyle.Fill;
            FNC.SetGridStyle(ref Table, 30, false);

            Table.Columns.Add("Address"  , ""  );
            Table.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Table.Columns[0].Width = 80 ;

            for (i = 0 ; i < MAX_CH ; i++) 
            {
                Table.Columns.Add(Convert.ToString(i+1)  , Convert.ToString(i+1));
                Table.Columns[i+1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Table.Columns[i+1].Width = 20 ;
            }

            String[] sItem = new String[MAX_CH + 1];
            //Set Row/Col
            for (i = 0 ; i < MAX_MASTER_COUNT ; i++) {
                sItem[0] = string.Format("{0,2:00}000 ~ {1,2:00}64F", i, i);
                for (int j = 0 ; j < MAX_CH ; j++) {
                    if(isInput) sItem[j+1] = Convert.ToString(m_iXDefine[i,j]);
                    else        sItem[j+1] = Convert.ToString(m_iYDefine[i,j]);
                    }
                Table.Rows.Add(sItem);    
                }   
            Table.Visible   = true;            
        }
        //--------------------------------------------------------------------------
        public void SaveFrGrid(bool isInput, ref System.Windows.Forms.DataGridView Grid)
        {
            for(int i=0; i<Grid.RowCount ;i++) 
            {
                for(int j=0; j<Grid.ColumnCount-1 ;j++) 
                {
                    if(isInput) m_iXDefine[i,j]  = Convert.ToInt32(Grid[j+1,i].Value.ToString());
                    else        m_iYDefine[i,j]  = Convert.ToInt32(Grid[j+1,i].Value.ToString());
                }
            }
            Load(false);    
        }
        //--------------------------------------------------------------------------
        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(bool IsLoad)
        {
            //Local Var.
            string sPath = m_sFNAddress;

            //File Open.
            int iFAccess = IsLoad ? (int)FileAccess.Read : (int)FileAccess.Write;
            FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, (FileAccess)iFAccess);

            if (IsLoad)
            {
                BinaryReader br = new BinaryReader(fp);
                if(br.PeekChar()<0) return;
                for (int i = 0; i < MAX_MASTER_COUNT; i++)
                {
                    for (int j = 0; j < MAX_CH; j++)
                    {
                        m_iXDefine[i, j] = br.ReadInt32();
                        m_iYDefine[i, j] = br.ReadInt32();
                        if (m_iXDefine[i, j] >= 0) m_InModuleQty ++;
                        if (m_iYDefine[i, j] >= 0) m_OutModuleQty++;
                    }
                }
                br.Close();
            }
            else
            {
                BinaryWriter wr = new BinaryWriter(fp);
                for (int i = 0; i < MAX_MASTER_COUNT; i++)
                {
                    for (int j = 0; j < MAX_CH; j++)
                    {
                        wr.Write(m_iXDefine[i, j]);
                        wr.Write(m_iYDefine[i, j]);

                    }
                }
                wr.Close();
            }
        }
    }
}
