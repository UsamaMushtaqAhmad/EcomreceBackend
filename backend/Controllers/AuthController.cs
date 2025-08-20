using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services.Users;
using backend.Services.Auth;
using backend.DTOs.User;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Name, Email aur Password required hain.");
            }

            var user = await _userService.RegisterAsync(dto);
            if (user is null)
                return Conflict("Email already exists.");

            var (token, exp) = _tokenService.GenerateToken(user);

            return Ok(new AuthResponseDTO
            {
                Message = "User registered successfully",
                Token = token,
                ExpiresAtUtc = exp,
                User = new { user.Id, user.Name, user.Email, user.Role }
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email aur Password required hain.");

            var user = await _userService.LoginAsync(dto);
            if (user is null)
                return Unauthorized("Invalid email or password.");

            var (token, exp) = _tokenService.GenerateToken(user);

            return Ok(new AuthResponseDTO
            {
                Message = "Login successful",
                Token = token,
                ExpiresAtUtc = exp,
                User = new { user.Id, user.Name, user.Email, user.Role }
            });
        }

        [Authorize] // token required
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var result = await _userService.LogoutAsync();
            if (!result) return BadRequest("Logout failed.");
            return Ok(new { message = "Logged out successfully (client should discard the token)" });
        }

        // Example: current user info (claims se)
        [Authorize]
        [HttpGet("me")]
        public ActionResult<object> Me()
        {
            var id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new { id, email, name, role });
        }
    }
}
