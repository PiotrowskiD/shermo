using System;

namespace ServerMonitor.Model.Validators
{
    public class CheckReboot
    {
        public static bool validate(ServerOtherParams reboot)
        {
            bool ifRequired = Convert.ToBoolean(reboot.Value);
            return ifRequired;
        }

    }
}
