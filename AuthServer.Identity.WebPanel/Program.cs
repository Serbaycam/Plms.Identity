using System.Security.Claims;
using AuthServer.Identity.WebPanel.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<AuthApiOptions>(builder.Configuration.GetSection("AuthApi"));
builder.Services.AddHttpClient<AuthApiClient>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/account/login";
        o.ExpireTimeSpan = TimeSpan.FromDays(7);
        o.SlidingExpiration = true;

        o.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async ctx =>
            {
                var props = ctx.Properties;

                var accessToken = AuthTicketTokenStore.GetAccessToken(props);
                var refreshToken = AuthTicketTokenStore.GetRefreshToken(props);
                var accessExp = AuthTicketTokenStore.GetAccessExp(props);
                var refreshExp = AuthTicketTokenStore.GetRefreshExp(props);

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken) || accessExp is null || refreshExp is null)
                    return;

                var now = DateTimeOffset.UtcNow;

                // Refresh token da bitmişse -> oturumu düşür
                if (refreshExp.Value <= now)
                {
                    ctx.RejectPrincipal();
                    await ctx.HttpContext.SignOutAsync();
                    return;
                }

                // Access token 2 dk içinde bitecekse refresh
                if (accessExp.Value - now > TimeSpan.FromMinutes(2))
                    return;

                // session bazlı lock (claim yoksa Name kullan)
                var sessionKey = ctx.Principal?.FindFirstValue(ClaimTypes.Sid)
                                ?? ctx.Principal?.Identity?.Name
                                ?? "default";

                using var _ = await SessionRefreshLock.AcquireAsync(sessionKey);

                // Lock aldıktan sonra tekrar oku (başka request refresh etmiş olabilir)
                var auth2 = await ctx.HttpContext.AuthenticateAsync();
                var props2 = auth2.Properties;
                if (props2 is null) return;

                var at2 = AuthTicketTokenStore.GetAccessToken(props2);
                var rt2 = AuthTicketTokenStore.GetRefreshToken(props2);
                var ax2 = AuthTicketTokenStore.GetAccessExp(props2);

                if (string.IsNullOrEmpty(at2) || string.IsNullOrEmpty(rt2) || ax2 is null)
                    return;

                if (ax2.Value - DateTimeOffset.UtcNow > TimeSpan.FromMinutes(2))
                    return; // zaten yenilenmiş

                var api = ctx.HttpContext.RequestServices.GetRequiredService<AuthApiClient>();
                var refreshed = await api.RefreshAsync(at2, rt2);

                if (refreshed is null)
                {
                    // Refresh başarısız => login'e düşsün (sonsuz 401 döngüsü kırılır)
                    ctx.RejectPrincipal();
                    await ctx.HttpContext.SignOutAsync();
                    return;
                }

                // API DateTime kind belirsiz gelirse UTC varsayalım
                DateTimeOffset accessExpUtc = ToUtc(refreshed.AccessTokenExpiration);
                DateTimeOffset refreshExpUtc = ToUtc(refreshed.RefreshTokenExpiration);

                AuthTicketTokenStore.Set(props2, refreshed.AccessToken, refreshed.RefreshToken, accessExpUtc, refreshExpUtc);

                ctx.ShouldRenew = true; // Cookie yeniden yazılsın
            }
        };
        o.Events.OnSigningOut = async ctx =>
        {
            var props = ctx.Properties;
            if (props is null) return;

            var rt = AuthTicketTokenStore.GetRefreshToken(props);
            var at = AuthTicketTokenStore.GetAccessToken(props);

            if (!string.IsNullOrWhiteSpace(rt))
            {
                var api = ctx.HttpContext.RequestServices.GetRequiredService<AuthApiClient>();
                await api.RevokeAsync(rt, bearerAccessToken: at);
            }
        };
        static DateTimeOffset ToUtc(DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Utc) return new DateTimeOffset(dt);
            if (dt.Kind == DateTimeKind.Local) return dt.ToUniversalTime();
            // Unspecified -> UTC varsay
            return new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Utc));
        }
    });

builder.Services.AddAuthorization(o =>
{
    o.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();