using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class PerformanceInfo
    {
        double cpuPercentUsage;
        double commitTotalPages;
        double physicalTotalMB;
        double physicalAvailableMB;
        double pageSizeMB;
        int processCount;
        int threadCount;
        string time;

        [DataMember]
        public double CpuPercentUsage
        {
            get { return cpuPercentUsage; }
            set { cpuPercentUsage = value; }
        }
        [DataMember]
        public double CommitTotalPages
        {
            get { return commitTotalPages; }
            set { commitTotalPages = value; }
        }
        [DataMember]
        public double PhysicalTotalMB
        {
            get { return physicalTotalMB; }
            set { physicalTotalMB = value; }
        }
        [DataMember]
        public double PhysicalAvailableMB
        {
            get { return physicalAvailableMB; }
            set { physicalAvailableMB = value; }
        }
        [DataMember]
        public double PageSizeMB
        {
            get { return pageSizeMB; }
            set { pageSizeMB = value; }
        }
        [DataMember]
        public int ProcessCount
        {
            get { return processCount; }
            set { processCount = value; }
        }

        [DataMember]
        public int ThreadCount
        {
            get { return threadCount; }
            set { threadCount = value; }
        }
        [DataMember]
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
