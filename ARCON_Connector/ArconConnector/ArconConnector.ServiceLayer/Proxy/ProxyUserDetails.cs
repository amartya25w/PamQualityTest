using ArconAPIUtility;
using ArconConnector.BusinessEntities;
using EntityModel.UserDetails;
using log4net;
using Models;
using System;
using System.Reflection;

namespace ArconConnector.ServiceLayer
{
    public class ProxyUserDetails : ProxyBase
    {
        public override string ApiUrl { get; set; }    = "api/UserDetails/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyUserDetails(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public UserDetails GetUserDetailsBySession(int sessionId)
        {
            try
            {
                UserDetails objUserDetails = null;
                string inputParams = "sessionId=" + sessionId;
                string apiUrl = "GetUserDetailsBySession?" + inputParams;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig(apiUrl, "application/x-www-form-urlencoded") };
                var request = objProxyHelper.CreateRequest("");
                var result = objProxyHelper.GetServiceResponse<GenericResponse<UserDetailsBySessionId>>(request);
                if (result.Success && result.Result != null)
                {
                    objUserDetails = new UserDetails()
                    {
                        UserId = Convert.ToInt32(result.Result.UserID),
                        UserName = result.Result.UserName,
                        DisplayName = result.Result.UserDisplayName,
                        Domain = result.Result.DomainName,
                        SessionId = sessionId,
                        SessionTimeOutTime = Convert.ToInt32(result.Result.SessionTimeOutTime),
                        IsSessionAlive = result.Result.IsSessionAlive
                    };
                }
                return objUserDetails;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public string GetUserPreference(int userId, int preferenceId)
        {
            try
            {
                string inputParams = string.Format("userId={0}&preferenceId={1}", userId, preferenceId);
                string apiUrl = "GetUserPreference?" + inputParams;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig(apiUrl, "application/x-www-form-urlencoded") };
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
