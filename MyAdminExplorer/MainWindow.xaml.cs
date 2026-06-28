using MyAdminExplorer.Infrastructure;
using MyAdminExplorer.ViewModels;
using System.Windows;

namespace MyAdminExplorer
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            var result = _viewModel.TryLogin(login.Text, password.Password);
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage);
                return;
            }

            _viewModel.NavigateAfterLogin(this, result.User);
        }
    }
}
