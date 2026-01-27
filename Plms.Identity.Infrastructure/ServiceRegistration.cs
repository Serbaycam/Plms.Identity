using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plms.Identity.Application.Interfaces;
using Plms.Identity.Infrastructure.Services;
using Plms.Identity.Infrastructure.Settings;

namespace Plms.Identity.Infrastructure
{
  public static class ServiceRegistration
  {
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
      // AppSettings.json'daki veriyi sınıfa map ediyoruz
      services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

      // Interface ve Implementation'ı eşleştiriyoruz
      services.AddTransient<ITokenService, TokenService>();
    }
  }
}