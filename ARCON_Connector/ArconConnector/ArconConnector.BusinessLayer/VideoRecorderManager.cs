using ArconConnector.ServiceLayer;
using ArconImageRecorder;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class VideoRecorderManager : ArconBaseManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ImageRecorder GetVideoRecordingConfiguration(int serviceId, ServiceType servicetype)
        {
            //Call the API to get below details refer FillVideoLogSettings
            ProxyVideoLog objProxyVideoLog = new ProxyVideoLog(ArconContext.APIConfig);
            objProxyVideoLog.GetVideoLogSettings(serviceId, servicetype);
            try
            {
                ImageRecorder objImageRecorder = new ImageRecorder
                {
                    ImageStorageType = ImageStorageType.Database,
                    ImageFormat = ImageFormat.Png,
                    TimeInterval = 250,
                    ImagePath = AppDomain.CurrentDomain.BaseDirectory,
                    KeepImagesInMemory = IsRealTimeSessionMonitoringActive()
                };
                return objImageRecorder;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public bool IsRealTimeSessionMonitoringActive()
        {
            try
            {
                ProxyConfiguration objProxyConfiguration = new ProxyConfiguration(ArconContext.APIConfig);
                var result = objProxyConfiguration.GetArcosConfiguration(
                                new EntityModel.Configuration.APIConfiguration
                                {
                                    ArcosConfigId = 41,
                                    ArcosConfigSubId = 5
                                });
                if (result != null)
                    return result.ArcosConfigDefaultValue == "1";
                return false;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw;
            }
        }
    }
}
