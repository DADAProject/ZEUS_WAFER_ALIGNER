using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eMachine
{
    /***************************************************************************/
    /* Class: TCOMZEUS                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TCOMZEUS
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */
        private cComunicationAligner COM_Z;

        bool _IsWaferExist => cDEF.SEQ.WAT.IsWaferExist(); //cDEF.IO.gX(EN_IN_ID.xWAFER_EXIST);
        bool _IsVacOn      => cDEF.SEQ.WAT.IsVacOn     (); //cDEF.IO.gX(EN_IN_ID.xVACUUM_ON  );

        public int _GetConCnt => COM_Z.GetConCnt();
        //--------------------------------------------------------------------------
        public void Init(int port)
        {
            //
            COM_Z = new cComunicationAligner(port, true);

            //
            COM_Z.CommandEvent   += AlignerCommandEx        ;
            COM_Z.ReceivedEvent  += WriteAlignerComRevLog   ;
            COM_Z.SendedEvent    += WriteAlignerComSendLog  ;
            COM_Z.ExceptionEvent += WriteAlignerExceptionLog;
            
        }
        //--------------------------------------------------------------------------
        public void AlignerCommandEx(object sender, cCmdData pCommandData)
        {
            bool isAlarm         = pCommandData.Command != eCommand.RST && cDEF.EPU._bHasErr && cDEF.EPU._bHasWrn;
            bool isNotexecutable = CheckExecutable(pCommandData) == false;

            if(isAlarm && cDEF.EPU._bHasErr)
            {
                switch (pCommandData.Command)
                {
                    case eCommand.VER: 
                    case eCommand.RST: 
                    case eCommand.ERR: 
                    case eCommand.MAN: 
                    case eCommand.WCK: 
                    case eCommand.STA: 
                    case eCommand.AYR: 
                    case eCommand.RCP: 
                    case eCommand.BCR: isAlarm = false;   break;
                }
            }
            
            //
            if(isAlarm || isNotexecutable)
            {
                if(isAlarm        ) pCommandData.ErrorNumber = cDEF.EPU.GetLastErrNo();
                //if(isNotexecutable) pCommandData.ErrorNumber = (int) eAlarm.HasAlarm;
                
                //pCommandData.ErrorNumber = (int)eAlarm.NotCommand;
                COM_Z.SetResult(pCommandData);
                return;
            }

            //
            cDEF.SEQ.ClearCmdData();
            cDEF.MAN.ClearCmdData();

            //
            switch (pCommandData.Command)
            {
                case eCommand.AGN: ExecuteAGN(pCommandData); break;
                case eCommand.HOM: ExecuteHOM(pCommandData); break;
                case eCommand.VON: ExecuteVON(pCommandData); break;
                case eCommand.VOF: ExecuteVOF(pCommandData); break;
                case eCommand.TRR: ExecuteTRR(pCommandData); break;
                case eCommand.TLL: ExecuteTLL(pCommandData); break;
                case eCommand.VER: ExecuteVER(pCommandData); break;
                case eCommand.RST: ExecuteRST(pCommandData); break;
                case eCommand.ERR: ExecuteERR(pCommandData); break;
                case eCommand.AUT: ExecuteAUT(pCommandData); break;
                case eCommand.MAN: ExecuteMAN(pCommandData); break;
                case eCommand.WCK: ExecuteWCK(pCommandData); break;
                case eCommand.STA: ExecuteSTA(pCommandData); break;
                case eCommand.AYR: ExecuteAYR(pCommandData); break;
                case eCommand.RCP: ExecuteRCP(pCommandData); break;
                case eCommand.INT: ExecuteINT(pCommandData); break;
                case eCommand.BCR: ExecuteBCR(pCommandData); break;

                default:
                    pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0099;
                    COM_Z.SetResult(pCommandData);
                    break;
            }

            //
            if (pCommandData.ErrorNumber > 0) cDEF.EPU.SetErr(pCommandData.ErrorNumber);

        }
        //------------------------------------------------------------------------
        private void WriteAlignerExceptionLog(object pSender, Exception ex)
        {
            cDEF.LOG.ExceptionTrace(ex.Message); //JUNG/221101
        }
        //------------------------------------------------------------------------
        private void WriteAlignerComSendLog(object pSender, byte[] pData)
        {
            cTcpClientBase client = pSender as cTcpClientBase;
            string stemp = string.Format($"[SND] [{client.SocketRemoteEndPoint.Address}] {Encoding.ASCII.GetString(pData)}");
            cDEF.LOG.TCPIPTrace(stemp); //JUNG/221031
        }
        //------------------------------------------------------------------------
        private void WriteAlignerComRevLog(object pSender, byte[] pData)
        {
            cTcpClientBase client = pSender as cTcpClientBase;
            string stemp = string.Format($"[REV] [{client.SocketRemoteEndPoint.Address}] {Encoding.ASCII.GetString(pData)}");
            cDEF.LOG.TCPIPTrace(stemp); //JUNG/221031
        }
        //--------------------------------------------------------------------------
        private bool CheckExecutable(cCmdData pCommandData)
        {
            //Check Mode
            switch (pCommandData.Command)
            {
                case eCommand.AGN:
                case eCommand.HOM:
                case eCommand.AYR:
                case eCommand.RCP:
                    if (!cDEF.SEQ._bAutoMode)
                    {
                        pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0012;
                        cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0012); 
                        return false;
                    }
                    break;

              //JUNG/231004/제우스 요청으로 Vacuum On/Off는 Auto일때도 동작하도록 변경
              //case eCommand.VON:
              //case eCommand.VOF:

                case eCommand.INT:
                    if (cDEF.SEQ._bAutoMode)
                    {
                        pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0011;
                        cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0011);
                        return false;
                    }
                    break;
            }
            return true;
        }
        //--------------------------------------------------------------------------
        private void ExecuteAGN(cCmdData pCommandData)
        {
            bool useBcr   = cDEF.FM.EngrOptn.bUseBCR ;
            bool bSimMode = cDEF.FM.SysOptn.bSimulRun;
            
            if (bSimMode)
            {
                useBcr = true; //???
                if (useBcr)
                {
                    //pCommandData.ErrorNumber = (int)eAlarm.BarcodeNotFound;
                    //pCommandData.Result = $"@AGN [1,-2,3/4,-5,6/{"ABCD"}]";
                    //pCommandData.Result = $"@AGN [1,-2,3/4,-5,6/{"NO_READ"}]";
                    pCommandData.Result = $"@AGN [0,0,0/0,0,0/{"ABCD"}]";
                }
                else
                {
                    pCommandData.Result = $"@AGN [0,0,0/0,0,0]";
                }

                COM_Z.SetResult(pCommandData);
                return; 
            }
            else
            {
                if(!cDEF.SEQ.IsAllHomeEnd())
                {
                    pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0001;
                    COM_Z.SetResult(pCommandData);
                    return; 
                }

                //Align 동작
                cDEF.SEQ.WAT.SetCmdData(pCommandData);
                cDEF.SEQ.WAT.SetReqAlign();
            }
        }
        //--------------------------------------------------------------------------
        public void SetResult(cCmdData pCommandData)
        {
            COM_Z.SetResult(pCommandData);
        }
        //--------------------------------------------------------------------------
        private void ExecuteVER(cCmdData pCommandData)
        {
            string ver    = cDEF.FM._sVersion       ;
            bool   useBcr = cDEF.FM.EngrOptn.bUseBCR;
            
            if (useBcr)
            {
                ver = $"{ver}_B";
            }
            
            COM_Z.SetResultVER(pCommandData, ver);
        }
        //--------------------------------------------------------------------------
        private void ExecuteRST(cCmdData pCommandData)
        {

            //
            if(cDEF.SEQ._bRun && cDEF.EPU._bHasErr)
            {
                cDEF.SEQ._bReqReset = true;
                cDEF.SEQ.SetCmdData(pCommandData);
            }
            else
            {
                cDEF.SEQ.Reset();
                if (cDEF.SEQ._bAutoMode && cDEF.SEQ.IsAllHomeEnd()) cDEF.SEQ._bBtnManStart = true; 
                COM_Z.SetResult(pCommandData);
            }
        }
        //--------------------------------------------------------------------------
        private void ExecuteERR(cCmdData pCommandData)
        {
            int nErrNo = cDEF.EPU.GetLastErrNo();

            if (nErrNo > 0)
            {
                COM_Z.SetResultERR(pCommandData, nErrNo);
            }
            else
            {
                COM_Z.SetResultERR(pCommandData, 0);
            }

        }
        //--------------------------------------------------------------------------
        private void ExecuteHOM(cCmdData pCommandData) 
        {
            //대기 위치로 이동
            //if(cDEF.SEQ.WAT.IsLocateWait(true))
            //{
            //    //끝나고 응답
            //
            //}

            //
            cDEF.SEQ.WAT.SetCmdData(pCommandData);
            cDEF.SEQ.WAT.SetReqWait(); 
        }
        //--------------------------------------------------------------------------
        private void ExecuteVON(cCmdData pCommandData)
        {
            bool r1 = cDEF.IO.sY(EN_OUT_ID.yVACUUM_ON, true);
            bool r2 = cDEF.IO.sY(EN_OUT_ID.yVACUUM_PURGE, false);

            //
            //COM_Z.SetResult(pCommandData);
            if (r1 && r2) COM_Z.SetResult(pCommandData);
            
        }
        //--------------------------------------------------------------------------
        private void ExecuteVOF(cCmdData pCommandData) 
        { 
            bool r1 = cDEF.IO.sY(EN_OUT_ID.yVACUUM_ON   , false); 
            bool r2 = cDEF.IO.sY(EN_OUT_ID.yVACUUM_PURGE, true );

            //
            if (r1 && r2) COM_Z.SetResult(pCommandData);
        }
        //--------------------------------------------------------------------------
        private void ExecuteTRR(cCmdData pCommandData) 
        { 
            //SeqManual.StartStep(pCommandData, cSeqManualMotion.eStep.MOVE_CW); 

            //??? 차후 적용

        }
        //--------------------------------------------------------------------------
        private void ExecuteTLL(cCmdData pCommandData) 
        {
            //SeqManual.StartStep(pCommandData, cSeqManualMotion.eStep.MOVE_CCW); 

            //??? 차후 적용
        }
        //--------------------------------------------------------------------------
        private void ExecuteAUT(cCmdData pCommandData)
        {
            if(cDEF.SEQ._bAutoMode)
            {
                COM_Z.SetResult(pCommandData);
            }
            else
            {
                if(cDEF.MAN._iManNo < 1)
                {
                    
                    if (cDEF.SEQ.IsAllHomeEnd() && !cDEF.EPU._bHasErr)
                    {
                        cDEF.SEQ._bAutoMode = true;
                        cDEF.SEQ._bBtnManStart = true;

                        cDEF.SEQ.SetCmdData(pCommandData);
                    }
                    else
                    {
                        //250123 Jaewon
                        pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0001;
                        COM_Z.SetResult(pCommandData);
                    }
                }
                else
                {
                    pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0004;
                    COM_Z.SetResult(pCommandData);
                }
            }
           
        }
        //--------------------------------------------------------------------------
        private void ExecuteMAN(cCmdData pCommandData)
        {
            if (cDEF.SEQ._bAutoMode)
            {
                cDEF.SEQ._bBtnManStop = true;
                cDEF.SEQ._bAutoMode   = false;

                cDEF.SEQ.SetCmdData(pCommandData);
            }
            else
            {
                //pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0004;
                COM_Z.SetResult(pCommandData);
            }

            
        }
        //--------------------------------------------------------------------------
        private void ExecuteSTA(cCmdData pCommandData)
        {
            bool isVacuumOn   =  cDEF.FM.SysOptn.bSimulRun? cDEF.IO.gY(EN_OUT_ID.yVACUUM_ON) : cDEF.IO.gX(EN_IN_ID.xVACUUM_ON);
            bool isManual     = !cDEF.SEQ._bAutoMode;
            bool IsWaferExist = _IsWaferExist;
            int  state = 0;
            
                 if(!cDEF.SEQ.IsAllHomeEnd()   ) state = 3; // Not Init
            else if( cDEF.EPU._bHasErr         ) state = 2; // Is Alarm
            else if( cDEF.SEQ.WAT._bDrngAlgn   ) state = 1; // Busy

            COM_Z.SetResultSTA(pCommandData, state, isVacuumOn, isManual, IsWaferExist);
        }
        //--------------------------------------------------------------------------
        private void ExecuteWCK(cCmdData pCommandData)
        {
            //bool waferExist = _IsWaferExist;
            //COM_Z.SetResultWCK(pCommandData, waferExist);

            //JUNG/231106/Vacuum Check Option 추가
            cDEF.MAN.SetCmdData(pCommandData);
            cDEF.MAN._bReqWCK = true; 

        }
        //--------------------------------------------------------------------------
        public void SetResultWCK(cCmdData pCommandData, bool set)
        {
            COM_Z.SetResultWCK(pCommandData, set);
        }

        //--------------------------------------------------------------------------
        private void ExecuteAYR(cCmdData pCommandData)
        {
            bool isHomePosition = cDEF.SEQ.WAT.IsLocateWait(false);
            
            if (!cDEF.SEQ.IsAllHomeEnd())
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0001;
                cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0001, true);
            }
            else if (cDEF.SEQ.WAT._bDrngAlgn || cDEF.MAN._bHoming)
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0004;
            }
            else if (_IsVacOn)
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0002;
                cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0002, true);
            }
            else if (_IsWaferExist)
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0006;
                cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0006, true);
            }
            else if (!isHomePosition)
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0010;
                cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0010, true);
            }

            COM_Z.SetResult(pCommandData);
        }
        //--------------------------------------------------------------------------
        private void ExecuteRCP(cCmdData pCommandData)
        {
            string sPath       = Application.StartupPath + "\\Project";
            string sRcpName    = pCommandData.Argument;
            string dir         = sPath + "\\" + sRcpName;
            //string name        = pCommandData.Argument.Replace("\n","");
            if(sRcpName == "" || sRcpName == string.Empty)
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0007;
                cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0007, true);
            }

            //
            if (Directory.Exists(dir))
            {
                cDEF.FM.LoadProj　　(true, sRcpName);
                cDEF.FM.ApplyProject(　　　sRcpName);
            }
            else
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0007;
                cDEF.EPU.SetErr(EN_ERR_LIST.ERR_0007, true);
            }
             
            //
            COM_Z.SetResult(pCommandData);
        }
        //--------------------------------------------------------------------------
        private void ExecuteINT(cCmdData pCommandData)
        {
            if(cDEF.EPU._bHasErr)
            {
                pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0009;
                COM_Z.SetResult(pCommandData);
                return;
            }
            //else if (!cDEF.MOTR.IsAllServoOn())
            //{
            //    pCommandData.ErrorNumber = (int)EN_ERR_LIST.ERR_0001;
            //    COM_Z.SetResult(pCommandData);
            //    return;
            //}

            //
            cDEF.MAN.SetCmdData(pCommandData);
            cDEF.MAN.ManProcOn(1, true, false); //Home 끝나면...Return 
        }
        //--------------------------------------------------------------------------
        private void ExecuteBCR(cCmdData pCommandData)
        {
            //Task.Run(() =>
            //{
            //    // string resut = cDevice.BCRReader.ExecCommand("LON");
            //    // if (string.IsNullOrEmpty(resut) == false)
            //    // {
            //    //     COM_Z.SetResultBCR(pCommandData, resut);
            //    // }
            //    // else
            //    // {
            //    //     pCommandData.ErrorNumber = (int)eAlarm.NotCommand;
            //    //     COM_Z.SetResult(pCommandData);
            //    // }
            //    //     cDevice.BCRReader.ExecCommand("LOFF");
            //});



            //Request Barcode Reading... ???
        }
        //--------------------------------------------------------------------------
        public void Close()
        {
            COM_Z.Close();
        }

    }


}
