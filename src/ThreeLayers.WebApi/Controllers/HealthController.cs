using Microsoft.AspNetCore.Mvc;

namespace ThreeLayers.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> Get()
    {
        return Ok(new 
        { 
            status = "healthy", 
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            message = "API with comprehensive error handling is running successfully"
        });
    }
}