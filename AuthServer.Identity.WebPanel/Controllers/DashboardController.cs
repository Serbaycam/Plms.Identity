using AuthServer.Identity.WebPanel.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AuthServer.Identity.WebPanel.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public DashboardController(IHttpClientFactory factory) => _factory = factory;
        public async Task<IActionResult> GetStatsAsync()
        {
            var auth = await HttpContext.AuthenticateAsync();
            var props = auth.Properties;
            if (props is null) return RedirectToAction("Login", "Account");

            var at = AuthTicketTokenStore.GetAccessToken(props);
            if (string.IsNullOrEmpty(at)) return RedirectToAction("Login", "Account");

            var http = _factory.CreateClient();
            http.BaseAddress = new Uri("https://localhost:7023/");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", at);

            var resp = await http.GetAsync("api/secure/ping");
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Normalde ValidatePrincipal refresh edeceği için nadir olmalı.
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            return Content(await resp.Content.ReadAsStringAsync());
        }
    }
}
