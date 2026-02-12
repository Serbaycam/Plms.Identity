using AuthServer.Identity.Application.Features.Management.Roles.Commands.UpdateRolePermissions;
using AuthServer.Identity.Application.Wrappers;
using AuthServer.Identity.Domain.Constants;
using AuthServer.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace AuthServer.Identity.API.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleManagementController(IMediator mediator, RoleManager<AppRole> roleManager)
        {
            _mediator = mediator;
            _roleManager = roleManager;
        }

        [HttpGet("all-permissions")]
        public IActionResult GetAllPermissions()
        {
            // Sistemde tanımlı tüm statik izinleri döner (UI'da seçtirmek için)
            var permissions = typeof(Permissions).GetNestedTypes()
                .SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Select(f => f.GetValue(null).ToString())
                .ToList();

            return Ok(new ServiceResponse<List<string>>(permissions));
        }

        [HttpPost("update-permissions")]
        public async Task<IActionResult> UpdatePermissions(UpdateRolePermissionsCommand command)
        {
            // Token'dan yönetici ID'sini çekiyoruz
            command.AdminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // IP adresini yakalıyoruz
            command.IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
