using ArconWinNativeAPI;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ArconRealTimeSessionMonitor.Enum;

namespace ArconRealTimeSessionMonitor
{
    public class StreamAgentManager : IDisposable
    {
        #region Variables
        private NetworkToolManager objNetworkToolManager;
        private StreamAgent _StreamAgent = null;
        private System.Timers.Timer _TmrStreamMonitor;
        private RealTimeServiceLog _RealTimeServiceLog = null;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<int, int> lstKVPSessionImageDetails = new Dictionary<int, int>();
        #endregion

        #region EventHandlers
        //public event EventHandler<MessageReceivedArgs> CloseSessionReceived;
        //public event EventHandler<MessageReceivedArgs> FreezeWindowReceived;
        //public event EventHandler<MessageReceivedArgs> UnfreezeWindowReceived;
        #endregion

        public StreamAgentManager(StreamAgent objStreamAgentDtl, RealTimeServiceLog objRealTimeServiceLog)
        {
            if (_StreamAgent == null)
                _StreamAgent = objStreamAgentDtl;
            if (_RealTimeServiceLog == null)
                _RealTimeServiceLog = objRealTimeServiceLog;
        }

        public StreamAgentManager(StreamAgent objStreamAgentDtl, int sessionId)
        {
            if (_StreamAgent == null)
                _StreamAgent = objStreamAgentDtl;
            if (_RealTimeServiceLog == null)
                _RealTimeServiceLog = new RealTimeServiceLog() { ServiceSessionId = sessionId };
        }

        private void AddEventHandler()
        {
            if (_StreamAgent != null)
            {
                //objStreamAgent.FreezeWindowReceived = new EventHandler<MessageReceivedArgs>(ObjStreamAgent_FreezeWindowReceived);
            }
        }

