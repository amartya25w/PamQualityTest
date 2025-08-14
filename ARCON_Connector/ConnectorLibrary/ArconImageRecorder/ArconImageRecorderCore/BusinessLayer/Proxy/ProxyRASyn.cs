using ArconAPIUtility;
using log4net;
using Models;
using Models.RA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace ArconImageRecorderCore
{
    public class ProxyRASyn : ProxyBase
    {
        private static readonly string apiUrl = "api/RASyn/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProxyRASyn(APIConfig objAPIConfig) : base(objAPIConfig)
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

        public void GetImgCapturedDet(Bitmap image, int sessionId)
        {
            try
            {
                RDPImageDetails objRDPImageDetails = new RDPImageDetails()
                {
                    ssl_log_id = sessionId,
                    RdpDate = DateTime.Now,
                    Action = "Image",
                    pImage = Helper.ConvertImageToByteArray(image)
                };
                Convert.ToBase64String(objRDPImageDetails.pImage);
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("GetImgCapturedDet") };
                var request = objProxyHelper.CreateRequest(objRDPImageDetails);
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
