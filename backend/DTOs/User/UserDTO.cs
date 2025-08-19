namespace backend.DTOs.User
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;   // Full name
        public string Email { get; set; } = string.Empty;  // Unique email
        public string Password { get; set; } = string.Empty;
    }
}
