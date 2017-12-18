using ServerMonitor.Model.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using NxServerMonitor.ViewModels;
using ServerMonitor.Model;
using NxServerMonitor.ServiceReference2;

namespace NxServerMonitor
{
    public class ServerStatusRepository
    {
        IService1 _webservice;

        public ServerStatusRepository(IService1 service)
        {
            _webservice = service;
        }

        public ServerViewModel GetServerInfo(string key, GroupModel group)
        {
            var server = _webservice.GetServerName(key);
            string hostName = server.HostName;
            string domainName = server.DomainName;
            string ip = server.IpAdress;
            var serverInfo = new ServerViewModel(hostName, domainName, key, group, ip);
            return serverInfo;
        }

        public ConnectionValidator.connection CheckConnection(string key)
        {
            var performanceInfo = _webservice.GetPerformanceStatus(key, 1000).ToArray();
            ConnectionValidator.connection isOK = ConnectionValidator.validate(performanceInfo);
            return isOK;
        }
        public async Task<ConnectionValidator.connection> CheckConnectionAsync(string key)
        {
            var performanceInfo = _webservice.GetPerformanceStatus(key, 1000).ToArray();
            var isOK = ConnectionValidator.validate(performanceInfo);
            return isOK;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> GetHTTPService(string key, string url)
        {
            var httpService = await _webservice.GetHttpServicesStatusAsync(key, 30);
            var data = new List<KeyValuePair<DateTime, double>>();
            foreach(var service in httpService)
            {
                if (service.Url == url)
                {
                    var date = DateTime.ParseExact(service.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    var keyValue = new KeyValuePair<DateTime, double>(date, double.Parse(service.Time.ToString(), CultureInfo.InvariantCulture));
                    data.Add(keyValue);
                }
            }
            return data;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> GetRAM(string key)
        {
            var performanceInfo = await _webservice.GetPerformanceStatusAsync(key, 100000);
            var ram = performanceInfo.ToArray();
            var orderedRAM = ram.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))).ToList();
            var allData = new List<KeyValuePair<DateTime, double>>();
            var ramListForChart = new List<KeyValuePair<DateTime, double>>();
            foreach (var obj in orderedRAM)
            {
                var date = DateTime.ParseExact(obj.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                double total = obj.PhysicalTotalMB;
                double available = obj.PhysicalAvailableMB;
                double percent = ((total - available) / total * 100);
                allData.Add(new KeyValuePair<DateTime, double>(date, percent));
            }

            var last = allData.Last();
            ramListForChart.Add(last);

            for (int i = 0; i < 10; i++)
            {
                var l = ramListForChart.Last().Key.AddMinutes(-30);
                try
                {
                    var l2 = allData.Last(v => v.Key < l);
                    ramListForChart.Add(l2);
                }
                catch
                {
                    return ramListForChart;
                }
            }
            return ramListForChart;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> GetCpu(string key)
        {
            var performanceData = await _webservice.GetPerformanceStatusAsync(key, 100000);
            var cpu = performanceData.ToArray();
            var orderedCpu = cpu.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))).ToList();
            var allData = new List<KeyValuePair<DateTime, double>>();
            var cpuListForChart = new List<KeyValuePair<DateTime, double>>();

            foreach (var obj in orderedCpu)
            {
                var date = DateTime.ParseExact(obj.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                double percent = double.Parse(obj.CpuPercentUsage.ToString(), NumberStyles.AllowDecimalPoint);
                allData.Add(new KeyValuePair<DateTime, double>(date, percent));
            }

            var last = allData.Last();
            cpuListForChart.Add(last);

            for (int i = 0; i < 10; i++)
            {
                var l = cpuListForChart.Last().Key.AddMinutes(-30);
                try
                {
                    var l2 = allData.Last(v => v.Key < l);
                    cpuListForChart.Add(l2);
                }
                catch
                {
                    return cpuListForChart;
                }
            }
            return cpuListForChart;
        }

