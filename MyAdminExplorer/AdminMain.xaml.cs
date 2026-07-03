using MyAdminExplorer.Models;
using System.Windows;

namespace MyAdminExplorer
{
    public partial class AdminMain : Window
    {
        private readonly User _user;

        public AdminMain(User user)
        {
            InitializeComponent();
            _user = user;
            welcome.Content = "Welcome to Admin Panel, " + user.Login;
        }

        private void GoControlUser(object sender, RoutedEventArgs e)
        {
            var controlUser = new ControlUser { Owner = this };
            controlUser.Show();
            Hide();
        }

        private void Main(object sender, RoutedEventArgs e)
        {
            var menu = new Menu(_user, true) { Owner = this };
            menu.Show();
            Hide();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
