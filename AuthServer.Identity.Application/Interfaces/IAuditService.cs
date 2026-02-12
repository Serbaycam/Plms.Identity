namespace AuthServer.Identity.Application.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(string userId, string action, string entityName, string entityId, object details, string ipAddress);
    }
}