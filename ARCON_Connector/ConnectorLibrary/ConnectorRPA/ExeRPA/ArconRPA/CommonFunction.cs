using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

//using windows form dll for taking screenshot
//C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Windows.Forms.dll

namespace ArconRPA
{
    class CommonFunction
    {
        

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public const int LEFTDOWN = 0x02;
        public const int LEFTUP = 0x04;
        public const int RIGHTDOWN = 0x08;
        public const int Double = 0x08;
        public const int RIGHTUP = 0x10;
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        public IntPtr appWin;

        #region Launch EXE 
        public Process launchExe(string exePath, string exeName, string processName, string argument)
        {
            Process processExecuteEXE = new Process();
            try
            {
                exePath = exePath + "\\" + exeName;
                processExecuteEXE.StartInfo.FileName = exePath;
                if (argument != "")
                    processExecuteEXE.StartInfo.Arguments = argument;
                processExecuteEXE.Start();
            }
            catch (Exception ex) { throw ex; }

            return processExecuteEXE;

        }
        #endregion

        

        #region Call Python Exe
        public bool PythonEXECalling(string largeImage, string smallImage, out Point p, string image_score)
        {

            string workingDirectoryPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            p = new Point(0, 0);
            bool resultt = false;
            float xavg, yavg;
            int x, y;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = workingDirectoryPath + "\\script.exe";
            //int co = 1;
            //string image_score = "0.7";
            //System.Drawing.Image large2 = Base64ToImage(largeImage);

            /*
            Image li = Base64ToImage(largeImage);
            Image si = Base64ToImage(smallImage);


            byte[] bytes = Convert.FromBase64String(largeImage);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                
                image = Image.FromStream(ms);
                
            }

            image = new Bitmap(image);
            image.Save(workingDirectoryPath + "\\dist\\la.png", ImageFormat.Png);
            image.Dispose();
            

            byte[] bytes2 = Convert.FromBase64String(smallImage);
            Image image2;
            using (MemoryStream ms = new MemoryStream(bytes2))
            {
                image2 = Image.FromStream(ms);
                image2.Save(workingDirectoryPath + "\\dist\\sm.png");
                image2.Dispose();
            }
            


            */

            //large2.Save(workingDirectoryPath+"\\dist\\la.png");
            // System.Drawing.Image small2 = Base64ToImage(smallImage);
            // large2.Save(workingDirectoryPath+"\\dist\\sm.png");
            string info = largeImage + "|" + smallImage + "|" + Screen.PrimaryScreen.Bounds.Width + "," + Screen.PrimaryScreen.Bounds.Height + "," + image_score;




            // startInfo.Arguments = path;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    process.StandardInput.WriteLine(info);
                    process.StandardInput.Close();
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        Console.WriteLine("c" + result);
                        if (result != "No Match Found" && result!="")
                        {
                            string[] splitPoints = result.Split(',');
                            string resultString = Regex.Match(splitPoints[0], @"\d+").Value;
                            int x1 = Int32.Parse(resultString);
                            Console.WriteLine("x1" + x1);
                            resultString = Regex.Match(splitPoints[1], @"\d+").Value;
                            int y1 = Int32.Parse(resultString);
                            Console.WriteLine("y1" + y1);
                            resultString = Regex.Match(splitPoints[2], @"\d+").Value;
                            int x2 = Int32.Parse(resultString);
                            Console.WriteLine("x2" + x2);
                            resultString = Regex.Match(splitPoints[3], @"\d+").Value;
                            int y2 = Int32.Parse(resultString);
                            Console.WriteLine("y2" + y2);
                            xavg = (x1 + x2) / 2;
                            yavg = (y1 + y2) / 2;
                            x = (int)xavg;
                            y = (int)yavg;
                            p = new Point(x, y);
                            resultt = true;
                            return resultt;
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                return resultt;
            }

            return resultt;

        }
        #endregion

