using JavaAutomation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static JavaAutomation.JabHelpers;
using static JavaAutomation.WindowsAccessBridge;

namespace TestClient
{


    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        private WinAccessAPIManager objWinAccessManager = ApiFactory.CreateNativeApi<WinAccessAPIManager>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            //////////{
            //////////    string value;
            //////////    // Check whether the environment variable exists.
            //////////    value = Environment.GetEnvironmentVariable("Path");
            //////////    // If necessary, create it.
            //////////    if (value == null)
            //////////    {
            //////////        Environment.SetEnvironmentVariable("Test1", "Value1");
            //////////    }

            //////////        // Now retrieve it.
            //////////        value = Environment.GetEnvironmentVariable("Test1");
            //////////    Environment.SetEnvironmentVariable("Path", "F:\\JavaAlok\\jdk1.8.0_171", EnvironmentVariableTarget.User);
            //////////  //  Environment.SetEnvironmentVariable("Path", "F:\\JavaAlok\\jre1.8.0_171", EnvironmentVariableTarget.User);
            //////////    //Environment.SetEnvironmentVariable()
            //////////    //EachVariable(value, EnvironmentVariableTarget.User);
            //////////    Environment.GetEnvironmentVariables();
            //////////    Environment.SetEnvironmentVariable("JAVA_HOME", "F:\\JavaAlok");
            ////////// //   Environment.env

            ////ProcessStartInfo psi = new ProcessStartInfo();
            ////psi.FileName = "java.exe";
            ////psi.Arguments = " -version";
            ////psi.RedirectStandardError = true;
            ////psi.UseShellExecute = false;

            ////Process pr = Process.Start(psi);
            ////string strOutput = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");


            //RegistryKey rk = Registry.LocalMachine;
            //RegistryKey subKey = rk.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");

