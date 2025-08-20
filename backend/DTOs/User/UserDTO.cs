namespace backend.DTOs.User
{
    // Registration ke liye
    public class RegisterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // plain aayega, hash hum banayenge
        public string? Role { get; set; } // optional, default user
    }

    // Login ke liye
    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Auth response client ko
    public class AuthResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public object User { get; set; } = default!;
    }
}
