using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NxServerMonitor.ViewModels;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for AddNewEmail.xaml
    /// </summary>
    public partial class AddNewEmail : Page
    {
        string Key;

        public AddNewEmail(string key)
        {
            Key = key;
            InitializeComponent();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            var config = new EmailConfiguration(Key);
            Content = config;
            NavigationService.Navigate(config);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            var isValid = IsValidEmail(email);
            if (!isValid)
                MessageBox.Show("Incorrect email format");
            else
            {
                var emailAddress = new EmailConfigViewModel(email);
                var config = new EmailConfiguration(Key);
                config.emailConfig.Add(emailAddress);
                config.emailList.Items.Refresh();
                Content = config;
                NavigationService.Navigate(config);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}