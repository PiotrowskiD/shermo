using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using NxServerMonitor.ServiceReference2;

namespace NxServerMonitor
{
    public class ServiceFactory
    {
        public static IService1 Create()
        {
            Service1Client service = new Service1Client();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            var user = service.ClientCredentials.UserName;
            user.UserName = ConfigurationManager.AppSettings["userName"];
            user.Password = ConfigurationManager.AppSettings["password"];

            return service;
        }
    }
}