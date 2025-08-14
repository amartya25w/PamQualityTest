using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArconConnector.BusinessEntities
{
    public class ServiceConnRetry
    {
        public Process Process { get; set; }
        public Exception Exception { get; set; }
        public Form ProcessForm { get; set; }
        public string ServiceBaseUrl { get; set; }
        public bool IsFormVisible { get; set; }
        public bool AllowFormClose { get; set; }
        public Action TerminateAction { get; set; }
    }
}
