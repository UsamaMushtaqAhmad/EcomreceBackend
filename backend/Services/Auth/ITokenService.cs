using backend.Models;

namespace backend.Services.Auth
{
    public interface ITokenService
    {
        (string token, DateTime expiresUtc) GenerateToken(User user);
    }
}
