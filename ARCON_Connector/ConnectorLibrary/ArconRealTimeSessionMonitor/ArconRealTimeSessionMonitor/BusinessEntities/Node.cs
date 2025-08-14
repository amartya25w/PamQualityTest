using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ArconRealTimeSessionMonitor
{
    public class Node
    {
        public Hashtable Attributes = new Hashtable();
        public IPEndPoint EndPoint;
        public Node(IPEndPoint ep)
        {
            this.EndPoint = ep;
        }

        public string GetHostName()
        {
            return Dns.GetHostEntry(EndPoint.Address).HostName;
        }

        public override string ToString()
        {
            return EndPoint.Address.ToString() + ":" +
                EndPoint.Port.ToString();
        }
    }
}
