namespace ServerMonitor.Model.Validators
{
    public class ServicesValidator
    {
        public enum outcome
        {
            OK,
            NotOK
        }

        public static outcome validate(WindowsServicesValue service)
        {
            if (service.Status == "Stopped")
                return outcome.NotOK;
            else
                return outcome.OK;
        }

    }
}
