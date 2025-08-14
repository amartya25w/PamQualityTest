using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Timers;
using Timer = System.Timers.Timer;
using log4net;
using System.Reflection;
using ArconConnector.BusinessEntities;

namespace ArconConnector.Forms
{
    public partial class FrmServiceConnRetry : Form
    {
        private int _RetryConnection = 5;
        private string _LogCode = string.Empty;
        private Timer _TmrRetryConnection = new Timer();
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ServiceConnRetry ServiceConnRetry;

        public FrmServiceConnRetry(ServiceConnRetry objServiceConnRetry)
        {
            InitializeComponent();
            this.ServiceConnRetry = objServiceConnRetry;
            _TmrRetryConnection.Elapsed += new ElapsedEventHandler(TmrRetryConnection_Tick);
        }

        public void ShowServiceRetryConnection()
        {
            try
            {
                ServiceConnRetry.IsFormVisible = true;
                _RetryConnection = 5;
                ServiceConnRetry.AllowFormClose = true;

                SetupTimer(true);

                if (ServiceConnRetry.ProcessForm != null)
                {
                    if (!ServiceConnRetry.ProcessForm.IsDisposed)
                    {
                        if (!ServiceConnRetry.ProcessForm.IsHandleCreated)
                            ServiceConnRetry.ProcessForm.CreateControl();

                        if (ServiceConnRetry.ProcessForm.InvokeRequired)
                        {
                            ServiceConnRetry.ProcessForm.Invoke((MethodInvoker)delegate
                            {
                                this.ShowDialog(ServiceConnRetry.ProcessForm);
                            });
                        }
                        else
                            this.ShowDialog(ServiceConnRetry.ProcessForm);
                    }
                    else
                        this.ShowDialog(ServiceConnRetry.ProcessForm);
                }
                else
                    this.ShowDialog();

                SetupTimer(false);
                ServiceConnRetry.IsFormVisible = false;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                ServiceConnRetry.IsFormVisible = false;
            }

        }

        private void BtnRetry_Click(object sender, EventArgs e)
        {
            try
            {
                SetupTimer(false);
                if (CheckServiceAvailability())
                {
                    MessageBox.Show("Connection Established. Now Process Will Contine....", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CloseForm();
                }
                else
                    throw new Exception("Unable To Connect ARCON PAM Server");
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);

                MessageBox.Show(ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetupTimer(true);
            }
            finally
            {
                //   SetupTimer(true);
            }
        }

        private void FrmServiceConnRetry_Load(object sender, EventArgs e)
        {
            try
            {
                txtErrorInfo.Text = ServiceConnRetry.Exception.Message.ToString();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);

                MessageBox.Show(ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TmrRetryConnection_Tick(object sender, EventArgs e)
        {
            try
            {
                _RetryConnection--;
                if (!btnRetry.IsDisposed)
                {
                    if (!btnRetry.IsHandleCreated)
                        btnRetry.CreateControl();

                    if (btnRetry.InvokeRequired)
                    {
                        btnRetry.Invoke((MethodInvoker)delegate
                        {
                            btnRetry.Text = "Retry Connection (in " + _RetryConnection.ToString() + " sec)";
                        });
                    }
                    else
                        btnRetry.Text = "Retry Connection (in " + _RetryConnection.ToString() + " sec)";
                }
                else
                    btnRetry.Text = "Retry Connection (in " + _RetryConnection.ToString() + " sec)";

                if (_RetryConnection <= 0)
                {
                    SetupTimer(false);
                    try
                    {
                        if (CheckServiceAvailability())
                            CloseForm();
                    }
                    catch { }
                    _RetryConnection = 5;
                    SetupTimer(true);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                MessageBox.Show(ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTerminateApplication_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("You May Loose Data After Terminating Application, And May Not Get Same Session Again.\n\n" +
                                    "Are You Sure, Do Want To Terminate Application?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CloseForm();
                    if (ServiceConnRetry.TerminateAction != null)
                        ServiceConnRetry.TerminateAction.Invoke();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                MessageBox.Show(ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseForm()
        {
            _LogCode = "FECL:0004";
            try
            {
                ServiceConnRetry.AllowFormClose = false;

                if (!this.IsDisposed)
                {
                    if (!this.IsHandleCreated)
                        this.CreateControl();

                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Close();
                        });
                    }
                    else
                        this.Close();
                }
                else
                    this.Close();
                return;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
        }

        private void FrmServiceConnRetry_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ServiceConnRetry.AllowFormClose;
        }

        private bool CheckServiceAvailability()
        {
            try
            {
                string url = ServiceConnRetry.ServiceBaseUrl + "api/Base/GetStatus";
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                myHttpWebResponse.Close();
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.Timeout)
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        private void SetupTimer(bool enable)
        {
            _TmrRetryConnection.Interval = 1000;
            _TmrRetryConnection.Enabled = enable;
            if (enable)
                _TmrRetryConnection.Start();
            else
                _TmrRetryConnection.Stop();
        }
    }
}
