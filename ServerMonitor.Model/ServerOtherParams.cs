using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class ServerOtherParams
    {
        string lastUpdate;
        string title;
        string _value;

        [DataMember]
        public string LastUpdate
        {
            get { return lastUpdate; }
            set { lastUpdate = value; }
        }
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
