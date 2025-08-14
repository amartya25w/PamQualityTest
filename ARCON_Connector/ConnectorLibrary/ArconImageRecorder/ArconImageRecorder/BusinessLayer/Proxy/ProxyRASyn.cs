using ArconAPIUtility;
using log4net;
using Models;
using Models.RA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace ArconImageRecorder
{
    public class ProxyRASyn : ProxyBase
    {
        public override string ApiUrl { get; set; }    = "api/RASyn/";
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int ImgCounter = 0;
        public ProxyRASyn(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }

        public void GetImgCapturedDet(byte[] image, int sessionId)
        {
            try
            {
                RDPImageDetails objRDPImageDetails = new RDPImageDetails()
                {
                    ssl_log_id = sessionId,
                    RdpDate = DateTime.Now,
                    Action = "Image",
                    pImage = image,
                    ImgCounter=++ImgCounter
                };
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
