using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Sample program that a client host connects SR series readers.
/// </summary>
namespace eMachine
{
    public partial class FormBarCode : Form
    {
        private const int READER_COUNT = 2;      // number of readers to connect
        private const int RECV_DATA_MAX = 10240;
        private ClientSocket[] clientSocketInstance;

        /// <summary>
        /// Constructor
        /// </summary>
        public FormBarCode()
        {
            InitializeComponent();

            //
            // Allocate Instances of ClientSocket, and set IP address, command port number and
            // data port number.
            //
            clientSocketInstance = new ClientSocket[READER_COUNT];

            int readerIndex = 0;
            int CommandPort = 9003; // 9003 for command
            int DataPort = 9004; // 9004 for data
            CommandPortInput.Text = Convert.ToString(CommandPort);
            DataPortInput.Text = Convert.ToString(DataPort);

            //
            // First reader to connect.
            //
            byte[] ip1 = { 192, 168, 100, 100 };
            clientSocketInstance[readerIndex++] = new ClientSocket(ip1, CommandPort, DataPort);  // 9003 for command, 9004 for data

            //
            // Second reader to connect.
            //
            byte[] ip2 = { 192, 168, 100, 101 };
            clientSocketInstance[readerIndex++] = new ClientSocket(ip2, CommandPort, DataPort);  // 9003 for command, 9004 for data
        }

