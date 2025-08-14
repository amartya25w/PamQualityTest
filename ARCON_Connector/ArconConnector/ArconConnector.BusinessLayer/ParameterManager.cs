using ArconConnector.BusinessEntities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Enum = ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class ParameterManager : ArconBaseManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _LogCode = "PM", _MethodName = string.Empty;
        public ParameterDetails ConvertParameterToObject(string[] cmdArgs)
        {
            _LogCode = "PM:CPTO";
            _MethodName = MethodBase.GetCurrentMethod().Name;
            _Log.Info(_MethodName + " Method Started");
            try
            {
                ParameterDetails objParameterDetails = new ParameterDetails();
                objParameterDetails.ExeName = cmdArgs[0];

                //if (objParameterDetails.ExeName.ToUpper().IndexOf(System.Enum.GetName(typeof(Enum.CallingApplication), Enum.CallingApplication.ARCOSSCRIPTMANAGER)) > 0)
                //    objParameterDetails.SessionId = Convert.ToInt32(ConnectorHelper.DecryptTextF(cmdArgs[1]));
                if (objParameterDetails.ExeName.ToUpper().IndexOf(System.Enum.GetName(typeof(Enum.CallingApplication), Enum.CallingApplication.ARCOSBIOMETRICFINGERPRINTAUTHENTICATOR)) > 0)
                    objParameterDetails.UserId = Convert.ToInt32(ConnectorHelper.DecryptTextF(cmdArgs[1]));
                else
                    objParameterDetails.SessionId = Convert.ToInt32(ConnectorHelper.DecryptTextF(cmdArgs[1]));

                objParameterDetails.DBConnection = new DBConnection()
                {
                    ServerIP = ConnectorHelper.DecryptTextF(cmdArgs[2]),
                    ServerPort = Convert.ToInt32(ConnectorHelper.DecryptTextF(cmdArgs[3])),
                    DataBaseName = ConnectorHelper.DecryptTextF(cmdArgs[4]),
                    DataSource = ConnectorHelper.DecryptTextF(cmdArgs[6]),
                };
                objParameterDetails.RDPDBConnection = new DBConnection()
                {
                    ServerIP = objParameterDetails.DBConnection.ServerIP,
                    ServerPort = objParameterDetails.DBConnection.ServerPort,
                    DataSource = objParameterDetails.DBConnection.DataSource,
                    DataBaseName = ConnectorHelper.DecryptTextF(cmdArgs[5]),
                    //ConnectTimeout = GetInt32Value(GetARCOSRDPDBSQLConnectTimeout())
                };
                objParameterDetails.TakeRDPConsole = Convert.ToBoolean(ConnectorHelper.DecryptTextF(cmdArgs[7]));
                if (Convert.ToBoolean(ConnectorHelper.DecryptTextF(cmdArgs[8])))
                {
                    objParameterDetails.SSHVPNDetails = new VPNDetails()
                    {
                        AllowVPNTunnel = Convert.ToBoolean(ConnectorHelper.DecryptTextF(cmdArgs[8])),
                        GatewayIPAddress = ConnectorHelper.DecryptTextF(cmdArgs[9]),
                        GatewayPort = Convert.ToInt32(ConnectorHelper.DecryptTextF(cmdArgs[10])),
                        GatewayUserName = ConnectorHelper.DecryptTextF(cmdArgs[11]).ToString(),
                        GatewayPassword = ConnectorHelper.DecryptTextF(cmdArgs[12]).ToString(),
                        AllowVPNTunnelForDB = Convert.ToBoolean(ConnectorHelper.DecryptTextF(cmdArgs[13]))
                    };
                }
                else
                {
                    objParameterDetails.SSHVPNDetails = new VPNDetails()
                    {
                        AllowVPNTunnel = Convert.ToBoolean(ConnectorHelper.DecryptTextF(cmdArgs[8])),
                        GatewayIPAddress = "",
                        GatewayPort = 0,
                        GatewayUserName = "",
                        GatewayPassword = "",
                        AllowVPNTunnelForDB = Convert.ToBoolean(ConnectorHelper.DecryptTextF(cmdArgs[13]))
                    };
                }
                if (cmdArgs.Length >= 16)
                {
                    objParameterDetails.IsUseARCOSWebServiceDTForDatabase = Convert.ToBoolean(ConnectorHelper.DecryptTextF(cmdArgs[14]));
                    objParameterDetails.ARCOSWebServiceDTURL = ConnectorHelper.DecryptTextF(cmdArgs[15]);
                }
                //if (objParameterDetails.IsUseARCOSWebServiceDTForDatabase)
                //{
                //    if (getTLSProtocol() == "1")  
                //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //    else if (getTLSProtocol() == "")
                //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //}

                if (cmdArgs.Length >= 17)
                    objParameterDetails.IsTimeBasedSession = Convert.ToInt32(ConnectorHelper.DecryptTextF(cmdArgs[16])) > 0 ? true : false;
                if (cmdArgs.Length == 23)
                {
                    objParameterDetails.ARCOSMode = ConnectorHelper.DecryptTextF(cmdArgs[17]);
                    if (objParameterDetails.ARCOSMode.ToUpper() == Constant.RESTRICTEDMODE)
                    {
                        objParameterDetails.RADBConnection = new DBConnection()
                        {
                            ServerIP = ConnectorHelper.DecryptTextF(cmdArgs[18]),
                            ServerPort = Convert.ToInt32(ConnectorHelper.DecryptTextF(cmdArgs[19])),
                            DataBaseName = ConnectorHelper.DecryptTextF(cmdArgs[20]),
                            DataSource = ConnectorHelper.DecryptTextF(cmdArgs[22])
                        };
                        objParameterDetails.RARDPDBConnection = new DBConnection()
                        {
                            ServerIP = objParameterDetails.RADBConnection.ServerIP,
                            ServerPort = objParameterDetails.RADBConnection.ServerPort,
                            DataSource = objParameterDetails.RADBConnection.DataSource,
                            DataBaseName = ConnectorHelper.DecryptTextF(cmdArgs[21])
                        };
                    }
                }
                if (cmdArgs.Length >= 20)
                    objParameterDetails.IsRDPSConnection = cmdArgs[19].ToUpper() == "RDPS" ? true : false;
                return objParameterDetails;
            }
            catch (Exception ex)
            {
                _Log.Error(_LogCode + ex);
                throw ex;
            }
            _Log.Info(_MethodName + " Method Ended");
        }

        public void SetAdditionalDetails()
        {
            ArconBaseManager obj = new ArconBaseManager();
            // obj.GetBO<>("exe")
            //obj.GetBO("exe").GetType().GetMethods()
        }

    }
}
