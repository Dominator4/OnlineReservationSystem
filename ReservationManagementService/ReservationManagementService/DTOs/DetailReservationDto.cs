using System;
using System.Collections.Generic;

namespace ReservationManagementService.DTOs
{
    /// <summary>
    /// DTO containing detailed information about a reservation.
    /// </summary>
    public class DetailReservationDto
    {
        /// <summary>
        /// List of rooms reserved.
        /// </summary>
        public IEnumerable<RoomDto> ReservedRooms { get; set; }

        /// <summary>
        /// Check-in date for the reservation.
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Check-out date for the reservation.
        /// </summary>
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Number of guests in the reservation.
        /// </summary>
        public int Guests { get; set; }
    }
}