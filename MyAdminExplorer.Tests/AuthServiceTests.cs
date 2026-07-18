using System;
using System.IO;
using FluentAssertions;
using MyAdminExplorer.Models;
using MyAdminExplorer.Services;
using Xunit;

namespace MyAdminExplorer.Tests
{
    public class AuthServiceTests : IDisposable
    {
        private readonly string _tempFile;

        public AuthServiceTests()
        {
            _tempFile = Path.Combine(Path.GetTempPath(), "users_" + Guid.NewGuid() + ".txt");
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }
        }

        [Fact]
        public void Authenticate_ReturnsFailure_ForUnknownUser()
        {
            File.WriteAllText(_tempFile, string.Empty);
            var auth = CreateAuthService();

            auth.Authenticate("missing", "password").Success.Should().BeFalse();
        }

        [Fact]
        public void Authenticate_ReturnsSuccess_ForValidLegacyPassword()
        {
            WriteUser("demo", LegacyMd5PasswordHasher.ComputeMd5Hex(System.Security.Cryptography.MD5.Create(), "secret"), 0, true);
            var auth = CreateAuthService();

            var result = auth.Authenticate("demo", "secret");

            result.Success.Should().BeTrue();
            result.User.Login.Should().Be("demo");
        }

        [Fact]
        public void Authenticate_UpgradesLegacyHashOnLogin()
        {
            WriteUser("demo", LegacyMd5PasswordHasher.ComputeMd5Hex(System.Security.Cryptography.MD5.Create(), "secret"), 0, true);
            var auth = CreateAuthService();

            auth.Authenticate("demo", "secret").Success.Should().BeTrue();

            var repository = new FileUserRepository(_tempFile);
            repository.FindByLogin("demo").Password.Should().StartWith(Pbkdf2PasswordHasher.Prefix);
        }

        private AuthService CreateAuthService()
        {
            return new AuthService(new FileUserRepository(_tempFile), new AccessPolicyService(), new CompositePasswordHasher());
        }

        private void WriteUser(string login, string passwordHash, byte role, bool even)
        {
            var line = string.Join("|", login, passwordHash, role, even, "2020-01-01", "2030-12-31", 0, 24, string.Empty);
            File.WriteAllText(_tempFile, line);
        }
    }
}
