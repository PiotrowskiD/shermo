using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NxServerMonitor.ViewModels
{
    public class ServerViewModel
    {
        public SolidColorBrush Color { get; set; }
        public string HostName { get; set; }
        public string DomainName { get; set; }
        public string Key { get; set; }
        public GroupModel Group { get; set; }
        public string Disk { get; set; }
        public SolidColorBrush DiskColor { get; set; }
        public string Cpu { get; set; }
        public string IP { get; set; }
        public SolidColorBrush CpuColor { get; set; }
        public ImageSource Icon { get; set; }

        public ServerViewModel(string hostName, string domainName, string key, GroupModel group, string ip)
        {
            HostName = hostName;
            DomainName = domainName;
            Key = key;
            Group = group;
            IP = ip;
        }
    }
}
