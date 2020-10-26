using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.DAL.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}