using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("Works")]
        public IActionResult Works()
        {
            return Ok("Works!");
        }
    }
}