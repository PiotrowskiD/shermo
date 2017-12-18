//using NxServerMonitor.ServiceReference1;
//using NxServerMonitor.ViewModels;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Threading;
//using ServerMonitor.Model.Validators;

//namespace NxServerMonitor
//{
//    public partial class Server1 : Page
//    {
//        IService1 service = ServiceFactory.Create();
//        GroupModel Group;
//        public Server1(GroupModel group, string key)
//        {
//            Group = group;
//            InitializeComponent();
//            GetServerInfo(key);
//            Refresh(key);
//        }

//        public void Refresh(string key)
//        {
//            var cpu = new CPU(Group, key);
//            var disk = new Disk(Group, key);
//            var windowsEvents = new WindowsEvents(Group, key);
//            var uls = new ULS(Group, key);
//            UpdateInfos(key, disk, cpu, windowsEvents, uls);
//            var timer = new DispatcherTimer();
//            timer.Tick += (s, args) => UpdateInfos(key, disk, cpu, windowsEvents, uls);
//            timer.Interval = new TimeSpan(0, 0, 10);
//            timer.Start();
//        }

//        private void UpdateInfos(string key, Disk DISK, CPU CPU, WindowsEvents EVENTS, ULS ULS)
//        {
//            // bool diskOK = DISK.GetDiskInfo(key);
//            //if (!diskOK)
//            //    disk.Background = Brushes.OrangeRed;

//            //bool cpuOK = CPU.GetPerformanceInfo(key);
//            //if (!cpuOK)
//            //    cpu.Background = Brushes.OrangeRed;

//            bool evOK = EVENTS.GetEventsInfo(key);
//            if (!evOK)
//                windowsEvents.Background = Brushes.OrangeRed;

//            bool ulsOK = ULS.GetULSInfo(key);
//            if (!ulsOK)
//                uls.Background = Brushes.OrangeRed;

//            ConnectionValidator.connection connection = CheckConnection(key);
//            if (connection == ConnectionValidator.connection.notOK)
//            {
//                disk.Background = Brushes.Red;
//                cpu.Background = Brushes.Red;
//                windowsEvents.Background = Brushes.Red;
//                uls.Background = Brushes.Red;
//            }
//        }

//        public static ConnectionValidator.connection CheckConnection(string ClientID)
//        {

//            //try
//            //{
//            var service = ServiceFactory.Create();
//            var allData = service.GetPerformanceStatus(ClientID, 1000).ToArray();
//            ConnectionValidator.connection isOK = ConnectionValidator.validate(allData);
//            return isOK;
//            //    var list = allData.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
//            //    var lastDate = DateTime.ParseExact(list.Last().Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
//            //    var now = DateTime.Now;
//            //    var span = now.Subtract(lastDate);
//            //    if (span.Minutes > 5)

//            //        isOK = false;

//            //    else
//            //        isOK = true;

//            //    return isOK;
//            //}
//            //catch (Exception e)
//            //{
//            //    return isOK = false;
//            //}
//        }

//        public static async Task<bool> CheckConnectionAsync(string ClientID)
//        {
//            bool isOK = true;
//            var service = ServiceFactory.Create();
//            var allData = service.GetPerformanceStatus(ClientID, 1000).ToArray();
//            var list = allData.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
//            var lastDate = DateTime.ParseExact(list.Last().Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
//            var now = DateTime.Now;
//            var span = now.Subtract(lastDate);
//            if (span.Minutes > 5)
//            {
//                isOK = false;
//            }
//            return isOK;
//        }

//        private void GetServerInfo(string hKey)
//        {
//            var server = service.GetServerName(hKey);

//            string hostName = server.HostName;
//            string domainName = server.DomainName;
//            string Key = server.IdKey.ToString();

//            SetName(hostName, domainName, Key);
//        }

//        private void SetName(string hName, string dName, string hKey)
//        {
//            textBox.Text = "SERVER" + " " + hName + " " + dName;
//            disk.CommandParameter = hKey;
//            cpu.CommandParameter = hKey;
//            windowsEvents.CommandParameter = hKey;
//            uls.CommandParameter = hKey;
//        }

//        private void disk_Click(object sender, RoutedEventArgs e)
//        {
//            Disk Desk = new Disk(Group, disk.CommandParameter.ToString());
//            this.Content = Desk;
//            mainFrame.NavigationService.Navigate(Desk);
//        }

//        private void cpu_Click(object sender, RoutedEventArgs e)
//        {
//            CPU Cpu = new CPU(Group, cpu.CommandParameter.ToString());
//            this.Content = Cpu;
//            mainFrame.NavigationService.Navigate(Cpu);
//        }

//        private void windowsEvents_Click(object sender, RoutedEventArgs e)
//        {
//            WindowsEvents WindowsEvents = new WindowsEvents(Group, windowsEvents.CommandParameter.ToString());
//            this.Content = WindowsEvents;
//            mainFrame.NavigationService.Navigate(WindowsEvents);
//        }

//        private void uls_Click(object sender, RoutedEventArgs e)
//        {
//            ULS Uls = new ULS(Group, uls.CommandParameter.ToString());
//            this.Content = Uls;
//            mainFrame.NavigationService.Navigate(Uls);
//        }

//        private void button_Click(object sender, RoutedEventArgs e)
//        {
//            ServersList servers = new ServersList(Group);
//            this.Content = servers;
//            mainFrame.NavigationService.Navigate(servers);
//        }
//    }
//}