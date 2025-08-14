using ArconAPIUtility;
using ArconConnector.BusinessEntities;
using ArconConnector.ServiceLayer;
using EntityModel.Configuration;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class CommonManager : ArconBaseManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static APIConfig GetAPIConfigurations(string conStr)
        {
            try
            {
                string PAMAPIUrl = "", PAMAPIUsername = "", PAMAPIPwd = "";
                System.Data.DataTable dt = GetorSetAppSettings(conStr);

                if (dt.Rows.Count > 0)
                {
                    PAMAPIUrl = dt.Rows[0]["URL"].ToString();
                    PAMAPIUsername = dt.Rows[0]["UserName"].ToString();
                    PAMAPIPwd = dt.Rows[0]["Password"].ToString();
                }

                //Call API to set below object
                APIConfig objAPIConfig = new APIConfig()
                {
                    APIBaseUrl = PAMAPIUrl,// ConfigurationManager.AppSettings["PAMAPIUrl"],//"https://10.10.0.91:50311/",
                    AuthToken = new APIAuthToken()
                    {
                        //APIUserName = PAMAPIUsername, // ConfigurationManager.AppSettings["PAMAPIUsername"],
                        //APIPassword = PAMAPIPwd //ConfigurationManager.AppSettings["PAMAPIPwd"]
                         APIUserName = ConfigurationManager.AppSettings["PAMAPIUsername"],
                        APIPassword = ConfigurationManager.AppSettings["PAMAPIPwd"]
                    }
                };
                return objAPIConfig;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static APIConfig GetWebDTServiceConfigurations(string url, string credential)
        {
            try
            {
                APIConfig objAPIConfig = new APIConfig()
                {
                    APIUrl = url,
                    AuthToken = new APIAuthToken()
                    {
                        APIUserName = credential,
                        APIPassword = credential
                    }
                };
                return objAPIConfig;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        ///  Soumya
        ///  Database call need to be removed later.
        /// <returns></returns>
        public static System.Data.DataTable GetorSetAppSettings(string conStr)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(conStr);
            try
            {
                System.Data.SqlClient.SqlParameter[] pSQLParam = new System.Data.SqlClient.SqlParameter[11];
                pSQLParam[0] = new System.Data.SqlClient.SqlParameter("@ActionType", System.Data.SqlDbType.VarChar, 100);
                pSQLParam[1] = new System.Data.SqlClient.SqlParameter("@AppName", System.Data.SqlDbType.VarChar, 100);
                pSQLParam[2] = new System.Data.SqlClient.SqlParameter("@Url", System.Data.SqlDbType.VarChar, 500);
                pSQLParam[3] = new System.Data.SqlClient.SqlParameter("@UserName", System.Data.SqlDbType.VarChar, 50);
                pSQLParam[4] = new System.Data.SqlClient.SqlParameter("@id", System.Data.SqlDbType.Int);
                pSQLParam[5] = new System.Data.SqlClient.SqlParameter("@IpAddress", System.Data.SqlDbType.VarChar, 50);
                pSQLParam[6] = new System.Data.SqlClient.SqlParameter("@Port1", System.Data.SqlDbType.VarChar, 50);
                pSQLParam[7] = new System.Data.SqlClient.SqlParameter("@Port2", System.Data.SqlDbType.VarChar, 50);
                pSQLParam[8] = new System.Data.SqlClient.SqlParameter("@IsSSL", System.Data.SqlDbType.Bit);
                pSQLParam[9] = new System.Data.SqlClient.SqlParameter("@AppCode", System.Data.SqlDbType.VarChar, 50);
                pSQLParam[10] = new System.Data.SqlClient.SqlParameter("@IsActive", System.Data.SqlDbType.Bit);

                pSQLParam[0].Value = "selectid";
                pSQLParam[9].Value = "APP001";

                

                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("usp_GetandSetAppSettings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                for(int paramCount=0; paramCount<pSQLParam.Length; paramCount++)
                {
                    cmd.Parameters.Add(pSQLParam[paramCount]);
                }

                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                
                dt.Load(dr);
                con.Close();
            }
            catch (Exception Ex)
            {
                _Log.Error(Ex);
                throw Ex;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();                
            }
            return dt;

        }

        public static List<SettingParameter> ConvertStringToSettingParameter(List<string> lstData)
        {
            try
            {
                List<SettingParameter> lstSettingParameter = new List<SettingParameter>();
                foreach (var data in lstData)
                {
                    if (!string.IsNullOrEmpty(data))
                        lstSettingParameter.Add(GetSettingParameter(data));
                }
                return lstSettingParameter;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private static SettingParameter GetSettingParameter(string data)
        {
            try
            {
                SettingParameter objSettingParameter = null;
                if (!string.IsNullOrEmpty(data))
                {
                    var arrData = data.Split('>');
                    if (arrData != null && arrData.Length > 1)
                    {
                        for (int i = 0; i < arrData.Length; i = i + 2)
                        {
                            objSettingParameter = new SettingParameter() { Key = arrData[i].Trim('<'), Value = arrData[i + 1] };
                            if (arrData[i + 1].Contains("<") && arrData[i + 1].Contains(">"))
                                objSettingParameter.ChildData = GetSettingParameter(arrData[i + 1]);
                        }
                    }
                }
                return objSettingParameter;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static string CreateJSONFromObject(List<string> lstData)
        {
            try
            {
                if (lstData != null && lstData.Any())
                {
                    List<SettingParameter> lstSettingParameter = ConvertStringToSettingParameter(lstData);
                    return JsonConvert.SerializeObject(lstSettingParameter);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static string GetFormTitle(ServiceType serviceType)
        {
            switch (serviceType)
            {
                case ServiceType.MSSQLEMLocal:
                    return "ARCON PAM MS SQL EM Local";
                case ServiceType.AppToadOracle:
                    return "ARCON PAM TOAD FOR ORACLE";
                case ServiceType.AppSQLDeveloperOracle:
                    return "ARCON PAM SQL DEVELOPER ORACLE";
                case ServiceType.AppToadForSQLServer:
                    return "ARCON PAM TOAD FOR SQLSERVER";
                case ServiceType.AppMySQLWorkBench:
                    return "ARCON PAM MYSQL WORKBENCH";
                case ServiceType.AppCitrixXenCenter:
                    return "ARCON PAM CITRIX XENCENTER";
                case ServiceType.AppDellStorageManagerClient:
                    return "ARCON PAM DELL STORAGE MANAGER CLIENT";
                default:
                    return "ARCON PAM Connector";
            }
        }

        public string GetArcosConfiguration(int configId, int subappId)
        {
            try
            {
                ProxyConfiguration objProxyConfiguration = new ProxyConfiguration(ArconContext.APIConfig);
                var result = objProxyConfiguration.GetArcosConfiguration(new APIConfiguration() { ArcosConfigId = configId, ArcosConfigSubId = subappId });
                return result.ArcosConfigDefaultValue;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public List<(int ConfigId, int SubAppId, string Value)> GetArcosConfigurations(List<(int ConfigId, int SubAppId)> lstConfigs)
        {
            try
            {
                List<(int ConfigId, int SubAppId, string Value)> lstResult = new List<(int, int,string)>();
                List<APIConfiguration> lstAPIConfiguration = new List<APIConfiguration>();
                foreach (var config in lstConfigs)
                    lstAPIConfiguration.Add(new APIConfiguration() { ArcosConfigId = config.ConfigId, ArcosConfigSubId = config.SubAppId });
                ProxyConfiguration objProxyConfiguration = new ProxyConfiguration(ArconContext.APIConfig);
                var result = objProxyConfiguration.GetArcosConfigurations(lstAPIConfiguration);
                if(result !=null && result.Any())
                    result.ForEach(data => { lstResult.Add((Convert.ToInt32(data.ArcosConfigId), Convert.ToInt32(data.ArcosConfigSubId), data.ArcosConfigDefaultValue)); });
                return lstResult;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
