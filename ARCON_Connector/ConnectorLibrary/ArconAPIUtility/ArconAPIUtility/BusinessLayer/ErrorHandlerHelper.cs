using ArconWinNativeAPI;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArconAPIUtility
{
    public class ErrorHandlerHelper
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _LogCode = "ERRH", _MethodName = string.Empty;

        public static void HandleConnectionError(WebException objException)
        {
            if (objException.Status == WebExceptionStatus.ConnectFailure || objException.Status == WebExceptionStatus.Timeout)
                SendMessageToProcess(objException);
        }

        private static void SendMessageToProcess(Exception objException)
        {
            _LogCode = "ERRH:SMTP";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                var exception = JsonConvert.SerializeObject(objException);
                string errorMsg = "ERRORAPI:100|" + exception;
                Process objProcess = Process.GetCurrentProcess();
                if (objProcess.MainWindowHandle != IntPtr.Zero)
                    User32APIManager.SendStringMessageToApplication(objProcess.MainWindowHandle, IntPtr.Zero, errorMsg);
                else
                {
                    var lstHandle = User32APIManager.WindowHandles(objProcess);
                    foreach (var hwd in lstHandle)
                    {
                        var title = User32APIManager.GetWindowText(hwd);
                        if (!string.IsNullOrEmpty(title))
                            User32APIManager.SendStringMessageToApplication(hwd, IntPtr.Zero, errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
            }
            _Log.Info(_MethodName + " Method Ended");
        }
    }
}
