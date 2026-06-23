using System;
using System.Windows;
using MyAdminExplorer.Models;
using MyAdminExplorer.Services;

namespace MyAdminExplorer.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly AuthService _authService;

        public MainWindowViewModel()
            : this(new AuthService())
        {
        }

        public MainWindowViewModel(AuthService authService)
        {
            _authService = authService;
        }

        public AuthResult TryLogin(string login, string password) =>
            _authService.Authenticate(login, password);

        public void NavigateAfterLogin(Window currentWindow, User user)
        {
            if (user.Role == 0)
            {
                var menu = new Menu(user, false);
                menu.Show();
            }
            else if (user.Role == 1)
            {
                var adminMain = new AdminMain(user);
                adminMain.Show();
            }

            currentWindow.Close();
        }
    }
}
