using MyAdminExplorer.Models;
using MyAdminExplorer.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyAdminExplorer
{
    public partial class ControlUser : Window
    {
        private readonly ControlUserViewModel _viewModel;

        public ControlUser()
        {
            InitializeComponent();
            _viewModel = new ControlUserViewModel();
            dataGrid.ItemsSource = _viewModel.Users;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            string message;
            _viewModel.SaveUsers(out message);
            MessageBox.Show(message);
        }

        private void EditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var user = e.Row.Item as User;
            if (user == null)
            {
                return;
            }

            if (e.Column.Header.ToString() == "Login")
            {
                var newLogin = ((TextBox)e.EditingElement).Text;
                if (_viewModel.IsLoginTaken(newLogin, user))
                {
                    MessageBox.Show("Such login already exists");
                    ((TextBox)e.EditingElement).Text = string.Empty;
                    e.Cancel = true;
                }

                return;
            }

            if (e.Column.Header.ToString() == "Password")
            {
                var plainPassword = ((TextBox)e.EditingElement).Text;
                if (!string.IsNullOrEmpty(plainPassword))
                {
                    ((TextBox)e.EditingElement).Text = _viewModel.HashPassword(plainPassword);
                }
            }
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            var selected = dataGrid.SelectedItem as User;
            if (selected != null)
            {
                _viewModel.RemoveUser(selected);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            var addUser = new AddUser { Owner = this };
            addUser.ShowDialog();

            if (addUser.User != null)
            {
                _viewModel.AddUser(addUser.User);
            }
        }

        private void Edit_Folders(object sender, RoutedEventArgs e)
        {
            var selected = dataGrid.SelectedItem as User;
            if (selected == null)
            {
                return;
            }

            var editForbidden = new EditForbidden(selected.ForbiddenList);
            editForbidden.ShowDialog();
            selected.ForbiddenList = editForbidden.Forbidden.ToList();
        }
    }
}
