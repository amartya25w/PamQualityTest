using ARCONPAMSecurity;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using ArconConnector.BusinessEntities;

namespace ArconConnector.BusinessLayer
{
    public static class ConnectorHelper
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

        public static string GenerateRandomDigits(int value)
        {
            string result = string.Empty;
            try
            {
                Random rnd = new Random(DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + +DateTime.Now.Millisecond);
                for (int i = 1; i <= value; i++)
                    result = result + rnd.Next(1, 9) + " ";
                return result;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static string GetRandomLocalhostIP()
        {
            try
            {
                Random objRandom = new Random();
                return "127.0.0." + objRandom.Next(1, 255).ToString();
            }
            catch(Exception ex)
            {
                _Log.Error(ex);
                return "2001";
            }
        }

        public static string GetRandomPort()
        {
            try
            {
                Random objRandom = new Random();
                return objRandom.Next(2000, 10000).ToString();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                return "2001";
            }
        }
    }
}
