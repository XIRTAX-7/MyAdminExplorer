using MyAdminExplorer.Infrastructure;
using System.Windows;

namespace MyAdminExplorer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppPaths.EnsureUsersFile();
        }
    }
}
