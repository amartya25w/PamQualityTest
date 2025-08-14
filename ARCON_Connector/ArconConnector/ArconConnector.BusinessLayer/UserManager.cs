using ArconConnector.BusinessEntities;
using ArconConnector.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArconConnector.BusinessLayer
{
    public class UserManager : ArconBaseManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserDetails GetUserDetailsBySessionId(int sessionId)
        {
            //Call the API to get below details refer FillSessionDetails method
            try
            {
                ProxyUserDetails objProxyUserDetails = new ProxyUserDetails(ArconContext.APIConfig);
                return objProxyUserDetails.GetUserDetailsBySession(sessionId);
                //return new UserDetails()
                //{
                //    UserId = 35,
                //    UserName = "arcosadmin",
                //    DisplayName = "arcosadmin",
                //    Domain = "ARCOSAUTH",
                //    SessionId = sessionId,
                //    SessionTimeOutTime = 1,
                //    IsSessionAlive = true
                //};
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
                throw ex;
            }
        }

        public string GetUserPreference(int userId, int preferenceId)
        {
            // Call the API to Get User prefernce refer GetUserPreference
            ProxyUserDetails objProxyUserDetails = new ProxyUserDetails(ArconContext.APIConfig);
            return objProxyUserDetails.GetUserPreference(userId, preferenceId);
            //return @"C:\Program Files (x86)\Microsoft SQL Server\140\Tools\Binn\ManagementStudio\Ssms.exe";
            //return @"C:\WINDOWS\system32\notepad.exe";//
        }

        public void EndUserServiceSession(int sessionId)
        {
            // Call the API to update User log details refer EndSessionDB
        }
    }
}
