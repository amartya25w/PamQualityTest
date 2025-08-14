using ArconAPIUtility;
using ARCONPAMSecurity;
using log4net;
using System;
using System.Net;
using System.Reflection;

namespace ArconConnector.ServiceLayer
{
    public class ProxyARCOSWebDT
    {
        private ARCOSWebDT.ARCOSWebDT _ARCOSWebDT = new ARCOSWebDT.ARCOSWebDT();
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string strEndeKey;
        private APIConfig _WebDTAPIConfig;

        public ProxyARCOSWebDT(APIConfig objWebDTAPIConfig)
        {
            _WebDTAPIConfig = objWebDTAPIConfig;
            InitializeClass();
        }

        private void InitializeClass()
        {
            try
            {
                _ARCOSWebDT.Url = _WebDTAPIConfig.APIUrl;
                _ARCOSWebDT.Timeout = 60000;
                if (_ARCOSWebDT.Url.ToUpper().StartsWith("HTTPS"))
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });

                EncryptionDecryption_Web objEncryption = new EncryptionDecryption_Web();
                strEndeKey = objEncryption.Decrypt(_ARCOSWebDT.InitializeConnection(_WebDTAPIConfig.AuthToken.APIPassword));
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString() == "The underlying connection was closed: An unexpected error occurred on a receive.")        //Meena 08052017 In case of TLS1.2 harden app server is not able to make successfull connection through webservice
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                _Log.Error(ex);
                throw ex;
            }
        }

        public string IsADAuthenticatedWithARCOSLogin(int authType, string domain, string userName, string password, string domainName)
        {
            try
            {
                EncryptionDecryption_Web objEncryption = new EncryptionDecryption_Web(strEndeKey);
                domain = objEncryption.Encrypt(domain);
                userName = objEncryption.Encrypt(userName);
                password = objEncryption.Encrypt(password);
                domainName = objEncryption.Encrypt(domainName);
                return objEncryption.Decrypt(_ARCOSWebDT.IsADAuthenticatedWithARCOSLogin(authType, domain, userName, password, domainName));
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }
    }
}
