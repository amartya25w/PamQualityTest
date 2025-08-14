using System;
using System.Collections.Generic;
using System.Text;

namespace ArconRealTimeSessionMonitor
{
    public class Servers // : ICloneable
    { 
    //{
    //    #region VARIABLES
    //    private int _RoboticImageId = 0;
    //    private string strHostName = string.Empty;
    //    private string strIPAddress = string.Empty;
    //    private string strIPAddress_Original = string.Empty;
    //    private string strServiceUserName = string.Empty;
    //    private string strServicePassword = string.Empty;
    //    private string strDomain = string.Empty;
    //    private string strDBInstanceName = string.Empty;
    //    private string strPort = string.Empty;
    //    private string strPort_Original = string.Empty;
    //    private Boolean bolRDPEM = false;
    //    private Boolean bolNamedService = false;
    //    private Boolean bolVaultService = false;
    //    private DateTime dtLastLoggedIn;
    //    private DateTime dtLastLoggedOut;
    //    private ObjectCommonProperties ocpCommonDetails = new ObjectCommonProperties();
    //    private ServiceType serviceType = ServiceType.SSHTelnet;
    //    private Boolean bolUserLockToConsole = false;
    //    private string strField1 = string.Empty;
    //    private string strField2 = string.Empty;
    //    private string strField3 = string.Empty;
    //    private string strField4 = string.Empty;
    //    private string strDescription = string.Empty;
    //    private string strServiceUserDisplayName = string.Empty;
    //    private string strServiceUserDescription = string.Empty;
    //    private Int32 intUseServer = 0;
    //    public Int32 ServiceVersion = -1;
    //    public Int64 LOBID { get; set; }
    //    public string PDM_ID { get; set; }

    //    //Added by Deepak for Putty WebService 
    //    private string _PuttyWebServiceURL = string.Empty;

    //    //Added on 27 Nov 2013 for Windows Authentication
    //    public Boolean _isWindowsAuth = false;

    //    // for Deskinsight Over internet - shabbir
    //    private RDPSServerdetails _RDPSServerDetails = new RDPSServerdetails();

    //    //Added on 07 May 2014 for PROMPT_USER (True or False)
    //    private Boolean bolIsPromptUser = false;
    //    private Boolean bolIsPerformSSO = true;

    //    //--- Add Variable by Nilesh For Dynamic Port  
    //    private Boolean _DynamicPort = false;

    //    public string ClienttSessionTitleConfig = "";

    //    // Added by shabbir for SSH Key Generation
    //    private Boolean _IsKeyEnabled = false;

    //    //added for robotic process
    //    private Boolean _IsRoboticProcess = false;

    //    #endregion

    //    #region PROPERTIES
    //    public override string ToString()
    //    {
    //        if (DBInstanceName.Length <= 0)
    //            return this.IPAddress + "@" + this.ServiceUserName + ":" + this.HostName;
    //        else
    //            return this.IPAddress + "@" + this.ServiceUserName + ":" + this.HostName + " (" + DBInstanceName + ")";
    //    }
    //    public string ToStringV2()
    //    {
    //        if (DBInstanceName.Length <= 0)
    //            return "(" + this.ServerServiceType + ") " + this.IPAddress + "@" + this.ServiceUserName;
    //        else
    //            return "(" + this.ServerServiceType + ") " + this.IPAddress + "@" + this.ServiceUserName + " (" + DBInstanceName + ")";
    //    }

    //    public string ToString_Description()
    //    {
    //        //return this.IPAddress + "@" + this.ServiceUserName + " (" + this.HostName + ") " + this.serviceType.ToString();
    //        return Description = GetSessionTitle();
    //        //if (DBInstanceName.Length <= 0)
    //        //    return this.IPAddress_Original + "@" + this.ServiceUserName + ":" + this.HostName + " - " + this.serviceType.ToString();
    //        //else
    //        //    return this.IPAddress_Original + "@" + this.ServiceUserName + ":" + this.HostName + " (" + DBInstanceName + ")" + " - " + this.serviceType.ToString();
    //    }

