using System;
using System.Collections.Generic;
using System.Text;

namespace ArconRealTimeSessionMonitor
{
    public class MessageReceivedArgs : EventArgs
    {
        public string Command;
        public string Body;
        public Node RemoteNode;
    }
}
