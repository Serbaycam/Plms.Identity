using MediatR;
using Plms.Identity.Application.Wrappers;
using System.Text.Json.Serialization;

namespace Plms.Identity.Application.Features.Auth.Commands.Revoke
{
  public class RevokeTokenCommand : IRequest<ServiceResponse<bool>>
  {
    public string Token { get; set; }

    [JsonIgnore]
    public string? IpAddress { get; set; }
  }
}