    //    public string ToString_Description_Domain()
    //    {
    //        if (DBInstanceName.Length <= 0)
    //            return "(" + this.ServerServiceType + ") " + this.IPAddress + "@" + this.ServiceUserName + ":" + this.Domain;
    //        else
    //            return "(" + this.ServerServiceType + ") " + this.IPAddress + "@" + this.ServiceUserName + ":" + this.Domain + " (" + DBInstanceName + ")";
    //    }
    //    public string LogFileName
    //    {
    //        get
    //        {
    //            if (DBInstanceName.Length <= 0)
    //                return this.serviceType.ToString() + "_" + this.IPAddress_Original + "_" + this.ServiceUserName;
    //            else
    //                return this.serviceType.ToString() + "_" + this.IPAddress_Original + "_" + this.ServiceUserName + "(" + DBInstanceName + ")";
    //        }
    //    }
    //    public DateTime LastLoggedIn
    //    {
    //        get { return dtLastLoggedIn; }
    //        set { dtLastLoggedIn = value; }
    //    }
    //    public DateTime LastLoggedOut
    //    {
    //        get { return dtLastLoggedOut; }
    //        set { dtLastLoggedOut = value; }
    //    }
    //    public string Port
    //    {
    //        get { return strPort; }
    //        set { strPort = value; }
    //    }
    //    public string Port_Original
    //    {
    //        get { return strPort_Original; }
    //        set { strPort_Original = value; }
    //    }
    //    public string DBInstanceName
    //    {
    //        get { return strDBInstanceName; }
    //        set { strDBInstanceName = value; }
    //    }
    //    public string HostName
    //    {
    //        get { return strHostName; }
    //        set { strHostName = value; }
    //    }
    //    public string Domain
    //    {
    //        get { return strDomain; }
    //        set { strDomain = value; }
    //    }
    //    public string IPAddress
    //    {
    //        get { return strIPAddress; }
    //        set { strIPAddress = value; }
    //    }
    //    public string IPAddress_Original
    //    {
    //        get { return strIPAddress_Original; }
    //        set { strIPAddress_Original = value; }
    //    }
    //    public string ServiceUserName
    //    {
    //        get { return strServiceUserName; }
    //        set { strServiceUserName = value; }
    //    }
    //    public string ServicePassword
    //    {
    //        get { return strServicePassword; }
    //        set { strServicePassword = value; }
    //    }
    //    public string Field1
    //    {
    //        get { return strField1; }
    //        set { strField1 = value; }
    //    }
    //    public string Field2
    //    {
    //        get { return strField2; }
    //        set { strField2 = value; }
    //    }
    //    public string Field3
    //    {
    //        get { return strField3; }
    //        set { strField3 = value; }
    //    }
    //    public string Field4
    //    {
    //        get { return strField4; }
    //        set { strField4 = value; }
    //    }
    //    public string Description
    //    {
    //        get { return strDescription; }
    //        set { strDescription = value; }
    //    }
    //    public string ServiceUserDisplayName
    //    {
    //        get { return strServiceUserDisplayName; }
    //        set { strServiceUserDisplayName = value; }
    //    }
    //    public string ServiceUserDescription
    //    {
    //        get { return strServiceUserDescription; }
    //        set { strServiceUserDescription = value; }
    //    }
    //    public Int32 UseServerID
    //    {
    //        get { return intUseServer; }
    //        set { intUseServer = value; }
    //    }
    //    public ObjectCommonProperties CommonDetails
    //    {
    //        get { return ocpCommonDetails; }
    //        set { ocpCommonDetails = value; }
    //    }
    //    public ServiceType ServerServiceType
    //    {
    //        get { return serviceType; }
    //        set { serviceType = value; }
    //    }
    //    public Boolean RDPEnetrpriseManager
    //    {
    //        get { return bolRDPEM; }
    //        set { bolRDPEM = value; }
    //    }
    //    public Boolean NamedService
    //    {
    //        get { return bolNamedService; }
    //        set { bolNamedService = value; }
    //    }
    //    public Boolean IsUserLockToConsole
    //    {
    //        get { return bolUserLockToConsole; }
    //        set { bolUserLockToConsole = value; }
    //    }
    //    public Boolean IsPromptUser
    //    {
    //        get { return bolIsPromptUser; }
    //        set { bolIsPromptUser = value; }
    //    }
    //    public Boolean IsPerformSSO
    //    {
    //        get { return bolIsPerformSSO; }
    //        set { bolIsPerformSSO = value; }
    //    }
    //    public Boolean IsWindowsAuth
    //    {
    //        get { return _isWindowsAuth; }
    //        set { _isWindowsAuth = value; }
    //    }

