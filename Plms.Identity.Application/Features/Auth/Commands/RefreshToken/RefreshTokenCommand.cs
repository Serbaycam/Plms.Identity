using MediatR;
using Plms.Identity.Application.Dtos;
using Plms.Identity.Application.Wrappers;
using System.Text.Json.Serialization;

namespace Plms.Identity.Application.Features.Auth.Commands.RefreshToken
{
  public class RefreshTokenCommand : IRequest<ServiceResponse<TokenDto>>
  {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    [JsonIgnore]
    public string? IpAddress { get; set; }
  }
}