        /// <summary>
        /// handler for "Connect" button is clicked
        /// </summary>
        private void connect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < READER_COUNT; i++)
            {
                //
                // Connect to the command port.
                //
                try
                {
                    clientSocketInstance[i].readerCommandEndPoint.Port = Convert.ToInt32(CommandPortInput.Text);
                    clientSocketInstance[i].readerDataEndPoint.Port    = Convert.ToInt32(DataPortInput.Text   );
                    //
                    // Close the socket if opened.
                    //
                    if (clientSocketInstance[i].commandSocket != null)
                    {
                        clientSocketInstance[i].commandSocket.Close();
                    }

                    //
                    // Create a new socket.
                    //
                    clientSocketInstance[i].commandSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    textBox1.Text = clientSocketInstance[i].readerCommandEndPoint.ToString() + " Connecting..";
                    textBox1.Update();

                    clientSocketInstance[i].commandSocket.Connect(clientSocketInstance[i].readerCommandEndPoint);

                    textBox1.Text = clientSocketInstance[i].readerCommandEndPoint.ToString() + " Connected.";
                    textBox1.Update();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    //
                    // Catch exceptions and show the message.
                    //
                    textBox1.Text = clientSocketInstance[i].readerCommandEndPoint.ToString() + " Failed to connect.";
                    textBox1.Update();
                    MessageBox.Show(ex.Message);
                    clientSocketInstance[i].commandSocket = null;
                    continue;
                }
                catch (SocketException ex)
                {
                    //
                    // Catch exceptions and show the message.
                    //
                    textBox1.Text = clientSocketInstance[i].readerCommandEndPoint.ToString() + " Failed to connect.";
                    textBox1.Update();
                    MessageBox.Show(ex.Message);
                    clientSocketInstance[i].commandSocket = null;
                    continue;
                }

                    //
                    // Connect to the data port.
                    //
                    try
                    {
                        //
                        // Close the socket if opend.
                        //
                        if (clientSocketInstance[i].dataSocket != null)
                        {
                            clientSocketInstance[i].dataSocket.Close();
                        }

            //
            // If the same port number is used for command port and data port, unify the sockets and skip a new connection. 
            //
                if (clientSocketInstance[i].readerCommandEndPoint.Port == clientSocketInstance[i].readerDataEndPoint.Port)
                {
                        clientSocketInstance[i].dataSocket = clientSocketInstance[i].commandSocket;
                }
                else
                {
                    //
                    // Create a new socket.
                    //
                        clientSocketInstance[i].dataSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        textBox1.Text = clientSocketInstance[i].readerDataEndPoint.ToString() + " Connecting..";
                        textBox1.Update();

                        clientSocketInstance[i].dataSocket.Connect(clientSocketInstance[i].readerDataEndPoint);

                        textBox1.Text = clientSocketInstance[i].readerDataEndPoint.ToString() + " Connected.";
                        textBox1.Update();
                }
                        //
                        // Set 100 milliseconds to receive timeout.
                        //
                        clientSocketInstance[i].dataSocket.ReceiveTimeout = 100;
                    }

                    catch (SocketException ex)
                    {
                        //
                        // Catch exceptions and show the message.
                        //
                        textBox1.Text = clientSocketInstance[i].readerDataEndPoint.ToString() + " Failed to connect.";
                        textBox1.Update();
                        MessageBox.Show(ex.Message);
                        clientSocketInstance[i].dataSocket = null;
                        continue;
                    }
                }

        }

        /// <summary>
        /// handler for "Disonnect" button is clicked
        /// </summary>
        private void disconnect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < READER_COUNT; i++)
            {
                //
                // Close the command socket.
                //
                if (clientSocketInstance[i].commandSocket != null)
                {
                    clientSocketInstance[i].commandSocket.Close();
                    clientSocketInstance[i].commandSocket = null;
                    textBox1.Text = clientSocketInstance[i].readerCommandEndPoint.ToString() + " Disconnected.";
                    textBox1.Update();
                }

                //
                // Close the data socket.
                //
                if (clientSocketInstance[i].dataSocket != null)
                {
                    clientSocketInstance[i].dataSocket.Close();
                    clientSocketInstance[i].dataSocket = null;
                    textBox1.Text = clientSocketInstance[i].readerDataEndPoint.ToString() + " Disconnected.";
                    textBox1.Update();
                }
            }

        }

        /// <summary>
        /// handler for "Timing ON" button is clicked
        /// </summary>
        private void lon_Click(object sender, EventArgs e)
        {
            //
            // Send "LON" command.
            // 
            string lon = "LON\r";   // CR is terminator
            Byte[] command = ASCIIEncoding.ASCII.GetBytes(lon);

            for (int i = 0; i < READER_COUNT; i++)
            {
                if (clientSocketInstance[i].commandSocket != null)
                {
                    clientSocketInstance[i].commandSocket.Send(command);
                }
                else
                {
                    MessageBox.Show(clientSocketInstance[i].readerCommandEndPoint.ToString() + " is disconnected.");
                }
            }
        }

        /// <summary>
        /// handler for "Timing OFF" button is clicked
        /// </summary>
        private void loff_Click(object sender, EventArgs e)
        {
            //
            // Send "LOFF" command.
            //  
            string loff = "LOFF\r"; // CR is terminator
            Byte[] command = ASCIIEncoding.ASCII.GetBytes(loff);

            for (int i = 0; i < READER_COUNT; i++)
            {
                if (clientSocketInstance[i].commandSocket != null)
                {
                    clientSocketInstance[i].commandSocket.Send(command);
                }
                else
                {
                    MessageBox.Show(clientSocketInstance[i].readerCommandEndPoint.ToString() + " is disconnected.");
                }
            }
        }

        /// <summary>
        /// handler for "Receive Data" button is clicked
        /// </summary>
        private void receive_Click(object sender, EventArgs e)
        {
            Byte[] recvBytes = new Byte[RECV_DATA_MAX];
            int recvSize = 0;

            for (int i = 0; i < READER_COUNT; i++)
            {
                if (clientSocketInstance[i].dataSocket != null)
                {
                    try
                    {
                        recvSize = clientSocketInstance[i].dataSocket.Receive(recvBytes);
                    }
                    catch (SocketException)
                    {
                        //
                        // Catch the exception, if cannot receive any data.
                        //
                        recvSize = 0;
                    }
                }
                else
                {
                    MessageBox.Show(clientSocketInstance[i].readerDataEndPoint.ToString() + " is disconnected.");
                    continue;
                }

                if (recvSize == 0)
                {
                    MessageBox.Show(clientSocketInstance[i].readerDataEndPoint.ToString() + " has no data.");
                }
                else
                {
                    //
                    // Show the receive data after converting the receive data to Shift-JIS.
                    // Terminating null to handle as string.
                    //
                    recvBytes[recvSize] = 0;
                    MessageBox.Show(clientSocketInstance[i].readerDataEndPoint.ToString() + "\r\n" + Encoding.GetEncoding("Shift_JIS").GetString(recvBytes));
                }
            }
        }
        //
        // Reject inputs except numbers in text boxes.
        //
        private void PortInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }      


    }

    /// <summary>
    /// Socket class for a reader.
    /// </summary>
    class ClientSocket
    {
        public Socket commandSocket;   // socket for command
        public Socket dataSocket;      // socket for data
        public IPEndPoint readerCommandEndPoint;
        public IPEndPoint readerDataEndPoint;

        public ClientSocket(byte[] ipAddress, int readerCommandPort, int readerDataPort)
        {
            IPAddress readerIpAddress = new IPAddress(ipAddress);
            readerCommandEndPoint     = new IPEndPoint(readerIpAddress, readerCommandPort);
            readerDataEndPoint        = new IPEndPoint(readerIpAddress, readerDataPort);
            commandSocket             = null;
            dataSocket                = null;
        }
    }
}
