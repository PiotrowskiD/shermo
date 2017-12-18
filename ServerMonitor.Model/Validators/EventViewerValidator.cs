using System.Collections.Generic;
using System.Linq;

namespace ServerMonitor.Model.Validators
{
    public class EventViewerValidator
    {
        public enum outcome
        {
            OK,
            Warning,
            NotOK
        }

        public static outcome validate(List<EventViewerInfo> eventViewerInfo)
        {
            var groups = eventViewerInfo.GroupBy(x => x.LogName).Select(grp => grp.ToList()).ToList();
            int errors = 0;

            foreach (var g in groups)
            {
                var last = g.Last();

                if (last.LogName == "Error")
                {
                    errors = g.Count();
                }
            }

            if (errors > 15)
                return outcome.NotOK;
            if (errors > 5)
                return outcome.Warning;
            else
                return outcome.OK;
        }
    }
}