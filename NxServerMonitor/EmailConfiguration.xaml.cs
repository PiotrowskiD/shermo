using NxServerMonitor.ServiceReference2;
using NxServerMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for MailConfiguration.xaml
    /// </summary>
    public partial class EmailConfiguration : Page
    {
        bool isSaved;
        IService1 service = ServiceFactory.Create();
        public List<EmailConfigViewModel> emailConfig = new List<EmailConfigViewModel>();
        List<string> configList = new List<string>();
        string Key;

        public EmailConfiguration(string key)
        {
            Key = key;
            InitializeComponent();
            SetConfig(key);
        }

        public void SetConfig(string key)
        {
            try
            {
                var emails = service.GetMailAdressesConfig(Key);
                var deserial = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(emails);

                foreach (var d in deserial)
                {
                    var email = new EmailConfigViewModel(d);
                    emailConfig.Add(email);
                }
                this.emailList.ItemsSource = emailConfig;
            }
            catch
            { }
        }

        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            isSaved = false;
            var newEmail = new AddNewEmail(Key);
            Content = newEmail;
            mainFrame.NavigationService.Navigate(newEmail);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (emailList.SelectedItem == null)
                return;
            var item = emailList.SelectedItem as EmailConfigViewModel;
            string name = item.EmailAddress;

            foreach (var email in emailConfig)
            {
                if (email.EmailAddress == name)
                {
                    emailConfig.Remove(email);
                    emailList.Items.Refresh();
                    isSaved = false;
                    saveButton_Click(new object(), new RoutedEventArgs());
                    return;
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSaved)
                return;

            foreach (var c in emailConfig)
            {
                string email = c.EmailAddress;
                configList.Add(email);
            }

            var config = Newtonsoft.Json.JsonConvert.SerializeObject(configList);
            service.UpdateMailAdressesConfig(config, Key);
            isSaved = true;
            Window window = (Window)DataContext;
            window.Close();
        }
    }
}