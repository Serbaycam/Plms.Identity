using AuthServer.Identity.Domain.Common;

namespace AuthServer.Identity.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public string UserId { get; set; } // İşlemi yapan yönetici
        public string Action { get; set; } // Örn: "UpdateRolePermissions"
        public string EntityName { get; set; } // Örn: "AppRole"
        public string EntityId { get; set; } // İşlem yapılan kaydın ID'si
        public string Details { get; set; } // Yapılan değişikliğin JSON özeti
        public string IpAddress { get; set; }
    }
}