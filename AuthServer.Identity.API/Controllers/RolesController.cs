using AuthServer.Identity.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  // [Authorize(Roles = "SuperAdmin")] // Şimdilik kapalı, testten sonra açacağız
  public class RolesController : ControllerBase
  {
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
      _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var roles = await _roleManager.Roles.ToListAsync();
      return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string roleName)
    {
      if (string.IsNullOrEmpty(roleName)) return BadRequest("Rol adı boş olamaz.");

      var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
      if (result.Succeeded) return Ok(new { message = "Rol oluşturuldu." });

      return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      var role = await _roleManager.FindByIdAsync(id);
      if (role == null) return NotFound();

      var result = await _roleManager.DeleteAsync(role);
      if (result.Succeeded) return Ok(new { message = "Rol silindi." });

      return BadRequest(result.Errors);
    }
  }
}