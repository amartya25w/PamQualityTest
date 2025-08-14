using ScreenCapturev2;
using ArconWinNativeAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.ComponentModel;
using System.Management;
using ArconRPA;
using ArconImageRecorder;
using ArconAPIUtility;
using System.Drawing.Imaging;
using System.Reflection;
using log4net;
using System.Security;
using System.Threading;

namespace ArconConnector.BusinessLayer
{
    public class MSSQLManager : ExeConnectorManager
    {

        string currentFocus = null;
        CaptureProcess cs = new CaptureProcess();
        private ImageRecorder _ImageRecorder;
        public APIConfig APIConfig;
        private Dictionary<int, int> lstKVPSessionImageDetails = new Dictionary<int, int>();
        private string _LogCode = "ECM", _MethodName = string.Empty;
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        bool hasprocess = false;
        //string bucketpath = @"C:\Users\varun.rathi\Desktop\Bucket\";
        //Randomly generate a processid to segregate between different scripts.
        //static int processid = 0;
        bool hasdirectory = false;
        //private void createBucket()
        //{
        //    while(!hasdirectory)
        //    {
        //        if (!Directory.Exists(bucketpath + processid.ToString()))
        //        {
        //            Directory.CreateDirectory(bucketpath + processid.ToString());
        //            hasdirectory = true;
        //        }
        //        else 
        //        {
        //            processid += 1;
        //        }
        //    }
        //}

        private int GetParentProcess(int processId)
        {
            _LogCode = "MSSQLM:0001";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
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
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
            return parentPid;
        }

