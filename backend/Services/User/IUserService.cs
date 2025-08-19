using backend.DTOs.User;
using backend.Models;

namespace backend.Services.Users
{
    public interface IUserService
    {
        Task<User?> Register(UserDTO userDto);
        Task<User?> Login(UserDTO userDto);
        Task<bool> Logout();  // logout ka method
    }
}
