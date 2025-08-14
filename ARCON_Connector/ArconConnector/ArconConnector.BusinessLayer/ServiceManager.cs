using ArconConnector.BusinessEntities;
using ArconConnector.ServiceLayer;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class ServiceManager : ArconBaseManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _LogCode = "SM", _MethodName = string.Empty;
        public ServiceDetails GetServiceDetailsBySessionId(int sessionId)
        {
            _LogCode = "SM:GSDBSid";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                //Call the API to get below details refer GetServersEntityObject
                ProxyServiceDetails objProxyServiceDetails = new ProxyServiceDetails(ArconContext.APIConfig);
                ServiceDetails objServiceDetails = objProxyServiceDetails.GetServiceDetailsFromSession(sessionId);
                if (objServiceDetails != null)
                {
                    if (string.IsNullOrEmpty(objServiceDetails.Port) || string.IsNullOrWhiteSpace(objServiceDetails.Port))
                        objServiceDetails.Port = GetDefaultPort(objServiceDetails.ServiceType);
                    //if (objServiceDetails.AllowRoboticProcess)
                    if(true)
                    {
                        objServiceDetails.JsonData = GetJSONData(objServiceDetails.ServiceId);
                        objServiceDetails.FileData = GetFileData(objServiceDetails.ServiceId);
                        var lstSettingParameter = GetSettingParametersFromJson(objServiceDetails.JsonData);
                        if (lstSettingParameter != null && lstSettingParameter.Any())
                            objServiceDetails.SettingParameter.AddRange(lstSettingParameter);
                    }

                }
                return objServiceDetails;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode + ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public string GetFileData(int serviceId)
        {
            _LogCode = "SM:GFILED";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {

                //var data = File.ReadAllText(@"C:\Users\varun.rathi.ARCON007\Desktop\MSSQLJSON.json");
                //return data;

                return "";

                ProxyUIAutomationData objProxyUIAutomationData = new ProxyUIAutomationData(ArconContext.APIConfig);
                return objProxyUIAutomationData.GetFilebyServiceId(serviceId);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode + ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public string GetJSONData(int serviceId)
        {
            _LogCode = "SM:GJSOND";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {

                var data = File.ReadAllText(@"T:\Project\Connector Json\WinSCP_SFTP.json");
                return data;

                ProxyUIAutomationData objProxyUIAutomationData = new ProxyUIAutomationData(ArconContext.APIConfig);
                return objProxyUIAutomationData.GetImagebyServiceId(serviceId);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode + ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public List<SettingParameter> GetSettingParametersFromJson(string jsonData)
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                var lstJsonData = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(jsonData);
                if (lstJsonData != null && lstJsonData.Any())
                {
                    var lstSettings = lstJsonData.SelectMany(data => data).Where(data => data.Key == "settings");
                    if (lstSettings != null && lstSettings.Any())
                    {
                        var setting = lstSettings.FirstOrDefault().Value;
                        if (!string.IsNullOrEmpty(setting.ToString()))
                        {
                            var lstSettingParameter = JsonConvert.DeserializeObject<List<SettingParameter>>(setting.ToString());
                            return lstSettingParameter;
                        }
                    }
                }
            }
            return new List<SettingParameter>();
        }

        public string GetDefaultPort(ServiceType serviceType)
        {
            switch (serviceType)
            {
                case ServiceType.AppEMCNetworker: return "9000";
                case ServiceType.AppHuaweiLMTClientOMS2600: return "9101";
                case ServiceType.AppIBMTotalStorageProductivityCenter: return "9549";
                case ServiceType.AppHuaweiiManagerN2000: return "9800";
                case ServiceType.AppHuaweiiManagerI2000: return "9999";
                case ServiceType.AppIBMNotesLotus: return "1352";
                case ServiceType.AppdqMan: return "1489";
                case ServiceType.AppUNIXGUI: return "177";
                case ServiceType.AppCheckPoint: return "18190";
                case ServiceType.AppHuaweiOAMClient: return "24";
                case ServiceType.AppIBMRationalClearQuest: return "27000";
                case ServiceType.AppMongoDbStudio3T: return "27017";
                case ServiceType.AppCGClusterZTEMSC: return "3002";
                case ServiceType.AppHuaweiiManagerU2000: return "31039";
                case ServiceType.AppHuaweiiManagerM2000: return "31040";
                case ServiceType.AppSAPLogon: return "3252";
                case ServiceType.AppIBMRationalClearCase: return "371";
                case ServiceType.AppIBMProventiaSiteProtector: return "3998";
                case ServiceType.AppCeraMapPolyView: return "4001";
                case ServiceType.SSHSybaseISQL: return "4100";
                case ServiceType.AppVeeamONEMonitor: return "445";
                case ServiceType.ARCONDeskInsight: return "45046";
                case ServiceType.AppMcAfeeProxy: return "4711";
                case ServiceType.AppGOGlobalAlcatel: return "491";
                case ServiceType.AppNetnumenHLRAgent: return "5057";
                case ServiceType.AppPgAdminPostgreSQL: return "5432";
                case ServiceType.AppAginityWorkbench: return "5480";
                case ServiceType.AppXRDP: return "5910";
                case ServiceType.AppPowerShellISE: return "5985";
                case ServiceType.AppDameWare: return "6129";
                case ServiceType.AppNetHorizhonTelecomDevice: return "7001";
                case ServiceType.AppAMDOCSCRM: return "7120";
                case ServiceType.AppHuaweiHLRSMU: return "7777";
                case ServiceType.AppClarifyClient:
                case ServiceType.AppSingleView:
                case ServiceType.AppNSCCNCMSClient:
                case ServiceType.AppHuaweiAirbridge:
                case ServiceType.AppNetnumenHLRIQT:
                case ServiceType.AppNTSWinCashLogistics:
                case ServiceType.AppMcafeeStoneSoft:
                    return "0";
                case ServiceType.AppASEClient:
                case ServiceType.AppSybaseCentral:
                    return "0000";
                case ServiceType.AppMDSSwitchJAVAClient:
                case ServiceType.AppClearCaseRemoteClient:
                    return "1234";
                case ServiceType.MSSQLEMLocal:
                case ServiceType.MSSQLQA:
                case ServiceType.MSSQLEMRDP:
                case ServiceType.AppSqlDbx:
                case ServiceType.AppToadForSQLServer:
                    return "1433";
                case ServiceType.OracleQA:
                case ServiceType.SSHOracleSQLPlus:
                case ServiceType.AppToadOracle:
                case ServiceType.AppSQLDeveloperOracle:
                case ServiceType.AppPLSQLDeveloperOracle:
                case ServiceType.AppEnterpriseManagerConsoleOracle:
                case ServiceType.AppOracleDiscoverer:
                case ServiceType.AppOracleWorkflowBuilder:
                case ServiceType.AppSQLNavigator:
                case ServiceType.AppSQLTools:
                case ServiceType.AppOracleJDEdwardsEnterpriseOne:
                case ServiceType.AppSAPHanaStudio:
                case ServiceType.AppSQLAdvantage:
                case ServiceType.AppOracleDataIntegrator:
                    return "1521";
                case ServiceType.AppNetnumenHLR:
                case ServiceType.AppNetnumenHLREMS:
                    return "21099";
                case ServiceType.SSHUNIX:
                case ServiceType.SSHLINUX:
                case ServiceType.AIX:
                case ServiceType.DMZSSHLinux:
                case ServiceType.AppSmarTermSSH2:
                case ServiceType.AppKVMSwitchSSH2:
                case ServiceType.SSHRouter:
                case ServiceType.SSHSwitch:
                case ServiceType.SSHFirewall:
                case ServiceType.AppMochaSoft:
                case ServiceType.AppAttachmateReflection:
                case ServiceType.AppSecureShellClient:
                case ServiceType.AppMobaXterm:
                case ServiceType.AppMRWIN6530:
                case ServiceType.AppFileZilla:
                case ServiceType.AppTeraTerm:
                case ServiceType.AppAvamarDataCenter:
                case ServiceType.AppAvamarLocation:
                case ServiceType.AppWinSCP:
                case ServiceType.AppIBMWIntegrate:
                case ServiceType.AppIBMNetezzaAdmin:
                case ServiceType.AppAvayaSiteAdministration:
                case ServiceType.AppForeScoutCounterACTAdmin:
                case ServiceType.AppAbInitioGDE:
                case ServiceType.AppReflectionX:
                case ServiceType.AppSecureCRT:
                case ServiceType.AppIBMBigFixConsole:
                case ServiceType.AppFastrackBrowser:
                case ServiceType.AppWatchGuardSystemManager:
                case ServiceType.AppTSSKeyManager:
                case ServiceType.AppCitrixXenCenter:
                case ServiceType.AppPeopleTools:
                case ServiceType.AppRapidSQL:
                case ServiceType.AppSAPBO:
                    return "22";
                case ServiceType.SSHTelnet:
                case ServiceType.TelnetROUTER:
                case ServiceType.IBMAS400:
                case ServiceType.IBMAS390:
                case ServiceType.DMZSSHTelnet:
                case ServiceType.TelnetJuniperRouterSSG20:
                case ServiceType.TelnetJuniperRouterJ2300:
                case ServiceType.AppSmarTermTelnet:
                case ServiceType.TelnetSwitch:
                case ServiceType.IBMZOS:
                case ServiceType.AppSymantecProcommPlus:
                case ServiceType.AppAvayaCMSSupervisor:
                case ServiceType.AppTn3270:
                case ServiceType.AppExtraEnterprise2000:
                    return "23";
                case ServiceType.AppHuaweiServiceSMAP:
                case ServiceType.AppHuaweiSystemSMAP:
                    return "29999";
                case ServiceType.MySQLQA:
                case ServiceType.AppMySqlConnector:
                case ServiceType.AppMySQLAdministrator:
                case ServiceType.AppMySQLWorkBench:
                case ServiceType.AppSQLyog:
                case ServiceType.AppMySqlDreamCoder:
                    return "3306";
                case ServiceType.WindowsRDP:
                case ServiceType.DMZWindowsRDP:
                    return "3389";
                case ServiceType.AppBrocadeSwitchBrowser:
                case ServiceType.AppCiscoDeviceManager:
                case ServiceType.AppVMwarevSphereClient:
                case ServiceType.AppCiscoASDM:
                case ServiceType.AppTippingPointSMSClient:
                case ServiceType.AppCitrixXenApp:
                case ServiceType.AppOracleEBS:
                case ServiceType.AppArcSightConsole:
                case ServiceType.AppDelliDRAC:
                case ServiceType.AppVmwareHorizon:
                case ServiceType.AppAventailVPNConnection:
                case ServiceType.AppSANSwitch:
                case ServiceType.AppVmwareVspherePowerCli:
                case ServiceType.AppCitrixSmartLauncher:
                    return "443";
                case ServiceType.SybaseQA:
                case ServiceType.AppEmbarcaderoDBArtisan:
                    return "5000";
                case ServiceType.DB2QA:
                case ServiceType.AppToadDB2:
                case ServiceType.AppIBMDB2Client:
                case ServiceType.AppIBMDataStudio:
                case ServiceType.AppDBVisualizer:
                    return "50000";
                case ServiceType.AppHuaweiLMTClient:
                case ServiceType.AppHuaweiSAUPMS:
                    return "6000";
                case ServiceType.AppWebBrowser:
                case ServiceType.AppKVMSwitchBrowser:
                case ServiceType.AppHMCBrowser:
                case ServiceType.AppIBMConsoleBrowser:
                case ServiceType.AppPortalInternalAdministrationBrowser:
                case ServiceType.AppComverseCustomerCareClient:
                case ServiceType.AppCiscoNetworkAssistant:
                    return "80";
                case ServiceType.AppSMCForcePoint:
                case ServiceType.AppBMCclient:
                case ServiceType.AppMicrosoftDynamicsNAVClient:
                    return "8080";
                case ServiceType.AppJuniperNetworksNSM:
                case ServiceType.AppSymantecEndpointProtectionManager:
                    return "8443";
                case ServiceType.AppIBMSASEnterpriseGuide:
                case ServiceType.AppIBMSASDataIntegrationStudio:
                case ServiceType.AppIBMSASManagementConsole:
                case ServiceType.AppIBMSASWorkflowStudio:
                    return "8561";
                case ServiceType.AppNetnumanZTEGSMBSC:
                case ServiceType.AppNetnumanZTEGSMMSC:
                case ServiceType.AppNetnumanZTEGSMMINOS:
                case ServiceType.AppNetnumanZTEGSMUGMSC:
                case ServiceType.AppNetnumanZTEGSMMGW:
                case ServiceType.AppNetnumanZTEGSMEMS:
                case ServiceType.AppNetnumanZTEGSMNGN:
                case ServiceType.AppNetnumanZTECDMABSC:
                case ServiceType.AppNetnumanZTECDMAMSC:
                case ServiceType.AppNetnumanZTECDMAMGW:
                case ServiceType.AppNetnumanZTECDMAGMGW:
                case ServiceType.AppNetnumanZTECDMAGMSC:
                case ServiceType.AppNetnumanZTECDMAPOMC:
                case ServiceType.AppNetnumanZTECDMAEMS:
                    return "8998";
                case ServiceType.AppDellStorageManagerClient:
                    return "3033";
                default: return "22";
            }
        }
    }
}
