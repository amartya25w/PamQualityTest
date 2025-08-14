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

namespace ArconImageRecorder
{
    public class ImageRecorderManager : IDisposable
    {
        #region Variables
        private static System.Timers.Timer _TmrImageCapture, _TmrCleanUpProcess;
        private ImageRecorder _ImageRecorder;
        private ManagementEventWatcher _StartWatch;
        private int _CleanUpInterval = 500;
        public APIConfig APIConfig;
        private readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<int, int> lstKVPSessionImageDetails = new Dictionary<int, int>();
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

                string processName = Convert.ToString(target["Name"]);
                if (_ImageRecorder.ProcessName != null && !string.IsNullOrEmpty(processName))
                {
                    lstProcess = _ImageRecorder.ProcessName.Where(proc => proc.AdditionalData.IndexOf(processName.Replace(".exe",string.Empty)) >= 0);
                    if (lstProcess != null && lstProcess.Any())
                    {
                        List<ProcessDetails> lstProcessDetails = new List<ProcessDetails> {
                            new ProcessDetails() {
                                ParentProcessId = lstProcess.FirstOrDefault().ParentProcessId,
                                ProcessId = processId,
                                SessionId = lstProcess.FirstOrDefault().SessionId
                            }
                        };
                        AddProcessDetails(lstProcessDetails);
                    }
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
                            var arrImage = ImageRecorderHelper.ConvertImageToByteArray(objBitmap);
                            if (processDetails.LastCapturedImage != null &&
                                (processDetails.LastCapturedImage.Length == arrImage.Length))
                                return;

                            var imageCounter = GetLastImageCounter(processDetails.SessionId);
                            if (_ImageRecorder.ImageStorageType == ImageStorageType.Database)
                            {
                                ProxyRASyn objProxyRASyn = new ProxyRASyn(APIConfig);
                                objProxyRASyn.GetImgCapturedDet(arrImage, processDetails.SessionId);
                            }
                            else if (_ImageRecorder.ImageStorageType == ImageStorageType.FileSystem)
                            {
                                string folderPath = _ImageRecorder.ImagePath;
                                string fileName = string.Empty;
                                var parentProcess = Process.GetProcessById(processDetails.ParentProcessId);
                                folderPath += parentProcess.ProcessName + "_" + processDetails.SessionId;
                                if (!Directory.Exists(folderPath))
                                    Directory.CreateDirectory(folderPath);

                                fileName = folderPath + "\\" + objProcess.ProcessName + "_" + imageCounter + "_" + ImageRecorderHelper.GetFileName() + "." + _ImageRecorder.ImageFormat;

                                EncoderParameters encoderParameters = new EncoderParameters();
                                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, _ImageRecorder.CompressionQualityLevel ?? 100);
                                objBitmap.Save(fileName, GetEncoder(ImageFormat.Jpeg), encoderParameters);
                            }
                            processDetails.LastCapturedImage = arrImage;
                            if (_ImageRecorder.KeepImagesInMemory)
                                Task.Run(() => { AddImagesToList(arrImage, processDetails.SessionId, imageCounter); });
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

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Where(data => data.FormatID == format.Guid).FirstOrDefault();
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

        public int GetLastImageCounter(int sessionId)
        {
            try
            {
                int lastImageCounter = 0;
                if (lstKVPSessionImageDetails.Any(data => data.Key == sessionId))
                {
                    var tempResult = lstKVPSessionImageDetails.Where(data => data.Key == sessionId).FirstOrDefault();
                    lastImageCounter = tempResult.Value;
                    lstKVPSessionImageDetails[sessionId] = lastImageCounter + 1;
                }
                else
                    lstKVPSessionImageDetails.Add(sessionId, lastImageCounter + 1);
                return lastImageCounter;
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        private void AddImagesToList(byte[] arrImage, int sessionId, int imageCounter)
        {
            try
            {
                var startImageCache = ImageRecorderHelper.GetDataFromCacheByKey("RTSMStartedFor_" + sessionId, string.Empty);
                if (startImageCache != null && Convert.ToBoolean(startImageCache))
                {
                    string imageData = Convert.ToBase64String(arrImage);
                    var lastKey = sessionId + "_" + imageCounter;
                    ImageRecorderHelper.SetDataFromCacheByKey(lastKey, imageData, DateTime.Now.AddHours(1), false, string.Empty);
                    ImageRecorderHelper.SetDataFromCacheByKey("Image_" + sessionId.ToString(), lastKey, DateTime.Now.AddHours(1), true, string.Empty);
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
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

        void IDisposable.Dispose()
        {
            if (_StartWatch != null)
                _StartWatch.Dispose();
        }
    }
}
