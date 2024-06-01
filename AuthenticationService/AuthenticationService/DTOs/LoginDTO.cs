namespace AuthenticationService.DTOs
{
    /// <summary>
    /// Data Transfer Object for user login. Contains the necessary information for user authentication.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// User's email address used for login.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password used for login.
        /// </summary>
        public string Password { get; set; }
    }
}
