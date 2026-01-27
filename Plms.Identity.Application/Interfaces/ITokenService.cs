using Plms.Identity.Application.Dtos;
using Plms.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Plms.Identity.Application.Interfaces
{
  public interface ITokenService
  {
    Task<TokenDto> CreateTokenAsync(AppUser user, IList<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
  }
}