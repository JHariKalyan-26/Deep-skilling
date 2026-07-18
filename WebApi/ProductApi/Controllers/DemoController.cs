using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet("welcome")]
    public ActionResult<ApiResponse<string>> Welcome() =>
        Ok(new ApiResponse<string>(
            true,
            "Web API is working.",
            "Welcome to Cognizant Deep Skilling."));

    [HttpGet("error")]
    public IActionResult GenerateError() =>
        throw new InvalidOperationException(
            "Demo exception for middleware testing.");
}
