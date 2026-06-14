namespace MyAdminExplorer.Services
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string storedHash);
        bool IsLegacyHash(string storedHash);
    }
}
