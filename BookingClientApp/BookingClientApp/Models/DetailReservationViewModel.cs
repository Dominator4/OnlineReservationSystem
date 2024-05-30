using System;
using System.Collections.Generic;

namespace BookingClientApp.Models
{
    /// <summary>
    /// ViewModel representing details of a reservation.
    /// </summary>
    public class DetailReservationViewModel
    {
        /// <summary>
        /// List of rooms reserved.
        /// </summary>
        public IEnumerable<Room> ReservedRooms { get; set; }

        /// <summary>
        /// Check-in date for the reservation.
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Check-out date for the reservation.
        /// </summary>
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Number of guests for the reservation.
        /// </summary>
        public int Guests { get; set; }
    }
}