        public async Task<ObservableCollection<SqlPerfViewModel>> GetSqlPerf(string key)
        {
            var sqlView = new ObservableCollection<SqlPerfViewModel>();
            var sqlPerfInfo = await _webservice.GetSqlPerformanceStatusAsync(key);
            var sqlPerf = sqlPerfInfo.ToArray();
            var ordered = sqlPerf.OrderBy(x => double.Parse(x.LastWorkerTime.ToString(), NumberStyles.AllowDecimalPoint)).ToList();
            ordered.Reverse();
            try
            {
                var topThree = ordered.GetRange(0, 3).ToList();
                foreach (var obj in topThree)
                {
                    double total = double.Parse(obj.TotalWorkerTime.ToString(), NumberStyles.AllowDecimalPoint);
                    double count = double.Parse(obj.ExecutionCount.ToString(), NumberStyles.AllowDecimalPoint);
                    var sp = new SqlPerfViewModel(obj.LastWorkerTime, total, count, obj.Message);
                    sqlView.Add(sp);

                }
                return sqlView;
            }
            catch
            {
            }
            return sqlView;
        }

        public async Task<ObservableCollection<DisksViewModel>> GetDisk(string key)
        {
            var disksView = new ObservableCollection<DisksViewModel>();
            var diskInfo = await _webservice.GetDisksStatusAsync(key, 60);
            var disk = diskInfo.ToArray();
            var orderedDisk = disk.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))).ToList();
            var disks = disk.GroupBy(x => x.Name).Select(grp => grp.ToList()).ToList();
            foreach (var d in disks)
            {
                Color color = Color.FromRgb(99, 255, 74);
                var last = d.Last();
                DiskValidator.diskOutcome isOk = DiskValidator.validate(last);

                switch (isOk)
                {
                    case DiskValidator.diskOutcome.NotOK:
                        color = Color.FromRgb(192, 0, 2);
                        break;
                    case DiskValidator.diskOutcome.Warning:
                        color = color = Color.FromRgb(222, 97, 0);
                        break;
                    default:
                        color = Color.FromRgb(99, 255, 74);
                        break;
                }

                var dvm = new DisksViewModel(color, last.Name, last.PercentOfFreeSpace.ToString());
                disksView.Add(dvm);
            }
            return disksView;
        }

        public async Task<ObservableCollection<AppPoolsViewModel>> GetAppPools(string key)
        {
            var activeApplicationPools = new ObservableCollection<AppPoolsViewModel>();
            var appPools = await _webservice.GetServerOtherParamsAsync("App Pools", key);
            var value = appPools.Value;
            var deserializeAll = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApplicationPoolsValue>>(value);
            var color = Color.FromRgb(99, 255, 74);
            try
            {
                var configuration = _webservice.GetAppPoolsConfig(key);
                var deserializeConf = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckedAppPools>>(configuration);

                foreach (var d in deserializeAll)
                {
                    foreach (var c in deserializeConf)
                    {
                        if (d.Name == c.Name)
                        {
                            var appPool = new AppPoolsViewModel(d.Name, d.State, d.User, color);
                            var state = AppPoolsValidator.validate(d);
                            if (state == AppPoolsValidator.outcome.NotOK)
                            {
                                color = Color.FromRgb(192, 0, 2);
                                appPool.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:\\Users\\zuziaa\\Desktop\\staf\\nexpertis\\NX_ServerMonitor\\NxServerMonitor\\assets\\status-icon-error.png"));
                            }
                            else
                            {
                                color = Color.FromRgb(99, 255, 74);
                                appPool.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:\\Users\\zuziaa\\Desktop\\staf\\nexpertis\\NX_ServerMonitor\\NxServerMonitor\\assets\\status-icon-ok.png"));
                            }
                            activeApplicationPools.Add(appPool);
                        }
                    }
                }
            }
            catch
            {
            }
            return activeApplicationPools;
        }

        public async Task<ObservableCollection<ServicesViewModel>> GetServices(string key)
        {
            var activeWindowsServices = new ObservableCollection<ServicesViewModel>();
            var allServices = await _webservice.GetServerOtherParamsAsync("Windows Services installed", key);
            var value = allServices.Value;
            var deserializeAll = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WindowsServicesValue>>(value);

            var configuration = _webservice.GetServicesConfig(key);
            var deserializeConf = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckedServices>>(configuration);
            var color = Color.FromRgb(99, 255, 74);

            foreach (var d in deserializeAll)
            {
                foreach (var c in deserializeConf)
                {
                    if (d.ServiceName == c.ServiceName)
                    {
                        var status = ServicesValidator.validate(d);
                        if (status == ServicesValidator.outcome.NotOK)
                            color = Color.FromRgb(192, 0, 2);
                        else
                            color = Color.FromRgb(99, 255, 74);

                        var service = new ServicesViewModel(d.ServiceName, d.DisplayName, d.ServiceType, d.Status, color);
                        activeWindowsServices.Add(service);
                    }
                }
            }
            return activeWindowsServices;
        }

        public async Task<ObservableCollection<EventsViewModel>> GetULS(string key)
        {
            var ulsView = new ObservableCollection<EventsViewModel>();
            var ulsInfo = await _webservice.GetUlsViewerStatusAsync(key, 30);
            var uls = ulsInfo.ToArray();
            var color = Color.FromRgb(99, 255, 74);

            var groups = uls.GroupBy(x => x.Level).Select(grp => grp.ToList()).ToList();
            int amount = 0;
            int criticals = 0;
            int errors = 0;
            int unexpecteds = 0;

            foreach (var g in groups)
            {
                var last = g.Last();
                UlsValidator.outcome ifOK = UlsValidator.validate(last);
                if (ifOK == UlsValidator.outcome.Critical)
                    criticals = g.Count();
                else if (ifOK == UlsValidator.outcome.Error)
                    errors = g.Count();
                else if (ifOK == UlsValidator.outcome.Unexpected)
                    unexpecteds = g.Count();
            }

            if (errors != 0)
                color = Color.FromRgb(222, 97, 0);

            if (criticals != 0)
                color = Color.FromRgb(192, 0, 2);

            if (unexpecteds != 0)
                color = Color.FromRgb(119, 68, 106);

            var uvm = new EventsViewModel(color, amount.ToString(), key);
            ulsView.Add(uvm);
            return ulsView;
        }

        public async Task<ObservableCollection<EventsViewModel>> GetWindowsEvents(string key)
        {
            var eventsView = new ObservableCollection<EventsViewModel>();
            var windowsEventsInfo = await _webservice.GetEventViewerStatusAsync(key, 30);
            var windowsEvents = windowsEventsInfo.ToArray();
            var outcome = EventViewerValidator.validate(windowsEventsInfo.ToList());
            var color = Color.FromRgb(99, 255, 74);

            var groups = windowsEvents.GroupBy(x => x.LogName).Select(grp => grp.ToList()).ToList();
            int errors = 0;

            foreach (var g in groups)
            {
                var last = g.Last();

                if (last.LogName == "Error")
                {
                    errors = g.Count();
                }
            }

            switch (outcome)
            {
                case EventViewerValidator.outcome.NotOK:
                    color = Color.FromRgb(192, 0, 2);
                    break;
                case EventViewerValidator.outcome.Warning:
                    color = Color.FromRgb(222, 97, 0);
                    break;
                default:
                    color = Color.FromRgb(99, 255, 74);
                    break;
            }

            var uvm = new EventsViewModel(color, errors.ToString(), key);
            eventsView.Add(uvm);
            return eventsView;
        }

        public ObservableCollection<ClientViewModel> GetClients()
        {
            var clientsList = new ObservableCollection<ClientViewModel>();
            var clients = _webservice.GetGroups().ToArray();
            foreach (var c in clients)
            {
                var obj = new ClientViewModel(new GroupModel(c.GroupName));
                obj.Color = RandomColor();
                clientsList.Add(obj);
            }
            return clientsList;
        }

        private SolidColorBrush RandomColor()
        {
            var r = new Random();
            var properties = typeof(Brushes).GetProperties();
            var count = properties.Count();

            var colour = properties
                        .Select(x => new { Property = x, Index = r.Next(count) })
                        .OrderBy(x => x.Index)
                        .First();

            return (SolidColorBrush)colour.Property.GetValue(colour, null);
        }

    }
}