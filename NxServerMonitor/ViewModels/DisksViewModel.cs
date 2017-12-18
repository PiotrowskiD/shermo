using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NxServerMonitor.ViewModels
{
    public class DisksViewModel
    {
        public string DiskName { get; set; }
        public string Usage { get; set; }
        public SolidColorBrush Color { get; set; }

        public DisksViewModel(Color color, string diskName, string usage)
        {
            Color = new SolidColorBrush(color);
            DiskName = diskName;
            Usage = usage + "%";
        }
    }
}
