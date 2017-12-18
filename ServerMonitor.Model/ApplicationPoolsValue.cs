using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class ApplicationPoolsValue
    {
        string name;
        string state;
        string user;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string State
        {
            get { return state; }
            set { state = value; }
        }

        [DataMember]
        public string User
        {
            get { return user; }
            set { user = value; }
        }
    }
}
