using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace eMachine
{
    //Chip Mask Kind.
    //===========================================================================
    public enum EN_MASK_KIND  : int
    {
        None      ,
        Manual    ,
        ContiFail ,
        LowYield  ,
        LimitOver ,
        PrimeYield,
        ContiLot
    };

    /***************************************************************************/
    /* Class: TSORT_INFO                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    //Sort Info
    //===========================================================================
    public class TSORT_INFO
    {
        public bool    bFind;
        public int     iFindMode;
        public int     iTopId   ;
        public int     iBtmId   ;
        public int     iBtmKind ;
        public int     iRShift  ;
        public int     iCShift  ;
        public int     iNShift  ;//Nozzle Shift
        public int     iFindRow ;
        public int     iFindCol ;
        public int     iDownCnt ;
        //public bool[]  bDown = new bool[vDEF.MAX_NOZL];
		public List<double> List = new List<double>();

                
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSORT_INFO()
        {
            //Init. SORT_INFO
            Init();
        }
        ~TSORT_INFO() { }
        //------------------------------------------------------------------------
        public TSORT_INFO Copy()
        {
			return FNC.DeepClone(this) as TSORT_INFO;
			//return this.MemberwiseClone() as TSORT_INFO;
        }
        //------------------------------------------------------------------------
        public void Init () 
        {
            bFind    = false;
            iFindRow = -1;
            iFindCol = -1;
            iRShift  = 0;
            iCShift  = 0;
            iNShift  = 0;
            //for (int i = 0; i < vDEF.MAX_NOZL; i++) bDown[i] = false;
        }
    };

    /***************************************************************************/
    /* Class: TChip                                                            */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    [Serializable()]
    public class TChip
    {

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
		int             m_iLayer     ;
		int             m_iPviBin    ;
		string          m_sBin       ;
        int             m_iBinWhre   ;
		//byte[]          m_byBin      = new byte[vDEF.MAX_BIN_LENGTH];
        bool            m_bAligned   ; 
        bool            m_bPreAligned;
		bool            m_bScaned    ;
        bool            m_bDeg90     ;
        EN_CHIP_STAT	m_iStat      ;
		EN_CHIP_STAT	m_iStatData  ;
		EN_CHIP_STAT	m_iStatScan  ;
        EN_CHIP_RSLT[]	m_iRslt   = new EN_CHIP_RSLT[(int)EN_RSLT_KIND.EndOfId];
		Point			m_CordPvi = new Point(); //Coordinate        on loading .

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
		
        public _TBLE_POSN   m_PosnCrntAlgn  = new _TBLE_POSN(); //Current Aligned Position           (Absolute position in um)
        public _TBLE_POSN   m_PosnPreAlgn   = new _TBLE_POSN(); //
		public _TBLE_POSN   m_PosnScan      = new _TBLE_POSN(); //

		//Spare Var.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool    m_bSpare1, m_bSpare2;
        int     m_iSpare1, m_iSpare2;
        double  m_dSpare1, m_dSpare2;


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public int			 _iLayer      { get { return m_iLayer       ; } set { m_iLayer        = value; } }  
		public int			 _iBinWhre    { get { return m_iBinWhre     ; } set { m_iBinWhre      = value; } }
		public string    	 _sBin        { get { return m_sBin         ; } set { m_sBin          = value; } }
		public int			 _CordPviX    { get { return m_CordPvi.X    ; } set { m_CordPvi.X     = value; } }
		public int			 _CordPviY    { get { return m_CordPvi.Y    ; } set { m_CordPvi.Y     = value; } }
		public int			 _iPviBin     { get { return m_iPviBin      ; } set { m_iPviBin       = value; } }
        public bool          _bDeg90      { get { return m_bDeg90       ; } set { m_bDeg90        = value; } }
        public EN_CHIP_STAT _iStat        { get { return m_iStat        ; } set { m_iStat         = value; } }
		public EN_CHIP_STAT _iStatData    { get { return m_iStatData    ; } set { m_iStatData     = value; } }
		public EN_CHIP_STAT _iStatScan    { get { return m_iStatScan    ; } set { m_iStatScan     = value; } } 
        public bool         _bAligned     { get { return m_bAligned     ; } set { m_bAligned      = value; } }
        public bool         _bPreAligned  { get { return m_bPreAligned  ; } set { m_bPreAligned   = value; } }
		public bool         _bScaned      { get { return m_bScaned      ; } set { m_bScaned       = value; } }

		public int          gVRslt(EN_RSLT_KIND RsltKind                   ) { if (RsltKind < 0) return 0; return (int)m_iRslt[(int)RsltKind]; }
		//public string       gBin  (                                        ) { return Encoding.ASCII.GetString(m_byBin); }

		public void         sVRslt(EN_RSLT_KIND RsltKind, EN_CHIP_RSLT Rslt) { if (RsltKind < 0) return  ; m_iRslt[(int)RsltKind] = Rslt; }
		//public void         sBin  (string Bin                              ) { m_byBin = Encoding.ASCII.GetBytes(Bin); }	


        
        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TVisnRslt[]  VisnRslt = new TVisnRslt[(int)EN_CAM.EndofCam]; //Vision 갯수만큼 생성하지 않음.

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TChip()
        {
            for(int i=0; i<(int)EN_CAM.EndofCam; i++)
			{
                VisnRslt[i] = new TVisnRslt();
			}

            Init();
        }
        ~TChip() { }

        public TChip Copy()
        {
			return FNC.DeepClone(this) as TChip;
            //return this.MemberwiseClone() as TChip;
        }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
            m_iStat          = EN_CHIP_STAT.None;
			m_iStatData		 = EN_CHIP_STAT.None;
			m_iStatScan		 = EN_CHIP_STAT.None;

			m_iLayer         = 0 ;
            m_iBinWhre       = -1;  
			m_iPviBin        = 0 ;
			m_sBin           = "";      
			//m_byBin          = Encoding.ASCII.GetBytes("0");
            m_bAligned       = false;   
            m_bPreAligned    = false;
			m_bScaned        = false;
            m_bDeg90         = false;        

            m_CordPvi.X      = 0; 
            m_CordPvi.Y      = 0;

            m_PosnCrntAlgn.ResetData();
			m_PosnPreAlgn .ResetData();
			m_PosnScan    .ResetData();

            for(int i=0;i<(int)EN_RSLT_KIND.EndOfId ;i++) m_iRslt [i] = EN_CHIP_RSLT.None;                
			for(int i=0;i<(int)EN_CAM      .EndofCam;i++) VisnRslt[i].ResetData();                
        }   
        //------------------------------------------------------------------------
 
        public void ClearAlign    () { m_bAligned     = false; }
		public void ClearPreAlign () { m_bPreAligned  = false; }
		public void ClearScan     () { m_bScaned      = false; }

        //Check Chip Status & Result.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsExist() 
        { 
            bool isExist =  (m_iStat != EN_CHIP_STAT.Empty) && (m_iStat != EN_CHIP_STAT.None) && (m_iStat != EN_CHIP_STAT.Mask) && (m_iStat != EN_CHIP_STAT.Skip);
			//
            return isExist;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsStat    (EN_CHIP_STAT Stat)
        {
            return (m_iStat == Stat);
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsRslt       (EN_CHIP_RSLT iRslt, EN_RSLT_KIND iRsltNo)
        {
			int iRsltKind = (int)iRsltNo;
			//
            if (iRsltNo < 0 || iRsltNo >= EN_RSLT_KIND.EndOfId) return false;
			//
            return (m_iRslt[iRsltKind] == iRslt);
        }
		public bool IsGood      (EN_RSLT_KIND iRsltNo)
        {
			if (!IsStat(EN_CHIP_STAT.Rslt)) return false;

            if (m_iRslt[(int)iRsltNo] != EN_CHIP_RSLT.Good) return false;
			//
            return true;
        }
        public bool IsFail      (EN_RSLT_KIND iRsltNo)
        {
			if (!IsStat(EN_CHIP_STAT.Rslt)) return false;

            if (m_iRslt[(int)iRsltNo] == EN_CHIP_RSLT.Fail) return true;
			//
            return false;
        }
        public bool IsGood      ()
        {
			if (!IsStat(EN_CHIP_STAT.Rslt)) return false;

            for(int i=0;i<(int)EN_RSLT_KIND.EndOfId;i++)
			{
				if (m_iRslt[i] == EN_CHIP_RSLT.Fail) return false;
			}
			//
            return true;
        }
        public bool IsFail      ()
        {
			if (!IsStat(EN_CHIP_STAT.Rslt)) return false;

            for(int i=0;i<(int)EN_RSLT_KIND.EndOfId;i++)
			{
				if (m_iRslt[i] == EN_CHIP_RSLT.Fail) return true;
			}
			//
            return false;
        }
        public bool IsWait      ()
        {
			if (!IsStat(EN_CHIP_STAT.Rslt)) return false;

            for(int i=0;i<(int)EN_RSLT_KIND.EndOfId;i++)
			{
				if (m_iRslt[i] == EN_CHIP_RSLT.Wait) return true;
			}
			//
            return false;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsRslt       (EN_CHIP_STAT iStat, EN_CHIP_RSLT iRslt, EN_RSLT_KIND iRsltNo)
        {
			int iRsltKind = (int)iRsltNo;
			//
            if (iRsltNo < 0 || iRsltNo >= EN_RSLT_KIND.EndOfId) return false;
			//
            return ((m_iStat==iStat) && (m_iRslt[iRsltKind] == iRslt));
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_CHIP_RSLT GetRslt(EN_RSLT_KIND iRsltNo) 
        {
			int iRsltKind = (int)iRsltNo;
			//
			if (iRsltNo < 0 || iRsltNo >= EN_RSLT_KIND.EndOfId) return EN_CHIP_RSLT.None;
			//
            return m_iRslt[iRsltKind];
        }
        public void SetRslt(EN_RSLT_KIND iRsltNo, EN_CHIP_RSLT Rslt) 
        {
			int iRsltKind = (int)iRsltNo;
			//
			if (iRsltNo < 0 || iRsltNo >= EN_RSLT_KIND.EndOfId) return;
			//
            m_iRslt[iRsltKind] = Rslt;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public string GetRsltText() 
        { 
            return ""; 
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public Color GetBinColor(EN_OBJ_KIND Obj) 
        {//UserSet - Chip Status DISPLAY 색깔 처리
            Color iBinColor = Color.Black;

                 if (m_iStat == EN_CHIP_STAT.None         ) iBinColor = (Obj == EN_OBJ_KIND.WAFER) ? Color.Black : Color.White  ;
            else if (m_iStat == EN_CHIP_STAT.Mask         ) iBinColor = Color.SkyBlue  ;
			else if (m_iStat == EN_CHIP_STAT.Skip         ) iBinColor = Color.Silver   ;
            else if (m_iStat == EN_CHIP_STAT.Empty        ) iBinColor = Color.Gray     ;
            else if (m_iStat == EN_CHIP_STAT.Mount        ) iBinColor = Color.Aqua     ;
			else if (m_iStat == EN_CHIP_STAT.GScan        ) iBinColor = Color.MediumAquamarine;
			else if (m_iStat == EN_CHIP_STAT.FScan        ) iBinColor = Color.Maroon   ;
			else if (m_iStat == EN_CHIP_STAT.PFail        ) iBinColor = Color.Tomato   ;
			else if (m_iStat == EN_CHIP_STAT.Start        ) iBinColor = Color.SteelBlue;
            else if (m_iStat == EN_CHIP_STAT.Fnsh         ) iBinColor = Color.Purple   ;
            else if (m_iStat == EN_CHIP_STAT.Rslt         ) 
			{
				if      (IsWait()) iBinColor = Color.Yellow ; 
				else if (IsFail()) iBinColor = Color.Red    ;
				else if (IsGood()) iBinColor = Color.Green  ;
                else		       iBinColor = Color.SkyBlue; 
            }
			//
            return iBinColor; 
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public Color GetLineColor()
        {//UserSet - Chip OPTIN DISPLAY 색깔 처리 
            return Color.Gray;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool FindChip(EN_FIND FindMode, int Layer = -1)
        {
			bool isOk = false;
            //Find.
            switch (FindMode)
            {
                //존재 여부 만 확인.
                case EN_FIND.Exist    : isOk = IsExist();												   break;
                case EN_FIND.Mask     : isOk = IsStat (EN_CHIP_STAT.Mask )								 ; break;
				case EN_FIND.Skip     : isOk = IsStat (EN_CHIP_STAT.Skip )								 ; break;
                case EN_FIND.Empty    : isOk = IsStat (EN_CHIP_STAT.Empty)								 ; break;
                case EN_FIND.Start    : isOk = IsStat (EN_CHIP_STAT.Start)								 ; break;
                case EN_FIND.Mount    : isOk = IsStat (EN_CHIP_STAT.Mount)								 ; break;
                case EN_FIND.GScan    : isOk = IsStat (EN_CHIP_STAT.GScan)								 ; break;
                case EN_FIND.Rslt     : isOk = IsStat (EN_CHIP_STAT.Rslt )								 ; break;
                case EN_FIND.Fnsh     : isOk = IsStat (EN_CHIP_STAT.Fnsh )								 ; break;
																										 
				case EN_FIND.RsltGood : isOk = IsStat (EN_CHIP_STAT.Rslt ) && IsGood()					 ; break;
				case EN_FIND.RsltFail : isOk = IsStat (EN_CHIP_STAT.Rslt ) && IsFail()					 ; break;
				case EN_FIND.SkipMask : isOk = IsStat (EN_CHIP_STAT.Mask ) || IsStat (EN_CHIP_STAT.Skip ); break; 
            }
			if (Layer <= 0)   return isOk;
			else			{ return isOk && (Layer == m_iLayer); }
        }

        //Set Chip.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetTo(EN_CHIP_STAT Stat, EN_CHIP_RSLT Rslt, EN_RSLT_KIND iRsltNo = EN_RSLT_KIND.All)
        {			
			//
			int iRsltKind = (int)iRsltNo;
			//
            m_iStat = Stat;        
			//
			if (iRsltNo > EN_RSLT_KIND.None)
			{
				if (iRsltNo == EN_RSLT_KIND.All)
				{
					for(int i=0;i<(int)EN_RSLT_KIND.EndOfId;i++) m_iRslt[i] = Rslt; //EN_CHIP_RSLT.None;   
				}
				else if (iRsltNo < EN_RSLT_KIND.EndOfId) m_iRslt[(int)iRsltNo] = Rslt;
				//else  return; 			
			}
			else 
			{
				for (int n = 0; n < (int)EN_RSLT_KIND.EndOfId; n++)
				{
					 if (m_iRslt[n] == EN_CHIP_RSLT.None) { m_iRslt[n] = Rslt; break; }
				}
			}
			//
            if (Stat == EN_CHIP_STAT.None) Init();
        }

        //Sorting을 위해 Align한 Vision 결과를 저장.
        //---------------------------------------------------------------------------
        public void SetVisnRslt(EN_RSLT_KIND iRsltNo, TVisnRslt VRslt)
        {
			//
			int iRsltKind = (int)iRsltNo;
			//
            if(iRsltNo<0 || iRsltNo>=EN_RSLT_KIND.EndOfId) return;
            //Check Range.
            VisnRslt[iRsltKind] = (TVisnRslt)VRslt.Copy();
        }

        //Update Pin Map
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void UpdateInfo(ref System.Windows.Forms.DataGridView pGrid, bool reDraw = false)
        {
            int i;
            int iTotWidth    = 0;
	        int[]     iWidth = {0, 200};
	        string[]  sItem  = {"NAME", "VALUE"};
            string    sName, sValue;

			if (pGrid == null) return;

            pGrid.Visible   = false;
            if(pGrid.RowCount == 0 || reDraw) 
            {
                FNC.SetGridStyle(ref pGrid, 30, false, true, false, DataGridViewSelectionMode.FullRowSelect);
                for (i = 0; i < 2; i++)
                {
                    pGrid.Columns.Add(sItem[i], sItem[i]);
                    pGrid.Columns[i].Width = iWidth[i];
                    iTotWidth += iWidth[i];
                }
                pGrid.Columns[0].Width = pGrid.Width - iTotWidth - 20;
                pGrid.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                pGrid.Columns[0].DefaultCellStyle.BackColor = Color.Silver;
                for (i = 0; i < vDEF.MAX_TEST_ITEM + 11; i++)
                {
                    if (!UpdateListVal(i, out sName, out sValue)) break;
                    pGrid.Rows.Add(sName, sValue);
                }
            }
            else
            {
                for (i = 0; i < pGrid.RowCount; i++)
                {
                    if (!UpdateListVal(i, out sName, out sValue)) break;
                    pGrid[1, i].Value = sValue;
                }
            }
            pGrid.Visible   = true;
        }
        //------------------------------------------------------------------------
        public bool UpdateListVal(int no, out string sName, out string sValue)
        {
            int  iCnt = 0 ;
            sName     = "";
            sValue    = "";
            object obj = new object(); 
           
            if(no == iCnt++) { sName = "CordPviX"     ; obj = m_CordPvi.X	  ; }
            if(no == iCnt++) { sName = "CordPviY"     ; obj = m_CordPvi.Y	  ; }
            if(no == iCnt++) { sName = "Stat"         ; obj = m_iStat         ; }
			if(no == iCnt++) { sName = "Data Stat"    ; obj = m_iStatData     ; }
			if(no == iCnt++) { sName = "Scan Stat"    ; obj = m_iStatScan     ; }

			for (int n = 0; n < (int)EN_RSLT_KIND.EndOfId; n++) 
            {
				 if(no == iCnt++) { sName = string.Format("iRslt_V{0:D2}", n) ; obj = m_iRslt[n]; }
			}

            if(no == iCnt++) { sName = "PviBin(ASCII) "; obj = FNC.ConvertIntToAscIIStr(m_iPviBin); }
			if(no == iCnt++) { sName = "Bin(string)"   ; obj = m_sBin           ; }	
			if(no == iCnt++) { sName = "Layer"         ; obj = m_iLayer         ; }
            if(no == iCnt++) { sName = "BinWhre"       ; obj = Enum.GetName(typeof(EN_TH_AREA), EN_TH_AREA.UT + m_iBinWhre); }											    

            if(no == iCnt++) { sName = "Aligned"       ; obj = m_bAligned       ; }
            if(no == iCnt++) { sName = "bPreAligned"   ; obj = m_bPreAligned    ; }
            if(no == iCnt++) { sName = "bScaned    "   ; obj = m_bScaned        ; }
                                                       
            if(no == iCnt++) { sName = "PosnCrntAlgnX" ; obj = m_PosnCrntAlgn.dX; }
            if(no == iCnt++) { sName = "PosnCrntAlgnY" ; obj = m_PosnCrntAlgn.dY; }
            if(no == iCnt++) { sName = "PosnCrntAlgnT" ; obj = m_PosnCrntAlgn.dT; } 
                                                       
            if(no == iCnt++) { sName = "PosnPreAlgnX " ; obj = m_PosnPreAlgn .dX; }
            if(no == iCnt++) { sName = "PosnPreAlgnY " ; obj = m_PosnPreAlgn .dY; }
            if(no == iCnt++) { sName = "PosnPreAlgnT " ; obj = m_PosnPreAlgn .dT; } 
												      					    
            if(no == iCnt++) { sName = "PosnScanX    " ; obj = m_PosnScan    .dX; }
            if(no == iCnt++) { sName = "PosnScanY    " ; obj = m_PosnScan    .dY; }
            if(no == iCnt++) { sName = "PosnScanT    " ; obj = m_PosnScan    .dT; } 
	

			for (int n = 0; n < (int)EN_CAM.EndofCam; n++) 
            {
				 if(no == iCnt++) { sName = string.Format("Visn #{0:D2} Match", n) ; obj = VisnRslt[n].Match; }
				 if(no == iCnt++) { sName = string.Format("Visn #{0:D2} Score", n) ; obj = VisnRslt[n].Score; }
				 if(no == iCnt++) { sName = string.Format("Visn #{0:D2} dX"   , n) ; obj = VisnRslt[n].X	 ; }
				 if(no == iCnt++) { sName = string.Format("Visn #{0:D2} dY"   , n) ; obj = VisnRslt[n].Y	 ; }
				 if(no == iCnt++) { sName = string.Format("Visn #{0:D2} T "   , n) ; obj = VisnRslt[n].T	 ; }
			}

            if(sName == "") return false;
            
            sName.Trim();
            sValue = obj.ToString(); 
            return true;
        }


        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(BinaryReader br)
        {//UserSet - CHIP Load (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
                                           
            m_sBin           = br.ReadString().Trim();
			//m_byBin          = br.ReadBytes  (vDEF.MAX_BIN_LENGTH);
			m_bAligned       = br.ReadBoolean();
            m_bPreAligned    = br.ReadBoolean();
			m_bScaned        = br.ReadBoolean();
                             
            m_bSpare1        = br.ReadBoolean();
            m_bSpare2        = br.ReadBoolean();
                             
            m_iStat          = (EN_CHIP_STAT)br.ReadInt32();
            m_iStatData      = (EN_CHIP_STAT)br.ReadInt32();
            m_iStatScan  	 = (EN_CHIP_STAT)br.ReadInt32();

            for(int i=0;i<(int)EN_RSLT_KIND.EndOfId;i++) 
            {
                m_iRslt[i]  = (EN_CHIP_RSLT)br.ReadInt32();
			}
			for(int i=0;i<(int)EN_CAM.EndofCam;i++) 
			{
				VisnRslt[i].CamId = (EN_CAM)br.ReadInt32  ();
                VisnRslt[i].Match = br.ReadBoolean();
                VisnRslt[i].X     = br.ReadDouble ();
                VisnRslt[i].Y     = br.ReadDouble ();
                VisnRslt[i].T     = br.ReadDouble ();
                VisnRslt[i].Score = br.ReadDouble ();
            }
			m_iLayer		       = br.ReadInt32();
            m_iBinWhre             = br.ReadInt32();
            m_CordPvi.X            = br.ReadInt32();
            m_CordPvi.Y            = br.ReadInt32();
			m_iPviBin              = br.ReadInt32();
                                   
            m_iSpare1              = br.ReadInt32();
            m_iSpare2              = br.ReadInt32();
                                   
            m_PosnCrntAlgn.dX      = br.ReadDouble();
            m_PosnCrntAlgn.dY      = br.ReadDouble();
            m_PosnCrntAlgn.dT      = br.ReadDouble();
            m_PosnPreAlgn .dX      = br.ReadDouble();
            m_PosnPreAlgn .dY      = br.ReadDouble();
            m_PosnPreAlgn .dT      = br.ReadDouble();
            m_PosnScan    .dX      = br.ReadDouble();
            m_PosnScan    .dY      = br.ReadDouble();
            m_PosnScan    .dT      = br.ReadDouble();
             
            m_dSpare1              = br.ReadDouble();
            m_dSpare2              = br.ReadDouble();

        }
        //------------------------------------------------------------------------
        public void Save(BinaryWriter wr)
        {//UserSet - CHIP Save (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
                                  
			wr.Write(m_sBin .PadRight(vDEF.MAX_BIN_LENGTH, ' '));
			//wr.Write(m_byBin         );
            wr.Write(m_bAligned      );                                                                          
            wr.Write(m_bPreAligned   );
			wr.Write(m_bScaned       );                                        
                                                                               
            wr.Write(m_bSpare1       );                                        
            wr.Write(m_bSpare2       );                                        
                                                                               
            wr.Write((int)m_iStat    );  
            wr.Write((int)m_iStatData);
            wr.Write((int)m_iStatScan);    
                                                                               
            for(int i=0;i<(int)EN_RSLT_KIND.EndOfId;i++) 
            {
                wr.Write((int)m_iRslt[i]   );
			}
            for(int i=0;i<(int)EN_CAM.EndofCam;i++) 
            {
				wr.Write((int)VisnRslt[i].CamId);
                wr.Write(VisnRslt[i].Match);
                wr.Write(VisnRslt[i].X    );
                wr.Write(VisnRslt[i].Y    );
                wr.Write(VisnRslt[i].T    );
                wr.Write(VisnRslt[i].Score);
            }

			wr.Write(m_iLayer         );
            wr.Write(m_iBinWhre       );                                          
                                           
            wr.Write(m_CordPvi.X      );                                          
            wr.Write(m_CordPvi.Y      ); 
			wr.Write(m_iPviBin        ); 
                                            
            wr.Write(m_iSpare1        );                                          
            wr.Write(m_iSpare2        );                                        

            wr.Write(m_PosnCrntAlgn.dX);                                        
            wr.Write(m_PosnCrntAlgn.dY);                                        
            wr.Write(m_PosnCrntAlgn.dT);                                                                               
            wr.Write(m_PosnPreAlgn .dX);                                        
            wr.Write(m_PosnPreAlgn .dY);                                        
            wr.Write(m_PosnPreAlgn .dT);    
            wr.Write(m_PosnScan    .dX);                                        
            wr.Write(m_PosnScan    .dY);                                        
            wr.Write(m_PosnScan    .dT);                                       
                                                                               
            wr.Write(m_dSpare1        );                                          
            wr.Write(m_dSpare2        );                                          
        }                                                                      
	}
}
