using AuthServer.Identity.Application.Wrappers;
using MediatR;

namespace AuthServer.Identity.Application.Features.Auth.Commands.RevokeAll
{
    public class RevokeAllTokensCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid UserId { get; set; } // Hangi kullanıcının fişini çekeceğiz?
    }
}