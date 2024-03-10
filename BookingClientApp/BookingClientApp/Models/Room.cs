using System;

namespace BookingClientApp.Models
{
    public class Room
    {
        public int Id { get; set; } // Unique identifier for the room
        public string Number { get; set; } // Room number
        public string Type { get; set; } // Type of the room (e.g., single, double, suite)
        public decimal Price { get; set; } // Price per night
        public string Amenities { get; set; } // Comma-separated list of amenities
    }
}