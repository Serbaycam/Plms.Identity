using System.Text.Json.Serialization;
namespace AuthServer.Identity.WebPanel.Models
{
    public sealed class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public sealed class RefreshTokenRequest
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
    public sealed class RevokeTokenRequest
    {
        public string Token { get; set; } = "";
    }
    public sealed class TokenDto
    {
        public string AccessToken { get; set; } = "";
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; } = "";
        public DateTime RefreshTokenExpiration { get; set; }
    }

    public sealed class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
