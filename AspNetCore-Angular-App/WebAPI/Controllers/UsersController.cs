using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAPI.Services.Services.Users;

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