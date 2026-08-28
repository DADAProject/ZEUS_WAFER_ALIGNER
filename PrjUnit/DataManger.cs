using Emgu.CV.CvEnum;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace eMachine
{
    /***************************************************************************/
    /* Class: TDataManger                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TDataManger
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
		int    m_iDispWafID;

        string m_sRFID     ;
        
        //protected: /* Inheritable Vars.        */

        
        
        //public:    /* Direct Accessable Vars.  */



        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public int _iDispWafID { get { return m_iDispWafID; } set { m_iDispWafID = value; } }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //UserSet - Chip 정보 셍성           
//      public TWafer     [] WAF        = new TWafer   [(int)EN_WAF_ID  .EndOfId];
//      public TMagazine  [] MGZ        = new TMagazine[(int)EN_MGZ_ID  .EndOfId];

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TDataManger()
        {//

            for (int i = 0; i < (int)EN_WAF_ID .EndOfId; i++) WAF [i] = new TWafer   ();

            for (int i = 0; i < (int)EN_MGZ_ID .EndOfId; i++) MGZ [i] = new TMagazine();

            Init();
        }
        ~TDataManger() { }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
            //Init.
            for (int t = 0; t < (int)EN_WAF_ID.EndOfId; t++) { WAF[t]?.Init(); } 
            for (int t = 0; t < (int)EN_MGZ_ID.EndOfId; t++) { MGZ[t]?.Init(); }
        }
        //------------------------------------------------------------------------
        public void ClearMap()
        {
            for (int t = 0; t < (int)EN_WAF_ID  .EndOfId; t++) { WAF[t]?.ClearMap(); }
            for (int t = 0; t < (int)EN_MGZ_ID  .EndOfId; t++) { MGZ[t]?.ClearMap(); }

        }  
        //------------------------------------------------------------------------
        public int GetNeedWorkMap()
        {
            int nCnt = 0;

            for (int t = 0; t < (int)EN_WAF_ID.EndOfId; t++) 
            {
                if(WAF[t].IsWaferExist()) nCnt++;
            }

            return nCnt; 
        }
        //------------------------------------------------------------------------
        //Set Row/Col Information
        public void SetRowColInfo()
        {
            MGZ[(int)EN_MGZ_ID.MGZ1]._iMaxSlot = cDEF.FM.ProjBase.iMaxMgzSlot[(int)EN_MGZ_ID.MGZ1];
            MGZ[(int)EN_MGZ_ID.MGZ2]._iMaxSlot = cDEF.FM.ProjBase.iMaxMgzSlot[(int)EN_MGZ_ID.MGZ2];

            //Target MC
            MGZ[(int)EN_MGZ_ID.MGZ1].SetTargerMC (cDEF.FM.ProjBase.iTargetASM[(int)EN_MGZ_ID.MGZ1]);
            MGZ[(int)EN_MGZ_ID.MGZ2].SetTargerMC (cDEF.FM.ProjBase.iTargetASM[(int)EN_MGZ_ID.MGZ2]);
        }
        //------------------------------------------------------------------------
		public bool ShiftWafData(EN_WAF_ID Src, EN_WAF_ID Dst)
		{
			if (Src <= EN_WAF_ID.None   ) return false;
			if (Src >= EN_WAF_ID.EndOfId) return false;
			if (Dst <= EN_WAF_ID.None   ) return false;
			if (Dst >= EN_WAF_ID.EndOfId) return false;
			//
			this.WAF[(int)Dst] = this.WAF[(int)Src].Copy();
			this.WAF[(int)Src].ClearMap();

            //
            return true;
		}
        //------------------------------------------------------------------------
		public bool ShiftMgzToWaf(EN_MGZ_ID Src, int SrcRow, EN_WAF_ID Dst)
		{
			if (Src    <= EN_MGZ_ID.None                        ) return false;
			if (Src    >= EN_MGZ_ID.EndOfId                     ) return false;
            if (SrcRow <  0                                     ) return false;
            if (SrcRow >= cDEF.FM.ProjBase.iMaxMgzSlot[(int)Src]) return false;
			if (Dst    <= EN_WAF_ID.None                        ) return false;
			if (Dst    >= EN_WAF_ID.EndOfId                     ) return false;
			//
            this.MGZ[(int)Src][SrcRow]._iSlot = SrcRow;
            
            //
			this.WAF[(int)Dst]            = this.MGZ[(int)Src][SrcRow].Copy();
            this.WAF[(int)Dst]._iFromMgz  = Src;
            
            //Auto Mode가 아닐경우에만...
            if(cDEF.FM.IsAutoMode()) this.WAF[(int)Dst]._iTargerMC = -1; 
            else                     this.WAF[(int)Dst]._iTargerMC = this.MGZ[(int)Src]._iTargerMC;

            this.MGZ[(int)Src][SrcRow].ClearMap  ();
            this.MGZ[(int)Src][SrcRow].SetTo(EN_WAFER_STAT.Empty);
			//
			return true;
		}
        //------------------------------------------------------------------------
		public bool ShiftWafToMgz(EN_WAF_ID Src, EN_MGZ_ID Dst, int DstRow)
		{
			if (Dst    <= EN_MGZ_ID.None                        ) return false;
			if (Dst    >= EN_MGZ_ID.EndOfId                     ) return false;
            if (DstRow <  0                                     ) return false;
            if (DstRow >= cDEF.FM.ProjBase.iMaxMgzSlot[(int)Dst]) return false;
			if (Src    <= EN_WAF_ID.None                        ) return false;
			if (Src    >= EN_WAF_ID.EndOfId                     ) return false;
			//
			this.MGZ[(int)Dst][DstRow] = this.WAF[(int)Src].Copy();
			this.WAF[(int)Src].ClearMap();
			//
			return true;
		}

        //Get Display Dir.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void GetDispDirRC(int Dir, int mRow, int mCol, int fRow, int fCol, out int R, out int C)
        {
            int iRow, iCol;
            switch (Dir)
            {
                default: iRow = 0; iCol = 0;                                 break;
                case  0: iRow = fRow; iCol = fCol;                           break; // 0                    //좌상
                case  1: iRow = fRow; iCol = (mCol - 1) - fCol;              break; // 0   -> Mirrror       //우상
                case  2: iRow = (mRow - 1) - fRow; iCol = (mCol - 1) - fCol; break; // 0   -> 180           //우하
                case  3: iRow = (mRow - 1) - fRow; iCol = fCol;              break; // 0   -> 180 -> Mirror //좌하
                case  4: iRow = fCol; iCol = (mRow - 1) - fRow;              break; // 90
                case  5: iRow = (mCol - 1) - fCol; iCol = fRow;              break; // 270
                case  6: iRow = (mCol - 1) - fCol; iCol = (mRow - 1) - fRow; break; // 90  -> Mirror
                case  7: iRow = fCol; iCol = fRow;                           break; // 270 -> Mirror
            }

            R = iRow;
            C = iCol;
        }
		//
        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(bool IsLoad)
        {//UserSet - Chip 정보 All Load/Save
            LoadMap   (IsLoad);
        }
        //------------------------------------------------------------------------
        public void LoadMap(bool IsLoad)
        {//UserSet - Chip 정보 All Load/Save
            try
            {
                //Local Var.
                string sPath;
                //Make Dir.
                FNC.CreateDirOnWork("SeqData");
                sPath = Application.StartupPath + "\\SeqData\\DataMap.DAT";

                //File Open.
                //int iFAccess = IsLoad ? (int)FileAccess.Read : (int)FileAccess.Write;
                FileAccess iFAccess = IsLoad ? FileAccess.Read : FileAccess.Write;
                FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, (FileAccess)iFAccess);

                if (fp == null) { return; }

                //Read&Write.
                if(IsLoad) 
                {
				    BinaryReader br = new BinaryReader(fp);
					if(br.PeekChar()<0) return;
					
					for (int i = 0; i < (int)EN_WAF_ID  .EndOfId; i++) WAF [i]?.Load(br);
					for (int i = 0; i < (int)EN_MGZ_ID  .EndOfId; i++) MGZ [i]?.Load(br);       

					br.Close();
					br = null;
                }   
                else 
                {
                    BinaryWriter wr = new BinaryWriter(fp);
                    
					for (int i = 0; i < (int)EN_WAF_ID  .EndOfId; i++) WAF[i]?.Save(wr);
                    for (int i = 0; i < (int)EN_MGZ_ID  .EndOfId; i++) MGZ[i]?.Save(wr);

                    wr.Close();
                    wr = null;
                }
                fp.Close();
                fp = null;
            }
            catch (Exception e)
            {
                cDEF.LOG.ExceptionTrace(e.StackTrace);
				MsgBox.Warning("Map Data Save/Load Exception!!!");
            }
        }
    }
}
