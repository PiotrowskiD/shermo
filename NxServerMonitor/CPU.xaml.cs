using NxServerMonitor.ServiceReference2;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ServerMonitor.Model.Validators;
using NxServerMonitor.ViewModels;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for CPU.xaml
    /// </summary>
    public partial class CPU : Page
    {
        IService1 service = ServiceFactory.Create();
        GroupModel Group;

        public CPU(GroupModel group, string ClientID)
        {
            Group = group;
            InitializeComponent();
            GetPerformanceInfo(ClientID);
        }

        public PerformanceValidator.performanceOutcome GetPerformanceInfo(string ClientID)
        {
            var isOk= PerformanceValidator.performanceOutcome.OK;
            var cpus = service.GetPerformanceStatus(ClientID, 100000).ToArray();
            var ordered = cpus.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
            ordered.Reverse();

            foreach (var o in ordered)
            {
                string cpuPercentUsage = o.CpuPercentUsage.ToString();
                string commitTotalPages = o.CommitTotalPages.ToString();
                string physicalTotalMB = o.PhysicalTotalMB.ToString();
                string physicalAvailableMB = o.PhysicalAvailableMB.ToString();
                string pageSize = o.PageSizeMB.ToString();
                string processCount = o.ProcessCount.ToString();
                string threadCount = o.ThreadCount.ToString();
                string time = o.Time.ToString();
                RowCreator(time, cpuPercentUsage, commitTotalPages, physicalTotalMB, physicalAvailableMB, pageSize, processCount, threadCount);
                isOk = PerformanceValidator.validate(o);            
            }
            return isOk;            
        }

        private void RowCreator(string CheckTime, string CpuPercentUsage, string CommitTotalPages, string PhysicalTotalMB, string PhysicalAvailableMB, string PageSizeMB, string ProcessCount, string ThreadCount)
        {
            TableRow tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(CheckTime))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(CpuPercentUsage))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(CommitTotalPages))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(PhysicalTotalMB))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(PhysicalAvailableMB))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(PageSizeMB))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(ProcessCount))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(ThreadCount))));

            TableRowGroup trg = new TableRowGroup();
            trg.Rows.Add(tr);
            CPUTable.RowGroups.Add(trg);
        }
    }
}