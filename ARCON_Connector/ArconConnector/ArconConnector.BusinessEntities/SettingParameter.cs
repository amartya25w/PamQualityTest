using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessEntities
{
    public class SettingParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public SettingParameter ChildData { get; set; }
    }
}
