using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Services;
using AuthenticationService.DTOs;

namespace AuthenticationService.Controllers
{
    /// <summary>
    /// Controller handling authentication operations such as login, registration, and logout.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        /// <summary>
        /// Initializes a new instance of the AuthController class.
        /// </summary>
        /// <param name="authService">The authentication service to be used by this controller.</param>
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="registerDto">The registration details including email and password.</param>
        /// <returns>Returns status code 201 if the user is successfully created; otherwise, returns a BadRequest with an error message.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var existingUser = await _authService.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }

            var user = await _authService.RegisterAsync(registerDto);
            if (user == null)
            {
                return BadRequest("Failed to create new user.");
            }

            return StatusCode(201, new { UserId = user.Id });
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token if the login is successful.
        /// </summary>
        /// <param name="loginDto">The login details including email and password.</param>
        /// <returns>Returns an Ok result with a JWT token if authentication is successful; otherwise, returns Unauthorized with an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <returns>Returns an Ok result with a logout success message.</returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "User logged out successfully. Please delete your token." });
        }

        /// <summary>
        /// A simple method to check if the AuthenticationService is running.
        /// </summary>
        /// <returns>A string indicating that the service is started.</returns>
        [HttpGet]
        public String Get()
        {
            return "AuthenticationService started";
        }
    }
}
