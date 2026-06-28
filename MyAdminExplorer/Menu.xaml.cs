using MyAdminExplorer.Models;
using System.Windows;

namespace MyAdminExplorer
{
    public partial class Menu : Window
    {
        private readonly User _user;

        public Menu(User user, bool fromAdmin)
        {
            InitializeComponent();
            _user = user;
            BackAdminPanel.Visibility = fromAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoToExplorer(object sender, RoutedEventArgs e)
        {
            var explorer = new Explorer(_user.ForbiddenList) { Owner = this };
            explorer.Show();
            Hide();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            Close();
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            var changePass = new ChangePass(_user) { Owner = this };
            changePass.Show();
            Hide();
        }

        private void BackAdmin(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }
    }
}
