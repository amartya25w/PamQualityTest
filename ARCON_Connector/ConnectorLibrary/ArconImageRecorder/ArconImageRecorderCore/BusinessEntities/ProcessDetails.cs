using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconImageRecorderCore
{
    public class ProcessDetails
    {
        public int ProcessId { get; set; }

        public int ParentProcessId { get; set; }

        public int SessionId { get; set; }
    }
}
