using System;
using System.Diagnostics;
using ServerMonitor.Model;
using log4net;
using System.Reflection;

namespace ServerReporterService
{
    class Performance
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PerformanceInfo CheckParameters()
        {
            PerformanceInfo perfInfo = new PerformanceInfo();
            var today = DateTime.Now;

            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerfomanceInfoData perfData = PsApiWrapper.GetPerformanceInfo();

            CounterSample cs1 = cpuCounter.NextSample();
            System.Threading.Thread.Sleep(100);
            CounterSample cs2 = cpuCounter.NextSample();
            perfInfo.CpuPercentUsage = CounterSample.Calculate(cs1, cs2);

            perfInfo.CommitTotalPages = perfData.CommitTotalPages;
            perfInfo.PageSizeMB = Math.Round((double)perfData.PageSizeBytes / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
            perfInfo.PhysicalAvailableMB = Math.Round((double)perfData.PhysicalAvailableBytes / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
            perfInfo.PhysicalTotalMB = Math.Round((double)perfData.PhysicalTotalBytes / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
            perfInfo.ThreadCount = perfData.ThreadCount;
            perfInfo.ProcessCount = perfData.ProcessCount;
            perfInfo.Time = today.ToString("yyyy-MM-dd HH:mm:ss");
            logger.Info("Performance details chcecked succesfully");
            return perfInfo;
        }

    }
}
