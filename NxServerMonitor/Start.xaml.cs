using System.Windows;
using System.Windows.Forms;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            //   var conf = new ServicesConfiguration("312c0161-bc5a-4423-bd54-1bff832d977b");
            //var conf = new AppPoolsConfiguration("312c0161-bc5a-4423-bd54-1bff832d977b");
            var list = new Clients();
            //var groups = new GroupManagement();
            //groups.Show();
            //conf.Show();
            //var mail = new EmailConfiguration("312c0161-bc5a-4423-bd54-1bff832d977b");
            //Window w = new Window();
            //w.Width = 300;
            //w.Height = 300;
            //w.Content = mail;
            //w.Show()
            //var list = new ServerView("0", "4902e260-3487-4e9b-9916-7d4f0c5b0056"); //komputer
            // var list = new ServerView("0", "312c0161-bc5a-4423-bd54-1bff832d977b"); //sp2013
            //var list = new View("0", "312c0161-bc5a-4423-bd54-1bff832d977b");
            //var list = new Disk("0", "312c0161-bc5a-4423-bd54-1bff832d977b", "C:\\ Lord Disk");
            this.Content = list;
            mainFrame.NavigationService.Navigate(list);
            //var url = new URLConfiguration("312c0161-bc5a-4423-bd54-1bff832d977b");
            //url.Show();
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        void myForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Clients.WriteGroupList();
        }
    }
}
