using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Services.Dto;

namespace WebAPI.Services.Services.Users
{
    public interface IUserService
    {
        long CreateUser(UserDto userDto);

        UserDto Authenticate(string username, string password);

        UserDto GetUserById(long id);

        IList<UserDto> GetUsersList();

        void UpdateUser(UserDto userDto);

        void DeleteUser(long userId);

        #region Async methods

        Task<long> CreateUserAsync(UserDto userDto);

        Task DeleteUserAsync(long userId);

        Task<UserDto> GetUserByIdAsync(long userId);

        Task<IList<UserDto>> GetUsersListAsync();

        Task UpdateUserAsync(UserDto userDto);

        #endregion
    }
}
