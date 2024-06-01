using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookingClientApp.Models
{
    /// <summary>
    /// ViewModel representing user details.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// User's first name.
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain only letters.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        /// <summary>
        /// User's phone number.
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Phone number must be 9 digits.")]
        public string PhoneNumber { get; set; }
    }
}
