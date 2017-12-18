using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxServerMonitor.ViewModels
{
    public class SqlPerfViewModel
    {
        public string LastTime { get; set; }
        public string AverageTime { get; set; }
        public string Message { get; set; }

        public SqlPerfViewModel(string lastTime, double totalTime, double executionCount, string message)
        {
            LastTime = lastTime + "μs";
            AverageTime = decimal.Round(decimal.Parse((totalTime / executionCount).ToString()), 1).ToString() + "μs";
            Message = message;
        }
    }
}