namespace ServerMonitor.Model.Validators
{
    public class PerformanceValidator
    {
        public enum performanceOutcome
        {
            OK,
            Warning,
            NotOK,
        }

        public static performanceOutcome validate(PerformanceInfo performance)
        {
            double usage = performance.CpuPercentUsage;

            if (usage > 90)
                return performanceOutcome.NotOK;
            else if (usage > 70)
                return performanceOutcome.Warning;
            else
                return performanceOutcome.OK;
        }
    }
}