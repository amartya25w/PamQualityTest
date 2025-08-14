using ArconAPIUtility;
using ArconConnector.BusinessEntities;
using EntityModel.ServiceDetails;
using log4net;
using Models;
using Models.ServiceDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;
using Enum = System.Enum;

namespace ArconConnector.ServiceLayer
{
    public class ProxyServiceDetails : ProxyBase
    {
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override string ApiUrl { get; set; } = "api/ServiceDetails/";

        public ProxyServiceDetails(APIConfig objAPIConfig) : base(objAPIConfig)
        {
        }
        
        public ServiceDetails GetServiceDetailsFromSession(int sessionId)
        {
            try
            {
                string inputParams = "sessionId=" + sessionId;
                string apiUrl = "GetServiceDetailsBySessionId?" + inputParams;
                ProxyHelper objProxyHelper = new ProxyHelper() {  APIConfig = GetRequestConfig(apiUrl) };
                var request = objProxyHelper.CreateRequest(string.Empty);
                var result = objProxyHelper.GetServiceResponse<GenericResponse<ServiceDetailsBySessionId>>(request);
                return MapServiceDetailsObject(result.Result);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private ServiceDetails MapServiceDetailsObject(ServiceDetailsBySessionId objServiceDtl)
        {
            //List<string> lstParameters = new List<string>();
            ServiceDetails objServiceDetails = null;
            //List<SettingParameter> lstSettingParameter = new List<SettingParameter>();
            try
            {
                if (objServiceDtl != null)
                {
                    //var lstProp = objServiceDtl.GetType().GetFields().Where(prop => prop.Name.Contains("Field")).ToList();
                    //foreach (var prop in lstProp)
                    //{
                    //    var value = prop.GetValue(objServiceDtl).ToString();
                    //    if (!string.IsNullOrEmpty(value))
                    //    {
                    //        lstParameters.Add(value);
                    //        lstSettingParameter.Add(new SettingParameter() { Key= prop.Name, Value = value });
                    //    }
                    //}
                    objServiceDetails = new ServiceDetails()
                    {
                        ServiceId = Convert.ToInt32(objServiceDtl.ServiceID),
                        ServiceTypeId = Convert.ToInt32(objServiceDtl.ServiceTypeId),
                        ServiceType = (ServiceType)Enum.Parse(typeof(ServiceType), objServiceDtl.ServiceType),
                        UserName = objServiceDtl.ServiceUsername,
                        Password = objServiceDtl.ServicePassword,
                        DomainName = objServiceDtl.ServiceDomain,
                        Instance = objServiceDtl.InstanceName,
                        Port = objServiceDtl.Port,
                        HostName = objServiceDtl.HostName,
                        IPAddress = objServiceDtl.IpAddress,
                        IsUserLockToConsole = objServiceDtl.IsUserLockToConsole,
                        AllowRoboticProcess = objServiceDtl.IsImageBasedSSO,
                        Field1 = objServiceDtl.Field1,
                        Field2 = objServiceDtl.Field2,
                        Field3 = objServiceDtl.Field3,
                        Field4 = objServiceDtl.Field4,
                        //ParameterJSON = ServiceHelper.CreateJSONFromObject(lstParameters),
                        SettingParameter = new List<SettingParameter>()//ServiceHelper.ConvertStringToSettingParameter(lstParameters),
                        
                    };
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
            return objServiceDetails;
        }

        public List<KeyValuePair<int, string>> GetDomains()
        {
            try
            {
                ProxyHelper objProxyHelper = new ProxyHelper() { APIConfig = GetRequestConfig("GetDomains") };
                var request = objProxyHelper.CreateRequest("");
                var result = objProxyHelper.GetServiceResponse<GenericResponse<List<RequestDomainDetails>>>(request);
                List<KeyValuePair<int, string>> lstDomainName = new List<KeyValuePair<int, string>>();
                foreach (var data in result.Result)
                    lstDomainName.Add(new KeyValuePair<int, string>(Convert.ToInt32(data.DomainId), data.DomainName));
                return lstDomainName;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
