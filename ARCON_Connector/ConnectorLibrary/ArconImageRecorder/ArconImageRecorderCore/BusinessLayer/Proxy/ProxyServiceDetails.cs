using ArconAPIUtility;
using log4net;
using Models;
using Models.ServiceDetails;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace ArconImageRecorderCore
{
    class ProxyServiceDetails : ProxyBase
    {
        private static readonly string apiUrl = "api/ServiceDetails/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyServiceDetails(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        private APIConfig GetRequestConfig(string apiName, string contentType = "application/json", string method = "POST")
        {
            try
            {
                APIConfig objAPIConfig = new APIConfig()
                {
                    RequestContentType = contentType,
                    RequestMethod = method,
                    APIUrl = apiUrl + apiName,
                    AuthToken = APIConfig.AuthToken,
                    APIBaseUrl = APIConfig.APIBaseUrl
                };
                return objAPIConfig;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void SetSessionVideoLog(Bitmap image, int sessionId)
        {
            try
            {
                var arrImage = Helper.ConvertImageToByteArray(image);
                SetSessionVideoLog objSetSessionVideoLog = new SetSessionVideoLog()
                {
                    ServiceSessionId = sessionId.ToString(),
                    Base64image = Convert.ToBase64String(arrImage),
                    Image = arrImage
                };
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("SetSessionVideoLog") };
                var request = objProxyHelper.CreateRequest(objSetSessionVideoLog);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<List<string>>>(request);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
