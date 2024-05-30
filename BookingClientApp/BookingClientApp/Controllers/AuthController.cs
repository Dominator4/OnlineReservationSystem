using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookingClientApp.Models;
using BookingClientApp.Services;
using System.Threading.Tasks;

namespace BookingClientApp.Controllers
{
    /// <summary>
    /// Handles login and registration actions in the BookingClientApp.
    /// </summary>
    public class AuthController : Controller
    {
        private readonly AuthServiceClient _authServiceClient;

        /// <summary>
        /// Initializes a new instance of the AuthController class.
        /// </summary>
        /// <param name="authServiceClient">The authentication service client used for login and registration operations.</param>
        public AuthController(AuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient;
        }

        /// <summary>
        /// Displays the login view.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        /// <summary>
        /// Handles the login process.
        /// </summary>
        /// <param name="model">The login view model containing the user's email and password.</param>
        /// <returns>Redirects to the UserProfile action if successful; otherwise, returns the login view with an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = await _authServiceClient.LoginAsync(model);
            if (!string.IsNullOrWhiteSpace(token))
            {
                // Set the JWT token in cookies
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Not accessible by JS
                    Secure = true, // Enforce HTTPS
                    SameSite = SameSiteMode.Strict, // Restrict to same site
                    Expires = DateTime.UtcNow.AddHours(1) // Set token expiration
                };

                Response.Cookies.Append("AuthToken", token, cookieOptions);

                // Redirect to another action
                return RedirectToAction("UserProfile", "Reservation");
            }

            ModelState.AddModelError("", "Login attempt failed");
            return View(model);
        }

        /// <summary>
        /// Displays the registration view.
        /// </summary>
        /// <returns>The registration view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handles the registration process.
        /// </summary>
        /// <param name="model">The registration view model containing the user's email, password, and confirm password.</param>
        /// <returns>Redirects to the login action if successful; otherwise, returns the registration view with an error message.</returns>
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

            ModelState.AddModelError("", "Registration attempt failed");
            return View(model);
        }
    }
}
