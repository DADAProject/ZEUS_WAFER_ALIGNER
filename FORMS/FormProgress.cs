using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static eMachine.cDEF;

namespace eMachine  
{
    public partial class FrmProgress : Form
    {
        public int m_iStepIndex;
        public FrmProgress()
        {
            InitializeComponent();

            this           .BackColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(160,160,160) : System.Drawing.Color.FromArgb(37 ,51 ,64 );
            pnTitle        .BackColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(238,238,238) : System.Drawing.Color.FromArgb(25 ,41 ,55 );
            lbVersion      .ForeColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(37 ,51 ,64 ) : System.Drawing.Color.FromArgb(230,230,200);
            lbPrjName      .ForeColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(37 ,51 ,64 ) : System.Drawing.Color.FromArgb(230,230,200);
            lbLoadingModule.ForeColor = FRM.UIType == EN_UI_TYPE.Light ? System.Drawing.Color.FromArgb(37 ,51 ,64 ) : System.Drawing.Color.FromArgb(230,230,200);
        }
        //------------------------------------------------------------------------
        private void FrmProgress_Load(object sender, EventArgs e)
        {
            this.tmProc.Interval = 1;
            this.tmProc.Enabled = true;
            
            //
            m_iStepIndex = 0;
 
 	        //Display Version.
            lbPrjName.Text = vDEF.sOsTitle;

            //Progress Bar
            progressBar1.Style = ProgressBarStyle.Continuous;
            if ((int)this.Tag == 0) progressBar1.Step  = progressBar1.Maximum / 12;
            else                    progressBar1.Step  = progressBar1.Maximum / 4 ;
        }
        //------------------------------------------------------------------------

        private void FrmProgress_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.tmProc.Enabled = false;
            Close();
        }

