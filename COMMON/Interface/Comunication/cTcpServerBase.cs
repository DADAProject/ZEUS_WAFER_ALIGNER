using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace eMachine
{
    public class cTcpServerBase 
    {
        #region # Events #
        
        public virtual event CommunicationEventHandler SendedEvent   ;
        public virtual event CommunicationEventHandler ReceivedEvent ;
        public virtual event ExceptionEventHandler     ExceptionEvent;
        #endregion

        #region # Fields #
        protected readonly List<cTcpClientBase> mConnectionList = new List<cTcpClientBase>();

        private readonly Thread mServerThread;
        private readonly int    mMaxConnection    = 1;   

        private Socket mServerSocket     = null;
        private bool   mEnable           = false;
        private int    mPort             = 5500;
        #endregion

        #region # Properties #
        public bool IsDisposed { get; protected set; }
        //--------------------------------------------------------------------------
        public int ServerPort 
        { 
            get
            {
                return mPort;
            }
            private set
            {
                if(mPort != value)
                {
                     mPort = value;
                    if(mServerSocket != null) mServerSocket.Dispose();
                    mServerSocket = null;
                    
                }
            }
        }
        //--------------------------------------------------------------------------
        public bool IsEnable 
        {
            get { return mEnable; }
            set
            {
                mEnable = value;
                if (mEnable == false)
                {
                    if(mServerSocket != null) mServerSocket.Dispose();
                    mServerSocket = null;
                    foreach(cTcpClientBase client in mConnectionList) client.Dispose();
                    mConnectionList.Clear();
                }
            }
        }
        //--------------------------------------------------------------------------
        public int ConnectionCount {get { return mConnectionList.Count; } }

        #endregion
        //--------------------------------------------------------------------------
        #region # Constructor #
        public cTcpServerBase() 
        {
            mServerThread = new Thread(ServerThreadMethod)
            {
                IsBackground = true,
                Name = string.Format("ServerThread")
            };
            mServerThread.Start();
        }
        //--------------------------------------------------------------------------
        public cTcpServerBase(int pServerPort, bool pEnable) : this()
        {
            ServerPort = pServerPort;
            IsEnable = pEnable;
        }
        //--------------------------------------------------------------------------
        public void Dispose()
        {
            IsDisposed = true;
        }
        #endregion

        #region # Methods #
        //--------------------------------------------------------------------------
        protected virtual void OnReceivePacket(object pSender, byte[] pPacket)
        {
            ReceivedEvent?.Invoke(pSender, pPacket);
        }
        //--------------------------------------------------------------------------
        protected virtual void OnSendPacket(object pSender, byte[] pPacket)
        {
            SendedEvent?.Invoke(pSender, pPacket);
        }
        //--------------------------------------------------------------------------
        protected Socket ServerOpen()
        {
            try
            {
                var ip = new IPEndPoint(IPAddress.Any, ServerPort);

                mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    mServerSocket.Bind(ip);

                    mServerSocket.Listen(20);
                }
                catch (Exception) { throw; }

                Socket client = mServerSocket.Accept();

                Thread.Sleep(1);
                mServerSocket.Close();
                mServerSocket = null;
                return client;

            }
            catch (Exception) 
            { 
                if(mServerSocket != null) mServerSocket.Close();
                mServerSocket = null;
                throw; 
            }
        }
        //--------------------------------------------------------------------------
        private void ServerThreadMethod()
        {
            while (!IsDisposed)
            {
                try
                {
                    Thread.Sleep(1);
                    if(IsEnable == false) continue;

                    cTcpClientBase disconnectedClient = mConnectionList.FirstOrDefault(p => p.IsConnected == false);
                    if(disconnectedClient != null)
                    {
                        disconnectedClient.Dispose();
                        mConnectionList.Remove(disconnectedClient);
                    }

                    if(mConnectionList.Count < mMaxConnection)
                    {
                        Socket client = ServerOpen();
                        cTcpClientBase tcpClient = new cTcpClientBase(client);
                        tcpClient.ReceivedEvent += OnReceivePacket;
                        tcpClient.SendedEvent   += OnSendPacket;
                        mConnectionList.Add(tcpClient);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionEvent?.Invoke(this, ex);
                }
                finally
                {
                    Thread.Sleep(1);
                }
            }
        }
        #endregion
    }
}
