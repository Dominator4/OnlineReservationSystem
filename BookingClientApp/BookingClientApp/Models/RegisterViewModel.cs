using System;

//Przechowuje dane potrzebne do rejestracji nowego użytkownika.
namespace BookingClientApp.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