        public Boolean StartMonitoring()
        {
            try
            {
                objNetworkToolManager = new NetworkToolManager();
                objNetworkToolManager.MessageReceivedCallback = OnMessageReceived;
                objNetworkToolManager.StartServer(_StreamAgent.LocalPort, objNetworkToolManager.MessageReceivedCallback);
                InitialiseTimer();
                Task.Run(() => CleanUpOldImageFromCache());
                return true;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public Boolean StopMonitoring()
        {
            try
            {
                objNetworkToolManager.StopServer();
                return true;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void OnMessageReceived(MessageReceivedArgs e)
        {
            try
            {
                SessionMonitorHelper.SetDataFromCacheByKey("RTSMStartedFor_" + _RealTimeServiceLog.ServiceSessionId, true, DateTime.Now.AddHours(1), string.Empty);
                switch (e.Command)
                {
                    case "$leftclick":
                    case "$rightclick":
                        User32APIManager.Mouse_event((uint)User32.MouseEventFlags.LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                        User32APIManager.Mouse_event((uint)User32.MouseEventFlags.LEFTUP, 0, 0, 0, UIntPtr.Zero);
                        break;
                    case "$startview":
                        if (!e.RemoteNode.Attributes.Contains("view_flag"))
                            e.RemoteNode.Attributes.Add("view_flag", "");
                        break;
                    case "$startcontrol":
                        if (!e.RemoteNode.Attributes.Contains("control_flag"))
                            e.RemoteNode.Attributes.Add("control_flag", "");
                        break;
                    case "$stopcontrol":
                        if (e.RemoteNode.Attributes.Contains("control_flag"))
                            e.RemoteNode.Attributes.Remove("control_flag");
                        if (e.RemoteNode.Attributes.Contains("view_flag"))
                            e.RemoteNode.Attributes.Remove("view_flag");
                        SessionMonitorHelper.RemoveDataFromCacheByKey("RTSMStartedFor_" + _RealTimeServiceLog.ServiceSessionId, string.Empty);
                        break;
                    case "$screencap":
                        //GrabScreen();
                        break;
                    case "$mousepos":
                        string[] data = e.Body.Split('*');
                        SetCursorPosition(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
                        break;
                    case "$showmsg":
                        MessageBox.Show(e.Body, "Message from Administrator", MessageBoxButtons.OK);
                        break;
                    case "$SessionClose":
                        if (_StreamAgent.CloseSessionReceived != null && e != null)
                            _StreamAgent.CloseSessionReceived(this, e);
                        SessionMonitorHelper.RemoveDataFromCacheByKey("RTSMStartedFor_" + _RealTimeServiceLog.ServiceSessionId, string.Empty);
                        break;
                    case "$FreezeWindow":
                        if (_StreamAgent.FreezeWindowReceived != null && e != null)
                            _StreamAgent.FreezeWindowReceived(this, e);
                        SessionMonitorHelper.RemoveDataFromCacheByKey("RTSMStartedFor_" + _RealTimeServiceLog.ServiceSessionId, string.Empty);
                        break;
                    case "$UnfreezeWindow":
                        if (_StreamAgent.UnfreezeWindowReceived != null && e != null)
                            _StreamAgent.UnfreezeWindowReceived(this, e);
                        break;
                    case "$message":
                    case "$file":
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void InitialiseTimer()
        {
            try
            {
                if (_TmrStreamMonitor == null || !_TmrStreamMonitor.Enabled)
                {
                    _TmrStreamMonitor = new System.Timers.Timer()
                    {
                        Enabled = true,
                        Interval = _StreamAgent.TimeInterval
                    };
                    _TmrStreamMonitor.Elapsed += TmrStreamMonitorTick;
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void TmrStreamMonitorTick(object sender, EventArgs e)
        {
            try
            {
                if (_StreamAgent.IsSessionFreezed)
                {
                    foreach (Node client in objNetworkToolManager.objNetworkTool.Connections.Values)
                    {
                        if (client.Attributes.Contains("control_flag") || client.Attributes.Contains("view_flag"))
                            objNetworkToolManager.SendMessage(client, "$SessionFreezed", _StreamAgent.SessionFreezedMessage);
                    }
                    return;
                }
                //switch ((int)objStreamAgent.WindowHandlerToMonitor)
                //{

                //    case 0:
                //        grabbed = CaptureScreen();
                //        break;
                //    default:
                //        grabbed = CaptureWindow(objStreamAgent.WindowHandlerToMonitor, objStreamAgent.DesktopHandlerToMonitor);
                //        break;
                //}

                var imageData = GetCapturedImage(_RealTimeServiceLog.ServiceSessionId);
                if (objNetworkToolManager.objNetworkTool.Connections.Count != 0)
                {
                    if (string.IsNullOrEmpty(imageData))
                        return;
                    foreach (Node client in objNetworkToolManager.objNetworkTool.Connections.Values)
                    {
                        if (client.Attributes.Contains("control_flag") || client.Attributes.Contains("view_flag"))
                            objNetworkToolManager.SendMessage(client, "$screen", imageData);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public int GetLastImageCounter(int sessionId)
        {
            try
            {
                int lastImageCounter = 0;
                if (lstKVPSessionImageDetails.Any(data => data.Key == sessionId))
                {
                    lastImageCounter = lstKVPSessionImageDetails.Where(data => data.Key == sessionId).FirstOrDefault().Value;

                    var value = SessionMonitorHelper.GetDataFromCacheByKey("Image_" + _RealTimeServiceLog.ServiceSessionId.ToString(), string.Empty);
                    if (value != null)
                    {
                        var imageCounter = Convert.ToInt32(value.ToString().Split('_')[1]);
                        if (imageCounter > lastImageCounter)
                        {
                            lastImageCounter = lastImageCounter + 1;
                            lstKVPSessionImageDetails[sessionId] = lastImageCounter;
                        }
                        else if (lastImageCounter > imageCounter)
                            lastImageCounter = -1;
                    }
                }
                else
                {
                    var value = SessionMonitorHelper.GetDataFromCacheByKey("Image_" + _RealTimeServiceLog.ServiceSessionId.ToString(), string.Empty);
                    if (value != null)
                    {
                        lastImageCounter = Convert.ToInt32(value.ToString().Split('_')[1]);
                        lstKVPSessionImageDetails.Add(sessionId, lastImageCounter);
                    }
                }
                return lastImageCounter;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public string GetCapturedImage(int sessionId)
        {
            try
            {
                string image = string.Empty;
                var imageCounter = GetLastImageCounter(sessionId);
                if (imageCounter >= 0)
                {
                    var key = sessionId + "_" + imageCounter;
                    var data = SessionMonitorHelper.GetDataFromCacheByKey(key, string.Empty);
                    if (data != null)
                    {
                        image = data.ToString();
                        SessionMonitorHelper.RemoveDataFromCacheByKey(key, string.Empty);
                    }
                }
                return image;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public void CleanUpOldImageFromCache()
        {
            try
            {
                var value = SessionMonitorHelper.GetDataFromCacheByKey("Image_" + _RealTimeServiceLog.ServiceSessionId.ToString(), string.Empty);
                if (value != null)
                {
                    value = value.ToString().Replace(_RealTimeServiceLog.ServiceSessionId + "_", "");
                    for (int i = 0; i < Convert.ToInt32(value); i++)
                    {
                        var key = _RealTimeServiceLog.ServiceSessionId + "_" + i;
                        if (SessionMonitorHelper.HasCacheByKey(key))
                            SessionMonitorHelper.RemoveDataFromCacheByKey(key);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void SetCursorPosition(int x, int y)
        {
            Point newpos = new Point(x, y);
            Cursor.Position = newpos;
        }

        public void Dispose()
        {
            if (_TmrStreamMonitor != null)
                _TmrStreamMonitor.Dispose();
        }
    }
}
