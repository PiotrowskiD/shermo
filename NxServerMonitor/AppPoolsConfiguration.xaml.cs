using ServerMonitor.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using NxServerMonitor.ViewModels;
using NxServerMonitor.ServiceReference2;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for AppPoolsConfiguration.xaml
    /// </summary>
    public partial class AppPoolsConfiguration : Window
    {
        IService1 service = ServiceFactory.Create();
        List<CheckedAppPools> Checked = new List<CheckedAppPools>();
        ServerStatusRepository serverStatus;
        ObservableCollection<CheckedAppPools> checkboxes = new ObservableCollection<CheckedAppPools>();
        string Key;
        GroupModel Group;

        public AppPoolsConfiguration(GroupModel group, string key)
        {
            Group = group;
            Key = key;
            serverStatus = new ServerStatusRepository(service);
            InitializeComponent();
            SetConfig(key);
        }

        private void SetConfig(string key)
        {
            var services = service.GetServerOtherParams("App Pools", key);
            var value = services.Value;
            var deserial = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApplicationPoolsValue>>(value);

            foreach (var d in deserial)
            {
                var checkedPool = new CheckedAppPools(d.Name, false);
                checkboxes.Add(checkedPool);
            }

            CheckTheBoxes(key);
            appPoolsList.ItemsSource = checkboxes;
        }

        private void CheckTheBoxes(string key)
        {
            try
            {
                var configuration = service.GetAppPoolsConfig(key);
                var deserializeConf = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckedAppPools>>(configuration);

                foreach (var c in deserializeConf)
                {
                    Checked.Add(new CheckedAppPools(c.Name, true));
                    foreach (var d in checkboxes)
                    {
                        if (c.Name == d.Name)
                        {
                            d.IsSelected = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var config = Newtonsoft.Json.JsonConvert.SerializeObject(Checked);
            service.UpdateAppPoolsConfig(config, Key);
            var server = (ServerView)DataContext;
            server.CheckServerStatus(Key);
            Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var selectedCheckbox = (CheckBox)sender;
            string name = selectedCheckbox.Content.ToString();
            var checkedPool = new CheckedAppPools(name, true);
            Checked.Add(checkedPool);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var selectedCheckbox = (CheckBox)sender;
            string name = selectedCheckbox.Content.ToString();
            foreach (var c in Checked)
            {
                if (c.Name == name)
                {
                    Checked.Remove(c);
                    return;
                }
            }
        }
    }
}