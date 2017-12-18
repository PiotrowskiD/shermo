using NxServerMonitor.ViewModels;
using System.Windows;

namespace NxServerMonitor
{
    /// <summary>
    /// Interaction logic for AddNewGroup.xaml
    /// </summary>
    public partial class AddNewGroup : Window
    {
        bool isSaved;
        public AddNewGroup()
        {
            InitializeComponent();
        }

        public void save_Click(object sender, RoutedEventArgs e)
        {
            if (isSaved)
                return;

            var groupName = groupTextBox.Text;

            var groupManagement = (GroupManagement)DataContext;
            var modelgrupy = new GroupModel(groupName);

            GroupManagement.groupInfoList.Add(modelgrupy);
            groupManagement.groupsList.Items.Refresh();
            isSaved = true;
            Close();
        }
    }
}