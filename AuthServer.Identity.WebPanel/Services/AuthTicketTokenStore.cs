using System.Globalization;
using Microsoft.AspNetCore.Authentication;
namespace AuthServer.Identity.WebPanel.Services
{
    public static class AuthTicketTokenStore
    {
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string AccessExpiresAtKey = "access_expires_at";
        private const string RefreshExpiresAtKey = "refresh_expires_at";

        public static void Set(AuthenticationProperties props, string accessToken, string refreshToken,
            DateTimeOffset accessExpUtc, DateTimeOffset refreshExpUtc)
        {
            props.UpdateTokenValue(AccessTokenKey, accessToken);
            props.UpdateTokenValue(RefreshTokenKey, refreshToken);
            props.UpdateTokenValue(AccessExpiresAtKey, accessExpUtc.ToString("o", CultureInfo.InvariantCulture));
            props.UpdateTokenValue(RefreshExpiresAtKey, refreshExpUtc.ToString("o", CultureInfo.InvariantCulture));
        }

        public static string? GetAccessToken(AuthenticationProperties props) => props.GetTokenValue(AccessTokenKey);
        public static string? GetRefreshToken(AuthenticationProperties props) => props.GetTokenValue(RefreshTokenKey);

        public static DateTimeOffset? GetAccessExp(AuthenticationProperties props) => Parse(props.GetTokenValue(AccessExpiresAtKey));
        public static DateTimeOffset? GetRefreshExp(AuthenticationProperties props) => Parse(props.GetTokenValue(RefreshExpiresAtKey));

        private static DateTimeOffset? Parse(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dto))
                return dto.ToUniversalTime();
            return null;
        }
    }
}
