using ArconAPIUtility;
using log4net;
using Models;
using Models.UIAutomation;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArconConnector.ServiceLayer
{
    public class ProxyUIAutomationData : ProxyBase
    {
        public override string ApiUrl { get; set; }    = "api/UIAutomationData/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyUIAutomationData(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public string GetImagebyServiceId(int serviceId)
        {
            try
            {
                UIAutomationData objUIAutomationData = new UIAutomationData() { ServiceId = serviceId };
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("GetImagebyServiceId") };
                var request = objProxyHelper.CreateRequest(objUIAutomationData);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<List<UIAutomationData>>>(request);
                if (result.Result != null && result.Result.Count > 0)
                    return result.Result[0].ImageDetails;
                return string.Empty;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public string GetFilebyServiceId(int serviceId)
        {
            try
            {
                UIAutomationData objUIAutomationData = new UIAutomationData() { ServiceId = serviceId };
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("GetFileImagebyServiceId") };
                var request = objProxyHelper.CreateRequest(objUIAutomationData);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<List<UIAutomationData>>>(request);
                if (result.Result != null && result.Result.Count > 0)
                    return result.Result[0].ImageDetails;
                return string.Empty;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
