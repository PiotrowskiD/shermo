using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NxServerMonitor.ViewModels
{
    public class ServicesViewModel
    {
        public string ServiceName { get; set; }
        public string DisplayName { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
        public SolidColorBrush Color { get; set; }

        public ServicesViewModel(string serviceName, string displayName, string serviceType, string status, Color color)
        {
            ServiceName = serviceName;
            DisplayName = displayName;
            ServiceType = serviceType;
            Status = status;
            Color = new SolidColorBrush(color);
        }
    }
}
