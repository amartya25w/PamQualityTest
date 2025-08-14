using ArconAPIUtility;
using ArconWinNativeAPI;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ArconImageRecorderCore
{
    public class ImageRecorderManager
    {
        #region Variables
        private static System.Timers.Timer _TmrImageCapture, _TmrCleanUpProcess;
        private ImageRecorder _ImageRecorder;
        private ManagementEventWatcher _StartWatch;
        private int _CleanUpInterval = 500;
        public APIConfig APIConfig;
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Timer
        private void SetupTimer()
        {
            try
            {
                if (_ImageRecorder != null && _ImageRecorder.TimeInterval > 0)
                {
                    if (_TmrImageCapture == null || !_TmrImageCapture.Enabled)
                    {
                        _TmrImageCapture = new System.Timers.Timer()
                        {
                            Enabled = true,
                            Interval = _ImageRecorder.TimeInterval,
                        };
                        _TmrImageCapture.Elapsed += TimerImageCaptureTick;
                    }
                    if (_TmrCleanUpProcess == null || !_TmrCleanUpProcess.Enabled)
                    {
                        _TmrCleanUpProcess = new System.Timers.Timer()
                        {
                            Enabled = true,
                            Interval = _CleanUpInterval,
                        };
                        _TmrCleanUpProcess.Elapsed += TimerCleanUpProcessTick;
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void TimerImageCaptureTick(object sender, EventArgs e)
        {
            try
            {
                if (_ImageRecorder != null && _ImageRecorder.ProcessDetails != null &&
                    _ImageRecorder.ProcessDetails.Any())
                    CaptureApplication(_ImageRecorder);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void TimerCleanUpProcessTick(object sender, EventArgs e)
        {
            RemoveClosedProcess();
        }

        public void RemoveClosedProcess()
        {
            try
            {
                List<int> lstProcessId = new List<int>();
                if (_ImageRecorder != null && _ImageRecorder.ProcessDetails != null &&
                    _ImageRecorder.ProcessDetails.Any())
                {
                    List<Process> lstProcess = Process.GetProcesses().ToList();
                    List<ProcessDetails> lstClosedProcess = _ImageRecorder.ProcessDetails.GroupJoin
                                            (lstProcess,
                                            tempproc => tempproc.ProcessId,
                                            proc => proc.Id,
                                            (tempproc, proc) => new { TempProcesss = tempproc, AllProcess = proc })
                                            .Where(proc => proc.AllProcess == null || proc.AllProcess.FirstOrDefault() == null || proc.AllProcess.FirstOrDefault().Id == 0)
                                            .Select(proc => proc.TempProcesss).ToList();

                    _ImageRecorder.ProcessDetails = _ImageRecorder.ProcessDetails.Except(lstClosedProcess).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        #endregion

        #region Process Watcher
        private void AddProcessWatcher(int processId)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                   // WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace");// WHERE ProcessID = " + processId + " OR ParentProcessID = " + processId);
                    WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_Process'");
                    _StartWatch = new ManagementEventWatcher(query);
                    _StartWatch.EventArrived += new EventArrivedEventHandler(ProcessStartCallBack);
                    _StartWatch.Start();
                });
                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void StopProcessWatcher()
        {
            if (_StartWatch != null)
                _StartWatch.Stop();
        }

        public void ProcessStartCallBack(object sender, EventArrivedEventArgs e)
        {
            try
            {
                //int parentProcessId = Convert.ToInt32(e.NewEvent.Properties["ParentProcessId"].Value);
                ManagementBaseObject target = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
                int processId = Convert.ToInt32(target["ProcessId"]);
                int parentProcessId = GetParentProcess(processId);

                var lstProcess = _ImageRecorder.ProcessDetails.Where(proc => proc.ParentProcessId == parentProcessId);
                if (lstProcess.Any())
                {
                    //int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
                    List<ProcessDetails> lstProcessDetails = new List<ProcessDetails> {
                        new ProcessDetails() {
                            ParentProcessId = parentProcessId,
                            ProcessId = processId,
                            SessionId = lstProcess.FirstOrDefault().SessionId
                        }
                    };
                    AddProcessDetails(lstProcessDetails);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private int GetParentProcess(int processId)
        {
            int parentPid = 0;
            try
            {
                string query = "SELECT * FROM Win32_Process  WHERE ProcessId= '" + processId.ToString() + "'";
                ManagementObjectSearcher objManagementObjectSearcher = new ManagementObjectSearcher(new ObjectQuery(query));
                var lstResult = objManagementObjectSearcher.Get();
                if (lstResult != null && lstResult.Count > 0)
                {
                    foreach (ManagementObject result in lstResult)
                        parentPid = Convert.ToInt32(result["ParentProcessId"]);
                }
            }
            catch (Exception ex) { _Log.Error(ex); }
            return parentPid;
        }

        public void AddProcessDetails(List<ProcessDetails> lstProcessDetails)
        {
            try
            {
                if (lstProcessDetails != null && lstProcessDetails.Any())
                {
                    if (_ImageRecorder != null && _ImageRecorder.ProcessDetails != null)
                    {
                        _ImageRecorder.ProcessDetails.AddRange(lstProcessDetails);
                        _ImageRecorder.ProcessDetails = _ImageRecorder.ProcessDetails.Distinct().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
        #endregion

        #region Image Processing
        public void CaptureApplication(ImageRecorder objImageRecorder)
        {
            _Log.Info("CaptureApplication method Started");
            try
            {
                Bitmap objBitmap = null;
                _ImageRecorder = objImageRecorder;
                _ImageRecorder.LastWindowHandle = User32APIManager.GetForegroundWindow();
                objBitmap = CaptureWindowImage(_ImageRecorder.LastWindowHandle);
                if (objBitmap != null)
                    Task.Run(() => SaveImage(_ImageRecorder.LastWindowHandle, objBitmap));
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
            _Log.Info("CaptureApplication method Ended");
        }

        public void SaveImage(IntPtr handle, Bitmap objBitmap)
        {
            _Log.Info("SaveImage method Started");
            try
            {
                if (objBitmap != null)
                {
                    Process objProcess = User32APIManager.GetProcessDetailByHandle(handle);
                    if (_ImageRecorder.ProcessDetails != null && _ImageRecorder.ProcessDetails.Where(proc => proc.ProcessId == objProcess.Id).Any())
                    {
                        var processDetails = _ImageRecorder.ProcessDetails.Where(proc => proc.ProcessId == objProcess.Id).FirstOrDefault();
                        if (processDetails != null && processDetails.ParentProcessId > 0)
                        {
                            if (_ImageRecorder.ImageStorageType == ImageStorageType.Database)
                            {
                                ProxyRASyn objProxyRASyn = new ProxyRASyn(APIConfig);
                                objProxyRASyn.GetImgCapturedDet(objBitmap, processDetails.SessionId);
                            }
                            else if (_ImageRecorder.ImageStorageType == ImageStorageType.FileSystem)
                            {
                                string folderPath = _ImageRecorder.ImagePath;
                                string fileName = string.Empty;
                                var parentProcess = Process.GetProcessById(processDetails.ParentProcessId);
                                folderPath += parentProcess.ProcessName + "_" + processDetails.SessionId;
                                if (!Directory.Exists(folderPath))
                                    Directory.CreateDirectory(folderPath);

                                fileName = folderPath + "\\" + objProcess.ProcessName + "_" + Helper.GetFileName() + ".png";
                                objBitmap.Save(fileName, _ImageRecorder.ImageFormat);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
            _Log.Info("SaveImage method Started");
        }

        public Bitmap CaptureWindowImage(IntPtr handle)
        {
            Bitmap objBitmap = null;
            try
            {
                var rect = new User32.Rect();
                User32APIManager.GetWindowRect(handle, ref rect);
                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;
                if (width > 0 || height > 0)
                {
                    objBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    Graphics graphics = Graphics.FromImage(objBitmap);
                    graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
            return objBitmap;
        }

        #endregion

        ~ImageRecorderManager()
        {
            StopProcessWatcher();
        }

        public void InvokeImageCapture(ImageRecorder objImgRecorder)
        {
            try
            {
                if (objImgRecorder != null)
                {
                    _ImageRecorder = objImgRecorder;
                    SetupTimer();
                    if (_ImageRecorder.ProcessDetails != null && _ImageRecorder.ProcessDetails.Any())
                    {
                        int parentProcessId = _ImageRecorder.ProcessDetails.FirstOrDefault().ParentProcessId;
                        AddProcessWatcher(parentProcessId);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

    }
}
