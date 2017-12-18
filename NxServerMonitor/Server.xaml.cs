using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for Server.xaml
    /// </summary>
    public partial class Server : Page
    {
        ServiceReference1.Service1Client service = new ServiceReference1.Service1Client();
        string Group;
        public Server(string group, string key)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            service.ClientCredentials.UserName.UserName = "ServerReporter";
            service.ClientCredentials.UserName.Password = "Damaz!23";
            Group = group;
            InitializeComponent();
            GetServerInfo(key);
            refresh(key);
        }

        public void refresh(string key)
        {
            CPU cpu = new CPU(Group, key);
            Disk disk = new Disk(Group, key);
            WindowsEvents windowsEvents = new WindowsEvents(Group, key);
            ULS uls = new ULS(Group, key);
            updateInfos(key, disk, cpu, windowsEvents, uls);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (s, args) => updateInfos(key, disk, cpu, windowsEvents, uls);
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();
        }

        private void updateInfos(string key, Disk DISK, CPU CPU, WindowsEvents EVENTS, ULS ULS)
        {
           // bool diskOK = DISK.GetDiskInfo(key);
            //if (!diskOK)
            //    disk.Background = Brushes.OrangeRed;

            //bool cpuOK = CPU.GetPerformanceInfo(key);
            //if (!cpuOK)
            //    cpu.Background = Brushes.OrangeRed;

            bool evOK = EVENTS.GetEventsInfo(key);
            if (!evOK)
                windowsEvents.Background = Brushes.OrangeRed;

            bool ulsOK = ULS.GetULSInfo(key);
            if (!ulsOK)
                uls.Background = Brushes.OrangeRed;

            bool connection = checkConnection(key);
            if (!connection)
            {
                disk.Background = Brushes.Red;
                cpu.Background = Brushes.Red;
                windowsEvents.Background = Brushes.Red;
                uls.Background = Brushes.Red;
            }
        }
        public static bool checkConnection(string ClientID)
        {
            bool isOK = true;
            ServiceReference1.Service1Client service = new ServiceReference1.Service1Client();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            service.ClientCredentials.UserName.UserName = "ServerReporter";
            service.ClientCredentials.UserName.Password = "Damaz!23";
            var allData = service.GetPerformanceStatus(ClientID).ToArray();
            var list = allData.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
            DateTime lastDate = DateTime.ParseExact(list.Last().Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime now = DateTime.Now;
            TimeSpan span = now.Subtract(lastDate);
            if (span.Minutes > 5)
            {
                isOK = false;
            }
            return isOK;
        }
        public static async Task<bool> checkConnectionAsync(string ClientID)
        {
            bool isOK = true;
            ServiceReference1.Service1Client service = new ServiceReference1.Service1Client();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            service.ClientCredentials.UserName.UserName = "ServerReporter";
            service.ClientCredentials.UserName.Password = "Damaz!23";
            var allData = service.GetPerformanceStatus(ClientID).ToArray();
            var list = allData.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture))).ToList();
            DateTime lastDate = DateTime.ParseExact(list.Last().Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime now = DateTime.Now;
            TimeSpan span = now.Subtract(lastDate);
            if (span.Minutes > 5)
            {
                isOK = false;
            }
            return isOK;
        }

        private void GetServerInfo(string hKey)
        {
            var servers = service.GetServerName(hKey).ToArray();

            foreach (var server in servers)
            {
                string hostName = server.HostName;
                string domainName = server.DomainName;
                string Key = server.IdKey.ToString();
               
                if (Key == hKey)
                    setName(hostName, domainName, Key);
            }
        }

        private void setName(string hName, string dName, string hKey)
        {
            textBox.Text = "SERVER" + " " + hName + " " + dName;
            disk.CommandParameter = hKey;
            cpu.CommandParameter = hKey;
            windowsEvents.CommandParameter = hKey;
            uls.CommandParameter = hKey;
        }

        private void disk_Click(object sender, RoutedEventArgs e)
        {
            Disk Desk = new Disk(Group, disk.CommandParameter.ToString());
            this.Content = Desk;
            mainFrame.NavigationService.Navigate(Desk);
        }

        private void cpu_Click(object sender, RoutedEventArgs e)
        {

            CPU Cpu = new CPU(Group, cpu.CommandParameter.ToString());
            this.Content = Cpu;
            mainFrame.NavigationService.Navigate(Cpu);
        }

        private void windowsEvents_Click(object sender, RoutedEventArgs e)
        {
            WindowsEvents WindowsEvents = new WindowsEvents(Group, windowsEvents.CommandParameter.ToString());
            this.Content = WindowsEvents;
            mainFrame.NavigationService.Navigate(WindowsEvents);
        }

        private void uls_Click(object sender, RoutedEventArgs e)
        {
            ULS Uls = new ULS(Group, uls.CommandParameter.ToString());
            this.Content = Uls;
            mainFrame.NavigationService.Navigate(Uls);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ServersList servers = new ServersList(Group);
            this.Content = servers;
            mainFrame.NavigationService.Navigate(servers);
        }
    }
}