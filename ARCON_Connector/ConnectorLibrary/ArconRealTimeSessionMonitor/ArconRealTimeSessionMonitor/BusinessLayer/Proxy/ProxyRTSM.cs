using ArconAPIUtility;
using EntityModel.RTSM;
using log4net;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace ArconRealTimeSessionMonitor
{
    public class ProxyRTSM : ProxyBase
    {
        public override string ApiUrl { get; set; }    = "api/RTSM/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyRTSM(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public bool InsertRTSMLog(RealTimeServiceLog objRealTimeServiceLog)
        {
            try
            {
                RTSMLog objRTSMLog = new RTSMLog()
                {
                    ServiceSessionId = objRealTimeServiceLog.ServiceSessionId,
                    UserSessionId = objRealTimeServiceLog.UserSessionId,
                    ServiceId = objRealTimeServiceLog.ServiceId,
                    UserId = objRealTimeServiceLog.UserId,
                    Port = objRealTimeServiceLog.Port1,
                    Port2 = objRealTimeServiceLog.Port2,
                    IPAddressLan = objRealTimeServiceLog.IPAddressLan,
                    IPAddressNAT = objRealTimeServiceLog.IPAddressNAT,
                    IsRTSM = objRealTimeServiceLog.IsRealTimeSessionMonitoringActive.ToString(),
                };
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("InsertRTSMLog") };
                var data = JsonConvert.SerializeObject(objRTSMLog);
                var request = objProxyHelper.CreateRequest(objRTSMLog);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<bool>>(request);
                return result.Result;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public bool UpdateRTSMLog(RealTimeServiceLog objRealTimeServiceLog)
        {
            try
            {
                RTSMLog objRTSMLog = new RTSMLog()
                {
                    ServiceSessionId = objRealTimeServiceLog.ServiceSessionId,
                    UserSessionId = objRealTimeServiceLog.UserSessionId,
                    ServiceId = objRealTimeServiceLog.ServiceId,
                    UserId = objRealTimeServiceLog.UserId,
                    Port = objRealTimeServiceLog.Port1,
                    Port2 = objRealTimeServiceLog.Port2,
                    IPAddressLan = objRealTimeServiceLog.IPAddressLan,
                    IPAddressNAT = objRealTimeServiceLog.IPAddressNAT,
                    IsRTSM = objRealTimeServiceLog.IsRealTimeSessionMonitoringActive.ToString(),
                };
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("UpdateRTSMLog") };
                var request = objProxyHelper.CreateRequest(objRTSMLog);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<bool>>(request);
                return result.Result;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public bool DeleteRTSMLog(RealTimeServiceLog objRealTimeServiceLog)
        {
            try
            {
                RTSMLog objRTSMLog = new RTSMLog()
                {
                    ServiceSessionId = objRealTimeServiceLog.ServiceSessionId,
                    Port = objRealTimeServiceLog.Port1
                };
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("DeleteRTSMLog") };
                var request = objProxyHelper.CreateRequest(objRTSMLog);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<bool>>(request);
                return result.Result;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