    //    //--- Add Property by Nilesh For Dynamic Port 

    //    public Boolean DynamicPort
    //    {
    //        get
    //        {
    //            return _DynamicPort;
    //        }
    //        set
    //        {
    //            _DynamicPort = value;
    //        }
    //    }

    //    // Added by shabbir for SSH Key Generation
    //    public bool IsKeyEnabled
    //    {
    //        get
    //        {
    //            return _IsKeyEnabled;
    //        }

    //        set
    //        {
    //            _IsKeyEnabled = value;
    //        }
    //    }
    //    public string PuttyWebServiceURL
    //    {
    //        get { return _PuttyWebServiceURL; }
    //        set { _PuttyWebServiceURL = value; }
    //    }
    //    #endregion
    //    public Boolean VaultService
    //    {
    //        get { return bolVaultService; }
    //        set { bolVaultService = value; }
    //    }

    //    public int RoboticImageId
    //    {
    //        get
    //        {
    //            return _RoboticImageId;
    //        }

    //        set
    //        {
    //            _RoboticImageId = value;
    //        }
    //    }


    //    public RDPSServerdetails RDPSServerDetails
    //    {
    //        get
    //        {
    //            return _RDPSServerDetails;
    //        }

    //        set
    //        {
    //            _RDPSServerDetails = value;
    //        }
    //    }



    //    #region Functions
    //    public Servers UpdateUserNameForDomainUser(Servers pValue)
    //    {
    //        try
    //        {
    //            if (IsDomainUserAccount(pValue))
    //            {
    //                pValue.ServiceUserName = pValue.ServiceUserName + "@" + pValue.Domain;
    //            }
    //            return pValue;
    //        }
    //        catch (Exception Ex)
    //        {
    //            throw Ex;
    //        }
    //    }
    //    public String DomainServiceUserName
    //    {
    //        get
    //        {
    //            try
    //            {
    //                return Domain + "\\" + ServiceUserName;
    //            }
    //            catch (Exception Ex)
    //            {
    //                throw Ex;
    //            }
    //        }
    //    }

    //    public bool IsRoboticProcess
    //    {
    //        get
    //        {
    //            return _IsRoboticProcess;
    //        }

    //        set
    //        {
    //            _IsRoboticProcess = value;
    //        }
    //    }

    //    public Boolean IsDomainUserAccount()
    //    {
    //        try
    //        {
    //            if (Domain.ToLower() == "" ||
    //                Domain.ToLower() == "." ||
    //                Domain.ToLower() == HostName.ToLower() ||
    //                Domain.ToLower() == IPAddress_Original.ToLower())
    //            {
    //                return false;
    //            }
    //            return true;
    //        }
    //        catch (Exception Ex)
    //        {
    //            throw Ex;
    //        }
    //    }
    //    public Boolean IsDomainUserAccount(Servers pValue)
    //    {
    //        try
    //        {
    //            if (pValue.Domain.ToLower() == "" ||
    //                pValue.Domain.ToLower() == "." ||
    //                pValue.Domain.ToLower() == pValue.HostName.ToLower() ||
    //                pValue.Domain.ToLower() == pValue.IPAddress_Original.ToLower())
    //            {
    //                return false;
    //            }
    //            return true;
    //        }
    //        catch (Exception Ex)
    //        {
    //            throw Ex;
    //        }
    //    }
    //    public object Clone()
    //    {
    //        return this.MemberwiseClone();
    //    }

