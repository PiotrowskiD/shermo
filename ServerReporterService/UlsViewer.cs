using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Diagnostics;
using ServerMonitor.Model;
using log4net;
using System.Reflection;

namespace ServerReporterService
{
    class UlsViewer
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<UlsViewerInfo> CheckParameters()
        {
            List<UlsViewerInfo> ulsViewerInfos = new List<UlsViewerInfo>();
            UlsViewerInfo ulsViewerInfo;

            var ulsLogEntries = new SPULSRetriever(300, 1000, DateTime.Now.AddMinutes(-4)).GetULSEntries(new Guid());
            foreach (var log in ulsLogEntries)
            {
                ulsViewerInfo = new UlsViewerInfo();
                ulsViewerInfo.Category = log.Category;
                ulsViewerInfo.EventId = log.EventID;
                ulsViewerInfo.Level = log.Level;
                ulsViewerInfo.Message = log.Message;
                ulsViewerInfo.Process = log.Process;
                ulsViewerInfo.Thread = log.ThreadID.ToString();
                ulsViewerInfo.Time = log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                ulsViewerInfos.Add(ulsViewerInfo);
            }
            logger.Info("Uls logs checked succesfully");
            return ulsViewerInfos;
        }
    }
}
