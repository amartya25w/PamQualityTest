using System;
using System.Windows.Forms;
using Renci.SshNet;
using ArconConnector.BusinessEntities;
using System.Threading;
using log4net;
using System.Reflection;
using EntityModel.Configuration;
using ArconConnector.ServiceLayer;
using Renci.SshNet.Common;
using ArconWinNativeAPI;
using System.Diagnostics;
using System.Management;
using System.Linq;

namespace ArconConnector.Forms
{
    public partial class FrmTunnel : Form
    {
        private string _OriginalWindowTitle = string.Empty;
        private bool _ShowWindow = true;
        private string _ErrorMsg = string.Empty;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ArconContext _ArconContext;
        private BaseConnector _BaseConnector;

        public FrmTunnel(BaseConnector objBaseConnector, ArconContext objArconContext, bool showWindow)
        {
            InitializeComponent();
            _BaseConnector = objBaseConnector;
            _OriginalWindowTitle = this.Text;
            _ShowWindow = showWindow;
            _ArconContext = objArconContext;
            this.Hide();
        }

        public bool StartSSHNetSecureGateway(bool formVisible = true)
        {
            try
            {
                this.Visible = formVisible;
                Application.DoEvents();
                return InitiateSShNetSecureGateway();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void StopSSHNetSecureGateway(bool allowDisconnect)
        {
            try
            {
                var port = _BaseConnector.VPNDetails.SSHClient.ForwardedPorts.FirstOrDefault();
                if (port == null || !port.IsStarted)
                    return;
                if (allowDisconnect)
                    _BaseConnector.VPNDetails.SSHClient.Disconnect();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static void StopSSHNetSecureGateway(SshClient objSSHClient,bool allowDisconnect)
        {
            try
            {
                var port = objSSHClient.ForwardedPorts.FirstOrDefault();
                if (port == null || !port.IsStarted)
                    return;
                if (allowDisconnect)
                    objSSHClient.Disconnect();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private bool InitiateSShNetSecureGateway()
        {
            try
            {
                _ErrorMsg = string.Empty;
                this.Text = this.Text + " - " + _BaseConnector.VPNDetails.TargetIPAddress;
                _BaseConnector.VPNDetails.SSHClient = new SshClient(_BaseConnector.VPNDetails.GatewayIPAddress, _BaseConnector.VPNDetails.GatewayPort, _BaseConnector.VPNDetails.GatewayUserName, _BaseConnector.VPNDetails.GatewayPassword);
                _BaseConnector.VPNDetails.SSHClient.Connect();
                if (_BaseConnector.VPNDetails.SSHClient.IsConnected)
                {
                    var port = new ForwardedPortLocal(_BaseConnector.VPNDetails.LocalIPAddress, Convert.ToUInt32(_BaseConnector.VPNDetails.LocalPort),
                                                  _BaseConnector.VPNDetails.TargetIPAddress, Convert.ToUInt32(_BaseConnector.VPNDetails.TargetPort));
                    _BaseConnector.VPNDetails.SSHClient.AddForwardedPort(port);
                    port.Start();
                    port.RequestReceived += Port_RequestReceived;
                    this.Hide();
                }
                return _BaseConnector.VPNDetails.SSHClient.IsConnected;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                this.Hide();
                _ErrorMsg = ex.Message;
                throw ex;
            }
        }

        public bool StartVPNClientOnlyPort(uint forwardingLocalPort, string windowTitle)
        {
            try
            {
                if (!_ShowWindow)
                    this.Hide();
                Application.DoEvents();
                this.Text = _OriginalWindowTitle + " - " + _BaseConnector.VPNDetails.TargetIPAddress;
                Application.DoEvents();
                if (windowTitle.Length > 0)
                    this.Text = this.Text + " (" + windowTitle + ")";

                Thread.Sleep(200);
                Application.DoEvents();
                if (IsEnabledComProSecureGatewayVersion) // added to handle multiple ports when compro is on - shabbir
                {
                    var port = new ForwardedPortLocal(_BaseConnector.VPNDetails.LocalIPAddress, forwardingLocalPort,
                                                   _BaseConnector.VPNDetails.TargetIPAddress, Convert.ToUInt32(_BaseConnector.VPNDetails.TargetPort));
                    _BaseConnector.VPNDetails.SSHClient.AddForwardedPort(port);
                    port.Start();
                }
                return true;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                MessageBox.Show(ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                return false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private bool IsEnabledComProSecureGatewayVersion
        {
            get
            {
                ProxyConfiguration objProxyConfiguration = new ProxyConfiguration(_ArconContext.APIConfig);
                var result = objProxyConfiguration.GetArcosConfiguration(new APIConfiguration() { ArcosConfigId = 174, ArcosConfigSubId = 1 });
                if (!string.IsNullOrEmpty(result.ArcosConfigDefaultValue))
                    return result.ArcosConfigDefaultValue == "1" ? true : false;
                return false;
            }
        }

        private void Port_RequestReceived(object sender, PortForwardEventArgs e)
        {
            var currentProcessId = 0;
            var allowSecureVPN = GetServiceParamConfig(_BaseConnector.ServiceDetails.ServiceId, 5) == "1" ? true : false;
            if ((allowSecureVPN && _BaseConnector.ProcessDetails == null) || _BaseConnector.ProcessDetails != null)
                currentProcessId = Process.GetCurrentProcess().Id;
            if (_BaseConnector.ProcessDetails != null || currentProcessId > 0)
            {
                IntPtr hwnd = User32APIManager.GetForegroundWindow();
                User32APIManager.GetWindowThreadProcessId(hwnd, out uint activepid);
                Process objActiveProcess = Process.GetProcessById((int)activepid);
                var currentProcessParentId = GetParentProcess(currentProcessId);

                if ((currentProcessId != (int)activepid && currentProcessId != currentProcessParentId) ||
                    (_BaseConnector.ProcessDetails != null &&
                     _BaseConnector.ProcessDetails.Id != (int)activepid && _BaseConnector.ProcessDetails.Id != currentProcessParentId &&
                      (objActiveProcess.ProcessName.ToLower() == _BaseConnector.ProcessDetails.ProcessName.ToLower() ||
                       objActiveProcess.ProcessName.ToLower() != "sqldeveloperw.exe" &&
                       objActiveProcess.ProcessName.ToLower() != "explorer"))
                     )
                {
                    IntPtr lHwnd = User32APIManager.FindWindow("Shell_TrayWnd", null);
                    User32APIManager.SendMessage(lHwnd, Constants.WM_COMMAND, (IntPtr)Constants.MIN_ALL, IntPtr.Zero);
                    Thread.Sleep(2000);
                    if (objActiveProcess != null)
                    {
                        try
                        {
                            objActiveProcess.Kill();
                        }
                        catch { }
                    }
                    MessageBox.Show("Access Denied! \n \n The session is in use.", "Security Alert!",
                    MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);  // MB_TOPMOST
                }
            }
        }

        private int GetParentProcess(int processId)
        {
            int parentPid = 0;
            try
            {
                string query = "SELECT * FROM Win32_BaseConnector.ProcessDetails WHERE ProcessId = '" + processId.ToString() + "'";
                ManagementObjectSearcher objManagementObjectSearcher = new ManagementObjectSearcher(new ObjectQuery(query));
                var lstResult = objManagementObjectSearcher.Get();
                if (lstResult != null && lstResult.Count > 0)
                {
                    foreach (ManagementObject result in lstResult)
                        parentPid = Convert.ToInt32(result["ParentProcessId"]);
                }
            }
            catch (Exception ex) { _Log.Error(ex); }
            return parentPid;
        }

        public string GetServiceParamConfig(int serviceId, int paramConfigId)
        {
            try
            {
                ProxyServiceDetailsV2 objProxyServiceDetailsV2 = new ProxyServiceDetailsV2(_ArconContext.APIConfig);
                return objProxyServiceDetailsV2.GetServiceParamConfig(serviceId, paramConfigId);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
