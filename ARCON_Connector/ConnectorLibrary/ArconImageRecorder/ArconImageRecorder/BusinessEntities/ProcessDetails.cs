using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconImageRecorder
{
    public class ProcessDetails
    {
        public int ProcessId { get; set; }

        public int ParentProcessId { get; set; }

        public int SessionId { get; set; }

        public byte[] LastCapturedImage { get; set; }

        public string AdditionalData { get; set; } // Use this for Setting Process name / title
    }
}
