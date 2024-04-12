using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookingClientApp.Models;
using BookingClientApp.Services;
using System.Threading.Tasks;


//Obsługuje akcje logowania i rejestracji w `BookingClientApp`.
namespace BookingClientApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthServiceClient _authServiceClient;

        public AuthController(AuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
    return View(new LoginViewModel());
        }

[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    var token = await _authServiceClient.LoginAsync(model);
//    if (token != null)
if (!string.IsNullOrWhiteSpace(token))
    {
        // Ustawienie tokena JWT w cookies
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Nie dostępne dla JS
            Secure = true, // Wymuszaj HTTPS
            SameSite = SameSiteMode.Strict, // Ogranicz wysyłanie cookies do oryginalnej domeny
            Expires = DateTime.UtcNow.AddHours(1) // Ustaw ważność tokena
        };

        Response.Cookies.Append("AuthToken", token, cookieOptions);

        // Przekierowanie do innej akcji
        return RedirectToAction("UserProfile", "Reservation");
    }

    ModelState.AddModelError("", "Nieudana próba logowania");
    return View(model);
}

[HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await _authServiceClient.RegisterAsync(model);
            if (result != null)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Nieudana próba rejestracji");
            return View(model);
        }
    }
}
