using System;
using System.Linq;
using System.Windows.Media;

namespace NxServerMonitor.ViewModels
{
    public class ClientViewModel
    {
        public SolidColorBrush Color { get; set; }
        public string GroupName { get; set; }
        public GroupModel GroupId { get; set; }
        

        public ClientViewModel(GroupModel groupId)
        {
          //  Color = RandomColor();
            GroupId = groupId;
        }

        //private SolidColorBrush RandomColor()
        //{
        //    var r = new Random();
        //    var properties = typeof(Brushes).GetProperties();
        //    var count = properties.Count();

        //    var colour = properties
        //                .Select(x => new { Property = x, Index = r.Next(count) })
        //                .OrderBy(x => x.Index)
        //                .First();

        //    return (SolidColorBrush)colour.Property.GetValue(colour, null);
        //}
    }
}