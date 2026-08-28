using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComiDll;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TAnalogUnit                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TAnalogAjin
    {
        //Analog Resolution.
        //===========================================================================
        //PRESS
        public const double MAX_PRESS          = 23.0 ;//kgf
        public const double ITV_MAX_PRESS      = 0.50 ;//MPa

        //
        const int           AI_RES             = 32768;  //16bit -32768 ~ +32768
        const int           AO_RES             = 32768;

        const int           AI_VOL_RES         = 10;
        const int           AO_VOL_RES         = 10;
        const double        AO_PRESS_RES       = 10;

        const double        PRESS_OFF_VOLT     = 0.0;

        const double        PRESS_CYL_DIA      = 24 ; //mm 2개 사용 (개당 12mm)
        const double        DEFAULT_WEIGHT     = 3.5; //Kg


        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */

        double[] AI           = new double[(int)EN_AI_CH.EndOfAI];   //Analog Input  value.
        double[] AO           = new double[(int)EN_AO_CH.EndOfAO];   //Analog Output value.
                              
        double[] m_dAICoeff   = new double[(int)EN_AI_CH.EndOfAI];
        double[] m_dAOCoeff   = new double[(int)EN_AO_CH.EndOfAO];

        public double[] m_dFeedAI    = new double[(int)EN_AI_CH.EndOfAI];
        public double[] m_dFeedAIAvr = new double[(int)EN_AI_CH.EndOfAI];

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public bool  m_bInit;

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TAnalogAjin()
        {
            m_bInit = false;
            //Init();
        }
        ~TAnalogAjin() 
        { 
            Close();
        }

        /***************************************************************************/
        /* Init.                                                                   */
        /***************************************************************************/
        //---------------------------------------------------------------------------
        public bool Init()
        {
            int  iModuleCnt = 0;
            uint uStatus = 0;
            //
            try
            {
                if (CAXL.AxlIsOpened() == 0)
                {
                    if ((CAXL.AxlOpenNoReset(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS))
                    {
                        MsgBox.Error("[AJIN Analog] AXL Open Fail");
                    }
                }
                //
                CAXA.AxaInfoIsAIOModule   (ref uStatus   );
                CAXA.AxaInfoGetModuleCount(ref iModuleCnt);
                //
                if ((uStatus != (uint)AXT_EXISTENCE.STATUS_EXIST) || (iModuleCnt <= 0 ))                    
                {
                    MsgBox.Error("[AJIN Analog] AIO Module does not exist.");
                }

                //Set Coefficient
                m_dAOCoeff[(int)EN_AO_CH.Press] = (double)(AO_VOL_RES / ITV_MAX_PRESS);

                
                //Set Range AI. //1171 , 1054 (g)
                //CAXA.AxaiSetRange((int)EN_AO_CH.Press, 0, AO_VOL_RES);
                //CAXA.AxaiSetRange((int)EN_AI_CH.VacBtm2, MIN_IN_VOL_FLOW, MAX_IN_VOL_FLOW);
                //CAXA.AxaiSetRange((int)EN_AI_CH.VacTop1, MIN_IN_VOL_FLOW, MAX_IN_VOL_FLOW);
                //CAXA.AxaiSetRange((int)EN_AI_CH.VacTop2, MIN_IN_VOL_FLOW, MAX_IN_VOL_FLOW);
                //CAXA.AxaiSetRange((int)EN_AI_CH.Load   , MIN_IN_VOL_LOAD, MAX_IN_VOL_LOAD);


                //
                m_bInit = true;
            }
            catch (Exception err)
            {
                Debug.WriteLine($"[TAnalogAjin. Open] Exception : {err.Message}");
                //cDEF.LOG.ExceptionTrace("TAnalogAjin. Open " + err.ToString());
            }
            //
            return m_bInit;

        }
        //---------------------------------------------------------------------------
        public void Close()
        {
            if (!m_bInit) return;
            if (CAXL.AxlIsOpened() == 1) CAXL.AxlClose();
        }
        //---------------------------------------------------------------------------
        public void Reset()
        {

        }
        //---------------------------------------------------------------------------
        public void SetRangeAI()
        {//

        }

        /***************************************************************************/
        /* Member Functions.                                                       */
        /***************************************************************************/
        //---------------------------------------------------------------------------
        public void   SetAO(EN_AO_CH AO_Ch , double Volt)
        {
            //Local Var.
            int nHexaValue;
            int iCh = (int)AO_Ch;

            if (Volt > AO_VOL_RES) Volt = AO_VOL_RES;

            if (!m_bInit || (iCh < 0 || iCh >= (int)EN_AO_CH.EndOfAO))
            {
                return;
            }
            //
            CAXA.AxaoWriteVoltage(iCh, Volt);

            //Check Error.
            if (Volt < 0) Volt = 0;

            //Convert value
            nHexaValue = (int)Volt;
            //Set buffer.
            AO[iCh]     = nHexaValue;
        }
        //---------------------------------------------------------------------------
        public double GetAI (EN_AI_CH AI_Ch)
        {
            double  dReadVolt = 0.0;
            int iCh = (int)AI_Ch;

            //Check Comm status .
            //Read.
            if (!m_bInit || (iCh < 0 || iCh>=(int)EN_AI_CH.EndOfAI))
            {
                return 0;
            }
            //
            try
            {
                CAXA.AxaiSwReadVoltage(iCh, ref dReadVolt);
            }
            catch (Exception err) { Debug.WriteLine($"[TAnalogAjin. GetAI] Exception : {err.Message}"); }

            return  dReadVolt;
        }
        //---------------------------------------------------------------------------
        public double GetAO (EN_AO_CH AO_Ch)
        {
            double  dReadVolt = 0.0;
            int iCh = (int)AO_Ch;

            if (!m_bInit || (iCh < 0 || iCh>=(int)EN_AO_CH.EndOfAO))
            {
                return 0;
            }

            //Read.
            try
            {
                CAXA.AxaoReadVoltage(iCh, ref dReadVolt);
            }
            catch (Exception err) { Debug.WriteLine($"[TAnalogAjin. GetAO] Exception : {err.Message}"); }
            //
            return  dReadVolt;

        }

        //---------------------------------------------------------------------------
        /***************************************************************************/
        /* Update Functions.                                                       */
        /***************************************************************************/
        //---------------------------------------------------------------------------




        /***************************************************************************/
        /* Direct Accessible Functions.                                            */
        /***************************************************************************/
        public double GetMaxPress()
        {
            return MAX_PRESS;
        }
        //-----------------------------------------------------------------------
        public bool m_SetPress(EN_AO_CH Ch, double dPress, double Offset)
        {
            if ((Ch < 0) || (Ch >= EN_AO_CH.EndOfAO)) return false;
            //
            double dPres = LoadToPres(dPress) + Offset;
            double dMpa = PresToLoadMpa(dPres);
            //
            double dSetVol = dMpa * m_dAOCoeff[(int)Ch];
            SetAO(Ch, dSetVol);
            //
            return true;
        }
        //-----------------------------------------------------------------------
        private double m_GetFlow(EN_AI_CH Ch)
        {
            //Local Var.
            int     iCh = (int)Ch;
            double  dMeas;

            //Check.

            //Get Flow Data.
            dMeas = GetAI(Ch);

            //Limit
            //if (dMeas > MAX_IN_VOL_FLOW) dMeas = MAX_IN_VOL_FLOW;
            //if (dMeas < MIN_IN_VOL_FLOW) dMeas = MIN_IN_VOL_FLOW;
            //
            //double dVal = ((dMeas - FLOW_OFF_VOLT) * m_dCoeff[iCh]);
            //
            //return dVal > 0 ? dVal * -1.0 : dVal;
            return dMeas;
        }
        //-----------------------------------------------------------------------
        private double m_GetAI(EN_AI_CH Ch)
        {
            return m_GetFlow(Ch);
        }
        //---------------------------------------------------------------------------
        public double PresToLoadMpa(double Pres)
        {
            //Local Var.
            double S; //C.S.A 단면적
            double L; //Pressure.
        
            //Cal. Pressure.
            S = (Math.PI * (PRESS_CYL_DIA)) / 4.0;
            L = (Pres* S                  ) / 100;
        
            //Return.
            return L;
        }
        //-----------------------------------------------------------------------
        public double PresToLoad(double Press)
        {
            //Local Var.
            double S; //C.S.A 단면적
            double L; //Pressure.

            //Cal. Pressure.
            S = (Math.PI * (PRESS_CYL_DIA)) / 4.0;
            L = (Press   * S) / 9.80665;

            //Return.
            return L;
        }
        //---------------------------------------------------------------------------
        public double LoadToPres(double Load)
        {
            //Local Var.
            double S; //C.S.A 단면적
            double P; //Pressure.
        
            //Cal. Pressure.
            S = (Math.PI * (PRESS_CYL_DIA)) / 4.0        ;
            P = (Load                     ) / S * 9.80665;
        
            //Return.
            return P;
        }


        /***************************************************************************/
        /* Update Functions.                                                       */
        /***************************************************************************/
        public void Update()
        {
            if (!m_bInit) return;

            //In
            for (int Ch = 0; Ch < (int)EN_AI_CH.EndOfAI; Ch++)
            {
                AI[Ch] = m_GetAI((EN_AI_CH)Ch);
                m_dFeedAI[Ch] = AI[Ch];
            }
        }
    }
}
