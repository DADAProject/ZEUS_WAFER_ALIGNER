using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Diagnostics;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TKaBit                                                           */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TKaBit
    {

        public bool DifuFlag;
        public bool Enabled ;
        public int  Axis    ;
        public int  Tag     ;


        public bool On      ;
        public bool Off     ;

        public TKaBit ()
        {
            Clear();
            Axis = 0;            
        }
        ~TKaBit(){}

        public void  Clear  ()
        {
            On      = false;
            Off     = true ;
            Enabled = true ;
            Tag     = 0    ;
        }
        //--------------------------------------------------------------------------
        public void  Out    (bool SeqCondition)
        {
            //if (!Enabled) return;
            if (SeqCondition) { On = true; Off = false; }
            else { On = false; Off = true; }
        }
        //--------------------------------------------------------------------------
        public void  Flk    () // Repeat On/Off.
        {
            if (On) Reset();
            else    Set();
        }
        //--------------------------------------------------------------------------
        public void  KeepOn ()
        {
            Enabled = true;
            Set();
            Enabled = false;
        }
        //--------------------------------------------------------------------------
        public void  KeepOff()
        {
            Enabled = true;
            Reset();
            Enabled = false;
        }
        //--------------------------------------------------------------------------
        public void  Set    ()
        {
            Out(true);
        }
        //--------------------------------------------------------------------------
        public void  Reset    ()
        {
            Out(false);
            Clear();
        }
        //--------------------------------------------------------------------------
        public void  Difu   (int Condition    )
        {
            if (!On)
            {
                if (Condition==1)
                {
                    if (!DifuFlag)
                    {
                        On = true;
                        Off = false;
                        DifuFlag = true;
                    }
                }
            }
            else
            {
                On = false;
                Off = true;
            }

            if (Condition==0) DifuFlag = false;
        }
    };


    /***************************************************************************/
    /* Class: TActuator                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TActuator:TKaBit
    {
        const int SKIP4  =   0xffff ;
        const int SKIP5  =   0xfffff;

        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`
        TOnDelayTimer tFwdTimeOutTimer = new TOnDelayTimer();
        TOnDelayTimer tBwdTimeOutTimer = new TOnDelayTimer();
        TOnDelayTimer tFwdOnDelayTimer = new TOnDelayTimer();
        TOnDelayTimer tBwdOnDelayTimer = new TOnDelayTimer();
 
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        //String.
        string Name;
        string Comt;

        //I/O Index.
        int    m_xfwdId   ;  //FWD Sensor Address.
        int    m_xbwdId   ;  //BWD Sensor Address.
        int    m_yfwdId   ;  //FWD Output Address.
        int    m_ybwdId   ;  //BWD Output Address.
        int    m_xfwdIndex;  //FWD Sensor Index No.
        int    m_xbwdIndex;  //BWD Sensor Index No.
        int    m_yfwdIndex;  //FWD Output Index No.
        int    m_ybwdIndex;  //BWD Output Index No.

        //Delay Time.
        double dFwdOnDelayTime     ;
        double dBwdOnDelayTime     ;
        double dFwdTimeOutDelayTime;                                                                  
        double dBwdTimeOutDelayTime;

        int    ThreadApplyTime;
        int    iInv;            //Inverse Output.
        int    LastOutCommand;

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool ApplyTimeout;
        public bool ApplyOutComplete;
        public bool ApplyKillOutput;
        public bool EnableLastOutCmdTOut;

        public bool vfy,dfy, dfx, fx, vfx, odfx, ftoe, Ltftoe;
        public bool vby,dby, dbx, bx, vbx, odbx, btoe, Ltbtoe;

        //Define Manual & No
        public int  m_iManNo;
        public int  m_iErrNo;

        Stopwatch sw;
        int iPreCmd, iSimDir; 


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TActuator()
        {
            Init();
        }
        ~TActuator() { }

        public bool   Input (int n)
        {
            if ((n < 0) || (n >= cDEF.IO._iNumOfX)) return false;
            return cDEF.IO.gX((EN_IN_ID)n);
        }
        //--------------------------------------------------------------------------
        public bool   Output(int n)
        {
            if ((n < 0) || (n >= cDEF.IO._iNumOfY)) return false;
            return cDEF.IO.gY((EN_OUT_ID)n);
        }
        //--------------------------------------------------------------------------
        public void Output(int n, int on)
        {
            if ((n < 0) || (n >= cDEF.IO._iNumOfY)) return;
            cDEF.IO.sY((EN_OUT_ID)n, (on==1) ? true : false);
        }
        //--------------------------------------------------------------------------
        //Init.
        public void Init()
        {
            m_xfwdId = m_xfwdIndex = -1;
            m_xbwdId = m_xbwdIndex = -1;
            m_yfwdId = m_yfwdIndex = -1;
            m_ybwdId = m_ybwdIndex = -1;

            m_iManNo = -1;
            m_iErrNo = -1;
            //options.
            ApplyTimeout = false;
            ApplyOutComplete = false;

            dFwdTimeOutDelayTime = 0.0;
            dFwdTimeOutDelayTime = 0.0;

            Name = "NONE";

            dfy = dfx = fx = vfx = odfx = ftoe = Ltftoe = false;
            dby = dbx = bx = vbx = odbx = btoe = Ltbtoe = false;

            LastOutCommand = -1;

            dFwdOnDelayTime = 0.0;
            dBwdOnDelayTime = 0.0;

            tFwdTimeOutTimer.Clear();
            tBwdTimeOutTimer.Clear();
            tFwdOnDelayTimer.Clear();
            tBwdOnDelayTimer.Clear();

            //
            sw = new Stopwatch();
            sw.Reset();
            iPreCmd = 0;
            iSimDir = 0; 


        }
        //--------------------------------------------------------------------------
        //basic  functions.
        public void Update(bool sim = false)
        {
	        UpdateInput();

            if(sim) RunSimul(iPreCmd);
        }
        //--------------------------------------------------------------------------
        public void UpdateInput()
        {
            //Local Var.
            bool  isXFwdID  ;
            bool  isXBwdID  ;
            bool  isYFwdID  ;
            bool  isYBwdID  ;
            int   iXfwdId   ;
            int   iXbwdId   ;
            int   iYfwdId   ;
            int   iYbwdId   ;
            int   iXfwdIndex;
            int   iXbwdIndex;
            int   iYfwdIndex;
            int   iYbwdIndex;

            //Set I/O ID.
            iXfwdId = (iInv ==1) ? m_xbwdId : m_xfwdId;
            iXbwdId = (iInv ==1) ? m_xfwdId : m_xbwdId;
            iYfwdId = (iInv ==1) ? m_ybwdId : m_yfwdId;
            iYbwdId = (iInv ==1) ? m_yfwdId : m_ybwdId;

            //Set I/O Index.
            iXfwdIndex = (iInv ==1) ? m_xbwdIndex : m_xfwdIndex;
            iXbwdIndex = (iInv ==1) ? m_xfwdIndex : m_xbwdIndex;
            iYfwdIndex = (iInv ==1) ? m_ybwdIndex : m_yfwdIndex;
            iYbwdIndex = (iInv ==1) ? m_yfwdIndex : m_ybwdIndex;

            //if(!cDEF.IO._bInitOk) return;

            //Check SKIP.
            if ((iYfwdId == SKIP4 && iYbwdId == SKIP4) ||
                (iYfwdId == SKIP5 && iYbwdId == SKIP5)   )
            {
                Rst();
                return ;
            }

            //Set exist ID.
            isXFwdID = (iXfwdId != SKIP4 && iXfwdId != SKIP5);
            isXBwdID = (iXbwdId != SKIP4 && iXbwdId != SKIP5);
            isYFwdID = (iYfwdId != SKIP4 && iYfwdId != SKIP5);
            isYBwdID = (iYbwdId != SKIP4 && iYbwdId != SKIP5);

            //I/O.
            if (isXFwdID) dfx =  Input (iXfwdIndex); //Detect X/Y Val.
			else          dfx = !Input (iXfwdIndex);
            if (isXBwdID) dbx =  Input (iXbwdIndex);
			else          dbx = !Input (iXbwdIndex);
            if (isYFwdID) dfy =  Output(iYfwdIndex);
            else          dfy = !Output(iYfwdIndex);
            if (isYBwdID) dby =  Output(iYbwdIndex);
            else          dby = !Output(iYbwdIndex);

            //Virtual Input Sensor
                //FWD.
            if      (isXFwdID) vfx =  dfx;
            else if (isXBwdID) vfx = !dbx;
            else if (isYFwdID) vfx =  dfy;
            else if (isYBwdID) vfx = !dby;
            else               vfx = !dby;
                //BWD.
            if      (isXBwdID) vbx =  dbx;
            else if (isXFwdID) vbx = !dfx;
            else if (isYBwdID) vbx =  dby;
            else if (isYFwdID) vbx = !dfy;
            else               vbx = !dfy;

            //Virtual Output Sensor
            if (isYFwdID) vfy =  dfy;
            else          vfy = !dby;
            if (isYBwdID) vby =  dby;
            else          vby = !dfy;

            //OnDelay Timer.
            odfx = tFwdOnDelayTimer.OnDelay(vfx , dFwdOnDelayTime);
            odbx = tBwdOnDelayTimer.OnDelay(vbx , dBwdOnDelayTime);

            //Check time out by ondelay result.
                //Time out By Input
            bool r = true;
            if ( odfx && !odbx) r = false;
            if (!odfx &&  odbx) r = false;
            if ( odfx &&  odbx) r = true ;
            if (!odfx && !odbx) r = true ;
                //Time out By Output
            if ( dfy  &&  odbx) r = true ;
            if ( dfy  && !odfx) r = true ;
            if ( dby  &&  odfx) r = true ;
            if ( dby  && !odbx) r = true ;

            //Is timeout.
            ftoe = tFwdTimeOutTimer.OnDelay(ApplyTimeout && r , dFwdTimeOutDelayTime);
            btoe = tBwdTimeOutTimer.OnDelay(ApplyTimeout && r , dBwdTimeOutDelayTime);
            if (ftoe) Ltftoe = true;
            if (btoe) Ltbtoe = true;

            //Error Action.
            if (ftoe || btoe) { fx = false; bx = false; }
            else              { fx = odfx ; bx = odbx ; }

            //Apply Out Complete.
            if (ApplyOutComplete) 
            {
                Rst();
                fx = dfy;
                bx = dby;

                if      (isYFwdID) fx =  dfy;
                else if (isYBwdID) fx = !dby;
                else               fx =  !bx;

                if      (isYBwdID) bx =  dby;
                else if (isYFwdID) bx = !dfy;
                else               bx =  !fx;
            }
        }
        //--------------------------------------------------------------------------
        public string GetName()            { return Name; }
        public void   SetName(string data) { Name = data; }
        public string GetComt()            { return Comt; }
        public void   SetComt(string data) { Comt = data; }
        public int    GetManNo()           { return m_iManNo; }
        public void   SetManNo(int data)   { m_iManNo = data; }
        public int    GetErrNo()           { return m_iErrNo; }
        public void   SetErrNo(int data)   { m_iErrNo = data; }
        //--------------------------------------------------------------------------
        //status functions.
        public int GetLastCmd() { return LastOutCommand; }
        //--------------------------------------------------------------------------
        public bool Complete(int Cmd) //1:On,Fwd,Up,Close,Rotate 0:Off,Bwd,Dw, Open.
        {
            if (Cmd==1) return  fx && !bx;
            else        return !fx &&  bx;
        }
        //------------------------------------------------------------------------
        public bool CompleteSim(int Cmd)
        {
            //
            return (iSimDir == Cmd);
        }

        //--------------------------------------------------------------------------
        public bool Complete(int Cmd, int Fwd, int Bwd)
        {
            if (Cmd==1) return  Input(Fwd) && !Input(Bwd);
            else        return !Input(Fwd) &&  Input(Bwd);
        }
        //--------------------------------------------------------------------------
        public bool Trg(int Cmd) //Last Command.
        {
            return (LastOutCommand == Cmd) ? true : false;
        }
        //--------------------------------------------------------------------------
        public int Err()
        {
            if (Ltftoe) return 1;
            if (Ltbtoe) return 2;
            return 0;
        }
        //--------------------------------------------------------------------------
        public void Rst()
        {
            //
            ftoe = false; Ltftoe = false;
            btoe = false; Ltbtoe = false;

            //Timer Reset.
            tFwdTimeOutTimer.Clear();
            tBwdTimeOutTimer.Clear();

            //sw.Reset();
        }
        //--------------------------------------------------------------------------
        public bool GetTimeOut()
        {
            if (IsSkip()) return false;

            //
            if (ftoe  ) return true;
            if (btoe  ) return true;
            if (Ltftoe) return true;
            if (Ltbtoe) return true;
            return false;
        }
        //--------------------------------------------------------------------------
        public bool IsSkip()
        {
            bool r1 = (m_yfwdId == SKIP4 || m_yfwdId == SKIP5);
            bool r2 = (m_ybwdId == SKIP4 || m_ybwdId == SKIP5);

            return (r1 && r2);
        }
        //--------------------------------------------------------------------------
        //run functions.
        public bool Out(int Cmd)  //Force.
        {
            int yf     ;
            int yb     ;
            int yfIndex;
            int ybIndex;

            //Inverse Out.
            if (iInv==1) {
                yf = GetybwdId(); yfIndex = GetybwdIndex();
                yb = GetyfwdId(); ybIndex = GetyfwdIndex();
                }
            else {
                yf = GetyfwdId(); yfIndex = GetyfwdIndex();
                yb = GetybwdId(); ybIndex = GetybwdIndex();
                }

            //Set.
            EnableLastOutCmdTOut = true;
            if (Cmd==1){
                LastOutCommand = 1;
                if (yf != SKIP4 && yf != SKIP5) Output(yfIndex , 1);
                if (yb != SKIP4 && yf != SKIP5) Output(ybIndex , 0);
                return dfy && !dby;
                }
            else {
                LastOutCommand = 0;
                if (yf != SKIP4 && yf != SKIP5) Output(yfIndex , 0);
                if (yb != SKIP4 && yb != SKIP5) Output(ybIndex , 1);
                return !dfy && dby;
                }
        }
        //--------------------------------------------------------------------------
        public bool Run(int Cmd)  //Check Time Out.
        {
            Out(Cmd);
            return (IsSkip() || Complete(Cmd));
        }
        //------------------------------------------------------------------------
        public bool RunSimul(int Cmd)
        {
            //
            if (IsSkip()) { iSimDir = Cmd; return true; }
            if (iSimDir == Cmd) return true; 

            double dOnDelayTime = (Cmd == 1) ? GetFwdOnDelayTime() : GetBwdOnDelayTime();

            iPreCmd = Cmd; 

            if (sw.IsRunning)
            {
                //Timer 중이면
                if (sw.ElapsedMilliseconds >= dOnDelayTime)
                {
                    iSimDir = Cmd; //Direction
                    sw.Stop();
                    return true;
                }
            }
            else sw.Restart();


            return false; 
        }
        //--------------------------------------------------------------------------
        //ID functions.
        //Set
        public void SetThreadApplyTime(int Time) { ThreadApplyTime = Time; }
        public void SetInv(int Inv) { iInv = Inv; }
        public void SetAllId(int xf, int xb, int yf, int yb)
        {
        }
        //--------------------------------------------------------------------------
        public void SetxfwdId(int n) 
        { 
            m_xfwdId  = n;
            for (int i = 0; i < cDEF.IO._iNumOfX; i++) 
            {
                if (n == cDEF.IO.XA[i]) { m_xfwdIndex = i; return; } 
            } 
        }
        //--------------------------------------------------------------------------
        public void SetxbwdId(int n) 
        { 
            m_xbwdId  = n; 
            for ( int i = 0 ; i < cDEF.IO._iNumOfX; i++) { 
                if (n == cDEF.IO.XA[i]) { m_xbwdIndex = i; return; } 
            } 
        }
        //--------------------------------------------------------------------------
        public void SetyfwdId(int n) 
        { 
            m_yfwdId  = n;
            for (int i = 0; i < cDEF.IO._iNumOfY; i++)
            {
                if (n == cDEF.IO.YA[i]) { m_yfwdIndex = i; return; } 
            } 
        }
        //--------------------------------------------------------------------------
        public void SetybwdId(int n) 
        { 
            m_ybwdId  = n;
            for (int i = 0; i < cDEF.IO._iNumOfY; i++)
            {
                if (n == cDEF.IO.YA[i]) { m_ybwdIndex = i; return; } 
            } 
        }
        //--------------------------------------------------------------------------
        //Get
        public int GetThreadApplyTime() { return ThreadApplyTime; }
        public int GetxfwdId()          { return m_xfwdId; }
        public int GetxbwdId()          { return m_xbwdId; }
        public int GetyfwdId()          { return m_yfwdId; }
        public int GetybwdId()          { return m_ybwdId; }
        public int GetxfwdIndex()       { return m_xfwdIndex; }
        public int GetxbwdIndex()       { return m_xbwdIndex; }
        public int GetyfwdIndex()       { return m_yfwdIndex; }
        public int GetybwdIndex()       { return m_ybwdIndex; }
        public int GetInv()             { return iInv; }
        //--------------------------------------------------------------------------
        //Set functions.
            //Set.
        public void   SetFwdOnDelayTime        (double data) { dFwdOnDelayTime = data; }
        public void   SetBwdOnDelayTime        (double data) { dBwdOnDelayTime = data; }
        public void   SetFwdTimeOutDelayTime   (double data) { dFwdTimeOutDelayTime = data; }
        public void   SetBwdTimeOutDelayTime   (double data) { dBwdTimeOutDelayTime = data; }
        public void   SetApplyTimeout          (bool data)   { ApplyTimeout = data; }
        public void   SetApplyOutComplete      (bool data)   { ApplyOutComplete = data; }
        //--------------------------------------------------------------------------
            //Get.
        public double GetFwdOnDelayTime()      { return dFwdOnDelayTime; }
        public double GetBwdOnDelayTime()      { return dBwdOnDelayTime; }
        public double GetFwdTimeOutDelayTime() { return dFwdTimeOutDelayTime; }
        public double GetBwdTimeOutDelayTime() { return dBwdTimeOutDelayTime; }
        public bool   GetApplyTimeout()        { return ApplyTimeout; }
        public bool   GetApplyOutComplete()    { return ApplyOutComplete; }
        
        //--------------------------------------------------------------------------
        //Read/Write Para.
        public void  Load(bool IsLoad , int Act , string Path)
        {
            //Local Var.
            String sName;
            String Temp ;
            TIniUnit ini = new TIniUnit();

            //Load.                         
            sName = string.Format("ACTUATOR({0,3:000})", Act + 1);
            if (IsLoad)
            {
                m_xfwdId = 0xFFFFF;
                m_xbwdId = 0xFFFFF;
                m_yfwdId = 0xFFFFF;
                m_ybwdId = 0xFFFFF;

                ini.Load(Path, "Name"   , sName, out Name);
                ini.Load(Path, "Comment", sName, out Comt);
                ini.Load(Path, "xFwdID" , sName, out Temp); if (Temp != "") m_xfwdId = int.Parse(Temp.Substring(1), System.Globalization.NumberStyles.HexNumber);
                ini.Load(Path, "xBwdID" , sName, out Temp); if (Temp != "") m_xbwdId = int.Parse(Temp.Substring(1), System.Globalization.NumberStyles.HexNumber);
                ini.Load(Path, "yFwdID" , sName, out Temp); if (Temp != "") m_yfwdId = int.Parse(Temp.Substring(1), System.Globalization.NumberStyles.HexNumber);
                ini.Load(Path, "yBwdID" , sName, out Temp); if (Temp != "") m_ybwdId = int.Parse(Temp.Substring(1), System.Globalization.NumberStyles.HexNumber);

                ini.Load(Path, "ApplyTimeout       " , sName, out ApplyTimeout        );  
                ini.Load(Path, "ThreadApplyTime    " , sName, out ThreadApplyTime     );  
                ini.Load(Path, "FwdTimeOutDelayTime" , sName, out dFwdTimeOutDelayTime); 
                ini.Load(Path, "BwdTimeOutDelayTime" , sName, out dBwdTimeOutDelayTime);  
                ini.Load(Path, "FwdOnDelayTime     " , sName, out dFwdOnDelayTime     );  
                ini.Load(Path, "BwdOnDelayTime     " , sName, out dBwdOnDelayTime     ); 
                ini.Load(Path, "ApplyOutComplete   " , sName, out ApplyOutComplete    );  
                ini.Load(Path, "Inv                " , sName, out iInv                );

                SetxfwdId(m_xfwdId);
                SetxbwdId(m_xbwdId);
                SetyfwdId(m_yfwdId);
                SetybwdId(m_ybwdId);
            }
            else
            {
                ini.Save(Path, "Name"   , sName, Name);
                ini.Save(Path, "Comment", sName, Comt);

                Temp = string.Format("X{0:X4}", m_xfwdId); ini.Save(Path, "xFwdID", sName, Temp);
                Temp = string.Format("X{0:X4}", m_xbwdId); ini.Save(Path, "xBwdID", sName, Temp);
                Temp = string.Format("Y{0:X4}", m_yfwdId); ini.Save(Path, "yFwdID", sName, Temp);
                Temp = string.Format("Y{0:X4}", m_ybwdId); ini.Save(Path, "yBwdID", sName, Temp); 
                    
                ini.Save(Path, "ApplyTimeout       ", sName, ApplyTimeout);
                ini.Save(Path, "ThreadApplyTime    ", sName, ThreadApplyTime);
                ini.Save(Path, "FwdTimeOutDelayTime", sName, dFwdTimeOutDelayTime);
                ini.Save(Path, "BwdTimeOutDelayTime", sName, dBwdTimeOutDelayTime);
                ini.Save(Path, "FwdOnDelayTime     ", sName, dFwdOnDelayTime);
                ini.Save(Path, "BwdOnDelayTime     ", sName, dBwdOnDelayTime);
                ini.Save(Path, "ApplyOutComplete   ", sName, ApplyOutComplete);
                ini.Save(Path, "Inv                ", sName, iInv);

                SetxfwdId(m_xfwdId);
                SetxbwdId(m_xbwdId);
                SetyfwdId(m_yfwdId);
                SetybwdId(m_ybwdId);
            }
        }

    }


    /***************************************************************************/
    /* Class: TSysActuator                                                     */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TSysActuator
    {
        //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer m_ChngActrOnTimer = new TOnDelayTimer();
        int           m_iNumOfACT;

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        TActuator[]  Actuator;
        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool[] m_bRptActr;
        public string m_sFNameACT    ;
        public int    m_iChngActrDlay;
		public int    m_iErrFNo      ;
        public int    m_iManFNo      ;
        public bool   m_bRptActrIng  ;
        public int    m_iRptCmd      ;
        
        //
         private bool m_bSim         ; //Simulation Mode

        //Object.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public TActuator this[int iActNo]
        {
            get
            {
                if (iActNo < 0 | iActNo >= m_iNumOfACT) return null;
                return Actuator[iActNo];
            }
            set
            {
                if (iActNo < 0 | iActNo >= m_iNumOfACT) return;
                Actuator[iActNo] = value;
            }
        }
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public string _sFNameACT
        {
            get { return m_sFNameACT; }
            set { m_sFNameACT = value; }
        }
        public int _iNumOfACT
        {
            get { return m_iNumOfACT; }
        }
        public int _iErrFNo
        {
            get { return m_iErrFNo; }
            set { m_iErrFNo = value; }
        }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSysActuator()
        {
            m_bSim = false; 
        }
        ~TSysActuator() { }
        //--------------------------------------------------------------------------
        //Init.
        public void  Init (int iErrNo, int iManNo, string sFNameACT, bool Sim = false)
        {
            String Name , Comt;
            m_iNumOfACT = (int)EN_ACTR_ID.EndOfId;
            Actuator    = new TActuator[m_iNumOfACT];
            m_bRptActr  = new bool     [m_iNumOfACT];

	        for (int i = 0 ; i < m_iNumOfACT ; i++) 
            {
                Actuator[i] = new TActuator();
                Name = string.Format("ACTUATOR{0,3:000}",i);   
                Comt = string.Format("COMMENT {0,3:000}",i);   
                Actuator[i].Init();
                Actuator[i].SetName  (Name   );
                Actuator[i].SetComt  (Comt   );
                Actuator[i].SetxfwdId(0xFFFFF);
                Actuator[i].SetxbwdId(0xFFFFF);
                Actuator[i].SetyfwdId(0xFFFFF);
                Actuator[i].SetybwdId(0xFFFFF);
                Actuator[i].SetManNo (iManNo + i);
                Actuator[i].SetErrNo (iErrNo + i);
                m_bRptActr [i] = false;
            }

            m_sFNameACT     = sFNameACT;
            m_iErrFNo       = iErrNo   ;
            m_bRptActrIng   = false;
            m_iChngActrDlay = 0;

            m_bSim          = Sim  ; 

            Load(true);
        }
        //--------------------------------------------------------------------------
        public void  Reset() 
        { 
            for (int i = 0 ; i < m_iNumOfACT ; i++) Rst(i); 
        }
        //--------------------------------------------------------------------------
        //Normal Control.
        public bool  Out        (int aNum , int Act                                      ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return false; 
            return Actuator[aNum].Out     (Act              ); 
        }
        //--------------------------------------------------------------------------
        public bool  Trg        (int aNum , int Act                                      ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return false; 
            return Actuator[aNum].Trg     (Act              ); 
        }
        //--------------------------------------------------------------------------
        public void  Rst        (int aNum                                                ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return  ;        
            Actuator[aNum].Rst     (                 ); 
        }
        //--------------------------------------------------------------------------
        public int   Err        (int aNum                                                ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return 0; 
            return Actuator[aNum].Err     (                 ); 
        }
        //--------------------------------------------------------------------------
        public bool   Complete   (int aNum , int Act                                      ) 
        {
            //
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return false; 
            return m_bSim? Actuator[aNum].CompleteSim(Act) : Actuator[aNum].Complete(Act); 
        }
        //--------------------------------------------------------------------------
        public bool   Complete   (int aNum , int Act , int xFwd , int xBwd                ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return false; 
            return Actuator[aNum].Complete(Act , xFwd , xBwd); 
        }
        //------------------------------------------------------------------------
        public bool GetCompleteFwd(EN_ACTR_ID aNum)
        {
            return Complete((int)aNum, vDEF.FWD);
        }
        //------------------------------------------------------------------------
        public bool GetCompleteBwd(EN_ACTR_ID aNum)
        {
            return Complete((int)aNum, vDEF.BWD);
        }
        //--------------------------------------------------------------------------
        //Cylinder Control.
        public int    GetLastCmd(int aNum          ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return 0; 
            return Actuator[aNum].GetLastCmd(   ); 
        }
        //--------------------------------------------------------------------------
        public bool   Getdfx    (int aNum          ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return false; 
            return Actuator[aNum].dfx            ; 
        }
        //--------------------------------------------------------------------------
        public bool   Getdbx    (int aNum          ) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return false; 
            return Actuator[aNum].dbx            ; 
        }
        //--------------------------------------------------------------------------
        public bool    GetCylStat(int aNum , int Act) 
        { 
            if ((aNum < 0) || (aNum >= m_iNumOfACT)) return false; 
            return Actuator[aNum].Complete  (Act); 
        }
        //--------------------------------------------------------------------------
        public bool MoveCyl(int aNum, EN_ACTR_CMD Act)
        {
            return MoveCyl(aNum, (int)Act);
        }
        //------------------------------------------------------------------------
        public bool   MoveCyl   (int aNum , int Act)
        {
            //Run Actuator.
	        if( aNum<0 || aNum>=m_iNumOfACT      ) return false;
            if(!cDEF.SEQ.CheckDstbActr(aNum, Act)) return false;

            return m_bSim ? Actuator[aNum].RunSimul(Act) : Actuator[aNum].Run(Act);
        }
        //--------------------------------------------------------------------------
        public void   SetRpt    (int aNum , bool Flag, int iDelay = 0)
        {
	        m_iChngActrDlay = iDelay;
            if (!Flag                       )  m_bRptActrIng = false;
	        if (aNum == -1) {
		        for (int n = 0 ; n < m_iNumOfACT; n++) m_bRptActr[n] = Flag;
		        }
	        else {
		        if(aNum<0 || aNum>=m_iNumOfACT) return;
		        m_bRptActr[aNum] = Flag;
		        m_iRptCmd = 1;
		        }
        }
        //--------------------------------------------------------------------------
        //Update.
        public void  Update()
        {
	        bool isOk = true;
	        for (int i = 0 ; i < m_iNumOfACT ; i++) {
		        Actuator[i].Update(m_bSim);

		        //Check Actuator Status.
		        if (Err(i)==1) m_bRptActr[i] = false;

		        //Changing Timer.
		        if (!m_bRptActrIng) continue;
		        if (m_bRptActr[i]) {
                    if(!cDEF.SEQ.CheckDstbActr(i, m_iRptCmd)) { m_bRptActrIng = false; continue; }
			        MoveCyl(i , m_iRptCmd);
			        if(!Complete(i , m_iRptCmd)) isOk = false;
		        }
	        }
	        m_ChngActrOnTimer.OnDelay(isOk , m_iChngActrDlay);
	        if(m_ChngActrOnTimer.Out)
	        {
		        m_ChngActrOnTimer.Clear();
                if (m_iRptCmd == (int)EN_ACTR_CMD.Fwd) m_iRptCmd = (int)EN_ACTR_CMD.Bwd;
		        else                                   m_iRptCmd = (int)EN_ACTR_CMD.Fwd;
	        }
        }
        //--------------------------------------------------------------------------
        //Read/Write Para.
        public void  Load(bool IsLoad)
        {
            //Local Var.
            string Path;

            //Input.
            Path = _sFNameACT;
            if (IsLoad) {
                if (!FNC.FileExists(Path)) return;
                }
            for (int i = 0 ; i < m_iNumOfACT  ; i++) {
                Actuator[i].Load(IsLoad , i , Path);
                }
        }
        //--------------------------------------------------------------------------
        public int  ManNoActr        (int Actr) 
        {
            if(Actr < 0           ) return -1; 
            if(Actr >= m_iNumOfACT) return -1; 
            return Actuator[Actr].m_iManNo   ;
        }
        //--------------------------------------------------------------------------
        public int ErrNoActr(int Actr) 
        { 
            if(Actr < 0           ) return -1; 
            if(Actr >= m_iNumOfACT) return -1; 
            return Actuator[Actr].m_iErrNo; 
        }
        //--------------------------------------------------------------------------
        //Display.
        public void UpdateByGrid(ref System.Windows.Forms.DataGridView Grid)
        {
	        int        rCount     = m_iNumOfACT;
	        int[]      iWidth     = {30, 0, 60, 60, 60, 60, 60, 60, 60, 60, 40, 40, 40, 40};
	        String[]   sItem      = {"No"       , "Comment"     , "xFwdID"      , "xBwdID"      , "yFwdID" ,
							         "yBwdID"   , "Fwd OnDelay", "Bwd OnDelay"  , "Fwd TimeOut" , "Bwd TimeOut",
							         "Apply TO" , "Apply OC"   , "Inv"          , "Rpt"};
            int        iTotWidth  = 0;
			DataTable  dt = new DataTable();
            
            Grid.Dock = System.Windows.Forms.DockStyle.Fill;			
            FNC.SetGridStyle(ref Grid);
            Grid.BackgroundColor = FRM.GetGridBackColor(); //Color.FromArgb(66, 72, 88);
            Grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Century Gothic", 9);
            Grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //
            for(int i=0;i<14;i++) 
            {
                     if(i==10) dt.Columns.Add(sItem[i], typeof(bool)); //CheckBox.
                else if(i==11) dt.Columns.Add(sItem[i], typeof(bool)); 
                else if(i==12) dt.Columns.Add(sItem[i], typeof(bool)); 
                else if(i==13) dt.Columns.Add(sItem[i], typeof(bool)); 
                else 
                {
                    dt.Columns.Add(sItem[i], typeof(string));    
                }
            }

            for(int i=0; i<rCount;i++) {
		         sItem[0 ] = Convert.ToString(i);
		         sItem[1 ] = Actuator[i].GetComt();
                 sItem[2 ] = string.Format("X{0:X4}", Actuator[i].GetxfwdId());
                 sItem[3 ] = string.Format("X{0:X4}", Actuator[i].GetxbwdId());
                 sItem[4 ] = string.Format("Y{0:X4}", Actuator[i].GetyfwdId());
                 sItem[5 ] = string.Format("Y{0:X4}", Actuator[i].GetybwdId());
		         sItem[6 ] = Convert.ToString(Actuator[i].GetFwdOnDelayTime     ());
		         sItem[7 ] = Convert.ToString(Actuator[i].GetBwdOnDelayTime     ());
		         sItem[8 ] = Convert.ToString(Actuator[i].GetFwdTimeOutDelayTime());
		         sItem[9 ] = Convert.ToString(Actuator[i].GetBwdTimeOutDelayTime());
		         sItem[10] = (Actuator[i].GetApplyTimeout       ()) ? "True" : "False"; 
		         sItem[11] = (Actuator[i].GetApplyOutComplete   ()) ? "True" : "False";
		         sItem[12] = (Actuator[i].GetInv()  == 1          ) ? "True" : "False";
		         sItem[13] = (m_bRptActr[i]) ? "True" : "False";
                 dt.Rows.Add(sItem);
                }
			Grid.DataSource = dt;
			//
            for(int i=0;i<14;i++) 
            {
                Grid.Columns[i].Width = iWidth[i];
                iTotWidth += iWidth[i];
			}
            Grid.Columns[1].Width = Grid.Width - iTotWidth-20;
            Grid.Columns[0].ReadOnly = true;
            Grid.Columns[1].ReadOnly = true;

            for (int n = 0; n < Grid.ColumnCount; n++)
            {
                if (n != 1)
                    Grid.Columns[n].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            Grid.Visible  = true;
        }
        //--------------------------------------------------------------------------
        public void SaveFrGrid(ref System.Windows.Forms.DataGridView Grid)
        {
            int xfwdId = 0xFFFFF;
            int xbwdId = 0xFFFFF;
            int yfwdId = 0xFFFFF;
            int ybwdId = 0xFFFFF;

            for(int i=0; i<Grid.RowCount ;i++) 
            {
                if(Actuator[i].GetComt() == "") continue;

                if (Grid[2,i].Value.ToString() != "") xfwdId = int.Parse(Grid[2,i].Value.ToString().Substring(1), System.Globalization.NumberStyles.HexNumber);
                if (Grid[3,i].Value.ToString() != "") xbwdId = int.Parse(Grid[3,i].Value.ToString().Substring(1), System.Globalization.NumberStyles.HexNumber);
                if (Grid[4,i].Value.ToString() != "") yfwdId = int.Parse(Grid[4,i].Value.ToString().Substring(1), System.Globalization.NumberStyles.HexNumber);
                if (Grid[5,i].Value.ToString() != "") ybwdId = int.Parse(Grid[5,i].Value.ToString().Substring(1), System.Globalization.NumberStyles.HexNumber);

                Actuator[i].SetxfwdId             (xfwdId);
                Actuator[i].SetxbwdId             (xbwdId);
                Actuator[i].SetyfwdId             (yfwdId);
                Actuator[i].SetybwdId             (ybwdId);

                Actuator[i].SetFwdOnDelayTime     (Convert.ToInt32(Grid[6,i].Value)                                );
                Actuator[i].SetBwdOnDelayTime     (Convert.ToInt32(Grid[7,i].Value)                                );
                Actuator[i].SetFwdTimeOutDelayTime(Convert.ToInt32(Grid[8,i].Value)                                );
                Actuator[i].SetBwdTimeOutDelayTime(Convert.ToInt32(Grid[9,i].Value)                                );
                Actuator[i].SetApplyTimeout       ((Grid[10,i].Value.ToString().ToUpper() == "TRUE") ? true : false);
                Actuator[i].SetApplyOutComplete   ((Grid[11,i].Value.ToString().ToUpper() == "TRUE") ? true : false);
                Actuator[i].SetInv                ((Grid[12,i].Value.ToString().ToUpper() == "TRUE") ? 1 : 0       );            
            }
            //
            Load(false);
        }
        //--------------------------------------------------------------------------
        public void DisplayStat(ref System.Windows.Forms.DataGridView Grid)
        {
            bool isActrErr = false;
            for(int i=0; i<Grid.RowCount ;i++) 
            {
                isActrErr = Actuator[i].Err() == 1;
                Grid.Rows[i].DefaultCellStyle.BackColor = isActrErr ? Color.FromArgb(255, 192, 192): Grid.DefaultCellStyle.BackColor; 
            }
        }
    }
}
