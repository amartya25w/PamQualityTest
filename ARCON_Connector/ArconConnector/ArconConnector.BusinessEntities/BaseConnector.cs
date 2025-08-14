using ArconImageRecorder;
using ArconRealTimeSessionMonitor;
using System.Diagnostics;

namespace ArconConnector.BusinessEntities
{
    public class BaseConnector
    {
        public UserDetails UserDetails { get; set; }

        public ServiceDetails ServiceDetails { get; set; }

        //public SessionDetails SessionDetails { get; set; }

        public ParameterDetails ParameterDetails { get; set; }

        public VPNDetails VPNDetails { get; set; }

        public Process ProcessDetails { get; set; }

        public ImageRecorder ImageRecorder { get; set; }

        public SessionMonitor SessionMonitor { get; set; }

        public string ApplicationPath { get; set; }

    }
}
