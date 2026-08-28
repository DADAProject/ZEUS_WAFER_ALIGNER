using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.ComponentModel;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TLOT_INFO                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TLOT_INFO
    { //Lot
        public string     sLotNo                ;
	    public string     sLotNo1               ;
        public string     sLotNo2               ;
	    public string     sOperator             ;
	    public string     sPartNo               ;
	    public int        iInQty                ; //Input 수량
	    public int        iLoadQty              ; //Load 수량
        public int        iWorkQty              ; //Work 수량
	    public string     sJobFile              ;
		public DateTime   dtInTime              ;
		public DateTime   dtOutTime             ;
        public int        iCrntLayer            ;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLOT_INFO()
        {
            ResetData();
        }
        ~TLOT_INFO() { }

        public void ResetData()
        {
            sLotNo1          = "";   
            sLotNo2          = "";   
            sOperator        = "";  
            sPartNo          = ""; 
            sJobFile         = ""; 
            iInQty           =  0; 
            iLoadQty         =  0;
        }
        //--------------------------------------------------------------------------
        public void Load(bool isLoad)
        {
            String sPath;
            String sFile = "LotInfo";
            String sSection = sFile;
            String sName    ;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\Lot");

            sPath = Application.StartupPath + "\\System\\Lot\\" + sFile + ".INI";

            ini.Loadini(sPath);

            if (isLoad)
            {                                                                                           
                sName = sSection + "_LotNo       "; ini.Load(sPath, sSection, sName, out sLotNo       );
                sName = sSection + "_LotNo1      "; ini.Load(sPath, sSection, sName, out sLotNo1      );
                sName = sSection + "_LotNo2      "; ini.Load(sPath, sSection, sName, out sLotNo2      );
                sName = sSection + "_Operator    "; ini.Load(sPath, sSection, sName, out sOperator    );
                sName = sSection + "_PartNo      "; ini.Load(sPath, sSection, sName, out sPartNo      );
                sName = sSection + "_JobFile     "; ini.Load(sPath, sSection, sName, out sJobFile     );
                sName = sSection + "_InQty       "; ini.Load(sPath, sSection, sName, out iInQty       );
                sName = sSection + "_LoadQty     "; ini.Load(sPath, sSection, sName, out iLoadQty     );
                
            }
            else
            {                                                 
                sName = sSection + "_LotNo       "; ini.Save(sPath, sSection, sName, sLotNo         );
                sName = sSection + "_LotNo1      "; ini.Save(sPath, sSection, sName, sLotNo1        );
                sName = sSection + "_LotNo2      "; ini.Save(sPath, sSection, sName, sLotNo2        );
                sName = sSection + "_Operator    "; ini.Save(sPath, sSection, sName, sOperator      );
                sName = sSection + "_PartNo      "; ini.Save(sPath, sSection, sName, sPartNo        );
                sName = sSection + "_JobFile     "; ini.Save(sPath, sSection, sName, sJobFile       );
                sName = sSection + "_InQty       "; ini.Save(sPath, sSection, sName, iInQty         );
                sName = sSection + "_LoadQty     "; ini.Save(sPath, sSection, sName, iLoadQty       );

                ini.Saveini(sPath);

            }
            ini = null; 
        }
    };

    /***************************************************************************/
    /* Class: TLOT_QTY                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    //Working Quantity.
    //---------------------------------------------------------------------------
    public class TLOT_QTY 
    {   	

        //Lot Loading Count
        public int[]	iLoadPVICnt    = new int[vDEF.MAX_PVI        ]; //PVI Vile Loading시 PVI Good , Fail Count
        public int[]	iLoadBinCnt    = new int[vDEF.MAX_WORK_BIN_NO]; //File Loading 시 해당 Bin Count (Prober가 완료된 Wafer File Loading시 사용)

        //Tool 관련                 
        public int[]    iPickCnt      = new int[(int)EN_TOOL_ID.EndOfId];
        public int[]    iPlceCnt      = new int[(int)EN_TOOL_ID.EndOfId];
        public int[]    iPickNGCnt    = new int[(int)EN_TOOL_ID.EndOfId];
        public int[]    iPlceNGCnt    = new int[(int)EN_TOOL_ID.EndOfId];	

        //Visn 관련
        public int[]    iVisnGDCnt     = new int[(int)EN_CAM.EndofCam];
        public int[]    iVisnNGCnt     = new int[(int)EN_CAM.EndofCam];

		//NG 수량
        public int[,]   iBinNGCnt      = new int   [vDEF.MAX_WORK_BIN_NO, (int)EN_CAM.EndofCam];
		public int      iNGPlaceCnt;

		//누적 수량.
		public int      iStckLoadPVICnt;
		public int      iStckWafWorkQty;             
        public int[]    iStckPickCnt      = new int[(int)EN_TOOL_ID.EndOfId];
        public int[]    iStckPlceCnt      = new int[(int)EN_TOOL_ID.EndOfId];
        public int[]    iStckPickNGCnt    = new int[(int)EN_TOOL_ID.EndOfId];
        public int[]    iStckPlceNGCnt    = new int[(int)EN_TOOL_ID.EndOfId];	
        public int[]    iStckVisnGDCnt    = new int[(int)EN_CAM.EndofCam   ];
        public int[]    iStckVisnNGCnt    = new int[(int)EN_CAM.EndofCam   ];

		//Wafer 관련
		public int      iLoadQty      ;     
        public int      iUnloadQty    ;     
        public int      iWorkQty      ; //Case Work Count   

        public int      iWafLoadCnt   ;     
        public int      iWafWorkQty   ;       

                                         
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLOT_QTY(int Kind)
        {
            //
            ResetData();
        }
        ~TLOT_QTY() { }

        public void ResetData()
        {
			iLoadQty     = 0;
            iUnloadQty   = 0;
            iWorkQty     = 0;

			for (int n = 0; n < vDEF.MAX_PVI        ; n++) iLoadPVICnt[n] = 0;
			for (int n = 0; n < vDEF.MAX_WORK_BIN_NO; n++) iLoadBinCnt[n] = 0;

			iWafLoadCnt     = 0;
			iWafWorkQty     = 0;
			iStckWafWorkQty = 0;
			iStckLoadPVICnt = 0;

			for (int n = 0; n < (int)EN_TOOL_ID.EndOfId; n++)
			{
				 iPickCnt  [n] = 0; iStckPickCnt  [n] = 0;
				 iPlceCnt  [n] = 0;	iStckPlceCnt  [n] = 0;
				 iPickNGCnt[n] = 0;	iStckPickNGCnt[n] = 0;
				 iPlceNGCnt[n] = 0;	iStckPlceNGCnt[n] = 0;
			}

            for(int n=0; n<(int)EN_CAM.EndofCam; n++)
            {
                iVisnGDCnt [n] = 0; iStckVisnGDCnt[n] = 0;
                iVisnNGCnt [n] = 0;	iStckVisnNGCnt[n] = 0;
            } 
			//
			for(int i=0; i<vDEF.MAX_WORK_BIN_NO; i++) {
				for(int j=0; j<(int)EN_CAM.EndofCam; j++) {
					iBinNGCnt[i, j] = 0;
					}
				}
			iNGPlaceCnt = 0;




        }
        //------------------------------------------------------------------------
        public void WorkQtyInc()
        {
            iWorkQty++;
        }
        public void LoadQtyInc()
        {
            iLoadQty++;
        }
        public void UnloadQtyInc()
        {
            iUnloadQty++;
        }
        //------------------------------------------------------------------------
        public void Load(bool isLoad)
        {
            string sType = "Lot";
            string sPath;
            string sFile = string.Format("{0}Qty", sType);
            string sSection = sFile;
            string sName = string.Empty   ;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\Lot");

            sPath = Application.StartupPath + "\\System\\Lot\\" + sFile + ".INI";

            ini.Loadini(sPath);

            if (isLoad)
            {                  
                sName = sSection + "_LoadQty       "      ; ini.Load(sPath, sSection, sName, out iLoadQty    );
				sName = sSection + "_UnloadQty     "      ; ini.Load(sPath, sSection, sName, out iUnloadQty  );
				sName = sSection + "_WorkQty       "      ; ini.Load(sPath, sSection, sName, out iWorkQty    );
            }
            else
            {
				sName = sSection + "_LoadQty       "      ; ini.Save(sPath, sSection, sName,    iLoadQty    );
                sName = sSection + "_UnloadQty     "      ; ini.Save(sPath, sSection, sName,    iUnloadQty  );
                sName = sSection + "_WorkQty       "      ; ini.Save(sPath, sSection, sName,    iWorkQty    );

                ini.Saveini(sPath);
            }

            ini = null;
        }
    };

    /***************************************************************************/
    /* Class: TLOT_TIME                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    //---------------------------------------------------------------------------
    public class TLOT_TIME
    { //Lot
        int            iTimeKind       ; //0 : Lot, 1 : Wafer, 2 : Day

	    public double  dLotStrtTime   ; 
	    public double  dLotEndTime    ; 
	    public double  dLotRunTime    ; 
	    public double  dLotMCJamTime  ; 
        public double  dLotHMJamTime  ; 
	    public double  dLotMLJamTime  ; 
        public double  dLotMDJamTime  ; 
        public double  dLotStopTime   ; 
        public double  dLotIdleTime   ; 
	    public double  dLotJamTime    ; 
	    public double  dLotInTime     ; 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLOT_TIME(int Type)
        {
            iTimeKind = Type;
            //
            ResetData();
        }
        ~TLOT_TIME() { }

        public void ResetData()
        {
            dLotStrtTime    = 0.0;
            dLotEndTime     = 0.0;
            dLotRunTime     = 0.0;
            dLotMCJamTime   = 0.0;
            dLotHMJamTime   = 0.0;
            dLotMLJamTime   = 0.0;
            dLotMDJamTime   = 0.0;
            dLotStopTime    = 0.0;
            dLotIdleTime    = 0.0;
            dLotJamTime     = 0.0;
            dLotInTime      = 0.0;
        }
        public void Load(bool isLoad)
        {
            String sType;
            String sPath;
            String sFile ; //= "Time";
            String sSection; // = sFile;
            String sName    ;
            TIniUnit ini = new TIniUnit();

            //
            if      (iTimeKind == 0) sType = "Lot"  ;
            else if (iTimeKind == 1) sType = "Wafer";
            else                     sType = "Day"  ;
            sFile = string.Format("{0}Time", sType);
            sSection = sFile;

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\Lot");

            sPath = Application.StartupPath + "\\System\\Lot\\" + sFile + ".INI";

            if (isLoad)
            {
                sName = sSection + "_LotStrtTime "; ini.Load(sPath, sSection, sName, out dLotStrtTime  );
                sName = sSection + "_LotEndTime  "; ini.Load(sPath, sSection, sName, out dLotEndTime   );
                sName = sSection + "_LotRunTime  "; ini.Load(sPath, sSection, sName, out dLotRunTime   );
                sName = sSection + "_LotMCJamTime"; ini.Load(sPath, sSection, sName, out dLotMCJamTime );
                sName = sSection + "_LotHMJamTime"; ini.Load(sPath, sSection, sName, out dLotHMJamTime );
                sName = sSection + "_LotMLJamTime"; ini.Load(sPath, sSection, sName, out dLotMLJamTime );
                sName = sSection + "_LotMDJamTime"; ini.Load(sPath, sSection, sName, out dLotMDJamTime );
                sName = sSection + "_LotStopTime "; ini.Load(sPath, sSection, sName, out dLotStopTime  );
                sName = sSection + "_LotIdleTime "; ini.Load(sPath, sSection, sName, out dLotIdleTime  );
                sName = sSection + "_LotJamTime  "; ini.Load(sPath, sSection, sName, out dLotJamTime   );
                sName = sSection + "_LotInTime   "; ini.Load(sPath, sSection, sName, out dLotInTime    );
            }
            else
            {

                sName = sSection + "_LotStrtTime "; ini.Save(sPath, sSection, sName, dLotStrtTime  );
                sName = sSection + "_LotEndTime  "; ini.Save(sPath, sSection, sName, dLotEndTime   );
                sName = sSection + "_LotRunTime  "; ini.Save(sPath, sSection, sName, dLotRunTime   );
                sName = sSection + "_LotMCJamTime"; ini.Save(sPath, sSection, sName, dLotMCJamTime );
                sName = sSection + "_LotHMJamTime"; ini.Save(sPath, sSection, sName, dLotHMJamTime );
                sName = sSection + "_LotMLJamTime"; ini.Save(sPath, sSection, sName, dLotMLJamTime );
                sName = sSection + "_LotMDJamTime"; ini.Save(sPath, sSection, sName, dLotMDJamTime );
                sName = sSection + "_LotStopTime "; ini.Save(sPath, sSection, sName, dLotStopTime  );
                sName = sSection + "_LotIdleTime "; ini.Save(sPath, sSection, sName, dLotIdleTime  );
                sName = sSection + "_LotJamTime  "; ini.Save(sPath, sSection, sName, dLotJamTime   );
                sName = sSection + "_LotInTime   "; ini.Save(sPath, sSection, sName, dLotInTime    );
            }
        }

    };

    /***************************************************************************/
    /* Class: TLotUnit                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TLotUnit
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
		//DataTable tbQtyInfo = new DataTable();

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
		//Buffer
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public TLOT_INFO Info   = new TLOT_INFO( );
        public TLOT_TIME Time   = new TLOT_TIME(0);
        public TLOT_QTY  LotQty = new TLOT_QTY (0);

		//Vars.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//Lot Flag
		bool      m_bLotOpen                    ;
		bool      m_bLotEnded                   ; //작업 완료.
		bool      m_bReqLotEnd                  ; //Lot 작업 종료하기 위한 Request.
		bool      m_bLotCancel                  ;
		bool      m_bRetest                     ;
		int       m_iWorkMode                   ;
		int       m_iJamQty                     ;

		//Buffer
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		TOnDelayTimer    m_tWait       = new TOnDelayTimer();



        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool _bLotOpen   { get { return m_bLotOpen  ; } set { m_bLotOpen   = value; } }
        public bool _bLotEnded  { get { return m_bLotEnded ; } set { m_bLotEnded  = value; } }
        public bool _bReqLotEnd { get { return m_bReqLotEnd; } set { m_bReqLotEnd = value; } }
        public bool _bLotCancel { get { return m_bLotCancel; } set { m_bLotCancel = value; } }
        public bool _bRetest    { get { return m_bRetest   ; } set { m_bRetest    = value; } }
        public int  _iWorkMode  { get { return m_iWorkMode ; } set { m_iWorkMode  = value; } }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLotUnit()
        {

        }
        ~TLotUnit() { }

		//Init.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void    Init              (int iLotNum)
        {
        }
		//Watch Lot.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void    ClearLot          ()
        {
        }


		//Lot Processing.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    LotOpen           (TLOT_INFO TmpLotInfo)
        {
 			//
			m_tWait.Clear();
			//
			//while (true)
			//{
			//	if (m_tWait.OnDelay(true, 5000))
			//	{
			//		cDEF.EPU.SetErr(1450, true); //Lot Open - Vision 통신 응답 실패.
			//		return false;
			//	System.Windows.Forms.Application.DoEvents();
			//	//
			//	break;
			//}

            Info.sJobFile  = TmpLotInfo.sJobFile  ;
            Info.sLotNo1   = TmpLotInfo.sLotNo1   ;  
            Info.sLotNo2   = TmpLotInfo.sLotNo2   ;  
            Info.sOperator = TmpLotInfo.sOperator ;
            Info.sPartNo   = TmpLotInfo.sPartNo   ;
			//	}
			Info.dtInTime  = DateTime.Now         ;
			Info.dtOutTime = DateTime.Now         ;
            
			//cDEF.DM .ClearMap    ();
            cDEF.SEQ.ClearWorkEnd();
            cDEF.SEQ._bLoadStop = false;
            LotQty  .ResetData   ();
            Time    .ResetData   ();

            m_bLotOpen   = true;
            m_bLotEnded  = false;
			m_bLotCancel = false;
            m_bReqLotEnd = false;

            //
            //for (int n = 0; n < vDEF.MAX_TR_MODULE; n++) SetRedayTR(n , true);			
            cDEF.FM.LoadProj    (true, Info.sJobFile);
            cDEF.FM.ApplyProject(Info.sJobFile);
            LoadLot(false);
            //
            cDEF.FM.SysOptn.InitSysOptn();
            cDEF.FM.EngrOptn.Load(false);
            //
            string sTemp = string.Format("[LOT OPEN] {0}, {1} / OPERATOR : {2}", Info.sLotNo1, Info.sLotNo2, Info.sOperator);
            cDEF.LOG.Trace   (sTemp);
            cDEF.LOG.SeqTrace(sTemp);
            
            return true;
        }
        //------------------------------------------------------------------------
        public bool    LotEnd            (bool bContinuous = false)
        {
			//
			m_tWait.Clear();
			//
			while (true)
			{
				if (m_tWait.OnDelay(true, 5000))
				{
					//cDEF.EPU.SetErr(1450, true); //LotEnd - Vision 통신 응답 실패.
					//return false;
				}
				System.Windows.Forms.Application.DoEvents();
				//
				break;
			}

			Info.dtOutTime = DateTime.Now;
            
            string sTemp = string.Format("[LOT END] {0}, {1}", Info.sLotNo1, Info.sLotNo2);
            cDEF.LOG.Trace(sTemp);
            cDEF.LOG.SeqTrace(sTemp);


            //Reset WorkEnd Flag.
            cDEF.SEQ._bLoadStop = false;
            cDEF.SEQ.ClearWorkEnd();

            
            //
            if (bContinuous)
            {//
                //Reset Lot Flag.
                m_bLotOpen   = true  ;
                m_bLotEnded  = false ;
            }
            else
            {//Lot End
                if (!m_bLotOpen) return false;
                //Reset Lot Flag.
                m_bLotOpen   = false;
                m_bLotEnded  = true ;
                

                //FRM.ShowMsg(true, "Confirm", "LOT이 종료되었습니다.");
                //if (cDEF.SEQ._bRun) cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0999, true);

            }
            return true;
        }
        //------------------------------------------------------------------------
        public void    FrceLotEnd        ()
        {
            m_bLotOpen  = false;
            m_bLotEnded = true ;
        }
        //------------------------------------------------------------------------
        public void    LotCancel         ()
        {
			m_bLotCancel = true ;
            //m_bLotOpen   = false;
            //m_bLotEnded  = true ;

            cDEF.LOG.SeqTrace($"[LOT CANCEL] {Info.sLotNo1},{Info.sLotNo2}");

        }
        //------------------------------------------------------------------------
		public bool ReqLotEnd()
        {
            return false;
        }

        //Time Process
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void AddLotRunTime(double Count)
        {
            Time.dLotRunTime += Count;
        }
        public void AddLotMCJamTime(double Count)
        {
            Time.dLotMCJamTime += Count;
        }
        public void AddLotHMJamTime(double Count)
        {
            Time.dLotHMJamTime += Count;
        }
        public void AddLotMLJamTime(double Count)
        {
            Time.dLotMLJamTime += Count;
        }
        public void AddLotMDJamTime(double Count)
        {
            Time.dLotMDJamTime += Count;
        }
        public void AddLotStopTime(double Count)
        {
            Time.dLotStopTime += Count;
        }
        public void AddLotIdleTime(double Count)
        {
            Time.dLotIdleTime += Count;
        }
        public double GetUPEH()
        {
            //TimeSpan sp = new TimeSpan();
            //
            //if (m_bLotOpen && !m_bLotEnded) sp = TimeSpan.FromMilliseconds(Time.dLotRunTime); //DateTime.Now   - Info.dtInTime;
            //else                            sp = Info.dtOutTime - Info.dtInTime;

            return cDEF.SPC.CalUPEH(TimeSpan.FromMilliseconds(Time.dLotRunTime), LotQty.iWorkQty);
        }
        public double GetUPH(double TactTime)
        {
            double dUph;

            if (TactTime <= 0) return 0;
            dUph = 3600 / TactTime;
            //
            return dUph;
        }
        //File Processing.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void LoadLot(bool isLoad)
        {
            Load         (isLoad);
            Info  .Load  (isLoad);
            LotQty.Load  (isLoad);
            Time  .Load  (isLoad);
        }

        public void Load(bool isLoad)
        {
            String sPath;
            String sFile = "LotInfo";
            String sSection = sFile;
            String sName    ;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            FNC.CreateDirOnWork("System\\Lot");

            sPath = Application.StartupPath + "\\System\\Lot\\" + sFile + ".INI";

            if (isLoad)
            {
                sName = sSection + "_LotOpen     "; ini.Load(sPath, sSection, sName, out m_bLotOpen           );
                sName = sSection + "_LotEnded    "; ini.Load(sPath, sSection, sName, out m_bLotEnded          );
                sName = sSection + "_ReqLotEnd   "; ini.Load(sPath, sSection, sName, out m_bReqLotEnd         );
                sName = sSection + "_LotCancel   "; ini.Load(sPath, sSection, sName, out m_bLotCancel         );
                sName = sSection + "_Retest      "; ini.Load(sPath, sSection, sName, out m_bRetest            );
                sName = sSection + "_WorkMode    "; ini.Load(sPath, sSection, sName, out m_iWorkMode          );
                sName = sSection + "_JamQty      "; ini.Load(sPath, sSection, sName, out m_iJamQty            );
            }
            else
            {
                sName = sSection + "_LotOpen     "; ini.Save(sPath, sSection, sName, m_bLotOpen             );
                sName = sSection + "_LotEnded    "; ini.Save(sPath, sSection, sName, m_bLotEnded            );
                sName = sSection + "_ReqLotEnd   "; ini.Save(sPath, sSection, sName, m_bReqLotEnd           );
                sName = sSection + "_LotCancel   "; ini.Save(sPath, sSection, sName, m_bLotCancel           );
                sName = sSection + "_Retest      "; ini.Save(sPath, sSection, sName, m_bRetest              );
                sName = sSection + "_WorkMode    "; ini.Save(sPath, sSection, sName, m_iWorkMode            );
                sName = sSection + "_JamQty      "; ini.Save(sPath, sSection, sName, m_iJamQty              );
            }
        }
        //------------------------------------------------------------------------
        public void DisplyQty(ref System.Windows.Forms.Panel Panel, int Type)
        {
            //int     iNGCnt = 0;
            //double  dYield;
            //System.Windows.Forms.Label lb;
            //for (int i = 1; i <= Panel.Controls.Count; i++)
            //{
            //    lb = Panel.Controls["lbQty1"] as System.Windows.Forms.Label; if (lb != null) lb.Text = string.Format("{0}", (Type == 0) ? WafQty.iLoadPVICnt[2] : LotQty.iLoadPVICnt[2]);
            //    lb = Panel.Controls["lbQty2"] as System.Windows.Forms.Label; if (lb != null) lb.Text = string.Format("{0}", (Type == 0) ? WafQty.iWafWorkQty    : LotQty.iWafWorkQty   );
            //    for (int n = 0; n < (int)EN_CAM.EndOfId; n++)
            //    { 
            //        lb = Panel.Controls[string.Format("lbQty{0}", n + 3)] as System.Windows.Forms.Label; if (lb != null) lb.Text = string.Format("{0}", (Type == 0) ? WafQty.iVisnNGCnt[n]: LotQty.iVisnNGCnt[n]);
            //        iNGCnt += (Type == 0) ? WafQty.iVisnNGCnt[n]: LotQty.iVisnNGCnt[n];
            //    }
            //    //
            //    dYield = FNC.GetUserYield1((Type == 0) ? WafQty.iWafLoadCnt : LotQty.iWafLoadCnt, iNGCnt);
            //    lb = Panel.Controls["lbQty20"] as System.Windows.Forms.Label; if (lb != null) lb.Text = string.Format("{0:F2}%", dYield);
            //}            
        }
    }
}

