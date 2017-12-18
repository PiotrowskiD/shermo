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
    /// Interaction logic for ServicesConfiguration.xaml
    /// </summary>
    public partial class ServicesConfiguration : Window
    {
        IService1 service = ServiceFactory.Create();
        List<CheckedServices> Checked = new List<CheckedServices>();
        ObservableCollection<CheckedServices> checkboxes = new ObservableCollection<CheckedServices>();
        string Key;

        public ServicesConfiguration(string key)
        {
            Key = key;

            InitializeComponent();
            SetConfig(key);
        }

        private void SetConfig(string key)
        {
            var services = service.GetServerOtherParams("Windows Services installed", key);
            var value = services.Value;
            var deserial = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WindowsServicesValue>>(value);

            foreach (var d in deserial)
            {
                var checkedService = new CheckedServices(d.ServiceName, false);
                checkboxes.Add(checkedService);
            }
            CheckTheBoxes(key);
            serviceList.ItemsSource = checkboxes;
        }

        private void CheckTheBoxes(string key)
        {
            try
            {
                var configuration = service.GetServicesConfig(key);
                var deserializeConf = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckedServices>>(configuration);

                foreach (var c in deserializeConf)
                {
                    Checked.Add(new CheckedServices(c.ServiceName, true));

                    foreach (var d in checkboxes)
                    {
                        if (c.ServiceName == d.ServiceName)
                        {
                            d.IsSelected = true;
                        }
                    }
                }
            }
            catch
            { }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var serial = Newtonsoft.Json.JsonConvert.SerializeObject(Checked);
            service.UpdateServicesConfig(serial, Key);
            ServerView server = (ServerView)DataContext;
            server.ServicesListView.Items.Refresh();
            server.CheckServerStatus(Key);
            Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var selectedCheckbox = (CheckBox)sender;
            string name = selectedCheckbox.Content.ToString();
            var checkedService = new CheckedServices(name, true);
            Checked.Add(checkedService);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var selectedCheckbox = (CheckBox)sender;
            string name = selectedCheckbox.Content.ToString();
            foreach (var c in Checked)
            {
                if (c.ServiceName == name)
                {
                    Checked.Remove(c);
                    return;
                }
            }
        }
    }
}