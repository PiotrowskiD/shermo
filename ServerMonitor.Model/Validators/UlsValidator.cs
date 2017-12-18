using System.Collections.Generic;
using System.Linq;

namespace ServerMonitor.Model.Validators
{
    public class UlsValidator
    {
        public enum outcome
        {
            Critical,
            Error,
            Unexpected,
            OK
        }

        public static outcome validate(UlsViewerInfo ulsViewerInfo)
        {
            switch (ulsViewerInfo.Level)
            {
                case "Unexpected": return outcome.Unexpected;
                case "Critical": return outcome.Critical;                
                case "Error": return outcome.Error;
                default: return outcome.OK;
            }
        }

    }
}