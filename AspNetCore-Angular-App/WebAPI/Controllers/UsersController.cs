using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DAL.Entities;
using WebAPI.DAL.Interfaces;
using WebAPI.Helpers;
using WebAPI.Models.Users;
using WebAPI.Services.Dto;
using WebAPI.Services.Services.Users;

namespace WebAPI.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly Func<IUserService> _userService;
        private readonly Func<IRepository<User>> _userRepository;
        private readonly AppSettings _appSettings;
        public UsersController(
            Func<IUserService> userService,
            Func<IRepository<User>> userRepository,
            IOptions<AppSettings> appSettings
            )
        {
            _userService = userService;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(AuthenticateModel model)
        {
            var user = _userService().Authenticate(model.Username, model.Password);

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
                //Id = user.UserId,
                //Username = user.UserName,
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

            _userService().CreateUser(user);
            return Ok();
        }

        [HttpGet("GetUser/{userId}")]
        public IActionResult GetUser(long userId)
        {
            var users = _userRepository()
                .GetDbSet()
                .ToList();

            var users2 = _userService()
                 .GetUsersList();


            //głupi przykład, ale przykład
            //PrivateMethod(users);

            return Json(new { });
        }

        private void PrivateMethod(List<User> users)
        {
            foreach (var user in users)
            {
                AnotherPrivateMethod(user);
            }
        }

        private void AnotherPrivateMethod(User user)
        {
            //logika już tak złożona, że zapominamy o naszej wcześniej podciągniętej liście userów z repozytorium

            //przykład redundacji zapytania
            var users2 = _userService()
                 .GetUsersList()
                 .Where(u => u.UserId == user.Id);
        }

        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> DeleteUser(long userId)
        {
            await _userService().DeleteUserAsync(userId);

            return Ok("Deleted");
        }
    }
}