using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TMagazine                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TMagazine
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //public:    /* Direct Accessable Vars.  */
        public EN_MGZ_ID m_Id;

        //protected: /* Inheritable Vars.        */

        //private:   /* Member Var.             */
        EN_MGZ_STAT    m_iStat         ;
        EN_PORT_MODE   m_iMode         ;
        EN_PORT_STATUS m_iPortStatus   ;
        EN_PORT_OPER   m_iPortOper     ;
        EN_MAP_DIR     m_iDispDir      ;
        string         m_sLotNo        ;
        string         m_sRFID         ;
        int            m_iMaxSlot      ;
        int            m_iTargerMC     ; //Target ASM
        bool           m_bDoorOpen     ;

		//Spare Var.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool    m_bSpare1, m_bSpare2;
        int     m_iSpare1, m_iSpare2;
        double  m_dSpare1, m_dSpare2;

        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TWafer this[int R]
        {
            get
            {
                if (R < 0 || R >= vDEF.MAX_MGZ_SLOT) return null;
                return WAF[R];
            }
            set
            {
                if (R < 0 || R >= vDEF.MAX_MGZ_SLOT) return;
                WAF[R] = value;
            }
        }

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_MGZ_ID   _Id          { get { return m_Id       ;  } }
        public string	   _sRFID       { get { return m_sRFID    ;  } set { m_sRFID     = value; } }
        public int         _iMaxSlot     
        { 
            get { return m_iMaxSlot ;  } 
            set 
            {
                if (value > vDEF.MAX_MGZ_SLOT) value = vDEF.MAX_MGZ_SLOT;
                m_iMaxSlot  = value; 
            } 
        }
        public int         _iTargerMC    => m_iTargerMC ;
        public EN_MAP_DIR  _iDispDir     { get { return m_iDispDir ;  } set { m_iDispDir  = value; } }
        public string      _sLotNo       { get { return m_sLotNo   ;  } set { m_sLotNo    = value; } }
        public bool        _bDoorOpen    { get { return m_bDoorOpen;  } set { m_bDoorOpen = value; } }


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TWafer[] WAF  = new TWafer[vDEF.MAX_MGZ_SLOT];

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TMagazine()
        {
            //
            m_iStat       = EN_MGZ_STAT.None      ;
            m_iMode       = EN_PORT_MODE.none     ;
            m_iPortStatus = EN_PORT_STATUS.Disable;
            m_iPortOper   = EN_PORT_OPER.none     ;

            m_iTargerMC   = -1                    ;
            m_sLotNo      = string.Empty;
            m_sRFID       = string.Empty;
            m_bDoorOpen   = false       ; 
            m_iMaxSlot    = 0           ; 
            m_iDispDir    = 0           ;

            //
            for (int r = 0; r < vDEF.MAX_MGZ_SLOT; r++)
            {
                WAF[r] = new TWafer();
            }

            Init();
        }
        ~TMagazine() { }
        //------------------------------------------------------------------------
        public TMagazine Copy()
        {
            //객체 Copy할때는 생성자에 매개변수가 없는 생성자를 만들어서 사용해야함.
            return FNC.DeepClone(this) as TMagazine;
        }

        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TWafer gWAF(int R) { return this[R]; }


        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void sWAF(int R, TWafer pWafer) { this[R] = pWafer; }

        //------------------------------------------------------------------------
        public void SetTargerMC(int mc)
        {
            m_iTargerMC = mc;

            //
            SetToTarget(mc);
        }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
            m_iMaxSlot    = vDEF.MAX_MGZ_SLOT;
            
            m_sLotNo      = string.Empty;
            m_sRFID       = string.Empty;
            m_bDoorOpen   = false; 
            
            m_iDispDir    = 0;

            m_iTargerMC   = -1;


            SetTo   (EN_WAFER_STAT.None);
            SetToMGZ(EN_MGZ_STAT  .None);
        }
        //------------------------------------------------------------------------
        //Clear.
        public void ClearMap()
        {
            for (int r = 0 ; r < vDEF.MAX_MGZ_SLOT ; r++) 
            {
                this[r].ClearMap();
		    }

            m_sLotNo      = string.Empty;
            m_sRFID       = string.Empty;
            m_bDoorOpen   = false; 
            
            m_iDispDir    = 0;
          //m_iTargerMC   =-1;
            
            SetTo   (EN_WAFER_STAT.None);
            SetToMGZ(EN_MGZ_STAT  .None);
        }
        //Set Magazine Status
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetToMGZ(EN_MGZ_STAT Stat)
        {
            m_iStat = Stat;
        }
        //------------------------------------------------------------------------
        public void SetPortMode(EN_PORT_MODE mode)
        {
            m_iMode = mode;
        }
        //------------------------------------------------------------------------
        public void SetPortStatus(EN_PORT_STATUS status)
        {
            m_iPortStatus = status;
        }
        //------------------------------------------------------------------------
        public void SetPortOper(EN_PORT_OPER oper)
        {
            m_iPortOper = oper;
        }
        //------------------------------------------------------------------------
        public void ClearPortOper()
        {
            m_iPortOper = EN_PORT_OPER.none;
        }

        //------------------------------------------------------------------------
        public EN_PORT_MODE GetPortMode()
        {
            return m_iMode ;
        }
        //------------------------------------------------------------------------
        public EN_PORT_STATUS GetPortStatus()
        {
            return m_iPortStatus ;
        }
        //------------------------------------------------------------------------
        public EN_PORT_OPER GetPortOper()
        {
            return m_iPortOper;
        }

        //------------------------------------------------------------------------
        public void SetToRFID(string id) //FOUP ID
        {
            for (int r = 0; r < m_iMaxSlot; r++)
            {
                this[r].SetToRFID(id);
            }
        }
        //------------------------------------------------------------------------
        public void SetToRFID(int r, string id)
        {
            this[r].SetToRFID(id);
        }
        //------------------------------------------------------------------------
        public void SetToTarget(int target)
        {
            for (int r = 0; r < m_iMaxSlot; r++)
            {
                this[r].SetToTarget(target);
            }
        }
        //------------------------------------------------------------------------
        public void SetToTarget(int r, int target)
        {
            this[r].SetToTarget(target);
        }
        //------------------------------------------------------------------------
        public void SetToPanelID(int r, string id)
        {
            this[r].SetPanelID(id);
        }

        //Set Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetTo(int r, EN_WAFER_STAT Stat)
        {
            if (r < 0 || r >= vDEF.MAX_MGZ_SLOT) return;
            this[r].SetTo(Stat);
        }
        //------------------------------------------------------------------------
        public void SetTo(EN_WAFER_STAT Stat)
        {
            for (int r = 0 ; r < m_iMaxSlot ; r++) 
            {
                this[r].SetTo(Stat);

                //
                if (Stat == EN_WAFER_STAT.Mount || Stat == EN_WAFER_STAT.Mask)
                {
                    this[r]._iTargerMC = m_iTargerMC;
                }
            }
        }
        //------------------------------------------------------------------------
		//Get Chip Status.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public EN_WAFER_STAT  GetStat            (int r) 
        { 
            if(this[r] == null) return EN_WAFER_STAT.None;

            return this[r].GetWaferStat() ; 
        }
        //------------------------------------------------------------------------
		public bool  IsOneExist     (                ) 
        {  
            for (int r=0; r<m_iMaxSlot; r++) {
                if(this[r] == null) continue;
                if(this[r].IsExist(    )) return true ; 
            } 
            return false;
        }
        //------------------------------------------------------------------------
        public bool IsOneStat(EN_WAFER_STAT Stat) 
        { 
            for (int r=0; r<m_iMaxSlot; r++) {
                if(this[r] == null) continue;
                if (this[r].IsStat(Stat)) return true; 
            } 
            return false; 
        }


		//Check Chip Status.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool  IsExist        (int r) 
        { 
            if ((r < 0) || (r >= m_iMaxSlot)) return false;
            if(this[r] == null) return false; 
            //
            return (this[r].IsExist     (    )); 
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool IsEmpty(int r)
        {
            if ((r < 0) || (r >= m_iMaxSlot)) return false;
            if (this[r] == null) return false;
            //
            return (this[r].IsEmpty());
        }

        public bool IsStat(int r, EN_WAFER_STAT Stat) 
        { 
            if ((r < 0) || (r >= m_iMaxSlot)) return false;
            //
            if(this[r] == null) return false;  
            return (this[r].IsWaferStat(Stat)); 
        }

        //Check All Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool  IsAllExist     (                 ) 
        { 
            for (int r = 0 ; r < m_iMaxSlot ; r++) 
            { 
                if (!this[r].IsWaferExist (                   )) return false; 
            } 
            return true; 
        }
        public bool IsAllEmpty()
        {
            for (int r = 0; r < m_iMaxSlot; r++)
            {
                if (!this[r].IsWaferEmpty()) return false;
            }
            return true;
        }

        public bool IsAllStat(EN_WAFER_STAT Stat) 
        { 
            for (int r = 0 ; r < m_iMaxSlot ; r++) 
            { 
                if (!this[r].IsWaferStat(Stat)) return false; 
            } 
            return true; 
        }

        //Get Row Count by ChipStatus.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int   GetCntExist    () 
        { 
            int iCnt = 0; 
            for (int r = 0 ; r < m_iMaxSlot ; r++) {
                if(this[r] == null) continue;    
                if (this[r].IsWaferExist(        )) iCnt++; 
            }
            return iCnt; 
        }
        public int GetCntStat(EN_WAFER_STAT Stat) 
        { 
            int iCnt = 0; 
            for (int r = 0; r < m_iMaxSlot; r++) 
            {
                if(this[r] == null) continue;    
                if (this[r].IsWaferStat(Stat)) iCnt++; 
            }
            return iCnt; 
        }

        //Search .
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int FindFrstRow(EN_WAFER_STAT Stat)
        {
            //Local Var.
            int iFndR = -1;

            //Find First Row and Col.
            for (int r = 0; r < m_iMaxSlot; r++) {
                if(this[r].IsWaferStat(Stat))  { iFndR = r; break; }
                }

            return iFndR;
        }
        //------------------------------------------------------------------------
        public int FindLastRow(EN_WAFER_STAT Stat)
        {
            //Local Var.
            int iFndR = -1;

            //Find Last Row and Col.
            for (int r = m_iMaxSlot - 1 ; r >= 0 ; r--) 
            {
                if(this[r].IsWaferStat(Stat))  { iFndR = r; break; }
            }

            //No Find.
            return iFndR;
        }

        //Update Chip Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void GetImageRC(ref System.Windows.Forms.PictureBox pPBox, int X, int Y, out int R)
        {
            //Local Var.
            int rRow = 0;
            R = 0;

            if (pPBox == null) return;

            //Set Disp Dir.
            int uRow = m_iMaxSlot;

            if(uRow<1) uRow = 1;


            //이미지 사이즈
            int iMainW   = pPBox.Size.Width ;
            int iMainH   = pPBox.Size.Height;
 
            //각 CELL 사이즈 
            int iCellW   = iMainW -1 ;
            int iCellH   = (iMainH / uRow);

            //중심을 맞추기 위해서 사용  
            //int iCOffW   = 1;
            int iCOffH   = (iMainH - (iCellH * uRow)) / 2;

            int icalR = (int)((double)(Y-iCOffH)/ iCellH);

            //Set Row Col.
            switch (m_iDispDir)
            {
                default                 : rRow = icalR;              break; // 0
                case EN_MAP_DIR.Deg180 : rRow = (uRow - 1) - icalR; break; // 180
            }
            //return.
            R = rRow;
        }
        //------------------------------------------------------------------------
        public void  GetDispRC         (int r, out float iRow)
        {
            float iMaxRow = m_iMaxSlot;
            iRow = 0;
            switch (m_iDispDir)
            {
                default                 : iRow = r; ;               break; // 0
                case EN_MAP_DIR.Deg180 : iRow = (iMaxRow - 1) - r; break; // 180
            }

        }
        //------------------------------------------------------------------------
        public void Update(ref System.Windows.Forms.PictureBox pPBox, EN_MAP_DIR iDir = EN_MAP_DIR.None)
        {
            try
            {
                //Local Var.
                Color         sBColor  = Color.White;
                Color         sPColor  = Color.Black;   
                float         iMaxR    = m_iMaxSlot ;
                string        sTemp,sTemp2          ;
                EN_WAFER_STAT iStat;          
                float         iDX , iDY;

                float sRow;

                Bitmap bmp  = new Bitmap(pPBox.Width, pPBox.Height);
                Graphics gr = Graphics.FromImage(bmp);

                if (pPBox == null           ) return;
                if (iDir  != EN_MAP_DIR.None) m_iDispDir   = iDir;

                //이미지 사이즈
                float iMainW   = pPBox.Size.Width ;
                float iMainH   = pPBox.Size.Height;

                if(iMaxR<=0) iMaxR = 1;
 
                //각 CELL 사이즈 
                float iCellW   = iMainW -1 ;
                float iCellH   = (iMainH / iMaxR);

                //중심을 맞추기 위해서 사용  
                float iCOffW   = 1;
                float iCOffH   = (iMainH - (iCellH * iMaxR)) / 2;

                for (int r = 0 ; r < iMaxR ; r++) 
                {
			        GetDispRC(r, out sRow);

			        iDX = iCOffW ;
			        iDY = iCOffH + sRow * iCellH;

                    sPColor = Color.Black;
                    iStat   = GetStat(r);
                    if (iStat == EN_WAFER_STAT.Fail)
                    {
                        sBColor = cDEF.SEQ._bFlick1 ? WAF[r].GetStatColor() : Color.Yellow;
                    }
                    else sBColor = WAF[r].GetStatColor();

                    if (iStat < 0) iStat = 0;
                    sTemp2  = vDEF.STR_WAF_STAT[(int)iStat] ;

                    DrawRect(ref gr, iDX, iDY, iCellW - 1 , iCellH - 1, sPColor, sBColor);
                    if (r == 0) 
                        DrawRect(ref gr, iDX+1, iDY+1, iCellW-2, iCellH-2, Color.Red);

                    //sTemp = string.Format($"[{r + 1:00}] {sTemp2}");
                    sTemp = string.Format("[{0:00}] {1}-{2}", (iMaxR - r), sTemp2, WAF[r]._sBarCodeNo); //JUNG/220329
                    DrawText(ref gr, iDX + 1, iDY + 1, Color.Black, sTemp, 7);
                    sTemp  = string.Empty;
                    sTemp2 = string.Empty;
                }
                pPBox.Image?.Dispose();
                pPBox.Image = bmp;
                if (gr     != null) gr    .Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
                return;
            }

        }
        //------------------------------------------------------------------------
        public void DrawRect(ref Graphics g, float x1, float y1, float x2, float y2, Color PenColor, Color? BrushColor = null)
        {
            
            Pen p = new Pen(PenColor);
            g.DrawRectangle(p, x1, y1, x2, y2);
            if(BrushColor != null)
            {
                Brush brush = new SolidBrush((Color)BrushColor);            
                g.FillRectangle(brush, x1+1, y1+1, x2-2, y2-2);
                brush?.Dispose();
                brush = null;
            }
            p?.Dispose();
            p = null;


        }
        //------------------------------------------------------------------------
        public void DrawRect(ref Graphics g, int x1, int y1, int x2, int y2, Color PenColor, Color? BrushColor = null)
        {
            DrawRect(ref g,  x1,  y1,  x2,  y2,  PenColor,BrushColor);
            //Pen p = new Pen(PenColor);
            //g.DrawRectangle(p, x1, y1, x2, y2);
            //p = null;
            //if(BrushColor != null) {
            //    Brush brush = new SolidBrush((Color)BrushColor);            
            //    g.FillRectangle(brush, x1+1, y1+1, x2-2, y2-2);
            //    brush = null;
            //    }
            
        }

        public void DrawText(ref Graphics g, float x1, float y1, Color PenColor, string sText, int fortsize = 6)
        {
            using(Font myFont = new Font("Small Fonts", fortsize))
            using(Brush brush = new SolidBrush(PenColor))
            {
                g.DrawString(sText, myFont, brush, new PointF(x1, y1));
            }
            //Font myFont = new Font("Small Fonts", fortsize);
            //Brush brush = new SolidBrush(PenColor);
            //myFont = null;        --s
        }

        public void DrawText(ref Graphics g, int x1, int y1, Color PenColor, string sText)
        {
            DrawText(ref g, x1, y1, PenColor, sText);
            //Font myFont = new Font("Small Fonts", 6);
            //Brush brush = new SolidBrush(PenColor);
            //g.DrawString(sText, myFont, brush, new Point(x1, y1));
            //myFont = null;

        }

        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(BinaryReader br)
        {//UserSet - Magazine 변수 Load (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
            
            for (int r = 0; r < vDEF.MAX_MGZ_SLOT; r++)
            {
                if(this[r] == null) continue;
                this[r].Load(br);
            }

            m_sLotNo      = br.ReadString().Trim();
            m_sRFID       = br.ReadString().Trim();

            m_bDoorOpen   =  br.ReadBoolean();
            m_bSpare1     =  br.ReadBoolean();
            m_bSpare2     =  br.ReadBoolean();

            m_iStat       = (EN_MGZ_STAT   )br.ReadInt32();
            m_Id          = (EN_MGZ_ID     )br.ReadInt32();
            m_iDispDir    = (EN_MAP_DIR    )br.ReadInt32();
            m_iTargerMC   =  br.ReadInt32( );
            m_iMode       = (EN_PORT_MODE  )br.ReadInt32();
            m_iPortStatus = (EN_PORT_STATUS)br.ReadInt32();
            m_iPortOper   = (EN_PORT_OPER  )br.ReadInt32();

            m_iSpare1     =  br.ReadInt32();
            m_iSpare2     =  br.ReadInt32();

            m_dSpare1     =  br.ReadDouble();
            m_dSpare2     =  br.ReadDouble();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Save(BinaryWriter wr)
        {//UserSet - Magazine 변수 Save (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)

            for (int r = 0; r < vDEF.MAX_MGZ_SLOT; r++)
            {
                if(this[r] == null) continue;
                this[r].Save(wr);
            }

            wr.Write(m_sLotNo  .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sRFID   .PadRight(vDEF.MAX_STR_LEN, ' '));
            
            wr.Write(m_bDoorOpen);
            wr.Write(m_bSpare1  );
            wr.Write(m_bSpare2  );
           
            wr.Write((int           )m_iStat      );
            wr.Write((int           )m_Id         );
            wr.Write((int           )m_iDispDir   );
            wr.Write(m_iTargerMC                  );
            wr.Write((int           )m_iMode      );
            wr.Write((int           )m_iPortStatus);
            wr.Write((int           )m_iPortOper  );

            wr.Write(m_iSpare1 );
            wr.Write(m_iSpare2 );

            wr.Write(m_dSpare1 );
            wr.Write(m_dSpare2 );
        }
    }
}
