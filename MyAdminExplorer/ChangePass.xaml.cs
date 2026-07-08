using MyAdminExplorer.Models;
using MyAdminExplorer.Services;
using System.Windows;

namespace MyAdminExplorer
{
    public partial class ChangePass : Window
    {
        private readonly User _user;
        private readonly AuthService _authService = new AuthService();

        public ChangePass(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Owner.Show();
        }

        private void Change(object sender, RoutedEventArgs e)
        {
            if (password.Password != confirmpassword.Password)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            _authService.ChangePassword(_user, password.Password);
            Close();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
