namespace MyAdminExplorer.Services
{
    public class CompositePasswordHasher : IPasswordHasher
    {
        private readonly LegacyMd5PasswordHasher _legacyHasher = new LegacyMd5PasswordHasher();
        private readonly Pbkdf2PasswordHasher _modernHasher = new Pbkdf2PasswordHasher();

        public string Hash(string password) => _modernHasher.Hash(password);

        public bool Verify(string password, string storedHash)
        {
            if (_modernHasher.Verify(password, storedHash))
            {
                return true;
            }

            return _legacyHasher.Verify(password, storedHash);
        }

        public bool IsLegacyHash(string storedHash) => _legacyHasher.IsLegacyHash(storedHash);

        public bool ShouldUpgrade(string storedHash) => IsLegacyHash(storedHash);
    }
}
