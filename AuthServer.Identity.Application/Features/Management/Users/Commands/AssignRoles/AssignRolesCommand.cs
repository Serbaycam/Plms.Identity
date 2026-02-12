using AuthServer.Identity.Application.Wrappers;
using MediatR;

namespace AuthServer.Identity.Application.Features.Management.Users.Commands.AssignRoles
{
    public class AssignRolesCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid UserId { get; set; }
        public List<string> Roles { get; set; } // Örn: ["LabManager", "Basic"]
    }
}
