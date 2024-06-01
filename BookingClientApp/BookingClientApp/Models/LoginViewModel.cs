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
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User's password for login.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
