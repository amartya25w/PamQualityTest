using ArconConnector.BusinessEntities;
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
    public class EMCOracleManager : ExeConnectorManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void PostLaunchActivity()
        {
            SaveCRDFile();
        }

        public override void PreLaunchActivity()
        {
            base.PreLaunchActivity();
        }

        private void SaveCRDFile()
        {
            try
            {
                UserManager objUserManager = new UserManager() { ArconContext = ArconContext };
                string crdFilePath = objUserManager.GetUserPreference(objBaseConnector.UserDetails.UserId, 48);
                string tnsName = objBaseConnector.ServiceDetails.Instance;
                
                #region tnsnames.ora Required
                if (objBaseConnector.ServiceDetails.IPAddress.IndexOf("127.0.0") >= 0)
                {
                    tnsName = objBaseConnector.ServiceDetails.Instance + "_" + objBaseConnector.ServiceDetails.IPAddress + "_" + DateTime.Now.ToString("ddMMyyHHmmss");
                    string tnsNameOld = objBaseConnector.ServiceDetails.Instance + "_" + objBaseConnector.ServiceDetails.IPAddress + "_";
                    CreateOracleTNSEntry(objBaseConnector.ServiceDetails, objBaseConnector.UserDetails.UserId, tnsName, tnsNameOld);
                }
                #endregion

                if (File.Exists(crdFilePath))
                    File.Delete(crdFilePath);
                StreamWriter objStreamWriter = new StreamWriter(crdFilePath, true);
                objStreamWriter.WriteLine("#DBManager Credentials");
                objStreamWriter.WriteLine("#");
                objStreamWriter.WriteLine(tnsName + "=//NORMAL");
                objStreamWriter.Close();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public bool CreateOracleTNSEntry(ServiceDetails objServiceDetails, int userId, string tnsName, string oldTnsName)
        {
            try
            {
                UserManager objUserManager = new UserManager() { ArconContext = ArconContext };
                string oracleTnsPath = objUserManager.GetUserPreference(userId, 8);

                if (string.IsNullOrEmpty(oracleTnsPath))
                    throw new Exception("ValidationException: Unable To Find Value For Oracle QA (Local Path - tnsnames.ora).\n\nPlease, Goto My Preference And Set Oracle QA (Local Path - tnsnames.ora).");

                if (!oracleTnsPath.ToLower().EndsWith("tnsnames.ora"))
                    throw new Exception("ValidationException: Invalid tnsnames.ora File.\n\nPlease, Goto My Preference And Set Oracle QA (Local Path - tnsnames.ora).");

                StringBuilder objStringBuilder = new StringBuilder();
                if (File.Exists(oracleTnsPath))
                {
                    StreamReader objStreamReader = new StreamReader(oracleTnsPath);
                    string tempLine = string.Empty;
                    while (!objStreamReader.EndOfStream)
                    {
                        tempLine = objStreamReader.ReadLine();
                        if (tempLine.IndexOf(oldTnsName) < 0)
                            objStringBuilder.AppendLine(tempLine);
                    }
                    objStreamReader.Close();
                    File.Delete(oracleTnsPath);
                }

                StreamWriter objStreamWriter = new StreamWriter(oracleTnsPath, true);
                if (objStringBuilder != null)
                    objStreamWriter.WriteLine(objStringBuilder.ToString());

                objStreamWriter.WriteLine("\n");
                objStreamWriter.WriteLine("\n");
                objStreamWriter.Write(tnsName + " =");
                objStreamWriter.Write("  (DESCRIPTION =");
                objStreamWriter.Write("  (ADDRESS_LIST =");
                objStreamWriter.Write("     (ADDRESS = (PROTOCOL = TCP)(HOST = " + objBaseConnector.ServiceDetails.IPAddress + ")(PORT = " + objBaseConnector.ServiceDetails.Port + "))");
                objStreamWriter.Write("  )");
                objStreamWriter.Write("    (CONNECT_DATA =");
                objStreamWriter.Write("    (SERVICE_NAME = " + objServiceDetails.Instance + ")");
                objStreamWriter.Write("  )");
                objStreamWriter.Write(")");
                objStreamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
