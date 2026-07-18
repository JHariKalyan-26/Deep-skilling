using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("dashboard")]
    public IActionResult GetAdminDashboard()
    {
        return Ok(new
        {
            message = "Welcome to the admin dashboard.",
            currentUser = User.Identity?.Name
        });
    }
}
