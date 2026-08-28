using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace eMachine
{
    public enum eScan : byte { NoScan = 0, Exist= 1, Empty=2 };
    public enum EN_WAF_SORT_MODE  : int
    {
        None    = -1  ,  
		NORMAL  =  0  ,
		SCAN		  ,
        EndOfId
    };
    public enum EN_ALGN_MODE  : int
    {
        None    = -1  ,  
		NALGN   =  0  , //Normal align
		PALGN		  , //Pre Align
        EndOfId
    };
    public enum SCANDIR
    {
        MINUS = -1,
        NONE  =  0,
        PLUS  =  1
    }

    /***************************************************************************/
    /* Class: TWafer                                                           */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TWafer
    {

        public const int _NAME_LENGTH = 100; // 문자 길이

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */

                
        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */

        EN_WAFER_STAT m_iStat                     ;
        int           m_iSlot                     ;
        
        string        m_sLotNo                    ;
        string        m_sBarNo                    ; //2D Barcode = Panel ID
        string        m_sRFID                     ; //FOUP ID
      //string        m_sPanelID                  ; //Panel ID

        EN_MGZ_ID     m_iFromMgz                  ;
      //int           m_iWhreMgzRow               ;
        int           m_iTargerMC                 ; //Target Main MC

        EN_MAP_DIR    m_iDispDir                  ;
        EN_MAP_DIR    m_iWafDir                   ;
		int           m_iWorkOptn                 ;
		int           m_iAlgnOptn                 ;

                   
		//Spare Var.
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        bool         m_bSpare1, m_bSpare2; 
        int          m_iSpare1, m_iSpare2; 
        double       m_dSpare1, m_dSpare2;


        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_WAFER_STAT _iStat          { get { return m_iStat          ;  } set { m_iStat            = value; } }
        public int           _iSlot          { get { return m_iSlot          ;  } set { m_iSlot            = value; } }
        public string        _sLotNo         { get { return m_sLotNo         ;  } set { m_sLotNo           = value; } }
        public string	     _sBarCodeNo     { get { return m_sBarNo         ;  }  set { m_sBarNo          = value; } }
        public string	     _sRFID          { get { return m_sRFID          ;  }  set { m_sRFID           = value; } }
        //public string	     _sPanelID       { get { return m_sPanelID       ;  }  set { m_sPanelID        = value; } }

		public EN_MAP_DIR    _iDispDir		 { get { return m_iDispDir       ;  }  set { m_iDispDir        = value; } }
		public EN_MAP_DIR    _iWafDir 		 { get { return m_iWafDir        ;  }  set { m_iWafDir         = value; } }
		public int		     _iWorkOptn	     { get { return m_iWorkOptn	     ;  }  set { m_iWorkOptn       = value; } }
		public int		     _iAlgnOptn	     { get { return m_iAlgnOptn	     ;  }  set { m_iAlgnOptn       = value; } }
      //public int           _iWhreMgzRow    { get { return m_iWhreMgzRow    ;  }  set { m_iWhreMgzRow     = value; } }
        public int           _iTargerMC      { get { return m_iTargerMC      ;  }  set { m_iTargerMC       = value; } }
        public EN_MGZ_ID     _iFromMgz       { get { return m_iFromMgz       ;  }  set { m_iFromMgz        = value; } }
        

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TWafer()
        {
            //Staus.
            m_iStat        = EN_WAFER_STAT.None;
            m_iSlot        = -1;
            
            m_sLotNo       = "";
            m_sBarNo       = "";
            m_sRFID        = "";
            //m_sPanelID     = "";

            m_iDispDir     = EN_MAP_DIR.Deg0;
            m_iWafDir      = EN_MAP_DIR.Deg0;

            m_iFromMgz     = EN_MGZ_ID.None;
            //m_iWhreMgzRow  = -1;
            m_iTargerMC    = -1;

            //
            Init();
        }
        ~TWafer() { }

        //------------------------------------------------------------------------
        public TWafer Copy()
        {
			return FNC.DeepClone(this) as TWafer;
        }

        //Get Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        

        //Set Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
            //
            m_iSlot       = -1;

            m_sLotNo      = "";
            m_sBarNo      = "";
            m_sRFID       = "";
            //m_sPanelID    = "";

            //m_iWhreMgzRow = -1;
            m_iTargerMC   = -1;

            //
            m_iFromMgz = EN_MGZ_ID.None;
            m_iStat    = EN_WAFER_STAT.None;
        }
        //------------------------------------------------------------------------
        //Clear.
        public void ClearMap()
        {
            //
            m_iSlot       = -1;

            m_sLotNo      = "";
            m_sBarNo      = "";
            m_sRFID       = "";
            //m_sPanelID    = "";

            //m_iWhreMgzRow = -1;
            m_iTargerMC   = -1;
            m_iFromMgz    = EN_MGZ_ID.None;

            //
            m_iStat = EN_WAFER_STAT.None;
        }
        //------------------------------------------------------------------------
        public void ClearData()
        {
            //
            m_iSlot       = -1;

            m_sLotNo      = "";
            m_sBarNo      = "";
            m_sRFID       = "";
            //m_sPanelID    = "";
            
            //m_iWhreMgzRow = -1;
            m_iTargerMC   = -1;
            m_iFromMgz    = EN_MGZ_ID.None;
        }
        //Set Unit.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetTo(EN_WAFER_STAT Stat)
        {
            if (Stat == EN_WAFER_STAT.None || Stat == EN_WAFER_STAT.Empty) ClearData();

            //
            m_iStat = Stat;
        }
        //------------------------------------------------------------------------
        public void SetToRFID(string id)
        {
            m_sRFID = id; 
        }
        //------------------------------------------------------------------------
        public void SetPanelID(string id)
        {
            m_sBarNo = id;
        }
        //------------------------------------------------------------------------
        public void SetToTarget(int targer)
        {
            m_iTargerMC = targer; 
        }

        //------------------------------------------------------------------------
        public void SetToBarCode(string id)
        {
            m_sBarNo = id;
        }
        //------------------------------------------------------------------------
        public bool IsExist()
        {
            bool isExist =  (m_iStat != EN_WAFER_STAT.Empty ) && 
                            (m_iStat != EN_WAFER_STAT.None  ) && 
                            //(m_iStat != EN_WAFER_STAT.Mask  ) && 
                            (m_iStat != EN_WAFER_STAT.Skip  );
            return isExist;
        }
        //------------------------------------------------------------------------
        public bool IsEmpty()
        {
            bool isEmpty = (m_iStat == EN_WAFER_STAT.Empty) ||
                           (m_iStat == EN_WAFER_STAT.None );
            return isEmpty;
        }

        //------------------------------------------------------------------------
        public bool IsNotExist()
        {
            bool isExist =  (m_iStat != EN_WAFER_STAT.Empty) && 
                            (m_iStat != EN_WAFER_STAT.None ) && 
                            //(m_iStat != EN_WAFER_STAT.Mask ) && 
                            (m_iStat != EN_WAFER_STAT.Skip );
            return isExist;
        }
        //------------------------------------------------------------------------
        public bool IsStat(EN_WAFER_STAT Stat)
        {
            return (m_iStat == Stat);
        }
        //------------------------------------------------------------------------
        public Color GetStatColor() 
        {//UserSet - Chip Status DISPLAY 색깔 처리
            Color iBinColor = Color.Black;
            iBinColor = cDEF.FM.ProjOptn.cStatColor[(int)m_iStat];

            //if      (m_iStat == EN_WAFER_STAT.None       ) iBinColor = Color.White  ; 
            //else if (m_iStat == EN_WAFER_STAT.Empty      ) iBinColor = Color.Silver ; 
            //else if (m_iStat == EN_WAFER_STAT.Mask       ) iBinColor = Color.SkyBlue; 
            //else if (m_iStat == EN_WAFER_STAT.Mount      ) iBinColor = Color.Aqua   ; 
            //else if (m_iStat == EN_WAFER_STAT.Aligned    ) iBinColor = Color.Brown  ; 
            //else if (m_iStat == EN_WAFER_STAT.Skip       ) iBinColor = Color.Gray   ; 
            //else if (m_iStat == EN_WAFER_STAT.Fnsh       ) iBinColor = Color.Purple ; 
            //else if (m_iStat == EN_WAFER_STAT.Work       ) iBinColor = Color.Lime   ;             
            //else if (m_iStat == EN_WAFER_STAT.Wait       ) iBinColor = Color.Yellow ;  
            //else if (m_iStat == EN_WAFER_STAT.Fail       ) iBinColor = Color.Red    ; 
            //else                                           iBinColor = Color.Black  ; 

            return iBinColor;
        }
        //------------------------------------------------------------------------
        public EN_WAFER_STAT GetWaferStat()
        {
            return m_iStat;
        }

        //------------------------------------------------------------------------
        public bool  IsWaferExist     (                 ) 
        { 
            return IsExist();
        }
        //------------------------------------------------------------------------
        public bool IsWaferEmpty()
        {
            return IsEmpty();
        }
        //------------------------------------------------------------------------
        public bool  IsWaferStat      (EN_WAFER_STAT Stat) 
        { 
            return IsStat(Stat); 
        }
        //------------------------------------------------------------------------
		public bool IsMapDirVerti() 
        { 
            return (m_iDispDir == EN_MAP_DIR.Deg90) || (m_iDispDir == EN_MAP_DIR.Deg270); 
        }

        //------------------------------------------------------------------------
        public void UpdateUnit(ref System.Windows.Forms.PictureBox pPBox, bool DrawEillipse = false, bool show = false)
        {
            //Local Var.
			Brush         brush ;
            Color         sBColor  = Color.White;
            Color         sPColor  = Color.Black;   
            //string        sTemp    = ""         ;
            string        sTemp2   = ""         ;
            EN_WAFER_STAT iStat;      
            int           nMGZMaxSlot = cDEF.DM.MGZ[(int)EN_MGZ_ID.MGZ1]._iMaxSlot;


            Bitmap   bmp = new Bitmap(pPBox.Width, pPBox.Height);
            Graphics gr  = Graphics.FromImage(bmp);

            if (pPBox == null) return;

           //이미지 사이즈
            int iMainW   = pPBox.Size.Width ;
            int iMainH   = pPBox.Size.Height;

            //중심을 맞추기 위해서 사용  
            int iCOffW   = 5;
            int iCOffH   = 5;

            iStat   = m_iStat;
            sBColor = GetStatColor();
			brush   = new SolidBrush(sBColor);

            //sTemp2  = vDEF.STR_UNIT_STAT[(int)iStat] + "_" + m_iWhreMgzRow;
            if (m_iSlot >= 0)
            {
                //if (iStat>=0) sTemp2 = string.Format($"[{vDEF.STR_UNIT_STAT[(int)iStat]}] {m_iFromMgz} \r\n      /R:{m_iWhreMgzRow+1}");
                if (iStat>=0) sTemp2 = string.Format($"[{vDEF.STR_UNIT_STAT[(int)iStat]}] {m_iFromMgz} \r\n      /R:{nMGZMaxSlot - m_iSlot}");
                if (show)
                {
                    sTemp2 += string.Format($" /T:{(EN_ASM_ID)m_iTargerMC}");
                }

            }
            else sTemp2 = string.Format($"{vDEF.STR_UNIT_STAT[(int)iStat]}");

            if (DrawEillipse)
			{
				gr.FillEllipse(brush     , iCOffW, iCOffH, iMainW-(iCOffW*2), iMainH-(iCOffH*2));
				gr.DrawEllipse(Pens.Black, iCOffW, iCOffH, iMainW-(iCOffW*2), iMainH-(iCOffH*2));
			}
			else
			{
				gr.FillRectangle(brush     , iCOffW, iCOffH, iMainW-(iCOffW*2), iMainH-(iCOffH*2));
				gr.DrawRectangle(Pens.Black, iCOffW, iCOffH, iMainW-(iCOffW*2), iMainH-(iCOffH*2));     
			}

            FNC.DrawText   (ref gr, 10, 40, Color.Black, sTemp2, 10);
            pPBox.Image = bmp;

            
            if (gr != null) gr.Dispose();
        }

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(bool IsLoad)
        {
            //Local Var.
            string sPath;
            //Make Dir.
            FNC.CreateDirOnWork("SeqData");
            sPath = Application.StartupPath + "\\SeqData\\WaferMap" + Convert.ToString(0) + ".DAT";

            //File Open.
            int iFAccess = IsLoad ? (int)FileAccess.Read : (int)FileAccess.Write;
            FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, (FileAccess)iFAccess);

            //Read&Write.
            if(IsLoad) 
            {
                BinaryReader br = new BinaryReader(fp);
                if(br.PeekChar()<0) return;
                Load(br);
                br.Close();
                br = null;
            }   
            else 
            {
                BinaryWriter wr = new BinaryWriter(fp);
                Save(wr);
                wr.Close();
                wr = null;
            }
            fp = null;
        }
        //------------------------------------------------------------------------
        public void Load(BinaryReader br)
        {//UserSet - Wafer 변수 Load (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
            //Local Var.
           
            //
            m_sLotNo           = br.ReadString().Trim();
            m_sBarNo           = br.ReadString().Trim();
            m_sRFID            = br.ReadString().Trim();
            //m_sPanelID         = br.ReadString().Trim();

            //Spare                 
            m_bSpare1          = br.ReadBoolean();
            m_bSpare2          = br.ReadBoolean();

            m_iStat            = (EN_WAFER_STAT)br.ReadInt32();
            m_iWafDir          = (EN_MAP_DIR   )br.ReadInt32();
            m_iDispDir         = (EN_MAP_DIR   )br.ReadInt32();
            m_iTargerMC        = br.ReadInt32 ();
            m_iFromMgz         = (EN_MGZ_ID    )br.ReadInt32();

            //Spare
            m_iSpare1          = br.ReadInt32  ();                   
            m_iSpare2          = br.ReadInt32  ();                   
            //m_iWhreMgzRow      = br.ReadInt32 ();
            
            //Spare                                                 
            m_dSpare1          = br.ReadDouble ();                  
            m_dSpare2          = br.ReadDouble ();                  
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Save(BinaryWriter wr)
        { 

            //
            wr.Write(m_sLotNo  .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sBarNo  .PadRight(vDEF.MAX_STR_LEN, ' '));
            wr.Write(m_sRFID   .PadRight(vDEF.MAX_STR_LEN, ' '));
            //wr.Write(m_sPanelID.PadRight(vDEF.MAX_STR_LEN, ' '));

            //Spare              
            wr.Write(m_bSpare1        ); 
            wr.Write(m_bSpare2        );

            //
            wr.Write((int)m_iStat     );
            wr.Write((int)m_iWafDir   ); 
            wr.Write((int)m_iDispDir  ); 
            //wr.Write(m_iWhreMgzRow    );
            wr.Write(m_iTargerMC      );
            wr.Write((int)m_iFromMgz  ); 

            //Spare
            wr.Write(m_iSpare1        );                
            wr.Write(m_iSpare2        );                

            //Spare                                   
            wr.Write(m_dSpare1        );               
            wr.Write(m_dSpare2        );               
        }
    }
}