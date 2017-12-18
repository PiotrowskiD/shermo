using Microsoft.Win32;
using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using ServerMonitor.Model;
using System.ServiceProcess;
using log4net;
using System.Reflection;
using System.Net;

namespace ServerReporterService
{
    class OtherServerParams
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public
        string title;
        public List<ServerOtherParams> CheckParameters()
        {
            List<ServerOtherParams> otherParams = new List<ServerOtherParams>();
            ServerOtherParams param;
            var today = DateTime.Now;

            param = new ServerOtherParams();
            param.Value = PendingReboot().ToString();
            param.Title = title;
            param.LastUpdate = today.ToString("yyyy - MM - dd HH: mm:ss");
            otherParams.Add(param);

            param = new ServerOtherParams();
            param.Value = AppPoolsValue();
            param.Title = title;
            param.LastUpdate = today.ToString("yyyy - MM - dd HH: mm:ss");
            otherParams.Add(param);

            param = new ServerOtherParams();
            param.Value = WinServicesValue();
            param.Title = title;
            param.LastUpdate = today.ToString("yyyy - MM - dd HH: mm:ss");
            otherParams.Add(param);

            logger.Info("Other server parameters checked succesfully");
            return otherParams;
        }
        bool PendingReboot()
        {
            title = "Reboot required";

            RegistryKey key;

            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Component Based Servicing\\RebootPendings");

            if (key != null)
                return true;

            key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\\Auto Update\\RebootRequired");

            if (key != null)
                return true;

            key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager");

            if (key.GetValue("PendingFileRenameOperations") != null)
                return true;
            else return false;
        }
        string AppPoolsValue()
        {
            title = "App Pools";
            ServerManager server = new ServerManager();
            List<ApplicationPoolsValue> values = new List<ApplicationPoolsValue>();
            ApplicationPoolCollection applicationPools = server.ApplicationPools;
            foreach (ApplicationPool pool in applicationPools)
            {
                ApplicationPoolsValue appPool = new ApplicationPoolsValue();
                appPool.Name = pool.Name;
                appPool.State = pool.State.ToString();
                var userName = pool.ProcessModel.UserName;
                appPool.User = !string.IsNullOrWhiteSpace(userName) ? userName : pool.ProcessModel.IdentityType.ToString();
                values.Add(appPool);
            }
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values);
            return jsonString;
        }
        string WinServicesValue()
        {
            title = "Windows Services installed";
            List<WindowsServicesValue> values = new List<WindowsServicesValue>();
            foreach (ServiceController sc in ServiceController.GetServices())
            {
                WindowsServicesValue wsv = new WindowsServicesValue();
                wsv.ServiceName = sc.ServiceName;
                wsv.Status = sc.Status.ToString();
                wsv.DisplayName = sc.DisplayName;
                wsv.ServiceType = sc.ServiceType.ToString();
                values.Add(wsv);
            }
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values);
            return jsonString;
        }
    }

}
