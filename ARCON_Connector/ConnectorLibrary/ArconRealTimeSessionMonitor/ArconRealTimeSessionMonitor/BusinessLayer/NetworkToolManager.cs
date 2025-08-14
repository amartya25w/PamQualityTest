using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ArconRealTimeSessionMonitor
{
    public class NetworkToolManager
    {
        #region Variables
        public delegate void MessageReceivedDelegate(MessageReceivedArgs e);
        public MessageReceivedDelegate MessageReceivedCallback;
        public NetworkTool objNetworkTool;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public NetworkToolManager()
        {
            objNetworkTool = new NetworkTool() { Connections = new Hashtable() };
        }

        public void BroadcastMessage(string command, string body)
        {
            try
            {
                foreach (DictionaryEntry obj in objNetworkTool.Connections)
                {
                    Node node = obj.Value as Node;
                    SendMessage(node, command, body);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void SendMessage(Node remoteNode, string command, string body)
        {
            try
            {
                string localHostName = Dns.GetHostEntry("localhost").HostName;
                TcpClient client = new TcpClient(remoteNode.EndPoint.Address.ToString(), remoteNode.EndPoint.Port);
                client.SendBufferSize = 999999;
                client.ReceiveBufferSize = 999999;

                Stream s = client.GetStream();
                StreamWriter sw = new StreamWriter(s);
                sw.AutoFlush = true;
                sw.Write(localHostName + "#" + objNetworkTool.PortNo.ToString() + "#" + objNetworkTool.ServerName + "#" + command + "#" + body);
                sw.Flush();
                sw.Dispose();
                s.Dispose();
                sw.Close();
                s.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void StartServer(int portNo, MessageReceivedDelegate callBack, string serverName = "")
        {
            try
            {
                if (string.IsNullOrEmpty(serverName))
                    serverName = "USR_" + Dns.GetHostEntry("localhost").HostName;
                objNetworkTool.ServerName = serverName;
                objNetworkTool.PortNo = portNo;
                objNetworkTool.IsRunning = true;
                MessageReceivedCallback = callBack;
                Thread thread = new Thread(() => { ListenTcpRequests(); });
                thread.IsBackground = true;
                thread.Start();
                //System.Threading.Thread.Sleep(250);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void StopServer()
        {
            try
            {
                objNetworkTool.IsRunning = false;
                objNetworkTool.TcpServerSocket.Close();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void ListenTcpRequests()
        {
            try
            {
                objNetworkTool.TcpServerSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.IP);
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, objNetworkTool.PortNo);
                objNetworkTool.TcpServerSocket.Bind(ep);
                objNetworkTool.TcpServerSocket.Listen(10);
                while (objNetworkTool.IsRunning)
                {
                    Socket soc = objNetworkTool.TcpServerSocket.Accept(); //Listener.AcceptSocket(); //blocks until a tcp connection request is received
                    Stream ns = new NetworkStream(soc);
                    StreamReader sr = new StreamReader(ns);
                    string message = sr.ReadToEnd();

                    //decode the entire message
                    //MACHINE#PORT#SENDER_NAME#COMMAND#BODY
                    if (message.Trim().Length <= 0)
                        continue;

                    var arrMsgData = message.Split('#');
                    string machineName = arrMsgData[0];
                    int portNo = Convert.ToInt32(arrMsgData[1]);
                    string sender = arrMsgData[2];
                    string command = arrMsgData[3];
                    string body = arrMsgData[4];

                    //parse the client
                    Node rnode = null;
                    IPEndPoint remoteEp = new IPEndPoint((soc.RemoteEndPoint as IPEndPoint).Address, portNo);

                    if (!objNetworkTool.Connections.Contains(remoteEp.Address.ToString() + ":" + portNo.ToString()))
                    {
                        rnode = new Node(remoteEp);
                        objNetworkTool.Connections.Add(remoteEp.Address.ToString() + ":" + portNo.ToString(), rnode);
                    }

                    //raise the event
                    MessageReceivedArgs e = new MessageReceivedArgs();
                    e.RemoteNode = objNetworkTool.Connections[remoteEp.Address.ToString() + ":" + portNo.ToString()] as Node; //client;
                    e.Command = command;
                    e.Body = body;

                    if (command == "$disconnect")
                        objNetworkTool.Connections.Remove(sender + ":" + portNo.ToString());
                    else
                        MessageReceivedCallback.Invoke(e);

                    //clean up
                    sr.Dispose();
                    ns.Dispose();

                    sr.Close();
                    ns.Close();
                    soc.Close();
                }
                BroadcastMessage("$disconnect", "");
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
