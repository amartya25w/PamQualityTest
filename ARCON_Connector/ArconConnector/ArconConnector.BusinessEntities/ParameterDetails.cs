using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessEntities
{
    public class ParameterDetails
    {
        public string ExeName { get; set; }
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public DBConnection DBConnection { get; set; }
        public DBConnection RDPDBConnection { get; set; }
        public bool TakeRDPConsole { get; set; }
        public VPNDetails SSHVPNDetails { get; set; }
        public bool IsUseARCOSWebServiceDTForDatabase { get; set; }
        public string ARCOSWebServiceDTURL { get; set; }
        public bool IsTimeBasedSession { get; set; }
        public string ARCOSMode { get; set; }
        public DBConnection RADBConnection { get; set; }
        public DBConnection RARDPDBConnection { get; set; }
        public bool IsRDPSConnection { get; set; }
        public string ACMOURL
        {
            get
            {
                if (!string.IsNullOrEmpty(ARCOSWebServiceDTURL))
                    return ARCOSWebServiceDTURL.Replace(new Uri(ARCOSWebServiceDTURL).AbsolutePath, "");
                return string.Empty;
            }
        }
        public string ARCOSPuttyWebServiceCLURL
        {
            get
            {
                if (!string.IsNullOrEmpty(ACMOURL))
                    return ACMOURL + "/ARCOSWebAPI/ARCOSSessionCommandLogAPI.asmx";
                return string.Empty;
            }
        }
        public string ARCOSWebServiceDTCredential
        {
            get { return "ARCOSClient&Server@2013"; }
        }
        //public ServicePointManager ServicePointManager { get; set; }
    }
}
