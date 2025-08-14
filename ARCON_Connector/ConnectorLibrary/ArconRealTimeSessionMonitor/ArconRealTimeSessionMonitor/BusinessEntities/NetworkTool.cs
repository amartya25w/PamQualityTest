using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ArconRealTimeSessionMonitor
{
    public class NetworkTool
    {
        public string ServerName { get; set; } //name of this server
        public int PortNo { get; set; }
        public Hashtable Connections { get; set; } //all clients (ip-end points) associated with this server
        public bool IsRunning { get; set; } //determines whether server is running or not
        public Socket TcpServerSocket { get; set; }
    }
}
