using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel.Channels;
using System.Threading;

namespace eMachine
{
    public delegate void ChangeConnedtedHandler   (object pSender, bool pIsConnected);
    public delegate void CommunicationEventHandler(object pSender, byte[] pData     );
    public delegate void ExceptionEventHandler    (object pSender, Exception ex     );

    public class cTcpClientBase
    {
        #region # Events #

        public          event ChangeConnedtedHandler    ChangeConnedtedEvent;
        public virtual  event CommunicationEventHandler SendedEvent;
        public virtual  event CommunicationEventHandler ReceivedEvent;
        public virtual  event ExceptionEventHandler     ExceptionEvent;
        public          event EventHandler              DisposeEvent;
        #endregion

        #region # Fields #

        private readonly Thread mRecvThread;
        private readonly byte[] mBuffer  = null;

        private bool   mConnected        = false;
        private bool   mServiceIsStarted = false;
        private bool   mEnable           = false;

        private readonly List<byte> mReceiveDataBuffer = new List<byte>();
        #endregion

        #region # Properties #
        protected Socket Socket                        { get; private set;                        }

        public IPEndPoint       RemoteEndPoint         { get; private set;                        }
        public BufferManager    BufferManager          { get; private set;                        }
        public int              SocketBufferSize       { get; protected set;                      }

        public bool             UseReConnect           { get; protected set;                      }
        public bool             IsDisposed             { get; protected set;                      }

        public IPEndPoint       SocketRemoteEndPoint   { get => (IPEndPoint)Socket.RemoteEndPoint;}
        public bool             IsEnable 
        {
            get { return mEnable; }
            set
            {
                mEnable = value;
                if (mEnable == false) Disconnect();
            }
        }
        //--------------------------------------------------------------------------
        public bool            IsConnected
        {
            get
            {
                if (Socket == null)
                    return false;
                else
                    return mConnected && Socket.Connected;
            }
            set
            {
                if (mConnected == value)
                    return;
                else
                {
                    if (mConnected != value)
                    {
                        mConnected = value;

                        ChangeConnedtedEvent?.Invoke(this, mConnected);
                    }
                }
            }
        }
        //--------------------------------------------------------------------------
        public int FixReceivePacketCount { get; protected set; }
        #endregion

        #region # Constructor #
        public cTcpClientBase   () 
        {
            SocketBufferSize = 2048;
            mBuffer          = new byte[2048];
            BufferManager    = BufferManager.CreateBufferManager(0, 2048);
            UseReConnect = true;

            mRecvThread = new Thread(ReceiveThreadMethod)
            {
                IsBackground = true,
                Name = string.Format("Receive Thread")
            };
            mRecvThread.Start();
            
        }
        //--------------------------------------------------------------------------
        public cTcpClientBase   (Socket pSocket) : this()
        {
            IsEnable     = true;
            Socket       = pSocket;
            IsConnected  = Socket.Connected;
            UseReConnect = false;
        }
        //--------------------------------------------------------------------------
        public cTcpClientBase   (IPEndPoint remoteEndPoint, bool pEnable) : this()
        {
            IsEnable       = pEnable;
            RemoteEndPoint = remoteEndPoint;

            //SocketBufferSize = 2048;
            //mBuffer = new byte[2048];
            //BufferManager = BufferManager.CreateBufferManager(0, 2048);


            //mRecvThread = new Thread(ReceiveThreadMethod);
            //mRecvThread.IsBackground = true;
            //mRecvThread.Name = string.Format("Receive Thread");
            //mRecvThread.Start();

        }
        //--------------------------------------------------------------------------
        public void Dispose ()
        {
            Disconnect();
            IsDisposed = true;
            DisposeEvent?.Invoke(this, new EventArgs());
        } 
        #endregion

        #region # Public Virtual Methods #

        //--------------------------------------------------------------------------
        public virtual void Connect         ()
        {
            try
            {
                //if (mServiceIsStarted == false) return;
                if (IsEnable == false) return;
                

                if (!IsDisposed)
                {
                    if (!IsConnected)
                    {
                        
                        Disconnect();
                        if(UseReConnect == false) return;
                        if (!IsDisposed)
                        {
                            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            Socket.Connect(RemoteEndPoint);
                            IsConnected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.Message, ex.StackTrace);
                IsConnected = false;
            }
        }

        public virtual void Disconnect      ()
        {
            try
            {
                if (Socket != null)
                {
                    Socket.Close();
                    Socket = null;
                }

                if (BufferManager != null)
                    BufferManager.Clear();

                IsConnected = false;
                SocketBufferSize = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void OnReceivePacket (byte[] pPacket)
        {
            ReceivedEvent?.Invoke(this, pPacket);
        }

        public virtual void OnSendPacket    (byte[] pPacket)
        {
            SendedEvent?.Invoke(this, pPacket);
        } 
        #endregion

        #region # Public Methods #

        public void SetAddress          (IPAddress pIP, int pPort)
        {
            RemoteEndPoint = new IPEndPoint(pIP, pPort);
        }

        public void SetAddress          (string pIP,int pPort )
        {

            if (IPAddress.TryParse(pIP, out IPAddress ip))
            {
                SetAddress(ip, pPort);

            }
        }

        public void StartService        ()
        {
            mServiceIsStarted = true;
        }

        public bool Send                (byte[] pPacket)
        {
            if (IsConnected == false) return false;
            if (IsEnable == false) return false;

            try
            {
                mReceiveDataBuffer.Clear();
                int nLen = Socket.Send(pPacket);
                if (nLen > 0)
                {
                    OnSendPacket(pPacket);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ExceptionEvent?.Invoke(this, ex);
                Disconnect();
                return false;
            }
        }
//--------------------------------------------------------------------------
        public void ReceiveThreadMethod ()
        {
            while (!IsDisposed)
            {
                try
                {
                    if (!IsConnected)
                    {
                        if(UseReConnect)  Connect();
      
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }

                    int nReadPacketLen = 0;

                    if (FixReceivePacketCount > 0) nReadPacketLen = Socket.Receive(mBuffer, FixReceivePacketCount, SocketFlags.None);
                    else                           nReadPacketLen = Socket.Receive(mBuffer);
                    
                    if (nReadPacketLen.Equals(0))
                    {
                        Disconnect();
                    }
                    else if (nReadPacketLen > 0)
                    {
                        byte[] receiveData = BufferManager.TakeBuffer(nReadPacketLen);
                        Buffer.BlockCopy(mBuffer, 0, receiveData, 0, nReadPacketLen);

                        if (FixReceivePacketCount > 0)
                        {
                            mReceiveDataBuffer.AddRange(receiveData);

                            if (mReceiveDataBuffer.Count >= FixReceivePacketCount)
                            {
                                while (mReceiveDataBuffer.Count > FixReceivePacketCount) mReceiveDataBuffer.RemoveAt(mReceiveDataBuffer.Count - 1);

                                OnReceivePacket(mReceiveDataBuffer.ToArray());
                                mReceiveDataBuffer.Clear();
                            }
                        }
                        else
                        {
                            OnReceivePacket(receiveData);
                            mReceiveDataBuffer.Clear();
                        }

                        Array.Clear(mBuffer, 0, mBuffer.Length);
                        BufferManager.ReturnBuffer(receiveData);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionEvent?.Invoke(this, ex);
                    Disconnect();
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
