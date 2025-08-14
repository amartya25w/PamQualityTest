using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconRealTimeSessionMonitor
{
    public class NameValuePair
    {
        public string Key;
        public object Value;
        public NameValuePair()
        { }

        public NameValuePair(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
