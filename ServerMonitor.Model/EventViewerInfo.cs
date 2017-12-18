using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class EventViewerInfo
    {
        string eventId;
        string index;
        string source;
        string logName;
        string message;
        string logDate;

        [DataMember]
        public string EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }
        [DataMember]
        public string Index
        {
            get { return index; }
            set { index = value; }
        }
        [DataMember]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        [DataMember]
        public string LogName
        {
            get { return logName; }
            set { logName = value; }
        }
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember]
        public string LogDate
        {
            get { return logDate; }
            set { logDate = value; }
        }
    }
}

