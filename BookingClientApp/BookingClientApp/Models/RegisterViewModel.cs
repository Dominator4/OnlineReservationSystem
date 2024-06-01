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
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        /// <summary>
        /// User's password for registration.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, and one digit.")]
        public string Password { get; set; }

        /// <summary>
        /// Confirmation of the user's password.
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
