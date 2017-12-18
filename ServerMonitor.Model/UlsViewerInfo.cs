using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class UlsViewerInfo
    {
        string time;
        string process;
        string thread;
        string category;
        string eventId;
        string level;
        string message;

        [DataMember]
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        [DataMember]
        public string Process
        {
            get { return process; }
            set { process = value; }
        }
        [DataMember]
        public string Thread
        {
            get { return thread; }
            set { thread = value; }
        }
        [DataMember]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        [DataMember]
        public string EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }
        [DataMember]
        public string Level
        {
            get { return level; }
            set { level = value; }
        }
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
