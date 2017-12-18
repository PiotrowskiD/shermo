namespace ServerMonitor.Model.Validators
{
    public class AppPoolsValidator
    {
        public enum outcome
        {
            OK,
            NotOK
        }

        public static outcome validate(ApplicationPoolsValue appPool)
        {
            if (appPool.State == "Stopped")
                return outcome.NotOK;
            else
                return outcome.OK;
        }
    }
}
