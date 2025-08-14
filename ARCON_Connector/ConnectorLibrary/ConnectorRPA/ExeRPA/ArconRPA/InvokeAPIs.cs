using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ArconRPA
{
    class InvokeAPIs
    {
        public dynamic JsonObjectInputData;
        private string username = string.Empty, password = string.Empty, exe_path = string.Empty, exe_name = string.Empty, Comp_type = string.Empty;

        #region Get Image by Service Id
        /// <summary>
        /// InvokeJsonApi - will use serviceid to invoke json api in order to  get application images data in json format. 
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="token_type"></param>
        /// <param name="service"></param>
        public dynamic GetImagebyServiceId(string apiUrl, string token, string token_type, string serviceId)
        {
            try
            {
                Logger.Log.Info("Inside GetImageByServiceId");
                string response = string.Empty, allImages = string.Empty;
                apiUrl = apiUrl + "/api/UIAutomationData/GetImagebyServiceId";
                Logger.Log.Info("apiUrl - " + apiUrl);
                Logger.Log.Info("Service ID - " + serviceId);
                WebRequest request = WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                Logger.Log.Info("Token - " + token);
                Logger.Log.Info("Token_Type - " + token_type);
                string tokenInfo = token_type + " " + token;
                request.Headers.Add("Authorization", tokenInfo);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var input = "ServiceId=" + serviceId;
                    streamWriter.Write(input);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = request.GetResponse();
                Logger.Log.Info("Creating API Call resquest in GetImageByServiceId");
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    allImages = json.Result[0].ImageDetails;
                    Logger.Log.Info("Got result from API - " + allImages);
                    Logger.Log.Info("Calling LoadJsonObject");
                    JsonObjectInputData = LoadJsonObject(allImages);
                    Logger.Log.Info("Calling LoadJsonObject - Completed");

                }
            }
            catch (Exception ex)
            {

                Logger.Log.Info("Inside Catch");
                Logger.Log.Error("Got Some error while calling api and loading images - " + ex.Message, ex);
                throw ex;
            }
            return JsonObjectInputData;

        }
        #endregion

        #region Get Service Details
        /// <summary>
        /// GetServiceDetails - This method is used to get input data from API.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="token_type"></param>
        /// <param name="service"></param>
        /// <param name="usersessionid"></param>
        /// <param name="serviceTypeId"></param>
        /// <param name="userId"></param>
        public void GetServiceDetails(string url, string token, string token_type, string service, string usersessionid, string serviceTypeId, string userId)
        {
            //https://stackoverflow.com/questions/47409678/web-api-authorization-via-httpwebrequest
            try
            {
                Logger.Log.Info("Inside GetServiceDetails");
                Logger.Log.Info("URL - " + url);
                Logger.Log.Info("Token - " + token);
                Logger.Log.Info("Token_type - " + token_type);
                Logger.Log.Info("Service - " + service);
                Logger.Log.Info("UserSessionId - " + usersessionid);
                Logger.Log.Info("ServiceTypeId - " + serviceTypeId);
                Logger.Log.Info("UserId - " + userId);

                url = url + "/api/ServiceDetails/GetServiceDetails";
                Logger.Log.Info("Final Url - " + url);
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                string tokenInfo = token_type + " " + token;

                request.Headers.Add("Authorization", tokenInfo);
                Logger.Log.Info("Creaing web api request");
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var input = "{\"ServiceId\":\"" + service + "\"," +
                    "\"UserSessionId\":\"" + usersessionid + "\"," + "\"ServiceTypeId\":\"" + serviceTypeId + "\"," + "\"UserId\":\"" + userId + "\"}";
                    streamWriter.Write(input);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = request.GetResponse();
                string response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    response = streamReader.ReadToEnd();
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                    username = json.Result.ServiceUsername;
                    password = json.Result.ServicePassword;
                    Logger.Log.Info("Got service response");
                    //Logger.Log.Info("UserName - "+username);
                    //Logger.Log.Info("Password - "+password);

                }
            }
            catch (Exception ex)
            {
                Logger.Log.Info("Inside Catch");
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion

        #region Load Json Object
        private dynamic LoadJsonObject(string json)
        {
            try
            {
                Logger.Log.Info("Inside LoadJsonObjects");
                Logger.Log.Info("Parsing Json data");
                JsonObjectInputData = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(json);
                Logger.Log.Info("Parsing Json data - Completed");
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }

            return JsonObjectInputData;
        }
        #endregion

    }
}
