using System;
using System.Security.Cryptography;
using System.Text;

namespace MyAdminExplorer.Services
{
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        public const string Prefix = "pbkdf2:";
        private const int Iterations = 100_000;
        private const int SaltSize = 16;
        private const int KeySize = 32;

        public string Hash(string password)
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = DeriveKey(password, salt);
            return string.Format(
                "{0}{1}${2}${3}",
                Prefix,
                Iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        public bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash) ||
                !storedHash.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var parts = storedHash.Substring(Prefix.Length).Split('$');
            if (parts.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out var iterations))
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[1]);
            var expectedHash = Convert.FromBase64String(parts[2]);
            var actualHash = DeriveKey(password, salt, iterations);
            return FixedTimeEquals(actualHash, expectedHash);
        }

        public bool IsLegacyHash(string storedHash)
        {
            return false;
        }

        private static byte[] DeriveKey(string password, byte[] salt, int iterations = Iterations)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return deriveBytes.GetBytes(KeySize);
            }
        }

        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }

            var diff = 0;
            for (var i = 0; i < left.Length; i++)
            {
                diff |= left[i] ^ right[i];
            }

            return diff == 0;
        }
    }
}
