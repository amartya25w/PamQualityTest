using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Text;

namespace ArconErrorLogger.BusinessLayer
{
    public class Log4NetHelper
    {
        private static string _LogFilePath = AppDomain.CurrentDomain.BaseDirectory;

        public static void ConfigureLogAppenders(Hierarchy objHierarchy, string connectionString)
        {
            try
            {
                Log4NetDBHelper.ConfigureDBAppender(objHierarchy, connectionString);
            }
            catch(Exception ex)
            {
                WriteFile(ex);
            }
        }

        private static void WriteFile(Exception exception)
        {
            string exceptionMessage = string.Empty;
            string path = string.Empty;
            try
            {
                if (!Directory.Exists(_LogFilePath + "\\ErrorLog\\"))
                    Directory.CreateDirectory(_LogFilePath + "\\ErrorLog\\");
                path = _LogFilePath + "\\ErrorLog\\" + "\\ErrorLog_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                using (FileStream objFileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter fileToWrite = new StreamWriter(objFileStream, Encoding.Unicode))
                    {
                        if (exception != null)
                            exceptionMessage = " Source:: " + exception.Source + " InnerException:: " + exception.InnerException + " StackTrace:: " + exception.StackTrace;
                        fileToWrite.WriteLine(exceptionMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
