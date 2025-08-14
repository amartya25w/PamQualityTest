using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace ArconRealTimeSessionMonitor
{
    public class StreamAgent
    {
        public IntPtr WindowHandlerToMonitor { get; set; }

        public IntPtr DesktopHandlerToMonitor { get; set; }
       
        public int LocalPort { get; set; }

        public int TimeInterval { get; set; }

        public bool IsSessionFreezed { get; set; }

        public string SessionFreezedMessage { get; set; }

        public Image LastImage { get; set; }

        public EventHandler<MessageReceivedArgs> CloseSessionReceived { get;set; }

        public EventHandler<MessageReceivedArgs> FreezeWindowReceived { get; set; }

        public EventHandler<MessageReceivedArgs> UnfreezeWindowReceived { get; set; }
    }
}
