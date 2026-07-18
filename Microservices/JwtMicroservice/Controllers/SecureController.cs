using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    [Authorize]
    [HttpGet("data")]
    public IActionResult GetSecureData()
    {
        return Ok(new
        {
            message = "This is protected data.",
            currentUser = User.Identity?.Name
        });
    }
}
