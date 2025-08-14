using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArconConnector.BusinessLayer
{
    public class ReflectionXManager : ExeConnectorManager
    {
        private static string _filepath = "";
        private static string _fileName = "";

        public static string R_filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        public static string R_fileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override void PreLaunchActivity()
        {
            ReflectionXGenerateLoginFile();
            //MessageBox.Show("File");
            SetExePath();
        }
        public override void PostLaunchActivity()
        {
            DeleteStoredConnections();
        }
        public string ReflectionXGenerateLoginFile()
        {



            ReflectionXManager.R_filepath = Application.StartupPath + "/" + objBaseConnector.ServiceDetails.IPAddress + objBaseConnector.ServiceDetails.UserName + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".r2w";

            if (objBaseConnector.ServiceDetails.FileData != "")
            {
                ExeHelper.GenerateFile(objBaseConnector, this.ArconContext, ReflectionXManager.R_filepath);
            }

            return ReflectionXManager.R_filepath;

            //try
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("");
            //    sb.Append("<?xml version =\"1.0\" encoding =\"UTF-8\" standalone =\"yes\" ?>");
            //    sb.AppendLine();
            //    sb.Append("<REFLECTION_SETTINGS version = \"3\" >");
            //    sb.AppendLine();
            //    sb.Append("<CREATOR product =\"WRQ Reflection for UNIX and OpenVMS\" version=\"13.0\" build = \"260\" language = \"English\" type = \"2\" emulation = \"VT\" />");
            //    sb.AppendLine();
            //    sb.Append("<TYPE mode =\"All\">SETTINGS</TYPE>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION version = \"3.0\">");
            //    sb.AppendLine();
            //    sb.Append("<COUNT> 19 </COUNT>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_TYPE>SECURE SHELL </CONNECTION_TYPE>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name = \"CheckParity\">");
            //    sb.AppendLine();
            //    sb.Append("<BOOLEAN> False </BOOLEAN>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name = \"Parity\">");
            //    sb.AppendLine();
            //    sb.Append("<ENUMERATION>8/NONE</ENUMERATION>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"CharDelay\">");
            //    sb.AppendLine();
            //    sb.Append("<INTEGER>0</INTEGER>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"Host\">");
            //    sb.AppendLine();
            //    sb.Append("<STRING>" + objBaseConnector.ServiceDetails.IPAddress + "</STRING>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"UserName\">");
            //    sb.AppendLine();
            //    sb.Append("<STRING>" + objBaseConnector.ServiceDetails.UserName + "</STRING>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"ExitAllowed\">");
            //    sb.AppendLine();
            //    sb.Append("<BOOLEAN>True</BOOLEAN>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"ExitOnDisconnect\">");
            //    sb.AppendLine();
            //    sb.Append("<BOOLEAN>False</BOOLEAN>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"ConnectMacro\">");
            //    sb.AppendLine();
            //    sb.Append("<FILE></FILE>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"ConnectMacroData\">");
            //    sb.AppendLine();
            //    sb.Append("<STRING></STRING>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"Timeout\">");
            //    sb.AppendLine();
            //    sb.Append("<INTEGER>0</INTEGER>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"UsePCUserName\">");
            //    sb.AppendLine();
            //    sb.Append("<BOOLEAN>False</BOOLEAN>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"SSHPort\">");
            //    sb.AppendLine();
            //    sb.Append("<INTEGER>" + objBaseConnector.ServiceDetails.Port + "</INTEGER>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"SSHTermType\">");
            //    sb.AppendLine();
            //    sb.Append("<STRING>LINUX</STRING>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"SSHTermDefault\">");
            //    sb.AppendLine();
            //    sb.Append("<STRING>VT100,DEC-VT100</STRING>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"LowerCaseUserName\">");
            //    sb.AppendLine();
            //    sb.Append("<BOOLEAN>False</BOOLEAN>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"SSHSetHostWindowSize\">");
            //    sb.AppendLine();
            //    sb.Append("<BOOLEAN>True</BOOLEAN>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"SSHShowBannerDialog\">");
            //    sb.AppendLine();
            //    sb.Append("<BOOLEAN>True</BOOLEAN>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("<CONNECTION_SETTING name=\"SSHConfigScheme\">");
            //    sb.AppendLine();
            //    sb.Append("<STRING></STRING>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION_SETTING>");
            //    sb.AppendLine();
            //    sb.Append("</CONNECTION>");
            //    sb.AppendLine();
            //    sb.Append("</REFLECTION_SETTINGS>");
            //    sb.AppendLine();
            //    using (StreamWriter swriter = new StreamWriter(ReflectionXManager.R_filepath))
            //    {
            //        swriter.Write(sb.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //}

            return ReflectionXManager.R_filepath;
        }
        public void DeleteStoredConnections()
        {
            string filepath = ReflectionXManager.R_filepath;

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
        }
        public void SetExePath()
        {
            objExeConnector.ExePath = ReflectionXManager.R_filepath;
        }
    }
}
