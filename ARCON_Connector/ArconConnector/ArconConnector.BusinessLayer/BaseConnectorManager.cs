using ArconAPIUtility;
using ArconConnector.BusinessEntities;
using ArconErrorLogger.BusinessEntities;
using ArconErrorLogger.BusinessLayer;
using ArconImageRecorder;
using ArconRealTimeSessionMonitor;
using ArconRPA;
using EntityModel.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class BaseConnectorManager : ArconBaseManager
    {
        public BaseConnector objBaseConnector;
        ParameterManager objParameterManager;
        UserManager objUserManager;
        ServiceManager objServiceManager;
        VideoRecorderManager objVideoRecorderManager;

        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BaseConnectorManager() : base()
        {
        }

        public void SetArconContext()
        {
            try
            {
                ArconContext = ArconContext ?? new ArconContext();
                ArconContext.UserId = objBaseConnector.ParameterDetails.UserId;
                ArconContext.ServiceId = objBaseConnector.ServiceDetails.ServiceId;
                ArconContext.SessionId = objBaseConnector.ParameterDetails.SessionId;
                ArconContext.DBConnectionString = objBaseConnector.ParameterDetails.DBConnection.ConnectionString;
                ArconContext.RDPDBConnectionString = objBaseConnector.ParameterDetails.RDPDBConnection != null ? objBaseConnector.ParameterDetails.RDPDBConnection.ConnectionString : string.Empty;
                ArconContext.RADBConnectionString = objBaseConnector.ParameterDetails.RARDPDBConnection != null ? objBaseConnector.ParameterDetails.RARDPDBConnection.ConnectionString : string.Empty;
                ArconContext.APIConfig = ArconContext.APIConfig ?? CommonManager.GetAPIConfigurations(objBaseConnector.ParameterDetails.DBConnection.ConnectionString);
                ArconContext.WebDTAPIConfig = CommonManager.GetWebDTServiceConfigurations(objBaseConnector.ParameterDetails.ARCOSWebServiceDTURL, objBaseConnector.ParameterDetails.ARCOSWebServiceDTCredential);

            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public virtual BaseConnector InitialiseConnector(string[] cmdArgs)
        {
            try
            {
                objBaseConnector = new BaseConnector();
                objParameterManager = new ParameterManager();
                objBaseConnector.ParameterDetails = objParameterManager.ConvertParameterToObject(cmdArgs);
                objBaseConnector.ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;
                InitialiseLogger();

                ArconContext = new ArconContext() { APIConfig = CommonManager.GetAPIConfigurations(objBaseConnector.ParameterDetails.DBConnection.ConnectionString) };

                objUserManager = new UserManager() { ArconContext = ArconContext };
                objBaseConnector.UserDetails = objUserManager.GetUserDetailsBySessionId(objBaseConnector.ParameterDetails.SessionId);

                objServiceManager = new ServiceManager() { ArconContext = ArconContext };
                objBaseConnector.ServiceDetails = objServiceManager.GetServiceDetailsBySessionId(objBaseConnector.ParameterDetails.SessionId);

                objVideoRecorderManager = new VideoRecorderManager() { ArconContext = ArconContext };
                objBaseConnector.ImageRecorder = objVideoRecorderManager.GetVideoRecordingConfiguration(objBaseConnector.ServiceDetails.ServiceId, objBaseConnector.ServiceDetails.ServiceType);

                return objBaseConnector;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void InitialiseLogger()
        {
            string cs = objBaseConnector.ParameterDetails.DBConnection.ConnectionString;
            Log4NetHelper.ConfigureLogAppenders((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository(), cs);
        }

        public virtual void InitiateSSO()
        {
            try
            {
                // Call the JSON utility and capture the Process details
                if (!string.IsNullOrEmpty(objBaseConnector.ServiceDetails.JsonData))
                {
                    var newJsonData = ExeHelper.GetJsonDataByReplacingProperty(objBaseConnector, this.ArconContext);
                    RoboticProcessAutomation objArconRPA = new RoboticProcessAutomation();
                    objBaseConnector.ProcessDetails.Refresh();
                    objArconRPA.SendAutomationData(newJsonData, objBaseConnector.ProcessDetails, PostSSOCallback);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public virtual void InitialiseVideoRecording()
        {
            try
            {
                if (objBaseConnector.ProcessDetails != null && objBaseConnector.ProcessDetails.Id > 0)
                {
                    ImageRecorderManager objImageRecorderManager = new ImageRecorderManager() { APIConfig = ArconContext.APIConfig };
                    if (objBaseConnector.ImageRecorder.ProcessDetails == null)
                        objBaseConnector.ImageRecorder.ProcessDetails = new List<ProcessDetails>();
                    objBaseConnector.ImageRecorder.ProcessDetails.Add(
                        new ProcessDetails()
                        {
                            ProcessId = objBaseConnector.ProcessDetails.Id,
                            ParentProcessId = objBaseConnector.ProcessDetails.Id,
                            SessionId = objBaseConnector.UserDetails.SessionId
                        });
                    if (objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "ccws"))
                    {
                        var processNames = objBaseConnector.ServiceDetails.SettingParameter.FirstOrDefault(sett => sett.Key == "ccws").Value;
                        if (!string.IsNullOrEmpty(processNames))
                        {
                            var lstProcessName = processNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (objBaseConnector.ImageRecorder.ProcessName == null)
                                objBaseConnector.ImageRecorder.ProcessName = new List<ProcessDetails>();
                            lstProcessName.ForEach(proc =>
                            {
                                objBaseConnector.ImageRecorder.ProcessName.Add(new ProcessDetails()
                                {
                                    ParentProcessId = objBaseConnector.ProcessDetails.Id,
                                    SessionId = objBaseConnector.UserDetails.SessionId,
                                    AdditionalData = proc
                                });
                                objBaseConnector.ImageRecorder.ProcessDetails.AddRange(
                                                    Process.GetProcessesByName(proc)
                                                    .Select(pro => new ProcessDetails()
                                                    {
                                                        ProcessId = pro.Id,
                                                        SessionId = objBaseConnector.UserDetails.SessionId,
                                                        ParentProcessId = objBaseConnector.ProcessDetails.Id
                                                    }).ToList()
                                              );
                            });
                        }
                    }
                    objImageRecorderManager.InvokeImageCapture(objBaseConnector.ImageRecorder);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public virtual void InitialiseRTSM()
        {
            try
            {
                // Set the RTSM object
                objBaseConnector.SessionMonitor = new SessionMonitor()
                {
                    StreamAgent = new StreamAgent(),// { WindowHandlerToMonitor = hwnd, DesktopHandlerToMonitor = hwnd },
                    RealTimeServiceLog = new RealTimeServiceLog()
                    {
                        ServiceId = objBaseConnector.ServiceDetails.ServiceId,
                        ServiceSessionId = objBaseConnector.UserDetails.SessionId,
                        UserId = objBaseConnector.UserDetails.UserId,
                        UserSessionId = objBaseConnector.UserDetails.SessionId,
                        ResetRealTimeServiceLog = false
                    }
                };
                SessionMonitorManager objSessionMonitorManager = new SessionMonitorManager(objBaseConnector.SessionMonitor, ArconContext.APIConfig);
                objSessionMonitorManager.CreateRealTimeServiceLog();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public virtual void InitialiseVPN()
        {
            try
            {
                if (!objBaseConnector.ServiceDetails.Field3.Contains("DA"))
                {
                    objBaseConnector.VPNDetails = objBaseConnector.ParameterDetails.SSHVPNDetails;
                    VPNManager objVPNManager = new VPNManager() { ArconContext = ArconContext };
                    objVPNManager.StartVPNTunnel(objBaseConnector);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public virtual void SetConnectorAdditionalDetails() { }
        public virtual void LaunchConnector() { }
        public virtual void PreLaunchActivity()
        {
            
            if (objBaseConnector.ServiceDetails.UserName.Trim().Equals("PROMPT_USER",StringComparison.OrdinalIgnoreCase))
            {
                Forms.FrmUserCredentials objFrmUserCredentials_Service = new Forms.FrmUserCredentials(objBaseConnector);
                objFrmUserCredentials_Service.txtDomain.Text = objBaseConnector.ServiceDetails.DomainName;
                objFrmUserCredentials_Service.PocessLogin();
                objBaseConnector.ServiceDetails.UserName = objFrmUserCredentials_Service.txtUserName.Text.Trim();
                objBaseConnector.ServiceDetails.Password = objFrmUserCredentials_Service.txtPassword.Text.Trim();
                objBaseConnector.ServiceDetails.DomainName = objFrmUserCredentials_Service.txtDomain.Text.Trim();
            }
        }

        public virtual void PostLaunchActivity() { }

        public virtual void PostSSOCallback(Process objProcess) { }

        public virtual void ExecuteConnector()
        {
            try
            {
                SetArconContext();
                SetConnectorAdditionalDetails();
                InitialiseVPN();
                PreLaunchActivity();
                LaunchConnector();
                PostLaunchActivity();
                InitiateSSO();
                InitialiseVideoRecording();
                InitialiseRTSM();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public virtual void TerminateProcess()
        {
            try
            {
                if (objBaseConnector.ImageRecorder != null && objBaseConnector.ImageRecorder.ProcessDetails != null && objBaseConnector.ImageRecorder.ProcessDetails.Any())
                {
                    var lstProcess = objBaseConnector.ImageRecorder.ProcessDetails.Where(proc => proc.SessionId == objBaseConnector.UserDetails.SessionId);
                    if (lstProcess != null && lstProcess.Any())
                    {
                        lstProcess.ToList().ForEach(proc =>
                        {
                            try
                            {
                                Process objProcess = Process.GetProcessById(proc.ProcessId);
                                if (objProcess != null && !objProcess.HasExited)
                                    objProcess.Kill();
                            }
                            catch (ArgumentException ex)
                            {
                                //Added for code Process.GetProcessById
                                _Log.Error(ex);
                            }
                        });
                    }
                }
                if (objBaseConnector.ProcessDetails != null && !objBaseConnector.ProcessDetails.HasExited)
                    objBaseConnector.ProcessDetails.Kill();
                if (objBaseConnector.SessionMonitor != null && objBaseConnector.SessionMonitor.StreamAgent != null)
                {
                    SessionMonitorManager objSessionMonitorManager = new SessionMonitorManager(objBaseConnector.SessionMonitor, ArconContext.APIConfig);
                    objSessionMonitorManager.DeleteRealTimeServiceLog();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
