using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using static ArconRealTimeSessionMonitor.Enum;

namespace ArconRealTimeSessionMonitor
{
    public class RealTimeServiceLog
    {
        public int ServiceSessionId { get; set; }

        public int UserSessionId { get; set; }

        public int ServiceId { get; set; }

        public int UserId { get; set; }

        public string IPAddressNAT { get; set; }

        public string IPAddressLan { get; set; }

        public int Port1 { get; set; }

        public int Port2 { get; set; }

        public RealTimeState State { get; set; }

        public int IsRealTimeSessionMonitoringActive { get; set; }

        public bool ResetRealTimeServiceLog { get; set; }

        public int TimeInterval { get; set; }
    }
}
