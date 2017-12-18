using ServerMonitor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxServerMonitor.ViewModels
{
    public class GroupModel
    {
        public string GroupName { get; set; }
        public List<ServerInfo> ServersList { get; set; }

        public GroupModel(string groupName)
        {
            ServersList = new List<ServerInfo>();
            GroupName = groupName;
        }

        public void AddToList(ServerInfo serverInfo)
        {
            ServersList.Add(serverInfo);
        }

    }
}
