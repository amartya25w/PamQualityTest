using log4net;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Runtime.InteropServices;

namespace ArconImageRecorderCore
{
    public static class Helper
    {
        private static string _LogFilePath = @"D:\Shrina\ConnectorPOC\Logs\Log.txt";
        //private static string _LogFilePath = @"N:\images\Log.txt";
        private static readonly MemoryCache _Cache = MemoryCache.Default;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            BitmapData bmpdata = null;
            try
            {
                bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int numbytes = bmpdata.Stride * bmpdata.Height;
                byte[] arrImage = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;
                Marshal.Copy(ptr, arrImage, 0, numbytes);
                return arrImage;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }
        }

        public static void SetDataFromCacheByKey<T>(string key, T value, DateTime absoluteExpiration)
        {
            try
            {
                CacheItem cacheItem = new CacheItem(key) { Key = key, Value = value };
                var data = _Cache.Get(key);
                if (data != null)
                    return;
                _Cache.Add(cacheItem, new CacheItemPolicy() { AbsoluteExpiration = absoluteExpiration });
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public static object GetDataFromCacheByKey(string key)
        {
            return _Cache.Get(key);
        }
    }
}
