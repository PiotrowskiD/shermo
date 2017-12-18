using NxServerMonitor.ViewModels;
using ServerMonitor.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace NxServerMonitor
{
    public partial class ServerAssignment : Window
    {
        ObservableCollection<CheckedGroup> checkboxes = new ObservableCollection<CheckedGroup>();
        ServerInfo Server;
        public ServerAssignment(ServerViewModel serverModel)
        {
            Server = new ServerInfo { HostName = serverModel.HostName, DomainName = serverModel.DomainName, IdKey = new Guid(serverModel.Key), IpAdress = serverModel.IP };
            InitializeComponent();
            SetGroupsNames();
        }

        private void SetGroupsNames()
        {
            var list = Clients.ReadGroupList();
            foreach (var client in list)
            {
                var checkedGroup = new CheckedGroup(client.GroupName, false);
                checkboxes.Add(checkedGroup);
            }
            CheckTheBoxes();
            groupList.ItemsSource = checkboxes;
        }

        private void CheckTheBoxes()
        {
            try
            {
                var list = Clients.ReadGroupList();
                var groups = Clients.groups;
                foreach (var l in groups)
                {
                    foreach (var s in l.ServersList)
                    {
                        if (s.IdKey == Server.IdKey)
                        {
                            foreach (var c in checkboxes)
                            {
                                if (c.Name == l.GroupName)
                                {
                                    c.IsSelected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var selectedCheckbox = (CheckBox)sender;
            string name = selectedCheckbox.Content.ToString();
            foreach(var c in Clients.groups)
            {
                if (c.GroupName == name)
                    c.AddToList(Server);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var selectedCheckbox = (CheckBox)sender;
            string name = selectedCheckbox.Content.ToString();
            foreach (var c in Clients.groups)
            {
                if (c.GroupName == name)
                {
                    foreach (var l in c.ServersList)
                    {
                        if (l.IdKey == Server.IdKey)
                        {
                            c.ServersList.Remove(l);
                            return;
                        }
                    }
                }                
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Clients.WriteGroupList();
            var management = (ServersList)DataContext;
            management.GetServersList();
            management.CheckServer();
            Close();
        }
    }
}