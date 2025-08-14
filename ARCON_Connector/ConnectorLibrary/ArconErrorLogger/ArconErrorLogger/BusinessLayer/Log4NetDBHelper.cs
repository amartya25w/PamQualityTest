using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Linq;

namespace ArconErrorLogger.BusinessLayer
{
    public static class Log4NetDBHelper
    {
        public static void ConfigureDBAppender(Hierarchy objHierarchy, string cs)
        {
            Hierarchy h = objHierarchy;
            if (objHierarchy.GetAppenders().Any(app => app.Name == "ADONetAppender"))
            {
                var appender = (AdoNetAppender)objHierarchy.GetAppenders().FirstOrDefault(app => app.Name == "ADONetAppender");
                if (string.IsNullOrEmpty(appender.ConnectionString))
                {
                    IAppender ado = CreateAdoNetAppender(appender, cs);
                    h.Root.AddAppender(ado);
                    h.Configured = true;
                    log4net.Config.BasicConfigurator.Configure(ado);
                }
            }
        }

        private static IAppender CreateAdoNetAppender(AdoNetAppender appender, string cs)
        {
            appender.Name = "AdoNetAppender";
            appender.BufferSize = 1;
            appender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version = 1.0.3300.0, Culture = neutral, PublicKeyToken = b77a5c561934e089";
            appender.ConnectionString = cs;
            appender.CommandType = System.Data.CommandType.StoredProcedure;
            appender.CommandText = "usp_SaveLogs";

            AddDateTimeParameterToAppender(appender, "@log_date");
            AddStringParameterToAppender(appender, "@thread", 20, "%thread");
            AddStringParameterToAppender(appender, "@log_level", 10, "%level");
            AddStringParameterToAppender(appender, "@namespace", 200, "%logger");
            AddStringParameterToAppender(appender, "@message", 1000, "%message");
            AddStringParameterToAppender(appender, "@ipaddress", 1000, "%aspnet-request{AUTH_USER}");
            AddStringParameterToAppender(appender, "@class", 1000, "%class");
            AddStringParameterToAppender(appender, "@method", 1000, "%method");
            AddErrorParameterToAppender(appender, "@exception", 4000);
            appender.ActivateOptions();
            return appender;
        }

        private static void AddStringParameterToAppender(this AdoNetAppender appender, string paramName,
                                                         int size, string conversionPattern)
        {
            AdoNetAppenderParameter param = new AdoNetAppenderParameter();
            param.ParameterName = paramName;
            param.DbType = System.Data.DbType.String;
            param.Size = size;
            param.Layout = new Layout2RawLayoutAdapter(new PatternLayout(conversionPattern));
            appender.AddParameter(param);
        }

        private static void AddDateTimeParameterToAppender(this AdoNetAppender appender, string paramName)
        {
            AdoNetAppenderParameter param = new AdoNetAppenderParameter();
            param.ParameterName = paramName;
            param.DbType = System.Data.DbType.DateTime;
            param.Layout = new RawUtcTimeStampLayout();
            appender.AddParameter(param);
        }

        private static void AddErrorParameterToAppender(this AdoNetAppender appender, string paramName, int size)
        {
            AdoNetAppenderParameter param = new AdoNetAppenderParameter();
            param.ParameterName = paramName;
            param.DbType = System.Data.DbType.String;
            param.Size = size;
            param.Layout = new Layout2RawLayoutAdapter(new ExceptionLayout());
            appender.AddParameter(param);
        }
    }
}
