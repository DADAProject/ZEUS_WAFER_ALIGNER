using System;
using System.IO;
using System.Drawing;

namespace eMachine
{


    //Nozzle ID.  
    //===========================================================================
    public enum EN_NOZL : int
    {
        N01,
        N02,
        N03,
        N04,
        N05,
        N06,
        N07,
        N08,
        N09,
        N10,
        N11,
        N12,
        N13,
        N14,
        N15,
        N16,
    };


    /***************************************************************************/
    /* Class: TTool                                                            */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TTool
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
		EN_TOOL_ID        m_ID          ;


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        //Buffers
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			   int         m_iMaxNozl    ;
			   int         m_iMaxRNozl   ;
			   int         m_iMaxCNozl   ;
			   EN_MAP_DIR  m_iDispDir    ;
			   int         m_iPosHead1   ;  
        public bool[]	   m_bMaskNozl = new bool[vDEF.MAX_NOZL];

			   bool        m_bBlkPP      ; //block Pick/Place Option

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
        public TChip this[int N]
        {
            get 
			{ 
                if (N < 0 || N >= vDEF.MAX_NOZL) return null; 
                return CHPS[N] as TChip; 
            }
            set 
			{
                if (N < 0 || N >= vDEF.MAX_NOZL) return; 
                CHPS[N] = value.Copy(); 
            }
        }

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_TOOL_ID _ID        { get { return m_ID       ;  } set { m_ID        = value; } }
        public int		  _iMaxNozl  { get { return m_iMaxNozl ;  } set { m_iMaxNozl  = value; } }
        public int		  _iMaxRNozl { get { return m_iMaxRNozl;  } set { m_iMaxRNozl = value; } }
        public int		  _iMaxCNozl { get { return m_iMaxCNozl;  } set { m_iMaxCNozl = value; } }
        public int		  _iPosHead1 { get { return m_iPosHead1;  } set { m_iPosHead1 = value; } }
		public bool		  _bBlkPP    { get { return m_bBlkPP   ;  } set { m_bBlkPP    = value; } }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TChip[]        CHPS   = new TChip [vDEF.MAX_NOZL];
        public TUnit   UNIT   = new TUnit ();


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TTool(EN_TOOL_ID ID)
        {
            m_sSpare1     = "";
            m_sSpare2     = "";
            m_sSpare3     = "";
            m_sSpare4     = "";
            m_sSpare5     = "";

            m_ID = ID;
            for (int n=0; n<vDEF.MAX_NOZL; n++) CHPS[n] = new TChip ();

            Init();
        }
        ~TTool() { }

   //     public void Copy(TTool Obj)
   //     {
			//for (int n=0; n<vDEF.MAX_NOZL; n++) CHPS[n] = Obj.CHPS[n].Copy();
   //         this.UNIT = Obj.UNIT.Copy();
   //     }
        public TTool Copy()
        {
			return FNC.DeepClone(this) as TTool;
            //return this.MemberwiseClone() as TChip;
        }

        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TChip gCHIP    (int N       ) { return CHPS[N].Copy();        }
		public int   gNzlNo   (int r, int c) { return (r * m_iMaxCNozl) + c; }
		public int   gRowNzlNo(int Nozl    ) { return Nozl / m_iMaxCNozl;    }
		public int   gColNzlNo(int Nozl    ) { return Nozl % m_iMaxCNozl;    }

        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void sCHIP(int N, TChip pChip) { CHPS[N] = pChip.Copy();      }
        

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init()
        {
			m_bBlkPP    = false;

            m_iMaxNozl  = vDEF.MAX_NOZL  ;
            m_iMaxRNozl = vDEF.MAX_NOZL_R;
            m_iMaxCNozl = vDEF.MAX_NOZL_C;
            m_iDispDir  = EN_MAP_DIR.Deg0;
			m_iPosHead1 = 0;
			//
			SetTo(EN_CHIP_STAT.Empty, EN_CHIP_RSLT.None, EN_RSLT_KIND.All);
            SetUnitTo(EN_UNIT_STAT.None); 

        }

        //Clear.
        public void  ClearMap()
        {
            for (int n = 0 ; n < vDEF.MAX_NOZL ; n++) CHPS[n].Init();//gCHIP(n).Init();
			//
			SetTo(EN_CHIP_STAT.Empty, EN_CHIP_RSLT.None, EN_RSLT_KIND.All);
        }


