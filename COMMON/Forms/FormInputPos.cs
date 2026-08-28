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
    public partial class FrmInputPos : Form
    {
        public double m_dValue ;
	    public double m_dMaxVal;
	    public double m_dMinVal;
	    public int    m_iDigit ;
	    public bool   m_bHomeOffset;
	    public bool   m_bChMinMax;
	    public String m_sTitle;

	    public int m_iMotor;


        public FrmInputPos()
        {
            InitializeComponent();
        }
        //--------------------------------------------------------------------------
        private void FrmInputPos_Load(object sender, EventArgs e)
        {
	        lbTitle    .Text     = m_sTitle;
	        edValue    .Text     = Convert.ToString(m_dValue );
	        edOldPos   .Text     = Convert.ToString(m_dValue );
	        edMaxPos   .Text     = Convert.ToString(m_dMaxVal);
	        edMinPos   .Text     = Convert.ToString(m_dMinVal);
	        if(m_bChMinMax)
	        {
		        edMinPos.ReadOnly = false;
		        edMaxPos.ReadOnly = false;
	        }
	        else
	        {
		        edMinPos.ReadOnly = true;
		        edMaxPos.ReadOnly = true;
	        }
	        Timer_Update.Enabled = true;
        }
        //--------------------------------------------------------------------------
        private void FrmInputPos_FormClosed(object sender, FormClosedEventArgs e)
        {
            Timer_Update.Enabled = false;
            Close();
         
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
        private void btnNo0_MouseDown(object sender, MouseEventArgs e)
        {
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);
            InputNum(iTag); 
        }
        //--------------------------------------------------------------------------
        private void btnDot_Click(object sender, EventArgs e)
        {
            InputDot ();
        }
        //--------------------------------------------------------------------------
        private void Timer_Update_Tick(object sender, EventArgs e)
        {
            if(!this.Visible) {this.Timer_Update.Enabled = false; return; }

	        //Local Var.
	        //String Str  ;
	        //Real Time Position.

	        if(m_iMotor<0 || m_iMotor>=cDEF.MOTR._iNumOfMotr) return;
            
            //Str = string.Format("{0:F4}" , cDEF.MOTR[m_iMotor].GetCmdPos());
	        edMotrPos.Text = cDEF.MOTR[m_iMotor].GetEncPos().ToString("F3");
        }
        //--------------------------------------------------------------------------
        private void btnPN_Click(object sender, EventArgs e)
        {
            InputPlus();
        }
        //--------------------------------------------------------------------------
        private void btnGetMotr_Click(object sender, EventArgs e)
        {
            //Local Var.
	        String Str   ;
	        double     dValue;

	        if (m_iMotor < 0 || m_iMotor>=cDEF.MOTR._iNumOfMotr) return;

            //Real Time Position.
            //if(m_bHomeOffset) dValue = cDEF.MOTR[m_iMotor].GetAbsEncPos();
            //else              dValue = cDEF.MOTR[m_iMotor].GetCmdPos   ();
            dValue = cDEF.MOTR[m_iMotor].GetEncPos();
            Str = string.Format("{0:F4}" , dValue);
	        edMotrPos  . Text = Str;
	        edValue    . Text = Str;        
        }
        //--------------------------------------------------------------------------
        private void btnBS_Click(object sender, EventArgs e)
        {
        	InputDel ();
        }
        //--------------------------------------------------------------------------
        private void btnCls_Click(object sender, EventArgs e)
        {
            edValue.Text     = "0";
        }
        //--------------------------------------------------------------------------
        private void btnSetPos_Click(object sender, EventArgs e)
        {
	        //if(CheckValue()) {
                m_dValue = Convert.ToDouble(edValue  .Text);
                btnSetPos.DialogResult = DialogResult.Yes; 
                Close();  
            //}
	               
        }
        //--------------------------------------------------------------------------
        private void btnPlus_Click(object sender, EventArgs e)
        {
	        String Str;

	        double  dVal     = Convert.ToDouble(edValue  .Text);
	        double  dIncDec  = Convert.ToDouble(edIncDec .Text);

	        dVal += dIncDec;
            Str = string.Format("{0:F4}" , dVal);
	        edValue  .Text = Str;        
        }
        //--------------------------------------------------------------------------
        private void btnMinus_Click(object sender, EventArgs e)
        {
 	        String Str;

	        double  dVal     = Convert.ToDouble(edValue  .Text);
	        double  dIncDec  = Convert.ToDouble(edIncDec .Text);

	        dVal -= dIncDec;
            Str = string.Format("{0:F4}" , dVal);

	        edValue  .Text = Str;       
        }
        //---------------------------------------------------------------------------
        bool CheckValue()
        {
	        String sTemp;
	        double  dVal     = Convert.ToDouble(edValue .Text);
	        double  dMinVal  = Convert.ToDouble(edMinPos.Text);
	        double  dMaxVal  = Convert.ToDouble(edMaxPos.Text);

	        if(dVal>=dMinVal && dVal<=dMaxVal) return true;
		    
            sTemp = string.Format(" Input Between  {0:F4}  ~  {0:F4} ",m_dMinVal, m_dMaxVal);
		    MsgBox.Warning(sTemp);
	        edValue.Focus();
            return false;
        }
        //---------------------------------------------------------------------------
        void InputNum (int Num)
        {
	        string sKey = "";
	        String StrVal = edValue.Text;
	        int dPos = StrVal.IndexOf(".",0);
	        if(StrVal == "0") StrVal = "";
	        if(dPos>0)
	        {
		        if(StrVal.Length > dPos+m_iDigit) return;
	        }
	        if      (Num ==  0)     sKey = "0";
	        else if (Num ==  1)     sKey = "1";
	        else if (Num ==  2)     sKey = "2";
	        else if (Num ==  3)     sKey = "3";
	        else if (Num ==  4)     sKey = "4";
	        else if (Num ==  5)     sKey = "5";
	        else if (Num ==  6)     sKey = "6";
            else if (Num ==  7)     sKey = "7";
	        else if (Num ==  8)     sKey = "8";
	        else if (Num ==  9)     sKey = "9";
	        StrVal += sKey;
	        edValue.Text = StrVal;
	        //edValue.SelStart = edValue.Text.Length();
        }
        //---------------------------------------------------------------------------
        void InputPlus()
        {
	        String StrVal = edValue.Text;
            int iMinusPos = StrVal.IndexOf("-");
	        if(iMinusPos >= 0) StrVal = StrVal.Remove(0, 1); //Substring(iMinusPos+1, StrVal.Length-iMinusPos-1);
	        else                        StrVal = "-" + StrVal;
	        edValue.Text = StrVal;
	        //edValue.SelStart = edValue.Text.Length();
        }
        //---------------------------------------------------------------------------
        void InputDel ()
        {
	        String StrVal = edValue.Text;
	        if(StrVal == "0") return;
	        int len=StrVal.Length;
	        if (len > 0)
	        {
	           StrVal = StrVal.Substring(0, len-1);
            }
	        if(StrVal == "") StrVal = "0";
	        edValue.Text = StrVal;
        }
        //---------------------------------------------------------------------------
        void InputDot ()
        {
	        String StrVal = edValue.Text;
	        if(m_iDigit == 0) return;
            int i = StrVal.IndexOf(".");
	        if(StrVal.IndexOf(".") >= 0) return;
	        edValue.Text = StrVal+".";
        }
        //---------------------------------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            btnClose.DialogResult = DialogResult.No;
        }


    }
}
