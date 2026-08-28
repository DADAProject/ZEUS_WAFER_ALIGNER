using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace eMachine
{
    public class cComunicationAligner : cTcpServerBase
	{
        public event EventHandler<cCmdData> CommandEvent  = null;


        private readonly ConcurrentQueue<cCmdData> mReceivedDataQueue  = new ConcurrentQueue<cCmdData>();
        private readonly Thread                    mProcessThread      = null;


        public int GetConCnt() { return ConnectionCount;}

        public cComunicationAligner(int pServerPort, bool pEnable): base(pServerPort, pEnable)
        {
            mProcessThread = new Thread(Process)
            {
                IsBackground = true,
                Name = string.Format("cZeus.mProcessThread")
            };
            mProcessThread.Start();
        }
        //--------------------------------------------------------------------------
		protected override void OnReceivePacket(object pSender, byte[] pPacket)
        {
            if (pPacket.Length >= 4)
            {
                string commandString = Encoding.ASCII.GetString(pPacket,1,3);
                string argument      = "";

                if (Enum.TryParse(commandString, out eCommand cmd))
                {
                    if(pPacket.Length > 5)
                    {
                        argument = Encoding.ASCII.GetString(pPacket,5,pPacket.Length - 5 );
                        argument = argument.Trim(); //JUNG/230302
                    }

                    mReceivedDataQueue.Enqueue(new cCmdData(cmd, pSender as cTcpClientBase) { Argument = argument });
                }
            }
            //
            base.OnReceivePacket(pSender, pPacket);
        }
        //--------------------------------------------------------------------------
        public void SetResultVER(cCmdData pResult, string pVer)
        {
            pResult.Socket.Send(Encoding.ASCII.GetBytes($"@{pVer}"));
            pResult.Dispose();
        }
        //--------------------------------------------------------------------------
        public void SetResultAGN(cCmdData pResult, double pX1, double pY1, double pT1, double pX2, double pY2, double pT2)
        {
            if(pResult.ErrorNumber != 0)
            {
                pResult.Socket.Send(Encoding.ASCII.GetBytes(pResult.Result));
            }
            else pResult.Socket.Send(Encoding.ASCII.GetBytes($"@AGN [{pX1},{pY1},{pT1}/{pX2},{pY2},{pT2}]"));
            pResult.Dispose();
        }
        //--------------------------------------------------------------------------
        public void SetResultERR(cCmdData pResult, int pErrorNumber)
        {
            pResult.Socket.Send(Encoding.ASCII.GetBytes($"@E{pErrorNumber:00}"));
            pResult.Dispose();
        }
        //--------------------------------------------------------------------------
        public void SetResultSTA(cCmdData pResult, int pState, bool pIsVacOn, bool pIsManual, bool pIsWaferExist)
        {
            pResult.Socket.Send(Encoding.ASCII.GetBytes(string.Format("@{0}{1}{2}{3}", pState, pIsVacOn ? 1 : 0, pIsManual ? 1 : 0, pIsWaferExist ? 1 : 0)));
            pResult.Dispose();
        }
        //--------------------------------------------------------------------------
        public void SetResultWCK(cCmdData pResult, bool pIsWaferExist)
        {
            pResult.Socket.Send(Encoding.ASCII.GetBytes(string.Format("@{0}", pIsWaferExist ? "EXT" : "NOT")));
            pResult.Dispose();
        }
        //--------------------------------------------------------------------------
        public void SetResultBCR(cCmdData pResult, string pCode)
        {
            pResult.Socket.Send(Encoding.ASCII.GetBytes(string.Format("@{0}", pCode)));
            pResult.Dispose();
        }
        //--------------------------------------------------------------------------
        public void SetResult(cCmdData pResult)
        {
            pResult.Socket?.Send(Encoding.ASCII.GetBytes(pResult.Result));
            pResult.Dispose();
        }
        //--------------------------------------------------------------------------
        private void Process()
        {
            while(IsDisposed == false)
            {
                Thread.Sleep(1);

                if(mReceivedDataQueue.Count > 0)
                {
                    if(mReceivedDataQueue.TryDequeue(out cCmdData receivedData))
                    {
                        string ackStr = $"<{receivedData.Command}";
                        receivedData.Socket.Send(Encoding.ASCII.GetBytes(ackStr));

                        //
                        CommandEvent?.Invoke(this, receivedData);
                    }
                }
            }
        }
        //--------------------------------------------------------------------------
        public void Close()
        {
            if (mProcessThread.IsAlive)
            {
                IsDisposed = true;
                if (mProcessThread.Join(1000)) mProcessThread.Abort();
            }

        }

    }
}