        //Set Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  SetDispDir(EN_MAP_DIR iDir)
        {
            m_iDispDir = iDir;
        }
        public void  SetTo (int n , EN_CHIP_STAT Stat , EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo)
        {
            if (n < 0 || n >= m_iMaxNozl) return;

            CHPS[n].SetTo(Stat, Rslt, RsltNo);

        }
		public void  SetToRow(int r, EN_CHIP_STAT Stat , EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo)
        {
			if (r < 0 || r >= m_iMaxRNozl) return;
			
			int iNzl;
            for (int c = 0; c < m_iMaxCNozl; c++) 
			{
				iNzl = (r * m_iMaxCNozl) + c;
				 CHPS[iNzl].SetTo(Stat, Rslt, RsltNo);
			}
        }
        public void  SetTo       (EN_CHIP_STAT Stat , EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo)
        {
             for (int n = 0 ; n < m_iMaxNozl ; n++) CHPS[n].SetTo(Stat, Rslt, RsltNo);
        }

        public void SetUnitTo(EN_UNIT_STAT Stat)
        {
             UNIT.SetTo(Stat);
        }


        //Get Chip Status.
		public EN_CHIP_STAT   GetChipStat (int n) 
        { 

            if(CHPS[n] == null) return EN_CHIP_STAT.None;
            return CHPS[n]._iStat     ; 
        }