        #region Take Screenshot
        public System.Drawing.Image Takescreenshot()
        {
            Bitmap shot = null;
            try
            {
                Logger.Log.Info("Inside TakeScreenshot");
                Size shotSize = Screen.PrimaryScreen.Bounds.Size;

                // the upper left point in the screen to start shot
                // 0,0 to get the shot from upper left point
                Point upperScreenPoint = new Point(0, 0);

                // the upper left point in the image to put the shot
                Point upperDestinationPoint = new Point(0, 0);

                // create image to get the shot in it
                shot = new Bitmap(shotSize.Width, shotSize.Height);

                // new Graphics instance 
                Graphics graphics = Graphics.FromImage(shot);

                // get the shot by Graphics class 
                graphics.CopyFromScreen(upperScreenPoint, upperDestinationPoint, shotSize);
                graphics.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
            return shot;
        }
        #endregion



        #region Convert To Base 64 
        public string ConvertToBase64(System.Drawing.Image image)
        {
            Logger.Log.Info("Inside ConvertToBase64");
            string base64String = string.Empty;
            try
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, ImageFormat.Png);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }catch(Exception ex)
            {
                Logger.Log.Error(ex.Message,ex);
                throw ex;
            }


        }
        #endregion

        #region Base 64 to Image
        public System.Drawing.Image Base64ToImage(string base64String)
        {
            System.Drawing.Image image = null;
            try
            {
                Logger.Log.Info("Inside Base64ToImage");
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0,
                  imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = System.Drawing.Image.FromStream(ms, true);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
            return image;
        }
        #endregion

        #region Load Image
        public string LoadImage(string filename)
        {
            string image_base = string.Empty;
            try
            {               
                if (filename == "db")
                    filename = "./config/db.png";
                else if (filename == "user")
                    filename = "./config/username.png";
                else if (filename == "password")
                    filename = "./config/password.png";
                else if (filename == "ok")
                    filename = "./config/ok.png";
                image_base = ConvertToBase64(Image.FromFile(filename));
                
            }
            catch(Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;              
            }
            return image_base;
        }
        #endregion

        #region Convert To Format
        public Bitmap ConvertToFormat(System.Drawing.Image image, System.Drawing.Imaging.PixelFormat format)
        {
            Bitmap copy = null;
            try
            {
                copy = new Bitmap(image.Width, image.Height, format);
                using (Graphics gr = Graphics.FromImage(copy))
                {
                    gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
                }
            }catch(Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
            return copy;
        }
        #endregion

        #region Find Bitmap
        public static bool FindBitmap(Bitmap searchBitmap, Bitmap withinBitmap, out Point point)
        {
            try
            {
                for (var outerX = 0; outerX < withinBitmap.Width - searchBitmap.Width; outerX++)
                {
                    for (var outerY = 0; outerY < withinBitmap.Height - searchBitmap.Height; outerY++)
                    {
                        for (var innerX = 0; innerX < searchBitmap.Width; innerX++)
                        {
                            for (var innerY = 0; innerY < searchBitmap.Height; innerY++)
                            {
                                var searchColor = searchBitmap.GetPixel(innerX, innerY);
                                var withinColor = withinBitmap.GetPixel(outerX + innerX, outerY + innerY);

                                if (searchColor.R != withinColor.R || searchColor.G != withinColor.G ||
                                    searchColor.B != withinColor.B)
                                {
                                    goto NotFound;
                                }
                            }
                        }
                        point = new Point(outerX, outerY);
                        point.X += searchBitmap.Width / 2; // Set X to the middle of the bitmap.
                        point.Y += searchBitmap.Height / 2; // Set Y to the center of the bitmap.
                        Console.Write("points" + point.X.ToString());
                        return true;

                        NotFound:
                        continue;
                    }
                }
                point = Point.Empty;
            }catch(Exception ex)
            {
                Logger.Log.Error(ex.Message,ex);
                throw ex;
            }
            return false;
        }
        #endregion

        #region Mouse Events
        public void MouseRight(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(RIGHTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(1000);
            mouse_event(RIGHTUP, xpos, ypos, 0, 0);
        }
        public void MouseLeft(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(LEFTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(1000);
            mouse_event(LEFTUP, xpos, ypos, 0, 0);



        }
        public void MouseDoubleClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            //MouseLeft(xpos, ypos);
            mouse_event(LEFTDOWN, xpos, ypos, 0, 0);
            //System.Threading.Thread.Sleep(1000);
            mouse_event(LEFTUP, xpos, ypos, 0, 0);
            mouse_event(LEFTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(1000);
            mouse_event(LEFTUP, xpos, ypos, 0, 0);



        }
        #endregion

        #region Find Element
        public AutomationElementCollection findelement(AutomationElement window, string identifier_typeId, string identifier_typeName, string identifierClass, out int elementCount)
        {
            AutomationElementCollection button = null;
            try
            {
                //int 
                elementCount = 0;
                if (identifierClass.Contains("[") && identifierClass.Contains("]"))
                {
                    try
                    {
                        var identifierClassTemp = identifierClass.Substring(0, identifierClass.LastIndexOf('['));
                        char[] replaceChar = { '[', ']' };
                        identifierClass = identifierClass.Replace(identifierClassTemp, "").Trim(replaceChar);
                        Int32.TryParse(identifierClass, out elementCount);
                        identifierClass = identifierClassTemp;
                    }
                    catch { }
                }
                Logger.Log.Info("Inside Automation Element Collection");
                if (identifierClass != "")
                {

                    if (identifier_typeId != "" && identifierClass != "")
                    {
                        button = window.FindAll(TreeScope.Descendants, new AndCondition(
                   new PropertyCondition(AutomationElement.AutomationIdProperty, identifier_typeId), new PropertyCondition(AutomationElement.ClassNameProperty, identifierClass)));
                    }
                    else if (identifier_typeName != "" && identifierClass != "")
                    {

                        button = window.FindAll(TreeScope.Descendants, new AndCondition(
                      new PropertyCondition(AutomationElement.NameProperty, identifier_typeName), new PropertyCondition(AutomationElement.ClassNameProperty, identifierClass)));
                    }


                    if (button == null || button.Count == 0)
                    {
                        if (identifier_typeName != "")
                        {
                            button = window.FindAll(TreeScope.Descendants, new AndCondition(
                                             new PropertyCondition(AutomationElement.NameProperty, identifier_typeName), new PropertyCondition(AutomationElement.ClassNameProperty, identifierClass)));
                        }
                        if (button == null || button.Count == 0)
                        {
                            if (identifierClass != "")
                            {

                             button = window.FindAll(TreeScope.Descendants,
                              new PropertyCondition(AutomationElement.ClassNameProperty, identifierClass));
                            }
                        }

                        if (button == null || button.Count == 0)
                        {
                            if (identifier_typeId != "")
                            {
                                button = window.FindAll(TreeScope.Descendants,
                           new PropertyCondition(AutomationElement.AutomationIdProperty, identifier_typeId));
                            }
                        }
                    }
                }
                else
                {
                    if (identifier_typeId != "")
                    {
                        button = window.FindAll(TreeScope.Descendants,new PropertyCondition(AutomationElement.AutomationIdProperty, identifier_typeId));
                    }
                    else if (identifier_typeName != "")
                    {
                        button = window.FindAll(TreeScope.Descendants,new PropertyCondition(AutomationElement.NameProperty, identifier_typeName));
                    }
                    if (button == null || button.Count == 0)
                    {
                        if (identifier_typeName != "")
                        {
                            button = window.FindAll(TreeScope.Descendants,new PropertyCondition(AutomationElement.NameProperty, identifier_typeName));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message,ex);
                throw ex;
            }
            //if(button!=null)
            //{
            //    (button.Count>0)
            //}
            return button;
        }
        #endregion

        #region Click Element By Coordinates
        public bool ClickElementByCoordinates(AutomationElement element, string action)
        {
            try
            {
                //element.SetFocus();
                
                
                Logger.Log.Info("Inside Click Element By Coordinates");
                int xcentre = 0, ycentre = 0;
                bool y = Getpoint(element);
                if (y == false)
                    Visible1(element);
                Thread.Sleep(5000);
                y = Getpoint(element);
                if (y == true)
                {
                    System.Windows.Point p = element.GetClickablePoint();
                    xcentre = Convert.ToInt32(p.X);
                    ycentre = Convert.ToInt32(p.Y);
                }
                else
                {
                    int xpos = (int)element.Current.BoundingRectangle.X;
                    int ypos = (int)element.Current.BoundingRectangle.Y;
                    Console.WriteLine("pos1" + xpos + "," + ypos);
                    float width = (int)element.Current.BoundingRectangle.Width / 2;
                    float height = (int)element.Current.BoundingRectangle.Height / 2;
                    Console.WriteLine("width " + width + "," + height);
                    float xcentre1 = xpos + width;
                    float ycentre1 = ypos + height;
                    xcentre = (int)xcentre1;
                    ycentre = (int)ycentre1;
                }
                SetCursorPos(xcentre, ycentre);
                if (action == "Click")
                {
                    MouseLeft(xcentre, ycentre);
                    //SendKeys.SendWait("{ENTER}");
                }
                else if (action == "DoubleClick")
                    MouseDoubleClick(xcentre, ycentre);

                
            }
            catch(Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }

            return true;
        }
        #endregion

        #region Previously Entered
        public bool PreviouslyEntered(string action, IList<string> input_text_list)
        {
            Logger.Log.Info("Inside Previously Entered");
            bool index = false;
            try
            {
                if (action != "")
                    index = input_text_list.Any(s => s.Contains(action));
            }
            catch(Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
            return index;
        }
        #endregion

        #region Click Element UI Automation
        public bool ClickElementUIAutomation(AutomationElement Element, int step_no)
        {
            Logger.Log.Info("Inside click Element UI Automation");
            object objPattern;
            TogglePattern togPattern;
            InvokePattern invPattern;
            ExpandCollapsePattern expcolPattern;
            bool done = false;
            try
            {
                if (true == Element.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out objPattern))
                {
                    expcolPattern = objPattern as ExpandCollapsePattern;
                    expcolPattern.Expand();
                    done = true;
                }
                else if (true == Element.TryGetCurrentPattern(TogglePattern.Pattern, out objPattern))
                {
                    togPattern = objPattern as TogglePattern;
                    togPattern.Toggle();
                    done = true;
                }
                else if (true == Element.TryGetCurrentPattern(InvokePattern.Pattern, out objPattern))
                {
                    invPattern = objPattern as InvokePattern;
                    invPattern.Invoke();
                    done = true;
                }
            }
            catch(Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
            return done;
        }
        #endregion

        #region Insert Text Using Automation
        public bool InsertTextUsingUIAutomation(AutomationElement element, string value,bool sendData=false)
        {
            Logger.Log.Info("Insert Text Using UI Automation");
            bool done = false;
            string fl = string.Empty;
            try
            {

                // Validate arguments / initial setup
                if (value == null)
                    //throw new ArgumentNullException(
                    //  "String parameter must not be null.");
                    return done;

                if (element == null)
                    return done;

                // A series of basic checks prior to attempting an insertion.
                //
                // Check #1: Is control enabled?
                // An alternative to testing for static or read-only controls 
                // is to filter using 
                // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
                // and exclude all read-only text controls from the collection.
                if (!element.Current.IsEnabled)
                {
                    return done;
                }

                // Check #2: Are there styles that prohibit us 
                //           from sending text to this control?
                /*   if (!element.Current.IsKeyboardFocusable)
                   {
                       throw new InvalidOperationException(
                           "The control with an AutomationID of "
                           + element.Current.AutomationId.ToString()
                           + "is read-only.\n\n");
                   }*/


                // Once you have an instance of an AutomationElement,  
                // check if it supports the ValuePattern pattern.
                object valuePattern = null;

                // Control does not support the ValuePattern pattern 
                // so use keyboard input to insert content.
                //
                // NOTE: Elements that support TextPattern 
                //       do not support ValuePattern and TextPattern
                //       does not support setting the text of 
                //       multi-line edit or document controls.
                //       For this reason, text input must be simulated
                //       using one of the following methods.
                //       
                if (!element.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern) || sendData)
                {

                    // Set focus for input functionality and begin.
                    if (element.Current.IsKeyboardFocusable)
                        element.SetFocus();
                    
                    // Pause before sending keyboard input.
                    Thread.Sleep(100);

                    // Delete existing content in the control and insert new content.
                    SendKeys.SendWait("^{HOME}");   // Move to start of control
                    SendKeys.SendWait("^+{END}");   // Select everything
                    SendKeys.SendWait("{DEL}");
                    // Delete selection


                    SendKeys.SendWait(value);
                    done = true;
                    return done;
                }
                // Control supports the ValuePattern pattern so we can 
                // use the SetValue method to insert content.
                else
                {
                    // Set focus for input functionality and begin.
                    if (element.Current.IsKeyboardFocusable)
                        element.SetFocus();

                    ((ValuePattern)valuePattern).SetValue(value);

                    done = true;
                    return done;
                }
            }
            catch (ArgumentNullException ez)
            {
                Logger.Log.Error(ez.Message, ez);
                throw ez;
            }
            catch (InvalidOperationException ey)
            {
                Logger.Log.Error(ey.Message, ey);
                throw ey;
            }
            catch(Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }

        }
        #endregion

        #region Select Combo Box Item
        public bool SelectComboboxItem(AutomationElement comboBox, string item, int step_no)
        {
            object objPattern;
            try
            {
                if (true == comboBox.TryGetCurrentPattern(SelectionItemPatternIdentifiers.Pattern, out objPattern))
                {
                    SelectionItemPattern selectionItemPattern =  objPattern as SelectionItemPattern;
                    selectionItemPattern.Select();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw ex;
            }
            return false;
        }
        #endregion

        #region Get Point
        private bool Getpoint(AutomationElement element)
        {
            System.Windows.Point p;
            try
            {
                p = element.GetClickablePoint();
                return true;
            }
            catch
            {
                return false;
            }



        }
        #endregion

        #region Visible
        private void Visible1(AutomationElement element)
        {
            try
            {
                AutomationElement window = AutomationElement.FromHandle((IntPtr)appWin);
                AutomationElementCollection buttonCollection = window.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ScrollBar));

                if (buttonCollection.Count != 0)
                {

                    AutomationElement aeButton = null;
                    if (buttonCollection.Count == 2)
                        aeButton = buttonCollection[1];
                    else
                        aeButton = buttonCollection[0];
                    RangeValuePattern rcpattern = (RangeValuePattern)aeButton.GetCurrentPattern(RangeValuePattern.Pattern);
                    int k = 50;
                    double ho_point1 = rcpattern.Current.Maximum;
                    int ho_point = Convert.ToInt32(ho_point1);
                    do
                    {
                        rcpattern.SetValue(Convert.ToDouble(k));
                        k = k + 50;
                        Console.WriteLine("y value of element " + element.Current.BoundingRectangle.Y);

                        if (Getpoint(element))
                        {
                            Console.WriteLine("inside if");
                            break;
                        }

                    } while (k < ho_point);
                }
            }
            catch
            {

            }
        }
        #endregion
    }

    
}
