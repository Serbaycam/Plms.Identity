using MediatR;
using Plms.Identity.Application.Dtos;
using Plms.Identity.Application.Wrappers;
using System.Text.Json.Serialization;

namespace Plms.Identity.Application.Features.Auth.Commands.Login
{
  // Başarılı olursa TokenDto dönecek
  public class LoginCommand : IRequest<ServiceResponse<TokenDto>>
  {
    public string Email { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public string? IpAddress { get; set; }
  }
}