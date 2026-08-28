using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    public enum EN_LAMP_OPER  : int 
    {
        LampOff   ,
        LampOn    ,
        LampFlick
    };

    //Buzzer Operations.
    //===========================================================================
    public enum  EN_BUZZ_OPER  : int 
    {
        BuzzOff ,
        Buzz1   ,
        Buzz2   ,
        Buzz3   ,
        Buzz4   ,
        Buzz5
    };


    /***************************************************************************/
    /* Structures & Variables                                                  */
    /***************************************************************************/
    //Lamp & Buzzer Informations.
    //===========================================================================
    public struct _TLampInfo {
        public int iRed  ;
        public int iYel  ;
        public int iGrn  ;
        public int iBuzz ;
        public string sKindStr;
    };


    /***************************************************************************/
    /* Class: TLampBuzz                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TLampBuzz
    {
        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_FlickOnTimer   = new TOnDelayTimer();
        TOnDelayTimer m_FlickOffTimer  = new TOnDelayTimer();
        TOnDelayTimer m_FlickOnTimer2  = new TOnDelayTimer();
        TOnDelayTimer m_FlickOffTimer2 = new TOnDelayTimer();

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        _TLampInfo[]  m_LampInfo = new _TLampInfo[vDEF.MAX_LAMP_KIND];

        int  m_iTestStat ;
        bool m_bTest     ;
        bool m_bBuzzOff  ;
        bool m_bFlickLamp;
        bool m_bFlickBuzz;

        //Set Output
        EN_OUT_ID  m_iYLempRed ;
        EN_OUT_ID  m_iYLempYel ;
        EN_OUT_ID  m_iYLempGrn ;

        EN_OUT_ID  m_iYBuzz1   ;
        EN_OUT_ID  m_iYBuzz2   ;
        EN_OUT_ID  m_iYBuzz3   ;

        EN_LAMP_OPER loRed, loYellow, loGreen; 


        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        string[] m_sKindStr = new string[vDEF.MAX_LAMP_KIND];

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool _bBuzzOff
        {
            get { return m_bBuzzOff; }
            set { m_bBuzzOff = value; }
        }
        public bool _bTest
        {
            get { return m_bTest; }
            set { m_bTest = value; }
        }
        public int _iTestStat
        {
            get { return m_iTestStat; }
            set { m_iTestStat = value; }
        }
        public EN_OUT_ID _iYLempRed
        {
            set { m_iYLempRed = value; }
        }
        public EN_OUT_ID _iYLempYel
        {
            set { m_iYLempYel = value; }
        }
        public EN_OUT_ID _iYLempGrn
        {
            set { m_iYLempGrn = value; }
        }

        public EN_OUT_ID _iYBuzz1
        {
            set { m_iYBuzz1 = value; }
        }
        public EN_OUT_ID _iYBuzz2
        {
            set { m_iYBuzz2 = value; }
        }
        public EN_OUT_ID _iYBuzz3
        {
            set { m_iYBuzz3 = value; }
        }

        public EN_LAMP_OPER _loRed    => loRed   ;
        public EN_LAMP_OPER _loYellow => loYellow;
        public EN_LAMP_OPER _loGreen  => loGreen ;

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TLampBuzz()
        {
            for (int i = 0; i < vDEF.MAX_LAMP_KIND; i++)
            {
                m_LampInfo[i] = new _TLampInfo();
            }               
        }
        ~TLampBuzz() { }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init   ()
        {
            m_iTestStat = 5;
            m_bTest = false;

            m_iYLempRed = EN_OUT_ID.yNone;
            m_iYLempYel = EN_OUT_ID.yNone;
            m_iYLempGrn = EN_OUT_ID.yNone;

            m_iYBuzz1   = EN_OUT_ID.yNone;
            m_iYBuzz2   = EN_OUT_ID.yNone;
            m_iYBuzz3   = EN_OUT_ID.yNone;

            m_bBuzzOff = false;


            for (int i = 0; i < vDEF.MAX_LAMP_KIND; i++) m_sKindStr[i] = "";
        }
        //--------------------------------------------------------------------------
        public void  Reset  ()
        {

        }
        //Conv
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int     ConvStrToStat (string sLampBuzz, bool IsBuzz = false)      
        {
            int iStat;

            if (!IsBuzz)
            {
                iStat = (int)EN_LAMP_OPER.LampOff;

                if (sLampBuzz == "ON") iStat = (int)EN_LAMP_OPER.LampOn;
                if (sLampBuzz == "FLICKING") iStat = (int)EN_LAMP_OPER.LampFlick;
            }
            else
            {

                iStat = (int)EN_BUZZ_OPER.BuzzOff;

                if (sLampBuzz == "Buzzer 1") iStat = (int)EN_BUZZ_OPER.Buzz1;
                if (sLampBuzz == "Buzzer 2") iStat = (int)EN_BUZZ_OPER.Buzz2;
                if (sLampBuzz == "Buzzer 3") iStat = (int)EN_BUZZ_OPER.Buzz3;
                if (sLampBuzz == "Buzzer 4") iStat = (int)EN_BUZZ_OPER.Buzz4;
                if (sLampBuzz == "Buzzer 5") iStat = (int)EN_BUZZ_OPER.Buzz5;

            }
            return iStat;
        }
        //--------------------------------------------------------------------------
        public string  ConvStatToStr (int iStat, bool IsBuzz = false)
        {
            String sTemp;
            EN_LAMP_OPER iLampStat;
            EN_BUZZ_OPER iBuzzStat;
            if (!IsBuzz)
            {
                iLampStat = (EN_LAMP_OPER)iStat;
                sTemp = "OFF";
                switch (iLampStat)
                {
                    default: sTemp = "OFF"; break;
                    case EN_LAMP_OPER.LampOff  : sTemp = "OFF";      break;
                    case EN_LAMP_OPER.LampOn   : sTemp = "ON";       break;
                    case EN_LAMP_OPER.LampFlick: sTemp = "FLICKING"; break;
                }
            }
            else
            {
                iBuzzStat = (EN_BUZZ_OPER)iStat;
                sTemp = "Buzzer Off";
                switch (iBuzzStat)
                {
                    default: sTemp = "Buzzer Off"; break;
                    case EN_BUZZ_OPER.BuzzOff: sTemp = "Buzzer Off"; break;
                    case EN_BUZZ_OPER.Buzz1  : sTemp = "Buzzer 1";   break;
                    case EN_BUZZ_OPER.Buzz2  : sTemp = "Buzzer 2";   break;
                    case EN_BUZZ_OPER.Buzz3  : sTemp = "Buzzer 3";   break;
                }

            }
            return sTemp;

        }
        //--------------------------------------------------------------------------
        //Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Set
        public void  SetKindStr(string[] sKIND )
        {
            for (int i = 0; i < vDEF.MAX_LAMP_KIND; i++)
            {
                if (sKIND[i] != "") m_sKindStr[i] = sKIND[i];

            }
        }
        //--------------------------------------------------------------------------
        //Status
        public bool  IsSetLamp  ()
        {
            if(m_iYLempRed < 0) return false;
            if(m_iYLempYel < 0) return false;
            if(m_iYLempGrn < 0) return false;
            return true;;
        }
        //--------------------------------------------------------------------------
        public bool  IsSetBuzz  ()
        {
            if(m_iYBuzz1 < 0 &&
               m_iYBuzz2 < 0 &&
               m_iYBuzz3 < 0) return false;
            return true;
        }
        //--------------------------------------------------------------------------
        public void  BuzzOff    ()
        {
            m_bBuzzOff = true;
        }
        //--------------------------------------------------------------------------
        //Update
        public void  Update     (int iSeqStat)
        {
            //Local Var.
            int  iStat      = m_bTest ? m_iTestStat : iSeqStat;


            if (m_bFlickLamp)   { m_FlickOnTimer  .Clear(); if (m_FlickOffTimer.OnDelay ( m_bFlickLamp ,  500 )) m_bFlickLamp = false; }
            else                { m_FlickOffTimer .Clear(); if (m_FlickOnTimer .OnDelay (!m_bFlickLamp ,  500 )) m_bFlickLamp = true ; }

            if (m_bFlickBuzz)  { m_FlickOnTimer2 .Clear(); if (m_FlickOffTimer2.OnDelay( m_bFlickBuzz,  200 )) m_bFlickBuzz = false; }
            else               { m_FlickOffTimer2.Clear(); if (m_FlickOnTimer2 .OnDelay(!m_bFlickBuzz,  200 )) m_bFlickBuzz = true ; }


            if(iStat<0 || iStat>=vDEF.MAX_LAMP_KIND) return;


            UpdateLamp(m_iYLempRed, (EN_LAMP_OPER)m_LampInfo[iStat].iRed );
            UpdateLamp(m_iYLempYel, (EN_LAMP_OPER)m_LampInfo[iStat].iYel );
            UpdateLamp(m_iYLempGrn, (EN_LAMP_OPER)m_LampInfo[iStat].iGrn );
            UpdateBuzz(             (EN_BUZZ_OPER)m_LampInfo[iStat].iBuzz);

            //
            loRed    = (EN_LAMP_OPER)m_LampInfo[iStat].iRed ; 
            loYellow = (EN_LAMP_OPER)m_LampInfo[iStat].iYel ;
            loGreen  = (EN_LAMP_OPER)m_LampInfo[iStat].iGrn ;

        }
        //--------------------------------------------------------------------------
        public void  UpdateLamp (EN_OUT_ID iLamp, EN_LAMP_OPER iLampStat)
        {
            if(!IsSetLamp()) return;
            switch (iLampStat) 
            {
                default                    : cDEF.IO. sY(iLamp, false       ); break;
                case EN_LAMP_OPER.LampOff  : cDEF.IO. sY(iLamp, false       ); break;
                case EN_LAMP_OPER.LampOn   : cDEF.IO. sY(iLamp, true        ); break;
                case EN_LAMP_OPER.LampFlick: cDEF.IO. sY(iLamp, m_bFlickLamp); break;
            }
        }
        //--------------------------------------------------------------------------
        public void  UpdateBuzz (EN_BUZZ_OPER iBuzzStat)
        {
            bool isBuzzOn1  = false;
            bool isBuzzOn2  = false;
            bool isBuzzOn3  = false;

            if(!IsSetBuzz()) return;

            switch (iBuzzStat) {
                 default                  : isBuzzOn1 = false   ; isBuzzOn2 = false   ; isBuzzOn3 = false; break;
                 case EN_BUZZ_OPER.BuzzOff: isBuzzOn1 = false   ; isBuzzOn2 = false   ; isBuzzOn3 = false; break;
                 case EN_BUZZ_OPER.Buzz1  : isBuzzOn1 = true    ; isBuzzOn2 = false   ; isBuzzOn3 = false; break;
                 case EN_BUZZ_OPER.Buzz2  : isBuzzOn1 = false   ; isBuzzOn2 = true    ; isBuzzOn3 = false; break;
                 case EN_BUZZ_OPER.Buzz3  : isBuzzOn1 = false   ; isBuzzOn2 = false   ; isBuzzOn3 = true ; break;
                 case EN_BUZZ_OPER.Buzz4  : if(m_bFlickBuzz)  { isBuzzOn1 = true    ; isBuzzOn2 = false   ; isBuzzOn3 = false; }
                                             else             { isBuzzOn1 = false   ; isBuzzOn2 = true    ; isBuzzOn3 = false; } break;
                 case EN_BUZZ_OPER.Buzz5  : if(m_bFlickBuzz)  { isBuzzOn1 = false   ; isBuzzOn2 = true    ; isBuzzOn3 = false; }
                                             else             { isBuzzOn1 = false   ; isBuzzOn2 = false   ; isBuzzOn3 = true ; } break;
                 }

            //Buzzer Control.
            if(m_iYBuzz1>=0) cDEF.IO. sY(m_iYBuzz1,isBuzzOn1 && !m_bBuzzOff);
            if(m_iYBuzz2>=0) cDEF.IO. sY(m_iYBuzz2,isBuzzOn2 && !m_bBuzzOff);
            if(m_iYBuzz3>=0) cDEF.IO. sY(m_iYBuzz3,isBuzzOn3 && !m_bBuzzOff);
        }
        //--------------------------------------------------------------------------
        public void UpdateGrid(bool ToTable, ref System.Windows.Forms.DataGridView Grid)
        {
            if(ToTable)
            {

	            DataGridViewComboBoxColumn comboBoxRed = new DataGridViewComboBoxColumn(); // 콤보박스 추가 
	            comboBoxRed.HeaderText = "RED";
	            comboBoxRed.Name = "combo";
	            comboBoxRed.Items.AddRange("OFF", "ON", "FLICKING");

	            DataGridViewComboBoxColumn comboBoxYel = new DataGridViewComboBoxColumn(); // 콤보박스 추가 
	            comboBoxYel.HeaderText = "YELLOW";
	            comboBoxYel.Name = "combo";
	            comboBoxYel.Items.AddRange("OFF", "ON", "FLICKING");

	            DataGridViewComboBoxColumn comboBoxGrn = new DataGridViewComboBoxColumn(); // 콤보박스 추가 
	            comboBoxGrn.HeaderText = "GREEN";
	            comboBoxGrn.Name = "combo";
	            comboBoxGrn.Items.AddRange("OFF", "ON", "FLICKING");

	            DataGridViewComboBoxColumn comboBoxBuzz = new DataGridViewComboBoxColumn(); // 콤보박스 추가 
	            comboBoxBuzz.HeaderText = "BUZZER";
	            comboBoxBuzz.Name = "combo";
	            comboBoxBuzz.Items.AddRange("Buzzer Off", "Buzzer 1", "Buzzer 2", "Buzzer 3", "Buzzer 4", "Buzzer 5");

                DataGridViewButtonColumn btnTest = new DataGridViewButtonColumn();  //버튼 추가
	            btnTest.HeaderText = "";
	            btnTest.Name       = "Test";
                btnTest.FlatStyle  =  FlatStyle.Popup;

                Grid.Dock = System.Windows.Forms.DockStyle.Fill;
                FNC.SetGridStyle(ref Grid, 50, false);
                Grid.BackgroundColor = FRM.GetGridBackColor(); //Color.FromArgb(66, 72, 88);
                //
                Grid.Columns.Add("KIND"  , "KIND"   );
                Grid.Columns.Add(comboBoxRed  );
                Grid.Columns.Add(comboBoxYel  );
                Grid.Columns.Add(comboBoxGrn  );
                Grid.Columns.Add(comboBoxBuzz );    
                Grid.Columns.Add(btnTest      );

                for(int i=0; i<Grid.ColumnCount; i++) 
                {
                    Grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    Grid.Columns[i].Width        = (Grid.Width-20)/6;
                    Grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                for(int i=0; i<vDEF.MAX_LAMP_KIND;i++) 
                {
                     if(m_sKindStr[i] == "") continue;

                     Grid.Rows.Add(m_sKindStr[i], ConvStatToStr(m_LampInfo[i].iRed       ), 
                                                  ConvStatToStr(m_LampInfo[i].iYel       ),  
                                                  ConvStatToStr(m_LampInfo[i].iGrn       ),
                                                  ConvStatToStr(m_LampInfo[i].iBuzz, true), 
                                                  "TEST" );                           
                }

                //
                //cDEF.POSN.SetGridFont(ref Grid);

            }
            else
            {   
                for(int i=0; i<Grid.RowCount;i++) 
                {
                    m_LampInfo[i].iRed   = ConvStrToStat((string)Grid[1,i].Value);
                    m_LampInfo[i].iYel   = ConvStrToStat((string)Grid[2,i].Value);
                    m_LampInfo[i].iGrn   = ConvStrToStat((string)Grid[3,i].Value);
                    m_LampInfo[i].iBuzz  = ConvStrToStat((string)Grid[4,i].Value, true);
                }
                Load(false);
            }
            Grid.Visible   = true;
        }
        //--------------------------------------------------------------------------
        //Loading Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Load(bool IsLoad)
        {
            string sPath;
            string sFile = "LampBuzz";
            string sSection;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("System");
            sPath = Application.StartupPath + "\\System\\" + sFile + ".INI";

            if (IsLoad)
            {
                for (int i = 0; i < vDEF.MAX_LAMP_KIND; i++)
                {
                    sSection = string.Format("LAMP_BUZZ{0}", i + 1);

                    ini.Load(sPath, sSection, "Red   ", out m_LampInfo[i].iRed );
                    ini.Load(sPath, sSection, "Yellow", out m_LampInfo[i].iYel );
                    ini.Load(sPath, sSection, "Green ", out m_LampInfo[i].iGrn );
                    ini.Load(sPath, sSection, "Buzzer", out m_LampInfo[i].iBuzz);
                }
            }
            else
            {
                for (int i = 0; i < vDEF.MAX_LAMP_KIND; i++)
                {
                    sSection = string.Format("LAMP_BUZZ{0}", i + 1);

                    ini.Save(sPath, sSection, "Red   ", m_LampInfo[i].iRed );
                    ini.Save(sPath, sSection, "Yellow", m_LampInfo[i].iYel );
                    ini.Save(sPath, sSection, "Green ", m_LampInfo[i].iGrn );
                    ini.Save(sPath, sSection, "Buzzer", m_LampInfo[i].iBuzz);
                }
            }
            ini = null;

        }
    }
}
