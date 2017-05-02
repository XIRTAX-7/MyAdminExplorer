using MyAdminExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        bool _admin;
        private User _user;
        public Menu(User user, bool fromAdmin)
        {
            InitializeComponent();        
            _admin = fromAdmin;
            _user = user;
            BackAdminPanel.Visibility = fromAdmin ? Visibility.Visible : Visibility.Hidden;
        }

        private void GoToExplorer(object sender, RoutedEventArgs e)
        {
            Explorer explorer = new Explorer(_user.ForbiddenList);
            explorer.Owner = this;
            explorer.Show();
            Hide();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
         
            ChangePass changePass = new ChangePass(_user);
            changePass.Owner = this;
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
