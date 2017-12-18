using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System;
using ServerMonitor.Model.Validators;
using NxServerMonitor.ServiceReference2;
using NxServerMonitor.ViewModels;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for Disk.xaml
    /// </summary>
    public partial class Disk : Page
    {
        IService1 service = ServiceFactory.Create();
        GroupModel Group;

        public Disk(GroupModel group, string ClientID)
        {
            Group = group;
            InitializeComponent();
            GetDiskInfo(ClientID);
        }

        public Disk(GroupModel group, string ClientID, string name)
        {
            InitializeComponent();
            SetDiskTable(ClientID, name);
        }

        private void SetDiskTable(string ClientID, string name)
        {
            var allDisks = service.GetDisksStatus(ClientID, 60).ToArray();
            var disks = allDisks.GroupBy(x => x.Name).Select(grp => grp.ToList()).ToList();

            foreach (var disk in disks)
            {
                var ordered = disk.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
                ordered.Reverse();

                foreach (var o in ordered)
                {
                    if (o.Name == name)
                    {
                        string checkTime = o.Time;
                        string name1 = o.Name;
                        string totalSize = o.TotalSize.ToString();
                        string freeSpace = o.FreeSpace.ToString();
                        double percentFreeSpace = o.PercentOfFreeSpace;
                        RowCreator(checkTime, name1, totalSize, freeSpace, percentFreeSpace.ToString());
                    }
                }
            }
        }

        public DiskValidator.diskOutcome GetDiskInfo(string ClientID)
        {
            var isOk = DiskValidator.diskOutcome.OK;
            var disks = service.GetDisksStatus(ClientID, 30).ToArray();
            var ordered = disks.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
            ordered.Reverse();

            foreach (var o in ordered)
            {
                string checkTime = o.Time;
                string name = o.Name;
                string totalSize = o.TotalSize.ToString();
                string freeSpace = o.FreeSpace.ToString();
                double percentFreeSpace = o.PercentOfFreeSpace;
                RowCreator(checkTime, name, totalSize, freeSpace, percentFreeSpace.ToString());
                isOk = DiskValidator.validate(o);
            }
            return isOk;
        }

        private void RowCreator(string CheckTime, string Name, string TotalSize, string FreeSpace, string PercentOfFreeSpace)
        {
            var tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(CheckTime))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(Name))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(TotalSize))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(FreeSpace))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(PercentOfFreeSpace))));

            var trg = new TableRowGroup();
            trg.Rows.Add(tr);

            DiskTable.RowGroups.Add(trg);
        }

    }
}