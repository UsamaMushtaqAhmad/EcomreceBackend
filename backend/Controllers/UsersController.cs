using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // koi bhi authenticated
    public class UsersController : ControllerBase
    {
        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            return Ok(new { message = "This is protected data" });
        }
    }
}
