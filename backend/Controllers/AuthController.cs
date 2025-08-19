using Microsoft.AspNetCore.Mvc;
using backend.Services.Users;
using backend.DTOs.User;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDto)
        {
            if (string.IsNullOrEmpty(userDto.Name) ||
                string.IsNullOrEmpty(userDto.Email) ||
                string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Name, Email and Password are required.");
            }

            var user = await _userService.Register(userDto);
            if (user == null)
            {
                return BadRequest("Registration failed.");
            }

            return Ok(new
            {
                message = "User registered successfully",
                user
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDTO userDto)
        {
            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var user = await _userService.Login(userDto);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                message = "Login successful",
                user
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var result = await _userService.Logout();
            if (!result)
            {
                return BadRequest("Logout failed.");
            }

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
