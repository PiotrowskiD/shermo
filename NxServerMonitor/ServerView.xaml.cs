using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using NxServerMonitor.ViewModels;
using NxServerMonitor.ServiceReference2;
using System.Windows.Controls.DataVisualization.Charting;
using ServerMonitor.Model.Validators;
using ServerMonitor.Model;

namespace NxServerMonitor
{
    public partial class ServerView : Page
    {
        IService1 service = ServiceFactory.Create();
        ServerStatusRepository serverStatus;
        GroupModel Group;
        string Key;

        public ServerView(GroupModel group, string key)
        {
            serverStatus = new ServerStatusRepository(service);
            Group = group;
            Key = key;
            this.Loaded += (s, e) => init(key);
        }

        public ServerView(GroupModel group, string key, bool opened)
        {
            serverStatus = new ServerStatusRepository(service);
            Group = group;
            Key = key;
            this.Loaded += (s, e) => init(key);
        }

        void init(string key)
        {
            InitializeComponent();
            CreateChartsAxis();
            GetServerInfo(key);
            SetComboBox();
            CheckServerStatus(key);
            Refresh(key);
        }

        private void GetServerInfo(string key)
        {
            try
            {
                var server = serverStatus.GetServerInfo(key, Group);
                serverName.Text = server.DomainName;
                serverIP.Text = server.IP;
            }
            catch (Exception e)
            {
                MessageBox.Show("No service connection", e.Message);
                GetServerInfo(key);
            }
        }

        private void Refresh(string key)
        {
            var timer = new DispatcherTimer();
            timer.Tick += async (s, args) => await CheckServerStatus(key);

            timer.Interval = new TimeSpan(0, 0, 30);

            timer.Start();
        }

        private void SetComboBox()
        {
            try
            {
                var config = service.GetUrlConfig(Key);
                var deserial = Newtonsoft.Json.JsonConvert.DeserializeObject<List<URLValue>>(config);
                foreach (var d in deserial)
                {
                    httpComboBox.Items.Add(d.Url);
                }
            }
            catch { }
        }



        public async Task CheckServerStatus(string key)
        {
            try
            {
                await MakeChart(key);
                await MakeHealthChart(key);
                await GetDisk(key);
                await GetWindowsEvents(key);
                await GetAppPools(key);
                await GetULS(key);
                await GetSql(key);
                await GetReboot(key);
                await GetServices(key);
                var ok = await serverStatus.CheckConnectionAsync(key);
                if (ok == ConnectionValidator.connection.notOK)
                {
                    ConnectionLostBox.Background = new SolidColorBrush(Color.FromRgb(192, 0, 2));
                    ConnectionLostBox.Text = "Server Connection Lost";
                }
            }
            catch
            { }
        }

        private async Task MakeHealthChart(string key)
        {
            var pickedHttpService = httpComboBox.SelectedItem.ToString();
            var chartData = await serverStatus.GetHTTPService(key, pickedHttpService);
            ServerHealthChart.DataContext = chartData;
        }

        private async Task MakeChart(string key)
        {
            var cpu = await serverStatus.GetCpu(key);
            var ram = await serverStatus.GetRAM(key);
            var chartData = new List<List<KeyValuePair<DateTime, double>>>();
            chartData.Add(cpu);
            chartData.Add(ram);
            InfoChart.DataContext = chartData;
        }

        private async Task GetSql(string key)
        {
            var sqlView = await serverStatus.GetSqlPerf(key);
            var dependencyObj = new DependencyObject();
            await dependencyObj.Dispatcher.BeginInvoke(new Action(() => { SqlPerfListView.ItemsSource = sqlView; }));
        }

        private async Task GetDisk(string key)
        {
            var disksView = await serverStatus.GetDisk(key);
            var dependencyObj = new DependencyObject();
            await dependencyObj.Dispatcher.BeginInvoke(new Action(() => { disksListView.ItemsSource = disksView; }));
        }

        private async Task GetAppPools(string key)
        {
            var appPools = await serverStatus.GetAppPools(key);
            var dependencyObj = new DependencyObject();
            await dependencyObj.Dispatcher.BeginInvoke(new Action(() => { AppPoolListView.ItemsSource = appPools; }));
        }

        private async Task GetServices(string key)
        {
            var windowsServices = await serverStatus.GetServices(key);
            var dependencyObj = new DependencyObject();
            await dependencyObj.Dispatcher.BeginInvoke(new Action(() => { ServicesListView.ItemsSource = windowsServices; }));
        }

