using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User's password for registration.
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        /// <summary>
        /// Confirmation of the user's password.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
