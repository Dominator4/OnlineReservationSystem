using Microsoft.AspNetCore.Mvc;
using BookingClientApp.Models;
using BookingClientApp.Services;
using System.Threading.Tasks;
// Dodatkowe dyrektywy using

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
                        if (token != null)
    {
                // Przechowaj token w sesji/cookie itp.
                // Przykład: HttpContext.Session.SetString("AuthToken", token);

                // Przekierowanie do akcji CheckAvailability kontrolera Reservation
                //        return RedirectToAction("CheckAvailability", "Reservation");
                return View("UserProfile");
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
