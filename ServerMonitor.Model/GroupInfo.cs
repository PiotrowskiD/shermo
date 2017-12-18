using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class GroupInfo
    {
        string groupName;
        string groupId;

        [DataMember]
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        [DataMember]
        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
    }
}
