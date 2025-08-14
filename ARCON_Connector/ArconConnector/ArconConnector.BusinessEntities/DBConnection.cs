using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessEntities
{
    public class DBConnection
    {
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public string DataBaseName { get; set; }
        public string DataSource { get; set; }
        public int ConnectTimeout { get; set; }
        public string ConnectionString
        {
            get
            {
                return string.Format(@"server=tcp: {0},{1};Initial Catalog= {2};{3};trusted_connection=false;Connect Timeout={4};", ServerIP, ServerPort, DataBaseName, DataSource, ConnectTimeout == 0 ? 20 : ConnectTimeout);
            }
        }
    }
}
