using FluentAssertions;
using MyAdminExplorer.Services;
using Xunit;

namespace MyAdminExplorer.Tests
{
    public class Pbkdf2PasswordHasherTests
    {
        private readonly Pbkdf2PasswordHasher _hasher = new Pbkdf2PasswordHasher();

        [Fact]
        public void HashAndVerify_ReturnsTrue_ForSamePassword()
        {
            var hash = _hasher.Hash("password123");

            _hasher.Verify("password123", hash).Should().BeTrue();
        }

        [Fact]
        public void Verify_ReturnsFalse_ForDifferentPassword()
        {
            var hash = _hasher.Hash("password123");

            _hasher.Verify("wrong", hash).Should().BeFalse();
        }

        [Fact]
        public void Hash_GeneratesDifferentSalts()
        {
            var first = _hasher.Hash("password123");
            var second = _hasher.Hash("password123");

            first.Should().NotBe(second);
            _hasher.Verify("password123", first).Should().BeTrue();
            _hasher.Verify("password123", second).Should().BeTrue();
        }
    }
}
