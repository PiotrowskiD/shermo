using ServerMonitor.Model;
using System;
using System.Windows;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for AddNewURL.xaml
    /// </summary>
    public partial class AddNewURL : Window
    {
        bool isSaved;
        public AddNewURL()
        {
            InitializeComponent();
            SetCombobox();
        }

        public enum URLtype
        {
            SharePoint,
            WebService,
        }

        private void SetCombobox()
        {
            foreach (var type in Enum.GetValues(typeof(URLtype)))
            {
                urlComboBox.Items.Add(type);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (isSaved)
                return;

            var urlName = urlTextBox.Text;
            var urlType = urlComboBox.SelectedItem.ToString();

            var urlConfig = (URLConfiguration)DataContext;

            URLConfiguration.URLList.Add(new URLValue(urlName, urlType));
            urlConfig.urlList.Items.Refresh();
            isSaved = true;
            Close();
        }
    }
}
