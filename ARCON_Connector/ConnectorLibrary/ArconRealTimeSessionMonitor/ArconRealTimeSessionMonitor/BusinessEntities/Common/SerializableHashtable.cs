using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArconRealTimeSessionMonitor
{
    [Serializable()]
    public class SerializableHashtable : List<NameValuePair>
    {
        public void WriteToFile(string filename)
        {
            //open the stream to write
            StreamWriter sw = new StreamWriter(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableHashtable));
            serializer.Serialize(sw, this);
            sw.Close();
        }

        public void ReadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return;
            }
            //open the stream to read
            StreamReader sr = new StreamReader(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableHashtable));
            SerializableHashtable ht = serializer.Deserialize(sr) as SerializableHashtable;
            base.Clear();
            foreach (NameValuePair nvp in ht)
            {
                base.Add(nvp);
            }
        }

        public void Add(string key, object value)
        {
            base.Add(new NameValuePair(key, value));
        }

        public bool ContainsKey(string key)
        {
            try
            {
                NameValuePair pair = this.Find(
                    delegate (NameValuePair nvp)
                    {
                        return nvp.Key == key;
                    });
                if (pair == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public object this[string key]
        {
            get
            {
                object val = null;
                NameValuePair pair = this.Find(
                    delegate (NameValuePair nvp)
                    {
                        return nvp.Key == key;
                    });
                if (pair != null)
                    val = pair.Value;
                return val;
            }

            set
            {
                NameValuePair pair = this.Find(
                    delegate (NameValuePair nvp)
                    {
                        return nvp.Key == key;
                    });
                if (pair != null)
                    pair.Value = value;
            }
        }
    }
}
