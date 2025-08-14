using ArconConnector.BusinessEntities;
using ArconConnector.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;
using Enum = ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessLayer
{
    public class ArconBaseManager
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ArconContext ArconContext { get; set; }

        public ArconBaseManager()
        {

        }

        public ArconBaseManager(ArconContext objArconContext)
        {
            ArconContext = objArconContext;
        }

        public static BaseConnectorManager GetBO(ServiceType serviceType)
        {
            switch (serviceType)
            {
                case ServiceType.MSSQLEMLocal:
                    return new MSSQLManager();
                case ServiceType.AppSQLDeveloperOracle:
                    return new SQLDeveloperOracleManager();
                case ServiceType.AppEnterpriseManagerConsoleOracle:
                    return new EMCOracleManager();
                case ServiceType.AppSQLyog:
                    return new SQLyogManager();
                case ServiceType.AppToadOracle:
                    return new ToadForOracleManager();
                case ServiceType.AppPLSQLDeveloperOracle:
                    return new PlSqlDeveloperManager();
                case ServiceType.AppReflectionX:
                    return new ReflectionXManager();                
                default:
                    return new ExeConnectorManager(); 
            }
        }
    }
}
