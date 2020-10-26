using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DAL.Entities;
using WebAPI.DAL.Interfaces;
using WebAPI.Services.Dto;

namespace WebAPI.Services.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        public UserService(
            IRepository<User> repository)
        {
            _repository = repository;
        }

        public long CreateUser(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new Exception("Password is required");

            if (_repository.GetDbSet().Any(u => u.UserName == userDto.UserName))
                throw new Exception("Username \"" + userDto.UserName + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            var user = new User
            {
                UserName = userDto.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            return _repository.Add(user);
        }

        public async Task<long> CreateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName
            };

            return await _repository.AddAsync(user);
        }

        public UserDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _repository.GetDbSet().SingleOrDefault(x => x.UserName == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            UserDto userDto = new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            return userDto;
        }

        public void DeleteUser(long userId)
        {
            var user = _repository.GetById(userId);
            _repository.Delete(user);
        }

        public async Task DeleteUserAsync(long userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            await _repository.DeleteAsync(user);
        }

        public UserDto GetUserById(long userId)
        {
            var user = _repository.GetById(userId);
            var result = new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            return result;
        }

        public async Task<UserDto> GetUserByIdAsync(long userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            var result = new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            return result;
        }

        public IList<UserDto> GetUsersList()
        {
            var result = new List<UserDto>();

            foreach (var user in _repository.GetAll())
            {
                var userDto = new UserDto
                {
                    UserName = user.UserName
                };

                result.Add(userDto);
            }

            return result;
        }


        public async Task<IList<UserDto>> GetUsersListAsync()
        {
            var result = new List<UserDto>();

            foreach (var user in await _repository.GetAllAsync())
            {
                var userDto = new UserDto
                {
                    UserName = user.UserName
                };

                result.Add(userDto);
            }

            return result;
        }

        public void UpdateUser(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName
            };

            _repository.Update(user);
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Id = userDto.UserId,
                UserName = userDto.UserName
            };

            await _repository.UpdateAsync(user);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
