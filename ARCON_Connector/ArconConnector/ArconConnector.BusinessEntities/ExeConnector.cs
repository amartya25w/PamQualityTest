using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessEntities
{
    public class ExeConnector : BaseConnector
    {
        public string ExePath { get; set; }
        public string ExeParameter { get; set; }
        public string WindowTitle { get; set; }
        public string ExeWorkingDir { get; set; }
        public bool RunAsDifferentUser { get; set; }
        public string ExeName
        {
            get
            {
                if (!string.IsNullOrEmpty(ExePath))
                    return Path.GetFileName(ExePath);
                return string.Empty;
            }
        }
    }
}
