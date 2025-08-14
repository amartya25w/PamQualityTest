using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace ArconAPIUtility
{
    public class ProxyHelper
    {
        public APIConfig APIConfig;
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _LogCode = "PRXH", _MethodName = string.Empty;

        private Uri GetServiceUrl()
        {
            return new Uri(APIConfig.APIBaseUrl + APIConfig.APIUrl);
        }

        public HttpWebRequest CreateRequest<T>(T obj, bool addHeader = true)
        {
            _LogCode = "PRXH:CR";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetServiceUrl());
                request.Method = APIConfig.RequestMethod;
                if (addHeader)
                    request.Headers.Add("Authorization", APIConfig.AuthToken.TokenType + " " + APIConfig.AuthToken.Token);
                request.ContentType = APIConfig.RequestContentType;
                using (var objStreamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    if (request.ContentType == "application/json")
                    {
                        string json = JsonConvert.SerializeObject(obj);
                        objStreamWriter.Write(json);
                    }
                    else
                        objStreamWriter.Write(obj);
                }
                return request;
            }
            catch (WebException ex)
            {
                _Log.Error(_LogCode, ex);
                ErrorHandlerHelper.HandleConnectionError(ex);
                throw ex;
            }
        }

        public T GetServiceResponse<T>(HttpWebRequest request)
        {
            _LogCode = "PRXH:GR";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string data = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(data);
                }
            }
            catch (WebException ex)
            {
                _Log.Error(_LogCode, ex);
                ErrorHandlerHelper.HandleConnectionError(ex);
                throw ex;
            }
        }
    }
}
