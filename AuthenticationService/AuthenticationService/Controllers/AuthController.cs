using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Services;
using AuthenticationService.DTOs;
using AuthenticationService.Models;
// Dodatkowe dyrektywy using, jeśli są potrzebne

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        // Tu dodaj pole dla kontekstu bazy danych, jeśli używasz EF Core

        public AuthController(AuthService authService /*, DbContext dbContext */)
        {
            _authService = authService;
            // Tu przypisz kontekst bazy danych do pola klasy, jeśli używasz EF Core
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDto)
        {
            // Sprawdź, czy użytkownik o takim emailu już istnieje
            // Jeśli tak, zwróć odpowiedni komunikat błędu

            // Hashowanie hasła
            var hashedPassword = _authService.HashPassword(registerDto.Password);

            // Utworzenie nowego użytkownika
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = hashedPassword
            };

            // Zapisz użytkownika w bazie danych

            // Zwróć informację o sukcesie (np. kod 201 Created)
            return StatusCode(201);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            // Sprawdź, czy użytkownik o takim emailu istnieje
            var user = new User //to na chwile
            {
                Email = "adam@gmail.com",
                PasswordHash = "tojesthaslo"
            };
            // Sprawdź, czy hasło jest poprawne

            // Jeśli uwierzytelnienie się powiedzie, generuj token JWT
            var token = _authService.GenerateJwtToken( user );

            // Zwróć token JWT klientowi
            return Ok(new { Token = token });
        }

        // Możesz dodać więcej akcji, np. odnowienie tokena, wylogowanie itp.
    }
}
