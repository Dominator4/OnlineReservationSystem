namespace AuthenticationService.DTOs
{
    /// <summary>
    /// Data Transfer Object for user registration. Contains the necessary information to register a new user.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// User's email address used for registration.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password used for registration.
        /// </summary>
        public string Password { get; set; }
    }
}
