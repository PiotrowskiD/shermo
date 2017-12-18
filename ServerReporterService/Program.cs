using Topshelf;

namespace ServerReporterService
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureService.Configure();
        }
    }
}
