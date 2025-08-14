using ArconConnector.BusinessEntities;
using ArconConnector.ServiceLayer;
using ArconConnector.Forms;
using EntityModel.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class VPNManager : ArconBaseManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private FrmTunnel _FrmTunnel;

        public void StartVPNTunnel(BaseConnector objBaseConnector)
        {
            try
            {
                var allowTunnelCreation = AllowVPNTunnel(objBaseConnector);
                List<string> lstPortNumber = new List<string>();
                if (AllowVPNTunnel(objBaseConnector))
                {
                    //SetSplashScreen("Connecting To ARCON PAM Secured Server");

                    if (objBaseConnector.ServiceDetails.Port == "0" || objBaseConnector.ServiceDetails.Port.Length <= 0)
                        return;
                    else if (objBaseConnector.ServiceDetails.ServiceType == ServiceType.AppVMwarevSphereClient && objBaseConnector.ServiceDetails.IsUserLockToConsole)
                        return;

                    #region Set Local & Target IP + Port
                    objBaseConnector.VPNDetails.AllowVPNTunnel = allowTunnelCreation;
                    objBaseConnector.VPNDetails.LocalIPAddress = ConnectorHelper.GetRandomLocalhostIP();

                    if (objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "sp"))
                        objBaseConnector.VPNDetails.LocalPort = Convert.ToInt32(objBaseConnector.ServiceDetails.Port);
                    else if (objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "USP")) //Added New - On 04 July 2015
                    {
                        objBaseConnector.VPNDetails.LocalPort = Convert.ToInt32(objBaseConnector.ServiceDetails.Port);
                        objBaseConnector.ServiceDetails.SettingParameter.Remove(objBaseConnector.ServiceDetails.SettingParameter.Find(sett => sett.Key == "USP"));
                    }
                    else if (objBaseConnector.ServiceDetails.ServiceType == ServiceType.AppNetnumanZTEGSMBSC ||
                            objBaseConnector.ServiceDetails.ServiceType == ServiceType.AppNetnumanZTEGSMMSC ||
                            objBaseConnector.ServiceDetails.ServiceType == ServiceType.AppNetnumanZTEGSMMINOS ||
                            objBaseConnector.ServiceDetails.ServiceType == ServiceType.AppNetnumanZTEGSMUGMSC ||
                            objBaseConnector.ServiceDetails.ServiceType == ServiceType.AppSAPLogon)
                        objBaseConnector.VPNDetails.LocalPort = Convert.ToInt32(objBaseConnector.ServiceDetails.Port);
                    else if (objBaseConnector.ServiceDetails.Port == "5985") //Windows Power Shell Port
                        objBaseConnector.VPNDetails.LocalPort = Convert.ToInt32(objBaseConnector.ServiceDetails.Port);
                    else
                        objBaseConnector.VPNDetails.LocalPort = Convert.ToInt32(ConnectorHelper.GetRandomPort());

                    objBaseConnector.VPNDetails.TargetIPAddress = objBaseConnector.ServiceDetails.IPAddress;
                    objBaseConnector.VPNDetails.TargetPort = Convert.ToInt32(objBaseConnector.ServiceDetails.Port);
                    #endregion

                    objBaseConnector.VPNDetails = StartVPNClient(objBaseConnector);    
                    if (!objBaseConnector.VPNDetails.IsConnected)
                    {
                        throw new Exception("ValidationException: Unable To Create Channel With VPN Server");
                        //MessageBox.Show("Unable To Create Channel With VPN Server.", "Error.....", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return false;
                    }
                    else
                    {
                        objBaseConnector.ServiceDetails.IPAddress = objBaseConnector.VPNDetails.LocalIPAddress;
                        objBaseConnector.ServiceDetails.Port = objBaseConnector.VPNDetails.LocalPort.ToString();

                        if (objBaseConnector.ServiceDetails.ServiceType == ServiceType.MSSQLQA)
                        {
                            if (!objBaseConnector.ServiceDetails.DynamicPort)
                                objBaseConnector.ServiceDetails.Instance = objBaseConnector.VPNDetails.LocalIPAddress;
                            else
                                objBaseConnector.ServiceDetails.IPAddress = objBaseConnector.VPNDetails.TargetIPAddress;
                        }
                        #region Additional Port Forwarding
                        if (objBaseConnector.VPNDetails.IsConnected)
                        {
                            lstPortNumber = GetPortListByServiceType(objBaseConnector.ServiceDetails, objBaseConnector.ApplicationPath);
                            if (lstPortNumber.Any())
                            {
                                _FrmTunnel.Show();
                                var count = 0;
                                lstPortNumber.ForEach(data =>
                                {
                                    decimal val = ((count + 1) * 100) / lstPortNumber.Count;
                                    StartVPNPort(Convert.ToUInt32(data), Math.Ceiling(val).ToString() + " %");
                                    count++;
                                });
                                _FrmTunnel.Hide();
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public bool AllowVPNTunnel(BaseConnector objBaseConnector)
        {
            try
            {
                if (objBaseConnector.ServiceDetails.Port == "0")
                    return false;
                else if (objBaseConnector.ServiceDetails.SettingParameter != null && objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "da"))
                    return false;
                else if (objBaseConnector.ServiceDetails.SettingParameter != null && objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "NSG"))
                {
                    objBaseConnector.ServiceDetails.SettingParameter.Remove(objBaseConnector.ServiceDetails.SettingParameter.Find(sett => sett.Key == "NSG"));
                    return false;
                }
                else if (objBaseConnector.ServiceDetails.ServiceType == ServiceType.MSSQLEMRDP)
                    return false;
                else if (objBaseConnector.ServiceDetails.ServiceType == ServiceType.ARCONDeskInsight && !objBaseConnector.ParameterDetails.TakeRDPConsole)
                    return false;
                else if (objBaseConnector.VPNDetails!=null && objBaseConnector.VPNDetails.AllowVPNTunnel)
                {
                    if (objBaseConnector.ServiceDetails.ServiceType == ServiceType.ARCONDeskInsight && objBaseConnector.ParameterDetails.TakeRDPConsole)
                        return IsVPNDeskInsightRDPEnabled;
                    else if (objBaseConnector.ServiceDetails.ServiceType == ServiceType.WindowsRDP && objBaseConnector.ParameterDetails.ExeName.ToLower().IndexOf(("ARCOSAppExeTerminal").ToLower()) > 0)
                        return false;
                    else
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public bool IsVPNDeskInsightRDPEnabled
        {
            get
            {
                try
                {
                    ProxyConfiguration objProxyConfiguration = new ProxyConfiguration(ArconContext.APIConfig);
                    var result = objProxyConfiguration.GetArcosConfiguration(new APIConfiguration() { ArcosConfigId = 84, ArcosConfigSubId = 1 });
                    if (!string.IsNullOrEmpty(result.ArcosConfigDefaultValue))
                        return result.ArcosConfigDefaultValue == "1" ? true : false;
                    return false;
                }
                catch (Exception ex)
                {
                    _Log.Error(ex);
                    throw ex;
                }
            }
        }

        public VPNDetails StartVPNClient(BaseConnector objBaseConnector)
        {
            try
            {
                _FrmTunnel = new FrmTunnel(objBaseConnector, ArconContext, false);
                Application.DoEvents();
                objBaseConnector.VPNDetails.IsConnected = _FrmTunnel.StartSSHNetSecureGateway();
                return objBaseConnector.VPNDetails;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public bool StartVPNPort(uint forwardingLocalPort, string windowTitle)
        {
            try
            {
                //SetSplashScreen("Connecting To ARCON PAM Secured Server (" + pWindowTitle + ")");
                if (_FrmTunnel != null)
                    return _FrmTunnel.StartVPNClientOnlyPort(forwardingLocalPort, windowTitle);
                return false;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public List<string> GetPortListByServiceType(ServiceDetails objServiceDetails, string applicationPath)
        {
            ServiceType serviceType = objServiceDetails.ServiceType;
            string fileName = applicationPath + "\\";
            switch (serviceType)
            {
                case ServiceType.IBMAS400:
                case ServiceType.IBMZOS:
                    return objServiceDetails.Port == "992" ? GetPortDataFromFile(fileName + "ARCOSAS400Terminal_Ports.ini", true) : GetPortDataFromFile(fileName + "ARCOSAS400Terminal_Ports.ini");
                case ServiceType.AppVMwarevSphereClient:
                    return GetPortDataFromFile(fileName + "ARCOSAppVMwarevSphereClient_Ports.ini");
                case ServiceType.AppCheckPoint:
                    return GetPortDataFromFile(fileName + "ARCOSAppCheckPoint_Ports.ini");
                case ServiceType.AppHuaweiServiceSMAP:
                case ServiceType.AppHuaweiSystemSMAP:
                    return new List<string>() { "21", "23", "9003" };
                case ServiceType.AppDelliDRAC:
                    return new List<string>() { "5900", "5901" };
            }
            return new List<string>();
        }

        public List<string> GetPortDataFromFile(string fileName, bool isSSL = false)
        {
            List<string> lstPortData = new List<string>();
            try
            {
                StreamReader fileToRead = new StreamReader(fileName);
                if (!fileToRead.EndOfStream)
                {
                    lstPortData = fileToRead.ReadLine().Split(',').ToList();
                    if (isSSL && !fileToRead.EndOfStream)
                        lstPortData = fileToRead.ReadLine().Split(',').ToList();
                }
                fileToRead.Close();
                return lstPortData;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
