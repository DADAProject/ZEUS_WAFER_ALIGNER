using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace eMachine
{

    /***************************************************************************/
    /* Class: TLoginSET                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TLoginSET {
      public bool[] bEnableMenu = new bool[10];
      //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
      //생성자 & 소멸자. (Constructor & Destructor)
      public TLoginSET()
      {
      }
      ~TLoginSET() { }
    };

    /***************************************************************************/
    /* Class: TPROJ_BASE                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TPROJ_BASE {
        //UserSet - Base Setting 변수 처리
        
        //Default
        public int       iWaferSize         ;
        public int       iWaferType         ;

        public double    dNotchSize         ;
        public double    dEdgeLength        ;
        public double    dEdgeAngle         ;

        //추가 기능 옵션
        public bool    bUseCenterGap         ;
        public double  dLimitCenterGap       ;


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TPROJ_BASE()
        {
        }
        ~TPROJ_BASE() { }

        
        //------------------------------------------------------------------------
        public void Load(bool IsLoad, String DevName, int OtherJobUpdate = 0)
        {
            string sPath;
            string sFile = "ProjectBase";
            string sSection = sFile;
            string sName;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("Project");
            FNC.CreateDirOnWork("Project\\" + DevName);

            sPath = Application.StartupPath + "\\Project\\" + DevName + "\\" + sFile + ".INI";

            if (!FNC.FileExists(sPath)) return; 

            ini.Loadini(sPath);

            if (IsLoad)
            {			
                sName = sSection + "_iWaferSize "; ini.Load(sPath, sSection, sName, out iWaferSize              );
                sName = sSection + "_iWaferType "; ini.Load(sPath, sSection, sName, out iWaferType              );
                sName = sSection + "_dNotchSize "; ini.Load(sPath, sSection, sName, out dNotchSize              );
                sName = sSection + "_dEdgeLength"; ini.Load(sPath, sSection, sName, out dEdgeLength             );
                sName = sSection + "_dEdgeAngle "; ini.Load(sPath, sSection, sName, out dEdgeAngle              );

                sName = sSection + "_bUseCenterGap  "; ini.Load(sPath, sSection, sName, out bUseCenterGap       );
                sName = sSection + "_dLimitCenterGap"; ini.Load(sPath, sSection, sName, out dLimitCenterGap     );
            }
            else
            {
                sName = sSection + "_iWaferSize "; ini.Save(sPath, sSection, sName,      iWaferSize             );
                sName = sSection + "_iWaferType "; ini.Save(sPath, sSection, sName,      iWaferType             );
                sName = sSection + "_dNotchSize "; ini.Save(sPath, sSection, sName,      dNotchSize             );
                sName = sSection + "_dEdgeLength"; ini.Save(sPath, sSection, sName,      dEdgeLength            );
                sName = sSection + "_dEdgeAngle "; ini.Save(sPath, sSection, sName,      dEdgeAngle             );

                sName = sSection + "_bUseCenterGap  "; ini.Save(sPath, sSection, sName,     bUseCenterGap       );
                sName = sSection + "_dLimitCenterGap"; ini.Save(sPath, sSection, sName,     dLimitCenterGap     );

                //
                ini.Saveini(sPath);

            }
            ini = null;
        }	
    };

    /***************************************************************************/
    /* Class: TENGR_OPTN                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TENGR_OPTN {
        //UserSet - Engineer Option 변수 처리, Job File과 상관없는 변수 [FrmEngr-Option]
        public bool       bUseAutoHome       ; //PGM 시작 시 자동 Homing
        public bool       bAutoLotEnd        ;
        public int        iScanMGZType       ; //Mgazine Wafer 유무 확인 방식 0 : Step, 1 : Step Scan, 2 : Line Scan
	    public int        iChangeOperTime    ;
	    public String     sDBClsTime         ;
        public String     sLogClsTime        ;
        public int        iSetMode           ;
        public int        iJamDBPer          ;
        public int        iLotDBPer          ;
        public int        iClsDBPer          ;
	    public int        iLanguage          ;
        public int        iLastErrTime       ;
        public int        iLastErrCnt        ;
        public int        iSpeedRatio        ;
        public bool       bHoldErrProcess    ;
		public bool       bIgnPickErrOnRetest; //Retest시 Pick Err 무시(투입 쪽만 해당)        
        public bool       bLampatRun         ;
        public int        iVacOption         ; //사용자 설정 옵션 추가
        public int        iVacCount          ;
        public int        iVacTimeOut        ;


        public string     sCom_Light         ;
        public int        nServerPort        ;
        public double     dToleranceX        ;
        public double     dToleranceY        ;
        public double     dToleranceT        ;
        public bool       bUseBCR            ;
        public string     sBCRIP             ;
        public int        nBCRPort           ;
        public int        nBCRRetryCnt       ;
        public bool       bUseAlignCheck     ;
        public bool       bUseAlignVerify    ;
        public bool       bUseImageSave      ;
        public int        nMaxImageStorage   ;
        public int        iMaxImageDay       ;
        public string     sImageSavePath     ;
        public int        nTestRunCnt        ;
        public int        nRetryCnt          ;
        public int        nAlignCnt          ;
        public bool       bUseDetect         ;
        public bool       bUseRingFrame1     ; //by Notch
        public bool       bUseRingFrame2     ; //by Base
        public bool       bUseRingFrame3     ; //by Sawing
        public bool       bUseOnlyXY         ;
        public bool       bUseWaferSkip      ;
        public bool       bUseFindRingFrameAngle; //by (사용안함: bUseRingFrame 옵션으로 사용)
        public int        nVacDelay          ;
        public bool       bUseVisnIO         ;
        public bool       bUseDcutAlgnT      ;
        public string     sEQNo              ;
        public string     sIP1, sIP2, sIP3, sIP4;

        public double     dToleranceX_Verify ;
        public double     dToleranceY_Verify ;
        public double     dToleranceT_Verify ;



        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TENGR_OPTN()
        {

        }
        ~TENGR_OPTN() { }
        //--------------------------------------------------------------------------
        public void Load(bool IsLoad)
        {
            string sPath;
            string sFile    = "Engineer";
            string sSection = sFile;
            string sName    ;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\Option");

            sPath = Application.StartupPath + "\\System\\Option\\" + sFile + ".ini";

            if (!FNC.FileExists(sPath))
            {
                MessageBox.Show($"{sFile}.ini File이 없습니다.");
                return; 
            }

            ini.Loadini(sPath);
                
            if (IsLoad)
            {
                sName = sSection + "_UseAutoHome       "; ini.Load(sPath, sSection, sName, out bUseAutoHome       );
                sName = sSection + "_AutoLotEnd        "; ini.Load(sPath, sSection, sName, out bAutoLotEnd        );
                sName = sSection + "_ChangeOperTime    "; ini.Load(sPath, sSection, sName, out iChangeOperTime    );
                sName = sSection + "_DBClsTime         "; ini.Load(sPath, sSection, sName, out sDBClsTime         );
                sName = sSection + "_LogClsTime        "; ini.Load(sPath, sSection, sName, out sLogClsTime        );
                sName = sSection + "_SetMode           "; ini.Load(sPath, sSection, sName, out iSetMode           );
                sName = sSection + "_JamDBPer          "; ini.Load(sPath, sSection, sName, out iJamDBPer          );
                sName = sSection + "_LotDBPer          "; ini.Load(sPath, sSection, sName, out iLotDBPer          );
                sName = sSection + "_ClsDBPer          "; ini.Load(sPath, sSection, sName, out iClsDBPer          );
                sName = sSection + "_Language          "; ini.Load(sPath, sSection, sName, out iLanguage          );
                sName = sSection + "_LastErrTime       "; ini.Load(sPath, sSection, sName, out iLastErrTime       );
                sName = sSection + "_LastErrCnt        "; ini.Load(sPath, sSection, sName, out iLastErrCnt        );
                sName = sSection + "_SpeedRatio        "; ini.Load(sPath, sSection, sName, out iSpeedRatio        );
                sName = sSection + "_HoldErrProcess    "; ini.Load(sPath, sSection, sName, out bHoldErrProcess    );
				sName = sSection + "_IgnPickErrOnRetest"; ini.Load(sPath, sSection, sName, out bIgnPickErrOnRetest);
                sName = sSection + "_LampatRun         "; ini.Load(sPath, sSection, sName, out bLampatRun         );
                sName = sSection + "_VacOption         "; ini.Load(sPath, sSection, sName, out iVacOption         );
                sName = sSection + "_VacCount          "; ini.Load(sPath, sSection, sName, out iVacCount          );
                sName = sSection + "_VacTimeOut        "; ini.Load(sPath, sSection, sName, out iVacTimeOut        );

                sName = sSection + "_Com_Light         "; ini.Load(sPath, sSection, sName, out sCom_Light         );
                sName = sSection + "_ServerPort        "; ini.Load(sPath, sSection, sName, out nServerPort        );
                sName = sSection + "_ToleranceX        "; ini.Load(sPath, sSection, sName, out dToleranceX        );
                sName = sSection + "_ToleranceY        "; ini.Load(sPath, sSection, sName, out dToleranceY        );
                sName = sSection + "_ToleranceT        "; ini.Load(sPath, sSection, sName, out dToleranceT        );
                sName = sSection + "_UseBCR            "; ini.Load(sPath, sSection, sName, out bUseBCR            );
                sName = sSection + "_BCRIP             "; ini.Load(sPath, sSection, sName, out sBCRIP             );
                sName = sSection + "_nBCRPort          "; ini.Load(sPath, sSection, sName, out nBCRPort           );
                sName = sSection + "_BCRRetryCnt       "; ini.Load(sPath, sSection, sName, out nBCRRetryCnt       );
                sName = sSection + "_UseAlignCheck     "; ini.Load(sPath, sSection, sName, out bUseAlignCheck     );
                sName = sSection + "_UseAlignVerify    "; ini.Load(sPath, sSection, sName, out bUseAlignVerify    );
                sName = sSection + "_UseImageSave      "; ini.Load(sPath, sSection, sName, out bUseImageSave      );
                sName = sSection + "_MaxImageStorage   "; ini.Load(sPath, sSection, sName, out nMaxImageStorage   );
                sName = sSection + "_MaxImageDay       "; ini.Load(sPath, sSection, sName, out iMaxImageDay       );
                sName = sSection + "_ImageSavePath     "; ini.Load(sPath, sSection, sName, out sImageSavePath     );
                sName = sSection + "_TestRunCnt        "; ini.Load(sPath, sSection, sName, out nTestRunCnt        );
                sName = sSection + "_VacDelay          "; ini.Load(sPath, sSection, sName, out nVacDelay          );
                sName = sSection + "_UseDetect         "; ini.Load(sPath, sSection, sName, out bUseDetect         );
                sName = sSection + "_RetryCnt          "; ini.Load(sPath, sSection, sName, out nRetryCnt          );
                sName = sSection + "_AlignCnt          "; ini.Load(sPath, sSection, sName, out nAlignCnt          );
                sName = sSection + "_UseRingFrame1     "; ini.Load(sPath, sSection, sName, out bUseRingFrame1     );
                sName = sSection + "_UseRingFrame2     "; ini.Load(sPath, sSection, sName, out bUseRingFrame2     );
                sName = sSection + "_UseRingFrame3     "; ini.Load(sPath, sSection, sName, out bUseRingFrame3     );
                sName = sSection + "_UseOnlyXY         "; ini.Load(sPath, sSection, sName, out bUseOnlyXY         );
                sName = sSection + "_UseWaferSkip      "; ini.Load(sPath, sSection, sName, out bUseWaferSkip      );
                sName = sSection + "_UseFindRingFrameAngle"; ini.Load(sPath, sSection, sName, out bUseFindRingFrameAngle);
                sName = sSection + "_UseVisnIO"         ; ini.Load(sPath, sSection, sName, out bUseVisnIO         );
                sName = sSection + "_UseDcutAlgnT"      ; ini.Load(sPath, sSection, sName, out bUseDcutAlgnT      );

                sName = sSection + "_ToleranceX_Verify" ; ini.Load(sPath, sSection, sName, out dToleranceX_Verify );
                sName = sSection + "_ToleranceY_Verify" ; ini.Load(sPath, sSection, sName, out dToleranceY_Verify );
                sName = sSection + "_ToleranceT_Verify" ; ini.Load(sPath, sSection, sName, out dToleranceT_Verify );

                                                                                               
                //sName = sSection + "EQNo               "; ini.Load(sPath, sSection, sName, out sEQNo              );
                
                sName = sSection + "_IP1               "; ini.Load(sPath, sSection, sName, out sIP1               );
                sName = sSection + "_IP2               "; ini.Load(sPath, sSection, sName, out sIP2               );
                sName = sSection + "_IP3               "; ini.Load(sPath, sSection, sName, out sIP3               );
                sName = sSection + "_IP4               "; ini.Load(sPath, sSection, sName, out sIP4               );

                //JUNG/
                bUseRingFrame1 = false;  //Notch, Base는 Ring Frame 사용 X
                bUseRingFrame2 = false;

                if (nVacDelay < 100) nVacDelay = 100;

            }
            else
            {
                sName = sSection + "_UseAutoHome       "; ini.Save(sPath, sSection, sName,     bUseAutoHome       );
                sName = sSection + "_AutoLotEnd        "; ini.Save(sPath, sSection, sName,     bAutoLotEnd        );
                sName = sSection + "_ChangeOperTime    "; ini.Save(sPath, sSection, sName,     iChangeOperTime    );
                sName = sSection + "_DBClsTime         "; ini.Save(sPath, sSection, sName,     sDBClsTime         );
                sName = sSection + "_LogClsTime        "; ini.Save(sPath, sSection, sName,     sLogClsTime        );
                sName = sSection + "_SetMode           "; ini.Save(sPath, sSection, sName,     iSetMode           );
                sName = sSection + "_JamDBPer          "; ini.Save(sPath, sSection, sName,     iJamDBPer          );
                sName = sSection + "_LotDBPer          "; ini.Save(sPath, sSection, sName,     iLotDBPer          );
                sName = sSection + "_ClsDBPer          "; ini.Save(sPath, sSection, sName,     iClsDBPer          );
                sName = sSection + "_Language          "; ini.Save(sPath, sSection, sName,     iLanguage          );
                sName = sSection + "_LastErrTime       "; ini.Save(sPath, sSection, sName,     iLastErrTime       );
                sName = sSection + "_LastErrCnt        "; ini.Save(sPath, sSection, sName,     iLastErrCnt        );
                sName = sSection + "_SpeedRatio        "; ini.Save(sPath, sSection, sName,     iSpeedRatio        );
                sName = sSection + "_HoldErrProcess    "; ini.Save(sPath, sSection, sName,     bHoldErrProcess    );
				sName = sSection + "_IgnPickErrOnRetest"; ini.Save(sPath, sSection, sName,     bIgnPickErrOnRetest);
                sName = sSection + "_LampatRun         "; ini.Save(sPath, sSection, sName,     bLampatRun         );
                sName = sSection + "_VacOption         "; ini.Save(sPath, sSection, sName,     iVacOption         );
                sName = sSection + "_VacCount          "; ini.Save(sPath, sSection, sName,     iVacCount          );
                sName = sSection + "_VacTimeOut        "; ini.Save(sPath, sSection, sName,     iVacTimeOut        );
                
                sName = sSection + "_Com_Light         "; ini.Save(sPath, sSection, sName,     sCom_Light         );
                sName = sSection + "_ServerPort        "; ini.Save(sPath, sSection, sName,     nServerPort        );
                sName = sSection + "_ToleranceX        "; ini.Save(sPath, sSection, sName,     dToleranceX        );
                sName = sSection + "_ToleranceY        "; ini.Save(sPath, sSection, sName,     dToleranceY        );
                sName = sSection + "_ToleranceT        "; ini.Save(sPath, sSection, sName,     dToleranceT        );
                sName = sSection + "_UseBCR            "; ini.Save(sPath, sSection, sName,     bUseBCR            );
                sName = sSection + "_BCRIP             "; ini.Save(sPath, sSection, sName,     sBCRIP             );
                sName = sSection + "_nBCRPort          "; ini.Save(sPath, sSection, sName,     nBCRPort           );
                sName = sSection + "_BCRRetryCnt       "; ini.Save(sPath, sSection, sName,     nBCRRetryCnt       );
                sName = sSection + "_UseAlignCheck     "; ini.Save(sPath, sSection, sName,     bUseAlignCheck     );
                sName = sSection + "_UseAlignVerify    "; ini.Save(sPath, sSection, sName,     bUseAlignVerify    );
                sName = sSection + "_UseImageSave      "; ini.Save(sPath, sSection, sName,     bUseImageSave      );
                sName = sSection + "_MaxImageStorage   "; ini.Save(sPath, sSection, sName,     nMaxImageStorage   );
                sName = sSection + "_MaxImageDay       "; ini.Save(sPath, sSection, sName,     iMaxImageDay       );
                sName = sSection + "_ImageSavePath     "; ini.Save(sPath, sSection, sName,     sImageSavePath     );
                sName = sSection + "_TestRunCnt        "; ini.Save(sPath, sSection, sName,     nTestRunCnt        );
                sName = sSection + "_VacDelay          "; ini.Save(sPath, sSection, sName,     nVacDelay          );
                sName = sSection + "_UseDetect         "; ini.Save(sPath, sSection, sName,     bUseDetect         );
                sName = sSection + "_RetryCnt          "; ini.Save(sPath, sSection, sName,     nRetryCnt          );
                sName = sSection + "_AlignCnt          "; ini.Save(sPath, sSection, sName,     nAlignCnt          );
                sName = sSection + "_UseRingFrame1     "; ini.Save(sPath, sSection, sName,     bUseRingFrame1     );
                sName = sSection + "_UseRingFrame2     "; ini.Save(sPath, sSection, sName,     bUseRingFrame2     );
                sName = sSection + "_UseRingFrame3     "; ini.Save(sPath, sSection, sName,     bUseRingFrame3     );
                sName = sSection + "_UseOnlyXY         "; ini.Save(sPath, sSection, sName,     bUseOnlyXY         );
                sName = sSection + "_UseWaferSkip      "; ini.Save(sPath, sSection, sName,     bUseWaferSkip      );
                sName = sSection + "_UseFindRingFrameAngle"; ini.Save(sPath, sSection, sName,  bUseFindRingFrameAngle);
                sName = sSection + "_UseVisnIO"         ; ini.Save(sPath, sSection, sName,     bUseVisnIO         );
                sName = sSection + "_UseDcutAlgnT"      ; ini.Save(sPath, sSection, sName,     bUseDcutAlgnT      );

                sName = sSection + "_ToleranceX_Verify" ; ini.Save(sPath, sSection, sName,     dToleranceX_Verify );
                sName = sSection + "_ToleranceY_Verify" ; ini.Save(sPath, sSection, sName,     dToleranceY_Verify );
                sName = sSection + "_ToleranceT_Verify" ; ini.Save(sPath, sSection, sName,     dToleranceT_Verify );


                //sName = sSection + "EQNo               "; ini.Save(sPath, sSection, sName,     sEQNo              );

                sName = sSection + "_IP1               "; ini.Save(sPath, sSection, sName,     sIP1               );
                sName = sSection + "_IP2               "; ini.Save(sPath, sSection, sName,     sIP2               );
                sName = sSection + "_IP3               "; ini.Save(sPath, sSection, sName,     sIP3               );
                sName = sSection + "_IP4               "; ini.Save(sPath, sSection, sName,     sIP4               );

                ini.Saveini(sPath);
            }
            //
            ini = null;
        }
    };  

    /***************************************************************************/
    /* Class: TENGR_SOCKET                                                     */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TENGR_SOCKET {
        //UserSet - Handler Socket 처리 변수 
	    public bool       bUseDblTest    ;
	    public int        iDblTestCnt    ;
	    public bool       bUseAutoOff    ;
	    public int        iCleanCount    ;
	    public bool       bCleanLotStrat ;
	    public bool       bCleanLotEnd   ;
	    public bool       bCleanCount    ;
	    public int        iUseLimit      ;
	    public int        iLimitEvent    ;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TENGR_SOCKET()
        {
        }
        ~TENGR_SOCKET() { }

        public void Load(bool IsLoad)
        {
            String sPath;
            String sFile = "Socket";
            String sSection = sFile;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\Option");
            sPath = Application.StartupPath + "\\System\\Option\\" + sFile + ".INI";

            if (IsLoad)
            {
                ini.Load(sPath, sSection, "UseDblTest", out bUseDblTest);
            }
            else
            {
                ini.Save(sPath, sSection, "UseDblTest", bUseDblTest);
            }
            ini = null;
        }
    };


    /***************************************************************************/
    /* Class: TTCPIP_OPTN                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TNET_OPTN {    
        //UserSet - TCP/IP 설정 변수
	    public string   sEQPID                  ;
	    public string   sMachNo                 ;                                  
	    
        //
        public bool[]   bOnline      = new bool  [(int)EN_FTP.EndOfId];
	    public int[]    iTimeOut     = new int   [(int)EN_FTP.EndOfId];
	    public int[]    iRetry       = new int   [(int)EN_FTP.EndOfId];
	    public string[] sIP          = new string[(int)EN_FTP.EndOfId]; //Server Ip 설정
        public int[]    iPort        = new int   [(int)EN_FTP.EndOfId];
        public int[]    iSrvPort     = new int   [(int)EN_FTP.EndOfId];

	    public string[] sHostName    = new string[(int)EN_FTP.EndOfId]; 
	    public string[] sUserName    = new string[(int)EN_FTP.EndOfId]; 
	    public string[] sPassword    = new string[(int)EN_FTP.EndOfId]; 
        public string[] sPath1       = new string[(int)EN_FTP.EndOfId];
        public string[] sPath2       = new string[(int)EN_FTP.EndOfId]; 

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TNET_OPTN()
        {
        }
        ~TNET_OPTN() { }

        public void UpdateByGrid(bool toGrid, ref System.Windows.Forms.DataGridView Grid)
        {
			int iCbIdx = 0;
			int nRow = 0;
            int n, i;
            int iTotWidth    = 0;
	        int[]     iWidth = {200, 300, 0};
	        string[]  sItem  = {" ", "NAME", "VALUE"};
            string    sName  = "";

			//EN_TCPIP & EN_FTP Use ComboBox 추가
			DataGridViewComboBoxCell[] cbCell = new DataGridViewComboBoxCell[(int)EN_FTP.EndOfId];
			for (n = 0; n < (int)EN_FTP.EndOfId; n++) {
				 cbCell[n] = new DataGridViewComboBoxCell();
				 cbCell[n].DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
				 cbCell[n].Items.Add("true" );
				 cbCell[n].Items.Add("false");
				 }

			//
            if(toGrid) {
                Grid.Dock = System.Windows.Forms.DockStyle.Fill;
				Grid.Font = new Font("Century Gothic", 12, FontStyle.Regular);
                FNC.SetGridStyle(ref Grid);
                Grid.BackgroundColor = FRM.GetGridBackColor(); //Color.FromArgb(66, 72, 88);
                //
                for(i=0;i<sItem.Length;i++) 
                {
                    Grid.Columns.Add(sItem[i] , sItem[i]);
                    Grid.Columns[i].Width = iWidth[i];
                    iTotWidth            += iWidth[i];
                    Grid.Columns[i].SortMode  =  DataGridViewColumnSortMode.NotSortable;
                }

                //Edit 금지
                Grid.Columns[0].ReadOnly = true;
                Grid.Columns[1].ReadOnly = true;

                Grid.Columns[0].DefaultCellStyle.WrapMode  = DataGridViewTriState.True;
                Grid.Columns[0].DefaultCellStyle.BackColor = Color.Silver;
                Grid.Columns[1].DefaultCellStyle.WrapMode  = DataGridViewTriState.True;
                Grid.Columns[1].DefaultCellStyle.BackColor = Color.Silver;
                Grid.Columns[2].Width                      = Grid.Width - iTotWidth-20;


                Grid.Rows.Add("Machine Info", "EQP ID     "           , Convert.ToString(sEQPID      )); nRow++;
                Grid.Rows.Add("Machine Info", "Machine No "           , Convert.ToString(sMachNo     )); nRow++;
                for(i=0;i<(int)EN_TCPIP.EndOfId;i++) 
                {
					iCbIdx = i;
                    sName = Enum.GetName(typeof(EN_TCPIP),i);

					cbCell[i].Value = bOnline [i] ? "true" : "false";  
                    Grid.Rows.Add(sName, "Online  "                   , null                         ); 
					Grid.Rows[nRow].Cells[2] = cbCell[i]; 
					Grid.Rows[nRow].Cells[2].Style.Font = new Font("Arial", 10, FontStyle.Bold);         nRow++;//Convert.ToString(bOnline [i])); 
                    
					Grid.Rows.Add(sName, "IP      "                   , Convert.ToString(sIP     [i]));  nRow++;
                    Grid.Rows.Add(sName, "Port    "                   , Convert.ToString(iPort   [i]));  nRow++;
                    Grid.Rows.Add(sName, "Time-Out"                   , Convert.ToString(iTimeOut[i]));  nRow++;
                    Grid.Rows.Add(sName, "Retry   "                   , Convert.ToString(iRetry  [i]));  nRow++;
                }

                for(i=(int)EN_TCPIP.EndOfId;i<(int)EN_FTP.EndOfId;i++) 
                {//
                    sName = "FTP_" + Enum.GetName(typeof(EN_FTP),i);
					cbCell[i].Value = bOnline [i] ? "true" : "false";  
                    Grid.Rows.Add(sName, "Online "                   , null                         ); 
					Grid.Rows[nRow].Cells[2] = cbCell[i]; 
					Grid.Rows[nRow].Cells[2].Style.Font = new Font("Arial", 10, FontStyle.Bold);        nRow++;//Convert.ToString(bOnline  [i])); 
                    Grid.Rows.Add(sName, "Host Name"                 , Convert.ToString(sHostName[i])); nRow++;
                    Grid.Rows.Add(sName, "Port   "                   , Convert.ToString(iPort    [i])); nRow++;
                    Grid.Rows.Add(sName, "User Name"                 , Convert.ToString(sUserName[i])); nRow++;
                    Grid.Rows.Add(sName, "Password"                  , Convert.ToString(sPassword[i])); nRow++;
                    Grid.Rows.Add(sName, "Time-Out"                  , Convert.ToString(iTimeOut [i])); nRow++;
                    Grid.Rows.Add(sName, "Retry  "                   , Convert.ToString(iRetry   [i])); nRow++;
                }
                //
                FNC.SameCellColor(ref Grid, 1, Color.LightCyan, Color.PaleTurquoise);
            }
            else 
            {
                n = 0;
                cDEF.POSN.WriteDatChLog(3, ref sEQPID   , Grid[2, n].Value     , Grid[1 ,n++].Value);
                cDEF.POSN.WriteDatChLog(3, ref sMachNo  , Grid[2, n].Value     , Grid[1, n++].Value);
                for(i=0;i<(int)EN_TCPIP.EndOfId;i++) 
                {
                    sName = Enum.GetName(typeof(EN_TCPIP),i);
                    cDEF.POSN.WriteDatChLog(3, ref bOnline [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref sIP     [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref iPort   [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref iTimeOut[i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref iRetry  [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);

                }

                for(i=(int)EN_TCPIP.EndOfId;i<(int)EN_FTP.EndOfId;i++) 
                {//
                    sName = Enum.GetName(typeof(EN_FTP),i);
                    cDEF.POSN.WriteDatChLog(3, ref bOnline  [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref sHostName[i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref iPort    [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref sUserName[i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref sPassword[i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref iTimeOut [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                    cDEF.POSN.WriteDatChLog(3, ref iRetry   [i] , Grid[2, n].Value , sName + Grid[1 ,n++].Value);
                }

                //
                Load(false);
            }
            Grid.Visible  = true;
        }
        //------------------------------------------------------------------------
        public void Load(bool IsLoad)
        {
            String sPath;
            String sFile = "TCPIP";
            String sSection = sFile;
            String sName    ;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\Option");
            sPath = Application.StartupPath + "\\System\\Option\\" + sFile + ".INI";
            ini.Loadini(sPath);
            if (IsLoad)
            {
                sName = sSection + string.Format("EQPID"          );   ini.Load(sPath, sSection, sName, out sEQPID       );  
                sName = sSection + string.Format("MachNo"         );   ini.Load(sPath, sSection, sName, out sMachNo      );  
                for(int i=0;i<(int)EN_FTP.EndOfId;i++) {
                    sName = sSection + string.Format("_{0}Online  ", i); ini.Load(sPath, sSection, sName, out bOnline  [i]);
                    sName = sSection + string.Format("_{0}TimeOut ", i); ini.Load(sPath, sSection, sName, out iTimeOut [i]);
                    sName = sSection + string.Format("_{0}Retry   ", i); ini.Load(sPath, sSection, sName, out iRetry   [i]);
                    sName = sSection + string.Format("_{0}IP1     ", i); ini.Load(sPath, sSection, sName, out sIP      [i]);
                    sName = sSection + string.Format("_{0}Port    ", i); ini.Load(sPath, sSection, sName, out iPort    [i]);
                    sName = sSection + string.Format("_{0}SrvPort ", i); ini.Load(sPath, sSection, sName, out iSrvPort [i]);
                    
                    sName = sSection + string.Format("_{0}HostName", i); ini.Load(sPath, sSection, sName, out sHostName[i]);
                    sName = sSection + string.Format("_{0}UserName", i); ini.Load(sPath, sSection, sName, out sUserName[i]);
                    sName = sSection + string.Format("_{0}Password", i); ini.Load(sPath, sSection, sName, out sPassword[i]);

                }
            }
            else
            {
                sName = sSection + string.Format("EQPID"          );   ini.Save(sPath, sSection, sName, sEQPID     );  
                sName = sSection + string.Format("MachNo"         );   ini.Save(sPath, sSection, sName, sMachNo    ); 
                for(int i=0;i<(int)EN_FTP.EndOfId;i++) {
                    sName = sSection + string.Format("_{0}Online " , i); ini.Save(sPath, sSection, sName, bOnline  [i]);
                    sName = sSection + string.Format("_{0}TimeOut" , i); ini.Save(sPath, sSection, sName, iTimeOut [i]);
                    sName = sSection + string.Format("_{0}Retry  " , i); ini.Save(sPath, sSection, sName, iRetry   [i]);
                    sName = sSection + string.Format("_{0}IP1    " , i); ini.Save(sPath, sSection, sName, sIP      [i]);
                    sName = sSection + string.Format("_{0}Port   " , i); ini.Save(sPath, sSection, sName, iPort    [i]);
                    sName = sSection + string.Format("_{0}SrvPort ", i); ini.Save(sPath, sSection, sName, iSrvPort [i]);

                    sName = sSection + string.Format("_{0}HostName", i); ini.Save(sPath, sSection, sName, sHostName[i]);
                    sName = sSection + string.Format("_{0}UserName", i); ini.Save(sPath, sSection, sName, sUserName[i]);
                    sName = sSection + string.Format("_{0}Password", i); ini.Save(sPath, sSection, sName, sPassword[i]);
                }
                ini.Saveini(sPath);
            }
            ini = null;
        }
    };

    /***************************************************************************/
    /* Class: TSYS_OPTN                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TSYS_OPTN {
        //UserSet -  MASTER OPTION 설정 변수 (FrmAdmin - Option)
        public int    iRunMode                    ;  //AutoRun
        public int    iWorkMode                   ;  //Work Mode
        public int[]   iTestMode     = new int[(int)EN_CAM.EndofCam];  //Test Mode
        public int    iViewErrDisp                ;  //Error Display

        public int    iRunSkipMat                 ;  
        public bool[] bSkipVac       = new bool[(int)EN_WAF_ID.EndOfId];  //Vacuum Check Set
        public bool[] bOffAR         = new bool[vDEF.MAX_SEQ_PART     ];  //AutoRun Mode Set.
        public int    iChkTopDoor                 ; //Door Option.
        public int    iChkFan                     ; //Door Option.
        public int    iChkBtmDoor                 ;
        public int    iChkDrLock                  ;
        public int    iChkSafety                  ;
        public int    iChkIon                     ;
        public int    iChkGrid                    ;
        public int    iLangOpt                    ;
        public int    iSkipSeqLog                 ;

        public bool   bSimulRun                   ;
        public bool   bViewROI                    ;

        public bool bFanSkipAlarm                    ;//2LC8 2026 08 25
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSYS_OPTN()
        {
            iChkTopDoor      = 1;
            iChkBtmDoor      = 1;
            iChkDrLock       = 1; 
            iChkSafety       = 1;
            iChkIon          = 1;
            iChkFan          = 1;
            iSkipSeqLog      = 0;

            for (int i = 0; i < (int)EN_WAF_ID.EndOfId; i++)
            {
                bSkipVac[i] = false;
            }

            bSimulRun        = false;
        }
        ~TSYS_OPTN() { }
        public void InitSysOptn()
        {
            iChkTopDoor = 1;
            iChkBtmDoor = 1;
            iChkDrLock = 1;
            iChkSafety = 1;
            iChkIon = 1;
            iChkFan = 1;
            iChkGrid = 1;

            iRunMode = 0;
            iRunSkipMat = 0;
            bSimulRun = false;

            for (int n = 0; n < bSkipVac.Length; n++) bSkipVac[n] = false;
            for (int n = 0; n < cDEF.POSN.GetPartCnt(); n++) bOffAR[n] = false;
            for (int n = 0; n < iTestMode.Length; n++) iTestMode[n] = (int)EN_TEST_MODE.CHK_AWAY;

            cDEF.MOTR.m_bSkipChkCrash = false;
        }

        public void Load(bool IsLoad)
        {
            string sPath;
            string sFile    = "system";
            string sSection = sFile;
            TIniUnit2 ini   = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            sPath = Application.StartupPath + "\\System\\" + sFile + ".ini";

            ini.Loadini(sPath);

            if (IsLoad)
            {
                 ini.Load(sPath, sSection, "SkipSeqLog", out iSkipSeqLog);
                 ini.Load(sPath,sSection , "SkipAlarm", out bFanSkipAlarm); //2026 08 25 2LC8
            }
            else
            {
                ini.Save(sPath, sSection, "SkipSeqLog", iSkipSeqLog);
                ini.Save(sPath, sSection, "SkipAlarm", bFanSkipAlarm); //2026 08 25 2LC8
                ini.Saveini(sPath);
            }
        }
    };

    /***************************************************************************/
    /* Class: TPASS_WORD                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TPASS_WORD {
        public String sEngr;
        public String sMstr;
        public String sTech;
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TPASS_WORD()
        {
        }
        ~TPASS_WORD() { }

        public void Load(bool IsLoad)
        {
            String sPath;
            String sFile = "Password";
            String sSection = sFile;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            sPath = Application.StartupPath + "\\System\\" + sFile + ".INI";

            ini.Loadini(sPath);

            if (IsLoad)
            {
                 ini.Load(sPath, sSection, "Engr", out sEngr);
                 ini.Load(sPath, sSection, "Mstr", out sMstr);
                 ini.Load(sPath, sSection, "Tech", out sTech);

            }
            else
            {
                 ini.Save(sPath, sSection, "Engr", sEngr);
                 ini.Save(sPath, sSection, "Mstr", sMstr);
                 ini.Save(sPath, sSection, "Tech", sTech);

                ini.Saveini(sPath);
            }
            ini = null;
        }
    };

    /***************************************************************************/
    /* Class: TPROJ_OPTN                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TPROJ_OPTN {
        public      Color[]       cStatColor      = new Color [20]; 

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TPROJ_OPTN()
        {

        }
        ~TPROJ_OPTN() { }
        public void LoadColor(bool IsLoad)
        {
            String sPath;
            String sFile = "WorkColor";
            String sSection = sFile;
            String sName;
            int r=0,g=0,b=0;

            TIniUnit2 ini = new TIniUnit2();
            //Make Dir.
            FNC.CreateDirOnWork("System");
            sPath = Application.StartupPath + "\\System\\" + sFile + ".INI";
            ini.Loadini(sPath);

            if (IsLoad)
            {
                for(int i=0;i<cStatColor.Length;i++)
                {
                    sName = sSection + string.Format("_{0}CaseColorR " , i); ini.Load(sPath, sSection, sName, out r  );
                    sName = sSection + string.Format("_{0}CaseColorG " , i); ini.Load(sPath, sSection, sName, out g  );
                    sName = sSection + string.Format("_{0}CaseColorB " , i); ini.Load(sPath, sSection, sName, out b  );
                    cStatColor[i] = Color.FromArgb(r,g,b);
                }
            }
            else
            {
                for(int i=0;i<cStatColor.Length;i++)
                {
                    r   = (int)cStatColor[i].R;
                    g   = (int)cStatColor[i].G;
                    b   = (int)cStatColor[i].B;
                    sName = sSection + string.Format("_{0}CaseColorR " , i); ini.Save(sPath, sSection, sName, r  );
                    sName = sSection + string.Format("_{0}CaseColorG " , i); ini.Save(sPath, sSection, sName, g  );
                    sName = sSection + string.Format("_{0}CaseColorB " , i); ini.Save(sPath, sSection, sName, b  );
                }
                ini.Saveini(sPath);
            }
        }
        //------------------------------------------------------------------------
        public void Load(bool IsLoad, String DevName)
        {
            String sPath;
            String sFile = "ProjOption";
            String sSection = sFile;

            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("Project");
            FNC.CreateDirOnWork("Project\\" + DevName);

            sPath = Application.StartupPath + "\\Project\\" + DevName + "\\" + sFile + ".INI";

            if (IsLoad)
            {

            }
            else
            {

            }
        }

        //UpdateGrid
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    };  
    
    /***************************************************************************/
    /* Class: TSysCnt                                                          */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TSysCnt 
    {
        public int        iAlignCnt; //
        
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSysCnt()
        {
            
            ResetData();

        }
        ~TSysCnt() { }

        public void ResetData()
        {
            iAlignCnt = 0; 
        }

        //------------------------------------------------------------------------
        public void Load(bool IsLoad, String DevName)
        {
            String sPathDvc, sPath;
            String sFile = "LifeQty";
            String sSection = sFile;
            String sName;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("Project");
            FNC.CreateDirOnWork("System" );
            FNC.CreateDirOnWork("Project\\" + DevName);

            sPathDvc = Application.StartupPath + "\\Project\\" + DevName + "\\" + sFile + ".INI";
            sPath    = Application.StartupPath + "\\System\\"  + sFile + ".INI";

            try
            {
                ini.Loadini(sPath);

                //            
                if (IsLoad)
                {
                    sName = sSection + string.Format("_AlignCnt"); ini.Load(sPath, sSection, sName, out iAlignCnt);

                }
                else
                {
                    //Backup
                    FNC.FileBackup(sPath);

                    sName = sSection + string.Format("_AlignCnt"); ini.Save(sPath, sSection, sName, iAlignCnt);

                    //
                    ini.Saveini(sPath);
                }
                ini = null;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"[Exception] Load {ex.Message}");
            }

        }
    };


    /***************************************************************************/
    /* Class: TFileManger                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TFileManger
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        String m_sCrntDevice    ;

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        //Buffers.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //UserSet - 사용할 변수 Class 정의 
        public TPROJ_BASE     ProjBase    = new TPROJ_BASE    ();
        public TENGR_OPTN     EngrOptn    = new TENGR_OPTN    ();
        public TSYS_OPTN      SysOptn     = new TSYS_OPTN     ();
        public TPASS_WORD     Password    = new TPASS_WORD    ();
        public TNET_OPTN      NetOptn     = new TNET_OPTN     ();   //
        public TLoginSET[]    LoginSet    = new TLoginSET[(int)EN_LOGIN.EndOfId];
        public TSysCnt        SysCnt      = new TSysCnt       ();
        public TPROJ_OPTN     ProjOptn    = new TPROJ_OPTN    ();

        
        //Update Info.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public String[] m_sUpInform = new string[vDEF.MAX_UPDATE_INFO];
        public String m_sVersion;

        //Var.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int    m_iCrntLevel     ;
        public int    m_iCrntUpdateNo  ;
        public String m_sCrntOperID    ;
        //public String m_sPreDevice     ;
        //public String m_sLoadJobName   ; //Manual & Auto 구분해서 Lot Open 할때 필요한 Job Name하고 OperID.
        //public String m_sLoadOperID    ;


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public string _sCrntDevice     { get { return m_sCrntDevice    ;  } }
        public string _sVersion        {
            get { return m_sVersion;  }
            set { m_sVersion = value; }
        }
        public EN_RUN_MODE GetRunMode() => (EN_RUN_MODE)SysOptn.iRunMode;
        public bool IsAutoMode() => SysOptn.iRunMode == vDEF.AUTO_RUN;
        public bool IsManMode () => SysOptn.iRunMode == vDEF.MAN_RUN;
        public bool IsDryMode () => SysOptn.iRunMode == vDEF.DRY_RUN;
        public bool IsSimRun  () => SysOptn.bSimulRun;


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TFileManger()
        { 
            m_sVersion    = string.Empty;
            m_sCrntDevice = string.Empty;

            for(int i=0;i<(int)EN_LOGIN.EndOfId;i++) 
            { 
                LoginSet[i] = new TLoginSET();
            }
        }
        ~TFileManger() { }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init()
        {
            //Set Default User Level.
            m_iCrntLevel = (int)EN_LOGIN.Operator;
            LoadLastInfo   (true                );
            Load           (true                );

        }
        //------------------------------------------------------------------------
        public void SetUserLevel(EN_LOGIN lv = EN_LOGIN.Operator)
        {
            if (!cDEF.FM.IsAutoMode() && lv == EN_LOGIN.Operator) return; 

            m_iCrntLevel = (int)lv;
        }
        //------------------------------------------------------------------------
        public bool IsMasterLv()
        {
            return m_iCrntLevel == (int)EN_LOGIN.Master;
        }
        //------------------------------------------------------------------------
        public bool IsOperLv()
        {
            return m_iCrntLevel == (int)EN_LOGIN.Operator;
        }

        //------------------------------------------------------------------------
        public void  DefaultSysChkOptn()
        {

        }
        //------------------------------------------------------------------------
        public void  Load(bool isLoad)
        {
            LoadProj         (isLoad , m_sCrntDevice);
            EngrOptn   .Load (isLoad                );
            NetOptn    .Load (isLoad                );
            Password   .Load (isLoad                );
            LoadLoginSet     (isLoad                );     
            SysCnt        .Load         (isLoad , m_sCrntDevice); 
            ProjOptn      .Load         (isLoad , m_sCrntDevice);     
            SysOptn       .Load         (isLoad);
        }

        //Proc List
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void DefineMotrList()
        {
            //Local Val.
            int iPart = 0; 
            int iItem = 0;


            for (int i = 0; i < cDEF.MOTR._iNumOfMotr; i++)
            {
                cDEF.POSN.GetMotorPart(ref iPart, ref iItem, i);

                //Define ErrorList
                DefineMotrErrList(0, iPart, i, cDEF.MOTR[i].m_iErrAlarm  );
                DefineMotrErrList(1, iPart, i, cDEF.MOTR[i].m_iErrCW     );
                DefineMotrErrList(2, iPart, i, cDEF.MOTR[i].m_iErrCCW    );
                DefineMotrErrList(3, iPart, i, cDEF.MOTR[i].m_iErrHome   );
                DefineMotrErrList(4, iPart, i, cDEF.MOTR[i].m_iErrControl);
                DefineMotrErrList(5, iPart, i, cDEF.MOTR[i].m_iErrHold   );
                DefineMotrErrList(6, iPart, i, cDEF.MOTR[i].m_iErrPos    );
                DefineMotrErrList(7, iPart, i, cDEF.MOTR[i].m_iErrVel    );
                DefineMotrErrList(8, iPart, i, cDEF.MOTR[i].m_iErrAcc    );

                //Define Manual List
                DefineMotrManList(0, iPart, i, cDEF.MOTR[i].m_iManStop   );
                DefineMotrManList(1, iPart, i, cDEF.MOTR[i].m_iManJog    );
                DefineMotrManList(2, iPart, i, cDEF.MOTR[i].m_iManPitch  );
                DefineMotrManList(3, iPart, i, cDEF.MOTR[i].m_iManServo  );
                DefineMotrManList(4, iPart, i, cDEF.MOTR[i].m_iManAlarm  );
                DefineMotrManList(5, iPart, i, cDEF.MOTR[i].m_iManDirect );
                DefineMotrManList(6, iPart, i, cDEF.MOTR[i].m_iManHome   );

                if (iPart < 0 || iPart >= cDEF.POSN.GetPartCnt()) continue;

                for (int j = 0; j < cDEF.POSN.Dat[iPart].m_iItemCnt; j++)
                {
                    if (i == cDEF.POSN.Dat[iPart].Set[j].m_iMotor)
                    {
                        DefineMotrManList(10, iPart, i, cDEF.POSN.Dat[iPart].Set[j].m_iManNo, cDEF.POSN.Dat[iPart].Set[j].m_sName);
                    }
                }
            }
        }
        //------------------------------------------------------------------------
        public void DefineMotrErrList(int iDefIdx, int iPart, int iMotr, int iErrNo)
        {
            String sTemp = "";
            String sName = "";
            if (iErrNo < 0) return;

            switch (iDefIdx)
            {
                case 0: sTemp = "ALARM"                          ; break;
                case 1: sTemp = "+LIMIT"                         ; break;
                case 2: sTemp = "-LIMIT"                         ; break;
                case 3: sTemp = "Initial"                        ; break;
                case 4: sTemp = "CONTROL ERROR"                  ; break;
                case 5: sTemp = "HOLDING (SAFETY Sensor)"        ; break;
                case 6: sTemp = "Position PARAMETER LIMIT Sensor"; break;
                case 7: sTemp = "Speed PARAMETER LIMIT Sensor"   ; break;
                case 8: sTemp = "Acc/Dec PARAMETER LIMIT Sensor" ; break;
            }
            if(cDEF.MOTR[iMotr].m_sName == "") return;
            sName = string.Format("{0} {1} {2}", cDEF.MOTR[iMotr].m_sName.Trim(), cDEF.MOTR[iMotr].m_sNameAxis.Trim(), sTemp);
            cDEF.EPU[iErrNo].m_iGrade = 2;
            cDEF.EPU[iErrNo].m_iPart = iPart;
            cDEF.EPU[iErrNo].m_iKind = 0;

            cDEF.EPU.SetName(iErrNo, sName);
        }
        public void DefineMotrManList(int iDefIdx, int iPart, int iMotr, int iManNo, String sItmName = "")
        {
            String sTemp = "" ;
            String sName = "" ;
            if (iManNo < 0) return;

            switch (iDefIdx)
            {
                case 0: sTemp = "Motor Stop                  "; break;
                case 1: sTemp = "Motor JOG Move              "; break;
                case 2: sTemp = "Motor User PITCH Move       "; break;
                case 3: sTemp = "Motor SERVO On/Off          "; break;
                case 4: sTemp = "Motor RESET                 "; break;
                case 5: sTemp = "Motor DIRECT MOVE           "; break;
                case 6: sTemp = "Motor HOME                  "; break;
                case 10: sTemp = "Motor " + sItmName          ; break;
            }
            if(cDEF.MOTR[iMotr].m_sName == "") return;
            sName = string.Format("{0} {1}-{2}", cDEF.MOTR[iMotr].m_sName.Trim(), cDEF.MOTR[iMotr].m_sNameAxis.Trim(), sTemp);


        }
        //------------------------------------------------------------------------
        public void DefineActrList()
        {
            //Local Val.
            int iPart =0; 


            for (int i = 0; i < cDEF.ACTR._iNumOfACT; i++)
            {
                //Define ErrorList
                DefineActrErrList(iPart, i, cDEF.ACTR[i].m_iErrNo);
                //Define Manual List
                DefineActrManList(iPart, i, cDEF.ACTR[i].m_iManNo);
            }
        }
        //------------------------------------------------------------------------
        public void DefineActrErrList(int iPart, int iActr, int iErrNo)
        {
            String sTemp;
            String sName;
            if (iErrNo < 0) return;
            if (cDEF.ACTR[iActr].GetComt() == "") return;
            if (cDEF.ACTR[iActr].GetComt().IndexOf("SPARE",0) > 0) return;
            sTemp = "Cylinder TimeOut Error";
            sName = string.Format("{0} {1}", cDEF.ACTR[iActr].GetComt(), sTemp);

            cDEF.EPU[iErrNo].m_iGrade = 2;
            cDEF.EPU[iErrNo].m_iPart = iPart;
            cDEF.EPU[iErrNo].m_iKind = 0;
            cDEF.EPU.SetName(iErrNo, sName);
        }
        public void DefineActrManList(int iPart, int iActr, int iManNo, String sItmName = "")
        {
            String sTemp = "";
            String sName = "";
            if (iManNo < 0) return;
            if (cDEF.ACTR[iActr].GetComt() == ""                 ) return;
            if (cDEF.ACTR[iActr].GetComt().IndexOf("SPARE",0) > 0) return;

            sTemp = "Cylinder Action";

            sName = string.Format("{0} {1}", cDEF.ACTR[iActr].GetComt(), sTemp);
        }
        public void DefineCycleManList(int iPart, int iManNo, String sItmName = "")
        {
            String sName;

            if (iManNo < 0) return;
            if (sItmName == "") return;
            sName = string.Format("{0} {1}", cDEF.POSN.GetPartName(iPart), sItmName);

        }

        //Apply Device.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  ApplyProject     (string DevName)
        { 
            cDEF.FM .SysCnt.Load(true, cDEF.FM._sCrntDevice);

            //Set Motor as device.
            cDEF.MOTR.Load            (true , DevName); //선택된 DEVICE에 따른 모터 환경 설정.
            cDEF.MOTR.SetAxis_AsDevice(               );

            //
            m_sCrntDevice = DevName;

            cDEF.VISN.ApplyProject(true, DevName);

            //검사 모드 변경 
            if (cDEF.FM.ProjBase.iWaferType == 0) //Wafer
            {
                cDEF.FM.EngrOptn.bUseRingFrame1 = false;
                cDEF.FM.EngrOptn.bUseRingFrame2 = false;
                cDEF.FM.EngrOptn.bUseRingFrame3 = true;
            }
            else//ring frame
            {
                cDEF.FM.EngrOptn.bUseRingFrame1 = true;
                cDEF.FM.EngrOptn.bUseRingFrame2 = true;
                cDEF.FM.EngrOptn.bUseRingFrame3 = true;
            }

            cDEF.FM.EngrOptn.Load(false);


            //
            LoadLastInfo(false);
            
        }
        //------------------------------------------------------------------------
        public void  ApplySystem      ()
        {//UserSet - Project 변경시 System에 적용할 항목을 정의하시오 
            
        }
        //Get Information.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void       SetPojInfo  (String DevName      , String OperID) 
        { 
            //m_sLoadJobName = DevName ; 
            //m_sLoadOperID  = OperID; 
        }

		//Load.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  LoadProj        (bool IsLoad , String DevName)
        {//UserSet - Job 별로 저장해야할 항목을 정의하시오 
            ProjBase.Load       (IsLoad, DevName);
            ProjOptn.Load       (IsLoad, DevName);
            ProjOptn.LoadColor  (IsLoad         );
        }
		//Load.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  LoadLastInfo        (bool IsLoad)
        {
            String sPath;
            String sFile = "LastInfo";
            String sSection = sFile;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            sPath = Application.StartupPath + "\\System\\" + sFile + ".INI";

            if (IsLoad)
            {
                ini.Load(sPath, "LAST_INFO", "Device     ", out m_sCrntDevice    );
                ini.Load(sPath, "LAST_INFO", "OperID     ", out m_sCrntOperID    );
                //ini.Load(sPath, "LAST_INFO", "LoadDevice ", out m_sLoadJobName   );
                //ini.Load(sPath, "LAST_INFO", "LoadOperID ", out m_sLoadOperID    );

                //if (m_sCrntDevice == "") m_sCrntDevice = "NONE";  
                if (m_sCrntDevice == "") m_sCrntDevice = "Default";

            }
            else
            {
                ini.Save(sPath, "LAST_INFO", "Device     ", m_sCrntDevice    );
                ini.Save(sPath, "LAST_INFO", "OperID     ", m_sCrntOperID    );
                //ini.Save(sPath, "LAST_INFO", "LoadDevice ", m_sLoadJobName   );
                //ini.Save(sPath, "LAST_INFO", "LoadOperID ", m_sLoadOperID    );

            }
            ini = null;
        }
        //--------------------------------------------------------------------------
        public void LoadLoginSet(bool IsLoad)
        {
            String sPath;
            String sFile = "LoginSet";
            String sSection = sFile;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            sPath = Application.StartupPath + "\\System\\" + sFile + ".INI";

            if (IsLoad)
            {
                for(int i=0; i<(int)EN_LOGIN.EndOfId; i++)
                {
                    for(int j=0; j<10;j++)
                    {
                        ini.Load(sPath, "LOGIN",string.Format ("{0}EnabledMenu{1}",i,j), out LoginSet[i].bEnableMenu[j] );
                    }
                }
            }
            else
            {
                for(int i=0; i<(int)EN_LOGIN.EndOfId; i++)
                {
                    for(int j=0; j<10;j++)
                    {
                        ini.Save(sPath, "LOGIN",string.Format ("{0}EnabledMenu{1}",i,j), LoginSet[i].bEnableMenu[j] );
                    }
                }

            }
            ini = null;
        }
        //--------------------------------------------------------------------------
        public string GetRecipeFromPartNo(string PartNo)
        {
            //String sTemp;
            String sPath;
            String sRootName = "PartNoInfo";
			String sElmName  = "Recipe";
            String sNode = string.Format("{0}/{1}", sRootName, sElmName);
            string sRecpName = "";

            //Make Dir.
            //FNC.CreateDirOnWork("Error");
            sPath = Application.StartupPath + "\\PartNoInfo" + ".XML";
			
			XmlDocument xml = new XmlDocument();
			xml.Load(sPath);
			XmlNodeList xList = xml.SelectNodes(sNode);

			foreach (XmlNode xn in xList)
            {
                //ls.Add(xn.Attributes[0].InnerText);
                sRecpName = xn.Attributes[0].InnerText;
                foreach (XmlNode xnn in xn)
                {
                    //ls.Add(xnn.Name);
                    if (PartNo.ToUpper() == xnn.Name.ToUpper()) return sRecpName;
                }
            }
            //
            return "";
        }
        //------------------------------------------------------------------------
        public int GetBinWhre(string Bin)
        {
            char ch = 'A';
            int idx;
            int iWher;
            string str;

            //
            for (int n = 0; n < (int)vDEF.MAX_WORK_BIN_NO; n++)
            {
                //
                for (int i = 0; i < (int)vDEF.MAX_BIN_NO; i++)
                {
                    //Index.
                    if (i < 26)
                    {
                        idx = (int)ch + i;
                        str = Convert.ToString((char)idx);
                    }
                    else { str = string.Format("{0}", i - 26); }  //26 알파벳 수량
                                                                  //
                    iWher = n;
                    if (iWher < 0) continue;
                    
                    //if ((str == Bin) && ProjOptn.BinInfo[iWher, i]) return n;
                }
            }
            return -1;
        }
        //------------------------------------------------------------------------
        public int GetBinNo(string Bin)
        {
            char ch = 'A';
            int idx;
            int iRet = -1;
            string str;

            for (int i = 0; i < (int)vDEF.MAX_BIN_NO; i++)
            {
                //Index.
                if (i < 26)
                {
                    idx = (int)ch + i;
                    str = Convert.ToString((char)idx);
                }
                else { str = string.Format("{0}", i - 26); }  //26 알파벳 수량
                                                              //
                if (str == Bin) { iRet = i; break; }
            }
            //
            return iRet;
        }
    }
}
