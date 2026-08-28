using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TSysMotor                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    //
    public class TSysMotor
    {
        /* Constants                                                          */
        /**********************************************************************/
        const int m_nFASTECH_MOTOR = 3;

        //Timer
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer   m_tServoOff = new TOnDelayTimer();
        TOnDelayTimer   m_tDrvReset = new TOnDelayTimer();
        
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        int          m_iNumOfMotr;
        int          m_iNumOfDstb;
        int          m_iAddDstb  ;    

        TAxisUnit[]  AXIS       ;
        int[]        m_iRef     ;
        int[]        m_iStepRst = new int[2];

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool m_bNeedReboot  ;
        public bool m_bInitAxis    ;
        public int  m_iFHomeErr    ;
        public int  m_iSpedMode    ;
        public bool m_bContiMove   ;
        public bool m_bSkipChkCrash;

        public _TDstbPosn[] DstbPosn;


        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TAxisUnit this[int Axe]
        {
            get 
            { 
                if (Axe<0 | Axe>=m_iNumOfMotr) return null;
                return AXIS[Axe]; 
            }
            set 
            {
                if (Axe<0 | Axe>=m_iNumOfMotr) return; 
                AXIS[Axe]= value; 
            }

        }

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int _iNumOfMotr { get { return m_iNumOfMotr; } }
        public int _iNumOfDstb { get { return m_iNumOfDstb; } }
        public int _iFHomeErr  { get { return m_iFHomeErr ; } set { m_iFHomeErr = value; }}

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSysMotor()
        {

        }
        ~TSysMotor() { /*CloseMotor();*/ }

        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public void    Init                 (params object[] args)
        {
            //Init - Motor Init는 MAX_MOTR:64 까지 진행
            try 
            {
				string sParamFile = Application.StartupPath + "\\System\\Motion_para_AJIN.mot";
				string sCmePath   = Application.StartupPath + "\\Motion_para_AJIN.mot";
				int    iObjNo;
				int    iMaker;
				int    iAxisNo  = (int)args[1];
				m_iNumOfMotr    = (int)EN_MOTR_ID.EndOfId;
				m_iNumOfDstb    = (int)EN_DSTB_ID.EndOfId ;
				m_iAddDstb      = 0;
				m_bSkipChkCrash = false;

				AXIS         = new TAxisUnit  [m_iNumOfMotr];
				m_iRef       = new int        [m_iNumOfMotr];
				DstbPosn     = new _TDstbPosn [m_iNumOfDstb];

				for (int iAxe=0; iAxe<m_iNumOfMotr; iAxe++) 
                {
				    AXIS[iAxe] = new TAxisUnit (); 
				}
				
                for (int i = 0; i < m_iNumOfDstb; i++)
				{
				    DstbPosn[i] = new _TDstbPosn();
				}
				
                for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                {
				    iObjNo = Axe;
				    iMaker = (int)args[0];

				    for(int iM=args.Count()-1;iM>=2;iM-=2)
				    {
				        if (Axe >= (int)args[iM]) 
                        { //3번 Maker Axis
				            iMaker = (int)args[iM-1];
				            iObjNo =  Axe - (int)args[iM];
				        }
				    }
				    this[(int)Axe].Init        (Axe, iObjNo, iMaker   ); //mmMC1x  , mmCOMI,  AJIN, mmEMTN
				    this[(int)Axe].SetParamPath(sParamFile , sCmePath );
				    this[(int)Axe].LoadOptn    (true);
				}

				//Open.
				m_bInitAxis = true;
				EN_MOTR_MAKER[] iOpenMaker = new EN_MOTR_MAKER[args.Count()];
				EN_MOTR_MAKER   iCrntMaker = EN_MOTR_MAKER.NONE;
				int             iOpenQty   = 0;
				bool            isSame     = false;

				for(int iM=1;iM<args.Count();iM+=2)
				{
				    iAxisNo = (int)args[iM];
				    if(iAxisNo>=m_iNumOfMotr) 
                    {
				        MsgBox.Error("Motor Open Error[Check InitMotor() Function]");
				        m_bInitAxis = false;
				        break;        
				    }
				    iCrntMaker = (EN_MOTR_MAKER)args[iM-1];

				    isSame = false;
				    for (int iO = 0; iO < iOpenQty; iO++)
				    {
				        if (iCrntMaker == iOpenMaker[iO]) { isSame = true; break; }
				        if (iCrntMaker == EN_MOTR_MAKER.AJIN || iCrntMaker == EN_MOTR_MAKER.AJECAT)
				        {
				            if (iOpenMaker[iO] == EN_MOTR_MAKER.AJIN || iOpenMaker[iO] == EN_MOTR_MAKER.AJECAT) { isSame = true; break; }
				        }
				    }
				    
                    //if (isSame) continue;

                    //
                    if(iCrntMaker == EN_MOTR_MAKER.FASTECH)
                    {
                        for (int i = 0; i < m_nFASTECH_MOTOR; i++)
                        {
                            if (!OpenMotor(i)) m_bInitAxis = false;
                        }
                    }
                    else
                    {
                        if (!OpenMotor(iAxisNo)) m_bInitAxis = false;
                    }

                    //
                    iOpenMaker[iOpenQty] = iCrntMaker;
				    iOpenQty ++;
				} 
				
				if(m_bInitAxis) InitServoOff();
     
				//Set Motor Parameter
				for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                {
				    this[Axe]._bInitAxis = m_bInitAxis; 
				    this[(int)Axe].SetParameter (Axe);
				}

				
				//Reset Motor. (Alarm , Flag, Step)
				Reset();            
			}
			catch (Exception err) 
            {
				m_bInitAxis = false;
                MsgBox.Error("Motor Open Error[Check InitMotor() Function] - " + err.Message.ToString());
			}
        }
        //------------------------------------------------------------------------
        public void SetDstb(String sName, String sDesc, String sUnit = "mm")
        {
            if(m_iAddDstb>=m_iNumOfDstb) return;
            DstbPosn[m_iAddDstb].sName = sName; 
		    DstbPosn[m_iAddDstb].sDesc = sDesc; 
		    DstbPosn[m_iAddDstb].sUnit = sUnit;
            m_iAddDstb ++;
        }
        //------------------------------------------------------------------------
        public void UpdateDstbByGrid(bool toGrid, ref System.Windows.Forms.DataGridView Grid)
        {
            int iTotWidth = 0;

	        int[]    iWidth = {250, 0, 100, 50};
	        String[] sItem  = {"NAME", "DESC", "VALUE","UNIT"};

            if(toGrid) {
                
                Grid.Dock = System.Windows.Forms.DockStyle.Fill;
                FNC.SetGridStyle(ref Grid);
                for(int i=0;i<4;i++) 
                {
                    Grid.Columns.Add(sItem[i] , sItem[i]);
                    Grid.Columns[i].Width = iWidth[i];
                    iTotWidth += iWidth[i];
                }
                Grid.Columns[1].Width = Grid.Width - iTotWidth-20;
                Grid.Columns[0].ReadOnly = true;
                Grid.Columns[1].ReadOnly = true;

                Grid.Columns[3].ReadOnly = true;

	            for(int i=0; i<m_iNumOfDstb;i++) 
                {
                    if(DstbPosn[i].sName == null) continue;
                    sItem[0] = DstbPosn[i].sName; 
		            sItem[1] = DstbPosn[i].sDesc; 
		            sItem[2] = Convert.ToString(DstbPosn[i].dPos); 
		            sItem[3] = DstbPosn[i].sUnit;
                    Grid.Rows.Add(sItem); 
		        }

                foreach (DataGridViewColumn item in Grid.Columns) { item.SortMode = DataGridViewColumnSortMode.NotSortable; }

                //
                //cDEF.POSN.SetGridFont(ref Grid);
            }
            else 
            {
                for(int i=0; i<Grid.RowCount;i++) 
                {
                    DstbPosn[i].dPos = Convert.ToDouble(Grid[2 ,i].Value);
                }
                LoadMotrDisturb(false);
            }
            Grid.Visible    = true;
        }
        //------------------------------------------------------------------------                                       
        public void    Reset(EN_MOTR_ID Axe)
        { 
            if(WrongAXE(Axe)) return; this[(int)Axe].Reset(); 
        }
        //------------------------------------------------------------------------                                       
        public void    Reset(          )
        { 
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) Reset((EN_MOTR_ID)Axe); 
        }
        //------------------------------------------------------------------------                                       
        public void    InitServoOff()
        {
            if(AXIS[0]._iMaker != (int)EN_MOTR_MAKER.AJIN) return;

            SetServo(false);
            m_tServoOff.Clear();
            bool isOneServoOn = false;
            do {
                isOneServoOn = false;
                m_tServoOff.OnDelay(true, 30000);
                for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) {
                    if(AXIS[Axe].GetServo()) { isOneServoOn = true; break; }
                }
                if(!isOneServoOn) break;
            }while(!m_tServoOff.Out);
        }
        //Check Min/Max                        
        public bool    WrongAXE             (EN_MOTR_ID Axe)
        { 
            return ((int)Axe<0 | (int)Axe>=m_iNumOfMotr); 
        }

        //Init. Motor.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    OpenMotor            (int iAxisNo) 
        {
            return AXIS[iAxisNo].Open  ();
        }
        //--------------------------------------------------------------------------
        public void    CloseMotor           (EN_MOTR_MAKER enAxis = EN_MOTR_MAKER.AJIN)         
        {//프로그램 종료시에 한번 호출.
            try
            {
                for (int iAxe = 0 ; iAxe < m_iNumOfMotr ; iAxe++) 
                {
                    AXIS[iAxe].SetServo(false);
                    AXIS[iAxe].SetPairAxe(AXIS[iAxe]._iPairAxisNo, 0);
                    if(enAxis == EN_MOTR_MAKER.FASTECH) AXIS[iAxe].Close();
                }
                //
                if (enAxis != EN_MOTR_MAKER.FASTECH) AXIS[0].Close ();

                m_bInitAxis = false; //JUNG/220307
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"Exception [CloseMotor] : {ex.Message}");
            }
        }
        public void    SetAxis()   
        {//프로그램 로딩시에 한번 호출.
            SetAxis_AsDevice();
        }
        public void    SetAxis_AsDevice()    
        {//JOB FILE 로딩시마다 호출.
            //Set InPos & S-Curve.
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
            {
                this[(int)Axe].SetInPos    (AXIS[Axe].MP.dPosn[(int)EN_POSN_ID.InPos]);
                this[(int)Axe].SetAppScurve(false                         );
                this[(int)Axe].MP.dPosn[(int)EN_POSN_ID.UserPitch] = 0.1;  //User Pitch는 모두 0.1mm로 한다.
            }
        }


        //Parameters.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TMOTN_PARA GetMP            (EN_MOTR_ID Axe ) {if(WrongAXE(Axe)) Axe = 0; return this[(int)Axe].MP ; }
        public TMOTN_PARA GetCMP           (EN_MOTR_ID Axe ) {if(WrongAXE(Axe)) Axe = 0; return this[(int)Axe].CMP; }
        public TAxisUnit  GetAXIS          (EN_MOTR_ID Axe ) {if(WrongAXE(Axe)) Axe = 0; return this[(int)Axe]    ; }
        public double     GetDSTB(int no)  { if (no < 0 || no >= m_iNumOfDstb) no=0;     return DstbPosn[no].dPos ; }
                                               


        //Device Informations.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double  GetPitch             (EN_MOTR_ID Axe     , EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            if(WrongAXE(Axe)) return 1.0;
            return this[(int)Axe].GetPitch(FStep);
        }        
        public double  GetCenPitch          (EN_MOTR_ID Axe     ,              EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            if(WrongAXE(Axe)) return 1.0;
            return this[(int)Axe].GetCenPitch(FStep);
        }
        public double  GetCenPitch          (EN_MOTR_ID Axe     , int iSlotNo, EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            if(WrongAXE(Axe)) return 1.0;
            return this[(int)Axe].GetCenPitch(iSlotNo, FStep);
        }
        public int     GetMaxSlot           (EN_MOTR_ID Axe     , EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1)
        {
            if(WrongAXE(Axe)) return 1;
            return this[(int)Axe].GetMaxSlot(FStep);
        }
        public bool  SetEachPitchSlot     (EN_MOTR_ID Axe, double Pitch = 1.0, int Slot = 1)
        {
            double[] dPitch    = new double [4];
            int   [] iSlot     = new int    [4];
 
            EN_FSTEP_INDEX FStep2 = EN_FSTEP_INDEX.NONE ;
            EN_FSTEP_INDEX FStep3 = EN_FSTEP_INDEX.NONE ;
            EN_FSTEP_INDEX FStep4 = EN_FSTEP_INDEX.NONE ;

            if(WrongAXE(Axe)) return false;

            dPitch[0] = Pitch;
            iSlot [0] = Slot ;

            if (dPitch[0] < 0.1) dPitch[0] = 1.0;
            if (iSlot [0] < 0.1) iSlot [0] = 1;

                                              this[(int)Axe].SetPitch(dPitch[0] , iSlot[0]                         );  //FSP1_x, FSP2_x, FSP3_x, FSP4_x;
            if(FStep2 != EN_FSTEP_INDEX.NONE) this[(int)Axe].SetPitch(dPitch[1] , iSlot[1], 0, EN_FSTEP_INDEX.Step2); //FSP2_x
            if(FStep3 != EN_FSTEP_INDEX.NONE) this[(int)Axe].SetPitch(dPitch[2] , iSlot[2], 0, EN_FSTEP_INDEX.Step3); //FSP3_x
            if(FStep4 != EN_FSTEP_INDEX.NONE) this[(int)Axe].SetPitch(dPitch[3] , iSlot[3], 0, EN_FSTEP_INDEX.Step4); //FSP4_x
            return true;
        }
        //Status.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    SetServo             (EN_MOTR_ID Axe , bool On)  //Each Servo On/Off.
        {
            if(WrongAXE(Axe)) return;          
            this[(int)Axe].SetServo(On);       
        } 
        
        public void    SetServo             (bool On, EN_MOTR_MAKER enAxis = EN_MOTR_MAKER.NONE) 
        {
            try
            {
                for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                {
                    if (enAxis == EN_MOTR_MAKER.NONE || this[(int)Axe]._iMaker == (int)enAxis)  SetServo((EN_MOTR_ID)Axe , On);
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"[SetServo] Exception : {ex.Message}");
            }
        }
       

        public void    SetAlarm             (EN_MOTR_ID Axe , bool On) //Clear each alarm.
        {
            if(WrongAXE(Axe)) return;          
            this[(int)Axe].SetAlarm(On);        
        }
        public void    SetAlarm             (bool On) 
        {//Clear all  alarm.
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                SetAlarm((EN_MOTR_ID)Axe , On); 
        }
        public void    ClearPos             (EN_MOTR_ID Axe , double Pos = 0.0) 
        {//Clear each axis position.
            if(WrongAXE(Axe)) return;          
            this[(int)Axe].ClearPos(Pos);      
        } 
        public void    ClearPos             (double Pos = 0.0) 
        {//Clear all  asis position.
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                ClearPos((EN_MOTR_ID)Axe , Pos);
        } 
        public bool    SetPos               (EN_MOTR_ID Axe , double Pos = 0.0) 
        {//Set Any Position.
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].SetPos        (Pos);
        } 
        public void    SetPosEncToCmd       (EN_MOTR_ID Axe) 
        {
            if(WrongAXE(Axe)) return;          
            this[(int)Axe].SetPosEncToCmd();   
        } 
        public void    ClearHomeEnd         (EN_MOTR_ID Axe) 
        {//Kill the each home end flag.
            if(WrongAXE(Axe)) return;          
            this[(int)Axe].ClearHomeEnd  ();   
        } 
        public void    ClearHomeEnd         () 
        {//Kill the all  home end flag. 
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                ClearHomeEnd((EN_MOTR_ID)Axe);   
        }
        public void    SetHomeEnd           (EN_MOTR_ID Axe , bool On) 
        { //Kill the each home end flag.
            if(WrongAXE(Axe)) return;          
            this[(int)Axe].SetHomeEnd  (On);   
        }
                                                                                                                                                
        public bool    IsAllAlarm () 
        {//Is all alarm    ? 
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
            {       
                 if (this[(int)Axe]._iNoUseMotr==1) continue;    
                 if(!this[(int)Axe].GetAlarm  ()) return false; 
            }
            return true; 
        } 
        public bool    IsAllServoOff () 
        {//Is all servo off 
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
            {
                if (this[(int)Axe]._iNoUseMotr==1) continue;    
                if ( this[(int)Axe].GetServo  ()) return false; 
            }
            return true; 
        }
        public bool    IsAllServoOn () 
        {
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
            {
                if (this[(int)Axe]._iNoUseMotr==1) continue;    
                if (!this[(int)Axe].GetServo  ()) return false; 
            }
            return true; 
        }
        public bool    IsAllHomeEnd () 
        {//Is all home  end 
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
            {
                if (this[(int)Axe]._iNoUseMotr==1) continue;    
                if (!this[(int)Axe].GetHomeEnd()) return false; 
            }
            return true; 
        }

        public bool    MotnIn (EN_MOTR_ID Axe , double S   , double E  , bool ChkTrg = false) 
        {//설정 범위내에 Motor가 위치 해 있는지  확인.
            //Local Var.
            double dCmd = GetCmdPos(Axe);
            double dEnc = GetEncPos(Axe);
            double dTrg = GetTrgPos(Axe);

            //Rturn.
            if (ChkTrg) { return ( ((dCmd >= S) &&  (dCmd <= E)) || ((dEnc >= S) &&  (dEnc <= E)) || ((dTrg >= S) &&  (dTrg <= E))); }
            else        { return ( ((dCmd >= S) &&  (dCmd <= E)) || ((dEnc >= S) &&  (dEnc <= E))                                 ); }
        }
        public bool    MotnIn(EN_MOTR_ID Axe , int iPosnId, double dArea = 0.5, bool ChkTrg = false)
        {
            //Local Var.
            double dCmd = GetCmdPos(Axe);
            double dEnc = GetEncPos(Axe);
            double dTrg = GetTrgPos(Axe);

            double dStrt  = this[(int)Axe].MP.dPosn[iPosnId] - dArea;
            double dEnd   = this[(int)Axe].MP.dPosn[iPosnId] + dArea;

            //Return.
            if (ChkTrg) { return ( ((dCmd >= dStrt) &&  (dCmd <= dEnd)) || ((dEnc >= dStrt) &&  (dEnc <= dEnd)) || ((dTrg >= dStrt) &&  (dTrg <= dEnd))); }
            else        { return ( ((dCmd >= dStrt) &&  (dCmd <= dEnd)) || ((dEnc >= dStrt) &&  (dEnc <= dEnd))                                        ); }
        }
        //Motor Position.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double  GetCmdPos            (EN_MOTR_ID Axe) 
        {//Set Any Position.
            if(WrongAXE(Axe)) return 0.0; 
            return this[(int)Axe].GetCmdPos   ();
        } 
        public double  GetTrgPos            (EN_MOTR_ID Axe) 
        {//Set Any Position.
            if(WrongAXE(Axe)) return 0.0; 
            return this[(int)Axe].GetTrgPos   ();
        } 
        public double  GetEncPos            (EN_MOTR_ID Axe) 
        { //Set Any Position.
            if(WrongAXE(Axe)) return 0.0; 
            return this[(int)Axe].GetEncPos   ();
        }
        public double  GetAbsEncPos         (EN_MOTR_ID Axe) 
        {//Set Any Position.
            if(WrongAXE(Axe)) return 0.0; 
            return this[(int)Axe].GetAbsEncPos();
        } 
        //Inspection Motor.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //Tool Impact.
        public bool    CheckCrash(EN_MOTR_ID Axe, double TrgPosn)
        {
            //bool        r1, r2;
            //double      dEncPosTFM_X   = cDEF.MOTR.GetEncPos(EN_MOTR_ID.TFM_X);
            //double      dCmdPosTFM_X   = cDEF.MOTR.GetCmdPos(EN_MOTR_ID.TFM_X);
            //double      dTrgPosTFM_X   = cDEF.MOTR.GetTrgPos(EN_MOTR_ID.TFM_X);
            //            
            //double      dEncPosTAM_X   = cDEF.MOTR.GetEncPos(EN_MOTR_ID.TAM_X);
            //double      dCmdPosTAM_X   = cDEF.MOTR.GetCmdPos(EN_MOTR_ID.TAM_X);
            //double      dTrgPosTAM_X   = cDEF.MOTR.GetTrgPos(EN_MOTR_ID.TAM_X);
            //            
            //double      dEncPosTCH_Z   = cDEF.MOTR.GetEncPos(EN_MOTR_ID.TCH_Z);
            //double      dCmdPosTCH_Z   = cDEF.MOTR.GetCmdPos(EN_MOTR_ID.TCH_Z);
            //double      dTrgPosTCH_Z   = cDEF.MOTR.GetTrgPos(EN_MOTR_ID.TCH_Z);
            //            
            //double      dEncPosWTR_AY  = cDEF.MOTR.GetEncPos(EN_MOTR_ID.WTR_AY);
            //double      dCmdPosWTR_AY  = cDEF.MOTR.GetCmdPos(EN_MOTR_ID.WTR_AY);
            //double      dTrgPosWTR_AY  = cDEF.MOTR.GetTrgPos(EN_MOTR_ID.WTR_AY);
            //            
            //double      dEncPosWTR_BY  = cDEF.MOTR.GetEncPos(EN_MOTR_ID.WTR_BY);
            //double      dCmdPosWTR_BY  = cDEF.MOTR.GetCmdPos(EN_MOTR_ID.WTR_BY);
            //double      dTrgPosWTR_BY  = cDEF.MOTR.GetTrgPos(EN_MOTR_ID.WTR_BY);

            

            //Check.
            switch (Axe)
            {
                default: 
                    break;
            }

            //
            return true;
        }
        //------------------------------------------------------------------------
        public bool    InspectCrash         (EN_SEQ_STAT SeqStat)
        {
            return false;
        }
        //------------------------------------------------------------------------
        public bool    CheckCrash           (EN_MOTR_ID  Axe   , EN_COMD_ID Cmd = EN_COMD_ID.NoneCmd, 
                                                                 int Step = vDEF.NONE_STEP , EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.NONE , double DirPosn = 0.0)
        {
            int iPart = 0;
            int iItem = 0;
            if (!cDEF.POSN.GetMotorPart(ref iPart, ref iItem, (int)Axe)) return false;
            //if(m_bSkipChkCrash) return true;
            bool isOk = cDEF.SEQ.CheckDstb((EN_SEQ_ID)iPart, Axe, Cmd, Step, FIndex, DirPosn);
            return isOk;
        }
        //------------------------------------------------------------------------
        public bool    CheckTorqueLimit     (EN_MOTR_ID Axe, double dTorque, double dchkRange)
        {
            if( dTorque <= 0                                ) return false;
            if( this[(int)Axe].GetStop()                    ) return false;
            if(!this[(int)Axe].m_bUseTorque                 ) return false;
            if( this[(int)Axe].GetEncPos() < (dchkRange+0.5)) return false;

            if(Math.Abs(this[(int)Axe].GetTorque())>=dTorque) 
            {
                Stop(Axe);
                return false;
            }
            return true;
        }

            //Min-Max
        public bool    InspectMinMax        (bool ChkOnly = false)
        {
            for (int i = 0 ; i < m_iNumOfMotr ; i++) {
                //Position.
                for (int p = 0 ; p < vDEF.MAX_POSN ; p++) {
                    if (!CheckMinMaxP((EN_MOTR_ID)i  , p)){
                        cDEF.EPU.SetErr(ErrNoPos((EN_MOTR_ID)i) , !ChkOnly);
                        return false;
                        }
                    }
                //Vel.
                for (int v = 0 ; v < (int)EN_MOTR_VEL.LJog  ; v++) {
                    if (!CheckMinMaxV((EN_MOTR_ID)i  , v)) {
                        cDEF.EPU.SetErr(ErrNoVel((EN_MOTR_ID)i) , !ChkOnly);
                        return false;
                        }
                    }
                //Acc
                for (int v = 0 ; v <= (int)EN_MOTR_VEL.LJog ; v++) {
                    if (!CheckMinMaxA((EN_MOTR_ID)i  , v)) {
                        cDEF.EPU.SetErr(ErrNoAcc((EN_MOTR_ID)i) , !ChkOnly); return false;
                        }
                    }
                }

            //Ok.
            return true;
        }
        public bool    CheckMinMax          (EN_MOTR_ID Axe , double P , double V , double A) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMax (P ,V ,A);
        }
        public bool    CheckMinMaxP         (EN_MOTR_ID Axe , double P) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMaxP(P      );
        }
        public bool    CheckMinMaxV         (EN_MOTR_ID Axe , double V) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMaxV(V      );
        }
        public bool    CheckMinMaxA         (EN_MOTR_ID Axe , double A) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMaxA(A      );
        }
        public bool    CheckMinMax          (EN_MOTR_ID Axe , int   P , int V) 
        {//mpPara Cheking.
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMax (P ,V   );
        }
        public bool    CheckMinMaxP         (EN_MOTR_ID Axe , int    P) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMaxP(P      );
        }
        public bool    CheckMinMaxV         (EN_MOTR_ID Axe , int    V) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMaxV(V      );
        }
        public bool    CheckMinMaxA         (EN_MOTR_ID Axe , int    V) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckMinMaxA(V      );
        }



        //Comapare position.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    CmprPos              (EN_MOTR_ID Axe , double Pos) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CmprPos     (Pos);
        }
        public bool    CmprPos              (EN_MOTR_ID Axe , double Pos , double InPos) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CmprPos     (Pos ,InPos);
        }
        public int     CmprPosStat          (EN_MOTR_ID Axe , double Pos , bool ChkTrg   = false) 
        {
            if(WrongAXE(Axe)) return 0; 
            return this[(int)Axe].CmprPosStat (Pos ,ChkTrg);
        }
        public int     CmprPosStat          (EN_MOTR_ID Axe , double Pos , double Offset , bool ChkTrg   = false ) 
        {
            if(WrongAXE(Axe)) return 0; 
            return this[(int)Axe].CmprPosStat (Pos ,Offset ,ChkTrg);
        }
        public int     CmprPosStat          (EN_MOTR_ID Axe , int    PosID , bool ChkTrg = false) 
        {
            if(WrongAXE(Axe)) return 0; 
            return this[(int)Axe].CmprPosStat (PosID ,ChkTrg);
        }
        public bool    CmprStep             (EN_MOTR_ID Axe , int Step   , 
                                             EN_FSTEP_INDEX FStep = EN_FSTEP_INDEX.Step1 , EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.Index1) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CmprStep    (Step ,FStep ,FIndex);
        }
        public bool    CmprArea             (EN_MOTR_ID Axe , double Pos , double Area , bool NoChkEnc = false) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CmprArea    (Pos ,Area ,NoChkEnc);
        }

        public bool    CmprPosByCmd         (EN_MOTR_ID Axe , EN_COMD_ID Comd , double InPos = 0, int iStep=0, int iIndex=0) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CmprPosByCmd(Comd ,InPos  ,iStep ,iIndex);
        }
                                               
        public int     WhreTarget           (EN_MOTR_ID Axe) 
        {//Motor의 Target 확인.
            if(WrongAXE(Axe)) return vDEF.MOTR_TARG_STP; 
            return this[(int)Axe].WhreTarget();
        }
        public int     WhreTarget           (EN_MOTR_ID Axe , EN_COMD_ID Cmd  , int Step , EN_FPOSN_INDEX FIndex , double DirPosn = 0.0  )
        {
            if(WrongAXE(Axe)) return vDEF.MOTR_TARG_STP;
            double dPos = 0.0;

            if      (Cmd == EN_COMD_ID.Direct) dPos = DirPosn                                 ;
            else                                         dPos = GetNextCmdTrg(Axe , Cmd , Step , FIndex);

            if      (this[(int)Axe].GetTrgPos() < (dPos + this[(int)Axe].MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.MOTR_TARG_POS;
            else if (this[(int)Axe].GetTrgPos() > (dPos - this[(int)Axe].MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.MOTR_TARG_NEG;

            return vDEF.MOTR_TARG_STP;
        }
        public int     WhreTarget           (EN_MOTR_ID Axe , double dPos)
        {
            if(WrongAXE(Axe)) return vDEF.MOTR_TARG_STP;

            if      (this[(int)Axe].GetTrgPos() < (dPos + this[(int)Axe].MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.MOTR_TARG_POS;
            else if (this[(int)Axe].GetTrgPos() > (dPos - this[(int)Axe].MP.dPosn[(int)EN_POSN_ID.InPos])) return vDEF.MOTR_TARG_NEG;

            return vDEF.MOTR_TARG_STP;
        }

        //Converter
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public double ConvVelToRPM(double dVel, double dDist = 1.0) 
        { // mm/s -> RPM ,  dDist :  //회전당 이동 거리
            double dRPM  = 0.0  ;

            dRPM = (dVel*60)/dDist;

            return dRPM;
        } 
        public double ConvRpmToVel(double dRPM, double dDist = 1.0) 
        { // RPM  -> mm/s ,  dDist :  //회전당 이동 거리 
            double dVel = 0.0;

            dVel = (dDist*dRPM)/60;

            return dVel;
        } 

        //Motion.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    EmrgStop(EN_MOTR_ID Axe) 
        { //지정 비상 정지.
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].EmrgStop();                    
        } 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    EmrgStop() 
        {//전제 비상 정지.
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                EmrgStop((EN_MOTR_ID)Axe); 
        } 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    Stop(EN_MOTR_ID Axe , double Dec = 0.1) 
        {//지정 모터 감속 정지.
            if(WrongAXE(Axe)) 
            {
                for (int iAxis = 0 ; iAxis < m_iNumOfMotr ; iAxis++) this[(int)iAxis].Stop(true, Dec);     
                return true;
            }    
            return this[(int)Axe].Stop(true, Dec);               
        } 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    Stop() 
        {  //전제 모터 감속 정지.
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
                Stop((EN_MOTR_ID)Axe);     
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    MoveHome(EN_MOTR_ID Axe) 
        { //지정 모터 Home. (모터 자체의 Home 작업이며, 각 Part별 Home 작업과 구별)
            if(WrongAXE(Axe)) return false; 

			if(!cDEF.MOTR.CheckCrash(Axe, EN_COMD_ID.Home)) return false;

            return this[(int)Axe].MoveHome();                    
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    MoveMotr(EN_MOTR_ID Axe , double P    , double V , double A , bool   R) //상대 위치 이동.
        {
            if((int)Axe<0 || (int)Axe>=m_iNumOfMotr) return false;

            //
            if (!CheckCrash(Axe, EN_COMD_ID.Direct , vDEF.NONE_STEP , EN_FPOSN_INDEX.NONE , P)) return false;


            //Check MinMax
            if (!CheckMinMax(Axe , P , V , A)) { InspectMinMax(); return false; }

            //Go Move.
            if (R) return this[(int)Axe].MoveR(P , V , A);
            else   return this[(int)Axe].MoveA(P , V , A);
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    MoveMotr(EN_MOTR_ID Axe , double P    , double V , double A , double D = 0.0) //절대 위치 이동.
        {
            if((int)Axe<0 || (int)Axe>=m_iNumOfMotr) return false;

            //
            if (!CheckCrash(Axe, EN_COMD_ID.Direct , vDEF.NONE_STEP , EN_FPOSN_INDEX.NONE , P)) return false;


            //Check MinMax
            if (!CheckMinMax(Axe , P , V , A)) { InspectMinMax(); return false; }
            if (!CheckMinMax(Axe , P , V , D)) { InspectMinMax(); return false; }

            //Go Move.
            return this[(int)Axe].MoveA(P , V , A , D);
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    MoveMotr(EN_MOTR_ID Axe , double P , EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal)
        {
            if((int)Axe<0 || (int)Axe>=m_iNumOfMotr) return false;

            double V, A, D;

            if(iSPD == EN_MOTR_VEL.Normal) 
            {
                switch ((EN_SPED_MODE)m_iSpedMode) 
                {
                    default               : V = this[(int)Axe].MP.dVel[(int)EN_MOTR_VEL.Dry ]; break;
                    case EN_SPED_MODE.Dry : V = this[(int)Axe].MP.dVel[(int)EN_MOTR_VEL.Dry ]; break;
                    case EN_SPED_MODE.Work: V = this[(int)Axe].MP.dVel[(int)EN_MOTR_VEL.Work]; break;
                }
                A = this[(int)Axe].MP.dAcc[(int)EN_MOTR_VEL.Work];
                D = this[(int)Axe].MP.dDec[(int)EN_MOTR_VEL.Work];
            }
            else 
            {
                if ((int)iSPD<0 || (int)iSPD>=vDEF.MAX_SPED) return false;
                A = this[(int)Axe].MP.dAcc[(int)iSPD];
                D = this[(int)Axe].MP.dDec[(int)iSPD];
                V = this[(int)Axe].MP.dVel[(int)iSPD];
            }

            //Check MinMax
            if (!CheckMinMax(Axe , P , V , A)) { InspectMinMax(); return false; }
            if (!CheckMinMax(Axe , P , V , D)) { InspectMinMax(); return false; }

            //
            if (!CheckCrash(Axe, EN_COMD_ID.Direct , vDEF.NONE_STEP , EN_FPOSN_INDEX.NONE , P)) return false;

            //Go Move.
            return this[(int)Axe].MoveA(P , V , A , D);
        }

        //public bool MoveLine(stSyncMoveParm Parm)
        //{
        //    //var
        //    long     lFuncRet    = (uint)AXT_FUNC_RESULT.AXT_ERROR_INVALID_AXIS_NO;
        //    int      iAxisCnt    = Parm.iAxisNo.Length;
        //    int      iAxis       = Parm.iAxisNo[0];
        //    bool     isContiDone = this[iAxis].GetContiMotnDone(Parm.iCoodiNo);
        //    bool[]   isAllDone   = new bool  [iAxisCnt];
        //    double[] lPos        = new double[iAxisCnt];
        //    double   lVel        = this[iAxis].ConvVel(Parm.dVel);
        //    double   lAcc        = this[iAxis].ConvAcc(Parm.dVel, Parm.dAccDec);           
        //    uint     iRtn        = 0;
        //    int []   aAxis       = {Parm.iAxisNo[0], Parm.iAxisNo[1]};
        //    int      nCodeNo     = Parm.iCoodiNo;
        //    int      nSize       = Parm.iPosSize;
        //
        //    //
        //    for (int n = 0; n < Parm.iAxisNo.Length; n++)
        //    {
        //        iAxis   = (int)Parm.iAxisNo[n];
        //        lPos[n] = this[iAxis].CalPosToPulse(Parm.dPosn[n]);
        //        //
        //        if (!this[iAxis].isMakerAjin() && !this[iAxis].isMakerSIMUL()) return false;
        //        if (iAxis<0 || iAxis>=m_iNumOfMotr) return false;
        //        
        //        //Check MinMax
        //        if (!CheckMinMax((EN_MOTR_ID)iAxis , Parm.dPosn[n] , Parm.dVel , Parm.dAccDec)) { InspectMinMax(); return false; }
        //        if (!CheckCrash ((EN_MOTR_ID)iAxis, EN_COMD_ID.Direct , vDEF.NONE_STEP , EN_FPOSN_INDEX.NONE , Parm.dPosn[n])) return false;
        //    }
        //
        //    //
        //    iRtn = CAXM.AxmContiWriteClear   (Parm.iCoodiNo);
        //    iRtn = CAXM.AxmContiSetAxisMap   (Parm.iCoodiNo, (uint)Parm.iPosSize, Parm.iAxisNo);
        //    iRtn = CAXM.AxmContiSetAbsRelMode(Parm.iCoodiNo, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE);
        //    
        //    //
        //    if (isContiDone)
        //    {
        //        lFuncRet = CAXM.AxmLineMove(Parm.iCoodiNo, lPos, lVel, lAcc, lAcc);
        //    }                                                    
        //    //
        //    if (lFuncRet == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
        //    {
        //        for (int n = 0; n < Parm.iAxisNo.Length; n++)
        //        {
        //            iAxis   = (int)Parm.iAxisNo[n];
        //            this[iAxis].SetTrgPos(Parm.dPosn[n]);
        //        }
        //    }
        //    //
        //    for (int n = 0; n < Parm.iAxisNo.Length; n++)
        //    {
        //        iAxis        = (int)Parm.iAxisNo[n];
        //        isAllDone[n] = this[iAxis].MotnDone(true, 0.1);
        //    }            
        //    return Array.TrueForAll(isAllDone, find => find == true) && isContiDone;
        //}
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    MoveJog(EN_MOTR_ID Axe , bool Dir    , EN_MOTR_VEL iSPD = EN_MOTR_VEL.Normal) 
        {//JOG 이동.
            for (int n = 0; n < (int)EN_MOTR_ID.EndOfId; n++)
            {
                if (n == (int)Axe) continue;
                cDEF.MOTR.Stop((EN_MOTR_ID)n);
            }
            //
            EN_COMD_ID cmd = (Dir == true) ? EN_COMD_ID.JogP : EN_COMD_ID.JogN;
            //
            if(WrongAXE(Axe)) return false; 

            //
            if (!CheckCrash(Axe, cmd))
            {
                this[(int)Axe].Stop(true);
                return false;
            }
            //
            return this[(int)Axe].MoveJog (Dir   , iSPD ); 
        } 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    MotnDone(EN_MOTR_ID Axe , bool ChkEnc , double Inp) 
        {//사용자 임의 지정 InPos의 Motion Done 확인.
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].MotnDone(ChkEnc, Inp);   
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    MotnDone(EN_MOTR_ID Axe) 
        {//설정 InPos의 Motion Done 확인.
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].MotnDone();              
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    SetVGain(EN_MOTR_ID Axe , double V) 
        {
            if(WrongAXE(Axe)) return; 
            this[(int)Axe].SetVGain(V);           
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    SetAGain(EN_MOTR_ID Axe , double A) 
        {
            if(WrongAXE(Axe)) return; 
            this[(int)Axe].SetAGain(A);           
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    SetGain(EN_MOTR_ID Axe , double V    , double A) 
        {
            if(WrongAXE(Axe)) return; 
            this[(int)Axe].SetGain (V,A);         
        }
        public void    SetVGainRatio(EN_MOTR_ID Axe , EN_MOTR_VEL VelType , double Ratio) 
        {
            if(WrongAXE(Axe)) return; 
            //
            double V = cDEF.MOTR[(int)Axe].MP.dVel[(int)VelType];
            if (V <= 0) V = cDEF.MOTR [(int)Axe].GetMinVel();
            //
            double dVGain = (Ratio / 100.0);
            this[(int)Axe].SetVGain (dVGain);         
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    SetSpeedMode(bool Run)
        {
           m_iSpedMode = Run ? (int)EN_SPED_MODE.Work : (int)EN_SPED_MODE.Dry;            
        }
 

        public bool    MoveAsComd(EN_MOTR_ID Axe , EN_COMD_ID Comd , EN_MOTR_VEL iSPD , int Step  = 0, 
                                  EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.NONE, double DirPosn = 0.0   ) //Command ID에 따른 위치 이동.
        {
            //To remove incorrect Axe.
            if((int)Axe < 0 || (int)Axe >= m_iNumOfMotr) return false;

            //Command
            double dPosn;
            switch (Comd) 
            {
                case EN_COMD_ID.Stop      : return this[(int)Axe].Stop    (true              );
                case EN_COMD_ID.EStop     : return this[(int)Axe].EmrgStop(                  );
                case EN_COMD_ID.Home      : return MoveHome               (Axe               );
                case EN_COMD_ID.JogP      : return MoveJog                (Axe  , true , iSPD);
                case EN_COMD_ID.JogN      : return MoveJog                (Axe  , false, iSPD);
            }

            dPosn = this[(int)Axe].GetNextCmdTrg(Comd ,Step ,FIndex ,DirPosn);

            //Check Crash.
            /*Move Motr로 이동
            if(Comd !=  EN_COMD_ID.Direct) {
                if (!CheckCrash(Axe, Comd , Step, FIndex)) return false;
                }
            else {
                if (!CheckCrash(Axe, Comd , vDEF.NONE_STEP , EN_FPOSN_INDEX.NONE , DirPosn)) return false;
            }
            */

            return MoveMotr(Axe , dPosn, iSPD);
        }
        public double  GetNextCmdTrg(EN_MOTR_ID Axe , EN_COMD_ID Comd , int Step  = 0, 
                                     EN_FPOSN_INDEX FIndex = EN_FPOSN_INDEX.NONE, double DirPosn = 0.0   ) //다음 모터의 Command Target.
        {
            double dPosn = this[(int)Axe].GetNextCmdTrg(Comd ,Step ,FIndex ,DirPosn);
            return dPosn;
        }
        public double  GetLinear(double dXi, double dX1, double dX2, double dY1, double dY2)
        {
            double dA      = 0.0;
            double dB      = 0.0;
            double dLinear = 0.0;

            if(dX2 - dX1 == 0) return 0.0;

            dA      = (dY2 -  dY1) / (dX2 - dX1);
            dB      =  dY1 -         (dA  * dX1);
            dLinear = (dA  *  dXi) +  dB        ;

            return dLinear;
        }
        public double  GetCalcStrtPlateX(int iTopId, int iBtmId, int iWorkRow)
        {            
            double dCalcPosnX     = 0.0;

            return dCalcPosnX;
        }
        public double  CalExpansion(EN_MOTR_ID Motr, double ColPitch , double Min , double Max)
        {
            double dPosn     = 0.0;

            return dPosn;
        }       
        //Check Part Motr.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    CheckPartMotr(EN_MOTR_ID Axe , int Part) 
        {
            if(WrongAXE(Axe)) return false; 
            return this[(int)Axe].CheckPartMotr(Part); 
        }
 
        //Disply Motor Set Form
        public void    DispParamFrm(EN_MOTR_ID Axe) 
        {
            if(WrongAXE(Axe)) return      ; 
            this[(int)Axe].DispParamFrm(); 
        }

        //Update.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void    Update(bool Run)
        {
            try
            {
                m_iSpedMode = Run ? (int)EN_SPED_MODE.Work : (int)EN_SPED_MODE.Dry;
                UpdateAxe1();
                if(AXIS[0]._iMaker == (int)EN_MOTR_MAKER.AJIN) AjinDrvReset();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Motor Update : " + e.Message);
                cDEF.LOG.ExceptionTrace("Motor Update", e);
                return;
            }
        }
        //------------------------------------------------------------------------
        public void    UpdateAxe1()
        {
            try
            {
                //Set Speed Mode.
                for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++)
                {
                    this[(int)Axe].Update();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Motor Update1 : " + e.Message);
                cDEF.LOG.ExceptionTrace("Motor Update1", e);
                return;
            }
        }
        //------------------------------------------------------------------------
        public void    UpdateAxe2()
        {

        }
        //------------------------------------------------------------------------
        //Read/Write Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Load(bool IsLoad, string Device, EN_SEQ_ID SelPart = EN_SEQ_ID.ALL)
        {
            //Check Device.
            if (Device == "") return;

            //Local Var.
            string sPath;

            //Make Dir.
            sPath = Application.StartupPath + "\\Project";
            FNC.CreateDir(sPath);
            sPath = Application.StartupPath + "\\Project\\" + Device;
            FNC.CreateDir(sPath);

            //Load Motor.
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
            {
				Application.DoEvents();
                 //Check Part Motr.
                if (SelPart != EN_SEQ_ID.ALL)
                {
                     if (!CheckPartMotr((EN_MOTR_ID)Axe, (int)SelPart)) continue;
                }
                //Load.
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                if (IsLoad)
                {
                    this[(int)Axe].Load(IsLoad , sPath, this[(int)Axe].MP);
                    //
                }
                //
                DefSetPos();

                //Save.
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                if (!IsLoad) 
                {
                    //
                    //
                    this[(int)Axe].Load(IsLoad , sPath, this[(int)Axe].MP);
                }
            }
            //
            SetAxis_AsDevice();
        }


       //Read/Write Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void LoadCommSpd(bool IsLoad, EN_SEQ_ID SelPart = EN_SEQ_ID.ALL)
        {
            //Load Motor Speed.
            for (int Axe = 0 ; Axe < m_iNumOfMotr ; Axe++) 
            {
                 //Check Part Motr.
                if (SelPart != EN_SEQ_ID.ALL)
                     if (!CheckPartMotr((EN_MOTR_ID)Axe, (int)SelPart)) continue;
                
                this[(int)Axe].LoadCommSpd(IsLoad);
            }

        }

        public void    LoadAxe(bool IsLoad , string Device , int Axe)
        {
            //Check Device.
            if (Device == "") return;

            //Local Var.
            String sPath;

            //Make Dir.
            sPath = Application.StartupPath + "\\Project";
            FNC.CreateDir(sPath);
            sPath = Application.StartupPath + "\\Project\\" + Device;
            FNC.CreateDir(sPath);
            //Load Motor.
            if (IsLoad) this[(int)Axe].Load(IsLoad , sPath, this[(int)Axe].MP);

            //
            DefSetPos();

            //Save.
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            if (!IsLoad) this[(int)Axe].Load(IsLoad , sPath, this[(int)Axe].MP);
        }
        public void    LoadMotrDisturb(bool IsLoad)                                      
        {
            //Local Var.
            String sName;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            String Path = Application.StartupPath + "\\System";
            FNC.CreateDir(Path);
            Path += "\\MotorDisturb.INI";

            //Load Master Options.
            if (IsLoad)
            {
                //Normal Load.
                for(int i=0;i<m_iNumOfDstb;i++)
                {
                    sName = string.Format("DSTB{0}", i);
                    ini.Load(Path, "MASTER_DSTB", sName, out DstbPosn[i].dPos);

                }
            }
            else
            {
                for(int i=0;i<m_iNumOfDstb;i++) 
                {
                    sName = string.Format("DSTB{0}", i);
                    ini.Save(Path, "MASTER_DSTB", sName, DstbPosn[i].dPos);
                }
            }
            ini = null;
        }                                               
        //Get Manual Define                    
        public int     ManNoStop   (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iManStop    ;}
        public int     ManNoJog    (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iManJog     ;}
        public int     ManNoPitch  (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iManPitch   ;}
        public int     ManNoServo  (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iManServo   ;}
        public int     ManNoAlarm  (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iManAlarm   ;}
        public int     ManNoDirect (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iManDirect  ;}
        public int     ManNoHome   (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iManHome    ;}
                                      
        //Get Error Define            
        public int     ErrNoAlarm  (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrAlarm   ;}
        public int     ErrNoCW     (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrCW      ;}
        public int     ErrNoCCW    (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrCCW     ;}
        public int     ErrNoHome   (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrHome    ;}
        public int     ErrNoControl(EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrControl ;}
        public int     ErrNoHold   (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrHold    ;}
        public int     ErrNoPos    (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrPos     ;}
        public int     ErrNoVel    (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrVel     ;}
        public int     ErrNoAcc    (EN_MOTR_ID Axe) {if(WrongAXE(Axe)) return -1; return this[(int)Axe].m_iErrAcc     ;}
                                               
        //------------------------------------------------------------------------
        //Default Pos Setting                  
        public void    DefSetPos()
        {

             //cDEF.MOTR[(int)EN_MOTR_ID.TCH_Z].MP.dPosn[(int)EN_POSN_ID.FSP2_1] =  cDEF.MOTR[(int)EN_MOTR_ID.TCH_Z].MP.dPosn[(int)EN_POSN_ID.FSP1_1];
            /*
            //UserSet - Motor Default Position 설정 
            AXIS[(int)EN_MOTR_ID.WP_X ].MP.dPosn[(int)EN_POSN_ID.FSP1_1] = -AXIS[(int)EN_MOTR_ID.WP_X ].MP.dPosn[(int)EN_POSN_ID.User1 ];
                                                                                                                                       
            AXIS[(int)EN_MOTR_ID.WP_Z2].MP.dPosn[(int)EN_POSN_ID.Pick1 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Pick1 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User1 ];
            AXIS[(int)EN_MOTR_ID.WP_Z3].MP.dPosn[(int)EN_POSN_ID.Pick1 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Pick1 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User3 ];
            AXIS[(int)EN_MOTR_ID.WP_Z4].MP.dPosn[(int)EN_POSN_ID.Pick1 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Pick1 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User5 ];
            AXIS[(int)EN_MOTR_ID.WP_Z2].MP.dPosn[(int)EN_POSN_ID.Pick2 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Pick2 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User2 ];
            AXIS[(int)EN_MOTR_ID.WP_Z3].MP.dPosn[(int)EN_POSN_ID.Pick2 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Pick2 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User4 ];
            AXIS[(int)EN_MOTR_ID.WP_Z4].MP.dPosn[(int)EN_POSN_ID.Pick2 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Pick2 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User6 ];
                                                                                                                                                                                                      
            AXIS[(int)EN_MOTR_ID.WP_Z2].MP.dPosn[(int)EN_POSN_ID.Plce1 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Plce1 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User1 ];
            AXIS[(int)EN_MOTR_ID.WP_Z3].MP.dPosn[(int)EN_POSN_ID.Plce1 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Plce1 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User3 ];
            AXIS[(int)EN_MOTR_ID.WP_Z4].MP.dPosn[(int)EN_POSN_ID.Plce1 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Plce1 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User5 ];
            AXIS[(int)EN_MOTR_ID.WP_Z2].MP.dPosn[(int)EN_POSN_ID.Plce2 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Plce2 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User2 ];
            AXIS[(int)EN_MOTR_ID.WP_Z3].MP.dPosn[(int)EN_POSN_ID.Plce2 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Plce2 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User4 ];
            AXIS[(int)EN_MOTR_ID.WP_Z4].MP.dPosn[(int)EN_POSN_ID.Plce2 ] =  AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.Plce2 ] + AXIS[(int)EN_MOTR_ID.WP_Z1].MP.dPosn[(int)EN_POSN_ID.User6 ];

            */
        }
        //------------------------------------------------------------------------
        public string  GetPosnName(int iPartId, int Axe, double dSrhPosn)
        {
            String sPosnName = "";
            int    iPosnId;
            double dPosn  ;

            for (int i = 0; i < cDEF.POSN.Dat[iPartId].m_iItemCnt; i++)
            {
                if(i>=vDEF.MAXITEM) continue;
                if (cDEF.POSN.Dat[iPartId].Set[i].m_iMotor != Axe) continue;
                iPosnId = cDEF.POSN.Dat[iPartId].Set[i].m_iPosnId;
                if (cDEF.POSN.Dat[iPartId].Set[i].m_bHomeOffset) dPosn = this[(int)Axe].MP.dPosn[iPosnId] + this[(int)Axe].m_dHomeOff;
                else                                             dPosn = this[(int)Axe].MP.dPosn[iPosnId];
                dPosn = this[(int)Axe].MP.dPosn[iPosnId];
                if(dSrhPosn == dPosn) 
                {
                    sPosnName = cDEF.POSN.Dat[iPartId].Set[i].m_sName;
                    break;
                }
            }
            return sPosnName;

        }
        //------------------------------------------------------------------------
        public double GetPosnbyCmd(EN_MOTR_ID axs, EN_COMD_ID cmd)
        {
            return this[(int)axs].GetPosToCmdId(cmd);
        }
        //------------------------------------------------------------------------
        public void ReqDrvReset(EN_MOTR_MAKER eMaker = EN_MOTR_MAKER.AJIN)
        {
            if(eMaker == EN_MOTR_MAKER.AJIN) m_iStepRst[0] = 10;
        }
        public bool  FnshDrvReset(EN_MOTR_MAKER eMaker = EN_MOTR_MAKER.AJIN)
        {
            return ((m_iStepRst[0] == 0) && (m_iStepRst[1] == 0));
        }
        public bool  AjinDrvReset()
        {
            int Axe;
            int iStrtAjinNo = 0;
            switch(m_iStepRst[0])
            {
                default: m_iStepRst[0] =0; break;
                case 10:
                    //Servo Off
                    FRM.ShowWarn(true, "Please Wait.. Motor Driver Reboot!!");
                    for (Axe = iStrtAjinNo ; Axe < m_iNumOfMotr ; Axe++) this[(int)Axe].SetServo(false);
                    m_tDrvReset .Clear();
                    m_iStepRst [0] ++;
                    break;

                case 11:
                    //Ajin Driver Reset
                    if(!m_tDrvReset.OnDelay(true, 5000)) return false;
                    for (Axe = iStrtAjinNo ; Axe < m_iNumOfMotr ; Axe++) this[(int)Axe].SetPairAxe(AXIS[Axe]._iPairAxisNo, 0);
                    AXIS[iStrtAjinNo].DevReset();
                    m_tDrvReset.Clear();
                    m_iStepRst [0] ++;
                    break;

                case 12:
                    //Set Motor Parameter
                    if(!m_tDrvReset.OnDelay(true, 1000)) return false;
                    for (Axe = iStrtAjinNo ; Axe < m_iNumOfMotr ; Axe++) this[(int)Axe].SetParameter (Axe);
                    m_tDrvReset.Clear();
                    m_iStepRst [0] ++;
                    break;

                case 13:
                    if(!m_tDrvReset.OnDelay(true, 1000)) return false;
                    for (Axe = iStrtAjinNo ; Axe < m_iNumOfMotr ; Axe++) this[(int)Axe].Reset();
                    m_tDrvReset.Clear();
                    m_iStepRst [0] ++;
                    break;

                case 14:
                    for (Axe = iStrtAjinNo ; Axe < m_iNumOfMotr ; Axe++) this[(int)Axe].SetServo(true);
                    m_iStepRst[0] = 0;
                    cDEF.EPU.Clear();
                    if ((m_iStepRst[0] == 0) && (m_iStepRst[1] == 0)) {
                        FRM.ShowWarn(false);
                        m_bNeedReboot = false;
                        }
                    return true;
            }
            return false;
        }
    }
}
