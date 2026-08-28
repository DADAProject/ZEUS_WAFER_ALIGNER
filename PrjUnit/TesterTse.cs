using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace InoModule
{

    /***************************************************************************/
    /* Class: TesterItemTse                                                    */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TesterItemTse
    {
        public int    m_nStationNum;
        public string m_strName    ;
        public string m_strUnit    ;
        public string m_strBais    ;
        public string m_strLower   ;
        public string m_strUpper   ;
        public string m_strApply   ;
        public string m_strClamp   ;
        public string m_strGain    ;
        public string m_strOffset  ;
        public string m_ItemCode   ;

        public TesterItemTse()
        {
            ResetTestItem();
        }
        public void ResetTestItem()
        {
            m_nStationNum = -1;
            m_strName     = "";
            m_strUnit     = "";
            m_strBais     = "";
            m_strLower    = "";
            m_strUpper    = "";
            m_strApply    = "";
            m_strClamp    = "";
            m_strGain     = "";
            m_strOffset   = "";
            m_ItemCode    = "";
        }
    }


    /***************************************************************************/
    /* Class: SocketDefineTse                                                  */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    class SocketDefineTse
    {
        #region Tester Enum
        public enum EN_PACKET
        {
            TLTESTER_PACKET_MAX_PACKET_SIZE             = 2048,
            TLTESTER_PACKET_MAX_DATA_SIZE               = 2032,        //2032 1016
            TLTESTER_PACKET_MAX_NAME_SIZE               = 150 ,        //150 
            TLTESTER_PACKET_MAX_ITEM_SIZE               = 150 ,
            TLTESTER_PACKET_MAX_BIN_TABLE_SIZE          = 2024,
            TLTESTER_PACKET_MAX_EOT_SIZE                = 1900,
            TLTESTER_PACKET_MAX_RECIPE_LIST_SIZE        = 2024, //2024
                                                        
            TLTESTER_PACKET_MAX_MULTI_TEST_COUNT        = 4,
            TLTESTER_PACKET_MAX_BIN_NAME_SIZE           = 15,
            TLTESTER_PACKET_MAX_CIEXY_SIZE              = 10,
                                                        
            TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT    = 8,

            TLTESTER_PACKET_MAX_CIEXY_CHANGE_RANK_COUNT = 10,
            TLTESTER_PACKET_MAX_CIEXY_CHANGE_RANK_NAME  = 10,
            TLTESTER_PACKET_MAX_CIEXY_CHANGE_RANK_POINT = 8,

            TLTESTER_PACKET_MAX_MULTI_EOT_SIZE          = 400,
        }

        public enum EN_CMD
        {
            // Command Define
            TLTESTER_CMD_RESERVED                      = 0x07,

            //-- LOT START
            TLTESTER_CMD_LOT_START                     = 5010,
            TLTESTER_CMD_LOT_START_ITEM_DATA           = 5011,
            TLTESTER_CMD_LOT_START_BIN_TABLE_DATA      = 5012,
            TLTESTER_CMD_LOT_START_RECIPE_LIST_DATA    = 5013,

            TLTESTER_CMD_LOT_START_CIEXY_RECIPE_CHANGE = 5014,

            //-- LOT END
            TLTESTER_CMD_LOT_END                       = 5020,

            //-- SOT & EOT
            TLTESTER_CMD_SOT                           = 5030,
            TLTESTER_CMD_EOT                           = 5040,

            //-- Time Synchronization
            TLTESTER_CMD_TIME_SYNCHRONIZATION          = 6010,

            //MSA MODE 추가 lhj
            TLTESTER_CMD_MSA_LOT_START                 = 5110,
            TLTESTER_CMD_MSA_LOT_END                   = 5120,
            TLTESTER_CMD_MSA_SOT                       = 5130,
            TLTESTER_CMD_MSA_EOT                       = 5140,

            // JETTING SOT & EOT(Multi Start Of Test, End Of Test)
            TLTESTER_CMD_MULTI_JETTING_SOT             = 5150,
            TLTESTER_CMD_MULTI_JETTING_EOT             = 5160,

            // 2호기 8MUX 용
            TLTESTER_CMD_MULTI_MUX_JETTING_SOT         = 5170,
            TLTESTER_CMD_MULTI_MUX_JETTING_EOT         = 5180,
        }

        public enum EN_ERROR_CODE
        {
            // ERROR CODE DEFINE
            TLTESTER_ERROR_CODE_SUCESS                                 = 0,     //Error 가 없을 경우
            TLTESTER_ERROR_CODE_LOT_START_NO_ITEM_FILE                 = 7011,  //Lot Start 시 Item File 없음 
            TLTESTER_ERROR_CODE_LOT_START_ALREADY_FAIL                 = 7012,  //현재 Tester의 상태가 Lot Start 가 된 상태임
            TLTESTER_ERROR_CODE_LOT_START_INIT_FAIL                    = 7013,  //Lot Start 초기화 실패
            TLTESTER_ERROR_CODE_LOT_START_OFF_FAIL                     = 7015,  //현재 Tester 의 상태가 Lot Start 된 상태가 아님
            TLTESTER_ERROR_CODE_MSA_LOT_START_NO_MASTER_DATA_FILE      = 7016,  //마스터 데이터 파일 없음
                                                                                
            TLTESTER_ERROR_CODE_LOT_START_RECIPE_CHANGE_NO_RECIPE_FILE = 7017,  //RECIPE 존재 X
            TLTESTER_ERROR_CODE_LOT_START_RECIPE_CHANGE_FAIL           = 7018,  //CIEXY RANK 적용 FAIL
            TLTESTER_ERROR_CODE_LOT_END_ALREADY_FAIL                   = 7021,  //현재 Tester 의 상태가 Lot End 된 상태임

            TLTESTER_ERROR_CODE_NOT_COMMAND                            = 7101,
            TLTESTER_ERROR_CODE_NOT_RESERVED                           = 7102,

            TLTESTER_ERROR_CODE_TIME_SYNCHRONIZATION_SET_ERROR         = 7201,  //Time Synchronization 오류

            TLTESTER_ERROR_CODE_TESTER_TIME_OUT                        = 7301,  //Time OUT 

            TLTESTER_ERROR_CODE_STATUS_ERROR                           = 7401,              // 테스터 상태 에러
            TLTESTER_ERROR_CODE_STATUS_ERROR_ALREADY_LOT_START,                 //테스터 상태 에러
            TLTESTER_ERROR_CODE_STATUS_ERROR_ALREADY_MSA_LOT_START,             //테스터 상태 에러

            TLTESTER_ERROR_CODE_EOT_COMMAND_CIE_NG_ALARM               = 7501,	//CIE NG ALARM
        }
        #endregion
        #region Tester Pack
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpCmdPackTse
        {
            public uint uiReserved ;
            public uint uiCommand  ;
            public uint uiSize     ;
            public uint uiErrorCode;

            public CTLTcpCmdPackTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                uiReserved = 7;
                uiCommand = 0;
                uiSize = 0;
                uiErrorCode = 0;
            }
        }
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)] // Auto
        public class CTLTcpPacketDataTse
        {
            internal CTLTcpCmdPackTse cmp;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_DATA_SIZE)]
            internal char[] szBuf;
            internal CTLTcpPacketDataTse()
            {
                cmp = new CTLTcpCmdPackTse();
                szBuf = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_DATA_SIZE];
            }
            internal void ResetData()
            {
                cmp.ResetData();
                Array.Clear(szBuf, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_DATA_SIZE);
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)] // Auto
        public class CTLTcpPacketDataTseRSP
        {
            internal CTLTcpCmdPackTse cmp;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_DATA_SIZE)]
            internal byte[] szBuf;
            internal CTLTcpPacketDataTseRSP()
            {
                cmp = new CTLTcpCmdPackTse();
                szBuf = new byte[(int)EN_PACKET.TLTESTER_PACKET_MAX_DATA_SIZE];
            }
            internal void ResetData()
            {
                cmp.ResetData();
                Array.Clear(szBuf, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_DATA_SIZE);
            }
        }

        #region [LOT START]
        // Lot Start
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpLotStartPackTse
        {
            internal int nTestCount;                                                     //총 측정 횟수
            internal int nTemp1;                                                         // 예비1
            internal int nTemp2;                                                         // 예비2
            internal int nCountClear;                                                    // Count Clear
            internal int nCountFileSave;                                                 // Count File Save

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szTestItemName   = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Test Item File Name
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szDescription    = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Description
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szLotName        = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Lot Name
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szOperator       = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Operator
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szPKG            = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // PKG
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szCHIP           = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // CHIP
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szIV             = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // IV
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szRecipe         = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Recipe
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szDPMCNO         = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // DPMCNO
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szComment        = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Comment
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szIdentification = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Identification
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szDesignation    = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                             // Designation
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            internal char[] szUserDefine     = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                                         	 // UserDefine

            internal CTLTcpLotStartPackTse()
            {
                ResetData();
            }
            internal void ResetData()
            {
                nTestCount = 0;
                nTemp1 = 0;
                nTemp2 = 0;
                nCountClear = 0;
                nCountFileSave = 0;

                Array.Clear(szTestItemName  , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szDescription   , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szLotName       , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szOperator      , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szPKG           , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szCHIP          , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szIV            , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szRecipe        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szDPMCNO        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szComment       , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szIdentification, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szDesignation   , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szUserDefine    , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpLotStartTseRSP
        {
            public int nTotalItemNum;                                          // 총 전송될 Item 개수
            public int nTotalBinNum ;                                          // 총 전송될 Bin Table 개수
            public int nTemp1       ;                                          // 예비1
            public int nTemp2       ;                                          // 예비2

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE)]
            public char[] szCenterWX;                                          // WX Data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE)]
            public char[] szCenterWY;     	                                   // WY Data

            public CTLTcpLotStartTseRSP()
            {
                nTotalItemNum = 0;
                nTotalBinNum  = 0;
                nTemp1        = 0;
                nTemp2        = 0;

                szCenterWX = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
                szCenterWY = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
            }
            public void ResetData()
            {
                nTotalItemNum = 0;
                nTotalBinNum  = 0;
                nTemp1        = 0;
                nTemp2        = 0;
                Array.Clear(szCenterWX, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE);
                Array.Clear(szCenterWY, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpLotStartItemDataTse
        {
            public int nCurrentItemIndex;
            public int nTotalItemNum    ;
            public CTLTcpLotStartItemDataTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nCurrentItemIndex = 0;
                nTotalItemNum     = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpLotStartItemDataTseRSP
        {
            public int nCurrentItemIndex;                   // 전송 ITEM
            public int nTotalItemNum    ;                   // 총 전송할 ITEM
            public int nStationNum      ;                   // Station No

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemName;                         // Item Name
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemUnit;                         // Item Measure Unit
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemBias;                         // Item Bias (Apply Value)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemLower;                        // Item Lower Data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemUpper;                        // Item Upper Data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemApplyTime;                    // Item Apply Time
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemClamp;                        // Item Clamp
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemGain;                         // Item Gain
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemOffset;                       // Item Offset
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szItemCode;                         // Item Code
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szTemp1;                            // Temp1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szTemp2;                            // Temp2
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE)]
            public char[] szTemp3;   			   		    // Temp3    

            public CTLTcpLotStartItemDataTseRSP()
            {
                nCurrentItemIndex = 0;
                nTotalItemNum = 0;
                nStationNum = 0;

                szItemName      = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemUnit      = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemBias      = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemLower     = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemUpper     = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemApplyTime = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemClamp     = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemGain      = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemOffset    = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szItemCode      = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szTemp1         = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szTemp2         = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
                szTemp3         = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE];
            }
            public void ResetData()
            {
                nCurrentItemIndex = 0;
                nTotalItemNum     = 0;
                nStationNum       = 0;

                Array.Clear(szItemName     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemUnit     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemBias     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemLower    , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemUpper    , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemApplyTime, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemClamp    , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemGain     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemOffset   , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szItemCode     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szTemp1        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szTemp2        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
                Array.Clear(szTemp3        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_ITEM_SIZE);
            }
        }


        #endregion
        #region [LOT START BIN DATA]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpLotStartBinTableDataTse
        {
            public int nCurrentBinIndex;
            public CTLTcpLotStartBinTableDataTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nCurrentBinIndex = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpLotStartBinTableDataTseRSP
        {
            public int nCurrentBinIndex;
            public int nTotalBinNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_TABLE_SIZE)]
            public char[] szBinTableData;
            public CTLTcpLotStartBinTableDataTseRSP()
            {
                nCurrentBinIndex = 0;
                nTotalBinNum = 0;
                szBinTableData = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_TABLE_SIZE];
            }
            public void ResetData()
            {
                nCurrentBinIndex = 0;
                nTotalBinNum = 0;
                Array.Clear(szBinTableData, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_TABLE_SIZE);
            }
        }

        #endregion
        #region [LOT START RECIPE DATA]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpLotStartRecipeListTse
        {
            public int nCurrentRecipeListIndex;
            public CTLTcpLotStartRecipeListTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nCurrentRecipeListIndex = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpLotStartRecipeListTseRSP
        {
            public int nTotalRecipeListCnt    ;
            public int nCurrentRecipeListIndex;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_RECIPE_LIST_SIZE)]
            public char[] szRecipeList;
            public CTLTcpLotStartRecipeListTseRSP()
            {
                nTotalRecipeListCnt     = 0;
                nCurrentRecipeListIndex = 0;
                szRecipeList            = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_RECIPE_LIST_SIZE];
            }
            public void ResetData()
            {
                nTotalRecipeListCnt     = 0;
                nCurrentRecipeListIndex = 0;
                Array.Clear(szRecipeList, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_RECIPE_LIST_SIZE);
            }
        }
        #endregion

        #region [LOT END]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpLotEndPakTse
        {
            public int nTestCount;
            public CTLTcpLotEndPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nTestCount = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpLotEndPakTseRSP
        {
            public int nTestCount;
            public CTLTcpLotEndPakTseRSP()
            {
                ResetData();
            }
            public void ResetData()
            {
                nTestCount = 0;
            }
        }
        #endregion

        #region [SOT]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpSOTPakTse
        {
            public int nDut1   ;                                                           // Handler 의 Station 1의 DUT 정보 , 
            public int nDut2   ;                                                           // Handler 의 Station 2의 DUT 정보 
            public int nIndexNo;                                                           // Handler 의 Index 정보
            public int nTemp1  ;                                                           // 예비1
            public int nTemp2  ;                                                           // 예비2
            public int nTemp3  ;                                                           // 예비3
            public int nTemp4  ;                                                           // 예비4
            public int nTemp5  ;     													   // 예비5
            public CTLTcpSOTPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nDut1    = 0;
                nDut2    = 0;
                nIndexNo = 0;
                nTemp1   = 0;
                nTemp2   = 0;
                nTemp3   = 0;
                nTemp4   = 0;
                nTemp5   = 0;
            }
        }
        #endregion
        #region [EOT]
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpEOTPakTse
        {
            public int  nBinNo    ;                                                 // 측정된 Bin 정보
            public int  nTestCount;                                                 // 측정된 Test Count
            public int  nSubBinNo ;                                                 // 측정된 Sub Bin 정보
            public bool bGood     ;                                                 // GOOD/ NG 정보 기입
            public int  nTemp2    ;                                                 // 예비2
            public int  nTemp3    ;                                                 // 예비3
            public int  nTemp4    ;                                                 // 예비4
            public int  nTemp5    ;                                                 // 예비5

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_EOT_SIZE)]
            public char[] szEotData;	                                        	// 측정 Raw 데이터 
            public CTLTcpEOTPakTse()
            {
                nBinNo     = 0;
                nTestCount = 0;
                nSubBinNo  = 0;
                bGood      = false;
                nTemp2     = 0;
                nTemp3     = 0;
                nTemp4     = 0;
                nTemp5     = 0;
                szEotData  = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_EOT_SIZE];
            }
            public void ResetData()
            {
                nBinNo     = 0;
                nTestCount = 0;
                nSubBinNo  = 0;
                bGood      = false;
                nTemp2     = 0;
                nTemp3     = 0;
                nTemp4     = 0;
                nTemp5     = 0;
                Array.Clear(szEotData, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_EOT_SIZE);
            }
        }
        #endregion

        #region [V1.563]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpTimeSyncTse
        {
            public int nYear  ;             // Year
            public int nMonth ;             // Month
            public int nDay   ;             // Day
            public int nHour  ;             // Hour
            public int nMinute;             // Minute
            public int nSecond;             // Second

            public int nTemp1 ;             // Temp
            public int nTemp2 ;             // Temp
            public int nTemp3 ;     	 	// Temp
            public CTLTcpTimeSyncTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nYear   = 0;
                nMonth  = 0;
                nDay    = 0;
                nHour   = 0;
                nMinute = 0;
                nSecond = 0;

                nTemp1  = 0;
                nTemp2  = 0;
                nTemp3  = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpTimeSyncTseRSP
        {
            public int nYear  ;      // Year
            public int nMonth ;      // Month
            public int nDay   ;      // Day
            public int nHour  ;      // Hour
            public int nMinute;      // Minute
            public int nSecond;      // Second

            public int nTemp1 ;      // Temp
            public int nTemp2 ;      // Temp
            public int nTemp3 ;      // Temp
            public CTLTcpTimeSyncTseRSP()
            {
                ResetData();
            }
            public void ResetData()
            {
                nYear   = 0;
                nMonth  = 0;
                nDay    = 0;
                nHour   = 0;
                nMinute = 0;
                nSecond = 0;

                nTemp1  = 0;
                nTemp2  = 0;
                nTemp3  = 0;
            }
        }
        #endregion

        #region [MSA MODE?]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpMSALotStartPakTse
        {
            public int nTestCount    ;                                                     //총 측정 횟수
            public int nTemp1        ;                                                     // 예비1
            public int nTemp2        ;                                                     // 예비2
            public int nCountClear   ;                                                     // Count Clear
            public int nCountFileSave;                                                     // Count File Save
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szTestItemName    = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                 // Test Item File Name
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szDescription    = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // Description
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szLotName        = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // Lot Name
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szOperator       = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // Operator
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szPKG            = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // PKG
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szCHIP           = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // CHIP
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szIV             = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // IV
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szRecipe         = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // Recipe
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szDPMCNO         = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // DPMCNO
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szComment        = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // Comment
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szIdentification = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // Identification
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szDesignation    = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // Designation
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szUserDefine     = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];                  // UserDefine
            public int nFilms     ;                                                     //현재 선택 Film
            public int nTotalPoint;                                                     //전체 측정 POINT
            public int nAutoTest  ;														//1: Auto 0:Manual

            public CTLTcpMSALotStartPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nTestCount     = 0;
                nTemp1         = 0;
                nTemp2         = 0;
                nCountClear    = 0;
                nCountFileSave = 0;

                Array.Clear(szTestItemName  , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szDescription   , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szLotName       , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szOperator      , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szPKG           , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szCHIP          , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szIV            , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szRecipe        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szDPMCNO        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szComment       , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szIdentification, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szDesignation   , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                Array.Clear(szUserDefine    , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
                nFilms = 0;
                nTotalPoint = 0;
                nAutoTest = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpMSALotStartTseRSP
        {
            public int nTotalItemNum;                                          // 총 전송될 Item 개수
            public int nTotalBinNum ;                                          // 총 전송될 Bin Table 개수
            public int nTemp1       ;                                          // 예비2
            public int nTemp2       ;		      							   // 예비3
            public CTLTcpMSALotStartTseRSP()
            {
                ResetData();
            }
            public void ResetData()
            {
                nTotalItemNum = 0;
                nTotalBinNum  = 0;
                nTemp1        = 0;
                nTemp2        = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpMSALotEndPakTse
        {
            public int nTestCount;
            public CTLTcpMSALotEndPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nTestCount = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpMSALotEndPakTseRSP
        {
            public int nTestCount;          // 총 측정된 Test Count
            public int nTemp1    ;
            public int nTemp2    ;
            public int nTemp3    ;
            public int nTemp4    ;
            public int nTemp5    ;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE)]
            public char[] szMSAMeasurePath = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE];
            public CTLTcpMSALotEndPakTseRSP()
            {
                ResetData();
            }
            public void ResetData()
            {
                nTestCount = 0;
                nTemp1     = 0;
                nTemp2     = 0;
                nTemp3     = 0;
                nTemp4     = 0;
                nTemp5     = 0;

                Array.Clear(szMSAMeasurePath, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpMSASOTPakTse
        {
            public int nDut1   ;                                                           // Handler 의 Station 1의 DUT 정보 , 
            public int nDut2   ;                                                           // Handler 의 Station 2의 DUT 정보 
            public int nIndexNo;                                                           // Handler 의 Index 정보
            public int nCol    ;
            public int nRow    ;
            public int nTemp1  ;                                                           // 예비1
            public int nTemp2  ;                                                           // 예비2
            public int nTemp3  ;                                                           // 예비3
            public int nTemp4  ;                                                           // 예비4
            public int nTemp5  ;														   // 예비5

            public CTLTcpMSASOTPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nDut1    = 0;
                nDut2    = 0;
                nIndexNo = 0;
                nCol     = 0;
                nRow     = 0;
                nTemp1   = 0;
                nTemp2   = 0;
                nTemp3   = 0;
                nTemp4   = 0;
                nTemp5   = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CTLTcpMSAEOTPakTse
        {
            public int  nBinNo     ;                                               // 측정된 Bin 정보
            public int  nTestCount ;                                               // 측정된 Test Count
            public int  nSubBinNo  ;                                               // 측정된 Sub Bin 정보
            public bool bTestResult;                                               // 테스트 결과 PASS or FAIL
            public int  nTemp1     ;                                               // 예비2
            public int  nTemp2     ;                                               // 예비3
            public int  nTemp3     ;                                               // 예비4
            public int  nTemp4     ;                                               // 예비5

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_EOT_SIZE)]
            char[] szEotData = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_EOT_SIZE];					// 측정 Raw 데이터 

            public CTLTcpMSAEOTPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                nBinNo      = 0;
                nTestCount  = 0;
                nSubBinNo   = 0;
                bTestResult = false;
                nTemp1      = 0;
                nTemp2      = 0;
                nTemp3      = 0;
                nTemp4      = 0;
                Array.Clear(szEotData, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_EOT_SIZE);
            }
        }
        #endregion

        #region [Multi SOT]

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpMultiSOTPakTse
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public bool[] m_bDut = new bool[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                  // Handler 의 Station 1의 DUT 정보 , 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] m_nRow  = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                   // Row 좌표
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] m_nCol  = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                   // Col 좌표

            public int nTemp1;                                                         // 예비1
            public int nTemp2;                                                         // 예비2
            public int nTemp3;                                                         // 예비3
            public int nTemp4;                                                         // 예비4
            public int nTemp5;														   // 예비5

            public CTLTcpMultiSOTPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                for (int i = 0; i < (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT; i++)
                {
                    m_bDut[i] = false;
                    m_nRow[i] = 0;
                    m_nCol[i] = 0;
                }

                nTemp1 = 0;
                nTemp2 = 0;
                nTemp3 = 0;
                nTemp4 = 0;
                nTemp5 = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpMultiEOTPakTse
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] nBinNo       = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                                  // 측정된 Bin 정보
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] nSubBinNo    = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                                  // 측정된 Sub Bin 정보
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public bool[] bTestResult = new bool[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                                 // 측정된 테스트 결과 PASS or FAIL
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE)]
            public char[,] szBinName  = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT,(int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE)]
            public char[,] fWX        = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT, (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];			// WX Data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE)]
            public char[,] fWY       = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT, (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];			// WY Data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] nCIERank    = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                                     // 예비1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] nTemp2      = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                                     // 예비2
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] nTemp3      = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                                     // 예비3
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT)]
            public int[] nTemp4      = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];                                     // 예비4
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_EOT_SIZE)]
            public char[,] szEotData = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_EOT_SIZE]; 
            public CTLTcpMultiEOTPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                Array.Clear(nBinNo     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
                Array.Clear(nSubBinNo  , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
                Array.Clear(bTestResult, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
                Array.Clear(nCIERank   , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
                Array.Clear(nTemp2     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
                Array.Clear(nTemp3     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
                Array.Clear(nTemp4     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
                                                                
                Array.Clear(szBinName  , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE );
                Array.Clear(fWX        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE    );
                Array.Clear(fWY        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE    );
                Array.Clear(szEotData  , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_EOT_SIZE);
            }
        }
        #endregion

        #region [Multi MUX SOT]
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpMultiMuxSOTPakTse
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public bool[] m_bDut = new bool[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                  // Handler 의 Station 1의 DUT 정보 , 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] m_nRow  = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                   // Row 좌표
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] m_nCol  = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                   // Col 좌표

            public int nTemp1;                                                             // 예비1
            public int nTemp2;                                                             // 예비2
            public int nTemp3;                                                             // 예비3
            public int nTemp4;                                                             // 예비4
            public int nTemp5;															   // 예비5
            public CTLTcpMultiMuxSOTPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                for (int i = 0; i < (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT; i++)
                {
                    m_bDut[i] = false;
                    m_nRow[i] = 0;
                    m_nCol[i] = 0;
                }

                nTemp1 = 0;
                nTemp2 = 0;
                nTemp3 = 0;
                nTemp4 = 0;
                nTemp5 = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public class CTLTcpMultiMuxEOTPakTse
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] nBinNo       = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                                     // 측정된 Bin 정보
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] nSubBinNo    = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                                     // 측정된 Sub Bin 정보
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public bool[] bTestResult = new bool[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                                    // 측정된 테스트 결과 PASS or FAIL
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE)]
            public char[,] szBinName  = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT, (int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE];             // BIN name 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE)]
            public char[,] fWX        = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT, (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];                // WX Data 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE)]
            public char[,] fWY       = new char[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT, (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];				 // WY Data 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] nCIERank    = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                                      // CIE Data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] nTemp2      = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                                      // 예비2
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] nTemp3      = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];                                      // 예비3
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT)]
            public int[] nTemp4      = new int[(int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];     		 						    // 예비4

            public CTLTcpMultiMuxEOTPakTse()
            {
                ResetData();
            }
            public void ResetData()
            {
                Array.Clear(nBinNo     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
                Array.Clear(nSubBinNo  , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
                Array.Clear(bTestResult, 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
                Array.Clear(szBinName  , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE);
                Array.Clear(fWX        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE   );
                Array.Clear(fWY        , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE   );
                Array.Clear(nCIERank   , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
                Array.Clear(nTemp2     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
                Array.Clear(nTemp3     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
                Array.Clear(nTemp4     , 0, (int)EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
            }

        }
            #endregion

        #endregion
    }

    /***************************************************************************/
    /* Class: XTesterDataTse                                                   */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class XTesterDataTse
    {
        public XTesterDataTse()
        {
            m_lstTesterRecipe.Clear();
            m_lstTesterItem.Clear();

        }
        //Recipe
        public List<string> m_lstTesterRecipe = new List<string>();
        public int m_nCurRecipeListIndex;
        public int m_nCUrRecipeListCnt;

        //Item
        public List<TesterItemTse> m_lstTesterItem = new List<TesterItemTse>();
        public int m_nCurItemIndex;
        public int m_nCurItemTotalCnt;

        // Multi SOT & EOT
        public int[]   m_nBinNo             = new int [(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];
        public int[]   m_nCIERank           = new int [(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];
        public int[]   m_nSubBinNo          = new int [(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];
        public bool[]  m_bTestResult        = new bool[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT];
        public char[,] m_szBinName          = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE];
                                            
        public char[,] m_fWX                = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
        public char[,] m_fWY                = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
        public string m_strMultiEOTData;

        // Multi Mux SOT & EOT
        public int[]  m_nMultiMuxBinNo      = new int [(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];
        public int[]  m_nMultiMuxCIERank    = new int [(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];
        public int[]  m_nMultiMuxSubBinNo   = new int [(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];
        public bool[] m_bMultiMuxTestResult = new bool[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT];
        public char[] m_szMultiMuxBinName   = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE];

        public char[] m_fMultiMuxWX         = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
        public char[] m_fMultiMuxWY         = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
        public string m_strMultiMuxEOTData;

        public char[] m_szCenterWX          = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
        public char[] m_szCenterWY          = new char[(int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE];
    }

    public class TTesterTse
    {
        //AsyncSocketListener server;
        AsyncSocketServer server;
        public XTesterDataTse TesterData;


        private bool   m_bTesterError       = false;
        private string m_strTesterErrorCode = "";

        public bool RecvEotComplete = false;

        public TTesterTse()
        {
            TesterData        = new XTesterDataTse();
        }

        public void intt(int iPort)
        {
            server            = new AsyncSocketServer (iPort);
            server.OnRecieve += new AsyncSocketServer.OnRecieveMessage(OnReciveSocket);

        }

        public void ResetTesterRecipeInfo()
        {
            TesterData.m_nCurRecipeListIndex = 0;
            TesterData.m_nCUrRecipeListCnt   = 0;
            TesterData.m_lstTesterRecipe.Clear();
        }

        public void ResetTesterItemInfo()
        {
            TesterData.m_nCurItemIndex     = 0;
            TesterData.m_lstTesterItem.Clear();
        }

        public void ResetTesterErrorInfor()
        {
            m_bTesterError = false;
            m_strTesterErrorCode = "";
        }

        public void ResetTesterMultiResultInfo()
        {
            Array.Clear(TesterData.m_nBinNo     , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
            Array.Clear(TesterData.m_nSubBinNo  , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
            Array.Clear(TesterData.m_bTestResult, 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);
            Array.Clear(TesterData.m_szBinName  , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE);
            Array.Clear(TesterData.m_fWX        , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE);
            Array.Clear(TesterData.m_fWY        , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE);
            Array.Clear(TesterData.m_nCIERank   , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT);

            TesterData.m_strMultiEOTData = "";
        }

        public void ResetTesterMultiMuxResultInfo()
        {
            Array.Clear(TesterData.m_nMultiMuxBinNo     , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
            Array.Clear(TesterData.m_nMultiMuxSubBinNo  , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
            Array.Clear(TesterData.m_bMultiMuxTestResult, 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);
            Array.Clear(TesterData.m_szMultiMuxBinName  , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_BIN_NAME_SIZE);
                                                                                                       
            Array.Clear(TesterData.m_fMultiMuxWX        , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE);
            Array.Clear(TesterData.m_fMultiMuxWY        , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT * (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE);
            Array.Clear(TesterData.m_nMultiMuxCIERank   , 0, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT);

            TesterData.m_strMultiMuxEOTData = "";
        }


        private void OnReciveSocket(object sender, int iLen, byte[] data)
        //private void OnReciveSocket(object sender, string data)
        {
            SocketDefineTse.CTLTcpPacketDataTseRSP cmData = new SocketDefineTse.CTLTcpPacketDataTseRSP();

            byte[] Temp = new byte[2048];
            //Temp = GetStringToByteArray(data, 2048);

            cmData = (SocketDefineTse.CTLTcpPacketDataTseRSP)FNC.ByteToStruct(data, typeof(SocketDefineTse.CTLTcpPacketDataTseRSP));
            String disp = "CMD : " + cmData.cmp.uiCommand.ToString();
            //Console.WriteLine(disp);

            disp = "Error Code : " + cmData.cmp.uiErrorCode.ToString();
            //Console.WriteLine(disp);

            disp = "Size : " + cmData.cmp.uiSize.ToString();
            //Console.WriteLine(disp);

            //char[] readchar = cmData.szBuf;
            //cmData.szBuf.CopyTo(readchar, 0);

            switch(cmData.cmp.uiCommand)
            {
                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_START:
                    //Recv Lot Start
                    Recv_Lot_Start(cmData);
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_START_ITEM_DATA:
                    //Recv Item Data
                    Recv_Lot_Start_Item_Data(cmData);
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_START_BIN_TABLE_DATA:
                    //Recv Bin Table Data
                    Recv_Lot_Start_Bin_Table_Data(cmData);
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_START_RECIPE_LIST_DATA:
                    //Recv Recipe List Data
                    Recv_Lot_Start_Recipe_List_Data(cmData);
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_END:
                    //Recv Lot End 
                    Recv_Lot_End(cmData);
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_EOT:
                    //Recv EOT (Not Use)
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_MSA_LOT_START:
                    //Recv MSA Lot Start(Not Use) 
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_MSA_LOT_END:
                    //Recv MSA Lot END(Not Use)
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_MSA_EOT:
                    //Recv MSA EOT(Not Use)
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_MULTI_JETTING_EOT:
                    //Recv multi jetting eot
                    Recv_Muti_EOT(cmData);
                    break;

                case (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_MULTI_MUX_JETTING_EOT:
                    //Recv multi Mux jetting eot
                    Recv_Muti_Mux_EOT(cmData);
                    break;

            }

        }

        #region [Recv Command]
        private void Recv_Lot_Start(SocketDefineTse.CTLTcpPacketDataTseRSP packet)
        {
            if(packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_SUCESS)
            {
                byte[] temp = new byte[Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartTseRSP))];
                //temp = GetCharArrayToByteArray(packet.szBuf, packet.szBuf.Length);

                SocketDefineTse.CTLTcpLotStartTseRSP LotStartData = new SocketDefineTse.CTLTcpLotStartTseRSP();
                LotStartData = (SocketDefineTse.CTLTcpLotStartTseRSP)FNC.ByteToStruct(packet.szBuf, typeof(SocketDefineTse.CTLTcpLotStartTseRSP));

                TesterData.m_nCurItemTotalCnt = LotStartData.nTotalItemNum;

                for(int i = 0; i < (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_CIEXY_SIZE; i ++)
                {
                    TesterData.m_szCenterWX[i] = LotStartData.szCenterWX[i];
                    TesterData.m_szCenterWY[i] = LotStartData.szCenterWY[i];
                }
                string disp = "(LOT START RSP) - CenterWX: " + new String(TesterData.m_szCenterWX) + "CenterWY: " + new String(TesterData.m_szCenterWY);
                //Console.WriteLine(disp);


                //Log
                string strTrace;
                strTrace = String.Format("[Tester] Receive : LotStart(%d, %d, %s, %s)", LotStartData.nTotalItemNum, LotStartData.nTotalBinNum, LotStartData.szCenterWX.ToString(), LotStartData.szCenterWY.ToString());
                Log(strTrace);
            }
            else // Error ocure
            {
                m_bTesterError = true;
                if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_NO_ITEM_FILE)
                {
                    m_strTesterErrorCode = "[Tester] Receive: Lotstart Error - Tester Lot Start No Item File";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_ALREADY_FAIL)
                {
                    m_strTesterErrorCode = "[Tester] Receive: Lotstart Error - Tester Lot Start Already Fail.";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_INIT_FAIL)
                {
                    m_strTesterErrorCode = "[Tester] Receive: Lotstart Error - Tester Lot Start Init Fail.";
                }
                else
                {
                    m_strTesterErrorCode = "[Tester] Receive: Lotstart Error - Tester Lot Start Not Define.";
                }
                Log(m_strTesterErrorCode);
            }
        }

        // 측정 Item 가져오기.
        private void Recv_Lot_Start_Item_Data(SocketDefineTse.CTLTcpPacketDataTseRSP packet)
        {
            if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_SUCESS)
            {
                byte[] temp = new byte[Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartItemDataTseRSP))];
                //temp = GetCharArrayToByteArray(packet.szBuf, packet.szBuf.Length);

                SocketDefineTse.CTLTcpLotStartItemDataTseRSP ItemData = new SocketDefineTse.CTLTcpLotStartItemDataTseRSP();
                ItemData = (SocketDefineTse.CTLTcpLotStartItemDataTseRSP)FNC.ByteToStruct(packet.szBuf, typeof(SocketDefineTse.CTLTcpLotStartItemDataTseRSP));

                TesterItemTse xTestItem = new TesterItemTse();
                xTestItem.ResetTestItem();

                xTestItem.m_nStationNum = ItemData.nStationNum;
                xTestItem.m_strName     = ItemData.szItemName     .ToString();
                xTestItem.m_strUnit     = ItemData.szItemUnit     .ToString();
                xTestItem.m_strBais     = ItemData.szItemBias     .ToString();
                xTestItem.m_strLower    = ItemData.szItemLower    .ToString();
                xTestItem.m_strUpper    = ItemData.szItemUpper    .ToString();
                xTestItem.m_strApply    = ItemData.szItemApplyTime.ToString();
                xTestItem.m_strClamp    = ItemData.szItemClamp    .ToString();
                xTestItem.m_strGain     = ItemData.szItemGain     .ToString();
                xTestItem.m_strOffset   = ItemData.szItemOffset   .ToString();
                xTestItem.m_ItemCode    = ItemData.szItemCode     .ToString();

                TesterData.m_lstTesterItem.Add(xTestItem);
                //Log
                string strTrace;
                strTrace = String.Format("[Tester] Receive :Item Data(%d, %d, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)", ItemData.nCurrentItemIndex, xTestItem.m_nStationNum
                    , xTestItem.m_strName, xTestItem.m_strUnit, xTestItem.m_strBais, xTestItem.m_strLower, xTestItem.m_strUpper
                    , xTestItem.m_strApply, xTestItem.m_strClamp, xTestItem.m_strGain, xTestItem.m_strOffset, xTestItem.m_ItemCode);
                Log(strTrace);
            }
            else // Error ocure
            {
                m_bTesterError = true;
                if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_OFF_FAIL)
                {
                    m_strTesterErrorCode = "[Tester] Receive: Lotstart Item Data Error - Tester Lot Start Off Fail";
                }
                else
                {
                    m_strTesterErrorCode = "[Tester] Receive: Lotstart Item Data Error - Tester Lot Start Not Define.";
                }
                Log(m_strTesterErrorCode);
            }
        }

        private void Recv_Lot_Start_Bin_Table_Data(SocketDefineTse.CTLTcpPacketDataTseRSP packet)
        {
            // Not Use
        }

        private void Recv_Lot_Start_Recipe_List_Data(SocketDefineTse.CTLTcpPacketDataTseRSP packet)
        {
            if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_SUCESS)
            {
                byte[] temp = new byte[Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartRecipeListTseRSP))];
                // temp = GetCharArrayToByteArray(packet.szBuf, packet.szBuf.Length);

                SocketDefineTse.CTLTcpLotStartRecipeListTseRSP RecipeListData = new SocketDefineTse.CTLTcpLotStartRecipeListTseRSP();
                RecipeListData = (SocketDefineTse.CTLTcpLotStartRecipeListTseRSP)FNC.ByteToStruct(packet.szBuf, typeof(SocketDefineTse.CTLTcpLotStartRecipeListTseRSP));

                // GetCount
                TesterData.m_nCUrRecipeListCnt = RecipeListData.nTotalRecipeListCnt;

                string strList =  new String(RecipeListData.szRecipeList);
                char delimiter = ';';
                string[] substrings = strList.Split(delimiter);
                TesterData.m_lstTesterRecipe.Clear();

                foreach (var substring in substrings)
                    TesterData.m_lstTesterRecipe.Add(substring.ToString());

                string disp;
                disp = "RecipeCount: " + TesterData.m_nCUrRecipeListCnt.ToString();
                //Server.SystemLog.SetLog(1, disp);
                //Console.WriteLine(disp);
                for (int i = 0; i < TesterData.m_nCUrRecipeListCnt; i++)
                {
                    disp = "RecipeList: " + TesterData.m_lstTesterRecipe[0];
                    //Console.WriteLine(disp);
                }


                string strTrace;
                strTrace = String.Format("[Tester] Receive :Item Data(%d, %s)", RecipeListData.nCurrentRecipeListIndex, strList);
                Log(strTrace);
            }
            else // Error ocure
            {
                m_bTesterError       = true;
                m_strTesterErrorCode = "Tester Lot Start Not Define.";
                Log(m_strTesterErrorCode);
            }
        }

        private void Recv_Lot_End(SocketDefineTse.CTLTcpPacketDataTseRSP packet)
        {
            if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_SUCESS)
            {
                byte[] temp = new byte[Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotEndPakTseRSP))];
                //temp = GetCharArrayToByteArray(packet.szBuf, packet.szBuf.Length);

                SocketDefineTse.CTLTcpLotEndPakTseRSP LotEndData = new SocketDefineTse.CTLTcpLotEndPakTseRSP();
                LotEndData = (SocketDefineTse.CTLTcpLotEndPakTseRSP)FNC.ByteToStruct(packet.szBuf, typeof(SocketDefineTse.CTLTcpLotEndPakTseRSP));

                string disp;
                disp = "Response - LOT END ";
                //Console.WriteLine(disp);
                

                string strTrace;
                strTrace = String.Format("[Tester] Receive :Lot End(%d)", LotEndData.nTestCount);
                Log(strTrace);
            }
            else // Error ocure
            {
                m_bTesterError = true;
                if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_END_ALREADY_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot End Already Fail";
                }
                else
                {
                    m_strTesterErrorCode = "Tester Lot Start Not Define.";
                }
                Log(m_strTesterErrorCode);
            }
        }

        private void Recv_Muti_EOT(SocketDefineTse.CTLTcpPacketDataTseRSP packet)
        {
            if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_SUCESS)
            {
                byte[] temp = new byte[Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpMultiEOTPakTse))];
                //temp = GetCharArrayToByteArray(packet.szBuf, packet.szBuf.Length);

                SocketDefineTse.CTLTcpMultiEOTPakTse EotData = new SocketDefineTse.CTLTcpMultiEOTPakTse();
                EotData = (SocketDefineTse.CTLTcpMultiEOTPakTse)FNC.ByteToStruct(packet.szBuf, typeof(SocketDefineTse.CTLTcpMultiEOTPakTse));

                string disp= "";
                for ( int i = 0; i < (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT; i++)
                {
                    TesterData.m_nBinNo[i]       = EotData.nBinNo[i];
                    disp = "Bin No" + i.ToString() + " : " + TesterData.m_nBinNo[i].ToString();
                    
                    TesterData.m_nSubBinNo[i]    = EotData.nSubBinNo[i];
                    disp += ", Sub Bin No" + i.ToString() + " : " + TesterData.m_nSubBinNo[i].ToString();

                    TesterData.m_bTestResult[i]  = EotData.bTestResult[i];
                    disp += ", Test Result" + i.ToString() + " : " + TesterData.m_bTestResult[i].ToString();

                    TesterData.m_nCIERank[i]     = EotData.nCIERank[i];
                    disp += ", CIE Rank" + i.ToString() + " : " + TesterData.m_nCIERank[i].ToString();
                    //Console.WriteLine(disp);
                }

                StringBuilder sb = new StringBuilder();

                TesterData.m_szBinName = EotData.szBinName;
                sb.Append(TesterData.m_szBinName);
                disp = "Bin Name : " + sb.ToString();
                //Console.WriteLine(disp);

                sb.Clear();
                TesterData.m_fWX = EotData.fWX;
                sb.Append(TesterData.m_fWX);
                disp = "WX : " + sb.ToString();
                //Console.WriteLine(disp);

                sb.Clear();
                TesterData.m_fWY = EotData.fWY;
                sb.Append(TesterData.m_fWY);
                disp = "WY : " + sb.ToString();
                //Console.WriteLine(disp);

                string strTrace;
                strTrace = "[Tester] Receive : Multi EOT";
                Log(strTrace);
            }
            else // Error ocure
            {
                m_bTesterError = true;
                if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_NO_ITEM_FILE)
                {
                    m_strTesterErrorCode = "Tester Lot Start No Item File";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_ALREADY_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot Start Already Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_INIT_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot Start Init Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_OFF_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot Start Off Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_END_ALREADY_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot End Already Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_NOT_COMMAND)
                {
                    m_strTesterErrorCode = "Tester Not Command";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_NOT_RESERVED)
                {
                    m_strTesterErrorCode = "Tester Not Reserved";
                }
                else
                {
                    m_strTesterErrorCode = "Tester Lot Start Not Define.";
                }
                Log(m_strTesterErrorCode);
            }
        }

        private void Recv_Muti_Mux_EOT(SocketDefineTse.CTLTcpPacketDataTseRSP packet)
        {
            if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_SUCESS)
            {
                SocketDefineTse.CTLTcpMultiMuxEOTPakTse EotData = new SocketDefineTse.CTLTcpMultiMuxEOTPakTse();
                EotData = (SocketDefineTse.CTLTcpMultiMuxEOTPakTse)FNC.ByteToStruct(packet.szBuf, typeof(SocketDefineTse.CTLTcpMultiMuxEOTPakTse));

                for (int i = 0; i < (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT; i++)
                {
                    TesterData.m_nMultiMuxBinNo[i]       = EotData.nBinNo[i];
                    TesterData.m_nMultiMuxSubBinNo[i]    = EotData.nSubBinNo[i];
                    TesterData.m_bMultiMuxTestResult[i]  = EotData.bTestResult[i];
                    TesterData.m_nMultiMuxCIERank[i]     = EotData.nCIERank[i];
                }
                EotData.szBinName.CopyTo(TesterData.m_szMultiMuxBinName, 0);
                EotData.fWX.CopyTo      (TesterData.m_fMultiMuxWX      , 0);
                EotData.fWY.CopyTo      (TesterData.m_fMultiMuxWY      , 0);

                RecvEotComplete = true;
                string strTrace;
                strTrace = "[Tester] Receive : Multi Mux EOT";
                Log(strTrace);
            }
            else // Error ocure
            {
                m_bTesterError = true;
                if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_NO_ITEM_FILE)
                {
                    m_strTesterErrorCode = "Tester Lot Start No Item File";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_ALREADY_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot Start Already Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_INIT_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot Start Init Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_START_OFF_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot Start Off Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_LOT_END_ALREADY_FAIL)
                {
                    m_strTesterErrorCode = "Tester Lot End Already Fail";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_NOT_COMMAND)
                {
                    m_strTesterErrorCode = "Tester Not Command";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_NOT_RESERVED)
                {
                    m_strTesterErrorCode = "Tester Not Reserved";
                }
                else if (packet.cmp.uiErrorCode == (uint)SocketDefineTse.EN_ERROR_CODE.TLTESTER_ERROR_CODE_EOT_COMMAND_CIE_NG_ALARM)
                {
                    m_strTesterErrorCode = "Tester Pc CIE NG Alram";
                }
                else
                {
                    m_strTesterErrorCode = "Tester Lot Start Not Define.";
                }
                Log(m_strTesterErrorCode);
            }
        }
        #endregion

        #region [Send Command]
        public void GetTesterRecipeList()
        {
            ResetTesterErrorInfor();
            ResetTesterRecipeInfo();

            SocketDefineTse.CTLTcpPacketDataTse cmData = new SocketDefineTse.CTLTcpPacketDataTse();
            cmData.cmp.uiCommand   = (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_START_RECIPE_LIST_DATA;
            cmData.cmp.uiReserved  = 7;
            cmData.cmp.uiErrorCode = 0;
            cmData.cmp.uiSize = (uint)Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartRecipeListTse));

            SocketDefineTse.CTLTcpLotStartRecipeListTse RecipeListData = new SocketDefineTse.CTLTcpLotStartRecipeListTse();
            RecipeListData.nCurrentRecipeListIndex = TesterData.m_nCurRecipeListIndex;

            //struct -> cmData.szBuf
            byte[] classbyte = FNC.StructToByte(RecipeListData);

            int count = Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartRecipeListTse));
            cmData.szBuf = FNC.GetByteArrayToCharArray(classbyte, count);

            //cmData -> byte
            byte[] transbyte = FNC.StructToByte(cmData);

            server.Send(transbyte);
            Log("[Tester] Send : Recipe List Data ");
        }


        public void GetTesterItemList(int Index)
        {
            ResetTesterErrorInfor();
            ResetTesterItemInfo  ();
            TesterData.m_nCurItemIndex = Index;

            SocketDefineTse.CTLTcpPacketDataTse cmData = new SocketDefineTse.CTLTcpPacketDataTse();
            cmData.cmp.uiCommand   = (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_START_ITEM_DATA;
            cmData.cmp.uiReserved  = 7;
            cmData.cmp.uiErrorCode = 0;
            cmData.cmp.uiSize = (uint)Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartItemDataTse));

            SocketDefineTse.CTLTcpLotStartItemDataTse ItemData = new SocketDefineTse.CTLTcpLotStartItemDataTse();
            ItemData.nCurrentItemIndex = Index;
            ItemData.nTotalItemNum     = TesterData.m_nCurItemTotalCnt;

            //struct -> cmData.szBuf
            byte[] classbyte = FNC.StructToByte(ItemData);

            int count = Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartItemDataTse));
            cmData.szBuf = FNC.GetByteArrayToCharArray(classbyte, count);

            //cmData -> byte
            byte[] transbyte = FNC.StructToByte(cmData);
            server.Send(transbyte);

            Log(String.Format("[Tester] Send : Item List Data : %d", Index));
        }
        


        public void SendTesterLotStart(string strRecipe, string strLotName, string strEmployeeNumber, string strMachinName)
        {
            ResetTesterErrorInfor();

            SocketDefineTse.CTLTcpPacketDataTse cmData = new SocketDefineTse.CTLTcpPacketDataTse();
            cmData.cmp.uiCommand   = (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_START;
            cmData.cmp.uiReserved  = 7;
            cmData.cmp.uiErrorCode = 0;
            int cout = Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartPackTse));
            cmData.cmp.uiSize      = (uint)Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartPackTse));

            SocketDefineTse.CTLTcpLotStartPackTse LotStartData = new SocketDefineTse.CTLTcpLotStartPackTse();
            LotStartData.nCountClear    = 1;
            LotStartData.nCountFileSave = 0;

            string[] temp;
            temp = Regex.Split(strRecipe, ".tlp");
            string strDescription = DateTime.Now.ToString("yyMMdd") + "\\" + temp[0];

            LotStartData.szDescription    = FNC.GetStringToCharArray(strDescription   , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szTestItemName   = FNC.GetStringToCharArray(strRecipe        , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szLotName        = FNC.GetStringToCharArray(strLotName       , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szOperator       = FNC.GetStringToCharArray(strEmployeeNumber, (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szPKG            = FNC.GetStringToCharArray(strMachinName    , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szCHIP           = FNC.GetStringToCharArray("Chip"           , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szIV             = FNC.GetStringToCharArray("IV"             , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szRecipe         = FNC.GetStringToCharArray("Recipe"         , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szDPMCNO         = FNC.GetStringToCharArray("DPMCNG"         , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szComment        = FNC.GetStringToCharArray("Comment"        , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szIdentification = FNC.GetStringToCharArray("Identification" , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szDesignation    = FNC.GetStringToCharArray("Designation"    , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);
            LotStartData.szUserDefine     = FNC.GetStringToCharArray("User Define"    , (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_NAME_SIZE);


            //struct -> cmData.szBuf
            byte[] classbyte = FNC.StructToByte(LotStartData);

            int count = Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpLotStartPackTse));
            cmData.szBuf = FNC.GetByteArrayToCharArray(classbyte, count);

            //cmData -> byte
            byte[] transbyte = FNC.StructToByte(cmData);
            server.Send(transbyte);
            
            Log(String.Format("[Tester] Send : Lot Start: %s", strRecipe));
        }

        public void SendTesterLotEnd()
        {
            ResetTesterErrorInfor();

            SocketDefineTse.CTLTcpPacketDataTse cmData = new SocketDefineTse.CTLTcpPacketDataTse();
            cmData.cmp.uiCommand   = (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_LOT_END;
            cmData.cmp.uiReserved  = 7;
            cmData.cmp.uiErrorCode = 0;
            cmData.cmp.uiSize      = 0;

            //cmData -> byte
            byte[] transbyte = FNC.StructToByte(cmData);
            server.Send(transbyte);

            Log("[Tester] Send : Lot End");
        }


		//Make Log.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void  Log           (String Msg                                                               )
        {
            //Local Var.
            string sPath;
            string sTemp;
            string sFile =   "[" + string.Format("{0:yyMMdd}", DateTime.Now)+ "]" + "Tester.txt"; 
            //Make Dir.
            FNC.CreateDirOnWork("LOG");
            FNC.CreateDirOnWork("LOG\\TESTER");
            sPath = Application.StartupPath + "\\LOG\\TESTER\\" + sFile;


            //File Open.
            FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fp, Encoding.Unicode);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sTemp = "[" + string.Format("{0:hh:mm:ss}", DateTime.Now) + "]" + Msg + "\r\n";
            sw.Write(sTemp);
            sw.Flush();
            sw.Close();

        }

        #endregion

        public void SendTesterMultiSOT(bool[] bDut, int[] nRow, int[] nCol)
        {
            ResetTesterErrorInfor();
            ResetTesterMultiResultInfo();

            SocketDefineTse.CTLTcpPacketDataTse cmData = new SocketDefineTse.CTLTcpPacketDataTse();
            cmData.cmp.uiCommand   = (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_MULTI_JETTING_SOT;
            cmData.cmp.uiReserved  = 7;
            cmData.cmp.uiErrorCode = 0;
            cmData.cmp.uiSize = (uint)Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpMultiSOTPakTse));

            SocketDefineTse.CTLTcpMultiSOTPakTse SotData = new SocketDefineTse.CTLTcpMultiSOTPakTse();
            
            for(int i = 0; i < (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_TEST_COUNT; i++)
            {
                SotData.m_bDut[i] = bDut[i];
                SotData.m_nRow[i] = nRow[i];
                SotData.m_nCol[i] = nCol[i];
            }

            byte[] classbyte = FNC.StructToByte(SotData);                                                                       //  SOT Data -> byte[]

            int count = Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpMultiSOTPakTse));
            cmData.szBuf = FNC.GetByteArrayToCharArray(classbyte, count);

            byte[] transbyte = FNC.StructToByte(cmData);
            server.Send(transbyte);
        }

        public void SendTesterMultiMuxSOT(bool[] bDut, int[] nRow, int[] nCol)
        {
            ResetTesterErrorInfor        ();
            ResetTesterMultiMuxResultInfo();

            SocketDefineTse.CTLTcpPacketDataTse cmData = new SocketDefineTse.CTLTcpPacketDataTse();
            cmData.cmp.uiCommand   = (uint)SocketDefineTse.EN_CMD.TLTESTER_CMD_MULTI_MUX_JETTING_SOT;
            cmData.cmp.uiReserved  = 7;
            cmData.cmp.uiErrorCode = 0;
            cmData.cmp.uiSize = (uint)Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpMultiMuxSOTPakTse));

            SocketDefineTse.CTLTcpMultiMuxSOTPakTse SotData = new SocketDefineTse.CTLTcpMultiMuxSOTPakTse();

            for (int i = 0; i < (int)SocketDefineTse.EN_PACKET.TLTESTER_PACKET_MAX_MULTI_MUX_TEST_COUNT; i++)
            {
                SotData.m_bDut[i] = bDut[i];
                SotData.m_nRow[i] = nRow[i];
                SotData.m_nCol[i] = nCol[i];
            }

            byte[] classbyte = FNC.StructToByte(SotData);                                                                       //  SOT Data -> byte[]

            int count    = Marshal.SizeOf(typeof(SocketDefineTse.CTLTcpMultiMuxSOTPakTse));
            cmData.szBuf = FNC.GetByteArrayToCharArray(classbyte, count);

            byte[] transbyte = FNC.StructToByte(cmData);
            server.Send(transbyte);
            RecvEotComplete = false;
        }

        public bool IsConnected()
        {
            return server.Connected;
        }
        public void DisConnect()
        {
            //server.DisConnect();
        }
    }


}
