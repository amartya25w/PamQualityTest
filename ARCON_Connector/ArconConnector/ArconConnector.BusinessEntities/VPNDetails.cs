using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessEntities
{
    public class VPNDetails
    {
        public string GatewayIPAddress { get; set; }
        public int GatewayPort { get; set; }
        public string GatewayUserName { get; set; }
        public string GatewayPassword { get; set; }
        public string LocalIPAddress { get; set; }
        public int LocalPort { get; set; }
        public string TargetIPAddress { get; set; }
        public int TargetPort { get; set; }
        public bool IsConnected { get; set; }
        public bool AllowVPNTunnel { get; set; }
        public bool AllowVPNTunnelForDB { get; set; }
        public SshClient SSHClient { get; set; }
    }
}
