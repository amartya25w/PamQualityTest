using ArconAPIUtility;
using ArconConnector.BusinessEntities;
using EntityModel.Configuration;
using log4net;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.ServiceLayer
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
                var configData = ServiceHelper.GetDataFromCacheByKey("ArcosConfig_" + objApiConfiguartion.ArcosConfigId + "_" + objApiConfiguartion.ArcosConfigSubId);
                if (configData != null)
                    return (APIConfiguration)configData;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("GetArcosConfiguration") };
                var request = objProxyHelper.CreateRequest(objApiConfiguartion);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<APIConfiguration>>(request);
                ServiceHelper.SetDataFromCacheByKey("ArcosConfig_" + objApiConfiguartion.ArcosConfigId + "_" + objApiConfiguartion.ArcosConfigSubId, result.Result);
                return result.Result;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public List<APIConfiguration> GetArcosConfigurations(List<APIConfiguration> lstApiConfiguartion)
        {
            try
            {
                List<APIConfiguration> lstTempConfiguartion = new List<APIConfiguration>();
                List<APIConfiguration> lstResult = new List<APIConfiguration>();

                foreach (var config in lstApiConfiguartion)
                {
                    var configData = ServiceHelper.GetDataFromCacheByKey("ArcosConfig_" + config.ArcosConfigId + "_" + config.ArcosConfigSubId);
                    if (configData != null)
                        lstResult.Add((APIConfiguration)configData);
                    else
                        lstTempConfiguartion.Add(config);
                }

                if (lstTempConfiguartion != null && lstTempConfiguartion.Any())
                {
                    ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("GetArcosConfigurations") };
                    var request = objProxyHelper.CreateRequest(lstTempConfiguartion);
                    var result = objProxyHelper.GetServiceResponse<GenericResponse<List<APIConfiguration>>>(request);
                    if(result.Success)
                        result.Result.ForEach(res=>
                                ServiceHelper.SetDataFromCacheByKey("ArcosConfig_" + res.ArcosConfigId + "_" + res.ArcosConfigSubId, result.Result)
                        );
                    return result.Result;
                }
                else
                    return lstResult;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
