using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchExecutable
{
    public class Launch
    {

        public Process LaunchApplication(string jsonData)
        {
            try
            {


                Process p = null;
                var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);
                int i = 0;
                #region Send EXE parameter
                if (i == 0)
                {
                    p = launchExe(data[i]["exe_path"].ToString(), data[i]["exe_name"].ToString(), data[i]["processName"].ToString(), data[i]["exeargument"].ToString());

                }
                return p;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Launch EXE 
        private Process launchExe(string exePath, string exeName, string processName, string argument)
        {
            Process processExecuteEXE = new Process();
            try
            {
                exePath = exePath + "\\" + exeName;
                processExecuteEXE.StartInfo.FileName = exePath;
                if (argument != "")
                    processExecuteEXE.StartInfo.Arguments = argument;
                processExecuteEXE.Start();
            }
            catch (Exception ex) { throw ex; }

            return processExecuteEXE;

        }
        #endregion
    }
}
