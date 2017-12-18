namespace ServerMonitor.Model.Validators
{
    public class DiskValidator
    {
        public enum diskOutcome
        {
            OK,
            Warning,
            NotOK,
        }

        public static diskOutcome validate(DiskInfo disk)
        {
            double percentFreeSpace = disk.PercentOfFreeSpace;

            if (percentFreeSpace < 10)
                return diskOutcome.NotOK;
            else if (percentFreeSpace < 30)
                return diskOutcome.Warning;
            else
                return diskOutcome.OK;
        }
    }
}