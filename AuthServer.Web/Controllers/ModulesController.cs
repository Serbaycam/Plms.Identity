using AuthServer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AuthServer.Web.Controllers
{
  public class ModulesController : Controller
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public ModulesController(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    // GET: /Modules
    public async Task<IActionResult> Index()
    {
      // Gateway'e istek atacak Client'ı al
      var client = _httpClientFactory.CreateClient("Gateway");

      // Veriyi çek
      var response = await client.GetAsync("/api/testmodules");

      if (response.IsSuccessStatusCode)
      {
        var jsonString = await response.Content.ReadAsStringAsync();
        var modules = JsonSerializer.Deserialize<List<TestModuleDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return View(modules);
      }

      // Hata varsa boş liste dön
      return View(new List<TestModuleDto>());
    }
  }
}