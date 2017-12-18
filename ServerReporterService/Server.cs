using System;
using System.Net;
using ServerMonitor.Model;
using System.Net.Sockets;
using log4net;
using System.Reflection;

namespace ServerReporterService
{
    class Server
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServerInfo updateInfo()
        {
            ServerInfo server = new ServerInfo();

            server.DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            server.HostName = Dns.GetHostName();
            Microsoft.Win32.RegistryKey key;
            server.IpAdress = GetLocalIPAddress();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Nexpertis");

            ServiceReference1.Service1Client objServiceClientobjService = new ServiceReference1.Service1Client();

            Guid newKey = Guid.NewGuid();

            if (key.GetValue("Id") == null)
            {
                key.SetValue("Id", newKey);
            }
            else
            {
                newKey = new Guid(key.GetValue("Id").ToString());
            }
            key.Close();
            server.IdKey = newKey;
            logger.Info("Server details chceked succesfully");
            return server;

        }
        string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}