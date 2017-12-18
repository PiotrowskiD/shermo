using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ServerMonitor.Model.Validators;
using NxServerMonitor.ViewModels;
using NxServerMonitor.ServiceReference2;
using System.Collections;

namespace NxServerMonitor
{
    public partial class ServersList : Page
    {
        IService1 service = ServiceFactory.Create();
        GroupModel Group;
        ServerStatusRepository serverStatus;
        public static bool loop = false;

        public ServersList(GroupModel group)
        {
            serverStatus = new ServerStatusRepository(service);
            Group = group;
            DataContext = (Clients)DataContext;
            this.Loaded += (s, e) => init(group);
        }
        void init(GroupModel group)
        {
            InitializeComponent();
            GetServersList();
            Refresh();
        }

        public void GetServersList()
        {
            try
            {
                var serversIcons = new ObservableCollection<ServerViewModel>();
                var serversInGroup = Group.ServersList;
                foreach (var server in serversInGroup)
                {
                    var servers = service.GetServerName(server.IdKey.ToString());
                    {
                        string hostName = server.HostName;
                        string domainName = server.DomainName;
                        string hKey = server.IdKey.ToString();
                        string ip = server.IpAdress;
                        var sn = new ServerViewModel(hostName, domainName, hKey, Group, ip);
                        sn.Color = new SolidColorBrush(Color.FromRgb(88, 232, 123));
                        sn.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:\\Users\\zuziaa\\Desktop\\staf\\nexpertis\\NX_ServerMonitor\\NxServerMonitor\\assets\\status-big-ok.png"));
                        serversIcons.Add(sn);                       
                    }
                }
                serversList.ItemsSource = serversIcons;
            }
            catch { }
        }

        private void selectServer(object sender, RoutedEventArgs e)
        {
            var selectedItem = serversList.SelectedItem as ServerViewModel;
            if (selectedItem == null)
                return;
            string key = selectedItem.Key;
            var server = new ServerView(Group, key);
            var clients = (Clients)DataContext;
            clients.Content = server;
            clients.NavigationService.Navigate(server);
        }

        public async void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            loop = true;
            var servers = Group.ServersList;
            Clients clients = (Clients)DataContext;

            foreach(var server in servers)
            {
                string Key = server.IdKey.ToString();
                var sv = new ServerView(Group, Key);
                sv.DataContext = this;
                clients.Content = sv;
                clients.mainFrame.NavigationService.Navigate(sv);
                await Task.Delay(new TimeSpan(0, 0, 10));
                if (!loop)
                    return;
                if (Key == servers.Last().IdKey.ToString())
                {

                }
            }   
        }

        public async Task CheckServer()
        {
            var items = serversList.Items;
            foreach (ServerViewModel item in items)
            {
                string itemKey = item.Key;
                ConnectionValidator.connection connect = serverStatus.CheckConnection(itemKey);
                if (connect == ConnectionValidator.connection.notOK)
                    item.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:\\Users\\zuziaa\\Desktop\\staf\\nexpertis\\NX_ServerMonitor\\NxServerMonitor\\assets\\status-big-wrong.png"));
                else
                    item.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:\\Users\\zuziaa\\Desktop\\staf\\nexpertis\\NX_ServerMonitor\\NxServerMonitor\\assets\\status-big-ok.png"));

                var disk = new Disk(Group, itemKey);
                await DiskCheck(item, disk, itemKey);

                var cpu = new CPU(Group, itemKey);
                await CpuCheck(item, cpu, itemKey);
            }
        }

        private async Task DiskCheck(ServerViewModel item, Disk disk, string itemKey)
        {
            var diskOK = disk.GetDiskInfo(itemKey);

            switch (diskOK)
            {
                case DiskValidator.diskOutcome.NotOK:
                    item.DiskColor = new SolidColorBrush(Color.FromRgb(192, 0, 2));
                    break;
                case DiskValidator.diskOutcome.Warning:
                    item.DiskColor = new SolidColorBrush(Color.FromRgb(222, 97, 0));
                    break;
                default:
                    item.DiskColor = new SolidColorBrush(Color.FromRgb(99, 255, 74));
                    break;
            }
        }

        private async Task CpuCheck(ServerViewModel item, CPU cpu, string itemKey)
        {
            var cpuOK = cpu.GetPerformanceInfo(itemKey);
            try
            {
                var cpus = service.GetPerformanceStatus(itemKey, 10000).ToArray();
                var ordered = cpus.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
                var cpuPercentUsage = decimal.Round(decimal.Parse(ordered.Last().CpuPercentUsage.ToString()), 2).ToString();
                item.Cpu = "CPU: " + cpuPercentUsage + "%";

                switch (cpuOK)
                {
                    case PerformanceValidator.performanceOutcome.NotOK:
                        item.CpuColor = new SolidColorBrush(Color.FromRgb(192, 0, 2));
                        break;
                    case PerformanceValidator.performanceOutcome.Warning:
                        item.CpuColor = new SolidColorBrush(Color.FromRgb(222, 97, 0));
                        break;
                    default:
                        item.CpuColor = new SolidColorBrush(Color.FromRgb(99, 255, 74));
                        break;
                }
            }
            catch { }
        }

        private void Refresh()
        {
            CheckServer();
            var timer = new DispatcherTimer();
            timer.Tick += async (s, args) => await CheckServer();
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var clients = new Clients();
            this.Content = clients;
            NavigationService.Navigate(clients);
        }

        private void serversList_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = serversList.SelectedItem as ServerViewModel;
            if (selectedItem == null)
                return;
            var assignment = new ServerAssignment(selectedItem);
            assignment.DataContext = this;
            assignment.ShowDialog();
        }
    }
}