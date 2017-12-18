namespace NxServerMonitor.ViewModels
{
    public class EmailConfigViewModel
    {
        public string EmailAddress { get; set; }

        public EmailConfigViewModel(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
