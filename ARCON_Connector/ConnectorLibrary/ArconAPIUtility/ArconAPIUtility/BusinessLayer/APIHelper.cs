using log4net;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Runtime.InteropServices;

namespace ArconAPIUtility
{
    public static class APIHelper
    {
        private static string _LogFilePath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
        private static readonly MemoryCache _Cache = MemoryCache.Default;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string _ModuleName = "CONNAPI_";
        private static string _LogCode = "APIH", _MethodName = string.Empty;

        public static void WriteMsg(string msg, bool overrideData = false)
        {
            _LogCode = "APIH:WM";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                _Log.Debug(_MethodName + " method msg : " + msg + " overrideData : " + overrideData);
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
                _Log.Error(_LogCode, ex);
                throw;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public static void CleanLogFile()
        {
            File.WriteAllBytes(_LogFilePath, new byte[0]);
        }

        public static string GetFileName()
        {
            _LogCode = "APIH:GFs";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                return fileName;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
        }

        #region Caching
        public static void SetDataFromCacheByKey<T>(string key, T value, DateTime absoluteExpiration = default(DateTime), string moduleName = _ModuleName)
        {
            _LogCode = "APIH:SDFC";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                _Log.Debug(_MethodName + " method key : " + key + " moduleName : " + moduleName + " value : " + value);

                key = moduleName + key;
                if (absoluteExpiration == default(DateTime))
                    absoluteExpiration = DateTime.UtcNow.AddHours(24);
                CacheItem cacheItem = new CacheItem(key) { Key = key, Value = value };
                if (!_Cache.Contains(key))
                    _Cache.Set(cacheItem, new CacheItemPolicy() { AbsoluteExpiration = absoluteExpiration });
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
            _Log.Info(_MethodName + " Method Ended");
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

        public static object RemoveDataFromCacheByKey(string key, string moduleName = _ModuleName)
        {
            _LogCode = "APIH:RDFC";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                _Log.Debug(_MethodName + " method key : " + key + " moduleName : " + moduleName);
                key = moduleName + key;
                if (HasCacheByKey(key,string.Empty))
                    return _Cache.Remove(key);
                return null;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw;
            }
        }
        #endregion Caching

    }
}
