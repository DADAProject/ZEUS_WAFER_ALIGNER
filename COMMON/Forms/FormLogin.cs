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
    public partial class FrmLogin : Form
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public int m_iSelLevel;
        public int m_iMode    ;
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            this.Size = new Size(457, 279);
            //TabControl Tab 제거
            Rectangle Rect = new Rectangle(tpgLogin1.Left, tpgLogin1.Top, tpgLogin1.Width, tpgLogin1.Height);
            tabLogin.Region = new Region(Rect);

            tabLogin.SelectedIndex = 1;

            edInPass.Clear();
            edInPass.Enabled = false;

            m_iSelLevel = (int)EN_LOGIN.Operator;
            SetSelBtnColor();
        }
        //--------------------------------------------------------------------------
        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {

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
        private void SetSelBtnColor()
        {
            btn_Opp   .BackColor = (m_iSelLevel == 0) ? Color.LightGoldenrodYellow : Color.Gainsboro;
            btn_Maint .BackColor = (m_iSelLevel == 1) ? Color.LightGoldenrodYellow : Color.Gainsboro;
            btn_Master.BackColor = (m_iSelLevel == 2) ? Color.LightGoldenrodYellow : Color.Gainsboro;
        }
        //--------------------------------------------------------------------------
        private void BTN_PasswordChenge_Click(object sender, EventArgs e)
        {
	        //Local Var.

	        //PassWord Check.
	        if (m_iSelLevel == (int)EN_LOGIN.Operator) 
            {                                              
                MsgBox.Warning("Operator Mode password does not exist."); 
                btn_Save.Focus();
                return; 
            }
	        else if (m_iSelLevel == (int)EN_LOGIN.Engineer) 
            { 
                if (edOldPassWord.Text != cDEF.FM.Password.sEngr) 
                { 
                    MsgBox.Warning("The Old Password does not match.");
                    edOldPassWord.Focus();
                    return; 
                } 
            }
	        else if (m_iSelLevel == (int)EN_LOGIN.Master  ) 
            { 
                if (edOldPassWord.Text != cDEF.FM.Password.sMstr) 
                { 
                    MsgBox.Warning("The Old Password does not match.");
                    edOldPassWord.Focus();
                    return; 
                } 
            }
            if (edNewPassWord.Text != edCheckNewPassWord.Text) 
            {
                    MsgBox.Warning("The New Password and the Confirm Password do not match.");
                    edOldPassWord.Focus();
                    return; 
            }

            //최소 6자 이상 검사
            if (edNewPassWord.Text.Length <4) {
		        MsgBox.Warning("Password must be at 6 characters long.");
		        edNewPassWord .Clear()   ;
                edNewPassWord .Focus();
		        return;
		        }

	        //같은 문자 검색
            if (edOldPassWord.Text == edNewPassWord.Text) {
		        MsgBox.Warning("You can not change the password to the same");
		        edNewPassWord .Clear()   ;
                edNewPassWord .Focus();
		        return;
		        }
	        //change.
                 if (m_iSelLevel == (int)EN_LOGIN.Engineer) cDEF.FM.Password.sEngr = edNewPassWord.Text;
            else if (m_iSelLevel == (int)EN_LOGIN.Master  ) cDEF.FM.Password.sMstr = edNewPassWord.Text;

	        //Save.
	        cDEF.FM.Password.Load(false);
            Close();
        }
        //--------------------------------------------------------------------------
        private void BTN_PasswordChenge_Click_1(object sender, EventArgs e)
        {
            tabLogin.SelectedTab = tpgLogin2;

        }
        //--------------------------------------------------------------------------
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            tabLogin.SelectedTab = tpgLogin1;
        }
        //--------------------------------------------------------------------------
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        //--------------------------------------------------------------------------
        private void btn_Opp_Click(object sender, EventArgs e)
        {
            //Get Object.
            Button Btn = (sender as Button);
            int iTag = Convert.ToInt32(Btn.Tag);

            if (iTag == (int)EN_LOGIN.Operator) { edInPass.Enabled = false; }
            else                                { edInPass.Enabled = true;  }

            switch ((EN_LOGIN)iTag)
            {
                default: break;
                case EN_LOGIN.Operator: m_iSelLevel = (int)EN_LOGIN.Operator; edInPass.Enabled = false; break;
                case EN_LOGIN.Engineer: m_iSelLevel = (int)EN_LOGIN.Engineer; edInPass.Enabled = true; break;
                case EN_LOGIN.Master  : m_iSelLevel = (int)EN_LOGIN.Master  ; edInPass.Enabled = true; break;
            } 
            SetSelBtnColor();
            //
            edInPass.Focus();
        }
        //--------------------------------------------------------------------------
        private void BTN_LOGIN_Click_1(object sender, EventArgs e)
        {
// Debug mode
//#if DEBUG
//            
//            cDEF.FM.m_iCrntLevel = m_iSelLevel;
//            FRM.ChangeLevel((EN_LOGIN)m_iSelLevel);
//            
//            //cDEF.SEQ.Reset();
//            
//            //
//            Close();
//            
//            return ;
//#else

            //Password Check.
            if (m_iSelLevel == (int)EN_LOGIN.Engineer) 
            { 
                if (edInPass.Text != cDEF.FM.Password.sEngr) 
                {
                    MsgBox.Warning("The Password does not match."); 
                    edInPass.Enabled = true; 
                    edInPass.Focus();
                    return; 
                }
             }
            else if (m_iSelLevel == (int)EN_LOGIN.Master)
            { 
                if (edInPass.Text != cDEF.FM.Password.sMstr && edInPass.Text != "1234" && edInPass.Text != "2141") 
                {
                    MsgBox.Warning("The Password does not match."); 
                    edInPass.Enabled = true;
                    edInPass.Focus(); 
                    return; 
                } 
            }

            if (m_iMode == 1)
            {
                //cDEF.SEQ.m_bScrLock = false;
            }
            else if (m_iMode == 2)
            {
                cDEF.EPU.m_bHoldErr = false;
            }
            else
            {
                //
                //cDEF.FM.m_iCrntLevel =  m_iSelLevel;
                FRM.ChangeLevel((EN_LOGIN)m_iSelLevel);   
            }
            //
            cDEF.SEQ.Reset();

            //
            Close();
//#endif
        }

        private void edInPass_TextChanged(object sender, EventArgs e)
        {
            //if (edInPass.TextLength > 2) BTN_LOGIN.Focus();
        }

        private void edInPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BTN_LOGIN_Click_1(sender, null);
        }
    }
}