		public EN_UNIT_STAT  GetUnitStat () 
        { 
            return UNIT._iStat     ; 
        }
        //------------------------------------------------------------------------------
        public bool IsMaskNozl     (int n)
        {
            if(n<0 || n>=vDEF.MAX_NOZL) return true;

            return m_bMaskNozl[n];

        }
        public bool  IsOneExist     () 
        {  
            for (int c=0; c<m_iMaxNozl; c++) 
            {
                if( CHPS[c].IsExist(    )) return true ; 
            } 
            return false;
        }
        public bool  IsOneStat      (EN_CHIP_STAT Stat) 
        {  
            for (int c=0; c<m_iMaxNozl; c++) {
                if( CHPS[c].IsStat (Stat)) return true ; 
            } 
            return false;
        }
        public bool  IsOneRslt      (EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        {  
            for (int c=0; c<m_iMaxNozl; c++) 
			{
                if( CHPS[c].IsRslt (Rslt, RsltNo)) return true ; 
            } 
            return false;
        }

        //Check Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool  IsExist     (int n          ) 
		{ 
            if(CHPS[n] == null) return false;
            return (CHPS[n].IsExist     (    )); 
        }
        public bool  IsStat      (int n, EN_CHIP_STAT Stat) 
        { 
            if(CHPS[n] == null) return false;
            return (CHPS[n].IsStat      (Stat)); 
        }
        public bool  IsUnitStat      (EN_UNIT_STAT Stat) 
        { 
            return UNIT.IsStat(Stat); 
        }
        public bool  IsRslt      (int n, EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        { 
            if(CHPS[n] == null) return false; 
            return (CHPS[n].IsRslt (Rslt, RsltNo)); 
        }

        //Check All Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool  IsUnitExist     (                 ) 
        { 
            return UNIT.IsExist();
        }

		public bool  IsAllExist     (                 ) 
        { 
            for (int c = 0 ; c < m_iMaxNozl ; c++) { 
                 if (!CHPS[c].IsExist (        )) return false; 
            } 
            return true; 
        }
		public bool  IsAllStat      (EN_CHIP_STAT Stat) 
        { 
            for (int c = 0 ; c < m_iMaxNozl ; c++) { 
                 if (!CHPS[c].IsStat  (Stat    )) return false; 
            } 
            return true; 
        }


		//>>> UserSet
        public bool  IsAllRslt      (EN_CHIP_RSLT Rslt , EN_RSLT_KIND RsltNo) 
        { 
            for (int c = 0 ; c < m_iMaxCNozl ; c++) { 
                if (!CHPS[c].IsRslt  (Rslt, RsltNo)) return false; 
			}
            //
			return true; 
        }

        //Get Row Count by ChipStatus.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int   GetCntExist    (                ) 
        { 
            int iCnt = 0; 
            for (int c = 0 ; c < m_iMaxNozl ; c++) { 
                if(CHPS[c] == null) continue; 
                if (CHPS[c].IsExist (        )) iCnt++; 
            }
            return iCnt; 
        }
        public int   GetCntStat     (EN_CHIP_STAT Stat      ) 
        { 
            int iCnt = 0; 
            for (int c = 0 ; c < m_iMaxNozl ; c++) {
                if(CHPS[c] == null) continue; 
                if (CHPS[c].IsStat  (Stat    )) iCnt++; 
                }
                return iCnt; 
        }
        public int   GetCntStat     (EN_FIND FindMode) 
        { 
            int iCnt = 0; 
            for (int c = 0 ; c < m_iMaxNozl ; c++) {
                if(CHPS[c] == null) continue; 
                if (CHPS[c].FindChip(FindMode)) iCnt++; 
                }
                return iCnt; 
            }
        public int   GetCntRslt     (EN_CHIP_RSLT Rslt, EN_RSLT_KIND RsltNo) 
        { 
           int iCnt = 0; 
           for (int c = 0 ; c < m_iMaxNozl ; c++) {
               if(CHPS[c] == null) continue; 
               if (CHPS[c].IsRslt  (Rslt, RsltNo)) iCnt++; 
           }
           return iCnt; 
        }
		public int GetCntBin (EN_TH_AREA WherBtmTH)
		{
           int        iCnt = 0; 
           bool       isFail  ;

           for (int c = 0 ; c < m_iMaxNozl ; c++)
           {
               if ( CHPS[c] == null  ) continue; 
               if (!CHPS[c].IsExist()) continue;
			   //if (!cDEF.FM.GetBinWhre(WherBtmTH, CHPS[c]._sBin)) continue;
               if (CHPS[c]._iBinWhre != WherBtmTH - EN_TH_AREA.UT) continue;
               isFail   = CHPS[c].IsFail(); 
	  		   //
               if (WherBtmTH == EN_TH_AREA.NT) {              iCnt++; }
               else                            { if (!isFail) iCnt++; }
           }
           return iCnt;			
		}
		//public int GetCntBin (EN_TH_AREA WherBtmTH)
		//{
  //         int        iCnt = 0; 
		//   string     sBin ="";
		//   bool       isExist ;
  //         bool       isFail  ;
		//   int        iWherBin;
  //         int        iWherBtm = WherBtmTH - EN_TH_AREA.UT;

  //         for (int c = 0 ; c < m_iMaxNozl ; c++) {
  //             if (CHPS[c] == null) continue; 
  //             if (!IsExist(c)    ) continue;
		//	   sBin     = CHPS[c]._sBin;
		//	   iWherBin = cDEF.FM.GetBinWhre(sBin);
  //             isFail   = CHPS[c].IsFail();
  //             //
  //             if ((iWherBin < 0) || (iWherBin >= (int)vDEF.MAX_WORK_BIN_NO)) continue;	   
	 // 		   //
  //             if (iWherBtm == iWherBin)
  //             {
  //                 if (WherBtmTH == EN_TH_AREA.NT) {              iCnt++; }
  //                 else                            { if (!isFail) iCnt++; }
  //             }
  //         }
  //         return iCnt;			
		//}

        //Get Count by FindMode.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool  IsFindRcvBin          (EN_FIND FindMode)
        {
            //if (FindMode == EN_FIND.InsGood) return true;
            return false;
        }
		public bool  IsAllExistWithMask    ()
        {
		    for ( int c = 0 ; c < m_iMaxCNozl ; c++) {
			    if (!CHPS[c].IsExist () && !m_bMaskNozl[c]) return false;
			    }
			
	        return true;
        }
        bool IsRowExistWithMask     (int iRow)
        {
	        if(iRow<0 || iRow>=m_iMaxRNozl) return false;

			int iNzl;
	        for (int c = 0 ; c < m_iMaxCNozl ; c++) {
				 iNzl = (iRow * m_iMaxCNozl) + c;
		         if (!CHPS[iNzl].IsExist () && !m_bMaskNozl[iNzl]) return false;
		         }
	        return true;
        }
        bool IsRowMask     (int iRow)
        {
	        if(iRow<0 || iRow>=m_iMaxRNozl) return false;
	
			int iNzl;
	        for (int c = 0 ; c < m_iMaxCNozl ; c++) {
				 iNzl = (iRow * m_iMaxCNozl) + c;
		         if (!m_bMaskNozl[iNzl]) return false;
		         }
	        return true;
        }

		//Search Chip.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool FindRslt(EN_RSLT_KIND RsltKind, EN_CHIP_STAT Stat, EN_CHIP_RSLT Rslt)
		{
			for (int n = 0; n < m_iMaxNozl; n++)
			{
				 if (CHPS[n].IsStat(Stat) && CHPS[n].gVRslt(RsltKind) == (int)Rslt) return true;
			}
			//
			return false;
		}
		public bool FindRslt(EN_RSLT_KIND RsltKind, EN_CHIP_RSLT Rslt)
		{
			for (int n = 0; n < m_iMaxNozl; n++)
			{
				 if (FindChip(EN_FIND.Rslt, n) && (CHPS[n].gVRslt(RsltKind) == (int)Rslt)) return true;
			}
			//
			return false;
		}
        public bool FindChip(EN_FIND FindMode)
        {
            for (int i = 0 ; i < m_iMaxNozl ; i++) {
                 if (FindChip(FindMode , i)) return true;
                }
            return false;
        }
        public bool FindChip(EN_FIND FindMode, int n)
        {
            if ((n < 0) || (n >= m_iMaxNozl)) return false;

            return CHPS[n].FindChip(FindMode);    
        }
        public bool FindChipRow(EN_FIND FindMode, int R)
        {
			int iNozl;
            if ((R < 0) || (R >= m_iMaxRNozl)) return false;

			for (int c = 0; c < m_iMaxCNozl; c++)
			{
				 iNozl = gNzlNo(R, c);
				 if (CHPS[iNozl].FindChip(FindMode)) return true;
			}
			//
			return false;
        }		
        public bool FindGood()
        {
			int iNozl;
		
			for (int r = 0 ; r < m_iMaxRNozl; r++) 
			{
				for (int c = 0; c < m_iMaxCNozl; c++)
				{
					 iNozl = gNzlNo(r, c);
					 if (CHPS[iNozl].IsGood()) return true;
				}
			}
			//
			return false;
        }	
        public bool FindFail()
        {
			int iNozl;
		
			for (int r = 0 ; r < m_iMaxRNozl; r++) 
			{
				for (int c = 0; c < m_iMaxCNozl; c++)
				{
					 iNozl = gNzlNo(r, c);
					 if (CHPS[iNozl].IsFail()) return true;
				}
			}
			//
			return false;
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
            for (int i = 0 ; i < m_iMaxNozl ; i++) {
                 if (FindChip(FindMode , i)) {
                     R = gRowNzlNo(i);
                     C = gColNzlNo(i);
                     return true;
                     }
                }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindLastRowCol        (EN_FIND        FindMode , out int R , out int C )
        {
            for (int j = m_iMaxNozl - 1; j >= 0; j--)
            {
                if (FindChip(FindMode , j)) {
                    R = gRowNzlNo(j);
                    C = gColNzlNo(j);
                    return true;
                    }
            }

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindFrstRowLastCol        (EN_FIND        FindMode , out int R , out int C )
        {
			int iNozl;
			for (int r = 0; r < m_iMaxRNozl; r++)
			{
				for (int c = m_iMaxCNozl - 1; c >= 0; c--)
				{
					iNozl = gNzlNo(r, c);
				    if (FindChip(FindMode , iNozl)) {
				        R = gRowNzlNo(iNozl);
				        C = gColNzlNo(iNozl);
				        return true;
				        }
				}
			}

            //No Find.
            R = -1;
            C = -1;
            return false;
        }
		public bool  FindFrstRowLastCol        (EN_FIND        FindMode , EN_RSLT_KIND RsltKind, out int R , out int C )
        {
			int iNozl;
			for (int r = 0; r < m_iMaxRNozl; r++)
			{
				for (int c = m_iMaxCNozl - 1; c >= 0; c--)
				{
					iNozl = gNzlNo(r, c);
				    if (FindChip(FindMode , iNozl) && this[iNozl].gVRslt(RsltKind) == (int)EN_CHIP_RSLT.None) {
				        R = gRowNzlNo(iNozl);
				        C = gColNzlNo(iNozl);
				        return true;
				        }
				}
			}

            //No Find.
            R = -1;
            C = -1;
            return false;
        }

		//Find Shift
		public bool  FndShiftRFirstC       (EN_FIND FindMode, out int Shift, bool IsRev = false)
        {
            //Initialize.
			int iNzl;

	        //RShift
            Shift = 0;

            if(!IsRev) 
			{
			   for (int r = 0; r < m_iMaxRNozl; r++)
					 for (int c = 0 ; c < m_iMaxCNozl ; c++) 
			   {
					{
						 iNzl = (r * m_iMaxCNozl) + c;
						 if (FindChip(FindMode, iNzl) && !m_bMaskNozl[iNzl]) { Shift = r; return true; }
					}
			   }
            }
            else 
			{
		        for (int r = m_iMaxRNozl-1 ; r >= 0  ; r--) {
                     for (int c = 0 ; c < m_iMaxCNozl ; c++) {
						  iNzl = (r * m_iMaxCNozl) + c;
				          if (FindChip(FindMode, iNzl) && !m_bMaskNozl[iNzl]) {
                              Shift = (m_iMaxRNozl-1) - r;
                              return true;
					          }
				         }
                    }
            }
            return false;
        }
        public bool FndShiftRLastC(EN_FIND FindMode, out int Shift, bool IsRev = false)
        {
            //Initialize.
			int iNzl;

            //RShift
            Shift = 0;

            if(!IsRev) {
                for (int r = 0 ; r < m_iMaxRNozl; r++) {
                     for (int c = m_iMaxCNozl-1 ; c >= 0 ; c--) {
						 iNzl = (r * m_iMaxCNozl) + c;
				         if (FindChip(FindMode, iNzl) && !m_bMaskNozl[iNzl]) {
                              Shift = r;
                              return true;
                              }
                         }
                    }
                }
            else {
                for (int r = m_iMaxRNozl-1 ; r >= 0  ; r--) {
                     for (int c = m_iMaxCNozl-1 ; c >= 0 ; c--) {
						 iNzl = (r * m_iMaxCNozl) + c;
				         if (FindChip(FindMode, iNzl) && !m_bMaskNozl[iNzl]) {
                              Shift = (m_iMaxRNozl-1) - r;
                              return true;
					          }
                         }
			        }
                }
            return false;
        }
        public bool FndFirstShiftC(EN_FIND FindMode, out int Shift, int RShift = 0)
        {
            int R, C;
			int iNzl;
            Shift = 0;

            for (C = 0 ; C < m_iMaxCNozl ; C++) {
				for (R = RShift ; R < m_iMaxRNozl; R++) {
					iNzl = (R * m_iMaxCNozl) + C;
					if (FindChip(FindMode, iNzl) && !m_bMaskNozl[iNzl]) {
					    Shift = C;
					    return true;
					    }
					}
                }
            return false;
        }
        public bool FndLastShiftC(EN_FIND FindMode, out int Shift, int RShift = 0)
        {
            int R,C;
			int iNzl;
            Shift = 0;

            for (C = m_iMaxCNozl-1 ; C >= 0 ; C--) {
                for (R = RShift ; R < m_iMaxRNozl; R++) {
					iNzl = (R * m_iMaxCNozl) + C;
			        if (FindChip(FindMode, iNzl) && !m_bMaskNozl[iNzl]) {
                        Shift = (m_iMaxCNozl-1)-C;
                        return true;
                        }
                    }
                }
            return false;
        }
        public bool FndLastShiftC(EN_FIND FindMode, int WhreBin, out int Shift, int RShift = 0)
        {
            int R,C;
			int iNzl;
            Shift = 0;

            for (C = m_iMaxCNozl-1 ; C >= 0 ; C--) {
                for (R = RShift ; R < m_iMaxRNozl; R++) {
					iNzl = (R * m_iMaxCNozl) + C;
			        if (FindChip(FindMode, iNzl) && !m_bMaskNozl[iNzl] && this[iNzl]._iBinWhre == WhreBin) {
                        Shift = (m_iMaxCNozl-1)-C;
                        return true;
                        }
                    }
                }
            return false;
        }
		public bool FndShiftR(EN_FIND FindMode, out int ShiftR)
		{
			ShiftR = 0;
			for (int r = 0; r < m_iMaxRNozl; r++)
			{
				if (FindChipRow(FindMode , r)) { ShiftR = r; return true; }
			}
			return false;
		}

		//Update Chip Status.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void GetImageRC(ref System.Windows.Forms.PictureBox pb,  int X, int Y, out int R, out int C)
        {
            int   uR , uC ;
            int   iR, iC  ;

            //
			uR = m_iMaxRNozl;
			uC = m_iMaxCNozl;
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
            int    iMaxRow = m_iMaxRNozl;
            int    iMaxCol = m_iMaxCNozl;

            iRow = 0;
            iCol = 0;

            switch (m_iDispDir) 
			{
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
		public void  UpdateChip(ref System.Windows.Forms.PictureBox pb)
        {
            //
			string sTemp;
            if (pb == null) return;
            Bitmap bmp = new Bitmap(pb.Width, pb.Height);
            Graphics g = Graphics.FromImage(bmp); 
            Brush brush;
            //string sTemp;

            //
			Color sBColor   = Color.White;
            Color sPColor   = Color.Black;  
            int   iMaxX = m_iMaxCNozl; 
            int   iMaxY = m_iMaxRNozl; 
            int   iMinX = 0; 
            int   iMinY = 0;     
            int   iR, iC  ; 
			int   iNozl;

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
					 //
					 iNozl = (iR * m_iMaxCNozl) + iC;
					//
                    sBColor = CHPS[iNozl].GetBinColor (EN_OBJ_KIND.TOOL);
					brush = new SolidBrush(sBColor);
					//
					g.FillRectangle(brush     , uX1, uY1, (uX2 - uX1)   , (uY2 - uY1)); //채우기
					if ((c == 0) && (r == 0))
						 g.DrawRectangle(Pens.Red  , uX1, uY1, (uX2 - uX1)-1 , (uY2 - uY1)-1); //테두리 사각형\
					else g.DrawRectangle(Pens.Black, uX1, uY1, (uX2 - uX1)-1 , (uY2 - uY1)-1); //테두리 사각형\
                    //                  
                    sTemp = string.Format("{0}", iNozl + 1);
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

		public void  UpdateRot(ref System.Windows.Forms.PictureBox pb, int Nozl)
        {
            //
            if (pb == null) return;
            Bitmap bmp = new Bitmap(pb.Width, pb.Height);
            Graphics g = Graphics.FromImage(bmp); 
            Brush brush;

            //string sTemp;
            //
			Color sBColor   = Color.White;
            Color sPColor   = Color.Black;  
			//
			if ((Nozl < 0) || (Nozl >= m_iMaxNozl)) return;
            //
            FNC.ClearPictureBox(ref pb, Color.White);
			//
            sBColor = CHPS[Nozl].GetBinColor (EN_OBJ_KIND.TOOL);
			brush = new SolidBrush(sBColor);

			g.FillRectangle(brush     , 0 , 0, pb.Width - 1   , pb.Height - 1); //채우기
			g.DrawRectangle(Pens.Black, 0 , 0, pb.Width - 1   , pb.Height - 1);
			//
			if (brush != null) brush.Dispose();
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
       
        public int GetVacNo(int Whre , int MotrIndex = -1)
        {
            int iNozlNo;

            if (MotrIndex == -1) iNozlNo =  ((Whre + m_iPosHead1) % m_iMaxNozl);
            else                 iNozlNo =  MotrIndex                          ;

            if (iNozlNo <= 0) iNozlNo = m_iMaxNozl-1;
            return iNozlNo;
        }

        public bool ShiftData(int Cnt)
        {
            //Local Var.
            TChip TempChip;
            int   iMoveCnt;

            //Check Count
            if (Cnt == 0         ) return true ;
            if (Cnt == m_iMaxNozl) return true ;
            if (Cnt >= m_iMaxNozl) return false;


            //Shift 1st Head.
            m_iPosHead1 += Cnt;
            if(m_iPosHead1<0) {
                m_iPosHead1 = m_iMaxNozl - 1;
                }
            m_iPosHead1  = m_iPosHead1 % m_iMaxNozl;

            iMoveCnt = Math.Abs(Cnt);
            if(Cnt<0) {
               TempChip = gCHIP(0);
                for (int n = 0; n < m_iMaxNozl-1; n++) {
                    sCHIP(n, gCHIP(n + 1));
                    }
                sCHIP(m_iMaxNozl - iMoveCnt, TempChip);
                }
            else {
                TempChip = gCHIP(m_iMaxNozl - iMoveCnt);
                for (int n = m_iMaxNozl - iMoveCnt - 1; n >= iMoveCnt - 1; n--) {
                    sCHIP(n + 1,gCHIP(n));
                    }
                sCHIP(iMoveCnt - 1, TempChip);
                }

           //Ok.
           return true;
        }

		//Loading Para.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void  Load(BinaryReader br)
        {//UserSet - Tool 변수 Load (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
            for (int c = 0 ; c < vDEF.MAX_NOZL; c++) 
            {
                if(CHPS[c] == null) continue;
                CHPS[c].Load(br);
            }
            UNIT.Load(br);

            m_sSpare1   = br.ReadString().Trim();
            m_sSpare2   = br.ReadString().Trim();
            m_sSpare3   = br.ReadString().Trim();
            m_sSpare4   = br.ReadString().Trim();
            m_sSpare5   = br.ReadString().Trim();
                         
            m_bSpare1   =  br.ReadBoolean();
            m_bSpare2   =  br.ReadBoolean();
            m_bSpare3   =  br.ReadBoolean();
            m_bSpare4   =  br.ReadBoolean();
            m_bSpare5   =  br.ReadBoolean();
            m_bSpare6   =  br.ReadBoolean();
            m_bSpare7   =  br.ReadBoolean();
            m_bSpare8   =  br.ReadBoolean();
            m_bSpare9   =  br.ReadBoolean();
            m_bSpare10  =  br.ReadBoolean();

			m_iPosHead1 =  br.ReadInt32();
            m_iSpare1   =  br.ReadInt32();
            m_iSpare2   =  br.ReadInt32();
            m_iSpare3   =  br.ReadInt32();
            m_iSpare4   =  br.ReadInt32();
            m_iSpare5   =  br.ReadInt32();
            m_iSpare6   =  br.ReadInt32();
            m_iSpare7   =  br.ReadInt32();
            m_iSpare8   =  br.ReadInt32();
            m_iSpare9   =  br.ReadInt32();
            m_iSpare10  = br.ReadInt32();

            m_dSpare1   =  br.ReadDouble();
            m_dSpare2   =  br.ReadDouble();
            m_dSpare3   =  br.ReadDouble();
            m_dSpare4   =  br.ReadDouble();
            m_dSpare5   =  br.ReadDouble();
            m_dSpare6   =  br.ReadDouble();
            m_dSpare7   =  br.ReadDouble();
            m_dSpare8   =  br.ReadDouble();
            m_dSpare9   =  br.ReadDouble();
            m_dSpare10  = br.ReadDouble();

        }
		public void  Save(BinaryWriter wr)
        {//UserSet - Tool 변수 Save (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
            for (int c = 0 ; c < vDEF.MAX_NOZL ; c++) 
            {
                if(CHPS[c] == null) continue;
                CHPS[c].Save(wr);
            }

            UNIT.Save(wr);

            wr.Write(m_sSpare1 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare2 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare3 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare4 .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sSpare5 .PadRight(vDEF.MAX_STR_LEN, ' '));
                               
            wr.Write(m_bSpare1  );
            wr.Write(m_bSpare2  );
            wr.Write(m_bSpare3  );
            wr.Write(m_bSpare4  );
            wr.Write(m_bSpare5  );
            wr.Write(m_bSpare6  );
            wr.Write(m_bSpare7  );
            wr.Write(m_bSpare8  );
            wr.Write(m_bSpare9  );
            wr.Write(m_bSpare10 );

			wr.Write(m_iPosHead1);
            wr.Write(m_iSpare1  );
            wr.Write(m_iSpare2  );
            wr.Write(m_iSpare3  );
            wr.Write(m_iSpare4  );
            wr.Write(m_iSpare5  );
            wr.Write(m_iSpare6  );
            wr.Write(m_iSpare7  );
            wr.Write(m_iSpare8  );
            wr.Write(m_iSpare9  );
            wr.Write(m_iSpare10 );
							    
            wr.Write(m_dSpare1  );
            wr.Write(m_dSpare2  );
            wr.Write(m_dSpare3  );
            wr.Write(m_dSpare4  );
            wr.Write(m_dSpare5  );
            wr.Write(m_dSpare6  );
            wr.Write(m_dSpare7  );
            wr.Write(m_dSpare8  );
            wr.Write(m_dSpare9  );
            wr.Write(m_dSpare10 );
        }
    }
}
