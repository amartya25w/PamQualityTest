using log4net;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Runtime.InteropServices;

namespace ArconImageRecorder
{
    public static class ImageRecorderHelper
    {
        private static string _LogFilePath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
        private static readonly MemoryCache _Cache = MemoryCache.Default;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string _ModuleName = "CONNIMRC_";

        public static void WriteMsg(string msg, bool overrideData = false)
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

        public static void CleanLogFile()
        {
            File.WriteAllBytes(_LogFilePath, new byte[0]);
        }

        public static string GetFileName()
        {
            string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            return fileName;
        }

        public static byte[] ConvertImageToByteArray(Bitmap bitmap)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static void SetDataFromCacheByKey<T>(string key, T value, DateTime absoluteExpiration = default(DateTime), bool overrideData = false, string moduleName = _ModuleName)
        {
            try
            {
                key = moduleName + key;
                if (absoluteExpiration == default(DateTime))
                    absoluteExpiration = DateTime.UtcNow.AddHours(24);
                CacheItem cacheItem = new CacheItem(key) { Key = key, Value = value };
                var data = _Cache.Get(key);
                if (data != null && !overrideData)
                    return;
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
    }
}