        private async Task GetULS(string key)
        {
            var ulsView = await serverStatus.GetULS(key);
            var dependencyObj = new DependencyObject();
            await dependencyObj.Dispatcher.BeginInvoke(new Action(() => { ULSViewList.ItemsSource = ulsView; }));
        }

        private async Task GetWindowsEvents(string key)
        {
            var eventsView = await serverStatus.GetWindowsEvents(key);
            var dependencyObj = new DependencyObject();
            await dependencyObj.Dispatcher.BeginInvoke(new Action(() => { EventsViewList.ItemsSource = eventsView; }));
        }

        private void sqlSelect(object sender, RoutedEventArgs e)
        {
            var selectedItem = SqlPerfListView.SelectedItem as SqlPerfViewModel;
            if (selectedItem == null)
                return;
            string message = selectedItem.Message;
            MessageBox.Show(message);
        }

        private async Task GetReboot(string key)
        {
            var reboot = await service.GetServerOtherParamsAsync("Reboot required", key);
            bool value = CheckReboot.validate(reboot);
            if (value)
            {
                RebootIcon.Visibility = Visibility.Visible;
                RebootRequiredBox.Visibility = Visibility.Visible;
            }
        }

        private void ULSView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = ULSViewList.SelectedItem as EventsViewModel;
            if (selectedItem == null)
                return;
            string key = selectedItem.ServerKey;
            var uls = new ULS(Group, key);
            var window = new Window();
            window.Content = uls;
            window.ShowDialog();
        }

        private void EventsViewList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = EventsViewList.SelectedItem as EventsViewModel;
            if (selectedItem == null)
                return;
            string key = selectedItem.ServerKey;
            var windowsEvents = new WindowsEvents(Group, key);
            var window = new Window();
            window.Content = windowsEvents;
            window.ShowDialog();
        }

        public void backButton_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            ServersList.loop = false;
            Content = clients;
            NavigationService.Navigate(clients);
        }

        private void ServicesButton_Click(object sender, RoutedEventArgs e)
        {
            var services = new ServicesConfiguration(Key);
            services.DataContext = this;
            services.ShowDialog();
        }

        private void AppPoolsButton_Click(object sender, RoutedEventArgs e)
        {
            var appPools = new AppPoolsConfiguration(Group, Key);
            appPools.DataContext = this;
            appPools.ShowDialog();
        }

        private void disksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = disksListView.SelectedItem as DisksViewModel;
            if (selectedItem == null)
                return;
            string name = selectedItem.DiskName;
            var window = new Window();
            var disk = new Disk(Group, Key, name);
            window.Content = disk;
            window.ShowDialog();
        }

        private void CreateChartsAxis()
        {
            var x = new DateTimeAxis();
            x.Orientation = AxisOrientation.X;
            x.Title = "Time";
            x.IntervalType = DateTimeIntervalType.Auto;

            var y = new LinearAxis();
            y.Orientation = AxisOrientation.Y;
            y.Title = "Usage";

            InfoChart.Axes.Add(x);
            InfoChart.Axes.Add(y);

            var xHealth = new DateTimeAxis();
            xHealth.Orientation = AxisOrientation.X;
            xHealth.Title = "Time";
            xHealth.IntervalType = DateTimeIntervalType.Minutes;

            ServerHealthChart.Axes.Add(xHealth);

            var yHealth = new LinearAxis();
            yHealth.Title = "Access TIme";
            yHealth.Orientation = AxisOrientation.Y;

            ServerHealthChart.Axes.Add(yHealth);
        }

        private void cpuDetails_Click(object sender, RoutedEventArgs e)
        {
            var cpu = new CPU(Group, Key);
            var window = new Window();
            window.Content = cpu;
            window.Show();
        }

        private void emailConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var emailConfig = new EmailConfiguration(Key);
            var window = new Window();
            window.Content = emailConfig;
            window.DataContext = window;
            window.Width = 300;
            window.Height = 300;
            window.ShowDialog();
        }

        private void URLconfig_Click(object sender, RoutedEventArgs e)
        {
            var url = new URLConfiguration(Key);
            url.ShowDialog();
        }

        private void httpComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MakeHealthChart(Key);
        }

        private void support_Click(object sender, RoutedEventArgs e)
        {
            var supportWin = new Support();
            supportWin.ShowDialog();
        }
    }
}