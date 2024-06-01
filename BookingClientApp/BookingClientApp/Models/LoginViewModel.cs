using System;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        /// <summary>
        /// User's password for login.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
    }
}
