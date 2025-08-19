using Dapper;
using backend.Dapper;
using backend.DTOs.User;
using backend.Models;

namespace backend.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DapperContext _context;

        public UserService(DapperContext context)
        {
            _context = context;
        }

        // =============== REGISTER ===============
        public async Task<User?> Register(UserDTO userDto)
        {
            var query = @"INSERT INTO Users (Name, Email, Password, Role) 
                          OUTPUT INSERTED.* 
                          VALUES (@Name, @Email, @Password, 'user')";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(
                query,
                new { userDto.Name, userDto.Email, userDto.Password }
            );
        }

        // =============== LOGIN ===============
        public async Task<User?> Login(UserDTO userDto)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(
                query,
                new { userDto.Email, userDto.Password }
            );
        }

        // =============== LOGOUT ===============
        public Task<bool> Logout()
        {
            
            return Task.FromResult(true);
        }
    }
}