            //string currentVerion = subKey.GetValue("CurrentVersion").ToString();
            //if(Environment.Is64BitOperatingSystem)
            //     objWinAccessManager.Windows_run();
            //    else
            objWinAccessManager.Windows_run();

        }


        public void Senddata(Int32 VmId, IntPtr acPtr, AccessibleTreeItem accessibleTreeItem, string name, string text, string data)
        {
            List<AccessibleTreeItem> point = null; IntPtr zero = IntPtr.Zero;
            AccessibleContextInfo ac = new AccessibleContextInfo();
            accessibleTreeItem = JabHelpers.GetAccessibleContextInfo(VmId, acPtr, out ac, null, 0, string.Empty);
            IntPtr test = IntPtr.Zero;

            for (int j = 0; j < accessibleTreeItem.childrenCount; j++)
            {
                point = Utils.checkval(accessibleTreeItem.children[j], name, text);

                if (point.Count > 0)
                {
                    zero = objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, j);
                    acPtr = zero;
                    accessibleTreeItem = accessibleTreeItem.children[j];
                    j = -1;
                }

            }

            objWinAccessManager.setTextContents(VmId, zero, data);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            //for lmt
            //IntPtr hWnd = FindWindow("SunAwtFrame", "PKLite SQL Client");
            //FindWindow("SunAwtDialog", "User Login");

           // sql developer
            IntPtr hWnd = FindWindow("SunAwtDialog", "New / Select Database Connection");

            Int32 VmId = 0; IntPtr acPtr;
            JavaAutomation.JabHelpers.GetComponentTree(hWnd, out VmId, out acPtr);

            AccessibleTreeItem accessibleTreeItem = new AccessibleTreeItem();
            AccessibleContextInfo ac = new AccessibleContextInfo();
            accessibleTreeItem = JabHelpers.GetAccessibleContextInfo(VmId, acPtr, out ac, null, 0, string.Empty);

            //IntPtr dropdown = objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, objWinAccessManager.getAccessibleChildFromContext(
            //    VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 9), 0), 0), 0), 4), 0), 0), 0), 0), 2);

            //objWinAccessManager.setTextContents(VmId, dropdown, "SYSDBA");



            //IntPtr test = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 9), 0), 0), 0), 4);//, 0),0),0),0),2);
            //objWinAccessManager.setTextContents(VmId, test, "SYSDBA");

            //1 st try
            //test using recursive
            //Senddata(VmId, acPtr, accessibleTreeItem, "", "text", "username");
            //Senddata(VmId, acPtr, accessibleTreeItem, "", "password text", "password");
            //

            //2 nd try
            //test using index
            //for sql developer
            //IntPtr test = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr,0),1),0),0),0),0),0),0),4);
            //objWinAccessManager.setTextContents(VmId, test, "saurabh");
            //for lmt client

            //IntPtr ConnectionName = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 0), 1), 0), 0), 0), 0), 1), 0), 3);
            IntPtr ConnectionName = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 2);
            MessageBox.Show("con -> " + ConnectionName.ToString());
            objWinAccessManager.setTextContents(VmId, ConnectionName, "connectionname");
            Senddata(VmId, acPtr, accessibleTreeItem, "", "text", "ds");

            IntPtr usernameLmt = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 1), 0), 1);
            objWinAccessManager.setTextContents(VmId, usernameLmt, "saurabh");

            IntPtr passwordLmt = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 1), 1), 1);
            objWinAccessManager.setTextContents(VmId, passwordLmt, "password");





























            //Senddata(VmId, acPtr, accessibleTreeItem, "Username", "text", "Alok");
            //Senddata(VmId, acPtr, accessibleTreeItem, "", "text", "service");
            //Senddata(VmId, acPtr, accessibleTreeItem, "Password", "password text", "Password1234");
            //Senddata(VmId, acPtr, accessibleTreeItem, "Connection Name", "text", "textdata");
            //Senddata(VmId, acPtr, accessibleTreeItem, "Hostname", "text", "hostname");
            //Senddata(VmId, acPtr, accessibleTreeItem, "Port", "text", "8080");
            //Senddata(VmId, acPtr, accessibleTreeItem, "", "text", "service");


            //uncomment below if above function is not working.

            //////////var pointer = new AccessibleTreeItem[] { accessibleTreeItem }.SelectNestedChildren(x => x.children).Where(t => t.name == "Username").Where(b => b.role == "text").ToList();
            //////////var output = JabHelpers.GetAccessibleContextInfo(VmId, acPtr, out ac, pointer[0], 0, string.Empty);

            ////////////  var pointer = new AccessibleTreeItem[] { accessibleTreeItem }.SelectNestedChildren(x => x.children).Where(t => t.name == "Username").Where(b=>b.role=="text").ToList();

            //IntPtr ConnectionName = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 2);

            /////////IntPtr username = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 4);

            //////////IntPtr password = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 6);

            //////////IntPtr Hostname = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId,
            //////////                  objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 8), 0), 0), 1), 0), 1);

            //////////IntPtr port = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId,
            //////////              objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 8), 0), 0), 1), 0), 3);


            //////////IntPtr sid = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId,
            //////////             objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 8), 0), 0), 1), 0), 5);

            //////////IntPtr serviceName = objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId,
            //////////                    objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, objWinAccessManager.getAccessibleChildFromContext(VmId, acPtr, 0), 1), 0), 0), 0), 0), 0), 0), 8), 0), 0), 1), 0), 7);

            //////////objWinAccessManager.setTextContents(VmId, ConnectionName, "Connection Name");
            //////////objWinAccessManager.setTextContents(VmId, username, "Alok");
            //////////objWinAccessManager.setTextContents(VmId, password, "Testpass12345");
            //////////objWinAccessManager.setTextContents(VmId, Hostname, "10.10.1.117");
            //////////objWinAccessManager.setTextContents(VmId, port, "8084");
            ////////////objWinAccessManager.setTextContents(VmId, sid, "123456");
            //////////objWinAccessManager.setTextContents(VmId, serviceName, "123456");
        }


    }
}
