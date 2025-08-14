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
    public class SQLDeveloperOracleManager : ExeConnectorManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _LogCode = "SDOM", _MethodName = string.Empty;

        public override void PreLaunchActivity()
        {
            DeleteStoredConnections();
            SetExePath();
        }

        private void SetExePath()
        {
            var exeName = objExeConnector.ExeName;
            if (File.Exists(objExeConnector.ExePath.ToLower().Replace(exeName, string.Empty) + @"sqldeveloper\bin\headless.exe"))
                objExeConnector.ExePath = objExeConnector.ExePath.ToLower().Replace(exeName, string.Empty) + @"sqldeveloper\bin\headless.exe";
            else if (SystemInfo.OSBitType == OSBitType.X64)
                objExeConnector.ExePath = objExeConnector.ExePath.ToLower().Replace(exeName, string.Empty) + @"sqldeveloper\bin\sqldeveloper64W.exe";
            else
                objExeConnector.ExePath = objExeConnector.ExePath.ToLower().Replace(exeName, string.Empty) + @"sqldeveloper\bin\sqldeveloperW.exe";
        }

        private void DeleteStoredConnections()
        {
            try
            {
                DirectoryInfo objDirectoryInfo = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(
                                Environment.SpecialFolder.ApplicationData), @"SQL Developer\"));
                _Log.Debug(_LogCode + " .XML file path : " + objDirectoryInfo);

                var lstFiles = objDirectoryInfo.GetFiles("*.xml", SearchOption.AllDirectories).Where(file => file.Name == "connections.xml").ToList();
                if (lstFiles != null && lstFiles.Any())
                    lstFiles.ForEach(file => { file.Delete(); });

                lstFiles = objDirectoryInfo.GetFiles("*.json", SearchOption.AllDirectories).Where(file => file.Name == "connections.json").ToList();
                if (lstFiles != null && lstFiles.Any())
                    lstFiles.ForEach(file => { file.Delete(); });
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void CheckForError()
        {
            string strProcessOutput = string.Empty;
            while (true)
            {
                if (objBaseConnector.ProcessDetails.StandardOutput.EndOfStream == true)
                {
                    break;
                }
                strProcessOutput = strProcessOutput + objBaseConnector.ProcessDetails.StandardOutput.ReadLine();
            }
            objBaseConnector.ProcessDetails.WaitForExit(10000);
            if (strProcessOutput.ToLower().IndexOf("mkconn completed successfully") < 0)
            {
                //MessageBox.Show("Error! In Connecting via ORACLE SQL Developer. \n\n" +
                //    "Database is not be accessible due to following reason(s) \n" +
                //    "1) Network connection problem \n" +
                //    "2) Database server may be not working properly \n" +
                //    "3) Your computer might not have permission to connect Database Server \n" +
                //    "4) Invalid username/password \n" +
                //    string.Empty + strProcessOutput.ToUpper(), "Error.....", MessageBoxButtons.OK, MessageBoxIcon.Error);

                TerminateProcess();
            }
        }
    }
}
