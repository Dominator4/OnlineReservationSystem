namespace ReservationManagementService.DTOs
{
    /// <summary>
    /// DTO representing a user in the reservation system.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}