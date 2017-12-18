using log4net;
using ServerMonitor.Model;
using System;
using System.Reflection;
using System.Timers;

namespace ServerReporterService
{
    public class ReporterService
    {
        private static Timer timer;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        bool isInit = false;
        ServerInfo serverInfo;
        Server server = new Server();
        Disk disk = new Disk();
        Performance perf = new Performance();
        EventViewer eventViewer = new EventViewer();
        UlsViewer ulsViewer = new UlsViewer();
        SqlPerformance sqlPerf = new SqlPerformance();
        OtherServerParams otherParams = new OtherServerParams();
        HttpServices httpServices = new HttpServices();

        public void Init()
        {
            var service = ConnectionFactory.Create();
            try
            {
                serverInfo = server.updateInfo();
                service.UpdateServerInfo(serverInfo, "0");
                isInit = true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message);
            }
            service.InsertUnasignedGroup();

        }
        public void Start()
        {
            logger.Info("Server Reporter service started");
            timer = new Timer(60000*3);
            timer.Elapsed += new ElapsedEventHandler(Run);
            timer.Enabled = true;
        }

        private void Run(object source, ElapsedEventArgs args)
        {
            if (!isInit) Init(); 
            var service = ConnectionFactory.Create();
            Action<Action> tryUpdate = (a) =>
            {
                try
                {
                    a();
                }
                catch (Exception e)
                {
                    logger.Error("Updating info failed: " + e.Message);
                }
            };

            tryUpdate(() =>
                service.UpdateDiskStatus(
                    disk.CheckParameters().ToArray(),
                    serverInfo.IdKey)
                    );
            tryUpdate(() =>
                service.UpdatePerformanceInfo(
                    perf.CheckParameters(),
                    serverInfo.IdKey)
                    );
            tryUpdate(() =>
                service.UpdateEventViewerStatus(
                    eventViewer.CheckParameters().ToArray(),
                    serverInfo.IdKey)
                    );
            tryUpdate(() =>
                service.UpdateUlsStatus(
                    ulsViewer.CheckParameters().ToArray(),
                    serverInfo.IdKey)
                    );
            tryUpdate(() =>
                service.UpdateSqlPerfStatus(
                    sqlPerf.CheckParameters().ToArray(),
                    serverInfo.IdKey)
                    );
            tryUpdate(() =>
                service.UpdateOtherServerParams(
                    otherParams.CheckParameters().ToArray(),
                    serverInfo.IdKey)
                    );
            tryUpdate(() =>
                service.UpdateHttpServicesStatus(
                    httpServices.CheckParameters(serverInfo.IdKey.ToString()).ToArray(),
                    serverInfo.IdKey)
                    );
        }

        public void Stop()
        {
            logger.Info("Server Reporter service stopped");
        }
    }
}
