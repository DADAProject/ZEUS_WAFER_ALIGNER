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
    /***************************************************************************/
    /* Class: TTray                                                            */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TTray
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */	
		EN_TRAY_ID    m_ID        ;


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int         m_iMaxRow     ;
        int         m_iMaxCol     ;
        EN_MAP_DIR  m_iDispDir    ;


		//Spare Var.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        string  m_sSpare1  ;
        string  m_sSpare2  ;
        string  m_sSpare3  ;
        string  m_sSpare4  ;
        string  m_sSpare5  ;
        bool    m_bSpare1, m_bSpare2, m_bSpare3, m_bSpare4, m_bSpare5, m_bSpare6, m_bSpare7, m_bSpare8, m_bSpare9, m_bSpare10;
        int     m_iSpare1, m_iSpare2, m_iSpare3, m_iSpare4, m_iSpare5, m_iSpare6, m_iSpare7, m_iSpare8, m_iSpare9, m_iSpare10;
        double  m_dSpare1, m_dSpare2, m_dSpare3, m_dSpare4, m_dSpare5, m_dSpare6, m_dSpare7, m_dSpare8, m_dSpare9, m_dSpare10;

        
        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TChip this[int R, int C]
        {
            get 
			{ 
                if (R < 0 || R >= vDEF.MAX_TRAY_R) return null; 
                if (C < 0 || C >= vDEF.MAX_TRAY_C) return null; 
                return CHPS[R,C] as TChip; 
            }
            set 
			{
                if (R < 0 || R >= vDEF.MAX_TRAY_R) return; 
                if (C < 0 || C >= vDEF.MAX_TRAY_C) return; 
                CHPS[R,C] = value.Copy(); 
            }

        }

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_TRAY_ID _ID       { get { return m_ID      ;  }                             }
        public int	      _iMaxRow  { get { return m_iMaxRow ;  } set { m_iMaxRow  = value; } }
        public int	      _iMaxCol  { get { return m_iMaxCol ;  } set { m_iMaxCol  = value; } }   

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TChip[,]      CHPS   = new TChip [vDEF.MAX_TRAY_R,vDEF.MAX_TRAY_C];
        public TUnit  UNIT   = new TUnit ();


		//Method
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//생성자 & 소멸자. (Constructor & Destructor)
		public TTray()
		{
            m_sSpare1     = "";
            m_sSpare2     = "";
            m_sSpare3     = "";
            m_sSpare4     = "";
            m_sSpare5     = "";

            m_ID = 0;
            for (int r=0; r<vDEF.MAX_TRAY_R; r++) 
			{
                for (int c=0; c<vDEF.MAX_TRAY_C; c++) 
				{
                    CHPS[r,c] = new TChip (); 
                }
            }
            Init();
		}
		public TTray(EN_TRAY_ID ID)
        {
            m_sSpare1     = "";
            m_sSpare2     = "";
            m_sSpare3     = "";
            m_sSpare4     = "";
            m_sSpare5     = "";

            m_ID = ID;
            for (int r=0; r<vDEF.MAX_TRAY_R; r++) 
			{
                for (int c=0; c<vDEF.MAX_TRAY_C; c++) 
				{
                    CHPS[r,c] = new TChip (); 
                }
            }
            Init();
        }
        ~TTray() { }

        //public void Copy(TTray Obj)
        //{
        //    for (int r=0; r<vDEF.MAX_TRAY_R; r++) {
        //        for (int c=0; c<vDEF.MAX_TRAY_C; c++) {
        //            CHPS[r, c] = Obj.CHPS[r, c].Copy();
        //        }
        //    }
        //    this.UNIT = Obj.UNIT.Copy();
        //}
        public TTray Copy()
        {
			return FNC.DeepClone(this) as TTray;
            //return this.MemberwiseClone() as TChip;
        }

        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TChip gCHIP(int R, int C) { return CHPS[R, C].Copy(); }


        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void sCHIP(int R, int C, TChip pChip) { CHPS[R, C] = pChip.Copy(); }
        

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init()
        {
            m_iMaxRow = vDEF.MAX_TRAY_R;
            m_iMaxCol = vDEF.MAX_TRAY_C;

            m_iDispDir  = EN_MAP_DIR.Deg0_HMir;
			//
            SetTo(EN_CHIP_STAT.None , EN_CHIP_RSLT.None,  EN_RSLT_KIND.All);
            SetUnitTo(EN_UNIT_STAT.None); 
        }

        //Clear.
        public void  ClearMap()
        {
            for (int i = 0 ; i < vDEF.MAX_TRAY_R ; i++) 
            {
                for (int j = 0; j < vDEF.MAX_TRAY_C; j++)
                {
                    CHPS[i,j].Init();
                }
            }
			//
			SetTo(EN_CHIP_STAT.None, EN_CHIP_RSLT.None, EN_RSLT_KIND.All);
        }

        //Set Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  SetDispDir(EN_MAP_DIR iDir)
        {
            m_iDispDir = iDir;
        }
        public void  SetTo (int r , int c , EN_CHIP_STAT Stat , EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo)
        {
            if (r < 0 || r >= m_iMaxRow) return;
            if (c < 0 || c >= m_iMaxCol) return;

            CHPS[r, c].SetTo(Stat, Rslt, RsltNo);

        }
		public void  SetToCol       (int r         , EN_CHIP_STAT Stat , EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo)
        {
            for (int c = 0; c < m_iMaxCol; c++) CHPS[r, c].SetTo(Stat, Rslt, RsltNo);
        }
		public void  SetToRow       (int c         , EN_CHIP_STAT Stat , EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo)
        {
            for (int r = 0; r < m_iMaxRow; r++) CHPS[r, c].SetTo(Stat, Rslt, RsltNo);
        }
        public void  SetTo       (                EN_CHIP_STAT Stat , EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo)
        {
             for (int r = 0 ; r < m_iMaxRow ; r++) {
                 for (int c = 0 ; c < m_iMaxCol ; c++)
                    CHPS[r,c].SetTo(Stat, Rslt, RsltNo);
                 }
        }

        public void SetUnitTo(EN_UNIT_STAT Stat)
        {
             UNIT.SetTo(Stat);
        }

        //Get Chip Status.
		public EN_CHIP_STAT   GetChipStat (int r , int c) 
        { 

            if(CHPS[r,c] == null) return EN_CHIP_STAT.None;

            return CHPS[r,c]._iStat     ; 
        }

		public EN_UNIT_STAT  GetUnitStat () 
        { 
			if (UNIT._iStat >= EN_UNIT_STAT.EndOfId) return EN_UNIT_STAT.None;
			if (UNIT._iStat < 0                    ) return EN_UNIT_STAT.None;

            return UNIT._iStat     ; 
        }
        //------------------------------------------------------------------------------
        public bool  IsOneExist     (int r) 
        {  
            for (int c=0; c<m_iMaxCol; c++) 
            {
                if( CHPS[r,c].IsExist()) return true ; 
            } 
            return false;
        }
        public bool  IsOneStat      (int r , EN_CHIP_STAT Stat) 
        {  
            for (int c=0; c<m_iMaxCol; c++) {
                if( CHPS[r,c].IsStat (Stat)) return true ; 
            } 
            return false;
        }
        public bool  IsOneRslt      (int r , EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        {  
            for (int c=0; c<m_iMaxCol; c++) 
			{
                if( CHPS[r,c].IsRslt (Rslt, RsltNo)) return true ; 
            } 
            return false;
        }

        public bool  IsOneExist     () 
        {  
            for (int r=0; r<m_iMaxRow; r++) 
            {
                if( IsOneExist(r)) return true ; 
            } 
            return false;
        }
        public bool  IsOneStat      (EN_CHIP_STAT Stat) 
        {  
            for (int r=0; r<m_iMaxRow; r++) {
                if( IsOneStat (r,Stat)) return true ; 
            } 
            return false;
        }
        public bool  IsOneRslt      (EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        {  
            for (int r=0; r<m_iMaxRow; r++) 
			{
                if( IsOneRslt (r,Rslt, RsltNo)) return true ; 
            } 
            return false;
        }

        //Check Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool  IsExist     (int r , int c          ) 
		{ 
            if(CHPS[r,c] == null) return false;
            return (CHPS[r,c].IsExist()); 
        }
        public bool  IsStat      (int r , int c, EN_CHIP_STAT Stat) 
        { 
            if(CHPS[r,c] == null) return false;
            return (CHPS[r,c].IsStat      (Stat)); 
        }
        public bool  IsUnitStat      (EN_UNIT_STAT Stat) 
        { 
            return UNIT.IsStat(Stat); 
        }
        public bool  IsRslt      (int r , int c, EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        { 
            if(CHPS[r,c] == null) return false; 
            return (CHPS[r,c].IsRslt (Rslt, RsltNo)); 
        }


        //Check All Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool  IsUnitExist     () 
        { 
            return UNIT.IsExist();
        }

		public bool  IsAllExist     () 
        { 
            for (int r = 0 ; r < m_iMaxRow ; r++) { 
                for (int c = 0 ; c < m_iMaxCol ; c++) { 
                    if (!CHPS[r,c].IsExist ()) return false; 
                } 
            } 
            return true; 
        }
		public bool  IsAllStat      (EN_CHIP_STAT Stat) 
        { 
            for (int r = 0 ; r < m_iMaxRow ; r++) { 
                for (int c = 0 ; c < m_iMaxCol ; c++) { 
                    if (!CHPS[r,c].IsStat  (Stat    )) return false; 
                } 
            } 
            return true; 
        }


		//>>> UserSet
        public bool  IsAllRslt      (EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo) 
        { 
            for (int r = 0 ; r < m_iMaxRow ; r++) { 
                for (int c = 0 ; c < m_iMaxCol ; c++) { 
                    if (!CHPS[r,c].IsRslt  (Rslt, RsltNo)) return false; 
                } 
            } return true; 
        }
        public bool  IsAllFail      () 
        { 
            for (int r = 0 ; r < m_iMaxRow ; r++) { 
                for (int c = 0 ; c < m_iMaxCol ; c++) { 
                    if (!CHPS[r,c].IsFail()) return false; 
                } 
            } return true; 
        }
        public bool  IsAllGood      () 
        { 
            for (int r = 0 ; r < m_iMaxRow ; r++) { 
                for (int c = 0 ; c < m_iMaxCol ; c++) { 
                    if (!CHPS[r,c].IsGood()) return false; 
                } 
            } return true; 
        }

        //Check ROW Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool  IsRowExist     (int r                   ) 
        { 
            for (int c = 0 ; c < m_iMaxCol ; c++) { 
                if(CHPS[r,c] == null) return false; 
                if (!CHPS[r,c].IsExist    (        )) return false; 
            }  
            return true; 
        }
        public bool  IsRowStat      (int r, EN_CHIP_STAT Stat) 
        { 
            for (int c = 0 ; c < m_iMaxCol ; c++) { 
                if(CHPS[r,c] == null) return false; 
                if (!CHPS[r,c].IsStat     (Stat    )) return false; }
            return true; 
        }
        public bool  IsRowRslt      (int r, EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        { 
            for (int c = 0 ; c < m_iMaxCol ; c++) { 
                if(CHPS[r,c] == null) return false; 
                if (!CHPS[r,c].IsRslt     (Rslt, RsltNo)) return false; 
            }  
            return true; 
        }

        //Get Row Count by ChipStatus.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int   GetCntExist    (int r) 
        { 
            int iCnt = 0; 
            for (int c = 0 ; c < m_iMaxCol ; c++) { 
                if(CHPS[r,c] == null) continue; 
                if (CHPS[r,c].IsExist (        )) iCnt++; 
            }
            return iCnt; 
        }
        public int   GetCntStat     (int r, EN_CHIP_STAT Stat) 
        { 
            int iCnt = 0; 
            for (int c = 0 ; c < m_iMaxCol ; c++) {
                if(CHPS[r,c] == null) continue; 
                if (CHPS[r,c].IsStat  (Stat    )) iCnt++; 
                }
                return iCnt; 
        }
        public int   GetCntStat     (int r, EN_FIND FindMode) 
        { 
            int iCnt = 0; 
            for (int c = 0 ; c < m_iMaxCol ; c++) {
                if(CHPS[r,c] == null) continue; 
                if (CHPS[r,c].FindChip(FindMode)) iCnt++; 
                }
                return iCnt; 
        }
        public int   GetCntRslt     (int r, EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        { 
           int iCnt = 0; 
           for (int c = 0 ; c < m_iMaxCol ; c++) {
               if(CHPS[r,c] == null) continue; 
               if (CHPS[r,c].IsRslt  (Rslt, RsltNo)) iCnt++; 
           }
           return iCnt; 
        }

        //Get All Count by ChipStatus.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int   GetCntExist    () 
        { 
            int iCnt = 0; 
            for (int i = 0 ; i < m_iMaxRow ; i++) iCnt = iCnt + GetCntExist(i); 
            return iCnt; 
        }
        public int   GetCntStat     (EN_CHIP_STAT Stat) 
        { 
            int iCnt = 0; 
            for (int i = 0 ; i < m_iMaxRow ; i++) 
                iCnt = iCnt + GetCntStat     (i, Stat    ); 
            return iCnt; 
        }
        public int   GetCntStat     (EN_FIND FindMode) 
        { 
            int iCnt = 0; 
            for (int i = 0 ; i < m_iMaxRow ; i++) 
                iCnt = iCnt + GetCntStat     (i, FindMode); 
            return iCnt; 
        }
        public int   GetCntRslt     (EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo ) 
        { 
            int iCnt = 0; 
            for (int i = 0 ; i < m_iMaxRow ; i++) iCnt = iCnt + GetCntRslt(i, Rslt, RsltNo); 
            return iCnt; 
        }
		public int GetCntBin (EN_TH_AREA WherBtmTH)
		{
           int        iCnt = 0; 
		   string     sBin ="";
		   bool       isRslt  ;
		   int        iWherBin;
           int        WherBtm = WherBtmTH - EN_TH_AREA.UT;

            for (int r = 0 ; r < m_iMaxRow ; r++) { 
                for (int c = 0 ; c < m_iMaxCol ; c++) { 
					 if(CHPS[r, c] == null) continue; 
					 sBin     = CHPS[r, c]._sBin;
					 iWherBin = cDEF.FM.GetBinWhre(sBin);
					 isRslt   = CHPS[r, c].IsStat(EN_CHIP_STAT.Rslt ) || CHPS[r, c].IsStat(EN_CHIP_STAT.GScan);
					 			   
	  				 if ((iWherBin < 0) || (iWherBin < (int)vDEF.MAX_WORK_BIN_NO)) continue;	  		   
	  				 //
	  				 if (isRslt && (WherBtm == iWherBin)) iCnt++;
					 }
				}
		   //
           return iCnt;			
		}
        //Get Count by FindMode.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool  IsFindRcvBin          (EN_FIND FindMode)
        {
            //if (FindMode == EN_FIND.InsGood) return true;
            return false;
        }

		//Search Chip.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool FindChip(EN_FIND FindMode)
        {
            for (int i = 0 ; i < m_iMaxRow ; i++) {
                for (int j = 0 ; j < m_iMaxCol ; j++) {
                    if (FindChip(FindMode , i , j)) return true;
                    }
                }
            return false;
        }
        public bool FindChip(EN_FIND FindMode, int r, int c)
        {
            if ((r < 0) || (r >= m_iMaxRow)) return false;
            if ((c < 0) || (c >= m_iMaxCol)) return false;

            return CHPS[r, c].FindChip(FindMode);    
        }
        public int FindFrstRow(EN_FIND FindMode)
        {
            //Local Var.
            int iRowNum = 0;
            int iColNum = 0;

            //Find First Row and Col.
            FindFrstRowCol(FindMode , out iRowNum , out iColNum);

            return iRowNum;
        }
        public int FindFrstCol(EN_FIND FindMode)
        {
            //Local Var.
            int iRowNum = 0;
            int iColNum = 0;

            //Find First Row and Col.
            FindFrstRowCol(FindMode, out iRowNum, out iColNum);

            return iColNum;
        }
        public bool  FindFrstRowCol        (EN_FIND        FindMode , out int R , out int C )
        {
            //Find First Row and Col.
            for (int i = 0 ; i < m_iMaxRow ; i++) {
                for (int j = 0; j < m_iMaxCol; j++)
                {
                    if (FindChip(FindMode , i , j)) {
                        R = i;
                        C = j;
                        return true;
                        }
                    }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindLastRowCol        (EN_FIND        FindMode , out int R , out int C )
        {
            for (int i = m_iMaxRow- 1 ; i >= 0 ; i--) {
                for (int j = m_iMaxCol - 1; j >= 0; j--)
                {
                    if (FindChip(FindMode , i , j)) {
                        R = i;
                        C = j;
                        return true;
                        }
                    }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindFrstRowLastCol    (EN_FIND        FindMode , out int R , out int C )
        {
            for (int i = 0 ; i < m_iMaxRow; i++) {
                for (int j = m_iMaxCol - 1 ; j >= 0 ; j--) {
                    if (FindChip(FindMode , i , j)) {
                        R = i;
                        C = j;
                        return true;
                        }
                    }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindLastRowFrstCol    (EN_FIND        FindMode , out int R , out int C )
        {
            for (int i = m_iMaxRow- 1 ; i >= 0 ; i--) {
                for (int j = 0 ; j < m_iMaxCol ; j++) {
                    if (FindChip(FindMode , i , j)) {
                        R = i;
                        C = j;
                        return true;
                        }
                    }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;            
        }
		public bool  FindFrstColLastRow    (EN_FIND        FindMode , out int R , out int C )
        {
            //Find First Row and Last Col.
            for (int j = 0 ; j < m_iMaxCol ; j++) {
                for (int i = m_iMaxRow- 1 ; i >= 0 ; i--) {
                    if (FindChip(FindMode , i , j)) {
                        R = i;
                        C = j;
                        return true;
                        }
                    }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindLastColFrstRow    (EN_FIND        FindMode , out int R , out int C )
        {
            //Find Last Row and First Col.
            for (int j = m_iMaxCol - 1 ; j >= 0 ; j--) {
                for (int i = 0 ; i < m_iMaxRow; i++) {
                    if (FindChip(FindMode , i , j)) {
                        R = i;
                        C = j;
                        return true;
                        }
                    }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindFrstColRow        (EN_FIND        FindMode , out int R , out int C )
        {
            //Find First Row and Col.
            for (int j = 0 ; j < m_iMaxCol ; j++) {
                for (int i = 0 ; i < m_iMaxRow; i++) {
                    if (FindChip(FindMode , i , j)) {
                        R = i;
                        C = j;
                        return true;
                        }
                    }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }

		//Update Chip Status.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void GetImageRC(ref System.Windows.Forms.PictureBox pb,  int X, int Y, out int R, out int C)
        {
            int   uR , uC ;
            int   iR, iC  ;

            //
			uR = m_iMaxRow;
			uC = m_iMaxCol;
            //
            double uGw   = Math.Round((double)pb.Width  / (double)uC, 0);
            double uGh   = Math.Round((double)pb.Height / (double)uR, 0);
            int   Urow  = (int)((double)Y / uGh);
            int   Ucol  = (int)((double)X / uGw);
            //
            switch (m_iDispDir) 
            {
                default                    : iR = Urow;            iC = Ucol;            break;
                case EN_MAP_DIR.Deg0       : iR = Urow;            iC = Ucol;            break; // 0
                case EN_MAP_DIR.Deg90      : iR = Ucol;            iC = (uR - 1) - Urow; break; // 90
                case EN_MAP_DIR.Deg180     : iR = (uR - 1) - Urow; iC = (uC - 1) - Ucol; break; // 180
                case EN_MAP_DIR.Deg270     : iR = (uC - 1) - Ucol; iC = Urow;            break; // 270
                case EN_MAP_DIR.Deg270_VMir: iR = (uC - 1) - Ucol; iC = (uR - 1) - Urow; break; // 270 + Vert. Mirror.           
                case EN_MAP_DIR.Deg0_HMir  : iR = Urow           ; iC = (uC - 1) - Ucol; break; // Horz. Mirrror.
                case EN_MAP_DIR.Deg180_VMir: iR = (uR - 1) - Urow; iC = Ucol;            break; // 180 + Vert. Mirror.              
            }
            //
            R = iR;
            C = iC;
        }
		public void  GetDispRC             (int r, int c, out int iRow, out int iCol)
        {
            int    iMaxRow = m_iMaxRow;
            int    iMaxCol = m_iMaxCol;

            iRow = 0;
            iCol = 0;

            switch (m_iDispDir) {
				default: return;
                case EN_MAP_DIR.Deg0       : iRow = r; iCol = c;                                 break; // 0
                case EN_MAP_DIR.Deg90      : iRow = c; iCol = (iMaxRow - 1) - r;                 break; // 90
                case EN_MAP_DIR.Deg180     : iRow = (iMaxRow - 1) - r; iCol = (iMaxCol - 1) - c; break; // 180
                case EN_MAP_DIR.Deg270     : iRow = (iMaxCol - 1) - c; iCol = r;                 break; // 270
                case EN_MAP_DIR.Deg270_VMir: iRow = (iMaxCol - 1) - c; iCol = (iMaxRow - 1) - r; break; // 270 + Vert. Mirror.
                case EN_MAP_DIR.Deg0_HMir  : iRow = r; iCol = (iMaxCol - 1) - c;                 break; // Horz. Mirrror.
                case EN_MAP_DIR.Deg180_VMir: iRow = (iMaxRow - 1) - r; iCol = c;                 break; // 180 + Vert. Mirror.
                }
        }
		public void UpdateChip(ref System.Windows.Forms.PictureBox pb)
		{
            //
            if (pb == null) return;
            Bitmap bmp = new Bitmap(pb.Width, pb.Height);
            Graphics g = Graphics.FromImage(bmp); 
            Brush brush;
            string sTemp;

            //
			Color sBColor   = Color.White;
            Color sPColor   = Color.Black;  
            int   iMaxX = m_iMaxCol; 
            int   iMaxY = m_iMaxRow; 
            int   iMinX = 0; 
            int   iMinY = 0;     
            int   iR, iC  ; 

            //
            float   uX1, uX2, uY1, uY2;
            float   uGw   = (float)pb.Width  / (float)iMaxX;
            float   uGh   = (float)pb.Height / (float)iMaxY;
            float   iWOff = ((float)pb.Width  - (uGw * (float)iMaxX)) / 2.0f;
            float   iHOff = ((float)pb.Height - (uGh * (float)iMaxY)) / 2.0f;	

            //
            FNC.ClearPictureBox(ref pb, Color.White);

            //
            for (int r = iMinY; r < iMaxY; r++)
            {
                for (int c = iMinX; c < iMaxX; c++)   
                {
                     switch (m_iDispDir) 
                     {
                        default: return;
                         case EN_MAP_DIR.Deg0       : iR = r;               iC = c;               break; // 0
                         case EN_MAP_DIR.Deg90      : iR = c;               iC = (iMaxY - 1) - r; break; // 90
                         case EN_MAP_DIR.Deg180     : iR = (iMaxY - 1) - r; iC = (iMaxX - 1) - c; break; // 180
                         case EN_MAP_DIR.Deg270     : iR = (iMaxX - 1) - c; iC = r;               break; // 270
                         case EN_MAP_DIR.Deg270_VMir: iR = (iMaxX - 1) - c; iC = (iMaxY - 1) - r; break; // 270 + Vert. Mirror.   
						 case EN_MAP_DIR.Deg0_HMir  : iR = r              ; iC = (iMaxX - 1) - c; break; // Horz. Mirrror.
						 case EN_MAP_DIR.Deg180_VMir: iR = (iMaxY - 1) - r; iC = c;               break; // 180 + Vert. Mirror.                         
                     }
                     uX1 = iWOff + c * uGw       + 1;
                     uX2 = iWOff + c * uGw + uGw - 1;
                     uY1 = iHOff + r * uGh       + 1;
                     uY2 = iHOff + r * uGh + uGh - 1;
					if ((iC < iMinX) || (iC >= iMaxX)) continue;
					if ((iR < iMinY) || (iR >= iMaxY)) continue;

					//
                    sBColor = CHPS[iR, iC].GetBinColor (EN_OBJ_KIND.TRAY);
					brush = new SolidBrush(sBColor);
					//
					g.FillRectangle(brush     , uX1, uY1, (uX2-uX1)   , (uY2-uY1)  ); //채우기
					g.DrawRectangle(Pens.Black, uX1, uY1, (uX2-uX1)-1 , (uY2-uY1)-1); //테두리 사각형
                    //                  
                    sTemp = string.Format("{0}",  iR * iMaxX + iC + 1);
                    g.DrawString(sTemp, new Font("Arial" , 5) , Brushes.Black, new Point((int)(uX1 + 2.0), (int)(uY1 + 2.0f)));
                    //
                    if (brush != null) brush.Dispose();
                }
            }
            //
            pb.Image = bmp;
            //
            if (g     != null) g    .Dispose();
		}

        public void UpdateUnit(ref System.Windows.Forms.PictureBox pPBox)
        {
            //Local Var.
			Brush        brush;
            Color        sBColor  = Color.White;
            Color        sPColor  = Color.Black;   
            String       sTemp2   = ""         ;
            EN_UNIT_STAT iStat;      
                       
            Bitmap bmp = new Bitmap(pPBox.Width, pPBox.Height);
            Graphics g = Graphics.FromImage(bmp);

            if (pPBox == null) return;

           //이미지 사이즈
            int iMainW   = pPBox.Size.Width ;
            int iMainH   = pPBox.Size.Height;
 
            //중심을 맞추기 위해서 사용  
            int iCOffW   = 1;
            int iCOffH   = 1;
            
            iStat   = GetUnitStat();
            sBColor = UNIT.GetStatColor();
			brush   = new SolidBrush(sBColor);
            sTemp2  = vDEF.STR_UNIT_STAT[(int)iStat];

			g.FillRectangle(brush     , iCOffW, iCOffH, iMainW-(iCOffW*2)   , iMainH-(iCOffH*2)  );
			g.DrawRectangle(Pens.Black, iCOffW, iCOffH, iMainW-(iCOffW*2)-1 , iMainH-(iCOffH*2)-1);

            //DrawRect   (ref g, iCOffW, iCOffH, iMainW-(iCOffW*2), iMainH-(iCOffH*2), sPColor, sBColor);      
            //DrawText   (ref g, 5 + 1, 5 + 1, Color.Black, sTemp);
			//
            pPBox.Image = bmp;
            if (g     != null) g.Dispose();
        }

     

		//Loading Para.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void  Load(BinaryReader br)
        {//UserSet - Tool 변수 Load (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
            for (int r = 0 ; r < vDEF.MAX_TRAY_R; r++) 
            {
                for (int c = 0 ; c < vDEF.MAX_TRAY_C ; c++) 
                {
                    if(CHPS[r,c] == null) continue;
                    CHPS[r,c].Load(br);
                    }
                }
            UNIT.Load(br);

            m_sSpare1  = br.ReadString().Trim();
            m_sSpare2  = br.ReadString().Trim();
            m_sSpare3  = br.ReadString().Trim();
            m_sSpare4  = br.ReadString().Trim();
            m_sSpare5  = br.ReadString().Trim();
                         
            m_bSpare1 =  br.ReadBoolean();
            m_bSpare2 =  br.ReadBoolean();
            m_bSpare3 =  br.ReadBoolean();
            m_bSpare4 =  br.ReadBoolean();
            m_bSpare5 =  br.ReadBoolean();
            m_bSpare6 =  br.ReadBoolean();
            m_bSpare7 =  br.ReadBoolean();
            m_bSpare8 =  br.ReadBoolean();
            m_bSpare9 =  br.ReadBoolean();
            m_bSpare10 = br.ReadBoolean();

            m_iSpare1 =  br.ReadInt32();
            m_iSpare2 =  br.ReadInt32();
            m_iSpare3 =  br.ReadInt32();
            m_iSpare4 =  br.ReadInt32();
            m_iSpare5 =  br.ReadInt32();
            m_iSpare6 =  br.ReadInt32();
            m_iSpare7 =  br.ReadInt32();
            m_iSpare8 =  br.ReadInt32();
            m_iSpare9 =  br.ReadInt32();
            m_iSpare10 = br.ReadInt32();

            m_dSpare1 =  br.ReadDouble();
            m_dSpare2 =  br.ReadDouble();
            m_dSpare3 =  br.ReadDouble();
            m_dSpare4 =  br.ReadDouble();
            m_dSpare5 =  br.ReadDouble();
            m_dSpare6 =  br.ReadDouble();
            m_dSpare7 =  br.ReadDouble();
            m_dSpare8 =  br.ReadDouble();
            m_dSpare9 =  br.ReadDouble();
            m_dSpare10 = br.ReadDouble();

        }
		public void  Save(BinaryWriter wr)
        {//UserSet - Tool 변수 Save (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
            for (int r = 0 ; r < vDEF.MAX_TRAY_R; r++) 
            {
                for (int c = 0 ; c < vDEF.MAX_TRAY_C ; c++) 
                {
                    if(CHPS[r,c] == null) continue;
                    CHPS[r,c].Save(wr);
                }
            }
            UNIT.Save(wr);

            wr.Write(m_sSpare1 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare2 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare3 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare4 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare5 .PadRight(vDEF.MAX_STR_LEN, ' '));
                               
            wr.Write(m_bSpare1 );
            wr.Write(m_bSpare2 );
            wr.Write(m_bSpare3 );
            wr.Write(m_bSpare4 );
            wr.Write(m_bSpare5 );
            wr.Write(m_bSpare6 );
            wr.Write(m_bSpare7 );
            wr.Write(m_bSpare8 );
            wr.Write(m_bSpare9 );
            wr.Write(m_bSpare10);

            wr.Write(m_iSpare1 );
            wr.Write(m_iSpare2 );
            wr.Write(m_iSpare3 );
            wr.Write(m_iSpare4 );
            wr.Write(m_iSpare5 );
            wr.Write(m_iSpare6 );
            wr.Write(m_iSpare7 );
            wr.Write(m_iSpare8 );
            wr.Write(m_iSpare9 );
            wr.Write(m_iSpare10);

            wr.Write(m_dSpare1 );
            wr.Write(m_dSpare2 );
            wr.Write(m_dSpare3 );
            wr.Write(m_dSpare4 );
            wr.Write(m_dSpare5 );
            wr.Write(m_dSpare6 );
            wr.Write(m_dSpare7 );
            wr.Write(m_dSpare8 );
            wr.Write(m_dSpare9 );
            wr.Write(m_dSpare10);
        }
    }
}

/*
		public void  UpdateChip(ref System.Windows.Forms.PictureBox pPBox   , EN_MAP_DIR iDir = EN_MAP_DIR.None)
        {
            //Local Var.
            Color      sBColor  = Color.White;
            Color      sPColor  = Color.Black;   
            int        iMaxR    = m_iMaxRow  ;
            int        iMaxC    = m_iMaxCol  ;
            int        cLbNo                 ; 
            String     sTemp,sTemp2          ;
            int        iRLbl, iCLbl          ;  
                       
            int iDX , iDY;

            int sRow, sCol, rRow, rCol;

            Bitmap bmp = new Bitmap(pPBox.Width, pPBox.Height);
            Graphics gr = Graphics.FromImage(bmp);

            if (pPBox == null) return;
            if (iDir  != EN_MAP_DIR.None) m_iDispDir   = iDir;

            bool isDisp90 = m_iDispDir == EN_MAP_DIR.Deg90 || m_iDispDir == EN_MAP_DIR.Deg270 || m_iDispDir == EN_MAP_DIR.Deg270_VMir;

            iRLbl = 0;
            iCLbl = 0;

           //이미지 사이즈
            int iMainW   = pPBox.Size.Width ;
            int iMainH   = pPBox.Size.Height;
            int iDispR   = (isDisp90) ?  (iMaxC + iCLbl) : (iMaxR + iRLbl);
            int iDispC   = (isDisp90) ?  (iMaxR + iRLbl) : (iMaxC + iCLbl);
 
            //각 CELL 사이즈 
            int iCellW   = iMainW / iDispC;
            int iCellH   = iMainH / iDispR;

            //중심을 맞추기 위해서 사용  
            int iCOffW   = (iMainW - (iCellW * iDispC)) / 2;
            int iCOffH   = (iMainH - (iCellH * iDispR)) / 2;
            int iLblC = (isDisp90) ?  iMaxR : iMaxC; 
            int iLblR = (isDisp90) ?  iMaxC : iMaxR;

  	        for (int r = 0 ; r < iMaxR ; r++) {
                for (int c = 0 ; c < iMaxC ; c++) {
			        GetDispRC(r, c, out sRow, out sCol);

			        iDX = iCOffW + sCol * iCellW + (iRLbl * iCellW);
			        iDY = iCOffH + sRow * iCellH + (iCLbl * iCellH);

                    sBColor = this[r, c].GetBinColor ();
                    sPColor = this[r, c].GetLineColor();
                    
                    DrawRect(ref gr, iDX, iDY, iCellW, iCellH, sPColor, sBColor);
                    if (r == 0 && c == 0) 
                        DrawRect(ref gr, iDX+1, iDY+1, iCellW-2, iCellH-2, Color.Red);
                    sTemp2 = this[r,c].GetRsltText();
                    DrawText(ref gr, iDX + 1, iDY + 1, Color.Black, sTemp2);
 
		        }
	        }
            pPBox.Image = bmp;
            if (gr     != null) gr    .Dispose();
        }
*/