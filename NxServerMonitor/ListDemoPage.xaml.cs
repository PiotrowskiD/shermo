using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NxServerMonitor
{
    class O
    {
        public SolidColorBrush Color { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public O(Color color, string id, string title, string description)
        {
            this.Color = new SolidColorBrush(color);
            this.Id = id;
            this.Title = title;
            this.Description = description;
        }
    }
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class ListDemoPage : Page
    {
        ObservableCollection<Obj> ServerList { get; set; }
        public ListDemoPage()
        {

            InitializeComponent();

            ServerList = new ObservableCollection<Obj>() {
                new Obj(Colors.Gold,"nxsp01","nexpertis.pl","Sharepoint + Active Directory"),
                new Obj(Colors.Red,"nxdc01","nexpertis.pl","Active Directory"),
                new Obj(Colors.Black,"nxsql01","nexpertis.pl","SQL Server 2016"),
                new Obj(Colors.DarkKhaki,"sp01","novartis.nexpertis.pl","Sharepoint 2013 Foundation"),
                new Obj(Colors.Green,"sql01","novartis.nexpertis.pl","SQL Server 2016"),
                new Obj(Colors.Gold,"nxsp01","nexpertis.pl","Sharepoint + Active Directory"),
                new Obj(Colors.Red,"nxdc01","nexpertis.pl","Active Directory"),
                new Obj(Colors.Black,"nxsql01","nexpertis.pl","SQL Server 2016"),
                new Obj(Colors.DarkKhaki,"sp01","novartis.nexpertis.pl","Sharepoint 2013 Foundation"),
                new Obj(Colors.Green,"sql01","novartis.nexpertis.pl","SQL Server 2016"),
                new Obj(Colors.Gold,"nxsp01","nexpertis.pl","Sharepoint + Active Directory"),
                new Obj(Colors.Red,"nxdc01","nexpertis.pl","Active Directory"),
                new Obj(Colors.Black,"nxsql01","nexpertis.pl","SQL Server 2016"),
                new Obj(Colors.DarkKhaki,"sp01","novartis.nexpertis.pl","Sharepoint 2013 Foundation"),
                new Obj(Colors.Green,"sql01","novartis.nexpertis.pl","SQL Server 2016"),

            };
            lvServers.ItemsSource = ServerList;
        }

        private void btnAddSampleData_Click(object sender, RoutedEventArgs e)
        {
            ServerList.Add(new Obj(Colors.Red, "sp01", "nexpertis.pl", "Sharepoint 2016 \nportal.nexpertis.pl:80"));
        }
    }
}
