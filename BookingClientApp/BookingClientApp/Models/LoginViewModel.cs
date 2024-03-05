using System;

//Przechowuje dane logowania podane przez użytkownika.
namespace BookingClientApp.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
