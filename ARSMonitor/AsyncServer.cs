using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.ComponentModel;

namespace ARSMonitor
{
    ///////////////////////////////////////////////////////
    ///////// async serv
    ///////////////////////////////////////////////////////



    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }



    public class AsynchronousSocketListener
    {
        private ManualResetEvent sendDone = new ManualResetEvent(false);

        public event EventHandler<NetworkEventArgs> eventFromNetworkClass;


        public static bool stopListen = false;
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        MainForm parent;

        public AsynchronousSocketListener(MainForm main)
        {
            parent = main;
        }

        /*
        BackgroundWorker bw = new BackgroundWorker();
        
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string s = e.Argument as string;
            showM(s);
            shownDone.WaitOne();
        }*/



        private ManualResetEvent shownDone =
            new ManualResetEvent(false);

        public void showM(string s)
        {
            Thread.Sleep(2);

            Task.Factory.StartNew((Action)delegate
            {
                if (eventFromNetworkClass != null)
                {
                    eventFromNetworkClass(this, new NetworkEventArgs(s));
                }
            });
            shownDone.Set();
        }

        public Socket listener;

        public void StartListening(System.ComponentModel.BackgroundWorker worker,
                                        System.ComponentModel.DoWorkEventArgs eW)
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());

            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse("192.168.0.160");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 3399);

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                //listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.AcceptConnection, 1);
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (!worker.CancellationPending)
                {
                    System.Threading.Thread.Sleep(100);
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    //worker.ReportProgress(1, "Waiting for a connection in " + localEndPoint.ToString() + "...");
                    showM("Waiting for a connection in " + localEndPoint.ToString() + "...");
                    shownDone.WaitOne();
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                showM(e.ToString());
                shownDone.WaitOne();
            }

            showM("Press enter...");
            shownDone.WaitOne();
            //Console.Read();
            //stopListen = false;
        }


        public void AcceptCallback(IAsyncResult ar)
        {
            //System.Threading.Thread.Sleep(100);
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);

            parent.eventFromMainForm += delegate(object sender, SocketEventArgs e)
            {
                if ((((IPEndPoint)handler.RemoteEndPoint).Address.ToString() == e.address) && (handler.Connected))
                {
                    Send(handler, e.programm + "<EOC>");
                    sendDone.WaitOne();
                    Send(handler, e.parameters + "<EOPN>");
                    sendDone.WaitOne();
                    Send(handler, "<SUBMIT>");
                    sendDone.WaitOne();
                }
                else showM("Handler is offline");
            };
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    // Echo the data back to the client.
                    if (content == "shutdown<EOF>")
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                    else Send(handler, "");
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        public void Send(Socket handler, String data)
        {


            //System.Threading.Thread.Sleep(100);
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            //System.Threading.Thread.Sleep(100);
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                showM("Sent " + bytesSent.ToString() + " bytes to " + ((IPEndPoint)handler.RemoteEndPoint).Address.ToString());
                shownDone.WaitOne();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
                showM(e.ToString());
                shownDone.WaitOne();
            }
            sendDone.Set();
        }
    }
}
