using NxServerMonitor.ServiceReference1;
using ServerMonitor.Model.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NxServerMonitor.ViewModels;
using ServerMonitor.Model;
using ServiceReference1.Model.Validators;

namespace NxServerMonitor
{
    public class ServerStatusService
    {
        IService1 _webservice;

        public ServerStatusService(IService1 service)
        {
            _webservice = service;
        }

        public ServerViewModel GetServerInfo(string key, string group)
        {
            var server = _webservice.GetServerName(key).ToArray();
            ServerViewModel serverInfo = null;
            foreach (var s in server)
            {
                string hostName = s.HostName;
                string domainName = s.DomainName;
                string ip = s.IpAdress;                
                serverInfo = new ServerViewModel(hostName, domainName, key, group, ip);             
            }
            return serverInfo;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> GetRAM(string key)
        {
            var performanceInfo = await _webservice.GetPerformanceStatusAsync(key);
            var ram = performanceInfo.ToArray();
            var orderedRAM = ram.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))).ToList();
            var list = new List<KeyValuePair<DateTime, double>>();
            var list2 = new List<KeyValuePair<DateTime, double>>();
            foreach (var obj in orderedRAM)
            {
                DateTime date = DateTime.ParseExact(obj.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                double total = obj.PhysicalTotalMB;
                double available = obj.PhysicalAvailableMB;
                double percent = ((total - available) / total * 100);
                list.Add(new KeyValuePair<DateTime, double>(date, percent));
            }

            var last = list.Last();
            list2.Add(last);

            for (int i = 0; i < 10; i++)
            {
                var l = list2.Last().Key.AddMinutes(-30);
                try
                {
                    var l2 = list.Last(v => v.Key < l);
                    list2.Add(l2);
                }
                catch
                {
                    return list2;
                }
            }
            return list2;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> GetCpu(string key)
        {
            var performanceData = await _webservice.GetPerformanceStatusAsync(key);
            var cpu = performanceData.ToArray();
            var orderedCpu = cpu.OrderBy(x => (DateTime.ParseExact(x.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))).ToList();
            var list = new List<KeyValuePair<DateTime, double>>();
            var list2 = new List<KeyValuePair<DateTime, double>>();

            foreach (var obj in orderedCpu)
            {
                DateTime date = DateTime.ParseExact(obj.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                double percent = double.Parse(obj.CpuPercentUsage.ToString(), NumberStyles.AllowDecimalPoint);
                list.Add(new KeyValuePair<DateTime, double>(date, percent));
            }

            var last = list.Last();
            list2.Add(last);

            for (int i = 0; i < 10; i++)
            {
                var l = list2.Last().Key.AddMinutes(-30);
                try
                {
                    var l2 = list.Last(v => v.Key < l);
                    list2.Add(l2);
                }
                catch
                {
                    return list2;
                }
            }
            return list2;
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
                    SqlPerfViewModel sp = new SqlPerfViewModel(obj.LastWorkerTime, total, count, obj.Message);
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
            var diskInfo = await _webservice.GetDisksStatusAsync(key);
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
                    case DiskValidator.diskOutcome.notOK:
                        color = Color.FromRgb(192, 0, 2);
                        break;
                    case DiskValidator.diskOutcome.Warning:
                        color = color = Color.FromRgb(222, 97, 0);
                        break;
                    default:
                        color = Color.FromRgb(99, 255, 74);
                        break;
                }

                DisksViewModel dvm = new DisksViewModel(color, last.Name, last.PercentOfFreeSpace.ToString());
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
            Color color = Color.FromRgb(99, 255, 74);
            try
            {
                var configuration = _webservice.GetAppPoolsConfig(key);
                var deserializeConf = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckedPools>>(configuration);

                foreach (var d in deserializeAll)
                {
                    foreach (var c in deserializeConf)
                    {
                        if (d.Name == c.Name)
                        {
                            if (d.State == "Stopped")
                                color = Color.FromRgb(192, 0, 2);
                            else
                                color = Color.FromRgb(99, 255, 74);

                            var appPool = new AppPoolsViewModel(d.Name, d.State, d.User, color);
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
            Color color = Color.FromRgb(99, 255, 74);

            foreach (var d in deserializeAll)
            {
                foreach (var c in deserializeConf)
                {
                    if (d.ServiceName == c.ServiceName)
                    {
                        if (d.Status == "Stopped")
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
            var ulsInfo = await _webservice.GetUlsViewerStatusAsync(key);
            var uls = ulsInfo.ToArray();

            Color color = Color.FromRgb(99, 255, 74);

            var groups = uls.GroupBy(x => x.Level).Select(grp => grp.ToList()).ToList();
            int amount = 0;
            int criticals = 0;
            int errors = 0;
            int unexpecteds = 0;
            foreach (var g in groups)
            {
                var last = g.Last();
                if (last.Level == "Critical")
                {
                    criticals = g.Count();
                    amount += criticals;
                }
                else if (last.Level == "Error")
                {
                    errors = g.Count();
                    amount += errors;
                }
                else if (last.Level == "Unexpected")
                {
                    unexpecteds = g.Count();
                    amount += unexpecteds;
                }

            }

            if (errors != 0)
                color = Color.FromRgb(222, 97, 0);
            if (criticals != 0)
                color = Color.FromRgb(192, 0, 2);
            if (unexpecteds != 0)
                color = Color.FromRgb(119, 68, 106);

            EventsViewModel uvm = new EventsViewModel(color, amount.ToString(), key);
            ulsView.Add(uvm);
            return ulsView;
        }

        public async Task<ObservableCollection<EventsViewModel>> GetWindowsEvents(string key)
        {
            var eventsView = new ObservableCollection<EventsViewModel>();
            var windowsEventsInfo = await _webservice.GetUlsViewerStatusAsync(key);
            var windowsEvents = windowsEventsInfo.ToArray();

            Color color = Color.FromRgb(99, 255, 74);

            var groups = windowsEvents.GroupBy(x => x.Level).Select(grp => grp.ToList()).ToList();
            int amount = 0;
            int errors = 0;

            foreach (var g in groups)
            {
                var last = g.Last();

                if (last.Level == "Error")
                {
                    errors = g.Count();
                    amount += errors;
                }
            }

            if (errors != 0)
                color = Color.FromRgb(222, 97, 0);

            EventsViewModel uvm = new EventsViewModel(color, amount.ToString(), key);
            eventsView.Add(uvm);
            return eventsView;
        }



    }
}
