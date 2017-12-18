using System.Windows.Media;

namespace NxServerMonitor.ViewModels
{
    public class EventsViewModel
    {
        public SolidColorBrush Color { get; set; }
        public string Count { get; set; }
        public string ServerKey { get; set; }

        public EventsViewModel(Color color, string count, string key)
        {
            Color = new SolidColorBrush(color);
            Count = count;
            ServerKey = key;
        }
    }
}
