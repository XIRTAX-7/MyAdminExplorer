using MyAdminExplorer.Models;
using MyAdminExplorer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MyAdminExplorer
{
    public partial class AddUser : Window
    {
        private readonly FileUserRepository _repository = new FileUserRepository();
        private readonly AuthService _authService = new AuthService();
        private List<string> _forbidden = new List<string>();

        public User User { get; set; }

        public AddUser()
        {
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (_repository.LoadAll().Any(u => string.Equals(u.Login, login.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Such login already exists");
                return;
            }

            if (!int.TryParse(fromtime.Text, out var fromTime) ||
                !int.TryParse(totime.Text, out var toTime) ||
                fromTime < 0 ||
                toTime > 24)
            {
                MessageBox.Show("The entered time is incorrect");
                return;
            }

            if (string.IsNullOrEmpty(login.Text) ||
                string.IsNullOrEmpty(password.Password) ||
                from.SelectedDate == null ||
                to.SelectedDate == null)
            {
                MessageBox.Show("The entered data is incorrect");
                return;
            }

            User = new User
            {
                Login = login.Text,
                Password = _authService.HashPassword(password.Password),
                Role = 0,
                Even = even.IsChecked.GetValueOrDefault(),
                From = from.SelectedDate.Value.Date,
                To = to.SelectedDate.Value.Date,
                FromTime = fromTime,
                ToTime = toTime,
                ForbiddenList = new List<string>(_forbidden)
            };
            Close();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditBlockedFolders(object sender, RoutedEventArgs e)
        {
            var editForbidden = new EditForbidden(_forbidden);
            editForbidden.ShowDialog();
            _forbidden = editForbidden.Forbidden.ToList();
        }
    }
}
