using ArconConnector.Forms;
using ArconConnector.BusinessEntities;
using ArconConnector.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Winforms.Components;
using static ArconConnector.BusinessEntities.Enum;
using ArconWinNativeAPI;
using Newtonsoft.Json;

namespace ArconConnector.ExeType
{
    public partial class FrmExeConnector : Form
    {
        BaseConnectorManager objConnectorManager;
        private ApplicationIdle _ApplicationIdle;
        BaseConnector objBaseConnector;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _LogCode = "FEC", _MethodName = string.Empty;
        private FrmServiceConnRetry _FrmServiceConnRetry = null;

        public FrmExeConnector()
        {
            InitializeComponent();
        }

        private void FrmExeConnector_Load(object sender, EventArgs e)
        {
            _LogCode = "FEC:0001";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                string[] cmdArgs = Environment.GetCommandLineArgs();
                _Log.Debug(_MethodName + " method cmdArgs : " + JsonConvert.SerializeObject(cmdArgs));
                objConnectorManager = ArconBaseManager.GetBO(ServiceType.NONE);
                objBaseConnector = objConnectorManager.InitialiseConnector(cmdArgs);

                Text = CommonManager.GetFormTitle(objConnectorManager.objBaseConnector.ServiceDetails.ServiceType);
                objConnectorManager = ArconBaseManager.GetBO(objConnectorManager.objBaseConnector.ServiceDetails.ServiceType);
                objConnectorManager.objBaseConnector = objBaseConnector;
                objConnectorManager.ExecuteConnector();
                objConnectorManager.objBaseConnector.ProcessDetails.Exited += new EventHandler(ProcessExited);
                _ApplicationIdle.IdleTime = new TimeSpan(0, 0, objBaseConnector.UserDetails.SessionTimeOutTime / 1000);
                _ApplicationIdle.Start();
                SetFormSize();
                SetProcessInFrame();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Connector Launch");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public void FrmExeConnector_FormClosing(object sender, FormClosingEventArgs e)
        {
            _LogCode = "FEC:0002";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if (objConnectorManager != null && objConnectorManager.objBaseConnector != null && objConnectorManager.objBaseConnector.ProcessDetails != null)
                {
                    objConnectorManager.objBaseConnector.ProcessDetails.Exited += null;
                    objConnectorManager.TerminateProcess();
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Closing Form");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            _LogCode = "FEC:0003";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                FrmExeConnector_FormClosing(sender, new FormClosingEventArgs(CloseReason.UserClosing, false));
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Process Exit");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void SetProcessInFrame()
        {
            _LogCode = "FEC:0004";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if (objBaseConnector.ServiceDetails.SettingParameter != null && objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "ShowProcessInFrame"))
                {
                    var showProcessInFrame = Convert.ToBoolean(objBaseConnector.ServiceDetails.SettingParameter.Where(sett => sett.Key == "ShowProcessInFrame").FirstOrDefault().Value);
                    _Log.Debug(_MethodName + " method showProcessInFrame : " + showProcessInFrame);
                    if (showProcessInFrame)
                    {
                        var objProcess = objBaseConnector.ProcessDetails;
                        objProcess.Refresh();
                        User32APIManager.SetParent(objProcess.MainWindowHandle, this.Handle);
                        User32APIManager.MoveWindow(objProcess.MainWindowHandle, 0, 0, this.Width, this.Height, true);
                        User32APIManager.SetWindowLong(objProcess.MainWindowHandle, Constants.GWL_STYLE, (IntPtr)Constants.WS_VISIBLE);
                        ToogleFormVisibility(true);
                    }
                    else
                        ToogleFormVisibility(false);
                }
                else
                    ToogleFormVisibility(false);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Connector Launch");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public void SetFormSize()
        {
            _LogCode = "FEC:0005";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                List<(int ConfigId, int SubAppId)> lstConfigs = new List<(int ConfigId, int SubAppId)>();
                lstConfigs.Add((99, 1));
                lstConfigs.Add((100, 1));

                CommonManager objCommonManager = new CommonManager() { ArconContext = objConnectorManager.ArconContext };
                var lstResult = objCommonManager.GetArcosConfigurations(lstConfigs);
                if (lstResult != null && lstResult.Any())
                {
                    var width = lstResult.Where(data => data.ConfigId == 99).FirstOrDefault().Value;
                    var height = lstResult.Where(data => data.ConfigId == 100).FirstOrDefault().Value;

                    Width = Width <= Convert.ToInt32(width) ? Convert.ToInt32(width) : Width;
                    Height = Height <= Convert.ToInt32(height) ? Convert.ToInt32(height) : Height;
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Form resizing");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public void ToogleFormVisibility(bool show)
        {
            _LogCode = "FEC:0006";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                this.Visible = show;
                this.ShowInTaskbar = show;
                if (show)
                    this.Show();
                else
                    this.Hide();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Connector Launch");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        #region Application Idle Events
        private void ApplicationIdle_Tick(object sender, Winforms.Components.ApplicationIdleData.TickEventArgs e)
        {
            if (ActiveForm == this)
                _ApplicationIdle.Restart();
        }

        private void ApplicationIdle_Idle(object sender, EventArgs e)
        {
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            LockSessionNow();
            _Log.Info(_MethodName + " Method Ended");
        }

        private void LockSessionNow()
        {
            _LogCode = "FEC:0007";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                _Log.Debug(_MethodName + " method IsSessionAlive : " + objBaseConnector.UserDetails.IsSessionAlive);
                if (_ApplicationIdle.IsRunning == true)
                    _ApplicationIdle.Stop();
                objBaseConnector.UserDetails.IsSessionAlive = false;
                FrmLogin objFrmLogin = new FrmLogin(Text, objConnectorManager.ArconContext, objBaseConnector);
                _Log.Debug(_MethodName + " method objFrmLogin IsUnlockSessionSuccess: " + objFrmLogin.IsUnlockSessionSuccess);
                if (objFrmLogin.IsUnlockSessionSuccess)
                {
                    objBaseConnector.UserDetails.IsSessionAlive = true;
                    _ApplicationIdle.IdleTime = new TimeSpan(0, 0, objBaseConnector.UserDetails.SessionTimeOutTime / 1000);
                    _ApplicationIdle.Start();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Locking Session");
            }
            _Log.Info(_MethodName + " Method Ended");
        }
        #endregion

        protected override void WndProc(ref Message message)
        {
            _LogCode = "FEC:0008";
            try
            {
                if (message.Msg == Constants.WM_COPYDATA)
                {
                    _MethodName = MethodBase.GetCurrentMethod().Name;
                    _Log.Info(_MethodName + " Method Started");
                    _Log.Debug(_MethodName + " method message : " + JsonConvert.SerializeObject(message));

                    ConnectorHelper.WriteMsg("WndProc message WM_COPYDATA");
                    User32.COPYDATASTRUCT cds = (User32.COPYDATASTRUCT)message.GetLParam(typeof(User32.COPYDATASTRUCT));
                    if (cds.lpData.IndexOf("ERRORAPI:100") >= 0)
                    {
                        var arrData = cds.lpData.Split('|');
                        if (arrData.Length > 0)
                        {
                            Exception objException = JsonConvert.DeserializeObject<Exception>(arrData[1]);
                            HandleConnectionError(objException);
                        }
                    }
                }
                //be sure to pass along all messages to the base also
                base.WndProc(ref message);
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Window message receiving");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void HandleConnectionError(Exception objException)
        {
            _LogCode = "FEC:0009";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if (_FrmServiceConnRetry == null || (_FrmServiceConnRetry.ServiceConnRetry != null && !_FrmServiceConnRetry.ServiceConnRetry.IsFormVisible))
                {
                    Process objProcess = null;
                    if (objConnectorManager != null && objBaseConnector != null)
                    {
                        objProcess = objBaseConnector.ProcessDetails ?? Process.GetCurrentProcess();
                        ServiceConnRetry objServiceConnRetry = new ServiceConnRetry()
                        {
                            Process = objProcess,
                            ProcessForm = this,
                            ServiceBaseUrl = objConnectorManager.ArconContext.APIConfig.APIBaseUrl,
                            TerminateAction = ConnectionErrorAction,
                            Exception = objException
                        };
                        _FrmServiceConnRetry = new FrmServiceConnRetry(objServiceConnRetry);
                    }
                    else
                    {
                        var objArconContext = new ArconContext() { APIConfig = CommonManager.GetAPIConfigurations() };
                        objProcess = Process.GetCurrentProcess();
                        ServiceConnRetry objServiceConnRetry = new ServiceConnRetry()
                        {
                            Process = objProcess,
                            ProcessForm = this,
                            ServiceBaseUrl = objArconContext.APIConfig.APIBaseUrl,
                            TerminateAction = ConnectionErrorAction,
                            Exception = objException
                        };
                        _FrmServiceConnRetry = new FrmServiceConnRetry(objServiceConnRetry);
                    }
                    _Log.Debug(_MethodName + " method ServiceConnRetry.IsFormVisible : " + _FrmServiceConnRetry.ServiceConnRetry.IsFormVisible);
                    _FrmServiceConnRetry.ShowServiceRetryConnection();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                if (ex.Message.Contains("ValidationException:"))
                    MessageBox.Show(ex.Message.Replace("ValidationException:", ""));
                else
                    MessageBox.Show(_LogCode + " : Error occured while Connection Retry");
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void ConnectionErrorAction()
        {
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            FrmExeConnector_FormClosing(null, new FormClosingEventArgs(CloseReason.UserClosing, false));
            _Log.Info(_MethodName + " Method Ended");
        }
    }
}
