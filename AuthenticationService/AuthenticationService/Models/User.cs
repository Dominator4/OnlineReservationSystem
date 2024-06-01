using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    /// <summary>
    /// Represents a user in the authentication service.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// User's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Hashed password for the user.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }
    }
}
