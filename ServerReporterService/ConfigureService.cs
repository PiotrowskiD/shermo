using Topshelf;

namespace ServerReporterService
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<ReporterService>(service =>
                {
                    service.ConstructUsing(s => new ReporterService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                    configure.StartAutomatically();
                    configure.UseLog4Net();
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("ServerReporterService");
                configure.SetDisplayName("ServerReporterService");
                configure.SetDescription("ServerMonitor Tool that send data about params of machine.");
            });
        }
    }
}
 