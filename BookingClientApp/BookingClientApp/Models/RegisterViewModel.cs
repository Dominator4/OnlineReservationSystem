using System;

namespace BookingClientApp.Models
{
    /// <summary>
    /// ViewModel for registering a new user.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// User's email address for registration.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password for registration.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Confirmation of the user's password.
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
