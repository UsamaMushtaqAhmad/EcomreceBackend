using Dapper;
using backend.Dapper;
using backend.DTOs.User;
using backend.Models;
using BCrypt.Net;

namespace backend.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DapperContext _context;

        public UserService(DapperContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string q = "SELECT TOP 1 * FROM Users WHERE Email = @Email";
            using var con = _context.CreateConnection();
            return await con.QuerySingleOrDefaultAsync<User>(q, new { Email = email });
        }

        public async Task<User?> RegisterAsync(RegisterDTO dto)
        {
            // Email unique check
            var existing = await GetByEmailAsync(dto.Email);
            if (existing is not null) return null;

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            const string q = @"
INSERT INTO Users (Name, Email, PasswordHash, Role)
OUTPUT INSERTED.*
VALUES (@Name, @Email, @PasswordHash, @Role);";

            using var con = _context.CreateConnection();
            var user = await con.QuerySingleOrDefaultAsync<User>(q, new
            {
                dto.Name,
                dto.Email,
                PasswordHash = hash,
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "user" : dto.Role!.Trim().ToLower()
            });

            return user;
        }

        public async Task<User?> LoginAsync(LoginDTO dto)
        {
            var user = await GetByEmailAsync(dto.Email);
            if (user is null) return null;

            var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            return ok ? user : null;
        }

        public Task<bool> LogoutAsync()
        {
            // JWT stateless hota hai — server pe kuch store nahi.
            // Optional: maintain blacklist/refresh-tokens if needed.
            return Task.FromResult(true);
        }
    }
}
