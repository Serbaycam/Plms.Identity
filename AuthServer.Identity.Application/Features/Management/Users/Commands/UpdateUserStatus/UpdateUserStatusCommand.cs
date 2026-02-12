using AuthServer.Identity.Application.Wrappers;
using MediatR;

namespace AuthServer.Identity.Application.Features.Management.Users.Commands.UpdateUserStatus
{
    public class UpdateUserStatusCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}