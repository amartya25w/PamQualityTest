using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessLayer
{ 
    public class SQLyogManager : ExeConnectorManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void PreLaunchActivity()
        {
            DeleteStoredConnectionsSqlyog();
        }

        private void DeleteStoredConnectionsSqlyog()
        {
            try
            {
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(
                               Environment.SpecialFolder.ApplicationData), @"SQLyog\"));
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*.db", SearchOption.AllDirectories);
                foreach (FileInfo f in filesInDir)
                {
                    if (f.Name == "connrestore.db")
                    {
                        f.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw new Exception("Error in deleting connection history SQLyog");
            }
        }

    }
}