    //    public void UpdateServiceDetails(Servers pValue)
    //    {
    //        try
    //        {
    //            String lstrDoNotChangeValue = "<dnc>";

    //            if (pValue.HostName.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                HostName = pValue.HostName;
    //            if (pValue.IPAddress.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                IPAddress = pValue.IPAddress;
    //            if (pValue.Domain.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                Domain = pValue.Domain;
    //            if (pValue.ServerServiceType.ToString().ToLower().IndexOf(lstrDoNotChangeValue) < 0 &&
    //                pValue.ServerServiceType.ToString() != "-1" &&
    //                pValue.ServerServiceType != ServiceType.NONE)
    //                ServerServiceType = pValue.ServerServiceType;
    //            if (pValue.RDPEnetrpriseManager.ToString().ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                RDPEnetrpriseManager = pValue.RDPEnetrpriseManager;
    //            if (pValue.DBInstanceName.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                DBInstanceName = pValue.DBInstanceName;
    //            if (pValue.Port.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                Port = pValue.Port;
    //            if (pValue.ServiceUserName.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                ServiceUserName = pValue.ServiceUserName;
    //            if (pValue.ServicePassword.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                ServicePassword = pValue.ServicePassword;
    //            if (pValue.Field1.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                Field1 = pValue.Field1;
    //            if (pValue.Field2.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                Field2 = pValue.Field2;
    //            if (pValue.Field3.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                Field3 = pValue.Field3;
    //            if (pValue.Field4.ToLower().IndexOf(lstrDoNotChangeValue) < 0)
    //                Field4 = pValue.Field4;
    //        }
    //        catch (Exception Ex)
    //        {
    //            throw Ex;
    //        }
    //    }

    //    //shabbir - For Session Window Title - 5/04/2017


    //    public String GetSessionTitle()
    //    {

    //        try
    //        {

    //            //return GetSessionTitleParam(GetSessionTitleConfiguiration());
    //            return GetSessionTitleParam(ClienttSessionTitleConfig);


    //        }
    //        catch (Exception Ex)
    //        {
    //            MessageBox.Show(Ex.Message, "Error-Session Title", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            return "";
    //        }

    //    }

    //    public String GetSessionTitleParam(string title)
    //    {
    //        try
    //        {
    //            if (title == "" || title == String.Empty)
    //            {
    //                return "IPADDRESS,USER,HOSTNAME,DBINSTANCE,SERVICETYPE";
    //            }
    //            else
    //            {
    //                string[] temptitle = title.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    //                string retTitle = string.Empty;
    //                foreach (string str in temptitle)
    //                {
    //                    switch (str.ToLower())
    //                    {
    //                        case "hostname":
    //                            retTitle += ":" + this.HostName;
    //                            for (int i = 0; i < temptitle.Length; i++)
    //                            {
    //                                if (temptitle[i].ToLower() == "dbinstance")
    //                                {
    //                                    if (DBInstanceName.Length <= 0)
    //                                    {

    //                                    }
    //                                    else
    //                                    {
    //                                        retTitle += "(" + DBInstanceName + ")";
    //                                    }
    //                                }
    //                            }
    //                            break;
    //                        case "ipaddress":
    //                            retTitle += ":" + this.IPAddress_Original;
    //                            break;
    //                        case "user":
    //                            retTitle += "@" + this.ServiceUserName;
    //                            break;
    //                        case "servicetype":
    //                            retTitle += "-" + this.serviceType.ToString();
    //                            break;

    //                    }
    //                }
    //                return retTitle.Substring(1, retTitle.Length - 1);
    //            }
    //        }
    //        catch (Exception Ex)
    //        {
    //            MessageBox.Show(Ex.Message, "Error-Get Session Title", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            return null;
    //        }
    //    }

    //    #endregion
    }
}
