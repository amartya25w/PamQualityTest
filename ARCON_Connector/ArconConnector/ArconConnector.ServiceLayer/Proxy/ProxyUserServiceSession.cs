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
    public class ProxyUserServiceSession : ProxyBase
    {
        public new virtual string ApiUrl { get; set; }  = "api/UserServiceSession/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyUserServiceSession(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public void EndUserServiceSession(int sessionId,string LogoutFlag)
        {
            try
            {
                //string inputParams = "sessionId=" + sessionId;
                string inputParams = "ServerConnectionLogID=" + sessionId.ToString() + "&LogoutTimeFlag="+ LogoutFlag;
                string apiUrl = "api/RASyn/UserServiceSessionUpdate?" + inputParams;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig(apiUrl, "application/x-www-form-urlencoded") };
                var request = objProxyHelper.CreateRequest(string.Empty);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<string>>(request);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
