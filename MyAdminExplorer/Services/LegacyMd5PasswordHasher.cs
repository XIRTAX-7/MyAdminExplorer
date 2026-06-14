using System.Security.Cryptography;
using System.Text;

namespace MyAdminExplorer.Services
{
    public class LegacyMd5PasswordHasher : IPasswordHasher
    {
        public const string Prefix = "md5:";

        public string Hash(string password)
        {
            using (var md5 = MD5.Create())
            {
                return Prefix + ComputeMd5Hex(md5, password);
            }
        }

        public bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            var hashValue = storedHash.StartsWith(Prefix, System.StringComparison.OrdinalIgnoreCase)
                ? storedHash.Substring(Prefix.Length)
                : storedHash;

            using (var md5 = MD5.Create())
            {
                return hashValue == ComputeMd5Hex(md5, password);
            }
        }

        public bool IsLegacyHash(string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            if (storedHash.StartsWith(Pbkdf2PasswordHasher.Prefix, System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var value = storedHash.StartsWith(Prefix, System.StringComparison.OrdinalIgnoreCase)
                ? storedHash.Substring(Prefix.Length)
                : storedHash;

            return value.Length == 32;
        }

        public static string ComputeMd5Hex(MD5 md5, string input)
        {
            var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var builder = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
