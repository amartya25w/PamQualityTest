using ArconAPIUtility;
using log4net;
using System;
using System.Reflection;

namespace ArconAPIUtility
{
    public class ProxyBase
    {
        public APIConfig APIConfig;
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public virtual string ApiUrl { get; set; }
        private static string _LogCode = "PRXB", _MethodName = string.Empty;

        public ProxyBase(APIConfig objAPIConfig)
        {
            try
            {
                AuthHelper objAuthHelper = new AuthHelper();
                objAPIConfig.AuthToken = objAuthHelper.GetArconToken(objAPIConfig);
                this.APIConfig = objAPIConfig;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
        }

        public APIConfig GetRequestConfig(string apiName, string contentType = "application/json", string method = "POST")
        {
            _LogCode = "PRXB:GRC";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                APIConfig objAPIConfig = new APIConfig()
                {
                    RequestContentType = contentType,
                    RequestMethod = method,
                    APIUrl = ApiUrl + apiName,
                    AuthToken = APIConfig.AuthToken,
                    APIBaseUrl = APIConfig.APIBaseUrl
                };
                return objAPIConfig;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
        }
    }
}
