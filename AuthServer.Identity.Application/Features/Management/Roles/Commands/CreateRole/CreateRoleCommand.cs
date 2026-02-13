using AuthServer.Identity.Application.Wrappers;
using MediatR;

namespace AuthServer.Identity.Application.Features.Management.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<ServiceResponse<string>>
    {
        public string RoleName { get; set; }
    }
}
