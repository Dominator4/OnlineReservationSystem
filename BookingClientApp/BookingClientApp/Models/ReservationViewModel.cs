using System;
using System.Collections.Generic;

namespace BookingClientApp.Models
{
    /// <summary>
    /// ViewModel for creating or displaying a reservation.
    /// </summary>
    public class ReservationViewModel
    {
        /// <summary>
        /// Unique identifier for the reservation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// List of selected room IDs for the reservation.
        /// </summary>
        public List<int> SelectedRoomIds { get; set; } = new List<int>();

        /// <summary>
        /// Customer ID making the reservation.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Check-in date for the reservation.
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Check-out date for the reservation.
        /// </summary>
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Status of the reservation (e.g., confirmed, pending, canceled).
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Number of guests included in the reservation.
        /// </summary>
        public int Guests { get; set; }
    }
}
