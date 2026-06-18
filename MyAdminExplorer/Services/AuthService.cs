using MyAdminExplorer.Models;

namespace MyAdminExplorer.Services
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public User User { get; set; }
        public AccessResult AccessResult { get; set; }
        public string ErrorMessage { get; set; }

        public static AuthResult Failed(string message) =>
            new AuthResult { Success = false, ErrorMessage = message };

        public static AuthResult Succeeded(User user) =>
            new AuthResult { Success = true, User = user, AccessResult = AccessResult.Allowed };
    }

    public class AuthService
    {
        private readonly FileUserRepository _repository;
        private readonly AccessPolicyService _accessPolicy;
        private readonly CompositePasswordHasher _passwordHasher;

        public AuthService()
            : this(new FileUserRepository(), new AccessPolicyService(), new CompositePasswordHasher())
        {
        }

        public AuthService(
            FileUserRepository repository,
            AccessPolicyService accessPolicy,
            CompositePasswordHasher passwordHasher)
        {
            _repository = repository;
            _accessPolicy = accessPolicy;
            _passwordHasher = passwordHasher;
        }

        public AuthResult Authenticate(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrEmpty(password))
            {
                return AuthResult.Failed("Incorrect Login or Password");
            }

            var user = _repository.FindByLogin(login);
            if (user == null || !_passwordHasher.Verify(password, user.Password))
            {
                return AuthResult.Failed("Incorrect Login or Password");
            }

            if (_passwordHasher.ShouldUpgrade(user.Password))
            {
                user.Password = _passwordHasher.Hash(password);
                _repository.UpdatePassword(user.Login, user.Password, user.Role, user);
            }

            var accessResult = _accessPolicy.Evaluate(user, System.DateTime.Now);
            if (accessResult != AccessResult.Allowed)
            {
                return new AuthResult
                {
                    Success = false,
                    User = user,
                    AccessResult = accessResult,
                    ErrorMessage = _accessPolicy.GetDeniedMessage(user, accessResult)
                };
            }

            return AuthResult.Succeeded(user);
        }

        public void ChangePassword(User user, string newPassword)
        {
            var hash = _passwordHasher.Hash(newPassword);
            user.Password = hash;
            _repository.UpdatePassword(user.Login, hash, user.Role, user);
        }

        public string HashPassword(string password) => _passwordHasher.Hash(password);
    }
}
