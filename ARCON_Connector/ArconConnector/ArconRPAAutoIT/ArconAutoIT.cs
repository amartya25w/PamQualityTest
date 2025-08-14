using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoIt;

namespace ArconRPAAutoIT
{
    public class ArconAutoIT
    {
        static void Main(string[] args)
        {

            String windowTitleName = "Connect to Server";
            String processName = "ssms.exe";
            String path = "C:/Program Files (x86)/Microsoft SQL Server/110/Tools/Binn/ManagementStudio/ssms.exe";
            String dialogBoxFieldParam1 = "NAME:userName";
            String dialogBoxFieldValue1 = "arcossqladmingh";
            String dialogBoxFieldParam2 = "NAME:password";
            String dialogBoxFieldValue2 = "arcossqladmin";
            String dialogBoxFieldParamComboBox = "NAME:comboBoxAuthentication";
            String dialogBoxFieldValueComboBox = "";
            String dialogBoxFieldParamButton = "NAME:connect";
            String dialogBoxFieldParamCheckBox = "NAME:savePassword";
            String dialogBoxFieldParamCheckBoxValue;                                                                    // 1:checked & 0:Unchecked      
            runProcess(processName, path);

            //For Windows Authentication : 
            dialogBoxFieldValueComboBox = "Window";                                                                  // Option name to be selected in checkbox
            SelectComboBoxField(processName, windowTitleName, dialogBoxFieldParamComboBox, dialogBoxFieldValueComboBox);
            PressButtonField(processName, windowTitleName, dialogBoxFieldParamButton);


            //Uncomment for SQL Authentication

            //For SQL Server Authentication : 
            dialogBoxFieldValueComboBox = "Windows";                                                                     // Option name to be selected in checkbox
            dialogBoxFieldParamCheckBoxValue = "0";
            //SelectComboBoxField(processName, windowTitleName, dialogBoxFieldParamComboBox, dialogBoxFieldValueComboBox);
            //PutTextField(processName, windowTitleName, dialogBoxFieldParam1, dialogBoxFieldValue1);
            //PutTextField(processName, windowTitleName, dialogBoxFieldParam2, dialogBoxFieldValue2);
            //  //SelectCheckBoxField(processName, windowTitleName, dialogBoxFieldParamCheckBox, dialogBoxFieldParamCheckBoxValue);
            // PressButtonField(processName, windowTitleName, dialogBoxFieldParamButton);


        }


        public static bool runProcess(String processName, String processInstalledPath)
        {
            long processID = 0;
            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {
                processID = AutoItX.Run(processInstalledPath, "", 1);
                Console.WriteLine("Process Started !");
                return true;

            }
            return false;
        }
         
        public static bool WinWaitActive(String windowTitleName)
        {
            if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
            {
                return true;
            }
            else
                return false;
        }

        public static bool PutTextField(String processName, String windowTitleName, String dialogueBoxFieldParam, String dialogueBoxFieldValue)
        {
            long handle = 0;

            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {
                while (true)
                {
                    if (AutoItX.ProcessExists(processName) != 1)
                    {
                        if (true)
                        {
                            AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                            {
                                if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                                {
                                    System.Threading.Thread.Sleep(3000);
                                    AutoItX.ControlSetText(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", dialogueBoxFieldValue);
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }


        public static bool PressButtonField(String processName, String windowTitleName, String dialogueBoxFieldParam)
        {
            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {
                while (true)
                {
                    if (AutoItX.ProcessExists(processName) != 1)
                    {
                        if (true)
                        {
                            AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                            {

                                if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                                {
                                    AutoItX.ControlClick(windowTitleName, "", "[" + dialogueBoxFieldParam + "]");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }


        public static bool SelectComboBoxField(String processName, String windowTitleName, String dialogueBoxFieldParam, String dialogueBoxFieldValue)
        {
            while (true)
            {
                if (AutoItX.ProcessExists(processName) != 1)
                {
                    if (true)
                    {
                        AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                        {
                            int handle = AutoItX.WinWaitActive(windowTitleName, "", 400000);
                            if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                            {
                                //From External file (required installation)
                                //AutoItX.Run(@"""D:\EXE\AutoIt3\AutoIt3.exe"" /AutoIt3ExecuteScript granular.au3 comboBoxField "+ dialogueBoxFieldParam + " "+ dialogueBoxFieldValue + " "+processName+" ", @"D:\EXE\AutoIt3\", 1);
                                //AutoItX.ControlClick(windowTitleName, "", "[" + dialogueBoxFieldParam + "]");

                                //Not Working
                                //AutoItX.ControlCommand(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", "ShowDropDown", "");
                                //String index= AutoItX.ControlCommand(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", "GetCurrentSelection", "");
                                //Console.WriteLine(index);
                                //AutoItX.Sleep(3000);
                                //AutoItX.ControlSend(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", "ShowDropDown");

                                AutoItX.ControlSend(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", dialogueBoxFieldValue);

                                //AutoItX.ControlClick(windowTitleName, "", "[" + dialogueBoxFieldParam + "]");

                                //Not Working
                                //AutoItX.ControlCommand(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", "ShowDropDown", "");
                                //AutoItX.ControlCommand(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", "SetCurrentSelection", "Windows");
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }
                }
            }
        }


        public static bool SelectCheckBoxField(String processName, String windowTitleName, String dialogueBoxFieldParam, String dialogBoxFieldParamCheckBoxIsSelected)
        {

            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {

                while (true)
                {
                    if (AutoItX.ProcessExists(processName) != 1)
                    {
                        if (true)
                        {
                            AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                            {

                                if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                                {
                                    if (dialogBoxFieldParamCheckBoxIsSelected.Equals("1"))
                                    {
                                        AutoItX.ControlCommand(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", "Check", "");
                                        return true;
                                    }
                                    else if (dialogBoxFieldParamCheckBoxIsSelected.Equals("0"))
                                    {
                                        AutoItX.ControlCommand(windowTitleName, "", "[" + dialogueBoxFieldParam + "]", "UnCheck", "");
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }


        public static bool SendKeyPutTextField(String processName, String windowTitleName, String dialogueBoxFieldValue)
        {
            long handle = 0;

            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {
                while (true)
                {
                    if (AutoItX.ProcessExists(processName) != 1)
                    {
                        if (true)
                        {
                            AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                            {
                                if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                                {
                                    AutoItX.Send(dialogueBoxFieldValue);
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }


        public static bool SendTab(String processName, String windowTitleName)
        {
            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {
                while (true)
                {
                    if (AutoItX.ProcessExists(processName) != 1)
                    {
                        if (true)
                        {
                            AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                            {

                                if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                                {
                                    AutoItX.Send("{TAB}");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }


        public static bool SendKeyClickField(String processName, String windowTitleName)
        {
            long handle = 0;

            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {
                while (true)
                {
                    if (AutoItX.ProcessExists(processName) != 1)
                    {
                        if (true)
                        {
                            AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                            {
                                if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                                {
                                    AutoItX.Send("{ENTER}");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }


        public static bool SendKeyCheckBoxField(String processName, String windowTitleName)
        {
            long handle = 0;

            AutoItX.AutoItSetOption("RunErrorsFatal", 0);
            {
                while (true)
                {
                    if (AutoItX.ProcessExists(processName) != 1)
                    {
                        if (true)
                        {
                            AutoItX.AutoItSetOption("WinTitleMatchMode", 4);
                            {
                                if (AutoItX.WinWaitActive(windowTitleName, "", 400000) != 0)
                                {
                                    AutoItX.Send("{SPACE}");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }

    }
}
