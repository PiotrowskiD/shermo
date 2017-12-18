using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class HttpServicesValue
    {
        string date;
        string url;
        string type;
        string status;
        double time;
        string data;
        string extra;

        [DataMember]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        [DataMember]
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        [DataMember]
        public double Time
        {
            get { return time; }
            set { time = value; }
        }
        [DataMember]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        [DataMember]
        public string Extra
        {
            get { return extra; }
            set { extra = value; }
        }
    }
}
