using JavaAutomation2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Windows.Threading;
using ArconRPAAutoIT;
using System.Management;

namespace ArconRPA
{
    public static class ProcessExtensions
    {
        public static List<Process> GetChildProcesses(this Process process)
        {
            List<Process> children = new List<Process>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));

            foreach (ManagementObject mo in mos.Get())
            {
                children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
            }

            return children;
        }
    }

    public class StepExecutor
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public IntPtr appWin;
        public int image_no = 0;
        static bool _first = true;

        public Process objProcess;
        List<int> processIds = new List<int>();

        List<int> _processList;
        

        public List<int> processList
        {
            get
            {
                return _processList;
            }
            set
            {
                _processList = value;
            }
        }

        private void checkProcess(int processID)
        {
            if (!processIds.Contains(processID))
            {
                processIds.Add(processID);
            }

            var process = Process.GetProcessById(processID).GetChildProcesses();
            for (int countProcess = 0; countProcess < process.Count; countProcess++)
            {
                if ((Int32)appWin == 0)
                    appWin = process[countProcess].MainWindowHandle;
                checkProcess(process[countProcess].Id);
            }
        }

        public void ExecuteSteps(List<Dictionary<string, dynamic>> JsonObjectData, Process a)
        {
            IntPtr processid;

            try
            {

                Logger.Log.Info("Inside Executor");

                Logger.Log.Info("Creating instance of common function");

                Logger.Log.Info("Creating instance of common function - Completed");

                List<string> input_text_list = new List<string>();
                List<string> input_text_list_element_name = new List<string>();
                Process p = a;
                Logger.Log.Info("Starting for loop");
                WindowHelper.BringProcessToFront(p);
                if (JsonObjectData.Count > 0) { Logger.Log.Info("json object count is - " + JsonObjectData.Count); }

                for (int i = 0; i < JsonObjectData.Count; i++)
                {
                    Logger.Log.Info("Inside for loop");
                    //if (JsonObjectData[i]["isAutoIT"] && !(JsonObjectData[i]["actionType"] == "Select")) // condition Automation UI   i.e. JsonObjectData[step]["Key"]
                    //if (JsonObjectData[i]["isAutoIT"])
                    if(false)
                    {
                        // AutoIT Automation UI
                        ExecuteStepAutoIT(JsonObjectData, a, i);
                        //ExecuteStep(JsonObjectData, a, i);
                    }
                    else
                    {
                        ExecuteStep(JsonObjectData, a, i);

                    }
                }
                //User32APIManager.BlockInput(false);
            }
            catch (Exception ex)
            {

                Logger.Log.Info("Inside executor catch Block");
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public void ExecuteSteps(Process p, string WindowName,
            string processName,
            string identifierTypeId,
            string identifierTypeRoleName,
            string identifierType,
            string identifierTypeName,
            string identifierTypeClassName,
            string shortCut,
            string image_score,
            string identifierTypeImage,
            string actionType,
            string state,
            string waittime,
            string text,
            string keymodifier,
            bool ismodifier,
            bool isWait,
            bool isAutoIT,
            bool settings)
        {

            //"exe_name": "GO-Global UNIX Client",
            //"exe_path": "D:\\ARCON Solutions\\Thick Clients\\GO-Global-UX_Client_2.2.15.1125\\GO-Global for UNIX v2.2\\goglobal_ux.exe",
            //"WindowName": "Connection",
            //"processName": "goglobal_ux",
            //"exeargument": "",
            //"identifierTypeId": "1001",
            //"identifierTypeRoleName": "",
            //"identifierType": "Other",
            //"identifierTypeName": "Server Address:",
            //"identifierTypeClassName": "Edit[0]",
            //"shortCut": "Alt+S",
            //"image_score": "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAAQAKUDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD1+z/5KHrP/YKsP/Rt3XQVj21jcR+MtTv2jxazafaQxvuHzOklyWGOvAkT8/Y1sUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFAH//Z",
            //"identifierTypeImage": "",
            //"actionType": "InputSend",
            //"state": "",
            //"waittime": "2",
            //"text": "172.16.130.6",
            //"keymodifier": "",
            //"ismodifier": false,
            //"isWait": true,
            //"isAutoIT": false,
            //"settings": null


            List<Dictionary<string, dynamic>> JsonObjectData = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> tempObj = new Dictionary<string, dynamic>();
            tempObj.Add("WindowName", WindowName);
            tempObj.Add("processName", processName);
            tempObj.Add("identifierTypeId", identifierTypeId);
            tempObj.Add("identifierTypeRoleName", identifierTypeRoleName);
            tempObj.Add("identifierType", identifierType);
            tempObj.Add("identifierTypeName", identifierTypeName);
            tempObj.Add("identifierTypeClassName", identifierTypeClassName);
            tempObj.Add("shortCut", shortCut);
            tempObj.Add("image_score", image_score);
            tempObj.Add("identifierTypeImage", identifierTypeImage);
            tempObj.Add("actionType", actionType);
            tempObj.Add("state", state);
            tempObj.Add("waittime", waittime);
            tempObj.Add("text", text);
            tempObj.Add("keymodifier", keymodifier);
            tempObj.Add("ismodifier", ismodifier);
            tempObj.Add("isWait", isWait);
            tempObj.Add("isAutoIT", isAutoIT);
            tempObj.Add("settings", settings);
            JsonObjectData.Add(tempObj);
            if (isAutoIT)
                ExecuteStepAutoIT(JsonObjectData, p, 0);
            else
                ExecuteStep(JsonObjectData, p, 0);
        }

        public void ExecuteStepAutoIT(List<Dictionary<string, dynamic>> JsonObjectData, Process p, int step)
        {
            CommonFunction objCommonFunction = new CommonFunction();
            List<string> input_text_list = new List<string>();
            List<string> input_text_list_element_name = new List<string>();
            int step_no = 0;

            var identifierType = string.Empty;
            if (JsonObjectData[step].Any(data => data.Key == "identifierType"))
                identifierType = JsonObjectData[step]["identifierType"];

            #region Auto IT Automation Element
            //if (identifierType != "java" && (JsonObjectData[step]["identifierTypeId"] != "" || JsonObjectData[step]["identifierTypeName"] != "" || JsonObjectData[step]["identifierTypeClassName"] != ""))
            if ((JsonObjectData[step]["identifierTypeId"] != "" || JsonObjectData[step]["identifierTypeName"] != "" || JsonObjectData[step]["identifierTypeClassName"] != ""))
            {
                Logger.Log.Info("Inside AutoIT Automation Element");
                //WindowHelper.BringProcessToFront(p);

                if (JsonObjectData[step]["actionType"] == "Input")
                {
                    ArconRPAAutoIT.ArconAutoIT.PutTextField(JsonObjectData[step]["processName"], JsonObjectData[step]["WindowName"], "ID:" + JsonObjectData[step]["identifierTypeId"] + ";" + "CLASS:" + JsonObjectData[step]["identifierTypeClassName"], JsonObjectData[step]["text"]);
                }
                else if (JsonObjectData[step]["actionType"] == "Select")
                {
                    ArconRPAAutoIT.ArconAutoIT.SelectComboBoxField(JsonObjectData[step]["processName"], JsonObjectData[step]["WindowName"], "ID:" + JsonObjectData[step]["identifierTypeId"] + ";" + "CLASS:" + JsonObjectData[step]["identifierTypeClassName"], JsonObjectData[step]["identifierTypeName"]);
                }
                else if (JsonObjectData[step]["actionType"] == "Click")
                {
                    ArconRPAAutoIT.ArconAutoIT.PressButtonField(JsonObjectData[step]["processName"], JsonObjectData[step]["WindowName"], "ID:" + JsonObjectData[step]["identifierTypeId"] + ";" + "CLASS:" + JsonObjectData[step]["identifierTypeClassName"]);
                }


            }




            #endregion


        }


        public void ExecuteStep(List<Dictionary<string, dynamic>> JsonObjectData, Process p, int step)
        {
            CommonFunction objCommonFunction = new CommonFunction();
            List<string> input_text_list = new List<string>();
            List<string> input_text_list_element_name = new List<string>();
            int step_no = 0;

            #region Dynamic wait
            //if (JsonObjectData[step]["isWait"])
            if (true)
            {
                try
                {
                    int waittime = Convert.ToInt32(JsonObjectData[step]["waittime"]) * 1000;
                    Logger.Log.Info("Dynamic wait - " + Convert.ToString(waittime));
                    Thread.Sleep(waittime);
                    //WindowHelper.BringProcessToFront(p);
                    //ArconRPAAutoIT.ArconAutoIT.WinWaitActive(JsonObjectData[step]["WindowName"]);
                }
                catch (Exception ex)
                {
                    Logger.Log.Info("Inside Dynamic Wait");
                    Logger.Log.Error(ex.Message, ex);
                    throw ex;
                }
            }
            #endregion



            var identifierType = string.Empty;
            if (JsonObjectData[step].Any(data => data.Key == "identifierType"))
                identifierType = JsonObjectData[step]["identifierType"];

            #region Automation Element
            if (!identifierType.Equals("java",StringComparison.OrdinalIgnoreCase) && (JsonObjectData[step]["identifierTypeId"] != "" || JsonObjectData[step]["identifierTypeName"] != "" || JsonObjectData[step]["identifierTypeClassName"] != ""))
            {
                Logger.Log.Info("Inside Automation Element");
                //WindowHelper.BringProcessToFront(p);
                appWin = p.MainWindowHandle;

                for (int _loopCount = 0; appWin == IntPtr.Zero && _loopCount < 15; _loopCount++)
                {
                    appWin = p.MainWindowHandle;
                    if (appWin == IntPtr.Zero)
                        Thread.Sleep(1000);
                }

                if (appWin == IntPtr.Zero)
                {
                    //var pname = Process.GetProcessById(p.Id);
                    //if (pname.Length == 0)
                    //    //MessageBox.Show("nothing");
                    //else
                    //    //MessageBox.Show("run");
                    if (p.HasExited)
                    {
                        if(_processList!=null && _processList.Count()>0)
                        {
                            Process[] systemProcessList = Process.GetProcesses();
                            foreach (Process theprocess in systemProcessList)
                            {
                                if(_processList.Contains(theprocess.Id))
                                {
                                    if(theprocess.MainWindowHandle  != IntPtr.Zero)
                                    {
                                        p = theprocess;
                                        appWin = theprocess.MainWindowHandle;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        checkProcess(p.Id);
                    }
                }

                if (appWin == IntPtr.Zero)
                {
                    MessageBox.Show("Unable to find process in step : " + step + 1);
                    return;
                }

                AutomationElement window = null;
                while (window == null)
                {
                    window = AutomationElement.FromHandle((IntPtr)appWin);
                    if (window == null)
                        Thread.Sleep(1000);
                }
                //AutomationElementCollection elementCollection1;
                //{
                //    IntPtr windowHwd = IntPtr.Zero;

                //    while (windowHwd == IntPtr.Zero)
                //    {
                //        windowHwd = FindWindow(null, JsonObjectData[step]["WindowName"]);
                //        if (windowHwd == IntPtr.Zero)
                //            Thread.Sleep(1000);
                //    }

                //    appWin = windowHwd;
                //    int vmId = 0;
                //    IntPtr acPtr = IntPtr.Zero;

                //    AutomationElement window1 = null;
                //    while (window1 == null)
                //    {
                //        window1 = AutomationElement.FromHandle((IntPtr)windowHwd);
                //        if (window1 == null)
                //            Thread.Sleep(1000);
                //    }

                //    if (window1 != null)
                //    {
                //        int elementCount1;
                //        elementCollection1 = objCommonFunction.findelement(window1, JsonObjectData[step]["identifierTypeId"], JsonObjectData[step]["identifierTypeName"], JsonObjectData[step]["identifierTypeClassName"], out elementCount1);
                //    }
                //}


                if (window != null)
                {
                    Logger.Log.Info("Window handle capture sucess");
                    if (JsonObjectData[step]["actionType"] != "Sendkey")
                    {
                        Logger.Log.Info("Inside ActionType Not equal to sendkey");
                        int elementCount;
                        AutomationElementCollection elementCollection = objCommonFunction.findelement(window, JsonObjectData[step]["identifierTypeId"], JsonObjectData[step]["identifierTypeName"], JsonObjectData[step]["identifierTypeClassName"], out elementCount);
                        AutomationElement element = null;
                        bool done = false;

                        if (elementCollection.Count == 0)
                        {
                            appWin = IntPtr.Zero;
                            while (appWin == IntPtr.Zero)
                            {
                                appWin = FindWindow(null, JsonObjectData[step]["WindowName"]);
                                if (appWin == IntPtr.Zero)
                                    Thread.Sleep(1000);
                            }
                            window = null;
                            while (window == null)
                            {
                                window = AutomationElement.FromHandle((IntPtr)appWin);
                                if (window == null)
                                    Thread.Sleep(1000);
                            }

                            //window = AutomationElement.FromHandle((IntPtr)appWin);
                            if (window != null)
                                elementCollection = objCommonFunction.findelement(window, JsonObjectData[step]["identifierTypeId"], JsonObjectData[step]["identifierTypeName"], JsonObjectData[step]["identifierTypeClassName"], out elementCount);

                        }
                        if (elementCollection == null)
                        { }
                        else if (elementCollection.Count != 0)
                        {
                            //int co = elementCollection.Count > elementCount ? elementCount : 0;
                            //for (int co = 0; co < elementCollection.Count; co++)
                            {
                                int co = elementCollection.Count > elementCount ? elementCount : 0;
                                element = elementCollection[co];
                                if (element.Current.IsEnabled)
                                {
                                    if (JsonObjectData[step]["actionType"] == "aClick")
                                    {
                                        Logger.Log.Info("Inside actionType : aClick");
                                        done = objCommonFunction.ClickElementByCoordinates(element, "Click");
                                        if (done == true)
                                        {
                                            //break;
                                        }
                                    }
                                    if (JsonObjectData[step]["actionType"] == "Click")
                                    {
                                        Thread.Sleep(3000);
                                        Logger.Log.Info("Inside actionType : Click");
                                        done = objCommonFunction.ClickElementUIAutomation(element, step_no);
                                        if (done == true)
                                        {
                                            //break;
                                        }

                                    }

                                    else if (JsonObjectData[step]["actionType"] == "Input")
                                    {
                                        //Logger.Log.Info("Inside ActionType : Input");
                                        bool old_values = objCommonFunction.PreviouslyEntered(element.Current.Name, input_text_list);
                                        bool old_names = objCommonFunction.PreviouslyEntered(element.Current.Name, input_text_list_element_name);
                                        //Logger.Log.Info("Before IdentifierTypeName check Calling Insert Text Using UIAutomation");
                                        if (element.Current.Name == JsonObjectData[step]["identifierTypeName"] && old_values == false && old_names == false)
                                            done = objCommonFunction.InsertTextUsingUIAutomation(element, JsonObjectData[step]["text"]);
                                        if (done == true)
                                        {

                                            input_text_list.Add(JsonObjectData[step]["text"]);
                                            input_text_list_element_name.Add(element.Current.Name);
                                            //break;
                                        }



                                    }
                                    else if (JsonObjectData[step]["actionType"] == "InputSend")
                                    {
                                        //Logger.Log.Info("Inside ActionType : Input");
                                        bool old_values = objCommonFunction.PreviouslyEntered(element.Current.Name, input_text_list);
                                        bool old_names = objCommonFunction.PreviouslyEntered(element.Current.Name, input_text_list_element_name);
                                        //Logger.Log.Info("Before IdentifierTypeName check Calling Insert Text Using UIAutomation");
                                        if (element.Current.Name == JsonObjectData[step]["identifierTypeName"] && old_values == false && old_names == false)
                                            done = objCommonFunction.InsertTextUsingUIAutomation(element, JsonObjectData[step]["text"], true);
                                        if (done == true)
                                        {

                                            input_text_list.Add(JsonObjectData[step]["text"]);
                                            input_text_list_element_name.Add(element.Current.Name);
                                            //break;
                                        }



                                    }
                                    else if (JsonObjectData[step]["actionType"] == "Select")
                                    {
                                        Logger.Log.Info("Inside actionType : Select");
                                        //AutomationElementCollection elementCollection1 = findelement(window, element_data.identifierTypeId, element_data.identifierTypeName, element_data.identifierTypeClassName);

                                        done = objCommonFunction.SelectComboboxItem(element, JsonObjectData[step]["text"], step_no);
                                        if (done == true)
                                        {

                                            done = objCommonFunction.ClickElementByCoordinates(element, "DoubleClick");
                                            if (done == true)
                                            {
                                                //break;
                                            }
                                        }
                                    }
                                }
                            }


                            if (JsonObjectData[step]["actionType"] == "Input" && done == false)
                            {
                                Logger.Log.Info("Inside ActionType :  Input");
                                #region input_second_attempt

                                //for (int co = 0; co < elementCollection.Count; co++)
                                {
                                    int co = elementCollection.Count > elementCount ? elementCount : 0;
                                    element = elementCollection[co];
                                    bool old_values = objCommonFunction.PreviouslyEntered(element.Current.Name, input_text_list);
                                    bool old_names = objCommonFunction.PreviouslyEntered(element.Current.Name, input_text_list_element_name);
                                    if (old_values == false && old_names == false)

                                        done = objCommonFunction.InsertTextUsingUIAutomation(element, JsonObjectData[step]["text"]);
                                    if (done == true)
                                    {

                                        input_text_list.Add(JsonObjectData[step]["text"]);
                                        input_text_list_element_name.Add(element.Current.Name);
                                        //break;
                                    }
                                }
                                #endregion
                                if (done == false)
                                {
                                    return;
                                    //break;
                                }
                            }
                        }
                        else
                        {
                            //break;
                            return;
                        }
                    }
                }
                else
                {

                }
            }


            #endregion

            #region Image Click
            else if (JsonObjectData[step]["identifierTypeImage"] != "")
            {

                //WindowHelper.BringProcessToFront(p);
                try
                {
                    appWin = p.MainWindowHandle;
                }
                catch { }
                System.Drawing.Image screenimage = objCommonFunction.Takescreenshot();
                string largeImage = objCommonFunction.ConvertToBase64(screenimage);
                string smallImage = JsonObjectData[step]["identifierTypeImage"];

                //Bitmap image compare  
                System.Drawing.Point points = System.Drawing.Point.Empty;

                bool resultt;
                try
                {
                    //Calling Python Exe
                    //resultt = objCommonFunction.PythonEXECalling(largeImage, smallImage, out points, JsonObjectData[step]["image_score"]);
                    resultt = objCommonFunction.PythonEXECalling(largeImage, smallImage, out points, "0.7");
                }
                catch (Exception ex) { throw ex; }
                if (resultt == false)
                {
                    //Displaying captured image and small image.
                    //break;
                    return;
                }
                else
                {

                    if (JsonObjectData[step]["actionType"] == "RightClick")
                        objCommonFunction.MouseRight(points.X, points.Y);
                    else if (JsonObjectData[step]["actionType"] == "Click")
                        objCommonFunction.MouseLeft(points.X, points.Y);
                    else if (JsonObjectData[step]["actionType"] == "DoubleClick")
                        objCommonFunction.MouseDoubleClick(points.X, points.Y);
                }
            }
            #endregion

            #region IsModifier

            if (JsonObjectData[step]["actionType"].Equals("Sendkey", StringComparison.CurrentCultureIgnoreCase))
            {
                //WindowHelper.BringProcessToFront(p);
                appWin = p.MainWindowHandle;
                if (appWin == IntPtr.Zero)
                {
                    Thread.Sleep(3000);
                    appWin = p.MainWindowHandle;
                }

                IntPtr windowHwd = IntPtr.Zero;

                while (windowHwd == IntPtr.Zero)
                {
                    //Thread.Sleep(1000);
                    windowHwd = FindWindow(null, JsonObjectData[step]["WindowName"]);
                    if (windowHwd == IntPtr.Zero)
                        Thread.Sleep(1000);
                }
                //var windowHwd = FindWindow(null, JsonObjectData[step]["WindowName"]);
                appWin = windowHwd;

                System.Threading.Thread.Sleep(500);

                string keyName = JsonObjectData[step]["text"];
                SendKeys.SendWait(keyName);
                SendKeys.Flush();
                //SendKeys.SendWait(" ");
                SendKeys.Flush();
            }

            if (JsonObjectData[step]["ismodifier"])
            {
                //WindowHelper.BringProcessToFront(p);
                appWin = p.MainWindowHandle;
                if (appWin == IntPtr.Zero)
                {
                    Thread.Sleep(3000);
                    appWin = p.MainWindowHandle;
                }

                IntPtr windowHwd = IntPtr.Zero;

                while (windowHwd == IntPtr.Zero)
                {
                    //Thread.Sleep(1000);
                    windowHwd = FindWindow(null, JsonObjectData[step]["WindowName"]);
                    if (windowHwd == IntPtr.Zero)
                        Thread.Sleep(1000);
                }
                //var windowHwd = FindWindow(null, JsonObjectData[step]["WindowName"]);
                appWin = windowHwd;

                System.Threading.Thread.Sleep(200);

                string keyName = JsonObjectData[step]["keymodifier"];
                SendKeys.SendWait(keyName);
                SendKeys.Flush();
                //SendKeys.SendWait(" ");
                SendKeys.Flush();
            }
            #endregion

            #region JAVA Element 
            if (identifierType.Equals("java", StringComparison.OrdinalIgnoreCase) && (JsonObjectData[step]["identifierTypeId"] != "" || JsonObjectData[step]["identifierTypeName"] != "" || JsonObjectData[step]["identifierTypeClassName"] != ""))
            {
                if (_first)
                {
                    //Thread.Sleep(5000);
                    //JabHelpers.Windows_run();
                    //_first = false;
                }
                Logger.Log.Info("Inside JAVA Automation Element");
                //WindowHelper.BringProcessToFront(p);
                //appWin = p.MainWindowHandle;

                //if (appWin == IntPtr.Zero)
                //{
                //    Thread.Sleep(1000);
                //    appWin = p.MainWindowHandle;
                //}

                IntPtr windowHwd = IntPtr.Zero;

                while (windowHwd == IntPtr.Zero)
                {
                    windowHwd = FindWindow(null, JsonObjectData[step]["WindowName"]);
                    if (windowHwd == IntPtr.Zero)
                        Thread.Sleep(1000);
                }

                appWin = windowHwd;
                int vmId = 0;
                IntPtr acPtr = IntPtr.Zero;

                while (acPtr == IntPtr.Zero || vmId == 0)
                {

                    JabHelpers.GetAccessibleDetails(windowHwd, out vmId, out acPtr);
                    if (acPtr == IntPtr.Zero || vmId == 0)
                        Thread.Sleep(1000);
                }
                //if (windowHwd != IntPtr.Zero)
                if (appWin != IntPtr.Zero)
                {
                    Logger.Log.Info("Window handle capture sucess");
                    if (JsonObjectData[step]["actionType"] != "Sendkey")
                    {
                        Logger.Log.Info("Inside ActionType Not equal to sendkey");
                        bool done = false;
                        if (JsonObjectData[step]["actionType"].ToLower().Contains("input"))
                        {
                            Logger.Log.Info("Inside ActionType : Input");
                            //int vmId;
                            //IntPtr acPtr;
                            //JabHelpers.GetAccessibleDetails(windowHwd, out vmId, out acPtr);
                            if (vmId > 0 && acPtr != IntPtr.Zero)
                            {
                                if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeId"]))
                                {
                                    if (JsonObjectData[step]["actionType"].Equals("Input", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        done = JabHelpers.SendDataByCoordinates(vmId, acPtr, JsonObjectData[step]["identifierTypeId"], JsonObjectData[step]["text"]);
                                    }
                                    else if (JsonObjectData[step]["actionType"].Equals("Input1", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        done = JabHelpers.SendDataKeysByCoordinates(vmId, acPtr, JsonObjectData[step]["identifierTypeId"], JsonObjectData[step]["text"]);
                                    }
                                }
                                else if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeName"]))
                                    done = JabHelpers.SendData(vmId, acPtr, null, JsonObjectData[step]["identifierTypeName"], "text", JsonObjectData[step]["text"]);
                            }
                        }
                        else if (JsonObjectData[step]["actionType"].ToLower().Contains("click"))
                        {
                            //"".Contains
                            Logger.Log.Info("Inside ActionType : Click");
                            if (vmId > 0 && acPtr != IntPtr.Zero)
                            {
                                var intPtr = IntPtr.Zero;
                                if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeId"]))
                                    intPtr = JabHelpers.GetAccessibleChildFromContext(JsonObjectData[step]["identifierTypeId"], acPtr, vmId);
                                else if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeName"]) && intPtr == IntPtr.Zero)
                                    intPtr = JabHelpers.GetAccessibleChildFromContextByName(vmId, acPtr, JsonObjectData[step]["identifierTypeName"]);
                                // Thread.Sleep(3000);
                                var data = JabHelpers.GetAccessibleContextInfo(vmId, intPtr);
                                if (data.accessibleAction)
                                {
                                    if (JsonObjectData[step]["actionType"].Equals("click", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        done = JabHelpers.SendButtonClick(intPtr, vmId);
                                    }
                                    else if (JsonObjectData[step]["actionType"].Equals("click1", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        done = JabHelpers.SendMouseClick(intPtr, vmId);
                                    }
                                }

                                //Thread.Sleep(3000);
                            }
                        }
                        else if (JsonObjectData[step]["actionType"].Equals("Select", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //int vmId;
                            //IntPtr acPtr;
                            //JabHelpers.GetAccessibleDetails(windowHwd, out vmId, out acPtr);
                            Logger.Log.Info("Inside ActionType : Select");
                            if (vmId > 0 && acPtr != IntPtr.Zero)
                            {
                                var intPtr = IntPtr.Zero;
                                if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeId"]))
                                    intPtr = JabHelpers.GetAccessibleChildFromContext(JsonObjectData[step]["identifierTypeId"], acPtr, vmId);
                                if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeName"]) && intPtr == IntPtr.Zero)
                                    intPtr = JabHelpers.GetAccessibleChildFromContextByName(vmId, acPtr, JsonObjectData[step]["identifierTypeName"]);

                                var data = JabHelpers.GetAccessibleContextInfo(vmId, intPtr);
                                if (data.accessibleAction)
                                    done = JabHelpers.SelectDropDownData(intPtr, vmId, JsonObjectData[step]["text"]);
                            }
                        }
                        else if (JsonObjectData[step]["actionType"].Equals("Switch", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //int vmId;
                            //IntPtr acPtr;
                            //JabHelpers.GetAccessibleDetails(windowHwd, out vmId, out acPtr);
                            Logger.Log.Info("Inside ActionType : Switch");
                            if (vmId > 0 && acPtr != IntPtr.Zero)
                            {
                                var intPtr = IntPtr.Zero;
                                if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeId"]))
                                    intPtr = JabHelpers.GetAccessibleChildFromContext(JsonObjectData[step]["identifierTypeId"], acPtr, vmId);
                                if (!string.IsNullOrEmpty(JsonObjectData[step]["identifierTypeName"]) && intPtr == IntPtr.Zero)
                                    intPtr = JabHelpers.GetAccessibleChildFromContextByName(vmId, acPtr, JsonObjectData[step]["identifierTypeName"]);

                                var data = JabHelpers.GetAccessibleContextInfo(vmId, intPtr);
                                if (data.accessibleSelection)
                                    done = JabHelpers.SetPanel(intPtr, vmId, JsonObjectData[step]["text"]);
                            }
                        }
                    }
                }
            }

            #endregion
        }

        //public IEnumerable<IntPtr> PMainwindowHWND(IntPtr p)
        //{
        //    yield return p;


        //}
    }
}
