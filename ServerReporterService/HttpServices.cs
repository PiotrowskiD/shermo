using log4net;
using ServerMonitor.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;

namespace ServerReporterService
{
    class HttpServices
    {

        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        System.Diagnostics.Stopwatch timer = new Stopwatch();
        HttpServicesValue httpInfo;


        public List<HttpServicesValue> CheckParameters(string id)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var service = ConnectionFactory.Create();
            List<HttpServicesValue> httpInfos = new List<HttpServicesValue>();
            var today = DateTime.Now;
            string s;
            var urls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<URLValue>>(service.GetUrlConfig(id));
            foreach (var url in urls)
            {
                timer.Reset();
                httpInfo = new HttpServicesValue();
                httpInfo.Type = url.Type;
                httpInfo.Date = today.ToString();
                httpInfo.Url = url.Url;
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(url.Url);
                    request.Method = "HEAD";
                    timer.Start();
                    var response = (HttpWebResponse)request.GetResponse();
                    timer.Stop();
                    httpInfo.Status = response.StatusCode.ToString();
                    httpInfo.Time = timer.Elapsed.Milliseconds;
                    using (WebClient client = new WebClient())
                    {
                        s = client.DownloadString(url.Url);
                    }

                }
                catch (Exception ex)
                {
                    timer.Stop();
                    httpInfo.Time = 0;
                    httpInfo.Status = "NOT FOUND";
                    s = "ERROR";
                }

                if (url.Type == "SharePoint")
                {

                    if (s.Contains("error", StringComparison.OrdinalIgnoreCase) || s.Contains("occured", StringComparison.OrdinalIgnoreCase)
                        || s.Contains("wystąpił", StringComparison.OrdinalIgnoreCase) || s.Contains("błąd", StringComparison.OrdinalIgnoreCase))
                    {
                        httpInfo.Data = "SHAREPOINT_ERROR";
                    }

                }
                else if (url.Type == "WebService")
                {

                    if (s.Contains("error", StringComparison.OrdinalIgnoreCase) || s.Contains("occured", StringComparison.OrdinalIgnoreCase)
                        || s.Contains("wystąpił", StringComparison.OrdinalIgnoreCase) || s.Contains("błąd", StringComparison.OrdinalIgnoreCase))
                    {
                        httpInfo.Data = "WEBSERVICE_ERROR";
                    }
                }
                httpInfos.Add(httpInfo);
            }
            logger.Info("Http services details checked succesfully");
            return httpInfos;
        }

    }
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
