namespace NxServerMonitor.ViewModels
{
    public class ServerManagementViewModel
    {
        public string Info { get; set; }
        public string DomainName { get; set; }
        public string IpAddress { get; set; }
        public bool IsSelected { get; set; }

        public ServerManagementViewModel(string domainName, string ipAddress, bool isSelected)
        {
            DomainName = domainName;
            IpAddress = ipAddress;
            IsSelected = isSelected;
            Info = DomainName + " " + IpAddress;
        }
    }
}
