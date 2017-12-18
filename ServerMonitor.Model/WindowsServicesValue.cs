using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class WindowsServicesValue
    {
        string serviceName;
        string displayName;
        string serviceType;
        string status;

        [DataMember]
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        [DataMember]
        public string ServiceType
        {
            get { return serviceType; }
            set { serviceType = value; }
        }
    }
}
