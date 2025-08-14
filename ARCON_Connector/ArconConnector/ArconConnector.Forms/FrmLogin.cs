using ArconConnector.BusinessEntities;
using ArconConnector.ServiceLayer;
using ArconRealTimeSessionMonitor;
using EntityModel.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ProxyConfiguration = ArconConnector.ServiceLayer.ProxyConfiguration;

namespace ArconConnector.Forms
{
    public partial class FrmLogin : Form
    {
        private bool _loginSuccess;
        private bool _cancelCloseForm = true;
        private string _titleAppend = "";
        private BaseConnector _BaseConnector;
        private ArconContext _ArconContext;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FrmLogin()
        {
            InitializeComponent();
        }

        public FrmLogin(string title, ArconContext objArconContext, BaseConnector objBaseConnector)
        {
            InitializeComponent();
            _BaseConnector = objBaseConnector;
            _ArconContext = objArconContext;
            _titleAppend = " [" + title + "]";
        }

        public bool IsLoginSuccess
        {
            get
            {
                this.lblFormHeading.Text = "Log On to ARCON PAM";
                this.Text = lblFormHeading.Text;
                this.ShowDialog();
                return _loginSuccess;
            }
        }

        public bool IsUnlockSessionSuccess
        {
            get
            {
                try
                {
                    ShowInTaskbar = true;
                    lblFormHeading.Text = "Unlock ARCON PAM" + _titleAppend;
                    Text = lblFormHeading.Text;
                    txtUserName.ReadOnly = true;
                    txtUserName.Text = _BaseConnector.UserDetails.UserName;
                    cboDomainName.Enabled = false;
                    cboDomainName.SelectedText = _BaseConnector.UserDetails.Domain;

                    btnUnlockSession.Visible = true;
                    btnCancel.Visible = false;
                    AcceptButton = btnUnlockSession;
                    txtPassword.Focus();

                    if (_BaseConnector.ProcessDetails != null && !_BaseConnector.ProcessDetails.HasExited)
                    {
                        Win32Window objWin32Window = new Win32Window(_BaseConnector.ProcessDetails.MainWindowHandle);
                        this.ShowDialog(objWin32Window);
                        return _loginSuccess;
                    }
                    this.ShowDialog();
                    return _loginSuccess;
                }
                catch (Exception ex)
                {
                    _Log.Error(ex);
                    throw ex;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _cancelCloseForm = false;
            this.Dispose();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                AssemblyDetails objAssemblyDetails = new AssemblyDetails();
                lblCopyright.Text = objAssemblyDetails.AssemblyCopyright;
                lblTrademark.Text = "®" + objAssemblyDetails.AssemblyCompany;

                FillComboBoxDomains(cboDomainName);
                if (_BaseConnector != null && _BaseConnector.UserDetails.Domain.ToString() != "")
                    cboDomainName.Text = _BaseConnector.UserDetails.Domain;

                btnTerminateSession.Visible = IsTerminateLockedSession();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                MessageBox.Show(ex.Message.ToString(), "Error.....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUnlockSession_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    errLogin.SetError(txtPassword, "Please, Enter Password!");
                    return;
                }

                string randomNo = FormHelper.GenerateRandomDigits(5);
                string pwd = string.Format("{0}_{1}", randomNo, txtPassword.Text);
                ProxyARCOSWebDT objProxyARCOSWebDT = new ProxyARCOSWebDT(_ArconContext.WebDTAPIConfig);
                string response = objProxyARCOSWebDT.IsADAuthenticatedWithARCOSLogin(-1, cboDomainName.SelectedValue.ToString(), txtUserName.Text, pwd, string.Empty);

                string[] arrResult = response.Split(new char[] { '_' });
                string result = Convert.ToString(arrResult[0]);
                string randomNumber = Convert.ToString(arrResult[1]);
                string user = Convert.ToString(arrResult[2]);

                int indexArr = 0;
                foreach (string str in arrResult)
                {
                    if (indexArr > 2)
                        user = user + "_" + str;
                    indexArr++;
                }

                if (result == "1" && user.ToLower() == txtUserName.Text.ToLower() && randomNumber == randomNo)
                {
                    if (result == "1" && user.ToLower() == txtUserName.Text.ToLower())
                    {
                        _loginSuccess = true;
                        _cancelCloseForm = false;
                        this.Close();
                    }
                    else
                    {
                        _loginSuccess = false;
                        MessageBox.Show("Invalid User ID OR Password.", this.Text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    _loginSuccess = false;
                    MessageBox.Show("Invalid User ID OR Password.", this.Text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                MessageBox.Show(ex.Message.ToString(), "Error.....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmLogin_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = _cancelCloseForm;
        }

        public void FillComboBoxDomains(ComboBox objComboBox)
        {
            try
            {
                ProxyServiceDetails objProxyServiceDetails = new ProxyServiceDetails(_ArconContext.APIConfig);
                List<KeyValuePair<int, string>> lstDomainData = objProxyServiceDetails.GetDomains();

                objComboBox.DataSource = lstDomainData;
                objComboBox.DisplayMember = "Value";
                objComboBox.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                MessageBox.Show(ex.Message.ToString(), "Error.....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefreshDomains_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLogin_Load(sender, e);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                MessageBox.Show(ex.Message.ToString(), "Error.....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTerminateSession_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are You Sure You Want To Close This Session?", "ARCON PAM Exit Confirmation",
                                              MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                              == DialogResult.Yes ? true : false;
                if (result)
                {
                    if (_BaseConnector.ProcessDetails != null && !_BaseConnector.ProcessDetails.HasExited)
                        _BaseConnector.ProcessDetails.Kill();
                    TerminateApplication(_BaseConnector, true, false);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                MessageBox.Show(ex.Message.ToString(), "Error.....", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool IsTerminateLockedSession()
        {
            try
            {
                ProxyConfiguration objProxyConfiguration = new ProxyConfiguration(_ArconContext.APIConfig);
                var result = objProxyConfiguration.GetArcosConfiguration(new APIConfiguration() { ArcosConfigId = 103, ArcosConfigSubId = 1 });
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

        public void TerminateApplication(BaseConnector objBaseConnector, bool killCurrentProcess, bool resetSessionTime)
        {
            try
            {
                if (objBaseConnector.ParameterDetails.ExeName.ToLower().IndexOf("arcosloginacmo") > 0 ||
                    objBaseConnector.ParameterDetails.ExeName.ToLower().IndexOf("arcosscriptmanager") > 0 ||
                    objBaseConnector.ParameterDetails.ExeName.ToLower().IndexOf("arcosbiometricfingerprintauthenticator") > 0 ||
                    objBaseConnector.ParameterDetails.ExeName.ToLower().IndexOf("arcosuseraccesslogviewer") > 0)
                    return;

                if (objBaseConnector.UserDetails.SessionId > 0)
                {
                    if (!resetSessionTime)
                    {
                        EndUserServiceSession(objBaseConnector.UserDetails.SessionId, "Logout");
                    }
                    if (objBaseConnector.SessionMonitor.RealTimeServiceLog != null)
                    {
                        ProxyRTSM objProxyRTSM = new ProxyRTSM(_ArconContext.APIConfig);
                        objProxyRTSM.DeleteRTSMLog(objBaseConnector.SessionMonitor.RealTimeServiceLog);
                    }
                }
                // Need to Code for VPN Disconnect
                FrmTunnel.StopSSHNetSecureGateway(objBaseConnector.VPNDetails.SSHClient, true);//Code changes corresponding to DisconnectVPNClientServer();
                objBaseConnector.VPNDetails.IsConnected = false;
                //DisconnectVPNClientDB();

                if (killCurrentProcess)
                    Process.GetCurrentProcess().Kill();
                else
                    Environment.Exit(0);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void EndUserServiceSession(int sessionId, string LogoutFlag)
        {
            // Call the API to update User log details refer EndSessionDB
            try
            {
                ProxyUserServiceSession objProxyUserServiceSession = new ProxyUserServiceSession(_ArconContext.APIConfig);
                objProxyUserServiceSession.EndUserServiceSession(sessionId, LogoutFlag);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}