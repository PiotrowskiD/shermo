using System;
using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class ServerInfo
    {
        string hostName;
        string domainName;
        string ipAdress;
        Guid idKey;

        [DataMember]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }
        [DataMember]
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }
        [DataMember]
        public Guid IdKey
        {
            get { return idKey; }
            set { idKey = value; }
        }
        [DataMember]
        public string IpAdress
        {
            get { return ipAdress; }
            set { ipAdress = value; }
        }
    }
}
