using System;

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
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's phone number.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
