using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Services;

using AuthenticationService.DTOs;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            // Sprawdź, czy użytkownik o takim emailu już istnieje
            var existingUser = await _authService.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }

            // Utworzenie nowego użytkownika
            var user = await _authService.RegisterAsync(registerDto);
            if (user == null)
            {
                return BadRequest("Failed to create new user.");
            }

            // Zwróć informację o sukcesie (np. kod 201 Created)
            return StatusCode(201, new { UserId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            return Ok(new { Token = "to jest jakis token" }); //tylko dla testu
            var token = await _authService.LoginAsync(loginDto);
            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Zwróć token JWT klientowi
            return Ok(new { Token = token });
        }

    [HttpGet]
        public String Get()
{
return "ciasto";
}

        // Możesz dodać więcej akcji, np. odnowienie tokena, wylogowanie itp.
    }
}
