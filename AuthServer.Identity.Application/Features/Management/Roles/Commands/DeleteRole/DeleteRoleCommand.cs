using AuthServer.Identity.Application.Wrappers;
using MediatR;

namespace AuthServer.Identity.Application.Features.Management.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest<ServiceResponse<bool>>
    {
        public string RoleId { get; set; }
    }
}
