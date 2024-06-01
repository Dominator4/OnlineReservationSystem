using System;

namespace ReservationManagementService.Models
{
    /// <summary>
    /// Model for requesting room availability between specific dates.
    /// </summary>
    public class AvailabilityRequest
    {
        /// <summary>
        /// Start date for checking availability.
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// End date for checking availability.
        /// </summary>
        public DateTime CheckOutDate { get; set; }
    }
}
