using AuthServer.Identity.Domain.Entities; // AppUser nerede tanımlıysa
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;

    public UsersController(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      // Şifreleri (Hash) geri dönmemek için Select ile seçiyoruz
      var users = await _userManager.Users
          .Select(u => new { u.Id, u.UserName, u.Email })
          .ToListAsync();
      return Ok(users);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null) return NotFound();

      var result = await _userManager.DeleteAsync(user);
      if (result.Succeeded) return Ok(new { message = "Kullanıcı silindi." });

      return BadRequest(result.Errors);
    }

    // Kullanıcıya Rol Atama Endpoint'i
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null) return NotFound("Kullanıcı bulunamadı.");

      var result = await _userManager.AddToRoleAsync(user, roleName);
      if (result.Succeeded) return Ok();

      return BadRequest(result.Errors);
    }
  }
}