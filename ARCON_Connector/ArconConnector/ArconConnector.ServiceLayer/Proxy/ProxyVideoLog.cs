using ArconAPIUtility;
using EntityModel.VideoLogSettings;
using log4net;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.ServiceLayer
{
    public class ProxyVideoLog : ProxyBase
    {
        public override string ApiUrl { get; set; }    = "api/VideoLog/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyVideoLog(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public void GetVideoLogSettings(int serviceId, ServiceType servicetype)
        {
            try
            {
                string inputParams = string.Format("serviceTypeId={0}&serviceId={1}", Convert.ToInt32(servicetype), serviceId);
                string apiUrl = "GetVideoLogSettings?" + inputParams;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig(apiUrl, "application/x-www-form-urlencoded") };
                var request = objProxyHelper.CreateRequest(string.Empty);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<VideoLogSetting>>(request);
                //return MapServiceDetailsObject(result.Result);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
