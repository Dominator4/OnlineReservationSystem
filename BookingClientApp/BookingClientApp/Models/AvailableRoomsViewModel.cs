using System;
using System.Collections.Generic;

namespace BookingClientApp.Models
{
    /// <summary>
    /// ViewModel representing available rooms for a given date range.
    /// </summary>
    public class AvailableRoomsViewModel
    {
        /// <summary>
        /// List of available rooms.
        /// </summary>
        public IEnumerable<Room> AvailableRooms { get; set; }

        /// <summary>
        /// Check-in date for the search.
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Check-out date for the search.
        /// </summary>
        public DateTime CheckOutDate { get; set; }
    }
}