        //------------------------------------------------------------------------
        private bool ShowMsg(string Msg)
        {
            if(Msg == lbLoadingModule.Text) return true;
            lbLoadingModule . Text = Msg; 
            return false;   
        }
        //------------------------------------------------------------------------
        private void tmProgress_Tick(object sender, EventArgs e)
        {

        }
        //------------------------------------------------------------------------
        private void tmProc_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.tmProc.Enabled = false; return; }

            this.tmProc.Enabled = false;            
            
            if((int)this.Tag == 0) 
            {
                if(ProgramLoad()) m_iStepIndex++; 
            }
            else 
            {
                if(ProgramUnload()) m_iStepIndex++;
            }
            //
            progressBar1.PerformStep(); 
            this.tmProc.Enabled = true;
        }
        //------------------------------------------------------------------------
        public bool ProgramLoad()
        {
            //Simulation Mode
            bool bSim = false;

            try
            {
                switch (m_iStepIndex) 
                {
                    default : break;
                    case 0:
                        //Load Default Info. file.
                        if(!ShowMsg("Load Default Info. file.")) return false;
                        cDEF.DllInit         ();
                        cDEF.Init            ();
                        cDEF.FM.Init         ();
                        cDEF.SetUpdateInfo   (); lbVersion.Text = cDEF.FM._sVersion;
                        return true;

                    case 1: 
                        //Load MOTOR.
                        if(!ShowMsg("Initializing the Motor")) return false;
                        InitMotor     (bSim);

                        //cDEF.ResetCamHotlink();
                        return true;

                    case 2: 
                        //Load IO.
                        if (!ShowMsg("Initializing the IO")) return false;
                        IOInit(bSim);
                        
                        LampBuzzInit();
                        return true;

                    case 3: 
                        //Load Actuator.
                        if(!ShowMsg("Initializing the Cylinder")) return false;
			            ACTRInit(bSim); 
                        return true;

		            case 4: 
                        //Initial Vision  
                        if(!ShowMsg("Initializing the Vision")) return false;
                        cDEF.VISN.Init();

                        return true;

		            case 5: 
                        //Init Interface
                        if(!ShowMsg("Initializing the Interface")) return false;

                        //
                        //COMZEUS.Init(FM.EngrOptn.nServerPort); //Default : 1001
                        COMZEUS.Init(1001); //Default : 1001
                        BCR    .Connect(FM.EngrOptn.sBCRIP, FM.EngrOptn.nBCRPort);
                        
                        return true;

                    case 6: 
                        //Load Error file.
                        if(!ShowMsg("Loading the Error File")) return false;
                        EPU     .Init           (    );
                        EPU     .LoadErrDataIni (true);
                        return true;

                    case 7: 
                        //Load Manual file.
                        if(!ShowMsg("Loading the Manual File")) return false;
                        cDEF.MAN.Init(      );
                        return true;

                    case 8: 
                        //Data Map & Sequence Data.
                        if(!ShowMsg("Loading the Working Data")) return false;
                        //DM .Load         (true);
                        SEQ.LoadSys      (true);
                        SEQ.LoadWorkInfo (true);
                        LOT.LoadLot      (true);
                        return true;

                    case 9: 
                        //Load SPC Options.
                        if(!ShowMsg("Loading the SPC Data")) return false;
                        cDEF.SPC.Load        (true);
                        cDEF.SPC.InitDB      ();
                        return true;

                    case 10: 
                        //Apply Last Device..
                        if(!ShowMsg("Apply JobFile")) return false;
                        cDEF.FM.ApplyProject  (cDEF.FM._sCrntDevice);
                        return true;

                    case 11: 
                        //Finish Loading.
                        if(!ShowMsg("Finish Loading Module")) return false;

                        cDEF.EPU    .Clear      (); //Clear All Error.
                        cDEF.LOG    .KillPast   (); //Kill Past Log
                        cDEF.MOTR   .Reset      (); //JUNG/220125
                        cDEF.MOTR   .SetServo   (true); 
                        cDEF.TH     .StartThread();
                        cDEF.SEQ    .Reset      ();
                        cDEF.LOG    .Trace      ("Program Start");

                        //
                        Close();
                        return true;
                }
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("FrmProgress. ProgramLoad :", ex);
                Debug.WriteLine("[EXCEPTION] FrmProgress.ProgramLoad : Message - " + ex.Message);
                return true;
            }

            return false;
        }
        //------------------------------------------------------------------------
        public bool ProgramUnload()
        {
            try 
            {
                switch (m_iStepIndex) 
                {
                    default : break;

                    case 0:
                        if (!ShowMsg("Finish Unload Module")) return false;

                        LOG.Trace("Program Close");

                        //cDEF.VISN.Final();
                        MOTR.SetServo    (false); 
                        TH  .EndThread   ();
                        MOTR.CloseMotor  ();
                        IO  .Close       ();

                        LOG.KillThread(); 

                        
                        return true;

                    case 1: 
                        if(!ShowMsg("Save Lot Information" )) return false;
                        LOT.LoadLot      (false);
                        return true;

                    case 2: 
                        if(!ShowMsg("Save Work Information")) return false;
                        //DM .Load         (false);
                        FM .Load         (false);
                        SEQ.LoadWorkInfo (false);

                        //Save Error File 
                        if (EPU._bNeedSave)
                        {
                            EPU.LoadErrDataIni(false);
                        }
                        return true;

                    case 3:
                        COMZEUS.Close();
                        cDEF.VISN.Final();
                        BCR    .DisConnect();

                        //COMASM.DisConnect();
                        
                        //Aligner     .Close();
                        //TempAutonics.Close();
                        //TOQMOTR     .Close(); //Torque Motor

                        this.tmProc.Enabled = false;
                        Close();
                        return true;


                }
            }
            catch (Exception ex)
            {
                cDEF.LOG.ExceptionTrace("FrmProgress. ProgramUnload " , ex);
                System.Diagnostics.Debug.WriteLine("[EXCEPTION] FrmProgress.ProgramUnload : Message - " + ex.Message);
                return true;
            }

            return false;    
        }
    }

}
