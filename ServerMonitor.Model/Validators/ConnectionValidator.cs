using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Model.Validators
{
    public class ConnectionValidator
    {
        public enum connection
        {
            OK,
            notOK,
        }

        public static connection validate(PerformanceInfo[] performanceInfo)
        {
            connection isOK;
            try
            {
                var list = performanceInfo.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
                var lastDate = DateTime.ParseExact(list.Last().Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                var now = DateTime.Now;
                var span = now.Subtract(lastDate);
                if (span.Minutes > 5)

                    isOK = connection.notOK;

                else
                    isOK = connection.OK;

                return isOK;
            }
            catch (Exception e)
            {
                return isOK = connection.notOK;
            }
        }

    }
}
