using AuthServer.Identity.Application.Features.Management.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Identity.API.Controllers
{
    [Authorize(Roles = "SuperAdmin")] // Sadece SuperAdmin girebilir
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserManagementController(IMediator mediator) => _mediator = mediator;

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetUsersWithRolesQuery());
            return Ok(response);
        }
    }
}