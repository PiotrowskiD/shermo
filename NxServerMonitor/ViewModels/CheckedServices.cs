namespace NxServerMonitor.ViewModels
{
    public class CheckedServices
    {
        public string ServiceName { get; set; }
        public bool IsSelected { get; set; }

        public CheckedServices(string serviceName, bool isSelected)
        {
            ServiceName = serviceName;
            IsSelected = isSelected;
        }
    }
}
