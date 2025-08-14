using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //string PostData = "username=" + Convert.ToBase64String(Encoding.UTF8.GetBytes("mylogin1")) + "&password=" + Convert.ToBase64String(Encoding.UTF8.GetBytes("mylogin@")) + "&grant_type=password";
            //var createRequest = CreateRequest("http://10.10.0.91:48509/", "arcontoken", "POST", "application/x-www-form-urlencoded", PostData);
            //var ResponseData = ResponsePostdata(createRequest);
            //JObject jObject = JObject.Parse(ResponseData);
            //string accessToken = (string)jObject.SelectToken("access_token");
            //string refreshToken = (string)jObject.SelectToken("refresh_token");
            //string jsonData = "{\"token_type\":\"bearer\",\"token\":\"" + accessToken + "\",\"expires_in\":\"86399\",\"refresh_token\":\"" + refreshToken + "\",\"ApiBaseURL\":\"http://10.10.0.91:48509\",\"ServiceId\":\"101\",\"UserSessionId\":\"1\",\"ServiceTypeId\":\"1\",\"UserId\":\"1\",\"ServiceSessionId\":\"122950\",\"ExePath\":\"alok\",\"ExeName\":\"db2pe.bat\"}";//C:\\Program Files (x86)\\IBM\\IBM DB2 Performance Expert Client V3\\bin
            //string[] finalJsonData = jsonData.Split(',');

            string json = string.Empty;
            using (StreamReader r = new StreamReader(@"D:\ConnectorJSON\MSSQL.json"))// @"F:\putty automation\2019-19-2--12-10-11.json"))
            {
                json = r.ReadToEnd();
                //items = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
            }
            //objArconRPA.SendAutomationData(jsonData);
            ArconRPA.RoboticProcessAutomation objArconRPA = new ArconRPA.RoboticProcessAutomation();
            IntPtr hdwn = IntPtr.Zero;

           

            LaunchExecutable.Launch obj = new LaunchExecutable.Launch();
            Process p= obj.LaunchApplication(json);

            objArconRPA.SendAutomationData(json, p);


        }

     
        public static HttpWebRequest CreateRequest(string URL, string MethodName, string MethodType, string ContentType, string PostData)
        {
            string ServerURl = URL + "/" + MethodName;
            HttpWebRequest request = null;

            request = (HttpWebRequest)WebRequest.Create(ServerURl);
            request.Method = MethodType;
            request.ContentType = ContentType;
            request.ContentLength = PostData.Length;            

            if (ServerURl.ToUpper().StartsWith("HTTPS") == true)
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
            }
            if (PostData != "")
            {
                using (Stream stm = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(PostData);
                    }
                }
            }

            return request;
        }


        protected static string ResponsePostdata(HttpWebRequest request)
        {
            string data = string.Empty;

            try
            {
                using (HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = httpResponse.GetResponseStream())
                    {
                        data = (new StreamReader(stream)).ReadToEnd();

                    }
                }
            }
            catch (WebException webEx)
            {

                HttpWebResponse response = (HttpWebResponse)webEx.Response;
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        string challenge = null;
                        challenge = response.GetResponseHeader("WWW-Authenticate");
                        if (challenge != null)
                        {
                            data = "Error : Authentication failed";
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)//400
                    {
                        data = "Error : Bad Request";
                    }
                    else
                    {
                        data = "Error : " + webEx.Message.ToString();
                    }
                }
                else
                {
                    data = "Error : " + webEx.Message.ToString();
                }

            }
            return data;
        }

    }

}
