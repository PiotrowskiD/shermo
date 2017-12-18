using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class DiskInfo
    {
        string name;
        double totalSize;
        double freeSpace;
        double percentOfFreeSpace;
        string time;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public double TotalSize
        {
            get { return totalSize; }
            set { totalSize = value; }
        }
        [DataMember]
        public double FreeSpace
        {
            get { return freeSpace; }
            set { freeSpace = value; }
        }
        [DataMember]
        public double PercentOfFreeSpace
        {
            get { return percentOfFreeSpace; }
            set { percentOfFreeSpace = value; }
        }
        [DataMember]
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
