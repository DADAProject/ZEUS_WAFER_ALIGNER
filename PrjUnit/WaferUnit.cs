using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    public class TUnit
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        EN_WAFER_STAT m_iStat                      ;
        int           m_iSlot                      ;
        int           m_iGoodQty                   ;
        int           m_iFailQty                   ;
        
        string        m_sId                        ;
        string        m_sLotNo                     ;
        string        m_sUnitID                    ;

        //Spare Var.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
        bool   m_bSpare1, m_bSpare2;
        int    m_iSpare1, m_iSpare2;
		double m_dSpare1, m_dSpare2;


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EN_WAFER_STAT _iStat      { get { return m_iStat     ; } set { m_iStat       = value; }}
        public int           _iSlot      { get { return m_iSlot     ; } set { m_iSlot       = value; }}
        public string        _sId        { get { return m_sId       ; } set { m_sId         = value; }}
        public string        _sLotNo     { get { return m_sLotNo    ; } set { m_sLotNo      = value; }}
        public string        _sUnitID    { get { return m_sUnitID   ; } set { m_sUnitID     = value; }}                           
        public int           _iGoodQty   { get { return m_iGoodQty  ; } set { m_iGoodQty    = value; }}
        public int           _iFailQty   { get { return m_iFailQty  ; } set { m_iFailQty    = value; }}

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TUnit()
        {
            Init();
        }
        ~TUnit() { }

        public TUnit Copy()
        {
			return FNC.DeepClone(this) as TUnit;
            //return this.MemberwiseClone();
        }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Init()
        {
             m_sId      = "";
             m_sLotNo   = "";
             m_sUnitID  = "";


             //Chip Staus.
            m_iStat     = EN_WAFER_STAT.None;
            m_iSlot     = 0;
            m_iGoodQty  = 0;
            m_iFailQty  = 0;
                      
            //m_bRqSply   = false;
            //m_bRqEjct   = false;
			 
        }

        //Set Unit.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void SetTo(EN_WAFER_STAT Stat)
        {
            m_iStat = Stat;

            //if (Stat == EN_WAFER_STAT.None) Init();
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
    
            if      (m_iStat == EN_WAFER_STAT.None       ) iBinColor = Color.White  ; 
            else if (m_iStat == EN_WAFER_STAT.Empty      ) iBinColor = Color.Silver ; 
            else if (m_iStat == EN_WAFER_STAT.Mask       ) iBinColor = Color.SkyBlue; 
            else if (m_iStat == EN_WAFER_STAT.Mount      ) iBinColor = Color.Aqua   ; 
            else if (m_iStat == EN_WAFER_STAT.Aligned    ) iBinColor = Color.Brown  ; 
            else if (m_iStat == EN_WAFER_STAT.Skip       ) iBinColor = Color.Gray   ; 
            else if (m_iStat == EN_WAFER_STAT.Fnsh       ) iBinColor = Color.Purple ; 
            else if (m_iStat == EN_WAFER_STAT.Work       ) iBinColor = Color.Lime   ;             
            else if (m_iStat == EN_WAFER_STAT.Wait       ) iBinColor = Color.Yellow ;  
            else if (m_iStat == EN_WAFER_STAT.Fail       ) iBinColor = Color.Red    ; 
            else                                           iBinColor = Color.Black  ; 
            return iBinColor;
        }
        //------------------------------------------------------------------------
        public Color GetLineColor()
        {//UserSet - Chip OPTIN DISPLAY 색깔 처리 
            return Color.Black;
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(BinaryReader br)
        {//UserSet - Unit 변수 Load (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)
            
            m_sId          = br.ReadString().Trim();
            m_sLotNo       = br.ReadString().Trim();
            m_sUnitID      = br.ReadString().Trim();

			m_bSpare1      = br.ReadBoolean();
            m_bSpare2      = br.ReadBoolean();
              
            m_iStat        = (EN_WAFER_STAT)br.ReadInt32();
            m_iSlot        = br.ReadInt32();
            m_iGoodQty     = br.ReadInt32();
            m_iFailQty     = br.ReadInt32();

			m_iSpare1      = br.ReadInt32();
			m_iSpare2      = br.ReadInt32();

            //
            m_dSpare1      = br.ReadDouble();
            m_dSpare2      = br.ReadDouble();
            
        }
        //------------------------------------------------------------------------
        public void Save(BinaryWriter wr)
        {//UserSet - Unit 변수 Save (절대 추가/삭제 하지 마시오 - 이진파일 : Spare변수와 교체)

            wr.Write(m_sId     .PadRight(vDEF.MAX_STR_LEN, ' ')); 
            wr.Write(m_sLotNo  .PadRight(vDEF.MAX_STR_LEN, ' ')); 
            wr.Write(m_sUnitID .PadRight(vDEF.MAX_STR_LEN, ' ')); 

             
            //
            //wr.Write(m_bRqSply       );
            //wr.Write(m_bRqEjct       );
            wr.Write(m_bSpare1       );                         
            wr.Write(m_bSpare2       );


            //
            //wr.Write((int)m_ID       );
            wr.Write((int)m_iStat    );
            wr.Write(m_iSlot         );                            
            wr.Write(m_iGoodQty      );
            wr.Write(m_iFailQty      );

			wr.Write(m_iSpare1       );
			wr.Write(m_iSpare2       );

            //
            wr.Write(m_dSpare1       );
            wr.Write(m_dSpare2       );                                 
        }
    }
}
