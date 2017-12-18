namespace NxServerMonitor.ViewModels
{
    public class CheckedAppPools
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public CheckedAppPools(string name, bool isSelected)
        {
            Name = name;
            IsSelected = isSelected;
        }
    }
}
