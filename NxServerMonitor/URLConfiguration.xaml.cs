using NxServerMonitor.ServiceReference2;
using NxServerMonitor.ViewModels;
using System.Collections.Generic;
using System.Windows;
using ServerMonitor.Model;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for URLConfiguration.xaml
    /// </summary>
    public partial class URLConfiguration : Window
    {
        IService1 service = ServiceFactory.Create();
        string Key;
        public static List<URLValue> URLList = new List<URLValue>();
        public URLConfiguration(string key)
        {
            Key = key;
            InitializeComponent();
            SetUrlList();
        }

        private void SetUrlList()
        {
            try
            {
                var config = service.GetUrlConfig(Key);
                var deserial = Newtonsoft.Json.JsonConvert.DeserializeObject<List<URLValue>>(config);
                foreach (var d in deserial)
                {
                    URLList.Add(d);
                }
            }
            catch
            { }
            urlList.ItemsSource = URLList;
        }


        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            var addNew = new AddNewURL();
            addNew.DataContext = this;
            addNew.ShowDialog();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (urlList.SelectedItem == null)
                return;
            var item = urlList.SelectedItem as URLValue;
            foreach (var c in URLList)
            {
                if (c.Url == item.Url)
                {
                    URLList.Remove(c);
                    urlList.Items.Refresh();
                    return;
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var config = Newtonsoft.Json.JsonConvert.SerializeObject(URLList);
            service.UpdateUrlConfig(config, Key);
            URLList.Clear();
            Close();
        }
    }
}
