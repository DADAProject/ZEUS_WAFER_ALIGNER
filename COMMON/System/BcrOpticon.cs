using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace eMachine
{

    /***************************************************************************/
    /* Class: TBcrOpticon                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TBcrOpticon
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   // Member Var.            
        byte[] m_szTxBuff = new byte[1024];


         //Timer.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TOnDelayTimer  m_tSendTimer  = new TOnDelayTimer();

        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        string     m_sRcvMsg       ;
        bool       m_bErrComm      ; //Communication - 통신` 에러
        string     m_sReadBcr      ; //Read 된 BCR 
 

        //protected: //Inheritable Vars.        

        //public:    //Direct Accessable Vars.  

        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool    _bErr       {get { return m_bErrComm;        } }
        //public string _sReadBcr    {get { return m_sReadBcr;        } }
        public string _sReadBcr    {get { return m_sReadBcr;        } set { m_sReadBcr = value;  } }
        //Objects.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        TSerialUnit RS232;

        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TBcrOpticon()
        {
            RS232            = new TSerialUnit();
            RS232.OnRecieve += new TSerialUnit.OnRecieveMessage(OnRecive);
        }
        //--------------------------------------------------------------------------
        ~TBcrOpticon() 
        { 


        }
        //--------------------------------------------------------------------------
        //Init.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void   Init (string sPortNo) //"COM1"
        {
            RS232.Open(sPortNo, 9600, 8, Parity.None, StopBits.One);
            //
            Reset();
        }
        //--------------------------------------------------------------------------
        public void   Reset()
        {
            //Init. Var.
            m_sRcvMsg    = ""   ;
            m_sReadBcr   = ""   ;
            m_tSendTimer.Clear();   
        }
        //--------------------------------------------------------------------------
        //Functions.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public bool   Read     (bool On)
        {
            //Clear.
            if(On) Reset();

            //Check Port.
            if ( RS232 == null      ) {return false;}
            if (!RS232._IsOpen      ) {return false;}

            m_szTxBuff[0] = 0x02;
            m_szTxBuff[1] = (On) ? Convert.ToByte('Z') : Convert.ToByte('Y');
            m_szTxBuff[2] = 0x03;

            //Write Data.
            bool bRet = RS232.SendByte(m_szTxBuff,3);
            return bRet;
        }
        //--------------------------------------------------------------------------
        void OnRecive(object sender, int len, byte[] data)
        {
            //Local Var.
            string sMsg  ;
            int    iEndPos;
            string sString = FNC.GetByteArrayToString(data, 0, len);

            //Copy Message.
            m_sRcvMsg = m_sRcvMsg + sString;
            sMsg      = m_sRcvMsg;
            iEndPos   = sMsg.IndexOf("\r");
            if(iEndPos < 0 || iEndPos > 14)
                return;         

           if(iEndPos<8 || iEndPos>14) {            
                return;
                }

            m_bErrComm  = false;
            m_sReadBcr  = sMsg.Substring(0, iEndPos);
            m_sRcvMsg   = "";
            Read       (false);

        }
    }
}
