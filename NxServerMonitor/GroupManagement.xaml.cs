using NxServerMonitor.ServiceReference2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using NxServerMonitor.ViewModels;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for GroupManagement.xaml
    /// </summary>
    public partial class GroupManagement : Window
    {
        public static List<GroupModel> groupInfoList = new List<GroupModel>();
        ObservableCollection<GroupModel> clientsGroups = new ObservableCollection<GroupModel>();
        Clients clients;
        bool isSaved;
        public GroupManagement(ObservableCollection<GroupModel> groupCollection)
        {
            clientsGroups = groupCollection;
            clients = (Clients)DataContext;
            InitializeComponent();
            SetGroupList();
        }

        public void SetGroupList()
        {
            try
            {
                foreach (var group in Clients.groups)
                {
                    bool isPresent = false;
                    foreach(var groupInfo in groupInfoList)
                    {
                        if (groupInfo.GroupName == group.GroupName)
                            isPresent = true;
                    }
                    if (!isPresent)
                        groupInfoList.Add(group);
                }
            }
            catch
            {
            }
            groupsList.ItemsSource = groupInfoList;
        }

        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            isSaved = false;
            var add = new AddNewGroup();
            add.DataContext = this;
            add.ShowDialog();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (groupsList.SelectedItem == null)
                return;
            var item = groupsList.SelectedItem as GroupModel;
            if (item.GroupName == "Unasigned")
                MessageBox.Show("You cannot delete this group.");
            else
            {
                foreach (var c in groupInfoList)
                {
                    if (c.GroupName == item.GroupName)
                    {
                        groupInfoList.Remove(c);
                        groupsList.Items.Refresh();
                        isSaved = false;
                        return;
                    }
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSaved)
                return;
            Clients.groups.Clear();
            foreach (var gr in groupInfoList)
            {
                Clients.groups.Add(new GroupModel(gr.GroupName));
            }
            isSaved = true;
            Clients.WriteGroupList();
            Close();
        }
    }
}