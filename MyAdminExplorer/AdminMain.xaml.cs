using MyAdminExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyAdminExplorer
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class AdminMain : Window
    {
        private User _user;
        public AdminMain(User user)
        {
            InitializeComponent();
            _user = user;
            welcome.Content = "Welcome to Admin Panel, " + user.Login;
        }

        private void GoControlUser(object sender, RoutedEventArgs e)
        {
            ControlUser controlUser = new ControlUser();
            controlUser.Owner = this;
            controlUser.Show();
            Hide();
        }

        private void Main(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(_user, true);
            menu.Owner = this;
            menu.Show();
            Hide();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
