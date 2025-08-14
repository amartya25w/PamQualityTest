using ArconConnector.BusinessEntities;
using ARCONPAMSecurity;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.ServiceLayer
{
    public class ServiceHelper
    {
        private static string _LogFilePath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
        private static readonly MemoryCache _Cache = MemoryCache.Default;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static EncryptionDecryption_F _EncryptionDecryptionF = new EncryptionDecryption_F();
        private const string _ModuleName = "CONN_";

        public static void WriteMsg(string msg, bool overrideData = false)
        {
            try
            {
                if (overrideData)
                    CleanLogFile();
                if (!string.IsNullOrEmpty(msg))
                {
                    using (StreamWriter sw = File.AppendText(_LogFilePath))
                    {
                        sw.WriteLine(DateTime.Now + " " + msg);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static void CleanLogFile()
        {
            try
            {
                File.WriteAllBytes(_LogFilePath, new byte[0]);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static string GetFileName()
        {
            try
            {
                string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                return fileName;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        #region Caching
        public static void SetDataFromCacheByKey<T>(string key, T value, DateTime absoluteExpiration = default(DateTime), string moduleName = _ModuleName)
        {
            try
            {
                key = moduleName + key;
                if (absoluteExpiration == default(DateTime))
                    absoluteExpiration = DateTime.UtcNow.AddHours(24);
                CacheItem cacheItem = new CacheItem(key) { Key = key, Value = value };
                if (!_Cache.Contains(key))
                    _Cache.Set(cacheItem, new CacheItemPolicy() { AbsoluteExpiration = absoluteExpiration });
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static object GetDataFromCacheByKey(string key, string moduleName = _ModuleName)
        {
            key = moduleName + key;
            return _Cache.Get(key);
        }

        public static bool HasCacheByKey(string key, string moduleName = _ModuleName)
        {
            key = moduleName + key;
            return _Cache.Contains(key);
        }

        public static void RemoveDataFromCacheByKey(string key, string moduleName = _ModuleName)
        {
            key = moduleName + key;
            if (HasCacheByKey(key,string.Empty))
                _Cache.Remove(key);
        }
        #endregion Caching

        #region Encryption/Decryption
        public static string EncryptTextF(string data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                    return _EncryptionDecryptionF.Encrypt_F(data);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
            return string.Empty;
        }

        public static string DecryptTextF(string data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                    return _EncryptionDecryptionF.Decrypt_F(data);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
            return string.Empty;
        }
        #endregion

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

    }
}
