using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Services.Dto
{
    public class UserDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
