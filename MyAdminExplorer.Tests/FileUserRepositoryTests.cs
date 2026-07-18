using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using MyAdminExplorer.Models;
using MyAdminExplorer.Services;
using Xunit;

namespace MyAdminExplorer.Tests
{
    public class FileUserRepositoryTests : IDisposable
    {
        private readonly string _tempFile;

        public FileUserRepositoryTests()
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
        public void TryParseLine_ParsesValidRecord()
        {
            var line = "john|hash|0|true|2024-01-01|2024-12-31|8|18|Windows?Program Files";

            var user = FileUserRepository.TryParseLine(line);

            user.Should().NotBeNull();
            user.Login.Should().Be("john");
            user.Password.Should().Be("hash");
            user.Role.Should().Be(0);
            user.Even.Should().BeTrue();
            user.FromTime.Should().Be(8);
            user.ToTime.Should().Be(18);
            user.ForbiddenList.Should().BeEquivalentTo("Windows", "Program Files");
        }

        [Fact]
        public void LoadAll_ReturnsEmpty_WhenFileMissing()
        {
            var repository = new FileUserRepository(_tempFile);

            repository.LoadAll().Should().BeEmpty();
        }

        [Fact]
        public void LoadAll_SkipsBrokenLines()
        {
            File.WriteAllLines(_tempFile, new[]
            {
                "broken-line",
                "valid|hash|0|true|2024-01-01|2024-12-31|0|24|"
            });

            var repository = new FileUserRepository(_tempFile);
            repository.LoadAll().Should().ContainSingle(u => u.Login == "valid");
        }

        [Fact]
        public void SaveAll_RoundTripsUsers()
        {
            var repository = new FileUserRepository(_tempFile);
            var users = new[]
            {
                new User
                {
                    Login = "alice",
                    Password = "pbkdf2:100000$c2FsdA==$aGFzaA==",
                    Role = 0,
                    Even = false,
                    From = new DateTime(2024, 1, 1),
                    To = new DateTime(2024, 12, 31),
                    FromTime = 0,
                    ToTime = 24,
                    ForbiddenList = { "Windows" }
                }
            };

            repository.SaveAll(users);
            var loaded = repository.LoadAll();

            loaded.Should().ContainSingle();
            loaded[0].Login.Should().Be("alice");
            loaded[0].ForbiddenList.Should().ContainSingle("Windows");
        }
    }
}
