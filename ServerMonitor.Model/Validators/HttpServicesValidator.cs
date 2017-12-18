namespace ServerMonitor.Model.Validators
{
    public class HttpServicesValidator
    {
        public enum outcome
        {
            OK,
            NotOK
        }

        public static outcome validate(HttpServicesValue service)
        {
            if (service.Status != "OK")
                return outcome.NotOK;
            else
                return outcome.OK;
        }

    }
}
