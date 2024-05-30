using System;

namespace BookingClientApp.Models
{
    /// <summary>
    /// Represents a room available for booking.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Unique identifier for the room.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Room number.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Floor number where the room is located.
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// Type of the room (e.g., single, double, suite).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Comma-separated list of amenities in the room.
        /// </summary>
        public string Amenities { get; set; }

        /// <summary>
        /// Price per night for the room.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Indicates whether the room is currently available.
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
