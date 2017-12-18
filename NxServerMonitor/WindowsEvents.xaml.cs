using NxServerMonitor.ServiceReference2;
using NxServerMonitor.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NxServerMonitor
{
    public partial class WindowsEvents : Page
    {
        IService1 service = ServiceFactory.Create();
        GroupModel Group;

        public WindowsEvents(GroupModel group, string ClientID)
        {
            Group = group;
            InitializeComponent();
            GetEventsInfo(ClientID);
        }

        public bool GetEventsInfo(string ClientID)
        {
            bool isOk = true;

            var events = service.GetEventViewerStatus(ClientID, 30).ToArray();
            var ordered = events.OrderBy(x => (DateTime.ParseExact(x.LogDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
            ordered.Reverse();

            foreach (var o in ordered)
            {
                string logDate = o.LogDate;
                string eventIndex = o.Index;
                string logName = o.LogName;
                string eventId = o.EventId;
                string eventSource = o.Source;
                string eventMessage = o.Message;
                RowCreator(logDate, eventIndex, logName, eventId, eventSource, eventMessage);
            }
            return isOk;
        }

        private void RowCreator(string LogDate, string EventIndex, string LogName, string EventId, string EventSource, string EventMessage)
        {
            TableRow tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(LogDate))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(EventIndex))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(LogName))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(EventId))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(EventSource))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(EventMessage))));

            TableRowGroup trg = new TableRowGroup();
            trg.Rows.Add(tr);

            WETable.RowGroups.Add(trg);
        }
    }
}