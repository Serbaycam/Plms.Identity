using AuthServer.Identity.Application.Wrappers;
using MediatR;

namespace AuthServer.Identity.Application.Features.Management.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<ServiceResponse<bool>>
    {
        public string RoleId { get; set; }
        public string NewRoleName { get; set; }
    }
}
