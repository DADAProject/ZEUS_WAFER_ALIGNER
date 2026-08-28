using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using WMX3ApiCLR;

namespace eMachine
{
    /***************************************************************************/
    /* Structure
    /***************************************************************************/
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

    /***************************************************************************/
    /* Class: TIoWmx3                                                          */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class TIoWmx3
    {
        WMX3Api     IoWmx3      = new WMX3Api    (); // When all the devices are done, the WMX3 engine will also terminate.	        
        DevicesInfo Wmx3DevInfo = new DevicesInfo(); // Get DevicesInfo to determine the type of device currently created
        Io          ioLib; //= new Io(apiWmx3);

        int         m_iDeviceCnt;


        public TModuleInfo[] ModuleInfo;

        const int MAX_MASTER_COUNT = 1;
        const int MAX_CH           = 64;

        const int MAX_INPUT_SIZE  = 1450;
        const int MAX_OUTPUT_SIZE = 1450;
        
        byte[] m_inData     = new byte[MAX_INPUT_SIZE ];
        byte[] m_outData    = new byte[MAX_OUTPUT_SIZE];
        byte[] m_ReadYData  = new byte[MAX_OUTPUT_SIZE];

        byte   m_gout = 0x00;
        byte   m_gIn  = 0x00;


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        int m_iBitAddr    ;
        int          m_InModuleQty ;
        int          m_OutModuleQty;
        //int          m_iSlaveTotal ; 

        int          m_iDiData     ;
        int          m_iDoData     ;
        int          m_iDoInit     ;
        int          m_iModuleQty  ;
                  

        int[]  m_iReadData  = new int [MAX_INPUT_SIZE           ];
        int[]  m_iWriteData = new int [MAX_OUTPUT_SIZE          ];
        
        int[]  m_iReadYData = new int [MAX_OUTPUT_SIZE          ];


        int[]  m_iWInitData = new int [MAX_MASTER_COUNT * MAX_CH];
        int[]  m_iReqChange = new int [MAX_MASTER_COUNT * MAX_CH];

        int[,] m_iXDefine   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[,] m_iYDefine   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[,] m_iXModule   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[,] m_iYModule   = new int [MAX_MASTER_COUNT,  MAX_CH];
        int[,] m_iModuleID  = new int [MAX_MASTER_COUNT , MAX_CH];


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool   m_bLoaded   ;   //Device Driver Loaded.
        public string m_sFNAddress;
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TIoWmx3()
        {
            //
            ioLib = new Io(IoWmx3);

            //Clear Var.
            //m_iSlaveTotal = 0    ;
            m_bLoaded     = false;
        }
        ~TIoWmx3() { }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool Init()
        {
            //Clear Var.
            //uint uiStatus     = 0;
            //uint uiRet        = 0;
            //uint uiRet1       = 0;
            //uint uiRet2       = 0;
            //int  lDBoardNo    = 0;
            //int  lDModulePos  = 0;
            //uint ModuleId     = 0;
            //int  iInputCount  = 0;
            //int  iOutputCount = 0;
            
            //
            m_bLoaded         = false;
            m_iModuleQty      = 0;

            try
            {
                string m_sWmx3Path = @"C:\Program Files\SoftServo\WMX3";
                if (!Directory.Exists(m_sWmx3Path))
                {
                    MsgBox.Error("[WMX3 IO] Directory Fail!!!");
                    return false;
                }

                // Create device.
                int ret = IoWmx3.CreateDevice(m_sWmx3Path, DeviceType.DeviceTypeNormal, 0xFFFFFFFF);
                if (ret != ErrorCode.None)
                {
                    MsgBox.Error("[WMX3 IO] Create Device Fail!!!");
                    return false;
                }

                // Set Device Name.
                IoWmx3.SetDeviceName("ControlIO");

                //// Get created device state.
                //Wmx3.GetAllDevices(ref Wmx3DevInfo);
                //m_iDeviceCnt = Convert.ToInt32(Wmx3DevInfo.Count);
                //if (m_iDeviceCnt <= 0)
                //{
                //    MsgBox.Error("[WMX3 IO] Cann't Load Device");
                //    return false;
                //}
                //

                //Start Communication.
                IoWmx3.StartCommunication(0xFFFFFFFF);
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxisWmx3. Open " + ex.ToString());
            }

            //
            ModuleInfo = new TModuleInfo[m_iDeviceCnt];
            
            //
            m_bLoaded = true;
            return true;
        }
        //------------------------------------------------------------------------
        public void Close()
        {
            try
            {
                if (m_iDeviceCnt <= 0) return;
                //

                // Stop Communication.
                IoWmx3.StopCommunication(0xFFFFFFFF);

                //Quit device.
                IoWmx3.CloseDevice();
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("TAxisWmx3. Close " + ex.ToString());
            }
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
        //------------------------------------------------------------------------
        public int Input    (int Addr         )
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
        public int  Output        (int Addr, int on)
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
        public int OutputRead(int Addr)
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
        public int OutInit(int Addr)
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
        //------------------------------------------------------------------------
        public void Update()
        {
            //
            if (!m_bLoaded) return;

            //uint ret;
            //int Idy       = 0;
            //int Idx       = 0;
            //int IdyRtn    = 0;
            //int moduleIdx = 0;

            //Read Input
            ioLib.GetInBytes(0x00, 1450, ref m_inData);
            //Console.WriteLine("MAX_INPUT_BUFF_SIZE(1450) Byte Get Input Memory Read");

            //Write Output
            // MAX_OUTPUT_BUFF_SIZE Byte Get Output Memory Write
            ioLib.SetOutBytes(0x00, 1450, m_outData);
            //Console.WriteLine("MAX_OUTPUT_BUFF_SIZE(1450) Byte Get Output Memory Write");


            // MAX_OUTPUT_BUFF_SIZE Byte Get Output Memory Read
            ioLib.GetOutBytes(0x01, 2, ref m_ReadYData);
            //Console.WriteLine("MAX_OUTPUT_BUFF_SIZE(1450) Byte Get Output Memory Read");


/* API Example --->>>
 
             // 1Byte Get Output Memory Read
             Console.WriteLine("1Byte Get Input Memory Read");
             Wmx3Lib_Io.GetInByte(0x00, ref gIn);

             // 1Byte Get Output Memory Write
             Console.WriteLine("1Byte Get Output Memory Write");
             Wmx3Lib_Io.GetOutByte(0x10, ref gout);

             // 1Byte Get Input  Memory Read
             Console.WriteLine("1Byte Get Input  Memory Read");
             Wmx3Lib_Io.SetOutByte(0x00, outData[0]);

             // 1Bit  Get Input  Memory Read
             Console.WriteLine("1Bit  Get Input  Memory Read");
             Wmx3Lib_Io.GetInBit(0x00, 0x00, ref inData[0]);

             // 1Bit  Get Output Memory Write
             Console.WriteLine("1Bit  Get Output Memory Write");
             Wmx3Lib_Io.SetOutBit(0x00, 0x00, outData[0]);
 
             // 1Bit  Get Output Memory Read
             Console.WriteLine("1Bit  Get Output Memory Read");
             Wmx3Lib_Io.GetOutBit(0x00, 0x00, ref outData[0]);
*/


            //
            //for (int i = 0; i < ModuleInfo.Length; i++)
            //{
            //    //
            //    moduleIdx = ModuleInfo[i].iModuleId;
            //
            //    //Read Input
            //    if (ModuleInfo[i].iCountInput > 0)
            //    {
            //        for (int offset = 0; offset < ModuleInfo[i].iCountInput / 16; offset++)
            //        {
            //            uint newState = 0;
            //            ret = CAXD.AxdiReadInportWord(moduleIdx, offset, ref newState);
            //            m_iReadData[Idx++] = Convert.ToUInt16(newState);
            //        }
            //    }
            //
            //    //Write Output
            //    if (ModuleInfo[i].iCountOutput > 0)
            //    {
            //        for (int offset = 0; offset < ModuleInfo[i].iCountOutput / 16; offset++)
            //        {
            //            uint newState = Convert.ToUInt16(m_iWriteData[Idy++]);
            //            ret = CAXD.AxdoWriteOutportWord(moduleIdx, offset, newState);
            //        }
            //
            //        //Read Write Value
            //        for (int offset = 0; offset < ModuleInfo[i].iCountOutput / 16; offset++)
            //        {
            //            uint newState = 0;
            //            ret = CAXD.AxdoReadOutportWord(moduleIdx, offset, ref newState);
            //            m_iReadYData[IdyRtn++] = Convert.ToUInt16(newState);
            //        }
            //    }
            //}
        }

        //------------------------------------------------------------------------
        public void GetDoInitData()
        {
            int iModuleNoY = 0;

            uint[] uiDataIO = new uint[MAX_CH];

            if (!m_bLoaded) return;

            uint ret;
            int Idy = 0;
            int Idx = 0;
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

            string[] sItem = new string[MAX_CH + 1];
            //Set Row/Col
            for (i = 0 ; i < MAX_MASTER_COUNT ; i++) 
            {
                sItem[0] = string.Format("{0,2:00}000 ~ {1,2:00}64F", i, i);
                for (int j = 0 ; j < MAX_CH ; j++) 
                {
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
            //for(int i=0; i<Grid.RowCount ;i++) 
            //{
            //    for(int j=0; j<Grid.ColumnCount-1 ;j++) 
            //    {
            //        if(isInput) m_iXDefine[i,j]  = Convert.ToInt32(Grid[j+1,i].Value.ToString());
            //        else        m_iYDefine[i,j]  = Convert.ToInt32(Grid[j+1,i].Value.ToString());
            //    }
            //}
            //Load(false);    
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(bool IsLoad)
        {
            ////Local Var.
            //string sPath = m_sFNAddress;
            //
            ////File Open.
            //int iFAccess = IsLoad ? (int)FileAccess.Read : (int)FileAccess.Write;
            //FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, (FileAccess)iFAccess);
            //
            //if (IsLoad)
            //{
            //    BinaryReader br = new BinaryReader(fp);
            //    if(br.PeekChar()<0) return;
            //    for (int i = 0; i < MAX_MASTER_COUNT; i++)
            //    {
            //        for (int j = 0; j < MAX_CH; j++)
            //        {
            //            m_iXDefine[i, j] = br.ReadInt32();
            //            m_iYDefine[i, j] = br.ReadInt32();
            //            if (m_iXDefine[i, j] >= 0) m_InModuleQty ++;
            //            if (m_iYDefine[i, j] >= 0) m_OutModuleQty++;
            //        }
            //    }
            //    br.Close();
            //}
            //else
            //{
            //    BinaryWriter wr = new BinaryWriter(fp);
            //    for (int i = 0; i < MAX_MASTER_COUNT; i++)
            //    {
            //        for (int j = 0; j < MAX_CH; j++)
            //        {
            //            wr.Write(m_iXDefine[i, j]);
            //            wr.Write(m_iYDefine[i, j]);
            //
            //        }
            //    }
            //    wr.Close();
            //}
        }
    }
}
