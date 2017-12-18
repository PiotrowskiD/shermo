using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxServerMonitor.ViewModels
{
    class CheckedGroup
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public CheckedGroup(string name, bool isSelected)
        {
            Name = name;
            IsSelected = isSelected;
        }
    }
}
