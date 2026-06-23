using System.Collections.ObjectModel;
using System.Linq;
using MyAdminExplorer.Models;
using MyAdminExplorer.Services;

namespace MyAdminExplorer.ViewModels
{
    public class ControlUserViewModel
    {
        private readonly FileUserRepository _repository;
        private readonly AuthService _authService;

        public ControlUserViewModel()
            : this(new FileUserRepository(), new AuthService())
        {
        }

        public ControlUserViewModel(FileUserRepository repository, AuthService authService)
        {
            _repository = repository;
            _authService = authService;
            Users = new ObservableCollection<User>(_repository.LoadAll());
        }

        public ObservableCollection<User> Users { get; }

        public bool SaveUsers(out string message)
        {
            var invalidUsers = Users.Where(u => string.IsNullOrWhiteSpace(u.Login) || string.IsNullOrWhiteSpace(u.Password)).ToList();
            if (invalidUsers.Any())
            {
                message = "Not all data is saved because some were incorrect";
                _repository.SaveAll(Users.Where(u => !string.IsNullOrWhiteSpace(u.Login) && !string.IsNullOrWhiteSpace(u.Password)));
                return false;
            }

            _repository.SaveAll(Users);
            message = "All data was saved";
            return true;
        }

        public bool IsLoginTaken(string login, User currentUser = null)
        {
            return Users.Any(u =>
                u != currentUser &&
                string.Equals(u.Login, login, System.StringComparison.OrdinalIgnoreCase));
        }

        public string HashPassword(string password) => _authService.HashPassword(password);

        public void RemoveUser(User user) => Users.Remove(user);

        public void AddUser(User user) => Users.Add(user);
    }
}
