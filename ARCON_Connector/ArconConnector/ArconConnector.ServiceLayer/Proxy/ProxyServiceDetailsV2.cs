using ArconAPIUtility;
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
    public class ProxyServiceDetailsV2 : ProxyBase
    {
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override string ApiUrl { get; set; } = "api/ServiceDetailsV2/";

        public ProxyServiceDetailsV2(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public string GetServiceParamConfig(int serviceId, int paramConfigId)
        {
            try
            {
                string inputParams = string.Format("serviceId={0}&paramConfigId={1}", serviceId, paramConfigId);
                string apiUrl = "GetServiceParamConfig?" + inputParams;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig(apiUrl) };
                var request = objProxyHelper.CreateRequest(string.Empty);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<string>>(request);
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
