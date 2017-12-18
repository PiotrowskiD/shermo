using System;
using System.Collections.Generic;
using System.Diagnostics;
using ServerMonitor.Model;
using log4net;
using System.Reflection;

namespace ServerReporterService
{
    class EventViewer
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<EventViewerInfo> CheckParameters()
        {
            List<EventViewerInfo> eventViewerInfos = new List<EventViewerInfo>();
            var today = DateTime.Now;
            string log = "Application";
            EventLog demoLog = new EventLog(log);
            EventLogEntryCollection entries = demoLog.Entries;
            foreach (EventLogEntry entry in entries)
            {
                if ((entry.EntryType.ToString() == "Error" || entry.EntryType.ToString() == "Warning") && entry.TimeGenerated > today.AddMinutes(-4))
                {
                    EventViewerInfo eventViewerInfo = new EventViewerInfo();
                    eventViewerInfo.LogName = entry.EntryType.ToString();
                    eventViewerInfo.EventId = entry.InstanceId.ToString();
                    eventViewerInfo.Message = entry.Message.ToString();
                    eventViewerInfo.Source = entry.Source.ToString();
                    eventViewerInfo.LogDate = entry.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss");
                    eventViewerInfo.Index = entry.Index.ToString();
                    eventViewerInfos.Add(eventViewerInfo);
                }
            }
            logger.Info("Event Viewer logs checked succesfully");
            return eventViewerInfos;
        }
    }
}

