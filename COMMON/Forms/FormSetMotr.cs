using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    public partial class FrmSetMotr : Form
    {
		Timer      m_TimerFI   = new Timer();

        public int m_iSelMotr;
        public FrmSetMotr()
        {
            InitializeComponent();
            //
            this.Opacity = 0;  //first the opacity is 0
        }
        //--------------------------------------------------------------------------
        private void FrmSetMotr_Load(object sender, EventArgs e)
        {
            //Fade In.            
            m_TimerFI.Interval = 10; //we'll increase the opacity every 10ms
            m_TimerFI.Tick += new EventHandler(FadeInForm); //this calls the function that changes opacity
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();

	        if(m_iSelMotr<0 || m_iSelMotr>=cDEF.MOTR._iNumOfMotr) return;
	        lbTitle.Text = cDEF.POSN.GetMotorName(m_iSelMotr);

	        tabMotrSet.SelectedTab = tabPage1;

	        UpdateMasterMotorSet(true);
        }
        //--------------------------------------------------------------------------
        private void FrmSetMotr_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void FrmSetMotr_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    //cancel the event so the form won't be closed

            m_TimerFI.Tick += new EventHandler(fadeOutForm);  //this calls the fade out function
            m_TimerFI.Enabled = true;
            m_TimerFI.Start();

            if (this.Opacity == 0)  //if the form is completly transparent
                e.Cancel = false;   //resume the event - the program can be closed
        }
        //--------------------------------------------------------------------------
        private void FadeInForm(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)  
            {
                m_TimerFI.Stop();   //this stops the timer if the form is completely displayed
                m_TimerFI.Tick -= new EventHandler(FadeInForm); 
            }
            else
                this.Opacity += 0.05;
        }
        //--------------------------------------------------------------------------
        private void fadeOutForm(object sender, EventArgs e)
        {
            if (this.Opacity <= 0)     //check if opacity is 0
            {
                m_TimerFI.Stop();    //if it is, we stop the timer
                m_TimerFI.Tick -= new EventHandler(fadeOutForm);
                Close();   //and we try to close the form
            }
            else
                this.Opacity -= 0.05;
        }
        //--------------------------------------------------------------------------
        private void lbTitle_MouseDown(object sender, MouseEventArgs e)
        {
             var s = sender as Label;
             if(this == null) return;
             if(s    == null) return;
             s.Tag = new Point(e.X, e.Y);
        }
        //--------------------------------------------------------------------------
        private void lbTitle_MouseMove(object sender, MouseEventArgs e)
        {
            var s = sender as Label;
            if(this  == null) return;
            if(s     == null) return;
            if(s.Tag == null) return;
            
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;

            this.Left = this.Left + (e.X - ((Point)s.Tag).X);
            this.Top  = this.Top  + (e.Y - ((Point)s.Tag).Y);
        }
        //--------------------------------------------------------------------------

        private void btnNo_Click(object sender, EventArgs e)
        {
            Close();
        }
        //--------------------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbSig16.Text))
            {
                MsgBox.Warning("Home Type Is Empty");
                return;
            }

            //Check Running Status.
            if (m_iSelMotr<0 || m_iSelMotr>=cDEF.MOTR._iNumOfMotr) return;
	        UpdateMasterMotorSet (false );
	        cDEF.MOTR[m_iSelMotr].LoadOptn(false);
	        UpdateMasterMotorSet (true  );

	        //Set Axis.
	        cDEF.MOTR[m_iSelMotr].SetParameter   (m_iSelMotr );
        }
        //--------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            cDEF.MOTR[m_iSelMotr].m_dGR_Pulse = 1000; //8192
	        if(cDEF.MOTR[m_iSelMotr].m_iMotrType == 1) cDEF.MOTR[m_iSelMotr].m_dGR_Pulse = 1000;


	        cDEF.MOTR[m_iSelMotr].m_dGR_Disp        =    1;
	        cDEF.MOTR[m_iSelMotr].m_iHomeDir        =    0;
	        cDEF.MOTR[m_iSelMotr].m_dMinPosn        =  -10;
	        cDEF.MOTR[m_iSelMotr].m_dMaxPosn        = 1000;
	        cDEF.MOTR[m_iSelMotr].m_dMaxVel         = 2000;
	        cDEF.MOTR[m_iSelMotr].m_dMinAcc         = 0.01;
	        cDEF.MOTR[m_iSelMotr].m_dMinDec         = 0.01;
	        cDEF.MOTR[m_iSelMotr].m_dHomeOff        =    0;
	        cDEF.MOTR[m_iSelMotr].m_iPulseOut       =    8;
	        cDEF.MOTR[m_iSelMotr].m_iEncInputLevel  =    3;
	        cDEF.MOTR[m_iSelMotr].m_dPartialPos     =    1;
	        cDEF.MOTR[m_iSelMotr].m_iIntpAxisNo     =   -1;
	        cDEF.MOTR[m_iSelMotr].m_iPairAxisNo     =   -1;

	        cDEF.MOTR[m_iSelMotr].m_iPackType       =    0;
	        cDEF.MOTR[m_iSelMotr].m_iAutoResp       =    0;
	        cDEF.MOTR[m_iSelMotr].m_iHomeOptUse     =    0;
	        cDEF.MOTR[m_iSelMotr].m_iHomeType       =    0;
            cDEF.MOTR[m_iSelMotr].m_iEMGLevel       =    0;
	        cDEF.MOTR[m_iSelMotr].m_iMotrKind       =    0;
	        cDEF.MOTR[m_iSelMotr].m_iNoUseMotr      =    0;
	        cDEF.MOTR[m_iSelMotr].m_iSONLevel       =    1;
	        cDEF.MOTR[m_iSelMotr].m_iHomeLevel      =    1;
	        cDEF.MOTR[m_iSelMotr].m_iInpLevel       =    0;
	        cDEF.MOTR[m_iSelMotr].m_iAlarmLevel     =    0;
	        cDEF.MOTR[m_iSelMotr].m_iNoUseEnc       =    0;
	        cDEF.MOTR[m_iSelMotr].m_iNoUseHomeSen   =    0;
	        cDEF.MOTR[m_iSelMotr].m_iMoveDirection  =    0;
	        cDEF.MOTR[m_iSelMotr].m_iProcessDir     =    1;
	        cDEF.MOTR[m_iSelMotr].m_iUseCam         =    0;
            cDEF.MOTR[m_iSelMotr].m_bUseOverride    = false; 
            cDEF.MOTR[m_iSelMotr].m_bUseTorque      = false; 
	        cDEF.MOTR[m_iSelMotr].m_iHomeSignal     =    2; //Home Sensor

	        cDEF.MOTR[m_iSelMotr].LoadOptn(false);
	        UpdateMasterMotorSet (true  );

	        //Set Axis.
	        cDEF.MOTR[m_iSelMotr].SetParameter   (m_iSelMotr );
        }
        //---------------------------------------------------------------------------
        void UpdateMasterMotorSet(bool toTable)
        {
	        //Check Run.
	        if(m_iSelMotr<0 || m_iSelMotr>=cDEF.MOTR._iNumOfMotr) return;

	        //Set buffer.
	        if (toTable) {
		        edPluseCnt     . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dGR_Pulse     );
		        edPulseDist    . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dGR_Disp      );
		        edMinPos       . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dMinPosn      );
		        edMaxPos       . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dMaxPosn      );
		        edMaxVel       . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dMaxVel       );
		        edMinAcc       . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dMinAcc       );
		        edMinDec       . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dMinDec       );
		        edHomeOff      . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dHomeOff      );
		        edPulseMeth    . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_iPulseOut     );
		        edEncInLvl     . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_iEncInputLevel);
		        edPartialPos   . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_dPartialPos   );
		        edIntpAxisNo   . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_iIntpAxisNo   );
		        edPairAxisNo   . Text      = Convert.ToString(cDEF.MOTR[m_iSelMotr].m_iPairAxisNo   );

                edToqWAddr     . Text      = cDEF.MOTR[m_iSelMotr].m_sToqWAddr             ;
                edToqRAddr     . Text      = cDEF.MOTR[m_iSelMotr].m_sToqRAddr             ;


		        rbMType1       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMotrType   == 0    ); 
		        rbMType2       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMotrType   == 1    ); 
		        rbMType3       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMotrType   == 2    ); 
                rbMType4       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMotrType   == 3    ); 

		        rbPackType1    . Checked   = (cDEF.MOTR[m_iSelMotr].m_iPackType   == 0    ); 
		        rbPackType2    . Checked   = (cDEF.MOTR[m_iSelMotr].m_iPackType   == 1    ); 
		        rbPackType3    . Checked   = (cDEF.MOTR[m_iSelMotr].m_iPackType   == 2    ); 
		        rbPackType4    . Checked   = (cDEF.MOTR[m_iSelMotr].m_iPackType   == 3    ); 

		        rbAResp1       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iAutoResp   == 0    ); 
		        rbAResp2       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iAutoResp   == 1    ); 
		        rbAResp3       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iAutoResp   == 2    ); 

		        rbMKind1       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMotrKind   == 0    ); 
		        rbMKind2       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMotrKind   == 1    ); 

		        rbServoType1   . Checked   = (cDEF.MOTR[m_iSelMotr].m_iServoType  == 0    ); 
		        rbServoType2   . Checked   = (cDEF.MOTR[m_iSelMotr].m_iServoType  == 1    ); 

		        rbSigOn1       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iNoUseMotr  == 0    ); 
		        rbSigOf1       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iNoUseMotr  == 1    ); 
                rbSigOn2       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iSONLevel       == 0);
		        rbSigOf2       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iSONLevel       == 1);
                rbSigOn3       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iHomeLevel      == 0);
		        rbSigOf3       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iHomeLevel      == 1);
                
                cbPLimit . SelectedIndex   = cDEF.MOTR[m_iSelMotr].m_iPosLimitLevel;
                cbNLimit . SelectedIndex   = cDEF.MOTR[m_iSelMotr].m_iNegLimitLevel;


		        rbSigOn6       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iInpLevel       == 0);
		        rbSigOf6       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iInpLevel       == 1);
                //rbSigOn7       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iAlarmLevel     == 0);
                //rbSigOf7       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iAlarmLevel     == 1);
		        rbSigOn9       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iNoUseEnc       == 0);
		        rbSigOf9       . Checked   = (cDEF.MOTR[m_iSelMotr].m_iNoUseEnc       == 1);
                rbSigOn10      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iNoUseHomeSen   == 0);
                rbSigOf10      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iNoUseHomeSen   == 1);
                rbSigOn11      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMoveDirection  == 0);
                rbSigOf11      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iMoveDirection  == 1);
                rbSigOn12      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iProcessDir     == 0);
		        rbSigOf12      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iProcessDir     == 1);
		        rbSigOn13      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iEncPulse       == 0);
                rbSigOf13      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iEncPulse       == 1);
                rbSigOn14      . Checked   =  cDEF.MOTR[m_iSelMotr].m_bSetScurve           ;
                rbSigOf14      . Checked   = !cDEF.MOTR[m_iSelMotr].m_bSetScurve      	   ;	        
                rbSigOn15      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iHomeOptUse     == 1);
		        rbSigOf15      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iHomeOptUse     == 0);
		        cbSig16        . SelectedIndex = cDEF.MOTR[m_iSelMotr].m_iHomeType         ;
                cbSig17        . SelectedIndex = cDEF.MOTR[m_iSelMotr].m_iAlarmLevel       ;


                rbSigOn17      . Checked   =  cDEF.MOTR[m_iSelMotr].m_bUseRingCounter      ;
                rbSigOf17      . Checked   = !cDEF.MOTR[m_iSelMotr].m_bUseRingCounter      ;

		        rbSigOn18      . Checked   =  cDEF.MOTR[m_iSelMotr].m_bUseOverride         ;
                rbSigOf18      . Checked   = !cDEF.MOTR[m_iSelMotr].m_bUseOverride         ;


                rbSigOn20      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iHomeDir        == 0);
		        rbSigOf20      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iHomeDir        == 1);

		        rbCamUse0      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iUseCam         == 1);
		        rbCamUse1      . Checked   = (cDEF.MOTR[m_iSelMotr].m_iUseCam         == 0);
                rbUseTorque0   . Checked   =  cDEF.MOTR[m_iSelMotr].m_bUseTorque           ;
                rbUseTorque1   . Checked   = !cDEF.MOTR[m_iSelMotr].m_bUseTorque           ;

		        cbHomeSignal  . SelectedIndex = cDEF.MOTR[m_iSelMotr].m_iHomeSignal        ;
                cbZPhase      . SelectedIndex = cDEF.MOTR[m_iSelMotr].m_iHomeZPhase        ;


                if(cDEF.MOTR[m_iSelMotr].m_sComPort == "") cDEF.MOTR[m_iSelMotr].m_sComPort = "COM1";
                ed232Port      . Text      = cDEF.MOTR[m_iSelMotr].m_sComPort              ;

            }
	        else 
            {
		        cDEF.MOTR[m_iSelMotr].m_dGR_Pulse       = Convert.ToDouble(edPluseCnt  . Text);
		        cDEF.MOTR[m_iSelMotr].m_dGR_Disp        = Convert.ToDouble(edPulseDist . Text);
		        cDEF.MOTR[m_iSelMotr].m_dMinPosn        = Convert.ToDouble(edMinPos    . Text);
		        cDEF.MOTR[m_iSelMotr].m_dMaxPosn        = Convert.ToDouble(edMaxPos    . Text);
		        cDEF.MOTR[m_iSelMotr].m_dMaxVel         = Convert.ToDouble(edMaxVel    . Text);
		        cDEF.MOTR[m_iSelMotr].m_dMinAcc         = Convert.ToDouble(edMinAcc    . Text);
		        cDEF.MOTR[m_iSelMotr].m_dMinDec         = Convert.ToDouble(edMinDec    . Text);
		        cDEF.MOTR[m_iSelMotr].m_dHomeOff        = Convert.ToDouble(edHomeOff   . Text);
		        cDEF.MOTR[m_iSelMotr].m_iPulseOut       = Convert.ToInt32 (edPulseMeth . Text);
		        cDEF.MOTR[m_iSelMotr].m_iEncInputLevel  = Convert.ToInt32 (edEncInLvl  . Text);
		        cDEF.MOTR[m_iSelMotr].m_dPartialPos     = Convert.ToDouble(edPartialPos. Text);
		        cDEF.MOTR[m_iSelMotr].m_iIntpAxisNo     = Convert.ToInt32 (edIntpAxisNo. Text);
		        cDEF.MOTR[m_iSelMotr].m_iPairAxisNo     = Convert.ToInt32 (edPairAxisNo. Text);

                cDEF.MOTR[m_iSelMotr].m_sToqWAddr       = edToqWAddr     .Text                ;
                cDEF.MOTR[m_iSelMotr].m_sToqRAddr       = edToqRAddr     .Text                ;


		        if(rbMType1       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMotrType   = 0    ; 
		        if(rbMType2       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMotrType   = 1    ; 
		        if(rbMType3       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMotrType   = 2    ; 
                if(rbMType4       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMotrType   = 3    ; 
                                              
		        if(rbPackType1    . Checked   ) cDEF.MOTR[m_iSelMotr].m_iPackType   = 0    ; 
		        if(rbPackType2    . Checked   ) cDEF.MOTR[m_iSelMotr].m_iPackType   = 1    ; 
		        if(rbPackType3    . Checked   ) cDEF.MOTR[m_iSelMotr].m_iPackType   = 2    ; 
		        if(rbPackType4    . Checked   ) cDEF.MOTR[m_iSelMotr].m_iPackType   = 3    ; 
                                              
		        if(rbAResp1       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iAutoResp   = 0    ; 
		        if(rbAResp2       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iAutoResp   = 1    ; 
		        if(rbAResp3       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iAutoResp   = 2    ; 
                                              
		        if(rbMKind1       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMotrKind   = 0    ; 
		        if(rbMKind2       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMotrKind   = 1    ; 
                                              
		        if(rbServoType1   . Checked   ) cDEF.MOTR[m_iSelMotr].m_iServoType  = 0    ; 
		        if(rbServoType2   . Checked   ) cDEF.MOTR[m_iSelMotr].m_iServoType  = 1    ; 
                                              
		        if(rbSigOn1       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iNoUseMotr  = 0    ; 
		        if(rbSigOf1       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iNoUseMotr  = 1    ;
                if(rbSigOn2       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iSONLevel       = 0;
		        if(rbSigOf2       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iSONLevel       = 1;
                if(rbSigOn3       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iHomeLevel      = 0;
		        if(rbSigOf3       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iHomeLevel      = 1;


                cDEF.MOTR[m_iSelMotr].m_iPosLimitLevel  = cbPLimit . SelectedIndex;
                cDEF.MOTR[m_iSelMotr].m_iNegLimitLevel  = cbNLimit . SelectedIndex;

		        if(rbSigOn6       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iInpLevel       = 0;
		        if(rbSigOf6       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iInpLevel       = 1;
                //if(rbSigOn7       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iAlarmLevel     = 0;
                //if(rbSigOf7       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iAlarmLevel     = 1;
		        if(rbSigOn9       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iNoUseEnc       = 0;
		        if(rbSigOf9       . Checked   ) cDEF.MOTR[m_iSelMotr].m_iNoUseEnc       = 1;
                if(rbSigOn10      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iNoUseHomeSen   = 0;
                if(rbSigOf10      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iNoUseHomeSen   = 1;
                if(rbSigOn11      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMoveDirection  = 0;
                if(rbSigOf11      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iMoveDirection  = 1;
                if(rbSigOn12      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iProcessDir     = 0;
		        if(rbSigOf12      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iProcessDir     = 1;
		        if(rbSigOn13      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iEncPulse       = 0;
                if(rbSigOf13      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iEncPulse       = 1;
                if(rbSigOn14      . Checked   ) cDEF.MOTR[m_iSelMotr].m_bSetScurve      = true ;
                if(rbSigOf14      . Checked   ) cDEF.MOTR[m_iSelMotr].m_bSetScurve      = false;
                if(rbSigOn15      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iHomeOptUse     = 1;
		        if(rbSigOf15      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iHomeOptUse     = 0;
                cDEF.MOTR[m_iSelMotr].m_iHomeType   = (int)cbSig16     .SelectedIndex      ;
                cDEF.MOTR[m_iSelMotr].m_iAlarmLevel = (int)cbSig17     .SelectedIndex      ;

                if (rbCamUse0     . Checked   ) cDEF.MOTR[m_iSelMotr].m_iUseCam         = 1;
		        if(rbCamUse1      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iUseCam         = 0;
		        if(rbUseTorque0   . Checked   ) cDEF.MOTR[m_iSelMotr].m_bUseTorque      = true ;
		        if(rbUseTorque1   . Checked   ) cDEF.MOTR[m_iSelMotr].m_bUseTorque      = false;

		        if(rbSigOn20      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iHomeDir        = 0;
		        if(rbSigOf20      . Checked   ) cDEF.MOTR[m_iSelMotr].m_iHomeDir        = 1;

              
                if(rbSigOn17      . Checked   ) cDEF.MOTR[m_iSelMotr].m_bUseRingCounter = true ;
                if(rbSigOf17      . Checked   ) cDEF.MOTR[m_iSelMotr].m_bUseRingCounter = false;

                if(rbSigOn18      . Checked   ) cDEF.MOTR[m_iSelMotr].m_bUseOverride    = true ;
                if(rbSigOf18      . Checked   ) cDEF.MOTR[m_iSelMotr].m_bUseOverride    = false;                


                cDEF.MOTR[m_iSelMotr].m_sComPort = ed232Port      . Text.Trim().ToUpper();


		        cDEF.MOTR[m_iSelMotr].m_iHomeSignal = cbHomeSignal. SelectedIndex;
                cDEF.MOTR[m_iSelMotr].m_iHomeZPhase = cbZPhase    . SelectedIndex;
		        
            }
        }
    }
}
