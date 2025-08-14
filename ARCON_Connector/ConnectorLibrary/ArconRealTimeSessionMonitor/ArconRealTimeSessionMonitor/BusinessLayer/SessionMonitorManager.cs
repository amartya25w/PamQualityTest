using ArconAPIUtility;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ArconRealTimeSessionMonitor
{
    public class SessionMonitorManager : IDisposable
    {
        private Timer _TmrRealTimeService;
        private StreamAgentManager _StreamAgentManager;
        private readonly SessionMonitor _SessionMonitor;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private APIConfig _APIConfig;

        public SessionMonitorManager(SessionMonitor objSessionMonitor, APIConfig objAPIConfig)
        {
            if (_SessionMonitor == null)
                _SessionMonitor = objSessionMonitor;
            _APIConfig = objAPIConfig;
        }

        public void InitialiseTimer(bool enableTimer = true)
        {
            const int interval = 220000;
            try
            {
                if (_TmrRealTimeService == null)
                {
                    _TmrRealTimeService = new Timer();
                    _TmrRealTimeService.Elapsed += TmrRealTimeService_Tick;
                    _TmrRealTimeService.Interval = _SessionMonitor.RealTimeServiceLog.TimeInterval == 0 ? interval : _SessionMonitor.RealTimeServiceLog.TimeInterval; //Runs every 3 minutes and 40 seconds
                }
                _TmrRealTimeService.Enabled = enableTimer;

                if (enableTimer)
                    _TmrRealTimeService.Start();
                else _TmrRealTimeService.Stop();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw;
            }
        }

        public void CreateRealTimeServiceLog()
        {
            try
            {
                ProxyRTSM objProxyRTSM = new ProxyRTSM(_APIConfig);
                if (_SessionMonitor.RealTimeServiceLog.ResetRealTimeServiceLog && _StreamAgentManager != null)
                {
                    try
                    {
                        InitialiseTimer(false);
                        _StreamAgentManager.StopMonitoring();
                        _StreamAgentManager = null;
                        if (_SessionMonitor.RealTimeServiceLog != null)
                            objProxyRTSM.DeleteRTSMLog(_SessionMonitor.RealTimeServiceLog);
                    }
                    catch (Exception ex)
                    {
                        _Log.Error(ex);
                        throw;
                    }
                }

                if (_StreamAgentManager != null)
                    objProxyRTSM.UpdateRTSMLog(_SessionMonitor.RealTimeServiceLog);
                else
                {
                    const int interval = 2, minRandomValue = 12000, maxRandomValue = 13000;
                    bool isRealTimeSessionMonitoringActive = IsRealTimeSessionMonitoringActive();
                    //if (IsTimeBasedSession)
                    //    StartTimeBasedAccessTimer();

                    Random objRandom = new Random();
                    _SessionMonitor.RealTimeServiceLog.UserSessionId = 0;
                    _SessionMonitor.RealTimeServiceLog.IPAddressLan = string.Empty;
                    _SessionMonitor.RealTimeServiceLog.IPAddressNAT = string.Empty;
                    _SessionMonitor.RealTimeServiceLog.Port1 = objRandom.Next(minRandomValue, maxRandomValue);
                    _SessionMonitor.RealTimeServiceLog.Port2 = 0;
                    _SessionMonitor.RealTimeServiceLog.IsRealTimeSessionMonitoringActive = isRealTimeSessionMonitoringActive ? 1 : 0;
                    objProxyRTSM.InsertRTSMLog(_SessionMonitor.RealTimeServiceLog);

                    if (isRealTimeSessionMonitoringActive)
                    {
                        if (_SessionMonitor.StreamAgent != null)
                        {
                            _SessionMonitor.StreamAgent.TimeInterval = _SessionMonitor.StreamAgent.TimeInterval == 0 ? interval : _SessionMonitor.StreamAgent.TimeInterval;
                            _SessionMonitor.StreamAgent.LocalPort = _SessionMonitor.RealTimeServiceLog.Port1;
                        }
                        _StreamAgentManager = new StreamAgentManager(_SessionMonitor.StreamAgent, _SessionMonitor.RealTimeServiceLog);
                        Thread ThreadObject = new Thread(() =>
                        {
                            _StreamAgentManager.StartMonitoring();
                        });
                        ThreadObject.IsBackground = true;
                        ThreadObject.Start();
                        InitialiseTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw;
            }
        }

        private void TmrRealTimeService_Tick(object sender, EventArgs e)
        {
            try
            {
                CreateRealTimeServiceLog();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw;
            }
        }

        public bool IsRealTimeSessionMonitoringActive()
        {
            try
            {
                ProxyConfiguration objProxyConfiguration = new ProxyConfiguration(_APIConfig);
                var result = objProxyConfiguration.GetArcosConfiguration(
                                new EntityModel.Configuration.APIConfiguration
                                {
                                    ArcosConfigId = 41,
                                    ArcosConfigSubId = 5
                                });
                if (result != null)
                    return result.ArcosConfigDefaultValue == "1";
                return false;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw;
            }
        }

        public void DeleteRealTimeServiceLog()
        {
            try
            {
                ProxyRTSM objProxyRTSM = new ProxyRTSM(_APIConfig);
                if (_SessionMonitor.RealTimeServiceLog != null)
                    objProxyRTSM.DeleteRTSMLog(_SessionMonitor.RealTimeServiceLog);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw;
            }
        }

        void IDisposable.Dispose()
        {
            if (_TmrRealTimeService != null)
                _TmrRealTimeService.Dispose();
        }
    }
}
