using NxServerMonitor.ServiceReference2;
using NxServerMonitor.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace NxServerMonitor
{
    public partial class Clients : Page
    {
        IService1 service = ServiceFactory.Create();
        ServerStatusRepository serverStatus;
        public static ObservableCollection<GroupModel> groups = new ObservableCollection<GroupModel>();
        public Clients()
        {
            serverStatus = new ServerStatusRepository(service);
            this.Loaded += (s, e) => init();
        }

        void init()
        {
            InitializeComponent();
            SetList();
            SetAllServersToGroup();
            ServersList servers = new ServersList(CheckUnasigned());
            servers.DataContext = this;
            serversFrame.Content = servers;
        }

        public void SetList()
        {
            groups = ReadGroupList();
            clientsList.ItemsSource = groups;
        }

        private void selectItem(object sender, RoutedEventArgs e)
        {
            var selectedItem = clientsList.SelectedItem as GroupModel;
            if (selectedItem == null)
                return;
            var servers = new ServersList(selectedItem);
            servers.DataContext = this;
            serversFrame.Content = servers;
        }

        private void manageGroups_Click(object sender, RoutedEventArgs e)
        {
            var groupManagement = new GroupManagement(groups);
            groupManagement.DataContext = this;
            groupManagement.ShowDialog();
        }

        public static ObservableCollection<GroupModel> ReadGroupList()
        {
            var groups = new ObservableCollection<GroupModel>();
            try
            {
                var listToDeserialize = File.ReadAllText("groups.txt");
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GroupModel>>(listToDeserialize);
                foreach (var l in list)
                {
                    groups.Add(l);
                }
                bool isUnasigned = false;
                foreach (var group in list)
                {
                    if (group.GroupName == "Unasigned")
                        isUnasigned = true;
                }
                if (!isUnasigned)
                {
                    groups.Add(new GroupModel("Unasigned"));
                }
            }
            catch
            {
                groups.Add(new GroupModel("Unasigned"));
            }
            return groups;
        }

        private void SetAllServersToGroup()
        {
            var all = service.GetAllServers();

            foreach (var server in all)
            {
                bool isAssigned = false;
                foreach (var group in groups)
                {
                    foreach (var s in group.ServersList)
                    {
                        if (s.IdKey == server.IdKey)
                            isAssigned = true;
                    }
                }
                if (!isAssigned)
                {
                    var unasigned = CheckUnasigned();
                    unasigned.AddToList(server);
                }
            }
            WriteGroupList();
        }

        private GroupModel CheckUnasigned()
        {
            GroupModel unasigned = null;
            foreach (var group in groups)
            {
                if (group.GroupName == "Unasigned")
                    unasigned = group;
            }
            return unasigned;
        }

        public static void WriteGroupList()
        {
            var list = Newtonsoft.Json.JsonConvert.SerializeObject(groups);
            File.WriteAllText("groups.txt", list);
        }
    }
}