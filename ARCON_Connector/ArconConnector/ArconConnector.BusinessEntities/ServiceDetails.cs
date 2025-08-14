using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArconConnector.BusinessEntities.Enum;

namespace ArconConnector.BusinessEntities
{
    public class ServiceDetails
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string DomainName { get; set; }
        public string Instance { get; set; }
        public string Port { get; set; }
        public string HostName { get; set; }
        public string IPAddress { get; set; }
        public bool DynamicPort { get; set; }
        public bool AllowRoboticProcess { get; set; }
        public bool AllowRDP { get; set; }
        public bool IsUserLockToConsole { get; set; }
        public string JsonData { get; set; }
        public string FileData { get; set; }
        public string FileExtension { get; set; }
        public string ParameterJSON { get; set; }
        public List<SettingParameter> SettingParameter { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }

    }
}
