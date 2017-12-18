using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NxServerMonitor.ViewModels
{
    public class AppPoolsViewModel
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string User { get; set; }
        public SolidColorBrush Color { get; set; }
        public ImageSource Icon { get; set; }

        public AppPoolsViewModel(string name, string state, string user, Color color)
        {
            Name = name;
            State = state;
            User = user;
            Color = new SolidColorBrush(color);
        }

    }
}
