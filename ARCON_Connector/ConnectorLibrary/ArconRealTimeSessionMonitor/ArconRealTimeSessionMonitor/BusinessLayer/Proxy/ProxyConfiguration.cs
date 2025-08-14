using ArconAPIUtility;
using EntityModel.Configuration;
using log4net;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArconRealTimeSessionMonitor
{
    public class ProxyConfiguration : ProxyBase
    {
        public override string ApiUrl { get; set; }    = "api/Configuration/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyConfiguration(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public APIConfiguration GetArcosConfiguration(APIConfiguration objApiConfiguartion)
        {
            try
            {
                var configData= SessionMonitorHelper.GetDataFromCacheByKey("ArcosConfig_" + objApiConfiguartion.ArcosConfigId + "_" + objApiConfiguartion.ArcosConfigSubId);
                if (configData != null)
                    return (APIConfiguration)configData;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("GetArcosConfiguration") };
                var request = objProxyHelper.CreateRequest(objApiConfiguartion);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<APIConfiguration>>(request);
                SessionMonitorHelper.SetDataFromCacheByKey("ArcosConfig_" + objApiConfiguartion.ArcosConfigId + "_" + objApiConfiguartion.ArcosConfigSubId, result.Result);
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
