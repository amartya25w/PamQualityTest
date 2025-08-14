using log4net;
using System;
using System.Reflection;
using System.Text;

namespace ArconAPIUtility
{
    public class AuthHelper
    {
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _LogCode = "AUTH", _MethodName = string.Empty;

        public APIAuthToken GetArconToken(APIConfig objAPIConfig)
        {
            _LogCode = "AUTH:GAT";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");

            APIAuthToken objToken = null;
            string username = string.Empty, password = string.Empty;
            try
            {
                if (objAPIConfig.AuthToken != null && !string.IsNullOrEmpty(objAPIConfig.AuthToken.Token))
                {
                    if (!IsTokenExipred(objAPIConfig.AuthToken))
                        return objAPIConfig.AuthToken;
                    else
                    {
                        username = objAPIConfig.AuthToken.APIUserName;
                        password = objAPIConfig.AuthToken.APIPassword;
                    }
                }
                else
                {
                    username = Convert.ToBase64String(Encoding.UTF8.GetBytes(objAPIConfig.AuthToken.APIUserName));
                    password = Convert.ToBase64String(Encoding.UTF8.GetBytes(objAPIConfig.AuthToken.APIPassword));
                }
               
                var data = APIHelper.GetDataFromCacheByKey("AuthToken_" + username);
                if (data != null && !IsTokenExipred((APIAuthToken)data))
                    return (APIAuthToken)data;

                objAPIConfig.RequestContentType = "application/x-www-form-urlencoded";
                objAPIConfig.RequestMethod = "POST";
                objAPIConfig.APIUrl = "arconToken";
                string inputParams = "grant_type=password&username=" + username + "&password=" + password;
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = objAPIConfig };
                var request = objProxyHelper.CreateRequest(inputParams, false);
                var result = objProxyHelper.GetServiceResponse<dynamic>(request);
                objToken = new APIAuthToken()
                {
                    Token = result["access_token"],
                    RefreshToken = result["refresh_token"],
                    ExpiresIn = result["expires_in"],
                    TokenType = result["token_type"],
                    CreatedOn = DateTime.UtcNow,
                    APIPassword = password,
                    APIUserName = username
                };
                APIHelper.SetDataFromCacheByKey("AuthToken_" + objToken.APIUserName, objToken, objToken.CreatedOn.AddSeconds(objToken.ExpiresIn));
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            return objToken;
        }

        public static bool IsTokenExipred(APIAuthToken objToken)
        {
            if (objToken != null)
            {
                int result = DateTime.Compare(objToken.CreatedOn.AddSeconds(objToken.ExpiresIn), DateTime.UtcNow);
                if (result > 0)
                    return false;
            }
            return true;
        }
    }
}
