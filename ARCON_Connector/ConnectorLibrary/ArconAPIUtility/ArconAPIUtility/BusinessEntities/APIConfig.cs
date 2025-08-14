namespace ArconAPIUtility
{
    public class APIConfig
    {
        public string RequestMethod { get; set; }

        public string RequestContentType { get; set; }

        public string APIBaseUrl { get; set; }

        public string APIUrl { get; set; }

        public APIAuthToken AuthToken { get; set; }
    }
}
