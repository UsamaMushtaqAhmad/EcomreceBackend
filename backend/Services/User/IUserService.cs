using backend.DTOs.User;
using backend.Models;

namespace backend.Services.Users
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(RegisterDTO userDto);
        Task<User?> LoginAsync(LoginDTO userDto);
        Task<bool> LogoutAsync(); // stateless demo
        Task<User?> GetByEmailAsync(string email);
    }
}
