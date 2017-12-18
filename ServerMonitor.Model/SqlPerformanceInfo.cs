using System.Runtime.Serialization;

namespace ServerMonitor.Model
{
    [DataContract]
    public class SqlPerformanceInfo
    {
        string message;
        string executionCount;
        string totalLogicalReads;
        string lastLogicalReads;
        string totalLogicalWrites;
        string lastLogicalWrites;
        string totalWorkerTime;
        string lastWorkerTime;
        string totalElapsedTime;
        string lastElapsedTime;
        string lastExecutionTime;
        string queryPlan;

        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember]
        public string ExecutionCount
        {
            get { return executionCount; }
            set { executionCount = value; }
        }
        [DataMember]
        public string TotalLogicalReads
        {
            get { return totalLogicalReads; }
            set { totalLogicalReads = value; }
        }
        [DataMember]
        public string LastLogicalReads
        {
            get { return lastLogicalReads; }
            set { lastLogicalReads = value; }
        }
        [DataMember]
        public string TotalLogicalWrites
        {
            get { return totalLogicalWrites; }
            set { totalLogicalWrites = value; }
        }
        [DataMember]
        public string LastLogicalWrites
        {
            get { return lastLogicalWrites; }
            set { lastLogicalWrites = value; }
        }
        [DataMember]
        public string TotalWorkerTime
        {
            get { return totalWorkerTime; }
            set { totalWorkerTime = value; }
        }
        [DataMember]
        public string LastWorkerTime
        {
            get { return lastWorkerTime; }
            set { lastWorkerTime = value; }
        }
        [DataMember]
        public string TotalElapsedTime
        {
            get { return totalElapsedTime; }
            set { totalElapsedTime = value; }
        }
        [DataMember]
        public string LastElapsedTime
        {
            get { return lastElapsedTime; }
            set { lastElapsedTime = value; }
        }
        [DataMember]
        public string LastExecutionTime
        {
            get { return lastExecutionTime; }
            set { lastExecutionTime = value; }
        }
        [DataMember]
        public string QueryPlan
        {
            get { return queryPlan; }
            set { queryPlan = value; }
        }
    }
}
