using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace eMachine
{


    //The grade of error.
    //===========================================================================
    public enum EN_ERR_GRADE  : int
    {
        Display,
        Warning,
        Error
    };

    //The grade of error.
    //===========================================================================
    public enum EN_ERR_KIND  : int
    {
        Machine,
        Material,
        Human,
        Method
    };

    //Error enum
    //===========================================================================
    public enum EN_ERR_COMD  : int
    {
        Retry,
        Skip,
        MaskTop,
        MaskBtm,
        NoIC,
        ShiftToBtm,
        ShiftToTop
    };

    //Kind of Tool Error
    //===========================================================================
    public enum EN_TOOL_ERR  : int
    {
        VacErr,            //Vacuum Error.
        UnknowPKGErr,      //알 수 없는 PKG 발견
        LossPKGErr,        //Chip 사라짐
        PlceValveErr,      //Place Down Valve Error
        PickValveErr       //Pick Up Valve Error.

    };


    /***************************************************************************/
    /* Class: TError                                                           */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TError
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */



        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public int           m_iGrade    ; //Data.
		public int           m_iPart     ;
		public int           m_iKind     ;
		public bool          m_bHoldErr  ;
		public bool          m_bSendErr  ;
		public int           m_iAplyMTBF ;
		public string        m_sName     ;
        public string        m_sSoluttion;
        public string        m_sCause    ;
        public DateTime      m_tSetTime  ; //Error Time.
		public DateTime      m_tResetTime;
		public bool          m_bOn       ; //Flags.
		public bool          m_bUpdate   ;
		public bool          m_bOnAtRun  ;
		public bool          m_bXSend    ;
		
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TError()
        {
        }
        ~TError() { }


        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init () { m_bOn = false; m_bOnAtRun = false; m_iGrade = (int)EN_ERR_GRADE.Error; m_bXSend = false; }
        public void  Reset() { m_bOn = false; m_bOnAtRun = false; m_bXSend = false; }

        public bool  IsGradeErr ()
        {
            return m_iGrade == (int)EN_ERR_GRADE.Error; //|| m_iGrade == (int)EN_ERR_GRADE.Display;
        }
        public bool IsGradeDisp()
        {
            return m_iGrade == (int)EN_ERR_GRADE.Display;
        }
        public bool IsGradeWarn()
        {
            return m_iGrade == (int)EN_ERR_GRADE.Warning;
        }

        //Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


    }

    public class TVacErr
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */



        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool bErr    ;
        public int  iSetCnt ;
        public int  iErrCnt ;
        public int  iTop    ;
        public int  iBtm    ;
        public int  iTRow   ;
        public int  iTCol   ;
        public int  iCRow   ;
        public int  iCCol   ;
		public int  iNozl   ;
        public int  iNozlR  ;
        public int  iNozlC  ;
        public int  iBtmKind;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TVacErr()
        {
            bErr       = false;
            iSetCnt    = 0;
            iErrCnt    = 0;
            iTop       = 0;
            iBtm       = 0;
            iTRow      = 0;
            iTCol      = 0;
            iCRow      = 0;
            iCCol      = 0;
			iNozl      = 0;
            iNozlR     = 0;
            iNozlC     = 0;
            iBtmKind   = 0;

        }                                                     
        ~TVacErr() { }

       public void  Retry() { Reset(); }
	   public void  Skip () {
			//if(iBtm<0 || iBtm>=(int)EN_TRAY_ID.EndOfId) return;
			Reset();
			}
		public void  NoIC() {
			//if(iBtm<0 || iBtm>=(int)EN_TRAY_ID.EndOfId) return;
			Reset();
			}

		public void  MaskTop ()  {
            Reset();
            }

        public void  MaskBtm ()  {
			Reset();
            }
        public void  ShiftToTool() {
			Reset();
			}
		public void  ShiftToBtm() {
			Reset();
			}


        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init () { bErr = false; iErrCnt = 0; iBtm = -1; iTop = -1; iTRow = -1; iTCol = -1; iCRow = -1; iCCol = -1; iNozl = -1; iNozlR = -1; iNozlC = -1;}
        public void  Reset() { bErr = false; iErrCnt = 0; iBtm = -1; iTop = -1; iTRow = -1; iTCol = -1; iCRow = -1; iCCol = -1; iNozl = -1; iNozlR = -1; iNozlC = -1;}

        //Apply error processing by user selection.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Apply(EN_ERR_COMD Cmd) 
        {
            if (Cmd == EN_ERR_COMD.Retry      ) Retry      ();
            if (Cmd == EN_ERR_COMD.Skip       ) Skip       ();
            if (Cmd == EN_ERR_COMD.NoIC       ) NoIC       ();
            if (Cmd == EN_ERR_COMD.MaskTop    ) MaskTop    ();
            if (Cmd == EN_ERR_COMD.MaskBtm    ) MaskBtm    ();
            if (Cmd == EN_ERR_COMD.ShiftToBtm ) ShiftToBtm ();
            if (Cmd == EN_ERR_COMD.ShiftToTop ) ShiftToTool();
        }
    }


    public class TErrProc
    {

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        DateTime m_tResetTime; //Last Reset Time.
        DateTime m_tJamTime  ;
        DateTime m_tStartTime;
        int      m_iLastErr  ;
        int      m_iReportErr;

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
               //bool[] m_bNoIC = new bool[(int)EN_TRAY_ID.EndOfId] ;               //NoIC Mode Flag.
        public  bool   m_bNoVNG         ; //No Vision Check.
        private bool   m_bHasErr        ; //Flag.
        public  bool   m_bHasWrn        ;
        public  bool   m_bHasDsp        ;
        public  bool   m_bUpdatedErrForm;
        public  bool   m_bVacErr        ;
        public  bool   m_bHoldErr       ;
        public  bool   m_bReqHoldClear  ;
        public  bool   m_bHasErrAtRun   ;
        public  bool   m_bRqCloseForm   ;
        public  bool   m_bRqClsList     ;
        public  bool   m_bNeedSave      ; //for Save

        public TError[]   Err      = new TError [vDEF.MAX_ERR ];
		public TVacErr[,] ErrVac   = new TVacErr[(int)EN_WAF_ID.EndOfId, vDEF.MAX_NOZL];  //Vacuum Error.


        //Object.
		

        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TError this[int iNo]
        {
            get { 
                if (iNo < 0 || iNo>=vDEF.MAX_ERR) return null;
                return Err[iNo]; 
            }
        }

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool _bHasErr         { get { return m_bHasErr;         } set { m_bHasErr         = value; }}
        public bool _bHasWrn         { get { return m_bHasWrn;         } set { m_bHasWrn         = value; }}
        public bool _bHasDsp         { get { return m_bHasDsp;         } set { m_bHasDsp         = value; }}
        public bool _bUpdatedErrForm { get { return m_bUpdatedErrForm; } set { m_bUpdatedErrForm = value; }}
        public int  _iLastErr        { get { return m_iLastErr;        } set { m_iLastErr        = value; }}
        public bool _bRqCloseForm    { get { return m_bRqCloseForm;    } set { m_bRqCloseForm    = value; }}

        public bool _bNeedSave => this.m_bNeedSave;

        //public bool _NoIC(int Whre          )  { if ((Whre < 0) || (Whre >= (int)EN_TRAY_ID.EndOfId)) return false; return m_bNoIC[Whre]; }
        //public void _NoIC(int Whre, bool Set)  { if ((Whre < 0) || (Whre >= (int)EN_TRAY_ID.EndOfId)) return      ; m_bNoIC[Whre] = Set;  }
        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TErrProc()
        {
            for (int i = 0; i < vDEF.MAX_ERR; i++)
                Err[i] = new TError();

            for (int i = 0; i < (int)EN_WAF_ID.EndOfId; i++)
                for (int j = 0; j < vDEF.MAX_NOZL; j++)
                ErrVac[i,j] = new TVacErr(); 
            Init();
        }
        ~TErrProc() { }


        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Init(  )
        {

            //Init. TError.
            for (int i = 0 ; i < vDEF.MAX_ERR ; i++) Err[i].Init();
            
            //Init. Update Form Flag.
            m_bUpdatedErrForm = false;

            //Init. Last Error No.
            m_iLastErr  = -1;

            m_bNeedSave = false;
        }
        //------------------------------------------------------------------------
        public void  Init(int No)
        {
            this[No].Init();
        }
        //Set & Clear Error.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  Clear  (              )  
        {
	        //Last Err Info.
	        int iLastErr = GetLastErrNo(true);   //Running중 On된 에러.

	        //Hold Jam.
	        if (m_bHoldErr)
	        {
		        if (cDEF.FM.m_iCrntLevel < (int)EN_LOGIN.Master)
		        {
			        if (!FRM.Login.Visible) m_bReqHoldClear = true;
			        return;
		        }
		        m_bReqHoldClear = false;
		        m_bHoldErr      = false;
	        }
            
            //
            if(m_iReportErr > 0 && m_iReportErr < vDEF.MAX_ERR) 
            {
                //Trace Log.
              //cDEF.LOG.Trace($"CLEAR ERROR [{iLastErr:0000}] - {GetName(iLastErr)}");
                cDEF.LOG.Trace($"CLEAR ERROR [{m_iReportErr:0000}] - {GetName(m_iReportErr)}"); //JUNG/221215
                
                //Write JAM to SPC.
                cDEF.SPC.InsDbJam(m_iReportErr);
                //
                //cDEF.GEM.GoSendALARM(m_iReportErr, false);
                m_iReportErr = -1;
            }

	        //Set Reset Time.
	        m_tResetTime = DateTime.Now;

	        //Reset Error.
	        m_bHasErr      = false;
	        m_bHasWrn      = false;
	        m_bHasDsp      = false;
	        m_bHasErrAtRun = false;            
            m_iLastErr     = -1;

	        //Error Clear.
	        for (int i = 0 ; i < vDEF.MAX_ERR; i++) { Clear(i  , true ); }

	        //Vac Err Clear.
            for (int t = 0; t < (int)EN_TOOL_ID.EndOfId; t++)
            {
                for (int n = 0; n < vDEF.MAX_NOZL; n++)
                {
                    ErrVac[t,n].Reset();
			    }
		    }

	        //Reset Buzz Flag.
	        cDEF.LampBuzz.Reset();

	        //Clear Error List.
            m_bRqClsList = true;
	        //Close Error Form.
            m_bRqCloseForm = true; 
        }
        //------------------------------------------------------------------------
        public void Clear(int No, bool IncUpdate = false)
        {
            if (No < 0 || No >= vDEF.MAX_ERR) return;

            if (Err[No].m_bOn)
            {
                //Reset Error.
                Err[No].Reset();
                //Set clear time.
                Err[No].m_tResetTime = DateTime.Now;
                m_bNoVNG = false;
                //Reset update flag.
                if (IncUpdate) Err[No].m_bUpdate = false;
            }
        }
        //------------------------------------------------------------------------
        public void SetErr(EN_ERR_LIST No)
        {
            SetErr((int)No);
        }
        //------------------------------------------------------------------------
        public void  SetErr (int  No           )
        {
            //Check No. Error.
            if (No < 0            ) return;
            if (No >= vDEF.MAX_ERR) return;

            //Check No Define Error.
            if (No == -1          ) return;

            //Check Already Flag.
            if (No < 0 || No >= vDEF.MAX_ERR) return;
            if (Err[No].m_bOn               ) return;

            //Set On flag.
            Err[No].m_bOn      = true;
            Err[No].m_tSetTime = DateTime.Now;
            Err[No].m_bOnAtRun = cDEF.SEQ._bRun && !cDEF.SEQ._bLtStop;

            //Set Grade.
            if (Err[No].m_iGrade == (int)EN_ERR_GRADE.Error  )
            {
                m_bHasErr = true;
                if (Err[No].m_bOnAtRun) m_bHasErrAtRun = true;
                if (Err[No].m_bHoldErr) m_bHoldErr     = true;
            }
            else if (Err[No].m_iGrade == (int)EN_ERR_GRADE.Warning) m_bHasWrn = true;
            else if (Err[No].m_iGrade == (int)EN_ERR_GRADE.Display) m_bHasDsp = true;  // m_bHasErr = true; if (Err[No].m_bOnAtRun) m_bHasErrAtRun = true; }

            //Save LastErr.
            if (!Err[No].IsGradeErr()) return;

            //Jam Trace는 무조건 Write
            cDEF.LOG.JamTrace(No, Err[No].m_tSetTime, GetName(No), Err[No].m_iPart, Err[No].m_iKind);

            if ( IsChkSameErr  (No)) return;
            if (!CheckWriteErr (No)) return;
            if (!Err[No].m_bOnAtRun) return;
                        
            //Set Hold Jam.
            if (Err[No].m_bHoldErr) m_bHoldErr = true;
            m_tJamTime = Err[No].m_tSetTime;

            m_iReportErr = No;
            cDEF.LOG.Trace   ($"SET ERROR [{No:0000}] {GetName(No)}");
            cDEF.LOG.SeqTrace($"SET ERROR [{No:0000}] {GetName(No)}");

            //cDEF.COMASM.CmdC605_RobotStatusReport("3"); //Error

        }
        //------------------------------------------------------------------------\
        public bool SetErr(EN_ERR_LIST No, bool bCon)
        {
            return SetErr((int)No, true);
        }
        //------------------------------------------------------------------------
        public bool  SetErr (int  No , bool bCon)
        {
            //Check No. Error.
            if (No <  0           ) return false;
            if (No >= vDEF.MAX_ERR) return false;

            //Check No Define Error.
            if (No < 0            ) return false;

            //Set Error.
            //에러는 이전에 동일한 에러가 발생되어 있지 않아야 한다.
            if (Err[No].m_iGrade == (int)EN_ERR_GRADE.Display) 
            { 
                //if (!Err[No].m_bOn && bCon) SetErr(No);     
				if (bCon) SetErr(No);
				else      Clear (No);
            }
            else if (Err[No].m_iGrade == (int)EN_ERR_GRADE.Warning) 
            {
                if (bCon) SetErr(No); 
                else      Clear (No);      
            }
            else if (Err[No].m_iGrade == (int)EN_ERR_GRADE.Error) 
            { 
                if (!Err[No].m_bOn && bCon) SetErr(No);  
            } 

            //Return Error Status.
            if (Err[No].m_bOn && bCon) bCon = true;
            return bCon;
        }
        public bool  IsErr  (int  No           )
        {
            if (No < 0 || No >= vDEF.MAX_ERR) return false;
            return Err[No].m_bOn;
        }


        public bool IsChkSameErr(int No)
        {
            //Check.
            if (No < vDEF.MAX_WARN) return false;

            if (cDEF.FM.EngrOptn.iLastErrTime < 180) cDEF.FM.EngrOptn.iLastErrTime = 180;

            //Cal Clear Time.
            TimeSpan SapnTime = m_tJamTime - m_tStartTime;


            //Check Same Err.
            if(No != m_iLastErr) return false;

            if (SapnTime.Seconds < cDEF.FM.EngrOptn.iLastErrTime) return true;

            //No Same Error.
            return false;
        }
   
        //Inspect Error.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool  HasError()
        {
            //Local Var.
            bool isErr      = false;
            bool isWrn      = false;
            bool isDsp      = false;
            bool isErrAtRun = false;
            
            //Has Error.
            for (int i = 0 ; i < vDEF.MAX_ERR ; i++) 
            {
                     if (Err[i].m_bOn && (Err[i].m_iGrade == (int)EN_ERR_GRADE.Error  )) { isErr = true; if (Err[i].m_bOnAtRun) isErrAtRun = true; }
                else if (Err[i].m_bOn && (Err[i].m_iGrade == (int)EN_ERR_GRADE.Warning)) isWrn = true;
                else if (Err[i].m_bOn && (Err[i].m_iGrade == (int)EN_ERR_GRADE.Display)) isDsp = true;  // isErr = true;
            }
            
            m_bHasErr      = isErr     ;
            m_bHasWrn      = isWrn     ;
            m_bHasDsp      = isDsp     ;
            m_bHasErrAtRun = isErrAtRun;

            return m_bHasErr;
        }
        //------------------------------------------------------------------------
        public int   GetLastErrNo    (bool ChkOnAtRun = false      )
        {
            int   No = -1;
            bool isGradeErr;
            for (int i = vDEF.MAX_ERR-1; i>=0; i--) 
            {
                //isGradeErr = Err[i].IsGradeErr();
                if (ChkOnAtRun) { if (Err[i].m_bOn /*&& isGradeErr*/ && Err[i].m_bOnAtRun) return i;}
                else            { if (Err[i].m_bOn /*&& isGradeErr*/                     ) 
                        return i;}

            }

            return No;

        }
        //--------------------------------------------------------------------------
		public bool  CheckWriteErr   (int ErrNo                    )
        {
            return true;
        }


        //Error Data.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public void         SetGrade      (int No , int Grade             ) { Err[No].m_iGrade    = Grade                 ; }
		public void         SetPart       (int No , int        Part       ) { Err[No].m_iPart     = Part                  ; }
		public void         SetKind       (int No , int        Kind       ) { Err[No].m_iKind     = Kind                  ; }
		public void         SetHoldErr    (int No , bool       HoldErr    ) { Err[No].m_bHoldErr  = HoldErr               ; }
		public void         SetSendErr    (int No , bool       SendErr    ) { Err[No].m_bSendErr  = SendErr               ; } 
		public void         SetName       (int No , string Name           ) { Err[No].m_sName     = Name                  ; }
        
        public bool         GetSendErr    (int No                         ) { if (this[No]==null) return false; return this[No].m_bSendErr                   ; }
        public int          GetGrade      (int No                         ) { if (this[No]==null) return 0    ; return this[No].m_iGrade                     ; }
		public int          GetPart       (int No                         ) { if (this[No]==null) return 0    ; return this[No].m_iPart                      ; }
		public int          GetKind       (int No                         ) { if (this[No]==null) return 0    ; return this[No].m_iKind                      ; }

        public bool         GetHoldErr    (int No) { if (this[No]==null) return false; return this[No].m_bHoldErr; }
		public string       GetName       (int No) { if(No<0 || No>=vDEF.MAX_ERR) return "NONE"; if (this[No]==null) return "NONE"   ; return this[No].m_sName                      ; }
		public string       GetSetTime    (int No) { if (this[No]==null) return ""   ; return this[No].m_tSetTime  .ToString()      ; }
		public string       GetResetTime  (int No) { if (this[No]==null) return ""   ; return this[No].m_tResetTime.ToString()      ; }
		public DateTime     GetSetTimeDB  (int No) { if (this[No]==null) return DateTime.Now; return this[No].m_tSetTime                   ; }
		public DateTime     GetResetTimeDB(int No) { if (this[No]==null) return DateTime.Now; return this[No].m_tResetTime                 ; }


		public string   GetCause     (int No)
        {
            if (this[No]==null) return ""   ;

            //String sPath;
            //String sFile = "Error";
            //String sSection;
            //String sData   ;
            //
            //TIniUnit ini = new TIniUnit();
            //
            ////Make Dir.
            //FNC.CreateDirOnWork("Error");
            //sPath = Application.StartupPath + "\\Error\\" + sFile + ".INI";
            //
            //sSection = string.Format("ERR{0,4:0000}", No);
            //ini.Load(sPath, "CAUSE   ", sSection, out sData  );
            //sData = ini.GetLineStringFrINI(sData);
            
            return cDEF.EPU[No].m_sCause; 
        }

 		public string   GetSolution  (int No)
        {
            if (this[No]==null) return ""   ;
            //String sPath;
            //String sFile = "Error";
            //String sSection;
            //TIniUnit ini = new TIniUnit();
            //String sData   ;
            //
            ////Make Dir.
            //FNC.CreateDirOnWork("Error");
            //sPath = Application.StartupPath + "\\Error\\" + sFile + ".INI";
            //
            //sSection = string.Format("ERR{0,4:0000}", No);
            //ini.Load(sPath, "SOLUTION   ", sSection, out sData  );
            //sData = ini.GetLineStringFrINI(sData);
            //return sData;
            return cDEF.EPU[No].m_sSoluttion;

        }

		public void  SetCause     (int No, String sData)
        {
            if (this[No]==null) return;

            String sPath;
            String sFile = "Error";
            String sSection;

            TIniUnit ini = new TIniUnit();

            sData = ini.SetLineStringToINI(sData);
            //Make Dir.
            FNC.CreateDirOnWork("Error");
            sPath = Application.StartupPath + "\\Error\\" + sFile + ".INI";

            sSection = string.Format("ERR{0,4:0000}", No);
            ini.Save(sPath, "CAUSE   ", sSection, sData  );
        }

 		public void  SetSolution  (int No, String sData)
        {
            if (this[No]==null) return;
            String sPath;
            String sFile = "Error";
            String sSection;
            TIniUnit ini = new TIniUnit();

            sData = ini.SetLineStringToINI(sData);

            //Make Dir.
            FNC.CreateDirOnWork("Error");
            sPath = Application.StartupPath + "\\Error\\" + sFile + ".INI";

            sSection = string.Format("ERR{0,4:0000}", No);
            ini.Save(sPath, "SOLUTION   ", sSection, sData  );
        }   



		//Error Display.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		public bool   GetPicture     (int No          , ref System.Windows.Forms.PictureBox PBox)
        {
            //Check Error.
            if (PBox == null) return false;
            if ((No < 0) || (No >= vDEF.MAX_ERR)) return false;

            //Local Var.
            String FN1;
            String FN2;
            String FN3;

            //Set Path.
            FN1 = Application.StartupPath + "\\Error\\Pictures\\" + string.Format("E{0,4:0000}.JPG" , No);
            FN2 = Application.StartupPath + "\\Error\\Pictures\\" + string.Format("E{0,4:0000}.gif" , No);
            FN3 = Application.StartupPath + "\\Error\\Pictures\\" + string.Format("E{0,4:0000}.PNG" , No);

            FNC.CreateDirOnWork("Error");
            FNC.CreateDirOnWork("Error\\Pictures");


            //File Open.
            if      (FNC.FileExists(FN2))
            {
                PBox.Image = Bitmap.FromFile(FN2);
                return true;
            }
            else if (FNC.FileExists(FN1))
            {
                PBox.Image = Bitmap.FromFile(FN1);
                return true;
            }
            else if (FNC.FileExists(FN3))
            {
                PBox.Image = Bitmap.FromFile(FN3);
                return true;
            }
            else
            {
                FN1 = Application.StartupPath + "\\Error\\Pictures\\" + "No_not.png";
                PBox.Image = Bitmap.FromFile(FN1);
            }

            if(GetPicturePart(Err[No].m_iPart+1, ref PBox))
            {
                return true;
            }    
            return false;
        }
        //------------------------------------------------------------------------
		public bool   GetPicturePart     (int iPart          , ref System.Windows.Forms.PictureBox PBox)
        {
            //Check Error.
            if (PBox == null) return false;
            if ((iPart < 0) || (iPart > (int)EN_SEQ_ID.SYS)) return false;

            //Local Var.
            String FN1, FN2;

            //Set Path.
            FN1 = Application.StartupPath + "\\Error\\Pictures\\" + string.Format("PART{0,2:00}.BMP", iPart);
            FN2 = Application.StartupPath + "\\Error\\Pictures\\PART00.BMP";


            FNC.CreateDirOnWork("Error");
            FNC.CreateDirOnWork("Error\\Pictures");

            if (FNC.FileExists(FN1))
            {
                PBox.Image = Bitmap.FromFile(FN1);
                return true;
            }
            if (FNC.FileExists(FN2))
            {
                PBox.Image = Bitmap.FromFile(FN2);
                return true;
            }
            return false;
        }

        //------------------------------------------------------------------------
        public void  UpdateErr()
        {
            //Local Var.
            int  iLastErr    ;
            int  iSubFormSel ;   //Sub Error Form. (Vacuum Error , Disapperar Chip)
            int  iSubFormKind;
            bool isNeedSubFrm = IsNeedSubForm(out iSubFormSel, out iSubFormKind);
            
            //Get Last Error No.
            iLastErr = GetLastErrNo(false);


            if(m_bRqCloseForm) 
            {
                m_bRqCloseForm = false;
                ShowFrom(false);
            }

            if (isNeedSubFrm && m_bUpdatedErrForm) m_bUpdatedErrForm = false;
            if (m_iLastErr !=  iLastErr          ) m_bUpdatedErrForm = false;

            //Checking Update.
            if (m_bUpdatedErrForm) return;
     
            //Switch On Buzzer Flag.
            cDEF.LampBuzz._bBuzzOff = false;

            //Main Error Form
            if ( iLastErr < 0 || iLastErr >= vDEF.MAX_ERR) return;//JUNG/230130
            if ( iLastErr >= vDEF.MAX_WARN               ) return;//Warning
            if (!Err[iLastErr].IsGradeErr()              ) return;

            m_iLastErr = iLastErr;
            ShowFrom(true, iSubFormSel, iSubFormKind);

        }
        //------------------------------------------------------------------------
        public void ShowFrom(bool bShow, int iSubFrmSel=0, int iSubFrmKind=0)
        {
            if (FRM.Alarm != null) 
            {
                FRM.Alarm.Close();
                FRM.Alarm = null ;
            }

            if (!bShow) return;

            FRM.Alarm = new FrmAlarm();
            FRM.Alarm.m_iSubFormSel  = iSubFrmSel;
            FRM.Alarm.m_iSelFormKind = iSubFrmKind;
            FRM.Alarm.BringToFront();
            FRM.Alarm.Show();
            m_bUpdatedErrForm = true;

        }
        //------------------------------------------------------------------------
		public void  ErrDispListBox (ref System.Windows.Forms.ListBox  pErrList)
        {
            String Str;
            bool   isUpdateErr  = false;
            int    i;

            if (pErrList == null) return;

  
	        if(m_bRqClsList) {
                  m_bRqClsList = false;
                  pErrList.Items.Clear();
                  return;
            }
            //Check Update List.
            for (i = 0 ; i < vDEF.MAX_ERR ; i++) {
                    if ( Err[i].m_bOn && !Err[i].m_bUpdate) { isUpdateErr = true; break; }
                    if (!Err[i].m_bOn &&  Err[i].m_bUpdate) { isUpdateErr = true; break; }
                }
            
            if (!isUpdateErr) return;

            //Display to ListBox.
            pErrList.Items.Clear();
            for (i = 0; i < vDEF.MAX_ERR; i++)
            {
                Err[i].m_bUpdate = false;

                if (!Err[i].m_bOn    ) continue;
                Err[i].m_bUpdate = true;
                Str = string.Format("[ERR{0,4:0000}]", i) + GetName(i);
                pErrList.Items.Add(Str);

           }
        }
        //------------------------------------------------------------------------
        public bool IsNeedSubForm(out int iFrmSel, out int iFrmKind)
        {//UserSet - Alarm 화면에 표시할 SubMenu 처리 
            //Init.
            iFrmSel  = 0;
            iFrmKind = 0;

            //Var.


            //
            //if      (Err[(int)EN_ERR_LIST.ERR_1200].m_bOn) { iFrmSel = 1; iFrmKind = 1 ; }
            //                                                                           
            //else if (Err[(int)EN_ERR_LIST.ERR_1002].m_bOn) { iFrmSel = 3; iFrmKind = 0 ; } //Wafer Transfer-A Port에 Wafer 사라짐.
            //else if (Err[(int)EN_ERR_LIST.ERR_1003].m_bOn) { iFrmSel = 3; iFrmKind = 1 ; } //Wafer Transfer-B Port에 Wafer 사라짐.
            //else if (Err[(int)EN_ERR_LIST.ERR_1104].m_bOn) { iFrmSel = 3; iFrmKind = 2 ; }
            //
            ////
            //else if (Err[(int)EN_ERR_LIST.ERR_0590].m_bOn) { iFrmSel = 3; iFrmKind = 10; } //Supply Tape Empty Check.
            //else if (Err[(int)EN_ERR_LIST.ERR_0591].m_bOn) { iFrmSel = 3; iFrmKind = 11; } //Used Tape Full Check.
            //else if (Err[(int)EN_ERR_LIST.ERR_0592].m_bOn) { iFrmSel = 3; iFrmKind = 12; } //PROTECTION TAPE ROLLER FULL CHECK

           

            //
            return false;
        }
        //------------------------------------------------------------------------
        public void Display(ref System.Windows.Forms.DataGridView Grid)
        {
            String sErrNo;
			DataTable dt = new DataTable();

            //Position.
            if (Grid == null) return;

            Grid.Dock                     = System.Windows.Forms.DockStyle.Fill;
            FNC.SetGridStyle(ref Grid);
            Grid.BackgroundColor = FRM.GetGridBackColor(); //Color.FromArgb(66, 72, 88);
            Grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Century Gothic", 9);
            Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //
			dt.Columns.Add("Index", Type.GetType("System.String"));
			dt.Columns.Add("Name" , Type.GetType("System.String"));

            for(int i=1; i<vDEF.MAX_ERR;i++) 
            {
                //Display To List.
                sErrNo = string.Format("ERR{0,4:0000}" , i);
                dt.Rows.Add(sErrNo, Err[i].m_sName); 
            }

			Grid.DataSource = dt;
            Grid.Columns[0].Width = 80 ;
            Grid.Columns[1].Width = Grid.Width - 80 - 20 ;

            for (int n = 0; n < Grid.ColumnCount; n++)
            {
                if (n != 1)
                    Grid.Columns[n].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            Grid.Visible  = true;
        }


        //Loading File.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void  LoadErrDataXml(bool IsLoad)
        {
            String sPath;
            String sRootName = "ErrorList";
			String sElmName  = "Error";
            String sNode = string.Format("{0}/{1}", sRootName, sElmName);
			int    iCnt = 0;

            //Make Dir.
            FNC.CreateDirOnWork("Error");
            sPath = Application.StartupPath + "\\Error\\" + sElmName + ".XML";
			
            if (IsLoad)
            {
				XmlDocument xml = new XmlDocument();
				xml.Load(sPath);
				XmlNodeList xList = xml.SelectNodes(sNode);

				foreach (XmlNode xn in xList)
				{
					FNC.ParseInt(           xn["GRADE"   ].InnerText, 0, out Err[iCnt].m_iGrade);
					FNC.ParseInt(           xn["PART"    ].InnerText, 0, out Err[iCnt].m_iPart );
					FNC.ParseInt(           xn["KIND"    ].InnerText, 0, out Err[iCnt].m_iKind );
					Err[iCnt].m_bHoldErr = (xn["HOLDERR" ].InnerText == "true") ? true : false;
					Err[iCnt].m_bSendErr = (xn["SENDERR" ].InnerText == "true") ? true : false;
					Err[iCnt].m_sName    =  xn["NAME"    ].InnerText; 
					iCnt++;					  
				}							  
			}
            else
            {
				XDocument xdoc  = new XDocument(new XDeclaration("1.0", "UTF-8", null));
				XElement  xroot = new XElement (sRootName);
				xdoc.Add(xroot);
				for (int i = 0; i < vDEF.MAX_ERR; i++)
				{
					XElement xe = new XElement(sElmName, new XAttribute("ID"      , string.Format("ERR{0,4:0000}", i)),
													     new XElement  ("GRADE"   , Err[i].m_iGrade                  ),
													     new XElement  ("PART"    , Err[i].m_iPart                   ),
													     new XElement  ("KIND"    , Err[i].m_iKind                   ),
													     new XElement  ("HOLDERR" , Err[i].m_bHoldErr                ),
													     new XElement  ("SENDERR" , Err[i].m_bSendErr                ),
													     new XElement  ("NAME"    , Err[i].m_sName                   ),
													     new XElement  ("CAUSE"   , ""                               ),
													     new XElement  ("SOLUTION", ""                               ));
					xroot.Add(xe);
				}
				xdoc.Save(sPath);
			}
        }
        //------------------------------------------------------------------------
        public void SaveErrDataOneXml(int iNo)
        {			
			XmlDocument xdoc = new XmlDocument();
			string sRootName = "ErrorList";
			string sElmName  = "Error";
			string sNode     = string.Format("{0}/{1}", sRootName, sElmName);
			string sPath     = Application.StartupPath + "\\Error\\" + sElmName + ".XML";
			string sErroNo   = string.Format("ERR{0,4:0000}", iNo);	
			//
			xdoc.Load(sPath);
			//
			XmlNodeList xList = xdoc.SelectNodes(sNode);
			XmlAttributeCollection acxNode;
			foreach (XmlNode xn in xList)
			{
				acxNode = xn.Attributes;
				if (acxNode.GetNamedItem("ID") != null && acxNode.GetNamedItem("ID").Value == sErroNo)
				{
					xn["GRADE"   ].InnerText = Err[iNo].m_iGrade  .ToString(); 
					xn["PART"    ].InnerText = Err[iNo].m_iPart   .ToString();                
					xn["KIND"    ].InnerText = Err[iNo].m_iKind   .ToString();                
					xn["HOLDERR" ].InnerText = Err[iNo].m_bHoldErr ? "true" : "false";        
					xn["SENDERR" ].InnerText = Err[iNo].m_bSendErr ? "true" : "false";        
					xn["NAME"    ].InnerText = Err[iNo].m_sName   ;  
					//xn["CAUSE"   ].InnerText = Err[iNo].m_sName   ;  
					//xn["SOLUTION"].InnerText = Err[iNo].m_sName   ;   
					break;                        
				}
			}
			//
			xdoc.Save(sPath);
        }
        //------------------------------------------------------------------------
        public void ExportList()
        {
            String sPath  ;
            String sData  = "";

            string sFile =   "[" + string.Format("{0:yyMMdd}", DateTime.Now)+ "]" + "ErrorList.csv"; 
            //Make Dir.
            FNC.CreateDirOnWork("Export");
            sPath = Application.StartupPath + "\\Export\\" + sFile;


            //File Open.
            FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fp, Encoding.Default);
            sw.BaseStream.Seek(0, SeekOrigin.End);

            //Set List.
            sData  += "GRADE [0=Display][1=Warning][2=Error]\n";
            sData  += "KIND  [0=Machine][1=Material][2=Man][3=Method]\n";
            sData  += "NO, GRADE, KIND, PART, NAME\n";
            for (int iErr = 1 ; iErr < vDEF.MAX_ERR ; iErr++) {
               if(Err[iErr].m_sName == "") continue;  
               sData  += string.Format("{0,4:0000}, " , iErr);
               sData  += Convert.ToString(Err[iErr].m_iGrade)     + ",";
               sData  += Convert.ToString(Err[iErr].m_iKind )     + ",";
               sData  += cDEF.POSN.GetPartName(Err[iErr].m_iPart) + ",";
               sData  += Err[iErr].m_sName;
	           sData += "\r\n";
               }

	        sw.Write(sData);
            sw.Flush();
            sw.Close();
        }
        //------------------------------------------------------------------------
        public void  LoadErrDataIni    (bool IsLoad)
        {
            string sTemp = string.Empty; 
            string sPath;
            string sFile = "Error";
            string sSection;
            TIniUnit2 ini = new TIniUnit2();
           
            //Make Dir.
            FNC.CreateDirOnWork("Error");
            //sPath = Application.StartupPath + "\\Error\\" + sFile + ".ini";
            sPath = Application.StartupPath + "\\Error\\" + sFile + ".txt";

            //Load INI
            ini.Loadini(sPath);

            if (IsLoad)
            {
				for (int i = 0; i < vDEF.MAX_ERR; i++)
				{
					sSection = string.Format("ERR{0,4:0000}", i);
					ini.Load(sPath, "GRADE   ", sSection, out Err[i].m_iGrade    ); 
					ini.Load(sPath, "PART    ", sSection, out Err[i].m_iPart     );
					ini.Load(sPath, "KIND    ", sSection, out Err[i].m_iKind     );
					ini.Load(sPath, "HOLDERR ", sSection, out Err[i].m_bHoldErr  );
					ini.Load(sPath, "SENDERR ", sSection, out Err[i].m_bSendErr  );
					ini.Load(sPath, "NAME    ", sSection, out Err[i].m_sName     );
                    ini.Load(sPath, "SOLUTION", sSection, out sTemp              ); Err[i].m_sSoluttion = ConvStr(sTemp, true); 
                    ini.Load(sPath, "CAUSE   ", sSection, out sTemp              ); Err[i].m_sCause     = ConvStr(sTemp, true); 

                    //Set Add Error
                    if(Err[i].m_sName == "" || Err[i].m_sName == string.Empty)
                    {
                        if(ERRID.GetErrNames(i) != "")
                        {
                            Err[i].m_sName = ERRID.GetErrNames(i);
                            if (i > 100) Err[i].m_iGrade = (int)EN_ERR_GRADE.Error;

                            m_bNeedSave = true;
                        }
                    }
                    if (i> 100 && Err[i].m_sName != "" && Err[i].m_sName != string.Empty)
                    {
                        if(Err[i].m_iGrade == 0) Err[i].m_iGrade = 2; 
                    }
                }
            }
            else
            {
				for (int i = 0; i < vDEF.MAX_ERR; i++)
				{
					sSection = string.Format("ERR{0,4:0000}", i);
					ini.Save(sPath, "GRADE   ", sSection, Err[i].m_iGrade);
					ini.Save(sPath, "PART    ", sSection, Err[i].m_iPart);
					ini.Save(sPath, "KIND    ", sSection, Err[i].m_iKind);
					ini.Save(sPath, "HOLDERR ", sSection, Err[i].m_bHoldErr);
					ini.Save(sPath, "SENDERR ", sSection, Err[i].m_bSendErr);
					ini.Save(sPath, "NAME    ", sSection, Err[i].m_sName);
					ini.Save(sPath, "SOLUTION", sSection, ConvStr(Err[i].m_sSoluttion, false));
					ini.Save(sPath, "CAUSE   ", sSection, ConvStr(Err[i].m_sCause    , false));
				}

                //Save INI
                ini.Saveini(sPath);

            }
           
            ini = null;
        }
        //------------------------------------------------------------------------
        private string ConvStr(string data, bool ToRead)
        {
            if (data == "" || data == null) return "";
            string sTemp = string.Empty;
            if (ToRead)
            {
                sTemp = data .Replace('$', '\r');
                sTemp = sTemp.Replace('^', '\n');
            }
            else
            {
                sTemp = data .Replace('\r', '$');
                sTemp = sTemp.Replace('\n', '^');
            }

            return sTemp; 
        }
        //------------------------------------------------------------------------
        public void SaveErrDataOneini(int iNo)
        {
            //
            string    sPath;
            string    sFile = "Error";
            string    sSection;
            TIniUnit2 ini = new TIniUnit2();

            //Make Dir.
            FNC.CreateDirOnWork("Error");
            // sPath = Application.StartupPath + "\\Error\\" + sFile + ".INI";
            sPath = Application.StartupPath + "\\Error\\" + sFile + ".txt"; 
            
            //Load INI
            ini.Loadini(sPath);

            sSection = string.Format("ERR{0,4:0000}", iNo);
            ini.Save(sPath, "GRADE   ", sSection, Err[iNo].m_iGrade  );
            ini.Save(sPath, "PART    ", sSection, Err[iNo].m_iPart   );
            ini.Save(sPath, "KIND    ", sSection, Err[iNo].m_iKind   );
            ini.Save(sPath, "HOLDERR ", sSection, Err[iNo].m_bHoldErr);
            ini.Save(sPath, "SENDERR ", sSection, Err[iNo].m_bSendErr);
            ini.Save(sPath, "NAME    ", sSection, Err[iNo].m_sName   );
            ini.Save(sPath, "SOLUTION", sSection, ConvStr(Err[iNo].m_sSoluttion, false));
            ini.Save(sPath, "CAUSE   ", sSection, ConvStr(Err[iNo].m_sCause    , false));

            //Save INI
            ini.Saveini(sPath);
            ini = null;

		}

    }
}
