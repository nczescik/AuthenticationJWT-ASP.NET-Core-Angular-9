using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Models.Users;
using WebAPI.Services.Dto;
using WebAPI.Services.Services.Users;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Func<IUserService> _userService;
        private readonly AppSettings _appSettings;
        public UsersController(
            Func<IUserService> userService,
            IOptions<AppSettings> appSettings
            )
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(AuthenticateModel model)
        {
            var user = _userService().Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);


            return Ok(new
            {
                Id = user.UserId,
                Username = user.UserName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(RegisterModel model)
        {
            var user = new UserDto
            {
                UserName = model.Username,
                Password = model.Password
            };

            try
            {
                _userService().CreateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetUser/{userId}")]
        public async Task<IActionResult> GetUser(long userId)
        {
            var user = await _userService().GetUserByIdAsync(userId);

            return Ok(user);
        }

        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> DeleteUser(long userId)
        {
            await _userService().DeleteUserAsync(userId);

            return Ok("Deleted");
        }
    }
}