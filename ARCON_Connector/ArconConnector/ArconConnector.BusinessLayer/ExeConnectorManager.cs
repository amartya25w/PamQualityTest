using ArconConnector.BusinessEntities;
using ArconWinNativeAPI;
using ArcosFloatingElement;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class ExeConnectorManager : BaseConnectorManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ExeConnector objExeConnector = new ExeConnector();
        private string _LogCode = "ECM", _MethodName = string.Empty;

        public override void SetConnectorAdditionalDetails()
        {
            _LogCode = "ECM:0001";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                UserManager objUserManager = new UserManager() { ArconContext = ArconContext };
                objExeConnector.ExePath = objUserManager.GetUserPreference(objBaseConnector.UserDetails.UserId, GetPreferenceIdForExePath(objBaseConnector.ServiceDetails.ServiceType));
                objExeConnector.WindowTitle = ExeHelper.GetExeWindowTitle(objBaseConnector);
                _Log.Debug(_MethodName + " method ExePath : " + objExeConnector.ExePath + "WindowTitle : " + objExeConnector.WindowTitle);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public override void LaunchConnector()
        {
            _LogCode = "ECM:0002";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                //Set the Exe Parameter in JSON
                objExeConnector.ExeParameter = ExeHelper.GetExeParameter(objBaseConnector, ArconContext);
                _Log.Debug(_MethodName + " method ExeParameter : " + objExeConnector.ExeParameter + " ExePath : " + objExeConnector.ExePath);

                // Call Launcher utility to start the process
                // Set the process which is Launched for Connector
                Process objProcess = new Process
                {
                    StartInfo = new ProcessStartInfo(objExeConnector.ExePath, objExeConnector.ExeParameter),
                    EnableRaisingEvents = true
                };
                if (objExeConnector.RunAsDifferentUser)
                {
                    objProcess.StartInfo.UseShellExecute = false;
                    objProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(objExeConnector.ExePath);
                    objProcess.StartInfo.Verb = "runas";
                    objProcess.StartInfo.LoadUserProfile = true;
                    objProcess.StartInfo.Domain = objBaseConnector.ServiceDetails.DomainName;
                    objProcess.StartInfo.UserName = objBaseConnector.ServiceDetails.UserName;
                    objProcess.StartInfo.Password = GetSecureString();
                    objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                }
                objProcess.Exited += new EventHandler(ProcessExited);
                objProcess.Start();
                objBaseConnector.ProcessDetails = objProcess;
                SetWindowTitle();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            _LogCode = "ECM:0003";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");

            TerminateProcess();

            _Log.Info(_MethodName + " Method Ended");
        }

        private SecureString GetSecureString()
        {
            _LogCode = "ECM:0004";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                SecureString secureString = new SecureString();
                foreach (char c in objBaseConnector.ServiceDetails.Password)
                {
                    secureString.AppendChar(c);
                }
                return secureString;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
        }

        public override void PostSSOCallback(Process objProcess)
        {
            _LogCode = "ECM:0005";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if (objBaseConnector.ServiceDetails.SettingParameter != null &&
                    objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "SetCustomTitle" && Convert.ToBoolean(sett.Value)))
                    SetCustomTitle();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void SetWindowTitle()
        {
            _LogCode = "ECM:0006";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                var objProcess = objBaseConnector.ProcessDetails;
                do
                {
                    Thread.Sleep(2000);
                    objProcess.Refresh();
                }
                while (objProcess.MainWindowHandle == IntPtr.Zero);
                User32APIManager.SetWindowText(objProcess.MainWindowHandle, objExeConnector.WindowTitle);
                if (objBaseConnector.ServiceDetails.SettingParameter != null &&
                    objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "SetCustomTitle" && Convert.ToBoolean(sett.Value)))
                    SetCustomTitle();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public void SetCustomTitle()
        {
            _LogCode = "ECM:0007";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                Thread.Sleep(10000);
                var objProcess = objBaseConnector.ProcessDetails;
                _Log.Debug(_MethodName + " method MainWindowHandle : " + objProcess.MainWindowHandle + " Title : " + objExeConnector.WindowTitle);
                TitleElement objTitleElement = new TitleElement() { WindowHwnd = objProcess.MainWindowHandle, Title = objExeConnector.WindowTitle };
                TitleElementManager objTitleElementManager = new TitleElementManager();
                objTitleElementManager.AddFloatingTitle(objTitleElement);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private int GetPreferenceIdForExePath(ServiceType serviceType)
        {
            _LogCode = "ECM:0008";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            switch (serviceType)
            {
                case ServiceType.MSSQLEMLocal: return 2;
                case ServiceType.AppSQLDeveloperOracle: return 5;
                case ServiceType.AppEnterpriseManagerConsoleOracle: return 45;
                case ServiceType.AppASEClient: return 140;
                case ServiceType.AppSQLAdvantage: return 144;
                case ServiceType.AppVMwarevSphereClient: return 18;
                case ServiceType.AppFileZilla: return 93;
                case ServiceType.AppSqlDbx: return 91;
                case ServiceType.AppSQLyog: return 102;
                case ServiceType.AppToadForSQLServer: return 145;
                case ServiceType.AppMySQLWorkBench: return 90;
                case ServiceType.AppDellStorageManagerClient: return 124;
                case ServiceType.AppCitrixXenCenter: return 157;
                case ServiceType.AppPLSQLDeveloperOracle: return 30;
                case ServiceType.AppWinSCP: return 98;
                case ServiceType.AppSybaseCentral: return 143;
                case ServiceType.AppSecureCRT: return 111;
                default: return 0;
            }
        }
    }
}
