namespace ServerMonitor.Model
{
    public class URLValue
    {
        public string Url { get; set; }
        public string Type { get; set; }

        public URLValue(string url, string type)
        {
            Url = url;
            Type = type;
        }
    }
}
