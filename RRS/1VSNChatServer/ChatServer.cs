using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1VSNChatServer
{
    class ChatServer
    {
        public static void Main(string[] args)
        {
            Task.Run(() => RunServer());

            
        }
        public static void RunServer() {
            Server s = new Server();
        }

    }


    public class Server
    {
        TcpListener server = null;
        TcpClient clientSocket = null;
        static int counter = 0;
        string date;

        public const int port = 12321;

        public Dictionary<TcpClient, string> clientList = new Dictionary<TcpClient, string>();

        public Server()
        {
            Thread t = new Thread(InitSocket);
            t.IsBackground = true;
            t.Start();
        }

        public void InitSocket()
        {
            server = new TcpListener(IPAddress.Any, port);
            clientSocket = server.AcceptTcpClient();
            server.Start();

            while (true)
            {
                try
                {
                    NetworkStream stream = clientSocket.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string user_name = Encoding.Unicode.GetString(buffer, 0, bytes);
                    user_name = user_name.Substring(0, user_name.IndexOf("$"));

                    clientList.Add(clientSocket, user_name);

                    handleClient h_client = new handleClient();
                    h_client.OnReceived += new handleClient.MessageDisplayHandler(OnReceived);
                    h_client.OnDisconnected += new handleClient.DisconnectedHandler(h_client_OnDisconnected);
                    h_client.startClient(clientSocket, clientList);
                }
                catch (SocketException se) { break; }
                catch (Exception ex) { break; }
            }
            clientSocket.Close();
            server.Stop();
        }

        void h_client_OnDisconnected(TcpClient clientSocket)
        {
            if (clientList.ContainsKey(clientSocket))
                clientList.Remove(clientSocket);
        }

        private void OnReceived(string message, string user_name)
        {
            
        }
    }

    class handleClient
    {
        TcpClient clientSocket = null;
        public Dictionary<TcpClient, string> clientList = null;

        public void startClient(TcpClient clientSocket, Dictionary<TcpClient, string> clientList)
        {
            this.clientSocket = clientSocket;
            this.clientList = clientList;

            Thread t_handler = new Thread(doChat);
            t_handler.IsBackground = true;
            t_handler.Start();
        }

        public delegate void DisconnectedHandler(TcpClient clientSocket);
        public event DisconnectedHandler OnDisconnected;

        public delegate void MessageDisplayHandler(string message, string user_name);
        public event MessageDisplayHandler OnReceived;

        private void doChat()
        {
            NetworkStream stream = null;

            try
            {
                byte[] buffer = new byte[1024];
                string msg = string.Empty;
                int bytes = 0;
                int MessageCount = 0;

                while (true)
                {
                    MessageCount++;
                    stream = clientSocket.GetStream();
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                    msg = msg.Substring(0, msg.IndexOf("$"));

                    if (OnReceived != null)
                    {
                        OnReceived(msg, clientList[clientSocket].ToString());
                    }
                }
            }
            catch (SocketException se)
            {
                if (clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(clientSocket);

                    clientSocket.Close();
                    stream.Close();
                }
            }
            catch(Exception ex)
            {
                if (clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(clientSocket);

                    clientSocket.Close();
                    stream.Close();
                }
            }
        }
    }
}
