using System;

namespace BookingClientApp.Models
{
    /// <summary>
    /// ViewModel for user login credentials.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// User's email address for login.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password for login.
        /// </summary>
        public string Password { get; set; }
    }
}
