using System;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AuthenticationService.DTOs;

namespace AuthenticationService.Services
{
    public class AuthService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> RegisterAsync(RegisterDTO registerDto)
        {
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var user = await FindByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(user.PasswordHash, loginDto.Password))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        public string HashPassword(string password)
        {
            // Użyj tutaj metody hashowania hasła, np. BCrypt
            return "zahashowane";// BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // Użyj metody weryfikacji hasła, np. BCrypt
            return true;// BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                    // Dodaj więcej Claimów według potrzeb
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
