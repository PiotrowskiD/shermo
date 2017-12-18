using NxServerMonitor.ServiceReference2;
using NxServerMonitor.ViewModels;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for ULS.xaml
    /// </summary>
    public partial class ULS : Page
    {
        IService1 service = ServiceFactory.Create();
        GroupModel Group;

        public ULS(GroupModel group, string ClientId)
        {
            Group = group;
            InitializeComponent();
            GetULSInfo(ClientId);
        }

        public bool GetULSInfo(string ClientID)
        {
            bool isOk = true;

            var ulss = service.GetUlsViewerStatus(ClientID, 30).ToArray();
            var ordered = ulss.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
            ordered.Reverse();

            foreach (var o in ordered)
            {
                string time = o.Time;
                string process = o.Process;
                string thread = o.Thread;
                string category = o.Category;
                string eventID = o.EventId;
                string level = o.Level;
                string message = o.Message;
                RowCreator(time, process, thread, category, eventID, level, message);
            }

            return isOk;
        }

        private void RowCreator(string Time, string Process, string Thread, string Category, string EventId, string Level, string Message)
        {
            var tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(Time))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(Process))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(Thread))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(Category))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(EventId))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(Level))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(Message))));

            var trg = new TableRowGroup();
            trg.Rows.Add(tr);

            ULSTable.RowGroups.Add(trg);
        }
    }
}