namespace MyAdminExplorer.Services
{
    public enum AccessResult
    {
        Allowed,
        DeniedWrongPassword,
        DeniedOddDay,
        DeniedEvenDay,
        DeniedExpired,
        DeniedNotYetActive
    }
}