        public override void PostSSOCallback(Process objProcess)
        {
            _LogCode = "MSSQLM:0002";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if (objBaseConnector.ServiceDetails.SettingParameter != null &&
                objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "VER" && Convert.ToDecimal(sett.Value) > 12))
                    SetCustomTitle();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public override void LaunchConnector()
        {
            _LogCode = "MSSQLM:0003";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                objExeConnector.ExeParameter = ExeHelper.GetExeParameter(objBaseConnector, ArconContext);

                var processes = Process.GetProcessesByName("Ssms");
                if (processes.Length > 0)
                {
                    foreach (Process proc in processes)
                    {
                        int parentprocessid = GetParentProcess(proc.Id);
                        try
                        {
                            //if (Process.GetProcessById(parentprocessid).ProcessName == "explorer")
                            if (Process.GetProcessById(parentprocessid).ProcessName == "ArconConnector.ExeType")
                            {
                                //proc.HasExited.Exited += new EventHandler(ProcessExited);
                                objBaseConnector.ProcessDetails = proc;
                                SetWindowTitle();
                                hasprocess = true;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                    }
                }
                if (!hasprocess)
                {
                    Process objProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo(objExeConnector.ExePath, objExeConnector.ExeParameter),
                        EnableRaisingEvents = true
                    };
                    if (objExeConnector.RunAsDifferentUser)
                    {
                        objProcess.StartInfo.UseShellExecute = false;
                        objProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(objExeConnector.ExePath);
                        objProcess.StartInfo.Verb = "runas";
                        objProcess.StartInfo.LoadUserProfile = true;
                        objProcess.StartInfo.Domain = objBaseConnector.ServiceDetails.DomainName;
                        objProcess.StartInfo.UserName = objBaseConnector.ServiceDetails.UserName;
                        objProcess.StartInfo.Password = GetSecureString();
                        objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    }
                    objProcess.Exited += new EventHandler(ProcessExited);
                    objProcess.Start();
                    objBaseConnector.ProcessDetails = objProcess;
                    SetWindowTitle();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void SetWindowTitle()
        {
            _LogCode = "MSSQLM:0004";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                var objProcess = objBaseConnector.ProcessDetails;
                do
                {
                    Thread.Sleep(2000);
                    objProcess.Refresh();
                }
                while (objProcess.MainWindowHandle == IntPtr.Zero);
                User32APIManager.SetWindowText(objProcess.MainWindowHandle, objExeConnector.WindowTitle);
                if (objBaseConnector.ServiceDetails.SettingParameter != null &&
                    objBaseConnector.ServiceDetails.SettingParameter.Any(sett => sett.Key == "SetCustomTitle" && Convert.ToBoolean(sett.Value)))
                    SetCustomTitle();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }
        private void ProcessExited(object sender, EventArgs e)
        {
            _LogCode = "MSSQLM:0005";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");

            TerminateProcess();

            _Log.Info(_MethodName + " Method Ended");
        }

        private SecureString GetSecureString()
        {
            _LogCode = "MSSQLM:0006";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                SecureString secureString = new SecureString();
                foreach (char c in objBaseConnector.ServiceDetails.Password)
                {
                    secureString.AppendChar(c);
                }
                return secureString;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
        }

        public override void InitiateSSO()
        {
            _LogCode = "MSSQLM:0007";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                //Write("Blocking input");
                User32APIManager.BlockInput(true);
                //Write("Blocked ip succesfully");
                //Write("Sleeping 5  seconds");
                Thread.Sleep(5000);
                //Write("Sleeping 5 seconds succesful");
                //Write("Calling FindWindow");
                IntPtr iHandle = User32APIManager.FindWindow(null, "Connect to Server");
                //Write("Called FindWindow succesfully");
                int coutRetry = 0;
                //Write("Starting while loop");
                while ((int)iHandle == 0 && coutRetry <= 20)
                {
                    Thread.Sleep(1000);
                    iHandle = User32APIManager.FindWindow(null, "Connect to Server");
                    coutRetry++;
                    if (hasprocess)
                        break;
                }
                //Write("Ended While loop succesful");
                //Write("Setforeground window and send escape keys");
                User32APIManager.SetForegroundWindow(iHandle);
                SendKeys.SendWait("{ESC}");
                SendKeys.SendWait("{ESC}");
                SendKeys.SendWait("{ESC}");
                //Write("Setforeground window and send escape keys calls succesfully");
                //Write("Calling InitiateSSO");
                base.InitiateSSO();
                //Write("Calling InitiateSSO successful");
                //Write("Unblocking input");
                User32APIManager.BlockInput(false);
                //Write("Unblocking input succesful");
            }
            catch (Exception ex)
            {
                User32APIManager.BlockInput(false);
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }



        public void SubscribeToTimerChange()
        {
            _LogCode = "MSSQLM:0008";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                System.Windows.Forms.Timer snapshotTimer = new System.Windows.Forms.Timer();
                snapshotTimer.Interval = objBaseConnector.ImageRecorder.TimeInterval;
                snapshotTimer.Tick += new EventHandler(TimerEventProcessor);
                snapshotTimer.Start();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public override void InitialiseVideoRecording()
        {
            _LogCode = "MSSQLM:0009";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");

            SubscribeToTimerChange();

            _Log.Info(_MethodName + " Method Ended");
        }


        private void TimerEventProcessor(object src, EventArgs e)
        {
            _LogCode = "MSSQLM:00010";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if (objBaseConnector.ProcessDetails.HasExited)
                {
                    TerminateProcess();
                    return;
                }
                AutomationElement element = null;
                element = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ProcessIdProperty, objBaseConnector.ProcessDetails.Id));
                if (element != null)
                {
                    try
                    {
                        if (Process.GetProcessById(element.Current.ProcessId).ProcessName == "Ssms" && !element.Current.IsOffscreen)
                        {

                            string tempCurrentFocus = null;
                            var tempElement = element;
                            TreeWalker tWalker = TreeWalker.ControlViewWalker;
                            while (!tempElement.Current.Name.Contains(".sql"))
                            {
                                tempElement = tWalker.GetParent(tempElement);
                            }

                            tempCurrentFocus = tempElement.Current.AutomationId + tempElement.Current.Name;
                            //The below two lines remove the file name from the tab name.
                            int fIndex = tempCurrentFocus.LastIndexOf(".sql") + 7;
                            tempCurrentFocus = tempCurrentFocus.Substring(fIndex - 1, tempCurrentFocus.Length - fIndex);
                            //The below two lines remove  'Microsoft SQL Server' from the tab name

                            int lIndex = tempCurrentFocus.LastIndexOf("-");
                            tempCurrentFocus = tempCurrentFocus.Remove(lIndex, tempCurrentFocus.Length - lIndex);
                            //The below two lines remove the connection number and the asterisk from the unsaved file.
                            int lIndex2 = tempCurrentFocus.LastIndexOf("(");
                            tempCurrentFocus = tempCurrentFocus.Remove(lIndex2, tempCurrentFocus.Length - lIndex2);
                            var tempCurrentFocusArr = tempCurrentFocus.Split('(');
                            var username = tempCurrentFocusArr[1];
                            int lIndex3 = tempCurrentFocusArr[0].LastIndexOf(".");
                            tempCurrentFocus = tempCurrentFocusArr[0].Remove(lIndex3, tempCurrentFocusArr[0].Length - lIndex3);
                            //tempCurrentFocus = tempCurrentFocus + username;
                            CaptureMyScreen(tempCurrentFocus, username);
                        }
                    }
                    catch (Exception x)
                    {
                        if (currentFocus == null)
                            CaptureMyScreen("Other", "Other");
                        else
                            CaptureMyScreen(currentFocus.Trim(), "Other");
                    }

                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public void AddProcessDetails(List<ProcessDetails> lstProcessDetails)
        {
            _LogCode = "MSSQLM:0011";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
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
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }


        private void CaptureMyScreen(string tempCurrentFocus, string username)
        {
            _LogCode = "MSSQLM:0012";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if ((tempCurrentFocus.Trim().Equals(objBaseConnector.ServiceDetails.IPAddress + @"," + objBaseConnector.ServiceDetails.Port) && username.Trim().Equals(objBaseConnector.ServiceDetails.UserName)) || tempCurrentFocus.Trim().Equals("Other"))
                //if (true)
                {
                    bool watermarkflag = false;
                    cs.addProcess(objBaseConnector.ProcessDetails.Id);
                    Bitmap bmp = cs.CaptureDesktop();

                    // debugger image code - Saves image on desktop
                    //string path = Application.ExecutablePath + tempCurrentFocus + username;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Bucket\" + objBaseConnector.ParameterDetails.SessionId.ToString() + @"\" + tempCurrentFocus;
                    //string path = @"C:\Users\varun.rathi\Desktop\Bucket\" + objBaseConnector.ParameterDetails.SessionId.ToString()+@"\"+tempCurrentFocus;
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    //createBucket();
                    // debugger image code - Saves image on desktop
                    if (bmp != null)
                    {

                        string imgpath = path + @"\" + DateTime.Now.ToString().Replace(':', '_') + @".jpg";
                        //string destpath = path + @"\" + DateTime.Now.ToString().Replace(':', '_') + @"_w.jpg"; ;
                        // debugger image code - Saves image on desktop


                        // debugger image code - Saves image on desktop
                        if (tempCurrentFocus.Equals("Other"))
                        {
                            //Call captureApplication in watermarkImage function
                            //watermarkImage(imgpath, "Arcon TechSolutions", destpath, ImageFormat.Jpeg);
                            //System.IO.File.Delete(imgpath);
                            watermarkflag = true;
                            CaptureApplication(objBaseConnector.ImageRecorder, watermarkflag);
                        }
                        else
                        {
                          //bmp.Save(imgpath, ImageFormat.Jpeg);
                            CaptureApplication(objBaseConnector.ImageRecorder, watermarkflag);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                //CaptureMyScreen("CaptureMyScreen", "Other");
            }
            _Log.Info(_MethodName + " Method Ended");
        }
        public void SaveImage(IntPtr handle, Bitmap objBitmap)
        {
            _LogCode = "MSSQLM:0013";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                if (objBitmap != null)
                {
                    Process objProcess = User32APIManager.GetProcessDetailByHandle(handle);
                    var arrImage = ImageRecorderHelper.ConvertImageToByteArray(objBitmap);
                    var imageCounter = GetLastImageCounter(objBaseConnector.UserDetails.SessionId);
                    if (_ImageRecorder.ImageStorageType == ImageStorageType.Database)
                    {
                        ProxyRASyn objProxyRASyn = new ProxyRASyn(ArconContext.APIConfig);
                        objProxyRASyn.GetImgCapturedDet(arrImage, objBaseConnector.UserDetails.SessionId);
                    }
                    if (_ImageRecorder.KeepImagesInMemory)
                        Task.Run(() => { AddImagesToList(arrImage, objBaseConnector.UserDetails.SessionId, imageCounter); });
                }
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public int GetLastImageCounter(int sessionId)
        {
            _LogCode = "MSSQLM:0014";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
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
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            //_Log.Info(_MethodName + " Method Ended");
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            _LogCode = "MSSQLM:0015";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                return codecs.Where(data => data.FormatID == format.Guid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        private void AddImagesToList(byte[] arrImage, int sessionId, int imageCounter)
        {
            _LogCode = "MSSQLM:0016";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
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
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public void CaptureApplication(ImageRecorder objImageRecorder, bool watermarkflag)
        {
            _LogCode = "MSSQLM:0017";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                Bitmap objBitmap = null;
                _ImageRecorder = objImageRecorder;
                _ImageRecorder.LastWindowHandle = User32APIManager.GetForegroundWindow();
                objBitmap = CaptureWindowImage(_ImageRecorder.LastWindowHandle);
                if (objBitmap != null)
                {
                    if (watermarkflag)
                    {
                        objBitmap = WatermarkImage(objBitmap, objBaseConnector.ParameterDetails.SessionId);
                    }
                    Task.Run(() => SaveImage(_ImageRecorder.LastWindowHandle, objBitmap));
                    //objBitmap.Dispose();
                }

            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode, ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public Bitmap CaptureWindowImage(IntPtr handle)
        {
            _LogCode = "MSSQLM:0018";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
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
                _Log.Error(_LogCode, ex);
                throw ex;

            }
            _Log.Info(_MethodName + " Method Ended");
            return objBitmap;
        }

        //public static void watermarkImage(string sourceImage, string text, string targetImage, ImageFormat fmt)
        //{
        //    try
        //    {
        //        // open source image as stream and create a memorystream for output
        //        FileStream source = new FileStream(sourceImage, FileMode.Open);
        //        Stream output = new MemoryStream();
        //        Image img = Image.FromStream(source);

        //        // choose font for text
        //        Font font = new Font("Helvetica", 50, FontStyle.Bold, GraphicsUnit.Pixel);
        //        //choose color and transparency
        //        Color color = Color.FromArgb(100, 255, 0, 0);
        //        //Color color = Color.FromArgb(100, 0, 0, 0);

        //        //location of the watermark text in the parent image
        //        Point pt = new Point(0, 0);
        //        SolidBrush brush = new SolidBrush(color);

        //        //draw text on image
        //        Graphics graphics = Graphics.FromImage(img);
        //        graphics.DrawString(text, font, brush, pt);
        //        graphics.Dispose();

        //        //update image memorystream
        //        img.Save(output, fmt);
        //        Image imgFinal = Image.FromStream(output);

        //        //write modified image to file
        //        Bitmap bmp = new System.Drawing.Bitmap(img.Width, img.Height, img.PixelFormat);
        //        Graphics graphics2 = Graphics.FromImage(bmp);
        //        graphics2.DrawImage(imgFinal, new Point(0, 0));

        //        bmp.Save(targetImage, fmt);
        //        imgFinal.Dispose();
        //        img.Dispose();
        //        source.Dispose();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        public static Bitmap WatermarkImage(Bitmap bmp, int sessionid)
        {
            Bitmap bmp2 = null;
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Bucket\" + sessionid.ToString() + @"\Other";
                //string path = @"C:\Users\varun.rathi\Desktop\Bucket\"+sessionid.ToString() +@"\Other";
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //string path = @"C:\Users\varun.rathi\Desktop\Bucket\" + @"Other";
                string imgpath = path + @"\" + DateTime.Now.ToString().Replace(':', '_') + @".jpg";
                Font font = new Font("Helvetica", 50, FontStyle.Bold, GraphicsUnit.Pixel);
                Color color = Color.FromArgb(100, 255, 0, 0);
                Point pt = new Point(0, 0);
                SolidBrush brush = new SolidBrush(color);
                Image img = (Image)bmp;
                Graphics graphics = Graphics.FromImage(img);
                graphics.DrawString("Arcon", font, brush, pt);
                bmp2 = (Bitmap)img;
                //bmp2.Save(imgpath, ImageFormat.Jpeg);
                //img.Dispose();
                //graphics.Dispose();
            }
            catch (Exception ex)
            {
                //Write(ex.Message); 
            }
            return bmp2;
        }

        //public static void CreateLogDirectory()
        //{
        //    string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        //    string logdirpath = @"C:\Users\varun.rathi\Desktop\SSMSLogs\";
        //    //var directory = System.IO.Path.GetDirectoryName(path);
        //    if (Directory.Exists(logdirpath + "/LogFiles") == false)
        //    {
        //        System.IO.Directory.CreateDirectory(logdirpath + "/LogFiles");
        //    }
        //    var directorypath = logdirpath + "/LogFiles//";

        //}
        //public static void Write(string logMessage)
        //{
        //    string logdirpath = @"C:\Users\varun.rathi\Desktop\SSMSLogs\";
        //    var directorypath = logdirpath + "/LogFiles//";
        //    CreateLogDirectory();
        //    using (StreamWriter w = File.AppendText(directorypath + @"log.txt"))
        //    {
        //        w.Write("\r\nLog Entry : " + $"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
        //        w.WriteLine("  :" + $"  :{logMessage}" + "\n-------------------------------");
        //    }
        //}
    }
}