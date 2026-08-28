using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace eMachine
{
    //enum
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public enum enIO_MAKER  : int
    {
        CNET    = 0,
        DEVN    = 1,
        CEIP    = 2,
        AJIN    = 3,
        WMX3    = 4,
        SIMUL   = 5,
        FASTECH = 6,

        EndofId
    };

    public enum enST_IO  : int  
    { 
        Off  = 0  , 
        On   = 1                          
    };

    public enum EN_ACTR_CMD  : int
    { 
        Bwd    = 0  , 
        Fwd    = 1  ,

        Up     = 0  ,
        Dn     = 1  ,

        Open   = 1 , 
        Close  = 0 , 

        Unlock = 1 ,
        Lock   = 0 ,
    };

    public enum EN_ALG_KIND  : int 
    { 
        CNET      ,
        DEVN      ,
        CEIP      ,
        SD201     , //Comi
        SD103     , //Comi
        AXA       , //AJIN
        EtherCAT  , //EtherCAT
        FASTECH   ,
        EndOfId
    };


    //delegate
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    delegate int  dgfIN          (int iAddr                                               );
    delegate int  dgfOUT         (int iAddr, int bOn                                      );
    delegate int  dgfReadOut     (int iAddr                                               );
    delegate void dgfUpdate      (                                                        );
    delegate void dfgReload      (                                                        );
    delegate void dgfUpdateByGrid(bool ToTable, ref System.Windows.Forms.DataGridView grid);
    delegate void dgfSaveFrGrid  (bool ToTable, ref System.Windows.Forms.DataGridView grid);
    delegate int  dgfGetMemoryX  (int iAddr                                               );
    delegate int  dgfGetMemoryY  (int iAddr                                               );
    delegate void dgfLoadAddress (bool IsLoad                                             );

    /***************************************************************************/
    /* Class: TSysIO                                                           */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TSysIO
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */

        string      m_sFNameX ;
        string      m_sFNameY ;
        int         m_iNumOfX ;
        int         m_iNumOfY ;
        enIO_MAKER  m_iModel  ;
        EN_ALG_KIND m_iIOKind ; 
        bool        m_bInitOk ;
        bool        m_bSim    ; //Simulation Mode

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public int   [] XA          ;   //X Address.
        public int   [] YA          ;   //Y Address.
        public bool  [] XV          ;   //X Val.
        public bool  [] YV          ;   //Y Val.
        public bool  [] YR          ;   //Y Return.
        public bool  [] YRtn        ;   //Y Return Read Value.
        public int   [] XInv        ;   //
        public int   [] YInv        ;   //
        public int   [] XPart       ;   //
        public int   [] YPart       ;   //
        public bool  [] YForce      ;   //Y Return.
        public string[] XComt       ;
        public string[] YComt       ;
        public bool  [] YV_Default  ;   //Y Val.
        public string[] sXA         ;   //X Address.
        public string[] sYA         ;   //Y Address.




        //
        //public SwitchIO swIonWAT  ;//= new SwitchIO(EN_OUT_ID.yWAT_ION_RUN      , EN_OUT_ID.yWAT_ION_STOP      );
        //public SwitchIO swIonMC1  ;//= new SwitchIO(EN_OUT_ID.yMMC_MC1_ION_RUN  , EN_OUT_ID.yMMC_MC1_ION_STOP  );
        //public SwitchIO swIonMC2  ;//= new SwitchIO(EN_OUT_ID.yMMC_MC2_ION_RUN  , EN_OUT_ID.yMMC_MC2_ION_STOP  );
        //public SwitchIO swIonMGZ1 ;//= new SwitchIO(EN_OUT_ID.yLPM_PORT1_ION_RUN, EN_OUT_ID.yLPM_PORT1_ION_STOP);
        //public SwitchIO swIonMGZ2 ;//= new SwitchIO(EN_OUT_ID.yLPM_PORT2_ION_RUN, EN_OUT_ID.yLPM_PORT2_ION_STOP);


        //Indexer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int _iNumOfX
        {
            get { return m_iNumOfX; }
            set { m_iNumOfX = value; }
        }
        public int _iNumOfY
        {
            get { return m_iNumOfY; }
            set { m_iNumOfY = value; }
        }
        public string _sFNameX
        {
            get { return m_sFNameX; }
            set { m_sFNameX = value; }
        }
        public string _sFNameY
        {
            get { return m_sFNameY; }
            set { m_sFNameY = value; }
        }
        public bool _bInitOk
        {
            get { return m_bInitOk; }
            set { m_bInitOk = value; }
        }

        public EN_ALG_KIND GetIOType() => m_iIOKind;
        public bool IsIOEtherCatType() => m_iIOKind == EN_ALG_KIND.EtherCAT;

        dgfIN           _fIn          ;
        dgfOUT          _fOut         ;
        dgfUpdate       _fUpdate      ;
        dfgReload       _fReload      ;
        dgfUpdateByGrid _fUpdateByGrid;
        dgfSaveFrGrid   _fSaveFrGrid  ;
        dgfGetMemoryX   _fGetMemoryX  ;
        dgfGetMemoryY   _fGetMemoryY  ;
        dgfLoadAddress  _fLoadAddress ;
        dgfReadOut      _fReadYData   ;
        dgfClose        _fClose       ;


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //TIoCEIP CEIP;
        //TIoCNET CNET;
        //TIoWmx3 Wmx3;
        //TIoAJIN AJIN;

        TIoFastech FASTECH;

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSysIO()
        {
            int iNumOfX = (int)EN_IN_ID .EndOfId;
            int iNumOfY = (int)EN_OUT_ID.EndOfId;

            XA          = new int   [iNumOfX+16];   //X Address.
            YA          = new int   [iNumOfY+16];   //Y Address.
            XV          = new bool  [iNumOfX+16];   //X Val.
            YV          = new bool  [iNumOfY+16];   //Y Val.
            YR          = new bool  [iNumOfY+16];   //Y Return.
            XInv        = new int   [iNumOfX+16];   //
            YInv        = new int   [iNumOfY+16];   //
            XPart       = new int   [iNumOfX+16];   //
            YPart       = new int   [iNumOfY+16];   //
            YForce      = new bool  [iNumOfY+16];   //Y Return.
            XComt       = new string[iNumOfX+16];
            YComt       = new string[iNumOfY+16];
            YV_Default  = new bool  [iNumOfY+16];   //Y Val.
            YRtn        = new bool  [iNumOfY+16];   //Y Read Return Value
            sXA         = new string[iNumOfX+16];
            sYA         = new string[iNumOfY+16];

        }
        ~TSysIO() { }
        //--------------------------------------------------------------------------
        public bool gX (EN_IN_ID Id, bool rtn = false) 
        { 
            int iNo = (int)Id;
            
            if ( iNo < 0 || iNo >= m_iNumOfX) return false;
            if ( m_bSim                     ) return rtn; 
            if (!m_bInitOk                  ) return false; 
            return XV[iNo];       
        }
        //------------------------------------------------------------------------
        public bool gX (int Id, bool rtn = false) 
        { 
            int iNo = Id;
            
            if ( iNo < 0 || iNo >= m_iNumOfX) return false;
            if ( m_bSim                     ) return rtn; 
            if (!m_bInitOk                  ) return false; 
            return XV[iNo];       
        }

        //--------------------------------------------------------------------------
        public bool gY (EN_OUT_ID Id, bool rtn = false) 
        { 
            int iNo = (int)Id;
            
            if (iNo < 0 || iNo >= m_iNumOfY ) return false;
            //if (m_bSim                      ) return rtn;
            if (!m_bInitOk                  ) return false; 
            return YR[iNo];       
        }
        //--------------------------------------------------------------------------
        public bool sY (EN_OUT_ID Id , bool Val)
        {
            int iNo = (int)Id;

            if(iNo<0 || iNo>=m_iNumOfY) return false;

            //if (m_iModel != enIO_MAKER.SIMUL)
            //{
                if(!m_bInitOk) return false;
            //}
            if(YForce[iNo]) return false;

            YV[iNo] = Val;

            return YV[iNo] == Val;
        }
        //--------------------------------------------------------------------------
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-------        
        public void Init      (enIO_MAKER iModel, String sFNAddress, EN_ALG_KIND iKind = EN_ALG_KIND.AXA)
        {
            int iNumOfX = (int)EN_IN_ID .EndOfId;
            int iNumOfY = (int)EN_OUT_ID.EndOfId;

            //XA          = new int   [iNumOfX+16];   //X Address.
            //YA          = new int   [iNumOfY+16];   //Y Address.
            //XV          = new bool  [iNumOfX+16];   //X Val.
            //YV          = new bool  [iNumOfY+16];   //Y Val.
            //YR          = new bool  [iNumOfY+16];   //Y Return.
            //XInv        = new int   [iNumOfX+16];   //
            //YInv        = new int   [iNumOfY+16];   //
            //XPart       = new int   [iNumOfX+16];   //
            //YPart       = new int   [iNumOfY+16];   //
            //YForce      = new bool  [iNumOfY+16];   //Y Return.
            //XComt       = new string[iNumOfX+16];
            //YComt       = new string[iNumOfY+16];
            //YV_Default  = new bool  [iNumOfY+16];   //Y Val.

            for (int i = 0; i < iNumOfX; i++)
            {
                XA   [i] = i;
                XInv [i] = 0;
                XPart[i] = 0;
                XV   [i] = false;
            }
            for (int i = 0; i < iNumOfY; i++)
            {
                YA    [i]    = i;
                YInv  [i]    = 0;
                YPart [i]    = 0;
                YV    [i]    = false;  // 초기화
              //YV    [i]    = YV_Default[i];
                YR    [i]    = false;
                YForce[i]    = false;
                YRtn  [i]    = false;
            }

            //Name, Address...
            Load(true);

            m_iModel  = iModel ;
            m_iNumOfX = iNumOfX;
            m_iNumOfY = iNumOfY;
            m_iIOKind = iKind  ;

            m_bSim = iModel == enIO_MAKER.SIMUL ? true: false ;

            //if(m_iModel == enIO_MAKER.CNET) {
            //    CNET = new TIoCNET();
            //    CNET.m_sFNAddress  = sFNAddress;

            //    _fIn           = CNET.Input ;
            //    _fOut          = CNET.Output;
            //    _fUpdate       = CNET.Update;
            //    _fReload       = CNET.Reload;
            //    _fGetMemoryX   = CNET.GetMemoryX ;
            //    _fGetMemoryY   = CNET.GetMemoryY ;
            //    _fLoadAddress  = CNET.Load       ;
            //    _fUpdateByGrid = CNET.UpdateByGrid;
            //    _fSaveFrGrid   = CNET.SaveFrGrid  ;

            //    if(CNET.Init()) _bInitOk = true;
            //}
            //if(m_iModel == enIO_MAKER.CEIP) {
            //    CEIP = new TIoCEIP();
            //    CEIP.m_sFNAddress = sFNAddress;

            //    _fIn           = CEIP.Input;
            //    _fOut          = CEIP.Output;
            //    _fUpdate       = CEIP.Update;
            //    _fReload       = CEIP.Reload;
            //    _fGetMemoryX   = CEIP.GetMemoryX ;
            //    _fGetMemoryY   = CEIP.GetMemoryY ;
            //    _fLoadAddress  = CEIP.Load       ;
            //    _fUpdateByGrid = CEIP.UpdateByGrid;
            //    _fSaveFrGrid   = CEIP.SaveFrGrid  ;

            //    if (CEIP.Init()) _bInitOk = true;
            //}
            try
            {
                if (m_iModel == enIO_MAKER.AJIN)
                {
                    //AJIN = new TIoAJIN();
                    //AJIN.m_sFNAddress = sFNAddress;
                    //
                    //if (m_iIOKind == EN_ALG_KIND.EtherCAT)
                    //{
                    //    _fIn        = AJIN.Input_ECAT     ;
                    //    _fOut       = AJIN.Output_ECAT    ;
                    //    _fUpdate    = AJIN.Update_ECAT    ;
                    //    _fReadYData = AJIN.OutputRead_ECAT;
                    //}                                 
                    //else                              
                    //{                                 
                    //    _fIn     = AJIN.Input         ;
                    //    _fOut    = AJIN.Output        ;
                    //    _fUpdate = AJIN.Update        ;
                    //}
                    //
                    //_fReload       = AJIN.Reload      ;
                    //_fGetMemoryX   = AJIN.GetMemoryX  ;
                    //_fGetMemoryY   = AJIN.GetMemoryY  ;
                    //_fLoadAddress  = AJIN.Load        ;
                    //_fUpdateByGrid = AJIN.UpdateByGrid;
                    //_fSaveFrGrid   = AJIN.SaveFrGrid  ;
                    //
                    //if (AJIN.Init(m_iIOKind)) m_bInitOk = true;

                }
                else if(m_iModel == enIO_MAKER.WMX3) 
                {
                    //Wmx3 = new TIoWmx3();
                    //Wmx3.m_sFNAddress = sFNAddress;
                    //
                    //_fIn           = Wmx3.Input       ;
                    //_fOut          = Wmx3.Output      ;
                    //_fUpdate       = Wmx3.Update      ;
                    //_fReload       = Wmx3.Reload      ;
                    //_fGetMemoryX   = Wmx3.GetMemoryX  ;
                    //_fGetMemoryY   = Wmx3.GetMemoryY  ;
                    //_fLoadAddress  = Wmx3.Load        ;
                    //_fUpdateByGrid = Wmx3.UpdateByGrid;
                    //_fSaveFrGrid   = Wmx3.SaveFrGrid  ;
                    //_fClose        = Wmx3.Close       ;


                    //if (Wmx3.Init()) m_bInitOk = true;
                }
                else if (m_iModel == enIO_MAKER.FASTECH)
                {
                    FASTECH = new TIoFastech();

                    _fIn           = FASTECH.Input       ;
                    _fOut          = FASTECH.Output      ;
                    _fUpdate       = FASTECH.Update      ;
                    _fReload       = FASTECH.Reload      ;
                    _fGetMemoryX   = null;
                    _fGetMemoryY   = null;
                    _fLoadAddress  = null;
                    _fUpdateByGrid = null;
                    _fSaveFrGrid   = null;
                    _fClose        = null;

                    FASTECH.SetIP(cDEF.FM.EngrOptn.sIP1, cDEF.FM.EngrOptn.sIP2, cDEF.FM.EngrOptn.sIP3, cDEF.FM.EngrOptn.sIP4, (int)EN_MOTR_ID.EndOfId);

                    if (FASTECH.Init()) m_bInitOk = true;

                }

                //Simulation Mode
                if (m_iModel == enIO_MAKER.SIMUL)
                {
                    _bInitOk = true;
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION] AJIN INIT - Message : {ex.Message}");
            }


            //Mapping Address
            if (m_iIOKind != EN_ALG_KIND.EtherCAT && m_iModel != enIO_MAKER.SIMUL) LoadAddress(true);

            if(m_iModel == enIO_MAKER.AJIN) {
                //AJIN.GetDoInitData(); //??
                //Default Run IO
            }
        }
        //------------------------------------------------------------------------
        public void Close()
        {
            if (!m_bInitOk) return;
            _fClose?.Invoke();
        }

        //--------------------------------------------------------------------------
        public void Reload    ()
        {
            if(!m_bInitOk) return;
            _fReload?.Invoke();
        }
        //--------------------------------------------------------------------------
        //Update.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~           
        public void Update       ()
        {
            if (!m_bInitOk) return;
            
            UpdateDevice();

            _fUpdate?.Invoke();
        }
        //--------------------------------------------------------------------------
        public void UpdateDevice ()
        {
            if(!_bInitOk) return;
            //
            UpdateInput ();
            UpdateOutput();
        }
        //--------------------------------------------------------------------------
        public void UpdateInput  () 
        {
            if(!_bInitOk) return;

            //
            bool bX = false; 
            for (int i = 0 ; i < m_iNumOfX ; i++) 
            {
                //XV[i] = _fIn?.Invoke(XA[i]) == 1;
                //if (XInv[i]==1) XV[i] = !XV[i];
                bX = _fIn?.Invoke(XA[i]) == 1;
                if (XInv[i]==1) XV[i] = !bX;
                else            XV[i] =  bX;
            }

        }
        //--------------------------------------------------------------------------
        public void UpdateOutput ()
        {
            //
            if(!_bInitOk) return;

            int  iYVal = 0;
            for (int i = 0 ; i < m_iNumOfY ; i++) 
            {
                if(YInv[i]==1) iYVal = (!YV[i]) ? 1:0; 
                else           iYVal = ( YV[i]) ? 1:0;

                if (m_iModel != enIO_MAKER.SIMUL)
                {
                    if (m_iIOKind == EN_ALG_KIND.EtherCAT)
                    {
                        YR  [i] = _fOut      ?.Invoke(i, iYVal) == 1;
                        YRtn[i] = _fReadYData?.Invoke(i       ) == 1; 
                    }
                    //else if (m_iIOKind == EN_ALG_KIND.FASTECH)
                    //{
                    //    YR  [i] = _fOut      ?.Invoke(i, iYVal) == 1;
                    //    YRtn[i] = _fReadYData?.Invoke(i       ) == 1;
                    //}
                    else
                    {
                        YR  [i] = _fOut?.Invoke(i, iYVal) == 1;
                    }
                }
                else
                {
                    YR  [i] = iYVal == 1? true : false;
                    YRtn[i] = iYVal == 1? true : false;
                }
                
                //
                if (YInv[i] == 1) YR[i] = !YR[i];
            }
        }
        //--------------------------------------------------------------------------
        public void UpdateAddress(bool ToTable, ref System.Windows.Forms.DataGridView InputTable, ref System.Windows.Forms.DataGridView OutputTable)
        {
            //if(!_bInitOk) return;
            if(ToTable)
            {
                _fUpdateByGrid?.Invoke(true , ref InputTable );
                _fUpdateByGrid?.Invoke(false, ref OutputTable);
            }
            else
            {
                _fSaveFrGrid?.Invoke(true , ref InputTable );
                _fSaveFrGrid?.Invoke(false, ref OutputTable);
            }             
        }
        //-------------------------------------------------------------------
        //Show Form
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void   ShowFrmAddress ()
        {
            FrmSetInOut frmSetInOut = new FrmSetInOut();
            frmSetInOut.ShowDialog(); 
            frmSetInOut = null;
        }
        //-------------------------------------------------------------------
        //Direct I/O/
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   DirectX        (int Id           )
        {
            if(Id < 0 || Id >= m_iNumOfX) return false;
            if(!_bInitOk                ) return false;
            bool X = _fIn?.Invoke(XA[Id]) == 1;
            return X;
        }
        //-------------------------------------------------------------------
        public bool   DirectY        (int Id , bool Val)
        {
            int iYVal;
            if (Id < 0 || Id >= _iNumOfY) return false;
            if(!_bInitOk                ) return false;

            if (YInv[Id] == 1) iYVal = (!Val) ? 1 : 0;
            else               iYVal = ( Val) ? 1 : 0;

            bool Y = _fOut?.Invoke(YA[Id], iYVal) == 1;
            return (YInv[Id] == 1) ? !Y : Y;
        }
        //-------------------------------------------------------------------
        //Force Control
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void   FrceOutput     (int Index , bool On)
        {
            //Check wrong index.
            if (Index < 0 || Index >= _iNumOfY) return;
            if(!_bInitOk                      ) return;
            //Output.
            YV[Index] = On;
        }
        //-------------------------------------------------------------------
        //Get Memory.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int    GetMemoryX     (int n , out int Ch)
        {
            int iAddress = -1;
            Ch = 0;
            iAddress = (int)_fGetMemoryX?.Invoke(n);
            return iAddress;
        }
        //-------------------------------------------------------------------
        public int    GetMemoryY     (int n , out int Ch)
        {
            int iAddress = -1;
            Ch = 0;
            iAddress = (int)_fGetMemoryY?.Invoke(n);
            return iAddress;
        }
        //-------------------------------------------------------------------
        public bool   GetInputStat   (int Index    )
        {
            //Check wrong index.
            if(Index < 0 || Index >= m_iNumOfX) return false;
            if(!_bInitOk                      ) return false;

            return XV[Index];
        }
        //-------------------------------------------------------------------
        public bool   GetOutputStat  (int Index    )
        {
            //Check wrong index.
            if(Index < 0 || Index >= m_iNumOfY) return false;
            if(!_bInitOk                      ) return false;
            return YR[Index];
        }
        //------------------------------------------------------------------------
        public bool GetOutputRtnStat(int Index)
        {
            //Check wrong index.
            if (Index < 0 || Index >= m_iNumOfY) return false;
            if (!_bInitOk) return false;
            return YRtn[Index];
        }

        //-------------------------------------------------------------------
        public double GetAI          (int  Addr , int Kind )
        {
            double dVal = 0.0; 

            if(!_bInitOk) return dVal;

            //if(Kind == (int)EN_ALG_KIND.CEIP) dVal = CEIP.AnalogInput(Addr);     
 
            return dVal;
        }
        //-------------------------------------------------------------------
        public bool   SetAO          (int  Addr  , int Kind , int  nSetValue)
        {
            bool bRet = false;    
            if(!_bInitOk) return false;

            //if(Kind == (int)EN_ALG_KIND.CEIP) bRet = CEIP.AnalogOutput(Addr, nSetValue);        
            return bRet;
        }
        //--------------------------------------------------------------------------
        public bool   SetRangeAI     (int  Addr  , int Kind, int  nSetValue)
        {
            bool bRet = false;
            if(!_bInitOk) return false;
            //if(Kind == (int)EN_ALG_KIND.CEIP) CEIP.AnalogSetRange(Addr, nSetValue);
            return bRet;
        }
        //--------------------------------------------------------------------------
        public int    GetCounter     (int  Addr                                                 )
        {
            return 0;
        }
        //--------------------------------------------------------------------------
        public void   SetCounter     (int  Addr  , char cSetValue1   , char cSetValue2          )
        {

        }
        //--------------------------------------------------------------------------
        public void   GetTemp        (int  Addr  , out String sReadTemp, int  iReadByte           )
        {
            sReadTemp = "";    
        }   
        //--------------------------------------------------------------------------     
        public void   SetTemp        (int  Addr  , out String sSetTemp , int  iWrtByte            )
        {
            sSetTemp = "";    
        }
        //------------------------------------------------------------------------
        public bool IsDoorOpen()
        {
            bool open = false; 
            //for (EN_IN_ID n = EN_IN_ID.xDR_DoorLock_Left; n <= EN_IN_ID.xDR_DoorLock_Rigt; n++)
            //{
            //    if (gX(n, false)) open = true; 
            //}
            return open; 
        }
        //Read/Write Para.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void   Load           (bool IsLoad)
        {
             //Local Var.
            string Temp1 , Temp2, Temp3;
            int    nTemp1;

            TIniUnit2 iniIN = new TIniUnit2();

            iniIN.Loadini(_sFNameX);

            if (IsLoad)
            { 
                m_iNumOfX = (int)EN_IN_ID .EndOfId;
                m_iNumOfY = (int)EN_OUT_ID.EndOfId;
            }

            //Input.
            if (IsLoad) 
            {
                for (int i = 0 ; i < m_iNumOfX ; i++) 
                {
                    Temp3 = i.ToString("X");
                    Temp1 = string.Format("INPUT({0,4:0000})" , i + 1);
                    iniIN.Load(_sFNameX, "ADDR", Temp1, out Temp2   );
                    iniIN.Load(_sFNameX, "INVR", Temp1, out XInv [i]);
                    iniIN.Load(_sFNameX, "PART", Temp1, out XPart[i]);
                    iniIN.Load(_sFNameX, "NAME", Temp1, out XComt[i]);

                    sXA  [i] = Temp2.Trim() ;
                    XComt[i] = XComt[i].Trim();
                    //if (Temp2 != "") XA[i] = int.Parse(Temp2.Substring(1), System.Globalization.NumberStyles.HexNumber); 
                    XA[i] = i;

                }
            }
            else {
                for (int i = 0 ; i < m_iNumOfX  ; i++) 
                {
                    Temp1 = string.Format("INPUT({0,4:0000})" , i + 1);
                    Temp2 = string.Format("X{0:X4}", XA[i]); 
                    
                    iniIN.Save(_sFNameX, "ADDR", Temp1, Temp2   );
                    iniIN.Save(_sFNameX, "INVR", Temp1, XInv [i]);
                    iniIN.Save(_sFNameX, "PART", Temp1, XPart[i]);
                    iniIN.Save(_sFNameX, "NAME", Temp1, XComt[i]);

                    iniIN.Saveini(_sFNameX);
                }
            }

            iniIN = null;

            TIniUnit2 iniOUT = new TIniUnit2();

            iniOUT.Loadini(_sFNameY);

            //Input.
            if (IsLoad) 
            {
                for (int i = 0 ; i < m_iNumOfY  ; i++)
                {
                    Temp1 = string.Format("OUTPUT({0,4:0000})" , i + 1);
                    iniOUT.Load(_sFNameY, "ADDR"    , Temp1, out Temp2        );
                    iniOUT.Load(_sFNameY, "INVR"    , Temp1, out YInv [i]     );
                    iniOUT.Load(_sFNameY, "PART"    , Temp1, out YPart[i]     );
                    iniOUT.Load(_sFNameY, "NAME"    , Temp1, out YComt[i]     );
                    iniOUT.Load(_sFNameY, "DEFAULT" , Temp1, out nTemp1       );

                    sYA[i] = Temp2;
                    //if (Temp2 != "") YA[i] = int.Parse(Temp2.Substring(1), System.Globalization.NumberStyles.HexNumber);

                    //YA[i] = i + m_iNumOfX;
                    YA[i] = i;
                    YComt[i] = YComt[i].Trim();

                    if (nTemp1 == 1)    YV_Default[i] = true;
                    else                YV_Default[i] = false;
                }
            }
            else 
            {
                for (int i = 0 ; i < m_iNumOfY  ; i++) 
                {

                    Temp1 = string.Format("OUTPUT({0,4:0000})" , i + 1);
                    Temp2 = string.Format("Y{0:X4}", YA[i]); 
                    
                    iniOUT.Save(_sFNameY, "ADDR"    , Temp1, Temp2            );
                    iniOUT.Save(_sFNameY, "INVR"    , Temp1, YInv [i]         );
                    iniOUT.Save(_sFNameY, "PART"    , Temp1, YPart[i]         );
                    iniOUT.Save(_sFNameY, "NAME"    , Temp1, YComt[i]         );
                    iniOUT.Save(_sFNameY, "DEFAULT" , Temp1, YV_Default[i]    );

                    iniOUT.Saveini(_sFNameY);
                }
            }
            
            iniOUT = null;
        }
        //--------------------------------------------------------------------------
        public void   LoadAddress    (bool IsLoad)
        {
            _fLoadAddress?.Invoke(IsLoad);
        }
        //Export List
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void   ExportList     (String sPath)
        {
            //Local Var.
            string sTemp ;
            string sTitle;
            string Data  = "";

            //File Open.
            FileStream fp = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fp, Encoding.Default);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            
            sTitle = "Addr.,Comment,Inv,Part\n";
            Data  += sTitle;

            for (int i = 0 ; i < m_iNumOfX ; i++) {
                    sTemp = string.Format("X{0:X4}", XA[i]); 
                    Data += XComt[i] + ",";
                    Data += XPart[i] + ",";
                    Data += sTemp    + ",";
                    Data += XInv [i] + ",";
                    Data += "\r\n";
            }

            sTitle = "Addr.,Comment,Inv,Part\n";
            Data += sTitle;

             for (int i = 0 ; i < m_iNumOfY ; i++) {
                    sTemp = string.Format("X{0:X4}", YA[i]); 
                    Data += YComt[i] + ",";
                    Data += YPart[i] + ",";
                    Data += sTemp    + ",";
                    Data += YInv [i] + ",";
                    Data += "\r\n";
            }

            sw.Write(Data);
            sw.Flush();
            sw.Close();

        }    

        //Display.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void UpdateByGrid(int Output, int Part, ref System.Windows.Forms.DataGridView Grid, Color backColor)
        {
            //Local Var.
            int    rCount = (Output == 1) ? _iNumOfY : _iNumOfX;
            string sTemp, sTemp1;

            DataGridViewButtonColumn btnColumnOn = new DataGridViewButtonColumn();  //버튼 추가
	        btnColumnOn.HeaderText = "";
	        btnColumnOn.Name       = "bottonOn";  
            btnColumnOn.FlatStyle  =  FlatStyle.Popup;
             
            DataGridViewButtonColumn btnColumnOf = new DataGridViewButtonColumn();  //버튼 추가
	        btnColumnOf.HeaderText = "";
	        btnColumnOf.Name       = "bottonOf";
            btnColumnOf.FlatStyle  =  FlatStyle.Popup;

            //            
            Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            FNC.SetGridStyle(ref Grid, 30, false, true, false, DataGridViewSelectionMode.RowHeaderSelect);
            //
            Grid.Columns.Add("No"     , "No"     );
            Grid.Columns.Add("Addr."  , "Addr."  );
            Grid.Columns.Add("Comment", "Comment");
            Grid.Columns.Add(btnColumnOn );    
            Grid.Columns.Add(btnColumnOf );

            Grid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None; //DataGridViewAutoSizeColumnMode.Fill; //DataGridViewAutoSizeColumnMode.NotSet
            Grid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Grid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Grid.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Grid.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            Grid.Columns[0].Width = 40 ;
            Grid.Columns[1].Width = 60 ;
            Grid.Columns[2].Width = Grid.Width - (40+60+40+40)-20;
            Grid.Columns[3].Width = 40 ;
            Grid.Columns[4].Width = 40 ;

            if (Output == 1) {
                for(int i=0; i<rCount;i++) 
                {
                    //Check View Part.
                    if ((YPart[i] != Part) && (Part != -1)) continue;
                    //Display To List.
                    sTemp  = string.Format($"{sYA[i]}");
                    sTemp1 = string.Format($"{YComt[i]}");
                    Grid.Rows.Add(i+1, sTemp, sTemp1, "ON", "OFF");  
                }
            }
            else 
            {
                for(int i=0; i<rCount;i++) 
                {
                    //Check View Part.
                    if ((XPart[i] != Part) && (Part != -1)) continue;
                    //Display To List.
                    sTemp  = string.Format($"{sXA[i]}");
                    sTemp1 = string.Format($"{XComt[i]}");
                    Grid.Rows.Add(i+1, sTemp, sTemp1, "ON", "OFF");
                }
            }
            Grid.BackgroundColor = backColor; //Color.FromArgb(66, 72, 88);
            Grid.ClearSelection();
            Grid.MultiSelect = false;

            //
            //cDEF.POSN.SetGridFont(ref Grid);

            //
            Grid.Visible = true;

        }
        //--------------------------------------------------------------------------
        public void DisplayStatus(int Output, int Part, ref System.Windows.Forms.DataGridView Grid)
        { 
            //Local Var.
            int    rCount = (Output==1) ? _iNumOfY : _iNumOfX;
            int    iIoNo  = -1;
            bool   bStat, bRtnStat ;
            bool   bMatchY      = false;
            bool   bUseRtnValue = false;
 
            //Check Null.
            if (Grid  == null) return;
            
            //Set StringGrid.
            for(int i=0; i<Grid.RowCount ;i++) 
            {
                iIoNo = Convert.ToInt32(Grid[0,i].Value)-1;
                if(iIoNo<0) continue;
                
                if (Output == 1) { bStat = GetOutputStat(iIoNo); bRtnStat = GetOutputRtnStat(iIoNo); bMatchY = (bStat == bRtnStat); }
                else               bStat = GetInputStat (iIoNo); 
                
                if (Output == 1)
                {
                    if (bUseRtnValue)
                    {
                        if(bStat)
                        {
                            Grid[3, i].Style.BackColor = bMatchY ? Color.Maroon : Color.Yellow;
                            Grid[4, i].Style.BackColor = Color.Gray  ; 
                        }
                        else
                        {
                            Grid[3, i].Style.BackColor = Color.Gray  ;
                            Grid[4, i].Style.BackColor = bMatchY ? Color.Maroon : Color.Yellow;
                        }
                    }
                    else
                    {
                        Grid[3, i].Style.BackColor = ( bStat) ? Color.Maroon : Color.Gray;
                        Grid[4, i].Style.BackColor = (!bStat) ? Color.Maroon : Color.Gray;
                    }
                    Grid[3,i].Style.ForeColor = ( bStat)? Color.White  : Color.Black; 
                    Grid[4,i].Style.ForeColor = (!bStat)? Color.White  : Color.Black; 
                }
                else
                {
                    Grid[3,i].Style.BackColor = ( bStat)? Color.Lime : Color.Gray;
                    Grid[4,i].Style.BackColor = (!bStat)? Color.Lime : Color.Gray;
                }
            }
        }
    }
}
