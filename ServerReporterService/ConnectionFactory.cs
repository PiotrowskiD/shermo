using ServerReporterService.ServiceReference1;
using System.Configuration;
using System.Net;

namespace ServerReporterService
{
    public class ConnectionFactory 
    {
        public static IService1 Create()
        {
            var service = new Service1Client();
            ServicePointManager.ServerCertificateValidationCallback =
                 (s, certificate, chain, sslPolicyErrors) => true;
            var username = service.ClientCredentials.UserName;
            username.UserName = ConfigurationManager.AppSettings["userName"];
            username.Password = ConfigurationManager.AppSettings["password"];
            return service;
        }
    }
}
