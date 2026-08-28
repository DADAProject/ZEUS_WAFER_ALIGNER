

namespace eMachine
{
    public class cCmdData
    {
        private bool mIsError;

        public  bool Vailid {get; private set;}

        public eCommand         Command    {get; private set;}
        public cTcpClientBase   Socket     {get; private set;}
        public string           Result     {get;         set;}
        public string           Argument  {get;         set;}

        private int mErrorNumber;

        public int ErrorNumber
        {
            get
            {
                return mErrorNumber;
            }
            set
            {
                mErrorNumber = value;
                if(mErrorNumber > 0)
                {
                    //Result  = $"@E{mErrorNumber:00} {cDEF.EPU.Err[mErrorNumber].m_sName}";
                    Result  = $"@E{mErrorNumber:00}";
                }
            }
        }

        //public bool             IsError
        //{
        //    get
        //    {
        //        return mIsError;
        //    }
        //    set
        //    {
        //        mIsError = value;
        //        if(mIsError)
        //        {
        //            Result  = $"@ERR";
        //        }
        //    }
        //}
        //--------------------------------------------------------------------------
        public cCmdData(eCommand pCommand, cTcpClientBase pSocket)
        {
            Vailid       = true          ;
            mIsError     = false         ;
            Command      = pCommand      ;
            Socket       = pSocket       ;
            Result       = $"@{pCommand}";
            Argument     = ""            ;
            mErrorNumber = 0             ;
        }
        //--------------------------------------------------------------------------
        public void Dispose()
        {
            Vailid = false;
        }
        //--------------------------------------------------------------------------
        public void ClearCMD()
        {
            Command = eCommand.None; 
        }
    }
}
