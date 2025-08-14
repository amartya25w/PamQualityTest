using ArconAPIUtility;

namespace ArconConnector.BusinessEntities
{
    public class ArconContext
    {
        public int SessionId { get; set; }
        public long ServiceId { get; set; }
        public int UserId { get; set; }
        public string DBConnectionString { get; set; }
        public string RDPDBConnectionString { get; set; }
        public string RADBConnectionString { get; set; }
        public APIConfig APIConfig { get; set; }
        public APIConfig WebDTAPIConfig { get; set; }
    }
}
