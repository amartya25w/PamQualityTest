using ArconConnector.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ArconConnector.BusinessLayer
{
    public class ExeHelper
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetExeParameter(BaseConnector objBaseConnector, ArconContext objArconContext)
        {
            try
            {
                if (objBaseConnector != null && objBaseConnector.ServiceDetails != null && !string.IsNullOrEmpty(objBaseConnector.ServiceDetails.JsonData))
                {
                    var lstJsonData = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(objBaseConnector.ServiceDetails.JsonData);
                    if (lstJsonData != null && lstJsonData.Any())
                    {
                        string exeParameter = lstJsonData.SelectMany(data => data).Where(data => data.Key == "exeargument").FirstOrDefault().Value;
                        //var lstPropData = GetPropertyData(objBaseConnector, objBaseConnector, objBaseConnector.GetType().Name);
                        if (!string.IsNullOrEmpty(exeParameter))
                        {
                            var lstPropName = Regex.Matches(exeParameter.Replace(Environment.NewLine, ""), @"\[([^]]*)\]")
                                               .Cast<Match>()
                                               .Select(x => x.Groups[1].Value)
                                               .ToList();
                            var lstPropData = GetPropertyValue(objBaseConnector, lstPropName);
                            lstPropData.AddRange(GetAdditionalExeParameterDetails(objBaseConnector, objArconContext));
                            exeParameter = lstPropData.Aggregate(exeParameter, (current, replacement) => current.Replace("[" + replacement.Key + "]", replacement.Value));
                            return exeParameter;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static bool GenerateFile(BaseConnector objBaseConnector, ArconContext objArconContext, string path)
        {
            try
            {
                if (objBaseConnector != null && objBaseConnector.ServiceDetails != null && !string.IsNullOrEmpty(objBaseConnector.ServiceDetails.FileData))
                {
                    var fileData = objBaseConnector.ServiceDetails.FileData;
                    var separator = "--PropName--";
                    if (fileData != "" && fileData.IndexOf(separator) > 0)
                    {
                        var propName = fileData.Substring(0, fileData.IndexOf(separator));
                        fileData = fileData.Substring(fileData.IndexOf(separator) + separator.Length);

                        var lstPropName = propName.Split(',').ToList(); ;

                        if (lstPropName.Any())
                        {
                            var lstPropData = GetPropertyValue(objBaseConnector, lstPropName);
                            lstPropData.AddRange(GetAdditionalExeParameterDetails(objBaseConnector, objArconContext));
                            fileData = lstPropData.Aggregate(fileData, (current, replacement) => current.Replace("[" + replacement.Key + "]", replacement.Value));
                            using (System.IO.StreamWriter swriter = new System.IO.StreamWriter(path))
                            {
                                swriter.Write(fileData);
                            }
                            return true;
                        }
                        return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static string GetJsonDataByReplacingProperty(BaseConnector objBaseConnector, ArconContext objArconContext)
        {
            try
            {
                if (objBaseConnector != null && objBaseConnector.ServiceDetails != null && !string.IsNullOrEmpty(objBaseConnector.ServiceDetails.JsonData))
                {
                    var jsonData = objBaseConnector.ServiceDetails.JsonData;
                    var lstJsonData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(objBaseConnector.ServiceDetails.JsonData);
                    if (lstJsonData != null && lstJsonData.Any())
                    {
                        var lstResult = lstJsonData.SelectMany(data => data.Values).Distinct().ToList();
                        var lstPropName = new List<string>();
                        lstResult.ForEach(data =>
                        {
                            lstPropName.AddRange(
                                                 Regex.Matches(data.ToString().Replace(Environment.NewLine, ""), @"\[([^]]*)\]")
                                                 .Cast<Match>()
                                                 .Select(x => x.Groups[1].Value)
                                                 .ToList()
                                                );
                        });
                        lstPropName = lstPropName.Distinct().ToList();
                        if (lstPropName.Any())
                        {
                            var lstPropData = GetPropertyValue(objBaseConnector, lstPropName);
                            lstPropData.AddRange(GetAdditionalExeParameterDetails(objBaseConnector, objArconContext));
                            jsonData = lstPropData.Aggregate(jsonData, (current, replacement) => current.Replace("[" + replacement.Key + "]", replacement.Value));

                        }
                        return jsonData;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private static List<KeyValuePair<string, string>> GetAdditionalExeParameterDetails(BaseConnector objBaseConnector, ArconContext objArconContext)
        {
            var jsonData = objBaseConnector.ServiceDetails.JsonData;
            List<KeyValuePair<string, string>> lstKVP = new List<KeyValuePair<string, string>>();
            lstKVP.Add(new KeyValuePair<string, string>("DateTime", DateTime.Now.ToString("ddMMyyHHmmss")));
            //lstKVP.Add(new KeyValuePair<string, string>("WebURL", ""));
            //lstKVP.Add(new KeyValuePair<string, string>("AppParametersPrefix", ""));
            //lstKVP.Add(new KeyValuePair<string, string>("IPAddressOriginal", ""));

            var key = "UserPreference";
            if (jsonData.Contains(key))
            {
                var startIndex = jsonData.IndexOf(key);
                var length = jsonData.IndexOf(']', startIndex) - startIndex;
                key = jsonData.Substring(startIndex, length);
                var arrData = key.Split('|');
                if (arrData != null && arrData.Length == 1)
                {
                    UserManager objUserManager = new UserManager() { ArconContext = objArconContext };
                    lstKVP.Add(new KeyValuePair<string, string>(key, objUserManager.GetUserPreference(objBaseConnector.UserDetails.UserId, Convert.ToInt32(arrData[1]))));
                }
            }
            return lstKVP;
        }

        public static string GetExeWindowTitle(BaseConnector objBaseConnector)
        {
            try
            {
                if (objBaseConnector != null && objBaseConnector.ServiceDetails != null && !string.IsNullOrEmpty(objBaseConnector.ServiceDetails.JsonData))
                {
                    var lstJsonData = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(objBaseConnector.ServiceDetails.JsonData);
                    if (lstJsonData != null && lstJsonData.Any())
                    {
                        string exeTitle = lstJsonData.SelectMany(data => data).Where(data => data.Key == "exetitle").FirstOrDefault().Value;
                        if (!string.IsNullOrEmpty(exeTitle))
                        {
                            //var lstPropData = GetPropertyData(objBaseConnector, objBaseConnector, objBaseConnector.GetType().Name);
                            var lstPropName = Regex.Matches(exeTitle.Replace(Environment.NewLine, ""), @"\[([^]]*)\]")
                                                   .Cast<Match>()
                                                   .Select(x => x.Groups[1].Value)
                                                   .ToList();
                            var lstPropData = GetPropertyValue(objBaseConnector, lstPropName);
                            exeTitle = lstPropData.Aggregate(exeTitle, (current, replacement) => current.Replace("[" + replacement.Key + "]", replacement.Value));
                            return exeTitle;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static List<KeyValuePair<string, string>> GetPropertyData<T>(object objData, T type, string propClassName = "", PropertyInfo[] propInfo = null, object propValue = null)
        {
            var key = "";
            try
            {
                List<KeyValuePair<string, string>> lstKVPProp = new List<KeyValuePair<string, string>>();
                List<PropertyInfo> lstProp = new List<PropertyInfo>();
                var nameSpace = objData.GetType().Namespace;
                if (propInfo != null && propInfo.Any())
                    lstProp = propInfo.ToList();
                else
                    lstProp = type.GetType().GetProperties().ToList();

                foreach (var prop in lstProp)
                {
                    if (type.ToString().Contains(nameSpace) || prop.PropertyType.Assembly.FullName.Contains(nameSpace))
                    {
                        if (prop.PropertyType.IsClass && prop.PropertyType.Assembly.FullName.Contains(nameSpace))
                        {
                            var propClass = !string.IsNullOrEmpty(propClassName) ? propClassName + "." + prop.Name : prop.Name;
                            object propData = null;
                            if (propValue == null && (propInfo == null || !propInfo.Any()))
                                propData = prop.GetValue(objData);
                            else if (propValue != null && (propInfo != null && propInfo.Any()))
                                propData = propValue.GetType().GetProperty(prop.Name).GetValue(propValue);
                            lstKVPProp.AddRange(GetPropertyData(objData, prop.PropertyType, propClass, prop.PropertyType.GetProperties(), propData));
                        }
                        else
                        {
                            key = !string.IsNullOrEmpty(propClassName) ? propClassName + "." + prop.Name : prop.Name;
                            var value = propValue == null ? string.Empty : Convert.ToString(propValue.GetType().GetProperty(prop.Name).GetValue(propValue)); //Convert.ToString(temp.GetValue(objData, null));
                            if (!string.IsNullOrEmpty(value))
                                lstKVPProp.Add(new KeyValuePair<string, string>(key, value));
                        }
                    }
                }
                return lstKVPProp;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static List<KeyValuePair<string, string>> GetPropertyValue(object src, List<string> lstPropName)
        {
            try
            {
                List<KeyValuePair<string, string>> lstKVPProp = new List<KeyValuePair<string, string>>();
                foreach (var propName in lstPropName)
                    lstKVPProp.Add(new KeyValuePair<string, string>(propName, Convert.ToString(GetPropertyValue(src, propName))));
                return lstKVPProp;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static object GetPropertyValue(object src, string propName)
        {
            try
            {
                if (propName.Contains(".")) //complex type nested
                {
                    var arrProp = propName.Split('.');
                    object prevResult = src;
                    foreach (var prop in arrProp)
                        prevResult = GetPropertyValue(prevResult, prop);
                    return prevResult;
                }
                else
                {
                    if (src.GetType().IsGenericType)
                        return GetComplexPropertyValue(src, propName);
                    var prop = src.GetType().GetProperty(propName);
                    return prop != null ? prop.GetValue(src, null) : null;
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Currently this method will handle only one level of Serach in Collection type
        /// Sample format : [ServiceDetails.SettingParameter.Key|Value=ShowProcessInFrame]
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetComplexPropertyValue(object src, string propName)
        {
            //if (prop.PropertyType.IsEnum)
            //    return "";
            if (src.GetType().IsGenericType && src.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                dynamic arrData = propName.Split('=');
                if (arrData != null && arrData.Length > 1)
                {
                    var tempData = arrData[0].Split('|');
                    if (tempData != null && tempData.Length > 1)
                    {
                        var lst = (System.Collections.IList)src;
                        foreach (var data in lst)
                        {
                            var property = data.GetType().GetProperty(tempData[0]);
                            if (property != null)
                            {
                                var value = property.GetValue(data, null);
                                if (value == arrData[1])
                                {
                                    property = data.GetType().GetProperty(tempData[1]);
                                    value = property != null ? property.GetValue(data, null) : null;
                                    return value;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
